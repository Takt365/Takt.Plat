<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：产品成本分析报表：必选单个产品 → 明细表组件月材料成本转置涨跌 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <material-cost-trend-query-form
      v-model:plant-code="queryPlantCode"
      v-model:model-code="queryModelCode"
      v-model:product-code="queryProductCode"
      v-model:period-range="periodRange"
      :loading="panelLoading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!canExport"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="trendFilterActions"
      export-permission="logistics:manufacturing:bom:material:cost:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <material-cost-trend-panel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 产品成本分析报表：必选单个产品，展示明细表组件行转置
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiListCheck,
} from '@remixicon/vue'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTenantStore } from '@/stores/identity/tenant'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import MaterialCostTrendPanel from './components/material-cost-trend-panel.vue'
import MaterialCostTrendQueryForm from './components/material-cost-item-analysis-query-form.vue'
import { MATERIAL_COST_ANALYSIS_LOCALE_PREFIX } from './composables/use-material-cost-item-analysis'
import { provideBomMaterialCostAnalysisMasterContext } from './composables/use-material-cost-analysis-master-context'

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const {
  queryPlantCode,
  queryModelCode,
  queryProductCode,
  periodRange,
} = provideBomMaterialCostAnalysisMasterContext()
const tenantStore = useTenantStore()

/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选 */
const trendFilter = ref('')
/** 右侧涨跌筛选：仅图标 + tooltip，与工具栏右侧一致 */
const trendFilterActions = computed<ToolBarAction[]>(() => [
  {
    key: 'trend-all',
    icon: RiListCheck,
    tooltip: t(`${localePrefix}.filter.all`),
    active: trendFilter.value === '',
    onClick: () => setTrendFilter(''),
  },
  {
    key: 'trend-changed',
    icon: RiArrowUpDownLine,
    tooltip: t(`${localePrefix}.filter.changed`),
    active: trendFilter.value === 'changed',
    onClick: () => setTrendFilter('changed'),
  },
  {
    key: 'trend-up',
    icon: RiArrowUpLine,
    tooltip: t(`${localePrefix}.trend.up`),
    active: trendFilter.value === 'up',
    onClick: () => setTrendFilter('up'),
  },
  {
    key: 'trend-down',
    icon: RiArrowDownLine,
    tooltip: t(`${localePrefix}.trend.down`),
    active: trendFilter.value === 'down',
    onClick: () => setTrendFilter('down'),
  },
])
/** 明细面板 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 可否导出 */
const canExport = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!queryProductCode.value?.trim()
    && hasRows.value,
)

/** 查询 */
function handleSearch() {
  if (!queryPlantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!queryProductCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectProductRequired`))
    return
  }
  if (!periodRange.value?.[0]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  void panelRef.value?.reload?.()
}

/** 涨跌筛选 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
  void panelRef.value?.reload?.()
}

/** 刷新 */
function handleRefresh() {
  void panelRef.value?.reload?.()
}

/** 默认核算年月 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(3)
}

/** 默认工厂 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const plant = await resolveCurrentCompanyRelatedPlantCode()
  queryPlantCode.value = plant || undefined
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryModelCode.value = undefined
  queryProductCode.value = undefined
  applyDefaultPeriodRange()
  trendFilter.value = ''
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!canExport.value) {
    message.warning(t(`${localePrefix}.selectProductRequired`))
    return
  }
  exportLoading.value = true
  try {
    await panelRef.value?.handleExport?.()
  } finally {
    exportLoading.value = false
  }
}

watch(
  () => tenantStore.companyCode,
  () => {
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  applyDefaultPeriodRange()
  await applyDefaultPlantFromCompany()
  void getTaktDefaultPageSize()
})
</script>
