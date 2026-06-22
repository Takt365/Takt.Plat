// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：seizounika.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造二课 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECSEIZOUNIKAS_API_BASE = 'TaktEcSeizounikas';

/**
 * 获取制造二课列表（分页）
 */
export function getEcSeizounikaList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECSEIZOUNIKAS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取制造二课详情
 */
export function getEcSeizounikaByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECSEIZOUNIKAS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新制造二课
 */
export function updateEcSeizounika(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECSEIZOUNIKAS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出制造二课
 */
export function exportEcSeizounikaData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECSEIZOUNIKAS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
