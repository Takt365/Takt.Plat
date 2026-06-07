// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktCountersignI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCountersign 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktCountersign 实体国际化翻译种子（键前缀 entity.countersign.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCountersignI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCountersign 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 countersign 实体翻译...", tenantCode);

        foreach (var item in GetCountersignTranslations())
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

        TaktLogger.Information("TaktCountersign 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCountersign 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.countersign._self / entity.countersign.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCountersignTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "en-US", "Countersign Information", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "ja-JP", "会签单信息", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "zh-CN", "会签单信息", "实体名称"),
            // entity.countersign._self
            new TranslationSeedItem("entity.countersign._self", "zh-HK", "会签单信息", "实体名称"),

            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "en-US", "会签编号", "会签编号"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "ja-JP", "会签编号", "会签编号"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "zh-CN", "会签编号", "会签编号"),
            // entity.countersign.code
            new TranslationSeedItem("entity.countersign.code", "zh-HK", "会签编号", "会签编号"),

            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "en-US", "会签部门", "会签部门 JSON"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "ja-JP", "会签部门", "会签部门 JSON"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "zh-CN", "会签部门", "会签部门 JSON"),
            // entity.countersign.depts
            new TranslationSeedItem("entity.countersign.depts", "zh-HK", "会签部门", "会签部门 JSON"),

            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "en-US", "财务部门", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "ja-JP", "财务部门", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "zh-CN", "财务部门", "财务部门 JSON"),
            // entity.countersign.financedept
            new TranslationSeedItem("entity.countersign.financedept", "zh-HK", "财务部门", "财务部门 JSON"),

            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "en-US", "预算审核意见", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "ja-JP", "预算审核意见", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "zh-CN", "预算审核意见", "预算审核意见"),
            // entity.countersign.budgetreviewcomment
            new TranslationSeedItem("entity.countersign.budgetreviewcomment", "zh-HK", "预算审核意见", "预算审核意见"),

            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "en-US", "总经室", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "ja-JP", "总经室", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "zh-CN", "总经室", "总经室 JSON"),
            // entity.countersign.executiveoffice
            new TranslationSeedItem("entity.countersign.executiveoffice", "zh-HK", "总经室", "总经室 JSON"),

            // entity.countersign.flowinstanceid
            new TranslationSeedItem("entity.countersign.flowinstanceid", "en-US", "流程实例ID", "流程实例 ID"),
            // entity.countersign.flowinstanceid
            new TranslationSeedItem("entity.countersign.flowinstanceid", "ja-JP", "流程实例ID", "流程实例 ID"),
            // entity.countersign.flowinstanceid
            new TranslationSeedItem("entity.countersign.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID"),
            // entity.countersign.flowinstanceid
            new TranslationSeedItem("entity.countersign.flowinstanceid", "zh-HK", "流程实例ID", "流程实例 ID"),

            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "en-US", "申请人", "申请人（员工 ID）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "ja-JP", "申请人", "申请人（员工 ID）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "zh-CN", "申请人", "申请人（员工 ID）"),
            // entity.countersign.applicantby
            new TranslationSeedItem("entity.countersign.applicantby", "zh-HK", "申请人", "申请人（员工 ID）"),

            // entity.countersign.applicationdept
            new TranslationSeedItem("entity.countersign.applicationdept", "en-US", "申请部门", "申请部门"),
            // entity.countersign.applicationdept
            new TranslationSeedItem("entity.countersign.applicationdept", "ja-JP", "申请部门", "申请部门"),
            // entity.countersign.applicationdept
            new TranslationSeedItem("entity.countersign.applicationdept", "zh-CN", "申请部门", "申请部门"),
            // entity.countersign.applicationdept
            new TranslationSeedItem("entity.countersign.applicationdept", "zh-HK", "申请部门", "申请部门"),

            // entity.countersign.costbearerdept
            new TranslationSeedItem("entity.countersign.costbearerdept", "en-US", "经费负担部门", "经费负担部门"),
            // entity.countersign.costbearerdept
            new TranslationSeedItem("entity.countersign.costbearerdept", "ja-JP", "经费负担部门", "经费负担部门"),
            // entity.countersign.costbearerdept
            new TranslationSeedItem("entity.countersign.costbearerdept", "zh-CN", "经费负担部门", "经费负担部门"),
            // entity.countersign.costbearerdept
            new TranslationSeedItem("entity.countersign.costbearerdept", "zh-HK", "经费负担部门", "经费负担部门"),

            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "en-US", "是否有预算", "是否有预算"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "ja-JP", "是否有预算", "是否有预算"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "zh-CN", "是否有预算", "是否有预算"),
            // entity.countersign.isbudget
            new TranslationSeedItem("entity.countersign.isbudget", "zh-HK", "是否有预算", "是否有预算"),

            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "en-US", "预算项目", "预算项目"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "ja-JP", "预算项目", "预算项目"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "zh-CN", "预算项目", "预算项目"),
            // entity.countersign.budgetitem
            new TranslationSeedItem("entity.countersign.budgetitem", "zh-HK", "预算项目", "预算项目"),

            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "en-US", "预算金额", "预算金额"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "ja-JP", "预算金额", "预算金额"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "zh-CN", "预算金额", "预算金额"),
            // entity.countersign.budgetamount
            new TranslationSeedItem("entity.countersign.budgetamount", "zh-HK", "预算金额", "预算金额"),

            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "en-US", "申请金额", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "ja-JP", "申请金额", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "zh-CN", "申请金额", "申请金额"),
            // entity.countersign.applicationamount
            new TranslationSeedItem("entity.countersign.applicationamount", "zh-HK", "申请金额", "申请金额"),

            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "en-US", "标题", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "ja-JP", "标题", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "zh-CN", "标题", "标题"),
            // entity.countersign.title
            new TranslationSeedItem("entity.countersign.title", "zh-HK", "标题", "标题"),

            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "en-US", "申请原因", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "ja-JP", "申请原因", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "zh-CN", "申请原因", "申请原因"),
            // entity.countersign.applicationreason
            new TranslationSeedItem("entity.countersign.applicationreason", "zh-HK", "申请原因", "申请原因"),

            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "en-US", "预算使用说明", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "ja-JP", "预算使用说明", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "zh-CN", "预算使用说明", "预算使用说明"),
            // entity.countersign.budgetusagedescription
            new TranslationSeedItem("entity.countersign.budgetusagedescription", "zh-HK", "预算使用说明", "预算使用说明"),

            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "en-US", "目标与预期效益", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "ja-JP", "目标与预期效益", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "zh-CN", "目标与预期效益", "目标与预期效益"),
            // entity.countersign.targetandexpectedbenefit
            new TranslationSeedItem("entity.countersign.targetandexpectedbenefit", "zh-HK", "目标与预期效益", "目标与预期效益"),

            // entity.countersign.attachments
            new TranslationSeedItem("entity.countersign.attachments", "en-US", "附件", "附件 JSON"),
            // entity.countersign.attachments
            new TranslationSeedItem("entity.countersign.attachments", "ja-JP", "附件", "附件 JSON"),
            // entity.countersign.attachments
            new TranslationSeedItem("entity.countersign.attachments", "zh-CN", "附件", "附件 JSON"),
            // entity.countersign.attachments
            new TranslationSeedItem("entity.countersign.attachments", "zh-HK", "附件", "附件 JSON"),

            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "en-US", "会签单状态", "会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "ja-JP", "会签单状态", "会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "zh-CN", "会签单状态", "会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）"),
            // entity.countersign.status
            new TranslationSeedItem("entity.countersign.status", "zh-HK", "会签单状态", "会签单业务状态（0=草稿，1=审批中，2=已承认，3=已驳回）"),
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
