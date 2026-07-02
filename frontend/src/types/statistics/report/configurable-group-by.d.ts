// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable-group-by.d.ts
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
 * 自定义报表分组字段定义
 * 对应前端 TaktConfigurableGroupByDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableGroupBy
 * @description 对应后端 TaktConfigurableGroupByDto
 */
export interface ConfigurableGroupBy extends CompanyDtoBase {
  /**
   * ConfigurableGroupByID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableGroupById: string;

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
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder: number;

  /**
   * 关联的报表主表 （主表：TaktConfigurable）
   */
  configurable?: Configurable;

}


/**
 * ConfigurableGroupBy 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableGroupByQuery
 * @description 对应后端 TaktConfigurableGroupByQueryDto
 */
export interface ConfigurableGroupByQuery extends TaktPagedQuery {
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
   * 排序号（GROUP BY 列顺序）
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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ConfigurableGroupBy DTO
 * 对应前端 ConfigurableGroupByCreate
 * @description 对应后端 TaktConfigurableGroupByCreateDto
 */
export interface ConfigurableGroupByCreate {
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
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder: number;

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
 * 更新ConfigurableGroupBy DTO
 * 继承 TaktConfigurableGroupByCreateDto，添加 ConfigurableGroupById 字段
 * 对应前端 ConfigurableGroupByUpdate
 * @description 对应后端 TaktConfigurableGroupByUpdateDto
 */
export interface ConfigurableGroupByUpdate extends ConfigurableGroupByCreate {
  /**
   * ConfigurableGroupByID（标识要更新的实体）
   */
  configurableGroupById: string;

}


/**
 * ConfigurableGroupBy 排序更新 DTO
 * 对应前端 ConfigurableGroupBySort
 * @description 对应后端 TaktConfigurableGroupBySortDto
 */
export interface ConfigurableGroupBySort {
  /**
   * ConfigurableGroupByID
   */
  configurableGroupById: string;

  /**
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder: number;

}


/**
 * ConfigurableGroupBy 导入模板行 DTO
 * 对应前端 ConfigurableGroupByTemplate
 * @description 对应后端 TaktConfigurableGroupByTemplateDto
 */
export interface ConfigurableGroupByTemplate {
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
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder?: number;

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
 * ConfigurableGroupBy 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableGroupByImport
 * @description 对应后端 TaktConfigurableGroupByImportDto
 */
export interface ConfigurableGroupByImport {
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
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder?: number;

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
 * ConfigurableGroupBy 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableGroupByExport
 * @description 对应后端 TaktConfigurableGroupByExportDto
 */
export interface ConfigurableGroupByExport {
  /**
   * ConfigurableGroupByID
   */
  configurableGroupById: string;

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
   * 排序号（GROUP BY 列顺序）
   */
  sortOrder: number;

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

