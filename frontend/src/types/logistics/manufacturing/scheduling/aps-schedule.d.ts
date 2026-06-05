// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/scheduling
// 文件名称：aps-schedule.d.ts
// 创建时间：2026-06-05
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
 * APS排程主表（高级计划与排程）
 * 对应前端 TaktApsScheduleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsSchedule
 * @description 对应后端 TaktApsScheduleDto
 */
export interface ApsSchedule extends CompanyDtoBase {
  /**
   * ApsScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  apsScheduleId: string;

  /**
   * 工厂编码（不可空）
   */
  plantCode: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode: string;

  /**
   * 排程名称
   */
  scheduleName: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType: number;

  /**
   * 计划日期
   */
  planDate: string;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus: number;

  /**
   * 计划员ID
   */
  plannerId?: string;

  /**
   * 计划员姓名
   */
  plannerName?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

  /**
   * 发布人ID
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

  /**
   * 排程明细列表（主子表关系） （子表：TaktApsScheduleItem）
   */
  items?: ApsScheduleItem[];

  /**
   * 变更日志列表（主子表关系） （子表：TaktApsScheduleChangeLog）
   */
  changeLogs?: ApsScheduleChangeLog[];

}


/**
 * ApsSchedule 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ApsScheduleQuery
 * @description 对应后端 TaktApsScheduleQueryDto
 */
export interface ApsScheduleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂编码（不可空）
   */
  plantCode?: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode?: string;

  /**
   * 排程名称
   */
  scheduleName?: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType?: number;

  /**
   * 计划日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 计划开始时间（范围查询-开始）
   */
  planStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  planStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  planEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  planEndTimeEnd?: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle?: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy?: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm?: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective?: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus?: number;

  /**
   * 计划员ID
   */
  plannerId?: string;

  /**
   * 计划员姓名
   */
  plannerName?: string;

  /**
   * 发布时间（范围查询-开始）
   */
  publishTimeStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  publishTimeEnd?: string;

  /**
   * 发布人ID
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

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
 * 创建ApsSchedule DTO
 * 对应前端 ApsScheduleCreate
 * @description 对应后端 TaktApsScheduleCreateDto
 */
export interface ApsScheduleCreate {
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
   * 工厂编码（不可空）
   */
  plantCode: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode: string;

  /**
   * 排程名称
   */
  scheduleName: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType: number;

  /**
   * 计划日期
   */
  planDate: string;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus: number;

  /**
   * 计划员ID
   */
  plannerId?: string;

  /**
   * 计划员姓名
   */
  plannerName?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

  /**
   * 发布人ID
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

  /**
   * 排程明细列表（主子表关系）（子表，级联保存）
   */
  items?: ApsScheduleItemCreate[];

  /**
   * 变更日志列表（主子表关系）（子表，级联保存）
   */
  changeLogs?: ApsScheduleChangeLogCreate[];

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
 * 更新ApsSchedule DTO
 * 继承 TaktApsScheduleCreateDto，添加 ApsScheduleId 字段
 * 对应前端 ApsScheduleUpdate
 * @description 对应后端 TaktApsScheduleUpdateDto
 */
export interface ApsScheduleUpdate extends ApsScheduleCreate {
  /**
   * ApsScheduleID（标识要更新的实体）
   */
  apsScheduleId: string;

}


/**
 * ApsSchedule 状态更新 DTO
 * 对应前端 ApsScheduleStatus
 * @description 对应后端 TaktApsScheduleStatusDto
 */
export interface ApsScheduleStatus {
  /**
   * ApsScheduleID
   */
  apsScheduleId: string;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus: number;

}


/**
 * ApsSchedule 导入模板行 DTO
 * 对应前端 ApsScheduleTemplate
 * @description 对应后端 TaktApsScheduleTemplateDto
 */
export interface ApsScheduleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂编码（不可空）
   */
  plantCode?: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode?: string;

  /**
   * 排程名称
   */
  scheduleName?: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType?: number;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle?: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy?: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm?: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective?: number;

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
 * ApsSchedule 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ApsScheduleImport
 * @description 对应后端 TaktApsScheduleImportDto
 */
export interface ApsScheduleImport {
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
   * 工厂编码（不可空）
   */
  plantCode?: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode?: string;

  /**
   * 排程名称
   */
  scheduleName?: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType?: number;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle?: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy?: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm?: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective?: number;

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
 * ApsSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsScheduleExport
 * @description 对应后端 TaktApsScheduleExportDto
 */
export interface ApsScheduleExport {
  /**
   * ApsScheduleID
   */
  apsScheduleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂编码（不可空）
   */
  plantCode: string;

  /**
   * 排程编码（唯一索引）
   */
  scheduleCode: string;

  /**
   * 排程名称
   */
  scheduleName: string;

  /**
   * 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
   */
  scheduleType: number;

  /**
   * 计划日期
   */
  planDate: string;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 计划周期（0=日计划，1=周计划，2=月计划）
   */
  planCycle: number;

  /**
   * 车间编码
   */
  workshopCode?: string;

  /**
   * 车间名称
   */
  workshopName?: string;

  /**
   * 生产线编码
   */
  productionLineCode?: string;

  /**
   * 生产线名称
   */
  productionLineName?: string;

  /**
   * 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
   */
  scheduleStrategy: number;

  /**
   * 排程算法（0=正向排程，1=逆向排程，2=双向排程）
   */
  scheduleAlgorithm: number;

  /**
   * 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
   */
  optimizationObjective: number;

  /**
   * 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
   */
  scheduleStatus: number;

  /**
   * 计划员ID
   */
  plannerId?: string;

  /**
   * 计划员姓名
   */
  plannerName?: string;

  /**
   * 发布时间
   */
  publishTime?: string;

  /**
   * 发布人ID
   */
  publishUserId?: string;

  /**
   * 发布人姓名
   */
  publishUserName?: string;

  /**
   * 排程说明
   */
  scheduleDescription?: string;

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

