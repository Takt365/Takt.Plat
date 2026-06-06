// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuePcbaReworkI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssuePcbaRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityIssuePcbaRework 实体国际化翻译种子（键前缀 entity.qualityIssuePcbaRework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssuePcbaReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssuePcbaRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityIssuePcbaRework 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssuePcbaReworkTranslations())
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

        TaktLogger.Information("TaktQualityIssuePcbaRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssuePcbaRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityIssuePcbaRework._self / entity.qualityIssuePcbaRework.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssuePcbaReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityIssuePcbaRework._self
            new TranslationSeedItem("entity.qualityIssuePcbaRework._self", "en-US", "Quality Issue Pcba Rework Information", "实体名称"),
            // entity.qualityIssuePcbaRework._self
            new TranslationSeedItem("entity.qualityIssuePcbaRework._self", "ja-JP", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),
            // entity.qualityIssuePcbaRework._self
            new TranslationSeedItem("entity.qualityIssuePcbaRework._self", "zh-CN", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),
            // entity.qualityIssuePcbaRework._self
            new TranslationSeedItem("entity.qualityIssuePcbaRework._self", "zh-HK", "品质问题应对明细 - PCBA不良改修应对信息", "实体名称"),

            // entity.qualityIssuePcbaRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissueid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssuePcbaRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissueid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssuePcbaRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssuePcbaRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissueid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityIssuePcbaRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissuecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssuePcbaRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissuecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssuePcbaRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssuePcbaRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssuePcbaRework.qualityissuecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityIssuePcbaRework.linenumber
            new TranslationSeedItem("entity.qualityIssuePcbaRework.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssuePcbaRework.linenumber
            new TranslationSeedItem("entity.qualityIssuePcbaRework.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssuePcbaRework.linenumber
            new TranslationSeedItem("entity.qualityIssuePcbaRework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssuePcbaRework.linenumber
            new TranslationSeedItem("entity.qualityIssuePcbaRework.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityIssuePcbaRework.pcbadefectparts
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadefectparts", "en-US", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityIssuePcbaRework.pcbadefectparts
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadefectparts", "ja-JP", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityIssuePcbaRework.pcbadefectparts
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadefectparts", "zh-CN", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),
            // entity.qualityIssuePcbaRework.pcbadefectparts
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadefectparts", "zh-HK", "PCBA不良内容", "PCBA不良内容(Parts/Components)"),

            // entity.qualityIssuePcbaRework.pcbareworkcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworkcost", "en-US", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityIssuePcbaRework.pcbareworkcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworkcost", "ja-JP", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityIssuePcbaRework.pcbareworkcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworkcost", "zh-CN", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),
            // entity.qualityIssuePcbaRework.pcbareworkcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworkcost", "zh-HK", "PCBA选别改修费用", "PCBA选别・改修费用（元）"),

            // entity.qualityIssuePcbaRework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworktimeminutes", "en-US", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworktimeminutes", "ja-JP", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworktimeminutes", "zh-CN", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareworktimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworktimeminutes", "zh-HK", "PCBA选别改修时间", "PCBA选别・改修时间（分钟）"),

            // entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes", "en-US", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes", "ja-JP", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes", "zh-CN", "PCBA再检查时间", "PCBA再检查时间（分钟）"),
            // entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes", "zh-HK", "PCBA再检查时间", "PCBA再检查时间（分钟）"),

            // entity.qualityIssuePcbaRework.pcbatravelcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbatravelcost", "en-US", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityIssuePcbaRework.pcbatravelcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbatravelcost", "ja-JP", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityIssuePcbaRework.pcbatravelcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbatravelcost", "zh-CN", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),
            // entity.qualityIssuePcbaRework.pcbatravelcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbatravelcost", "zh-HK", "PCBA交通费旅费", "PCBA交通费、旅费（元）"),

            // entity.qualityIssuePcbaRework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbawarehousecost", "en-US", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityIssuePcbaRework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbawarehousecost", "ja-JP", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityIssuePcbaRework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbawarehousecost", "zh-CN", "PCBA仓库管理费", "PCBA仓库管理费（元）"),
            // entity.qualityIssuePcbaRework.pcbawarehousecost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbawarehousecost", "zh-HK", "PCBA仓库管理费", "PCBA仓库管理费（元）"),

            // entity.qualityIssuePcbaRework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses", "en-US", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses", "ja-JP", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses", "zh-CN", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses", "zh-HK", "PCBA选别改修其他费用", "PCBA选别・改修其他费用（元）"),

            // entity.qualityIssuePcbaRework.pcbareworknote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworknote", "en-US", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityIssuePcbaRework.pcbareworknote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworknote", "ja-JP", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityIssuePcbaRework.pcbareworknote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworknote", "zh-CN", "PCBA选别改修备注", "PCBA选别・改修备注"),
            // entity.qualityIssuePcbaRework.pcbareworknote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbareworknote", "zh-HK", "PCBA选别改修备注", "PCBA选别・改修备注"),

            // entity.qualityIssuePcbaRework.pcbascrapcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbascrapcost", "en-US", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityIssuePcbaRework.pcbascrapcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbascrapcost", "ja-JP", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityIssuePcbaRework.pcbascrapcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbascrapcost", "zh-CN", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),
            // entity.qualityIssuePcbaRework.pcbascrapcost
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbascrapcost", "zh-HK", "PCBA向顾客费用请求", "PCBA向顾客的费用请求（元）"),

            // entity.qualityIssuePcbaRework.pcbacustomername
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbacustomername", "en-US", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityIssuePcbaRework.pcbacustomername
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbacustomername", "ja-JP", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityIssuePcbaRework.pcbacustomername
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbacustomername", "zh-CN", "PCBA顾客名", "PCBA顾客名"),
            // entity.qualityIssuePcbaRework.pcbacustomername
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbacustomername", "zh-HK", "PCBA顾客名", "PCBA顾客名"),

            // entity.qualityIssuePcbaRework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadebitnoteno", "en-US", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityIssuePcbaRework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadebitnoteno", "ja-JP", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityIssuePcbaRework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadebitnoteno", "zh-CN", "PCBA Debit Note No", "PCBA Debit Note No"),
            // entity.qualityIssuePcbaRework.pcbadebitnoteno
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbadebitnoteno", "zh-HK", "PCBA Debit Note No", "PCBA Debit Note No"),

            // entity.qualityIssuePcbaRework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses2", "en-US", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses2", "ja-JP", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses2", "zh-CN", "PCBA其他费用", "PCBA其他费用（元）"),
            // entity.qualityIssuePcbaRework.pcbaotherexpenses2
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbaotherexpenses2", "zh-HK", "PCBA其他费用", "PCBA其他费用（元）"),

            // entity.qualityIssuePcbaRework.pcbanote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbanote", "en-US", "PCBA备注", "PCBA备注"),
            // entity.qualityIssuePcbaRework.pcbanote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbanote", "ja-JP", "PCBA备注", "PCBA备注"),
            // entity.qualityIssuePcbaRework.pcbanote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbanote", "zh-CN", "PCBA备注", "PCBA备注"),
            // entity.qualityIssuePcbaRework.pcbanote
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbanote", "zh-HK", "PCBA备注", "PCBA备注"),

            // entity.qualityIssuePcbaRework.pcbarecorder
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbarecorder", "en-US", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityIssuePcbaRework.pcbarecorder
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbarecorder", "ja-JP", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityIssuePcbaRework.pcbarecorder
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbarecorder", "zh-CN", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
            // entity.qualityIssuePcbaRework.pcbarecorder
            new TranslationSeedItem("entity.qualityIssuePcbaRework.pcbarecorder", "zh-HK", "PCBA不良改修对应记录者", "PCBA不良改修应对记录者"),
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
