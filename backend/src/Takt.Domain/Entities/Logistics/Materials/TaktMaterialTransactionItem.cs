// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialTransactionItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料交易明细实体，记录物料交易单行项目
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料交易明细实体
/// </summary>
[SugarTable("takt_logistics_materials_material_transaction_item", "物料交易明细表")]
[SugarIndex("ix_material_transaction_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_transaction_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialTransactionId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_item_transaction_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialTransactionId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_transaction_item_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktMaterialTransactionItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料交易ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "material_transaction_id", ColumnDescription = "物料交易ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 物料交易单号（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "material_transaction_code", ColumnDescription = "物料交易单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单等业务来源编码）
    /// </summary>
    [SugarColumn(ColumnName = "source_code", ColumnDescription = "来源单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SourceCode { get; set; }

    /// <summary>
    /// 来源单行号
    /// </summary>
    [SugarColumn(ColumnName = "source_line_number", ColumnDescription = "来源单行号", ColumnDataType = "int", IsNullable = true)]
    public int? SourceLineNumber { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [SugarColumn(ColumnName = "material_name", ColumnDescription = "物料名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 交易单位
    /// </summary>
    [SugarColumn(ColumnName = "transaction_unit", ColumnDescription = "交易单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string TransactionUnit { get; set; } = "PC";

    /// <summary>
    /// 交易数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_quantity", ColumnDescription = "交易数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TransactionQuantity { get; set; } = 0;

    /// <summary>
    /// 批次号
    /// </summary>
    [SugarColumn(ColumnName = "batch_no", ColumnDescription = "批次号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? BatchNo { get; set; }

    /// <summary>
    /// 源仓库编码（行级可覆盖主表，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "源仓库编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? WarehouseCode { get; set; }

    /// <summary>
    /// 源库位编码（行级可覆盖主表，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    [SugarColumn(ColumnName = "location_code", ColumnDescription = "源库位编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? LocationCode { get; set; }

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用）
    /// </summary>
    [SugarColumn(ColumnName = "target_warehouse_code", ColumnDescription = "目标仓库编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TargetWarehouseCode { get; set; }

    /// <summary>
    /// 目标库位编码（移库/调拨时使用）
    /// </summary>
    [SugarColumn(ColumnName = "target_location_code", ColumnDescription = "目标库位编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TargetLocationCode { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    [SugarColumn(ColumnName = "unit_price", ColumnDescription = "单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal UnitPrice { get; set; } = 0;

    /// <summary>
    /// 行金额
    /// </summary>
    [SugarColumn(ColumnName = "line_amount", ColumnDescription = "行金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal LineAmount { get; set; } = 0;

    /// <summary>
    /// 物料交易主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialTransactionId))]
    public TaktMaterialTransaction? MaterialTransaction { get; set; }
}
