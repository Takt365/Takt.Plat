// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：standard-operation-rate.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 标准生产稼动率实体 OperationRate 为标准对标目标值；对比参考：达成率(%) = 实际稼动率 ÷ 标准稼动率 × 100%。
 * 对应前端 TaktStandardOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StandardOperationRate
 * @description 对应后端 TaktStandardOperationRateDto
 */
export interface StandardOperationRate extends CompanyDtoBase {
  /**
   * StandardOperationRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  standardOperationRateId: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus: number;

}


/**
 * StandardOperationRate 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 StandardOperationRateQuery
 * @description 对应后端 TaktStandardOperationRateQueryDto
 */
export interface StandardOperationRateQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear?: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType?: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus?: number;

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
 * 创建StandardOperationRate DTO
 * 对应前端 StandardOperationRateCreate
 * @description 对应后端 TaktStandardOperationRateCreateDto
 */
export interface StandardOperationRateCreate {
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
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus: number;

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
 * 更新StandardOperationRate DTO
 * 继承 TaktStandardOperationRateCreateDto，添加 StandardOperationRateId 字段
 * 对应前端 StandardOperationRateUpdate
 * @description 对应后端 TaktStandardOperationRateUpdateDto
 */
export interface StandardOperationRateUpdate extends StandardOperationRateCreate {
  /**
   * StandardOperationRateID（标识要更新的实体）
   */
  standardOperationRateId: string;

}


/**
 * StandardOperationRate 状态更新 DTO
 * 对应前端 StandardOperationRateStatus
 * @description 对应后端 TaktStandardOperationRateStatusDto
 */
export interface StandardOperationRateStatus {
  /**
   * StandardOperationRateID
   */
  standardOperationRateId: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus: number;

}


/**
 * StandardOperationRate 导入模板行 DTO
 * 对应前端 StandardOperationRateTemplate
 * @description 对应后端 TaktStandardOperationRateTemplateDto
 */
export interface StandardOperationRateTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear?: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType?: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus?: number;

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
 * StandardOperationRate 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 StandardOperationRateImport
 * @description 对应后端 TaktStandardOperationRateImportDto
 */
export interface StandardOperationRateImport {
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
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear?: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType?: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus?: number;

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
 * StandardOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardOperationRateExport
 * @description 对应后端 TaktStandardOperationRateExportDto
 */
export interface StandardOperationRateExport {
  /**
   * StandardOperationRateID
   */
  standardOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status；0=禁用，1=启用）
   */
  rateStatus: number;

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

