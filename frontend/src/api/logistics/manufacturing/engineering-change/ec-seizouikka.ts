// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-seizouikka.ts
// 创建时间：2026-06-30
// 功能描述：设变制一部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcSeizouikka, EcSeizouikkaQuery, EcSeizouikkaUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seizouikka';

const TAKTECSEIZOUIKKAS_API_BASE = 'TaktEcSeizouikkas';

/**
 * 获取制一部门列表（分页）
 */
export function getEcSeizouikkaList(queryDto: EcSeizouikkaQuery) {
  return request.get<TaktPagedResult<EcSeizouikka>>(`/${TAKTECSEIZOUIKKAS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取制一部门详情
 */
export function getEcSeizouikkaByEcDetailId(ecDetailId: string) {
  return request.get<EcSeizouikka>(`/${TAKTECSEIZOUIKKAS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新制一部门
 */
export function updateEcSeizouikka(ecDetailId: string, dto: EcSeizouikkaUpdate) {
  return request.put<EcSeizouikka>(`/${TAKTECSEIZOUIKKAS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出制一部门
 */
export function exportEcSeizouikkaData(queryDto?: EcSeizouikkaQuery) {
  return request.get(`/${TAKTECSEIZOUIKKAS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
