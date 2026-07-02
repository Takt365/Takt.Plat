// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/components/business/takt-cron-editor
// 文件名称：quartz-cron-core.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 6 字段 Cron 构建/反解析与最近运行时间预览（参照博客园 BlackCatFish 自定义 cron 组件）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 单段 Cron 配置（秒/分/时/天/周/月/年 Tab 内 radio 状态） */
export interface QuartzCronFieldState {
  cronEvery: string
  incrementStart: number | string
  incrementIncrement: number | string
  rangeStart: number | string
  rangeEnd: number | string
  specificSpecific: number[]
  cronLastSpecificDomDay?: number
  cronDaysBeforeEomMinus?: number
  cronDaysNearestWeekday?: number
  cronNthDayDay?: number
  cronNthDayNth?: number | string
}

/** Cron 弹窗完整编辑状态 */
export interface QuartzCronEditorState {
  second: QuartzCronFieldState
  minute: QuartzCronFieldState
  hour: QuartzCronFieldState
  day: QuartzCronFieldState
  week: QuartzCronFieldState
  month: QuartzCronFieldState
  year: QuartzCronFieldState
}

/** 解析后的 Quartz Cron 六段文本 */
export interface QuartzCronSegments {
  second: string
  minute: string
  hour: string
  day: string
  month: string
  week: string
}

const currentYear = new Date().getFullYear()

/**
 * 创建默认 Cron 编辑状态
 * @returns 默认 state（各 Tab 选中「每一*」）
 */
export function createDefaultQuartzCronEditorState(): QuartzCronEditorState {
  return {
    second: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 5,
      rangeStart: 0,
      rangeEnd: 59,
      specificSpecific: [],
    },
    minute: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 5,
      rangeStart: 0,
      rangeEnd: 59,
      specificSpecific: [],
    },
    hour: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 1,
      rangeStart: 0,
      rangeEnd: 23,
      specificSpecific: [],
    },
    day: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 31,
      specificSpecific: [],
      cronLastSpecificDomDay: 1,
      cronDaysBeforeEomMinus: 1,
      cronDaysNearestWeekday: 1,
    },
    week: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 7,
      specificSpecific: [],
      cronNthDayDay: 1,
      cronNthDayNth: 1,
    },
    month: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 12,
      specificSpecific: [],
    },
    year: {
      cronEvery: '1',
      incrementStart: currentYear,
      incrementIncrement: 1,
      rangeStart: currentYear,
      rangeEnd: currentYear,
      specificSpecific: [],
    },
  }
}

function buildSecondText(state: QuartzCronEditorState): string {
  const field = state.second
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '0'
  }
}

