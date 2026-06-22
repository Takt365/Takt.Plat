// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：dict-data.ts
// 创建时间：2026-06-02
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  DictData,
  DictDataAll,
  DictDataCreate,
  DictDataSort,
  DictDataUpdate
} from '@/types/foundation/dict-data';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDictDatas
 */
const DICT_DATA_API_BASE = 'TaktDictDatas';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取字典数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DictData>>} 分页结果
 */
export function getDictDataList(queryDto: any): Promise<TaktPagedResult<DictData>> {
  return request<TaktPagedResult<DictData>>({
    url: `${DICT_DATA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取字典数据
 * @param {string} id 字典数据ID
 * @returns {Promise<DictData>} 字典数据DTO
 */
export function getDictDataById(id: string): Promise<DictData> {
  return request<DictData>({
    url: `${DICT_DATA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建字典数据
 * @param {DictDataCreate} dto 创建DTO
 * @returns {Promise<DictData>} 字典数据DTO
 */
export function createDictData(dto: DictDataCreate): Promise<DictData> {
  return request<DictData>({
    url: `${DICT_DATA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新字典数据
 * @param {string} id 字典数据ID
 * @param {DictDataUpdate} dto 更新DTO
 * @returns {Promise<DictData>} 字典数据DTO
 */
export function updateDictData(id: string, dto: DictDataUpdate): Promise<DictData> {
  return request<DictData>({
    url: `${DICT_DATA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除字典数据
 * @param {string} id 字典数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDictDataById(id: string): Promise<void> {
  return request({
    url: `${DICT_DATA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除字典数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDictDataBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DICT_DATA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新字典数据排序
 * @param {DictDataSort} dto 排序DTO
 * @returns {Promise<DictData>} 字典数据DTO
 */
export function updateDictDataSort(dto: DictDataSort): Promise<DictData> {
  return request<DictData>({
    url: `${DICT_DATA_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取字典数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDictDataOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${DICT_DATA_API_BASE}/options`,
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
export function getDictDataTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DICT_DATA_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入字典数据
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDictData(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DICT_DATA_API_BASE}/import`,
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
 * 导出字典数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDictData(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DICT_DATA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 租户全量字典
// ========================================

/**
 * 获取当前租户下全部字典数据（扁平列表，含 dictTypeCode）
 * @returns {Promise<TaktSelectOption[]>} 字典项列表
 */
export function getDictDataAll(): Promise<TaktSelectOption[]> {
  return request<DictDataAll>({
    url: `${DICT_DATA_API_BASE}/all`,
    method: 'get',
  }).then((dto) => dto.items ?? []);
}
