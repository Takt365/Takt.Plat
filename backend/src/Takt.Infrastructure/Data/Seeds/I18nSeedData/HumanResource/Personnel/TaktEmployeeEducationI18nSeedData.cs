// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeEducation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeEducation 实体国际化翻译种子（键前缀 entity.employeeEducation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeEducationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeEducation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeEducation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeEducationTranslations())
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

        TaktLogger.Information("TaktEmployeeEducation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeEducation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeEducation._self / entity.employeeEducation.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeEducationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeEducation._self
            new TranslationSeedItem("entity.employeeEducation._self", "en-US", "Employee Education Information", "实体名称"),
            // entity.employeeEducation._self
            new TranslationSeedItem("entity.employeeEducation._self", "ja-JP", "员工教育经历信息", "实体名称"),
            // entity.employeeEducation._self
            new TranslationSeedItem("entity.employeeEducation._self", "zh-CN", "员工教育经历信息", "实体名称"),
            // entity.employeeEducation._self
            new TranslationSeedItem("entity.employeeEducation._self", "zh-HK", "员工教育经历信息", "实体名称"),

            // entity.employeeEducation.employeeid
            new TranslationSeedItem("entity.employeeEducation.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeEducation.employeeid
            new TranslationSeedItem("entity.employeeEducation.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeEducation.employeeid
            new TranslationSeedItem("entity.employeeEducation.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeEducation.employeeid
            new TranslationSeedItem("entity.employeeEducation.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeEducation.schoolname
            new TranslationSeedItem("entity.employeeEducation.schoolname", "en-US", "学校名称", "学校名称"),
            // entity.employeeEducation.schoolname
            new TranslationSeedItem("entity.employeeEducation.schoolname", "ja-JP", "学校名称", "学校名称"),
            // entity.employeeEducation.schoolname
            new TranslationSeedItem("entity.employeeEducation.schoolname", "zh-CN", "学校名称", "学校名称"),
            // entity.employeeEducation.schoolname
            new TranslationSeedItem("entity.employeeEducation.schoolname", "zh-HK", "学校名称", "学校名称"),

            // entity.employeeEducation.educationlevel
            new TranslationSeedItem("entity.employeeEducation.educationlevel", "en-US", "学历层次", "学历层次（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.employeeEducation.educationlevel
            new TranslationSeedItem("entity.employeeEducation.educationlevel", "ja-JP", "学历层次", "学历层次（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.employeeEducation.educationlevel
            new TranslationSeedItem("entity.employeeEducation.educationlevel", "zh-CN", "学历层次", "学历层次（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.employeeEducation.educationlevel
            new TranslationSeedItem("entity.employeeEducation.educationlevel", "zh-HK", "学历层次", "学历层次（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),

            // entity.employeeEducation.degreelevel
            new TranslationSeedItem("entity.employeeEducation.degreelevel", "en-US", "学位层次", "学位层次（0=无，1=学士，2=硕士，3=博士）"),
            // entity.employeeEducation.degreelevel
            new TranslationSeedItem("entity.employeeEducation.degreelevel", "ja-JP", "学位层次", "学位层次（0=无，1=学士，2=硕士，3=博士）"),
            // entity.employeeEducation.degreelevel
            new TranslationSeedItem("entity.employeeEducation.degreelevel", "zh-CN", "学位层次", "学位层次（0=无，1=学士，2=硕士，3=博士）"),
            // entity.employeeEducation.degreelevel
            new TranslationSeedItem("entity.employeeEducation.degreelevel", "zh-HK", "学位层次", "学位层次（0=无，1=学士，2=硕士，3=博士）"),

            // entity.employeeEducation.majorname
            new TranslationSeedItem("entity.employeeEducation.majorname", "en-US", "专业名称", "专业名称"),
            // entity.employeeEducation.majorname
            new TranslationSeedItem("entity.employeeEducation.majorname", "ja-JP", "专业名称", "专业名称"),
            // entity.employeeEducation.majorname
            new TranslationSeedItem("entity.employeeEducation.majorname", "zh-CN", "专业名称", "专业名称"),
            // entity.employeeEducation.majorname
            new TranslationSeedItem("entity.employeeEducation.majorname", "zh-HK", "专业名称", "专业名称"),

            // entity.employeeEducation.certificateno
            new TranslationSeedItem("entity.employeeEducation.certificateno", "en-US", "证书编号", "证书编号"),
            // entity.employeeEducation.certificateno
            new TranslationSeedItem("entity.employeeEducation.certificateno", "ja-JP", "证书编号", "证书编号"),
            // entity.employeeEducation.certificateno
            new TranslationSeedItem("entity.employeeEducation.certificateno", "zh-CN", "证书编号", "证书编号"),
            // entity.employeeEducation.certificateno
            new TranslationSeedItem("entity.employeeEducation.certificateno", "zh-HK", "证书编号", "证书编号"),

            // entity.employeeEducation.startdate
            new TranslationSeedItem("entity.employeeEducation.startdate", "en-US", "开始日期", "开始日期"),
            // entity.employeeEducation.startdate
            new TranslationSeedItem("entity.employeeEducation.startdate", "ja-JP", "开始日期", "开始日期"),
            // entity.employeeEducation.startdate
            new TranslationSeedItem("entity.employeeEducation.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.employeeEducation.startdate
            new TranslationSeedItem("entity.employeeEducation.startdate", "zh-HK", "开始日期", "开始日期"),

            // entity.employeeEducation.enddate
            new TranslationSeedItem("entity.employeeEducation.enddate", "en-US", "结束日期", "结束日期"),
            // entity.employeeEducation.enddate
            new TranslationSeedItem("entity.employeeEducation.enddate", "ja-JP", "结束日期", "结束日期"),
            // entity.employeeEducation.enddate
            new TranslationSeedItem("entity.employeeEducation.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.employeeEducation.enddate
            new TranslationSeedItem("entity.employeeEducation.enddate", "zh-HK", "结束日期", "结束日期"),
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
