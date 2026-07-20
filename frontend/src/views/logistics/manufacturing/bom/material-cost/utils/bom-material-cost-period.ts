// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bom-material-cost-period.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：核算单月默认值与 costingDateStart/End 纯转换（浏览页仅单月）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 默认核算单月（当月 yyyy-MM）
 * @returns {string} yyyy-MM
 */
export function buildDefaultCostingMonth(): string {
  const now = new Date()
  return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`
}

/**
 * 默认核算年月区间（含当月共 monthCount 个月；供分析/推移多月页使用）
 * @param monthCount 月数（≥1）
 * @returns {[string, string]} [起 yyyy-MM, 止 yyyy-MM]
 */
export function buildDefaultCostingPeriodRange(monthCount = 3): [string, string] {
  const count = Math.max(1, Math.floor(monthCount))
  const end = new Date()
  const start = new Date(end.getFullYear(), end.getMonth() - (count - 1), 1)
  const fmt = (d: Date) => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`
  return [fmt(start), fmt(end)]
}

/**
 * 核算单月转为核算日期起止（yyyy-MM → 当月首末日）
 * @param month 核算月 yyyy-MM
 * @returns costingDateStart / costingDateEnd
 */
export function costingMonthToDateQuery(
  month: string | null | undefined,
): { costingDateStart?: string; costingDateEnd?: string } {
  const value = month?.trim()
  if (!value) {
    return {}
  }
  return periodRangeToCostingDateQuery([value, value])
}

/**
 * 年月区间转为核算日期起止（yyyy-MM → yyyy-MM-dd）
 * @param range 年月区间 [起, 止]
 * @returns costingDateStart / costingDateEnd
 */
export function periodRangeToCostingDateQuery(
  range: [string, string] | null | undefined,
): { costingDateStart?: string; costingDateEnd?: string } {
  if (!range?.[0]) {
    return {}
  }
  const costingDateStart = `${range[0]}-01`
  if (!range[1]) {
    return { costingDateStart }
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { costingDateStart }
  }
  const lastDay = new Date(year, month, 0).getDate()
  const costingDateEnd = `${range[1]}-${String(lastDay).padStart(2, '0')}`
  return { costingDateStart, costingDateEnd }
}
