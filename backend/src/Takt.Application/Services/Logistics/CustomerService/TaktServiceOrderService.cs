// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktServiceOrderService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：服务订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Domain.Entities.Logistics.CustomerService;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 服务订单应用服务
/// </summary>
public class TaktServiceOrderService : TaktServiceBase, ITaktServiceOrderService
{
    private readonly ITaktCompanyRepository<TaktServiceOrder> _serviceOrderRepository;
    private readonly ITaktCompanyRepository<TaktServiceTicket> _serviceTicketRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceOrderRepository">服务订单仓储</param>
    /// <param name="serviceTicketRepository">ServiceTicket仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktServiceOrderService(
        ITaktCompanyRepository<TaktServiceOrder> serviceOrderRepository,
        ITaktCompanyRepository<TaktServiceTicket> serviceTicketRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _serviceTicketRepository = serviceTicketRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktServiceOrderDto>> GetServiceOrderListAsync(TaktServiceOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serviceOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktServiceOrderDto>.Create(
            data.Adapt<List<TaktServiceOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceOrderDto?> GetServiceOrderByIdAsync(long id)
    {
        var entity = await _serviceOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktServiceOrderDto>();
        await FillServiceOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取服务订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetServiceOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _serviceOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.ServiceOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceOrderCode,
            DictLabel = e.ServiceOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceOrderDto> CreateServiceOrderAsync(TaktServiceOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktServiceOrder>();
        var isUnique_ix_takt_logistics_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceOrderCode == entity.ServiceOrderCode);
        if (!isUnique_ix_takt_logistics_service_order_code_unique)
        {
            throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _serviceOrderRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _serviceOrderRepository.CreateAsync(entity);
                await SaveServiceOrderChildrenAsync(entity, dto);
        return await GetServiceOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktServiceOrderDto>();
    }

    /// <summary>
    /// 更新服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceOrderDto> UpdateServiceOrderAsync(long id, TaktServiceOrderUpdateDto dto)
    {
        var entity = await _serviceOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceOrderCode == entity.ServiceOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_service_order_code_unique)
        {
            throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
        }
        await _serviceOrderRepository.UpdateAsync(entity);
                await SaveServiceOrderChildrenAsync(entity, dto);
        return await GetServiceOrderByIdAsync(id) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 删除服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceOrderByIdAsync(long id)
    {
        var entity = await _serviceOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在或已删除");
        }
        await _serviceTicketRepository.DeleteAsync(x => x.ServiceOrderId == entity.Id);
        var deleted = await _serviceOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteServiceOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceOrderDto> UpdateServiceOrderStatusAsync(TaktServiceOrderStatusDto dto)
    {
        var entity = await _serviceOrderRepository.GetByIdAsync(dto.ServiceOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _serviceOrderRepository.UpdateAsync(entity);
        return await GetServiceOrderByIdAsync(dto.ServiceOrderId) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 更新服务订单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceOrderDto> UpdateServiceOrderSortAsync(TaktServiceOrderSortDto dto)
    {
        var entity = await _serviceOrderRepository.GetByIdAsync(dto.ServiceOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _serviceOrderRepository.UpdateAsync(entity);
        return await GetServiceOrderByIdAsync(dto.ServiceOrderId) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetServiceOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktServiceOrderTemplateDto>(
            sheetName ?? "服务订单导入模板",
            fileName ?? "服务订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportServiceOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktServiceOrderImportDto>(fileStream, sheetName ?? "服务订单导入模板");
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
                var entity = rows[i].Adapt<TaktServiceOrder>();
                var importKey = $"{entity.PlantCode}|{entity.ServiceOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceOrderCode）");
                }
                var isUnique_ix_takt_logistics_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _serviceOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceOrderCode == entity.ServiceOrderCode);
                if (!isUnique_ix_takt_logistics_service_order_code_unique)
                {
                    throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _serviceOrderRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _serviceOrderRepository.CreateAsync(entity);
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
    /// 导出服务订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportServiceOrderAsync(TaktServiceOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktServiceOrderQueryDto());
        var list = await _serviceOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktServiceOrderExportDto>(),
                sheetName ?? "服务订单数据",
                fileName ?? "服务订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktServiceOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务订单数据",
            fileName ?? "服务订单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充服务订单详情（加载 OneToMany 子表：服务工单）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillServiceOrderDetailsAsync(TaktServiceOrderDto dto, TaktServiceOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 服务工单 → dto.Tickets
        var tickets = await _serviceTicketRepository.GetListAsync(x => x.ServiceOrderId == entity.Id);
        dto.Tickets = tickets.Adapt<List<TaktServiceTicketDto>>();
    }

    /// <summary>
    /// 保存服务订单子表级联（服务工单；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveServiceOrderChildrenAsync(TaktServiceOrder entity, TaktServiceOrderCreateDto dto)
    {
        // 服务工单（Tickets）
        List<TaktServiceTicketUpdateDto>? ticketsForSave;
        if (dto is TaktServiceOrderUpdateDto updateDtoForTickets && updateDtoForTickets.Tickets != null)
        {
            ticketsForSave = updateDtoForTickets.Tickets;
        }
        else if (dto.Tickets != null)
        {
            ticketsForSave = dto.Tickets.Adapt<List<TaktServiceTicketUpdateDto>>();
        }
        else
        {
            ticketsForSave = null;
        }
        if (ticketsForSave is not { Count: > 0 })
        {
            await _serviceTicketRepository.DeleteAsync(x => x.ServiceOrderId == entity.Id);
        }
        else
        {
            var existingList = await _serviceTicketRepository.GetListAsync(x => x.ServiceOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktServiceTicket>();
            for (var i = 0; i < ticketsForSave.Count; i++)
            {
                var childDto = ticketsForSave[i];
                childDto.ServiceOrderId = entity.Id;
                if (childDto.ServiceTicketId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ServiceTicketId, out var target))
                    {
                        throw new TaktBusinessException("服务工单不存在（ServiceTicketId={childDto.ServiceTicketId}）");
                    }
                    if (target.ServiceOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("服务工单不属于当前主表（ServiceTicketId={childDto.ServiceTicketId}）");
                    }
                    submittedIds.Add(childDto.ServiceTicketId);
                    childDto.Adapt(target);
                    target.Id = childDto.ServiceTicketId;
                    target.ServiceOrderId = entity.Id;
                    await _serviceTicketRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktServiceTicket>();
                    child.Id = 0;
                    child.ServiceOrderId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _serviceTicketRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _serviceTicketRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktServiceOrder, bool>> QueryExpression(TaktServiceOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktServiceOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.ClientId).Contains(keywords)
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || SqlFunc.ToString(x.ServiceContractId).Contains(keywords)
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || SqlFunc.ToString(x.ServiceRequestId).Contains(keywords)
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.OrderType).Contains(keywords)
                || SqlFunc.ToString(x.OrderStatus).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.DiscountAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || SqlFunc.ToString(x.ActualAmount).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ServiceBy != null && x.ServiceBy.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OrderDate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedStartDate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualStartDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualEndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceOrderCode))
        {
            exp = exp.And(x => x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(queryDto.ServiceOrderCode));
        }

        if (queryDto?.ClientId.HasValue == true)
        {
            exp = exp.And(x => x.ClientId == queryDto.ClientId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientCode))
        {
            exp = exp.And(x => x.ClientCode != null && x.ClientCode.Contains(queryDto.ClientCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientName1))
        {
            exp = exp.And(x => x.ClientName1 != null && x.ClientName1.Contains(queryDto.ClientName1));
        }

        if (queryDto?.ServiceContractId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceContractId == queryDto.ServiceContractId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceContractCode))
        {
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(queryDto.ServiceContractCode));
        }

        if (queryDto?.ServiceRequestId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceRequestId == queryDto.ServiceRequestId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceRequestCode))
        {
            exp = exp.And(x => x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(queryDto.ServiceRequestCode));
        }

        if (queryDto?.OrderType.HasValue == true)
        {
            exp = exp.And(x => x.OrderType == queryDto.OrderType);
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.OrderStatus == queryDto.OrderStatus);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            exp = exp.And(x => x.DiscountAmount == queryDto.DiscountAmount);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.TaxAmount == queryDto.TaxAmount);
        }

        if (queryDto?.ActualAmount.HasValue == true)
        {
            exp = exp.And(x => x.ActualAmount == queryDto.ActualAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceBy))
        {
            exp = exp.And(x => x.ServiceBy != null && x.ServiceBy.Contains(queryDto.ServiceBy));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.OrderDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OrderDate >= queryDto.OrderDateStart);
        }

        if (queryDto?.OrderDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OrderDate <= queryDto.OrderDateEnd);
        }

        if (queryDto?.PlannedStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartDate >= queryDto.PlannedStartDateStart);
        }

        if (queryDto?.PlannedStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartDate <= queryDto.PlannedStartDateEnd);
        }

        if (queryDto?.PlannedEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndDate >= queryDto.PlannedEndDateStart);
        }

        if (queryDto?.PlannedEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndDate <= queryDto.PlannedEndDateEnd);
        }

        if (queryDto?.ActualStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartDate >= queryDto.ActualStartDateStart);
        }

        if (queryDto?.ActualStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartDate <= queryDto.ActualStartDateEnd);
        }

        if (queryDto?.ActualEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndDate >= queryDto.ActualEndDateStart);
        }

        if (queryDto?.ActualEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndDate <= queryDto.ActualEndDateEnd);
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
