// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/foundation
// 文件名称：data-dict-all.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：全部字典数据 API（独立模块，非 generate-from-backend 生成）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktSelectOption } from '@/types/common';
import type { DataDictAll } from '@/types/foundation/data-dict-all';

/**
 * API 路径前缀（相对 request baseURL，对应后端 TaktDataDictAllsController）
 */
const DATA_DICT_ALL_API_BASE = 'TaktDataDictAlls';

/**
 * 获取当前租户下全部字典数据（扁平列表，含 dictTypeCode）
 * @returns {Promise<TaktSelectOption[]>} 字典项列表
 */
export function getDictDataAll(): Promise<TaktSelectOption[]> {
  return request<DataDictAll>({
    url: DATA_DICT_ALL_API_BASE,
    method: 'get',
  }).then((dto) => dto.items ?? []);
}
