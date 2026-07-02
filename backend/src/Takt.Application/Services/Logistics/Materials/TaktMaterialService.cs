// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：全局物料应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 全局物料应用服务
/// </summary>
public class TaktMaterialService : TaktServiceBase, ITaktMaterialService
{
    private readonly ITaktTenantRepository<TaktMaterial> _materialRepository;
    private readonly ITaktTenantRepository<TaktMaterialChangeLog> _materialChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRepository">全局物料仓储</param>
    /// <param name="materialChangeLogRepository">MaterialChangeLog仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialService(
        ITaktTenantRepository<TaktMaterial> materialRepository,
        ITaktTenantRepository<TaktMaterialChangeLog> materialChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialRepository = materialRepository;
        _materialChangeLogRepository = materialChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取全局物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDto>> GetMaterialListAsync(TaktMaterialQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDto>.Create(
            data.Adapt<List<TaktMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto?> GetMaterialByIdAsync(long id)
    {
        var entity = await _materialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaterialDto>();
        await FillMaterialDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取全局物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialOptionsAsync()
    {
        var list = await _materialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialStatus == 1,
            x => x.MaterialName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.MaterialName ?? e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建全局物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> CreateMaterialAsync(TaktMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterial>();
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRepository,
            x => x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        entity = await _materialRepository.CreateAsync(entity);
                await SaveMaterialChildrenAsync(entity, dto);
        return await GetMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDto>();
    }

    /// <summary>
    /// 更新全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> UpdateMaterialAsync(long id, TaktMaterialUpdateDto dto)
    {
        var entity = await _materialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRepository,
            x => x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        await _materialRepository.UpdateAsync(entity);
                await SaveMaterialChildrenAsync(entity, dto);
        return await GetMaterialByIdAsync(id) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 删除全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialByIdAsync(long id)
    {
        var entity = await _materialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在或已删除");
        }
        await _materialChangeLogRepository.DeleteAsync(x => x.MaterialId == entity.Id);
        var deleted = await _materialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("全局物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除全局物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新全局物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDto> UpdateMaterialStatusAsync(TaktMaterialStatusDto dto)
    {
        var entity = await _materialRepository.GetByIdAsync(dto.MaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        entity.MaterialStatus = dto.MaterialStatus;
        await _materialRepository.UpdateAsync(entity);
        return await GetMaterialByIdAsync(dto.MaterialId) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialTemplateDto>(
            sheetName ?? "全局物料导入模板",
            fileName ?? "全局物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入全局物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialImportDto>(fileStream, sheetName ?? "全局物料导入模板");
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
                var entity = rows[i].Adapt<TaktMaterial>();
                var importKey = $"{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialRepository,
                    x => x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_material_unique)
                {
                    throw new TaktBusinessException("全局物料的MaterialCode已存在");
                }
                await _materialRepository.CreateAsync(entity);
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
    /// 导出全局物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialAsync(TaktMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialQueryDto());
        var list = await _materialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialExportDto>(),
                sheetName ?? "全局物料数据",
                fileName ?? "全局物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "全局物料数据",
            fileName ?? "全局物料导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充全局物料详情（加载 OneToMany 子表：全局物料变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaterialDetailsAsync(TaktMaterialDto dto, TaktMaterial entity)
    {
        if (dto == null)
        {
            return;
        }
        // 全局物料变更记录 → dto.ChangeLogs
        var changelogs = await _materialChangeLogRepository.GetListAsync(x => x.MaterialId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktMaterialChangeLogDto>>();
    }

    /// <summary>
    /// 保存全局物料子表级联（全局物料变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaterialChildrenAsync(TaktMaterial entity, TaktMaterialCreateDto dto)
    {
        // 全局物料变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _materialChangeLogRepository.DeleteAsync(x => x.MaterialId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktMaterialChangeLog>>();
            foreach (var child in changelogs)
            {
                child.MaterialId = entity.Id;
            }
            await _materialChangeLogRepository.DeleteAsync(x => x.MaterialId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _materialChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建全局物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterial, bool>> QueryExpression(TaktMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterial>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.MaterialModel != null && x.MaterialModel.Contains(keywords))
                || (x.MaterialBrand != null && x.MaterialBrand.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.ManufacturerMaterialCode != null && x.ManufacturerMaterialCode.Contains(keywords))
                || (x.MaterialAttributes != null && x.MaterialAttributes.Contains(keywords))
                || (x.IsEndOfLife != null && x.IsEndOfLife.Contains(keywords))
                || SqlFunc.ToString(x.MaterialStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialName))
        {
            exp = exp.And(x => x.MaterialName != null && x.MaterialName.Contains(queryDto.MaterialName));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialHierarchy))
        {
            exp = exp.And(x => x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(queryDto.MaterialHierarchy));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialGroup))
        {
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(queryDto.MaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialType))
        {
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(queryDto.MaterialType));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialModel))
        {
            exp = exp.And(x => x.MaterialModel != null && x.MaterialModel.Contains(queryDto.MaterialModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialBrand))
        {
            exp = exp.And(x => x.MaterialBrand != null && x.MaterialBrand.Contains(queryDto.MaterialBrand));
        }

        if (!string.IsNullOrEmpty(queryDto?.BaseUnit))
        {
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(queryDto.BaseUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.Manufacturer))
        {
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(queryDto.Manufacturer));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerMaterialCode))
        {
            exp = exp.And(x => x.ManufacturerMaterialCode != null && x.ManufacturerMaterialCode.Contains(queryDto.ManufacturerMaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialAttributes))
        {
            exp = exp.And(x => x.MaterialAttributes != null && x.MaterialAttributes.Contains(queryDto.MaterialAttributes));
        }

        if (!string.IsNullOrEmpty(queryDto?.IsEndOfLife))
        {
            exp = exp.And(x => x.IsEndOfLife != null && x.IsEndOfLife.Contains(queryDto.IsEndOfLife));
        }

        if (queryDto?.MaterialStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaterialStatus == queryDto.MaterialStatus);
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
