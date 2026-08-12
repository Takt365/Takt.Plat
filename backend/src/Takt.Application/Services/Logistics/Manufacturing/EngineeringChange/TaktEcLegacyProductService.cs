// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcLegacyProductService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变旧品管制应用服务实现
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
/// 设变旧品管制应用服务
/// </summary>
public class TaktEcLegacyProductService : TaktServiceBase, ITaktEcLegacyProductService
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
    public TaktEcLegacyProductService(
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
    /// 获取旧品管制列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcLegacyProductDto>> GetEcLegacyProductListAsync(TaktEcLegacyProductQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var rows = new List<TaktEcLegacyProductDto>();
        foreach (var detail in details)
        {
            rows.Add(await MapLegacyProductRowAsync(detail));
        }
        return TaktPagedResult<TaktEcLegacyProductDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变明细 ID 获取旧品管制行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>旧品管制 DTO</returns>
    public async Task<TaktEcLegacyProductDto?> GetEcLegacyProductByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await MapLegacyProductRowAsync(detail);
    }

    /// <summary>
    /// 更新旧品管制
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>旧品管制 DTO</returns>
    public async Task<TaktEcLegacyProductDto> UpdateEcLegacyProductAsync(long ecDetailId, TaktEcLegacyProductUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        detail.IsEndOfLine = dto.IsEndOfLine;
        detail.Remark = dto.Remark;
        await _ecDetailRepository.UpdateAsync(detail);
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
            pmc.OldProductHandling = dto.OldProductHandling;
            await pmcRepo.CreateAsync(pmc);
        }
        else
        {
            pmc.OldProductHandling = dto.OldProductHandling;
            await pmcRepo.UpdateAsync(pmc);
        }
        return await MapLegacyProductRowAsync(detail);
    }

    /// <summary>
    /// 导出旧品管制
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcLegacyProductAsync(
        TaktEcLegacyProductQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktEcLegacyProductQueryDto());
        var list = await _ecDetailRepository.GetListForExportAsync(predicate);
        var rows = new List<TaktEcLegacyProductDto>();
        foreach (var detail in list)
        {
            rows.Add(await MapLegacyProductRowAsync(detail));
        }
        return await TaktExcelHelper.ExportAsync(
            rows,
            sheetName ?? "旧品管制",
            fileName ?? "旧品管制导出.xlsx");
    }

    /// <summary>
    /// 映射旧品管制行
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>旧品管制 DTO</returns>
    private async Task<TaktEcLegacyProductDto> MapLegacyProductRowAsync(TaktEcDetail detail)
    {
        var dto = detail.Adapt<TaktEcLegacyProductDto>();
        var pmc = await _ecExecDeptAccess.PmcRepository.FirstAsync(x => x.EcnDetailId == detail.Id);
        dto.OldProductHandling = pmc?.OldProductHandling;
        return dto;
    }

    /// <summary>
    /// 构建旧品管制查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> QueryExpression(TaktEcLegacyProductQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcOldItem != null && x.EcOldItem != string.Empty);
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModel != null && x.EcModel.Contains(keywords))
                || (x.EcOldItem != null && x.EcOldItem.Contains(keywords))
                || (x.EcOldText != null && x.EcOldText.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcModel))
        {
            exp = exp.And(x => x.EcModel != null && x.EcModel.Contains(queryDto.EcModel));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcOldItem))
        {
            exp = exp.And(x => x.EcOldItem != null && x.EcOldItem.Contains(queryDto.EcOldItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }
}
