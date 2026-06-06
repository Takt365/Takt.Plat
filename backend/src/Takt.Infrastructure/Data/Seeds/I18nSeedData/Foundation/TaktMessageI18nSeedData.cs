// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktMessageI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMessage 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktMessage 实体国际化翻译种子（键前缀 entity.message.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMessageI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMessage 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 message 实体翻译...", tenantCode);

        foreach (var item in GetMessageTranslations())
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

        TaktLogger.Information("TaktMessage 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMessage 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.message._self / entity.message.{{field}}；ResourceGroup=TaktModule.Foundation；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMessageTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.message._self
            new TranslationSeedItem("entity.message._self", "en-US", "Message Information", "实体名称"),
            // entity.message._self
            new TranslationSeedItem("entity.message._self", "ja-JP", "在线消息信息", "实体名称"),
            // entity.message._self
            new TranslationSeedItem("entity.message._self", "zh-CN", "在线消息信息", "实体名称"),
            // entity.message._self
            new TranslationSeedItem("entity.message._self", "zh-HK", "在线消息信息", "实体名称"),

            // entity.message.fromusername
            new TranslationSeedItem("entity.message.fromusername", "en-US", "发送者用户名", "发送者用户名"),
            // entity.message.fromusername
            new TranslationSeedItem("entity.message.fromusername", "ja-JP", "发送者用户名", "发送者用户名"),
            // entity.message.fromusername
            new TranslationSeedItem("entity.message.fromusername", "zh-CN", "发送者用户名", "发送者用户名"),
            // entity.message.fromusername
            new TranslationSeedItem("entity.message.fromusername", "zh-HK", "发送者用户名", "发送者用户名"),

            // entity.message.fromuserid
            new TranslationSeedItem("entity.message.fromuserid", "en-US", "发送者用户ID", "发送者用户 ID"),
            // entity.message.fromuserid
            new TranslationSeedItem("entity.message.fromuserid", "ja-JP", "发送者用户ID", "发送者用户 ID"),
            // entity.message.fromuserid
            new TranslationSeedItem("entity.message.fromuserid", "zh-CN", "发送者用户ID", "发送者用户 ID"),
            // entity.message.fromuserid
            new TranslationSeedItem("entity.message.fromuserid", "zh-HK", "发送者用户ID", "发送者用户 ID"),

            // entity.message.tousername
            new TranslationSeedItem("entity.message.tousername", "en-US", "接收者用户名", "接收者用户名"),
            // entity.message.tousername
            new TranslationSeedItem("entity.message.tousername", "ja-JP", "接收者用户名", "接收者用户名"),
            // entity.message.tousername
            new TranslationSeedItem("entity.message.tousername", "zh-CN", "接收者用户名", "接收者用户名"),
            // entity.message.tousername
            new TranslationSeedItem("entity.message.tousername", "zh-HK", "接收者用户名", "接收者用户名"),

            // entity.message.touserid
            new TranslationSeedItem("entity.message.touserid", "en-US", "接收者用户ID", "接收者用户 ID"),
            // entity.message.touserid
            new TranslationSeedItem("entity.message.touserid", "ja-JP", "接收者用户ID", "接收者用户 ID"),
            // entity.message.touserid
            new TranslationSeedItem("entity.message.touserid", "zh-CN", "接收者用户ID", "接收者用户 ID"),
            // entity.message.touserid
            new TranslationSeedItem("entity.message.touserid", "zh-HK", "接收者用户ID", "接收者用户 ID"),

            // entity.message.title
            new TranslationSeedItem("entity.message.title", "en-US", "消息标题", "消息标题"),
            // entity.message.title
            new TranslationSeedItem("entity.message.title", "ja-JP", "消息标题", "消息标题"),
            // entity.message.title
            new TranslationSeedItem("entity.message.title", "zh-CN", "消息标题", "消息标题"),
            // entity.message.title
            new TranslationSeedItem("entity.message.title", "zh-HK", "消息标题", "消息标题"),

            // entity.message.content
            new TranslationSeedItem("entity.message.content", "en-US", "消息内容", "消息内容"),
            // entity.message.content
            new TranslationSeedItem("entity.message.content", "ja-JP", "消息内容", "消息内容"),
            // entity.message.content
            new TranslationSeedItem("entity.message.content", "zh-CN", "消息内容", "消息内容"),
            // entity.message.content
            new TranslationSeedItem("entity.message.content", "zh-HK", "消息内容", "消息内容"),

            // entity.message.type
            new TranslationSeedItem("entity.message.type", "en-US", "消息类型", "消息类型"),
            // entity.message.type
            new TranslationSeedItem("entity.message.type", "ja-JP", "消息类型", "消息类型"),
            // entity.message.type
            new TranslationSeedItem("entity.message.type", "zh-CN", "消息类型", "消息类型"),
            // entity.message.type
            new TranslationSeedItem("entity.message.type", "zh-HK", "消息类型", "消息类型"),

            // entity.message.readstatus
            new TranslationSeedItem("entity.message.readstatus", "en-US", "读取状态", "读取状态"),
            // entity.message.readstatus
            new TranslationSeedItem("entity.message.readstatus", "ja-JP", "读取状态", "读取状态"),
            // entity.message.readstatus
            new TranslationSeedItem("entity.message.readstatus", "zh-CN", "读取状态", "读取状态"),
            // entity.message.readstatus
            new TranslationSeedItem("entity.message.readstatus", "zh-HK", "读取状态", "读取状态"),

            // entity.message.readtime
            new TranslationSeedItem("entity.message.readtime", "en-US", "读取时间", "读取时间"),
            // entity.message.readtime
            new TranslationSeedItem("entity.message.readtime", "ja-JP", "读取时间", "读取时间"),
            // entity.message.readtime
            new TranslationSeedItem("entity.message.readtime", "zh-CN", "读取时间", "读取时间"),
            // entity.message.readtime
            new TranslationSeedItem("entity.message.readtime", "zh-HK", "读取时间", "读取时间"),

            // entity.message.sendtime
            new TranslationSeedItem("entity.message.sendtime", "en-US", "发送时间", "发送时间"),
            // entity.message.sendtime
            new TranslationSeedItem("entity.message.sendtime", "ja-JP", "发送时间", "发送时间"),
            // entity.message.sendtime
            new TranslationSeedItem("entity.message.sendtime", "zh-CN", "发送时间", "发送时间"),
            // entity.message.sendtime
            new TranslationSeedItem("entity.message.sendtime", "zh-HK", "发送时间", "发送时间"),

            // entity.message.extdata
            new TranslationSeedItem("entity.message.extdata", "en-US", "消息扩展数据", "消息扩展数据（JSON）"),
            // entity.message.extdata
            new TranslationSeedItem("entity.message.extdata", "ja-JP", "消息扩展数据", "消息扩展数据（JSON）"),
            // entity.message.extdata
            new TranslationSeedItem("entity.message.extdata", "zh-CN", "消息扩展数据", "消息扩展数据（JSON）"),
            // entity.message.extdata
            new TranslationSeedItem("entity.message.extdata", "zh-HK", "消息扩展数据", "消息扩展数据（JSON）"),
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
        translation.ResourceGroup = TaktModule.Foundation;
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
