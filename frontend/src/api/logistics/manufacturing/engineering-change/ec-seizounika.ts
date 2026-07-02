// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-seizounika.ts
// 创建时间：2026-06-30
// 功能描述：设变制二部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcSeizounika, EcSeizounikaQuery, EcSeizounikaUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seizounika';

const TAKTECSEIZOUNIKAS_API_BASE = 'TaktEcSeizounikas';

/**
 * 获取制二部门列表（分页）
 */
export function getEcSeizounikaList(queryDto: EcSeizounikaQuery) {
  return request.get<TaktPagedResult<EcSeizounika>>(`/${TAKTECSEIZOUNIKAS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取制二部门详情
 */
export function getEcSeizounikaByEcDetailId(ecDetailId: string) {
  return request.get<EcSeizounika>(`/${TAKTECSEIZOUNIKAS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新制二部门
 */
export function updateEcSeizounika(ecDetailId: string, dto: EcSeizounikaUpdate) {
  return request.put<EcSeizounika>(`/${TAKTECSEIZOUNIKAS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出制二部门
 */
export function exportEcSeizounikaData(queryDto?: EcSeizounikaQuery) {
  return request.get(`/${TAKTECSEIZOUNIKAS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
