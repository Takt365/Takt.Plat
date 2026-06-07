// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktEquipmentOperationRateI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEquipmentOperationRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEquipmentOperationRate 实体国际化翻译种子（键前缀 entity.equipmentOperationRate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEquipmentOperationRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEquipmentOperationRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 equipmentOperationRate 实体翻译...", tenantCode);

        foreach (var item in GetEquipmentOperationRateTranslations())
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

        TaktLogger.Information("TaktEquipmentOperationRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEquipmentOperationRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.equipmentOperationRate._self / entity.equipmentOperationRate.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEquipmentOperationRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.equipmentOperationRate._self
            new TranslationSeedItem("entity.equipmentOperationRate._self", "en-US", "Equipment Operation Rate Information", "实体名称"),
            // entity.equipmentOperationRate._self
            new TranslationSeedItem("entity.equipmentOperationRate._self", "ja-JP", "机器稼动率信息", "实体名称"),
            // entity.equipmentOperationRate._self
            new TranslationSeedItem("entity.equipmentOperationRate._self", "zh-CN", "机器稼动率信息", "实体名称"),
            // entity.equipmentOperationRate._self
            new TranslationSeedItem("entity.equipmentOperationRate._self", "zh-HK", "机器稼动率信息", "实体名称"),

            // entity.equipmentOperationRate.plantcode
            new TranslationSeedItem("entity.equipmentOperationRate.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.equipmentOperationRate.plantcode
            new TranslationSeedItem("entity.equipmentOperationRate.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.equipmentOperationRate.plantcode
            new TranslationSeedItem("entity.equipmentOperationRate.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.equipmentOperationRate.plantcode
            new TranslationSeedItem("entity.equipmentOperationRate.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.equipmentOperationRate.timecategory
            new TranslationSeedItem("entity.equipmentOperationRate.timecategory", "en-US", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentOperationRate.timecategory
            new TranslationSeedItem("entity.equipmentOperationRate.timecategory", "ja-JP", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentOperationRate.timecategory
            new TranslationSeedItem("entity.equipmentOperationRate.timecategory", "zh-CN", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.equipmentOperationRate.timecategory
            new TranslationSeedItem("entity.equipmentOperationRate.timecategory", "zh-HK", "时间类别", "时间类别（1=天，2=周，3=月）"),

            // entity.equipmentOperationRate.startdate
            new TranslationSeedItem("entity.equipmentOperationRate.startdate", "en-US", "开始日期", "开始日期"),
            // entity.equipmentOperationRate.startdate
            new TranslationSeedItem("entity.equipmentOperationRate.startdate", "ja-JP", "开始日期", "开始日期"),
            // entity.equipmentOperationRate.startdate
            new TranslationSeedItem("entity.equipmentOperationRate.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.equipmentOperationRate.startdate
            new TranslationSeedItem("entity.equipmentOperationRate.startdate", "zh-HK", "开始日期", "开始日期"),

            // entity.equipmentOperationRate.enddate
            new TranslationSeedItem("entity.equipmentOperationRate.enddate", "en-US", "结束日期", "结束日期"),
            // entity.equipmentOperationRate.enddate
            new TranslationSeedItem("entity.equipmentOperationRate.enddate", "ja-JP", "结束日期", "结束日期"),
            // entity.equipmentOperationRate.enddate
            new TranslationSeedItem("entity.equipmentOperationRate.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.equipmentOperationRate.enddate
            new TranslationSeedItem("entity.equipmentOperationRate.enddate", "zh-HK", "结束日期", "结束日期"),

            // entity.equipmentOperationRate.weeknumber
            new TranslationSeedItem("entity.equipmentOperationRate.weeknumber", "en-US", "周数", "周数（1-53）"),
            // entity.equipmentOperationRate.weeknumber
            new TranslationSeedItem("entity.equipmentOperationRate.weeknumber", "ja-JP", "周数", "周数（1-53）"),
            // entity.equipmentOperationRate.weeknumber
            new TranslationSeedItem("entity.equipmentOperationRate.weeknumber", "zh-CN", "周数", "周数（1-53）"),
            // entity.equipmentOperationRate.weeknumber
            new TranslationSeedItem("entity.equipmentOperationRate.weeknumber", "zh-HK", "周数", "周数（1-53）"),

            // entity.equipmentOperationRate.monthnumber
            new TranslationSeedItem("entity.equipmentOperationRate.monthnumber", "en-US", "月份", "月份（1-12）"),
            // entity.equipmentOperationRate.monthnumber
            new TranslationSeedItem("entity.equipmentOperationRate.monthnumber", "ja-JP", "月份", "月份（1-12）"),
            // entity.equipmentOperationRate.monthnumber
            new TranslationSeedItem("entity.equipmentOperationRate.monthnumber", "zh-CN", "月份", "月份（1-12）"),
            // entity.equipmentOperationRate.monthnumber
            new TranslationSeedItem("entity.equipmentOperationRate.monthnumber", "zh-HK", "月份", "月份（1-12）"),

            // entity.equipmentOperationRate.equipmentcode
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentcode", "en-US", "设备编码", "设备编码"),
            // entity.equipmentOperationRate.equipmentcode
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentcode", "ja-JP", "设备编码", "设备编码"),
            // entity.equipmentOperationRate.equipmentcode
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentcode", "zh-CN", "设备编码", "设备编码"),
            // entity.equipmentOperationRate.equipmentcode
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentcode", "zh-HK", "设备编码", "设备编码"),

            // entity.equipmentOperationRate.equipmentname
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentname", "en-US", "设备名称", "设备名称"),
            // entity.equipmentOperationRate.equipmentname
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentname", "ja-JP", "设备名称", "设备名称"),
            // entity.equipmentOperationRate.equipmentname
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentname", "zh-CN", "设备名称", "设备名称"),
            // entity.equipmentOperationRate.equipmentname
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentname", "zh-HK", "设备名称", "设备名称"),

            // entity.equipmentOperationRate.equipmenttype
            new TranslationSeedItem("entity.equipmentOperationRate.equipmenttype", "en-US", "设备类型", "设备类型（1=生产设备，2=检测设备，3=包装设备，4=其他）"),
            // entity.equipmentOperationRate.equipmenttype
            new TranslationSeedItem("entity.equipmentOperationRate.equipmenttype", "ja-JP", "设备类型", "设备类型（1=生产设备，2=检测设备，3=包装设备，4=其他）"),
            // entity.equipmentOperationRate.equipmenttype
            new TranslationSeedItem("entity.equipmentOperationRate.equipmenttype", "zh-CN", "设备类型", "设备类型（1=生产设备，2=检测设备，3=包装设备，4=其他）"),
            // entity.equipmentOperationRate.equipmenttype
            new TranslationSeedItem("entity.equipmentOperationRate.equipmenttype", "zh-HK", "设备类型", "设备类型（1=生产设备，2=检测设备，3=包装设备，4=其他）"),

            // entity.equipmentOperationRate.productionline
            new TranslationSeedItem("entity.equipmentOperationRate.productionline", "en-US", "生产线", "生产线"),
            // entity.equipmentOperationRate.productionline
            new TranslationSeedItem("entity.equipmentOperationRate.productionline", "ja-JP", "生产线", "生产线"),
            // entity.equipmentOperationRate.productionline
            new TranslationSeedItem("entity.equipmentOperationRate.productionline", "zh-CN", "生产线", "生产线"),
            // entity.equipmentOperationRate.productionline
            new TranslationSeedItem("entity.equipmentOperationRate.productionline", "zh-HK", "生产线", "生产线"),

            // entity.equipmentOperationRate.shiftno
            new TranslationSeedItem("entity.equipmentOperationRate.shiftno", "en-US", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.equipmentOperationRate.shiftno
            new TranslationSeedItem("entity.equipmentOperationRate.shiftno", "ja-JP", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.equipmentOperationRate.shiftno
            new TranslationSeedItem("entity.equipmentOperationRate.shiftno", "zh-CN", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.equipmentOperationRate.shiftno
            new TranslationSeedItem("entity.equipmentOperationRate.shiftno", "zh-HK", "班次", "班次（1=早班，2=中班，3=晚班）"),

            // entity.equipmentOperationRate.plannedruntime
            new TranslationSeedItem("entity.equipmentOperationRate.plannedruntime", "en-US", "负荷时间(分钟)", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentOperationRate.plannedruntime
            new TranslationSeedItem("entity.equipmentOperationRate.plannedruntime", "ja-JP", "负荷时间(分钟)", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentOperationRate.plannedruntime
            new TranslationSeedItem("entity.equipmentOperationRate.plannedruntime", "zh-CN", "负荷时间(分钟)", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),
            // entity.equipmentOperationRate.plannedruntime
            new TranslationSeedItem("entity.equipmentOperationRate.plannedruntime", "zh-HK", "负荷时间(分钟)", "负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。"),

            // entity.equipmentOperationRate.actualruntime
            new TranslationSeedItem("entity.equipmentOperationRate.actualruntime", "en-US", "稼动时间(分钟)", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentOperationRate.actualruntime
            new TranslationSeedItem("entity.equipmentOperationRate.actualruntime", "ja-JP", "稼动时间(分钟)", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentOperationRate.actualruntime
            new TranslationSeedItem("entity.equipmentOperationRate.actualruntime", "zh-CN", "稼动时间(分钟)", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),
            // entity.equipmentOperationRate.actualruntime
            new TranslationSeedItem("entity.equipmentOperationRate.actualruntime", "zh-HK", "稼动时间(分钟)", "稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。"),

            // entity.equipmentOperationRate.downtime
            new TranslationSeedItem("entity.equipmentOperationRate.downtime", "en-US", "停线损失时间(分钟)", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentOperationRate.downtime
            new TranslationSeedItem("entity.equipmentOperationRate.downtime", "ja-JP", "停线损失时间(分钟)", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentOperationRate.downtime
            new TranslationSeedItem("entity.equipmentOperationRate.downtime", "zh-CN", "停线损失时间(分钟)", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),
            // entity.equipmentOperationRate.downtime
            new TranslationSeedItem("entity.equipmentOperationRate.downtime", "zh-HK", "停线损失时间(分钟)", "停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。"),

            // entity.equipmentOperationRate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperationrate", "en-US", "时间稼动率(%)", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentOperationRate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperationrate", "ja-JP", "时间稼动率(%)", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentOperationRate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperationrate", "zh-CN", "时间稼动率(%)", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),
            // entity.equipmentOperationRate.equipmentoperationrate
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperationrate", "zh-HK", "时间稼动率(%)", "时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。"),

            // entity.equipmentOperationRate.plannedoutput
            new TranslationSeedItem("entity.equipmentOperationRate.plannedoutput", "en-US", "计划产量", "计划产量"),
            // entity.equipmentOperationRate.plannedoutput
            new TranslationSeedItem("entity.equipmentOperationRate.plannedoutput", "ja-JP", "计划产量", "计划产量"),
            // entity.equipmentOperationRate.plannedoutput
            new TranslationSeedItem("entity.equipmentOperationRate.plannedoutput", "zh-CN", "计划产量", "计划产量"),
            // entity.equipmentOperationRate.plannedoutput
            new TranslationSeedItem("entity.equipmentOperationRate.plannedoutput", "zh-HK", "计划产量", "计划产量"),

            // entity.equipmentOperationRate.actualoutput
            new TranslationSeedItem("entity.equipmentOperationRate.actualoutput", "en-US", "实际产量", "实际产量"),
            // entity.equipmentOperationRate.actualoutput
            new TranslationSeedItem("entity.equipmentOperationRate.actualoutput", "ja-JP", "实际产量", "实际产量"),
            // entity.equipmentOperationRate.actualoutput
            new TranslationSeedItem("entity.equipmentOperationRate.actualoutput", "zh-CN", "实际产量", "实际产量"),
            // entity.equipmentOperationRate.actualoutput
            new TranslationSeedItem("entity.equipmentOperationRate.actualoutput", "zh-HK", "实际产量", "实际产量"),

            // entity.equipmentOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentOperationRate.qualifiedquantity", "en-US", "合格品数量", "合格品数量"),
            // entity.equipmentOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentOperationRate.qualifiedquantity", "ja-JP", "合格品数量", "合格品数量"),
            // entity.equipmentOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentOperationRate.qualifiedquantity", "zh-CN", "合格品数量", "合格品数量"),
            // entity.equipmentOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.equipmentOperationRate.qualifiedquantity", "zh-HK", "合格品数量", "合格品数量"),

            // entity.equipmentOperationRate.defectivequantity
            new TranslationSeedItem("entity.equipmentOperationRate.defectivequantity", "en-US", "不良品数量", "不良品数量"),
            // entity.equipmentOperationRate.defectivequantity
            new TranslationSeedItem("entity.equipmentOperationRate.defectivequantity", "ja-JP", "不良品数量", "不良品数量"),
            // entity.equipmentOperationRate.defectivequantity
            new TranslationSeedItem("entity.equipmentOperationRate.defectivequantity", "zh-CN", "不良品数量", "不良品数量"),
            // entity.equipmentOperationRate.defectivequantity
            new TranslationSeedItem("entity.equipmentOperationRate.defectivequantity", "zh-HK", "不良品数量", "不良品数量"),

            // entity.equipmentOperationRate.yieldrate
            new TranslationSeedItem("entity.equipmentOperationRate.yieldrate", "en-US", "良品率(%)", "良品率（%）"),
            // entity.equipmentOperationRate.yieldrate
            new TranslationSeedItem("entity.equipmentOperationRate.yieldrate", "ja-JP", "良品率(%)", "良品率（%）"),
            // entity.equipmentOperationRate.yieldrate
            new TranslationSeedItem("entity.equipmentOperationRate.yieldrate", "zh-CN", "良品率(%)", "良品率（%）"),
            // entity.equipmentOperationRate.yieldrate
            new TranslationSeedItem("entity.equipmentOperationRate.yieldrate", "zh-HK", "良品率(%)", "良品率（%）"),

            // entity.equipmentOperationRate.downtimereasontype
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereasontype", "en-US", "停机原因类型", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentOperationRate.downtimereasontype
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereasontype", "ja-JP", "停机原因类型", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentOperationRate.downtimereasontype
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereasontype", "zh-CN", "停机原因类型", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),
            // entity.equipmentOperationRate.downtimereasontype
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereasontype", "zh-HK", "停机原因类型", "停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）"),

            // entity.equipmentOperationRate.downtimereason
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereason", "en-US", "停机原因描述", "停机原因描述"),
            // entity.equipmentOperationRate.downtimereason
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereason", "ja-JP", "停机原因描述", "停机原因描述"),
            // entity.equipmentOperationRate.downtimereason
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereason", "zh-CN", "停机原因描述", "停机原因描述"),
            // entity.equipmentOperationRate.downtimereason
            new TranslationSeedItem("entity.equipmentOperationRate.downtimereason", "zh-HK", "停机原因描述", "停机原因描述"),

            // entity.equipmentOperationRate.equipmentstatus
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentstatus", "en-US", "设备状态", "设备状态（1=正常运行，2=故障停机，3=维护保养，4=换型调试，5=其他）"),
            // entity.equipmentOperationRate.equipmentstatus
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentstatus", "ja-JP", "设备状态", "设备状态（1=正常运行，2=故障停机，3=维护保养，4=换型调试，5=其他）"),
            // entity.equipmentOperationRate.equipmentstatus
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentstatus", "zh-CN", "设备状态", "设备状态（1=正常运行，2=故障停机，3=维护保养，4=换型调试，5=其他）"),
            // entity.equipmentOperationRate.equipmentstatus
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentstatus", "zh-HK", "设备状态", "设备状态（1=正常运行，2=故障停机，3=维护保养，4=换型调试，5=其他）"),

            // entity.equipmentOperationRate.equipmentoperator
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperator", "en-US", "设备操作员", "设备操作员"),
            // entity.equipmentOperationRate.equipmentoperator
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperator", "ja-JP", "设备操作员", "设备操作员"),
            // entity.equipmentOperationRate.equipmentoperator
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperator", "zh-CN", "设备操作员", "设备操作员"),
            // entity.equipmentOperationRate.equipmentoperator
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentoperator", "zh-HK", "设备操作员", "设备操作员"),

            // entity.equipmentOperationRate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentmaintainer", "en-US", "设备维护员", "设备维护员"),
            // entity.equipmentOperationRate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentmaintainer", "ja-JP", "设备维护员", "设备维护员"),
            // entity.equipmentOperationRate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentmaintainer", "zh-CN", "设备维护员", "设备维护员"),
            // entity.equipmentOperationRate.equipmentmaintainer
            new TranslationSeedItem("entity.equipmentOperationRate.equipmentmaintainer", "zh-HK", "设备维护员", "设备维护员"),

            // entity.equipmentOperationRate.teamleader
            new TranslationSeedItem("entity.equipmentOperationRate.teamleader", "en-US", "班组长", "班组长"),
            // entity.equipmentOperationRate.teamleader
            new TranslationSeedItem("entity.equipmentOperationRate.teamleader", "ja-JP", "班组长", "班组长"),
            // entity.equipmentOperationRate.teamleader
            new TranslationSeedItem("entity.equipmentOperationRate.teamleader", "zh-CN", "班组长", "班组长"),
            // entity.equipmentOperationRate.teamleader
            new TranslationSeedItem("entity.equipmentOperationRate.teamleader", "zh-HK", "班组长", "班组长"),

            // entity.equipmentOperationRate.status
            new TranslationSeedItem("entity.equipmentOperationRate.status", "en-US", "状态", "状态（0=正常，1=停用）"),
            // entity.equipmentOperationRate.status
            new TranslationSeedItem("entity.equipmentOperationRate.status", "ja-JP", "状态", "状态（0=正常，1=停用）"),
            // entity.equipmentOperationRate.status
            new TranslationSeedItem("entity.equipmentOperationRate.status", "zh-CN", "状态", "状态（0=正常，1=停用）"),
            // entity.equipmentOperationRate.status
            new TranslationSeedItem("entity.equipmentOperationRate.status", "zh-HK", "状态", "状态（0=正常，1=停用）"),
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
