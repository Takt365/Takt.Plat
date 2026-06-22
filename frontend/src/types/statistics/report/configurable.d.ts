// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable.d.ts
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
 * 自定义报表主实体（SQVI 查询定义）
 * 对应前端 TaktConfigurableDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Configurable
 * @description 对应后端 TaktConfigurableDto
 */
export interface Configurable extends CompanyDtoBase {
  /**
   * ConfigurableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableId: string;

  /**
   * 报表编码（租户+公司内唯一）
   */
  reportCode: string;

  /**
   * 报表名称
   */
  reportName: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus: number;

  /**
   * 报表描述
   */
  description?: string;

  /**
   * 数据源表列表（FROM） （子表：TaktConfigurableSource）
   */
  sources?: ConfigurableSource[];

  /**
   * 多表关联列表（JOIN） （子表：TaktConfigurableJoin）
   */
  joins?: ConfigurableJoin[];

  /**
   * 输出字段列表（SELECT） （子表：TaktConfigurableField）
   */
  fields?: ConfigurableField[];

  /**
   * 筛选条件列表（SQVI WHERE） （子表：TaktConfigurableSelection）
   */
  selections?: ConfigurableSelection[];

  /**
   * 分组字段列表（GROUP BY） （子表：TaktConfigurableGroupBy）
   */
  groupBys?: ConfigurableGroupBy[];

  /**
   * 排序字段列表（ORDER BY） （子表：TaktConfigurableOrderBy）
   */
  orderBys?: ConfigurableOrderBy[];

}


/**
 * Configurable 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableQuery
 * @description 对应后端 TaktConfigurableQueryDto
 */
export interface ConfigurableQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 报表编码（租户+公司内唯一）
   */
  reportCode?: string;

  /**
   * 报表名称
   */
  reportName?: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain?: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows?: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows?: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows?: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus?: number;

  /**
   * 报表描述
   */
  description?: string;

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
 * 创建Configurable DTO
 * 对应前端 ConfigurableCreate
 * @description 对应后端 TaktConfigurableCreateDto
 */
export interface ConfigurableCreate {
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
   * 报表编码（租户+公司内唯一）
   */
  reportCode: string;

  /**
   * 报表名称
   */
  reportName: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus: number;

  /**
   * 报表描述
   */
  description?: string;

  /**
   * 数据源表列表（FROM）（子表，级联保存）
   */
  sources?: ConfigurableSourceCreate[];

  /**
   * 多表关联列表（JOIN）（子表，级联保存）
   */
  joins?: ConfigurableJoinCreate[];

  /**
   * 输出字段列表（SELECT）（子表，级联保存）
   */
  fields?: ConfigurableFieldCreate[];

  /**
   * 筛选条件列表（SQVI WHERE）（子表，级联保存）
   */
  selections?: ConfigurableSelectionCreate[];

  /**
   * 分组字段列表（GROUP BY）（子表，级联保存）
   */
  groupBys?: ConfigurableGroupByCreate[];

  /**
   * 排序字段列表（ORDER BY）（子表，级联保存）
   */
  orderBys?: ConfigurableOrderByCreate[];

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
 * 更新Configurable DTO
 * 继承 TaktConfigurableCreateDto，添加 ConfigurableId 字段
 * 对应前端 ConfigurableUpdate
 * @description 对应后端 TaktConfigurableUpdateDto
 */
export interface ConfigurableUpdate extends ConfigurableCreate {
  /**
   * ConfigurableID（标识要更新的实体）
   */
  configurableId: string;

}


/**
 * Configurable 状态更新 DTO
 * 对应前端 ConfigurableStatus
 * @description 对应后端 TaktConfigurableStatusDto
 */
export interface ConfigurableStatus {
  /**
   * ConfigurableID
   */
  configurableId: string;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus: number;

}


/**
 * Configurable 排序更新 DTO
 * 对应前端 ConfigurableSort
 * @description 对应后端 TaktConfigurableSortDto
 */
export interface ConfigurableSort {
  /**
   * ConfigurableID
   */
  configurableId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Configurable 导入模板行 DTO
 * 对应前端 ConfigurableTemplate
 * @description 对应后端 TaktConfigurableTemplateDto
 */
export interface ConfigurableTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 报表编码（租户+公司内唯一）
   */
  reportCode?: string;

