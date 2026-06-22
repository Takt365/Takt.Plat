// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptViewServiceBase.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变各部门视图服务基类（按 DeptCode 过滤 TaktEcDept + TaktEcDetail 合并展示）
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
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门视图服务基类
/// </summary>
public abstract class TaktEcDeptViewServiceBase : TaktServiceBase
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcDept> _ecDeptRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deptCode">固定部门编码</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecDeptRepository">设变部门仓储</param>
    /// <param name="lineNumberGenerator">行号生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    protected TaktEcDeptViewServiceBase(
        string deptCode,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        DeptCode = deptCode;
        _ecDetailRepository = ecDetailRepository;
        _ecDeptRepository = ecDeptRepository;
        _lineNumberGenerator = lineNumberGenerator;
    }

    /// <summary>
    /// 当前视图固定部门编码
    /// </summary>
    protected string DeptCode { get; }

    /// <summary>
    /// 获取部门视图列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDeptViewDto>> GetDeptViewListAsync(TaktEcDeptViewQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = BuildDetailQueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcNo,
            false);
        var detailIds = details.Select(x => x.Id).ToList();
        var deptMap = await LoadDeptMapAsync(detailIds);
        var rows = details.Select(d => MapDeptViewRow(d, deptMap.GetValueOrDefault(d.Id))).ToList();
        return TaktPagedResult<TaktEcDeptViewDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变明细 ID 获取部门视图行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>视图 DTO</returns>
    public async Task<TaktEcDeptViewDto?> GetDeptViewByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dept = await _ecDeptRepository.FirstAsync(x =>
            x.EcnDetailId == ecDetailId && x.DeptCode == DeptCode);
        return MapDeptViewRow(detail, dept);
    }

    /// <summary>
    /// 更新部门视图（不存在则创建部门记录）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>视图 DTO</returns>
    public async Task<TaktEcDeptViewDto> UpdateDeptViewAsync(long ecDetailId, TaktEcDeptViewUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        var dept = await _ecDeptRepository.FirstAsync(x =>
            x.EcnDetailId == ecDetailId && x.DeptCode == DeptCode);
        if (dept == null)
        {
            dept = new TaktEcDept
            {
                EcnDetailId = detail.Id,
                EcNo = detail.EcNo,
                DeptCode = DeptCode,
            };
            var maxLine = await _ecDeptRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == detail.Id,
                x => x.LineNumber);
            dept.LineNumber = _lineNumberGenerator.GenerateNext(detail.Id.ToString(), maxLine);
            dto.Adapt(dept);
            dept = await _ecDeptRepository.CreateAsync(dept);
        }
        else
        {
            dto.Adapt(dept);
            await _ecDeptRepository.UpdateAsync(dept);
        }
        return MapDeptViewRow(detail, dept);
    }

    /// <summary>
    /// 导出部门视图
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeptViewAsync(
        TaktEcDeptViewQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = BuildDetailQueryExpression(query ?? new TaktEcDeptViewQueryDto());
        var list = await _ecDetailRepository.GetListForExportAsync(predicate);
        var detailIds = list.Select(x => x.Id).ToList();
        var deptMap = await LoadDeptMapAsync(detailIds);
        var exportRows = list.Select(d => MapDeptViewRow(d, deptMap.GetValueOrDefault(d.Id))).ToList();
        return await TaktExcelHelper.ExportAsync(
            exportRows,
            sheetName ?? "设变部门视图",
            fileName ?? "设变部门视图导出.xlsx");
    }

    /// <summary>
    /// 批量加载部门记录映射
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>明细 ID 到部门记录映射</returns>
    private async Task<Dictionary<long, TaktEcDept>> LoadDeptMapAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, TaktEcDept>();
        }
        var depts = await _ecDeptRepository.GetListAsync(x =>
            detailIds.Contains(x.EcnDetailId) && x.DeptCode == DeptCode);
        return depts.ToDictionary(x => x.EcnDetailId);
    }

    /// <summary>
    /// 合并明细与部门记录为视图 DTO
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="dept">部门记录（可为空）</param>
    /// <returns>视图 DTO</returns>
    private TaktEcDeptViewDto MapDeptViewRow(TaktEcDetail detail, TaktEcDept? dept)
    {
        var dto = detail.Adapt<TaktEcDeptViewDto>();
        dto.EcDetailId = detail.Id;
        dto.DeptCode = DeptCode;
        if (dept != null)
        {
            dept.Adapt(dto);
            dto.EcDeptId = dept.Id;
            dto.DeptCode = dept.DeptCode;
        }
        return dto;
    }

    /// <summary>
    /// 构建设变明细查询表达式（含部门实施状态过滤）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktEcDetail, bool>> BuildDetailQueryExpression(TaktEcDeptViewQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        if (!string.IsNullOrEmpty(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcNo != null && x.EcNo.Contains(keywords))
                || (x.EcModel != null && x.EcModel.Contains(keywords))
                || (x.EcOldItem != null && x.EcOldItem.Contains(keywords))
                || (x.EcNewItem != null && x.EcNewItem.Contains(keywords))
                || (x.EcChange != null && x.EcChange.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }
        if (!string.IsNullOrEmpty(queryDto.EcModel))
        {
            exp = exp.And(x => x.EcModel != null && x.EcModel.Contains(queryDto.EcModel));
        }
        if (!string.IsNullOrEmpty(queryDto.EcOldItem))
        {
            exp = exp.And(x => x.EcOldItem != null && x.EcOldItem.Contains(queryDto.EcOldItem));
        }
        if (!string.IsNullOrEmpty(queryDto.EcNewItem))
        {
            exp = exp.And(x => x.EcNewItem != null && x.EcNewItem.Contains(queryDto.EcNewItem));
        }
        if (queryDto.IsImplemented.HasValue)
        {
            var flag = queryDto.IsImplemented.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcDept>()
                .Where(d => d.EcnDetailId == x.Id && d.DeptCode == DeptCode && d.IsImplemented == flag)
                .Any());
        }
        return exp.ToExpression();
    }
}
