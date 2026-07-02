// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-ukeken.ts
// 创建时间：2026-06-30
// 功能描述：设变受检部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcUkeken, EcUkekenQuery, EcUkekenUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-ukeken';

const TAKTECUKEKENS_API_BASE = 'TaktEcUkekens';

/**
 * 获取受检部门列表（分页）
 */
export function getEcUkekenList(queryDto: EcUkekenQuery) {
  return request.get<TaktPagedResult<EcUkeken>>(`/${TAKTECUKEKENS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取受检部门详情
 */
export function getEcUkekenByEcDetailId(ecDetailId: string) {
  return request.get<EcUkeken>(`/${TAKTECUKEKENS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新受检部门
 */
export function updateEcUkeken(ecDetailId: string, dto: EcUkekenUpdate) {
  return request.put<EcUkeken>(`/${TAKTECUKEKENS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出受检部门
 */
export function exportEcUkekenData(queryDto?: EcUkekenQuery) {
  return request.get(`/${TAKTECUKEKENS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
