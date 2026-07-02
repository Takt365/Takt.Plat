<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/dashboard/data-board/modules -->
<!-- 文件名称：StatsSalesModule.vue -->
<!-- 功能描述：数据看板销售统计（本月发票 / 销售额 / 同比） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <StatsMetricGrid
    :loading="loading"
    :period-label="t('dashboard.data-board.page.periodmonth')"
    :items="metricItems"
  />
</template>

<script setup lang="ts">
/**
 * 销售统计：本月销售发票数、销售额（本位币元）、销售额同比
 */
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/stats-metric-grid.vue'
import { usePermissionStore } from '@/stores/identity/permission'
import type { SalesInvoiceStat } from '@/types/logistics/sales/invoice'
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
/** 销售统计 */
const salesStats = ref({
  invoices: 0,
  amount: 0,
  yoy: 0,
})

/** a-statistic 指标 */
const metricItems = computed(() => [
  { key: 'invoices', title: t('dashboard.data-board.page.sales.orders'), value: salesStats.value.invoices },
  {
    key: 'amount',
    title: t('dashboard.data-board.page.sales.amount'),
    value: salesStats.value.amount,
    prefix: '¥',
    precision: 2,
  },
  {
    key: 'yoy',
    title: t('dashboard.data-board.page.sales.yoy'),
    value: salesStats.value.yoy,
    suffix: '%',
    precision: 1,
  },
])

/**
 * 加载本月销售统计（销售发票 invoice-stat）
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.salesInvoiceStat)
  loading.value = true
  try {
    const stat = await fetchDashboardMetricIfPermitted(
      canList,
      'salesInvoiceStat',
      () => fetchDashboardGet<SalesInvoiceStat>(DASHBOARD_STATS_API.salesInvoiceStat),
      null,
    )
    salesStats.value = {
      invoices: stat?.monthInvoiceCount ?? 0,
      amount: stat?.monthSalesAmount ?? 0,
      yoy: stat?.salesAmountYoYPercent ?? 0,
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
