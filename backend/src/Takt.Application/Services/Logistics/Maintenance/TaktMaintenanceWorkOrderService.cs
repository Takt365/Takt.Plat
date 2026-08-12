// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单应用服务实现
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
/// 维护工单应用服务
/// </summary>
public class TaktMaintenanceWorkOrderService : TaktServiceBase, ITaktMaintenanceWorkOrderService
{
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> _maintenanceWorkOrderMaterialRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> _maintenanceWorkOrderLaborRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="maintenanceWorkOrderMaterialRepository">MaintenanceWorkOrderMaterial仓储</param>
    /// <param name="maintenanceWorkOrderLaborRepository">MaintenanceWorkOrderLabor仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceWorkOrderService(
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> maintenanceWorkOrderMaterialRepository,
        ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> maintenanceWorkOrderLaborRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _maintenanceWorkOrderMaterialRepository = maintenanceWorkOrderMaterialRepository;
        _maintenanceWorkOrderLaborRepository = maintenanceWorkOrderLaborRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取维护工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceWorkOrderDto>> GetMaintenanceWorkOrderListAsync(TaktMaintenanceWorkOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceWorkOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceWorkOrderDto>.Create(
            data.Adapt<List<TaktMaintenanceWorkOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderDto?> GetMaintenanceWorkOrderByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaintenanceWorkOrderDto>();
        await FillMaintenanceWorkOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取维护工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceWorkOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceWorkOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WorkOrderStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EquipmentName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建维护工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderDto> CreateMaintenanceWorkOrderAsync(TaktMaintenanceWorkOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceWorkOrder>();
        var isUnique_ix_takt_logistics_maintenance_work_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkOrderCode == entity.WorkOrderCode);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_code_unique)
        {
            throw new TaktBusinessException("维护工单的PlantCode、WorkOrderCode已存在");
        }
        entity = await _maintenanceWorkOrderRepository.CreateAsync(entity);
                await SaveMaintenanceWorkOrderChildrenAsync(entity, dto);
        return await GetMaintenanceWorkOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceWorkOrderDto>();
    }

