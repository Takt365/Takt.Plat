// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktManufacturerMaterialI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktManufacturerMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktManufacturerMaterial 实体国际化翻译种子（键前缀 entity.manufacturerMaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktManufacturerMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktManufacturerMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 manufacturerMaterial 实体翻译...", tenantCode);

        foreach (var item in GetManufacturerMaterialTranslations())
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

        TaktLogger.Information("TaktManufacturerMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktManufacturerMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.manufacturerMaterial._self / entity.manufacturerMaterial.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetManufacturerMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.manufacturerMaterial._self
            new TranslationSeedItem("entity.manufacturerMaterial._self", "en-US", "Manufacturer Material Information", "实体名称"),
            // entity.manufacturerMaterial._self
            new TranslationSeedItem("entity.manufacturerMaterial._self", "ja-JP", "Takt制造商物料明细信息", "实体名称"),
            // entity.manufacturerMaterial._self
            new TranslationSeedItem("entity.manufacturerMaterial._self", "zh-CN", "Takt制造商物料明细信息", "实体名称"),
            // entity.manufacturerMaterial._self
            new TranslationSeedItem("entity.manufacturerMaterial._self", "zh-HK", "Takt制造商物料明细信息", "实体名称"),

            // entity.manufacturerMaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturerid", "en-US", "制造商ID", "制造商ID（关联TaktManufacturer主表）"),
            // entity.manufacturerMaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturerid", "ja-JP", "制造商ID", "制造商ID（关联TaktManufacturer主表）"),
            // entity.manufacturerMaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturerid", "zh-CN", "制造商ID", "制造商ID（关联TaktManufacturer主表）"),
            // entity.manufacturerMaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturerid", "zh-HK", "制造商ID", "制造商ID（关联TaktManufacturer主表）"),

            // entity.manufacturerMaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturercode", "en-US", "制造商编码", "制造商编码（冗余字段，便于查询）"),
            // entity.manufacturerMaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturercode", "ja-JP", "制造商编码", "制造商编码（冗余字段，便于查询）"),
            // entity.manufacturerMaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturercode", "zh-CN", "制造商编码", "制造商编码（冗余字段，便于查询）"),
            // entity.manufacturerMaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturerMaterial.manufacturercode", "zh-HK", "制造商编码", "制造商编码（冗余字段，便于查询）"),

            // entity.manufacturerMaterial.linenumber
            new TranslationSeedItem("entity.manufacturerMaterial.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturerMaterial.linenumber
            new TranslationSeedItem("entity.manufacturerMaterial.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturerMaterial.linenumber
            new TranslationSeedItem("entity.manufacturerMaterial.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturerMaterial.linenumber
            new TranslationSeedItem("entity.manufacturerMaterial.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.manufacturerMaterial.materialtype
            new TranslationSeedItem("entity.manufacturerMaterial.materialtype", "en-US", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.manufacturerMaterial.materialtype
            new TranslationSeedItem("entity.manufacturerMaterial.materialtype", "ja-JP", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.manufacturerMaterial.materialtype
            new TranslationSeedItem("entity.manufacturerMaterial.materialtype", "zh-CN", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),
            // entity.manufacturerMaterial.materialtype
            new TranslationSeedItem("entity.manufacturerMaterial.materialtype", "zh-HK", "物料类型", "物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）"),

            // entity.manufacturerMaterial.code
            new TranslationSeedItem("entity.manufacturerMaterial.code", "en-US", "制造商物料编码", "制造商物料编码（制造商内部的物料编号）"),
            // entity.manufacturerMaterial.code
            new TranslationSeedItem("entity.manufacturerMaterial.code", "ja-JP", "制造商物料编码", "制造商物料编码（制造商内部的物料编号）"),
            // entity.manufacturerMaterial.code
            new TranslationSeedItem("entity.manufacturerMaterial.code", "zh-CN", "制造商物料编码", "制造商物料编码（制造商内部的物料编号）"),
            // entity.manufacturerMaterial.code
            new TranslationSeedItem("entity.manufacturerMaterial.code", "zh-HK", "制造商物料编码", "制造商物料编码（制造商内部的物料编号）"),

            // entity.manufacturerMaterial.name
            new TranslationSeedItem("entity.manufacturerMaterial.name", "en-US", "制造商物料名称", "制造商物料名称（制造商内部的物料名称）"),
            // entity.manufacturerMaterial.name
            new TranslationSeedItem("entity.manufacturerMaterial.name", "ja-JP", "制造商物料名称", "制造商物料名称（制造商内部的物料名称）"),
            // entity.manufacturerMaterial.name
            new TranslationSeedItem("entity.manufacturerMaterial.name", "zh-CN", "制造商物料名称", "制造商物料名称（制造商内部的物料名称）"),
            // entity.manufacturerMaterial.name
            new TranslationSeedItem("entity.manufacturerMaterial.name", "zh-HK", "制造商物料名称", "制造商物料名称（制造商内部的物料名称）"),

            // entity.manufacturerMaterial.specification
            new TranslationSeedItem("entity.manufacturerMaterial.specification", "en-US", "制造商物料规格", "制造商物料规格"),
            // entity.manufacturerMaterial.specification
            new TranslationSeedItem("entity.manufacturerMaterial.specification", "ja-JP", "制造商物料规格", "制造商物料规格"),
            // entity.manufacturerMaterial.specification
            new TranslationSeedItem("entity.manufacturerMaterial.specification", "zh-CN", "制造商物料规格", "制造商物料规格"),
            // entity.manufacturerMaterial.specification
            new TranslationSeedItem("entity.manufacturerMaterial.specification", "zh-HK", "制造商物料规格", "制造商物料规格"),

            // entity.manufacturerMaterial.materialcode
            new TranslationSeedItem("entity.manufacturerMaterial.materialcode", "en-US", "物料编码", "物料编码（对应的内部物料编码）"),
            // entity.manufacturerMaterial.materialcode
            new TranslationSeedItem("entity.manufacturerMaterial.materialcode", "ja-JP", "物料编码", "物料编码（对应的内部物料编码）"),
            // entity.manufacturerMaterial.materialcode
            new TranslationSeedItem("entity.manufacturerMaterial.materialcode", "zh-CN", "物料编码", "物料编码（对应的内部物料编码）"),
            // entity.manufacturerMaterial.materialcode
            new TranslationSeedItem("entity.manufacturerMaterial.materialcode", "zh-HK", "物料编码", "物料编码（对应的内部物料编码）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
