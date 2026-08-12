<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/modules -->
<!-- 文件名称：StatsOnlineModule.vue -->
<!-- 功能描述：数据看板在线统计（在线用户 / 今日访问 / 活跃会话） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <StatsMetricGrid
    :loading="loading"
    :items="metricItems"
  />
</template>

<script setup lang="ts">
/**
 * 在线统计：公司维度看板（在线人数、当日访问、活跃会话）
 */
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/stats-metric-grid.vue'
import { usePermissionStore } from '@/stores/identity/permission'
import type { OnlineDashboardStatistics } from '@/types/foundation/online'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/stats-query'

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 在线统计 */
const onlineStats = ref({
  users: 0,
  todayVisits: 0,
  sessions: 0,
})

/** a-statistic 指标 */
const metricItems = computed(() => [
  { key: 'users', title: t('dashboard.data-board.page.online.users'), value: onlineStats.value.users },
  { key: 'todayvisits', title: t('dashboard.data-board.page.online.todayvisits'), value: onlineStats.value.todayVisits },
  { key: 'sessions', title: t('dashboard.data-board.page.online.sessions'), value: onlineStats.value.sessions }])

/**
 * 加载在线看板统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const stats = await fetchDashboardMetricIfPermitted(
      permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard),
      'onlineDashboard',
      () => fetchDashboardGet<OnlineDashboardStatistics>(DASHBOARD_STATS_API.onlineDashboard),
      null,
    )
    onlineStats.value = {
      users: stats?.onlineUserCount ?? 0,
      todayVisits: stats?.todayVisitCount ?? 0,
      sessions: stats?.activeSessionCount ?? 0,
    }
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
