// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecPersistence.Batch.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行批量派生（预加载 + CreateRange/UpdateRange，避免逐行 N+1）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行聚合持久化（批量派生扩展）
/// </summary>
public partial class TaktEcExecPersistence
{
    /// <summary>
    /// 批量派生单部门执行行结果
    /// </summary>
    public readonly record struct TaktEcDeptExecBatchResult(int SavedCount, int SkippedCount);

    /// <summary>
    /// 按明细列表批量 Upsert 单部门执行行（编排层专用；禁止再逐行查库/回写）
    /// </summary>
    /// <param name="details">同一设变下未作废明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="shouldAutoComplete">是否自动填完（按明细）</param>
    /// <param name="ecScope">设变实施范围</param>
    /// <returns>写入/跳过条数</returns>
    public async Task<TaktEcDeptExecBatchResult> UpsertDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecScope)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        ArgumentNullException.ThrowIfNull(shouldAutoComplete);
        if (details.Count == 0)
        {
            return new TaktEcDeptExecBatchResult(0, 0);
        }
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            return await UpsertPcbaDeptExecBatchWithFillModeAsync(details, shouldAutoComplete, ecScope);
        }
        return await UpsertTypedDeptExecBatchWithFillModeAsync(details, deptCode, shouldAutoComplete, ecScope);
    }

    /// <summary>
    /// 非制造二课：单表批量 Upsert
    /// </summary>
    private async Task<TaktEcDeptExecBatchResult> UpsertTypedDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        var existingList = !string.IsNullOrWhiteSpace(ecCode)
            ? await ListEntitiesByEcCodeAndDeptAsync(ecCode, deptCode)
            : await ListEntitiesByDetailIdsAndDeptAsync(details.Select(x => x.Id).ToList(), deptCode);
        var existingByDetailId = existingList
            .OfType<ITaktEcDeptExecEntity>()
            .GroupBy(x => x.EcDetailId)
            .ToDictionary(g => g.Key, g => (object)g.First());
        var skipByDedup = BuildListDedupSkipDetailIds(details, deptCode);
        if (skipByDedup.Count > 0)
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 部门执行去重 DeptCode={DeptCode} DetailCount={DetailCount} SkipByDedup={SkipCount}",
                deptCode,
                details.Count,
                skipByDedup.Count);
        }
        var applyNotRelatedAuto = ecScope == TaktEcScopeConstants.AllDestination
            || ecScope == TaktEcScopeConstants.MaterialControl;
        var deptName = await ResolveDeptNameAsync(deptCode);
        var toCreate = new List<object>();
        var toUpdate = new List<object>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            existingByDetailId.TryGetValue(detail.Id, out var existing);
            if (TaktEcScopeConstants.IsNewMaterialDependentDept(deptCode)
                && !TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            // 采购/受检/部管：非列表可见条件不落库（与 QueryHelper 一致；禁止对非可见明细灌满执行表）
            if (TaktEcScopeConstants.IsNewMaterialDependentDept(deptCode)
                && !IsNewMaterialDeptListVisible(detail, deptCode))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            if (skipByDedup.Contains(detail.Id))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            var isNew = existing == null;
            var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
            var exec = CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto: false);
            EnsureDeptName(exec, deptName);
            if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
            {
                deptExec.IsObsolete = 0;
            }
            ApplyScopeFillMode(exec, isNew, shouldAutoComplete(detail), ecScope, detail);
            var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
            var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
            if (!isEolFilled && applyNotRelatedAuto)
            {
                TaktEcExecNotRelated.TryAfterFill(exec, detail);
            }
            NormalizeExecContent(exec);
            if (isNew)
            {
                toCreate.Add(exec);
            }
            else
            {
                toUpdate.Add(exec);
            }
            savedCount++;
        }
        await SaveTypedDeptRangeAsync(deptCode, toCreate, toUpdate);
        TaktLogger.Information(
            "[EcGijutsuPersist] 部门执行已落库 DeptCode={DeptCode} Saved={Saved} Skipped={Skipped}",
            deptCode,
            savedCount,
            skippedCount);
        return new TaktEcDeptExecBatchResult(savedCount, skippedCount);
    }

    /// <summary>
    /// 制造二课双表批量 Upsert
    /// </summary>
    private async Task<TaktEcDeptExecBatchResult> UpsertPcbaDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        Dictionary<long, TaktEcSmt> smtExisting;
        Dictionary<long, TaktEcSeizounika> seizounikaExisting;
        if (!string.IsNullOrWhiteSpace(ecCode))
        {
            smtExisting = (await _smtRepository.GetListAsync(x => x.EcCode == ecCode))
                .GroupBy(x => x.EcDetailId)
                .ToDictionary(g => g.Key, g => g.First());
            seizounikaExisting = (await _seizounikaRepository.GetListAsync(x => x.EcCode == ecCode))
                .GroupBy(x => x.EcDetailId)
                .ToDictionary(g => g.Key, g => g.First());
        }
        else
        {
            var detailIds = details.Select(x => x.Id).ToList();
            smtExisting = new Dictionary<long, TaktEcSmt>();
            seizounikaExisting = new Dictionary<long, TaktEcSeizounika>();
            const int idChunk = 1000;
            for (var offset = 0; offset < detailIds.Count; offset += idChunk)
            {
                var chunk = detailIds.Skip(offset).Take(idChunk).ToList();
                foreach (var row in await _smtRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)))
                {
                    smtExisting.TryAdd(row.EcDetailId, row);
                }
                foreach (var row in await _seizounikaRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)))
                {
                    seizounikaExisting.TryAdd(row.EcDetailId, row);
                }
            }
        }
        var skipSmt = BuildPcbaSmtSkipDetailIds(details);
        var skipSeizounika = BuildPcbaSeizounikaSkipDetailIds(details);
        var applyNotRelatedAuto = ecScope == TaktEcScopeConstants.AllDestination
            || ecScope == TaktEcScopeConstants.MaterialControl;
        var smtDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Smt);
        var seizounikaDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Pcba);
        var smtCreate = new List<TaktEcSmt>();
        var smtUpdate = new List<TaktEcSmt>();
        var seizounikaCreate = new List<TaktEcSeizounika>();
        var seizounikaUpdate = new List<TaktEcSeizounika>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            var route = TaktEcSmtRouteHelper.Resolve(detail);
            smtExisting.TryGetValue(detail.Id, out var smtRow);
            seizounikaExisting.TryGetValue(detail.Id, out var seizounikaRow);
            if (route == TaktEcSmtRouteTarget.None)
            {
                if (TryMarkObsolete(smtRow))
                {
                    smtUpdate.Add(smtRow!);
                }
                if (TryMarkObsolete(seizounikaRow))
                {
                    seizounikaUpdate.Add(seizounikaRow!);
                }
                skippedCount++;
                continue;
            }
            if (route == TaktEcSmtRouteTarget.Smt)
            {
                if (!TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode)
                    || skipSmt.Contains(detail.Id))
                {
                    if (TryMarkObsolete(smtRow))
                    {
                        smtUpdate.Add(smtRow!);
                    }
                    if (TryMarkObsolete(seizounikaRow))
                    {
                        seizounikaUpdate.Add(seizounikaRow!);
                    }
                    skippedCount++;
                    continue;
                }
                if (TryMarkObsolete(seizounikaRow))
                {
                    seizounikaUpdate.Add(seizounikaRow!);
                }
                var isNew = smtRow == null;
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = (TaktEcSmt)CreatePcbaConcreteExec(
                    detail,
                    TaktEcSmtRouteTarget.Smt,
                    smtRow,
                    lineNumber,
                    applyNotRelatedAuto: false);
                EnsureDeptName(exec, smtDeptName);
                if (exec.IsObsolete == 1)
                {
                    exec.IsObsolete = 0;
                }
                ApplyScopeFillMode(exec, isNew, shouldAutoComplete(detail), ecScope, detail);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
                NormalizeExecContent(exec);
                if (isNew)
                {
                    smtCreate.Add(exec);
                }
                else
                {
                    smtUpdate.Add(exec);
                }
                savedCount++;
                continue;
            }
            if (skipSeizounika.Contains(detail.Id))
            {
                if (TryMarkObsolete(smtRow))
                {
                    smtUpdate.Add(smtRow!);
                }
                if (TryMarkObsolete(seizounikaRow))
                {
                    seizounikaUpdate.Add(seizounikaRow!);
                }
                skippedCount++;
                continue;
            }
            if (TryMarkObsolete(smtRow))
            {
                smtUpdate.Add(smtRow!);
            }
            {
                var isNew = seizounikaRow == null;
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = (TaktEcSeizounika)CreatePcbaConcreteExec(
                    detail,
                    TaktEcSmtRouteTarget.Seizounika,
                    seizounikaRow,
                    lineNumber,
                    applyNotRelatedAuto: false);
                EnsureDeptName(exec, seizounikaDeptName);
                if (exec.IsObsolete == 1)
                {
                    exec.IsObsolete = 0;
                }
                ApplyScopeFillMode(exec, isNew, shouldAutoComplete(detail), ecScope, detail);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
                NormalizeExecContent(exec);
                if (isNew)
                {
                    seizounikaCreate.Add(exec);
                }
                else
                {
                    seizounikaUpdate.Add(exec);
                }
                savedCount++;
            }
        }
        if (smtCreate.Count > 0)
        {
            await _smtRepository.CreateRangeAsync(smtCreate);
        }
        if (smtUpdate.Count > 0)
        {
            await _smtRepository.UpdateRangeAsync(smtUpdate);
        }
        if (seizounikaCreate.Count > 0)
        {
            await _seizounikaRepository.CreateRangeAsync(seizounikaCreate);
        }
        if (seizounikaUpdate.Count > 0)
        {
            await _seizounikaRepository.UpdateRangeAsync(seizounikaUpdate);
        }
        return new TaktEcDeptExecBatchResult(savedCount, skippedCount);
    }

    /// <summary>
    /// 填充后「无关」补齐（与单行 Upsert 一致）
    /// </summary>
    private static void ApplyPostFillNotRelated(
        object exec,
        TaktEcDetail detail,
        bool applyNotRelatedAuto)
    {
        if (!applyNotRelatedAuto)
        {
            return;
        }
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
        if (!isEolFilled)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
    }

    /// <summary>
    /// 规范化执行内容字段
    /// </summary>
    private static void NormalizeExecContent(object entity)
    {
        TaktEcDeptEntityHelper.SetExecContent(
            entity,
            TaktEcDeptEntityHelper.GetExecContent(entity),
            overwrite: true);
    }

    /// <summary>
    /// 标记作废（已作废则 false）
    /// </summary>
    private static bool TryMarkObsolete(object? existing)
    {
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return false;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 按部门批量落库
    /// </summary>
    private async Task SaveTypedDeptRangeAsync(string deptCode, List<object> toCreate, List<object> toUpdate)
    {
        switch (deptCode)
        {
            case TaktEcDeptCodes.Pmc:
                await SaveTypedRangeAsync(_pmcRepository, toCreate.Cast<TaktEcSeikan>().ToList(), toUpdate.Cast<TaktEcSeikan>().ToList());
                break;
            case TaktEcDeptCodes.Mp:
                await SaveTypedRangeAsync(_mpRepository, toCreate.Cast<TaktEcKoubai>().ToList(), toUpdate.Cast<TaktEcKoubai>().ToList());
                break;
            case TaktEcDeptCodes.Iqc:
                await SaveTypedRangeAsync(_iqcRepository, toCreate.Cast<TaktEcUkeken>().ToList(), toUpdate.Cast<TaktEcUkeken>().ToList());
                break;
            case TaktEcDeptCodes.Mc:
                await SaveTypedRangeAsync(_mcRepository, toCreate.Cast<TaktEcBukan>().ToList(), toUpdate.Cast<TaktEcBukan>().ToList());
                break;
            case TaktEcDeptCodes.Assy:
                await SaveTypedRangeAsync(_assyRepository, toCreate.Cast<TaktEcSeizouikka>().ToList(), toUpdate.Cast<TaktEcSeizouikka>().ToList());
                break;
            case TaktEcDeptCodes.Qa:
                await SaveTypedRangeAsync(_qaRepository, toCreate.Cast<TaktEcHinkan>().ToList(), toUpdate.Cast<TaktEcHinkan>().ToList());
                break;
            case TaktEcDeptCodes.Te:
                await SaveTypedRangeAsync(_teRepository, toCreate.Cast<TaktEcSeizougijutsu>().ToList(), toUpdate.Cast<TaktEcSeizougijutsu>().ToList());
                break;
            default:
                throw new InvalidOperationException($"不支持的部门编码：{deptCode}");
        }
    }

    /// <summary>
    /// 泛型批量 Create/Update
    /// </summary>
    private static async Task SaveTypedRangeAsync<TEntity>(
        ITaktCompanyRepository<TEntity> repository,
        List<TEntity> toCreate,
        List<TEntity> toUpdate)
        where TEntity : TaktCompanyEntityBase, new()
    {
        if (toCreate.Count > 0)
        {
            await repository.CreateRangeAsync(toCreate);
        }
        if (toUpdate.Count > 0)
        {
            await repository.UpdateRangeAsync(toUpdate);
        }
    }

    /// <summary>
    /// 采购/受检/部管是否属于列表可见条件（不含组内去重）
    /// </summary>
    private static bool IsNewMaterialDeptListVisible(TaktEcDetail detail, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Mp => TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType),
            TaktEcDeptCodes.Iqc => detail.EcNewRequiresInspection == 1,
            TaktEcDeptCodes.Mc => TaktEcScopeConstants.IsBukanVisible(
                detail.EcNewPurchaseType,
                detail.EcNewWarehouse),
            _ => true
        };
    }

    /// <summary>
    /// 各部门列表去重：可见组内非最大 Id 跳过（与各课 QueryHelper 同键）
    /// </summary>
    private static HashSet<long> BuildListDedupSkipDetailIds(IReadOnlyList<TaktEcDetail> details, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa or TaktEcDeptCodes.Te
                => BuildModelRootMaterialSkipDetailIds(details),
            TaktEcDeptCodes.Mp => BuildKoubaiSkipDetailIds(details),
            TaktEcDeptCodes.Iqc => BuildUkekenSkipDetailIds(details),
            TaktEcDeptCodes.Mc => BuildBukanSkipDetailIds(details),
            _ => []
        };
    }

    /// <summary>
    /// 生管/制一/品管/制技：设变+机种+根物料编码去重，非最大 Id 跳过
    /// </summary>
    private static HashSet<long> BuildModelRootMaterialSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d => d.IsObsolete == 0);
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty,
            Finished: d.EcRootMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 采购课可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildKoubaiSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsExternalPurchaseType(d.EcNewPurchaseType)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Material: d.EcNewMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 受检课可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildUkekenSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            d.EcNewRequiresInspection == 1
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Material: d.EcNewMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 部管课可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildBukanSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsBukanVisible(d.EcNewPurchaseType, d.EcNewWarehouse)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty,
            Material: d.EcNewMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// SMT 可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildPcbaSmtSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsPcbaC003ExternalGroup(d.EcNewPurchaseType, d.EcNewWarehouse));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Parent: d.EcParentMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 制二非 F 可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildPcbaSeizounikaSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(d.EcNewPurchaseType));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty,
            Finished: d.EcRootMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }
}
