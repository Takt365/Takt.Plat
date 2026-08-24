// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialModelTrendDtos.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Materials;

/// <summary>
/// 物料机种推移转置分析查询
/// </summary>
public class TaktMaterialModelTrendQueryDto : TaktPagedQuery
{
    public string PlantCode { get; set; } = string.Empty;
    public DateTime? PeriodDateStart { get; set; }
    public DateTime? PeriodDateEnd { get; set; }
    public string? FocusPeriod { get; set; }
    public string? Valuation { get; set; }
    /// <summary>
    /// 产品物料类型（机种推移必填；用于 BOM 产品组过滤，如 FERT）
    /// </summary>
    public string? MaterialType { get; set; }
    public string? MaterialCode { get; set; }
    /// <summary>
    /// 涨跌筛选：空/leading=领涨领跌各 50；all=全部；up/down/changed
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料机种推移行（物料×机种组×产品组 + 各月单价）
/// </summary>
public class TaktMaterialModelTrendDto : TaktMaterialMovingTrendDto
{
    public string ModelGroup { get; set; } = string.Empty;
    public string ProductGroup { get; set; } = string.Empty;
    public List<string> ModelCodes { get; set; } = new();
    public List<string> ProductCodes { get; set; } = new();
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 物料机种推移分析结果
/// </summary>
public class TaktMaterialModelTrendResultDto
{
    public TaktPagedResult<TaktMaterialModelTrendDto> Paged { get; set; } = null!;
    public List<string> PeriodOrder { get; set; } = new();
    public int MaterialCount { get; set; }
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public int UpCount { get; set; }
    public int DownCount { get; set; }
    public int FlatCount { get; set; }
    public int NoneCount { get; set; }
}
