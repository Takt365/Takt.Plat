// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderChangeLogI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionOrderChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionOrderChangeLog 实体国际化翻译种子（键前缀 entity.productionorderchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionOrderChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionOrderChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionorderchangelog 实体翻译...", tenantCode);

        foreach (var item in GetProductionOrderChangeLogTranslations())
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

        TaktLogger.Information("TaktProductionOrderChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionOrderChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionorderchangelog._self / entity.productionorderchangelog.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionOrderChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionorderchangelog._self
            new TranslationSeedItem("entity.productionorderchangelog._self", "en-US", "Production Order Change Log Information_us", "实体名称"),
            // entity.productionorderchangelog._self
            new TranslationSeedItem("entity.productionorderchangelog._self", "ja-JP", "生产工单变更记录信息_jp", "实体名称"),
            // entity.productionorderchangelog._self
            new TranslationSeedItem("entity.productionorderchangelog._self", "zh-CN", "生产工单变更记录信息", "实体名称"),
            // entity.productionorderchangelog._self
            new TranslationSeedItem("entity.productionorderchangelog._self", "zh-HK", "生产工单变更记录信息_hk", "实体名称"),

            // entity.productionorderchangelog.productionorderid
            new TranslationSeedItem("entity.productionorderchangelog.productionorderid", "en-US", "生产工单ID_us", "生产工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productionorderchangelog.productionorderid
            new TranslationSeedItem("entity.productionorderchangelog.productionorderid", "ja-JP", "生产工单ID_jp", "生产工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productionorderchangelog.productionorderid
            new TranslationSeedItem("entity.productionorderchangelog.productionorderid", "zh-CN", "生产工单ID", "生产工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productionorderchangelog.productionorderid
            new TranslationSeedItem("entity.productionorderchangelog.productionorderid", "zh-HK", "生产工单ID_hk", "生产工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.productionorderchangelog.prodordercode
            new TranslationSeedItem("entity.productionorderchangelog.prodordercode", "en-US", "生产工单号_us", "生产工单号"),
            // entity.productionorderchangelog.prodordercode
            new TranslationSeedItem("entity.productionorderchangelog.prodordercode", "ja-JP", "生产工单号_jp", "生产工单号"),
            // entity.productionorderchangelog.prodordercode
            new TranslationSeedItem("entity.productionorderchangelog.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.productionorderchangelog.prodordercode
            new TranslationSeedItem("entity.productionorderchangelog.prodordercode", "zh-HK", "生产工单号_hk", "生产工单号"),

            // entity.productionorderchangelog.changefields
            new TranslationSeedItem("entity.productionorderchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.productionorderchangelog.changefields
            new TranslationSeedItem("entity.productionorderchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.productionorderchangelog.changefields
            new TranslationSeedItem("entity.productionorderchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.productionorderchangelog.changefields
            new TranslationSeedItem("entity.productionorderchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),

            // entity.productionorderchangelog.changetime
            new TranslationSeedItem("entity.productionorderchangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.productionorderchangelog.changetime
            new TranslationSeedItem("entity.productionorderchangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.productionorderchangelog.changetime
            new TranslationSeedItem("entity.productionorderchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.productionorderchangelog.changetime
            new TranslationSeedItem("entity.productionorderchangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.productionorderchangelog.changeby
            new TranslationSeedItem("entity.productionorderchangelog.changeby", "en-US", "变更人_us", "变更人（人员代码）"),
            // entity.productionorderchangelog.changeby
            new TranslationSeedItem("entity.productionorderchangelog.changeby", "ja-JP", "变更人_jp", "变更人（人员代码）"),
            // entity.productionorderchangelog.changeby
            new TranslationSeedItem("entity.productionorderchangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.productionorderchangelog.changeby
            new TranslationSeedItem("entity.productionorderchangelog.changeby", "zh-HK", "变更人_hk", "变更人（人员代码）"),

            // entity.productionorderchangelog.changereason
            new TranslationSeedItem("entity.productionorderchangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.productionorderchangelog.changereason
            new TranslationSeedItem("entity.productionorderchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.productionorderchangelog.changereason
            new TranslationSeedItem("entity.productionorderchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.productionorderchangelog.changereason
            new TranslationSeedItem("entity.productionorderchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.productionorderchangelog.productionorder
            new TranslationSeedItem("entity.productionorderchangelog.productionorder", "en-US", "生产工单主表_us", "生产工单主表"),
            // entity.productionorderchangelog.productionorder
            new TranslationSeedItem("entity.productionorderchangelog.productionorder", "ja-JP", "生产工单主表_jp", "生产工单主表"),
            // entity.productionorderchangelog.productionorder
            new TranslationSeedItem("entity.productionorderchangelog.productionorder", "zh-CN", "生产工单主表", "生产工单主表"),
            // entity.productionorderchangelog.productionorder
            new TranslationSeedItem("entity.productionorderchangelog.productionorder", "zh-HK", "生产工单主表_hk", "生产工单主表"),
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
        translation.ResourceGroup = "Output";
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
