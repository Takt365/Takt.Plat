// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationOtherI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperationOther 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityOperationOther 实体国际化翻译种子（键前缀 entity.qualityoperationother.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationOtherI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperationOther 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityoperationother 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationOtherTranslations())
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

        TaktLogger.Information("TaktQualityOperationOther 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperationOther 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityoperationother._self / entity.qualityoperationother.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationOtherTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityoperationother._self
            new TranslationSeedItem("entity.qualityoperationother._self", "en-US", "Quality Operation Other Information", "实体名称"),
            // entity.qualityoperationother._self
            new TranslationSeedItem("entity.qualityoperationother._self", "ja-JP", "品质业务明细 - 其他通常业务费用信息", "实体名称"),
            // entity.qualityoperationother._self
            new TranslationSeedItem("entity.qualityoperationother._self", "zh-CN", "品质业务明细 - 其他通常业务费用信息", "实体名称"),
            // entity.qualityoperationother._self
            new TranslationSeedItem("entity.qualityoperationother._self", "zh-HK", "品质业务明细 - 其他通常业务费用信息", "实体名称"),

            // entity.qualityoperationother.qualityoperationid
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityoperationother.qualityoperationid
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityoperationother.qualityoperationid
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityoperationother.qualityoperationid
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityoperationother.qualityoperationcode
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityoperationother.qualityoperationcode
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityoperationother.qualityoperationcode
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityoperationother.qualityoperationcode
            new TranslationSeedItem("entity.qualityoperationother.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityoperationother.linenumber
            new TranslationSeedItem("entity.qualityoperationother.linenumber", "en-US", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityoperationother.linenumber
            new TranslationSeedItem("entity.qualityoperationother.linenumber", "ja-JP", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityoperationother.linenumber
            new TranslationSeedItem("entity.qualityoperationother.linenumber", "zh-CN", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityoperationother.linenumber
            new TranslationSeedItem("entity.qualityoperationother.linenumber", "zh-HK", "项号", "项号（如10, 20, 30，步长严格为10）"),

            // entity.qualityoperationother.operationscost
            new TranslationSeedItem("entity.qualityoperationother.operationscost", "en-US", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityoperationother.operationscost
            new TranslationSeedItem("entity.qualityoperationother.operationscost", "ja-JP", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityoperationother.operationscost
            new TranslationSeedItem("entity.qualityoperationother.operationscost", "zh-CN", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityoperationother.operationscost
            new TranslationSeedItem("entity.qualityoperationother.operationscost", "zh-HK", "其他通常业务费用", "其他通常业务费用(元)"),

            // entity.qualityoperationother.worktimeminutes
            new TranslationSeedItem("entity.qualityoperationother.worktimeminutes", "en-US", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityoperationother.worktimeminutes
            new TranslationSeedItem("entity.qualityoperationother.worktimeminutes", "ja-JP", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityoperationother.worktimeminutes
            new TranslationSeedItem("entity.qualityoperationother.worktimeminutes", "zh-CN", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityoperationother.worktimeminutes
            new TranslationSeedItem("entity.qualityoperationother.worktimeminutes", "zh-HK", "通常业务作业时间", "通常业务作业时间(分钟)"),

            // entity.qualityoperationother.otherexpenses
            new TranslationSeedItem("entity.qualityoperationother.otherexpenses", "en-US", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityoperationother.otherexpenses
            new TranslationSeedItem("entity.qualityoperationother.otherexpenses", "ja-JP", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityoperationother.otherexpenses
            new TranslationSeedItem("entity.qualityoperationother.otherexpenses", "zh-CN", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityoperationother.otherexpenses
            new TranslationSeedItem("entity.qualityoperationother.otherexpenses", "zh-HK", "通常业务其他费用", "通常业务其他费用(元)"),

            // entity.qualityoperationother.othernote
            new TranslationSeedItem("entity.qualityoperationother.othernote", "en-US", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityoperationother.othernote
            new TranslationSeedItem("entity.qualityoperationother.othernote", "ja-JP", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityoperationother.othernote
            new TranslationSeedItem("entity.qualityoperationother.othernote", "zh-CN", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityoperationother.othernote
            new TranslationSeedItem("entity.qualityoperationother.othernote", "zh-HK", "通常业务其他备注", "通常业务其他备注"),

            // entity.qualityoperationother.operation
            new TranslationSeedItem("entity.qualityoperationother.operation", "en-US", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityoperationother.operation
            new TranslationSeedItem("entity.qualityoperationother.operation", "ja-JP", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityoperationother.operation
            new TranslationSeedItem("entity.qualityoperationother.operation", "zh-CN", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityoperationother.operation
            new TranslationSeedItem("entity.qualityoperationother.operation", "zh-HK", "品质业务主表", "品质业务主表(导航属性)"),
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
