// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktSupplier.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt供货商实体，定义供货商领域模型
// 
// 业务语义说明：
// Supplier（供应商）：更常用于"供货方"，强调向你提供物料、产品、零部件的对象
// 典型出现在采购（Procurement）、库存、生产、MM（物料管理）等场景
// 例如：制造商的原材料供应商、贸易公司的货品供应商
// 
// Vendor（卖方/供应商）：含义更广，常作为"交易对方"的统称，既可指供货方，
// 也可涵盖服务提供者、资产出租方等；在ERP/财务系统里，"Vendor Master"用
// 来管理所有对外付款对象（包含Supplier、服务公司、租赁公司等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt供货商实体
/// </summary>
[SugarTable("takt_logistics_materials_supplier", "供货商信息表")]
[SugarIndex("ix_supplier_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_supplier_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_supplier_supplier_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_supplier_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSupplier : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供货商编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 供货商名称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name", ColumnDescription = "供货商名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string SupplierName { get; set; } = string.Empty;
    /// <summary>
    /// 供货商简称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_short_name", ColumnDescription = "供货商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SupplierShortName { get; set; }
    /// <summary>
    /// 供货商类型（字典 logistics_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_type", ColumnDescription = "供货商类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SupplierType { get; set; } = 0;
    /// <summary>
    /// 行业领域（字典 logistics_industry_sector，DictValue=A/C/M/P）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? IndustrySector { get; set; }
    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_tax_number", ColumnDescription = "供货商标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierTaxNumber { get; set; }
    /// <summary>
    /// 税率（字典 accounting_tax_rate_param；13=13%，9=9%，0=0% 等）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 注册国家（ISO 3166-1 alpha-2两位代码）
    /// </summary>
    [SugarColumn(ColumnName = "registration_country", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RegistrationCountry { get; set; }
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress1 { get; set; }
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 注册地址3
    /// </summary>
    [SugarColumn(ColumnName = "registration_address3", ColumnDescription = "注册地址3", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RegistrationAddress3 { get; set; }
    /// <summary>
    /// 供货商电话
    /// </summary>
    [SugarColumn(ColumnName = "supplier_phone", ColumnDescription = "供货商电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierPhone { get; set; }
    /// <summary>
    /// 供货商传真
    /// </summary>
    [SugarColumn(ColumnName = "supplier_fax", ColumnDescription = "供货商传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierFax { get; set; }
    /// <summary>
    /// 供货商邮箱
    /// </summary>
    [SugarColumn(ColumnName = "supplier_email", ColumnDescription = "供货商邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SupplierEmail { get; set; }
    /// <summary>
    /// 供货商网站
    /// </summary>
    [SugarColumn(ColumnName = "supplier_website", ColumnDescription = "供货商网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? SupplierWebsite { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "联系人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 联系人电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系人电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系人邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }
    /// <summary>
    /// 结算币种代码（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 付款条件（字典 accounting_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "cod")]
    public string PaymentTerms { get; set; } = "cod";
    /// <summary>
    /// 供货商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_level", ColumnDescription = "供货商等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SupplierLevel { get; set; } = 0;
    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_score", ColumnDescription = "评价分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EvaluationScore { get; set; } = 0;
    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 供货商状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_status", ColumnDescription = "供货商状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SupplierStatus { get; set; } = 1;
}
