// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomQuartzExecuteParamsHelper.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM Quartz 执行参数解析（costingPeriod / asOfDate；空则当月）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM Quartz 任务 ExecuteParams 解析（手动执行可传核算月份；Cron 默认当月）
/// </summary>
public static class TaktBomQuartzExecuteParamsHelper
{
    /// <summary>
    /// 从执行参数解析判定日（取核算月份内任意日即可；空/无效则用今日）
    /// 支持 JSON：{"costingPeriod":"yyyy-MM"}、{"asOfDate":"yyyy-MM-dd"}；或裸 yyyy-MM
    /// </summary>
    /// <param name="executeParams">任务执行参数</param>
    /// <param name="fallback">缺省判定日；默认今天</param>
    /// <returns>判定日（本地日历日）</returns>
    public static DateTime ResolveAsOfDate(string? executeParams, DateTime? fallback = null)
    {
        var today = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(fallback ?? DateTime.Today);
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return today;
        }
        var raw = executeParams.Trim();
        if (TryParsePeriodKey(raw, out var barePeriod))
        {
            return FirstDayOfPeriod(barePeriod);
        }
        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
            {
                return today;
            }
            if (doc.RootElement.TryGetProperty("costingPeriod", out var periodProp)
                || doc.RootElement.TryGetProperty("CostingPeriod", out periodProp))
            {
                var periodText = periodProp.ValueKind == JsonValueKind.String
                    ? periodProp.GetString()
                    : periodProp.ToString();
                if (TryParsePeriodKey(periodText, out var periodKey))
                {
                    return FirstDayOfPeriod(periodKey);
                }
            }
            if (doc.RootElement.TryGetProperty("asOfDate", out var asOfProp)
                || doc.RootElement.TryGetProperty("AsOfDate", out asOfProp))
            {
                var asOfText = asOfProp.ValueKind == JsonValueKind.String
                    ? asOfProp.GetString()
                    : asOfProp.ToString();
                if (!string.IsNullOrWhiteSpace(asOfText)
                    && DateTime.TryParse(asOfText, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
                {
                    return TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(parsed);
                }
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // 非 JSON 忽略，回退当月
        }
        return today;
    }

    /// <summary>
    /// 解析是否强制（兼容旧 force 参数）
    /// </summary>
    /// <param name="executeParams">执行参数</param>
    /// <returns>是否 force</returns>
    public static bool TryParseForce(string? executeParams)
    {
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return false;
        }
        var raw = executeParams.Trim();
        if (string.Equals(raw, "force", StringComparison.OrdinalIgnoreCase)
            || string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.ValueKind == JsonValueKind.Object
                && (doc.RootElement.TryGetProperty("force", out var forceProp)
                    || doc.RootElement.TryGetProperty("Force", out forceProp)))
            {
                return forceProp.ValueKind == JsonValueKind.True
                    || (forceProp.ValueKind == JsonValueKind.String
                        && bool.TryParse(forceProp.GetString(), out var parsed)
                        && parsed);
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // 非 JSON 忽略
        }
        return false;
    }

    /// <summary>
    /// 是否 yyyy-MM 核算月份
    /// </summary>
    /// <param name="text">文本</param>
    /// <param name="periodKey">规范化 yyyy-MM</param>
    /// <returns>是否解析成功</returns>
    private static bool TryParsePeriodKey(string? text, out string periodKey)
    {
        periodKey = string.Empty;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }
        var s = text.Trim();
        if (s.Length != 7 || s[4] != '-')
        {
            return false;
        }
        if (!int.TryParse(s.AsSpan(0, 4), NumberStyles.None, CultureInfo.InvariantCulture, out var year)
            || !int.TryParse(s.AsSpan(5, 2), NumberStyles.None, CultureInfo.InvariantCulture, out var month)
            || month < 1
            || month > 12)
        {
            return false;
        }
        periodKey = $"{year:D4}-{month:D2}";
        return true;
    }

    /// <summary>
    /// 核算月份第一天
    /// </summary>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>月初</returns>
    private static DateTime FirstDayOfPeriod(string periodKey)
    {
        var year = int.Parse(periodKey.AsSpan(0, 4), CultureInfo.InvariantCulture);
        var month = int.Parse(periodKey.AsSpan(5, 2), CultureInfo.InvariantCulture);
        return new DateTime(year, month, 1);
    }
}
