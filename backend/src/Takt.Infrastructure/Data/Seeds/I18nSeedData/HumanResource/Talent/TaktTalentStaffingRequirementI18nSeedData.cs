// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentStaffingRequirement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTalentStaffingRequirement 实体国际化翻译种子（键前缀 entity.talentStaffingRequirement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentStaffingRequirementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentStaffingRequirement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentStaffingRequirement 实体翻译...", tenantCode);

        foreach (var item in GetTalentStaffingRequirementTranslations())
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

        TaktLogger.Information("TaktTalentStaffingRequirement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentStaffingRequirement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentStaffingRequirement._self / entity.talentStaffingRequirement.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentStaffingRequirementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentStaffingRequirement._self
            new TranslationSeedItem("entity.talentStaffingRequirement._self", "en-US", "Talent Staffing Requirement Information", "实体名称"),
            // entity.talentStaffingRequirement._self
            new TranslationSeedItem("entity.talentStaffingRequirement._self", "ja-JP", "用人需求信息", "实体名称"),
            // entity.talentStaffingRequirement._self
            new TranslationSeedItem("entity.talentStaffingRequirement._self", "zh-CN", "用人需求信息", "实体名称"),
            // entity.talentStaffingRequirement._self
            new TranslationSeedItem("entity.talentStaffingRequirement._self", "zh-HK", "用人需求信息", "实体名称"),

            // entity.talentStaffingRequirement.reqno
            new TranslationSeedItem("entity.talentStaffingRequirement.reqno", "en-US", "需求单号", "需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）"),
            // entity.talentStaffingRequirement.reqno
            new TranslationSeedItem("entity.talentStaffingRequirement.reqno", "ja-JP", "需求单号", "需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）"),
            // entity.talentStaffingRequirement.reqno
            new TranslationSeedItem("entity.talentStaffingRequirement.reqno", "zh-CN", "需求单号", "需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）"),
            // entity.talentStaffingRequirement.reqno
            new TranslationSeedItem("entity.talentStaffingRequirement.reqno", "zh-HK", "需求单号", "需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）"),

            // entity.talentStaffingRequirement.deptid
            new TranslationSeedItem("entity.talentStaffingRequirement.deptid", "en-US", "申请部门ID", "申请部门ID（DeptID，FK→TaktDept）"),
            // entity.talentStaffingRequirement.deptid
            new TranslationSeedItem("entity.talentStaffingRequirement.deptid", "ja-JP", "申请部门ID", "申请部门ID（DeptID，FK→TaktDept）"),
            // entity.talentStaffingRequirement.deptid
            new TranslationSeedItem("entity.talentStaffingRequirement.deptid", "zh-CN", "申请部门ID", "申请部门ID（DeptID，FK→TaktDept）"),
            // entity.talentStaffingRequirement.deptid
            new TranslationSeedItem("entity.talentStaffingRequirement.deptid", "zh-HK", "申请部门ID", "申请部门ID（DeptID，FK→TaktDept）"),

            // entity.talentStaffingRequirement.postid
            new TranslationSeedItem("entity.talentStaffingRequirement.postid", "en-US", "申请岗位ID", "申请岗位ID（PositionID，FK→TaktPost）"),
            // entity.talentStaffingRequirement.postid
            new TranslationSeedItem("entity.talentStaffingRequirement.postid", "ja-JP", "申请岗位ID", "申请岗位ID（PositionID，FK→TaktPost）"),
            // entity.talentStaffingRequirement.postid
            new TranslationSeedItem("entity.talentStaffingRequirement.postid", "zh-CN", "申请岗位ID", "申请岗位ID（PositionID，FK→TaktPost）"),
            // entity.talentStaffingRequirement.postid
            new TranslationSeedItem("entity.talentStaffingRequirement.postid", "zh-HK", "申请岗位ID", "申请岗位ID（PositionID，FK→TaktPost）"),

            // entity.talentStaffingRequirement.jobgrade
            new TranslationSeedItem("entity.talentStaffingRequirement.jobgrade", "en-US", "职级", "职级（JobGrade/Rank，如专员/主任/工程师）"),
            // entity.talentStaffingRequirement.jobgrade
            new TranslationSeedItem("entity.talentStaffingRequirement.jobgrade", "ja-JP", "职级", "职级（JobGrade/Rank，如专员/主任/工程师）"),
            // entity.talentStaffingRequirement.jobgrade
            new TranslationSeedItem("entity.talentStaffingRequirement.jobgrade", "zh-CN", "职级", "职级（JobGrade/Rank，如专员/主任/工程师）"),
            // entity.talentStaffingRequirement.jobgrade
            new TranslationSeedItem("entity.talentStaffingRequirement.jobgrade", "zh-HK", "职级", "职级（JobGrade/Rank，如专员/主任/工程师）"),

            // entity.talentStaffingRequirement.requestqty
            new TranslationSeedItem("entity.talentStaffingRequirement.requestqty", "en-US", "需求人数", "需求人数（RequestQty，默认 1）"),
            // entity.talentStaffingRequirement.requestqty
            new TranslationSeedItem("entity.talentStaffingRequirement.requestqty", "ja-JP", "需求人数", "需求人数（RequestQty，默认 1）"),
            // entity.talentStaffingRequirement.requestqty
            new TranslationSeedItem("entity.talentStaffingRequirement.requestqty", "zh-CN", "需求人数", "需求人数（RequestQty，默认 1）"),
            // entity.talentStaffingRequirement.requestqty
            new TranslationSeedItem("entity.talentStaffingRequirement.requestqty", "zh-HK", "需求人数", "需求人数（RequestQty，默认 1）"),

            // entity.talentStaffingRequirement.headcounttype
            new TranslationSeedItem("entity.talentStaffingRequirement.headcounttype", "en-US", "编制类型", "编制类型（HeadcountType：正式/派遣/实习生/临时）"),
            // entity.talentStaffingRequirement.headcounttype
            new TranslationSeedItem("entity.talentStaffingRequirement.headcounttype", "ja-JP", "编制类型", "编制类型（HeadcountType：正式/派遣/实习生/临时）"),
            // entity.talentStaffingRequirement.headcounttype
            new TranslationSeedItem("entity.talentStaffingRequirement.headcounttype", "zh-CN", "编制类型", "编制类型（HeadcountType：正式/派遣/实习生/临时）"),
            // entity.talentStaffingRequirement.headcounttype
            new TranslationSeedItem("entity.talentStaffingRequirement.headcounttype", "zh-HK", "编制类型", "编制类型（HeadcountType：正式/派遣/实习生/临时）"),

            // entity.talentStaffingRequirement.reasoncode
            new TranslationSeedItem("entity.talentStaffingRequirement.reasoncode", "en-US", "需求原因", "需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）"),
            // entity.talentStaffingRequirement.reasoncode
            new TranslationSeedItem("entity.talentStaffingRequirement.reasoncode", "ja-JP", "需求原因", "需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）"),
            // entity.talentStaffingRequirement.reasoncode
            new TranslationSeedItem("entity.talentStaffingRequirement.reasoncode", "zh-CN", "需求原因", "需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）"),
            // entity.talentStaffingRequirement.reasoncode
            new TranslationSeedItem("entity.talentStaffingRequirement.reasoncode", "zh-HK", "需求原因", "需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）"),

            // entity.talentStaffingRequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentStaffingRequirement.replaceemployeeid", "en-US", "替补员工ID", "替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）"),
            // entity.talentStaffingRequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentStaffingRequirement.replaceemployeeid", "ja-JP", "替补员工ID", "替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）"),
            // entity.talentStaffingRequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentStaffingRequirement.replaceemployeeid", "zh-CN", "替补员工ID", "替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）"),
            // entity.talentStaffingRequirement.replaceemployeeid
            new TranslationSeedItem("entity.talentStaffingRequirement.replaceemployeeid", "zh-HK", "替补员工ID", "替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）"),

            // entity.talentStaffingRequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentStaffingRequirement.expectedonboarddate", "en-US", "期望入职日", "期望入职日（ExpectedOnboardDate）"),
            // entity.talentStaffingRequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentStaffingRequirement.expectedonboarddate", "ja-JP", "期望入职日", "期望入职日（ExpectedOnboardDate）"),
            // entity.talentStaffingRequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentStaffingRequirement.expectedonboarddate", "zh-CN", "期望入职日", "期望入职日（ExpectedOnboardDate）"),
            // entity.talentStaffingRequirement.expectedonboarddate
            new TranslationSeedItem("entity.talentStaffingRequirement.expectedonboarddate", "zh-HK", "期望入职日", "期望入职日（ExpectedOnboardDate）"),

            // entity.talentStaffingRequirement.contracttype
            new TranslationSeedItem("entity.talentStaffingRequirement.contracttype", "en-US", "合同类型", "合同类型（ContractType：固定期/无固定/实习协议）"),
            // entity.talentStaffingRequirement.contracttype
            new TranslationSeedItem("entity.talentStaffingRequirement.contracttype", "ja-JP", "合同类型", "合同类型（ContractType：固定期/无固定/实习协议）"),
            // entity.talentStaffingRequirement.contracttype
            new TranslationSeedItem("entity.talentStaffingRequirement.contracttype", "zh-CN", "合同类型", "合同类型（ContractType：固定期/无固定/实习协议）"),
            // entity.talentStaffingRequirement.contracttype
            new TranslationSeedItem("entity.talentStaffingRequirement.contracttype", "zh-HK", "合同类型", "合同类型（ContractType：固定期/无固定/实习协议）"),

            // entity.talentStaffingRequirement.worklocation
            new TranslationSeedItem("entity.talentStaffingRequirement.worklocation", "en-US", "工作地点", "工作地点（WorkLocation，如工厂/分公司）"),
            // entity.talentStaffingRequirement.worklocation
            new TranslationSeedItem("entity.talentStaffingRequirement.worklocation", "ja-JP", "工作地点", "工作地点（WorkLocation，如工厂/分公司）"),
            // entity.talentStaffingRequirement.worklocation
            new TranslationSeedItem("entity.talentStaffingRequirement.worklocation", "zh-CN", "工作地点", "工作地点（WorkLocation，如工厂/分公司）"),
            // entity.talentStaffingRequirement.worklocation
            new TranslationSeedItem("entity.talentStaffingRequirement.worklocation", "zh-HK", "工作地点", "工作地点（WorkLocation，如工厂/分公司）"),

            // entity.talentStaffingRequirement.jobdesc
            new TranslationSeedItem("entity.talentStaffingRequirement.jobdesc", "en-US", "岗位职责", "岗位职责（JobDesc）"),
            // entity.talentStaffingRequirement.jobdesc
            new TranslationSeedItem("entity.talentStaffingRequirement.jobdesc", "ja-JP", "岗位职责", "岗位职责（JobDesc）"),
            // entity.talentStaffingRequirement.jobdesc
            new TranslationSeedItem("entity.talentStaffingRequirement.jobdesc", "zh-CN", "岗位职责", "岗位职责（JobDesc）"),
            // entity.talentStaffingRequirement.jobdesc
            new TranslationSeedItem("entity.talentStaffingRequirement.jobdesc", "zh-HK", "岗位职责", "岗位职责（JobDesc）"),

            // entity.talentStaffingRequirement.qualification
            new TranslationSeedItem("entity.talentStaffingRequirement.qualification", "en-US", "任职要求", "任职要求（Qualification，学历/经验/技能）"),
            // entity.talentStaffingRequirement.qualification
            new TranslationSeedItem("entity.talentStaffingRequirement.qualification", "ja-JP", "任职要求", "任职要求（Qualification，学历/经验/技能）"),
            // entity.talentStaffingRequirement.qualification
            new TranslationSeedItem("entity.talentStaffingRequirement.qualification", "zh-CN", "任职要求", "任职要求（Qualification，学历/经验/技能）"),
            // entity.talentStaffingRequirement.qualification
            new TranslationSeedItem("entity.talentStaffingRequirement.qualification", "zh-HK", "任职要求", "任职要求（Qualification，学历/经验/技能）"),

            // entity.talentStaffingRequirement.budgetyear
            new TranslationSeedItem("entity.talentStaffingRequirement.budgetyear", "en-US", "预算年度", "预算年度（BudgetYear，用于 headcount 控制）"),
            // entity.talentStaffingRequirement.budgetyear
            new TranslationSeedItem("entity.talentStaffingRequirement.budgetyear", "ja-JP", "预算年度", "预算年度（BudgetYear，用于 headcount 控制）"),
            // entity.talentStaffingRequirement.budgetyear
            new TranslationSeedItem("entity.talentStaffingRequirement.budgetyear", "zh-CN", "预算年度", "预算年度（BudgetYear，用于 headcount 控制）"),
            // entity.talentStaffingRequirement.budgetyear
            new TranslationSeedItem("entity.talentStaffingRequirement.budgetyear", "zh-HK", "预算年度", "预算年度（BudgetYear，用于 headcount 控制）"),
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
