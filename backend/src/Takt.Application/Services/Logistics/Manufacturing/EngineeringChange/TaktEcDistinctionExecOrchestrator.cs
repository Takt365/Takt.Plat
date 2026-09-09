// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDistinctionExecOrchestrator.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：技术课保存后按管理区分生成各部门执行行（唯一编排入口）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变区分 → 部门执行行编排（新增/更新/来源导入共用这一条链路）
/// </summary>
public class TaktEcDistinctionExecOrchestrator
{
    private readonly TaktEcExecPersistence _ecExecPersistence;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecExecPersistence">部门执行持久化</param>
    public TaktEcDistinctionExecOrchestrator(TaktEcExecPersistence ecExecPersistence)
    {
        _ecExecPersistence = ecExecPersistence;
    }

    /// <summary>
    /// 按主表区分与明细生成或刷新各部门执行行（按部门批量落库）。
    /// </summary>
    /// <param name="gijutsu">设变技术课主</param>
    /// <param name="details">设变明细（通常已过滤作废）</param>
    /// <returns>各部门写入统计（供日志与完成通知）</returns>
    public async Task<TaktEcDistinctionExecApplyResult> ApplyAsync(
        TaktEcGijutsu gijutsu,
        IReadOnlyList<TaktEcDetail> details)
    {
        ArgumentNullException.ThrowIfNull(gijutsu);
        if (details == null || details.Count == 0)
        {
            return TaktEcDistinctionExecApplyResult.Empty;
        }
        var active = details.Where(x => x.IsObsolete == 0).ToList();
        if (active.Count == 0)
        {
            return TaktEcDistinctionExecApplyResult.Empty;
        }

        var ecCode = gijutsu.EcCode?.Trim() ?? string.Empty;
        var needPerDetail = active.Count;
        var deptCount = TaktEcDeptCodes.KanbanOrder.Length;
        var needTotalMax = checked(needPerDetail * deptCount);
        TaktLogger.Information(
            "[EcGijutsuPersist] 派生计划 EcCode={EcCode} 明细数={DetailCount} 部门数={DeptCount} 最大待处理={NeedTotal}（每部门需处理明细={NeedPerDept}）",
            ecCode,
            needPerDetail,
            deptCount,
            needTotalMax,
            needPerDetail);

        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        var grandCompleted = 0;
        var grandSkipped = 0;
        var totalSw = Stopwatch.StartNew();
        foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
        {
            var deptName = TaktEcDeptCodes.GetDisplayName(deptCode);
            var needCount = needPerDetail;
            TaktLogger.Information(
                "[EcGijutsuPersist] {DeptName}开始派生 需要记录数={NeedCount} EcCode={EcCode} DeptCode={DeptCode}",
                deptName,
                needCount,
                ecCode,
                deptCode);

            var deptSw = Stopwatch.StartNew();
            var batch = await _ecExecPersistence.UpsertDeptExecBatchWithFillModeAsync(
                active,
                deptCode,
                detail => ShouldAutoCompleteExec(gijutsu.EcDistinction, deptCode, detail),
                gijutsu.EcDistinction);
            deptSw.Stop();

            grandCompleted += batch.SavedCount;
            grandSkipped += batch.SkippedCount;
            if (batch.SavedCount > 0)
            {
                counts[deptCode] = batch.SavedCount;
            }
            TaktLogger.Information(
                "[EcGijutsuPersist] {DeptName}完成 需要记录数={NeedCount} 完成记录数={SavedCount} 跳过={SkippedCount} 耗时={ElapsedMs}ms EcCode={EcCode} DeptCode={DeptCode}",
                deptName,
                needCount,
                batch.SavedCount,
                batch.SkippedCount,
                deptSw.ElapsedMilliseconds,
                ecCode,
                deptCode);
        }
        totalSw.Stop();

        var result = TaktEcDistinctionExecApplyResult.FromCounts(counts);
        TaktLogger.Information(
            "[EcGijutsuPersist] 各部门执行行派生结束 EcCode={EcCode} 需要合计(明细×部门)={NeedTotal} 完成合计={Completed} 跳过合计={Skipped} 耗时={ElapsedMs}ms Summary={Summary}",
            ecCode,
            needTotalMax,
            grandCompleted,
            grandSkipped,
            totalSw.ElapsedMilliseconds,
            string.IsNullOrWhiteSpace(result.FormatSummary()) ? "(无写入)" : result.FormatSummary());
        return result;
    }

    /// <summary>
    /// 该部门执行行是否按区分自动填完（false=待人工填写）
    /// </summary>
    /// <param name="ecDistinction">管理区分</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="detail">设变明细</param>
    /// <returns>是否自动填完</returns>
    private static bool ShouldAutoCompleteExec(int ecDistinction, string deptCode, TaktEcDetail detail)
    {
        if (ecDistinction == TaktEcDistinctionConstants.Internal
            || ecDistinction == TaktEcDistinctionConstants.Technical)
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
        if (ecDistinction != TaktEcDistinctionConstants.AllDestination)
        {
            return true;
        }
        var purchaseTypeF = TaktEcDistinctionConstants.IsExternalPurchaseType(detail.EcNewPurchaseType);
        var requiresInspection = detail.EcNewRequiresInspection == 1;
        var bukanNeedFill = TaktEcDistinctionConstants.IsBukanVisible(
            detail.EcNewPurchaseType,
            detail.EcNewWarehouse);
        var needFill = deptCode switch
        {
            TaktEcDeptCodes.Mp => purchaseTypeF,
            TaktEcDeptCodes.Iqc => purchaseTypeF,
            TaktEcDeptCodes.Mc => bukanNeedFill,
            TaktEcDeptCodes.Qa => requiresInspection,
            TaktEcDeptCodes.Pcba => true,
            _ => false
        };
        return !needFill;
    }
}
