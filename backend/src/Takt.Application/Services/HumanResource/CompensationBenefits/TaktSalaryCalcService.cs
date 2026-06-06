// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.CompensationBenefits
// 文件名称：TaktSalaryCalcService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资核算应用服务实现
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
/// 薪资核算应用服务
/// </summary>
public class TaktSalaryCalcService : TaktServiceBase, ITaktSalaryCalcService
{
    private readonly ITaktCompanyRepository<TaktSalaryCalc> _salaryCalcRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryCalcRepository">薪资核算仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalaryCalcService(
        ITaktCompanyRepository<TaktSalaryCalc> salaryCalcRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salaryCalcRepository = salaryCalcRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取薪资核算列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalaryCalcDto>> GetSalaryCalcListAsync(TaktSalaryCalcQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salaryCalcRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalaryCalcDto>.Create(
            data.Adapt<List<TaktSalaryCalcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryCalcDto?> GetSalaryCalcByIdAsync(long id)
    {
        var entity = await _salaryCalcRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalaryCalcDto>();
    }

    /// <summary>
    /// 获取薪资核算选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalaryCalcOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salaryCalcRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CalcName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CalcName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建薪资核算
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryCalcDto> CreateSalaryCalcAsync(TaktSalaryCalcCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalaryCalc>();
        var isUnique_ix_salary_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryCalcRepository,
            x => x.CalcCode == entity.CalcCode);
        if (!isUnique_ix_salary_calc_code_unique)
        {
            throw new TaktBusinessException("薪资核算的CalcCode已存在");
        }
        entity = await _salaryCalcRepository.CreateAsync(entity);
        return await GetSalaryCalcByIdAsync(entity.Id) ?? entity.Adapt<TaktSalaryCalcDto>();
    }

    /// <summary>
    /// 更新薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryCalcDto> UpdateSalaryCalcAsync(long id, TaktSalaryCalcUpdateDto dto)
    {
        var entity = await _salaryCalcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资核算不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_salary_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryCalcRepository,
            x => x.CalcCode == entity.CalcCode,
            id);
        if (!isUnique_ix_salary_calc_code_unique)
        {
            throw new TaktBusinessException("薪资核算的CalcCode已存在");
        }
        await _salaryCalcRepository.UpdateAsync(entity);
        return await GetSalaryCalcByIdAsync(id) ?? throw new TaktBusinessException("薪资核算不存在");
    }

    /// <summary>
    /// 删除薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryCalcByIdAsync(long id)
    {
        var deleted = await _salaryCalcRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("薪资核算不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除薪资核算
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryCalcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalaryCalcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新薪资核算状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryCalcDto> UpdateSalaryCalcStatusAsync(TaktSalaryCalcStatusDto dto)
    {
        var entity = await _salaryCalcRepository.GetByIdAsync(dto.SalaryCalcId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资核算不存在");
        }
        entity.CalcStatus = dto.CalcStatus;
        await _salaryCalcRepository.UpdateAsync(entity);
        return await GetSalaryCalcByIdAsync(dto.SalaryCalcId) ?? throw new TaktBusinessException("薪资核算不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalaryCalcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalaryCalcTemplateDto>(
            sheetName ?? "薪资核算导入模板",
            fileName ?? "薪资核算导入模板.xlsx");
    }

    /// <summary>
    /// 导入薪资核算
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalaryCalcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalaryCalcImportDto>(fileStream, sheetName ?? "薪资核算导入模板");
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
                var entity = rows[i].Adapt<TaktSalaryCalc>();
                var importKey = $"{entity.CalcCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CalcCode）");
                }
                var isUnique_ix_salary_calc_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salaryCalcRepository,
                    x => x.CalcCode == entity.CalcCode);
                if (!isUnique_ix_salary_calc_code_unique)
                {
                    throw new TaktBusinessException("薪资核算的CalcCode已存在");
                }
                await _salaryCalcRepository.CreateAsync(entity);
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
    /// 导出薪资核算
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalaryCalcAsync(TaktSalaryCalcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalaryCalcQueryDto());
        var list = await _salaryCalcRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalaryCalcExportDto>(),
                sheetName ?? "薪资核算数据",
                fileName ?? "薪资核算导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalaryCalcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "薪资核算数据",
            fileName ?? "薪资核算导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建薪资核算查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalaryCalc, bool>> QueryExpression(TaktSalaryCalcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalaryCalc>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CalcCode != null && x.CalcCode.Contains(keywords))
                || (x.CalcName != null && x.CalcName.Contains(keywords))
                || (x.PayPeriod != null && x.PayPeriod.Contains(keywords))
                || SqlFunc.ToString(x.EmployeeCount).Contains(keywords)
                || SqlFunc.ToString(x.GrossAmount).Contains(keywords)
                || SqlFunc.ToString(x.NetAmount).Contains(keywords)
                || SqlFunc.ToString(x.CalcStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CalcDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CalcCode))
        {
            exp = exp.And(x => x.CalcCode != null && x.CalcCode.Contains(queryDto.CalcCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CalcName))
        {
            exp = exp.And(x => x.CalcName != null && x.CalcName.Contains(queryDto.CalcName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PayPeriod))
        {
            exp = exp.And(x => x.PayPeriod != null && x.PayPeriod.Contains(queryDto.PayPeriod));
        }

        if (queryDto?.EmployeeCount.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeCount == queryDto.EmployeeCount);
        }

        if (queryDto?.GrossAmount.HasValue == true)
        {
            exp = exp.And(x => x.GrossAmount == queryDto.GrossAmount);
        }

        if (queryDto?.NetAmount.HasValue == true)
        {
            exp = exp.And(x => x.NetAmount == queryDto.NetAmount);
        }

        if (queryDto?.CalcStatus.HasValue == true)
        {
            exp = exp.And(x => x.CalcStatus == queryDto.CalcStatus);
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

        if (queryDto?.CalcDateStart.HasValue == true)
        {
            exp = exp.And(x => x.CalcDate >= queryDto.CalcDateStart);
        }

        if (queryDto?.CalcDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.CalcDate <= queryDto.CalcDateEnd);
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
