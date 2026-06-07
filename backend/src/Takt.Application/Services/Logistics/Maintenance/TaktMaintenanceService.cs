// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护记录应用服务实现
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
/// 设备维护记录应用服务
/// </summary>
public class TaktMaintenanceService : TaktServiceBase, ITaktMaintenanceService
{
    private readonly ITaktCompanyRepository<TaktMaintenance> _maintenanceRepository;
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceRepository">设备维护记录仓储</param>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceService(
        ITaktCompanyRepository<TaktMaintenance> maintenanceRepository,
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceRepository = maintenanceRepository;
        _equipmentRepository = equipmentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设备维护记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceDto>> GetMaintenanceListAsync(TaktMaintenanceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceDto>.Create(
            data.Adapt<List<TaktMaintenanceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceDto?> GetMaintenanceByIdAsync(long id)
    {
        var entity = await _maintenanceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceDto>();
    }

    /// <summary>
    /// 获取设备维护记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EquipmentCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EquipmentCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设备维护记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceDto> CreateMaintenanceAsync(TaktMaintenanceCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenance>();
        await StampMaintenanceEquipmentAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_equipment_date_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceRepository,
            x => x.EquipmentId == entity.EquipmentId
                && x.MaintenanceDate == entity.MaintenanceDate);
        if (!isUnique_ix_takt_logistics_maintenance_equipment_date_unique)
        {
            throw new TaktBusinessException("设备维护记录的EquipmentId、MaintenanceDate已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _maintenanceRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EquipmentId == entity.EquipmentId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.EquipmentCode) ? entity.EquipmentCode : entity.EquipmentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _maintenanceRepository.CreateAsync(entity);
        return await GetMaintenanceByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceDto>();
    }

    /// <summary>
    /// 更新设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceDto> UpdateMaintenanceAsync(long id, TaktMaintenanceUpdateDto dto)
    {
        var entity = await _maintenanceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护记录不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceEquipmentAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_equipment_date_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceRepository,
            x => x.EquipmentId == entity.EquipmentId
                && x.MaintenanceDate == entity.MaintenanceDate,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_equipment_date_unique)
        {
            throw new TaktBusinessException("设备维护记录的EquipmentId、MaintenanceDate已存在");
        }
        await _maintenanceRepository.UpdateAsync(entity);
        return await GetMaintenanceByIdAsync(id) ?? throw new TaktBusinessException("设备维护记录不存在");
    }

    /// <summary>
    /// 删除设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceByIdAsync(long id)
    {
        var deleted = await _maintenanceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设备维护记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设备维护记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设备维护记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceDto> UpdateMaintenanceStatusAsync(TaktMaintenanceStatusDto dto)
    {
        var entity = await _maintenanceRepository.GetByIdAsync(dto.MaintenanceId);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护记录不存在");
        }
        entity.MaintenanceStatus = dto.MaintenanceStatus;
        await _maintenanceRepository.UpdateAsync(entity);
        return await GetMaintenanceByIdAsync(dto.MaintenanceId) ?? throw new TaktBusinessException("设备维护记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceTemplateDto>(
            sheetName ?? "设备维护记录导入模板",
            fileName ?? "设备维护记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入设备维护记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceImportDto>(fileStream, sheetName ?? "设备维护记录导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenance>();
                var importDto = rows[i].Adapt<TaktMaintenanceCreateDto>();
                await StampMaintenanceEquipmentAsync(entity, importDto);
                var importKey = $"{entity.EquipmentId}|{entity.MaintenanceDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EquipmentId、MaintenanceDate）");
                }
                var isUnique_ix_takt_logistics_maintenance_equipment_date_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceRepository,
                    x => x.EquipmentId == entity.EquipmentId
                        && x.MaintenanceDate == entity.MaintenanceDate);
                if (!isUnique_ix_takt_logistics_maintenance_equipment_date_unique)
                {
                    throw new TaktBusinessException("设备维护记录的EquipmentId、MaintenanceDate已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _maintenanceRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EquipmentId == entity.EquipmentId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.EquipmentCode) ? entity.EquipmentCode : entity.EquipmentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _maintenanceRepository.CreateAsync(entity);
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
    /// 导出设备维护记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceAsync(TaktMaintenanceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaintenanceQueryDto());
        var list = await _maintenanceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceExportDto>(),
                sheetName ?? "设备维护记录数据",
                fileName ?? "设备维护记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设备维护记录数据",
            fileName ?? "设备维护记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设备维护记录主表外键（ManyToOne → 工厂设备）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceEquipmentAsync(TaktMaintenance entity, TaktMaintenanceCreateDto dto)
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
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设备维护记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenance, bool>> QueryExpression(TaktMaintenanceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenance>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EquipmentId).Contains(keywords)
                || (x.EquipmentCode != null && x.EquipmentCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceType).Contains(keywords)
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
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.MaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceStartTime).Contains(keywords)
                || SqlFunc.ToString(x.MaintenanceEndTime).Contains(keywords)
                || SqlFunc.ToString(x.NextMaintenanceDate).Contains(keywords)
                || SqlFunc.ToString(x.AcceptedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EquipmentId.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentId == queryDto.EquipmentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentCode))
        {
            exp = exp.And(x => x.EquipmentCode != null && x.EquipmentCode.Contains(queryDto.EquipmentCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.MaintenanceType.HasValue == true)
        {
            exp = exp.And(x => x.MaintenanceType == queryDto.MaintenanceType);
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

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
