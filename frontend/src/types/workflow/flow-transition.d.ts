// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-transition.d.ts
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
 * 流程流转历史实体
 * 对应前端 TaktFlowTransitionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowTransition
 * @description 对应后端 TaktFlowTransitionDto
 */
export interface FlowTransition extends CompanyDtoBase {
  /**
   * FlowTransitionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowTransitionId: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 流程实例 名称（填充字段）
   */
  instanceName?: string;

  /**
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  transitionTime: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

  /**
   * 动作类型
   */
  actionType: number;

  /**
   * 所属流程实例 （主表：TaktFlowInstance）
   */
  instance?: FlowInstance;

}


/**
 * FlowTransition 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowTransitionQuery
 * @description 对应后端 TaktFlowTransitionQueryDto
 */
export interface FlowTransitionQuery extends TaktPagedQuery {
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
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId?: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 开始时间（范围查询-开始）
   */
  startTimeStart?: string;

  /**
   * 开始时间（范围查询-结束）
   */
  startTimeEnd?: string;

  /**
   * 结束时间（范围查询-开始）
   */
  transitionTimeStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  transitionTimeEnd?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

  /**
   * 动作类型
   */
  actionType?: number;

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
 * 创建FlowTransition DTO
 * 对应前端 FlowTransitionCreate
 * @description 对应后端 TaktFlowTransitionCreateDto
 */
export interface FlowTransitionCreate {
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
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  transitionTime: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

  /**
   * 动作类型
   */
  actionType: number;

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
 * 更新FlowTransition DTO
 * 继承 TaktFlowTransitionCreateDto，添加 FlowTransitionId 字段
 * 对应前端 FlowTransitionUpdate
 * @description 对应后端 TaktFlowTransitionUpdateDto
 */
export interface FlowTransitionUpdate extends FlowTransitionCreate {
  /**
   * FlowTransitionID（标识要更新的实体）
   */
  flowTransitionId: string;

}


/**
 * FlowTransition 导入模板行 DTO
 * 对应前端 FlowTransitionTemplate
 * @description 对应后端 TaktFlowTransitionTemplateDto
 */
export interface FlowTransitionTemplate {
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
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId?: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

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
 * FlowTransition 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowTransitionImport
 * @description 对应后端 TaktFlowTransitionImportDto
 */
export interface FlowTransitionImport {
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
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId?: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

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
 * FlowTransition 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowTransitionExport
 * @description 对应后端 TaktFlowTransitionExportDto
 */
export interface FlowTransitionExport {
  /**
   * FlowTransitionID
   */
  flowTransitionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 节点 ID
   */
  activityId?: string;

  /**
   * 节点名称
   */
  activityName?: string;

  /**
   * 节点类型（如 userTask、start、end）
   */
  activityType?: string;

  /**
   * 源节点 ID
   */
  fromNodeId?: string;

  /**
   * 源节点名称
   */
  fromNodeName?: string;

  /**
   * 目标节点 ID
   */
  toNodeId?: string;

  /**
   * 目标节点名称
   */
  toNodeName?: string;

  /**
   * 操作人 ID
   */
  transitionUserId: string;

  /**
   * 操作人姓名
   */
  transitionUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  transitionTime: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 操作意见
   */
  transitionComment?: string;

  /**
   * 动作类型
   */
  actionType: number;

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

