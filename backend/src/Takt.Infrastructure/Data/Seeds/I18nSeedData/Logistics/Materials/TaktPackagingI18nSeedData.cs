// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPackagingI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPackaging 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPackaging 实体国际化翻译种子（键前缀 entity.packaging.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPackagingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPackaging 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 packaging 实体翻译...", tenantCode);

        foreach (var item in GetPackagingTranslations())
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

        TaktLogger.Information("TaktPackaging 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPackaging 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.packaging._self / entity.packaging.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPackagingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.packaging._self
            new TranslationSeedItem("entity.packaging._self", "en-US", "Packaging Information_us", "实体名称"),
            // entity.packaging._self
            new TranslationSeedItem("entity.packaging._self", "ja-JP", "Takt物料包装信息信息_jp", "实体名称"),
            // entity.packaging._self
            new TranslationSeedItem("entity.packaging._self", "zh-CN", "Takt物料包装信息信息", "实体名称"),
            // entity.packaging._self
            new TranslationSeedItem("entity.packaging._self", "zh-HK", "Takt物料包装信息信息_hk", "实体名称"),

            // entity.packaging.plantcode
            new TranslationSeedItem("entity.packaging.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.packaging.plantcode
            new TranslationSeedItem("entity.packaging.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.packaging.plantcode
            new TranslationSeedItem("entity.packaging.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.packaging.plantcode
            new TranslationSeedItem("entity.packaging.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.packaging.materialcode
            new TranslationSeedItem("entity.packaging.materialcode", "en-US", "物料编码_us", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.packaging.materialcode
            new TranslationSeedItem("entity.packaging.materialcode", "ja-JP", "物料编码_jp", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.packaging.materialcode
            new TranslationSeedItem("entity.packaging.materialcode", "zh-CN", "物料编码", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),
            // entity.packaging.materialcode
            new TranslationSeedItem("entity.packaging.materialcode", "zh-HK", "物料编码_hk", "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）"),

            // entity.packaging.materialname
            new TranslationSeedItem("entity.packaging.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.packaging.materialname
            new TranslationSeedItem("entity.packaging.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.packaging.materialname
            new TranslationSeedItem("entity.packaging.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.packaging.materialname
            new TranslationSeedItem("entity.packaging.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.packaging.hscode
            new TranslationSeedItem("entity.packaging.hscode", "en-US", "海关商品编码_us", "海关商品编码（HS Code）"),
            // entity.packaging.hscode
            new TranslationSeedItem("entity.packaging.hscode", "ja-JP", "海关商品编码_jp", "海关商品编码（HS Code）"),
            // entity.packaging.hscode
            new TranslationSeedItem("entity.packaging.hscode", "zh-CN", "海关商品编码", "海关商品编码（HS Code）"),
            // entity.packaging.hscode
            new TranslationSeedItem("entity.packaging.hscode", "zh-HK", "海关商品编码_hk", "海关商品编码（HS Code）"),

            // entity.packaging.hsname
            new TranslationSeedItem("entity.packaging.hsname", "en-US", "商品名称_us", "商品名称（HS Name）"),
            // entity.packaging.hsname
            new TranslationSeedItem("entity.packaging.hsname", "ja-JP", "商品名称_jp", "商品名称（HS Name）"),
            // entity.packaging.hsname
            new TranslationSeedItem("entity.packaging.hsname", "zh-CN", "商品名称", "商品名称（HS Name）"),
            // entity.packaging.hsname
            new TranslationSeedItem("entity.packaging.hsname", "zh-HK", "商品名称_hk", "商品名称（HS Name）"),

            // entity.packaging.additionalcode
            new TranslationSeedItem("entity.packaging.additionalcode", "en-US", "附加编码_us", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packaging.additionalcode
            new TranslationSeedItem("entity.packaging.additionalcode", "ja-JP", "附加编码_jp", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packaging.additionalcode
            new TranslationSeedItem("entity.packaging.additionalcode", "zh-CN", "附加编码", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),
            // entity.packaging.additionalcode
            new TranslationSeedItem("entity.packaging.additionalcode", "zh-HK", "附加编码_hk", "附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）"),

            // entity.packaging.origincountryregioncode
            new TranslationSeedItem("entity.packaging.origincountryregioncode", "en-US", "原产国/地区编码_us", "原产国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.origincountryregioncode
            new TranslationSeedItem("entity.packaging.origincountryregioncode", "ja-JP", "原产国/地区编码_jp", "原产国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.origincountryregioncode
            new TranslationSeedItem("entity.packaging.origincountryregioncode", "zh-CN", "原产国/地区编码", "原产国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.origincountryregioncode
            new TranslationSeedItem("entity.packaging.origincountryregioncode", "zh-HK", "原产国/地区编码_hk", "原产国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),

            // entity.packaging.origincountryregionname
            new TranslationSeedItem("entity.packaging.origincountryregionname", "en-US", "原产国/地区名称_us", "原产国/地区名称"),
            // entity.packaging.origincountryregionname
            new TranslationSeedItem("entity.packaging.origincountryregionname", "ja-JP", "原产国/地区名称_jp", "原产国/地区名称"),
            // entity.packaging.origincountryregionname
            new TranslationSeedItem("entity.packaging.origincountryregionname", "zh-CN", "原产国/地区名称", "原产国/地区名称"),
            // entity.packaging.origincountryregionname
            new TranslationSeedItem("entity.packaging.origincountryregionname", "zh-HK", "原产国/地区名称_hk", "原产国/地区名称"),

            // entity.packaging.destinationcountryregioncode
            new TranslationSeedItem("entity.packaging.destinationcountryregioncode", "en-US", "目的国/地区编码_us", "目的国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.destinationcountryregioncode
            new TranslationSeedItem("entity.packaging.destinationcountryregioncode", "ja-JP", "目的国/地区编码_jp", "目的国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.destinationcountryregioncode
            new TranslationSeedItem("entity.packaging.destinationcountryregioncode", "zh-CN", "目的国/地区编码", "目的国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),
            // entity.packaging.destinationcountryregioncode
            new TranslationSeedItem("entity.packaging.destinationcountryregioncode", "zh-HK", "目的国/地区编码_hk", "目的国/地区编码（ISO 3166-1 alpha-2 两位代码，选项 TaktIsoCodes/options，DictValue=IsoCode）"),

            // entity.packaging.destinationcountryregionname
            new TranslationSeedItem("entity.packaging.destinationcountryregionname", "en-US", "目的国/地区名称_us", "目的国/地区名称"),
            // entity.packaging.destinationcountryregionname
            new TranslationSeedItem("entity.packaging.destinationcountryregionname", "ja-JP", "目的国/地区名称_jp", "目的国/地区名称"),
            // entity.packaging.destinationcountryregionname
            new TranslationSeedItem("entity.packaging.destinationcountryregionname", "zh-CN", "目的国/地区名称", "目的国/地区名称"),
            // entity.packaging.destinationcountryregionname
            new TranslationSeedItem("entity.packaging.destinationcountryregionname", "zh-HK", "目的国/地区名称_hk", "目的国/地区名称"),

            // entity.packaging.regulatoryconditioncode
            new TranslationSeedItem("entity.packaging.regulatoryconditioncode", "en-US", "监管条件代码_us", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packaging.regulatoryconditioncode
            new TranslationSeedItem("entity.packaging.regulatoryconditioncode", "ja-JP", "监管条件代码_jp", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packaging.regulatoryconditioncode
            new TranslationSeedItem("entity.packaging.regulatoryconditioncode", "zh-CN", "监管条件代码", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),
            // entity.packaging.regulatoryconditioncode
            new TranslationSeedItem("entity.packaging.regulatoryconditioncode", "zh-HK", "监管条件代码_hk", "监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）"),

            // entity.packaging.tariffratetype
            new TranslationSeedItem("entity.packaging.tariffratetype", "en-US", "税率/协定税率标识_us", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packaging.tariffratetype
            new TranslationSeedItem("entity.packaging.tariffratetype", "ja-JP", "税率/协定税率标识_jp", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packaging.tariffratetype
            new TranslationSeedItem("entity.packaging.tariffratetype", "zh-CN", "税率/协定税率标识", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),
            // entity.packaging.tariffratetype
            new TranslationSeedItem("entity.packaging.tariffratetype", "zh-HK", "税率/协定税率标识_hk", "税率/协定税率标识（记录适用的关税税率类型，便于成本核算）"),

            // entity.packaging.grossweight
            new TranslationSeedItem("entity.packaging.grossweight", "en-US", "毛重_us", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packaging.grossweight
            new TranslationSeedItem("entity.packaging.grossweight", "ja-JP", "毛重_jp", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packaging.grossweight
            new TranslationSeedItem("entity.packaging.grossweight", "zh-CN", "毛重", "毛重（包含包装物的总重量，单位：千克）"),
            // entity.packaging.grossweight
            new TranslationSeedItem("entity.packaging.grossweight", "zh-HK", "毛重_hk", "毛重（包含包装物的总重量，单位：千克）"),

            // entity.packaging.netweight
            new TranslationSeedItem("entity.packaging.netweight", "en-US", "净重_us", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packaging.netweight
            new TranslationSeedItem("entity.packaging.netweight", "ja-JP", "净重_jp", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packaging.netweight
            new TranslationSeedItem("entity.packaging.netweight", "zh-CN", "净重", "净重（不含包装物的净重量，单位：千克）"),
            // entity.packaging.netweight
            new TranslationSeedItem("entity.packaging.netweight", "zh-HK", "净重_hk", "净重（不含包装物的净重量，单位：千克）"),

            // entity.packaging.weightunit
            new TranslationSeedItem("entity.packaging.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_unit_of_measure_code，DictValue=KG/G/T 等；默认 KG）"),
            // entity.packaging.weightunit
            new TranslationSeedItem("entity.packaging.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_unit_of_measure_code，DictValue=KG/G/T 等；默认 KG）"),
            // entity.packaging.weightunit
            new TranslationSeedItem("entity.packaging.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_unit_of_measure_code，DictValue=KG/G/T 等；默认 KG）"),
            // entity.packaging.weightunit
            new TranslationSeedItem("entity.packaging.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_unit_of_measure_code，DictValue=KG/G/T 等；默认 KG）"),

            // entity.packaging.businessvolume
            new TranslationSeedItem("entity.packaging.businessvolume", "en-US", "业务量_us", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packaging.businessvolume
            new TranslationSeedItem("entity.packaging.businessvolume", "ja-JP", "业务量_jp", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packaging.businessvolume
            new TranslationSeedItem("entity.packaging.businessvolume", "zh-CN", "业务量", "业务量/容积（一个包装单位的体积，单位：立方米）"),
            // entity.packaging.businessvolume
            new TranslationSeedItem("entity.packaging.businessvolume", "zh-HK", "业务量_hk", "业务量/容积（一个包装单位的体积，单位：立方米）"),

            // entity.packaging.volumeunit
            new TranslationSeedItem("entity.packaging.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_unit_of_measure_code，DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packaging.volumeunit
            new TranslationSeedItem("entity.packaging.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_unit_of_measure_code，DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packaging.volumeunit
            new TranslationSeedItem("entity.packaging.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_unit_of_measure_code，DictValue=M3/L/ML 等；默认 M3）"),
            // entity.packaging.volumeunit
            new TranslationSeedItem("entity.packaging.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_unit_of_measure_code，DictValue=M3/L/ML 等；默认 M3）"),

            // entity.packaging.sizedimension
            new TranslationSeedItem("entity.packaging.sizedimension", "en-US", "大小/量纲_us", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packaging.sizedimension
            new TranslationSeedItem("entity.packaging.sizedimension", "ja-JP", "大小/量纲_jp", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packaging.sizedimension
            new TranslationSeedItem("entity.packaging.sizedimension", "zh-CN", "大小/量纲", "大小/量纲（尺寸量纲或大小规格）"),
            // entity.packaging.sizedimension
            new TranslationSeedItem("entity.packaging.sizedimension", "zh-HK", "大小/量纲_hk", "大小/量纲（尺寸量纲或大小规格）"),

            // entity.packaging.type
            new TranslationSeedItem("entity.packaging.type", "en-US", "包装类型_us", "包装类型（字典 logistics_material_type，DictValue=VERP 等；默认 VERP）"),
            // entity.packaging.type
            new TranslationSeedItem("entity.packaging.type", "ja-JP", "包装类型_jp", "包装类型（字典 logistics_material_type，DictValue=VERP 等；默认 VERP）"),
            // entity.packaging.type
            new TranslationSeedItem("entity.packaging.type", "zh-CN", "包装类型", "包装类型（字典 logistics_material_type，DictValue=VERP 等；默认 VERP）"),
            // entity.packaging.type
            new TranslationSeedItem("entity.packaging.type", "zh-HK", "包装类型_hk", "包装类型（字典 logistics_material_type，DictValue=VERP 等；默认 VERP）"),

            // entity.packaging.packingunit
            new TranslationSeedItem("entity.packaging.packingunit", "en-US", "包装单位_us", "包装单位（字典 logistics_unit_of_measure_code，DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packaging.packingunit
            new TranslationSeedItem("entity.packaging.packingunit", "ja-JP", "包装单位_jp", "包装单位（字典 logistics_unit_of_measure_code，DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packaging.packingunit
            new TranslationSeedItem("entity.packaging.packingunit", "zh-CN", "包装单位", "包装单位（字典 logistics_unit_of_measure_code，DictValue=CAR/CT 等；默认 CAR）"),
            // entity.packaging.packingunit
            new TranslationSeedItem("entity.packaging.packingunit", "zh-HK", "包装单位_hk", "包装单位（字典 logistics_unit_of_measure_code，DictValue=CAR/CT 等；默认 CAR）"),

            // entity.packaging.quantityperpacking
            new TranslationSeedItem("entity.packaging.quantityperpacking", "en-US", "每包装数量_us", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packaging.quantityperpacking
            new TranslationSeedItem("entity.packaging.quantityperpacking", "ja-JP", "每包装数量_jp", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packaging.quantityperpacking
            new TranslationSeedItem("entity.packaging.quantityperpacking", "zh-CN", "每包装数量", "每包装数量（一个包装包含的基本单位数量）"),
            // entity.packaging.quantityperpacking
            new TranslationSeedItem("entity.packaging.quantityperpacking", "zh-HK", "每包装数量_hk", "每包装数量（一个包装包含的基本单位数量）"),

            // entity.packaging.spec
            new TranslationSeedItem("entity.packaging.spec", "en-US", "包装规格_us", "包装规格"),
            // entity.packaging.spec
            new TranslationSeedItem("entity.packaging.spec", "ja-JP", "包装规格_jp", "包装规格"),
            // entity.packaging.spec
            new TranslationSeedItem("entity.packaging.spec", "zh-CN", "包装规格", "包装规格"),
            // entity.packaging.spec
            new TranslationSeedItem("entity.packaging.spec", "zh-HK", "包装规格_hk", "包装规格"),

            // entity.packaging.description
            new TranslationSeedItem("entity.packaging.description", "en-US", "包装描述_us", "包装描述"),
            // entity.packaging.description
            new TranslationSeedItem("entity.packaging.description", "ja-JP", "包装描述_jp", "包装描述"),
            // entity.packaging.description
            new TranslationSeedItem("entity.packaging.description", "zh-CN", "包装描述", "包装描述"),
            // entity.packaging.description
            new TranslationSeedItem("entity.packaging.description", "zh-HK", "包装描述_hk", "包装描述"),

            // entity.packaging.sortorder
            new TranslationSeedItem("entity.packaging.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.packaging.sortorder
            new TranslationSeedItem("entity.packaging.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.packaging.sortorder
            new TranslationSeedItem("entity.packaging.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.packaging.sortorder
            new TranslationSeedItem("entity.packaging.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),
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
