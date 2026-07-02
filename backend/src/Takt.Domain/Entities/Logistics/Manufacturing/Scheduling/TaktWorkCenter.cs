// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Scheduling
// 文件名称：TaktWorkCenter.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心主数据，APS 排程资源组织单元；产能日历复用 HR TaktCalendar（RelatedPlant）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Scheduling;

/// <summary>
/// 工作中心（WC；PlantCode 对齐 TaktCalendar.RelatedPlant，班次对齐 TaktWorkShift）
/// </summary>
[SugarTable("takt_logistics_manufacturing_scheduling_work_center", "工作中心表")]
[SugarIndex("ix_work_center_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_work_center_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_scheduling_work_center_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkCenterCode), OrderByType.Asc, true)]
public class TaktWorkCenter : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心名称
    /// </summary>
    [SugarColumn(ColumnName = "work_center_name", ColumnDescription = "工作中心名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string WorkCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 车间编码
    /// </summary>
    [SugarColumn(ColumnName = "workshop_code", ColumnDescription = "车间编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? WorkshopCode { get; set; }

    /// <summary>
    /// 默认班次 ID（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [SugarColumn(ColumnName = "default_shift_id", ColumnDescription = "默认班次ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DefaultShiftId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_status", ColumnDescription = "工作中心状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int WorkCenterStatus { get; set; } = 1;

    /// <summary>
    /// 工作中心资源列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktWorkCenterResource.WorkCenterId))]
    public List<TaktWorkCenterResource>? Resources { get; set; }
}
