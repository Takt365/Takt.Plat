// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipantI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConferenceParticipant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter;

/// <summary>
/// TaktConferenceParticipant 实体国际化翻译种子（键前缀 entity.conferenceparticipant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConferenceParticipantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConferenceParticipant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 conferenceparticipant 实体翻译...", tenantCode);

        foreach (var item in GetConferenceParticipantTranslations())
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

        TaktLogger.Information("TaktConferenceParticipant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConferenceParticipant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.conferenceparticipant._self / entity.conferenceparticipant.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetConferenceParticipantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.conferenceparticipant._self
            new TranslationSeedItem("entity.conferenceparticipant._self", "en-US", "Conference Participant Information", "实体名称"),
            // entity.conferenceparticipant._self
            new TranslationSeedItem("entity.conferenceparticipant._self", "ja-JP", "会议参与人子信息", "实体名称"),
            // entity.conferenceparticipant._self
            new TranslationSeedItem("entity.conferenceparticipant._self", "zh-CN", "会议参与人子信息", "实体名称"),
            // entity.conferenceparticipant._self
            new TranslationSeedItem("entity.conferenceparticipant._self", "zh-HK", "会议参与人子信息", "实体名称"),

            // entity.conferenceparticipant.conferenceid
            new TranslationSeedItem("entity.conferenceparticipant.conferenceid", "en-US", "会议ID", "会议 ID"),
            // entity.conferenceparticipant.conferenceid
            new TranslationSeedItem("entity.conferenceparticipant.conferenceid", "ja-JP", "会议ID", "会议 ID"),
            // entity.conferenceparticipant.conferenceid
            new TranslationSeedItem("entity.conferenceparticipant.conferenceid", "zh-CN", "会议ID", "会议 ID"),
            // entity.conferenceparticipant.conferenceid
            new TranslationSeedItem("entity.conferenceparticipant.conferenceid", "zh-HK", "会议ID", "会议 ID"),

            // entity.conferenceparticipant.userid
            new TranslationSeedItem("entity.conferenceparticipant.userid", "en-US", "用户ID", "用户 ID"),
            // entity.conferenceparticipant.userid
            new TranslationSeedItem("entity.conferenceparticipant.userid", "ja-JP", "用户ID", "用户 ID"),
            // entity.conferenceparticipant.userid
            new TranslationSeedItem("entity.conferenceparticipant.userid", "zh-CN", "用户ID", "用户 ID"),
            // entity.conferenceparticipant.userid
            new TranslationSeedItem("entity.conferenceparticipant.userid", "zh-HK", "用户ID", "用户 ID"),

            // entity.conferenceparticipant.username
            new TranslationSeedItem("entity.conferenceparticipant.username", "en-US", "用户姓名", "用户姓名"),
            // entity.conferenceparticipant.username
            new TranslationSeedItem("entity.conferenceparticipant.username", "ja-JP", "用户姓名", "用户姓名"),
            // entity.conferenceparticipant.username
            new TranslationSeedItem("entity.conferenceparticipant.username", "zh-CN", "用户姓名", "用户姓名"),
            // entity.conferenceparticipant.username
            new TranslationSeedItem("entity.conferenceparticipant.username", "zh-HK", "用户姓名", "用户姓名"),

            // entity.conferenceparticipant.participantrole
            new TranslationSeedItem("entity.conferenceparticipant.participantrole", "en-US", "参与角色", "参与角色"),
            // entity.conferenceparticipant.participantrole
            new TranslationSeedItem("entity.conferenceparticipant.participantrole", "ja-JP", "参与角色", "参与角色"),
            // entity.conferenceparticipant.participantrole
            new TranslationSeedItem("entity.conferenceparticipant.participantrole", "zh-CN", "参与角色", "参与角色"),
            // entity.conferenceparticipant.participantrole
            new TranslationSeedItem("entity.conferenceparticipant.participantrole", "zh-HK", "参与角色", "参与角色"),

            // entity.conferenceparticipant.attendancestatus
            new TranslationSeedItem("entity.conferenceparticipant.attendancestatus", "en-US", "出席状态", "出席状态"),
            // entity.conferenceparticipant.attendancestatus
            new TranslationSeedItem("entity.conferenceparticipant.attendancestatus", "ja-JP", "出席状态", "出席状态"),
            // entity.conferenceparticipant.attendancestatus
            new TranslationSeedItem("entity.conferenceparticipant.attendancestatus", "zh-CN", "出席状态", "出席状态"),
            // entity.conferenceparticipant.attendancestatus
            new TranslationSeedItem("entity.conferenceparticipant.attendancestatus", "zh-HK", "出席状态", "出席状态"),

            // entity.conferenceparticipant.checkintime
            new TranslationSeedItem("entity.conferenceparticipant.checkintime", "en-US", "签到时间", "签到时间"),
            // entity.conferenceparticipant.checkintime
            new TranslationSeedItem("entity.conferenceparticipant.checkintime", "ja-JP", "签到时间", "签到时间"),
            // entity.conferenceparticipant.checkintime
            new TranslationSeedItem("entity.conferenceparticipant.checkintime", "zh-CN", "签到时间", "签到时间"),
            // entity.conferenceparticipant.checkintime
            new TranslationSeedItem("entity.conferenceparticipant.checkintime", "zh-HK", "签到时间", "签到时间"),

            // entity.conferenceparticipant.checkouttime
            new TranslationSeedItem("entity.conferenceparticipant.checkouttime", "en-US", "签退时间", "签退时间"),
            // entity.conferenceparticipant.checkouttime
            new TranslationSeedItem("entity.conferenceparticipant.checkouttime", "ja-JP", "签退时间", "签退时间"),
            // entity.conferenceparticipant.checkouttime
            new TranslationSeedItem("entity.conferenceparticipant.checkouttime", "zh-CN", "签退时间", "签退时间"),
            // entity.conferenceparticipant.checkouttime
            new TranslationSeedItem("entity.conferenceparticipant.checkouttime", "zh-HK", "签退时间", "签退时间"),

            // entity.conferenceparticipant.checkinmethod
            new TranslationSeedItem("entity.conferenceparticipant.checkinmethod", "en-US", "签到方式", "签到方式（0=手动，1=扫码，2=人脸等）"),
            // entity.conferenceparticipant.checkinmethod
            new TranslationSeedItem("entity.conferenceparticipant.checkinmethod", "ja-JP", "签到方式", "签到方式（0=手动，1=扫码，2=人脸等）"),
            // entity.conferenceparticipant.checkinmethod
            new TranslationSeedItem("entity.conferenceparticipant.checkinmethod", "zh-CN", "签到方式", "签到方式（0=手动，1=扫码，2=人脸等）"),
            // entity.conferenceparticipant.checkinmethod
            new TranslationSeedItem("entity.conferenceparticipant.checkinmethod", "zh-HK", "签到方式", "签到方式（0=手动，1=扫码，2=人脸等）"),

            // entity.conferenceparticipant.conference
            new TranslationSeedItem("entity.conferenceparticipant.conference", "en-US", "会议", "会议（主表）"),
            // entity.conferenceparticipant.conference
            new TranslationSeedItem("entity.conferenceparticipant.conference", "ja-JP", "会议", "会议（主表）"),
            // entity.conferenceparticipant.conference
            new TranslationSeedItem("entity.conferenceparticipant.conference", "zh-CN", "会议", "会议（主表）"),
            // entity.conferenceparticipant.conference
            new TranslationSeedItem("entity.conferenceparticipant.conference", "zh-HK", "会议", "会议（主表）"),
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
        translation.ResourceGroup = 2;
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
