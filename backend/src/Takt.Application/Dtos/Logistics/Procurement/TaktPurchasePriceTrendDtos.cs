// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月度波动分析 DTO（price-trend-analysis）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// 采购价格月度波动分析查询 DTO
/// </summary>
public class TaktPurchasePriceTrendQueryDto
{
    /// <summary>
    /// 物料编码（必填）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 分析开始日期（默认 2016-01-01）
    /// </summary>
    public DateTime? DateStart { get; set; }

    /// <summary>
    /// 分析结束日期（默认当前月末）
    /// </summary>
    public DateTime? DateEnd { get; set; }

    /// <summary>
    /// 工厂代码（可选）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 供应商编码（可选）
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_price_type；可选）
    /// </summary>
    public int? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格（PriceStatus=1；默认 true）
    /// </summary>
    public bool? OnlyEnabled { get; set; }
}

/// <summary>
/// 采购价格月度波动点 DTO
/// </summary>
public class TaktPurchasePriceTrendPointDto
{
    /// <summary>
    /// 月份 yyyy-MM
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 当月是否存在有效采购价
    /// </summary>
    public bool HasPrice { get; set; }

    /// <summary>
    /// 折算单价（每 1 采购单位）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 原始单价
    /// </summary>
    public decimal RawPrice { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public int PerUnit { get; set; }

    /// <summary>
    /// 采购单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 环比涨跌幅（%）
    /// </summary>
    public decimal? ChangePercent { get; set; }

    /// <summary>
    /// 当月有效价记录数
    /// </summary>
    public int SourceRecordCount { get; set; }
}

/// <summary>
/// 采购价格月度波动分析结果 DTO
/// </summary>
public class TaktPurchasePriceTrendResultDto
{
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（取自明细冗余字段）
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 分析开始日期
    /// </summary>
    public DateTime DateStart { get; set; }

    /// <summary>
    /// 分析结束日期
    /// </summary>
    public DateTime DateEnd { get; set; }

    /// <summary>
    /// 月度波动序列
    /// </summary>
    public List<TaktPurchasePriceTrendPointDto> Points { get; set; } = new();
}
