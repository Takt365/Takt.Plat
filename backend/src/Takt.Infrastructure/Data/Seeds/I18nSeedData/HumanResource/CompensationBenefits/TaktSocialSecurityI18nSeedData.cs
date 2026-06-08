// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.CompensationBenefits
// 文件名称：TaktSocialSecurityI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSocialSecurity 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSocialSecurity 实体国际化翻译种子（键前缀 entity.socialSecurity.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSocialSecurityI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSocialSecurity 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 socialSecurity 实体翻译...", tenantCode);

        foreach (var item in GetSocialSecurityTranslations())
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

        TaktLogger.Information("TaktSocialSecurity 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSocialSecurity 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.socialSecurity._self / entity.socialSecurity.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSocialSecurityTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.socialSecurity._self
            new TranslationSeedItem("entity.socialSecurity._self", "en-US", "Social Security Information", "实体名称"),
            // entity.socialSecurity._self
            new TranslationSeedItem("entity.socialSecurity._self", "ja-JP", "员工社保缴纳记录信息", "实体名称"),
            // entity.socialSecurity._self
            new TranslationSeedItem("entity.socialSecurity._self", "zh-CN", "员工社保缴纳记录信息", "实体名称"),
            // entity.socialSecurity._self
            new TranslationSeedItem("entity.socialSecurity._self", "zh-HK", "员工社保缴纳记录信息", "实体名称"),

            // entity.socialSecurity.employeeid
            new TranslationSeedItem("entity.socialSecurity.employeeid", "en-US", "员工ID", "员工 ID"),
            // entity.socialSecurity.employeeid
            new TranslationSeedItem("entity.socialSecurity.employeeid", "ja-JP", "员工ID", "员工 ID"),
            // entity.socialSecurity.employeeid
            new TranslationSeedItem("entity.socialSecurity.employeeid", "zh-CN", "员工ID", "员工 ID"),
            // entity.socialSecurity.employeeid
            new TranslationSeedItem("entity.socialSecurity.employeeid", "zh-HK", "员工ID", "员工 ID"),

            // entity.socialSecurity.employeename
            new TranslationSeedItem("entity.socialSecurity.employeename", "en-US", "员工姓名", "员工姓名"),
            // entity.socialSecurity.employeename
            new TranslationSeedItem("entity.socialSecurity.employeename", "ja-JP", "员工姓名", "员工姓名"),
            // entity.socialSecurity.employeename
            new TranslationSeedItem("entity.socialSecurity.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.socialSecurity.employeename
            new TranslationSeedItem("entity.socialSecurity.employeename", "zh-HK", "员工姓名", "员工姓名"),

            // entity.socialSecurity.payperiod
            new TranslationSeedItem("entity.socialSecurity.payperiod", "en-US", "缴纳期间", "缴纳期间（如 2026-06）"),
            // entity.socialSecurity.payperiod
            new TranslationSeedItem("entity.socialSecurity.payperiod", "ja-JP", "缴纳期间", "缴纳期间（如 2026-06）"),
            // entity.socialSecurity.payperiod
            new TranslationSeedItem("entity.socialSecurity.payperiod", "zh-CN", "缴纳期间", "缴纳期间（如 2026-06）"),
            // entity.socialSecurity.payperiod
            new TranslationSeedItem("entity.socialSecurity.payperiod", "zh-HK", "缴纳期间", "缴纳期间（如 2026-06）"),

            // entity.socialSecurity.base
            new TranslationSeedItem("entity.socialSecurity.base", "en-US", "社保缴纳基数", "社保缴纳基数"),
            // entity.socialSecurity.base
            new TranslationSeedItem("entity.socialSecurity.base", "ja-JP", "社保缴纳基数", "社保缴纳基数"),
            // entity.socialSecurity.base
            new TranslationSeedItem("entity.socialSecurity.base", "zh-CN", "社保缴纳基数", "社保缴纳基数"),
            // entity.socialSecurity.base
            new TranslationSeedItem("entity.socialSecurity.base", "zh-HK", "社保缴纳基数", "社保缴纳基数"),

            // entity.socialSecurity.pensionamount
            new TranslationSeedItem("entity.socialSecurity.pensionamount", "en-US", "养老保险", "养老保险（元）"),
            // entity.socialSecurity.pensionamount
            new TranslationSeedItem("entity.socialSecurity.pensionamount", "ja-JP", "养老保险", "养老保险（元）"),
            // entity.socialSecurity.pensionamount
            new TranslationSeedItem("entity.socialSecurity.pensionamount", "zh-CN", "养老保险", "养老保险（元）"),
            // entity.socialSecurity.pensionamount
            new TranslationSeedItem("entity.socialSecurity.pensionamount", "zh-HK", "养老保险", "养老保险（元）"),

            // entity.socialSecurity.medicalamount
            new TranslationSeedItem("entity.socialSecurity.medicalamount", "en-US", "医疗保险", "医疗保险（元）"),
            // entity.socialSecurity.medicalamount
            new TranslationSeedItem("entity.socialSecurity.medicalamount", "ja-JP", "医疗保险", "医疗保险（元）"),
            // entity.socialSecurity.medicalamount
            new TranslationSeedItem("entity.socialSecurity.medicalamount", "zh-CN", "医疗保险", "医疗保险（元）"),
            // entity.socialSecurity.medicalamount
            new TranslationSeedItem("entity.socialSecurity.medicalamount", "zh-HK", "医疗保险", "医疗保险（元）"),

            // entity.socialSecurity.unemploymentamount
            new TranslationSeedItem("entity.socialSecurity.unemploymentamount", "en-US", "失业保险", "失业保险（元）"),
            // entity.socialSecurity.unemploymentamount
            new TranslationSeedItem("entity.socialSecurity.unemploymentamount", "ja-JP", "失业保险", "失业保险（元）"),
            // entity.socialSecurity.unemploymentamount
            new TranslationSeedItem("entity.socialSecurity.unemploymentamount", "zh-CN", "失业保险", "失业保险（元）"),
            // entity.socialSecurity.unemploymentamount
            new TranslationSeedItem("entity.socialSecurity.unemploymentamount", "zh-HK", "失业保险", "失业保险（元）"),

            // entity.socialSecurity.injuryamount
            new TranslationSeedItem("entity.socialSecurity.injuryamount", "en-US", "工伤保险", "工伤保险（元）"),
            // entity.socialSecurity.injuryamount
            new TranslationSeedItem("entity.socialSecurity.injuryamount", "ja-JP", "工伤保险", "工伤保险（元）"),
            // entity.socialSecurity.injuryamount
            new TranslationSeedItem("entity.socialSecurity.injuryamount", "zh-CN", "工伤保险", "工伤保险（元）"),
            // entity.socialSecurity.injuryamount
            new TranslationSeedItem("entity.socialSecurity.injuryamount", "zh-HK", "工伤保险", "工伤保险（元）"),

            // entity.socialSecurity.maternityamount
            new TranslationSeedItem("entity.socialSecurity.maternityamount", "en-US", "生育保险", "生育保险（元）"),
            // entity.socialSecurity.maternityamount
            new TranslationSeedItem("entity.socialSecurity.maternityamount", "ja-JP", "生育保险", "生育保险（元）"),
            // entity.socialSecurity.maternityamount
            new TranslationSeedItem("entity.socialSecurity.maternityamount", "zh-CN", "生育保险", "生育保险（元）"),
            // entity.socialSecurity.maternityamount
            new TranslationSeedItem("entity.socialSecurity.maternityamount", "zh-HK", "生育保险", "生育保险（元）"),

            // entity.socialSecurity.housingfundbase
            new TranslationSeedItem("entity.socialSecurity.housingfundbase", "en-US", "公积金缴纳基数", "公积金缴纳基数"),
            // entity.socialSecurity.housingfundbase
            new TranslationSeedItem("entity.socialSecurity.housingfundbase", "ja-JP", "公积金缴纳基数", "公积金缴纳基数"),
            // entity.socialSecurity.housingfundbase
            new TranslationSeedItem("entity.socialSecurity.housingfundbase", "zh-CN", "公积金缴纳基数", "公积金缴纳基数"),
            // entity.socialSecurity.housingfundbase
            new TranslationSeedItem("entity.socialSecurity.housingfundbase", "zh-HK", "公积金缴纳基数", "公积金缴纳基数"),

            // entity.socialSecurity.housingfundamount
            new TranslationSeedItem("entity.socialSecurity.housingfundamount", "en-US", "公积金", "公积金（元）"),
            // entity.socialSecurity.housingfundamount
            new TranslationSeedItem("entity.socialSecurity.housingfundamount", "ja-JP", "公积金", "公积金（元）"),
            // entity.socialSecurity.housingfundamount
            new TranslationSeedItem("entity.socialSecurity.housingfundamount", "zh-CN", "公积金", "公积金（元）"),
            // entity.socialSecurity.housingfundamount
            new TranslationSeedItem("entity.socialSecurity.housingfundamount", "zh-HK", "公积金", "公积金（元）"),

            // entity.socialSecurity.totalamount
            new TranslationSeedItem("entity.socialSecurity.totalamount", "en-US", "缴纳合计", "缴纳合计（元）"),
            // entity.socialSecurity.totalamount
            new TranslationSeedItem("entity.socialSecurity.totalamount", "ja-JP", "缴纳合计", "缴纳合计（元）"),
            // entity.socialSecurity.totalamount
            new TranslationSeedItem("entity.socialSecurity.totalamount", "zh-CN", "缴纳合计", "缴纳合计（元）"),
            // entity.socialSecurity.totalamount
            new TranslationSeedItem("entity.socialSecurity.totalamount", "zh-HK", "缴纳合计", "缴纳合计（元）"),

            // entity.socialSecurity.paystatus
            new TranslationSeedItem("entity.socialSecurity.paystatus", "en-US", "缴纳状态", "缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialSecurity.paystatus
            new TranslationSeedItem("entity.socialSecurity.paystatus", "ja-JP", "缴纳状态", "缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialSecurity.paystatus
            new TranslationSeedItem("entity.socialSecurity.paystatus", "zh-CN", "缴纳状态", "缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）"),
            // entity.socialSecurity.paystatus
            new TranslationSeedItem("entity.socialSecurity.paystatus", "zh-HK", "缴纳状态", "缴纳状态（0=待缴纳 1=已缴纳 2=已补缴）"),

            // entity.socialSecurity.relatedplant
            new TranslationSeedItem("entity.socialSecurity.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.socialSecurity.relatedplant
            new TranslationSeedItem("entity.socialSecurity.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.socialSecurity.relatedplant
            new TranslationSeedItem("entity.socialSecurity.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.socialSecurity.relatedplant
            new TranslationSeedItem("entity.socialSecurity.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
