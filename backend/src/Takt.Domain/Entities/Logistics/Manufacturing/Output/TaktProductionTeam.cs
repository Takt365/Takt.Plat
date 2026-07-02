#nullable enable
// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeam.cs
// 创建时间：2026-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组实体，用于替代 prod_team_category 字典管理生产班组主数据
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 生产班组实体（生产线班组主数据）
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_production_team", "生产班组表")]
[SugarIndex("ix_production_team_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_team_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_production_team_team_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(TeamCategory), OrderByType.Asc, true)]
public class TaktProductionTeam : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "班组编码", ColumnDataType = "nvarchar", Length = 32, IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    [SugarColumn(ColumnName = "team_name", ColumnDescription = "班组名称", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_team_category，存 DictValue；A=组立 P=PCBA S=SMT Q=质检 O=其他）
    /// </summary>
    [SugarColumn(ColumnName = "team_category", ColumnDescription = "班组分类编码", ColumnDataType = "nvarchar", Length = 2, IsNullable = false, DefaultValue = "A")]
    public string TeamCategory { get; set; } = "A";

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader_name", ColumnDescription = "班组长姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TeamLeaderName { get; set; }

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable_status；0=禁用，1=启用）
    /// </summary>
    [SugarColumn(ColumnName = "status", ColumnDescription = "启用状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Status { get; set; } = 1;

}

