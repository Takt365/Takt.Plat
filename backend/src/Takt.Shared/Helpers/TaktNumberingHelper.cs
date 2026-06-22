// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktNumberingHelper.cs
// 功能描述：编号规则 DateFormat 与 ResetPeriod 对齐、日期格式键归一化（与 TaktNumberingService 一致）
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 编号规则 DateFormat ↔ ResetPeriod 对齐辅助（无 I/O）
/// </summary>
public static class TaktNumberingHelper
{
    /// <summary>
    /// 归一化日期格式键（none/空 → none；其余去空白）
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>归一化键</returns>
    public static string NormalizeDateFormatKey(string? dateFormat)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
        {
            return "none";
        }
        var trimmed = dateFormat.Trim();
        return trimmed.Equals("none", StringComparison.OrdinalIgnoreCase) ? "none" : trimmed;
    }

    /// <summary>
    /// 归一化重置周期（兼容 daily/monthly/yearly）
    /// </summary>
    /// <param name="resetPeriod">重置周期</param>
    /// <returns>标准值 none/day/month/year/hour</returns>
    public static string NormalizeResetPeriod(string? resetPeriod)
    {
        if (string.IsNullOrWhiteSpace(resetPeriod))
        {
            return "none";
        }
        return resetPeriod.Trim().ToLowerInvariant() switch
        {
            "daily" => "day",
            "monthly" => "month",
            "yearly" => "year",
            "hourly" => "hour",
            "minutely" => "hour",
            "minute" => "hour",
            _ => resetPeriod.Trim().ToLowerInvariant(),
        };
    }

    /// <summary>
    /// 归一化并迁移已废弃的日期格式（yyyyMMddHHmm → yyyyMMddHH）
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>none 时为 null；否则为支持的格式键</returns>
    public static string? NormalizeSupportedDateFormat(string? dateFormat)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
        {
            return null;
        }
        var trimmed = dateFormat.Trim();
        if (trimmed.Equals("none", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        if (trimmed.Equals("yyyyMMddHHmm", StringComparison.OrdinalIgnoreCase))
        {
            return "yyyyMMddHH";
        }
        return trimmed;
    }

    /// <summary>
    /// 根据 DateFormat 解析与之匹配的重置周期
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="resetPeriod">匹配的重置周期</param>
    /// <returns>是否支持该日期格式</returns>
    public static bool TryResolveRequiredResetPeriod(string? dateFormat, out string resetPeriod)
    {
        resetPeriod = "none";
        var formatKey = NormalizeSupportedDateFormat(dateFormat);
        if (formatKey == null)
        {
            return Assign("none", out resetPeriod);
        }
        return formatKey switch
        {
            "yyyy" => Assign("year", out resetPeriod),
            "yyyyMM" => Assign("month", out resetPeriod),
            "yyyyMMdd" => Assign("day", out resetPeriod),
            "yyyyMMddHH" => Assign("hour", out resetPeriod),
            _ => false,
        };
    }

    /// <summary>
    /// 根据 DateFormat 返回必须使用的重置周期
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <returns>重置周期</returns>
    /// <exception cref="ArgumentException">不支持的日期格式</exception>
    public static string ResolveRequiredResetPeriod(string? dateFormat)
    {
        if (!TryResolveRequiredResetPeriod(dateFormat, out var resetPeriod))
        {
            throw new ArgumentException($"不支持的日期格式: {dateFormat}", nameof(dateFormat));
        }
        return resetPeriod;
    }

    /// <summary>
    /// 重置周期是否与 DateFormat 粒度匹配
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="resetPeriod">重置周期</param>
    /// <returns>是否匹配</returns>
    public static bool IsResetPeriodMatched(string? dateFormat, string? resetPeriod)
    {
        if (!TryResolveRequiredResetPeriod(dateFormat, out var required))
        {
            return false;
        }
        return string.Equals(NormalizeResetPeriod(resetPeriod), required, StringComparison.OrdinalIgnoreCase);
    }

    private static bool Assign(string value, out string resetPeriod)
    {
        resetPeriod = value;
        return true;
    }

    /// <summary>
    /// 归一化业务领域（兼容旧版数字 document_type 种子）
    /// </summary>
    /// <param name="documentType">业务领域或旧版数字</param>
    /// <returns>标准领域文本</returns>
    public static string NormalizeDocumentType(string? documentType)
    {
        if (string.IsNullOrWhiteSpace(documentType))
        {
            return string.Empty;
        }
        var trimmed = documentType.Trim();
        if (int.TryParse(trimmed, out var legacyCode))
        {
            return legacyCode switch
            {
                1 or 9 => "Foundation",
                2 => "Routine",
                3 => "Accounting",
                4 => "Logistics",
                5 => "HumanResource",
                6 => "Identity",
                7 => "Workflow",
                8 => "Code",
                _ => string.Empty,
            };
        }
        return trimmed;
    }

    /// <summary>
    /// 归一化编码段键（兼容旧版 Prefix/Suffix 段名）
    /// </summary>
    /// <param name="segmentKey">段键</param>
    /// <returns>标准段名</returns>
    public static string NormalizeSegmentKey(string segmentKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(segmentKey);
        if (segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
        {
            return "PrefixCode";
        }
        if (segmentKey.Equals("Suffix", StringComparison.OrdinalIgnoreCase))
        {
            return "SuffixCode";
        }
        return segmentKey.Trim();
    }

    /// <summary>
    /// 归一化 segments 描述中的段名（Prefix → PrefixCode）
    /// </summary>
    /// <param name="description">Description 字段</param>
    /// <returns>归一化后的段配置</returns>
    public static string NormalizeSegmentDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return description ?? string.Empty;
        }
        var text = description.Trim();
        if (!text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return text;
        }
        const string marker = "segments:";
        var body = text[marker.Length..];
        var keys = body.Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (keys.Length == 0)
        {
            return text;
        }
        return marker + string.Join(",", keys.Select(NormalizeSegmentKey));
    }
}
