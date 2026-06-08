// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBillOfMaterialItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktBillOfMaterialItem 实体国际化翻译种子（键前缀 entity.billOfMaterialItem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBillOfMaterialItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBillOfMaterialItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billOfMaterialItem 实体翻译...", tenantCode);

        foreach (var item in GetBillOfMaterialItemTranslations())
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

        TaktLogger.Information("TaktBillOfMaterialItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBillOfMaterialItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.billOfMaterialItem._self / entity.billOfMaterialItem.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billOfMaterialItem._self
            new TranslationSeedItem("entity.billOfMaterialItem._self", "en-US", "Bill Of Material Item Information", "实体名称"),
            // entity.billOfMaterialItem._self
            new TranslationSeedItem("entity.billOfMaterialItem._self", "ja-JP", "Takt物料清单明细信息", "实体名称"),
            // entity.billOfMaterialItem._self
            new TranslationSeedItem("entity.billOfMaterialItem._self", "zh-CN", "Takt物料清单明细信息", "实体名称"),
            // entity.billOfMaterialItem._self
            new TranslationSeedItem("entity.billOfMaterialItem._self", "zh-HK", "Takt物料清单明细信息", "实体名称"),

            // entity.billOfMaterialItem.billofmaterialid
            new TranslationSeedItem("entity.billOfMaterialItem.billofmaterialid", "en-US", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.billofmaterialid
            new TranslationSeedItem("entity.billOfMaterialItem.billofmaterialid", "ja-JP", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.billofmaterialid
            new TranslationSeedItem("entity.billOfMaterialItem.billofmaterialid", "zh-CN", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.billofmaterialid
            new TranslationSeedItem("entity.billOfMaterialItem.billofmaterialid", "zh-HK", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),

            // entity.billOfMaterialItem.bomcode
            new TranslationSeedItem("entity.billOfMaterialItem.bomcode", "en-US", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billOfMaterialItem.bomcode
            new TranslationSeedItem("entity.billOfMaterialItem.bomcode", "ja-JP", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billOfMaterialItem.bomcode
            new TranslationSeedItem("entity.billOfMaterialItem.bomcode", "zh-CN", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billOfMaterialItem.bomcode
            new TranslationSeedItem("entity.billOfMaterialItem.bomcode", "zh-HK", "BOM编码", "BOM编码（冗余，便于查询）"),

            // entity.billOfMaterialItem.linenumber
            new TranslationSeedItem("entity.billOfMaterialItem.linenumber", "en-US", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billOfMaterialItem.linenumber
            new TranslationSeedItem("entity.billOfMaterialItem.linenumber", "ja-JP", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billOfMaterialItem.linenumber
            new TranslationSeedItem("entity.billOfMaterialItem.linenumber", "zh-CN", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billOfMaterialItem.linenumber
            new TranslationSeedItem("entity.billOfMaterialItem.linenumber", "zh-HK", "行号", "行号（项号，步长10：10/20/30…）"),

            // entity.billOfMaterialItem.materialid
            new TranslationSeedItem("entity.billOfMaterialItem.materialid", "en-US", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.materialid
            new TranslationSeedItem("entity.billOfMaterialItem.materialid", "ja-JP", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.materialid
            new TranslationSeedItem("entity.billOfMaterialItem.materialid", "zh-CN", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billOfMaterialItem.materialid
            new TranslationSeedItem("entity.billOfMaterialItem.materialid", "zh-HK", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),

            // entity.billOfMaterialItem.materialcode
            new TranslationSeedItem("entity.billOfMaterialItem.materialcode", "en-US", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billOfMaterialItem.materialcode
            new TranslationSeedItem("entity.billOfMaterialItem.materialcode", "ja-JP", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billOfMaterialItem.materialcode
            new TranslationSeedItem("entity.billOfMaterialItem.materialcode", "zh-CN", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billOfMaterialItem.materialcode
            new TranslationSeedItem("entity.billOfMaterialItem.materialcode", "zh-HK", "子项物料编码", "子项物料编码（冗余，component_item_code）"),

            // entity.billOfMaterialItem.usagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.usagequantity", "en-US", "用量", "用量（quantity）"),
            // entity.billOfMaterialItem.usagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.usagequantity", "ja-JP", "用量", "用量（quantity）"),
            // entity.billOfMaterialItem.usagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.usagequantity", "zh-CN", "用量", "用量（quantity）"),
            // entity.billOfMaterialItem.usagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.usagequantity", "zh-HK", "用量", "用量（quantity）"),

            // entity.billOfMaterialItem.materialunit
            new TranslationSeedItem("entity.billOfMaterialItem.materialunit", "en-US", "单位", "单位（unit）"),
            // entity.billOfMaterialItem.materialunit
            new TranslationSeedItem("entity.billOfMaterialItem.materialunit", "ja-JP", "单位", "单位（unit）"),
            // entity.billOfMaterialItem.materialunit
            new TranslationSeedItem("entity.billOfMaterialItem.materialunit", "zh-CN", "单位", "单位（unit）"),
            // entity.billOfMaterialItem.materialunit
            new TranslationSeedItem("entity.billOfMaterialItem.materialunit", "zh-HK", "单位", "单位（unit）"),

            // entity.billOfMaterialItem.scraprate
            new TranslationSeedItem("entity.billOfMaterialItem.scraprate", "en-US", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billOfMaterialItem.scraprate
            new TranslationSeedItem("entity.billOfMaterialItem.scraprate", "ja-JP", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billOfMaterialItem.scraprate
            new TranslationSeedItem("entity.billOfMaterialItem.scraprate", "zh-CN", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billOfMaterialItem.scraprate
            new TranslationSeedItem("entity.billOfMaterialItem.scraprate", "zh-HK", "损耗率", "损耗率（0-100，scrap_rate）"),

            // entity.billOfMaterialItem.actualusagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.actualusagequantity", "en-US", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billOfMaterialItem.actualusagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.actualusagequantity", "ja-JP", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billOfMaterialItem.actualusagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.actualusagequantity", "zh-CN", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billOfMaterialItem.actualusagequantity
            new TranslationSeedItem("entity.billOfMaterialItem.actualusagequantity", "zh-HK", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),

            // entity.billOfMaterialItem.operationseq
            new TranslationSeedItem("entity.billOfMaterialItem.operationseq", "en-US", "工序号", "工序号（operation_seq）"),
            // entity.billOfMaterialItem.operationseq
            new TranslationSeedItem("entity.billOfMaterialItem.operationseq", "ja-JP", "工序号", "工序号（operation_seq）"),
            // entity.billOfMaterialItem.operationseq
            new TranslationSeedItem("entity.billOfMaterialItem.operationseq", "zh-CN", "工序号", "工序号（operation_seq）"),
            // entity.billOfMaterialItem.operationseq
            new TranslationSeedItem("entity.billOfMaterialItem.operationseq", "zh-HK", "工序号", "工序号（operation_seq）"),

            // entity.billOfMaterialItem.workcenter
            new TranslationSeedItem("entity.billOfMaterialItem.workcenter", "en-US", "工作中心", "工作中心（work_center）"),
            // entity.billOfMaterialItem.workcenter
            new TranslationSeedItem("entity.billOfMaterialItem.workcenter", "ja-JP", "工作中心", "工作中心（work_center）"),
            // entity.billOfMaterialItem.workcenter
            new TranslationSeedItem("entity.billOfMaterialItem.workcenter", "zh-CN", "工作中心", "工作中心（work_center）"),
            // entity.billOfMaterialItem.workcenter
            new TranslationSeedItem("entity.billOfMaterialItem.workcenter", "zh-HK", "工作中心", "工作中心（work_center）"),

            // entity.billOfMaterialItem.position
            new TranslationSeedItem("entity.billOfMaterialItem.position", "en-US", "位号", "位号（position，PCB位号等）"),
            // entity.billOfMaterialItem.position
            new TranslationSeedItem("entity.billOfMaterialItem.position", "ja-JP", "位号", "位号（position，PCB位号等）"),
            // entity.billOfMaterialItem.position
            new TranslationSeedItem("entity.billOfMaterialItem.position", "zh-CN", "位号", "位号（position，PCB位号等）"),
            // entity.billOfMaterialItem.position
            new TranslationSeedItem("entity.billOfMaterialItem.position", "zh-HK", "位号", "位号（position，PCB位号等）"),

            // entity.billOfMaterialItem.substitutegroup
            new TranslationSeedItem("entity.billOfMaterialItem.substitutegroup", "en-US", "替代组号", "替代组号（substitute_group）"),
            // entity.billOfMaterialItem.substitutegroup
            new TranslationSeedItem("entity.billOfMaterialItem.substitutegroup", "ja-JP", "替代组号", "替代组号（substitute_group）"),
            // entity.billOfMaterialItem.substitutegroup
            new TranslationSeedItem("entity.billOfMaterialItem.substitutegroup", "zh-CN", "替代组号", "替代组号（substitute_group）"),
            // entity.billOfMaterialItem.substitutegroup
            new TranslationSeedItem("entity.billOfMaterialItem.substitutegroup", "zh-HK", "替代组号", "替代组号（substitute_group）"),

            // entity.billOfMaterialItem.substitutepriority
            new TranslationSeedItem("entity.billOfMaterialItem.substitutepriority", "en-US", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billOfMaterialItem.substitutepriority
            new TranslationSeedItem("entity.billOfMaterialItem.substitutepriority", "ja-JP", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billOfMaterialItem.substitutepriority
            new TranslationSeedItem("entity.billOfMaterialItem.substitutepriority", "zh-CN", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billOfMaterialItem.substitutepriority
            new TranslationSeedItem("entity.billOfMaterialItem.substitutepriority", "zh-HK", "替代优先级", "替代优先级（组内越小越优先）"),

            // entity.billOfMaterialItem.isoptional
            new TranslationSeedItem("entity.billOfMaterialItem.isoptional", "en-US", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billOfMaterialItem.isoptional
            new TranslationSeedItem("entity.billOfMaterialItem.isoptional", "ja-JP", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billOfMaterialItem.isoptional
            new TranslationSeedItem("entity.billOfMaterialItem.isoptional", "zh-CN", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billOfMaterialItem.isoptional
            new TranslationSeedItem("entity.billOfMaterialItem.isoptional", "zh-HK", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),

            // entity.billOfMaterialItem.isphantom
            new TranslationSeedItem("entity.billOfMaterialItem.isphantom", "en-US", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billOfMaterialItem.isphantom
            new TranslationSeedItem("entity.billOfMaterialItem.isphantom", "ja-JP", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billOfMaterialItem.isphantom
            new TranslationSeedItem("entity.billOfMaterialItem.isphantom", "zh-CN", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billOfMaterialItem.isphantom
            new TranslationSeedItem("entity.billOfMaterialItem.isphantom", "zh-HK", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),

            // entity.billOfMaterialItem.bom
            new TranslationSeedItem("entity.billOfMaterialItem.bom", "en-US", "物料清单", "物料清单（BOM头）"),
            // entity.billOfMaterialItem.bom
            new TranslationSeedItem("entity.billOfMaterialItem.bom", "ja-JP", "物料清单", "物料清单（BOM头）"),
            // entity.billOfMaterialItem.bom
            new TranslationSeedItem("entity.billOfMaterialItem.bom", "zh-CN", "物料清单", "物料清单（BOM头）"),
            // entity.billOfMaterialItem.bom
            new TranslationSeedItem("entity.billOfMaterialItem.bom", "zh-HK", "物料清单", "物料清单（BOM头）"),

            // entity.billOfMaterialItem.material
            new TranslationSeedItem("entity.billOfMaterialItem.material", "en-US", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billOfMaterialItem.material
            new TranslationSeedItem("entity.billOfMaterialItem.material", "ja-JP", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billOfMaterialItem.material
            new TranslationSeedItem("entity.billOfMaterialItem.material", "zh-CN", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billOfMaterialItem.material
            new TranslationSeedItem("entity.billOfMaterialItem.material", "zh-HK", "子项物料", "子项物料（工厂物料主数据）"),
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
