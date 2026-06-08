// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial
// 文件名称：TaktProductSerialOutboundItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductSerialOutboundItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Serial;

/// <summary>
/// TaktProductSerialOutboundItem 实体国际化翻译种子（键前缀 entity.productSerialOutboundItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductSerialOutboundItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductSerialOutboundItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productSerialOutboundItem 实体翻译...", tenantCode);

        foreach (var item in GetProductSerialOutboundItemTranslations())
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

        TaktLogger.Information("TaktProductSerialOutboundItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductSerialOutboundItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productSerialOutboundItem._self / entity.productSerialOutboundItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductSerialOutboundItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productSerialOutboundItem._self
            new TranslationSeedItem("entity.productSerialOutboundItem._self", "en-US", "Product Serial Outbound Item Information", "实体名称"),
            // entity.productSerialOutboundItem._self
            new TranslationSeedItem("entity.productSerialOutboundItem._self", "ja-JP", "产品序列号出库明细信息", "实体名称"),
            // entity.productSerialOutboundItem._self
            new TranslationSeedItem("entity.productSerialOutboundItem._self", "zh-CN", "产品序列号出库明细信息", "实体名称"),
            // entity.productSerialOutboundItem._self
            new TranslationSeedItem("entity.productSerialOutboundItem._self", "zh-HK", "产品序列号出库明细信息", "实体名称"),

            // entity.productSerialOutboundItem.outboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundid", "en-US", "出库ID", "出库ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productSerialOutboundItem.outboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundid", "ja-JP", "出库ID", "出库ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productSerialOutboundItem.outboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundid", "zh-CN", "出库ID", "出库ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.productSerialOutboundItem.outboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundid", "zh-HK", "出库ID", "出库ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.productSerialOutboundItem.outboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundno", "en-US", "出库单号", "出库单号（冗余字段，便于查询）"),
            // entity.productSerialOutboundItem.outboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundno", "ja-JP", "出库单号", "出库单号（冗余字段，便于查询）"),
            // entity.productSerialOutboundItem.outboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundno", "zh-CN", "出库单号", "出库单号（冗余字段，便于查询）"),
            // entity.productSerialOutboundItem.outboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundno", "zh-HK", "出库单号", "出库单号（冗余字段，便于查询）"),

            // entity.productSerialOutboundItem.linenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productSerialOutboundItem.linenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productSerialOutboundItem.linenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productSerialOutboundItem.linenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.productSerialOutboundItem.outboundserialno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundserialno", "en-US", "出库序列号", "出库序列号（唯一索引）"),
            // entity.productSerialOutboundItem.outboundserialno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundserialno", "ja-JP", "出库序列号", "出库序列号（唯一索引）"),
            // entity.productSerialOutboundItem.outboundserialno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundserialno", "zh-CN", "出库序列号", "出库序列号（唯一索引）"),
            // entity.productSerialOutboundItem.outboundserialno
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundserialno", "zh-HK", "出库序列号", "出库序列号（唯一索引）"),

            // entity.productSerialOutboundItem.referenceinboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundid", "en-US", "关联入库ID", "关联入库ID(序列化为string以避免Javascript精度问题)"),
            // entity.productSerialOutboundItem.referenceinboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundid", "ja-JP", "关联入库ID", "关联入库ID(序列化为string以避免Javascript精度问题)"),
            // entity.productSerialOutboundItem.referenceinboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundid", "zh-CN", "关联入库ID", "关联入库ID(序列化为string以避免Javascript精度问题)"),
            // entity.productSerialOutboundItem.referenceinboundid
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundid", "zh-HK", "关联入库ID", "关联入库ID(序列化为string以避免Javascript精度问题)"),

            // entity.productSerialOutboundItem.referenceinboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundno", "en-US", "关联入库单号", "关联入库单号"),
            // entity.productSerialOutboundItem.referenceinboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundno", "ja-JP", "关联入库单号", "关联入库单号"),
            // entity.productSerialOutboundItem.referenceinboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundno", "zh-CN", "关联入库单号", "关联入库单号"),
            // entity.productSerialOutboundItem.referenceinboundno
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundno", "zh-HK", "关联入库单号", "关联入库单号"),

            // entity.productSerialOutboundItem.referenceinboundlinenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundlinenumber", "en-US", "关联入库行号", "关联入库行号"),
            // entity.productSerialOutboundItem.referenceinboundlinenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundlinenumber", "ja-JP", "关联入库行号", "关联入库行号"),
            // entity.productSerialOutboundItem.referenceinboundlinenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundlinenumber", "zh-CN", "关联入库行号", "关联入库行号"),
            // entity.productSerialOutboundItem.referenceinboundlinenumber
            new TranslationSeedItem("entity.productSerialOutboundItem.referenceinboundlinenumber", "zh-HK", "关联入库行号", "关联入库行号"),

            // entity.productSerialOutboundItem.outboundtime
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundtime", "en-US", "出库时间", "出库时间"),
            // entity.productSerialOutboundItem.outboundtime
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundtime", "ja-JP", "出库时间", "出库时间"),
            // entity.productSerialOutboundItem.outboundtime
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundtime", "zh-CN", "出库时间", "出库时间"),
            // entity.productSerialOutboundItem.outboundtime
            new TranslationSeedItem("entity.productSerialOutboundItem.outboundtime", "zh-HK", "出库时间", "出库时间"),

            // entity.productSerialOutboundItem.outbound
            new TranslationSeedItem("entity.productSerialOutboundItem.outbound", "en-US", "出库主表", "出库主表"),
            // entity.productSerialOutboundItem.outbound
            new TranslationSeedItem("entity.productSerialOutboundItem.outbound", "ja-JP", "出库主表", "出库主表"),
            // entity.productSerialOutboundItem.outbound
            new TranslationSeedItem("entity.productSerialOutboundItem.outbound", "zh-CN", "出库主表", "出库主表"),
            // entity.productSerialOutboundItem.outbound
            new TranslationSeedItem("entity.productSerialOutboundItem.outbound", "zh-HK", "出库主表", "出库主表"),
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
