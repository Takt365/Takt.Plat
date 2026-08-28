// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBukanQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：部管课主表/子表可见明细：采购类型 F 且仓库非 C003，设变+机种+新物料去重（保留最大 Id）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 部管课列表可见设变明细条件
/// </summary>
internal static class TaktEcBukanQueryHelper
{
    /// <summary>
    /// 新采购类型为 F、新品仓库非 C003，且同设变单号+机种+新物料仅保留最大 Id 一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewPurchaseType == purchaseTypeF);
        exp = exp.And(x => x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003);
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcModelCode == x.EcModelCode
                    && s.EcNewMaterialCode == x.EcNewMaterialCode
                    && s.EcNewPurchaseType == purchaseTypeF
                    && (s.EcNewWarehouse == null || s.EcNewWarehouse != warehouseC003)
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 部管执行行对应可见明细（单层表达式，供 TaktEcBukan 列表使用）
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcBukan, bool>> VisibleExecExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        return x => SqlFunc.Subqueryable<TaktEcDetail>()
            .Where(d =>
                d.Id == x.EcnDetailId
                && d.IsDeleted == 0
                && d.IsObsolete == 0
                && d.EcNewPurchaseType == purchaseTypeF
                && (d.EcNewWarehouse == null || d.EcNewWarehouse != warehouseC003)
                && !SqlFunc.Subqueryable<TaktEcDetail>()
                    .Where(s =>
                        s.TenantCode == d.TenantCode
                        && s.CompanyCode == d.CompanyCode
                        && s.IsDeleted == 0
                        && s.IsObsolete == 0
                        && s.EcCode == d.EcCode
                        && s.EcModelCode == d.EcModelCode
                        && s.EcNewMaterialCode == d.EcNewMaterialCode
                        && s.EcNewPurchaseType == purchaseTypeF
                        && (s.EcNewWarehouse == null || s.EcNewWarehouse != warehouseC003)
                        && s.Id > d.Id)
                    .Any())
            .Any();
    }
}
