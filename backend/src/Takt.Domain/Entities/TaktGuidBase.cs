// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktGuidBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：GUID 主键 × 三种隔离
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户级实体基类（GUID 主键）
/// </summary>
public abstract class TaktTenantEntityGuidBase : TaktTenantEntityScopeBase
{
    /// <summary>
    /// 主键 ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false)]
    public Guid Id { get; set; }
}

/// <summary>
/// 公司级实体基类（GUID 主键）
/// </summary>
public abstract class TaktCompanyEntityGuidBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键 ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false)]
    public Guid Id { get; set; }
}

/// <summary>
/// 审批级实体基类（GUID 主键）
/// </summary>
public abstract class TaktApprovalEntityGuidBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键 ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false)]
    public Guid Id { get; set; }
}
