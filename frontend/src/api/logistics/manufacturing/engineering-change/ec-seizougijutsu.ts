// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-seizougijutsu.ts
// 创建时间：2026-07-01
// 功能描述：设变制造技术课部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcSeizougijutsu, EcSeizougijutsuQuery, EcSeizougijutsuUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seizougijutsu';

const TAKTECSEIZOUGIJUTSUS_API_BASE = 'TaktEcSeizougijutsus';

/**
 * 获取制造技术课部门列表（分页）
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<EcSeizougijutsu>>} 分页结果
 */
export function getEcSeizougijutsuList(queryDto: EcSeizougijutsuQuery) {
  return request.get<TaktPagedResult<EcSeizougijutsu>>(`/${TAKTECSEIZOUGIJUTSUS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取制造技术课部门详情
 * @param ecDetailId 设变明细 ID
 * @returns {Promise<EcSeizougijutsu>} 部门视图
 */
export function getEcSeizougijutsuByEcDetailId(ecDetailId: string) {
  return request.get<EcSeizougijutsu>(`/${TAKTECSEIZOUGIJUTSUS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新制造技术课部门
 * @param ecDetailId 设变明细 ID
 * @param dto 更新 DTO
 * @returns {Promise<EcSeizougijutsu>} 更新结果
 */
export function updateEcSeizougijutsu(ecDetailId: string, dto: EcSeizougijutsuUpdate) {
  return request.put<EcSeizougijutsu>(`/${TAKTECSEIZOUGIJUTSUS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出制造技术课部门
 * @param queryDto 查询参数
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportEcSeizougijutsuData(queryDto?: EcSeizougijutsuQuery) {
  return request.get(`/${TAKTECSEIZOUGIJUTSUS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
