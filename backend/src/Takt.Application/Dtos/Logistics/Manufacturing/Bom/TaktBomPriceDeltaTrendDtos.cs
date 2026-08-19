// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomPriceDeltaTrendDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移 DTO（产品月成本 + 0价格组 + 价格差异组 PriceDeltaTrend）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移查询
/// </summary>
public class TaktBomPriceDeltaTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（必选语义；空=默认 FERT）。与工厂/期间/机种/产品共同限定主表产品，再算 0价格组与价格差异组
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（可选）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码（可选）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 核算日起
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日止
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 关注月 yyyy-MM（可选；默认期间止；用于 0价格组与价格差异组对比）
    /// </summary>
    public string? FocusPeriod { get; set; }
}

/// <summary>
/// 成本差异推移行
/// </summary>
public class TaktBomPriceDeltaTrendDto
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
    /// 各月产品成本（键 yyyy-MM；仅查询期间内）
    /// </summary>
    public Dictionary<string, decimal> PeriodCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 差异：价格差异组 Summary Var + 组件差异 Summary Var。
    /// 比较月/基准月任一产品成本为 0，或环比差异为 0 → 差异=0，且 0价格组/价格差异组/组件差异组全空。
    /// </summary>
    public decimal? PriceDelta { get; set; }

    /// <summary>
    /// 0价格组：关注月移动平均价=0；格式 (物料:用量:可替代:替代价, …)；无可替代则 (物料:用量)；可替代按末字母 Z→A 逆推 material_moving_price，替代价=MovingPrice÷PriceUnit（不查 cost_item）
    /// </summary>
    public string ZeroPriceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 价格差异组：同编码行成本变动；格式 (组件:用量:基期价→关注价,Diff:单价差, …),Summary Var:行成本差合计
    /// </summary>
    public string PriceDeltaTrend { get; set; } = string.Empty;

    /// <summary>
    /// 组件差异：new/remove/version；格式 (…),Summary Var:结构变动行成本合计；与价格差异组 Summary 之和=差异列
    /// </summary>
    public string ComponentDeltaGroup { get; set; } = string.Empty;

    /// <summary>
    /// 环比基准月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比关注月
    /// </summary>
    public string? ComparePeriod { get; set; }
}

/// <summary>
/// 成本差异推移结果
/// </summary>
public class TaktBomPriceDeltaTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktBomPriceDeltaTrendDto> Paged { get; set; } =
        TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
            new List<TaktBomPriceDeltaTrendDto>(), 0, 1, 20);

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 环比基准月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比关注月
    /// </summary>
    public string? ComparePeriod { get; set; }
}
