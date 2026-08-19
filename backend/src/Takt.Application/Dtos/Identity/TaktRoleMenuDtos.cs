// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktRoleMenuDtos.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktRoleMenu RBAC 关联 DTO（仅列表，分配走 TaktRbacsController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.Identity;

/// <summary>
/// 角色-菜单关联列表 DTO（对应 TaktRoleMenu）
/// </summary>
public class TaktRoleMenuDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// 关联主键（适配实体 Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleMenuId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色名称（填充字段）
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 菜单名称（填充字段）
    /// </summary>
    public string? MenuName { get; set; }
}
