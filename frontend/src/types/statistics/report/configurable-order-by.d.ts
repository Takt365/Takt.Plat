// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable-order-by.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/report 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 自定义报表排序字段定义
 * 对应前端 TaktConfigurableOrderByDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableOrderBy
 * @description 对应后端 TaktConfigurableOrderByDto
 */
export interface ConfigurableOrderBy extends CompanyDtoBase {
  /**
   * ConfigurableOrderByID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableOrderById: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 关联报表主表 名称（填充字段）
   */
  configurableName?: string;

  /**
   * 数据源别名
   */
  sourceAlias: string;

  /**
   * 列名
   */
  columnName: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection: number;

  /**
   * 排序号（ORDER BY 优先级）
   */
  sortOrder: number;

  /**
   * 关联的报表主表 （主表：TaktConfigurable）
   */
  configurable?: Configurable;

}


/**
 * ConfigurableOrderBy 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableOrderByQuery
 * @description 对应后端 TaktConfigurableOrderByQueryDto
 */
export interface ConfigurableOrderByQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId?: string;

  /**
   * 数据源别名
   */
  sourceAlias?: string;

  /**
   * 列名
   */
  columnName?: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection?: number;

  /**
   * 排序号（ORDER BY 优先级）
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
 * 创建ConfigurableOrderBy DTO
 * 对应前端 ConfigurableOrderByCreate
 * @description 对应后端 TaktConfigurableOrderByCreateDto
 */
export interface ConfigurableOrderByCreate {
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
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 数据源别名
   */
  sourceAlias: string;

  /**
   * 列名
   */
  columnName: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection: number;

  /**
   * 排序号（ORDER BY 优先级）
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
 * 更新ConfigurableOrderBy DTO
 * 继承 TaktConfigurableOrderByCreateDto，添加 ConfigurableOrderById 字段
 * 对应前端 ConfigurableOrderByUpdate
 * @description 对应后端 TaktConfigurableOrderByUpdateDto
 */
export interface ConfigurableOrderByUpdate extends ConfigurableOrderByCreate {
  /**
   * ConfigurableOrderByID（标识要更新的实体）
   */
  configurableOrderById: string;

}


/**
 * ConfigurableOrderBy 排序更新 DTO
 * 对应前端 ConfigurableOrderBySort
 * @description 对应后端 TaktConfigurableOrderBySortDto
 */
export interface ConfigurableOrderBySort {
  /**
   * ConfigurableOrderByID
   */
  configurableOrderById: string;

  /**
   * 排序号（ORDER BY 优先级）
   */
  sortOrder: number;

}


/**
 * ConfigurableOrderBy 导入模板行 DTO
 * 对应前端 ConfigurableOrderByTemplate
 * @description 对应后端 TaktConfigurableOrderByTemplateDto
 */
export interface ConfigurableOrderByTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId?: string;

  /**
   * 数据源别名
   */
  sourceAlias?: string;

  /**
   * 列名
   */
  columnName?: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection?: number;

  /**
   * 排序号（ORDER BY 优先级）
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
 * ConfigurableOrderBy 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableOrderByImport
 * @description 对应后端 TaktConfigurableOrderByImportDto
 */
export interface ConfigurableOrderByImport {
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
   * 关联报表主表 ID（主子表关系）
   */
  configurableId?: string;

  /**
   * 数据源别名
   */
  sourceAlias?: string;

  /**
   * 列名
   */
  columnName?: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection?: number;

  /**
   * 排序号（ORDER BY 优先级）
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
 * ConfigurableOrderBy 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableOrderByExport
 * @description 对应后端 TaktConfigurableOrderByExportDto
 */
export interface ConfigurableOrderByExport {
  /**
   * ConfigurableOrderByID
   */
  configurableOrderById: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 数据源别名
   */
  sourceAlias: string;

  /**
   * 列名
   */
  columnName: string;

  /**
   * 排序方向（升序/降序）
   */
  sortDirection: number;

  /**
   * 排序号（ORDER BY 优先级）
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

