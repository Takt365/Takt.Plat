// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Scheduling;

/// <summary>
/// TaktApsSchedule 实体国际化翻译种子（键前缀 entity.apsSchedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsSchedule 实体翻译...", tenantCode);

        foreach (var item in GetApsScheduleTranslations())
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

        TaktLogger.Information("TaktApsSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsSchedule._self / entity.apsSchedule.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsSchedule._self
            new TranslationSeedItem("entity.apsSchedule._self", "en-US", "Aps Schedule Information", "实体名称"),
            // entity.apsSchedule._self
            new TranslationSeedItem("entity.apsSchedule._self", "ja-JP", "APS排程主表信息", "实体名称"),
            // entity.apsSchedule._self
            new TranslationSeedItem("entity.apsSchedule._self", "zh-CN", "APS排程主表信息", "实体名称"),
            // entity.apsSchedule._self
            new TranslationSeedItem("entity.apsSchedule._self", "zh-HK", "APS排程主表信息", "实体名称"),

            // entity.apsSchedule.plantcode
            new TranslationSeedItem("entity.apsSchedule.plantcode", "en-US", "工厂编码", "工厂编码（不可空）"),
            // entity.apsSchedule.plantcode
            new TranslationSeedItem("entity.apsSchedule.plantcode", "ja-JP", "工厂编码", "工厂编码（不可空）"),
            // entity.apsSchedule.plantcode
            new TranslationSeedItem("entity.apsSchedule.plantcode", "zh-CN", "工厂编码", "工厂编码（不可空）"),
            // entity.apsSchedule.plantcode
            new TranslationSeedItem("entity.apsSchedule.plantcode", "zh-HK", "工厂编码", "工厂编码（不可空）"),

            // entity.apsSchedule.schedulecode
            new TranslationSeedItem("entity.apsSchedule.schedulecode", "en-US", "排程编码", "排程编码（唯一索引）"),
            // entity.apsSchedule.schedulecode
            new TranslationSeedItem("entity.apsSchedule.schedulecode", "ja-JP", "排程编码", "排程编码（唯一索引）"),
            // entity.apsSchedule.schedulecode
            new TranslationSeedItem("entity.apsSchedule.schedulecode", "zh-CN", "排程编码", "排程编码（唯一索引）"),
            // entity.apsSchedule.schedulecode
            new TranslationSeedItem("entity.apsSchedule.schedulecode", "zh-HK", "排程编码", "排程编码（唯一索引）"),

            // entity.apsSchedule.schedulename
            new TranslationSeedItem("entity.apsSchedule.schedulename", "en-US", "排程名称", "排程名称"),
            // entity.apsSchedule.schedulename
            new TranslationSeedItem("entity.apsSchedule.schedulename", "ja-JP", "排程名称", "排程名称"),
            // entity.apsSchedule.schedulename
            new TranslationSeedItem("entity.apsSchedule.schedulename", "zh-CN", "排程名称", "排程名称"),
            // entity.apsSchedule.schedulename
            new TranslationSeedItem("entity.apsSchedule.schedulename", "zh-HK", "排程名称", "排程名称"),

            // entity.apsSchedule.scheduletype
            new TranslationSeedItem("entity.apsSchedule.scheduletype", "en-US", "排程类型", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsSchedule.scheduletype
            new TranslationSeedItem("entity.apsSchedule.scheduletype", "ja-JP", "排程类型", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsSchedule.scheduletype
            new TranslationSeedItem("entity.apsSchedule.scheduletype", "zh-CN", "排程类型", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),
            // entity.apsSchedule.scheduletype
            new TranslationSeedItem("entity.apsSchedule.scheduletype", "zh-HK", "排程类型", "排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）"),

            // entity.apsSchedule.plandate
            new TranslationSeedItem("entity.apsSchedule.plandate", "en-US", "计划日期", "计划日期"),
            // entity.apsSchedule.plandate
            new TranslationSeedItem("entity.apsSchedule.plandate", "ja-JP", "计划日期", "计划日期"),
            // entity.apsSchedule.plandate
            new TranslationSeedItem("entity.apsSchedule.plandate", "zh-CN", "计划日期", "计划日期"),
            // entity.apsSchedule.plandate
            new TranslationSeedItem("entity.apsSchedule.plandate", "zh-HK", "计划日期", "计划日期"),

            // entity.apsSchedule.planstarttime
            new TranslationSeedItem("entity.apsSchedule.planstarttime", "en-US", "计划开始时间", "计划开始时间"),
            // entity.apsSchedule.planstarttime
            new TranslationSeedItem("entity.apsSchedule.planstarttime", "ja-JP", "计划开始时间", "计划开始时间"),
            // entity.apsSchedule.planstarttime
            new TranslationSeedItem("entity.apsSchedule.planstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsSchedule.planstarttime
            new TranslationSeedItem("entity.apsSchedule.planstarttime", "zh-HK", "计划开始时间", "计划开始时间"),

            // entity.apsSchedule.planendtime
            new TranslationSeedItem("entity.apsSchedule.planendtime", "en-US", "计划结束时间", "计划结束时间"),
            // entity.apsSchedule.planendtime
            new TranslationSeedItem("entity.apsSchedule.planendtime", "ja-JP", "计划结束时间", "计划结束时间"),
            // entity.apsSchedule.planendtime
            new TranslationSeedItem("entity.apsSchedule.planendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsSchedule.planendtime
            new TranslationSeedItem("entity.apsSchedule.planendtime", "zh-HK", "计划结束时间", "计划结束时间"),

            // entity.apsSchedule.plancycle
            new TranslationSeedItem("entity.apsSchedule.plancycle", "en-US", "计划周期", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsSchedule.plancycle
            new TranslationSeedItem("entity.apsSchedule.plancycle", "ja-JP", "计划周期", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsSchedule.plancycle
            new TranslationSeedItem("entity.apsSchedule.plancycle", "zh-CN", "计划周期", "计划周期（0=日计划，1=周计划，2=月计划）"),
            // entity.apsSchedule.plancycle
            new TranslationSeedItem("entity.apsSchedule.plancycle", "zh-HK", "计划周期", "计划周期（0=日计划，1=周计划，2=月计划）"),

            // entity.apsSchedule.workshopcode
            new TranslationSeedItem("entity.apsSchedule.workshopcode", "en-US", "车间编码", "车间编码"),
            // entity.apsSchedule.workshopcode
            new TranslationSeedItem("entity.apsSchedule.workshopcode", "ja-JP", "车间编码", "车间编码"),
            // entity.apsSchedule.workshopcode
            new TranslationSeedItem("entity.apsSchedule.workshopcode", "zh-CN", "车间编码", "车间编码"),
            // entity.apsSchedule.workshopcode
            new TranslationSeedItem("entity.apsSchedule.workshopcode", "zh-HK", "车间编码", "车间编码"),

            // entity.apsSchedule.workshopname
            new TranslationSeedItem("entity.apsSchedule.workshopname", "en-US", "车间名称", "车间名称"),
            // entity.apsSchedule.workshopname
            new TranslationSeedItem("entity.apsSchedule.workshopname", "ja-JP", "车间名称", "车间名称"),
            // entity.apsSchedule.workshopname
            new TranslationSeedItem("entity.apsSchedule.workshopname", "zh-CN", "车间名称", "车间名称"),
            // entity.apsSchedule.workshopname
            new TranslationSeedItem("entity.apsSchedule.workshopname", "zh-HK", "车间名称", "车间名称"),

            // entity.apsSchedule.productionlinecode
            new TranslationSeedItem("entity.apsSchedule.productionlinecode", "en-US", "生产线编码", "生产线编码"),
            // entity.apsSchedule.productionlinecode
            new TranslationSeedItem("entity.apsSchedule.productionlinecode", "ja-JP", "生产线编码", "生产线编码"),
            // entity.apsSchedule.productionlinecode
            new TranslationSeedItem("entity.apsSchedule.productionlinecode", "zh-CN", "生产线编码", "生产线编码"),
            // entity.apsSchedule.productionlinecode
            new TranslationSeedItem("entity.apsSchedule.productionlinecode", "zh-HK", "生产线编码", "生产线编码"),

            // entity.apsSchedule.productionlinename
            new TranslationSeedItem("entity.apsSchedule.productionlinename", "en-US", "生产线名称", "生产线名称"),
            // entity.apsSchedule.productionlinename
            new TranslationSeedItem("entity.apsSchedule.productionlinename", "ja-JP", "生产线名称", "生产线名称"),
            // entity.apsSchedule.productionlinename
            new TranslationSeedItem("entity.apsSchedule.productionlinename", "zh-CN", "生产线名称", "生产线名称"),
            // entity.apsSchedule.productionlinename
            new TranslationSeedItem("entity.apsSchedule.productionlinename", "zh-HK", "生产线名称", "生产线名称"),

            // entity.apsSchedule.schedulestrategy
            new TranslationSeedItem("entity.apsSchedule.schedulestrategy", "en-US", "排程策略", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsSchedule.schedulestrategy
            new TranslationSeedItem("entity.apsSchedule.schedulestrategy", "ja-JP", "排程策略", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsSchedule.schedulestrategy
            new TranslationSeedItem("entity.apsSchedule.schedulestrategy", "zh-CN", "排程策略", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),
            // entity.apsSchedule.schedulestrategy
            new TranslationSeedItem("entity.apsSchedule.schedulestrategy", "zh-HK", "排程策略", "排程策略（0=按订单排程，1=按库存排程，2=混合排程）"),

            // entity.apsSchedule.schedulealgorithm
            new TranslationSeedItem("entity.apsSchedule.schedulealgorithm", "en-US", "排程算法", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsSchedule.schedulealgorithm
            new TranslationSeedItem("entity.apsSchedule.schedulealgorithm", "ja-JP", "排程算法", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsSchedule.schedulealgorithm
            new TranslationSeedItem("entity.apsSchedule.schedulealgorithm", "zh-CN", "排程算法", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),
            // entity.apsSchedule.schedulealgorithm
            new TranslationSeedItem("entity.apsSchedule.schedulealgorithm", "zh-HK", "排程算法", "排程算法（0=正向排程，1=逆向排程，2=双向排程）"),

            // entity.apsSchedule.optimizationobjective
            new TranslationSeedItem("entity.apsSchedule.optimizationobjective", "en-US", "优化目标", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsSchedule.optimizationobjective
            new TranslationSeedItem("entity.apsSchedule.optimizationobjective", "ja-JP", "优化目标", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsSchedule.optimizationobjective
            new TranslationSeedItem("entity.apsSchedule.optimizationobjective", "zh-CN", "优化目标", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),
            // entity.apsSchedule.optimizationobjective
            new TranslationSeedItem("entity.apsSchedule.optimizationobjective", "zh-HK", "优化目标", "优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）"),

            // entity.apsSchedule.schedulestatus
            new TranslationSeedItem("entity.apsSchedule.schedulestatus", "en-US", "排程状态", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsSchedule.schedulestatus
            new TranslationSeedItem("entity.apsSchedule.schedulestatus", "ja-JP", "排程状态", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsSchedule.schedulestatus
            new TranslationSeedItem("entity.apsSchedule.schedulestatus", "zh-CN", "排程状态", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),
            // entity.apsSchedule.schedulestatus
            new TranslationSeedItem("entity.apsSchedule.schedulestatus", "zh-HK", "排程状态", "排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）"),

            // entity.apsSchedule.plannerid
            new TranslationSeedItem("entity.apsSchedule.plannerid", "en-US", "计划员ID", "计划员ID"),
            // entity.apsSchedule.plannerid
            new TranslationSeedItem("entity.apsSchedule.plannerid", "ja-JP", "计划员ID", "计划员ID"),
            // entity.apsSchedule.plannerid
            new TranslationSeedItem("entity.apsSchedule.plannerid", "zh-CN", "计划员ID", "计划员ID"),
            // entity.apsSchedule.plannerid
            new TranslationSeedItem("entity.apsSchedule.plannerid", "zh-HK", "计划员ID", "计划员ID"),

            // entity.apsSchedule.plannername
            new TranslationSeedItem("entity.apsSchedule.plannername", "en-US", "计划员姓名", "计划员姓名"),
            // entity.apsSchedule.plannername
            new TranslationSeedItem("entity.apsSchedule.plannername", "ja-JP", "计划员姓名", "计划员姓名"),
            // entity.apsSchedule.plannername
            new TranslationSeedItem("entity.apsSchedule.plannername", "zh-CN", "计划员姓名", "计划员姓名"),
            // entity.apsSchedule.plannername
            new TranslationSeedItem("entity.apsSchedule.plannername", "zh-HK", "计划员姓名", "计划员姓名"),

            // entity.apsSchedule.publishtime
            new TranslationSeedItem("entity.apsSchedule.publishtime", "en-US", "发布时间", "发布时间"),
            // entity.apsSchedule.publishtime
            new TranslationSeedItem("entity.apsSchedule.publishtime", "ja-JP", "发布时间", "发布时间"),
            // entity.apsSchedule.publishtime
            new TranslationSeedItem("entity.apsSchedule.publishtime", "zh-CN", "发布时间", "发布时间"),
            // entity.apsSchedule.publishtime
            new TranslationSeedItem("entity.apsSchedule.publishtime", "zh-HK", "发布时间", "发布时间"),

            // entity.apsSchedule.publishuserid
            new TranslationSeedItem("entity.apsSchedule.publishuserid", "en-US", "发布人ID", "发布人ID"),
            // entity.apsSchedule.publishuserid
            new TranslationSeedItem("entity.apsSchedule.publishuserid", "ja-JP", "发布人ID", "发布人ID"),
            // entity.apsSchedule.publishuserid
            new TranslationSeedItem("entity.apsSchedule.publishuserid", "zh-CN", "发布人ID", "发布人ID"),
            // entity.apsSchedule.publishuserid
            new TranslationSeedItem("entity.apsSchedule.publishuserid", "zh-HK", "发布人ID", "发布人ID"),

            // entity.apsSchedule.publishusername
            new TranslationSeedItem("entity.apsSchedule.publishusername", "en-US", "发布人姓名", "发布人姓名"),
            // entity.apsSchedule.publishusername
            new TranslationSeedItem("entity.apsSchedule.publishusername", "ja-JP", "发布人姓名", "发布人姓名"),
            // entity.apsSchedule.publishusername
            new TranslationSeedItem("entity.apsSchedule.publishusername", "zh-CN", "发布人姓名", "发布人姓名"),
            // entity.apsSchedule.publishusername
            new TranslationSeedItem("entity.apsSchedule.publishusername", "zh-HK", "发布人姓名", "发布人姓名"),

            // entity.apsSchedule.scheduledescription
            new TranslationSeedItem("entity.apsSchedule.scheduledescription", "en-US", "排程说明", "排程说明"),
            // entity.apsSchedule.scheduledescription
            new TranslationSeedItem("entity.apsSchedule.scheduledescription", "ja-JP", "排程说明", "排程说明"),
            // entity.apsSchedule.scheduledescription
            new TranslationSeedItem("entity.apsSchedule.scheduledescription", "zh-CN", "排程说明", "排程说明"),
            // entity.apsSchedule.scheduledescription
            new TranslationSeedItem("entity.apsSchedule.scheduledescription", "zh-HK", "排程说明", "排程说明"),

            // entity.apsSchedule.items
            new TranslationSeedItem("entity.apsSchedule.items", "en-US", "排程明细列表", "排程明细列表（主子表关系）"),
            // entity.apsSchedule.items
            new TranslationSeedItem("entity.apsSchedule.items", "ja-JP", "排程明细列表", "排程明细列表（主子表关系）"),
            // entity.apsSchedule.items
            new TranslationSeedItem("entity.apsSchedule.items", "zh-CN", "排程明细列表", "排程明细列表（主子表关系）"),
            // entity.apsSchedule.items
            new TranslationSeedItem("entity.apsSchedule.items", "zh-HK", "排程明细列表", "排程明细列表（主子表关系）"),

            // entity.apsSchedule.changelogs
            new TranslationSeedItem("entity.apsSchedule.changelogs", "en-US", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.apsSchedule.changelogs
            new TranslationSeedItem("entity.apsSchedule.changelogs", "ja-JP", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.apsSchedule.changelogs
            new TranslationSeedItem("entity.apsSchedule.changelogs", "zh-CN", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.apsSchedule.changelogs
            new TranslationSeedItem("entity.apsSchedule.changelogs", "zh-HK", "变更日志列表", "变更日志列表（主子表关系）"),
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
