// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/scheduling
// 文件名称：changeover-matrix.d.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/scheduling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 换型矩阵（工作中心 + 前产品 → 后产品的换型时间）
 * 对应前端 TaktChangeoverMatrixDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ChangeoverMatrix
 * @description 对应后端 TaktChangeoverMatrixDto
 */
export interface ChangeoverMatrix extends CompanyDtoBase {
  /**
   * ChangeoverMatrixID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  changeoverMatrixId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus: number;

}


/**
 * ChangeoverMatrix 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ChangeoverMatrixQuery
 * @description 对应后端 TaktChangeoverMatrixQueryDto
 */
export interface ChangeoverMatrixQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode?: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode?: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus?: number;

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
 * 创建ChangeoverMatrix DTO
 * 对应前端 ChangeoverMatrixCreate
 * @description 对应后端 TaktChangeoverMatrixCreateDto
 */
export interface ChangeoverMatrixCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus: number;

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
 * 更新ChangeoverMatrix DTO
 * 继承 TaktChangeoverMatrixCreateDto，添加 ChangeoverMatrixId 字段
 * 对应前端 ChangeoverMatrixUpdate
 * @description 对应后端 TaktChangeoverMatrixUpdateDto
 */
export interface ChangeoverMatrixUpdate extends ChangeoverMatrixCreate {
  /**
   * ChangeoverMatrixID（标识要更新的实体）
   */
  changeoverMatrixId: string;

}


/**
 * ChangeoverMatrix 状态更新 DTO
 * 对应前端 ChangeoverMatrixStatus
 * @description 对应后端 TaktChangeoverMatrixStatusDto
 */
export interface ChangeoverMatrixStatus {
  /**
   * ChangeoverMatrixID
   */
  changeoverMatrixId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus: number;

}


/**
 * ChangeoverMatrix 导入模板行 DTO
 * 对应前端 ChangeoverMatrixTemplate
 * @description 对应后端 TaktChangeoverMatrixTemplateDto
 */
export interface ChangeoverMatrixTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode?: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus?: number;

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
 * ChangeoverMatrix 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ChangeoverMatrixImport
 * @description 对应后端 TaktChangeoverMatrixImportDto
 */
export interface ChangeoverMatrixImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode?: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus?: number;

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
 * ChangeoverMatrix 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ChangeoverMatrixExport
 * @description 对应后端 TaktChangeoverMatrixExportDto
 */
export interface ChangeoverMatrixExport {
  /**
   * ChangeoverMatrixID
   */
  changeoverMatrixId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 换型前物料编码
   */
  fromMaterialCode: string;

  /**
   * 换型后物料编码
   */
  toMaterialCode: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus: number;

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

