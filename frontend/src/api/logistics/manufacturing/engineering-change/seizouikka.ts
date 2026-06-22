// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：seizouikka.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造一课 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECSEIZOUIKKAS_API_BASE = 'TaktEcSeizouikkas';

/**
 * 获取制造一课列表（分页）
 */
export function getEcSeizouikkaList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECSEIZOUIKKAS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取制造一课详情
 */
export function getEcSeizouikkaByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECSEIZOUIKKAS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新制造一课
 */
export function updateEcSeizouikka(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECSEIZOUIKKAS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出制造一课
 */
export function exportEcSeizouikkaData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECSEIZOUIKKAS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
