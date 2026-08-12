// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesGroupService.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：销售组主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售组主数据应用服务
/// </summary>
public class TaktSalesGroupService : TaktServiceBase, ITaktSalesGroupService
{
    private readonly ITaktCompanyRepository<TaktSalesGroup> _salesGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesGroupRepository">销售组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesGroupService(
        ITaktCompanyRepository<TaktSalesGroup> salesGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesGroupRepository = salesGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesGroupDto>> GetSalesGroupListAsync(TaktSalesGroupQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesGroupDto>.Create(
            data.Adapt<List<TaktSalesGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto?> GetSalesGroupByIdAsync(long id)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesGroupDto>();
    }

    /// <summary>
    /// 获取销售组主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesGroupOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesGroupRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesGroupCode,
            DictLabel = string.IsNullOrWhiteSpace(e.SalesGroupName) ? e.SalesGroupCode : e.SalesGroupName,
            ExtLabel = e.SalesGroupCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建销售组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> CreateSalesGroupAsync(TaktSalesGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
            _salesGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesGroupCode == entity.SalesGroupCode);
        if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
        {
            throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _salesGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ResponsibleUserId == entity.ResponsibleUserId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ResponsibleUserId.GetValueOrDefault(), maxSort);
        }
        entity = await _salesGroupRepository.CreateAsync(entity);
        return await GetSalesGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesGroupDto>();
    }

    /// <summary>
    /// 更新销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupAsync(long id, TaktSalesGroupUpdateDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
            _salesGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesGroupCode == entity.SalesGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
        {
            throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
        }
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(id) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 删除销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesGroupByIdAsync(long id)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置销售组主数据不允许删除");
        }
        var deleted = await _salesGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _salesGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置销售组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteSalesGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupStatusAsync(TaktSalesGroupStatusDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(dto.SalesGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置销售组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(dto.SalesGroupId) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 更新销售组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupSortAsync(TaktSalesGroupSortDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(dto.SalesGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(dto.SalesGroupId) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesGroupTemplateDto>(
            sheetName ?? "销售组主数据导入模板",
            fileName ?? "销售组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesGroupImportDto>(fileStream, sheetName ?? "销售组主数据导入模板");
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
                var entity = rows[i].Adapt<TaktSalesGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.SalesGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesGroupCode）");
                }
                var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesGroupCode == entity.SalesGroupCode);
                if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
                {
                    throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _salesGroupRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ResponsibleUserId == entity.ResponsibleUserId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ResponsibleUserId.GetValueOrDefault(), maxSort);
                }
                await _salesGroupRepository.CreateAsync(entity);
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
    /// 导出销售组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesGroupAsync(TaktSalesGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesGroupQueryDto());
        var list = await _salesGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesGroupExportDto>(),
                sheetName ?? "销售组主数据数据",
                fileName ?? "销售组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售组主数据数据",
            fileName ?? "销售组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesGroup, bool>> QueryExpression(TaktSalesGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesGroup>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesGroupCode != null && x.SalesGroupCode.Contains(keywords))
                || (x.SalesGroupName != null && x.SalesGroupName.Contains(keywords))
                || (x.SalesGroupDescription != null && x.SalesGroupDescription.Contains(keywords))
                || SqlFunc.ToString(x.ResponsibleUserId).Contains(keywords)
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.GroupStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesGroupCode))
        {
            exp = exp.And(x => x.SalesGroupCode != null && x.SalesGroupCode.Contains(queryDto.SalesGroupCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesGroupName))
        {
            exp = exp.And(x => x.SalesGroupName != null && x.SalesGroupName.Contains(queryDto.SalesGroupName));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesGroupDescription))
        {
            exp = exp.And(x => x.SalesGroupDescription != null && x.SalesGroupDescription.Contains(queryDto.SalesGroupDescription));
        }

        if (queryDto?.ResponsibleUserId.HasValue == true)
        {
            exp = exp.And(x => x.ResponsibleUserId == queryDto.ResponsibleUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.GroupStatus.HasValue == true)
        {
            exp = exp.And(x => x.GroupStatus == queryDto.GroupStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
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
