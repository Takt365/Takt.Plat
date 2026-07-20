// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDocument.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料凭证主表实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料凭证主表实体（公司级；行项目见 TaktMaterialDocumentItem）
/// </summary>
[SugarTable("takt_logistics_materials_material_document", "物料凭证表")]
[SugarIndex("ix_material_document_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_document_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_doc_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialDocumentCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_document_document_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktMaterialDocument : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料凭证号（租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_code", ColumnDescription = "物料凭证号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string MaterialDocumentCode { get; set; } = string.Empty;
    /// <summary>
    /// 过账人（选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? PostedBy { get; set; }
    /// <summary>
    /// 物料凭证状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_status", ColumnDescription = "物料凭证状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaterialDocumentStatus { get; set; } = 0;

    /// <summary>
    /// 物料凭证行项目列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaterialDocumentItem.MaterialDocumentId))]
    public List<TaktMaterialDocumentItem>? Items { get; set; }
}
