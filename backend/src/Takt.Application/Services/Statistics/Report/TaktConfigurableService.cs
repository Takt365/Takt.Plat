// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Report
// 文件名称：TaktConfigurableService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Report;
using Takt.Domain.Entities.Statistics.Report;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Statistics.Report;

/// <summary>
/// 自定义报表主应用服务
/// </summary>
public class TaktConfigurableService : TaktServiceBase, ITaktConfigurableService
{
    private readonly ITaktCompanyRepository<TaktConfigurable> _configurableRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableSource> _configurableSourceRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableJoin> _configurableJoinRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableField> _configurableFieldRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableSelection> _configurableSelectionRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableGroupBy> _configurableGroupByRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableOrderBy> _configurableOrderByRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableRepository">自定义报表主仓储</param>
    /// <param name="configurableSourceRepository">ConfigurableSource仓储</param>
    /// <param name="configurableJoinRepository">ConfigurableJoin仓储</param>
    /// <param name="configurableFieldRepository">ConfigurableField仓储</param>
    /// <param name="configurableSelectionRepository">ConfigurableSelection仓储</param>
    /// <param name="configurableGroupByRepository">ConfigurableGroupBy仓储</param>
    /// <param name="configurableOrderByRepository">ConfigurableOrderBy仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConfigurableService(
        ITaktCompanyRepository<TaktConfigurable> configurableRepository,
        ITaktCompanyRepository<TaktConfigurableSource> configurableSourceRepository,
        ITaktCompanyRepository<TaktConfigurableJoin> configurableJoinRepository,
        ITaktCompanyRepository<TaktConfigurableField> configurableFieldRepository,
        ITaktCompanyRepository<TaktConfigurableSelection> configurableSelectionRepository,
        ITaktCompanyRepository<TaktConfigurableGroupBy> configurableGroupByRepository,
        ITaktCompanyRepository<TaktConfigurableOrderBy> configurableOrderByRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _configurableRepository = configurableRepository;
        _configurableSourceRepository = configurableSourceRepository;
        _configurableJoinRepository = configurableJoinRepository;
        _configurableFieldRepository = configurableFieldRepository;
        _configurableSelectionRepository = configurableSelectionRepository;
        _configurableGroupByRepository = configurableGroupByRepository;
        _configurableOrderByRepository = configurableOrderByRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取自定义报表主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConfigurableDto>> GetConfigurableListAsync(TaktConfigurableQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _configurableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConfigurableDto>.Create(
            data.Adapt<List<TaktConfigurableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto?> GetConfigurableByIdAsync(long id)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktConfigurableDto>();
        await FillConfigurableDetailsAsync(dto, entity);
        return dto;    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetConfigurableOptionsAsync()
    {
        var list = await _configurableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ReportStatus == TaktCommonStatus.Enabled,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ReportName ?? e.ReportCode,
        }).ToList();
    }

    /// <summary>
    /// 创建自定义报表主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> CreateConfigurableAsync(TaktConfigurableCreateDto dto)
    {
        var entity = dto.Adapt<TaktConfigurable>();
        entity.IsBuiltIn = TaktYesNo.No;
        var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
            _configurableRepository,
            x => x.ReportCode == entity.ReportCode);
        if (!isUnique_ix_configurable_code_unique)
        {
            throw new TaktBusinessException("自定义报表主的ReportCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _configurableRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OwnerUserId == entity.OwnerUserId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.OwnerUserId.GetValueOrDefault(), maxSort);
        }
        entity = await _configurableRepository.CreateAsync(entity);
                await SaveConfigurableChildrenAsync(entity, dto);
        return await GetConfigurableByIdAsync(entity.Id) ?? entity.Adapt<TaktConfigurableDto>();
    }

