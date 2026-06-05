// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktMessage.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息实体，用于通过 SignalR 管理在线消息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 在线消息实体
/// 公司级实体：消息按租户+公司双重隔离
/// </summary>
[SugarTable("takt_foundation_message", "在线消息表")]
[SugarIndex("ix_message_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_message_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_message_from_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FromUserName), OrderByType.Asc, false)]
[SugarIndex("ix_message_read_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReadStatus), OrderByType.Asc, false)]
[SugarIndex("ix_message_send_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SendTime), OrderByType.Desc, false)]
[SugarIndex("ix_message_to_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ToUserName), OrderByType.Asc, false)]
[SugarIndex("ix_message_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MessageType), OrderByType.Asc, false)]
public class TaktMessage : TaktCompanyEntityBase
{
    /// <summary>
    /// 发送者用户名
    /// </summary>
    [SugarColumn(ColumnName = "from_user_name", ColumnDescription = "发送者用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "from_user_id", ColumnDescription = "发送者用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 接收者用户名
    /// </summary>
    [SugarColumn(ColumnName = "to_user_name", ColumnDescription = "接收者用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "to_user_id", ColumnDescription = "接收者用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 消息标题
    /// </summary>
    [SugarColumn(ColumnName = "message_title", ColumnDescription = "消息标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MessageTitle { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [SugarColumn(ColumnName = "message_content", ColumnDescription = "消息内容", ColumnDataType = "ntext", IsNullable = false)]
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    [SugarColumn(ColumnName = "message_type", ColumnDescription = "消息类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public TaktMessageType MessageType { get; set; } = TaktMessageType.UserMessage;

    /// <summary>
    /// 消息分组
    /// </summary>
    [SugarColumn(ColumnName = "message_group", ColumnDescription = "消息分组", ColumnDataType = "int", IsNullable = true)]
    public TaktMessageGroup? MessageGroup { get; set; }

    /// <summary>
    /// 读取状态
    /// </summary>
    [SugarColumn(ColumnName = "read_status", ColumnDescription = "读取状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktMessageReadStatus ReadStatus { get; set; } = TaktMessageReadStatus.Unread;

    /// <summary>
    /// 读取时间
    /// </summary>
    [SugarColumn(ColumnName = "read_time", ColumnDescription = "读取时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    [SugarColumn(ColumnName = "send_time", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime SendTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    [SugarColumn(ColumnName = "message_ext_data", ColumnDescription = "消息扩展数据", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? MessageExtData { get; set; }
}
