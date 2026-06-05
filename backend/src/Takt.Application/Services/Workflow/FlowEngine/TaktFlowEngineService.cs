// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowEngineService.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时（发起、审批、待办、加签、挂起等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Workflow;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Models;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 流程引擎服务
/// </summary>
public class TaktFlowEngineService : TaktServiceBase, ITaktFlowEngineService
{
    private readonly ITaktCompanyRepository<TaktFlowInstance> _flowInstanceRepository;
    private readonly ITaktCompanyRepository<TaktFlowScheme> _flowSchemeRepository;
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktCompanyRepository<TaktFlowTransition> _flowTransitionRepository;
    private readonly ITaktCompanyRepository<TaktFlowAddSign> _flowAddSignRepository;
    private readonly TaktFlowApproverResolverService _approverResolver;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktFlowEngineService(
        ITaktCompanyRepository<TaktFlowInstance> flowInstanceRepository,
        ITaktCompanyRepository<TaktFlowScheme> flowSchemeRepository,
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktCompanyRepository<TaktFlowTransition> flowTransitionRepository,
        ITaktCompanyRepository<TaktFlowAddSign> flowAddSignRepository,
        TaktFlowApproverResolverService approverResolver,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowInstanceRepository = flowInstanceRepository;
        _flowSchemeRepository = flowSchemeRepository;
        _flowTaskRepository = flowTaskRepository;
        _flowTransitionRepository = flowTransitionRepository;
        _flowAddSignRepository = flowAddSignRepository;
        _approverResolver = approverResolver;
    }

    /// <inheritdoc />
    public async Task<TaktFlowInstanceDetailDto> StartFlowInstanceAsync(TaktFlowStartDto dto)
    {
        EnsureThreeLayerContext();
        var scheme = await LoadPublishedSchemeAsync(dto.ProcessKey);
        var userId = GetCurrentUserIdForApproval();
        var userName = CurrentUserName;
        var instance = await CreateInstanceEntityAsync(scheme, dto, userId, userName, TaktFlowInstanceStatus.Running);
        await RecordTransitionAsync(instance, null, null, "发起", userId, userName, TaktFlowActionType.Start, null);
        await AdvanceInstanceAsync(instance, null);
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <inheritdoc />
    public async Task<TaktFlowInstanceDetailDto> CreateFlowInstanceDraftAsync(TaktFlowStartDto dto)
    {
        EnsureThreeLayerContext();
        var scheme = await LoadPublishedSchemeAsync(dto.ProcessKey);
        var userId = GetCurrentUserIdForApproval();
        var instance = await CreateInstanceEntityAsync(scheme, dto, userId, CurrentUserName, TaktFlowInstanceStatus.Draft);
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <inheritdoc />
    public async Task<TaktFlowInstanceDetailDto> StartFlowInstanceFromDraftAsync(long instanceId)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(instanceId);
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Draft)
        {
            ThrowBusinessException("仅草稿状态可提交启动");
        }
        if (instance.StartUserId != GetCurrentUserIdForApproval())
        {
            ThrowBusinessException("仅发起人可提交草稿");
        }
        instance.InstanceStatus = TaktFlowInstanceStatus.Running;
        instance.StartTime ??= DateTime.Now;
        await _flowInstanceRepository.UpdateAsync(instance);
        var userId = GetCurrentUserIdForApproval();
        await RecordTransitionAsync(instance, null, null, "发起", userId, CurrentUserName, TaktFlowActionType.Start, null);
        await AdvanceInstanceAsync(instance, null);
        return (await GetFlowInstanceDetailByIdAsync(instance.Id))!;
    }

