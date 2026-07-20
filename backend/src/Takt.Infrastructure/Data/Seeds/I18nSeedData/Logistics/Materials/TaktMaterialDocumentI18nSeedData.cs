// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialDocumentI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialDocument 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialDocument 实体国际化翻译种子（键前缀 entity.materialdocument.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialDocumentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialDocument 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialdocument 实体翻译...", tenantCode);

        foreach (var item in GetMaterialDocumentTranslations())
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

        TaktLogger.Information("TaktMaterialDocument 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialDocument 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialdocument._self / entity.materialdocument.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialDocumentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "en-US", "Material Document Information_us", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "ja-JP", "Takt物料凭证主表信息_jp", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "zh-CN", "Takt物料凭证主表信息", "实体名称"),
            // entity.materialdocument._self
            new TranslationSeedItem("entity.materialdocument._self", "zh-HK", "Takt物料凭证主表信息_hk", "实体名称"),

            // entity.materialdocument.plantcode
            new TranslationSeedItem("entity.materialdocument.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.materialdocument.plantcode
            new TranslationSeedItem("entity.materialdocument.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.materialdocument.plantcode
            new TranslationSeedItem("entity.materialdocument.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),
            // entity.materialdocument.plantcode
            new TranslationSeedItem("entity.materialdocument.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）"),

            // entity.materialdocument.materialcode
            new TranslationSeedItem("entity.materialdocument.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocument.materialcode
            new TranslationSeedItem("entity.materialdocument.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocument.materialcode
            new TranslationSeedItem("entity.materialdocument.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocument.materialcode
            new TranslationSeedItem("entity.materialdocument.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "en-US", "物料凭证号_us", "物料凭证号（租户+公司+工厂内唯一）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "ja-JP", "物料凭证号_jp", "物料凭证号（租户+公司+工厂内唯一）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "zh-CN", "物料凭证号", "物料凭证号（租户+公司+工厂内唯一）"),
            // entity.materialdocument.code
            new TranslationSeedItem("entity.materialdocument.code", "zh-HK", "物料凭证号_hk", "物料凭证号（租户+公司+工厂内唯一）"),

            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "en-US", "过账人_us", "过账人（选项 TaktEmployees/options，DictValue=EmployeeNo）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "ja-JP", "过账人_jp", "过账人（选项 TaktEmployees/options，DictValue=EmployeeNo）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "zh-CN", "过账人", "过账人（选项 TaktEmployees/options，DictValue=EmployeeNo）"),
            // entity.materialdocument.postedby
            new TranslationSeedItem("entity.materialdocument.postedby", "zh-HK", "过账人_hk", "过账人（选项 TaktEmployees/options，DictValue=EmployeeNo）"),

            // entity.materialdocument.status
            new TranslationSeedItem("entity.materialdocument.status", "en-US", "物料凭证状态_us", "物料凭证状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialdocument.status
            new TranslationSeedItem("entity.materialdocument.status", "ja-JP", "物料凭证状态_jp", "物料凭证状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialdocument.status
            new TranslationSeedItem("entity.materialdocument.status", "zh-CN", "物料凭证状态", "物料凭证状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialdocument.status
            new TranslationSeedItem("entity.materialdocument.status", "zh-HK", "物料凭证状态_hk", "物料凭证状态（0=草稿，1=已过账，2=已作废）"),

            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "en-US", "物料凭证行项目列表_us", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "ja-JP", "物料凭证行项目列表_jp", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "zh-CN", "物料凭证行项目列表", "物料凭证行项目列表（主子表关系）"),
            // entity.materialdocument.items
            new TranslationSeedItem("entity.materialdocument.items", "zh-HK", "物料凭证行项目列表_hk", "物料凭证行项目列表（主子表关系）"),
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
