// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBillOfMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktBillOfMaterial 实体国际化翻译种子（键前缀 entity.billOfMaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBillOfMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBillOfMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billOfMaterial 实体翻译...", tenantCode);

        foreach (var item in GetBillOfMaterialTranslations())
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

        TaktLogger.Information("TaktBillOfMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBillOfMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.billOfMaterial._self / entity.billOfMaterial.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billOfMaterial._self
            new TranslationSeedItem("entity.billOfMaterial._self", "en-US", "Bill Of Material Information", "实体名称"),
            // entity.billOfMaterial._self
            new TranslationSeedItem("entity.billOfMaterial._self", "ja-JP", "Takt物料清单信息", "实体名称"),
            // entity.billOfMaterial._self
            new TranslationSeedItem("entity.billOfMaterial._self", "zh-CN", "Takt物料清单信息", "实体名称"),
            // entity.billOfMaterial._self
            new TranslationSeedItem("entity.billOfMaterial._self", "zh-HK", "Takt物料清单信息", "实体名称"),

            // entity.billOfMaterial.plantcode
            new TranslationSeedItem("entity.billOfMaterial.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.billOfMaterial.plantcode
            new TranslationSeedItem("entity.billOfMaterial.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.billOfMaterial.plantcode
            new TranslationSeedItem("entity.billOfMaterial.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.billOfMaterial.plantcode
            new TranslationSeedItem("entity.billOfMaterial.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.billOfMaterial.bomcode
            new TranslationSeedItem("entity.billOfMaterial.bomcode", "en-US", "BOM编码", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billOfMaterial.bomcode
            new TranslationSeedItem("entity.billOfMaterial.bomcode", "ja-JP", "BOM编码", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billOfMaterial.bomcode
            new TranslationSeedItem("entity.billOfMaterial.bomcode", "zh-CN", "BOM编码", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billOfMaterial.bomcode
            new TranslationSeedItem("entity.billOfMaterial.bomcode", "zh-HK", "BOM编码", "BOM编码（业务单据号，便于检索，非唯一键）"),

            // entity.billOfMaterial.bomname
            new TranslationSeedItem("entity.billOfMaterial.bomname", "en-US", "BOM名称", "BOM名称"),
            // entity.billOfMaterial.bomname
            new TranslationSeedItem("entity.billOfMaterial.bomname", "ja-JP", "BOM名称", "BOM名称"),
            // entity.billOfMaterial.bomname
            new TranslationSeedItem("entity.billOfMaterial.bomname", "zh-CN", "BOM名称", "BOM名称"),
            // entity.billOfMaterial.bomname
            new TranslationSeedItem("entity.billOfMaterial.bomname", "zh-HK", "BOM名称", "BOM名称"),

            // entity.billOfMaterial.parentmaterialid
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialid", "en-US", "父物料ID", "父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterial.parentmaterialid
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialid", "ja-JP", "父物料ID", "父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterial.parentmaterialid
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialid", "zh-CN", "父物料ID", "父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterial.parentmaterialid
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialid", "zh-HK", "父物料ID", "父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),

            // entity.billOfMaterial.parentmaterialcode
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialcode", "en-US", "父物料编码", "父物料编码（父项物料编码 item_code，冗余）"),
            // entity.billOfMaterial.parentmaterialcode
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialcode", "ja-JP", "父物料编码", "父物料编码（父项物料编码 item_code，冗余）"),
            // entity.billOfMaterial.parentmaterialcode
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialcode", "zh-CN", "父物料编码", "父物料编码（父项物料编码 item_code，冗余）"),
            // entity.billOfMaterial.parentmaterialcode
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialcode", "zh-HK", "父物料编码", "父物料编码（父项物料编码 item_code，冗余）"),

            // entity.billOfMaterial.parentmaterialname
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialname", "en-US", "父物料名称", "父物料名称（冗余）"),
            // entity.billOfMaterial.parentmaterialname
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialname", "ja-JP", "父物料名称", "父物料名称（冗余）"),
            // entity.billOfMaterial.parentmaterialname
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialname", "zh-CN", "父物料名称", "父物料名称（冗余）"),
            // entity.billOfMaterial.parentmaterialname
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialname", "zh-HK", "父物料名称", "父物料名称（冗余）"),

            // entity.billOfMaterial.bomversion
            new TranslationSeedItem("entity.billOfMaterial.bomversion", "en-US", "BOM版本号", "BOM版本号"),
            // entity.billOfMaterial.bomversion
            new TranslationSeedItem("entity.billOfMaterial.bomversion", "ja-JP", "BOM版本号", "BOM版本号"),
            // entity.billOfMaterial.bomversion
            new TranslationSeedItem("entity.billOfMaterial.bomversion", "zh-CN", "BOM版本号", "BOM版本号"),
            // entity.billOfMaterial.bomversion
            new TranslationSeedItem("entity.billOfMaterial.bomversion", "zh-HK", "BOM版本号", "BOM版本号"),

            // entity.billOfMaterial.bomtype
            new TranslationSeedItem("entity.billOfMaterial.bomtype", "en-US", "BOM类型", "BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）"),
            // entity.billOfMaterial.bomtype
            new TranslationSeedItem("entity.billOfMaterial.bomtype", "ja-JP", "BOM类型", "BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）"),
            // entity.billOfMaterial.bomtype
            new TranslationSeedItem("entity.billOfMaterial.bomtype", "zh-CN", "BOM类型", "BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）"),
            // entity.billOfMaterial.bomtype
            new TranslationSeedItem("entity.billOfMaterial.bomtype", "zh-HK", "BOM类型", "BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）"),

            // entity.billOfMaterial.alternativebomnumber
            new TranslationSeedItem("entity.billOfMaterial.alternativebomnumber", "en-US", "备选BOM编号", "备选BOM编号（对应SAP Alternative BOM，如01/02）"),
            // entity.billOfMaterial.alternativebomnumber
            new TranslationSeedItem("entity.billOfMaterial.alternativebomnumber", "ja-JP", "备选BOM编号", "备选BOM编号（对应SAP Alternative BOM，如01/02）"),
            // entity.billOfMaterial.alternativebomnumber
            new TranslationSeedItem("entity.billOfMaterial.alternativebomnumber", "zh-CN", "备选BOM编号", "备选BOM编号（对应SAP Alternative BOM，如01/02）"),
            // entity.billOfMaterial.alternativebomnumber
            new TranslationSeedItem("entity.billOfMaterial.alternativebomnumber", "zh-HK", "备选BOM编号", "备选BOM编号（对应SAP Alternative BOM，如01/02）"),

            // entity.billOfMaterial.effectivedate
            new TranslationSeedItem("entity.billOfMaterial.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.billOfMaterial.effectivedate
            new TranslationSeedItem("entity.billOfMaterial.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.billOfMaterial.effectivedate
            new TranslationSeedItem("entity.billOfMaterial.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.billOfMaterial.effectivedate
            new TranslationSeedItem("entity.billOfMaterial.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.billOfMaterial.expirydate
            new TranslationSeedItem("entity.billOfMaterial.expirydate", "en-US", "失效日期", "失效日期（为空表示永久有效）"),
            // entity.billOfMaterial.expirydate
            new TranslationSeedItem("entity.billOfMaterial.expirydate", "ja-JP", "失效日期", "失效日期（为空表示永久有效）"),
            // entity.billOfMaterial.expirydate
            new TranslationSeedItem("entity.billOfMaterial.expirydate", "zh-CN", "失效日期", "失效日期（为空表示永久有效）"),
            // entity.billOfMaterial.expirydate
            new TranslationSeedItem("entity.billOfMaterial.expirydate", "zh-HK", "失效日期", "失效日期（为空表示永久有效）"),

            // entity.billOfMaterial.parentmaterialunit
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialunit", "en-US", "父物料单位", "父物料单位"),
            // entity.billOfMaterial.parentmaterialunit
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialunit", "ja-JP", "父物料单位", "父物料单位"),
            // entity.billOfMaterial.parentmaterialunit
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialunit", "zh-CN", "父物料单位", "父物料单位"),
            // entity.billOfMaterial.parentmaterialunit
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialunit", "zh-HK", "父物料单位", "父物料单位"),

            // entity.billOfMaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialquantity", "en-US", "基本数量", "基本数量（BOM基数，对应SAP Base quantity）"),
            // entity.billOfMaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialquantity", "ja-JP", "基本数量", "基本数量（BOM基数，对应SAP Base quantity）"),
            // entity.billOfMaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialquantity", "zh-CN", "基本数量", "基本数量（BOM基数，对应SAP Base quantity）"),
            // entity.billOfMaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billOfMaterial.parentmaterialquantity", "zh-HK", "基本数量", "基本数量（BOM基数，对应SAP Base quantity）"),

            // entity.billOfMaterial.isenabled
            new TranslationSeedItem("entity.billOfMaterial.isenabled", "en-US", "是否启用", "是否启用（0=否，1=是）"),
            // entity.billOfMaterial.isenabled
            new TranslationSeedItem("entity.billOfMaterial.isenabled", "ja-JP", "是否启用", "是否启用（0=否，1=是）"),
            // entity.billOfMaterial.isenabled
            new TranslationSeedItem("entity.billOfMaterial.isenabled", "zh-CN", "是否启用", "是否启用（0=否，1=是）"),
            // entity.billOfMaterial.isenabled
            new TranslationSeedItem("entity.billOfMaterial.isenabled", "zh-HK", "是否启用", "是否启用（0=否，1=是）"),

            // entity.billOfMaterial.bomstatus
            new TranslationSeedItem("entity.billOfMaterial.bomstatus", "en-US", "BOM状态", "BOM状态（0=草稿，1=已发布，2=已停用）"),
            // entity.billOfMaterial.bomstatus
            new TranslationSeedItem("entity.billOfMaterial.bomstatus", "ja-JP", "BOM状态", "BOM状态（0=草稿，1=已发布，2=已停用）"),
            // entity.billOfMaterial.bomstatus
            new TranslationSeedItem("entity.billOfMaterial.bomstatus", "zh-CN", "BOM状态", "BOM状态（0=草稿，1=已发布，2=已停用）"),
            // entity.billOfMaterial.bomstatus
            new TranslationSeedItem("entity.billOfMaterial.bomstatus", "zh-HK", "BOM状态", "BOM状态（0=草稿，1=已发布，2=已停用）"),

            // entity.billOfMaterial.bomdescription
            new TranslationSeedItem("entity.billOfMaterial.bomdescription", "en-US", "BOM描述", "BOM描述"),
            // entity.billOfMaterial.bomdescription
            new TranslationSeedItem("entity.billOfMaterial.bomdescription", "ja-JP", "BOM描述", "BOM描述"),
            // entity.billOfMaterial.bomdescription
            new TranslationSeedItem("entity.billOfMaterial.bomdescription", "zh-CN", "BOM描述", "BOM描述"),
            // entity.billOfMaterial.bomdescription
            new TranslationSeedItem("entity.billOfMaterial.bomdescription", "zh-HK", "BOM描述", "BOM描述"),

            // entity.billOfMaterial.sortorder
            new TranslationSeedItem("entity.billOfMaterial.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.billOfMaterial.sortorder
            new TranslationSeedItem("entity.billOfMaterial.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.billOfMaterial.sortorder
            new TranslationSeedItem("entity.billOfMaterial.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.billOfMaterial.sortorder
            new TranslationSeedItem("entity.billOfMaterial.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.billOfMaterial.items
            new TranslationSeedItem("entity.billOfMaterial.items", "en-US", "BOM组成件明细", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billOfMaterial.items
            new TranslationSeedItem("entity.billOfMaterial.items", "ja-JP", "BOM组成件明细", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billOfMaterial.items
            new TranslationSeedItem("entity.billOfMaterial.items", "zh-CN", "BOM组成件明细", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billOfMaterial.items
            new TranslationSeedItem("entity.billOfMaterial.items", "zh-HK", "BOM组成件明细", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),

            // entity.billOfMaterial.changelogs
            new TranslationSeedItem("entity.billOfMaterial.changelogs", "en-US", "BOM变更记录列表", "BOM变更记录列表（外键在子表 <see cref=\"TaktBillOfMaterialChangeLog.BillOfMaterialId\"/>）"),
            // entity.billOfMaterial.changelogs
            new TranslationSeedItem("entity.billOfMaterial.changelogs", "ja-JP", "BOM变更记录列表", "BOM变更记录列表（外键在子表 <see cref=\"TaktBillOfMaterialChangeLog.BillOfMaterialId\"/>）"),
            // entity.billOfMaterial.changelogs
            new TranslationSeedItem("entity.billOfMaterial.changelogs", "zh-CN", "BOM变更记录列表", "BOM变更记录列表（外键在子表 <see cref=\"TaktBillOfMaterialChangeLog.BillOfMaterialId\"/>）"),
            // entity.billOfMaterial.changelogs
            new TranslationSeedItem("entity.billOfMaterial.changelogs", "zh-HK", "BOM变更记录列表", "BOM变更记录列表（外键在子表 <see cref=\"TaktBillOfMaterialChangeLog.BillOfMaterialId\"/>）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
