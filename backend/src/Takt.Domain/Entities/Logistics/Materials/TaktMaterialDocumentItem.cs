// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDocumentItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料凭证行项目实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料凭证行项目实体
/// </summary>
[SugarTable("takt_logistics_materials_material_document_item", "物料凭证行项目表")]
[SugarIndex("ix_material_document_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_document_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_transaction_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_purchase_order", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, false)]
public class TaktMaterialDocumentItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料凭证 ID（关联 TaktMaterialDocument.Id，选项 TaktMaterialDocuments/options）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_id", ColumnDescription = "物料凭证ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }
    /// <summary>
    /// 物料凭证号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_code", ColumnDescription = "物料凭证号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string MaterialDocumentCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 库存地点（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "库存地点", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string WarehouseCode { get; set; } = string.Empty;
    /// <summary>
    /// 移动类型（字典 logistics_movement_type，如 101=收货）
    /// </summary>
    [SugarColumn(ColumnName = "movement_type", ColumnDescription = "移动类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "101")]
    public string MovementType { get; set; } = "101";
    /// <summary>
    /// 过账日期
    /// </summary>
    [SugarColumn(ColumnName = "posting_date", ColumnDescription = "过账日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PostingDate { get; set; }
    /// <summary>
    /// 数量（基本单位数量，出库为负由移动类型决定）
    /// </summary>
    [SugarColumn(ColumnName = "quantity", ColumnDescription = "数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal Quantity { get; set; } = 0;
    /// <summary>
    /// 特殊库存（字典 logistics_special_stock_type，空=非特殊库存）
    /// </summary>
    [SugarColumn(ColumnName = "special_stock", ColumnDescription = "特殊库存", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SpecialStock { get; set; }
    /// <summary>
    /// 采购订单（关联 TaktPurchaseOrder.PurchaseOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }
    /// <summary>
    /// 生产订单
    /// </summary>
    [SugarColumn(ColumnName = "production_order_code", ColumnDescription = "生产订单", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? ProductionOrderCode { get; set; }
    /// <summary>
    /// 项目编号（WBS 元素）
    /// </summary>
    [SugarColumn(ColumnName = "project_code", ColumnDescription = "项目编号", ColumnDataType = "nvarchar", Length = 24, IsNullable = true)]
    public string? ProjectCode { get; set; }
    /// <summary>
    /// 本位币金额
    /// </summary>
    [SugarColumn(ColumnName = "local_currency_amount", ColumnDescription = "本位币金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal LocalCurrencyAmount { get; set; } = 0;
    /// <summary>
    /// 凭证日期
    /// </summary>
    [SugarColumn(ColumnName = "document_date", ColumnDescription = "凭证日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime DocumentDate { get; set; }
    /// <summary>
    /// 收货/发货单编号
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_code", ColumnDescription = "收货/发货单编号", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? ReferenceDocumentCode { get; set; }
    /// <summary>
    /// 客户（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料凭证主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialDocumentId))]
    public TaktMaterialDocument? MaterialTransaction { get; set; }
}
