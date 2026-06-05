// ========================================
// 椤圭洰鍚嶇О锛氳妭鎷嶅伐鍘偮稵akt Plat
// 鍛藉悕绌洪棿锛歍akt.Infrastructure.Extensions
// 鏂囦欢鍚嶇О锛歍aktServiceCollectionExtensions.cs
// 鍒涘缓鏃堕棿锛?025-01-20
// 鍒涘缓浜猴細Takt365(Cursor AI)
// 鍔熻兘鎻忚堪锛氫笟鍔℃湇鍔℃敞鍐屾墿灞曟柟娉曪紙鐢ㄦ埛涓婁笅鏂囥€佹湰鍦板寲銆丆ORS绛夛級
// 
// 鐗堟潈淇℃伅锛欳opyright (c) 2025 Takt  All rights reserved.
// 鍏嶈矗澹版槑锛氭杞欢浣跨敤 MIT License锛屼綔鑰呬笉鎵挎媴浠讳綍浣跨敤椋庨櫓銆?
// ========================================

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Options;
using CorsSettings = Takt.Shared.Options.CorsSettings;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 涓氬姟鏈嶅姟娉ㄥ唽鎵╁睍鏂规硶
/// </summary>
public static class TaktServiceCollectionExtensions
{
    /// <summary>
    /// 娣诲姞 Takt 涓氬姟鏈嶅姟锛堢敤鎴蜂笂涓嬫枃銆佹湰鍦板寲銆丆ORS绛夛級
    /// </summary>
    /// <param name="services">鏈嶅姟闆嗗悎</param>
    /// <param name="configuration">閰嶇疆</param>
    /// <returns>鏈嶅姟闆嗗悎</returns>
    public static IServiceCollection AddTaktServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 娉ㄥ唽 HTTP 涓婁笅鏂囪闂櫒锛堢敤浜庤幏鍙栧綋鍓嶇敤鎴蜂俊鎭級
        services.AddHttpContextAccessor();

        // 娉ㄥ唽鐢ㄦ埛涓婁笅鏂囨湇鍔★紙鏀寔绉嶅瓙鏁版嵁闃舵锛?
        services.AddScoped<ITaktUserContext>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            // 濡傛灉鏈?HTTP 涓婁笅鏂囷紝浣跨敤 TaktUserContext
            if (httpContextAccessor.HttpContext != null)
            {
                return ActivatorUtilities.CreateInstance<TaktUserContext>(sp);
            }

            // 鍚﹀垯浣跨敤 TaktSeedUserContext锛堢瀛愭暟鎹樁娈碉級
            var configuration = sp.GetRequiredService<IConfiguration>();
            return TaktSeedUserContext.Create(configuration.RequireDatabase().GetSeedTenantCode());
        });

        // 娉ㄥ唽鏈湴鍖栨湇鍔★紙榛樿 en-US锛涜姹傚ご Accept-Language 涓庡墠绔?locale 鍚屾锛?
        var localizationOptions = configuration.RequireOptions<TaktLocalizationOptions>(TaktLocalizationOptions.SectionName);
        localizationOptions.Validate();

        services.Configure<TaktLocalizationOptions>(configuration.GetSection(TaktLocalizationOptions.SectionName));

        services.AddLocalization(options =>
        {
            options.ResourcesPath = localizationOptions.ResourcesPath;
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(localizationOptions.DefaultCulture);
            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider(),
            ];
        });

        services.AddScoped<ITaktLocalizationService, TaktLocalizationService>();

        var corsSettings = configuration.RequireOptions<CorsSettings>(CorsSettings.SectionName);
        corsSettings.Validate();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsSettings.AllowedOrigins)
                      .WithMethods(corsSettings.AllowedMethods)
                      .WithHeaders(corsSettings.AllowedHeaders);

                if (corsSettings.AllowCredentials)
                {
                    policy.AllowCredentials();
                }
            });
        });

        return services;
    }
}
