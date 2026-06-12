// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/statistics/report
// 文件名称：configurable-join.d.ts
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
 * 自定义报表多表关联定义
 * 对应前端 TaktConfigurableJoinDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableJoin
 * @description 对应后端 TaktConfigurableJoinDto
 */
export interface ConfigurableJoin extends CompanyDtoBase {
  /**
   * ConfigurableJoinID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  configurableJoinId: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 关联报表主表 名称（填充字段）
   */
  configurableName?: string;

  /**
   * 关联类型（内/左/右/全连接）
   */
  joinType: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias: string;

  /**
   * 左表关联列名
   */
  leftColumnName: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias: string;

  /**
   * 右表关联列名
   */
  rightColumnName: string;

  /**
   * 排序号（JOIN 应用顺序）
   */
  sortOrder: number;

  /**
   * 关联的报表主表 （主表：TaktConfigurable）
   */
  configurable?: Configurable;

}


/**
 * ConfigurableJoin 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConfigurableJoinQuery
 * @description 对应后端 TaktConfigurableJoinQueryDto
 */
export interface ConfigurableJoinQuery extends TaktPagedQuery {
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
   * 关联类型（内/左/右/全连接）
   */
  joinType?: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias?: string;

  /**
   * 左表关联列名
   */
  leftColumnName?: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias?: string;

  /**
   * 右表关联列名
   */
  rightColumnName?: string;

  /**
   * 排序号（JOIN 应用顺序）
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
 * 创建ConfigurableJoin DTO
 * 对应前端 ConfigurableJoinCreate
 * @description 对应后端 TaktConfigurableJoinCreateDto
 */
export interface ConfigurableJoinCreate {
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
   * 关联类型（内/左/右/全连接）
   */
  joinType: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias: string;

  /**
   * 左表关联列名
   */
  leftColumnName: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias: string;

  /**
   * 右表关联列名
   */
  rightColumnName: string;

  /**
   * 排序号（JOIN 应用顺序）
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
 * 更新ConfigurableJoin DTO
 * 继承 TaktConfigurableJoinCreateDto，添加 ConfigurableJoinId 字段
 * 对应前端 ConfigurableJoinUpdate
 * @description 对应后端 TaktConfigurableJoinUpdateDto
 */
export interface ConfigurableJoinUpdate extends ConfigurableJoinCreate {
  /**
   * ConfigurableJoinID（标识要更新的实体）
   */
  configurableJoinId: string;

}


/**
 * ConfigurableJoin 排序更新 DTO
 * 对应前端 ConfigurableJoinSort
 * @description 对应后端 TaktConfigurableJoinSortDto
 */
export interface ConfigurableJoinSort {
  /**
   * ConfigurableJoinID
   */
  configurableJoinId: string;

  /**
   * 排序号（JOIN 应用顺序）
   */
  sortOrder: number;

}


/**
 * ConfigurableJoin 导入模板行 DTO
 * 对应前端 ConfigurableJoinTemplate
 * @description 对应后端 TaktConfigurableJoinTemplateDto
 */
export interface ConfigurableJoinTemplate {
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
   * 关联类型（内/左/右/全连接）
   */
  joinType?: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias?: string;

  /**
   * 左表关联列名
   */
  leftColumnName?: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias?: string;

  /**
   * 右表关联列名
   */
  rightColumnName?: string;

  /**
   * 排序号（JOIN 应用顺序）
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
 * ConfigurableJoin 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConfigurableJoinImport
 * @description 对应后端 TaktConfigurableJoinImportDto
 */
export interface ConfigurableJoinImport {
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
   * 关联类型（内/左/右/全连接）
   */
  joinType?: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias?: string;

  /**
   * 左表关联列名
   */
  leftColumnName?: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias?: string;

  /**
   * 右表关联列名
   */
  rightColumnName?: string;

  /**
   * 排序号（JOIN 应用顺序）
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
 * ConfigurableJoin 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableJoinExport
 * @description 对应后端 TaktConfigurableJoinExportDto
 */
export interface ConfigurableJoinExport {
  /**
   * ConfigurableJoinID
   */
  configurableJoinId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 关联类型（内/左/右/全连接）
   */
  joinType: number;

  /**
   * 左表数据源别名
   */
  leftSourceAlias: string;

  /**
   * 左表关联列名
   */
  leftColumnName: string;

  /**
   * 右表数据源别名
   */
  rightSourceAlias: string;

  /**
   * 右表关联列名
   */
  rightColumnName: string;

  /**
   * 排序号（JOIN 应用顺序）
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

