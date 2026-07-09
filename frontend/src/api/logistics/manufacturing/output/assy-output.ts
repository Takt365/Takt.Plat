// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：assy-output.ts
// 创建时间：2026-07-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  AssyOutput,
  AssyOutputCreate,
  AssyOutputUpdate
} from '@/types/logistics/manufacturing/output/assy-output';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssyOutputs
 */
const ASSY_OUTPUT_API_BASE = 'TaktAssyOutputs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取组立日报列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AssyOutput>>} 分页结果
 */
export function getAssyOutputList(queryDto: any): Promise<TaktPagedResult<AssyOutput>> {
  return request<TaktPagedResult<AssyOutput>>({
    url: `${ASSY_OUTPUT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取组立日报
 * @param {string} id 组立日报ID
 * @returns {Promise<AssyOutput>} 组立日报DTO
 */
export function getAssyOutputById(id: string): Promise<AssyOutput> {
  return request<AssyOutput>({
    url: `${ASSY_OUTPUT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建组立日报
 * @param {AssyOutputCreate} dto 创建DTO
 * @returns {Promise<AssyOutput>} 组立日报DTO
 */
export function createAssyOutput(dto: AssyOutputCreate): Promise<AssyOutput> {
  return request<AssyOutput>({
    url: `${ASSY_OUTPUT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新组立日报
 * @param {string} id 组立日报ID
 * @param {AssyOutputUpdate} dto 更新DTO
 * @returns {Promise<AssyOutput>} 组立日报DTO
 */
export function updateAssyOutput(id: string, dto: AssyOutputUpdate): Promise<AssyOutput> {
  return request<AssyOutput>({
    url: `${ASSY_OUTPUT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除组立日报
 * @param {string} id 组立日报ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyOutputById(id: string): Promise<void> {
  return request({
    url: `${ASSY_OUTPUT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除组立日报
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyOutputBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSY_OUTPUT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取组立日报选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssyOutputOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ASSY_OUTPUT_API_BASE}/options`,
    method: 'get',
  });
}

/**
 * 获取组立日报新增时固定的生产时段列表（13 条）
 * @returns {Promise<string[]>} 生产时段列表
 */
export function getAssyOutputDefaultTimePeriods(): Promise<string[]> {
  return request<string[]>({
    url: `${ASSY_OUTPUT_API_BASE}/default-time-periods`,
    method: 'get',
  });
}

/**
 * 获取组立不良日报新增用工单选项（来源已生产的组立日报，排除同日同工单已存在不良日报）
 * @param {string} [excludeAssyDefectId] 编辑态当前不良日报 ID
 * @returns {Promise<TaktSelectOption[]>} 下拉选项（DictValue 为组立日报 Id）
 */
export function getAssyOutputProdOrderOptions(excludeAssyDefectId?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ASSY_OUTPUT_API_BASE}/prod-order-options`,
    method: 'get',
    params: excludeAssyDefectId ? { excludeAssyDefectId } : undefined,
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
export function getAssyOutputTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_OUTPUT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入组立日报
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAssyOutput(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSY_OUTPUT_API_BASE}/import`,
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
 * 导出组立日报
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssyOutput(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_OUTPUT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
