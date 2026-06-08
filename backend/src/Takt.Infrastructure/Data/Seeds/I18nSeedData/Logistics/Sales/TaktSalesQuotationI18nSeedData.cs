// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesQuotation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesQuotation 实体国际化翻译种子（键前缀 entity.salesQuotation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesQuotationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesQuotation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesQuotation 实体翻译...", tenantCode);

        foreach (var item in GetSalesQuotationTranslations())
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

        TaktLogger.Information("TaktSalesQuotation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesQuotation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesQuotation._self / entity.salesQuotation.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesQuotation._self
            new TranslationSeedItem("entity.salesQuotation._self", "en-US", "Sales Quotation Information", "实体名称"),
            // entity.salesQuotation._self
            new TranslationSeedItem("entity.salesQuotation._self", "ja-JP", "Takt销售报价信息", "实体名称"),
            // entity.salesQuotation._self
            new TranslationSeedItem("entity.salesQuotation._self", "zh-CN", "Takt销售报价信息", "实体名称"),
            // entity.salesQuotation._self
            new TranslationSeedItem("entity.salesQuotation._self", "zh-HK", "Takt销售报价信息", "实体名称"),

            // entity.salesQuotation.plantcode
            new TranslationSeedItem("entity.salesQuotation.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.salesQuotation.plantcode
            new TranslationSeedItem("entity.salesQuotation.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.salesQuotation.plantcode
            new TranslationSeedItem("entity.salesQuotation.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.salesQuotation.plantcode
            new TranslationSeedItem("entity.salesQuotation.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.salesQuotation.code
            new TranslationSeedItem("entity.salesQuotation.code", "en-US", "销售报价编码", "销售报价编码（唯一索引）"),
            // entity.salesQuotation.code
            new TranslationSeedItem("entity.salesQuotation.code", "ja-JP", "销售报价编码", "销售报价编码（唯一索引）"),
            // entity.salesQuotation.code
            new TranslationSeedItem("entity.salesQuotation.code", "zh-CN", "销售报价编码", "销售报价编码（唯一索引）"),
            // entity.salesQuotation.code
            new TranslationSeedItem("entity.salesQuotation.code", "zh-HK", "销售报价编码", "销售报价编码（唯一索引）"),

            // entity.salesQuotation.customercode
            new TranslationSeedItem("entity.salesQuotation.customercode", "en-US", "客户编码", "客户编码"),
            // entity.salesQuotation.customercode
            new TranslationSeedItem("entity.salesQuotation.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.salesQuotation.customercode
            new TranslationSeedItem("entity.salesQuotation.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.salesQuotation.customercode
            new TranslationSeedItem("entity.salesQuotation.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.salesQuotation.customername
            new TranslationSeedItem("entity.salesQuotation.customername", "en-US", "客户名称", "客户名称"),
            // entity.salesQuotation.customername
            new TranslationSeedItem("entity.salesQuotation.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.salesQuotation.customername
            new TranslationSeedItem("entity.salesQuotation.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesQuotation.customername
            new TranslationSeedItem("entity.salesQuotation.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.salesQuotation.quotationdate
            new TranslationSeedItem("entity.salesQuotation.quotationdate", "en-US", "报价日期", "报价日期"),
            // entity.salesQuotation.quotationdate
            new TranslationSeedItem("entity.salesQuotation.quotationdate", "ja-JP", "报价日期", "报价日期"),
            // entity.salesQuotation.quotationdate
            new TranslationSeedItem("entity.salesQuotation.quotationdate", "zh-CN", "报价日期", "报价日期"),
            // entity.salesQuotation.quotationdate
            new TranslationSeedItem("entity.salesQuotation.quotationdate", "zh-HK", "报价日期", "报价日期"),

            // entity.salesQuotation.validuntildate
            new TranslationSeedItem("entity.salesQuotation.validuntildate", "en-US", "报价有效期至", "报价有效期至"),
            // entity.salesQuotation.validuntildate
            new TranslationSeedItem("entity.salesQuotation.validuntildate", "ja-JP", "报价有效期至", "报价有效期至"),
            // entity.salesQuotation.validuntildate
            new TranslationSeedItem("entity.salesQuotation.validuntildate", "zh-CN", "报价有效期至", "报价有效期至"),
            // entity.salesQuotation.validuntildate
            new TranslationSeedItem("entity.salesQuotation.validuntildate", "zh-HK", "报价有效期至", "报价有效期至"),

            // entity.salesQuotation.salesby
            new TranslationSeedItem("entity.salesQuotation.salesby", "en-US", "销售员", "销售员（人员代码）"),
            // entity.salesQuotation.salesby
            new TranslationSeedItem("entity.salesQuotation.salesby", "ja-JP", "销售员", "销售员（人员代码）"),
            // entity.salesQuotation.salesby
            new TranslationSeedItem("entity.salesQuotation.salesby", "zh-CN", "销售员", "销售员（人员代码）"),
            // entity.salesQuotation.salesby
            new TranslationSeedItem("entity.salesQuotation.salesby", "zh-HK", "销售员", "销售员（人员代码）"),

            // entity.salesQuotation.totalquantity
            new TranslationSeedItem("entity.salesQuotation.totalquantity", "en-US", "报价总数量", "报价总数量（基本单位数量）"),
            // entity.salesQuotation.totalquantity
            new TranslationSeedItem("entity.salesQuotation.totalquantity", "ja-JP", "报价总数量", "报价总数量（基本单位数量）"),
            // entity.salesQuotation.totalquantity
            new TranslationSeedItem("entity.salesQuotation.totalquantity", "zh-CN", "报价总数量", "报价总数量（基本单位数量）"),
            // entity.salesQuotation.totalquantity
            new TranslationSeedItem("entity.salesQuotation.totalquantity", "zh-HK", "报价总数量", "报价总数量（基本单位数量）"),

            // entity.salesQuotation.totalamount
            new TranslationSeedItem("entity.salesQuotation.totalamount", "en-US", "报价总金额", "报价总金额"),
            // entity.salesQuotation.totalamount
            new TranslationSeedItem("entity.salesQuotation.totalamount", "ja-JP", "报价总金额", "报价总金额"),
            // entity.salesQuotation.totalamount
            new TranslationSeedItem("entity.salesQuotation.totalamount", "zh-CN", "报价总金额", "报价总金额"),
            // entity.salesQuotation.totalamount
            new TranslationSeedItem("entity.salesQuotation.totalamount", "zh-HK", "报价总金额", "报价总金额"),

            // entity.salesQuotation.discountamount
            new TranslationSeedItem("entity.salesQuotation.discountamount", "en-US", "折扣金额", "折扣金额"),
            // entity.salesQuotation.discountamount
            new TranslationSeedItem("entity.salesQuotation.discountamount", "ja-JP", "折扣金额", "折扣金额"),
            // entity.salesQuotation.discountamount
            new TranslationSeedItem("entity.salesQuotation.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesQuotation.discountamount
            new TranslationSeedItem("entity.salesQuotation.discountamount", "zh-HK", "折扣金额", "折扣金额"),

            // entity.salesQuotation.taxamount
            new TranslationSeedItem("entity.salesQuotation.taxamount", "en-US", "税费", "税费"),
            // entity.salesQuotation.taxamount
            new TranslationSeedItem("entity.salesQuotation.taxamount", "ja-JP", "税费", "税费"),
            // entity.salesQuotation.taxamount
            new TranslationSeedItem("entity.salesQuotation.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesQuotation.taxamount
            new TranslationSeedItem("entity.salesQuotation.taxamount", "zh-HK", "税费", "税费"),

            // entity.salesQuotation.actualamount
            new TranslationSeedItem("entity.salesQuotation.actualamount", "en-US", "报价实付金额", "报价实付金额"),
            // entity.salesQuotation.actualamount
            new TranslationSeedItem("entity.salesQuotation.actualamount", "ja-JP", "报价实付金额", "报价实付金额"),
            // entity.salesQuotation.actualamount
            new TranslationSeedItem("entity.salesQuotation.actualamount", "zh-CN", "报价实付金额", "报价实付金额"),
            // entity.salesQuotation.actualamount
            new TranslationSeedItem("entity.salesQuotation.actualamount", "zh-HK", "报价实付金额", "报价实付金额"),

            // entity.salesQuotation.quotationstatus
            new TranslationSeedItem("entity.salesQuotation.quotationstatus", "en-US", "报价状态", "报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）"),
            // entity.salesQuotation.quotationstatus
            new TranslationSeedItem("entity.salesQuotation.quotationstatus", "ja-JP", "报价状态", "报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）"),
            // entity.salesQuotation.quotationstatus
            new TranslationSeedItem("entity.salesQuotation.quotationstatus", "zh-CN", "报价状态", "报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）"),
            // entity.salesQuotation.quotationstatus
            new TranslationSeedItem("entity.salesQuotation.quotationstatus", "zh-HK", "报价状态", "报价状态（0=草稿，1=已发送，2=已接受，3=已拒绝，4=已过期，5=已作废）"),

            // entity.salesQuotation.salesordercode
            new TranslationSeedItem("entity.salesQuotation.salesordercode", "en-US", "销售订单编码", "关联销售订单编码（报价转订单后回填）"),
            // entity.salesQuotation.salesordercode
            new TranslationSeedItem("entity.salesQuotation.salesordercode", "ja-JP", "销售订单编码", "关联销售订单编码（报价转订单后回填）"),
            // entity.salesQuotation.salesordercode
            new TranslationSeedItem("entity.salesQuotation.salesordercode", "zh-CN", "销售订单编码", "关联销售订单编码（报价转订单后回填）"),
            // entity.salesQuotation.salesordercode
            new TranslationSeedItem("entity.salesQuotation.salesordercode", "zh-HK", "销售订单编码", "关联销售订单编码（报价转订单后回填）"),

            // entity.salesQuotation.items
            new TranslationSeedItem("entity.salesQuotation.items", "en-US", "销售报价明细列表", "销售报价明细列表（主子表关系）"),
            // entity.salesQuotation.items
            new TranslationSeedItem("entity.salesQuotation.items", "ja-JP", "销售报价明细列表", "销售报价明细列表（主子表关系）"),
            // entity.salesQuotation.items
            new TranslationSeedItem("entity.salesQuotation.items", "zh-CN", "销售报价明细列表", "销售报价明细列表（主子表关系）"),
            // entity.salesQuotation.items
            new TranslationSeedItem("entity.salesQuotation.items", "zh-HK", "销售报价明细列表", "销售报价明细列表（主子表关系）"),
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
