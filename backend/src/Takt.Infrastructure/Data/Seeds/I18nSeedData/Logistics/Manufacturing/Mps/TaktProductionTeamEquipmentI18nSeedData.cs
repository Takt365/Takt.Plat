// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipmentI18nSeedData.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionTeamEquipment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps;

/// <summary>
/// TaktProductionTeamEquipment 实体国际化翻译种子（键前缀 entity.productionteamequipment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionTeamEquipmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionTeamEquipment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionteamequipment 实体翻译...", tenantCode);

        foreach (var item in GetProductionTeamEquipmentTranslations())
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

        TaktLogger.Information("TaktProductionTeamEquipment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionTeamEquipment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionteamequipment._self / entity.productionteamequipment.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionTeamEquipmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionteamequipment._self
            new TranslationSeedItem("entity.productionteamequipment._self", "en-US", "Production Team Equipment Information_us", "实体名称"),
            // entity.productionteamequipment._self
            new TranslationSeedItem("entity.productionteamequipment._self", "ja-JP", "生产班组设备组明细信息_jp", "实体名称"),
            // entity.productionteamequipment._self
            new TranslationSeedItem("entity.productionteamequipment._self", "zh-CN", "生产班组设备组明细信息", "实体名称"),
            // entity.productionteamequipment._self
            new TranslationSeedItem("entity.productionteamequipment._self", "zh-HK", "生产班组设备组明细信息_hk", "实体名称"),

            // entity.productionteamequipment.plantcode
            new TranslationSeedItem("entity.productionteamequipment.plantcode", "en-US", "工厂代码_us", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.productionteamequipment.plantcode
            new TranslationSeedItem("entity.productionteamequipment.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.productionteamequipment.plantcode
            new TranslationSeedItem("entity.productionteamequipment.plantcode", "zh-CN", "工厂代码", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.productionteamequipment.plantcode
            new TranslationSeedItem("entity.productionteamequipment.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.productionteamequipment.productionteamid
            new TranslationSeedItem("entity.productionteamequipment.productionteamid", "en-US", "生产班组主键_us", "生产班组主键（主子表关系，关联 TaktProductionTeam.Id）"),
            // entity.productionteamequipment.productionteamid
            new TranslationSeedItem("entity.productionteamequipment.productionteamid", "ja-JP", "生产班组主键_jp", "生产班组主键（主子表关系，关联 TaktProductionTeam.Id）"),
            // entity.productionteamequipment.productionteamid
            new TranslationSeedItem("entity.productionteamequipment.productionteamid", "zh-CN", "生产班组主键", "生产班组主键（主子表关系，关联 TaktProductionTeam.Id）"),
            // entity.productionteamequipment.productionteamid
            new TranslationSeedItem("entity.productionteamequipment.productionteamid", "zh-HK", "生产班组主键_hk", "生产班组主键（主子表关系，关联 TaktProductionTeam.Id）"),

            // entity.productionteamequipment.teamcode
            new TranslationSeedItem("entity.productionteamequipment.teamcode", "en-US", "班组编码_us", "班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）"),
            // entity.productionteamequipment.teamcode
            new TranslationSeedItem("entity.productionteamequipment.teamcode", "ja-JP", "班组编码_jp", "班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）"),
            // entity.productionteamequipment.teamcode
            new TranslationSeedItem("entity.productionteamequipment.teamcode", "zh-CN", "班组编码", "班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）"),
            // entity.productionteamequipment.teamcode
            new TranslationSeedItem("entity.productionteamequipment.teamcode", "zh-HK", "班组编码_hk", "班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）"),

            // entity.productionteamequipment.linenumber
            new TranslationSeedItem("entity.productionteamequipment.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.productionteamequipment.linenumber
            new TranslationSeedItem("entity.productionteamequipment.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.productionteamequipment.linenumber
            new TranslationSeedItem("entity.productionteamequipment.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productionteamequipment.linenumber
            new TranslationSeedItem("entity.productionteamequipment.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.productionteamequipment.productionequipmentid
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentid", "en-US", "生产设备主键_us", "生产设备主键（关联 TaktProductionEquipment.Id）"),
            // entity.productionteamequipment.productionequipmentid
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentid", "ja-JP", "生产设备主键_jp", "生产设备主键（关联 TaktProductionEquipment.Id）"),
            // entity.productionteamequipment.productionequipmentid
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentid", "zh-CN", "生产设备主键", "生产设备主键（关联 TaktProductionEquipment.Id）"),
            // entity.productionteamequipment.productionequipmentid
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentid", "zh-HK", "生产设备主键_hk", "生产设备主键（关联 TaktProductionEquipment.Id）"),

            // entity.productionteamequipment.productionequipmentcode
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentcode", "en-US", "生产设备编码_us", "生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）"),
            // entity.productionteamequipment.productionequipmentcode
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentcode", "ja-JP", "生产设备编码_jp", "生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）"),
            // entity.productionteamequipment.productionequipmentcode
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentcode", "zh-CN", "生产设备编码", "生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）"),
            // entity.productionteamequipment.productionequipmentcode
            new TranslationSeedItem("entity.productionteamequipment.productionequipmentcode", "zh-HK", "生产设备编码_hk", "生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）"),

            // entity.productionteamequipment.equipmentquantity
            new TranslationSeedItem("entity.productionteamequipment.equipmentquantity", "en-US", "设备台数_us", "设备台数（同型号多台时 &gt;1）"),
            // entity.productionteamequipment.equipmentquantity
            new TranslationSeedItem("entity.productionteamequipment.equipmentquantity", "ja-JP", "设备台数_jp", "设备台数（同型号多台时 &gt;1）"),
            // entity.productionteamequipment.equipmentquantity
            new TranslationSeedItem("entity.productionteamequipment.equipmentquantity", "zh-CN", "设备台数", "设备台数（同型号多台时 &gt;1）"),
            // entity.productionteamequipment.equipmentquantity
            new TranslationSeedItem("entity.productionteamequipment.equipmentquantity", "zh-HK", "设备台数_hk", "设备台数（同型号多台时 &gt;1）"),

            // entity.productionteamequipment.teamequipmentstatus
            new TranslationSeedItem("entity.productionteamequipment.teamequipmentstatus", "en-US", "班组设备状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionteamequipment.teamequipmentstatus
            new TranslationSeedItem("entity.productionteamequipment.teamequipmentstatus", "ja-JP", "班组设备状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionteamequipment.teamequipmentstatus
            new TranslationSeedItem("entity.productionteamequipment.teamequipmentstatus", "zh-CN", "班组设备状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.productionteamequipment.teamequipmentstatus
            new TranslationSeedItem("entity.productionteamequipment.teamequipmentstatus", "zh-HK", "班组设备状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),

            // entity.productionteamequipment.isobsolete
            new TranslationSeedItem("entity.productionteamequipment.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionteamequipment.isobsolete
            new TranslationSeedItem("entity.productionteamequipment.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionteamequipment.isobsolete
            new TranslationSeedItem("entity.productionteamequipment.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionteamequipment.isobsolete
            new TranslationSeedItem("entity.productionteamequipment.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mps";
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
