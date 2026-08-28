// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizounikaQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：制二课主表/子表页签：F+C003 与其它；各组按设变+上阶物料去重（保留最大 Id）
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
/// 制二课列表页签可见设变明细条件
/// </summary>
internal static class TaktEcSeizounikaQueryHelper
{
    /// <summary>
    /// 按页签返回明细过滤；空或未知时默认 C003 页签。
    /// </summary>
    /// <param name="pcbaTab">制二页签（1=F+C003 2=其它）</param>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> TabDetailExpression(int? pcbaTab)
    {
        if (pcbaTab == TaktEcSeizounikaConstants.ListTabOther)
        {
            return OtherGroupDetailExpression();
        }
        return C003GroupDetailExpression();
    }

    /// <summary>
    /// 新采购类型 F、新品仓库 C003，且同设变单号+上阶物料仅保留最大 Id 一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> C003GroupDetailExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewPurchaseType == purchaseTypeF);
        exp = exp.And(x => x.EcNewWarehouse != null && x.EcNewWarehouse == warehouseC003);
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcParentMaterialCode == x.EcParentMaterialCode
                    && s.EcNewPurchaseType == purchaseTypeF
                    && s.EcNewWarehouse != null
                    && s.EcNewWarehouse == warehouseC003
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 非（采购 F 且仓库 C003），且同设变单号+上阶物料仅保留最大 Id 一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> OtherGroupDetailExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x =>
            x.EcNewPurchaseType != purchaseTypeF
            || x.EcNewWarehouse == null
            || x.EcNewWarehouse != warehouseC003);
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcParentMaterialCode == x.EcParentMaterialCode
                    && (s.EcNewPurchaseType != purchaseTypeF
                        || s.EcNewWarehouse == null
                        || s.EcNewWarehouse != warehouseC003)
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 制二执行行对应 C003 页签可见明细（单层表达式）
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcSeizounika, bool>> VisibleC003ExecExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        return x => SqlFunc.Subqueryable<TaktEcDetail>()
            .Where(d =>
                d.Id == x.EcnDetailId
                && d.EcNewPurchaseType == purchaseTypeF
                && d.EcNewWarehouse != null
                && d.EcNewWarehouse == warehouseC003
                && !SqlFunc.Subqueryable<TaktEcDetail>()
                    .Where(s =>
                        s.TenantCode == d.TenantCode
                        && s.CompanyCode == d.CompanyCode
                        && s.IsDeleted == 0
                        && s.IsObsolete == 0
                        && s.EcCode == d.EcCode
                        && s.EcParentMaterialCode == d.EcParentMaterialCode
                        && s.EcNewPurchaseType == purchaseTypeF
                        && s.EcNewWarehouse != null
                        && s.EcNewWarehouse == warehouseC003
                        && s.Id > d.Id)
                    .Any())
            .Any();
    }

    /// <summary>
    /// 制二执行行对应其它页签可见明细（单层表达式）
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcSeizounika, bool>> VisibleOtherExecExpression()
    {
        var purchaseTypeF = TaktEcDistinctionConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcDistinctionConstants.NewWarehousePcbaGate;
        return x => SqlFunc.Subqueryable<TaktEcDetail>()
            .Where(d =>
                d.Id == x.EcnDetailId
                && (d.EcNewPurchaseType != purchaseTypeF
                    || d.EcNewWarehouse == null
                    || d.EcNewWarehouse != warehouseC003)
                && !SqlFunc.Subqueryable<TaktEcDetail>()
                    .Where(s =>
                        s.TenantCode == d.TenantCode
                        && s.CompanyCode == d.CompanyCode
                        && s.IsDeleted == 0
                        && s.IsObsolete == 0
                        && s.EcCode == d.EcCode
                        && s.EcParentMaterialCode == d.EcParentMaterialCode
                        && (s.EcNewPurchaseType != purchaseTypeF
                            || s.EcNewWarehouse == null
                            || s.EcNewWarehouse != warehouseC003)
                        && s.Id > d.Id)
                    .Any())
            .Any();
    }
}
