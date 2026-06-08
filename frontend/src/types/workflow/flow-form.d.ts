// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-form.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 流程表单定义实体
 * 对应前端 TaktFlowFormDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowForm
 * @description 对应后端 TaktFlowFormDto
 */
export interface FlowForm extends CompanyDtoBase {
  /**
   * FlowFormID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowFormId: string;

  /**
   * 表单编码（公司内唯一）
   */
  formCode: string;

  /**
   * 表单名称
   */
  formName: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion: string;

  /**
   * 是否绑定数据源
   */
  isDatasource: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 表单状态
   */
  formStatus: number;

}


/**
 * FlowForm 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowFormQuery
 * @description 对应后端 TaktFlowFormQueryDto
 */
export interface FlowFormQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 表单编码（公司内唯一）
   */
  formCode?: string;

  /**
   * 表单名称
   */
  formName?: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory?: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType?: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion?: string;

  /**
   * 是否绑定数据源
   */
  isDatasource?: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 表单状态
   */
  formStatus?: number;

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
 * 创建FlowForm DTO
 * 对应前端 FlowFormCreate
 * @description 对应后端 TaktFlowFormCreateDto
 */
export interface FlowFormCreate {
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
   * 表单编码（公司内唯一）
   */
  formCode: string;

  /**
   * 表单名称
   */
  formName: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion: string;

  /**
   * 是否绑定数据源
   */
  isDatasource: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 表单状态
   */
  formStatus: number;

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
 * 更新FlowForm DTO
 * 继承 TaktFlowFormCreateDto，添加 FlowFormId 字段
 * 对应前端 FlowFormUpdate
 * @description 对应后端 TaktFlowFormUpdateDto
 */
export interface FlowFormUpdate extends FlowFormCreate {
  /**
   * FlowFormID（标识要更新的实体）
   */
  flowFormId: string;

}


/**
 * FlowForm 状态更新 DTO
 * 对应前端 FlowFormStatus
 * @description 对应后端 TaktFlowFormStatusDto
 */
export interface FlowFormStatus {
  /**
   * FlowFormID
   */
  flowFormId: string;

  /**
   * 表单状态
   */
  formStatus: number;

}


/**
 * FlowForm 排序更新 DTO
 * 对应前端 FlowFormSort
 * @description 对应后端 TaktFlowFormSortDto
 */
export interface FlowFormSort {
  /**
   * FlowFormID
   */
  flowFormId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * FlowForm 导入模板行 DTO
 * 对应前端 FlowFormTemplate
 * @description 对应后端 TaktFlowFormTemplateDto
 */
export interface FlowFormTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 表单编码（公司内唯一）
   */
  formCode?: string;

  /**
   * 表单名称
   */
  formName?: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory?: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType?: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion?: string;

  /**
   * 是否绑定数据源
   */
  isDatasource?: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
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
 * FlowForm 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowFormImport
 * @description 对应后端 TaktFlowFormImportDto
 */
export interface FlowFormImport {
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
   * 表单编码（公司内唯一）
   */
  formCode?: string;

  /**
   * 表单名称
   */
  formName?: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory?: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType?: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion?: string;

  /**
   * 是否绑定数据源
   */
  isDatasource?: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
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
 * FlowForm 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowFormExport
 * @description 对应后端 TaktFlowFormExportDto
 */
export interface FlowFormExport {
  /**
   * FlowFormID
   */
  flowFormId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 表单编码（公司内唯一）
   */
  formCode: string;

  /**
   * 表单名称
   */
  formName: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion: string;

  /**
   * 是否绑定数据源
   */
  isDatasource: number;

  /**
   * 关联数据库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联字段映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 表单状态
   */
  formStatus: number;

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

