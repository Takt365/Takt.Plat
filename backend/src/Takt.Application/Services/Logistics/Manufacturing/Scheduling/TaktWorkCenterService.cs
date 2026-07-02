// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Scheduling
// 文件名称：TaktWorkCenterService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心应用服务实现
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
/// 工作中心应用服务
/// </summary>
public class TaktWorkCenterService : TaktServiceBase, ITaktWorkCenterService
{
    private readonly ITaktCompanyRepository<TaktWorkCenter> _workCenterRepository;
    private readonly ITaktCompanyRepository<TaktWorkCenterResource> _workCenterResourceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workCenterRepository">工作中心仓储</param>
    /// <param name="workCenterResourceRepository">WorkCenterResource仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktWorkCenterService(
        ITaktCompanyRepository<TaktWorkCenter> workCenterRepository,
        ITaktCompanyRepository<TaktWorkCenterResource> workCenterResourceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workCenterRepository = workCenterRepository;
        _workCenterResourceRepository = workCenterResourceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工作中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktWorkCenterDto>> GetWorkCenterListAsync(TaktWorkCenterQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _workCenterRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktWorkCenterDto>.Create(
            data.Adapt<List<TaktWorkCenterDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto?> GetWorkCenterByIdAsync(long id)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktWorkCenterDto>();
        await FillWorkCenterDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工作中心选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetWorkCenterOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _workCenterRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WorkCenterStatus == 1,
            x => x.WorkCenterName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkCenterCode,
            DictLabel = e.WorkCenterName ?? e.WorkCenterCode,
            ExtValue = e.PlantCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工作中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> CreateWorkCenterAsync(TaktWorkCenterCreateDto dto)
    {
        var entity = dto.Adapt<TaktWorkCenter>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique)
        {
            throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
        }
        entity = await _workCenterRepository.CreateAsync(entity);
                await SaveWorkCenterChildrenAsync(entity, dto);
        return await GetWorkCenterByIdAsync(entity.Id) ?? entity.Adapt<TaktWorkCenterDto>();
    }

    /// <summary>
    /// 更新工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> UpdateWorkCenterAsync(long id, TaktWorkCenterUpdateDto dto)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WorkCenterCode == entity.WorkCenterCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique)
        {
            throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
        }
        await _workCenterRepository.UpdateAsync(entity);
                await SaveWorkCenterChildrenAsync(entity, dto);
        return await GetWorkCenterByIdAsync(id) ?? throw new TaktBusinessException("工作中心不存在");
    }

    /// <summary>
    /// 删除工作中心
    /// </summary>
    /// <param name="id">工作中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterByIdAsync(long id)
    {
        var entity = await _workCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在或已删除");
        }
        await _workCenterResourceRepository.DeleteAsync(x => x.WorkCenterId == entity.Id);
        var deleted = await _workCenterRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工作中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工作中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteWorkCenterByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工作中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterDto> UpdateWorkCenterStatusAsync(TaktWorkCenterStatusDto dto)
    {
        var entity = await _workCenterRepository.GetByIdAsync(dto.WorkCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心不存在");
        }
        entity.WorkCenterStatus = dto.WorkCenterStatus;
        await _workCenterRepository.UpdateAsync(entity);
        return await GetWorkCenterByIdAsync(dto.WorkCenterId) ?? throw new TaktBusinessException("工作中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetWorkCenterTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktWorkCenterTemplateDto>(
            sheetName ?? "工作中心导入模板",
            fileName ?? "工作中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入工作中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportWorkCenterAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktWorkCenterImportDto>(fileStream, sheetName ?? "工作中心导入模板");
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
                var entity = rows[i].Adapt<TaktWorkCenter>();
                var importKey = $"{entity.PlantCode}|{entity.WorkCenterCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WorkCenterCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique = await _uniqueValidator.IsUniqueAsync(
                    _workCenterRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WorkCenterCode == entity.WorkCenterCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_work_center_unique)
                {
                    throw new TaktBusinessException("工作中心的PlantCode、WorkCenterCode已存在");
                }
                await _workCenterRepository.CreateAsync(entity);
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
    /// 导出工作中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportWorkCenterAsync(TaktWorkCenterQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktWorkCenterQueryDto());
        var list = await _workCenterRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWorkCenterExportDto>(),
                sheetName ?? "工作中心数据",
                fileName ?? "工作中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktWorkCenterExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工作中心数据",
            fileName ?? "工作中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工作中心详情（加载 OneToMany 子表：工作中心资源）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillWorkCenterDetailsAsync(TaktWorkCenterDto dto, TaktWorkCenter entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工作中心资源 → dto.Resources
        var resources = await _workCenterResourceRepository.GetListAsync(x => x.WorkCenterId == entity.Id);
        dto.Resources = resources.Adapt<List<TaktWorkCenterResourceDto>>();
    }

    /// <summary>
    /// 保存工作中心子表级联（工作中心资源；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveWorkCenterChildrenAsync(TaktWorkCenter entity, TaktWorkCenterCreateDto dto)
    {
        // 工作中心资源（Resources）
        if (dto.Resources is not { Count: > 0 })
        {
            await _workCenterResourceRepository.DeleteAsync(x => x.WorkCenterId == entity.Id);
        }
        else
        {
            var resources = dto.Resources.Adapt<List<TaktWorkCenterResource>>();
            foreach (var child in resources)
            {
                child.WorkCenterId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < resources.Count; i++)
                        {
                            var key = $"{resources[i].CompanyCode}|{resources[i].WorkCenterId}|{resources[i].ResourceCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"工作中心资源第{i + 1}项与本次提交的其他项重复（CompanyCode、WorkCenterId、ResourceCode）");
                            }
                        }
            await _workCenterResourceRepository.DeleteAsync(x => x.WorkCenterId == entity.Id);
            foreach (var child in resources)
            {
            var isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique = await _uniqueValidator.IsUniqueAsync(
                _workCenterResourceRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.WorkCenterId == child.WorkCenterId
                    && x.ResourceCode == child.ResourceCode);
            if (!isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique)
            {
                throw new TaktBusinessException("工作中心资源的CompanyCode、WorkCenterId、ResourceCode已存在");
            }
            }
            await _workCenterResourceRepository.CreateRangeAsync(resources);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工作中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktWorkCenter, bool>> QueryExpression(TaktWorkCenterQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktWorkCenter>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.WorkCenterName != null && x.WorkCenterName.Contains(keywords))
                || (x.WorkshopCode != null && x.WorkshopCode.Contains(keywords))
                || SqlFunc.ToString(x.DefaultShiftId).Contains(keywords)
                || SqlFunc.ToString(x.WorkCenterStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterName))
        {
            exp = exp.And(x => x.WorkCenterName != null && x.WorkCenterName.Contains(queryDto.WorkCenterName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkshopCode))
        {
            exp = exp.And(x => x.WorkshopCode != null && x.WorkshopCode.Contains(queryDto.WorkshopCode));
        }

        if (queryDto?.DefaultShiftId.HasValue == true)
        {
            exp = exp.And(x => x.DefaultShiftId == queryDto.DefaultShiftId);
        }

        if (queryDto?.WorkCenterStatus.HasValue == true)
        {
            exp = exp.And(x => x.WorkCenterStatus == queryDto.WorkCenterStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
