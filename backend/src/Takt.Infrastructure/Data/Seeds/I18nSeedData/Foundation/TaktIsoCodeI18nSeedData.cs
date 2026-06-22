// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktIsoCodeI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIsoCode 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktIsoCode 实体国际化翻译种子（键前缀 entity.isocode.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIsoCodeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIsoCode 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 isocode 实体翻译...", tenantCode);

        foreach (var item in GetIsoCodeTranslations())
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

        TaktLogger.Information("TaktIsoCode 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIsoCode 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.isocode._self / entity.isocode.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIsoCodeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.isocode._self
            new TranslationSeedItem("entity.isocode._self", "en-US", "Iso Code Information_us", "实体名称"),
            // entity.isocode._self
            new TranslationSeedItem("entity.isocode._self", "ja-JP", "ISO 编码信息_jp", "实体名称"),
            // entity.isocode._self
            new TranslationSeedItem("entity.isocode._self", "zh-CN", "ISO 编码信息", "实体名称"),
            // entity.isocode._self
            new TranslationSeedItem("entity.isocode._self", "zh-HK", "ISO 编码信息_hk", "实体名称"),

            // entity.isocode.category
            new TranslationSeedItem("entity.isocode.category", "en-US", "编码类别_us", "编码类别（字典 sys_iso_code_category；1=部门，2=公司，3=产品，4=通用）"),
            // entity.isocode.category
            new TranslationSeedItem("entity.isocode.category", "ja-JP", "编码类别_jp", "编码类别（字典 sys_iso_code_category；1=部门，2=公司，3=产品，4=通用）"),
            // entity.isocode.category
            new TranslationSeedItem("entity.isocode.category", "zh-CN", "编码类别", "编码类别（字典 sys_iso_code_category；1=部门，2=公司，3=产品，4=通用）"),
            // entity.isocode.category
            new TranslationSeedItem("entity.isocode.category", "zh-HK", "编码类别_hk", "编码类别（字典 sys_iso_code_category；1=部门，2=公司，3=产品，4=通用）"),

            // entity.isocode.isocode
            new TranslationSeedItem("entity.isocode.isocode", "en-US", "ISO编码_us", "ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编号规则等段引用，如 Eng、Pmc、D1000）"),
            // entity.isocode.isocode
            new TranslationSeedItem("entity.isocode.isocode", "ja-JP", "ISO编码_jp", "ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编号规则等段引用，如 Eng、Pmc、D1000）"),
            // entity.isocode.isocode
            new TranslationSeedItem("entity.isocode.isocode", "zh-CN", "ISO编码", "ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编号规则等段引用，如 Eng、Pmc、D1000）"),
            // entity.isocode.isocode
            new TranslationSeedItem("entity.isocode.isocode", "zh-HK", "ISO编码_hk", "ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编号规则等段引用，如 Eng、Pmc、D1000）"),

            // entity.isocode.isoname
            new TranslationSeedItem("entity.isocode.isoname", "en-US", "ISO名称_us", "ISO 名称（如：技术、生管、总经理室）"),
            // entity.isocode.isoname
            new TranslationSeedItem("entity.isocode.isoname", "ja-JP", "ISO名称_jp", "ISO 名称（如：技术、生管、总经理室）"),
            // entity.isocode.isoname
            new TranslationSeedItem("entity.isocode.isoname", "zh-CN", "ISO名称", "ISO 名称（如：技术、生管、总经理室）"),
            // entity.isocode.isoname
            new TranslationSeedItem("entity.isocode.isoname", "zh-HK", "ISO名称_hk", "ISO 名称（如：技术、生管、总经理室）"),

            // entity.isocode.sortorder
            new TranslationSeedItem("entity.isocode.sortorder", "en-US", "排序号_us", "排序号"),
            // entity.isocode.sortorder
            new TranslationSeedItem("entity.isocode.sortorder", "ja-JP", "排序号_jp", "排序号"),
            // entity.isocode.sortorder
            new TranslationSeedItem("entity.isocode.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.isocode.sortorder
            new TranslationSeedItem("entity.isocode.sortorder", "zh-HK", "排序号_hk", "排序号"),

            // entity.isocode.isbuiltin
            new TranslationSeedItem("entity.isocode.isbuiltin", "en-US", "是否内置_us", "是否内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）"),
            // entity.isocode.isbuiltin
            new TranslationSeedItem("entity.isocode.isbuiltin", "ja-JP", "是否内置_jp", "是否内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）"),
            // entity.isocode.isbuiltin
            new TranslationSeedItem("entity.isocode.isbuiltin", "zh-CN", "是否内置", "是否内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）"),
            // entity.isocode.isbuiltin
            new TranslationSeedItem("entity.isocode.isbuiltin", "zh-HK", "是否内置_hk", "是否内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）"),

            // entity.isocode.status
            new TranslationSeedItem("entity.isocode.status", "en-US", "状态_us", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.isocode.status
            new TranslationSeedItem("entity.isocode.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.isocode.status
            new TranslationSeedItem("entity.isocode.status", "zh-CN", "状态", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.isocode.status
            new TranslationSeedItem("entity.isocode.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),

            // entity.isocode.description
            new TranslationSeedItem("entity.isocode.description", "en-US", "描述说明_us", "描述说明"),
            // entity.isocode.description
            new TranslationSeedItem("entity.isocode.description", "ja-JP", "描述说明_jp", "描述说明"),
            // entity.isocode.description
            new TranslationSeedItem("entity.isocode.description", "zh-CN", "描述说明", "描述说明"),
            // entity.isocode.description
            new TranslationSeedItem("entity.isocode.description", "zh-HK", "描述说明_hk", "描述说明"),
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
        translation.ResourceGroup = "Foundation";
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
