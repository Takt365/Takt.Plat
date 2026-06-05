// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：standard-wage-rate.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 标准工资率实体
 * 对应前端 TaktStandardWageRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StandardWageRate
 * @description 对应后端 TaktStandardWageRateDto
 */
export interface StandardWageRate extends CompanyDtoBase {
  /**
   * StandardWageRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  standardWageRateId: string;

  /**
   * 年月（yyyyMM）
   */
  yearMonth: string;

  /**
   * 工作天数
   */
  workingDays: number;

  /**
   * 销售额
   */
  salesAmount: number;

  /**
   * 直接人数
   */
  directLaborCount: number;

  /**
   * 直接工资
   */
  directLaborWage: number;

  /**
   * 直接加班小时
   */
  directOvertimeHours: number;

  /**
   * 直接加班总额
   */
  directOvertimeTotal: number;

  /**
   * 直接工资率
   */
  directWageRate: number;

  /**
   * 间接人数
   */
  indirectLaborCount: number;

  /**
   * 间接工资
   */
  indirectLaborWage: number;

  /**
   * 间接加班小时
   */
  indirectOvertimeHours: number;

  /**
   * 间接加班总额
   */
  indirectOvertimeTotal: number;

  /**
   * 间接工资率
   */
  indirectWageRate: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * StandardWageRate 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 StandardWageRateQuery
 * @description 对应后端 TaktStandardWageRateQueryDto
 */
export interface StandardWageRateQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 年月（yyyyMM）
   */
  yearMonth?: string;

  /**
   * 工作天数
   */
  workingDays?: number;

  /**
   * 销售额
   */
  salesAmount?: number;

  /**
   * 直接人数
   */
  directLaborCount?: number;

  /**
   * 直接工资
   */
  directLaborWage?: number;

  /**
   * 直接加班小时
   */
  directOvertimeHours?: number;

  /**
   * 直接加班总额
   */
  directOvertimeTotal?: number;

  /**
   * 直接工资率
   */
  directWageRate?: number;

  /**
   * 间接人数
   */
  indirectLaborCount?: number;

  /**
   * 间接工资
   */
  indirectLaborWage?: number;

  /**
   * 间接加班小时
   */
  indirectOvertimeHours?: number;

  /**
   * 间接加班总额
   */
  indirectOvertimeTotal?: number;

  /**
   * 间接工资率
   */
  indirectWageRate?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 创建StandardWageRate DTO
 * 对应前端 StandardWageRateCreate
 * @description 对应后端 TaktStandardWageRateCreateDto
 */
export interface StandardWageRateCreate {
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
   * 年月（yyyyMM）
   */
  yearMonth: string;

  /**
   * 工作天数
   */
  workingDays: number;

  /**
   * 销售额
   */
  salesAmount: number;

  /**
   * 直接人数
   */
  directLaborCount: number;

  /**
   * 直接工资
   */
  directLaborWage: number;

  /**
   * 直接加班小时
   */
  directOvertimeHours: number;

  /**
   * 直接加班总额
   */
  directOvertimeTotal: number;

  /**
   * 直接工资率
   */
  directWageRate: number;

  /**
   * 间接人数
   */
  indirectLaborCount: number;

  /**
   * 间接工资
   */
  indirectLaborWage: number;

  /**
   * 间接加班小时
   */
  indirectOvertimeHours: number;

  /**
   * 间接加班总额
   */
  indirectOvertimeTotal: number;

  /**
   * 间接工资率
   */
  indirectWageRate: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新StandardWageRate DTO
 * 继承 TaktStandardWageRateCreateDto，添加 StandardWageRateId 字段
 * 对应前端 StandardWageRateUpdate
 * @description 对应后端 TaktStandardWageRateUpdateDto
 */
export interface StandardWageRateUpdate extends StandardWageRateCreate {
  /**
   * StandardWageRateID（标识要更新的实体）
   */
  standardWageRateId: string;

}


/**
 * StandardWageRate 导入模板行 DTO
 * 对应前端 StandardWageRateTemplate
 * @description 对应后端 TaktStandardWageRateTemplateDto
 */
export interface StandardWageRateTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 年月（yyyyMM）
   */
  yearMonth?: string;

  /**
   * 直接人数
   */
  directLaborCount?: number;

  /**
   * 间接人数
   */
  indirectLaborCount?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * StandardWageRate 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 StandardWageRateImport
 * @description 对应后端 TaktStandardWageRateImportDto
 */
export interface StandardWageRateImport {
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
   * 年月（yyyyMM）
   */
  yearMonth?: string;

  /**
   * 直接人数
   */
  directLaborCount?: number;

  /**
   * 间接人数
   */
  indirectLaborCount?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * StandardWageRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardWageRateExport
 * @description 对应后端 TaktStandardWageRateExportDto
 */
export interface StandardWageRateExport {
  /**
   * StandardWageRateID
   */
  standardWageRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 年月（yyyyMM）
   */
  yearMonth: string;

  /**
   * 工作天数
   */
  workingDays: number;

  /**
   * 销售额
   */
  salesAmount: number;

  /**
   * 直接人数
   */
  directLaborCount: number;

  /**
   * 直接工资
   */
  directLaborWage: number;

  /**
   * 直接加班小时
   */
  directOvertimeHours: number;

  /**
   * 直接加班总额
   */
  directOvertimeTotal: number;

  /**
   * 直接工资率
   */
  directWageRate: number;

  /**
   * 间接人数
   */
  indirectLaborCount: number;

  /**
   * 间接工资
   */
  indirectLaborWage: number;

  /**
   * 间接加班小时
   */
  indirectOvertimeHours: number;

  /**
   * 间接加班总额
   */
  indirectOvertimeTotal: number;

  /**
   * 间接工资率
   */
  indirectWageRate: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

