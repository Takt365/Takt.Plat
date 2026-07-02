// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-seikan.ts
// 创建时间：2026-06-30
// 功能描述：设变生管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcSeikan, EcSeikanQuery, EcSeikanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seikan';

const TAKTECSEIKANS_API_BASE = 'TaktEcSeikans';

/**
 * 获取生管部门列表（分页）
 */
export function getEcSeikanList(queryDto: EcSeikanQuery) {
  return request.get<TaktPagedResult<EcSeikan>>(`/${TAKTECSEIKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取生管部门详情
 */
export function getEcSeikanByEcDetailId(ecDetailId: string) {
  return request.get<EcSeikan>(`/${TAKTECSEIKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新生管部门
 */
export function updateEcSeikan(ecDetailId: string, dto: EcSeikanUpdate) {
  return request.put<EcSeikan>(`/${TAKTECSEIKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出生管部门
 */
export function exportEcSeikanData(queryDto?: EcSeikanQuery) {
  return request.get(`/${TAKTECSEIKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
