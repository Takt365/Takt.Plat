// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：purchase-request.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购申请实体
 * 对应前端 TaktPurchaseRequestDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PurchaseRequest
 * @description 对应后端 TaktPurchaseRequestDto
 */
export interface PurchaseRequest extends ApprovalDtoBase {
  /**
   * PurchaseRequestID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseRequestId: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode: string;

  /**
   * 申请日期
   */
  requestDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人员工名称（填充字段）
   */
  requestName?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy: string;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount: number;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 流程实例名称（填充字段）
   */
  flowInstanceName?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 采购申请明细列表（主子表关系，一个申请可以有多个明细） （子表：TaktPurchaseRequestItem）
   */
  items?: PurchaseRequestItem[];

  /**
   * 采购申请变更记录列表（外键在子表 <see cref="TaktPurchaseRequestChangeLog.RequestId"/>） （子表：TaktPurchaseRequestChangeLog）
   */
  changeLogs?: PurchaseRequestChangeLog[];

}


/**
 * PurchaseRequest 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseRequestQuery
 * @description 对应后端 TaktPurchaseRequestQueryDto
 */
export interface PurchaseRequestQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode?: string;

  /**
   * 申请日期（范围查询-开始）
   */
  requestDateStart?: string;

  /**
   * 申请日期（范围查询-结束）
   */
  requestDateEnd?: string;

  /**
   * 要求到货日期（范围查询-开始）
   */
  requiredArrivalDateStart?: string;

  /**
   * 要求到货日期（范围查询-结束）
   */
  requiredArrivalDateEnd?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy?: string;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount?: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount?: number;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus?: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus?: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 审批状态（TaktApprovalStatus）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PurchaseRequest DTO
 * 对应前端 PurchaseRequestCreate
 * @description 对应后端 TaktPurchaseRequestCreateDto
 */
export interface PurchaseRequestCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode: string;

  /**
   * 申请日期
   */
  requestDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy: string;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount: number;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 采购申请明细列表（主子表关系，一个申请可以有多个明细）（子表，级联保存）
   */
  items?: PurchaseRequestItemCreate[];

  /**
   * 采购申请变更记录列表（外键在子表 <see cref="TaktPurchaseRequestChangeLog.RequestId"/>）（子表，级联保存）
   */
  changeLogs?: PurchaseRequestChangeLogCreate[];

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PurchaseRequest DTO
 * 继承 TaktPurchaseRequestCreateDto，添加 PurchaseRequestId 字段
 * 对应前端 PurchaseRequestUpdate
 * @description 对应后端 TaktPurchaseRequestUpdateDto
 */
export interface PurchaseRequestUpdate extends PurchaseRequestCreate {
  /**
   * PurchaseRequestID（标识要更新的实体）
   */
  purchaseRequestId: string;

}


/**
 * PurchaseRequest 状态更新 DTO
 * 对应前端 PurchaseRequestStatus
 * @description 对应后端 TaktPurchaseRequestStatusDto
 */
export interface PurchaseRequestStatus {
  /**
   * PurchaseRequestID
   */
  purchaseRequestId: string;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus: number;

}


/**
 * PurchaseRequest 导入模板行 DTO
 * 对应前端 PurchaseRequestTemplate
 * @description 对应后端 TaktPurchaseRequestTemplateDto
 */
export interface PurchaseRequestTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy?: string;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus?: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus?: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchaseRequest 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseRequestImport
 * @description 对应后端 TaktPurchaseRequestImportDto
 */
export interface PurchaseRequestImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy?: string;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus?: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus?: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PurchaseRequest 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseRequestExport
 * @description 对应后端 TaktPurchaseRequestExportDto
 */
export interface PurchaseRequestExport {
  /**
   * PurchaseRequestID
   */
  purchaseRequestId: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 采购申请编码（唯一索引）
   */
  purchaseRequestCode: string;

  /**
   * 申请日期
   */
  requestDate: string;

  /**
   * 要求到货日期
   */
  requiredArrivalDate?: string;

  /**
   * 申请人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  requestId?: string;

  /**
   * 申请人（人员代码）
   */
  requestBy: string;

  /**
   * 申请总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 申请总金额（精确到分，存储为整数，单位为分）
   */
  totalAmount: number;

  /**
   * 已转订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转订单金额（精确到分，存储为整数，单位为分）
   */
  convertedAmount: number;

  /**
   * 申请状态（1=启用，0=禁用）
   */
  requestStatus: number;

  /**
   * 转订单状态（0=未转订单，1=部分转订单，2=全部转订单）
   */
  convertedStatus: number;

  /**
   * 流程实例ID（关联 TaktFlowInstance，发起审批后由业务写入，用于审批流程）
   */
  flowInstanceId?: string;

  /**
   * 申请原因
   */
  requestReason?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

