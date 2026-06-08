// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktStandardWageRateI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardWageRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktStandardWageRate 实体国际化翻译种子（键前缀 entity.standardWageRate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardWageRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardWageRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardWageRate 实体翻译...", tenantCode);

        foreach (var item in GetStandardWageRateTranslations())
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

        TaktLogger.Information("TaktStandardWageRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardWageRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardWageRate._self / entity.standardWageRate.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardWageRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardWageRate._self
            new TranslationSeedItem("entity.standardWageRate._self", "en-US", "Standard Wage Rate Information", "实体名称"),
            // entity.standardWageRate._self
            new TranslationSeedItem("entity.standardWageRate._self", "ja-JP", "标准工资率信息", "实体名称"),
            // entity.standardWageRate._self
            new TranslationSeedItem("entity.standardWageRate._self", "zh-CN", "标准工资率信息", "实体名称"),
            // entity.standardWageRate._self
            new TranslationSeedItem("entity.standardWageRate._self", "zh-HK", "标准工资率信息", "实体名称"),

            // entity.standardWageRate.yearmonth
            new TranslationSeedItem("entity.standardWageRate.yearmonth", "en-US", "年月", "年月（yyyyMM）"),
            // entity.standardWageRate.yearmonth
            new TranslationSeedItem("entity.standardWageRate.yearmonth", "ja-JP", "年月", "年月（yyyyMM）"),
            // entity.standardWageRate.yearmonth
            new TranslationSeedItem("entity.standardWageRate.yearmonth", "zh-CN", "年月", "年月（yyyyMM）"),
            // entity.standardWageRate.yearmonth
            new TranslationSeedItem("entity.standardWageRate.yearmonth", "zh-HK", "年月", "年月（yyyyMM）"),

            // entity.standardWageRate.workingdays
            new TranslationSeedItem("entity.standardWageRate.workingdays", "en-US", "工作天数", "工作天数"),
            // entity.standardWageRate.workingdays
            new TranslationSeedItem("entity.standardWageRate.workingdays", "ja-JP", "工作天数", "工作天数"),
            // entity.standardWageRate.workingdays
            new TranslationSeedItem("entity.standardWageRate.workingdays", "zh-CN", "工作天数", "工作天数"),
            // entity.standardWageRate.workingdays
            new TranslationSeedItem("entity.standardWageRate.workingdays", "zh-HK", "工作天数", "工作天数"),

            // entity.standardWageRate.salesamount
            new TranslationSeedItem("entity.standardWageRate.salesamount", "en-US", "销售额", "销售额"),
            // entity.standardWageRate.salesamount
            new TranslationSeedItem("entity.standardWageRate.salesamount", "ja-JP", "销售额", "销售额"),
            // entity.standardWageRate.salesamount
            new TranslationSeedItem("entity.standardWageRate.salesamount", "zh-CN", "销售额", "销售额"),
            // entity.standardWageRate.salesamount
            new TranslationSeedItem("entity.standardWageRate.salesamount", "zh-HK", "销售额", "销售额"),

            // entity.standardWageRate.directlaborcount
            new TranslationSeedItem("entity.standardWageRate.directlaborcount", "en-US", "直接人数", "直接人数"),
            // entity.standardWageRate.directlaborcount
            new TranslationSeedItem("entity.standardWageRate.directlaborcount", "ja-JP", "直接人数", "直接人数"),
            // entity.standardWageRate.directlaborcount
            new TranslationSeedItem("entity.standardWageRate.directlaborcount", "zh-CN", "直接人数", "直接人数"),
            // entity.standardWageRate.directlaborcount
            new TranslationSeedItem("entity.standardWageRate.directlaborcount", "zh-HK", "直接人数", "直接人数"),

            // entity.standardWageRate.directlaborwage
            new TranslationSeedItem("entity.standardWageRate.directlaborwage", "en-US", "直接工资", "直接工资"),
            // entity.standardWageRate.directlaborwage
            new TranslationSeedItem("entity.standardWageRate.directlaborwage", "ja-JP", "直接工资", "直接工资"),
            // entity.standardWageRate.directlaborwage
            new TranslationSeedItem("entity.standardWageRate.directlaborwage", "zh-CN", "直接工资", "直接工资"),
            // entity.standardWageRate.directlaborwage
            new TranslationSeedItem("entity.standardWageRate.directlaborwage", "zh-HK", "直接工资", "直接工资"),

            // entity.standardWageRate.directovertimehours
            new TranslationSeedItem("entity.standardWageRate.directovertimehours", "en-US", "直接加班小时", "直接加班小时"),
            // entity.standardWageRate.directovertimehours
            new TranslationSeedItem("entity.standardWageRate.directovertimehours", "ja-JP", "直接加班小时", "直接加班小时"),
            // entity.standardWageRate.directovertimehours
            new TranslationSeedItem("entity.standardWageRate.directovertimehours", "zh-CN", "直接加班小时", "直接加班小时"),
            // entity.standardWageRate.directovertimehours
            new TranslationSeedItem("entity.standardWageRate.directovertimehours", "zh-HK", "直接加班小时", "直接加班小时"),

            // entity.standardWageRate.directovertimetotal
            new TranslationSeedItem("entity.standardWageRate.directovertimetotal", "en-US", "直接加班总额", "直接加班总额"),
            // entity.standardWageRate.directovertimetotal
            new TranslationSeedItem("entity.standardWageRate.directovertimetotal", "ja-JP", "直接加班总额", "直接加班总额"),
            // entity.standardWageRate.directovertimetotal
            new TranslationSeedItem("entity.standardWageRate.directovertimetotal", "zh-CN", "直接加班总额", "直接加班总额"),
            // entity.standardWageRate.directovertimetotal
            new TranslationSeedItem("entity.standardWageRate.directovertimetotal", "zh-HK", "直接加班总额", "直接加班总额"),

            // entity.standardWageRate.directwagerate
            new TranslationSeedItem("entity.standardWageRate.directwagerate", "en-US", "直接工资率", "直接工资率"),
            // entity.standardWageRate.directwagerate
            new TranslationSeedItem("entity.standardWageRate.directwagerate", "ja-JP", "直接工资率", "直接工资率"),
            // entity.standardWageRate.directwagerate
            new TranslationSeedItem("entity.standardWageRate.directwagerate", "zh-CN", "直接工资率", "直接工资率"),
            // entity.standardWageRate.directwagerate
            new TranslationSeedItem("entity.standardWageRate.directwagerate", "zh-HK", "直接工资率", "直接工资率"),

            // entity.standardWageRate.indirectlaborcount
            new TranslationSeedItem("entity.standardWageRate.indirectlaborcount", "en-US", "间接人数", "间接人数"),
            // entity.standardWageRate.indirectlaborcount
            new TranslationSeedItem("entity.standardWageRate.indirectlaborcount", "ja-JP", "间接人数", "间接人数"),
            // entity.standardWageRate.indirectlaborcount
            new TranslationSeedItem("entity.standardWageRate.indirectlaborcount", "zh-CN", "间接人数", "间接人数"),
            // entity.standardWageRate.indirectlaborcount
            new TranslationSeedItem("entity.standardWageRate.indirectlaborcount", "zh-HK", "间接人数", "间接人数"),

            // entity.standardWageRate.indirectlaborwage
            new TranslationSeedItem("entity.standardWageRate.indirectlaborwage", "en-US", "间接工资", "间接工资"),
            // entity.standardWageRate.indirectlaborwage
            new TranslationSeedItem("entity.standardWageRate.indirectlaborwage", "ja-JP", "间接工资", "间接工资"),
            // entity.standardWageRate.indirectlaborwage
            new TranslationSeedItem("entity.standardWageRate.indirectlaborwage", "zh-CN", "间接工资", "间接工资"),
            // entity.standardWageRate.indirectlaborwage
            new TranslationSeedItem("entity.standardWageRate.indirectlaborwage", "zh-HK", "间接工资", "间接工资"),

            // entity.standardWageRate.indirectovertimehours
            new TranslationSeedItem("entity.standardWageRate.indirectovertimehours", "en-US", "间接加班小时", "间接加班小时"),
            // entity.standardWageRate.indirectovertimehours
            new TranslationSeedItem("entity.standardWageRate.indirectovertimehours", "ja-JP", "间接加班小时", "间接加班小时"),
            // entity.standardWageRate.indirectovertimehours
            new TranslationSeedItem("entity.standardWageRate.indirectovertimehours", "zh-CN", "间接加班小时", "间接加班小时"),
            // entity.standardWageRate.indirectovertimehours
            new TranslationSeedItem("entity.standardWageRate.indirectovertimehours", "zh-HK", "间接加班小时", "间接加班小时"),

            // entity.standardWageRate.indirectovertimetotal
            new TranslationSeedItem("entity.standardWageRate.indirectovertimetotal", "en-US", "间接加班总额", "间接加班总额"),
            // entity.standardWageRate.indirectovertimetotal
            new TranslationSeedItem("entity.standardWageRate.indirectovertimetotal", "ja-JP", "间接加班总额", "间接加班总额"),
            // entity.standardWageRate.indirectovertimetotal
            new TranslationSeedItem("entity.standardWageRate.indirectovertimetotal", "zh-CN", "间接加班总额", "间接加班总额"),
            // entity.standardWageRate.indirectovertimetotal
            new TranslationSeedItem("entity.standardWageRate.indirectovertimetotal", "zh-HK", "间接加班总额", "间接加班总额"),

            // entity.standardWageRate.indirectwagerate
            new TranslationSeedItem("entity.standardWageRate.indirectwagerate", "en-US", "间接工资率", "间接工资率"),
            // entity.standardWageRate.indirectwagerate
            new TranslationSeedItem("entity.standardWageRate.indirectwagerate", "ja-JP", "间接工资率", "间接工资率"),
            // entity.standardWageRate.indirectwagerate
            new TranslationSeedItem("entity.standardWageRate.indirectwagerate", "zh-CN", "间接工资率", "间接工资率"),
            // entity.standardWageRate.indirectwagerate
            new TranslationSeedItem("entity.standardWageRate.indirectwagerate", "zh-HK", "间接工资率", "间接工资率"),

            // entity.standardWageRate.relatedplant
            new TranslationSeedItem("entity.standardWageRate.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.standardWageRate.relatedplant
            new TranslationSeedItem("entity.standardWageRate.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.standardWageRate.relatedplant
            new TranslationSeedItem("entity.standardWageRate.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.standardWageRate.relatedplant
            new TranslationSeedItem("entity.standardWageRate.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
        translation.ResourceGroup = TaktModule.Accounting;
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
