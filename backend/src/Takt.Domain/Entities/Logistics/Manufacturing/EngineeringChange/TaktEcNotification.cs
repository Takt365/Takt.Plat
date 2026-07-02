// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotification.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述：工程变更通知单（技术阶段一 ④）；技术保存主表/附件/明细后由系统自动生成并派发，通知各部门进入执行阶段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单（技术阶段一 ④，隶属 TaktEcGijutsu）。技术完成 ①主表 ②附件 ③明细 保存后由 TaktEcGijutsuService 自动生成并派发；
/// 各部门确认后在 TaktEcExec* 执行，技术通过看板/批次监控。FlowInstanceId 由通知审批流程写入（可选）。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_notification", "工程变更通知单表")]
[SugarIndex("ix_ec_notification_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_notification_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_no_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationNo), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktEcNotification : TaktApprovalEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_no", ColumnDescription = "通知单号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNotificationNo { get; set; } = string.Empty;

    /// <summary>
    /// 关联的设变主表ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_id", ColumnDescription = "设变ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_no", ColumnDescription = "设变单号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    [SugarColumn(ColumnName = "ec_title", ColumnDescription = "设变标题", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcTitle { get; set; }

    /// <summary>
    /// 通知日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_date", ColumnDescription = "通知日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_dept_codes", ColumnDescription = "通知部门编码", Length = 200, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNotificationDeptCodes { get; set; }

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_dept_names", ColumnDescription = "通知部门名称", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNotificationDeptNames { get; set; }

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_notifier_id", ColumnDescription = "通知人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_notifier_name", ColumnDescription = "通知人姓名", Length = 50, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNotificationNotifierName { get; set; }

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_method", ColumnDescription = "通知方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcNotificationMethod { get; set; } = 2;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_status", ColumnDescription = "通知状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcNotificationStatus { get; set; } = 0;

    /// <summary>
    /// 关联的设变主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcId))]
    public TaktEcGijutsu? EcGijutsu { get; set; }
}
