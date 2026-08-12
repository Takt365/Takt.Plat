<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/modules -->
<!-- 文件名称：StatsChangeModule.vue -->
<!-- 功能描述：数据看板设变统计（TaktEc 主表数量 + TaktEcExec/Kanban 实施） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <a-spin :spinning="loading">
      <p class="mb-2 text-xs text-text-secondary">
        {{ t('dashboard.data-board.page.periodmonth') }}
      </p>
      <p class="mb-4 text-base font-medium leading-relaxed text-text">
        {{ ecSummaryLine }}
      </p>
      <StatsMetricGrid
        :loading="false"
        :items="metricItems"
      />
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变统计：TaktEcGijutsus/stat 主表+子表数量；TaktEcKanbans 部门行 + 实施路径
 */
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/stats-metric-grid.vue'
import { usePermissionStore } from '@/stores/identity/permission'
import { TaktEcImplementationStatus } from '@/constants/logistics/ec-implementation-status'
import type { EcGijutsuStat } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardGet,
  fetchDashboardDeptExecutionCount,
  fetchDashboardMetricIfPermitted,
  fetchDashboardPagedTotal,
  getCurrentMonthRange,
  scheduleDashboardLoad,
} from '../utils/stats-query'

/** isImplemented：未实施 */
const EC_DEPT_NOT_IMPLEMENTED = 0
/** isImplemented：已实施 */
const EC_DEPT_IMPLEMENTED = 1

/** 空设变主统计 */
const EMPTY_EC_STAT: EcGijutsuStat = {
  statMonth: '',
  ecCount: 0,
  ecDetailCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 设变主表统计 */
const ecStat = ref<EcGijutsuStat>({ ...EMPTY_EC_STAT })
/** 设变部门实施统计 */
const changeData = ref({
  total: 0,
  notImplemented: 0,
  implemented: 0,
  inProgressEc: 0,
  notOfficiallyCompletedEc: 0,
})

/** 当月设变摘要行，如「当月设变1（18）」 */
const ecSummaryLine = computed(() =>
  t('dashboard.data-board.page.change.summaryEcCount', {
    ecCount: ecStat.value.ecCount,
    detailCount: ecStat.value.ecDetailCount,
  }),
)

/** a-statistic 指标 */
const metricItems = computed(() => [
  { key: 'total', title: t('dashboard.data-board.page.change.total'), value: changeData.value.total },
  { key: 'notimplemented', title: t('dashboard.data-board.page.change.notimplemented'), value: changeData.value.notImplemented },
  { key: 'implemented', title: t('dashboard.data-board.page.change.implemented'), value: changeData.value.implemented },
  { key: 'inprogress', title: t('dashboard.data-board.page.change.inprogressec'), value: changeData.value.inProgressEc },
  { key: 'notofficial', title: t('dashboard.data-board.page.change.notofficiallycompleted'), value: changeData.value.notOfficiallyCompletedEc }])

/**
 * 加载设变主表统计（当月录入日期范围）
 * @returns {Promise<EcGijutsuStat>} 设变统计
 */
async function loadEcGijutsuStat(): Promise<EcGijutsuStat> {
  const monthRange = getCurrentMonthRange()
  const data = await fetchDashboardGet<EcGijutsuStat>(DASHBOARD_STATS_API.ecStat, {
    ecEntryDateStart: monthRange.start,
    ecEntryDateEnd: monthRange.end,
  })
  return data ?? { ...EMPTY_EC_STAT }
}

/**
 * 加载设变部门实施统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canListEc = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecList)
  const canListDept = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecDeptExecutionCount)
  const canListKanban = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecKanbanList)
  loading.value = true
  try {
    const [stat, total, notImplemented, implemented, inProgressEc, notOfficiallyCompletedEc] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canListEc,
        'ecStat',
        () => loadEcGijutsuStat(),
        { ...EMPTY_EC_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canListDept,
        'ecDeptTotal',
        () => fetchDashboardDeptExecutionCount(),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListDept,
        'ecDeptNotImplemented',
        () => fetchDashboardDeptExecutionCount(EC_DEPT_NOT_IMPLEMENTED),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListDept,
        'ecDeptImplemented',
        () => fetchDashboardDeptExecutionCount(EC_DEPT_IMPLEMENTED),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListKanban,
        'ecKanbanInProgress',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.ecKanbanList, {
          implementationStatus: TaktEcImplementationStatus.InProgress,
        }),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListKanban,
        'ecKanbanNotOfficial',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.ecKanbanList, { onlyNotOfficiallyCompleted: 1 }),
        0,
      )])
    ecStat.value = stat
    changeData.value = { total, notImplemented, implemented, inProgressEc, notOfficiallyCompletedEc }
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
