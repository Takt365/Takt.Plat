// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：routing.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 工艺路线主表实体
 * 对应前端 TaktRoutingDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Routing
 * @description 对应后端 TaktRoutingDto
 */
export interface Routing extends ApprovalDtoBase {
  /**
   * RoutingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  routingId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工作中心
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系） （子表：TaktRoutingItem）
   */
  items?: RoutingItem[];

  /**
   * 变更日志列表（主子表关系） （子表：TaktRoutingChangeLog）
   */
  changeLogs?: RoutingChangeLog[];

}


/**
 * Routing 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoutingQuery
 * @description 对应后端 TaktRoutingQueryDto
 */
export interface RoutingQuery extends TaktPagedQuery {
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
   * 工作中心
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 审批状态（TaktApprovalStatus）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

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
 * 创建Routing DTO
 * 对应前端 RoutingCreate
 * @description 对应后端 TaktRoutingCreateDto
 */
export interface RoutingCreate {
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
   * 工作中心
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系）（子表，级联保存）
   */
  items?: RoutingItemCreate[];

  /**
   * 变更日志列表（主子表关系）（子表，级联保存）
   */
  changeLogs?: RoutingChangeLogCreate[];

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
 * 更新Routing DTO
 * 继承 TaktRoutingCreateDto，添加 RoutingId 字段
 * 对应前端 RoutingUpdate
 * @description 对应后端 TaktRoutingUpdateDto
 */
export interface RoutingUpdate extends RoutingCreate {
  /**
   * RoutingID（标识要更新的实体）
   */
  routingId: string;

}


/**
 * Routing 状态更新 DTO
 * 对应前端 RoutingStatus
 * @description 对应后端 TaktRoutingStatusDto
 */
export interface RoutingStatus {
  /**
   * RoutingID
   */
  routingId: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus: number;

}


/**
 * Routing 导入模板行 DTO
 * 对应前端 RoutingTemplate
 * @description 对应后端 TaktRoutingTemplateDto
 */
export interface RoutingTemplate {
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
   * 工作中心
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus?: number;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

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
 * Routing 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 RoutingImport
 * @description 对应后端 TaktRoutingImportDto
 */
export interface RoutingImport {
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
   * 工作中心
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus?: number;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

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
 * Routing 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoutingExport
 * @description 对应后端 TaktRoutingExportDto
 */
export interface RoutingExport {
  /**
   * RoutingID
   */
  routingId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工作中心
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（1=生成的，2=对订单下达，3=对成本核算下达，4=下达的（通用））
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

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

