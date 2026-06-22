// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesQuotationChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesQuotationChangeLog 实体国际化翻译种子（键前缀 entity.salesquotationchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesQuotationChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesQuotationChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesquotationchangelog 实体翻译...", tenantCode);

        foreach (var item in GetSalesQuotationChangeLogTranslations())
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

        TaktLogger.Information("TaktSalesQuotationChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesQuotationChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesquotationchangelog._self / entity.salesquotationchangelog.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesquotationchangelog._self
            new TranslationSeedItem("entity.salesquotationchangelog._self", "en-US", "Sales Quotation Change Log Information_us", "实体名称"),
            // entity.salesquotationchangelog._self
            new TranslationSeedItem("entity.salesquotationchangelog._self", "ja-JP", "销售报价变更记录信息_jp", "实体名称"),
            // entity.salesquotationchangelog._self
            new TranslationSeedItem("entity.salesquotationchangelog._self", "zh-CN", "销售报价变更记录信息", "实体名称"),
            // entity.salesquotationchangelog._self
            new TranslationSeedItem("entity.salesquotationchangelog._self", "zh-HK", "销售报价变更记录信息_hk", "实体名称"),

            // entity.salesquotationchangelog.salesquotationid
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationid", "en-US", "销售报价ID_us", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationchangelog.salesquotationid
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationid", "ja-JP", "销售报价ID_jp", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationchangelog.salesquotationid
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationid", "zh-CN", "销售报价ID", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.salesquotationchangelog.salesquotationid
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationid", "zh-HK", "销售报价ID_hk", "销售报价ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.salesquotationchangelog.salesquotationcode
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationcode", "en-US", "销售报价编码_us", "销售报价编码"),
            // entity.salesquotationchangelog.salesquotationcode
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationcode", "ja-JP", "销售报价编码_jp", "销售报价编码"),
            // entity.salesquotationchangelog.salesquotationcode
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationcode", "zh-CN", "销售报价编码", "销售报价编码"),
            // entity.salesquotationchangelog.salesquotationcode
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotationcode", "zh-HK", "销售报价编码_hk", "销售报价编码"),

            // entity.salesquotationchangelog.changefields
            new TranslationSeedItem("entity.salesquotationchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.salesquotationchangelog.changefields
            new TranslationSeedItem("entity.salesquotationchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.salesquotationchangelog.changefields
            new TranslationSeedItem("entity.salesquotationchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.salesquotationchangelog.changefields
            new TranslationSeedItem("entity.salesquotationchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),

            // entity.salesquotationchangelog.changetime
            new TranslationSeedItem("entity.salesquotationchangelog.changetime", "en-US", "变更时间_us", "变更时间"),
            // entity.salesquotationchangelog.changetime
            new TranslationSeedItem("entity.salesquotationchangelog.changetime", "ja-JP", "变更时间_jp", "变更时间"),
            // entity.salesquotationchangelog.changetime
            new TranslationSeedItem("entity.salesquotationchangelog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.salesquotationchangelog.changetime
            new TranslationSeedItem("entity.salesquotationchangelog.changetime", "zh-HK", "变更时间_hk", "变更时间"),

            // entity.salesquotationchangelog.changeby
            new TranslationSeedItem("entity.salesquotationchangelog.changeby", "en-US", "变更人_us", "变更人（人员代码）"),
            // entity.salesquotationchangelog.changeby
            new TranslationSeedItem("entity.salesquotationchangelog.changeby", "ja-JP", "变更人_jp", "变更人（人员代码）"),
            // entity.salesquotationchangelog.changeby
            new TranslationSeedItem("entity.salesquotationchangelog.changeby", "zh-CN", "变更人", "变更人（人员代码）"),
            // entity.salesquotationchangelog.changeby
            new TranslationSeedItem("entity.salesquotationchangelog.changeby", "zh-HK", "变更人_hk", "变更人（人员代码）"),

            // entity.salesquotationchangelog.changereason
            new TranslationSeedItem("entity.salesquotationchangelog.changereason", "en-US", "变更原因_us", "变更原因"),
            // entity.salesquotationchangelog.changereason
            new TranslationSeedItem("entity.salesquotationchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因"),
            // entity.salesquotationchangelog.changereason
            new TranslationSeedItem("entity.salesquotationchangelog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.salesquotationchangelog.changereason
            new TranslationSeedItem("entity.salesquotationchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因"),

            // entity.salesquotationchangelog.salesquotation
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotation", "en-US", "销售报价主表_us", "销售报价主表"),
            // entity.salesquotationchangelog.salesquotation
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotation", "ja-JP", "销售报价主表_jp", "销售报价主表"),
            // entity.salesquotationchangelog.salesquotation
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotation", "zh-CN", "销售报价主表", "销售报价主表"),
            // entity.salesquotationchangelog.salesquotation
            new TranslationSeedItem("entity.salesquotationchangelog.salesquotation", "zh-HK", "销售报价主表_hk", "销售报价主表"),
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
        translation.ResourceGroup = "Sales";
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