    /// <inheritdoc />
    public async Task CompleteFlowInstanceTaskAsync(TaktFlowCompleteTaskDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        if (!string.IsNullOrWhiteSpace(dto.FrmData))
        {
            instance.FrmData = dto.FrmData;
            await _flowInstanceRepository.UpdateAsync(instance);
        }
        var pendingTask = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == TaktFlowTaskStatus.Pending);
        if (pendingTask == null)
        {
            ThrowBusinessException("当前用户无待办任务");
        }
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root == null)
        {
            ThrowBusinessException("流程设计快照无效");
        }
        var currentNode = TaktFlowProcessNavigator.FindNode(root, pendingTask.TaskDefinitionKey)
            ?? TaktFlowProcessNavigator.FindNode(root, instance.CurrentActivityId);
        if (!dto.Approved)
        {
            await HandleRejectAsync(instance, pendingTask, currentNode, dto, userId);
            return;
        }
        pendingTask.TaskStatus = TaktFlowTaskStatus.Completed;
        pendingTask.CompletedAt = DateTime.Now;
        pendingTask.Comment = dto.Comment;
        await _flowTaskRepository.UpdateAsync(pendingTask);
        if (pendingTask.IsAddSign == 1 && pendingTask.AddSignId.HasValue)
        {
            var addHandled = await TryCompleteAddSignGroupAsync(instance, pendingTask);
            if (!addHandled)
            {
                return;
            }
        }
        if (currentNode != null && !await IsNodeApprovalCompleteAsync(instance, currentNode, pendingTask))
        {
            return;
        }
        if (currentNode != null)
        {
            var parentParallel = TaktFlowProcessNavigator.FindParentParallelGateway(root, currentNode.NodeId);
            if (parentParallel != null)
            {
                var branchIds = TaktFlowProcessNavigator.CollectParallelApproverNodes(parentParallel)
                    .Select(x => x.NodeId)
                    .ToHashSet(StringComparer.Ordinal);
                var otherPending = await _flowTaskRepository.FirstAsync(x =>
                    x.InstanceId == instance.Id
                    && x.TaskStatus == TaktFlowTaskStatus.Pending
                    && x.IsAddSign == 0
                    && branchIds.Contains(x.TaskDefinitionKey)
                    && x.Id != pendingTask.Id);
                if (otherPending != null)
                {
                    return;
                }
            }
        }
        var fromName = instance.CurrentActivityName ?? pendingTask.TaskName;
        await RecordTransitionAsync(
            instance,
            instance.CurrentActivityId,
            fromName,
            dto.Comment,
            userId,
            CurrentUserName,
            TaktFlowActionType.Approve,
            null);
        if (currentNode == null)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            return;
        }
        var nextNodes = TaktFlowProcessNavigator.ResolveAfterNodeCompleted(root, currentNode, instance.FrmData);
        if (nextNodes.Count == 0)
        {
            if (!await HasOtherPendingTasksAsync(instance.Id))
            {
                await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            }
            return;
        }
        await EnterExecutionNodesAsync(instance, root, nextNodes);
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktFlowTodoItemDto>> GetFlowInstanceTodoListAsync(TaktFlowTodoQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var tasks = await _flowTaskRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == TaktFlowTaskStatus.Pending);
        var instanceIds = tasks.Select(x => x.InstanceId).Distinct().ToList();
        if (instanceIds.Count == 0)
        {
            return TaktPagedResult<TaktFlowTodoItemDto>.Create(new List<TaktFlowTodoItemDto>(), 0, query.PageIndex, query.PageSize);
        }
        var instances = await _flowInstanceRepository.GetListAsync(x => instanceIds.Contains(x.Id));
        var instanceMap = instances.ToDictionary(x => x.Id);
        var items = new List<TaktFlowTodoItemDto>();
        foreach (var task in tasks.OrderByDescending(x => x.CreatedAt))
        {
            if (!instanceMap.TryGetValue(task.InstanceId, out var inst))
            {
                continue;
            }
            if (inst.InstanceStatus != TaktFlowInstanceStatus.Running)
            {
                continue;
            }
            if (!MatchesTodoQuery(inst, task, query))
            {
                continue;
            }
            items.Add(new TaktFlowTodoItemDto
            {
                FlowInstanceId = inst.Id,
                InstanceCode = inst.InstanceCode,
                ProcessName = inst.ProcessName,
                ProcessTitle = inst.ProcessTitle,
                TaskName = task.TaskName ?? inst.CurrentActivityName,
                StartUserName = inst.StartUserName,
                StartTime = inst.StartTime,
                FlowTaskId = task.Id
            });
        }
        var page = items
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        return TaktPagedResult<TaktFlowTodoItemDto>.Create(page, items.Count, query.PageIndex, query.PageSize);
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceMyListAsync(TaktFlowMyInstanceQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var exp = Expressionable.Create<TaktFlowInstance>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.StartUserId == userId);
        exp = ApplyFlowInstanceListQuery(exp, query);
        var (data, total) = await _flowInstanceRepository.GetPagedAsync(query.PageIndex, query.PageSize, exp.ToExpression());
        var list = data.Select(MapListItem).ToList();
        return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(list, total, query.PageIndex, query.PageSize);
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktFlowInstanceListItemDto>> GetFlowInstanceProcessedListAsync(TaktFlowTodoQueryDto query)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var doneTasks = await _flowTaskRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.AssigneeUserId == userId
            && x.TaskStatus == TaktFlowTaskStatus.Completed);
        var instanceIds = doneTasks.Select(x => x.InstanceId).Distinct().ToList();
        if (instanceIds.Count == 0)
        {
            return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(new List<TaktFlowInstanceListItemDto>(), 0, query.PageIndex, query.PageSize);
        }
        var instances = await _flowInstanceRepository.GetListAsync(x => instanceIds.Contains(x.Id));
        var items = instances
            .Where(x => MatchesTodoQuery(x, null, query))
            .OrderByDescending(x => x.UpdatedAt)
            .Select(MapListItem)
            .ToList();
        var page = items.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToList();
        return TaktPagedResult<TaktFlowInstanceListItemDto>.Create(page, items.Count, query.PageIndex, query.PageSize);
    }

    /// <inheritdoc />
    public async Task<TaktFlowInstanceDetailDto?> GetFlowInstanceDetailByIdAsync(long instanceId)
    {
        EnsureThreeLayerContext();
        var instance = await _flowInstanceRepository.GetByIdAsync(instanceId);
        if (instance == null || instance.TenantCode != CurrentTenantCode || instance.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await MapDetailAsync(instance);
    }

    /// <inheritdoc />
    public async Task RevokeFlowInstanceAsync(string instanceCode)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await _flowInstanceRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.InstanceCode == instanceCode);
        if (instance == null)
        {
            ThrowBusinessException("流程实例不存在");
        }
        if (instance.StartUserId != userId)
        {
            ThrowBusinessException("仅发起人可撤回");
        }
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可撤回");
        }
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = TaktFlowInstanceStatus.Terminated;
        instance.DeleteReason = "发起人撤回";
        instance.EndTime = DateTime.Now;
        if (instance.StartTime.HasValue)
        {
            instance.DurationMs = (long)(instance.EndTime.Value - instance.StartTime.Value).TotalMilliseconds;
        }
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, "撤回", userId, CurrentUserName, TaktFlowActionType.Revoke, null);
    }

    /// <inheritdoc />
    public async Task TransferFlowInstanceAsync(TaktFlowTransferDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        var task = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == TaktFlowTaskStatus.Pending);
        if (task == null)
        {
            ThrowBusinessException("当前用户无待办任务");
        }
        task.OwnerUserId = userId;
        task.AssigneeUserId = dto.ToUserId;
        task.AssigneeUserName = dto.ToUserName;
        await _flowTaskRepository.UpdateAsync(task);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Comment, userId, CurrentUserName, TaktFlowActionType.Transfer, dto.ToUserName);
    }

    /// <inheritdoc />
    public async Task AddFlowInstanceApproversAsync(TaktFlowAddApproversDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        EnsureInstanceRunnable(instance);
        var nodeId = instance.CurrentActivityId ?? string.Empty;
        var signType = string.IsNullOrWhiteSpace(dto.ApproveType) ? "sequential" : dto.ApproveType.Trim().ToLowerInvariant();
        var sort = 0;
        foreach (var approver in dto.Approvers)
        {
            var addSign = new TaktFlowAddSign
            {
                InstanceId = instance.Id,
                NodeId = nodeId,
                SignUserId = approver.ApproverUserId,
                SignUserName = approver.ApproverUserName,
                SignType = signType,
                ReturnToSignNode = dto.ReturnToSignNode ? 1 : 0,
                Reason = dto.Reason,
                IsHandled = 0
            };
            addSign = await _flowAddSignRepository.CreateAsync(addSign);
            var isPending = signType == "all" || signType == "one" || sort == 0;
            var task = new TaktFlowTask
            {
                InstanceId = instance.Id,
                TaskDefinitionKey = nodeId,
                TaskName = instance.CurrentActivityName,
                AssigneeUserId = approver.ApproverUserId,
                AssigneeUserName = approver.ApproverUserName,
                TaskStatus = isPending ? TaktFlowTaskStatus.Pending : TaktFlowTaskStatus.Cancelled,
                SignType = signType == "all" ? TaktFlowSignType.All : TaktFlowSignType.Any,
                IsAddSign = 1,
                AddSignId = addSign.Id,
                SortOrder = sort++
            };
            if (!isPending)
            {
                task.TaskStatus = TaktFlowTaskStatus.Cancelled;
            }
            await _flowTaskRepository.CreateAsync(task);
        }
        await RecordTransitionAsync(instance, nodeId, instance.CurrentActivityName, dto.Reason, userId, CurrentUserName, TaktFlowActionType.AddSign, null);
    }

    /// <inheritdoc />
    public async Task ReduceFlowInstanceApprovalAsync(TaktFlowReduceApprovalDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        var addSign = await _flowAddSignRepository.GetByIdAsync(dto.FlowAddSignId);
        if (addSign == null || addSign.InstanceId != instance.Id)
        {
            ThrowBusinessException("加签记录不存在");
        }
        addSign.IsHandled = 1;
        await _flowAddSignRepository.UpdateAsync(addSign);
        var tasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.AddSignId == addSign.Id);
        foreach (var task in tasks)
        {
            if (task.TaskStatus == TaktFlowTaskStatus.Pending)
            {
                task.TaskStatus = TaktFlowTaskStatus.Cancelled;
                await _flowTaskRepository.UpdateAsync(task);
            }
        }
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, "减签", userId, CurrentUserName, TaktFlowActionType.ReduceSign, addSign.SignUserName);
    }

    /// <inheritdoc />
    public async Task SuspendFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可挂起");
        }
        instance.InstanceStatus = TaktFlowInstanceStatus.Suspended;
        instance.DeleteReason = dto.Reason;
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Suspend, null);
    }

    /// <inheritdoc />
    public async Task ResumeFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Suspended)
        {
            ThrowBusinessException("仅已挂起流程可恢复");
        }
        instance.InstanceStatus = TaktFlowInstanceStatus.Running;
        instance.DeleteReason = null;
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Resume, null);
    }

    /// <inheritdoc />
    public async Task TerminateFlowInstanceAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = TaktFlowInstanceStatus.Terminated;
        instance.DeleteReason = dto.Reason;
        instance.EndTime = DateTime.Now;
        if (instance.StartTime.HasValue)
        {
            instance.DurationMs = (long)(instance.EndTime.Value - instance.StartTime.Value).TotalMilliseconds;
        }
        await _flowInstanceRepository.UpdateAsync(instance);
        await RecordTransitionAsync(instance, instance.CurrentActivityId, instance.CurrentActivityName, dto.Reason, GetCurrentUserIdForApproval(), CurrentUserName, TaktFlowActionType.Terminate, null);
    }

    /// <inheritdoc />
    public async Task UndoFlowInstanceVerificationAsync(TaktFlowInstanceOperateDto dto)
    {
        EnsureThreeLayerContext();
        var userId = GetCurrentUserIdForApproval();
        var instance = await LoadInstanceOrThrowAsync(dto.FlowInstanceId);
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Running)
        {
            ThrowBusinessException("仅运行中流程可撤销审批");
        }
        var lastTask = await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId
            && x.TaskStatus == TaktFlowTaskStatus.Completed
            && x.IsAddSign == 0);
        if (lastTask == null)
        {
            ThrowBusinessException("未找到可撤销的已办任务");
        }
        lastTask.TaskStatus = TaktFlowTaskStatus.Pending;
        lastTask.CompletedAt = null;
        await _flowTaskRepository.UpdateAsync(lastTask);
        instance.CurrentActivityId = lastTask.TaskDefinitionKey;
        instance.CurrentActivityName = lastTask.TaskName;
        await _flowInstanceRepository.UpdateAsync(instance);
        var transitions = await _flowTransitionRepository.GetListAsync(
            x => x.InstanceId == instance.Id,
            x => x.TransitionTime,
            true);
        var lastTransition = transitions.FirstOrDefault(x => x.ActionType == TaktFlowActionType.Approve && x.TransitionUserId == userId);
        if (lastTransition != null)
        {
            await _flowTransitionRepository.DeleteAsync(lastTransition.Id);
        }
    }

    /// <summary>
    /// 推进流程到下一执行节点
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="fromNodeId">来源节点</param>
    /// <returns>任务</returns>
    private async Task AdvanceInstanceAsync(TaktFlowInstance instance, string? fromNodeId)
    {
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root == null)
        {
            ThrowBusinessException("流程设计快照无效");
        }
        var nextNodes = TaktFlowProcessNavigator.ResolveNextExecutionNodes(root, fromNodeId, instance.FrmData);
        if (nextNodes.Count == 0)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            return;
        }
        await EnterExecutionNodesAsync(instance, root, nextNodes);
    }

    /// <summary>
    /// 进入执行节点并创建待办
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="root">流程根</param>
    /// <param name="nodes">节点列表</param>
    /// <returns>任务</returns>
    private async Task EnterExecutionNodesAsync(TaktFlowInstance instance, TaktFlowTreeNode root, List<TaktFlowTreeNode> nodes)
    {
        if (nodes.Count == 0)
        {
            await CompleteInstanceAsync(instance, TaktFlowInstanceStatus.Completed);
            return;
        }
        if (nodes.Count == 1)
        {
            await EnterApproverNodeAsync(instance, nodes[0]);
            return;
        }
        var parallelGateway = TaktFlowProcessNavigator.FindParentParallelGateway(root, nodes[0].NodeId);
        instance.CurrentActivityId = parallelGateway?.NodeId ?? nodes[0].NodeId;
        instance.CurrentActivityName = parallelGateway != null
            ? TaktFlowProcessNavigator.GetNodeDisplayName(parallelGateway)
            : TaktFlowProcessNavigator.GetNodeDisplayName(nodes[0]);
        await _flowInstanceRepository.UpdateAsync(instance);
        foreach (var node in nodes)
        {
            await CreateTasksForApproverNodeAsync(instance, node, activateAll: true);
        }
    }

    /// <summary>
    /// 进入单个审批节点
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">审批节点</param>
    /// <returns>任务</returns>
    private async Task EnterApproverNodeAsync(TaktFlowInstance instance, TaktFlowTreeNode node)
    {
        instance.CurrentActivityId = node.NodeId;
        instance.CurrentActivityName = TaktFlowProcessNavigator.GetNodeDisplayName(node);
        await _flowInstanceRepository.UpdateAsync(instance);
        var setType = node.SetType ?? 1;
        var sequentialChain = setType == 6;
        await CreateTasksForApproverNodeAsync(instance, node, activateAll: !sequentialChain, onlyFirstForChain: sequentialChain);
    }

    /// <summary>
    /// 为审批节点创建任务
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">节点</param>
    /// <param name="activateAll">是否全部激活</param>
    /// <param name="onlyFirstForChain">层层审批仅激活首个</param>
    /// <returns>任务</returns>
    private async Task CreateTasksForApproverNodeAsync(
        TaktFlowInstance instance,
        TaktFlowTreeNode node,
        bool activateAll,
        bool onlyFirstForChain = false)
    {
        var approvers = await _approverResolver.ResolveApproversAsync(
            node,
            instance.StartUserId,
            instance.TenantCode,
            instance.CompanyCode);
        if (approvers.Count == 0)
        {
            ThrowBusinessException($"节点「{TaktFlowProcessNavigator.GetNodeDisplayName(node)}」未解析到审批人");
        }
        var signType = node.SignType == 2 ? TaktFlowSignType.All : TaktFlowSignType.Any;
        var sort = 0;
        foreach (var approver in approvers)
        {
            var active = activateAll || (onlyFirstForChain && sort == 0);
            var task = new TaktFlowTask
            {
                InstanceId = instance.Id,
                TaskDefinitionKey = node.NodeId,
                TaskName = TaktFlowProcessNavigator.GetNodeDisplayName(node),
                AssigneeUserId = approver.UserId,
                AssigneeUserName = approver.UserName,
                TaskStatus = active ? TaktFlowTaskStatus.Pending : TaktFlowTaskStatus.Cancelled,
                SignType = signType,
                SortOrder = sort++
            };
            await _flowTaskRepository.CreateAsync(task);
        }
    }

    /// <summary>
    /// 判断节点审批是否已全部完成
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="node">节点</param>
    /// <param name="completedTask">刚完成的任务</param>
    /// <returns>是否可流转</returns>
    private async Task<bool> IsNodeApprovalCompleteAsync(TaktFlowInstance instance, TaktFlowTreeNode node, TaktFlowTask completedTask)
    {
        var nodeTasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id
            && x.TaskDefinitionKey == node.NodeId
            && x.IsAddSign == 0);
        var pending = nodeTasks.Where(x => x.TaskStatus == TaktFlowTaskStatus.Pending).ToList();
        if (node.SignType == 2)
        {
            return pending.Count == 0;
        }
        foreach (var other in pending)
        {
            other.TaskStatus = TaktFlowTaskStatus.Cancelled;
            await _flowTaskRepository.UpdateAsync(other);
        }
        return true;
    }

    /// <summary>
    /// 处理加签组完成
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="task">任务</param>
    /// <returns>是否继续主流程</returns>
    private async Task<bool> TryCompleteAddSignGroupAsync(TaktFlowInstance instance, TaktFlowTask task)
    {
        var addSign = await _flowAddSignRepository.GetByIdAsync(task.AddSignId!.Value);
        if (addSign == null)
        {
            return true;
        }
        var groupTasks = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.AddSignId == addSign.Id);
        var signType = addSign.SignType;
        if (signType == "one")
        {
            foreach (var other in groupTasks.Where(x => x.Id != task.Id && x.TaskStatus == TaktFlowTaskStatus.Pending))
            {
                other.TaskStatus = TaktFlowTaskStatus.Cancelled;
                await _flowTaskRepository.UpdateAsync(other);
            }
            addSign.IsHandled = 1;
            await _flowAddSignRepository.UpdateAsync(addSign);
            return addSign.ReturnToSignNode != 1;
        }
        if (signType == "all")
        {
            if (groupTasks.Any(x => x.TaskStatus == TaktFlowTaskStatus.Pending))
            {
                return false;
            }
            addSign.IsHandled = 1;
            await _flowAddSignRepository.UpdateAsync(addSign);
            return addSign.ReturnToSignNode != 1;
        }
        var next = groupTasks
            .Where(x => x.TaskStatus == TaktFlowTaskStatus.Cancelled)
            .OrderBy(x => x.SortOrder)
            .FirstOrDefault();
        if (next != null)
        {
            next.TaskStatus = TaktFlowTaskStatus.Pending;
            await _flowTaskRepository.UpdateAsync(next);
            return false;
        }
        addSign.IsHandled = 1;
        await _flowAddSignRepository.UpdateAsync(addSign);
        return addSign.ReturnToSignNode != 1;
    }

    /// <summary>
    /// 驳回处理
    /// </summary>
    private async Task HandleRejectAsync(
        TaktFlowInstance instance,
        TaktFlowTask pendingTask,
        TaktFlowTreeNode? currentNode,
        TaktFlowCompleteTaskDto dto,
        long userId)
    {
        pendingTask.TaskStatus = TaktFlowTaskStatus.Completed;
        pendingTask.CompletedAt = DateTime.Now;
        pendingTask.Comment = dto.Comment;
        await _flowTaskRepository.UpdateAsync(pendingTask);
        await CancelPendingTasksAsync(instance.Id, pendingTask.Id);
        await RecordTransitionAsync(
            instance,
            instance.CurrentActivityId,
            instance.CurrentActivityName,
            dto.Comment,
            userId,
            CurrentUserName,
            TaktFlowActionType.Reject,
            dto.NodeRejectStep);
        var root = TaktFlowProcessNavigator.ParseRoot(instance.ProcessContentSnapshot);
        if (root != null && !string.IsNullOrWhiteSpace(dto.NodeRejectStep))
        {
            var rejectNode = TaktFlowProcessNavigator.FindNode(root, dto.NodeRejectStep);
            if (rejectNode != null)
            {
                instance.InstanceStatus = TaktFlowInstanceStatus.Running;
                await EnterApproverNodeAsync(instance, rejectNode);
                return;
            }
        }
        instance.InstanceStatus = TaktFlowInstanceStatus.Rejected;
        instance.EndTime = DateTime.Now;
        if (instance.StartTime.HasValue)
        {
            instance.DurationMs = (long)(instance.EndTime.Value - instance.StartTime.Value).TotalMilliseconds;
        }
        await _flowInstanceRepository.UpdateAsync(instance);
    }

    /// <summary>
    /// 完成流程实例
    /// </summary>
    /// <param name="instance">实例</param>
    /// <param name="status">终态</param>
    /// <returns>任务</returns>
    private async Task CompleteInstanceAsync(TaktFlowInstance instance, TaktFlowInstanceStatus status)
    {
        await CancelPendingTasksAsync(instance.Id);
        instance.InstanceStatus = status;
        instance.EndTime = DateTime.Now;
        instance.CurrentActivityId = null;
        instance.CurrentActivityName = null;
        if (instance.StartTime.HasValue)
        {
            instance.DurationMs = (long)(instance.EndTime.Value - instance.StartTime.Value).TotalMilliseconds;
        }
        await _flowInstanceRepository.UpdateAsync(instance);
    }

    /// <summary>
    /// 取消实例待办
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <param name="exceptTaskId">排除任务</param>
    /// <returns>任务</returns>
    private async Task CancelPendingTasksAsync(long instanceId, long? exceptTaskId = null)
    {
        var pending = await _flowTaskRepository.GetListAsync(x =>
            x.InstanceId == instanceId && x.TaskStatus == TaktFlowTaskStatus.Pending);
        foreach (var task in pending)
        {
            if (exceptTaskId.HasValue && task.Id == exceptTaskId.Value)
            {
                continue;
            }
            task.TaskStatus = TaktFlowTaskStatus.Cancelled;
            await _flowTaskRepository.UpdateAsync(task);
        }
    }

    /// <summary>
    /// 是否仍有其他待办
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>是否存在</returns>
    private async Task<bool> HasOtherPendingTasksAsync(long instanceId)
    {
        return await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instanceId && x.TaskStatus == TaktFlowTaskStatus.Pending) != null;
    }

    /// <summary>
    /// 记录流转历史
    /// </summary>
    private async Task RecordTransitionAsync(
        TaktFlowInstance instance,
        string? fromNodeId,
        string? fromNodeName,
        string? comment,
        long userId,
        string? userName,
        TaktFlowActionType actionType,
        string? toNodeNameOverride)
    {
        var transition = new TaktFlowTransition
        {
            InstanceId = instance.Id,
            FromNodeId = fromNodeId,
            FromNodeName = fromNodeName,
            ToNodeId = instance.CurrentActivityId,
            ToNodeName = toNodeNameOverride ?? instance.CurrentActivityName,
            TransitionUserId = userId,
            TransitionUserName = userName,
            TransitionTime = DateTime.Now,
            TransitionComment = comment,
            ActionType = actionType
        };
        await _flowTransitionRepository.CreateAsync(transition);
    }

    /// <summary>
    /// 加载已发布流程定义
    /// </summary>
    /// <param name="processKey">流程键</param>
    /// <returns>定义</returns>
    private async Task<TaktFlowScheme> LoadPublishedSchemeAsync(string processKey)
    {
        var scheme = await _flowSchemeRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ProcessKey == processKey
            && x.IsLatest == 1
            && x.ProcessStatus == TaktFlowSchemeStatus.Published
            && x.SuspensionState == TaktFlowSuspensionState.Active);
        if (scheme == null)
        {
            ThrowBusinessException($"流程「{processKey}」未发布或不可用");
        }
        if (string.IsNullOrWhiteSpace(scheme.ProcessContent))
        {
            ThrowBusinessException("流程设计内容为空");
        }
        return scheme;
    }

    /// <summary>
    /// 创建流程实例实体
    /// </summary>
    private async Task<TaktFlowInstance> CreateInstanceEntityAsync(
        TaktFlowScheme scheme,
        TaktFlowStartDto dto,
        long startUserId,
        string? startUserName,
        TaktFlowInstanceStatus status)
    {
        var instance = new TaktFlowInstance
        {
            InstanceCode = GenerateInstanceCode(),
            ProcessDefinitionId = scheme.Id,
            ProcessKey = scheme.ProcessKey,
            ProcessName = scheme.ProcessName,
            DefinitionVersion = scheme.DefinitionVersion,
            ProcessTitle = dto.ProcessTitle,
            InstanceStatus = status,
            StartUserId = startUserId,
            StartUserName = startUserName,
            StartTime = status == TaktFlowInstanceStatus.Running ? DateTime.Now : null,
            BusinessKey = dto.BusinessKey,
            BusinessType = dto.BusinessType,
            FrmData = dto.FrmData,
            FormId = scheme.FormId,
            FormCode = scheme.FormCode,
            ProcessContentSnapshot = scheme.ProcessContent
        };
        return await _flowInstanceRepository.CreateAsync(instance);
    }

    /// <summary>
    /// 生成实例编码
    /// </summary>
    /// <returns>编码</returns>
    private static string GenerateInstanceCode()
    {
        return $"WF{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    /// <summary>
    /// 加载实例或抛错
    /// </summary>
    /// <param name="instanceId">实例 ID</param>
    /// <returns>实例</returns>
    private async Task<TaktFlowInstance> LoadInstanceOrThrowAsync(long instanceId)
    {
        var instance = await _flowInstanceRepository.GetByIdAsync(instanceId);
        if (instance == null || instance.TenantCode != CurrentTenantCode || instance.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("流程实例不存在");
        }
        return instance;
    }

    /// <summary>
    /// 校验实例可审批
    /// </summary>
    /// <param name="instance">实例</param>
    private static void EnsureInstanceRunnable(TaktFlowInstance instance)
    {
        if (instance.InstanceStatus != TaktFlowInstanceStatus.Running)
        {
            throw new TaktBusinessException("流程未处于运行中，无法操作");
        }
    }

    /// <summary>
    /// 映射列表项
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>列表 DTO</returns>
    private static TaktFlowInstanceListItemDto MapListItem(TaktFlowInstance entity)
    {
        return new TaktFlowInstanceListItemDto
        {
            FlowInstanceId = entity.Id,
            InstanceCode = entity.InstanceCode,
            ProcessName = entity.ProcessName,
            ProcessTitle = entity.ProcessTitle,
            InstanceStatus = entity.InstanceStatus,
            CurrentActivityName = entity.CurrentActivityName,
            StartUserId = entity.StartUserId,
            StartUserName = entity.StartUserName,
            StartTime = entity.StartTime,
            FrmData = entity.FrmData
        };
    }

    /// <summary>
    /// 映射详情
    /// </summary>
    /// <param name="instance">实例</param>
    /// <returns>详情 DTO</returns>
    private async Task<TaktFlowInstanceDetailDto> MapDetailAsync(TaktFlowInstance instance)
    {
        var userId = CurrentUserId;
        var transitions = await _flowTransitionRepository.GetListAsync(
            x => x.InstanceId == instance.Id,
            x => x.TransitionTime,
            false);
        var history = transitions.Select(t => new TaktFlowHistoryItemDto
        {
            FromNodeName = t.FromNodeName ?? "-",
            ToNodeName = t.ToNodeName ?? "-",
            TransitionUserName = t.TransitionUserName ?? "-",
            TransitionTime = t.TransitionTime,
            TransitionComment = t.TransitionComment
        }).ToList();
        var pendingAdds = await _flowAddSignRepository.GetListAsync(x =>
            x.InstanceId == instance.Id && x.IsHandled == 0);
        var canVerify = userId.HasValue && await _flowTaskRepository.FirstAsync(x =>
            x.InstanceId == instance.Id
            && x.AssigneeUserId == userId.Value
            && x.TaskStatus == TaktFlowTaskStatus.Pending) != null;
        return new TaktFlowInstanceDetailDto
        {
            FlowInstanceId = instance.Id,
            InstanceCode = instance.InstanceCode,
            ProcessDefinitionId = instance.ProcessDefinitionId,
            ProcessKey = instance.ProcessKey,
            ProcessName = instance.ProcessName,
            ProcessTitle = instance.ProcessTitle,
            InstanceStatus = instance.InstanceStatus,
            CurrentActivityId = instance.CurrentActivityId,
            CurrentActivityName = instance.CurrentActivityName,
            StartUserId = instance.StartUserId,
            StartUserName = instance.StartUserName,
            StartTime = instance.StartTime,
            EndTime = instance.EndTime,
            FrmData = instance.FrmData,
            History = history,
            PendingAddApprovers = pendingAdds.Select(p => new TaktFlowPendingAddApproverDto
            {
                FlowAddSignId = p.Id,
                ApproverUserName = p.SignUserName ?? p.SignUserId.ToString()
            }).ToList(),
            CanVerify = canVerify
        };
    }

    /// <summary>
    /// 待办/已办列表是否满足查询条件
    /// </summary>
    /// <param name="instance">流程实例</param>
    /// <param name="task">待办任务（已办可为 null）</param>
    /// <param name="query">查询条件</param>
    /// <returns>是否匹配</returns>
    private static bool MatchesTodoQuery(TaktFlowInstance instance, TaktFlowTask? task, TaktFlowTodoQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.InstanceCode)
            && !instance.InstanceCode.Contains(query.InstanceCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessKey)
            && !(instance.ProcessKey?.Contains(query.ProcessKey, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessName)
            && !(instance.ProcessName?.Contains(query.ProcessName, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessTitle)
            && !(instance.ProcessTitle?.Contains(query.ProcessTitle, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (query.ProcessDefinitionId.HasValue
            && instance.ProcessDefinitionId != query.ProcessDefinitionId.Value)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.TaskName))
        {
            var nodeName = task?.TaskName ?? instance.CurrentActivityName;
            if (nodeName?.Contains(query.TaskName, StringComparison.OrdinalIgnoreCase) != true)
            {
                return false;
            }
        }
        if (!string.IsNullOrWhiteSpace(query.StartUserName)
            && !(instance.StartUserName?.Contains(query.StartUserName, StringComparison.OrdinalIgnoreCase) == true))
        {
            return false;
        }
        if (query.StartTimeStart.HasValue
            && (instance.StartTime == null || instance.StartTime < query.StartTimeStart))
        {
            return false;
        }
        if (query.StartTimeEnd.HasValue
            && (instance.StartTime == null || instance.StartTime > query.StartTimeEnd))
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(query.KeyWords))
        {
            var kw = query.KeyWords;
            if (!(instance.ProcessName?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true
                || instance.ProcessTitle?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true
                || instance.InstanceCode.Contains(kw, StringComparison.OrdinalIgnoreCase)
                || (instance.ProcessKey?.Contains(kw, StringComparison.OrdinalIgnoreCase) == true)))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 为「我的流程」列表追加实例查询条件
    /// </summary>
    /// <param name="exp">表达式</param>
    /// <param name="query">查询 DTO</param>
    /// <returns>追加条件后的表达式</returns>
    private static Expressionable<TaktFlowInstance> ApplyFlowInstanceListQuery(
        Expressionable<TaktFlowInstance> exp,
        TaktFlowInstanceQueryDto query)
    {
        if (!string.IsNullOrWhiteSpace(query.InstanceCode))
        {
            var code = query.InstanceCode;
            exp = exp.And(x => x.InstanceCode.Contains(code));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessKey))
        {
            var key = query.ProcessKey;
            exp = exp.And(x => x.ProcessKey != null && x.ProcessKey.Contains(key));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessName))
        {
            var name = query.ProcessName;
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(name));
        }
        if (!string.IsNullOrWhiteSpace(query.ProcessTitle))
        {
            var title = query.ProcessTitle;
            exp = exp.And(x => x.ProcessTitle != null && x.ProcessTitle.Contains(title));
        }
        if (query.ProcessDefinitionId.HasValue)
        {
            var defId = query.ProcessDefinitionId.Value;
            exp = exp.And(x => x.ProcessDefinitionId == defId);
        }
        if (query.InstanceStatus.HasValue)
        {
            var status = query.InstanceStatus.Value;
            exp = exp.And(x => x.InstanceStatus == status);
        }
        if (!string.IsNullOrWhiteSpace(query.StartUserName))
        {
            var userName = query.StartUserName;
            exp = exp.And(x => x.StartUserName != null && x.StartUserName.Contains(userName));
        }
        if (query.StartTimeStart.HasValue)
        {
            var start = query.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime >= start);
        }
        if (query.StartTimeEnd.HasValue)
        {
            var end = query.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime != null && x.StartTime <= end);
        }
        if (!string.IsNullOrWhiteSpace(query.KeyWords))
        {
            var kw = query.KeyWords;
            exp = exp.And(x =>
                x.InstanceCode.Contains(kw)
                || (x.ProcessKey != null && x.ProcessKey.Contains(kw))
                || (x.ProcessName != null && x.ProcessName.Contains(kw))
                || (x.ProcessTitle != null && x.ProcessTitle.Contains(kw)));
        }
        return exp;
    }
}
