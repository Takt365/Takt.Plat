// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：quartz-task.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Quartz 定时任务实体
 * 对应前端 TaktQuartzTaskDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QuartzTask
 * @description 对应后端 TaktQuartzTaskDto
 */
export interface QuartzTask extends CompanyDtoBase {
  /**
   * QuartzTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  quartzTaskId: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组
   */
  jobGroup: string;

  /**
   * Cron 表达式
   */
  cronExpression: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent: number;

  /**
   * Misfire 策略
   */
  misfirePolicy: number;

  /**
   * 上次执行时间
   */
  lastRunAt?: string;

  /**
   * 下次执行时间
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  description?: string;

}


/**
 * QuartzTask 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QuartzTaskQuery
 * @description 对应后端 TaktQuartzTaskQueryDto
 */
export interface QuartzTaskQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组
   */
  jobGroup?: string;

  /**
   * Cron 表达式
   */
  cronExpression?: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType?: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent?: number;

  /**
   * Misfire 策略
   */
  misfirePolicy?: number;

  /**
   * 上次执行时间（范围查询-开始）
   */
  lastRunAtStart?: string;

  /**
   * 上次执行时间（范围查询-结束）
   */
  lastRunAtEnd?: string;

  /**
   * 下次执行时间（范围查询-开始）
   */
  nextRunAtStart?: string;

  /**
   * 下次执行时间（范围查询-结束）
   */
  nextRunAtEnd?: string;

  /**
   * 任务描述
   */
  description?: string;

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
 * 创建QuartzTask DTO
 * 对应前端 QuartzTaskCreate
 * @description 对应后端 TaktQuartzTaskCreateDto
 */
export interface QuartzTaskCreate {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组
   */
  jobGroup: string;

  /**
   * Cron 表达式
   */
  cronExpression: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent: number;

  /**
   * Misfire 策略
   */
  misfirePolicy: number;

  /**
   * 上次执行时间
   */
  lastRunAt?: string;

  /**
   * 下次执行时间
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  description?: string;

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
 * 更新QuartzTask DTO
 * 继承 TaktQuartzTaskCreateDto，添加 QuartzTaskId 字段
 * 对应前端 QuartzTaskUpdate
 * @description 对应后端 TaktQuartzTaskUpdateDto
 */
export interface QuartzTaskUpdate extends QuartzTaskCreate {
  /**
   * QuartzTaskID（标识要更新的实体）
   */
  quartzTaskId: string;

}


/**
 * QuartzTask 状态更新 DTO
 * 对应前端 QuartzTaskStatus
 * @description 对应后端 TaktQuartzTaskStatusDto
 */
export interface QuartzTaskStatus {
  /**
   * QuartzTaskID
   */
  quartzTaskId: string;

  /**
   * 任务状态
   */
  taskStatus: number;

}


/**
 * QuartzTask 导入模板行 DTO
 * 对应前端 QuartzTaskTemplate
 * @description 对应后端 TaktQuartzTaskTemplateDto
 */
export interface QuartzTaskTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组
   */
  jobGroup?: string;

  /**
   * Cron 表达式
   */
  cronExpression?: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType?: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent?: number;

  /**
   * Misfire 策略
   */
  misfirePolicy?: number;

  /**
   * 任务描述
   */
  description?: string;

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
 * QuartzTask 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QuartzTaskImport
 * @description 对应后端 TaktQuartzTaskImportDto
 */
export interface QuartzTaskImport {
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
   * 任务编码（租户+公司内唯一）
   */
  taskCode?: string;

  /**
   * 任务名称
   */
  taskName?: string;

  /**
   * Quartz Job 名称
   */
  jobName?: string;

  /**
   * Quartz Job 分组
   */
  jobGroup?: string;

  /**
   * Cron 表达式
   */
  cronExpression?: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType?: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus?: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent?: number;

  /**
   * Misfire 策略
   */
  misfirePolicy?: number;

  /**
   * 任务描述
   */
  description?: string;

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
 * QuartzTask 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QuartzTaskExport
 * @description 对应后端 TaktQuartzTaskExportDto
 */
export interface QuartzTaskExport {
  /**
   * QuartzTaskID
   */
  quartzTaskId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 任务编码（租户+公司内唯一）
   */
  taskCode: string;

  /**
   * 任务名称
   */
  taskName: string;

  /**
   * Quartz Job 名称
   */
  jobName: string;

  /**
   * Quartz Job 分组
   */
  jobGroup: string;

  /**
   * Cron 表达式
   */
  cronExpression: string;

  /**
   * 任务处理器类型（DI 注册键或完整类型名）
   */
  jobType: string;

  /**
   * 任务参数 JSON
   */
  jobParams?: string;

  /**
   * 任务状态
   */
  taskStatus: number;

  /**
   * 是否允许并发执行（0=禁止，1=允许）
   */
  concurrent: number;

  /**
   * Misfire 策略
   */
  misfirePolicy: number;

  /**
   * 上次执行时间
   */
  lastRunAt?: string;

  /**
   * 下次执行时间
   */
  nextRunAt?: string;

  /**
   * 任务描述
   */
  description?: string;

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

