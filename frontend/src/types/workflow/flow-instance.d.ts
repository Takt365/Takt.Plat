// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-instance.d.ts
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
 * 流程实例实体
 * 对应前端 TaktFlowInstanceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowInstance
 * @description 对应后端 TaktFlowInstanceDto
 */
export interface FlowInstance extends CompanyDtoBase {
  /**
   * FlowInstanceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowInstanceId: string;

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId: string;

  /**
   * 流程定义 名称（填充字段）
   */
  processDefinitionName?: string;

  /**
   * 流程键（冗余）
   */
  processKey: string;

  /**
   * 流程名称（冗余）
   */
  processName: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 业务主键（关联业务单据 Id 等）
   */
  businessKey?: string;

  /**
   * 业务类型（由业务模块约定，用于回写）
   */
  businessType?: string;

  /**
   * 父流程实例 ID（子流程场景）
   */
  superInstanceId?: string;

  /**
   * 父流程实例 名称（填充字段）
   */
  superInstanceName?: string;

  /**
   * 终止原因
   */
  deleteReason?: string;

  /**
   * 表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref="TaktFlowVariable"/>）
   */
  frmData?: string;

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
   * 流程设计快照（启动时复制，避免定义变更影响在途实例）
   */
  processContentSnapshot?: string;

  /**
   * 流程定义 （主表：TaktFlowScheme）
   */
  processDefinition?: FlowScheme;

  /**
   * 待办任务 （子表：TaktFlowTask）
   */
  tasks?: FlowTask[];

  /**
   * 流转历史 （子表：TaktFlowTransition）
   */
  historicActivities?: FlowTransition[];

  /**
   * 流程变量 （子表：TaktFlowVariable）
   */
  variables?: FlowVariable[];

  /**
   * 加签记录 （子表：TaktFlowAddSign）
   */
  addSigns?: FlowAddSign[];

}


/**
 * FlowInstance 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowInstanceQuery
 * @description 对应后端 TaktFlowInstanceQueryDto
 */
export interface FlowInstanceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode?: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId?: string;

  /**
   * 流程键（冗余）
   */
  processKey?: string;

  /**
   * 流程名称（冗余）
   */
  processName?: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion?: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus?: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

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
  endTimeStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  endTimeEnd?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 业务主键（关联业务单据 Id 等）
   */
  businessKey?: string;

  /**
   * 业务类型（由业务模块约定，用于回写）
   */
  businessType?: string;

  /**
   * 父流程实例 ID（子流程场景）
   */
  superInstanceId?: string;

  /**
   * 终止原因
   */
  deleteReason?: string;

  /**
   * 表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref="TaktFlowVariable"/>）
   */
  frmData?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 流程设计快照（启动时复制，避免定义变更影响在途实例）
   */
  processContentSnapshot?: string;

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
 * 创建FlowInstance DTO
 * 对应前端 FlowInstanceCreate
 * @description 对应后端 TaktFlowInstanceCreateDto
 */
export interface FlowInstanceCreate {
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
   * 实例编码（对外业务单号）
   */
  instanceCode: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId: string;

  /**
   * 流程键（冗余）
   */
  processKey: string;

  /**
   * 流程名称（冗余）
   */
  processName: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 业务主键（关联业务单据 Id 等）
   */
  businessKey?: string;

  /**
   * 业务类型（由业务模块约定，用于回写）
   */
  businessType?: string;

  /**
   * 父流程实例 ID（子流程场景）
   */
  superInstanceId?: string;

  /**
   * 终止原因
   */
  deleteReason?: string;

  /**
   * 表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref="TaktFlowVariable"/>）
   */
  frmData?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 流程设计快照（启动时复制，避免定义变更影响在途实例）
   */
  processContentSnapshot?: string;

  /**
   * 待办任务（子表，级联保存）
   */
  tasks?: FlowTaskCreate[];

  /**
   * 流转历史（子表，级联保存）
   */
  historicActivities?: FlowTransitionCreate[];

  /**
   * 流程变量（子表，级联保存）
   */
  variables?: FlowVariableCreate[];

  /**
   * 加签记录（子表，级联保存）
   */
  addSigns?: FlowAddSignCreate[];

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
 * 更新FlowInstance DTO
 * 继承 TaktFlowInstanceCreateDto，添加 FlowInstanceId 字段
 * 对应前端 FlowInstanceUpdate
 * @description 对应后端 TaktFlowInstanceUpdateDto
 */
export interface FlowInstanceUpdate extends FlowInstanceCreate {
  /**
   * FlowInstanceID（标识要更新的实体）
   */
  flowInstanceId: string;

}


/**
 * FlowInstance 状态更新 DTO
 * 对应前端 FlowInstanceStatus
 * @description 对应后端 TaktFlowInstanceStatusDto
 */
export interface FlowInstanceStatus {
  /**
   * FlowInstanceID
   */
  flowInstanceId: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

}


/**
 * FlowInstance 导入模板行 DTO
 * 对应前端 FlowInstanceTemplate
 * @description 对应后端 TaktFlowInstanceTemplateDto
 */
export interface FlowInstanceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode?: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId?: string;

  /**
   * 流程键（冗余）
   */
  processKey?: string;

  /**
   * 流程名称（冗余）
   */
  processName?: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion?: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus?: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

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
 * FlowInstance 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowInstanceImport
 * @description 对应后端 TaktFlowInstanceImportDto
 */
export interface FlowInstanceImport {
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
   * 实例编码（对外业务单号）
   */
  instanceCode?: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId?: string;

  /**
   * 流程键（冗余）
   */
  processKey?: string;

  /**
   * 流程名称（冗余）
   */
  processName?: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion?: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus?: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

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
 * FlowInstance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowInstanceExport
 * @description 对应后端 TaktFlowInstanceExportDto
 */
export interface FlowInstanceExport {
  /**
   * FlowInstanceID
   */
  flowInstanceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode: string;

  /**
   * 流程定义 ID（<see cref="TaktFlowScheme"/> Id）
   */
  processDefinitionId: string;

  /**
   * 流程键（冗余）
   */
  processKey: string;

  /**
   * 流程名称（冗余）
   */
  processName: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 业务主键（关联业务单据 Id 等）
   */
  businessKey?: string;

  /**
   * 业务类型（由业务模块约定，用于回写）
   */
  businessType?: string;

  /**
   * 父流程实例 ID（子流程场景）
   */
  superInstanceId?: string;

  /**
   * 终止原因
   */
  deleteReason?: string;

  /**
   * 表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref="TaktFlowVariable"/>）
   */
  frmData?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 流程设计快照（启动时复制，避免定义变更影响在途实例）
   */
  processContentSnapshot?: string;

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

