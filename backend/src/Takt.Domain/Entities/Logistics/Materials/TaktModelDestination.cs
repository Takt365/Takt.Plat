// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktModelDestination.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt型号目的地实体，定义机型/型号与出货目的地的关联
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt型号目的地实体（租户级；物料编码/名称、机种编码/名称、仕向地编码/名称）
/// </summary>
[SugarTable("takt_logistics_materials_model_destination", "型号目的地表")]
[SugarIndex("ix_model_destination_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_model_destination_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_model_destination_order_num", nameof(TenantCode), OrderByType.Asc, nameof(SortOrder), OrderByType.Asc, false)]
public class TaktModelDestination : TaktTenantEntityBase
{
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
    /// 机种编码
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称
    /// </summary>
    [SugarColumn(ColumnName = "model_name", ColumnDescription = "机种名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码
    /// </summary>
    [SugarColumn(ColumnName = "destination_code", ColumnDescription = "仕向地编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称
    /// </summary>
    [SugarColumn(ColumnName = "destination_name", ColumnDescription = "仕向地名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string DestinationName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
}
