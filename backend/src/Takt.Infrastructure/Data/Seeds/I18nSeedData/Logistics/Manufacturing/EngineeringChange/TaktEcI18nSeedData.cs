// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEc 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEc 实体国际化翻译种子（键前缀 entity.ec.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ec 实体翻译...", tenantCode);

        foreach (var item in GetEcTranslations())
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

        TaktLogger.Information("TaktEc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ec._self / entity.ec.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "en-US", "Ec Information", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "ja-JP", "设变信息", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "zh-CN", "设变信息", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "zh-HK", "设变信息", "实体名称"),

            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "en-US", "设变单号", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "ja-JP", "设变单号", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "zh-CN", "设变单号", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "zh-HK", "设变单号", "设变单号（唯一）"),

            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "en-US", "发行日期", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "ja-JP", "发行日期", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "zh-CN", "发行日期", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "zh-HK", "发行日期", "发行日期"),

            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "en-US", "变更状态", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "ja-JP", "变更状态", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "zh-CN", "变更状态", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "zh-HK", "变更状态", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),

            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "en-US", "设变主题", "设变主题/标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "ja-JP", "设变主题", "设变主题/标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "zh-CN", "设变主题", "设变主题/标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "zh-HK", "设变主题", "设变主题/标题"),

            // entity.ec.detailtext
            new TranslationSeedItem("entity.ec.detailtext", "en-US", "设变详情", "设变详情/详细说明"),
            // entity.ec.detailtext
            new TranslationSeedItem("entity.ec.detailtext", "ja-JP", "设变详情", "设变详情/详细说明"),
            // entity.ec.detailtext
            new TranslationSeedItem("entity.ec.detailtext", "zh-CN", "设变详情", "设变详情/详细说明"),
            // entity.ec.detailtext
            new TranslationSeedItem("entity.ec.detailtext", "zh-HK", "设变详情", "设变详情/详细说明"),

            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "en-US", "负责人", "负责人"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "ja-JP", "负责人", "负责人"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "zh-CN", "负责人", "负责人"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "zh-HK", "负责人", "负责人"),

            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "en-US", "损失金额", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "ja-JP", "损失金额", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "zh-CN", "损失金额", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "zh-HK", "损失金额", "损失金额"),

            // entity.ec.distinction
            new TranslationSeedItem("entity.ec.distinction", "en-US", "区分", "区分/类别 1:全仕向，2：部管，3：内部，4：技术"),
            // entity.ec.distinction
            new TranslationSeedItem("entity.ec.distinction", "ja-JP", "区分", "区分/类别 1:全仕向，2：部管，3：内部，4：技术"),
            // entity.ec.distinction
            new TranslationSeedItem("entity.ec.distinction", "zh-CN", "区分", "区分/类别 1:全仕向，2：部管，3：内部，4：技术"),
            // entity.ec.distinction
            new TranslationSeedItem("entity.ec.distinction", "zh-HK", "区分", "区分/类别 1:全仕向，2：部管，3：内部，4：技术"),

            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "en-US", "录入日期", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "ja-JP", "录入日期", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "zh-CN", "录入日期", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "zh-HK", "录入日期", "录入日期"),

            // entity.ec.flowinstanceid
            new TranslationSeedItem("entity.ec.flowinstanceid", "en-US", "流程实例ID", "流程实例ID（关联工作流）"),
            // entity.ec.flowinstanceid
            new TranslationSeedItem("entity.ec.flowinstanceid", "ja-JP", "流程实例ID", "流程实例ID（关联工作流）"),
            // entity.ec.flowinstanceid
            new TranslationSeedItem("entity.ec.flowinstanceid", "zh-CN", "流程实例ID", "流程实例ID（关联工作流）"),
            // entity.ec.flowinstanceid
            new TranslationSeedItem("entity.ec.flowinstanceid", "zh-HK", "流程实例ID", "流程实例ID（关联工作流）"),

            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "en-US", "设变状态", "设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "ja-JP", "设变状态", "设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "zh-CN", "设变状态", "设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "zh-HK", "设变状态", "设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）"),

            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "en-US", "ecDetails", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "ja-JP", "ecDetails", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "zh-CN", "ecDetails", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "zh-HK", "ecDetails", "设变明细列表"),
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
