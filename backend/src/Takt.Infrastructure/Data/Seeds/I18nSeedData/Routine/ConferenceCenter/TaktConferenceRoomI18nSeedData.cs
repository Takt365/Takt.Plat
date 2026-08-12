// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.ConferenceCenter
// 文件名称：TaktConferenceRoomI18nSeedData.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConferenceRoom 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktConferenceRoom 实体国际化翻译种子（键前缀 entity.conferenceroom.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConferenceRoomI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConferenceRoom 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 conferenceroom 实体翻译...", tenantCode);

        foreach (var item in GetConferenceRoomTranslations())
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

        TaktLogger.Information("TaktConferenceRoom 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConferenceRoom 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.conferenceroom._self / entity.conferenceroom.{{field}}；ResourceGroup=ConferenceCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConferenceRoomTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.conferenceroom._self
            new TranslationSeedItem("entity.conferenceroom._self", "en-US", "Conference Room Information_us", "实体名称"),
            // entity.conferenceroom._self
            new TranslationSeedItem("entity.conferenceroom._self", "ja-JP", "会议室信息_jp", "实体名称"),
            // entity.conferenceroom._self
            new TranslationSeedItem("entity.conferenceroom._self", "zh-CN", "会议室信息", "实体名称"),
            // entity.conferenceroom._self
            new TranslationSeedItem("entity.conferenceroom._self", "zh-HK", "会议室信息_hk", "实体名称"),

            // entity.conferenceroom.roomcode
            new TranslationSeedItem("entity.conferenceroom.roomcode", "en-US", "会议室编码_us", "会议室编码（租户+公司内唯一）"),
            // entity.conferenceroom.roomcode
            new TranslationSeedItem("entity.conferenceroom.roomcode", "ja-JP", "会议室编码_jp", "会议室编码（租户+公司内唯一）"),
            // entity.conferenceroom.roomcode
            new TranslationSeedItem("entity.conferenceroom.roomcode", "zh-CN", "会议室编码", "会议室编码（租户+公司内唯一）"),
            // entity.conferenceroom.roomcode
            new TranslationSeedItem("entity.conferenceroom.roomcode", "zh-HK", "会议室编码_hk", "会议室编码（租户+公司内唯一）"),

            // entity.conferenceroom.roomname
            new TranslationSeedItem("entity.conferenceroom.roomname", "en-US", "会议室名称_us", "会议室名称"),
            // entity.conferenceroom.roomname
            new TranslationSeedItem("entity.conferenceroom.roomname", "ja-JP", "会议室名称_jp", "会议室名称"),
            // entity.conferenceroom.roomname
            new TranslationSeedItem("entity.conferenceroom.roomname", "zh-CN", "会议室名称", "会议室名称"),
            // entity.conferenceroom.roomname
            new TranslationSeedItem("entity.conferenceroom.roomname", "zh-HK", "会议室名称_hk", "会议室名称"),

            // entity.conferenceroom.building
            new TranslationSeedItem("entity.conferenceroom.building", "en-US", "楼栋_us", "楼栋/建筑"),
            // entity.conferenceroom.building
            new TranslationSeedItem("entity.conferenceroom.building", "ja-JP", "楼栋_jp", "楼栋/建筑"),
            // entity.conferenceroom.building
            new TranslationSeedItem("entity.conferenceroom.building", "zh-CN", "楼栋", "楼栋/建筑"),
            // entity.conferenceroom.building
            new TranslationSeedItem("entity.conferenceroom.building", "zh-HK", "楼栋_hk", "楼栋/建筑"),

            // entity.conferenceroom.floor
            new TranslationSeedItem("entity.conferenceroom.floor", "en-US", "楼层_us", "楼层"),
            // entity.conferenceroom.floor
            new TranslationSeedItem("entity.conferenceroom.floor", "ja-JP", "楼层_jp", "楼层"),
            // entity.conferenceroom.floor
            new TranslationSeedItem("entity.conferenceroom.floor", "zh-CN", "楼层", "楼层"),
            // entity.conferenceroom.floor
            new TranslationSeedItem("entity.conferenceroom.floor", "zh-HK", "楼层_hk", "楼层"),

            // entity.conferenceroom.locationdetail
            new TranslationSeedItem("entity.conferenceroom.locationdetail", "en-US", "详细位置_us", "详细位置说明"),
            // entity.conferenceroom.locationdetail
            new TranslationSeedItem("entity.conferenceroom.locationdetail", "ja-JP", "详细位置_jp", "详细位置说明"),
            // entity.conferenceroom.locationdetail
            new TranslationSeedItem("entity.conferenceroom.locationdetail", "zh-CN", "详细位置", "详细位置说明"),
            // entity.conferenceroom.locationdetail
            new TranslationSeedItem("entity.conferenceroom.locationdetail", "zh-HK", "详细位置_hk", "详细位置说明"),

            // entity.conferenceroom.capacity
            new TranslationSeedItem("entity.conferenceroom.capacity", "en-US", "容纳人数_us", "容纳人数（0 表示不限）"),
            // entity.conferenceroom.capacity
            new TranslationSeedItem("entity.conferenceroom.capacity", "ja-JP", "容纳人数_jp", "容纳人数（0 表示不限）"),
            // entity.conferenceroom.capacity
            new TranslationSeedItem("entity.conferenceroom.capacity", "zh-CN", "容纳人数", "容纳人数（0 表示不限）"),
            // entity.conferenceroom.capacity
            new TranslationSeedItem("entity.conferenceroom.capacity", "zh-HK", "容纳人数_hk", "容纳人数（0 表示不限）"),

            // entity.conferenceroom.facilities
            new TranslationSeedItem("entity.conferenceroom.facilities", "en-US", "设施说明_us", "设施说明（投影、视频会议设备等）"),
            // entity.conferenceroom.facilities
            new TranslationSeedItem("entity.conferenceroom.facilities", "ja-JP", "设施说明_jp", "设施说明（投影、视频会议设备等）"),
            // entity.conferenceroom.facilities
            new TranslationSeedItem("entity.conferenceroom.facilities", "zh-CN", "设施说明", "设施说明（投影、视频会议设备等）"),
            // entity.conferenceroom.facilities
            new TranslationSeedItem("entity.conferenceroom.facilities", "zh-HK", "设施说明_hk", "设施说明（投影、视频会议设备等）"),

            // entity.conferenceroom.sortorder
            new TranslationSeedItem("entity.conferenceroom.sortorder", "en-US", "排序号_us", "排序号"),
            // entity.conferenceroom.sortorder
            new TranslationSeedItem("entity.conferenceroom.sortorder", "ja-JP", "排序号_jp", "排序号"),
            // entity.conferenceroom.sortorder
            new TranslationSeedItem("entity.conferenceroom.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.conferenceroom.sortorder
            new TranslationSeedItem("entity.conferenceroom.sortorder", "zh-HK", "排序号_hk", "排序号"),

            // entity.conferenceroom.roomstatus
            new TranslationSeedItem("entity.conferenceroom.roomstatus", "en-US", "会议室状态_us", "会议室状态（字典 routine_conference_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.conferenceroom.roomstatus
            new TranslationSeedItem("entity.conferenceroom.roomstatus", "ja-JP", "会议室状态_jp", "会议室状态（字典 routine_conference_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.conferenceroom.roomstatus
            new TranslationSeedItem("entity.conferenceroom.roomstatus", "zh-CN", "会议室状态", "会议室状态（字典 routine_conference_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
            // entity.conferenceroom.roomstatus
            new TranslationSeedItem("entity.conferenceroom.roomstatus", "zh-HK", "会议室状态_hk", "会议室状态（字典 routine_conference_room_status；0=可用 1=使用中 2=维护中 3=停用）"),
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
        translation.ResourceGroup = "ConferenceCenter";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