function buildMinuteText(state: QuartzCronEditorState): string {
  const field = state.minute
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

function buildHourText(state: QuartzCronEditorState): string {
  const field = state.hour
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

function buildDayText(state: QuartzCronEditorState): string {
  const field = state.day
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
    case '4':
    case '11':
      return '?'
    case '3':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '5':
      return field.specificSpecific.join(',') || '1'
    case '6':
      return 'L'
    case '7':
      return 'LW'
    case '8':
      return '?'
    case '9':
      return `L-${field.cronDaysBeforeEomMinus ?? 1}`
    case '10':
      return `${field.cronDaysNearestWeekday ?? 1}W`
    default:
      return '*'
  }
}

function buildWeekText(state: QuartzCronEditorState): string {
  const dayEvery = String(state.day.cronEvery)
  const field = state.week
  switch (dayEvery) {
    case '1':
    case '3':
    case '5':
      return '?'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '4':
      return field.specificSpecific.join(',') || '1'
    case '6':
    case '7':
    case '9':
    case '10':
      return '?'
    case '8':
      return `${state.day.cronLastSpecificDomDay ?? 1}L`
    case '11':
      return `${field.cronNthDayDay ?? 1}#${field.cronNthDayNth ?? 1}`
    default:
      return '?'
  }
}

function buildMonthText(state: QuartzCronEditorState): string {
  const field = state.month
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '1'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

/**
 * 由编辑状态生成 Quartz 六段 Cron
 * @param state 弹窗编辑 state
 * @returns 秒 分 时 日 月 周 六段文本
 */
export function buildQuartzCronSegments(state: QuartzCronEditorState): QuartzCronSegments {
  return {
    second: buildSecondText(state) || '0',
    minute: buildMinuteText(state) || '*',
    hour: buildHourText(state) || '*',
    day: buildDayText(state) || '*',
    month: buildMonthText(state) || '*',
    week: buildWeekText(state) || '?',
  }
}

/**
 * 由编辑状态生成 Quartz Cron 字符串（对齐 Quartz.NET / 种子数据 6 字段）
 * @param state 弹窗编辑 state
 * @returns Cron 表达式
 */
export function buildQuartzCronExpression(state: QuartzCronEditorState): string {
  const seg = buildQuartzCronSegments(state)
  return `${seg.second} ${seg.minute} ${seg.hour} ${seg.day} ${seg.month} ${seg.week}`
}

function parseSecondPart(value: string): QuartzCronFieldState {
  const second = createDefaultQuartzCronEditorState().second
  if (value.includes('*')) {
    second.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    second.cronEvery = '2'
    second.incrementStart = Number(start)
    second.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    second.cronEvery = '3'
    second.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    second.cronEvery = '4'
    second.rangeStart = Number(start)
    second.rangeEnd = Number(end)
  } else {
    second.cronEvery = '1'
  }
  return second
}

function parseMinutePart(value: string): QuartzCronFieldState {
  const minute = createDefaultQuartzCronEditorState().minute
  if (value.includes('*')) {
    minute.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    minute.cronEvery = '2'
    minute.incrementStart = Number(start)
    minute.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    minute.cronEvery = '3'
    minute.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    minute.cronEvery = '4'
    minute.rangeStart = Number(start)
    minute.rangeEnd = Number(end)
  } else {
    minute.cronEvery = '1'
  }
  return minute
}

function parseHourPart(value: string): QuartzCronFieldState {
  const hour = createDefaultQuartzCronEditorState().hour
  if (value.includes('*')) {
    hour.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    hour.cronEvery = '2'
    hour.incrementStart = Number(start)
    hour.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    hour.cronEvery = '3'
    hour.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    hour.cronEvery = '4'
    hour.rangeStart = Number(start)
    hour.rangeEnd = Number(end)
  } else {
    hour.cronEvery = '1'
  }
  return hour
}

function parseDayWeekParts(dayValue: string, weekValue: string): Pick<QuartzCronEditorState, 'day' | 'week'> {
  const day = createDefaultQuartzCronEditorState().day
  const week = createDefaultQuartzCronEditorState().week
  if (!dayValue.includes('?')) {
    switch (true) {
      case dayValue.includes('*'):
        day.cronEvery = '1'
        break
      case dayValue.includes('/'):
        day.cronEvery = '3'
        day.incrementStart = Number(dayValue.split('/')[0])
        day.incrementIncrement = Number(dayValue.split('/')[1])
        break
      case dayValue.includes(','):
        day.cronEvery = '5'
        day.specificSpecific = dayValue.split(',').map(Number).sort((a, b) => a - b)
        break
      case dayValue.includes('LW'):
        day.cronEvery = '7'
        break
      case dayValue.includes('L-'):
        day.cronEvery = '9'
        day.cronDaysBeforeEomMinus = Number(dayValue.split('L-')[1])
        break
      case dayValue.includes('L'):
        if (dayValue.length === 1) {
          day.cronEvery = '6'
        } else {
          day.cronEvery = '8'
          day.cronLastSpecificDomDay = Number(dayValue.split('L')[0])
        }
        break
      case dayValue.includes('W'):
        day.cronEvery = '10'
        day.cronDaysNearestWeekday = Number(dayValue.split('W')[0])
        break
      default:
        day.cronEvery = '1'
    }
  } else {
    switch (true) {
      case weekValue.includes('/'):
        day.cronEvery = '2'
        week.incrementStart = Number(weekValue.split('/')[0])
        week.incrementIncrement = Number(weekValue.split('/')[1])
        break
      case weekValue.includes(','):
        day.cronEvery = '4'
        week.specificSpecific = weekValue.split(',').map(Number).sort((a, b) => a - b)
        break
      case weekValue.includes('#'):
        day.cronEvery = '11'
        week.cronNthDayDay = Number(weekValue.split('#')[0])
        week.cronNthDayNth = Number(weekValue.split('#')[1])
        break
      case /^\d+L$/.test(weekValue):
        day.cronEvery = '8'
        day.cronLastSpecificDomDay = Number(weekValue.replace('L', ''))
        break
      default:
        day.cronEvery = '1'
    }
  }
  return { day, week }
}

function parseMonthPart(value: string): QuartzCronFieldState {
  const month = createDefaultQuartzCronEditorState().month
  if (value.includes('*')) {
    month.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    month.cronEvery = '2'
    month.incrementStart = Number(start)
    month.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    month.cronEvery = '3'
    month.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    month.cronEvery = '4'
    month.rangeStart = Number(start)
    month.rangeEnd = Number(end)
  } else {
    month.cronEvery = '1'
  }
  return month
}

/**
 * 将 Quartz Cron 字符串反解析为编辑状态
 * @param expression Cron 表达式
 * @returns 弹窗 state；空串返回默认 state
 */
export function parseQuartzCronExpression(expression: string): QuartzCronEditorState {
  const trimmed = String(expression ?? '').trim()
  if (!trimmed) {
    return createDefaultQuartzCronEditorState()
  }
  const parts = trimmed.split(/\s+/)
  if (parts.length < 6) {
    return createDefaultQuartzCronEditorState()
  }
  const [second, minute, hour, day, month, week] = parts
  const dayWeek = parseDayWeekParts(day, week)
  return {
    second: parseSecondPart(second),
    minute: parseMinutePart(minute),
    hour: parseHourPart(hour),
    day: dayWeek.day,
    week: dayWeek.week,
    month: parseMonthPart(month),
    year: createDefaultQuartzCronEditorState().year,
  }
}
