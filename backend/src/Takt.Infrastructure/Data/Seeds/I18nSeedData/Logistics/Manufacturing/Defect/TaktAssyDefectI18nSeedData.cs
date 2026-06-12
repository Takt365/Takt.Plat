// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyDefect 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/defect/assy-defect）
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
/// TaktAssyDefect 实体国际化翻译种子（键前缀 entity.assydefect.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyDefectI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyDefect 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assydefect 实体翻译...", tenantCode);

        foreach (var item in GetAssyDefectTranslations())
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

        TaktLogger.Information("TaktAssyDefect 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyDefect 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assydefect._self / entity.assydefect.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetAssyDefectTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assydefect._self
            new TranslationSeedItem("entity.assydefect._self", "en-US", "Assy Defect Information", "实体名称"),
            // entity.assydefect._self
            new TranslationSeedItem("entity.assydefect._self", "ja-JP", "组立不良日报信息", "实体名称"),
            // entity.assydefect._self
            new TranslationSeedItem("entity.assydefect._self", "zh-CN", "组立不良日报信息", "实体名称"),
            // entity.assydefect._self
            new TranslationSeedItem("entity.assydefect._self", "zh-HK", "组立不良日报信息", "实体名称"),

            // entity.assydefect.plantcode
            new TranslationSeedItem("entity.assydefect.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.assydefect.plantcode
            new TranslationSeedItem("entity.assydefect.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.assydefect.plantcode
            new TranslationSeedItem("entity.assydefect.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.assydefect.plantcode
            new TranslationSeedItem("entity.assydefect.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.assydefect.prodcategory
            new TranslationSeedItem("entity.assydefect.prodcategory", "en-US", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assydefect.prodcategory
            new TranslationSeedItem("entity.assydefect.prodcategory", "ja-JP", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assydefect.prodcategory
            new TranslationSeedItem("entity.assydefect.prodcategory", "zh-CN", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assydefect.prodcategory
            new TranslationSeedItem("entity.assydefect.prodcategory", "zh-HK", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),

            // entity.assydefect.proddate
            new TranslationSeedItem("entity.assydefect.proddate", "en-US", "生产日期", "生产日期"),
            // entity.assydefect.proddate
            new TranslationSeedItem("entity.assydefect.proddate", "ja-JP", "生产日期", "生产日期"),
            // entity.assydefect.proddate
            new TranslationSeedItem("entity.assydefect.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.assydefect.proddate
            new TranslationSeedItem("entity.assydefect.proddate", "zh-HK", "生产日期", "生产日期"),

            // entity.assydefect.prodline
            new TranslationSeedItem("entity.assydefect.prodline", "en-US", "生产线", "生产线"),
            // entity.assydefect.prodline
            new TranslationSeedItem("entity.assydefect.prodline", "ja-JP", "生产线", "生产线"),
            // entity.assydefect.prodline
            new TranslationSeedItem("entity.assydefect.prodline", "zh-CN", "生产线", "生产线"),
            // entity.assydefect.prodline
            new TranslationSeedItem("entity.assydefect.prodline", "zh-HK", "生产线", "生产线"),

            // entity.assydefect.shiftno
            new TranslationSeedItem("entity.assydefect.shiftno", "en-US", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assydefect.shiftno
            new TranslationSeedItem("entity.assydefect.shiftno", "ja-JP", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assydefect.shiftno
            new TranslationSeedItem("entity.assydefect.shiftno", "zh-CN", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assydefect.shiftno
            new TranslationSeedItem("entity.assydefect.shiftno", "zh-HK", "班次", "班次(1=早班 2=中班 3=晚班)"),

            // entity.assydefect.prodordercode
            new TranslationSeedItem("entity.assydefect.prodordercode", "en-US", "生产订单号", "生产订单号"),
            // entity.assydefect.prodordercode
            new TranslationSeedItem("entity.assydefect.prodordercode", "ja-JP", "生产订单号", "生产订单号"),
            // entity.assydefect.prodordercode
            new TranslationSeedItem("entity.assydefect.prodordercode", "zh-CN", "生产订单号", "生产订单号"),
            // entity.assydefect.prodordercode
            new TranslationSeedItem("entity.assydefect.prodordercode", "zh-HK", "生产订单号", "生产订单号"),

            // entity.assydefect.prodorderqty
            new TranslationSeedItem("entity.assydefect.prodorderqty", "en-US", "生产订单数量", "生产订单数量"),
            // entity.assydefect.prodorderqty
            new TranslationSeedItem("entity.assydefect.prodorderqty", "ja-JP", "生产订单数量", "生产订单数量"),
            // entity.assydefect.prodorderqty
            new TranslationSeedItem("entity.assydefect.prodorderqty", "zh-CN", "生产订单数量", "生产订单数量"),
            // entity.assydefect.prodorderqty
            new TranslationSeedItem("entity.assydefect.prodorderqty", "zh-HK", "生产订单数量", "生产订单数量"),

            // entity.assydefect.modelcode
            new TranslationSeedItem("entity.assydefect.modelcode", "en-US", "机种", "机种"),
            // entity.assydefect.modelcode
            new TranslationSeedItem("entity.assydefect.modelcode", "ja-JP", "机种", "机种"),
            // entity.assydefect.modelcode
            new TranslationSeedItem("entity.assydefect.modelcode", "zh-CN", "机种", "机种"),
            // entity.assydefect.modelcode
            new TranslationSeedItem("entity.assydefect.modelcode", "zh-HK", "机种", "机种"),

            // entity.assydefect.batchno
            new TranslationSeedItem("entity.assydefect.batchno", "en-US", "批次", "批次"),
            // entity.assydefect.batchno
            new TranslationSeedItem("entity.assydefect.batchno", "ja-JP", "批次", "批次"),
            // entity.assydefect.batchno
            new TranslationSeedItem("entity.assydefect.batchno", "zh-CN", "批次", "批次"),
            // entity.assydefect.batchno
            new TranslationSeedItem("entity.assydefect.batchno", "zh-HK", "批次", "批次"),

            // entity.assydefect.materialcode
            new TranslationSeedItem("entity.assydefect.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.assydefect.materialcode
            new TranslationSeedItem("entity.assydefect.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.assydefect.materialcode
            new TranslationSeedItem("entity.assydefect.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.assydefect.materialcode
            new TranslationSeedItem("entity.assydefect.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.assydefect.prodactualqty
            new TranslationSeedItem("entity.assydefect.prodactualqty", "en-US", "生实实绩", "生实实绩"),
            // entity.assydefect.prodactualqty
            new TranslationSeedItem("entity.assydefect.prodactualqty", "ja-JP", "生实实绩", "生实实绩"),
            // entity.assydefect.prodactualqty
            new TranslationSeedItem("entity.assydefect.prodactualqty", "zh-CN", "生实实绩", "生实实绩"),
            // entity.assydefect.prodactualqty
            new TranslationSeedItem("entity.assydefect.prodactualqty", "zh-HK", "生实实绩", "生实实绩"),

            // entity.assydefect.goodquantity
            new TranslationSeedItem("entity.assydefect.goodquantity", "en-US", "无不良数量", "无不良数量"),
            // entity.assydefect.goodquantity
            new TranslationSeedItem("entity.assydefect.goodquantity", "ja-JP", "无不良数量", "无不良数量"),
            // entity.assydefect.goodquantity
            new TranslationSeedItem("entity.assydefect.goodquantity", "zh-CN", "无不良数量", "无不良数量"),
            // entity.assydefect.goodquantity
            new TranslationSeedItem("entity.assydefect.goodquantity", "zh-HK", "无不良数量", "无不良数量"),

            // entity.assydefect.status
            new TranslationSeedItem("entity.assydefect.status", "en-US", "状态", "状态(0=正常 1=停用)"),
            // entity.assydefect.status
            new TranslationSeedItem("entity.assydefect.status", "ja-JP", "状态", "状态(0=正常 1=停用)"),
            // entity.assydefect.status
            new TranslationSeedItem("entity.assydefect.status", "zh-CN", "状态", "状态(0=正常 1=停用)"),
            // entity.assydefect.status
            new TranslationSeedItem("entity.assydefect.status", "zh-HK", "状态", "状态(0=正常 1=停用)"),

            // entity.assydefect.details
            new TranslationSeedItem("entity.assydefect.details", "en-US", "组立不良明细列表", "组立不良明细列表"),
            // entity.assydefect.details
            new TranslationSeedItem("entity.assydefect.details", "ja-JP", "组立不良明细列表", "组立不良明细列表"),
            // entity.assydefect.details
            new TranslationSeedItem("entity.assydefect.details", "zh-CN", "组立不良明细列表", "组立不良明细列表"),
            // entity.assydefect.details
            new TranslationSeedItem("entity.assydefect.details", "zh-HK", "组立不良明细列表", "组立不良明细列表"),
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
