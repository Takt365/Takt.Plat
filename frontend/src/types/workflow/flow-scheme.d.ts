// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-scheme.d.ts
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
 * 流程定义实体（前端流程方案 FlowScheme）
 * 对应前端 TaktFlowSchemeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowScheme
 * @description 对应后端 TaktFlowSchemeDto
 */
export interface FlowScheme extends CompanyDtoBase {
  /**
   * FlowSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowSchemeId: string;

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest: number;

  /**
   * 流程分类
   */
  processCategory: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 部署批次号
   */
  deploymentName?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单 名称（填充字段）
   */
  formName?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 关联表单 （主表：TaktFlowForm）
   */
  form?: FlowForm;

}


/**
 * FlowScheme 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowSchemeQuery
 * @description 对应后端 TaktFlowSchemeQueryDto
 */
export interface FlowSchemeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey?: string;

  /**
   * 流程名称
   */
  processName?: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion?: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion?: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest?: number;

  /**
   * 流程分类
   */
  processCategory?: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus?: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState?: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 排序号
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
 * 创建FlowScheme DTO
 * 对应前端 FlowSchemeCreate
 * @description 对应后端 TaktFlowSchemeCreateDto
 */
export interface FlowSchemeCreate {
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
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest: number;

  /**
   * 流程分类
   */
  processCategory: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 排序号
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
 * 更新FlowScheme DTO
 * 继承 TaktFlowSchemeCreateDto，添加 FlowSchemeId 字段
 * 对应前端 FlowSchemeUpdate
 * @description 对应后端 TaktFlowSchemeUpdateDto
 */
export interface FlowSchemeUpdate extends FlowSchemeCreate {
  /**
   * FlowSchemeID（标识要更新的实体）
   */
  flowSchemeId: string;

}


/**
 * FlowScheme 状态更新 DTO
 * 对应前端 FlowSchemeStatus
 * @description 对应后端 TaktFlowSchemeStatusDto
 */
export interface FlowSchemeStatus {
  /**
   * FlowSchemeID
   */
  flowSchemeId: string;

  /**
   * 发布状态
   */
  processStatus: number;

}


/**
 * FlowScheme 排序更新 DTO
 * 对应前端 FlowSchemeSort
 * @description 对应后端 TaktFlowSchemeSortDto
 */
export interface FlowSchemeSort {
  /**
   * FlowSchemeID
   */
  flowSchemeId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * FlowScheme 导入模板行 DTO
 * 对应前端 FlowSchemeTemplate
 * @description 对应后端 TaktFlowSchemeTemplateDto
 */
export interface FlowSchemeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey?: string;

  /**
   * 流程名称
   */
  processName?: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion?: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion?: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest?: number;

  /**
   * 流程分类
   */
  processCategory?: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus?: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState?: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

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
 * FlowScheme 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowSchemeImport
 * @description 对应后端 TaktFlowSchemeImportDto
 */
export interface FlowSchemeImport {
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
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey?: string;

  /**
   * 流程名称
   */
  processName?: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion?: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion?: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest?: number;

  /**
   * 流程分类
   */
  processCategory?: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus?: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState?: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

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
 * FlowScheme 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowSchemeExport
 * @description 对应后端 TaktFlowSchemeExportDto
 */
export interface FlowSchemeExport {
  /**
   * FlowSchemeID
   */
  flowSchemeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程键（公司内业务唯一标识，如 leave）
   */
  processKey: string;

  /**
   * 流程名称
   */
  processName: string;

  /**
   * 定义版本号（同流程键可多版本）
   */
  definitionVersion: number;

  /**
   * 版本标签（如 v1.0.0）
   */
  processVersion: string;

  /**
   * 是否当前最新版（同键仅一条为 1）
   */
  isLatest: number;

  /**
   * 流程分类
   */
  processCategory: number;

  /**
   * 流程说明
   */
  processDescription?: string;

  /**
   * 发布状态
   */
  processStatus: number;

  /**
   * 挂起状态（1 激活，2 挂起）
   */
  suspensionState: number;

  /**
   * 流程设计 JSON（节点、网关、条件、审批人配置）
   */
  processContent?: string;

  /**
   * 部署批次号
   */
  deploymentId?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 排序号
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

