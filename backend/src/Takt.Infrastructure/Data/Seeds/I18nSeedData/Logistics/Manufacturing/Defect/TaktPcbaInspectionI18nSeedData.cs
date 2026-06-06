// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaInspection 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect;

/// <summary>
/// TaktPcbaInspection 实体国际化翻译种子（键前缀 entity.pcbaInspection.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaInspectionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaInspection 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaInspection 实体翻译...", tenantCode);

        foreach (var item in GetPcbaInspectionTranslations())
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

        TaktLogger.Information("TaktPcbaInspection 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaInspection 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaInspection._self / entity.pcbaInspection.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaInspectionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaInspection._self
            new TranslationSeedItem("entity.pcbaInspection._self", "en-US", "Pcba Inspection Information", "实体名称"),
            // entity.pcbaInspection._self
            new TranslationSeedItem("entity.pcbaInspection._self", "ja-JP", "PCBA检查日报信息", "实体名称"),
            // entity.pcbaInspection._self
            new TranslationSeedItem("entity.pcbaInspection._self", "zh-CN", "PCBA检查日报信息", "实体名称"),
            // entity.pcbaInspection._self
            new TranslationSeedItem("entity.pcbaInspection._self", "zh-HK", "PCBA检查日报信息", "实体名称"),

            // entity.pcbaInspection.plantcode
            new TranslationSeedItem("entity.pcbaInspection.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.pcbaInspection.plantcode
            new TranslationSeedItem("entity.pcbaInspection.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.pcbaInspection.plantcode
            new TranslationSeedItem("entity.pcbaInspection.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.pcbaInspection.plantcode
            new TranslationSeedItem("entity.pcbaInspection.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.pcbaInspection.prodcategory
            new TranslationSeedItem("entity.pcbaInspection.prodcategory", "en-US", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaInspection.prodcategory
            new TranslationSeedItem("entity.pcbaInspection.prodcategory", "ja-JP", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaInspection.prodcategory
            new TranslationSeedItem("entity.pcbaInspection.prodcategory", "zh-CN", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaInspection.prodcategory
            new TranslationSeedItem("entity.pcbaInspection.prodcategory", "zh-HK", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),

            // entity.pcbaInspection.proddate
            new TranslationSeedItem("entity.pcbaInspection.proddate", "en-US", "生产日期", "生产日期"),
            // entity.pcbaInspection.proddate
            new TranslationSeedItem("entity.pcbaInspection.proddate", "ja-JP", "生产日期", "生产日期"),
            // entity.pcbaInspection.proddate
            new TranslationSeedItem("entity.pcbaInspection.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbaInspection.proddate
            new TranslationSeedItem("entity.pcbaInspection.proddate", "zh-HK", "生产日期", "生产日期"),

            // entity.pcbaInspection.prodordercode
            new TranslationSeedItem("entity.pcbaInspection.prodordercode", "en-US", "生产工单号", "生产工单号"),
            // entity.pcbaInspection.prodordercode
            new TranslationSeedItem("entity.pcbaInspection.prodordercode", "ja-JP", "生产工单号", "生产工单号"),
            // entity.pcbaInspection.prodordercode
            new TranslationSeedItem("entity.pcbaInspection.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.pcbaInspection.prodordercode
            new TranslationSeedItem("entity.pcbaInspection.prodordercode", "zh-HK", "生产工单号", "生产工单号"),

            // entity.pcbaInspection.prodorderqty
            new TranslationSeedItem("entity.pcbaInspection.prodorderqty", "en-US", "订单数量", "订单数量"),
            // entity.pcbaInspection.prodorderqty
            new TranslationSeedItem("entity.pcbaInspection.prodorderqty", "ja-JP", "订单数量", "订单数量"),
            // entity.pcbaInspection.prodorderqty
            new TranslationSeedItem("entity.pcbaInspection.prodorderqty", "zh-CN", "订单数量", "订单数量"),
            // entity.pcbaInspection.prodorderqty
            new TranslationSeedItem("entity.pcbaInspection.prodorderqty", "zh-HK", "订单数量", "订单数量"),

            // entity.pcbaInspection.modelcode
            new TranslationSeedItem("entity.pcbaInspection.modelcode", "en-US", "机种", "机种"),
            // entity.pcbaInspection.modelcode
            new TranslationSeedItem("entity.pcbaInspection.modelcode", "ja-JP", "机种", "机种"),
            // entity.pcbaInspection.modelcode
            new TranslationSeedItem("entity.pcbaInspection.modelcode", "zh-CN", "机种", "机种"),
            // entity.pcbaInspection.modelcode
            new TranslationSeedItem("entity.pcbaInspection.modelcode", "zh-HK", "机种", "机种"),

            // entity.pcbaInspection.batchno
            new TranslationSeedItem("entity.pcbaInspection.batchno", "en-US", "批次", "批次"),
            // entity.pcbaInspection.batchno
            new TranslationSeedItem("entity.pcbaInspection.batchno", "ja-JP", "批次", "批次"),
            // entity.pcbaInspection.batchno
            new TranslationSeedItem("entity.pcbaInspection.batchno", "zh-CN", "批次", "批次"),
            // entity.pcbaInspection.batchno
            new TranslationSeedItem("entity.pcbaInspection.batchno", "zh-HK", "批次", "批次"),

            // entity.pcbaInspection.materialcode
            new TranslationSeedItem("entity.pcbaInspection.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.pcbaInspection.materialcode
            new TranslationSeedItem("entity.pcbaInspection.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.pcbaInspection.materialcode
            new TranslationSeedItem("entity.pcbaInspection.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.pcbaInspection.materialcode
            new TranslationSeedItem("entity.pcbaInspection.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.pcbaInspection.status
            new TranslationSeedItem("entity.pcbaInspection.status", "en-US", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbaInspection.status
            new TranslationSeedItem("entity.pcbaInspection.status", "ja-JP", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbaInspection.status
            new TranslationSeedItem("entity.pcbaInspection.status", "zh-CN", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbaInspection.status
            new TranslationSeedItem("entity.pcbaInspection.status", "zh-HK", "状态", "状态(0=正常 1=停用)"),

            // entity.pcbaInspection.details
            new TranslationSeedItem("entity.pcbaInspection.details", "en-US", "pcbaInspectionDetails", "PCBA检查明细列表"),
            // entity.pcbaInspection.details
            new TranslationSeedItem("entity.pcbaInspection.details", "ja-JP", "pcbaInspectionDetails", "PCBA检查明细列表"),
            // entity.pcbaInspection.details
            new TranslationSeedItem("entity.pcbaInspection.details", "zh-CN", "pcbaInspectionDetails", "PCBA检查明细列表"),
            // entity.pcbaInspection.details
            new TranslationSeedItem("entity.pcbaInspection.details", "zh-HK", "pcbaInspectionDetails", "PCBA检查明细列表"),
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
