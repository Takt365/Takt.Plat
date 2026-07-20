// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterial.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt全局物料实体，租户级物料主数据（无公司/工厂隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt全局物料实体（租户内共享主数据；工厂维度扩展见 TaktMaterialPlant）
/// </summary>
[SugarTable("takt_logistics_materials_material", "全局物料表")]
[SugarIndex("ix_takt_logistics_materials_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_status", nameof(TenantCode), OrderByType.Asc, nameof(MaterialStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_type", nameof(TenantCode), OrderByType.Asc, nameof(MaterialType), OrderByType.Asc, false)]
public class TaktMaterial : TaktTenantEntityBase
{
    /// <summary>
    /// 物料编码（租户内唯一）
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
    /// 物料描述
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MaterialDescription { get; set; }
    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string IndustrySector { get; set; } = string.Empty;
    /// <summary>
    /// 物料层级
    /// </summary>
    [SugarColumn(ColumnName = "material_hierarchy", ColumnDescription = "物料层级", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaterialHierarchy { get; set; }
    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string MaterialGroup { get; set; } = string.Empty;
    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ROH")]
    public string MaterialType { get; set; } = "ROH";
    /// <summary>
    /// 物料型号
    /// </summary>
    [SugarColumn(ColumnName = "material_model", ColumnDescription = "物料型号", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MaterialModel { get; set; }
    /// <summary>
    /// 物料品牌
    /// </summary>
    [SugarColumn(ColumnName = "material_brand", ColumnDescription = "物料品牌", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MaterialBrand { get; set; }
    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";
    /// <summary>
    /// 制造商
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer", ColumnDescription = "制造商", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Manufacturer { get; set; }
    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_code", ColumnDescription = "制造商物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ManufacturerMaterialCode { get; set; }
    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    [SugarColumn(ColumnName = "material_attributes", ColumnDescription = "物料属性", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? MaterialAttributes { get; set; }
    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    [SugarColumn(ColumnName = "is_end_of_life", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "Z0")]
    public string IsEndOfLife { get; set; } = "Z0";
    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "material_status", ColumnDescription = "物料状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MaterialStatus { get; set; } = 1;
}
