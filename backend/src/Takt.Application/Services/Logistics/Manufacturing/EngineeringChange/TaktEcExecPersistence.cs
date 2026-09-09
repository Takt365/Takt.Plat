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

using Mapster;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行聚合持久化
/// </summary>
public partial class TaktEcExecPersistence
{
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcSeikan> _pmcRepository;
    private readonly ITaktCompanyRepository<TaktEcKoubai> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcUkeken> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcBukan> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizounika> _seizounikaRepository;
    private readonly ITaktCompanyRepository<TaktEcSmt> _smtRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizouikka> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcHinkan> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizougijutsu> _teRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;

    private readonly TaktEcExecDeptAccess _deptAccess;
    private readonly TaktEcGijutsuStatusSynchronizer _ecGijutsuStatusSynchronizer;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecPersistence(
        TaktEcExecDeptAccess deptAccess,
        TaktEcGijutsuStatusSynchronizer ecGijutsuStatusSynchronizer,
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcSeikan> pmcRepository,
        ITaktCompanyRepository<TaktEcKoubai> mpRepository,
        ITaktCompanyRepository<TaktEcUkeken> iqcRepository,
        ITaktCompanyRepository<TaktEcBukan> mcRepository,
        ITaktCompanyRepository<TaktEcSeizounika> seizounikaRepository,
        ITaktCompanyRepository<TaktEcSmt> smtRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository,
        ITaktCompanyRepository<TaktDept> deptRepository)
    {
        _deptAccess = deptAccess;
        _ecGijutsuStatusSynchronizer = ecGijutsuStatusSynchronizer;
        _ecGijutsuRepository = ecGijutsuRepository;
        _ecDetailRepository = ecDetailRepository;
        _pmcRepository = pmcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _seizounikaRepository = seizounikaRepository;
        _smtRepository = smtRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
        _deptRepository = deptRepository;
    }

    /// <summary>
    /// 取明细在指定部门的最大行号
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <returns>最大行号</returns>
    public Task<int> GetMaxLineNumberForDetailDeptAsync(
        long ecDetailId,
        string deptCode,
        string tenantCode,
        string companyCode) =>
        _deptAccess.GetMaxLineNumberForDetailDeptAsync(ecDetailId, deptCode, tenantCode, companyCode);

    /// <summary>
    /// 根据明细与部门加载执行实体
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>部门执行实体</returns>
    public Task<object?> LoadByDetailAndDeptAsync(long ecDetailId, string deptCode) =>
        FirstEntityByDetailAndDeptAsync(ecDetailId, deptCode);

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
            result[TaktEcDeptEntityHelper.GetEcDetailId(exec)] = exec;
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
            .GroupBy(TaktEcDeptEntityHelper.GetEcDetailId)
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
        execs.AddRange(await _seizounikaRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _smtRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
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
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteByDetailIdCascadeAsync(long ecDetailId)
    {
        var existingRows = await _deptAccess.ListBaseByEcDetailIdAsync(ecDetailId);
        var ecCode = existingRows.FirstOrDefault()?.EcCode;
        await _deptAccess.DeleteAllByEcDetailIdAsync(ecDetailId);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(ecCode);
    }

    /// <summary>
    /// 按设变单号删除全部部门执行行（含子表；用于明细先删后插后的孤儿清理）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>任务</returns>
    public async Task DeleteAllByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        await _pmcRepository.DeleteAsync(x => x.EcCode == code);
        await _mpRepository.DeleteAsync(x => x.EcCode == code);
        await _iqcRepository.DeleteAsync(x => x.EcCode == code);
        await _mcRepository.DeleteAsync(x => x.EcCode == code);
        await _seizounikaRepository.DeleteAsync(x => x.EcCode == code);
        await _smtRepository.DeleteAsync(x => x.EcCode == code);
        await _assyRepository.DeleteAsync(x => x.EcCode == code);
        await _qaRepository.DeleteAsync(x => x.EcCode == code);
        await _teRepository.DeleteAsync(x => x.EcCode == code);
    }

