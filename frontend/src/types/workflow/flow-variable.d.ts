// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-variable.d.ts
// 创建时间：2026-06-05
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
 * 流程变量实体
 * 对应前端 TaktFlowVariableDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowVariable
 * @description 对应后端 TaktFlowVariableDto
 */
export interface FlowVariable extends CompanyDtoBase {
  /**
   * FlowVariableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowVariableId: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 流程实例 名称（填充字段）
   */
  instanceName?: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 任务 名称（填充字段）
   */
  taskName?: string;

  /**
   * 变量名
   */
  variableName: string;

  /**
   * 变量类型
   */
  variableType: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 双精度值
   */
  doubleValue?: number;

  /**
   * 所属流程实例 （主表：TaktFlowInstance）
   */
  instance?: FlowInstance;

}


/**
 * FlowVariable 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowVariableQuery
 * @description 对应后端 TaktFlowVariableQueryDto
 */
export interface FlowVariableQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName?: string;

  /**
   * 变量类型
   */
  variableType?: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 双精度值
   */
  doubleValue?: number;

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
 * 创建FlowVariable DTO
 * 对应前端 FlowVariableCreate
 * @description 对应后端 TaktFlowVariableCreateDto
 */
export interface FlowVariableCreate {
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
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName: string;

  /**
   * 变量类型
   */
  variableType: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 双精度值
   */
  doubleValue?: number;

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
 * 更新FlowVariable DTO
 * 继承 TaktFlowVariableCreateDto，添加 FlowVariableId 字段
 * 对应前端 FlowVariableUpdate
 * @description 对应后端 TaktFlowVariableUpdateDto
 */
export interface FlowVariableUpdate extends FlowVariableCreate {
  /**
   * FlowVariableID（标识要更新的实体）
   */
  flowVariableId: string;

}


/**
 * FlowVariable 导入模板行 DTO
 * 对应前端 FlowVariableTemplate
 * @description 对应后端 TaktFlowVariableTemplateDto
 */
export interface FlowVariableTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName?: string;

  /**
   * 变量类型
   */
  variableType?: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

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
 * FlowVariable 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowVariableImport
 * @description 对应后端 TaktFlowVariableImportDto
 */
export interface FlowVariableImport {
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
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName?: string;

  /**
   * 变量类型
   */
  variableType?: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

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
 * FlowVariable 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowVariableExport
 * @description 对应后端 TaktFlowVariableExportDto
 */
export interface FlowVariableExport {
  /**
   * FlowVariableID
   */
  flowVariableId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 任务 ID（任务级变量时填写）
   */
  taskId?: string;

  /**
   * 变量名
   */
  variableName: string;

  /**
   * 变量类型
   */
  variableType: number;

  /**
   * 文本值（JSON 变量存此列）
   */
  textValue?: string;

  /**
   * 长整型值
   */
  longValue?: string;

  /**
   * 双精度值
   */
  doubleValue?: number;

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

