// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerComplaintItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaintItem 实体国际化翻译种子（键前缀 entity.customerComplaintItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerComplaintItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerComplaintItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerComplaintItem 实体翻译...", tenantCode);

        foreach (var item in GetCustomerComplaintItemTranslations())
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

        TaktLogger.Information("TaktCustomerComplaintItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerComplaintItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerComplaintItem._self / entity.customerComplaintItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerComplaintItem._self
            new TranslationSeedItem("entity.customerComplaintItem._self", "en-US", "Customer Complaint Item Information", "实体名称"),
            // entity.customerComplaintItem._self
            new TranslationSeedItem("entity.customerComplaintItem._self", "ja-JP", "客诉明细信息", "实体名称"),
            // entity.customerComplaintItem._self
            new TranslationSeedItem("entity.customerComplaintItem._self", "zh-CN", "客诉明细信息", "实体名称"),
            // entity.customerComplaintItem._self
            new TranslationSeedItem("entity.customerComplaintItem._self", "zh-HK", "客诉明细信息", "实体名称"),

            // entity.customerComplaintItem.complaintid
            new TranslationSeedItem("entity.customerComplaintItem.complaintid", "en-US", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintItem.complaintid
            new TranslationSeedItem("entity.customerComplaintItem.complaintid", "ja-JP", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintItem.complaintid
            new TranslationSeedItem("entity.customerComplaintItem.complaintid", "zh-CN", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaintItem.complaintid
            new TranslationSeedItem("entity.customerComplaintItem.complaintid", "zh-HK", "客诉ID", "客诉ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaintItem.customercomplaintcode
            new TranslationSeedItem("entity.customerComplaintItem.customercomplaintcode", "en-US", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintItem.customercomplaintcode
            new TranslationSeedItem("entity.customerComplaintItem.customercomplaintcode", "ja-JP", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintItem.customercomplaintcode
            new TranslationSeedItem("entity.customerComplaintItem.customercomplaintcode", "zh-CN", "客诉单号", "客诉单号（冗余字段，便于查询）"),
            // entity.customerComplaintItem.customercomplaintcode
            new TranslationSeedItem("entity.customerComplaintItem.customercomplaintcode", "zh-HK", "客诉单号", "客诉单号（冗余字段，便于查询）"),

            // entity.customerComplaintItem.linenumber
            new TranslationSeedItem("entity.customerComplaintItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerComplaintItem.linenumber
            new TranslationSeedItem("entity.customerComplaintItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerComplaintItem.linenumber
            new TranslationSeedItem("entity.customerComplaintItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customerComplaintItem.linenumber
            new TranslationSeedItem("entity.customerComplaintItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.customerComplaintItem.productcode
            new TranslationSeedItem("entity.customerComplaintItem.productcode", "en-US", "产品编码", "产品编码"),
            // entity.customerComplaintItem.productcode
            new TranslationSeedItem("entity.customerComplaintItem.productcode", "ja-JP", "产品编码", "产品编码"),
            // entity.customerComplaintItem.productcode
            new TranslationSeedItem("entity.customerComplaintItem.productcode", "zh-CN", "产品编码", "产品编码"),
            // entity.customerComplaintItem.productcode
            new TranslationSeedItem("entity.customerComplaintItem.productcode", "zh-HK", "产品编码", "产品编码"),

            // entity.customerComplaintItem.productname
            new TranslationSeedItem("entity.customerComplaintItem.productname", "en-US", "产品名称", "产品名称"),
            // entity.customerComplaintItem.productname
            new TranslationSeedItem("entity.customerComplaintItem.productname", "ja-JP", "产品名称", "产品名称"),
            // entity.customerComplaintItem.productname
            new TranslationSeedItem("entity.customerComplaintItem.productname", "zh-CN", "产品名称", "产品名称"),
            // entity.customerComplaintItem.productname
            new TranslationSeedItem("entity.customerComplaintItem.productname", "zh-HK", "产品名称", "产品名称"),

            // entity.customerComplaintItem.batchno
            new TranslationSeedItem("entity.customerComplaintItem.batchno", "en-US", "批次号", "批次号"),
            // entity.customerComplaintItem.batchno
            new TranslationSeedItem("entity.customerComplaintItem.batchno", "ja-JP", "批次号", "批次号"),
            // entity.customerComplaintItem.batchno
            new TranslationSeedItem("entity.customerComplaintItem.batchno", "zh-CN", "批次号", "批次号"),
            // entity.customerComplaintItem.batchno
            new TranslationSeedItem("entity.customerComplaintItem.batchno", "zh-HK", "批次号", "批次号"),

            // entity.customerComplaintItem.itemtype
            new TranslationSeedItem("entity.customerComplaintItem.itemtype", "en-US", "不良项目类型", "不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）"),
            // entity.customerComplaintItem.itemtype
            new TranslationSeedItem("entity.customerComplaintItem.itemtype", "ja-JP", "不良项目类型", "不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）"),
            // entity.customerComplaintItem.itemtype
            new TranslationSeedItem("entity.customerComplaintItem.itemtype", "zh-CN", "不良项目类型", "不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）"),
            // entity.customerComplaintItem.itemtype
            new TranslationSeedItem("entity.customerComplaintItem.itemtype", "zh-HK", "不良项目类型", "不良项目类型（0=外观，1=尺寸，2=性能，3=功能，4=包装，5=其他）"),

            // entity.customerComplaintItem.defectdescription
            new TranslationSeedItem("entity.customerComplaintItem.defectdescription", "en-US", "不良现象描述", "不良现象描述"),
            // entity.customerComplaintItem.defectdescription
            new TranslationSeedItem("entity.customerComplaintItem.defectdescription", "ja-JP", "不良现象描述", "不良现象描述"),
            // entity.customerComplaintItem.defectdescription
            new TranslationSeedItem("entity.customerComplaintItem.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.customerComplaintItem.defectdescription
            new TranslationSeedItem("entity.customerComplaintItem.defectdescription", "zh-HK", "不良现象描述", "不良现象描述"),

            // entity.customerComplaintItem.defectlevel
            new TranslationSeedItem("entity.customerComplaintItem.defectlevel", "en-US", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.customerComplaintItem.defectlevel
            new TranslationSeedItem("entity.customerComplaintItem.defectlevel", "ja-JP", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.customerComplaintItem.defectlevel
            new TranslationSeedItem("entity.customerComplaintItem.defectlevel", "zh-CN", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),
            // entity.customerComplaintItem.defectlevel
            new TranslationSeedItem("entity.customerComplaintItem.defectlevel", "zh-HK", "缺点等级", "缺点等级（CR=严重，MA=主要，MI=次要）"),

            // entity.customerComplaintItem.defectquantity
            new TranslationSeedItem("entity.customerComplaintItem.defectquantity", "en-US", "不良数量", "不良数量"),
            // entity.customerComplaintItem.defectquantity
            new TranslationSeedItem("entity.customerComplaintItem.defectquantity", "ja-JP", "不良数量", "不良数量"),
            // entity.customerComplaintItem.defectquantity
            new TranslationSeedItem("entity.customerComplaintItem.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.customerComplaintItem.defectquantity
            new TranslationSeedItem("entity.customerComplaintItem.defectquantity", "zh-HK", "不良数量", "不良数量"),

            // entity.customerComplaintItem.defectrate
            new TranslationSeedItem("entity.customerComplaintItem.defectrate", "en-US", "不良率", "不良率（%）"),
            // entity.customerComplaintItem.defectrate
            new TranslationSeedItem("entity.customerComplaintItem.defectrate", "ja-JP", "不良率", "不良率（%）"),
            // entity.customerComplaintItem.defectrate
            new TranslationSeedItem("entity.customerComplaintItem.defectrate", "zh-CN", "不良率", "不良率（%）"),
            // entity.customerComplaintItem.defectrate
            new TranslationSeedItem("entity.customerComplaintItem.defectrate", "zh-HK", "不良率", "不良率（%）"),

            // entity.customerComplaintItem.causeanalysis
            new TranslationSeedItem("entity.customerComplaintItem.causeanalysis", "en-US", "原因分析", "原因分析"),
            // entity.customerComplaintItem.causeanalysis
            new TranslationSeedItem("entity.customerComplaintItem.causeanalysis", "ja-JP", "原因分析", "原因分析"),
            // entity.customerComplaintItem.causeanalysis
            new TranslationSeedItem("entity.customerComplaintItem.causeanalysis", "zh-CN", "原因分析", "原因分析"),
            // entity.customerComplaintItem.causeanalysis
            new TranslationSeedItem("entity.customerComplaintItem.causeanalysis", "zh-HK", "原因分析", "原因分析"),

            // entity.customerComplaintItem.improvementaction
            new TranslationSeedItem("entity.customerComplaintItem.improvementaction", "en-US", "改善对策", "改善对策"),
            // entity.customerComplaintItem.improvementaction
            new TranslationSeedItem("entity.customerComplaintItem.improvementaction", "ja-JP", "改善对策", "改善对策"),
            // entity.customerComplaintItem.improvementaction
            new TranslationSeedItem("entity.customerComplaintItem.improvementaction", "zh-CN", "改善对策", "改善对策"),
            // entity.customerComplaintItem.improvementaction
            new TranslationSeedItem("entity.customerComplaintItem.improvementaction", "zh-HK", "改善对策", "改善对策"),

            // entity.customerComplaintItem.improvementresponsible
            new TranslationSeedItem("entity.customerComplaintItem.improvementresponsible", "en-US", "改善责任人", "改善责任人"),
            // entity.customerComplaintItem.improvementresponsible
            new TranslationSeedItem("entity.customerComplaintItem.improvementresponsible", "ja-JP", "改善责任人", "改善责任人"),
            // entity.customerComplaintItem.improvementresponsible
            new TranslationSeedItem("entity.customerComplaintItem.improvementresponsible", "zh-CN", "改善责任人", "改善责任人"),
            // entity.customerComplaintItem.improvementresponsible
            new TranslationSeedItem("entity.customerComplaintItem.improvementresponsible", "zh-HK", "改善责任人", "改善责任人"),

            // entity.customerComplaintItem.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.plannedcompletiondate", "en-US", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintItem.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.plannedcompletiondate", "ja-JP", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintItem.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.plannedcompletiondate", "zh-CN", "计划完成日期", "计划完成日期"),
            // entity.customerComplaintItem.plannedcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.plannedcompletiondate", "zh-HK", "计划完成日期", "计划完成日期"),

            // entity.customerComplaintItem.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.actualcompletiondate", "en-US", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintItem.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.actualcompletiondate", "ja-JP", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintItem.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.actualcompletiondate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.customerComplaintItem.actualcompletiondate
            new TranslationSeedItem("entity.customerComplaintItem.actualcompletiondate", "zh-HK", "实际完成日期", "实际完成日期"),

            // entity.customerComplaintItem.improvementstatus
            new TranslationSeedItem("entity.customerComplaintItem.improvementstatus", "en-US", "改善状态", "改善状态（0=待改善，1=改善中，2=已完成，3=已验证）"),
            // entity.customerComplaintItem.improvementstatus
            new TranslationSeedItem("entity.customerComplaintItem.improvementstatus", "ja-JP", "改善状态", "改善状态（0=待改善，1=改善中，2=已完成，3=已验证）"),
            // entity.customerComplaintItem.improvementstatus
            new TranslationSeedItem("entity.customerComplaintItem.improvementstatus", "zh-CN", "改善状态", "改善状态（0=待改善，1=改善中，2=已完成，3=已验证）"),
            // entity.customerComplaintItem.improvementstatus
            new TranslationSeedItem("entity.customerComplaintItem.improvementstatus", "zh-HK", "改善状态", "改善状态（0=待改善，1=改善中，2=已完成，3=已验证）"),

            // entity.customerComplaintItem.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintItem.attachmentpaths", "en-US", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.customerComplaintItem.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintItem.attachmentpaths", "ja-JP", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.customerComplaintItem.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintItem.attachmentpaths", "zh-CN", "附件路径", "附件路径（多个附件用逗号分隔）"),
            // entity.customerComplaintItem.attachmentpaths
            new TranslationSeedItem("entity.customerComplaintItem.attachmentpaths", "zh-HK", "附件路径", "附件路径（多个附件用逗号分隔）"),
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
