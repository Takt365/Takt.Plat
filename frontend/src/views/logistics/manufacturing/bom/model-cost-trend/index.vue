<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：机种成本推移（材料成本；机种/物料多选可空） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <model-cost-trend-query-form
      v-model:plant-code="queryPlantCode"
      v-model:material-type="queryMaterialType"
      v-model:model-codes="queryModelCodes"
      v-model:component-codes="queryComponentCodes"
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
      export-permission="logistics:manufacturing:bom:model:cost:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <model-cost-trend-panel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 min-w-0 flex-1"
      :trend-filter="trendFilter"
      :sort-by="sortBy"
      merge-mode="summary"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 机种成本推移 · 查询栏 + 工具栏
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiFundsLine,
  RiLineChartLine,
  RiListCheck,
  RiSortNumberAsc,
  RiSortNumberDesc,
} from '@remixicon/vue'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/cost-option'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTenantStore } from '@/stores/identity/tenant'
import ModelCostTrendQueryForm from './components/model-cost-trend-query-form.vue'
import { provideBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-analysis-master-context'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import ModelCostTrendPanel from './components/model-cost-trend-panel.vue'

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.bom.model-cost-trend.page'
const tenantStore = useTenantStore()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCodes,
  queryComponentCodes,
  periodRange,
} = provideBomMaterialCostAnalysisMasterContext()

/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选 */
const trendFilter = ref('')
/** 全量排序（分页前） */
const sortBy = ref('productCountDesc')
/** 右侧：全量排序 + 涨跌筛选 */
const toolbarRightActions = computed<ToolBarAction[]>(() => [
  {
    key: 'sort-product-count-desc',
    icon: RiSortNumberDesc,
    tooltip: t(`${localePrefix}.sort.productCountDesc`),
    active: sortBy.value === 'productCountDesc',
    onClick: () => setSortBy('productCountDesc'),
  },
  {
    key: 'sort-product-count-asc',
    icon: RiSortNumberAsc,
    tooltip: t(`${localePrefix}.sort.productCountAsc`),
    active: sortBy.value === 'productCountAsc',
    onClick: () => setSortBy('productCountAsc'),
  },
  {
    key: 'sort-trend',
    icon: RiLineChartLine,
    tooltip: t(`${localePrefix}.sort.trend`),
    active: sortBy.value === 'trend',
    onClick: () => setSortBy('trend'),
  },
  {
    key: 'sort-variance-desc',
    icon: RiFundsLine,
    tooltip: t(`${localePrefix}.sort.varianceDesc`),
    active: sortBy.value === 'varianceDesc',
    onClick: () => setSortBy('varianceDesc'),
  },
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
  }])
/** 明细面板 */
const panelRef = ref<{
  reload?: (trendFilterOverride?: string, sortByOverride?: string) => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 查询条件是否满足 */
const canQuery = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && !!queryMaterialType.value?.trim()
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1],
)

/** 可否导出 */
const canExport = computed(() => canQuery.value && hasRows.value)

/**
 * 校验查询条件
 * @returns 是否通过
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
  if (!validateQuery()) {
    return
  }
  void panelRef.value?.reload?.()
}

/**
 * 涨跌筛选
 * @param value 空 / changed / up / down
 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
  if (!canQuery.value) {
    return
  }
  void panelRef.value?.reload?.(value, sortBy.value)
}

/**
 * 全量排序（分页前作用于整表）
 * @param value productCountDesc / productCountAsc / trend
 */
function setSortBy(value: string) {
  if (sortBy.value === value) {
    return
  }
  sortBy.value = value
  if (!canQuery.value) {
    return
  }
  void panelRef.value?.reload?.(trendFilter.value, value)
}

/** 刷新 */
function handleRefresh() {
  if (!validateQuery()) {
    return
  }
  void panelRef.value?.reload?.()
}

/** 默认核算年月 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(3)
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在 plant-options（RelatedPlant∩本表）时选中；无则清空
 * @returns {Promise<void>}
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getBomCostOptionPlantOptions()
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
  queryComponentCodes.value = []
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryMaterialType.value = undefined
  applyDefaultPeriodRange()
  trendFilter.value = ''
  sortBy.value = 'productCountDesc'
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 导出 */
async function handleExport() {
  if (!validateQuery()) {
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportFailed`))
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
    void (async () => {
      await applyDefaultPlantFromCompany()
      hasRows.value = false
      panelRef.value?.clear?.()
    })()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  applyDefaultPeriodRange()
  await applyDefaultPlantFromCompany()
  void getTaktDefaultPageSize()
})
</script>
