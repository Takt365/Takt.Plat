// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktInspectionStandardItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktInspectionStandardItem 实体国际化翻译种子（键前缀 entity.inspectionStandardItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktInspectionStandardItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktInspectionStandardItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 inspectionStandardItem 实体翻译...", tenantCode);

        foreach (var item in GetInspectionStandardItemTranslations())
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

        TaktLogger.Information("TaktInspectionStandardItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktInspectionStandardItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.inspectionStandardItem._self / entity.inspectionStandardItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetInspectionStandardItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.inspectionStandardItem._self
            new TranslationSeedItem("entity.inspectionStandardItem._self", "en-US", "Inspection Standard Item Information", "实体名称"),
            // entity.inspectionStandardItem._self
            new TranslationSeedItem("entity.inspectionStandardItem._self", "ja-JP", "检验标准明细信息", "实体名称"),
            // entity.inspectionStandardItem._self
            new TranslationSeedItem("entity.inspectionStandardItem._self", "zh-CN", "检验标准明细信息", "实体名称"),
            // entity.inspectionStandardItem._self
            new TranslationSeedItem("entity.inspectionStandardItem._self", "zh-HK", "检验标准明细信息", "实体名称"),

            // entity.inspectionStandardItem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionstandardid", "en-US", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionStandardItem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionstandardid", "ja-JP", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionStandardItem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionstandardid", "zh-CN", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionStandardItem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionstandardid", "zh-HK", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.inspectionStandardItem.linenumber
            new TranslationSeedItem("entity.inspectionStandardItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionStandardItem.linenumber
            new TranslationSeedItem("entity.inspectionStandardItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionStandardItem.linenumber
            new TranslationSeedItem("entity.inspectionStandardItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionStandardItem.linenumber
            new TranslationSeedItem("entity.inspectionStandardItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.inspectionStandardItem.itemcode
            new TranslationSeedItem("entity.inspectionStandardItem.itemcode", "en-US", "检验项目编码", "检验项目编码"),
            // entity.inspectionStandardItem.itemcode
            new TranslationSeedItem("entity.inspectionStandardItem.itemcode", "ja-JP", "检验项目编码", "检验项目编码"),
            // entity.inspectionStandardItem.itemcode
            new TranslationSeedItem("entity.inspectionStandardItem.itemcode", "zh-CN", "检验项目编码", "检验项目编码"),
            // entity.inspectionStandardItem.itemcode
            new TranslationSeedItem("entity.inspectionStandardItem.itemcode", "zh-HK", "检验项目编码", "检验项目编码"),

            // entity.inspectionStandardItem.itemname
            new TranslationSeedItem("entity.inspectionStandardItem.itemname", "en-US", "检验项目名称", "检验项目名称"),
            // entity.inspectionStandardItem.itemname
            new TranslationSeedItem("entity.inspectionStandardItem.itemname", "ja-JP", "检验项目名称", "检验项目名称"),
            // entity.inspectionStandardItem.itemname
            new TranslationSeedItem("entity.inspectionStandardItem.itemname", "zh-CN", "检验项目名称", "检验项目名称"),
            // entity.inspectionStandardItem.itemname
            new TranslationSeedItem("entity.inspectionStandardItem.itemname", "zh-HK", "检验项目名称", "检验项目名称"),

            // entity.inspectionStandardItem.itemtype
            new TranslationSeedItem("entity.inspectionStandardItem.itemtype", "en-US", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionStandardItem.itemtype
            new TranslationSeedItem("entity.inspectionStandardItem.itemtype", "ja-JP", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionStandardItem.itemtype
            new TranslationSeedItem("entity.inspectionStandardItem.itemtype", "zh-CN", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionStandardItem.itemtype
            new TranslationSeedItem("entity.inspectionStandardItem.itemtype", "zh-HK", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),

            // entity.inspectionStandardItem.defectlevel
            new TranslationSeedItem("entity.inspectionStandardItem.defectlevel", "en-US", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionStandardItem.defectlevel
            new TranslationSeedItem("entity.inspectionStandardItem.defectlevel", "ja-JP", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionStandardItem.defectlevel
            new TranslationSeedItem("entity.inspectionStandardItem.defectlevel", "zh-CN", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionStandardItem.defectlevel
            new TranslationSeedItem("entity.inspectionStandardItem.defectlevel", "zh-HK", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),

            // entity.inspectionStandardItem.inspectionmode
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmode", "en-US", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionStandardItem.inspectionmode
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmode", "ja-JP", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionStandardItem.inspectionmode
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmode", "zh-CN", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionStandardItem.inspectionmode
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmode", "zh-HK", "检验方式", "检验方式（1=计数，2=计量）"),

            // entity.inspectionStandardItem.standardvalue
            new TranslationSeedItem("entity.inspectionStandardItem.standardvalue", "en-US", "检验标准值", "检验标准值"),
            // entity.inspectionStandardItem.standardvalue
            new TranslationSeedItem("entity.inspectionStandardItem.standardvalue", "ja-JP", "检验标准值", "检验标准值"),
            // entity.inspectionStandardItem.standardvalue
            new TranslationSeedItem("entity.inspectionStandardItem.standardvalue", "zh-CN", "检验标准值", "检验标准值"),
            // entity.inspectionStandardItem.standardvalue
            new TranslationSeedItem("entity.inspectionStandardItem.standardvalue", "zh-HK", "检验标准值", "检验标准值"),

            // entity.inspectionStandardItem.upperlimit
            new TranslationSeedItem("entity.inspectionStandardItem.upperlimit", "en-US", "检验上限值", "检验上限值"),
            // entity.inspectionStandardItem.upperlimit
            new TranslationSeedItem("entity.inspectionStandardItem.upperlimit", "ja-JP", "检验上限值", "检验上限值"),
            // entity.inspectionStandardItem.upperlimit
            new TranslationSeedItem("entity.inspectionStandardItem.upperlimit", "zh-CN", "检验上限值", "检验上限值"),
            // entity.inspectionStandardItem.upperlimit
            new TranslationSeedItem("entity.inspectionStandardItem.upperlimit", "zh-HK", "检验上限值", "检验上限值"),

            // entity.inspectionStandardItem.lowerlimit
            new TranslationSeedItem("entity.inspectionStandardItem.lowerlimit", "en-US", "检验下限值", "检验下限值"),
            // entity.inspectionStandardItem.lowerlimit
            new TranslationSeedItem("entity.inspectionStandardItem.lowerlimit", "ja-JP", "检验下限值", "检验下限值"),
            // entity.inspectionStandardItem.lowerlimit
            new TranslationSeedItem("entity.inspectionStandardItem.lowerlimit", "zh-CN", "检验下限值", "检验下限值"),
            // entity.inspectionStandardItem.lowerlimit
            new TranslationSeedItem("entity.inspectionStandardItem.lowerlimit", "zh-HK", "检验下限值", "检验下限值"),

            // entity.inspectionStandardItem.inspectiontool
            new TranslationSeedItem("entity.inspectionStandardItem.inspectiontool", "en-US", "检验工具", "检验工具"),
            // entity.inspectionStandardItem.inspectiontool
            new TranslationSeedItem("entity.inspectionStandardItem.inspectiontool", "ja-JP", "检验工具", "检验工具"),
            // entity.inspectionStandardItem.inspectiontool
            new TranslationSeedItem("entity.inspectionStandardItem.inspectiontool", "zh-CN", "检验工具", "检验工具"),
            // entity.inspectionStandardItem.inspectiontool
            new TranslationSeedItem("entity.inspectionStandardItem.inspectiontool", "zh-HK", "检验工具", "检验工具"),

            // entity.inspectionStandardItem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmethoddescription", "en-US", "检验方法说明", "检验方法说明"),
            // entity.inspectionStandardItem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmethoddescription", "ja-JP", "检验方法说明", "检验方法说明"),
            // entity.inspectionStandardItem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmethoddescription", "zh-CN", "检验方法说明", "检验方法说明"),
            // entity.inspectionStandardItem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionStandardItem.inspectionmethoddescription", "zh-HK", "检验方法说明", "检验方法说明"),

            // entity.inspectionStandardItem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionStandardItem.acceptancecriteria", "en-US", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionStandardItem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionStandardItem.acceptancecriteria", "ja-JP", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionStandardItem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionStandardItem.acceptancecriteria", "zh-CN", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionStandardItem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionStandardItem.acceptancecriteria", "zh-HK", "接收标准(AC值)", "接收标准（AC值）"),

            // entity.inspectionStandardItem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionStandardItem.rejectioncriteria", "en-US", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionStandardItem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionStandardItem.rejectioncriteria", "ja-JP", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionStandardItem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionStandardItem.rejectioncriteria", "zh-CN", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionStandardItem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionStandardItem.rejectioncriteria", "zh-HK", "拒收标准(RE值)", "拒收标准（RE值）"),

            // entity.inspectionStandardItem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionStandardItem.isqualifiedbasis", "en-US", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionStandardItem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionStandardItem.isqualifiedbasis", "ja-JP", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionStandardItem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionStandardItem.isqualifiedbasis", "zh-CN", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionStandardItem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionStandardItem.isqualifiedbasis", "zh-HK", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),

            // entity.inspectionStandardItem.standard
            new TranslationSeedItem("entity.inspectionStandardItem.standard", "en-US", "检验标准", "检验标准（主表）"),
            // entity.inspectionStandardItem.standard
            new TranslationSeedItem("entity.inspectionStandardItem.standard", "ja-JP", "检验标准", "检验标准（主表）"),
            // entity.inspectionStandardItem.standard
            new TranslationSeedItem("entity.inspectionStandardItem.standard", "zh-CN", "检验标准", "检验标准（主表）"),
            // entity.inspectionStandardItem.standard
            new TranslationSeedItem("entity.inspectionStandardItem.standard", "zh-HK", "检验标准", "检验标准（主表）"),
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
