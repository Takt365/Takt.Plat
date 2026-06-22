// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：bukan.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECBUKANS_API_BASE = 'TaktEcBukans';

/**
 * 获取部管部门列表（分页）
 */
export function getEcBukanList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECBUKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取部管部门详情
 */
export function getEcBukanByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECBUKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新部管部门
 */
export function updateEcBukan(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECBUKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出部管部门
 */
export function exportEcBukanData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECBUKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
