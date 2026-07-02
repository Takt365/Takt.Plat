// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料清单应用服务
/// </summary>
public class TaktBillOfMaterialService : TaktServiceBase, ITaktBillOfMaterialService
{
    private const int MaxExplosionLevel = 20;
    private const int MaxExplosionLines = 5000;
    private const int PublishedBomStatus = 1;

    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _billOfMaterialRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialChangeLog> _billOfMaterialChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialRepository">物料清单仓储</param>
    /// <param name="billOfMaterialItemRepository">BillOfMaterialItem仓储</param>
    /// <param name="billOfMaterialChangeLogRepository">BillOfMaterialChangeLog仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialService(
        ITaktCompanyRepository<TaktBillOfMaterial> billOfMaterialRepository,
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktCompanyRepository<TaktBillOfMaterialChangeLog> billOfMaterialChangeLogRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialRepository = billOfMaterialRepository;
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _billOfMaterialChangeLogRepository = billOfMaterialChangeLogRepository;
        _materialPlantRepository = materialPlantRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料清单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialDto>> GetBillOfMaterialListAsync(TaktBillOfMaterialQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialDto>.Create(
            data.Adapt<List<TaktBillOfMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto?> GetBillOfMaterialByIdAsync(long id)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktBillOfMaterialDto>();
        await FillBillOfMaterialDetailsAsync(dto, entity);
        return dto;
    }

    /// <summary>
    /// 获取物料清单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BomStatus == 1,
            x => x.BomName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.BomName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料清单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> CreateBillOfMaterialAsync(TaktBillOfMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterial>();
        var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ParentMaterialCode == entity.ParentMaterialCode
                && x.BomType == entity.BomType
                && x.BomVersion == entity.BomVersion);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
        {
            throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _billOfMaterialRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentMaterialId == entity.ParentMaterialId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ParentMaterialId, maxSort);
        }
        entity = await _billOfMaterialRepository.CreateAsync(entity);
                await SaveBillOfMaterialChildrenAsync(entity, dto);
        return await GetBillOfMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialDto>();
    }

    /// <summary>
    /// 更新物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialAsync(long id, TaktBillOfMaterialUpdateDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ParentMaterialCode == entity.ParentMaterialCode
                && x.BomType == entity.BomType
                && x.BomVersion == entity.BomVersion,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
        {
            throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
        }
        await _billOfMaterialRepository.UpdateAsync(entity);
                await SaveBillOfMaterialChildrenAsync(entity, dto);
        return await GetBillOfMaterialByIdAsync(id) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 删除物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialByIdAsync(long id)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在或已删除");
        }
        await _billOfMaterialItemRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
        await _billOfMaterialChangeLogRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
        var deleted = await _billOfMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料清单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料清单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料清单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialStatusAsync(TaktBillOfMaterialStatusDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.BomStatus = dto.BomStatus;
        await _billOfMaterialRepository.UpdateAsync(entity);
        return await GetBillOfMaterialByIdAsync(dto.BillOfMaterialId) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 更新物料清单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialDto> UpdateBillOfMaterialSortAsync(TaktBillOfMaterialSortDto dto)
    {
        var entity = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _billOfMaterialRepository.UpdateAsync(entity);
        return await GetBillOfMaterialByIdAsync(dto.BillOfMaterialId) ?? throw new TaktBusinessException("物料清单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialTemplateDto>(
            sheetName ?? "物料清单导入模板",
            fileName ?? "物料清单导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料清单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialImportDto>(fileStream, sheetName ?? "物料清单导入模板");
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
                var entity = rows[i].Adapt<TaktBillOfMaterial>();
                var importKey = $"{entity.PlantCode}|{entity.ParentMaterialCode}|{entity.BomType}|{entity.BomVersion}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ParentMaterialCode、BomType、BomVersion）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_header_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ParentMaterialCode == entity.ParentMaterialCode
                        && x.BomType == entity.BomType
                        && x.BomVersion == entity.BomVersion);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_header_unique)
                {
                    throw new TaktBusinessException("物料清单的PlantCode、ParentMaterialCode、BomType、BomVersion已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _billOfMaterialRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentMaterialId == entity.ParentMaterialId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ParentMaterialId, maxSort);
                }
                await _billOfMaterialRepository.CreateAsync(entity);
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
    /// 导出物料清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialAsync(TaktBillOfMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBillOfMaterialQueryDto());
        var list = await _billOfMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialExportDto>(),
                sheetName ?? "物料清单数据",
                fileName ?? "物料清单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料清单数据",
            fileName ?? "物料清单导出.xlsx");
    }

    /// <summary>
    /// BOM 多层递归展开（单层存储、运行时多层展开）
    /// </summary>
    /// <param name="queryDto">展开查询参数</param>
    /// <returns>展开结果；根 BOM 不存在时返回 null</returns>
    public async Task<TaktBillOfMaterialExplosionDto?> GetBillOfMaterialExplosionAsync(TaktBillOfMaterialExplosionQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        if (queryDto.BillOfMaterialId <= 0)
        {
            throw new TaktBusinessException("展开根 BOM ID 无效");
        }

        var demandQuantity = queryDto.Quantity <= 0 ? 1m : queryDto.Quantity;
        var maxLevel = Math.Min(MaxExplosionLevel, Math.Max(0, queryDto.MaxLevel));

        var root = await _billOfMaterialRepository.GetByIdAsync(queryDto.BillOfMaterialId);
        if (root == null || root.TenantCode != CurrentTenantCode || root.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }

        var now = DateTime.Now;
        var plantBoms = await _billOfMaterialRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == root.PlantCode
            && x.BomType == root.BomType);
        var childBomLookup = BuildChildBomLookup(plantBoms, now);

        var bomIds = plantBoms.Select(x => x.Id).ToHashSet();
        bomIds.Add(root.Id);
        var allItems = await _billOfMaterialItemRepository.GetListAsync(x => bomIds.Contains(x.BillOfMaterialId));
        var itemsByBomId = allItems
            .GroupBy(x => x.BillOfMaterialId)
            .ToDictionary(g => g.Key, g => g.OrderBy(i => i.LineNumber).ToList());

        var materialNameMap = BuildMaterialNameMap(plantBoms, root);
        var materialPlants = await _materialPlantRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == root.PlantCode);
        foreach (var materialPlant in materialPlants)
        {
            if (!string.IsNullOrWhiteSpace(materialPlant.MaterialCode))
            {
                materialNameMap[materialPlant.MaterialCode] = materialPlant.MaterialName;
            }
        }

        var lines = new List<TaktBillOfMaterialExplosionLineDto>();
        if (queryDto.IncludeLevelZero)
        {
            lines.Add(BuildLevelZeroLine(root, demandQuantity));
        }

        if (maxLevel > 0)
        {
            var rootItems = itemsByBomId.GetValueOrDefault(root.Id) ?? new List<TaktBillOfMaterialItem>();
            var ancestorCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                root.ParentMaterialCode
            };
            ExpandBillOfMaterialLines(
                root,
                rootItems,
                demandQuantity,
                1,
                root.ParentMaterialCode,
                maxLevel,
                ancestorCodes,
                childBomLookup,
                itemsByBomId,
                materialNameMap,
                lines);
        }

        return new TaktBillOfMaterialExplosionDto
        {
            BillOfMaterialId = root.Id,
            BomCode = root.BomCode,
            ParentMaterialCode = root.ParentMaterialCode,
            ParentMaterialName = root.ParentMaterialName,
            Quantity = demandQuantity,
            Lines = lines
                .OrderBy(x => x.HierarchyLevel)
                .ThenBy(x => x.LineNumber)
                .ThenBy(x => x.MaterialCode, StringComparer.OrdinalIgnoreCase)
                .ToList()
        };
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充物料清单详情（加载 OneToMany 子表：物料清单明细、BOM变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillBillOfMaterialDetailsAsync(TaktBillOfMaterialDto dto, TaktBillOfMaterial entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料清单明细 → dto.Items
        var items = await _billOfMaterialItemRepository.GetListAsync(x => x.BillOfMaterialId == entity.Id);
        dto.Items = items.Adapt<List<TaktBillOfMaterialItemDto>>();
        // BOM变更记录 → dto.ChangeLogs
        var changelogs = await _billOfMaterialChangeLogRepository.GetListAsync(x => x.BillOfMaterialId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktBillOfMaterialChangeLogDto>>();
    }

    /// <summary>
    /// 保存物料清单子表级联（物料清单明细、BOM变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveBillOfMaterialChildrenAsync(TaktBillOfMaterial entity, TaktBillOfMaterialCreateDto dto)
    {
        // 物料清单明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _billOfMaterialItemRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktBillOfMaterialItem>>();
            foreach (var child in items)
            {
                child.BillOfMaterialId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.BomCode) ? entity.BomCode : entity.Id.ToString();
                var maxLine = await _billOfMaterialItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].BillOfMaterialId}|{items[i].LineNumber}|{items[i].MaterialId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"物料清单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、BillOfMaterialId、LineNumber、MaterialId）");
                            }
                        }
            await _billOfMaterialItemRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
                _billOfMaterialItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.BillOfMaterialId == child.BillOfMaterialId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialId == child.MaterialId);
            if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
            {
                throw new TaktBusinessException("物料清单明细的CompanyCode、BillOfMaterialId、LineNumber、MaterialId已存在");
            }
            }
            await _billOfMaterialItemRepository.CreateRangeAsync(items);
        }
        // BOM变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _billOfMaterialChangeLogRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktBillOfMaterialChangeLog>>();
            foreach (var child in changelogs)
            {
                child.BillOfMaterialId = entity.Id;
            }
            await _billOfMaterialChangeLogRepository.DeleteAsync(x => x.BillOfMaterialId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _billOfMaterialChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料清单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterial, bool>> QueryExpression(TaktBillOfMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterial>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.BomName != null && x.BomName.Contains(keywords))
                || SqlFunc.ToString(x.ParentMaterialId).Contains(keywords)
                || (x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(keywords))
                || (x.ParentMaterialName != null && x.ParentMaterialName.Contains(keywords))
                || (x.BomVersion != null && x.BomVersion.Contains(keywords))
                || SqlFunc.ToString(x.BomType).Contains(keywords)
                || (x.AlternativeBomNumber != null && x.AlternativeBomNumber.Contains(keywords))
                || (x.ParentMaterialUnit != null && x.ParentMaterialUnit.Contains(keywords))
                || SqlFunc.ToString(x.ParentMaterialQuantity).Contains(keywords)
                || SqlFunc.ToString(x.BomStatus).Contains(keywords)
                || (x.BomDescription != null && x.BomDescription.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.BomCode))
        {
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(queryDto.BomCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomName))
        {
            exp = exp.And(x => x.BomName != null && x.BomName.Contains(queryDto.BomName));
        }

        if (queryDto?.ParentMaterialId.HasValue == true)
        {
            exp = exp.And(x => x.ParentMaterialId == queryDto.ParentMaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ParentMaterialCode))
        {
            exp = exp.And(x => x.ParentMaterialCode != null && x.ParentMaterialCode.Contains(queryDto.ParentMaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ParentMaterialName))
        {
            exp = exp.And(x => x.ParentMaterialName != null && x.ParentMaterialName.Contains(queryDto.ParentMaterialName));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomVersion))
        {
            exp = exp.And(x => x.BomVersion != null && x.BomVersion.Contains(queryDto.BomVersion));
        }

        if (queryDto?.BomType.HasValue == true)
        {
            exp = exp.And(x => x.BomType == queryDto.BomType);
        }

        if (!string.IsNullOrEmpty(queryDto?.AlternativeBomNumber))
        {
            exp = exp.And(x => x.AlternativeBomNumber != null && x.AlternativeBomNumber.Contains(queryDto.AlternativeBomNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.ParentMaterialUnit))
        {
            exp = exp.And(x => x.ParentMaterialUnit != null && x.ParentMaterialUnit.Contains(queryDto.ParentMaterialUnit));
        }

        if (queryDto?.ParentMaterialQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ParentMaterialQuantity == queryDto.ParentMaterialQuantity);
        }

        if (queryDto?.BomStatus.HasValue == true)
        {
            exp = exp.And(x => x.BomStatus == queryDto.BomStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.BomDescription))
        {
            exp = exp.And(x => x.BomDescription != null && x.BomDescription.Contains(queryDto.BomDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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

    // ========================================
    // BOM 多层展开（运行时）
    // ========================================

    /// <summary>
    /// 构建子件 BOM 查找表（同工厂、同 BOM 类型；已发布且在有效期内；多版本取最新生效）
    /// </summary>
    /// <param name="plantBoms">同工厂同类型 BOM 头列表</param>
    /// <param name="asOf">生效判定时间点</param>
    /// <returns>父件物料编码 → BOM 头</returns>
    private static Dictionary<string, TaktBillOfMaterial> BuildChildBomLookup(
        IEnumerable<TaktBillOfMaterial> plantBoms,
        DateTime asOf)
    {
        return plantBoms
            .Where(b => b.BomStatus == PublishedBomStatus
                && b.EffectiveDate <= asOf
                && (b.ExpiryDate == null || b.ExpiryDate >= asOf))
            .GroupBy(b => b.ParentMaterialCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(b => b.EffectiveDate)
                    .ThenByDescending(b => b.BomVersion, StringComparer.Ordinal)
                    .First(),
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 构建物料编码 → 名称映射（BOM 头冗余父件名称）
    /// </summary>
    /// <param name="plantBoms">同工厂 BOM 头</param>
    /// <param name="root">根 BOM</param>
    /// <returns>物料编码 → 名称</returns>
    private static Dictionary<string, string> BuildMaterialNameMap(
        IEnumerable<TaktBillOfMaterial> plantBoms,
        TaktBillOfMaterial root)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var bom in plantBoms)
        {
            if (!string.IsNullOrWhiteSpace(bom.ParentMaterialCode))
            {
                map[bom.ParentMaterialCode] = bom.ParentMaterialName;
            }
        }
        if (!string.IsNullOrWhiteSpace(root.ParentMaterialCode))
        {
            map[root.ParentMaterialCode] = root.ParentMaterialName;
        }
        return map;
    }

    /// <summary>
    /// 构建层级 0 父件行
    /// </summary>
    /// <param name="root">根 BOM</param>
    /// <param name="demandQuantity">需求数量</param>
    /// <returns>展开行</returns>
    private static TaktBillOfMaterialExplosionLineDto BuildLevelZeroLine(
        TaktBillOfMaterial root,
        decimal demandQuantity)
    {
        return new TaktBillOfMaterialExplosionLineDto
        {
            HierarchyLevel = 0,
            LevelPrefix = string.Empty,
            SourceBillOfMaterialId = root.Id,
            SourceBillOfMaterialItemId = null,
            LineNumber = 0,
            MaterialId = root.ParentMaterialId,
            MaterialCode = root.ParentMaterialCode,
            MaterialName = root.ParentMaterialName,
            ImmediateParentMaterialCode = string.Empty,
            UsageQuantity = root.ParentMaterialQuantity <= 0 ? 1m : root.ParentMaterialQuantity,
            MaterialUnit = root.ParentMaterialUnit,
            ScrapRate = 0,
            CumulativeQuantity = demandQuantity,
            OperationSeq = 0,
            IsPhantom = 0,
            IsOptional = 0,
            HasChildBom = 0,
            IsCircular = 0
        };
    }

    /// <summary>
    /// 递归展开 BOM 明细行
    /// </summary>
    /// <param name="bom">当前 BOM 头</param>
    /// <param name="items">当前 BOM 直接子件</param>
    /// <param name="parentDemandQuantity">直接父件需求量</param>
    /// <param name="currentLevel">当前层级（1=直接子件）</param>
    /// <param name="immediateParentMaterialCode">直接父件物料编码</param>
    /// <param name="maxLevel">最大展开层级</param>
    /// <param name="ancestorMaterialCodes">祖先物料编码（环检测）</param>
    /// <param name="childBomLookup">子件 BOM 查找表</param>
    /// <param name="itemsByBomId">BOM ID → 明细行</param>
    /// <param name="materialNameMap">物料编码 → 名称</param>
    /// <param name="lines">输出行集合</param>
    private void ExpandBillOfMaterialLines(
        TaktBillOfMaterial bom,
        IReadOnlyList<TaktBillOfMaterialItem> items,
        decimal parentDemandQuantity,
        int currentLevel,
        string immediateParentMaterialCode,
        int maxLevel,
        HashSet<string> ancestorMaterialCodes,
        IReadOnlyDictionary<string, TaktBillOfMaterial> childBomLookup,
        IReadOnlyDictionary<long, List<TaktBillOfMaterialItem>> itemsByBomId,
        IReadOnlyDictionary<string, string> materialNameMap,
        List<TaktBillOfMaterialExplosionLineDto> lines)
    {
        if (currentLevel > maxLevel || items.Count == 0)
        {
            return;
        }

        var baseQuantity = bom.ParentMaterialQuantity <= 0 ? 1m : bom.ParentMaterialQuantity;
        foreach (var item in items)
        {
            if (lines.Count >= MaxExplosionLines)
            {
                throw new TaktBusinessException($"BOM 展开行数超过上限 {MaxExplosionLines}，请缩小需求数量或降低展开层级");
            }

            var cumulativeQuantity = parentDemandQuantity * item.ActualUsageQuantity / baseQuantity;
            childBomLookup.TryGetValue(item.MaterialCode, out var childBom);
            var hasChildBom = childBom != null ? 1 : 0;
            var isCircular = ancestorMaterialCodes.Contains(item.MaterialCode) ? 1 : 0;
            materialNameMap.TryGetValue(item.MaterialCode, out var materialName);

            lines.Add(new TaktBillOfMaterialExplosionLineDto
            {
                HierarchyLevel = currentLevel,
                LevelPrefix = BuildLevelPrefix(currentLevel),
                SourceBillOfMaterialId = bom.Id,
                SourceBillOfMaterialItemId = item.Id,
                LineNumber = item.LineNumber,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                MaterialName = materialName,
                ImmediateParentMaterialCode = immediateParentMaterialCode,
                UsageQuantity = item.UsageQuantity,
                MaterialUnit = item.MaterialUnit,
                ScrapRate = item.ScrapRate,
                CumulativeQuantity = cumulativeQuantity,
                OperationSeq = item.OperationSeq,
                WorkCenter = item.WorkCenter,
                Position = item.Position,
                IsPhantom = item.IsPhantom,
                IsOptional = item.IsOptional,
                SubstituteGroup = item.SubstituteGroup,
                HasChildBom = hasChildBom,
                IsCircular = isCircular
            });

            if (childBom == null || isCircular == 1 || currentLevel >= maxLevel)
            {
                continue;
            }

            var childItems = itemsByBomId.GetValueOrDefault(childBom.Id) ?? new List<TaktBillOfMaterialItem>();
            var nextAncestors = new HashSet<string>(ancestorMaterialCodes, StringComparer.OrdinalIgnoreCase)
            {
                item.MaterialCode
            };
            ExpandBillOfMaterialLines(
                childBom,
                childItems,
                cumulativeQuantity,
                currentLevel + 1,
                item.MaterialCode,
                maxLevel,
                nextAncestors,
                childBomLookup,
                itemsByBomId,
                materialNameMap,
                lines);
        }
    }

    /// <summary>
    /// 构建层级显示前缀
    /// </summary>
    /// <param name="level">层级</param>
    /// <returns>前缀字符串</returns>
    private static string BuildLevelPrefix(int level)
    {
        if (level <= 0)
        {
            return string.Empty;
        }
        return new string('.', level);
    }
}
