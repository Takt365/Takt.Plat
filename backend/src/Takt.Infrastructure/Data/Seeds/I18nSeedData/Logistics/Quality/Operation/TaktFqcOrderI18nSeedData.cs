// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktFqcOrder 实体国际化翻译种子（键前缀 entity.fqcorder.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 fqcorder 实体翻译...", tenantCode);

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
    /// I18nKey：entity.fqcorder._self / entity.fqcorder.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetFqcOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.fqcorder._self
            new TranslationSeedItem("entity.fqcorder._self", "en-US", "Fqc Order Information", "实体名称"),
            // entity.fqcorder._self
            new TranslationSeedItem("entity.fqcorder._self", "ja-JP", "FQC出货检验单信息", "实体名称"),
            // entity.fqcorder._self
            new TranslationSeedItem("entity.fqcorder._self", "zh-CN", "FQC出货检验单信息", "实体名称"),
            // entity.fqcorder._self
            new TranslationSeedItem("entity.fqcorder._self", "zh-HK", "FQC出货检验单信息", "实体名称"),

            // entity.fqcorder.plantcode
            new TranslationSeedItem("entity.fqcorder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.fqcorder.plantcode
            new TranslationSeedItem("entity.fqcorder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.fqcorder.plantcode
            new TranslationSeedItem("entity.fqcorder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.fqcorder.plantcode
            new TranslationSeedItem("entity.fqcorder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.fqcorder.sourcecode
            new TranslationSeedItem("entity.fqcorder.sourcecode", "en-US", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcorder.sourcecode
            new TranslationSeedItem("entity.fqcorder.sourcecode", "ja-JP", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcorder.sourcecode
            new TranslationSeedItem("entity.fqcorder.sourcecode", "zh-CN", "来源单号", "来源单号（销售订单编码或发货单编码）"),
            // entity.fqcorder.sourcecode
            new TranslationSeedItem("entity.fqcorder.sourcecode", "zh-HK", "来源单号", "来源单号（销售订单编码或发货单编码）"),

            // entity.fqcorder.inspectiondate
            new TranslationSeedItem("entity.fqcorder.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.fqcorder.inspectiondate
            new TranslationSeedItem("entity.fqcorder.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.fqcorder.inspectiondate
            new TranslationSeedItem("entity.fqcorder.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.fqcorder.inspectiondate
            new TranslationSeedItem("entity.fqcorder.inspectiondate", "zh-HK", "检验日期", "检验日期"),

            // entity.fqcorder.code
            new TranslationSeedItem("entity.fqcorder.code", "en-US", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcorder.code
            new TranslationSeedItem("entity.fqcorder.code", "ja-JP", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcorder.code
            new TranslationSeedItem("entity.fqcorder.code", "zh-CN", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.fqcorder.code
            new TranslationSeedItem("entity.fqcorder.code", "zh-HK", "FQC检验单编码", "FQC检验单编码（唯一索引，根据来源单号自动生成）"),

            // entity.fqcorder.customercode
            new TranslationSeedItem("entity.fqcorder.customercode", "en-US", "客户编码", "客户编码（可选）"),
            // entity.fqcorder.customercode
            new TranslationSeedItem("entity.fqcorder.customercode", "ja-JP", "客户编码", "客户编码（可选）"),
            // entity.fqcorder.customercode
            new TranslationSeedItem("entity.fqcorder.customercode", "zh-CN", "客户编码", "客户编码（可选）"),
            // entity.fqcorder.customercode
            new TranslationSeedItem("entity.fqcorder.customercode", "zh-HK", "客户编码", "客户编码（可选）"),

            // entity.fqcorder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcorder.totalwarehousequantity", "en-US", "总入库数", "总入库数"),
            // entity.fqcorder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcorder.totalwarehousequantity", "ja-JP", "总入库数", "总入库数"),
            // entity.fqcorder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcorder.totalwarehousequantity", "zh-CN", "总入库数", "总入库数"),
            // entity.fqcorder.totalwarehousequantity
            new TranslationSeedItem("entity.fqcorder.totalwarehousequantity", "zh-HK", "总入库数", "总入库数"),

            // entity.fqcorder.totalsamplequantity
            new TranslationSeedItem("entity.fqcorder.totalsamplequantity", "en-US", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcorder.totalsamplequantity
            new TranslationSeedItem("entity.fqcorder.totalsamplequantity", "ja-JP", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcorder.totalsamplequantity
            new TranslationSeedItem("entity.fqcorder.totalsamplequantity", "zh-CN", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.fqcorder.totalsamplequantity
            new TranslationSeedItem("entity.fqcorder.totalsamplequantity", "zh-HK", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),

            // entity.fqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalqualifiedquantity", "en-US", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalqualifiedquantity", "ja-JP", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalqualifiedquantity", "zh-CN", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.fqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalqualifiedquantity", "zh-HK", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),

            // entity.fqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalunqualifiedquantity", "en-US", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalunqualifiedquantity", "ja-JP", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalunqualifiedquantity", "zh-CN", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.fqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.fqcorder.totalunqualifiedquantity", "zh-HK", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),

            // entity.fqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcorder.totalinspectionreturnquantity", "en-US", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcorder.totalinspectionreturnquantity", "ja-JP", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcorder.totalinspectionreturnquantity", "zh-CN", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.fqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.fqcorder.totalinspectionreturnquantity", "zh-HK", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),

            // entity.fqcorder.judgestatus
            new TranslationSeedItem("entity.fqcorder.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcorder.judgestatus
            new TranslationSeedItem("entity.fqcorder.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcorder.judgestatus
            new TranslationSeedItem("entity.fqcorder.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcorder.judgestatus
            new TranslationSeedItem("entity.fqcorder.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.fqcorder.judgeby
            new TranslationSeedItem("entity.fqcorder.judgeby", "en-US", "判定人", "判定人（人员代码）"),
            // entity.fqcorder.judgeby
            new TranslationSeedItem("entity.fqcorder.judgeby", "ja-JP", "判定人", "判定人（人员代码）"),
            // entity.fqcorder.judgeby
            new TranslationSeedItem("entity.fqcorder.judgeby", "zh-CN", "判定人", "判定人（人员代码）"),
            // entity.fqcorder.judgeby
            new TranslationSeedItem("entity.fqcorder.judgeby", "zh-HK", "判定人", "判定人（人员代码）"),

            // entity.fqcorder.judgedate
            new TranslationSeedItem("entity.fqcorder.judgedate", "en-US", "判定日期", "判定日期"),
            // entity.fqcorder.judgedate
            new TranslationSeedItem("entity.fqcorder.judgedate", "ja-JP", "判定日期", "判定日期"),
            // entity.fqcorder.judgedate
            new TranslationSeedItem("entity.fqcorder.judgedate", "zh-CN", "判定日期", "判定日期"),
            // entity.fqcorder.judgedate
            new TranslationSeedItem("entity.fqcorder.judgedate", "zh-HK", "判定日期", "判定日期"),

            // entity.fqcorder.judgedescription
            new TranslationSeedItem("entity.fqcorder.judgedescription", "en-US", "判定说明", "判定说明"),
            // entity.fqcorder.judgedescription
            new TranslationSeedItem("entity.fqcorder.judgedescription", "ja-JP", "判定说明", "判定说明"),
            // entity.fqcorder.judgedescription
            new TranslationSeedItem("entity.fqcorder.judgedescription", "zh-CN", "判定说明", "判定说明"),
            // entity.fqcorder.judgedescription
            new TranslationSeedItem("entity.fqcorder.judgedescription", "zh-HK", "判定说明", "判定说明"),

            // entity.fqcorder.items
            new TranslationSeedItem("entity.fqcorder.items", "en-US", "FQC检验单明细列表", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcorder.items
            new TranslationSeedItem("entity.fqcorder.items", "ja-JP", "FQC检验单明细列表", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcorder.items
            new TranslationSeedItem("entity.fqcorder.items", "zh-CN", "FQC检验单明细列表", "FQC检验单明细列表（主子表关系）"),
            // entity.fqcorder.items
            new TranslationSeedItem("entity.fqcorder.items", "zh-HK", "FQC检验单明细列表", "FQC检验单明细列表（主子表关系）"),

            // entity.fqcorder.changelogs
            new TranslationSeedItem("entity.fqcorder.changelogs", "en-US", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.fqcorder.changelogs
            new TranslationSeedItem("entity.fqcorder.changelogs", "ja-JP", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.fqcorder.changelogs
            new TranslationSeedItem("entity.fqcorder.changelogs", "zh-CN", "变更日志列表", "变更日志列表（主子表关系）"),
            // entity.fqcorder.changelogs
            new TranslationSeedItem("entity.fqcorder.changelogs", "zh-HK", "变更日志列表", "变更日志列表（主子表关系）"),
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
        translation.ResourceGroup = 4;
        translation.ResourceType = 0;
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
