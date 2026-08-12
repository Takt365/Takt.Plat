// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleValueService.cs
// 创建时间：2026-08-10
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格价值等级应用服务实现
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
/// 采购价格价值等级应用服务
/// </summary>
public class TaktPurchasePriceScaleValueService : TaktServiceBase, ITaktPurchasePriceScaleValueService
{
    private readonly ITaktCompanyRepository<TaktPurchasePriceScaleValue> _purchasePriceScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceScaleValueRepository">采购价格价值等级仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceScaleValueService(
        ITaktCompanyRepository<TaktPurchasePriceScaleValue> purchasePriceScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePriceScaleValueRepository = purchasePriceScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购价格价值等级列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePriceScaleValueDto>> GetPurchasePriceScaleValueListAsync(TaktPurchasePriceScaleValueQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchasePriceScaleValueDto>.Create(
                new List<TaktPurchasePriceScaleValueDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePriceScaleValueRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePriceScaleValueDto>.Create(
            data.Adapt<List<TaktPurchasePriceScaleValueDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购价格价值等级
    /// </summary>
    /// <param name="id">采购价格价值等级ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleValueDto?> GetPurchasePriceScaleValueByIdAsync(long id)
    {
        var entity = await _purchasePriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchasePriceScaleValueDto>();
    }

    /// <summary>
    /// 获取采购价格价值等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceScaleValueOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchasePriceScaleValueRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchasePriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchasePriceCode,
            DictLabel = e.PurchasePriceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购价格价值等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleValueDto> CreatePurchasePriceScaleValueAsync(TaktPurchasePriceScaleValueCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePriceScaleValue>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleValueRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.PurchasePriceCode == entity.PurchasePriceCode
                && x.PurchasePriceSeq == entity.PurchasePriceSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleValue == entity.ScaleValue);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique)
        {
            throw new TaktBusinessException("采购价格价值等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleValue已存在");
        }
        entity = await _purchasePriceScaleValueRepository.CreateAsync(entity);
        return await GetPurchasePriceScaleValueByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePriceScaleValueDto>();
    }

    /// <summary>
    /// 更新采购价格价值等级
    /// </summary>
    /// <param name="id">采购价格价值等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleValueDto> UpdatePurchasePriceScaleValueAsync(long id, TaktPurchasePriceScaleValueUpdateDto dto)
    {
        var entity = await _purchasePriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格价值等级不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePriceScaleValueRepository,
            x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                && x.PurchasePriceCode == entity.PurchasePriceCode
                && x.PurchasePriceSeq == entity.PurchasePriceSeq
                && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                && x.ScaleValue == entity.ScaleValue,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique)
        {
            throw new TaktBusinessException("采购价格价值等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleValue已存在");
        }
        await _purchasePriceScaleValueRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleValueByIdAsync(id) ?? throw new TaktBusinessException("采购价格价值等级不存在");
    }

