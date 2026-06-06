// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Logging
// 文件名称：TaktAuthLoginLogHandler.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：认证登录统一日志处理器（Serilog 结构化日志 + 登录日志表持久化）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Takt.Application.Services.Identity;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;

namespace Takt.WebApi.Logging;

/// <summary>
/// 认证登录日志写入请求
/// </summary>
public sealed class TaktAuthLoginLogWriteRequest
{
    /// <summary>流程阶段（<see cref="TaktAuthLoginPhases"/>）</summary>
    public required string Phase { get; init; }

    /// <summary>登录方式（<see cref="TaktLoginTypes"/>）</summary>
    public required string LoginType { get; init; }

    /// <summary>登录结果</summary>
    public required TaktLoginResult LoginResult { get; init; }

    /// <summary>租户编码</summary>
    public string TenantCode { get; init; } = string.Empty;

    /// <summary>公司编码</summary>
    public string? CompanyCode { get; init; }

    /// <summary>用户名或 client_id</summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>结果说明</summary>
    public string? Message { get; init; }

    /// <summary>用户 ID（可选）</summary>
    public long? UserId { get; init; }

    /// <summary>耗时（毫秒，可选）</summary>
    public long? ElapsedMs { get; init; }

    /// <summary>是否写入登录日志表（默认 true；RBAC 诊断步骤建议 false）</summary>
    public bool PersistToLoginLog { get; init; } = true;

    /// <summary>结构化诊断字段（权限数、菜单采样等）</summary>
    public IReadOnlyDictionary<string, object?>? Detail { get; init; }
}

/// <summary>
/// 认证流程 RBAC 诊断步骤（仅 Serilog，不写登录日志表）
/// </summary>
public sealed class TaktAuthFlowStepRequest
{
    /// <summary>流程阶段（<see cref="TaktAuthLoginPhases"/>）</summary>
    public required string Phase { get; init; }

    /// <summary>说明</summary>
    public string? Message { get; init; }

    /// <summary>用户 ID</summary>
    public long? UserId { get; init; }

    /// <summary>用户名</summary>
    public string? Username { get; init; }

    /// <summary>租户编码</summary>
    public string? TenantCode { get; init; }

    /// <summary>公司编码</summary>
    public string? CompanyCode { get; init; }

    /// <summary>耗时（毫秒）</summary>
    public long? ElapsedMs { get; init; }

    /// <summary>是否成功</summary>
    public bool IsSuccess { get; init; } = true;

    /// <summary>诊断明细</summary>
    public IReadOnlyDictionary<string, object?>? Detail { get; init; }
}

/// <summary>
/// 认证登录统一日志处理器
/// </summary>
public interface ITaktAuthLoginLogHandler
{
    /// <summary>
    /// 写入认证登录日志（控制台/文件 Serilog + 登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">日志内容</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task WriteAsync(
        HttpContext httpContext,
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 写入认证流程 RBAC 诊断日志（仅 Serilog，不落登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">步骤内容</param>
    void WriteFlowStep(HttpContext httpContext, TaktAuthFlowStepRequest request);
}

/// <summary>
/// <see cref="ITaktAuthLoginLogHandler"/> 实现
/// </summary>
public sealed class TaktAuthLoginLogHandler : ITaktAuthLoginLogHandler
{
    private const string LogModule = "identity/auth";

    private readonly ITaktLoginLogTenantWriter _loginLogTenantWriter;
    private readonly ITaktAuthService _authService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="loginLogTenantWriter">登录日志租户库写入器</param>
    /// <param name="authService">身份认证服务</param>
    public TaktAuthLoginLogHandler(ITaktLoginLogTenantWriter loginLogTenantWriter, ITaktAuthService authService)
    {
        _loginLogTenantWriter = loginLogTenantWriter;
        _authService = authService;
    }

    /// <summary>
    /// 写入认证登录日志（控制台/文件 Serilog + 可选登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">日志内容</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task WriteAsync(
        HttpContext httpContext,
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        var userAgent = httpContext.Request.Headers.UserAgent.ToString();
        var (browser, os) = ParseUserAgent(userAgent);
        var clientIp = GetClientIp(httpContext);
        var requestId = httpContext.Items["RequestId"] as string;
        var path = httpContext.Request.Path.Value;
        var isSuccess = request.LoginResult == TaktLoginResult.Success;

