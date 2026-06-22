// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeamService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组应用服务实现
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
/// 生产班组应用服务
/// </summary>
public class TaktProductionTeamService : TaktServiceBase, ITaktProductionTeamService
{
    private readonly ITaktCompanyRepository<TaktProductionTeam> _productionTeamRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionTeamRepository">生产班组仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionTeamService(
        ITaktCompanyRepository<TaktProductionTeam> productionTeamRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionTeamRepository = productionTeamRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产班组列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionTeamDto>> GetProductionTeamListAsync(TaktProductionTeamQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionTeamRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionTeamDto>.Create(
            data.Adapt<List<TaktProductionTeamDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto?> GetProductionTeamByIdAsync(long id)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionTeamDto>();
    }

    /// <summary>
    /// 获取生产班组选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionTeamOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productionTeamRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.Status == 1,
            x => x.TeamName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TeamName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建生产班组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> CreateProductionTeamAsync(TaktProductionTeamCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionTeam>();
        var isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TeamCategory == entity.TeamCategory);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique)
        {
            throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
        }
        entity = await _productionTeamRepository.CreateAsync(entity);
        return await GetProductionTeamByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionTeamDto>();
    }

    /// <summary>
    /// 更新生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> UpdateProductionTeamAsync(long id, TaktProductionTeamUpdateDto dto)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TeamCategory == entity.TeamCategory,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique)
        {
            throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
        }
        await _productionTeamRepository.UpdateAsync(entity);
        return await GetProductionTeamByIdAsync(id) ?? throw new TaktBusinessException("生产班组不存在");
    }

    /// <summary>
    /// 删除生产班组
    /// </summary>
    /// <param name="id">生产班组ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamByIdAsync(long id)
    {
        var deleted = await _productionTeamRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产班组不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产班组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionTeamByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产班组状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamDto> UpdateProductionTeamStatusAsync(TaktProductionTeamStatusDto dto)
    {
        var entity = await _productionTeamRepository.GetByIdAsync(dto.ProductionTeamId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组不存在");
        }
        entity.Status = dto.Status;
        await _productionTeamRepository.UpdateAsync(entity);
        return await GetProductionTeamByIdAsync(dto.ProductionTeamId) ?? throw new TaktBusinessException("生产班组不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionTeamTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionTeamTemplateDto>(
            sheetName ?? "生产班组导入模板",
            fileName ?? "生产班组导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产班组
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionTeamAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionTeamImportDto>(fileStream, sheetName ?? "生产班组导入模板");
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
                var entity = rows[i].Adapt<TaktProductionTeam>();
                var importKey = $"{entity.PlantCode}|{entity.TeamCode}|{entity.TeamCategory}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、TeamCode、TeamCategory）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionTeamRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.TeamCode == entity.TeamCode
                        && x.TeamCategory == entity.TeamCategory);
                if (!isUnique_ix_takt_logistics_manufacturing_output_production_team_team_unique)
                {
                    throw new TaktBusinessException("生产班组的PlantCode、TeamCode、TeamCategory已存在");
                }
                await _productionTeamRepository.CreateAsync(entity);
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
    /// 导出生产班组
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionTeamAsync(TaktProductionTeamQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionTeamQueryDto());
        var list = await _productionTeamRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionTeamExportDto>(),
                sheetName ?? "生产班组数据",
                fileName ?? "生产班组导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionTeamExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产班组数据",
            fileName ?? "生产班组导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产班组查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionTeam, bool>> QueryExpression(TaktProductionTeamQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionTeam>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.TeamName != null && x.TeamName.Contains(keywords))
                || (x.TeamCategory != null && x.TeamCategory.Contains(keywords))
                || (x.TeamCategoryName != null && x.TeamCategoryName.Contains(keywords))
                || (x.ProductionLine != null && x.ProductionLine.Contains(keywords))
                || SqlFunc.ToString(x.TeamLeaderId).Contains(keywords)
                || (x.TeamLeaderName != null && x.TeamLeaderName.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || SqlFunc.ToString(x.Status).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamName))
        {
            exp = exp.And(x => x.TeamName != null && x.TeamName.Contains(queryDto.TeamName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCategory))
        {
            exp = exp.And(x => x.TeamCategory != null && x.TeamCategory.Contains(queryDto.TeamCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCategoryName))
        {
            exp = exp.And(x => x.TeamCategoryName != null && x.TeamCategoryName.Contains(queryDto.TeamCategoryName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLine))
        {
            exp = exp.And(x => x.ProductionLine != null && x.ProductionLine.Contains(queryDto.ProductionLine));
        }

        if (queryDto?.TeamLeaderId.HasValue == true)
        {
            exp = exp.And(x => x.TeamLeaderId == queryDto.TeamLeaderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamLeaderName))
        {
            exp = exp.And(x => x.TeamLeaderName != null && x.TeamLeaderName.Contains(queryDto.TeamLeaderName));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (queryDto?.Status.HasValue == true)
        {
            exp = exp.And(x => x.Status == queryDto.Status);
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
