// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：holiday.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 假日实体 假日条目，用于考勤日历、排班与薪资计算；字典 hr_holiday_type、hr_holiday_is_working_day 与字段取值一致 公司级实体：按 TenantCode + CompanyCode 隔离；同一公司内以开始日期+结束日期+假日类型唯一
 * 对应前端 TaktHolidayDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Holiday
 * @description 对应后端 TaktHolidayDto
 */
export interface Holiday extends CompanyDtoBase {
  /**
   * HolidayID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  holidayId: string;

  /**
   * 假日名称
   */
  holidayName: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType: number;

  /**
   * 假日开始日期
   */
  startDate: string;

  /**
   * 假日结束日期
   */
  endDate: string;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme: string;

}


/**
 * Holiday 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 HolidayQuery
 * @description 对应后端 TaktHolidayQueryDto
 */
export interface HolidayQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 假日名称
   */
  holidayName?: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType?: number;

  /**
   * 假日开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 假日开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 假日结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 假日结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay?: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting?: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote?: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme?: string;

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
 * 创建Holiday DTO
 * 对应前端 HolidayCreate
 * @description 对应后端 TaktHolidayCreateDto
 */
export interface HolidayCreate {
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
   * 假日名称
   */
  holidayName: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType: number;

  /**
   * 假日开始日期
   */
  startDate: string;

  /**
   * 假日结束日期
   */
  endDate: string;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme: string;

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
 * 更新Holiday DTO
 * 继承 TaktHolidayCreateDto，添加 HolidayId 字段
 * 对应前端 HolidayUpdate
 * @description 对应后端 TaktHolidayUpdateDto
 */
export interface HolidayUpdate extends HolidayCreate {
  /**
   * HolidayID（标识要更新的实体）
   */
  holidayId: string;

}


/**
 * Holiday 导入模板行 DTO
 * 对应前端 HolidayTemplate
 * @description 对应后端 TaktHolidayTemplateDto
 */
export interface HolidayTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 假日名称
   */
  holidayName?: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType?: number;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay?: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting?: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote?: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme?: string;

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
 * Holiday 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 HolidayImport
 * @description 对应后端 TaktHolidayImportDto
 */
export interface HolidayImport {
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
   * 假日名称
   */
  holidayName?: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType?: number;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay?: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting?: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote?: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme?: string;

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
 * Holiday 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 HolidayExport
 * @description 对应后端 TaktHolidayExportDto
 */
export interface HolidayExport {
  /**
   * HolidayID
   */
  holidayId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 假日名称
   */
  holidayName: string;

  /**
   * 假日类型（字典 hr_holiday_type）
   */
  holidayType: number;

  /**
   * 假日开始日期
   */
  startDate: string;

  /**
   * 假日结束日期
   */
  endDate: string;

  /**
   * 是否工作日（字典 hr_holiday_is_working_day）
   */
  isWorkingDay: number;

  /**
   * 假日问候语（简短，用于界面问候展示）
   */
  holidayGreeting: string;

  /**
   * 假日引用/诗句（用于引用区展示）
   */
  holidayQuote: string;

  /**
   * 假日主题（对应前端主题色 key，用于日历等非工作日展示）
   */
  holidayTheme: string;

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

