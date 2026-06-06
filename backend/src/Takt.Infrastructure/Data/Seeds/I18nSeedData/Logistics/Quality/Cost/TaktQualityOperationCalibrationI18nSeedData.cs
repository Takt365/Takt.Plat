// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationCalibrationI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperationCalibration 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityOperationCalibration 实体国际化翻译种子（键前缀 entity.qualityOperationCalibration.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationCalibrationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperationCalibration 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperationCalibration 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationCalibrationTranslations())
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

        TaktLogger.Information("TaktQualityOperationCalibration 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperationCalibration 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityOperationCalibration._self / entity.qualityOperationCalibration.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationCalibrationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperationCalibration._self
            new TranslationSeedItem("entity.qualityOperationCalibration._self", "en-US", "Quality Operation Calibration Information", "实体名称"),
            // entity.qualityOperationCalibration._self
            new TranslationSeedItem("entity.qualityOperationCalibration._self", "ja-JP", "品质业务明细 - 测定器校正费用信息", "实体名称"),
            // entity.qualityOperationCalibration._self
            new TranslationSeedItem("entity.qualityOperationCalibration._self", "zh-CN", "品质业务明细 - 测定器校正费用信息", "实体名称"),
            // entity.qualityOperationCalibration._self
            new TranslationSeedItem("entity.qualityOperationCalibration._self", "zh-HK", "品质业务明细 - 测定器校正费用信息", "实体名称"),

            // entity.qualityOperationCalibration.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCalibration.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCalibration.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationCalibration.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityOperationCalibration.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCalibration.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCalibration.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationCalibration.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationCalibration.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityOperationCalibration.linenumber
            new TranslationSeedItem("entity.qualityOperationCalibration.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCalibration.linenumber
            new TranslationSeedItem("entity.qualityOperationCalibration.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCalibration.linenumber
            new TranslationSeedItem("entity.qualityOperationCalibration.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationCalibration.linenumber
            new TranslationSeedItem("entity.qualityOperationCalibration.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityOperationCalibration.calibrationcost
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationcost", "en-US", "测定器校正业务费用", "测定器校正业务费用(元)"),
            // entity.qualityOperationCalibration.calibrationcost
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationcost", "ja-JP", "测定器校正业务费用", "测定器校正业务费用(元)"),
            // entity.qualityOperationCalibration.calibrationcost
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationcost", "zh-CN", "测定器校正业务费用", "测定器校正业务费用(元)"),
            // entity.qualityOperationCalibration.calibrationcost
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationcost", "zh-HK", "测定器校正业务费用", "测定器校正业务费用(元)"),

            // entity.qualityOperationCalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCalibration.worktimeminutes", "en-US", "校正作业时间", "校正作业时间(分钟)"),
            // entity.qualityOperationCalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCalibration.worktimeminutes", "ja-JP", "校正作业时间", "校正作业时间(分钟)"),
            // entity.qualityOperationCalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCalibration.worktimeminutes", "zh-CN", "校正作业时间", "校正作业时间(分钟)"),
            // entity.qualityOperationCalibration.worktimeminutes
            new TranslationSeedItem("entity.qualityOperationCalibration.worktimeminutes", "zh-HK", "校正作业时间", "校正作业时间(分钟)"),

            // entity.qualityOperationCalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityOperationCalibration.externalagentservicefee", "en-US", "外部委托费运搬费", "外部委托费、运搬费(元)"),
            // entity.qualityOperationCalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityOperationCalibration.externalagentservicefee", "ja-JP", "外部委托费运搬费", "外部委托费、运搬费(元)"),
            // entity.qualityOperationCalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityOperationCalibration.externalagentservicefee", "zh-CN", "外部委托费运搬费", "外部委托费、运搬费(元)"),
            // entity.qualityOperationCalibration.externalagentservicefee
            new TranslationSeedItem("entity.qualityOperationCalibration.externalagentservicefee", "zh-HK", "外部委托费运搬费", "外部委托费、运搬费(元)"),

            // entity.qualityOperationCalibration.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCalibration.otherexpenses", "en-US", "校正其他费用", "校正其他费用(元)"),
            // entity.qualityOperationCalibration.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCalibration.otherexpenses", "ja-JP", "校正其他费用", "校正其他费用(元)"),
            // entity.qualityOperationCalibration.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCalibration.otherexpenses", "zh-CN", "校正其他费用", "校正其他费用(元)"),
            // entity.qualityOperationCalibration.otherexpenses
            new TranslationSeedItem("entity.qualityOperationCalibration.otherexpenses", "zh-HK", "校正其他费用", "校正其他费用(元)"),

            // entity.qualityOperationCalibration.calibrationnote
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationnote", "en-US", "校正备注", "校正备注"),
            // entity.qualityOperationCalibration.calibrationnote
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationnote", "ja-JP", "校正备注", "校正备注"),
            // entity.qualityOperationCalibration.calibrationnote
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationnote", "zh-CN", "校正备注", "校正备注"),
            // entity.qualityOperationCalibration.calibrationnote
            new TranslationSeedItem("entity.qualityOperationCalibration.calibrationnote", "zh-HK", "校正备注", "校正备注"),
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
