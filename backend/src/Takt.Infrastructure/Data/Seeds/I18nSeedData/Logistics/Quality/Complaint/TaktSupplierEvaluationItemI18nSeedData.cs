// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplierEvaluationItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSupplierEvaluationItem 实体国际化翻译种子（键前缀 entity.supplierEvaluationItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierEvaluationItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplierEvaluationItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplierEvaluationItem 实体翻译...", tenantCode);

        foreach (var item in GetSupplierEvaluationItemTranslations())
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

        TaktLogger.Information("TaktSupplierEvaluationItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplierEvaluationItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplierEvaluationItem._self / entity.supplierEvaluationItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierEvaluationItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplierEvaluationItem._self
            new TranslationSeedItem("entity.supplierEvaluationItem._self", "en-US", "Supplier Evaluation Item Information", "实体名称"),
            // entity.supplierEvaluationItem._self
            new TranslationSeedItem("entity.supplierEvaluationItem._self", "ja-JP", "供应商评价考核项目明细信息", "实体名称"),
            // entity.supplierEvaluationItem._self
            new TranslationSeedItem("entity.supplierEvaluationItem._self", "zh-CN", "供应商评价考核项目明细信息", "实体名称"),
            // entity.supplierEvaluationItem._self
            new TranslationSeedItem("entity.supplierEvaluationItem._self", "zh-HK", "供应商评价考核项目明细信息", "实体名称"),

            // entity.supplierEvaluationItem.evaluationid
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationid", "en-US", "评价表ID", "评价表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluationItem.evaluationid
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationid", "ja-JP", "评价表ID", "评价表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluationItem.evaluationid
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationid", "zh-CN", "评价表ID", "评价表ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.supplierEvaluationItem.evaluationid
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationid", "zh-HK", "评价表ID", "评价表ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.supplierEvaluationItem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierEvaluationItem.supplierevaluationcode", "en-US", "评价表编号", "评价表编号（冗余字段，便于查询）"),
            // entity.supplierEvaluationItem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierEvaluationItem.supplierevaluationcode", "ja-JP", "评价表编号", "评价表编号（冗余字段，便于查询）"),
            // entity.supplierEvaluationItem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierEvaluationItem.supplierevaluationcode", "zh-CN", "评价表编号", "评价表编号（冗余字段，便于查询）"),
            // entity.supplierEvaluationItem.supplierevaluationcode
            new TranslationSeedItem("entity.supplierEvaluationItem.supplierevaluationcode", "zh-HK", "评价表编号", "评价表编号（冗余字段，便于查询）"),

            // entity.supplierEvaluationItem.linenumber
            new TranslationSeedItem("entity.supplierEvaluationItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.supplierEvaluationItem.linenumber
            new TranslationSeedItem("entity.supplierEvaluationItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.supplierEvaluationItem.linenumber
            new TranslationSeedItem("entity.supplierEvaluationItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.supplierEvaluationItem.linenumber
            new TranslationSeedItem("entity.supplierEvaluationItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.supplierEvaluationItem.categorytype
            new TranslationSeedItem("entity.supplierEvaluationItem.categorytype", "en-US", "评价类别", "评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）"),
            // entity.supplierEvaluationItem.categorytype
            new TranslationSeedItem("entity.supplierEvaluationItem.categorytype", "ja-JP", "评价类别", "评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）"),
            // entity.supplierEvaluationItem.categorytype
            new TranslationSeedItem("entity.supplierEvaluationItem.categorytype", "zh-CN", "评价类别", "评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）"),
            // entity.supplierEvaluationItem.categorytype
            new TranslationSeedItem("entity.supplierEvaluationItem.categorytype", "zh-HK", "评价类别", "评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）"),

            // entity.supplierEvaluationItem.itemname
            new TranslationSeedItem("entity.supplierEvaluationItem.itemname", "en-US", "评价项目", "评价项目名称"),
            // entity.supplierEvaluationItem.itemname
            new TranslationSeedItem("entity.supplierEvaluationItem.itemname", "ja-JP", "评价项目", "评价项目名称"),
            // entity.supplierEvaluationItem.itemname
            new TranslationSeedItem("entity.supplierEvaluationItem.itemname", "zh-CN", "评价项目", "评价项目名称"),
            // entity.supplierEvaluationItem.itemname
            new TranslationSeedItem("entity.supplierEvaluationItem.itemname", "zh-HK", "评价项目", "评价项目名称"),

            // entity.supplierEvaluationItem.itemdescription
            new TranslationSeedItem("entity.supplierEvaluationItem.itemdescription", "en-US", "项目说明", "评价项目说明"),
            // entity.supplierEvaluationItem.itemdescription
            new TranslationSeedItem("entity.supplierEvaluationItem.itemdescription", "ja-JP", "项目说明", "评价项目说明"),
            // entity.supplierEvaluationItem.itemdescription
            new TranslationSeedItem("entity.supplierEvaluationItem.itemdescription", "zh-CN", "项目说明", "评价项目说明"),
            // entity.supplierEvaluationItem.itemdescription
            new TranslationSeedItem("entity.supplierEvaluationItem.itemdescription", "zh-HK", "项目说明", "评价项目说明"),

            // entity.supplierEvaluationItem.weight
            new TranslationSeedItem("entity.supplierEvaluationItem.weight", "en-US", "权重", "权重（%）"),
            // entity.supplierEvaluationItem.weight
            new TranslationSeedItem("entity.supplierEvaluationItem.weight", "ja-JP", "权重", "权重（%）"),
            // entity.supplierEvaluationItem.weight
            new TranslationSeedItem("entity.supplierEvaluationItem.weight", "zh-CN", "权重", "权重（%）"),
            // entity.supplierEvaluationItem.weight
            new TranslationSeedItem("entity.supplierEvaluationItem.weight", "zh-HK", "权重", "权重（%）"),

            // entity.supplierEvaluationItem.scoringstandard
            new TranslationSeedItem("entity.supplierEvaluationItem.scoringstandard", "en-US", "评分标准", "评分标准"),
            // entity.supplierEvaluationItem.scoringstandard
            new TranslationSeedItem("entity.supplierEvaluationItem.scoringstandard", "ja-JP", "评分标准", "评分标准"),
            // entity.supplierEvaluationItem.scoringstandard
            new TranslationSeedItem("entity.supplierEvaluationItem.scoringstandard", "zh-CN", "评分标准", "评分标准"),
            // entity.supplierEvaluationItem.scoringstandard
            new TranslationSeedItem("entity.supplierEvaluationItem.scoringstandard", "zh-HK", "评分标准", "评分标准"),

            // entity.supplierEvaluationItem.score
            new TranslationSeedItem("entity.supplierEvaluationItem.score", "en-US", "评分", "评分（0-100分）"),
            // entity.supplierEvaluationItem.score
            new TranslationSeedItem("entity.supplierEvaluationItem.score", "ja-JP", "评分", "评分（0-100分）"),
            // entity.supplierEvaluationItem.score
            new TranslationSeedItem("entity.supplierEvaluationItem.score", "zh-CN", "评分", "评分（0-100分）"),
            // entity.supplierEvaluationItem.score
            new TranslationSeedItem("entity.supplierEvaluationItem.score", "zh-HK", "评分", "评分（0-100分）"),

            // entity.supplierEvaluationItem.ratinglevel
            new TranslationSeedItem("entity.supplierEvaluationItem.ratinglevel", "en-US", "评级", "评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluationItem.ratinglevel
            new TranslationSeedItem("entity.supplierEvaluationItem.ratinglevel", "ja-JP", "评级", "评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluationItem.ratinglevel
            new TranslationSeedItem("entity.supplierEvaluationItem.ratinglevel", "zh-CN", "评级", "评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),
            // entity.supplierEvaluationItem.ratinglevel
            new TranslationSeedItem("entity.supplierEvaluationItem.ratinglevel", "zh-HK", "评级", "评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）"),

            // entity.supplierEvaluationItem.evaluationcomment
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationcomment", "en-US", "评价说明", "评价说明/事实依据"),
            // entity.supplierEvaluationItem.evaluationcomment
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationcomment", "ja-JP", "评价说明", "评价说明/事实依据"),
            // entity.supplierEvaluationItem.evaluationcomment
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationcomment", "zh-CN", "评价说明", "评价说明/事实依据"),
            // entity.supplierEvaluationItem.evaluationcomment
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluationcomment", "zh-HK", "评价说明", "评价说明/事实依据"),

            // entity.supplierEvaluationItem.existingissues
            new TranslationSeedItem("entity.supplierEvaluationItem.existingissues", "en-US", "存在问题", "存在问题"),
            // entity.supplierEvaluationItem.existingissues
            new TranslationSeedItem("entity.supplierEvaluationItem.existingissues", "ja-JP", "存在问题", "存在问题"),
            // entity.supplierEvaluationItem.existingissues
            new TranslationSeedItem("entity.supplierEvaluationItem.existingissues", "zh-CN", "存在问题", "存在问题"),
            // entity.supplierEvaluationItem.existingissues
            new TranslationSeedItem("entity.supplierEvaluationItem.existingissues", "zh-HK", "存在问题", "存在问题"),

            // entity.supplierEvaluationItem.improvementrequirement
            new TranslationSeedItem("entity.supplierEvaluationItem.improvementrequirement", "en-US", "改进要求", "改进要求"),
            // entity.supplierEvaluationItem.improvementrequirement
            new TranslationSeedItem("entity.supplierEvaluationItem.improvementrequirement", "ja-JP", "改进要求", "改进要求"),
            // entity.supplierEvaluationItem.improvementrequirement
            new TranslationSeedItem("entity.supplierEvaluationItem.improvementrequirement", "zh-CN", "改进要求", "改进要求"),
            // entity.supplierEvaluationItem.improvementrequirement
            new TranslationSeedItem("entity.supplierEvaluationItem.improvementrequirement", "zh-HK", "改进要求", "改进要求"),

            // entity.supplierEvaluationItem.rectificationrequired
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationrequired", "en-US", "整改要求", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierEvaluationItem.rectificationrequired
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationrequired", "ja-JP", "整改要求", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierEvaluationItem.rectificationrequired
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationrequired", "zh-CN", "整改要求", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),
            // entity.supplierEvaluationItem.rectificationrequired
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationrequired", "zh-HK", "整改要求", "整改要求（0=无需整改，1=限期整改，2=重点整改）"),

            // entity.supplierEvaluationItem.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationdeadline", "en-US", "整改期限", "整改期限"),
            // entity.supplierEvaluationItem.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationdeadline", "ja-JP", "整改期限", "整改期限"),
            // entity.supplierEvaluationItem.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationdeadline", "zh-CN", "整改期限", "整改期限"),
            // entity.supplierEvaluationItem.rectificationdeadline
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationdeadline", "zh-HK", "整改期限", "整改期限"),

            // entity.supplierEvaluationItem.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationstatus", "en-US", "整改状态", "整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluationItem.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationstatus", "ja-JP", "整改状态", "整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluationItem.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationstatus", "zh-CN", "整改状态", "整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),
            // entity.supplierEvaluationItem.rectificationstatus
            new TranslationSeedItem("entity.supplierEvaluationItem.rectificationstatus", "zh-HK", "整改状态", "整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）"),

            // entity.supplierEvaluationItem.evaluation
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluation", "en-US", "评价表主表", "评价表主表"),
            // entity.supplierEvaluationItem.evaluation
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluation", "ja-JP", "评价表主表", "评价表主表"),
            // entity.supplierEvaluationItem.evaluation
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluation", "zh-CN", "评价表主表", "评价表主表"),
            // entity.supplierEvaluationItem.evaluation
            new TranslationSeedItem("entity.supplierEvaluationItem.evaluation", "zh-HK", "评价表主表", "评价表主表"),
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
