// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Organization
// 文件名称：TaktDeptService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：部门应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;
using Takt.Application.Services.Identity;

namespace Takt.Application.Services.HumanResource.Organization;

/// <summary>
/// 部门应用服务
/// </summary>
public class TaktDeptService : TaktServiceBase, ITaktDeptService
{
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDeptService(
        ITaktCompanyRepository<TaktDept> deptRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _deptRepository = deptRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDeptDto>> GetDeptListAsync(TaktDeptQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _deptRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDeptDto>.Create(
            data.Adapt<List<TaktDeptDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto?> GetDeptByIdAsync(long id)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDeptDto>();
        return dto;    }

    /// <summary>
    /// 获取部门树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    public async Task<List<TaktTreeSelectOption>> GetDeptTreeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _deptRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptStatus == TaktCommonStatus.Enabled);
        return BuildDeptTreeOptions(list, 0);
    }

    /// <summary>
    /// 在内存中构建部门树形选项（递归，按 ParentId）
    /// </summary>
    private List<TaktTreeSelectOption> BuildDeptTreeOptions(List<TaktDept> all, long parentId)
    {
        var result = new List<TaktTreeSelectOption>();
        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder))
        {
            var option = new TaktTreeSelectOption
            {
                DictValue = item.Id,
                DictLabel = item.DeptName ?? item.Id.ToString(),
                SortOrder = item.SortOrder,
            };
            var children = BuildDeptTreeOptions(all, item.Id);
            if (children.Count > 0)
            {
                option.Children = children;
            }
            result.Add(option);
        }
        return result;
    }

    /// <summary>
    /// 获取部门树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    public async Task<List<TaktDeptTreeDto>> GetDeptTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        var list = await _deptRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        var filtered = includeDisabled
            ? list
            : list.Where(x => x.DeptStatus == TaktCommonStatus.Enabled).ToList();
        return BuildDeptTree(filtered, parentId);
    }

    /// <summary>
    /// 在内存中构建部门树（递归，按 ParentId）
    /// </summary>
    private List<TaktDeptTreeDto> BuildDeptTree(List<TaktDept> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.SortOrder)
            .ToList();
        var treeList = new List<TaktDeptTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktDeptTreeDto>();
            var childTree = BuildDeptTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }
            treeList.Add(treeDto);
        }
        return treeList;
    }

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> CreateDeptAsync(TaktDeptCreateDto dto)
    {
        var entity = dto.Adapt<TaktDept>();
        entity.IsBuiltIn = TaktYesNo.No;
        var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
            _deptRepository,
            x => x.DeptCode == entity.DeptCode);
        if (!isUnique_ix_dept_code_unique)
        {
            throw new TaktBusinessException("部门的DeptCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _deptRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _deptRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleDeptIdsAsync(roleId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleDeptsAsync(roleId, ids.ToArray());
            }
        }
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeeDeptIdsAsync(employeeId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeeDeptsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetDeptByIdAsync(entity.Id) ?? entity.Adapt<TaktDeptDto>();
    }

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptAsync(long id, TaktDeptUpdateDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
            _deptRepository,
            x => x.DeptCode == entity.DeptCode,
            id);
        if (!isUnique_ix_dept_code_unique)
        {
            throw new TaktBusinessException("部门的DeptCode已存在");
        }
        await _deptRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleDeptIdsAsync(roleId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignRoleDeptsAsync(roleId, ids.ToArray());
            }
        }
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeeDeptIdsAsync(employeeId);
                var ids = links.Select(x => x.DeptId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeeDeptsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetDeptByIdAsync(id) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDeptByIdAsync(long id)
    {
        var entity = await _deptRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在或已删除");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置部门不允许删除");
        }
        var deleted = await _deptRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("部门不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDeptBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _deptRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置部门不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteDeptByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptStatusAsync(TaktDeptStatusDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(dto.DeptId);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.DeptStatus != TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException("不允许禁用内置部门");
        }
        entity.DeptStatus = dto.DeptStatus;
        await _deptRepository.UpdateAsync(entity);
        return await GetDeptByIdAsync(dto.DeptId) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 更新部门排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeptDto> UpdateDeptSortAsync(TaktDeptSortDto dto)
    {
        var entity = await _deptRepository.GetByIdAsync(dto.DeptId);
        if (entity == null)
        {
            throw new TaktBusinessException("部门不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _deptRepository.UpdateAsync(entity);
        return await GetDeptByIdAsync(dto.DeptId) ?? throw new TaktBusinessException("部门不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDeptTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDeptTemplateDto>(
            sheetName ?? "部门导入模板",
            fileName ?? "部门导入模板.xlsx");
    }

    /// <summary>
    /// 导入部门
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDeptAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDeptImportDto>(fileStream, sheetName ?? "部门导入模板");
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
                var entity = rows[i].Adapt<TaktDept>();
                entity.IsBuiltIn = TaktYesNo.No;
                var importKey = $"{entity.DeptCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DeptCode）");
                }
                var isUnique_ix_dept_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _deptRepository,
                    x => x.DeptCode == entity.DeptCode);
                if (!isUnique_ix_dept_code_unique)
                {
                    throw new TaktBusinessException("部门的DeptCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _deptRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _deptRepository.CreateAsync(entity);
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
    /// 导出部门
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeptAsync(TaktDeptQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDeptQueryDto());
        var list = await _deptRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDeptExportDto>(),
                sheetName ?? "部门数据",
                fileName ?? "部门导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDeptExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "部门数据",
            fileName ?? "部门导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建部门查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDept, bool>> QueryExpression(TaktDeptQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDept>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.Level).Contains(keywords)
                || (x.DeptPath != null && x.DeptPath.Contains(keywords))
                || SqlFunc.ToString(x.IsLeaf).Contains(keywords)
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || SqlFunc.ToString(x.CostCategory).Contains(keywords)
                || SqlFunc.ToString(x.HeadUserId).Contains(keywords)
                || (x.Phone != null && x.Phone.Contains(keywords))
                || (x.Email != null && x.Email.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || SqlFunc.ToString(x.DeptStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptCode))
        {
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(queryDto.DeptCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            exp = exp.And(x => x.Level == queryDto.Level);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptPath))
        {
            exp = exp.And(x => x.DeptPath != null && x.DeptPath.Contains(queryDto.DeptPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            exp = exp.And(x => x.IsLeaf == queryDto.IsLeaf);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (queryDto?.CostCategory.HasValue == true)
        {
            exp = exp.And(x => x.CostCategory == queryDto.CostCategory);
        }

        if (queryDto?.HeadUserId.HasValue == true)
        {
            exp = exp.And(x => x.HeadUserId == queryDto.HeadUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.Phone))
        {
            exp = exp.And(x => x.Phone != null && x.Phone.Contains(queryDto.Phone));
        }

        if (!string.IsNullOrEmpty(queryDto?.Email))
        {
            exp = exp.And(x => x.Email != null && x.Email.Contains(queryDto.Email));
        }

        if (!string.IsNullOrEmpty(queryDto?.Location))
        {
            exp = exp.And(x => x.Location != null && x.Location.Contains(queryDto.Location));
        }

        if (queryDto?.DeptStatus.HasValue == true)
        {
            exp = exp.And(x => x.DeptStatus == queryDto.DeptStatus);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
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
