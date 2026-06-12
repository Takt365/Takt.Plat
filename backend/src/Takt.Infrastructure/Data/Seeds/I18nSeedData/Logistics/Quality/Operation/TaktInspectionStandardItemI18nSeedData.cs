// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktInspectionStandardItem 实体国际化翻译种子（键前缀 entity.inspectionstandarditem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 inspectionstandarditem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.inspectionstandarditem._self / entity.inspectionstandarditem.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetInspectionStandardItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.inspectionstandarditem._self
            new TranslationSeedItem("entity.inspectionstandarditem._self", "en-US", "Inspection Standard Item Information", "实体名称"),
            // entity.inspectionstandarditem._self
            new TranslationSeedItem("entity.inspectionstandarditem._self", "ja-JP", "检验标准明细信息", "实体名称"),
            // entity.inspectionstandarditem._self
            new TranslationSeedItem("entity.inspectionstandarditem._self", "zh-CN", "检验标准明细信息", "实体名称"),
            // entity.inspectionstandarditem._self
            new TranslationSeedItem("entity.inspectionstandarditem._self", "zh-HK", "检验标准明细信息", "实体名称"),

            // entity.inspectionstandarditem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionstandardid", "en-US", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionstandarditem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionstandardid", "ja-JP", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionstandarditem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionstandardid", "zh-CN", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.inspectionstandarditem.inspectionstandardid
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionstandardid", "zh-HK", "检验标准ID", "检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.inspectionstandarditem.linenumber
            new TranslationSeedItem("entity.inspectionstandarditem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionstandarditem.linenumber
            new TranslationSeedItem("entity.inspectionstandarditem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionstandarditem.linenumber
            new TranslationSeedItem("entity.inspectionstandarditem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.inspectionstandarditem.linenumber
            new TranslationSeedItem("entity.inspectionstandarditem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.inspectionstandarditem.itemcode
            new TranslationSeedItem("entity.inspectionstandarditem.itemcode", "en-US", "检验项目编码", "检验项目编码"),
            // entity.inspectionstandarditem.itemcode
            new TranslationSeedItem("entity.inspectionstandarditem.itemcode", "ja-JP", "检验项目编码", "检验项目编码"),
            // entity.inspectionstandarditem.itemcode
            new TranslationSeedItem("entity.inspectionstandarditem.itemcode", "zh-CN", "检验项目编码", "检验项目编码"),
            // entity.inspectionstandarditem.itemcode
            new TranslationSeedItem("entity.inspectionstandarditem.itemcode", "zh-HK", "检验项目编码", "检验项目编码"),

            // entity.inspectionstandarditem.itemname
            new TranslationSeedItem("entity.inspectionstandarditem.itemname", "en-US", "检验项目名称", "检验项目名称"),
            // entity.inspectionstandarditem.itemname
            new TranslationSeedItem("entity.inspectionstandarditem.itemname", "ja-JP", "检验项目名称", "检验项目名称"),
            // entity.inspectionstandarditem.itemname
            new TranslationSeedItem("entity.inspectionstandarditem.itemname", "zh-CN", "检验项目名称", "检验项目名称"),
            // entity.inspectionstandarditem.itemname
            new TranslationSeedItem("entity.inspectionstandarditem.itemname", "zh-HK", "检验项目名称", "检验项目名称"),

            // entity.inspectionstandarditem.itemtype
            new TranslationSeedItem("entity.inspectionstandarditem.itemtype", "en-US", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionstandarditem.itemtype
            new TranslationSeedItem("entity.inspectionstandarditem.itemtype", "ja-JP", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionstandarditem.itemtype
            new TranslationSeedItem("entity.inspectionstandarditem.itemtype", "zh-CN", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),
            // entity.inspectionstandarditem.itemtype
            new TranslationSeedItem("entity.inspectionstandarditem.itemtype", "zh-HK", "检验项目类型", "检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）"),

            // entity.inspectionstandarditem.defectlevel
            new TranslationSeedItem("entity.inspectionstandarditem.defectlevel", "en-US", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionstandarditem.defectlevel
            new TranslationSeedItem("entity.inspectionstandarditem.defectlevel", "ja-JP", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionstandarditem.defectlevel
            new TranslationSeedItem("entity.inspectionstandarditem.defectlevel", "zh-CN", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.inspectionstandarditem.defectlevel
            new TranslationSeedItem("entity.inspectionstandarditem.defectlevel", "zh-HK", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),

            // entity.inspectionstandarditem.inspectionmode
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmode", "en-US", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionstandarditem.inspectionmode
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmode", "ja-JP", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionstandarditem.inspectionmode
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmode", "zh-CN", "检验方式", "检验方式（1=计数，2=计量）"),
            // entity.inspectionstandarditem.inspectionmode
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmode", "zh-HK", "检验方式", "检验方式（1=计数，2=计量）"),

            // entity.inspectionstandarditem.standardvalue
            new TranslationSeedItem("entity.inspectionstandarditem.standardvalue", "en-US", "检验标准值", "检验标准值"),
            // entity.inspectionstandarditem.standardvalue
            new TranslationSeedItem("entity.inspectionstandarditem.standardvalue", "ja-JP", "检验标准值", "检验标准值"),
            // entity.inspectionstandarditem.standardvalue
            new TranslationSeedItem("entity.inspectionstandarditem.standardvalue", "zh-CN", "检验标准值", "检验标准值"),
            // entity.inspectionstandarditem.standardvalue
            new TranslationSeedItem("entity.inspectionstandarditem.standardvalue", "zh-HK", "检验标准值", "检验标准值"),

            // entity.inspectionstandarditem.upperlimit
            new TranslationSeedItem("entity.inspectionstandarditem.upperlimit", "en-US", "检验上限值", "检验上限值"),
            // entity.inspectionstandarditem.upperlimit
            new TranslationSeedItem("entity.inspectionstandarditem.upperlimit", "ja-JP", "检验上限值", "检验上限值"),
            // entity.inspectionstandarditem.upperlimit
            new TranslationSeedItem("entity.inspectionstandarditem.upperlimit", "zh-CN", "检验上限值", "检验上限值"),
            // entity.inspectionstandarditem.upperlimit
            new TranslationSeedItem("entity.inspectionstandarditem.upperlimit", "zh-HK", "检验上限值", "检验上限值"),

            // entity.inspectionstandarditem.lowerlimit
            new TranslationSeedItem("entity.inspectionstandarditem.lowerlimit", "en-US", "检验下限值", "检验下限值"),
            // entity.inspectionstandarditem.lowerlimit
            new TranslationSeedItem("entity.inspectionstandarditem.lowerlimit", "ja-JP", "检验下限值", "检验下限值"),
            // entity.inspectionstandarditem.lowerlimit
            new TranslationSeedItem("entity.inspectionstandarditem.lowerlimit", "zh-CN", "检验下限值", "检验下限值"),
            // entity.inspectionstandarditem.lowerlimit
            new TranslationSeedItem("entity.inspectionstandarditem.lowerlimit", "zh-HK", "检验下限值", "检验下限值"),

            // entity.inspectionstandarditem.inspectiontool
            new TranslationSeedItem("entity.inspectionstandarditem.inspectiontool", "en-US", "检验工具", "检验工具"),
            // entity.inspectionstandarditem.inspectiontool
            new TranslationSeedItem("entity.inspectionstandarditem.inspectiontool", "ja-JP", "检验工具", "检验工具"),
            // entity.inspectionstandarditem.inspectiontool
            new TranslationSeedItem("entity.inspectionstandarditem.inspectiontool", "zh-CN", "检验工具", "检验工具"),
            // entity.inspectionstandarditem.inspectiontool
            new TranslationSeedItem("entity.inspectionstandarditem.inspectiontool", "zh-HK", "检验工具", "检验工具"),

            // entity.inspectionstandarditem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmethoddescription", "en-US", "检验方法说明", "检验方法说明"),
            // entity.inspectionstandarditem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmethoddescription", "ja-JP", "检验方法说明", "检验方法说明"),
            // entity.inspectionstandarditem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmethoddescription", "zh-CN", "检验方法说明", "检验方法说明"),
            // entity.inspectionstandarditem.inspectionmethoddescription
            new TranslationSeedItem("entity.inspectionstandarditem.inspectionmethoddescription", "zh-HK", "检验方法说明", "检验方法说明"),

            // entity.inspectionstandarditem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionstandarditem.acceptancecriteria", "en-US", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionstandarditem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionstandarditem.acceptancecriteria", "ja-JP", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionstandarditem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionstandarditem.acceptancecriteria", "zh-CN", "接收标准(AC值)", "接收标准（AC值）"),
            // entity.inspectionstandarditem.acceptancecriteria
            new TranslationSeedItem("entity.inspectionstandarditem.acceptancecriteria", "zh-HK", "接收标准(AC值)", "接收标准（AC值）"),

            // entity.inspectionstandarditem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionstandarditem.rejectioncriteria", "en-US", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionstandarditem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionstandarditem.rejectioncriteria", "ja-JP", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionstandarditem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionstandarditem.rejectioncriteria", "zh-CN", "拒收标准(RE值)", "拒收标准（RE值）"),
            // entity.inspectionstandarditem.rejectioncriteria
            new TranslationSeedItem("entity.inspectionstandarditem.rejectioncriteria", "zh-HK", "拒收标准(RE值)", "拒收标准（RE值）"),

            // entity.inspectionstandarditem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionstandarditem.isqualifiedbasis", "en-US", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionstandarditem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionstandarditem.isqualifiedbasis", "ja-JP", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionstandarditem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionstandarditem.isqualifiedbasis", "zh-CN", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),
            // entity.inspectionstandarditem.isqualifiedbasis
            new TranslationSeedItem("entity.inspectionstandarditem.isqualifiedbasis", "zh-HK", "是否合格判定项目", "是否合格判定项目（0=否，1=是）"),

            // entity.inspectionstandarditem.standard
            new TranslationSeedItem("entity.inspectionstandarditem.standard", "en-US", "检验标准", "检验标准（主表）"),
            // entity.inspectionstandarditem.standard
            new TranslationSeedItem("entity.inspectionstandarditem.standard", "ja-JP", "检验标准", "检验标准（主表）"),
            // entity.inspectionstandarditem.standard
            new TranslationSeedItem("entity.inspectionstandarditem.standard", "zh-CN", "检验标准", "检验标准（主表）"),
            // entity.inspectionstandarditem.standard
            new TranslationSeedItem("entity.inspectionstandarditem.standard", "zh-HK", "检验标准", "检验标准（主表）"),
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
        translation.ResourceGroup = 4;
        translation.ResourceType = 0;
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
