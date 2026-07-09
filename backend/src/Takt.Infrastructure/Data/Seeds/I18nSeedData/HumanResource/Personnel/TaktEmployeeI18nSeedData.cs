// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeI18nSeedData.cs
// 创建时间：2026-07-09
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
    /// I18nKey：entity.employee._self / entity.employee.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "en-US", "Employee Information_us", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "ja-JP", "员工信息_jp", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-CN", "员工信息", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-HK", "员工信息_hk", "实体名称"),

            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "en-US", "员工编号_us", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "ja-JP", "员工编号_jp", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "zh-CN", "员工编号", "员工编号（租户+公司内唯一）"),
            // entity.employee.no
            new TranslationSeedItem("entity.employee.no", "zh-HK", "员工编号_hk", "员工编号（租户+公司内唯一）"),

            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "en-US", "姓名_us", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "ja-JP", "姓名_jp", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-CN", "姓名", "姓名"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-HK", "姓名_hk", "姓名"),

            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "en-US", "性别_us", "性别（字典 sys_user_gender_category；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "ja-JP", "性别_jp", "性别（字典 sys_user_gender_category；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-CN", "性别", "性别（字典 sys_user_gender_category；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-HK", "性别_hk", "性别（字典 sys_user_gender_category；0=未知 1=男 2=女）"),

            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "en-US", "出生日期_us", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "ja-JP", "出生日期_jp", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-CN", "出生日期", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-HK", "出生日期_hk", "出生日期（人事档案必填）"),

            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "en-US", "身份证号_us", "身份证号（人事档案必填）"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "ja-JP", "身份证号_jp", "身份证号（人事档案必填）"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "zh-CN", "身份证号", "身份证号（人事档案必填）"),
            // entity.employee.idcardno
            new TranslationSeedItem("entity.employee.idcardno", "zh-HK", "身份证号_hk", "身份证号（人事档案必填）"),

            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "en-US", "手机号码_us", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "ja-JP", "手机号码_jp", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-CN", "手机号码", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-HK", "手机号码_hk", "手机号码（人事档案必填）"),

            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "en-US", "电子邮箱_us", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "ja-JP", "电子邮箱_jp", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-CN", "电子邮箱", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-HK", "电子邮箱_hk", "电子邮箱"),

            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "en-US", "籍贯_us", "籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "ja-JP", "籍贯_jp", "籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-CN", "籍贯", "籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-HK", "籍贯_hk", "籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填）"),

            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "en-US", "民族_us", "民族（字典 hr_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "ja-JP", "民族_jp", "民族（字典 hr_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-CN", "民族", "民族（字典 hr_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-HK", "民族_hk", "民族（字典 hr_ethnic_code；DictValue 1～56）"),

            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "en-US", "最高学历_us", "最高学历摘要（字典 hr_education_level_category；1=高中及以下 2=大专 3=本科 4=硕士 5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "ja-JP", "最高学历_jp", "最高学历摘要（字典 hr_education_level_category；1=高中及以下 2=大专 3=本科 4=硕士 5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "zh-CN", "最高学历", "最高学历摘要（字典 hr_education_level_category；1=高中及以下 2=大专 3=本科 4=硕士 5=博士；明细见 EmployeeEducations）"),
            // entity.employee.education
            new TranslationSeedItem("entity.employee.education", "zh-HK", "最高学历_hk", "最高学历摘要（字典 hr_education_level_category；1=高中及以下 2=大专 3=本科 4=硕士 5=博士；明细见 EmployeeEducations）"),

            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "en-US", "毕业院校_us", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "ja-JP", "毕业院校_jp", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "zh-CN", "毕业院校", "毕业院校（最高学历摘要）"),
            // entity.employee.graduateschool
            new TranslationSeedItem("entity.employee.graduateschool", "zh-HK", "毕业院校_hk", "毕业院校（最高学历摘要）"),

            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "en-US", "专业_us", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "ja-JP", "专业_jp", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "zh-CN", "专业", "专业（最高学历摘要）"),
            // entity.employee.major
            new TranslationSeedItem("entity.employee.major", "zh-HK", "专业_hk", "专业（最高学历摘要）"),

            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "en-US", "实际上岗日期_us", "实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "ja-JP", "实际上岗日期_jp", "实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "zh-CN", "实际上岗日期", "实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）"),
            // entity.employee.joineddate
            new TranslationSeedItem("entity.employee.joineddate", "zh-HK", "实际上岗日期_hk", "实际上岗日期（JoinedDate：入职上班；投影字段，由上岗审批通过后回写，未上岗可空）"),

            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "en-US", "试用期结束日期_us", "试用期结束日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "ja-JP", "试用期结束日期_jp", "试用期结束日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "zh-CN", "试用期结束日期", "试用期结束日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.probationenddate
            new TranslationSeedItem("entity.employee.probationenddate", "zh-HK", "试用期结束日期_hk", "试用期结束日期（投影字段，由上岗审批通过后回写）"),

            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "en-US", "转正日期_us", "转正日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "ja-JP", "转正日期_jp", "转正日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "zh-CN", "转正日期", "转正日期（投影字段，由上岗审批通过后回写）"),
            // entity.employee.regulardate
            new TranslationSeedItem("entity.employee.regulardate", "zh-HK", "转正日期_hk", "转正日期（投影字段，由上岗审批通过后回写）"),

            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "en-US", "离职日期_us", "离职日期（投影字段，由离职审批通过后回写）"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "ja-JP", "离职日期_jp", "离职日期（投影字段，由离职审批通过后回写）"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "zh-CN", "离职日期", "离职日期（投影字段，由离职审批通过后回写）"),
            // entity.employee.terminationdate
            new TranslationSeedItem("entity.employee.terminationdate", "zh-HK", "离职日期_hk", "离职日期（投影字段，由离职审批通过后回写）"),

            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "en-US", "最后工作日_us", "最后工作日（投影字段，由离职审批通过后回写）"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "ja-JP", "最后工作日_jp", "最后工作日（投影字段，由离职审批通过后回写）"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "zh-CN", "最后工作日", "最后工作日（投影字段，由离职审批通过后回写）"),
            // entity.employee.lastworkdate
            new TranslationSeedItem("entity.employee.lastworkdate", "zh-HK", "最后工作日_hk", "最后工作日（投影字段，由离职审批通过后回写）"),

            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "en-US", "离职类型_us", "离职类型（字典 hr_resignation_category；投影字段，由离职审批通过后回写；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "ja-JP", "离职类型_jp", "离职类型（字典 hr_resignation_category；投影字段，由离职审批通过后回写；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "zh-CN", "离职类型", "离职类型（字典 hr_resignation_category；投影字段，由离职审批通过后回写；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),
            // entity.employee.resignationtype
            new TranslationSeedItem("entity.employee.resignationtype", "zh-HK", "离职类型_hk", "离职类型（字典 hr_resignation_category；投影字段，由离职审批通过后回写；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）"),

            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "en-US", "离职原因_us", "离职原因（投影字段，由离职审批通过后回写）"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "ja-JP", "离职原因_jp", "离职原因（投影字段，由离职审批通过后回写）"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "zh-CN", "离职原因", "离职原因（投影字段，由离职审批通过后回写）"),
            // entity.employee.resignationreason
            new TranslationSeedItem("entity.employee.resignationreason", "zh-HK", "离职原因_hk", "离职原因（投影字段，由离职审批通过后回写）"),

            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "en-US", "员工状态_us", "员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "ja-JP", "员工状态_jp", "员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-CN", "员工状态", "员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-HK", "员工状态_hk", "员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）"),

            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "en-US", "当前主部门ID_us", "当前主部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；任职投影快照，未上岗可空）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "ja-JP", "当前主部门ID_jp", "当前主部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；任职投影快照，未上岗可空）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "zh-CN", "当前主部门ID", "当前主部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；任职投影快照，未上岗可空）"),
            // entity.employee.primarydeptid
            new TranslationSeedItem("entity.employee.primarydeptid", "zh-HK", "当前主部门ID_hk", "当前主部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；任职投影快照，未上岗可空）"),

            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "en-US", "当前主岗位ID_us", "当前主岗位（关联 TaktPost.Id，选项 TaktPosts/options；任职投影快照，未上岗可空）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "ja-JP", "当前主岗位ID_jp", "当前主岗位（关联 TaktPost.Id，选项 TaktPosts/options；任职投影快照，未上岗可空）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "zh-CN", "当前主岗位ID", "当前主岗位（关联 TaktPost.Id，选项 TaktPosts/options；任职投影快照，未上岗可空）"),
            // entity.employee.primarypostid
            new TranslationSeedItem("entity.employee.primarypostid", "zh-HK", "当前主岗位ID_hk", "当前主岗位（关联 TaktPost.Id，选项 TaktPosts/options；任职投影快照，未上岗可空）"),

            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）"),

            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "en-US", "紧急联系人姓名_us", "紧急联系人姓名（人事档案必填）"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "ja-JP", "紧急联系人姓名_jp", "紧急联系人姓名（人事档案必填）"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "zh-CN", "紧急联系人姓名", "紧急联系人姓名（人事档案必填）"),
            // entity.employee.emergencycontactname
            new TranslationSeedItem("entity.employee.emergencycontactname", "zh-HK", "紧急联系人姓名_hk", "紧急联系人姓名（人事档案必填）"),

            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "en-US", "紧急联系人电话_us", "紧急联系人电话（人事档案必填）"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "ja-JP", "紧急联系人电话_jp", "紧急联系人电话（人事档案必填）"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "zh-CN", "紧急联系人电话", "紧急联系人电话（人事档案必填）"),
            // entity.employee.emergencycontactphone
            new TranslationSeedItem("entity.employee.emergencycontactphone", "zh-HK", "紧急联系人电话_hk", "紧急联系人电话（人事档案必填）"),

            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "en-US", "家庭住址_us", "家庭住址（人事档案必填）"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "ja-JP", "家庭住址_jp", "家庭住址（人事档案必填）"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "zh-CN", "家庭住址", "家庭住址（人事档案必填）"),
            // entity.employee.homeaddress
            new TranslationSeedItem("entity.employee.homeaddress", "zh-HK", "家庭住址_hk", "家庭住址（人事档案必填）"),

            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "en-US", "头像URL_us", "头像URL"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "ja-JP", "头像URL_jp", "头像URL"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "zh-CN", "头像URL", "头像URL"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "zh-HK", "头像URL_hk", "头像URL"),

            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "en-US", "政治面貌_us", "政治面貌（字典 hr_political_status；0～12；人事档案必填）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "ja-JP", "政治面貌_jp", "政治面貌（字典 hr_political_status；0～12；人事档案必填）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "zh-CN", "政治面貌", "政治面貌（字典 hr_political_status；0～12；人事档案必填）"),
            // entity.employee.politicalstatus
            new TranslationSeedItem("entity.employee.politicalstatus", "zh-HK", "政治面貌_hk", "政治面貌（字典 hr_political_status；0～12；人事档案必填）"),

            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "en-US", "婚姻状况_us", "婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "ja-JP", "婚姻状况_jp", "婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-CN", "婚姻状况", "婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-HK", "婚姻状况_hk", "婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),

            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "en-US", "员工部门关联_us", "员工部门关联（RBAC，表 takt_human_resource_organization_employee_dept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "ja-JP", "员工部门关联_jp", "员工部门关联（RBAC，表 takt_human_resource_organization_employee_dept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-CN", "员工部门关联", "员工部门关联（RBAC，表 takt_human_resource_organization_employee_dept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-HK", "员工部门关联_hk", "员工部门关联（RBAC，表 takt_human_resource_organization_employee_dept）"),

            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "en-US", "员工岗位关联_us", "员工岗位关联（RBAC，表 takt_human_resource_organization_employee_post）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "ja-JP", "员工岗位关联_jp", "员工岗位关联（RBAC，表 takt_human_resource_organization_employee_post）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "zh-CN", "员工岗位关联", "员工岗位关联（RBAC，表 takt_human_resource_organization_employee_post）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "zh-HK", "员工岗位关联_hk", "员工岗位关联（RBAC，表 takt_human_resource_organization_employee_post）"),
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
        translation.ResourceGroup = "Personnel";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
