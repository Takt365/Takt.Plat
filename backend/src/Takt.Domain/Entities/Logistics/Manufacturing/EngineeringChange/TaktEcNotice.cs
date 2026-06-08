// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotice.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述：工程变更通知单（EC Notice），用于将设变（ECN）通知到相关部门和人员
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单实体（EC Notice）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_notice", "工程变更通知单表")]
[SugarIndex("ix_ec_notice_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_notice_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notice_no_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNoticeNo), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notice_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notice_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNoticeDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notice_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNoticeStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notice_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktEcNotice : TaktApprovalEntityBase
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单号（唯一，如：EC-2026-0001）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_no", ColumnDescription = "通知单号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNoticeNo { get; set; } = string.Empty;

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
    /// 设变主题（冗余字段）
    /// </summary>
    [SugarColumn(ColumnName = "ec_title", ColumnDescription = "设变主题", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcTitle { get; set; }

    /// <summary>
    /// 通知日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_date", ColumnDescription = "通知日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcNoticeDate { get; set; }

    /// <summary>
    /// 通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_dept_codes", ColumnDescription = "通知部门编码", Length = 200, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNoticeDeptCodes { get; set; }

    /// <summary>
    /// 通知部门名称（多个部门用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_dept_names", ColumnDescription = "通知部门名称", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNoticeDeptNames { get; set; }

    /// <summary>
    /// 通知人ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_notifier_id", ColumnDescription = "通知人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNoticeNotifierId { get; set; }

    /// <summary>
    /// 通知人姓名
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_notifier_name", ColumnDescription = "通知人姓名", Length = 50, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNoticeNotifierName { get; set; }

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_method", ColumnDescription = "通知方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcNoticeMethod { get; set; } = 2;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notice_status", ColumnDescription = "通知状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcNoticeStatus { get; set; } = 0;

    /// <summary>
    /// 流程实例 ID（<see cref="Workflow.TaktFlowInstance"/>；发起审批后由业务写入）
    /// </summary>
    [SugarColumn(ColumnName = "flow_instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 关联的设变主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcId))]
    public TaktEc? Ec { get; set; }
}
