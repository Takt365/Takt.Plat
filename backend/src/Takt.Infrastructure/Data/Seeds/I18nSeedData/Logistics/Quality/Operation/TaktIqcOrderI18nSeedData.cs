// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIqcOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktIqcOrder 实体国际化翻译种子（键前缀 entity.iqcOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIqcOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIqcOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 iqcOrder 实体翻译...", tenantCode);

        foreach (var item in GetIqcOrderTranslations())
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

        TaktLogger.Information("TaktIqcOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIqcOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.iqcOrder._self / entity.iqcOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIqcOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.iqcOrder._self
            new TranslationSeedItem("entity.iqcOrder._self", "en-US", "Iqc Order Information", "实体名称"),
            // entity.iqcOrder._self
            new TranslationSeedItem("entity.iqcOrder._self", "ja-JP", "IQC进货检验单信息", "实体名称"),
            // entity.iqcOrder._self
            new TranslationSeedItem("entity.iqcOrder._self", "zh-CN", "IQC进货检验单信息", "实体名称"),
            // entity.iqcOrder._self
            new TranslationSeedItem("entity.iqcOrder._self", "zh-HK", "IQC进货检验单信息", "实体名称"),

            // entity.iqcOrder.plantcode
            new TranslationSeedItem("entity.iqcOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.iqcOrder.plantcode
            new TranslationSeedItem("entity.iqcOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.iqcOrder.plantcode
            new TranslationSeedItem("entity.iqcOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.iqcOrder.plantcode
            new TranslationSeedItem("entity.iqcOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.iqcOrder.sourcecode
            new TranslationSeedItem("entity.iqcOrder.sourcecode", "en-US", "来源单号", "来源单号（采购订单编码）"),
            // entity.iqcOrder.sourcecode
            new TranslationSeedItem("entity.iqcOrder.sourcecode", "ja-JP", "来源单号", "来源单号（采购订单编码）"),
            // entity.iqcOrder.sourcecode
            new TranslationSeedItem("entity.iqcOrder.sourcecode", "zh-CN", "来源单号", "来源单号（采购订单编码）"),
            // entity.iqcOrder.sourcecode
            new TranslationSeedItem("entity.iqcOrder.sourcecode", "zh-HK", "来源单号", "来源单号（采购订单编码）"),

            // entity.iqcOrder.inspectiondate
            new TranslationSeedItem("entity.iqcOrder.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.iqcOrder.inspectiondate
            new TranslationSeedItem("entity.iqcOrder.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.iqcOrder.inspectiondate
            new TranslationSeedItem("entity.iqcOrder.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.iqcOrder.inspectiondate
            new TranslationSeedItem("entity.iqcOrder.inspectiondate", "zh-HK", "检验日期", "检验日期"),

            // entity.iqcOrder.code
            new TranslationSeedItem("entity.iqcOrder.code", "en-US", "IQC检验单编码", "IQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.iqcOrder.code
            new TranslationSeedItem("entity.iqcOrder.code", "ja-JP", "IQC检验单编码", "IQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.iqcOrder.code
            new TranslationSeedItem("entity.iqcOrder.code", "zh-CN", "IQC检验单编码", "IQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.iqcOrder.code
            new TranslationSeedItem("entity.iqcOrder.code", "zh-HK", "IQC检验单编码", "IQC检验单编码（唯一索引，根据来源单号自动生成）"),

            // entity.iqcOrder.suppliercode
            new TranslationSeedItem("entity.iqcOrder.suppliercode", "en-US", "供应商编码", "供应商编码"),
            // entity.iqcOrder.suppliercode
            new TranslationSeedItem("entity.iqcOrder.suppliercode", "ja-JP", "供应商编码", "供应商编码"),
            // entity.iqcOrder.suppliercode
            new TranslationSeedItem("entity.iqcOrder.suppliercode", "zh-CN", "供应商编码", "供应商编码"),
            // entity.iqcOrder.suppliercode
            new TranslationSeedItem("entity.iqcOrder.suppliercode", "zh-HK", "供应商编码", "供应商编码"),

            // entity.iqcOrder.totalpurchasequantity
            new TranslationSeedItem("entity.iqcOrder.totalpurchasequantity", "en-US", "进货总数", "进货总数"),
            // entity.iqcOrder.totalpurchasequantity
            new TranslationSeedItem("entity.iqcOrder.totalpurchasequantity", "ja-JP", "进货总数", "进货总数"),
            // entity.iqcOrder.totalpurchasequantity
            new TranslationSeedItem("entity.iqcOrder.totalpurchasequantity", "zh-CN", "进货总数", "进货总数"),
            // entity.iqcOrder.totalpurchasequantity
            new TranslationSeedItem("entity.iqcOrder.totalpurchasequantity", "zh-HK", "进货总数", "进货总数"),

            // entity.iqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.iqcOrder.totalsamplequantity", "en-US", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.iqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.iqcOrder.totalsamplequantity", "ja-JP", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.iqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.iqcOrder.totalsamplequantity", "zh-CN", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.iqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.iqcOrder.totalsamplequantity", "zh-HK", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),

            // entity.iqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalqualifiedquantity", "en-US", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.iqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalqualifiedquantity", "ja-JP", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.iqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalqualifiedquantity", "zh-CN", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.iqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalqualifiedquantity", "zh-HK", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),

            // entity.iqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalunqualifiedquantity", "en-US", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.iqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalunqualifiedquantity", "ja-JP", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.iqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalunqualifiedquantity", "zh-CN", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.iqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.iqcOrder.totalunqualifiedquantity", "zh-HK", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),

            // entity.iqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrder.totalinspectionreturnquantity", "en-US", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.iqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrder.totalinspectionreturnquantity", "ja-JP", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.iqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrder.totalinspectionreturnquantity", "zh-CN", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.iqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrder.totalinspectionreturnquantity", "zh-HK", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),

            // entity.iqcOrder.judgestatus
            new TranslationSeedItem("entity.iqcOrder.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrder.judgestatus
            new TranslationSeedItem("entity.iqcOrder.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrder.judgestatus
            new TranslationSeedItem("entity.iqcOrder.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrder.judgestatus
            new TranslationSeedItem("entity.iqcOrder.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.iqcOrder.judgeby
            new TranslationSeedItem("entity.iqcOrder.judgeby", "en-US", "判定人", "判定人（人员代码）"),
            // entity.iqcOrder.judgeby
            new TranslationSeedItem("entity.iqcOrder.judgeby", "ja-JP", "判定人", "判定人（人员代码）"),
            // entity.iqcOrder.judgeby
            new TranslationSeedItem("entity.iqcOrder.judgeby", "zh-CN", "判定人", "判定人（人员代码）"),
            // entity.iqcOrder.judgeby
            new TranslationSeedItem("entity.iqcOrder.judgeby", "zh-HK", "判定人", "判定人（人员代码）"),

            // entity.iqcOrder.judgedate
            new TranslationSeedItem("entity.iqcOrder.judgedate", "en-US", "判定日期", "判定日期"),
            // entity.iqcOrder.judgedate
            new TranslationSeedItem("entity.iqcOrder.judgedate", "ja-JP", "判定日期", "判定日期"),
            // entity.iqcOrder.judgedate
            new TranslationSeedItem("entity.iqcOrder.judgedate", "zh-CN", "判定日期", "判定日期"),
            // entity.iqcOrder.judgedate
            new TranslationSeedItem("entity.iqcOrder.judgedate", "zh-HK", "判定日期", "判定日期"),

            // entity.iqcOrder.judgedescription
            new TranslationSeedItem("entity.iqcOrder.judgedescription", "en-US", "判定说明", "判定说明"),
            // entity.iqcOrder.judgedescription
            new TranslationSeedItem("entity.iqcOrder.judgedescription", "ja-JP", "判定说明", "判定说明"),
            // entity.iqcOrder.judgedescription
            new TranslationSeedItem("entity.iqcOrder.judgedescription", "zh-CN", "判定说明", "判定说明"),
            // entity.iqcOrder.judgedescription
            new TranslationSeedItem("entity.iqcOrder.judgedescription", "zh-HK", "判定说明", "判定说明"),

            // entity.iqcOrder.items
            new TranslationSeedItem("entity.iqcOrder.items", "en-US", "IQC检验单明细列表", "IQC检验单明细列表（主子表关系）"),
            // entity.iqcOrder.items
            new TranslationSeedItem("entity.iqcOrder.items", "ja-JP", "IQC检验单明细列表", "IQC检验单明细列表（主子表关系）"),
            // entity.iqcOrder.items
            new TranslationSeedItem("entity.iqcOrder.items", "zh-CN", "IQC检验单明细列表", "IQC检验单明细列表（主子表关系）"),
            // entity.iqcOrder.items
            new TranslationSeedItem("entity.iqcOrder.items", "zh-HK", "IQC检验单明细列表", "IQC检验单明细列表（主子表关系）"),

            // entity.iqcOrder.changelogs
            new TranslationSeedItem("entity.iqcOrder.changelogs", "en-US", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.iqcOrder.changelogs
            new TranslationSeedItem("entity.iqcOrder.changelogs", "ja-JP", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.iqcOrder.changelogs
            new TranslationSeedItem("entity.iqcOrder.changelogs", "zh-CN", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.iqcOrder.changelogs
            new TranslationSeedItem("entity.iqcOrder.changelogs", "zh-HK", "变更日志列表", "变更日志列表（主子表关系）"),
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
