// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanItemI18nSeedData.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPlanItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning;

/// <summary>
/// TaktSalesPlanItem 实体国际化翻译种子（键前缀 entity.salesplanitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPlanItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPlanItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesplanitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesPlanItemTranslations())
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

        TaktLogger.Information("TaktSalesPlanItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPlanItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesplanitem._self / entity.salesplanitem.{{field}}；ResourceGroup=Planning；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPlanItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesplanitem._self
            new TranslationSeedItem("entity.salesplanitem._self", "en-US", "Sales Plan Item Information_us", "实体名称"),
            // entity.salesplanitem._self
            new TranslationSeedItem("entity.salesplanitem._self", "ja-JP", "Takt销售计划明细信息_jp", "实体名称"),
            // entity.salesplanitem._self
            new TranslationSeedItem("entity.salesplanitem._self", "zh-CN", "Takt销售计划明细信息", "实体名称"),
            // entity.salesplanitem._self
            new TranslationSeedItem("entity.salesplanitem._self", "zh-HK", "Takt销售计划明细信息_hk", "实体名称"),

            // entity.salesplanitem.salesplanid
            new TranslationSeedItem("entity.salesplanitem.salesplanid", "en-US", "销售计划ID_us", "销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplanitem.salesplanid
            new TranslationSeedItem("entity.salesplanitem.salesplanid", "ja-JP", "销售计划ID_jp", "销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplanitem.salesplanid
            new TranslationSeedItem("entity.salesplanitem.salesplanid", "zh-CN", "销售计划ID", "销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplanitem.salesplanid
            new TranslationSeedItem("entity.salesplanitem.salesplanid", "zh-HK", "销售计划ID_hk", "销售计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.salesplanitem.salesplancode
            new TranslationSeedItem("entity.salesplanitem.salesplancode", "en-US", "销售计划编码_us", "销售计划编码（冗余字段，便于查询）"),
            // entity.salesplanitem.salesplancode
            new TranslationSeedItem("entity.salesplanitem.salesplancode", "ja-JP", "销售计划编码_jp", "销售计划编码（冗余字段，便于查询）"),
            // entity.salesplanitem.salesplancode
            new TranslationSeedItem("entity.salesplanitem.salesplancode", "zh-CN", "销售计划编码", "销售计划编码（冗余字段，便于查询）"),
            // entity.salesplanitem.salesplancode
            new TranslationSeedItem("entity.salesplanitem.salesplancode", "zh-HK", "销售计划编码_hk", "销售计划编码（冗余字段，便于查询）"),

            // entity.salesplanitem.linenumber
            new TranslationSeedItem("entity.salesplanitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.salesplanitem.linenumber
            new TranslationSeedItem("entity.salesplanitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.salesplanitem.linenumber
            new TranslationSeedItem("entity.salesplanitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesplanitem.linenumber
            new TranslationSeedItem("entity.salesplanitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.salesplanitem.materialcode
            new TranslationSeedItem("entity.salesplanitem.materialcode", "en-US", "物料编码_us", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.salesplanitem.materialcode
            new TranslationSeedItem("entity.salesplanitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.salesplanitem.materialcode
            new TranslationSeedItem("entity.salesplanitem.materialcode", "zh-CN", "物料编码", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.salesplanitem.materialcode
            new TranslationSeedItem("entity.salesplanitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),

            // entity.salesplanitem.materialname
            new TranslationSeedItem("entity.salesplanitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.salesplanitem.materialname
            new TranslationSeedItem("entity.salesplanitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.salesplanitem.materialname
            new TranslationSeedItem("entity.salesplanitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.salesplanitem.materialname
            new TranslationSeedItem("entity.salesplanitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.salesplanitem.materialspecification
            new TranslationSeedItem("entity.salesplanitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.salesplanitem.materialspecification
            new TranslationSeedItem("entity.salesplanitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.salesplanitem.materialspecification
            new TranslationSeedItem("entity.salesplanitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.salesplanitem.materialspecification
            new TranslationSeedItem("entity.salesplanitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.salesplanitem.customercode
            new TranslationSeedItem("entity.salesplanitem.customercode", "en-US", "客户编码_us", "客户编码（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options；行级客户，可选）"),
            // entity.salesplanitem.customercode
            new TranslationSeedItem("entity.salesplanitem.customercode", "ja-JP", "客户编码_jp", "客户编码（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options；行级客户，可选）"),
            // entity.salesplanitem.customercode
            new TranslationSeedItem("entity.salesplanitem.customercode", "zh-CN", "客户编码", "客户编码（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options；行级客户，可选）"),
            // entity.salesplanitem.customercode
            new TranslationSeedItem("entity.salesplanitem.customercode", "zh-HK", "客户编码_hk", "客户编码（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options；行级客户，可选）"),

            // entity.salesplanitem.customername
            new TranslationSeedItem("entity.salesplanitem.customername", "en-US", "客户名称_us", "客户名称"),
            // entity.salesplanitem.customername
            new TranslationSeedItem("entity.salesplanitem.customername", "ja-JP", "客户名称_jp", "客户名称"),
            // entity.salesplanitem.customername
            new TranslationSeedItem("entity.salesplanitem.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesplanitem.customername
            new TranslationSeedItem("entity.salesplanitem.customername", "zh-HK", "客户名称_hk", "客户名称"),

            // entity.salesplanitem.planunit
            new TranslationSeedItem("entity.salesplanitem.planunit", "en-US", "计划单位_us", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesplanitem.planunit
            new TranslationSeedItem("entity.salesplanitem.planunit", "ja-JP", "计划单位_jp", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesplanitem.planunit
            new TranslationSeedItem("entity.salesplanitem.planunit", "zh-CN", "计划单位", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesplanitem.planunit
            new TranslationSeedItem("entity.salesplanitem.planunit", "zh-HK", "计划单位_hk", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),

            // entity.salesplanitem.planquantity
            new TranslationSeedItem("entity.salesplanitem.planquantity", "en-US", "计划数量_us", "计划数量（基本单位数量）"),
            // entity.salesplanitem.planquantity
            new TranslationSeedItem("entity.salesplanitem.planquantity", "ja-JP", "计划数量_jp", "计划数量（基本单位数量）"),
            // entity.salesplanitem.planquantity
            new TranslationSeedItem("entity.salesplanitem.planquantity", "zh-CN", "计划数量", "计划数量（基本单位数量）"),
            // entity.salesplanitem.planquantity
            new TranslationSeedItem("entity.salesplanitem.planquantity", "zh-HK", "计划数量_hk", "计划数量（基本单位数量）"),

            // entity.salesplanitem.planneddeliverydate
            new TranslationSeedItem("entity.salesplanitem.planneddeliverydate", "en-US", "计划交货日期_us", "计划交货日期"),
            // entity.salesplanitem.planneddeliverydate
            new TranslationSeedItem("entity.salesplanitem.planneddeliverydate", "ja-JP", "计划交货日期_jp", "计划交货日期"),
            // entity.salesplanitem.planneddeliverydate
            new TranslationSeedItem("entity.salesplanitem.planneddeliverydate", "zh-CN", "计划交货日期", "计划交货日期"),
            // entity.salesplanitem.planneddeliverydate
            new TranslationSeedItem("entity.salesplanitem.planneddeliverydate", "zh-HK", "计划交货日期_hk", "计划交货日期"),

            // entity.salesplanitem.convertedquantity
            new TranslationSeedItem("entity.salesplanitem.convertedquantity", "en-US", "已转生产销售数量_us", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplanitem.convertedquantity
            new TranslationSeedItem("entity.salesplanitem.convertedquantity", "ja-JP", "已转生产销售数量_jp", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplanitem.convertedquantity
            new TranslationSeedItem("entity.salesplanitem.convertedquantity", "zh-CN", "已转生产销售数量", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplanitem.convertedquantity
            new TranslationSeedItem("entity.salesplanitem.convertedquantity", "zh-HK", "已转生产销售数量_hk", "已转生产/销售数量（基本单位数量）"),

            // entity.salesplanitem.estimatedunitprice
            new TranslationSeedItem("entity.salesplanitem.estimatedunitprice", "en-US", "预计单价_us", "预计单价"),
            // entity.salesplanitem.estimatedunitprice
            new TranslationSeedItem("entity.salesplanitem.estimatedunitprice", "ja-JP", "预计单价_jp", "预计单价"),
            // entity.salesplanitem.estimatedunitprice
            new TranslationSeedItem("entity.salesplanitem.estimatedunitprice", "zh-CN", "预计单价", "预计单价"),
            // entity.salesplanitem.estimatedunitprice
            new TranslationSeedItem("entity.salesplanitem.estimatedunitprice", "zh-HK", "预计单价_hk", "预计单价"),

            // entity.salesplanitem.estimatedamount
            new TranslationSeedItem("entity.salesplanitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.salesplanitem.estimatedamount
            new TranslationSeedItem("entity.salesplanitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.salesplanitem.estimatedamount
            new TranslationSeedItem("entity.salesplanitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.salesplanitem.estimatedamount
            new TranslationSeedItem("entity.salesplanitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.salesplanitem.isobsolete
            new TranslationSeedItem("entity.salesplanitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesplanitem.isobsolete
            new TranslationSeedItem("entity.salesplanitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesplanitem.isobsolete
            new TranslationSeedItem("entity.salesplanitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesplanitem.isobsolete
            new TranslationSeedItem("entity.salesplanitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Planning";
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
