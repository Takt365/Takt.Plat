// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktManufacturer.cs
// 创建时间：2026-05-13
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt制造商实体，定义制造商领域模型
// 
// 业务语义说明：
// Manufacturer（制造商）：指实际生产产品的工厂或企业，
// 强调生产制造能力，通常拥有生产线、设备、工艺技术等
// 典型出现在生产管理、质量管理、供应链等场景
// 例如：汽车制造商、电子产品制造商、食品制造商等
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt制造商实体
/// </summary>
[SugarTable("takt_logistics_materials_manufacturer", "制造商信息表")]
[SugarIndex("ix_manufacturer_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_manufacturer_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_manufacturer_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerCode), OrderByType.Asc, true)]
public class TaktManufacturer : TaktCompanyEntityBase
{
    /// <summary>
    /// 制造商编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_code", ColumnDescription = "制造商编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ManufacturerCode { get; set; } = string.Empty;
    /// <summary>
    /// 制造商名称1
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_name1", ColumnDescription = "制造商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string ManufacturerName1 { get; set; } = string.Empty;
    /// <summary>
    /// 制造商名称2
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_name2", ColumnDescription = "制造商名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? ManufacturerName2 { get; set; }
    /// <summary>
    /// 制造商简称
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_short_name", ColumnDescription = "制造商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ManufacturerShortName { get; set; }
    /// <summary>
    /// 制造商类型（字典 logistics_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_type", ColumnDescription = "制造商类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ManufacturerType { get; set; } = 0;
    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；DictValue=A/C/M/P）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? IndustrySector { get; set; }
    /// <summary>
    /// 制造商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_tax_number", ColumnDescription = "制造商标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ManufacturerTaxNumber { get; set; }
    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_country", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RegistrationCountry { get; set; }
    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_province", ColumnDescription = "注册省", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? RegistrationProvince { get; set; }
    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册市", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? RegistrationCity { get; set; }
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? RegistrationAddress1 { get; set; }
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 制造商电话
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_phone", ColumnDescription = "制造商电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ManufacturerPhone { get; set; }
    /// <summary>
    /// 制造商传真
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_fax", ColumnDescription = "制造商传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ManufacturerFax { get; set; }
    /// <summary>
    /// 制造商邮箱
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_email", ColumnDescription = "制造商邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ManufacturerEmail { get; set; }
    /// <summary>
    /// 制造商网站
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_website", ColumnDescription = "制造商网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ManufacturerWebsite { get; set; }
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
    /// 制造商等级（字典 logistics_grade_category；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_level", ColumnDescription = "制造商等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ManufacturerLevel { get; set; } = 0;
    /// <summary>
    /// 质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）
    /// </summary>
    [SugarColumn(ColumnName = "quality_certification", ColumnDescription = "质量认证", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QualityCertification { get; set; } = 0;
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
    /// 制造商状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_status", ColumnDescription = "制造商状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ManufacturerStatus { get; set; } = 1;

    /// <summary>
    /// 导航属性：制造商物料明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktManufacturerMaterial.ManufacturerId))]
    public List<TaktManufacturerMaterial>? ManufacturerMaterials { get; set; }

}
