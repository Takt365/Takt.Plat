// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomVarianceCostTrendDtos.cs
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移 DTO（机种可多选；有无差异组件×移动单价月度推移）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 差异成本推移查询（工厂、期间、机种必填；机种可多选）
/// </summary>
public class TaktBomVarianceCostTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（兼容单值；与 ModelCodes 合并；至少选一个）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔；与 ModelCode 合并）
    /// </summary>
    public string? ModelCodes { get; set; }

    /// <summary>
    /// 物料类型（本表 MaterialType；空=默认 FERT）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 产品编码（兼容单值；与 ProductCodes 合并；空=机种下全部产品）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 产品编码多选（逗号/分号分隔；与 ProductCode 合并；空=机种下全部产品）
    /// </summary>
    public string? ProductCodes { get; set; }

    /// <summary>
    /// 核算期间起（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选；默认期间止）
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部（仅有无差异）；new/removed；changed=新增+剔除
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// 全量列表排序（分页前）：trend（默认：新增/剔除/版本）/ varianceDesc / componentCode
    /// </summary>
    public string? SortBy { get; set; }
}

/// <summary>
/// 差异成本推移机种/产品选项查询（产品选项须带机种以联动）
/// </summary>
public class TaktBomVarianceCostTrendOptionsQueryDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间最后月 yyyy-MM（必填）
    /// </summary>
    public string FocusPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（空=不过滤）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（兼容单值；产品选项与 ModelCodes 合并过滤）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种编码多选（逗号/分号分隔；产品选项用；空则产品选项返回空）
    /// </summary>
    public string? ModelCodes { get; set; }
}

/// <summary>
/// 差异成本推移分析行（有无差异组件 × 月移动单价）
/// </summary>
public class TaktBomVarianceCostTrendDto
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
    /// 产品编码（对比槽位键之一）
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（对比槽位键之一）
    /// </summary>
    public string SequenceCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（对比槽位键之一）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（对比槽位键之一）
    /// </summary>
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（对比月/关注月；版本变更时为新版本）
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 基准月组件编码（版本变更时为旧版本；新增/剔除可空）
    /// </summary>
    public string? PreviousComponentCode { get; set; }

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关注月组件数量（展示用）
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 关注月移动单价（移动平均价÷移动价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 移动价格货币
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关
    /// </summary>
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（逗号分隔）
    /// </summary>
    public string ProductCodes { get; set; } = string.Empty;

    /// <summary>
    /// 产品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 各核算月移动单价（键 yyyy-MM；缺月无键=该月无此组件）
    /// </summary>
    public Dictionary<string, decimal> PeriodMovingPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各月变动码：present / absent / new / removed / up / down / flat
    /// </summary>
    public Dictionary<string, string> PeriodChangeTypes { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 差异类型：new / removed / version（两端月均有明细时才判定）
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
    /// 环比移动单价差额（对比月 − 基准月；新增=对比月价，剔除=−基准月价）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比移动单价变动率（百分点）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 差异成本推移分析结果
/// </summary>
public class TaktBomVarianceCostTrendResultDto
{
    /// <summary>
    /// 分页分析行
    /// </summary>
    public TaktPagedResult<TaktBomVarianceCostTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 机种下产品编码列表
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 分析行总数（分页前）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨（用量增加）行数
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌（用量减少）行数
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 新增组件行数
    /// </summary>
    public int NewCount { get; set; }

    /// <summary>
    /// 剔除组件行数
    /// </summary>
    public int RemovedCount { get; set; }

    /// <summary>
    /// 版本变更行数（同 BOM 项目+层级，组件末位版本字母变化）
    /// </summary>
    public int VersionCount { get; set; }

    /// <summary>
    /// 无趋势行数
    /// </summary>
    public int NoneCount { get; set; }
}
