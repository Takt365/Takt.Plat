// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesPriceI18nSeedData.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPrice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesPrice 实体国际化翻译种子（键前缀 entity.salesprice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPriceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPrice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesprice 实体翻译...", tenantCode);

        foreach (var item in GetSalesPriceTranslations())
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

        TaktLogger.Information("TaktSalesPrice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPrice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesprice._self / entity.salesprice.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPriceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "en-US", "Sales Price Information_us", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "ja-JP", "Takt销售价格信息_jp", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "zh-CN", "Takt销售价格信息", "实体名称"),
            // entity.salesprice._self
            new TranslationSeedItem("entity.salesprice._self", "zh-HK", "Takt销售价格信息_hk", "实体名称"),

            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "en-US", "定价记录号_us", "定价记录号（唯一索引；长度 20）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "ja-JP", "定价记录号_jp", "定价记录号（唯一索引；长度 20）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "zh-CN", "定价记录号", "定价记录号（唯一索引；长度 20）"),
            // entity.salesprice.code
            new TranslationSeedItem("entity.salesprice.code", "zh-HK", "定价记录号_hk", "定价记录号（唯一索引；长度 20）"),

            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "en-US", "条件类型_us", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "ja-JP", "条件类型_jp", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "zh-CN", "条件类型", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),
            // entity.salesprice.pricetype
            new TranslationSeedItem("entity.salesprice.pricetype", "zh-HK", "条件类型_hk", "条件类型（字典 logistics_procurement_price_type；PB00=采购总价 Gross Price，PR00=基本价格 Base Price，MWST=销项税/增值税，MWRK=不可抵扣进项税，NLXV=购置税）"),

            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "en-US", "客户_us", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "ja-JP", "客户_jp", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "zh-CN", "客户", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesprice.customercode
            new TranslationSeedItem("entity.salesprice.customercode", "zh-HK", "客户_hk", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.salesprice.materialcode
            new TranslationSeedItem("entity.salesprice.materialcode", "en-US", "物料_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salesprice.materialcode
            new TranslationSeedItem("entity.salesprice.materialcode", "ja-JP", "物料_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salesprice.materialcode
            new TranslationSeedItem("entity.salesprice.materialcode", "zh-CN", "物料", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.salesprice.materialcode
            new TranslationSeedItem("entity.salesprice.materialcode", "zh-HK", "物料_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode）"),

            // entity.salesprice.materialdescription
            new TranslationSeedItem("entity.salesprice.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesprice.materialdescription
            new TranslationSeedItem("entity.salesprice.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesprice.materialdescription
            new TranslationSeedItem("entity.salesprice.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesprice.materialdescription
            new TranslationSeedItem("entity.salesprice.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salesprice.salesgroup
            new TranslationSeedItem("entity.salesprice.salesgroup", "en-US", "销售组_us", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesprice.salesgroup
            new TranslationSeedItem("entity.salesprice.salesgroup", "ja-JP", "销售组_jp", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesprice.salesgroup
            new TranslationSeedItem("entity.salesprice.salesgroup", "zh-CN", "销售组", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.salesprice.salesgroup
            new TranslationSeedItem("entity.salesprice.salesgroup", "zh-HK", "销售组_hk", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),

            // entity.salesprice.taxcode
            new TranslationSeedItem("entity.salesprice.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.salesprice.taxcode
            new TranslationSeedItem("entity.salesprice.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.salesprice.taxcode
            new TranslationSeedItem("entity.salesprice.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),
            // entity.salesprice.taxcode
            new TranslationSeedItem("entity.salesprice.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；DictValue=J0～J8/L1/X0～X3；中国）"),

            // entity.salesprice.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salesprice.grbasedinvoiceinspection", "en-US", "基于收货的发票检验_us", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salesprice.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salesprice.grbasedinvoiceinspection", "ja-JP", "基于收货的发票检验_jp", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salesprice.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salesprice.grbasedinvoiceinspection", "zh-CN", "基于收货的发票检验", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),
            // entity.salesprice.grbasedinvoiceinspection
            new TranslationSeedItem("entity.salesprice.grbasedinvoiceinspection", "zh-HK", "基于收货的发票检验_hk", "基于收货的发票检验（字典 sys_yes_no；0=否 1=是）"),

            // entity.salesprice.pricingdatecontrol
            new TranslationSeedItem("entity.salesprice.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salesprice.pricingdatecontrol
            new TranslationSeedItem("entity.salesprice.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salesprice.pricingdatecontrol
            new TranslationSeedItem("entity.salesprice.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.salesprice.pricingdatecontrol
            new TranslationSeedItem("entity.salesprice.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=采购订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.salesprice.validfrom
            new TranslationSeedItem("entity.salesprice.validfrom", "en-US", "有效起始日_us", "有效起始日"),
            // entity.salesprice.validfrom
            new TranslationSeedItem("entity.salesprice.validfrom", "ja-JP", "有效起始日_jp", "有效起始日"),
            // entity.salesprice.validfrom
            new TranslationSeedItem("entity.salesprice.validfrom", "zh-CN", "有效起始日", "有效起始日"),
            // entity.salesprice.validfrom
            new TranslationSeedItem("entity.salesprice.validfrom", "zh-HK", "有效起始日_hk", "有效起始日"),

            // entity.salesprice.validto
            new TranslationSeedItem("entity.salesprice.validto", "en-US", "有效截至日_us", "有效截至日"),
            // entity.salesprice.validto
            new TranslationSeedItem("entity.salesprice.validto", "ja-JP", "有效截至日_jp", "有效截至日"),
            // entity.salesprice.validto
            new TranslationSeedItem("entity.salesprice.validto", "zh-CN", "有效截至日", "有效截至日"),
            // entity.salesprice.validto
            new TranslationSeedItem("entity.salesprice.validto", "zh-HK", "有效截至日_hk", "有效截至日"),

            // entity.salesprice.salesquotationid
            new TranslationSeedItem("entity.salesprice.salesquotationid", "en-US", "来源销售报价ID_us", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salesprice.salesquotationid
            new TranslationSeedItem("entity.salesprice.salesquotationid", "ja-JP", "来源销售报价ID_jp", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salesprice.salesquotationid
            new TranslationSeedItem("entity.salesprice.salesquotationid", "zh-CN", "来源销售报价ID", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),
            // entity.salesprice.salesquotationid
            new TranslationSeedItem("entity.salesprice.salesquotationid", "zh-HK", "来源销售报价ID_hk", "来源销售报价 ID（选项 TaktSalesQuotations/options；DictValue=Id；对应采购侧来源询价）"),

            // entity.salesprice.salesquotationcode
            new TranslationSeedItem("entity.salesprice.salesquotationcode", "en-US", "来源销售报价编码_us", "来源销售报价编码（冗余）"),
            // entity.salesprice.salesquotationcode
            new TranslationSeedItem("entity.salesprice.salesquotationcode", "ja-JP", "来源销售报价编码_jp", "来源销售报价编码（冗余）"),
            // entity.salesprice.salesquotationcode
            new TranslationSeedItem("entity.salesprice.salesquotationcode", "zh-CN", "来源销售报价编码", "来源销售报价编码（冗余）"),
            // entity.salesprice.salesquotationcode
            new TranslationSeedItem("entity.salesprice.salesquotationcode", "zh-HK", "来源销售报价编码_hk", "来源销售报价编码（冗余）"),

            // entity.salesprice.variablekey
            new TranslationSeedItem("entity.salesprice.variablekey", "en-US", "可变关键字_us", "可变关键字"),
            // entity.salesprice.variablekey
            new TranslationSeedItem("entity.salesprice.variablekey", "ja-JP", "可变关键字_jp", "可变关键字"),
            // entity.salesprice.variablekey
            new TranslationSeedItem("entity.salesprice.variablekey", "zh-CN", "可变关键字", "可变关键字"),
            // entity.salesprice.variablekey
            new TranslationSeedItem("entity.salesprice.variablekey", "zh-HK", "可变关键字_hk", "可变关键字"),

            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "en-US", "定价条件行列表_us", "定价条件行列表（主子表关系）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "ja-JP", "定价条件行列表_jp", "定价条件行列表（主子表关系）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "zh-CN", "定价条件行列表", "定价条件行列表（主子表关系）"),
            // entity.salesprice.items
            new TranslationSeedItem("entity.salesprice.items", "zh-HK", "定价条件行列表_hk", "定价条件行列表（主子表关系）"),
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
