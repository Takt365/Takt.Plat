// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktPackagingService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：物料包装信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料包装信息应用服务
/// </summary>
public class TaktPackagingService : TaktServiceBase, ITaktPackagingService
{
    private readonly ITaktCompanyRepository<TaktPackaging> _packagingRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="packagingRepository">物料包装信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPackagingService(
        ITaktCompanyRepository<TaktPackaging> packagingRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _packagingRepository = packagingRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料包装信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPackagingDto>> GetPackagingListAsync(TaktPackagingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _packagingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPackagingDto>.Create(
            data.Adapt<List<TaktPackagingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingDto?> GetPackagingByIdAsync(long id)
    {
        var entity = await _packagingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPackagingDto>();
    }

    /// <summary>
    /// 获取物料包装信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPackagingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _packagingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.HsName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.HsName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料包装信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingDto> CreatePackagingAsync(TaktPackagingCreateDto dto)
    {
        var entity = dto.Adapt<TaktPackaging>();
        var isUnique_ix_takt_logistics_manufacturing_packaging_unique = await _uniqueValidator.IsUniqueAsync(
            _packagingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_packaging_unique)
        {
            throw new TaktBusinessException("物料包装信息的PlantCode、MaterialCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _packagingRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _packagingRepository.CreateAsync(entity);
        return await GetPackagingByIdAsync(entity.Id) ?? entity.Adapt<TaktPackagingDto>();
    }

    /// <summary>
    /// 更新物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingDto> UpdatePackagingAsync(long id, TaktPackagingUpdateDto dto)
    {
        var entity = await _packagingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料包装信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_packaging_unique = await _uniqueValidator.IsUniqueAsync(
            _packagingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_packaging_unique)
        {
            throw new TaktBusinessException("物料包装信息的PlantCode、MaterialCode已存在");
        }
        await _packagingRepository.UpdateAsync(entity);
        return await GetPackagingByIdAsync(id) ?? throw new TaktBusinessException("物料包装信息不存在");
    }

    /// <summary>
    /// 删除物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>任务</returns>
    public async Task DeletePackagingByIdAsync(long id)
    {
        var deleted = await _packagingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料包装信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料包装信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePackagingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePackagingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料包装信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingDto> UpdatePackagingSortAsync(TaktPackagingSortDto dto)
    {
        var entity = await _packagingRepository.GetByIdAsync(dto.PackagingId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料包装信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _packagingRepository.UpdateAsync(entity);
        return await GetPackagingByIdAsync(dto.PackagingId) ?? throw new TaktBusinessException("物料包装信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPackagingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPackagingTemplateDto>(
            sheetName ?? "物料包装信息导入模板",
            fileName ?? "物料包装信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料包装信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPackagingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPackagingImportDto>(fileStream, sheetName ?? "物料包装信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _packagingRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPackaging>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_packaging_unique = await _uniqueValidator.IsUniqueAsync(
                    _packagingRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_packaging_unique)
                {
                    throw new TaktBusinessException("物料包装信息的PlantCode、MaterialCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _packagingRepository.CreateAsync(entity);
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
    /// 导出物料包装信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPackagingAsync(TaktPackagingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPackagingQueryDto());
        var list = await _packagingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPackagingExportDto>(),
                sheetName ?? "物料包装信息数据",
                fileName ?? "物料包装信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPackagingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料包装信息数据",
            fileName ?? "物料包装信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料包装信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPackaging, bool>> QueryExpression(TaktPackagingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPackaging>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.HsCode != null && x.HsCode.Contains(keywords))
                || (x.HsName != null && x.HsName.Contains(keywords))
                || (x.AdditionalCode != null && x.AdditionalCode.Contains(keywords))
                || (x.OriginCountryRegionCode != null && x.OriginCountryRegionCode.Contains(keywords))
                || (x.OriginCountryRegionName != null && x.OriginCountryRegionName.Contains(keywords))
                || (x.DestinationCountryRegionCode != null && x.DestinationCountryRegionCode.Contains(keywords))
                || (x.DestinationCountryRegionName != null && x.DestinationCountryRegionName.Contains(keywords))
                || (x.RegulatoryConditionCode != null && x.RegulatoryConditionCode.Contains(keywords))
                || (x.TariffRateType != null && x.TariffRateType.Contains(keywords))
                || SqlFunc.ToString(x.GrossWeight).Contains(keywords)
                || SqlFunc.ToString(x.NetWeight).Contains(keywords)
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || SqlFunc.ToString(x.BusinessVolume).Contains(keywords)
                || (x.VolumeUnit != null && x.VolumeUnit.Contains(keywords))
                || (x.SizeDimension != null && x.SizeDimension.Contains(keywords))
                || (x.PackagingType != null && x.PackagingType.Contains(keywords))
                || (x.PackingUnit != null && x.PackingUnit.Contains(keywords))
                || SqlFunc.ToString(x.QuantityPerPacking).Contains(keywords)
                || (x.PackagingSpec != null && x.PackagingSpec.Contains(keywords))
                || (x.PackagingDescription != null && x.PackagingDescription.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.HsCode))
        {
            exp = exp.And(x => x.HsCode != null && x.HsCode.Contains(queryDto.HsCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.HsName))
        {
            exp = exp.And(x => x.HsName != null && x.HsName.Contains(queryDto.HsName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AdditionalCode))
        {
            exp = exp.And(x => x.AdditionalCode != null && x.AdditionalCode.Contains(queryDto.AdditionalCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.OriginCountryRegionCode))
        {
            exp = exp.And(x => x.OriginCountryRegionCode != null && x.OriginCountryRegionCode.Contains(queryDto.OriginCountryRegionCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.OriginCountryRegionName))
        {
            exp = exp.And(x => x.OriginCountryRegionName != null && x.OriginCountryRegionName.Contains(queryDto.OriginCountryRegionName));
        }

        if (!string.IsNullOrEmpty(queryDto?.DestinationCountryRegionCode))
        {
            exp = exp.And(x => x.DestinationCountryRegionCode != null && x.DestinationCountryRegionCode.Contains(queryDto.DestinationCountryRegionCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DestinationCountryRegionName))
        {
            exp = exp.And(x => x.DestinationCountryRegionName != null && x.DestinationCountryRegionName.Contains(queryDto.DestinationCountryRegionName));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegulatoryConditionCode))
        {
            exp = exp.And(x => x.RegulatoryConditionCode != null && x.RegulatoryConditionCode.Contains(queryDto.RegulatoryConditionCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TariffRateType))
        {
            exp = exp.And(x => x.TariffRateType != null && x.TariffRateType.Contains(queryDto.TariffRateType));
        }

        if (queryDto?.GrossWeight.HasValue == true)
        {
            exp = exp.And(x => x.GrossWeight == queryDto.GrossWeight);
        }

        if (queryDto?.NetWeight.HasValue == true)
        {
            exp = exp.And(x => x.NetWeight == queryDto.NetWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.WeightUnit))
        {
            exp = exp.And(x => x.WeightUnit != null && x.WeightUnit.Contains(queryDto.WeightUnit));
        }

        if (queryDto?.BusinessVolume.HasValue == true)
        {
            exp = exp.And(x => x.BusinessVolume == queryDto.BusinessVolume);
        }

        if (!string.IsNullOrEmpty(queryDto?.VolumeUnit))
        {
            exp = exp.And(x => x.VolumeUnit != null && x.VolumeUnit.Contains(queryDto.VolumeUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.SizeDimension))
        {
            exp = exp.And(x => x.SizeDimension != null && x.SizeDimension.Contains(queryDto.SizeDimension));
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingType))
        {
            exp = exp.And(x => x.PackagingType != null && x.PackagingType.Contains(queryDto.PackagingType));
        }

        if (!string.IsNullOrEmpty(queryDto?.PackingUnit))
        {
            exp = exp.And(x => x.PackingUnit != null && x.PackingUnit.Contains(queryDto.PackingUnit));
        }

        if (queryDto?.QuantityPerPacking.HasValue == true)
        {
            exp = exp.And(x => x.QuantityPerPacking == queryDto.QuantityPerPacking);
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingSpec))
        {
            exp = exp.And(x => x.PackagingSpec != null && x.PackagingSpec.Contains(queryDto.PackagingSpec));
        }

        if (!string.IsNullOrEmpty(queryDto?.PackagingDescription))
        {
            exp = exp.And(x => x.PackagingDescription != null && x.PackagingDescription.Contains(queryDto.PackagingDescription));
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
