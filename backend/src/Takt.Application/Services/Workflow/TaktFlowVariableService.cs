// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowVariableService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：流程变量应用服务实现
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
using Takt.Shared.Enums;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程变量应用服务
/// </summary>
public class TaktFlowVariableService : TaktServiceBase, ITaktFlowVariableService
{
    private readonly ITaktCompanyRepository<TaktFlowVariable> _flowVariableRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowVariableRepository">流程变量仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowVariableService(
        ITaktCompanyRepository<TaktFlowVariable> flowVariableRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowVariableRepository = flowVariableRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程变量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowVariableDto>> GetFlowVariableListAsync(TaktFlowVariableQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowVariableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowVariableDto>.Create(
            data.Adapt<List<TaktFlowVariableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto?> GetFlowVariableByIdAsync(long id)
    {
        var entity = await _flowVariableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowVariableDto>();
    }

    /// <summary>
    /// 获取流程变量选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowVariableOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _flowVariableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.VariableName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.VariableName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建流程变量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto> CreateFlowVariableAsync(TaktFlowVariableCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowVariable>();
        var isUnique_ix_flow_variable_instance_name = await _uniqueValidator.IsUniqueAsync(
            _flowVariableRepository,
            x => x.InstanceId == entity.InstanceId
                && x.VariableName == entity.VariableName);
        if (!isUnique_ix_flow_variable_instance_name)
        {
            throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
        }
        entity = await _flowVariableRepository.CreateAsync(entity);
        return await GetFlowVariableByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowVariableDto>();
    }

    /// <summary>
    /// 更新流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto> UpdateFlowVariableAsync(long id, TaktFlowVariableUpdateDto dto)
    {
        var entity = await _flowVariableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程变量不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_flow_variable_instance_name = await _uniqueValidator.IsUniqueAsync(
            _flowVariableRepository,
            x => x.InstanceId == entity.InstanceId
                && x.VariableName == entity.VariableName,
            id);
        if (!isUnique_ix_flow_variable_instance_name)
        {
            throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
        }
        await _flowVariableRepository.UpdateAsync(entity);
        return await GetFlowVariableByIdAsync(id) ?? throw new TaktBusinessException("流程变量不存在");
    }

    /// <summary>
    /// 删除流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowVariableByIdAsync(long id)
    {
        var deleted = await _flowVariableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程变量不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程变量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowVariableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowVariableByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowVariableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowVariableTemplateDto>(
            sheetName ?? "流程变量导入模板",
            fileName ?? "流程变量导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程变量
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowVariableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowVariableImportDto>(fileStream, sheetName ?? "流程变量导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowVariable>();
                var importKey = $"{entity.InstanceId}|{entity.VariableName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InstanceId、VariableName）");
                }
                var isUnique_ix_flow_variable_instance_name = await _uniqueValidator.IsUniqueAsync(
                    _flowVariableRepository,
                    x => x.InstanceId == entity.InstanceId
                        && x.VariableName == entity.VariableName);
                if (!isUnique_ix_flow_variable_instance_name)
                {
                    throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
                }
                await _flowVariableRepository.CreateAsync(entity);
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
    /// 导出流程变量
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowVariableAsync(TaktFlowVariableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowVariableQueryDto());
        var list = await _flowVariableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowVariableExportDto>(),
                sheetName ?? "流程变量数据",
                fileName ?? "流程变量导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowVariableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程变量数据",
            fileName ?? "流程变量导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程变量查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowVariable, bool>> QueryExpression(TaktFlowVariableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowVariable>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.InstanceId).Contains(keywords)
                || SqlFunc.ToString(x.TaskId).Contains(keywords)
                || (x.VariableName != null && x.VariableName.Contains(keywords))
                || SqlFunc.ToString(x.VariableType).Contains(keywords)
                || (x.TextValue != null && x.TextValue.Contains(keywords))
                || SqlFunc.ToString(x.LongValue).Contains(keywords)
                || SqlFunc.ToString(x.DoubleValue).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InstanceId.HasValue == true)
        {
            exp = exp.And(x => x.InstanceId == queryDto.InstanceId);
        }

        if (queryDto?.TaskId.HasValue == true)
        {
            exp = exp.And(x => x.TaskId == queryDto.TaskId);
        }

        if (!string.IsNullOrEmpty(queryDto?.VariableName))
        {
            exp = exp.And(x => x.VariableName != null && x.VariableName.Contains(queryDto.VariableName));
        }

        if (queryDto?.VariableType.HasValue == true)
        {
            exp = exp.And(x => x.VariableType == queryDto.VariableType);
        }

        if (!string.IsNullOrEmpty(queryDto?.TextValue))
        {
            exp = exp.And(x => x.TextValue != null && x.TextValue.Contains(queryDto.TextValue));
        }

        if (queryDto?.LongValue.HasValue == true)
        {
            exp = exp.And(x => x.LongValue == queryDto.LongValue);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
