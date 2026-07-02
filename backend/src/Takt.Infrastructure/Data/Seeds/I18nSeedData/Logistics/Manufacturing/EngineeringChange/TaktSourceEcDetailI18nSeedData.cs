// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSourceEcDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktSourceEcDetail 实体国际化翻译种子（键前缀 entity.sourceecdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSourceEcDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSourceEcDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sourceecdetail 实体翻译...", tenantCode);

        foreach (var item in GetSourceEcDetailTranslations())
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

        TaktLogger.Information("TaktSourceEcDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSourceEcDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sourceecdetail._self / entity.sourceecdetail.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSourceEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "en-US", "Source Ec Detail Information_us", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "ja-JP", "设变来源主表信息_jp", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "zh-CN", "设变来源主表信息", "实体名称"),
            // entity.sourceecdetail._self
            new TranslationSeedItem("entity.sourceecdetail._self", "zh-HK", "设变来源主表信息_hk", "实体名称"),

            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "en-US", "主ID_us", "主ID"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "ja-JP", "主ID_jp", "主ID"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "zh-CN", "主ID", "主ID"),
            // entity.sourceecdetail.sourceecid
            new TranslationSeedItem("entity.sourceecdetail.sourceecid", "zh-HK", "主ID_hk", "主ID"),

            // entity.sourceecdetail.sourcefinishedproduct
            new TranslationSeedItem("entity.sourceecdetail.sourcefinishedproduct", "en-US", "完成品_us", "完成品"),
            // entity.sourceecdetail.sourcefinishedproduct
            new TranslationSeedItem("entity.sourceecdetail.sourcefinishedproduct", "ja-JP", "完成品_jp", "完成品"),
            // entity.sourceecdetail.sourcefinishedproduct
            new TranslationSeedItem("entity.sourceecdetail.sourcefinishedproduct", "zh-CN", "完成品", "完成品"),
            // entity.sourceecdetail.sourcefinishedproduct
            new TranslationSeedItem("entity.sourceecdetail.sourcefinishedproduct", "zh-HK", "完成品_hk", "完成品"),

            // entity.sourceecdetail.sourceparentpart
            new TranslationSeedItem("entity.sourceecdetail.sourceparentpart", "en-US", "上阶物料_us", "上阶物料"),
            // entity.sourceecdetail.sourceparentpart
            new TranslationSeedItem("entity.sourceecdetail.sourceparentpart", "ja-JP", "上阶物料_jp", "上阶物料"),
            // entity.sourceecdetail.sourceparentpart
            new TranslationSeedItem("entity.sourceecdetail.sourceparentpart", "zh-CN", "上阶物料", "上阶物料"),
            // entity.sourceecdetail.sourceparentpart
            new TranslationSeedItem("entity.sourceecdetail.sourceparentpart", "zh-HK", "上阶物料_hk", "上阶物料"),

            // entity.sourceecdetail.sourcelegacypartno
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartno", "en-US", "旧物料号_us", "旧物料号"),
            // entity.sourceecdetail.sourcelegacypartno
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartno", "ja-JP", "旧物料号_jp", "旧物料号"),
            // entity.sourceecdetail.sourcelegacypartno
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartno", "zh-CN", "旧物料号", "旧物料号"),
            // entity.sourceecdetail.sourcelegacypartno
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartno", "zh-HK", "旧物料号_hk", "旧物料号"),

            // entity.sourceecdetail.sourcelegacypartname
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartname", "en-US", "旧物料_us", "旧物料"),
            // entity.sourceecdetail.sourcelegacypartname
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartname", "ja-JP", "旧物料_jp", "旧物料"),
            // entity.sourceecdetail.sourcelegacypartname
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartname", "zh-CN", "旧物料", "旧物料"),
            // entity.sourceecdetail.sourcelegacypartname
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartname", "zh-HK", "旧物料_hk", "旧物料"),

            // entity.sourceecdetail.sourcelegacyusage
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacyusage", "en-US", "旧物料用量_us", "旧物料用量"),
            // entity.sourceecdetail.sourcelegacyusage
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacyusage", "ja-JP", "旧物料用量_jp", "旧物料用量"),
            // entity.sourceecdetail.sourcelegacyusage
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacyusage", "zh-CN", "旧物料用量", "旧物料用量"),
            // entity.sourceecdetail.sourcelegacyusage
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacyusage", "zh-HK", "旧物料用量_hk", "旧物料用量"),

            // entity.sourceecdetail.sourcelegacymountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacymountingposition", "en-US", "旧物料安装位置_us", "旧物料安装位置"),
            // entity.sourceecdetail.sourcelegacymountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacymountingposition", "ja-JP", "旧物料安装位置_jp", "旧物料安装位置"),
            // entity.sourceecdetail.sourcelegacymountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacymountingposition", "zh-CN", "旧物料安装位置", "旧物料安装位置"),
            // entity.sourceecdetail.sourcelegacymountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacymountingposition", "zh-HK", "旧物料安装位置_hk", "旧物料安装位置"),

            // entity.sourceecdetail.sourcereplacementpartno
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartno", "en-US", "新物料_us", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartno
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartno", "ja-JP", "新物料_jp", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartno
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartno", "zh-CN", "新物料", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartno
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartno", "zh-HK", "新物料_hk", "新物料"),

            // entity.sourceecdetail.sourcereplacementpartname
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartname", "en-US", "新物料_us", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartname
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartname", "ja-JP", "新物料_jp", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartname
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartname", "zh-CN", "新物料", "新物料"),
            // entity.sourceecdetail.sourcereplacementpartname
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementpartname", "zh-HK", "新物料_hk", "新物料"),

            // entity.sourceecdetail.sourcereplacementusage
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementusage", "en-US", "新物料用量_us", "新物料用量"),
            // entity.sourceecdetail.sourcereplacementusage
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementusage", "ja-JP", "新物料用量_jp", "新物料用量"),
            // entity.sourceecdetail.sourcereplacementusage
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementusage", "zh-CN", "新物料用量", "新物料用量"),
            // entity.sourceecdetail.sourcereplacementusage
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementusage", "zh-HK", "新物料用量_hk", "新物料用量"),

            // entity.sourceecdetail.sourcereplacementmountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementmountingposition", "en-US", "新物料安装位置_us", "新物料安装位置"),
            // entity.sourceecdetail.sourcereplacementmountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementmountingposition", "ja-JP", "新物料安装位置_jp", "新物料安装位置"),
            // entity.sourceecdetail.sourcereplacementmountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementmountingposition", "zh-CN", "新物料安装位置", "新物料安装位置"),
            // entity.sourceecdetail.sourcereplacementmountingposition
            new TranslationSeedItem("entity.sourceecdetail.sourcereplacementmountingposition", "zh-HK", "新物料安装位置_hk", "新物料安装位置"),

            // entity.sourceecdetail.sourcebomno
            new TranslationSeedItem("entity.sourceecdetail.sourcebomno", "en-US", "BOM番号_us", "BOM番号"),
            // entity.sourceecdetail.sourcebomno
            new TranslationSeedItem("entity.sourceecdetail.sourcebomno", "ja-JP", "BOM番号_jp", "BOM番号"),
            // entity.sourceecdetail.sourcebomno
            new TranslationSeedItem("entity.sourceecdetail.sourcebomno", "zh-CN", "BOM番号", "BOM番号"),
            // entity.sourceecdetail.sourcebomno
            new TranslationSeedItem("entity.sourceecdetail.sourcebomno", "zh-HK", "BOM番号_hk", "BOM番号"),

            // entity.sourceecdetail.sourceinterchangeability
            new TranslationSeedItem("entity.sourceecdetail.sourceinterchangeability", "en-US", "互换性_us", "互换性"),
            // entity.sourceecdetail.sourceinterchangeability
            new TranslationSeedItem("entity.sourceecdetail.sourceinterchangeability", "ja-JP", "互换性_jp", "互换性"),
            // entity.sourceecdetail.sourceinterchangeability
            new TranslationSeedItem("entity.sourceecdetail.sourceinterchangeability", "zh-CN", "互换性", "互换性"),
            // entity.sourceecdetail.sourceinterchangeability
            new TranslationSeedItem("entity.sourceecdetail.sourceinterchangeability", "zh-HK", "互换性_hk", "互换性"),

            // entity.sourceecdetail.sourcedistinction
            new TranslationSeedItem("entity.sourceecdetail.sourcedistinction", "en-US", "区分_us", "区分"),
            // entity.sourceecdetail.sourcedistinction
            new TranslationSeedItem("entity.sourceecdetail.sourcedistinction", "ja-JP", "区分_jp", "区分"),
            // entity.sourceecdetail.sourcedistinction
            new TranslationSeedItem("entity.sourceecdetail.sourcedistinction", "zh-CN", "区分", "区分"),
            // entity.sourceecdetail.sourcedistinction
            new TranslationSeedItem("entity.sourceecdetail.sourcedistinction", "zh-HK", "区分_hk", "区分"),

            // entity.sourceecdetail.sourcearrangementinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourcearrangementinstruction", "en-US", "安排指示_us", "安排指示"),
            // entity.sourceecdetail.sourcearrangementinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourcearrangementinstruction", "ja-JP", "安排指示_jp", "安排指示"),
            // entity.sourceecdetail.sourcearrangementinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourcearrangementinstruction", "zh-CN", "安排指示", "安排指示"),
            // entity.sourceecdetail.sourcearrangementinstruction
            new TranslationSeedItem("entity.sourceecdetail.sourcearrangementinstruction", "zh-HK", "安排指示_hk", "安排指示"),

            // entity.sourceecdetail.sourcelegacypartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartdisposition", "en-US", "旧物料处理_us", "旧物料处理"),
            // entity.sourceecdetail.sourcelegacypartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartdisposition", "ja-JP", "旧物料处理_jp", "旧物料处理"),
            // entity.sourceecdetail.sourcelegacypartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartdisposition", "zh-CN", "旧物料处理", "旧物料处理"),
            // entity.sourceecdetail.sourcelegacypartdisposition
            new TranslationSeedItem("entity.sourceecdetail.sourcelegacypartdisposition", "zh-HK", "旧物料处理_hk", "旧物料处理"),

            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "en-US", "BOM生效日期_us", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "ja-JP", "BOM生效日期_jp", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "zh-CN", "BOM生效日期", "BOM生效日期"),
            // entity.sourceecdetail.sourcebomeffectivedate
            new TranslationSeedItem("entity.sourceecdetail.sourcebomeffectivedate", "zh-HK", "BOM生效日期_hk", "BOM生效日期"),

            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "en-US", "设变来源主表_us", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "ja-JP", "设变来源主表_jp", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "zh-CN", "设变来源主表", "设变来源主表"),
            // entity.sourceecdetail.sourceec
            new TranslationSeedItem("entity.sourceecdetail.sourceec", "zh-HK", "设变来源主表_hk", "设变来源主表"),
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
        translation.ResourceGroup = "EngineeringChange";
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
