// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSecurityOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：安全配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 安全配置选项
/// </summary>
public class TaktSecurityOptions
{
    public const string SectionName = "Security";

    /// <summary>
    /// 频率限制配置
    /// </summary>
    public TaktRateLimitOptions RateLimit { get; set; } = null!;

    /// <summary>
    /// CSRF 防护配置
    /// </summary>
    public TaktCsrfProtectionOptions CsrfProtection { get; set; } = null!;

    /// <summary>
    /// XSS 防护配置
    /// </summary>
    public TaktXssProtectionOptions XssProtection { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (RateLimit == null)
        {
            throw new InvalidOperationException($"{SectionName}:RateLimit 配置不能为空");
        }

        RateLimit.Validate();

        if (CsrfProtection == null)
        {
            throw new InvalidOperationException($"{SectionName}:CsrfProtection 配置不能为空");
        }

        if (XssProtection == null)
        {
            throw new InvalidOperationException($"{SectionName}:XssProtection 配置不能为空");
        }

        XssProtection.Validate();
    }
}

/// <summary>
/// 频率限制配置
/// </summary>
public class TaktRateLimitOptions
{
    /// <summary>
    /// 是否启用全局限流
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 最大请求数
    /// </summary>
    public int MaxRequests { get; set; }

    /// <summary>
    /// 时间窗口（秒）
    /// </summary>
    public int TimeWindowSeconds { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (MaxRequests <= 0)
        {
            throw new InvalidOperationException($"{TaktSecurityOptions.SectionName}:RateLimit:MaxRequests 必须大于 0");
        }

        if (TimeWindowSeconds <= 0)
        {
            throw new InvalidOperationException($"{TaktSecurityOptions.SectionName}:RateLimit:TimeWindowSeconds 必须大于 0");
        }
    }
}

/// <summary>
/// CSRF 防护配置
/// </summary>
public class TaktCsrfProtectionOptions
{
    /// <summary>
    /// 是否启用 CSRF 防护
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// XSS 防护配置
/// </summary>
public class TaktXssProtectionOptions
{
    /// <summary>
    /// 是否启用 XSS 防护
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 允许的文件扩展名
    /// </summary>
    public List<string> AllowedFileExtensions { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (Enabled && (AllowedFileExtensions == null || AllowedFileExtensions.Count == 0))
        {
            throw new InvalidOperationException(
                $"{TaktSecurityOptions.SectionName}:XssProtection:AllowedFileExtensions 在启用时不能为空");
        }
    }
}
