// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程主应用服务实现
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
/// APS排程主应用服务
/// </summary>
public class TaktApsScheduleService : TaktServiceBase, ITaktApsScheduleService
{
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktCompanyRepository<TaktApsScheduleItem> _apsScheduleItemRepository;
    private readonly ITaktCompanyRepository<TaktApsScheduleChangeLog> _apsScheduleChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleRepository">APS排程主仓储</param>
    /// <param name="apsScheduleItemRepository">ApsScheduleItem仓储</param>
    /// <param name="apsScheduleChangeLogRepository">ApsScheduleChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsScheduleService(
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktCompanyRepository<TaktApsScheduleItem> apsScheduleItemRepository,
        ITaktCompanyRepository<TaktApsScheduleChangeLog> apsScheduleChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsScheduleRepository = apsScheduleRepository;
        _apsScheduleItemRepository = apsScheduleItemRepository;
        _apsScheduleChangeLogRepository = apsScheduleChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsScheduleDto>> GetApsScheduleListAsync(TaktApsScheduleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsScheduleDto>.Create(
            data.Adapt<List<TaktApsScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto?> GetApsScheduleByIdAsync(long id)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktApsScheduleDto>();
        await FillApsScheduleDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取APS排程主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsScheduleOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _apsScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ScheduleName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ScheduleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> CreateApsScheduleAsync(TaktApsScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsSchedule>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ScheduleCode == entity.ScheduleCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique)
        {
            throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
        }
        entity = await _apsScheduleRepository.CreateAsync(entity);
                await SaveApsScheduleChildrenAsync(entity, dto);
        return await GetApsScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktApsScheduleDto>();
    }

    /// <summary>
    /// 更新APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> UpdateApsScheduleAsync(long id, TaktApsScheduleUpdateDto dto)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ScheduleCode == entity.ScheduleCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique)
        {
            throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
        }
        await _apsScheduleRepository.UpdateAsync(entity);
                await SaveApsScheduleChildrenAsync(entity, dto);
        return await GetApsScheduleByIdAsync(id) ?? throw new TaktBusinessException("APS排程主不存在");
    }

