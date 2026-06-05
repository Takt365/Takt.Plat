// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/identity
// 文件名称：user.d.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common';

/**
 * 用户响应 DTO
 * 对应前端 TaktUserDto
 * 继承 TaktTenantDtoBase（租户级实体）
 * 对应前端 User
 * @description 对应后端 TaktUserDto
 */
export interface User extends TenantDtoBase {
  /**
   * 用户ID（适配字段，序列化为string以避免Javascript精度问题）
   */
  userId: string;

  /**
   * 用户名（登录账号）
   */
  username: string;

  /**
   * 昵称（显示名称）
   */
  nickname?: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 密码哈希值（加密后，不返回明文）
   */
  passwordHash: string;

  /**
   * 关联的员工ID
   */
  employeeId: string;

  /**
   * 员工姓名（填充字段）
   */
  employeeName?: string;

  /**
   * 状态
   */
  userStatus: number;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount: number;

  /**
   * 密码过期天数（0=永不过期）
   */
  passwordExpireDays: number;

  /**
   * 失败登录次数
   */
  loginFailCount: number;

  /**
   * 锁定时间
   */
  lockedUntil?: string;

  /**
   * 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
   */
  defaultCulture: string;

  /**
   * 已分配角色 ID 列表
   */
  roleIds?: string[];

  /**
   * 已分配角色名称列表
   */
  roleNames?: string[];

  /**
   * 可访问公司编码列表
   */
  companyCodes?: string[];

}


/**
 * 用户分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 UserQuery
 * @description 对应后端 TaktUserQueryDto
 */
export interface UserQuery extends TaktPagedQuery {
  /**
   * 用户名（模糊查询）
   */
  username?: string;

  /**
   * 昵称（模糊查询）
   */
  nickname?: string;

  /**
   * 用户类型
   */
  userType?: number;

  /**
   * 关联的员工ID
   */
  employeeId?: string;

  /**
   * 状态
   */
  userStatus?: number;

  /**
   * 默认区域文化编码（模糊查询）
   */
  defaultCulture?: string;

  /**
   * 创建人ID
   */
  createdBy?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建用户 DTO
 * 对应前端 CreateUser
 * @description 对应后端 TaktCreateUserDto
 */
export interface CreateUser {
  /**
   * 用户名（登录账号，20位）
   */
  username: string;

  /**
   * 昵称（显示名称，20位）
   */
  nickname?: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 密码哈希值（加密后的密码）
   */
  passwordHash: string;

  /**
   * 关联的员工ID（必填）
   */
  employeeId: string;

  /**
   * 状态
   */
  userStatus: number;

  /**
   * 默认区域文化编码（BCP47，对齐 TaktCulture.CultureCode）
   */
  defaultCulture: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 角色 ID 列表（全量覆盖；不传则创建时不分配角色）
   */
  roleIds?: string[];

  /**
   * 公司编码列表（全量覆盖；不传则不改公司范围）
   */
  companyCodes?: string[];

}


/**
 * 更新用户 DTO
 * 继承 TaktCreateUserDto，添加 UserId 字段
 * 对应前端 UpdateUser
 * @description 对应后端 TaktUpdateUserDto
 */
export interface UpdateUser extends CreateUser {
  /**
   * 用户ID（标识要更新的实体）
   */
  userId: string;

}


/**
 * 用户状态更新 DTO
 * 对应前端 UserStatus
 * @description 对应后端 TaktUserStatusDto
 */
export interface UserStatus {
  /**
   * 用户ID
   */
  userId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  userStatus: number;

}


/**
 * 重置密码 DTO（管理员重置指定用户密码，或按 UserId 重置）
 * 对应前端 ResetPassword
 * @description 对应后端 TaktResetPasswordDto
 */
export interface ResetPassword {
  /**
   * 用户ID
   */
  userId: string;

