// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktStandardOperationRateChangeLogService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率变更记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 标准生产稼动率变更记录应用服务
/// </summary>
public class TaktStandardOperationRateChangeLogService : TaktServiceBase, ITaktStandardOperationRateChangeLogService
{
    private readonly ITaktCompanyRepository<TaktStandardOperationRateChangeLog> _standardOperationRateChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktStandardOperationRate> _standardOperationRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationRateChangeLogRepository">标准生产稼动率变更记录仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStandardOperationRateChangeLogService(
        ITaktCompanyRepository<TaktStandardOperationRateChangeLog> standardOperationRateChangeLogRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _standardOperationRateChangeLogRepository = standardOperationRateChangeLogRepository;
        _standardOperationRateRepository = standardOperationRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取标准生产稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStandardOperationRateChangeLogDto>> GetStandardOperationRateChangeLogListAsync(TaktStandardOperationRateChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _standardOperationRateChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStandardOperationRateChangeLogDto>.Create(
            data.Adapt<List<TaktStandardOperationRateChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateChangeLogDto?> GetStandardOperationRateChangeLogByIdAsync(long id)
    {
        var entity = await _standardOperationRateChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktStandardOperationRateChangeLogDto>();
    }

    /// <summary>
    /// 获取标准生产稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStandardOperationRateChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _standardOperationRateChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建标准生产稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateChangeLogDto> CreateStandardOperationRateChangeLogAsync(TaktStandardOperationRateChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktStandardOperationRateChangeLog>();
        await StampStandardOperationRateChangeLogStandardOperationRateAsync(entity, dto);
        entity = await _standardOperationRateChangeLogRepository.CreateAsync(entity);
        return await GetStandardOperationRateChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktStandardOperationRateChangeLogDto>();
    }

    /// <summary>
    /// 更新标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateChangeLogDto> UpdateStandardOperationRateChangeLogAsync(long id, TaktStandardOperationRateChangeLogUpdateDto dto)
    {
        var entity = await _standardOperationRateChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准生产稼动率变更记录不存在");
        }
        dto.Adapt(entity);
        await StampStandardOperationRateChangeLogStandardOperationRateAsync(entity, dto);
        await _standardOperationRateChangeLogRepository.UpdateAsync(entity);
        return await GetStandardOperationRateChangeLogByIdAsync(id) ?? throw new TaktBusinessException("标准生产稼动率变更记录不存在");
    }

    /// <summary>
    /// 删除标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationRateChangeLogByIdAsync(long id)
    {
        var deleted = await _standardOperationRateChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("标准生产稼动率变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除标准生产稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationRateChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteStandardOperationRateChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出标准生产稼动率变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStandardOperationRateChangeLogAsync(TaktStandardOperationRateChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktStandardOperationRateChangeLogQueryDto());
        var list = await _standardOperationRateChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStandardOperationRateChangeLogExportDto>(),
                sheetName ?? "标准生产稼动率变更记录数据",
                fileName ?? "标准生产稼动率变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStandardOperationRateChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "标准生产稼动率变更记录数据",
            fileName ?? "标准生产稼动率变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步标准生产稼动率变更记录主表外键（ManyToOne → 标准生产稼动率）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampStandardOperationRateChangeLogStandardOperationRateAsync(TaktStandardOperationRateChangeLog entity, TaktStandardOperationRateChangeLogCreateDto dto)
    {
        if (dto.StandardOperationRateId <= 0)
        {
            return;
        }
        var master = await _standardOperationRateRepository.GetByIdAsync(dto.StandardOperationRateId);
        if (master == null)
        {
            throw new TaktBusinessException("标准生产稼动率不存在");
        }
        entity.StandardOperationRateId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建标准生产稼动率变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStandardOperationRateChangeLog, bool>> QueryExpression(TaktStandardOperationRateChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStandardOperationRateChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.StandardOperationRateId).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.StandardOperationRateId.HasValue == true)
        {
            exp = exp.And(x => x.StandardOperationRateId == queryDto.StandardOperationRateId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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
