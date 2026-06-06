// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowTaskService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：流程用户任务应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Workflow;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程用户任务应用服务
/// </summary>
public class TaktFlowTaskService : TaktServiceBase, ITaktFlowTaskService
{
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTaskRepository">流程用户任务仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowTaskService(
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowTaskRepository = flowTaskRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程用户任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowTaskDto>> GetFlowTaskListAsync(TaktFlowTaskQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowTaskRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowTaskDto>.Create(
            data.Adapt<List<TaktFlowTaskDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto?> GetFlowTaskByIdAsync(long id)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowTaskDto>();
    }

    /// <summary>
    /// 获取流程用户任务选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowTaskOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _flowTaskRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TaskName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TaskName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建流程用户任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> CreateFlowTaskAsync(TaktFlowTaskCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowTask>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _flowTaskRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InstanceId == entity.InstanceId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.InstanceId, maxSort);
        }
        entity = await _flowTaskRepository.CreateAsync(entity);
        return await GetFlowTaskByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowTaskDto>();
    }

    /// <summary>
    /// 更新流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskAsync(long id, TaktFlowTaskUpdateDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        dto.Adapt(entity);
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(id) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 删除流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTaskByIdAsync(long id)
    {
        var deleted = await _flowTaskRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程用户任务不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程用户任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTaskBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowTaskByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新流程用户任务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskStatusAsync(TaktFlowTaskStatusDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(dto.FlowTaskId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        entity.TaskStatus = dto.TaskStatus;
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(dto.FlowTaskId) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 更新流程用户任务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskSortAsync(TaktFlowTaskSortDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(dto.FlowTaskId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(dto.FlowTaskId) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowTaskTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowTaskTemplateDto>(
            sheetName ?? "流程用户任务导入模板",
            fileName ?? "流程用户任务导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程用户任务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowTaskAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowTaskImportDto>(fileStream, sheetName ?? "流程用户任务导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowTask>();
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _flowTaskRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InstanceId == entity.InstanceId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.InstanceId, maxSort);
                }
                await _flowTaskRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出流程用户任务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowTaskAsync(TaktFlowTaskQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowTaskQueryDto());
        var list = await _flowTaskRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTaskExportDto>(),
                sheetName ?? "流程用户任务数据",
                fileName ?? "流程用户任务导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowTaskExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程用户任务数据",
            fileName ?? "流程用户任务导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程用户任务查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowTask, bool>> QueryExpression(TaktFlowTaskQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowTask>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.InstanceId).Contains(keywords)
                || (x.TaskDefinitionKey != null && x.TaskDefinitionKey.Contains(keywords))
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || SqlFunc.ToString(x.AssigneeUserId).Contains(keywords)
                || (x.AssigneeUserName != null && x.AssigneeUserName.Contains(keywords))
                || SqlFunc.ToString(x.OwnerUserId).Contains(keywords)
                || SqlFunc.ToString(x.TaskStatus).Contains(keywords)
                || SqlFunc.ToString(x.SignType).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || SqlFunc.ToString(x.IsAddSign).Contains(keywords)
                || SqlFunc.ToString(x.AddSignId).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.Comment != null && x.Comment.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.DueDate).Contains(keywords)
                || SqlFunc.ToString(x.ClaimTime).Contains(keywords)
                || SqlFunc.ToString(x.CompletedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InstanceId.HasValue == true)
        {
            exp = exp.And(x => x.InstanceId == queryDto.InstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TaskDefinitionKey))
        {
            exp = exp.And(x => x.TaskDefinitionKey != null && x.TaskDefinitionKey.Contains(queryDto.TaskDefinitionKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaskName))
        {
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(queryDto.TaskName));
        }

        if (queryDto?.AssigneeUserId.HasValue == true)
        {
            exp = exp.And(x => x.AssigneeUserId == queryDto.AssigneeUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssigneeUserName))
        {
            exp = exp.And(x => x.AssigneeUserName != null && x.AssigneeUserName.Contains(queryDto.AssigneeUserName));
        }

        if (queryDto?.OwnerUserId.HasValue == true)
        {
            exp = exp.And(x => x.OwnerUserId == queryDto.OwnerUserId);
        }

        if (queryDto?.TaskStatus.HasValue == true)
        {
            exp = exp.And(x => x.TaskStatus == queryDto.TaskStatus);
        }

        if (queryDto?.SignType.HasValue == true)
        {
            exp = exp.And(x => x.SignType == queryDto.SignType);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (queryDto?.IsAddSign.HasValue == true)
        {
            exp = exp.And(x => x.IsAddSign == queryDto.IsAddSign);
        }

        if (queryDto?.AddSignId.HasValue == true)
        {
            exp = exp.And(x => x.AddSignId == queryDto.AddSignId);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.Comment))
        {
            exp = exp.And(x => x.Comment != null && x.Comment.Contains(queryDto.Comment));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.DueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DueDate >= queryDto.DueDateStart);
        }

        if (queryDto?.DueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DueDate <= queryDto.DueDateEnd);
        }

        if (queryDto?.ClaimTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ClaimTime >= queryDto.ClaimTimeStart);
        }

        if (queryDto?.ClaimTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ClaimTime <= queryDto.ClaimTimeEnd);
        }

        if (queryDto?.CompletedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CompletedAt >= queryDto.CompletedAtStart);
        }

        if (queryDto?.CompletedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CompletedAt <= queryDto.CompletedAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
