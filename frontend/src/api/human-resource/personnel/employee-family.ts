// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employee-family.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块 API（自动生成，请勿手改路由常量）
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
  EmployeeFamily,
  EmployeeFamilyCreate,
  EmployeeFamilyUpdate
} from '@/types/human-resource/personnel/employee-family';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeFamilies
 */
const EMPLOYEE_FAMILY_API_BASE = 'TaktEmployeeFamilies';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工家庭成员列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeFamily>>} 分页结果
 */
export function getEmployeeFamilyList(queryDto: any): Promise<TaktPagedResult<EmployeeFamily>> {
  return request<TaktPagedResult<EmployeeFamily>>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取员工家庭成员
 * @param {string} id 员工家庭成员ID
 * @returns {Promise<EmployeeFamily>} 员工家庭成员DTO
 */
export function getEmployeeFamilyById(id: string): Promise<EmployeeFamily> {
  return request<EmployeeFamily>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工家庭成员
 * @param {EmployeeFamilyCreate} dto 创建DTO
 * @returns {Promise<EmployeeFamily>} 员工家庭成员DTO
 */
export function createEmployeeFamily(dto: EmployeeFamilyCreate): Promise<EmployeeFamily> {
  return request<EmployeeFamily>({
    url: `${EMPLOYEE_FAMILY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工家庭成员
 * @param {string} id 员工家庭成员ID
 * @param {EmployeeFamilyUpdate} dto 更新DTO
 * @returns {Promise<EmployeeFamily>} 员工家庭成员DTO
 */
export function updateEmployeeFamily(id: string, dto: EmployeeFamilyUpdate): Promise<EmployeeFamily> {
  return request<EmployeeFamily>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工家庭成员
 * @param {string} id 员工家庭成员ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeFamilyById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_FAMILY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工家庭成员
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeFamilyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_FAMILY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工家庭成员选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeFamilyOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/options`,
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
export function getEmployeeFamilyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工家庭成员
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeFamily(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_FAMILY_API_BASE}/import`,
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
 * 导出员工家庭成员
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeFamily(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_FAMILY_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
