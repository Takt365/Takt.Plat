// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktStandardOperationRateI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardOperationRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktStandardOperationRate 实体国际化翻译种子（键前缀 entity.standardOperationRate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardOperationRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardOperationRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardOperationRate 实体翻译...", tenantCode);

        foreach (var item in GetStandardOperationRateTranslations())
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

        TaktLogger.Information("TaktStandardOperationRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardOperationRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardOperationRate._self / entity.standardOperationRate.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardOperationRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardOperationRate._self
            new TranslationSeedItem("entity.standardOperationRate._self", "en-US", "Standard Operation Rate Information", "实体名称"),
            // entity.standardOperationRate._self
            new TranslationSeedItem("entity.standardOperationRate._self", "ja-JP", "标准生产稼动率信息", "实体名称"),
            // entity.standardOperationRate._self
            new TranslationSeedItem("entity.standardOperationRate._self", "zh-CN", "标准生产稼动率信息", "实体名称"),
            // entity.standardOperationRate._self
            new TranslationSeedItem("entity.standardOperationRate._self", "zh-HK", "标准生产稼动率信息", "实体名称"),

            // entity.standardOperationRate.plantcode
            new TranslationSeedItem("entity.standardOperationRate.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.standardOperationRate.plantcode
            new TranslationSeedItem("entity.standardOperationRate.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.standardOperationRate.plantcode
            new TranslationSeedItem("entity.standardOperationRate.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.standardOperationRate.plantcode
            new TranslationSeedItem("entity.standardOperationRate.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.standardOperationRate.financialyear
            new TranslationSeedItem("entity.standardOperationRate.financialyear", "en-US", "财务年度", "财务年度"),
            // entity.standardOperationRate.financialyear
            new TranslationSeedItem("entity.standardOperationRate.financialyear", "ja-JP", "财务年度", "财务年度"),
            // entity.standardOperationRate.financialyear
            new TranslationSeedItem("entity.standardOperationRate.financialyear", "zh-CN", "财务年度", "财务年度"),
            // entity.standardOperationRate.financialyear
            new TranslationSeedItem("entity.standardOperationRate.financialyear", "zh-HK", "财务年度", "财务年度"),

            // entity.standardOperationRate.operationtype
            new TranslationSeedItem("entity.standardOperationRate.operationtype", "en-US", "稼动率类型", "稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）"),
            // entity.standardOperationRate.operationtype
            new TranslationSeedItem("entity.standardOperationRate.operationtype", "ja-JP", "稼动率类型", "稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）"),
            // entity.standardOperationRate.operationtype
            new TranslationSeedItem("entity.standardOperationRate.operationtype", "zh-CN", "稼动率类型", "稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）"),
            // entity.standardOperationRate.operationtype
            new TranslationSeedItem("entity.standardOperationRate.operationtype", "zh-HK", "稼动率类型", "稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）"),

            // entity.standardOperationRate.operationrate
            new TranslationSeedItem("entity.standardOperationRate.operationrate", "en-US", "稼动率(%)", "稼动率（%）"),
            // entity.standardOperationRate.operationrate
            new TranslationSeedItem("entity.standardOperationRate.operationrate", "ja-JP", "稼动率(%)", "稼动率（%）"),
            // entity.standardOperationRate.operationrate
            new TranslationSeedItem("entity.standardOperationRate.operationrate", "zh-CN", "稼动率(%)", "稼动率（%）"),
            // entity.standardOperationRate.operationrate
            new TranslationSeedItem("entity.standardOperationRate.operationrate", "zh-HK", "稼动率(%)", "稼动率（%）"),

            // entity.standardOperationRate.effectivedate
            new TranslationSeedItem("entity.standardOperationRate.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.standardOperationRate.effectivedate
            new TranslationSeedItem("entity.standardOperationRate.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.standardOperationRate.effectivedate
            new TranslationSeedItem("entity.standardOperationRate.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.standardOperationRate.effectivedate
            new TranslationSeedItem("entity.standardOperationRate.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.standardOperationRate.expirydate
            new TranslationSeedItem("entity.standardOperationRate.expirydate", "en-US", "失效日期", "失效日期"),
            // entity.standardOperationRate.expirydate
            new TranslationSeedItem("entity.standardOperationRate.expirydate", "ja-JP", "失效日期", "失效日期"),
            // entity.standardOperationRate.expirydate
            new TranslationSeedItem("entity.standardOperationRate.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.standardOperationRate.expirydate
            new TranslationSeedItem("entity.standardOperationRate.expirydate", "zh-HK", "失效日期", "失效日期"),

            // entity.standardOperationRate.status
            new TranslationSeedItem("entity.standardOperationRate.status", "en-US", "状态", "状态"),
            // entity.standardOperationRate.status
            new TranslationSeedItem("entity.standardOperationRate.status", "ja-JP", "状态", "状态"),
            // entity.standardOperationRate.status
            new TranslationSeedItem("entity.standardOperationRate.status", "zh-CN", "状态", "状态"),
            // entity.standardOperationRate.status
            new TranslationSeedItem("entity.standardOperationRate.status", "zh-HK", "状态", "状态"),
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
