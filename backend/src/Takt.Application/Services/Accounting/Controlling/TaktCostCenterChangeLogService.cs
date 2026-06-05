// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktCostCenterChangeLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心变更记录应用服务实现
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

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 成本中心变更记录应用服务
/// </summary>
public class TaktCostCenterChangeLogService : TaktServiceBase, ITaktCostCenterChangeLogService
{
    private readonly ITaktCompanyRepository<TaktCostCenterChangeLog> _costCenterChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costCenterChangeLogRepository">成本中心变更记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCostCenterChangeLogService(
        ITaktCompanyRepository<TaktCostCenterChangeLog> costCenterChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _costCenterChangeLogRepository = costCenterChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取成本中心变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCostCenterChangeLogDto>> GetCostCenterChangeLogListAsync(TaktCostCenterChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _costCenterChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCostCenterChangeLogDto>.Create(
            data.Adapt<List<TaktCostCenterChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterChangeLogDto?> GetCostCenterChangeLogByIdAsync(long id)
    {
        var entity = await _costCenterChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCostCenterChangeLogDto>();
    }

    /// <summary>
    /// 获取成本中心变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCostCenterChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _costCenterChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CostCenterCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CostCenterCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建成本中心变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterChangeLogDto> CreateCostCenterChangeLogAsync(TaktCostCenterChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktCostCenterChangeLog>();
        entity = await _costCenterChangeLogRepository.CreateAsync(entity);
        return await GetCostCenterChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktCostCenterChangeLogDto>();
    }

    /// <summary>
    /// 更新成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterChangeLogDto> UpdateCostCenterChangeLogAsync(long id, TaktCostCenterChangeLogUpdateDto dto)
    {
        var entity = await _costCenterChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心变更记录不存在");
        }
        dto.Adapt(entity);
        await _costCenterChangeLogRepository.UpdateAsync(entity);
        return await GetCostCenterChangeLogByIdAsync(id) ?? throw new TaktBusinessException("成本中心变更记录不存在");
    }

    /// <summary>
    /// 删除成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterChangeLogByIdAsync(long id)
    {
        var deleted = await _costCenterChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("成本中心变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除成本中心变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCostCenterChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出成本中心变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCostCenterChangeLogAsync(TaktCostCenterChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCostCenterChangeLogQueryDto());
        var list = await _costCenterChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostCenterChangeLogExportDto>(),
                sheetName ?? "成本中心变更记录数据",
                fileName ?? "成本中心变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCostCenterChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "成本中心变更记录数据",
            fileName ?? "成本中心变更记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建成本中心变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCostCenterChangeLog, bool>> QueryExpression(TaktCostCenterChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCostCenterChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.CostCenterId).Contains(keywords)
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterId == queryDto.CostCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeBy))
        {
            exp = exp.And(x => x.ChangeBy != null && x.ChangeBy.Contains(queryDto.ChangeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ChangeTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime >= queryDto.ChangeTimeStart);
        }

        if (queryDto?.ChangeTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime <= queryDto.ChangeTimeEnd);
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
