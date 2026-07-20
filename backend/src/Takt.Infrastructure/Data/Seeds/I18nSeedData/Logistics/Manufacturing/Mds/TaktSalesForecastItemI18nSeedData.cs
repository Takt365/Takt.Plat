// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastItemI18nSeedData.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesForecastItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds;

/// <summary>
/// TaktSalesForecastItem 实体国际化翻译种子（键前缀 entity.salesforecastitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesForecastItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesForecastItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesforecastitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesForecastItemTranslations())
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

        TaktLogger.Information("TaktSalesForecastItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesForecastItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesforecastitem._self / entity.salesforecastitem.{{field}}；ResourceGroup=Mds；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesForecastItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "en-US", "Sales Forecast Item Information_us", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "ja-JP", "Takt销售预测明细信息_jp", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "zh-CN", "Takt销售预测明细信息", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "zh-HK", "Takt销售预测明细信息_hk", "实体名称"),

            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "en-US", "销售预测ID_us", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "ja-JP", "销售预测ID_jp", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "zh-CN", "销售预测ID", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "zh-HK", "销售预测ID_hk", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "en-US", "销售预测编码_us", "销售预测编码（冗余字段，便于查询）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "ja-JP", "销售预测编码_jp", "销售预测编码（冗余字段，便于查询）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "zh-CN", "销售预测编码", "销售预测编码（冗余字段，便于查询）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "zh-HK", "销售预测编码_hk", "销售预测编码（冗余字段，便于查询）"),

            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.salesforecastitem.materialcode
            new TranslationSeedItem("entity.salesforecastitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesforecastitem.materialcode
            new TranslationSeedItem("entity.salesforecastitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesforecastitem.materialcode
            new TranslationSeedItem("entity.salesforecastitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesforecastitem.materialcode
            new TranslationSeedItem("entity.salesforecastitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.salesforecastitem.materialname
            new TranslationSeedItem("entity.salesforecastitem.materialname", "en-US", "物料名称_us", "物料名称（回填：随物料）"),
            // entity.salesforecastitem.materialname
            new TranslationSeedItem("entity.salesforecastitem.materialname", "ja-JP", "物料名称_jp", "物料名称（回填：随物料）"),
            // entity.salesforecastitem.materialname
            new TranslationSeedItem("entity.salesforecastitem.materialname", "zh-CN", "物料名称", "物料名称（回填：随物料）"),
            // entity.salesforecastitem.materialname
            new TranslationSeedItem("entity.salesforecastitem.materialname", "zh-HK", "物料名称_hk", "物料名称（回填：随物料）"),

            // entity.salesforecastitem.materialspecification
            new TranslationSeedItem("entity.salesforecastitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.salesforecastitem.materialspecification
            new TranslationSeedItem("entity.salesforecastitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.salesforecastitem.materialspecification
            new TranslationSeedItem("entity.salesforecastitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.salesforecastitem.materialspecification
            new TranslationSeedItem("entity.salesforecastitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.salesforecastitem.modelcode
            new TranslationSeedItem("entity.salesforecastitem.modelcode", "en-US", "机种编码_us", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.salesforecastitem.modelcode
            new TranslationSeedItem("entity.salesforecastitem.modelcode", "ja-JP", "机种编码_jp", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.salesforecastitem.modelcode
            new TranslationSeedItem("entity.salesforecastitem.modelcode", "zh-CN", "机种编码", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.salesforecastitem.modelcode
            new TranslationSeedItem("entity.salesforecastitem.modelcode", "zh-HK", "机种编码_hk", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),

            // entity.salesforecastitem.modelname
            new TranslationSeedItem("entity.salesforecastitem.modelname", "en-US", "机种名称_us", "机种名称（冗余字段，便于查询展示）"),
            // entity.salesforecastitem.modelname
            new TranslationSeedItem("entity.salesforecastitem.modelname", "ja-JP", "机种名称_jp", "机种名称（冗余字段，便于查询展示）"),
            // entity.salesforecastitem.modelname
            new TranslationSeedItem("entity.salesforecastitem.modelname", "zh-CN", "机种名称", "机种名称（冗余字段，便于查询展示）"),
            // entity.salesforecastitem.modelname
            new TranslationSeedItem("entity.salesforecastitem.modelname", "zh-HK", "机种名称_hk", "机种名称（冗余字段，便于查询展示）"),

            // entity.salesforecastitem.planunit
            new TranslationSeedItem("entity.salesforecastitem.planunit", "en-US", "计划单位_us", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesforecastitem.planunit
            new TranslationSeedItem("entity.salesforecastitem.planunit", "ja-JP", "计划单位_jp", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesforecastitem.planunit
            new TranslationSeedItem("entity.salesforecastitem.planunit", "zh-CN", "计划单位", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),
            // entity.salesforecastitem.planunit
            new TranslationSeedItem("entity.salesforecastitem.planunit", "zh-HK", "计划单位_hk", "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）"),

            // entity.salesforecastitem.planquantity
            new TranslationSeedItem("entity.salesforecastitem.planquantity", "en-US", "计划数量_us", "计划数量（基本单位数量）"),
            // entity.salesforecastitem.planquantity
            new TranslationSeedItem("entity.salesforecastitem.planquantity", "ja-JP", "计划数量_jp", "计划数量（基本单位数量）"),
            // entity.salesforecastitem.planquantity
            new TranslationSeedItem("entity.salesforecastitem.planquantity", "zh-CN", "计划数量", "计划数量（基本单位数量）"),
            // entity.salesforecastitem.planquantity
            new TranslationSeedItem("entity.salesforecastitem.planquantity", "zh-HK", "计划数量_hk", "计划数量（基本单位数量）"),

            // entity.salesforecastitem.planneddeliverydate
            new TranslationSeedItem("entity.salesforecastitem.planneddeliverydate", "en-US", "计划交货日期_us", "计划交货日期"),
            // entity.salesforecastitem.planneddeliverydate
            new TranslationSeedItem("entity.salesforecastitem.planneddeliverydate", "ja-JP", "计划交货日期_jp", "计划交货日期"),
            // entity.salesforecastitem.planneddeliverydate
            new TranslationSeedItem("entity.salesforecastitem.planneddeliverydate", "zh-CN", "计划交货日期", "计划交货日期"),
            // entity.salesforecastitem.planneddeliverydate
            new TranslationSeedItem("entity.salesforecastitem.planneddeliverydate", "zh-HK", "计划交货日期_hk", "计划交货日期"),

            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "en-US", "已转生产销售数量_us", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "ja-JP", "已转生产销售数量_jp", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "zh-CN", "已转生产销售数量", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "zh-HK", "已转生产销售数量_hk", "已转生产/销售数量（基本单位数量）"),

            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "en-US", "预计单价_us", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "ja-JP", "预计单价_jp", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "zh-CN", "预计单价", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "zh-HK", "预计单价_hk", "预计单价"),

            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mds";
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
