// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：material.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  Material,
  MaterialCreate,
  MaterialStatus,
  MaterialUpdate
} from '@/types/logistics/materials/material';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterials
 */
const MATERIAL_API_BASE = 'TaktMaterials';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Material>>} 分页结果
 */
export function getMaterialList(queryDto: any): Promise<TaktPagedResult<Material>> {
  return request<TaktPagedResult<Material>>({
    url: `${MATERIAL_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取物料
 * @param {string} id 物料ID
 * @returns {Promise<Material>} 物料DTO
 */
export function getMaterialById(id: string): Promise<Material> {
  return request<Material>({
    url: `${MATERIAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料
 * @param {MaterialCreate} dto 创建DTO
 * @returns {Promise<Material>} 物料DTO
 */
export function createMaterial(dto: MaterialCreate): Promise<Material> {
  return request<Material>({
    url: `${MATERIAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料
 * @param {string} id 物料ID
 * @param {MaterialUpdate} dto 更新DTO
 * @returns {Promise<Material>} 物料DTO
 */
export function updateMaterial(id: string, dto: MaterialUpdate): Promise<Material> {
  return request<Material>({
    url: `${MATERIAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料
 * @param {string} id 物料ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料状态
 * @param {MaterialStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Material>} 物料DTO
 */
export function updateMaterialStatus(dto: MaterialStatus): Promise<Material> {
  return request<Material>({
    url: `${MATERIAL_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_API_BASE}/options`,
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
export function getMaterialTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterial(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_API_BASE}/import`,
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
 * 导出物料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterial(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
