// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktIncrementBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：库自增 bigint IDENTITY 主键 × 三种隔离（继承雪花基类，仅覆盖 Id 列映射）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户级实体基类（库自增主键）
/// </summary>
public abstract class TaktTenantEntityIncrementBase : TaktTenantEntityBase
{
    /// <summary>
    /// 主键 ID（库自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public override long Id { get; set; }
}

/// <summary>
/// 公司级实体基类（库自增主键）
/// </summary>
public abstract class TaktCompanyEntityIncrementBase : TaktCompanyEntityBase
{
    /// <summary>
    /// 主键 ID（库自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public override long Id { get; set; }
}

/// <summary>
/// 审批级实体基类（库自增主键）
/// </summary>
public abstract class TaktApprovalEntityIncrementBase : TaktApprovalEntityBase
{
    /// <summary>
    /// 主键 ID（库自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public override long Id { get; set; }
}
