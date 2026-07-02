// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ec-bukan.ts
// 创建时间：2026-06-30
// 功能描述：设变部管部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcBukan, EcBukanQuery, EcBukanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-bukan';
import type {
  EcExecTransposedQuery,
  EcExecTransposedResult,
} from '@/types/logistics/manufacturing/engineering-change/ec-exec-transposed';

const TAKTECBUKANS_API_BASE = 'TaktEcBukans';

/**
 * 获取部管部门列表（分页）
 */
export function getEcBukanList(queryDto: EcBukanQuery) {
  return request.get<TaktPagedResult<EcBukan>>(`/${TAKTECBUKANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取部管部门详情
 */
export function getEcBukanByEcDetailId(ecDetailId: string) {
  return request.get<EcBukan>(`/${TAKTECBUKANS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新部管部门
 */
export function updateEcBukan(ecDetailId: string, dto: EcBukanUpdate) {
  return request.put<EcBukan>(`/${TAKTECBUKANS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出部管部门
 */
export function exportEcBukanData(queryDto?: EcBukanQuery) {
  return request.get(`/${TAKTECBUKANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}

/**
 * 获取设变部门执行转置列表（分页；行=设变明细，列=各部门实施状态）
 * @param queryDto 查询参数
 * @returns {Promise<EcExecTransposedResult>} 转置结果
 */
export function getEcBukanTransposedList(queryDto: EcExecTransposedQuery): Promise<EcExecTransposedResult> {
  return request.get<EcExecTransposedResult>(`/${TAKTECBUKANS_API_BASE}/transposed`, { params: queryDto });
}
