// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktItAssetChangeLogI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktItAssetChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktItAssetChangeLog 实体国际化翻译种子（键前缀 entity.itassetchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktItAssetChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktItAssetChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 itassetchangelog 实体翻译...", tenantCode);

        foreach (var item in GetItAssetChangeLogTranslations())
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

        TaktLogger.Information("TaktItAssetChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktItAssetChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.itassetchangelog._self / entity.itassetchangelog.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetItAssetChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.itassetchangelog._self
            new TranslationSeedItem("entity.itassetchangelog._self", "en-US", "It Asset Change Log Information_us", "实体名称"),
            // entity.itassetchangelog._self
            new TranslationSeedItem("entity.itassetchangelog._self", "ja-JP", "IT 设备保修扩展变更日志信息_jp", "实体名称"),
            // entity.itassetchangelog._self
            new TranslationSeedItem("entity.itassetchangelog._self", "zh-CN", "IT 设备保修扩展变更日志信息", "实体名称"),
            // entity.itassetchangelog._self
            new TranslationSeedItem("entity.itassetchangelog._self", "zh-HK", "IT 设备保修扩展变更日志信息_hk", "实体名称"),

            // entity.itassetchangelog.itassetid
            new TranslationSeedItem("entity.itassetchangelog.itassetid", "en-US", "IT设备保修扩展ID_us", "IT 设备保修扩展 ID（关联 TaktItAsset.Id，选项 TaktItAssets/options）"),
            // entity.itassetchangelog.itassetid
            new TranslationSeedItem("entity.itassetchangelog.itassetid", "ja-JP", "IT设备保修扩展ID_jp", "IT 设备保修扩展 ID（关联 TaktItAsset.Id，选项 TaktItAssets/options）"),
            // entity.itassetchangelog.itassetid
            new TranslationSeedItem("entity.itassetchangelog.itassetid", "zh-CN", "IT设备保修扩展ID", "IT 设备保修扩展 ID（关联 TaktItAsset.Id，选项 TaktItAssets/options）"),
            // entity.itassetchangelog.itassetid
            new TranslationSeedItem("entity.itassetchangelog.itassetid", "zh-HK", "IT设备保修扩展ID_hk", "IT 设备保修扩展 ID（关联 TaktItAsset.Id，选项 TaktItAssets/options）"),

            // entity.itassetchangelog.assetcode
            new TranslationSeedItem("entity.itassetchangelog.assetcode", "en-US", "资产号码_us", "资产号码（冗余）"),
            // entity.itassetchangelog.assetcode
            new TranslationSeedItem("entity.itassetchangelog.assetcode", "ja-JP", "资产号码_jp", "资产号码（冗余）"),
            // entity.itassetchangelog.assetcode
            new TranslationSeedItem("entity.itassetchangelog.assetcode", "zh-CN", "资产号码", "资产号码（冗余）"),
            // entity.itassetchangelog.assetcode
            new TranslationSeedItem("entity.itassetchangelog.assetcode", "zh-HK", "资产号码_hk", "资产号码（冗余）"),

            // entity.itassetchangelog.changetype
            new TranslationSeedItem("entity.itassetchangelog.changetype", "en-US", "变更类型_us", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.itassetchangelog.changetype
            new TranslationSeedItem("entity.itassetchangelog.changetype", "ja-JP", "变更类型_jp", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.itassetchangelog.changetype
            new TranslationSeedItem("entity.itassetchangelog.changetype", "zh-CN", "变更类型", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.itassetchangelog.changetype
            new TranslationSeedItem("entity.itassetchangelog.changetype", "zh-HK", "变更类型_hk", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),

            // entity.itassetchangelog.changesummary
            new TranslationSeedItem("entity.itassetchangelog.changesummary", "en-US", "修改内容摘要_us", "修改内容摘要"),
            // entity.itassetchangelog.changesummary
            new TranslationSeedItem("entity.itassetchangelog.changesummary", "ja-JP", "修改内容摘要_jp", "修改内容摘要"),
            // entity.itassetchangelog.changesummary
            new TranslationSeedItem("entity.itassetchangelog.changesummary", "zh-CN", "修改内容摘要", "修改内容摘要"),
            // entity.itassetchangelog.changesummary
            new TranslationSeedItem("entity.itassetchangelog.changesummary", "zh-HK", "修改内容摘要_hk", "修改内容摘要"),

            // entity.itassetchangelog.changefields
            new TranslationSeedItem("entity.itassetchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON 数组）"),
            // entity.itassetchangelog.changefields
            new TranslationSeedItem("entity.itassetchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON 数组）"),
            // entity.itassetchangelog.changefields
            new TranslationSeedItem("entity.itassetchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.itassetchangelog.changefields
            new TranslationSeedItem("entity.itassetchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON 数组）"),

            // entity.itassetchangelog.changereason
            new TranslationSeedItem("entity.itassetchangelog.changereason", "en-US", "变更原因_us", "变更原因或备注"),
            // entity.itassetchangelog.changereason
            new TranslationSeedItem("entity.itassetchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因或备注"),
            // entity.itassetchangelog.changereason
            new TranslationSeedItem("entity.itassetchangelog.changereason", "zh-CN", "变更原因", "变更原因或备注"),
            // entity.itassetchangelog.changereason
            new TranslationSeedItem("entity.itassetchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因或备注"),

            // entity.itassetchangelog.itasset
            new TranslationSeedItem("entity.itassetchangelog.itasset", "en-US", "IT 设备保修扩展_us", "IT 设备保修扩展（主表）"),
            // entity.itassetchangelog.itasset
            new TranslationSeedItem("entity.itassetchangelog.itasset", "ja-JP", "IT 设备保修扩展_jp", "IT 设备保修扩展（主表）"),
            // entity.itassetchangelog.itasset
            new TranslationSeedItem("entity.itassetchangelog.itasset", "zh-CN", "IT 设备保修扩展", "IT 设备保修扩展（主表）"),
            // entity.itassetchangelog.itasset
            new TranslationSeedItem("entity.itassetchangelog.itasset", "zh-HK", "IT 设备保修扩展_hk", "IT 设备保修扩展（主表）"),
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
        translation.ResourceGroup = "HelpDesk";
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
