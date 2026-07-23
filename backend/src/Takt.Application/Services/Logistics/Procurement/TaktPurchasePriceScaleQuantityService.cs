// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleQuantityService.cs
// 创建时间：2026-07-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格数量等级应用服务实现
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
/// 采购价格数量等级应用服务
/// </summary>
public class TaktPurchasePriceScaleQuantityService : TaktServiceBase, ITaktPurchasePriceScaleQuantityService
{
    private readonly ITaktCompanyRepository<TaktPurchasePriceScaleQuantity> _purchasePriceScaleQuantityRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceScaleQuantityRepository">采购价格数量等级仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceScaleQuantityService(
        ITaktCompanyRepository<TaktPurchasePriceScaleQuantity> purchasePriceScaleQuantityRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceScaleQuantityRepository = purchasePriceScaleQuantityRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格数量等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceScaleQuantityDto>> GetPurchasePriceScaleQuantityListAsync(TaktPurchasePriceScaleQuantityQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePriceScaleQuantityRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePriceScaleQuantityDto>.Create(
            data.Adapt<List<TaktPurchasePriceScaleQuantityDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleQuantityDto?> GetPurchasePriceScaleQuantityByIdAsync(long id)
    {
        var entity = await _purchasePriceScaleQuantityRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchasePriceScaleQuantityDto>();
    }

    /// <summary>
    /// 获取采购价格数量等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceScaleQuantityOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceScaleQuantityRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchasePriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PurchasePriceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建采购价格数量等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleQuantityDto> CreatePurchasePriceScaleQuantityAsync(TaktPurchasePriceScaleQuantityCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePriceScaleQuantity>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleQuantityRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.PurchasePriceCode == entity.PurchasePriceCode
                && x.PurchasePriceSeq == entity.PurchasePriceSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleQuantity == entity.ScaleQuantity);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique)
        {
            throw new TaktBusinessException("采购价格数量等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleQuantity已存在");
        }
        entity = await _purchasePriceScaleQuantityRepository.CreateAsync(entity);
        return await GetPurchasePriceScaleQuantityByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePriceScaleQuantityDto>();
    }

    /// <summary>
    /// 更新采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleQuantityDto> UpdatePurchasePriceScaleQuantityAsync(long id, TaktPurchasePriceScaleQuantityUpdateDto dto)
    {
        var entity = await _purchasePriceScaleQuantityRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格数量等级不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleQuantityRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.PurchasePriceCode == entity.PurchasePriceCode
                && x.PurchasePriceSeq == entity.PurchasePriceSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleQuantity == entity.ScaleQuantity,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique)
        {
            throw new TaktBusinessException("采购价格数量等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleQuantity已存在");
        }
        await _purchasePriceScaleQuantityRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleQuantityByIdAsync(id) ?? throw new TaktBusinessException("采购价格数量等级不存在");
    }

    /// <summary>
    /// 删除采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleQuantityByIdAsync(long id)
    {
        var entity = await _purchasePriceScaleQuantityRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格数量等级不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购价格数量等级不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购价格数量等级已作废");
        }
        entity.IsObsolete = 1;
        await _purchasePriceScaleQuantityRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购价格数量等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleQuantityBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePriceScaleQuantityByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购价格数量等级作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleQuantityDto> UpdatePurchasePriceScaleQuantityObsoleteAsync(TaktPurchasePriceScaleQuantityObsoleteDto dto)
    {
        var entity = await _purchasePriceScaleQuantityRepository.GetByIdAsync(dto.PurchasePriceScaleQuantityId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格数量等级不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购价格数量等级不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchasePriceScaleQuantityRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleQuantityByIdAsync(dto.PurchasePriceScaleQuantityId) ?? throw new TaktBusinessException("采购价格数量等级不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePriceScaleQuantityTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePriceScaleQuantityTemplateDto>(
            sheetName ?? "采购价格数量等级导入模板",
            fileName ?? "采购价格数量等级导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购价格数量等级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePriceScaleQuantityAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePriceScaleQuantityImportDto>(fileStream, sheetName ?? "采购价格数量等级导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePriceScaleQuantity>();
                var importKey = $"{entity.PurchasePriceItemId}|{entity.PurchasePriceCode}|{entity.PurchasePriceSeq}|{entity.PurchaseScaleSeq}|{entity.ScaleQuantity}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleQuantity）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceScaleQuantityRepository,
                    x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                        && x.PurchasePriceCode == entity.PurchasePriceCode
                        && x.PurchasePriceSeq == entity.PurchasePriceSeq
                        && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                        && x.ScaleQuantity == entity.ScaleQuantity);
                if (!isUnique_ix_takt_logistics_materials_purchase_price_scale_quantity_unique)
                {
                    throw new TaktBusinessException("采购价格数量等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleQuantity已存在");
                }
                await _purchasePriceScaleQuantityRepository.CreateAsync(entity);
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
    /// 导出采购价格数量等级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceScaleQuantityAsync(TaktPurchasePriceScaleQuantityQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchasePriceScaleQuantityQueryDto());
        var list = await _purchasePriceScaleQuantityRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceScaleQuantityExportDto>(),
                sheetName ?? "采购价格数量等级数据",
                fileName ?? "采购价格数量等级导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePriceScaleQuantityExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购价格数量等级数据",
            fileName ?? "采购价格数量等级导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购价格数量等级查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePriceScaleQuantity, bool>> QueryExpression(TaktPurchasePriceScaleQuantityQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePriceScaleQuantity>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PurchasePriceItemId).Contains(keywords)
                || (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || SqlFunc.ToString(x.PurchasePriceSeq).Contains(keywords)
                || SqlFunc.ToString(x.PurchaseScaleSeq).Contains(keywords)
                || SqlFunc.ToString(x.ScaleQuantity).Contains(keywords)
                || SqlFunc.ToString(x.Price).Contains(keywords)
                || SqlFunc.ToString(x.UntaxedPrice).Contains(keywords)
                || SqlFunc.ToString(x.TaxIncludedPrice).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PurchasePriceItemId.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePriceItemId == queryDto.PurchasePriceItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasePriceCode))
        {
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(queryDto.PurchasePriceCode));
        }

        if (queryDto?.PurchasePriceSeq.HasValue == true)
        {
            exp = exp.And(x => x.PurchasePriceSeq == queryDto.PurchasePriceSeq);
        }

        if (queryDto?.PurchaseScaleSeq.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseScaleSeq == queryDto.PurchaseScaleSeq);
        }

        if (queryDto?.ScaleQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ScaleQuantity == queryDto.ScaleQuantity);
        }

        if (queryDto?.Price.HasValue == true)
        {
            exp = exp.And(x => x.Price == queryDto.Price);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            exp = exp.And(x => x.UntaxedPrice == queryDto.UntaxedPrice);
        }

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            exp = exp.And(x => x.TaxIncludedPrice == queryDto.TaxIncludedPrice);
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
