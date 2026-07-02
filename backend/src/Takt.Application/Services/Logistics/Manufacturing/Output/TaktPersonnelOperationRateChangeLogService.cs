// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPersonnelOperationRateChangeLogService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率变更记录应用服务实现
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
/// 人员稼动率变更记录应用服务
/// </summary>
public class TaktPersonnelOperationRateChangeLogService : TaktServiceBase, ITaktPersonnelOperationRateChangeLogService
{
    private readonly ITaktCompanyRepository<TaktPersonnelOperationRateChangeLog> _personnelOperationRateChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktPersonnelOperationRate> _personnelOperationRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="personnelOperationRateChangeLogRepository">人员稼动率变更记录仓储</param>
    /// <param name="personnelOperationRateRepository">人员稼动率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPersonnelOperationRateChangeLogService(
        ITaktCompanyRepository<TaktPersonnelOperationRateChangeLog> personnelOperationRateChangeLogRepository,
        ITaktCompanyRepository<TaktPersonnelOperationRate> personnelOperationRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _personnelOperationRateChangeLogRepository = personnelOperationRateChangeLogRepository;
        _personnelOperationRateRepository = personnelOperationRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取人员稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPersonnelOperationRateChangeLogDto>> GetPersonnelOperationRateChangeLogListAsync(TaktPersonnelOperationRateChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _personnelOperationRateChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPersonnelOperationRateChangeLogDto>.Create(
            data.Adapt<List<TaktPersonnelOperationRateChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateChangeLogDto?> GetPersonnelOperationRateChangeLogByIdAsync(long id)
    {
        var entity = await _personnelOperationRateChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPersonnelOperationRateChangeLogDto>();
    }

    /// <summary>
    /// 获取人员稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPersonnelOperationRateChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _personnelOperationRateChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdTeam ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProdTeam ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建人员稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateChangeLogDto> CreatePersonnelOperationRateChangeLogAsync(TaktPersonnelOperationRateChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktPersonnelOperationRateChangeLog>();
        await StampPersonnelOperationRateChangeLogPersonnelOperationRateAsync(entity, dto);
        entity = await _personnelOperationRateChangeLogRepository.CreateAsync(entity);
        return await GetPersonnelOperationRateChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktPersonnelOperationRateChangeLogDto>();
    }

    /// <summary>
    /// 更新人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateChangeLogDto> UpdatePersonnelOperationRateChangeLogAsync(long id, TaktPersonnelOperationRateChangeLogUpdateDto dto)
    {
        var entity = await _personnelOperationRateChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("人员稼动率变更记录不存在");
        }
        dto.Adapt(entity);
        await StampPersonnelOperationRateChangeLogPersonnelOperationRateAsync(entity, dto);
        await _personnelOperationRateChangeLogRepository.UpdateAsync(entity);
        return await GetPersonnelOperationRateChangeLogByIdAsync(id) ?? throw new TaktBusinessException("人员稼动率变更记录不存在");
    }

    /// <summary>
    /// 删除人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeletePersonnelOperationRateChangeLogByIdAsync(long id)
    {
        var deleted = await _personnelOperationRateChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("人员稼动率变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除人员稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePersonnelOperationRateChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePersonnelOperationRateChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出人员稼动率变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPersonnelOperationRateChangeLogAsync(TaktPersonnelOperationRateChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPersonnelOperationRateChangeLogQueryDto());
        var list = await _personnelOperationRateChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPersonnelOperationRateChangeLogExportDto>(),
                sheetName ?? "人员稼动率变更记录数据",
                fileName ?? "人员稼动率变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPersonnelOperationRateChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "人员稼动率变更记录数据",
            fileName ?? "人员稼动率变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步人员稼动率变更记录主表外键（ManyToOne → 人员稼动率）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPersonnelOperationRateChangeLogPersonnelOperationRateAsync(TaktPersonnelOperationRateChangeLog entity, TaktPersonnelOperationRateChangeLogCreateDto dto)
    {
        if (dto.PersonnelOperationRateId <= 0)
        {
            return;
        }
        var master = await _personnelOperationRateRepository.GetByIdAsync(dto.PersonnelOperationRateId);
        if (master == null)
        {
            throw new TaktBusinessException("人员稼动率不存在");
        }
        entity.PersonnelOperationRateId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建人员稼动率变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPersonnelOperationRateChangeLog, bool>> QueryExpression(TaktPersonnelOperationRateChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPersonnelOperationRateChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PersonnelOperationRateId).Contains(keywords)
                || (x.ProdTeam != null && x.ProdTeam.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PersonnelOperationRateId.HasValue == true)
        {
            exp = exp.And(x => x.PersonnelOperationRateId == queryDto.PersonnelOperationRateId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdTeam))
        {
            exp = exp.And(x => x.ProdTeam != null && x.ProdTeam.Contains(queryDto.ProdTeam));
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
