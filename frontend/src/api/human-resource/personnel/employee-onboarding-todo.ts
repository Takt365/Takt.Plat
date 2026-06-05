// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employee-onboarding-todo.ts
// 创建时间：2026-06-05
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
  EmployeeOnboardingTodo,
  EmployeeOnboardingTodoCreate,
  EmployeeOnboardingTodoStatus,
  EmployeeOnboardingTodoUpdate
} from '@/types/human-resource/personnel/employee-onboarding-todo';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeOnboardingTodos
 */
const EMPLOYEE_ONBOARDING_TODO_API_BASE = 'TaktEmployeeOnboardingTodos';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取入职待办列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeOnboardingTodo>>} 分页结果
 */
export function getEmployeeOnboardingTodoList(queryDto: any): Promise<TaktPagedResult<EmployeeOnboardingTodo>> {
  return request<TaktPagedResult<EmployeeOnboardingTodo>>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/list`,
    method: 'get',
    params: {
      queryDto
    },
  });
}

/**
 * 根据ID获取入职待办
 * @param {string} id 入职待办ID
 * @returns {Promise<EmployeeOnboardingTodo>} 入职待办DTO
 */
export function getEmployeeOnboardingTodoById(id: string): Promise<EmployeeOnboardingTodo> {
  return request<EmployeeOnboardingTodo>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建入职待办
 * @param {EmployeeOnboardingTodoCreate} dto 创建DTO
 * @returns {Promise<EmployeeOnboardingTodo>} 入职待办DTO
 */
export function createEmployeeOnboardingTodo(dto: EmployeeOnboardingTodoCreate): Promise<EmployeeOnboardingTodo> {
  return request<EmployeeOnboardingTodo>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新入职待办
 * @param {string} id 入职待办ID
 * @param {EmployeeOnboardingTodoUpdate} dto 更新DTO
 * @returns {Promise<EmployeeOnboardingTodo>} 入职待办DTO
 */
export function updateEmployeeOnboardingTodo(id: string, dto: EmployeeOnboardingTodoUpdate): Promise<EmployeeOnboardingTodo> {
  return request<EmployeeOnboardingTodo>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除入职待办
 * @param {string} id 入职待办ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeOnboardingTodoById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除入职待办
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeOnboardingTodoBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新入职待办状态
 * @param {EmployeeOnboardingTodoStatus} dto 状态DTO
 * @returns {Promise<EmployeeOnboardingTodo>} 入职待办DTO
 */
export function updateEmployeeOnboardingTodoStatus(dto: EmployeeOnboardingTodoStatus): Promise<EmployeeOnboardingTodo> {
  return request<EmployeeOnboardingTodo>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取入职待办选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeOnboardingTodoOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/options`,
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
export function getEmployeeOnboardingTodoTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入入职待办
 * @param {File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeOnboardingTodo(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/import`,
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
 * 导出入职待办
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeOnboardingTodo(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_ONBOARDING_TODO_API_BASE}/export`,
    method: 'get',
    params: {
      query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
