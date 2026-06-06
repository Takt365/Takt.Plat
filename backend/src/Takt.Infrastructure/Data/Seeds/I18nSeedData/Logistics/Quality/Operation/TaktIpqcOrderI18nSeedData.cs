// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIpqcOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktIpqcOrder 实体国际化翻译种子（键前缀 entity.ipqcOrder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIpqcOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIpqcOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ipqcOrder 实体翻译...", tenantCode);

        foreach (var item in GetIpqcOrderTranslations())
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

        TaktLogger.Information("TaktIpqcOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIpqcOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ipqcOrder._self / entity.ipqcOrder.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIpqcOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ipqcOrder._self
            new TranslationSeedItem("entity.ipqcOrder._self", "en-US", "Ipqc Order Information", "实体名称"),
            // entity.ipqcOrder._self
            new TranslationSeedItem("entity.ipqcOrder._self", "ja-JP", "IPQC制程检验单信息", "实体名称"),
            // entity.ipqcOrder._self
            new TranslationSeedItem("entity.ipqcOrder._self", "zh-CN", "IPQC制程检验单信息", "实体名称"),
            // entity.ipqcOrder._self
            new TranslationSeedItem("entity.ipqcOrder._self", "zh-HK", "IPQC制程检验单信息", "实体名称"),

            // entity.ipqcOrder.plantcode
            new TranslationSeedItem("entity.ipqcOrder.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.ipqcOrder.plantcode
            new TranslationSeedItem("entity.ipqcOrder.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.ipqcOrder.plantcode
            new TranslationSeedItem("entity.ipqcOrder.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.ipqcOrder.plantcode
            new TranslationSeedItem("entity.ipqcOrder.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.ipqcOrder.sourcecode
            new TranslationSeedItem("entity.ipqcOrder.sourcecode", "en-US", "来源单号", "来源单号（生产工单编码）"),
            // entity.ipqcOrder.sourcecode
            new TranslationSeedItem("entity.ipqcOrder.sourcecode", "ja-JP", "来源单号", "来源单号（生产工单编码）"),
            // entity.ipqcOrder.sourcecode
            new TranslationSeedItem("entity.ipqcOrder.sourcecode", "zh-CN", "来源单号", "来源单号（生产工单编码）"),
            // entity.ipqcOrder.sourcecode
            new TranslationSeedItem("entity.ipqcOrder.sourcecode", "zh-HK", "来源单号", "来源单号（生产工单编码）"),

            // entity.ipqcOrder.inspectiondate
            new TranslationSeedItem("entity.ipqcOrder.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.ipqcOrder.inspectiondate
            new TranslationSeedItem("entity.ipqcOrder.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.ipqcOrder.inspectiondate
            new TranslationSeedItem("entity.ipqcOrder.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.ipqcOrder.inspectiondate
            new TranslationSeedItem("entity.ipqcOrder.inspectiondate", "zh-HK", "检验日期", "检验日期"),

            // entity.ipqcOrder.code
            new TranslationSeedItem("entity.ipqcOrder.code", "en-US", "IPQC检验单编码", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcOrder.code
            new TranslationSeedItem("entity.ipqcOrder.code", "ja-JP", "IPQC检验单编码", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcOrder.code
            new TranslationSeedItem("entity.ipqcOrder.code", "zh-CN", "IPQC检验单编码", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcOrder.code
            new TranslationSeedItem("entity.ipqcOrder.code", "zh-HK", "IPQC检验单编码", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),

            // entity.ipqcOrder.processcode
            new TranslationSeedItem("entity.ipqcOrder.processcode", "en-US", "工序编码", "工序编码"),
            // entity.ipqcOrder.processcode
            new TranslationSeedItem("entity.ipqcOrder.processcode", "ja-JP", "工序编码", "工序编码"),
            // entity.ipqcOrder.processcode
            new TranslationSeedItem("entity.ipqcOrder.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.ipqcOrder.processcode
            new TranslationSeedItem("entity.ipqcOrder.processcode", "zh-HK", "工序编码", "工序编码"),

            // entity.ipqcOrder.processname
            new TranslationSeedItem("entity.ipqcOrder.processname", "en-US", "工序名称", "工序名称"),
            // entity.ipqcOrder.processname
            new TranslationSeedItem("entity.ipqcOrder.processname", "ja-JP", "工序名称", "工序名称"),
            // entity.ipqcOrder.processname
            new TranslationSeedItem("entity.ipqcOrder.processname", "zh-CN", "工序名称", "工序名称"),
            // entity.ipqcOrder.processname
            new TranslationSeedItem("entity.ipqcOrder.processname", "zh-HK", "工序名称", "工序名称"),

            // entity.ipqcOrder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcOrder.totalproductionquantity", "en-US", "生产总数", "生产总数"),
            // entity.ipqcOrder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcOrder.totalproductionquantity", "ja-JP", "生产总数", "生产总数"),
            // entity.ipqcOrder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcOrder.totalproductionquantity", "zh-CN", "生产总数", "生产总数"),
            // entity.ipqcOrder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcOrder.totalproductionquantity", "zh-HK", "生产总数", "生产总数"),

            // entity.ipqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcOrder.totalsamplequantity", "en-US", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcOrder.totalsamplequantity", "ja-JP", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcOrder.totalsamplequantity", "zh-CN", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcOrder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcOrder.totalsamplequantity", "zh-HK", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),

            // entity.ipqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalqualifiedquantity", "en-US", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalqualifiedquantity", "ja-JP", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalqualifiedquantity", "zh-CN", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcOrder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalqualifiedquantity", "zh-HK", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),

            // entity.ipqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalunqualifiedquantity", "en-US", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalunqualifiedquantity", "ja-JP", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalunqualifiedquantity", "zh-CN", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcOrder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrder.totalunqualifiedquantity", "zh-HK", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),

            // entity.ipqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrder.totalinspectionreturnquantity", "en-US", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrder.totalinspectionreturnquantity", "ja-JP", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrder.totalinspectionreturnquantity", "zh-CN", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcOrder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrder.totalinspectionreturnquantity", "zh-HK", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),

            // entity.ipqcOrder.judgestatus
            new TranslationSeedItem("entity.ipqcOrder.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrder.judgestatus
            new TranslationSeedItem("entity.ipqcOrder.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrder.judgestatus
            new TranslationSeedItem("entity.ipqcOrder.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrder.judgestatus
            new TranslationSeedItem("entity.ipqcOrder.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),

            // entity.ipqcOrder.judgeby
            new TranslationSeedItem("entity.ipqcOrder.judgeby", "en-US", "判定人", "判定人（人员代码）"),
            // entity.ipqcOrder.judgeby
            new TranslationSeedItem("entity.ipqcOrder.judgeby", "ja-JP", "判定人", "判定人（人员代码）"),
            // entity.ipqcOrder.judgeby
            new TranslationSeedItem("entity.ipqcOrder.judgeby", "zh-CN", "判定人", "判定人（人员代码）"),
            // entity.ipqcOrder.judgeby
            new TranslationSeedItem("entity.ipqcOrder.judgeby", "zh-HK", "判定人", "判定人（人员代码）"),

            // entity.ipqcOrder.judgedate
            new TranslationSeedItem("entity.ipqcOrder.judgedate", "en-US", "判定日期", "判定日期"),
            // entity.ipqcOrder.judgedate
            new TranslationSeedItem("entity.ipqcOrder.judgedate", "ja-JP", "判定日期", "判定日期"),
            // entity.ipqcOrder.judgedate
            new TranslationSeedItem("entity.ipqcOrder.judgedate", "zh-CN", "判定日期", "判定日期"),
            // entity.ipqcOrder.judgedate
            new TranslationSeedItem("entity.ipqcOrder.judgedate", "zh-HK", "判定日期", "判定日期"),

            // entity.ipqcOrder.judgedescription
            new TranslationSeedItem("entity.ipqcOrder.judgedescription", "en-US", "判定说明", "判定说明"),
            // entity.ipqcOrder.judgedescription
            new TranslationSeedItem("entity.ipqcOrder.judgedescription", "ja-JP", "判定说明", "判定说明"),
            // entity.ipqcOrder.judgedescription
            new TranslationSeedItem("entity.ipqcOrder.judgedescription", "zh-CN", "判定说明", "判定说明"),
            // entity.ipqcOrder.judgedescription
            new TranslationSeedItem("entity.ipqcOrder.judgedescription", "zh-HK", "判定说明", "判定说明"),

            // entity.ipqcOrder.items
            new TranslationSeedItem("entity.ipqcOrder.items", "en-US", "items", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcOrder.items
            new TranslationSeedItem("entity.ipqcOrder.items", "ja-JP", "items", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcOrder.items
            new TranslationSeedItem("entity.ipqcOrder.items", "zh-CN", "items", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcOrder.items
            new TranslationSeedItem("entity.ipqcOrder.items", "zh-HK", "items", "IPQC检验单明细列表（主子表关系）"),
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
