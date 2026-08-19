// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析 DTO（转置 / 差异 / 月度涨跌；三页共用工厂/机种/物料级联选项查询）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// 三页共用级联选项查询（工厂 → 物料类型 → 机种 → 物料/产品；本表去重）
// 产品成本推移 / 机种成本推移查询栏统一引用本栈选项，不另建 Cascade 服务
// ❌ 分析视图物料类型选项勿用字典 logistics_material_type（CRUD 表单专用）
// ❌ 分析视图机种选项勿用 TaktBomMaterialCosts/model-options（CRUD 主数据 TaktModelDestination）
// ========================================

/// <summary>
/// 物料类型选项查询（分析视图；本表 MaterialType 去重；须工厂）
/// <para>❌ 非字典 logistics_material_type（CRUD 用）；仅返回头表真实存在的类型码。</para>
/// </summary>
public class TaktBomMaterialCostAnalysisMaterialTypeOptionsQueryDto
{
    /// <summary>
    /// 工厂代码（必填；空则返回空列表）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 机种选项查询（分析视图；本表 ModelCode 去重；须工厂；MaterialType 可空）
/// <para>❌ 非 CRUD 主数据 TaktModelDestination / TaktBomMaterialCosts/model-options。</para>
/// </summary>
public class TaktBomMaterialCostAnalysisModelOptionsQueryDto
{
    /// <summary>
    /// 工厂代码（必填；空则返回空列表）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（本表 MaterialType；空=不过滤，返回该工厂全部机种）
    /// </summary>
    public string? MaterialType { get; set; }
}

/// <summary>
/// 物料/产品选项查询（级联；须工厂；机种/物料类型可空）
/// <para>仅查本表 TaktBomMaterialCost.ProductCode 去重；物料类型筛选用本表值，勿与 CRUD 字典选项混淆。</para>
/// </summary>
public class TaktBomMaterialCostAnalysisProductOptionsQueryDto
{
    /// <summary>
    /// 工厂代码（必填；空则返回空列表）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（可空；空则工厂下全部产品）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 物料类型筛选（本表 MaterialType；空=不过滤）
    /// </summary>
    public string? MaterialType { get; set; }
}

// ========================================
// BOM 物料成本明细转置/差异/涨跌分析 DTO
// ========================================

/// <summary>
/// 成本转置查询（行=产品，列=月份期间）
/// </summary>
public class TaktBomMaterialCostAnalysisTransposedQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 机种编码
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 物料类型（本表 MaterialType；空=不过滤，统计全部类型产品头）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 核算日期起
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 关注期间（yyyy-MM，可选）；设置后按该月相对上月计算各行环比涨跌
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// 全量列表排序（分页前）：productCode（默认）/ trend / varianceDesc
    /// </summary>
    public string? SortBy { get; set; }
}

/// <summary>
/// 成本转置行（产品各月总成本）
/// </summary>
public class TaktBomMaterialCostAnalysisTransposedDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（取自主表 TaktBomMaterialCost）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 各期间总成本（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> PeriodCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 币种（取该产品最新核算行）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 环比涨跌：none / up / down / flat（FocusPeriod 设置且存在上月成本时有效）
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间（yyyy-MM）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（yyyy-MM，通常为 FocusPeriod）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比月 - 基准月）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 机种材料成本汇总（转置页未选单物料时展示）
/// </summary>
public class TaktBomMaterialCostAnalysisModelSummaryDto
{
    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 机种下成品数量
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 各月平均材料成本（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> AveragePeriodCosts { get; set; } = new(StringComparer.Ordinal);
}

/// <summary>
/// 成本转置分页结果（含动态列顺序）
/// </summary>
public class TaktBomMaterialCostAnalysisTransposedResultDto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostAnalysisTransposedDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序（yyyy-MM）
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 机种汇总（未选产品时：全量成品平均成本）
    /// </summary>
    public TaktBomMaterialCostAnalysisModelSummaryDto? ModelSummary { get; set; }

    /// <summary>
    /// 全量行各期间成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}

/// <summary>
/// 成本差异分析查询（两期间组件级对比）
/// </summary>
public class TaktBomMaterialCostAnalysisVarianceQueryDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 基准期间（yyyy-MM）
    /// </summary>
    [Required]
    public string BasePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 对比期间（yyyy-MM）
    /// </summary>
    [Required]
    public string ComparePeriod { get; set; } = string.Empty;
}

/// <summary>
/// 成本差异分析行（组件级）
/// </summary>
public class TaktBomMaterialCostAnalysisVarianceLineDto
{
    /// <summary>
    /// BOM 项目号
    /// </summary>
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F/E）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 货币
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 基准行成本
    /// </summary>
    public decimal BaseCost { get; set; }

    /// <summary>
    /// 对比行成本
    /// </summary>
    public decimal CompareCost { get; set; }

    /// <summary>
    /// 成本差异（对比 - 基准）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 成本差异率（%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 基准单价
    /// </summary>
    public decimal BaseUnitPrice { get; set; }

    /// <summary>
    /// 对比单价
    /// </summary>
    public decimal CompareUnitPrice { get; set; }

    /// <summary>
    /// 单价差异
    /// </summary>
    public decimal UnitPriceVariance { get; set; }

    /// <summary>
    /// 基准数量
    /// </summary>
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// 对比数量
    /// </summary>
    public decimal CompareQuantity { get; set; }

    /// <summary>
    /// 数量差异
    /// </summary>
    public decimal QuantityVariance { get; set; }

    /// <summary>
    /// 价格因素影响额
    /// </summary>
    public decimal PriceEffectAmount { get; set; }

    /// <summary>
    /// 数量因素影响额
    /// </summary>
    public decimal QuantityEffectAmount { get; set; }

    /// <summary>
    /// 变动类型：new / removed / price / quantity / mixed / unchanged
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
}

/// <summary>
/// 成本差异分析结果
/// </summary>
public class TaktBomMaterialCostAnalysisVarianceResultDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 基准期间
    /// </summary>
    public string BasePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 对比期间
    /// </summary>
    public string ComparePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基准总成本
    /// </summary>
    public decimal BaseTotalCost { get; set; }

    /// <summary>
    /// 对比总成本
    /// </summary>
    public decimal CompareTotalCost { get; set; }

    /// <summary>
    /// 总成本差异
    /// </summary>
    public decimal TotalVariance { get; set; }

    /// <summary>
    /// 组件差异明细
    /// </summary>
    public List<TaktBomMaterialCostAnalysisVarianceLineDto> Lines { get; set; } = new();
}

/// <summary>
/// 成本月度涨跌分析查询
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendQueryDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    [Required]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选定单物料时必填；为空表示机种下全部物料）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 起始年月 yyyy-MM
    /// </summary>
    public string? PeriodStart { get; set; }

    /// <summary>
    /// 结束年月 yyyy-MM
    /// </summary>
    public string? PeriodEnd { get; set; }
}

/// <summary>
/// 成本月度涨跌分析行
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendLineDto
{
    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// 材料总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 对比基准月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 基准月成本
    /// </summary>
    public decimal? BaseTotalCost { get; set; }

    /// <summary>
    /// 环比差额
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = string.Empty;
}

/// <summary>
/// 成本月度涨跌分析结果
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendResultDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 是否为机种下全部物料汇总
    /// </summary>
    public bool AllMaterialsUnderModel { get; set; }

    /// <summary>
    /// 月度涨跌行
    /// </summary>
    public List<TaktBomMaterialCostAnalysisMonthlyTrendLineDto> Lines { get; set; } = new();
}

