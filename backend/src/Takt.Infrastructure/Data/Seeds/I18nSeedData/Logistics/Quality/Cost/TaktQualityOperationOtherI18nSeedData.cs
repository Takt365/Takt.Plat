// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationOtherI18nSeedData.cs
// 创建时间：2026-06-08
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityOperationOther 实体国际化翻译种子（键前缀 entity.qualityOperationOther.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperationOther 实体翻译...", tenantCode);

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
    /// I18nKey：entity.qualityOperationOther._self / entity.qualityOperationOther.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationOtherTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperationOther._self
            new TranslationSeedItem("entity.qualityOperationOther._self", "en-US", "Quality Operation Other Information", "实体名称"),
            // entity.qualityOperationOther._self
            new TranslationSeedItem("entity.qualityOperationOther._self", "ja-JP", "品质业务明细 - 其他通常业务费用信息", "实体名称"),
            // entity.qualityOperationOther._self
            new TranslationSeedItem("entity.qualityOperationOther._self", "zh-CN", "品质业务明细 - 其他通常业务费用信息", "实体名称"),
            // entity.qualityOperationOther._self
            new TranslationSeedItem("entity.qualityOperationOther._self", "zh-HK", "品质业务明细 - 其他通常业务费用信息", "实体名称"),

            // entity.qualityOperationOther.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOther.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOther.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOther.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityOperationOther.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOther.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOther.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOther.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOther.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityOperationOther.linenumber
            new TranslationSeedItem("entity.qualityOperationOther.linenumber", "en-US", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityOperationOther.linenumber
            new TranslationSeedItem("entity.qualityOperationOther.linenumber", "ja-JP", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityOperationOther.linenumber
            new TranslationSeedItem("entity.qualityOperationOther.linenumber", "zh-CN", "项号", "项号（如10, 20, 30，步长严格为10）"),
            // entity.qualityOperationOther.linenumber
            new TranslationSeedItem("entity.qualityOperationOther.linenumber", "zh-HK", "项号", "项号（如10, 20, 30，步长严格为10）"),

            // entity.qualityOperationOther.operationscost
            new TranslationSeedItem("entity.qualityOperationOther.operationscost", "en-US", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityOperationOther.operationscost
            new TranslationSeedItem("entity.qualityOperationOther.operationscost", "ja-JP", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityOperationOther.operationscost
            new TranslationSeedItem("entity.qualityOperationOther.operationscost", "zh-CN", "其他通常业务费用", "其他通常业务费用(元)"),
            // entity.qualityOperationOther.operationscost
            new TranslationSeedItem("entity.qualityOperationOther.operationscost", "zh-HK", "其他通常业务费用", "其他通常业务费用(元)"),

            // entity.qualityOperationOther.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationOther.worktimeminutes", "en-US", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityOperationOther.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationOther.worktimeminutes", "ja-JP", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityOperationOther.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationOther.worktimeminutes", "zh-CN", "通常业务作业时间", "通常业务作业时间(分钟)"),
            // entity.qualityOperationOther.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationOther.worktimeminutes", "zh-HK", "通常业务作业时间", "通常业务作业时间(分钟)"),

            // entity.qualityOperationOther.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOther.otherexpenses", "en-US", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityOperationOther.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOther.otherexpenses", "ja-JP", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityOperationOther.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOther.otherexpenses", "zh-CN", "通常业务其他费用", "通常业务其他费用(元)"),
            // entity.qualityOperationOther.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOther.otherexpenses", "zh-HK", "通常业务其他费用", "通常业务其他费用(元)"),

            // entity.qualityOperationOther.othernote
            new TranslationSeedItem("entity.qualityOperationOther.othernote", "en-US", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityOperationOther.othernote
            new TranslationSeedItem("entity.qualityOperationOther.othernote", "ja-JP", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityOperationOther.othernote
            new TranslationSeedItem("entity.qualityOperationOther.othernote", "zh-CN", "通常业务其他备注", "通常业务其他备注"),
            // entity.qualityOperationOther.othernote
            new TranslationSeedItem("entity.qualityOperationOther.othernote", "zh-HK", "通常业务其他备注", "通常业务其他备注"),

            // entity.qualityOperationOther.operation
            new TranslationSeedItem("entity.qualityOperationOther.operation", "en-US", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityOperationOther.operation
            new TranslationSeedItem("entity.qualityOperationOther.operation", "ja-JP", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityOperationOther.operation
            new TranslationSeedItem("entity.qualityOperationOther.operation", "zh-CN", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityOperationOther.operation
            new TranslationSeedItem("entity.qualityOperationOther.operation", "zh-HK", "品质业务主表", "品质业务主表(导航属性)"),
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
