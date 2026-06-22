// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktUserDtos.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户管理相关 DTO（查询、创建、更新、状态、导入、导出）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Identity;

// ========================================
// 用户响应 DTO
// ========================================

/// <summary>
/// 用户响应 DTO
/// 对应前端 TaktUserDto
/// 继承 TaktTenantDtoBase（租户级实体）
/// </summary>
public class TaktUserDto : TaktTenantDtoBase
{
    /// <summary>
    /// 用户ID（适配字段，序列化为string以避免Javascript精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public int UserType { get; set; }

    /// <summary>
    /// 密码哈希值（加密后，不返回明文）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int UserStatus { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; }

    /// <summary>
    /// 密码过期天数（0=永不过期）
    /// </summary>
    public int PasswordExpireDays { get; set; }

    /// <summary>
    /// 失败登录次数
    /// </summary>
    public int LoginFailCount { get; set; }

    /// <summary>
    /// 锁定时间
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 已分配角色 ID 列表（查询填充）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 已分配角色名称列表（查询填充）
    /// </summary>
    public List<string>? RoleNames { get; set; }

    /// <summary>
    /// 可访问公司编码列表（查询填充）
    /// </summary>
    public string[]? CompanyCodes { get; set; }
}

// ========================================
// 用户查询 DTO
// ========================================

/// <summary>
/// 用户分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktUserQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 用户名（模糊查询）
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// 昵称（模糊查询）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public int? UserType { get; set; }

    /// <summary>
    /// 关联的员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? UserStatus { get; set; }

    /// <summary>
    /// 默认区域文化编码（模糊查询）
    /// </summary>
    public string? DefaultCulture { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CreatedBy { get; set; }

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建用户 DTO
// ========================================

/// <summary>
/// 创建用户 DTO
/// </summary>
public class TaktCreateUserDto
{
    /// <summary>
    /// 用户名（登录账号，20位）
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，20位）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 密码哈希值（加密后的密码）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int UserStatus { get; set; } = 1;

    /// <summary>
    /// 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 角色 ID 列表（全量覆盖，分配走 ITaktRbacService.AssignUserRolesAsync；null 表示创建/更新时不改角色）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问公司编码列表（全量覆盖，分配走 ITaktRbacService.AssignUserCompaniesAsync；null 表示不改公司范围）
    /// </summary>
    public string[]? CompanyCodes { get; set; }
}

// ========================================
// 更新用户 DTO
// ========================================

/// <summary>
/// 更新用户 DTO
/// 继承 TaktCreateUserDto，添加 UserId 字段
/// </summary>
public class TaktUpdateUserDto : TaktCreateUserDto
{
    /// <summary>
    /// 用户ID（标识要更新的实体）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
}

// ========================================
// 用户状态 DTO
// ========================================

/// <summary>
/// 用户状态更新 DTO
/// </summary>
public class TaktUserStatusDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int UserStatus { get; set; }
}

// ========================================
// 密码管理 DTO
// ========================================

/// <summary>
/// 重置密码 DTO（管理员重置指定用户密码，或按 UserId 重置）
/// </summary>
public class TaktResetPasswordDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 修改密码 DTO（用户修改自己的密码）
/// </summary>
public class TaktChangePasswordDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// 确认新密码
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 忘记密码 DTO
/// </summary>
public class TaktForgotPasswordDto
{
    /// <summary>
    /// 用户名或邮箱
    /// </summary>
    public string UsernameOrEmail { get; set; } = string.Empty;
}

/// <summary>
/// 忘记密码结果 DTO
/// </summary>
public class TaktForgotPasswordResultDto
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误码（Success 为 false 时有效）
    /// EmailNotFound = 邮箱未找到, ProtectedUser = 保护用户
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// 解锁用户 DTO
/// </summary>
public class TaktUserUnlockDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 解锁原因
    /// </summary>
    public string? Reason { get; set; }
}

// ========================================
// 用户导入模板 DTO
// ========================================

/// <summary>
/// 用户导入模板 DTO（用于生成 Excel 导入模板）
/// </summary>
public class TaktUserTemplateDto
{
    /// <summary>
    /// 用户名（登录账号，20位）
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户类型数值（Excel 填 0/1/2；与 UserTypeName 二选一，文本列优先）
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 用户类型名称（Excel 填文本时优先；字典 sys_user_type DictLabel）
    /// </summary>
    public string? UserTypeName { get; set; }

    /// <summary>
    /// 初始密码（Excel 填明文；留空时导入使用系统默认密码）
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 状态数值（Excel 填 0/1；与 StatusName 二选一，文本列优先）
    /// </summary>
    public int UserStatus { get; set; } = 1;

    /// <summary>
    /// 状态名称（Excel 填文本时优先；字典 sys_yes_no_type DictLabel）
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 用户导入 DTO
// ========================================

/// <summary>
/// 用户导入 DTO（Excel 导入数据）
/// </summary>
public class TaktUserImportDto
{
    /// <summary>
    /// 用户名（登录账号，20位）
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 员工编号（用于查找员工）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型数值（Excel 填 0/1/2；与 UserTypeName 二选一，文本列优先）
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 用户类型名称（Excel 填文本时优先；字典 sys_user_type DictLabel）
    /// </summary>
    public string? UserTypeName { get; set; }

    /// <summary>
    /// 初始密码（Excel 填明文；留空时导入使用系统默认密码）
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// 员工ID（可选，与员工编号二选一；填写时优先使用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 状态数值（Excel 填 0/1；与 StatusName 二选一，文本列优先）
    /// </summary>
    public int UserStatus { get; set; } = 1;

    /// <summary>
    /// 状态名称（Excel 填文本时优先；字典 sys_yes_no_type DictLabel）
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 用户导出 DTO
// ========================================

/// <summary>
/// 用户导出 DTO
/// </summary>
public class TaktUserExportDto
{
    /// <summary>
    /// 用户ID（适配字段，序列化为string以避免Javascript精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称）
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public int UserType { get; set; }

    /// <summary>
    /// 用户类型名称（导出用；字典 sys_user_type）
    /// </summary>
    [TaktDictType("sys_user_type")]
    public string UserTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 员工ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（导出用）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称（导出用，逗号分隔）
    /// </summary>
    public string RoleNames { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    public int UserStatus { get; set; }

    /// <summary>
    /// 状态名称（导出用；字典 sys_yes_no_type）
    /// </summary>
    [TaktDictType("sys_yes_no_type")]
    public string StatusName { get; set; } = string.Empty;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; }

    /// <summary>
    /// 密码过期天数
    /// </summary>
    public int PasswordExpireDays { get; set; }

    /// <summary>
    /// 失败登录次数
    /// </summary>
    public int LoginFailCount { get; set; }

    /// <summary>
    /// 锁定时间
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
