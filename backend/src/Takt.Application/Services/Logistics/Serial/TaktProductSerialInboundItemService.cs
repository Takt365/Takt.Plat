// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktProductSerialInboundItemService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：产品序列号入库明细应用服务实现
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
/// 产品序列号入库明细应用服务
/// </summary>
public class TaktProductSerialInboundItemService : TaktServiceBase, ITaktProductSerialInboundItemService
{
    private readonly ITaktCompanyRepository<TaktProductSerialInboundItem> _productSerialInboundItemRepository;
    private readonly ITaktCompanyRepository<TaktProductSerialInbound> _productSerialInboundRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productSerialInboundItemRepository">产品序列号入库明细仓储</param>
    /// <param name="productSerialInboundRepository">产品序列号入库仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductSerialInboundItemService(
        ITaktCompanyRepository<TaktProductSerialInboundItem> productSerialInboundItemRepository,
        ITaktCompanyRepository<TaktProductSerialInbound> productSerialInboundRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productSerialInboundItemRepository = productSerialInboundItemRepository;
        _productSerialInboundRepository = productSerialInboundRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取产品序列号入库明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductSerialInboundItemDto>> GetProductSerialInboundItemListAsync(TaktProductSerialInboundItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productSerialInboundItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductSerialInboundItemDto>.Create(
            data.Adapt<List<TaktProductSerialInboundItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundItemDto?> GetProductSerialInboundItemByIdAsync(long id)
    {
        var entity = await _productSerialInboundItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductSerialInboundItemDto>();
    }

    /// <summary>
    /// 获取产品序列号入库明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductSerialInboundItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productSerialInboundItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.InboundNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.InboundNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建产品序列号入库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundItemDto> CreateProductSerialInboundItemAsync(TaktProductSerialInboundItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductSerialInboundItem>();
        await StampProductSerialInboundItemProductSerialInboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialInboundItemRepository,
            x => x.InboundSerialNo == entity.InboundSerialNo);
        if (!isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique)
        {
            throw new TaktBusinessException("产品序列号入库明细的InboundSerialNo已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _productSerialInboundItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InboundId == entity.InboundId,
                x => x.LineNumber);
            var businessCode = entity.InboundId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _productSerialInboundItemRepository.CreateAsync(entity);
        return await GetProductSerialInboundItemByIdAsync(entity.Id) ?? entity.Adapt<TaktProductSerialInboundItemDto>();
    }

    /// <summary>
    /// 更新产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundItemDto> UpdateProductSerialInboundItemAsync(long id, TaktProductSerialInboundItemUpdateDto dto)
    {
        var entity = await _productSerialInboundItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("产品序列号入库明细不存在");
        }
        dto.Adapt(entity);
        await StampProductSerialInboundItemProductSerialInboundAsync(entity, dto);
        var isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialInboundItemRepository,
            x => x.InboundSerialNo == entity.InboundSerialNo,
            id);
        if (!isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique)
        {
            throw new TaktBusinessException("产品序列号入库明细的InboundSerialNo已存在");
        }
        await _productSerialInboundItemRepository.UpdateAsync(entity);
        return await GetProductSerialInboundItemByIdAsync(id) ?? throw new TaktBusinessException("产品序列号入库明细不存在");
    }

    /// <summary>
    /// 删除产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialInboundItemByIdAsync(long id)
    {
        var deleted = await _productSerialInboundItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("产品序列号入库明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除产品序列号入库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialInboundItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductSerialInboundItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductSerialInboundItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductSerialInboundItemTemplateDto>(
            sheetName ?? "产品序列号入库明细导入模板",
            fileName ?? "产品序列号入库明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入产品序列号入库明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductSerialInboundItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductSerialInboundItemImportDto>(fileStream, sheetName ?? "产品序列号入库明细导入模板");
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
                var entity = rows[i].Adapt<TaktProductSerialInboundItem>();
                var importDto = rows[i].Adapt<TaktProductSerialInboundItemCreateDto>();
                await StampProductSerialInboundItemProductSerialInboundAsync(entity, importDto);
                var importKey = $"{entity.InboundSerialNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InboundSerialNo）");
                }
                var isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _productSerialInboundItemRepository,
                    x => x.InboundSerialNo == entity.InboundSerialNo);
                if (!isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique)
                {
                    throw new TaktBusinessException("产品序列号入库明细的InboundSerialNo已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _productSerialInboundItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InboundId == entity.InboundId,
                        x => x.LineNumber);
                    var businessCode = entity.InboundId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _productSerialInboundItemRepository.CreateAsync(entity);
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
    /// 导出产品序列号入库明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductSerialInboundItemAsync(TaktProductSerialInboundItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductSerialInboundItemQueryDto());
        var list = await _productSerialInboundItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductSerialInboundItemExportDto>(),
                sheetName ?? "产品序列号入库明细数据",
                fileName ?? "产品序列号入库明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductSerialInboundItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "产品序列号入库明细数据",
            fileName ?? "产品序列号入库明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步产品序列号入库明细主表外键（ManyToOne → 产品序列号入库）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampProductSerialInboundItemProductSerialInboundAsync(TaktProductSerialInboundItem entity, TaktProductSerialInboundItemCreateDto dto)
    {
        if (dto.InboundId <= 0)
        {
            return;
        }
        var master = await _productSerialInboundRepository.GetByIdAsync(dto.InboundId);
        if (master == null)
        {
            throw new TaktBusinessException("产品序列号入库不存在");
        }
        entity.InboundId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建产品序列号入库明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductSerialInboundItem, bool>> QueryExpression(TaktProductSerialInboundItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductSerialInboundItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.InboundId).Contains(keywords)
                || (x.InboundNo != null && x.InboundNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.InboundSerialNo != null && x.InboundSerialNo.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InboundTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InboundId.HasValue == true)
        {
            exp = exp.And(x => x.InboundId == queryDto.InboundId);
        }

        if (!string.IsNullOrEmpty(queryDto?.InboundNo))
        {
            exp = exp.And(x => x.InboundNo != null && x.InboundNo.Contains(queryDto.InboundNo));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.InboundSerialNo))
        {
            exp = exp.And(x => x.InboundSerialNo != null && x.InboundSerialNo.Contains(queryDto.InboundSerialNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InboundTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.InboundTime >= queryDto.InboundTimeStart);
        }

        if (queryDto?.InboundTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.InboundTime <= queryDto.InboundTimeEnd);
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
