// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：expense-detail.d.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 费用单明细实体
 * 对应前端 TaktExpenseDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ExpenseDetail
 * @description 对应后端 TaktExpenseDetailDto
 */
export interface ExpenseDetail extends CompanyDtoBase {
  /**
   * ExpenseDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  expenseDetailId: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId: string;

  /**
   * 费用单 名称（填充字段）
   */
  expenseName?: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 明细项名称
   */
  itemName: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity: number;

  /**
   * 金额
   */
  itemAmount: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

}


/**
 * ExpenseDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ExpenseDetailQuery
 * @description 对应后端 TaktExpenseDetailQueryDto
 */
export interface ExpenseDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId?: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 明细项名称
   */
  itemName?: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity?: number;

  /**
   * 金额
   */
  itemAmount?: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期（范围查询-开始）
   */
  expenseDetailDateStart?: string;

  /**
   * 费用发生日期（范围查询-结束）
   */
  expenseDetailDateEnd?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ExpenseDetail DTO
 * 对应前端 ExpenseDetailCreate
 * @description 对应后端 TaktExpenseDetailCreateDto
 */
export interface ExpenseDetailCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 明细项名称
   */
  itemName: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity: number;

  /**
   * 金额
   */
  itemAmount: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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
 * 更新ExpenseDetail DTO
 * 继承 TaktExpenseDetailCreateDto，添加 ExpenseDetailId 字段
 * 对应前端 ExpenseDetailUpdate
 * @description 对应后端 TaktExpenseDetailUpdateDto
 */
export interface ExpenseDetailUpdate extends ExpenseDetailCreate {
  /**
   * ExpenseDetailID（标识要更新的实体）
   */
  expenseDetailId: string;

}


/**
 * ExpenseDetail 导入模板行 DTO
 * 对应前端 ExpenseDetailTemplate
 * @description 对应后端 TaktExpenseDetailTemplateDto
 */
export interface ExpenseDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId?: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 明细项名称
   */
  itemName?: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity?: number;

  /**
   * 金额
   */
  itemAmount?: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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
 * ExpenseDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ExpenseDetailImport
 * @description 对应后端 TaktExpenseDetailImportDto
 */
export interface ExpenseDetailImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId?: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 明细项名称
   */
  itemName?: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity?: number;

  /**
   * 金额
   */
  itemAmount?: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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
 * ExpenseDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ExpenseDetailExport
 * @description 对应后端 TaktExpenseDetailExportDto
 */
export interface ExpenseDetailExport {
  /**
   * ExpenseDetailID
   */
  expenseDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId: string;

  /**
   * 费用单编号（冗余，便于查询）
   */
  expenseCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 明细项名称
   */
  itemName: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity: number;

  /**
   * 金额
   */
  itemAmount: number;

  /**
   * 会计科目（关联 TaktAccountTitle.AccountTitleCode，选项 TaktAccountTitles/options）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceNo?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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

