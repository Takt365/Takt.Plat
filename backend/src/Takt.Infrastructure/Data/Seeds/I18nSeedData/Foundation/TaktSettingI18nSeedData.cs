// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktSettingI18nSeedData.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSetting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSetting 实体国际化翻译种子（键前缀 entity.setting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSettingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSetting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 setting 实体翻译...", tenantCode);

        foreach (var item in GetSettingTranslations())
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

        TaktLogger.Information("TaktSetting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSetting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.setting._self / entity.setting.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSettingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.setting._self
            new TranslationSeedItem("entity.setting._self", "en-US", "Setting Information_us", "实体名称"),
            // entity.setting._self
            new TranslationSeedItem("entity.setting._self", "ja-JP", "系统设置信息_jp", "实体名称"),
            // entity.setting._self
            new TranslationSeedItem("entity.setting._self", "zh-CN", "系统设置信息", "实体名称"),
            // entity.setting._self
            new TranslationSeedItem("entity.setting._self", "zh-HK", "系统设置信息_hk", "实体名称"),

            // entity.setting.key
            new TranslationSeedItem("entity.setting.key", "en-US", "设置键_us", "设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.setting.key
            new TranslationSeedItem("entity.setting.key", "ja-JP", "设置键_jp", "设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.setting.key
            new TranslationSeedItem("entity.setting.key", "zh-CN", "设置键", "设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）"),
            // entity.setting.key
            new TranslationSeedItem("entity.setting.key", "zh-HK", "设置键_hk", "设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）"),

            // entity.setting.value
            new TranslationSeedItem("entity.setting.value", "en-US", "设置值_us", "设置值（字符串形式，复杂对象用JSON）"),
            // entity.setting.value
            new TranslationSeedItem("entity.setting.value", "ja-JP", "设置值_jp", "设置值（字符串形式，复杂对象用JSON）"),
            // entity.setting.value
            new TranslationSeedItem("entity.setting.value", "zh-CN", "设置值", "设置值（字符串形式，复杂对象用JSON）"),
            // entity.setting.value
            new TranslationSeedItem("entity.setting.value", "zh-HK", "设置值_hk", "设置值（字符串形式，复杂对象用JSON）"),

            // entity.setting.name
            new TranslationSeedItem("entity.setting.name", "en-US", "设置名称_us", "设置名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.setting.name
            new TranslationSeedItem("entity.setting.name", "ja-JP", "设置名称_jp", "设置名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.setting.name
            new TranslationSeedItem("entity.setting.name", "zh-CN", "设置名称", "设置名称（显示名称，如：站点名称、最大上传大小）"),
            // entity.setting.name
            new TranslationSeedItem("entity.setting.name", "zh-HK", "设置名称_hk", "设置名称（显示名称，如：站点名称、最大上传大小）"),

            // entity.setting.description
            new TranslationSeedItem("entity.setting.description", "en-US", "设置描述_us", "设置描述"),
            // entity.setting.description
            new TranslationSeedItem("entity.setting.description", "ja-JP", "设置描述_jp", "设置描述"),
            // entity.setting.description
            new TranslationSeedItem("entity.setting.description", "zh-CN", "设置描述", "设置描述"),
            // entity.setting.description
            new TranslationSeedItem("entity.setting.description", "zh-HK", "设置描述_hk", "设置描述"),

            // entity.setting.group
            new TranslationSeedItem("entity.setting.group", "en-US", "设置类别_us", "设置类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.setting.group
            new TranslationSeedItem("entity.setting.group", "ja-JP", "设置类别_jp", "设置类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.setting.group
            new TranslationSeedItem("entity.setting.group", "zh-CN", "设置类别", "设置类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.setting.group
            new TranslationSeedItem("entity.setting.group", "zh-HK", "设置类别_hk", "设置类别（字典 sys_resource_type；frontend=前端 backend=后端）"),

            // entity.setting.valuetype
            new TranslationSeedItem("entity.setting.valuetype", "en-US", "值类型_us", "值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.setting.valuetype
            new TranslationSeedItem("entity.setting.valuetype", "ja-JP", "值类型_jp", "值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.setting.valuetype
            new TranslationSeedItem("entity.setting.valuetype", "zh-CN", "值类型", "值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.setting.valuetype
            new TranslationSeedItem("entity.setting.valuetype", "zh-HK", "值类型_hk", "值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）"),

            // entity.setting.isbuiltin
            new TranslationSeedItem("entity.setting.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isbuiltin
            new TranslationSeedItem("entity.setting.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isbuiltin
            new TranslationSeedItem("entity.setting.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isbuiltin
            new TranslationSeedItem("entity.setting.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no_type；0=否 1=是）"),

            // entity.setting.isreadonly
            new TranslationSeedItem("entity.setting.isreadonly", "en-US", "只读_us", "只读（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isreadonly
            new TranslationSeedItem("entity.setting.isreadonly", "ja-JP", "只读_jp", "只读（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isreadonly
            new TranslationSeedItem("entity.setting.isreadonly", "zh-CN", "只读", "只读（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isreadonly
            new TranslationSeedItem("entity.setting.isreadonly", "zh-HK", "只读_hk", "只读（字典 sys_yes_no_type；0=否 1=是）"),

            // entity.setting.isencrypted
            new TranslationSeedItem("entity.setting.isencrypted", "en-US", "加密_us", "加密（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isencrypted
            new TranslationSeedItem("entity.setting.isencrypted", "ja-JP", "加密_jp", "加密（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isencrypted
            new TranslationSeedItem("entity.setting.isencrypted", "zh-CN", "加密", "加密（字典 sys_yes_no_type；0=否 1=是）"),
            // entity.setting.isencrypted
            new TranslationSeedItem("entity.setting.isencrypted", "zh-HK", "加密_hk", "加密（字典 sys_yes_no_type；0=否 1=是）"),

            // entity.setting.sortorder
            new TranslationSeedItem("entity.setting.sortorder", "en-US", "排序号_us", "排序号"),
            // entity.setting.sortorder
            new TranslationSeedItem("entity.setting.sortorder", "ja-JP", "排序号_jp", "排序号"),
            // entity.setting.sortorder
            new TranslationSeedItem("entity.setting.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.setting.sortorder
            new TranslationSeedItem("entity.setting.sortorder", "zh-HK", "排序号_hk", "排序号"),

            // entity.setting.status
            new TranslationSeedItem("entity.setting.status", "en-US", "状态_us", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.setting.status
            new TranslationSeedItem("entity.setting.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.setting.status
            new TranslationSeedItem("entity.setting.status", "zh-CN", "状态", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
            // entity.setting.status
            new TranslationSeedItem("entity.setting.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable_status；1=启用 0=禁用）"),
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
