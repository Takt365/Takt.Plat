// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterial 实体国际化翻译种子（键前缀 entity.material.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 material 实体翻译...", tenantCode);

        foreach (var item in GetMaterialTranslations())
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

        TaktLogger.Information("TaktMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.material._self / entity.material.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "en-US", "Material Information_us", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "ja-JP", "Takt全局物料信息_jp", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-CN", "Takt全局物料信息", "实体名称"),
            // entity.material._self
            new TranslationSeedItem("entity.material._self", "zh-HK", "Takt全局物料信息_hk", "实体名称"),

            // entity.material.code
            new TranslationSeedItem("entity.material.code", "en-US", "物料编码_us", "物料编码（租户内唯一）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "ja-JP", "物料编码_jp", "物料编码（租户内唯一）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-CN", "物料编码", "物料编码（租户内唯一）"),
            // entity.material.code
            new TranslationSeedItem("entity.material.code", "zh-HK", "物料编码_hk", "物料编码（租户内唯一）"),

            // entity.material.name
            new TranslationSeedItem("entity.material.name", "en-US", "物料名称_us", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "zh-CN", "物料名称", "物料名称"),
            // entity.material.name
            new TranslationSeedItem("entity.material.name", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "en-US", "物料规格_us", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "zh-CN", "物料规格", "物料规格"),
            // entity.material.specification
            new TranslationSeedItem("entity.material.specification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.material.description
            new TranslationSeedItem("entity.material.description", "en-US", "物料描述_us", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "ja-JP", "物料描述_jp", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "zh-CN", "物料描述", "物料描述"),
            // entity.material.description
            new TranslationSeedItem("entity.material.description", "zh-HK", "物料描述_hk", "物料描述"),

            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "en-US", "行业领域_us", "行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "ja-JP", "行业领域_jp", "行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-CN", "行业领域", "行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.material.industrysector
            new TranslationSeedItem("entity.material.industrysector", "zh-HK", "行业领域_hk", "行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),

            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "en-US", "物料层级_us", "物料层级"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "ja-JP", "物料层级_jp", "物料层级"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "zh-CN", "物料层级", "物料层级"),
            // entity.material.hierarchy
            new TranslationSeedItem("entity.material.hierarchy", "zh-HK", "物料层级_hk", "物料层级"),

            // entity.material.group
            new TranslationSeedItem("entity.material.group", "en-US", "物料组_us", "物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "ja-JP", "物料组_jp", "物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "zh-CN", "物料组", "物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）"),
            // entity.material.group
            new TranslationSeedItem("entity.material.group", "zh-HK", "物料组_hk", "物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）"),

            // entity.material.type
            new TranslationSeedItem("entity.material.type", "en-US", "物料类型_us", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-CN", "物料类型", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.material.type
            new TranslationSeedItem("entity.material.type", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）"),

            // entity.material.model
            new TranslationSeedItem("entity.material.model", "en-US", "物料型号_us", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "ja-JP", "物料型号_jp", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "zh-CN", "物料型号", "物料型号"),
            // entity.material.model
            new TranslationSeedItem("entity.material.model", "zh-HK", "物料型号_hk", "物料型号"),

            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "en-US", "物料品牌_us", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "ja-JP", "物料品牌_jp", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "zh-CN", "物料品牌", "物料品牌"),
            // entity.material.brand
            new TranslationSeedItem("entity.material.brand", "zh-HK", "物料品牌_hk", "物料品牌"),

            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "en-US", "基本单位_us", "基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "ja-JP", "基本单位_jp", "基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-CN", "基本单位", "基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.material.baseunit
            new TranslationSeedItem("entity.material.baseunit", "zh-HK", "基本单位_hk", "基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),

            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "en-US", "制造商_us", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "ja-JP", "制造商_jp", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "zh-CN", "制造商", "制造商"),
            // entity.material.manufacturer
            new TranslationSeedItem("entity.material.manufacturer", "zh-HK", "制造商_hk", "制造商"),

            // entity.material.manufacturermaterialcode
            new TranslationSeedItem("entity.material.manufacturermaterialcode", "en-US", "制造商物料编码_us", "制造商物料编码（制造商内部的物料编号）"),
            // entity.material.manufacturermaterialcode
            new TranslationSeedItem("entity.material.manufacturermaterialcode", "ja-JP", "制造商物料编码_jp", "制造商物料编码（制造商内部的物料编号）"),
            // entity.material.manufacturermaterialcode
            new TranslationSeedItem("entity.material.manufacturermaterialcode", "zh-CN", "制造商物料编码", "制造商物料编码（制造商内部的物料编号）"),
            // entity.material.manufacturermaterialcode
            new TranslationSeedItem("entity.material.manufacturermaterialcode", "zh-HK", "制造商物料编码_hk", "制造商物料编码（制造商内部的物料编号）"),

            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "en-US", "物料属性_us", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "ja-JP", "物料属性_jp", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "zh-CN", "物料属性", "物料属性（JSON格式，存储物料自定义属性）"),
            // entity.material.attributes
            new TranslationSeedItem("entity.material.attributes", "zh-HK", "物料属性_hk", "物料属性（JSON格式，存储物料自定义属性）"),

            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "en-US", "停产状态_us", "停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "ja-JP", "停产状态_jp", "停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "zh-CN", "停产状态", "停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.material.isendoflife
            new TranslationSeedItem("entity.material.isendoflife", "zh-HK", "停产状态_hk", "停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）"),

            // entity.material.status
            new TranslationSeedItem("entity.material.status", "en-US", "物料状态_us", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "ja-JP", "物料状态_jp", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-CN", "物料状态", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）"),
            // entity.material.status
            new TranslationSeedItem("entity.material.status", "zh-HK", "物料状态_hk", "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）"),

            // entity.material.changelogs
            new TranslationSeedItem("entity.material.changelogs", "en-US", "全局物料变更记录列表_us", "全局物料变更记录列表（外键在子表 TaktMaterialChangeLog.MaterialId）"),
            // entity.material.changelogs
            new TranslationSeedItem("entity.material.changelogs", "ja-JP", "全局物料变更记录列表_jp", "全局物料变更记录列表（外键在子表 TaktMaterialChangeLog.MaterialId）"),
            // entity.material.changelogs
            new TranslationSeedItem("entity.material.changelogs", "zh-CN", "全局物料变更记录列表", "全局物料变更记录列表（外键在子表 TaktMaterialChangeLog.MaterialId）"),
            // entity.material.changelogs
            new TranslationSeedItem("entity.material.changelogs", "zh-HK", "全局物料变更记录列表_hk", "全局物料变更记录列表（外键在子表 TaktMaterialChangeLog.MaterialId）"),
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
        translation.ResourceGroup = "Materials";
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
