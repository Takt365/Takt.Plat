// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKanbanService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板应用服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变看板应用服务
/// </summary>
public class TaktEcKanbanService : TaktServiceBase, ITaktEcKanbanService
{
    private readonly ITaktCompanyRepository<TaktEc> _ecRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcDept> _ecDeptRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecRepository">设变主仓储</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecDeptRepository">设变部门仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcKanbanService(
        ITaktCompanyRepository<TaktEc> ecRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecRepository = ecRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecDeptRepository = ecDeptRepository;
    }

    /// <summary>
    /// 获取设变看板列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcKanbanDto>> GetEcKanbanListAsync(TaktEcKanbanQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var (ecs, total) = await _ecRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcNo,
            false);
        var rows = new List<TaktEcKanbanDto>();
        foreach (var ec in ecs)
        {
            rows.Add(await BuildKanbanRowAsync(ec));
        }
        return TaktPagedResult<TaktEcKanbanDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变主表 ID 获取看板行
    /// </summary>
    /// <param name="ecId">设变主表 ID</param>
    /// <returns>看板 DTO</returns>
    public async Task<TaktEcKanbanDto?> GetEcKanbanByEcIdAsync(long ecId)
    {
        EnsureThreeLayerContext();
        var ec = await _ecRepository.GetByIdAsync(ecId);
        if (ec == null || ec.TenantCode != CurrentTenantCode || ec.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await BuildKanbanRowAsync(ec);
    }

    /// <summary>
    /// 导出设变看板
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcKanbanAsync(
        TaktEcKanbanQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktEcKanbanQueryDto());
        var list = await _ecRepository.GetListForExportAsync(predicate);
        var rows = new List<TaktEcKanbanDto>();
        foreach (var ec in list)
        {
            rows.Add(await BuildKanbanRowAsync(ec));
        }
        return await TaktExcelHelper.ExportAsync(
            rows,
            sheetName ?? "设变看板",
            fileName ?? "设变看板导出.xlsx");
    }

    /// <summary>
    /// 构建设变看板行
    /// </summary>
    /// <param name="ec">设变主表</param>
    /// <returns>看板 DTO</returns>
    private async Task<TaktEcKanbanDto> BuildKanbanRowAsync(TaktEc ec)
    {
        var dto = ec.Adapt<TaktEcKanbanDto>();
        var details = await _ecDetailRepository.GetListAsync(x => x.EcId == ec.Id);
        dto.DetailCount = details.Count;
        var detailIds = details.Select(x => x.Id).ToList();
        var depts = detailIds.Count == 0
            ? []
            : await _ecDeptRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId));
        dto.DeptStages = TaktEcDeptCodes.KanbanOrder.Select(code =>
        {
            var matched = depts.Where(d => d.DeptCode == code).ToList();
            return new TaktEcKanbanDeptStageDto
            {
                DeptCode = code,
                TotalCount = details.Count,
                ImplementedCount = matched.Count(d => d.IsImplemented == 1),
            };
        }).ToList();
        return dto;
    }

    /// <summary>
    /// 构建看板查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEc, bool>> QueryExpression(TaktEcKanbanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEc>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcNo != null && x.EcNo.Contains(keywords))
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcLeader != null && x.EcLeader.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }
        if (queryDto?.ChangeStatus.HasValue == true)
        {
            exp = exp.And(x => x.ChangeStatus == queryDto.ChangeStatus);
        }
        if (queryDto?.EcStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcStatus == queryDto.EcStatus);
        }
        return exp.ToExpression();
    }
}
