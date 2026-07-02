// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktManufacturerMaterial.cs
// 创建时间：2026-05-13
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt制造商物料明细实体，定义制造商与物料的生产关系
// 
// 业务语义说明：
// TaktManufacturerMaterial是TaktManufacturer的子表，
// 用于记录制造商可以生产的物料清单，仅关注生产制造关系，不涉及商务交易
// 价格和交易信息应在TaktPurchasePrice/TaktPurchasePriceItem中管理
// 
// 典型应用场景：
// - 物料溯源：查询某物料可以由哪些制造商生产
// - 制造商能力：查看某制造商能生产哪些物料
// - 质量管理：记录制造商生产特定物料的质量等级
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt制造商物料明细实体
/// </summary>
[SugarTable("takt_logistics_materials_manufacturer_material", "制造商物料明细表")]
[SugarIndex("ix_manufacturer_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_manufacturer_material_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_material_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerId), OrderByType.Asc, nameof(ManufacturerMaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_material_manufacturer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_material_manufacturer_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_manufacturer_material_manufacturer_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ManufacturerMaterialCode), OrderByType.Asc, false)]
public class TaktManufacturerMaterial : TaktCompanyEntityBase
{
    /// <summary>
    /// 制造商 ID（关联 TaktManufacturer.Id，选项 TaktManufacturers/options）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_id", ColumnDescription = "制造商ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ManufacturerId { get; set; }

    /// <summary>
    /// 制造商编码（关联 TaktManufacturer.ManufacturerCode，冗余；选项 TaktManufacturers/options，DictValue=ManufacturerCode）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_code", ColumnDescription = "制造商编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ManufacturerCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_code", ColumnDescription = "制造商物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料名称（制造商内部的物料名称）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_name", ColumnDescription = "制造商物料名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ManufacturerMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_specification", ColumnDescription = "制造商物料规格", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ManufacturerMaterialSpecification { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 导航属性：关联的制造商
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ManufacturerId))]
    public TaktManufacturer? Manufacturer { get; set; }
}
