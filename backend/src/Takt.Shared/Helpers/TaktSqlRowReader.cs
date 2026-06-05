// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlRowReader.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：SQL 动态行字典读取辅助（列名大小写不敏感）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;

namespace Takt.Shared.Helpers;

/// <summary>
/// <see cref="Dictionary{TKey,TValue}"/> 形式 SQL 结果行读取器
/// </summary>
public static class TaktSqlRowReader
{
    /// <summary>
    /// 读取字符串列（支持多个候选列名）
    /// </summary>
    /// <param name="row">结果行</param>
    /// <param name="columnNames">列名候选</param>
    /// <returns>去空白后的值；不存在或空则 null</returns>
    public static string? GetString(IReadOnlyDictionary<string, object> row, params string[] columnNames)
    {
        foreach (var name in columnNames)
        {
            if (!row.TryGetValue(name, out var raw) || raw == null)
            {
                continue;
            }

            var text = Convert.ToString(raw, CultureInfo.InvariantCulture)?.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
        }

        return null;
    }

    /// <summary>
    /// 读取 32 位整数列
    /// </summary>
    /// <param name="row">结果行</param>
    /// <param name="columnNames">列名候选</param>
    /// <returns>整数值；不存在或无法转换则 null</returns>
    public static int? GetInt32(IReadOnlyDictionary<string, object> row, params string[] columnNames)
    {
        foreach (var name in columnNames)
        {
            if (!row.TryGetValue(name, out var raw) || raw == null)
            {
                continue;
            }

            if (raw is int intValue)
            {
                return intValue;
            }

            if (raw is long longValue && longValue >= int.MinValue && longValue <= int.MaxValue)
            {
                return (int)longValue;
            }

            if (int.TryParse(Convert.ToString(raw, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
        }

        return null;
    }
}
