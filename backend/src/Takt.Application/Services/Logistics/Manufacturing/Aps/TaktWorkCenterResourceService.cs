// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterResourceService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心资源应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 工作中心资源应用服务
/// </summary>
public class TaktWorkCenterResourceService : TaktServiceBase, ITaktWorkCenterResourceService
{
    private readonly ITaktCompanyRepository<TaktWorkCenterResource> _workCenterResourceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workCenterResourceRepository">工作中心资源仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktWorkCenterResourceService(
        ITaktCompanyRepository<TaktWorkCenterResource> workCenterResourceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workCenterResourceRepository = workCenterResourceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工作中心资源列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktWorkCenterResourceDto>> GetWorkCenterResourceListAsync(TaktWorkCenterResourceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _workCenterResourceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktWorkCenterResourceDto>.Create(
            data.Adapt<List<TaktWorkCenterResourceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterResourceDto?> GetWorkCenterResourceByIdAsync(long id)
    {
        var entity = await _workCenterResourceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktWorkCenterResourceDto>();
    }

    /// <summary>
    /// 获取工作中心资源选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetWorkCenterResourceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _workCenterResourceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ResourceStatus == 1,
            x => x.ResourceName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ResourceName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工作中心资源
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterResourceDto> CreateWorkCenterResourceAsync(TaktWorkCenterResourceCreateDto dto)
    {
        var entity = dto.Adapt<TaktWorkCenterResource>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterResourceRepository,
            x => x.WorkCenterId == entity.WorkCenterId
                && x.ResourceCode == entity.ResourceCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique)
        {
            throw new TaktBusinessException("工作中心资源的WorkCenterId、ResourceCode已存在");
        }
        entity = await _workCenterResourceRepository.CreateAsync(entity);
        return await GetWorkCenterResourceByIdAsync(entity.Id) ?? entity.Adapt<TaktWorkCenterResourceDto>();
    }

    /// <summary>
    /// 更新工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterResourceDto> UpdateWorkCenterResourceAsync(long id, TaktWorkCenterResourceUpdateDto dto)
    {
        var entity = await _workCenterResourceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心资源不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique = await _uniqueValidator.IsUniqueAsync(
            _workCenterResourceRepository,
            x => x.WorkCenterId == entity.WorkCenterId
                && x.ResourceCode == entity.ResourceCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique)
        {
            throw new TaktBusinessException("工作中心资源的WorkCenterId、ResourceCode已存在");
        }
        await _workCenterResourceRepository.UpdateAsync(entity);
        return await GetWorkCenterResourceByIdAsync(id) ?? throw new TaktBusinessException("工作中心资源不存在");
    }

    /// <summary>
    /// 删除工作中心资源
    /// </summary>
    /// <param name="id">工作中心资源ID</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterResourceByIdAsync(long id)
    {
        var deleted = await _workCenterResourceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工作中心资源不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工作中心资源
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkCenterResourceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteWorkCenterResourceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工作中心资源状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkCenterResourceDto> UpdateWorkCenterResourceStatusAsync(TaktWorkCenterResourceStatusDto dto)
    {
        var entity = await _workCenterResourceRepository.GetByIdAsync(dto.WorkCenterResourceId);
        if (entity == null)
        {
            throw new TaktBusinessException("工作中心资源不存在");
        }
        entity.ResourceStatus = dto.ResourceStatus;
        await _workCenterResourceRepository.UpdateAsync(entity);
        return await GetWorkCenterResourceByIdAsync(dto.WorkCenterResourceId) ?? throw new TaktBusinessException("工作中心资源不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetWorkCenterResourceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktWorkCenterResourceTemplateDto>(
            sheetName ?? "工作中心资源导入模板",
            fileName ?? "工作中心资源导入模板.xlsx");
    }

    /// <summary>
    /// 导入工作中心资源
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportWorkCenterResourceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktWorkCenterResourceImportDto>(fileStream, sheetName ?? "工作中心资源导入模板");
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
                var entity = rows[i].Adapt<TaktWorkCenterResource>();
                var importKey = $"{entity.WorkCenterId}|{entity.ResourceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（WorkCenterId、ResourceCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique = await _uniqueValidator.IsUniqueAsync(
                    _workCenterResourceRepository,
                    x => x.WorkCenterId == entity.WorkCenterId
                        && x.ResourceCode == entity.ResourceCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_wc_resource_unique)
                {
                    throw new TaktBusinessException("工作中心资源的WorkCenterId、ResourceCode已存在");
                }
                await _workCenterResourceRepository.CreateAsync(entity);
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
    /// 导出工作中心资源
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportWorkCenterResourceAsync(TaktWorkCenterResourceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktWorkCenterResourceQueryDto());
        var list = await _workCenterResourceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWorkCenterResourceExportDto>(),
                sheetName ?? "工作中心资源数据",
                fileName ?? "工作中心资源导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktWorkCenterResourceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工作中心资源数据",
            fileName ?? "工作中心资源导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工作中心资源查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktWorkCenterResource, bool>> QueryExpression(TaktWorkCenterResourceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktWorkCenterResource>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.WorkCenterId).Contains(keywords)
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.ResourceCode != null && x.ResourceCode.Contains(keywords))
                || (x.ResourceName != null && x.ResourceName.Contains(keywords))
                || SqlFunc.ToString(x.ResourceType).Contains(keywords)
                || SqlFunc.ToString(x.ParallelCapacity).Contains(keywords)
                || SqlFunc.ToString(x.EfficiencyRate).Contains(keywords)
                || SqlFunc.ToString(x.ResourceStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.WorkCenterId.HasValue == true)
        {
            exp = exp.And(x => x.WorkCenterId == queryDto.WorkCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResourceCode))
        {
            exp = exp.And(x => x.ResourceCode != null && x.ResourceCode.Contains(queryDto.ResourceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResourceName))
        {
            exp = exp.And(x => x.ResourceName != null && x.ResourceName.Contains(queryDto.ResourceName));
        }

        if (queryDto?.ResourceType.HasValue == true)
        {
            exp = exp.And(x => x.ResourceType == queryDto.ResourceType);
        }

        if (queryDto?.ParallelCapacity.HasValue == true)
        {
            exp = exp.And(x => x.ParallelCapacity == queryDto.ParallelCapacity);
        }

        if (queryDto?.EfficiencyRate.HasValue == true)
        {
            exp = exp.And(x => x.EfficiencyRate == queryDto.EfficiencyRate);
        }

        if (queryDto?.ResourceStatus.HasValue == true)
        {
            exp = exp.And(x => x.ResourceStatus == queryDto.ResourceStatus);
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
