// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaOutputDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPcbaOutputDetail 实体国际化翻译种子（键前缀 entity.pcbaOutputDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaOutputDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaOutputDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaOutputDetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaOutputDetailTranslations())
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

        TaktLogger.Information("TaktPcbaOutputDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaOutputDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaOutputDetail._self / entity.pcbaOutputDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaOutputDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaOutputDetail._self
            new TranslationSeedItem("entity.pcbaOutputDetail._self", "en-US", "Pcba Output Detail Information", "实体名称"),
            // entity.pcbaOutputDetail._self
            new TranslationSeedItem("entity.pcbaOutputDetail._self", "ja-JP", "PCBA明细信息", "实体名称"),
            // entity.pcbaOutputDetail._self
            new TranslationSeedItem("entity.pcbaOutputDetail._self", "zh-CN", "PCBA明细信息", "实体名称"),
            // entity.pcbaOutputDetail._self
            new TranslationSeedItem("entity.pcbaOutputDetail._self", "zh-HK", "PCBA明细信息", "实体名称"),

            // entity.pcbaOutputDetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbaoutputid", "en-US", "PCBA日报ID", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaOutputDetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbaoutputid", "ja-JP", "PCBA日报ID", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaOutputDetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbaoutputid", "zh-CN", "PCBA日报ID", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaOutputDetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbaoutputid", "zh-HK", "PCBA日报ID", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbaOutputDetail.prodordercode
            new TranslationSeedItem("entity.pcbaOutputDetail.prodordercode", "en-US", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaOutputDetail.prodordercode
            new TranslationSeedItem("entity.pcbaOutputDetail.prodordercode", "ja-JP", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaOutputDetail.prodordercode
            new TranslationSeedItem("entity.pcbaOutputDetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaOutputDetail.prodordercode
            new TranslationSeedItem("entity.pcbaOutputDetail.prodordercode", "zh-HK", "生产工单号", "生产工单号（冗余字段,便于查询）"),

            // entity.pcbaOutputDetail.linenumber
            new TranslationSeedItem("entity.pcbaOutputDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaOutputDetail.linenumber
            new TranslationSeedItem("entity.pcbaOutputDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaOutputDetail.linenumber
            new TranslationSeedItem("entity.pcbaOutputDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaOutputDetail.linenumber
            new TranslationSeedItem("entity.pcbaOutputDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.pcbaOutputDetail.timeperiod
            new TranslationSeedItem("entity.pcbaOutputDetail.timeperiod", "en-US", "生产时段", "生产时段"),
            // entity.pcbaOutputDetail.timeperiod
            new TranslationSeedItem("entity.pcbaOutputDetail.timeperiod", "ja-JP", "生产时段", "生产时段"),
            // entity.pcbaOutputDetail.timeperiod
            new TranslationSeedItem("entity.pcbaOutputDetail.timeperiod", "zh-CN", "生产时段", "生产时段"),
            // entity.pcbaOutputDetail.timeperiod
            new TranslationSeedItem("entity.pcbaOutputDetail.timeperiod", "zh-HK", "生产时段", "生产时段"),

            // entity.pcbaOutputDetail.shiftno
            new TranslationSeedItem("entity.pcbaOutputDetail.shiftno", "en-US", "班组", "班组"),
            // entity.pcbaOutputDetail.shiftno
            new TranslationSeedItem("entity.pcbaOutputDetail.shiftno", "ja-JP", "班组", "班组"),
            // entity.pcbaOutputDetail.shiftno
            new TranslationSeedItem("entity.pcbaOutputDetail.shiftno", "zh-CN", "班组", "班组"),
            // entity.pcbaOutputDetail.shiftno
            new TranslationSeedItem("entity.pcbaOutputDetail.shiftno", "zh-HK", "班组", "班组"),

            // entity.pcbaOutputDetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbboardtype", "en-US", "PCB板别", "板别（PCB板别）"),
            // entity.pcbaOutputDetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbboardtype", "ja-JP", "PCB板别", "板别（PCB板别）"),
            // entity.pcbaOutputDetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbboardtype", "zh-CN", "PCB板别", "板别（PCB板别）"),
            // entity.pcbaOutputDetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaOutputDetail.pcbboardtype", "zh-HK", "PCB板别", "板别（PCB板别）"),

            // entity.pcbaOutputDetail.panelside
            new TranslationSeedItem("entity.pcbaOutputDetail.panelside", "en-US", "面板别", "面板别"),
            // entity.pcbaOutputDetail.panelside
            new TranslationSeedItem("entity.pcbaOutputDetail.panelside", "ja-JP", "面板别", "面板别"),
            // entity.pcbaOutputDetail.panelside
            new TranslationSeedItem("entity.pcbaOutputDetail.panelside", "zh-CN", "面板别", "面板别"),
            // entity.pcbaOutputDetail.panelside
            new TranslationSeedItem("entity.pcbaOutputDetail.panelside", "zh-HK", "面板别", "面板别"),

            // entity.pcbaOutputDetail.batchqty
            new TranslationSeedItem("entity.pcbaOutputDetail.batchqty", "en-US", "批次数量", "批次数量"),
            // entity.pcbaOutputDetail.batchqty
            new TranslationSeedItem("entity.pcbaOutputDetail.batchqty", "ja-JP", "批次数量", "批次数量"),
            // entity.pcbaOutputDetail.batchqty
            new TranslationSeedItem("entity.pcbaOutputDetail.batchqty", "zh-CN", "批次数量", "批次数量"),
            // entity.pcbaOutputDetail.batchqty
            new TranslationSeedItem("entity.pcbaOutputDetail.batchqty", "zh-HK", "批次数量", "批次数量"),

            // entity.pcbaOutputDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.dailycompletedqty", "en-US", "当日完成数", "当日完成数"),
            // entity.pcbaOutputDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.dailycompletedqty", "ja-JP", "当日完成数", "当日完成数"),
            // entity.pcbaOutputDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.dailycompletedqty", "zh-CN", "当日完成数", "当日完成数"),
            // entity.pcbaOutputDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.dailycompletedqty", "zh-HK", "当日完成数", "当日完成数"),

            // entity.pcbaOutputDetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.totalcompletedqty", "en-US", "累计完成数", "累计完成数"),
            // entity.pcbaOutputDetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.totalcompletedqty", "ja-JP", "累计完成数", "累计完成数"),
            // entity.pcbaOutputDetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.totalcompletedqty", "zh-CN", "累计完成数", "累计完成数"),
            // entity.pcbaOutputDetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaOutputDetail.totalcompletedqty", "zh-HK", "累计完成数", "累计完成数"),

            // entity.pcbaOutputDetail.completedstatus
            new TranslationSeedItem("entity.pcbaOutputDetail.completedstatus", "en-US", "完成状态", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaOutputDetail.completedstatus
            new TranslationSeedItem("entity.pcbaOutputDetail.completedstatus", "ja-JP", "完成状态", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaOutputDetail.completedstatus
            new TranslationSeedItem("entity.pcbaOutputDetail.completedstatus", "zh-CN", "完成状态", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaOutputDetail.completedstatus
            new TranslationSeedItem("entity.pcbaOutputDetail.completedstatus", "zh-HK", "完成状态", "完成状态（0=未完成 1=部分完成 2=已完成）"),

            // entity.pcbaOutputDetail.serialno
            new TranslationSeedItem("entity.pcbaOutputDetail.serialno", "en-US", "序列号", "序列号"),
            // entity.pcbaOutputDetail.serialno
            new TranslationSeedItem("entity.pcbaOutputDetail.serialno", "ja-JP", "序列号", "序列号"),
            // entity.pcbaOutputDetail.serialno
            new TranslationSeedItem("entity.pcbaOutputDetail.serialno", "zh-CN", "序列号", "序列号"),
            // entity.pcbaOutputDetail.serialno
            new TranslationSeedItem("entity.pcbaOutputDetail.serialno", "zh-HK", "序列号", "序列号"),

            // entity.pcbaOutputDetail.defectcount
            new TranslationSeedItem("entity.pcbaOutputDetail.defectcount", "en-US", "不良台数", "不良台数"),
            // entity.pcbaOutputDetail.defectcount
            new TranslationSeedItem("entity.pcbaOutputDetail.defectcount", "ja-JP", "不良台数", "不良台数"),
            // entity.pcbaOutputDetail.defectcount
            new TranslationSeedItem("entity.pcbaOutputDetail.defectcount", "zh-CN", "不良台数", "不良台数"),
            // entity.pcbaOutputDetail.defectcount
            new TranslationSeedItem("entity.pcbaOutputDetail.defectcount", "zh-HK", "不良台数", "不良台数"),

            // entity.pcbaOutputDetail.inputminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.inputminutes", "en-US", "投入工数", "投入工数(分钟)"),
            // entity.pcbaOutputDetail.inputminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.inputminutes", "ja-JP", "投入工数", "投入工数(分钟)"),
            // entity.pcbaOutputDetail.inputminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.inputminutes", "zh-CN", "投入工数", "投入工数(分钟)"),
            // entity.pcbaOutputDetail.inputminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.inputminutes", "zh-HK", "投入工数", "投入工数(分钟)"),

            // entity.pcbaOutputDetail.repairminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.repairminutes", "en-US", "修工数", "修工数(分钟)"),
            // entity.pcbaOutputDetail.repairminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.repairminutes", "ja-JP", "修工数", "修工数(分钟)"),
            // entity.pcbaOutputDetail.repairminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.repairminutes", "zh-CN", "修工数", "修工数(分钟)"),
            // entity.pcbaOutputDetail.repairminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.repairminutes", "zh-HK", "修工数", "修工数(分钟)"),

            // entity.pcbaOutputDetail.switchcount
            new TranslationSeedItem("entity.pcbaOutputDetail.switchcount", "en-US", "切换次数", "切换次数"),
            // entity.pcbaOutputDetail.switchcount
            new TranslationSeedItem("entity.pcbaOutputDetail.switchcount", "ja-JP", "切换次数", "切换次数"),
            // entity.pcbaOutputDetail.switchcount
            new TranslationSeedItem("entity.pcbaOutputDetail.switchcount", "zh-CN", "切换次数", "切换次数"),
            // entity.pcbaOutputDetail.switchcount
            new TranslationSeedItem("entity.pcbaOutputDetail.switchcount", "zh-HK", "切换次数", "切换次数"),

            // entity.pcbaOutputDetail.switchtime
            new TranslationSeedItem("entity.pcbaOutputDetail.switchtime", "en-US", "切换时间", "切换时间(分钟)"),
            // entity.pcbaOutputDetail.switchtime
            new TranslationSeedItem("entity.pcbaOutputDetail.switchtime", "ja-JP", "切换时间", "切换时间(分钟)"),
            // entity.pcbaOutputDetail.switchtime
            new TranslationSeedItem("entity.pcbaOutputDetail.switchtime", "zh-CN", "切换时间", "切换时间(分钟)"),
            // entity.pcbaOutputDetail.switchtime
            new TranslationSeedItem("entity.pcbaOutputDetail.switchtime", "zh-HK", "切换时间", "切换时间(分钟)"),

            // entity.pcbaOutputDetail.stoptime
            new TranslationSeedItem("entity.pcbaOutputDetail.stoptime", "en-US", "切停机时间", "切停机时间(分钟)"),
            // entity.pcbaOutputDetail.stoptime
            new TranslationSeedItem("entity.pcbaOutputDetail.stoptime", "ja-JP", "切停机时间", "切停机时间(分钟)"),
            // entity.pcbaOutputDetail.stoptime
            new TranslationSeedItem("entity.pcbaOutputDetail.stoptime", "zh-CN", "切停机时间", "切停机时间(分钟)"),
            // entity.pcbaOutputDetail.stoptime
            new TranslationSeedItem("entity.pcbaOutputDetail.stoptime", "zh-HK", "切停机时间", "切停机时间(分钟)"),

            // entity.pcbaOutputDetail.totalminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.totalminutes", "en-US", "总工数", "总工数(分钟)"),
            // entity.pcbaOutputDetail.totalminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.totalminutes", "ja-JP", "总工数", "总工数(分钟)"),
            // entity.pcbaOutputDetail.totalminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.totalminutes", "zh-CN", "总工数", "总工数(分钟)"),
            // entity.pcbaOutputDetail.totalminutes
            new TranslationSeedItem("entity.pcbaOutputDetail.totalminutes", "zh-HK", "总工数", "总工数(分钟)"),

            // entity.pcbaOutputDetail.unachievedreason
            new TranslationSeedItem("entity.pcbaOutputDetail.unachievedreason", "en-US", "未达成原因", "未达成原因"),
            // entity.pcbaOutputDetail.unachievedreason
            new TranslationSeedItem("entity.pcbaOutputDetail.unachievedreason", "ja-JP", "未达成原因", "未达成原因"),
            // entity.pcbaOutputDetail.unachievedreason
            new TranslationSeedItem("entity.pcbaOutputDetail.unachievedreason", "zh-CN", "未达成原因", "未达成原因"),
            // entity.pcbaOutputDetail.unachievedreason
            new TranslationSeedItem("entity.pcbaOutputDetail.unachievedreason", "zh-HK", "未达成原因", "未达成原因"),

            // entity.pcbaOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaOutputDetail.unachieveddescription", "en-US", "未达成说明", "未达成说明"),
            // entity.pcbaOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaOutputDetail.unachieveddescription", "ja-JP", "未达成说明", "未达成说明"),
            // entity.pcbaOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaOutputDetail.unachieveddescription", "zh-CN", "未达成说明", "未达成说明"),
            // entity.pcbaOutputDetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaOutputDetail.unachieveddescription", "zh-HK", "未达成说明", "未达成说明"),
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
