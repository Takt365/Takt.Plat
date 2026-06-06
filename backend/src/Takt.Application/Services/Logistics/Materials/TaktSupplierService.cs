// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktSupplierService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：供货商信息应用服务实现
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
/// 供货商信息应用服务
/// </summary>
public class TaktSupplierService : TaktServiceBase, ITaktSupplierService
{
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierRepository">供货商信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierService(
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierRepository = supplierRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取供货商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSupplierDto>> GetSupplierListAsync(TaktSupplierQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _supplierRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSupplierDto>.Create(
            data.Adapt<List<TaktSupplierDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto?> GetSupplierByIdAsync(long id)
    {
        var entity = await _supplierRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSupplierDto>();
    }

    /// <summary>
    /// 获取供货商信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSupplierOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _supplierRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SupplierName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SupplierName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建供货商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> CreateSupplierAsync(TaktSupplierCreateDto dto)
    {
        var entity = dto.Adapt<TaktSupplier>();
        var isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierRepository,
            x => x.SupplierCode == entity.SupplierCode);
        if (!isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的SupplierCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _supplierRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _supplierRepository.CreateAsync(entity);
        return await GetSupplierByIdAsync(entity.Id) ?? entity.Adapt<TaktSupplierDto>();
    }

    /// <summary>
    /// 更新供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierAsync(long id, TaktSupplierUpdateDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierRepository,
            x => x.SupplierCode == entity.SupplierCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的SupplierCode已存在");
        }
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(id) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 删除供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierByIdAsync(long id)
    {
        var deleted = await _supplierRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("供货商信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除供货商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSupplierByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新供货商信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierStatusAsync(TaktSupplierStatusDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(dto.SupplierId);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        entity.SupplierStatus = dto.SupplierStatus;
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(dto.SupplierId) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 更新供货商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierSortAsync(TaktSupplierSortDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(dto.SupplierId);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(dto.SupplierId) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSupplierTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSupplierTemplateDto>(
            sheetName ?? "供货商信息导入模板",
            fileName ?? "供货商信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入供货商信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSupplierAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSupplierImportDto>(fileStream, sheetName ?? "供货商信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _supplierRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSupplier>();
                var importKey = $"{entity.SupplierCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SupplierCode）");
                }
                var isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierRepository,
                    x => x.SupplierCode == entity.SupplierCode);
                if (!isUnique_ix_takt_logistics_materials_supplier_supplier_code_unique)
                {
                    throw new TaktBusinessException("供货商信息的SupplierCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _supplierRepository.CreateAsync(entity);
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
    /// 导出供货商信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSupplierAsync(TaktSupplierQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSupplierQueryDto());
        var list = await _supplierRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierExportDto>(),
                sheetName ?? "供货商信息数据",
                fileName ?? "供货商信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSupplierExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "供货商信息数据",
            fileName ?? "供货商信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建供货商信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSupplier, bool>> QueryExpression(TaktSupplierQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSupplier>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName != null && x.SupplierName.Contains(keywords))
                || (x.SupplierShortName != null && x.SupplierShortName.Contains(keywords))
                || SqlFunc.ToString(x.SupplierType).Contains(keywords)
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.SupplierTaxNumber != null && x.SupplierTaxNumber.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.SupplierPhone != null && x.SupplierPhone.Contains(keywords))
                || (x.SupplierFax != null && x.SupplierFax.Contains(keywords))
                || (x.SupplierEmail != null && x.SupplierEmail.Contains(keywords))
                || (x.SupplierWebsite != null && x.SupplierWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PaymentTerms).Contains(keywords)
                || SqlFunc.ToString(x.SupplierLevel).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationScore).Contains(keywords)
                || SqlFunc.ToString(x.IsQualified).Contains(keywords)
                || SqlFunc.ToString(x.SupplierStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierName))
        {
            exp = exp.And(x => x.SupplierName != null && x.SupplierName.Contains(queryDto.SupplierName));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierShortName))
        {
            exp = exp.And(x => x.SupplierShortName != null && x.SupplierShortName.Contains(queryDto.SupplierShortName));
        }

        if (queryDto?.SupplierType.HasValue == true)
        {
            exp = exp.And(x => x.SupplierType == queryDto.SupplierType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierTaxNumber))
        {
            exp = exp.And(x => x.SupplierTaxNumber != null && x.SupplierTaxNumber.Contains(queryDto.SupplierTaxNumber));
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

        if (!string.IsNullOrEmpty(queryDto?.SupplierPhone))
        {
            exp = exp.And(x => x.SupplierPhone != null && x.SupplierPhone.Contains(queryDto.SupplierPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierFax))
        {
            exp = exp.And(x => x.SupplierFax != null && x.SupplierFax.Contains(queryDto.SupplierFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierEmail))
        {
            exp = exp.And(x => x.SupplierEmail != null && x.SupplierEmail.Contains(queryDto.SupplierEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierWebsite))
        {
            exp = exp.And(x => x.SupplierWebsite != null && x.SupplierWebsite.Contains(queryDto.SupplierWebsite));
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

        if (queryDto?.SupplierLevel.HasValue == true)
        {
            exp = exp.And(x => x.SupplierLevel == queryDto.SupplierLevel);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationScore == queryDto.EvaluationScore);
        }

        if (queryDto?.IsQualified.HasValue == true)
        {
            exp = exp.And(x => x.IsQualified == queryDto.IsQualified);
        }

        if (queryDto?.SupplierStatus.HasValue == true)
        {
            exp = exp.And(x => x.SupplierStatus == queryDto.SupplierStatus);
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