        var logContext = new TaktLogContext
        {
            Module = LogModule,
            Action = request.Phase,
            UserId = request.UserId?.ToString(),
            Username = request.Username,
            TenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode,
            CompanyCode = request.CompanyCode,
            Route = path,
            RequestId = requestId,
            ClientIp = clientIp,
            Extra = MergeExtra(
                new Dictionary<string, object?>
                {
                    ["LoginType"] = request.LoginType,
                    ["LoginResult"] = request.LoginResult.ToString(),
                    ["Message"] = request.Message,
                    ["Browser"] = browser,
                    ["Os"] = os,
                    ["ElapsedMs"] = request.ElapsedMs,
                },
                request.Detail),
        };

        using (TaktLogger.BeginScope(logContext))
        {
            if (isSuccess)
            {
                TaktLogger.Information(
                    logContext,
                    "认证登录 [{Phase}] 成功: 用户={Username}, 租户={TenantCode}, 方式={LoginType}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.Username,
                    request.TenantCode,
                    request.LoginType,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
            else
            {
                TaktLogger.Warning(
                    logContext,
                    "认证登录 [{Phase}] 失败: 用户={Username}, 租户={TenantCode}, 方式={LoginType}, 结果={LoginResult}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.Username,
                    request.TenantCode,
                    request.LoginType,
                    request.LoginResult,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
        }

        if (request.PersistToLoginLog)
        {
            try
            {
                var (tenantCode, companyCode) = await ResolveLoginLogScopeAsync(request, cancellationToken);
                if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
                {
                    TaktLogger.Debug(
                        "[AuthLoginLog] 跳过登录日志表写入：租户或公司未解析 Phase={Phase}, Tenant={TenantCode}, Company={CompanyCode}, Username={Username}",
                        request.Phase,
                        tenantCode ?? string.Empty,
                        companyCode ?? string.Empty,
                        request.Username);
                }
                else
                {
                    await _loginLogTenantWriter.CreateInTenantAsync(
                        tenantCode,
                        new TaktLoginLog
                        {
                            CompanyCode = companyCode,
                            Username = request.Username,
                            LoginType = request.LoginType,
                            LoginResult = request.LoginResult,
                            LoginMessage = BuildPersistMessage(request),
                            LoginIp = clientIp,
                            LoginLocation = TaktLocationHelper.ResolveIpLocationForLog(clientIp),
                            UserAgent = Truncate(userAgent, 500),
                            Browser = browser,
                            Os = os,
                        },
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 持久化登录日志失败: Phase={Phase}, Username={Username}",
                    request.Phase,
                    request.Username);
            }
        }

        if (isSuccess
            && request.UserId is > 0
            && !string.IsNullOrWhiteSpace(request.TenantCode)
            && ShouldRecordUserLastLogin(request.LoginType))
        {
            try
            {
                await _authService.RecordUserLastLoginAsync(
                    request.UserId.Value,
                    request.TenantCode,
                    clientIp,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 更新用户最后登录信息失败: Phase={Phase}, UserId={UserId}",
                    request.Phase,
                    request.UserId);
            }
        }
    }

    /// <summary>
    /// 解析登录日志落库的租户与公司（租户库硬隔离：仅使用请求显式租户，连接 Tenant_{code} 库；公司为公司级共享隔离）
    /// </summary>
    /// <param name="request">认证登录日志请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>租户与公司编码；任一项无法解析则对应值为 null</returns>
    private async Task<(string? TenantCode, string? CompanyCode)> ResolveLoginLogScopeAsync(
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken)
    {
        _ = cancellationToken;
        var tenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode.Trim();
        var companyCode = string.IsNullOrWhiteSpace(request.CompanyCode) ? null : request.CompanyCode.Trim();

        if (!string.IsNullOrEmpty(tenantCode) && !_loginLogTenantWriter.IsTenantDatabaseConfigured(tenantCode))
        {
            TaktLogger.Debug(
                "[AuthLoginLog] 跳过登录日志表写入：未配置租户库 ConnectionStrings:Tenant_{TenantCode}",
                tenantCode);
            return (null, null);
        }

        if (string.IsNullOrEmpty(companyCode)
            && request.UserId is > 0
            && !string.IsNullOrEmpty(tenantCode))
        {
            companyCode = await _loginLogTenantWriter.ResolveUserDefaultCompanyCodeAsync(
                tenantCode,
                request.UserId.Value,
                cancellationToken);
        }

        return (tenantCode, companyCode);
    }

    /// <summary>
    /// 是否应在成功认证后更新用户表最后登录字段
    /// </summary>
    /// <param name="loginType">登录方式</param>
    /// <returns>需要更新为 true</returns>
    private static bool ShouldRecordUserLastLogin(string loginType)
    {
        if (string.IsNullOrWhiteSpace(loginType))
        {
            return false;
        }

        return loginType is not (
            TaktLoginTypes.SignOut
            or TaktLoginTypes.RefreshToken
            or TaktLoginTypes.VerifyPassword
            or TaktLoginTypes.ClientCredentials);
    }

    /// <summary>
    /// 写入认证流程 RBAC 诊断日志（仅 Serilog，不落登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">步骤内容</param>
    public void WriteFlowStep(HttpContext httpContext, TaktAuthFlowStepRequest request)
    {
        var requestId = httpContext.Items["RequestId"] as string;
        var path = httpContext.Request.Path.Value;
        var clientIp = GetClientIp(httpContext);

        var logContext = new TaktLogContext
        {
            Module = LogModule,
            Action = request.Phase,
            UserId = request.UserId?.ToString(),
            Username = request.Username,
            TenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode,
            CompanyCode = request.CompanyCode,
            Route = path,
            RequestId = requestId,
            ClientIp = clientIp,
            Extra = MergeExtra(
                new Dictionary<string, object?>
                {
                    ["Message"] = request.Message,
                    ["ElapsedMs"] = request.ElapsedMs,
                    ["IsSuccess"] = request.IsSuccess,
                },
                request.Detail),
        };

        using (TaktLogger.BeginScope(logContext))
        {
            if (request.IsSuccess)
            {
                TaktLogger.Information(
                    logContext,
                    "认证流程 [{Phase}] 成功: 用户={Username}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.Username ?? string.Empty,
                    request.TenantCode ?? string.Empty,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
            else
            {
                TaktLogger.Warning(
                    logContext,
                    "认证流程 [{Phase}] 失败: 用户={Username}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.Username ?? string.Empty,
                    request.TenantCode ?? string.Empty,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
        }
    }

    /// <summary>
    /// 将诊断明细字段合并到 Serilog Extra 字典
    /// </summary>
    /// <param name="target">基础 Extra 字段</param>
    /// <param name="detail">可选诊断明细</param>
    /// <returns>合并后的字典</returns>
    private static Dictionary<string, object?> MergeExtra(
        Dictionary<string, object?> target,
        IReadOnlyDictionary<string, object?>? detail)
    {
        if (detail == null)
        {
            return target;
        }

        foreach (var pair in detail)
        {
            target[pair.Key] = pair.Value;
        }

        return target;
    }

    /// <summary>
    /// 合并阶段与说明，写入登录日志表 Message 字段
    /// </summary>
    private static string BuildPersistMessage(TaktAuthLoginLogWriteRequest request)
    {
        var message = request.Message?.Trim();
        if (string.IsNullOrEmpty(message))
        {
            return $"[{request.Phase}] {request.LoginResult}";
        }

        return $"[{request.Phase}] {message}";
    }

    /// <summary>
    /// 解析客户端 IP（优先 X-Forwarded-For 首段，否则 RemoteIpAddress）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>IP 字符串；无法解析时为 unknown</returns>
    private static string GetClientIp(HttpContext context)
    {
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            var ip = forwarded.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    /// <summary>
    /// 从 User-Agent 粗粒度解析浏览器与操作系统名称
    /// </summary>
    /// <param name="userAgent">请求头 User-Agent</param>
    /// <returns>浏览器与操作系统；无法识别时为 null</returns>
    private static (string? Browser, string? Os) ParseUserAgent(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return (null, null);
        }

        string? os = null;
        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            os = "Windows";
        }
        else if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase))
        {
            os = "macOS";
        }
        else if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            os = "Android";
        }
        else if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase))
        {
            os = "iOS";
        }
        else if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
        {
            os = "Linux";
        }

        string? browser = null;
        if (userAgent.Contains("Edg/", StringComparison.OrdinalIgnoreCase))
        {
            browser = "Edge";
        }
        else if (userAgent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase))
        {
            browser = "Chrome";
        }
        else if (userAgent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase))
        {
            browser = "Firefox";
        }
        else if (userAgent.Contains("Safari/", StringComparison.OrdinalIgnoreCase))
        {
            browser = "Safari";
        }

        return (browser, os);
    }

    /// <summary>
    /// 截断字符串至指定最大长度（用于 UserAgent 等落库字段）
    /// </summary>
    /// <param name="value">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>截断后的字符串；空值原样返回</returns>
    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength];
    }
}
