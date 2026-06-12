// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingChangeLogI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRoutingChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktRoutingChangeLog 实体国际化翻译种子（键前缀 entity.routingchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoutingChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRoutingChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 routingchangelog 实体翻译...", tenantCode);

        foreach (var item in GetRoutingChangeLogTranslations())
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

        TaktLogger.Information("TaktRoutingChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRoutingChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.routingchangelog._self / entity.routingchangelog.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetRoutingChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.routingchangelog._self
            new TranslationSeedItem("entity.routingchangelog._self", "en-US", "Routing Change Log Information", "实体名称"),
            // entity.routingchangelog._self
            new TranslationSeedItem("entity.routingchangelog._self", "ja-JP", "工艺路线变更日志", "实体名称"),
            // entity.routingchangelog._self
            new TranslationSeedItem("entity.routingchangelog._self", "zh-CN", "工艺路线变更日志", "实体名称"),
            // entity.routingchangelog._self
            new TranslationSeedItem("entity.routingchangelog._self", "zh-HK", "工艺路线变更日志", "实体名称"),

            // entity.routingchangelog.routingid
            new TranslationSeedItem("entity.routingchangelog.routingid", "en-US", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingchangelog.routingid
            new TranslationSeedItem("entity.routingchangelog.routingid", "ja-JP", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingchangelog.routingid
            new TranslationSeedItem("entity.routingchangelog.routingid", "zh-CN", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.routingchangelog.routingid
            new TranslationSeedItem("entity.routingchangelog.routingid", "zh-HK", "工艺路线ID", "工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.routingchangelog.changefields
            new TranslationSeedItem("entity.routingchangelog.changefields", "en-US", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.routingchangelog.changefields
            new TranslationSeedItem("entity.routingchangelog.changefields", "ja-JP", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.routingchangelog.changefields
            new TranslationSeedItem("entity.routingchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.routingchangelog.changefields
            new TranslationSeedItem("entity.routingchangelog.changefields", "zh-HK", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),

            // entity.routingchangelog.changetype
            new TranslationSeedItem("entity.routingchangelog.changetype", "en-US", "变更类型", "变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）"),
            // entity.routingchangelog.changetype
            new TranslationSeedItem("entity.routingchangelog.changetype", "ja-JP", "变更类型", "变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）"),
            // entity.routingchangelog.changetype
            new TranslationSeedItem("entity.routingchangelog.changetype", "zh-CN", "变更类型", "变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）"),
            // entity.routingchangelog.changetype
            new TranslationSeedItem("entity.routingchangelog.changetype", "zh-HK", "变更类型", "变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）"),

            // entity.routingchangelog.changereason
            new TranslationSeedItem("entity.routingchangelog.changereason", "en-US", "变更原因", "变更原因"),
            // entity.routingchangelog.changereason
            new TranslationSeedItem("entity.routingchangelog.changereason", "ja-JP", "变更原因", "变更原因"),
            // entity.routingchangelog.changereason
            new TranslationSeedItem("entity.routingchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.routingchangelog.changereason
            new TranslationSeedItem("entity.routingchangelog.changereason", "zh-HK", "变更原因", "变更原因"),

            // entity.routingchangelog.changeby
            new TranslationSeedItem("entity.routingchangelog.changeby", "en-US", "变更人", "变更人（人员代码）"),
            // entity.routingchangelog.changeby
            new TranslationSeedItem("entity.routingchangelog.changeby", "ja-JP", "变更人", "变更人（人员代码）"),
            // entity.routingchangelog.changeby
            new TranslationSeedItem("entity.routingchangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.routingchangelog.changeby
            new TranslationSeedItem("entity.routingchangelog.changeby", "zh-HK", "变更人", "变更人（人员代码）"),

            // entity.routingchangelog.changetime
            new TranslationSeedItem("entity.routingchangelog.changetime", "en-US", "变更时间", "变更时间"),
            // entity.routingchangelog.changetime
            new TranslationSeedItem("entity.routingchangelog.changetime", "ja-JP", "变更时间", "变更时间"),
            // entity.routingchangelog.changetime
            new TranslationSeedItem("entity.routingchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.routingchangelog.changetime
            new TranslationSeedItem("entity.routingchangelog.changetime", "zh-HK", "变更时间", "变更时间"),

            // entity.routingchangelog.routing
            new TranslationSeedItem("entity.routingchangelog.routing", "en-US", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingchangelog.routing
            new TranslationSeedItem("entity.routingchangelog.routing", "ja-JP", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingchangelog.routing
            new TranslationSeedItem("entity.routingchangelog.routing", "zh-CN", "工艺路线主表", "工艺路线主表（主表）"),
            // entity.routingchangelog.routing
            new TranslationSeedItem("entity.routingchangelog.routing", "zh-HK", "工艺路线主表", "工艺路线主表（主表）"),
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
        translation.ResourceGroup = 4;
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
