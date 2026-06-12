// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation
// 文件名称：TaktSalaryItemI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalaryItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Compensation;

/// <summary>
/// TaktSalaryItem 实体国际化翻译种子（键前缀 entity.salaryitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalaryItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalaryItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salaryitem 实体翻译...", tenantCode);

        foreach (var item in GetSalaryItemTranslations())
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

        TaktLogger.Information("TaktSalaryItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalaryItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salaryitem._self / entity.salaryitem.{{field}}；ResourceGroup=5；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSalaryItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "en-US", "Salary Item Information", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "ja-JP", "薪资项目信息", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "zh-CN", "薪资项目信息", "实体名称"),
            // entity.salaryitem._self
            new TranslationSeedItem("entity.salaryitem._self", "zh-HK", "薪资项目信息", "实体名称"),

            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "en-US", "项目编码", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "ja-JP", "项目编码", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "zh-CN", "项目编码", "项目编码（租户+公司内唯一）"),
            // entity.salaryitem.itemcode
            new TranslationSeedItem("entity.salaryitem.itemcode", "zh-HK", "项目编码", "项目编码（租户+公司内唯一）"),

            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "en-US", "项目名称", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "ja-JP", "项目名称", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "zh-CN", "项目名称", "项目名称"),
            // entity.salaryitem.itemname
            new TranslationSeedItem("entity.salaryitem.itemname", "zh-HK", "项目名称", "项目名称"),

            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "en-US", "简称", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "ja-JP", "简称", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "zh-CN", "简称", "简称"),
            // entity.salaryitem.shortname
            new TranslationSeedItem("entity.salaryitem.shortname", "zh-HK", "简称", "简称"),

            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "en-US", "项目类型", "项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "ja-JP", "项目类型", "项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "zh-CN", "项目类型", "项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）"),
            // entity.salaryitem.itemtype
            new TranslationSeedItem("entity.salaryitem.itemtype", "zh-HK", "项目类型", "项目类型（字典 hr_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）"),

            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "en-US", "计算方式", "计算方式（字典 hr_salary_calc_method：固定金额/按比例/按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "ja-JP", "计算方式", "计算方式（字典 hr_salary_calc_method：固定金额/按比例/按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "zh-CN", "计算方式", "计算方式（字典 hr_salary_calc_method：固定金额/按比例/按公式）"),
            // entity.salaryitem.calcmethod
            new TranslationSeedItem("entity.salaryitem.calcmethod", "zh-HK", "计算方式", "计算方式（字典 hr_salary_calc_method：固定金额/按比例/按公式）"),

            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "en-US", "计算公式ID", "关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "ja-JP", "计算公式ID", "关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "zh-CN", "计算公式ID", "关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）"),
            // entity.salaryitem.salaryformulaid
            new TranslationSeedItem("entity.salaryitem.salaryformulaid", "zh-HK", "计算公式ID", "关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）"),

            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "en-US", "默认金额", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "ja-JP", "默认金额", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "zh-CN", "默认金额", "默认金额（元）"),
            // entity.salaryitem.defaultamount
            new TranslationSeedItem("entity.salaryitem.defaultamount", "zh-HK", "默认金额", "默认金额（元）"),

            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "en-US", "默认比例", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "ja-JP", "默认比例", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "zh-CN", "默认比例", "默认比例（%，0~100）"),
            // entity.salaryitem.defaultrate
            new TranslationSeedItem("entity.salaryitem.defaultrate", "zh-HK", "默认比例", "默认比例（%，0~100）"),

            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "en-US", "默认行权价格", "默认行权/授予价格（元；item_type 为股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "ja-JP", "默认行权价格", "默认行权/授予价格（元；item_type 为股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "zh-CN", "默认行权价格", "默认行权/授予价格（元；item_type 为股权激励时使用）"),
            // entity.salaryitem.strikeprice
            new TranslationSeedItem("entity.salaryitem.strikeprice", "zh-HK", "默认行权价格", "默认行权/授予价格（元；item_type 为股权激励时使用）"),

            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "en-US", "默认归属年限", "默认归属年限（年；item_type 为股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "ja-JP", "默认归属年限", "默认归属年限（年；item_type 为股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "zh-CN", "默认归属年限", "默认归属年限（年；item_type 为股权激励时使用）"),
            // entity.salaryitem.vestingyears
            new TranslationSeedItem("entity.salaryitem.vestingyears", "zh-HK", "默认归属年限", "默认归属年限（年；item_type 为股权激励时使用）"),

            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "en-US", "是否扣款项", "是否扣款项（字典 sys_yes_no）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "ja-JP", "是否扣款项", "是否扣款项（字典 sys_yes_no）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "zh-CN", "是否扣款项", "是否扣款项（字典 sys_yes_no）"),
            // entity.salaryitem.isdeduction
            new TranslationSeedItem("entity.salaryitem.isdeduction", "zh-HK", "是否扣款项", "是否扣款项（字典 sys_yes_no）"),

            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "en-US", "是否计入应税所得", "是否计入应税所得（字典 sys_yes_no）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "ja-JP", "是否计入应税所得", "是否计入应税所得（字典 sys_yes_no）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "zh-CN", "是否计入应税所得", "是否计入应税所得（字典 sys_yes_no）"),
            // entity.salaryitem.istaxable
            new TranslationSeedItem("entity.salaryitem.istaxable", "zh-HK", "是否计入应税所得", "是否计入应税所得（字典 sys_yes_no）"),

            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "en-US", "是否计入社保基数", "是否计入社保基数（字典 sys_yes_no）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "ja-JP", "是否计入社保基数", "是否计入社保基数（字典 sys_yes_no）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "zh-CN", "是否计入社保基数", "是否计入社保基数（字典 sys_yes_no）"),
            // entity.salaryitem.includesocialsecuritybase
            new TranslationSeedItem("entity.salaryitem.includesocialsecuritybase", "zh-HK", "是否计入社保基数", "是否计入社保基数（字典 sys_yes_no）"),

            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "en-US", "是否计入公积金基数", "是否计入公积金基数（字典 sys_yes_no）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "ja-JP", "是否计入公积金基数", "是否计入公积金基数（字典 sys_yes_no）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "zh-CN", "是否计入公积金基数", "是否计入公积金基数（字典 sys_yes_no）"),
            // entity.salaryitem.includehousingfundbase
            new TranslationSeedItem("entity.salaryitem.includehousingfundbase", "zh-HK", "是否计入公积金基数", "是否计入公积金基数（字典 sys_yes_no）"),

            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "en-US", "排序号", "排序号"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.salaryitem.sortorder
            new TranslationSeedItem("entity.salaryitem.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "en-US", "状态", "状态（字典 sys_normal_disable）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "ja-JP", "状态", "状态（字典 sys_normal_disable）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.salaryitem.itemstatus
            new TranslationSeedItem("entity.salaryitem.itemstatus", "zh-HK", "状态", "状态（字典 sys_normal_disable）"),

            // entity.salaryitem.relatedplant
            new TranslationSeedItem("entity.salaryitem.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.salaryitem.relatedplant
            new TranslationSeedItem("entity.salaryitem.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.salaryitem.relatedplant
            new TranslationSeedItem("entity.salaryitem.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.salaryitem.relatedplant
            new TranslationSeedItem("entity.salaryitem.relatedplant", "zh-HK", "关联工厂", "关联工厂"),
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
        translation.ResourceGroup = 5;
        translation.ResourceType = 0;
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
