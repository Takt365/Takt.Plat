// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceScaleValueService.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格价值等级应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格价值等级应用服务
/// </summary>
public class TaktSalesPriceScaleValueService : TaktServiceBase, ITaktSalesPriceScaleValueService
{
    private readonly ITaktCompanyRepository<TaktSalesPriceScaleValue> _salesPriceScaleValueRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceScaleValueRepository">销售价格价值等级仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceScaleValueService(
        ITaktCompanyRepository<TaktSalesPriceScaleValue> salesPriceScaleValueRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPriceScaleValueRepository = salesPriceScaleValueRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售价格价值等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPriceScaleValueDto>> GetSalesPriceScaleValueListAsync(TaktSalesPriceScaleValueQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPriceScaleValueRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPriceScaleValueDto>.Create(
            data.Adapt<List<TaktSalesPriceScaleValueDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售价格价值等级
    /// </summary>
    /// <param name="id">销售价格价值等级ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleValueDto?> GetSalesPriceScaleValueByIdAsync(long id)
    {
        var entity = await _salesPriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesPriceScaleValueDto>();
    }

    /// <summary>
    /// 获取销售价格价值等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceScaleValueOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPriceScaleValueRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.SalesPriceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SalesPriceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建销售价格价值等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleValueDto> CreateSalesPriceScaleValueAsync(TaktSalesPriceScaleValueCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPriceScaleValue>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_sales_price_scale_value_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceScaleValueRepository,
            x => x.SalesPriceItemId == entity.SalesPriceItemId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_price_scale_value_line_unique)
        {
            throw new TaktBusinessException("销售价格价值等级的SalesPriceItemId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesPriceScaleValueRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPriceItemId == entity.SalesPriceItemId,
                x => x.LineNumber);
            var businessCode = entity.SalesPriceItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesPriceScaleValueRepository.CreateAsync(entity);
        return await GetSalesPriceScaleValueByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPriceScaleValueDto>();
    }

    /// <summary>
    /// 更新销售价格价值等级
    /// </summary>
    /// <param name="id">销售价格价值等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleValueDto> UpdateSalesPriceScaleValueAsync(long id, TaktSalesPriceScaleValueUpdateDto dto)
    {
        var entity = await _salesPriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格价值等级不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_price_scale_value_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPriceScaleValueRepository,
            x => x.SalesPriceItemId == entity.SalesPriceItemId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_price_scale_value_line_unique)
        {
            throw new TaktBusinessException("销售价格价值等级的SalesPriceItemId、LineNumber已存在");
        }
        await _salesPriceScaleValueRepository.UpdateAsync(entity);
        return await GetSalesPriceScaleValueByIdAsync(id) ?? throw new TaktBusinessException("销售价格价值等级不存在");
    }

    /// <summary>
    /// 删除销售价格价值等级
    /// </summary>
    /// <param name="id">销售价格价值等级ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceScaleValueByIdAsync(long id)
    {
        var entity = await _salesPriceScaleValueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格价值等级不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售价格价值等级不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售价格价值等级已作废");
        }
        entity.IsObsolete = 1;
        await _salesPriceScaleValueRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售价格价值等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPriceScaleValueBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPriceScaleValueByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售价格价值等级作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPriceScaleValueDto> UpdateSalesPriceScaleValueObsoleteAsync(TaktSalesPriceScaleValueObsoleteDto dto)
    {
        var entity = await _salesPriceScaleValueRepository.GetByIdAsync(dto.SalesPriceScaleValueId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售价格价值等级不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售价格价值等级不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesPriceScaleValueRepository.UpdateAsync(entity);
        return await GetSalesPriceScaleValueByIdAsync(dto.SalesPriceScaleValueId) ?? throw new TaktBusinessException("销售价格价值等级不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPriceScaleValueTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPriceScaleValueTemplateDto>(
            sheetName ?? "销售价格价值等级导入模板",
            fileName ?? "销售价格价值等级导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售价格价值等级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPriceScaleValueAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPriceScaleValueImportDto>(fileStream, sheetName ?? "销售价格价值等级导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPriceScaleValue>();
                var importKey = $"{entity.SalesPriceItemId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesPriceItemId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_price_scale_value_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPriceScaleValueRepository,
                    x => x.SalesPriceItemId == entity.SalesPriceItemId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_price_scale_value_line_unique)
                {
                    throw new TaktBusinessException("销售价格价值等级的SalesPriceItemId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesPriceScaleValueRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesPriceItemId == entity.SalesPriceItemId,
                        x => x.LineNumber);
                    var businessCode = entity.SalesPriceItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesPriceScaleValueRepository.CreateAsync(entity);
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
    /// 导出销售价格价值等级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceScaleValueAsync(TaktSalesPriceScaleValueQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPriceScaleValueQueryDto());
        var list = await _salesPriceScaleValueRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPriceScaleValueExportDto>(),
                sheetName ?? "销售价格价值等级数据",
                fileName ?? "销售价格价值等级导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPriceScaleValueExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售价格价值等级数据",
            fileName ?? "销售价格价值等级导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售价格价值等级查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPriceScaleValue, bool>> QueryExpression(TaktSalesPriceScaleValueQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPriceScaleValue>();

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
                SqlFunc.ToString(x.SalesPriceItemId).Contains(keywords)
                || (x.SalesPriceCode != null && x.SalesPriceCode.Contains(keywords))
                || SqlFunc.ToString(x.SalesPriceSeq).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.ScaleValue).Contains(keywords)
                || SqlFunc.ToString(x.Amount).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SalesPriceItemId.HasValue == true)
        {
            exp = exp.And(x => x.SalesPriceItemId == queryDto.SalesPriceItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPriceCode))
        {
            exp = exp.And(x => x.SalesPriceCode != null && x.SalesPriceCode.Contains(queryDto.SalesPriceCode));
        }

        if (queryDto?.SalesPriceSeq.HasValue == true)
        {
            exp = exp.And(x => x.SalesPriceSeq == queryDto.SalesPriceSeq);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.ScaleValue.HasValue == true)
        {
            exp = exp.And(x => x.ScaleValue == queryDto.ScaleValue);
        }

        if (queryDto?.Amount.HasValue == true)
        {
            exp = exp.And(x => x.Amount == queryDto.Amount);
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
