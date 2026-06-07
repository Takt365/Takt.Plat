// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionOrder 实体国际化翻译种子（键前缀 entity.productionOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionOrder 实体翻译...", tenantCode);

        foreach (var item in GetProductionOrderTranslations())
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

        TaktLogger.Information("TaktProductionOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionOrder._self / entity.productionOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionOrder._self
            new TranslationSeedItem("entity.productionOrder._self", "en-US", "Production Order Information", "实体名称"),
            // entity.productionOrder._self
            new TranslationSeedItem("entity.productionOrder._self", "ja-JP", "生产工单信息", "实体名称"),
            // entity.productionOrder._self
            new TranslationSeedItem("entity.productionOrder._self", "zh-CN", "生产工单信息", "实体名称"),
            // entity.productionOrder._self
            new TranslationSeedItem("entity.productionOrder._self", "zh-HK", "生产工单信息", "实体名称"),

            // entity.productionOrder.plantcode
            new TranslationSeedItem("entity.productionOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.productionOrder.plantcode
            new TranslationSeedItem("entity.productionOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.productionOrder.plantcode
            new TranslationSeedItem("entity.productionOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.productionOrder.plantcode
            new TranslationSeedItem("entity.productionOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.productionOrder.prodordertype
            new TranslationSeedItem("entity.productionOrder.prodordertype", "en-US", "生产工单类型", "生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA"),
            // entity.productionOrder.prodordertype
            new TranslationSeedItem("entity.productionOrder.prodordertype", "ja-JP", "生产工单类型", "生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA"),
            // entity.productionOrder.prodordertype
            new TranslationSeedItem("entity.productionOrder.prodordertype", "zh-CN", "生产工单类型", "生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA"),
            // entity.productionOrder.prodordertype
            new TranslationSeedItem("entity.productionOrder.prodordertype", "zh-HK", "生产工单类型", "生产工单类型 ZDTA=製造指図：DTA通常生産 ZDTB=製造指図：DTA改造改修 ZDTC=製造指図：DTA開発試作 ZDTD=製造指図：DTA通常生産 PCBA ZDTE=製造指図：DTA改造改修 PCBA ZDTF=製造指図：DTA開発試作 PCBA"),

            // entity.productionOrder.prodordercode
            new TranslationSeedItem("entity.productionOrder.prodordercode", "en-US", "生产工单号", "生产工单号"),
            // entity.productionOrder.prodordercode
            new TranslationSeedItem("entity.productionOrder.prodordercode", "ja-JP", "生产工单号", "生产工单号"),
            // entity.productionOrder.prodordercode
            new TranslationSeedItem("entity.productionOrder.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.productionOrder.prodordercode
            new TranslationSeedItem("entity.productionOrder.prodordercode", "zh-HK", "生产工单号", "生产工单号"),

            // entity.productionOrder.materialcode
            new TranslationSeedItem("entity.productionOrder.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.productionOrder.materialcode
            new TranslationSeedItem("entity.productionOrder.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.productionOrder.materialcode
            new TranslationSeedItem("entity.productionOrder.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.productionOrder.materialcode
            new TranslationSeedItem("entity.productionOrder.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.productionOrder.prodorderqty
            new TranslationSeedItem("entity.productionOrder.prodorderqty", "en-US", "生产工单数量", "生产工单数量"),
            // entity.productionOrder.prodorderqty
            new TranslationSeedItem("entity.productionOrder.prodorderqty", "ja-JP", "生产工单数量", "生产工单数量"),
            // entity.productionOrder.prodorderqty
            new TranslationSeedItem("entity.productionOrder.prodorderqty", "zh-CN", "生产工单数量", "生产工单数量"),
            // entity.productionOrder.prodorderqty
            new TranslationSeedItem("entity.productionOrder.prodorderqty", "zh-HK", "生产工单数量", "生产工单数量"),

            // entity.productionOrder.producedqty
            new TranslationSeedItem("entity.productionOrder.producedqty", "en-US", "已生产数量", "已生产数量"),
            // entity.productionOrder.producedqty
            new TranslationSeedItem("entity.productionOrder.producedqty", "ja-JP", "已生产数量", "已生产数量"),
            // entity.productionOrder.producedqty
            new TranslationSeedItem("entity.productionOrder.producedqty", "zh-CN", "已生产数量", "已生产数量"),
            // entity.productionOrder.producedqty
            new TranslationSeedItem("entity.productionOrder.producedqty", "zh-HK", "已生产数量", "已生产数量"),

            // entity.productionOrder.unitofmeasure
            new TranslationSeedItem("entity.productionOrder.unitofmeasure", "en-US", "计量单位", "计量单位"),
            // entity.productionOrder.unitofmeasure
            new TranslationSeedItem("entity.productionOrder.unitofmeasure", "ja-JP", "计量单位", "计量单位"),
            // entity.productionOrder.unitofmeasure
            new TranslationSeedItem("entity.productionOrder.unitofmeasure", "zh-CN", "计量单位", "计量单位"),
            // entity.productionOrder.unitofmeasure
            new TranslationSeedItem("entity.productionOrder.unitofmeasure", "zh-HK", "计量单位", "计量单位"),

            // entity.productionOrder.actualstartdate
            new TranslationSeedItem("entity.productionOrder.actualstartdate", "en-US", "实际开始日期", "实际开始日期"),
            // entity.productionOrder.actualstartdate
            new TranslationSeedItem("entity.productionOrder.actualstartdate", "ja-JP", "实际开始日期", "实际开始日期"),
            // entity.productionOrder.actualstartdate
            new TranslationSeedItem("entity.productionOrder.actualstartdate", "zh-CN", "实际开始日期", "实际开始日期"),
            // entity.productionOrder.actualstartdate
            new TranslationSeedItem("entity.productionOrder.actualstartdate", "zh-HK", "实际开始日期", "实际开始日期"),

            // entity.productionOrder.actualenddate
            new TranslationSeedItem("entity.productionOrder.actualenddate", "en-US", "实际完成日期", "实际完成日期"),
            // entity.productionOrder.actualenddate
            new TranslationSeedItem("entity.productionOrder.actualenddate", "ja-JP", "实际完成日期", "实际完成日期"),
            // entity.productionOrder.actualenddate
            new TranslationSeedItem("entity.productionOrder.actualenddate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.productionOrder.actualenddate
            new TranslationSeedItem("entity.productionOrder.actualenddate", "zh-HK", "实际完成日期", "实际完成日期"),

            // entity.productionOrder.priority
            new TranslationSeedItem("entity.productionOrder.priority", "en-US", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.productionOrder.priority
            new TranslationSeedItem("entity.productionOrder.priority", "ja-JP", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.productionOrder.priority
            new TranslationSeedItem("entity.productionOrder.priority", "zh-CN", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),
            // entity.productionOrder.priority
            new TranslationSeedItem("entity.productionOrder.priority", "zh-HK", "优先级", "优先级（1=低，2=中，3=高，4=紧急）"),

            // entity.productionOrder.workcenter
            new TranslationSeedItem("entity.productionOrder.workcenter", "en-US", "工作中心", "工作中心"),
            // entity.productionOrder.workcenter
            new TranslationSeedItem("entity.productionOrder.workcenter", "ja-JP", "工作中心", "工作中心"),
            // entity.productionOrder.workcenter
            new TranslationSeedItem("entity.productionOrder.workcenter", "zh-CN", "工作中心", "工作中心"),
            // entity.productionOrder.workcenter
            new TranslationSeedItem("entity.productionOrder.workcenter", "zh-HK", "工作中心", "工作中心"),

            // entity.productionOrder.prodline
            new TranslationSeedItem("entity.productionOrder.prodline", "en-US", "生产线", "生产线"),
            // entity.productionOrder.prodline
            new TranslationSeedItem("entity.productionOrder.prodline", "ja-JP", "生产线", "生产线"),
            // entity.productionOrder.prodline
            new TranslationSeedItem("entity.productionOrder.prodline", "zh-CN", "生产线", "生产线"),
            // entity.productionOrder.prodline
            new TranslationSeedItem("entity.productionOrder.prodline", "zh-HK", "生产线", "生产线"),

            // entity.productionOrder.prodbatch
            new TranslationSeedItem("entity.productionOrder.prodbatch", "en-US", "生产批次", "生产批次"),
            // entity.productionOrder.prodbatch
            new TranslationSeedItem("entity.productionOrder.prodbatch", "ja-JP", "生产批次", "生产批次"),
            // entity.productionOrder.prodbatch
            new TranslationSeedItem("entity.productionOrder.prodbatch", "zh-CN", "生产批次", "生产批次"),
            // entity.productionOrder.prodbatch
            new TranslationSeedItem("entity.productionOrder.prodbatch", "zh-HK", "生产批次", "生产批次"),

            // entity.productionOrder.serialno
            new TranslationSeedItem("entity.productionOrder.serialno", "en-US", "序列号", "序列号"),
            // entity.productionOrder.serialno
            new TranslationSeedItem("entity.productionOrder.serialno", "ja-JP", "序列号", "序列号"),
            // entity.productionOrder.serialno
            new TranslationSeedItem("entity.productionOrder.serialno", "zh-CN", "序列号", "序列号"),
            // entity.productionOrder.serialno
            new TranslationSeedItem("entity.productionOrder.serialno", "zh-HK", "序列号", "序列号"),

            // entity.productionOrder.routingcode
            new TranslationSeedItem("entity.productionOrder.routingcode", "en-US", "工艺路线编码", "工艺路线编码"),
            // entity.productionOrder.routingcode
            new TranslationSeedItem("entity.productionOrder.routingcode", "ja-JP", "工艺路线编码", "工艺路线编码"),
            // entity.productionOrder.routingcode
            new TranslationSeedItem("entity.productionOrder.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码"),
            // entity.productionOrder.routingcode
            new TranslationSeedItem("entity.productionOrder.routingcode", "zh-HK", "工艺路线编码", "工艺路线编码"),

            // entity.productionOrder.status
            new TranslationSeedItem("entity.productionOrder.status", "en-US", "状态", "状态（0=正常，1=生产中，2=已完成）"),
            // entity.productionOrder.status
            new TranslationSeedItem("entity.productionOrder.status", "ja-JP", "状态", "状态（0=正常，1=生产中，2=已完成）"),
            // entity.productionOrder.status
            new TranslationSeedItem("entity.productionOrder.status", "zh-CN", "状态", "状态（0=正常，1=生产中，2=已完成）"),
            // entity.productionOrder.status
            new TranslationSeedItem("entity.productionOrder.status", "zh-HK", "状态", "状态（0=正常，1=生产中，2=已完成）"),
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
