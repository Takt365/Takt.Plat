// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-hinkan.ts
// 创建时间：2026-06-30
// 功能描述：设变品管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcHinkan, EcHinkanQuery, EcHinkanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-hinkan';

const TAKTECHINKANS_API_BASE = 'TaktEcHinkans';

/**
 * 获取品管部门列表（分页）
 */
export function getEcHinkanList(queryDto: EcHinkanQuery) {
  return request.get<TaktPagedResult<EcHinkan>>(`/${TAKTECHINKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取品管部门详情
 */
export function getEcHinkanByEcDetailId(ecDetailId: string) {
  return request.get<EcHinkan>(`/${TAKTECHINKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新品管部门
 */
export function updateEcHinkan(ecDetailId: string, dto: EcHinkanUpdate) {
  return request.put<EcHinkan>(`/${TAKTECHINKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出品管部门
 */
export function exportEcHinkanData(queryDto?: EcHinkanQuery) {
  return request.get(`/${TAKTECHINKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
