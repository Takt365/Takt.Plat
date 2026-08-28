// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcSeizounikaConstants.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造二课列表页签常量（采购 F+仓库 C003 / 其它）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变制造二课（TaktEcSeizounika）列表页签
/// </summary>
public static class TaktEcSeizounikaConstants
{
    /// <summary>
    /// 页签：采购类型 F 且新品仓库 C003（按设变+上阶物料去重）
    /// </summary>
    public const int ListTabC003 = 1;

    /// <summary>
    /// 页签：其它明细（非 F+C003；按设变+上阶物料去重）
    /// </summary>
    public const int ListTabOther = 2;
}
