// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecPersistence.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行聚合持久化（8 张 TaktEcExec* 部门表读写，由 TaktEcDetail 关联）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行聚合持久化
/// </summary>
public class TaktEcExecPersistence
{
    private readonly ITaktCompanyRepository<TaktEcSeikan> _pmcRepository;
    private readonly ITaktCompanyRepository<TaktEcKoubai> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcUkeken> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcBukan> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizounika> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizouikka> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcHinkan> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizougijutsu> _teRepository;

    private readonly TaktEcExecDeptAccess _deptAccess;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecPersistence(
        TaktEcExecDeptAccess deptAccess,
        ITaktCompanyRepository<TaktEcSeikan> pmcRepository,
        ITaktCompanyRepository<TaktEcKoubai> mpRepository,
        ITaktCompanyRepository<TaktEcUkeken> iqcRepository,
        ITaktCompanyRepository<TaktEcBukan> mcRepository,
        ITaktCompanyRepository<TaktEcSeizounika> pcbaRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository)
    {
        _deptAccess = deptAccess;
        _pmcRepository = pmcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _pcbaRepository = pcbaRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
    }

    /// <summary>
    /// 取明细在指定部门的最大行号
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
        string companyCode) =>
        _deptAccess.GetMaxLineNumberForDetailDeptAsync(ecnDetailId, deptCode, tenantCode, companyCode);

    /// <summary>
    /// 根据明细与部门加载执行实体
    /// </summary>
    /// <param name="ecnDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>部门执行实体</returns>
    public Task<object?> LoadByDetailAndDeptAsync(long ecnDetailId, string deptCode) =>
        FirstEntityByDetailAndDeptAsync(ecnDetailId, deptCode);

    /// <summary>
    /// 批量加载指定部门的执行映射（明细 ID → 实体）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>映射</returns>
    public async Task<Dictionary<long, object>> LoadMapByDetailIdsAsync(
        IReadOnlyList<long> detailIds,
        string deptCode)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, object>();
        }
        var execs = await ListEntitiesByDetailIdsAndDeptAsync(detailIds, deptCode);
        var result = new Dictionary<long, object>(execs.Count);
        foreach (var exec in execs)
        {
            result[TaktEcDeptEntityHelper.GetEcnDetailId(exec)] = exec;
        }
        return result;
    }

    /// <summary>
    /// 批量加载全部部门的执行分组（明细 ID → 实体列表）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>分组映射</returns>
    public async Task<Dictionary<long, List<object>>> LoadGroupsAllDeptsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, List<object>>();
        }
        var execs = await ListAllEntitiesByDetailIdsAsync(detailIds);
        return execs
            .GroupBy(TaktEcDeptEntityHelper.GetEcnDetailId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// 统计租户+公司范围内全部部门执行行数
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="isImplemented">是否实施（空=全部）</param>
    /// <returns>行数</returns>
    public async Task<int> CountAllDeptRowsForScopeAsync(string tenantCode, string companyCode, int? isImplemented = null)
    {
        var execs = new List<object>();
        execs.AddRange(await _pmcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _mpRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _iqcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _mcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _pcbaRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _assyRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _qaRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _teRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        if (!isImplemented.HasValue)
        {
            return execs.Count;
        }
        return execs.Count(x => TaktEcDeptEntityHelper.MatchesIsImplemented(x, isImplemented.Value));
    }

    /// <summary>
    /// 按明细 ID 批量删除执行记录（含子表）
    /// </summary>
    /// <param name="ecnDetailId">设变明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteByDetailIdCascadeAsync(long ecnDetailId)
    {
        await _deptAccess.DeleteAllByEcnDetailIdAsync(ecnDetailId);
    }

    /// <summary>
    /// 从部门视图更新 DTO 写入或更新执行记录
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="dto">更新 DTO</param>
    /// <param name="lineNumberGenerator">行号生成回调</param>
    /// <returns>部门执行实体</returns>
    public async Task<object> UpsertFromViewUpdateAsync(
        TaktEcDetail detail,
        string deptCode,
        TaktEcDeptViewUpdateDto dto,
        Func<Task<int>> lineNumberGenerator)
    {
        var exec = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
        var isNew = exec == null;
        exec = CreateConcreteExec(detail, deptCode, exec, await lineNumberGenerator());
        ApplyViewUpdateToExec(exec, dto);
        ApplyViewUpdateDeptFields(exec, dto);
        return await SaveEntityAsync(exec, deptCode, isNew);
    }

    /// <summary>
    /// 将视图更新 DTO 写入部门执行实体
    /// </summary>
    private static void ApplyViewUpdateToExec(object exec, TaktEcDeptViewUpdateDto dto)
    {
        switch (exec)
        {
            case TaktEcSeikan e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcKoubai e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcUkeken e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcBukan e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcSeizounika e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcSeizouikka e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcHinkan e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcSeizougijutsu e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
        }
    }

    /// <summary>
    /// 从视图 DTO 写入部门专有字段（实体已含同表字段）
    /// </summary>
    private static void ApplyViewUpdateDeptFields(object exec, TaktEcDeptViewUpdateDto dto)
    {
        switch (exec)
        {
            case TaktEcSeikan pmc:
                pmc.ScheduledProductionDate = dto.ScheduledProductionDate;
                pmc.ScheduledBatch = dto.ScheduledBatch;
                pmc.PoRemainder = dto.PoRemainder;
                pmc.Balance = dto.Balance;
                pmc.OldProductHandling = dto.OldProductHandling;
                break;
            case TaktEcKoubai mp:
                mp.PurchaseOrderIssueDate = dto.PurchaseOrderIssueDate;
                mp.Supplier = dto.Supplier;
                mp.PurchaseOrderNo = dto.PurchaseOrderNo;
                break;
            case TaktEcUkeken iqc:
                iqc.IqcOrderNo = dto.IqcOrderNo;
                iqc.InspectionDate = dto.InspectionDate;
                break;
            case TaktEcBukan mc:
                mc.OutboundBatch = dto.OutboundBatch;
                mc.OutboundDate = dto.OutboundDate;
                break;
            case TaktEcSeizounika pcba:
                pcba.ProductionDate = dto.ProductionDate;
                pcba.ProductionBatch = dto.ProductionBatch;
                pcba.ProductionTeam = dto.ProductionTeam;
                pcba.OutboundOrderNo = dto.OutboundOrderNo;
                break;
            case TaktEcSeizouikka assy:
                assy.ProductionTeam = dto.ProductionTeam;
                assy.ProductionDate = dto.ProductionDate;
                assy.ImplementationBatch = dto.ImplementationBatch;
                break;
            case TaktEcHinkan qa:
                qa.ProductionTeam = dto.ProductionTeam;
                qa.InspectionDate = dto.InspectionDate;
                qa.InspectionBatch = dto.InspectionBatch;
                qa.SamplingNo = dto.SamplingNo;
                break;
            case TaktEcSeizougijutsu te:
                te.ConfirmationDate = dto.ConfirmationDate;
                te.IsSopUpdated = dto.IsSopUpdated;
                break;
        }
    }

    /// <summary>
    /// 按明细与部门查找执行实体
    /// </summary>
    private async Task<object?> FirstEntityByDetailAndDeptAsync(long ecnDetailId, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => await _pmcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Mp => await _mpRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Iqc => await _iqcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Mc => await _mcRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Pcba => await _pcbaRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Assy => await _assyRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Qa => await _qaRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            TaktEcDeptCodes.Te => await _teRepository.FirstAsync(x => x.EcnDetailId == ecnDetailId),
            _ => null
        };
    }

    /// <summary>
    /// 按明细 ID 列表与部门加载执行实体
    /// </summary>
    private async Task<List<object>> ListEntitiesByDetailIdsAndDeptAsync(IReadOnlyList<long> detailIds, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => (await _pmcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Mp => (await _mpRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Iqc => (await _iqcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Mc => (await _mcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Pcba => (await _pcbaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Assy => (await _assyRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Qa => (await _qaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            TaktEcDeptCodes.Te => (await _teRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId))).Cast<object>().ToList(),
            _ => []
        };
    }

    /// <summary>
    /// 按明细 ID 列表加载全部部门执行实体
    /// </summary>
    private async Task<List<object>> ListAllEntitiesByDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        var rows = new List<object>();
        rows.AddRange(await _pmcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _mpRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _iqcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _mcRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _pcbaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _assyRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _qaRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        rows.AddRange(await _teRepository.GetListAsync(x => detailIds.Contains(x.EcnDetailId)));
        return rows;
    }

    /// <summary>
    /// 创建具体部门执行实体实例
    /// </summary>
    private static object CreateConcreteExec(TaktEcDetail detail, string deptCode, object? existing, int lineNumber) =>
        existing ?? deptCode switch
        {
            TaktEcDeptCodes.Pmc => new TaktEcSeikan { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Mp => new TaktEcKoubai { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Iqc => new TaktEcUkeken { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Mc => new TaktEcBukan { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Pcba => new TaktEcSeizounika { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Assy => new TaktEcSeizouikka { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Qa => new TaktEcHinkan { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            TaktEcDeptCodes.Te => new TaktEcSeizougijutsu { EcnDetailId = detail.Id, EcNo = detail.EcNo, DeptCode = deptCode, LineNumber = lineNumber },
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };

    /// <summary>
    /// 保存执行实体到对应部门表
    /// </summary>
    private async Task<object> SaveEntityAsync(object exec, string deptCode, bool isNew) =>
        deptCode switch
        {
            TaktEcDeptCodes.Pmc => await SaveTypedAsync(_pmcRepository, (TaktEcSeikan)exec, isNew),
            TaktEcDeptCodes.Mp => await SaveTypedAsync(_mpRepository, (TaktEcKoubai)exec, isNew),
            TaktEcDeptCodes.Iqc => await SaveTypedAsync(_iqcRepository, (TaktEcUkeken)exec, isNew),
            TaktEcDeptCodes.Mc => await SaveTypedAsync(_mcRepository, (TaktEcBukan)exec, isNew),
            TaktEcDeptCodes.Pcba => await SaveTypedAsync(_pcbaRepository, (TaktEcSeizounika)exec, isNew),
            TaktEcDeptCodes.Assy => await SaveTypedAsync(_assyRepository, (TaktEcSeizouikka)exec, isNew),
            TaktEcDeptCodes.Qa => await SaveTypedAsync(_qaRepository, (TaktEcHinkan)exec, isNew),
            TaktEcDeptCodes.Te => await SaveTypedAsync(_teRepository, (TaktEcSeizougijutsu)exec, isNew),
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };

    /// <summary>
    /// 为设变明细初始化全部责任部门执行行（KanbanOrder 共 8 课，每明细×部门一行；已存在则跳过）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>新建部门行数</returns>
    public async Task<int> EnsureAllDeptExecRowsForDetailAsync(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (detail.Id <= 0)
        {
            throw new ArgumentException("设变明细 ID 无效", nameof(detail));
        }
        var created = 0;
        foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
        {
            var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
            if (existing != null)
            {
                continue;
            }
            var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
            var exec = CreateConcreteExec(detail, deptCode, null, lineNumber);
            await SaveEntityAsync(exec, deptCode, true);
            created += 1;
        }
        return created;
    }

    /// <summary>
    /// 批量为设变明细初始化全部责任部门执行行
    /// </summary>
    /// <param name="details">设变明细列表</param>
    /// <returns>任务</returns>
    public async Task EnsureAllDeptExecRowsForDetailsAsync(IReadOnlyList<TaktEcDetail> details)
    {
        if (details == null || details.Count == 0)
        {
            return;
        }
        foreach (var detail in details)
        {
            await EnsureAllDeptExecRowsForDetailAsync(detail);
        }
    }

    /// <summary>
    /// 按指定部门编码顺序为每条设变明细初始化部门执行行（已存在则跳过）
    /// </summary>
    /// <param name="details">设变明细列表</param>
    /// <param name="deptCodesInOrder">部门编码顺序</param>
    /// <returns>任务</returns>
    public async Task EnsureDeptExecRowsForDetailsInOrderAsync(
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<string> deptCodesInOrder)
    {
        if (details == null || details.Count == 0 || deptCodesInOrder == null || deptCodesInOrder.Count == 0)
        {
            return;
        }
        foreach (var detail in details)
        {
            ArgumentNullException.ThrowIfNull(detail);
            if (detail.Id <= 0)
            {
                throw new ArgumentException("设变明细 ID 无效", nameof(details));
            }
            foreach (var deptCode in deptCodesInOrder)
            {
                if (string.IsNullOrWhiteSpace(deptCode))
                {
                    continue;
                }
                var normalizedDeptCode = deptCode.Trim();
                var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, normalizedDeptCode);
                if (existing != null)
                {
                    continue;
                }
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = CreateConcreteExec(detail, normalizedDeptCode, null, lineNumber);
                await SaveEntityAsync(exec, normalizedDeptCode, true);
            }
        }
    }

    /// <summary>
    /// 泛型保存
    /// </summary>
    private static async Task<TEntity> SaveTypedAsync<TEntity>(ITaktCompanyRepository<TEntity> repository, TEntity entity, bool isNew)
        where TEntity : TaktCompanyEntityBase, new()
    {
        if (isNew)
        {
            return await repository.CreateAsync(entity);
        }
        await repository.UpdateAsync(entity);
        return entity;
    }
}