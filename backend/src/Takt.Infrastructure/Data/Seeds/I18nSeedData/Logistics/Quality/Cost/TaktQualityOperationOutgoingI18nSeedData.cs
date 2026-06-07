// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationOutgoingI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityOperationOutgoing 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityOperationOutgoing 实体国际化翻译种子（键前缀 entity.qualityOperationOutgoing.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityOperationOutgoingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityOperationOutgoing 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityOperationOutgoing 实体翻译...", tenantCode);

        foreach (var item in GetQualityOperationOutgoingTranslations())
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

        TaktLogger.Information("TaktQualityOperationOutgoing 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityOperationOutgoing 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityOperationOutgoing._self / entity.qualityOperationOutgoing.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityOperationOutgoingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityOperationOutgoing._self
            new TranslationSeedItem("entity.qualityOperationOutgoing._self", "en-US", "Quality Operation Outgoing Information", "实体名称"),
            // entity.qualityOperationOutgoing._self
            new TranslationSeedItem("entity.qualityOperationOutgoing._self", "ja-JP", "品质业务明细 - 出货检验业务费用信息", "实体名称"),
            // entity.qualityOperationOutgoing._self
            new TranslationSeedItem("entity.qualityOperationOutgoing._self", "zh-CN", "品质业务明细 - 出货检验业务费用信息", "实体名称"),
            // entity.qualityOperationOutgoing._self
            new TranslationSeedItem("entity.qualityOperationOutgoing._self", "zh-HK", "品质业务明细 - 出货检验业务费用信息", "实体名称"),

            // entity.qualityOperationOutgoing.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationid", "en-US", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOutgoing.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationid", "ja-JP", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOutgoing.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationid", "zh-CN", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),
            // entity.qualityOperationOutgoing.qualityoperationid
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationid", "zh-HK", "品质业务主表ID", "品质业务主表ID(主子表关系,序列化为string以避免Javascript精度问题)"),

            // entity.qualityOperationOutgoing.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationcode", "en-US", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOutgoing.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationcode", "ja-JP", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOutgoing.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationcode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityOperationOutgoing.qualityoperationcode
            new TranslationSeedItem("entity.qualityOperationOutgoing.qualityoperationcode", "zh-HK", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityOperationOutgoing.linenumber
            new TranslationSeedItem("entity.qualityOperationOutgoing.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationOutgoing.linenumber
            new TranslationSeedItem("entity.qualityOperationOutgoing.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationOutgoing.linenumber
            new TranslationSeedItem("entity.qualityOperationOutgoing.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityOperationOutgoing.linenumber
            new TranslationSeedItem("entity.qualityOperationOutgoing.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.qualityOperationOutgoing.inspectioncost
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectioncost", "en-US", "出货检验业务费用", "出货检验业务费用(元)"),
            // entity.qualityOperationOutgoing.inspectioncost
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectioncost", "ja-JP", "出货检验业务费用", "出货检验业务费用(元)"),
            // entity.qualityOperationOutgoing.inspectioncost
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectioncost", "zh-CN", "出货检验业务费用", "出货检验业务费用(元)"),
            // entity.qualityOperationOutgoing.inspectioncost
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectioncost", "zh-HK", "出货检验业务费用", "出货检验业务费用(元)"),

            // entity.qualityOperationOutgoing.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectiontimeminutes", "en-US", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationOutgoing.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectiontimeminutes", "ja-JP", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationOutgoing.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectiontimeminutes", "zh-CN", "检查时间", "检查时间(分钟)"),
            // entity.qualityOperationOutgoing.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityOperationOutgoing.inspectiontimeminutes", "zh-HK", "检查时间", "检查时间(分钟)"),

            // entity.qualityOperationOutgoing.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOutgoing.otherexpenses", "en-US", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationOutgoing.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOutgoing.otherexpenses", "ja-JP", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationOutgoing.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOutgoing.otherexpenses", "zh-CN", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityOperationOutgoing.otherexpenses
            new TranslationSeedItem("entity.qualityOperationOutgoing.otherexpenses", "zh-HK", "检查其他费用", "检查其他费用(元)"),

            // entity.qualityOperationOutgoing.outgoingnote
            new TranslationSeedItem("entity.qualityOperationOutgoing.outgoingnote", "en-US", "出货检验备注", "出货检验备注"),
            // entity.qualityOperationOutgoing.outgoingnote
            new TranslationSeedItem("entity.qualityOperationOutgoing.outgoingnote", "ja-JP", "出货检验备注", "出货检验备注"),
            // entity.qualityOperationOutgoing.outgoingnote
            new TranslationSeedItem("entity.qualityOperationOutgoing.outgoingnote", "zh-CN", "出货检验备注", "出货检验备注"),
            // entity.qualityOperationOutgoing.outgoingnote
            new TranslationSeedItem("entity.qualityOperationOutgoing.outgoingnote", "zh-HK", "出货检验备注", "出货检验备注"),
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
