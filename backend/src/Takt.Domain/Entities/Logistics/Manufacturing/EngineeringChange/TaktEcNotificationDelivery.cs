// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationDelivery.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知投递记录（按部门持久化，status=待发送/已发送/已确认）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 确认时间
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_notification_delivery", "工程变更通知投递表")]
[SugarIndex("ix_ec_notification_delivery_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_notification_delivery_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationId), OrderByType.Asc, nameof(DeptCode), OrderByType.Asc, true)]
[SugarIndex("ix_ec_notification_delivery_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeliveryStatus), OrderByType.Asc, false)]
public class TaktEcNotificationDelivery : TaktCompanyEntityBase
{
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_id", ColumnDescription = "通知单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }
    /// <summary>
    /// 通知单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_no", ColumnDescription = "通知单号", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EcNotificationNo { get; set; } = string.Empty;
    /// <summary>
    /// 设变 ID
    /// </summary>
    [SugarColumn(ColumnName = "ec_id", ColumnDescription = "设变ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }
    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "ec_no", ColumnDescription = "设变单号", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 目标部门编码（TaktDept.DeptCode，如 D0710、D0810）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "目标部门编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 目标部门名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "目标部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 优先级（1=普通，2=高，3=紧急）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Priority { get; set; } = 1;
    /// <summary>
    /// 投递状态（0=待发送，1=已发送，2=已确认）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_status", ColumnDescription = "投递状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryStatus { get; set; }
    /// <summary>
    /// 发送时间
    /// </summary>
    [SugarColumn(ColumnName = "sent_at", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? SentAt { get; set; }
    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by_user_id", ColumnDescription = "确认人用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }
    /// <summary>
    /// 确认人用户名
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by_user_name", ColumnDescription = "确认人用户名", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? ConfirmedByUserName { get; set; }
    /// <summary>
    /// 确认时间
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_at", ColumnDescription = "确认时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ConfirmedAt { get; set; }
}
