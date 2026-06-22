// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：kakunin.d.ts
// 创建时间：2026-06-22
// 功能描述：设变物料确认类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcKakunin extends CompanyDtoBase {
  ecDetailId: string; ecNo: string; lineNumber: number; ecModel: string; ecOldItem?: string; ecNewItem?: string; isCheck: number; isProcurement: number; ecChange?: string; ecNote?: string;
}

export interface EcKakuninQuery extends TaktPagedQuery {
  ecNo?: string;
  ecModel?: string;
  isCheck?: number; ecNewItem?: string;
}

export interface EcKakuninUpdate {
  ecDetailId: string;
  isCheck: number; isProcurement: number; ecNote?: string;
}
