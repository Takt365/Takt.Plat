// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktAdminDivisionService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：行政区划应用服务实现
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
using Takt.Application.Helpers;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 行政区划应用服务
/// </summary>
public class TaktAdminDivisionService : TaktServiceBase, ITaktAdminDivisionService
{
    private readonly ITaktTenantRepository<TaktAdminDivision> _adminDivisionRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="adminDivisionRepository">行政区划仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAdminDivisionService(
        ITaktTenantRepository<TaktAdminDivision> adminDivisionRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _adminDivisionRepository = adminDivisionRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取行政区划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAdminDivisionDto>> GetAdminDivisionListAsync(TaktAdminDivisionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _adminDivisionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAdminDivisionDto>.Create(
            data.Adapt<List<TaktAdminDivisionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto?> GetAdminDivisionByIdAsync(long id)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktAdminDivisionDto>();
    }

    /// <summary>
    /// 获取行政区划树形选项（懒加载：仅 parentId 直接子级一层；DictValue=Id 字符串，供表单 parentId）。
    /// 不整表、不递归；非叶子 Children=null + IsLeaf=false，前端 TaktTreeSelect loadData 再请求。
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetAdminDivisionTreeOptionsAsync(long parentId = 0)
    {
        var list = await _adminDivisionRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.ParentId == parentId
            && x.DivisionStatus == 1);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                return new TaktTreeSelectOption
                {
                    DictValue = item.Id.ToString(),
                    DictLabel = string.IsNullOrWhiteSpace(item.DivisionName) ? item.DivisionCode : item.DivisionName,
                    ExtLabel = item.DivisionCode,
                    ExtValue = item.Id.ToString(),
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取行政区划树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktAdminDivisionTreeDto>> GetAdminDivisionTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        Expression<Func<TaktAdminDivision, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId && x.DivisionStatus == 1);
        var list = await _adminDivisionRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktAdminDivisionTreeDto>();
                treeDto.Children = new List<TaktAdminDivisionTreeDto>();
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建行政区划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> CreateAdminDivisionAsync(TaktAdminDivisionCreateDto dto)
    {
        var entity = dto.Adapt<TaktAdminDivision>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
            _adminDivisionRepository,
            x => x.DivisionCode == entity.DivisionCode);
        if (!isUnique_ix_admin_division_code_unique)
        {
            throw new TaktBusinessException("行政区划的DivisionCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _adminDivisionRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _adminDivisionRepository.CreateAsync(entity);
        return await GetAdminDivisionByIdAsync(entity.Id) ?? entity.Adapt<TaktAdminDivisionDto>();
    }

    /// <summary>
    /// 更新行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionAsync(long id, TaktAdminDivisionUpdateDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
            _adminDivisionRepository,
            x => x.DivisionCode == entity.DivisionCode,
            id);
        if (!isUnique_ix_admin_division_code_unique)
        {
            throw new TaktBusinessException("行政区划的DivisionCode已存在");
        }
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(id) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 删除行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAdminDivisionByIdAsync(long id)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置行政区划不允许删除");
        }

        var hasChildren = await _adminDivisionRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _adminDivisionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("行政区划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除行政区划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAdminDivisionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _adminDivisionRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置行政区划不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteAdminDivisionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新行政区划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionStatusAsync(TaktAdminDivisionStatusDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(dto.AdminDivisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.DivisionStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置行政区划");
        }
        entity.DivisionStatus = dto.DivisionStatus;
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(dto.AdminDivisionId) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 更新行政区划排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionSortAsync(TaktAdminDivisionSortDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(dto.AdminDivisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(dto.AdminDivisionId) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAdminDivisionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAdminDivisionTemplateDto>(
            sheetName ?? "行政区划导入模板",
            fileName ?? "行政区划导入模板.xlsx");
    }

    /// <summary>
    /// 导入行政区划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAdminDivisionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAdminDivisionImportDto>(fileStream, sheetName ?? "行政区划导入模板");
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
                var entity = rows[i].Adapt<TaktAdminDivision>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.DivisionCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DivisionCode）");
                }
                var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _adminDivisionRepository,
                    x => x.DivisionCode == entity.DivisionCode);
                if (!isUnique_ix_admin_division_code_unique)
                {
                    throw new TaktBusinessException("行政区划的DivisionCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _adminDivisionRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _adminDivisionRepository.CreateAsync(entity);
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
    /// 导出行政区划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAdminDivisionAsync(TaktAdminDivisionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAdminDivisionQueryDto());
        var list = await _adminDivisionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAdminDivisionExportDto>(),
                sheetName ?? "行政区划数据",
                fileName ?? "行政区划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAdminDivisionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "行政区划数据",
            fileName ?? "行政区划导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建行政区划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAdminDivision, bool>> QueryExpression(TaktAdminDivisionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAdminDivision>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.DivisionCode != null && x.DivisionCode.Contains(keywords))
                || (x.DivisionName != null && x.DivisionName.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.Level).Contains(keywords)
                || (x.DivisionPath != null && x.DivisionPath.Contains(keywords))
                || SqlFunc.ToString(x.IsLeaf).Contains(keywords)
                || (x.PostalCode != null && x.PostalCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.PhoneCode != null && x.PhoneCode.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.DivisionStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CountryCode))
        {
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(queryDto.CountryCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DivisionCode))
        {
            exp = exp.And(x => x.DivisionCode != null && x.DivisionCode.Contains(queryDto.DivisionCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DivisionName))
        {
            exp = exp.And(x => x.DivisionName != null && x.DivisionName.Contains(queryDto.DivisionName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            exp = exp.And(x => x.Level == queryDto.Level);
        }

        if (!string.IsNullOrEmpty(queryDto?.DivisionPath))
        {
            exp = exp.And(x => x.DivisionPath != null && x.DivisionPath.Contains(queryDto.DivisionPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            exp = exp.And(x => x.IsLeaf == queryDto.IsLeaf);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostalCode))
        {
            exp = exp.And(x => x.PostalCode != null && x.PostalCode.Contains(queryDto.PostalCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PhoneCode))
        {
            exp = exp.And(x => x.PhoneCode != null && x.PhoneCode.Contains(queryDto.PhoneCode));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.DivisionStatus.HasValue == true)
        {
            exp = exp.And(x => x.DivisionStatus == queryDto.DivisionStatus);
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
