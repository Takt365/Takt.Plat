// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPlantService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 工厂应用服务
/// </summary>
public class TaktPlantService : TaktServiceBase, ITaktPlantService
{
    private readonly ITaktTenantRepository<TaktPlant> _plantRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="plantRepository">工厂仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPlantService(
        ITaktTenantRepository<TaktPlant> plantRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _plantRepository = plantRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPlantDto>> GetPlantListAsync(TaktPlantQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _plantRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPlantDto>.Create(
            data.Adapt<List<TaktPlantDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto?> GetPlantByIdAsync(long id)
    {
        var entity = await _plantRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktPlantDto>();
    }

    /// <summary>
    /// 获取工厂选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPlantOptionsAsync()
    {
        var list = await _plantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.PlantStatus == 1,
            x => x.PlantShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PlantCode,
            DictLabel = e.PlantShortName ?? e.PlantCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工厂
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> CreatePlantAsync(TaktPlantCreateDto dto)
    {
        var entity = dto.Adapt<TaktPlant>();
        var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _plantRepository,
            x => x.PlantCode == entity.PlantCode);
        if (!isUnique_ix_plant_code_unique)
        {
            throw new TaktBusinessException("工厂的PlantCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _plantRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _plantRepository.CreateAsync(entity);
        return await GetPlantByIdAsync(entity.Id) ?? entity.Adapt<TaktPlantDto>();
    }

    /// <summary>
    /// 更新工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantAsync(long id, TaktPlantUpdateDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _plantRepository,
            x => x.PlantCode == entity.PlantCode,
            id);
        if (!isUnique_ix_plant_code_unique)
        {
            throw new TaktBusinessException("工厂的PlantCode已存在");
        }
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(id) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 删除工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>任务</returns>
    public async Task DeletePlantByIdAsync(long id)
    {
        var deleted = await _plantRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePlantBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePlantByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工厂状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantStatusAsync(TaktPlantStatusDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(dto.PlantId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        entity.PlantStatus = dto.PlantStatus;
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(dto.PlantId) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 更新工厂排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantSortAsync(TaktPlantSortDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(dto.PlantId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(dto.PlantId) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPlantTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPlantTemplateDto>(
            sheetName ?? "工厂导入模板",
            fileName ?? "工厂导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPlantAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPlantImportDto>(fileStream, sheetName ?? "工厂导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _plantRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPlant>();
                var importKey = $"{entity.PlantCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode）");
                }
                var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _plantRepository,
                    x => x.PlantCode == entity.PlantCode);
                if (!isUnique_ix_plant_code_unique)
                {
                    throw new TaktBusinessException("工厂的PlantCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _plantRepository.CreateAsync(entity);
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
    /// 导出工厂
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPlantAsync(TaktPlantQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPlantQueryDto());
        var list = await _plantRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPlantExportDto>(),
                sheetName ?? "工厂数据",
                fileName ?? "工厂导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPlantExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂数据",
            fileName ?? "工厂导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPlant, bool>> QueryExpression(TaktPlantQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPlant>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PlantName1 != null && x.PlantName1.Contains(keywords))
                || (x.PlantName2 != null && x.PlantName2.Contains(keywords))
                || (x.PlantShortName != null && x.PlantShortName.Contains(keywords))
                || (x.CodeAlias != null && x.CodeAlias.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.EnterpriseScale != null && x.EnterpriseScale.Contains(keywords))
                || (x.BusinessScope != null && x.BusinessScope.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationRegion != null && x.RegistrationRegion.Contains(keywords))
                || (x.RegistrationProvince != null && x.RegistrationProvince.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.BusinessRegion != null && x.BusinessRegion.Contains(keywords))
                || (x.BusinessProvince != null && x.BusinessProvince.Contains(keywords))
                || (x.BusinessCity != null && x.BusinessCity.Contains(keywords))
                || (x.BusinessAddress1 != null && x.BusinessAddress1.Contains(keywords))
                || (x.BusinessAddress2 != null && x.BusinessAddress2.Contains(keywords))
                || (x.PlantAddress1 != null && x.PlantAddress1.Contains(keywords))
                || (x.PlantAddress2 != null && x.PlantAddress2.Contains(keywords))
                || (x.PlantPhone != null && x.PlantPhone.Contains(keywords))
                || (x.PlantEmail != null && x.PlantEmail.Contains(keywords))
                || (x.PlantFax != null && x.PlantFax.Contains(keywords))
                || (x.PlantWebsite != null && x.PlantWebsite.Contains(keywords))
                || (x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(keywords))
                || (x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(keywords))
                || (x.LegalRepresentative != null && x.LegalRepresentative.Contains(keywords))
                || (x.PlantManager != null && x.PlantManager.Contains(keywords))
                || SqlFunc.ToString(x.RegisteredCapital).Contains(keywords)
                || SqlFunc.ToString(x.PlantExistence).Contains(keywords)
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.AccountHolder != null && x.AccountHolder.Contains(keywords))
                || (x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.MaterialRequirementsPlanning != null && x.MaterialRequirementsPlanning.Contains(keywords))
                || (x.DistributionChannel != null && x.DistributionChannel.Contains(keywords))
                || (x.IntercompanyBillingProductGroup != null && x.IntercompanyBillingProductGroup.Contains(keywords))
                || (x.TaxIndicator != null && x.TaxIndicator.Contains(keywords))
                || (x.ValuationArea != null && x.ValuationArea.Contains(keywords))
                || (x.PlantVendorNumber != null && x.PlantVendorNumber.Contains(keywords))
                || (x.PlantCustomerNumber != null && x.PlantCustomerNumber.Contains(keywords))
                || (x.FactoryCalendar != null && x.FactoryCalendar.Contains(keywords))
                || (x.RelatedCompany != null && x.RelatedCompany.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.PlantStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EstablishmentDate).Contains(keywords)
                || SqlFunc.ToString(x.ClosingDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantName1))
        {
            exp = exp.And(x => x.PlantName1 != null && x.PlantName1.Contains(queryDto.PlantName1));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantName2))
        {
            exp = exp.And(x => x.PlantName2 != null && x.PlantName2.Contains(queryDto.PlantName2));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantShortName))
        {
            exp = exp.And(x => x.PlantShortName != null && x.PlantShortName.Contains(queryDto.PlantShortName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CodeAlias))
        {
            exp = exp.And(x => x.CodeAlias != null && x.CodeAlias.Contains(queryDto.CodeAlias));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EnterpriseNature))
        {
            exp = exp.And(x => x.EnterpriseNature != null && x.EnterpriseNature.Contains(queryDto.EnterpriseNature));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustryAttribute))
        {
            exp = exp.And(x => x.IndustryAttribute != null && x.IndustryAttribute.Contains(queryDto.IndustryAttribute));
        }

        if (!string.IsNullOrEmpty(queryDto?.EnterpriseScale))
        {
            exp = exp.And(x => x.EnterpriseScale != null && x.EnterpriseScale.Contains(queryDto.EnterpriseScale));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessScope))
        {
            exp = exp.And(x => x.BusinessScope != null && x.BusinessScope.Contains(queryDto.BusinessScope));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress1))
        {
            exp = exp.And(x => x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(queryDto.RegistrationAddress1));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress2))
        {
            exp = exp.And(x => x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(queryDto.RegistrationAddress2));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationRegion))
        {
            exp = exp.And(x => x.RegistrationRegion != null && x.RegistrationRegion.Contains(queryDto.RegistrationRegion));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationProvince))
        {
            exp = exp.And(x => x.RegistrationProvince != null && x.RegistrationProvince.Contains(queryDto.RegistrationProvince));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationCity))
        {
            exp = exp.And(x => x.RegistrationCity != null && x.RegistrationCity.Contains(queryDto.RegistrationCity));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessRegion))
        {
            exp = exp.And(x => x.BusinessRegion != null && x.BusinessRegion.Contains(queryDto.BusinessRegion));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessProvince))
        {
            exp = exp.And(x => x.BusinessProvince != null && x.BusinessProvince.Contains(queryDto.BusinessProvince));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessCity))
        {
            exp = exp.And(x => x.BusinessCity != null && x.BusinessCity.Contains(queryDto.BusinessCity));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessAddress1))
        {
            exp = exp.And(x => x.BusinessAddress1 != null && x.BusinessAddress1.Contains(queryDto.BusinessAddress1));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessAddress2))
        {
            exp = exp.And(x => x.BusinessAddress2 != null && x.BusinessAddress2.Contains(queryDto.BusinessAddress2));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantAddress1))
        {
            exp = exp.And(x => x.PlantAddress1 != null && x.PlantAddress1.Contains(queryDto.PlantAddress1));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantAddress2))
        {
            exp = exp.And(x => x.PlantAddress2 != null && x.PlantAddress2.Contains(queryDto.PlantAddress2));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantPhone))
        {
            exp = exp.And(x => x.PlantPhone != null && x.PlantPhone.Contains(queryDto.PlantPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantEmail))
        {
            exp = exp.And(x => x.PlantEmail != null && x.PlantEmail.Contains(queryDto.PlantEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantFax))
        {
            exp = exp.And(x => x.PlantFax != null && x.PlantFax.Contains(queryDto.PlantFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantWebsite))
        {
            exp = exp.And(x => x.PlantWebsite != null && x.PlantWebsite.Contains(queryDto.PlantWebsite));
        }

        if (!string.IsNullOrEmpty(queryDto?.UnifiedSocialCreditCode))
        {
            exp = exp.And(x => x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(queryDto.UnifiedSocialCreditCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaxRegistrationNumber))
        {
            exp = exp.And(x => x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(queryDto.TaxRegistrationNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.LegalRepresentative))
        {
            exp = exp.And(x => x.LegalRepresentative != null && x.LegalRepresentative.Contains(queryDto.LegalRepresentative));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantManager))
        {
            exp = exp.And(x => x.PlantManager != null && x.PlantManager.Contains(queryDto.PlantManager));
        }

        if (queryDto?.RegisteredCapital.HasValue == true)
        {
            exp = exp.And(x => x.RegisteredCapital == queryDto.RegisteredCapital);
        }

        if (queryDto?.PlantExistence.HasValue == true)
        {
            exp = exp.And(x => x.PlantExistence == queryDto.PlantExistence);
        }

        if (!string.IsNullOrEmpty(queryDto?.BankCode))
        {
            exp = exp.And(x => x.BankCode != null && x.BankCode.Contains(queryDto.BankCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankAccount))
        {
            exp = exp.And(x => x.BankAccount != null && x.BankAccount.Contains(queryDto.BankAccount));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountHolder))
        {
            exp = exp.And(x => x.AccountHolder != null && x.AccountHolder.Contains(queryDto.AccountHolder));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasingOrganization))
        {
            exp = exp.And(x => x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(queryDto.PurchasingOrganization));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesOrganization))
        {
            exp = exp.And(x => x.SalesOrganization != null && x.SalesOrganization.Contains(queryDto.SalesOrganization));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialRequirementsPlanning))
        {
            exp = exp.And(x => x.MaterialRequirementsPlanning != null && x.MaterialRequirementsPlanning.Contains(queryDto.MaterialRequirementsPlanning));
        }

        if (!string.IsNullOrEmpty(queryDto?.DistributionChannel))
        {
            exp = exp.And(x => x.DistributionChannel != null && x.DistributionChannel.Contains(queryDto.DistributionChannel));
        }

        if (!string.IsNullOrEmpty(queryDto?.IntercompanyBillingProductGroup))
        {
            exp = exp.And(x => x.IntercompanyBillingProductGroup != null && x.IntercompanyBillingProductGroup.Contains(queryDto.IntercompanyBillingProductGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.TaxIndicator))
        {
            exp = exp.And(x => x.TaxIndicator != null && x.TaxIndicator.Contains(queryDto.TaxIndicator));
        }

        if (!string.IsNullOrEmpty(queryDto?.ValuationArea))
        {
            exp = exp.And(x => x.ValuationArea != null && x.ValuationArea.Contains(queryDto.ValuationArea));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantVendorNumber))
        {
            exp = exp.And(x => x.PlantVendorNumber != null && x.PlantVendorNumber.Contains(queryDto.PlantVendorNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCustomerNumber))
        {
            exp = exp.And(x => x.PlantCustomerNumber != null && x.PlantCustomerNumber.Contains(queryDto.PlantCustomerNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.FactoryCalendar))
        {
            exp = exp.And(x => x.FactoryCalendar != null && x.FactoryCalendar.Contains(queryDto.FactoryCalendar));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedCompany))
        {
            exp = exp.And(x => x.RelatedCompany != null && x.RelatedCompany.Contains(queryDto.RelatedCompany));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.PlantStatus.HasValue == true)
        {
            exp = exp.And(x => x.PlantStatus == queryDto.PlantStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EstablishmentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EstablishmentDate >= queryDto.EstablishmentDateStart);
        }

        if (queryDto?.EstablishmentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EstablishmentDate <= queryDto.EstablishmentDateEnd);
        }

        if (queryDto?.ClosingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ClosingDate >= queryDto.ClosingDateStart);
        }

        if (queryDto?.ClosingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ClosingDate <= queryDto.ClosingDateEnd);
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
