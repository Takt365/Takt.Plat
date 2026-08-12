// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移动价格推移 / 机种推移分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// 物料月移动价格推移分析 DTO
// ========================================

/// <summary>
/// 物料 × 月份移动单价转置分析查询
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 移动价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 评估类别（可选；为空时按物料+估值分行）
    /// </summary>
    public string? Valuation { get; set; }

    /// <summary>
    /// 产品物料类型（机种推移必填；用于 BOM 产品组过滤，如 FERT）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 涨跌筛选：空=物料价格推移全部 / 机种推移默认领涨领跌各 50；leading=领涨领跌各 50；all=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料月移动价格转置行（行=物料+估值，列=各月单价 MovingPrice÷PriceUnit）
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendDto
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
    /// 评估类别
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 各期间单价（键 yyyy-MM；无当月数据时沿用最近有价期间）
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间单价来源月（键=展示 yyyy-MM，值=实际取价 yyyy-MM；值≠键表示回填）
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
    /// 环比变动率（小数比率，保留 4 位；如 0.2978 表示 29.78%，便于 Excel 百分比格式）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 物料月移动价格转置分析结果
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendResultDto
{
    /// <summary>
    /// 分页物料行
    /// </summary>
    public TaktPagedResult<TaktMaterialMovingPriceMonthlyTrendDto> Paged { get; set; } = null!;

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
/// 物料-机种-价格推移行（物料×机种组×产品组 + 各月单价）
/// </summary>
public class TaktMaterialMovingPriceModelTrendDto : TaktMaterialMovingPriceMonthlyTrendDto
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
    /// 物料描述（优先工厂物料名称，否则 BOM 组件描述）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 物料-机种-价格推移分析结果
/// </summary>
public class TaktMaterialMovingPriceModelTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktMaterialMovingPriceModelTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 物料行总数（分页前）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数
    /// </summary>
    public int NoneCount { get; set; }
}
