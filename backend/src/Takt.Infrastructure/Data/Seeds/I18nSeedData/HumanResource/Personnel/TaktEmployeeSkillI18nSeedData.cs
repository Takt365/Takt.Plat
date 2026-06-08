// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeSkillI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeSkill 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeSkill 实体国际化翻译种子（键前缀 entity.employeeSkill.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeSkillI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeSkill 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeSkill 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeSkillTranslations())
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

        TaktLogger.Information("TaktEmployeeSkill 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeSkill 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeSkill._self / entity.employeeSkill.{{field}}；ResourceGroup=TaktModule.HumanResource；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeSkillTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeSkill._self
            new TranslationSeedItem("entity.employeeSkill._self", "en-US", "Employee Skill Information", "实体名称"),
            // entity.employeeSkill._self
            new TranslationSeedItem("entity.employeeSkill._self", "ja-JP", "员工技能与证书信息", "实体名称"),
            // entity.employeeSkill._self
            new TranslationSeedItem("entity.employeeSkill._self", "zh-CN", "员工技能与证书信息", "实体名称"),
            // entity.employeeSkill._self
            new TranslationSeedItem("entity.employeeSkill._self", "zh-HK", "员工技能与证书信息", "实体名称"),

            // entity.employeeSkill.employeeid
            new TranslationSeedItem("entity.employeeSkill.employeeid", "en-US", "员工ID", "员工ID"),
            // entity.employeeSkill.employeeid
            new TranslationSeedItem("entity.employeeSkill.employeeid", "ja-JP", "员工ID", "员工ID"),
            // entity.employeeSkill.employeeid
            new TranslationSeedItem("entity.employeeSkill.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeSkill.employeeid
            new TranslationSeedItem("entity.employeeSkill.employeeid", "zh-HK", "员工ID", "员工ID"),

            // entity.employeeSkill.skillname
            new TranslationSeedItem("entity.employeeSkill.skillname", "en-US", "技能名称", "技能名称"),
            // entity.employeeSkill.skillname
            new TranslationSeedItem("entity.employeeSkill.skillname", "ja-JP", "技能名称", "技能名称"),
            // entity.employeeSkill.skillname
            new TranslationSeedItem("entity.employeeSkill.skillname", "zh-CN", "技能名称", "技能名称"),
            // entity.employeeSkill.skillname
            new TranslationSeedItem("entity.employeeSkill.skillname", "zh-HK", "技能名称", "技能名称"),

            // entity.employeeSkill.skilllevel
            new TranslationSeedItem("entity.employeeSkill.skilllevel", "en-US", "技能等级", "技能等级（0=入门，1=熟练，2=精通，3=专家）"),
            // entity.employeeSkill.skilllevel
            new TranslationSeedItem("entity.employeeSkill.skilllevel", "ja-JP", "技能等级", "技能等级（0=入门，1=熟练，2=精通，3=专家）"),
            // entity.employeeSkill.skilllevel
            new TranslationSeedItem("entity.employeeSkill.skilllevel", "zh-CN", "技能等级", "技能等级（0=入门，1=熟练，2=精通，3=专家）"),
            // entity.employeeSkill.skilllevel
            new TranslationSeedItem("entity.employeeSkill.skilllevel", "zh-HK", "技能等级", "技能等级（0=入门，1=熟练，2=精通，3=专家）"),

            // entity.employeeSkill.certificatename
            new TranslationSeedItem("entity.employeeSkill.certificatename", "en-US", "证书名称", "证书名称"),
            // entity.employeeSkill.certificatename
            new TranslationSeedItem("entity.employeeSkill.certificatename", "ja-JP", "证书名称", "证书名称"),
            // entity.employeeSkill.certificatename
            new TranslationSeedItem("entity.employeeSkill.certificatename", "zh-CN", "证书名称", "证书名称"),
            // entity.employeeSkill.certificatename
            new TranslationSeedItem("entity.employeeSkill.certificatename", "zh-HK", "证书名称", "证书名称"),

            // entity.employeeSkill.certificateno
            new TranslationSeedItem("entity.employeeSkill.certificateno", "en-US", "证书编号", "证书编号"),
            // entity.employeeSkill.certificateno
            new TranslationSeedItem("entity.employeeSkill.certificateno", "ja-JP", "证书编号", "证书编号"),
            // entity.employeeSkill.certificateno
            new TranslationSeedItem("entity.employeeSkill.certificateno", "zh-CN", "证书编号", "证书编号"),
            // entity.employeeSkill.certificateno
            new TranslationSeedItem("entity.employeeSkill.certificateno", "zh-HK", "证书编号", "证书编号"),

            // entity.employeeSkill.obtaineddate
            new TranslationSeedItem("entity.employeeSkill.obtaineddate", "en-US", "取得日期", "取得日期"),
            // entity.employeeSkill.obtaineddate
            new TranslationSeedItem("entity.employeeSkill.obtaineddate", "ja-JP", "取得日期", "取得日期"),
            // entity.employeeSkill.obtaineddate
            new TranslationSeedItem("entity.employeeSkill.obtaineddate", "zh-CN", "取得日期", "取得日期"),
            // entity.employeeSkill.obtaineddate
            new TranslationSeedItem("entity.employeeSkill.obtaineddate", "zh-HK", "取得日期", "取得日期"),

            // entity.employeeSkill.expirydate
            new TranslationSeedItem("entity.employeeSkill.expirydate", "en-US", "到期日期", "到期日期"),
            // entity.employeeSkill.expirydate
            new TranslationSeedItem("entity.employeeSkill.expirydate", "ja-JP", "到期日期", "到期日期"),
            // entity.employeeSkill.expirydate
            new TranslationSeedItem("entity.employeeSkill.expirydate", "zh-CN", "到期日期", "到期日期"),
            // entity.employeeSkill.expirydate
            new TranslationSeedItem("entity.employeeSkill.expirydate", "zh-HK", "到期日期", "到期日期"),
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
