// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeChangeLogService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工序时间变更记录应用服务实现
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
/// 标准工序时间变更记录应用服务
/// </summary>
public class TaktStandardOperationTimeChangeLogService : TaktServiceBase, ITaktStandardOperationTimeChangeLogService
{
    private readonly ITaktCompanyRepository<TaktStandardOperationTimeChangeLog> _standardOperationTimeChangeLogRepository;
    private readonly ITaktApprovalRepository<TaktStandardOperationTime> _standardOperationTimeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationTimeChangeLogRepository">标准工序时间变更记录仓储</param>
    /// <param name="standardOperationTimeRepository">标准工序时间仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStandardOperationTimeChangeLogService(
        ITaktCompanyRepository<TaktStandardOperationTimeChangeLog> standardOperationTimeChangeLogRepository,
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _standardOperationTimeChangeLogRepository = standardOperationTimeChangeLogRepository;
        _standardOperationTimeRepository = standardOperationTimeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取标准工序时间变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStandardOperationTimeChangeLogDto>> GetStandardOperationTimeChangeLogListAsync(TaktStandardOperationTimeChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _standardOperationTimeChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStandardOperationTimeChangeLogDto>.Create(
            data.Adapt<List<TaktStandardOperationTimeChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeChangeLogDto?> GetStandardOperationTimeChangeLogByIdAsync(long id)
    {
        var entity = await _standardOperationTimeChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktStandardOperationTimeChangeLogDto>();
    }

    /// <summary>
    /// 获取标准工序时间变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStandardOperationTimeChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _standardOperationTimeChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建标准工序时间变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeChangeLogDto> CreateStandardOperationTimeChangeLogAsync(TaktStandardOperationTimeChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktStandardOperationTimeChangeLog>();
        await StampStandardOperationTimeChangeLogStandardOperationTimeAsync(entity, dto);
        entity = await _standardOperationTimeChangeLogRepository.CreateAsync(entity);
        return await GetStandardOperationTimeChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktStandardOperationTimeChangeLogDto>();
    }

    /// <summary>
    /// 更新标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeChangeLogDto> UpdateStandardOperationTimeChangeLogAsync(long id, TaktStandardOperationTimeChangeLogUpdateDto dto)
    {
        var entity = await _standardOperationTimeChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准工序时间变更记录不存在");
        }
        dto.Adapt(entity);
        await StampStandardOperationTimeChangeLogStandardOperationTimeAsync(entity, dto);
        await _standardOperationTimeChangeLogRepository.UpdateAsync(entity);
        return await GetStandardOperationTimeChangeLogByIdAsync(id) ?? throw new TaktBusinessException("标准工序时间变更记录不存在");
    }

    /// <summary>
    /// 删除标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationTimeChangeLogByIdAsync(long id)
    {
        var deleted = await _standardOperationTimeChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("标准工序时间变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除标准工序时间变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationTimeChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteStandardOperationTimeChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出标准工序时间变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStandardOperationTimeChangeLogAsync(TaktStandardOperationTimeChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktStandardOperationTimeChangeLogQueryDto());
        var list = await _standardOperationTimeChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStandardOperationTimeChangeLogExportDto>(),
                sheetName ?? "标准工序时间变更记录数据",
                fileName ?? "标准工序时间变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStandardOperationTimeChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "标准工序时间变更记录数据",
            fileName ?? "标准工序时间变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步标准工序时间变更记录主表外键（ManyToOne → 标准工序时间）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampStandardOperationTimeChangeLogStandardOperationTimeAsync(TaktStandardOperationTimeChangeLog entity, TaktStandardOperationTimeChangeLogCreateDto dto)
    {
        if (dto.StandardOperationTimeId <= 0)
        {
            return;
        }
        var master = await _standardOperationTimeRepository.GetByIdAsync(dto.StandardOperationTimeId);
        if (master == null)
        {
            throw new TaktBusinessException("标准工序时间不存在");
        }
        entity.StandardOperationTimeId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建标准工序时间变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStandardOperationTimeChangeLog, bool>> QueryExpression(TaktStandardOperationTimeChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStandardOperationTimeChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.StandardOperationTimeId).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.StandardOperationTimeId.HasValue == true)
        {
            exp = exp.And(x => x.StandardOperationTimeId == queryDto.StandardOperationTimeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
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
