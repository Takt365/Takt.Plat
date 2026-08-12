// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/views/foundation/quartz-task/utils
// 文件名称：quartz-sync-db-fields.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：同步 SQL 立即执行弹窗：源/目标库识别与 ExecuteParams 组装
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** Sap_Data 日链脚本（源库固定，仅选目标库） */
const SAP_DATA_SYNC_SCRIPTS = new Set([
  'quartz/sync_matplt.sql',
  'quartz/sync_mdl.sql',
  'quartz/sync_st.sql',
  'quartz/sync_ec.sql',
  'quartz/sync_mo.sql',
])

/** 仅目标库（无跨库源占位） */
const TARGET_ONLY_SYNC_SCRIPTS = new Set([
  ...SAP_DATA_SYNC_SCRIPTS,
  'quartz/sync_desc.sql',
])

/** 默认暂存源库名（三部分标识；列表中可能无 Tenant_900） */
export const DEFAULT_STAGING_SOURCE_DATABASE = 'zTakt_900_Dev'

/**
 * 规范化 SqlScript 路径为小写正斜杠
 * @param sqlScript 任务 SqlScript
 * @returns {string} 规范化路径
 */
export function normalizeQuartzSqlScriptPath(sqlScript: string | null | undefined): string {
  return String(sqlScript ?? '')
    .trim()
    .replace(/\\/g, '/')
    .toLowerCase()
}

/**
 * 是否为需选库的同步脚本（排除手工 DDL）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 是否同步脚本
 */
export function isQuartzSyncSqlScript(sqlScript: string | null | undefined): boolean {
  const path = normalizeQuartzSqlScriptPath(sqlScript)
  if (!path.startsWith('quartz/sync_') || !path.endsWith('.sql')) {
    return false
  }
  return path !== 'quartz/sync_data_create_tables.sql'
}

/**
 * 是否仅需目标库（Sap_Data 日链 + sync_desc）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 仅目标
 */
export function needsSyncTargetOnlyPicker(sqlScript: string | null | undefined): boolean {
  return TARGET_ONLY_SYNC_SCRIPTS.has(normalizeQuartzSqlScriptPath(sqlScript))
}

/**
 * 是否需源库+目标库（zTakt_900 暂存族）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 源+目标
 */
export function needsSyncSourceTargetPicker(sqlScript: string | null | undefined): boolean {
  return isQuartzSyncSqlScript(sqlScript) && !needsSyncTargetOnlyPicker(sqlScript)
}

/**
 * 从任务 ExecuteParams JSON 解析源/目标库
 * @param executeParams 任务配置或上次参数
 * @returns {{ sourceDatabase?: string, targetDatabase?: string }}
 */
export function parseSyncExecuteDatabaseParams(executeParams: string | null | undefined): {
  sourceDatabase?: string
  targetDatabase?: string
} {
  if (!executeParams?.trim()) {
    return {}
  }
  try {
    const raw = JSON.parse(executeParams.trim()) as Record<string, unknown>
    if (!raw || typeof raw !== 'object') {
      return {}
    }
    const source =
      typeof raw.sourceDatabase === 'string'
        ? raw.sourceDatabase.trim()
        : typeof raw.SourceDatabase === 'string'
          ? raw.SourceDatabase.trim()
          : undefined
    const target =
      typeof raw.targetDatabase === 'string'
        ? raw.targetDatabase.trim()
        : typeof raw.TargetDatabase === 'string'
          ? raw.TargetDatabase.trim()
          : undefined
    return {
      sourceDatabase: source || undefined,
      targetDatabase: target || undefined,
    }
  } catch {
    return {}
  }
}

/**
 * 组装同步任务立即执行参数 JSON
 * @param options 源/目标库
 * @param options.sourceDatabase 源库（可选）
 * @param options.targetDatabase 目标库
 * @returns {string} executeParams JSON
 */
export function buildSyncExecuteParams(options: {
  sourceDatabase?: string
  targetDatabase: string
}): string {
  const targetDatabase = options.targetDatabase.trim()
  const sourceDatabase = options.sourceDatabase?.trim()
  if (sourceDatabase) {
    return JSON.stringify({ sourceDatabase, targetDatabase })
  }
  return JSON.stringify({ targetDatabase })
}
