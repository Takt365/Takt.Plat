// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialTransactionI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialTransaction 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktMaterialTransaction 实体国际化翻译种子（键前缀 entity.materialtransaction.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialTransactionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialTransaction 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialtransaction 实体翻译...", tenantCode);

        foreach (var item in GetMaterialTransactionTranslations())
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

        TaktLogger.Information("TaktMaterialTransaction 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialTransaction 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialtransaction._self / entity.materialtransaction.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialTransactionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialtransaction._self
            new TranslationSeedItem("entity.materialtransaction._self", "en-US", "Material Transaction Information_us", "实体名称"),
            // entity.materialtransaction._self
            new TranslationSeedItem("entity.materialtransaction._self", "ja-JP", "Takt物料交易主表信息_jp", "实体名称"),
            // entity.materialtransaction._self
            new TranslationSeedItem("entity.materialtransaction._self", "zh-CN", "Takt物料交易主表信息", "实体名称"),
            // entity.materialtransaction._self
            new TranslationSeedItem("entity.materialtransaction._self", "zh-HK", "Takt物料交易主表信息_hk", "实体名称"),

            // entity.materialtransaction.plantcode
            new TranslationSeedItem("entity.materialtransaction.plantcode", "en-US", "工厂代码_us", "工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）"),
            // entity.materialtransaction.plantcode
            new TranslationSeedItem("entity.materialtransaction.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）"),
            // entity.materialtransaction.plantcode
            new TranslationSeedItem("entity.materialtransaction.plantcode", "zh-CN", "工厂代码", "工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）"),
            // entity.materialtransaction.plantcode
            new TranslationSeedItem("entity.materialtransaction.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）"),

            // entity.materialtransaction.code
            new TranslationSeedItem("entity.materialtransaction.code", "en-US", "物料交易单号_us", "物料交易单号（租户+公司+工厂内唯一）"),
            // entity.materialtransaction.code
            new TranslationSeedItem("entity.materialtransaction.code", "ja-JP", "物料交易单号_jp", "物料交易单号（租户+公司+工厂内唯一）"),
            // entity.materialtransaction.code
            new TranslationSeedItem("entity.materialtransaction.code", "zh-CN", "物料交易单号", "物料交易单号（租户+公司+工厂内唯一）"),
            // entity.materialtransaction.code
            new TranslationSeedItem("entity.materialtransaction.code", "zh-HK", "物料交易单号_hk", "物料交易单号（租户+公司+工厂内唯一）"),

            // entity.materialtransaction.transactiondate
            new TranslationSeedItem("entity.materialtransaction.transactiondate", "en-US", "交易日期_us", "交易日期"),
            // entity.materialtransaction.transactiondate
            new TranslationSeedItem("entity.materialtransaction.transactiondate", "ja-JP", "交易日期_jp", "交易日期"),
            // entity.materialtransaction.transactiondate
            new TranslationSeedItem("entity.materialtransaction.transactiondate", "zh-CN", "交易日期", "交易日期"),
            // entity.materialtransaction.transactiondate
            new TranslationSeedItem("entity.materialtransaction.transactiondate", "zh-HK", "交易日期_hk", "交易日期"),

            // entity.materialtransaction.transactiondirection
            new TranslationSeedItem("entity.materialtransaction.transactiondirection", "en-US", "交易方向_us", "交易方向（0=入库，1=出库，2=库内/移库）"),
            // entity.materialtransaction.transactiondirection
            new TranslationSeedItem("entity.materialtransaction.transactiondirection", "ja-JP", "交易方向_jp", "交易方向（0=入库，1=出库，2=库内/移库）"),
            // entity.materialtransaction.transactiondirection
            new TranslationSeedItem("entity.materialtransaction.transactiondirection", "zh-CN", "交易方向", "交易方向（0=入库，1=出库，2=库内/移库）"),
            // entity.materialtransaction.transactiondirection
            new TranslationSeedItem("entity.materialtransaction.transactiondirection", "zh-HK", "交易方向_hk", "交易方向（0=入库，1=出库，2=库内/移库）"),

            // entity.materialtransaction.transactiontype
            new TranslationSeedItem("entity.materialtransaction.transactiontype", "en-US", "交易类型_us", "交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）"),
            // entity.materialtransaction.transactiontype
            new TranslationSeedItem("entity.materialtransaction.transactiontype", "ja-JP", "交易类型_jp", "交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）"),
            // entity.materialtransaction.transactiontype
            new TranslationSeedItem("entity.materialtransaction.transactiontype", "zh-CN", "交易类型", "交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）"),
            // entity.materialtransaction.transactiontype
            new TranslationSeedItem("entity.materialtransaction.transactiontype", "zh-HK", "交易类型_hk", "交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）"),

            // entity.materialtransaction.businessaction
            new TranslationSeedItem("entity.materialtransaction.businessaction", "en-US", "业务动作_us", "业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）"),
            // entity.materialtransaction.businessaction
            new TranslationSeedItem("entity.materialtransaction.businessaction", "ja-JP", "业务动作_jp", "业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）"),
            // entity.materialtransaction.businessaction
            new TranslationSeedItem("entity.materialtransaction.businessaction", "zh-CN", "业务动作", "业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）"),
            // entity.materialtransaction.businessaction
            new TranslationSeedItem("entity.materialtransaction.businessaction", "zh-HK", "业务动作_hk", "业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）"),

            // entity.materialtransaction.sourcecode
            new TranslationSeedItem("entity.materialtransaction.sourcecode", "en-US", "来源单号_us", "来源单号（采购订单、销售订单、生产订单等业务来源编码）"),
            // entity.materialtransaction.sourcecode
            new TranslationSeedItem("entity.materialtransaction.sourcecode", "ja-JP", "来源单号_jp", "来源单号（采购订单、销售订单、生产订单等业务来源编码）"),
            // entity.materialtransaction.sourcecode
            new TranslationSeedItem("entity.materialtransaction.sourcecode", "zh-CN", "来源单号", "来源单号（采购订单、销售订单、生产订单等业务来源编码）"),
            // entity.materialtransaction.sourcecode
            new TranslationSeedItem("entity.materialtransaction.sourcecode", "zh-HK", "来源单号_hk", "来源单号（采购订单、销售订单、生产订单等业务来源编码）"),

            // entity.materialtransaction.partnercode
            new TranslationSeedItem("entity.materialtransaction.partnercode", "en-US", "往来方编码_us", "往来方编码（供应商、客户或部门等业务编码）"),
            // entity.materialtransaction.partnercode
            new TranslationSeedItem("entity.materialtransaction.partnercode", "ja-JP", "往来方编码_jp", "往来方编码（供应商、客户或部门等业务编码）"),
            // entity.materialtransaction.partnercode
            new TranslationSeedItem("entity.materialtransaction.partnercode", "zh-CN", "往来方编码", "往来方编码（供应商、客户或部门等业务编码）"),
            // entity.materialtransaction.partnercode
            new TranslationSeedItem("entity.materialtransaction.partnercode", "zh-HK", "往来方编码_hk", "往来方编码（供应商、客户或部门等业务编码）"),

            // entity.materialtransaction.partnername
            new TranslationSeedItem("entity.materialtransaction.partnername", "en-US", "往来方名称_us", "往来方名称"),
            // entity.materialtransaction.partnername
            new TranslationSeedItem("entity.materialtransaction.partnername", "ja-JP", "往来方名称_jp", "往来方名称"),
            // entity.materialtransaction.partnername
            new TranslationSeedItem("entity.materialtransaction.partnername", "zh-CN", "往来方名称", "往来方名称"),
            // entity.materialtransaction.partnername
            new TranslationSeedItem("entity.materialtransaction.partnername", "zh-HK", "往来方名称_hk", "往来方名称"),

            // entity.materialtransaction.warehousecode
            new TranslationSeedItem("entity.materialtransaction.warehousecode", "en-US", "源仓库编码_us", "源仓库编码（关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.warehousecode
            new TranslationSeedItem("entity.materialtransaction.warehousecode", "ja-JP", "源仓库编码_jp", "源仓库编码（关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.warehousecode
            new TranslationSeedItem("entity.materialtransaction.warehousecode", "zh-CN", "源仓库编码", "源仓库编码（关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.warehousecode
            new TranslationSeedItem("entity.materialtransaction.warehousecode", "zh-HK", "源仓库编码_hk", "源仓库编码（关联 TaktWarehouse.WarehouseCode）"),

            // entity.materialtransaction.locationcode
            new TranslationSeedItem("entity.materialtransaction.locationcode", "en-US", "源库位编码_us", "源库位编码（关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.locationcode
            new TranslationSeedItem("entity.materialtransaction.locationcode", "ja-JP", "源库位编码_jp", "源库位编码（关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.locationcode
            new TranslationSeedItem("entity.materialtransaction.locationcode", "zh-CN", "源库位编码", "源库位编码（关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.locationcode
            new TranslationSeedItem("entity.materialtransaction.locationcode", "zh-HK", "源库位编码_hk", "源库位编码（关联 TaktStorageLocation.LocationCode）"),

            // entity.materialtransaction.targetwarehousecode
            new TranslationSeedItem("entity.materialtransaction.targetwarehousecode", "en-US", "目标仓库编码_us", "目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.targetwarehousecode
            new TranslationSeedItem("entity.materialtransaction.targetwarehousecode", "ja-JP", "目标仓库编码_jp", "目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.targetwarehousecode
            new TranslationSeedItem("entity.materialtransaction.targetwarehousecode", "zh-CN", "目标仓库编码", "目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）"),
            // entity.materialtransaction.targetwarehousecode
            new TranslationSeedItem("entity.materialtransaction.targetwarehousecode", "zh-HK", "目标仓库编码_hk", "目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）"),

            // entity.materialtransaction.targetlocationcode
            new TranslationSeedItem("entity.materialtransaction.targetlocationcode", "en-US", "目标库位编码_us", "目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.targetlocationcode
            new TranslationSeedItem("entity.materialtransaction.targetlocationcode", "ja-JP", "目标库位编码_jp", "目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.targetlocationcode
            new TranslationSeedItem("entity.materialtransaction.targetlocationcode", "zh-CN", "目标库位编码", "目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）"),
            // entity.materialtransaction.targetlocationcode
            new TranslationSeedItem("entity.materialtransaction.targetlocationcode", "zh-HK", "目标库位编码_hk", "目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）"),

            // entity.materialtransaction.relatedcompany
            new TranslationSeedItem("entity.materialtransaction.relatedcompany", "en-US", "关联公司_us", "关联公司"),
            // entity.materialtransaction.relatedcompany
            new TranslationSeedItem("entity.materialtransaction.relatedcompany", "ja-JP", "关联公司_jp", "关联公司"),
            // entity.materialtransaction.relatedcompany
            new TranslationSeedItem("entity.materialtransaction.relatedcompany", "zh-CN", "关联公司", "关联公司"),
            // entity.materialtransaction.relatedcompany
            new TranslationSeedItem("entity.materialtransaction.relatedcompany", "zh-HK", "关联公司_hk", "关联公司"),

            // entity.materialtransaction.totalquantity
            new TranslationSeedItem("entity.materialtransaction.totalquantity", "en-US", "交易总数量_us", "交易总数量（基本单位数量）"),
            // entity.materialtransaction.totalquantity
            new TranslationSeedItem("entity.materialtransaction.totalquantity", "ja-JP", "交易总数量_jp", "交易总数量（基本单位数量）"),
            // entity.materialtransaction.totalquantity
            new TranslationSeedItem("entity.materialtransaction.totalquantity", "zh-CN", "交易总数量", "交易总数量（基本单位数量）"),
            // entity.materialtransaction.totalquantity
            new TranslationSeedItem("entity.materialtransaction.totalquantity", "zh-HK", "交易总数量_hk", "交易总数量（基本单位数量）"),

            // entity.materialtransaction.transactionstatus
            new TranslationSeedItem("entity.materialtransaction.transactionstatus", "en-US", "交易状态_us", "交易状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialtransaction.transactionstatus
            new TranslationSeedItem("entity.materialtransaction.transactionstatus", "ja-JP", "交易状态_jp", "交易状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialtransaction.transactionstatus
            new TranslationSeedItem("entity.materialtransaction.transactionstatus", "zh-CN", "交易状态", "交易状态（0=草稿，1=已过账，2=已作废）"),
            // entity.materialtransaction.transactionstatus
            new TranslationSeedItem("entity.materialtransaction.transactionstatus", "zh-HK", "交易状态_hk", "交易状态（0=草稿，1=已过账，2=已作废）"),

            // entity.materialtransaction.posteddate
            new TranslationSeedItem("entity.materialtransaction.posteddate", "en-US", "过账日期_us", "过账日期"),
            // entity.materialtransaction.posteddate
            new TranslationSeedItem("entity.materialtransaction.posteddate", "ja-JP", "过账日期_jp", "过账日期"),
            // entity.materialtransaction.posteddate
            new TranslationSeedItem("entity.materialtransaction.posteddate", "zh-CN", "过账日期", "过账日期"),
            // entity.materialtransaction.posteddate
            new TranslationSeedItem("entity.materialtransaction.posteddate", "zh-HK", "过账日期_hk", "过账日期"),

            // entity.materialtransaction.postedby
            new TranslationSeedItem("entity.materialtransaction.postedby", "en-US", "过账人_us", "过账人（人员代码）"),
            // entity.materialtransaction.postedby
            new TranslationSeedItem("entity.materialtransaction.postedby", "ja-JP", "过账人_jp", "过账人（人员代码）"),
            // entity.materialtransaction.postedby
            new TranslationSeedItem("entity.materialtransaction.postedby", "zh-CN", "过账人", "过账人（人员代码）"),
            // entity.materialtransaction.postedby
            new TranslationSeedItem("entity.materialtransaction.postedby", "zh-HK", "过账人_hk", "过账人（人员代码）"),

            // entity.materialtransaction.items
            new TranslationSeedItem("entity.materialtransaction.items", "en-US", "物料交易明细列表_us", "物料交易明细列表（主子表关系）"),
            // entity.materialtransaction.items
            new TranslationSeedItem("entity.materialtransaction.items", "ja-JP", "物料交易明细列表_jp", "物料交易明细列表（主子表关系）"),
            // entity.materialtransaction.items
            new TranslationSeedItem("entity.materialtransaction.items", "zh-CN", "物料交易明细列表", "物料交易明细列表（主子表关系）"),
            // entity.materialtransaction.items
            new TranslationSeedItem("entity.materialtransaction.items", "zh-HK", "物料交易明细列表_hk", "物料交易明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Materials";
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
