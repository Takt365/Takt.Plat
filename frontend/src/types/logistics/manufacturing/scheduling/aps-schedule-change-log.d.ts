// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/scheduling
// 文件名称：aps-schedule-change-log.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/scheduling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * APS排程变更日志（记录排程的变更历史）
 * 对应前端 TaktApsScheduleChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsScheduleChangeLog
 * @description 对应后端 TaktApsScheduleChangeLogDto
 */
export interface ApsScheduleChangeLog extends CompanyDtoBase {
  /**
   * ApsScheduleChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  apsScheduleChangeLogId: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * APS排程名称（填充字段）
   */
  apsScheduleName?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=重新排程）
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
   * APS排程主表（主表） （主表：TaktApsSchedule）
   */
  schedule?: ApsSchedule;

}


/**
 * ApsScheduleChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ApsScheduleChangeLogQuery
 * @description 对应后端 TaktApsScheduleChangeLogQueryDto
 */
export interface ApsScheduleChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=重新排程）
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
 * 创建ApsScheduleChangeLog DTO
 * 对应前端 ApsScheduleChangeLogCreate
 * @description 对应后端 TaktApsScheduleChangeLogCreateDto
 */
export interface ApsScheduleChangeLogCreate {
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
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=重新排程）
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
 * 更新ApsScheduleChangeLog DTO
 * 继承 TaktApsScheduleChangeLogCreateDto，添加 ApsScheduleChangeLogId 字段
 * 对应前端 ApsScheduleChangeLogUpdate
 * @description 对应后端 TaktApsScheduleChangeLogUpdateDto
 */
export interface ApsScheduleChangeLogUpdate extends ApsScheduleChangeLogCreate {
  /**
   * ApsScheduleChangeLogID（标识要更新的实体）
   */
  apsScheduleChangeLogId: string;

}


/**
 * ApsScheduleChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsScheduleChangeLogExport
 * @description 对应后端 TaktApsScheduleChangeLogExportDto
 */
export interface ApsScheduleChangeLogExport {
  /**
   * ApsScheduleChangeLogID
   */
  apsScheduleChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更，4=重新排程）
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

