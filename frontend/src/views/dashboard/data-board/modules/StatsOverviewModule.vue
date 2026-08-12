<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/modules -->
<!-- 文件名称：StatsOverviewModule.vue -->
<!-- 功能描述：数据看板统计概览（待办 / 消息 / 在线 / 订单 / 设变 / 在制） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <StatsMetricGrid
    :loading="loading"
    :items="metricItems"
    :col-xs="12"
    :col-sm="8"
    :col-md="8"
    :col-lg="4"
  />
</template>

<script setup lang="ts">
/**
 * 统计概览：聚合工作流待办、消息、在线、本月订单、设变部门行、在制工单
 */
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/stats-metric-grid.vue'
import { usePermissionStore } from '@/stores/identity/permission'
import type { MessageStatistics } from '@/types/foundation/message'
import type { OnlineDashboardStatistics } from '@/types/foundation/online'
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

/** productionOrderStatus：生产中 */
const PRODUCTION_ORDER_IN_PROGRESS = 1

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 概览指标 */
const overview = ref({
  todoCount: 0,
  unreadCount: 0,
  onlineUsers: 0,
  monthOrders: 0,
  ecTotal: 0,
  wipOrders: 0,
})

/** a-statistic 指标 */
const metricItems = computed(() => [
  { key: 'todo', title: t('dashboard.data-board.page.overview.todo'), value: overview.value.todoCount },
  { key: 'unread', title: t('dashboard.data-board.page.overview.unread'), value: overview.value.unreadCount },
  { key: 'online', title: t('dashboard.data-board.page.overview.online'), value: overview.value.onlineUsers },
  { key: 'orders', title: t('dashboard.data-board.page.overview.monthorders'), value: overview.value.monthOrders },
  { key: 'ec', title: t('dashboard.data-board.page.overview.ectotal'), value: overview.value.ecTotal },
  { key: 'wip', title: t('dashboard.data-board.page.overview.wip'), value: overview.value.wipOrders }])

/**
 * 加载未读消息数
 * @returns {Promise<number>} 未读条数
 */
async function loadUnreadCount(): Promise<number> {
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.messageStatistics)) {
    const stats = await fetchDashboardGet<MessageStatistics>(DASHBOARD_STATS_API.messageStatistics)
    if (stats) {
      return stats.unreadCount ?? 0
    }
  }
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.messageUnreadList)) {
    return fetchDashboardPagedTotal(DASHBOARD_STATS_API.messageUnreadList)
  }
  return 0
}

/**
 * 加载概览指标（核心指标优先，物流指标延后）
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const month = getCurrentMonthRange()
    const [todoCount, unreadCount, onlineUsers] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.flowTodoList),
        'todo',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.flowTodoList),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasAny([
          DASHBOARD_STATS_PERMISSION.messageStatistics,
          DASHBOARD_STATS_PERMISSION.messageUnreadList]),
        'unread',
        loadUnreadCount,
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard),
        'online',
        async () => {
          const stats = await fetchDashboardGet<OnlineDashboardStatistics>(DASHBOARD_STATS_API.onlineDashboard)
          return stats?.onlineUserCount ?? 0
        },
        0,
      )])
    overview.value = {
      ...overview.value,
      todoCount,
      unreadCount,
      onlineUsers,
    }
    const [monthOrders, ecTotal, wipOrders] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.salesOrderList),
        'monthOrders',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.salesOrderList, {
          orderDateStart: month.start,
          orderDateEnd: month.end,
        }),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecDeptExecutionCount),
        'ecDeptTotal',
        () => fetchDashboardDeptExecutionCount(),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.productionOrderList),
        'wipOrders',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.productionOrderList, {
          productionOrderStatus: PRODUCTION_ORDER_IN_PROGRESS,
        }),
        0,
      )])
    overview.value = {
      todoCount,
      unreadCount,
      onlineUsers,
      monthOrders,
      ecTotal,
      wipOrders,
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
