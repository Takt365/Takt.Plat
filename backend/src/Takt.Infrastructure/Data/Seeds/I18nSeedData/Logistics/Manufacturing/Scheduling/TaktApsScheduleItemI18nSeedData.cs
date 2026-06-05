// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleItemI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsScheduleItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktApsScheduleItem 实体国际化翻译种子（键前缀 entity.apsScheduleItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsScheduleItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsScheduleItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsScheduleItem 实体翻译...", tenantCode);

        foreach (var item in GetApsScheduleItemTranslations())
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

        TaktLogger.Information("TaktApsScheduleItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsScheduleItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsScheduleItem._self / entity.apsScheduleItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsScheduleItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsScheduleItem._self
            new TranslationSeedItem("entity.apsScheduleItem._self", "en-US", "Aps Schedule Item Information", "实体名称"),
            // entity.apsScheduleItem._self
            new TranslationSeedItem("entity.apsScheduleItem._self", "ja-JP", "APS排程明细信息", "实体名称"),
            // entity.apsScheduleItem._self
            new TranslationSeedItem("entity.apsScheduleItem._self", "zh-CN", "APS排程明细信息", "实体名称"),
            // entity.apsScheduleItem._self
            new TranslationSeedItem("entity.apsScheduleItem._self", "zh-HK", "APS排程明细信息", "实体名称"),

            // entity.apsScheduleItem.apsscheduleid
            new TranslationSeedItem("entity.apsScheduleItem.apsscheduleid", "en-US", "APS排程ID", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsScheduleItem.apsscheduleid
            new TranslationSeedItem("entity.apsScheduleItem.apsscheduleid", "ja-JP", "APS排程ID", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsScheduleItem.apsscheduleid
            new TranslationSeedItem("entity.apsScheduleItem.apsscheduleid", "zh-CN", "APS排程ID", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.apsScheduleItem.apsscheduleid
            new TranslationSeedItem("entity.apsScheduleItem.apsscheduleid", "zh-HK", "APS排程ID", "APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.apsScheduleItem.apsschedulecode
            new TranslationSeedItem("entity.apsScheduleItem.apsschedulecode", "en-US", "APS排程编码", "APS排程编码（冗余字段，便于查询）"),
            // entity.apsScheduleItem.apsschedulecode
            new TranslationSeedItem("entity.apsScheduleItem.apsschedulecode", "ja-JP", "APS排程编码", "APS排程编码（冗余字段，便于查询）"),
            // entity.apsScheduleItem.apsschedulecode
            new TranslationSeedItem("entity.apsScheduleItem.apsschedulecode", "zh-CN", "APS排程编码", "APS排程编码（冗余字段，便于查询）"),
            // entity.apsScheduleItem.apsschedulecode
            new TranslationSeedItem("entity.apsScheduleItem.apsschedulecode", "zh-HK", "APS排程编码", "APS排程编码（冗余字段，便于查询）"),

            // entity.apsScheduleItem.linenumber
            new TranslationSeedItem("entity.apsScheduleItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.apsScheduleItem.linenumber
            new TranslationSeedItem("entity.apsScheduleItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.apsScheduleItem.linenumber
            new TranslationSeedItem("entity.apsScheduleItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.apsScheduleItem.linenumber
            new TranslationSeedItem("entity.apsScheduleItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.apsScheduleItem.workordercode
            new TranslationSeedItem("entity.apsScheduleItem.workordercode", "en-US", "生产工单编码", "生产工单编码"),
            // entity.apsScheduleItem.workordercode
            new TranslationSeedItem("entity.apsScheduleItem.workordercode", "ja-JP", "生产工单编码", "生产工单编码"),
            // entity.apsScheduleItem.workordercode
            new TranslationSeedItem("entity.apsScheduleItem.workordercode", "zh-CN", "生产工单编码", "生产工单编码"),
            // entity.apsScheduleItem.workordercode
            new TranslationSeedItem("entity.apsScheduleItem.workordercode", "zh-HK", "生产工单编码", "生产工单编码"),

            // entity.apsScheduleItem.productcode
            new TranslationSeedItem("entity.apsScheduleItem.productcode", "en-US", "产品编码", "产品编码"),
            // entity.apsScheduleItem.productcode
            new TranslationSeedItem("entity.apsScheduleItem.productcode", "ja-JP", "产品编码", "产品编码"),
            // entity.apsScheduleItem.productcode
            new TranslationSeedItem("entity.apsScheduleItem.productcode", "zh-CN", "产品编码", "产品编码"),
            // entity.apsScheduleItem.productcode
            new TranslationSeedItem("entity.apsScheduleItem.productcode", "zh-HK", "产品编码", "产品编码"),

            // entity.apsScheduleItem.productname
            new TranslationSeedItem("entity.apsScheduleItem.productname", "en-US", "产品名称", "产品名称"),
            // entity.apsScheduleItem.productname
            new TranslationSeedItem("entity.apsScheduleItem.productname", "ja-JP", "产品名称", "产品名称"),
            // entity.apsScheduleItem.productname
            new TranslationSeedItem("entity.apsScheduleItem.productname", "zh-CN", "产品名称", "产品名称"),
            // entity.apsScheduleItem.productname
            new TranslationSeedItem("entity.apsScheduleItem.productname", "zh-HK", "产品名称", "产品名称"),

            // entity.apsScheduleItem.workcentercode
            new TranslationSeedItem("entity.apsScheduleItem.workcentercode", "en-US", "工作中心编码", "工作中心编码"),
            // entity.apsScheduleItem.workcentercode
            new TranslationSeedItem("entity.apsScheduleItem.workcentercode", "ja-JP", "工作中心编码", "工作中心编码"),
            // entity.apsScheduleItem.workcentercode
            new TranslationSeedItem("entity.apsScheduleItem.workcentercode", "zh-CN", "工作中心编码", "工作中心编码"),
            // entity.apsScheduleItem.workcentercode
            new TranslationSeedItem("entity.apsScheduleItem.workcentercode", "zh-HK", "工作中心编码", "工作中心编码"),

            // entity.apsScheduleItem.workcentername
            new TranslationSeedItem("entity.apsScheduleItem.workcentername", "en-US", "工作中心名称", "工作中心名称"),
            // entity.apsScheduleItem.workcentername
            new TranslationSeedItem("entity.apsScheduleItem.workcentername", "ja-JP", "工作中心名称", "工作中心名称"),
            // entity.apsScheduleItem.workcentername
            new TranslationSeedItem("entity.apsScheduleItem.workcentername", "zh-CN", "工作中心名称", "工作中心名称"),
            // entity.apsScheduleItem.workcentername
            new TranslationSeedItem("entity.apsScheduleItem.workcentername", "zh-HK", "工作中心名称", "工作中心名称"),

            // entity.apsScheduleItem.processcode
            new TranslationSeedItem("entity.apsScheduleItem.processcode", "en-US", "工序编码", "工序编码"),
            // entity.apsScheduleItem.processcode
            new TranslationSeedItem("entity.apsScheduleItem.processcode", "ja-JP", "工序编码", "工序编码"),
            // entity.apsScheduleItem.processcode
            new TranslationSeedItem("entity.apsScheduleItem.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.apsScheduleItem.processcode
            new TranslationSeedItem("entity.apsScheduleItem.processcode", "zh-HK", "工序编码", "工序编码"),

            // entity.apsScheduleItem.processname
            new TranslationSeedItem("entity.apsScheduleItem.processname", "en-US", "工序名称", "工序名称"),
            // entity.apsScheduleItem.processname
            new TranslationSeedItem("entity.apsScheduleItem.processname", "ja-JP", "工序名称", "工序名称"),
            // entity.apsScheduleItem.processname
            new TranslationSeedItem("entity.apsScheduleItem.processname", "zh-CN", "工序名称", "工序名称"),
            // entity.apsScheduleItem.processname
            new TranslationSeedItem("entity.apsScheduleItem.processname", "zh-HK", "工序名称", "工序名称"),

            // entity.apsScheduleItem.processsequence
            new TranslationSeedItem("entity.apsScheduleItem.processsequence", "en-US", "工序序号", "工序序号"),
            // entity.apsScheduleItem.processsequence
            new TranslationSeedItem("entity.apsScheduleItem.processsequence", "ja-JP", "工序序号", "工序序号"),
            // entity.apsScheduleItem.processsequence
            new TranslationSeedItem("entity.apsScheduleItem.processsequence", "zh-CN", "工序序号", "工序序号"),
            // entity.apsScheduleItem.processsequence
            new TranslationSeedItem("entity.apsScheduleItem.processsequence", "zh-HK", "工序序号", "工序序号"),

            // entity.apsScheduleItem.processstandardst
            new TranslationSeedItem("entity.apsScheduleItem.processstandardst", "en-US", "工序标准ST值", "工序标准ST值"),
            // entity.apsScheduleItem.processstandardst
            new TranslationSeedItem("entity.apsScheduleItem.processstandardst", "ja-JP", "工序标准ST值", "工序标准ST值"),
            // entity.apsScheduleItem.processstandardst
            new TranslationSeedItem("entity.apsScheduleItem.processstandardst", "zh-CN", "工序标准ST值", "工序标准ST值"),
            // entity.apsScheduleItem.processstandardst
            new TranslationSeedItem("entity.apsScheduleItem.processstandardst", "zh-HK", "工序标准ST值", "工序标准ST值"),

            // entity.apsScheduleItem.processstandardstunit
            new TranslationSeedItem("entity.apsScheduleItem.processstandardstunit", "en-US", "工序标准ST单位", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsScheduleItem.processstandardstunit
            new TranslationSeedItem("entity.apsScheduleItem.processstandardstunit", "ja-JP", "工序标准ST单位", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsScheduleItem.processstandardstunit
            new TranslationSeedItem("entity.apsScheduleItem.processstandardstunit", "zh-CN", "工序标准ST单位", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),
            // entity.apsScheduleItem.processstandardstunit
            new TranslationSeedItem("entity.apsScheduleItem.processstandardstunit", "zh-HK", "工序标准ST单位", "工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）"),

            // entity.apsScheduleItem.extraminutes
            new TranslationSeedItem("entity.apsScheduleItem.extraminutes", "en-US", "额外时间", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsScheduleItem.extraminutes
            new TranslationSeedItem("entity.apsScheduleItem.extraminutes", "ja-JP", "额外时间", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsScheduleItem.extraminutes
            new TranslationSeedItem("entity.apsScheduleItem.extraminutes", "zh-CN", "额外时间", "额外时间（分钟），如换模、调试、清洁等准备时间"),
            // entity.apsScheduleItem.extraminutes
            new TranslationSeedItem("entity.apsScheduleItem.extraminutes", "zh-HK", "额外时间", "额外时间（分钟），如换模、调试、清洁等准备时间"),

            // entity.apsScheduleItem.planquantity
            new TranslationSeedItem("entity.apsScheduleItem.planquantity", "en-US", "计划数量", "计划数量"),
            // entity.apsScheduleItem.planquantity
            new TranslationSeedItem("entity.apsScheduleItem.planquantity", "ja-JP", "计划数量", "计划数量"),
            // entity.apsScheduleItem.planquantity
            new TranslationSeedItem("entity.apsScheduleItem.planquantity", "zh-CN", "计划数量", "计划数量"),
            // entity.apsScheduleItem.planquantity
            new TranslationSeedItem("entity.apsScheduleItem.planquantity", "zh-HK", "计划数量", "计划数量"),

            // entity.apsScheduleItem.planstarttime
            new TranslationSeedItem("entity.apsScheduleItem.planstarttime", "en-US", "计划开始时间", "计划开始时间"),
            // entity.apsScheduleItem.planstarttime
            new TranslationSeedItem("entity.apsScheduleItem.planstarttime", "ja-JP", "计划开始时间", "计划开始时间"),
            // entity.apsScheduleItem.planstarttime
            new TranslationSeedItem("entity.apsScheduleItem.planstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsScheduleItem.planstarttime
            new TranslationSeedItem("entity.apsScheduleItem.planstarttime", "zh-HK", "计划开始时间", "计划开始时间"),

            // entity.apsScheduleItem.planendtime
            new TranslationSeedItem("entity.apsScheduleItem.planendtime", "en-US", "计划结束时间", "计划结束时间"),
            // entity.apsScheduleItem.planendtime
            new TranslationSeedItem("entity.apsScheduleItem.planendtime", "ja-JP", "计划结束时间", "计划结束时间"),
            // entity.apsScheduleItem.planendtime
            new TranslationSeedItem("entity.apsScheduleItem.planendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsScheduleItem.planendtime
            new TranslationSeedItem("entity.apsScheduleItem.planendtime", "zh-HK", "计划结束时间", "计划结束时间"),

            // entity.apsScheduleItem.actualstarttime
            new TranslationSeedItem("entity.apsScheduleItem.actualstarttime", "en-US", "实际开始时间", "实际开始时间"),
            // entity.apsScheduleItem.actualstarttime
            new TranslationSeedItem("entity.apsScheduleItem.actualstarttime", "ja-JP", "实际开始时间", "实际开始时间"),
            // entity.apsScheduleItem.actualstarttime
            new TranslationSeedItem("entity.apsScheduleItem.actualstarttime", "zh-CN", "实际开始时间", "实际开始时间"),
            // entity.apsScheduleItem.actualstarttime
            new TranslationSeedItem("entity.apsScheduleItem.actualstarttime", "zh-HK", "实际开始时间", "实际开始时间"),

            // entity.apsScheduleItem.actualendtime
            new TranslationSeedItem("entity.apsScheduleItem.actualendtime", "en-US", "实际结束时间", "实际结束时间"),
            // entity.apsScheduleItem.actualendtime
            new TranslationSeedItem("entity.apsScheduleItem.actualendtime", "ja-JP", "实际结束时间", "实际结束时间"),
            // entity.apsScheduleItem.actualendtime
            new TranslationSeedItem("entity.apsScheduleItem.actualendtime", "zh-CN", "实际结束时间", "实际结束时间"),
            // entity.apsScheduleItem.actualendtime
            new TranslationSeedItem("entity.apsScheduleItem.actualendtime", "zh-HK", "实际结束时间", "实际结束时间"),

            // entity.apsScheduleItem.processstatus
            new TranslationSeedItem("entity.apsScheduleItem.processstatus", "en-US", "工序状态", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsScheduleItem.processstatus
            new TranslationSeedItem("entity.apsScheduleItem.processstatus", "ja-JP", "工序状态", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsScheduleItem.processstatus
            new TranslationSeedItem("entity.apsScheduleItem.processstatus", "zh-CN", "工序状态", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),
            // entity.apsScheduleItem.processstatus
            new TranslationSeedItem("entity.apsScheduleItem.processstatus", "zh-HK", "工序状态", "工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）"),

            // entity.apsScheduleItem.priority
            new TranslationSeedItem("entity.apsScheduleItem.priority", "en-US", "优先级", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsScheduleItem.priority
            new TranslationSeedItem("entity.apsScheduleItem.priority", "ja-JP", "优先级", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsScheduleItem.priority
            new TranslationSeedItem("entity.apsScheduleItem.priority", "zh-CN", "优先级", "优先级（0=普通，1=紧急，2=特急）"),
            // entity.apsScheduleItem.priority
            new TranslationSeedItem("entity.apsScheduleItem.priority", "zh-HK", "优先级", "优先级（0=普通，1=紧急，2=特急）"),
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
