// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployee 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployee 实体国际化翻译种子（键前缀 entity.employee.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployee 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employee 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeTranslations())
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

        TaktLogger.Information("TaktEmployee 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployee 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employee._self / entity.employee.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "en-US", "Employee Information", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "ja-JP", "员工信息", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-CN", "员工信息", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-HK", "员工信息", "实体名称"),

            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "en-US", "员工编号", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "ja-JP", "员工编号", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "zh-CN", "员工编号", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "zh-HK", "员工编号", "员工编号（租户+公司内唯一）"),

            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "en-US", "姓名", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "ja-JP", "姓名", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-CN", "姓名", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-HK", "姓名", "姓名"),

            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "en-US", "性别", "性别（0=未知，1=男，2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "ja-JP", "性别", "性别（0=未知，1=男，2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-CN", "性别", "性别（0=未知，1=男，2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-HK", "性别", "性别（0=未知，1=男，2=女）"),

            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "en-US", "出生日期", "出生日期"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "ja-JP", "出生日期", "出生日期"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-CN", "出生日期", "出生日期"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-HK", "出生日期", "出生日期"),

            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "en-US", "身份证号", "身份证号"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "ja-JP", "身份证号", "身份证号"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "zh-CN", "身份证号", "身份证号"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "zh-HK", "身份证号", "身份证号"),

            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "en-US", "手机号码", "手机号码"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "ja-JP", "手机号码", "手机号码"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-CN", "手机号码", "手机号码"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-HK", "手机号码", "手机号码"),

            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "en-US", "电子邮箱", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "ja-JP", "电子邮箱", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-CN", "电子邮箱", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-HK", "电子邮箱", "电子邮箱"),

            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "en-US", "籍贯", "籍贯（字典 hr_native_place 编码或文本）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "ja-JP", "籍贯", "籍贯（字典 hr_native_place 编码或文本）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-CN", "籍贯", "籍贯（字典 hr_native_place 编码或文本）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-HK", "籍贯", "籍贯（字典 hr_native_place 编码或文本）"),

            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "en-US", "民族", "民族（字典 hr_ethnic_group 编码或文本）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "ja-JP", "民族", "民族（字典 hr_ethnic_group 编码或文本）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-CN", "民族", "民族（字典 hr_ethnic_group 编码或文本）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-HK", "民族", "民族（字典 hr_ethnic_group 编码或文本）"),

            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "en-US", "政治面貌", "政治面貌（字典 hr_political_status 编码或文本）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "ja-JP", "政治面貌", "政治面貌（字典 hr_political_status 编码或文本）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "zh-CN", "政治面貌", "政治面貌（字典 hr_political_status 编码或文本）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "zh-HK", "政治面貌", "政治面貌（字典 hr_political_status 编码或文本）"),

            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "en-US", "婚姻状况", "婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "ja-JP", "婚姻状况", "婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-CN", "婚姻状况", "婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-HK", "婚姻状况", "婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）"),

            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "en-US", "最高学历", "最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "ja-JP", "最高学历", "最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "zh-CN", "最高学历", "最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "zh-HK", "最高学历", "最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）"),

            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "en-US", "毕业院校", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "ja-JP", "毕业院校", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "zh-CN", "毕业院校", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "zh-HK", "毕业院校", "毕业院校（最高学历摘要）"),

            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "en-US", "专业", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "ja-JP", "专业", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "zh-CN", "专业", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "zh-HK", "专业", "专业（最高学历摘要）"),

            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "en-US", "实际上岗日期", "实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "ja-JP", "实际上岗日期", "实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "zh-CN", "实际上岗日期", "实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "zh-HK", "实际上岗日期", "实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）"),

            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "en-US", "试用期结束日期", "试用期结束日期"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "ja-JP", "试用期结束日期", "试用期结束日期"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "zh-CN", "试用期结束日期", "试用期结束日期"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "zh-HK", "试用期结束日期", "试用期结束日期"),

            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "en-US", "转正日期", "转正日期"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "ja-JP", "转正日期", "转正日期"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "zh-CN", "转正日期", "转正日期"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "zh-HK", "转正日期", "转正日期"),

            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "en-US", "离职日期", "离职日期"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "ja-JP", "离职日期", "离职日期"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "zh-CN", "离职日期", "离职日期"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "zh-HK", "离职日期", "离职日期"),

            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "en-US", "最后工作日", "最后工作日"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "ja-JP", "最后工作日", "最后工作日"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "zh-CN", "最后工作日", "最后工作日"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "zh-HK", "最后工作日", "最后工作日"),

            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "en-US", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "ja-JP", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "zh-CN", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "zh-HK", "离职类型", "离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）"),

            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "en-US", "离职原因", "离职原因"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "ja-JP", "离职原因", "离职原因"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "zh-CN", "离职原因", "离职原因"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "zh-HK", "离职原因", "离职原因"),

            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "en-US", "员工状态", "员工状态（1=试用期，2=正式，3=离职，4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "ja-JP", "员工状态", "员工状态（1=试用期，2=正式，3=离职，4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-CN", "员工状态", "员工状态（1=试用期，2=正式，3=离职，4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-HK", "员工状态", "员工状态（1=试用期，2=正式，3=离职，4=退休）"),

            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "en-US", "当前主部门ID", "当前主部门ID（任职快照，与最新已生效上岗单同步）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "ja-JP", "当前主部门ID", "当前主部门ID（任职快照，与最新已生效上岗单同步）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "zh-CN", "当前主部门ID", "当前主部门ID（任职快照，与最新已生效上岗单同步）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "zh-HK", "当前主部门ID", "当前主部门ID（任职快照，与最新已生效上岗单同步）"),

            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "en-US", "当前主岗位ID", "当前主岗位ID（任职快照）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "ja-JP", "当前主岗位ID", "当前主岗位ID（任职快照）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "zh-CN", "当前主岗位ID", "当前主岗位ID（任职快照）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "zh-HK", "当前主岗位ID", "当前主岗位ID（任职快照）"),

            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "en-US", "紧急联系人姓名", "紧急联系人姓名"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "ja-JP", "紧急联系人姓名", "紧急联系人姓名"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "zh-CN", "紧急联系人姓名", "紧急联系人姓名"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "zh-HK", "紧急联系人姓名", "紧急联系人姓名"),

            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "en-US", "紧急联系人电话", "紧急联系人电话"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "ja-JP", "紧急联系人电话", "紧急联系人电话"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "zh-CN", "紧急联系人电话", "紧急联系人电话"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "zh-HK", "紧急联系人电话", "紧急联系人电话"),

            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "en-US", "家庭住址", "家庭住址"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "ja-JP", "家庭住址", "家庭住址"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "zh-CN", "家庭住址", "家庭住址"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "zh-HK", "家庭住址", "家庭住址"),

            // entity.employee.photourl
            new TranslationSeedItem("entity.employee.photourl", "en-US", "照片URL", "照片URL"),
            // entity.employee.photourl
            new TranslationSeedItem("entity.employee.photourl", "ja-JP", "照片URL", "照片URL"),
            // entity.employee.photourl
            new TranslationSeedItem("entity.employee.photourl", "zh-CN", "照片URL", "照片URL"),
            // entity.employee.photourl
            new TranslationSeedItem("entity.employee.photourl", "zh-HK", "照片URL", "照片URL"),

            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "en-US", "employeeDepts", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "ja-JP", "employeeDepts", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-CN", "employeeDepts", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-HK", "employeeDepts", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
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
