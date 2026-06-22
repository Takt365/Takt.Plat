// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktDictTypeService.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型应用服务实现
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
/// 字典类型应用服务
/// </summary>
public class TaktDictTypeService : TaktServiceBase, ITaktDictTypeService
{
    private readonly ITaktTenantRepository<TaktDictType> _dictTypeRepository;
    private readonly ITaktTenantRepository<TaktDictData> _dictDataRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictTypeRepository">字典类型仓储</param>
    /// <param name="dictDataRepository">DictData仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDictTypeService(
        ITaktTenantRepository<TaktDictType> dictTypeRepository,
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取字典类型列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDictTypeDto>> GetDictTypeListAsync(TaktDictTypeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _dictTypeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDictTypeDto>.Create(
            data.Adapt<List<TaktDictTypeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto?> GetDictTypeByIdAsync(long id)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDictTypeDto>();
        await FillDictTypeDetailsAsync(dto, entity);
        return dto;
    }

    /// <summary>
    /// 获取字典类型选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDictTypeOptionsAsync()
    {
        var list = await _dictTypeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.DictTypeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DictTypeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建字典类型
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> CreateDictTypeAsync(TaktDictTypeCreateDto dto)
    {
        var entity = dto.Adapt<TaktDictType>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
            _dictTypeRepository,
            x => x.DictTypeCode == entity.DictTypeCode);
        if (!isUnique_ix_dict_type_code_unique)
        {
            throw new TaktBusinessException("字典类型的DictTypeCode已存在");
        }
        entity = await _dictTypeRepository.CreateAsync(entity);
        await SaveDictTypeChildrenAsync(entity, dto);
        return await GetDictTypeByIdAsync(entity.Id) ?? entity.Adapt<TaktDictTypeDto>();
    }

    /// <summary>
    /// 更新字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeAsync(long id, TaktDictTypeUpdateDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
            _dictTypeRepository,
            x => x.DictTypeCode == entity.DictTypeCode,
            id);
        if (!isUnique_ix_dict_type_code_unique)
        {
            throw new TaktBusinessException("字典类型的DictTypeCode已存在");
        }
        await _dictTypeRepository.UpdateAsync(entity);
        await SaveDictTypeChildrenAsync(entity, dto);
        return await GetDictTypeByIdAsync(id) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDictTypeByIdAsync(long id)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置字典类型不允许删除");
        }
        await _dictDataRepository.DeleteAsync(x => x.DictTypeId == entity.Id);
        var deleted = await _dictTypeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("字典类型不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDictTypeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _dictTypeRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置字典类型不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteDictTypeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新字典类型状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeStatusAsync(TaktDictTypeStatusDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.DictStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置字典类型");
        }
        entity.DictStatus = dto.DictStatus;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 更新字典类型是否内置
    /// </summary>
    /// <param name="dto">是否内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeBuiltInAsync(TaktDictTypeBuiltInDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("是否内置必须为字典 sys_yes_no_type 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置字典类型标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 更新字典类型排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeSortAsync(TaktDictTypeSortDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDictTypeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDictTypeTemplateDto>(
            sheetName ?? "字典类型导入模板",
            fileName ?? "字典类型导入模板.xlsx");
    }

    /// <summary>
    /// 导入字典类型
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDictTypeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDictTypeImportDto>(fileStream, sheetName ?? "字典类型导入模板");
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
                var entity = rows[i].Adapt<TaktDictType>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.DictTypeCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DictTypeCode）");
                }
                var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _dictTypeRepository,
                    x => x.DictTypeCode == entity.DictTypeCode);
                if (!isUnique_ix_dict_type_code_unique)
                {
                    throw new TaktBusinessException("字典类型的DictTypeCode已存在");
                }
                await _dictTypeRepository.CreateAsync(entity);
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
    /// 导出字典类型
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDictTypeAsync(TaktDictTypeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDictTypeQueryDto());
        var list = await _dictTypeRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDictTypeExportDto>(),
                sheetName ?? "字典类型数据",
                fileName ?? "字典类型导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDictTypeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "字典类型数据",
            fileName ?? "字典类型导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充字典类型详情（加载 OneToMany 子表：字典数据）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillDictTypeDetailsAsync(TaktDictTypeDto dto, TaktDictType entity)
    {
        if (dto == null)
        {
            return;
        }
        // 字典数据 → dto.DictDataList
        var dictdatalist = await _dictDataRepository.GetListAsync(x => x.DictTypeId == entity.Id);
        dto.DictDataList = dictdatalist.Adapt<List<TaktDictDataDto>>();
    }

    /// <summary>
    /// 保存字典类型子表级联（字典数据；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveDictTypeChildrenAsync(TaktDictType entity, TaktDictTypeCreateDto dto)
    {
        if (dto.DictDataList is not { Count: > 0 })
        {
            await _dictDataRepository.DeleteAsync(x => x.DictTypeId == entity.Id);
            return;
        }
        var dictDataList = dto.DictDataList.Adapt<List<TaktDictData>>();
        foreach (var child in dictDataList)
        {
            child.DictTypeId = entity.Id;
            child.DictTypeCode = entity.DictTypeCode;
        }
        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < dictDataList.Count; i++)
        {
            var key = $"{dictDataList[i].DictTypeId}|{dictDataList[i].DictLabel}|{dictDataList[i].I18nKey}";
            if (!seenKeys.Add(key))
            {
                throw new TaktBusinessException($"字典数据第{i + 1}项与本次提交的其他项重复（DictTypeId、DictLabel、I18nKey）");
            }
        }
        await _dictDataRepository.DeleteAsync(x => x.DictTypeId == entity.Id);
        foreach (var child in dictDataList)
        {
            var isUnique_ix_dict_data_type_label_i18n_unique = await _uniqueValidator.IsUniqueAsync(
                _dictDataRepository,
                x => x.DictTypeId == child.DictTypeId
                    && x.DictLabel == child.DictLabel
                    && x.I18nKey == child.I18nKey);
            if (!isUnique_ix_dict_data_type_label_i18n_unique)
            {
                throw new TaktBusinessException("字典数据的DictTypeId、DictLabel、I18nKey已存在");
            }
        }
        await _dictDataRepository.CreateRangeAsync(dictDataList);
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建字典类型查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDictType, bool>> QueryExpression(TaktDictTypeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDictType>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.DictTypeCode != null && x.DictTypeCode.Contains(keywords))
                || (x.DictTypeName != null && x.DictTypeName.Contains(keywords))
                || SqlFunc.ToString(x.DataSource).Contains(keywords)
                || (x.DictScript != null && x.DictScript.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.DictStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.DictTypeCode))
        {
            exp = exp.And(x => x.DictTypeCode != null && x.DictTypeCode.Contains(queryDto.DictTypeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DictTypeName))
        {
            exp = exp.And(x => x.DictTypeName != null && x.DictTypeName.Contains(queryDto.DictTypeName));
        }

        if (queryDto?.DataSource.HasValue == true)
        {
            exp = exp.And(x => x.DataSource == queryDto.DataSource);
        }

        if (!string.IsNullOrEmpty(queryDto?.DictScript))
        {
            exp = exp.And(x => x.DictScript != null && x.DictScript.Contains(queryDto.DictScript));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.DictStatus.HasValue == true)
        {
            exp = exp.And(x => x.DictStatus == queryDto.DictStatus);
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
