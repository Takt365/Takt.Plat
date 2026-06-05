// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktMenuDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Menu 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMenu 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Identity;

// ========================================
// Menu 响应 DTO
// ========================================

/// <summary>
/// 菜单实体 代表系统菜单和权限（树形结构） 支持目录、菜单、按钮三种类型
/// 对应前端 TaktMenuDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktMenuDto : TaktTenantDtoBase
{
    /// <summary>
    /// MenuID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级菜单，2=二级菜单，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    public string MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int MenuType { get; set; } = 0;

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    public string Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    public string ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int IsExternal { get; set; } = 0;

    /// <summary>
    /// 外部链接地址
    /// </summary>
    public string ExternalUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否缓存（前端keep-alive）
    /// </summary>
    public int IsCached { get; set; } = 0;

    /// <summary>
    /// 是否显示（0=隐藏，1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int MenuStatus { get; set; } = 0;

    /// <summary>
    /// 是否内置（1=是，0=否） 种子菜单为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 菜单描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）
    /// （子表：TaktRoleMenu）
    /// </summary>
    public List<TaktRoleMenuDto>? RoleMenus { get; set; }

}

// ========================================
// Menu 树形响应 DTO
// ========================================

/// <summary>
/// Menu 树形列表/树选择 DTO（含子节点）
/// 对应 GetMenuTreeAsync 等接口
/// </summary>
public class TaktMenuTreeDto : TaktMenuDto
{
    /// <summary>
    /// 子节点
    /// </summary>
    public List<TaktMenuTreeDto> Children { get; set; } = new();
}

// ========================================
// Menu 查询 DTO
// ========================================

/// <summary>
/// Menu 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMenuQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    public string? MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string? MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级菜单，2=二级菜单，以此类推）
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    public string? MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int? IsLeaf { get; set; }

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int? MenuType { get; set; }

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    public string? Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    public string? RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    public string? ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int? IsExternal { get; set; }

    /// <summary>
    /// 外部链接地址
    /// </summary>
    public string? ExternalUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否缓存（前端keep-alive）
    /// </summary>
    public int? IsCached { get; set; }

    /// <summary>
    /// 是否显示（0=隐藏，1=显示）
    /// </summary>
    public int? IsVisible { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? MenuStatus { get; set; }

    /// <summary>
    /// 是否内置（1=是，0=否） 种子菜单为内置，不允许删除
    /// </summary>
    public TaktYesNo? IsBuiltIn { get; set; }

    /// <summary>
    /// 菜单描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Menu DTO
// ========================================

/// <summary>
/// 创建Menu DTO
/// </summary>
public class TaktMenuCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    [Required(ErrorMessage = "菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）不能为空")]
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Required(ErrorMessage = "菜单名称不能为空")]
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    [Required(ErrorMessage = "本地化键（用于多语言支持）不能为空")]
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    [Required(ErrorMessage = "菜单图标不能为空")]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    [Required(ErrorMessage = "菜单路径（如：/100/1000/1001/，用于快速查询子菜单）不能为空")]
    public string MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int MenuType { get; set; } = 0;

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    [Required(ErrorMessage = "权限标识（格式：module:resource:action）不能为空")]
    public string Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    [Required(ErrorMessage = "路由地址（前端路由）不能为空")]
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    [Required(ErrorMessage = "组件路径（前端组件路径）不能为空")]
    public string ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int IsExternal { get; set; } = 0;

    /// <summary>
    /// 外部链接地址
    /// </summary>
    [Required(ErrorMessage = "外部链接地址不能为空")]
    public string ExternalUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否缓存（前端keep-alive）
    /// </summary>
    public int IsCached { get; set; } = 0;

    /// <summary>
    /// 是否显示（0=隐藏，1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int MenuStatus { get; set; } = 0;

    /// <summary>
    /// 是否内置（1=是，0=否） 种子菜单为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 菜单描述
    /// </summary>
    [Required(ErrorMessage = "菜单描述不能为空")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 拥有该菜单权限的角色 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Menu DTO
// ========================================

/// <summary>
/// 更新Menu DTO
/// 继承 TaktMenuCreateDto，添加 MenuId 字段
/// </summary>
public class TaktMenuUpdateDto : TaktMenuCreateDto
{
    /// <summary>
    /// MenuID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

}

// ========================================
// Menu 状态 DTO
// ========================================

/// <summary>
/// Menu 状态更新 DTO
/// </summary>
public class TaktMenuStatusDto
{
    /// <summary>
    /// MenuID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用，0=禁用）不能为空")]
    public int MenuStatus { get; set; } = 0;
}

// ========================================
// Menu 排序 DTO
// ========================================

/// <summary>
/// Menu 排序更新 DTO
/// </summary>
public class TaktMenuSortDto
{
    /// <summary>
    /// MenuID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    [Required(ErrorMessage = "排序号（同级菜单排序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Menu 导入模板行 DTO
/// </summary>
public class TaktMenuTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    public string? MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string? MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    public string? MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int? MenuType { get; set; }

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    public string? Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    public string? RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    public string? ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int? IsExternal { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Menu 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMenuImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    public string? MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string? MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    public string? I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    public string? MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int? MenuType { get; set; }

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    public string? Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    public string? RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    public string? ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int? IsExternal { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Menu 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMenuExportDto
{
    /// <summary>
    /// MenuID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 本地化键（用于多语言支持）
    /// </summary>
    public string I18nKey { get; set; } = string.Empty;

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级菜单，2=二级菜单，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    public string MenuPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（0=否，1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 菜单类型（与 <see cref="Takt.Shared.Enums.TaktMenuType"/> 一致：0=目录，1=页面菜单，2=按钮）
    /// </summary>
    public int MenuType { get; set; } = 0;

    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    public string Permission { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    public string ComponentPath { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（同级菜单排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否外部链接
    /// </summary>
    public int IsExternal { get; set; } = 0;

    /// <summary>
    /// 外部链接地址
    /// </summary>
    public string ExternalUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否缓存（前端keep-alive）
    /// </summary>
    public int IsCached { get; set; } = 0;

    /// <summary>
    /// 是否显示（0=隐藏，1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int MenuStatus { get; set; } = 0;

    /// <summary>
    /// 是否内置（1=是，0=否） 种子菜单为内置，不允许删除
    /// </summary>
    public TaktYesNo IsBuiltIn { get; set; }

    /// <summary>
    /// 菜单描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
