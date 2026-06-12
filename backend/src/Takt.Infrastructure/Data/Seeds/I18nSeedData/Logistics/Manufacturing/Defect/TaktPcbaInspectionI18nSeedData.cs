// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect;

/// <summary>
/// TaktPcbaInspection 实体国际化翻译种子（键前缀 entity.pcbainspection.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbainspection 实体翻译...", tenantCode);

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
    /// I18nKey：entity.pcbainspection._self / entity.pcbainspection.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaInspectionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbainspection._self
            new TranslationSeedItem("entity.pcbainspection._self", "en-US", "Pcba Inspection Information", "实体名称"),
            // entity.pcbainspection._self
            new TranslationSeedItem("entity.pcbainspection._self", "ja-JP", "PCBA检查日报信息", "实体名称"),
            // entity.pcbainspection._self
            new TranslationSeedItem("entity.pcbainspection._self", "zh-CN", "PCBA检查日报信息", "实体名称"),
            // entity.pcbainspection._self
            new TranslationSeedItem("entity.pcbainspection._self", "zh-HK", "PCBA检查日报信息", "实体名称"),

            // entity.pcbainspection.plantcode
            new TranslationSeedItem("entity.pcbainspection.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.pcbainspection.plantcode
            new TranslationSeedItem("entity.pcbainspection.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.pcbainspection.plantcode
            new TranslationSeedItem("entity.pcbainspection.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.pcbainspection.plantcode
            new TranslationSeedItem("entity.pcbainspection.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.pcbainspection.prodcategory
            new TranslationSeedItem("entity.pcbainspection.prodcategory", "en-US", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbainspection.prodcategory
            new TranslationSeedItem("entity.pcbainspection.prodcategory", "ja-JP", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbainspection.prodcategory
            new TranslationSeedItem("entity.pcbainspection.prodcategory", "zh-CN", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbainspection.prodcategory
            new TranslationSeedItem("entity.pcbainspection.prodcategory", "zh-HK", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),

            // entity.pcbainspection.proddate
            new TranslationSeedItem("entity.pcbainspection.proddate", "en-US", "生产日期", "生产日期"),
            // entity.pcbainspection.proddate
            new TranslationSeedItem("entity.pcbainspection.proddate", "ja-JP", "生产日期", "生产日期"),
            // entity.pcbainspection.proddate
            new TranslationSeedItem("entity.pcbainspection.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbainspection.proddate
            new TranslationSeedItem("entity.pcbainspection.proddate", "zh-HK", "生产日期", "生产日期"),

            // entity.pcbainspection.prodordercode
            new TranslationSeedItem("entity.pcbainspection.prodordercode", "en-US", "生产工单号", "生产工单号"),
            // entity.pcbainspection.prodordercode
            new TranslationSeedItem("entity.pcbainspection.prodordercode", "ja-JP", "生产工单号", "生产工单号"),
            // entity.pcbainspection.prodordercode
            new TranslationSeedItem("entity.pcbainspection.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.pcbainspection.prodordercode
            new TranslationSeedItem("entity.pcbainspection.prodordercode", "zh-HK", "生产工单号", "生产工单号"),

            // entity.pcbainspection.prodorderqty
            new TranslationSeedItem("entity.pcbainspection.prodorderqty", "en-US", "订单数量", "订单数量"),
            // entity.pcbainspection.prodorderqty
            new TranslationSeedItem("entity.pcbainspection.prodorderqty", "ja-JP", "订单数量", "订单数量"),
            // entity.pcbainspection.prodorderqty
            new TranslationSeedItem("entity.pcbainspection.prodorderqty", "zh-CN", "订单数量", "订单数量"),
            // entity.pcbainspection.prodorderqty
            new TranslationSeedItem("entity.pcbainspection.prodorderqty", "zh-HK", "订单数量", "订单数量"),

            // entity.pcbainspection.modelcode
            new TranslationSeedItem("entity.pcbainspection.modelcode", "en-US", "机种", "机种"),
            // entity.pcbainspection.modelcode
            new TranslationSeedItem("entity.pcbainspection.modelcode", "ja-JP", "机种", "机种"),
            // entity.pcbainspection.modelcode
            new TranslationSeedItem("entity.pcbainspection.modelcode", "zh-CN", "机种", "机种"),
            // entity.pcbainspection.modelcode
            new TranslationSeedItem("entity.pcbainspection.modelcode", "zh-HK", "机种", "机种"),

            // entity.pcbainspection.batchno
            new TranslationSeedItem("entity.pcbainspection.batchno", "en-US", "批次", "批次"),
            // entity.pcbainspection.batchno
            new TranslationSeedItem("entity.pcbainspection.batchno", "ja-JP", "批次", "批次"),
            // entity.pcbainspection.batchno
            new TranslationSeedItem("entity.pcbainspection.batchno", "zh-CN", "批次", "批次"),
            // entity.pcbainspection.batchno
            new TranslationSeedItem("entity.pcbainspection.batchno", "zh-HK", "批次", "批次"),

            // entity.pcbainspection.materialcode
            new TranslationSeedItem("entity.pcbainspection.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.pcbainspection.materialcode
            new TranslationSeedItem("entity.pcbainspection.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.pcbainspection.materialcode
            new TranslationSeedItem("entity.pcbainspection.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.pcbainspection.materialcode
            new TranslationSeedItem("entity.pcbainspection.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.pcbainspection.status
            new TranslationSeedItem("entity.pcbainspection.status", "en-US", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbainspection.status
            new TranslationSeedItem("entity.pcbainspection.status", "ja-JP", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbainspection.status
            new TranslationSeedItem("entity.pcbainspection.status", "zh-CN", "状态", "状态(0=正常 1=停用)"),
            // entity.pcbainspection.status
            new TranslationSeedItem("entity.pcbainspection.status", "zh-HK", "状态", "状态(0=正常 1=停用)"),

            // entity.pcbainspection.details
            new TranslationSeedItem("entity.pcbainspection.details", "en-US", "PCBA检查明细列表", "PCBA检查明细列表"),
            // entity.pcbainspection.details
            new TranslationSeedItem("entity.pcbainspection.details", "ja-JP", "PCBA检查明细列表", "PCBA检查明细列表"),
            // entity.pcbainspection.details
            new TranslationSeedItem("entity.pcbainspection.details", "zh-CN", "PCBA检查明细列表", "PCBA检查明细列表"),
            // entity.pcbainspection.details
            new TranslationSeedItem("entity.pcbainspection.details", "zh-HK", "PCBA检查明细列表", "PCBA检查明细列表"),
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
