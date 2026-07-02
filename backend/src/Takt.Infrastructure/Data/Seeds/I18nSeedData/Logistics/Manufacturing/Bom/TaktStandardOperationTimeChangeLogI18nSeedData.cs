// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeChangeLogI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardOperationTimeChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktStandardOperationTimeChangeLog 实体国际化翻译种子（键前缀 entity.standardoperationtimechangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardOperationTimeChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardOperationTimeChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardoperationtimechangelog 实体翻译...", tenantCode);

        foreach (var item in GetStandardOperationTimeChangeLogTranslations())
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

        TaktLogger.Information("TaktStandardOperationTimeChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardOperationTimeChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardoperationtimechangelog._self / entity.standardoperationtimechangelog.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardOperationTimeChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardoperationtimechangelog._self
            new TranslationSeedItem("entity.standardoperationtimechangelog._self", "en-US", "Standard Operation Time Change Log Information_us", "实体名称"),
            // entity.standardoperationtimechangelog._self
            new TranslationSeedItem("entity.standardoperationtimechangelog._self", "ja-JP", "标准工序时间变更记录信息_jp", "实体名称"),
            // entity.standardoperationtimechangelog._self
            new TranslationSeedItem("entity.standardoperationtimechangelog._self", "zh-CN", "标准工序时间变更记录信息", "实体名称"),
            // entity.standardoperationtimechangelog._self
            new TranslationSeedItem("entity.standardoperationtimechangelog._self", "zh-HK", "标准工序时间变更记录信息_hk", "实体名称"),

            // entity.standardoperationtimechangelog.standardoperationtimeid
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtimeid", "en-US", "标准工序时间ID_us", "标准工序时间ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationtimechangelog.standardoperationtimeid
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtimeid", "ja-JP", "标准工序时间ID_jp", "标准工序时间ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationtimechangelog.standardoperationtimeid
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtimeid", "zh-CN", "标准工序时间ID", "标准工序时间ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.standardoperationtimechangelog.standardoperationtimeid
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtimeid", "zh-HK", "标准工序时间ID_hk", "标准工序时间ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.standardoperationtimechangelog.materialcode
            new TranslationSeedItem("entity.standardoperationtimechangelog.materialcode", "en-US", "物料编码_us", "物料编码（冗余）"),
            // entity.standardoperationtimechangelog.materialcode
            new TranslationSeedItem("entity.standardoperationtimechangelog.materialcode", "ja-JP", "物料编码_jp", "物料编码（冗余）"),
            // entity.standardoperationtimechangelog.materialcode
            new TranslationSeedItem("entity.standardoperationtimechangelog.materialcode", "zh-CN", "物料编码", "物料编码（冗余）"),
            // entity.standardoperationtimechangelog.materialcode
            new TranslationSeedItem("entity.standardoperationtimechangelog.materialcode", "zh-HK", "物料编码_hk", "物料编码（冗余）"),

            // entity.standardoperationtimechangelog.changefields
            new TranslationSeedItem("entity.standardoperationtimechangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationtimechangelog.changefields
            new TranslationSeedItem("entity.standardoperationtimechangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationtimechangelog.changefields
            new TranslationSeedItem("entity.standardoperationtimechangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),
            // entity.standardoperationtimechangelog.changefields
            new TranslationSeedItem("entity.standardoperationtimechangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）"),

            // entity.standardoperationtimechangelog.changetime
            new TranslationSeedItem("entity.standardoperationtimechangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.standardoperationtimechangelog.changetime
            new TranslationSeedItem("entity.standardoperationtimechangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.standardoperationtimechangelog.changetime
            new TranslationSeedItem("entity.standardoperationtimechangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.standardoperationtimechangelog.changetime
            new TranslationSeedItem("entity.standardoperationtimechangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.standardoperationtimechangelog.changeby
            new TranslationSeedItem("entity.standardoperationtimechangelog.changeby", "en-US", "变更人_us", "变更人（人员代码）"),
            // entity.standardoperationtimechangelog.changeby
            new TranslationSeedItem("entity.standardoperationtimechangelog.changeby", "ja-JP", "变更人_jp", "变更人（人员代码）"),
            // entity.standardoperationtimechangelog.changeby
            new TranslationSeedItem("entity.standardoperationtimechangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.standardoperationtimechangelog.changeby
            new TranslationSeedItem("entity.standardoperationtimechangelog.changeby", "zh-HK", "变更人_hk", "变更人（人员代码）"),

            // entity.standardoperationtimechangelog.changereason
            new TranslationSeedItem("entity.standardoperationtimechangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.standardoperationtimechangelog.changereason
            new TranslationSeedItem("entity.standardoperationtimechangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.standardoperationtimechangelog.changereason
            new TranslationSeedItem("entity.standardoperationtimechangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.standardoperationtimechangelog.changereason
            new TranslationSeedItem("entity.standardoperationtimechangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.standardoperationtimechangelog.standardoperationtime
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtime", "en-US", "标准工序时间主表_us", "标准工序时间主表"),
            // entity.standardoperationtimechangelog.standardoperationtime
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtime", "ja-JP", "标准工序时间主表_jp", "标准工序时间主表"),
            // entity.standardoperationtimechangelog.standardoperationtime
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtime", "zh-CN", "标准工序时间主表", "标准工序时间主表"),
            // entity.standardoperationtimechangelog.standardoperationtime
            new TranslationSeedItem("entity.standardoperationtimechangelog.standardoperationtime", "zh-HK", "标准工序时间主表_hk", "标准工序时间主表"),
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
        translation.ResourceGroup = "Bom";
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
