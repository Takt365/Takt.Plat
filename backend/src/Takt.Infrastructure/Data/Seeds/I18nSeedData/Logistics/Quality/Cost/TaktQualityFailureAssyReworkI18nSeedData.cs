// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityFailureAssyReworkI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityFailureAssyRework 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityFailureAssyRework 实体国际化翻译种子（键前缀 entity.qualityfailureassyrework.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityFailureAssyReworkI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityFailureAssyRework 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityfailureassyrework 实体翻译...", tenantCode);

        foreach (var item in GetQualityFailureAssyReworkTranslations())
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

        TaktLogger.Information("TaktQualityFailureAssyRework 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityFailureAssyRework 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityfailureassyrework._self / entity.qualityfailureassyrework.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetQualityFailureAssyReworkTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityfailureassyrework._self
            new TranslationSeedItem("entity.qualityfailureassyrework._self", "en-US", "Quality Failure Assy Rework Information", "实体名称"),
            // entity.qualityfailureassyrework._self
            new TranslationSeedItem("entity.qualityfailureassyrework._self", "ja-JP", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),
            // entity.qualityfailureassyrework._self
            new TranslationSeedItem("entity.qualityfailureassyrework._self", "zh-CN", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),
            // entity.qualityfailureassyrework._self
            new TranslationSeedItem("entity.qualityfailureassyrework._self", "zh-HK", "品质问题应对明细 - 组装不良改修应对信息", "实体名称"),

            // entity.qualityfailureassyrework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailureid", "en-US", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailureassyrework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailureid", "ja-JP", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailureassyrework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailureid", "zh-CN", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityfailureassyrework.qualityfailureid
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailureid", "zh-HK", "品质问题主表ID", "品质问题主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityfailureassyrework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailurecode", "en-US", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailureassyrework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailurecode", "ja-JP", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailureassyrework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailurecode", "zh-CN", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),
            // entity.qualityfailureassyrework.qualityfailurecode
            new TranslationSeedItem("entity.qualityfailureassyrework.qualityfailurecode", "zh-HK", "品质问题编码", "品质问题编码（冗余字段，便于查询）"),

            // entity.qualityfailureassyrework.linenumber
            new TranslationSeedItem("entity.qualityfailureassyrework.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailureassyrework.linenumber
            new TranslationSeedItem("entity.qualityfailureassyrework.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailureassyrework.linenumber
            new TranslationSeedItem("entity.qualityfailureassyrework.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityfailureassyrework.linenumber
            new TranslationSeedItem("entity.qualityfailureassyrework.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityfailureassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityfailureassyrework.assydefectparts", "en-US", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityfailureassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityfailureassyrework.assydefectparts", "ja-JP", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityfailureassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityfailureassyrework.assydefectparts", "zh-CN", "组装不良内容", "组装不良内容(Parts/Components)"),
            // entity.qualityfailureassyrework.assydefectparts
            new TranslationSeedItem("entity.qualityfailureassyrework.assydefectparts", "zh-HK", "组装不良内容", "组装不良内容(Parts/Components)"),

            // entity.qualityfailureassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworkcost", "en-US", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityfailureassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworkcost", "ja-JP", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityfailureassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworkcost", "zh-CN", "组装选别改修费用", "组装选别・改修费用(元)"),
            // entity.qualityfailureassyrework.assyreworkcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworkcost", "zh-HK", "组装选别改修费用", "组装选别・改修费用(元)"),

            // entity.qualityfailureassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworktimeminutes", "en-US", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityfailureassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworktimeminutes", "ja-JP", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityfailureassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworktimeminutes", "zh-CN", "组装选别改修时间", "组装选别・改修时间(分钟)"),
            // entity.qualityfailureassyrework.assyreworktimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworktimeminutes", "zh-HK", "组装选别改修时间", "组装选别・改修时间(分钟)"),

            // entity.qualityfailureassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreinspectiontimeminutes", "en-US", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityfailureassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreinspectiontimeminutes", "ja-JP", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityfailureassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreinspectiontimeminutes", "zh-CN", "组装再检查时间", "组装再检查时间(分钟)"),
            // entity.qualityfailureassyrework.assyreinspectiontimeminutes
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreinspectiontimeminutes", "zh-HK", "组装再检查时间", "组装再检查时间(分钟)"),

            // entity.qualityfailureassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assytravelcost", "en-US", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityfailureassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assytravelcost", "ja-JP", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityfailureassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assytravelcost", "zh-CN", "组装交通费旅费", "组装交通费、旅费(元)"),
            // entity.qualityfailureassyrework.assytravelcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assytravelcost", "zh-HK", "组装交通费旅费", "组装交通费、旅费(元)"),

            // entity.qualityfailureassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityfailureassyrework.assywarehousecost", "en-US", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityfailureassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityfailureassyrework.assywarehousecost", "ja-JP", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityfailureassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityfailureassyrework.assywarehousecost", "zh-CN", "组装仓库管理费", "组装仓库管理费(元)"),
            // entity.qualityfailureassyrework.assywarehousecost
            new TranslationSeedItem("entity.qualityfailureassyrework.assywarehousecost", "zh-HK", "组装仓库管理费", "组装仓库管理费(元)"),

            // entity.qualityfailureassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses", "en-US", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses", "ja-JP", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses", "zh-CN", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses", "zh-HK", "组装选别改修其他费用", "组装选别・改修其他费用(元)"),

            // entity.qualityfailureassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworknote", "en-US", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityfailureassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworknote", "ja-JP", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityfailureassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworknote", "zh-CN", "组装选别改修备注", "组装选别・改修备注"),
            // entity.qualityfailureassyrework.assyreworknote
            new TranslationSeedItem("entity.qualityfailureassyrework.assyreworknote", "zh-HK", "组装选别改修备注", "组装选别・改修备注"),

            // entity.qualityfailureassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyscrapcost", "en-US", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityfailureassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyscrapcost", "ja-JP", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityfailureassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyscrapcost", "zh-CN", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),
            // entity.qualityfailureassyrework.assyscrapcost
            new TranslationSeedItem("entity.qualityfailureassyrework.assyscrapcost", "zh-HK", "组装向顾客费用请求", "组装向顾客的费用请求(元)"),

            // entity.qualityfailureassyrework.assycustomername
            new TranslationSeedItem("entity.qualityfailureassyrework.assycustomername", "en-US", "组装顾客名", "组装顾客名"),
            // entity.qualityfailureassyrework.assycustomername
            new TranslationSeedItem("entity.qualityfailureassyrework.assycustomername", "ja-JP", "组装顾客名", "组装顾客名"),
            // entity.qualityfailureassyrework.assycustomername
            new TranslationSeedItem("entity.qualityfailureassyrework.assycustomername", "zh-CN", "组装顾客名", "组装顾客名"),
            // entity.qualityfailureassyrework.assycustomername
            new TranslationSeedItem("entity.qualityfailureassyrework.assycustomername", "zh-HK", "组装顾客名", "组装顾客名"),

            // entity.qualityfailureassyrework.assydebitnoteno
            new TranslationSeedItem("entity.qualityfailureassyrework.assydebitnoteno", "en-US", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityfailureassyrework.assydebitnoteno
            new TranslationSeedItem("entity.qualityfailureassyrework.assydebitnoteno", "ja-JP", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityfailureassyrework.assydebitnoteno
            new TranslationSeedItem("entity.qualityfailureassyrework.assydebitnoteno", "zh-CN", "组装 Debit Note No", "组装 Debit Note No"),
            // entity.qualityfailureassyrework.assydebitnoteno
            new TranslationSeedItem("entity.qualityfailureassyrework.assydebitnoteno", "zh-HK", "组装 Debit Note No", "组装 Debit Note No"),

            // entity.qualityfailureassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses2", "en-US", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses2", "ja-JP", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses2", "zh-CN", "组装其他费用", "组装其他费用(元)"),
            // entity.qualityfailureassyrework.assyotherexpenses2
            new TranslationSeedItem("entity.qualityfailureassyrework.assyotherexpenses2", "zh-HK", "组装其他费用", "组装其他费用(元)"),

            // entity.qualityfailureassyrework.assynote
            new TranslationSeedItem("entity.qualityfailureassyrework.assynote", "en-US", "组装备注", "组装备注"),
            // entity.qualityfailureassyrework.assynote
            new TranslationSeedItem("entity.qualityfailureassyrework.assynote", "ja-JP", "组装备注", "组装备注"),
            // entity.qualityfailureassyrework.assynote
            new TranslationSeedItem("entity.qualityfailureassyrework.assynote", "zh-CN", "组装备注", "组装备注"),
            // entity.qualityfailureassyrework.assynote
            new TranslationSeedItem("entity.qualityfailureassyrework.assynote", "zh-HK", "组装备注", "组装备注"),

            // entity.qualityfailureassyrework.assyrecorder
            new TranslationSeedItem("entity.qualityfailureassyrework.assyrecorder", "en-US", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityfailureassyrework.assyrecorder
            new TranslationSeedItem("entity.qualityfailureassyrework.assyrecorder", "ja-JP", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityfailureassyrework.assyrecorder
            new TranslationSeedItem("entity.qualityfailureassyrework.assyrecorder", "zh-CN", "组装不良改修对应记录者", "组装不良改修应对记录者"),
            // entity.qualityfailureassyrework.assyrecorder
            new TranslationSeedItem("entity.qualityfailureassyrework.assyrecorder", "zh-HK", "组装不良改修对应记录者", "组装不良改修应对记录者"),

            // entity.qualityfailureassyrework.issue
            new TranslationSeedItem("entity.qualityfailureassyrework.issue", "en-US", "品质问题主表", "品质问题主表(导航属性)"),
            // entity.qualityfailureassyrework.issue
            new TranslationSeedItem("entity.qualityfailureassyrework.issue", "ja-JP", "品质问题主表", "品质问题主表(导航属性)"),
            // entity.qualityfailureassyrework.issue
            new TranslationSeedItem("entity.qualityfailureassyrework.issue", "zh-CN", "品质问题主表", "品质问题主表(导航属性)"),
            // entity.qualityfailureassyrework.issue
            new TranslationSeedItem("entity.qualityfailureassyrework.issue", "zh-HK", "品质问题主表", "品质问题主表(导航属性)"),
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
