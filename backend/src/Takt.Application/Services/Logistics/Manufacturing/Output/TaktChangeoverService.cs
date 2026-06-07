// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：切换记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 切换记录应用服务
/// </summary>
public class TaktChangeoverService : TaktServiceBase, ITaktChangeoverService
{
    private readonly ITaktCompanyRepository<TaktChangeover> _changeoverRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="changeoverRepository">切换记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktChangeoverService(
        ITaktCompanyRepository<TaktChangeover> changeoverRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _changeoverRepository = changeoverRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取切换记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktChangeoverDto>> GetChangeoverListAsync(TaktChangeoverQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _changeoverRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktChangeoverDto>.Create(
            data.Adapt<List<TaktChangeoverDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverDto?> GetChangeoverByIdAsync(long id)
    {
        var entity = await _changeoverRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktChangeoverDto>();
    }

    /// <summary>
    /// 获取切换记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetChangeoverOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _changeoverRepository.GetListAsync(
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
    /// 创建切换记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverDto> CreateChangeoverAsync(TaktChangeoverCreateDto dto)
    {
        var entity = dto.Adapt<TaktChangeover>();
        var isUnique_ix_takt_logistics_manufacturing_output_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _changeoverRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductionCategory == entity.ProductionCategory
                && x.ProductionDate == entity.ProductionDate
                && x.ProductionLine == entity.ProductionLine);
        if (!isUnique_ix_takt_logistics_manufacturing_output_changeover_unique)
        {
            throw new TaktBusinessException("切换记录的PlantCode、ProductionCategory、ProductionDate、ProductionLine已存在");
        }
        entity = await _changeoverRepository.CreateAsync(entity);
        return await GetChangeoverByIdAsync(entity.Id) ?? entity.Adapt<TaktChangeoverDto>();
    }

    /// <summary>
    /// 更新切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktChangeoverDto> UpdateChangeoverAsync(long id, TaktChangeoverUpdateDto dto)
    {
        var entity = await _changeoverRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("切换记录不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_changeover_unique = await _uniqueValidator.IsUniqueAsync(
            _changeoverRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductionCategory == entity.ProductionCategory
                && x.ProductionDate == entity.ProductionDate
                && x.ProductionLine == entity.ProductionLine,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_changeover_unique)
        {
            throw new TaktBusinessException("切换记录的PlantCode、ProductionCategory、ProductionDate、ProductionLine已存在");
        }
        await _changeoverRepository.UpdateAsync(entity);
        return await GetChangeoverByIdAsync(id) ?? throw new TaktBusinessException("切换记录不存在");
    }

    /// <summary>
    /// 删除切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteChangeoverByIdAsync(long id)
    {
        var deleted = await _changeoverRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("切换记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除切换记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteChangeoverBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteChangeoverByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetChangeoverTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktChangeoverTemplateDto>(
            sheetName ?? "切换记录导入模板",
            fileName ?? "切换记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入切换记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportChangeoverAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktChangeoverImportDto>(fileStream, sheetName ?? "切换记录导入模板");
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
                var entity = rows[i].Adapt<TaktChangeover>();
                var importKey = $"{entity.PlantCode}|{entity.ProductionCategory}|{entity.ProductionDate}|{entity.ProductionLine}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProductionCategory、ProductionDate、ProductionLine）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_changeover_unique = await _uniqueValidator.IsUniqueAsync(
                    _changeoverRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProductionCategory == entity.ProductionCategory
                        && x.ProductionDate == entity.ProductionDate
                        && x.ProductionLine == entity.ProductionLine);
                if (!isUnique_ix_takt_logistics_manufacturing_output_changeover_unique)
                {
                    throw new TaktBusinessException("切换记录的PlantCode、ProductionCategory、ProductionDate、ProductionLine已存在");
                }
                await _changeoverRepository.CreateAsync(entity);
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
    /// 导出切换记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportChangeoverAsync(TaktChangeoverQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktChangeoverQueryDto());
        var list = await _changeoverRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktChangeoverExportDto>(),
                sheetName ?? "切换记录数据",
                fileName ?? "切换记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktChangeoverExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "切换记录数据",
            fileName ?? "切换记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建切换记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktChangeover, bool>> QueryExpression(TaktChangeoverQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktChangeover>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProductionCategory != null && x.ProductionCategory.Contains(keywords))
                || (x.ProductionLine != null && x.ProductionLine.Contains(keywords))
                || SqlFunc.ToString(x.ReadSopTime).Contains(keywords)
                || SqlFunc.ToString(x.PersonCount).Contains(keywords)
                || SqlFunc.ToString(x.TotalSopTime).Contains(keywords)
                || SqlFunc.ToString(x.ChangeoverCount).Contains(keywords)
                || SqlFunc.ToString(x.ChangeoverTime).Contains(keywords)
                || SqlFunc.ToString(x.TotalChangeoverTime).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ProductionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionCategory))
        {
            exp = exp.And(x => x.ProductionCategory != null && x.ProductionCategory.Contains(queryDto.ProductionCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLine))
        {
            exp = exp.And(x => x.ProductionLine != null && x.ProductionLine.Contains(queryDto.ProductionLine));
        }

        if (queryDto?.ReadSopTime.HasValue == true)
        {
            exp = exp.And(x => x.ReadSopTime == queryDto.ReadSopTime);
        }

        if (queryDto?.PersonCount.HasValue == true)
        {
            exp = exp.And(x => x.PersonCount == queryDto.PersonCount);
        }

        if (queryDto?.TotalSopTime.HasValue == true)
        {
            exp = exp.And(x => x.TotalSopTime == queryDto.TotalSopTime);
        }

        if (queryDto?.ChangeoverCount.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverCount == queryDto.ChangeoverCount);
        }

        if (queryDto?.ChangeoverTime.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverTime == queryDto.ChangeoverTime);
        }

        if (queryDto?.TotalChangeoverTime.HasValue == true)
        {
            exp = exp.And(x => x.TotalChangeoverTime == queryDto.TotalChangeoverTime);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ProductionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProductionDate >= queryDto.ProductionDateStart);
        }

        if (queryDto?.ProductionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProductionDate <= queryDto.ProductionDateEnd);
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
