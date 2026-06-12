// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktBillOfMaterialItem 实体国际化翻译种子（键前缀 entity.billofmaterialitem.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billofmaterialitem 实体翻译...", tenantCode);

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
    /// I18nKey：entity.billofmaterialitem._self / entity.billofmaterialitem.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "en-US", "Bill Of Material Item Information", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "ja-JP", "Takt物料清单明细信息", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "zh-CN", "Takt物料清单明细信息", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "zh-HK", "Takt物料清单明细信息", "实体名称"),

            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "en-US", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "ja-JP", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "zh-CN", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "zh-HK", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),

            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "en-US", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "ja-JP", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "zh-CN", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "zh-HK", "BOM编码", "BOM编码（冗余，便于查询）"),

            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "en-US", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "ja-JP", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "zh-CN", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "zh-HK", "行号", "行号（项号，步长10：10/20/30…）"),

            // entity.billofmaterialitem.materialid
            new TranslationSeedItem("entity.billofmaterialitem.materialid", "en-US", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.materialid
            new TranslationSeedItem("entity.billofmaterialitem.materialid", "ja-JP", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.materialid
            new TranslationSeedItem("entity.billofmaterialitem.materialid", "zh-CN", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.materialid
            new TranslationSeedItem("entity.billofmaterialitem.materialid", "zh-HK", "子项物料ID", "子项物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),

            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "en-US", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "ja-JP", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "zh-CN", "子项物料编码", "子项物料编码（冗余，component_item_code）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "zh-HK", "子项物料编码", "子项物料编码（冗余，component_item_code）"),

            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "en-US", "用量", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "ja-JP", "用量", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "zh-CN", "用量", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "zh-HK", "用量", "用量（quantity）"),

            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "en-US", "单位", "单位（unit）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "ja-JP", "单位", "单位（unit）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "zh-CN", "单位", "单位（unit）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "zh-HK", "单位", "单位（unit）"),

            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "en-US", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "ja-JP", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "zh-CN", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "zh-HK", "损耗率", "损耗率（0-100，scrap_rate）"),

            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "en-US", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "ja-JP", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "zh-CN", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "zh-HK", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),

            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "en-US", "工序号", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "ja-JP", "工序号", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "zh-CN", "工序号", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "zh-HK", "工序号", "工序号（operation_seq）"),

            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "en-US", "工作中心", "工作中心（work_center）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "ja-JP", "工作中心", "工作中心（work_center）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "zh-CN", "工作中心", "工作中心（work_center）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "zh-HK", "工作中心", "工作中心（work_center）"),

            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "en-US", "位号", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "ja-JP", "位号", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "zh-CN", "位号", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "zh-HK", "位号", "位号（position，PCB位号等）"),

            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "en-US", "替代组号", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "ja-JP", "替代组号", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "zh-CN", "替代组号", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "zh-HK", "替代组号", "替代组号（substitute_group）"),

            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "en-US", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "ja-JP", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "zh-CN", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "zh-HK", "替代优先级", "替代优先级（组内越小越优先）"),

            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "en-US", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "ja-JP", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "zh-CN", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "zh-HK", "是否可选件", "是否可选件（0=否，1=是，optional_flag）"),

            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "en-US", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "ja-JP", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "zh-CN", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "zh-HK", "是否虚拟件", "是否虚拟件（0=否，1=是，phantom_flag）"),

            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "en-US", "物料清单", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "ja-JP", "物料清单", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "zh-CN", "物料清单", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "zh-HK", "物料清单", "物料清单（BOM头）"),

            // entity.billofmaterialitem.material
            new TranslationSeedItem("entity.billofmaterialitem.material", "en-US", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billofmaterialitem.material
            new TranslationSeedItem("entity.billofmaterialitem.material", "ja-JP", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billofmaterialitem.material
            new TranslationSeedItem("entity.billofmaterialitem.material", "zh-CN", "子项物料", "子项物料（工厂物料主数据）"),
            // entity.billofmaterialitem.material
            new TranslationSeedItem("entity.billofmaterialitem.material", "zh-HK", "子项物料", "子项物料（工厂物料主数据）"),
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