  /**
   * 新密码
   */
  newPassword: string;

}


/**
 * 修改密码 DTO（用户修改自己的密码）
 * 对应前端 ChangePassword
 * @description 对应后端 TaktChangePasswordDto
 */
export interface ChangePassword {
  /**
   * 旧密码
   */
  oldPassword: string;

  /**
   * 新密码
   */
  newPassword: string;

  /**
   * 确认新密码
   */
  confirmPassword: string;

}


/**
 * 忘记密码 DTO
 * 对应前端 ForgotPassword
 * @description 对应后端 TaktForgotPasswordDto
 */
export interface ForgotPassword {
  /**
   * 用户名或邮箱
   */
  usernameOrEmail: string;

}


/**
 * 忘记密码结果 DTO
 * 对应前端 ForgotPasswordResult
 * @description 对应后端 TaktForgotPasswordResultDto
 */
export interface ForgotPasswordResult {
  /**
   * 是否成功
   */
  success: boolean;

  /**
   * 错误码（Success 为 false 时有效） EmailNotFound = 邮箱未找到, ProtectedUser = 保护用户
   */
  code?: string;

  /**
   * 错误信息
   */
  message?: string;

}


/**
 * 解锁用户 DTO
 * 对应前端 UserUnlock
 * @description 对应后端 TaktUserUnlockDto
 */
export interface UserUnlock {
  /**
   * 用户ID
   */
  userId: string;

  /**
   * 解锁原因
   */
  reason?: string;

}


/**
 * 用户导入模板 DTO（用于生成 Excel 导入模板）
 * 对应前端 UserTemplate
 * @description 对应后端 TaktUserTemplateDto
 */
export interface UserTemplate {
  /**
   * 用户名（登录账号，20位）
   */
  username: string;

  /**
   * 昵称（显示名称）
   */
  nickname?: string;

  /**
   * 用户类型（Normal=普通用户，Admin=管理员，SuperAdmin=超级管理员）
   */
  userType: number;

  /**
   * 初始密码（Excel 填明文；留空时导入使用系统默认密码）
   */
  passwordHash?: string;

  /**
   * 员工ID
   */
  employeeId: string;

  /**
   * 状态（Enabled=启用，Disabled=禁用）
   */
  userStatus: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 用户导入 DTO（Excel 导入数据）
 * 对应前端 UserImport
 * @description 对应后端 TaktUserImportDto
 */
export interface UserImport {
  /**
   * 用户名（登录账号，20位）
   */
  username: string;

  /**
   * 昵称（显示名称）
   */
  nickname?: string;

  /**
   * 员工编号（用于查找员工）
   */
  employeeCode: string;

  /**
   * 用户类型（Normal=普通用户，Admin=管理员，SuperAdmin=超级管理员）
   */
  userType: number;

  /**
   * 初始密码（Excel 填明文；留空时导入使用系统默认密码）
   */
  passwordHash?: string;

  /**
   * 员工ID（可选，与员工编号二选一；填写时优先使用）
   */
  employeeId: string;

  /**
   * 状态（Enabled=启用，Disabled=禁用）
   */
  userStatus: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 用户导出 DTO
 * 对应前端 UserExport
 * @description 对应后端 TaktUserExportDto
 */
export interface UserExport {
  /**
   * 用户ID（适配字段，序列化为string以避免Javascript精度问题）
   */
  userId: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 用户名（登录账号）
   */
  username: string;

  /**
   * 昵称（显示名称）
   */
  nickname?: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 用户类型名称（导出用）
   */
  userTypeName: string;

  /**
   * 员工ID
   */
  employeeId: string;

  /**
   * 员工姓名（导出用）
   */
  employeeName: string;

  /**
   * 状态
   */
  userStatus: number;

  /**
   * 状态名称（导出用）
   */
  statusName: string;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount: number;

  /**
   * 密码过期天数
   */
  passwordExpireDays: number;

  /**
   * 失败登录次数
   */
  loginFailCount: number;

  /**
   * 锁定时间
   */
  lockedUntil?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

