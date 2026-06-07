// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Code.Generator
// 文件名称：TaktGenTableService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成数据表配置应用服务实现
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
/// 代码生成数据表配置应用服务
/// </summary>
public class TaktGenTableService : TaktServiceBase, ITaktGenTableService
{
    private readonly ITaktTenantRepository<TaktGenTable> _genTableRepository;
    private readonly ITaktTenantRepository<TaktGenTableColumn> _genTableColumnRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="genTableRepository">代码生成数据表配置仓储</param>
    /// <param name="genTableColumnRepository">GenTableColumn仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGenTableService(
        ITaktTenantRepository<TaktGenTable> genTableRepository,
        ITaktTenantRepository<TaktGenTableColumn> genTableColumnRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _genTableRepository = genTableRepository;
        _genTableColumnRepository = genTableColumnRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取代码生成数据表配置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGenTableDto>> GetGenTableListAsync(TaktGenTableQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _genTableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGenTableDto>.Create(
            data.Adapt<List<TaktGenTableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto?> GetGenTableByIdAsync(long id)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktGenTableDto>();
        await FillGenTableDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取代码生成表配置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGenTableOptionsAsync()
    {
        var list = await _genTableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.TableName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TableName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建代码生成数据表配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto> CreateGenTableAsync(TaktGenTableCreateDto dto)
    {
        var entity = dto.Adapt<TaktGenTable>();
        var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableRepository,
            x => x.DataSource == entity.DataSource
                && x.TableName == entity.TableName);
        if (!isUnique_ix_gen_table_datasource_table_unique)
        {
            throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
        }
        entity = await _genTableRepository.CreateAsync(entity);
                await SaveGenTableChildrenAsync(entity, dto);
        return await GetGenTableByIdAsync(entity.Id) ?? entity.Adapt<TaktGenTableDto>();
    }

    /// <summary>
    /// 更新代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto> UpdateGenTableAsync(long id, TaktGenTableUpdateDto dto)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableRepository,
            x => x.DataSource == entity.DataSource
                && x.TableName == entity.TableName,
            id);
        if (!isUnique_ix_gen_table_datasource_table_unique)
        {
            throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
        }
        await _genTableRepository.UpdateAsync(entity);
                await SaveGenTableChildrenAsync(entity, dto);
        return await GetGenTableByIdAsync(id) ?? throw new TaktBusinessException("代码生成数据表配置不存在");
    }

