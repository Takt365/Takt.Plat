// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：hinkan.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变品管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECHINKANS_API_BASE = 'TaktEcHinkans';

/**
 * 获取品管部门列表（分页）
 */
export function getEcHinkanList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECHINKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取品管部门详情
 */
export function getEcHinkanByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECHINKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新品管部门
 */
export function updateEcHinkan(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECHINKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出品管部门
 */
export function exportEcHinkanData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECHINKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