    /// <summary>
    /// 更新维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderDto> UpdateMaintenanceWorkOrderAsync(long id, TaktMaintenanceWorkOrderUpdateDto dto)
    {
        var entity = await _maintenanceWorkOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_maintenance_work_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkOrderCode == entity.WorkOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_code_unique)
        {
            throw new TaktBusinessException("维护工单的PlantCode、WorkOrderCode已存在");
        }
        await _maintenanceWorkOrderRepository.UpdateAsync(entity);
                await SaveMaintenanceWorkOrderChildrenAsync(entity, dto);
        return await GetMaintenanceWorkOrderByIdAsync(id) ?? throw new TaktBusinessException("维护工单不存在");
    }

    /// <summary>
    /// 删除维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单不存在或已删除");
        }
        await _maintenanceWorkOrderMaterialRepository.DeleteAsync(x => x.MaintenanceWorkOrderId == entity.Id);
        await _maintenanceWorkOrderLaborRepository.DeleteAsync(x => x.MaintenanceWorkOrderId == entity.Id);
        var deleted = await _maintenanceWorkOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("维护工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除维护工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceWorkOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新维护工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderDto> UpdateMaintenanceWorkOrderStatusAsync(TaktMaintenanceWorkOrderStatusDto dto)
    {
        var entity = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单不存在");
        }
        entity.WorkOrderStatus = dto.WorkOrderStatus;
        await _maintenanceWorkOrderRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderByIdAsync(dto.MaintenanceWorkOrderId) ?? throw new TaktBusinessException("维护工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceWorkOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceWorkOrderTemplateDto>(
            sheetName ?? "维护工单导入模板",
            fileName ?? "维护工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入维护工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceWorkOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceWorkOrderImportDto>(fileStream, sheetName ?? "维护工单导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceWorkOrder>();
                var importKey = $"{entity.PlantCode}|{entity.WorkOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WorkOrderCode）");
                }
                var isUnique_ix_takt_logistics_maintenance_work_order_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceWorkOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WorkOrderCode == entity.WorkOrderCode);
                if (!isUnique_ix_takt_logistics_maintenance_work_order_code_unique)
                {
                    throw new TaktBusinessException("维护工单的PlantCode、WorkOrderCode已存在");
                }
                await _maintenanceWorkOrderRepository.CreateAsync(entity);
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
    /// 导出维护工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceWorkOrderAsync(TaktMaintenanceWorkOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaintenanceWorkOrderQueryDto());
        var list = await _maintenanceWorkOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceWorkOrderExportDto>(),
                sheetName ?? "维护工单数据",
                fileName ?? "维护工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceWorkOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "维护工单数据",
            fileName ?? "维护工单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废维护工单领料标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="maintenanceWorkOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMaintenanceWorkOrderMaterialsObsoleteAsync(long maintenanceWorkOrderId)
    {
        if (maintenanceWorkOrderId <= 0)
        {
            return;
        }
        var rows = await _maintenanceWorkOrderMaterialRepository.GetListAsync(
            x => x.MaintenanceWorkOrderId == maintenanceWorkOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _maintenanceWorkOrderMaterialRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废维护工单报工标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="maintenanceWorkOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMaintenanceWorkOrderLaborsObsoleteAsync(long maintenanceWorkOrderId)
    {
        if (maintenanceWorkOrderId <= 0)
        {
            return;
        }
        var rows = await _maintenanceWorkOrderLaborRepository.GetListAsync(
            x => x.MaintenanceWorkOrderId == maintenanceWorkOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _maintenanceWorkOrderLaborRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充维护工单详情（加载 OneToMany 子表：维护工单领料、维护工单报工）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaintenanceWorkOrderDetailsAsync(TaktMaintenanceWorkOrderDto dto, TaktMaintenanceWorkOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 维护工单领料 → dto.Materials（含作废行）
        var materials = await _maintenanceWorkOrderMaterialRepository.GetListAsync(x => x.MaintenanceWorkOrderId == entity.Id);
        dto.Materials = materials.Adapt<List<TaktMaintenanceWorkOrderMaterialDto>>();
        // 维护工单报工 → dto.Labors（含作废行）
        var labors = await _maintenanceWorkOrderLaborRepository.GetListAsync(x => x.MaintenanceWorkOrderId == entity.Id);
        dto.Labors = labors.Adapt<List<TaktMaintenanceWorkOrderLaborDto>>();
    }

    /// <summary>
    /// 保存维护工单子表级联（维护工单领料、维护工单报工；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaintenanceWorkOrderChildrenAsync(TaktMaintenanceWorkOrder entity, TaktMaintenanceWorkOrderCreateDto dto)
    {
        var workOrderUpdateDto = dto as TaktMaintenanceWorkOrderUpdateDto;
        // 维护工单领料（Materials）
        List<TaktMaintenanceWorkOrderMaterialUpdateDto>? materialsForSave;
        if (workOrderUpdateDto?.Materials != null)
        {
            materialsForSave = workOrderUpdateDto.Materials;
        }
        else if (dto.Materials != null)
        {
            materialsForSave = dto.Materials.Adapt<List<TaktMaintenanceWorkOrderMaterialUpdateDto>>();
        }
        else
        {
            materialsForSave = null;
        }
        if (materialsForSave is not { Count: > 0 })
        {
            await MarkMaintenanceWorkOrderMaterialsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _maintenanceWorkOrderMaterialRepository.GetListAsync(x => x.MaintenanceWorkOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaintenanceWorkOrderMaterial>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < materialsForSave.Count; i++)
            {
                var childDto = materialsForSave[i];
                childDto.MaintenanceWorkOrderId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("维护工单领料第{i + 1}项与本次提交的其他项重复（CompanyCode、MaintenanceWorkOrderId、LineNumber）");
                }
                if (childDto.MaintenanceWorkOrderMaterialId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaintenanceWorkOrderMaterialId, out var target))
                    {
                        throw new TaktBusinessException("维护工单领料不存在（MaintenanceWorkOrderMaterialId={childDto.MaintenanceWorkOrderMaterialId}）");
                    }
                    if (target.MaintenanceWorkOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("维护工单领料不属于当前主表（MaintenanceWorkOrderMaterialId={childDto.MaintenanceWorkOrderMaterialId}）");
                    }
                    submittedIds.Add(childDto.MaintenanceWorkOrderMaterialId);
                    var isUniqueUpdate_ix_takt_logistics_maintenance_work_order_material_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _maintenanceWorkOrderMaterialRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaintenanceWorkOrderId == x.MaintenanceWorkOrderId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.MaintenanceWorkOrderMaterialId);
                    if (!isUniqueUpdate_ix_takt_logistics_maintenance_work_order_material_order_line_unique)
                    {
                        throw new TaktBusinessException("维护工单领料的CompanyCode、MaintenanceWorkOrderId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MaintenanceWorkOrderMaterialId;
                    target.MaintenanceWorkOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _maintenanceWorkOrderMaterialRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_maintenance_work_order_material_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _maintenanceWorkOrderMaterialRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaintenanceWorkOrderId == x.MaintenanceWorkOrderId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_maintenance_work_order_material_order_line_unique)
                    {
                        throw new TaktBusinessException("维护工单领料的CompanyCode、MaintenanceWorkOrderId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktMaintenanceWorkOrderMaterial>();
                    child.Id = 0;
                    child.MaintenanceWorkOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _maintenanceWorkOrderMaterialRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.WorkOrderCode) ? entity.WorkOrderCode : entity.Id.ToString();
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
                await _maintenanceWorkOrderMaterialRepository.CreateRangeAsync(toCreate);
            }
        }
        // 维护工单报工（Labors）
        List<TaktMaintenanceWorkOrderLaborUpdateDto>? laborsForSave;
        if (workOrderUpdateDto?.Labors != null)
        {
            laborsForSave = workOrderUpdateDto.Labors;
        }
        else if (dto.Labors != null)
        {
            laborsForSave = dto.Labors.Adapt<List<TaktMaintenanceWorkOrderLaborUpdateDto>>();
        }
        else
        {
            laborsForSave = null;
        }
        if (laborsForSave is not { Count: > 0 })
        {
            await MarkMaintenanceWorkOrderLaborsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _maintenanceWorkOrderLaborRepository.GetListAsync(x => x.MaintenanceWorkOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaintenanceWorkOrderLabor>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < laborsForSave.Count; i++)
            {
                var childDto = laborsForSave[i];
                childDto.MaintenanceWorkOrderId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("维护工单报工第{i + 1}项与本次提交的其他项重复（CompanyCode、MaintenanceWorkOrderId、LineNumber）");
                }
                if (childDto.MaintenanceWorkOrderLaborId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaintenanceWorkOrderLaborId, out var target))
                    {
                        throw new TaktBusinessException("维护工单报工不存在（MaintenanceWorkOrderLaborId={childDto.MaintenanceWorkOrderLaborId}）");
                    }
                    if (target.MaintenanceWorkOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("维护工单报工不属于当前主表（MaintenanceWorkOrderLaborId={childDto.MaintenanceWorkOrderLaborId}）");
                    }
                    submittedIds.Add(childDto.MaintenanceWorkOrderLaborId);
                    var isUniqueUpdate_ix_takt_logistics_maintenance_work_order_labor_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _maintenanceWorkOrderLaborRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaintenanceWorkOrderId == x.MaintenanceWorkOrderId
                && x.LineNumber == x.LineNumber
                && x.EmployeeCode == x.EmployeeCode,
                        childDto.MaintenanceWorkOrderLaborId);
                    if (!isUniqueUpdate_ix_takt_logistics_maintenance_work_order_labor_order_line_unique)
                    {
                        throw new TaktBusinessException("维护工单报工的CompanyCode、MaintenanceWorkOrderId、LineNumber、EmployeeCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MaintenanceWorkOrderLaborId;
                    target.MaintenanceWorkOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _maintenanceWorkOrderLaborRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_maintenance_work_order_labor_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _maintenanceWorkOrderLaborRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaintenanceWorkOrderId == x.MaintenanceWorkOrderId
                && x.LineNumber == x.LineNumber
                && x.EmployeeCode == x.EmployeeCode);
                    if (!isUniqueCreate_ix_takt_logistics_maintenance_work_order_labor_order_line_unique)
                    {
                        throw new TaktBusinessException("维护工单报工的CompanyCode、MaintenanceWorkOrderId、LineNumber、EmployeeCode已存在");
                    }
                    var child = childDto.Adapt<TaktMaintenanceWorkOrderLabor>();
                    child.Id = 0;
                    child.MaintenanceWorkOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _maintenanceWorkOrderLaborRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.WorkOrderCode) ? entity.WorkOrderCode : entity.Id.ToString();
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
                await _maintenanceWorkOrderLaborRepository.CreateRangeAsync(toCreate);
            }
        }
    }

    /// <summary>
    /// 获取维护工单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护工单统计</returns>
    public async Task<TaktMaintenanceWorkOrderStatDto> GetMaintenanceWorkOrderStatAsync(TaktMaintenanceWorkOrderStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktMaintenanceWorkOrder, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        var monthWorkOrderCount = await _maintenanceWorkOrderRepository.CountAsync(predicate);
        var monthTotalCost = await _maintenanceWorkOrderRepository.SumAsync(x => x.TotalCost, predicate);
        Expression<Func<TaktMaintenanceWorkOrder, bool>> openPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && x.WorkOrderStatus >= 0
            && x.WorkOrderStatus <= 3;
        var monthOpenWorkOrderCount = await _maintenanceWorkOrderRepository.CountAsync(openPredicate);
        return new TaktMaintenanceWorkOrderStatDto
        {
            StatMonth = statMonth,
            MonthWorkOrderCount = monthWorkOrderCount,
            MonthOpenWorkOrderCount = monthOpenWorkOrderCount,
            MonthTotalCost = monthTotalCost,
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建维护工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceWorkOrder, bool>> QueryExpression(TaktMaintenanceWorkOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceWorkOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceNotificationId).Contains(keywords)
                || (x.NotificationCode != null && x.NotificationCode.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentId).Contains(keywords)
                || (x.EquipCode != null && x.EquipCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceCategory).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceType).Contains(keywords)
                || SqlFunc.ToString(x.WorkOrderStatus).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.AssignedTechnician != null && x.AssignedTechnician.Contains(keywords))
                || (x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.MaintenanceContent != null && x.MaintenanceContent.Contains(keywords))
                || (x.Solution != null && x.Solution.Contains(keywords))
                || SqlFunc.ToString(x.CostCenterId).Contains(keywords)
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || SqlFunc.ToString(x.CostElementId).Contains(keywords)
                || (x.CostElementCode != null && x.CostElementCode.Contains(keywords))
                || SqlFunc.ToString(x.TotalMaterialCost).Contains(keywords)
                || SqlFunc.ToString(x.TotalLaborCost).Contains(keywords)
                || SqlFunc.ToString(x.TotalOtherCost).Contains(keywords)
                || SqlFunc.ToString(x.TotalCost).Contains(keywords)
                || SqlFunc.ToString(x.SettlementStatus).Contains(keywords)
                || (x.AcceptedBy != null && x.AcceptedBy.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceResult).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceCycleDays).Contains(keywords)
                || (x.MaintenanceImages != null && x.MaintenanceImages.Contains(keywords))
                || (x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(keywords))
                || (x.AcceptedSummary != null && x.AcceptedSummary.Contains(keywords))
                || SqlFunc.ToString(x.IsHistoryArchived).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualStartTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualEndTime).Contains(keywords)
                || SqlFunc.ToString(x.SettlementTime).Contains(keywords)
                || SqlFunc.ToString(x.CompletedAt).Contains(keywords)
                || SqlFunc.ToString(x.AcceptedAt).Contains(keywords)
                || SqlFunc.ToString(x.NextMaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkOrderCode))
        {
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(queryDto.WorkOrderCode));
        }

        if (queryDto?.MaintenanceNotificationId.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceNotificationId == queryDto.MaintenanceNotificationId);
        }

        if (!string.IsNullOrEmpty(queryDto?.NotificationCode))
        {
            exp = exp.And(x => x.NotificationCode != null && x.NotificationCode.Contains(queryDto.NotificationCode));
        }

        if (queryDto?.EquipmentId.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentId == queryDto.EquipmentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipCode))
        {
            exp = exp.And(x => x.EquipCode != null && x.EquipCode.Contains(queryDto.EquipCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentName))
        {
            exp = exp.And(x => x.EquipmentName != null && x.EquipmentName.Contains(queryDto.EquipmentName));
        }

        if (queryDto?.MaintenanceCategory.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCategory == queryDto.MaintenanceCategory);
        }

        if (queryDto?.MaintenanceType.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceType == queryDto.MaintenanceType);
        }

        if (queryDto?.WorkOrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.WorkOrderStatus == queryDto.WorkOrderStatus);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssignedTechnician))
        {
            exp = exp.And(x => x.AssignedTechnician != null && x.AssignedTechnician.Contains(queryDto.AssignedTechnician));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceCompany))
        {
            exp = exp.And(x => x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(queryDto.MaintenanceCompany));
        }

        if (!string.IsNullOrEmpty(queryDto?.FaultDescription))
        {
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(queryDto.FaultDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceContent))
        {
            exp = exp.And(x => x.MaintenanceContent != null && x.MaintenanceContent.Contains(queryDto.MaintenanceContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.Solution))
        {
            exp = exp.And(x => x.Solution != null && x.Solution.Contains(queryDto.Solution));
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterId == queryDto.CostCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (queryDto?.CostElementId.HasValue == true)
        {
            exp = exp.And(x => x.CostElementId == queryDto.CostElementId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostElementCode))
        {
            exp = exp.And(x => x.CostElementCode != null && x.CostElementCode.Contains(queryDto.CostElementCode));
        }

        if (queryDto?.TotalMaterialCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalMaterialCost == queryDto.TotalMaterialCost);
        }

        if (queryDto?.TotalLaborCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalLaborCost == queryDto.TotalLaborCost);
        }

        if (queryDto?.TotalOtherCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalOtherCost == queryDto.TotalOtherCost);
        }

        if (queryDto?.TotalCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalCost == queryDto.TotalCost);
        }

        if (queryDto?.SettlementStatus.HasValue == true)
        {
            exp = exp.And(x => x.SettlementStatus == queryDto.SettlementStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptedBy))
        {
            exp = exp.And(x => x.AcceptedBy != null && x.AcceptedBy.Contains(queryDto.AcceptedBy));
        }

        if (queryDto?.MaintenanceResult.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceResult == queryDto.MaintenanceResult);
        }

        if (queryDto?.MaintenanceCycleDays.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceCycleDays == queryDto.MaintenanceCycleDays);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceImages))
        {
            exp = exp.And(x => x.MaintenanceImages != null && x.MaintenanceImages.Contains(queryDto.MaintenanceImages));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaintenanceDocuments))
        {
            exp = exp.And(x => x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(queryDto.MaintenanceDocuments));
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptedSummary))
        {
            exp = exp.And(x => x.AcceptedSummary != null && x.AcceptedSummary.Contains(queryDto.AcceptedSummary));
        }

        if (queryDto?.IsHistoryArchived.HasValue == true)
        {
            exp = exp.And(x => x.IsHistoryArchived == queryDto.IsHistoryArchived);
        }


        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
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

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime >= queryDto.ActualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime <= queryDto.ActualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime >= queryDto.ActualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime <= queryDto.ActualEndTimeEnd);
        }

        if (queryDto?.SettlementTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.SettlementTime >= queryDto.SettlementTimeStart);
        }

        if (queryDto?.SettlementTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.SettlementTime <= queryDto.SettlementTimeEnd);
        }

        if (queryDto?.CompletedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CompletedAt >= queryDto.CompletedAtStart);
        }

        if (queryDto?.CompletedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CompletedAt <= queryDto.CompletedAtEnd);
        }

        if (queryDto?.AcceptedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt >= queryDto.AcceptedAtStart);
        }

        if (queryDto?.AcceptedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt <= queryDto.AcceptedAtEnd);
        }

        if (queryDto?.NextMaintenanceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate >= queryDto.NextMaintenanceDateStart);
        }

        if (queryDto?.NextMaintenanceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.NextMaintenanceDate <= queryDto.NextMaintenanceDateEnd);
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
