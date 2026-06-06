// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils
// 文件名称：takt-id.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：实体主键 / 长整型 ID 安全处理（与 07-overflow-vue、05-utils-vue 对齐；禁止对雪花 ID 做 Number 比较）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** JavaScript 安全整数上限（与 Number.MAX_SAFE_INTEGER 一致） */
export const TAKT_MAX_SAFE_INTEGER = 9007199254740991

/**
 * 将 API / 表单值规范为 string 主键（types/ 约定 id: string）。
 * @param value 原始 id（string | number | null | undefined）
 * @returns  trim 后的 string；空值返回 ''
 */
export function normalizeEntityId(value: string | number | null | undefined): string {
  if (value == null) return ''
  if (typeof value === 'string') return value.trim()
  if (typeof value === 'number') {
    if (!Number.isFinite(value)) return ''
    return String(value)
  }
  return String(value).trim()
}

/**
 * 判断数值或纯数字字符串是否超出 JS 安全整数（雪花 ID 须用 string）。
 * @param value 待检测值
 * @returns 超出安全整数范围时为 true
 */
export function isUnsafeNumericId(value: string | number): boolean {
  if (typeof value === 'number') {
    return !Number.isSafeInteger(value)
  }
  const trimmed = value.trim()
  if (!trimmed || !/^\d+$/.test(trimmed)) return false
  if (trimmed.length > 16) return true
  const asNumber = Number(trimmed)
  return !Number.isSafeInteger(asNumber)
}

/**
 * 字典枚举等小整数可转 number；实体主键（apiUrl 场景）强制 string。
 * @param value 原始值
 * @param options.forceString 为 true 时始终返回 string（实体 ID / API 选项）
 * @returns string 或 number
 */
export function coerceSelectValue(
  value: string | number,
  options?: { forceString?: boolean }
): string | number {
  if (options?.forceString) return normalizeEntityId(value)
  if (typeof value === 'number') return value
  const trimmed = value.trim()
  if (!trimmed) return ''
  if (isUnsafeNumericId(trimmed)) return trimmed
  if (/^-?\d+$/.test(trimmed) && Number.isSafeInteger(Number(trimmed))) {
    return Number(trimmed)
  }
  return trimmed
}

/**
 * 比较两个实体 id（一律按 string 比较，避免精度丢失）。
 * @param a 第一个 id
 * @param b 第二个 id
 * @returns 规范化后是否相等
 */
export function entityIdsEqual(
  a: string | number | null | undefined,
  b: string | number | null | undefined
): boolean {
  return normalizeEntityId(a) === normalizeEntityId(b)
}