    /// <summary>
    /// 删除APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleByIdAsync(long id)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在或已删除");
        }
        await _apsScheduleItemRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        await _apsScheduleChangeLogRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        var deleted = await _apsScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> UpdateApsScheduleStatusAsync(TaktApsScheduleStatusDto dto)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        entity.ScheduleStatus = dto.ScheduleStatus;
        await _apsScheduleRepository.UpdateAsync(entity);
        return await GetApsScheduleByIdAsync(dto.ApsScheduleId) ?? throw new TaktBusinessException("APS排程主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsScheduleTemplateDto>(
            sheetName ?? "APS排程主导入模板",
            fileName ?? "APS排程主导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsScheduleImportDto>(fileStream, sheetName ?? "APS排程主导入模板");
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
                var entity = rows[i].Adapt<TaktApsSchedule>();
                var importKey = $"{entity.PlantCode}|{entity.ScheduleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ScheduleCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsScheduleRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ScheduleCode == entity.ScheduleCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_plant_code_unique)
                {
                    throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
                }
                await _apsScheduleRepository.CreateAsync(entity);
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
    /// 导出APS排程主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsScheduleAsync(TaktApsScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktApsScheduleQueryDto());
        var list = await _apsScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleExportDto>(),
                sheetName ?? "APS排程主数据",
                fileName ?? "APS排程主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程主数据",
            fileName ?? "APS排程主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充APS排程主详情（加载 OneToMany 子表：APS排程明细、APS排程变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillApsScheduleDetailsAsync(TaktApsScheduleDto dto, TaktApsSchedule entity)
    {
        if (dto == null)
        {
            return;
        }
        // APS排程明细 → dto.Items
        var items = await _apsScheduleItemRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
        dto.Items = items.Adapt<List<TaktApsScheduleItemDto>>();
        // APS排程变更日志 → dto.ChangeLogs
        var changelogs = await _apsScheduleChangeLogRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktApsScheduleChangeLogDto>>();
    }

    /// <summary>
    /// 保存APS排程主子表级联（APS排程明细、APS排程变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveApsScheduleChildrenAsync(TaktApsSchedule entity, TaktApsScheduleCreateDto dto)
    {
        // APS排程明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _apsScheduleItemRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktApsScheduleItem>>();
            foreach (var child in items)
            {
                child.ApsScheduleId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.ScheduleCode) ? entity.ScheduleCode : entity.Id.ToString();
                var maxLine = await _apsScheduleItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsScheduleId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (string.IsNullOrWhiteSpace(child.ApsScheduleCode))
                    {
                        child.ApsScheduleCode = !string.IsNullOrWhiteSpace(entity.ScheduleCode) ? entity.ScheduleCode : entity.Id.ToString();
                    }
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].ApsScheduleId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"APS排程明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ApsScheduleId、LineNumber）");
                            }
                        }
            await _apsScheduleItemRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _apsScheduleItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ApsScheduleId == child.ApsScheduleId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique)
            {
                throw new TaktBusinessException("APS排程明细的CompanyCode、ApsScheduleId、LineNumber已存在");
            }
            }
            await _apsScheduleItemRepository.CreateRangeAsync(items);
        }
        // APS排程变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _apsScheduleChangeLogRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktApsScheduleChangeLog>>();
            foreach (var child in changelogs)
            {
                child.ApsScheduleId = entity.Id;
            }
            await _apsScheduleChangeLogRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _apsScheduleChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsSchedule, bool>> QueryExpression(TaktApsScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsSchedule>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ScheduleCode != null && x.ScheduleCode.Contains(keywords))
                || (x.ScheduleName != null && x.ScheduleName.Contains(keywords))
                || SqlFunc.ToString(x.ScheduleType).Contains(keywords)
                || SqlFunc.ToString(x.PlanCycle).Contains(keywords)
                || (x.WorkshopCode != null && x.WorkshopCode.Contains(keywords))
                || (x.WorkshopName != null && x.WorkshopName.Contains(keywords))
                || (x.ProductionLineCode != null && x.ProductionLineCode.Contains(keywords))
                || (x.ProductionLineName != null && x.ProductionLineName.Contains(keywords))
                || SqlFunc.ToString(x.ScheduleStrategy).Contains(keywords)
                || SqlFunc.ToString(x.ScheduleAlgorithm).Contains(keywords)
                || SqlFunc.ToString(x.OptimizationObjective).Contains(keywords)
                || SqlFunc.ToString(x.ScheduleStatus).Contains(keywords)
                || SqlFunc.ToString(x.PlannerId).Contains(keywords)
                || (x.PlannerName != null && x.PlannerName.Contains(keywords))
                || SqlFunc.ToString(x.PublishUserId).Contains(keywords)
                || (x.PublishUserName != null && x.PublishUserName.Contains(keywords))
                || (x.ScheduleDescription != null && x.ScheduleDescription.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanDate).Contains(keywords)
                || SqlFunc.ToString(x.PlanStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlanEndTime).Contains(keywords)
                || SqlFunc.ToString(x.PublishTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScheduleCode))
        {
            exp = exp.And(x => x.ScheduleCode != null && x.ScheduleCode.Contains(queryDto.ScheduleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScheduleName))
        {
            exp = exp.And(x => x.ScheduleName != null && x.ScheduleName.Contains(queryDto.ScheduleName));
        }

        if (queryDto?.ScheduleType.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleType == queryDto.ScheduleType);
        }

        if (queryDto?.PlanCycle.HasValue == true)
        {
            exp = exp.And(x => x.PlanCycle == queryDto.PlanCycle);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkshopCode))
        {
            exp = exp.And(x => x.WorkshopCode != null && x.WorkshopCode.Contains(queryDto.WorkshopCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkshopName))
        {
            exp = exp.And(x => x.WorkshopName != null && x.WorkshopName.Contains(queryDto.WorkshopName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLineCode))
        {
            exp = exp.And(x => x.ProductionLineCode != null && x.ProductionLineCode.Contains(queryDto.ProductionLineCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLineName))
        {
            exp = exp.And(x => x.ProductionLineName != null && x.ProductionLineName.Contains(queryDto.ProductionLineName));
        }

        if (queryDto?.ScheduleStrategy.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleStrategy == queryDto.ScheduleStrategy);
        }

        if (queryDto?.ScheduleAlgorithm.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleAlgorithm == queryDto.ScheduleAlgorithm);
        }

        if (queryDto?.OptimizationObjective.HasValue == true)
        {
            exp = exp.And(x => x.OptimizationObjective == queryDto.OptimizationObjective);
        }

        if (queryDto?.ScheduleStatus.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleStatus == queryDto.ScheduleStatus);
        }

        if (queryDto?.PlannerId.HasValue == true)
        {
            exp = exp.And(x => x.PlannerId == queryDto.PlannerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlannerName))
        {
            exp = exp.And(x => x.PlannerName != null && x.PlannerName.Contains(queryDto.PlannerName));
        }

        if (queryDto?.PublishUserId.HasValue == true)
        {
            exp = exp.And(x => x.PublishUserId == queryDto.PublishUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PublishUserName))
        {
            exp = exp.And(x => x.PublishUserName != null && x.PublishUserName.Contains(queryDto.PublishUserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScheduleDescription))
        {
            exp = exp.And(x => x.ScheduleDescription != null && x.ScheduleDescription.Contains(queryDto.ScheduleDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate >= queryDto.PlanDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate <= queryDto.PlanDateEnd);
        }

        if (queryDto?.PlanStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartTime >= queryDto.PlanStartTimeStart);
        }

        if (queryDto?.PlanStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartTime <= queryDto.PlanStartTimeEnd);
        }

        if (queryDto?.PlanEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndTime >= queryDto.PlanEndTimeStart);
        }

        if (queryDto?.PlanEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndTime <= queryDto.PlanEndTimeEnd);
        }

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime >= queryDto.PublishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime <= queryDto.PublishTimeEnd);
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
