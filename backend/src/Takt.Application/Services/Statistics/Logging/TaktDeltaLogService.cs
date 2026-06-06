// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktDeltaLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：差异日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 差异日志应用服务
/// </summary>
public class TaktDeltaLogService : TaktServiceBase, ITaktDeltaLogService
{
    private readonly ITaktCompanyRepository<TaktDeltaLog> _deltaLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deltaLogRepository">差异日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDeltaLogService(
        ITaktCompanyRepository<TaktDeltaLog> deltaLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _deltaLogRepository = deltaLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取差异日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDeltaLogDto>> GetDeltaLogListAsync(TaktDeltaLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _deltaLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDeltaLogDto>.Create(
            data.Adapt<List<TaktDeltaLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto?> GetDeltaLogByIdAsync(long id)
    {
        var entity = await _deltaLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktDeltaLogDto>();
    }

    /// <summary>
    /// 获取差异日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDeltaLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _deltaLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.UserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建差异日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto> CreateDeltaLogAsync(TaktDeltaLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktDeltaLog>();
        entity = await _deltaLogRepository.CreateAsync(entity);
        return await GetDeltaLogByIdAsync(entity.Id) ?? entity.Adapt<TaktDeltaLogDto>();
    }

    /// <summary>
    /// 更新差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto> UpdateDeltaLogAsync(long id, TaktDeltaLogUpdateDto dto)
    {
        var entity = await _deltaLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("差异日志不存在");
        }
        dto.Adapt(entity);
        await _deltaLogRepository.UpdateAsync(entity);
        return await GetDeltaLogByIdAsync(id) ?? throw new TaktBusinessException("差异日志不存在");
    }

    /// <summary>
    /// 删除差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDeltaLogByIdAsync(long id)
    {
        var deleted = await _deltaLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("差异日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除差异日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDeltaLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDeltaLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出差异日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeltaLogAsync(TaktDeltaLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDeltaLogQueryDto());
        var list = await _deltaLogRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDeltaLogExportDto>(),
                sheetName ?? "差异日志数据",
                fileName ?? "差异日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDeltaLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "差异日志数据",
            fileName ?? "差异日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建差异日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDeltaLog, bool>> QueryExpression(TaktDeltaLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDeltaLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.UserName != null && x.UserName.Contains(keywords))
                || (x.OperType != null && x.OperType.Contains(keywords))
                || (x.TableName != null && x.TableName.Contains(keywords))
                || SqlFunc.ToString(x.PrimaryKeyId).Contains(keywords)
                || (x.BeforeData != null && x.BeforeData.Contains(keywords))
                || (x.AfterData != null && x.AfterData.Contains(keywords))
                || (x.DiffData != null && x.DiffData.Contains(keywords))
                || (x.SqlStatement != null && x.SqlStatement.Contains(keywords))
                || (x.OperIp != null && x.OperIp.Contains(keywords))
                || (x.OperLocation != null && x.OperLocation.Contains(keywords))
                || SqlFunc.ToString(x.ElapsedTime).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OperTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperType))
        {
            exp = exp.And(x => x.OperType != null && x.OperType.Contains(queryDto.OperType));
        }

        if (!string.IsNullOrEmpty(queryDto?.TableName))
        {
            exp = exp.And(x => x.TableName != null && x.TableName.Contains(queryDto.TableName));
        }

        if (queryDto?.PrimaryKeyId.HasValue == true)
        {
            exp = exp.And(x => x.PrimaryKeyId == queryDto.PrimaryKeyId);
        }

        if (!string.IsNullOrEmpty(queryDto?.BeforeData))
        {
            exp = exp.And(x => x.BeforeData != null && x.BeforeData.Contains(queryDto.BeforeData));
        }

        if (!string.IsNullOrEmpty(queryDto?.AfterData))
        {
            exp = exp.And(x => x.AfterData != null && x.AfterData.Contains(queryDto.AfterData));
        }

        if (!string.IsNullOrEmpty(queryDto?.DiffData))
        {
            exp = exp.And(x => x.DiffData != null && x.DiffData.Contains(queryDto.DiffData));
        }

        if (!string.IsNullOrEmpty(queryDto?.SqlStatement))
        {
            exp = exp.And(x => x.SqlStatement != null && x.SqlStatement.Contains(queryDto.SqlStatement));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperIp))
        {
            exp = exp.And(x => x.OperIp != null && x.OperIp.Contains(queryDto.OperIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperLocation))
        {
            exp = exp.And(x => x.OperLocation != null && x.OperLocation.Contains(queryDto.OperLocation));
        }

        if (queryDto?.ElapsedTime.HasValue == true)
        {
            exp = exp.And(x => x.ElapsedTime == queryDto.ElapsedTime);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.OperTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.OperTime >= queryDto.OperTimeStart);
        }

        if (queryDto?.OperTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.OperTime <= queryDto.OperTimeEnd);
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
