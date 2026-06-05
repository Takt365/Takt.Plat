// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktUserContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户上下文实现，从 HTTP 请求头与 Claims 解析当前登录用户
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// <see cref="ITaktUserContext"/> 实现
/// 从 HTTP Context 或 SignalR Hub 调用链中解析当前登录用户、租户与公司
/// </summary>
public class TaktUserContext : ITaktUserContext
{
    /// <summary>
    /// SignalR Hub 调用链上的用户主体（WebSocket 无 HTTP 上下文时使用）
    /// </summary>
    private static readonly AsyncLocal<ClaimsPrincipal?> _hubInvocationPrincipal = new();

    /// <summary>
    /// HTTP 上下文访问器
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 租户/公司请求头配置
    /// </summary>
    private readonly TaktTenantContextOptions _tenantOptions;

    /// <summary>
    /// SignalR Hub 生命周期或客户端调用时写入的 <see cref="ClaimsPrincipal"/>，
    /// 供 <see cref="UserId"/>、<see cref="UserName"/> 在 WebSocket 场景下解析用户
    /// </summary>
    public static ClaimsPrincipal? HubInvocationPrincipal
    {
        get => _hubInvocationPrincipal.Value;
        set => _hubInvocationPrincipal.Value = value;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantOptions">租户上下文配置（请求头名称与默认值）</param>
    public TaktUserContext(IHttpContextAccessor httpContextAccessor, IOptions<TaktTenantContextOptions> tenantOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _tenantOptions = tenantOptions.Value;
    }

    /// <summary>
    /// 当前用户ID（从 JWT 的 sub 或 NameIdentifier claim 解析）
    /// </summary>
    public long? UserId
    {
        get
        {
            var userIdClaim = ResolvePrincipal()?.FindFirst("sub")?.Value
                ?? ResolvePrincipal()?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    /// <summary>
    /// 当前用户名（从 JWT 的 name 或 Name claim 解析）
    /// </summary>
    public string? UserName =>
        ResolvePrincipal()?.FindFirst("name")?.Value
        ?? ResolvePrincipal()?.FindFirst(ClaimTypes.Name)?.Value;

    /// <summary>
    /// 解析当前请求应使用的租户编码（请求头优先，其次 JWT tenant_code；无则 null）
    /// </summary>
    public string? TenantCode
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var headerName = _tenantOptions.TenantHeaderName;
            var tenantCode = httpContext.Request.Headers[headerName].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(tenantCode))
            {
                return tenantCode.Trim();
            }

            var fromClaim = ResolvePrincipal()?.FindFirst("tenant_code")?.Value;
            if (!string.IsNullOrWhiteSpace(fromClaim))
            {
                return fromClaim.Trim();
            }

            return null;
        }
    }

    /// <summary>
    /// 当前公司编码（仅请求头或 JWT company_code，不含 appsettings 默认公司回退）
    /// </summary>
    public string? CompanyCode => RequestCompanyCode;

    /// <summary>
    /// 请求头或 JWT 中的公司编码（未传则为 null）
    /// </summary>
    public string? RequestCompanyCode
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var headerName = _tenantOptions.CompanyHeaderName;
            var companyCode = httpContext.Request.Headers[headerName].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(companyCode))
            {
                return companyCode.Trim();
            }

            var fromClaim = ResolvePrincipal()?.FindFirst("company_code")?.Value;
            return string.IsNullOrWhiteSpace(fromClaim) ? null : fromClaim.Trim();
        }
    }

    /// <summary>
    /// 是否已认证（HTTP 或 Hub 用户主体）
    /// </summary>
    public bool IsAuthenticated => ResolvePrincipal()?.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// 解析当前请求或 Hub 调用的用户主体（优先 Hub AsyncLocal，其次 HTTP User）
    /// </summary>
    /// <returns>已认证的 <see cref="ClaimsPrincipal"/>；无上下文时返回 null</returns>
    private ClaimsPrincipal? ResolvePrincipal()
    {
        var hubPrincipal = HubInvocationPrincipal;
        if (hubPrincipal?.Identity?.IsAuthenticated == true)
        {
            return hubPrincipal;
        }

        return _httpContextAccessor.HttpContext?.User;
    }
}
