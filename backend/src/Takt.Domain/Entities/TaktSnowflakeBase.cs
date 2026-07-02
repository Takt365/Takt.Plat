// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktSnowflakeBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：雪花 bigint 主键 × 三种隔离（默认业务实体基类名保持不变）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户级实体基类（雪花主键）
/// </summary>
public abstract class TaktTenantEntityBase : TaktTenantEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 公司级实体基类（雪花主键）
/// </summary>
public abstract class TaktCompanyEntityBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 审批级实体基类（雪花主键）
/// </summary>
public abstract class TaktApprovalEntityBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键 ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}