    /// <summary>
    /// 更新自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableAsync(long id, TaktConfigurableUpdateDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("自定义报表主不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
            _configurableRepository,
            x => x.ReportCode == entity.ReportCode,
            id);
        if (!isUnique_ix_configurable_code_unique)
        {
            throw new TaktBusinessException("自定义报表主的ReportCode已存在");
        }
        await _configurableRepository.UpdateAsync(entity);
                await SaveConfigurableChildrenAsync(entity, dto);
        return await GetConfigurableByIdAsync(id) ?? throw new TaktBusinessException("自定义报表主不存在");
    }

    /// <summary>
    /// 删除自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableByIdAsync(long id)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("自定义报表主不存在或已删除");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置自定义报表主不允许删除");
        }
        await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        var deleted = await _configurableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("自定义报表主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除自定义报表主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _configurableRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置自定义报表主不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteConfigurableByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新自定义报表主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableStatusAsync(TaktConfigurableStatusDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(dto.ConfigurableId);
        if (entity == null)
        {
            throw new TaktBusinessException("自定义报表主不存在");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.ReportStatus != TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException("不允许禁用内置自定义报表主");
        }
        entity.ReportStatus = dto.ReportStatus;
        await _configurableRepository.UpdateAsync(entity);
        return await GetConfigurableByIdAsync(dto.ConfigurableId) ?? throw new TaktBusinessException("自定义报表主不存在");
    }

    /// <summary>
    /// 更新自定义报表主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableSortAsync(TaktConfigurableSortDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(dto.ConfigurableId);
        if (entity == null)
        {
            throw new TaktBusinessException("自定义报表主不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _configurableRepository.UpdateAsync(entity);
        return await GetConfigurableByIdAsync(dto.ConfigurableId) ?? throw new TaktBusinessException("自定义报表主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConfigurableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConfigurableTemplateDto>(
            sheetName ?? "自定义报表主导入模板",
            fileName ?? "自定义报表主导入模板.xlsx");
    }

    /// <summary>
    /// 导入自定义报表主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConfigurableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConfigurableImportDto>(fileStream, sheetName ?? "自定义报表主导入模板");
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
                var entity = rows[i].Adapt<TaktConfigurable>();
                entity.IsBuiltIn = TaktYesNo.No;
                var importKey = $"{entity.ReportCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ReportCode）");
                }
                var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _configurableRepository,
                    x => x.ReportCode == entity.ReportCode);
                if (!isUnique_ix_configurable_code_unique)
                {
                    throw new TaktBusinessException("自定义报表主的ReportCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _configurableRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OwnerUserId == entity.OwnerUserId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.OwnerUserId.GetValueOrDefault(), maxSort);
                }
                await _configurableRepository.CreateAsync(entity);
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
    /// 导出自定义报表主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConfigurableAsync(TaktConfigurableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktConfigurableQueryDto());
        var list = await _configurableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableExportDto>(),
                sheetName ?? "自定义报表主数据",
                fileName ?? "自定义报表主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConfigurableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "自定义报表主数据",
            fileName ?? "自定义报表主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充自定义报表主详情（加载 OneToMany 子表：自定义报表数据源、自定义报表关联、自定义报表输出字段、自定义报表筛选、自定义报表分组、自定义报表排序）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillConfigurableDetailsAsync(TaktConfigurableDto dto, TaktConfigurable entity)
    {
        if (dto == null)
        {
            return;
        }
        // 自定义报表数据源 → dto.Sources
        var sources = await _configurableSourceRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Sources = sources.Adapt<List<TaktConfigurableSourceDto>>();
        // 自定义报表关联 → dto.Joins
        var joins = await _configurableJoinRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Joins = joins.Adapt<List<TaktConfigurableJoinDto>>();
        // 自定义报表输出字段 → dto.Fields
        var fields = await _configurableFieldRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Fields = fields.Adapt<List<TaktConfigurableFieldDto>>();
        // 自定义报表筛选 → dto.Selections
        var selections = await _configurableSelectionRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Selections = selections.Adapt<List<TaktConfigurableSelectionDto>>();
        // 自定义报表分组 → dto.GroupBys
        var groupbys = await _configurableGroupByRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.GroupBys = groupbys.Adapt<List<TaktConfigurableGroupByDto>>();
        // 自定义报表排序 → dto.OrderBys
        var orderbys = await _configurableOrderByRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.OrderBys = orderbys.Adapt<List<TaktConfigurableOrderByDto>>();
    }

    /// <summary>
    /// 保存自定义报表主子表级联（自定义报表数据源、自定义报表关联、自定义报表输出字段、自定义报表筛选、自定义报表分组、自定义报表排序；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveConfigurableChildrenAsync(TaktConfigurable entity, TaktConfigurableCreateDto dto)
    {
        // 自定义报表数据源（Sources）
        if (dto.Sources is not { Count: > 0 })
        {
            await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var sources = dto.Sources.Adapt<List<TaktConfigurableSource>>();
            foreach (var child in sources)
            {
                child.ConfigurableId = entity.Id;
            }
            var sourcesNeedSort = sources.Where(c => c.SortOrder <= 0).ToList();
            if (sourcesNeedSort.Count > 0)
            {
                var maxSort = await _configurableSourceRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, sourcesNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in sources)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in sources)
            {
            }
            await _configurableSourceRepository.CreateRangeAsync(sources);
        }
        // 自定义报表关联（Joins）
        if (dto.Joins is not { Count: > 0 })
        {
            await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var joins = dto.Joins.Adapt<List<TaktConfigurableJoin>>();
            foreach (var child in joins)
            {
                child.ConfigurableId = entity.Id;
            }
            var joinsNeedSort = joins.Where(c => c.SortOrder <= 0).ToList();
            if (joinsNeedSort.Count > 0)
            {
                var maxSort = await _configurableJoinRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, joinsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in joins)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in joins)
            {
            }
            await _configurableJoinRepository.CreateRangeAsync(joins);
        }
        // 自定义报表输出字段（Fields）
        if (dto.Fields is not { Count: > 0 })
        {
            await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var fields = dto.Fields.Adapt<List<TaktConfigurableField>>();
            foreach (var child in fields)
            {
                child.ConfigurableId = entity.Id;
            }
            var fieldsNeedSort = fields.Where(c => c.SortOrder <= 0).ToList();
            if (fieldsNeedSort.Count > 0)
            {
                var maxSort = await _configurableFieldRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, fieldsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in fields)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in fields)
            {
            }
            await _configurableFieldRepository.CreateRangeAsync(fields);
        }
        // 自定义报表筛选（Selections）
        if (dto.Selections is not { Count: > 0 })
        {
            await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var selections = dto.Selections.Adapt<List<TaktConfigurableSelection>>();
            foreach (var child in selections)
            {
                child.ConfigurableId = entity.Id;
            }
            var selectionsNeedSort = selections.Where(c => c.SortOrder <= 0).ToList();
            if (selectionsNeedSort.Count > 0)
            {
                var maxSort = await _configurableSelectionRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, selectionsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in selections)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in selections)
            {
            }
            await _configurableSelectionRepository.CreateRangeAsync(selections);
        }
        // 自定义报表分组（GroupBys）
        if (dto.GroupBys is not { Count: > 0 })
        {
            await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var groupbys = dto.GroupBys.Adapt<List<TaktConfigurableGroupBy>>();
            foreach (var child in groupbys)
            {
                child.ConfigurableId = entity.Id;
            }
            var groupbysNeedSort = groupbys.Where(c => c.SortOrder <= 0).ToList();
            if (groupbysNeedSort.Count > 0)
            {
                var maxSort = await _configurableGroupByRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, groupbysNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in groupbys)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in groupbys)
            {
            }
            await _configurableGroupByRepository.CreateRangeAsync(groupbys);
        }
        // 自定义报表排序（OrderBys）
        if (dto.OrderBys is not { Count: > 0 })
        {
            await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var orderbys = dto.OrderBys.Adapt<List<TaktConfigurableOrderBy>>();
            foreach (var child in orderbys)
            {
                child.ConfigurableId = entity.Id;
            }
            var orderbysNeedSort = orderbys.Where(c => c.SortOrder <= 0).ToList();
            if (orderbysNeedSort.Count > 0)
            {
                var maxSort = await _configurableOrderByRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, orderbysNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in orderbys)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in orderbys)
            {
            }
            await _configurableOrderByRepository.CreateRangeAsync(orderbys);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建自定义报表主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktConfigurable, bool>> QueryExpression(TaktConfigurableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConfigurable>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ReportCode != null && x.ReportCode.Contains(keywords))
                || (x.ReportName != null && x.ReportName.Contains(keywords))
                || SqlFunc.ToString(x.ReportDomain).Contains(keywords)
                || (x.ReportSubCategory != null && x.ReportSubCategory.Contains(keywords))
                || SqlFunc.ToString(x.DistinctRows).Contains(keywords)
                || SqlFunc.ToString(x.MaxExportRows).Contains(keywords)
                || SqlFunc.ToString(x.MaxQueryRows).Contains(keywords)
                || SqlFunc.ToString(x.OwnerUserId).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ReportStatus).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ReportCode))
        {
            exp = exp.And(x => x.ReportCode != null && x.ReportCode.Contains(queryDto.ReportCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReportName))
        {
            exp = exp.And(x => x.ReportName != null && x.ReportName.Contains(queryDto.ReportName));
        }

        if (queryDto?.ReportDomain.HasValue == true)
        {
            exp = exp.And(x => x.ReportDomain == queryDto.ReportDomain);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReportSubCategory))
        {
            exp = exp.And(x => x.ReportSubCategory != null && x.ReportSubCategory.Contains(queryDto.ReportSubCategory));
        }

        if (queryDto?.DistinctRows.HasValue == true)
        {
            exp = exp.And(x => x.DistinctRows == queryDto.DistinctRows);
        }

        if (queryDto?.MaxExportRows.HasValue == true)
        {
            exp = exp.And(x => x.MaxExportRows == queryDto.MaxExportRows);
        }

        if (queryDto?.MaxQueryRows.HasValue == true)
        {
            exp = exp.And(x => x.MaxQueryRows == queryDto.MaxQueryRows);
        }

        if (queryDto?.OwnerUserId.HasValue == true)
        {
            exp = exp.And(x => x.OwnerUserId == queryDto.OwnerUserId);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ReportStatus.HasValue == true)
        {
            exp = exp.And(x => x.ReportStatus == queryDto.ReportStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
