// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationIncomingI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperationIncoming 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityOperationIncoming 实体国际化翻译种子（键前缀 entity.qualityOperationIncoming.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationIncomingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperationIncoming 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperationIncoming 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationIncomingTranslations())
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

        TaktLogger.Information("TaktQualityOperationIncoming 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperationIncoming 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityOperationIncoming._self / entity.qualityOperationIncoming.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationIncomingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperationIncoming._self
            new TranslationSeedItem("entity.qualityOperationIncoming._self", "en-US", "Quality Operation Incoming Information", "实体名称"),
            // entity.qualityOperationIncoming._self
            new TranslationSeedItem("entity.qualityOperationIncoming._self", "ja-JP", "品质业务明细 - 来料检验费用信息", "实体名称"),
            // entity.qualityOperationIncoming._self
            new TranslationSeedItem("entity.qualityOperationIncoming._self", "zh-CN", "品质业务明细 - 来料检验费用信息", "实体名称"),
            // entity.qualityOperationIncoming._self
            new TranslationSeedItem("entity.qualityOperationIncoming._self", "zh-HK", "品质业务明细 - 来料检验费用信息", "实体名称"),

            // entity.qualityOperationIncoming.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationIncoming.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationIncoming.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationIncoming.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityOperationIncoming.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationIncoming.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationIncoming.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationIncoming.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationIncoming.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityOperationIncoming.linenumber
            new TranslationSeedItem("entity.qualityOperationIncoming.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationIncoming.linenumber
            new TranslationSeedItem("entity.qualityOperationIncoming.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationIncoming.linenumber
            new TranslationSeedItem("entity.qualityOperationIncoming.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationIncoming.linenumber
            new TranslationSeedItem("entity.qualityOperationIncoming.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityOperationIncoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityOperationIncoming.directmanpowercostperminute", "en-US", "直接人员费率", "直接人员费率(元/分钟)"),
            // entity.qualityOperationIncoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityOperationIncoming.directmanpowercostperminute", "ja-JP", "直接人员费率", "直接人员费率(元/分钟)"),
            // entity.qualityOperationIncoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityOperationIncoming.directmanpowercostperminute", "zh-CN", "直接人员费率", "直接人员费率(元/分钟)"),
            // entity.qualityOperationIncoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityOperationIncoming.directmanpowercostperminute", "zh-HK", "直接人员费率", "直接人员费率(元/分钟)"),

            // entity.qualityOperationIncoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityOperationIncoming.incominginspectioncost", "en-US", "来料检验业务费用", "来料检验业务费用(元)"),
            // entity.qualityOperationIncoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityOperationIncoming.incominginspectioncost", "ja-JP", "来料检验业务费用", "来料检验业务费用(元)"),
            // entity.qualityOperationIncoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityOperationIncoming.incominginspectioncost", "zh-CN", "来料检验业务费用", "来料检验业务费用(元)"),
            // entity.qualityOperationIncoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityOperationIncoming.incominginspectioncost", "zh-HK", "来料检验业务费用", "来料检验业务费用(元)"),

            // entity.qualityOperationIncoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationIncoming.inspectiontimeminutes", "en-US", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationIncoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationIncoming.inspectiontimeminutes", "ja-JP", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationIncoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationIncoming.inspectiontimeminutes", "zh-CN", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationIncoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationIncoming.inspectiontimeminutes", "zh-HK", "检查时间", "检查时间(分钟)"),

            // entity.qualityOperationIncoming.travelcost
            new TranslationSeedItem("entity.qualityOperationIncoming.travelcost", "en-US", "交通费旅费", "交通费、旅费(元)"),
            // entity.qualityOperationIncoming.travelcost
            new TranslationSeedItem("entity.qualityOperationIncoming.travelcost", "ja-JP", "交通费旅费", "交通费、旅费(元)"),
            // entity.qualityOperationIncoming.travelcost
            new TranslationSeedItem("entity.qualityOperationIncoming.travelcost", "zh-CN", "交通费旅费", "交通费、旅费(元)"),
            // entity.qualityOperationIncoming.travelcost
            new TranslationSeedItem("entity.qualityOperationIncoming.travelcost", "zh-HK", "交通费旅费", "交通费、旅费(元)"),

            // entity.qualityOperationIncoming.otherexpenses
            new TranslationSeedItem("entity.qualityOperationIncoming.otherexpenses", "en-US", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationIncoming.otherexpenses
            new TranslationSeedItem("entity.qualityOperationIncoming.otherexpenses", "ja-JP", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationIncoming.otherexpenses
            new TranslationSeedItem("entity.qualityOperationIncoming.otherexpenses", "zh-CN", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationIncoming.otherexpenses
            new TranslationSeedItem("entity.qualityOperationIncoming.otherexpenses", "zh-HK", "检查其他费用", "检查其他费用(元)"),

            // entity.qualityOperationIncoming.incomingnote
            new TranslationSeedItem("entity.qualityOperationIncoming.incomingnote", "en-US", "来料检验备注", "来料检验备注"),
            // entity.qualityOperationIncoming.incomingnote
            new TranslationSeedItem("entity.qualityOperationIncoming.incomingnote", "ja-JP", "来料检验备注", "来料检验备注"),
            // entity.qualityOperationIncoming.incomingnote
            new TranslationSeedItem("entity.qualityOperationIncoming.incomingnote", "zh-CN", "来料检验备注", "来料检验备注"),
            // entity.qualityOperationIncoming.incomingnote
            new TranslationSeedItem("entity.qualityOperationIncoming.incomingnote", "zh-HK", "来料检验备注", "来料检验备注"),
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
