// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCompanyService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;
using Takt.Application.Services.Identity;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 公司应用服务
/// </summary>
public class TaktCompanyService : TaktServiceBase, ITaktCompanyService
{
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCompanyService(
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _companyRepository = companyRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取公司列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCompanyDto>> GetCompanyListAsync(TaktCompanyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _companyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCompanyDto>.Create(
            data.Adapt<List<TaktCompanyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto?> GetCompanyByIdAsync(long id)
    {
        var entity = await _companyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCompanyDto>();
        return dto;    }

    /// <summary>
    /// 获取公司选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCompanyOptionsAsync()
    {
        var list = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.CompanyName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CompanyCode,
            DictLabel = $"{e.CompanyName ?? e.CompanyCode}({e.DefaultCulture})",
        }).ToList();
    }

    /// <summary>
    /// 创建公司
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> CreateCompanyAsync(TaktCompanyCreateDto dto)
    {
        var entity = dto.Adapt<TaktCompany>();
        var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
            _companyRepository,
            x => x.CompanyCode == entity.CompanyCode);
        if (!isUnique_ix_company_code_unique)
        {
            throw new TaktBusinessException("公司的CompanyCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _companyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _companyRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleCompanyIdsAsync(roleId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignRoleCompaniesAsync(roleId, codes.ToArray());
            }
        }
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserCompanyIdsAsync(userId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignUserCompaniesAsync(userId, codes.ToArray());
            }
        }
        return await GetCompanyByIdAsync(entity.Id) ?? entity.Adapt<TaktCompanyDto>();
    }

    /// <summary>
    /// 更新公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanyAsync(long id, TaktCompanyUpdateDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
            _companyRepository,
            x => x.CompanyCode == entity.CompanyCode,
            id);
        if (!isUnique_ix_company_code_unique)
        {
            throw new TaktBusinessException("公司的CompanyCode已存在");
        }
        await _companyRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleCompanyIdsAsync(roleId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignRoleCompaniesAsync(roleId, codes.ToArray());
            }
        }
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserCompanyIdsAsync(userId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignUserCompaniesAsync(userId, codes.ToArray());
            }
        }
        return await GetCompanyByIdAsync(id) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 删除公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCompanyByIdAsync(long id)
    {
        var deleted = await _companyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("公司不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除公司
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCompanyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCompanyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新公司状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanyStatusAsync(TaktCompanyStatusDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        entity.CompanyStatus = dto.CompanyStatus;
        await _companyRepository.UpdateAsync(entity);
        return await GetCompanyByIdAsync(dto.CompanyId) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 更新公司排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanySortAsync(TaktCompanySortDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _companyRepository.UpdateAsync(entity);
        return await GetCompanyByIdAsync(dto.CompanyId) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCompanyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCompanyTemplateDto>(
            sheetName ?? "公司导入模板",
            fileName ?? "公司导入模板.xlsx");
    }

    /// <summary>
    /// 导入公司
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCompanyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCompanyImportDto>(fileStream, sheetName ?? "公司导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _companyRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCompany>();
                var importKey = $"{entity.CompanyCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CompanyCode）");
                }
                var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _companyRepository,
                    x => x.CompanyCode == entity.CompanyCode);
                if (!isUnique_ix_company_code_unique)
                {
                    throw new TaktBusinessException("公司的CompanyCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _companyRepository.CreateAsync(entity);
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
    /// 导出公司
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCompanyAsync(TaktCompanyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCompanyQueryDto());
        var list = await _companyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCompanyExportDto>(),
                sheetName ?? "公司数据",
                fileName ?? "公司导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCompanyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "公司数据",
            fileName ?? "公司导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建公司查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCompany, bool>> QueryExpression(TaktCompanyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCompany>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CompanyName != null && x.CompanyName.Contains(keywords))
                || (x.CompanyShortName != null && x.CompanyShortName.Contains(keywords))
                || SqlFunc.ToString(x.CompanyType).Contains(keywords)
                || SqlFunc.ToString(x.EnterpriseNature).Contains(keywords)
                || SqlFunc.ToString(x.IndustryAttribute).Contains(keywords)
                || SqlFunc.ToString(x.EnterpriseScale).Contains(keywords)
                || (x.BusinessScope != null && x.BusinessScope.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.RegistrationRegion != null && x.RegistrationRegion.Contains(keywords))
                || (x.RegistrationProvince != null && x.RegistrationProvince.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.BusinessRegion != null && x.BusinessRegion.Contains(keywords))
                || (x.BusinessProvince != null && x.BusinessProvince.Contains(keywords))
                || (x.BusinessCity != null && x.BusinessCity.Contains(keywords))
                || (x.BusinessAddress1 != null && x.BusinessAddress1.Contains(keywords))
                || (x.BusinessAddress2 != null && x.BusinessAddress2.Contains(keywords))
                || (x.BusinessAddress3 != null && x.BusinessAddress3.Contains(keywords))
                || (x.CompanyPhone != null && x.CompanyPhone.Contains(keywords))
                || (x.CompanyEmail != null && x.CompanyEmail.Contains(keywords))
                || (x.CompanyFax != null && x.CompanyFax.Contains(keywords))
                || (x.CompanyWebsite != null && x.CompanyWebsite.Contains(keywords))
                || (x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(keywords))
                || (x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(keywords))
                || (x.LegalRepresentative != null && x.LegalRepresentative.Contains(keywords))
                || (x.CompanyManager != null && x.CompanyManager.Contains(keywords))
                || SqlFunc.ToString(x.RegisteredCapital).Contains(keywords)
                || SqlFunc.ToString(x.CompanyExistence).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.DefaultCulture != null && x.DefaultCulture.Contains(keywords))
                || (x.CodeAlias != null && x.CodeAlias.Contains(keywords))
                || SqlFunc.ToString(x.CompanyStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EstablishmentDate).Contains(keywords)
                || SqlFunc.ToString(x.ClosingDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyName))
        {
            exp = exp.And(x => x.CompanyName != null && x.CompanyName.Contains(queryDto.CompanyName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyShortName))
        {
            exp = exp.And(x => x.CompanyShortName != null && x.CompanyShortName.Contains(queryDto.CompanyShortName));
        }

        if (queryDto?.CompanyType.HasValue == true)
        {
            exp = exp.And(x => x.CompanyType == queryDto.CompanyType);
        }

        if (queryDto?.EnterpriseNature.HasValue == true)
        {
            exp = exp.And(x => x.EnterpriseNature == queryDto.EnterpriseNature);
        }

        if (queryDto?.IndustryAttribute.HasValue == true)
        {
            exp = exp.And(x => x.IndustryAttribute == queryDto.IndustryAttribute);
        }

        if (queryDto?.EnterpriseScale.HasValue == true)
        {
            exp = exp.And(x => x.EnterpriseScale == queryDto.EnterpriseScale);
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

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress3))
        {
            exp = exp.And(x => x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(queryDto.RegistrationAddress3));
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

        if (!string.IsNullOrEmpty(queryDto?.BusinessAddress3))
        {
            exp = exp.And(x => x.BusinessAddress3 != null && x.BusinessAddress3.Contains(queryDto.BusinessAddress3));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyPhone))
        {
            exp = exp.And(x => x.CompanyPhone != null && x.CompanyPhone.Contains(queryDto.CompanyPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyEmail))
        {
            exp = exp.And(x => x.CompanyEmail != null && x.CompanyEmail.Contains(queryDto.CompanyEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyFax))
        {
            exp = exp.And(x => x.CompanyFax != null && x.CompanyFax.Contains(queryDto.CompanyFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyWebsite))
        {
            exp = exp.And(x => x.CompanyWebsite != null && x.CompanyWebsite.Contains(queryDto.CompanyWebsite));
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

        if (!string.IsNullOrEmpty(queryDto?.CompanyManager))
        {
            exp = exp.And(x => x.CompanyManager != null && x.CompanyManager.Contains(queryDto.CompanyManager));
        }

        if (queryDto?.RegisteredCapital.HasValue == true)
        {
            exp = exp.And(x => x.RegisteredCapital == queryDto.RegisteredCapital);
        }

        if (queryDto?.CompanyExistence.HasValue == true)
        {
            exp = exp.And(x => x.CompanyExistence == queryDto.CompanyExistence);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefaultCulture))
        {
            exp = exp.And(x => x.DefaultCulture != null && x.DefaultCulture.Contains(queryDto.DefaultCulture));
        }

        if (!string.IsNullOrEmpty(queryDto?.CodeAlias))
        {
            exp = exp.And(x => x.CodeAlias != null && x.CodeAlias.Contains(queryDto.CodeAlias));
        }

        if (queryDto?.CompanyStatus.HasValue == true)
        {
            exp = exp.And(x => x.CompanyStatus == queryDto.CompanyStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
