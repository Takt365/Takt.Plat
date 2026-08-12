<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-moving-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：物料月移动价格推移（物料×月份转置；机种推移见 model-moving-trend） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <material-moving-trend-query-form
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:material-type="materialType"
      v-model:valuation="valuation"
      v-model:material-code="materialCode"
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
      :export-disabled="!plantCode?.trim() || !hasRows"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="trendFilterActions"
      export-permission="logistics:materials:material:moving:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <material-moving-trend-panel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
      active-tab="price"
      :plant-code="plantCode"
      :period-range="periodRange"
      :material-type="materialType"
      :valuation="valuation"
      :material-code="materialCode"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 物料月移动价格推移（仅物料价格 Tab；机种见独立菜单）
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
import { useTenantStore } from '@/stores/identity/tenant'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import MaterialMovingTrendPanel from './components/material-moving-trend-panel.vue'
import MaterialMovingTrendQueryForm from './components/material-moving-trend-query-form.vue'

/** 静态 locales 前缀 */
const localePrefix = 'logistics.materials.material-moving-trend.page'
const { t } = useI18n()
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
/** 产品物料类型（必选） */
const materialType = ref<string | undefined>()
/** 评估类别 */
const valuation = ref<string | undefined>()
/** 物料编码（可空） */
const materialCode = ref<string | undefined>()
/** 涨跌筛选：默认空=全部 */
const trendFilter = ref('')
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 右侧涨跌筛选 */
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
  }])
/** 明细面板 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 查询 */
function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!materialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return
  }
  if (!valuation.value?.trim()) {
    message.warning(t(`${localePrefix}.selectValuationRequired`))
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
}

/** 刷新 */
function handleRefresh() {
  void panelRef.value?.reload?.()
}

/** 清空工厂级联与结果 */
function clearPlantCascade() {
  plantCode.value = undefined
  materialType.value = undefined
  valuation.value = undefined
  materialCode.value = undefined
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 重置 */
function handleReset() {
  clearPlantCascade()
  periodRange.value = buildDefaultCostingPeriodRange(3)
  trendFilter.value = ''
}

/** 导出 */
async function handleExport() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportEmpty`))
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
    clearPlantCascade()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  periodRange.value = buildDefaultCostingPeriodRange(3)
  void getTaktDefaultPageSize()
})
</script>
