// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIqcOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktIqcOrderItem 实体国际化翻译种子（键前缀 entity.iqcOrderItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIqcOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIqcOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 iqcOrderItem 实体翻译...", tenantCode);

        foreach (var item in GetIqcOrderItemTranslations())
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

        TaktLogger.Information("TaktIqcOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIqcOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.iqcOrderItem._self / entity.iqcOrderItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIqcOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.iqcOrderItem._self
            new TranslationSeedItem("entity.iqcOrderItem._self", "en-US", "Iqc Order Item Information", "实体名称"),
            // entity.iqcOrderItem._self
            new TranslationSeedItem("entity.iqcOrderItem._self", "ja-JP", "IQC进货检验单明细信息", "实体名称"),
            // entity.iqcOrderItem._self
            new TranslationSeedItem("entity.iqcOrderItem._self", "zh-CN", "IQC进货检验单明细信息", "实体名称"),
            // entity.iqcOrderItem._self
            new TranslationSeedItem("entity.iqcOrderItem._self", "zh-HK", "IQC进货检验单明细信息", "实体名称"),

            // entity.iqcOrderItem.iqcorderid
            new TranslationSeedItem("entity.iqcOrderItem.iqcorderid", "en-US", "IQC检验单ID", "IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcOrderItem.iqcorderid
            new TranslationSeedItem("entity.iqcOrderItem.iqcorderid", "ja-JP", "IQC检验单ID", "IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcOrderItem.iqcorderid
            new TranslationSeedItem("entity.iqcOrderItem.iqcorderid", "zh-CN", "IQC检验单ID", "IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcOrderItem.iqcorderid
            new TranslationSeedItem("entity.iqcOrderItem.iqcorderid", "zh-HK", "IQC检验单ID", "IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.iqcOrderItem.iqcordercode
            new TranslationSeedItem("entity.iqcOrderItem.iqcordercode", "en-US", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcOrderItem.iqcordercode
            new TranslationSeedItem("entity.iqcOrderItem.iqcordercode", "ja-JP", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcOrderItem.iqcordercode
            new TranslationSeedItem("entity.iqcOrderItem.iqcordercode", "zh-CN", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcOrderItem.iqcordercode
            new TranslationSeedItem("entity.iqcOrderItem.iqcordercode", "zh-HK", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),

            // entity.iqcOrderItem.linenumber
            new TranslationSeedItem("entity.iqcOrderItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcOrderItem.linenumber
            new TranslationSeedItem("entity.iqcOrderItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcOrderItem.linenumber
            new TranslationSeedItem("entity.iqcOrderItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcOrderItem.linenumber
            new TranslationSeedItem("entity.iqcOrderItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.iqcOrderItem.materialcode
            new TranslationSeedItem("entity.iqcOrderItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.iqcOrderItem.materialcode
            new TranslationSeedItem("entity.iqcOrderItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.iqcOrderItem.materialcode
            new TranslationSeedItem("entity.iqcOrderItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.iqcOrderItem.materialcode
            new TranslationSeedItem("entity.iqcOrderItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.iqcOrderItem.materialname
            new TranslationSeedItem("entity.iqcOrderItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.iqcOrderItem.materialname
            new TranslationSeedItem("entity.iqcOrderItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.iqcOrderItem.materialname
            new TranslationSeedItem("entity.iqcOrderItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.iqcOrderItem.materialname
            new TranslationSeedItem("entity.iqcOrderItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.iqcOrderItem.batchno
            new TranslationSeedItem("entity.iqcOrderItem.batchno", "en-US", "批次号", "批次号"),
            // entity.iqcOrderItem.batchno
            new TranslationSeedItem("entity.iqcOrderItem.batchno", "ja-JP", "批次号", "批次号"),
            // entity.iqcOrderItem.batchno
            new TranslationSeedItem("entity.iqcOrderItem.batchno", "zh-CN", "批次号", "批次号"),
            // entity.iqcOrderItem.batchno
            new TranslationSeedItem("entity.iqcOrderItem.batchno", "zh-HK", "批次号", "批次号"),

            // entity.iqcOrderItem.purchasequantity
            new TranslationSeedItem("entity.iqcOrderItem.purchasequantity", "en-US", "进货数量", "进货数量"),
            // entity.iqcOrderItem.purchasequantity
            new TranslationSeedItem("entity.iqcOrderItem.purchasequantity", "ja-JP", "进货数量", "进货数量"),
            // entity.iqcOrderItem.purchasequantity
            new TranslationSeedItem("entity.iqcOrderItem.purchasequantity", "zh-CN", "进货数量", "进货数量"),
            // entity.iqcOrderItem.purchasequantity
            new TranslationSeedItem("entity.iqcOrderItem.purchasequantity", "zh-HK", "进货数量", "进货数量"),

            // entity.iqcOrderItem.standardcode
            new TranslationSeedItem("entity.iqcOrderItem.standardcode", "en-US", "检验标准编码", "检验标准编码"),
            // entity.iqcOrderItem.standardcode
            new TranslationSeedItem("entity.iqcOrderItem.standardcode", "ja-JP", "检验标准编码", "检验标准编码"),
            // entity.iqcOrderItem.standardcode
            new TranslationSeedItem("entity.iqcOrderItem.standardcode", "zh-CN", "检验标准编码", "检验标准编码"),
            // entity.iqcOrderItem.standardcode
            new TranslationSeedItem("entity.iqcOrderItem.standardcode", "zh-HK", "检验标准编码", "检验标准编码"),

            // entity.iqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.iqcOrderItem.samplingschemecode", "en-US", "抽样方案编码", "抽样方案编码"),
            // entity.iqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.iqcOrderItem.samplingschemecode", "ja-JP", "抽样方案编码", "抽样方案编码"),
            // entity.iqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.iqcOrderItem.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码"),
            // entity.iqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.iqcOrderItem.samplingschemecode", "zh-HK", "抽样方案编码", "抽样方案编码"),

            // entity.iqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.iqcOrderItem.inspectionmethod", "en-US", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.iqcOrderItem.inspectionmethod", "ja-JP", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.iqcOrderItem.inspectionmethod", "zh-CN", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.iqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.iqcOrderItem.inspectionmethod", "zh-HK", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),

            // entity.iqcOrderItem.samplequantity
            new TranslationSeedItem("entity.iqcOrderItem.samplequantity", "en-US", "抽样数量", "抽样数量"),
            // entity.iqcOrderItem.samplequantity
            new TranslationSeedItem("entity.iqcOrderItem.samplequantity", "ja-JP", "抽样数量", "抽样数量"),
            // entity.iqcOrderItem.samplequantity
            new TranslationSeedItem("entity.iqcOrderItem.samplequantity", "zh-CN", "抽样数量", "抽样数量"),
            // entity.iqcOrderItem.samplequantity
            new TranslationSeedItem("entity.iqcOrderItem.samplequantity", "zh-HK", "抽样数量", "抽样数量"),

            // entity.iqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.qualifiedquantity", "en-US", "合格数量", "合格数量"),
            // entity.iqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.qualifiedquantity", "ja-JP", "合格数量", "合格数量"),
            // entity.iqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.qualifiedquantity", "zh-CN", "合格数量", "合格数量"),
            // entity.iqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.qualifiedquantity", "zh-HK", "合格数量", "合格数量"),

            // entity.iqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.unqualifiedquantity", "en-US", "不合格数量", "不合格数量"),
            // entity.iqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.unqualifiedquantity", "ja-JP", "不合格数量", "不合格数量"),
            // entity.iqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.unqualifiedquantity", "zh-CN", "不合格数量", "不合格数量"),
            // entity.iqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.iqcOrderItem.unqualifiedquantity", "zh-HK", "不合格数量", "不合格数量"),

            // entity.iqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrderItem.inspectionreturnquantity", "en-US", "验退数量", "验退数量"),
            // entity.iqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrderItem.inspectionreturnquantity", "ja-JP", "验退数量", "验退数量"),
            // entity.iqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrderItem.inspectionreturnquantity", "zh-CN", "验退数量", "验退数量"),
            // entity.iqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.iqcOrderItem.inspectionreturnquantity", "zh-HK", "验退数量", "验退数量"),

            // entity.iqcOrderItem.judgestatus
            new TranslationSeedItem("entity.iqcOrderItem.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrderItem.judgestatus
            new TranslationSeedItem("entity.iqcOrderItem.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrderItem.judgestatus
            new TranslationSeedItem("entity.iqcOrderItem.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.iqcOrderItem.judgestatus
            new TranslationSeedItem("entity.iqcOrderItem.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.iqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.iqcOrderItem.sampleserialno", "en-US", "抽检序列号", "抽检序列号"),
            // entity.iqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.iqcOrderItem.sampleserialno", "ja-JP", "抽检序列号", "抽检序列号"),
            // entity.iqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.iqcOrderItem.sampleserialno", "zh-CN", "抽检序列号", "抽检序列号"),
            // entity.iqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.iqcOrderItem.sampleserialno", "zh-HK", "抽检序列号", "抽检序列号"),

            // entity.iqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondescription", "en-US", "检验说明", "检验说明"),
            // entity.iqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondescription", "ja-JP", "检验说明", "检验说明"),
            // entity.iqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondescription", "zh-CN", "检验说明", "检验说明"),
            // entity.iqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondescription", "zh-HK", "检验说明", "检验说明"),

            // entity.iqcOrderItem.inspectorby
            new TranslationSeedItem("entity.iqcOrderItem.inspectorby", "en-US", "检验员", "检验员（人员代码）"),
            // entity.iqcOrderItem.inspectorby
            new TranslationSeedItem("entity.iqcOrderItem.inspectorby", "ja-JP", "检验员", "检验员（人员代码）"),
            // entity.iqcOrderItem.inspectorby
            new TranslationSeedItem("entity.iqcOrderItem.inspectorby", "zh-CN", "检验员", "检验员（人员代码）"),
            // entity.iqcOrderItem.inspectorby
            new TranslationSeedItem("entity.iqcOrderItem.inspectorby", "zh-HK", "检验员", "检验员（人员代码）"),

            // entity.iqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.iqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.iqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.iqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.iqcOrderItem.inspectiondate", "zh-HK", "检验日期", "检验日期"),

            // entity.iqcOrderItem.order
            new TranslationSeedItem("entity.iqcOrderItem.order", "en-US", "IQC检验单", "IQC检验单（主表）"),
            // entity.iqcOrderItem.order
            new TranslationSeedItem("entity.iqcOrderItem.order", "ja-JP", "IQC检验单", "IQC检验单（主表）"),
            // entity.iqcOrderItem.order
            new TranslationSeedItem("entity.iqcOrderItem.order", "zh-CN", "IQC检验单", "IQC检验单（主表）"),
            // entity.iqcOrderItem.order
            new TranslationSeedItem("entity.iqcOrderItem.order", "zh-HK", "IQC检验单", "IQC检验单（主表）"),

            // entity.iqcOrderItem.defecthandlings
            new TranslationSeedItem("entity.iqcOrderItem.defecthandlings", "en-US", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
            // entity.iqcOrderItem.defecthandlings
            new TranslationSeedItem("entity.iqcOrderItem.defecthandlings", "ja-JP", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
            // entity.iqcOrderItem.defecthandlings
            new TranslationSeedItem("entity.iqcOrderItem.defecthandlings", "zh-CN", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
            // entity.iqcOrderItem.defecthandlings
            new TranslationSeedItem("entity.iqcOrderItem.defecthandlings", "zh-HK", "不良处理记录列表", "不良处理记录列表（主子表关系）"),
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
