// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/compensation-benefits
// 文件名称：social-security.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation-benefits 模块 API（自动生成，请勿手改路由常量）
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
  SocialSecurity,
  SocialSecurityCreate,
  SocialSecurityStatus,
  SocialSecurityUpdate
} from '@/types/human-resource/compensation-benefits/social-security';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSocialSecurities
 */
const SOCIAL_SECURITY_API_BASE = 'TaktSocialSecurities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取社保缴纳列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SocialSecurity>>} 分页结果
 */
export function getSocialSecurityList(queryDto: any): Promise<TaktPagedResult<SocialSecurity>> {
  return request<TaktPagedResult<SocialSecurity>>({
    url: `${SOCIAL_SECURITY_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取社保缴纳
 * @param {string} id 社保缴纳ID
 * @returns {Promise<SocialSecurity>} 社保缴纳DTO
 */
export function getSocialSecurityById(id: string): Promise<SocialSecurity> {
  return request<SocialSecurity>({
    url: `${SOCIAL_SECURITY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建社保缴纳
 * @param {SocialSecurityCreate} dto 创建DTO
 * @returns {Promise<SocialSecurity>} 社保缴纳DTO
 */
export function createSocialSecurity(dto: SocialSecurityCreate): Promise<SocialSecurity> {
  return request<SocialSecurity>({
    url: `${SOCIAL_SECURITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新社保缴纳
 * @param {string} id 社保缴纳ID
 * @param {SocialSecurityUpdate} dto 更新DTO
 * @returns {Promise<SocialSecurity>} 社保缴纳DTO
 */
export function updateSocialSecurity(id: string, dto: SocialSecurityUpdate): Promise<SocialSecurity> {
  return request<SocialSecurity>({
    url: `${SOCIAL_SECURITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除社保缴纳
 * @param {string} id 社保缴纳ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSocialSecurityById(id: string): Promise<void> {
  return request({
    url: `${SOCIAL_SECURITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除社保缴纳
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSocialSecurityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOCIAL_SECURITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新社保缴纳状态
 * @param {SocialSecurityStatus} dto 状态 DTO
 * @returns {Promise<SocialSecurity>} 社保缴纳DTO
 */
export function updateSocialSecurityStatus(dto: SocialSecurityStatus): Promise<SocialSecurity> {
  return request<SocialSecurity>({
    url: `${SOCIAL_SECURITY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取社保缴纳选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSocialSecurityOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SOCIAL_SECURITY_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getSocialSecurityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOCIAL_SECURITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入社保缴纳
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSocialSecurity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOCIAL_SECURITY_API_BASE}/import`,
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
 * 导出社保缴纳
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSocialSecurity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOCIAL_SECURITY_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
