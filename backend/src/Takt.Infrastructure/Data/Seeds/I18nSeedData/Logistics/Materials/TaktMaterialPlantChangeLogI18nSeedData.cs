// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialPlantChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialPlantChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterialPlantChangeLog 实体国际化翻译种子（键前缀 entity.materialplantchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialPlantChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialPlantChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialplantchangelog 实体翻译...", tenantCode);

        foreach (var item in GetMaterialPlantChangeLogTranslations())
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

        TaktLogger.Information("TaktMaterialPlantChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialPlantChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialplantchangelog._self / entity.materialplantchangelog.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialPlantChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialplantchangelog._self
            new TranslationSeedItem("entity.materialplantchangelog._self", "en-US", "Material Plant Change Log Information_us", "实体名称"),
            // entity.materialplantchangelog._self
            new TranslationSeedItem("entity.materialplantchangelog._self", "ja-JP", "工厂物料变更记录信息_jp", "实体名称"),
            // entity.materialplantchangelog._self
            new TranslationSeedItem("entity.materialplantchangelog._self", "zh-CN", "工厂物料变更记录信息", "实体名称"),
            // entity.materialplantchangelog._self
            new TranslationSeedItem("entity.materialplantchangelog._self", "zh-HK", "工厂物料变更记录信息_hk", "实体名称"),

            // entity.materialplantchangelog.materialplantid
            new TranslationSeedItem("entity.materialplantchangelog.materialplantid", "en-US", "工厂物料ID_us", "工厂物料ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialplantchangelog.materialplantid
            new TranslationSeedItem("entity.materialplantchangelog.materialplantid", "ja-JP", "工厂物料ID_jp", "工厂物料ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialplantchangelog.materialplantid
            new TranslationSeedItem("entity.materialplantchangelog.materialplantid", "zh-CN", "工厂物料ID", "工厂物料ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.materialplantchangelog.materialplantid
            new TranslationSeedItem("entity.materialplantchangelog.materialplantid", "zh-HK", "工厂物料ID_hk", "工厂物料ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.materialplantchangelog.materialcode
            new TranslationSeedItem("entity.materialplantchangelog.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.materialplantchangelog.materialcode
            new TranslationSeedItem("entity.materialplantchangelog.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.materialplantchangelog.materialcode
            new TranslationSeedItem("entity.materialplantchangelog.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.materialplantchangelog.materialcode
            new TranslationSeedItem("entity.materialplantchangelog.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.materialplantchangelog.plantcode
            new TranslationSeedItem("entity.materialplantchangelog.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.materialplantchangelog.plantcode
            new TranslationSeedItem("entity.materialplantchangelog.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.materialplantchangelog.plantcode
            new TranslationSeedItem("entity.materialplantchangelog.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.materialplantchangelog.plantcode
            new TranslationSeedItem("entity.materialplantchangelog.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.materialplantchangelog.changefields
            new TranslationSeedItem("entity.materialplantchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.materialplantchangelog.changefields
            new TranslationSeedItem("entity.materialplantchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.materialplantchangelog.changefields
            new TranslationSeedItem("entity.materialplantchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.materialplantchangelog.changefields
            new TranslationSeedItem("entity.materialplantchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),

            // entity.materialplantchangelog.changetime
            new TranslationSeedItem("entity.materialplantchangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.materialplantchangelog.changetime
            new TranslationSeedItem("entity.materialplantchangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.materialplantchangelog.changetime
            new TranslationSeedItem("entity.materialplantchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.materialplantchangelog.changetime
            new TranslationSeedItem("entity.materialplantchangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.materialplantchangelog.changeby
            new TranslationSeedItem("entity.materialplantchangelog.changeby", "en-US", "变更人_us", "变更人（人员代码）"),
            // entity.materialplantchangelog.changeby
            new TranslationSeedItem("entity.materialplantchangelog.changeby", "ja-JP", "变更人_jp", "变更人（人员代码）"),
            // entity.materialplantchangelog.changeby
            new TranslationSeedItem("entity.materialplantchangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.materialplantchangelog.changeby
            new TranslationSeedItem("entity.materialplantchangelog.changeby", "zh-HK", "变更人_hk", "变更人（人员代码）"),

            // entity.materialplantchangelog.changereason
            new TranslationSeedItem("entity.materialplantchangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.materialplantchangelog.changereason
            new TranslationSeedItem("entity.materialplantchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.materialplantchangelog.changereason
            new TranslationSeedItem("entity.materialplantchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.materialplantchangelog.changereason
            new TranslationSeedItem("entity.materialplantchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.materialplantchangelog.materialplant
            new TranslationSeedItem("entity.materialplantchangelog.materialplant", "en-US", "工厂物料主表_us", "工厂物料主表"),
            // entity.materialplantchangelog.materialplant
            new TranslationSeedItem("entity.materialplantchangelog.materialplant", "ja-JP", "工厂物料主表_jp", "工厂物料主表"),
            // entity.materialplantchangelog.materialplant
            new TranslationSeedItem("entity.materialplantchangelog.materialplant", "zh-CN", "工厂物料主表", "工厂物料主表"),
            // entity.materialplantchangelog.materialplant
            new TranslationSeedItem("entity.materialplantchangelog.materialplant", "zh-HK", "工厂物料主表_hk", "工厂物料主表"),
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
        translation.ResourceGroup = "Materials";
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
