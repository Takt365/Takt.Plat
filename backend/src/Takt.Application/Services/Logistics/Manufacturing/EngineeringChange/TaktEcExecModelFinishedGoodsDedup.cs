// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecModelFinishedGoodsDedup.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：生管/制一/品管/制技共用：设变单号+机种+完成品去重（保留最大 Id）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 同设变单号+机种编码+完成品仅保留最大 Id 一行（生管/制一/品管/制技同一套去重）
/// </summary>
internal static class TaktEcExecModelFinishedGoodsDedup
{
    /// <summary>
    /// 明细可见条件
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcModelCode == x.EcModelCode
                    && s.EcFinishedGoods == x.EcFinishedGoods
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 部门执行行对应可见明细（单层表达式）
    /// </summary>
    /// <typeparam name="TExec">部门执行实体</typeparam>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TExec, bool>> VisibleExecExpression<TExec>()
        where TExec : class, ITaktEcDeptExecEntity
    {
        return x => SqlFunc.Subqueryable<TaktEcDetail>()
            .Where(d =>
                d.Id == x.EcnDetailId
                && !SqlFunc.Subqueryable<TaktEcDetail>()
                    .Where(s =>
                        s.TenantCode == d.TenantCode
                        && s.CompanyCode == d.CompanyCode
                        && s.IsDeleted == 0
                        && s.IsObsolete == 0
                        && s.EcCode == d.EcCode
                        && s.EcModelCode == d.EcModelCode
                        && s.EcFinishedGoods == d.EcFinishedGoods
                        && s.Id > d.Id)
                    .Any())
            .Any();
    }
}
