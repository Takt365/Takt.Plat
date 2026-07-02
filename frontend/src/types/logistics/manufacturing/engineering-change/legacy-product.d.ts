// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：legacy-product.d.ts
// 创建时间：2026-06-22
// 功能描述：设变旧品管制类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcLegacyProduct extends CompanyDtoBase {
  ecDetailId: string; ecNo: string; lineNumber: number; ecModel: string; ecOldItem?: string; ecOldText?: string; ecOldUsage?: number; ecNewItem?: string; oldProductHandling?: string; isEndOfLine?: string;
}

export interface EcLegacyProductQuery extends TaktPagedQuery {
  ecNo?: string;
  ecModel?: string;
  ecOldItem?: string;
}

export interface EcLegacyProductUpdate {
  ecDetailId: string;
  oldProductHandling?: string; isEndOfLine?: string; remark?: string;
}
