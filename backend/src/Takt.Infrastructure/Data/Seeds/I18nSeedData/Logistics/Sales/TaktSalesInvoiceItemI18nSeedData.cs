// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoiceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoiceItem 实体国际化翻译种子（键前缀 entity.salesinvoiceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoiceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoiceitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceItemTranslations())
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

        TaktLogger.Information("TaktSalesInvoiceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoiceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoiceitem._self / entity.salesinvoiceitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "en-US", "Sales Invoice Item Information_us", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "ja-JP", "Takt销售发票明细信息_jp", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-CN", "Takt销售发票明细信息", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-HK", "Takt销售发票明细信息_hk", "实体名称"),

            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "en-US", "销售发票ID_us", "销售发票（选项 TaktSalesInvoices/options，DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "ja-JP", "销售发票ID_jp", "销售发票（选项 TaktSalesInvoices/options，DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-CN", "销售发票ID", "销售发票（选项 TaktSalesInvoices/options，DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-HK", "销售发票ID_hk", "销售发票（选项 TaktSalesInvoices/options，DictValue=Id）"),

            // entity.salesinvoiceitem.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.accountingdocumentcode", "en-US", "会计凭证编号_us", "会计凭证编号（冗余，与主表 AccountingDocumentCode 一致）"),
            // entity.salesinvoiceitem.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.accountingdocumentcode", "ja-JP", "会计凭证编号_jp", "会计凭证编号（冗余，与主表 AccountingDocumentCode 一致）"),
            // entity.salesinvoiceitem.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.accountingdocumentcode", "zh-CN", "会计凭证编号", "会计凭证编号（冗余，与主表 AccountingDocumentCode 一致）"),
            // entity.salesinvoiceitem.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.accountingdocumentcode", "zh-HK", "会计凭证编号_hk", "会计凭证编号（冗余，与主表 AccountingDocumentCode 一致）"),

            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "en-US", "项目_us", "行号（项目/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "ja-JP", "项目_jp", "行号（项目/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-CN", "项目", "行号（项目/序号，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-HK", "项目_hk", "行号（项目/序号，固定步长=10）"),

            // entity.salesinvoiceitem.postingdate
            new TranslationSeedItem("entity.salesinvoiceitem.postingdate", "en-US", "过帐日期_us", "过帐日期"),
            // entity.salesinvoiceitem.postingdate
            new TranslationSeedItem("entity.salesinvoiceitem.postingdate", "ja-JP", "过帐日期_jp", "过帐日期"),
            // entity.salesinvoiceitem.postingdate
            new TranslationSeedItem("entity.salesinvoiceitem.postingdate", "zh-CN", "过帐日期", "过帐日期"),
            // entity.salesinvoiceitem.postingdate
            new TranslationSeedItem("entity.salesinvoiceitem.postingdate", "zh-HK", "过帐日期_hk", "过帐日期"),

            // entity.salesinvoiceitem.currency
            new TranslationSeedItem("entity.salesinvoiceitem.currency", "en-US", "货币_us", "货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.salesinvoiceitem.currency
            new TranslationSeedItem("entity.salesinvoiceitem.currency", "ja-JP", "货币_jp", "货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.salesinvoiceitem.currency
            new TranslationSeedItem("entity.salesinvoiceitem.currency", "zh-CN", "货币", "货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),
            // entity.salesinvoiceitem.currency
            new TranslationSeedItem("entity.salesinvoiceitem.currency", "zh-HK", "货币_hk", "货币（字典 accounting_currency_code，DictValue=CNY/USD 等）"),

            // entity.salesinvoiceitem.modelname
            new TranslationSeedItem("entity.salesinvoiceitem.modelname", "en-US", "机种名称_us", "机种名称"),
            // entity.salesinvoiceitem.modelname
            new TranslationSeedItem("entity.salesinvoiceitem.modelname", "ja-JP", "机种名称_jp", "机种名称"),
            // entity.salesinvoiceitem.modelname
            new TranslationSeedItem("entity.salesinvoiceitem.modelname", "zh-CN", "机种名称", "机种名称"),
            // entity.salesinvoiceitem.modelname
            new TranslationSeedItem("entity.salesinvoiceitem.modelname", "zh-HK", "机种名称_hk", "机种名称"),

            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.salesinvoiceitem.materialtype
            new TranslationSeedItem("entity.salesinvoiceitem.materialtype", "en-US", "物料类型_us", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.salesinvoiceitem.materialtype
            new TranslationSeedItem("entity.salesinvoiceitem.materialtype", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.salesinvoiceitem.materialtype
            new TranslationSeedItem("entity.salesinvoiceitem.materialtype", "zh-CN", "物料类型", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.salesinvoiceitem.materialtype
            new TranslationSeedItem("entity.salesinvoiceitem.materialtype", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),

            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "en-US", "物料名称_us", "物料名称（回填：随物料）"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "ja-JP", "物料名称_jp", "物料名称（回填：随物料）"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "zh-CN", "物料名称", "物料名称（回填：随物料）"),
            // entity.salesinvoiceitem.materialname
            new TranslationSeedItem("entity.salesinvoiceitem.materialname", "zh-HK", "物料名称_hk", "物料名称（回填：随物料）"),

            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options，DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options，DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options，DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options，DictValue=ProfitCenterCode）"),

            // entity.salesinvoiceitem.accounttitle
            new TranslationSeedItem("entity.salesinvoiceitem.accounttitle", "en-US", "会计科目_us", "会计科目（选项 TaktAccountTitles/options，DictValue=Id）"),
            // entity.salesinvoiceitem.accounttitle
            new TranslationSeedItem("entity.salesinvoiceitem.accounttitle", "ja-JP", "会计科目_jp", "会计科目（选项 TaktAccountTitles/options，DictValue=Id）"),
            // entity.salesinvoiceitem.accounttitle
            new TranslationSeedItem("entity.salesinvoiceitem.accounttitle", "zh-CN", "会计科目", "会计科目（选项 TaktAccountTitles/options，DictValue=Id）"),
            // entity.salesinvoiceitem.accounttitle
            new TranslationSeedItem("entity.salesinvoiceitem.accounttitle", "zh-HK", "会计科目_hk", "会计科目（选项 TaktAccountTitles/options，DictValue=Id）"),

            // entity.salesinvoiceitem.quantity
            new TranslationSeedItem("entity.salesinvoiceitem.quantity", "en-US", "数量_us", "数量"),
            // entity.salesinvoiceitem.quantity
            new TranslationSeedItem("entity.salesinvoiceitem.quantity", "ja-JP", "数量_jp", "数量"),
            // entity.salesinvoiceitem.quantity
            new TranslationSeedItem("entity.salesinvoiceitem.quantity", "zh-CN", "数量", "数量"),
            // entity.salesinvoiceitem.quantity
            new TranslationSeedItem("entity.salesinvoiceitem.quantity", "zh-HK", "数量_hk", "数量"),

            // entity.salesinvoiceitem.unit
            new TranslationSeedItem("entity.salesinvoiceitem.unit", "en-US", "单位_us", "单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesinvoiceitem.unit
            new TranslationSeedItem("entity.salesinvoiceitem.unit", "ja-JP", "单位_jp", "单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesinvoiceitem.unit
            new TranslationSeedItem("entity.salesinvoiceitem.unit", "zh-CN", "单位", "单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesinvoiceitem.unit
            new TranslationSeedItem("entity.salesinvoiceitem.unit", "zh-HK", "单位_hk", "单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),

            // entity.salesinvoiceitem.localcurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.localcurrencyamount", "en-US", "本位币金额_us", "本位币金额"),
            // entity.salesinvoiceitem.localcurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.localcurrencyamount", "ja-JP", "本位币金额_jp", "本位币金额"),
            // entity.salesinvoiceitem.localcurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.localcurrencyamount", "zh-CN", "本位币金额", "本位币金额"),
            // entity.salesinvoiceitem.localcurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.localcurrencyamount", "zh-HK", "本位币金额_hk", "本位币金额"),

            // entity.salesinvoiceitem.transactioncurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.transactioncurrencyamount", "en-US", "业务货币金额_us", "业务货币计价的金额"),
            // entity.salesinvoiceitem.transactioncurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.transactioncurrencyamount", "ja-JP", "业务货币金额_jp", "业务货币计价的金额"),
            // entity.salesinvoiceitem.transactioncurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.transactioncurrencyamount", "zh-CN", "业务货币金额", "业务货币计价的金额"),
            // entity.salesinvoiceitem.transactioncurrencyamount
            new TranslationSeedItem("entity.salesinvoiceitem.transactioncurrencyamount", "zh-HK", "业务货币金额_hk", "业务货币计价的金额"),

            // entity.salesinvoiceitem.documenttype
            new TranslationSeedItem("entity.salesinvoiceitem.documenttype", "en-US", "凭证类型_us", "凭证类型（字典 logistics_accounting_document_type，DictValue=AA/AB/…）"),
            // entity.salesinvoiceitem.documenttype
            new TranslationSeedItem("entity.salesinvoiceitem.documenttype", "ja-JP", "凭证类型_jp", "凭证类型（字典 logistics_accounting_document_type，DictValue=AA/AB/…）"),
            // entity.salesinvoiceitem.documenttype
            new TranslationSeedItem("entity.salesinvoiceitem.documenttype", "zh-CN", "凭证类型", "凭证类型（字典 logistics_accounting_document_type，DictValue=AA/AB/…）"),
            // entity.salesinvoiceitem.documenttype
            new TranslationSeedItem("entity.salesinvoiceitem.documenttype", "zh-HK", "凭证类型_hk", "凭证类型（字典 logistics_accounting_document_type，DictValue=AA/AB/…）"),

            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "en-US", "参考凭证_us", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "ja-JP", "参考凭证_jp", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "zh-CN", "参考凭证", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "zh-HK", "参考凭证_hk", "参考凭证"),

            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "en-US", "参考凭证项目_us", "参考凭证项目（行号）"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "ja-JP", "参考凭证项目_jp", "参考凭证项目（行号）"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "zh-CN", "参考凭证项目", "参考凭证项目（行号）"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "zh-HK", "参考凭证项目_hk", "参考凭证项目（行号）"),

            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "en-US", "销售发票主表_us", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "ja-JP", "销售发票主表_jp", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-CN", "销售发票主表", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-HK", "销售发票主表_hk", "销售发票主表"),
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
