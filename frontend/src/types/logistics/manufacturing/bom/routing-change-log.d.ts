// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：routing-change-log.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 工艺路线变更日志（记录工艺路线的变更历史）
 * 对应前端 TaktRoutingChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 RoutingChangeLog
 * @description 对应后端 TaktRoutingChangeLogDto
 */
export interface RoutingChangeLog extends CompanyDtoBase {
  /**
   * RoutingChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  routingChangeLogId: string;

  /**
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 工艺路线主表名称（填充字段）
   */
  routingName?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 工艺路线主表（主表） （主表：TaktRouting）
   */
  routing?: Routing;

}


/**
 * RoutingChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoutingChangeLogQuery
 * @description 对应后端 TaktRoutingChangeLogQueryDto
 */
export interface RoutingChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）
   */
  changeType?: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

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
 * 创建RoutingChangeLog DTO
 * 对应前端 RoutingChangeLogCreate
 * @description 对应后端 TaktRoutingChangeLogCreateDto
 */
export interface RoutingChangeLogCreate {
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
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

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
 * 更新RoutingChangeLog DTO
 * 继承 TaktRoutingChangeLogCreateDto，添加 RoutingChangeLogId 字段
 * 对应前端 RoutingChangeLogUpdate
 * @description 对应后端 TaktRoutingChangeLogUpdateDto
 */
export interface RoutingChangeLogUpdate extends RoutingChangeLogCreate {
  /**
   * RoutingChangeLogID（标识要更新的实体）
   */
  routingChangeLogId: string;

}


/**
 * RoutingChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoutingChangeLogExport
 * @description 对应后端 TaktRoutingChangeLogExportDto
 */
export interface RoutingChangeLogExport {
  /**
   * RoutingChangeLogID
   */
  routingChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  routingId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=版本升级）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

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

