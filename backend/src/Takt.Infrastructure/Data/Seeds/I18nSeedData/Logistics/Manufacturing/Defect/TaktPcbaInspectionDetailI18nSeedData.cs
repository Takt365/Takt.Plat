// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaInspectionDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect;

/// <summary>
/// TaktPcbaInspectionDetail 实体国际化翻译种子（键前缀 entity.pcbaInspectionDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaInspectionDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaInspectionDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaInspectionDetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaInspectionDetailTranslations())
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

        TaktLogger.Information("TaktPcbaInspectionDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaInspectionDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaInspectionDetail._self / entity.pcbaInspectionDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaInspectionDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaInspectionDetail._self
            new TranslationSeedItem("entity.pcbaInspectionDetail._self", "en-US", "Pcba Inspection Detail Information", "实体名称"),
            // entity.pcbaInspectionDetail._self
            new TranslationSeedItem("entity.pcbaInspectionDetail._self", "ja-JP", "PCBA检查明细信息", "实体名称"),
            // entity.pcbaInspectionDetail._self
            new TranslationSeedItem("entity.pcbaInspectionDetail._self", "zh-CN", "PCBA检查明细信息", "实体名称"),
            // entity.pcbaInspectionDetail._self
            new TranslationSeedItem("entity.pcbaInspectionDetail._self", "zh-HK", "PCBA检查明细信息", "实体名称"),

            // entity.pcbaInspectionDetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbainspectionid", "en-US", "PCBA检查ID", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaInspectionDetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbainspectionid", "ja-JP", "PCBA检查ID", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaInspectionDetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbainspectionid", "zh-CN", "PCBA检查ID", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaInspectionDetail.pcbainspectionid
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbainspectionid", "zh-HK", "PCBA检查ID", "PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbaInspectionDetail.prodordercode
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodordercode", "en-US", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaInspectionDetail.prodordercode
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodordercode", "ja-JP", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaInspectionDetail.prodordercode
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaInspectionDetail.prodordercode
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodordercode", "zh-HK", "生产工单号", "生产工单号（冗余字段,便于查询）"),

            // entity.pcbaInspectionDetail.linenumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaInspectionDetail.linenumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaInspectionDetail.linenumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaInspectionDetail.linenumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.pcbaInspectionDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbaboardtype", "en-US", "PCBA板别", "PCBA板别"),
            // entity.pcbaInspectionDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbaboardtype", "ja-JP", "PCBA板别", "PCBA板别"),
            // entity.pcbaInspectionDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbaboardtype", "zh-CN", "PCBA板别", "PCBA板别"),
            // entity.pcbaInspectionDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaInspectionDetail.pcbaboardtype", "zh-HK", "PCBA板别", "PCBA板别"),

            // entity.pcbaInspectionDetail.visualinspectionline
            new TranslationSeedItem("entity.pcbaInspectionDetail.visualinspectionline", "en-US", "目视线别", "目视线别"),
            // entity.pcbaInspectionDetail.visualinspectionline
            new TranslationSeedItem("entity.pcbaInspectionDetail.visualinspectionline", "ja-JP", "目视线别", "目视线别"),
            // entity.pcbaInspectionDetail.visualinspectionline
            new TranslationSeedItem("entity.pcbaInspectionDetail.visualinspectionline", "zh-CN", "目视线别", "目视线别"),
            // entity.pcbaInspectionDetail.visualinspectionline
            new TranslationSeedItem("entity.pcbaInspectionDetail.visualinspectionline", "zh-HK", "目视线别", "目视线别"),

            // entity.pcbaInspectionDetail.aoiline
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiline", "en-US", "AOI线别", "AOI线别"),
            // entity.pcbaInspectionDetail.aoiline
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiline", "ja-JP", "AOI线别", "AOI线别"),
            // entity.pcbaInspectionDetail.aoiline
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiline", "zh-CN", "AOI线别", "AOI线别"),
            // entity.pcbaInspectionDetail.aoiline
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiline", "zh-HK", "AOI线别", "AOI线别"),

            // entity.pcbaInspectionDetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.bsideassemblydate", "en-US", "B面实装日期", "B面实装日期"),
            // entity.pcbaInspectionDetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.bsideassemblydate", "ja-JP", "B面实装日期", "B面实装日期"),
            // entity.pcbaInspectionDetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.bsideassemblydate", "zh-CN", "B面实装日期", "B面实装日期"),
            // entity.pcbaInspectionDetail.bsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.bsideassemblydate", "zh-HK", "B面实装日期", "B面实装日期"),

            // entity.pcbaInspectionDetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.tsideassemblydate", "en-US", "T面实装日期", "T面实装日期"),
            // entity.pcbaInspectionDetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.tsideassemblydate", "ja-JP", "T面实装日期", "T面实装日期"),
            // entity.pcbaInspectionDetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.tsideassemblydate", "zh-CN", "T面实装日期", "T面实装日期"),
            // entity.pcbaInspectionDetail.tsideassemblydate
            new TranslationSeedItem("entity.pcbaInspectionDetail.tsideassemblydate", "zh-HK", "T面实装日期", "T面实装日期"),

            // entity.pcbaInspectionDetail.shiftno
            new TranslationSeedItem("entity.pcbaInspectionDetail.shiftno", "en-US", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaInspectionDetail.shiftno
            new TranslationSeedItem("entity.pcbaInspectionDetail.shiftno", "ja-JP", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaInspectionDetail.shiftno
            new TranslationSeedItem("entity.pcbaInspectionDetail.shiftno", "zh-CN", "班次", "班次(1=早班 2=中班 3=晚班)"),
            // entity.pcbaInspectionDetail.shiftno
            new TranslationSeedItem("entity.pcbaInspectionDetail.shiftno", "zh-HK", "班次", "班次(1=早班 2=中班 3=晚班)"),

            // entity.pcbaInspectionDetail.inspectorname
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectorname", "en-US", "检查员", "检查员"),
            // entity.pcbaInspectionDetail.inspectorname
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectorname", "ja-JP", "检查员", "检查员"),
            // entity.pcbaInspectionDetail.inspectorname
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectorname", "zh-CN", "检查员", "检查员"),
            // entity.pcbaInspectionDetail.inspectorname
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectorname", "zh-HK", "检查员", "检查员"),

            // entity.pcbaInspectionDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.dailycompletedqty", "en-US", "当日完成数量", "当日完成数量"),
            // entity.pcbaInspectionDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.dailycompletedqty", "ja-JP", "当日完成数量", "当日完成数量"),
            // entity.pcbaInspectionDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.dailycompletedqty", "zh-CN", "当日完成数量", "当日完成数量"),
            // entity.pcbaInspectionDetail.dailycompletedqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.dailycompletedqty", "zh-HK", "当日完成数量", "当日完成数量"),

            // entity.pcbaInspectionDetail.inspectionqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionqty", "en-US", "检查数量", "检查数量"),
            // entity.pcbaInspectionDetail.inspectionqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionqty", "ja-JP", "检查数量", "检查数量"),
            // entity.pcbaInspectionDetail.inspectionqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionqty", "zh-CN", "检查数量", "检查数量"),
            // entity.pcbaInspectionDetail.inspectionqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionqty", "zh-HK", "检查数量", "检查数量"),

            // entity.pcbaInspectionDetail.inspectionstatus
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionstatus", "en-US", "检查状态", "检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)"),
            // entity.pcbaInspectionDetail.inspectionstatus
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionstatus", "ja-JP", "检查状态", "检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)"),
            // entity.pcbaInspectionDetail.inspectionstatus
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionstatus", "zh-CN", "检查状态", "检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)"),
            // entity.pcbaInspectionDetail.inspectionstatus
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionstatus", "zh-HK", "检查状态", "检查状态(1=检查中 2=测试中 3=检查完成 4=测试完成)"),

            // entity.pcbaInspectionDetail.prodline
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodline", "en-US", "生产线", "生产线"),
            // entity.pcbaInspectionDetail.prodline
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodline", "ja-JP", "生产线", "生产线"),
            // entity.pcbaInspectionDetail.prodline
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodline", "zh-CN", "生产线", "生产线"),
            // entity.pcbaInspectionDetail.prodline
            new TranslationSeedItem("entity.pcbaInspectionDetail.prodline", "zh-HK", "生产线", "生产线"),

            // entity.pcbaInspectionDetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionworkhours", "en-US", "检查工数", "检查工数"),
            // entity.pcbaInspectionDetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionworkhours", "ja-JP", "检查工数", "检查工数"),
            // entity.pcbaInspectionDetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionworkhours", "zh-CN", "检查工数", "检查工数"),
            // entity.pcbaInspectionDetail.inspectionworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.inspectionworkhours", "zh-HK", "检查工数", "检查工数"),

            // entity.pcbaInspectionDetail.aoiworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiworkhours", "en-US", "AOI工数", "AOI工数"),
            // entity.pcbaInspectionDetail.aoiworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiworkhours", "ja-JP", "AOI工数", "AOI工数"),
            // entity.pcbaInspectionDetail.aoiworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiworkhours", "zh-CN", "AOI工数", "AOI工数"),
            // entity.pcbaInspectionDetail.aoiworkhours
            new TranslationSeedItem("entity.pcbaInspectionDetail.aoiworkhours", "zh-HK", "AOI工数", "AOI工数"),

            // entity.pcbaInspectionDetail.defectqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectqty", "en-US", "不良数量", "不良数量"),
            // entity.pcbaInspectionDetail.defectqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectqty", "ja-JP", "不良数量", "不良数量"),
            // entity.pcbaInspectionDetail.defectqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.pcbaInspectionDetail.defectqty
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectqty", "zh-HK", "不良数量", "不良数量"),

            // entity.pcbaInspectionDetail.handplacement
            new TranslationSeedItem("entity.pcbaInspectionDetail.handplacement", "en-US", "手贴", "手贴"),
            // entity.pcbaInspectionDetail.handplacement
            new TranslationSeedItem("entity.pcbaInspectionDetail.handplacement", "ja-JP", "手贴", "手贴"),
            // entity.pcbaInspectionDetail.handplacement
            new TranslationSeedItem("entity.pcbaInspectionDetail.handplacement", "zh-CN", "手贴", "手贴"),
            // entity.pcbaInspectionDetail.handplacement
            new TranslationSeedItem("entity.pcbaInspectionDetail.handplacement", "zh-HK", "手贴", "手贴"),

            // entity.pcbaInspectionDetail.serialnumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.serialnumber", "en-US", "流水号", "流水号"),
            // entity.pcbaInspectionDetail.serialnumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.serialnumber", "ja-JP", "流水号", "流水号"),
            // entity.pcbaInspectionDetail.serialnumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.serialnumber", "zh-CN", "流水号", "流水号"),
            // entity.pcbaInspectionDetail.serialnumber
            new TranslationSeedItem("entity.pcbaInspectionDetail.serialnumber", "zh-HK", "流水号", "流水号"),

            // entity.pcbaInspectionDetail.content
            new TranslationSeedItem("entity.pcbaInspectionDetail.content", "en-US", "内容", "内容"),
            // entity.pcbaInspectionDetail.content
            new TranslationSeedItem("entity.pcbaInspectionDetail.content", "ja-JP", "内容", "内容"),
            // entity.pcbaInspectionDetail.content
            new TranslationSeedItem("entity.pcbaInspectionDetail.content", "zh-CN", "内容", "内容"),
            // entity.pcbaInspectionDetail.content
            new TranslationSeedItem("entity.pcbaInspectionDetail.content", "zh-HK", "内容", "内容"),

            // entity.pcbaInspectionDetail.defectlocation
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectlocation", "en-US", "不良个所", "不良个所"),
            // entity.pcbaInspectionDetail.defectlocation
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectlocation", "ja-JP", "不良个所", "不良个所"),
            // entity.pcbaInspectionDetail.defectlocation
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectlocation", "zh-CN", "不良个所", "不良个所"),
            // entity.pcbaInspectionDetail.defectlocation
            new TranslationSeedItem("entity.pcbaInspectionDetail.defectlocation", "zh-HK", "不良个所", "不良个所"),
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
