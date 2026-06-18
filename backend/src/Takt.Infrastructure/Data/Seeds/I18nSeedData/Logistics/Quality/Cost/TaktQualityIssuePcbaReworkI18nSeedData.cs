// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityFailurePcbaReworkI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityFailurePcbaRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityFailurePcbaRework 实体国际化翻译种子（键前缀 entity.qualityfailurepcbarework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityFailurePcbaReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityFailurePcbaRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityfailurepcbarework 实体翻译...", tenantCode);

        foreach (var item in GetQualityFailurePcbaReworkTranslations())
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

        TaktLogger.Information("TaktQualityFailurePcbaRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityFailurePcbaRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityfailurepcbarework._self / entity.qualityfailurepcbarework.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetQualityFailurePcbaReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityfailurepcbarework._self
            new TranslationSeedItem("entity.qualityfailurepcbarework._self", "en-US", "Quality Failure Pcba Rework Information", "实体名称"),
            // entity.qualityfailurepcbarework._self
            new TranslationSeedItem("entity.qualityfailurepcbarework._self", "ja-JP", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),
            // entity.qualityfailurepcbarework._self
            new TranslationSeedItem("entity.qualityfailurepcbarework._self", "zh-CN", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),
            // entity.qualityfailurepcbarework._self
            new TranslationSeedItem("entity.qualityfailurepcbarework._self", "zh-HK", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),

            // entity.qualityfailurepcbarework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailureid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailurepcbarework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailureid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailurepcbarework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailureid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailurepcbarework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailureid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityfailurepcbarework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailurecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailurepcbarework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailurecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailurepcbarework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailurecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailurepcbarework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailurepcbarework.qualityfailurecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityfailurepcbarework.linenumber
            new TranslationSeedItem("entity.qualityfailurepcbarework.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailurepcbarework.linenumber
            new TranslationSeedItem("entity.qualityfailurepcbarework.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailurepcbarework.linenumber
            new TranslationSeedItem("entity.qualityfailurepcbarework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailurepcbarework.linenumber
            new TranslationSeedItem("entity.qualityfailurepcbarework.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityfailurepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadefectparts", "en-US", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityfailurepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadefectparts", "ja-JP", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityfailurepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadefectparts", "zh-CN", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityfailurepcbarework.pcbadefectparts
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadefectparts", "zh-HK", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),

            // entity.qualityfailurepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworkcost", "en-US", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityfailurepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworkcost", "ja-JP", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityfailurepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworkcost", "zh-CN", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityfailurepcbarework.pcbareworkcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworkcost", "zh-HK", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),

            // entity.qualityfailurepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworktimeminutes", "en-US", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworktimeminutes", "ja-JP", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworktimeminutes", "zh-CN", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworktimeminutes", "zh-HK", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),

            // entity.qualityfailurepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareinspectiontimeminutes", "en-US", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareinspectiontimeminutes", "ja-JP", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareinspectiontimeminutes", "zh-CN", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityfailurepcbarework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareinspectiontimeminutes", "zh-HK", "PCBA再检查时间", "PCBA再检查时间（分钟）"),

            // entity.qualityfailurepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbatravelcost", "en-US", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityfailurepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbatravelcost", "ja-JP", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityfailurepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbatravelcost", "zh-CN", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityfailurepcbarework.pcbatravelcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbatravelcost", "zh-HK", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),

            // entity.qualityfailurepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbawarehousecost", "en-US", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityfailurepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbawarehousecost", "ja-JP", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityfailurepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbawarehousecost", "zh-CN", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityfailurepcbarework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbawarehousecost", "zh-HK", "PCBA仓库管理费", "PCBA仓库管理费（元）"),

            // entity.qualityfailurepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses", "en-US", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses", "ja-JP", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses", "zh-CN", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses", "zh-HK", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),

            // entity.qualityfailurepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworknote", "en-US", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityfailurepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworknote", "ja-JP", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityfailurepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworknote", "zh-CN", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityfailurepcbarework.pcbareworknote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbareworknote", "zh-HK", "PCBA选别改修备注", "PCBA选别・改修备注"),

            // entity.qualityfailurepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbascrapcost", "en-US", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityfailurepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbascrapcost", "ja-JP", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityfailurepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbascrapcost", "zh-CN", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityfailurepcbarework.pcbascrapcost
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbascrapcost", "zh-HK", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),

            // entity.qualityfailurepcbarework.pcbacustomername
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbacustomername", "en-US", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityfailurepcbarework.pcbacustomername
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbacustomername", "ja-JP", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityfailurepcbarework.pcbacustomername
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbacustomername", "zh-CN", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityfailurepcbarework.pcbacustomername
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbacustomername", "zh-HK", "PCBA顾客名", "PCBA顾客名"),

            // entity.qualityfailurepcbarework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadebitnoteno", "en-US", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityfailurepcbarework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadebitnoteno", "ja-JP", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityfailurepcbarework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadebitnoteno", "zh-CN", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityfailurepcbarework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbadebitnoteno", "zh-HK", "PCBA Debit Note No", "PCBA Debit Note No"),

            // entity.qualityfailurepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses2", "en-US", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses2", "ja-JP", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses2", "zh-CN", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityfailurepcbarework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbaotherexpenses2", "zh-HK", "PCBA其他费用", "PCBA其他费用（元）"),

            // entity.qualityfailurepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbanote", "en-US", "PCBA备注", "PCBA备注"),
            // entity.qualityfailurepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbanote", "ja-JP", "PCBA备注", "PCBA备注"),
            // entity.qualityfailurepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbanote", "zh-CN", "PCBA备注", "PCBA备注"),
            // entity.qualityfailurepcbarework.pcbanote
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbanote", "zh-HK", "PCBA备注", "PCBA备注"),

            // entity.qualityfailurepcbarework.pcbarecorder
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbarecorder", "en-US", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityfailurepcbarework.pcbarecorder
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbarecorder", "ja-JP", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityfailurepcbarework.pcbarecorder
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbarecorder", "zh-CN", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityfailurepcbarework.pcbarecorder
            new TranslationSeedItem("entity.qualityfailurepcbarework.pcbarecorder", "zh-HK", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),

            // entity.qualityfailurepcbarework.issue
            new TranslationSeedItem("entity.qualityfailurepcbarework.issue", "en-US", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailurepcbarework.issue
            new TranslationSeedItem("entity.qualityfailurepcbarework.issue", "ja-JP", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailurepcbarework.issue
            new TranslationSeedItem("entity.qualityfailurepcbarework.issue", "zh-CN", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityfailurepcbarework.issue
            new TranslationSeedItem("entity.qualityfailurepcbarework.issue", "zh-HK", "质量问题主表", "质量问题主表（导航属性）"),
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
