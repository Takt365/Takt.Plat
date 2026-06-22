// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsOrderService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Entities.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Scheduling;

/// <summary>
/// APS排程订单应用服务
/// </summary>
public class TaktApsOrderService : TaktServiceBase, ITaktApsOrderService
{
    private readonly ITaktCompanyRepository<TaktApsOrder> _apsOrderRepository;
    private readonly ITaktCompanyRepository<TaktApsOperation> _apsOperationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOrderRepository">APS排程订单仓储</param>
    /// <param name="apsOperationRepository">ApsOperation仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsOrderService(
        ITaktCompanyRepository<TaktApsOrder> apsOrderRepository,
        ITaktCompanyRepository<TaktApsOperation> apsOperationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsOrderRepository = apsOrderRepository;
        _apsOperationRepository = apsOperationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsOrderDto>> GetApsOrderListAsync(TaktApsOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsOrderDto>.Create(
            data.Adapt<List<TaktApsOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto?> GetApsOrderByIdAsync(long id)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktApsOrderDto>();
        await FillApsOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取APS排程订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _apsOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> CreateApsOrderAsync(TaktApsOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsOrder>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsOrderCode == entity.ApsOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique)
        {
            throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
        }
        entity = await _apsOrderRepository.CreateAsync(entity);
                await SaveApsOrderChildrenAsync(entity, dto);
        return await GetApsOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktApsOrderDto>();
    }

    /// <summary>
    /// 更新APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> UpdateApsOrderAsync(long id, TaktApsOrderUpdateDto dto)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsOrderCode == entity.ApsOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique)
        {
            throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
        }
        await _apsOrderRepository.UpdateAsync(entity);
                await SaveApsOrderChildrenAsync(entity, dto);
        return await GetApsOrderByIdAsync(id) ?? throw new TaktBusinessException("APS排程订单不存在");
    }

    /// <summary>
    /// 删除APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOrderByIdAsync(long id)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在或已删除");
        }
        await _apsOperationRepository.DeleteAsync(x => x.ApsOrderId == entity.Id);
        var deleted = await _apsOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> UpdateApsOrderStatusAsync(TaktApsOrderStatusDto dto)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(dto.ApsOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _apsOrderRepository.UpdateAsync(entity);
        return await GetApsOrderByIdAsync(dto.ApsOrderId) ?? throw new TaktBusinessException("APS排程订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsOrderTemplateDto>(
            sheetName ?? "APS排程订单导入模板",
            fileName ?? "APS排程订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsOrderImportDto>(fileStream, sheetName ?? "APS排程订单导入模板");
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
                var entity = rows[i].Adapt<TaktApsOrder>();
                var importKey = $"{entity.PlantCode}|{entity.ApsOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ApsOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ApsOrderCode == entity.ApsOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_order_unique)
                {
                    throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
                }
                await _apsOrderRepository.CreateAsync(entity);
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
    /// 导出APS排程订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsOrderAsync(TaktApsOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktApsOrderQueryDto());
        var list = await _apsOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsOrderExportDto>(),
                sheetName ?? "APS排程订单数据",
                fileName ?? "APS排程订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程订单数据",
            fileName ?? "APS排程订单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充APS排程订单详情（加载 OneToMany 子表：APS工序排程）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillApsOrderDetailsAsync(TaktApsOrderDto dto, TaktApsOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // APS工序排程 → dto.Operations
        var operations = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == entity.Id);
        dto.Operations = operations.Adapt<List<TaktApsOperationDto>>();
    }

    /// <summary>
    /// 保存APS排程订单子表级联（APS工序排程；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveApsOrderChildrenAsync(TaktApsOrder entity, TaktApsOrderCreateDto dto)
    {
        // APS工序排程（Operations）
        if (dto.Operations is not { Count: > 0 })
        {
            await _apsOperationRepository.DeleteAsync(x => x.ApsOrderId == entity.Id);
        }
        else
        {
            var operations = dto.Operations.Adapt<List<TaktApsOperation>>();
            foreach (var child in operations)
            {
                child.ApsOrderId = entity.Id;
            }
            var operationsNeedLine = operations.Where(c => c.LineNumber <= 0).ToList();
            if (operationsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.ApsOrderCode) ? entity.ApsOrderCode : entity.Id.ToString();
                var maxLine = await _apsOperationRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsOrderId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, operationsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in operations)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < operations.Count; i++)
                        {
                            var key = $"{operations[i].CompanyCode}|{operations[i].ApsOrderId}|{operations[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"APS工序排程第{i + 1}项与本次提交的其他项重复（CompanyCode、ApsOrderId、LineNumber）");
                            }
                        }
            await _apsOperationRepository.DeleteAsync(x => x.ApsOrderId == entity.Id);
            foreach (var child in operations)
            {
            var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
                _apsOperationRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ApsOrderId == child.ApsOrderId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_operation_line_unique)
            {
                throw new TaktBusinessException("APS工序排程的CompanyCode、ApsOrderId、LineNumber已存在");
            }
            }
            await _apsOperationRepository.CreateRangeAsync(operations);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsOrder, bool>> QueryExpression(TaktApsOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ApsOrderCode != null && x.ApsOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.PlannedOrderId).Contains(keywords)
                || (x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.OrderQuantity).Contains(keywords)
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || SqlFunc.ToString(x.OrderStatus).Contains(keywords)
                || SqlFunc.ToString(x.ApsScheduleId).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ApsOrderCode))
        {
            exp = exp.And(x => x.ApsOrderCode != null && x.ApsOrderCode.Contains(queryDto.ApsOrderCode));
        }

        if (queryDto?.PlannedOrderId.HasValue == true)
        {
            exp = exp.And(x => x.PlannedOrderId == queryDto.PlannedOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlannedOrderCode))
        {
            exp = exp.And(x => x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(queryDto.PlannedOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.OrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.OrderQuantity == queryDto.OrderQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitOfMeasure))
        {
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(queryDto.UnitOfMeasure));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingCode))
        {
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(queryDto.RoutingCode));
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.OrderStatus == queryDto.OrderStatus);
        }

        if (queryDto?.ApsScheduleId.HasValue == true)
        {
            exp = exp.And(x => x.ApsScheduleId == queryDto.ApsScheduleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime >= queryDto.PlannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime <= queryDto.PlannedEndTimeEnd);
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
