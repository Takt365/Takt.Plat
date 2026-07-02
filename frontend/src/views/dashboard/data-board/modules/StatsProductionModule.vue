<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/modules -->
<!-- 文件名称：StatsProductionModule.vue -->
<!-- 功能描述：数据看板生产统计（月产量/达成率/投入/停线损失/工时分项） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <a-spin :spinning="loading">
      <p class="mb-2 text-xs text-text-secondary">
        {{ t('dashboard.data-board.page.periodmonth') }}
      </p>
      <p class="text-base font-medium leading-relaxed text-text">
        {{ productionSummaryLine }}
      </p>
      <p class="mt-2 text-sm leading-relaxed text-text-secondary">
        {{ inputSummaryLine }}
      </p>
      <a-row :gutter="16" class="mt-4">
        <a-col :xs="12" :sm="6">
          <a-statistic
            :title="t('dashboard.data-board.page.production.monthdowntime')"
            :value="productionStats.monthDowntimeMinutes"
            :precision="0"
            suffix="min"
          />
        </a-col>
        <a-col :xs="12" :sm="6">
          <a-statistic
            :title="t('dashboard.data-board.page.production.monthinputminutes')"
            :value="productionStats.monthInputMinutes"
            :precision="0"
            suffix="min"
          />
        </a-col>
        <a-col :xs="12" :sm="6">
          <a-statistic
            :title="t('dashboard.data-board.page.production.monthprodminutes')"
            :value="productionStats.monthProdMinutes"
            :precision="0"
            suffix="min"
          />
        </a-col>
        <a-col :xs="12" :sm="6">
          <a-statistic
            :title="t('dashboard.data-board.page.production.monthactualminutes')"
            :value="productionStats.monthActualMinutes"
            :precision="0"
            suffix="min"
          />
        </a-col>
      </a-row>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 生产统计：组立 + PCBA 合并；看板呈现「N月生产：实绩…/达成率…」「投入：…（损失：…）」
 */
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePermissionStore } from '@/stores/identity/permission'
import type { AssyOutputProductionStat } from '@/types/logistics/manufacturing/output/assy-output'
import type { PcbaOutputProductionStat } from '@/types/logistics/manufacturing/output/pcba-output'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getCurrentMonthRange,
  scheduleDashboardLoad,
} from '../utils/stats-query'

/** 空生产统计 */
const EMPTY_PRODUCTION_STAT = {
  statMonth: '',
  monthStdCapacity: 0,
  monthProdActualQty: 0,
  monthAchievementRate: 0,
  monthDowntimeMinutes: 0,
  monthInputMinutes: 0,
  monthProdMinutes: 0,
  monthActualMinutes: 0,
}

/** 合并后的生产统计 */
interface MergedProductionStat {
  statMonth: string
  monthStdCapacity: number
  monthProdActualQty: number
  monthAchievementRate: number
  monthDowntimeMinutes: number
  monthInputMinutes: number
  monthProdMinutes: number
  monthActualMinutes: number
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 生产统计 */
const productionStats = ref<MergedProductionStat>({ ...EMPTY_PRODUCTION_STAT })

/** 产量汇总行：5月生产：实绩3000（计划3500）/达成率85% */
const productionSummaryLine = computed(() => {
  const monthLabel = formatStatMonthLabel(productionStats.value.statMonth)
  return t('dashboard.data-board.page.production.summaryProduction', {
    month: monthLabel,
    actual: Math.round(productionStats.value.monthProdActualQty),
    plan: Math.round(productionStats.value.monthStdCapacity),
    rate: formatRate(productionStats.value.monthAchievementRate),
  })
})

/** 投入汇总行：投入：10000分钟（损失：2500分钟） */
const inputSummaryLine = computed(() => t('dashboard.data-board.page.production.summaryInput', {
  input: Math.round(productionStats.value.monthInputMinutes),
  loss: Math.round(productionStats.value.monthDowntimeMinutes),
}))

/**
 * 统计月份转展示标签（yyyy-MM → 5月）
 * @param statMonth 统计月份
 * @returns {string} 展示标签
 */
function formatStatMonthLabel(statMonth: string): string {
  if (!statMonth) {
    return t('dashboard.data-board.page.production.currentMonth')
  }
  const parts = statMonth.split('-')
  const monthNum = Number.parseInt(parts[1] ?? '', 10)
  if (!Number.isFinite(monthNum) || monthNum < 1 || monthNum > 12) {
    return statMonth
  }
  return t('dashboard.data-board.page.production.monthLabel', { month: monthNum })
}

/**
 * 达成率展示（保留 1 位小数，整数时不带小数）
 * @param rate 达成率
 * @returns {string} 展示值
 */
function formatRate(rate: number): string {
  const rounded = Math.round(rate * 10) / 10
  return Number.isInteger(rounded) ? String(rounded) : rounded.toFixed(1)
}

/**
 * 合并组立与 PCBA 生产统计
 * @param assy 组立统计
 * @param pcba PCBA 统计
 * @returns {MergedProductionStat} 合并结果
 */
function mergeProductionStats(
  assy: AssyOutputProductionStat,
  pcba: PcbaOutputProductionStat,
): MergedProductionStat {
  const monthStdCapacity = (assy.monthStdCapacity ?? 0) + (pcba.monthStdCapacity ?? 0)
  const monthProdActualQty = (assy.monthProdActualQty ?? 0) + (pcba.monthProdActualQty ?? 0)
  const monthDowntimeMinutes = (assy.monthDowntimeMinutes ?? 0) + (pcba.monthDowntimeMinutes ?? 0)
  const monthInputMinutes = (assy.monthInputMinutes ?? 0) + (pcba.monthInputMinutes ?? 0)
  const monthProdMinutes = (assy.monthProdMinutes ?? 0) + (pcba.monthProdMinutes ?? 0)
  const monthActualMinutes = (assy.monthActualMinutes ?? 0) + (pcba.monthActualMinutes ?? 0)
  const monthAchievementRate = monthStdCapacity > 0
    ? Math.round((monthProdActualQty / monthStdCapacity) * 10000) / 100
    : 0
  const statMonth = assy.statMonth || pcba.statMonth
  return {
    statMonth,
    monthStdCapacity,
    monthProdActualQty,
    monthAchievementRate,
    monthDowntimeMinutes,
    monthInputMinutes,
    monthProdMinutes,
    monthActualMinutes,
  }
}

/**
 * 加载本月生产统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canAssy = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.assyOutputList)
  const canPcba = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.pcbaOutputList)
  loading.value = true
  try {
    const month = getCurrentMonthRange()
    const query = { prodDateStart: month.start, prodDateEnd: month.end }
    const [assyStat, pcbaStat] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canAssy,
        'assyProductionStat',
        () => fetchDashboardGet<AssyOutputProductionStat>(
          DASHBOARD_STATS_API.assyOutputProductionStat,
          query,
        ).then((res) => ({ ...EMPTY_PRODUCTION_STAT, ...res })),
        { ...EMPTY_PRODUCTION_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canPcba,
        'pcbaProductionStat',
        () => fetchDashboardGet<PcbaOutputProductionStat>(
          DASHBOARD_STATS_API.pcbaOutputProductionStat,
          query,
        ).then((res) => ({ ...EMPTY_PRODUCTION_STAT, ...res })),
        { ...EMPTY_PRODUCTION_STAT },
      ),
    ])
    productionStats.value = mergeProductionStats(assyStat, pcbaStat)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  scheduleDashboardLoad(loadData)
})

useTableRefresh(() => {
  scheduleDashboardLoad(loadData)
})
</script>
