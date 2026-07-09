// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 进货检验单应用服务
/// </summary>
public class TaktIqcOrderService : TaktServiceBase, ITaktIqcOrderService
{
    private readonly ITaktCompanyRepository<TaktIqcOrder> _iqcOrderRepository;
    private readonly ITaktCompanyRepository<TaktIqcOrderItem> _iqcOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderRepository">进货检验单仓储</param>
    /// <param name="iqcOrderItemRepository">IqcOrderItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIqcOrderService(
        ITaktCompanyRepository<TaktIqcOrder> iqcOrderRepository,
        ITaktCompanyRepository<TaktIqcOrderItem> iqcOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _iqcOrderRepository = iqcOrderRepository;
        _iqcOrderItemRepository = iqcOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取进货检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIqcOrderDto>> GetIqcOrderListAsync(TaktIqcOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _iqcOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIqcOrderDto>.Create(
            data.Adapt<List<TaktIqcOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderDto?> GetIqcOrderByIdAsync(long id)
    {
        var entity = await _iqcOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktIqcOrderDto>();
        await FillIqcOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取进货检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIqcOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _iqcOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.JudgeStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建进货检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderDto> CreateIqcOrderAsync(TaktIqcOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktIqcOrder>();
        var isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _iqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.IqcOrderCode == entity.IqcOrderCode);
        if (!isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique)
        {
            throw new TaktBusinessException("进货检验单的PlantCode、IqcOrderCode已存在");
        }
        entity = await _iqcOrderRepository.CreateAsync(entity);
                await SaveIqcOrderChildrenAsync(entity, dto);
        return await GetIqcOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktIqcOrderDto>();
    }

    /// <summary>
    /// 更新进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderDto> UpdateIqcOrderAsync(long id, TaktIqcOrderUpdateDto dto)
    {
        var entity = await _iqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _iqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.IqcOrderCode == entity.IqcOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique)
        {
            throw new TaktBusinessException("进货检验单的PlantCode、IqcOrderCode已存在");
        }
        await _iqcOrderRepository.UpdateAsync(entity);
                await SaveIqcOrderChildrenAsync(entity, dto);
        return await GetIqcOrderByIdAsync(id) ?? throw new TaktBusinessException("进货检验单不存在");
    }

    /// <summary>
    /// 删除进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderByIdAsync(long id)
    {
        var entity = await _iqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单不存在或已删除");
        }
        await _iqcOrderItemRepository.DeleteAsync(x => x.IqcOrderId == entity.Id);
        var deleted = await _iqcOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("进货检验单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除进货检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIqcOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteIqcOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新进货检验单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIqcOrderDto> UpdateIqcOrderStatusAsync(TaktIqcOrderStatusDto dto)
    {
        var entity = await _iqcOrderRepository.GetByIdAsync(dto.IqcOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("进货检验单不存在");
        }
        entity.JudgeStatus = dto.JudgeStatus;
        await _iqcOrderRepository.UpdateAsync(entity);
        return await GetIqcOrderByIdAsync(dto.IqcOrderId) ?? throw new TaktBusinessException("进货检验单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetIqcOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktIqcOrderTemplateDto>(
            sheetName ?? "进货检验单导入模板",
            fileName ?? "进货检验单导入模板.xlsx");
    }

    /// <summary>
    /// 导入进货检验单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportIqcOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktIqcOrderImportDto>(fileStream, sheetName ?? "进货检验单导入模板");
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
                var entity = rows[i].Adapt<TaktIqcOrder>();
                var importKey = $"{entity.PlantCode}|{entity.IqcOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、IqcOrderCode）");
                }
                var isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _iqcOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.IqcOrderCode == entity.IqcOrderCode);
                if (!isUnique_ix_takt_logistics_quality_iqc_order_iqc_order_unique)
                {
                    throw new TaktBusinessException("进货检验单的PlantCode、IqcOrderCode已存在");
                }
                await _iqcOrderRepository.CreateAsync(entity);
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
    /// 导出进货检验单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIqcOrderAsync(TaktIqcOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktIqcOrderQueryDto());
        var list = await _iqcOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIqcOrderExportDto>(),
                sheetName ?? "进货检验单数据",
                fileName ?? "进货检验单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIqcOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "进货检验单数据",
            fileName ?? "进货检验单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废进货检验单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="iqcOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkIqcOrderItemsObsoleteAsync(long iqcOrderId)
    {
        if (iqcOrderId <= 0)
        {
            return;
        }
        var rows = await _iqcOrderItemRepository.GetListAsync(
            x => x.IqcOrderId == iqcOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _iqcOrderItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充进货检验单详情（加载 OneToMany 子表：进货检验单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillIqcOrderDetailsAsync(TaktIqcOrderDto dto, TaktIqcOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 进货检验单明细 → dto.Items（含作废行）
        var items = await _iqcOrderItemRepository.GetListAsync(x => x.IqcOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktIqcOrderItemDto>>();
    }

    /// <summary>
    /// 保存进货检验单子表级联（进货检验单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveIqcOrderChildrenAsync(TaktIqcOrder entity, TaktIqcOrderCreateDto dto)
    {
        // 进货检验单明细（Items）
        List<TaktIqcOrderItemUpdateDto>? itemsForSave;
        if (dto is TaktIqcOrderUpdateDto updateDto && updateDto.Items != null)
        {
            itemsForSave = updateDto.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktIqcOrderItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkIqcOrderItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _iqcOrderItemRepository.GetListAsync(x => x.IqcOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktIqcOrderItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.IqcOrderId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("进货检验单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、IqcOrderId、LineNumber）");
                }
                if (childDto.IqcOrderItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.IqcOrderItemId, out var target))
                    {
                        throw new TaktBusinessException("进货检验单明细不存在（IqcOrderItemId={childDto.IqcOrderItemId}）");
                    }
                    if (target.IqcOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("进货检验单明细不属于当前主表（IqcOrderItemId={childDto.IqcOrderItemId}）");
                    }
                    submittedIds.Add(childDto.IqcOrderItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_iqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _iqcOrderItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.IqcOrderId == x.IqcOrderId
                && x.LineNumber == x.LineNumber,
                        childDto.IqcOrderItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_iqc_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("进货检验单明细的CompanyCode、IqcOrderId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.IqcOrderItemId;
                    target.IqcOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _iqcOrderItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_iqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _iqcOrderItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.IqcOrderId == x.IqcOrderId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_iqc_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("进货检验单明细的CompanyCode、IqcOrderId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktIqcOrderItem>();
                    child.Id = 0;
                    child.IqcOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _iqcOrderItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.IqcOrderCode) ? entity.IqcOrderCode : entity.Id.ToString();
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
                await _iqcOrderItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }

    /// <summary>
    /// 获取 IQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>IQC 检验统计</returns>
    public async Task<TaktIqcOrderStatDto> GetIqcOrderStatAsync(TaktQualityStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.InspectionDateStart,
            queryDto.InspectionDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktIqcOrder, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InspectionDate != null
            && x.InspectionDate >= start
            && x.InspectionDate <= end;
        var monthOrderCount = await _iqcOrderRepository.CountAsync(predicate);
        var monthSampleQuantity = await _iqcOrderRepository.SumAsync(x => x.TotalSampleQuantity, predicate);
        var monthQualifiedQuantity = await _iqcOrderRepository.SumAsync(x => x.TotalQualifiedQuantity, predicate);
        var monthUnqualifiedQuantity = await _iqcOrderRepository.SumAsync(x => x.TotalUnqualifiedQuantity, predicate);
        return new TaktIqcOrderStatDto
        {
            StatMonth = statMonth,
            MonthOrderCount = monthOrderCount,
            MonthSampleQuantity = monthSampleQuantity,
            MonthQualifiedQuantity = monthQualifiedQuantity,
            MonthUnqualifiedQuantity = monthUnqualifiedQuantity,
            MonthPassRatePercent = TaktQualityStatHelper.CalculatePassRatePercent(monthQualifiedQuantity, monthSampleQuantity),
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建进货检验单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIqcOrder, bool>> QueryExpression(TaktIqcOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIqcOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || (x.IqcOrderCode != null && x.IqcOrderCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || SqlFunc.ToString(x.TotalPurchaseQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalSampleQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalQualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalUnqualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalInspectionReturnQuantity).Contains(keywords)
                || (x.JudgeBy != null && x.JudgeBy.Contains(keywords))
                || (x.JudgeDescription != null && x.JudgeDescription.Contains(keywords))
                || SqlFunc.ToString(x.JudgeStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InspectionDate).Contains(keywords)
                || SqlFunc.ToString(x.JudgeDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCode))
        {
            exp = exp.And(x => x.SourceCode != null && x.SourceCode.Contains(queryDto.SourceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.IqcOrderCode))
        {
            exp = exp.And(x => x.IqcOrderCode != null && x.IqcOrderCode.Contains(queryDto.IqcOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (queryDto?.TotalPurchaseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalPurchaseQuantity == queryDto.TotalPurchaseQuantity);
        }

        if (queryDto?.TotalSampleQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalSampleQuantity == queryDto.TotalSampleQuantity);
        }

        if (queryDto?.TotalQualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQualifiedQuantity == queryDto.TotalQualifiedQuantity);
        }

        if (queryDto?.TotalUnqualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalUnqualifiedQuantity == queryDto.TotalUnqualifiedQuantity);
        }

        if (queryDto?.TotalInspectionReturnQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalInspectionReturnQuantity == queryDto.TotalInspectionReturnQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.JudgeBy))
        {
            exp = exp.And(x => x.JudgeBy != null && x.JudgeBy.Contains(queryDto.JudgeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.JudgeDescription))
        {
            exp = exp.And(x => x.JudgeDescription != null && x.JudgeDescription.Contains(queryDto.JudgeDescription));
        }

        if (queryDto?.JudgeStatus.HasValue == true)
        {
            exp = exp.And(x => x.JudgeStatus == queryDto.JudgeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InspectionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate >= queryDto.InspectionDateStart);
        }

        if (queryDto?.InspectionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate <= queryDto.InspectionDateEnd);
        }

        if (queryDto?.JudgeDateStart.HasValue == true)
        {
            exp = exp.And(x => x.JudgeDate >= queryDto.JudgeDateStart);
        }

        if (queryDto?.JudgeDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.JudgeDate <= queryDto.JudgeDateEnd);
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
