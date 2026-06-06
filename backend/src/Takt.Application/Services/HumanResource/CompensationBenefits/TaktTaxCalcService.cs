// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalcService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：个税计算规则应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.CompensationBenefits;
using Takt.Domain.Entities.HumanResource.CompensationBenefits;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.CompensationBenefits;

/// <summary>
/// 个税计算规则应用服务
/// </summary>
public class TaktTaxCalcService : TaktServiceBase, ITaktTaxCalcService
{
    private readonly ITaktCompanyRepository<TaktTaxCalc> _taxCalcRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxCalcRepository">个税计算规则仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTaxCalcService(
        ITaktCompanyRepository<TaktTaxCalc> taxCalcRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _taxCalcRepository = taxCalcRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取个税计算规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTaxCalcDto>> GetTaxCalcListAsync(TaktTaxCalcQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _taxCalcRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTaxCalcDto>.Create(
            data.Adapt<List<TaktTaxCalcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCalcDto?> GetTaxCalcByIdAsync(long id)
    {
        var entity = await _taxCalcRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTaxCalcDto>();
    }

    /// <summary>
    /// 获取个税计算规则选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTaxCalcOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _taxCalcRepository.GetListAsync(
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
    /// 创建个税计算规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCalcDto> CreateTaxCalcAsync(TaktTaxCalcCreateDto dto)
    {
        var entity = dto.Adapt<TaktTaxCalc>();
        var isUnique_ix_tax_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _taxCalcRepository,
            x => x.RuleCode == entity.RuleCode);
        if (!isUnique_ix_tax_calc_code_unique)
        {
            throw new TaktBusinessException("个税计算规则的RuleCode已存在");
        }
        entity = await _taxCalcRepository.CreateAsync(entity);
        return await GetTaxCalcByIdAsync(entity.Id) ?? entity.Adapt<TaktTaxCalcDto>();
    }

    /// <summary>
    /// 更新个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCalcDto> UpdateTaxCalcAsync(long id, TaktTaxCalcUpdateDto dto)
    {
        var entity = await _taxCalcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("个税计算规则不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_tax_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _taxCalcRepository,
            x => x.RuleCode == entity.RuleCode,
            id);
        if (!isUnique_ix_tax_calc_code_unique)
        {
            throw new TaktBusinessException("个税计算规则的RuleCode已存在");
        }
        await _taxCalcRepository.UpdateAsync(entity);
        return await GetTaxCalcByIdAsync(id) ?? throw new TaktBusinessException("个税计算规则不存在");
    }

    /// <summary>
    /// 删除个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxCalcByIdAsync(long id)
    {
        var deleted = await _taxCalcRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("个税计算规则不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除个税计算规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxCalcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTaxCalcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新个税计算规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCalcDto> UpdateTaxCalcStatusAsync(TaktTaxCalcStatusDto dto)
    {
        var entity = await _taxCalcRepository.GetByIdAsync(dto.TaxCalcId);
        if (entity == null)
        {
            throw new TaktBusinessException("个税计算规则不存在");
        }
        entity.TaxCalcStatus = dto.TaxCalcStatus;
        await _taxCalcRepository.UpdateAsync(entity);
        return await GetTaxCalcByIdAsync(dto.TaxCalcId) ?? throw new TaktBusinessException("个税计算规则不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTaxCalcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTaxCalcTemplateDto>(
            sheetName ?? "个税计算规则导入模板",
            fileName ?? "个税计算规则导入模板.xlsx");
    }

    /// <summary>
    /// 导入个税计算规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTaxCalcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTaxCalcImportDto>(fileStream, sheetName ?? "个税计算规则导入模板");
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
                var entity = rows[i].Adapt<TaktTaxCalc>();
                var importKey = $"{entity.RuleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RuleCode）");
                }
                var isUnique_ix_tax_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _taxCalcRepository,
                    x => x.RuleCode == entity.RuleCode);
                if (!isUnique_ix_tax_calc_code_unique)
                {
                    throw new TaktBusinessException("个税计算规则的RuleCode已存在");
                }
                await _taxCalcRepository.CreateAsync(entity);
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
    /// 导出个税计算规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTaxCalcAsync(TaktTaxCalcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTaxCalcQueryDto());
        var list = await _taxCalcRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTaxCalcExportDto>(),
                sheetName ?? "个税计算规则数据",
                fileName ?? "个税计算规则导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTaxCalcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "个税计算规则数据",
            fileName ?? "个税计算规则导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建个税计算规则查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTaxCalc, bool>> QueryExpression(TaktTaxCalcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTaxCalc>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RuleCode != null && x.RuleCode.Contains(keywords))
                || (x.RuleName != null && x.RuleName.Contains(keywords))
                || SqlFunc.ToString(x.TaxYear).Contains(keywords)
                || SqlFunc.ToString(x.TaxThreshold).Contains(keywords)
                || SqlFunc.ToString(x.TaxableIncomeMin).Contains(keywords)
                || SqlFunc.ToString(x.TaxableIncomeMax).Contains(keywords)
                || SqlFunc.ToString(x.TaxRate).Contains(keywords)
                || SqlFunc.ToString(x.QuickDeduction).Contains(keywords)
                || SqlFunc.ToString(x.SpecialDeductionStandard).Contains(keywords)
                || SqlFunc.ToString(x.SocialSecurityDeductionRate).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundDeductionRate).Contains(keywords)
                || (x.CalculationFormula != null && x.CalculationFormula.Contains(keywords))
                || (x.Description != null && x.Description.Contains(keywords))
                || SqlFunc.ToString(x.TaxCalcStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
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

        if (queryDto?.TaxYear.HasValue == true)
        {
            exp = exp.And(x => x.TaxYear == queryDto.TaxYear);
        }

        if (queryDto?.TaxThreshold.HasValue == true)
        {
            exp = exp.And(x => x.TaxThreshold == queryDto.TaxThreshold);
        }

        if (queryDto?.TaxableIncomeMin.HasValue == true)
        {
            exp = exp.And(x => x.TaxableIncomeMin == queryDto.TaxableIncomeMin);
        }

        if (queryDto?.TaxableIncomeMax.HasValue == true)
        {
            exp = exp.And(x => x.TaxableIncomeMax == queryDto.TaxableIncomeMax);
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            exp = exp.And(x => x.TaxRate == queryDto.TaxRate);
        }

        if (queryDto?.QuickDeduction.HasValue == true)
        {
            exp = exp.And(x => x.QuickDeduction == queryDto.QuickDeduction);
        }

        if (queryDto?.SpecialDeductionStandard.HasValue == true)
        {
            exp = exp.And(x => x.SpecialDeductionStandard == queryDto.SpecialDeductionStandard);
        }

        if (queryDto?.SocialSecurityDeductionRate.HasValue == true)
        {
            exp = exp.And(x => x.SocialSecurityDeductionRate == queryDto.SocialSecurityDeductionRate);
        }

        if (queryDto?.HousingFundDeductionRate.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundDeductionRate == queryDto.HousingFundDeductionRate);
        }

        if (!string.IsNullOrEmpty(queryDto?.CalculationFormula))
        {
            exp = exp.And(x => x.CalculationFormula != null && x.CalculationFormula.Contains(queryDto.CalculationFormula));
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (queryDto?.TaxCalcStatus.HasValue == true)
        {
            exp = exp.And(x => x.TaxCalcStatus == queryDto.TaxCalcStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
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
