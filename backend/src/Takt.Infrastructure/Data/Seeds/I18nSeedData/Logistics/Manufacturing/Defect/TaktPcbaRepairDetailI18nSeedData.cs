// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPcbaRepairDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPcbaRepairDetail 实体国际化翻译种子（键前缀 entity.pcbaRepairDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPcbaRepairDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPcbaRepairDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 pcbaRepairDetail 实体翻译...", tenantCode);

        foreach (var item in GetPcbaRepairDetailTranslations())
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

        TaktLogger.Information("TaktPcbaRepairDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPcbaRepairDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.pcbaRepairDetail._self / entity.pcbaRepairDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPcbaRepairDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.pcbaRepairDetail._self
            new TranslationSeedItem("entity.pcbaRepairDetail._self", "en-US", "Pcba Repair Detail Information", "实体名称"),
            // entity.pcbaRepairDetail._self
            new TranslationSeedItem("entity.pcbaRepairDetail._self", "ja-JP", "PCBA改修明细信息", "实体名称"),
            // entity.pcbaRepairDetail._self
            new TranslationSeedItem("entity.pcbaRepairDetail._self", "zh-CN", "PCBA改修明细信息", "实体名称"),
            // entity.pcbaRepairDetail._self
            new TranslationSeedItem("entity.pcbaRepairDetail._self", "zh-HK", "PCBA改修明细信息", "实体名称"),

            // entity.pcbaRepairDetail.pcbarepairid
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepairid", "en-US", "PCBA改修ID", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaRepairDetail.pcbarepairid
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepairid", "ja-JP", "PCBA改修ID", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaRepairDetail.pcbarepairid
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepairid", "zh-CN", "PCBA改修ID", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.pcbaRepairDetail.pcbarepairid
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepairid", "zh-HK", "PCBA改修ID", "PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.pcbaRepairDetail.prodordercode
            new TranslationSeedItem("entity.pcbaRepairDetail.prodordercode", "en-US", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaRepairDetail.prodordercode
            new TranslationSeedItem("entity.pcbaRepairDetail.prodordercode", "ja-JP", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaRepairDetail.prodordercode
            new TranslationSeedItem("entity.pcbaRepairDetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.pcbaRepairDetail.prodordercode
            new TranslationSeedItem("entity.pcbaRepairDetail.prodordercode", "zh-HK", "生产工单号", "生产工单号（冗余字段,便于查询）"),

            // entity.pcbaRepairDetail.linenumber
            new TranslationSeedItem("entity.pcbaRepairDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaRepairDetail.linenumber
            new TranslationSeedItem("entity.pcbaRepairDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaRepairDetail.linenumber
            new TranslationSeedItem("entity.pcbaRepairDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.pcbaRepairDetail.linenumber
            new TranslationSeedItem("entity.pcbaRepairDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.pcbaRepairDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbaboardtype", "en-US", "PCBA板别", "PCBA板别"),
            // entity.pcbaRepairDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbaboardtype", "ja-JP", "PCBA板别", "PCBA板别"),
            // entity.pcbaRepairDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbaboardtype", "zh-CN", "PCBA板别", "PCBA板别"),
            // entity.pcbaRepairDetail.pcbaboardtype
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbaboardtype", "zh-HK", "PCBA板别", "PCBA板别"),

            // entity.pcbaRepairDetail.prodactualqty
            new TranslationSeedItem("entity.pcbaRepairDetail.prodactualqty", "en-US", "生产实绩", "生产实绩"),
            // entity.pcbaRepairDetail.prodactualqty
            new TranslationSeedItem("entity.pcbaRepairDetail.prodactualqty", "ja-JP", "生产实绩", "生产实绩"),
            // entity.pcbaRepairDetail.prodactualqty
            new TranslationSeedItem("entity.pcbaRepairDetail.prodactualqty", "zh-CN", "生产实绩", "生产实绩"),
            // entity.pcbaRepairDetail.prodactualqty
            new TranslationSeedItem("entity.pcbaRepairDetail.prodactualqty", "zh-HK", "生产实绩", "生产实绩"),

            // entity.pcbaRepairDetail.prodline
            new TranslationSeedItem("entity.pcbaRepairDetail.prodline", "en-US", "生产线", "生产线"),
            // entity.pcbaRepairDetail.prodline
            new TranslationSeedItem("entity.pcbaRepairDetail.prodline", "ja-JP", "生产线", "生产线"),
            // entity.pcbaRepairDetail.prodline
            new TranslationSeedItem("entity.pcbaRepairDetail.prodline", "zh-CN", "生产线", "生产线"),
            // entity.pcbaRepairDetail.prodline
            new TranslationSeedItem("entity.pcbaRepairDetail.prodline", "zh-HK", "生产线", "生产线"),

            // entity.pcbaRepairDetail.cardno
            new TranslationSeedItem("entity.pcbaRepairDetail.cardno", "en-US", "卡号", "卡号"),
            // entity.pcbaRepairDetail.cardno
            new TranslationSeedItem("entity.pcbaRepairDetail.cardno", "ja-JP", "卡号", "卡号"),
            // entity.pcbaRepairDetail.cardno
            new TranslationSeedItem("entity.pcbaRepairDetail.cardno", "zh-CN", "卡号", "卡号"),
            // entity.pcbaRepairDetail.cardno
            new TranslationSeedItem("entity.pcbaRepairDetail.cardno", "zh-HK", "卡号", "卡号"),

            // entity.pcbaRepairDetail.defectsymptom
            new TranslationSeedItem("entity.pcbaRepairDetail.defectsymptom", "en-US", "不良症状", "不良症状"),
            // entity.pcbaRepairDetail.defectsymptom
            new TranslationSeedItem("entity.pcbaRepairDetail.defectsymptom", "ja-JP", "不良症状", "不良症状"),
            // entity.pcbaRepairDetail.defectsymptom
            new TranslationSeedItem("entity.pcbaRepairDetail.defectsymptom", "zh-CN", "不良症状", "不良症状"),
            // entity.pcbaRepairDetail.defectsymptom
            new TranslationSeedItem("entity.pcbaRepairDetail.defectsymptom", "zh-HK", "不良症状", "不良症状"),

            // entity.pcbaRepairDetail.defectengineering
            new TranslationSeedItem("entity.pcbaRepairDetail.defectengineering", "en-US", "检出工程", "检出工程"),
            // entity.pcbaRepairDetail.defectengineering
            new TranslationSeedItem("entity.pcbaRepairDetail.defectengineering", "ja-JP", "检出工程", "检出工程"),
            // entity.pcbaRepairDetail.defectengineering
            new TranslationSeedItem("entity.pcbaRepairDetail.defectengineering", "zh-CN", "检出工程", "检出工程"),
            // entity.pcbaRepairDetail.defectengineering
            new TranslationSeedItem("entity.pcbaRepairDetail.defectengineering", "zh-HK", "检出工程", "检出工程"),

            // entity.pcbaRepairDetail.defectreason
            new TranslationSeedItem("entity.pcbaRepairDetail.defectreason", "en-US", "不良原因", "不良原因"),
            // entity.pcbaRepairDetail.defectreason
            new TranslationSeedItem("entity.pcbaRepairDetail.defectreason", "ja-JP", "不良原因", "不良原因"),
            // entity.pcbaRepairDetail.defectreason
            new TranslationSeedItem("entity.pcbaRepairDetail.defectreason", "zh-CN", "不良原因", "不良原因"),
            // entity.pcbaRepairDetail.defectreason
            new TranslationSeedItem("entity.pcbaRepairDetail.defectreason", "zh-HK", "不良原因", "不良原因"),

            // entity.pcbaRepairDetail.defectqty
            new TranslationSeedItem("entity.pcbaRepairDetail.defectqty", "en-US", "不良数量", "不良数量"),
            // entity.pcbaRepairDetail.defectqty
            new TranslationSeedItem("entity.pcbaRepairDetail.defectqty", "ja-JP", "不良数量", "不良数量"),
            // entity.pcbaRepairDetail.defectqty
            new TranslationSeedItem("entity.pcbaRepairDetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.pcbaRepairDetail.defectqty
            new TranslationSeedItem("entity.pcbaRepairDetail.defectqty", "zh-HK", "不良数量", "不良数量"),

            // entity.pcbaRepairDetail.defectresponsibility
            new TranslationSeedItem("entity.pcbaRepairDetail.defectresponsibility", "en-US", "责任归属", "责任归属"),
            // entity.pcbaRepairDetail.defectresponsibility
            new TranslationSeedItem("entity.pcbaRepairDetail.defectresponsibility", "ja-JP", "责任归属", "责任归属"),
            // entity.pcbaRepairDetail.defectresponsibility
            new TranslationSeedItem("entity.pcbaRepairDetail.defectresponsibility", "zh-CN", "责任归属", "责任归属"),
            // entity.pcbaRepairDetail.defectresponsibility
            new TranslationSeedItem("entity.pcbaRepairDetail.defectresponsibility", "zh-HK", "责任归属", "责任归属"),

            // entity.pcbaRepairDetail.defectnature
            new TranslationSeedItem("entity.pcbaRepairDetail.defectnature", "en-US", "不良性质", "不良性质"),
            // entity.pcbaRepairDetail.defectnature
            new TranslationSeedItem("entity.pcbaRepairDetail.defectnature", "ja-JP", "不良性质", "不良性质"),
            // entity.pcbaRepairDetail.defectnature
            new TranslationSeedItem("entity.pcbaRepairDetail.defectnature", "zh-CN", "不良性质", "不良性质"),
            // entity.pcbaRepairDetail.defectnature
            new TranslationSeedItem("entity.pcbaRepairDetail.defectnature", "zh-HK", "不良性质", "不良性质"),

            // entity.pcbaRepairDetail.repairoperator
            new TranslationSeedItem("entity.pcbaRepairDetail.repairoperator", "en-US", "修理员", "修理员"),
            // entity.pcbaRepairDetail.repairoperator
            new TranslationSeedItem("entity.pcbaRepairDetail.repairoperator", "ja-JP", "修理员", "修理员"),
            // entity.pcbaRepairDetail.repairoperator
            new TranslationSeedItem("entity.pcbaRepairDetail.repairoperator", "zh-CN", "修理员", "修理员"),
            // entity.pcbaRepairDetail.repairoperator
            new TranslationSeedItem("entity.pcbaRepairDetail.repairoperator", "zh-HK", "修理员", "修理员"),

            // entity.pcbaRepairDetail.pcbarepair
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepair", "en-US", "PCBA改修日报", "PCBA改修日报（主表）"),
            // entity.pcbaRepairDetail.pcbarepair
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepair", "ja-JP", "PCBA改修日报", "PCBA改修日报（主表）"),
            // entity.pcbaRepairDetail.pcbarepair
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepair", "zh-CN", "PCBA改修日报", "PCBA改修日报（主表）"),
            // entity.pcbaRepairDetail.pcbarepair
            new TranslationSeedItem("entity.pcbaRepairDetail.pcbarepair", "zh-HK", "PCBA改修日报", "PCBA改修日报（主表）"),
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
