// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowTransitionService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：流程流转历史应用服务实现
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
/// 流程流转历史应用服务
/// </summary>
public class TaktFlowTransitionService : TaktServiceBase, ITaktFlowTransitionService
{
    private readonly ITaktCompanyRepository<TaktFlowTransition> _flowTransitionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTransitionRepository">流程流转历史仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowTransitionService(
        ITaktCompanyRepository<TaktFlowTransition> flowTransitionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowTransitionRepository = flowTransitionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程流转历史列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowTransitionDto>> GetFlowTransitionListAsync(TaktFlowTransitionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowTransitionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowTransitionDto>.Create(
            data.Adapt<List<TaktFlowTransitionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto?> GetFlowTransitionByIdAsync(long id)
    {
        var entity = await _flowTransitionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowTransitionDto>();
    }

    /// <summary>
    /// 获取流程流转历史选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowTransitionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _flowTransitionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ActivityName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ActivityName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建流程流转历史
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto> CreateFlowTransitionAsync(TaktFlowTransitionCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowTransition>();
        entity = await _flowTransitionRepository.CreateAsync(entity);
        return await GetFlowTransitionByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowTransitionDto>();
    }

    /// <summary>
    /// 更新流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto> UpdateFlowTransitionAsync(long id, TaktFlowTransitionUpdateDto dto)
    {
        var entity = await _flowTransitionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程流转历史不存在");
        }
        dto.Adapt(entity);
        await _flowTransitionRepository.UpdateAsync(entity);
        return await GetFlowTransitionByIdAsync(id) ?? throw new TaktBusinessException("流程流转历史不存在");
    }

    /// <summary>
    /// 删除流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTransitionByIdAsync(long id)
    {
        var deleted = await _flowTransitionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程流转历史不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程流转历史
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTransitionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowTransitionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowTransitionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowTransitionTemplateDto>(
            sheetName ?? "流程流转历史导入模板",
            fileName ?? "流程流转历史导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程流转历史
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowTransitionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowTransitionImportDto>(fileStream, sheetName ?? "流程流转历史导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowTransition>();
                await _flowTransitionRepository.CreateAsync(entity);
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
    /// 导出流程流转历史
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowTransitionAsync(TaktFlowTransitionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowTransitionQueryDto());
        var list = await _flowTransitionRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTransitionExportDto>(),
                sheetName ?? "流程流转历史数据",
                fileName ?? "流程流转历史导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowTransitionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程流转历史数据",
            fileName ?? "流程流转历史导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程流转历史查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowTransition, bool>> QueryExpression(TaktFlowTransitionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowTransition>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.InstanceId).Contains(keywords)
                || (x.ActivityId != null && x.ActivityId.Contains(keywords))
                || (x.ActivityName != null && x.ActivityName.Contains(keywords))
                || (x.ActivityType != null && x.ActivityType.Contains(keywords))
                || (x.FromNodeId != null && x.FromNodeId.Contains(keywords))
                || (x.FromNodeName != null && x.FromNodeName.Contains(keywords))
                || (x.ToNodeId != null && x.ToNodeId.Contains(keywords))
                || (x.ToNodeName != null && x.ToNodeName.Contains(keywords))
                || SqlFunc.ToString(x.TransitionUserId).Contains(keywords)
                || (x.TransitionUserName != null && x.TransitionUserName.Contains(keywords))
                || SqlFunc.ToString(x.DurationMs).Contains(keywords)
                || (x.TransitionComment != null && x.TransitionComment.Contains(keywords))
                || SqlFunc.ToString(x.ActionType).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartTime).Contains(keywords)
                || SqlFunc.ToString(x.TransitionTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InstanceId.HasValue == true)
        {
            exp = exp.And(x => x.InstanceId == queryDto.InstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ActivityId))
        {
            exp = exp.And(x => x.ActivityId != null && x.ActivityId.Contains(queryDto.ActivityId));
        }

        if (!string.IsNullOrEmpty(queryDto?.ActivityName))
        {
            exp = exp.And(x => x.ActivityName != null && x.ActivityName.Contains(queryDto.ActivityName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ActivityType))
        {
            exp = exp.And(x => x.ActivityType != null && x.ActivityType.Contains(queryDto.ActivityType));
        }

        if (!string.IsNullOrEmpty(queryDto?.FromNodeId))
        {
            exp = exp.And(x => x.FromNodeId != null && x.FromNodeId.Contains(queryDto.FromNodeId));
        }

        if (!string.IsNullOrEmpty(queryDto?.FromNodeName))
        {
            exp = exp.And(x => x.FromNodeName != null && x.FromNodeName.Contains(queryDto.FromNodeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ToNodeId))
        {
            exp = exp.And(x => x.ToNodeId != null && x.ToNodeId.Contains(queryDto.ToNodeId));
        }

        if (!string.IsNullOrEmpty(queryDto?.ToNodeName))
        {
            exp = exp.And(x => x.ToNodeName != null && x.ToNodeName.Contains(queryDto.ToNodeName));
        }

        if (queryDto?.TransitionUserId.HasValue == true)
        {
            exp = exp.And(x => x.TransitionUserId == queryDto.TransitionUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TransitionUserName))
        {
            exp = exp.And(x => x.TransitionUserName != null && x.TransitionUserName.Contains(queryDto.TransitionUserName));
        }

        if (queryDto?.DurationMs.HasValue == true)
        {
            exp = exp.And(x => x.DurationMs == queryDto.DurationMs);
        }

        if (!string.IsNullOrEmpty(queryDto?.TransitionComment))
        {
            exp = exp.And(x => x.TransitionComment != null && x.TransitionComment.Contains(queryDto.TransitionComment));
        }

        if (queryDto?.ActionType.HasValue == true)
        {
            exp = exp.And(x => x.ActionType == queryDto.ActionType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.StartTime >= queryDto.StartTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartTime <= queryDto.StartTimeEnd);
        }

        if (queryDto?.TransitionTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.TransitionTime >= queryDto.TransitionTimeStart);
        }

        if (queryDto?.TransitionTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.TransitionTime <= queryDto.TransitionTimeEnd);
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