    /// <summary>
    /// 删除采购价格价值等级
    /// </summary>
    /// <param name="id">采购价格价值等级ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleValueByIdAsync(long id)
    {
        var entity = await _purchasePriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格价值等级不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购价格价值等级不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购价格价值等级已作废");
        }
        entity.IsObsolete = 1;
        await _purchasePriceScaleValueRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购价格价值等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePriceScaleValueBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePriceScaleValueByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购价格价值等级作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePriceScaleValueDto> UpdatePurchasePriceScaleValueObsoleteAsync(TaktPurchasePriceScaleValueObsoleteDto dto)
    {
        var entity = await _purchasePriceScaleValueRepository.GetByIdAsync(dto.PurchasePriceScaleValueId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购价格价值等级不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购价格价值等级不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchasePriceScaleValueRepository.UpdateAsync(entity);
        return await GetPurchasePriceScaleValueByIdAsync(dto.PurchasePriceScaleValueId) ?? throw new TaktBusinessException("采购价格价值等级不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePriceScaleValueTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePriceScaleValueTemplateDto>(
            sheetName ?? "采购价格价值等级导入模板",
            fileName ?? "采购价格价值等级导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购价格价值等级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePriceScaleValueAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePriceScaleValueImportDto>(fileStream, sheetName ?? "采购价格价值等级导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePriceScaleValue>();
                var importKey = $"{entity.PurchasePriceItemId}|{entity.PurchasePriceCode}|{entity.PurchasePriceSeq}|{entity.PurchaseScaleSeq}|{entity.ScaleValue}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleValue）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePriceScaleValueRepository,
                    x => x.PurchasePriceItemId == entity.PurchasePriceItemId
                        && x.PurchasePriceCode == entity.PurchasePriceCode
                        && x.PurchasePriceSeq == entity.PurchasePriceSeq
                        && x.PurchaseScaleSeq == entity.PurchaseScaleSeq
                        && x.ScaleValue == entity.ScaleValue);
                if (!isUnique_ix_takt_logistics_procurement_purchase_price_scale_value_unique)
                {
                    throw new TaktBusinessException("采购价格价值等级的PurchasePriceItemId、PurchasePriceCode、PurchasePriceSeq、PurchaseScaleSeq、ScaleValue已存在");
                }
                await _purchasePriceScaleValueRepository.CreateAsync(entity);
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
    /// 导出采购价格价值等级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceScaleValueAsync(TaktPurchasePriceScaleValueQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchasePriceScaleValueQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceScaleValueExportDto>(),
                sheetName ?? "采购价格价值等级数据",
                fileName ?? "采购价格价值等级导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchasePriceScaleValueRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePriceScaleValueExportDto>(),
                sheetName ?? "采购价格价值等级数据",
                fileName ?? "采购价格价值等级导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePriceScaleValueExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购价格价值等级数据",
            fileName ?? "采购价格价值等级导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购价格价值等级查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePriceScaleValue, bool>> QueryExpression(TaktPurchasePriceScaleValueQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePriceScaleValue>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (queryDto?.PurchasePriceItemId.HasValue == true)
        {
            var purchasePriceItemId = queryDto.PurchasePriceItemId;
            exp = exp.And(x => x.PurchasePriceItemId == purchasePriceItemId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasePriceCode))
        {
            var purchasePriceCode = queryDto.PurchasePriceCode;
            exp = exp.And(x => x.PurchasePriceCode != null && x.PurchasePriceCode.Contains(purchasePriceCode));
        }

        if (queryDto?.PurchasePriceSeq.HasValue == true)
        {
            var purchasePriceSeq = queryDto.PurchasePriceSeq;
            exp = exp.And(x => x.PurchasePriceSeq == purchasePriceSeq);
        }

        if (queryDto?.PurchaseScaleSeq.HasValue == true)
        {
            var purchaseScaleSeq = queryDto.PurchaseScaleSeq;
            exp = exp.And(x => x.PurchaseScaleSeq == purchaseScaleSeq);
        }

        if (queryDto?.ScaleValue.HasValue == true)
        {
            var scaleValue = queryDto.ScaleValue;
            exp = exp.And(x => x.ScaleValue == scaleValue);
        }

        if (queryDto?.Price.HasValue == true)
        {
            var price = queryDto.Price;
            exp = exp.And(x => x.Price == price);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            var untaxedPrice = queryDto.UntaxedPrice;
            exp = exp.And(x => x.UntaxedPrice == untaxedPrice);
        }

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            var taxIncludedPrice = queryDto.TaxIncludedPrice;
            exp = exp.And(x => x.TaxIncludedPrice == taxIncludedPrice);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktPurchasePriceScaleValueQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (queryDto.PurchasePriceItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasePriceCode))
        {
            return true;
        }
        if (queryDto.PurchasePriceSeq.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseScaleSeq.HasValue)
        {
            return true;
        }
        if (queryDto.ScaleValue.HasValue)
        {
            return true;
        }
        if (queryDto.Price.HasValue)
        {
            return true;
        }
        if (queryDto.UntaxedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxIncludedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
