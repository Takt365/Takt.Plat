// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.CompensationBenefits
// 文件名称：TaktSocialSecurityService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：社保缴纳应用服务实现
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
/// 社保缴纳应用服务
/// </summary>
public class TaktSocialSecurityService : TaktServiceBase, ITaktSocialSecurityService
{
    private readonly ITaktCompanyRepository<TaktSocialSecurity> _socialSecurityRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="socialSecurityRepository">社保缴纳仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSocialSecurityService(
        ITaktCompanyRepository<TaktSocialSecurity> socialSecurityRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _socialSecurityRepository = socialSecurityRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取社保缴纳列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSocialSecurityDto>> GetSocialSecurityListAsync(TaktSocialSecurityQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _socialSecurityRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSocialSecurityDto>.Create(
            data.Adapt<List<TaktSocialSecurityDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialSecurityDto?> GetSocialSecurityByIdAsync(long id)
    {
        var entity = await _socialSecurityRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSocialSecurityDto>();
    }

    /// <summary>
    /// 获取社保缴纳选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSocialSecurityOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _socialSecurityRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建社保缴纳
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialSecurityDto> CreateSocialSecurityAsync(TaktSocialSecurityCreateDto dto)
    {
        var entity = dto.Adapt<TaktSocialSecurity>();
        entity = await _socialSecurityRepository.CreateAsync(entity);
        return await GetSocialSecurityByIdAsync(entity.Id) ?? entity.Adapt<TaktSocialSecurityDto>();
    }

    /// <summary>
    /// 更新社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialSecurityDto> UpdateSocialSecurityAsync(long id, TaktSocialSecurityUpdateDto dto)
    {
        var entity = await _socialSecurityRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("社保缴纳不存在");
        }
        dto.Adapt(entity);
        await _socialSecurityRepository.UpdateAsync(entity);
        return await GetSocialSecurityByIdAsync(id) ?? throw new TaktBusinessException("社保缴纳不存在");
    }

    /// <summary>
    /// 删除社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSocialSecurityByIdAsync(long id)
    {
        var deleted = await _socialSecurityRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("社保缴纳不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除社保缴纳
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSocialSecurityBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSocialSecurityByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新社保缴纳状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSocialSecurityDto> UpdateSocialSecurityStatusAsync(TaktSocialSecurityStatusDto dto)
    {
        var entity = await _socialSecurityRepository.GetByIdAsync(dto.SocialSecurityId);
        if (entity == null)
        {
            throw new TaktBusinessException("社保缴纳不存在");
        }
        entity.PayStatus = dto.PayStatus;
        await _socialSecurityRepository.UpdateAsync(entity);
        return await GetSocialSecurityByIdAsync(dto.SocialSecurityId) ?? throw new TaktBusinessException("社保缴纳不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSocialSecurityTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSocialSecurityTemplateDto>(
            sheetName ?? "社保缴纳导入模板",
            fileName ?? "社保缴纳导入模板.xlsx");
    }

    /// <summary>
    /// 导入社保缴纳
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSocialSecurityAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSocialSecurityImportDto>(fileStream, sheetName ?? "社保缴纳导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSocialSecurity>();
                await _socialSecurityRepository.CreateAsync(entity);
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
    /// 导出社保缴纳
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSocialSecurityAsync(TaktSocialSecurityQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSocialSecurityQueryDto());
        var list = await _socialSecurityRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSocialSecurityExportDto>(),
                sheetName ?? "社保缴纳数据",
                fileName ?? "社保缴纳导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSocialSecurityExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "社保缴纳数据",
            fileName ?? "社保缴纳导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建社保缴纳查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSocialSecurity, bool>> QueryExpression(TaktSocialSecurityQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSocialSecurity>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.PayPeriod != null && x.PayPeriod.Contains(keywords))
                || SqlFunc.ToString(x.SocialSecurityBase).Contains(keywords)
                || SqlFunc.ToString(x.PensionAmount).Contains(keywords)
                || SqlFunc.ToString(x.MedicalAmount).Contains(keywords)
                || SqlFunc.ToString(x.UnemploymentAmount).Contains(keywords)
                || SqlFunc.ToString(x.InjuryAmount).Contains(keywords)
                || SqlFunc.ToString(x.MaternityAmount).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundBase).Contains(keywords)
                || SqlFunc.ToString(x.HousingFundAmount).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.PayStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PayPeriod))
        {
            exp = exp.And(x => x.PayPeriod != null && x.PayPeriod.Contains(queryDto.PayPeriod));
        }

        if (queryDto?.SocialSecurityBase.HasValue == true)
        {
            exp = exp.And(x => x.SocialSecurityBase == queryDto.SocialSecurityBase);
        }

        if (queryDto?.PensionAmount.HasValue == true)
        {
            exp = exp.And(x => x.PensionAmount == queryDto.PensionAmount);
        }

        if (queryDto?.MedicalAmount.HasValue == true)
        {
            exp = exp.And(x => x.MedicalAmount == queryDto.MedicalAmount);
        }

        if (queryDto?.UnemploymentAmount.HasValue == true)
        {
            exp = exp.And(x => x.UnemploymentAmount == queryDto.UnemploymentAmount);
        }

        if (queryDto?.InjuryAmount.HasValue == true)
        {
            exp = exp.And(x => x.InjuryAmount == queryDto.InjuryAmount);
        }

        if (queryDto?.MaternityAmount.HasValue == true)
        {
            exp = exp.And(x => x.MaternityAmount == queryDto.MaternityAmount);
        }

        if (queryDto?.HousingFundBase.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundBase == queryDto.HousingFundBase);
        }

        if (queryDto?.HousingFundAmount.HasValue == true)
        {
            exp = exp.And(x => x.HousingFundAmount == queryDto.HousingFundAmount);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.PayStatus.HasValue == true)
        {
            exp = exp.And(x => x.PayStatus == queryDto.PayStatus);
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
