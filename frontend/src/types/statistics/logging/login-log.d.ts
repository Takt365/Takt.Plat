// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：login-log.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 登录日志实体
 * 对应前端 TaktLoginLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 LoginLog
 * @description 对应后端 TaktLoginLogDto
 */
export interface LoginLog extends CompanyDtoBase {
  /**
   * LoginLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  loginLogId: string;

  /**
   * 用户名（登录账号）
   */
  username: string;

  /**
   * 登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）
   */
  loginType?: string;

  /**
   * 浏览器类型
   */
  browser?: string;

  /**
   * 操作系统
   */
  os?: string;

  /**
   * 用户代理字符串（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult: number;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间
   */
  logoutAt?: string;

  /**
   * 会话时长（秒，从登录到登出的时长）
   */
  sessionDuration?: number;

}


/**
 * LoginLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 LoginLogQuery
 * @description 对应后端 TaktLoginLogQueryDto
 */
export interface LoginLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 用户名（登录账号）
   */
  username?: string;

  /**
   * 登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）
   */
  loginType?: string;

  /**
   * 浏览器类型
   */
  browser?: string;

  /**
   * 操作系统
   */
  os?: string;

  /**
   * 用户代理字符串（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult?: number;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间（范围查询-开始）
   */
  logoutAtStart?: string;

  /**
   * 登出时间（范围查询-结束）
   */
  logoutAtEnd?: string;

  /**
   * 会话时长（秒，从登录到登出的时长）
   */
  sessionDuration?: number;

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
 * 创建LoginLog DTO
 * 对应前端 LoginLogCreate
 * @description 对应后端 TaktLoginLogCreateDto
 */
export interface LoginLogCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 用户名（登录账号）
   */
  username: string;

  /**
   * 登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）
   */
  loginType?: string;

  /**
   * 浏览器类型
   */
  browser?: string;

  /**
   * 操作系统
   */
  os?: string;

  /**
   * 用户代理字符串（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult: number;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间
   */
  logoutAt?: string;

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
 * 更新LoginLog DTO
 * 继承 TaktLoginLogCreateDto，添加 LoginLogId 字段
 * 对应前端 LoginLogUpdate
 * @description 对应后端 TaktLoginLogUpdateDto
 */
export interface LoginLogUpdate extends LoginLogCreate {
  /**
   * LoginLogID（标识要更新的实体）
   */
  loginLogId: string;

}


/**
 * LoginLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 LoginLogExport
 * @description 对应后端 TaktLoginLogExportDto
 */
export interface LoginLogExport {
  /**
   * LoginLogID
   */
  loginLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号）
   */
  username: string;

  /**
   * 登录方式（如：Password=账号密码，RefreshToken=刷新令牌，Sms=手机验证码，Email=邮箱验证码）
   */
  loginType?: string;

  /**
   * 浏览器类型
   */
  browser?: string;

  /**
   * 操作系统
   */
  os?: string;

  /**
   * 用户代理字符串（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult: number;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间
   */
  logoutAt?: string;

  /**
   * 会话时长（秒，从登录到登出的时长）
   */
  sessionDuration?: number;

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

