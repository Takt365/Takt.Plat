// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：assessment.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块 API（自动生成，请勿手改路由常量）
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
  Assessment,
  AssessmentCreate,
  AssessmentStatus,
  AssessmentUpdate
} from '@/types/human-resource/performance/assessment';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssessments
 */
const ASSESSMENT_API_BASE = 'TaktAssessments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效考核列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Assessment>>} 分页结果
 */
export function getAssessmentList(queryDto: any): Promise<TaktPagedResult<Assessment>> {
  return request<TaktPagedResult<Assessment>>({
    url: `${ASSESSMENT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取绩效考核
 * @param {string} id 绩效考核ID
 * @returns {Promise<Assessment>} 绩效考核DTO
 */
export function getAssessmentById(id: string): Promise<Assessment> {
  return request<Assessment>({
    url: `${ASSESSMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效考核
 * @param {AssessmentCreate} dto 创建DTO
 * @returns {Promise<Assessment>} 绩效考核DTO
 */
export function createAssessment(dto: AssessmentCreate): Promise<Assessment> {
  return request<Assessment>({
    url: `${ASSESSMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效考核
 * @param {string} id 绩效考核ID
 * @param {AssessmentUpdate} dto 更新DTO
 * @returns {Promise<Assessment>} 绩效考核DTO
 */
export function updateAssessment(id: string, dto: AssessmentUpdate): Promise<Assessment> {
  return request<Assessment>({
    url: `${ASSESSMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效考核
 * @param {string} id 绩效考核ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssessmentById(id: string): Promise<void> {
  return request({
    url: `${ASSESSMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效考核
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssessmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSESSMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效考核状态
 * @param {AssessmentStatus} dto 状态DTO
 * @returns {Promise<Assessment>} 绩效考核DTO
 */
export function updateAssessmentStatus(dto: AssessmentStatus): Promise<Assessment> {
  return request<Assessment>({
    url: `${ASSESSMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效考核选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssessmentOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ASSESSMENT_API_BASE}/options`,
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
export function getAssessmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSESSMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效考核
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAssessment(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSESSMENT_API_BASE}/import`,
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
 * 导出绩效考核
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssessment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSESSMENT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
