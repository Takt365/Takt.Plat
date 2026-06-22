// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktVendorService.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：经销商信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 经销商信息应用服务
/// </summary>
public class TaktVendorService : TaktServiceBase, ITaktVendorService
{
    private readonly ITaktCompanyRepository<TaktVendor> _vendorRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vendorRepository">经销商信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktVendorService(
        ITaktCompanyRepository<TaktVendor> vendorRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _vendorRepository = vendorRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取经销商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktVendorDto>> GetVendorListAsync(TaktVendorQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _vendorRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktVendorDto>.Create(
            data.Adapt<List<TaktVendorDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktVendorDto?> GetVendorByIdAsync(long id)
    {
        var entity = await _vendorRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktVendorDto>();
    }

    /// <summary>
    /// 获取经销商信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetVendorOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _vendorRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.VendorStatus == 1,
            x => x.VendorName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.VendorName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建经销商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVendorDto> CreateVendorAsync(TaktVendorCreateDto dto)
    {
        var entity = dto.Adapt<TaktVendor>();
        var isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique = await _uniqueValidator.IsUniqueAsync(
            _vendorRepository,
            x => x.VendorCode == entity.VendorCode);
        if (!isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique)
        {
            throw new TaktBusinessException("经销商信息的VendorCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _vendorRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _vendorRepository.CreateAsync(entity);
        return await GetVendorByIdAsync(entity.Id) ?? entity.Adapt<TaktVendorDto>();
    }

    /// <summary>
    /// 更新经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVendorDto> UpdateVendorAsync(long id, TaktVendorUpdateDto dto)
    {
        var entity = await _vendorRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("经销商信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique = await _uniqueValidator.IsUniqueAsync(
            _vendorRepository,
            x => x.VendorCode == entity.VendorCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique)
        {
            throw new TaktBusinessException("经销商信息的VendorCode已存在");
        }
        await _vendorRepository.UpdateAsync(entity);
        return await GetVendorByIdAsync(id) ?? throw new TaktBusinessException("经销商信息不存在");
    }

    /// <summary>
    /// 删除经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteVendorByIdAsync(long id)
    {
        var deleted = await _vendorRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("经销商信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除经销商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteVendorBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteVendorByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新经销商信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVendorDto> UpdateVendorStatusAsync(TaktVendorStatusDto dto)
    {
        var entity = await _vendorRepository.GetByIdAsync(dto.VendorId);
        if (entity == null)
        {
            throw new TaktBusinessException("经销商信息不存在");
        }
        entity.VendorStatus = dto.VendorStatus;
        await _vendorRepository.UpdateAsync(entity);
        return await GetVendorByIdAsync(dto.VendorId) ?? throw new TaktBusinessException("经销商信息不存在");
    }

    /// <summary>
    /// 更新经销商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVendorDto> UpdateVendorSortAsync(TaktVendorSortDto dto)
    {
        var entity = await _vendorRepository.GetByIdAsync(dto.VendorId);
        if (entity == null)
        {
            throw new TaktBusinessException("经销商信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _vendorRepository.UpdateAsync(entity);
        return await GetVendorByIdAsync(dto.VendorId) ?? throw new TaktBusinessException("经销商信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetVendorTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktVendorTemplateDto>(
            sheetName ?? "经销商信息导入模板",
            fileName ?? "经销商信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入经销商信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportVendorAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktVendorImportDto>(fileStream, sheetName ?? "经销商信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _vendorRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktVendor>();
                var importKey = $"{entity.VendorCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（VendorCode）");
                }
                var isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _vendorRepository,
                    x => x.VendorCode == entity.VendorCode);
                if (!isUnique_ix_takt_logistics_materials_vendor_vendor_code_unique)
                {
                    throw new TaktBusinessException("经销商信息的VendorCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _vendorRepository.CreateAsync(entity);
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
    /// 导出经销商信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportVendorAsync(TaktVendorQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktVendorQueryDto());
        var list = await _vendorRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktVendorExportDto>(),
                sheetName ?? "经销商信息数据",
                fileName ?? "经销商信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktVendorExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "经销商信息数据",
            fileName ?? "经销商信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建经销商信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktVendor, bool>> QueryExpression(TaktVendorQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktVendor>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.VendorCode != null && x.VendorCode.Contains(keywords))
                || (x.VendorName != null && x.VendorName.Contains(keywords))
                || (x.VendorShortName != null && x.VendorShortName.Contains(keywords))
                || SqlFunc.ToString(x.VendorType).Contains(keywords)
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.VendorTaxNumber != null && x.VendorTaxNumber.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.VendorPhone != null && x.VendorPhone.Contains(keywords))
                || (x.VendorFax != null && x.VendorFax.Contains(keywords))
                || (x.VendorEmail != null && x.VendorEmail.Contains(keywords))
                || (x.VendorWebsite != null && x.VendorWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PaymentTerms).Contains(keywords)
                || SqlFunc.ToString(x.CreditLevel).Contains(keywords)
                || SqlFunc.ToString(x.CreditAmount).Contains(keywords)
                || (x.AuthorizedBrand != null && x.AuthorizedBrand.Contains(keywords))
                || (x.AgentRegion != null && x.AgentRegion.Contains(keywords))
                || SqlFunc.ToString(x.VendorLevel).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationScore).Contains(keywords)
                || SqlFunc.ToString(x.IsQualified).Contains(keywords)
                || SqlFunc.ToString(x.VendorStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorCode))
        {
            exp = exp.And(x => x.VendorCode != null && x.VendorCode.Contains(queryDto.VendorCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorName))
        {
            exp = exp.And(x => x.VendorName != null && x.VendorName.Contains(queryDto.VendorName));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorShortName))
        {
            exp = exp.And(x => x.VendorShortName != null && x.VendorShortName.Contains(queryDto.VendorShortName));
        }

        if (queryDto?.VendorType.HasValue == true)
        {
            exp = exp.And(x => x.VendorType == queryDto.VendorType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorTaxNumber))
        {
            exp = exp.And(x => x.VendorTaxNumber != null && x.VendorTaxNumber.Contains(queryDto.VendorTaxNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationCountry))
        {
            exp = exp.And(x => x.RegistrationCountry != null && x.RegistrationCountry.Contains(queryDto.RegistrationCountry));
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

        if (!string.IsNullOrEmpty(queryDto?.VendorPhone))
        {
            exp = exp.And(x => x.VendorPhone != null && x.VendorPhone.Contains(queryDto.VendorPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorFax))
        {
            exp = exp.And(x => x.VendorFax != null && x.VendorFax.Contains(queryDto.VendorFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorEmail))
        {
            exp = exp.And(x => x.VendorEmail != null && x.VendorEmail.Contains(queryDto.VendorEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.VendorWebsite))
        {
            exp = exp.And(x => x.VendorWebsite != null && x.VendorWebsite.Contains(queryDto.VendorWebsite));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPerson))
        {
            exp = exp.And(x => x.ContactPerson != null && x.ContactPerson.Contains(queryDto.ContactPerson));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.PaymentTerms.HasValue == true)
        {
            exp = exp.And(x => x.PaymentTerms == queryDto.PaymentTerms);
        }

        if (queryDto?.CreditLevel.HasValue == true)
        {
            exp = exp.And(x => x.CreditLevel == queryDto.CreditLevel);
        }

        if (queryDto?.CreditAmount.HasValue == true)
        {
            exp = exp.And(x => x.CreditAmount == queryDto.CreditAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.AuthorizedBrand))
        {
            exp = exp.And(x => x.AuthorizedBrand != null && x.AuthorizedBrand.Contains(queryDto.AuthorizedBrand));
        }

        if (!string.IsNullOrEmpty(queryDto?.AgentRegion))
        {
            exp = exp.And(x => x.AgentRegion != null && x.AgentRegion.Contains(queryDto.AgentRegion));
        }

        if (queryDto?.VendorLevel.HasValue == true)
        {
            exp = exp.And(x => x.VendorLevel == queryDto.VendorLevel);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationScore == queryDto.EvaluationScore);
        }

        if (queryDto?.IsQualified.HasValue == true)
        {
            exp = exp.And(x => x.IsQualified == queryDto.IsQualified);
        }

        if (queryDto?.VendorStatus.HasValue == true)
        {
            exp = exp.And(x => x.VendorStatus == queryDto.VendorStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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