    /// <summary>
    /// 区分=内部/技术时不做采购类型等条件扇出（执行内容已按「管理区分-内部」「管理区分-技术」填好）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>是否跳过条件扇出</returns>
    private async Task<bool> ShouldSkipConditionFanOutAsync(string? ecCode)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return false;
        }
        var gijutsu = await _ecGijutsuRepository.FirstAsync(x => x.EcCode == ecCode);
        return gijutsu != null
            && (gijutsu.EcDistinction == TaktEcDistinctionConstants.Internal
                || gijutsu.EcDistinction == TaktEcDistinctionConstants.Technical);
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
        object? exec;
        bool isNew;
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            var route = TaktEcSmtRouteHelper.Resolve(detail);
            if (route == TaktEcSmtRouteTarget.None)
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                throw new InvalidOperationException("制造二课当前明细无有效执行表路由（F 且非 C003）");
            }
            if (route == TaktEcSmtRouteTarget.Smt
                && !TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                throw new InvalidOperationException("制造二课 SMT需要有效新物料编码");
            }
            await ObsoleteTypedDeptExecIfExistsAsync(
                detail.Id,
                route == TaktEcSmtRouteTarget.Smt
                    ? TaktEcSmtRouteTarget.Seizounika
                    : TaktEcSmtRouteTarget.Smt);
            exec = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, route);
            isNew = exec == null;
            exec = CreatePcbaConcreteExec(detail, route, exec, await lineNumberGenerator(), applyNotRelatedAuto: false);
        }
        else
        {
            exec = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
            isNew = exec == null;
            exec = CreateConcreteExec(detail, deptCode, exec, await lineNumberGenerator(), applyNotRelatedAuto: false);
        }
        ApplyViewUpdateToExec(exec, dto);
        ApplyViewUpdateDeptFields(exec, dto);
        var saved = await SaveEntityAsync(exec, deptCode, isNew);
        if (saved is TaktEcSeikan seikan)
        {
            await FanOutSeikanFillableByEcModelAndFinishedGoodsAsync(seikan);
        }
        else if (saved is TaktEcKoubai koubai)
        {
            await FanOutKoubaiFillableByEcAndNewMaterialAsync(koubai);
        }
        else if (saved is TaktEcUkeken ukeken)
        {
            await FanOutUkekenFillableByEcAndNewMaterialAsync(ukeken);
        }
        else if (saved is TaktEcBukan bukan)
        {
            await FanOutBukanFillableByEcModelAndNewMaterialAsync(bukan);
        }
        else if (saved is TaktEcSeizounika seizounika)
        {
            await FanOutSeizounikaFillableByEcModelAndFinishedGoodsAsync(seizounika);
        }
        else if (saved is TaktEcSmt smt)
        {
            await FanOutSmtFillableByEcAndParentMaterialAsync(smt);
        }
        else if (saved is TaktEcSeizouikka assy)
        {
            await FanOutSeizouikkaFillableByEcModelAndFinishedGoodsAsync(assy);
        }
        else if (saved is TaktEcHinkan hinkan)
        {
            await FanOutHinkanFillableByEcModelAndFinishedGoodsAsync(hinkan);
        }
        else if (saved is TaktEcSeizougijutsu te)
        {
            await FanOutSeizougijutsuFillableByEcModelAndFinishedGoodsAsync(te);
        }
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
        await TryCascadeAfterGateDeptCompletedAsync(detail, deptCode, saved);
        return saved;
    }

    /// <summary>
    /// 采购课：将可填字段同步到同设变单号+新物料且新采购类型为 F 的全部未作废执行行（不含当前行）。
    /// 源明细非 F 时不扇出，避免覆盖「采购无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的采购执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutKoubaiFillableByEcAndNewMaterialAsync(TaktEcKoubai source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null || !TaktEcDistinctionConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcNewMaterialCode == detail.EcNewMaterialCode
            && x.EcNewPurchaseType == purchaseTypeF
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _mpRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.PurchaseOrderIssueDate = source.PurchaseOrderIssueDate;
            row.Supplier = source.Supplier;
            row.PurchaseOrderCode = source.PurchaseOrderCode;
            row.EcOldPartDisposition = source.EcOldPartDisposition;
        }
        await _mpRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 受检课：将可填字段同步到同设变单号+新物料且新品需检验=1 的全部未作废执行行（不含当前行）。
    /// 源明细无需检验时不扇出，避免覆盖「跟 IQC 无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的受检执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutUkekenFillableByEcAndNewMaterialAsync(TaktEcUkeken source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null || detail.EcNewRequiresInspection != 1)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcNewMaterialCode == detail.EcNewMaterialCode
            && x.EcNewRequiresInspection == 1
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _iqcRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.IqcOrderCode = source.IqcOrderCode;
            row.InspectionDate = source.InspectionDate;
        }
        await _iqcRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 部管课：将可填字段同步到同设变单号+机种+新物料且采购类型 F、仓库非 C003 的全部未作废执行行（不含当前行）。
    /// 源明细不可见时不扇出，避免覆盖「跟部管无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的部管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutBukanFillableByEcModelAndNewMaterialAsync(TaktEcBukan source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null || !TaktEcDistinctionConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcNewMaterialCode == detail.EcNewMaterialCode
            && x.EcNewPurchaseType == purchaseTypeF
            && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _mcRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.OutboundBatch = source.OutboundBatch;
            row.OutboundDate = source.OutboundDate;
        }
        await _mcRepository.UpdateRangeAsync(rows);
        if (source.IsImplemented != 1)
        {
            return;
        }
        foreach (var row in rows)
        {
            await TryCascadeAfterGateDeptCompletedByDetailIdAsync(row.EcDetailId, TaktEcDeptCodes.Mc, row);
        }
    }

    /// <summary>
    /// SMT：将可填字段同步到同设变单号+上阶物料且 F+C003 的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的 SMT执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSmtFillableByEcAndParentMaterialAsync(TaktEcSmt source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null
            || !TaktEcDistinctionConstants.IsPcbaC003ExternalGroup(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var parent = detail.EcParentMaterialCode ?? string.Empty;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcParentMaterialCode == parent
            && x.EcNewPurchaseType == purchaseTypeF
            && x.EcNewWarehouse != null
            && x.EcNewWarehouse == warehouseC003
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _smtRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.OutboundBatch = source.OutboundBatch;
            row.OutboundDate = source.OutboundDate;
        }
        await _smtRepository.UpdateRangeAsync(rows);
        if (source.IsImplemented != 1)
        {
            return;
        }
        foreach (var row in rows)
        {
            await TryCascadeAfterGateDeptCompletedByDetailIdAsync(row.EcDetailId, TaktEcDeptCodes.Pcba, row);
        }
    }

    /// <summary>
    /// SMT扇出（兼容旧方法名）
    /// </summary>
    /// <param name="source">已写入当前行的 SMT执行实体</param>
    /// <returns>任务</returns>
    public Task FanOutSmtFillableByEcAndNewMaterialAsync(TaktEcSmt source) =>
        FanOutSmtFillableByEcAndParentMaterialAsync(source);

    /// <summary>
    /// 制二课：将可填字段同步到同设变单号+机种+完成品且采购类型非 F 的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制二执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSeizounikaFillableByEcModelAndFinishedGoodsAsync(TaktEcSeizounika source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null || !TaktEcDistinctionConstants.IsPcbaOtherPurchaseGroup(detail.EcNewPurchaseType))
        {
            return;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcFinishedGoods == detail.EcFinishedGoods
            && x.EcNewPurchaseType != purchaseTypeF
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _seizounikaRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.ProductionDate = source.ProductionDate;
            row.ImplementationBatch = source.ImplementationBatch;
        }
        await _seizounikaRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 生管课：将可填字段同步到同设变单号+机种+完成品的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的生管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSeikanFillableByEcModelAndFinishedGoodsAsync(TaktEcSeikan source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcFinishedGoods == detail.EcFinishedGoods
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _pmcRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ScheduledProductionDate = source.ScheduledProductionDate;
            row.ScheduledBatch = source.ScheduledBatch;
            row.PoRemainder = source.PoRemainder;
            row.Balance = source.Balance;
            row.EcOldPartDisposition = source.EcOldPartDisposition;
        }
        await _pmcRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 制一课：将可填字段同步到同设变单号+机种+完成品的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制一执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSeizouikkaFillableByEcModelAndFinishedGoodsAsync(TaktEcSeizouikka source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcFinishedGoods == detail.EcFinishedGoods
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _assyRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.ProductionDate = source.ProductionDate;
            row.ImplementationBatch = source.ImplementationBatch;
        }
        await _assyRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 品管课：将可填字段同步到同设变单号+机种+完成品的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的品管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutHinkanFillableByEcModelAndFinishedGoodsAsync(TaktEcHinkan source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcFinishedGoods == detail.EcFinishedGoods
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _qaRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.InspectionDate = source.InspectionDate;
            row.InspectionBatch = source.InspectionBatch;
            row.SamplingCode = source.SamplingCode;
        }
        await _qaRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 制技课：将可填字段同步到同设变单号+机种+完成品的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制技执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSeizougijutsuFillableByEcModelAndFinishedGoodsAsync(TaktEcSeizougijutsu source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcDetailId);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcFinishedGoods == detail.EcFinishedGoods
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _teRepository.GetListAsync(x =>
            detailIds.Contains(x.EcDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ConfirmationDate = source.ConfirmationDate;
            row.IsSopUpdated = source.IsSopUpdated;
        }
        await _teRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 按明细确保部门执行行，并按区分写入实施状态与执行内容。
    /// 内部/技术：强制填「管理区分-内部」「管理区分-技术」，不判断采购类型/仓库/检验/EOL。
    /// 全仕向/部管：新建或系统文案可覆盖；EOL 优先；条件无关行再按采购/受检/部管规则写入。
    /// 采购/受检/部管：新物料编码为空或「0」时不新建、已有行作废（内部/技术同样遵守）。
    /// 采购/受检/部管：列表可见组内与 QueryHelper 同键去重，仅最大 Id 明细生成执行行（其余作废）。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="autoComplete">true=按区分自动填写；false=待人工填写</param>
    /// <param name="ecDistinction">设变区分（字典 logistics_manufacturing_ec_distinction_category）</param>
    /// <returns>部门执行实体；跳过生成时为 null</returns>
    public async Task<object?> UpsertDeptExecWithFillModeAsync(
        TaktEcDetail detail,
        string deptCode,
        bool autoComplete,
        int ecDistinction)
    {
        ArgumentNullException.ThrowIfNull(detail);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            return await UpsertPcbaDeptExecWithFillModeAsync(detail, autoComplete, ecDistinction);
        }
        if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(deptCode)
            && !TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        // 采购/受检/部管：非列表可见不生成（与 QueryHelper 一致）
        if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(deptCode)
            && !IsNewMaterialDeptListVisible(detail, deptCode))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        if (await ShouldSkipNewMaterialDeptByListDedupAsync(detail, deptCode))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        if (await ShouldSkipModelFinishedGoodsDeptByListDedupAsync(detail, deptCode))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
        var isNew = existing == null;
        var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var applyNotRelatedAuto = ecDistinction == TaktEcDistinctionConstants.AllDestination
            || ecDistinction == TaktEcDistinctionConstants.MaterialControl;
        var exec = CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto);
        EnsureDeptName(exec, await ResolveDeptNameAsync(deptCode));
        // 新物料有效时恢复此前因「无新物料」作废的行
        if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
        {
            deptExec.IsObsolete = 0;
        }
        ApplyDistinctionFillMode(exec, isNew, autoComplete, ecDistinction, detail);
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = string.Equals(
            filledContent,
            TaktEcDistinctionConstants.EolExecContent,
            StringComparison.Ordinal);
        if (!isEolFilled
            && (ecDistinction == TaktEcDistinctionConstants.AllDestination
                || ecDistinction == TaktEcDistinctionConstants.MaterialControl))
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        // 批量派生时不在此刷新主表状态（由编排层结束后统一 RefreshByEcCodeAsync）
        return await SaveEntityAsync(exec, deptCode, isNew);
    }

    /// <summary>
    /// 制造二课：按 F+C003 / 非 F 路由到双表；其余组别两侧作废
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="autoComplete">是否自动填写</param>
    /// <param name="ecDistinction">设变区分</param>
    /// <returns>部门执行实体；跳过时 null</returns>
    private async Task<object?> UpsertPcbaDeptExecWithFillModeAsync(
        TaktEcDetail detail,
        bool autoComplete,
        int ecDistinction)
    {
        var route = TaktEcSmtRouteHelper.Resolve(detail);
        if (route == TaktEcSmtRouteTarget.None)
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return null;
        }
        if (route == TaktEcSmtRouteTarget.Smt)
        {
            if (!TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                return null;
            }
            if (await HasNewerPcbaVisibleSiblingAsync(detail))
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                return null;
            }
            await ObsoleteTypedDeptExecIfExistsAsync(detail.Id, TaktEcSmtRouteTarget.Seizounika);
            return await UpsertSmtTypedExecAsync(detail, TaktEcSmtRouteTarget.Smt, autoComplete, ecDistinction);
        }
        if (await HasNewerSeizounikaVisibleSiblingAsync(detail))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return null;
        }
        await ObsoleteTypedDeptExecIfExistsAsync(detail.Id, TaktEcSmtRouteTarget.Smt);
        return await UpsertSmtTypedExecAsync(detail, TaktEcSmtRouteTarget.Seizounika, autoComplete, ecDistinction);
    }

    /// <summary>
    /// 制造二课指定目标表 Upsert
    /// </summary>
    private async Task<object> UpsertSmtTypedExecAsync(
        TaktEcDetail detail,
        TaktEcSmtRouteTarget target,
        bool autoComplete,
        int ecDistinction)
    {
        var existing = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, target);
        var isNew = existing == null;
        var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var applyNotRelatedAuto = ecDistinction == TaktEcDistinctionConstants.AllDestination
            || ecDistinction == TaktEcDistinctionConstants.MaterialControl;
        var exec = CreatePcbaConcreteExec(detail, target, existing, lineNumber, applyNotRelatedAuto);
        EnsureDeptName(exec, await ResolveDeptNameAsync(TaktEcDeptCodes.Pcba));
        if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
        {
            deptExec.IsObsolete = 0;
        }
        ApplyDistinctionFillMode(exec, isNew, autoComplete, ecDistinction, detail);
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = string.Equals(
            filledContent,
            TaktEcDistinctionConstants.EolExecContent,
            StringComparison.Ordinal);
        if (!isEolFilled
            && (ecDistinction == TaktEcDistinctionConstants.AllDestination
                || ecDistinction == TaktEcDistinctionConstants.MaterialControl))
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        // 批量派生时不在此刷新主表状态（由编排层结束后统一 RefreshByEcCodeAsync）
        return await SavePcbaEntityAsync(exec, isNew);
    }

    /// <summary>
    /// 新物料无效时：将已有采购/受检/部管执行行标记作废
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    private async Task ObsoleteDeptExecIfExistsAsync(long ecDetailId, string deptCode)
    {
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            await ObsoletePcbaBothTablesAsync(ecDetailId);
            return;
        }
        var existing = await FirstEntityByDetailAndDeptAsync(ecDetailId, deptCode);
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
        }
        await SaveEntityAsync(existing, deptCode, isNew: false);
    }

    /// <summary>
    /// 作废制造二课两侧表中该明细行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    private async Task ObsoletePcbaBothTablesAsync(long ecDetailId)
    {
        await ObsoleteTypedDeptExecIfExistsAsync(ecDetailId, TaktEcSmtRouteTarget.Smt);
        await ObsoleteTypedDeptExecIfExistsAsync(ecDetailId, TaktEcSmtRouteTarget.Seizounika);
    }

    /// <summary>
    /// 作废制造二课指定目标表行
    /// </summary>
    private async Task ObsoleteTypedDeptExecIfExistsAsync(long ecDetailId, TaktEcSmtRouteTarget target)
    {
        var existing = await FirstSmtEntityByDetailAndTargetAsync(ecDetailId, target);
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
        }
        await SavePcbaEntityAsync(existing, isNew: false);
    }

    /// <summary>
    /// 采购/受检/部管是否因与列表 QueryHelper 同键去重而跳过生成（组内仅最大 Id 保留）。
    /// 调用方须先用 IsNewMaterialDeptListVisible 排除非可见明细；本方法仅处理可见组内兄弟去重。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>true=跳过并应作废已有行</returns>
    private async Task<bool> ShouldSkipNewMaterialDeptByListDedupAsync(TaktEcDetail detail, string deptCode)
    {
        if (!TaktEcDistinctionConstants.IsNewMaterialDependentDept(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Mp => await HasNewerKoubaiVisibleSiblingAsync(detail),
            TaktEcDeptCodes.Iqc => await HasNewerUkekenVisibleSiblingAsync(detail),
            TaktEcDeptCodes.Mc => await HasNewerBukanVisibleSiblingAsync(detail),
            _ => false
        };
    }

    /// <summary>
    /// 生管/制一/品管/制技：设变+机种+完成品组内是否存在更大 Id（与 QueryHelper 同键）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>true=跳过并应作废已有行</returns>
    private async Task<bool> ShouldSkipModelFinishedGoodsDeptByListDedupAsync(TaktEcDetail detail, string deptCode)
    {
        if (deptCode is not (TaktEcDeptCodes.Pmc or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa or TaktEcDeptCodes.Te))
        {
            return false;
        }
        if (detail.IsObsolete != 0)
        {
            return true;
        }
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcModelCode == detail.EcModelCode
            && s.EcFinishedGoods == detail.EcFinishedGoods
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// SMT可见组内是否存在更大 Id 的同设变+上阶物料明细
    /// </summary>
    private async Task<bool> HasNewerPcbaVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcDistinctionConstants.IsPcbaC003ExternalGroup(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return false;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var parent = detail.EcParentMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcParentMaterialCode == parent
            && s.EcNewPurchaseType == purchaseTypeF
            && s.EcNewWarehouse != null
            && s.EcNewWarehouse == warehouseC003
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 制二非 F 可见组内是否存在更大 Id 的同设变+机种+完成品明细
    /// </summary>
    private async Task<bool> HasNewerSeizounikaVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcDistinctionConstants.IsPcbaOtherPurchaseGroup(detail.EcNewPurchaseType))
        {
            return false;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var model = detail.EcModelCode ?? string.Empty;
        var finished = detail.EcFinishedGoods ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcModelCode == model
            && s.EcFinishedGoods == finished
            && s.EcNewPurchaseType != purchaseTypeF
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 采购课可见组（采购类型 F）内是否存在更大 Id 的同设变+新物料明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <returns>存在更大 Id 兄弟则 true</returns>
    private async Task<bool> HasNewerKoubaiVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcDistinctionConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return false;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcNewMaterialCode == material
            && s.EcNewPurchaseType == purchaseTypeF
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 受检课可见组（需检验=1）内是否存在更大 Id 的同设变+新物料明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <returns>存在更大 Id 兄弟则 true</returns>
    private async Task<bool> HasNewerUkekenVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (detail.EcNewRequiresInspection != 1)
        {
            return false;
        }
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcNewMaterialCode == material
            && s.EcNewRequiresInspection == 1
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 部管课可见组（F 且非 C003）内是否存在更大 Id 的同设变+机种+新物料明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <returns>存在更大 Id 兄弟则 true</returns>
    private async Task<bool> HasNewerBukanVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcDistinctionConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return false;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var model = detail.EcModelCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcModelCode == model
            && s.EcNewMaterialCode == material
            && s.EcNewPurchaseType == purchaseTypeF
            && (s.EcNewWarehouse == null || s.EcNewWarehouse != warehouseC003)
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 按明细更新停产状态，并同步各课执行行冗余字段与自动填充/清除
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="discontinuedStatus">完成品物料状态（Z0=在产；非 Z0 视为停产，按钮默认写 ZQ）</param>
    public async Task ApplyDiscontinuedStatusForDetailAsync(long ecDetailId, string discontinuedStatus)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        var status = string.IsNullOrWhiteSpace(discontinuedStatus)
            ? TaktEcDistinctionConstants.PlannedMaterialStatus
            : discontinuedStatus.Trim();
        detail.DiscontinuedStatus = status;
        await _ecDetailRepository.UpdateAsync(detail);
        var gijutsu = await _ecGijutsuRepository.FirstAsync(x => x.EcCode == detail.EcCode);
        var ecDistinction = gijutsu?.EcDistinction ?? TaktEcDistinctionConstants.Technical;
        var rows = await ListAllEntitiesByDetailIdsAsync(new[] { detail.Id });
        foreach (var exec in rows)
        {
            if (TaktEcDeptEntityHelper.GetIsObsolete(exec) == 1)
            {
                continue;
            }
            SetDiscontinuedStatusIfPresent(exec, status);
            ApplyManualDiscontinuedFillMode(exec, ecDistinction, detail);
            var deptCode = TaktEcDeptEntityHelper.GetDeptCode(exec);
            if (!TaktEcDistinctionConstants.IsEolDiscontinued(status)
                && (ecDistinction == TaktEcDistinctionConstants.AllDestination
                    || ecDistinction == TaktEcDistinctionConstants.MaterialControl))
            {
                var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
                var isEolFilled = string.Equals(
                    filledContent,
                    TaktEcDistinctionConstants.EolExecContent,
                    StringComparison.OrdinalIgnoreCase);
                if (!isEolFilled)
                {
                    TaktEcExecNotRelated.TryAfterFill(exec, detail);
                }
            }
            await SaveEntityAsync(exec, deptCode == TaktEcDeptCodes.Pcba ? TaktEcDeptCodes.Pcba : deptCode, isNew: false);
        }
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
    }

    /// <summary>
    /// 写入执行行冗余停产状态（采购/受检无该列则跳过）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="status">停产状态</param>
    private static void SetDiscontinuedStatusIfPresent(object exec, string status)
    {
        switch (exec)
        {
            case TaktEcSeikan e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcBukan e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcSmt e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcSeizounika e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcSeizouikka e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcHinkan e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcSeizougijutsu e:
                e.DiscontinuedStatus = status;
                break;
        }
    }

    /// <summary>
    /// 人工停产/在产：强制覆盖执行内容与实施态（停产=EOL；在产=按区分回填或清空）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="ecDistinction">设变区分</param>
    /// <param name="detail">设变明细</param>
    private static void ApplyManualDiscontinuedFillMode(object exec, int ecDistinction, TaktEcDetail detail)
    {
        if (TaktEcDistinctionConstants.IsEolDiscontinued(detail.DiscontinuedStatus))
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(exec, TaktEcDistinctionConstants.EolExecContent, overwrite: true);
            return;
        }
        if (ecDistinction == TaktEcDistinctionConstants.Internal
            || ecDistinction == TaktEcDistinctionConstants.Technical)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcDistinctionConstants.ResolveAutoExecContent(ecDistinction),
                overwrite: true);
            return;
        }
        var deptCode = TaktEcDeptEntityHelper.GetDeptCode(exec);
        var autoComplete = ResolveAutoCompleteForManualClear(ecDistinction, deptCode, detail);
        if (autoComplete)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcDistinctionConstants.ResolveAutoExecContent(ecDistinction),
                overwrite: true);
            return;
        }
        TaktEcDeptEntityHelper.SetIsImplemented(exec, 0);
        TaktEcDeptEntityHelper.SetExecContent(exec, string.Empty, overwrite: true);
    }

    /// <summary>
    /// 在产清除后是否按区分自动完成（与部管/全仕向人工待填规则一致）
    /// </summary>
    /// <param name="ecDistinction">设变区分</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="detail">设变明细</param>
    /// <returns>是否自动完成</returns>
    private static bool ResolveAutoCompleteForManualClear(int ecDistinction, string deptCode, TaktEcDetail detail)
    {
        if (ecDistinction == TaktEcDistinctionConstants.AllDestination)
        {
            return true;
        }
        if (ecDistinction == TaktEcDistinctionConstants.MaterialControl)
        {
            return !TaktEcDistinctionConstants.IsMaterialControlNeedFillDept(
                deptCode,
                detail.EcNewPurchaseType,
                detail.EcNewWarehouse);
        }
        return false;
    }

    /// <summary>
    /// 按区分写入实施态与执行内容
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isNew">是否新建</param>
    /// <param name="autoComplete">是否自动填写</param>
    /// <param name="ecDistinction">设变区分</param>
    /// <param name="detail">设变明细</param>
    private static void ApplyDistinctionFillMode(
        object exec,
        bool isNew,
        bool autoComplete,
        int ecDistinction,
        TaktEcDetail detail)
    {
        if (ecDistinction == TaktEcDistinctionConstants.Internal
            || ecDistinction == TaktEcDistinctionConstants.Technical)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcDistinctionConstants.ResolveAutoExecContent(ecDistinction),
                overwrite: true);
            return;
        }
        var currentContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var canOverwrite = isNew
            || TaktEcDistinctionConstants.IsDistinctionGeneratedExecContent(currentContent);
        if (!canOverwrite)
        {
            return;
        }
        var applyEol = TaktEcDistinctionConstants.IsEolDiscontinued(detail.DiscontinuedStatus);
        if (applyEol)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(exec, TaktEcDistinctionConstants.EolExecContent, overwrite: true);
            return;
        }
        if (autoComplete)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcDistinctionConstants.ResolveAutoExecContent(ecDistinction),
                overwrite: true);
            return;
        }
        TaktEcDeptEntityHelper.SetIsImplemented(exec, 0);
        TaktEcDeptEntityHelper.SetExecContent(exec, string.Empty, overwrite: true);
    }

    /// <summary>
    /// 区分=部管：部管/制二完成后，按条件补齐采购、受检、部管、制二课，其余部门填「管理区分-部管」
    /// </summary>
    public async Task TryCascadeAfterGateDeptCompletedAsync(
        TaktEcDetail detail,
        string deptCode,
        object savedExec)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (TaktEcDeptEntityHelper.GetIsImplemented(savedExec) != 1)
        {
            return;
        }
        var isMcGate = deptCode == TaktEcDeptCodes.Mc;
        var isPcbaGate = deptCode == TaktEcDeptCodes.Pcba;
        if (!isMcGate && !isPcbaGate)
        {
            return;
        }
        var gijutsu = await _ecGijutsuRepository.FirstAsync(x => x.EcCode == detail.EcCode);
        if (gijutsu == null || gijutsu.EcDistinction != TaktEcDistinctionConstants.MaterialControl)
        {
            return;
        }
        var isC003 = TaktEcDistinctionConstants.IsPcbaGateWarehouse(detail.EcNewWarehouse);
        if (isMcGate && isC003)
        {
            return;
        }
        if (isPcbaGate && !isC003)
        {
            return;
        }
        foreach (var otherDept in TaktEcDeptCodes.KanbanOrder)
        {
            if (otherDept == deptCode)
            {
                continue;
            }
            var autoCompleteOther = !TaktEcDistinctionConstants.IsMaterialControlNeedFillDept(
                otherDept,
                detail.EcNewPurchaseType,
                detail.EcNewWarehouse);
            _ = await UpsertDeptExecWithFillModeAsync(
                detail,
                otherDept,
                autoComplete: autoCompleteOther,
                TaktEcDistinctionConstants.MaterialControl);
        }
    }

    /// <summary>
    /// 按明细 ID 触发门禁部门完成后的级联（CRUD 写路径用）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="savedExec">已保存的部门执行实体</param>
    /// <returns>任务</returns>
    public async Task TryCascadeAfterGateDeptCompletedByDetailIdAsync(
        long ecDetailId,
        string deptCode,
        object savedExec)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return;
        }
        await TryCascadeAfterGateDeptCompletedAsync(detail, deptCode, savedExec);
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
            case TaktEcSmt e:
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
                pmc.EcOldPartDisposition = dto.OldProductHandling;
                break;
            case TaktEcKoubai mp:
                mp.PurchaseOrderIssueDate = dto.PurchaseOrderIssueDate;
                mp.Supplier = dto.Supplier;
                mp.PurchaseOrderCode = dto.PurchaseOrderCode;
                mp.EcOldPartDisposition = dto.OldProductHandling;
                break;
            case TaktEcUkeken iqc:
                iqc.IqcOrderCode = dto.IqcOrderCode;
                iqc.InspectionDate = dto.InspectionDate;
                break;
            case TaktEcBukan mc:
                mc.OutboundBatch = dto.OutboundBatch;
                mc.OutboundDate = dto.OutboundDate;
                break;
            case TaktEcSmt electronic:
                electronic.OutboundBatch = dto.OutboundBatch;
                electronic.OutboundDate = dto.OutboundDate;
                break;
            case TaktEcSeizounika pcba:
                pcba.ProductionDate = dto.ProductionDate;
                pcba.ProductionTeam = dto.ProductionTeam;
                pcba.ImplementationBatch = dto.ImplementationBatch ?? dto.ProductionBatch;
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
                qa.SamplingCode = dto.SamplingCode;
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
    private async Task<object?> FirstEntityByDetailAndDeptAsync(long ecDetailId, string deptCode)
    {
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            return await FirstPcbaEntityByDetailIdAsync(ecDetailId);
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => await _pmcRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Mp => await _mpRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Iqc => await _iqcRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Mc => await _mcRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Assy => await _assyRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Qa => await _qaRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcDeptCodes.Te => await _teRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            _ => null
        };
    }

    /// <summary>
    /// 按明细解析制造二课执行行（优先按路由目标表）
    /// </summary>
    private async Task<object?> FirstPcbaEntityByDetailIdAsync(long ecDetailId)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail != null)
        {
            var route = TaktEcSmtRouteHelper.Resolve(detail);
            if (route != TaktEcSmtRouteTarget.None)
            {
                return await FirstSmtEntityByDetailAndTargetAsync(ecDetailId, route);
            }
        }
        var electronic = await _smtRepository.FirstAsync(x => x.EcDetailId == ecDetailId);
        if (electronic != null)
        {
            return electronic;
        }
        return await _seizounikaRepository.FirstAsync(x => x.EcDetailId == ecDetailId);
    }

    /// <summary>
    /// 按明细与目标表取制造二课执行行
    /// </summary>
    private async Task<object?> FirstSmtEntityByDetailAndTargetAsync(long ecDetailId, TaktEcSmtRouteTarget target)
    {
        return target switch
        {
            TaktEcSmtRouteTarget.Smt =>
                await _smtRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            TaktEcSmtRouteTarget.Seizounika =>
                await _seizounikaRepository.FirstAsync(x => x.EcDetailId == ecDetailId),
            _ => null
        };
    }

    /// <summary>
    /// 按设变单号与部门加载执行实体（禁止对十万 DetailId 做 Contains/IN）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>执行实体列表</returns>
    private async Task<List<object>> ListEntitiesByEcCodeAndDeptAsync(string ecCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            var rows = new List<object>();
            rows.AddRange(await _smtRepository.GetListAsync(x => x.EcCode == code));
            rows.AddRange(await _seizounikaRepository.GetListAsync(x => x.EcCode == code));
            return rows;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => (await _pmcRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Mp => (await _mpRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Iqc => (await _iqcRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Mc => (await _mcRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Assy => (await _assyRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Qa => (await _qaRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            TaktEcDeptCodes.Te => (await _teRepository.GetListAsync(x => x.EcCode == code)).Cast<object>().ToList(),
            _ => []
        };
    }

    /// <summary>
    /// 按明细 ID 列表与部门加载执行实体（仅小批量；大批量请用 ListEntitiesByEcCodeAndDeptAsync）
    /// </summary>
    private async Task<List<object>> ListEntitiesByDetailIdsAndDeptAsync(IReadOnlyList<long> detailIds, string deptCode)
    {
        if (detailIds == null || detailIds.Count == 0)
        {
            return [];
        }

        // SQL Server IN 参数上限约 2100；分片查询
        const int idChunk = 1000;
        var rows = new List<object>();
        for (var offset = 0; offset < detailIds.Count; offset += idChunk)
        {
            var chunk = detailIds.Skip(offset).Take(idChunk).ToList();
            if (deptCode == TaktEcDeptCodes.Pcba)
            {
                rows.AddRange(await _smtRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)));
                rows.AddRange(await _seizounikaRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)));
                continue;
            }

            rows.AddRange(deptCode switch
            {
                TaktEcDeptCodes.Pmc => (await _pmcRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Mp => (await _mpRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Iqc => (await _iqcRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Mc => (await _mcRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Assy => (await _assyRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Qa => (await _qaRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                TaktEcDeptCodes.Te => (await _teRepository.GetListAsync(x => chunk.Contains(x.EcDetailId))).Cast<object>(),
                _ => Enumerable.Empty<object>()
            });
        }

        return rows;
    }

    /// <summary>
    /// 按明细 ID 列表加载全部部门执行实体
    /// </summary>
    private async Task<List<object>> ListAllEntitiesByDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        var rows = new List<object>();
        rows.AddRange(await _pmcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _mpRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _iqcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _mcRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _smtRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _seizounikaRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _assyRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _qaRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        rows.AddRange(await _teRepository.GetListAsync(x => detailIds.Contains(x.EcDetailId)));
        return rows;
    }

    /// <summary>
    /// 创建或复用部门执行实体，并从明细同步冗余字段
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="existing">已有执行实体；为空则新建</param>
    /// <param name="lineNumber">行号</param>
    /// <param name="applyNotRelatedAuto">是否按采购类型/检验写入「无关」自动完成（默认否；仅全仕向/部管由 Upsert 显式传入 true）</param>
    private static object CreateConcreteExec(
        TaktEcDetail detail,
        string deptCode,
        object? existing,
        int lineNumber,
        bool applyNotRelatedAuto = false)
    {
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            var route = TaktEcSmtRouteHelper.Resolve(detail);
            if (route == TaktEcSmtRouteTarget.None)
            {
                throw new InvalidOperationException("制造二课当前明细无有效执行表路由（F 且非 C003）");
            }
            return CreatePcbaConcreteExec(detail, route, existing, lineNumber, applyNotRelatedAuto);
        }
        var isNew = existing == null;
        var exec = existing ?? deptCode switch
        {
            TaktEcDeptCodes.Pmc => new TaktEcSeikan { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Mp => new TaktEcKoubai { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Iqc => new TaktEcUkeken { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Mc => new TaktEcBukan { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Assy => new TaktEcSeizouikka { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Qa => new TaktEcHinkan { EcDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Te => new TaktEcSeizougijutsu { EcDetailId = detail.Id, DeptCode = deptCode },
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        return exec;
    }

    /// <summary>
    /// 创建或复用制造二课目标表实体
    /// </summary>
    private static object CreatePcbaConcreteExec(
        TaktEcDetail detail,
        TaktEcSmtRouteTarget target,
        object? existing,
        int lineNumber,
        bool applyNotRelatedAuto)
    {
        object exec;
        var isNew = false;
        if (target == TaktEcSmtRouteTarget.Smt)
        {
            if (existing is TaktEcSmt electronic)
            {
                exec = electronic;
            }
            else
            {
                isNew = true;
                exec = new TaktEcSmt { EcDetailId = detail.Id, DeptCode = TaktEcDeptCodes.Pcba };
            }
        }
        else if (target == TaktEcSmtRouteTarget.Seizounika)
        {
            if (existing is TaktEcSeizounika seizounika)
            {
                exec = seizounika;
            }
            else
            {
                isNew = true;
                exec = new TaktEcSeizounika { EcDetailId = detail.Id, DeptCode = TaktEcDeptCodes.Pcba };
            }
        }
        else
        {
            throw new InvalidOperationException("制造二课路由目标无效");
        }
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        return exec;
    }

    /// <summary>
    /// 从设变明细同步冗余字段到部门执行行（新建时写入行号）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号</param>
    /// <param name="applyNotRelatedAuto">是否按采购类型/检验写入「无关」自动完成（默认否）</param>
    private static void ApplyDetailRedundantFields(
        object exec,
        TaktEcDetail detail,
        int? lineNumberForCreate,
        bool applyNotRelatedAuto = false)
    {
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, lineNumberForCreate);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
    }

    /// <summary>
    /// 按 DeptCode 解析部门名称（优先 TaktDept.DeptName1，缺失回退课别显示名）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>部门名称</returns>
    private async Task<string> ResolveDeptNameAsync(string deptCode)
    {
        var fallback = TaktEcDeptCodes.GetDisplayName(deptCode);
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return fallback;
        }
        var dept = await _deptRepository.FirstAsync(x => x.DeptCode == deptCode);
        if (dept != null && !string.IsNullOrWhiteSpace(dept.DeptName1))
        {
            return dept.DeptName1.Trim();
        }
        return fallback;
    }

    /// <summary>
    /// 回填部门名称；preferred 非空则强制写入，否则空值时用课别显示名兜底
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="preferredName">已解析的部门名称（批量路径一次解析后传入）</param>
    private static void EnsureDeptName(object exec, string? preferredName = null)
    {
        if (exec is not ITaktEcDeptExecEntity entity)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(preferredName))
        {
            entity.DeptName = preferredName.Trim();
            return;
        }
        if (string.IsNullOrWhiteSpace(entity.DeptName))
        {
            entity.DeptName = TaktEcDeptCodes.GetDisplayName(entity.DeptCode);
        }
    }

    /// <summary>
    /// 保存执行实体到对应部门表，并回写明细子表外键 EcXxxId（一对多）
    /// </summary>
    private async Task<object> SaveEntityAsync(object exec, string deptCode, bool isNew)
    {
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            return await SavePcbaEntityAsync(exec, isNew);
        }
        var saved = deptCode switch
        {
            TaktEcDeptCodes.Pmc => (object)await SaveTypedAsync(_pmcRepository, (TaktEcSeikan)exec, isNew),
            TaktEcDeptCodes.Mp => await SaveTypedAsync(_mpRepository, (TaktEcKoubai)exec, isNew),
            TaktEcDeptCodes.Iqc => await SaveTypedAsync(_iqcRepository, (TaktEcUkeken)exec, isNew),
            TaktEcDeptCodes.Mc => await SaveTypedAsync(_mcRepository, (TaktEcBukan)exec, isNew),
            TaktEcDeptCodes.Assy => await SaveTypedAsync(_assyRepository, (TaktEcSeizouikka)exec, isNew),
            TaktEcDeptCodes.Qa => await SaveTypedAsync(_qaRepository, (TaktEcHinkan)exec, isNew),
            TaktEcDeptCodes.Te => await SaveTypedAsync(_teRepository, (TaktEcSeizougijutsu)exec, isNew),
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };
        return saved;
    }

    /// <summary>
    /// 保存制造二课执行实体
    /// </summary>
    private async Task<object> SavePcbaEntityAsync(object exec, bool isNew)
    {
        var saved = exec switch
        {
            TaktEcSmt electronic => (object)await SaveTypedAsync(_smtRepository, electronic, isNew),
            TaktEcSeizounika seizounika => await SaveTypedAsync(_seizounikaRepository, seizounika, isNew),
            _ => throw new InvalidOperationException("制造二课执行实体类型不匹配")
        };
        return saved;
    }

    /// <summary>
    /// 填充执行 DTO 视图子表明细（按业务键 + IsObsolete==0；含各部门可见性过滤）
    /// </summary>
    /// <param name="savedExec">执行实体</param>
    /// <param name="dto">对应响应 DTO</param>
    public async Task FillExecViewDetailsAsync(object savedExec, object dto)
    {
        ArgumentNullException.ThrowIfNull(savedExec);
        ArgumentNullException.ThrowIfNull(dto);
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        switch (savedExec)
        {
            case TaktEcSeikan e when dto is TaktEcSeikanDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcFinishedGoods == e.EcFinishedGoods
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcKoubai e when dto is TaktEcKoubaiDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewMaterialCode == e.EcNewMaterialCode
                        && x.EcNewPurchaseType == purchaseTypeF
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcUkeken e when dto is TaktEcUkekenDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewMaterialCode == e.EcNewMaterialCode
                        && x.EcNewRequiresInspection == 1
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcBukan e when dto is TaktEcBukanDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcNewMaterialCode == e.EcNewMaterialCode
                        && x.EcNewPurchaseType == purchaseTypeF
                        && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcSmt e when dto is TaktEcSmtDto d:
                var smtParent = e.EcParentMaterialCode ?? string.Empty;
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcParentMaterialCode == smtParent
                        && x.EcNewPurchaseType == purchaseTypeF
                        && x.EcNewWarehouse != null
                        && x.EcNewWarehouse == warehouseC003
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcSeizounika e when dto is TaktEcSeizounikaDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcFinishedGoods == e.EcFinishedGoods
                        && x.EcNewPurchaseType != purchaseTypeF
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcSeizouikka e when dto is TaktEcSeizouikkaDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcFinishedGoods == e.EcFinishedGoods
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcHinkan e when dto is TaktEcHinkanDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcFinishedGoods == e.EcFinishedGoods
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcSeizougijutsu e when dto is TaktEcSeizougijutsuDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcModelCode == e.EcModelCode
                        && x.EcFinishedGoods == e.EcFinishedGoods
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
        }
    }

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
            if (deptCode == TaktEcDeptCodes.Pcba)
            {
                created += await EnsurePcbaDeptExecRowAsync(detail);
                continue;
            }
            if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(deptCode)
                && !TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
            {
                await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
                continue;
            }
            if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(deptCode)
                && !IsNewMaterialDeptListVisible(detail, deptCode))
            {
                await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
                continue;
            }
            if (await ShouldSkipNewMaterialDeptByListDedupAsync(detail, deptCode))
            {
                await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
                continue;
            }
            if (await ShouldSkipModelFinishedGoodsDeptByListDedupAsync(detail, deptCode))
            {
                await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
                continue;
            }
            var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
            if (existing != null)
            {
                ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
                if (existing is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
                {
                    deptExec.IsObsolete = 0;
                }
                await SaveEntityAsync(existing, deptCode, false);
                continue;
            }
            var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
            var exec = CreateConcreteExec(detail, deptCode, null, lineNumber, applyNotRelatedAuto: false);
            await SaveEntityAsync(exec, deptCode, true);
            created += 1;
        }
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
        return created;
    }

    /// <summary>
    /// 制造二课 Ensure：按路由作废对侧并创建/同步目标表
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>新建行数（0 或 1）</returns>
    private async Task<int> EnsurePcbaDeptExecRowAsync(TaktEcDetail detail)
    {
        var route = TaktEcSmtRouteHelper.Resolve(detail);
        if (route == TaktEcSmtRouteTarget.None)
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        if (route == TaktEcSmtRouteTarget.Smt
            && !TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        if (route == TaktEcSmtRouteTarget.Smt
            && await HasNewerPcbaVisibleSiblingAsync(detail))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        if (route == TaktEcSmtRouteTarget.Seizounika
            && await HasNewerSeizounikaVisibleSiblingAsync(detail))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        await ObsoleteTypedDeptExecIfExistsAsync(
            detail.Id,
            route == TaktEcSmtRouteTarget.Smt
                ? TaktEcSmtRouteTarget.Seizounika
                : TaktEcSmtRouteTarget.Smt);
        var existingPcba = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, route);
        if (existingPcba != null)
        {
            ApplyDetailRedundantFields(existingPcba, detail, null, applyNotRelatedAuto: false);
            if (existingPcba is ITaktEcDeptExecEntity existingDept && existingDept.IsObsolete == 1)
            {
                existingDept.IsObsolete = 0;
            }
            await SavePcbaEntityAsync(existingPcba, false);
            return 0;
        }
        var pcbaLine = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var pcbaExec = CreatePcbaConcreteExec(detail, route, null, pcbaLine, applyNotRelatedAuto: false);
        await SavePcbaEntityAsync(pcbaExec, true);
        return 1;
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
                if (normalizedDeptCode == TaktEcDeptCodes.Pcba)
                {
                    await EnsurePcbaDeptExecRowAsync(detail);
                    continue;
                }
                if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(normalizedDeptCode)
                    && !TaktEcDistinctionConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
                {
                    await ObsoleteDeptExecIfExistsAsync(detail.Id, normalizedDeptCode);
                    continue;
                }
                if (TaktEcDistinctionConstants.IsNewMaterialDependentDept(normalizedDeptCode)
                    && !IsNewMaterialDeptListVisible(detail, normalizedDeptCode))
                {
                    await ObsoleteDeptExecIfExistsAsync(detail.Id, normalizedDeptCode);
                    continue;
                }
                if (await ShouldSkipNewMaterialDeptByListDedupAsync(detail, normalizedDeptCode))
                {
                    await ObsoleteDeptExecIfExistsAsync(detail.Id, normalizedDeptCode);
                    continue;
                }
                if (await ShouldSkipModelFinishedGoodsDeptByListDedupAsync(detail, normalizedDeptCode))
                {
                    await ObsoleteDeptExecIfExistsAsync(detail.Id, normalizedDeptCode);
                    continue;
                }
                var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, normalizedDeptCode);
                if (existing != null)
                {
                    ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
                    if (existing is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
                    {
                        deptExec.IsObsolete = 0;
                    }
                    await SaveEntityAsync(existing, normalizedDeptCode, false);
                    continue;
                }
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = CreateConcreteExec(detail, normalizedDeptCode, null, lineNumber, applyNotRelatedAuto: false);
                await SaveEntityAsync(exec, normalizedDeptCode, true);
            }
        }
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodesAsync(details.Select(x => x.EcCode));
    }

    /// <summary>
    /// 泛型保存（落库前把历史短文案规范为「管理区分-…」）
    /// </summary>
    private static async Task<TEntity> SaveTypedAsync<TEntity>(ITaktCompanyRepository<TEntity> repository, TEntity entity, bool isNew)
        where TEntity : TaktCompanyEntityBase, new()
    {
        TaktEcDeptEntityHelper.SetExecContent(entity, TaktEcDeptEntityHelper.GetExecContent(entity), overwrite: true);
        if (isNew)
        {
            return await repository.CreateAsync(entity);
        }
        await repository.UpdateAsync(entity);
        return entity;
    }
}