// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktMenuService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Identity;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 菜单应用服务
/// </summary>
public class TaktMenuService : TaktServiceBase, ITaktMenuService
{
    private readonly ITaktTenantRepository<TaktMenu> _menuRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="menuRepository">菜单仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMenuService(
        ITaktTenantRepository<TaktMenu> menuRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _menuRepository = menuRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取菜单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMenuDto>> GetMenuListAsync(TaktMenuQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _menuRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMenuDto>.Create(
            data.Adapt<List<TaktMenuDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto?> GetMenuByIdAsync(long id)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMenuDto>();
        return dto;    }

    /// <summary>
    /// 获取菜单树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    public async Task<List<TaktTreeSelectOption>> GetMenuTreeOptionsAsync()
    {
        var list = await _menuRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.MenuStatus == 1);
        return BuildMenuTreeOptions(list, 0);
    }

    /// <summary>
    /// 在内存中构建菜单树形选项（递归，按 ParentId）
    /// </summary>
    private List<TaktTreeSelectOption> BuildMenuTreeOptions(List<TaktMenu> all, long parentId)
    {
        var result = new List<TaktTreeSelectOption>();
        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder))
        {
            var option = new TaktTreeSelectOption
            {
                DictValue = item.Id,
                DictLabel = item.MenuName ?? item.Id.ToString(),
                SortOrder = item.SortOrder,
            };
            var children = BuildMenuTreeOptions(all, item.Id);
            if (children.Count > 0)
            {
                option.Children = children;
            }
            result.Add(option);
        }
        return result;
    }

    /// <summary>
    /// 获取菜单树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    public async Task<List<TaktMenuTreeDto>> GetMenuTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        var list = await _menuRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode);
        var filtered = includeDisabled
            ? list
            : list.Where(x => x.MenuStatus == 1).ToList();
        return BuildMenuTree(filtered, parentId);
    }

    /// <summary>
    /// 在内存中构建菜单树（递归，按 ParentId）
    /// </summary>
    private List<TaktMenuTreeDto> BuildMenuTree(List<TaktMenu> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.SortOrder)
            .ToList();
        var treeList = new List<TaktMenuTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktMenuTreeDto>();
            var childTree = BuildMenuTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }
            treeList.Add(treeDto);
        }
        return treeList;
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> CreateMenuAsync(TaktMenuCreateDto dto)
    {
        var entity = dto.Adapt<TaktMenu>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
            _menuRepository,
            x => x.MenuCode == entity.MenuCode);
        if (!isUnique_ix_menu_code_unique)
        {
            throw new TaktBusinessException("菜单的MenuCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _menuRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _menuRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleMenuIdsAsync(roleId);
                var ids = links.Select(x => x.MenuId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleMenusAsync(roleId, ids.ToArray());
            }
        }
        return await GetMenuByIdAsync(entity.Id) ?? entity.Adapt<TaktMenuDto>();
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuAsync(long id, TaktMenuUpdateDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
            _menuRepository,
            x => x.MenuCode == entity.MenuCode,
            id);
        if (!isUnique_ix_menu_code_unique)
        {
            throw new TaktBusinessException("菜单的MenuCode已存在");
        }
        await _menuRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleMenuIdsAsync(roleId);
                var ids = links.Select(x => x.MenuId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleMenusAsync(roleId, ids.ToArray());
            }
        }
        return await GetMenuByIdAsync(id) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMenuByIdAsync(long id)
    {
        var entity = await _menuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置菜单不允许删除");
        }
        var deleted = await _menuRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("菜单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMenuBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _menuRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置菜单不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteMenuByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新菜单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuStatusAsync(TaktMenuStatusDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(dto.MenuId);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.MenuStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置菜单");
        }
        entity.MenuStatus = dto.MenuStatus;
        await _menuRepository.UpdateAsync(entity);
        return await GetMenuByIdAsync(dto.MenuId) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 更新菜单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMenuDto> UpdateMenuSortAsync(TaktMenuSortDto dto)
    {
        var entity = await _menuRepository.GetByIdAsync(dto.MenuId);
        if (entity == null)
        {
            throw new TaktBusinessException("菜单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _menuRepository.UpdateAsync(entity);
        return await GetMenuByIdAsync(dto.MenuId) ?? throw new TaktBusinessException("菜单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMenuTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMenuTemplateDto>(
            sheetName ?? "菜单导入模板",
            fileName ?? "菜单导入模板.xlsx");
    }

    /// <summary>
    /// 导入菜单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMenuAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMenuImportDto>(fileStream, sheetName ?? "菜单导入模板");
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
                var entity = rows[i].Adapt<TaktMenu>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.MenuCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MenuCode）");
                }
                var isUnique_ix_menu_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _menuRepository,
                    x => x.MenuCode == entity.MenuCode);
                if (!isUnique_ix_menu_code_unique)
                {
                    throw new TaktBusinessException("菜单的MenuCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _menuRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _menuRepository.CreateAsync(entity);
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
    /// 导出菜单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMenuAsync(TaktMenuQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMenuQueryDto());
        var list = await _menuRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMenuExportDto>(),
                sheetName ?? "菜单数据",
                fileName ?? "菜单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMenuExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "菜单数据",
            fileName ?? "菜单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建菜单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMenu, bool>> QueryExpression(TaktMenuQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMenu>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.MenuCode != null && x.MenuCode.Contains(keywords))
                || (x.MenuName != null && x.MenuName.Contains(keywords))
                || (x.I18nKey != null && x.I18nKey.Contains(keywords))
                || (x.Icon != null && x.Icon.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.Level).Contains(keywords)
                || (x.MenuPath != null && x.MenuPath.Contains(keywords))
                || SqlFunc.ToString(x.IsLeaf).Contains(keywords)
                || SqlFunc.ToString(x.MenuType).Contains(keywords)
                || (x.Permission != null && x.Permission.Contains(keywords))
                || (x.RoutePath != null && x.RoutePath.Contains(keywords))
                || (x.ComponentPath != null && x.ComponentPath.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsExternal).Contains(keywords)
                || (x.ExternalUrl != null && x.ExternalUrl.Contains(keywords))
                || SqlFunc.ToString(x.IsCached).Contains(keywords)
                || SqlFunc.ToString(x.IsVisible).Contains(keywords)
                || SqlFunc.ToString(x.MenuStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || (x.MenuDescription != null && x.MenuDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuCode))
        {
            exp = exp.And(x => x.MenuCode != null && x.MenuCode.Contains(queryDto.MenuCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuName))
        {
            exp = exp.And(x => x.MenuName != null && x.MenuName.Contains(queryDto.MenuName));
        }

        if (!string.IsNullOrEmpty(queryDto?.I18nKey))
        {
            exp = exp.And(x => x.I18nKey != null && x.I18nKey.Contains(queryDto.I18nKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.Icon))
        {
            exp = exp.And(x => x.Icon != null && x.Icon.Contains(queryDto.Icon));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            exp = exp.And(x => x.Level == queryDto.Level);
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuPath))
        {
            exp = exp.And(x => x.MenuPath != null && x.MenuPath.Contains(queryDto.MenuPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            exp = exp.And(x => x.IsLeaf == queryDto.IsLeaf);
        }

        if (queryDto?.MenuType.HasValue == true)
        {
            exp = exp.And(x => x.MenuType == queryDto.MenuType);
        }

        if (!string.IsNullOrEmpty(queryDto?.Permission))
        {
            exp = exp.And(x => x.Permission != null && x.Permission.Contains(queryDto.Permission));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutePath))
        {
            exp = exp.And(x => x.RoutePath != null && x.RoutePath.Contains(queryDto.RoutePath));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComponentPath))
        {
            exp = exp.And(x => x.ComponentPath != null && x.ComponentPath.Contains(queryDto.ComponentPath));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsExternal.HasValue == true)
        {
            exp = exp.And(x => x.IsExternal == queryDto.IsExternal);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExternalUrl))
        {
            exp = exp.And(x => x.ExternalUrl != null && x.ExternalUrl.Contains(queryDto.ExternalUrl));
        }

        if (queryDto?.IsCached.HasValue == true)
        {
            exp = exp.And(x => x.IsCached == queryDto.IsCached);
        }

        if (queryDto?.IsVisible.HasValue == true)
        {
            exp = exp.And(x => x.IsVisible == queryDto.IsVisible);
        }

        if (queryDto?.MenuStatus.HasValue == true)
        {
            exp = exp.And(x => x.MenuStatus == queryDto.MenuStatus);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (!string.IsNullOrEmpty(queryDto?.MenuDescription))
        {
            exp = exp.And(x => x.MenuDescription != null && x.MenuDescription.Contains(queryDto.MenuDescription));
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
        if (!string.IsNullOrWhiteSpace(queryDto?.RelatedPlant))
        {
            var relatedPlant = queryDto.RelatedPlant;
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(relatedPlant));
        }


        return exp.ToExpression();
    }
}
