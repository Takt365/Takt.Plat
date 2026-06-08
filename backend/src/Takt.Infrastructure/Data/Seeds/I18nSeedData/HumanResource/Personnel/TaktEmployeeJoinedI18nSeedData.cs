// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeJoinedI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeJoined 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeJoined 实体国际化翻译种子（键前缀 entity.employeeJoined.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeJoinedI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeJoined 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeJoined 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeJoinedTranslations())
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

        TaktLogger.Information("TaktEmployeeJoined 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeJoined 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeJoined._self / entity.employeeJoined.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeJoinedTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeJoined._self
            new TranslationSeedItem("entity.employeeJoined._self", "en-US", "Employee Joined Information", "实体名称"),
            // entity.employeeJoined._self
            new TranslationSeedItem("entity.employeeJoined._self", "ja-JP", "员工入职上岗办理记录信息", "实体名称"),
            // entity.employeeJoined._self
            new TranslationSeedItem("entity.employeeJoined._self", "zh-CN", "员工入职上岗办理记录信息", "实体名称"),
            // entity.employeeJoined._self
            new TranslationSeedItem("entity.employeeJoined._self", "zh-HK", "员工入职上岗办理记录信息", "实体名称"),

            // entity.employeeJoined.employeeid
            new TranslationSeedItem("entity.employeeJoined.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeJoined.employeeid
            new TranslationSeedItem("entity.employeeJoined.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeJoined.employeeid
            new TranslationSeedItem("entity.employeeJoined.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeJoined.employeeid
            new TranslationSeedItem("entity.employeeJoined.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeJoined.onboardingid
            new TranslationSeedItem("entity.employeeJoined.onboardingid", "en-US", "入职待办ID", "入职待办ID（由入职待办办结后生成上岗单时回填，可空）"),
            // entity.employeeJoined.onboardingid
            new TranslationSeedItem("entity.employeeJoined.onboardingid", "ja-JP", "入职待办ID", "入职待办ID（由入职待办办结后生成上岗单时回填，可空）"),
            // entity.employeeJoined.onboardingid
            new TranslationSeedItem("entity.employeeJoined.onboardingid", "zh-CN", "入职待办ID", "入职待办ID（由入职待办办结后生成上岗单时回填，可空）"),
            // entity.employeeJoined.onboardingid
            new TranslationSeedItem("entity.employeeJoined.onboardingid", "zh-HK", "入职待办ID", "入职待办ID（由入职待办办结后生成上岗单时回填，可空）"),

            // entity.employeeJoined.joineddate
            new TranslationSeedItem("entity.employeeJoined.joineddate", "en-US", "实际上岗日期", "实际上岗日期（JoinedDate：我去上班）"),
            // entity.employeeJoined.joineddate
            new TranslationSeedItem("entity.employeeJoined.joineddate", "ja-JP", "实际上岗日期", "实际上岗日期（JoinedDate：我去上班）"),
            // entity.employeeJoined.joineddate
            new TranslationSeedItem("entity.employeeJoined.joineddate", "zh-CN", "实际上岗日期", "实际上岗日期（JoinedDate：我去上班）"),
            // entity.employeeJoined.joineddate
            new TranslationSeedItem("entity.employeeJoined.joineddate", "zh-HK", "实际上岗日期", "实际上岗日期（JoinedDate：我去上班）"),

            // entity.employeeJoined.probationenddate
            new TranslationSeedItem("entity.employeeJoined.probationenddate", "en-US", "试用期结束日期", "试用期结束日期"),
            // entity.employeeJoined.probationenddate
            new TranslationSeedItem("entity.employeeJoined.probationenddate", "ja-JP", "试用期结束日期", "试用期结束日期"),
            // entity.employeeJoined.probationenddate
            new TranslationSeedItem("entity.employeeJoined.probationenddate", "zh-CN", "试用期结束日期", "试用期结束日期"),
            // entity.employeeJoined.probationenddate
            new TranslationSeedItem("entity.employeeJoined.probationenddate", "zh-HK", "试用期结束日期", "试用期结束日期"),

            // entity.employeeJoined.regulardate
            new TranslationSeedItem("entity.employeeJoined.regulardate", "en-US", "转正日期", "转正日期"),
            // entity.employeeJoined.regulardate
            new TranslationSeedItem("entity.employeeJoined.regulardate", "ja-JP", "转正日期", "转正日期"),
            // entity.employeeJoined.regulardate
            new TranslationSeedItem("entity.employeeJoined.regulardate", "zh-CN", "转正日期", "转正日期"),
            // entity.employeeJoined.regulardate
            new TranslationSeedItem("entity.employeeJoined.regulardate", "zh-HK", "转正日期", "转正日期"),

            // entity.employeeJoined.deptid
            new TranslationSeedItem("entity.employeeJoined.deptid", "en-US", "上岗部门ID", "上岗部门ID"),
            // entity.employeeJoined.deptid
            new TranslationSeedItem("entity.employeeJoined.deptid", "ja-JP", "上岗部门ID", "上岗部门ID"),
            // entity.employeeJoined.deptid
            new TranslationSeedItem("entity.employeeJoined.deptid", "zh-CN", "上岗部门ID", "上岗部门ID"),
            // entity.employeeJoined.deptid
            new TranslationSeedItem("entity.employeeJoined.deptid", "zh-HK", "上岗部门ID", "上岗部门ID"),

            // entity.employeeJoined.deptname
            new TranslationSeedItem("entity.employeeJoined.deptname", "en-US", "上岗部门名称", "上岗部门名称"),
            // entity.employeeJoined.deptname
            new TranslationSeedItem("entity.employeeJoined.deptname", "ja-JP", "上岗部门名称", "上岗部门名称"),
            // entity.employeeJoined.deptname
            new TranslationSeedItem("entity.employeeJoined.deptname", "zh-CN", "上岗部门名称", "上岗部门名称"),
            // entity.employeeJoined.deptname
            new TranslationSeedItem("entity.employeeJoined.deptname", "zh-HK", "上岗部门名称", "上岗部门名称"),

            // entity.employeeJoined.postid
            new TranslationSeedItem("entity.employeeJoined.postid", "en-US", "上岗岗位ID", "上岗岗位ID"),
            // entity.employeeJoined.postid
            new TranslationSeedItem("entity.employeeJoined.postid", "ja-JP", "上岗岗位ID", "上岗岗位ID"),
            // entity.employeeJoined.postid
            new TranslationSeedItem("entity.employeeJoined.postid", "zh-CN", "上岗岗位ID", "上岗岗位ID"),
            // entity.employeeJoined.postid
            new TranslationSeedItem("entity.employeeJoined.postid", "zh-HK", "上岗岗位ID", "上岗岗位ID"),

            // entity.employeeJoined.postname
            new TranslationSeedItem("entity.employeeJoined.postname", "en-US", "上岗岗位名称", "上岗岗位名称"),
            // entity.employeeJoined.postname
            new TranslationSeedItem("entity.employeeJoined.postname", "ja-JP", "上岗岗位名称", "上岗岗位名称"),
            // entity.employeeJoined.postname
            new TranslationSeedItem("entity.employeeJoined.postname", "zh-CN", "上岗岗位名称", "上岗岗位名称"),
            // entity.employeeJoined.postname
            new TranslationSeedItem("entity.employeeJoined.postname", "zh-HK", "上岗岗位名称", "上岗岗位名称"),

            // entity.employeeJoined.jobtitle
            new TranslationSeedItem("entity.employeeJoined.jobtitle", "en-US", "职务", "职务/职称"),
            // entity.employeeJoined.jobtitle
            new TranslationSeedItem("entity.employeeJoined.jobtitle", "ja-JP", "职务", "职务/职称"),
            // entity.employeeJoined.jobtitle
            new TranslationSeedItem("entity.employeeJoined.jobtitle", "zh-CN", "职务", "职务/职称"),
            // entity.employeeJoined.jobtitle
            new TranslationSeedItem("entity.employeeJoined.jobtitle", "zh-HK", "职务", "职务/职称"),

            // entity.employeeJoined.worknature
            new TranslationSeedItem("entity.employeeJoined.worknature", "en-US", "工作性质", "工作性质（0=全职，1=兼职，2=实习，3=外包，4=其他）"),
            // entity.employeeJoined.worknature
            new TranslationSeedItem("entity.employeeJoined.worknature", "ja-JP", "工作性质", "工作性质（0=全职，1=兼职，2=实习，3=外包，4=其他）"),
            // entity.employeeJoined.worknature
            new TranslationSeedItem("entity.employeeJoined.worknature", "zh-CN", "工作性质", "工作性质（0=全职，1=兼职，2=实习，3=外包，4=其他）"),
            // entity.employeeJoined.worknature
            new TranslationSeedItem("entity.employeeJoined.worknature", "zh-HK", "工作性质", "工作性质（0=全职，1=兼职，2=实习，3=外包，4=其他）"),

            // entity.employeeJoined.employmenttype
            new TranslationSeedItem("entity.employeeJoined.employmenttype", "en-US", "任职类型", "任职类型（0=主职，1=兼职，2=借调，3=挂职）"),
            // entity.employeeJoined.employmenttype
            new TranslationSeedItem("entity.employeeJoined.employmenttype", "ja-JP", "任职类型", "任职类型（0=主职，1=兼职，2=借调，3=挂职）"),
            // entity.employeeJoined.employmenttype
            new TranslationSeedItem("entity.employeeJoined.employmenttype", "zh-CN", "任职类型", "任职类型（0=主职，1=兼职，2=借调，3=挂职）"),
            // entity.employeeJoined.employmenttype
            new TranslationSeedItem("entity.employeeJoined.employmenttype", "zh-HK", "任职类型", "任职类型（0=主职，1=兼职，2=借调，3=挂职）"),

            // entity.employeeJoined.directmanagerid
            new TranslationSeedItem("entity.employeeJoined.directmanagerid", "en-US", "直属上级员工ID", "直属上级员工ID"),
            // entity.employeeJoined.directmanagerid
            new TranslationSeedItem("entity.employeeJoined.directmanagerid", "ja-JP", "直属上级员工ID", "直属上级员工ID"),
            // entity.employeeJoined.directmanagerid
            new TranslationSeedItem("entity.employeeJoined.directmanagerid", "zh-CN", "直属上级员工ID", "直属上级员工ID"),
            // entity.employeeJoined.directmanagerid
            new TranslationSeedItem("entity.employeeJoined.directmanagerid", "zh-HK", "直属上级员工ID", "直属上级员工ID"),

            // entity.employeeJoined.directmanagername
            new TranslationSeedItem("entity.employeeJoined.directmanagername", "en-US", "直属上级姓名", "直属上级姓名"),
            // entity.employeeJoined.directmanagername
            new TranslationSeedItem("entity.employeeJoined.directmanagername", "ja-JP", "直属上级姓名", "直属上级姓名"),
            // entity.employeeJoined.directmanagername
            new TranslationSeedItem("entity.employeeJoined.directmanagername", "zh-CN", "直属上级姓名", "直属上级姓名"),
            // entity.employeeJoined.directmanagername
            new TranslationSeedItem("entity.employeeJoined.directmanagername", "zh-HK", "直属上级姓名", "直属上级姓名"),
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
