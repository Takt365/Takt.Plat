// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Report
// 文件名称：TaktConfigurableSelectionI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableSelection 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktConfigurableSelection 实体国际化翻译种子（键前缀 entity.configurableSelection.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableSelectionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableSelection 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurableSelection 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableSelectionTranslations())
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

        TaktLogger.Information("TaktConfigurableSelection 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableSelection 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurableSelection._self / entity.configurableSelection.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableSelectionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurableSelection._self
            new TranslationSeedItem("entity.configurableSelection._self", "en-US", "Configurable Selection Information", "实体名称"),
            // entity.configurableSelection._self
            new TranslationSeedItem("entity.configurableSelection._self", "ja-JP", "自定义报表筛选条件信息", "实体名称"),
            // entity.configurableSelection._self
            new TranslationSeedItem("entity.configurableSelection._self", "zh-CN", "自定义报表筛选条件信息", "实体名称"),
            // entity.configurableSelection._self
            new TranslationSeedItem("entity.configurableSelection._self", "zh-HK", "自定义报表筛选条件信息", "实体名称"),

            // entity.configurableSelection.configurableid
            new TranslationSeedItem("entity.configurableSelection.configurableid", "en-US", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableSelection.configurableid
            new TranslationSeedItem("entity.configurableSelection.configurableid", "ja-JP", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableSelection.configurableid
            new TranslationSeedItem("entity.configurableSelection.configurableid", "zh-CN", "报表主表ID", "关联报表主表 ID（主子表关系）"),
            // entity.configurableSelection.configurableid
            new TranslationSeedItem("entity.configurableSelection.configurableid", "zh-HK", "报表主表ID", "关联报表主表 ID（主子表关系）"),

            // entity.configurableSelection.sourcealias
            new TranslationSeedItem("entity.configurableSelection.sourcealias", "en-US", "数据源别名", "数据源别名"),
            // entity.configurableSelection.sourcealias
            new TranslationSeedItem("entity.configurableSelection.sourcealias", "ja-JP", "数据源别名", "数据源别名"),
            // entity.configurableSelection.sourcealias
            new TranslationSeedItem("entity.configurableSelection.sourcealias", "zh-CN", "数据源别名", "数据源别名"),
            // entity.configurableSelection.sourcealias
            new TranslationSeedItem("entity.configurableSelection.sourcealias", "zh-HK", "数据源别名", "数据源别名"),

            // entity.configurableSelection.columnname
            new TranslationSeedItem("entity.configurableSelection.columnname", "en-US", "列名", "列名"),
            // entity.configurableSelection.columnname
            new TranslationSeedItem("entity.configurableSelection.columnname", "ja-JP", "列名", "列名"),
            // entity.configurableSelection.columnname
            new TranslationSeedItem("entity.configurableSelection.columnname", "zh-CN", "列名", "列名"),
            // entity.configurableSelection.columnname
            new TranslationSeedItem("entity.configurableSelection.columnname", "zh-HK", "列名", "列名"),

            // entity.configurableSelection.displayname
            new TranslationSeedItem("entity.configurableSelection.displayname", "en-US", "显示名称", "显示名称（Selection Screen 标签）"),
            // entity.configurableSelection.displayname
            new TranslationSeedItem("entity.configurableSelection.displayname", "ja-JP", "显示名称", "显示名称（Selection Screen 标签）"),
            // entity.configurableSelection.displayname
            new TranslationSeedItem("entity.configurableSelection.displayname", "zh-CN", "显示名称", "显示名称（Selection Screen 标签）"),
            // entity.configurableSelection.displayname
            new TranslationSeedItem("entity.configurableSelection.displayname", "zh-HK", "显示名称", "显示名称（Selection Screen 标签）"),

            // entity.configurableSelection.filteroperator
            new TranslationSeedItem("entity.configurableSelection.filteroperator", "en-US", "比较运算符", "比较运算符"),
            // entity.configurableSelection.filteroperator
            new TranslationSeedItem("entity.configurableSelection.filteroperator", "ja-JP", "比较运算符", "比较运算符"),
            // entity.configurableSelection.filteroperator
            new TranslationSeedItem("entity.configurableSelection.filteroperator", "zh-CN", "比较运算符", "比较运算符"),
            // entity.configurableSelection.filteroperator
            new TranslationSeedItem("entity.configurableSelection.filteroperator", "zh-HK", "比较运算符", "比较运算符"),

            // entity.configurableSelection.defaultvalue
            new TranslationSeedItem("entity.configurableSelection.defaultvalue", "en-US", "默认值", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableSelection.defaultvalue
            new TranslationSeedItem("entity.configurableSelection.defaultvalue", "ja-JP", "默认值", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableSelection.defaultvalue
            new TranslationSeedItem("entity.configurableSelection.defaultvalue", "zh-CN", "默认值", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableSelection.defaultvalue
            new TranslationSeedItem("entity.configurableSelection.defaultvalue", "zh-HK", "默认值", "默认值（单值或 IN 列表逗号分隔）"),

            // entity.configurableSelection.defaultvalueto
            new TranslationSeedItem("entity.configurableSelection.defaultvalueto", "en-US", "区间结束值", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableSelection.defaultvalueto
            new TranslationSeedItem("entity.configurableSelection.defaultvalueto", "ja-JP", "区间结束值", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableSelection.defaultvalueto
            new TranslationSeedItem("entity.configurableSelection.defaultvalueto", "zh-CN", "区间结束值", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableSelection.defaultvalueto
            new TranslationSeedItem("entity.configurableSelection.defaultvalueto", "zh-HK", "区间结束值", "区间结束值（BETWEEN 时使用）"),

            // entity.configurableSelection.isrequired
            new TranslationSeedItem("entity.configurableSelection.isrequired", "en-US", "是否必填", "是否必填（0=否 1=是）"),
            // entity.configurableSelection.isrequired
            new TranslationSeedItem("entity.configurableSelection.isrequired", "ja-JP", "是否必填", "是否必填（0=否 1=是）"),
            // entity.configurableSelection.isrequired
            new TranslationSeedItem("entity.configurableSelection.isrequired", "zh-CN", "是否必填", "是否必填（0=否 1=是）"),
            // entity.configurableSelection.isrequired
            new TranslationSeedItem("entity.configurableSelection.isrequired", "zh-HK", "是否必填", "是否必填（0=否 1=是）"),

            // entity.configurableSelection.sortorder
            new TranslationSeedItem("entity.configurableSelection.sortorder", "en-US", "排序号", "排序号（Selection Screen 展示顺序）"),
            // entity.configurableSelection.sortorder
            new TranslationSeedItem("entity.configurableSelection.sortorder", "ja-JP", "排序号", "排序号（Selection Screen 展示顺序）"),
            // entity.configurableSelection.sortorder
            new TranslationSeedItem("entity.configurableSelection.sortorder", "zh-CN", "排序号", "排序号（Selection Screen 展示顺序）"),
            // entity.configurableSelection.sortorder
            new TranslationSeedItem("entity.configurableSelection.sortorder", "zh-HK", "排序号", "排序号（Selection Screen 展示顺序）"),

            // entity.configurableSelection.configurable
            new TranslationSeedItem("entity.configurableSelection.configurable", "en-US", "关联的报表主表", "关联的报表主表"),
            // entity.configurableSelection.configurable
            new TranslationSeedItem("entity.configurableSelection.configurable", "ja-JP", "关联的报表主表", "关联的报表主表"),
            // entity.configurableSelection.configurable
            new TranslationSeedItem("entity.configurableSelection.configurable", "zh-CN", "关联的报表主表", "关联的报表主表"),
            // entity.configurableSelection.configurable
            new TranslationSeedItem("entity.configurableSelection.configurable", "zh-HK", "关联的报表主表", "关联的报表主表"),
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
