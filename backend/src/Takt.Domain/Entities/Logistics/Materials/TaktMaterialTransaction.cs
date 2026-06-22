// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialTransaction.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料交易主表实体，统一记录收货/发货/移库/盘点等库存异动单据头
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料交易主表实体（公司级；覆盖后勤模块收发货、库内作业、领借还与调拨核销等业务）
/// </summary>
[SugarTable("takt_logistics_materials_material_transaction", "物料交易表")]
[SugarIndex("ix_material_transaction_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_transaction_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialTransactionCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_transaction_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TransactionDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_transaction_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TransactionStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_source_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceCode), OrderByType.Asc, false)]
public class TaktMaterialTransaction : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "C100")]
    public string PlantCode { get; set; } = "C100";

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "material_transaction_code", ColumnDescription = "物料交易单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易日期
    /// </summary>
    [SugarColumn(ColumnName = "transaction_date", ColumnDescription = "交易日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_direction", ColumnDescription = "交易方向", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TransactionDirection { get; set; } = 0;

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_type", ColumnDescription = "交易类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TransactionType { get; set; } = 0;

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    [SugarColumn(ColumnName = "business_action", ColumnDescription = "业务动作", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BusinessAction { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    [SugarColumn(ColumnName = "source_code", ColumnDescription = "来源单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SourceCode { get; set; }

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    [SugarColumn(ColumnName = "partner_code", ColumnDescription = "往来方编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? PartnerCode { get; set; }

    /// <summary>
    /// 往来方名称
    /// </summary>
    [SugarColumn(ColumnName = "partner_name", ColumnDescription = "往来方名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? PartnerName { get; set; }

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "源仓库编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "C008")]
    public string WarehouseCode { get; set; } = "C008";

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    [SugarColumn(ColumnName = "location_code", ColumnDescription = "源库位编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "1F-2")]
    public string LocationCode { get; set; } = "1F-2";

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "target_warehouse_code", ColumnDescription = "目标仓库编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TargetWarehouseCode { get; set; }

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    [SugarColumn(ColumnName = "target_location_code", ColumnDescription = "目标库位编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TargetLocationCode { get; set; }

    /// <summary>
    /// 关联公司
    /// </summary>
    [SugarColumn(ColumnName = "related_company", ColumnDescription = "关联公司", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "2300")]
    public string RelatedCompany { get; set; } = "2300";

    /// <summary>
    /// 交易总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "交易总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_status", ColumnDescription = "交易状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TransactionStatus { get; set; } = 0;

    /// <summary>
    /// 过账日期
    /// </summary>
    [SugarColumn(ColumnName = "posted_date", ColumnDescription = "过账日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// 过账人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// 物料交易明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaterialTransactionItem.MaterialTransactionId))]
    public List<TaktMaterialTransactionItem>? Items { get; set; }
}
