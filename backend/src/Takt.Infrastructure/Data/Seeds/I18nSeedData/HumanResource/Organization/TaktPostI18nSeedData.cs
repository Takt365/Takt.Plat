// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization
// 文件名称：TaktPostI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPost 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization;

/// <summary>
/// TaktPost 实体国际化翻译种子（键前缀 entity.post.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPostI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPost 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 post 实体翻译...", tenantCode);

        foreach (var item in GetPostTranslations())
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

        TaktLogger.Information("TaktPost 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPost 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.post._self / entity.post.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPostTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.post._self
            new TranslationSeedItem("entity.post._self", "en-US", "Post Information", "实体名称"),
            // entity.post._self
            new TranslationSeedItem("entity.post._self", "ja-JP", "岗位信息", "实体名称"),
            // entity.post._self
            new TranslationSeedItem("entity.post._self", "zh-CN", "岗位信息", "实体名称"),
            // entity.post._self
            new TranslationSeedItem("entity.post._self", "zh-HK", "岗位信息", "实体名称"),

            // entity.post.code
            new TranslationSeedItem("entity.post.code", "en-US", "岗位编码", "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）"),
            // entity.post.code
            new TranslationSeedItem("entity.post.code", "ja-JP", "岗位编码", "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）"),
            // entity.post.code
            new TranslationSeedItem("entity.post.code", "zh-CN", "岗位编码", "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）"),
            // entity.post.code
            new TranslationSeedItem("entity.post.code", "zh-HK", "岗位编码", "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）"),

            // entity.post.name
            new TranslationSeedItem("entity.post.name", "en-US", "岗位名称", "岗位名称"),
            // entity.post.name
            new TranslationSeedItem("entity.post.name", "ja-JP", "岗位名称", "岗位名称"),
            // entity.post.name
            new TranslationSeedItem("entity.post.name", "zh-CN", "岗位名称", "岗位名称"),
            // entity.post.name
            new TranslationSeedItem("entity.post.name", "zh-HK", "岗位名称", "岗位名称"),

            // entity.post.deptid
            new TranslationSeedItem("entity.post.deptid", "en-US", "所属部门ID", "所属部门ID"),
            // entity.post.deptid
            new TranslationSeedItem("entity.post.deptid", "ja-JP", "所属部门ID", "所属部门ID"),
            // entity.post.deptid
            new TranslationSeedItem("entity.post.deptid", "zh-CN", "所属部门ID", "所属部门ID"),
            // entity.post.deptid
            new TranslationSeedItem("entity.post.deptid", "zh-HK", "所属部门ID", "所属部门ID"),

            // entity.post.type
            new TranslationSeedItem("entity.post.type", "en-US", "岗位类型", "岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）"),
            // entity.post.type
            new TranslationSeedItem("entity.post.type", "ja-JP", "岗位类型", "岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）"),
            // entity.post.type
            new TranslationSeedItem("entity.post.type", "zh-CN", "岗位类型", "岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）"),
            // entity.post.type
            new TranslationSeedItem("entity.post.type", "zh-HK", "岗位类型", "岗位类型（0=管理岗，1=技术岗，2=业务岗，3=职能岗，4=操作岗）"),

            // entity.post.headcount
            new TranslationSeedItem("entity.post.headcount", "en-US", "编制人数", "编制人数"),
            // entity.post.headcount
            new TranslationSeedItem("entity.post.headcount", "ja-JP", "编制人数", "编制人数"),
            // entity.post.headcount
            new TranslationSeedItem("entity.post.headcount", "zh-CN", "编制人数", "编制人数"),
            // entity.post.headcount
            new TranslationSeedItem("entity.post.headcount", "zh-HK", "编制人数", "编制人数"),

            // entity.post.currentcount
            new TranslationSeedItem("entity.post.currentcount", "en-US", "当前在职人数", "当前在职人数"),
            // entity.post.currentcount
            new TranslationSeedItem("entity.post.currentcount", "ja-JP", "当前在职人数", "当前在职人数"),
            // entity.post.currentcount
            new TranslationSeedItem("entity.post.currentcount", "zh-CN", "当前在职人数", "当前在职人数"),
            // entity.post.currentcount
            new TranslationSeedItem("entity.post.currentcount", "zh-HK", "当前在职人数", "当前在职人数"),

            // entity.post.responsibilities
            new TranslationSeedItem("entity.post.responsibilities", "en-US", "岗位职责", "岗位职责"),
            // entity.post.responsibilities
            new TranslationSeedItem("entity.post.responsibilities", "ja-JP", "岗位职责", "岗位职责"),
            // entity.post.responsibilities
            new TranslationSeedItem("entity.post.responsibilities", "zh-CN", "岗位职责", "岗位职责"),
            // entity.post.responsibilities
            new TranslationSeedItem("entity.post.responsibilities", "zh-HK", "岗位职责", "岗位职责"),

            // entity.post.requirements
            new TranslationSeedItem("entity.post.requirements", "en-US", "任职要求", "任职要求"),
            // entity.post.requirements
            new TranslationSeedItem("entity.post.requirements", "ja-JP", "任职要求", "任职要求"),
            // entity.post.requirements
            new TranslationSeedItem("entity.post.requirements", "zh-CN", "任职要求", "任职要求"),
            // entity.post.requirements
            new TranslationSeedItem("entity.post.requirements", "zh-HK", "任职要求", "任职要求"),

            // entity.post.educationrequired
            new TranslationSeedItem("entity.post.educationrequired", "en-US", "学历要求", "学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.post.educationrequired
            new TranslationSeedItem("entity.post.educationrequired", "ja-JP", "学历要求", "学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.post.educationrequired
            new TranslationSeedItem("entity.post.educationrequired", "zh-CN", "学历要求", "学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),
            // entity.post.educationrequired
            new TranslationSeedItem("entity.post.educationrequired", "zh-HK", "学历要求", "学历要求（1=高中及以下，2=大专，3=本科，4=硕士，5=博士）"),

            // entity.post.experienceyears
            new TranslationSeedItem("entity.post.experienceyears", "en-US", "工作经验要求（年）", "工作经验要求（年）"),
            // entity.post.experienceyears
            new TranslationSeedItem("entity.post.experienceyears", "ja-JP", "工作经验要求（年）", "工作经验要求（年）"),
            // entity.post.experienceyears
            new TranslationSeedItem("entity.post.experienceyears", "zh-CN", "工作经验要求（年）", "工作经验要求（年）"),
            // entity.post.experienceyears
            new TranslationSeedItem("entity.post.experienceyears", "zh-HK", "工作经验要求（年）", "工作经验要求（年）"),

            // entity.post.salarymin
            new TranslationSeedItem("entity.post.salarymin", "en-US", "薪资范围（最低）", "薪资范围（最低）"),
            // entity.post.salarymin
            new TranslationSeedItem("entity.post.salarymin", "ja-JP", "薪资范围（最低）", "薪资范围（最低）"),
            // entity.post.salarymin
            new TranslationSeedItem("entity.post.salarymin", "zh-CN", "薪资范围（最低）", "薪资范围（最低）"),
            // entity.post.salarymin
            new TranslationSeedItem("entity.post.salarymin", "zh-HK", "薪资范围（最低）", "薪资范围（最低）"),

            // entity.post.salarymax
            new TranslationSeedItem("entity.post.salarymax", "en-US", "薪资范围（最高）", "薪资范围（最高）"),
            // entity.post.salarymax
            new TranslationSeedItem("entity.post.salarymax", "ja-JP", "薪资范围（最高）", "薪资范围（最高）"),
            // entity.post.salarymax
            new TranslationSeedItem("entity.post.salarymax", "zh-CN", "薪资范围（最高）", "薪资范围（最高）"),
            // entity.post.salarymax
            new TranslationSeedItem("entity.post.salarymax", "zh-HK", "薪资范围（最高）", "薪资范围（最高）"),

            // entity.post.status
            new TranslationSeedItem("entity.post.status", "en-US", "状态", "状态（1=启用，0=禁用）"),
            // entity.post.status
            new TranslationSeedItem("entity.post.status", "ja-JP", "状态", "状态（1=启用，0=禁用）"),
            // entity.post.status
            new TranslationSeedItem("entity.post.status", "zh-CN", "状态", "状态（1=启用，0=禁用）"),
            // entity.post.status
            new TranslationSeedItem("entity.post.status", "zh-HK", "状态", "状态（1=启用，0=禁用）"),

            // entity.post.sortorder
            new TranslationSeedItem("entity.post.sortorder", "en-US", "排序号", "排序号"),
            // entity.post.sortorder
            new TranslationSeedItem("entity.post.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.post.sortorder
            new TranslationSeedItem("entity.post.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.post.sortorder
            new TranslationSeedItem("entity.post.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.post.description
            new TranslationSeedItem("entity.post.description", "en-US", "岗位描述", "岗位描述"),
            // entity.post.description
            new TranslationSeedItem("entity.post.description", "ja-JP", "岗位描述", "岗位描述"),
            // entity.post.description
            new TranslationSeedItem("entity.post.description", "zh-CN", "岗位描述", "岗位描述"),
            // entity.post.description
            new TranslationSeedItem("entity.post.description", "zh-HK", "岗位描述", "岗位描述"),

            // entity.post.employeeposts
            new TranslationSeedItem("entity.post.employeeposts", "en-US", "employeePosts", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.post.employeeposts
            new TranslationSeedItem("entity.post.employeeposts", "ja-JP", "employeePosts", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.post.employeeposts
            new TranslationSeedItem("entity.post.employeeposts", "zh-CN", "employeePosts", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.post.employeeposts
            new TranslationSeedItem("entity.post.employeeposts", "zh-HK", "employeePosts", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
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
