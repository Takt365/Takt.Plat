// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBatchService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变投入批次应用服务实现（生管预定批次 + 制二生产批次）
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
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变投入批次应用服务
/// </summary>
public class TaktEcBatchService : TaktServiceBase, ITaktEcBatchService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecExecDeptAccess">设变部门执行跨表访问</param>
    /// <param name="lineNumberGenerator">行号生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcBatchService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        TaktEcExecDeptAccess ecExecDeptAccess,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
        _ecExecDeptAccess = ecExecDeptAccess;
        _lineNumberGenerator = lineNumberGenerator;
    }

    /// <summary>
    /// 获取投入批次列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcBatchDto>> GetEcBatchListAsync(TaktEcBatchQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var rows = new List<TaktEcBatchDto>();
        foreach (var detail in details)
        {
            rows.Add(await MapBatchRowAsync(detail));
        }
        return TaktPagedResult<TaktEcBatchDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变明细 ID 获取投入批次行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>批次 DTO</returns>
    public async Task<TaktEcBatchDto?> GetEcBatchByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await MapBatchRowAsync(detail);
    }

    /// <summary>
    /// 更新投入批次
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>批次 DTO</returns>
    public async Task<TaktEcBatchDto> UpdateEcBatchAsync(long ecDetailId, TaktEcBatchUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        await UpsertPmcBatchFieldAsync(detail, pmc =>
        {
            pmc.ScheduledBatch = dto.ScheduledBatch;
            pmc.ScheduledProductionDate = dto.ScheduledProductionDate;
        });
        await UpsertPcbaBatchFieldAsync(detail, pcba =>
        {
            pcba.ProductionBatch = dto.ProductionBatch;
            pcba.ProductionDate = dto.ProductionDate;
        });
        return await MapBatchRowAsync(detail);
    }

    /// <summary>
    /// 导出投入批次
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcBatchAsync(
        TaktEcBatchQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktEcBatchQueryDto());
        var list = await _ecDetailRepository.GetListForExportAsync(predicate);
        var rows = new List<TaktEcBatchDto>();
        foreach (var detail in list)
        {
            rows.Add(await MapBatchRowAsync(detail));
        }
        return await TaktExcelHelper.ExportAsync(
            rows,
            sheetName ?? "投入批次",
            fileName ?? "投入批次导出.xlsx");
    }

    /// <summary>
    /// 映射投入批次行
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>批次 DTO</returns>
    private async Task<TaktEcBatchDto> MapBatchRowAsync(TaktEcDetail detail)
    {
        var dto = detail.Adapt<TaktEcBatchDto>();
        var pmc = await _ecExecDeptAccess.PmcRepository.FirstAsync(x => x.EcnDetailId == detail.Id);
        var pcba = await _ecExecDeptAccess.PcbaRepository.FirstAsync(x => x.EcnDetailId == detail.Id);
        if (pmc != null)
        {
            dto.ScheduledBatch = pmc.ScheduledBatch;
            dto.ScheduledProductionDate = pmc.ScheduledProductionDate;
        }
        if (pcba != null)
        {
            dto.ProductionBatch = pcba.ProductionBatch;
            dto.ProductionDate = pcba.ProductionDate;
        }
        return dto;
    }

    /// <summary>
    /// 更新或创建生管批次字段
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="apply">字段赋值</param>
    /// <returns>任务</returns>
    private async Task UpsertPmcBatchFieldAsync(TaktEcDetail detail, Action<TaktEcSeikan> apply)
    {
        var pmcRepo = _ecExecDeptAccess.PmcRepository;
        var pmc = await pmcRepo.FirstAsync(x => x.EcnDetailId == detail.Id);
        if (pmc == null)
        {
            pmc = new TaktEcSeikan
            {
                EcnDetailId = detail.Id,
                EcCode = detail.EcCode,
                DeptCode = TaktEcDeptCodes.Pmc,
            };
            var maxLine = await pmcRepo.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == detail.Id,
                x => x.LineNumber);
            pmc.LineNumber = _lineNumberGenerator.GenerateNext(detail.Id.ToString(), maxLine);
            apply(pmc);
            await pmcRepo.CreateAsync(pmc);
            return;
        }
        apply(pmc);
        await pmcRepo.UpdateAsync(pmc);
    }

    /// <summary>
    /// 更新或创建制二批次字段
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="apply">字段赋值</param>
    /// <returns>任务</returns>
    private async Task UpsertPcbaBatchFieldAsync(TaktEcDetail detail, Action<TaktEcSeizounika> apply)
    {
        var pcbaRepo = _ecExecDeptAccess.PcbaRepository;
        var pcba = await pcbaRepo.FirstAsync(x => x.EcnDetailId == detail.Id);
        if (pcba == null)
        {
            pcba = new TaktEcSeizounika
            {
                EcnDetailId = detail.Id,
                EcCode = detail.EcCode,
                DeptCode = TaktEcDeptCodes.Pcba,
            };
            var maxLine = await pcbaRepo.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == detail.Id,
                x => x.LineNumber);
            pcba.LineNumber = _lineNumberGenerator.GenerateNext(detail.Id.ToString(), maxLine);
            apply(pcba);
            await pcbaRepo.CreateAsync(pcba);
            return;
        }
        apply(pcba);
        await pcbaRepo.UpdateAsync(pcba);
    }

    /// <summary>
    /// 构建投入批次查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> QueryExpression(TaktEcBatchQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModel != null && x.EcModel.Contains(keywords))
                || (x.EcNewItem != null && x.EcNewItem.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcModel))
        {
            exp = exp.And(x => x.EcModel != null && x.EcModel.Contains(queryDto.EcModel));
        }
        if (!string.IsNullOrEmpty(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x =>
                SqlFunc.Subqueryable<TaktEcSeikan>()
                    .Where(d => d.EcnDetailId == x.Id && d.ScheduledBatch != null && d.ScheduledBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcSeizounika>()
                    .Where(d => d.EcnDetailId == x.Id && d.ProductionBatch != null && d.ProductionBatch.Contains(batchCode))
                    .Any());
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }
}
