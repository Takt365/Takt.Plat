// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyOutput 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssyOutput 实体国际化翻译种子（键前缀 entity.assyoutput.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyOutputI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyOutput 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assyoutput 实体翻译...", tenantCode);

        foreach (var item in GetAssyOutputTranslations())
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

        TaktLogger.Information("TaktAssyOutput 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyOutput 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assyoutput._self / entity.assyoutput.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyOutputTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assyoutput._self
            new TranslationSeedItem("entity.assyoutput._self", "en-US", "Assy Output Information_us", "实体名称"),
            // entity.assyoutput._self
            new TranslationSeedItem("entity.assyoutput._self", "ja-JP", "组立日报信息_jp", "实体名称"),
            // entity.assyoutput._self
            new TranslationSeedItem("entity.assyoutput._self", "zh-CN", "组立日报信息", "实体名称"),
            // entity.assyoutput._self
            new TranslationSeedItem("entity.assyoutput._self", "zh-HK", "组立日报信息_hk", "实体名称"),

            // entity.assyoutput.plantcode
            new TranslationSeedItem("entity.assyoutput.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.assyoutput.plantcode
            new TranslationSeedItem("entity.assyoutput.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.assyoutput.plantcode
            new TranslationSeedItem("entity.assyoutput.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.assyoutput.plantcode
            new TranslationSeedItem("entity.assyoutput.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.assyoutput.prodcategory
            new TranslationSeedItem("entity.assyoutput.prodcategory", "en-US", "生产类别_us", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assyoutput.prodcategory
            new TranslationSeedItem("entity.assyoutput.prodcategory", "ja-JP", "生产类别_jp", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assyoutput.prodcategory
            new TranslationSeedItem("entity.assyoutput.prodcategory", "zh-CN", "生产类别", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),
            // entity.assyoutput.prodcategory
            new TranslationSeedItem("entity.assyoutput.prodcategory", "zh-HK", "生产类别_hk", "生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产"),

            // entity.assyoutput.proddate
            new TranslationSeedItem("entity.assyoutput.proddate", "en-US", "生产日期_us", "生产日期"),
            // entity.assyoutput.proddate
            new TranslationSeedItem("entity.assyoutput.proddate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.assyoutput.proddate
            new TranslationSeedItem("entity.assyoutput.proddate", "zh-CN", "生产日期", "生产日期"),
            // entity.assyoutput.proddate
            new TranslationSeedItem("entity.assyoutput.proddate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.assyoutput.prodline
            new TranslationSeedItem("entity.assyoutput.prodline", "en-US", "生产线_us", "生产线"),
            // entity.assyoutput.prodline
            new TranslationSeedItem("entity.assyoutput.prodline", "ja-JP", "生产线_jp", "生产线"),
            // entity.assyoutput.prodline
            new TranslationSeedItem("entity.assyoutput.prodline", "zh-CN", "生产线", "生产线"),
            // entity.assyoutput.prodline
            new TranslationSeedItem("entity.assyoutput.prodline", "zh-HK", "生产线_hk", "生产线"),

            // entity.assyoutput.directlabor
            new TranslationSeedItem("entity.assyoutput.directlabor", "en-US", "直接人员_us", "直接人员"),
            // entity.assyoutput.directlabor
            new TranslationSeedItem("entity.assyoutput.directlabor", "ja-JP", "直接人员_jp", "直接人员"),
            // entity.assyoutput.directlabor
            new TranslationSeedItem("entity.assyoutput.directlabor", "zh-CN", "直接人员", "直接人员"),
            // entity.assyoutput.directlabor
            new TranslationSeedItem("entity.assyoutput.directlabor", "zh-HK", "直接人员_hk", "直接人员"),

            // entity.assyoutput.indirectlabor
            new TranslationSeedItem("entity.assyoutput.indirectlabor", "en-US", "间接人员_us", "间接人员"),
            // entity.assyoutput.indirectlabor
            new TranslationSeedItem("entity.assyoutput.indirectlabor", "ja-JP", "间接人员_jp", "间接人员"),
            // entity.assyoutput.indirectlabor
            new TranslationSeedItem("entity.assyoutput.indirectlabor", "zh-CN", "间接人员", "间接人员"),
            // entity.assyoutput.indirectlabor
            new TranslationSeedItem("entity.assyoutput.indirectlabor", "zh-HK", "间接人员_hk", "间接人员"),

            // entity.assyoutput.shiftno
            new TranslationSeedItem("entity.assyoutput.shiftno", "en-US", "班次_us", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assyoutput.shiftno
            new TranslationSeedItem("entity.assyoutput.shiftno", "ja-JP", "班次_jp", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assyoutput.shiftno
            new TranslationSeedItem("entity.assyoutput.shiftno", "zh-CN", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.assyoutput.shiftno
            new TranslationSeedItem("entity.assyoutput.shiftno", "zh-HK", "班次_hk", "班次(1=早班 2=中班 3=晚班)"),

            // entity.assyoutput.prodordertype
            new TranslationSeedItem("entity.assyoutput.prodordertype", "en-US", "生产订单类型_us", "生产订单类型"),
            // entity.assyoutput.prodordertype
            new TranslationSeedItem("entity.assyoutput.prodordertype", "ja-JP", "生产订单类型_jp", "生产订单类型"),
            // entity.assyoutput.prodordertype
            new TranslationSeedItem("entity.assyoutput.prodordertype", "zh-CN", "生产订单类型", "生产订单类型"),
            // entity.assyoutput.prodordertype
            new TranslationSeedItem("entity.assyoutput.prodordertype", "zh-HK", "生产订单类型_hk", "生产订单类型"),

            // entity.assyoutput.prodordercode
            new TranslationSeedItem("entity.assyoutput.prodordercode", "en-US", "生产工单号_us", "生产工单号"),
            // entity.assyoutput.prodordercode
            new TranslationSeedItem("entity.assyoutput.prodordercode", "ja-JP", "生产工单号_jp", "生产工单号"),
            // entity.assyoutput.prodordercode
            new TranslationSeedItem("entity.assyoutput.prodordercode", "zh-CN", "生产工单号", "生产工单号"),
            // entity.assyoutput.prodordercode
            new TranslationSeedItem("entity.assyoutput.prodordercode", "zh-HK", "生产工单号_hk", "生产工单号"),

            // entity.assyoutput.modelcode
            new TranslationSeedItem("entity.assyoutput.modelcode", "en-US", "机种_us", "机种"),
            // entity.assyoutput.modelcode
            new TranslationSeedItem("entity.assyoutput.modelcode", "ja-JP", "机种_jp", "机种"),
            // entity.assyoutput.modelcode
            new TranslationSeedItem("entity.assyoutput.modelcode", "zh-CN", "机种", "机种"),
            // entity.assyoutput.modelcode
            new TranslationSeedItem("entity.assyoutput.modelcode", "zh-HK", "机种_hk", "机种"),

            // entity.assyoutput.materialcode
            new TranslationSeedItem("entity.assyoutput.materialcode", "en-US", "物料编码_us", "物料编码"),
            // entity.assyoutput.materialcode
            new TranslationSeedItem("entity.assyoutput.materialcode", "ja-JP", "物料编码_jp", "物料编码"),
            // entity.assyoutput.materialcode
            new TranslationSeedItem("entity.assyoutput.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.assyoutput.materialcode
            new TranslationSeedItem("entity.assyoutput.materialcode", "zh-HK", "物料编码_hk", "物料编码"),

            // entity.assyoutput.batchno
            new TranslationSeedItem("entity.assyoutput.batchno", "en-US", "批次_us", "批次"),
            // entity.assyoutput.batchno
            new TranslationSeedItem("entity.assyoutput.batchno", "ja-JP", "批次_jp", "批次"),
            // entity.assyoutput.batchno
            new TranslationSeedItem("entity.assyoutput.batchno", "zh-CN", "批次", "批次"),
            // entity.assyoutput.batchno
            new TranslationSeedItem("entity.assyoutput.batchno", "zh-HK", "批次_hk", "批次"),

            // entity.assyoutput.prodorderqty
            new TranslationSeedItem("entity.assyoutput.prodorderqty", "en-US", "订单数量_us", "订单数量"),
            // entity.assyoutput.prodorderqty
            new TranslationSeedItem("entity.assyoutput.prodorderqty", "ja-JP", "订单数量_jp", "订单数量"),
            // entity.assyoutput.prodorderqty
            new TranslationSeedItem("entity.assyoutput.prodorderqty", "zh-CN", "订单数量", "订单数量"),
            // entity.assyoutput.prodorderqty
            new TranslationSeedItem("entity.assyoutput.prodorderqty", "zh-HK", "订单数量_hk", "订单数量"),

            // entity.assyoutput.stdminutes
            new TranslationSeedItem("entity.assyoutput.stdminutes", "en-US", "标准工时(分钟)_us", "标准工时(分钟)"),
            // entity.assyoutput.stdminutes
            new TranslationSeedItem("entity.assyoutput.stdminutes", "ja-JP", "标准工时(分钟)_jp", "标准工时(分钟)"),
            // entity.assyoutput.stdminutes
            new TranslationSeedItem("entity.assyoutput.stdminutes", "zh-CN", "标准工时(分钟)", "标准工时(分钟)"),
            // entity.assyoutput.stdminutes
            new TranslationSeedItem("entity.assyoutput.stdminutes", "zh-HK", "标准工时(分钟)_hk", "标准工时(分钟)"),

            // entity.assyoutput.stdcapacity
            new TranslationSeedItem("entity.assyoutput.stdcapacity", "en-US", "标准产能_us", "标准产能"),
            // entity.assyoutput.stdcapacity
            new TranslationSeedItem("entity.assyoutput.stdcapacity", "ja-JP", "标准产能_jp", "标准产能"),
            // entity.assyoutput.stdcapacity
            new TranslationSeedItem("entity.assyoutput.stdcapacity", "zh-CN", "标准产能", "标准产能"),
            // entity.assyoutput.stdcapacity
            new TranslationSeedItem("entity.assyoutput.stdcapacity", "zh-HK", "标准产能_hk", "标准产能"),

            // entity.assyoutput.status
            new TranslationSeedItem("entity.assyoutput.status", "en-US", "状态_us", "状态"),
            // entity.assyoutput.status
            new TranslationSeedItem("entity.assyoutput.status", "ja-JP", "状态_jp", "状态"),
            // entity.assyoutput.status
            new TranslationSeedItem("entity.assyoutput.status", "zh-CN", "状态", "状态"),
            // entity.assyoutput.status
            new TranslationSeedItem("entity.assyoutput.status", "zh-HK", "状态_hk", "状态"),

            // entity.assyoutput.details
            new TranslationSeedItem("entity.assyoutput.details", "en-US", "组立日报明细列表_us", "组立日报明细列表"),
            // entity.assyoutput.details
            new TranslationSeedItem("entity.assyoutput.details", "ja-JP", "组立日报明细列表_jp", "组立日报明细列表"),
            // entity.assyoutput.details
            new TranslationSeedItem("entity.assyoutput.details", "zh-CN", "组立日报明细列表", "组立日报明细列表"),
            // entity.assyoutput.details
            new TranslationSeedItem("entity.assyoutput.details", "zh-HK", "组立日报明细列表_hk", "组立日报明细列表"),
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
        translation.ResourceGroup = "Output";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
