// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktCostElementI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCostElement 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCostElement 实体国际化翻译种子（键前缀 entity.costElement.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCostElementI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCostElement 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 costElement 实体翻译...", tenantCode);

        foreach (var item in GetCostElementTranslations())
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

        TaktLogger.Information("TaktCostElement 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCostElement 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.costElement._self / entity.costElement.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCostElementTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.costElement._self
            new TranslationSeedItem("entity.costElement._self", "en-US", "Cost Element Information", "实体名称"),
            // entity.costElement._self
            new TranslationSeedItem("entity.costElement._self", "ja-JP", "成本要素信息", "实体名称"),
            // entity.costElement._self
            new TranslationSeedItem("entity.costElement._self", "zh-CN", "成本要素信息", "实体名称"),
            // entity.costElement._self
            new TranslationSeedItem("entity.costElement._self", "zh-HK", "成本要素信息", "实体名称"),

            // entity.costElement.code
            new TranslationSeedItem("entity.costElement.code", "en-US", "成本要素编码", "成本要素编码"),
            // entity.costElement.code
            new TranslationSeedItem("entity.costElement.code", "ja-JP", "成本要素编码", "成本要素编码"),
            // entity.costElement.code
            new TranslationSeedItem("entity.costElement.code", "zh-CN", "成本要素编码", "成本要素编码"),
            // entity.costElement.code
            new TranslationSeedItem("entity.costElement.code", "zh-HK", "成本要素编码", "成本要素编码"),

            // entity.costElement.name
            new TranslationSeedItem("entity.costElement.name", "en-US", "成本要素名称", "成本要素名称"),
            // entity.costElement.name
            new TranslationSeedItem("entity.costElement.name", "ja-JP", "成本要素名称", "成本要素名称"),
            // entity.costElement.name
            new TranslationSeedItem("entity.costElement.name", "zh-CN", "成本要素名称", "成本要素名称"),
            // entity.costElement.name
            new TranslationSeedItem("entity.costElement.name", "zh-HK", "成本要素名称", "成本要素名称"),

            // entity.costElement.type
            new TranslationSeedItem("entity.costElement.type", "en-US", "成本要素类型", "成本要素类型（0=初级，1=次级）"),
            // entity.costElement.type
            new TranslationSeedItem("entity.costElement.type", "ja-JP", "成本要素类型", "成本要素类型（0=初级，1=次级）"),
            // entity.costElement.type
            new TranslationSeedItem("entity.costElement.type", "zh-CN", "成本要素类型", "成本要素类型（0=初级，1=次级）"),
            // entity.costElement.type
            new TranslationSeedItem("entity.costElement.type", "zh-HK", "成本要素类型", "成本要素类型（0=初级，1=次级）"),

            // entity.costElement.category
            new TranslationSeedItem("entity.costElement.category", "en-US", "成本要素类别", "成本要素类别（0=人工，1=材料，2=制造费用，3=其他）"),
            // entity.costElement.category
            new TranslationSeedItem("entity.costElement.category", "ja-JP", "成本要素类别", "成本要素类别（0=人工，1=材料，2=制造费用，3=其他）"),
            // entity.costElement.category
            new TranslationSeedItem("entity.costElement.category", "zh-CN", "成本要素类别", "成本要素类别（0=人工，1=材料，2=制造费用，3=其他）"),
            // entity.costElement.category
            new TranslationSeedItem("entity.costElement.category", "zh-HK", "成本要素类别", "成本要素类别（0=人工，1=材料，2=制造费用，3=其他）"),

            // entity.costElement.parentid
            new TranslationSeedItem("entity.costElement.parentid", "en-US", "父级ID", "父级 ID"),
            // entity.costElement.parentid
            new TranslationSeedItem("entity.costElement.parentid", "ja-JP", "父级ID", "父级 ID"),
            // entity.costElement.parentid
            new TranslationSeedItem("entity.costElement.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.costElement.parentid
            new TranslationSeedItem("entity.costElement.parentid", "zh-HK", "父级ID", "父级 ID"),

            // entity.costElement.level
            new TranslationSeedItem("entity.costElement.level", "en-US", "成本要素层级", "成本要素层级"),
            // entity.costElement.level
            new TranslationSeedItem("entity.costElement.level", "ja-JP", "成本要素层级", "成本要素层级"),
            // entity.costElement.level
            new TranslationSeedItem("entity.costElement.level", "zh-CN", "成本要素层级", "成本要素层级"),
            // entity.costElement.level
            new TranslationSeedItem("entity.costElement.level", "zh-HK", "成本要素层级", "成本要素层级"),

            // entity.costElement.status
            new TranslationSeedItem("entity.costElement.status", "en-US", "成本要素状态", "成本要素状态（1=启用，0=禁用）"),
            // entity.costElement.status
            new TranslationSeedItem("entity.costElement.status", "ja-JP", "成本要素状态", "成本要素状态（1=启用，0=禁用）"),
            // entity.costElement.status
            new TranslationSeedItem("entity.costElement.status", "zh-CN", "成本要素状态", "成本要素状态（1=启用，0=禁用）"),
            // entity.costElement.status
            new TranslationSeedItem("entity.costElement.status", "zh-HK", "成本要素状态", "成本要素状态（1=启用，0=禁用）"),

            // entity.costElement.validfrom
            new TranslationSeedItem("entity.costElement.validfrom", "en-US", "生效日期", "生效日期"),
            // entity.costElement.validfrom
            new TranslationSeedItem("entity.costElement.validfrom", "ja-JP", "生效日期", "生效日期"),
            // entity.costElement.validfrom
            new TranslationSeedItem("entity.costElement.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.costElement.validfrom
            new TranslationSeedItem("entity.costElement.validfrom", "zh-HK", "生效日期", "生效日期"),

            // entity.costElement.validto
            new TranslationSeedItem("entity.costElement.validto", "en-US", "失效日期", "失效日期"),
            // entity.costElement.validto
            new TranslationSeedItem("entity.costElement.validto", "ja-JP", "失效日期", "失效日期"),
            // entity.costElement.validto
            new TranslationSeedItem("entity.costElement.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.costElement.validto
            new TranslationSeedItem("entity.costElement.validto", "zh-HK", "失效日期", "失效日期"),

            // entity.costElement.sortorder
            new TranslationSeedItem("entity.costElement.sortorder", "en-US", "排序号", "排序号"),
            // entity.costElement.sortorder
            new TranslationSeedItem("entity.costElement.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.costElement.sortorder
            new TranslationSeedItem("entity.costElement.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.costElement.sortorder
            new TranslationSeedItem("entity.costElement.sortorder", "zh-HK", "排序号", "排序号"),
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
