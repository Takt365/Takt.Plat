// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoryService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护履历应用服务实现
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
/// 设备维护履历应用服务
/// </summary>
public class TaktMaintenanceHistoryService : TaktServiceBase, ITaktMaintenanceHistoryService
{
    private readonly ITaktCompanyRepository<TaktMaintenanceHistory> _maintenanceHistoryRepository;
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceHistoryRepository">设备维护履历仓储</param>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceHistoryService(
        ITaktCompanyRepository<TaktMaintenanceHistory> maintenanceHistoryRepository,
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceHistoryRepository = maintenanceHistoryRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设备维护履历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceHistoryDto>> GetMaintenanceHistoryListAsync(TaktMaintenanceHistoryQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceHistoryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceHistoryDto>.Create(
            data.Adapt<List<TaktMaintenanceHistoryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto?> GetMaintenanceHistoryByIdAsync(long id)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceHistoryDto>();
    }

    /// <summary>
    /// 获取设备维护履历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceHistoryOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceHistoryRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceStatus == 1,
            x => x.WorkOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.WorkOrderCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设备维护履历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> CreateMaintenanceHistoryAsync(TaktMaintenanceHistoryCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceHistory>();
        await StampMaintenanceHistoryEquipmentAsync(entity, dto);
        await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceHistoryRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId);
        if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
        {
            throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
        }
        entity = await _maintenanceHistoryRepository.CreateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceHistoryDto>();
    }

