// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：model-destination.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt型号目的地实体（物料名称、机种名称、仕向地名称）
 * 对应前端 TaktModelDestinationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ModelDestination
 * @description 对应后端 TaktModelDestinationDto
 */
export interface ModelDestination extends CompanyDtoBase {
  /**
   * ModelDestinationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  modelDestinationId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ModelDestination 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ModelDestinationQuery
 * @description 对应后端 TaktModelDestinationQueryDto
 */
export interface ModelDestinationQuery extends TaktPagedQuery {
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
   * 物料名称
   */
  materialName?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * 创建ModelDestination DTO
 * 对应前端 ModelDestinationCreate
 * @description 对应后端 TaktModelDestinationCreateDto
 */
export interface ModelDestinationCreate {
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
  plantCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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
 * 更新ModelDestination DTO
 * 继承 TaktModelDestinationCreateDto，添加 ModelDestinationId 字段
 * 对应前端 ModelDestinationUpdate
 * @description 对应后端 TaktModelDestinationUpdateDto
 */
export interface ModelDestinationUpdate extends ModelDestinationCreate {
  /**
   * ModelDestinationID（标识要更新的实体）
   */
  modelDestinationId: string;

}


/**
 * ModelDestination 排序更新 DTO
 * 对应前端 ModelDestinationSort
 * @description 对应后端 TaktModelDestinationSortDto
 */
export interface ModelDestinationSort {
  /**
   * ModelDestinationID
   */
  modelDestinationId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ModelDestination 导入模板行 DTO
 * 对应前端 ModelDestinationTemplate
 * @description 对应后端 TaktModelDestinationTemplateDto
 */
export interface ModelDestinationTemplate {
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
   * 物料名称
   */
  materialName?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * ModelDestination 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ModelDestinationImport
 * @description 对应后端 TaktModelDestinationImportDto
 */
export interface ModelDestinationImport {
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
   * 物料名称
   */
  materialName?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * ModelDestination 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ModelDestinationExport
 * @description 对应后端 TaktModelDestinationExportDto
 */
export interface ModelDestinationExport {
  /**
   * ModelDestinationID
   */
  modelDestinationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

