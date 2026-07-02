// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：production-changeover.d.ts
// 创建时间：2026-06-23
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
 * 生产切换记录实体
 * 对应前端 TaktProductionChangeoverDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionChangeover
 * @description 对应后端 TaktProductionChangeoverDto
 */
export interface ProductionChangeover extends CompanyDtoBase {
  /**
   * ProductionChangeoverID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionChangeoverId: string;

  /**
   * 生产工厂
   */
  plantCode: string;

  /**
   * 生产类别
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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
 * ProductionChangeover 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionChangeoverQuery
 * @description 对应后端 TaktProductionChangeoverQueryDto
 */
export interface ProductionChangeoverQuery extends TaktPagedQuery {
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
  prodCategory?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  prodDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  prodDateEnd?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ProductionChangeover DTO
 * 对应前端 ProductionChangeoverCreate
 * @description 对应后端 TaktProductionChangeoverCreateDto
 */
export interface ProductionChangeoverCreate {
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
   * 生产工厂
   */
  plantCode: string;

  /**
   * 生产类别
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新ProductionChangeover DTO
 * 继承 TaktProductionChangeoverCreateDto，添加 ProductionChangeoverId 字段
 * 对应前端 ProductionChangeoverUpdate
 * @description 对应后端 TaktProductionChangeoverUpdateDto
 */
export interface ProductionChangeoverUpdate extends ProductionChangeoverCreate {
  /**
   * ProductionChangeoverID（标识要更新的实体）
   */
  productionChangeoverId: string;

}


/**
 * ProductionChangeover 导入模板行 DTO
 * 对应前端 ProductionChangeoverTemplate
 * @description 对应后端 TaktProductionChangeoverTemplateDto
 */
export interface ProductionChangeoverTemplate {
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
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ProductionChangeover 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionChangeoverImport
 * @description 对应后端 TaktProductionChangeoverImportDto
 */
export interface ProductionChangeoverImport {
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
   * 生产工厂
   */
  plantCode?: string;

  /**
   * 生产类别
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * ProductionChangeover 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionChangeoverExport
 * @description 对应后端 TaktProductionChangeoverExportDto
 */
export interface ProductionChangeoverExport {
  /**
   * ProductionChangeoverID
   */
  productionChangeoverId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产工厂
   */
  plantCode: string;

  /**
   * 生产类别
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode；最大 20 字符）
   */
  prodTeam?: string;

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

