// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemArgumentService.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线工序参数应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线工序参数应用服务
/// </summary>
public class TaktRoutingItemArgumentService : TaktServiceBase, ITaktRoutingItemArgumentService
{
    private readonly ITaktCompanyRepository<TaktRoutingItemArgument> _routingItemArgumentRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingItemArgumentRepository">工艺路线工序参数仓储</param>
    /// <param name="routingItemRepository">工艺路线明细仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingItemArgumentService(
        ITaktCompanyRepository<TaktRoutingItemArgument> routingItemArgumentRepository,
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingItemArgumentRepository = routingItemArgumentRepository;
        _routingItemRepository = routingItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线工序参数列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingItemArgumentDto>> GetRoutingItemArgumentListAsync(TaktRoutingItemArgumentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingItemArgumentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingItemArgumentDto>.Create(
            data.Adapt<List<TaktRoutingItemArgumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemArgumentDto?> GetRoutingItemArgumentByIdAsync(long id)
    {
        var entity = await _routingItemArgumentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktRoutingItemArgumentDto>();
    }

    /// <summary>
    /// 获取工艺路线工序参数选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingItemArgumentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _routingItemArgumentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ParamName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ParamName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线工序参数
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemArgumentDto> CreateRoutingItemArgumentAsync(TaktRoutingItemArgumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktRoutingItemArgument>();
        await StampRoutingItemArgumentRoutingItemAsync(entity, dto);
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _routingItemArgumentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingItemId == entity.RoutingItemId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingItemId, maxSort);
        }
        entity = await _routingItemArgumentRepository.CreateAsync(entity);
        return await GetRoutingItemArgumentByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingItemArgumentDto>();
    }

    /// <summary>
    /// 更新工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemArgumentDto> UpdateRoutingItemArgumentAsync(long id, TaktRoutingItemArgumentUpdateDto dto)
    {
        var entity = await _routingItemArgumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线工序参数不存在");
        }
        dto.Adapt(entity);
        await StampRoutingItemArgumentRoutingItemAsync(entity, dto);
        await _routingItemArgumentRepository.UpdateAsync(entity);
        return await GetRoutingItemArgumentByIdAsync(id) ?? throw new TaktBusinessException("工艺路线工序参数不存在");
    }

    /// <summary>
    /// 删除工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemArgumentByIdAsync(long id)
    {
        var deleted = await _routingItemArgumentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线工序参数不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线工序参数
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemArgumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingItemArgumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工艺路线工序参数排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemArgumentDto> UpdateRoutingItemArgumentSortAsync(TaktRoutingItemArgumentSortDto dto)
    {
        var entity = await _routingItemArgumentRepository.GetByIdAsync(dto.RoutingItemArgumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线工序参数不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _routingItemArgumentRepository.UpdateAsync(entity);
        return await GetRoutingItemArgumentByIdAsync(dto.RoutingItemArgumentId) ?? throw new TaktBusinessException("工艺路线工序参数不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoutingItemArgumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoutingItemArgumentTemplateDto>(
            sheetName ?? "工艺路线工序参数导入模板",
            fileName ?? "工艺路线工序参数导入模板.xlsx");
    }

    /// <summary>
    /// 导入工艺路线工序参数
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoutingItemArgumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoutingItemArgumentImportDto>(fileStream, sheetName ?? "工艺路线工序参数导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktRoutingItemArgument>();
                var importDto = rows[i].Adapt<TaktRoutingItemArgumentCreateDto>();
                await StampRoutingItemArgumentRoutingItemAsync(entity, importDto);
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _routingItemArgumentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingItemId == entity.RoutingItemId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingItemId, maxSort);
                }
                await _routingItemArgumentRepository.CreateAsync(entity);
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
    /// 导出工艺路线工序参数
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingItemArgumentAsync(TaktRoutingItemArgumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktRoutingItemArgumentQueryDto());
        var list = await _routingItemArgumentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingItemArgumentExportDto>(),
                sheetName ?? "工艺路线工序参数数据",
                fileName ?? "工艺路线工序参数导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingItemArgumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线工序参数数据",
            fileName ?? "工艺路线工序参数导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工艺路线工序参数主表外键（ManyToOne → 工艺路线明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampRoutingItemArgumentRoutingItemAsync(TaktRoutingItemArgument entity, TaktRoutingItemArgumentCreateDto dto)
    {
        if (dto.RoutingItemId <= 0)
        {
            return;
        }
        var master = await _routingItemRepository.GetByIdAsync(dto.RoutingItemId);
        if (master == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        entity.RoutingItemId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线工序参数查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRoutingItemArgument, bool>> QueryExpression(TaktRoutingItemArgumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRoutingItemArgument>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.RoutingItemId).Contains(keywords)
                || (x.ParamCode != null && x.ParamCode.Contains(keywords))
                || (x.ParamName != null && x.ParamName.Contains(keywords))
                || (x.ParamUnit != null && x.ParamUnit.Contains(keywords))
                || SqlFunc.ToString(x.StandardValue).Contains(keywords)
                || SqlFunc.ToString(x.LowerLimit).Contains(keywords)
                || SqlFunc.ToString(x.UpperLimit).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingItemId == queryDto.RoutingItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ParamCode))
        {
            exp = exp.And(x => x.ParamCode != null && x.ParamCode.Contains(queryDto.ParamCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ParamName))
        {
            exp = exp.And(x => x.ParamName != null && x.ParamName.Contains(queryDto.ParamName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ParamUnit))
        {
            exp = exp.And(x => x.ParamUnit != null && x.ParamUnit.Contains(queryDto.ParamUnit));
        }

        if (queryDto?.StandardValue.HasValue == true)
        {
            exp = exp.And(x => x.StandardValue == queryDto.StandardValue);
        }

        if (queryDto?.LowerLimit.HasValue == true)
        {
            exp = exp.And(x => x.LowerLimit == queryDto.LowerLimit);
        }

        if (queryDto?.UpperLimit.HasValue == true)
        {
            exp = exp.And(x => x.UpperLimit == queryDto.UpperLimit);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
