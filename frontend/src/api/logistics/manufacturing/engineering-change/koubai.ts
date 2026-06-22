// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：koubai.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变采购部门 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcDeptView, EcDeptViewQuery, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const TAKTECKOUBAIS_API_BASE = 'TaktEcKoubais';

/**
 * 获取采购部门列表（分页）
 */
export function getEcKoubaiList(queryDto: EcDeptViewQuery) {
  return request.get<TaktPagedResult<EcDeptView>>(`/${TAKTECKOUBAIS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取采购部门详情
 */
export function getEcKoubaiByEcDetailId(ecDetailId: string) {
  return request.get<EcDeptView>(`/${TAKTECKOUBAIS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新采购部门
 */
export function updateEcKoubai(ecDetailId: string, dto: EcDeptViewUpdate) {
  return request.put<EcDeptView>(`/${TAKTECKOUBAIS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出采购部门
 */
export function exportEcKoubaiData(queryDto?: EcDeptViewQuery) {
  return request.get(`/${TAKTECKOUBAIS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
