// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：changeover.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 切换记录实体
 * 对应前端 TaktChangeoverDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Changeover
 * @description 对应后端 TaktChangeoverDto
 */
export interface Changeover extends CompanyDtoBase {
  /**
   * ChangeoverID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  changeoverId: string;

  /**
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产日期
   */
  productionDate: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 读取SOP时间
   */
  readSopTime: number;

  /**
   * 人数
   */
  personCount: number;

  /**
   * SOP总时间
   */
  totalSopTime: number;

  /**
   * 切换次数
   */
  changeoverCount: number;

  /**
   * 切换时间（单次）
   */
  changeoverTime: number;

  /**
   * 切换总时间
   */
  totalChangeoverTime: number;

}


/**
 * Changeover 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ChangeoverQuery
 * @description 对应后端 TaktChangeoverQueryDto
 */
export interface ChangeoverQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 读取SOP时间
   */
  readSopTime?: number;

  /**
   * 人数
   */
  personCount?: number;

  /**
   * SOP总时间
   */
  totalSopTime?: number;

  /**
   * 切换次数
   */
  changeoverCount?: number;

  /**
   * 切换时间（单次）
   */
  changeoverTime?: number;

  /**
   * 切换总时间
   */
  totalChangeoverTime?: number;

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
 * 创建Changeover DTO
 * 对应前端 ChangeoverCreate
 * @description 对应后端 TaktChangeoverCreateDto
 */
export interface ChangeoverCreate {
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
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产日期
   */
  productionDate: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 读取SOP时间
   */
  readSopTime: number;

  /**
   * 人数
   */
  personCount: number;

  /**
   * SOP总时间
   */
  totalSopTime: number;

  /**
   * 切换次数
   */
  changeoverCount: number;

  /**
   * 切换时间（单次）
   */
  changeoverTime: number;

  /**
   * 切换总时间
   */
  totalChangeoverTime: number;

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
 * 更新Changeover DTO
 * 继承 TaktChangeoverCreateDto，添加 ChangeoverId 字段
 * 对应前端 ChangeoverUpdate
 * @description 对应后端 TaktChangeoverUpdateDto
 */
export interface ChangeoverUpdate extends ChangeoverCreate {
  /**
   * ChangeoverID（标识要更新的实体）
   */
  changeoverId: string;

}


/**
 * Changeover 导入模板行 DTO
 * 对应前端 ChangeoverTemplate
 * @description 对应后端 TaktChangeoverTemplateDto
 */
export interface ChangeoverTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 人数
   */
  personCount?: number;

  /**
   * 切换次数
   */
  changeoverCount?: number;

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
 * Changeover 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ChangeoverImport
 * @description 对应后端 TaktChangeoverImportDto
 */
export interface ChangeoverImport {
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
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 人数
   */
  personCount?: number;

  /**
   * 切换次数
   */
  changeoverCount?: number;

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
 * Changeover 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ChangeoverExport
 * @description 对应后端 TaktChangeoverExportDto
 */
export interface ChangeoverExport {
  /**
   * ChangeoverID
   */
  changeoverId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  productionCategory?: string;

  /**
   * 生产日期
   */
  productionDate: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 读取SOP时间
   */
  readSopTime: number;

  /**
   * 人数
   */
  personCount: number;

  /**
   * SOP总时间
   */
  totalSopTime: number;

  /**
   * 切换次数
   */
  changeoverCount: number;

  /**
   * 切换时间（单次）
   */
  changeoverTime: number;

  /**
   * 切换总时间
   */
  totalChangeoverTime: number;

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

