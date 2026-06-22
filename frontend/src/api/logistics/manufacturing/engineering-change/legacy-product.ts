// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：legacy-product.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变旧品管制 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type { EcLegacyProduct, EcLegacyProductQuery, EcLegacyProductUpdate } from '@/types/logistics/manufacturing/engineering-change/legacy-product';

const TAKTECLEGACYPRODUCTS_API_BASE = 'TaktEcLegacyProducts';

/**
 * 获取旧品管制列表（分页）
 */
export function getEcLegacyProductList(queryDto: EcLegacyProductQuery) {
  return request.get<TaktPagedResult<EcLegacyProduct>>(`/${TAKTECLEGACYPRODUCTS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取旧品管制详情
 */
export function getEcLegacyProductByEcDetailId(ecDetailId: string) {
  return request.get<EcLegacyProduct>(`/${TAKTECLEGACYPRODUCTS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新旧品管制
 */
export function updateEcLegacyProduct(ecDetailId: string, dto: EcLegacyProductUpdate) {
  return request.put<EcLegacyProduct>(`/${TAKTECLEGACYPRODUCTS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出旧品管制
 */
export function exportEcLegacyProductData(queryDto?: EcLegacyProductQuery) {
  return request.get(`/${TAKTECLEGACYPRODUCTS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
