// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPagedClamp.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：分页参数规范化（页码、页大小 clamp，Skip 算术安全）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 分页参数规范化工具（纯函数，无状态）
/// </summary>
public static class TaktPagedClamp
{
    /// <summary>
    /// 列表接口允许的最大每页条数
    /// </summary>
    public const int DefaultMaxPageSize = 100;

    /// <summary>
    /// 规范化页码（最小为 1）
    /// </summary>
    /// <param name="pageIndex">原始页码</param>
    /// <returns>不小于 1 的页码</returns>
    public static int NormalizePageIndex(int pageIndex) => Math.Max(1, pageIndex);

    /// <summary>
    /// 规范化每页大小（限制在 1～<paramref name="maxPageSize"/>）
    /// </summary>
    /// <param name="pageSize">原始每页大小</param>
    /// <param name="maxPageSize">上限，默认 <see cref="DefaultMaxPageSize"/></param>
    /// <returns>规范化后的每页大小</returns>
    public static int NormalizePageSize(int pageSize, int maxPageSize = DefaultMaxPageSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxPageSize);
        if (pageSize <= 0)
        {
            return 1;
        }

        return Math.Min(maxPageSize, pageSize);
    }

    /// <summary>
    /// 计算分页 Skip 偏移（checked 防算术溢出）
    /// </summary>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>Skip 行数</returns>
    /// <exception cref="OverflowException">偏移计算溢出时抛出</exception>
    public static int ComputeSkip(int pageIndex, int pageSize)
    {
        var normalizedIndex = NormalizePageIndex(pageIndex);
        var normalizedSize = NormalizePageSize(pageSize);
        return checked((normalizedIndex - 1) * normalizedSize);
    }
}
