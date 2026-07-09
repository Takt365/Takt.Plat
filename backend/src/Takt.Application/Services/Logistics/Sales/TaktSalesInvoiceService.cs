// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesInvoiceService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票应用服务实现
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
/// 销售发票应用服务
/// </summary>
public class TaktSalesInvoiceService : TaktServiceBase, ITaktSalesInvoiceService
{
    private readonly ITaktCompanyRepository<TaktSalesInvoice> _salesInvoiceRepository;
    private readonly ITaktCompanyRepository<TaktSalesInvoiceItem> _salesInvoiceItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceRepository">销售发票仓储</param>
    /// <param name="salesInvoiceItemRepository">SalesInvoiceItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesInvoiceService(
        ITaktCompanyRepository<TaktSalesInvoice> salesInvoiceRepository,
        ITaktCompanyRepository<TaktSalesInvoiceItem> salesInvoiceItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesInvoiceRepository = salesInvoiceRepository;
        _salesInvoiceItemRepository = salesInvoiceItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售发票列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesInvoiceDto>> GetSalesInvoiceListAsync(TaktSalesInvoiceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesInvoiceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesInvoiceDto>.Create(
            data.Adapt<List<TaktSalesInvoiceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto?> GetSalesInvoiceByIdAsync(long id)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesInvoiceDto>();
        await FillSalesInvoiceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售发票选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesInvoiceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesInvoiceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AccountingDocumentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.AccountingDocumentCode,
            DictLabel = string.IsNullOrWhiteSpace(e.CustomerName)
                ? e.AccountingDocumentCode
                : $"{e.AccountingDocumentCode} {e.CustomerName}",
        }).ToList();
    }

    /// <summary>
    /// 创建销售发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto> CreateSalesInvoiceAsync(TaktSalesInvoiceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesInvoice>();
        var isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.AccountingDocumentCode == entity.AccountingDocumentCode);
        if (!isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique)
        {
            throw new TaktBusinessException("销售发票的PlantCode、AccountingDocumentCode已存在");
        }
        entity = await _salesInvoiceRepository.CreateAsync(entity);
                await SaveSalesInvoiceChildrenAsync(entity, dto);
        return await GetSalesInvoiceByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesInvoiceDto>();
    }

    /// <summary>
    /// 更新销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto> UpdateSalesInvoiceAsync(long id, TaktSalesInvoiceUpdateDto dto)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceRepository,
            x => x.PlantCode == entity.PlantCode
                && x.AccountingDocumentCode == entity.AccountingDocumentCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique)
        {
            throw new TaktBusinessException("销售发票的PlantCode、AccountingDocumentCode已存在");
        }
        await _salesInvoiceRepository.UpdateAsync(entity);
                await SaveSalesInvoiceChildrenAsync(entity, dto);
        return await GetSalesInvoiceByIdAsync(id) ?? throw new TaktBusinessException("销售发票不存在");
    }

    /// <summary>
    /// 删除销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceByIdAsync(long id)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票不存在或已删除");
        }
        await _salesInvoiceItemRepository.DeleteAsync(x => x.SalesInvoiceId == entity.Id);
        var deleted = await _salesInvoiceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售发票不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesInvoiceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesInvoiceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesInvoiceTemplateDto>(
            sheetName ?? "销售发票导入模板",
            fileName ?? "销售发票导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售发票
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesInvoiceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesInvoiceImportDto>(fileStream, sheetName ?? "销售发票导入模板");
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
                var entity = rows[i].Adapt<TaktSalesInvoice>();
                var importKey = $"{entity.PlantCode}|{entity.AccountingDocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、AccountingDocumentCode）");
                }
                var isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesInvoiceRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.AccountingDocumentCode == entity.AccountingDocumentCode);
                if (!isUnique_ix_takt_logistics_sales_invoice_accounting_doc_unique)
                {
                    throw new TaktBusinessException("销售发票的PlantCode、AccountingDocumentCode已存在");
                }
                await _salesInvoiceRepository.CreateAsync(entity);
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
    /// 导出销售发票
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesInvoiceAsync(TaktSalesInvoiceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesInvoiceQueryDto());
        var list = await _salesInvoiceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceExportDto>(),
                sheetName ?? "销售发票数据",
                fileName ?? "销售发票导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesInvoiceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售发票数据",
            fileName ?? "销售发票导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售发票明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesInvoiceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesInvoiceItemsObsoleteAsync(long salesInvoiceId)
    {
        if (salesInvoiceId <= 0)
        {
            return;
        }
        var rows = await _salesInvoiceItemRepository.GetListAsync(
            x => x.SalesInvoiceId == salesInvoiceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesInvoiceItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售发票详情（加载 OneToMany 子表：销售发票明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesInvoiceDetailsAsync(TaktSalesInvoiceDto dto, TaktSalesInvoice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售发票明细 → dto.Items（含作废行）
        var items = await _salesInvoiceItemRepository.GetListAsync(x => x.SalesInvoiceId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesInvoiceItemDto>>();
    }

    /// <summary>
    /// 保存销售发票子表级联（销售发票明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesInvoiceChildrenAsync(TaktSalesInvoice entity, TaktSalesInvoiceCreateDto dto)
    {
        // 销售发票明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await MarkSalesInvoiceItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesInvoiceItemRepository.GetListAsync(x => x.SalesInvoiceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesInvoiceItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.Items.Count; i++)
            {
                var childDto = dto.Items[i];
                childDto.SalesInvoiceId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售发票明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesInvoiceId、LineNumber）");
                }
                if (childDto.SalesInvoiceItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesInvoiceItemId, out var target))
                    {
                        throw new TaktBusinessException("销售发票明细不存在（SalesInvoiceItemId={childDto.SalesInvoiceItemId}）");
                    }
                    if (target.SalesInvoiceId != entity.Id)
                    {
                        throw new TaktBusinessException("销售发票明细不属于当前主表（SalesInvoiceItemId={childDto.SalesInvoiceItemId}）");
                    }
                    submittedIds.Add(childDto.SalesInvoiceItemId);
                    var isUniqueUpdate_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesInvoiceItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesInvoiceId == x.SalesInvoiceId
                && x.LineNumber == x.LineNumber,
                        childDto.SalesInvoiceItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("销售发票明细的CompanyCode、SalesInvoiceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesInvoiceItemId;
                    target.SalesInvoiceId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesInvoiceItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesInvoiceItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesInvoiceId == x.SalesInvoiceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("销售发票明细的CompanyCode、SalesInvoiceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSalesInvoiceItem>();
                    child.Id = 0;
                    child.SalesInvoiceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesInvoiceItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CustomerCode) ? entity.CustomerCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _salesInvoiceItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售发票查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesInvoice, bool>> QueryExpression(TaktSalesInvoiceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesInvoice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.YearMonth != null && x.YearMonth.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.AccountingDocumentCode != null && x.AccountingDocumentCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.YearMonth))
        {
            exp = exp.And(x => x.YearMonth != null && x.YearMonth.Contains(queryDto.YearMonth));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountingDocumentCode))
        {
            exp = exp.And(x => x.AccountingDocumentCode != null && x.AccountingDocumentCode.Contains(queryDto.AccountingDocumentCode));
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
