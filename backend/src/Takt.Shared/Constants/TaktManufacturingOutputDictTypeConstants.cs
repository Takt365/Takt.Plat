// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktManufacturingOutputDictTypeConstants.cs
// 功能描述：制造日报（组立/PCBA 等）明细共用字典类型编码
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 制造日报明细共用字典类型编码（停线原因、未达成原因等）
/// </summary>
public static class TaktManufacturingOutputDictTypeConstants
{
    /// <summary>停线原因（字典 logistics_stop_reason_category）</summary>
    public const string StopReasonCategory = "logistics_stop_reason_category";

    /// <summary>未达成原因（字典 logistics_nonachievement_reason_category）</summary>
    public const string NonachievementReasonCategory = "logistics_nonachievement_reason_category";
}
