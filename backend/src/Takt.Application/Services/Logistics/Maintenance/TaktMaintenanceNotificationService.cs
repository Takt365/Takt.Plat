// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotificationService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：维护通知单应用服务实现
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
/// 维护通知单应用服务
/// </summary>
public class TaktMaintenanceNotificationService : TaktServiceBase, ITaktMaintenanceNotificationService
{
    private readonly ITaktApprovalRepository<TaktMaintenanceNotification> _maintenanceNotificationRepository;
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceNotificationRepository">维护通知单仓储</param>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceNotificationService(
        ITaktApprovalRepository<TaktMaintenanceNotification> maintenanceNotificationRepository,
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceNotificationRepository = maintenanceNotificationRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取维护通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceNotificationDto>> GetMaintenanceNotificationListAsync(TaktMaintenanceNotificationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceNotificationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceNotificationDto>.Create(
            data.Adapt<List<TaktMaintenanceNotificationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto?> GetMaintenanceNotificationByIdAsync(long id)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceNotificationDto>();
    }

    /// <summary>
    /// 获取维护通知单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceNotificationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceNotificationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NotificationStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EquipmentName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建维护通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> CreateMaintenanceNotificationAsync(TaktMaintenanceNotificationCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceNotification>();
        await StampMaintenanceNotificationEquipmentAsync(entity, dto);
        await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceNotificationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.NotificationCode == entity.NotificationCode);
        if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
        {
            throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
        }
        entity = await _maintenanceNotificationRepository.CreateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceNotificationDto>();
    }

    /// <summary>
    /// 更新维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationAsync(long id, TaktMaintenanceNotificationUpdateDto dto)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护通知单不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceNotificationEquipmentAsync(entity, dto);
        await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceNotificationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.NotificationCode == entity.NotificationCode,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
        {
            throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
        }
        await _maintenanceNotificationRepository.UpdateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(id) ?? throw new TaktBusinessException("维护通知单不存在");
    }

    /// <summary>
    /// 删除维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceNotificationByIdAsync(long id)
    {
        var deleted = await _maintenanceNotificationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("维护通知单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除维护通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceNotificationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceNotificationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新维护通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationStatusAsync(TaktMaintenanceNotificationStatusDto dto)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(dto.MaintenanceNotificationId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护通知单不存在");
        }
        entity.NotificationStatus = dto.NotificationStatus;
        await _maintenanceNotificationRepository.UpdateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(dto.MaintenanceNotificationId) ?? throw new TaktBusinessException("维护通知单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceNotificationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceNotificationTemplateDto>(
            sheetName ?? "维护通知单导入模板",
            fileName ?? "维护通知单导入模板.xlsx");
    }

    /// <summary>
    /// 导入维护通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceNotificationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceNotificationImportDto>(fileStream, sheetName ?? "维护通知单导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceNotification>();
                var importDto = rows[i].Adapt<TaktMaintenanceNotificationCreateDto>();
                await StampMaintenanceNotificationEquipmentAsync(entity, importDto);
                await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.NotificationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、NotificationCode）");
                }
                var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceNotificationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.NotificationCode == entity.NotificationCode);
                if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
                {
                    throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
                }
                await _maintenanceNotificationRepository.CreateAsync(entity);
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
    /// 导出维护通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceNotificationAsync(TaktMaintenanceNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaintenanceNotificationQueryDto());
        var list = await _maintenanceNotificationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceNotificationExportDto>(),
                sheetName ?? "维护通知单数据",
                fileName ?? "维护通知单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceNotificationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "维护通知单数据",
            fileName ?? "维护通知单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步维护通知单主表外键（ManyToOne → 工厂设备）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceNotificationEquipmentAsync(TaktMaintenanceNotification entity, TaktMaintenanceNotificationCreateDto dto)
    {
        if (dto.EquipmentId <= 0)
        {
            return;
        }
        var master = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
        if (master == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        entity.EquipmentId = master.Id;
    }

    /// <summary>
    /// 同步维护通知单主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceNotificationMaintenanceWorkOrderAsync(TaktMaintenanceNotification entity, TaktMaintenanceNotificationCreateDto dto)
    {
        if (dto.MaintenanceWorkOrderId is not > 0)
        {
            return;
        }
        var master = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("维护工单不存在");
        }
        entity.MaintenanceWorkOrderId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建维护通知单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceNotification, bool>> QueryExpression(TaktMaintenanceNotificationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceNotification>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.NotificationCode != null && x.NotificationCode.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentId).Contains(keywords)
                || (x.EquipmentCode != null && x.EquipmentCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceCategory).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || SqlFunc.ToString(x.NotificationStatus).Contains(keywords)
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.ReportedBy != null && x.ReportedBy.Contains(keywords))
                || SqlFunc.ToString(x.CostCenterId).Contains(keywords)
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceWorkOrderId).Contains(keywords)
                || (x.MaintenanceWorkOrderCode != null && x.MaintenanceWorkOrderCode.Contains(keywords))
                || (x.NotificationImages != null && x.NotificationImages.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.DiscoveredAt).Contains(keywords)
                || SqlFunc.ToString(x.BreakdownStartTime).Contains(keywords)
                || SqlFunc.ToString(x.BreakdownEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.NotificationCode))
        {
            exp = exp.And(x => x.NotificationCode != null && x.NotificationCode.Contains(queryDto.NotificationCode));
        }

        if (queryDto?.EquipmentId.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentId == queryDto.EquipmentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentCode))
        {
            exp = exp.And(x => x.EquipmentCode != null && x.EquipmentCode.Contains(queryDto.EquipmentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentName))
        {
            exp = exp.And(x => x.EquipmentName != null && x.EquipmentName.Contains(queryDto.EquipmentName));
        }

        if (queryDto?.MaintenanceCategory.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCategory == queryDto.MaintenanceCategory);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (queryDto?.NotificationStatus.HasValue == true)
        {
            exp = exp.And(x => x.NotificationStatus == queryDto.NotificationStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.FaultDescription))
        {
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(queryDto.FaultDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReportedBy))
        {
            exp = exp.And(x => x.ReportedBy != null && x.ReportedBy.Contains(queryDto.ReportedBy));
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterId == queryDto.CostCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (queryDto?.MaintenanceWorkOrderId.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceWorkOrderId == queryDto.MaintenanceWorkOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceWorkOrderCode))
        {
            exp = exp.And(x => x.MaintenanceWorkOrderCode != null && x.MaintenanceWorkOrderCode.Contains(queryDto.MaintenanceWorkOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.NotificationImages))
        {
            exp = exp.And(x => x.NotificationImages != null && x.NotificationImages.Contains(queryDto.NotificationImages));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.DiscoveredAtStart.HasValue == true)
        {
            exp = exp.And(x => x.DiscoveredAt >= queryDto.DiscoveredAtStart);
        }

        if (queryDto?.DiscoveredAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.DiscoveredAt <= queryDto.DiscoveredAtEnd);
        }

        if (queryDto?.BreakdownStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.BreakdownStartTime >= queryDto.BreakdownStartTimeStart);
        }

        if (queryDto?.BreakdownStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.BreakdownStartTime <= queryDto.BreakdownStartTimeEnd);
        }

        if (queryDto?.BreakdownEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.BreakdownEndTime >= queryDto.BreakdownEndTimeStart);
        }

        if (queryDto?.BreakdownEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.BreakdownEndTime <= queryDto.BreakdownEndTimeEnd);
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
