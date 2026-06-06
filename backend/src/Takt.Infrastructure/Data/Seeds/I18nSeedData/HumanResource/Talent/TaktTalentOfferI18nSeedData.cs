// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentOfferI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentOffer 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTalentOffer 实体国际化翻译种子（键前缀 entity.talentOffer.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentOfferI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentOffer 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentOffer 实体翻译...", tenantCode);

        foreach (var item in GetTalentOfferTranslations())
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

        TaktLogger.Information("TaktTalentOffer 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentOffer 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentOffer._self / entity.talentOffer.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentOfferTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentOffer._self
            new TranslationSeedItem("entity.talentOffer._self", "en-US", "Talent Offer Information", "实体名称"),
            // entity.talentOffer._self
            new TranslationSeedItem("entity.talentOffer._self", "ja-JP", "录用信息", "实体名称"),
            // entity.talentOffer._self
            new TranslationSeedItem("entity.talentOffer._self", "zh-CN", "录用信息", "实体名称"),
            // entity.talentOffer._self
            new TranslationSeedItem("entity.talentOffer._self", "zh-HK", "录用信息", "实体名称"),

            // entity.talentOffer.interviewid
            new TranslationSeedItem("entity.talentOffer.interviewid", "en-US", "面试安排ID", "面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）"),
            // entity.talentOffer.interviewid
            new TranslationSeedItem("entity.talentOffer.interviewid", "ja-JP", "面试安排ID", "面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）"),
            // entity.talentOffer.interviewid
            new TranslationSeedItem("entity.talentOffer.interviewid", "zh-CN", "面试安排ID", "面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）"),
            // entity.talentOffer.interviewid
            new TranslationSeedItem("entity.talentOffer.interviewid", "zh-HK", "面试安排ID", "面试安排ID（用人需求→招聘计划→职位发布→面试安排→录用）"),

            // entity.talentOffer.offerno
            new TranslationSeedItem("entity.talentOffer.offerno", "en-US", "录用编号", "录用编号（租户+公司内业务编号）"),
            // entity.talentOffer.offerno
            new TranslationSeedItem("entity.talentOffer.offerno", "ja-JP", "录用编号", "录用编号（租户+公司内业务编号）"),
            // entity.talentOffer.offerno
            new TranslationSeedItem("entity.talentOffer.offerno", "zh-CN", "录用编号", "录用编号（租户+公司内业务编号）"),
            // entity.talentOffer.offerno
            new TranslationSeedItem("entity.talentOffer.offerno", "zh-HK", "录用编号", "录用编号（租户+公司内业务编号）"),

            // entity.talentOffer.hiredate
            new TranslationSeedItem("entity.talentOffer.hiredate", "en-US", "录用日期", "录用日期（HireDate：确认录用/发 offer）"),
            // entity.talentOffer.hiredate
            new TranslationSeedItem("entity.talentOffer.hiredate", "ja-JP", "录用日期", "录用日期（HireDate：确认录用/发 offer）"),
            // entity.talentOffer.hiredate
            new TranslationSeedItem("entity.talentOffer.hiredate", "zh-CN", "录用日期", "录用日期（HireDate：确认录用/发 offer）"),
            // entity.talentOffer.hiredate
            new TranslationSeedItem("entity.talentOffer.hiredate", "zh-HK", "录用日期", "录用日期（HireDate：确认录用/发 offer）"),

            // entity.talentOffer.employeeid
            new TranslationSeedItem("entity.talentOffer.employeeid", "en-US", "关联员工ID", "关联员工ID（录用通过并建档后回填，可空）"),
            // entity.talentOffer.employeeid
            new TranslationSeedItem("entity.talentOffer.employeeid", "ja-JP", "关联员工ID", "关联员工ID（录用通过并建档后回填，可空）"),
            // entity.talentOffer.employeeid
            new TranslationSeedItem("entity.talentOffer.employeeid", "zh-CN", "关联员工ID", "关联员工ID（录用通过并建档后回填，可空）"),
            // entity.talentOffer.employeeid
            new TranslationSeedItem("entity.talentOffer.employeeid", "zh-HK", "关联员工ID", "关联员工ID（录用通过并建档后回填，可空）"),

            // entity.talentOffer.deptid
            new TranslationSeedItem("entity.talentOffer.deptid", "en-US", "拟录用部门ID", "拟录用部门ID"),
            // entity.talentOffer.deptid
            new TranslationSeedItem("entity.talentOffer.deptid", "ja-JP", "拟录用部门ID", "拟录用部门ID"),
            // entity.talentOffer.deptid
            new TranslationSeedItem("entity.talentOffer.deptid", "zh-CN", "拟录用部门ID", "拟录用部门ID"),
            // entity.talentOffer.deptid
            new TranslationSeedItem("entity.talentOffer.deptid", "zh-HK", "拟录用部门ID", "拟录用部门ID"),

            // entity.talentOffer.deptname
            new TranslationSeedItem("entity.talentOffer.deptname", "en-US", "拟录用部门名称", "拟录用部门名称"),
            // entity.talentOffer.deptname
            new TranslationSeedItem("entity.talentOffer.deptname", "ja-JP", "拟录用部门名称", "拟录用部门名称"),
            // entity.talentOffer.deptname
            new TranslationSeedItem("entity.talentOffer.deptname", "zh-CN", "拟录用部门名称", "拟录用部门名称"),
            // entity.talentOffer.deptname
            new TranslationSeedItem("entity.talentOffer.deptname", "zh-HK", "拟录用部门名称", "拟录用部门名称"),

            // entity.talentOffer.postid
            new TranslationSeedItem("entity.talentOffer.postid", "en-US", "拟录用岗位ID", "拟录用岗位ID"),
            // entity.talentOffer.postid
            new TranslationSeedItem("entity.talentOffer.postid", "ja-JP", "拟录用岗位ID", "拟录用岗位ID"),
            // entity.talentOffer.postid
            new TranslationSeedItem("entity.talentOffer.postid", "zh-CN", "拟录用岗位ID", "拟录用岗位ID"),
            // entity.talentOffer.postid
            new TranslationSeedItem("entity.talentOffer.postid", "zh-HK", "拟录用岗位ID", "拟录用岗位ID"),

            // entity.talentOffer.postname
            new TranslationSeedItem("entity.talentOffer.postname", "en-US", "拟录用岗位名称", "拟录用岗位名称"),
            // entity.talentOffer.postname
            new TranslationSeedItem("entity.talentOffer.postname", "ja-JP", "拟录用岗位名称", "拟录用岗位名称"),
            // entity.talentOffer.postname
            new TranslationSeedItem("entity.talentOffer.postname", "zh-CN", "拟录用岗位名称", "拟录用岗位名称"),
            // entity.talentOffer.postname
            new TranslationSeedItem("entity.talentOffer.postname", "zh-HK", "拟录用岗位名称", "拟录用岗位名称"),

            // entity.talentOffer.reason
            new TranslationSeedItem("entity.talentOffer.reason", "en-US", "录用说明", "录用说明"),
            // entity.talentOffer.reason
            new TranslationSeedItem("entity.talentOffer.reason", "ja-JP", "录用说明", "录用说明"),
            // entity.talentOffer.reason
            new TranslationSeedItem("entity.talentOffer.reason", "zh-CN", "录用说明", "录用说明"),
            // entity.talentOffer.reason
            new TranslationSeedItem("entity.talentOffer.reason", "zh-HK", "录用说明", "录用说明"),
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