  /**
   * 报表名称
   */
  reportName?: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain?: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows?: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows?: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows?: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus?: number;

  /**
   * 报表描述
   */
  description?: string;

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
 * Configurable 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableImport
 * @description 对应后端 TaktConfigurableImportDto
 */
export interface ConfigurableImport {
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
   * 报表编码（租户+公司内唯一）
   */
  reportCode?: string;

  /**
   * 报表名称
   */
  reportName?: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain?: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows?: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows?: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows?: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus?: number;

  /**
   * 报表描述
   */
  description?: string;

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
 * Configurable 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableExport
 * @description 对应后端 TaktConfigurableExportDto
 */
export interface ConfigurableExport {
  /**
   * ConfigurableID
   */
  configurableId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 报表编码（租户+公司内唯一）
   */
  reportCode: string;

  /**
   * 报表名称
   */
  reportName: string;

  /**
   * 报表业务域（财务/人力/后勤等）
   */
  reportDomain: number;

  /**
   * 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  reportSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows: number;

  /**
   * 公开（字典 sys_is_public_type；0=公开，1=私有）
   */
  isPublic: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 报表状态（0=禁用 1=启用）
   */
  reportStatus: number;

  /**
   * 报表描述
   */
  description?: string;

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

// ========================================
// SQVI 运行时
// ========================================

/**
 * 运行时结果列
 */
export interface ConfigurableRuntimeColumn {
  /**
   * 列键
   */
  key: string;

  /**
   * 列显示名
   */
  label: string;
}

/**
 * SQVI 筛选项
 */
export interface ConfigurableRuntimeSelection {
  /**
   * 筛选项主键（运行时表单独立绑定）
   */
  configurableSelectionId?: string;

  /**
   * 排序号（运行时取值键）
   */
  sortOrder: number;

  /**
   * 数据源别名
   */
  sourceAlias: string;

  /**
   * 列名
   */
  columnName: string;

  /**
   * 显示名称
   */
  displayName: string;

  /**
   * 比较运算符
   */
  filterOperator: number;

  /**
   * 是否必填
   */
  isRequired: number;

  /**
   * 默认值
   */
  defaultValue?: string;

  /**
   * 区间结束默认值
   */
  defaultValueTo?: string;
}

/**
 * SQVI 运行时筛选条件
 */
export interface ConfigurableRuntimeScreen {
  /**
   * 报表主键
   */
  configurableId: string;

  /**
   * 报表编码
   */
  reportCode: string;

  /**
   * 报表名称
   */
  reportName: string;

  /**
   * 查询最大行数
   */
  maxQueryRows: number;

  /**
   * 导出最大行数
   */
  maxExportRows: number;

  /**
   * 输出列
   */
  columns: ConfigurableRuntimeColumn[];

  /**
   * 筛选项
   */
  selections: ConfigurableRuntimeSelection[];
}

/**
 * 运行时筛选值
 */
export interface ConfigurableRuntimeSelectionValue {
  /**
   * 筛选项主键（优先于 sortOrder 匹配）
   */
  configurableSelectionId?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 筛选值
   */
  value?: string;

  /**
   * 区间结束值
   */
  valueTo?: string;

  /**
   * 运行时比较运算符（1～8，与 gen_query_type SortOrder 一致）
   */
  filterOperator?: number;
}

/**
 * 执行报表查询请求
 */
export interface ConfigurableExecuteQuery extends TaktPagedQuery {
  /**
   * 筛选值列表
   */
  selectionValues: ConfigurableRuntimeSelectionValue[];

  /**
   * 本次查询行数上限（0 或未传默认 500，最大 50000）
   */
  rowLimit?: number;
}

/**
 * 报表查询结果
 */
export interface ConfigurableQueryResult {
  /**
   * 输出列
   */
  columns: ConfigurableRuntimeColumn[];

  /**
   * 数据行
   */
  rows: Record<string, unknown>[];

  /**
   * 总记录数
   */
  total: number;

  /**
   * 当前页码
   */
  pageIndex: number;

  /**
   * 每页大小
   */
  pageSize: number;
}

/**
 * 导出报表数据请求
 */
export interface ConfigurableExportData {
  /**
   * 筛选值列表
   */
  selectionValues: ConfigurableRuntimeSelectionValue[];

  /**
   * 本次导出行数上限（0 或未传默认 500，最大 50000）
   */
  rowLimit?: number;
}

