// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderItemI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIpqcOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktIpqcOrderItem 实体国际化翻译种子（键前缀 entity.ipqcOrderItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIpqcOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIpqcOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ipqcOrderItem 实体翻译...", tenantCode);

        foreach (var item in GetIpqcOrderItemTranslations())
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

        TaktLogger.Information("TaktIpqcOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIpqcOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ipqcOrderItem._self / entity.ipqcOrderItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIpqcOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ipqcOrderItem._self
            new TranslationSeedItem("entity.ipqcOrderItem._self", "en-US", "Ipqc Order Item Information", "实体名称"),
            // entity.ipqcOrderItem._self
            new TranslationSeedItem("entity.ipqcOrderItem._self", "ja-JP", "IPQC制程检验单明细信息", "实体名称"),
            // entity.ipqcOrderItem._self
            new TranslationSeedItem("entity.ipqcOrderItem._self", "zh-CN", "IPQC制程检验单明细信息", "实体名称"),
            // entity.ipqcOrderItem._self
            new TranslationSeedItem("entity.ipqcOrderItem._self", "zh-HK", "IPQC制程检验单明细信息", "实体名称"),

            // entity.ipqcOrderItem.ipqcorderid
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcorderid", "en-US", "IPQC检验单ID", "IPQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.ipqcOrderItem.ipqcorderid
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcorderid", "ja-JP", "IPQC检验单ID", "IPQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.ipqcOrderItem.ipqcorderid
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcorderid", "zh-CN", "IPQC检验单ID", "IPQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.ipqcOrderItem.ipqcorderid
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcorderid", "zh-HK", "IPQC检验单ID", "IPQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.ipqcOrderItem.ipqcordercode
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcordercode", "en-US", "IPQC检验单编码", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcOrderItem.ipqcordercode
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcordercode", "ja-JP", "IPQC检验单编码", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcOrderItem.ipqcordercode
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcordercode", "zh-CN", "IPQC检验单编码", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcOrderItem.ipqcordercode
            new TranslationSeedItem("entity.ipqcOrderItem.ipqcordercode", "zh-HK", "IPQC检验单编码", "IPQC检验单编码（冗余字段，便于查询）"),

            // entity.ipqcOrderItem.linenumber
            new TranslationSeedItem("entity.ipqcOrderItem.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcOrderItem.linenumber
            new TranslationSeedItem("entity.ipqcOrderItem.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcOrderItem.linenumber
            new TranslationSeedItem("entity.ipqcOrderItem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcOrderItem.linenumber
            new TranslationSeedItem("entity.ipqcOrderItem.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.ipqcOrderItem.materialcode
            new TranslationSeedItem("entity.ipqcOrderItem.materialcode", "en-US", "物料编码", "物料编码"),
            // entity.ipqcOrderItem.materialcode
            new TranslationSeedItem("entity.ipqcOrderItem.materialcode", "ja-JP", "物料编码", "物料编码"),
            // entity.ipqcOrderItem.materialcode
            new TranslationSeedItem("entity.ipqcOrderItem.materialcode", "zh-CN", "物料编码", "物料编码"),
            // entity.ipqcOrderItem.materialcode
            new TranslationSeedItem("entity.ipqcOrderItem.materialcode", "zh-HK", "物料编码", "物料编码"),

            // entity.ipqcOrderItem.materialname
            new TranslationSeedItem("entity.ipqcOrderItem.materialname", "en-US", "物料名称", "物料名称"),
            // entity.ipqcOrderItem.materialname
            new TranslationSeedItem("entity.ipqcOrderItem.materialname", "ja-JP", "物料名称", "物料名称"),
            // entity.ipqcOrderItem.materialname
            new TranslationSeedItem("entity.ipqcOrderItem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.ipqcOrderItem.materialname
            new TranslationSeedItem("entity.ipqcOrderItem.materialname", "zh-HK", "物料名称", "物料名称"),

            // entity.ipqcOrderItem.batchno
            new TranslationSeedItem("entity.ipqcOrderItem.batchno", "en-US", "批次号", "批次号"),
            // entity.ipqcOrderItem.batchno
            new TranslationSeedItem("entity.ipqcOrderItem.batchno", "ja-JP", "批次号", "批次号"),
            // entity.ipqcOrderItem.batchno
            new TranslationSeedItem("entity.ipqcOrderItem.batchno", "zh-CN", "批次号", "批次号"),
            // entity.ipqcOrderItem.batchno
            new TranslationSeedItem("entity.ipqcOrderItem.batchno", "zh-HK", "批次号", "批次号"),

            // entity.ipqcOrderItem.productionquantity
            new TranslationSeedItem("entity.ipqcOrderItem.productionquantity", "en-US", "生产数量", "生产数量"),
            // entity.ipqcOrderItem.productionquantity
            new TranslationSeedItem("entity.ipqcOrderItem.productionquantity", "ja-JP", "生产数量", "生产数量"),
            // entity.ipqcOrderItem.productionquantity
            new TranslationSeedItem("entity.ipqcOrderItem.productionquantity", "zh-CN", "生产数量", "生产数量"),
            // entity.ipqcOrderItem.productionquantity
            new TranslationSeedItem("entity.ipqcOrderItem.productionquantity", "zh-HK", "生产数量", "生产数量"),

            // entity.ipqcOrderItem.standardcode
            new TranslationSeedItem("entity.ipqcOrderItem.standardcode", "en-US", "检验标准编码", "检验标准编码"),
            // entity.ipqcOrderItem.standardcode
            new TranslationSeedItem("entity.ipqcOrderItem.standardcode", "ja-JP", "检验标准编码", "检验标准编码"),
            // entity.ipqcOrderItem.standardcode
            new TranslationSeedItem("entity.ipqcOrderItem.standardcode", "zh-CN", "检验标准编码", "检验标准编码"),
            // entity.ipqcOrderItem.standardcode
            new TranslationSeedItem("entity.ipqcOrderItem.standardcode", "zh-HK", "检验标准编码", "检验标准编码"),

            // entity.ipqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.ipqcOrderItem.samplingschemecode", "en-US", "抽样方案编码", "抽样方案编码"),
            // entity.ipqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.ipqcOrderItem.samplingschemecode", "ja-JP", "抽样方案编码", "抽样方案编码"),
            // entity.ipqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.ipqcOrderItem.samplingschemecode", "zh-CN", "抽样方案编码", "抽样方案编码"),
            // entity.ipqcOrderItem.samplingschemecode
            new TranslationSeedItem("entity.ipqcOrderItem.samplingschemecode", "zh-HK", "抽样方案编码", "抽样方案编码"),

            // entity.ipqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionmethod", "en-US", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionmethod", "ja-JP", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionmethod", "zh-CN", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),
            // entity.ipqcOrderItem.inspectionmethod
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionmethod", "zh-HK", "检验方式", "检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）"),

            // entity.ipqcOrderItem.samplequantity
            new TranslationSeedItem("entity.ipqcOrderItem.samplequantity", "en-US", "抽样数量", "抽样数量"),
            // entity.ipqcOrderItem.samplequantity
            new TranslationSeedItem("entity.ipqcOrderItem.samplequantity", "ja-JP", "抽样数量", "抽样数量"),
            // entity.ipqcOrderItem.samplequantity
            new TranslationSeedItem("entity.ipqcOrderItem.samplequantity", "zh-CN", "抽样数量", "抽样数量"),
            // entity.ipqcOrderItem.samplequantity
            new TranslationSeedItem("entity.ipqcOrderItem.samplequantity", "zh-HK", "抽样数量", "抽样数量"),

            // entity.ipqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.qualifiedquantity", "en-US", "合格数量", "合格数量"),
            // entity.ipqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.qualifiedquantity", "ja-JP", "合格数量", "合格数量"),
            // entity.ipqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.qualifiedquantity", "zh-CN", "合格数量", "合格数量"),
            // entity.ipqcOrderItem.qualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.qualifiedquantity", "zh-HK", "合格数量", "合格数量"),

            // entity.ipqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.unqualifiedquantity", "en-US", "不合格数量", "不合格数量"),
            // entity.ipqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.unqualifiedquantity", "ja-JP", "不合格数量", "不合格数量"),
            // entity.ipqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.unqualifiedquantity", "zh-CN", "不合格数量", "不合格数量"),
            // entity.ipqcOrderItem.unqualifiedquantity
            new TranslationSeedItem("entity.ipqcOrderItem.unqualifiedquantity", "zh-HK", "不合格数量", "不合格数量"),

            // entity.ipqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionreturnquantity", "en-US", "验退数量", "验退数量"),
            // entity.ipqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionreturnquantity", "ja-JP", "验退数量", "验退数量"),
            // entity.ipqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionreturnquantity", "zh-CN", "验退数量", "验退数量"),
            // entity.ipqcOrderItem.inspectionreturnquantity
            new TranslationSeedItem("entity.ipqcOrderItem.inspectionreturnquantity", "zh-HK", "验退数量", "验退数量"),

            // entity.ipqcOrderItem.judgestatus
            new TranslationSeedItem("entity.ipqcOrderItem.judgestatus", "en-US", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrderItem.judgestatus
            new TranslationSeedItem("entity.ipqcOrderItem.judgestatus", "ja-JP", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrderItem.judgestatus
            new TranslationSeedItem("entity.ipqcOrderItem.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcOrderItem.judgestatus
            new TranslationSeedItem("entity.ipqcOrderItem.judgestatus", "zh-HK", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),

            // entity.ipqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.ipqcOrderItem.sampleserialno", "en-US", "抽检序列号", "抽检序列号"),
            // entity.ipqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.ipqcOrderItem.sampleserialno", "ja-JP", "抽检序列号", "抽检序列号"),
            // entity.ipqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.ipqcOrderItem.sampleserialno", "zh-CN", "抽检序列号", "抽检序列号"),
            // entity.ipqcOrderItem.sampleserialno
            new TranslationSeedItem("entity.ipqcOrderItem.sampleserialno", "zh-HK", "抽检序列号", "抽检序列号"),

            // entity.ipqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondescription", "en-US", "检验说明", "检验说明"),
            // entity.ipqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondescription", "ja-JP", "检验说明", "检验说明"),
            // entity.ipqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondescription", "zh-CN", "检验说明", "检验说明"),
            // entity.ipqcOrderItem.inspectiondescription
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondescription", "zh-HK", "检验说明", "检验说明"),

            // entity.ipqcOrderItem.inspectorby
            new TranslationSeedItem("entity.ipqcOrderItem.inspectorby", "en-US", "检验员", "检验员（人员代码）"),
            // entity.ipqcOrderItem.inspectorby
            new TranslationSeedItem("entity.ipqcOrderItem.inspectorby", "ja-JP", "检验员", "检验员（人员代码）"),
            // entity.ipqcOrderItem.inspectorby
            new TranslationSeedItem("entity.ipqcOrderItem.inspectorby", "zh-CN", "检验员", "检验员（人员代码）"),
            // entity.ipqcOrderItem.inspectorby
            new TranslationSeedItem("entity.ipqcOrderItem.inspectorby", "zh-HK", "检验员", "检验员（人员代码）"),

            // entity.ipqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondate", "en-US", "检验日期", "检验日期"),
            // entity.ipqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondate", "ja-JP", "检验日期", "检验日期"),
            // entity.ipqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.ipqcOrderItem.inspectiondate
            new TranslationSeedItem("entity.ipqcOrderItem.inspectiondate", "zh-HK", "检验日期", "检验日期"),
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
