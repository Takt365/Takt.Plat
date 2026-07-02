// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktAuthService.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：身份认证服务实现（登录验密 + RBAC 权限与当前用户资料）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using Mapster;
using Microsoft.Extensions.Options;
using Takt.Application.Dtos.Identity;
using Takt.Application.Services;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Extensions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 身份认证服务实现
/// </summary>
public class TaktAuthService : TaktServiceBase, ITaktAuthService
{
    private const int AuthLogSampleSize = 40;

    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktCompanyRepository<TaktUserCompany> _userCompanyRepository;
    private readonly ITaktPermissionService _permissionService;
    private readonly ITaktTenantRepository<TaktUserRole> _userRoleRepository;
    private readonly ITaktTenantRepository<TaktRole> _roleRepository;
    private readonly ITaktTenantRepository<TaktRoleMenu> _roleMenuRepository;
    private readonly ITaktTenantRepository<TaktMenu> _menuRepository;
    private readonly ITaktCacheService _cacheService;
    private readonly ITaktLoginSessionService _loginSessionService;
    private readonly TaktCacheOptions _cacheOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="userCompanyRepository">用户公司关联仓储</param>
    /// <param name="userRoleRepository">用户角色关联仓储</param>
    /// <param name="roleRepository">角色仓储</param>
    /// <param name="roleMenuRepository">角色菜单关联仓储</param>
    /// <param name="menuRepository">菜单仓储</param>
    /// <param name="permissionService">权限服务</param>
    /// <param name="cacheService">缓存服务</param>
    /// <param name="loginSessionService">登录会话服务</param>
    /// <param name="cacheOptions">缓存配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAuthService(
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktCompanyRepository<TaktUserCompany> userCompanyRepository,
        ITaktTenantRepository<TaktUserRole> userRoleRepository,
        ITaktTenantRepository<TaktRole> roleRepository,
        ITaktTenantRepository<TaktRoleMenu> roleMenuRepository,
        ITaktTenantRepository<TaktMenu> menuRepository,
        ITaktPermissionService permissionService,
        ITaktCacheService cacheService,
        ITaktLoginSessionService loginSessionService,
        IOptions<TaktCacheOptions> cacheOptions,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _userCompanyRepository = userCompanyRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _roleMenuRepository = roleMenuRepository;
        _menuRepository = menuRepository;
        _permissionService = permissionService;
        _cacheService = cacheService;
        _loginSessionService = loginSessionService;
        _cacheOptions = cacheOptions.Value;
    }

    /// <summary>
    /// 获取登录页租户下拉选项（登录前；来源 Database:TenantCodes，登录后不可切换租户）
    /// </summary>
    /// <returns>租户选项列表</returns>
    public Task<List<TaktSelectOption>> GetSessionTenantOptionsAsync()
    {
        return _loginSessionService.GetLoginTenantOptionsAsync();
    }

    /// <summary>
    /// 校验登录页输入的租户编码是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>存在且启用为 true</returns>
    public Task<bool> ValidateSessionTenantCodeAsync(string tenantCode)
    {
        return _loginSessionService.ValidateLoginTenantCodeAsync(tenantCode);
    }

    /// <summary>
    /// 获取登录页语言切换选项（匿名）
    /// </summary>
    /// <param name="tenantCode">租户编码（可选）</param>
    /// <returns>语言下拉选项</returns>
    public Task<List<TaktSelectOption>> GetSessionCultureOptionsAsync(string? tenantCode = null)
    {
        return _loginSessionService.GetLoginCultureOptionsAsync(tenantCode);
    }

    /// <summary>
    /// 登录前预览：解析用户默认公司、用户 DefaultCulture 与公司 DefaultCulture（与假日无关）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">登录用户名</param>
    /// <returns>公司编码、用户/公司默认语言；解析失败时字段为空</returns>
    public async Task<TaktLoginPreviewLocaleDto> GetLoginPreviewLocaleAsync(string tenantCode, string username)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new TaktLoginPreviewLocaleDto();
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(username))
        {
            return result;
        }

        var effectiveTenant = tenantCode.Trim();
        var trimmedUsername = username.Trim();

        if (!await _loginSessionService.ValidateLoginTenantCodeAsync(effectiveTenant))
        {
            WriteAuthFlowLog(
                TaktAuthLoginPhases.LoginPreviewLocale,
                0,
                trimmedUsername,
                effectiveTenant,
                null,
                false,
                "租户不存在或未启用（TaktTenant）",
                stopwatch.ElapsedMilliseconds,
                new Dictionary<string, object?> { ["Step"] = "ValidateTenant" });
            return result;
        }

        result.TenantFound = true;

        var user = await _userRepository.FirstAsync(u =>
            u.TenantCode == effectiveTenant
            && u.Username == trimmedUsername
            && u.UserStatus == 1);
        if (user == null)
        {
            WriteAuthFlowLog(
                TaktAuthLoginPhases.LoginPreviewLocale,
                0,
                trimmedUsername,
                effectiveTenant,
                null,
                false,
                "用户不存在或已禁用（TaktUser）",
                stopwatch.ElapsedMilliseconds,
                new Dictionary<string, object?> { ["Step"] = "FindUser" });
            return result;
        }

        result.UserFound = true;
        result.DefaultCulture = (user.DefaultCulture ?? string.Empty).Trim();

        WriteAuthFlowLog(
            TaktAuthLoginPhases.LoginPreviewLocale,
            user.Id,
            trimmedUsername,
            effectiveTenant,
            null,
            true,
            "开始解析登录预览默认公司与语言",
            0,
            new Dictionary<string, object?>
            {
                ["Step"] = "Start",
                ["UserDefaultCulture"] = result.DefaultCulture,
            });

        var (companyCode, companyDefaultCulture) = await ResolveDefaultCompanyFromTablesAsync(user.Id, effectiveTenant);
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            WriteAuthFlowLog(
                TaktAuthLoginPhases.LoginPreviewLocale,
                user.Id,
                trimmedUsername,
                effectiveTenant,
                null,
                false,
                "未在 TaktUserCompany/TaktCompany 解析到默认公司",
                stopwatch.ElapsedMilliseconds,
                new Dictionary<string, object?> { ["Step"] = "ResolveDefaultCompany" });
            return result;
        }

        result.DefaultCompanyFound = true;
        result.CompanyCode = companyCode;
        result.CompanyDefaultCulture = companyDefaultCulture ?? string.Empty;

        WriteAuthFlowLog(
            TaktAuthLoginPhases.LoginPreviewLocale,
            user.Id,
            trimmedUsername,
            effectiveTenant,
            companyCode,
            true,
            "已解析用户默认语言与公司默认语言",
            stopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["Step"] = "ResolveDefaultCulture",
                ["UserDefaultCulture"] = result.DefaultCulture,
                ["CompanyDefaultCulture"] = result.CompanyDefaultCulture,
            });

        return result;
    }

    /// <summary>
    /// 按 TaktUserCompany.is_default=Yes 与 TaktCompany 启用状态解析默认公司及 DefaultCulture
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>公司编码与 DefaultCulture；无匹配时返回 (null, null)</returns>
    private async Task<(string? CompanyCode, string? DefaultCulture)> ResolveDefaultCompanyFromTablesAsync(
        long userId,
        string tenantCode)
    {
        var defaultLinkCodes = (await _userCompanyRepository.GetListAsync(
            uc => uc.TenantCode == tenantCode
                && uc.UserId == userId
                && uc.IsDefault == 1,
            uc => uc.CompanyCode,
            false))
            .Select(link => link.CompanyCode)
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (defaultLinkCodes.Count == 0)
        {
            return (null, null);
        }

        var companies = await _companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode
                && defaultLinkCodes.Contains(c.CompanyCode)
                && c.CompanyStatus == 1,
            c => c.SortOrder,
            false);

        var company = companies
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.CompanyCode, StringComparer.Ordinal)
            .FirstOrDefault();

        if (company == null)
        {
            return (null, null);
        }

        return (company.CompanyCode, (company.DefaultCulture ?? string.Empty).Trim());
    }

    /// <summary>
    /// 校验用户名在指定租户下是否具备登录权限（不校验密码）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <returns>有权限为 true</returns>
    public Task<bool> ValidateUserTenantAccessAsync(string tenantCode, string username)
    {
        return _loginSessionService.HasUserLoginAccessInTenantAsync(tenantCode, username);
    }

    /// <summary>
    /// 校验租户权限通过后验证密码
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <param name="password">明文密码</param>
    /// <returns>用户 ID；失败返回 null</returns>
    public async Task<long?> ValidateUserPasswordAsync(string tenantCode, string username, string password)
    {
        if (!await ValidateUserTenantAccessAsync(tenantCode, username))
        {
            LogWarning($"租户登录权限不足: TenantCode={tenantCode}, Username={username}");
            return null;
        }

        return await ValidateUserPasswordCoreAsync(tenantCode, username, password);
    }

    /// <summary>
    /// 仅校验密码（调用方须已单独通过租户内用户存在性校验）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>用户 ID；密码错误返回 null</returns>
    public Task<long?> ValidateUserPasswordOnlyAsync(string tenantCode, string username, string password)
    {
        return ValidateUserPasswordCoreAsync(tenantCode, username, password);
    }

    /// <summary>
    /// 校验租户权限与密码（租户权限优先，再验密）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <param name="password">明文密码</param>
    /// <returns>用户 ID；失败返回 null</returns>
    public async Task<long?> ValidateUserAsync(string tenantCode, string username, string password)
    {
        if (!await ValidateUserTenantAccessAsync(tenantCode, username))
        {
            LogWarning($"租户登录权限不足: TenantCode={tenantCode}, Username={username}");
            return null;
        }

        return await ValidateUserPasswordCoreAsync(tenantCode, username, password);
    }

    /// <summary>
    /// 校验密码（假定租户权限已通过）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名</param>
    /// <param name="password">明文密码</param>
    /// <returns>用户 ID；用户不存在、密码错误或账号禁用时返回 null</returns>
    private async Task<long?> ValidateUserPasswordCoreAsync(string tenantCode, string username, string password)
    {
        var trimmedTenant = tenantCode.Trim();
        var normalizedUsername = username.Trim().ToLowerInvariant();
        LogInformation(
            "尝试验证用户密码: TenantCode={TenantCode}, Username={Username}",
            trimmedTenant,
            normalizedUsername);

        var user = await _userRepository.FirstAsync(u =>
            u.TenantCode == trimmedTenant && u.Username == normalizedUsername);

        if (user == null)
        {
            LogWarning($"用户不存在: TenantCode={tenantCode}, Username={username}");
            return null;
        }

        if (!TaktEncryptHelper.VerifyPassword(password, user.PasswordHash))
        {
            LogWarning($"密码错误: UserId={user.Id}, Username={username}");
            return null;
        }

        if (user.UserStatus != 1)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.StatusAccountDisabled);
        }

        LogInformation("用户密码验证成功: UserId={UserId}, Username={Username}", user.Id, username);
        return user.Id;
    }

    /// <summary>
    /// 解析登录会话使用的租户与公司（用户 TaktUserCompany.is_default + TaktCompany 启用，无其它公司兜底）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">登录所选租户</param>
    /// <param name="cultureCode">区域文化编码（保留兼容；公司解析不依赖此参数）</param>
    /// <param name="requestedCompanyCode">前端指定公司（可选）</param>
    /// <returns>生效的租户与公司编码</returns>
    public async Task<(string TenantCode, string CompanyCode)> ResolveLoginTenantAndCompanyAsync(
        long userId,
        string tenantCode,
        string? cultureCode,
        string? requestedCompanyCode)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserSelf);
        }

        var effectiveTenant = string.IsNullOrWhiteSpace(tenantCode) ? user.TenantCode : tenantCode.Trim();
        _ = cultureCode;

        var candidate = !string.IsNullOrWhiteSpace(requestedCompanyCode)
            ? requestedCompanyCode.Trim()
            : await ResolveUserDefaultCompanyCodeAsync(userId, effectiveTenant, user.Username);

        if (string.IsNullOrWhiteSpace(candidate)
            || !await IsEnabledCompanyAsync(effectiveTenant, candidate))
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.Invalid, TaktValidationI18nKeys.FieldCompanyCode);
        }

        if (await _permissionService.HasCompanyAccessAsync(userId, effectiveTenant, candidate))
        {
            return (effectiveTenant, candidate);
        }

        ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.PermissionCompanyNoAccess);
        return (effectiveTenant, candidate);
    }

    /// <summary>
    /// 记录用户最后登录时间与 IP（成功登录后由认证日志处理器调用）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="loginIp">登录 IP</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task RecordUserLastLoginAsync(
        long userId,
        string tenantCode,
        string? loginIp,
        CancellationToken cancellationToken = default)
    {
        if (userId <= 0 || string.IsNullOrWhiteSpace(tenantCode))
        {
            return;
        }

        var effectiveTenant = tenantCode.Trim();
        var user = await FindUserAsync(userId, effectiveTenant);
        if (user == null)
        {
            return;
        }

        user.LastLoginAt = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(loginIp))
        {
            var trimmedIp = loginIp.Trim();
            user.LastLoginIp = trimmedIp.Length > 50 ? trimmedIp[..50] : trimmedIp;
        }

        user.LoginCount = user.LoginCount + 1;
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// 解析用户在指定租户下的默认登录公司（TaktUserCompany.is_default=Yes + TaktCompany 启用）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名（保留参数，供诊断日志扩展）</param>
    /// <returns>公司代码；无默认关联时返回 null</returns>
    public async Task<string?> ResolveUserDefaultCompanyCodeAsync(long userId, string tenantCode, string? username = null)
    {
        _ = username;
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return null;
        }

        var (companyCode, _) = await ResolveDefaultCompanyFromTablesAsync(userId, tenantCode.Trim());
        return companyCode;
    }

    /// <summary>
    /// 获取当前登录用户可切换的公司下拉选项（按数据权限过滤，ExtLabel 标记当前公司）
    /// </summary>
    /// <returns>公司选项列表；未登录返回空列表</returns>
    public async Task<List<TaktSelectOption>> GetUserCompanyOptionsAsync()
    {
        if (!IsAuthenticated || CurrentUserId == null || string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return new List<TaktSelectOption>();
        }

        var tenantCode = CurrentTenantCode;
        var userId = CurrentUserId.Value;
        var activeCompany = await ResolveCurrentActiveCompanyCodeAsync(userId, tenantCode);
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == tenantCode && x.CompanyStatus == 1,
            x => x.SortOrder,
            false);

        var codes = await _permissionService.GetAccessibleCompaniesAsync(userId, tenantCode);
        var accessibleSet = codes.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var accessible = companies
            .Where(c => accessibleSet.Contains(c.CompanyCode))
            .Select(c => c.CompanyCode)
            .ToList();

        var accessibleOrdered = companies
            .Where(c => accessible.Contains(c.CompanyCode, StringComparer.OrdinalIgnoreCase))
            .ToList();

        return accessibleOrdered
            .Select((e, index) => new TaktSelectOption
            {
                DictValue = e.CompanyCode,
                DictLabel = !string.IsNullOrWhiteSpace(e.CompanyShortName) ? e.CompanyShortName : e.CompanyName,
                ExtLabel = string.Equals(e.CompanyCode, activeCompany, StringComparison.OrdinalIgnoreCase) ? "1" : "0",
                SortOrder = index,
            })
            .ToList();
    }

    /// <summary>
    /// 判断用户是否拥有指定功能权限码
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="permissionCode">权限码</param>
    /// <returns>拥有为 true；权限码为空时视为通过</returns>
    public async Task<bool> HasUserPermissionAsync(long userId, string tenantCode, string permissionCode)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
        {
            return true;
        }

        var codes = await GetUserPermissionCodesAsync(userId, tenantCode);
        var normalizedRequired = NormalizePermissionCodeForMatch(permissionCode);
        return codes.Any(c =>
            string.Equals(
                NormalizePermissionCodeForMatch(c),
                normalizedRequired,
                StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 工作流域历史权限码与菜单种子对齐（如 flowscheme→scheme），校验前归一化。
    /// </summary>
    /// <param name="permissionCode">原始权限码</param>
    /// <returns>归一化后的权限码</returns>
    private static string NormalizePermissionCodeForMatch(string permissionCode)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
        {
            return permissionCode;
        }

        var code = permissionCode.Trim();
        const string flowSchemePrefix = "workflow:flowscheme:";
        const string flowFormPrefix = "workflow:flowform:";
        const string flowInstancePrefix = "workflow:flowinstance:";
        if (code.StartsWith(flowSchemePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return "workflow:scheme:" + code[flowSchemePrefix.Length..];
        }

        if (code.StartsWith(flowFormPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return "workflow:form:" + code[flowFormPrefix.Length..];
        }

        if (code.StartsWith(flowInstancePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return "workflow:instance:" + code[flowInstancePrefix.Length..];
        }

        return code;
    }

    /// <summary>
    /// 获取用户功能权限码列表（带缓存，来源于可访问菜单的 Permission 字段）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>权限码列表</returns>
    public async Task<List<string>> GetUserPermissionCodesAsync(long userId, string tenantCode)
    {
        var cacheKey = $"takt:perm-codes:{tenantCode}:{userId}";
        var expiration = TimeSpan.FromMinutes(_cacheOptions.DefaultExpirationMinutes);

        return await _cacheService.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var menus = await GetAccessibleMenusAsync(userId, tenantCode);
                return menus
                    .Where(m => !string.IsNullOrWhiteSpace(m.Permission))
                    .Select(m => m.Permission.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            },
            expiration);
    }

    /// <summary>
    /// 获取用户角色编码列表（用于写入 Token Claims）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>角色编码列表</returns>
    public async Task<List<string>> GetUserRoleCodesAsync(long userId, string tenantCode)
    {
        var roles = await GetEnabledRolesAsync(userId, tenantCode);
        return roles.Select(r => r.RoleCode).Distinct().ToList();
    }

    /// <summary>
    /// 获取当前登录用户资料（角色、权限、菜单树、路由路径、可访问公司）
    /// </summary>
    /// <returns>用户资料 DTO；未登录或用户不存在返回 null</returns>
    public async Task<TaktUserInfoResponseDto?> GetCurrentUserAsync()
    {
        if (!IsAuthenticated || CurrentUserId == null || string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return null;
        }

        var profileStopwatch = Stopwatch.StartNew();
        var userId = CurrentUserId.Value;
        var tenantCode = CurrentTenantCode;
        var user = await FindUserAsync(userId, tenantCode);
        if (user == null)
        {
            WriteAuthFlowLog(
                TaktAuthLoginPhases.UserProfile,
                userId,
                string.Empty,
                tenantCode,
                null,
                false,
                "用户不存在或租户不匹配",
                profileStopwatch.ElapsedMilliseconds);
            return null;
        }

        var companiesStopwatch = Stopwatch.StartNew();
        var accessibleCompanies = await GetAccessibleCompanyCodesAsync(userId, tenantCode);
        var companyCode = await ResolveCurrentActiveCompanyCodeAsync(userId, tenantCode, user.Username);
        var (companySample, companyTotal) = TaktLogFormatter.SampleForLog(accessibleCompanies, AuthLogSampleSize);
        WriteAuthFlowLog(
            TaktAuthLoginPhases.UserProfile,
            userId,
            user.Username,
            tenantCode,
            companyCode,
            true,
            "已解析可访问公司",
            companiesStopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["AccessibleCompanyCount"] = companyTotal,
                ["AccessibleCompanySample"] = companySample,
            });

        var permissionsStopwatch = Stopwatch.StartNew();
        var permissions = await GetUserPermissionCodesAsync(userId, tenantCode);
        var roles = await GetUserRoleCodesAsync(userId, tenantCode);
        var (permissionSample, permissionTotal) = TaktLogFormatter.SampleForLog(permissions, AuthLogSampleSize);
        var (roleSample, roleTotal) = TaktLogFormatter.SampleForLog(roles, AuthLogSampleSize);
        WriteAuthFlowLog(
            TaktAuthLoginPhases.UserPermissions,
            userId,
            user.Username,
            tenantCode,
            companyCode,
            true,
            "已加载角色与功能权限码",
            permissionsStopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["RoleCount"] = roleTotal,
                ["RoleSample"] = roleSample,
                ["PermissionCount"] = permissionTotal,
                ["PermissionSample"] = permissionSample,
            });

        var menusStopwatch = Stopwatch.StartNew();
        var menus = await GetUserMenuTreeAsync(userId, tenantCode);
        var menuNodeCount = CountMenuTreeNodes(menus);
        var (menuCodeSample, menuCodeTotal) = CollectMenuCodesForLog(menus);
        var (menuPathSample, menuPathTotal) = CollectRoutablePathsForLog(menus);
        WriteAuthFlowLog(
            TaktAuthLoginPhases.UserMenus,
            userId,
            user.Username,
            tenantCode,
            companyCode,
            true,
            "已构建用户菜单树",
            menusStopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["TopLevelMenuCount"] = menus.Count,
                ["MenuNodeCount"] = menuNodeCount,
                ["MenuCodeCount"] = menuCodeTotal,
                ["MenuCodeSample"] = menuCodeSample,
                ["MenuRoutePathCount"] = menuPathTotal,
                ["MenuRoutePathSample"] = menuPathSample,
            });

        var routesStopwatch = Stopwatch.StartNew();
        var routePaths = await GetUserRoutePathsAsync(userId, tenantCode);
        var (routeSample, routeTotal) = TaktLogFormatter.SampleForLog(routePaths, AuthLogSampleSize);
        WriteAuthFlowLog(
            TaktAuthLoginPhases.UserRoutes,
            userId,
            user.Username,
            tenantCode,
            companyCode,
            true,
            "已解析可访问路由路径",
            routesStopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["RoutePathCount"] = routeTotal,
                ["RoutePathSample"] = routeSample,
            });

        WriteAuthFlowLog(
            TaktAuthLoginPhases.UserProfile,
            userId,
            user.Username,
            tenantCode,
            companyCode,
            true,
            "用户资料聚合完成",
            profileStopwatch.ElapsedMilliseconds,
            new Dictionary<string, object?>
            {
                ["RoleCount"] = roleTotal,
                ["PermissionCount"] = permissionTotal,
                ["MenuNodeCount"] = menuNodeCount,
                ["RoutePathCount"] = routeTotal,
            });

        string? employeeName = null;
        int employeeGender = 0;
        string? employeeMobile = null;
        string? employeeEmail = null;
        string? avatar = null;
        if (user.EmployeeId > 0)
        {
            var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);
            if (employee != null && employee.TenantCode == tenantCode)
            {
                employeeName = employee.EmployeeName;
                employeeGender = employee.Gender;
                employeeMobile = employee.Mobile;
                employeeEmail = employee.Email;
                avatar = employee.Avatar;
            }
        }

        var companyDefaultCulture = string.Empty;
        if (!string.IsNullOrWhiteSpace(companyCode))
        {
            var activeCompany = await _companyRepository.FirstAsync(c =>
                c.TenantCode == tenantCode && c.CompanyCode == companyCode);
            companyDefaultCulture = (activeCompany?.DefaultCulture ?? string.Empty).Trim();
        }

        return new TaktUserInfoResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Nickname = user.Nickname,
            UserType = user.UserType,
            EmployeeId = user.EmployeeId,
            EmployeeName = employeeName,
            Gender = employeeGender,
            Mobile = employeeMobile,
            Email = employeeEmail,
            Avatar = avatar,
            TenantCode = user.TenantCode,
            CompanyCode = companyCode,
            DefaultCulture = (user.DefaultCulture ?? string.Empty).Trim(),
            CompanyDefaultCulture = companyDefaultCulture,
            UserStatus = (int)user.UserStatus,
            Roles = roles,
            Permissions = permissions,
            AccessibleCompanies = accessibleCompanies,
            LastLoginAt = user.LastLoginAt,
            LastLoginIp = user.LastLoginIp,
            LoginCount = user.LoginCount,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt ?? DateTime.UtcNow,
            Menus = menus,
            RoutePaths = routePaths
        };
    }

    /// <summary>
    /// 写入认证 RBAC 流程诊断日志（Application 层，无 HttpContext）
    /// </summary>
    /// <param name="phase">认证阶段标识</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="username">用户名</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="isSuccess">是否成功</param>
    /// <param name="message">说明信息</param>
    /// <param name="elapsedMs">耗时（毫秒）</param>
    /// <param name="detail">附加诊断字段</param>
    private static void WriteAuthFlowLog(
        string phase,
        long userId,
        string username,
        string tenantCode,
        string? companyCode,
        bool isSuccess,
        string message,
        long elapsedMs,
        IReadOnlyDictionary<string, object?>? detail = null)
    {
        var logContext = new TaktLogContext
        {
            Module = "identity/auth",
            Action = phase,
            UserId = userId > 0 ? userId.ToString() : null,
            Username = string.IsNullOrWhiteSpace(username) ? null : username,
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            Extra = detail == null
                ? new Dictionary<string, object?> { ["Message"] = message, ["ElapsedMs"] = elapsedMs }
                : new Dictionary<string, object?>(detail)
                {
                    ["Message"] = message,
                    ["ElapsedMs"] = elapsedMs,
                },
        };

        using (TaktLogger.BeginScope(logContext))
        {
            if (isSuccess)
            {
                TaktLogger.Information(
                    logContext,
                    "认证流程 [{Phase}] 成功: 用户={Username}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    phase,
                    username,
                    tenantCode,
                    message,
                    elapsedMs);
            }
            else
            {
                TaktLogger.Warning(
                    logContext,
                    "认证流程 [{Phase}] 失败: 用户={Username}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    phase,
                    username,
                    tenantCode,
                    message,
                    elapsedMs);
            }
        }
    }

    /// <summary>
    /// 获取用户可访问的前端路由路径（菜单类型为目录/页面且 RoutePath 非空）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>路由路径列表</returns>
    public async Task<List<string>> GetUserRoutePathsAsync(long userId, string tenantCode)
    {
        var menus = await GetAccessibleMenusAsync(userId, tenantCode);
        return menus
            .Where(m => m.MenuType == (int)1 && !string.IsNullOrWhiteSpace(m.RoutePath))
            .Select(m => NormalizeRoutePath(m.RoutePath))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 按用户 ID 与租户编码查询用户
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>用户实体；不存在时返回 null</returns>
    private async Task<TaktUser?> FindUserAsync(long userId, string tenantCode)
    {
        return await _userRepository.FirstAsync(u => u.Id == userId && u.TenantCode == tenantCode);
    }

    /// <summary>
    /// 获取用户在指定租户下已启用的角色列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>已启用角色列表</returns>
    private async Task<List<TaktRole>> GetEnabledRolesAsync(long userId, string tenantCode)
    {
        var userRoles = await _userRoleRepository.GetListAsync(ur =>
            ur.UserId == userId && ur.TenantCode == tenantCode);
        if (userRoles.Count == 0)
        {
            return new List<TaktRole>();
        }

        var roleIds = userRoles.Select(ur => ur.RoleId).Distinct().ToList();
        var roles = await _roleRepository.GetListAsync(r =>
            roleIds.Contains(r.Id) && r.TenantCode == tenantCode);
        return roles
            .Where(r => r.RoleStatus == 1)
            .ToList();
    }

    /// <summary>
    /// 获取用户通过角色可访问的菜单列表（已启用菜单，不含按钮）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>可访问菜单列表</returns>
    private async Task<List<TaktMenu>> GetAccessibleMenusAsync(long userId, string tenantCode)
    {
        var allMenus = await _menuRepository.GetListAsync(m =>
            m.TenantCode == tenantCode && m.MenuStatus == 1);

        var roles = await GetEnabledRolesAsync(userId, tenantCode);
        if (roles.Count == 0)
        {
            return new List<TaktMenu>();
        }

        var roleIds = roles.Select(r => r.Id).ToList();
        var roleMenus = await _roleMenuRepository.GetListAsync(rm =>
            rm.TenantCode == tenantCode && roleIds.Contains(rm.RoleId));
        var menuIds = roleMenus.Select(rm => rm.MenuId).Distinct().ToHashSet();
        return allMenus.Where(m => menuIds.Contains(m.Id)).ToList();
    }

    /// <summary>
    /// 构建用户可访问的菜单树（排除按钮类型）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>菜单树 DTO 列表</returns>
    private async Task<List<TaktMenuTreeDto>> GetUserMenuTreeAsync(long userId, string tenantCode)
    {
        var menus = await GetAccessibleMenusAsync(userId, tenantCode);
        // 与 TaktMenuType 及种子数据一致：0=目录、1=页面菜单、2=按钮（按钮不参与侧栏树）
        var treeSource = menus.Where(m => m.MenuType != (int)2).ToList();
        return BuildMenuTree(treeSource, 0);
    }

    /// <summary>
    /// 递归构建菜单树
    /// </summary>
    /// <param name="allRecords">全部菜单记录</param>
    /// <param name="parentId">父级菜单 ID</param>
    /// <returns>指定父级下的菜单树节点</returns>
    private List<TaktMenuTreeDto> BuildMenuTree(List<TaktMenu> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.SortOrder)
            .ToList();

        var treeList = new List<TaktMenuTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktMenuTreeDto>();
            var childTree = BuildMenuTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }

            treeList.Add(treeDto);
        }

        return treeList;
    }

    /// <summary>
    /// 获取用户在指定租户下可访问的公司编码列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>可访问公司编码列表</returns>
    private async Task<List<string>> GetAccessibleCompanyCodesAsync(long userId, string tenantCode)
    {
        return await _permissionService.GetAccessibleCompaniesAsync(userId, tenantCode);
    }

    /// <summary>
    /// 解析当前会话生效公司（请求头 X-Company-Code 优先，否则 UserCompany.is_default）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="username">用户名（保留供诊断扩展）</param>
    /// <returns>生效公司编码；无可用公司时返回空字符串</returns>
    public async Task<string> ResolveCurrentActiveCompanyCodeAsync(
        long userId,
        string tenantCode,
        string? username = null)
    {
        var accessibleCompanies = await GetAccessibleCompanyCodesAsync(userId, tenantCode);
        var defaultCompanyCode = await ResolveUserDefaultCompanyCodeAsync(userId, tenantCode, username);
        return ResolveActiveCompanyCode(
            accessibleCompanies,
            _userContext?.RequestCompanyCode,
            defaultCompanyCode);
    }

    /// <summary>
    /// 判断指定租户下公司是否存在且已启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>存在且启用为 true</returns>
    private async Task<bool> IsEnabledCompanyAsync(string tenantCode, string companyCode)
    {
        var company = await _companyRepository.FirstAsync(c =>
            c.TenantCode == tenantCode
            && c.CompanyCode == companyCode
            && c.CompanyStatus == 1);
        return company != null;
    }

    /// <summary>
    /// 按可访问公司、请求头公司与默认公司解析当前生效公司编码
    /// </summary>
    /// <param name="accessibleCompanies">用户可访问公司列表</param>
    /// <param name="requestedCompanyCode">请求头指定公司编码</param>
    /// <param name="fallbackCompanyCode">用户默认公司编码</param>
    /// <returns>生效公司编码；无可用公司时返回空字符串</returns>
    private static string ResolveActiveCompanyCode(
        IReadOnlyList<string> accessibleCompanies,
        string? requestedCompanyCode,
        string? fallbackCompanyCode = null)
    {
        if (accessibleCompanies.Count == 0)
        {
            return string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(requestedCompanyCode)
            && accessibleCompanies.Contains(requestedCompanyCode, StringComparer.OrdinalIgnoreCase))
        {
            return requestedCompanyCode.Trim();
        }

        if (!string.IsNullOrWhiteSpace(fallbackCompanyCode)
            && accessibleCompanies.Contains(fallbackCompanyCode, StringComparer.OrdinalIgnoreCase))
        {
            return fallbackCompanyCode.Trim();
        }

        return accessibleCompanies[0];
    }

    /// <summary>
    /// 规范化前端路由路径（补前导斜杠并去除尾部斜杠）
    /// </summary>
    /// <param name="routePath">原始路由路径</param>
    /// <returns>规范化后的路由路径</returns>
    private static string NormalizeRoutePath(string routePath)
    {
        var path = routePath.Trim();
        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return path.TrimEnd('/');
    }

    /// <summary>
    /// 统计菜单树节点数（含子级，用于 RBAC 诊断日志）
    /// </summary>
    /// <param name="menus">菜单树</param>
    /// <returns>节点总数</returns>
    private static int CountMenuTreeNodes(IReadOnlyList<TaktMenuTreeDto> menus)
    {
        var count = 0;

        void Walk(IReadOnlyList<TaktMenuTreeDto> nodes)
        {
            foreach (var node in nodes)
            {
                count++;
                if (node.Children is { Count: > 0 })
                {
                    Walk(node.Children);
                }
            }
        }

        Walk(menus);
        return count;
    }

    /// <summary>
    /// 收集菜单编码采样（先根遍历，用于 RBAC 诊断日志）
    /// </summary>
    /// <param name="menus">菜单树</param>
    /// <returns>菜单编码采样与总数</returns>
    private static (IReadOnlyList<string> Sample, int Total) CollectMenuCodesForLog(
        IReadOnlyList<TaktMenuTreeDto> menus)
    {
        var all = new List<string>();

        void Walk(IReadOnlyList<TaktMenuTreeDto> nodes)
        {
            foreach (var node in nodes)
            {
                if (!string.IsNullOrWhiteSpace(node.MenuCode))
                {
                    all.Add(node.MenuCode);
                }

                if (node.Children is { Count: > 0 })
                {
                    Walk(node.Children);
                }
            }
        }

        Walk(menus);
        return TaktLogFormatter.SampleForLog(all, AuthLogSampleSize);
    }

    /// <summary>
    /// 收集可导航页面路由路径采样（用于 RBAC 诊断日志）
    /// </summary>
    /// <param name="menus">菜单树</param>
    /// <returns>路由路径采样与总数</returns>
    private static (IReadOnlyList<string> Sample, int Total) CollectRoutablePathsForLog(
        IReadOnlyList<TaktMenuTreeDto> menus)
    {
        var paths = new List<string>();

        void Walk(IReadOnlyList<TaktMenuTreeDto> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.MenuType == (int)1 && !string.IsNullOrWhiteSpace(node.RoutePath))
                {
                    paths.Add(node.RoutePath.Trim());
                }

                if (node.Children is { Count: > 0 })
                {
                    Walk(node.Children);
                }
            }
        }

        Walk(menus);
        return TaktLogFormatter.SampleForLog(paths, AuthLogSampleSize);
    }
}
