// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialPlantChangeLog.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂物料变更记录实体（变更记录表，非历史表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// 工厂物料变更记录实体
/// </summary>
[SugarTable("takt_logistics_materials_material_plant_change_log", "工厂物料变更记录表")]
[SugarIndex("ix_material_plant_change_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_plant_change_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_plant_change_log_change_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeTime), OrderByType.Desc, false)]
public class TaktMaterialPlantChangeLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂物料 ID（关联 TaktMaterialPlant.Id，选项 TaktMaterialPlants/options）
    /// </summary>
    [SugarColumn(ColumnName = "material_plant_id", ColumnDescription = "工厂物料ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialPlantId { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，冗余；选项 TaktMaterials/options）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值）
    /// 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    [SugarColumn(ColumnName = "change_fields", ColumnDescription = "变更字段列表", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ChangeFields { get; set; }

    /// <summary>
    /// 变更时间
    /// </summary>
    [SugarColumn(ColumnName = "change_time", ColumnDescription = "变更时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ChangeTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 变更人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    [SugarColumn(ColumnName = "change_by", ColumnDescription = "变更人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ChangeBy { get; set; }

    /// <summary>
    /// 变更原因
    /// </summary>
    [SugarColumn(ColumnName = "change_reason", ColumnDescription = "变更原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ChangeReason { get; set; }

    /// <summary>
    /// 工厂物料主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialPlantId))]
    public TaktMaterialPlant? MaterialPlant { get; set; }
}
