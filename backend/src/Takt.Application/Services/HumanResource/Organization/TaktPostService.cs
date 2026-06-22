// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Organization
// 文件名称：TaktPostService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位应用服务实现
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
/// 岗位应用服务
/// </summary>
public class TaktPostService : TaktServiceBase, ITaktPostService
{
    private readonly ITaktCompanyRepository<TaktPost> _postRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="postRepository">岗位仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPostService(
        ITaktCompanyRepository<TaktPost> postRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _postRepository = postRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取岗位列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPostDto>> GetPostListAsync(TaktPostQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _postRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPostDto>.Create(
            data.Adapt<List<TaktPostDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto?> GetPostByIdAsync(long id)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPostDto>();
        return dto;    }

    /// <summary>
    /// 获取岗位选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPostOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _postRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PostName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PostName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建岗位
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> CreatePostAsync(TaktPostCreateDto dto)
    {
        var entity = dto.Adapt<TaktPost>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
            _postRepository,
            x => x.PostCode == entity.PostCode);
        if (!isUnique_ix_post_code_unique)
        {
            throw new TaktBusinessException("岗位的PostCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _postRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId, maxSort);
        }
        entity = await _postRepository.CreateAsync(entity);
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeePostIdsAsync(employeeId);
                var ids = links.Select(x => x.PostId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeePostsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetPostByIdAsync(entity.Id) ?? entity.Adapt<TaktPostDto>();
    }

    /// <summary>
    /// 更新岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostAsync(long id, TaktPostUpdateDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
            _postRepository,
            x => x.PostCode == entity.PostCode,
            id);
        if (!isUnique_ix_post_code_unique)
        {
            throw new TaktBusinessException("岗位的PostCode已存在");
        }
        await _postRepository.UpdateAsync(entity);
        if (dto.EmployeeIds != null)
        {
            foreach (var employeeId in dto.EmployeeIds.Distinct())
            {
                var links = await _rbacService.GetEmployeePostIdsAsync(employeeId);
                var ids = links.Select(x => x.PostId).Distinct().ToList();
                if (!ids.Contains(entity.Id))
                {
                    ids.Add(entity.Id);
                }
                await _rbacService.AssignEmployeePostsAsync(employeeId, ids.ToArray());
            }
        }
        return await GetPostByIdAsync(id) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 删除岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>任务</returns>
    public async Task DeletePostByIdAsync(long id)
    {
        var entity = await _postRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置岗位不允许删除");
        }
        var deleted = await _postRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("岗位不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePostBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _postRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置岗位不允许删除");
        }
        foreach (var id in idList)
        {
            await DeletePostByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新岗位状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostStatusAsync(TaktPostStatusDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(dto.PostId);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.PostStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置岗位");
        }
        entity.PostStatus = dto.PostStatus;
        await _postRepository.UpdateAsync(entity);
        return await GetPostByIdAsync(dto.PostId) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 更新岗位是否内置
    /// </summary>
    /// <param name="dto">是否内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostBuiltInAsync(TaktPostBuiltInDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(dto.PostId);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("是否内置必须为字典 sys_yes_no_type 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置岗位标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _postRepository.UpdateAsync(entity);
        return await GetPostByIdAsync(dto.PostId) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 更新岗位排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPostDto> UpdatePostSortAsync(TaktPostSortDto dto)
    {
        var entity = await _postRepository.GetByIdAsync(dto.PostId);
        if (entity == null)
        {
            throw new TaktBusinessException("岗位不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _postRepository.UpdateAsync(entity);
        return await GetPostByIdAsync(dto.PostId) ?? throw new TaktBusinessException("岗位不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPostTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPostTemplateDto>(
            sheetName ?? "岗位导入模板",
            fileName ?? "岗位导入模板.xlsx");
    }

    /// <summary>
    /// 导入岗位
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPostAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPostImportDto>(fileStream, sheetName ?? "岗位导入模板");
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
                var entity = rows[i].Adapt<TaktPost>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PostCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PostCode）");
                }
                var isUnique_ix_post_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _postRepository,
                    x => x.PostCode == entity.PostCode);
                if (!isUnique_ix_post_code_unique)
                {
                    throw new TaktBusinessException("岗位的PostCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _postRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId, maxSort);
                }
                await _postRepository.CreateAsync(entity);
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
    /// 导出岗位
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPostAsync(TaktPostQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPostQueryDto());
        var list = await _postRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPostExportDto>(),
                sheetName ?? "岗位数据",
                fileName ?? "岗位导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPostExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "岗位数据",
            fileName ?? "岗位导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建岗位查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPost, bool>> QueryExpression(TaktPostQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPost>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PostCode != null && x.PostCode.Contains(keywords))
                || (x.PostName != null && x.PostName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.PostCategory != null && x.PostCategory.Contains(keywords))
                || (x.PostLevel != null && x.PostLevel.Contains(keywords))
                || SqlFunc.ToString(x.Headcount).Contains(keywords)
                || SqlFunc.ToString(x.CurrentCount).Contains(keywords)
                || (x.Responsibilities.Contains(keywords))
                || (x.Requirements.Contains(keywords))
                || SqlFunc.ToString(x.EducationRequired).Contains(keywords)
                || SqlFunc.ToString(x.ExperienceYears).Contains(keywords)
                || SqlFunc.ToString(x.SalaryMin).Contains(keywords)
                || SqlFunc.ToString(x.SalaryMax).Contains(keywords)
                || SqlFunc.ToString(x.PostStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PostCode))
        {
            exp = exp.And(x => x.PostCode != null && x.PostCode.Contains(queryDto.PostCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PostName))
        {
            exp = exp.And(x => x.PostName != null && x.PostName.Contains(queryDto.PostName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostCategory))
        {
            exp = exp.And(x => x.PostCategory == queryDto.PostCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostLevel))
        {
            exp = exp.And(x => x.PostLevel == queryDto.PostLevel);
        }

        if (queryDto?.Headcount.HasValue == true)
        {
            exp = exp.And(x => x.Headcount == queryDto.Headcount);
        }

        if (queryDto?.CurrentCount.HasValue == true)
        {
            exp = exp.And(x => x.CurrentCount == queryDto.CurrentCount);
        }

        if (!string.IsNullOrEmpty(queryDto?.Responsibilities))
        {
            exp = exp.And(x => x.Responsibilities.Contains(queryDto.Responsibilities));
        }

        if (!string.IsNullOrEmpty(queryDto?.Requirements))
        {
            exp = exp.And(x => x.Requirements.Contains(queryDto.Requirements));
        }

        if (queryDto?.EducationRequired.HasValue == true)
        {
            exp = exp.And(x => x.EducationRequired == queryDto.EducationRequired);
        }

        if (queryDto?.ExperienceYears.HasValue == true)
        {
            exp = exp.And(x => x.ExperienceYears == queryDto.ExperienceYears);
        }

        if (queryDto?.SalaryMin.HasValue == true)
        {
            exp = exp.And(x => x.SalaryMin == queryDto.SalaryMin);
        }

        if (queryDto?.SalaryMax.HasValue == true)
        {
            exp = exp.And(x => x.SalaryMax == queryDto.SalaryMax);
        }

        if (queryDto?.PostStatus.HasValue == true)
        {
            exp = exp.And(x => x.PostStatus == queryDto.PostStatus);
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
