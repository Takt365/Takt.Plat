// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：data-dict-all.d.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：全部字典数据类型（类型名去 Takt 前缀与末尾 Dto，如 TaktDataDictAllDto → DataDictAll）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktSelectOption } from '@/types/common';

/**
 * 租户下全部字典数据响应
 * @description 对应后端 TaktDataDictAllDto
 */
export interface DataDictAll {
  /**
   * 字典项列表（含 dictTypeCode 供前端分组）
   */
  items: TaktSelectOption[];
}
