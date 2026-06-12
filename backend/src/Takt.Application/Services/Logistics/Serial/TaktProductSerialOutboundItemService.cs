// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktProductSerialOutboundItemService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：产品序列号出库明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Domain.Entities.Logistics.Serial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Serial;

/// <summary>
/// 产品序列号出库明细应用服务
/// </summary>
public class TaktProductSerialOutboundItemService : TaktServiceBase, ITaktProductSerialOutboundItemService
{
    private readonly ITaktCompanyRepository<TaktProductSerialOutboundItem> _productSerialOutboundItemRepository;
    private readonly ITaktCompanyRepository<TaktProductSerialOutbound> _productSerialOutboundRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productSerialOutboundItemRepository">产品序列号出库明细仓储</param>
    /// <param name="productSerialOutboundRepository">产品序列号出库仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductSerialOutboundItemService(
        ITaktCompanyRepository<TaktProductSerialOutboundItem> productSerialOutboundItemRepository,
        ITaktCompanyRepository<TaktProductSerialOutbound> productSerialOutboundRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productSerialOutboundItemRepository = productSerialOutboundItemRepository;
        _productSerialOutboundRepository = productSerialOutboundRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取产品序列号出库明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductSerialOutboundItemDto>> GetProductSerialOutboundItemListAsync(TaktProductSerialOutboundItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productSerialOutboundItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductSerialOutboundItemDto>.Create(
            data.Adapt<List<TaktProductSerialOutboundItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialOutboundItemDto?> GetProductSerialOutboundItemByIdAsync(long id)
    {
        var entity = await _productSerialOutboundItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductSerialOutboundItemDto>();
    }

    /// <summary>
    /// 获取产品序列号出库明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductSerialOutboundItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productSerialOutboundItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.OutboundNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.OutboundNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建产品序列号出库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialOutboundItemDto> CreateProductSerialOutboundItemAsync(TaktProductSerialOutboundItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductSerialOutboundItem>();
        await StampProductSerialOutboundItemProductSerialOutboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialOutboundItemRepository,
            x => x.OutboundSerialNo == entity.OutboundSerialNo);
        if (!isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique)
        {
            throw new TaktBusinessException("产品序列号出库明细的OutboundSerialNo已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _productSerialOutboundItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OutboundId == entity.OutboundId,
                x => x.LineNumber);
            var businessCode = entity.OutboundId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _productSerialOutboundItemRepository.CreateAsync(entity);
        return await GetProductSerialOutboundItemByIdAsync(entity.Id) ?? entity.Adapt<TaktProductSerialOutboundItemDto>();
    }

    /// <summary>
    /// 更新产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialOutboundItemDto> UpdateProductSerialOutboundItemAsync(long id, TaktProductSerialOutboundItemUpdateDto dto)
    {
        var entity = await _productSerialOutboundItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("产品序列号出库明细不存在");
        }
        dto.Adapt(entity);
        await StampProductSerialOutboundItemProductSerialOutboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialOutboundItemRepository,
            x => x.OutboundSerialNo == entity.OutboundSerialNo,
            id);
        if (!isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique)
        {
            throw new TaktBusinessException("产品序列号出库明细的OutboundSerialNo已存在");
        }
        await _productSerialOutboundItemRepository.UpdateAsync(entity);
        return await GetProductSerialOutboundItemByIdAsync(id) ?? throw new TaktBusinessException("产品序列号出库明细不存在");
    }

    /// <summary>
    /// 删除产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialOutboundItemByIdAsync(long id)
    {
        var deleted = await _productSerialOutboundItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("产品序列号出库明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除产品序列号出库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialOutboundItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductSerialOutboundItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductSerialOutboundItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductSerialOutboundItemTemplateDto>(
            sheetName ?? "产品序列号出库明细导入模板",
            fileName ?? "产品序列号出库明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入产品序列号出库明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductSerialOutboundItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductSerialOutboundItemImportDto>(fileStream, sheetName ?? "产品序列号出库明细导入模板");
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
                var entity = rows[i].Adapt<TaktProductSerialOutboundItem>();
                var importDto = rows[i].Adapt<TaktProductSerialOutboundItemCreateDto>();
                await StampProductSerialOutboundItemProductSerialOutboundAsync(entity, importDto);
                var importKey = $"{entity.OutboundSerialNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（OutboundSerialNo）");
                }
                var isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _productSerialOutboundItemRepository,
                    x => x.OutboundSerialNo == entity.OutboundSerialNo);
                if (!isUnique_ix_takt_logistics_product_serial_outbound_item_outbound_serial_no_unique)
                {
                    throw new TaktBusinessException("产品序列号出库明细的OutboundSerialNo已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _productSerialOutboundItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OutboundId == entity.OutboundId,
                        x => x.LineNumber);
                    var businessCode = entity.OutboundId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _productSerialOutboundItemRepository.CreateAsync(entity);
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
    /// 导出产品序列号出库明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductSerialOutboundItemAsync(TaktProductSerialOutboundItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductSerialOutboundItemQueryDto());
        var list = await _productSerialOutboundItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductSerialOutboundItemExportDto>(),
                sheetName ?? "产品序列号出库明细数据",
                fileName ?? "产品序列号出库明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductSerialOutboundItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "产品序列号出库明细数据",
            fileName ?? "产品序列号出库明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步产品序列号出库明细主表外键（ManyToOne → 产品序列号出库）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampProductSerialOutboundItemProductSerialOutboundAsync(TaktProductSerialOutboundItem entity, TaktProductSerialOutboundItemCreateDto dto)
    {
        if (dto.OutboundId <= 0)
        {
            return;
        }
        var master = await _productSerialOutboundRepository.GetByIdAsync(dto.OutboundId);
        if (master == null)
        {
            throw new TaktBusinessException("产品序列号出库不存在");
        }
        entity.OutboundId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建产品序列号出库明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductSerialOutboundItem, bool>> QueryExpression(TaktProductSerialOutboundItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductSerialOutboundItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.OutboundId).Contains(keywords)
                || (x.OutboundNo != null && x.OutboundNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.OutboundSerialNo != null && x.OutboundSerialNo.Contains(keywords))
                || SqlFunc.ToString(x.ReferenceInboundId).Contains(keywords)
                || (x.ReferenceInboundNo != null && x.ReferenceInboundNo.Contains(keywords))
                || SqlFunc.ToString(x.ReferenceInboundLineNumber).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OutboundTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.OutboundId.HasValue == true)
        {
            exp = exp.And(x => x.OutboundId == queryDto.OutboundId);
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundNo))
        {
            exp = exp.And(x => x.OutboundNo != null && x.OutboundNo.Contains(queryDto.OutboundNo));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundSerialNo))
        {
            exp = exp.And(x => x.OutboundSerialNo != null && x.OutboundSerialNo.Contains(queryDto.OutboundSerialNo));
        }

        if (queryDto?.ReferenceInboundId.HasValue == true)
        {
            exp = exp.And(x => x.ReferenceInboundId == queryDto.ReferenceInboundId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReferenceInboundNo))
        {
            exp = exp.And(x => x.ReferenceInboundNo != null && x.ReferenceInboundNo.Contains(queryDto.ReferenceInboundNo));
        }

        if (queryDto?.ReferenceInboundLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.ReferenceInboundLineNumber == queryDto.ReferenceInboundLineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.OutboundTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.OutboundTime >= queryDto.OutboundTimeStart);
        }

        if (queryDto?.OutboundTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.OutboundTime <= queryDto.OutboundTimeEnd);
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
