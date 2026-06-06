// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityOperation 实体国际化翻译种子（键前缀 entity.qualityOperation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperation 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationTranslations())
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

        TaktLogger.Information("TaktQualityOperation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityOperation._self / entity.qualityOperation.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperation._self
            new TranslationSeedItem("entity.qualityOperation._self", "en-US", "Quality Operation Information", "实体名称"),
            // entity.qualityOperation._self
            new TranslationSeedItem("entity.qualityOperation._self", "ja-JP", "品质业务主表信息", "实体名称"),
            // entity.qualityOperation._self
            new TranslationSeedItem("entity.qualityOperation._self", "zh-CN", "品质业务主表信息", "实体名称"),
            // entity.qualityOperation._self
            new TranslationSeedItem("entity.qualityOperation._self", "zh-HK", "品质业务主表信息", "实体名称"),

            // entity.qualityOperation.plantcode
            new TranslationSeedItem("entity.qualityOperation.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.qualityOperation.plantcode
            new TranslationSeedItem("entity.qualityOperation.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.qualityOperation.plantcode
            new TranslationSeedItem("entity.qualityOperation.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.qualityOperation.plantcode
            new TranslationSeedItem("entity.qualityOperation.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.qualityOperation.code
            new TranslationSeedItem("entity.qualityOperation.code", "en-US", "品质业务编码", "品质业务编码(唯一,如:QO-2026-0001)"),
            // entity.qualityOperation.code
            new TranslationSeedItem("entity.qualityOperation.code", "ja-JP", "品质业务编码", "品质业务编码(唯一,如:QO-2026-0001)"),
            // entity.qualityOperation.code
            new TranslationSeedItem("entity.qualityOperation.code", "zh-CN", "品质业务编码", "品质业务编码(唯一,如:QO-2026-0001)"),
            // entity.qualityOperation.code
            new TranslationSeedItem("entity.qualityOperation.code", "zh-HK", "品质业务编码", "品质业务编码(唯一,如:QO-2026-0001)"),

            // entity.qualityOperation.operationmonth
            new TranslationSeedItem("entity.qualityOperation.operationmonth", "en-US", "业务年月", "业务年月(格式:2026-05)"),
            // entity.qualityOperation.operationmonth
            new TranslationSeedItem("entity.qualityOperation.operationmonth", "ja-JP", "业务年月", "业务年月(格式:2026-05)"),
            // entity.qualityOperation.operationmonth
            new TranslationSeedItem("entity.qualityOperation.operationmonth", "zh-CN", "业务年月", "业务年月(格式:2026-05)"),
            // entity.qualityOperation.operationmonth
            new TranslationSeedItem("entity.qualityOperation.operationmonth", "zh-HK", "业务年月", "业务年月(格式:2026-05)"),

            // entity.qualityOperation.customername
            new TranslationSeedItem("entity.qualityOperation.customername", "en-US", "顾客名", "顾客名"),
            // entity.qualityOperation.customername
            new TranslationSeedItem("entity.qualityOperation.customername", "ja-JP", "顾客名", "顾客名"),
            // entity.qualityOperation.customername
            new TranslationSeedItem("entity.qualityOperation.customername", "zh-CN", "顾客名", "顾客名"),
            // entity.qualityOperation.customername
            new TranslationSeedItem("entity.qualityOperation.customername", "zh-HK", "顾客名", "顾客名"),

            // entity.qualityOperation.debitnoteno
            new TranslationSeedItem("entity.qualityOperation.debitnoteno", "en-US", "Debit Note No", "Debit Note No"),
            // entity.qualityOperation.debitnoteno
            new TranslationSeedItem("entity.qualityOperation.debitnoteno", "ja-JP", "Debit Note No", "Debit Note No"),
            // entity.qualityOperation.debitnoteno
            new TranslationSeedItem("entity.qualityOperation.debitnoteno", "zh-CN", "Debit Note No", "Debit Note No"),
            // entity.qualityOperation.debitnoteno
            new TranslationSeedItem("entity.qualityOperation.debitnoteno", "zh-HK", "Debit Note No", "Debit Note No"),

            // entity.qualityOperation.recorder
            new TranslationSeedItem("entity.qualityOperation.recorder", "en-US", "记录者", "记录者"),
            // entity.qualityOperation.recorder
            new TranslationSeedItem("entity.qualityOperation.recorder", "ja-JP", "记录者", "记录者"),
            // entity.qualityOperation.recorder
            new TranslationSeedItem("entity.qualityOperation.recorder", "zh-CN", "记录者", "记录者"),
            // entity.qualityOperation.recorder
            new TranslationSeedItem("entity.qualityOperation.recorder", "zh-HK", "记录者", "记录者"),

            // entity.qualityOperation.totalqualitycost
            new TranslationSeedItem("entity.qualityOperation.totalqualitycost", "en-US", "质量总成本", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityOperation.totalqualitycost
            new TranslationSeedItem("entity.qualityOperation.totalqualitycost", "ja-JP", "质量总成本", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityOperation.totalqualitycost
            new TranslationSeedItem("entity.qualityOperation.totalqualitycost", "zh-CN", "质量总成本", "质量总成本(元,自动计算 = 各子表费用合计)"),
            // entity.qualityOperation.totalqualitycost
            new TranslationSeedItem("entity.qualityOperation.totalqualitycost", "zh-HK", "质量总成本", "质量总成本(元,自动计算 = 各子表费用合计)"),

            // entity.qualityOperation.costcurrency
            new TranslationSeedItem("entity.qualityOperation.costcurrency", "en-US", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityOperation.costcurrency
            new TranslationSeedItem("entity.qualityOperation.costcurrency", "ja-JP", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityOperation.costcurrency
            new TranslationSeedItem("entity.qualityOperation.costcurrency", "zh-CN", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityOperation.costcurrency
            new TranslationSeedItem("entity.qualityOperation.costcurrency", "zh-HK", "成本币种", "成本币种(CNY/USD/JPY等)"),

            // entity.qualityOperation.incomingitems
            new TranslationSeedItem("entity.qualityOperation.incomingitems", "en-US", "incomingItems", "来料检验费用明细列表"),
            // entity.qualityOperation.incomingitems
            new TranslationSeedItem("entity.qualityOperation.incomingitems", "ja-JP", "incomingItems", "来料检验费用明细列表"),
            // entity.qualityOperation.incomingitems
            new TranslationSeedItem("entity.qualityOperation.incomingitems", "zh-CN", "incomingItems", "来料检验费用明细列表"),
            // entity.qualityOperation.incomingitems
            new TranslationSeedItem("entity.qualityOperation.incomingitems", "zh-HK", "incomingItems", "来料检验费用明细列表"),
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
