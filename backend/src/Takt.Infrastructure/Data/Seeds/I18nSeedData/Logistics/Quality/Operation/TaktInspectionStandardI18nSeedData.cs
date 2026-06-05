// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktInspectionStandard 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktInspectionStandard 实体国际化翻译种子（键前缀 entity.inspectionStandard.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktInspectionStandardI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktInspectionStandard 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 inspectionStandard 实体翻译...", tenantCode);

        foreach (var item in GetInspectionStandardTranslations())
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

        TaktLogger.Information("TaktInspectionStandard 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktInspectionStandard 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.inspectionStandard._self / entity.inspectionStandard.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetInspectionStandardTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.inspectionStandard._self
            new TranslationSeedItem("entity.inspectionStandard._self", "en-US", "Inspection Standard Information", "实体名称"),
            // entity.inspectionStandard._self
            new TranslationSeedItem("entity.inspectionStandard._self", "ja-JP", "检验标准信息", "实体名称"),
            // entity.inspectionStandard._self
            new TranslationSeedItem("entity.inspectionStandard._self", "zh-CN", "检验标准信息", "实体名称"),
            // entity.inspectionStandard._self
            new TranslationSeedItem("entity.inspectionStandard._self", "zh-HK", "检验标准信息", "实体名称"),

            // entity.inspectionStandard.plantcode
            new TranslationSeedItem("entity.inspectionStandard.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.inspectionStandard.plantcode
            new TranslationSeedItem("entity.inspectionStandard.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.inspectionStandard.plantcode
            new TranslationSeedItem("entity.inspectionStandard.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.inspectionStandard.plantcode
            new TranslationSeedItem("entity.inspectionStandard.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.inspectionStandard.standardcode
            new TranslationSeedItem("entity.inspectionStandard.standardcode", "en-US", "检验标准编码", "检验标准编码（唯一索引）"),
            // entity.inspectionStandard.standardcode
            new TranslationSeedItem("entity.inspectionStandard.standardcode", "ja-JP", "检验标准编码", "检验标准编码（唯一索引）"),
            // entity.inspectionStandard.standardcode
            new TranslationSeedItem("entity.inspectionStandard.standardcode", "zh-CN", "检验标准编码", "检验标准编码（唯一索引）"),
            // entity.inspectionStandard.standardcode
            new TranslationSeedItem("entity.inspectionStandard.standardcode", "zh-HK", "检验标准编码", "检验标准编码（唯一索引）"),

            // entity.inspectionStandard.standardname
            new TranslationSeedItem("entity.inspectionStandard.standardname", "en-US", "检验标准名称", "检验标准名称"),
            // entity.inspectionStandard.standardname
            new TranslationSeedItem("entity.inspectionStandard.standardname", "ja-JP", "检验标准名称", "检验标准名称"),
            // entity.inspectionStandard.standardname
            new TranslationSeedItem("entity.inspectionStandard.standardname", "zh-CN", "检验标准名称", "检验标准名称"),
            // entity.inspectionStandard.standardname
            new TranslationSeedItem("entity.inspectionStandard.standardname", "zh-HK", "检验标准名称", "检验标准名称"),

            // entity.inspectionStandard.inspectiontype
            new TranslationSeedItem("entity.inspectionStandard.inspectiontype", "en-US", "检验类型", "检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）"),
            // entity.inspectionStandard.inspectiontype
            new TranslationSeedItem("entity.inspectionStandard.inspectiontype", "ja-JP", "检验类型", "检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）"),
            // entity.inspectionStandard.inspectiontype
            new TranslationSeedItem("entity.inspectionStandard.inspectiontype", "zh-CN", "检验类型", "检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）"),
            // entity.inspectionStandard.inspectiontype
            new TranslationSeedItem("entity.inspectionStandard.inspectiontype", "zh-HK", "检验类型", "检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）"),

            // entity.inspectionStandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionStandard.materialcategorycode", "en-US", "物料类别编码", "物料类别编码"),
            // entity.inspectionStandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionStandard.materialcategorycode", "ja-JP", "物料类别编码", "物料类别编码"),
            // entity.inspectionStandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionStandard.materialcategorycode", "zh-CN", "物料类别编码", "物料类别编码"),
            // entity.inspectionStandard.materialcategorycode
            new TranslationSeedItem("entity.inspectionStandard.materialcategorycode", "zh-HK", "物料类别编码", "物料类别编码"),

            // entity.inspectionStandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionStandard.materialcategoryname", "en-US", "物料类别名称", "物料类别名称"),
            // entity.inspectionStandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionStandard.materialcategoryname", "ja-JP", "物料类别名称", "物料类别名称"),
            // entity.inspectionStandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionStandard.materialcategoryname", "zh-CN", "物料类别名称", "物料类别名称"),
            // entity.inspectionStandard.materialcategoryname
            new TranslationSeedItem("entity.inspectionStandard.materialcategoryname", "zh-HK", "物料类别名称", "物料类别名称"),

            // entity.inspectionStandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionStandard.samplingschemecode", "en-US", "抽样方案编码", "抽样方案编码"),
            // entity.inspectionStandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionStandard.samplingschemecode", "ja-JP", "抽样方案编码", "抽样方案编码"),
            // entity.inspectionStandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionStandard.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码"),
            // entity.inspectionStandard.samplingschemecode
            new TranslationSeedItem("entity.inspectionStandard.samplingschemecode", "zh-HK", "抽样方案编码", "抽样方案编码"),

            // entity.inspectionStandard.samplingschemename
            new TranslationSeedItem("entity.inspectionStandard.samplingschemename", "en-US", "抽样方案名称", "抽样方案名称"),
            // entity.inspectionStandard.samplingschemename
            new TranslationSeedItem("entity.inspectionStandard.samplingschemename", "ja-JP", "抽样方案名称", "抽样方案名称"),
            // entity.inspectionStandard.samplingschemename
            new TranslationSeedItem("entity.inspectionStandard.samplingschemename", "zh-CN", "抽样方案名称", "抽样方案名称"),
            // entity.inspectionStandard.samplingschemename
            new TranslationSeedItem("entity.inspectionStandard.samplingschemename", "zh-HK", "抽样方案名称", "抽样方案名称"),

            // entity.inspectionStandard.isenabled
            new TranslationSeedItem("entity.inspectionStandard.isenabled", "en-US", "是否启用", "是否启用（0=否，1=是）"),
            // entity.inspectionStandard.isenabled
            new TranslationSeedItem("entity.inspectionStandard.isenabled", "ja-JP", "是否启用", "是否启用（0=否，1=是）"),
            // entity.inspectionStandard.isenabled
            new TranslationSeedItem("entity.inspectionStandard.isenabled", "zh-CN", "是否启用", "是否启用（0=否，1=是）"),
            // entity.inspectionStandard.isenabled
            new TranslationSeedItem("entity.inspectionStandard.isenabled", "zh-HK", "是否启用", "是否启用（0=否，1=是）"),

            // entity.inspectionStandard.standardstatus
            new TranslationSeedItem("entity.inspectionStandard.standardstatus", "en-US", "检验标准状态", "检验标准状态（0=草稿，1=已发布，2=已停用）"),
            // entity.inspectionStandard.standardstatus
            new TranslationSeedItem("entity.inspectionStandard.standardstatus", "ja-JP", "检验标准状态", "检验标准状态（0=草稿，1=已发布，2=已停用）"),
            // entity.inspectionStandard.standardstatus
            new TranslationSeedItem("entity.inspectionStandard.standardstatus", "zh-CN", "检验标准状态", "检验标准状态（0=草稿，1=已发布，2=已停用）"),
            // entity.inspectionStandard.standardstatus
            new TranslationSeedItem("entity.inspectionStandard.standardstatus", "zh-HK", "检验标准状态", "检验标准状态（0=草稿，1=已发布，2=已停用）"),

            // entity.inspectionStandard.standarddescription
            new TranslationSeedItem("entity.inspectionStandard.standarddescription", "en-US", "检验标准描述", "检验标准描述"),
            // entity.inspectionStandard.standarddescription
            new TranslationSeedItem("entity.inspectionStandard.standarddescription", "ja-JP", "检验标准描述", "检验标准描述"),
            // entity.inspectionStandard.standarddescription
            new TranslationSeedItem("entity.inspectionStandard.standarddescription", "zh-CN", "检验标准描述", "检验标准描述"),
            // entity.inspectionStandard.standarddescription
            new TranslationSeedItem("entity.inspectionStandard.standarddescription", "zh-HK", "检验标准描述", "检验标准描述"),

            // entity.inspectionStandard.items
            new TranslationSeedItem("entity.inspectionStandard.items", "en-US", "items", "检验标准明细列表（主子表关系）"),
            // entity.inspectionStandard.items
            new TranslationSeedItem("entity.inspectionStandard.items", "ja-JP", "items", "检验标准明细列表（主子表关系）"),
            // entity.inspectionStandard.items
            new TranslationSeedItem("entity.inspectionStandard.items", "zh-CN", "items", "检验标准明细列表（主子表关系）"),
            // entity.inspectionStandard.items
            new TranslationSeedItem("entity.inspectionStandard.items", "zh-HK", "items", "检验标准明细列表（主子表关系）"),
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
