// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：pay-scale.d.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 薪级薪等（现金报酬等级带宽）
 * 对应前端 TaktPayScaleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PayScale
 * @description 对应后端 TaktPayScaleDto
 */
export interface PayScale extends CompanyDtoBase {
  /**
   * PayScaleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  payScaleId: string;

  /**
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode: string;

  /**
   * 薪级名称
   */
  scaleName: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel: number;

  /**
   * 下限金额（元）
   */
  minSalary: number;

  /**
   * 中位金额（元）
   */
  midSalary: number;

  /**
   * 上限金额（元）
   */
  maxSalary: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * PayScale 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PayScaleQuery
 * @description 对应后端 TaktPayScaleQueryDto
 */
export interface PayScaleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode?: string;

  /**
   * 薪级名称
   */
  scaleName?: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel?: number;

  /**
   * 下限金额（元）
   */
  minSalary?: number;

  /**
   * 中位金额（元）
   */
  midSalary?: number;

  /**
   * 上限金额（元）
   */
  maxSalary?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus?: number;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建PayScale DTO
 * 对应前端 PayScaleCreate
 * @description 对应后端 TaktPayScaleCreateDto
 */
export interface PayScaleCreate {
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
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode: string;

  /**
   * 薪级名称
   */
  scaleName: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel: number;

  /**
   * 下限金额（元）
   */
  minSalary: number;

  /**
   * 中位金额（元）
   */
  midSalary: number;

  /**
   * 上限金额（元）
   */
  maxSalary: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新PayScale DTO
 * 继承 TaktPayScaleCreateDto，添加 PayScaleId 字段
 * 对应前端 PayScaleUpdate
 * @description 对应后端 TaktPayScaleUpdateDto
 */
export interface PayScaleUpdate extends PayScaleCreate {
  /**
   * PayScaleID（标识要更新的实体）
   */
  payScaleId: string;

}


/**
 * PayScale 状态更新 DTO
 * 对应前端 PayScaleStatus
 * @description 对应后端 TaktPayScaleStatusDto
 */
export interface PayScaleStatus {
  /**
   * PayScaleID
   */
  payScaleId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus: number;

}


/**
 * PayScale 排序更新 DTO
 * 对应前端 PayScaleSort
 * @description 对应后端 TaktPayScaleSortDto
 */
export interface PayScaleSort {
  /**
   * PayScaleID
   */
  payScaleId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * PayScale 导入模板行 DTO
 * 对应前端 PayScaleTemplate
 * @description 对应后端 TaktPayScaleTemplateDto
 */
export interface PayScaleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode?: string;

  /**
   * 薪级名称
   */
  scaleName?: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PayScale 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PayScaleImport
 * @description 对应后端 TaktPayScaleImportDto
 */
export interface PayScaleImport {
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
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode?: string;

  /**
   * 薪级名称
   */
  scaleName?: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * PayScale 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayScaleExport
 * @description 对应后端 TaktPayScaleExportDto
 */
export interface PayScaleExport {
  /**
   * PayScaleID
   */
  payScaleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode: string;

  /**
   * 薪级名称
   */
  scaleName: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel: number;

  /**
   * 下限金额（元）
   */
  minSalary: number;

  /**
   * 中位金额（元）
   */
  midSalary: number;

  /**
   * 上限金额（元）
   */
  maxSalary: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  scaleStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

