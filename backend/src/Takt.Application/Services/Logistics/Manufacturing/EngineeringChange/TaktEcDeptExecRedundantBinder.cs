// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptExecRedundantBinder.cs
// 创建时间：2026-09-02
// 创建人：Takt365(Cursor AI)
// 功能描述：从来源设变明细同步各部门执行行冗余字段（采购/受检/部管/制一/品管/制二字段集不同）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细 → 部门执行行冗余字段绑定（新增/更新/来源导入共用）
/// </summary>
internal static class TaktEcDeptExecRedundantBinder
{
    /// <summary>
    /// 按实体类型拷贝明细冗余字段；新建时写入行号。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号；更新时传 null 不改行号</param>
    public static void Apply(object exec, TaktEcDetail detail, int? lineNumberForCreate)
    {
        ArgumentNullException.ThrowIfNull(exec);
        ArgumentNullException.ThrowIfNull(detail);
        switch (exec)
        {
            case TaktEcKoubai koubai:
                ApplyIdentity(koubai, detail, lineNumberForCreate);
                ApplyEcDistinction(koubai, detail);
                koubai.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                koubai.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                koubai.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                koubai.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                break;
            case TaktEcUkeken ukeken:
                ApplyIdentity(ukeken, detail, lineNumberForCreate);
                ApplyEcDistinction(ukeken, detail);
                ukeken.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                ukeken.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                ukeken.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                ukeken.EcNewRequiresInspection = detail.EcNewRequiresInspection;
                break;
            case TaktEcBukan bukan:
                ApplyIdentity(bukan, detail, lineNumberForCreate);
                ApplyEcDistinction(bukan, detail);
                bukan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                bukan.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                bukan.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                bukan.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                bukan.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                bukan.EcModelCode = detail.EcModelCode ?? string.Empty;
                bukan.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                bukan.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcSmt smt:
                ApplyIdentity(smt, detail, lineNumberForCreate);
                ApplyEcDistinction(smt, detail);
                smt.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                smt.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                smt.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                smt.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                smt.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                smt.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                smt.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                smt.EcModelCode = detail.EcModelCode ?? string.Empty;
                smt.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                smt.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcSeizounika seizounika:
                ApplyIdentity(seizounika, detail, lineNumberForCreate);
                ApplyEcDistinction(seizounika, detail);
                seizounika.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                seizounika.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                seizounika.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizounika.EcModelCode = detail.EcModelCode ?? string.Empty;
                seizounika.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                seizounika.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcSeikan seikan:
                ApplyIdentity(seikan, detail, lineNumberForCreate);
                ApplyEcDistinction(seikan, detail);
                seikan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seikan.EcModelCode = detail.EcModelCode ?? string.Empty;
                seikan.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                seikan.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcSeizouikka seizouikka:
                ApplyIdentity(seizouikka, detail, lineNumberForCreate);
                ApplyEcDistinction(seizouikka, detail);
                seizouikka.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizouikka.EcModelCode = detail.EcModelCode ?? string.Empty;
                seizouikka.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                seizouikka.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcHinkan hinkan:
                ApplyIdentity(hinkan, detail, lineNumberForCreate);
                ApplyEcDistinction(hinkan, detail);
                hinkan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                hinkan.EcModelCode = detail.EcModelCode ?? string.Empty;
                hinkan.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                hinkan.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            case TaktEcSeizougijutsu seizougijutsu:
                ApplyIdentity(seizougijutsu, detail, lineNumberForCreate);
                ApplyEcDistinction(seizougijutsu, detail);
                seizougijutsu.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizougijutsu.EcModelCode = detail.EcModelCode ?? string.Empty;
                seizougijutsu.EcFinishedGoods = detail.EcFinishedGoods ?? string.Empty;
                seizougijutsu.EcFinishedGoodsDescription = detail.EcFinishedGoodsDescription ?? string.Empty;
                break;
            default:
                if (exec is ITaktEcDeptExecEntity identity)
                {
                    ApplyIdentity(identity, detail, lineNumberForCreate);
                }
                break;
        }
    }

    /// <summary>
    /// 同步设变单号与新建行号
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号</param>
    private static void ApplyIdentity(ITaktEcDeptExecEntity exec, TaktEcDetail detail, int? lineNumberForCreate)
    {
        exec.EcCode = detail.EcCode;
        exec.EcDetailId = detail.Id;
        if (lineNumberForCreate.HasValue)
        {
            exec.LineNumber = lineNumberForCreate.Value;
        }
    }

    /// <summary>
    /// 同步明细冗余区分（各执行部门 EcDistinction）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    private static void ApplyEcDistinction(object exec, TaktEcDetail detail)
    {
        switch (exec)
        {
            case TaktEcKoubai koubai:
                koubai.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcUkeken ukeken:
                ukeken.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcBukan bukan:
                bukan.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcSmt smt:
                smt.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcSeizounika seizounika:
                seizounika.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcSeikan seikan:
                seikan.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcSeizouikka seizouikka:
                seizouikka.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcHinkan hinkan:
                hinkan.EcDistinction = detail.EcDistinction;
                break;
            case TaktEcSeizougijutsu seizougijutsu:
                seizougijutsu.EcDistinction = detail.EcDistinction;
                break;
        }
    }
}
