// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/identity
// 文件名称：auth.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktSelectOption
} from '@/types/common';
import type {
  TaktLoginRequestDto,
  TaktSessionVerifyPasswordRequestDto,
  TaktUserInfoResponseDto
} from '@/types/identity/login';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAuths
 */
const AUTH_API_BASE = 'TaktAuths';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 建立 Cookie 登录会话
 * @param {TaktLoginRequestDto} dto 登录请求
 * @returns {Promise<boolean>} 是否成功
 */
export function signInSession(dto: TaktLoginRequestDto): Promise<boolean> {
  return request<boolean>({
    url: `${AUTH_API_BASE}/session/signin`,
    method: 'post',
    data: dto,
  });
}

/**
 * 清除 Cookie 登录会话
 * @returns {Promise<boolean>} 是否成功
 */
export function signOutSession(): Promise<boolean> {
  return request<boolean>({
    url: `${AUTH_API_BASE}/session/signout`,
    method: 'post',
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}

/**
 * 生成登录验证码挑战（匿名）
 * @returns {Promise<unknown>} 验证码挑战
 */
export function getSessionCaptcha(): Promise<unknown> {
  return request({
    url: `${AUTH_API_BASE}/session/captcha`,
    method: 'get',
  });
}

/**
 * 获取当前登录用户资料（权限、菜单、路由）
 * @returns {Promise<TaktUserInfoResponseDto>} 用户资料
 */
export function getCurrentUser(): Promise<TaktUserInfoResponseDto> {
  return request<TaktUserInfoResponseDto>({
    url: `${AUTH_API_BASE}/me`,
    method: 'get',
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取登录页租户选项（匿名；返回全部租户，登录时按用户校验租户权限）
 * @returns {Promise<TaktSelectOption[]>} 租户下拉选项
 */
export function getSessionTenantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${AUTH_API_BASE}/session/tenant-options`,
    method: 'get',
  });
}

/**
 * 获取当前用户可切换的公司选项（已登录，按权限过滤）
 * @returns {Promise<TaktSelectOption[]>} 公司下拉选项
 */
export function getUserCompanyOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${AUTH_API_BASE}/me/company-options`,
    method: 'get',
  });
}

// ========================================
// 密码与解锁
// ========================================

/**
 * 登录预检：① 租户下是否存在可登录用户（不验密）→ ② 验密 → 返回是否需验证码与登录票据
 * @param {TaktSessionVerifyPasswordRequestDto} dto 校验请求
 * @returns {Promise<unknown>} 校验结果
 */
export function verifySessionPassword(dto: TaktSessionVerifyPasswordRequestDto): Promise<unknown> {
  return request({
    url: `${AUTH_API_BASE}/session/verify-password`,
    method: 'post',
    data: dto,
  });
}
