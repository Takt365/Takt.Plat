// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线主应用服务
/// </summary>
public class TaktRoutingService : TaktServiceBase, ITaktRoutingService
{
    private readonly ITaktApprovalRepository<TaktRouting> _routingRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktCompanyRepository<TaktRoutingChangeLog> _routingChangeLogRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingRepository">工艺路线主仓储</param>
    /// <param name="routingItemRepository">RoutingItem仓储</param>
    /// <param name="routingChangeLogRepository">RoutingChangeLog仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingService(
        ITaktApprovalRepository<TaktRouting> routingRepository,
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktCompanyRepository<TaktRoutingChangeLog> routingChangeLogRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingRepository = routingRepository;
        _routingItemRepository = routingItemRepository;
        _routingChangeLogRepository = routingChangeLogRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingDto>> GetRoutingListAsync(TaktRoutingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingDto>.Create(
            data.Adapt<List<TaktRoutingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto?> GetRoutingByIdAsync(long id)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktRoutingDto>();
        await FillRoutingDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工艺路线主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _routingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.RoutingName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RoutingName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> CreateRoutingAsync(TaktRoutingCreateDto dto)
    {
        var entity = dto.Adapt<TaktRouting>();
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
            _routingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.RoutingCode == entity.RoutingCode);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
        {
            throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
        }
        entity = await _routingRepository.CreateAsync(entity);
                await SaveRoutingChildrenAsync(entity, dto);
        return await GetRoutingByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingDto>();
    }

    /// <summary>
    /// 更新工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> UpdateRoutingAsync(long id, TaktRoutingUpdateDto dto)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
            _routingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.RoutingCode == entity.RoutingCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
        {
            throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
        }
        await _routingRepository.UpdateAsync(entity);
                await SaveRoutingChildrenAsync(entity, dto);
        return await GetRoutingByIdAsync(id) ?? throw new TaktBusinessException("工艺路线主不存在");
    }

    /// <summary>
    /// 删除工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingByIdAsync(long id)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在或已删除");
        }
        await _routingItemRepository.DeleteAsync(x => x.RoutingId == entity.Id);
        await _routingChangeLogRepository.DeleteAsync(x => x.RoutingId == entity.Id);
        var deleted = await _routingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工艺路线主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> UpdateRoutingStatusAsync(TaktRoutingStatusDto dto)
    {
        var entity = await _routingRepository.GetByIdAsync(dto.RoutingId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在");
        }
        entity.RoutingStatus = dto.RoutingStatus;
        await _routingRepository.UpdateAsync(entity);
        return await GetRoutingByIdAsync(dto.RoutingId) ?? throw new TaktBusinessException("工艺路线主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoutingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoutingTemplateDto>(
            sheetName ?? "工艺路线主导入模板",
            fileName ?? "工艺路线主导入模板.xlsx");
    }

    /// <summary>
    /// 导入工艺路线主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoutingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoutingImportDto>(fileStream, sheetName ?? "工艺路线主导入模板");
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
                var entity = rows[i].Adapt<TaktRouting>();
                var importKey = $"{entity.PlantCode}|{entity.RoutingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、RoutingCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
                    _routingRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.RoutingCode == entity.RoutingCode);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
                {
                    throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
                }
                await _routingRepository.CreateAsync(entity);
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
    /// 导出工艺路线主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingAsync(TaktRoutingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktRoutingQueryDto());
        var list = await _routingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingExportDto>(),
                sheetName ?? "工艺路线主数据",
                fileName ?? "工艺路线主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线主数据",
            fileName ?? "工艺路线主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工艺路线主详情（加载 OneToMany 子表：工艺路线明细、工艺路线变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillRoutingDetailsAsync(TaktRoutingDto dto, TaktRouting entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工艺路线明细 → dto.Items
        var items = await _routingItemRepository.GetListAsync(x => x.RoutingId == entity.Id);
        dto.Items = items.Adapt<List<TaktRoutingItemDto>>();
        // 工艺路线变更日志 → dto.ChangeLogs
        var changelogs = await _routingChangeLogRepository.GetListAsync(x => x.RoutingId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktRoutingChangeLogDto>>();
    }

    /// <summary>
    /// 保存工艺路线主子表级联（工艺路线明细、工艺路线变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveRoutingChildrenAsync(TaktRouting entity, TaktRoutingCreateDto dto)
    {
        // 工艺路线明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _routingItemRepository.DeleteAsync(x => x.RoutingId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktRoutingItem>>();
            foreach (var child in items)
            {
                child.RoutingId = entity.Id;
            }
            var itemsNeedSort = items.Where(c => c.SortOrder <= 0).ToList();
            if (itemsNeedSort.Count > 0)
            {
                var maxSort = await _routingItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, itemsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in items)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.Id.ToString();
                var maxLine = await _routingItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].RoutingId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"工艺路线明细第{i + 1}项与本次提交的其他项重复（CompanyCode、RoutingId、LineNumber）");
                            }
                        }
            await _routingItemRepository.DeleteAsync(x => x.RoutingId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
                _routingItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.RoutingId == child.RoutingId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
            {
                throw new TaktBusinessException("工艺路线明细的CompanyCode、RoutingId、LineNumber已存在");
            }
            }
            await _routingItemRepository.CreateRangeAsync(items);
        }
        // 工艺路线变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _routingChangeLogRepository.DeleteAsync(x => x.RoutingId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktRoutingChangeLog>>();
            foreach (var child in changelogs)
            {
                child.RoutingId = entity.Id;
            }
            await _routingChangeLogRepository.DeleteAsync(x => x.RoutingId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _routingChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRouting, bool>> QueryExpression(TaktRoutingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRouting>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || (x.RoutingName != null && x.RoutingName.Contains(keywords))
                || SqlFunc.ToString(x.Purpose).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.Version != null && x.Version.Contains(keywords))
                || SqlFunc.ToString(x.RoutingStatus).Contains(keywords)
                || (x.RoutingDescription != null && x.RoutingDescription.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingCode))
        {
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(queryDto.RoutingCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingName))
        {
            exp = exp.And(x => x.RoutingName != null && x.RoutingName.Contains(queryDto.RoutingName));
        }

        if (queryDto?.Purpose.HasValue == true)
        {
            exp = exp.And(x => x.Purpose == queryDto.Purpose);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Version))
        {
            exp = exp.And(x => x.Version != null && x.Version.Contains(queryDto.Version));
        }

        if (queryDto?.RoutingStatus.HasValue == true)
        {
            exp = exp.And(x => x.RoutingStatus == queryDto.RoutingStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingDescription))
        {
            exp = exp.And(x => x.RoutingDescription != null && x.RoutingDescription.Contains(queryDto.RoutingDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
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
