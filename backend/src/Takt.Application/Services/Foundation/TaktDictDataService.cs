// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktDictDataService.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据应用服务实现
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

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 字典数据应用服务
/// </summary>
public class TaktDictDataService : TaktServiceBase, ITaktDictDataService
{
    private readonly ITaktTenantRepository<TaktDictData> _dictDataRepository;
    private readonly ITaktTenantRepository<TaktDictType> _dictTypeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictDataRepository">字典数据仓储</param>
    /// <param name="dictTypeRepository">字典类型仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDictDataService(
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        ITaktTenantRepository<TaktDictType> dictTypeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _dictDataRepository = dictDataRepository;
        _dictTypeRepository = dictTypeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取字典数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDictDataDto>> GetDictDataListAsync(TaktDictDataQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _dictDataRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDictDataDto>.Create(
            data.Adapt<List<TaktDictDataDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictDataDto?> GetDictDataByIdAsync(long id)
    {
        var entity = await _dictDataRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktDictDataDto>();
    }

    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDictDataOptionsAsync()
    {
        var list = await _dictDataRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.DictTypeCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DictTypeCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建字典数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictDataDto> CreateDictDataAsync(TaktDictDataCreateDto dto)
    {
        var entity = dto.Adapt<TaktDictData>();
        await StampDictDataDictTypeAsync(entity, dto);
        var isUnique_ix_dict_data_type_label_i18n_unique = await _uniqueValidator.IsUniqueAsync(
            _dictDataRepository,
            x => x.DictTypeId == entity.DictTypeId
                && x.DictLabel == entity.DictLabel
                && x.I18nKey == entity.I18nKey);
        if (!isUnique_ix_dict_data_type_label_i18n_unique)
        {
            throw new TaktBusinessException("字典数据的DictTypeId、DictLabel、I18nKey已存在");
        }
        entity = await _dictDataRepository.CreateAsync(entity);
        return await GetDictDataByIdAsync(entity.Id) ?? entity.Adapt<TaktDictDataDto>();
    }

    /// <summary>
    /// 更新字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictDataDto> UpdateDictDataAsync(long id, TaktDictDataUpdateDto dto)
    {
        var entity = await _dictDataRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("字典数据不存在");
        }
        dto.Adapt(entity);
        await StampDictDataDictTypeAsync(entity, dto);
        var isUnique_ix_dict_data_type_label_i18n_unique = await _uniqueValidator.IsUniqueAsync(
            _dictDataRepository,
            x => x.DictTypeId == entity.DictTypeId
                && x.DictLabel == entity.DictLabel
                && x.I18nKey == entity.I18nKey,
            id);
        if (!isUnique_ix_dict_data_type_label_i18n_unique)
        {
            throw new TaktBusinessException("字典数据的DictTypeId、DictLabel、I18nKey已存在");
        }
        await _dictDataRepository.UpdateAsync(entity);
        return await GetDictDataByIdAsync(id) ?? throw new TaktBusinessException("字典数据不存在");
    }

    /// <summary>
    /// 删除字典数据
    /// </summary>
    /// <param name="id">字典数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDictDataByIdAsync(long id)
    {
        var deleted = await _dictDataRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("字典数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除字典数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDictDataBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDictDataByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新字典数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictDataDto> UpdateDictDataSortAsync(TaktDictDataSortDto dto)
    {
        var entity = await _dictDataRepository.GetByIdAsync(dto.DictDataId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _dictDataRepository.UpdateAsync(entity);
        return await GetDictDataByIdAsync(dto.DictDataId) ?? throw new TaktBusinessException("字典数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDictDataTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDictDataTemplateDto>(
            sheetName ?? "字典数据导入模板",
            fileName ?? "字典数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入字典数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDictDataAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDictDataImportDto>(fileStream, sheetName ?? "字典数据导入模板");
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
                var row = rows[i];
                var entity = row.Adapt<TaktDictData>();
                await ResolveDictDataDictTypeAsync(entity, row.DictTypeId ?? 0, row.DictTypeCode);
                var importKey = $"{entity.DictTypeId}|{entity.DictLabel}|{entity.I18nKey}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DictTypeId、DictLabel、I18nKey）");
                }
                var isUnique_ix_dict_data_type_label_i18n_unique = await _uniqueValidator.IsUniqueAsync(
                    _dictDataRepository,
                    x => x.DictTypeId == entity.DictTypeId
                        && x.DictLabel == entity.DictLabel
                        && x.I18nKey == entity.I18nKey);
                if (!isUnique_ix_dict_data_type_label_i18n_unique)
                {
                    throw new TaktBusinessException("字典数据的DictTypeId、DictLabel、I18nKey已存在");
                }
                await _dictDataRepository.CreateAsync(entity);
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
    /// 导出字典数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDictDataAsync(TaktDictDataQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDictDataQueryDto());
        var list = await _dictDataRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDictDataExportDto>(),
                sheetName ?? "字典数据数据",
                fileName ?? "字典数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDictDataExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "字典数据数据",
            fileName ?? "字典数据导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步字典数据主表外键（ManyToOne → 字典类型）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建/更新 DTO</param>
    /// <returns>任务</returns>
    private Task StampDictDataDictTypeAsync(TaktDictData entity, TaktDictDataCreateDto dto)
    {
        return ResolveDictDataDictTypeAsync(entity, dto.DictTypeId, dto.DictTypeCode);
    }

    /// <summary>
    /// 按字典类型 ID 或编码解析并写入 DictTypeId、DictTypeCode
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dictTypeId">字典类型 ID</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <returns>任务</returns>
    private async Task ResolveDictDataDictTypeAsync(TaktDictData entity, long dictTypeId, string? dictTypeCode)
    {
        TaktDictType? master = null;
        if (dictTypeId > 0)
        {
            master = await _dictTypeRepository.GetByIdAsync(dictTypeId);
            if (master != null && master.TenantCode != CurrentTenantCode)
            {
                master = null;
            }
        }
        if (master == null && !string.IsNullOrEmpty(dictTypeCode))
        {
            master = await _dictTypeRepository.FirstAsync(x => x.DictTypeCode == dictTypeCode && x.TenantCode == CurrentTenantCode);
        }
        if (master == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        entity.DictTypeId = master.Id;
        entity.DictTypeCode = master.DictTypeCode;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建字典数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDictData, bool>> QueryExpression(TaktDictDataQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDictData>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.DictTypeId).Contains(keywords)
                || (x.DictTypeCode != null && x.DictTypeCode.Contains(keywords))
                || (x.DictLabel != null && x.DictLabel.Contains(keywords))
                || (x.DictValue != null && x.DictValue.Contains(keywords))
                || (x.I18nKey != null && x.I18nKey.Contains(keywords))
                || (x.ExtLabel != null && x.ExtLabel.Contains(keywords))
                || (x.ExtValue != null && x.ExtValue.Contains(keywords))
                || SqlFunc.ToString(x.ListClass).Contains(keywords)
                || SqlFunc.ToString(x.CssClass).Contains(keywords)
                || SqlFunc.ToString(x.IsDefault).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.DictTypeId.HasValue == true)
        {
            exp = exp.And(x => x.DictTypeId == queryDto.DictTypeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DictTypeCode))
        {
            exp = exp.And(x => x.DictTypeCode != null && x.DictTypeCode.Contains(queryDto.DictTypeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DictLabel))
        {
            exp = exp.And(x => x.DictLabel != null && x.DictLabel.Contains(queryDto.DictLabel));
        }

        if (!string.IsNullOrEmpty(queryDto?.DictValue))
        {
            exp = exp.And(x => x.DictValue != null && x.DictValue.Contains(queryDto.DictValue));
        }

        if (!string.IsNullOrEmpty(queryDto?.I18nKey))
        {
            exp = exp.And(x => x.I18nKey != null && x.I18nKey.Contains(queryDto.I18nKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtLabel))
        {
            exp = exp.And(x => x.ExtLabel != null && x.ExtLabel.Contains(queryDto.ExtLabel));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtValue))
        {
            exp = exp.And(x => x.ExtValue != null && x.ExtValue.Contains(queryDto.ExtValue));
        }

        if (queryDto?.ListClass.HasValue == true)
        {
            exp = exp.And(x => x.ListClass == queryDto.ListClass);
        }

        if (queryDto?.CssClass.HasValue == true)
        {
            exp = exp.And(x => x.CssClass == queryDto.CssClass);
        }

        if (queryDto?.IsDefault.HasValue == true)
        {
            exp = exp.And(x => x.IsDefault == queryDto.IsDefault);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
