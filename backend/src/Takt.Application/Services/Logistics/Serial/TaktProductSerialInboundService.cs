// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：TaktProductSerialInboundService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：产品序列号入库应用服务实现
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
/// 产品序列号入库应用服务
/// </summary>
public class TaktProductSerialInboundService : TaktServiceBase, ITaktProductSerialInboundService
{
    private readonly ITaktCompanyRepository<TaktProductSerialInbound> _productSerialInboundRepository;
    private readonly ITaktCompanyRepository<TaktProductSerialInboundItem> _productSerialInboundItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productSerialInboundRepository">产品序列号入库仓储</param>
    /// <param name="productSerialInboundItemRepository">ProductSerialInboundItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductSerialInboundService(
        ITaktCompanyRepository<TaktProductSerialInbound> productSerialInboundRepository,
        ITaktCompanyRepository<TaktProductSerialInboundItem> productSerialInboundItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productSerialInboundRepository = productSerialInboundRepository;
        _productSerialInboundItemRepository = productSerialInboundItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取产品序列号入库列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductSerialInboundDto>> GetProductSerialInboundListAsync(TaktProductSerialInboundQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productSerialInboundRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductSerialInboundDto>.Create(
            data.Adapt<List<TaktProductSerialInboundDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取产品序列号入库
    /// </summary>
    /// <param name="id">产品序列号入库ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundDto?> GetProductSerialInboundByIdAsync(long id)
    {
        var entity = await _productSerialInboundRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktProductSerialInboundDto>();
        await FillProductSerialInboundDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取产品序列号入库选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductSerialInboundOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productSerialInboundRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建产品序列号入库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundDto> CreateProductSerialInboundAsync(TaktProductSerialInboundCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductSerialInbound>();
        var isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialInboundRepository,
            x => x.PlantCode == entity.PlantCode
                && x.InboundNo == entity.InboundNo);
        if (!isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique)
        {
            throw new TaktBusinessException("产品序列号入库的PlantCode、InboundNo已存在");
        }
        entity = await _productSerialInboundRepository.CreateAsync(entity);
                await SaveProductSerialInboundChildrenAsync(entity, dto);
        return await GetProductSerialInboundByIdAsync(entity.Id) ?? entity.Adapt<TaktProductSerialInboundDto>();
    }

    /// <summary>
    /// 更新产品序列号入库
    /// </summary>
    /// <param name="id">产品序列号入库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductSerialInboundDto> UpdateProductSerialInboundAsync(long id, TaktProductSerialInboundUpdateDto dto)
    {
        var entity = await _productSerialInboundRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("产品序列号入库不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique = await _uniqueValidator.IsUniqueAsync(
            _productSerialInboundRepository,
            x => x.PlantCode == entity.PlantCode
                && x.InboundNo == entity.InboundNo,
            id);
        if (!isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique)
        {
            throw new TaktBusinessException("产品序列号入库的PlantCode、InboundNo已存在");
        }
        await _productSerialInboundRepository.UpdateAsync(entity);
                await SaveProductSerialInboundChildrenAsync(entity, dto);
        return await GetProductSerialInboundByIdAsync(id) ?? throw new TaktBusinessException("产品序列号入库不存在");
    }

    /// <summary>
    /// 删除产品序列号入库
    /// </summary>
    /// <param name="id">产品序列号入库ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialInboundByIdAsync(long id)
    {
        var entity = await _productSerialInboundRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("产品序列号入库不存在或已删除");
        }
        await _productSerialInboundItemRepository.DeleteAsync(x => x.InboundId == entity.Id);
        var deleted = await _productSerialInboundRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("产品序列号入库不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除产品序列号入库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductSerialInboundBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductSerialInboundByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductSerialInboundTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductSerialInboundTemplateDto>(
            sheetName ?? "产品序列号入库导入模板",
            fileName ?? "产品序列号入库导入模板.xlsx");
    }

    /// <summary>
    /// 导入产品序列号入库
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductSerialInboundAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductSerialInboundImportDto>(fileStream, sheetName ?? "产品序列号入库导入模板");
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
                var entity = rows[i].Adapt<TaktProductSerialInbound>();
                var importKey = $"{entity.PlantCode}|{entity.InboundNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、InboundNo）");
                }
                var isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique = await _uniqueValidator.IsUniqueAsync(
                    _productSerialInboundRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.InboundNo == entity.InboundNo);
                if (!isUnique_ix_takt_logistics_product_serial_inbound_inbound_unique)
                {
                    throw new TaktBusinessException("产品序列号入库的PlantCode、InboundNo已存在");
                }
                await _productSerialInboundRepository.CreateAsync(entity);
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
    /// 导出产品序列号入库
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductSerialInboundAsync(TaktProductSerialInboundQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductSerialInboundQueryDto());
        var list = await _productSerialInboundRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductSerialInboundExportDto>(),
                sheetName ?? "产品序列号入库数据",
                fileName ?? "产品序列号入库导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductSerialInboundExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "产品序列号入库数据",
            fileName ?? "产品序列号入库导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充产品序列号入库详情（加载 OneToMany 子表：产品序列号入库明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillProductSerialInboundDetailsAsync(TaktProductSerialInboundDto dto, TaktProductSerialInbound entity)
    {
        if (dto == null)
        {
            return;
        }
        // 产品序列号入库明细 → dto.Items
        var items = await _productSerialInboundItemRepository.GetListAsync(x => x.InboundId == entity.Id);
        dto.Items = items.Adapt<List<TaktProductSerialInboundItemDto>>();
    }

    /// <summary>
    /// 保存产品序列号入库子表级联（产品序列号入库明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveProductSerialInboundChildrenAsync(TaktProductSerialInbound entity, TaktProductSerialInboundCreateDto dto)
    {
        // 产品序列号入库明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _productSerialInboundItemRepository.DeleteAsync(x => x.InboundId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktProductSerialInboundItem>>();
            foreach (var child in items)
            {
                child.InboundId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _productSerialInboundItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InboundId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].InboundSerialNo}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"产品序列号入库明细第{i + 1}项与本次提交的其他项重复（CompanyCode、InboundSerialNo）");
                            }
                        }
            await _productSerialInboundItemRepository.DeleteAsync(x => x.InboundId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique = await _uniqueValidator.IsUniqueAsync(
                _productSerialInboundItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.InboundSerialNo == child.InboundSerialNo);
            if (!isUnique_ix_takt_logistics_product_serial_inbound_item_inbound_serial_no_unique)
            {
                throw new TaktBusinessException("产品序列号入库明细的CompanyCode、InboundSerialNo已存在");
            }
            }
            await _productSerialInboundItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建产品序列号入库查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductSerialInbound, bool>> QueryExpression(TaktProductSerialInboundQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductSerialInbound>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.InboundNo != null && x.InboundNo.Contains(keywords))
                || SqlFunc.ToString(x.InboundType).Contains(keywords)
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.LocationCode != null && x.LocationCode.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || (x.RelatedCompany != null && x.RelatedCompany.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InboundDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.InboundNo))
        {
            exp = exp.And(x => x.InboundNo != null && x.InboundNo.Contains(queryDto.InboundNo));
        }

        if (queryDto?.InboundType.HasValue == true)
        {
            exp = exp.And(x => x.InboundType == queryDto.InboundType);
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseCode))
        {
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(queryDto.WarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.LocationCode))
        {
            exp = exp.And(x => x.LocationCode != null && x.LocationCode.Contains(queryDto.LocationCode));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedCompany))
        {
            exp = exp.And(x => x.RelatedCompany != null && x.RelatedCompany.Contains(queryDto.RelatedCompany));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InboundDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InboundDate >= queryDto.InboundDateStart);
        }

        if (queryDto?.InboundDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InboundDate <= queryDto.InboundDateEnd);
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
