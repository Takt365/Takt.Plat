// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemService.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线明细应用服务实现
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

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线明细应用服务
/// </summary>
public class TaktRoutingItemService : TaktServiceBase, ITaktRoutingItemService
{
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItemArgument> _routingItemArgumentRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingItemRepository">工艺路线明细仓储</param>
    /// <param name="routingItemArgumentRepository">RoutingItemArgument仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingItemService(
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktCompanyRepository<TaktRoutingItemArgument> routingItemArgumentRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingItemRepository = routingItemRepository;
        _routingItemArgumentRepository = routingItemArgumentRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingItemDto>> GetRoutingItemListAsync(TaktRoutingItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingItemDto>.Create(
            data.Adapt<List<TaktRoutingItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto?> GetRoutingItemByIdAsync(long id)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktRoutingItemDto>();
        await FillRoutingItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工艺路线明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _routingItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.RoutingCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RoutingCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> CreateRoutingItemAsync(TaktRoutingItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktRoutingItem>();
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
            _routingItemRepository,
            x => x.RoutingId == entity.RoutingId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
        {
            throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _routingItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingId, maxSort);
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _routingItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.RoutingId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _routingItemRepository.CreateAsync(entity);
                await SaveRoutingItemChildrenAsync(entity, dto);
        return await GetRoutingItemByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingItemDto>();
    }

    /// <summary>
    /// 更新工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> UpdateRoutingItemAsync(long id, TaktRoutingItemUpdateDto dto)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
            _routingItemRepository,
            x => x.RoutingId == entity.RoutingId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
        {
            throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
        }
        await _routingItemRepository.UpdateAsync(entity);
                await SaveRoutingItemChildrenAsync(entity, dto);
        return await GetRoutingItemByIdAsync(id) ?? throw new TaktBusinessException("工艺路线明细不存在");
    }

    /// <summary>
    /// 删除工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemByIdAsync(long id)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在或已删除");
        }
        await _routingItemArgumentRepository.DeleteAsync(x => x.RoutingItemId == entity.Id);
        var deleted = await _routingItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工艺路线明细排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> UpdateRoutingItemSortAsync(TaktRoutingItemSortDto dto)
    {
        var entity = await _routingItemRepository.GetByIdAsync(dto.RoutingItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _routingItemRepository.UpdateAsync(entity);
        return await GetRoutingItemByIdAsync(dto.RoutingItemId) ?? throw new TaktBusinessException("工艺路线明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoutingItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoutingItemTemplateDto>(
            sheetName ?? "工艺路线明细导入模板",
            fileName ?? "工艺路线明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入工艺路线明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoutingItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoutingItemImportDto>(fileStream, sheetName ?? "工艺路线明细导入模板");
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
                var entity = rows[i].Adapt<TaktRoutingItem>();
                var importKey = $"{entity.RoutingId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RoutingId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _routingItemRepository,
                    x => x.RoutingId == entity.RoutingId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
                {
                    throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _routingItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingId, maxSort);
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _routingItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.RoutingId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _routingItemRepository.CreateAsync(entity);
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
    /// 导出工艺路线明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingItemAsync(TaktRoutingItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktRoutingItemQueryDto());
        var list = await _routingItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingItemExportDto>(),
                sheetName ?? "工艺路线明细数据",
                fileName ?? "工艺路线明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线明细数据",
            fileName ?? "工艺路线明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工艺路线明细详情（加载 OneToMany 子表：工艺路线工序参数）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillRoutingItemDetailsAsync(TaktRoutingItemDto dto, TaktRoutingItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工艺路线工序参数 → dto.Arguments
        var arguments = await _routingItemArgumentRepository.GetListAsync(x => x.RoutingItemId == entity.Id);
        dto.Arguments = arguments.Adapt<List<TaktRoutingItemArgumentDto>>();
    }

    /// <summary>
    /// 保存工艺路线明细子表级联（工艺路线工序参数；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveRoutingItemChildrenAsync(TaktRoutingItem entity, TaktRoutingItemCreateDto dto)
    {
        // 工艺路线工序参数（Arguments）
        if (dto.Arguments is not { Count: > 0 })
        {
            await _routingItemArgumentRepository.DeleteAsync(x => x.RoutingItemId == entity.Id);
        }
        else
        {
            var arguments = dto.Arguments.Adapt<List<TaktRoutingItemArgument>>();
            foreach (var child in arguments)
            {
                child.RoutingItemId = entity.Id;
            }
            var argumentsNeedSort = arguments.Where(c => c.SortOrder <= 0).ToList();
            if (argumentsNeedSort.Count > 0)
            {
                var maxSort = await _routingItemArgumentRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingItemId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, argumentsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in arguments)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _routingItemArgumentRepository.DeleteAsync(x => x.RoutingItemId == entity.Id);
            foreach (var child in arguments)
            {
            }
            await _routingItemArgumentRepository.CreateRangeAsync(arguments);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRoutingItem, bool>> QueryExpression(TaktRoutingItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRoutingItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.RoutingId).Contains(keywords)
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || SqlFunc.ToString(x.BaseQuantity).Contains(keywords)
                || SqlFunc.ToString(x.StandardMinutes).Contains(keywords)
                || (x.TimeUnit != null && x.TimeUnit.Contains(keywords))
                || SqlFunc.ToString(x.StandardShorts).Contains(keywords)
                || (x.PointsUnit != null && x.PointsUnit.Contains(keywords))
                || SqlFunc.ToString(x.PointsToMinutesRate).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedMinutes).Contains(keywords)
                || SqlFunc.ToString(x.SetupMinutes).Contains(keywords)
                || SqlFunc.ToString(x.TeardownMinutes).Contains(keywords)
                || SqlFunc.ToString(x.IsQualityCheck).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ProcessDescription != null && x.ProcessDescription.Contains(keywords))
                || SqlFunc.ToString(x.ProcessSegmentType).Contains(keywords)
                || (x.ExtJson != null && x.ExtJson.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.RoutingId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingId == queryDto.RoutingId);
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingCode))
        {
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(queryDto.RoutingCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.BaseUnit))
        {
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(queryDto.BaseUnit));
        }

        if (queryDto?.BaseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.BaseQuantity == queryDto.BaseQuantity);
        }

        if (queryDto?.StandardMinutes.HasValue == true)
        {
            exp = exp.And(x => x.StandardMinutes == queryDto.StandardMinutes);
        }

        if (!string.IsNullOrEmpty(queryDto?.TimeUnit))
        {
            exp = exp.And(x => x.TimeUnit != null && x.TimeUnit.Contains(queryDto.TimeUnit));
        }

        if (queryDto?.StandardShorts.HasValue == true)
        {
            exp = exp.And(x => x.StandardShorts == queryDto.StandardShorts);
        }

        if (!string.IsNullOrEmpty(queryDto?.PointsUnit))
        {
            exp = exp.And(x => x.PointsUnit != null && x.PointsUnit.Contains(queryDto.PointsUnit));
        }

        if (queryDto?.PointsToMinutesRate.HasValue == true)
        {
            exp = exp.And(x => x.PointsToMinutesRate == queryDto.PointsToMinutesRate);
        }

        if (queryDto?.ConvertedMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedMinutes == queryDto.ConvertedMinutes);
        }

        if (queryDto?.SetupMinutes.HasValue == true)
        {
            exp = exp.And(x => x.SetupMinutes == queryDto.SetupMinutes);
        }

        if (queryDto?.TeardownMinutes.HasValue == true)
        {
            exp = exp.And(x => x.TeardownMinutes == queryDto.TeardownMinutes);
        }

        if (queryDto?.IsQualityCheck.HasValue == true)
        {
            exp = exp.And(x => x.IsQualityCheck == queryDto.IsQualityCheck);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessDescription))
        {
            exp = exp.And(x => x.ProcessDescription != null && x.ProcessDescription.Contains(queryDto.ProcessDescription));
        }

        if (queryDto?.ProcessSegmentType.HasValue == true)
        {
            exp = exp.And(x => x.ProcessSegmentType == queryDto.ProcessSegmentType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtJson))
        {
            exp = exp.And(x => x.ExtJson != null && x.ExtJson.Contains(queryDto.ExtJson));
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
