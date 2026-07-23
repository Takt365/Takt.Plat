// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：sales-forecast.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt销售预测实体（公司级；MDS 独立需求源头，可下达生产计划或销售订单）
 * 对应前端 TaktSalesForecastDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 SalesForecast
 * @description 对应后端 TaktSalesForecastDto
 */
export interface SalesForecast extends ApprovalDtoBase {
  /**
   * SalesForecastID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode: string;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人员工名称（填充字段）
   */
  plannerName?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售预测明细列表（主子表关系） （子表：TaktSalesForecastItem）
   */
  items?: SalesForecastItem[];

}


/**
 * SalesForecast 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesForecastQuery
 * @description 对应后端 TaktSalesForecastQueryDto
 */
export interface SalesForecastQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode?: string;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
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
 * 创建SalesForecast DTO
 * 对应前端 SalesForecastCreate
 * @description 对应后端 TaktSalesForecastCreateDto
 */
export interface SalesForecastCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode: string;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售预测明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesForecastItemCreate[];

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
 * 更新SalesForecast DTO
 * 继承 TaktSalesForecastCreateDto，添加 SalesForecastId 字段
 * 对应前端 SalesForecastUpdate
 * @description 对应后端 TaktSalesForecastUpdateDto
 */
export interface SalesForecastUpdate extends SalesForecastCreate {
  /**
   * SalesForecastID（标识要更新的实体）
   */
  salesForecastId: string;

  /**
   * 销售预测明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesForecast 状态更新 DTO
 * 对应前端 SalesForecastStatus
 * @description 对应后端 TaktSalesForecastStatusDto
 */
export interface SalesForecastStatus {
  /**
   * SalesForecastID
   */
  salesForecastId: string;

  /**
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

}


/**
 * SalesForecast 导入模板行 DTO
 * 对应前端 SalesForecastTemplate
 * @description 对应后端 TaktSalesForecastTemplateDto
 */
export interface SalesForecastTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售预测明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesForecastItemCreate[];

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
 * SalesForecast 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesForecastImport
 * @description 对应后端 TaktSalesForecastImportDto
 */
export interface SalesForecastImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售预测明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesForecastItemCreate[];

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
 * SalesForecast 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesForecastExport
 * @description 对应后端 TaktSalesForecastExportDto
 */
export interface SalesForecastExport {
  /**
   * SalesForecastID
   */
  salesForecastId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode: string;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerId?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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

