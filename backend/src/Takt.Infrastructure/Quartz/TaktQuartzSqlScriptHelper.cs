// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSqlScriptHelper.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：按相对 wwwroot 的 .sql 路径读取 Quartz SQL 并绑定租户占位符（禁止内联 SQL）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz SQL 脚本解析（仅 wwwroot 相对 .sql 路径）
/// </summary>
public static class TaktQuartzSqlScriptHelper
{
    /// <summary>
    /// 解析为可执行 SQL：SqlScript 必须为相对 wwwroot 的 .sql 路径，读文件后替换占位符
    /// </summary>
    /// <param name="sqlScript">相对路径，如 Quartz/sap_sync_ma.sql</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可执行 SQL 文本</returns>
    /// <exception cref="ArgumentException">路径为空或格式非法</exception>
    /// <exception cref="FileNotFoundException">.sql 文件不存在</exception>
    /// <exception cref="InvalidOperationException">路径越界或不安全</exception>
    public static async Task<string> ResolveExecutableSqlAsync(
        string? sqlScript,
        string tenantCode,
        string companyCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlScript);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (!TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(sqlScript))
        {
            throw new ArgumentException(
                "SqlScript 只可填相对 wwwroot 的 .sql 路径（如 Quartz/sap_sync_ma.sql），不允许填写具体 SQL 语句",
                nameof(sqlScript));
        }
        var body = await File.ReadAllTextAsync(ResolveWwwRootSqlFilePath(sqlScript.Trim()), cancellationToken);
        return BindPlaceholders(body, tenantCode, companyCode);
    }

    /// <summary>
    /// 是否为相对 wwwroot 的 .sql 文件引用
    /// </summary>
    /// <param name="sqlScript">SqlScript 字段值</param>
    /// <returns>是文件引用则为 true</returns>
    public static bool IsWwwRootSqlFileReference(string sqlScript)
        => TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(sqlScript);

    /// <summary>
    /// 将相对 wwwroot 的路径解析为绝对路径（禁止 .. 穿越）
    /// </summary>
    /// <param name="relativePath">相对路径，如 Quartz/sap_sync_ma.sql</param>
    /// <returns>绝对路径</returns>
    public static string ResolveWwwRootSqlFilePath(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        if (!TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(relativePath))
        {
            throw new InvalidOperationException($"非法 SQL 脚本路径：{relativePath}");
        }
        var normalized = relativePath.Trim().Replace('\\', '/');
        var wwwroot = Path.GetFullPath(TaktFileHelper.GetWwwRootPath());
        var fullPath = Path.GetFullPath(Path.Combine(wwwroot, normalized.Replace('/', Path.DirectorySeparatorChar)));
        var prefix = wwwroot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(fullPath, wwwroot, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"SQL 脚本路径越界：{relativePath}");
        }
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"未找到 Quartz SQL 脚本：{relativePath}", fullPath);
        }
        return fullPath;
    }

    /// <summary>
    /// 替换 {{TenantCode}} / {{CompanyCode}} / {{SyncUserId}}
    /// </summary>
    /// <param name="sqlTemplate">SQL 模板</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>绑定后的 SQL</returns>
    public static string BindPlaceholders(string sqlTemplate, string tenantCode, string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlTemplate);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (!sqlTemplate.Contains("{{TenantCode}}", StringComparison.Ordinal)
            && !sqlTemplate.Contains("{{CompanyCode}}", StringComparison.Ordinal)
            && !sqlTemplate.Contains("{{SyncUserId}}", StringComparison.Ordinal))
        {
            return sqlTemplate;
        }
        static string Esc(string value) => value.Replace("'", "''", StringComparison.Ordinal);
        return sqlTemplate
            .Replace("{{TenantCode}}", Esc(tenantCode.Trim()), StringComparison.Ordinal)
            .Replace("{{CompanyCode}}", Esc(companyCode.Trim()), StringComparison.Ordinal)
            .Replace(
                "{{SyncUserId}}",
                TaktConstants.SystemAuditUser.Id.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal);
    }
}
