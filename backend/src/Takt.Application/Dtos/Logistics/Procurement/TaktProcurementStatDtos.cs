// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktProcurementStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：采购统计 DTO（数据看板 order-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// 采购统计查询 DTO（按订单日期区间）
/// </summary>
public class TaktProcurementStatQueryDto
{
    /// <summary>
    /// 订单日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 同比基期订单日期（范围-开始；可选）
    /// </summary>
    public DateTime? CompareOrderDateStart { get; set; }

    /// <summary>
    /// 同比基期订单日期（范围-结束；可选）
    /// </summary>
    public DateTime? CompareOrderDateEnd { get; set; }
}

/// <summary>
/// 采购订单统计 DTO
/// </summary>
public class TaktPurchaseOrderStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月订单数量
    /// </summary>
    public int MonthOrderCount { get; set; }

    /// <summary>
    /// 月订单金额合计（分；前端展示时 ÷100 为元）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }

    /// <summary>
    /// 同比基期订单数量（未传 compare 区间时为 0）
    /// </summary>
    public int CompareOrderCount { get; set; }

    /// <summary>
    /// 订单数同比增长率（%）
    /// </summary>
    public decimal OrderCountYoYPercent { get; set; }
}
