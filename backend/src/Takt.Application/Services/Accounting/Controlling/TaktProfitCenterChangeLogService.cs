// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktProfitCenterChangeLogService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心变更记录应用服务实现
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
/// 利润中心变更记录应用服务
/// </summary>
public class TaktProfitCenterChangeLogService : TaktServiceBase, ITaktProfitCenterChangeLogService
{
    private readonly ITaktCompanyRepository<TaktProfitCenterChangeLog> _profitCenterChangeLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitCenterChangeLogRepository">利润中心变更记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProfitCenterChangeLogService(
        ITaktCompanyRepository<TaktProfitCenterChangeLog> profitCenterChangeLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _profitCenterChangeLogRepository = profitCenterChangeLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取利润中心变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProfitCenterChangeLogDto>> GetProfitCenterChangeLogListAsync(TaktProfitCenterChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _profitCenterChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProfitCenterChangeLogDto>.Create(
            data.Adapt<List<TaktProfitCenterChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterChangeLogDto?> GetProfitCenterChangeLogByIdAsync(long id)
    {
        var entity = await _profitCenterChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProfitCenterChangeLogDto>();
    }

    /// <summary>
    /// 获取利润中心变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProfitCenterChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _profitCenterChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProfitCenterCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProfitCenterCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建利润中心变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterChangeLogDto> CreateProfitCenterChangeLogAsync(TaktProfitCenterChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktProfitCenterChangeLog>();
        entity = await _profitCenterChangeLogRepository.CreateAsync(entity);
        return await GetProfitCenterChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktProfitCenterChangeLogDto>();
    }

    /// <summary>
    /// 更新利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterChangeLogDto> UpdateProfitCenterChangeLogAsync(long id, TaktProfitCenterChangeLogUpdateDto dto)
    {
        var entity = await _profitCenterChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("利润中心变更记录不存在");
        }
        dto.Adapt(entity);
        await _profitCenterChangeLogRepository.UpdateAsync(entity);
        return await GetProfitCenterChangeLogByIdAsync(id) ?? throw new TaktBusinessException("利润中心变更记录不存在");
    }

    /// <summary>
    /// 删除利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitCenterChangeLogByIdAsync(long id)
    {
        var deleted = await _profitCenterChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("利润中心变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除利润中心变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitCenterChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProfitCenterChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出利润中心变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProfitCenterChangeLogAsync(TaktProfitCenterChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProfitCenterChangeLogQueryDto());
        var list = await _profitCenterChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProfitCenterChangeLogExportDto>(),
                sheetName ?? "利润中心变更记录数据",
                fileName ?? "利润中心变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProfitCenterChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "利润中心变更记录数据",
            fileName ?? "利润中心变更记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建利润中心变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProfitCenterChangeLog, bool>> QueryExpression(TaktProfitCenterChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProfitCenterChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ProfitCenterId).Contains(keywords)
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ProfitCenterId.HasValue == true)
        {
            exp = exp.And(x => x.ProfitCenterId == queryDto.ProfitCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenterCode))
        {
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(queryDto.ProfitCenterCode));
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
