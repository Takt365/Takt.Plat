// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueAssyReworkI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssueAssyRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssueAssyRework 实体国际化翻译种子（键前缀 entity.qualityIssueAssyRework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueAssyReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssueAssyRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityIssueAssyRework 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueAssyReworkTranslations())
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

        TaktLogger.Information("TaktQualityIssueAssyRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssueAssyRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityIssueAssyRework._self / entity.qualityIssueAssyRework.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueAssyReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityIssueAssyRework._self
            new TranslationSeedItem("entity.qualityIssueAssyRework._self", "en-US", "Quality Issue Assy Rework Information", "实体名称"),
            // entity.qualityIssueAssyRework._self
            new TranslationSeedItem("entity.qualityIssueAssyRework._self", "ja-JP", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),
            // entity.qualityIssueAssyRework._self
            new TranslationSeedItem("entity.qualityIssueAssyRework._self", "zh-CN", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),
            // entity.qualityIssueAssyRework._self
            new TranslationSeedItem("entity.qualityIssueAssyRework._self", "zh-HK", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),

            // entity.qualityIssueAssyRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissueid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueAssyRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissueid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueAssyRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityIssueAssyRework.qualityissueid
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissueid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityIssueAssyRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissuecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueAssyRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissuecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueAssyRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityIssueAssyRework.qualityissuecode
            new TranslationSeedItem("entity.qualityIssueAssyRework.qualityissuecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityIssueAssyRework.linenumber
            new TranslationSeedItem("entity.qualityIssueAssyRework.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueAssyRework.linenumber
            new TranslationSeedItem("entity.qualityIssueAssyRework.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueAssyRework.linenumber
            new TranslationSeedItem("entity.qualityIssueAssyRework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityIssueAssyRework.linenumber
            new TranslationSeedItem("entity.qualityIssueAssyRework.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityIssueAssyRework.assydefectparts
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydefectparts", "en-US", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityIssueAssyRework.assydefectparts
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydefectparts", "ja-JP", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityIssueAssyRework.assydefectparts
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydefectparts", "zh-CN", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityIssueAssyRework.assydefectparts
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydefectparts", "zh-HK", "组装不良内容", "组装不良内容(Parts/Components)"),

            // entity.qualityIssueAssyRework.assyreworkcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworkcost", "en-US", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityIssueAssyRework.assyreworkcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworkcost", "ja-JP", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityIssueAssyRework.assyreworkcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworkcost", "zh-CN", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityIssueAssyRework.assyreworkcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworkcost", "zh-HK", "组装选别改修费用", "组装选别・改修费用(元)"),

            // entity.qualityIssueAssyRework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworktimeminutes", "en-US", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworktimeminutes", "ja-JP", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworktimeminutes", "zh-CN", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworktimeminutes", "zh-HK", "组装选别改修时间", "组装选别・改修时间(分钟)"),

            // entity.qualityIssueAssyRework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreinspectiontimeminutes", "en-US", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreinspectiontimeminutes", "ja-JP", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreinspectiontimeminutes", "zh-CN", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityIssueAssyRework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreinspectiontimeminutes", "zh-HK", "组装再检查时间", "组装再检查时间(分钟)"),

            // entity.qualityIssueAssyRework.assytravelcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assytravelcost", "en-US", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityIssueAssyRework.assytravelcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assytravelcost", "ja-JP", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityIssueAssyRework.assytravelcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assytravelcost", "zh-CN", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityIssueAssyRework.assytravelcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assytravelcost", "zh-HK", "组装交通费旅费", "组装交通费、旅费(元)"),

            // entity.qualityIssueAssyRework.assywarehousecost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assywarehousecost", "en-US", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityIssueAssyRework.assywarehousecost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assywarehousecost", "ja-JP", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityIssueAssyRework.assywarehousecost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assywarehousecost", "zh-CN", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityIssueAssyRework.assywarehousecost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assywarehousecost", "zh-HK", "组装仓库管理费", "组装仓库管理费(元)"),

            // entity.qualityIssueAssyRework.assyotherexpenses
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses", "en-US", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses", "ja-JP", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses", "zh-CN", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses", "zh-HK", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),

            // entity.qualityIssueAssyRework.assyreworknote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworknote", "en-US", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityIssueAssyRework.assyreworknote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworknote", "ja-JP", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityIssueAssyRework.assyreworknote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworknote", "zh-CN", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityIssueAssyRework.assyreworknote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyreworknote", "zh-HK", "组装选别改修备注", "组装选别・改修备注"),

            // entity.qualityIssueAssyRework.assyscrapcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyscrapcost", "en-US", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityIssueAssyRework.assyscrapcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyscrapcost", "ja-JP", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityIssueAssyRework.assyscrapcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyscrapcost", "zh-CN", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityIssueAssyRework.assyscrapcost
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyscrapcost", "zh-HK", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),

            // entity.qualityIssueAssyRework.assycustomername
            new TranslationSeedItem("entity.qualityIssueAssyRework.assycustomername", "en-US", "组装顾客名", "组装顾客名"),
            // entity.qualityIssueAssyRework.assycustomername
            new TranslationSeedItem("entity.qualityIssueAssyRework.assycustomername", "ja-JP", "组装顾客名", "组装顾客名"),
            // entity.qualityIssueAssyRework.assycustomername
            new TranslationSeedItem("entity.qualityIssueAssyRework.assycustomername", "zh-CN", "组装顾客名", "组装顾客名"),
            // entity.qualityIssueAssyRework.assycustomername
            new TranslationSeedItem("entity.qualityIssueAssyRework.assycustomername", "zh-HK", "组装顾客名", "组装顾客名"),

            // entity.qualityIssueAssyRework.assydebitnoteno
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydebitnoteno", "en-US", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityIssueAssyRework.assydebitnoteno
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydebitnoteno", "ja-JP", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityIssueAssyRework.assydebitnoteno
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydebitnoteno", "zh-CN", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityIssueAssyRework.assydebitnoteno
            new TranslationSeedItem("entity.qualityIssueAssyRework.assydebitnoteno", "zh-HK", "组装 Debit Note No", "组装 Debit Note No"),

            // entity.qualityIssueAssyRework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses2", "en-US", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses2", "ja-JP", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses2", "zh-CN", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityIssueAssyRework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyotherexpenses2", "zh-HK", "组装其他费用", "组装其他费用(元)"),

            // entity.qualityIssueAssyRework.assynote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assynote", "en-US", "组装备注", "组装备注"),
            // entity.qualityIssueAssyRework.assynote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assynote", "ja-JP", "组装备注", "组装备注"),
            // entity.qualityIssueAssyRework.assynote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assynote", "zh-CN", "组装备注", "组装备注"),
            // entity.qualityIssueAssyRework.assynote
            new TranslationSeedItem("entity.qualityIssueAssyRework.assynote", "zh-HK", "组装备注", "组装备注"),

            // entity.qualityIssueAssyRework.assyrecorder
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyrecorder", "en-US", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityIssueAssyRework.assyrecorder
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyrecorder", "ja-JP", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityIssueAssyRework.assyrecorder
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyrecorder", "zh-CN", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityIssueAssyRework.assyrecorder
            new TranslationSeedItem("entity.qualityIssueAssyRework.assyrecorder", "zh-HK", "组装不良改修对应记录者", "组装不良改修应对记录者"),
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
