// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomModelCostTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移 DTO（与产品推移 / 成本分析分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 机种成本推移查询
/// </summary>
public class TaktBomModelCostTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并；空=全部机种）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号分隔；空=期间最后月全部机种）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 产品编码（可选；有值时仅汇总该产品下明细）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 组件/物料编码（兼容单值；与 ComponentCodes 合并；空=期间最后月全部物料）
    /// </summary>
    public string? ComponentCode { get; set; }

    /// <summary>
    /// 组件/物料编码多选（逗号分隔；空=期间最后月全部 X+F 物料）
    /// </summary>
    public string? ComponentCodes { get; set; }

    /// <summary>
    /// 物料类型（本表 MaterialType；空=不过滤）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 核算期间起（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；环比对比月
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// 全量列表排序（分页前）：productCountDesc（默认）/ productCountAsc / trend
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// 合并模式：summary=材料成本推移；detail=差异组件推移
    /// </summary>
    public string? MergeMode { get; set; }
}

/// <summary>
/// 机种成本推移分析行（summary/detail 合并键跨产品组合并；列为核算月组件单价）
/// </summary>
public class TaktBomModelCostTrendDto
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
    /// 机种名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量（detail 模式展示；summary 可为空）
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（detail 模式）
    /// </summary>
    public string? BatchIndicator { get; set; }

    /// <summary>
    /// 生产相关
    /// </summary>
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（detail 模式）
    /// </summary>
    public string? SpecialProcurementType { get; set; }

    /// <summary>
    /// 利润中心（detail 模式）
    /// </summary>
    public string? ProfitCenterCode { get; set; }

    /// <summary>
    /// 产品组（最后月命中产品及该组件用量；格式「产品编码:数量」英文逗号分隔，如 8Y00000154:1,09VRS7TS04:1）
    /// </summary>
    public string ProductCodes { get; set; } = string.Empty;

    /// <summary>
    /// 产品组内产品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 各核算月组件单价（键 yyyy-MM；MovingAveragePrice÷MovingPriceUnit，不乘组件数量、不按产品数加总；缺月无键）
    /// </summary>
    public Dictionary<string, decimal> PeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各月存在/变动码（detail）：present / absent / new / removed / up / down / flat
    /// </summary>
    public Dictionary<string, string> PeriodChangeTypes { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 兼容旧字段：同 PeriodMaterialCosts
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices
    {
        get => PeriodMaterialCosts;
        set => PeriodMaterialCosts = value ?? new Dictionary<string, decimal>(StringComparer.Ordinal);
    }

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
    /// 环比差额
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 机种成本推移分析结果
/// </summary>
public class TaktBomModelCostTrendResultDto
{
    /// <summary>
    /// 分页分析行（组件合并键）
    /// </summary>
    public TaktPagedResult<TaktBomModelCostTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 机种下产品编码列表（产品组）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 机种各月材料成本（产品月成本算术平均，与主表机种月平均口径一致）
    /// </summary>
    public Dictionary<string, decimal> ModelPeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 机种月材料成本环比涨跌
    /// </summary>
    public string ModelTrend { get; set; } = "none";

    /// <summary>
    /// 机种环比基准月
    /// </summary>
    public string? ModelBasePeriod { get; set; }

    /// <summary>
    /// 机种环比对比月
    /// </summary>
    public string? ModelComparePeriod { get; set; }

    /// <summary>
    /// 机种环比差额
    /// </summary>
    public decimal? ModelVarianceAmount { get; set; }

    /// <summary>
    /// 机种环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? ModelVariancePercent { get; set; }

    /// <summary>
    /// 合并分析行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 环比基准期间（分析行）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价分析行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价分析行数
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

    /// <summary>
    /// 全量分析行各期间材料成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量分析行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}
