// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec.d.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设变（ECN）主表实体。FlowInstanceId 存流程实例 Id，由业务方在发起流程后写入；流程引擎不识别本表，BusinessKey/BusinessType 与“设变”的对应由调用方（设变业务模块）约定并实现。联络等文档见附件表 Attachments。
 * 对应前端 TaktEcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Ec
 * @description 对应后端 TaktEcDto
 */
export interface Ec extends CompanyDtoBase {
  /**
   * EcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 设变单号（唯一）
   */
  ecNo: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变主题/标题
   */
  ecTitle: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText: string;

  /**
   * 负责人
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus: number;

  /**
   * 设变明细列表 （子表：TaktEcDetail）
   */
  ecDetails?: EcDetail[];

  /**
   * 设变附件列表（一个设变可对应多个附件） （子表：TaktEcAttachment）
   */
  attachments?: EcAttachment[];

}


/**
 * Ec 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcQuery
 * @description 对应后端 TaktEcQueryDto
 */
export interface EcQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecNo?: string;

  /**
   * 发行日期（范围查询-开始）
   */
  ecIssueDateStart?: string;

  /**
   * 发行日期（范围查询-结束）
   */
  ecIssueDateEnd?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变主题/标题
   */
  ecTitle?: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText?: string;

  /**
   * 负责人
   */
  ecLeader?: string;

  /**
   * 损失金额
   */
  ecLossAmount?: number;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 录入日期（范围查询-开始）
   */
  ecEntryDateStart?: string;

  /**
   * 录入日期（范围查询-结束）
   */
  ecEntryDateEnd?: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus?: number;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Ec DTO
 * 对应前端 EcCreate
 * @description 对应后端 TaktEcCreateDto
 */
export interface EcCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 设变单号（唯一）
   */
  ecNo: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变主题/标题
   */
  ecTitle: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText: string;

  /**
   * 负责人
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus: number;

  /**
   * 设变明细列表（子表，级联保存）
   */
  ecDetails?: EcDetailCreate[];

  /**
   * 设变附件列表（一个设变可对应多个附件）（子表，级联保存）
   */
  attachments?: EcAttachmentCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Ec DTO
 * 继承 TaktEcCreateDto，添加 EcId 字段
 * 对应前端 EcUpdate
 * @description 对应后端 TaktEcUpdateDto
 */
export interface EcUpdate extends EcCreate {
  /**
   * EcID（标识要更新的实体）
   */
  ecId: string;

}


/**
 * Ec 状态更新 DTO
 * 对应前端 EcStatus
 * @description 对应后端 TaktEcStatusDto
 */
export interface EcStatus {
  /**
   * EcID
   */
  ecId: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

}


/**
 * Ec 导入模板行 DTO
 * 对应前端 EcTemplate
 * @description 对应后端 TaktEcTemplateDto
 */
export interface EcTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecNo?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变主题/标题
   */
  ecTitle?: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText?: string;

  /**
   * 负责人
   */
  ecLeader?: string;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction?: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Ec 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcImport
 * @description 对应后端 TaktEcImportDto
 */
export interface EcImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecNo?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变主题/标题
   */
  ecTitle?: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText?: string;

  /**
   * 负责人
   */
  ecLeader?: string;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction?: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Ec 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcExport
 * @description 对应后端 TaktEcExportDto
 */
export interface EcExport {
  /**
   * EcID
   */
  ecId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 设变单号（唯一）
   */
  ecNo: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变主题/标题
   */
  ecTitle: string;

  /**
   * 设变详情/详细说明
   */
  ecDetailText: string;

  /**
   * 负责人
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 区分/类别 1:全仕向，2：部管，3：内部，4：技术
   */
  ecDistinction: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（0=草稿 1=审批中 2=已通过 3=已驳回 4=已撤回）
   */
  ecStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

