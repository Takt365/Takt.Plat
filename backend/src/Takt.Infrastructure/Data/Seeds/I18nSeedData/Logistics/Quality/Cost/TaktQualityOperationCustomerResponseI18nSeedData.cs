// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationCustomerResponseI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperationCustomerResponse 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityOperationCustomerResponse 实体国际化翻译种子（键前缀 entity.qualityOperationCustomerResponse.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationCustomerResponseI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperationCustomerResponse 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperationCustomerResponse 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationCustomerResponseTranslations())
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

        TaktLogger.Information("TaktQualityOperationCustomerResponse 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperationCustomerResponse 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityOperationCustomerResponse._self / entity.qualityOperationCustomerResponse.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationCustomerResponseTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperationCustomerResponse._self
            new TranslationSeedItem("entity.qualityOperationCustomerResponse._self", "en-US", "Quality Operation Customer Response Information", "实体名称"),
            // entity.qualityOperationCustomerResponse._self
            new TranslationSeedItem("entity.qualityOperationCustomerResponse._self", "ja-JP", "品质业务明细 - 顾客品质要求对应业务费用信息", "实体名称"),
            // entity.qualityOperationCustomerResponse._self
            new TranslationSeedItem("entity.qualityOperationCustomerResponse._self", "zh-CN", "品质业务明细 - 顾客品质要求对应业务费用信息", "实体名称"),
            // entity.qualityOperationCustomerResponse._self
            new TranslationSeedItem("entity.qualityOperationCustomerResponse._self", "zh-HK", "品质业务明细 - 顾客品质要求对应业务费用信息", "实体名称"),

            // entity.qualityOperationCustomerResponse.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCustomerResponse.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCustomerResponse.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCustomerResponse.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityOperationCustomerResponse.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCustomerResponse.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCustomerResponse.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCustomerResponse.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityOperationCustomerResponse.linenumber
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCustomerResponse.linenumber
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCustomerResponse.linenumber
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCustomerResponse.linenumber
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityOperationCustomerResponse.responsecost
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.responsecost", "en-US", "顾客品质要求对应业务费用", "顾客品质要求对应业务费用(元)"),
            // entity.qualityOperationCustomerResponse.responsecost
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.responsecost", "ja-JP", "顾客品质要求对应业务费用", "顾客品质要求对应业务费用(元)"),
            // entity.qualityOperationCustomerResponse.responsecost
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.responsecost", "zh-CN", "顾客品质要求对应业务费用", "顾客品质要求对应业务费用(元)"),
            // entity.qualityOperationCustomerResponse.responsecost
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.responsecost", "zh-HK", "顾客品质要求对应业务费用", "顾客品质要求对应业务费用(元)"),

            // entity.qualityOperationCustomerResponse.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.worktimeminutes", "en-US", "评价作业时间", "评价作业时间(分钟)"),
            // entity.qualityOperationCustomerResponse.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.worktimeminutes", "ja-JP", "评价作业时间", "评价作业时间(分钟)"),
            // entity.qualityOperationCustomerResponse.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.worktimeminutes", "zh-CN", "评价作业时间", "评价作业时间(分钟)"),
            // entity.qualityOperationCustomerResponse.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.worktimeminutes", "zh-HK", "评价作业时间", "评价作业时间(分钟)"),

            // entity.qualityOperationCustomerResponse.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.otherexpenses", "en-US", "评价其他费用", "评价其他费用(元)"),
            // entity.qualityOperationCustomerResponse.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.otherexpenses", "ja-JP", "评价其他费用", "评价其他费用(元)"),
            // entity.qualityOperationCustomerResponse.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.otherexpenses", "zh-CN", "评价其他费用", "评价其他费用(元)"),
            // entity.qualityOperationCustomerResponse.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.otherexpenses", "zh-HK", "评价其他费用", "评价其他费用(元)"),

            // entity.qualityOperationCustomerResponse.customerresponsenote
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.customerresponsenote", "en-US", "顾客应对备注", "顾客应对备注"),
            // entity.qualityOperationCustomerResponse.customerresponsenote
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.customerresponsenote", "ja-JP", "顾客应对备注", "顾客应对备注"),
            // entity.qualityOperationCustomerResponse.customerresponsenote
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.customerresponsenote", "zh-CN", "顾客应对备注", "顾客应对备注"),
            // entity.qualityOperationCustomerResponse.customerresponsenote
            new TranslationSeedItem("entity.qualityOperationCustomerResponse.customerresponsenote", "zh-HK", "顾客应对备注", "顾客应对备注"),
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
