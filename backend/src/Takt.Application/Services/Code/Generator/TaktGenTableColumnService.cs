// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Code.Generator
// 文件名称：TaktGenTableColumnService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成数据表列配置应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Code.Generator;
using Takt.Domain.Entities.Code.Generator;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Generator;

/// <summary>
/// 代码生成数据表列配置应用服务
/// </summary>
public class TaktGenTableColumnService : TaktServiceBase, ITaktGenTableColumnService
{
    private readonly ITaktTenantRepository<TaktGenTableColumn> _genTableColumnRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="genTableColumnRepository">代码生成数据表列配置仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGenTableColumnService(
        ITaktTenantRepository<TaktGenTableColumn> genTableColumnRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _genTableColumnRepository = genTableColumnRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取代码生成数据表列配置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGenTableColumnDto>> GetGenTableColumnListAsync(TaktGenTableColumnQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _genTableColumnRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGenTableColumnDto>.Create(
            data.Adapt<List<TaktGenTableColumnDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto?> GetGenTableColumnByIdAsync(long id)
    {
        var entity = await _genTableColumnRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktGenTableColumnDto>();
    }

    /// <summary>
    /// 获取代码生成字段配置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGenTableColumnOptionsAsync()
    {
        var list = await _genTableColumnRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.DatabaseColumnName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DatabaseColumnName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建代码生成数据表列配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto> CreateGenTableColumnAsync(TaktGenTableColumnCreateDto dto)
    {
        var entity = dto.Adapt<TaktGenTableColumn>();
        var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableColumnRepository,
            x => x.GenTableId == entity.GenTableId
                && x.DatabaseColumnName == entity.DatabaseColumnName);
        if (!isUnique_ix_gen_table_column_column_unique)
        {
            throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _genTableColumnRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.GenTableId, maxSort);
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                x => x.LineNumber);
            var businessCode = entity.GenTableId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _genTableColumnRepository.CreateAsync(entity);
        return await GetGenTableColumnByIdAsync(entity.Id) ?? entity.Adapt<TaktGenTableColumnDto>();
    }

    /// <summary>
    /// 更新代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto> UpdateGenTableColumnAsync(long id, TaktGenTableColumnUpdateDto dto)
    {
        var entity = await _genTableColumnRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表列配置不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableColumnRepository,
            x => x.GenTableId == entity.GenTableId
                && x.DatabaseColumnName == entity.DatabaseColumnName,
            id);
        if (!isUnique_ix_gen_table_column_column_unique)
        {
            throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
        }
        await _genTableColumnRepository.UpdateAsync(entity);
        return await GetGenTableColumnByIdAsync(id) ?? throw new TaktBusinessException("代码生成数据表列配置不存在");
    }

    /// <summary>
    /// 删除代码生成数据表列配置
    /// </summary>
    /// <param name="id">代码生成数据表列配置ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableColumnByIdAsync(long id)
    {
        var deleted = await _genTableColumnRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("代码生成数据表列配置不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除代码生成数据表列配置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableColumnBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGenTableColumnByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新代码生成数据表列配置排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableColumnDto> UpdateGenTableColumnSortAsync(TaktGenTableColumnSortDto dto)
    {
        var entity = await _genTableColumnRepository.GetByIdAsync(dto.GenTableColumnId);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表列配置不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _genTableColumnRepository.UpdateAsync(entity);
        return await GetGenTableColumnByIdAsync(dto.GenTableColumnId) ?? throw new TaktBusinessException("代码生成数据表列配置不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGenTableColumnTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGenTableColumnTemplateDto>(
            sheetName ?? "代码生成数据表列配置导入模板",
            fileName ?? "代码生成数据表列配置导入模板.xlsx");
    }

    /// <summary>
    /// 导入代码生成数据表列配置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGenTableColumnAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGenTableColumnImportDto>(fileStream, sheetName ?? "代码生成数据表列配置导入模板");
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
                var entity = rows[i].Adapt<TaktGenTableColumn>();
                var importKey = $"{entity.GenTableId}|{entity.DatabaseColumnName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（GenTableId、DatabaseColumnName）");
                }
                var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
                    _genTableColumnRepository,
                    x => x.GenTableId == entity.GenTableId
                        && x.DatabaseColumnName == entity.DatabaseColumnName);
                if (!isUnique_ix_gen_table_column_column_unique)
                {
                    throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _genTableColumnRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.GenTableId, maxSort);
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.GenTableId,
                        x => x.LineNumber);
                    var businessCode = entity.GenTableId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _genTableColumnRepository.CreateAsync(entity);
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
    /// 导出代码生成数据表列配置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGenTableColumnAsync(TaktGenTableColumnQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktGenTableColumnQueryDto());
        var list = await _genTableColumnRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableColumnExportDto>(),
                sheetName ?? "代码生成数据表列配置数据",
                fileName ?? "代码生成数据表列配置导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGenTableColumnExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "代码生成数据表列配置数据",
            fileName ?? "代码生成数据表列配置导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建代码生成数据表列配置查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGenTableColumn, bool>> QueryExpression(TaktGenTableColumnQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGenTableColumn>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.GenTableId).Contains(keywords)
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DatabaseColumnName != null && x.DatabaseColumnName.Contains(keywords))
                || (x.ColumnComment != null && x.ColumnComment.Contains(keywords))
                || (x.DatabaseDataType != null && x.DatabaseDataType.Contains(keywords))
                || (x.CsharpDataType != null && x.CsharpDataType.Contains(keywords))
                || (x.CsharpColumnName != null && x.CsharpColumnName.Contains(keywords))
                || SqlFunc.ToString(x.Length).Contains(keywords)
                || SqlFunc.ToString(x.DecimalDigits).Contains(keywords)
                || SqlFunc.ToString(x.IsPk).Contains(keywords)
                || SqlFunc.ToString(x.IsIncrement).Contains(keywords)
                || SqlFunc.ToString(x.IsRequired).Contains(keywords)
                || SqlFunc.ToString(x.IsCreate).Contains(keywords)
                || SqlFunc.ToString(x.IsUpdate).Contains(keywords)
                || SqlFunc.ToString(x.IsUnique).Contains(keywords)
                || SqlFunc.ToString(x.IsList).Contains(keywords)
                || SqlFunc.ToString(x.IsExport).Contains(keywords)
                || SqlFunc.ToString(x.IsSort).Contains(keywords)
                || SqlFunc.ToString(x.IsQuery).Contains(keywords)
                || (x.QueryType != null && x.QueryType.Contains(keywords))
                || (x.HtmlType != null && x.HtmlType.Contains(keywords))
                || (x.DictType != null && x.DictType.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.GenTableId.HasValue == true)
        {
            exp = exp.And(x => x.GenTableId == queryDto.GenTableId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.DatabaseColumnName))
        {
            exp = exp.And(x => x.DatabaseColumnName != null && x.DatabaseColumnName.Contains(queryDto.DatabaseColumnName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ColumnComment))
        {
            exp = exp.And(x => x.ColumnComment != null && x.ColumnComment.Contains(queryDto.ColumnComment));
        }

        if (!string.IsNullOrEmpty(queryDto?.DatabaseDataType))
        {
            exp = exp.And(x => x.DatabaseDataType != null && x.DatabaseDataType.Contains(queryDto.DatabaseDataType));
        }

        if (!string.IsNullOrEmpty(queryDto?.CsharpDataType))
        {
            exp = exp.And(x => x.CsharpDataType != null && x.CsharpDataType.Contains(queryDto.CsharpDataType));
        }

        if (!string.IsNullOrEmpty(queryDto?.CsharpColumnName))
        {
            exp = exp.And(x => x.CsharpColumnName != null && x.CsharpColumnName.Contains(queryDto.CsharpColumnName));
        }

        if (queryDto?.Length.HasValue == true)
        {
            exp = exp.And(x => x.Length == queryDto.Length);
        }

        if (queryDto?.DecimalDigits.HasValue == true)
        {
            exp = exp.And(x => x.DecimalDigits == queryDto.DecimalDigits);
        }

        if (queryDto?.IsPk.HasValue == true)
        {
            exp = exp.And(x => x.IsPk == queryDto.IsPk);
        }

        if (queryDto?.IsIncrement.HasValue == true)
        {
            exp = exp.And(x => x.IsIncrement == queryDto.IsIncrement);
        }

        if (queryDto?.IsRequired.HasValue == true)
        {
            exp = exp.And(x => x.IsRequired == queryDto.IsRequired);
        }

        if (queryDto?.IsCreate.HasValue == true)
        {
            exp = exp.And(x => x.IsCreate == queryDto.IsCreate);
        }

        if (queryDto?.IsUpdate.HasValue == true)
        {
            exp = exp.And(x => x.IsUpdate == queryDto.IsUpdate);
        }

        if (queryDto?.IsUnique.HasValue == true)
        {
            exp = exp.And(x => x.IsUnique == queryDto.IsUnique);
        }

        if (queryDto?.IsList.HasValue == true)
        {
            exp = exp.And(x => x.IsList == queryDto.IsList);
        }

        if (queryDto?.IsExport.HasValue == true)
        {
            exp = exp.And(x => x.IsExport == queryDto.IsExport);
        }

        if (queryDto?.IsSort.HasValue == true)
        {
            exp = exp.And(x => x.IsSort == queryDto.IsSort);
        }

        if (queryDto?.IsQuery.HasValue == true)
        {
            exp = exp.And(x => x.IsQuery == queryDto.IsQuery);
        }

        if (!string.IsNullOrEmpty(queryDto?.QueryType))
        {
            exp = exp.And(x => x.QueryType != null && x.QueryType.Contains(queryDto.QueryType));
        }

        if (!string.IsNullOrEmpty(queryDto?.HtmlType))
        {
            exp = exp.And(x => x.HtmlType != null && x.HtmlType.Contains(queryDto.HtmlType));
        }

        if (!string.IsNullOrEmpty(queryDto?.DictType))
        {
            exp = exp.And(x => x.DictType != null && x.DictType.Contains(queryDto.DictType));
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
