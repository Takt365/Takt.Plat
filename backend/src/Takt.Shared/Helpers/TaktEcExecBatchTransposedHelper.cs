// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEcExecBatchTransposedHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变批次转置阶段单元格构建（日期 + 批次）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 设变批次转置阶段单元格
/// </summary>
public sealed class TaktEcDeptBatchTransposedStageCell
{
    /// <summary>阶段编码</summary>
    public string StageCode { get; init; } = string.Empty;
    /// <summary>阶段日期</summary>
    public DateTime? StageDate { get; init; }
    /// <summary>批次号/批次说明</summary>
    public string? BatchNo { get; init; }
    /// <summary>日期展示文本（yyyyMMdd）</summary>
    public string? DateDisplayText { get; init; }
}

/// <summary>
/// 设变批次转置阶段单元格构建
/// </summary>
public static class TaktEcExecBatchTransposedHelper
{
    /// <summary>
    /// 构建批次阶段单元格
    /// </summary>
    /// <param name="stageCode">阶段编码</param>
    /// <param name="stageDate">阶段日期</param>
    /// <param name="batchNo">批次号</param>
    /// <returns>阶段单元格</returns>
    public static TaktEcDeptBatchTransposedStageCell BuildStageCell(
        string stageCode,
        DateTime? stageDate,
        string? batchNo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stageCode);
        var date = stageDate?.Date;
        return new TaktEcDeptBatchTransposedStageCell
        {
            StageCode = stageCode,
            StageDate = date,
            BatchNo = string.IsNullOrWhiteSpace(batchNo) ? null : batchNo.Trim(),
            DateDisplayText = date?.ToString("yyyyMMdd"),
        };
    }
}
