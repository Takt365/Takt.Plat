// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktNumberingService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：编号规则应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 编号规则应用服务
/// </summary>
public class TaktNumberingService : TaktServiceBase, ITaktNumberingService
{
    private readonly ITaktCompanyRepository<TaktNumbering> _numberingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编号规则仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNumberingService(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _numberingRepository = numberingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取编号规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNumberingDto>> GetNumberingListAsync(TaktNumberingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _numberingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNumberingDto>.Create(
            data.Adapt<List<TaktNumberingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto?> GetNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 获取编号规则选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNumberingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _numberingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.RuleName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RuleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建编号规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> CreateNumberingAsync(TaktNumberingCreateDto dto)
    {
        var entity = dto.Adapt<TaktNumbering>();
        entity.IsBuiltIn = TaktYesNo.No;
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编号规则的RuleCode已存在");
        }
        entity = await _numberingRepository.CreateAsync(entity);
        return await GetNumberingByIdAsync(entity.Id) ?? entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 更新编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingAsync(long id, TaktNumberingUpdateDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode,
            id);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编号规则的RuleCode已存在");
        }
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(id) ?? throw new TaktBusinessException("编号规则不存在");
    }

    /// <summary>
    /// 删除编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在或已删除");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置编号规则不允许删除");
        }
        var deleted = await _numberingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("编号规则不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除编号规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _numberingRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置编号规则不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteNumberingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新编号规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingStatusAsync(TaktNumberingStatusDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(dto.NumberingId);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.Status != TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException("不允许禁用内置编号规则");
        }
        entity.Status = dto.Status;
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(dto.NumberingId) ?? throw new TaktBusinessException("编号规则不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNumberingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNumberingTemplateDto>(
            sheetName ?? "编号规则导入模板",
            fileName ?? "编号规则导入模板.xlsx");
    }

    /// <summary>
    /// 导入编号规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNumberingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNumberingImportDto>(fileStream, sheetName ?? "编号规则导入模板");
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
                var entity = rows[i].Adapt<TaktNumbering>();
                entity.IsBuiltIn = TaktYesNo.No;
                var importKey = $"{entity.RuleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RuleCode）");
                }
                var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _numberingRepository,
                    x => x.RuleCode == entity.RuleCode);
                if (!isUnique_ix_numbering_code_unique)
                {
                    throw new TaktBusinessException("编号规则的RuleCode已存在");
                }
                await _numberingRepository.CreateAsync(entity);
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
    /// 导出编号规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNumberingAsync(TaktNumberingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNumberingQueryDto());
        var list = await _numberingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNumberingExportDto>(),
                sheetName ?? "编号规则数据",
                fileName ?? "编号规则导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNumberingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "编号规则数据",
            fileName ?? "编号规则导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建编号规则查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNumbering, bool>> QueryExpression(TaktNumberingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNumbering>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RuleCode != null && x.RuleCode.Contains(keywords))
                || (x.RuleName != null && x.RuleName.Contains(keywords))
                || SqlFunc.ToString(x.DocumentType).Contains(keywords)
                || (x.DepartmentCode != null && x.DepartmentCode.Contains(keywords))
                || (x.Prefix != null && x.Prefix.Contains(keywords))
                || (x.DateFormat != null && x.DateFormat.Contains(keywords))
                || SqlFunc.ToString(x.SequenceLength).Contains(keywords)
                || SqlFunc.ToString(x.SequenceStep).Contains(keywords)
                || (x.Suffix != null && x.Suffix.Contains(keywords))
                || (x.ResetPeriod != null && x.ResetPeriod.Contains(keywords))
                || SqlFunc.ToString(x.CurrentSequence).Contains(keywords)
                || (x.ExampleCode != null && x.ExampleCode.Contains(keywords))
                || (x.Separator != null && x.Separator.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.Status).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleCode))
        {
            exp = exp.And(x => x.RuleCode != null && x.RuleCode.Contains(queryDto.RuleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleName))
        {
            exp = exp.And(x => x.RuleName != null && x.RuleName.Contains(queryDto.RuleName));
        }

        if (queryDto?.DocumentType.HasValue == true)
        {
            exp = exp.And(x => x.DocumentType == queryDto.DocumentType);
        }

        if (!string.IsNullOrEmpty(queryDto?.DepartmentCode))
        {
            exp = exp.And(x => x.DepartmentCode != null && x.DepartmentCode.Contains(queryDto.DepartmentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Prefix))
        {
            exp = exp.And(x => x.Prefix != null && x.Prefix.Contains(queryDto.Prefix));
        }

        if (!string.IsNullOrEmpty(queryDto?.DateFormat))
        {
            exp = exp.And(x => x.DateFormat != null && x.DateFormat.Contains(queryDto.DateFormat));
        }

        if (queryDto?.SequenceLength.HasValue == true)
        {
            exp = exp.And(x => x.SequenceLength == queryDto.SequenceLength);
        }

        if (queryDto?.SequenceStep.HasValue == true)
        {
            exp = exp.And(x => x.SequenceStep == queryDto.SequenceStep);
        }

        if (!string.IsNullOrEmpty(queryDto?.Suffix))
        {
            exp = exp.And(x => x.Suffix != null && x.Suffix.Contains(queryDto.Suffix));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResetPeriod))
        {
            exp = exp.And(x => x.ResetPeriod != null && x.ResetPeriod.Contains(queryDto.ResetPeriod));
        }

        if (queryDto?.CurrentSequence.HasValue == true)
        {
            exp = exp.And(x => x.CurrentSequence == queryDto.CurrentSequence);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExampleCode))
        {
            exp = exp.And(x => x.ExampleCode != null && x.ExampleCode.Contains(queryDto.ExampleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Separator))
        {
            exp = exp.And(x => x.Separator != null && x.Separator.Contains(queryDto.Separator));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.Status.HasValue == true)
        {
            exp = exp.And(x => x.Status == queryDto.Status);
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
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
