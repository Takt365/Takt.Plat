// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/talent
// 文件名称：talent-interview.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块 API（自动生成，请勿手改路由常量）
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
  TalentInterview,
  TalentInterviewCreate,
  TalentInterviewStatus,
  TalentInterviewUpdate
} from '@/types/human-resource/talent/talent-interview';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTalentInterviews
 */
const TALENT_INTERVIEW_API_BASE = 'TaktTalentInterviews';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取面试安排列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TalentInterview>>} 分页结果
 */
export function getTalentInterviewList(queryDto: any): Promise<TaktPagedResult<TalentInterview>> {
  return request<TaktPagedResult<TalentInterview>>({
    url: `${TALENT_INTERVIEW_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取面试安排
 * @param {string} id 面试安排ID
 * @returns {Promise<TalentInterview>} 面试安排DTO
 */
export function getTalentInterviewById(id: string): Promise<TalentInterview> {
  return request<TalentInterview>({
    url: `${TALENT_INTERVIEW_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建面试安排
 * @param {TalentInterviewCreate} dto 创建DTO
 * @returns {Promise<TalentInterview>} 面试安排DTO
 */
export function createTalentInterview(dto: TalentInterviewCreate): Promise<TalentInterview> {
  return request<TalentInterview>({
    url: `${TALENT_INTERVIEW_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新面试安排
 * @param {string} id 面试安排ID
 * @param {TalentInterviewUpdate} dto 更新DTO
 * @returns {Promise<TalentInterview>} 面试安排DTO
 */
export function updateTalentInterview(id: string, dto: TalentInterviewUpdate): Promise<TalentInterview> {
  return request<TalentInterview>({
    url: `${TALENT_INTERVIEW_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除面试安排
 * @param {string} id 面试安排ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentInterviewById(id: string): Promise<void> {
  return request({
    url: `${TALENT_INTERVIEW_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除面试安排
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentInterviewBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TALENT_INTERVIEW_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新面试安排状态
 * @param {TalentInterviewStatus} dto 状态DTO
 * @returns {Promise<TalentInterview>} 面试安排DTO
 */
export function updateTalentInterviewStatus(dto: TalentInterviewStatus): Promise<TalentInterview> {
  return request<TalentInterview>({
    url: `${TALENT_INTERVIEW_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取面试安排选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTalentInterviewOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TALENT_INTERVIEW_API_BASE}/options`,
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
export function getTalentInterviewTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_INTERVIEW_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入面试安排
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTalentInterview(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TALENT_INTERVIEW_API_BASE}/import`,
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
 * 导出面试安排
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTalentInterview(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_INTERVIEW_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
