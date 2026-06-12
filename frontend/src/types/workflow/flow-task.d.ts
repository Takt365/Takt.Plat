// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-task.d.ts
// 创建时间：2026-06-09
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
 * 流程用户任务实体
 * 对应前端 TaktFlowTaskDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowTask
 * @description 对应后端 TaktFlowTaskDto
 */
export interface FlowTask extends CompanyDtoBase {
  /**
   * FlowTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowTaskId: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 流程实例 名称（填充字段）
   */
  instanceName?: string;

  /**
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务所有者 名称（填充字段）
   */
  ownerUserName?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 会签类型
   */
  signType: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 到期时间
   */
  dueDate?: string;

  /**
   * 认领时间
   */
  claimTime?: string;

  /**
   * 办结时间
   */
  completedAt?: string;

  /**
   * 是否加签任务
   */
  isAddSign: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 加签记录 名称（填充字段）
   */
  addSignName?: string;

  /**
   * 多实例序号
   */
  sortOrder: number;

  /**
   * 审批意见
   */
  comment?: string;

  /**
   * 所属流程实例 （主表：TaktFlowInstance）
   */
  instance?: FlowInstance;

}


/**
 * FlowTask 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowTaskQuery
 * @description 对应后端 TaktFlowTaskQueryDto
 */
export interface FlowTaskQuery extends TaktPagedQuery {
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
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId?: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 会签类型
   */
  signType?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 到期时间（范围查询-开始）
   */
  dueDateStart?: string;

  /**
   * 到期时间（范围查询-结束）
   */
  dueDateEnd?: string;

  /**
   * 认领时间（范围查询-开始）
   */
  claimTimeStart?: string;

  /**
   * 认领时间（范围查询-结束）
   */
  claimTimeEnd?: string;

  /**
   * 办结时间（范围查询-开始）
   */
  completedAtStart?: string;

  /**
   * 办结时间（范围查询-结束）
   */
  completedAtEnd?: string;

  /**
   * 是否加签任务
   */
  isAddSign?: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
   */
  sortOrder?: number;

  /**
   * 审批意见
   */
  comment?: string;

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
 * 创建FlowTask DTO
 * 对应前端 FlowTaskCreate
 * @description 对应后端 TaktFlowTaskCreateDto
 */
export interface FlowTaskCreate {
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
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 会签类型
   */
  signType: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 到期时间
   */
  dueDate?: string;

  /**
   * 认领时间
   */
  claimTime?: string;

  /**
   * 办结时间
   */
  completedAt?: string;

  /**
   * 是否加签任务
   */
  isAddSign: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
   */
  sortOrder: number;

  /**
   * 审批意见
   */
  comment?: string;

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
 * 更新FlowTask DTO
 * 继承 TaktFlowTaskCreateDto，添加 FlowTaskId 字段
 * 对应前端 FlowTaskUpdate
 * @description 对应后端 TaktFlowTaskUpdateDto
 */
export interface FlowTaskUpdate extends FlowTaskCreate {
  /**
   * FlowTaskID（标识要更新的实体）
   */
  flowTaskId: string;

}


/**
 * FlowTask 状态更新 DTO
 * 对应前端 FlowTaskStatus
 * @description 对应后端 TaktFlowTaskStatusDto
 */
export interface FlowTaskStatus {
  /**
   * FlowTaskID
   */
  flowTaskId: string;

  /**
   * 任务状态
   */
  taskStatus: number;

}


/**
 * FlowTask 排序更新 DTO
 * 对应前端 FlowTaskSort
 * @description 对应后端 TaktFlowTaskSortDto
 */
export interface FlowTaskSort {
  /**
   * FlowTaskID
   */
  flowTaskId: string;

  /**
   * 多实例序号
   */
  sortOrder: number;

}


/**
 * FlowTask 导入模板行 DTO
 * 对应前端 FlowTaskTemplate
 * @description 对应后端 TaktFlowTaskTemplateDto
 */
export interface FlowTaskTemplate {
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
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId?: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 会签类型
   */
  signType?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 是否加签任务
   */
  isAddSign?: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
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
 * FlowTask 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowTaskImport
 * @description 对应后端 TaktFlowTaskImportDto
 */
export interface FlowTaskImport {
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
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId?: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 会签类型
   */
  signType?: number;

  /**
   * 优先级
   */
  priority?: number;

  /**
   * 是否加签任务
   */
  isAddSign?: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
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
 * FlowTask 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowTaskExport
 * @description 对应后端 TaktFlowTaskExportDto
 */
export interface FlowTaskExport {
  /**
   * FlowTaskID
   */
  flowTaskId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 任务定义键（设计器节点 nodeId）
   */
  taskDefinitionKey: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * 办理人 ID
   */
  assigneeUserId: string;

  /**
   * 办理人姓名
   */
  assigneeUserName?: string;

  /**
   * 任务所有者 ID（转办前原办理人）
   */
  ownerUserId?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 会签类型
   */
  signType: number;

  /**
   * 优先级
   */
  priority: number;

  /**
   * 到期时间
   */
  dueDate?: string;

  /**
   * 认领时间
   */
  claimTime?: string;

  /**
   * 办结时间
   */
  completedAt?: string;

  /**
   * 是否加签任务
   */
  isAddSign: number;

  /**
   * 加签记录 ID（TaktFlowAddSign）
   */
  addSignId?: string;

  /**
   * 多实例序号
   */
  sortOrder: number;

  /**
   * 审批意见
   */
  comment?: string;

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