    /// <summary>
    /// 删除代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableByIdAsync(long id)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在或已删除");
        }
        await _genTableColumnRepository.DeleteAsync(x => x.GenTableId == entity.Id);
        var deleted = await _genTableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除代码生成数据表配置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGenTableByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGenTableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGenTableTemplateDto>(
            sheetName ?? "代码生成数据表配置导入模板",
            fileName ?? "代码生成数据表配置导入模板.xlsx");
    }

    /// <summary>
    /// 导入代码生成数据表配置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGenTableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGenTableImportDto>(fileStream, sheetName ?? "代码生成数据表配置导入模板");
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
                var entity = rows[i].Adapt<TaktGenTable>();
                var importKey = $"{entity.DataSource}|{entity.TableName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DataSource、TableName）");
                }
                var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
                    _genTableRepository,
                    x => x.DataSource == entity.DataSource
                        && x.TableName == entity.TableName);
                if (!isUnique_ix_gen_table_datasource_table_unique)
                {
                    throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
                }
                await _genTableRepository.CreateAsync(entity);
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
    /// 导出代码生成数据表配置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGenTableAsync(TaktGenTableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktGenTableQueryDto());
        var list = await _genTableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableExportDto>(),
                sheetName ?? "代码生成数据表配置数据",
                fileName ?? "代码生成数据表配置导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGenTableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "代码生成数据表配置数据",
            fileName ?? "代码生成数据表配置导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充代码生成数据表配置详情（加载 OneToMany 子表：代码生成数据表列配置）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillGenTableDetailsAsync(TaktGenTableDto dto, TaktGenTable entity)
    {
        if (dto == null)
        {
            return;
        }
        // 代码生成数据表列配置 → dto.Columns
        var columns = await _genTableColumnRepository.GetListAsync(x => x.GenTableId == entity.Id);
        dto.Columns = columns.Adapt<List<TaktGenTableColumnDto>>();
    }

    /// <summary>
    /// 保存代码生成数据表配置子表级联（代码生成数据表列配置；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveGenTableChildrenAsync(TaktGenTable entity, TaktGenTableCreateDto dto)
    {
        // 代码生成数据表列配置（Columns）
        if (dto.Columns is not { Count: > 0 })
        {
            await _genTableColumnRepository.DeleteAsync(x => x.GenTableId == entity.Id);
        }
        else
        {
            var columns = dto.Columns.Adapt<List<TaktGenTableColumn>>();
            foreach (var child in columns)
            {
                child.GenTableId = entity.Id;
            }
            var columnsNeedSort = columns.Where(c => c.SortOrder <= 0).ToList();
            if (columnsNeedSort.Count > 0)
            {
                var maxSort = await _genTableColumnRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, columnsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in columns)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            var columnsNeedLine = columns.Where(c => c.LineNumber <= 0).ToList();
            if (columnsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.TreeCode) ? entity.TreeCode : entity.Id.ToString();
                var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, columnsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in columns)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < columns.Count; i++)
                        {
                            var key = $"{columns[i].GenTableId}|{columns[i].DatabaseColumnName}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"代码生成数据表列配置第{i + 1}项与本次提交的其他项重复（GenTableId、DatabaseColumnName）");
                            }
                        }
            await _genTableColumnRepository.DeleteAsync(x => x.GenTableId == entity.Id);
            foreach (var child in columns)
            {
            var isUnique_ix_gen_table_column_column_unique = await _uniqueValidator.IsUniqueAsync(
                _genTableColumnRepository,
                x => x.GenTableId == child.GenTableId
                    && x.DatabaseColumnName == child.DatabaseColumnName);
            if (!isUnique_ix_gen_table_column_column_unique)
            {
                throw new TaktBusinessException("代码生成数据表列配置的GenTableId、DatabaseColumnName已存在");
            }
            }
            await _genTableColumnRepository.CreateRangeAsync(columns);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建代码生成数据表配置查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGenTable, bool>> QueryExpression(TaktGenTableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGenTable>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.DataSource != null && x.DataSource.Contains(keywords))
                || (x.TableName != null && x.TableName.Contains(keywords))
                || (x.TableComment != null && x.TableComment.Contains(keywords))
                || (x.SubTableName != null && x.SubTableName.Contains(keywords))
                || (x.SubTableFkName != null && x.SubTableFkName.Contains(keywords))
                || (x.TreeCode != null && x.TreeCode.Contains(keywords))
                || (x.TreeParentCode != null && x.TreeParentCode.Contains(keywords))
                || (x.TreeName != null && x.TreeName.Contains(keywords))
                || SqlFunc.ToString(x.InDatabase).Contains(keywords)
                || (x.GenTemplateCategory != null && x.GenTemplateCategory.Contains(keywords))
                || (x.GenModuleName != null && x.GenModuleName.Contains(keywords))
                || (x.GenBusinessName != null && x.GenBusinessName.Contains(keywords))
                || (x.GenFunctionName != null && x.GenFunctionName.Contains(keywords))
                || (x.PermsPrefix != null && x.PermsPrefix.Contains(keywords))
                || (x.MenuButtonGroup != null && x.MenuButtonGroup.Contains(keywords))
                || (x.NamePrefix != null && x.NamePrefix.Contains(keywords))
                || (x.EntityNamespace != null && x.EntityNamespace.Contains(keywords))
                || (x.EntityClassName != null && x.EntityClassName.Contains(keywords))
                || (x.DtoNamespace != null && x.DtoNamespace.Contains(keywords))
                || (x.DtoClassName != null && x.DtoClassName.Contains(keywords))
                || (x.ServiceNamespace != null && x.ServiceNamespace.Contains(keywords))
                || (x.IServiceClassName != null && x.IServiceClassName.Contains(keywords))
                || (x.ServiceClassName != null && x.ServiceClassName.Contains(keywords))
                || (x.ControllerNamespace != null && x.ControllerNamespace.Contains(keywords))
                || (x.ControllerClassName != null && x.ControllerClassName.Contains(keywords))
                || SqlFunc.ToString(x.IsRepository).Contains(keywords)
                || (x.RepositoryInterfaceNamespace != null && x.RepositoryInterfaceNamespace.Contains(keywords))
                || (x.IRepositoryClassName != null && x.IRepositoryClassName.Contains(keywords))
                || (x.RepositoryNamespace != null && x.RepositoryNamespace.Contains(keywords))
                || (x.RepositoryClassName != null && x.RepositoryClassName.Contains(keywords))
                || (x.GenFunction != null && x.GenFunction.Contains(keywords))
                || SqlFunc.ToString(x.GenMethod).Contains(keywords)
                || (x.GenPath != null && x.GenPath.Contains(keywords))
                || SqlFunc.ToString(x.IsGenMenu).Contains(keywords)
                || SqlFunc.ToString(x.ParentMenuId).Contains(keywords)
                || SqlFunc.ToString(x.IsGenTranslation).Contains(keywords)
                || (x.SortField != null && x.SortField.Contains(keywords))
                || (x.SortType != null && x.SortType.Contains(keywords))
                || SqlFunc.ToString(x.FrontUi).Contains(keywords)
                || SqlFunc.ToString(x.FrontFormLayout).Contains(keywords)
                || SqlFunc.ToString(x.FrontBtnStyle).Contains(keywords)
                || SqlFunc.ToString(x.IsGenCode).Contains(keywords)
                || SqlFunc.ToString(x.GenCodeCount).Contains(keywords)
                || SqlFunc.ToString(x.IsUseTabs).Contains(keywords)
                || SqlFunc.ToString(x.TabsFieldCount).Contains(keywords)
                || (x.GenAuthor != null && x.GenAuthor.Contains(keywords))
                || (x.OtherGenOptions != null && x.OtherGenOptions.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.DataSource))
        {
            exp = exp.And(x => x.DataSource != null && x.DataSource.Contains(queryDto.DataSource));
        }

        if (!string.IsNullOrEmpty(queryDto?.TableName))
        {
            exp = exp.And(x => x.TableName != null && x.TableName.Contains(queryDto.TableName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TableComment))
        {
            exp = exp.And(x => x.TableComment != null && x.TableComment.Contains(queryDto.TableComment));
        }

        if (!string.IsNullOrEmpty(queryDto?.SubTableName))
        {
            exp = exp.And(x => x.SubTableName != null && x.SubTableName.Contains(queryDto.SubTableName));
        }

        if (!string.IsNullOrEmpty(queryDto?.SubTableFkName))
        {
            exp = exp.And(x => x.SubTableFkName != null && x.SubTableFkName.Contains(queryDto.SubTableFkName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TreeCode))
        {
            exp = exp.And(x => x.TreeCode != null && x.TreeCode.Contains(queryDto.TreeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TreeParentCode))
        {
            exp = exp.And(x => x.TreeParentCode != null && x.TreeParentCode.Contains(queryDto.TreeParentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TreeName))
        {
            exp = exp.And(x => x.TreeName != null && x.TreeName.Contains(queryDto.TreeName));
        }

        if (queryDto?.InDatabase.HasValue == true)
        {
            exp = exp.And(x => x.InDatabase == queryDto.InDatabase);
        }

        if (!string.IsNullOrEmpty(queryDto?.GenTemplateCategory))
        {
            exp = exp.And(x => x.GenTemplateCategory != null && x.GenTemplateCategory.Contains(queryDto.GenTemplateCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.GenModuleName))
        {
            exp = exp.And(x => x.GenModuleName != null && x.GenModuleName.Contains(queryDto.GenModuleName));
        }

        if (!string.IsNullOrEmpty(queryDto?.GenBusinessName))
        {
            exp = exp.And(x => x.GenBusinessName != null && x.GenBusinessName.Contains(queryDto.GenBusinessName));
        }

        if (!string.IsNullOrEmpty(queryDto?.GenFunctionName))
        {
            exp = exp.And(x => x.GenFunctionName != null && x.GenFunctionName.Contains(queryDto.GenFunctionName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PermsPrefix))
        {
            exp = exp.And(x => x.PermsPrefix != null && x.PermsPrefix.Contains(queryDto.PermsPrefix));
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuButtonGroup))
        {
            exp = exp.And(x => x.MenuButtonGroup != null && x.MenuButtonGroup.Contains(queryDto.MenuButtonGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.NamePrefix))
        {
            exp = exp.And(x => x.NamePrefix != null && x.NamePrefix.Contains(queryDto.NamePrefix));
        }

        if (!string.IsNullOrEmpty(queryDto?.EntityNamespace))
        {
            exp = exp.And(x => x.EntityNamespace != null && x.EntityNamespace.Contains(queryDto.EntityNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.EntityClassName))
        {
            exp = exp.And(x => x.EntityClassName != null && x.EntityClassName.Contains(queryDto.EntityClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.DtoNamespace))
        {
            exp = exp.And(x => x.DtoNamespace != null && x.DtoNamespace.Contains(queryDto.DtoNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.DtoClassName))
        {
            exp = exp.And(x => x.DtoClassName != null && x.DtoClassName.Contains(queryDto.DtoClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceNamespace))
        {
            exp = exp.And(x => x.ServiceNamespace != null && x.ServiceNamespace.Contains(queryDto.ServiceNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.IServiceClassName))
        {
            exp = exp.And(x => x.IServiceClassName != null && x.IServiceClassName.Contains(queryDto.IServiceClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceClassName))
        {
            exp = exp.And(x => x.ServiceClassName != null && x.ServiceClassName.Contains(queryDto.ServiceClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ControllerNamespace))
        {
            exp = exp.And(x => x.ControllerNamespace != null && x.ControllerNamespace.Contains(queryDto.ControllerNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.ControllerClassName))
        {
            exp = exp.And(x => x.ControllerClassName != null && x.ControllerClassName.Contains(queryDto.ControllerClassName));
        }

        if (queryDto?.IsRepository.HasValue == true)
        {
            exp = exp.And(x => x.IsRepository == queryDto.IsRepository);
        }

        if (!string.IsNullOrEmpty(queryDto?.RepositoryInterfaceNamespace))
        {
            exp = exp.And(x => x.RepositoryInterfaceNamespace != null && x.RepositoryInterfaceNamespace.Contains(queryDto.RepositoryInterfaceNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.IRepositoryClassName))
        {
            exp = exp.And(x => x.IRepositoryClassName != null && x.IRepositoryClassName.Contains(queryDto.IRepositoryClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.RepositoryNamespace))
        {
            exp = exp.And(x => x.RepositoryNamespace != null && x.RepositoryNamespace.Contains(queryDto.RepositoryNamespace));
        }

        if (!string.IsNullOrEmpty(queryDto?.RepositoryClassName))
        {
            exp = exp.And(x => x.RepositoryClassName != null && x.RepositoryClassName.Contains(queryDto.RepositoryClassName));
        }

        if (!string.IsNullOrEmpty(queryDto?.GenFunction))
        {
            exp = exp.And(x => x.GenFunction != null && x.GenFunction.Contains(queryDto.GenFunction));
        }

        if (queryDto?.GenMethod.HasValue == true)
        {
            exp = exp.And(x => x.GenMethod == queryDto.GenMethod);
        }

        if (!string.IsNullOrEmpty(queryDto?.GenPath))
        {
            exp = exp.And(x => x.GenPath != null && x.GenPath.Contains(queryDto.GenPath));
        }

        if (queryDto?.IsGenMenu.HasValue == true)
        {
            exp = exp.And(x => x.IsGenMenu == queryDto.IsGenMenu);
        }

        if (queryDto?.ParentMenuId.HasValue == true)
        {
            exp = exp.And(x => x.ParentMenuId == queryDto.ParentMenuId);
        }

        if (queryDto?.IsGenTranslation.HasValue == true)
        {
            exp = exp.And(x => x.IsGenTranslation == queryDto.IsGenTranslation);
        }

        if (!string.IsNullOrEmpty(queryDto?.SortField))
        {
            exp = exp.And(x => x.SortField != null && x.SortField.Contains(queryDto.SortField));
        }

        if (!string.IsNullOrEmpty(queryDto?.SortType))
        {
            exp = exp.And(x => x.SortType != null && x.SortType.Contains(queryDto.SortType));
        }

        if (queryDto?.FrontUi.HasValue == true)
        {
            exp = exp.And(x => x.FrontUi == queryDto.FrontUi);
        }

        if (queryDto?.FrontFormLayout.HasValue == true)
        {
            exp = exp.And(x => x.FrontFormLayout == queryDto.FrontFormLayout);
        }

        if (queryDto?.FrontBtnStyle.HasValue == true)
        {
            exp = exp.And(x => x.FrontBtnStyle == queryDto.FrontBtnStyle);
        }

        if (queryDto?.IsGenCode.HasValue == true)
        {
            exp = exp.And(x => x.IsGenCode == queryDto.IsGenCode);
        }

        if (queryDto?.GenCodeCount.HasValue == true)
        {
            exp = exp.And(x => x.GenCodeCount == queryDto.GenCodeCount);
        }

        if (queryDto?.IsUseTabs.HasValue == true)
        {
            exp = exp.And(x => x.IsUseTabs == queryDto.IsUseTabs);
        }

        if (queryDto?.TabsFieldCount.HasValue == true)
        {
            exp = exp.And(x => x.TabsFieldCount == queryDto.TabsFieldCount);
        }

        if (!string.IsNullOrEmpty(queryDto?.GenAuthor))
        {
            exp = exp.And(x => x.GenAuthor != null && x.GenAuthor.Contains(queryDto.GenAuthor));
        }

        if (!string.IsNullOrEmpty(queryDto?.OtherGenOptions))
        {
            exp = exp.And(x => x.OtherGenOptions != null && x.OtherGenOptions.Contains(queryDto.OtherGenOptions));
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
