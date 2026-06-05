// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowEngineController.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时控制器（发起/待办/审批/加签等；与实例 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Workflow;
using Takt.Application.Services.Workflow.FlowEngine;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Workflow;

/// <summary>
/// 流程引擎控制器（运行时能力，对应 <see cref="ITaktFlowEngineService"/>）
/// </summary>
[ApiModule(TaktModule.Workflow, "流程引擎")]
[Route("api/[controller]", Name = "流程引擎")]
public class TaktFlowEngineController : TaktControllerBase
{
    private readonly ITaktFlowEngineService _flowEngineService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowEngineService">流程引擎服务</param>
    public TaktFlowEngineController(ITaktFlowEngineService flowEngineService)
    {
        _flowEngineService = flowEngineService;
    }

    #region 详情与列表

    /// <summary>
    /// 获取流程实例详情（运行时视图）
    /// </summary>
    /// <param name="id">实例 ID</param>
    /// <returns>实例详情</returns>
    [TaktPermission("workflow:instance:detail", "流程实例详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFlowInstanceDetailByIdAsync(long id)
    {
        try
        {
            var result = await _flowEngineService.GetFlowInstanceDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程实例不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取待办列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:todo:list", "待办列表")]
    [HttpGet("todo/list")]
    public async Task<IActionResult> GetFlowInstanceTodoListAsync([FromQuery] TaktFlowTodoQueryDto queryDto)
    {
        try
        {
            var result = await _flowEngineService.GetFlowInstanceTodoListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取我发起的流程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:my:list", "我的流程列表")]
    [HttpGet("my/list")]
    public async Task<IActionResult> GetFlowInstanceMyListAsync([FromQuery] TaktFlowMyInstanceQueryDto queryDto)
    {
        try
        {
            var result = await _flowEngineService.GetFlowInstanceMyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取已办流程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:processed:list", "已办流程列表")]
    [HttpGet("processed/list")]
    public async Task<IActionResult> GetFlowInstanceProcessedListAsync([FromQuery] TaktFlowTodoQueryDto queryDto)
    {
        try
        {
            var result = await _flowEngineService.GetFlowInstanceProcessedListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 发起与草稿

    /// <summary>
    /// 发起流程
    /// </summary>
    /// <param name="dto">发起参数</param>
    /// <returns>实例详情</returns>
    [TaktPermission("workflow:instance:start", "发起流程")]
    [HttpPost("start")]
    public async Task<IActionResult> StartFlowInstanceAsync([FromBody] TaktFlowStartDto dto)
    {
        try
        {
            var result = await _flowEngineService.StartFlowInstanceAsync(dto);
            return Success(result, "发起成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 保存草稿
    /// </summary>
    /// <param name="dto">发起参数</param>
    /// <returns>实例详情</returns>
    [TaktPermission("workflow:instance:start", "保存流程草稿")]
    [HttpPost("draft")]
    public async Task<IActionResult> CreateFlowInstanceDraftAsync([FromBody] TaktFlowStartDto dto)
    {
        try
        {
            var result = await _flowEngineService.CreateFlowInstanceDraftAsync(dto);
            return Success(result, "保存成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 从草稿启动流程
    /// </summary>
    /// <param name="id">实例 ID</param>
    /// <returns>实例详情</returns>
    [TaktPermission("workflow:instance:start", "从草稿启动流程")]
    [HttpPost("{id:long}/start-from-draft")]
    public async Task<IActionResult> StartFlowInstanceFromDraftAsync(long id)
    {
        try
        {
            var result = await _flowEngineService.StartFlowInstanceFromDraftAsync(id);
            return Success(result, "启动成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 审批操作

    /// <summary>
    /// 办结任务（通过/驳回）
    /// </summary>
    /// <param name="dto">办结参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:approve", "审批流程任务")]
    [HttpPost("complete")]
    public async Task<IActionResult> CompleteFlowInstanceTaskAsync([FromBody] TaktFlowCompleteTaskDto dto)
    {
        try
        {
            await _flowEngineService.CompleteFlowInstanceTaskAsync(dto);
            return Success("操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 撤回流程（发起人）
    /// </summary>
    /// <param name="instanceCode">实例编码</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:revoke", "撤回流程")]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeFlowInstanceAsync([FromQuery] string instanceCode)
    {
        try
        {
            await _flowEngineService.RevokeFlowInstanceAsync(instanceCode);
            return Success("撤回成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 转办
    /// </summary>
    /// <param name="dto">转办参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:transfer", "转办流程任务")]
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferFlowInstanceAsync([FromBody] TaktFlowTransferDto dto)
    {
        try
        {
            await _flowEngineService.TransferFlowInstanceAsync(dto);
            return Success("转办成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 加签
    /// </summary>
    /// <param name="dto">加签参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:addsign", "加签")]
    [HttpPost("add-approvers")]
    public async Task<IActionResult> AddFlowInstanceApproversAsync([FromBody] TaktFlowAddApproversDto dto)
    {
        try
        {
            await _flowEngineService.AddFlowInstanceApproversAsync(dto);
            return Success("加签成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 减签
    /// </summary>
    /// <param name="dto">减签参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:reducesign", "减签")]
    [HttpPost("reduce-sign")]
    public async Task<IActionResult> ReduceFlowInstanceApprovalAsync([FromBody] TaktFlowReduceApprovalDto dto)
    {
        try
        {
            await _flowEngineService.ReduceFlowInstanceApprovalAsync(dto);
            return Success("减签成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 挂起流程
    /// </summary>
    /// <param name="dto">操作参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:suspend", "挂起流程")]
    [HttpPost("suspend")]
    public async Task<IActionResult> SuspendFlowInstanceAsync([FromBody] TaktFlowInstanceOperateDto dto)
    {
        try
        {
            await _flowEngineService.SuspendFlowInstanceAsync(dto);
            return Success("挂起成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 恢复流程
    /// </summary>
    /// <param name="dto">操作参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:resume", "恢复流程")]
    [HttpPost("resume")]
    public async Task<IActionResult> ResumeFlowInstanceAsync([FromBody] TaktFlowInstanceOperateDto dto)
    {
        try
        {
            await _flowEngineService.ResumeFlowInstanceAsync(dto);
            return Success("恢复成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 终止流程
    /// </summary>
    /// <param name="dto">操作参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:terminate", "终止流程")]
    [HttpPost("terminate")]
    public async Task<IActionResult> TerminateFlowInstanceAsync([FromBody] TaktFlowInstanceOperateDto dto)
    {
        try
        {
            await _flowEngineService.TerminateFlowInstanceAsync(dto);
            return Success("终止成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 撤销当前节点审批
    /// </summary>
    /// <param name="dto">操作参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:undo", "撤销审批")]
    [HttpPost("undo-verification")]
    public async Task<IActionResult> UndoFlowInstanceVerificationAsync([FromBody] TaktFlowInstanceOperateDto dto)
    {
        try
        {
            await _flowEngineService.UndoFlowInstanceVerificationAsync(dto);
            return Success("撤销成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion
}
