// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/identity
// 文件名称：users.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common';
import type {
  TaktChangePasswordDto,
  TaktCreateUserDto,
  TaktForgotPasswordDto,
  TaktForgotPasswordResultDto,
  TaktResetPasswordDto,
  TaktUpdateUserDto,
  TaktUserDto,
  TaktUserQueryDto,
  TaktUserStatusDto,
  TaktUserUnlockDto
} from '@/types/identity/user';
import type {
  TaktUserDataQueryDto
} from '@/types/identity/user-data-query';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktUsers
 */
const USER_API_BASE = 'TaktUsers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取用户列表（分页）
 * @param {TaktUserQueryDto} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaktUserDto>>} 分页结果
 */
export function getUserList(queryDto: TaktUserQueryDto): Promise<TaktPagedResult<TaktUserDto>> {
  return request<TaktPagedResult<TaktUserDto>>({
    url: `${USER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取用户
 * @param {string} id 用户ID
 * @returns {Promise<TaktUserDto>} 用户DTO
 */
export function getUserById(id: string): Promise<TaktUserDto> {
  return request<TaktUserDto>({
    url: `${USER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建用户
 * @param {TaktCreateUserDto} dto 创建用户DTO
 * @returns {Promise<TaktUserDto>} 用户DTO
 */
export function createUser(dto: TaktCreateUserDto): Promise<TaktUserDto> {
  return request<TaktUserDto>({
    url: `${USER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新用户
 * @param {string} id 用户ID
 * @param {TaktUpdateUserDto} dto 更新用户DTO
 * @returns {Promise<TaktUserDto>} 用户DTO
 */
export function updateUser(id: string, dto: TaktUpdateUserDto): Promise<TaktUserDto> {
  return request<TaktUserDto>({
    url: `${USER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除用户
 * @param {string} id 用户ID
 * @returns {Promise<void>} 任务
 */
export function deleteUserById(id: string): Promise<void> {
  return request({
    url: `${USER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除用户
 * @param {string[]} ids 用户ID列表
 * @returns {Promise<void>} 任务
 */
export function deleteUserBatch(ids: string[]): Promise<void> {
  return request({
    url: `${USER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新用户状态
 * @param {string} id 用户ID
 * @param {TaktUserStatusDto} dto 状态DTO
 * @returns {Promise<TaktUserDto>} 用户DTO
 */
export function updateUserStatus(id: string, dto: TaktUserStatusDto): Promise<TaktUserDto> {
  return request<TaktUserDto>({
    url: `${USER_API_BASE}/${id}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取用户选项列表（用于下拉框等）
 * @returns {Promise<TaktSelectOption[]>} 用户选项列表
 */
export function getUserOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${USER_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 密码与解锁
// ========================================

/**
 * 重置用户密码（管理员操作）
 * @param {string} id 用户ID
 * @param {TaktResetPasswordDto} dto 重置密码DTO
 * @returns {Promise<void>} 任务
 */
export function resetUserPassword(id: string, dto: TaktResetPasswordDto): Promise<void> {
  return request({
    url: `${USER_API_BASE}/${id}/reset-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 重置密码
 * @param {TaktResetPasswordDto} dto 重置密码DTO
 * @returns {Promise<void>} 任务
 */
export function resetPassword(dto: TaktResetPasswordDto): Promise<void> {
  return request({
    url: `${USER_API_BASE}/reset-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 修改密码（用户自己操作）
 * @param {TaktChangePasswordDto} dto 修改密码DTO
 * @returns {Promise<void>} 任务
 */
export function changePassword(dto: TaktChangePasswordDto): Promise<void> {
  return request({
    url: `${USER_API_BASE}/change-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 忘记密码（发送密码重置邮件）
 * @param {TaktForgotPasswordDto} dto 忘记密码DTO
 * @returns {Promise<TaktForgotPasswordResultDto>} 结果
 */
export function forgotPassword(dto: TaktForgotPasswordDto): Promise<TaktForgotPasswordResultDto> {
  return request<TaktForgotPasswordResultDto>({
    url: `${USER_API_BASE}/forgot-password`,
    method: 'post',
    data: dto,
  });
}

/**
 * 解锁用户
 * @param {TaktUserUnlockDto} dto 解锁用户DTO
 * @returns {Promise<TaktUserUnlockDto>} 用户DTO
 */
export function unlock(dto: TaktUserUnlockDto): Promise<TaktUserUnlockDto> {
  return request<TaktUserUnlockDto>({
    url: `${USER_API_BASE}/unlock`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 统计
// ========================================

/**
 * 统计用户总数
 * @returns {Promise<number>} 用户总数
 */
export function getUserCount(): Promise<number> {
  return request<number>({
    url: `${USER_API_BASE}/count`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName 工作表名称
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getUserTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${USER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入用户
 * @param {File} file Excel文件
 * @param {string} sheetName 工作表名称
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importUserData(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${USER_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出用户
 * @param {TaktUserDataQueryDto} queryDto 查询DTO
 * @param {string} sheetName 工作表名称
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportUserData(
  queryDto: TaktUserDataQueryDto,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${USER_API_BASE}/export`,
    method: 'get',
    params: {
      ...queryDto,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
