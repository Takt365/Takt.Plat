// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工步应用服务
/// </summary>
public class TaktSopStepService : TaktServiceBase, ITaktSopStepService
{
    private readonly ITaktCompanyRepository<TaktSopStep> _sopStepRepository;
    private readonly ITaktCompanyRepository<TaktSopStepMedia> _sopStepMediaRepository;
    private readonly ITaktCompanyRepository<TaktSopStepCheckItem> _sopStepCheckItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepRepository">SOP工步仓储</param>
    /// <param name="sopStepMediaRepository">SopStepMedia仓储</param>
    /// <param name="sopStepCheckItemRepository">SopStepCheckItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopStepService(
        ITaktCompanyRepository<TaktSopStep> sopStepRepository,
        ITaktCompanyRepository<TaktSopStepMedia> sopStepMediaRepository,
        ITaktCompanyRepository<TaktSopStepCheckItem> sopStepCheckItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopStepRepository = sopStepRepository;
        _sopStepMediaRepository = sopStepMediaRepository;
        _sopStepCheckItemRepository = sopStepCheckItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopStepDto>> GetSopStepListAsync(TaktSopStepQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopStepRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopStepDto>.Create(
            data.Adapt<List<TaktSopStepDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto?> GetSopStepByIdAsync(long id)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopStepDto>();
        await FillSopStepDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP工步选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopStepOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopStepRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.StepTitle ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.StepTitle ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto> CreateSopStepAsync(TaktSopStepCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopStep>();
        entity = await _sopStepRepository.CreateAsync(entity);
                await SaveSopStepChildrenAsync(entity, dto);
        return await GetSopStepByIdAsync(entity.Id) ?? entity.Adapt<TaktSopStepDto>();
    }

    /// <summary>
    /// 更新SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepDto> UpdateSopStepAsync(long id, TaktSopStepUpdateDto dto)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步不存在");
        }
        dto.Adapt(entity);
        await _sopStepRepository.UpdateAsync(entity);
                await SaveSopStepChildrenAsync(entity, dto);
        return await GetSopStepByIdAsync(id) ?? throw new TaktBusinessException("SOP工步不存在");
    }

    /// <summary>
    /// 删除SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepByIdAsync(long id)
    {
        var entity = await _sopStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步不存在或已删除");
        }
        await _sopStepMediaRepository.DeleteAsync(x => x.StepId == entity.Id);
        await _sopStepCheckItemRepository.DeleteAsync(x => x.StepId == entity.Id);
        var deleted = await _sopStepRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopStepByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopStepTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopStepTemplateDto>(
            sheetName ?? "SOP工步导入模板",
            fileName ?? "SOP工步导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopStepAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopStepImportDto>(fileStream, sheetName ?? "SOP工步导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopStep>();
                await _sopStepRepository.CreateAsync(entity);
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
    /// 导出SOP工步
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopStepAsync(TaktSopStepQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopStepQueryDto());
        var list = await _sopStepRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepExportDto>(),
                sheetName ?? "SOP工步数据",
                fileName ?? "SOP工步导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopStepExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步数据",
            fileName ?? "SOP工步导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP工步详情（加载 OneToMany 子表：SOP工步多媒体、SOP工步检验项目）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopStepDetailsAsync(TaktSopStepDto dto, TaktSopStep entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP工步多媒体 → dto.MediaList
        var medialist = await _sopStepMediaRepository.GetListAsync(x => x.StepId == entity.Id);
        dto.MediaList = medialist.Adapt<List<TaktSopStepMediaDto>>();
        // SOP工步检验项目 → dto.CheckItems
        var checkitems = await _sopStepCheckItemRepository.GetListAsync(x => x.StepId == entity.Id);
        dto.CheckItems = checkitems.Adapt<List<TaktSopStepCheckItemDto>>();
    }

    /// <summary>
    /// 保存SOP工步子表级联（SOP工步多媒体、SOP工步检验项目；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopStepChildrenAsync(TaktSopStep entity, TaktSopStepCreateDto dto)
    {
        // SOP工步多媒体（MediaList）
        if (dto.MediaList is not { Count: > 0 })
        {
            await _sopStepMediaRepository.DeleteAsync(x => x.StepId == entity.Id);
        }
        else
        {
            var medialist = dto.MediaList.Adapt<List<TaktSopStepMedia>>();
            foreach (var child in medialist)
            {
                child.StepId = entity.Id;
            }
            var medialistNeedSort = medialist.Where(c => c.SortOrder <= 0).ToList();
            if (medialistNeedSort.Count > 0)
            {
                var maxSort = await _sopStepMediaRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, medialistNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in medialist)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _sopStepMediaRepository.DeleteAsync(x => x.StepId == entity.Id);
            foreach (var child in medialist)
            {
            }
            await _sopStepMediaRepository.CreateRangeAsync(medialist);
        }
        // SOP工步检验项目（CheckItems）
        if (dto.CheckItems is not { Count: > 0 })
        {
            await _sopStepCheckItemRepository.DeleteAsync(x => x.StepId == entity.Id);
        }
        else
        {
            var checkitems = dto.CheckItems.Adapt<List<TaktSopStepCheckItem>>();
            foreach (var child in checkitems)
            {
                child.StepId = entity.Id;
            }
            var checkitemsNeedSort = checkitems.Where(c => c.SortOrder <= 0).ToList();
            if (checkitemsNeedSort.Count > 0)
            {
                var maxSort = await _sopStepCheckItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, checkitemsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in checkitems)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _sopStepCheckItemRepository.DeleteAsync(x => x.StepId == entity.Id);
            foreach (var child in checkitems)
            {
            }
            await _sopStepCheckItemRepository.CreateRangeAsync(checkitems);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工步查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopStep, bool>> QueryExpression(TaktSopStepQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopStep>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ContentId).Contains(keywords)
                || SqlFunc.ToString(x.StepNo).Contains(keywords)
                || (x.StepTitle != null && x.StepTitle.Contains(keywords))
                || (x.StepDescription != null && x.StepDescription.Contains(keywords))
                || (x.SafetyAlert != null && x.SafetyAlert.Contains(keywords))
                || SqlFunc.ToString(x.SafetyPopupRequired).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ContentId.HasValue == true)
        {
            exp = exp.And(x => x.ContentId == queryDto.ContentId);
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            exp = exp.And(x => x.StepNo == queryDto.StepNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.StepTitle))
        {
            exp = exp.And(x => x.StepTitle != null && x.StepTitle.Contains(queryDto.StepTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.StepDescription))
        {
            exp = exp.And(x => x.StepDescription != null && x.StepDescription.Contains(queryDto.StepDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.SafetyAlert))
        {
            exp = exp.And(x => x.SafetyAlert != null && x.SafetyAlert.Contains(queryDto.SafetyAlert));
        }

        if (queryDto?.SafetyPopupRequired.HasValue == true)
        {
            exp = exp.And(x => x.SafetyPopupRequired == queryDto.SafetyPopupRequired);
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
