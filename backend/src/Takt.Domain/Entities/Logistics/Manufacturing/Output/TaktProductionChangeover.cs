// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeover.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：生产切换记录实体，记录生产工厂、生产类别、生产日期、生产班组、SOP时间、人数、切换次数等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 生产切换记录实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_production_changeover", "生产切换记录表")]
[SugarIndex("ix_production_changeover_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_changeover_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_production_changeover_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdTeam), OrderByType.Asc, true)]
public class TaktProductionChangeover : TaktCompanyEntityBase
{
    /// <summary>
    /// 生产工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "生产工厂", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdCategory { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    [SugarColumn(ColumnName = "prod_date", ColumnDescription = "生产日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    [SugarColumn(ColumnName = "prod_team", ColumnDescription = "生产班组", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdTeam { get; set; }

    /// <summary>
    /// 读取SOP时间
    /// </summary>
    [SugarColumn(ColumnName = "read_sop_time", ColumnDescription = "读取SOP时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ReadSopTime { get; set; } = 0;

    /// <summary>
    /// 人数
    /// </summary>
    [SugarColumn(ColumnName = "person_count", ColumnDescription = "人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// SOP总时间
    /// </summary>
    [SugarColumn(ColumnName = "total_sop_time", ColumnDescription = "SOP总时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalSopTime { get; set; } = 0;

    /// <summary>
    /// 切换次数
    /// </summary>
    [SugarColumn(ColumnName = "changeover_count", ColumnDescription = "切换次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_time", ColumnDescription = "切换时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 切换总时间
    /// </summary>
    [SugarColumn(ColumnName = "total_changeover_time", ColumnDescription = "切换总时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalChangeoverTime { get; set; } = 0;
}
