// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/constants/logistics
// 文件名称：ec-scope.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施范围字典值（对齐后端 TaktEcScopeConstants / logistics_manufacturing_ec_scope_category）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 实施范围：1=全仕向 2=部管 3=内部 4=技术 */
export const TaktEcScope = {
  /** 全仕向 */
  AllDestination: 1,
  /** 部管 */
  MaterialControl: 2,
  /** 内部 */
  Internal: 3,
  /** 技术 */
  Technical: 4,
} as const

/**
 * 实施范围=部管时，允许按 Z0 填报更新的执行部门 slug（采购/生管/部管/SMT）
 */
export const EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS = new Set([
  'eckoubai',
  'ecseikan',
  'ecbukan',
  'ecsmt',
])

/** 执行内容标准前缀（对齐后端 ExecContentPrefix） */
export const EC_EXEC_CONTENT_PREFIX = '实施范围-'

/** 历史执行内容前缀（读入时规范为 EC_EXEC_CONTENT_PREFIX） */
export const EC_LEGACY_EXEC_CONTENT_PREFIX = '管理区分-'

/**
 * 历史前缀「管理区分-」统一为「实施范围-」（对齐后端 ReplaceLegacyExecContentPrefix）
 * @param content 执行内容
 * @returns {string} 前缀规范后的文案
 */
export function replaceLegacyEcExecContentPrefix(content: unknown): string {
  const value = String(content ?? '').trim()
  if (!value) {
    return ''
  }
  if (value.startsWith(EC_LEGACY_EXEC_CONTENT_PREFIX)) {
    return EC_EXEC_CONTENT_PREFIX + value.slice(EC_LEGACY_EXEC_CONTENT_PREFIX.length)
  }
  return value
}

/**
 * 是否为停产自动执行内容（裸 EOL 或「实施范围-*-EOL」；历史「管理区分-」先规范）
 * @param content 执行内容
 * @returns {boolean} 是否 EOL 类文案
 */
export function isEcEolExecContent(content: unknown): boolean {
  const value = replaceLegacyEcExecContentPrefix(content)
  if (!value) {
    return false
  }
  const upper = value.toUpperCase()
  return upper === 'EOL' || upper.endsWith('-EOL')
}