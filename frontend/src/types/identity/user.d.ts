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

import type { TaktPagedQuery, TenantCultureDtoBase } from '@/types/common';

/**
 * 用户响应 DTO
 * 对应前端 TaktUserDto
 * 继承 TaktTenantCultureDtoBase（组合 2：无工厂、有语言）
 * 对应前端 User
 * @description 对应后端 TaktUserDto
 */
export interface User extends TenantCultureDtoBase {

  /**
   * 用户名（登录账号，20位）
   */
  username: string;

  /**
   * 昵称（显示名称）
   */
  nickname?: string;

  /**
   * 员工编码（用于查找员工）
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
   * 员工ID（可选，与员工编码二选一；填写时优先使用）
   */
  employeeId: string;

  /**
   * 状态（Enabled=启用，Disabled=禁用）
   */
  userStatus: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

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
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

