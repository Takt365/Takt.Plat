// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：seikan.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变生管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECSEIKANS_API_BASE = 'TaktEcSeikans';

/**
 * 获取生管部门列表（分页）
 */
export function getEcSeikanList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECSEIKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取生管部门详情
 */
export function getEcSeikanByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECSEIKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新生管部门
 */
export function updateEcSeikan(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECSEIKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出生管部门
 */
export function exportEcSeikanData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECSEIKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
