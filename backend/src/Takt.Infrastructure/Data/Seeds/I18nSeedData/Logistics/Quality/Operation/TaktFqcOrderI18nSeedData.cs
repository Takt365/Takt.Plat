// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFqcOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFqcOrder 实体国际化翻译种子（键前缀 entity.fqcOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFqcOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFqcOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 fqcOrder 实体翻译...", tenantCode);

        foreach (var item in GetFqcOrderTranslations())
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

        TaktLogger.Information("TaktFqcOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFqcOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.fqcOrder._self / entity.fqcOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFqcOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.fqcOrder._self
            new TranslationSeedItem("entity.fqcOrder._self", "en-US", "Fqc Order Information", "实体名称"),
            // entity.fqcOrder._self
            new TranslationSeedItem("entity.fqcOrder._self", "ja-JP", "FQC出货检验单信息", "实体名称"),
            // entity.fqcOrder._self
            new TranslationSeedItem("entity.fqcOrder._self", "zh-CN", "FQC出货检验单信息", "实体名称"),
            // entity.fqcOrder._self
            new TranslationSeedItem("entity.fqcOrder._self", "zh-HK", "FQC出货检验单信息", "实体名称"),

            // entity.fqcOrder.plantcode
            new TranslationSeedItem("entity.fqcOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.fqcOrder.plantcode
            new TranslationSeedItem("entity.fqcOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.fqcOrder.plantcode
            new TranslationSeedItem("entity.fqcOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.fqcOrder.plantcode
            new TranslationSeedItem("entity.fqcOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.fqcOrder.sourcecode
            new TranslationSeedItem("entity.fqcOrder.sourcecode", "en-US", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcOrder.sourcecode
            new TranslationSeedItem("entity.fqcOrder.sourcecode", "ja-JP", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcOrder.sourcecode
            new TranslationSeedItem("entity.fqcOrder.sourcecode", "zh-CN", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcOrder.sourcecode
            new TranslationSeedItem("entity.fqcOrder.sourcecode", "zh-HK", "来源单号", "来源单号（销售订单编码或发货单编码）"),

            // entity.fqcOrder.inspectiondate
            new TranslationSeedItem("entity.fqcOrder.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.fqcOrder.inspectiondate
            new TranslationSeedItem("entity.fqcOrder.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.fqcOrder.inspectiondate
            new TranslationSeedItem("entity.fqcOrder.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.fqcOrder.inspectiondate
            new TranslationSeedItem("entity.fqcOrder.inspectiondate", "zh-HK", "检验日期", "检验日期"),

            // entity.fqcOrder.code
            new TranslationSeedItem("entity.fqcOrder.code", "en-US", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcOrder.code
            new TranslationSeedItem("entity.fqcOrder.code", "ja-JP", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcOrder.code
            new TranslationSeedItem("entity.fqcOrder.code", "zh-CN", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcOrder.code
            new TranslationSeedItem("entity.fqcOrder.code", "zh-HK", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),

            // entity.fqcOrder.customercode
            new TranslationSeedItem("entity.fqcOrder.customercode", "en-US", "客户编码", "客户编码（可选）"),
            // entity.fqcOrder.customercode
            new TranslationSeedItem("entity.fqcOrder.customercode", "ja-JP", "客户编码", "客户编码（可选）"),
            // entity.fqcOrder.customercode
            new TranslationSeedItem("entity.fqcOrder.customercode", "zh-CN", "客户编码", "客户编码（可选）"),
            // entity.fqcOrder.customercode
            new TranslationSeedItem("entity.fqcOrder.customercode", "zh-HK", "客户编码", "客户编码（可选）"),

            // entity.fqcOrder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcOrder.totalwarehousequantity", "en-US", "总入库数", "总入库数"),
            // entity.fqcOrder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcOrder.totalwarehousequantity", "ja-JP", "总入库数", "总入库数"),
            // entity.fqcOrder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcOrder.totalwarehousequantity", "zh-CN", "总入库数", "总入库数"),
            // entity.fqcOrder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcOrder.totalwarehousequantity", "zh-HK", "总入库数", "总入库数"),

            // entity.fqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.fqcOrder.totalsamplequantity", "en-US", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.fqcOrder.totalsamplequantity", "ja-JP", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.fqcOrder.totalsamplequantity", "zh-CN", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.fqcOrder.totalsamplequantity", "zh-HK", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),

            // entity.fqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalqualifiedquantity", "en-US", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalqualifiedquantity", "ja-JP", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalqualifiedquantity", "zh-CN", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalqualifiedquantity", "zh-HK", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),

            // entity.fqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalunqualifiedquantity", "en-US", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalunqualifiedquantity", "ja-JP", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalunqualifiedquantity", "zh-CN", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcOrder.totalunqualifiedquantity", "zh-HK", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),

            // entity.fqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrder.totalinspectionreturnquantity", "en-US", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrder.totalinspectionreturnquantity", "ja-JP", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrder.totalinspectionreturnquantity", "zh-CN", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrder.totalinspectionreturnquantity", "zh-HK", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),

            // entity.fqcOrder.judgestatus
            new TranslationSeedItem("entity.fqcOrder.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrder.judgestatus
            new TranslationSeedItem("entity.fqcOrder.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrder.judgestatus
            new TranslationSeedItem("entity.fqcOrder.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrder.judgestatus
            new TranslationSeedItem("entity.fqcOrder.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.fqcOrder.judgeby
            new TranslationSeedItem("entity.fqcOrder.judgeby", "en-US", "判定人", "判定人（人员代码）"),
            // entity.fqcOrder.judgeby
            new TranslationSeedItem("entity.fqcOrder.judgeby", "ja-JP", "判定人", "判定人（人员代码）"),
            // entity.fqcOrder.judgeby
            new TranslationSeedItem("entity.fqcOrder.judgeby", "zh-CN", "判定人", "判定人（人员代码）"),
            // entity.fqcOrder.judgeby
            new TranslationSeedItem("entity.fqcOrder.judgeby", "zh-HK", "判定人", "判定人（人员代码）"),

            // entity.fqcOrder.judgedate
            new TranslationSeedItem("entity.fqcOrder.judgedate", "en-US", "判定日期", "判定日期"),
            // entity.fqcOrder.judgedate
            new TranslationSeedItem("entity.fqcOrder.judgedate", "ja-JP", "判定日期", "判定日期"),
            // entity.fqcOrder.judgedate
            new TranslationSeedItem("entity.fqcOrder.judgedate", "zh-CN", "判定日期", "判定日期"),
            // entity.fqcOrder.judgedate
            new TranslationSeedItem("entity.fqcOrder.judgedate", "zh-HK", "判定日期", "判定日期"),

            // entity.fqcOrder.judgedescription
            new TranslationSeedItem("entity.fqcOrder.judgedescription", "en-US", "判定说明", "判定说明"),
            // entity.fqcOrder.judgedescription
            new TranslationSeedItem("entity.fqcOrder.judgedescription", "ja-JP", "判定说明", "判定说明"),
            // entity.fqcOrder.judgedescription
            new TranslationSeedItem("entity.fqcOrder.judgedescription", "zh-CN", "判定说明", "判定说明"),
            // entity.fqcOrder.judgedescription
            new TranslationSeedItem("entity.fqcOrder.judgedescription", "zh-HK", "判定说明", "判定说明"),

            // entity.fqcOrder.items
            new TranslationSeedItem("entity.fqcOrder.items", "en-US", "items", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcOrder.items
            new TranslationSeedItem("entity.fqcOrder.items", "ja-JP", "items", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcOrder.items
            new TranslationSeedItem("entity.fqcOrder.items", "zh-CN", "items", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcOrder.items
            new TranslationSeedItem("entity.fqcOrder.items", "zh-HK", "items", "FQC检验单明细列表（主子表关系）"),
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
