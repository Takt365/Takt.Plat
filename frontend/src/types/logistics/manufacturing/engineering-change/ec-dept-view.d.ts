// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-dept-view.d.ts
// 创建时间：2026-06-22
// 功能描述：设变部门视图共用类型；引用键 logistics.manufacturing.engineering-change
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common';

export interface EcDeptView extends CompanyDtoBase {
  ecDeptId?: string;
  ecDetailId: string;
  ecId: string;
  ecNo: string;
  lineNumber: number;
  ecModel: string;
  ecChange?: string;
  ecOldItem?: string;
  ecNewItem?: string;
  ecOldText?: string;
  ecNewText?: string;
  deptCode: string;
  isImplemented: number;
  content?: string;
  scheduledProductionDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderNo?: string;
  iqcOrderNo?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderNo?: string;
  productionTeam?: string;
  implementationDate?: string;
  inspectionBatch?: string;
  samplingNo?: string;
  isSopUpdated: number;
}

export interface EcDeptViewQuery extends TaktPagedQuery {
  ecNo?: string;
  ecModel?: string;
  isImplemented?: number;
  ecOldItem?: string;
  ecNewItem?: string;
}

export interface EcDeptViewUpdate {
  ecDetailId: string;
  isImplemented: number;
  content?: string;
  scheduledProductionDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderNo?: string;
  iqcOrderNo?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderNo?: string;
  productionTeam?: string;
  implementationDate?: string;
  inspectionBatch?: string;
  samplingNo?: string;
  isSopUpdated: number;
  remark?: string;
}
