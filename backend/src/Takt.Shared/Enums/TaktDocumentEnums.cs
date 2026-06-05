// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktDocumentEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：单据相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 单据类型枚举
/// </summary>
public enum TaktDocumentType
{
    /// <summary>
    /// 销售订单
    /// </summary>
    [Display(Name = "销售订单")]
    SalesOrder = 0,

    /// <summary>
    /// 采购订单
    /// </summary>
    [Display(Name = "采购订单")]
    PurchaseOrder = 1,

    /// <summary>
    /// 合同
    /// </summary>
    [Display(Name = "合同")]
    Contract = 2,

    /// <summary>
    /// 发票
    /// </summary>
    [Display(Name = "发票")]
    Invoice = 3,

    /// <summary>
    /// 收款单
    /// </summary>
    [Display(Name = "收款单")]
    Receipt = 4,

    /// <summary>
    /// 付款单
    /// </summary>
    [Display(Name = "付款单")]
    Payment = 5,

    /// <summary>
    /// 出库单
    /// </summary>
    [Display(Name = "出库单")]
    Delivery = 6,

    /// <summary>
    /// 入库单
    /// </summary>
    [Display(Name = "入库单")]
    GoodsIn = 7,

    /// <summary>
    /// 退货单
    /// </summary>
    [Display(Name = "退货单")]
    Return = 8,

    /// <summary>
    /// 报价单
    /// </summary>
    [Display(Name = "报价单")]
    Quotation = 9
}
