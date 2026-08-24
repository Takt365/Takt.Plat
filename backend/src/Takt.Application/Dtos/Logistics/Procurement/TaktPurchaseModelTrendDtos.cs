// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseModelTrendDtos.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购机种价格推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// 采购机种价格推移转置分析查询
/// </summary>
public class TaktPurchaseModelTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 供应商编码（可选）
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_price_type；可选；如 PB00）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格主表（主表已无 PriceStatus；保留兼容，忽略）
    /// </summary>
    public bool? OnlyEnabled { get; set; }

    /// <summary>
    /// 涨跌筛选：空=机种推移默认领涨领跌各 50；leading=领涨领跌各 50；all=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码；机种推移必填；字典 logistics_material_type）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;
}

/// <summary>
/// 采购机种价格推移行（工厂×物料×供应商 + BOM 机种/产品组 + 各月单价）
/// </summary>
public class TaktPurchaseModelTrendDto : TaktPurchasePriceTrendDto
{
    /// <summary>
    /// 机种组展示（逗号分隔 ModelCode；来源 BOM 成本汇总）
    /// </summary>
    public string ModelGroup { get; set; } = string.Empty;

    /// <summary>
    /// 产品组展示（逗号分隔 ProductCode；来源 BOM 成本明细组件行）
    /// </summary>
    public string ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码列表（去重排序）
    /// </summary>
    public List<string> ModelCodes { get; set; } = new();

    /// <summary>
    /// 产品编码列表（去重排序）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 物料描述（优先采购明细物料名称，否则 BOM 组件描述）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 采购机种价格推移分析结果
/// </summary>
public class TaktPurchaseModelTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktPurchaseModelTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 物料行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（关注月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }
}
