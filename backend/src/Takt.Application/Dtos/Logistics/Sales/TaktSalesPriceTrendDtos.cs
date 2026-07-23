// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceTrendDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格月度波动分析 DTO（price-trend-analysis）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Sales;

/// <summary>
/// 销售价格月度波动分析查询 DTO
/// </summary>
public class TaktSalesPriceTrendQueryDto
{
    /// <summary>
    /// 物料编码（必填；成品/物料均可）
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
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_sales_price_type，如 PR00；可选）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格（PriceStatus=1；默认 true）
    /// </summary>
    public bool? OnlyEnabled { get; set; }
}

/// <summary>
/// 销售价格月度波动点 DTO
/// </summary>
public class TaktSalesPriceTrendPointDto
{
    /// <summary>
    /// 月份 yyyy-MM
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 当月是否存在有效销售价
    /// </summary>
    public bool HasPrice { get; set; }

    /// <summary>
    /// 折算单价（每 1 销售单位）
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
    /// 销售单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码
    /// </summary>
    public string? CustomerCode { get; set; }

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
/// 销售价格月度波动分析结果 DTO
/// </summary>
public class TaktSalesPriceTrendResultDto
{
    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

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
    public List<TaktSalesPriceTrendPointDto> Points { get; set; } = new();
}

// ========================================
// 销售价格月推移转置分析 DTO
// ========================================

/// <summary>
/// 工厂 × 物料 × 客户销售单价转置分析查询
/// </summary>
public class TaktSalesPriceMonthlyTrendQueryDto : TaktPagedQuery
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
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_price_type，如 PR00；可选）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格主表（兼容字段；当前实体无 PriceStatus，服务侧暂不按状态过滤）
    /// </summary>
    public bool? OnlyEnabled { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 销售价格月推移转置行（行=工厂+物料+客户，列=各月折算单价）
/// </summary>
public class TaktSalesPriceMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（空串表示通用价）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称（冗余，回填：随客户）
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 各期间折算单价（键 yyyy-MM；仅含当月有效价月份）
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间单价来源月（有效期填充场景可留空）
    /// </summary>
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比单价 - 基准单价）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位；如 0.2978 表示 29.78%）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 销售价格月推移转置分析结果
/// </summary>
public class TaktSalesPriceMonthlyTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktSalesPriceMonthlyTrendDto> Paged { get; set; } = null!;

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

/// <summary>
/// 销售机种价格推移行（工厂×物料×客户 + BOM 机种/产品组 + 各月单价）
/// </summary>
public class TaktSalesPriceModelTrendDto : TaktSalesPriceMonthlyTrendDto
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
    /// 物料描述（优先销售明细物料名称，否则 BOM 组件描述）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 销售机种价格推移分析结果
/// </summary>
public class TaktSalesPriceModelTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktSalesPriceModelTrendDto> Paged { get; set; } = null!;

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
