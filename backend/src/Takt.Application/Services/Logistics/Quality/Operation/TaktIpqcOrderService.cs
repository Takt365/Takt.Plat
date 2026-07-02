// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单应用服务实现
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
/// 制程检验单应用服务
/// </summary>
public class TaktIpqcOrderService : TaktServiceBase, ITaktIpqcOrderService
{
    private readonly ITaktCompanyRepository<TaktIpqcOrder> _ipqcOrderRepository;
    private readonly ITaktCompanyRepository<TaktIpqcOrderItem> _ipqcOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktIpqcOrderChangeLog> _ipqcOrderChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcOrderRepository">制程检验单仓储</param>
    /// <param name="ipqcOrderItemRepository">IpqcOrderItem仓储</param>
    /// <param name="ipqcOrderChangeLogRepository">IpqcOrderChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIpqcOrderService(
        ITaktCompanyRepository<TaktIpqcOrder> ipqcOrderRepository,
        ITaktCompanyRepository<TaktIpqcOrderItem> ipqcOrderItemRepository,
        ITaktCompanyRepository<TaktIpqcOrderChangeLog> ipqcOrderChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ipqcOrderRepository = ipqcOrderRepository;
        _ipqcOrderItemRepository = ipqcOrderItemRepository;
        _ipqcOrderChangeLogRepository = ipqcOrderChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取制程检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIpqcOrderDto>> GetIpqcOrderListAsync(TaktIpqcOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ipqcOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIpqcOrderDto>.Create(
            data.Adapt<List<TaktIpqcOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderDto?> GetIpqcOrderByIdAsync(long id)
    {
        var entity = await _ipqcOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktIpqcOrderDto>();
        await FillIpqcOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取制程检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIpqcOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ipqcOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.JudgeStatus == 1,
            x => x.ProcessName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProcessName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建制程检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderDto> CreateIpqcOrderAsync(TaktIpqcOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktIpqcOrder>();
        var isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _ipqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.IpqcOrderCode == entity.IpqcOrderCode);
        if (!isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique)
        {
            throw new TaktBusinessException("制程检验单的PlantCode、IpqcOrderCode已存在");
        }
        entity = await _ipqcOrderRepository.CreateAsync(entity);
                await SaveIpqcOrderChildrenAsync(entity, dto);
        return await GetIpqcOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktIpqcOrderDto>();
    }

    /// <summary>
    /// 更新制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderDto> UpdateIpqcOrderAsync(long id, TaktIpqcOrderUpdateDto dto)
    {
        var entity = await _ipqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("制程检验单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _ipqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.IpqcOrderCode == entity.IpqcOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique)
        {
            throw new TaktBusinessException("制程检验单的PlantCode、IpqcOrderCode已存在");
        }
        await _ipqcOrderRepository.UpdateAsync(entity);
                await SaveIpqcOrderChildrenAsync(entity, dto);
        return await GetIpqcOrderByIdAsync(id) ?? throw new TaktBusinessException("制程检验单不存在");
    }

    /// <summary>
    /// 删除制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIpqcOrderByIdAsync(long id)
    {
        var entity = await _ipqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("制程检验单不存在或已删除");
        }
        await _ipqcOrderItemRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
        await _ipqcOrderChangeLogRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
        var deleted = await _ipqcOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("制程检验单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除制程检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIpqcOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteIpqcOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新制程检验单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIpqcOrderDto> UpdateIpqcOrderStatusAsync(TaktIpqcOrderStatusDto dto)
    {
        var entity = await _ipqcOrderRepository.GetByIdAsync(dto.IpqcOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("制程检验单不存在");
        }
        entity.JudgeStatus = dto.JudgeStatus;
        await _ipqcOrderRepository.UpdateAsync(entity);
        return await GetIpqcOrderByIdAsync(dto.IpqcOrderId) ?? throw new TaktBusinessException("制程检验单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetIpqcOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktIpqcOrderTemplateDto>(
            sheetName ?? "制程检验单导入模板",
            fileName ?? "制程检验单导入模板.xlsx");
    }

    /// <summary>
    /// 导入制程检验单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportIpqcOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktIpqcOrderImportDto>(fileStream, sheetName ?? "制程检验单导入模板");
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
                var entity = rows[i].Adapt<TaktIpqcOrder>();
                var importKey = $"{entity.PlantCode}|{entity.IpqcOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、IpqcOrderCode）");
                }
                var isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _ipqcOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.IpqcOrderCode == entity.IpqcOrderCode);
                if (!isUnique_ix_takt_logistics_quality_ipqc_order_ipqc_order_unique)
                {
                    throw new TaktBusinessException("制程检验单的PlantCode、IpqcOrderCode已存在");
                }
                await _ipqcOrderRepository.CreateAsync(entity);
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
    /// 导出制程检验单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIpqcOrderAsync(TaktIpqcOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktIpqcOrderQueryDto());
        var list = await _ipqcOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIpqcOrderExportDto>(),
                sheetName ?? "制程检验单数据",
                fileName ?? "制程检验单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIpqcOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "制程检验单数据",
            fileName ?? "制程检验单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充制程检验单详情（加载 OneToMany 子表：制程检验单明细、制程检验单变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillIpqcOrderDetailsAsync(TaktIpqcOrderDto dto, TaktIpqcOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 制程检验单明细 → dto.Items
        var items = await _ipqcOrderItemRepository.GetListAsync(x => x.IpqcOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktIpqcOrderItemDto>>();
        // 制程检验单变更日志 → dto.ChangeLogs
        var changelogs = await _ipqcOrderChangeLogRepository.GetListAsync(x => x.IpqcOrderId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktIpqcOrderChangeLogDto>>();
    }

    /// <summary>
    /// 保存制程检验单子表级联（制程检验单明细、制程检验单变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveIpqcOrderChildrenAsync(TaktIpqcOrder entity, TaktIpqcOrderCreateDto dto)
    {
        // 制程检验单明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _ipqcOrderItemRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktIpqcOrderItem>>();
            foreach (var child in items)
            {
                child.IpqcOrderId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.IpqcOrderCode) ? entity.IpqcOrderCode : entity.Id.ToString();
                var maxLine = await _ipqcOrderItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IpqcOrderId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].IpqcOrderId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"制程检验单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、IpqcOrderId、LineNumber）");
                            }
                        }
            await _ipqcOrderItemRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_quality_ipqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                _ipqcOrderItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.IpqcOrderId == child.IpqcOrderId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_ipqc_order_item_order_line_unique)
            {
                throw new TaktBusinessException("制程检验单明细的CompanyCode、IpqcOrderId、LineNumber已存在");
            }
            }
            await _ipqcOrderItemRepository.CreateRangeAsync(items);
        }
        // 制程检验单变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _ipqcOrderChangeLogRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktIpqcOrderChangeLog>>();
            foreach (var child in changelogs)
            {
                child.IpqcOrderId = entity.Id;
            }
            await _ipqcOrderChangeLogRepository.DeleteAsync(x => x.IpqcOrderId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _ipqcOrderChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }

    /// <summary>
    /// 获取 IPQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>IPQC 检验统计</returns>
    public async Task<TaktIpqcOrderStatDto> GetIpqcOrderStatAsync(TaktQualityStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.InspectionDateStart,
            queryDto.InspectionDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktIpqcOrder, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InspectionDate != null
            && x.InspectionDate >= start
            && x.InspectionDate <= end;
        var monthOrderCount = await _ipqcOrderRepository.CountAsync(predicate);
        var monthSampleQuantity = await _ipqcOrderRepository.SumAsync(x => x.TotalSampleQuantity, predicate);
        var monthQualifiedQuantity = await _ipqcOrderRepository.SumAsync(x => x.TotalQualifiedQuantity, predicate);
        var monthUnqualifiedQuantity = await _ipqcOrderRepository.SumAsync(x => x.TotalUnqualifiedQuantity, predicate);
        return new TaktIpqcOrderStatDto
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
    /// 构建制程检验单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIpqcOrder, bool>> QueryExpression(TaktIpqcOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIpqcOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || (x.IpqcOrderCode != null && x.IpqcOrderCode.Contains(keywords))
                || (x.ProcessCode != null && x.ProcessCode.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
                || SqlFunc.ToString(x.TotalProductionQuantity).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.IpqcOrderCode))
        {
            exp = exp.And(x => x.IpqcOrderCode != null && x.IpqcOrderCode.Contains(queryDto.IpqcOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessCode))
        {
            exp = exp.And(x => x.ProcessCode != null && x.ProcessCode.Contains(queryDto.ProcessCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessName))
        {
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(queryDto.ProcessName));
        }

        if (queryDto?.TotalProductionQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalProductionQuantity == queryDto.TotalProductionQuantity);
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
