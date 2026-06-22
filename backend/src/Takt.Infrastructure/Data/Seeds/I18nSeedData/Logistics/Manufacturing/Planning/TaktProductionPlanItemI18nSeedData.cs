// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning
// 文件名称：TaktProductionPlanItemI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionPlanItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning;

/// <summary>
/// TaktProductionPlanItem 实体国际化翻译种子（键前缀 entity.productionplanitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionPlanItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionPlanItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionplanitem 实体翻译...", tenantCode);

        foreach (var item in GetProductionPlanItemTranslations())
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

        TaktLogger.Information("TaktProductionPlanItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionPlanItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionplanitem._self / entity.productionplanitem.{{field}}；ResourceGroup=Planning；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionPlanItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "en-US", "Production Plan Item Information_us", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "ja-JP", "Takt生产计划明细信息_jp", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "zh-CN", "Takt生产计划明细信息", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "zh-HK", "Takt生产计划明细信息_hk", "实体名称"),

            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "en-US", "生产计划ID_us", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "ja-JP", "生产计划ID_jp", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "zh-CN", "生产计划ID", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "zh-HK", "生产计划ID_hk", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "en-US", "生产计划编码_us", "生产计划编码（冗余字段，便于查询）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "ja-JP", "生产计划编码_jp", "生产计划编码（冗余字段，便于查询）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "zh-CN", "生产计划编码", "生产计划编码（冗余字段，便于查询）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "zh-HK", "生产计划编码_hk", "生产计划编码（冗余字段，便于查询）"),

            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.productionplanitem.salesplanid
            new TranslationSeedItem("entity.productionplanitem.salesplanid", "en-US", "来源销售计划ID_us", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesplanid
            new TranslationSeedItem("entity.productionplanitem.salesplanid", "ja-JP", "来源销售计划ID_jp", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesplanid
            new TranslationSeedItem("entity.productionplanitem.salesplanid", "zh-CN", "来源销售计划ID", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesplanid
            new TranslationSeedItem("entity.productionplanitem.salesplanid", "zh-HK", "来源销售计划ID_hk", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplanitem.salesplancode
            new TranslationSeedItem("entity.productionplanitem.salesplancode", "en-US", "来源销售计划编码_us", "来源销售计划编码"),
            // entity.productionplanitem.salesplancode
            new TranslationSeedItem("entity.productionplanitem.salesplancode", "ja-JP", "来源销售计划编码_jp", "来源销售计划编码"),
            // entity.productionplanitem.salesplancode
            new TranslationSeedItem("entity.productionplanitem.salesplancode", "zh-CN", "来源销售计划编码", "来源销售计划编码"),
            // entity.productionplanitem.salesplancode
            new TranslationSeedItem("entity.productionplanitem.salesplancode", "zh-HK", "来源销售计划编码_hk", "来源销售计划编码"),

            // entity.productionplanitem.salesplanlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesplanlinenumber", "en-US", "来源销售计划行号_us", "来源销售计划行号"),
            // entity.productionplanitem.salesplanlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesplanlinenumber", "ja-JP", "来源销售计划行号_jp", "来源销售计划行号"),
            // entity.productionplanitem.salesplanlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesplanlinenumber", "zh-CN", "来源销售计划行号", "来源销售计划行号"),
            // entity.productionplanitem.salesplanlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesplanlinenumber", "zh-HK", "来源销售计划行号_hk", "来源销售计划行号"),

            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "en-US", "物料编码_us", "物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "zh-CN", "物料编码", "物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（计划生产物料，关联 TaktMaterialPlant.MaterialCode）"),

            // entity.productionplanitem.materialname
            new TranslationSeedItem("entity.productionplanitem.materialname", "en-US", "物料名称_us", "物料名称"),
            // entity.productionplanitem.materialname
            new TranslationSeedItem("entity.productionplanitem.materialname", "ja-JP", "物料名称_jp", "物料名称"),
            // entity.productionplanitem.materialname
            new TranslationSeedItem("entity.productionplanitem.materialname", "zh-CN", "物料名称", "物料名称"),
            // entity.productionplanitem.materialname
            new TranslationSeedItem("entity.productionplanitem.materialname", "zh-HK", "物料名称_hk", "物料名称"),

            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "en-US", "计划单位_us", "计划单位"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "ja-JP", "计划单位_jp", "计划单位"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "zh-CN", "计划单位", "计划单位"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "zh-HK", "计划单位_hk", "计划单位"),

            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "en-US", "计划数量_us", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "ja-JP", "计划数量_jp", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "zh-CN", "计划数量", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "zh-HK", "计划数量_hk", "计划数量（基本单位数量）"),

            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "en-US", "计划开工日期_us", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "ja-JP", "计划开工日期_jp", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "zh-CN", "计划开工日期", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "zh-HK", "计划开工日期_hk", "计划开工日期"),

            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "en-US", "计划完工日期_us", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "ja-JP", "计划完工日期_jp", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "zh-CN", "计划完工日期", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "zh-HK", "计划完工日期_hk", "计划完工日期"),

            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "en-US", "已转工单采购数量_us", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "ja-JP", "已转工单采购数量_jp", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "zh-CN", "已转工单采购数量", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "zh-HK", "已转工单采购数量_hk", "已转工单/采购数量（基本单位数量）"),

            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "en-US", "预计单位成本_us", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "ja-JP", "预计单位成本_jp", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "zh-CN", "预计单位成本", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "zh-HK", "预计单位成本_hk", "预计单位成本"),

            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),
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
        translation.ResourceGroup = "Planning";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
