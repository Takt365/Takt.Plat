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

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

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
    /// 按主表区分与明细生成或刷新各部门执行行。
    /// 执行内容一律由 UpsertDeptExecWithFillModeAsync 按 ResolveAutoExecContent 写入（管理区分-全仕向/部管/内部/技术）。
    /// 仅「是否自动填完」随区分变化：内部/技术全自动；全仕向/部管按采购类型、仓库、检验决定待填部门。
    /// </summary>
    /// <param name="gijutsu">设变技术课主</param>
    /// <param name="details">设变明细（通常已过滤作废）</param>
    /// <returns>本次涉及的部门编码（去重，供通知用）</returns>
    public async Task<IReadOnlyList<string>> ApplyAsync(
        TaktEcGijutsu gijutsu,
        IReadOnlyList<TaktEcDetail> details)
    {
        ArgumentNullException.ThrowIfNull(gijutsu);
        if (details == null || details.Count == 0)
        {
            return Array.Empty<string>();
        }
        var active = details.Where(x => x.IsObsolete == 0).ToList();
        if (active.Count == 0)
        {
            return Array.Empty<string>();
        }
        var touched = new HashSet<string>(StringComparer.Ordinal);
        foreach (var detail in active)
        {
            foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
            {
                await _ecExecPersistence.UpsertDeptExecWithFillModeAsync(
                    detail,
                    deptCode,
                    autoComplete: ShouldAutoCompleteExec(gijutsu.EcDistinction, deptCode, detail),
                    gijutsu.EcDistinction);
                touched.Add(deptCode);
            }
        }
        return touched.ToList();
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
