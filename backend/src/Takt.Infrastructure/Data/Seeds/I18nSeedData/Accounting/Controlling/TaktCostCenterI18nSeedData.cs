// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktCostCenterI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCostCenter 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCostCenter 实体国际化翻译种子（键前缀 entity.costCenter.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCostCenterI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCostCenter 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 costCenter 实体翻译...", tenantCode);

        foreach (var item in GetCostCenterTranslations())
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

        TaktLogger.Information("TaktCostCenter 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCostCenter 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.costCenter._self / entity.costCenter.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCostCenterTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.costCenter._self
            new TranslationSeedItem("entity.costCenter._self", "en-US", "Cost Center Information", "实体名称"),
            // entity.costCenter._self
            new TranslationSeedItem("entity.costCenter._self", "ja-JP", "成本中心信息", "实体名称"),
            // entity.costCenter._self
            new TranslationSeedItem("entity.costCenter._self", "zh-CN", "成本中心信息", "实体名称"),
            // entity.costCenter._self
            new TranslationSeedItem("entity.costCenter._self", "zh-HK", "成本中心信息", "实体名称"),

            // entity.costCenter.code
            new TranslationSeedItem("entity.costCenter.code", "en-US", "成本中心编码", "成本中心编码"),
            // entity.costCenter.code
            new TranslationSeedItem("entity.costCenter.code", "ja-JP", "成本中心编码", "成本中心编码"),
            // entity.costCenter.code
            new TranslationSeedItem("entity.costCenter.code", "zh-CN", "成本中心编码", "成本中心编码"),
            // entity.costCenter.code
            new TranslationSeedItem("entity.costCenter.code", "zh-HK", "成本中心编码", "成本中心编码"),

            // entity.costCenter.name
            new TranslationSeedItem("entity.costCenter.name", "en-US", "成本中心名称", "成本中心名称"),
            // entity.costCenter.name
            new TranslationSeedItem("entity.costCenter.name", "ja-JP", "成本中心名称", "成本中心名称"),
            // entity.costCenter.name
            new TranslationSeedItem("entity.costCenter.name", "zh-CN", "成本中心名称", "成本中心名称"),
            // entity.costCenter.name
            new TranslationSeedItem("entity.costCenter.name", "zh-HK", "成本中心名称", "成本中心名称"),

            // entity.costCenter.parentid
            new TranslationSeedItem("entity.costCenter.parentid", "en-US", "父级ID", "父级 ID（0 表示根节点）"),
            // entity.costCenter.parentid
            new TranslationSeedItem("entity.costCenter.parentid", "ja-JP", "父级ID", "父级 ID（0 表示根节点）"),
            // entity.costCenter.parentid
            new TranslationSeedItem("entity.costCenter.parentid", "zh-CN", "父级ID", "父级 ID（0 表示根节点）"),
            // entity.costCenter.parentid
            new TranslationSeedItem("entity.costCenter.parentid", "zh-HK", "父级ID", "父级 ID（0 表示根节点）"),

            // entity.costCenter.type
            new TranslationSeedItem("entity.costCenter.type", "en-US", "成本中心类型", "成本中心类型（0=成本中心，1=利润中心，2=投资中心）"),
            // entity.costCenter.type
            new TranslationSeedItem("entity.costCenter.type", "ja-JP", "成本中心类型", "成本中心类型（0=成本中心，1=利润中心，2=投资中心）"),
            // entity.costCenter.type
            new TranslationSeedItem("entity.costCenter.type", "zh-CN", "成本中心类型", "成本中心类型（0=成本中心，1=利润中心，2=投资中心）"),
            // entity.costCenter.type
            new TranslationSeedItem("entity.costCenter.type", "zh-HK", "成本中心类型", "成本中心类型（0=成本中心，1=利润中心，2=投资中心）"),

            // entity.costCenter.managerid
            new TranslationSeedItem("entity.costCenter.managerid", "en-US", "负责人ID", "负责人用户 ID"),
            // entity.costCenter.managerid
            new TranslationSeedItem("entity.costCenter.managerid", "ja-JP", "负责人ID", "负责人用户 ID"),
            // entity.costCenter.managerid
            new TranslationSeedItem("entity.costCenter.managerid", "zh-CN", "负责人ID", "负责人用户 ID"),
            // entity.costCenter.managerid
            new TranslationSeedItem("entity.costCenter.managerid", "zh-HK", "负责人ID", "负责人用户 ID"),

            // entity.costCenter.managername
            new TranslationSeedItem("entity.costCenter.managername", "en-US", "负责人姓名", "负责人姓名"),
            // entity.costCenter.managername
            new TranslationSeedItem("entity.costCenter.managername", "ja-JP", "负责人姓名", "负责人姓名"),
            // entity.costCenter.managername
            new TranslationSeedItem("entity.costCenter.managername", "zh-CN", "负责人姓名", "负责人姓名"),
            // entity.costCenter.managername
            new TranslationSeedItem("entity.costCenter.managername", "zh-HK", "负责人姓名", "负责人姓名"),

            // entity.costCenter.deptid
            new TranslationSeedItem("entity.costCenter.deptid", "en-US", "所属部门ID", "所属部门 ID"),
            // entity.costCenter.deptid
            new TranslationSeedItem("entity.costCenter.deptid", "ja-JP", "所属部门ID", "所属部门 ID"),
            // entity.costCenter.deptid
            new TranslationSeedItem("entity.costCenter.deptid", "zh-CN", "所属部门ID", "所属部门 ID"),
            // entity.costCenter.deptid
            new TranslationSeedItem("entity.costCenter.deptid", "zh-HK", "所属部门ID", "所属部门 ID"),

            // entity.costCenter.deptname
            new TranslationSeedItem("entity.costCenter.deptname", "en-US", "所属部门名称", "所属部门名称"),
            // entity.costCenter.deptname
            new TranslationSeedItem("entity.costCenter.deptname", "ja-JP", "所属部门名称", "所属部门名称"),
            // entity.costCenter.deptname
            new TranslationSeedItem("entity.costCenter.deptname", "zh-CN", "所属部门名称", "所属部门名称"),
            // entity.costCenter.deptname
            new TranslationSeedItem("entity.costCenter.deptname", "zh-HK", "所属部门名称", "所属部门名称"),

            // entity.costCenter.level
            new TranslationSeedItem("entity.costCenter.level", "en-US", "成本中心层级", "成本中心层级"),
            // entity.costCenter.level
            new TranslationSeedItem("entity.costCenter.level", "ja-JP", "成本中心层级", "成本中心层级"),
            // entity.costCenter.level
            new TranslationSeedItem("entity.costCenter.level", "zh-CN", "成本中心层级", "成本中心层级"),
            // entity.costCenter.level
            new TranslationSeedItem("entity.costCenter.level", "zh-HK", "成本中心层级", "成本中心层级"),

            // entity.costCenter.relatedplant
            new TranslationSeedItem("entity.costCenter.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.costCenter.relatedplant
            new TranslationSeedItem("entity.costCenter.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.costCenter.relatedplant
            new TranslationSeedItem("entity.costCenter.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.costCenter.relatedplant
            new TranslationSeedItem("entity.costCenter.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.costCenter.status
            new TranslationSeedItem("entity.costCenter.status", "en-US", "成本中心状态", "成本中心状态（1=启用，0=禁用）"),
            // entity.costCenter.status
            new TranslationSeedItem("entity.costCenter.status", "ja-JP", "成本中心状态", "成本中心状态（1=启用，0=禁用）"),
            // entity.costCenter.status
            new TranslationSeedItem("entity.costCenter.status", "zh-CN", "成本中心状态", "成本中心状态（1=启用，0=禁用）"),
            // entity.costCenter.status
            new TranslationSeedItem("entity.costCenter.status", "zh-HK", "成本中心状态", "成本中心状态（1=启用，0=禁用）"),

            // entity.costCenter.validfrom
            new TranslationSeedItem("entity.costCenter.validfrom", "en-US", "生效日期", "生效日期"),
            // entity.costCenter.validfrom
            new TranslationSeedItem("entity.costCenter.validfrom", "ja-JP", "生效日期", "生效日期"),
            // entity.costCenter.validfrom
            new TranslationSeedItem("entity.costCenter.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.costCenter.validfrom
            new TranslationSeedItem("entity.costCenter.validfrom", "zh-HK", "生效日期", "生效日期"),

            // entity.costCenter.validto
            new TranslationSeedItem("entity.costCenter.validto", "en-US", "失效日期", "失效日期"),
            // entity.costCenter.validto
            new TranslationSeedItem("entity.costCenter.validto", "ja-JP", "失效日期", "失效日期"),
            // entity.costCenter.validto
            new TranslationSeedItem("entity.costCenter.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.costCenter.validto
            new TranslationSeedItem("entity.costCenter.validto", "zh-HK", "失效日期", "失效日期"),

            // entity.costCenter.sortorder
            new TranslationSeedItem("entity.costCenter.sortorder", "en-US", "排序号", "排序号"),
            // entity.costCenter.sortorder
            new TranslationSeedItem("entity.costCenter.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.costCenter.sortorder
            new TranslationSeedItem("entity.costCenter.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.costCenter.sortorder
            new TranslationSeedItem("entity.costCenter.sortorder", "zh-HK", "排序号", "排序号"),
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
