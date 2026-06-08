// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable-source.d.ts
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
 * 自定义报表数据源（单表及别名）
 * 对应前端 TaktConfigurableSourceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableSource
 * @description 对应后端 TaktConfigurableSourceDto
 */
export interface ConfigurableSource extends CompanyDtoBase {
  /**
   * ConfigurableSourceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableSourceId: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 关联报表主表 名称（填充字段）
   */
  configurableName?: string;

  /**
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary: number;

  /**
   * 排序号（多表 FROM 顺序）
   */
  sortOrder: number;

  /**
   * 关联的报表主表 （主表：TaktConfigurable）
   */
  configurable?: Configurable;

}


/**
 * ConfigurableSource 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableSourceQuery
 * @description 对应后端 TaktConfigurableSourceQueryDto
 */
export interface ConfigurableSourceQuery extends TaktPagedQuery {
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
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias?: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName?: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary?: number;

  /**
   * 排序号（多表 FROM 顺序）
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
 * 创建ConfigurableSource DTO
 * 对应前端 ConfigurableSourceCreate
 * @description 对应后端 TaktConfigurableSourceCreateDto
 */
export interface ConfigurableSourceCreate {
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
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary: number;

  /**
   * 排序号（多表 FROM 顺序）
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
 * 更新ConfigurableSource DTO
 * 继承 TaktConfigurableSourceCreateDto，添加 ConfigurableSourceId 字段
 * 对应前端 ConfigurableSourceUpdate
 * @description 对应后端 TaktConfigurableSourceUpdateDto
 */
export interface ConfigurableSourceUpdate extends ConfigurableSourceCreate {
  /**
   * ConfigurableSourceID（标识要更新的实体）
   */
  configurableSourceId: string;

}


/**
 * ConfigurableSource 排序更新 DTO
 * 对应前端 ConfigurableSourceSort
 * @description 对应后端 TaktConfigurableSourceSortDto
 */
export interface ConfigurableSourceSort {
  /**
   * ConfigurableSourceID
   */
  configurableSourceId: string;

  /**
   * 排序号（多表 FROM 顺序）
   */
  sortOrder: number;

}


/**
 * ConfigurableSource 导入模板行 DTO
 * 对应前端 ConfigurableSourceTemplate
 * @description 对应后端 TaktConfigurableSourceTemplateDto
 */
export interface ConfigurableSourceTemplate {
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
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias?: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName?: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary?: number;

  /**
   * 排序号（多表 FROM 顺序）
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
 * ConfigurableSource 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableSourceImport
 * @description 对应后端 TaktConfigurableSourceImportDto
 */
export interface ConfigurableSourceImport {
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
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias?: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName?: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary?: number;

  /**
   * 排序号（多表 FROM 顺序）
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
 * ConfigurableSource 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableSourceExport
 * @description 对应后端 TaktConfigurableSourceExportDto
 */
export interface ConfigurableSourceExport {
  /**
   * ConfigurableSourceID
   */
  configurableSourceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary: number;

  /**
   * 排序号（多表 FROM 顺序）
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

