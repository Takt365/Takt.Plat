// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：kanban.d.ts
// 创建时间：2026-06-22
// 功能描述：设变设变看板类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcKanbanDeptStage {
  deptCode: string;
  implementedCount: number;
  totalCount: number;
}

export interface EcKanban extends CompanyDtoBase {
  ecId: string;
  ecNo: string;
  ecTitle: string;
  changeStatus: number;
  ecStatus: number;
  ecLeader: string;
  effectiveDate: string;
  detailCount: number;
  deptStages: EcKanbanDeptStage[];
}

export interface EcKanbanQuery extends TaktPagedQuery {
  ecNo?: string;
  changeStatus?: number;
  ecStatus?: number;
}
