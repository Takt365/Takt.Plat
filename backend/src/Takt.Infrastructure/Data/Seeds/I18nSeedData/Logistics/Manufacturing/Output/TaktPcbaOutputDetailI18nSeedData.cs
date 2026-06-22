// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailI18nSeedData.cs
// 创建时间：2026-06-22
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktPcbaOutputDetail 实体国际化翻译种子（键前缀 entity.pcbaoutputdetail.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaoutputdetail 实体翻译...", tenantCode);

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
    /// I18nKey：entity.pcbaoutputdetail._self / entity.pcbaoutputdetail.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaOutputDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "en-US", "Pcba Output Detail Information_us", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "ja-JP", "PCBA明细信息_jp", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "zh-CN", "PCBA明细信息", "实体名称"),
            // entity.pcbaoutputdetail._self
            new TranslationSeedItem("entity.pcbaoutputdetail._self", "zh-HK", "PCBA明细信息_hk", "实体名称"),

            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "en-US", "PCBA日报ID_us", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "ja-JP", "PCBA日报ID_jp", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "zh-CN", "PCBA日报ID", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaoutputdetail.pcbaoutputid
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutputid", "zh-HK", "PCBA日报ID_hk", "PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "en-US", "生产工单号_us", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "ja-JP", "生产工单号_jp", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaoutputdetail.prodordercode
            new TranslationSeedItem("entity.pcbaoutputdetail.prodordercode", "zh-HK", "生产工单号_hk", "生产工单号（冗余字段,便于查询）"),

            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaoutputdetail.linenumber
            new TranslationSeedItem("entity.pcbaoutputdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "en-US", "生产时段_us", "生产时段"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "ja-JP", "生产时段_jp", "生产时段"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "zh-CN", "生产时段", "生产时段"),
            // entity.pcbaoutputdetail.timeperiod
            new TranslationSeedItem("entity.pcbaoutputdetail.timeperiod", "zh-HK", "生产时段_hk", "生产时段"),

            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "en-US", "班组_us", "班组"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "ja-JP", "班组_jp", "班组"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "zh-CN", "班组", "班组"),
            // entity.pcbaoutputdetail.shiftno
            new TranslationSeedItem("entity.pcbaoutputdetail.shiftno", "zh-HK", "班组_hk", "班组"),

            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "en-US", "PCB板别_us", "板别（PCB板别）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "ja-JP", "PCB板别_jp", "板别（PCB板别）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "zh-CN", "PCB板别", "板别（PCB板别）"),
            // entity.pcbaoutputdetail.pcbboardtype
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbboardtype", "zh-HK", "PCB板别_hk", "板别（PCB板别）"),

            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "en-US", "面板别_us", "面板别"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "ja-JP", "面板别_jp", "面板别"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "zh-CN", "面板别", "面板别"),
            // entity.pcbaoutputdetail.panelside
            new TranslationSeedItem("entity.pcbaoutputdetail.panelside", "zh-HK", "面板别_hk", "面板别"),

            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "en-US", "批次数量_us", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "ja-JP", "批次数量_jp", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "zh-CN", "批次数量", "批次数量"),
            // entity.pcbaoutputdetail.batchqty
            new TranslationSeedItem("entity.pcbaoutputdetail.batchqty", "zh-HK", "批次数量_hk", "批次数量"),

            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "en-US", "当日完成数_us", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "ja-JP", "当日完成数_jp", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "zh-CN", "当日完成数", "当日完成数"),
            // entity.pcbaoutputdetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.dailycompletedqty", "zh-HK", "当日完成数_hk", "当日完成数"),

            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "en-US", "累计完成数_us", "累计完成数"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "ja-JP", "累计完成数_jp", "累计完成数"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "zh-CN", "累计完成数", "累计完成数"),
            // entity.pcbaoutputdetail.totalcompletedqty
            new TranslationSeedItem("entity.pcbaoutputdetail.totalcompletedqty", "zh-HK", "累计完成数_hk", "累计完成数"),

            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "en-US", "完成状态_us", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "ja-JP", "完成状态_jp", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "zh-CN", "完成状态", "完成状态（0=未完成 1=部分完成 2=已完成）"),
            // entity.pcbaoutputdetail.completedstatus
            new TranslationSeedItem("entity.pcbaoutputdetail.completedstatus", "zh-HK", "完成状态_hk", "完成状态（0=未完成 1=部分完成 2=已完成）"),

            // entity.pcbaoutputdetail.serialno
            new TranslationSeedItem("entity.pcbaoutputdetail.serialno", "en-US", "序列号_us", "序列号"),
            // entity.pcbaoutputdetail.serialno
            new TranslationSeedItem("entity.pcbaoutputdetail.serialno", "ja-JP", "序列号_jp", "序列号"),
            // entity.pcbaoutputdetail.serialno
            new TranslationSeedItem("entity.pcbaoutputdetail.serialno", "zh-CN", "序列号", "序列号"),
            // entity.pcbaoutputdetail.serialno
            new TranslationSeedItem("entity.pcbaoutputdetail.serialno", "zh-HK", "序列号_hk", "序列号"),

            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "en-US", "不良台数_us", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "ja-JP", "不良台数_jp", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "zh-CN", "不良台数", "不良台数"),
            // entity.pcbaoutputdetail.defectcount
            new TranslationSeedItem("entity.pcbaoutputdetail.defectcount", "zh-HK", "不良台数_hk", "不良台数"),

            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "en-US", "投入工数_us", "投入工数(分钟)"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "ja-JP", "投入工数_jp", "投入工数(分钟)"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "zh-CN", "投入工数", "投入工数(分钟)"),
            // entity.pcbaoutputdetail.inputminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.inputminutes", "zh-HK", "投入工数_hk", "投入工数(分钟)"),

            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "en-US", "修工数_us", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "ja-JP", "修工数_jp", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "zh-CN", "修工数", "修工数(分钟)"),
            // entity.pcbaoutputdetail.repairminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.repairminutes", "zh-HK", "修工数_hk", "修工数(分钟)"),

            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "en-US", "切换次数_us", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "ja-JP", "切换次数_jp", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "zh-CN", "切换次数", "切换次数"),
            // entity.pcbaoutputdetail.switchcount
            new TranslationSeedItem("entity.pcbaoutputdetail.switchcount", "zh-HK", "切换次数_hk", "切换次数"),

            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "en-US", "切换时间_us", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "ja-JP", "切换时间_jp", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "zh-CN", "切换时间", "切换时间(分钟)"),
            // entity.pcbaoutputdetail.switchtime
            new TranslationSeedItem("entity.pcbaoutputdetail.switchtime", "zh-HK", "切换时间_hk", "切换时间(分钟)"),

            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "en-US", "切停机时间_us", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "ja-JP", "切停机时间_jp", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "zh-CN", "切停机时间", "切停机时间(分钟)"),
            // entity.pcbaoutputdetail.stoptime
            new TranslationSeedItem("entity.pcbaoutputdetail.stoptime", "zh-HK", "切停机时间_hk", "切停机时间(分钟)"),

            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "en-US", "总工数_us", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "ja-JP", "总工数_jp", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "zh-CN", "总工数", "总工数(分钟)"),
            // entity.pcbaoutputdetail.totalminutes
            new TranslationSeedItem("entity.pcbaoutputdetail.totalminutes", "zh-HK", "总工数_hk", "总工数(分钟)"),

            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "en-US", "未达成原因_us", "未达成原因"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "ja-JP", "未达成原因_jp", "未达成原因"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "zh-CN", "未达成原因", "未达成原因"),
            // entity.pcbaoutputdetail.unachievedreason
            new TranslationSeedItem("entity.pcbaoutputdetail.unachievedreason", "zh-HK", "未达成原因_hk", "未达成原因"),

            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "en-US", "未达成说明_us", "未达成说明"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "ja-JP", "未达成说明_jp", "未达成说明"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "zh-CN", "未达成说明", "未达成说明"),
            // entity.pcbaoutputdetail.unachieveddescription
            new TranslationSeedItem("entity.pcbaoutputdetail.unachieveddescription", "zh-HK", "未达成说明_hk", "未达成说明"),

            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "en-US", "PCBA日报_us", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "ja-JP", "PCBA日报_jp", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "zh-CN", "PCBA日报", "PCBA日报（主表）"),
            // entity.pcbaoutputdetail.pcbaoutput
            new TranslationSeedItem("entity.pcbaoutputdetail.pcbaoutput", "zh-HK", "PCBA日报_hk", "PCBA日报（主表）"),
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
