// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：holiday.d.ts
// 创建时间：2026-06-23
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
 * 假日实体 假日条目，用于考勤日历、排班与薪资计算；字典 hr_holiday_category、hr_holiday_working_day_type 与字段取值一致 公司级实体：按 TenantCode + CompanyCode 隔离；同一公司内以开始日期+结束日期+假日类型唯一
 * 对应前端 TaktHolidayDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Holiday
 * @description 对应后端 TaktHolidayDto
 */
export interface Holiday extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 假日名称
   */
  holidayName?: string;

  /**
   * 假日类型（字典 hr_holiday_category；0=法定 1=调休 2=公司）
   */
  holidayType?: number;

  /**
   * 假日开始日期
   */
  startDate?: string;

  /**
   * 假日结束日期
   */
  endDate?: string;

  /**
   * 是否工作日（字典 hr_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
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
  extField?: string;

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
   * 假日类型（字典 hr_holiday_category；0=法定 1=调休 2=公司）
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
   * 是否工作日（字典 hr_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
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

/**
 * 服务器当日、用户默认登录公司下的假日主题响应 DTO
 * @description 对应后端 TaktHolidayThemeDto；业务字段与 TaktHoliday 实体一致，并含 isHolidayToday
 */
export interface HolidayTheme {
  /**
   * 公司代码（来自 TaktHoliday.CompanyCode）
   */
  companyCode: string;

  /**
   * 假日名称
   */
  holidayName: string;

  /**
   * 假日类型（字典 hr_holiday_category；0=法定 1=调休 2=公司）
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
   * 是否工作日（字典 hr_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
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
   * 服务器当日是否处于假日区间且为非工作日（无匹配记录时为 false）
   */
  isHolidayToday: boolean;
}
