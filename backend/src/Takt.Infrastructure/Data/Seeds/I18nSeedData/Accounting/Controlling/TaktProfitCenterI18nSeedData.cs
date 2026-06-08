// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktProfitCenterI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProfitCenter 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktProfitCenter 实体国际化翻译种子（键前缀 entity.profitCenter.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProfitCenterI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProfitCenter 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 profitCenter 实体翻译...", tenantCode);

        foreach (var item in GetProfitCenterTranslations())
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

        TaktLogger.Information("TaktProfitCenter 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProfitCenter 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.profitCenter._self / entity.profitCenter.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProfitCenterTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.profitCenter._self
            new TranslationSeedItem("entity.profitCenter._self", "en-US", "Profit Center Information", "实体名称"),
            // entity.profitCenter._self
            new TranslationSeedItem("entity.profitCenter._self", "ja-JP", "利润中心信息", "实体名称"),
            // entity.profitCenter._self
            new TranslationSeedItem("entity.profitCenter._self", "zh-CN", "利润中心信息", "实体名称"),
            // entity.profitCenter._self
            new TranslationSeedItem("entity.profitCenter._self", "zh-HK", "利润中心信息", "实体名称"),

            // entity.profitCenter.code
            new TranslationSeedItem("entity.profitCenter.code", "en-US", "利润中心编码", "利润中心编码"),
            // entity.profitCenter.code
            new TranslationSeedItem("entity.profitCenter.code", "ja-JP", "利润中心编码", "利润中心编码"),
            // entity.profitCenter.code
            new TranslationSeedItem("entity.profitCenter.code", "zh-CN", "利润中心编码", "利润中心编码"),
            // entity.profitCenter.code
            new TranslationSeedItem("entity.profitCenter.code", "zh-HK", "利润中心编码", "利润中心编码"),

            // entity.profitCenter.name
            new TranslationSeedItem("entity.profitCenter.name", "en-US", "利润中心名称", "利润中心名称"),
            // entity.profitCenter.name
            new TranslationSeedItem("entity.profitCenter.name", "ja-JP", "利润中心名称", "利润中心名称"),
            // entity.profitCenter.name
            new TranslationSeedItem("entity.profitCenter.name", "zh-CN", "利润中心名称", "利润中心名称"),
            // entity.profitCenter.name
            new TranslationSeedItem("entity.profitCenter.name", "zh-HK", "利润中心名称", "利润中心名称"),

            // entity.profitCenter.parentid
            new TranslationSeedItem("entity.profitCenter.parentid", "en-US", "父级ID", "父级 ID"),
            // entity.profitCenter.parentid
            new TranslationSeedItem("entity.profitCenter.parentid", "ja-JP", "父级ID", "父级 ID"),
            // entity.profitCenter.parentid
            new TranslationSeedItem("entity.profitCenter.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.profitCenter.parentid
            new TranslationSeedItem("entity.profitCenter.parentid", "zh-HK", "父级ID", "父级 ID"),

            // entity.profitCenter.managerid
            new TranslationSeedItem("entity.profitCenter.managerid", "en-US", "负责人ID", "负责人用户 ID"),
            // entity.profitCenter.managerid
            new TranslationSeedItem("entity.profitCenter.managerid", "ja-JP", "负责人ID", "负责人用户 ID"),
            // entity.profitCenter.managerid
            new TranslationSeedItem("entity.profitCenter.managerid", "zh-CN", "负责人ID", "负责人用户 ID"),
            // entity.profitCenter.managerid
            new TranslationSeedItem("entity.profitCenter.managerid", "zh-HK", "负责人ID", "负责人用户 ID"),

            // entity.profitCenter.managername
            new TranslationSeedItem("entity.profitCenter.managername", "en-US", "负责人姓名", "负责人姓名"),
            // entity.profitCenter.managername
            new TranslationSeedItem("entity.profitCenter.managername", "ja-JP", "负责人姓名", "负责人姓名"),
            // entity.profitCenter.managername
            new TranslationSeedItem("entity.profitCenter.managername", "zh-CN", "负责人姓名", "负责人姓名"),
            // entity.profitCenter.managername
            new TranslationSeedItem("entity.profitCenter.managername", "zh-HK", "负责人姓名", "负责人姓名"),

            // entity.profitCenter.deptid
            new TranslationSeedItem("entity.profitCenter.deptid", "en-US", "所属部门ID", "所属部门 ID"),
            // entity.profitCenter.deptid
            new TranslationSeedItem("entity.profitCenter.deptid", "ja-JP", "所属部门ID", "所属部门 ID"),
            // entity.profitCenter.deptid
            new TranslationSeedItem("entity.profitCenter.deptid", "zh-CN", "所属部门ID", "所属部门 ID"),
            // entity.profitCenter.deptid
            new TranslationSeedItem("entity.profitCenter.deptid", "zh-HK", "所属部门ID", "所属部门 ID"),

            // entity.profitCenter.deptname
            new TranslationSeedItem("entity.profitCenter.deptname", "en-US", "所属部门名称", "所属部门名称"),
            // entity.profitCenter.deptname
            new TranslationSeedItem("entity.profitCenter.deptname", "ja-JP", "所属部门名称", "所属部门名称"),
            // entity.profitCenter.deptname
            new TranslationSeedItem("entity.profitCenter.deptname", "zh-CN", "所属部门名称", "所属部门名称"),
            // entity.profitCenter.deptname
            new TranslationSeedItem("entity.profitCenter.deptname", "zh-HK", "所属部门名称", "所属部门名称"),

            // entity.profitCenter.level
            new TranslationSeedItem("entity.profitCenter.level", "en-US", "利润中心层级", "利润中心层级"),
            // entity.profitCenter.level
            new TranslationSeedItem("entity.profitCenter.level", "ja-JP", "利润中心层级", "利润中心层级"),
            // entity.profitCenter.level
            new TranslationSeedItem("entity.profitCenter.level", "zh-CN", "利润中心层级", "利润中心层级"),
            // entity.profitCenter.level
            new TranslationSeedItem("entity.profitCenter.level", "zh-HK", "利润中心层级", "利润中心层级"),

            // entity.profitCenter.relatedplant
            new TranslationSeedItem("entity.profitCenter.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.profitCenter.relatedplant
            new TranslationSeedItem("entity.profitCenter.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.profitCenter.relatedplant
            new TranslationSeedItem("entity.profitCenter.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.profitCenter.relatedplant
            new TranslationSeedItem("entity.profitCenter.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.profitCenter.status
            new TranslationSeedItem("entity.profitCenter.status", "en-US", "利润中心状态", "利润中心状态（1=启用，0=禁用）"),
            // entity.profitCenter.status
            new TranslationSeedItem("entity.profitCenter.status", "ja-JP", "利润中心状态", "利润中心状态（1=启用，0=禁用）"),
            // entity.profitCenter.status
            new TranslationSeedItem("entity.profitCenter.status", "zh-CN", "利润中心状态", "利润中心状态（1=启用，0=禁用）"),
            // entity.profitCenter.status
            new TranslationSeedItem("entity.profitCenter.status", "zh-HK", "利润中心状态", "利润中心状态（1=启用，0=禁用）"),

            // entity.profitCenter.validfrom
            new TranslationSeedItem("entity.profitCenter.validfrom", "en-US", "生效日期", "生效日期"),
            // entity.profitCenter.validfrom
            new TranslationSeedItem("entity.profitCenter.validfrom", "ja-JP", "生效日期", "生效日期"),
            // entity.profitCenter.validfrom
            new TranslationSeedItem("entity.profitCenter.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.profitCenter.validfrom
            new TranslationSeedItem("entity.profitCenter.validfrom", "zh-HK", "生效日期", "生效日期"),

            // entity.profitCenter.validto
            new TranslationSeedItem("entity.profitCenter.validto", "en-US", "失效日期", "失效日期"),
            // entity.profitCenter.validto
            new TranslationSeedItem("entity.profitCenter.validto", "ja-JP", "失效日期", "失效日期"),
            // entity.profitCenter.validto
            new TranslationSeedItem("entity.profitCenter.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.profitCenter.validto
            new TranslationSeedItem("entity.profitCenter.validto", "zh-HK", "失效日期", "失效日期"),

            // entity.profitCenter.sortorder
            new TranslationSeedItem("entity.profitCenter.sortorder", "en-US", "排序号", "排序号"),
            // entity.profitCenter.sortorder
            new TranslationSeedItem("entity.profitCenter.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.profitCenter.sortorder
            new TranslationSeedItem("entity.profitCenter.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.profitCenter.sortorder
            new TranslationSeedItem("entity.profitCenter.sortorder", "zh-HK", "排序号", "排序号"),
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
        translation.ResourceGroup = TaktModule.Accounting;
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
