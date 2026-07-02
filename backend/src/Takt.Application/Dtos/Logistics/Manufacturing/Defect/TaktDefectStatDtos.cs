// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：不良/检查/改修统计 DTO（数据看板 defect-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

/// <summary>
/// 不良统计查询 DTO（按生产日期区间）
/// </summary>
public class TaktDefectStatQueryDto
{
    /// <summary>
    /// 生产日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }
}

/// <summary>
/// 不良统计 DTO（组立/PCBA 检查/改修共用结构）
/// </summary>
public class TaktDefectStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月统计分母合计（组立：生实实绩；PCBA 检查：检查数量；PCBA 改修：生产实绩）
    /// </summary>
    public decimal MonthBaseQty { get; set; }

    /// <summary>
    /// 月良品/无不良数量合计
    /// </summary>
    public decimal MonthGoodQty { get; set; }

    /// <summary>
    /// 月不良数量合计
    /// </summary>
    public decimal MonthDefectQty { get; set; }

    /// <summary>
    /// 月不良率（%）
    /// </summary>
    public decimal MonthDefectRatePercent { get; set; }

    /// <summary>
    /// 月直行率（%）
    /// </summary>
    public decimal MonthYieldRatePercent { get; set; }
}

/// <summary>
/// 组立不良统计 DTO
/// </summary>
public class TaktAssyDefectStatDto : TaktDefectStatDto
{
}

/// <summary>
/// PCBA 检查统计 DTO
/// </summary>
public class TaktPcbaInspectionStatDto : TaktDefectStatDto
{
}

/// <summary>
/// PCBA 改修统计 DTO
/// </summary>
public class TaktPcbaRepairStatDto : TaktDefectStatDto
{
}
