// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktOpenIddictSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict种子数据（Authorization Code + PKCE 公共客户端）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Takt.Shared.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// OpenIddict种子数据初始化
/// </summary>
public class TaktOpenIddictSeedData
{
    /// <summary>
    /// 初始化OpenIddict数据（应用、范围等）
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var openIddictOptions = serviceProvider.GetRequiredService<IOptions<TaktOpenIddictOptions>>().Value;

        await CreateScopesAsync(scopeManager, openIddictOptions.ApiAudience);
        await CreateApplicationsAsync(applicationManager, openIddictOptions);
    }

    private static async Task CreateScopesAsync(IOpenIddictScopeManager scopeManager, string apiAudience)
    {
        await CreateScopeIfMissingAsync(scopeManager, Scopes.OpenId, "OpenID Connect", "OpenID Connect 标准范围", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.Profile, "用户资料", "访问用户基本资料", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.Email, "电子邮箱", "访问用户电子邮箱", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.OfflineAccess, "离线访问", "允许刷新令牌", apiAudience);
    }

    private static async Task CreateScopeIfMissingAsync(
        IOpenIddictScopeManager scopeManager,
        string name,
        string displayName,
        string description,
        string resource)
    {
        if (await scopeManager.FindByNameAsync(name) != null)
        {
            return;
        }

        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayName,
            Description = description,
            Resources = { resource }
        });
    }

    private static async Task CreateApplicationsAsync(
        IOpenIddictApplicationManager applicationManager,
        TaktOpenIddictOptions options)
    {
        await CreateOrUpdateSpaClientAsync(applicationManager, options);
        await CreateApiClientIfMissingAsync(applicationManager, options.ApiAudience);
    }

    /// <summary>
    /// SPA 公共客户端：Authorization Code + PKCE（强制）
    /// </summary>
    private static async Task CreateOrUpdateSpaClientAsync(
        IOpenIddictApplicationManager applicationManager,
        TaktOpenIddictOptions options)
    {
        var clientId = options.SpaClientId;
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Implicit,
            DisplayName = "Takt SPA",
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.Endpoints.EndSession,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Prefixes.Scope + Scopes.OpenId,
                Permissions.Prefixes.Scope + Scopes.Profile,
                Permissions.Prefixes.Scope + Scopes.Email,
                Permissions.Prefixes.Scope + Scopes.OfflineAccess
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        };

        foreach (var uri in options.SpaRedirectUris)
        {
            descriptor.RedirectUris.Add(new Uri(uri));
        }

        foreach (var uri in options.SpaPostLogoutRedirectUris)
        {
            descriptor.PostLogoutRedirectUris.Add(new Uri(uri));
        }

        var existing = await applicationManager.FindByClientIdAsync(clientId);
        if (existing == null)
        {
            await applicationManager.CreateAsync(descriptor);
            return;
        }

        await applicationManager.UpdateAsync(existing, descriptor);
    }

    /// <summary>
    /// 机密客户端：Client Credentials（服务间调用）
    /// </summary>
    private static async Task CreateApiClientIfMissingAsync(
        IOpenIddictApplicationManager applicationManager,
        string apiAudience)
    {
        const string clientId = "takt-api-client";
        if (await applicationManager.FindByClientIdAsync(clientId) != null)
        {
            return;
        }

        await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
            ClientType = ClientTypes.Confidential,
            DisplayName = "Takt API Client",
            Permissions =
            {
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.ClientCredentials,
                Permissions.Prefixes.Scope + Scopes.OpenId,
                Permissions.Prefixes.Scope + Scopes.Profile
            }
        });
    }
}
