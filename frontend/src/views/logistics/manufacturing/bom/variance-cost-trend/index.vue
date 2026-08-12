<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/variance-cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：差异成本推移（工厂/期间/机种必选可多选/产品可选联动；有无差异组件×移动单价推移） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <variance-cost-trend-query-form
      v-model:plant-code="queryPlantCode"
      v-model:material-type="queryMaterialType"
      v-model:model-codes="queryModelCodes"
      v-model:product-codes="queryProductCodes"
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
      :right-actions="toolbarRightActions"
      export-permission="logistics:manufacturing:bom:variance:cost:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <variance-cost-trend-panel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 min-w-0 flex-1"
      :trend-filter="trendFilter"
      :sort-by="sortBy"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 差异成本推移 · 查询栏 + 工具栏 + 面板
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import {
  RiAddLine,
  RiArrowLeftRightLine,
  RiLineChartLine,
  RiListCheck,
  RiSortAlphabetAsc,
  RiSortNumberDesc,
  RiSubtractLine,
} from '@remixicon/vue'
import { getBomMaterialCostAnalysisPlantOptions } from '@/api/logistics/manufacturing/bom/material-cost-analysis'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { provideBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-analysis-master-context'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import VarianceCostTrendQueryForm from './components/variance-cost-trend-query-form.vue'
import VarianceCostTrendPanel from './components/variance-cost-trend-panel.vue'

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.bom.variance-cost-trend.page'
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCodes,
  queryProductCodes,
  periodRange,
} = provideBomMaterialCostAnalysisMasterContext()

const panelLoading = ref(false)
const exportLoading = ref(false)
const hasRows = ref(false)
const trendFilter = ref('')
/** 全量排序（分页前） */
const sortBy = ref('trend')
const toolbarRightActions = computed<ToolBarAction[]>(() => [
  {
    key: 'sort-trend',
    icon: RiLineChartLine,
    tooltip: t(`${localePrefix}.sort.trend`),
    active: sortBy.value === 'trend',
    onClick: () => setSortBy('trend'),
  },
  {
    key: 'sort-variance-desc',
    icon: RiSortNumberDesc,
    tooltip: t(`${localePrefix}.sort.varianceDesc`),
    active: sortBy.value === 'varianceDesc',
    onClick: () => setSortBy('varianceDesc'),
  },
  {
    key: 'sort-component-code',
    icon: RiSortAlphabetAsc,
    tooltip: t(`${localePrefix}.sort.componentCode`),
    active: sortBy.value === 'componentCode',
    onClick: () => setSortBy('componentCode'),
  },
  {
    key: 'trend-all',
    icon: RiListCheck,
    tooltip: t(`${localePrefix}.filter.all`),
    active: trendFilter.value === '',
    onClick: () => setTrendFilter(''),
  },
  {
    key: 'trend-new',
    icon: RiAddLine,
    tooltip: t(`${localePrefix}.trend.new`),
    active: trendFilter.value === 'new',
    onClick: () => setTrendFilter('new'),
  },
  {
    key: 'trend-removed',
    icon: RiSubtractLine,
    tooltip: t(`${localePrefix}.trend.removed`),
    active: trendFilter.value === 'removed',
    onClick: () => setTrendFilter('removed'),
  },
  {
    key: 'trend-version',
    icon: RiArrowLeftRightLine,
    tooltip: t(`${localePrefix}.trend.version`),
    active: trendFilter.value === 'version',
    onClick: () => setTrendFilter('version'),
  }])
const panelRef = ref<{
  reload?: (trendFilterOverride?: string, sortByOverride?: string) => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 已选机种 */
const selectedModelCodes = computed(() =>
  (queryModelCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean),
)

const canQuery = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!queryMaterialType.value?.trim()
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1],
)
const canExport = computed(() => canQuery.value && hasRows.value)

/**
 * 校验查询
 * @returns {boolean} 是否通过
 */
function validateQuery(): boolean {
  if (!queryPlantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return false
  }
  if (!queryMaterialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return false
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return false
  }
  return true
}

/** 查询 */
function handleSearch() {
  if (!validateQuery()) return
  void panelRef.value?.reload?.()
}

/**
 * 涨跌筛选
 * @param {string} value 筛选值
 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) return
  trendFilter.value = value
  if (!canQuery.value) return
  void panelRef.value?.reload?.(value, sortBy.value)
}

/**
 * 全量排序（分页前作用于整表）
 * @param {string} value trend / varianceDesc / componentCode
 */
function setSortBy(value: string) {
  if (sortBy.value === value) return
  sortBy.value = value
  if (!canQuery.value) return
  void panelRef.value?.reload?.(trendFilter.value, value)
}

/** 刷新 */
function handleRefresh() {
  if (!validateQuery()) return
  void panelRef.value?.reload?.()
}

/** 默认期间 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(2)
}

/** 默认工厂 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getBomMaterialCostAnalysisPlantOptions()
      const hit = (plants ?? []).find(
        (o) => String(o.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : undefined
    } catch {
      matched = undefined
    }
  }
  queryPlantCode.value = matched
  queryMaterialType.value = undefined
  queryModelCodes.value = []
  queryProductCodes.value = []
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  applyDefaultPeriodRange()
  queryModelCodes.value = []
  queryProductCodes.value = []
  trendFilter.value = ''
  sortBy.value = 'trend'
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!validateQuery()) return
  exportLoading.value = true
  try {
    await panelRef.value?.handleExport?.()
  } finally {
    exportLoading.value = false
  }
}

onMounted(async () => {
  applyDefaultPeriodRange()
  await applyDefaultPlantFromCompany()
})
</script>
