// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaOutput 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktPcbaOutput 实体国际化翻译种子（键前缀 entity.pcbaoutput.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaOutputI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaOutput 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaoutput 实体翻译...", tenantCode);

        foreach (var item in GetPcbaOutputTranslations())
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

        TaktLogger.Information("TaktPcbaOutput 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaOutput 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaoutput._self / entity.pcbaoutput.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaOutputTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "en-US", "Pcba Output Information", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "ja-JP", "PCBA日报信息", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "zh-CN", "PCBA日报信息", "实体名称"),
            // entity.pcbaoutput._self
            new TranslationSeedItem("entity.pcbaoutput._self", "zh-HK", "PCBA日报信息", "实体名称"),

            // entity.pcbaoutput.plantcode
            new TranslationSeedItem("entity.pcbaoutput.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.pcbaoutput.plantcode
            new TranslationSeedItem("entity.pcbaoutput.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.pcbaoutput.plantcode
            new TranslationSeedItem("entity.pcbaoutput.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.pcbaoutput.plantcode
            new TranslationSeedItem("entity.pcbaoutput.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "en-US", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "ja-JP", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "zh-CN", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.pcbaoutput.prodcategory
            new TranslationSeedItem("entity.pcbaoutput.prodcategory", "zh-HK", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),

            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "en-US", "生产日期", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "ja-JP", "生产日期", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.pcbaoutput.proddate
            new TranslationSeedItem("entity.pcbaoutput.proddate", "zh-HK", "生产日期", "生产日期"),

            // entity.pcbaoutput.prodline
            new TranslationSeedItem("entity.pcbaoutput.prodline", "en-US", "生产线", "生产线"),
            // entity.pcbaoutput.prodline
            new TranslationSeedItem("entity.pcbaoutput.prodline", "ja-JP", "生产线", "生产线"),
            // entity.pcbaoutput.prodline
            new TranslationSeedItem("entity.pcbaoutput.prodline", "zh-CN", "生产线", "生产线"),
            // entity.pcbaoutput.prodline
            new TranslationSeedItem("entity.pcbaoutput.prodline", "zh-HK", "生产线", "生产线"),

            // entity.pcbaoutput.shiftno
            new TranslationSeedItem("entity.pcbaoutput.shiftno", "en-US", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaoutput.shiftno
            new TranslationSeedItem("entity.pcbaoutput.shiftno", "ja-JP", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaoutput.shiftno
            new TranslationSeedItem("entity.pcbaoutput.shiftno", "zh-CN", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaoutput.shiftno
            new TranslationSeedItem("entity.pcbaoutput.shiftno", "zh-HK", "班次", "班次(1=早班 2=中班 3=晚班)"),

            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "en-US", "生产工单号", "生产工单号"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "ja-JP", "生产工单号", "生产工单号"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.pcbaoutput.prodordercode
            new TranslationSeedItem("entity.pcbaoutput.prodordercode", "zh-HK", "生产工单号", "生产工单号"),

            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "en-US", "机种", "机种"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "ja-JP", "机种", "机种"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "zh-CN", "机种", "机种"),
            // entity.pcbaoutput.modelcode
            new TranslationSeedItem("entity.pcbaoutput.modelcode", "zh-HK", "机种", "机种"),

            // entity.pcbaoutput.batchno
            new TranslationSeedItem("entity.pcbaoutput.batchno", "en-US", "批次", "批次"),
            // entity.pcbaoutput.batchno
            new TranslationSeedItem("entity.pcbaoutput.batchno", "ja-JP", "批次", "批次"),
            // entity.pcbaoutput.batchno
            new TranslationSeedItem("entity.pcbaoutput.batchno", "zh-CN", "批次", "批次"),
            // entity.pcbaoutput.batchno
            new TranslationSeedItem("entity.pcbaoutput.batchno", "zh-HK", "批次", "批次"),

            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.pcbaoutput.materialcode
            new TranslationSeedItem("entity.pcbaoutput.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "en-US", "订单数量", "订单数量"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "ja-JP", "订单数量", "订单数量"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "zh-CN", "订单数量", "订单数量"),
            // entity.pcbaoutput.prodorderqty
            new TranslationSeedItem("entity.pcbaoutput.prodorderqty", "zh-HK", "订单数量", "订单数量"),

            // entity.pcbaoutput.stdminutes
            new TranslationSeedItem("entity.pcbaoutput.stdminutes", "en-US", "标准工时(分钟)", "标准工时(分钟)"),
            // entity.pcbaoutput.stdminutes
            new TranslationSeedItem("entity.pcbaoutput.stdminutes", "ja-JP", "标准工时(分钟)", "标准工时(分钟)"),
            // entity.pcbaoutput.stdminutes
            new TranslationSeedItem("entity.pcbaoutput.stdminutes", "zh-CN", "标准工时(分钟)", "标准工时(分钟)"),
            // entity.pcbaoutput.stdminutes
            new TranslationSeedItem("entity.pcbaoutput.stdminutes", "zh-HK", "标准工时(分钟)", "标准工时(分钟)"),

            // entity.pcbaoutput.stdshorts
            new TranslationSeedItem("entity.pcbaoutput.stdshorts", "en-US", "标准点数", "标准点数"),
            // entity.pcbaoutput.stdshorts
            new TranslationSeedItem("entity.pcbaoutput.stdshorts", "ja-JP", "标准点数", "标准点数"),
            // entity.pcbaoutput.stdshorts
            new TranslationSeedItem("entity.pcbaoutput.stdshorts", "zh-CN", "标准点数", "标准点数"),
            // entity.pcbaoutput.stdshorts
            new TranslationSeedItem("entity.pcbaoutput.stdshorts", "zh-HK", "标准点数", "标准点数"),

            // entity.pcbaoutput.stdcapacity
            new TranslationSeedItem("entity.pcbaoutput.stdcapacity", "en-US", "标准产能", "标准产能"),
            // entity.pcbaoutput.stdcapacity
            new TranslationSeedItem("entity.pcbaoutput.stdcapacity", "ja-JP", "标准产能", "标准产能"),
            // entity.pcbaoutput.stdcapacity
            new TranslationSeedItem("entity.pcbaoutput.stdcapacity", "zh-CN", "标准产能", "标准产能"),
            // entity.pcbaoutput.stdcapacity
            new TranslationSeedItem("entity.pcbaoutput.stdcapacity", "zh-HK", "标准产能", "标准产能"),

            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "en-US", "PCBA明细列表", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "ja-JP", "PCBA明细列表", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "zh-CN", "PCBA明细列表", "PCBA明细列表"),
            // entity.pcbaoutput.details
            new TranslationSeedItem("entity.pcbaoutput.details", "zh-HK", "PCBA明细列表", "PCBA明细列表"),
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
