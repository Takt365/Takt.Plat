// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcDeptCodes.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门编码常量，与 TaktEcDept.DeptCode 及各部门视图服务对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变部门编码（TaktEcDept.DeptCode）。顺序：Eng、Pmc、Mp、Iqc、Mc、Pcba、Assy、Qa、Te。
/// </summary>
public static class TaktEcDeptCodes
{
    /// <summary>技术部门</summary>
    public const string Eng = "Eng";
    /// <summary>生管部门</summary>
    public const string Pmc = "Pmc";
    /// <summary>采购部门</summary>
    public const string Mp = "Mp";
    /// <summary>受检部门</summary>
    public const string Iqc = "Iqc";
    /// <summary>部管部门</summary>
    public const string Mc = "Mc";
    /// <summary>制造二课（PCBA）</summary>
    public const string Pcba = "Pcba";
    /// <summary>制造一课（Assembly）</summary>
    public const string Assy = "Assy";
    /// <summary>品管部门</summary>
    public const string Qa = "Qa";
    /// <summary>制技部门</summary>
    public const string Te = "Te";
    /// <summary>看板列顺序（不含 Te 时按业务看板展示）</summary>
    public static readonly string[] KanbanOrder =
    [
        Eng, Pmc, Mp, Iqc, Mc, Pcba, Assy, Qa, Te
    ];
}
