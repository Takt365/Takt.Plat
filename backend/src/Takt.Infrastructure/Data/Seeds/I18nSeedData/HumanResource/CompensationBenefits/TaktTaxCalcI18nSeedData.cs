// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalcI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTaxCalc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.CompensationBenefits;

/// <summary>
/// TaktTaxCalc 实体国际化翻译种子（键前缀 entity.taxCalc.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTaxCalcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTaxCalc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 taxCalc 实体翻译...", tenantCode);

        foreach (var item in GetTaxCalcTranslations())
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

        TaktLogger.Information("TaktTaxCalc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTaxCalc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.taxCalc._self / entity.taxCalc.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTaxCalcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.taxCalc._self
            new TranslationSeedItem("entity.taxCalc._self", "en-US", "Tax Calc Information", "实体名称"),
            // entity.taxCalc._self
            new TranslationSeedItem("entity.taxCalc._self", "ja-JP", "个税计算规则信息", "实体名称"),
            // entity.taxCalc._self
            new TranslationSeedItem("entity.taxCalc._self", "zh-CN", "个税计算规则信息", "实体名称"),
            // entity.taxCalc._self
            new TranslationSeedItem("entity.taxCalc._self", "zh-HK", "个税计算规则信息", "实体名称"),

            // entity.taxCalc.rulecode
            new TranslationSeedItem("entity.taxCalc.rulecode", "en-US", "规则编码", "规则编码（租户+公司内唯一）"),
            // entity.taxCalc.rulecode
            new TranslationSeedItem("entity.taxCalc.rulecode", "ja-JP", "规则编码", "规则编码（租户+公司内唯一）"),
            // entity.taxCalc.rulecode
            new TranslationSeedItem("entity.taxCalc.rulecode", "zh-CN", "规则编码", "规则编码（租户+公司内唯一）"),
            // entity.taxCalc.rulecode
            new TranslationSeedItem("entity.taxCalc.rulecode", "zh-HK", "规则编码", "规则编码（租户+公司内唯一）"),

            // entity.taxCalc.rulename
            new TranslationSeedItem("entity.taxCalc.rulename", "en-US", "规则名称", "规则名称"),
            // entity.taxCalc.rulename
            new TranslationSeedItem("entity.taxCalc.rulename", "ja-JP", "规则名称", "规则名称"),
            // entity.taxCalc.rulename
            new TranslationSeedItem("entity.taxCalc.rulename", "zh-CN", "规则名称", "规则名称"),
            // entity.taxCalc.rulename
            new TranslationSeedItem("entity.taxCalc.rulename", "zh-HK", "规则名称", "规则名称"),

            // entity.taxCalc.taxyear
            new TranslationSeedItem("entity.taxCalc.taxyear", "en-US", "税务年度", "税务年度"),
            // entity.taxCalc.taxyear
            new TranslationSeedItem("entity.taxCalc.taxyear", "ja-JP", "税务年度", "税务年度"),
            // entity.taxCalc.taxyear
            new TranslationSeedItem("entity.taxCalc.taxyear", "zh-CN", "税务年度", "税务年度"),
            // entity.taxCalc.taxyear
            new TranslationSeedItem("entity.taxCalc.taxyear", "zh-HK", "税务年度", "税务年度"),

            // entity.taxCalc.taxthreshold
            new TranslationSeedItem("entity.taxCalc.taxthreshold", "en-US", "税收起征点", "税收起征点"),
            // entity.taxCalc.taxthreshold
            new TranslationSeedItem("entity.taxCalc.taxthreshold", "ja-JP", "税收起征点", "税收起征点"),
            // entity.taxCalc.taxthreshold
            new TranslationSeedItem("entity.taxCalc.taxthreshold", "zh-CN", "税收起征点", "税收起征点"),
            // entity.taxCalc.taxthreshold
            new TranslationSeedItem("entity.taxCalc.taxthreshold", "zh-HK", "税收起征点", "税收起征点"),

            // entity.taxCalc.taxableincomemin
            new TranslationSeedItem("entity.taxCalc.taxableincomemin", "en-US", "应纳税所得额下限", "应纳税所得额下限"),
            // entity.taxCalc.taxableincomemin
            new TranslationSeedItem("entity.taxCalc.taxableincomemin", "ja-JP", "应纳税所得额下限", "应纳税所得额下限"),
            // entity.taxCalc.taxableincomemin
            new TranslationSeedItem("entity.taxCalc.taxableincomemin", "zh-CN", "应纳税所得额下限", "应纳税所得额下限"),
            // entity.taxCalc.taxableincomemin
            new TranslationSeedItem("entity.taxCalc.taxableincomemin", "zh-HK", "应纳税所得额下限", "应纳税所得额下限"),

            // entity.taxCalc.taxableincomemax
            new TranslationSeedItem("entity.taxCalc.taxableincomemax", "en-US", "应纳税所得额上限", "应纳税所得额上限"),
            // entity.taxCalc.taxableincomemax
            new TranslationSeedItem("entity.taxCalc.taxableincomemax", "ja-JP", "应纳税所得额上限", "应纳税所得额上限"),
            // entity.taxCalc.taxableincomemax
            new TranslationSeedItem("entity.taxCalc.taxableincomemax", "zh-CN", "应纳税所得额上限", "应纳税所得额上限"),
            // entity.taxCalc.taxableincomemax
            new TranslationSeedItem("entity.taxCalc.taxableincomemax", "zh-HK", "应纳税所得额上限", "应纳税所得额上限"),

            // entity.taxCalc.taxrate
            new TranslationSeedItem("entity.taxCalc.taxrate", "en-US", "税率", "税率（%）"),
            // entity.taxCalc.taxrate
            new TranslationSeedItem("entity.taxCalc.taxrate", "ja-JP", "税率", "税率（%）"),
            // entity.taxCalc.taxrate
            new TranslationSeedItem("entity.taxCalc.taxrate", "zh-CN", "税率", "税率（%）"),
            // entity.taxCalc.taxrate
            new TranslationSeedItem("entity.taxCalc.taxrate", "zh-HK", "税率", "税率（%）"),

            // entity.taxCalc.quickdeduction
            new TranslationSeedItem("entity.taxCalc.quickdeduction", "en-US", "速算扣除数", "速算扣除数"),
            // entity.taxCalc.quickdeduction
            new TranslationSeedItem("entity.taxCalc.quickdeduction", "ja-JP", "速算扣除数", "速算扣除数"),
            // entity.taxCalc.quickdeduction
            new TranslationSeedItem("entity.taxCalc.quickdeduction", "zh-CN", "速算扣除数", "速算扣除数"),
            // entity.taxCalc.quickdeduction
            new TranslationSeedItem("entity.taxCalc.quickdeduction", "zh-HK", "速算扣除数", "速算扣除数"),

            // entity.taxCalc.specialdeductionstandard
            new TranslationSeedItem("entity.taxCalc.specialdeductionstandard", "en-US", "专项扣除标准", "专项扣除标准"),
            // entity.taxCalc.specialdeductionstandard
            new TranslationSeedItem("entity.taxCalc.specialdeductionstandard", "ja-JP", "专项扣除标准", "专项扣除标准"),
            // entity.taxCalc.specialdeductionstandard
            new TranslationSeedItem("entity.taxCalc.specialdeductionstandard", "zh-CN", "专项扣除标准", "专项扣除标准"),
            // entity.taxCalc.specialdeductionstandard
            new TranslationSeedItem("entity.taxCalc.specialdeductionstandard", "zh-HK", "专项扣除标准", "专项扣除标准"),

            // entity.taxCalc.socialsecuritydeductionrate
            new TranslationSeedItem("entity.taxCalc.socialsecuritydeductionrate", "en-US", "社保扣除比例", "社保扣除比例（%）"),
            // entity.taxCalc.socialsecuritydeductionrate
            new TranslationSeedItem("entity.taxCalc.socialsecuritydeductionrate", "ja-JP", "社保扣除比例", "社保扣除比例（%）"),
            // entity.taxCalc.socialsecuritydeductionrate
            new TranslationSeedItem("entity.taxCalc.socialsecuritydeductionrate", "zh-CN", "社保扣除比例", "社保扣除比例（%）"),
            // entity.taxCalc.socialsecuritydeductionrate
            new TranslationSeedItem("entity.taxCalc.socialsecuritydeductionrate", "zh-HK", "社保扣除比例", "社保扣除比例（%）"),

            // entity.taxCalc.housingfunddeductionrate
            new TranslationSeedItem("entity.taxCalc.housingfunddeductionrate", "en-US", "公积金扣除比例", "公积金扣除比例（%）"),
            // entity.taxCalc.housingfunddeductionrate
            new TranslationSeedItem("entity.taxCalc.housingfunddeductionrate", "ja-JP", "公积金扣除比例", "公积金扣除比例（%）"),
            // entity.taxCalc.housingfunddeductionrate
            new TranslationSeedItem("entity.taxCalc.housingfunddeductionrate", "zh-CN", "公积金扣除比例", "公积金扣除比例（%）"),
            // entity.taxCalc.housingfunddeductionrate
            new TranslationSeedItem("entity.taxCalc.housingfunddeductionrate", "zh-HK", "公积金扣除比例", "公积金扣除比例（%）"),

            // entity.taxCalc.calculationformula
            new TranslationSeedItem("entity.taxCalc.calculationformula", "en-US", "计算公式", "计算公式"),
            // entity.taxCalc.calculationformula
            new TranslationSeedItem("entity.taxCalc.calculationformula", "ja-JP", "计算公式", "计算公式"),
            // entity.taxCalc.calculationformula
            new TranslationSeedItem("entity.taxCalc.calculationformula", "zh-CN", "计算公式", "计算公式"),
            // entity.taxCalc.calculationformula
            new TranslationSeedItem("entity.taxCalc.calculationformula", "zh-HK", "计算公式", "计算公式"),

            // entity.taxCalc.description
            new TranslationSeedItem("entity.taxCalc.description", "en-US", "规则说明", "规则说明"),
            // entity.taxCalc.description
            new TranslationSeedItem("entity.taxCalc.description", "ja-JP", "规则说明", "规则说明"),
            // entity.taxCalc.description
            new TranslationSeedItem("entity.taxCalc.description", "zh-CN", "规则说明", "规则说明"),
            // entity.taxCalc.description
            new TranslationSeedItem("entity.taxCalc.description", "zh-HK", "规则说明", "规则说明"),

            // entity.taxCalc.effectivedate
            new TranslationSeedItem("entity.taxCalc.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.taxCalc.effectivedate
            new TranslationSeedItem("entity.taxCalc.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.taxCalc.effectivedate
            new TranslationSeedItem("entity.taxCalc.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.taxCalc.effectivedate
            new TranslationSeedItem("entity.taxCalc.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.taxCalc.status
            new TranslationSeedItem("entity.taxCalc.status", "en-US", "状态", "状态（0=启用 1=停用）"),
            // entity.taxCalc.status
            new TranslationSeedItem("entity.taxCalc.status", "ja-JP", "状态", "状态（0=启用 1=停用）"),
            // entity.taxCalc.status
            new TranslationSeedItem("entity.taxCalc.status", "zh-CN", "状态", "状态（0=启用 1=停用）"),
            // entity.taxCalc.status
            new TranslationSeedItem("entity.taxCalc.status", "zh-HK", "状态", "状态（0=启用 1=停用）"),

            // entity.taxCalc.relatedplant
            new TranslationSeedItem("entity.taxCalc.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.taxCalc.relatedplant
            new TranslationSeedItem("entity.taxCalc.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.taxCalc.relatedplant
            new TranslationSeedItem("entity.taxCalc.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.taxCalc.relatedplant
            new TranslationSeedItem("entity.taxCalc.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
        translation.ResourceGroup = TaktModule.HumanResource;
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
