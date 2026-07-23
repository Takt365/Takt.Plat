// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseRequest.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购申请实体，定义采购申请领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购申请实体
/// </summary>
[SugarTable("takt_logistics_materials_purchase_request", "采购申请表")]
[SugarIndex("ix_purchase_request_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_request_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_pr_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseRequestCode), OrderByType.Asc, nameof(RequestDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_request_by", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RequestBy), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_request_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RequestDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_request_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RequestId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_purchase_request_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
public class TaktPurchaseRequest : TaktApprovalEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购申请编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_code", ColumnDescription = "采购申请编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchaseRequestCode { get; set; } = string.Empty;
    /// <summary>
    /// 来源采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_id", ColumnDescription = "来源采购询价ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }
    /// <summary>
    /// 来源采购询价编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "来源采购询价编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? PurchaseInquiryCode { get; set; }
    /// <summary>
    /// 来源采购计划 ID（MRP 下推，关联 TaktPurchasePlan.Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_id", ColumnDescription = "来源采购计划ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }
    /// <summary>
    /// 来源采购计划编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_code", ColumnDescription = "来源采购计划编码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? PurchasePlanCode { get; set; }
    /// <summary>
    /// 采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一，2=方案二）
    /// </summary>
    [SugarColumn(ColumnName = "chain_scheme", ColumnDescription = "采购链路方案", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ChainScheme { get; set; } = 1;
    /// <summary>
    /// PO 生成决策（方案一：null=待决策，1=生成 PO，0=暂不生成 PO）
    /// </summary>
    [SugarColumn(ColumnName = "po_decision", ColumnDescription = "PO生成决策", ColumnDataType = "int", IsNullable = true)]
    public int? PoDecision { get; set; }
    /// <summary>
    /// PR 会签单 ID（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_id", ColumnDescription = "PR会签单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }
    /// <summary>
    /// PR 会签编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_code", ColumnDescription = "PR会签编码", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? CountersignCode { get; set; }
    /// <summary>
    /// 申请日期
    /// </summary>
    [SugarColumn(ColumnName = "request_date", ColumnDescription = "申请日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime RequestDate { get; set; } = DateTime.Now;
    /// <summary>
    /// 要求到货日期
    /// </summary>
    [SugarColumn(ColumnName = "required_arrival_date", ColumnDescription = "要求到货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RequiredArrivalDate { get; set; }
    /// <summary>
    /// 申请人员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "request_id", ColumnDescription = "申请人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RequestId { get; set; }
    /// <summary>
    /// 申请人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "request_by", ColumnDescription = "申请人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string RequestBy { get; set; } = string.Empty;
    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 结算币种（字典 accounting_currency_code；DictValue=CNY/USD 等；一单一币种）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等；一单一税率）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 税费（精确到分）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;
    /// <summary>
    /// 申请总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "申请总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;
    /// <summary>
    /// 申请总金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "申请总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;
    /// <summary>
    /// 已转订单数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;
    /// <summary>
    /// 已转订单金额（精确到分，存储为整数，单位为分）
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转订单金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; } = 0;
    /// <summary>
    /// 申请原因
    /// </summary>
    [SugarColumn(ColumnName = "request_reason", ColumnDescription = "申请原因", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? RequestReason { get; set; }
    /// <summary>
    /// 申请状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [SugarColumn(ColumnName = "request_status", ColumnDescription = "申请状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RequestStatus { get; set; } = 1;
    /// <summary>
    /// 转订单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 采购申请明细列表（主子表关系，一个申请可以有多个明细）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseRequestItem.PurchaseRequestId))]
    public List<TaktPurchaseRequestItem>? Items { get; set; }
}
