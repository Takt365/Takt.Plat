// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：holiday-theme.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { HolidayTheme } from '@/types/human-resource/attendance/holiday-theme';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktHolidayThemes
 */
const HOLIDAY_THEME_API_BASE = 'TaktHolidayThemes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务器当日、指定租户与公司下的假日主题色与问候信息（登录前预览，须 X-Tenant-Code）
 * @param {string} tenantCode 租户编码（与登录页已校验租户一致）
 * @param {string} companyCode 公司编码（由 getLoginPreviewLocale 解析的默认公司）
 * @returns {Promise<HolidayTheme>} 假日主题
 */
export function getHolidayTheme(tenantCode: string, companyCode: string): Promise<HolidayTheme> {
  return request<HolidayTheme>({
    url: `${HOLIDAY_THEME_API_BASE}/theme`,
    method: 'get',
    params: {
      tenantCode,
      companyCode,
    },
    skipTokenRefresh: true,
    skipLoginAuthError: true,
  });
}
