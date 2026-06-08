// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Report
// 文件名称：TaktConfigurableJoinI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableJoin 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Report;

/// <summary>
/// TaktConfigurableJoin 实体国际化翻译种子（键前缀 entity.configurableJoin.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableJoinI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableJoin 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurableJoin 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableJoinTranslations())
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

        TaktLogger.Information("TaktConfigurableJoin 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableJoin 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurableJoin._self / entity.configurableJoin.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableJoinTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurableJoin._self
            new TranslationSeedItem("entity.configurableJoin._self", "en-US", "Configurable Join Information", "实体名称"),
            // entity.configurableJoin._self
            new TranslationSeedItem("entity.configurableJoin._self", "ja-JP", "自定义报表多表关联定义信息", "实体名称"),
            // entity.configurableJoin._self
            new TranslationSeedItem("entity.configurableJoin._self", "zh-CN", "自定义报表多表关联定义信息", "实体名称"),
            // entity.configurableJoin._self
            new TranslationSeedItem("entity.configurableJoin._self", "zh-HK", "自定义报表多表关联定义信息", "实体名称"),

            // entity.configurableJoin.configurableid
            new TranslationSeedItem("entity.configurableJoin.configurableid", "en-US", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableJoin.configurableid
            new TranslationSeedItem("entity.configurableJoin.configurableid", "ja-JP", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableJoin.configurableid
            new TranslationSeedItem("entity.configurableJoin.configurableid", "zh-CN", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableJoin.configurableid
            new TranslationSeedItem("entity.configurableJoin.configurableid", "zh-HK", "报表主表ID", "关联报表主表 ID（主子表关系）"),

            // entity.configurableJoin.jointype
            new TranslationSeedItem("entity.configurableJoin.jointype", "en-US", "关联类型", "关联类型（内/左/右/全连接）"),
            // entity.configurableJoin.jointype
            new TranslationSeedItem("entity.configurableJoin.jointype", "ja-JP", "关联类型", "关联类型（内/左/右/全连接）"),
            // entity.configurableJoin.jointype
            new TranslationSeedItem("entity.configurableJoin.jointype", "zh-CN", "关联类型", "关联类型（内/左/右/全连接）"),
            // entity.configurableJoin.jointype
            new TranslationSeedItem("entity.configurableJoin.jointype", "zh-HK", "关联类型", "关联类型（内/左/右/全连接）"),

            // entity.configurableJoin.leftsourcealias
            new TranslationSeedItem("entity.configurableJoin.leftsourcealias", "en-US", "左表别名", "左表数据源别名"),
            // entity.configurableJoin.leftsourcealias
            new TranslationSeedItem("entity.configurableJoin.leftsourcealias", "ja-JP", "左表别名", "左表数据源别名"),
            // entity.configurableJoin.leftsourcealias
            new TranslationSeedItem("entity.configurableJoin.leftsourcealias", "zh-CN", "左表别名", "左表数据源别名"),
            // entity.configurableJoin.leftsourcealias
            new TranslationSeedItem("entity.configurableJoin.leftsourcealias", "zh-HK", "左表别名", "左表数据源别名"),

            // entity.configurableJoin.leftcolumnname
            new TranslationSeedItem("entity.configurableJoin.leftcolumnname", "en-US", "左表关联列", "左表关联列名"),
            // entity.configurableJoin.leftcolumnname
            new TranslationSeedItem("entity.configurableJoin.leftcolumnname", "ja-JP", "左表关联列", "左表关联列名"),
            // entity.configurableJoin.leftcolumnname
            new TranslationSeedItem("entity.configurableJoin.leftcolumnname", "zh-CN", "左表关联列", "左表关联列名"),
            // entity.configurableJoin.leftcolumnname
            new TranslationSeedItem("entity.configurableJoin.leftcolumnname", "zh-HK", "左表关联列", "左表关联列名"),

            // entity.configurableJoin.rightsourcealias
            new TranslationSeedItem("entity.configurableJoin.rightsourcealias", "en-US", "右表别名", "右表数据源别名"),
            // entity.configurableJoin.rightsourcealias
            new TranslationSeedItem("entity.configurableJoin.rightsourcealias", "ja-JP", "右表别名", "右表数据源别名"),
            // entity.configurableJoin.rightsourcealias
            new TranslationSeedItem("entity.configurableJoin.rightsourcealias", "zh-CN", "右表别名", "右表数据源别名"),
            // entity.configurableJoin.rightsourcealias
            new TranslationSeedItem("entity.configurableJoin.rightsourcealias", "zh-HK", "右表别名", "右表数据源别名"),

            // entity.configurableJoin.rightcolumnname
            new TranslationSeedItem("entity.configurableJoin.rightcolumnname", "en-US", "右表关联列", "右表关联列名"),
            // entity.configurableJoin.rightcolumnname
            new TranslationSeedItem("entity.configurableJoin.rightcolumnname", "ja-JP", "右表关联列", "右表关联列名"),
            // entity.configurableJoin.rightcolumnname
            new TranslationSeedItem("entity.configurableJoin.rightcolumnname", "zh-CN", "右表关联列", "右表关联列名"),
            // entity.configurableJoin.rightcolumnname
            new TranslationSeedItem("entity.configurableJoin.rightcolumnname", "zh-HK", "右表关联列", "右表关联列名"),

            // entity.configurableJoin.sortorder
            new TranslationSeedItem("entity.configurableJoin.sortorder", "en-US", "排序号", "排序号（JOIN 应用顺序）"),
            // entity.configurableJoin.sortorder
            new TranslationSeedItem("entity.configurableJoin.sortorder", "ja-JP", "排序号", "排序号（JOIN 应用顺序）"),
            // entity.configurableJoin.sortorder
            new TranslationSeedItem("entity.configurableJoin.sortorder", "zh-CN", "排序号", "排序号（JOIN 应用顺序）"),
            // entity.configurableJoin.sortorder
            new TranslationSeedItem("entity.configurableJoin.sortorder", "zh-HK", "排序号", "排序号（JOIN 应用顺序）"),

            // entity.configurableJoin.configurable
            new TranslationSeedItem("entity.configurableJoin.configurable", "en-US", "关联的报表主表", "关联的报表主表"),
            // entity.configurableJoin.configurable
            new TranslationSeedItem("entity.configurableJoin.configurable", "ja-JP", "关联的报表主表", "关联的报表主表"),
            // entity.configurableJoin.configurable
            new TranslationSeedItem("entity.configurableJoin.configurable", "zh-CN", "关联的报表主表", "关联的报表主表"),
            // entity.configurableJoin.configurable
            new TranslationSeedItem("entity.configurableJoin.configurable", "zh-HK", "关联的报表主表", "关联的报表主表"),
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
