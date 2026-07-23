// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialI18nSeedData.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterial 实体国际化翻译种子（键前缀 entity.material.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 material 实体翻译...", tenantCode);

        foreach (var item in GetMaterialTranslations())
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

        TaktLogger.Information("TaktMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.material._self / entity.material.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "en-US", "Material Information_us", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "ja-JP", "Takt全局物料信息_jp", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-CN", "Takt全局物料信息", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-HK", "Takt全局物料信息_hk", "实体名称"),

            // entity.material.code
            new TranslationSeedItem("entity.material.code", "en-US", "物料编码_us", "物料编码（SAP MARA.MATNR）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "ja-JP", "物料编码_jp", "物料编码（SAP MARA.MATNR）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-CN", "物料编码", "物料编码（SAP MARA.MATNR）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-HK", "物料编码_hk", "物料编码（SAP MARA.MATNR）"),

            // entity.material.completemaintenancestatus
            new TranslationSeedItem("entity.material.completemaintenancestatus", "en-US", "完整维护状态_us", "完整维护状态（SAP MARA.VPSTA）"),
            // entity.material.completemaintenancestatus
            new TranslationSeedItem("entity.material.completemaintenancestatus", "ja-JP", "完整维护状态_jp", "完整维护状态（SAP MARA.VPSTA）"),
            // entity.material.completemaintenancestatus
            new TranslationSeedItem("entity.material.completemaintenancestatus", "zh-CN", "完整维护状态", "完整维护状态（SAP MARA.VPSTA）"),
            // entity.material.completemaintenancestatus
            new TranslationSeedItem("entity.material.completemaintenancestatus", "zh-HK", "完整维护状态_hk", "完整维护状态（SAP MARA.VPSTA）"),

            // entity.material.maintenancestatus
            new TranslationSeedItem("entity.material.maintenancestatus", "en-US", "维护状态_us", "维护状态（SAP MARA.PSTAT）"),
            // entity.material.maintenancestatus
            new TranslationSeedItem("entity.material.maintenancestatus", "ja-JP", "维护状态_jp", "维护状态（SAP MARA.PSTAT）"),
            // entity.material.maintenancestatus
            new TranslationSeedItem("entity.material.maintenancestatus", "zh-CN", "维护状态", "维护状态（SAP MARA.PSTAT）"),
            // entity.material.maintenancestatus
            new TranslationSeedItem("entity.material.maintenancestatus", "zh-HK", "维护状态_hk", "维护状态（SAP MARA.PSTAT）"),

            // entity.material.clientdeletionflag
            new TranslationSeedItem("entity.material.clientdeletionflag", "en-US", "客户级删除标记_us", "客户级删除标记（SAP MARA.LVORM）"),
            // entity.material.clientdeletionflag
            new TranslationSeedItem("entity.material.clientdeletionflag", "ja-JP", "客户级删除标记_jp", "客户级删除标记（SAP MARA.LVORM）"),
            // entity.material.clientdeletionflag
            new TranslationSeedItem("entity.material.clientdeletionflag", "zh-CN", "客户级删除标记", "客户级删除标记（SAP MARA.LVORM）"),
            // entity.material.clientdeletionflag
            new TranslationSeedItem("entity.material.clientdeletionflag", "zh-HK", "客户级删除标记_hk", "客户级删除标记（SAP MARA.LVORM）"),

            // entity.material.type
            new TranslationSeedItem("entity.material.type", "en-US", "物料类型_us", "物料类型（SAP MARA.MTART）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "ja-JP", "物料类型_jp", "物料类型（SAP MARA.MTART）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-CN", "物料类型", "物料类型（SAP MARA.MTART）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-HK", "物料类型_hk", "物料类型（SAP MARA.MTART）"),

            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "en-US", "行业领域_us", "行业领域（SAP MARA.MBRSH）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "ja-JP", "行业领域_jp", "行业领域（SAP MARA.MBRSH）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-CN", "行业领域", "行业领域（SAP MARA.MBRSH）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-HK", "行业领域_hk", "行业领域（SAP MARA.MBRSH）"),

            // entity.material.group
            new TranslationSeedItem("entity.material.group", "en-US", "物料组_us", "物料组（SAP MARA.MATKL）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "ja-JP", "物料组_jp", "物料组（SAP MARA.MATKL）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "zh-CN", "物料组", "物料组（SAP MARA.MATKL）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "zh-HK", "物料组_hk", "物料组（SAP MARA.MATKL）"),

            // entity.material.oldmaterialnumber
            new TranslationSeedItem("entity.material.oldmaterialnumber", "en-US", "旧物料号_us", "旧物料号（SAP MARA.BISMT）"),
            // entity.material.oldmaterialnumber
            new TranslationSeedItem("entity.material.oldmaterialnumber", "ja-JP", "旧物料号_jp", "旧物料号（SAP MARA.BISMT）"),
            // entity.material.oldmaterialnumber
            new TranslationSeedItem("entity.material.oldmaterialnumber", "zh-CN", "旧物料号", "旧物料号（SAP MARA.BISMT）"),
            // entity.material.oldmaterialnumber
            new TranslationSeedItem("entity.material.oldmaterialnumber", "zh-HK", "旧物料号_hk", "旧物料号（SAP MARA.BISMT）"),

            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "en-US", "基本计量单位_us", "基本计量单位（SAP MARA.MEINS）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "ja-JP", "基本计量单位_jp", "基本计量单位（SAP MARA.MEINS）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-CN", "基本计量单位", "基本计量单位（SAP MARA.MEINS）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-HK", "基本计量单位_hk", "基本计量单位（SAP MARA.MEINS）"),

            // entity.material.orderunit
            new TranslationSeedItem("entity.material.orderunit", "en-US", "采购订单单位_us", "采购订单单位（SAP MARA.BSTME）"),
            // entity.material.orderunit
            new TranslationSeedItem("entity.material.orderunit", "ja-JP", "采购订单单位_jp", "采购订单单位（SAP MARA.BSTME）"),
            // entity.material.orderunit
            new TranslationSeedItem("entity.material.orderunit", "zh-CN", "采购订单单位", "采购订单单位（SAP MARA.BSTME）"),
            // entity.material.orderunit
            new TranslationSeedItem("entity.material.orderunit", "zh-HK", "采购订单单位_hk", "采购订单单位（SAP MARA.BSTME）"),

            // entity.material.documentnumber
            new TranslationSeedItem("entity.material.documentnumber", "en-US", "单据号_us", "单据号（SAP MARA.ZEINR）"),
            // entity.material.documentnumber
            new TranslationSeedItem("entity.material.documentnumber", "ja-JP", "单据号_jp", "单据号（SAP MARA.ZEINR）"),
            // entity.material.documentnumber
            new TranslationSeedItem("entity.material.documentnumber", "zh-CN", "单据号", "单据号（SAP MARA.ZEINR）"),
            // entity.material.documentnumber
            new TranslationSeedItem("entity.material.documentnumber", "zh-HK", "单据号_hk", "单据号（SAP MARA.ZEINR）"),

            // entity.material.documenttype
            new TranslationSeedItem("entity.material.documenttype", "en-US", "单据类型_us", "单据类型（SAP MARA.ZEIAR）"),
            // entity.material.documenttype
            new TranslationSeedItem("entity.material.documenttype", "ja-JP", "单据类型_jp", "单据类型（SAP MARA.ZEIAR）"),
            // entity.material.documenttype
            new TranslationSeedItem("entity.material.documenttype", "zh-CN", "单据类型", "单据类型（SAP MARA.ZEIAR）"),
            // entity.material.documenttype
            new TranslationSeedItem("entity.material.documenttype", "zh-HK", "单据类型_hk", "单据类型（SAP MARA.ZEIAR）"),

            // entity.material.documentversion
            new TranslationSeedItem("entity.material.documentversion", "en-US", "单据版本_us", "单据版本（SAP MARA.ZEIVR）"),
            // entity.material.documentversion
            new TranslationSeedItem("entity.material.documentversion", "ja-JP", "单据版本_jp", "单据版本（SAP MARA.ZEIVR）"),
            // entity.material.documentversion
            new TranslationSeedItem("entity.material.documentversion", "zh-CN", "单据版本", "单据版本（SAP MARA.ZEIVR）"),
            // entity.material.documentversion
            new TranslationSeedItem("entity.material.documentversion", "zh-HK", "单据版本_hk", "单据版本（SAP MARA.ZEIVR）"),

            // entity.material.documentpageformat
            new TranslationSeedItem("entity.material.documentpageformat", "en-US", "单据页格式_us", "单据页格式（SAP MARA.ZEIFO）"),
            // entity.material.documentpageformat
            new TranslationSeedItem("entity.material.documentpageformat", "ja-JP", "单据页格式_jp", "单据页格式（SAP MARA.ZEIFO）"),
            // entity.material.documentpageformat
            new TranslationSeedItem("entity.material.documentpageformat", "zh-CN", "单据页格式", "单据页格式（SAP MARA.ZEIFO）"),
            // entity.material.documentpageformat
            new TranslationSeedItem("entity.material.documentpageformat", "zh-HK", "单据页格式_hk", "单据页格式（SAP MARA.ZEIFO）"),

            // entity.material.documentchangenumber
            new TranslationSeedItem("entity.material.documentchangenumber", "en-US", "单据更改号_us", "单据更改号（SAP MARA.AESZN）"),
            // entity.material.documentchangenumber
            new TranslationSeedItem("entity.material.documentchangenumber", "ja-JP", "单据更改号_jp", "单据更改号（SAP MARA.AESZN）"),
            // entity.material.documentchangenumber
            new TranslationSeedItem("entity.material.documentchangenumber", "zh-CN", "单据更改号", "单据更改号（SAP MARA.AESZN）"),
            // entity.material.documentchangenumber
            new TranslationSeedItem("entity.material.documentchangenumber", "zh-HK", "单据更改号_hk", "单据更改号（SAP MARA.AESZN）"),

            // entity.material.documentpagenumber
            new TranslationSeedItem("entity.material.documentpagenumber", "en-US", "单据页号_us", "单据页号（SAP MARA.BLATT）"),
            // entity.material.documentpagenumber
            new TranslationSeedItem("entity.material.documentpagenumber", "ja-JP", "单据页号_jp", "单据页号（SAP MARA.BLATT）"),
            // entity.material.documentpagenumber
            new TranslationSeedItem("entity.material.documentpagenumber", "zh-CN", "单据页号", "单据页号（SAP MARA.BLATT）"),
            // entity.material.documentpagenumber
            new TranslationSeedItem("entity.material.documentpagenumber", "zh-HK", "单据页号_hk", "单据页号（SAP MARA.BLATT）"),

            // entity.material.documentsheetcount
            new TranslationSeedItem("entity.material.documentsheetcount", "en-US", "单据页数_us", "单据页数（SAP MARA.BLANZ）"),
            // entity.material.documentsheetcount
            new TranslationSeedItem("entity.material.documentsheetcount", "ja-JP", "单据页数_jp", "单据页数（SAP MARA.BLANZ）"),
            // entity.material.documentsheetcount
            new TranslationSeedItem("entity.material.documentsheetcount", "zh-CN", "单据页数", "单据页数（SAP MARA.BLANZ）"),
            // entity.material.documentsheetcount
            new TranslationSeedItem("entity.material.documentsheetcount", "zh-HK", "单据页数_hk", "单据页数（SAP MARA.BLANZ）"),

            // entity.material.productioninspectionmemo
            new TranslationSeedItem("entity.material.productioninspectionmemo", "en-US", "生产/检验备忘_us", "生产/检验备忘（SAP MARA.FERTH）"),
            // entity.material.productioninspectionmemo
            new TranslationSeedItem("entity.material.productioninspectionmemo", "ja-JP", "生产/检验备忘_jp", "生产/检验备忘（SAP MARA.FERTH）"),
            // entity.material.productioninspectionmemo
            new TranslationSeedItem("entity.material.productioninspectionmemo", "zh-CN", "生产/检验备忘", "生产/检验备忘（SAP MARA.FERTH）"),
            // entity.material.productioninspectionmemo
            new TranslationSeedItem("entity.material.productioninspectionmemo", "zh-HK", "生产/检验备忘_hk", "生产/检验备忘（SAP MARA.FERTH）"),

            // entity.material.productionmemopageformat
            new TranslationSeedItem("entity.material.productionmemopageformat", "en-US", "生产备忘页格式_us", "生产备忘页格式（SAP MARA.FORMT）"),
            // entity.material.productionmemopageformat
            new TranslationSeedItem("entity.material.productionmemopageformat", "ja-JP", "生产备忘页格式_jp", "生产备忘页格式（SAP MARA.FORMT）"),
            // entity.material.productionmemopageformat
            new TranslationSeedItem("entity.material.productionmemopageformat", "zh-CN", "生产备忘页格式", "生产备忘页格式（SAP MARA.FORMT）"),
            // entity.material.productionmemopageformat
            new TranslationSeedItem("entity.material.productionmemopageformat", "zh-HK", "生产备忘页格式_hk", "生产备忘页格式（SAP MARA.FORMT）"),

            // entity.material.sizedimensions
            new TranslationSeedItem("entity.material.sizedimensions", "en-US", "尺寸/规格_us", "尺寸/规格（SAP MARA.GROES）"),
            // entity.material.sizedimensions
            new TranslationSeedItem("entity.material.sizedimensions", "ja-JP", "尺寸/规格_jp", "尺寸/规格（SAP MARA.GROES）"),
            // entity.material.sizedimensions
            new TranslationSeedItem("entity.material.sizedimensions", "zh-CN", "尺寸/规格", "尺寸/规格（SAP MARA.GROES）"),
            // entity.material.sizedimensions
            new TranslationSeedItem("entity.material.sizedimensions", "zh-HK", "尺寸/规格_hk", "尺寸/规格（SAP MARA.GROES）"),

            // entity.material.basicmaterial
            new TranslationSeedItem("entity.material.basicmaterial", "en-US", "基本物料（材质）_us", "基本物料（材质）（SAP MARA.WRKST）"),
            // entity.material.basicmaterial
            new TranslationSeedItem("entity.material.basicmaterial", "ja-JP", "基本物料（材质）_jp", "基本物料（材质）（SAP MARA.WRKST）"),
            // entity.material.basicmaterial
            new TranslationSeedItem("entity.material.basicmaterial", "zh-CN", "基本物料（材质）", "基本物料（材质）（SAP MARA.WRKST）"),
            // entity.material.basicmaterial
            new TranslationSeedItem("entity.material.basicmaterial", "zh-HK", "基本物料（材质）_hk", "基本物料（材质）（SAP MARA.WRKST）"),

            // entity.material.industrystandarddescription
            new TranslationSeedItem("entity.material.industrystandarddescription", "en-US", "行业标准描述_us", "行业标准描述（SAP MARA.NORMT）"),
            // entity.material.industrystandarddescription
            new TranslationSeedItem("entity.material.industrystandarddescription", "ja-JP", "行业标准描述_jp", "行业标准描述（SAP MARA.NORMT）"),
            // entity.material.industrystandarddescription
            new TranslationSeedItem("entity.material.industrystandarddescription", "zh-CN", "行业标准描述", "行业标准描述（SAP MARA.NORMT）"),
            // entity.material.industrystandarddescription
            new TranslationSeedItem("entity.material.industrystandarddescription", "zh-HK", "行业标准描述_hk", "行业标准描述（SAP MARA.NORMT）"),

            // entity.material.laboratorydesignoffice
            new TranslationSeedItem("entity.material.laboratorydesignoffice", "en-US", "实验室/设计室_us", "实验室/设计室（SAP MARA.LABOR）"),
            // entity.material.laboratorydesignoffice
            new TranslationSeedItem("entity.material.laboratorydesignoffice", "ja-JP", "实验室/设计室_jp", "实验室/设计室（SAP MARA.LABOR）"),
            // entity.material.laboratorydesignoffice
            new TranslationSeedItem("entity.material.laboratorydesignoffice", "zh-CN", "实验室/设计室", "实验室/设计室（SAP MARA.LABOR）"),
            // entity.material.laboratorydesignoffice
            new TranslationSeedItem("entity.material.laboratorydesignoffice", "zh-HK", "实验室/设计室_hk", "实验室/设计室（SAP MARA.LABOR）"),

            // entity.material.purchasingvaluekey
            new TranslationSeedItem("entity.material.purchasingvaluekey", "en-US", "采购价值码_us", "采购价值码（SAP MARA.EKWSL）"),
            // entity.material.purchasingvaluekey
            new TranslationSeedItem("entity.material.purchasingvaluekey", "ja-JP", "采购价值码_jp", "采购价值码（SAP MARA.EKWSL）"),
            // entity.material.purchasingvaluekey
            new TranslationSeedItem("entity.material.purchasingvaluekey", "zh-CN", "采购价值码", "采购价值码（SAP MARA.EKWSL）"),
            // entity.material.purchasingvaluekey
            new TranslationSeedItem("entity.material.purchasingvaluekey", "zh-HK", "采购价值码_hk", "采购价值码（SAP MARA.EKWSL）"),

            // entity.material.grossweight
            new TranslationSeedItem("entity.material.grossweight", "en-US", "毛重_us", "毛重（SAP MARA.BRGEW）"),
            // entity.material.grossweight
            new TranslationSeedItem("entity.material.grossweight", "ja-JP", "毛重_jp", "毛重（SAP MARA.BRGEW）"),
            // entity.material.grossweight
            new TranslationSeedItem("entity.material.grossweight", "zh-CN", "毛重", "毛重（SAP MARA.BRGEW）"),
            // entity.material.grossweight
            new TranslationSeedItem("entity.material.grossweight", "zh-HK", "毛重_hk", "毛重（SAP MARA.BRGEW）"),

            // entity.material.netweight
            new TranslationSeedItem("entity.material.netweight", "en-US", "净重_us", "净重（SAP MARA.NTGEW）"),
            // entity.material.netweight
            new TranslationSeedItem("entity.material.netweight", "ja-JP", "净重_jp", "净重（SAP MARA.NTGEW）"),
            // entity.material.netweight
            new TranslationSeedItem("entity.material.netweight", "zh-CN", "净重", "净重（SAP MARA.NTGEW）"),
            // entity.material.netweight
            new TranslationSeedItem("entity.material.netweight", "zh-HK", "净重_hk", "净重（SAP MARA.NTGEW）"),

            // entity.material.weightunit
            new TranslationSeedItem("entity.material.weightunit", "en-US", "重量单位_us", "重量单位（SAP MARA.GEWEI）"),
            // entity.material.weightunit
            new TranslationSeedItem("entity.material.weightunit", "ja-JP", "重量单位_jp", "重量单位（SAP MARA.GEWEI）"),
            // entity.material.weightunit
            new TranslationSeedItem("entity.material.weightunit", "zh-CN", "重量单位", "重量单位（SAP MARA.GEWEI）"),
            // entity.material.weightunit
            new TranslationSeedItem("entity.material.weightunit", "zh-HK", "重量单位_hk", "重量单位（SAP MARA.GEWEI）"),

            // entity.material.volume
            new TranslationSeedItem("entity.material.volume", "en-US", "体积_us", "体积（SAP MARA.VOLUM）"),
            // entity.material.volume
            new TranslationSeedItem("entity.material.volume", "ja-JP", "体积_jp", "体积（SAP MARA.VOLUM）"),
            // entity.material.volume
            new TranslationSeedItem("entity.material.volume", "zh-CN", "体积", "体积（SAP MARA.VOLUM）"),
            // entity.material.volume
            new TranslationSeedItem("entity.material.volume", "zh-HK", "体积_hk", "体积（SAP MARA.VOLUM）"),

            // entity.material.volumeunit
            new TranslationSeedItem("entity.material.volumeunit", "en-US", "体积单位_us", "体积单位（SAP MARA.VOLEH）"),
            // entity.material.volumeunit
            new TranslationSeedItem("entity.material.volumeunit", "ja-JP", "体积单位_jp", "体积单位（SAP MARA.VOLEH）"),
            // entity.material.volumeunit
            new TranslationSeedItem("entity.material.volumeunit", "zh-CN", "体积单位", "体积单位（SAP MARA.VOLEH）"),
            // entity.material.volumeunit
            new TranslationSeedItem("entity.material.volumeunit", "zh-HK", "体积单位_hk", "体积单位（SAP MARA.VOLEH）"),

            // entity.material.containerrequirements
            new TranslationSeedItem("entity.material.containerrequirements", "en-US", "容器要求_us", "容器要求（SAP MARA.BEHVO）"),
            // entity.material.containerrequirements
            new TranslationSeedItem("entity.material.containerrequirements", "ja-JP", "容器要求_jp", "容器要求（SAP MARA.BEHVO）"),
            // entity.material.containerrequirements
            new TranslationSeedItem("entity.material.containerrequirements", "zh-CN", "容器要求", "容器要求（SAP MARA.BEHVO）"),
            // entity.material.containerrequirements
            new TranslationSeedItem("entity.material.containerrequirements", "zh-HK", "容器要求_hk", "容器要求（SAP MARA.BEHVO）"),

            // entity.material.storageconditions
            new TranslationSeedItem("entity.material.storageconditions", "en-US", "仓储条件_us", "仓储条件（SAP MARA.RAUBE）"),
            // entity.material.storageconditions
            new TranslationSeedItem("entity.material.storageconditions", "ja-JP", "仓储条件_jp", "仓储条件（SAP MARA.RAUBE）"),
            // entity.material.storageconditions
            new TranslationSeedItem("entity.material.storageconditions", "zh-CN", "仓储条件", "仓储条件（SAP MARA.RAUBE）"),
            // entity.material.storageconditions
            new TranslationSeedItem("entity.material.storageconditions", "zh-HK", "仓储条件_hk", "仓储条件（SAP MARA.RAUBE）"),

            // entity.material.temperatureconditions
            new TranslationSeedItem("entity.material.temperatureconditions", "en-US", "温度条件_us", "温度条件（SAP MARA.TEMPB）"),
            // entity.material.temperatureconditions
            new TranslationSeedItem("entity.material.temperatureconditions", "ja-JP", "温度条件_jp", "温度条件（SAP MARA.TEMPB）"),
            // entity.material.temperatureconditions
            new TranslationSeedItem("entity.material.temperatureconditions", "zh-CN", "温度条件", "温度条件（SAP MARA.TEMPB）"),
            // entity.material.temperatureconditions
            new TranslationSeedItem("entity.material.temperatureconditions", "zh-HK", "温度条件_hk", "温度条件（SAP MARA.TEMPB）"),

            // entity.material.lowlevelcode
            new TranslationSeedItem("entity.material.lowlevelcode", "en-US", "低层码_us", "低层码（SAP MARA.DISST）"),
            // entity.material.lowlevelcode
            new TranslationSeedItem("entity.material.lowlevelcode", "ja-JP", "低层码_jp", "低层码（SAP MARA.DISST）"),
            // entity.material.lowlevelcode
            new TranslationSeedItem("entity.material.lowlevelcode", "zh-CN", "低层码", "低层码（SAP MARA.DISST）"),
            // entity.material.lowlevelcode
            new TranslationSeedItem("entity.material.lowlevelcode", "zh-HK", "低层码_hk", "低层码（SAP MARA.DISST）"),

            // entity.material.transportationgroup
            new TranslationSeedItem("entity.material.transportationgroup", "en-US", "运输组_us", "运输组（SAP MARA.TRAGR）"),
            // entity.material.transportationgroup
            new TranslationSeedItem("entity.material.transportationgroup", "ja-JP", "运输组_jp", "运输组（SAP MARA.TRAGR）"),
            // entity.material.transportationgroup
            new TranslationSeedItem("entity.material.transportationgroup", "zh-CN", "运输组", "运输组（SAP MARA.TRAGR）"),
            // entity.material.transportationgroup
            new TranslationSeedItem("entity.material.transportationgroup", "zh-HK", "运输组_hk", "运输组（SAP MARA.TRAGR）"),

            // entity.material.hazardousmaterialnumber
            new TranslationSeedItem("entity.material.hazardousmaterialnumber", "en-US", "危险品编码_us", "危险品编码（SAP MARA.STOFF）"),
            // entity.material.hazardousmaterialnumber
            new TranslationSeedItem("entity.material.hazardousmaterialnumber", "ja-JP", "危险品编码_jp", "危险品编码（SAP MARA.STOFF）"),
            // entity.material.hazardousmaterialnumber
            new TranslationSeedItem("entity.material.hazardousmaterialnumber", "zh-CN", "危险品编码", "危险品编码（SAP MARA.STOFF）"),
            // entity.material.hazardousmaterialnumber
            new TranslationSeedItem("entity.material.hazardousmaterialnumber", "zh-HK", "危险品编码_hk", "危险品编码（SAP MARA.STOFF）"),

            // entity.material.division
            new TranslationSeedItem("entity.material.division", "en-US", "产品组_us", "产品组（SAP MARA.SPART）"),
            // entity.material.division
            new TranslationSeedItem("entity.material.division", "ja-JP", "产品组_jp", "产品组（SAP MARA.SPART）"),
            // entity.material.division
            new TranslationSeedItem("entity.material.division", "zh-CN", "产品组", "产品组（SAP MARA.SPART）"),
            // entity.material.division
            new TranslationSeedItem("entity.material.division", "zh-HK", "产品组_hk", "产品组（SAP MARA.SPART）"),

            // entity.material.competitor
            new TranslationSeedItem("entity.material.competitor", "en-US", "竞争对手_us", "竞争对手（SAP MARA.KUNNR）"),
            // entity.material.competitor
            new TranslationSeedItem("entity.material.competitor", "ja-JP", "竞争对手_jp", "竞争对手（SAP MARA.KUNNR）"),
            // entity.material.competitor
            new TranslationSeedItem("entity.material.competitor", "zh-CN", "竞争对手", "竞争对手（SAP MARA.KUNNR）"),
            // entity.material.competitor
            new TranslationSeedItem("entity.material.competitor", "zh-HK", "竞争对手_hk", "竞争对手（SAP MARA.KUNNR）"),

            // entity.material.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.material.europeanarticlenumberobsolete", "en-US", "欧洲商品号（旧）_us", "欧洲商品号（旧）（SAP MARA.EANNR）"),
            // entity.material.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.material.europeanarticlenumberobsolete", "ja-JP", "欧洲商品号（旧）_jp", "欧洲商品号（旧）（SAP MARA.EANNR）"),
            // entity.material.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.material.europeanarticlenumberobsolete", "zh-CN", "欧洲商品号（旧）", "欧洲商品号（旧）（SAP MARA.EANNR）"),
            // entity.material.europeanarticlenumberobsolete
            new TranslationSeedItem("entity.material.europeanarticlenumberobsolete", "zh-HK", "欧洲商品号（旧）_hk", "欧洲商品号（旧）（SAP MARA.EANNR）"),

            // entity.material.grgislipquantity
            new TranslationSeedItem("entity.material.grgislipquantity", "en-US", "收发货凭证打印数量_us", "收发货凭证打印数量（SAP MARA.WESCH）"),
            // entity.material.grgislipquantity
            new TranslationSeedItem("entity.material.grgislipquantity", "ja-JP", "收发货凭证打印数量_jp", "收发货凭证打印数量（SAP MARA.WESCH）"),
            // entity.material.grgislipquantity
            new TranslationSeedItem("entity.material.grgislipquantity", "zh-CN", "收发货凭证打印数量", "收发货凭证打印数量（SAP MARA.WESCH）"),
            // entity.material.grgislipquantity
            new TranslationSeedItem("entity.material.grgislipquantity", "zh-HK", "收发货凭证打印数量_hk", "收发货凭证打印数量（SAP MARA.WESCH）"),

            // entity.material.procurementrule
            new TranslationSeedItem("entity.material.procurementrule", "en-US", "采购规则_us", "采购规则（SAP MARA.BWVOR）"),
            // entity.material.procurementrule
            new TranslationSeedItem("entity.material.procurementrule", "ja-JP", "采购规则_jp", "采购规则（SAP MARA.BWVOR）"),
            // entity.material.procurementrule
            new TranslationSeedItem("entity.material.procurementrule", "zh-CN", "采购规则", "采购规则（SAP MARA.BWVOR）"),
            // entity.material.procurementrule
            new TranslationSeedItem("entity.material.procurementrule", "zh-HK", "采购规则_hk", "采购规则（SAP MARA.BWVOR）"),

            // entity.material.sourceofsupply
            new TranslationSeedItem("entity.material.sourceofsupply", "en-US", "货源_us", "货源（SAP MARA.BWSCL）"),
            // entity.material.sourceofsupply
            new TranslationSeedItem("entity.material.sourceofsupply", "ja-JP", "货源_jp", "货源（SAP MARA.BWSCL）"),
            // entity.material.sourceofsupply
            new TranslationSeedItem("entity.material.sourceofsupply", "zh-CN", "货源", "货源（SAP MARA.BWSCL）"),
            // entity.material.sourceofsupply
            new TranslationSeedItem("entity.material.sourceofsupply", "zh-HK", "货源_hk", "货源（SAP MARA.BWSCL）"),

            // entity.material.seasoncategory
            new TranslationSeedItem("entity.material.seasoncategory", "en-US", "季节类别_us", "季节类别（SAP MARA.SAISO）"),
            // entity.material.seasoncategory
            new TranslationSeedItem("entity.material.seasoncategory", "ja-JP", "季节类别_jp", "季节类别（SAP MARA.SAISO）"),
            // entity.material.seasoncategory
            new TranslationSeedItem("entity.material.seasoncategory", "zh-CN", "季节类别", "季节类别（SAP MARA.SAISO）"),
            // entity.material.seasoncategory
            new TranslationSeedItem("entity.material.seasoncategory", "zh-HK", "季节类别_hk", "季节类别（SAP MARA.SAISO）"),

            // entity.material.labeltype
            new TranslationSeedItem("entity.material.labeltype", "en-US", "标签类型_us", "标签类型（SAP MARA.ETIAR）"),
            // entity.material.labeltype
            new TranslationSeedItem("entity.material.labeltype", "ja-JP", "标签类型_jp", "标签类型（SAP MARA.ETIAR）"),
            // entity.material.labeltype
            new TranslationSeedItem("entity.material.labeltype", "zh-CN", "标签类型", "标签类型（SAP MARA.ETIAR）"),
            // entity.material.labeltype
            new TranslationSeedItem("entity.material.labeltype", "zh-HK", "标签类型_hk", "标签类型（SAP MARA.ETIAR）"),

            // entity.material.labelform
            new TranslationSeedItem("entity.material.labelform", "en-US", "标签格式_us", "标签格式（SAP MARA.ETIFO）"),
            // entity.material.labelform
            new TranslationSeedItem("entity.material.labelform", "ja-JP", "标签格式_jp", "标签格式（SAP MARA.ETIFO）"),
            // entity.material.labelform
            new TranslationSeedItem("entity.material.labelform", "zh-CN", "标签格式", "标签格式（SAP MARA.ETIFO）"),
            // entity.material.labelform
            new TranslationSeedItem("entity.material.labelform", "zh-HK", "标签格式_hk", "标签格式（SAP MARA.ETIFO）"),

            // entity.material.deactivatedfield
            new TranslationSeedItem("entity.material.deactivatedfield", "en-US", "已停用字段_us", "已停用字段（SAP MARA.ENTAR）"),
            // entity.material.deactivatedfield
            new TranslationSeedItem("entity.material.deactivatedfield", "ja-JP", "已停用字段_jp", "已停用字段（SAP MARA.ENTAR）"),
            // entity.material.deactivatedfield
            new TranslationSeedItem("entity.material.deactivatedfield", "zh-CN", "已停用字段", "已停用字段（SAP MARA.ENTAR）"),
            // entity.material.deactivatedfield
            new TranslationSeedItem("entity.material.deactivatedfield", "zh-HK", "已停用字段_hk", "已停用字段（SAP MARA.ENTAR）"),

            // entity.material.internationalarticlenumber
            new TranslationSeedItem("entity.material.internationalarticlenumber", "en-US", "国际商品编码EAN/UPC_us", "国际商品编码EAN/UPC（SAP MARA.EAN11）"),
            // entity.material.internationalarticlenumber
            new TranslationSeedItem("entity.material.internationalarticlenumber", "ja-JP", "国际商品编码EAN/UPC_jp", "国际商品编码EAN/UPC（SAP MARA.EAN11）"),
            // entity.material.internationalarticlenumber
            new TranslationSeedItem("entity.material.internationalarticlenumber", "zh-CN", "国际商品编码EAN/UPC", "国际商品编码EAN/UPC（SAP MARA.EAN11）"),
            // entity.material.internationalarticlenumber
            new TranslationSeedItem("entity.material.internationalarticlenumber", "zh-HK", "国际商品编码EAN/UPC_hk", "国际商品编码EAN/UPC（SAP MARA.EAN11）"),

            // entity.material.eancategory
            new TranslationSeedItem("entity.material.eancategory", "en-US", "EAN类别_us", "EAN类别（SAP MARA.NUMTP）"),
            // entity.material.eancategory
            new TranslationSeedItem("entity.material.eancategory", "ja-JP", "EAN类别_jp", "EAN类别（SAP MARA.NUMTP）"),
            // entity.material.eancategory
            new TranslationSeedItem("entity.material.eancategory", "zh-CN", "EAN类别", "EAN类别（SAP MARA.NUMTP）"),
            // entity.material.eancategory
            new TranslationSeedItem("entity.material.eancategory", "zh-HK", "EAN类别_hk", "EAN类别（SAP MARA.NUMTP）"),

            // entity.material.length
            new TranslationSeedItem("entity.material.length", "en-US", "长度_us", "长度（SAP MARA.LAENG）"),
            // entity.material.length
            new TranslationSeedItem("entity.material.length", "ja-JP", "长度_jp", "长度（SAP MARA.LAENG）"),
            // entity.material.length
            new TranslationSeedItem("entity.material.length", "zh-CN", "长度", "长度（SAP MARA.LAENG）"),
            // entity.material.length
            new TranslationSeedItem("entity.material.length", "zh-HK", "长度_hk", "长度（SAP MARA.LAENG）"),

            // entity.material.width
            new TranslationSeedItem("entity.material.width", "en-US", "宽度_us", "宽度（SAP MARA.BREIT）"),
            // entity.material.width
            new TranslationSeedItem("entity.material.width", "ja-JP", "宽度_jp", "宽度（SAP MARA.BREIT）"),
            // entity.material.width
            new TranslationSeedItem("entity.material.width", "zh-CN", "宽度", "宽度（SAP MARA.BREIT）"),
            // entity.material.width
            new TranslationSeedItem("entity.material.width", "zh-HK", "宽度_hk", "宽度（SAP MARA.BREIT）"),

            // entity.material.height
            new TranslationSeedItem("entity.material.height", "en-US", "高度_us", "高度（SAP MARA.HOEHE）"),
            // entity.material.height
            new TranslationSeedItem("entity.material.height", "ja-JP", "高度_jp", "高度（SAP MARA.HOEHE）"),
            // entity.material.height
            new TranslationSeedItem("entity.material.height", "zh-CN", "高度", "高度（SAP MARA.HOEHE）"),
            // entity.material.height
            new TranslationSeedItem("entity.material.height", "zh-HK", "高度_hk", "高度（SAP MARA.HOEHE）"),

            // entity.material.dimensionunit
            new TranslationSeedItem("entity.material.dimensionunit", "en-US", "长宽高单位_us", "长宽高单位（SAP MARA.MEABM）"),
            // entity.material.dimensionunit
            new TranslationSeedItem("entity.material.dimensionunit", "ja-JP", "长宽高单位_jp", "长宽高单位（SAP MARA.MEABM）"),
            // entity.material.dimensionunit
            new TranslationSeedItem("entity.material.dimensionunit", "zh-CN", "长宽高单位", "长宽高单位（SAP MARA.MEABM）"),
            // entity.material.dimensionunit
            new TranslationSeedItem("entity.material.dimensionunit", "zh-HK", "长宽高单位_hk", "长宽高单位（SAP MARA.MEABM）"),

            // entity.material.producthierarchy
            new TranslationSeedItem("entity.material.producthierarchy", "en-US", "产品层次_us", "产品层次（SAP MARA.PRDHA）"),
            // entity.material.producthierarchy
            new TranslationSeedItem("entity.material.producthierarchy", "ja-JP", "产品层次_jp", "产品层次（SAP MARA.PRDHA）"),
            // entity.material.producthierarchy
            new TranslationSeedItem("entity.material.producthierarchy", "zh-CN", "产品层次", "产品层次（SAP MARA.PRDHA）"),
            // entity.material.producthierarchy
            new TranslationSeedItem("entity.material.producthierarchy", "zh-HK", "产品层次_hk", "产品层次（SAP MARA.PRDHA）"),

            // entity.material.stocktransfernetchangecosting
            new TranslationSeedItem("entity.material.stocktransfernetchangecosting", "en-US", "库存调拨净更改成本核算_us", "库存调拨净更改成本核算（SAP MARA.AEKLK）"),
            // entity.material.stocktransfernetchangecosting
            new TranslationSeedItem("entity.material.stocktransfernetchangecosting", "ja-JP", "库存调拨净更改成本核算_jp", "库存调拨净更改成本核算（SAP MARA.AEKLK）"),
            // entity.material.stocktransfernetchangecosting
            new TranslationSeedItem("entity.material.stocktransfernetchangecosting", "zh-CN", "库存调拨净更改成本核算", "库存调拨净更改成本核算（SAP MARA.AEKLK）"),
            // entity.material.stocktransfernetchangecosting
            new TranslationSeedItem("entity.material.stocktransfernetchangecosting", "zh-HK", "库存调拨净更改成本核算_hk", "库存调拨净更改成本核算（SAP MARA.AEKLK）"),

            // entity.material.cadindicator
            new TranslationSeedItem("entity.material.cadindicator", "en-US", "CAD标识_us", "CAD标识（SAP MARA.CADKZ）"),
            // entity.material.cadindicator
            new TranslationSeedItem("entity.material.cadindicator", "ja-JP", "CAD标识_jp", "CAD标识（SAP MARA.CADKZ）"),
            // entity.material.cadindicator
            new TranslationSeedItem("entity.material.cadindicator", "zh-CN", "CAD标识", "CAD标识（SAP MARA.CADKZ）"),
            // entity.material.cadindicator
            new TranslationSeedItem("entity.material.cadindicator", "zh-HK", "CAD标识_hk", "CAD标识（SAP MARA.CADKZ）"),

            // entity.material.qminprocurement
            new TranslationSeedItem("entity.material.qminprocurement", "en-US", "采购QM激活_us", "采购QM激活（SAP MARA.QMPUR）"),
            // entity.material.qminprocurement
            new TranslationSeedItem("entity.material.qminprocurement", "ja-JP", "采购QM激活_jp", "采购QM激活（SAP MARA.QMPUR）"),
            // entity.material.qminprocurement
            new TranslationSeedItem("entity.material.qminprocurement", "zh-CN", "采购QM激活", "采购QM激活（SAP MARA.QMPUR）"),
            // entity.material.qminprocurement
            new TranslationSeedItem("entity.material.qminprocurement", "zh-HK", "采购QM激活_hk", "采购QM激活（SAP MARA.QMPUR）"),

            // entity.material.allowedpackagingweight
            new TranslationSeedItem("entity.material.allowedpackagingweight", "en-US", "允许包装重量_us", "允许包装重量（SAP MARA.ERGEW）"),
            // entity.material.allowedpackagingweight
            new TranslationSeedItem("entity.material.allowedpackagingweight", "ja-JP", "允许包装重量_jp", "允许包装重量（SAP MARA.ERGEW）"),
            // entity.material.allowedpackagingweight
            new TranslationSeedItem("entity.material.allowedpackagingweight", "zh-CN", "允许包装重量", "允许包装重量（SAP MARA.ERGEW）"),
            // entity.material.allowedpackagingweight
            new TranslationSeedItem("entity.material.allowedpackagingweight", "zh-HK", "允许包装重量_hk", "允许包装重量（SAP MARA.ERGEW）"),

            // entity.material.allowedpackagingweightunit
            new TranslationSeedItem("entity.material.allowedpackagingweightunit", "en-US", "允许包装重量单位_us", "允许包装重量单位（SAP MARA.ERGEI）"),
            // entity.material.allowedpackagingweightunit
            new TranslationSeedItem("entity.material.allowedpackagingweightunit", "ja-JP", "允许包装重量单位_jp", "允许包装重量单位（SAP MARA.ERGEI）"),
            // entity.material.allowedpackagingweightunit
            new TranslationSeedItem("entity.material.allowedpackagingweightunit", "zh-CN", "允许包装重量单位", "允许包装重量单位（SAP MARA.ERGEI）"),
            // entity.material.allowedpackagingweightunit
            new TranslationSeedItem("entity.material.allowedpackagingweightunit", "zh-HK", "允许包装重量单位_hk", "允许包装重量单位（SAP MARA.ERGEI）"),

            // entity.material.allowedpackagingvolume
            new TranslationSeedItem("entity.material.allowedpackagingvolume", "en-US", "允许包装体积_us", "允许包装体积（SAP MARA.ERVOL）"),
            // entity.material.allowedpackagingvolume
            new TranslationSeedItem("entity.material.allowedpackagingvolume", "ja-JP", "允许包装体积_jp", "允许包装体积（SAP MARA.ERVOL）"),
            // entity.material.allowedpackagingvolume
            new TranslationSeedItem("entity.material.allowedpackagingvolume", "zh-CN", "允许包装体积", "允许包装体积（SAP MARA.ERVOL）"),
            // entity.material.allowedpackagingvolume
            new TranslationSeedItem("entity.material.allowedpackagingvolume", "zh-HK", "允许包装体积_hk", "允许包装体积（SAP MARA.ERVOL）"),

            // entity.material.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.material.allowedpackagingvolumeunit", "en-US", "允许包装体积单位_us", "允许包装体积单位（SAP MARA.ERVOE）"),
            // entity.material.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.material.allowedpackagingvolumeunit", "ja-JP", "允许包装体积单位_jp", "允许包装体积单位（SAP MARA.ERVOE）"),
            // entity.material.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.material.allowedpackagingvolumeunit", "zh-CN", "允许包装体积单位", "允许包装体积单位（SAP MARA.ERVOE）"),
            // entity.material.allowedpackagingvolumeunit
            new TranslationSeedItem("entity.material.allowedpackagingvolumeunit", "zh-HK", "允许包装体积单位_hk", "允许包装体积单位（SAP MARA.ERVOE）"),

            // entity.material.excessweighttolerance
            new TranslationSeedItem("entity.material.excessweighttolerance", "en-US", "超重容差_us", "超重容差（SAP MARA.GEWTO）"),
            // entity.material.excessweighttolerance
            new TranslationSeedItem("entity.material.excessweighttolerance", "ja-JP", "超重容差_jp", "超重容差（SAP MARA.GEWTO）"),
            // entity.material.excessweighttolerance
            new TranslationSeedItem("entity.material.excessweighttolerance", "zh-CN", "超重容差", "超重容差（SAP MARA.GEWTO）"),
            // entity.material.excessweighttolerance
            new TranslationSeedItem("entity.material.excessweighttolerance", "zh-HK", "超重容差_hk", "超重容差（SAP MARA.GEWTO）"),

            // entity.material.excessvolumetolerance
            new TranslationSeedItem("entity.material.excessvolumetolerance", "en-US", "超体积容差_us", "超体积容差（SAP MARA.VOLTO）"),
            // entity.material.excessvolumetolerance
            new TranslationSeedItem("entity.material.excessvolumetolerance", "ja-JP", "超体积容差_jp", "超体积容差（SAP MARA.VOLTO）"),
            // entity.material.excessvolumetolerance
            new TranslationSeedItem("entity.material.excessvolumetolerance", "zh-CN", "超体积容差", "超体积容差（SAP MARA.VOLTO）"),
            // entity.material.excessvolumetolerance
            new TranslationSeedItem("entity.material.excessvolumetolerance", "zh-HK", "超体积容差_hk", "超体积容差（SAP MARA.VOLTO）"),

            // entity.material.variablepurchaseorderunit
            new TranslationSeedItem("entity.material.variablepurchaseorderunit", "en-US", "可变采购订单单位_us", "可变采购订单单位（SAP MARA.VABME）"),
            // entity.material.variablepurchaseorderunit
            new TranslationSeedItem("entity.material.variablepurchaseorderunit", "ja-JP", "可变采购订单单位_jp", "可变采购订单单位（SAP MARA.VABME）"),
            // entity.material.variablepurchaseorderunit
            new TranslationSeedItem("entity.material.variablepurchaseorderunit", "zh-CN", "可变采购订单单位", "可变采购订单单位（SAP MARA.VABME）"),
            // entity.material.variablepurchaseorderunit
            new TranslationSeedItem("entity.material.variablepurchaseorderunit", "zh-HK", "可变采购订单单位_hk", "可变采购订单单位（SAP MARA.VABME）"),

            // entity.material.revisionlevelassigned
            new TranslationSeedItem("entity.material.revisionlevelassigned", "en-US", "已分配修订级别_us", "已分配修订级别（SAP MARA.KZREV）"),
            // entity.material.revisionlevelassigned
            new TranslationSeedItem("entity.material.revisionlevelassigned", "ja-JP", "已分配修订级别_jp", "已分配修订级别（SAP MARA.KZREV）"),
            // entity.material.revisionlevelassigned
            new TranslationSeedItem("entity.material.revisionlevelassigned", "zh-CN", "已分配修订级别", "已分配修订级别（SAP MARA.KZREV）"),
            // entity.material.revisionlevelassigned
            new TranslationSeedItem("entity.material.revisionlevelassigned", "zh-HK", "已分配修订级别_hk", "已分配修订级别（SAP MARA.KZREV）"),

            // entity.material.configurablematerial
            new TranslationSeedItem("entity.material.configurablematerial", "en-US", "可配置物料_us", "可配置物料（SAP MARA.KZKFG）"),
            // entity.material.configurablematerial
            new TranslationSeedItem("entity.material.configurablematerial", "ja-JP", "可配置物料_jp", "可配置物料（SAP MARA.KZKFG）"),
            // entity.material.configurablematerial
            new TranslationSeedItem("entity.material.configurablematerial", "zh-CN", "可配置物料", "可配置物料（SAP MARA.KZKFG）"),
            // entity.material.configurablematerial
            new TranslationSeedItem("entity.material.configurablematerial", "zh-HK", "可配置物料_hk", "可配置物料（SAP MARA.KZKFG）"),

            // entity.material.batchmanagementrequired
            new TranslationSeedItem("entity.material.batchmanagementrequired", "en-US", "批次管理要求_us", "批次管理要求（SAP MARA.XCHPF）"),
            // entity.material.batchmanagementrequired
            new TranslationSeedItem("entity.material.batchmanagementrequired", "ja-JP", "批次管理要求_jp", "批次管理要求（SAP MARA.XCHPF）"),
            // entity.material.batchmanagementrequired
            new TranslationSeedItem("entity.material.batchmanagementrequired", "zh-CN", "批次管理要求", "批次管理要求（SAP MARA.XCHPF）"),
            // entity.material.batchmanagementrequired
            new TranslationSeedItem("entity.material.batchmanagementrequired", "zh-HK", "批次管理要求_hk", "批次管理要求（SAP MARA.XCHPF）"),

            // entity.material.packagingmaterialtype
            new TranslationSeedItem("entity.material.packagingmaterialtype", "en-US", "包装物料类型_us", "包装物料类型（SAP MARA.VHART）"),
            // entity.material.packagingmaterialtype
            new TranslationSeedItem("entity.material.packagingmaterialtype", "ja-JP", "包装物料类型_jp", "包装物料类型（SAP MARA.VHART）"),
            // entity.material.packagingmaterialtype
            new TranslationSeedItem("entity.material.packagingmaterialtype", "zh-CN", "包装物料类型", "包装物料类型（SAP MARA.VHART）"),
            // entity.material.packagingmaterialtype
            new TranslationSeedItem("entity.material.packagingmaterialtype", "zh-HK", "包装物料类型_hk", "包装物料类型（SAP MARA.VHART）"),

            // entity.material.maximumlevelbyvolume
            new TranslationSeedItem("entity.material.maximumlevelbyvolume", "en-US", "最大装载量（体积）_us", "最大装载量（体积）（SAP MARA.FUELG）"),
            // entity.material.maximumlevelbyvolume
            new TranslationSeedItem("entity.material.maximumlevelbyvolume", "ja-JP", "最大装载量（体积）_jp", "最大装载量（体积）（SAP MARA.FUELG）"),
            // entity.material.maximumlevelbyvolume
            new TranslationSeedItem("entity.material.maximumlevelbyvolume", "zh-CN", "最大装载量（体积）", "最大装载量（体积）（SAP MARA.FUELG）"),
            // entity.material.maximumlevelbyvolume
            new TranslationSeedItem("entity.material.maximumlevelbyvolume", "zh-HK", "最大装载量（体积）_hk", "最大装载量（体积）（SAP MARA.FUELG）"),

            // entity.material.stackingfactor
            new TranslationSeedItem("entity.material.stackingfactor", "en-US", "堆叠因子_us", "堆叠因子（SAP MARA.STFAK）"),
            // entity.material.stackingfactor
            new TranslationSeedItem("entity.material.stackingfactor", "ja-JP", "堆叠因子_jp", "堆叠因子（SAP MARA.STFAK）"),
            // entity.material.stackingfactor
            new TranslationSeedItem("entity.material.stackingfactor", "zh-CN", "堆叠因子", "堆叠因子（SAP MARA.STFAK）"),
            // entity.material.stackingfactor
            new TranslationSeedItem("entity.material.stackingfactor", "zh-HK", "堆叠因子_hk", "堆叠因子（SAP MARA.STFAK）"),

            // entity.material.packagingmaterialgroup
            new TranslationSeedItem("entity.material.packagingmaterialgroup", "en-US", "包装物料组_us", "包装物料组（SAP MARA.MAGRV）"),
            // entity.material.packagingmaterialgroup
            new TranslationSeedItem("entity.material.packagingmaterialgroup", "ja-JP", "包装物料组_jp", "包装物料组（SAP MARA.MAGRV）"),
            // entity.material.packagingmaterialgroup
            new TranslationSeedItem("entity.material.packagingmaterialgroup", "zh-CN", "包装物料组", "包装物料组（SAP MARA.MAGRV）"),
            // entity.material.packagingmaterialgroup
            new TranslationSeedItem("entity.material.packagingmaterialgroup", "zh-HK", "包装物料组_hk", "包装物料组（SAP MARA.MAGRV）"),

            // entity.material.authorizationgroup
            new TranslationSeedItem("entity.material.authorizationgroup", "en-US", "权限组_us", "权限组（SAP MARA.BEGRU）"),
            // entity.material.authorizationgroup
            new TranslationSeedItem("entity.material.authorizationgroup", "ja-JP", "权限组_jp", "权限组（SAP MARA.BEGRU）"),
            // entity.material.authorizationgroup
            new TranslationSeedItem("entity.material.authorizationgroup", "zh-CN", "权限组", "权限组（SAP MARA.BEGRU）"),
            // entity.material.authorizationgroup
            new TranslationSeedItem("entity.material.authorizationgroup", "zh-HK", "权限组_hk", "权限组（SAP MARA.BEGRU）"),

            // entity.material.validfromdate
            new TranslationSeedItem("entity.material.validfromdate", "en-US", "有效起始日期_us", "有效起始日期（SAP MARA.DATAB）"),
            // entity.material.validfromdate
            new TranslationSeedItem("entity.material.validfromdate", "ja-JP", "有效起始日期_jp", "有效起始日期（SAP MARA.DATAB）"),
            // entity.material.validfromdate
            new TranslationSeedItem("entity.material.validfromdate", "zh-CN", "有效起始日期", "有效起始日期（SAP MARA.DATAB）"),
            // entity.material.validfromdate
            new TranslationSeedItem("entity.material.validfromdate", "zh-HK", "有效起始日期_hk", "有效起始日期（SAP MARA.DATAB）"),

            // entity.material.seasonyear
            new TranslationSeedItem("entity.material.seasonyear", "en-US", "季节年份_us", "季节年份（SAP MARA.SAISJ）"),
            // entity.material.seasonyear
            new TranslationSeedItem("entity.material.seasonyear", "ja-JP", "季节年份_jp", "季节年份（SAP MARA.SAISJ）"),
            // entity.material.seasonyear
            new TranslationSeedItem("entity.material.seasonyear", "zh-CN", "季节年份", "季节年份（SAP MARA.SAISJ）"),
            // entity.material.seasonyear
            new TranslationSeedItem("entity.material.seasonyear", "zh-HK", "季节年份_hk", "季节年份（SAP MARA.SAISJ）"),

            // entity.material.pricebandcategory
            new TranslationSeedItem("entity.material.pricebandcategory", "en-US", "价格带类别_us", "价格带类别（SAP MARA.PLGTP）"),
            // entity.material.pricebandcategory
            new TranslationSeedItem("entity.material.pricebandcategory", "ja-JP", "价格带类别_jp", "价格带类别（SAP MARA.PLGTP）"),
            // entity.material.pricebandcategory
            new TranslationSeedItem("entity.material.pricebandcategory", "zh-CN", "价格带类别", "价格带类别（SAP MARA.PLGTP）"),
            // entity.material.pricebandcategory
            new TranslationSeedItem("entity.material.pricebandcategory", "zh-HK", "价格带类别_hk", "价格带类别（SAP MARA.PLGTP）"),

            // entity.material.emptiesbillofmaterial
            new TranslationSeedItem("entity.material.emptiesbillofmaterial", "en-US", "空容器BOM_us", "空容器BOM（SAP MARA.MLGUT）"),
            // entity.material.emptiesbillofmaterial
            new TranslationSeedItem("entity.material.emptiesbillofmaterial", "ja-JP", "空容器BOM_jp", "空容器BOM（SAP MARA.MLGUT）"),
            // entity.material.emptiesbillofmaterial
            new TranslationSeedItem("entity.material.emptiesbillofmaterial", "zh-CN", "空容器BOM", "空容器BOM（SAP MARA.MLGUT）"),
            // entity.material.emptiesbillofmaterial
            new TranslationSeedItem("entity.material.emptiesbillofmaterial", "zh-HK", "空容器BOM_hk", "空容器BOM（SAP MARA.MLGUT）"),

            // entity.material.externalmaterialgroup
            new TranslationSeedItem("entity.material.externalmaterialgroup", "en-US", "外部物料组_us", "外部物料组（SAP MARA.EXTWG）"),
            // entity.material.externalmaterialgroup
            new TranslationSeedItem("entity.material.externalmaterialgroup", "ja-JP", "外部物料组_jp", "外部物料组（SAP MARA.EXTWG）"),
            // entity.material.externalmaterialgroup
            new TranslationSeedItem("entity.material.externalmaterialgroup", "zh-CN", "外部物料组", "外部物料组（SAP MARA.EXTWG）"),
            // entity.material.externalmaterialgroup
            new TranslationSeedItem("entity.material.externalmaterialgroup", "zh-HK", "外部物料组_hk", "外部物料组（SAP MARA.EXTWG）"),

            // entity.material.crossplantconfigurablematerial
            new TranslationSeedItem("entity.material.crossplantconfigurablematerial", "en-US", "跨工厂可配置物料_us", "跨工厂可配置物料（SAP MARA.SATNR）"),
            // entity.material.crossplantconfigurablematerial
            new TranslationSeedItem("entity.material.crossplantconfigurablematerial", "ja-JP", "跨工厂可配置物料_jp", "跨工厂可配置物料（SAP MARA.SATNR）"),
            // entity.material.crossplantconfigurablematerial
            new TranslationSeedItem("entity.material.crossplantconfigurablematerial", "zh-CN", "跨工厂可配置物料", "跨工厂可配置物料（SAP MARA.SATNR）"),
            // entity.material.crossplantconfigurablematerial
            new TranslationSeedItem("entity.material.crossplantconfigurablematerial", "zh-HK", "跨工厂可配置物料_hk", "跨工厂可配置物料（SAP MARA.SATNR）"),

            // entity.material.category
            new TranslationSeedItem("entity.material.category", "en-US", "物料类别_us", "物料类别（SAP MARA.ATTYP）"),
            // entity.material.category
            new TranslationSeedItem("entity.material.category", "ja-JP", "物料类别_jp", "物料类别（SAP MARA.ATTYP）"),
            // entity.material.category
            new TranslationSeedItem("entity.material.category", "zh-CN", "物料类别", "物料类别（SAP MARA.ATTYP）"),
            // entity.material.category
            new TranslationSeedItem("entity.material.category", "zh-HK", "物料类别_hk", "物料类别（SAP MARA.ATTYP）"),

            // entity.material.coproductindicator
            new TranslationSeedItem("entity.material.coproductindicator", "en-US", "联产品标识_us", "联产品标识（SAP MARA.KZKUP）"),
            // entity.material.coproductindicator
            new TranslationSeedItem("entity.material.coproductindicator", "ja-JP", "联产品标识_jp", "联产品标识（SAP MARA.KZKUP）"),
            // entity.material.coproductindicator
            new TranslationSeedItem("entity.material.coproductindicator", "zh-CN", "联产品标识", "联产品标识（SAP MARA.KZKUP）"),
            // entity.material.coproductindicator
            new TranslationSeedItem("entity.material.coproductindicator", "zh-HK", "联产品标识_hk", "联产品标识（SAP MARA.KZKUP）"),

            // entity.material.followupmaterialindicator
            new TranslationSeedItem("entity.material.followupmaterialindicator", "en-US", "后续物料标识_us", "后续物料标识（SAP MARA.KZNFM）"),
            // entity.material.followupmaterialindicator
            new TranslationSeedItem("entity.material.followupmaterialindicator", "ja-JP", "后续物料标识_jp", "后续物料标识（SAP MARA.KZNFM）"),
            // entity.material.followupmaterialindicator
            new TranslationSeedItem("entity.material.followupmaterialindicator", "zh-CN", "后续物料标识", "后续物料标识（SAP MARA.KZNFM）"),
            // entity.material.followupmaterialindicator
            new TranslationSeedItem("entity.material.followupmaterialindicator", "zh-HK", "后续物料标识_hk", "后续物料标识（SAP MARA.KZNFM）"),

            // entity.material.pricingreferencematerial
            new TranslationSeedItem("entity.material.pricingreferencematerial", "en-US", "定价参考物料_us", "定价参考物料（SAP MARA.PMATA）"),
            // entity.material.pricingreferencematerial
            new TranslationSeedItem("entity.material.pricingreferencematerial", "ja-JP", "定价参考物料_jp", "定价参考物料（SAP MARA.PMATA）"),
            // entity.material.pricingreferencematerial
            new TranslationSeedItem("entity.material.pricingreferencematerial", "zh-CN", "定价参考物料", "定价参考物料（SAP MARA.PMATA）"),
            // entity.material.pricingreferencematerial
            new TranslationSeedItem("entity.material.pricingreferencematerial", "zh-HK", "定价参考物料_hk", "定价参考物料（SAP MARA.PMATA）"),

            // entity.material.crossplantmaterialstatus
            new TranslationSeedItem("entity.material.crossplantmaterialstatus", "en-US", "跨工厂物料状态_us", "跨工厂物料状态（SAP MARA.MSTAE）"),
            // entity.material.crossplantmaterialstatus
            new TranslationSeedItem("entity.material.crossplantmaterialstatus", "ja-JP", "跨工厂物料状态_jp", "跨工厂物料状态（SAP MARA.MSTAE）"),
            // entity.material.crossplantmaterialstatus
            new TranslationSeedItem("entity.material.crossplantmaterialstatus", "zh-CN", "跨工厂物料状态", "跨工厂物料状态（SAP MARA.MSTAE）"),
            // entity.material.crossplantmaterialstatus
            new TranslationSeedItem("entity.material.crossplantmaterialstatus", "zh-HK", "跨工厂物料状态_hk", "跨工厂物料状态（SAP MARA.MSTAE）"),

            // entity.material.crossdistributionchainstatus
            new TranslationSeedItem("entity.material.crossdistributionchainstatus", "en-US", "跨分销链物料状态_us", "跨分销链物料状态（SAP MARA.MSTAV）"),
            // entity.material.crossdistributionchainstatus
            new TranslationSeedItem("entity.material.crossdistributionchainstatus", "ja-JP", "跨分销链物料状态_jp", "跨分销链物料状态（SAP MARA.MSTAV）"),
            // entity.material.crossdistributionchainstatus
            new TranslationSeedItem("entity.material.crossdistributionchainstatus", "zh-CN", "跨分销链物料状态", "跨分销链物料状态（SAP MARA.MSTAV）"),
            // entity.material.crossdistributionchainstatus
            new TranslationSeedItem("entity.material.crossdistributionchainstatus", "zh-HK", "跨分销链物料状态_hk", "跨分销链物料状态（SAP MARA.MSTAV）"),

            // entity.material.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.material.crossplantstatusvalidfrom", "en-US", "跨工厂状态生效日期_us", "跨工厂状态生效日期（SAP MARA.MSTDE）"),
            // entity.material.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.material.crossplantstatusvalidfrom", "ja-JP", "跨工厂状态生效日期_jp", "跨工厂状态生效日期（SAP MARA.MSTDE）"),
            // entity.material.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.material.crossplantstatusvalidfrom", "zh-CN", "跨工厂状态生效日期", "跨工厂状态生效日期（SAP MARA.MSTDE）"),
            // entity.material.crossplantstatusvalidfrom
            new TranslationSeedItem("entity.material.crossplantstatusvalidfrom", "zh-HK", "跨工厂状态生效日期_hk", "跨工厂状态生效日期（SAP MARA.MSTDE）"),

            // entity.material.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.material.crossdistributionstatusvalidfrom", "en-US", "跨分销链状态生效日期_us", "跨分销链状态生效日期（SAP MARA.MSTDV）"),
            // entity.material.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.material.crossdistributionstatusvalidfrom", "ja-JP", "跨分销链状态生效日期_jp", "跨分销链状态生效日期（SAP MARA.MSTDV）"),
            // entity.material.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.material.crossdistributionstatusvalidfrom", "zh-CN", "跨分销链状态生效日期", "跨分销链状态生效日期（SAP MARA.MSTDV）"),
            // entity.material.crossdistributionstatusvalidfrom
            new TranslationSeedItem("entity.material.crossdistributionstatusvalidfrom", "zh-HK", "跨分销链状态生效日期_hk", "跨分销链状态生效日期（SAP MARA.MSTDV）"),

            // entity.material.taxclassification
            new TranslationSeedItem("entity.material.taxclassification", "en-US", "物料税分类_us", "物料税分类（SAP MARA.TAKLV）"),
            // entity.material.taxclassification
            new TranslationSeedItem("entity.material.taxclassification", "ja-JP", "物料税分类_jp", "物料税分类（SAP MARA.TAKLV）"),
            // entity.material.taxclassification
            new TranslationSeedItem("entity.material.taxclassification", "zh-CN", "物料税分类", "物料税分类（SAP MARA.TAKLV）"),
            // entity.material.taxclassification
            new TranslationSeedItem("entity.material.taxclassification", "zh-HK", "物料税分类_hk", "物料税分类（SAP MARA.TAKLV）"),

            // entity.material.catalogprofile
            new TranslationSeedItem("entity.material.catalogprofile", "en-US", "目录参数文件_us", "目录参数文件（SAP MARA.RBNRM）"),
            // entity.material.catalogprofile
            new TranslationSeedItem("entity.material.catalogprofile", "ja-JP", "目录参数文件_jp", "目录参数文件（SAP MARA.RBNRM）"),
            // entity.material.catalogprofile
            new TranslationSeedItem("entity.material.catalogprofile", "zh-CN", "目录参数文件", "目录参数文件（SAP MARA.RBNRM）"),
            // entity.material.catalogprofile
            new TranslationSeedItem("entity.material.catalogprofile", "zh-HK", "目录参数文件_hk", "目录参数文件（SAP MARA.RBNRM）"),

            // entity.material.minimumremainingshelflife
            new TranslationSeedItem("entity.material.minimumremainingshelflife", "en-US", "最短剩余货架寿命_us", "最短剩余货架寿命（SAP MARA.MHDRZ）"),
            // entity.material.minimumremainingshelflife
            new TranslationSeedItem("entity.material.minimumremainingshelflife", "ja-JP", "最短剩余货架寿命_jp", "最短剩余货架寿命（SAP MARA.MHDRZ）"),
            // entity.material.minimumremainingshelflife
            new TranslationSeedItem("entity.material.minimumremainingshelflife", "zh-CN", "最短剩余货架寿命", "最短剩余货架寿命（SAP MARA.MHDRZ）"),
            // entity.material.minimumremainingshelflife
            new TranslationSeedItem("entity.material.minimumremainingshelflife", "zh-HK", "最短剩余货架寿命_hk", "最短剩余货架寿命（SAP MARA.MHDRZ）"),

            // entity.material.totalshelflife
            new TranslationSeedItem("entity.material.totalshelflife", "en-US", "总货架寿命_us", "总货架寿命（SAP MARA.MHDHB）"),
            // entity.material.totalshelflife
            new TranslationSeedItem("entity.material.totalshelflife", "ja-JP", "总货架寿命_jp", "总货架寿命（SAP MARA.MHDHB）"),
            // entity.material.totalshelflife
            new TranslationSeedItem("entity.material.totalshelflife", "zh-CN", "总货架寿命", "总货架寿命（SAP MARA.MHDHB）"),
            // entity.material.totalshelflife
            new TranslationSeedItem("entity.material.totalshelflife", "zh-HK", "总货架寿命_hk", "总货架寿命（SAP MARA.MHDHB）"),

            // entity.material.storagepercentage
            new TranslationSeedItem("entity.material.storagepercentage", "en-US", "仓储百分比_us", "仓储百分比（SAP MARA.MHDLP）"),
            // entity.material.storagepercentage
            new TranslationSeedItem("entity.material.storagepercentage", "ja-JP", "仓储百分比_jp", "仓储百分比（SAP MARA.MHDLP）"),
            // entity.material.storagepercentage
            new TranslationSeedItem("entity.material.storagepercentage", "zh-CN", "仓储百分比", "仓储百分比（SAP MARA.MHDLP）"),
            // entity.material.storagepercentage
            new TranslationSeedItem("entity.material.storagepercentage", "zh-HK", "仓储百分比_hk", "仓储百分比（SAP MARA.MHDLP）"),

            // entity.material.contentunit
            new TranslationSeedItem("entity.material.contentunit", "en-US", "含量单位_us", "含量单位（SAP MARA.INHME）"),
            // entity.material.contentunit
            new TranslationSeedItem("entity.material.contentunit", "ja-JP", "含量单位_jp", "含量单位（SAP MARA.INHME）"),
            // entity.material.contentunit
            new TranslationSeedItem("entity.material.contentunit", "zh-CN", "含量单位", "含量单位（SAP MARA.INHME）"),
            // entity.material.contentunit
            new TranslationSeedItem("entity.material.contentunit", "zh-HK", "含量单位_hk", "含量单位（SAP MARA.INHME）"),

            // entity.material.netcontents
            new TranslationSeedItem("entity.material.netcontents", "en-US", "净含量_us", "净含量（SAP MARA.INHAL）"),
            // entity.material.netcontents
            new TranslationSeedItem("entity.material.netcontents", "ja-JP", "净含量_jp", "净含量（SAP MARA.INHAL）"),
            // entity.material.netcontents
            new TranslationSeedItem("entity.material.netcontents", "zh-CN", "净含量", "净含量（SAP MARA.INHAL）"),
            // entity.material.netcontents
            new TranslationSeedItem("entity.material.netcontents", "zh-HK", "净含量_hk", "净含量（SAP MARA.INHAL）"),

            // entity.material.comparisonpriceunit
            new TranslationSeedItem("entity.material.comparisonpriceunit", "en-US", "比较价格单位_us", "比较价格单位（SAP MARA.VPREH）"),
            // entity.material.comparisonpriceunit
            new TranslationSeedItem("entity.material.comparisonpriceunit", "ja-JP", "比较价格单位_jp", "比较价格单位（SAP MARA.VPREH）"),
            // entity.material.comparisonpriceunit
            new TranslationSeedItem("entity.material.comparisonpriceunit", "zh-CN", "比较价格单位", "比较价格单位（SAP MARA.VPREH）"),
            // entity.material.comparisonpriceunit
            new TranslationSeedItem("entity.material.comparisonpriceunit", "zh-HK", "比较价格单位_hk", "比较价格单位（SAP MARA.VPREH）"),

            // entity.material.labelingmaterialgrouping
            new TranslationSeedItem("entity.material.labelingmaterialgrouping", "en-US", "标签物料分组_us", "标签物料分组（SAP MARA.ETIAG）"),
            // entity.material.labelingmaterialgrouping
            new TranslationSeedItem("entity.material.labelingmaterialgrouping", "ja-JP", "标签物料分组_jp", "标签物料分组（SAP MARA.ETIAG）"),
            // entity.material.labelingmaterialgrouping
            new TranslationSeedItem("entity.material.labelingmaterialgrouping", "zh-CN", "标签物料分组", "标签物料分组（SAP MARA.ETIAG）"),
            // entity.material.labelingmaterialgrouping
            new TranslationSeedItem("entity.material.labelingmaterialgrouping", "zh-HK", "标签物料分组_hk", "标签物料分组（SAP MARA.ETIAG）"),

            // entity.material.grosscontents
            new TranslationSeedItem("entity.material.grosscontents", "en-US", "毛含量_us", "毛含量（SAP MARA.INHBR）"),
            // entity.material.grosscontents
            new TranslationSeedItem("entity.material.grosscontents", "ja-JP", "毛含量_jp", "毛含量（SAP MARA.INHBR）"),
            // entity.material.grosscontents
            new TranslationSeedItem("entity.material.grosscontents", "zh-CN", "毛含量", "毛含量（SAP MARA.INHBR）"),
            // entity.material.grosscontents
            new TranslationSeedItem("entity.material.grosscontents", "zh-HK", "毛含量_hk", "毛含量（SAP MARA.INHBR）"),

            // entity.material.quantityconversionmethod
            new TranslationSeedItem("entity.material.quantityconversionmethod", "en-US", "数量换算方法_us", "数量换算方法（SAP MARA.CMETH）"),
            // entity.material.quantityconversionmethod
            new TranslationSeedItem("entity.material.quantityconversionmethod", "ja-JP", "数量换算方法_jp", "数量换算方法（SAP MARA.CMETH）"),
            // entity.material.quantityconversionmethod
            new TranslationSeedItem("entity.material.quantityconversionmethod", "zh-CN", "数量换算方法", "数量换算方法（SAP MARA.CMETH）"),
            // entity.material.quantityconversionmethod
            new TranslationSeedItem("entity.material.quantityconversionmethod", "zh-HK", "数量换算方法_hk", "数量换算方法（SAP MARA.CMETH）"),

            // entity.material.internalobjectnumber
            new TranslationSeedItem("entity.material.internalobjectnumber", "en-US", "内部对象号_us", "内部对象号（SAP MARA.CUOBF）"),
            // entity.material.internalobjectnumber
            new TranslationSeedItem("entity.material.internalobjectnumber", "ja-JP", "内部对象号_jp", "内部对象号（SAP MARA.CUOBF）"),
            // entity.material.internalobjectnumber
            new TranslationSeedItem("entity.material.internalobjectnumber", "zh-CN", "内部对象号", "内部对象号（SAP MARA.CUOBF）"),
            // entity.material.internalobjectnumber
            new TranslationSeedItem("entity.material.internalobjectnumber", "zh-HK", "内部对象号_hk", "内部对象号（SAP MARA.CUOBF）"),

            // entity.material.environmentallyrelevant
            new TranslationSeedItem("entity.material.environmentallyrelevant", "en-US", "环境相关_us", "环境相关（SAP MARA.KZUMW）"),
            // entity.material.environmentallyrelevant
            new TranslationSeedItem("entity.material.environmentallyrelevant", "ja-JP", "环境相关_jp", "环境相关（SAP MARA.KZUMW）"),
            // entity.material.environmentallyrelevant
            new TranslationSeedItem("entity.material.environmentallyrelevant", "zh-CN", "环境相关", "环境相关（SAP MARA.KZUMW）"),
            // entity.material.environmentallyrelevant
            new TranslationSeedItem("entity.material.environmentallyrelevant", "zh-HK", "环境相关_hk", "环境相关（SAP MARA.KZUMW）"),

            // entity.material.productallocationprocedure
            new TranslationSeedItem("entity.material.productallocationprocedure", "en-US", "产品分配确定过程_us", "产品分配确定过程（SAP MARA.KOSCH）"),
            // entity.material.productallocationprocedure
            new TranslationSeedItem("entity.material.productallocationprocedure", "ja-JP", "产品分配确定过程_jp", "产品分配确定过程（SAP MARA.KOSCH）"),
            // entity.material.productallocationprocedure
            new TranslationSeedItem("entity.material.productallocationprocedure", "zh-CN", "产品分配确定过程", "产品分配确定过程（SAP MARA.KOSCH）"),
            // entity.material.productallocationprocedure
            new TranslationSeedItem("entity.material.productallocationprocedure", "zh-HK", "产品分配确定过程_hk", "产品分配确定过程（SAP MARA.KOSCH）"),

            // entity.material.variantpricingprofile
            new TranslationSeedItem("entity.material.variantpricingprofile", "en-US", "变式定价参数文件_us", "变式定价参数文件（SAP MARA.SPROF）"),
            // entity.material.variantpricingprofile
            new TranslationSeedItem("entity.material.variantpricingprofile", "ja-JP", "变式定价参数文件_jp", "变式定价参数文件（SAP MARA.SPROF）"),
            // entity.material.variantpricingprofile
            new TranslationSeedItem("entity.material.variantpricingprofile", "zh-CN", "变式定价参数文件", "变式定价参数文件（SAP MARA.SPROF）"),
            // entity.material.variantpricingprofile
            new TranslationSeedItem("entity.material.variantpricingprofile", "zh-HK", "变式定价参数文件_hk", "变式定价参数文件（SAP MARA.SPROF）"),

            // entity.material.discountinkind
            new TranslationSeedItem("entity.material.discountinkind", "en-US", "实物折扣资格_us", "实物折扣资格（SAP MARA.NRFHG）"),
            // entity.material.discountinkind
            new TranslationSeedItem("entity.material.discountinkind", "ja-JP", "实物折扣资格_jp", "实物折扣资格（SAP MARA.NRFHG）"),
            // entity.material.discountinkind
            new TranslationSeedItem("entity.material.discountinkind", "zh-CN", "实物折扣资格", "实物折扣资格（SAP MARA.NRFHG）"),
            // entity.material.discountinkind
            new TranslationSeedItem("entity.material.discountinkind", "zh-HK", "实物折扣资格_hk", "实物折扣资格（SAP MARA.NRFHG）"),

            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "en-US", "制造商零件号_us", "制造商零件号（SAP MARA.MFRPN）"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "ja-JP", "制造商零件号_jp", "制造商零件号（SAP MARA.MFRPN）"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "zh-CN", "制造商零件号", "制造商零件号（SAP MARA.MFRPN）"),
            // entity.material.manufacturerpartnumber
            new TranslationSeedItem("entity.material.manufacturerpartnumber", "zh-HK", "制造商零件号_hk", "制造商零件号（SAP MARA.MFRPN）"),

            // entity.material.manufacturernumber
            new TranslationSeedItem("entity.material.manufacturernumber", "en-US", "制造商编码_us", "制造商编码（SAP MARA.MFRNR）"),
            // entity.material.manufacturernumber
            new TranslationSeedItem("entity.material.manufacturernumber", "ja-JP", "制造商编码_jp", "制造商编码（SAP MARA.MFRNR）"),
            // entity.material.manufacturernumber
            new TranslationSeedItem("entity.material.manufacturernumber", "zh-CN", "制造商编码", "制造商编码（SAP MARA.MFRNR）"),
            // entity.material.manufacturernumber
            new TranslationSeedItem("entity.material.manufacturernumber", "zh-HK", "制造商编码_hk", "制造商编码（SAP MARA.MFRNR）"),

            // entity.material.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.material.inventorymanagedmaterialnumber", "en-US", "自有库存管理物料号_us", "自有库存管理物料号（SAP MARA.BMATN）"),
            // entity.material.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.material.inventorymanagedmaterialnumber", "ja-JP", "自有库存管理物料号_jp", "自有库存管理物料号（SAP MARA.BMATN）"),
            // entity.material.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.material.inventorymanagedmaterialnumber", "zh-CN", "自有库存管理物料号", "自有库存管理物料号（SAP MARA.BMATN）"),
            // entity.material.inventorymanagedmaterialnumber
            new TranslationSeedItem("entity.material.inventorymanagedmaterialnumber", "zh-HK", "自有库存管理物料号_hk", "自有库存管理物料号（SAP MARA.BMATN）"),

            // entity.material.manufacturerpartprofile
            new TranslationSeedItem("entity.material.manufacturerpartprofile", "en-US", "制造商零件参数文件_us", "制造商零件参数文件（SAP MARA.MPROF）"),
            // entity.material.manufacturerpartprofile
            new TranslationSeedItem("entity.material.manufacturerpartprofile", "ja-JP", "制造商零件参数文件_jp", "制造商零件参数文件（SAP MARA.MPROF）"),
            // entity.material.manufacturerpartprofile
            new TranslationSeedItem("entity.material.manufacturerpartprofile", "zh-CN", "制造商零件参数文件", "制造商零件参数文件（SAP MARA.MPROF）"),
            // entity.material.manufacturerpartprofile
            new TranslationSeedItem("entity.material.manufacturerpartprofile", "zh-HK", "制造商零件参数文件_hk", "制造商零件参数文件（SAP MARA.MPROF）"),

            // entity.material.unitsofmeasureusage
            new TranslationSeedItem("entity.material.unitsofmeasureusage", "en-US", "计量单位用途_us", "计量单位用途（SAP MARA.KZWSM）"),
            // entity.material.unitsofmeasureusage
            new TranslationSeedItem("entity.material.unitsofmeasureusage", "ja-JP", "计量单位用途_jp", "计量单位用途（SAP MARA.KZWSM）"),
            // entity.material.unitsofmeasureusage
            new TranslationSeedItem("entity.material.unitsofmeasureusage", "zh-CN", "计量单位用途", "计量单位用途（SAP MARA.KZWSM）"),
            // entity.material.unitsofmeasureusage
            new TranslationSeedItem("entity.material.unitsofmeasureusage", "zh-HK", "计量单位用途_hk", "计量单位用途（SAP MARA.KZWSM）"),

            // entity.material.seasonrollout
            new TranslationSeedItem("entity.material.seasonrollout", "en-US", "季节推出_us", "季节推出（SAP MARA.SAITY）"),
            // entity.material.seasonrollout
            new TranslationSeedItem("entity.material.seasonrollout", "ja-JP", "季节推出_jp", "季节推出（SAP MARA.SAITY）"),
            // entity.material.seasonrollout
            new TranslationSeedItem("entity.material.seasonrollout", "zh-CN", "季节推出", "季节推出（SAP MARA.SAITY）"),
            // entity.material.seasonrollout
            new TranslationSeedItem("entity.material.seasonrollout", "zh-HK", "季节推出_hk", "季节推出（SAP MARA.SAITY）"),

            // entity.material.dangerousgoodsprofile
            new TranslationSeedItem("entity.material.dangerousgoodsprofile", "en-US", "危险品参数文件_us", "危险品参数文件（SAP MARA.PROFL）"),
            // entity.material.dangerousgoodsprofile
            new TranslationSeedItem("entity.material.dangerousgoodsprofile", "ja-JP", "危险品参数文件_jp", "危险品参数文件（SAP MARA.PROFL）"),
            // entity.material.dangerousgoodsprofile
            new TranslationSeedItem("entity.material.dangerousgoodsprofile", "zh-CN", "危险品参数文件", "危险品参数文件（SAP MARA.PROFL）"),
            // entity.material.dangerousgoodsprofile
            new TranslationSeedItem("entity.material.dangerousgoodsprofile", "zh-HK", "危险品参数文件_hk", "危险品参数文件（SAP MARA.PROFL）"),

            // entity.material.highlyviscous
            new TranslationSeedItem("entity.material.highlyviscous", "en-US", "高粘度_us", "高粘度（SAP MARA.IHIVI）"),
            // entity.material.highlyviscous
            new TranslationSeedItem("entity.material.highlyviscous", "ja-JP", "高粘度_jp", "高粘度（SAP MARA.IHIVI）"),
            // entity.material.highlyviscous
            new TranslationSeedItem("entity.material.highlyviscous", "zh-CN", "高粘度", "高粘度（SAP MARA.IHIVI）"),
            // entity.material.highlyviscous
            new TranslationSeedItem("entity.material.highlyviscous", "zh-HK", "高粘度_hk", "高粘度（SAP MARA.IHIVI）"),

            // entity.material.inbulkliquid
            new TranslationSeedItem("entity.material.inbulkliquid", "en-US", "散装/液体_us", "散装/液体（SAP MARA.ILOOS）"),
            // entity.material.inbulkliquid
            new TranslationSeedItem("entity.material.inbulkliquid", "ja-JP", "散装/液体_jp", "散装/液体（SAP MARA.ILOOS）"),
            // entity.material.inbulkliquid
            new TranslationSeedItem("entity.material.inbulkliquid", "zh-CN", "散装/液体", "散装/液体（SAP MARA.ILOOS）"),
            // entity.material.inbulkliquid
            new TranslationSeedItem("entity.material.inbulkliquid", "zh-HK", "散装/液体_hk", "散装/液体（SAP MARA.ILOOS）"),

            // entity.material.serialnumberexplicitness
            new TranslationSeedItem("entity.material.serialnumberexplicitness", "en-US", "序列号明确级别_us", "序列号明确级别（SAP MARA.SERLV）"),
            // entity.material.serialnumberexplicitness
            new TranslationSeedItem("entity.material.serialnumberexplicitness", "ja-JP", "序列号明确级别_jp", "序列号明确级别（SAP MARA.SERLV）"),
            // entity.material.serialnumberexplicitness
            new TranslationSeedItem("entity.material.serialnumberexplicitness", "zh-CN", "序列号明确级别", "序列号明确级别（SAP MARA.SERLV）"),
            // entity.material.serialnumberexplicitness
            new TranslationSeedItem("entity.material.serialnumberexplicitness", "zh-HK", "序列号明确级别_hk", "序列号明确级别（SAP MARA.SERLV）"),

            // entity.material.closedpackaging
            new TranslationSeedItem("entity.material.closedpackaging", "en-US", "封闭包装_us", "封闭包装（SAP MARA.KZGVH）"),
            // entity.material.closedpackaging
            new TranslationSeedItem("entity.material.closedpackaging", "ja-JP", "封闭包装_jp", "封闭包装（SAP MARA.KZGVH）"),
            // entity.material.closedpackaging
            new TranslationSeedItem("entity.material.closedpackaging", "zh-CN", "封闭包装", "封闭包装（SAP MARA.KZGVH）"),
            // entity.material.closedpackaging
            new TranslationSeedItem("entity.material.closedpackaging", "zh-HK", "封闭包装_hk", "封闭包装（SAP MARA.KZGVH）"),

            // entity.material.approvedbatchrecordrequired
            new TranslationSeedItem("entity.material.approvedbatchrecordrequired", "en-US", "需批准批次记录_us", "需批准批次记录（SAP MARA.XGCHP）"),
            // entity.material.approvedbatchrecordrequired
            new TranslationSeedItem("entity.material.approvedbatchrecordrequired", "ja-JP", "需批准批次记录_jp", "需批准批次记录（SAP MARA.XGCHP）"),
            // entity.material.approvedbatchrecordrequired
            new TranslationSeedItem("entity.material.approvedbatchrecordrequired", "zh-CN", "需批准批次记录", "需批准批次记录（SAP MARA.XGCHP）"),
            // entity.material.approvedbatchrecordrequired
            new TranslationSeedItem("entity.material.approvedbatchrecordrequired", "zh-HK", "需批准批次记录_hk", "需批准批次记录（SAP MARA.XGCHP）"),

            // entity.material.effectivityparameteroverride
            new TranslationSeedItem("entity.material.effectivityparameteroverride", "en-US", "有效性参数覆盖_us", "有效性参数覆盖（SAP MARA.KZEFF）"),
            // entity.material.effectivityparameteroverride
            new TranslationSeedItem("entity.material.effectivityparameteroverride", "ja-JP", "有效性参数覆盖_jp", "有效性参数覆盖（SAP MARA.KZEFF）"),
            // entity.material.effectivityparameteroverride
            new TranslationSeedItem("entity.material.effectivityparameteroverride", "zh-CN", "有效性参数覆盖", "有效性参数覆盖（SAP MARA.KZEFF）"),
            // entity.material.effectivityparameteroverride
            new TranslationSeedItem("entity.material.effectivityparameteroverride", "zh-HK", "有效性参数覆盖_hk", "有效性参数覆盖（SAP MARA.KZEFF）"),

            // entity.material.completionlevel
            new TranslationSeedItem("entity.material.completionlevel", "en-US", "物料完成级别_us", "物料完成级别（SAP MARA.COMPL）"),
            // entity.material.completionlevel
            new TranslationSeedItem("entity.material.completionlevel", "ja-JP", "物料完成级别_jp", "物料完成级别（SAP MARA.COMPL）"),
            // entity.material.completionlevel
            new TranslationSeedItem("entity.material.completionlevel", "zh-CN", "物料完成级别", "物料完成级别（SAP MARA.COMPL）"),
            // entity.material.completionlevel
            new TranslationSeedItem("entity.material.completionlevel", "zh-HK", "物料完成级别_hk", "物料完成级别（SAP MARA.COMPL）"),

            // entity.material.shelflifeperiodindicator
            new TranslationSeedItem("entity.material.shelflifeperiodindicator", "en-US", "货架寿命期间标识_us", "货架寿命期间标识（SAP MARA.IPRKZ）"),
            // entity.material.shelflifeperiodindicator
            new TranslationSeedItem("entity.material.shelflifeperiodindicator", "ja-JP", "货架寿命期间标识_jp", "货架寿命期间标识（SAP MARA.IPRKZ）"),
            // entity.material.shelflifeperiodindicator
            new TranslationSeedItem("entity.material.shelflifeperiodindicator", "zh-CN", "货架寿命期间标识", "货架寿命期间标识（SAP MARA.IPRKZ）"),
            // entity.material.shelflifeperiodindicator
            new TranslationSeedItem("entity.material.shelflifeperiodindicator", "zh-HK", "货架寿命期间标识_hk", "货架寿命期间标识（SAP MARA.IPRKZ）"),

            // entity.material.shelfliferoundingrule
            new TranslationSeedItem("entity.material.shelfliferoundingrule", "en-US", "货架寿命舍入规则_us", "货架寿命舍入规则（SAP MARA.RDMHD）"),
            // entity.material.shelfliferoundingrule
            new TranslationSeedItem("entity.material.shelfliferoundingrule", "ja-JP", "货架寿命舍入规则_jp", "货架寿命舍入规则（SAP MARA.RDMHD）"),
            // entity.material.shelfliferoundingrule
            new TranslationSeedItem("entity.material.shelfliferoundingrule", "zh-CN", "货架寿命舍入规则", "货架寿命舍入规则（SAP MARA.RDMHD）"),
            // entity.material.shelfliferoundingrule
            new TranslationSeedItem("entity.material.shelfliferoundingrule", "zh-HK", "货架寿命舍入规则_hk", "货架寿命舍入规则（SAP MARA.RDMHD）"),

            // entity.material.productcompositiononpackaging
            new TranslationSeedItem("entity.material.productcompositiononpackaging", "en-US", "包装打印产品成分_us", "包装打印产品成分（SAP MARA.PRZUS）"),
            // entity.material.productcompositiononpackaging
            new TranslationSeedItem("entity.material.productcompositiononpackaging", "ja-JP", "包装打印产品成分_jp", "包装打印产品成分（SAP MARA.PRZUS）"),
            // entity.material.productcompositiononpackaging
            new TranslationSeedItem("entity.material.productcompositiononpackaging", "zh-CN", "包装打印产品成分", "包装打印产品成分（SAP MARA.PRZUS）"),
            // entity.material.productcompositiononpackaging
            new TranslationSeedItem("entity.material.productcompositiononpackaging", "zh-HK", "包装打印产品成分_hk", "包装打印产品成分（SAP MARA.PRZUS）"),

            // entity.material.generalitemcategorygroup
            new TranslationSeedItem("entity.material.generalitemcategorygroup", "en-US", "通用项目类别组_us", "通用项目类别组（SAP MARA.MTPOS_MARA）"),
            // entity.material.generalitemcategorygroup
            new TranslationSeedItem("entity.material.generalitemcategorygroup", "ja-JP", "通用项目类别组_jp", "通用项目类别组（SAP MARA.MTPOS_MARA）"),
            // entity.material.generalitemcategorygroup
            new TranslationSeedItem("entity.material.generalitemcategorygroup", "zh-CN", "通用项目类别组", "通用项目类别组（SAP MARA.MTPOS_MARA）"),
            // entity.material.generalitemcategorygroup
            new TranslationSeedItem("entity.material.generalitemcategorygroup", "zh-HK", "通用项目类别组_hk", "通用项目类别组（SAP MARA.MTPOS_MARA）"),

            // entity.material.logisticalvariants
            new TranslationSeedItem("entity.material.logisticalvariants", "en-US", "后勤变式通用物料_us", "后勤变式通用物料（SAP MARA.BFLME）"),
            // entity.material.logisticalvariants
            new TranslationSeedItem("entity.material.logisticalvariants", "ja-JP", "后勤变式通用物料_jp", "后勤变式通用物料（SAP MARA.BFLME）"),
            // entity.material.logisticalvariants
            new TranslationSeedItem("entity.material.logisticalvariants", "zh-CN", "后勤变式通用物料", "后勤变式通用物料（SAP MARA.BFLME）"),
            // entity.material.logisticalvariants
            new TranslationSeedItem("entity.material.logisticalvariants", "zh-HK", "后勤变式通用物料_hk", "后勤变式通用物料（SAP MARA.BFLME）"),

            // entity.material.locked
            new TranslationSeedItem("entity.material.locked", "en-US", "物料锁定_us", "物料锁定（SAP MARA.MATFI）"),
            // entity.material.locked
            new TranslationSeedItem("entity.material.locked", "ja-JP", "物料锁定_jp", "物料锁定（SAP MARA.MATFI）"),
            // entity.material.locked
            new TranslationSeedItem("entity.material.locked", "zh-CN", "物料锁定", "物料锁定（SAP MARA.MATFI）"),
            // entity.material.locked
            new TranslationSeedItem("entity.material.locked", "zh-HK", "物料锁定_hk", "物料锁定（SAP MARA.MATFI）"),

            // entity.material.configurationmanagementrelevant
            new TranslationSeedItem("entity.material.configurationmanagementrelevant", "en-US", "配置管理相关_us", "配置管理相关（SAP MARA.CMREL）"),
            // entity.material.configurationmanagementrelevant
            new TranslationSeedItem("entity.material.configurationmanagementrelevant", "ja-JP", "配置管理相关_jp", "配置管理相关（SAP MARA.CMREL）"),
            // entity.material.configurationmanagementrelevant
            new TranslationSeedItem("entity.material.configurationmanagementrelevant", "zh-CN", "配置管理相关", "配置管理相关（SAP MARA.CMREL）"),
            // entity.material.configurationmanagementrelevant
            new TranslationSeedItem("entity.material.configurationmanagementrelevant", "zh-HK", "配置管理相关_hk", "配置管理相关（SAP MARA.CMREL）"),

            // entity.material.assortmentlisttype
            new TranslationSeedItem("entity.material.assortmentlisttype", "en-US", "品种清单类型_us", "品种清单类型（SAP MARA.BBTYP）"),
            // entity.material.assortmentlisttype
            new TranslationSeedItem("entity.material.assortmentlisttype", "ja-JP", "品种清单类型_jp", "品种清单类型（SAP MARA.BBTYP）"),
            // entity.material.assortmentlisttype
            new TranslationSeedItem("entity.material.assortmentlisttype", "zh-CN", "品种清单类型", "品种清单类型（SAP MARA.BBTYP）"),
            // entity.material.assortmentlisttype
            new TranslationSeedItem("entity.material.assortmentlisttype", "zh-HK", "品种清单类型_hk", "品种清单类型（SAP MARA.BBTYP）"),

            // entity.material.expirationdatetype
            new TranslationSeedItem("entity.material.expirationdatetype", "en-US", "到期日期类型_us", "到期日期类型（SAP MARA.SLED_BBD）"),
            // entity.material.expirationdatetype
            new TranslationSeedItem("entity.material.expirationdatetype", "ja-JP", "到期日期类型_jp", "到期日期类型（SAP MARA.SLED_BBD）"),
            // entity.material.expirationdatetype
            new TranslationSeedItem("entity.material.expirationdatetype", "zh-CN", "到期日期类型", "到期日期类型（SAP MARA.SLED_BBD）"),
            // entity.material.expirationdatetype
            new TranslationSeedItem("entity.material.expirationdatetype", "zh-HK", "到期日期类型_hk", "到期日期类型（SAP MARA.SLED_BBD）"),

            // entity.material.gtinvariant
            new TranslationSeedItem("entity.material.gtinvariant", "en-US", "GTIN变式_us", "GTIN变式（SAP MARA.GTIN_VARIANT）"),
            // entity.material.gtinvariant
            new TranslationSeedItem("entity.material.gtinvariant", "ja-JP", "GTIN变式_jp", "GTIN变式（SAP MARA.GTIN_VARIANT）"),
            // entity.material.gtinvariant
            new TranslationSeedItem("entity.material.gtinvariant", "zh-CN", "GTIN变式", "GTIN变式（SAP MARA.GTIN_VARIANT）"),
            // entity.material.gtinvariant
            new TranslationSeedItem("entity.material.gtinvariant", "zh-HK", "GTIN变式_hk", "GTIN变式（SAP MARA.GTIN_VARIANT）"),

            // entity.material.genericmaterialnumber
            new TranslationSeedItem("entity.material.genericmaterialnumber", "en-US", "通用物料号_us", "通用物料号（SAP MARA.GENNR）"),
            // entity.material.genericmaterialnumber
            new TranslationSeedItem("entity.material.genericmaterialnumber", "ja-JP", "通用物料号_jp", "通用物料号（SAP MARA.GENNR）"),
            // entity.material.genericmaterialnumber
            new TranslationSeedItem("entity.material.genericmaterialnumber", "zh-CN", "通用物料号", "通用物料号（SAP MARA.GENNR）"),
            // entity.material.genericmaterialnumber
            new TranslationSeedItem("entity.material.genericmaterialnumber", "zh-HK", "通用物料号_hk", "通用物料号（SAP MARA.GENNR）"),

            // entity.material.samepackingreferencematerial
            new TranslationSeedItem("entity.material.samepackingreferencematerial", "en-US", "相同包装参考物料_us", "相同包装参考物料（SAP MARA.RMATP）"),
            // entity.material.samepackingreferencematerial
            new TranslationSeedItem("entity.material.samepackingreferencematerial", "ja-JP", "相同包装参考物料_jp", "相同包装参考物料（SAP MARA.RMATP）"),
            // entity.material.samepackingreferencematerial
            new TranslationSeedItem("entity.material.samepackingreferencematerial", "zh-CN", "相同包装参考物料", "相同包装参考物料（SAP MARA.RMATP）"),
            // entity.material.samepackingreferencematerial
            new TranslationSeedItem("entity.material.samepackingreferencematerial", "zh-HK", "相同包装参考物料_hk", "相同包装参考物料（SAP MARA.RMATP）"),

            // entity.material.globaldatasyncrelevant
            new TranslationSeedItem("entity.material.globaldatasyncrelevant", "en-US", "全球数据同步相关_us", "全球数据同步相关（SAP MARA.GDS_RELEVANT）"),
            // entity.material.globaldatasyncrelevant
            new TranslationSeedItem("entity.material.globaldatasyncrelevant", "ja-JP", "全球数据同步相关_jp", "全球数据同步相关（SAP MARA.GDS_RELEVANT）"),
            // entity.material.globaldatasyncrelevant
            new TranslationSeedItem("entity.material.globaldatasyncrelevant", "zh-CN", "全球数据同步相关", "全球数据同步相关（SAP MARA.GDS_RELEVANT）"),
            // entity.material.globaldatasyncrelevant
            new TranslationSeedItem("entity.material.globaldatasyncrelevant", "zh-HK", "全球数据同步相关_hk", "全球数据同步相关（SAP MARA.GDS_RELEVANT）"),

            // entity.material.acceptanceatorigin
            new TranslationSeedItem("entity.material.acceptanceatorigin", "en-US", "原产地验收_us", "原产地验收（SAP MARA.WEORA）"),
            // entity.material.acceptanceatorigin
            new TranslationSeedItem("entity.material.acceptanceatorigin", "ja-JP", "原产地验收_jp", "原产地验收（SAP MARA.WEORA）"),
            // entity.material.acceptanceatorigin
            new TranslationSeedItem("entity.material.acceptanceatorigin", "zh-CN", "原产地验收", "原产地验收（SAP MARA.WEORA）"),
            // entity.material.acceptanceatorigin
            new TranslationSeedItem("entity.material.acceptanceatorigin", "zh-HK", "原产地验收_hk", "原产地验收（SAP MARA.WEORA）"),

            // entity.material.standardhutype
            new TranslationSeedItem("entity.material.standardhutype", "en-US", "标准HU类型_us", "标准HU类型（SAP MARA.HUTYP_DFLT）"),
            // entity.material.standardhutype
            new TranslationSeedItem("entity.material.standardhutype", "ja-JP", "标准HU类型_jp", "标准HU类型（SAP MARA.HUTYP_DFLT）"),
            // entity.material.standardhutype
            new TranslationSeedItem("entity.material.standardhutype", "zh-CN", "标准HU类型", "标准HU类型（SAP MARA.HUTYP_DFLT）"),
            // entity.material.standardhutype
            new TranslationSeedItem("entity.material.standardhutype", "zh-HK", "标准HU类型_hk", "标准HU类型（SAP MARA.HUTYP_DFLT）"),

            // entity.material.pilferable
            new TranslationSeedItem("entity.material.pilferable", "en-US", "易被盗_us", "易被盗（SAP MARA.PILFERABLE）"),
            // entity.material.pilferable
            new TranslationSeedItem("entity.material.pilferable", "ja-JP", "易被盗_jp", "易被盗（SAP MARA.PILFERABLE）"),
            // entity.material.pilferable
            new TranslationSeedItem("entity.material.pilferable", "zh-CN", "易被盗", "易被盗（SAP MARA.PILFERABLE）"),
            // entity.material.pilferable
            new TranslationSeedItem("entity.material.pilferable", "zh-HK", "易被盗_hk", "易被盗（SAP MARA.PILFERABLE）"),

            // entity.material.warehousestoragecondition
            new TranslationSeedItem("entity.material.warehousestoragecondition", "en-US", "仓储存储条件_us", "仓储存储条件（SAP MARA.WHSTC）"),
            // entity.material.warehousestoragecondition
            new TranslationSeedItem("entity.material.warehousestoragecondition", "ja-JP", "仓储存储条件_jp", "仓储存储条件（SAP MARA.WHSTC）"),
            // entity.material.warehousestoragecondition
            new TranslationSeedItem("entity.material.warehousestoragecondition", "zh-CN", "仓储存储条件", "仓储存储条件（SAP MARA.WHSTC）"),
            // entity.material.warehousestoragecondition
            new TranslationSeedItem("entity.material.warehousestoragecondition", "zh-HK", "仓储存储条件_hk", "仓储存储条件（SAP MARA.WHSTC）"),

            // entity.material.warehousematerialgroup
            new TranslationSeedItem("entity.material.warehousematerialgroup", "en-US", "仓储物料组_us", "仓储物料组（SAP MARA.WHMATGR）"),
            // entity.material.warehousematerialgroup
            new TranslationSeedItem("entity.material.warehousematerialgroup", "ja-JP", "仓储物料组_jp", "仓储物料组（SAP MARA.WHMATGR）"),
            // entity.material.warehousematerialgroup
            new TranslationSeedItem("entity.material.warehousematerialgroup", "zh-CN", "仓储物料组", "仓储物料组（SAP MARA.WHMATGR）"),
            // entity.material.warehousematerialgroup
            new TranslationSeedItem("entity.material.warehousematerialgroup", "zh-HK", "仓储物料组_hk", "仓储物料组（SAP MARA.WHMATGR）"),

            // entity.material.handlingindicator
            new TranslationSeedItem("entity.material.handlingindicator", "en-US", "处理标识_us", "处理标识（SAP MARA.HNDLCODE）"),
            // entity.material.handlingindicator
            new TranslationSeedItem("entity.material.handlingindicator", "ja-JP", "处理标识_jp", "处理标识（SAP MARA.HNDLCODE）"),
            // entity.material.handlingindicator
            new TranslationSeedItem("entity.material.handlingindicator", "zh-CN", "处理标识", "处理标识（SAP MARA.HNDLCODE）"),
            // entity.material.handlingindicator
            new TranslationSeedItem("entity.material.handlingindicator", "zh-HK", "处理标识_hk", "处理标识（SAP MARA.HNDLCODE）"),

            // entity.material.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.material.hazardoussubstancesrelevant", "en-US", "危险物质相关_us", "危险物质相关（SAP MARA.HAZMAT）"),
            // entity.material.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.material.hazardoussubstancesrelevant", "ja-JP", "危险物质相关_jp", "危险物质相关（SAP MARA.HAZMAT）"),
            // entity.material.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.material.hazardoussubstancesrelevant", "zh-CN", "危险物质相关", "危险物质相关（SAP MARA.HAZMAT）"),
            // entity.material.hazardoussubstancesrelevant
            new TranslationSeedItem("entity.material.hazardoussubstancesrelevant", "zh-HK", "危险物质相关_hk", "危险物质相关（SAP MARA.HAZMAT）"),

            // entity.material.handlingunittype
            new TranslationSeedItem("entity.material.handlingunittype", "en-US", "处理单元类型_us", "处理单元类型（SAP MARA.HUTYP）"),
            // entity.material.handlingunittype
            new TranslationSeedItem("entity.material.handlingunittype", "ja-JP", "处理单元类型_jp", "处理单元类型（SAP MARA.HUTYP）"),
            // entity.material.handlingunittype
            new TranslationSeedItem("entity.material.handlingunittype", "zh-CN", "处理单元类型", "处理单元类型（SAP MARA.HUTYP）"),
            // entity.material.handlingunittype
            new TranslationSeedItem("entity.material.handlingunittype", "zh-HK", "处理单元类型_hk", "处理单元类型（SAP MARA.HUTYP）"),

            // entity.material.variabletareweight
            new TranslationSeedItem("entity.material.variabletareweight", "en-US", "可变皮重_us", "可变皮重（SAP MARA.TARE_VAR）"),
            // entity.material.variabletareweight
            new TranslationSeedItem("entity.material.variabletareweight", "ja-JP", "可变皮重_jp", "可变皮重（SAP MARA.TARE_VAR）"),
            // entity.material.variabletareweight
            new TranslationSeedItem("entity.material.variabletareweight", "zh-CN", "可变皮重", "可变皮重（SAP MARA.TARE_VAR）"),
            // entity.material.variabletareweight
            new TranslationSeedItem("entity.material.variabletareweight", "zh-HK", "可变皮重_hk", "可变皮重（SAP MARA.TARE_VAR）"),

            // entity.material.maximumallowedcapacity
            new TranslationSeedItem("entity.material.maximumallowedcapacity", "en-US", "最大允许容量_us", "最大允许容量（SAP MARA.MAXC）"),
            // entity.material.maximumallowedcapacity
            new TranslationSeedItem("entity.material.maximumallowedcapacity", "ja-JP", "最大允许容量_jp", "最大允许容量（SAP MARA.MAXC）"),
            // entity.material.maximumallowedcapacity
            new TranslationSeedItem("entity.material.maximumallowedcapacity", "zh-CN", "最大允许容量", "最大允许容量（SAP MARA.MAXC）"),
            // entity.material.maximumallowedcapacity
            new TranslationSeedItem("entity.material.maximumallowedcapacity", "zh-HK", "最大允许容量_hk", "最大允许容量（SAP MARA.MAXC）"),

            // entity.material.overcapacitytolerance
            new TranslationSeedItem("entity.material.overcapacitytolerance", "en-US", "超容量容差_us", "超容量容差（SAP MARA.MAXC_TOL）"),
            // entity.material.overcapacitytolerance
            new TranslationSeedItem("entity.material.overcapacitytolerance", "ja-JP", "超容量容差_jp", "超容量容差（SAP MARA.MAXC_TOL）"),
            // entity.material.overcapacitytolerance
            new TranslationSeedItem("entity.material.overcapacitytolerance", "zh-CN", "超容量容差", "超容量容差（SAP MARA.MAXC_TOL）"),
            // entity.material.overcapacitytolerance
            new TranslationSeedItem("entity.material.overcapacitytolerance", "zh-HK", "超容量容差_hk", "超容量容差（SAP MARA.MAXC_TOL）"),

            // entity.material.maximumpackinglength
            new TranslationSeedItem("entity.material.maximumpackinglength", "en-US", "最大包装长度_us", "最大包装长度（SAP MARA.MAXL）"),
            // entity.material.maximumpackinglength
            new TranslationSeedItem("entity.material.maximumpackinglength", "ja-JP", "最大包装长度_jp", "最大包装长度（SAP MARA.MAXL）"),
            // entity.material.maximumpackinglength
            new TranslationSeedItem("entity.material.maximumpackinglength", "zh-CN", "最大包装长度", "最大包装长度（SAP MARA.MAXL）"),
            // entity.material.maximumpackinglength
            new TranslationSeedItem("entity.material.maximumpackinglength", "zh-HK", "最大包装长度_hk", "最大包装长度（SAP MARA.MAXL）"),

            // entity.material.maximumpackingwidth
            new TranslationSeedItem("entity.material.maximumpackingwidth", "en-US", "最大包装宽度_us", "最大包装宽度（SAP MARA.MAXB）"),
            // entity.material.maximumpackingwidth
            new TranslationSeedItem("entity.material.maximumpackingwidth", "ja-JP", "最大包装宽度_jp", "最大包装宽度（SAP MARA.MAXB）"),
            // entity.material.maximumpackingwidth
            new TranslationSeedItem("entity.material.maximumpackingwidth", "zh-CN", "最大包装宽度", "最大包装宽度（SAP MARA.MAXB）"),
            // entity.material.maximumpackingwidth
            new TranslationSeedItem("entity.material.maximumpackingwidth", "zh-HK", "最大包装宽度_hk", "最大包装宽度（SAP MARA.MAXB）"),

            // entity.material.maximumpackingheight
            new TranslationSeedItem("entity.material.maximumpackingheight", "en-US", "最大包装高度_us", "最大包装高度（SAP MARA.MAXH）"),
            // entity.material.maximumpackingheight
            new TranslationSeedItem("entity.material.maximumpackingheight", "ja-JP", "最大包装高度_jp", "最大包装高度（SAP MARA.MAXH）"),
            // entity.material.maximumpackingheight
            new TranslationSeedItem("entity.material.maximumpackingheight", "zh-CN", "最大包装高度", "最大包装高度（SAP MARA.MAXH）"),
            // entity.material.maximumpackingheight
            new TranslationSeedItem("entity.material.maximumpackingheight", "zh-HK", "最大包装高度_hk", "最大包装高度（SAP MARA.MAXH）"),

            // entity.material.maximumpackingdimensionunit
            new TranslationSeedItem("entity.material.maximumpackingdimensionunit", "en-US", "最大包装尺寸单位_us", "最大包装尺寸单位（SAP MARA.MAXDIM_UOM）"),
            // entity.material.maximumpackingdimensionunit
            new TranslationSeedItem("entity.material.maximumpackingdimensionunit", "ja-JP", "最大包装尺寸单位_jp", "最大包装尺寸单位（SAP MARA.MAXDIM_UOM）"),
            // entity.material.maximumpackingdimensionunit
            new TranslationSeedItem("entity.material.maximumpackingdimensionunit", "zh-CN", "最大包装尺寸单位", "最大包装尺寸单位（SAP MARA.MAXDIM_UOM）"),
            // entity.material.maximumpackingdimensionunit
            new TranslationSeedItem("entity.material.maximumpackingdimensionunit", "zh-HK", "最大包装尺寸单位_hk", "最大包装尺寸单位（SAP MARA.MAXDIM_UOM）"),

            // entity.material.countryoforigin
            new TranslationSeedItem("entity.material.countryoforigin", "en-US", "原产国_us", "原产国（SAP MARA.HERKL）"),
            // entity.material.countryoforigin
            new TranslationSeedItem("entity.material.countryoforigin", "ja-JP", "原产国_jp", "原产国（SAP MARA.HERKL）"),
            // entity.material.countryoforigin
            new TranslationSeedItem("entity.material.countryoforigin", "zh-CN", "原产国", "原产国（SAP MARA.HERKL）"),
            // entity.material.countryoforigin
            new TranslationSeedItem("entity.material.countryoforigin", "zh-HK", "原产国_hk", "原产国（SAP MARA.HERKL）"),

            // entity.material.freightgroup
            new TranslationSeedItem("entity.material.freightgroup", "en-US", "物料运费组_us", "物料运费组（SAP MARA.MFRGR）"),
            // entity.material.freightgroup
            new TranslationSeedItem("entity.material.freightgroup", "ja-JP", "物料运费组_jp", "物料运费组（SAP MARA.MFRGR）"),
            // entity.material.freightgroup
            new TranslationSeedItem("entity.material.freightgroup", "zh-CN", "物料运费组", "物料运费组（SAP MARA.MFRGR）"),
            // entity.material.freightgroup
            new TranslationSeedItem("entity.material.freightgroup", "zh-HK", "物料运费组_hk", "物料运费组（SAP MARA.MFRGR）"),

            // entity.material.quarantineperiod
            new TranslationSeedItem("entity.material.quarantineperiod", "en-US", "隔离期_us", "隔离期（SAP MARA.QQTIME）"),
            // entity.material.quarantineperiod
            new TranslationSeedItem("entity.material.quarantineperiod", "ja-JP", "隔离期_jp", "隔离期（SAP MARA.QQTIME）"),
            // entity.material.quarantineperiod
            new TranslationSeedItem("entity.material.quarantineperiod", "zh-CN", "隔离期", "隔离期（SAP MARA.QQTIME）"),
            // entity.material.quarantineperiod
            new TranslationSeedItem("entity.material.quarantineperiod", "zh-HK", "隔离期_hk", "隔离期（SAP MARA.QQTIME）"),

            // entity.material.quarantineperiodunit
            new TranslationSeedItem("entity.material.quarantineperiodunit", "en-US", "隔离期单位_us", "隔离期单位（SAP MARA.QQTIMEUOM）"),
            // entity.material.quarantineperiodunit
            new TranslationSeedItem("entity.material.quarantineperiodunit", "ja-JP", "隔离期单位_jp", "隔离期单位（SAP MARA.QQTIMEUOM）"),
            // entity.material.quarantineperiodunit
            new TranslationSeedItem("entity.material.quarantineperiodunit", "zh-CN", "隔离期单位", "隔离期单位（SAP MARA.QQTIMEUOM）"),
            // entity.material.quarantineperiodunit
            new TranslationSeedItem("entity.material.quarantineperiodunit", "zh-HK", "隔离期单位_hk", "隔离期单位（SAP MARA.QQTIMEUOM）"),

            // entity.material.qualityinspectiongroup
            new TranslationSeedItem("entity.material.qualityinspectiongroup", "en-US", "质检组_us", "质检组（SAP MARA.QGRP）"),
            // entity.material.qualityinspectiongroup
            new TranslationSeedItem("entity.material.qualityinspectiongroup", "ja-JP", "质检组_jp", "质检组（SAP MARA.QGRP）"),
            // entity.material.qualityinspectiongroup
            new TranslationSeedItem("entity.material.qualityinspectiongroup", "zh-CN", "质检组", "质检组（SAP MARA.QGRP）"),
            // entity.material.qualityinspectiongroup
            new TranslationSeedItem("entity.material.qualityinspectiongroup", "zh-HK", "质检组_hk", "质检组（SAP MARA.QGRP）"),

            // entity.material.serialnumberprofile
            new TranslationSeedItem("entity.material.serialnumberprofile", "en-US", "序列号参数文件_us", "序列号参数文件（SAP MARA.SERIAL）"),
            // entity.material.serialnumberprofile
            new TranslationSeedItem("entity.material.serialnumberprofile", "ja-JP", "序列号参数文件_jp", "序列号参数文件（SAP MARA.SERIAL）"),
            // entity.material.serialnumberprofile
            new TranslationSeedItem("entity.material.serialnumberprofile", "zh-CN", "序列号参数文件", "序列号参数文件（SAP MARA.SERIAL）"),
            // entity.material.serialnumberprofile
            new TranslationSeedItem("entity.material.serialnumberprofile", "zh-HK", "序列号参数文件_hk", "序列号参数文件（SAP MARA.SERIAL）"),

            // entity.material.formname
            new TranslationSeedItem("entity.material.formname", "en-US", "表单名称_us", "表单名称（SAP MARA.PS_SMARTFORM）"),
            // entity.material.formname
            new TranslationSeedItem("entity.material.formname", "ja-JP", "表单名称_jp", "表单名称（SAP MARA.PS_SMARTFORM）"),
            // entity.material.formname
            new TranslationSeedItem("entity.material.formname", "zh-CN", "表单名称", "表单名称（SAP MARA.PS_SMARTFORM）"),
            // entity.material.formname
            new TranslationSeedItem("entity.material.formname", "zh-HK", "表单名称_hk", "表单名称（SAP MARA.PS_SMARTFORM）"),

            // entity.material.logisticsunitofmeasure
            new TranslationSeedItem("entity.material.logisticsunitofmeasure", "en-US", "后勤计量单位_us", "后勤计量单位（SAP MARA.LOGUNIT）"),
            // entity.material.logisticsunitofmeasure
            new TranslationSeedItem("entity.material.logisticsunitofmeasure", "ja-JP", "后勤计量单位_jp", "后勤计量单位（SAP MARA.LOGUNIT）"),
            // entity.material.logisticsunitofmeasure
            new TranslationSeedItem("entity.material.logisticsunitofmeasure", "zh-CN", "后勤计量单位", "后勤计量单位（SAP MARA.LOGUNIT）"),
            // entity.material.logisticsunitofmeasure
            new TranslationSeedItem("entity.material.logisticsunitofmeasure", "zh-HK", "后勤计量单位_hk", "后勤计量单位（SAP MARA.LOGUNIT）"),

            // entity.material.catchweightmaterial
            new TranslationSeedItem("entity.material.catchweightmaterial", "en-US", "捕捞重量物料_us", "捕捞重量物料（SAP MARA.CWQREL）"),
            // entity.material.catchweightmaterial
            new TranslationSeedItem("entity.material.catchweightmaterial", "ja-JP", "捕捞重量物料_jp", "捕捞重量物料（SAP MARA.CWQREL）"),
            // entity.material.catchweightmaterial
            new TranslationSeedItem("entity.material.catchweightmaterial", "zh-CN", "捕捞重量物料", "捕捞重量物料（SAP MARA.CWQREL）"),
            // entity.material.catchweightmaterial
            new TranslationSeedItem("entity.material.catchweightmaterial", "zh-HK", "捕捞重量物料_hk", "捕捞重量物料（SAP MARA.CWQREL）"),

            // entity.material.catchweightprofile
            new TranslationSeedItem("entity.material.catchweightprofile", "en-US", "捕捞重量参数文件_us", "捕捞重量参数文件（SAP MARA.CWQPROC）"),
            // entity.material.catchweightprofile
            new TranslationSeedItem("entity.material.catchweightprofile", "ja-JP", "捕捞重量参数文件_jp", "捕捞重量参数文件（SAP MARA.CWQPROC）"),
            // entity.material.catchweightprofile
            new TranslationSeedItem("entity.material.catchweightprofile", "zh-CN", "捕捞重量参数文件", "捕捞重量参数文件（SAP MARA.CWQPROC）"),
            // entity.material.catchweightprofile
            new TranslationSeedItem("entity.material.catchweightprofile", "zh-HK", "捕捞重量参数文件_hk", "捕捞重量参数文件（SAP MARA.CWQPROC）"),

            // entity.material.catchweighttolerancegroup
            new TranslationSeedItem("entity.material.catchweighttolerancegroup", "en-US", "捕捞重量容差组_us", "捕捞重量容差组（SAP MARA.CWQTOLGR）"),
            // entity.material.catchweighttolerancegroup
            new TranslationSeedItem("entity.material.catchweighttolerancegroup", "ja-JP", "捕捞重量容差组_jp", "捕捞重量容差组（SAP MARA.CWQTOLGR）"),
            // entity.material.catchweighttolerancegroup
            new TranslationSeedItem("entity.material.catchweighttolerancegroup", "zh-CN", "捕捞重量容差组", "捕捞重量容差组（SAP MARA.CWQTOLGR）"),
            // entity.material.catchweighttolerancegroup
            new TranslationSeedItem("entity.material.catchweighttolerancegroup", "zh-HK", "捕捞重量容差组_hk", "捕捞重量容差组（SAP MARA.CWQTOLGR）"),

            // entity.material.adjustmentprofile
            new TranslationSeedItem("entity.material.adjustmentprofile", "en-US", "调整参数文件_us", "调整参数文件（SAP MARA.ADPROF）"),
            // entity.material.adjustmentprofile
            new TranslationSeedItem("entity.material.adjustmentprofile", "ja-JP", "调整参数文件_jp", "调整参数文件（SAP MARA.ADPROF）"),
            // entity.material.adjustmentprofile
            new TranslationSeedItem("entity.material.adjustmentprofile", "zh-CN", "调整参数文件", "调整参数文件（SAP MARA.ADPROF）"),
            // entity.material.adjustmentprofile
            new TranslationSeedItem("entity.material.adjustmentprofile", "zh-HK", "调整参数文件_hk", "调整参数文件（SAP MARA.ADPROF）"),

            // entity.material.intellectualpropertyid
            new TranslationSeedItem("entity.material.intellectualpropertyid", "en-US", "知识产权ID_us", "知识产权ID（SAP MARA.IPMIPPRODUCT）"),
            // entity.material.intellectualpropertyid
            new TranslationSeedItem("entity.material.intellectualpropertyid", "ja-JP", "知识产权ID_jp", "知识产权ID（SAP MARA.IPMIPPRODUCT）"),
            // entity.material.intellectualpropertyid
            new TranslationSeedItem("entity.material.intellectualpropertyid", "zh-CN", "知识产权ID", "知识产权ID（SAP MARA.IPMIPPRODUCT）"),
            // entity.material.intellectualpropertyid
            new TranslationSeedItem("entity.material.intellectualpropertyid", "zh-HK", "知识产权ID_hk", "知识产权ID（SAP MARA.IPMIPPRODUCT）"),

            // entity.material.variantpriceallowed
            new TranslationSeedItem("entity.material.variantpriceallowed", "en-US", "允许变式价格_us", "允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）"),
            // entity.material.variantpriceallowed
            new TranslationSeedItem("entity.material.variantpriceallowed", "ja-JP", "允许变式价格_jp", "允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）"),
            // entity.material.variantpriceallowed
            new TranslationSeedItem("entity.material.variantpriceallowed", "zh-CN", "允许变式价格", "允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）"),
            // entity.material.variantpriceallowed
            new TranslationSeedItem("entity.material.variantpriceallowed", "zh-HK", "允许变式价格_hk", "允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）"),

            // entity.material.medium
            new TranslationSeedItem("entity.material.medium", "en-US", "介质_us", "介质（SAP MARA.MEDIUM）"),
            // entity.material.medium
            new TranslationSeedItem("entity.material.medium", "ja-JP", "介质_jp", "介质（SAP MARA.MEDIUM）"),
            // entity.material.medium
            new TranslationSeedItem("entity.material.medium", "zh-CN", "介质", "介质（SAP MARA.MEDIUM）"),
            // entity.material.medium
            new TranslationSeedItem("entity.material.medium", "zh-HK", "介质_hk", "介质（SAP MARA.MEDIUM）"),

            // entity.material.physicalcommodity
            new TranslationSeedItem("entity.material.physicalcommodity", "en-US", "实物商品_us", "实物商品（SAP MARA.COMMODITY）"),
            // entity.material.physicalcommodity
            new TranslationSeedItem("entity.material.physicalcommodity", "ja-JP", "实物商品_jp", "实物商品（SAP MARA.COMMODITY）"),
            // entity.material.physicalcommodity
            new TranslationSeedItem("entity.material.physicalcommodity", "zh-CN", "实物商品", "实物商品（SAP MARA.COMMODITY）"),
            // entity.material.physicalcommodity
            new TranslationSeedItem("entity.material.physicalcommodity", "zh-HK", "实物商品_hk", "实物商品（SAP MARA.COMMODITY）"),

            // entity.material.animalorigin
            new TranslationSeedItem("entity.material.animalorigin", "en-US", "动物源_us", "动物源（SAP MARA.ANIMAL_ORIGIN）"),
            // entity.material.animalorigin
            new TranslationSeedItem("entity.material.animalorigin", "ja-JP", "动物源_jp", "动物源（SAP MARA.ANIMAL_ORIGIN）"),
            // entity.material.animalorigin
            new TranslationSeedItem("entity.material.animalorigin", "zh-CN", "动物源", "动物源（SAP MARA.ANIMAL_ORIGIN）"),
            // entity.material.animalorigin
            new TranslationSeedItem("entity.material.animalorigin", "zh-HK", "动物源_hk", "动物源（SAP MARA.ANIMAL_ORIGIN）"),

            // entity.material.textilecompositionfunction
            new TranslationSeedItem("entity.material.textilecompositionfunction", "en-US", "纺织成分功能_us", "纺织成分功能（SAP MARA.TEXTILE_COMP_IND）"),
            // entity.material.textilecompositionfunction
            new TranslationSeedItem("entity.material.textilecompositionfunction", "ja-JP", "纺织成分功能_jp", "纺织成分功能（SAP MARA.TEXTILE_COMP_IND）"),
            // entity.material.textilecompositionfunction
            new TranslationSeedItem("entity.material.textilecompositionfunction", "zh-CN", "纺织成分功能", "纺织成分功能（SAP MARA.TEXTILE_COMP_IND）"),
            // entity.material.textilecompositionfunction
            new TranslationSeedItem("entity.material.textilecompositionfunction", "zh-HK", "纺织成分功能_hk", "纺织成分功能（SAP MARA.TEXTILE_COMP_IND）"),

            // entity.material.segmentationstructure
            new TranslationSeedItem("entity.material.segmentationstructure", "en-US", "细分结构_us", "细分结构（SAP MARA.SGT_CSGR）"),
            // entity.material.segmentationstructure
            new TranslationSeedItem("entity.material.segmentationstructure", "ja-JP", "细分结构_jp", "细分结构（SAP MARA.SGT_CSGR）"),
            // entity.material.segmentationstructure
            new TranslationSeedItem("entity.material.segmentationstructure", "zh-CN", "细分结构", "细分结构（SAP MARA.SGT_CSGR）"),
            // entity.material.segmentationstructure
            new TranslationSeedItem("entity.material.segmentationstructure", "zh-HK", "细分结构_hk", "细分结构（SAP MARA.SGT_CSGR）"),

            // entity.material.segmentationstrategy
            new TranslationSeedItem("entity.material.segmentationstrategy", "en-US", "细分策略_us", "细分策略（SAP MARA.SGT_COVSA）"),
            // entity.material.segmentationstrategy
            new TranslationSeedItem("entity.material.segmentationstrategy", "ja-JP", "细分策略_jp", "细分策略（SAP MARA.SGT_COVSA）"),
            // entity.material.segmentationstrategy
            new TranslationSeedItem("entity.material.segmentationstrategy", "zh-CN", "细分策略", "细分策略（SAP MARA.SGT_COVSA）"),
            // entity.material.segmentationstrategy
            new TranslationSeedItem("entity.material.segmentationstrategy", "zh-HK", "细分策略_hk", "细分策略（SAP MARA.SGT_COVSA）"),

            // entity.material.segmentationstatus
            new TranslationSeedItem("entity.material.segmentationstatus", "en-US", "细分状态_us", "细分状态（SAP MARA.SGT_STAT）"),
            // entity.material.segmentationstatus
            new TranslationSeedItem("entity.material.segmentationstatus", "ja-JP", "细分状态_jp", "细分状态（SAP MARA.SGT_STAT）"),
            // entity.material.segmentationstatus
            new TranslationSeedItem("entity.material.segmentationstatus", "zh-CN", "细分状态", "细分状态（SAP MARA.SGT_STAT）"),
            // entity.material.segmentationstatus
            new TranslationSeedItem("entity.material.segmentationstatus", "zh-HK", "细分状态_hk", "细分状态（SAP MARA.SGT_STAT）"),

            // entity.material.segmentationscope
            new TranslationSeedItem("entity.material.segmentationscope", "en-US", "细分范围_us", "细分范围（SAP MARA.SGT_SCOPE）"),
            // entity.material.segmentationscope
            new TranslationSeedItem("entity.material.segmentationscope", "ja-JP", "细分范围_jp", "细分范围（SAP MARA.SGT_SCOPE）"),
            // entity.material.segmentationscope
            new TranslationSeedItem("entity.material.segmentationscope", "zh-CN", "细分范围", "细分范围（SAP MARA.SGT_SCOPE）"),
            // entity.material.segmentationscope
            new TranslationSeedItem("entity.material.segmentationscope", "zh-HK", "细分范围_hk", "细分范围（SAP MARA.SGT_SCOPE）"),

            // entity.material.segmentationrelevant
            new TranslationSeedItem("entity.material.segmentationrelevant", "en-US", "细分相关_us", "细分相关（SAP MARA.SGT_REL）"),
            // entity.material.segmentationrelevant
            new TranslationSeedItem("entity.material.segmentationrelevant", "ja-JP", "细分相关_jp", "细分相关（SAP MARA.SGT_REL）"),
            // entity.material.segmentationrelevant
            new TranslationSeedItem("entity.material.segmentationrelevant", "zh-CN", "细分相关", "细分相关（SAP MARA.SGT_REL）"),
            // entity.material.segmentationrelevant
            new TranslationSeedItem("entity.material.segmentationrelevant", "zh-HK", "细分相关_hk", "细分相关（SAP MARA.SGT_REL）"),

            // entity.material.fashionattribute1
            new TranslationSeedItem("entity.material.fashionattribute1", "en-US", "时装属性1_us", "时装属性1（SAP MARA.FSH_MG_AT1）"),
            // entity.material.fashionattribute1
            new TranslationSeedItem("entity.material.fashionattribute1", "ja-JP", "时装属性1_jp", "时装属性1（SAP MARA.FSH_MG_AT1）"),
            // entity.material.fashionattribute1
            new TranslationSeedItem("entity.material.fashionattribute1", "zh-CN", "时装属性1", "时装属性1（SAP MARA.FSH_MG_AT1）"),
            // entity.material.fashionattribute1
            new TranslationSeedItem("entity.material.fashionattribute1", "zh-HK", "时装属性1_hk", "时装属性1（SAP MARA.FSH_MG_AT1）"),

            // entity.material.fashionattribute2
            new TranslationSeedItem("entity.material.fashionattribute2", "en-US", "时装属性2_us", "时装属性2（SAP MARA.FSH_MG_AT2）"),
            // entity.material.fashionattribute2
            new TranslationSeedItem("entity.material.fashionattribute2", "ja-JP", "时装属性2_jp", "时装属性2（SAP MARA.FSH_MG_AT2）"),
            // entity.material.fashionattribute2
            new TranslationSeedItem("entity.material.fashionattribute2", "zh-CN", "时装属性2", "时装属性2（SAP MARA.FSH_MG_AT2）"),
            // entity.material.fashionattribute2
            new TranslationSeedItem("entity.material.fashionattribute2", "zh-HK", "时装属性2_hk", "时装属性2（SAP MARA.FSH_MG_AT2）"),

            // entity.material.fashionattribute3
            new TranslationSeedItem("entity.material.fashionattribute3", "en-US", "时装属性3_us", "时装属性3（SAP MARA.FSH_MG_AT3）"),
            // entity.material.fashionattribute3
            new TranslationSeedItem("entity.material.fashionattribute3", "ja-JP", "时装属性3_jp", "时装属性3（SAP MARA.FSH_MG_AT3）"),
            // entity.material.fashionattribute3
            new TranslationSeedItem("entity.material.fashionattribute3", "zh-CN", "时装属性3", "时装属性3（SAP MARA.FSH_MG_AT3）"),
            // entity.material.fashionattribute3
            new TranslationSeedItem("entity.material.fashionattribute3", "zh-HK", "时装属性3_hk", "时装属性3（SAP MARA.FSH_MG_AT3）"),

            // entity.material.seasonusageindicator
            new TranslationSeedItem("entity.material.seasonusageindicator", "en-US", "季节使用标识_us", "季节使用标识（SAP MARA.FSH_SEALV）"),
            // entity.material.seasonusageindicator
            new TranslationSeedItem("entity.material.seasonusageindicator", "ja-JP", "季节使用标识_jp", "季节使用标识（SAP MARA.FSH_SEALV）"),
            // entity.material.seasonusageindicator
            new TranslationSeedItem("entity.material.seasonusageindicator", "zh-CN", "季节使用标识", "季节使用标识（SAP MARA.FSH_SEALV）"),
            // entity.material.seasonusageindicator
            new TranslationSeedItem("entity.material.seasonusageindicator", "zh-HK", "季节使用标识_hk", "季节使用标识（SAP MARA.FSH_SEALV）"),

            // entity.material.seasonactiveininventory
            new TranslationSeedItem("entity.material.seasonactiveininventory", "en-US", "库存季节激活_us", "库存季节激活（SAP MARA.FSH_SEAIM）"),
            // entity.material.seasonactiveininventory
            new TranslationSeedItem("entity.material.seasonactiveininventory", "ja-JP", "库存季节激活_jp", "库存季节激活（SAP MARA.FSH_SEAIM）"),
            // entity.material.seasonactiveininventory
            new TranslationSeedItem("entity.material.seasonactiveininventory", "zh-CN", "库存季节激活", "库存季节激活（SAP MARA.FSH_SEAIM）"),
            // entity.material.seasonactiveininventory
            new TranslationSeedItem("entity.material.seasonactiveininventory", "zh-HK", "库存季节激活_hk", "库存季节激活（SAP MARA.FSH_SEAIM）"),

            // entity.material.characteristicconversionid
            new TranslationSeedItem("entity.material.characteristicconversionid", "en-US", "特性转换ID_us", "特性转换ID（SAP MARA.FSH_SC_MID）"),
            // entity.material.characteristicconversionid
            new TranslationSeedItem("entity.material.characteristicconversionid", "ja-JP", "特性转换ID_jp", "特性转换ID（SAP MARA.FSH_SC_MID）"),
            // entity.material.characteristicconversionid
            new TranslationSeedItem("entity.material.characteristicconversionid", "zh-CN", "特性转换ID", "特性转换ID（SAP MARA.FSH_SC_MID）"),
            // entity.material.characteristicconversionid
            new TranslationSeedItem("entity.material.characteristicconversionid", "zh-HK", "特性转换ID_hk", "特性转换ID（SAP MARA.FSH_SC_MID）"),

            // entity.material.anpcode
            new TranslationSeedItem("entity.material.anpcode", "en-US", "ANP代码_us", "ANP代码（SAP MARA.ANP）"),
            // entity.material.anpcode
            new TranslationSeedItem("entity.material.anpcode", "ja-JP", "ANP代码_jp", "ANP代码（SAP MARA.ANP）"),
            // entity.material.anpcode
            new TranslationSeedItem("entity.material.anpcode", "zh-CN", "ANP代码", "ANP代码（SAP MARA.ANP）"),
            // entity.material.anpcode
            new TranslationSeedItem("entity.material.anpcode", "zh-HK", "ANP代码_hk", "ANP代码（SAP MARA.ANP）"),

            // entity.material.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.material.dangerousgoodspackagingstatus", "en-US", "危险品包装状态_us", "危险品包装状态（SAP MARA.DG_PACK_STATUS）"),
            // entity.material.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.material.dangerousgoodspackagingstatus", "ja-JP", "危险品包装状态_jp", "危险品包装状态（SAP MARA.DG_PACK_STATUS）"),
            // entity.material.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.material.dangerousgoodspackagingstatus", "zh-CN", "危险品包装状态", "危险品包装状态（SAP MARA.DG_PACK_STATUS）"),
            // entity.material.dangerousgoodspackagingstatus
            new TranslationSeedItem("entity.material.dangerousgoodspackagingstatus", "zh-HK", "危险品包装状态_hk", "危险品包装状态（SAP MARA.DG_PACK_STATUS）"),

            // entity.material.conditionmanagement
            new TranslationSeedItem("entity.material.conditionmanagement", "en-US", "物料条件管理_us", "物料条件管理（SAP MARA.MCOND）"),
            // entity.material.conditionmanagement
            new TranslationSeedItem("entity.material.conditionmanagement", "ja-JP", "物料条件管理_jp", "物料条件管理（SAP MARA.MCOND）"),
            // entity.material.conditionmanagement
            new TranslationSeedItem("entity.material.conditionmanagement", "zh-CN", "物料条件管理", "物料条件管理（SAP MARA.MCOND）"),
            // entity.material.conditionmanagement
            new TranslationSeedItem("entity.material.conditionmanagement", "zh-HK", "物料条件管理_hk", "物料条件管理（SAP MARA.MCOND）"),

            // entity.material.returncode
            new TranslationSeedItem("entity.material.returncode", "en-US", "退货代码_us", "退货代码（SAP MARA.RETDELC）"),
            // entity.material.returncode
            new TranslationSeedItem("entity.material.returncode", "ja-JP", "退货代码_jp", "退货代码（SAP MARA.RETDELC）"),
            // entity.material.returncode
            new TranslationSeedItem("entity.material.returncode", "zh-CN", "退货代码", "退货代码（SAP MARA.RETDELC）"),
            // entity.material.returncode
            new TranslationSeedItem("entity.material.returncode", "zh-HK", "退货代码_hk", "退货代码（SAP MARA.RETDELC）"),

            // entity.material.returntologisticslevel
            new TranslationSeedItem("entity.material.returntologisticslevel", "en-US", "退回后勤级别_us", "退回后勤级别（SAP MARA.LOGLEV_RETO）"),
            // entity.material.returntologisticslevel
            new TranslationSeedItem("entity.material.returntologisticslevel", "ja-JP", "退回后勤级别_jp", "退回后勤级别（SAP MARA.LOGLEV_RETO）"),
            // entity.material.returntologisticslevel
            new TranslationSeedItem("entity.material.returntologisticslevel", "zh-CN", "退回后勤级别", "退回后勤级别（SAP MARA.LOGLEV_RETO）"),
            // entity.material.returntologisticslevel
            new TranslationSeedItem("entity.material.returntologisticslevel", "zh-HK", "退回后勤级别_hk", "退回后勤级别（SAP MARA.LOGLEV_RETO）"),

            // entity.material.natoitemidentificationnumber
            new TranslationSeedItem("entity.material.natoitemidentificationnumber", "en-US", "NATO物料识别号_us", "NATO物料识别号（SAP MARA.NSNID）"),
            // entity.material.natoitemidentificationnumber
            new TranslationSeedItem("entity.material.natoitemidentificationnumber", "ja-JP", "NATO物料识别号_jp", "NATO物料识别号（SAP MARA.NSNID）"),
            // entity.material.natoitemidentificationnumber
            new TranslationSeedItem("entity.material.natoitemidentificationnumber", "zh-CN", "NATO物料识别号", "NATO物料识别号（SAP MARA.NSNID）"),
            // entity.material.natoitemidentificationnumber
            new TranslationSeedItem("entity.material.natoitemidentificationnumber", "zh-HK", "NATO物料识别号_hk", "NATO物料识别号（SAP MARA.NSNID）"),

            // entity.material.fffclass
            new TranslationSeedItem("entity.material.fffclass", "en-US", "FFF类别_us", "FFF类别（SAP MARA.IMATN）"),
            // entity.material.fffclass
            new TranslationSeedItem("entity.material.fffclass", "ja-JP", "FFF类别_jp", "FFF类别（SAP MARA.IMATN）"),
            // entity.material.fffclass
            new TranslationSeedItem("entity.material.fffclass", "zh-CN", "FFF类别", "FFF类别（SAP MARA.IMATN）"),
            // entity.material.fffclass
            new TranslationSeedItem("entity.material.fffclass", "zh-HK", "FFF类别_hk", "FFF类别（SAP MARA.IMATN）"),

            // entity.material.supersessionchainnumber
            new TranslationSeedItem("entity.material.supersessionchainnumber", "en-US", "替代链编码_us", "替代链编码（SAP MARA.PICNUM）"),
            // entity.material.supersessionchainnumber
            new TranslationSeedItem("entity.material.supersessionchainnumber", "ja-JP", "替代链编码_jp", "替代链编码（SAP MARA.PICNUM）"),
            // entity.material.supersessionchainnumber
            new TranslationSeedItem("entity.material.supersessionchainnumber", "zh-CN", "替代链编码", "替代链编码（SAP MARA.PICNUM）"),
            // entity.material.supersessionchainnumber
            new TranslationSeedItem("entity.material.supersessionchainnumber", "zh-HK", "替代链编码_hk", "替代链编码（SAP MARA.PICNUM）"),

            // entity.material.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.material.seasonalprocurementcreationstatus", "en-US", "季节采购创建状态_us", "季节采购创建状态（SAP MARA.BSTAT）"),
            // entity.material.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.material.seasonalprocurementcreationstatus", "ja-JP", "季节采购创建状态_jp", "季节采购创建状态（SAP MARA.BSTAT）"),
            // entity.material.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.material.seasonalprocurementcreationstatus", "zh-CN", "季节采购创建状态", "季节采购创建状态（SAP MARA.BSTAT）"),
            // entity.material.seasonalprocurementcreationstatus
            new TranslationSeedItem("entity.material.seasonalprocurementcreationstatus", "zh-HK", "季节采购创建状态_hk", "季节采购创建状态（SAP MARA.BSTAT）"),

            // entity.material.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.material.colorcharacteristicinternalnumber", "en-US", "颜色特性内部号_us", "颜色特性内部号（SAP MARA.COLOR_ATINN）"),
            // entity.material.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.material.colorcharacteristicinternalnumber", "ja-JP", "颜色特性内部号_jp", "颜色特性内部号（SAP MARA.COLOR_ATINN）"),
            // entity.material.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.material.colorcharacteristicinternalnumber", "zh-CN", "颜色特性内部号", "颜色特性内部号（SAP MARA.COLOR_ATINN）"),
            // entity.material.colorcharacteristicinternalnumber
            new TranslationSeedItem("entity.material.colorcharacteristicinternalnumber", "zh-HK", "颜色特性内部号_hk", "颜色特性内部号（SAP MARA.COLOR_ATINN）"),

            // entity.material.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.mainsizecharacteristicinternalnumber", "en-US", "主尺码特性内部号_us", "主尺码特性内部号（SAP MARA.SIZE1_ATINN）"),
            // entity.material.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.mainsizecharacteristicinternalnumber", "ja-JP", "主尺码特性内部号_jp", "主尺码特性内部号（SAP MARA.SIZE1_ATINN）"),
            // entity.material.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.mainsizecharacteristicinternalnumber", "zh-CN", "主尺码特性内部号", "主尺码特性内部号（SAP MARA.SIZE1_ATINN）"),
            // entity.material.mainsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.mainsizecharacteristicinternalnumber", "zh-HK", "主尺码特性内部号_hk", "主尺码特性内部号（SAP MARA.SIZE1_ATINN）"),

            // entity.material.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.secondsizecharacteristicinternalnumber", "en-US", "次尺码特性内部号_us", "次尺码特性内部号（SAP MARA.SIZE2_ATINN）"),
            // entity.material.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.secondsizecharacteristicinternalnumber", "ja-JP", "次尺码特性内部号_jp", "次尺码特性内部号（SAP MARA.SIZE2_ATINN）"),
            // entity.material.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.secondsizecharacteristicinternalnumber", "zh-CN", "次尺码特性内部号", "次尺码特性内部号（SAP MARA.SIZE2_ATINN）"),
            // entity.material.secondsizecharacteristicinternalnumber
            new TranslationSeedItem("entity.material.secondsizecharacteristicinternalnumber", "zh-HK", "次尺码特性内部号_hk", "次尺码特性内部号（SAP MARA.SIZE2_ATINN）"),

            // entity.material.color
            new TranslationSeedItem("entity.material.color", "en-US", "颜色_us", "颜色（SAP MARA.COLOR）"),
            // entity.material.color
            new TranslationSeedItem("entity.material.color", "ja-JP", "颜色_jp", "颜色（SAP MARA.COLOR）"),
            // entity.material.color
            new TranslationSeedItem("entity.material.color", "zh-CN", "颜色", "颜色（SAP MARA.COLOR）"),
            // entity.material.color
            new TranslationSeedItem("entity.material.color", "zh-HK", "颜色_hk", "颜色（SAP MARA.COLOR）"),

            // entity.material.mainsize
            new TranslationSeedItem("entity.material.mainsize", "en-US", "主尺码_us", "主尺码（SAP MARA.SIZE1）"),
            // entity.material.mainsize
            new TranslationSeedItem("entity.material.mainsize", "ja-JP", "主尺码_jp", "主尺码（SAP MARA.SIZE1）"),
            // entity.material.mainsize
            new TranslationSeedItem("entity.material.mainsize", "zh-CN", "主尺码", "主尺码（SAP MARA.SIZE1）"),
            // entity.material.mainsize
            new TranslationSeedItem("entity.material.mainsize", "zh-HK", "主尺码_hk", "主尺码（SAP MARA.SIZE1）"),

            // entity.material.secondsize
            new TranslationSeedItem("entity.material.secondsize", "en-US", "次尺码_us", "次尺码（SAP MARA.SIZE2）"),
            // entity.material.secondsize
            new TranslationSeedItem("entity.material.secondsize", "ja-JP", "次尺码_jp", "次尺码（SAP MARA.SIZE2）"),
            // entity.material.secondsize
            new TranslationSeedItem("entity.material.secondsize", "zh-CN", "次尺码", "次尺码（SAP MARA.SIZE2）"),
            // entity.material.secondsize
            new TranslationSeedItem("entity.material.secondsize", "zh-HK", "次尺码_hk", "次尺码（SAP MARA.SIZE2）"),

            // entity.material.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.material.evaluationcharacteristicvalue", "en-US", "评估特性值_us", "评估特性值（SAP MARA.FREE_CHAR）"),
            // entity.material.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.material.evaluationcharacteristicvalue", "ja-JP", "评估特性值_jp", "评估特性值（SAP MARA.FREE_CHAR）"),
            // entity.material.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.material.evaluationcharacteristicvalue", "zh-CN", "评估特性值", "评估特性值（SAP MARA.FREE_CHAR）"),
            // entity.material.evaluationcharacteristicvalue
            new TranslationSeedItem("entity.material.evaluationcharacteristicvalue", "zh-HK", "评估特性值_hk", "评估特性值（SAP MARA.FREE_CHAR）"),

            // entity.material.carecode
            new TranslationSeedItem("entity.material.carecode", "en-US", "护理代码_us", "护理代码（SAP MARA.CARE_CODE）"),
            // entity.material.carecode
            new TranslationSeedItem("entity.material.carecode", "ja-JP", "护理代码_jp", "护理代码（SAP MARA.CARE_CODE）"),
            // entity.material.carecode
            new TranslationSeedItem("entity.material.carecode", "zh-CN", "护理代码", "护理代码（SAP MARA.CARE_CODE）"),
            // entity.material.carecode
            new TranslationSeedItem("entity.material.carecode", "zh-HK", "护理代码_hk", "护理代码（SAP MARA.CARE_CODE）"),

            // entity.material.brandid
            new TranslationSeedItem("entity.material.brandid", "en-US", "品牌_us", "品牌（SAP MARA.BRAND_ID）"),
            // entity.material.brandid
            new TranslationSeedItem("entity.material.brandid", "ja-JP", "品牌_jp", "品牌（SAP MARA.BRAND_ID）"),
            // entity.material.brandid
            new TranslationSeedItem("entity.material.brandid", "zh-CN", "品牌", "品牌（SAP MARA.BRAND_ID）"),
            // entity.material.brandid
            new TranslationSeedItem("entity.material.brandid", "zh-HK", "品牌_hk", "品牌（SAP MARA.BRAND_ID）"),

            // entity.material.fibercode1
            new TranslationSeedItem("entity.material.fibercode1", "en-US", "纤维代码1_us", "纤维代码1（SAP MARA.FIBER_CODE1）"),
            // entity.material.fibercode1
            new TranslationSeedItem("entity.material.fibercode1", "ja-JP", "纤维代码1_jp", "纤维代码1（SAP MARA.FIBER_CODE1）"),
            // entity.material.fibercode1
            new TranslationSeedItem("entity.material.fibercode1", "zh-CN", "纤维代码1", "纤维代码1（SAP MARA.FIBER_CODE1）"),
            // entity.material.fibercode1
            new TranslationSeedItem("entity.material.fibercode1", "zh-HK", "纤维代码1_hk", "纤维代码1（SAP MARA.FIBER_CODE1）"),

            // entity.material.fiberpart1
            new TranslationSeedItem("entity.material.fiberpart1", "en-US", "纤维占比1_us", "纤维占比1（SAP MARA.FIBER_PART1）"),
            // entity.material.fiberpart1
            new TranslationSeedItem("entity.material.fiberpart1", "ja-JP", "纤维占比1_jp", "纤维占比1（SAP MARA.FIBER_PART1）"),
            // entity.material.fiberpart1
            new TranslationSeedItem("entity.material.fiberpart1", "zh-CN", "纤维占比1", "纤维占比1（SAP MARA.FIBER_PART1）"),
            // entity.material.fiberpart1
            new TranslationSeedItem("entity.material.fiberpart1", "zh-HK", "纤维占比1_hk", "纤维占比1（SAP MARA.FIBER_PART1）"),

            // entity.material.fibercode2
            new TranslationSeedItem("entity.material.fibercode2", "en-US", "纤维代码2_us", "纤维代码2（SAP MARA.FIBER_CODE2）"),
            // entity.material.fibercode2
            new TranslationSeedItem("entity.material.fibercode2", "ja-JP", "纤维代码2_jp", "纤维代码2（SAP MARA.FIBER_CODE2）"),
            // entity.material.fibercode2
            new TranslationSeedItem("entity.material.fibercode2", "zh-CN", "纤维代码2", "纤维代码2（SAP MARA.FIBER_CODE2）"),
            // entity.material.fibercode2
            new TranslationSeedItem("entity.material.fibercode2", "zh-HK", "纤维代码2_hk", "纤维代码2（SAP MARA.FIBER_CODE2）"),

            // entity.material.fiberpart2
            new TranslationSeedItem("entity.material.fiberpart2", "en-US", "纤维占比2_us", "纤维占比2（SAP MARA.FIBER_PART2）"),
            // entity.material.fiberpart2
            new TranslationSeedItem("entity.material.fiberpart2", "ja-JP", "纤维占比2_jp", "纤维占比2（SAP MARA.FIBER_PART2）"),
            // entity.material.fiberpart2
            new TranslationSeedItem("entity.material.fiberpart2", "zh-CN", "纤维占比2", "纤维占比2（SAP MARA.FIBER_PART2）"),
            // entity.material.fiberpart2
            new TranslationSeedItem("entity.material.fiberpart2", "zh-HK", "纤维占比2_hk", "纤维占比2（SAP MARA.FIBER_PART2）"),

            // entity.material.fibercode3
            new TranslationSeedItem("entity.material.fibercode3", "en-US", "纤维代码3_us", "纤维代码3（SAP MARA.FIBER_CODE3）"),
            // entity.material.fibercode3
            new TranslationSeedItem("entity.material.fibercode3", "ja-JP", "纤维代码3_jp", "纤维代码3（SAP MARA.FIBER_CODE3）"),
            // entity.material.fibercode3
            new TranslationSeedItem("entity.material.fibercode3", "zh-CN", "纤维代码3", "纤维代码3（SAP MARA.FIBER_CODE3）"),
            // entity.material.fibercode3
            new TranslationSeedItem("entity.material.fibercode3", "zh-HK", "纤维代码3_hk", "纤维代码3（SAP MARA.FIBER_CODE3）"),

            // entity.material.fiberpart3
            new TranslationSeedItem("entity.material.fiberpart3", "en-US", "纤维占比3_us", "纤维占比3（SAP MARA.FIBER_PART3）"),
            // entity.material.fiberpart3
            new TranslationSeedItem("entity.material.fiberpart3", "ja-JP", "纤维占比3_jp", "纤维占比3（SAP MARA.FIBER_PART3）"),
            // entity.material.fiberpart3
            new TranslationSeedItem("entity.material.fiberpart3", "zh-CN", "纤维占比3", "纤维占比3（SAP MARA.FIBER_PART3）"),
            // entity.material.fiberpart3
            new TranslationSeedItem("entity.material.fiberpart3", "zh-HK", "纤维占比3_hk", "纤维占比3（SAP MARA.FIBER_PART3）"),

            // entity.material.fibercode4
            new TranslationSeedItem("entity.material.fibercode4", "en-US", "纤维代码4_us", "纤维代码4（SAP MARA.FIBER_CODE4）"),
            // entity.material.fibercode4
            new TranslationSeedItem("entity.material.fibercode4", "ja-JP", "纤维代码4_jp", "纤维代码4（SAP MARA.FIBER_CODE4）"),
            // entity.material.fibercode4
            new TranslationSeedItem("entity.material.fibercode4", "zh-CN", "纤维代码4", "纤维代码4（SAP MARA.FIBER_CODE4）"),
            // entity.material.fibercode4
            new TranslationSeedItem("entity.material.fibercode4", "zh-HK", "纤维代码4_hk", "纤维代码4（SAP MARA.FIBER_CODE4）"),

            // entity.material.fiberpart4
            new TranslationSeedItem("entity.material.fiberpart4", "en-US", "纤维占比4_us", "纤维占比4（SAP MARA.FIBER_PART4）"),
            // entity.material.fiberpart4
            new TranslationSeedItem("entity.material.fiberpart4", "ja-JP", "纤维占比4_jp", "纤维占比4（SAP MARA.FIBER_PART4）"),
            // entity.material.fiberpart4
            new TranslationSeedItem("entity.material.fiberpart4", "zh-CN", "纤维占比4", "纤维占比4（SAP MARA.FIBER_PART4）"),
            // entity.material.fiberpart4
            new TranslationSeedItem("entity.material.fiberpart4", "zh-HK", "纤维占比4_hk", "纤维占比4（SAP MARA.FIBER_PART4）"),

            // entity.material.fibercode5
            new TranslationSeedItem("entity.material.fibercode5", "en-US", "纤维代码5_us", "纤维代码5（SAP MARA.FIBER_CODE5）"),
            // entity.material.fibercode5
            new TranslationSeedItem("entity.material.fibercode5", "ja-JP", "纤维代码5_jp", "纤维代码5（SAP MARA.FIBER_CODE5）"),
            // entity.material.fibercode5
            new TranslationSeedItem("entity.material.fibercode5", "zh-CN", "纤维代码5", "纤维代码5（SAP MARA.FIBER_CODE5）"),
            // entity.material.fibercode5
            new TranslationSeedItem("entity.material.fibercode5", "zh-HK", "纤维代码5_hk", "纤维代码5（SAP MARA.FIBER_CODE5）"),

            // entity.material.fiberpart5
            new TranslationSeedItem("entity.material.fiberpart5", "en-US", "纤维占比5_us", "纤维占比5（SAP MARA.FIBER_PART5）"),
            // entity.material.fiberpart5
            new TranslationSeedItem("entity.material.fiberpart5", "ja-JP", "纤维占比5_jp", "纤维占比5（SAP MARA.FIBER_PART5）"),
            // entity.material.fiberpart5
            new TranslationSeedItem("entity.material.fiberpart5", "zh-CN", "纤维占比5", "纤维占比5（SAP MARA.FIBER_PART5）"),
            // entity.material.fiberpart5
            new TranslationSeedItem("entity.material.fiberpart5", "zh-HK", "纤维占比5_hk", "纤维占比5（SAP MARA.FIBER_PART5）"),

            // entity.material.fashiongrade
            new TranslationSeedItem("entity.material.fashiongrade", "en-US", "时装等级_us", "时装等级（SAP MARA.FASHGRD）"),
            // entity.material.fashiongrade
            new TranslationSeedItem("entity.material.fashiongrade", "ja-JP", "时装等级_jp", "时装等级（SAP MARA.FASHGRD）"),
            // entity.material.fashiongrade
            new TranslationSeedItem("entity.material.fashiongrade", "zh-CN", "时装等级", "时装等级（SAP MARA.FASHGRD）"),
            // entity.material.fashiongrade
            new TranslationSeedItem("entity.material.fashiongrade", "zh-HK", "时装等级_hk", "时装等级（SAP MARA.FASHGRD）"),

            // entity.material.status
            new TranslationSeedItem("entity.material.status", "en-US", "物料状态_us", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "ja-JP", "物料状态_jp", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-CN", "物料状态", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-HK", "物料状态_hk", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）"),

            // entity.material.descriptions
            new TranslationSeedItem("entity.material.descriptions", "en-US", "多语言描述列表_us", "多语言描述列表（主子表关系；对齐 SAP MAKT）"),
            // entity.material.descriptions
            new TranslationSeedItem("entity.material.descriptions", "ja-JP", "多语言描述列表_jp", "多语言描述列表（主子表关系；对齐 SAP MAKT）"),
            // entity.material.descriptions
            new TranslationSeedItem("entity.material.descriptions", "zh-CN", "多语言描述列表", "多语言描述列表（主子表关系；对齐 SAP MAKT）"),
            // entity.material.descriptions
            new TranslationSeedItem("entity.material.descriptions", "zh-HK", "多语言描述列表_hk", "多语言描述列表（主子表关系；对齐 SAP MAKT）"),
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
