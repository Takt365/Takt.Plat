// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktOnlineI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktOnline 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktOnline 实体国际化翻译种子（键前缀 entity.online.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktOnlineI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktOnline 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 online 实体翻译...", tenantCode);

        foreach (var item in GetOnlineTranslations())
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

        TaktLogger.Information("TaktOnline 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktOnline 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.online._self / entity.online.{{field}}；ResourceGroup=8；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetOnlineTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.online._self
            new TranslationSeedItem("entity.online._self", "en-US", "Online Information", "实体名称"),
            // entity.online._self
            new TranslationSeedItem("entity.online._self", "ja-JP", "在线用户信息", "实体名称"),
            // entity.online._self
            new TranslationSeedItem("entity.online._self", "zh-CN", "在线用户信息", "实体名称"),
            // entity.online._self
            new TranslationSeedItem("entity.online._self", "zh-HK", "在线用户信息", "实体名称"),

            // entity.online.connectionid
            new TranslationSeedItem("entity.online.connectionid", "en-US", "SignalR连接ID", "SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）"),
            // entity.online.connectionid
            new TranslationSeedItem("entity.online.connectionid", "ja-JP", "SignalR连接ID", "SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）"),
            // entity.online.connectionid
            new TranslationSeedItem("entity.online.connectionid", "zh-CN", "SignalR连接ID", "SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）"),
            // entity.online.connectionid
            new TranslationSeedItem("entity.online.connectionid", "zh-HK", "SignalR连接ID", "SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）"),

            // entity.online.username
            new TranslationSeedItem("entity.online.username", "en-US", "用户名", "用户名"),
            // entity.online.username
            new TranslationSeedItem("entity.online.username", "ja-JP", "用户名", "用户名"),
            // entity.online.username
            new TranslationSeedItem("entity.online.username", "zh-CN", "用户名", "用户名"),
            // entity.online.username
            new TranslationSeedItem("entity.online.username", "zh-HK", "用户名", "用户名"),

            // entity.online.userid
            new TranslationSeedItem("entity.online.userid", "en-US", "用户ID", "用户 ID"),
            // entity.online.userid
            new TranslationSeedItem("entity.online.userid", "ja-JP", "用户ID", "用户 ID"),
            // entity.online.userid
            new TranslationSeedItem("entity.online.userid", "zh-CN", "用户ID", "用户 ID"),
            // entity.online.userid
            new TranslationSeedItem("entity.online.userid", "zh-HK", "用户ID", "用户 ID"),

            // entity.online.status
            new TranslationSeedItem("entity.online.status", "en-US", "在线状态", "在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）"),
            // entity.online.status
            new TranslationSeedItem("entity.online.status", "ja-JP", "在线状态", "在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）"),
            // entity.online.status
            new TranslationSeedItem("entity.online.status", "zh-CN", "在线状态", "在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）"),
            // entity.online.status
            new TranslationSeedItem("entity.online.status", "zh-HK", "在线状态", "在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）"),

            // entity.online.connectip
            new TranslationSeedItem("entity.online.connectip", "en-US", "连接IP地址", "连接 IP 地址"),
            // entity.online.connectip
            new TranslationSeedItem("entity.online.connectip", "ja-JP", "连接IP地址", "连接 IP 地址"),
            // entity.online.connectip
            new TranslationSeedItem("entity.online.connectip", "zh-CN", "连接IP地址", "连接 IP 地址"),
            // entity.online.connectip
            new TranslationSeedItem("entity.online.connectip", "zh-HK", "连接IP地址", "连接 IP 地址"),

            // entity.online.connectlocation
            new TranslationSeedItem("entity.online.connectlocation", "en-US", "连接地点", "连接地点"),
            // entity.online.connectlocation
            new TranslationSeedItem("entity.online.connectlocation", "ja-JP", "连接地点", "连接地点"),
            // entity.online.connectlocation
            new TranslationSeedItem("entity.online.connectlocation", "zh-CN", "连接地点", "连接地点"),
            // entity.online.connectlocation
            new TranslationSeedItem("entity.online.connectlocation", "zh-HK", "连接地点", "连接地点"),

            // entity.online.useragent
            new TranslationSeedItem("entity.online.useragent", "en-US", "User-Agent", "User-Agent"),
            // entity.online.useragent
            new TranslationSeedItem("entity.online.useragent", "ja-JP", "User-Agent", "User-Agent"),
            // entity.online.useragent
            new TranslationSeedItem("entity.online.useragent", "zh-CN", "User-Agent", "User-Agent"),
            // entity.online.useragent
            new TranslationSeedItem("entity.online.useragent", "zh-HK", "User-Agent", "User-Agent"),

            // entity.online.devicetype
            new TranslationSeedItem("entity.online.devicetype", "en-US", "设备类型", "设备类型"),
            // entity.online.devicetype
            new TranslationSeedItem("entity.online.devicetype", "ja-JP", "设备类型", "设备类型"),
            // entity.online.devicetype
            new TranslationSeedItem("entity.online.devicetype", "zh-CN", "设备类型", "设备类型"),
            // entity.online.devicetype
            new TranslationSeedItem("entity.online.devicetype", "zh-HK", "设备类型", "设备类型"),

            // entity.online.browsertype
            new TranslationSeedItem("entity.online.browsertype", "en-US", "浏览器类型", "浏览器类型（User-Agent 解析；0=未知 1=Chrome 2=Firefox 3=Safari 4=Edge）"),
            // entity.online.browsertype
            new TranslationSeedItem("entity.online.browsertype", "ja-JP", "浏览器类型", "浏览器类型（User-Agent 解析；0=未知 1=Chrome 2=Firefox 3=Safari 4=Edge）"),
            // entity.online.browsertype
            new TranslationSeedItem("entity.online.browsertype", "zh-CN", "浏览器类型", "浏览器类型（User-Agent 解析；0=未知 1=Chrome 2=Firefox 3=Safari 4=Edge）"),
            // entity.online.browsertype
            new TranslationSeedItem("entity.online.browsertype", "zh-HK", "浏览器类型", "浏览器类型（User-Agent 解析；0=未知 1=Chrome 2=Firefox 3=Safari 4=Edge）"),

            // entity.online.operatingsystem
            new TranslationSeedItem("entity.online.operatingsystem", "en-US", "操作系统", "操作系统（User-Agent 解析；0=未知 1=Windows 2=macOS 3=Linux 4=Android 5=iOS）"),
            // entity.online.operatingsystem
            new TranslationSeedItem("entity.online.operatingsystem", "ja-JP", "操作系统", "操作系统（User-Agent 解析；0=未知 1=Windows 2=macOS 3=Linux 4=Android 5=iOS）"),
            // entity.online.operatingsystem
            new TranslationSeedItem("entity.online.operatingsystem", "zh-CN", "操作系统", "操作系统（User-Agent 解析；0=未知 1=Windows 2=macOS 3=Linux 4=Android 5=iOS）"),
            // entity.online.operatingsystem
            new TranslationSeedItem("entity.online.operatingsystem", "zh-HK", "操作系统", "操作系统（User-Agent 解析；0=未知 1=Windows 2=macOS 3=Linux 4=Android 5=iOS）"),

            // entity.online.connecttime
            new TranslationSeedItem("entity.online.connecttime", "en-US", "连接时间", "连接时间"),
            // entity.online.connecttime
            new TranslationSeedItem("entity.online.connecttime", "ja-JP", "连接时间", "连接时间"),
            // entity.online.connecttime
            new TranslationSeedItem("entity.online.connecttime", "zh-CN", "连接时间", "连接时间"),
            // entity.online.connecttime
            new TranslationSeedItem("entity.online.connecttime", "zh-HK", "连接时间", "连接时间"),

            // entity.online.lastactivetime
            new TranslationSeedItem("entity.online.lastactivetime", "en-US", "最后活动时间", "最后活动时间"),
            // entity.online.lastactivetime
            new TranslationSeedItem("entity.online.lastactivetime", "ja-JP", "最后活动时间", "最后活动时间"),
            // entity.online.lastactivetime
            new TranslationSeedItem("entity.online.lastactivetime", "zh-CN", "最后活动时间", "最后活动时间"),
            // entity.online.lastactivetime
            new TranslationSeedItem("entity.online.lastactivetime", "zh-HK", "最后活动时间", "最后活动时间"),

            // entity.online.disconnecttime
            new TranslationSeedItem("entity.online.disconnecttime", "en-US", "断开时间", "断开时间"),
            // entity.online.disconnecttime
            new TranslationSeedItem("entity.online.disconnecttime", "ja-JP", "断开时间", "断开时间"),
            // entity.online.disconnecttime
            new TranslationSeedItem("entity.online.disconnecttime", "zh-CN", "断开时间", "断开时间"),
            // entity.online.disconnecttime
            new TranslationSeedItem("entity.online.disconnecttime", "zh-HK", "断开时间", "断开时间"),

            // entity.online.connectionduration
            new TranslationSeedItem("entity.online.connectionduration", "en-US", "连接时长", "连接时长（秒）"),
            // entity.online.connectionduration
            new TranslationSeedItem("entity.online.connectionduration", "ja-JP", "连接时长", "连接时长（秒）"),
            // entity.online.connectionduration
            new TranslationSeedItem("entity.online.connectionduration", "zh-CN", "连接时长", "连接时长（秒）"),
            // entity.online.connectionduration
            new TranslationSeedItem("entity.online.connectionduration", "zh-HK", "连接时长", "连接时长（秒）"),
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
        translation.ResourceGroup = 8;
        translation.ResourceType = 0;
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
