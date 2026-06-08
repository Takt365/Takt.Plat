// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentJobPostingI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentJobPosting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent;

/// <summary>
/// TaktTalentJobPosting 实体国际化翻译种子（键前缀 entity.talentJobPosting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentJobPostingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentJobPosting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentJobPosting 实体翻译...", tenantCode);

        foreach (var item in GetTalentJobPostingTranslations())
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

        TaktLogger.Information("TaktTalentJobPosting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentJobPosting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentJobPosting._self / entity.talentJobPosting.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentJobPostingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentJobPosting._self
            new TranslationSeedItem("entity.talentJobPosting._self", "en-US", "Talent Job Posting Information", "实体名称"),
            // entity.talentJobPosting._self
            new TranslationSeedItem("entity.talentJobPosting._self", "ja-JP", "职位发布信息", "实体名称"),
            // entity.talentJobPosting._self
            new TranslationSeedItem("entity.talentJobPosting._self", "zh-CN", "职位发布信息", "实体名称"),
            // entity.talentJobPosting._self
            new TranslationSeedItem("entity.talentJobPosting._self", "zh-HK", "职位发布信息", "实体名称"),

            // entity.talentJobPosting.recruitmentplanid
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplanid", "en-US", "招聘计划ID", "招聘计划ID"),
            // entity.talentJobPosting.recruitmentplanid
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplanid", "ja-JP", "招聘计划ID", "招聘计划ID"),
            // entity.talentJobPosting.recruitmentplanid
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplanid", "zh-CN", "招聘计划ID", "招聘计划ID"),
            // entity.talentJobPosting.recruitmentplanid
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplanid", "zh-HK", "招聘计划ID", "招聘计划ID"),

            // entity.talentJobPosting.postingcode
            new TranslationSeedItem("entity.talentJobPosting.postingcode", "en-US", "发布编号", "发布编号（租户+公司内唯一）"),
            // entity.talentJobPosting.postingcode
            new TranslationSeedItem("entity.talentJobPosting.postingcode", "ja-JP", "发布编号", "发布编号（租户+公司内唯一）"),
            // entity.talentJobPosting.postingcode
            new TranslationSeedItem("entity.talentJobPosting.postingcode", "zh-CN", "发布编号", "发布编号（租户+公司内唯一）"),
            // entity.talentJobPosting.postingcode
            new TranslationSeedItem("entity.talentJobPosting.postingcode", "zh-HK", "发布编号", "发布编号（租户+公司内唯一）"),

            // entity.talentJobPosting.title
            new TranslationSeedItem("entity.talentJobPosting.title", "en-US", "职位标题", "职位标题"),
            // entity.talentJobPosting.title
            new TranslationSeedItem("entity.talentJobPosting.title", "ja-JP", "职位标题", "职位标题"),
            // entity.talentJobPosting.title
            new TranslationSeedItem("entity.talentJobPosting.title", "zh-CN", "职位标题", "职位标题"),
            // entity.talentJobPosting.title
            new TranslationSeedItem("entity.talentJobPosting.title", "zh-HK", "职位标题", "职位标题"),

            // entity.talentJobPosting.postingstatus
            new TranslationSeedItem("entity.talentJobPosting.postingstatus", "en-US", "发布状态", "发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）"),
            // entity.talentJobPosting.postingstatus
            new TranslationSeedItem("entity.talentJobPosting.postingstatus", "ja-JP", "发布状态", "发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）"),
            // entity.talentJobPosting.postingstatus
            new TranslationSeedItem("entity.talentJobPosting.postingstatus", "zh-CN", "发布状态", "发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）"),
            // entity.talentJobPosting.postingstatus
            new TranslationSeedItem("entity.talentJobPosting.postingstatus", "zh-HK", "发布状态", "发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）"),

            // entity.talentJobPosting.publishdate
            new TranslationSeedItem("entity.talentJobPosting.publishdate", "en-US", "职位发布日期", "职位发布日期"),
            // entity.talentJobPosting.publishdate
            new TranslationSeedItem("entity.talentJobPosting.publishdate", "ja-JP", "职位发布日期", "职位发布日期"),
            // entity.talentJobPosting.publishdate
            new TranslationSeedItem("entity.talentJobPosting.publishdate", "zh-CN", "职位发布日期", "职位发布日期"),
            // entity.talentJobPosting.publishdate
            new TranslationSeedItem("entity.talentJobPosting.publishdate", "zh-HK", "职位发布日期", "职位发布日期"),

            // entity.talentJobPosting.opendate
            new TranslationSeedItem("entity.talentJobPosting.opendate", "en-US", "招聘开放日期", "招聘开放日期"),
            // entity.talentJobPosting.opendate
            new TranslationSeedItem("entity.talentJobPosting.opendate", "ja-JP", "招聘开放日期", "招聘开放日期"),
            // entity.talentJobPosting.opendate
            new TranslationSeedItem("entity.talentJobPosting.opendate", "zh-CN", "招聘开放日期", "招聘开放日期"),
            // entity.talentJobPosting.opendate
            new TranslationSeedItem("entity.talentJobPosting.opendate", "zh-HK", "招聘开放日期", "招聘开放日期"),

            // entity.talentJobPosting.closedate
            new TranslationSeedItem("entity.talentJobPosting.closedate", "en-US", "招聘关闭日期", "招聘关闭日期"),
            // entity.talentJobPosting.closedate
            new TranslationSeedItem("entity.talentJobPosting.closedate", "ja-JP", "招聘关闭日期", "招聘关闭日期"),
            // entity.talentJobPosting.closedate
            new TranslationSeedItem("entity.talentJobPosting.closedate", "zh-CN", "招聘关闭日期", "招聘关闭日期"),
            // entity.talentJobPosting.closedate
            new TranslationSeedItem("entity.talentJobPosting.closedate", "zh-HK", "招聘关闭日期", "招聘关闭日期"),

            // entity.talentJobPosting.publishchannel
            new TranslationSeedItem("entity.talentJobPosting.publishchannel", "en-US", "发布渠道", "发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）"),
            // entity.talentJobPosting.publishchannel
            new TranslationSeedItem("entity.talentJobPosting.publishchannel", "ja-JP", "发布渠道", "发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）"),
            // entity.talentJobPosting.publishchannel
            new TranslationSeedItem("entity.talentJobPosting.publishchannel", "zh-CN", "发布渠道", "发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）"),
            // entity.talentJobPosting.publishchannel
            new TranslationSeedItem("entity.talentJobPosting.publishchannel", "zh-HK", "发布渠道", "发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）"),

            // entity.talentJobPosting.reason
            new TranslationSeedItem("entity.talentJobPosting.reason", "en-US", "发布说明", "发布说明"),
            // entity.talentJobPosting.reason
            new TranslationSeedItem("entity.talentJobPosting.reason", "ja-JP", "发布说明", "发布说明"),
            // entity.talentJobPosting.reason
            new TranslationSeedItem("entity.talentJobPosting.reason", "zh-CN", "发布说明", "发布说明"),
            // entity.talentJobPosting.reason
            new TranslationSeedItem("entity.talentJobPosting.reason", "zh-HK", "发布说明", "发布说明"),

            // entity.talentJobPosting.recruitmentplan
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplan", "en-US", "招聘计划", "招聘计划"),
            // entity.talentJobPosting.recruitmentplan
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplan", "ja-JP", "招聘计划", "招聘计划"),
            // entity.talentJobPosting.recruitmentplan
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplan", "zh-CN", "招聘计划", "招聘计划"),
            // entity.talentJobPosting.recruitmentplan
            new TranslationSeedItem("entity.talentJobPosting.recruitmentplan", "zh-HK", "招聘计划", "招聘计划"),

            // entity.talentJobPosting.talentinterviews
            new TranslationSeedItem("entity.talentJobPosting.talentinterviews", "en-US", "面试安排", "面试安排"),
            // entity.talentJobPosting.talentinterviews
            new TranslationSeedItem("entity.talentJobPosting.talentinterviews", "ja-JP", "面试安排", "面试安排"),
            // entity.talentJobPosting.talentinterviews
            new TranslationSeedItem("entity.talentJobPosting.talentinterviews", "zh-CN", "面试安排", "面试安排"),
            // entity.talentJobPosting.talentinterviews
            new TranslationSeedItem("entity.talentJobPosting.talentinterviews", "zh-HK", "面试安排", "面试安排"),
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
