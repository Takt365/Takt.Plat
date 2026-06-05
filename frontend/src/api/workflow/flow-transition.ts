// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/workflow
// 文件名称：flow-transition.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块 API（自动生成，请勿手改路由常量）
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
  FlowTransition,
  FlowTransitionCreate,
  FlowTransitionUpdate
} from '@/types/workflow/flow-transition';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowTransitions
 */
const FLOW_TRANSITION_API_BASE = 'TaktFlowTransitions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程流转历史列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowTransition>>} 分页结果
 */
export function getFlowTransitionList(queryDto: any): Promise<TaktPagedResult<FlowTransition>> {
  return request<TaktPagedResult<FlowTransition>>({
    url: `${FLOW_TRANSITION_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取流程流转历史
 * @param {string} id 流程流转历史ID
 * @returns {Promise<FlowTransition>} 流程流转历史DTO
 */
export function getFlowTransitionById(id: string): Promise<FlowTransition> {
  return request<FlowTransition>({
    url: `${FLOW_TRANSITION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程流转历史
 * @param {FlowTransitionCreate} dto 创建DTO
 * @returns {Promise<FlowTransition>} 流程流转历史DTO
 */
export function createFlowTransition(dto: FlowTransitionCreate): Promise<FlowTransition> {
  return request<FlowTransition>({
    url: `${FLOW_TRANSITION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程流转历史
 * @param {string} id 流程流转历史ID
 * @param {FlowTransitionUpdate} dto 更新DTO
 * @returns {Promise<FlowTransition>} 流程流转历史DTO
 */
export function updateFlowTransition(id: string, dto: FlowTransitionUpdate): Promise<FlowTransition> {
  return request<FlowTransition>({
    url: `${FLOW_TRANSITION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程流转历史
 * @param {string} id 流程流转历史ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowTransitionById(id: string): Promise<void> {
  return request({
    url: `${FLOW_TRANSITION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程流转历史
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowTransitionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_TRANSITION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程流转历史选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowTransitionOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${FLOW_TRANSITION_API_BASE}/options`,
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
export function getFlowTransitionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_TRANSITION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程流转历史
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowTransition(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_TRANSITION_API_BASE}/import`,
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
 * 导出流程流转历史
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowTransition(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_TRANSITION_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
