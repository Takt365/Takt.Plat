// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentInterviewI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentInterview 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTalentInterview 实体国际化翻译种子（键前缀 entity.talentInterview.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentInterviewI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentInterview 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentInterview 实体翻译...", tenantCode);

        foreach (var item in GetTalentInterviewTranslations())
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

        TaktLogger.Information("TaktTalentInterview 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentInterview 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentInterview._self / entity.talentInterview.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentInterviewTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentInterview._self
            new TranslationSeedItem("entity.talentInterview._self", "en-US", "Talent Interview Information", "实体名称"),
            // entity.talentInterview._self
            new TranslationSeedItem("entity.talentInterview._self", "ja-JP", "面试安排信息", "实体名称"),
            // entity.talentInterview._self
            new TranslationSeedItem("entity.talentInterview._self", "zh-CN", "面试安排信息", "实体名称"),
            // entity.talentInterview._self
            new TranslationSeedItem("entity.talentInterview._self", "zh-HK", "面试安排信息", "实体名称"),

            // entity.talentInterview.jobpostingid
            new TranslationSeedItem("entity.talentInterview.jobpostingid", "en-US", "职位发布ID", "职位发布ID"),
            // entity.talentInterview.jobpostingid
            new TranslationSeedItem("entity.talentInterview.jobpostingid", "ja-JP", "职位发布ID", "职位发布ID"),
            // entity.talentInterview.jobpostingid
            new TranslationSeedItem("entity.talentInterview.jobpostingid", "zh-CN", "职位发布ID", "职位发布ID"),
            // entity.talentInterview.jobpostingid
            new TranslationSeedItem("entity.talentInterview.jobpostingid", "zh-HK", "职位发布ID", "职位发布ID"),

            // entity.talentInterview.interviewno
            new TranslationSeedItem("entity.talentInterview.interviewno", "en-US", "面试单号", "面试单号（租户+公司内业务编号）"),
            // entity.talentInterview.interviewno
            new TranslationSeedItem("entity.talentInterview.interviewno", "ja-JP", "面试单号", "面试单号（租户+公司内业务编号）"),
            // entity.talentInterview.interviewno
            new TranslationSeedItem("entity.talentInterview.interviewno", "zh-CN", "面试单号", "面试单号（租户+公司内业务编号）"),
            // entity.talentInterview.interviewno
            new TranslationSeedItem("entity.talentInterview.interviewno", "zh-HK", "面试单号", "面试单号（租户+公司内业务编号）"),

            // entity.talentInterview.interviewstatus
            new TranslationSeedItem("entity.talentInterview.interviewstatus", "en-US", "面试办理状态", "面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）"),
            // entity.talentInterview.interviewstatus
            new TranslationSeedItem("entity.talentInterview.interviewstatus", "ja-JP", "面试办理状态", "面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）"),
            // entity.talentInterview.interviewstatus
            new TranslationSeedItem("entity.talentInterview.interviewstatus", "zh-CN", "面试办理状态", "面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）"),
            // entity.talentInterview.interviewstatus
            new TranslationSeedItem("entity.talentInterview.interviewstatus", "zh-HK", "面试办理状态", "面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）"),

            // entity.talentInterview.interviewround
            new TranslationSeedItem("entity.talentInterview.interviewround", "en-US", "面试轮次", "面试轮次（1=初试，2=复试，3=终试）"),
            // entity.talentInterview.interviewround
            new TranslationSeedItem("entity.talentInterview.interviewround", "ja-JP", "面试轮次", "面试轮次（1=初试，2=复试，3=终试）"),
            // entity.talentInterview.interviewround
            new TranslationSeedItem("entity.talentInterview.interviewround", "zh-CN", "面试轮次", "面试轮次（1=初试，2=复试，3=终试）"),
            // entity.talentInterview.interviewround
            new TranslationSeedItem("entity.talentInterview.interviewround", "zh-HK", "面试轮次", "面试轮次（1=初试，2=复试，3=终试）"),

            // entity.talentInterview.interviewdate
            new TranslationSeedItem("entity.talentInterview.interviewdate", "en-US", "面试时间", "面试时间"),
            // entity.talentInterview.interviewdate
            new TranslationSeedItem("entity.talentInterview.interviewdate", "ja-JP", "面试时间", "面试时间"),
            // entity.talentInterview.interviewdate
            new TranslationSeedItem("entity.talentInterview.interviewdate", "zh-CN", "面试时间", "面试时间"),
            // entity.talentInterview.interviewdate
            new TranslationSeedItem("entity.talentInterview.interviewdate", "zh-HK", "面试时间", "面试时间"),

            // entity.talentInterview.interviewername
            new TranslationSeedItem("entity.talentInterview.interviewername", "en-US", "面试官姓名", "面试官姓名"),
            // entity.talentInterview.interviewername
            new TranslationSeedItem("entity.talentInterview.interviewername", "ja-JP", "面试官姓名", "面试官姓名"),
            // entity.talentInterview.interviewername
            new TranslationSeedItem("entity.talentInterview.interviewername", "zh-CN", "面试官姓名", "面试官姓名"),
            // entity.talentInterview.interviewername
            new TranslationSeedItem("entity.talentInterview.interviewername", "zh-HK", "面试官姓名", "面试官姓名"),

            // entity.talentInterview.candidatename
            new TranslationSeedItem("entity.talentInterview.candidatename", "en-US", "候选人姓名", "候选人姓名"),
            // entity.talentInterview.candidatename
            new TranslationSeedItem("entity.talentInterview.candidatename", "ja-JP", "候选人姓名", "候选人姓名"),
            // entity.talentInterview.candidatename
            new TranslationSeedItem("entity.talentInterview.candidatename", "zh-CN", "候选人姓名", "候选人姓名"),
            // entity.talentInterview.candidatename
            new TranslationSeedItem("entity.talentInterview.candidatename", "zh-HK", "候选人姓名", "候选人姓名"),

            // entity.talentInterview.mobile
            new TranslationSeedItem("entity.talentInterview.mobile", "en-US", "候选人手机", "候选人手机"),
            // entity.talentInterview.mobile
            new TranslationSeedItem("entity.talentInterview.mobile", "ja-JP", "候选人手机", "候选人手机"),
            // entity.talentInterview.mobile
            new TranslationSeedItem("entity.talentInterview.mobile", "zh-CN", "候选人手机", "候选人手机"),
            // entity.talentInterview.mobile
            new TranslationSeedItem("entity.talentInterview.mobile", "zh-HK", "候选人手机", "候选人手机"),

            // entity.talentInterview.email
            new TranslationSeedItem("entity.talentInterview.email", "en-US", "候选人邮箱", "候选人邮箱"),
            // entity.talentInterview.email
            new TranslationSeedItem("entity.talentInterview.email", "ja-JP", "候选人邮箱", "候选人邮箱"),
            // entity.talentInterview.email
            new TranslationSeedItem("entity.talentInterview.email", "zh-CN", "候选人邮箱", "候选人邮箱"),
            // entity.talentInterview.email
            new TranslationSeedItem("entity.talentInterview.email", "zh-HK", "候选人邮箱", "候选人邮箱"),

            // entity.talentInterview.interviewlocation
            new TranslationSeedItem("entity.talentInterview.interviewlocation", "en-US", "面试地点", "面试地点"),
            // entity.talentInterview.interviewlocation
            new TranslationSeedItem("entity.talentInterview.interviewlocation", "ja-JP", "面试地点", "面试地点"),
            // entity.talentInterview.interviewlocation
            new TranslationSeedItem("entity.talentInterview.interviewlocation", "zh-CN", "面试地点", "面试地点"),
            // entity.talentInterview.interviewlocation
            new TranslationSeedItem("entity.talentInterview.interviewlocation", "zh-HK", "面试地点", "面试地点"),

            // entity.talentInterview.reason
            new TranslationSeedItem("entity.talentInterview.reason", "en-US", "面试说明", "面试说明"),
            // entity.talentInterview.reason
            new TranslationSeedItem("entity.talentInterview.reason", "ja-JP", "面试说明", "面试说明"),
            // entity.talentInterview.reason
            new TranslationSeedItem("entity.talentInterview.reason", "zh-CN", "面试说明", "面试说明"),
            // entity.talentInterview.reason
            new TranslationSeedItem("entity.talentInterview.reason", "zh-HK", "面试说明", "面试说明"),
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
