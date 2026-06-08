// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktCostCenterService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Domain.Entities.Accounting.Controlling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 成本中心应用服务
/// </summary>
public class TaktCostCenterService : TaktServiceBase, ITaktCostCenterService
{
    private readonly ITaktCompanyRepository<TaktCostCenter> _costCenterRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costCenterRepository">成本中心仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCostCenterService(
        ITaktCompanyRepository<TaktCostCenter> costCenterRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _costCenterRepository = costCenterRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取成本中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCostCenterDto>> GetCostCenterListAsync(TaktCostCenterQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _costCenterRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCostCenterDto>.Create(
            data.Adapt<List<TaktCostCenterDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto?> GetCostCenterByIdAsync(long id)
    {
        var entity = await _costCenterRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCostCenterDto>();
    }

    /// <summary>
    /// 获取成本中心树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    public async Task<List<TaktTreeSelectOption>> GetCostCenterTreeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _costCenterRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CostCenterStatus == TaktCommonStatus.Enabled);
        return BuildCostCenterTreeOptions(list, 0);
    }

    /// <summary>
    /// 在内存中构建成本中心树形选项（递归，按 ParentId）
    /// </summary>
    private List<TaktTreeSelectOption> BuildCostCenterTreeOptions(List<TaktCostCenter> all, long parentId)
    {
        var result = new List<TaktTreeSelectOption>();
        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder))
        {
            var option = new TaktTreeSelectOption
            {
                DictValue = item.Id,
                DictLabel = item.CostCenterName ?? item.Id.ToString(),
                SortOrder = item.SortOrder,
            };
            var children = BuildCostCenterTreeOptions(all, item.Id);
            if (children.Count > 0)
            {
                option.Children = children;
            }
            result.Add(option);
        }
        return result;
    }

    /// <summary>
    /// 获取成本中心树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    public async Task<List<TaktCostCenterTreeDto>> GetCostCenterTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        var list = await _costCenterRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        var filtered = includeDisabled
            ? list
            : list.Where(x => x.CostCenterStatus == TaktCommonStatus.Enabled).ToList();
        return BuildCostCenterTree(filtered, parentId);
    }

    /// <summary>
    /// 在内存中构建成本中心树（递归，按 ParentId）
    /// </summary>
    private List<TaktCostCenterTreeDto> BuildCostCenterTree(List<TaktCostCenter> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.SortOrder)
            .ToList();
        var treeList = new List<TaktCostCenterTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktCostCenterTreeDto>();
            var childTree = BuildCostCenterTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }
            treeList.Add(treeDto);
        }
        return treeList;
    }

    /// <summary>
    /// 创建成本中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> CreateCostCenterAsync(TaktCostCenterCreateDto dto)
    {
        var entity = dto.Adapt<TaktCostCenter>();
        var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costCenterRepository,
            x => x.CostCenterCode == entity.CostCenterCode);
        if (!isUnique_ix_cost_center_code_unique)
        {
            throw new TaktBusinessException("成本中心的CostCenterCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _costCenterRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _costCenterRepository.CreateAsync(entity);
        return await GetCostCenterByIdAsync(entity.Id) ?? entity.Adapt<TaktCostCenterDto>();
    }

    /// <summary>
    /// 更新成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterAsync(long id, TaktCostCenterUpdateDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costCenterRepository,
            x => x.CostCenterCode == entity.CostCenterCode,
            id);
        if (!isUnique_ix_cost_center_code_unique)
        {
            throw new TaktBusinessException("成本中心的CostCenterCode已存在");
        }
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(id) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 删除成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterByIdAsync(long id)
    {

        var hasChildren = await _costCenterRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _costCenterRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("成本中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除成本中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCostCenterByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新成本中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterStatusAsync(TaktCostCenterStatusDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(dto.CostCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        entity.CostCenterStatus = dto.CostCenterStatus;
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(dto.CostCenterId) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 更新成本中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterSortAsync(TaktCostCenterSortDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(dto.CostCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(dto.CostCenterId) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCostCenterTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCostCenterTemplateDto>(
            sheetName ?? "成本中心导入模板",
            fileName ?? "成本中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入成本中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCostCenterAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCostCenterImportDto>(fileStream, sheetName ?? "成本中心导入模板");
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
                var entity = rows[i].Adapt<TaktCostCenter>();
                var importKey = $"{entity.CostCenterCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CostCenterCode）");
                }
                var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _costCenterRepository,
                    x => x.CostCenterCode == entity.CostCenterCode);
                if (!isUnique_ix_cost_center_code_unique)
                {
                    throw new TaktBusinessException("成本中心的CostCenterCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _costCenterRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _costCenterRepository.CreateAsync(entity);
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
    /// 导出成本中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCostCenterAsync(TaktCostCenterQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCostCenterQueryDto());
        var list = await _costCenterRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostCenterExportDto>(),
                sheetName ?? "成本中心数据",
                fileName ?? "成本中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCostCenterExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "成本中心数据",
            fileName ?? "成本中心导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建成本中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCostCenter, bool>> QueryExpression(TaktCostCenterQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCostCenter>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.CostCenterName != null && x.CostCenterName.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.CostCenterType).Contains(keywords)
                || SqlFunc.ToString(x.ManagerId).Contains(keywords)
                || (x.ManagerName != null && x.ManagerName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.CostCenterLevel).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.CostCenterStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterName))
        {
            exp = exp.And(x => x.CostCenterName != null && x.CostCenterName.Contains(queryDto.CostCenterName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.CostCenterType.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterType == queryDto.CostCenterType);
        }

        if (queryDto?.ManagerId.HasValue == true)
        {
            exp = exp.And(x => x.ManagerId == queryDto.ManagerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ManagerName))
        {
            exp = exp.And(x => x.ManagerName != null && x.ManagerName.Contains(queryDto.ManagerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.CostCenterLevel.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterLevel == queryDto.CostCenterLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.CostCenterStatus.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterStatus == queryDto.CostCenterStatus);
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
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
