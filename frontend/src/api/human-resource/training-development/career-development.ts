// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/training-development
// 文件名称：career-development.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training-development 模块 API（自动生成，请勿手改路由常量）
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
  CareerDevelopment,
  CareerDevelopmentCreate,
  CareerDevelopmentStatus,
  CareerDevelopmentUpdate
} from '@/types/human-resource/training-development/career-development';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCareerDevelopments
 */
const CAREER_DEVELOPMENT_API_BASE = 'TaktCareerDevelopments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取职业发展列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CareerDevelopment>>} 分页结果
 */
export function getCareerDevelopmentList(queryDto: any): Promise<TaktPagedResult<CareerDevelopment>> {
  return request<TaktPagedResult<CareerDevelopment>>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取职业发展
 * @param {string} id 职业发展ID
 * @returns {Promise<CareerDevelopment>} 职业发展DTO
 */
export function getCareerDevelopmentById(id: string): Promise<CareerDevelopment> {
  return request<CareerDevelopment>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建职业发展
 * @param {CareerDevelopmentCreate} dto 创建DTO
 * @returns {Promise<CareerDevelopment>} 职业发展DTO
 */
export function createCareerDevelopment(dto: CareerDevelopmentCreate): Promise<CareerDevelopment> {
  return request<CareerDevelopment>({
    url: `${CAREER_DEVELOPMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新职业发展
 * @param {string} id 职业发展ID
 * @param {CareerDevelopmentUpdate} dto 更新DTO
 * @returns {Promise<CareerDevelopment>} 职业发展DTO
 */
export function updateCareerDevelopment(id: string, dto: CareerDevelopmentUpdate): Promise<CareerDevelopment> {
  return request<CareerDevelopment>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除职业发展
 * @param {string} id 职业发展ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCareerDevelopmentById(id: string): Promise<void> {
  return request({
    url: `${CAREER_DEVELOPMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除职业发展
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCareerDevelopmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CAREER_DEVELOPMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新职业发展状态
 * @param {CareerDevelopmentStatus} dto 状态DTO
 * @returns {Promise<CareerDevelopment>} 职业发展DTO
 */
export function updateCareerDevelopmentStatus(dto: CareerDevelopmentStatus): Promise<CareerDevelopment> {
  return request<CareerDevelopment>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取职业发展选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCareerDevelopmentOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/options`,
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
export function getCareerDevelopmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入职业发展
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCareerDevelopment(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CAREER_DEVELOPMENT_API_BASE}/import`,
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
 * 导出职业发展
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCareerDevelopment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CAREER_DEVELOPMENT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
