// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecDeptAccess.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行跨表访问（8 张 TaktEcExec* 部门表，按 DeptCode 路由）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行公共字段快照（跨 8 张子表聚合用）
/// </summary>
public sealed class TaktEcExecBaseRow
{
    /// <summary>主键</summary>
    public long Id { get; init; }
    /// <summary>设变明细 ID</summary>
    public long EcnDetailId { get; init; }
    /// <summary>设变单号</summary>
    public string EcCode { get; init; } = string.Empty;
    /// <summary>部门编码</summary>
    public string DeptCode { get; init; } = string.Empty;
    /// <summary>行号</summary>
    public int LineNumber { get; init; }
    /// <summary>是否实施</summary>
    public int IsImplemented { get; init; }
    /// <summary>执行内容</summary>
    public string? ExecContent { get; init; }
    /// <summary>租户编码</summary>
    public string TenantCode { get; init; } = string.Empty;
    /// <summary>公司代码</summary>
    public string CompanyCode { get; init; } = string.Empty;
}

/// <summary>
/// 设变部门执行跨表访问
/// </summary>
public class TaktEcExecDeptAccess
{
    private readonly ITaktCompanyRepository<TaktEcSeikan> _pmcRepository;
    private readonly ITaktCompanyRepository<TaktEcKoubai> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcUkeken> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcBukan> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizounika> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizouikka> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcHinkan> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizougijutsu> _teRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecDeptAccess(
        ITaktCompanyRepository<TaktEcSeikan> pmcRepository,
        ITaktCompanyRepository<TaktEcKoubai> mpRepository,
        ITaktCompanyRepository<TaktEcUkeken> iqcRepository,
        ITaktCompanyRepository<TaktEcBukan> mcRepository,
        ITaktCompanyRepository<TaktEcSeizounika> pcbaRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository)
    {
        _pmcRepository = pmcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _pcbaRepository = pcbaRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
    }

    /// <summary>生管课仓储</summary>
    public ITaktCompanyRepository<TaktEcSeikan> PmcRepository => _pmcRepository;

    /// <summary>制二课仓储</summary>
    public ITaktCompanyRepository<TaktEcSeizounika> PcbaRepository => _pcbaRepository;

    /// <summary>
    /// 按设变单号与部门取首条执行记录（公共字段）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByEcCodeAndDeptAsync(string ecCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => ToBaseRow(await _pmcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mp => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Pcba => ToBaseRow(await _pcbaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Assy => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Qa => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Te => ToBaseRow(await _teRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            _ => null
        };
    }

    /// <summary>
    /// 按明细 ID 与部门取执行记录（公共字段）
    /// </summary>
    /// <param name="ecnDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByDetailAndDeptAsync(long ecnDetailId, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => ToBaseRow(await _pmcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mp => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Pcba => ToBaseRow(await _pcbaRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Assy => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Qa => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            TaktEcDeptCodes.Te => ToBaseRow(await _teRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId && x.IsDeleted == 0)),
            _ => null
        };
    }

    /// <summary>
    /// 按明细 ID 列表聚合全部部门执行行（公共字段）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcnDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return [];
        }
        var rows = new List<TaktEcExecBaseRow>();
        rows.AddRange((await _pmcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mpRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _iqcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _mcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _pcbaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _assyRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _qaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        rows.AddRange((await _teRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId) && x.IsDeleted == 0)).Select(ToBaseRow).Where(x => x != null)!);
        return rows;
    }

    /// <summary>
    /// 按明细 ID 聚合全部部门执行行（公共字段）
    /// </summary>
    /// <param name="ecnDetailId">明细 ID</param>
    /// <returns>执行行列表</returns>
    public Task<List<TaktEcExecBaseRow>> ListBaseByEcnDetailIdAsync(long ecnDetailId) =>
        ListBaseByEcnDetailIdsAsync([ecnDetailId]);

    /// <summary>
    /// 取明细下全部部门执行行的最大行号
    /// </summary>
    /// <param name="ecnDetailId">明细 ID</param>
    /// <returns>最大行号；无记录时为 0</returns>
    public async Task<int> GetMaxLineNumberForDetailAsync(long ecnDetailId)
    {
        var rows = await ListBaseByEcnDetailIdAsync(ecnDetailId);
        return rows.Count == 0 ? 0 : rows.Max(x => x.LineNumber);
    }

    /// <summary>
    /// 按明细 ID 删除全部部门执行行
    /// </summary>
    /// <param name="ecnDetailId">明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAllByEcnDetailIdAsync(long ecnDetailId)
    {
        await _pmcRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _mpRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _iqcRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _mcRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _pcbaRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _assyRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _qaRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
        await _teRepository.DeleteAsync(x => x.EcnDetailId == ecnDetailId);
    }

    /// <summary>
    /// 按 ID 与部门编码取最大行号（单表）
    /// </summary>
    /// <param name="ecnDetailId">明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <returns>最大行号</returns>
    public Task<int> GetMaxLineNumberForDetailDeptAsync(
        long ecnDetailId,
        string deptCode,
        string tenantCode,
        string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => _pmcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Mp => _mpRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Iqc => _iqcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Mc => _mcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Pcba => _pcbaRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Assy => _assyRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Qa => _qaRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            TaktEcDeptCodes.Te => _teRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcnDetailId == ecnDetailId,
                x => x.LineNumber),
            _ => Task.FromResult(0)
        };
    }

    /// <summary>
    /// 映射公共字段
    /// </summary>
    /// <param name="entity">执行实体</param>
    /// <returns>公共字段快照</returns>
    private static TaktEcExecBaseRow? ToBaseRow(object? entity) => entity switch
    {
        TaktEcSeikan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcKoubai e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcUkeken e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcBukan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizounika e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizouikka e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcHinkan e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcSeizougijutsu e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcnDetailId = e.EcnDetailId,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        _ => null
    };
}
