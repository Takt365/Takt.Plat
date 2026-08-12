// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：expense.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 费用单实体。继承审批基类，与 TaktFlowEngine 对接；ExpenseStatus 与 ApprovalStatus 取值对齐。
 * 对应前端 TaktExpenseDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Expense
 * @description 对应后端 TaktExpenseDto
 */
export interface Expense extends ApprovalDtoBase {

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus?: number;

  /**
   * 费用单明细列表（主子表关系）（子表，级联保存）
   */
  expenseDetails?: ExpenseDetailCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * Expense 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ExpenseExport
 * @description 对应后端 TaktExpenseExportDto
 */
export interface ExpenseExport {
  /**
   * ExpenseID
   */
  expenseId: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode: string;

  /**
   * 费用标题
   */
  expenseTitle: string;

  /**
   * 费用类型（字典 accounting_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  applicationDept?: string;

  /**
   * 经费负担部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  costBearerDept?: string;

  /**
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount: number;

  /**
   * 税率（字典 accounting_tax_rate_param；整单统一税率）
   */
  taxRate: number;

  /**
   * 税额（整单合计）
   */
  taxAmount: number;

  /**
   * 费用发生日期
   */
  expenseDate: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 附件 JSON
   */
  attachments?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=Id）
   */
  plantCode: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

