// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Planning
// 文件名称：TaktMasterDemandScheduleService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划MDS头应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;
using Takt.Domain.Entities.Logistics.Manufacturing.Planning;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Planning;

/// <summary>
/// 主需求计划MDS头应用服务
/// </summary>
public class TaktMasterDemandScheduleService : TaktServiceBase, ITaktMasterDemandScheduleService
{
    private readonly ITaktApprovalRepository<TaktMasterDemandSchedule> _masterDemandScheduleRepository;
    private readonly ITaktCompanyRepository<TaktMasterDemandScheduleLine> _masterDemandScheduleLineRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterDemandScheduleRepository">主需求计划MDS头仓储</param>
    /// <param name="masterDemandScheduleLineRepository">MasterDemandScheduleLine仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterDemandScheduleService(
        ITaktApprovalRepository<TaktMasterDemandSchedule> masterDemandScheduleRepository,
        ITaktCompanyRepository<TaktMasterDemandScheduleLine> masterDemandScheduleLineRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterDemandScheduleRepository = masterDemandScheduleRepository;
        _masterDemandScheduleLineRepository = masterDemandScheduleLineRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取主需求计划MDS头列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterDemandScheduleDto>> GetMasterDemandScheduleListAsync(TaktMasterDemandScheduleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterDemandScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterDemandScheduleDto>.Create(
            data.Adapt<List<TaktMasterDemandScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主需求计划MDS头
    /// </summary>
    /// <param name="id">主需求计划MDS头ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleDto?> GetMasterDemandScheduleByIdAsync(long id)
    {
        var entity = await _masterDemandScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMasterDemandScheduleDto>();
        await FillMasterDemandScheduleDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取主需求计划MDS头选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterDemandScheduleOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _masterDemandScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ScheduleStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建主需求计划MDS头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleDto> CreateMasterDemandScheduleAsync(TaktMasterDemandScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterDemandSchedule>();
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MdsCode == entity.MdsCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_unique)
        {
            throw new TaktBusinessException("主需求计划MDS头的PlantCode、MdsCode已存在");
        }
        entity = await _masterDemandScheduleRepository.CreateAsync(entity);
                await SaveMasterDemandScheduleChildrenAsync(entity, dto);
        return await GetMasterDemandScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterDemandScheduleDto>();
    }

    /// <summary>
    /// 更新主需求计划MDS头
    /// </summary>
    /// <param name="id">主需求计划MDS头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleDto> UpdateMasterDemandScheduleAsync(long id, TaktMasterDemandScheduleUpdateDto dto)
    {
        var entity = await _masterDemandScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS头不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MdsCode == entity.MdsCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_unique)
        {
            throw new TaktBusinessException("主需求计划MDS头的PlantCode、MdsCode已存在");
        }
        await _masterDemandScheduleRepository.UpdateAsync(entity);
                await SaveMasterDemandScheduleChildrenAsync(entity, dto);
        return await GetMasterDemandScheduleByIdAsync(id) ?? throw new TaktBusinessException("主需求计划MDS头不存在");
    }

    /// <summary>
    /// 删除主需求计划MDS头
    /// </summary>
    /// <param name="id">主需求计划MDS头ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleByIdAsync(long id)
    {
        var entity = await _masterDemandScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS头不存在或已删除");
        }
        await _masterDemandScheduleLineRepository.DeleteAsync(x => x.MasterDemandScheduleId == entity.Id);
        var deleted = await _masterDemandScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("主需求计划MDS头不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除主需求计划MDS头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterDemandScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新主需求计划MDS头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleDto> UpdateMasterDemandScheduleStatusAsync(TaktMasterDemandScheduleStatusDto dto)
    {
        var entity = await _masterDemandScheduleRepository.GetByIdAsync(dto.MasterDemandScheduleId);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS头不存在");
        }
        entity.ScheduleStatus = dto.ScheduleStatus;
        await _masterDemandScheduleRepository.UpdateAsync(entity);
        return await GetMasterDemandScheduleByIdAsync(dto.MasterDemandScheduleId) ?? throw new TaktBusinessException("主需求计划MDS头不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterDemandScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterDemandScheduleTemplateDto>(
            sheetName ?? "主需求计划MDS头导入模板",
            fileName ?? "主需求计划MDS头导入模板.xlsx");
    }

    /// <summary>
    /// 导入主需求计划MDS头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterDemandScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterDemandScheduleImportDto>(fileStream, sheetName ?? "主需求计划MDS头导入模板");
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
                var entity = rows[i].Adapt<TaktMasterDemandSchedule>();
                var importKey = $"{entity.PlantCode}|{entity.MdsCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MdsCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_mds_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterDemandScheduleRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MdsCode == entity.MdsCode);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_unique)
                {
                    throw new TaktBusinessException("主需求计划MDS头的PlantCode、MdsCode已存在");
                }
                await _masterDemandScheduleRepository.CreateAsync(entity);
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
    /// 导出主需求计划MDS头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterDemandScheduleAsync(TaktMasterDemandScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMasterDemandScheduleQueryDto());
        var list = await _masterDemandScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterDemandScheduleExportDto>(),
                sheetName ?? "主需求计划MDS头数据",
                fileName ?? "主需求计划MDS头导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterDemandScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主需求计划MDS头数据",
            fileName ?? "主需求计划MDS头导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充主需求计划MDS头详情（加载 OneToMany 子表：主需求计划MDS行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMasterDemandScheduleDetailsAsync(TaktMasterDemandScheduleDto dto, TaktMasterDemandSchedule entity)
    {
        if (dto == null)
        {
            return;
        }
        // 主需求计划MDS行 → dto.Lines
        var lines = await _masterDemandScheduleLineRepository.GetListAsync(x => x.MasterDemandScheduleId == entity.Id);
        dto.Lines = lines.Adapt<List<TaktMasterDemandScheduleLineDto>>();
    }

    /// <summary>
    /// 保存主需求计划MDS头子表级联（主需求计划MDS行；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMasterDemandScheduleChildrenAsync(TaktMasterDemandSchedule entity, TaktMasterDemandScheduleCreateDto dto)
    {
        // 主需求计划MDS行（Lines）
        if (dto.Lines is not { Count: > 0 })
        {
            await _masterDemandScheduleLineRepository.DeleteAsync(x => x.MasterDemandScheduleId == entity.Id);
        }
        else
        {
            var lines = dto.Lines.Adapt<List<TaktMasterDemandScheduleLine>>();
            foreach (var child in lines)
            {
                child.MasterDemandScheduleId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < lines.Count; i++)
                        {
                            var key = $"{lines[i].CompanyCode}|{lines[i].MasterDemandScheduleId}|{lines[i].MaterialCode}|{lines[i].BucketStart}|{lines[i].DemandSourceType}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"主需求计划MDS行第{i + 1}项与本次提交的其他项重复（CompanyCode、MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType）");
                            }
                        }
            await _masterDemandScheduleLineRepository.DeleteAsync(x => x.MasterDemandScheduleId == entity.Id);
            foreach (var child in lines)
            {
            var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
                _masterDemandScheduleLineRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.MasterDemandScheduleId == child.MasterDemandScheduleId
                    && x.MaterialCode == child.MaterialCode
                    && x.BucketStart == child.BucketStart
                    && x.DemandSourceType == child.DemandSourceType);
            if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique)
            {
                throw new TaktBusinessException("主需求计划MDS行的CompanyCode、MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
            }
            }
            await _masterDemandScheduleLineRepository.CreateRangeAsync(lines);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建主需求计划MDS头查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterDemandSchedule, bool>> QueryExpression(TaktMasterDemandScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterDemandSchedule>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MdsCode != null && x.MdsCode.Contains(keywords))
                || SqlFunc.ToString(x.BucketType).Contains(keywords)
                || SqlFunc.ToString(x.ScheduleStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanPeriodStart).Contains(keywords)
                || SqlFunc.ToString(x.PlanPeriodEnd).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MdsCode))
        {
            exp = exp.And(x => x.MdsCode != null && x.MdsCode.Contains(queryDto.MdsCode));
        }

        if (queryDto?.BucketType.HasValue == true)
        {
            exp = exp.And(x => x.BucketType == queryDto.BucketType);
        }

        if (queryDto?.ScheduleStatus.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleStatus == queryDto.ScheduleStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlanPeriodStartStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodStart >= queryDto.PlanPeriodStartStart);
        }

        if (queryDto?.PlanPeriodStartEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodStart <= queryDto.PlanPeriodStartEnd);
        }

        if (queryDto?.PlanPeriodEndStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodEnd >= queryDto.PlanPeriodEndStart);
        }

        if (queryDto?.PlanPeriodEndEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodEnd <= queryDto.PlanPeriodEndEnd);
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
