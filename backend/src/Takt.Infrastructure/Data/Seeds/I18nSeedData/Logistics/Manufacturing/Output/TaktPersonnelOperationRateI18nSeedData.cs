// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPersonnelOperationRateI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPersonnelOperationRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPersonnelOperationRate 实体国际化翻译种子（键前缀 entity.personnelOperationRate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPersonnelOperationRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPersonnelOperationRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 personnelOperationRate 实体翻译...", tenantCode);

        foreach (var item in GetPersonnelOperationRateTranslations())
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

        TaktLogger.Information("TaktPersonnelOperationRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPersonnelOperationRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.personnelOperationRate._self / entity.personnelOperationRate.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPersonnelOperationRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.personnelOperationRate._self
            new TranslationSeedItem("entity.personnelOperationRate._self", "en-US", "Personnel Operation Rate Information", "实体名称"),
            // entity.personnelOperationRate._self
            new TranslationSeedItem("entity.personnelOperationRate._self", "ja-JP", "人员稼动率信息", "实体名称"),
            // entity.personnelOperationRate._self
            new TranslationSeedItem("entity.personnelOperationRate._self", "zh-CN", "人员稼动率信息", "实体名称"),
            // entity.personnelOperationRate._self
            new TranslationSeedItem("entity.personnelOperationRate._self", "zh-HK", "人员稼动率信息", "实体名称"),

            // entity.personnelOperationRate.plantcode
            new TranslationSeedItem("entity.personnelOperationRate.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.personnelOperationRate.plantcode
            new TranslationSeedItem("entity.personnelOperationRate.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.personnelOperationRate.plantcode
            new TranslationSeedItem("entity.personnelOperationRate.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.personnelOperationRate.plantcode
            new TranslationSeedItem("entity.personnelOperationRate.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.personnelOperationRate.timecategory
            new TranslationSeedItem("entity.personnelOperationRate.timecategory", "en-US", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.personnelOperationRate.timecategory
            new TranslationSeedItem("entity.personnelOperationRate.timecategory", "ja-JP", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.personnelOperationRate.timecategory
            new TranslationSeedItem("entity.personnelOperationRate.timecategory", "zh-CN", "时间类别", "时间类别（1=天，2=周，3=月）"),
            // entity.personnelOperationRate.timecategory
            new TranslationSeedItem("entity.personnelOperationRate.timecategory", "zh-HK", "时间类别", "时间类别（1=天，2=周，3=月）"),

            // entity.personnelOperationRate.startdate
            new TranslationSeedItem("entity.personnelOperationRate.startdate", "en-US", "开始日期", "开始日期"),
            // entity.personnelOperationRate.startdate
            new TranslationSeedItem("entity.personnelOperationRate.startdate", "ja-JP", "开始日期", "开始日期"),
            // entity.personnelOperationRate.startdate
            new TranslationSeedItem("entity.personnelOperationRate.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.personnelOperationRate.startdate
            new TranslationSeedItem("entity.personnelOperationRate.startdate", "zh-HK", "开始日期", "开始日期"),

            // entity.personnelOperationRate.enddate
            new TranslationSeedItem("entity.personnelOperationRate.enddate", "en-US", "结束日期", "结束日期"),
            // entity.personnelOperationRate.enddate
            new TranslationSeedItem("entity.personnelOperationRate.enddate", "ja-JP", "结束日期", "结束日期"),
            // entity.personnelOperationRate.enddate
            new TranslationSeedItem("entity.personnelOperationRate.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.personnelOperationRate.enddate
            new TranslationSeedItem("entity.personnelOperationRate.enddate", "zh-HK", "结束日期", "结束日期"),

            // entity.personnelOperationRate.weeknumber
            new TranslationSeedItem("entity.personnelOperationRate.weeknumber", "en-US", "周数", "周数（1-53）"),
            // entity.personnelOperationRate.weeknumber
            new TranslationSeedItem("entity.personnelOperationRate.weeknumber", "ja-JP", "周数", "周数（1-53）"),
            // entity.personnelOperationRate.weeknumber
            new TranslationSeedItem("entity.personnelOperationRate.weeknumber", "zh-CN", "周数", "周数（1-53）"),
            // entity.personnelOperationRate.weeknumber
            new TranslationSeedItem("entity.personnelOperationRate.weeknumber", "zh-HK", "周数", "周数（1-53）"),

            // entity.personnelOperationRate.monthnumber
            new TranslationSeedItem("entity.personnelOperationRate.monthnumber", "en-US", "月份", "月份（1-12）"),
            // entity.personnelOperationRate.monthnumber
            new TranslationSeedItem("entity.personnelOperationRate.monthnumber", "ja-JP", "月份", "月份（1-12）"),
            // entity.personnelOperationRate.monthnumber
            new TranslationSeedItem("entity.personnelOperationRate.monthnumber", "zh-CN", "月份", "月份（1-12）"),
            // entity.personnelOperationRate.monthnumber
            new TranslationSeedItem("entity.personnelOperationRate.monthnumber", "zh-HK", "月份", "月份（1-12）"),

            // entity.personnelOperationRate.productionline
            new TranslationSeedItem("entity.personnelOperationRate.productionline", "en-US", "生产线", "生产线"),
            // entity.personnelOperationRate.productionline
            new TranslationSeedItem("entity.personnelOperationRate.productionline", "ja-JP", "生产线", "生产线"),
            // entity.personnelOperationRate.productionline
            new TranslationSeedItem("entity.personnelOperationRate.productionline", "zh-CN", "生产线", "生产线"),
            // entity.personnelOperationRate.productionline
            new TranslationSeedItem("entity.personnelOperationRate.productionline", "zh-HK", "生产线", "生产线"),

            // entity.personnelOperationRate.productionlinename
            new TranslationSeedItem("entity.personnelOperationRate.productionlinename", "en-US", "生产线名称", "生产线名称"),
            // entity.personnelOperationRate.productionlinename
            new TranslationSeedItem("entity.personnelOperationRate.productionlinename", "ja-JP", "生产线名称", "生产线名称"),
            // entity.personnelOperationRate.productionlinename
            new TranslationSeedItem("entity.personnelOperationRate.productionlinename", "zh-CN", "生产线名称", "生产线名称"),
            // entity.personnelOperationRate.productionlinename
            new TranslationSeedItem("entity.personnelOperationRate.productionlinename", "zh-HK", "生产线名称", "生产线名称"),

            // entity.personnelOperationRate.shiftno
            new TranslationSeedItem("entity.personnelOperationRate.shiftno", "en-US", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.personnelOperationRate.shiftno
            new TranslationSeedItem("entity.personnelOperationRate.shiftno", "ja-JP", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.personnelOperationRate.shiftno
            new TranslationSeedItem("entity.personnelOperationRate.shiftno", "zh-CN", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.personnelOperationRate.shiftno
            new TranslationSeedItem("entity.personnelOperationRate.shiftno", "zh-HK", "班次", "班次（1=早班，2=中班，3=晚班）"),

            // entity.personnelOperationRate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.planneddirectpersonnelcount", "en-US", "计划直接人员数量", "计划直接人员数量"),
            // entity.personnelOperationRate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.planneddirectpersonnelcount", "ja-JP", "计划直接人员数量", "计划直接人员数量"),
            // entity.personnelOperationRate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.planneddirectpersonnelcount", "zh-CN", "计划直接人员数量", "计划直接人员数量"),
            // entity.personnelOperationRate.planneddirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.planneddirectpersonnelcount", "zh-HK", "计划直接人员数量", "计划直接人员数量"),

            // entity.personnelOperationRate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualdirectpersonnelcount", "en-US", "实际直接人员数量", "实际直接人员数量"),
            // entity.personnelOperationRate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualdirectpersonnelcount", "ja-JP", "实际直接人员数量", "实际直接人员数量"),
            // entity.personnelOperationRate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualdirectpersonnelcount", "zh-CN", "实际直接人员数量", "实际直接人员数量"),
            // entity.personnelOperationRate.actualdirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualdirectpersonnelcount", "zh-HK", "实际直接人员数量", "实际直接人员数量"),

            // entity.personnelOperationRate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.plannedindirectpersonnelcount", "en-US", "计划间接人员数量", "计划间接人员数量"),
            // entity.personnelOperationRate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.plannedindirectpersonnelcount", "ja-JP", "计划间接人员数量", "计划间接人员数量"),
            // entity.personnelOperationRate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.plannedindirectpersonnelcount", "zh-CN", "计划间接人员数量", "计划间接人员数量"),
            // entity.personnelOperationRate.plannedindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.plannedindirectpersonnelcount", "zh-HK", "计划间接人员数量", "计划间接人员数量"),

            // entity.personnelOperationRate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualindirectpersonnelcount", "en-US", "实际间接人员数量", "实际间接人员数量"),
            // entity.personnelOperationRate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualindirectpersonnelcount", "ja-JP", "实际间接人员数量", "实际间接人员数量"),
            // entity.personnelOperationRate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualindirectpersonnelcount", "zh-CN", "实际间接人员数量", "实际间接人员数量"),
            // entity.personnelOperationRate.actualindirectpersonnelcount
            new TranslationSeedItem("entity.personnelOperationRate.actualindirectpersonnelcount", "zh-HK", "实际间接人员数量", "实际间接人员数量"),

            // entity.personnelOperationRate.plannedworktime
            new TranslationSeedItem("entity.personnelOperationRate.plannedworktime", "en-US", "出勤时间(分钟)", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personnelOperationRate.plannedworktime
            new TranslationSeedItem("entity.personnelOperationRate.plannedworktime", "ja-JP", "出勤时间(分钟)", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personnelOperationRate.plannedworktime
            new TranslationSeedItem("entity.personnelOperationRate.plannedworktime", "zh-CN", "出勤时间(分钟)", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),
            // entity.personnelOperationRate.plannedworktime
            new TranslationSeedItem("entity.personnelOperationRate.plannedworktime", "zh-HK", "出勤时间(分钟)", "出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。"),

            // entity.personnelOperationRate.actualworktime
            new TranslationSeedItem("entity.personnelOperationRate.actualworktime", "en-US", "在岗作业时间(分钟)", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personnelOperationRate.actualworktime
            new TranslationSeedItem("entity.personnelOperationRate.actualworktime", "ja-JP", "在岗作业时间(分钟)", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personnelOperationRate.actualworktime
            new TranslationSeedItem("entity.personnelOperationRate.actualworktime", "zh-CN", "在岗作业时间(分钟)", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),
            // entity.personnelOperationRate.actualworktime
            new TranslationSeedItem("entity.personnelOperationRate.actualworktime", "zh-HK", "在岗作业时间(分钟)", "在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。"),

            // entity.personnelOperationRate.breaktime
            new TranslationSeedItem("entity.personnelOperationRate.breaktime", "en-US", "休息时间(分钟)", "休息时间（分钟）"),
            // entity.personnelOperationRate.breaktime
            new TranslationSeedItem("entity.personnelOperationRate.breaktime", "ja-JP", "休息时间(分钟)", "休息时间（分钟）"),
            // entity.personnelOperationRate.breaktime
            new TranslationSeedItem("entity.personnelOperationRate.breaktime", "zh-CN", "休息时间(分钟)", "休息时间（分钟）"),
            // entity.personnelOperationRate.breaktime
            new TranslationSeedItem("entity.personnelOperationRate.breaktime", "zh-HK", "休息时间(分钟)", "休息时间（分钟）"),

            // entity.personnelOperationRate.idletime
            new TranslationSeedItem("entity.personnelOperationRate.idletime", "en-US", "空闲时间(分钟)", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personnelOperationRate.idletime
            new TranslationSeedItem("entity.personnelOperationRate.idletime", "ja-JP", "空闲时间(分钟)", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personnelOperationRate.idletime
            new TranslationSeedItem("entity.personnelOperationRate.idletime", "zh-CN", "空闲时间(分钟)", "空闲时间（分钟）。等料、设备调试等非作业时间。"),
            // entity.personnelOperationRate.idletime
            new TranslationSeedItem("entity.personnelOperationRate.idletime", "zh-HK", "空闲时间(分钟)", "空闲时间（分钟）。等料、设备调试等非作业时间。"),

            // entity.personnelOperationRate.personneloperationrate
            new TranslationSeedItem("entity.personnelOperationRate.personneloperationrate", "en-US", "人员稼动率(%)", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personnelOperationRate.personneloperationrate
            new TranslationSeedItem("entity.personnelOperationRate.personneloperationrate", "ja-JP", "人员稼动率(%)", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personnelOperationRate.personneloperationrate
            new TranslationSeedItem("entity.personnelOperationRate.personneloperationrate", "zh-CN", "人员稼动率(%)", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),
            // entity.personnelOperationRate.personneloperationrate
            new TranslationSeedItem("entity.personnelOperationRate.personneloperationrate", "zh-HK", "人员稼动率(%)", "人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。"),

            // entity.personnelOperationRate.plannedoutput
            new TranslationSeedItem("entity.personnelOperationRate.plannedoutput", "en-US", "计划产量", "计划产量"),
            // entity.personnelOperationRate.plannedoutput
            new TranslationSeedItem("entity.personnelOperationRate.plannedoutput", "ja-JP", "计划产量", "计划产量"),
            // entity.personnelOperationRate.plannedoutput
            new TranslationSeedItem("entity.personnelOperationRate.plannedoutput", "zh-CN", "计划产量", "计划产量"),
            // entity.personnelOperationRate.plannedoutput
            new TranslationSeedItem("entity.personnelOperationRate.plannedoutput", "zh-HK", "计划产量", "计划产量"),

            // entity.personnelOperationRate.actualoutput
            new TranslationSeedItem("entity.personnelOperationRate.actualoutput", "en-US", "实际产量", "实际产量"),
            // entity.personnelOperationRate.actualoutput
            new TranslationSeedItem("entity.personnelOperationRate.actualoutput", "ja-JP", "实际产量", "实际产量"),
            // entity.personnelOperationRate.actualoutput
            new TranslationSeedItem("entity.personnelOperationRate.actualoutput", "zh-CN", "实际产量", "实际产量"),
            // entity.personnelOperationRate.actualoutput
            new TranslationSeedItem("entity.personnelOperationRate.actualoutput", "zh-HK", "实际产量", "实际产量"),

            // entity.personnelOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.personnelOperationRate.qualifiedquantity", "en-US", "合格品数量", "合格品数量"),
            // entity.personnelOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.personnelOperationRate.qualifiedquantity", "ja-JP", "合格品数量", "合格品数量"),
            // entity.personnelOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.personnelOperationRate.qualifiedquantity", "zh-CN", "合格品数量", "合格品数量"),
            // entity.personnelOperationRate.qualifiedquantity
            new TranslationSeedItem("entity.personnelOperationRate.qualifiedquantity", "zh-HK", "合格品数量", "合格品数量"),

            // entity.personnelOperationRate.defectivequantity
            new TranslationSeedItem("entity.personnelOperationRate.defectivequantity", "en-US", "不良品数量", "不良品数量"),
            // entity.personnelOperationRate.defectivequantity
            new TranslationSeedItem("entity.personnelOperationRate.defectivequantity", "ja-JP", "不良品数量", "不良品数量"),
            // entity.personnelOperationRate.defectivequantity
            new TranslationSeedItem("entity.personnelOperationRate.defectivequantity", "zh-CN", "不良品数量", "不良品数量"),
            // entity.personnelOperationRate.defectivequantity
            new TranslationSeedItem("entity.personnelOperationRate.defectivequantity", "zh-HK", "不良品数量", "不良品数量"),

            // entity.personnelOperationRate.yieldrate
            new TranslationSeedItem("entity.personnelOperationRate.yieldrate", "en-US", "良品率(%)", "良品率（%）"),
            // entity.personnelOperationRate.yieldrate
            new TranslationSeedItem("entity.personnelOperationRate.yieldrate", "ja-JP", "良品率(%)", "良品率（%）"),
            // entity.personnelOperationRate.yieldrate
            new TranslationSeedItem("entity.personnelOperationRate.yieldrate", "zh-CN", "良品率(%)", "良品率（%）"),
            // entity.personnelOperationRate.yieldrate
            new TranslationSeedItem("entity.personnelOperationRate.yieldrate", "zh-HK", "良品率(%)", "良品率（%）"),

            // entity.personnelOperationRate.workefficiency
            new TranslationSeedItem("entity.personnelOperationRate.workefficiency", "en-US", "工作效率(%)", "工作效率（%）"),
            // entity.personnelOperationRate.workefficiency
            new TranslationSeedItem("entity.personnelOperationRate.workefficiency", "ja-JP", "工作效率(%)", "工作效率（%）"),
            // entity.personnelOperationRate.workefficiency
            new TranslationSeedItem("entity.personnelOperationRate.workefficiency", "zh-CN", "工作效率(%)", "工作效率（%）"),
            // entity.personnelOperationRate.workefficiency
            new TranslationSeedItem("entity.personnelOperationRate.workefficiency", "zh-HK", "工作效率(%)", "工作效率（%）"),

            // entity.personnelOperationRate.idlereasontype
            new TranslationSeedItem("entity.personnelOperationRate.idlereasontype", "en-US", "空闲原因类型", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personnelOperationRate.idlereasontype
            new TranslationSeedItem("entity.personnelOperationRate.idlereasontype", "ja-JP", "空闲原因类型", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personnelOperationRate.idlereasontype
            new TranslationSeedItem("entity.personnelOperationRate.idlereasontype", "zh-CN", "空闲原因类型", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),
            // entity.personnelOperationRate.idlereasontype
            new TranslationSeedItem("entity.personnelOperationRate.idlereasontype", "zh-HK", "空闲原因类型", "空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）"),

            // entity.personnelOperationRate.idlereason
            new TranslationSeedItem("entity.personnelOperationRate.idlereason", "en-US", "空闲原因描述", "空闲原因描述"),
            // entity.personnelOperationRate.idlereason
            new TranslationSeedItem("entity.personnelOperationRate.idlereason", "ja-JP", "空闲原因描述", "空闲原因描述"),
            // entity.personnelOperationRate.idlereason
            new TranslationSeedItem("entity.personnelOperationRate.idlereason", "zh-CN", "空闲原因描述", "空闲原因描述"),
            // entity.personnelOperationRate.idlereason
            new TranslationSeedItem("entity.personnelOperationRate.idlereason", "zh-HK", "空闲原因描述", "空闲原因描述"),

            // entity.personnelOperationRate.overtimehours
            new TranslationSeedItem("entity.personnelOperationRate.overtimehours", "en-US", "加班时间(分钟)", "加班时间（分钟）"),
            // entity.personnelOperationRate.overtimehours
            new TranslationSeedItem("entity.personnelOperationRate.overtimehours", "ja-JP", "加班时间(分钟)", "加班时间（分钟）"),
            // entity.personnelOperationRate.overtimehours
            new TranslationSeedItem("entity.personnelOperationRate.overtimehours", "zh-CN", "加班时间(分钟)", "加班时间（分钟）"),
            // entity.personnelOperationRate.overtimehours
            new TranslationSeedItem("entity.personnelOperationRate.overtimehours", "zh-HK", "加班时间(分钟)", "加班时间（分钟）"),

            // entity.personnelOperationRate.teamleader
            new TranslationSeedItem("entity.personnelOperationRate.teamleader", "en-US", "班组长", "班组长"),
            // entity.personnelOperationRate.teamleader
            new TranslationSeedItem("entity.personnelOperationRate.teamleader", "ja-JP", "班组长", "班组长"),
            // entity.personnelOperationRate.teamleader
            new TranslationSeedItem("entity.personnelOperationRate.teamleader", "zh-CN", "班组长", "班组长"),
            // entity.personnelOperationRate.teamleader
            new TranslationSeedItem("entity.personnelOperationRate.teamleader", "zh-HK", "班组长", "班组长"),

            // entity.personnelOperationRate.supervisor
            new TranslationSeedItem("entity.personnelOperationRate.supervisor", "en-US", "主管", "主管"),
            // entity.personnelOperationRate.supervisor
            new TranslationSeedItem("entity.personnelOperationRate.supervisor", "ja-JP", "主管", "主管"),
            // entity.personnelOperationRate.supervisor
            new TranslationSeedItem("entity.personnelOperationRate.supervisor", "zh-CN", "主管", "主管"),
            // entity.personnelOperationRate.supervisor
            new TranslationSeedItem("entity.personnelOperationRate.supervisor", "zh-HK", "主管", "主管"),

            // entity.personnelOperationRate.status
            new TranslationSeedItem("entity.personnelOperationRate.status", "en-US", "状态", "状态（0=正常，1=停用）"),
            // entity.personnelOperationRate.status
            new TranslationSeedItem("entity.personnelOperationRate.status", "ja-JP", "状态", "状态（0=正常，1=停用）"),
            // entity.personnelOperationRate.status
            new TranslationSeedItem("entity.personnelOperationRate.status", "zh-CN", "状态", "状态（0=正常，1=停用）"),
            // entity.personnelOperationRate.status
            new TranslationSeedItem("entity.personnelOperationRate.status", "zh-HK", "状态", "状态（0=正常，1=停用）"),
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
