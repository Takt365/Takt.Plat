// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktEquipmentService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂设备应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Domain.Entities.Logistics.Maintenance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 工厂设备应用服务
/// </summary>
public class TaktEquipmentService : TaktServiceBase, ITaktEquipmentService
{
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceNotification> _maintenanceNotificationRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceHistory> _maintenanceHistoryRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceNotificationRepository">MaintenanceNotification仓储</param>
    /// <param name="maintenanceWorkOrderRepository">MaintenanceWorkOrder仓储</param>
    /// <param name="maintenanceHistoryRepository">MaintenanceHistory仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEquipmentService(
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceNotification> maintenanceNotificationRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktCompanyRepository<TaktMaintenanceHistory> maintenanceHistoryRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _equipmentRepository = equipmentRepository;
        _maintenanceNotificationRepository = maintenanceNotificationRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _maintenanceHistoryRepository = maintenanceHistoryRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂设备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEquipmentDto>> GetEquipmentListAsync(TaktEquipmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _equipmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEquipmentDto>.Create(
            data.Adapt<List<TaktEquipmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto?> GetEquipmentByIdAsync(long id)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEquipmentDto>();
        await FillEquipmentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工厂设备选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEquipmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _equipmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WarrantyStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EquipmentName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工厂设备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> CreateEquipmentAsync(TaktEquipmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEquipment>();
        var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipmentCode == entity.EquipmentCode);
        if (!isUnique_ix_equipment_code_unique)
        {
            throw new TaktBusinessException("工厂设备的PlantCode、EquipmentCode已存在");
        }
        entity = await _equipmentRepository.CreateAsync(entity);
                await SaveEquipmentChildrenAsync(entity, dto);
        return await GetEquipmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEquipmentDto>();
    }

    /// <summary>
    /// 更新工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> UpdateEquipmentAsync(long id, TaktEquipmentUpdateDto dto)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipmentCode == entity.EquipmentCode,
            id);
        if (!isUnique_ix_equipment_code_unique)
        {
            throw new TaktBusinessException("工厂设备的PlantCode、EquipmentCode已存在");
        }
        await _equipmentRepository.UpdateAsync(entity);
                await SaveEquipmentChildrenAsync(entity, dto);
        return await GetEquipmentByIdAsync(id) ?? throw new TaktBusinessException("工厂设备不存在");
    }

    /// <summary>
    /// 删除工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentByIdAsync(long id)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在或已删除");
        }
        await _maintenanceNotificationRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        await _maintenanceWorkOrderRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        await _maintenanceHistoryRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        var deleted = await _equipmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂设备不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂设备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEquipmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工厂设备状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> UpdateEquipmentStatusAsync(TaktEquipmentStatusDto dto)
    {
        var entity = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        entity.WarrantyStatus = dto.WarrantyStatus;
        await _equipmentRepository.UpdateAsync(entity);
        return await GetEquipmentByIdAsync(dto.EquipmentId) ?? throw new TaktBusinessException("工厂设备不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEquipmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEquipmentTemplateDto>(
            sheetName ?? "工厂设备导入模板",
            fileName ?? "工厂设备导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂设备
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEquipmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEquipmentImportDto>(fileStream, sheetName ?? "工厂设备导入模板");
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
                var entity = rows[i].Adapt<TaktEquipment>();
                var importKey = $"{entity.PlantCode}|{entity.EquipmentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EquipmentCode）");
                }
                var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _equipmentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EquipmentCode == entity.EquipmentCode);
                if (!isUnique_ix_equipment_code_unique)
                {
                    throw new TaktBusinessException("工厂设备的PlantCode、EquipmentCode已存在");
                }
                await _equipmentRepository.CreateAsync(entity);
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
    /// 导出工厂设备
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEquipmentAsync(TaktEquipmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEquipmentQueryDto());
        var list = await _equipmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEquipmentExportDto>(),
                sheetName ?? "工厂设备数据",
                fileName ?? "工厂设备导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEquipmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂设备数据",
            fileName ?? "工厂设备导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工厂设备详情（加载 OneToMany 子表：维护通知单、维护工单、设备维护履历）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEquipmentDetailsAsync(TaktEquipmentDto dto, TaktEquipment entity)
    {
        if (dto == null)
        {
            return;
        }
        // 维护通知单 → dto.MaintenanceNotifications
        var maintenancenotifications = await _maintenanceNotificationRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceNotifications = maintenancenotifications.Adapt<List<TaktMaintenanceNotificationDto>>();
        // 维护工单 → dto.MaintenanceWorkOrders
        var maintenanceworkorders = await _maintenanceWorkOrderRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceWorkOrders = maintenanceworkorders.Adapt<List<TaktMaintenanceWorkOrderDto>>();
        // 设备维护履历 → dto.MaintenanceHistories
        var maintenancehistories = await _maintenanceHistoryRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceHistories = maintenancehistories.Adapt<List<TaktMaintenanceHistoryDto>>();
    }

    /// <summary>
    /// 保存工厂设备子表级联（维护通知单、维护工单、设备维护履历；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEquipmentChildrenAsync(TaktEquipment entity, TaktEquipmentCreateDto dto)
    {
        // 维护通知单（MaintenanceNotifications）
        if (dto.MaintenanceNotifications is not { Count: > 0 })
        {
            await _maintenanceNotificationRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var maintenancenotifications = dto.MaintenanceNotifications.Adapt<List<TaktMaintenanceNotification>>();
            foreach (var child in maintenancenotifications)
            {
                child.EquipmentId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < maintenancenotifications.Count; i++)
                        {
                            var key = $"{maintenancenotifications[i].CompanyCode}|{maintenancenotifications[i].PlantCode}|{maintenancenotifications[i].NotificationCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"维护通知单第{i + 1}项与本次提交的其他项重复（CompanyCode、PlantCode、NotificationCode）");
                            }
                        }
            await _maintenanceNotificationRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
            foreach (var child in maintenancenotifications)
            {
            var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                _maintenanceNotificationRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PlantCode == child.PlantCode
                    && x.NotificationCode == child.NotificationCode);
            if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
            {
                throw new TaktBusinessException("维护通知单的CompanyCode、PlantCode、NotificationCode已存在");
            }
            }
            await _maintenanceNotificationRepository.CreateRangeAsync(maintenancenotifications);
        }
        // 维护工单（MaintenanceWorkOrders）
        if (dto.MaintenanceWorkOrders is not { Count: > 0 })
        {
            await _maintenanceWorkOrderRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var maintenanceworkorders = dto.MaintenanceWorkOrders.Adapt<List<TaktMaintenanceWorkOrder>>();
            foreach (var child in maintenanceworkorders)
            {
                child.EquipmentId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < maintenanceworkorders.Count; i++)
                        {
                            var key = $"{maintenanceworkorders[i].CompanyCode}|{maintenanceworkorders[i].PlantCode}|{maintenanceworkorders[i].WorkOrderCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"维护工单第{i + 1}项与本次提交的其他项重复（CompanyCode、PlantCode、WorkOrderCode）");
                            }
                        }
            await _maintenanceWorkOrderRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
            foreach (var child in maintenanceworkorders)
            {
            var isUnique_ix_takt_logistics_maintenance_work_order_code_unique = await _uniqueValidator.IsUniqueAsync(
                _maintenanceWorkOrderRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PlantCode == child.PlantCode
                    && x.WorkOrderCode == child.WorkOrderCode);
            if (!isUnique_ix_takt_logistics_maintenance_work_order_code_unique)
            {
                throw new TaktBusinessException("维护工单的CompanyCode、PlantCode、WorkOrderCode已存在");
            }
            }
            await _maintenanceWorkOrderRepository.CreateRangeAsync(maintenanceworkorders);
        }
        // 设备维护履历（MaintenanceHistories）
        if (dto.MaintenanceHistories is not { Count: > 0 })
        {
            await _maintenanceHistoryRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var maintenancehistories = dto.MaintenanceHistories.Adapt<List<TaktMaintenanceHistory>>();
            foreach (var child in maintenancehistories)
            {
                child.EquipmentId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < maintenancehistories.Count; i++)
                        {
                            var key = $"{maintenancehistories[i].CompanyCode}|{maintenancehistories[i].MaintenanceWorkOrderId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设备维护履历第{i + 1}项与本次提交的其他项重复（CompanyCode、MaintenanceWorkOrderId）");
                            }
                        }
            await _maintenanceHistoryRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
            foreach (var child in maintenancehistories)
            {
            var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
                _maintenanceHistoryRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.MaintenanceWorkOrderId == child.MaintenanceWorkOrderId);
            if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
            {
                throw new TaktBusinessException("设备维护履历的CompanyCode、MaintenanceWorkOrderId已存在");
            }
            }
            await _maintenanceHistoryRepository.CreateRangeAsync(maintenancehistories);
        }
    }

    /// <summary>
    /// 获取设备统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设备统计</returns>
    public async Task<TaktEquipmentStatDto> GetEquipmentStatAsync(TaktEquipmentStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktEquipment, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        var monthEquipmentCount = await _equipmentRepository.CountAsync(predicate);
        return new TaktEquipmentStatDto
        {
            StatMonth = statMonth,
            MonthEquipmentCount = monthEquipmentCount,
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂设备查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEquipment, bool>> QueryExpression(TaktEquipmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEquipment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EquipmentCode != null && x.EquipmentCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentType).Contains(keywords)
                || (x.EquipmentModel != null && x.EquipmentModel.Contains(keywords))
                || (x.EquipmentSpecification != null && x.EquipmentSpecification.Contains(keywords))
                || (x.EquipmentBrand != null && x.EquipmentBrand.Contains(keywords))
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.DealerBy != null && x.DealerBy.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.WorkshopBy != null && x.WorkshopBy.Contains(keywords))
                || (x.ProductionLineBy != null && x.ProductionLineBy.Contains(keywords))
                || (x.WorkstationBy != null && x.WorkstationBy.Contains(keywords))
                || (x.DeptBy != null && x.DeptBy.Contains(keywords))
                || (x.EquipmentLocation != null && x.EquipmentLocation.Contains(keywords))
                || (x.ResponsibleUserBy != null && x.ResponsibleUserBy.Contains(keywords))
                || (x.OperatorBy != null && x.OperatorBy.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentOriginalValue).Contains(keywords)
                || (x.TechnicalParameters != null && x.TechnicalParameters.Contains(keywords))
                || (x.EquipmentImages != null && x.EquipmentImages.Contains(keywords))
                || (x.EquipmentDocuments != null && x.EquipmentDocuments.Contains(keywords))
                || SqlFunc.ToString(x.IsCritical).Contains(keywords)
                || SqlFunc.ToString(x.WarrantyStatus).Contains(keywords)
                || SqlFunc.ToString(x.EquipmentStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PurchaseDate).Contains(keywords)
                || SqlFunc.ToString(x.InstallationDate).Contains(keywords)
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.WarrantyStartDate).Contains(keywords)
                || SqlFunc.ToString(x.WarrantyEndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentCode))
        {
            exp = exp.And(x => x.EquipmentCode != null && x.EquipmentCode.Contains(queryDto.EquipmentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentName))
        {
            exp = exp.And(x => x.EquipmentName != null && x.EquipmentName.Contains(queryDto.EquipmentName));
        }

        if (queryDto?.EquipmentType.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentType == queryDto.EquipmentType);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentModel))
        {
            exp = exp.And(x => x.EquipmentModel != null && x.EquipmentModel.Contains(queryDto.EquipmentModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentSpecification))
        {
            exp = exp.And(x => x.EquipmentSpecification != null && x.EquipmentSpecification.Contains(queryDto.EquipmentSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentBrand))
        {
            exp = exp.And(x => x.EquipmentBrand != null && x.EquipmentBrand.Contains(queryDto.EquipmentBrand));
        }

        if (!string.IsNullOrEmpty(queryDto?.Manufacturer))
        {
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(queryDto.Manufacturer));
        }

        if (!string.IsNullOrEmpty(queryDto?.DealerBy))
        {
            exp = exp.And(x => x.DealerBy != null && x.DealerBy.Contains(queryDto.DealerBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNumber))
        {
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(queryDto.SerialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkshopBy))
        {
            exp = exp.And(x => x.WorkshopBy != null && x.WorkshopBy.Contains(queryDto.WorkshopBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLineBy))
        {
            exp = exp.And(x => x.ProductionLineBy != null && x.ProductionLineBy.Contains(queryDto.ProductionLineBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkstationBy))
        {
            exp = exp.And(x => x.WorkstationBy != null && x.WorkstationBy.Contains(queryDto.WorkstationBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptBy))
        {
            exp = exp.And(x => x.DeptBy != null && x.DeptBy.Contains(queryDto.DeptBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentLocation))
        {
            exp = exp.And(x => x.EquipmentLocation != null && x.EquipmentLocation.Contains(queryDto.EquipmentLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleUserBy))
        {
            exp = exp.And(x => x.ResponsibleUserBy != null && x.ResponsibleUserBy.Contains(queryDto.ResponsibleUserBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperatorBy))
        {
            exp = exp.And(x => x.OperatorBy != null && x.OperatorBy.Contains(queryDto.OperatorBy));
        }

        if (queryDto?.EquipmentOriginalValue.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentOriginalValue == queryDto.EquipmentOriginalValue);
        }

        if (!string.IsNullOrEmpty(queryDto?.TechnicalParameters))
        {
            exp = exp.And(x => x.TechnicalParameters != null && x.TechnicalParameters.Contains(queryDto.TechnicalParameters));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentImages))
        {
            exp = exp.And(x => x.EquipmentImages != null && x.EquipmentImages.Contains(queryDto.EquipmentImages));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentDocuments))
        {
            exp = exp.And(x => x.EquipmentDocuments != null && x.EquipmentDocuments.Contains(queryDto.EquipmentDocuments));
        }

        if (queryDto?.IsCritical.HasValue == true)
        {
            exp = exp.And(x => x.IsCritical == queryDto.IsCritical);
        }

        if (queryDto?.WarrantyStatus.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyStatus == queryDto.WarrantyStatus);
        }

        if (queryDto?.EquipmentStatus.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentStatus == queryDto.EquipmentStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PurchaseDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseDate >= queryDto.PurchaseDateStart);
        }

        if (queryDto?.PurchaseDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseDate <= queryDto.PurchaseDateEnd);
        }

        if (queryDto?.InstallationDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InstallationDate >= queryDto.InstallationDateStart);
        }

        if (queryDto?.InstallationDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InstallationDate <= queryDto.InstallationDateEnd);
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.WarrantyStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyStartDate >= queryDto.WarrantyStartDateStart);
        }

        if (queryDto?.WarrantyStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyStartDate <= queryDto.WarrantyStartDateEnd);
        }

        if (queryDto?.WarrantyEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyEndDate >= queryDto.WarrantyEndDateStart);
        }

        if (queryDto?.WarrantyEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.WarrantyEndDate <= queryDto.WarrantyEndDateEnd);
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
