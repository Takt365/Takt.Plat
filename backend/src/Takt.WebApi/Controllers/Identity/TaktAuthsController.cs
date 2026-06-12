// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktAuthsController.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：身份认证统一入口（Cookie 会话、OAuth2/PKCE、当前用户 RBAC）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using Takt.Shared.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services.Identity;
using Takt.Domain.Interfaces;
using Takt.Shared.Enums;
using Takt.Shared.Options;
using Takt.WebApi.Controllers;
using Takt.WebApi.Logging;
using Takt.WebApi.OpenIddict;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// 身份认证控制器（会话 signin、OAuth /connect/*、GET me）
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "身份认证")]
public class TaktAuthsController : TaktControllerBase
{
    private readonly ITaktAuthService _authService;
    private readonly ITaktLoginTicketService _loginTicketService;
    private readonly ITaktCaptchaService _captchaService;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly TaktOpenIddictLogHandler _openIddictLogHandler;
    private readonly ITaktAuthLoginLogHandler _authLoginLogHandler;
    private readonly TaktOpenIddictOptions _openIddictOptions;
    private readonly TaktTenantContextOptions _tenantContextOptions;
    private readonly TaktCaptchaOptions _captchaOptions;
    private readonly TaktPasswordPolicyOptions _passwordPolicyOptions;
    private readonly TaktLocalizationOptions _localizationOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="authService">身份认证服务</param>
    /// <param name="loginTicketService">登录票据服务</param>
    /// <param name="captchaService">验证码服务</param>
    /// <param name="passwordPolicyOptions">密码策略（含 RSA 传输密钥）</param>
    /// <param name="applicationManager">OpenIddict 应用管理器</param>
    /// <param name="openIddictLogHandler">OpenIddict 运行时处理器</param>
    /// <param name="authLoginLogHandler">认证登录统一日志处理器</param>
    /// <param name="openIddictOptions">OpenIddict 配置</param>
    /// <param name="tenantContextOptions">租户上下文配置</param>
    /// <param name="captchaOptions">验证码配置</param>
    /// <param name="localizationOptions">本地化配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAuthsController(
        ITaktAuthService authService,
        ITaktLoginTicketService loginTicketService,
        ITaktCaptchaService captchaService,
        IOptions<TaktPasswordPolicyOptions> passwordPolicyOptions,
        IOpenIddictApplicationManager applicationManager,
        TaktOpenIddictLogHandler openIddictLogHandler,
        ITaktAuthLoginLogHandler authLoginLogHandler,
        IOptions<TaktOpenIddictOptions> openIddictOptions,
        IOptions<TaktTenantContextOptions> tenantContextOptions,
        IOptions<TaktCaptchaOptions> captchaOptions,
        IOptions<TaktLocalizationOptions> localizationOptions,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _authService = authService;
        _loginTicketService = loginTicketService;
        _captchaService = captchaService;
        _applicationManager = applicationManager;
        _openIddictLogHandler = openIddictLogHandler;
        _authLoginLogHandler = authLoginLogHandler;
        _openIddictOptions = openIddictOptions.Value;
        _tenantContextOptions = tenantContextOptions.Value;
        _captchaOptions = captchaOptions.Value;
        _passwordPolicyOptions = passwordPolicyOptions.Value;
        _localizationOptions = localizationOptions.Value;
    }

    #region Cookie 会话（PKCE 前置，匿名）

    /// <summary>
    /// 获取登录密码 RSA 公钥（匿名；前端加密后提交密文）
    /// </summary>
    /// <returns>公钥 PEM</returns>
    [AllowAnonymous]
    [HttpGet("session/login-public-key")]
    public IActionResult GetLoginPublicKey()
    {
        if (!_passwordPolicyOptions.Transport.Enabled)
        {
            return BadRequest(new { message = GetValidationMessage(TaktValidationI18nKeys.SystemFeatureDisabled, extraTokens: new Dictionary<string, string> { ["feature"] = GetLocalizedString(TaktValidationI18nKeys.FieldPasswordEncryption) }) });
        }

        return Ok(new TaktLoginPublicKeyResponseDto
        {
            Algorithm = "RSA-PKCS1",
            PublicKeyPem = _passwordPolicyOptions.Transport.PublicKeyPem,
        });
    }

    /// <summary>
    /// 建立 Cookie 登录会话
    /// </summary>
    /// <param name="dto">登录请求</param>
    /// <returns>是否成功</returns>
    [AllowAnonymous]
    [HttpPost("session/signin")]
    [TaktPermission("identity:auth:signin", "登录会话")]
    public async Task<IActionResult> SignInSessionAsync([FromBody] TaktLoginRequestDto dto)
    {
        var tenantCode = NormalizeLoginTenantCode(dto.TenantCode);
        var username = NormalizeLoginUsername(dto.Username);
        if (string.IsNullOrEmpty(tenantCode))
        {
            var tenantRequiredMessage = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldTenantCode);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.SignInSession, tenantCode, dto.CompanyCode, username,
                TaktLoginType.Password, TaktLoginResult.PasswordError, tenantRequiredMessage);
            return BadRequest(new { message = tenantRequiredMessage });
        }

        ApplyLoginContextHeaders(tenantCode, null);

        if (!await _authService.ValidateUserTenantAccessAsync(tenantCode, username))
        {
            var tenantAccessMessage = GetLocalizedString(TaktValidationI18nKeys.PermissionTenantNoAccess);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.SignInSession, tenantCode, dto.CompanyCode, username,
                TaktLoginType.Password, TaktLoginResult.PasswordError, tenantAccessMessage);
            return Unauthorized(new { message = tenantAccessMessage });
        }

        var userId = await ResolveSignInUserIdAsync(tenantCode, username, dto);
        if (userId == null)
        {
            var invalidCredentialsMessage = GetValidationMessage(TaktValidationI18nKeys.Incorrect, TaktValidationI18nKeys.FieldLoginCredentials);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.SignInSession, tenantCode, dto.CompanyCode, username,
                TaktLoginType.Password, TaktLoginResult.PasswordError, invalidCredentialsMessage);
            return Unauthorized(new { message = invalidCredentialsMessage });
        }

        if (_captchaOptions.Enabled)
        {
            if (string.IsNullOrWhiteSpace(dto.CaptchaId) || string.IsNullOrWhiteSpace(dto.CaptchaCode))
            {
                var captchaRequiredMessage = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldCaptcha);
                await _openIddictLogHandler.CreateLoginLogAsync(
                    HttpContext, TaktAuthLoginPhases.SignInSession, tenantCode, dto.CompanyCode, username,
                    TaktLoginType.Password, TaktLoginResult.CaptchaError, captchaRequiredMessage);
                return BadRequest(new { message = captchaRequiredMessage, captchaRequired = true });
            }

            var captchaVerify = await _captchaService.VerifyAsync(new TaktCaptchaVerifyRequest
            {
                CaptchaId = dto.CaptchaId,
                UserInput = dto.CaptchaCode,
            });
            if (!captchaVerify.Success)
            {
                var captchaInvalidMessage = GetValidationMessage(TaktValidationI18nKeys.NotFoundOrExpired, TaktValidationI18nKeys.FieldCaptcha);
                await _openIddictLogHandler.CreateLoginLogAsync(
                    HttpContext, TaktAuthLoginPhases.SignInSession, tenantCode, dto.CompanyCode, username,
                    TaktLoginType.Password, TaktLoginResult.CaptchaError, captchaInvalidMessage);
                return BadRequest(new { message = captchaInvalidMessage });
            }
        }

        var cultureCode = ResolveLoginCultureCode(dto);
        var tenantAndCompany = await _authService.ResolveLoginTenantAndCompanyAsync(
            userId.Value,
            tenantCode,
            cultureCode,
            dto.CompanyCode);

        ApplyLoginContextHeaders(tenantAndCompany.TenantCode, tenantAndCompany.CompanyCode);

        var principal = await _openIddictLogHandler.CreateUserPrincipalAsync(
            userId.Value,
            username,
            tenantAndCompany.TenantCode,
            tenantAndCompany.CompanyCode);

        await HttpContext.SignInAsync(
            TaktAuthCookieDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = dto.RememberMe,
                ExpiresUtc = dto.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(14)
                    : DateTimeOffset.UtcNow.AddHours(8)
            });

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.SignInSession, tenantAndCompany.TenantCode, tenantAndCompany.CompanyCode,
            username, TaktLoginType.Password, TaktLoginResult.Success, "登录会话已建立", userId.Value);

        return Ok(new { success = true });
    }

    /// <summary>
    /// 清除 Cookie 登录会话
    /// </summary>
    /// <returns>是否成功</returns>
    [AllowAnonymous]
    [HttpPost("session/signout")]
    [TaktPermission("identity:auth:signout", "注销会话")]
    public async Task<IActionResult> SignOutSessionAsync()
    {
        var cookieAuth = await HttpContext.AuthenticateAsync(TaktAuthCookieDefaults.AuthenticationScheme);
        var signOutUsername = cookieAuth?.Principal?.FindFirst(Claims.Name)?.Value
            ?? cookieAuth?.Principal?.FindFirst(Claims.Subject)?.Value
            ?? string.Empty;
        var signOutTenant = cookieAuth?.Principal?.FindFirst("tenant_code")?.Value ?? string.Empty;
        var signOutCompany = cookieAuth?.Principal?.FindFirst("company_code")?.Value;

        await HttpContext.SignOutAsync(TaktAuthCookieDefaults.AuthenticationScheme);

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.SignOutSession, signOutTenant, signOutCompany, signOutUsername,
            TaktLoginType.SignOut, TaktLoginResult.Success, "Cookie 会话已注销");

        return Ok(new { success = true });
    }

    /// <summary>
    /// 登录预检：① 租户下是否存在可登录用户（不验密）→ ② 验密 → 返回是否需验证码与登录票据
    /// </summary>
    /// <param name="dto">校验请求</param>
    /// <returns>校验结果</returns>
    [AllowAnonymous]
    [HttpPost("session/verify-password")]
    public async Task<IActionResult> VerifySessionPasswordAsync([FromBody] TaktSessionVerifyPasswordRequestDto dto)
    {
        var stopwatch = Stopwatch.StartNew();
        var tenantCode = NormalizeLoginTenantCode(dto.TenantCode);
        var username = NormalizeLoginUsername(dto.Username);
        if (string.IsNullOrEmpty(tenantCode))
        {
            var tenantRequiredMessage = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldTenantCode);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.VerifyPassword, tenantCode, null, username,
                TaktLoginType.VerifyPassword, TaktLoginResult.PasswordError, tenantRequiredMessage,
                elapsedMs: stopwatch.ElapsedMilliseconds);
            return BadRequest(new { message = tenantRequiredMessage });
        }

        ApplyLoginContextHeaders(tenantCode, null);

        // ① 租户下用户登录权限（不验证密码）
        if (!await _authService.ValidateUserTenantAccessAsync(tenantCode, username))
        {
            var tenantAccessMessage = GetLocalizedString(TaktValidationI18nKeys.PermissionTenantNoAccess);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.VerifyPassword, tenantCode, null, username,
                TaktLoginType.VerifyPassword, TaktLoginResult.PasswordError, tenantAccessMessage,
                elapsedMs: stopwatch.ElapsedMilliseconds);
            return Unauthorized(new { message = tenantAccessMessage });
        }

        // ② 解密并验证密码
        if (!TryDecryptTransportPassword(dto.Password, out var plainPassword, out var cipherError))
        {
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.VerifyPassword, tenantCode, null, username,
                TaktLoginType.VerifyPassword, TaktLoginResult.PasswordError, cipherError,
                elapsedMs: stopwatch.ElapsedMilliseconds);
            return BadRequest(new { message = cipherError });
        }

        var userId = await _authService.ValidateUserPasswordOnlyAsync(tenantCode, username, plainPassword);
        if (userId == null)
        {
            var invalidCredentialsMessage = GetValidationMessage(TaktValidationI18nKeys.Incorrect, TaktValidationI18nKeys.FieldLoginCredentials);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.VerifyPassword, tenantCode, null, username,
                TaktLoginType.VerifyPassword, TaktLoginResult.PasswordError, invalidCredentialsMessage,
                elapsedMs: stopwatch.ElapsedMilliseconds);
            return Unauthorized(new { message = invalidCredentialsMessage });
        }

        var loginTicket = await _loginTicketService.CreateLoginTicketAsync(
            userId.Value,
            tenantCode,
            username);

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.VerifyPassword, tenantCode, null, username,
            TaktLoginType.VerifyPassword, TaktLoginResult.Success, "密码校验通过，已签发登录票据",
            userId.Value, stopwatch.ElapsedMilliseconds);

        return Ok(new TaktSessionVerifyPasswordResponseDto
        {
            PasswordValid = true,
            CaptchaRequired = _captchaOptions.Enabled,
            LoginTicket = loginTicket,
        });
    }

    /// <summary>
    /// 解析 signin 用户 ID：有效登录票据则跳过重复验密，否则校验密码
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名（已规范化）</param>
    /// <param name="dto">登录请求</param>
    /// <returns>用户 ID；失败返回 null</returns>
    private async Task<long?> ResolveSignInUserIdAsync(string tenantCode, string username, TaktLoginRequestDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.LoginTicket))
        {
            return await _loginTicketService.ConsumeLoginTicketAsync(
                dto.LoginTicket,
                tenantCode,
                username);
        }

        if (!TryDecryptTransportPassword(dto.Password, out var plainPassword, out _))
        {
            return null;
        }

        return await _authService.ValidateUserPasswordOnlyAsync(tenantCode, username, plainPassword);
    }

    /// <summary>
    /// 解密前端 RSA 传输的登录密码
    /// </summary>
    private bool TryDecryptTransportPassword(string cipherPassword, out string plainPassword, out string errorMessage)
    {
        plainPassword = string.Empty;
        errorMessage = string.Empty;

        if (!_passwordPolicyOptions.Transport.Enabled)
        {
            errorMessage = GetValidationMessage(TaktValidationI18nKeys.SystemFeatureDisabled, extraTokens: new Dictionary<string, string> { ["feature"] = GetLocalizedString(TaktValidationI18nKeys.FieldPasswordEncryption) });
            return false;
        }

        try
        {
            plainPassword = TaktEncryptHelper.DecryptPassword(
                cipherPassword,
                _passwordPolicyOptions.Transport.PrivateKeyPem);
            return true;
        }
        catch (CryptographicException)
        {
            errorMessage = GetValidationMessage(TaktValidationI18nKeys.InvalidFormat, TaktValidationI18nKeys.FieldPasswordCipher);
            return false;
        }
        catch (ArgumentException)
        {
            errorMessage = GetValidationMessage(TaktValidationI18nKeys.InvalidFormat, TaktValidationI18nKeys.FieldPasswordCipher);
            return false;
        }
    }

    /// <summary>
    /// 生成登录验证码挑战（匿名）
    /// </summary>
    /// <returns>验证码挑战</returns>
    [AllowAnonymous]
    [HttpGet("session/captcha")]
    public async Task<IActionResult> GetSessionCaptchaAsync()
    {
        try
        {
            if (!_captchaOptions.Enabled)
            {
                return BadRequest(new { message = GetValidationMessage(TaktValidationI18nKeys.SystemFeatureDisabled, extraTokens: new Dictionary<string, string> { ["feature"] = GetLocalizedString(TaktValidationI18nKeys.FieldCaptcha) }) });
            }

            var generated = await _captchaService.GenerateAsync();
            var challenge = new TaktCaptchaChallengeDto
            {
                CaptchaId = generated.CaptchaId,
                CaptchaType = generated.Type,
                Width = _captchaOptions.Slider.Width,
                Height = _captchaOptions.Slider.Height,
                SliderWidth = _captchaOptions.Slider.SliderWidth,
                SliderHeight = _captchaOptions.Slider.SliderHeight,
                RequireBehaviorData = string.Equals(
                        generated.Type,
                        TaktCaptchaTypeNames.Behavior,
                        StringComparison.OrdinalIgnoreCase)
                    || _captchaOptions.Slider.RequireBehaviorData,
                BackgroundImage = generated.BackgroundImage,
                SliderImage = generated.SliderImage,
                TargetPosition = string.Equals(
                        generated.Type,
                        TaktCaptchaTypeNames.Behavior,
                        StringComparison.OrdinalIgnoreCase)
                    ? generated.TargetPosition
                    : null,
            };
            return Success(challenge, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取登录页租户选项（匿名；返回全部租户，登录时按用户校验租户权限）
    /// </summary>
    /// <returns>租户下拉选项</returns>
    [AllowAnonymous]
    [HttpGet("session/tenant-options")]
    public async Task<IActionResult> GetSessionTenantOptionsAsync()
    {
        try
        {
            var result = await _authService.GetSessionTenantOptionsAsync();
            return Success(result, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 校验登录页输入的租户编码是否存在且启用（匿名）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>存在且启用为 true</returns>
    [AllowAnonymous]
    [HttpGet("session/tenant-validate")]
    public async Task<IActionResult> ValidateSessionTenantCodeAsync([FromQuery] string tenantCode)
    {
        try
        {
            var valid = await _authService.ValidateSessionTenantCodeAsync(tenantCode);
            return Success(valid, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取登录页语言切换选项（匿名；须传 tenantCode，仅查询该租户库）
    /// </summary>
    /// <param name="tenantCode">租户编码（必填，与 X-Tenant-Code、登录页输入一致）</param>
    /// <returns>语言下拉选项</returns>
    [AllowAnonymous]
    [HttpGet("session/culture-options")]
    public async Task<IActionResult> GetSessionCultureOptionsAsync([FromQuery] string? tenantCode = null)
    {
        try
        {
            var result = await _authService.GetSessionCultureOptionsAsync(tenantCode);
            return Success(result, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 登录前预览：用户默认公司、用户 DefaultCulture 与公司 DefaultCulture（未签发 OAuth 访问令牌）
    /// </summary>
    /// <param name="tenantCode">租户编码（与 X-Tenant-Code、登录页输入一致）</param>
    /// <param name="username">登录用户名</param>
    /// <returns>公司编码、用户/公司默认语言</returns>
    [AllowAnonymous]
    [HttpGet("session/login-preview-locale")]
    public async Task<IActionResult> GetLoginPreviewLocaleAsync([FromQuery] string tenantCode, [FromQuery] string username)
    {
        try
        {
            var tenantError = ValidateLoginPreviewTenantHeader(tenantCode);
            if (tenantError != null)
            {
                return tenantError;
            }

            var result = await _authService.GetLoginPreviewLocaleAsync(tenantCode, username);
            return Success(result, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前用户可切换的公司选项（已登录，按权限过滤）
    /// </summary>
    /// <returns>公司下拉选项</returns>
    [Authorize]
    [HttpGet("me/company-options")]
    public async Task<IActionResult> GetUserCompanyOptionsAsync()
    {
        try
        {
            var result = await _authService.GetUserCompanyOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    /// <summary>
    /// 解析登录请求的区域文化编码
    /// </summary>
    private string ResolveLoginCultureCode(TaktLoginRequestDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.CultureCode))
        {
            return dto.CultureCode.Trim();
        }

        var acceptLanguage = HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(acceptLanguage))
        {
            var first = acceptLanguage.Split(',')[0].Trim();
            if (!string.IsNullOrEmpty(first))
            {
                return first;
            }
        }

        return _localizationOptions.DefaultCulture;
    }

    /// <summary>
    /// 规范化登录租户编码
    /// </summary>
    /// <param name="tenantCode">原始租户编码</param>
    /// <returns>去空白后的租户编码</returns>
    private static string NormalizeLoginTenantCode(string? tenantCode)
    {
        return string.IsNullOrWhiteSpace(tenantCode) ? string.Empty : tenantCode.Trim();
    }

    /// <summary>
    /// 规范化登录用户名（与前端 normalizeUsername 一致：trim + 小写）
    /// </summary>
    /// <param name="username">原始用户名</param>
    /// <returns>规范化用户名</returns>
    private static string NormalizeLoginUsername(string? username)
    {
        return string.IsNullOrWhiteSpace(username) ? string.Empty : username.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// 将登录解析出的租户/公司写入当前请求头，供公司级仓储过滤
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码（为空时移除公司头，避免回落 appsettings 默认公司）</param>
    private void ApplyLoginContextHeaders(string tenantCode, string? companyCode)
    {
        HttpContext.Request.Headers[_tenantContextOptions.TenantHeaderName] = tenantCode;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            HttpContext.Request.Headers.Remove(_tenantContextOptions.CompanyHeaderName);
            return;
        }

        HttpContext.Request.Headers[_tenantContextOptions.CompanyHeaderName] = companyCode.Trim();
    }

    #region OAuth2 / OIDC（/connect/*，匿名）

    /// <summary>
    /// 授权端点（Authorization Code + PKCE）
    /// </summary>
    /// <returns>重定向登录页或签发授权码</returns>
    [AllowAnonymous]
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    public async Task<IActionResult> AuthorizeAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var cookieAuth = await HttpContext.AuthenticateAsync(TaktAuthCookieDefaults.AuthenticationScheme);
        if (cookieAuth?.Principal == null)
        {
            var returnUrl = BuildOAuthReturnUrl();
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.OAuthAuthorize, string.Empty, null, string.Empty,
                TaktLoginType.OAuthAuthorize, TaktLoginResult.PasswordError,
                $"未建立 Cookie 会话，重定向登录页: {returnUrl}");
            var loginUrl = $"{_openIddictOptions.FrontendLoginUrl.TrimEnd('/')}?returnUrl={Uri.EscapeDataString(returnUrl)}";
            return Redirect(loginUrl);
        }

        var authUsername = cookieAuth.Principal.FindFirst(Claims.Name)?.Value
            ?? cookieAuth.Principal.FindFirst(Claims.Subject)?.Value
            ?? string.Empty;
        var authTenant = cookieAuth.Principal.FindFirst("tenant_code")?.Value ?? string.Empty;
        var authCompany = cookieAuth.Principal.FindFirst("company_code")?.Value;

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.OAuthAuthorize, authTenant, authCompany, authUsername,
            TaktLoginType.OAuthAuthorize, TaktLoginResult.Success, "OAuth 授权成功");

        var oidcPrincipal = _openIddictLogHandler.CreateOpenIddictPrincipal(
            cookieAuth.Principal,
            request.GetScopes());

        var resources = request.GetResources();
        if (resources.IsDefaultOrEmpty)
        {
            resources = ImmutableArray.Create(_openIddictOptions.ApiAudience);
        }

        oidcPrincipal.SetResources(resources);

        return SignIn(oidcPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// 构建 OAuth 回调地址（开发环境经 Vite 代理时优先使用 X-Forwarded-*）
    /// </summary>
    /// <returns>完整授权请求 URL</returns>
    private string BuildOAuthReturnUrl()
    {
        var forwardedHost = Request.Headers["X-Forwarded-Host"].FirstOrDefault();
        var forwardedProto = Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedHost))
        {
            var scheme = string.IsNullOrWhiteSpace(forwardedProto) ? Request.Scheme : forwardedProto;
            return $"{scheme}://{forwardedHost}{Request.PathBase}{Request.Path}{Request.QueryString}";
        }

        return $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
    }

    /// <summary>
    /// 令牌端点（authorization_code / refresh_token / client_credentials）
    /// </summary>
    /// <returns>令牌 JSON</returns>
    [AllowAnonymous]
    [HttpPost("~/connect/token")]
    [Produces("application/json")]
    public async Task<IActionResult> ExchangeTokenAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsGrantAsync(request);
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            return await HandleAuthorizationCodeGrantAsync();
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await HandleRefreshTokenGrantAsync();
        }

        throw new InvalidOperationException("The specified grant type is not supported by this endpoint.");
    }

    /// <summary>
    /// OIDC 登出端点
    /// </summary>
    /// <returns>登出重定向</returns>
    [AllowAnonymous]
    [HttpGet("~/connect/logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var cookieAuth = await HttpContext.AuthenticateAsync(TaktAuthCookieDefaults.AuthenticationScheme);
        var logoutUsername = cookieAuth?.Principal?.FindFirst(Claims.Name)?.Value
            ?? cookieAuth?.Principal?.FindFirst(Claims.Subject)?.Value
            ?? string.Empty;
        var logoutTenant = cookieAuth?.Principal?.FindFirst("tenant_code")?.Value ?? string.Empty;
        var logoutCompany = cookieAuth?.Principal?.FindFirst("company_code")?.Value;

        await HttpContext.SignOutAsync(TaktAuthCookieDefaults.AuthenticationScheme);

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.OidcLogout, logoutTenant, logoutCompany, logoutUsername,
            TaktLoginType.SignOut, TaktLoginResult.Success, "OIDC 登出");

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = _openIddictOptions.SpaPostLogoutRedirectUris.FirstOrDefault()
                    ?? _openIddictOptions.FrontendLoginUrl
            });
    }

    #endregion

    #region 当前用户（Bearer，需登录）

    /// <summary>
    /// 获取当前登录用户资料（权限、菜单、路由）
    /// </summary>
    /// <returns>用户资料</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var requestStopwatch = Stopwatch.StartNew();
        try
        {
            var profile = await _authService.GetCurrentUserAsync();
            if (profile == null)
            {
                _authLoginLogHandler.WriteFlowStep(
                    HttpContext,
                    new TaktAuthFlowStepRequest
                    {
                        Phase = TaktAuthLoginPhases.UserProfile,
                        Message = GetLocalizedString("common.permission.unauthorized"),
                        IsSuccess = false,
                        ElapsedMs = requestStopwatch.ElapsedMilliseconds,
                    });
                return Unauthorized(GetLocalizedString("common.permission.unauthorized"));
            }

            _authLoginLogHandler.WriteFlowStep(
                HttpContext,
                new TaktAuthFlowStepRequest
                {
                    Phase = TaktAuthLoginPhases.UserProfile,
                    UserId = profile.UserId,
                    Username = profile.Username,
                    TenantCode = profile.TenantCode,
                    CompanyCode = profile.CompanyCode,
                    Message = "GET /me 返回用户资料",
                    ElapsedMs = requestStopwatch.ElapsedMilliseconds,
                    Detail = new Dictionary<string, object?>
                    {
                        ["RoleCount"] = profile.Roles.Count,
                        ["PermissionCount"] = profile.Permissions.Count,
                        ["TopLevelMenuCount"] = profile.Menus.Count,
                        ["RoutePathCount"] = profile.RoutePaths.Count,
                        ["AccessibleCompanyCount"] = profile.AccessibleCompanies.Count,
                    },
                });

            return Success(profile, GetLocalizedString("common.feedback.query.success"));
        }
        catch (Exception ex)
        {
            _authLoginLogHandler.WriteFlowStep(
                HttpContext,
                new TaktAuthFlowStepRequest
                {
                    Phase = TaktAuthLoginPhases.UserProfile,
                    Message = ex.Message,
                    IsSuccess = false,
                    ElapsedMs = requestStopwatch.ElapsedMilliseconds,
                });
            return HandleException(ex);
        }
    }

    #endregion

    #region OAuth 私有方法

    private async Task<IActionResult> HandleClientCredentialsGrantAsync(OpenIddictRequest request)
    {
        var clientId = request.ClientId;
        if (string.IsNullOrEmpty(clientId))
        {
            var clientIdMissingMessage = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldClientId);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.ClientCredentials, string.Empty, null, string.Empty,
                TaktLoginType.ClientCredentials, TaktLoginResult.PasswordError, clientIdMissingMessage);
            throw new InvalidOperationException("The client_id is missing.");
        }

        var application = await _applicationManager.FindByClientIdAsync(clientId);
        if (application == null)
        {
            var applicationNotFoundMessage = GetValidationMessage(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.FieldApplication);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.ClientCredentials, string.Empty, null, clientId,
                TaktLoginType.ClientCredentials, TaktLoginResult.PasswordError, applicationNotFoundMessage);
            throw new InvalidOperationException("The application cannot be found.");
        }

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.ClientCredentials, string.Empty, null, clientId,
            TaktLoginType.ClientCredentials, TaktLoginResult.Success, "客户端凭证登录成功");

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));
        identity.SetDestinations(TaktOpenIddictLogHandler.GetClaimDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleAuthorizationCodeGrantAsync()
    {
        var principal = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (principal.Principal == null)
        {
            var authorizationCodeInvalidMessage = GetValidationMessage(TaktValidationI18nKeys.NotFoundOrExpired, TaktValidationI18nKeys.FieldAuthorizationCode);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.AuthorizationCode, string.Empty, null, string.Empty,
                TaktLoginType.AuthorizationCode, TaktLoginResult.PasswordError, authorizationCodeInvalidMessage);
            return OAuthForbid(Errors.InvalidGrant, authorizationCodeInvalidMessage);
        }

        var codeUsername = principal.Principal.FindFirst(Claims.Name)?.Value
            ?? principal.Principal.FindFirst(Claims.Subject)?.Value
            ?? string.Empty;
        var codeTenant = principal.Principal.FindFirst("tenant_code")?.Value ?? string.Empty;
        var codeCompany = principal.Principal.FindFirst("company_code")?.Value;

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.AuthorizationCode, codeTenant, codeCompany, codeUsername,
            TaktLoginType.AuthorizationCode, TaktLoginResult.Success, "授权码换令牌成功");

        return SignIn(principal.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleRefreshTokenGrantAsync()
    {
        var principal = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var identity = principal.Principal?.Identity as ClaimsIdentity;
        if (identity == null)
        {
            var refreshTokenInvalidMessage = GetValidationMessage(TaktValidationI18nKeys.NotFoundOrExpired, TaktValidationI18nKeys.FieldRefreshToken);
            await _openIddictLogHandler.CreateLoginLogAsync(
                HttpContext, TaktAuthLoginPhases.RefreshToken, string.Empty, null, string.Empty,
                TaktLoginType.RefreshToken, TaktLoginResult.PasswordError, refreshTokenInvalidMessage);
            return OAuthForbid(Errors.InvalidGrant, refreshTokenInvalidMessage);
        }

        var refreshUsername = principal.Principal?.FindFirst(Claims.Name)?.Value
            ?? principal.Principal?.FindFirst(Claims.Subject)?.Value
            ?? string.Empty;
        var refreshTenant = principal.Principal?.FindFirst("tenant_code")?.Value ?? string.Empty;
        var refreshCompany = principal.Principal?.FindFirst("company_code")?.Value;

        await _openIddictLogHandler.CreateLoginLogAsync(
            HttpContext, TaktAuthLoginPhases.RefreshToken, refreshTenant, refreshCompany, refreshUsername,
            TaktLoginType.RefreshToken, TaktLoginResult.Success, "刷新令牌成功");

        var newPrincipal = new ClaimsPrincipal(identity);
        newPrincipal.SetScopes(principal.Principal?.GetScopes());
        newPrincipal.SetResources(principal.Principal?.GetResources());
        TaktOpenIddictLogHandler.ApplyClaimDestinations(newPrincipal);

        return SignIn(newPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private IActionResult OAuthForbid(string error, string description)
    {
        return Forbid(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
            }));
    }

    #endregion
}