    /// <summary>
    /// 更新设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryAsync(long id, TaktMaintenanceHistoryUpdateDto dto)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护履历不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceHistoryEquipmentAsync(entity, dto);
        await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceHistoryRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
        {
            throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
        }
        await _maintenanceHistoryRepository.UpdateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(id) ?? throw new TaktBusinessException("设备维护履历不存在");
    }

    /// <summary>
    /// 删除设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceHistoryByIdAsync(long id)
    {
        var deleted = await _maintenanceHistoryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设备维护履历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设备维护履历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceHistoryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceHistoryByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设备维护履历状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryStatusAsync(TaktMaintenanceHistoryStatusDto dto)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(dto.MaintenanceHistoryId);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护履历不存在");
        }
        entity.MaintenanceStatus = dto.MaintenanceStatus;
        await _maintenanceHistoryRepository.UpdateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(dto.MaintenanceHistoryId) ?? throw new TaktBusinessException("设备维护履历不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceHistoryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceHistoryTemplateDto>(
            sheetName ?? "设备维护履历导入模板",
            fileName ?? "设备维护履历导入模板.xlsx");
    }

    /// <summary>
    /// 导入设备维护履历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceHistoryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceHistoryImportDto>(fileStream, sheetName ?? "设备维护履历导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceHistory>();
                var importDto = rows[i].Adapt<TaktMaintenanceHistoryCreateDto>();
                await StampMaintenanceHistoryEquipmentAsync(entity, importDto);
                await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, importDto);
                var importKey = $"{entity.MaintenanceWorkOrderId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaintenanceWorkOrderId）");
                }
                var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceHistoryRepository,
                    x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId);
                if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
                {
                    throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
                }
                await _maintenanceHistoryRepository.CreateAsync(entity);
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
    /// 导出设备维护履历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceHistoryAsync(TaktMaintenanceHistoryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaintenanceHistoryQueryDto());
        var list = await _maintenanceHistoryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceHistoryExportDto>(),
                sheetName ?? "设备维护履历数据",
                fileName ?? "设备维护履历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceHistoryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设备维护履历数据",
            fileName ?? "设备维护履历导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设备维护履历主表外键（ManyToOne → 工厂设备）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceHistoryEquipmentAsync(TaktMaintenanceHistory entity, TaktMaintenanceHistoryCreateDto dto)
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
    /// 同步设备维护履历主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceHistoryMaintenanceWorkOrderAsync(TaktMaintenanceHistory entity, TaktMaintenanceHistoryCreateDto dto)
    {
        if (dto.MaintenanceWorkOrderId <= 0)
        {
            return;
        }
        var master = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId);
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
    /// 构建设备维护履历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceHistory, bool>> QueryExpression(TaktMaintenanceHistoryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceHistory>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.MaintenanceWorkOrderId).Contains(keywords)
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentId).Contains(keywords)
                || (x.EquipmentCode != null && x.EquipmentCode.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceType).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceCategory).Contains(keywords)
                || (x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(keywords))
                || (x.MaintenanceTechnician != null && x.MaintenanceTechnician.Contains(keywords))
                || (x.MaintenanceContent != null && x.MaintenanceContent.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.Solution != null && x.Solution.Contains(keywords))
                || (x.UsedParts != null && x.UsedParts.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceCost).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceResult).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceStatus).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceCycleDays).Contains(keywords)
                || (x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(keywords))
                || (x.MaintenanceImages != null && x.MaintenanceImages.Contains(keywords))
                || (x.AcceptedSummary != null && x.AcceptedSummary.Contains(keywords))
                || (x.AcceptedBy != null && x.AcceptedBy.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceStartTime).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceEndTime).Contains(keywords)
                || SqlFunc.ToString(x.NextMaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.AcceptedAt).Contains(keywords)
                || SqlFunc.ToString(x.ArchivedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MaintenanceWorkOrderId.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceWorkOrderId == queryDto.MaintenanceWorkOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkOrderCode))
        {
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(queryDto.WorkOrderCode));
        }

        if (queryDto?.EquipmentId.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentId == queryDto.EquipmentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentCode))
        {
            exp = exp.And(x => x.EquipmentCode != null && x.EquipmentCode.Contains(queryDto.EquipmentCode));
        }

        if (queryDto?.MaintenanceType.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceType == queryDto.MaintenanceType);
        }

        if (queryDto?.MaintenanceCategory.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCategory == queryDto.MaintenanceCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceCompany))
        {
            exp = exp.And(x => x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(queryDto.MaintenanceCompany));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceTechnician))
        {
            exp = exp.And(x => x.MaintenanceTechnician != null && x.MaintenanceTechnician.Contains(queryDto.MaintenanceTechnician));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceContent))
        {
            exp = exp.And(x => x.MaintenanceContent != null && x.MaintenanceContent.Contains(queryDto.MaintenanceContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.FaultDescription))
        {
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(queryDto.FaultDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.Solution))
        {
            exp = exp.And(x => x.Solution != null && x.Solution.Contains(queryDto.Solution));
        }

        if (!string.IsNullOrEmpty(queryDto?.UsedParts))
        {
            exp = exp.And(x => x.UsedParts != null && x.UsedParts.Contains(queryDto.UsedParts));
        }

        if (queryDto?.MaintenanceCost.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCost == queryDto.MaintenanceCost);
        }

        if (queryDto?.MaintenanceResult.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceResult == queryDto.MaintenanceResult);
        }

        if (queryDto?.MaintenanceStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceStatus == queryDto.MaintenanceStatus);
        }

        if (queryDto?.MaintenanceCycleDays.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCycleDays == queryDto.MaintenanceCycleDays);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceDocuments))
        {
            exp = exp.And(x => x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(queryDto.MaintenanceDocuments));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceImages))
        {
            exp = exp.And(x => x.MaintenanceImages != null && x.MaintenanceImages.Contains(queryDto.MaintenanceImages));
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptedSummary))
        {
            exp = exp.And(x => x.AcceptedSummary != null && x.AcceptedSummary.Contains(queryDto.AcceptedSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptedBy))
        {
            exp = exp.And(x => x.AcceptedBy != null && x.AcceptedBy.Contains(queryDto.AcceptedBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.MaintenanceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceDate >= queryDto.MaintenanceDateStart);
        }

        if (queryDto?.MaintenanceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceDate <= queryDto.MaintenanceDateEnd);
        }

        if (queryDto?.MaintenanceStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceStartTime >= queryDto.MaintenanceStartTimeStart);
        }

        if (queryDto?.MaintenanceStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceStartTime <= queryDto.MaintenanceStartTimeEnd);
        }

        if (queryDto?.MaintenanceEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceEndTime >= queryDto.MaintenanceEndTimeStart);
        }

        if (queryDto?.MaintenanceEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceEndTime <= queryDto.MaintenanceEndTimeEnd);
        }

        if (queryDto?.NextMaintenanceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate >= queryDto.NextMaintenanceDateStart);
        }

        if (queryDto?.NextMaintenanceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate <= queryDto.NextMaintenanceDateEnd);
        }

        if (queryDto?.AcceptedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt >= queryDto.AcceptedAtStart);
        }

        if (queryDto?.AcceptedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt <= queryDto.AcceptedAtEnd);
        }

        if (queryDto?.ArchivedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ArchivedAt >= queryDto.ArchivedAtStart);
        }

        if (queryDto?.ArchivedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ArchivedAt <= queryDto.ArchivedAtEnd);
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
