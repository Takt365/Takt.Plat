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
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcSeikan> _pmcRepository;
    private readonly ITaktCompanyRepository<TaktEcKoubai> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcUkeken> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcBukan> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizounika> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizouikka> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcHinkan> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcSeizougijutsu> _teRepository;

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
        ITaktCompanyRepository<TaktEcSeizounika> pcbaRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository)
    {
        _deptAccess = deptAccess;
        _ecGijutsuStatusSynchronizer = ecGijutsuStatusSynchronizer;
        _ecGijutsuRepository = ecGijutsuRepository;
        _ecDetailRepository = ecDetailRepository;
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
        var existingRows = await _deptAccess.ListBaseByEcnDetailIdAsync(ecnDetailId);
        var ecCode = existingRows.FirstOrDefault()?.EcCode;
        await _deptAccess.DeleteAllByEcnDetailIdAsync(ecnDetailId);
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
        await _pcbaRepository.DeleteAsync(x => x.EcCode == code);
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
        var exec = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
        var isNew = exec == null;
        exec = CreateConcreteExec(detail, deptCode, exec, await lineNumberGenerator(), applyNotRelatedAuto: false);
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
            await FanOutSeizounikaFillableByEcAndParentMaterialAsync(seizounika);
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
            await TryCascadeAfterGateDeptCompletedByDetailIdAsync(row.EcnDetailId, TaktEcDeptCodes.Mc, row);
        }
    }

    /// <summary>
    /// 制二课：将可填字段同步到同设变单号+上阶物料且同页签分组（F+C003 或其它）的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制二执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSeizounikaFillableByEcAndParentMaterialAsync(TaktEcSeizounika source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
        if (detail == null)
        {
            return;
        }
        var isC003Group = TaktEcDistinctionConstants.IsPcbaC003ExternalGroup(
            detail.EcNewPurchaseType,
            detail.EcNewWarehouse);
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var siblingDetails = isC003Group
            ? await _ecDetailRepository.GetListAsync(x =>
                x.EcCode == detail.EcCode
                && x.EcParentMaterialCode == detail.EcParentMaterialCode
                && x.EcNewPurchaseType == purchaseTypeF
                && x.EcNewWarehouse != null
                && x.EcNewWarehouse == warehouseC003
                && x.IsObsolete == 0)
            : await _ecDetailRepository.GetListAsync(x =>
                x.EcCode == detail.EcCode
                && x.EcParentMaterialCode == detail.EcParentMaterialCode
                && (x.EcNewPurchaseType != purchaseTypeF
                    || x.EcNewWarehouse == null
                    || x.EcNewWarehouse != warehouseC003)
                && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _pcbaRepository.GetListAsync(x =>
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionDate = source.ProductionDate;
            row.ProductionBatch = source.ProductionBatch;
            row.ProductionTeam = source.ProductionTeam;
            row.OutboundOrderCode = source.OutboundOrderCode;
        }
        await _pcbaRepository.UpdateRangeAsync(rows);
        if (source.IsImplemented != 1 || !isC003Group)
        {
            return;
        }
        foreach (var row in rows)
        {
            await TryCascadeAfterGateDeptCompletedByDetailIdAsync(row.EcnDetailId, TaktEcDeptCodes.Pcba, row);
        }
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
            row.OldProductHandling = source.OldProductHandling;
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
        var detail = await _ecDetailRepository.GetByIdAsync(source.EcnDetailId);
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
            detailIds.Contains(x.EcnDetailId) && x.IsObsolete == 0 && x.Id != source.Id);
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
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="autoComplete">true=按区分自动填写；false=待人工填写</param>
    /// <param name="ecDistinction">设变区分（字典 logistics_manufacturing_ec_distinction_category）</param>
    /// <returns>部门执行实体</returns>
    public async Task<object> UpsertDeptExecWithFillModeAsync(
        TaktEcDetail detail,
        string deptCode,
        bool autoComplete,
        int ecDistinction)
    {
        ArgumentNullException.ThrowIfNull(detail);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
        var isNew = existing == null;
        var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var applyNotRelatedAuto = ecDistinction == TaktEcDistinctionConstants.AllDestination
            || ecDistinction == TaktEcDistinctionConstants.MaterialControl;
        var exec = CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto);
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
            TryApplyKoubaiNotPurchasingRelated(exec, detail);
            TryApplyUkekenNotRelatedToIqc(exec, detail);
            TryApplyBukanNotRelatedToMaterialControl(exec, detail);
        }
        var saved = await SaveEntityAsync(exec, deptCode, isNew);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
        return saved;
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
            await UpsertDeptExecWithFillModeAsync(
                detail,
                otherDept,
                autoComplete: autoCompleteOther,
                TaktEcDistinctionConstants.MaterialControl);
        }
    }

    /// <summary>
    /// 按明细 ID 触发门禁部门完成后的级联（CRUD 写路径用）
    /// </summary>
    /// <param name="ecnDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="savedExec">已保存的部门执行实体</param>
    /// <returns>任务</returns>
    public async Task TryCascadeAfterGateDeptCompletedByDetailIdAsync(
        long ecnDetailId,
        string deptCode,
        object savedExec)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecnDetailId);
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
                mp.PurchaseOrderCode = dto.PurchaseOrderCode;
                break;
            case TaktEcUkeken iqc:
                iqc.IqcOrderCode = dto.IqcOrderCode;
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
                pcba.OutboundOrderCode = dto.OutboundOrderCode;
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
        var isNew = existing == null;
        var exec = existing ?? deptCode switch
        {
            TaktEcDeptCodes.Pmc => new TaktEcSeikan { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Mp => new TaktEcKoubai { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Iqc => new TaktEcUkeken { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Mc => new TaktEcBukan { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Pcba => new TaktEcSeizounika { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Assy => new TaktEcSeizouikka { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Qa => new TaktEcHinkan { EcnDetailId = detail.Id, DeptCode = deptCode },
            TaktEcDeptCodes.Te => new TaktEcSeizougijutsu { EcnDetailId = detail.Id, DeptCode = deptCode },
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };
        ApplyDetailRedundantFields(exec, detail, isNew ? lineNumber : null, applyNotRelatedAuto);
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
        switch (exec)
        {
            case TaktEcSeikan e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                break;
            case TaktEcKoubai e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                if (applyNotRelatedAuto)
                {
                    TryApplyKoubaiNotPurchasingRelated(e, detail);
                }
                break;
            case TaktEcUkeken e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                if (applyNotRelatedAuto)
                {
                    TryApplyUkekenNotRelatedToIqc(e, detail);
                }
                break;
            case TaktEcBukan e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                if (applyNotRelatedAuto)
                {
                    TryApplyBukanNotRelatedToMaterialControl(e, detail);
                }
                break;
            case TaktEcSeizounika e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                break;
            case TaktEcSeizouikka e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                break;
            case TaktEcHinkan e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                break;
            case TaktEcSeizougijutsu e:
                e.EcCode = detail.EcCode;
                if (lineNumberForCreate.HasValue)
                {
                    e.LineNumber = lineNumberForCreate.Value;
                }
                e.EcModelCode = detail.EcModelCode ?? string.Empty;
                e.EcFinishedGoods = detail.EcFinishedGoods;
                e.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription;
                e.EcParentMaterialCode = detail.EcParentMaterialCode;
                e.EcParentMaterialDescription = detail.EcParentMaterialDescription;
                e.DiscontinuedStatus = detail.DiscontinuedStatus;
                break;
        }
    }

    /// <summary>
    /// 新采购类型非 F 时：采购课执行内容为采购无关，清空订单字段并实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    private static void TryApplyKoubaiNotPurchasingRelated(object exec, TaktEcDetail detail)
    {
        if (exec is not TaktEcKoubai koubai)
        {
            return;
        }
        if (TaktEcDistinctionConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return;
        }
        koubai.ExecContent = TaktEcKoubaiConstants.NotPurchasingRelatedExecContent;
        koubai.PurchaseOrderIssueDate = null;
        koubai.Supplier = null;
        koubai.PurchaseOrderCode = null;
        koubai.IsImplemented = 1;
    }

    /// <summary>
    /// 新品无需检验时：受检课执行内容为与 IQC 无关，清空受检字段并实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    private static void TryApplyUkekenNotRelatedToIqc(object exec, TaktEcDetail detail)
    {
        if (exec is not TaktEcUkeken ukeken)
        {
            return;
        }
        if (detail.EcNewRequiresInspection == 1)
        {
            return;
        }
        ukeken.ExecContent = TaktEcUkekenConstants.NotRelatedToIqcExecContent;
        ukeken.IqcOrderCode = null;
        ukeken.InspectionDate = null;
        ukeken.IsImplemented = 1;
    }

    /// <summary>
    /// 新采购类型非 F 或新品仓库为 C003 时：部管课执行内容为与部管无关，清空出库字段并实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    private static void TryApplyBukanNotRelatedToMaterialControl(object exec, TaktEcDetail detail)
    {
        if (exec is not TaktEcBukan bukan)
        {
            return;
        }
        if (TaktEcDistinctionConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        bukan.ExecContent = TaktEcBukanConstants.NotRelatedToMaterialControlExecContent;
        bukan.OutboundBatch = null;
        bukan.OutboundDate = null;
        bukan.IsImplemented = 1;
    }

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
                ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
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
                    ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
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