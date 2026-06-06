// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeExperienceI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeExperience 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeExperience 实体国际化翻译种子（键前缀 entity.employeeExperience.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeExperienceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeExperience 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeExperience 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeExperienceTranslations())
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

        TaktLogger.Information("TaktEmployeeExperience 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeExperience 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeExperience._self / entity.employeeExperience.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeExperienceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeExperience._self
            new TranslationSeedItem("entity.employeeExperience._self", "en-US", "Employee Experience Information", "实体名称"),
            // entity.employeeExperience._self
            new TranslationSeedItem("entity.employeeExperience._self", "ja-JP", "员工外部工作经历信息", "实体名称"),
            // entity.employeeExperience._self
            new TranslationSeedItem("entity.employeeExperience._self", "zh-CN", "员工外部工作经历信息", "实体名称"),
            // entity.employeeExperience._self
            new TranslationSeedItem("entity.employeeExperience._self", "zh-HK", "员工外部工作经历信息", "实体名称"),

            // entity.employeeExperience.employeeid
            new TranslationSeedItem("entity.employeeExperience.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeExperience.employeeid
            new TranslationSeedItem("entity.employeeExperience.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeExperience.employeeid
            new TranslationSeedItem("entity.employeeExperience.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeExperience.employeeid
            new TranslationSeedItem("entity.employeeExperience.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeExperience.companyname
            new TranslationSeedItem("entity.employeeExperience.companyname", "en-US", "工作单位", "工作单位名称"),
            // entity.employeeExperience.companyname
            new TranslationSeedItem("entity.employeeExperience.companyname", "ja-JP", "工作单位", "工作单位名称"),
            // entity.employeeExperience.companyname
            new TranslationSeedItem("entity.employeeExperience.companyname", "zh-CN", "工作单位", "工作单位名称"),
            // entity.employeeExperience.companyname
            new TranslationSeedItem("entity.employeeExperience.companyname", "zh-HK", "工作单位", "工作单位名称"),

            // entity.employeeExperience.positionname
            new TranslationSeedItem("entity.employeeExperience.positionname", "en-US", "职位名称", "职位名称"),
            // entity.employeeExperience.positionname
            new TranslationSeedItem("entity.employeeExperience.positionname", "ja-JP", "职位名称", "职位名称"),
            // entity.employeeExperience.positionname
            new TranslationSeedItem("entity.employeeExperience.positionname", "zh-CN", "职位名称", "职位名称"),
            // entity.employeeExperience.positionname
            new TranslationSeedItem("entity.employeeExperience.positionname", "zh-HK", "职位名称", "职位名称"),

            // entity.employeeExperience.jobcontent
            new TranslationSeedItem("entity.employeeExperience.jobcontent", "en-US", "工作内容", "工作内容"),
            // entity.employeeExperience.jobcontent
            new TranslationSeedItem("entity.employeeExperience.jobcontent", "ja-JP", "工作内容", "工作内容"),
            // entity.employeeExperience.jobcontent
            new TranslationSeedItem("entity.employeeExperience.jobcontent", "zh-CN", "工作内容", "工作内容"),
            // entity.employeeExperience.jobcontent
            new TranslationSeedItem("entity.employeeExperience.jobcontent", "zh-HK", "工作内容", "工作内容"),

            // entity.employeeExperience.startdate
            new TranslationSeedItem("entity.employeeExperience.startdate", "en-US", "开始日期", "开始日期"),
            // entity.employeeExperience.startdate
            new TranslationSeedItem("entity.employeeExperience.startdate", "ja-JP", "开始日期", "开始日期"),
            // entity.employeeExperience.startdate
            new TranslationSeedItem("entity.employeeExperience.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.employeeExperience.startdate
            new TranslationSeedItem("entity.employeeExperience.startdate", "zh-HK", "开始日期", "开始日期"),

            // entity.employeeExperience.enddate
            new TranslationSeedItem("entity.employeeExperience.enddate", "en-US", "结束日期", "结束日期"),
            // entity.employeeExperience.enddate
            new TranslationSeedItem("entity.employeeExperience.enddate", "ja-JP", "结束日期", "结束日期"),
            // entity.employeeExperience.enddate
            new TranslationSeedItem("entity.employeeExperience.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.employeeExperience.enddate
            new TranslationSeedItem("entity.employeeExperience.enddate", "zh-HK", "结束日期", "结束日期"),

            // entity.employeeExperience.witnessname
            new TranslationSeedItem("entity.employeeExperience.witnessname", "en-US", "证明人姓名", "证明人姓名"),
            // entity.employeeExperience.witnessname
            new TranslationSeedItem("entity.employeeExperience.witnessname", "ja-JP", "证明人姓名", "证明人姓名"),
            // entity.employeeExperience.witnessname
            new TranslationSeedItem("entity.employeeExperience.witnessname", "zh-CN", "证明人姓名", "证明人姓名"),
            // entity.employeeExperience.witnessname
            new TranslationSeedItem("entity.employeeExperience.witnessname", "zh-HK", "证明人姓名", "证明人姓名"),

            // entity.employeeExperience.witnessphone
            new TranslationSeedItem("entity.employeeExperience.witnessphone", "en-US", "证明人电话", "证明人电话"),
            // entity.employeeExperience.witnessphone
            new TranslationSeedItem("entity.employeeExperience.witnessphone", "ja-JP", "证明人电话", "证明人电话"),
            // entity.employeeExperience.witnessphone
            new TranslationSeedItem("entity.employeeExperience.witnessphone", "zh-CN", "证明人电话", "证明人电话"),
            // entity.employeeExperience.witnessphone
            new TranslationSeedItem("entity.employeeExperience.witnessphone", "zh-HK", "证明人电话", "证明人电话"),
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
