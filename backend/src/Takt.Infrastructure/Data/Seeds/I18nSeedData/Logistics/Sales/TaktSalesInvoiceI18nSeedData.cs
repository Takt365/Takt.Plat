// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoice 实体国际化翻译种子（键前缀 entity.salesinvoice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoice 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceTranslations())
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

        TaktLogger.Information("TaktSalesInvoice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoice._self / entity.salesinvoice.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "en-US", "Sales Invoice Information_us", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "ja-JP", "Takt销售发票信息_jp", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-CN", "Takt销售发票信息", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-HK", "Takt销售发票信息_hk", "实体名称"),

            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.salesinvoice.plantcode
            new TranslationSeedItem("entity.salesinvoice.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.salesinvoice.yearmonth
            new TranslationSeedItem("entity.salesinvoice.yearmonth", "en-US", "年度期间_us", "年度期间（yyyyMM）"),
            // entity.salesinvoice.yearmonth
            new TranslationSeedItem("entity.salesinvoice.yearmonth", "ja-JP", "年度期间_jp", "年度期间（yyyyMM）"),
            // entity.salesinvoice.yearmonth
            new TranslationSeedItem("entity.salesinvoice.yearmonth", "zh-CN", "年度期间", "年度期间（yyyyMM）"),
            // entity.salesinvoice.yearmonth
            new TranslationSeedItem("entity.salesinvoice.yearmonth", "zh-HK", "年度期间_hk", "年度期间（yyyyMM）"),

            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）"),

            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "en-US", "客户名称_us", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "ja-JP", "客户名称_jp", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.salesinvoice.customername
            new TranslationSeedItem("entity.salesinvoice.customername", "zh-HK", "客户名称_hk", "客户名称"),

            // entity.salesinvoice.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.accountingdocumentcode", "en-US", "会计凭证编号_us", "会计凭证编号（租户+公司+工厂内唯一）"),
            // entity.salesinvoice.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.accountingdocumentcode", "ja-JP", "会计凭证编号_jp", "会计凭证编号（租户+公司+工厂内唯一）"),
            // entity.salesinvoice.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.accountingdocumentcode", "zh-CN", "会计凭证编号", "会计凭证编号（租户+公司+工厂内唯一）"),
            // entity.salesinvoice.accountingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.accountingdocumentcode", "zh-HK", "会计凭证编号_hk", "会计凭证编号（租户+公司+工厂内唯一）"),

            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "en-US", "销售发票明细列表_us", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "ja-JP", "销售发票明细列表_jp", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-CN", "销售发票明细列表", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-HK", "销售发票明细列表_hk", "销售发票明细列表（主子表关系，一张发票可有多个明细行）"),
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
