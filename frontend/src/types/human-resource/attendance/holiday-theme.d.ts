// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：holiday-theme.d.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：假日主题公开 API 类型（与后端 TaktHolidayThemeDto 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 服务器当日、用户默认登录公司下的假日主题响应 DTO
 * 对应前端 HolidayTheme
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
   * 服务器当日是否处于假日区间且为非工作日（无匹配记录时为 false）
   */
  isHolidayToday: boolean;
}
