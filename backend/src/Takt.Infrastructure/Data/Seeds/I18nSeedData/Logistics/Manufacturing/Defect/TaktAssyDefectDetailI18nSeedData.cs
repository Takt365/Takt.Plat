// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAssyDefectDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktAssyDefectDetail 实体国际化翻译种子（键前缀 entity.assyDefectDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAssyDefectDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAssyDefectDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 assyDefectDetail 实体翻译...", tenantCode);

        foreach (var item in GetAssyDefectDetailTranslations())
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

        TaktLogger.Information("TaktAssyDefectDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAssyDefectDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.assyDefectDetail._self / entity.assyDefectDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAssyDefectDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.assyDefectDetail._self
            new TranslationSeedItem("entity.assyDefectDetail._self", "en-US", "Assy Defect Detail Information", "实体名称"),
            // entity.assyDefectDetail._self
            new TranslationSeedItem("entity.assyDefectDetail._self", "ja-JP", "组立不良明细信息", "实体名称"),
            // entity.assyDefectDetail._self
            new TranslationSeedItem("entity.assyDefectDetail._self", "zh-CN", "组立不良明细信息", "实体名称"),
            // entity.assyDefectDetail._self
            new TranslationSeedItem("entity.assyDefectDetail._self", "zh-HK", "组立不良明细信息", "实体名称"),

            // entity.assyDefectDetail.assydefectid
            new TranslationSeedItem("entity.assyDefectDetail.assydefectid", "en-US", "组立不良ID", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyDefectDetail.assydefectid
            new TranslationSeedItem("entity.assyDefectDetail.assydefectid", "ja-JP", "组立不良ID", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyDefectDetail.assydefectid
            new TranslationSeedItem("entity.assyDefectDetail.assydefectid", "zh-CN", "组立不良ID", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.assyDefectDetail.assydefectid
            new TranslationSeedItem("entity.assyDefectDetail.assydefectid", "zh-HK", "组立不良ID", "组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.assyDefectDetail.prodordercode
            new TranslationSeedItem("entity.assyDefectDetail.prodordercode", "en-US", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyDefectDetail.prodordercode
            new TranslationSeedItem("entity.assyDefectDetail.prodordercode", "ja-JP", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyDefectDetail.prodordercode
            new TranslationSeedItem("entity.assyDefectDetail.prodordercode", "zh-CN", "生产工单号", "生产工单号（冗余字段,便于查询）"),
            // entity.assyDefectDetail.prodordercode
            new TranslationSeedItem("entity.assyDefectDetail.prodordercode", "zh-HK", "生产工单号", "生产工单号（冗余字段,便于查询）"),

            // entity.assyDefectDetail.linenumber
            new TranslationSeedItem("entity.assyDefectDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyDefectDetail.linenumber
            new TranslationSeedItem("entity.assyDefectDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyDefectDetail.linenumber
            new TranslationSeedItem("entity.assyDefectDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.assyDefectDetail.linenumber
            new TranslationSeedItem("entity.assyDefectDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.assyDefectDetail.defectcategory
            new TranslationSeedItem("entity.assyDefectDetail.defectcategory", "en-US", "不良区分", "不良区分"),
            // entity.assyDefectDetail.defectcategory
            new TranslationSeedItem("entity.assyDefectDetail.defectcategory", "ja-JP", "不良区分", "不良区分"),
            // entity.assyDefectDetail.defectcategory
            new TranslationSeedItem("entity.assyDefectDetail.defectcategory", "zh-CN", "不良区分", "不良区分"),
            // entity.assyDefectDetail.defectcategory
            new TranslationSeedItem("entity.assyDefectDetail.defectcategory", "zh-HK", "不良区分", "不良区分"),

            // entity.assyDefectDetail.defectqty
            new TranslationSeedItem("entity.assyDefectDetail.defectqty", "en-US", "不良数量", "不良数量"),
            // entity.assyDefectDetail.defectqty
            new TranslationSeedItem("entity.assyDefectDetail.defectqty", "ja-JP", "不良数量", "不良数量"),
            // entity.assyDefectDetail.defectqty
            new TranslationSeedItem("entity.assyDefectDetail.defectqty", "zh-CN", "不良数量", "不良数量"),
            // entity.assyDefectDetail.defectqty
            new TranslationSeedItem("entity.assyDefectDetail.defectqty", "zh-HK", "不良数量", "不良数量"),

            // entity.assyDefectDetail.cumulativedefectqty
            new TranslationSeedItem("entity.assyDefectDetail.cumulativedefectqty", "en-US", "累计不良", "累计不良"),
            // entity.assyDefectDetail.cumulativedefectqty
            new TranslationSeedItem("entity.assyDefectDetail.cumulativedefectqty", "ja-JP", "累计不良", "累计不良"),
            // entity.assyDefectDetail.cumulativedefectqty
            new TranslationSeedItem("entity.assyDefectDetail.cumulativedefectqty", "zh-CN", "累计不良", "累计不良"),
            // entity.assyDefectDetail.cumulativedefectqty
            new TranslationSeedItem("entity.assyDefectDetail.cumulativedefectqty", "zh-HK", "累计不良", "累计不良"),

            // entity.assyDefectDetail.randomcardno
            new TranslationSeedItem("entity.assyDefectDetail.randomcardno", "en-US", "随机卡号", "随机卡号"),
            // entity.assyDefectDetail.randomcardno
            new TranslationSeedItem("entity.assyDefectDetail.randomcardno", "ja-JP", "随机卡号", "随机卡号"),
            // entity.assyDefectDetail.randomcardno
            new TranslationSeedItem("entity.assyDefectDetail.randomcardno", "zh-CN", "随机卡号", "随机卡号"),
            // entity.assyDefectDetail.randomcardno
            new TranslationSeedItem("entity.assyDefectDetail.randomcardno", "zh-HK", "随机卡号", "随机卡号"),

            // entity.assyDefectDetail.occurrenceengineering
            new TranslationSeedItem("entity.assyDefectDetail.occurrenceengineering", "en-US", "发生工程", "发生工程"),
            // entity.assyDefectDetail.occurrenceengineering
            new TranslationSeedItem("entity.assyDefectDetail.occurrenceengineering", "ja-JP", "发生工程", "发生工程"),
            // entity.assyDefectDetail.occurrenceengineering
            new TranslationSeedItem("entity.assyDefectDetail.occurrenceengineering", "zh-CN", "发生工程", "发生工程"),
            // entity.assyDefectDetail.occurrenceengineering
            new TranslationSeedItem("entity.assyDefectDetail.occurrenceengineering", "zh-HK", "发生工程", "发生工程"),

            // entity.assyDefectDetail.teststep
            new TranslationSeedItem("entity.assyDefectDetail.teststep", "en-US", "测试步骤", "测试步骤"),
            // entity.assyDefectDetail.teststep
            new TranslationSeedItem("entity.assyDefectDetail.teststep", "ja-JP", "测试步骤", "测试步骤"),
            // entity.assyDefectDetail.teststep
            new TranslationSeedItem("entity.assyDefectDetail.teststep", "zh-CN", "测试步骤", "测试步骤"),
            // entity.assyDefectDetail.teststep
            new TranslationSeedItem("entity.assyDefectDetail.teststep", "zh-HK", "测试步骤", "测试步骤"),

            // entity.assyDefectDetail.defectsymptom
            new TranslationSeedItem("entity.assyDefectDetail.defectsymptom", "en-US", "不良症状", "不良症状"),
            // entity.assyDefectDetail.defectsymptom
            new TranslationSeedItem("entity.assyDefectDetail.defectsymptom", "ja-JP", "不良症状", "不良症状"),
            // entity.assyDefectDetail.defectsymptom
            new TranslationSeedItem("entity.assyDefectDetail.defectsymptom", "zh-CN", "不良症状", "不良症状"),
            // entity.assyDefectDetail.defectsymptom
            new TranslationSeedItem("entity.assyDefectDetail.defectsymptom", "zh-HK", "不良症状", "不良症状"),

            // entity.assyDefectDetail.defectlocation
            new TranslationSeedItem("entity.assyDefectDetail.defectlocation", "en-US", "不良个所", "不良个所"),
            // entity.assyDefectDetail.defectlocation
            new TranslationSeedItem("entity.assyDefectDetail.defectlocation", "ja-JP", "不良个所", "不良个所"),
            // entity.assyDefectDetail.defectlocation
            new TranslationSeedItem("entity.assyDefectDetail.defectlocation", "zh-CN", "不良个所", "不良个所"),
            // entity.assyDefectDetail.defectlocation
            new TranslationSeedItem("entity.assyDefectDetail.defectlocation", "zh-HK", "不良个所", "不良个所"),

            // entity.assyDefectDetail.defectreason
            new TranslationSeedItem("entity.assyDefectDetail.defectreason", "en-US", "不良原因", "不良原因"),
            // entity.assyDefectDetail.defectreason
            new TranslationSeedItem("entity.assyDefectDetail.defectreason", "ja-JP", "不良原因", "不良原因"),
            // entity.assyDefectDetail.defectreason
            new TranslationSeedItem("entity.assyDefectDetail.defectreason", "zh-CN", "不良原因", "不良原因"),
            // entity.assyDefectDetail.defectreason
            new TranslationSeedItem("entity.assyDefectDetail.defectreason", "zh-HK", "不良原因", "不良原因"),

            // entity.assyDefectDetail.repairoperator
            new TranslationSeedItem("entity.assyDefectDetail.repairoperator", "en-US", "修理员", "修理员"),
            // entity.assyDefectDetail.repairoperator
            new TranslationSeedItem("entity.assyDefectDetail.repairoperator", "ja-JP", "修理员", "修理员"),
            // entity.assyDefectDetail.repairoperator
            new TranslationSeedItem("entity.assyDefectDetail.repairoperator", "zh-CN", "修理员", "修理员"),
            // entity.assyDefectDetail.repairoperator
            new TranslationSeedItem("entity.assyDefectDetail.repairoperator", "zh-HK", "修理员", "修理员"),
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
