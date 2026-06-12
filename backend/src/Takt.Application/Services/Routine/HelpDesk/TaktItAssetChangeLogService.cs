// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktItAssetChangeLogService.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：IT设备保修变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// IT设备保修变更日志应用服务
/// </summary>
public class TaktItAssetChangeLogService : TaktServiceBase, ITaktItAssetChangeLogService
{
    private readonly ITaktCompanyRepository<TaktItAssetChangeLog> _itAssetChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktItAsset> _itAssetRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="itAssetChangeLogRepository">IT设备保修变更日志仓储</param>
    /// <param name="itAssetRepository">IT设备保修扩展仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktItAssetChangeLogService(
        ITaktCompanyRepository<TaktItAssetChangeLog> itAssetChangeLogRepository,
        ITaktCompanyRepository<TaktItAsset> itAssetRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _itAssetChangeLogRepository = itAssetChangeLogRepository;
        _itAssetRepository = itAssetRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取IT设备保修变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktItAssetChangeLogDto>> GetItAssetChangeLogListAsync(TaktItAssetChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _itAssetChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktItAssetChangeLogDto>.Create(
            data.Adapt<List<TaktItAssetChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetChangeLogDto?> GetItAssetChangeLogByIdAsync(long id)
    {
        var entity = await _itAssetChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktItAssetChangeLogDto>();
    }

    /// <summary>
    /// 获取IT设备保修变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetItAssetChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _itAssetChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AssetCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AssetCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建IT设备保修变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetChangeLogDto> CreateItAssetChangeLogAsync(TaktItAssetChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktItAssetChangeLog>();
        await StampItAssetChangeLogItAssetAsync(entity, dto);
        entity = await _itAssetChangeLogRepository.CreateAsync(entity);
        return await GetItAssetChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktItAssetChangeLogDto>();
    }

    /// <summary>
    /// 更新IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktItAssetChangeLogDto> UpdateItAssetChangeLogAsync(long id, TaktItAssetChangeLogUpdateDto dto)
    {
        var entity = await _itAssetChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("IT设备保修变更日志不存在");
        }
        dto.Adapt(entity);
        await StampItAssetChangeLogItAssetAsync(entity, dto);
        await _itAssetChangeLogRepository.UpdateAsync(entity);
        return await GetItAssetChangeLogByIdAsync(id) ?? throw new TaktBusinessException("IT设备保修变更日志不存在");
    }

    /// <summary>
    /// 删除IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteItAssetChangeLogByIdAsync(long id)
    {
        var deleted = await _itAssetChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("IT设备保修变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除IT设备保修变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteItAssetChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteItAssetChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出IT设备保修变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportItAssetChangeLogAsync(TaktItAssetChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktItAssetChangeLogQueryDto());
        var list = await _itAssetChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktItAssetChangeLogExportDto>(),
                sheetName ?? "IT设备保修变更日志数据",
                fileName ?? "IT设备保修变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktItAssetChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "IT设备保修变更日志数据",
            fileName ?? "IT设备保修变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步IT设备保修变更日志主表外键（ManyToOne → IT设备保修扩展）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampItAssetChangeLogItAssetAsync(TaktItAssetChangeLog entity, TaktItAssetChangeLogCreateDto dto)
    {
        if (dto.ItAssetId <= 0)
        {
            return;
        }
        var master = await _itAssetRepository.GetByIdAsync(dto.ItAssetId);
        if (master == null)
        {
            throw new TaktBusinessException("IT设备保修扩展不存在");
        }
        entity.ItAssetId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建IT设备保修变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktItAssetChangeLog, bool>> QueryExpression(TaktItAssetChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktItAssetChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ItAssetId).Contains(keywords)
                || (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || SqlFunc.ToString(x.ChangeType).Contains(keywords)
                || (x.ChangeSummary != null && x.ChangeSummary.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ItAssetId.HasValue == true)
        {
            exp = exp.And(x => x.ItAssetId == queryDto.ItAssetId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetCode))
        {
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(queryDto.AssetCode));
        }

        if (queryDto?.ChangeType.HasValue == true)
        {
            exp = exp.And(x => x.ChangeType == queryDto.ChangeType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeSummary))
        {
            exp = exp.And(x => x.ChangeSummary != null && x.ChangeSummary.Contains(queryDto.ChangeSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
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
