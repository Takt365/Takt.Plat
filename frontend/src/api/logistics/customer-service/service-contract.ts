// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：service-contract.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块 API（自动生成，请勿手改路由常量）
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
  ServiceContract,
  ServiceContractCreate,
  ServiceContractSort,
  ServiceContractStatus,
  ServiceContractUpdate
} from '@/types/logistics/customer-service/service-contract';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktServiceContracts
 */
const SERVICE_CONTRACT_API_BASE = 'TaktServiceContracts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务合同列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ServiceContract>>} 分页结果
 */
export function getServiceContractList(queryDto: any): Promise<TaktPagedResult<ServiceContract>> {
  return request<TaktPagedResult<ServiceContract>>({
    url: `${SERVICE_CONTRACT_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取服务合同
 * @param {string} id 服务合同ID
 * @returns {Promise<ServiceContract>} 服务合同DTO
 */
export function getServiceContractById(id: string): Promise<ServiceContract> {
  return request<ServiceContract>({
    url: `${SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建服务合同
 * @param {ServiceContractCreate} dto 创建DTO
 * @returns {Promise<ServiceContract>} 服务合同DTO
 */
export function createServiceContract(dto: ServiceContractCreate): Promise<ServiceContract> {
  return request<ServiceContract>({
    url: `${SERVICE_CONTRACT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新服务合同
 * @param {string} id 服务合同ID
 * @param {ServiceContractUpdate} dto 更新DTO
 * @returns {Promise<ServiceContract>} 服务合同DTO
 */
export function updateServiceContract(id: string, dto: ServiceContractUpdate): Promise<ServiceContract> {
  return request<ServiceContract>({
    url: `${SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除服务合同
 * @param {string} id 服务合同ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceContractById(id: string): Promise<void> {
  return request({
    url: `${SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除服务合同
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteServiceContractBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SERVICE_CONTRACT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新服务合同状态
 * @param {ServiceContractStatus} dto 状态DTO
 * @returns {Promise<ServiceContract>} 服务合同DTO
 */
export function updateServiceContractStatus(dto: ServiceContractStatus): Promise<ServiceContract> {
  return request<ServiceContract>({
    url: `${SERVICE_CONTRACT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新服务合同排序
 * @param {ServiceContractSort} dto 排序DTO
 * @returns {Promise<ServiceContract>} 服务合同DTO
 */
export function updateServiceContractSort(dto: ServiceContractSort): Promise<ServiceContract> {
  return request<ServiceContract>({
    url: `${SERVICE_CONTRACT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取服务合同选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getServiceContractOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SERVICE_CONTRACT_API_BASE}/options`,
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
export function getServiceContractTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_CONTRACT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入服务合同
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importServiceContract(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SERVICE_CONTRACT_API_BASE}/import`,
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
 * 导出服务合同
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportServiceContract(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SERVICE_CONTRACT_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
