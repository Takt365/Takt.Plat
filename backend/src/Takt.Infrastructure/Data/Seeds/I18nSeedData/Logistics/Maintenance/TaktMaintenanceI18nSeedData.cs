// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenance 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance;

/// <summary>
/// TaktMaintenance 实体国际化翻译种子（键前缀 entity.maintenance.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenance 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenance 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceTranslations())
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

        TaktLogger.Information("TaktMaintenance 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenance 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenance._self / entity.maintenance.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenance._self
            new TranslationSeedItem("entity.maintenance._self", "en-US", "Maintenance Information", "实体名称"),
            // entity.maintenance._self
            new TranslationSeedItem("entity.maintenance._self", "ja-JP", "Takt设备维护记录信息", "实体名称"),
            // entity.maintenance._self
            new TranslationSeedItem("entity.maintenance._self", "zh-CN", "Takt设备维护记录信息", "实体名称"),
            // entity.maintenance._self
            new TranslationSeedItem("entity.maintenance._self", "zh-HK", "Takt设备维护记录信息", "实体名称"),

            // entity.maintenance.equipmentid
            new TranslationSeedItem("entity.maintenance.equipmentid", "en-US", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenance.equipmentid
            new TranslationSeedItem("entity.maintenance.equipmentid", "ja-JP", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenance.equipmentid
            new TranslationSeedItem("entity.maintenance.equipmentid", "zh-CN", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),
            // entity.maintenance.equipmentid
            new TranslationSeedItem("entity.maintenance.equipmentid", "zh-HK", "设备ID", "设备ID（序列化为string以避免Javascript精度问题）"),

            // entity.maintenance.equipmentcode
            new TranslationSeedItem("entity.maintenance.equipmentcode", "en-US", "设备编码", "设备编码（冗余字段,便于查询）"),
            // entity.maintenance.equipmentcode
            new TranslationSeedItem("entity.maintenance.equipmentcode", "ja-JP", "设备编码", "设备编码（冗余字段,便于查询）"),
            // entity.maintenance.equipmentcode
            new TranslationSeedItem("entity.maintenance.equipmentcode", "zh-CN", "设备编码", "设备编码（冗余字段,便于查询）"),
            // entity.maintenance.equipmentcode
            new TranslationSeedItem("entity.maintenance.equipmentcode", "zh-HK", "设备编码", "设备编码（冗余字段,便于查询）"),

            // entity.maintenance.linenumber
            new TranslationSeedItem("entity.maintenance.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.maintenance.linenumber
            new TranslationSeedItem("entity.maintenance.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.maintenance.linenumber
            new TranslationSeedItem("entity.maintenance.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.maintenance.linenumber
            new TranslationSeedItem("entity.maintenance.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.maintenance.type
            new TranslationSeedItem("entity.maintenance.type", "en-US", "维护类型", "维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenance.type
            new TranslationSeedItem("entity.maintenance.type", "ja-JP", "维护类型", "维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenance.type
            new TranslationSeedItem("entity.maintenance.type", "zh-CN", "维护类型", "维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),
            // entity.maintenance.type
            new TranslationSeedItem("entity.maintenance.type", "zh-HK", "维护类型", "维护类型（0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）"),

            // entity.maintenance.company
            new TranslationSeedItem("entity.maintenance.company", "en-US", "维护单位", "维护单位"),
            // entity.maintenance.company
            new TranslationSeedItem("entity.maintenance.company", "ja-JP", "维护单位", "维护单位"),
            // entity.maintenance.company
            new TranslationSeedItem("entity.maintenance.company", "zh-CN", "维护单位", "维护单位"),
            // entity.maintenance.company
            new TranslationSeedItem("entity.maintenance.company", "zh-HK", "维护单位", "维护单位"),

            // entity.maintenance.technician
            new TranslationSeedItem("entity.maintenance.technician", "en-US", "维护技师", "维护技师"),
            // entity.maintenance.technician
            new TranslationSeedItem("entity.maintenance.technician", "ja-JP", "维护技师", "维护技师"),
            // entity.maintenance.technician
            new TranslationSeedItem("entity.maintenance.technician", "zh-CN", "维护技师", "维护技师"),
            // entity.maintenance.technician
            new TranslationSeedItem("entity.maintenance.technician", "zh-HK", "维护技师", "维护技师"),

            // entity.maintenance.date
            new TranslationSeedItem("entity.maintenance.date", "en-US", "维护日期", "维护日期"),
            // entity.maintenance.date
            new TranslationSeedItem("entity.maintenance.date", "ja-JP", "维护日期", "维护日期"),
            // entity.maintenance.date
            new TranslationSeedItem("entity.maintenance.date", "zh-CN", "维护日期", "维护日期"),
            // entity.maintenance.date
            new TranslationSeedItem("entity.maintenance.date", "zh-HK", "维护日期", "维护日期"),

            // entity.maintenance.starttime
            new TranslationSeedItem("entity.maintenance.starttime", "en-US", "维护开始时间", "维护开始时间"),
            // entity.maintenance.starttime
            new TranslationSeedItem("entity.maintenance.starttime", "ja-JP", "维护开始时间", "维护开始时间"),
            // entity.maintenance.starttime
            new TranslationSeedItem("entity.maintenance.starttime", "zh-CN", "维护开始时间", "维护开始时间"),
            // entity.maintenance.starttime
            new TranslationSeedItem("entity.maintenance.starttime", "zh-HK", "维护开始时间", "维护开始时间"),

            // entity.maintenance.endtime
            new TranslationSeedItem("entity.maintenance.endtime", "en-US", "维护结束时间", "维护结束时间"),
            // entity.maintenance.endtime
            new TranslationSeedItem("entity.maintenance.endtime", "ja-JP", "维护结束时间", "维护结束时间"),
            // entity.maintenance.endtime
            new TranslationSeedItem("entity.maintenance.endtime", "zh-CN", "维护结束时间", "维护结束时间"),
            // entity.maintenance.endtime
            new TranslationSeedItem("entity.maintenance.endtime", "zh-HK", "维护结束时间", "维护结束时间"),

            // entity.maintenance.content
            new TranslationSeedItem("entity.maintenance.content", "en-US", "维护内容", "维护内容描述"),
            // entity.maintenance.content
            new TranslationSeedItem("entity.maintenance.content", "ja-JP", "维护内容", "维护内容描述"),
            // entity.maintenance.content
            new TranslationSeedItem("entity.maintenance.content", "zh-CN", "维护内容", "维护内容描述"),
            // entity.maintenance.content
            new TranslationSeedItem("entity.maintenance.content", "zh-HK", "维护内容", "维护内容描述"),

            // entity.maintenance.faultdescription
            new TranslationSeedItem("entity.maintenance.faultdescription", "en-US", "故障描述", "故障描述"),
            // entity.maintenance.faultdescription
            new TranslationSeedItem("entity.maintenance.faultdescription", "ja-JP", "故障描述", "故障描述"),
            // entity.maintenance.faultdescription
            new TranslationSeedItem("entity.maintenance.faultdescription", "zh-CN", "故障描述", "故障描述"),
            // entity.maintenance.faultdescription
            new TranslationSeedItem("entity.maintenance.faultdescription", "zh-HK", "故障描述", "故障描述"),

            // entity.maintenance.solution
            new TranslationSeedItem("entity.maintenance.solution", "en-US", "处理方案", "处理方案"),
            // entity.maintenance.solution
            new TranslationSeedItem("entity.maintenance.solution", "ja-JP", "处理方案", "处理方案"),
            // entity.maintenance.solution
            new TranslationSeedItem("entity.maintenance.solution", "zh-CN", "处理方案", "处理方案"),
            // entity.maintenance.solution
            new TranslationSeedItem("entity.maintenance.solution", "zh-HK", "处理方案", "处理方案"),

            // entity.maintenance.usedparts
            new TranslationSeedItem("entity.maintenance.usedparts", "en-US", "使用配件", "使用配件（JSON格式，存储使用的配件列表）"),
            // entity.maintenance.usedparts
            new TranslationSeedItem("entity.maintenance.usedparts", "ja-JP", "使用配件", "使用配件（JSON格式，存储使用的配件列表）"),
            // entity.maintenance.usedparts
            new TranslationSeedItem("entity.maintenance.usedparts", "zh-CN", "使用配件", "使用配件（JSON格式，存储使用的配件列表）"),
            // entity.maintenance.usedparts
            new TranslationSeedItem("entity.maintenance.usedparts", "zh-HK", "使用配件", "使用配件（JSON格式，存储使用的配件列表）"),

            // entity.maintenance.cost
            new TranslationSeedItem("entity.maintenance.cost", "en-US", "维护费用", "维护费用（精确到分，存储为整数，单位为分）"),
            // entity.maintenance.cost
            new TranslationSeedItem("entity.maintenance.cost", "ja-JP", "维护费用", "维护费用（精确到分，存储为整数，单位为分）"),
            // entity.maintenance.cost
            new TranslationSeedItem("entity.maintenance.cost", "zh-CN", "维护费用", "维护费用（精确到分，存储为整数，单位为分）"),
            // entity.maintenance.cost
            new TranslationSeedItem("entity.maintenance.cost", "zh-HK", "维护费用", "维护费用（精确到分，存储为整数，单位为分）"),

            // entity.maintenance.result
            new TranslationSeedItem("entity.maintenance.result", "en-US", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenance.result
            new TranslationSeedItem("entity.maintenance.result", "ja-JP", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenance.result
            new TranslationSeedItem("entity.maintenance.result", "zh-CN", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),
            // entity.maintenance.result
            new TranslationSeedItem("entity.maintenance.result", "zh-HK", "维护结果", "维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）"),

            // entity.maintenance.status
            new TranslationSeedItem("entity.maintenance.status", "en-US", "维护状态", "维护状态（0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.maintenance.status
            new TranslationSeedItem("entity.maintenance.status", "ja-JP", "维护状态", "维护状态（0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.maintenance.status
            new TranslationSeedItem("entity.maintenance.status", "zh-CN", "维护状态", "维护状态（0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.maintenance.status
            new TranslationSeedItem("entity.maintenance.status", "zh-HK", "维护状态", "维护状态（0=待执行，1=执行中，2=已完成，3=已取消）"),

            // entity.maintenance.nextmaintenancedate
            new TranslationSeedItem("entity.maintenance.nextmaintenancedate", "en-US", "下次维护日期", "下次维护日期"),
            // entity.maintenance.nextmaintenancedate
            new TranslationSeedItem("entity.maintenance.nextmaintenancedate", "ja-JP", "下次维护日期", "下次维护日期"),
            // entity.maintenance.nextmaintenancedate
            new TranslationSeedItem("entity.maintenance.nextmaintenancedate", "zh-CN", "下次维护日期", "下次维护日期"),
            // entity.maintenance.nextmaintenancedate
            new TranslationSeedItem("entity.maintenance.nextmaintenancedate", "zh-HK", "下次维护日期", "下次维护日期"),

            // entity.maintenance.cycledays
            new TranslationSeedItem("entity.maintenance.cycledays", "en-US", "维护周期（天）", "维护周期（天）"),
            // entity.maintenance.cycledays
            new TranslationSeedItem("entity.maintenance.cycledays", "ja-JP", "维护周期（天）", "维护周期（天）"),
            // entity.maintenance.cycledays
            new TranslationSeedItem("entity.maintenance.cycledays", "zh-CN", "维护周期（天）", "维护周期（天）"),
            // entity.maintenance.cycledays
            new TranslationSeedItem("entity.maintenance.cycledays", "zh-HK", "维护周期（天）", "维护周期（天）"),

            // entity.maintenance.documents
            new TranslationSeedItem("entity.maintenance.documents", "en-US", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenance.documents
            new TranslationSeedItem("entity.maintenance.documents", "ja-JP", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenance.documents
            new TranslationSeedItem("entity.maintenance.documents", "zh-CN", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),
            // entity.maintenance.documents
            new TranslationSeedItem("entity.maintenance.documents", "zh-HK", "维护文档", "维护文档（JSON格式，存储维护文档ID列表）"),

            // entity.maintenance.images
            new TranslationSeedItem("entity.maintenance.images", "en-US", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenance.images
            new TranslationSeedItem("entity.maintenance.images", "ja-JP", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenance.images
            new TranslationSeedItem("entity.maintenance.images", "zh-CN", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),
            // entity.maintenance.images
            new TranslationSeedItem("entity.maintenance.images", "zh-HK", "维护图片", "维护图片（JSON格式，存储维护图片URL列表）"),

            // entity.maintenance.acceptedsummary
            new TranslationSeedItem("entity.maintenance.acceptedsummary", "en-US", "验收总结", "验收总结"),
            // entity.maintenance.acceptedsummary
            new TranslationSeedItem("entity.maintenance.acceptedsummary", "ja-JP", "验收总结", "验收总结"),
            // entity.maintenance.acceptedsummary
            new TranslationSeedItem("entity.maintenance.acceptedsummary", "zh-CN", "验收总结", "验收总结"),
            // entity.maintenance.acceptedsummary
            new TranslationSeedItem("entity.maintenance.acceptedsummary", "zh-HK", "验收总结", "验收总结"),

            // entity.maintenance.acceptedby
            new TranslationSeedItem("entity.maintenance.acceptedby", "en-US", "验收人", "验收人"),
            // entity.maintenance.acceptedby
            new TranslationSeedItem("entity.maintenance.acceptedby", "ja-JP", "验收人", "验收人"),
            // entity.maintenance.acceptedby
            new TranslationSeedItem("entity.maintenance.acceptedby", "zh-CN", "验收人", "验收人"),
            // entity.maintenance.acceptedby
            new TranslationSeedItem("entity.maintenance.acceptedby", "zh-HK", "验收人", "验收人"),

            // entity.maintenance.acceptedat
            new TranslationSeedItem("entity.maintenance.acceptedat", "en-US", "验收时间", "验收时间"),
            // entity.maintenance.acceptedat
            new TranslationSeedItem("entity.maintenance.acceptedat", "ja-JP", "验收时间", "验收时间"),
            // entity.maintenance.acceptedat
            new TranslationSeedItem("entity.maintenance.acceptedat", "zh-CN", "验收时间", "验收时间"),
            // entity.maintenance.acceptedat
            new TranslationSeedItem("entity.maintenance.acceptedat", "zh-HK", "验收时间", "验收时间"),
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
