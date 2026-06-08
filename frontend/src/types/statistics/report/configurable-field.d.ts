// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable-field.d.ts
// 创建时间：2026-06-08
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
 * 自定义报表输出字段定义
 * 对应前端 TaktConfigurableFieldDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableField
 * @description 对应后端 TaktConfigurableFieldDto
 */
export interface ConfigurableField extends CompanyDtoBase {
  /**
   * ConfigurableFieldID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableFieldId: string;

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
   * 显示名称（表头/Excel 列标题）
   */
  displayName: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible: number;

  /**
   * 排序号（SELECT 列顺序）
   */
  sortOrder: number;

  /**
   * 关联的报表主表 （主表：TaktConfigurable）
   */
  configurable?: Configurable;

}


/**
 * ConfigurableField 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableFieldQuery
 * @description 对应后端 TaktConfigurableFieldQueryDto
 */
export interface ConfigurableFieldQuery extends TaktPagedQuery {
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
   * 显示名称（表头/Excel 列标题）
   */
  displayName?: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc?: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible?: number;

  /**
   * 排序号（SELECT 列顺序）
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
 * 创建ConfigurableField DTO
 * 对应前端 ConfigurableFieldCreate
 * @description 对应后端 TaktConfigurableFieldCreateDto
 */
export interface ConfigurableFieldCreate {
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
   * 显示名称（表头/Excel 列标题）
   */
  displayName: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible: number;

  /**
   * 排序号（SELECT 列顺序）
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
 * 更新ConfigurableField DTO
 * 继承 TaktConfigurableFieldCreateDto，添加 ConfigurableFieldId 字段
 * 对应前端 ConfigurableFieldUpdate
 * @description 对应后端 TaktConfigurableFieldUpdateDto
 */
export interface ConfigurableFieldUpdate extends ConfigurableFieldCreate {
  /**
   * ConfigurableFieldID（标识要更新的实体）
   */
  configurableFieldId: string;

}


/**
 * ConfigurableField 排序更新 DTO
 * 对应前端 ConfigurableFieldSort
 * @description 对应后端 TaktConfigurableFieldSortDto
 */
export interface ConfigurableFieldSort {
  /**
   * ConfigurableFieldID
   */
  configurableFieldId: string;

  /**
   * 排序号（SELECT 列顺序）
   */
  sortOrder: number;

}


/**
 * ConfigurableField 导入模板行 DTO
 * 对应前端 ConfigurableFieldTemplate
 * @description 对应后端 TaktConfigurableFieldTemplateDto
 */
export interface ConfigurableFieldTemplate {
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
   * 显示名称（表头/Excel 列标题）
   */
  displayName?: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc?: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible?: number;

  /**
   * 排序号（SELECT 列顺序）
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
 * ConfigurableField 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableFieldImport
 * @description 对应后端 TaktConfigurableFieldImportDto
 */
export interface ConfigurableFieldImport {
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
   * 显示名称（表头/Excel 列标题）
   */
  displayName?: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc?: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible?: number;

  /**
   * 排序号（SELECT 列顺序）
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
 * ConfigurableField 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableFieldExport
 * @description 对应后端 TaktConfigurableFieldExportDto
 */
export interface ConfigurableFieldExport {
  /**
   * ConfigurableFieldID
   */
  configurableFieldId: string;

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
   * 显示名称（表头/Excel 列标题）
   */
  displayName: string;

  /**
   * 输出别名（SELECT AS，为空时使用 display_name）
   */
  outputAlias?: string;

  /**
   * 聚合函数（无分组时为 None）
   */
  aggregateFunc: number;

  /**
   * 是否输出（0=隐藏 1=显示）
   */
  isVisible: number;

  /**
   * 排序号（SELECT 列顺序）
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

