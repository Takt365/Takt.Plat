// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktSettingService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：系统设置应用服务实现
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
/// 系统设置应用服务
/// </summary>
public class TaktSettingService : TaktServiceBase, ITaktSettingService
{
    private readonly ITaktCompanyRepository<TaktSetting> _settingRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="settingRepository">系统设置仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSettingService(
        ITaktCompanyRepository<TaktSetting> settingRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _settingRepository = settingRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取系统设置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSettingDto>> GetSettingListAsync(TaktSettingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _settingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSettingDto>.Create(
            data.Adapt<List<TaktSettingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSettingDto?> GetSettingByIdAsync(long id)
    {
        var entity = await _settingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSettingDto>();
    }

    /// <summary>
    /// 获取系统设置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSettingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _settingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SettingName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SettingName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建系统设置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSettingDto> CreateSettingAsync(TaktSettingCreateDto dto)
    {
        var entity = dto.Adapt<TaktSetting>();
        entity.IsBuiltIn = TaktYesNo.No;
        var isUnique_ix_setting_key_unique = await _uniqueValidator.IsUniqueAsync(
            _settingRepository,
            x => x.SettingKey == entity.SettingKey);
        if (!isUnique_ix_setting_key_unique)
        {
            throw new TaktBusinessException("系统设置的SettingKey已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _settingRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _settingRepository.CreateAsync(entity);
        return await GetSettingByIdAsync(entity.Id) ?? entity.Adapt<TaktSettingDto>();
    }

    /// <summary>
    /// 更新系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSettingDto> UpdateSettingAsync(long id, TaktSettingUpdateDto dto)
    {
        var entity = await _settingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("系统设置不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_setting_key_unique = await _uniqueValidator.IsUniqueAsync(
            _settingRepository,
            x => x.SettingKey == entity.SettingKey,
            id);
        if (!isUnique_ix_setting_key_unique)
        {
            throw new TaktBusinessException("系统设置的SettingKey已存在");
        }
        await _settingRepository.UpdateAsync(entity);
        return await GetSettingByIdAsync(id) ?? throw new TaktBusinessException("系统设置不存在");
    }

    /// <summary>
    /// 删除系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSettingByIdAsync(long id)
    {
        var entity = await _settingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("系统设置不存在或已删除");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置系统设置不允许删除");
        }
        var deleted = await _settingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("系统设置不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除系统设置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSettingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _settingRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置系统设置不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteSettingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新系统设置排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSettingDto> UpdateSettingSortAsync(TaktSettingSortDto dto)
    {
        var entity = await _settingRepository.GetByIdAsync(dto.SettingId);
        if (entity == null)
        {
            throw new TaktBusinessException("系统设置不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _settingRepository.UpdateAsync(entity);
        return await GetSettingByIdAsync(dto.SettingId) ?? throw new TaktBusinessException("系统设置不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSettingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSettingTemplateDto>(
            sheetName ?? "系统设置导入模板",
            fileName ?? "系统设置导入模板.xlsx");
    }

    /// <summary>
    /// 导入系统设置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSettingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSettingImportDto>(fileStream, sheetName ?? "系统设置导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _settingRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSetting>();
                entity.IsBuiltIn = TaktYesNo.No;
                var importKey = $"{entity.SettingKey}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SettingKey）");
                }
                var isUnique_ix_setting_key_unique = await _uniqueValidator.IsUniqueAsync(
                    _settingRepository,
                    x => x.SettingKey == entity.SettingKey);
                if (!isUnique_ix_setting_key_unique)
                {
                    throw new TaktBusinessException("系统设置的SettingKey已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _settingRepository.CreateAsync(entity);
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
    /// 导出系统设置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSettingAsync(TaktSettingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSettingQueryDto());
        var list = await _settingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSettingExportDto>(),
                sheetName ?? "系统设置数据",
                fileName ?? "系统设置导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSettingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "系统设置数据",
            fileName ?? "系统设置导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建系统设置查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSetting, bool>> QueryExpression(TaktSettingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSetting>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.SettingKey != null && x.SettingKey.Contains(keywords))
                || (x.SettingValue != null && x.SettingValue.Contains(keywords))
                || (x.SettingName != null && x.SettingName.Contains(keywords))
                || (x.Description != null && x.Description.Contains(keywords))
                || SqlFunc.ToString(x.SettingGroup).Contains(keywords)
                || SqlFunc.ToString(x.ValueType).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.IsReadonly).Contains(keywords)
                || SqlFunc.ToString(x.IsEncrypted).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.SettingKey))
        {
            exp = exp.And(x => x.SettingKey != null && x.SettingKey.Contains(queryDto.SettingKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.SettingValue))
        {
            exp = exp.And(x => x.SettingValue != null && x.SettingValue.Contains(queryDto.SettingValue));
        }

        if (!string.IsNullOrEmpty(queryDto?.SettingName))
        {
            exp = exp.And(x => x.SettingName != null && x.SettingName.Contains(queryDto.SettingName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (queryDto?.SettingGroup.HasValue == true)
        {
            exp = exp.And(x => x.SettingGroup == queryDto.SettingGroup);
        }

        if (queryDto?.ValueType.HasValue == true)
        {
            exp = exp.And(x => x.ValueType == queryDto.ValueType);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.IsReadonly.HasValue == true)
        {
            exp = exp.And(x => x.IsReadonly == queryDto.IsReadonly);
        }

        if (queryDto?.IsEncrypted.HasValue == true)
        {
            exp = exp.And(x => x.IsEncrypted == queryDto.IsEncrypted);
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
