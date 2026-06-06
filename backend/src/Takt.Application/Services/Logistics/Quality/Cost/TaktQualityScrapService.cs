// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：品质废弃主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Quality.Cost;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质废弃主应用服务
/// </summary>
public class TaktQualityScrapService : TaktServiceBase, ITaktQualityScrapService
{
    private readonly ITaktCompanyRepository<TaktQualityScrap> _qualityScrapRepository;
    private readonly ITaktCompanyRepository<TaktQualityScrapItem> _qualityScrapItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityScrapRepository">品质废弃主仓储</param>
    /// <param name="qualityScrapItemRepository">QualityScrapItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityScrapService(
        ITaktCompanyRepository<TaktQualityScrap> qualityScrapRepository,
        ITaktCompanyRepository<TaktQualityScrapItem> qualityScrapItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityScrapRepository = qualityScrapRepository;
        _qualityScrapItemRepository = qualityScrapItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质废弃主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityScrapDto>> GetQualityScrapListAsync(TaktQualityScrapQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityScrapRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityScrapDto>.Create(
            data.Adapt<List<TaktQualityScrapDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityScrapDto?> GetQualityScrapByIdAsync(long id)
    {
        var entity = await _qualityScrapRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityScrapDto>();
        await FillQualityScrapDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质废弃主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityScrapOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityScrapRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建品质废弃主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityScrapDto> CreateQualityScrapAsync(TaktQualityScrapCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityScrap>();
        var isUnique_ix_takt_logistics_quality_scrap_qs_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityScrapRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityScrapCode == entity.QualityScrapCode
                && x.ScrapDate == entity.ScrapDate);
        if (!isUnique_ix_takt_logistics_quality_scrap_qs_unique)
        {
            throw new TaktBusinessException("品质废弃主的PlantCode、QualityScrapCode、ScrapDate已存在");
        }
        entity = await _qualityScrapRepository.CreateAsync(entity);
                await SaveQualityScrapChildrenAsync(entity, dto);
        return await GetQualityScrapByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityScrapDto>();
    }

    /// <summary>
    /// 更新品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityScrapDto> UpdateQualityScrapAsync(long id, TaktQualityScrapUpdateDto dto)
    {
        var entity = await _qualityScrapRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质废弃主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_scrap_qs_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityScrapRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityScrapCode == entity.QualityScrapCode
                && x.ScrapDate == entity.ScrapDate,
            id);
        if (!isUnique_ix_takt_logistics_quality_scrap_qs_unique)
        {
            throw new TaktBusinessException("品质废弃主的PlantCode、QualityScrapCode、ScrapDate已存在");
        }
        await _qualityScrapRepository.UpdateAsync(entity);
                await SaveQualityScrapChildrenAsync(entity, dto);
        return await GetQualityScrapByIdAsync(id) ?? throw new TaktBusinessException("品质废弃主不存在");
    }

    /// <summary>
    /// 删除品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityScrapByIdAsync(long id)
    {
        var entity = await _qualityScrapRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质废弃主不存在或已删除");
        }
        await _qualityScrapItemRepository.DeleteAsync(x => x.QualityScrapId == entity.Id);
        var deleted = await _qualityScrapRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质废弃主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质废弃主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityScrapBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityScrapByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityScrapTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityScrapTemplateDto>(
            sheetName ?? "品质废弃主导入模板",
            fileName ?? "品质废弃主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质废弃主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityScrapAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityScrapImportDto>(fileStream, sheetName ?? "品质废弃主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityScrap>();
                var importKey = $"{entity.PlantCode}|{entity.QualityScrapCode}|{entity.ScrapDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityScrapCode、ScrapDate）");
                }
                var isUnique_ix_takt_logistics_quality_scrap_qs_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityScrapRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityScrapCode == entity.QualityScrapCode
                        && x.ScrapDate == entity.ScrapDate);
                if (!isUnique_ix_takt_logistics_quality_scrap_qs_unique)
                {
                    throw new TaktBusinessException("品质废弃主的PlantCode、QualityScrapCode、ScrapDate已存在");
                }
                await _qualityScrapRepository.CreateAsync(entity);
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
    /// 导出品质废弃主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityScrapAsync(TaktQualityScrapQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityScrapQueryDto());
        var list = await _qualityScrapRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityScrapExportDto>(),
                sheetName ?? "品质废弃主数据",
                fileName ?? "品质废弃主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityScrapExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质废弃主数据",
            fileName ?? "品质废弃主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充品质废弃主详情（加载 OneToMany 子表：品质废弃明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityScrapDetailsAsync(TaktQualityScrapDto dto, TaktQualityScrap entity)
    {
        if (dto == null)
        {
            return;
        }
        // 品质废弃明细 → dto.ScrapItems
        var scrapitems = await _qualityScrapItemRepository.GetListAsync(x => x.QualityScrapId == entity.Id);
        dto.ScrapItems = scrapitems.Adapt<List<TaktQualityScrapItemDto>>();
    }

    /// <summary>
    /// 保存品质废弃主子表级联（品质废弃明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityScrapChildrenAsync(TaktQualityScrap entity, TaktQualityScrapCreateDto dto)
    {
        // 品质废弃明细（ScrapItems）
        if (dto.ScrapItems is not { Count: > 0 })
        {
            await _qualityScrapItemRepository.DeleteAsync(x => x.QualityScrapId == entity.Id);
        }
        else
        {
            var scrapitems = dto.ScrapItems.Adapt<List<TaktQualityScrapItem>>();
            foreach (var child in scrapitems)
            {
                child.QualityScrapId = entity.Id;
            }
            var scrapitemsNeedLine = scrapitems.Where(c => c.LineNumber <= 0).ToList();
            if (scrapitemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.QualityScrapCode) ? entity.QualityScrapCode : entity.Id.ToString();
                var maxLine = await _qualityScrapItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityScrapId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, scrapitemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in scrapitems)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < scrapitems.Count; i++)
                        {
                            var key = $"{scrapitems[i].CompanyCode}|{scrapitems[i].QualityScrapId}|{scrapitems[i].LineNumber}|{scrapitems[i].MaterialCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"品质废弃明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityScrapId、LineNumber、MaterialCode）");
                            }
                        }
            await _qualityScrapItemRepository.DeleteAsync(x => x.QualityScrapId == entity.Id);
            foreach (var child in scrapitems)
            {
            var isUnique_ix_takt_logistics_quality_scrap_item_line_number_unique = await _uniqueValidator.IsUniqueAsync(
                _qualityScrapItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.QualityScrapId == child.QualityScrapId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialCode == child.MaterialCode);
            if (!isUnique_ix_takt_logistics_quality_scrap_item_line_number_unique)
            {
                throw new TaktBusinessException("品质废弃明细的CompanyCode、QualityScrapId、LineNumber、MaterialCode已存在");
            }
            }
            await _qualityScrapItemRepository.CreateRangeAsync(scrapitems);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质废弃主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityScrap, bool>> QueryExpression(TaktQualityScrapQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityScrap>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityScrapCode != null && x.QualityScrapCode.Contains(keywords))
                || SqlFunc.ToString(x.IndirectManpowerCostPerMinute).Contains(keywords)
                || (x.Model != null && x.Model.Contains(keywords))
                || (x.ScrapReason != null && x.ScrapReason.Contains(keywords))
                || SqlFunc.ToString(x.TotalScrapQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalScrapCost).Contains(keywords)
                || (x.CostCurrency != null && x.CostCurrency.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScrapDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityScrapCode))
        {
            exp = exp.And(x => x.QualityScrapCode != null && x.QualityScrapCode.Contains(queryDto.QualityScrapCode));
        }

        if (queryDto?.IndirectManpowerCostPerMinute.HasValue == true)
        {
            exp = exp.And(x => x.IndirectManpowerCostPerMinute == queryDto.IndirectManpowerCostPerMinute);
        }

        if (!string.IsNullOrEmpty(queryDto?.Model))
        {
            exp = exp.And(x => x.Model != null && x.Model.Contains(queryDto.Model));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScrapReason))
        {
            exp = exp.And(x => x.ScrapReason != null && x.ScrapReason.Contains(queryDto.ScrapReason));
        }

        if (queryDto?.TotalScrapQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalScrapQuantity == queryDto.TotalScrapQuantity);
        }

        if (queryDto?.TotalScrapCost.HasValue == true)
        {
            exp = exp.And(x => x.TotalScrapCost == queryDto.TotalScrapCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCurrency))
        {
            exp = exp.And(x => x.CostCurrency != null && x.CostCurrency.Contains(queryDto.CostCurrency));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ScrapDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate >= queryDto.ScrapDateStart);
        }

        if (queryDto?.ScrapDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate <= queryDto.ScrapDateEnd);
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
