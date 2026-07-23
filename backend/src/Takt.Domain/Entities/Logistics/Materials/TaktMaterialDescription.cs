// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDescription.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料多语言描述实体（对齐 SAP MAKT；主子表子表，主表为 TaktMaterial）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料多语言描述实体（租户级；SAP MAKT：MATNR + SPRAS + MAKTX）
/// </summary>
[SugarTable("takt_logistics_materials_material_description", "物料描述表")]
[SugarIndex("ix_takt_logistics_materials_material_description_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_description_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialId), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_description_material_id", nameof(TenantCode), OrderByType.Asc, nameof(MaterialId), OrderByType.Asc, false)]
public class TaktMaterialDescription : TaktTenantEntityBase
{
    /// <summary>
    /// 物料ID（主子表关系：关联 TaktMaterial.Id；SAP MAKT.MATNR）
    /// </summary>
    [SugarColumn(ColumnName = "material_id", ColumnDescription = "物料ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料描述（SAP MAKT.MAKTX）
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；对齐 SAP MAKT.SPRAS，存 BCP47 如 zh-CN）
    /// </summary>
    [SugarColumn(ColumnName = "culture_code", ColumnDescription = "语言", ColumnDataType = "varchar", Length = 5, IsNullable = false)]
    public string CultureCode { get; set; } = string.Empty;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 所属物料（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialId))]
    public TaktMaterial? Material { get; set; }
}
