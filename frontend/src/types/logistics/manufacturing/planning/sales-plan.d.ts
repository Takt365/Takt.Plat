// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/planning
// 文件名称：sales-plan.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/planning 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt销售计划实体（公司级；MRP 需求计划源头，可下达生产计划或销售订单）
 * 对应前端 TaktSalesPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 SalesPlan
 * @description 对应后端 TaktSalesPlanDto
 */
export interface SalesPlan extends ApprovalDtoBase {
  /**
   * SalesPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPlanId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人员工名称（填充字段）
   */
  plannerName?: string;

  /**
   * 计划人（人员代码）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售计划明细列表（主子表关系） （子表：TaktSalesPlanItem）
   */
  items?: SalesPlanItem[];

}


/**
 * SalesPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPlanQuery
 * @description 对应后端 TaktSalesPlanQueryDto
 */
export interface SalesPlanQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode?: string;

  /**
   * 计划编制日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划编制日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 计划周期开始日期（范围查询-开始）
   */
  planPeriodStartStart?: string;

  /**
   * 计划周期开始日期（范围查询-结束）
   */
  planPeriodStartEnd?: string;

  /**
   * 计划周期结束日期（范围查询-开始）
   */
  planPeriodEndStart?: string;

  /**
   * 计划周期结束日期（范围查询-结束）
   */
  planPeriodEndEnd?: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人（人员代码）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus?: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建SalesPlan DTO
 * 对应前端 SalesPlanCreate
 * @description 对应后端 TaktSalesPlanCreateDto
 */
export interface SalesPlanCreate {
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
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人（人员代码）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesPlanItemCreate[];

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
 * 更新SalesPlan DTO
 * 继承 TaktSalesPlanCreateDto，添加 SalesPlanId 字段
 * 对应前端 SalesPlanUpdate
 * @description 对应后端 TaktSalesPlanUpdateDto
 */
export interface SalesPlanUpdate extends SalesPlanCreate {
  /**
   * SalesPlanID（标识要更新的实体）
   */
  salesPlanId: string;

}


/**
 * SalesPlan 状态更新 DTO
 * 对应前端 SalesPlanStatus
 * @description 对应后端 TaktSalesPlanStatusDto
 */
export interface SalesPlanStatus {
  /**
   * SalesPlanID
   */
  salesPlanId: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus: number;

}


/**
 * SalesPlan 导入模板行 DTO
 * 对应前端 SalesPlanTemplate
 * @description 对应后端 TaktSalesPlanTemplateDto
 */
export interface SalesPlanTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode?: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人（人员代码）
   */
  planBy?: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus?: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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
 * SalesPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPlanImport
 * @description 对应后端 TaktSalesPlanImportDto
 */
export interface SalesPlanImport {
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
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode?: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人（人员代码）
   */
  planBy?: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus?: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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
 * SalesPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPlanExport
 * @description 对应后端 TaktSalesPlanExportDto
 */
export interface SalesPlanExport {
  /**
   * SalesPlanID
   */
  salesPlanId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 销售计划编码（租户+公司+工厂内业务唯一）
   */
  salesPlanCode: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）
   */
  customerCode?: string;

  /**
   * 客户名称（冗余字段，便于查询展示）
   */
  customerName?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）
   */
  plannerId?: string;

  /**
   * 计划人（人员代码）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  planStatus: number;

  /**
   * 转单状态（0=未转单，1=部分转单，2=全部转单）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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

