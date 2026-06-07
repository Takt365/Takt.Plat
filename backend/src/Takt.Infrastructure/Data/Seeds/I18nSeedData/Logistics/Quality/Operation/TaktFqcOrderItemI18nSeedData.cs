// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderItemI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFqcOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFqcOrderItem 实体国际化翻译种子（键前缀 entity.fqcOrderItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFqcOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFqcOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 fqcOrderItem 实体翻译...", tenantCode);

        foreach (var item in GetFqcOrderItemTranslations())
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

        TaktLogger.Information("TaktFqcOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFqcOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.fqcOrderItem._self / entity.fqcOrderItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFqcOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.fqcOrderItem._self
            new TranslationSeedItem("entity.fqcOrderItem._self", "en-US", "Fqc Order Item Information", "实体名称"),
            // entity.fqcOrderItem._self
            new TranslationSeedItem("entity.fqcOrderItem._self", "ja-JP", "FQC出货检验单明细信息", "实体名称"),
            // entity.fqcOrderItem._self
            new TranslationSeedItem("entity.fqcOrderItem._self", "zh-CN", "FQC出货检验单明细信息", "实体名称"),
            // entity.fqcOrderItem._self
            new TranslationSeedItem("entity.fqcOrderItem._self", "zh-HK", "FQC出货检验单明细信息", "实体名称"),

            // entity.fqcOrderItem.fqcorderid
            new TranslationSeedItem("entity.fqcOrderItem.fqcorderid", "en-US", "FQC检验单ID", "FQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcOrderItem.fqcorderid
            new TranslationSeedItem("entity.fqcOrderItem.fqcorderid", "ja-JP", "FQC检验单ID", "FQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcOrderItem.fqcorderid
            new TranslationSeedItem("entity.fqcOrderItem.fqcorderid", "zh-CN", "FQC检验单ID", "FQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcOrderItem.fqcorderid
            new TranslationSeedItem("entity.fqcOrderItem.fqcorderid", "zh-HK", "FQC检验单ID", "FQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.fqcOrderItem.fqcordercode
            new TranslationSeedItem("entity.fqcOrderItem.fqcordercode", "en-US", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcOrderItem.fqcordercode
            new TranslationSeedItem("entity.fqcOrderItem.fqcordercode", "ja-JP", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcOrderItem.fqcordercode
            new TranslationSeedItem("entity.fqcOrderItem.fqcordercode", "zh-CN", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcOrderItem.fqcordercode
            new TranslationSeedItem("entity.fqcOrderItem.fqcordercode", "zh-HK", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),

            // entity.fqcOrderItem.linenumber
            new TranslationSeedItem("entity.fqcOrderItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcOrderItem.linenumber
            new TranslationSeedItem("entity.fqcOrderItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcOrderItem.linenumber
            new TranslationSeedItem("entity.fqcOrderItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcOrderItem.linenumber
            new TranslationSeedItem("entity.fqcOrderItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.fqcOrderItem.materialcode
            new TranslationSeedItem("entity.fqcOrderItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.fqcOrderItem.materialcode
            new TranslationSeedItem("entity.fqcOrderItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.fqcOrderItem.materialcode
            new TranslationSeedItem("entity.fqcOrderItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.fqcOrderItem.materialcode
            new TranslationSeedItem("entity.fqcOrderItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.fqcOrderItem.materialname
            new TranslationSeedItem("entity.fqcOrderItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.fqcOrderItem.materialname
            new TranslationSeedItem("entity.fqcOrderItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.fqcOrderItem.materialname
            new TranslationSeedItem("entity.fqcOrderItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.fqcOrderItem.materialname
            new TranslationSeedItem("entity.fqcOrderItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.fqcOrderItem.batchno
            new TranslationSeedItem("entity.fqcOrderItem.batchno", "en-US", "批次号", "批次号"),
            // entity.fqcOrderItem.batchno
            new TranslationSeedItem("entity.fqcOrderItem.batchno", "ja-JP", "批次号", "批次号"),
            // entity.fqcOrderItem.batchno
            new TranslationSeedItem("entity.fqcOrderItem.batchno", "zh-CN", "批次号", "批次号"),
            // entity.fqcOrderItem.batchno
            new TranslationSeedItem("entity.fqcOrderItem.batchno", "zh-HK", "批次号", "批次号"),

            // entity.fqcOrderItem.warehousequantity
            new TranslationSeedItem("entity.fqcOrderItem.warehousequantity", "en-US", "入库数量", "入库数量"),
            // entity.fqcOrderItem.warehousequantity
            new TranslationSeedItem("entity.fqcOrderItem.warehousequantity", "ja-JP", "入库数量", "入库数量"),
            // entity.fqcOrderItem.warehousequantity
            new TranslationSeedItem("entity.fqcOrderItem.warehousequantity", "zh-CN", "入库数量", "入库数量"),
            // entity.fqcOrderItem.warehousequantity
            new TranslationSeedItem("entity.fqcOrderItem.warehousequantity", "zh-HK", "入库数量", "入库数量"),

            // entity.fqcOrderItem.standardcode
            new TranslationSeedItem("entity.fqcOrderItem.standardcode", "en-US", "检验标准编码", "检验标准编码"),
            // entity.fqcOrderItem.standardcode
            new TranslationSeedItem("entity.fqcOrderItem.standardcode", "ja-JP", "检验标准编码", "检验标准编码"),
            // entity.fqcOrderItem.standardcode
            new TranslationSeedItem("entity.fqcOrderItem.standardcode", "zh-CN", "检验标准编码", "检验标准编码"),
            // entity.fqcOrderItem.standardcode
            new TranslationSeedItem("entity.fqcOrderItem.standardcode", "zh-HK", "检验标准编码", "检验标准编码"),

            // entity.fqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.fqcOrderItem.samplingschemecode", "en-US", "抽样方案编码", "抽样方案编码"),
            // entity.fqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.fqcOrderItem.samplingschemecode", "ja-JP", "抽样方案编码", "抽样方案编码"),
            // entity.fqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.fqcOrderItem.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码"),
            // entity.fqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.fqcOrderItem.samplingschemecode", "zh-HK", "抽样方案编码", "抽样方案编码"),

            // entity.fqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.fqcOrderItem.inspectionmethod", "en-US", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.fqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.fqcOrderItem.inspectionmethod", "ja-JP", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.fqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.fqcOrderItem.inspectionmethod", "zh-CN", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.fqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.fqcOrderItem.inspectionmethod", "zh-HK", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),

            // entity.fqcOrderItem.samplequantity
            new TranslationSeedItem("entity.fqcOrderItem.samplequantity", "en-US", "抽样数量", "抽样数量"),
            // entity.fqcOrderItem.samplequantity
            new TranslationSeedItem("entity.fqcOrderItem.samplequantity", "ja-JP", "抽样数量", "抽样数量"),
            // entity.fqcOrderItem.samplequantity
            new TranslationSeedItem("entity.fqcOrderItem.samplequantity", "zh-CN", "抽样数量", "抽样数量"),
            // entity.fqcOrderItem.samplequantity
            new TranslationSeedItem("entity.fqcOrderItem.samplequantity", "zh-HK", "抽样数量", "抽样数量"),

            // entity.fqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.qualifiedquantity", "en-US", "合格数量", "合格数量"),
            // entity.fqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.qualifiedquantity", "ja-JP", "合格数量", "合格数量"),
            // entity.fqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.qualifiedquantity", "zh-CN", "合格数量", "合格数量"),
            // entity.fqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.qualifiedquantity", "zh-HK", "合格数量", "合格数量"),

            // entity.fqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.unqualifiedquantity", "en-US", "不合格数量", "不合格数量"),
            // entity.fqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.unqualifiedquantity", "ja-JP", "不合格数量", "不合格数量"),
            // entity.fqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.unqualifiedquantity", "zh-CN", "不合格数量", "不合格数量"),
            // entity.fqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.fqcOrderItem.unqualifiedquantity", "zh-HK", "不合格数量", "不合格数量"),

            // entity.fqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrderItem.inspectionreturnquantity", "en-US", "验退数量", "验退数量"),
            // entity.fqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrderItem.inspectionreturnquantity", "ja-JP", "验退数量", "验退数量"),
            // entity.fqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrderItem.inspectionreturnquantity", "zh-CN", "验退数量", "验退数量"),
            // entity.fqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.fqcOrderItem.inspectionreturnquantity", "zh-HK", "验退数量", "验退数量"),

            // entity.fqcOrderItem.judgestatus
            new TranslationSeedItem("entity.fqcOrderItem.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrderItem.judgestatus
            new TranslationSeedItem("entity.fqcOrderItem.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrderItem.judgestatus
            new TranslationSeedItem("entity.fqcOrderItem.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),
            // entity.fqcOrderItem.judgestatus
            new TranslationSeedItem("entity.fqcOrderItem.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）"),

            // entity.fqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.fqcOrderItem.sampleserialno", "en-US", "抽检序列号", "抽检序列号"),
            // entity.fqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.fqcOrderItem.sampleserialno", "ja-JP", "抽检序列号", "抽检序列号"),
            // entity.fqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.fqcOrderItem.sampleserialno", "zh-CN", "抽检序列号", "抽检序列号"),
            // entity.fqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.fqcOrderItem.sampleserialno", "zh-HK", "抽检序列号", "抽检序列号"),

            // entity.fqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondescription", "en-US", "检验说明", "检验说明"),
            // entity.fqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondescription", "ja-JP", "检验说明", "检验说明"),
            // entity.fqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondescription", "zh-CN", "检验说明", "检验说明"),
            // entity.fqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondescription", "zh-HK", "检验说明", "检验说明"),

            // entity.fqcOrderItem.inspectorby
            new TranslationSeedItem("entity.fqcOrderItem.inspectorby", "en-US", "检验员", "检验员（人员代码）"),
            // entity.fqcOrderItem.inspectorby
            new TranslationSeedItem("entity.fqcOrderItem.inspectorby", "ja-JP", "检验员", "检验员（人员代码）"),
            // entity.fqcOrderItem.inspectorby
            new TranslationSeedItem("entity.fqcOrderItem.inspectorby", "zh-CN", "检验员", "检验员（人员代码）"),
            // entity.fqcOrderItem.inspectorby
            new TranslationSeedItem("entity.fqcOrderItem.inspectorby", "zh-HK", "检验员", "检验员（人员代码）"),

            // entity.fqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.fqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.fqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.fqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.fqcOrderItem.inspectiondate", "zh-HK", "检验日期", "检验日期"),
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
