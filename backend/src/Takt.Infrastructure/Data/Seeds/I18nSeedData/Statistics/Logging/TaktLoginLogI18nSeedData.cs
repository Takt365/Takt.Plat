// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktLoginLogI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktLoginLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktLoginLog 实体国际化翻译种子（键前缀 entity.loginLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktLoginLogI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktLoginLog 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 loginLog 实体翻译...", tenantCode);

        foreach (var item in GetLoginLogTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktLoginLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktLoginLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.loginLog._self / entity.loginLog.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetLoginLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.loginLog._self
            new TranslationSeedItem("entity.loginLog._self", "en-US", "Login Log Information", "实体名称"),
            // entity.loginLog._self
            new TranslationSeedItem("entity.loginLog._self", "ja-JP", "登录日志信息", "实体名称"),
            // entity.loginLog._self
            new TranslationSeedItem("entity.loginLog._self", "zh-CN", "登录日志信息", "实体名称"),
            // entity.loginLog._self
            new TranslationSeedItem("entity.loginLog._self", "zh-HK", "登录日志信息", "实体名称"),

            // entity.loginLog.username
            new TranslationSeedItem("entity.loginLog.username", "en-US", "用户名", "用户名（登录账号）"),
            // entity.loginLog.username
            new TranslationSeedItem("entity.loginLog.username", "ja-JP", "用户名", "用户名（登录账号）"),
            // entity.loginLog.username
            new TranslationSeedItem("entity.loginLog.username", "zh-CN", "用户名", "用户名（登录账号）"),
            // entity.loginLog.username
            new TranslationSeedItem("entity.loginLog.username", "zh-HK", "用户名", "用户名（登录账号）"),

            // entity.loginLog.logintype
            new TranslationSeedItem("entity.loginLog.logintype", "en-US", "登录方式", "登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）"),
            // entity.loginLog.logintype
            new TranslationSeedItem("entity.loginLog.logintype", "ja-JP", "登录方式", "登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）"),
            // entity.loginLog.logintype
            new TranslationSeedItem("entity.loginLog.logintype", "zh-CN", "登录方式", "登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）"),
            // entity.loginLog.logintype
            new TranslationSeedItem("entity.loginLog.logintype", "zh-HK", "登录方式", "登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）"),

            // entity.loginLog.browser
            new TranslationSeedItem("entity.loginLog.browser", "en-US", "浏览器类型", "浏览器类型"),
            // entity.loginLog.browser
            new TranslationSeedItem("entity.loginLog.browser", "ja-JP", "浏览器类型", "浏览器类型"),
            // entity.loginLog.browser
            new TranslationSeedItem("entity.loginLog.browser", "zh-CN", "浏览器类型", "浏览器类型"),
            // entity.loginLog.browser
            new TranslationSeedItem("entity.loginLog.browser", "zh-HK", "浏览器类型", "浏览器类型"),

            // entity.loginLog.os
            new TranslationSeedItem("entity.loginLog.os", "en-US", "操作系统", "操作系统"),
            // entity.loginLog.os
            new TranslationSeedItem("entity.loginLog.os", "ja-JP", "操作系统", "操作系统"),
            // entity.loginLog.os
            new TranslationSeedItem("entity.loginLog.os", "zh-CN", "操作系统", "操作系统"),
            // entity.loginLog.os
            new TranslationSeedItem("entity.loginLog.os", "zh-HK", "操作系统", "操作系统"),

            // entity.loginLog.useragent
            new TranslationSeedItem("entity.loginLog.useragent", "en-US", "用户代理字符串", "用户代理字符串（User-Agent）"),
            // entity.loginLog.useragent
            new TranslationSeedItem("entity.loginLog.useragent", "ja-JP", "用户代理字符串", "用户代理字符串（User-Agent）"),
            // entity.loginLog.useragent
            new TranslationSeedItem("entity.loginLog.useragent", "zh-CN", "用户代理字符串", "用户代理字符串（User-Agent）"),
            // entity.loginLog.useragent
            new TranslationSeedItem("entity.loginLog.useragent", "zh-HK", "用户代理字符串", "用户代理字符串（User-Agent）"),

            // entity.loginLog.loginresult
            new TranslationSeedItem("entity.loginLog.loginresult", "en-US", "登录结果", "登录结果"),
            // entity.loginLog.loginresult
            new TranslationSeedItem("entity.loginLog.loginresult", "ja-JP", "登录结果", "登录结果"),
            // entity.loginLog.loginresult
            new TranslationSeedItem("entity.loginLog.loginresult", "zh-CN", "登录结果", "登录结果"),
            // entity.loginLog.loginresult
            new TranslationSeedItem("entity.loginLog.loginresult", "zh-HK", "登录结果", "登录结果"),

            // entity.loginLog.loginmessage
            new TranslationSeedItem("entity.loginLog.loginmessage", "en-US", "登录结果消息", "登录结果消息"),
            // entity.loginLog.loginmessage
            new TranslationSeedItem("entity.loginLog.loginmessage", "ja-JP", "登录结果消息", "登录结果消息"),
            // entity.loginLog.loginmessage
            new TranslationSeedItem("entity.loginLog.loginmessage", "zh-CN", "登录结果消息", "登录结果消息"),
            // entity.loginLog.loginmessage
            new TranslationSeedItem("entity.loginLog.loginmessage", "zh-HK", "登录结果消息", "登录结果消息"),

            // entity.loginLog.loginip
            new TranslationSeedItem("entity.loginLog.loginip", "en-US", "登录IP地址", "登录IP地址"),
            // entity.loginLog.loginip
            new TranslationSeedItem("entity.loginLog.loginip", "ja-JP", "登录IP地址", "登录IP地址"),
            // entity.loginLog.loginip
            new TranslationSeedItem("entity.loginLog.loginip", "zh-CN", "登录IP地址", "登录IP地址"),
            // entity.loginLog.loginip
            new TranslationSeedItem("entity.loginLog.loginip", "zh-HK", "登录IP地址", "登录IP地址"),

            // entity.loginLog.loginlocation
            new TranslationSeedItem("entity.loginLog.loginlocation", "en-US", "登录地点", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginLog.loginlocation
            new TranslationSeedItem("entity.loginLog.loginlocation", "ja-JP", "登录地点", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginLog.loginlocation
            new TranslationSeedItem("entity.loginLog.loginlocation", "zh-CN", "登录地点", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginLog.loginlocation
            new TranslationSeedItem("entity.loginLog.loginlocation", "zh-HK", "登录地点", "登录地点（IP解析，如：中国-广东省-深圳市）"),

            // entity.loginLog.logoutat
            new TranslationSeedItem("entity.loginLog.logoutat", "en-US", "登出时间", "登出时间"),
            // entity.loginLog.logoutat
            new TranslationSeedItem("entity.loginLog.logoutat", "ja-JP", "登出时间", "登出时间"),
            // entity.loginLog.logoutat
            new TranslationSeedItem("entity.loginLog.logoutat", "zh-CN", "登出时间", "登出时间"),
            // entity.loginLog.logoutat
            new TranslationSeedItem("entity.loginLog.logoutat", "zh-HK", "登出时间", "登出时间"),

            // entity.loginLog.sessionduration
            new TranslationSeedItem("entity.loginLog.sessionduration", "en-US", "会话时长", "会话时长（秒，从登录到登出的时长）"),
            // entity.loginLog.sessionduration
            new TranslationSeedItem("entity.loginLog.sessionduration", "ja-JP", "会话时长", "会话时长（秒，从登录到登出的时长）"),
            // entity.loginLog.sessionduration
            new TranslationSeedItem("entity.loginLog.sessionduration", "zh-CN", "会话时长", "会话时长（秒，从登录到登出的时长）"),
            // entity.loginLog.sessionduration
            new TranslationSeedItem("entity.loginLog.sessionduration", "zh-HK", "会话时长", "会话时长（秒，从登录到登出的时长）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = TaktModule.Statistics;
        translation.ResourceType = TaktAppSide.Frontend;
        translation.ContextNote = item.ContextNote;
        translation.ExtFieldJson = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
