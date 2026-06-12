// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRouting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktRouting 实体国际化翻译种子（键前缀 entity.routing.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoutingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRouting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 routing 实体翻译...", tenantCode);

        foreach (var item in GetRoutingTranslations())
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

        TaktLogger.Information("TaktRouting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRouting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.routing._self / entity.routing.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetRoutingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.routing._self
            new TranslationSeedItem("entity.routing._self", "en-US", "Routing Information", "实体名称"),
            // entity.routing._self
            new TranslationSeedItem("entity.routing._self", "ja-JP", "工艺路线主表信息", "实体名称"),
            // entity.routing._self
            new TranslationSeedItem("entity.routing._self", "zh-CN", "工艺路线主表信息", "实体名称"),
            // entity.routing._self
            new TranslationSeedItem("entity.routing._self", "zh-HK", "工艺路线主表信息", "实体名称"),

            // entity.routing.plantcode
            new TranslationSeedItem("entity.routing.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.routing.plantcode
            new TranslationSeedItem("entity.routing.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.routing.plantcode
            new TranslationSeedItem("entity.routing.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.routing.plantcode
            new TranslationSeedItem("entity.routing.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.routing.workcenter
            new TranslationSeedItem("entity.routing.workcenter", "en-US", "工作中心", "工作中心"),
            // entity.routing.workcenter
            new TranslationSeedItem("entity.routing.workcenter", "ja-JP", "工作中心", "工作中心"),
            // entity.routing.workcenter
            new TranslationSeedItem("entity.routing.workcenter", "zh-CN", "工作中心", "工作中心"),
            // entity.routing.workcenter
            new TranslationSeedItem("entity.routing.workcenter", "zh-HK", "工作中心", "工作中心"),

            // entity.routing.code
            new TranslationSeedItem("entity.routing.code", "en-US", "工艺路线编码", "工艺路线编码"),
            // entity.routing.code
            new TranslationSeedItem("entity.routing.code", "ja-JP", "工艺路线编码", "工艺路线编码"),
            // entity.routing.code
            new TranslationSeedItem("entity.routing.code", "zh-CN", "工艺路线编码", "工艺路线编码"),
            // entity.routing.code
            new TranslationSeedItem("entity.routing.code", "zh-HK", "工艺路线编码", "工艺路线编码"),

            // entity.routing.name
            new TranslationSeedItem("entity.routing.name", "en-US", "工艺路线名称", "工艺路线名称"),
            // entity.routing.name
            new TranslationSeedItem("entity.routing.name", "ja-JP", "工艺路线名称", "工艺路线名称"),
            // entity.routing.name
            new TranslationSeedItem("entity.routing.name", "zh-CN", "工艺路线名称", "工艺路线名称"),
            // entity.routing.name
            new TranslationSeedItem("entity.routing.name", "zh-HK", "工艺路线名称", "工艺路线名称"),

            // entity.routing.purpose
            new TranslationSeedItem("entity.routing.purpose", "en-US", "用途", "用途（1=生产，2=工程/设计，3=万能，4=工厂维护）"),
            // entity.routing.purpose
            new TranslationSeedItem("entity.routing.purpose", "ja-JP", "用途", "用途（1=生产，2=工程/设计，3=万能，4=工厂维护）"),
            // entity.routing.purpose
            new TranslationSeedItem("entity.routing.purpose", "zh-CN", "用途", "用途（1=生产，2=工程/设计，3=万能，4=工厂维护）"),
            // entity.routing.purpose
            new TranslationSeedItem("entity.routing.purpose", "zh-HK", "用途", "用途（1=生产，2=工程/设计，3=万能，4=工厂维护）"),

            // entity.routing.materialcode
            new TranslationSeedItem("entity.routing.materialcode", "en-US", "物料编码", "适用物料编码"),
            // entity.routing.materialcode
            new TranslationSeedItem("entity.routing.materialcode", "ja-JP", "物料编码", "适用物料编码"),
            // entity.routing.materialcode
            new TranslationSeedItem("entity.routing.materialcode", "zh-CN", "物料编码", "适用物料编码"),
            // entity.routing.materialcode
            new TranslationSeedItem("entity.routing.materialcode", "zh-HK", "物料编码", "适用物料编码"),

            // entity.routing.version
            new TranslationSeedItem("entity.routing.version", "en-US", "版本号", "版本号"),
            // entity.routing.version
            new TranslationSeedItem("entity.routing.version", "ja-JP", "版本号", "版本号"),
            // entity.routing.version
            new TranslationSeedItem("entity.routing.version", "zh-CN", "版本号", "版本号"),
            // entity.routing.version
            new TranslationSeedItem("entity.routing.version", "zh-HK", "版本号", "版本号"),

            // entity.routing.status
            new TranslationSeedItem("entity.routing.status", "en-US", "状态", "状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））"),
            // entity.routing.status
            new TranslationSeedItem("entity.routing.status", "ja-JP", "状态", "状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））"),
            // entity.routing.status
            new TranslationSeedItem("entity.routing.status", "zh-CN", "状态", "状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））"),
            // entity.routing.status
            new TranslationSeedItem("entity.routing.status", "zh-HK", "状态", "状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））"),

            // entity.routing.effectivedate
            new TranslationSeedItem("entity.routing.effectivedate", "en-US", "生效日期", "生效日期"),
            // entity.routing.effectivedate
            new TranslationSeedItem("entity.routing.effectivedate", "ja-JP", "生效日期", "生效日期"),
            // entity.routing.effectivedate
            new TranslationSeedItem("entity.routing.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.routing.effectivedate
            new TranslationSeedItem("entity.routing.effectivedate", "zh-HK", "生效日期", "生效日期"),

            // entity.routing.expirydate
            new TranslationSeedItem("entity.routing.expirydate", "en-US", "失效日期", "失效日期"),
            // entity.routing.expirydate
            new TranslationSeedItem("entity.routing.expirydate", "ja-JP", "失效日期", "失效日期"),
            // entity.routing.expirydate
            new TranslationSeedItem("entity.routing.expirydate", "zh-CN", "失效日期", "失效日期"),
            // entity.routing.expirydate
            new TranslationSeedItem("entity.routing.expirydate", "zh-HK", "失效日期", "失效日期"),

            // entity.routing.description
            new TranslationSeedItem("entity.routing.description", "en-US", "工艺路线说明", "工艺路线说明"),
            // entity.routing.description
            new TranslationSeedItem("entity.routing.description", "ja-JP", "工艺路线说明", "工艺路线说明"),
            // entity.routing.description
            new TranslationSeedItem("entity.routing.description", "zh-CN", "工艺路线说明", "工艺路线说明"),
            // entity.routing.description
            new TranslationSeedItem("entity.routing.description", "zh-HK", "工艺路线说明", "工艺路线说明"),

            // entity.routing.items
            new TranslationSeedItem("entity.routing.items", "en-US", "工艺路线明细列表", "工艺路线明细列表（主子表关系）"),
            // entity.routing.items
            new TranslationSeedItem("entity.routing.items", "ja-JP", "工艺路线明细列表", "工艺路线明细列表（主子表关系）"),
            // entity.routing.items
            new TranslationSeedItem("entity.routing.items", "zh-CN", "工艺路线明细列表", "工艺路线明细列表（主子表关系）"),
            // entity.routing.items
            new TranslationSeedItem("entity.routing.items", "zh-HK", "工艺路线明细列表", "工艺路线明细列表（主子表关系）"),

            // entity.routing.changelogs
            new TranslationSeedItem("entity.routing.changelogs", "en-US", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.routing.changelogs
            new TranslationSeedItem("entity.routing.changelogs", "ja-JP", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.routing.changelogs
            new TranslationSeedItem("entity.routing.changelogs", "zh-CN", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.routing.changelogs
            new TranslationSeedItem("entity.routing.changelogs", "zh-HK", "变更日志列表", "变更日志列表（主子表关系）"),
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
