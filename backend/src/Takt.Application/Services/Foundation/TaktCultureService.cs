// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktCultureService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：区域应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 区域应用服务
/// </summary>
public class TaktCultureService : TaktServiceBase, ITaktCultureService
{
    private readonly ITaktTenantRepository<TaktCulture> _cultureRepository;
    private readonly ITaktTenantRepository<TaktTranslation> _translationRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cultureRepository">区域仓储</param>
    /// <param name="translationRepository">Translation仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCultureService(
        ITaktTenantRepository<TaktCulture> cultureRepository,
        ITaktTenantRepository<TaktTranslation> translationRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cultureRepository = cultureRepository;
        _translationRepository = translationRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取区域列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCultureDto>> GetCultureListAsync(TaktCultureQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _cultureRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCultureDto>.Create(
            data.Adapt<List<TaktCultureDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto?> GetCultureByIdAsync(long id)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCultureDto>();
        await FillCultureDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取语言切换选项列表（仅启用；仓储按 SortOrder 升序，不向下拉项回填 SortOrder）
    /// </summary>
    /// <returns>
    /// TaktSelectOption：DictValue=CultureCode，DictLabel=LanguageName，ExtValue=Icon，ExtLabel=IsDefault（1/0）
    /// </returns>
    public async Task<List<TaktSelectOption>> GetCultureOptionsAsync()
    {
        var list = await _cultureRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.LanguageStatus == TaktCommonStatus.Enabled,
            x => x.SortOrder,
            false);

        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CultureCode,
            DictLabel = e.LanguageName,
            ExtValue = e.Icon,
            ExtLabel = ((int)e.IsDefault).ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建区域
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> CreateCultureAsync(TaktCultureCreateDto dto)
    {
        var entity = dto.Adapt<TaktCulture>();
        var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _cultureRepository,
            x => x.CultureCode == entity.CultureCode);
        if (!isUnique_ix_culture_culture_unique)
        {
            throw new TaktBusinessException("区域的CultureCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _cultureRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _cultureRepository.CreateAsync(entity);
                await SaveCultureChildrenAsync(entity, dto);
        return await GetCultureByIdAsync(entity.Id) ?? entity.Adapt<TaktCultureDto>();
    }

    /// <summary>
    /// 更新区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> UpdateCultureAsync(long id, TaktCultureUpdateDto dto)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _cultureRepository,
            x => x.CultureCode == entity.CultureCode,
            id);
        if (!isUnique_ix_culture_culture_unique)
        {
            throw new TaktBusinessException("区域的CultureCode已存在");
        }
        await _cultureRepository.UpdateAsync(entity);
                await SaveCultureChildrenAsync(entity, dto);
        return await GetCultureByIdAsync(id) ?? throw new TaktBusinessException("区域不存在");
    }

    /// <summary>
    /// 删除区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCultureByIdAsync(long id)
    {
        var entity = await _cultureRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在或已删除");
        }
        await _translationRepository.DeleteAsync(x => x.CultureId == entity.Id);
        var deleted = await _cultureRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("区域不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除区域
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCultureBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCultureByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新区域状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> UpdateCultureStatusAsync(TaktCultureStatusDto dto)
    {
        var entity = await _cultureRepository.GetByIdAsync(dto.CultureId);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        entity.LanguageStatus = dto.LanguageStatus;
        await _cultureRepository.UpdateAsync(entity);
        return await GetCultureByIdAsync(dto.CultureId) ?? throw new TaktBusinessException("区域不存在");
    }

    /// <summary>
    /// 更新区域排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCultureDto> UpdateCultureSortAsync(TaktCultureSortDto dto)
    {
        var entity = await _cultureRepository.GetByIdAsync(dto.CultureId);
        if (entity == null)
        {
            throw new TaktBusinessException("区域不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _cultureRepository.UpdateAsync(entity);
        return await GetCultureByIdAsync(dto.CultureId) ?? throw new TaktBusinessException("区域不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCultureTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCultureTemplateDto>(
            sheetName ?? "区域导入模板",
            fileName ?? "区域导入模板.xlsx");
    }

    /// <summary>
    /// 导入区域
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCultureAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCultureImportDto>(fileStream, sheetName ?? "区域导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _cultureRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCulture>();
                var importKey = $"{entity.CultureCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CultureCode）");
                }
                var isUnique_ix_culture_culture_unique = await _uniqueValidator.IsUniqueAsync(
                    _cultureRepository,
                    x => x.CultureCode == entity.CultureCode);
                if (!isUnique_ix_culture_culture_unique)
                {
                    throw new TaktBusinessException("区域的CultureCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _cultureRepository.CreateAsync(entity);
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
    /// 导出区域
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCultureAsync(TaktCultureQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCultureQueryDto());
        var list = await _cultureRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCultureExportDto>(),
                sheetName ?? "区域数据",
                fileName ?? "区域导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCultureExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "区域数据",
            fileName ?? "区域导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充区域详情（加载 OneToMany 子表：翻译）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCultureDetailsAsync(TaktCultureDto dto, TaktCulture entity)
    {
        if (dto == null)
        {
            return;
        }
        // 翻译 → dto.TranslationList
        var translationlist = await _translationRepository.GetListAsync(x => x.CultureId == entity.Id);
        dto.TranslationList = translationlist.Adapt<List<TaktTranslationDto>>();
    }

    /// <summary>
    /// 保存区域子表级联（翻译；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCultureChildrenAsync(TaktCulture entity, TaktCultureCreateDto dto)
    {
        // 翻译（TranslationList）
        if (dto.TranslationList is not { Count: > 0 })
        {
            await _translationRepository.DeleteAsync(x => x.CultureId == entity.Id);
        }
        else
        {
            var translationlist = dto.TranslationList.Adapt<List<TaktTranslation>>();
            foreach (var child in translationlist)
            {
                child.CultureId = entity.Id;
                child.CultureCode = entity.CultureCode;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < translationlist.Count; i++)
                        {
                            var key = $"{translationlist[i].I18nKey}|{translationlist[i].CultureCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"翻译第{i + 1}项与本次提交的其他项重复（I18nKey、CultureCode）");
                            }
                        }
            await _translationRepository.DeleteAsync(x => x.CultureId == entity.Id);
            foreach (var child in translationlist)
            {
            var isUnique_ix_translation_key_culture_unique = await _uniqueValidator.IsUniqueAsync(
                _translationRepository,
                x => x.I18nKey == child.I18nKey
                    && x.CultureCode == child.CultureCode);
            if (!isUnique_ix_translation_key_culture_unique)
            {
                throw new TaktBusinessException("翻译的I18nKey、CultureCode已存在");
            }
            }
            await _translationRepository.CreateRangeAsync(translationlist);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建区域查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCulture, bool>> QueryExpression(TaktCultureQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCulture>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.LanguageName != null && x.LanguageName.Contains(keywords))
                || (x.NativeName != null && x.NativeName.Contains(keywords))
                || (x.Icon != null && x.Icon.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsDefault).Contains(keywords)
                || SqlFunc.ToString(x.LanguageStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.LanguageName))
        {
            exp = exp.And(x => x.LanguageName != null && x.LanguageName.Contains(queryDto.LanguageName));
        }

        if (!string.IsNullOrEmpty(queryDto?.NativeName))
        {
            exp = exp.And(x => x.NativeName != null && x.NativeName.Contains(queryDto.NativeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Icon))
        {
            exp = exp.And(x => x.Icon != null && x.Icon.Contains(queryDto.Icon));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsDefault.HasValue == true)
        {
            exp = exp.And(x => x.IsDefault == queryDto.IsDefault);
        }

        if (queryDto?.LanguageStatus.HasValue == true)
        {
            exp = exp.And(x => x.LanguageStatus == queryDto.LanguageStatus);
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
