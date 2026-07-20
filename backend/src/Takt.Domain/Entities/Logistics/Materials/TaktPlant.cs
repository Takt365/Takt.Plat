// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics
// 文件名称：TaktPlant.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt工厂实体，定义工厂领域模型（字段与公司种子对称，供 TaktPlantSeedData 写入）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt工厂实体
/// 代表租户下的独立工厂（租户级实体，只需要TenantCode）
/// 与公司种子对称，参照 SAP Plant 设计
/// </summary>
[SugarTable("takt_logistics_materials_plant", "工厂表")]
[SugarIndex("ix_plant_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_plant_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_plant_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, true)]
public class TaktPlant : TaktTenantEntityBase
{
    /// <summary>
    /// 工厂代码（唯一索引：租户内唯一，见 ix_plant_code_unique）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 工厂名称
    /// </summary>
    [SugarColumn(ColumnName = "plant_name", ColumnDescription = "工厂名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string PlantName { get; set; } = string.Empty;
    /// <summary>
    /// 工厂简称
    /// </summary>
    [SugarColumn(ColumnName = "plant_short_name", ColumnDescription = "工厂简称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string PlantShortName { get; set; } = string.Empty;
    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [SugarColumn(ColumnName = "code_alias", ColumnDescription = "编码代号", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "TKC")]
    public string CodeAlias { get; set; } = "TKC";
    /// <summary>
    /// 区域文化编码（字典 sys_culture_code，选项 TaktCultures/options，DictValue=CultureCode）
    /// </summary>
    [SugarColumn(ColumnName = "default_culture", ColumnDescription = "区域文化", ColumnDataType = "varchar", Length = 5, IsNullable = false, DefaultValue = "en-US")]
    public string DefaultCulture { get; set; } = "en-US";
    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature_type，DictValue=150 等）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_nature", ColumnDescription = "企业性质", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "150")]
    public string EnterpriseNature { get; set; } = "150";
    /// <summary>
    /// 行业属性（字典 sys_industry_attribute_type，DictValue=C 等）
    /// </summary>
    [SugarColumn(ColumnName = "industry_attribute", ColumnDescription = "行业属性", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "C")]
    public string IndustryAttribute { get; set; } = "C";
    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale_type，DictValue=M 等）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_scale", ColumnDescription = "企业规模", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "M")]
    public string EnterpriseScale { get; set; } = "M";
    /// <summary>
    /// 经营范围
    /// </summary>
    [SugarColumn(ColumnName = "business_scope", ColumnDescription = "经营范围", ColumnDataType = "nvarchar", Length = -1, IsNullable = false, DefaultValue = "")]
    public string BusinessScope { get; set; } = string.Empty;
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string RegistrationAddress1 { get; set; } = string.Empty;
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 注册地址3
    /// </summary>
    [SugarColumn(ColumnName = "registration_address3", ColumnDescription = "注册地址3", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RegistrationAddress3 { get; set; }
    /// <summary>
    /// 注册国家
    /// </summary>
    [SugarColumn(ColumnName = "registration_region", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationRegion { get; set; } = string.Empty;
    /// <summary>
    /// 注册省
    /// </summary>
    [SugarColumn(ColumnName = "registration_province", ColumnDescription = "注册省", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationProvince { get; set; } = string.Empty;
    /// <summary>
    /// 注册市
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册市", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string RegistrationCity { get; set; } = string.Empty;
    /// <summary>
    /// 经营国家
    /// </summary>
    [SugarColumn(ColumnName = "business_region", ColumnDescription = "经营国家", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessRegion { get; set; } = string.Empty;
    /// <summary>
    /// 经营地区-省
    /// </summary>
    [SugarColumn(ColumnName = "business_province", ColumnDescription = "经营地区-省", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessProvince { get; set; } = string.Empty;
    /// <summary>
    /// 经营地区-市
    /// </summary>
    [SugarColumn(ColumnName = "business_city", ColumnDescription = "经营地区-市", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string BusinessCity { get; set; } = string.Empty;
    /// <summary>
    /// 经营地址1
    /// </summary>
    [SugarColumn(ColumnName = "business_address1", ColumnDescription = "经营地址1", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string BusinessAddress1 { get; set; } = string.Empty;
    /// <summary>
    /// 经营地址2
    /// </summary>
    [SugarColumn(ColumnName = "business_address2", ColumnDescription = "经营地址2", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BusinessAddress2 { get; set; }
    /// <summary>
    /// 经营地址3
    /// </summary>
    [SugarColumn(ColumnName = "business_address3", ColumnDescription = "经营地址3", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BusinessAddress3 { get; set; }
    /// <summary>
    /// 工厂地址1
    /// </summary>
    [SugarColumn(ColumnName = "plant_address1", ColumnDescription = "工厂地址1", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? PlantAddress1 { get; set; }
    /// <summary>
    /// 工厂地址2
    /// </summary>
    [SugarColumn(ColumnName = "plant_address2", ColumnDescription = "工厂地址2", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? PlantAddress2 { get; set; }
    /// <summary>
    /// 工厂地址3
    /// </summary>
    [SugarColumn(ColumnName = "plant_address3", ColumnDescription = "工厂地址3", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? PlantAddress3 { get; set; }
    /// <summary>
    /// 工厂电话
    /// </summary>
    [SugarColumn(ColumnName = "plant_phone", ColumnDescription = "工厂电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string PlantPhone { get; set; } = string.Empty;
    /// <summary>
    /// 工厂邮箱
    /// </summary>
    [SugarColumn(ColumnName = "plant_email", ColumnDescription = "工厂邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string PlantEmail { get; set; } = string.Empty;
    /// <summary>
    /// 工厂传真
    /// </summary>
    [SugarColumn(ColumnName = "plant_fax", ColumnDescription = "工厂传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string PlantFax { get; set; } = string.Empty;
    /// <summary>
    /// 工厂网站
    /// </summary>
    [SugarColumn(ColumnName = "plant_website", ColumnDescription = "工厂网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string PlantWebsite { get; set; } = string.Empty;
    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [SugarColumn(ColumnName = "unified_social_credit_code", ColumnDescription = "统一社会信用代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;
    /// <summary>
    /// 税务登记号
    /// </summary>
    [SugarColumn(ColumnName = "tax_registration_number", ColumnDescription = "税务登记号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    /// <summary>
    /// 法定代表人
    /// </summary>
    [SugarColumn(ColumnName = "legal_representative", ColumnDescription = "法定代表人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string LegalRepresentative { get; set; } = string.Empty;
    /// <summary>
    /// 工厂负责人
    /// </summary>
    [SugarColumn(ColumnName = "plant_manager", ColumnDescription = "工厂负责人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string PlantManager { get; set; } = string.Empty;
    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    [SugarColumn(ColumnName = "registered_capital", ColumnDescription = "注册资本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal RegisteredCapital { get; set; } = 0;
    /// <summary>
    /// 成立日期
    /// </summary>
    [SugarColumn(ColumnName = "establishment_date", ColumnDescription = "成立日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EstablishmentDate { get; set; }
    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    [SugarColumn(ColumnName = "closing_date", ColumnDescription = "关闭日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClosingDate { get; set; }
    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
    /// </summary>
    [SugarColumn(ColumnName = "plant_existence", ColumnDescription = "存续状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlantExistence { get; set; } = 1;
    /// <summary>
    /// 关联公司（选项 TaktCompanies/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "related_company", ColumnDescription = "关联公司", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string RelatedCompany { get; set; } = string.Empty;
    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 工厂状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "plant_status", ColumnDescription = "工厂状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlantStatus { get; set; } = 1;
}
