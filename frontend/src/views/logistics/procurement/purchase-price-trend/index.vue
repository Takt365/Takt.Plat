<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-price-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：采购价格推移 / 机种价格推移（BOM 机种产品组 + 月单价转置） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <purchase-price-trend-query-form
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:supplier-code="supplierCode"
      v-model:material-code="materialCode"
      v-model:price-type="priceType"
      :loading="panelLoading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <a-tabs
      v-model:activeKey="activeTab"
      class="purchase-price-trend-tabs mb-1 shrink-0"
    >
      <a-tab-pane
        key="price"
        :tab="t(`${localePrefix}.tabs.price`)"
      />
      <a-tab-pane
        key="model"
        :tab="t(`${localePrefix}.tabs.model`)"
      />
    </a-tabs>
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
      export-permission="logistics:procurement:purchase:price:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <purchase-price-trend-panel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
      :active-tab="activeTab"
      :plant-code="plantCode"
      :period-range="periodRange"
      :supplier-code="supplierCode"
      :material-code="materialCode"
      :price-type="priceType"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 采购价格推移 / 机种价格推移（Tabs：price | model）
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiListCheck,
  RiTrophyLine,
} from '@remixicon/vue'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTenantStore } from '@/stores/identity/tenant'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import PurchasePriceTrendQueryForm from './components/purchase-price-trend-query-form.vue'
import PurchasePriceTrendPanel from './components/purchase-price-trend-panel.vue'

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.procurement.purchase-price-trend.page'
const tenantStore = useTenantStore()

/** 当前 Tab：price=采购价格推移；model=机种价格推移 */
const activeTab = ref<'price' | 'model'>('price')
/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)
/** 供应商编码 */
const supplierCode = ref<string | undefined>()
/** 物料编码 */
const materialCode = ref<string | undefined>()
/** 价格类型 */
const priceType = ref<string | undefined>()
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选：price 默认空=全部；model 默认 leading=领涨领跌各 50 */
const trendFilter = ref('')
/** 右侧涨跌筛选：仅图标 + tooltip，与工具栏右侧一致 */
const trendFilterActions = computed<ToolBarAction[]>(() => {
  const actions: ToolBarAction[] = []
  if (activeTab.value === 'model') {
    actions.push({
      key: 'trend-leading',
      icon: RiTrophyLine,
      tooltip: t(`${localePrefix}.filter.leading`),
      active: trendFilter.value === 'leading' || trendFilter.value === '',
      onClick: () => setTrendFilter('leading'),
    })
  }
  actions.push(
    {
      key: 'trend-all',
      icon: RiListCheck,
      tooltip: t(`${localePrefix}.filter.all`),
      active: activeTab.value === 'model'
        ? trendFilter.value === 'all'
        : trendFilter.value === '',
      onClick: () => setTrendFilter(activeTab.value === 'model' ? 'all' : ''),
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
  )
  return actions
})
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

/**
 * 按当前 Tab 归一化默认涨跌筛选
 * @param {string} tab 当前 Tab
 */
function applyDefaultTrendFilterForTab(tab: 'price' | 'model') {
  if (tab === 'model') {
    if (trendFilter.value === '' || trendFilter.value === 'all') {
      trendFilter.value = 'leading'
    }
    return
  }
  if (trendFilter.value === 'leading') {
    trendFilter.value = ''
  }
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
  plantCode.value = plant || undefined
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  supplierCode.value = undefined
  materialCode.value = undefined
  priceType.value = undefined
  applyDefaultPeriodRange()
  trendFilter.value = activeTab.value === 'model' ? 'leading' : ''
  hasRows.value = false
  panelRef.value?.clear?.()
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

watch(activeTab, (tab) => {
  applyDefaultTrendFilterForTab(tab)
})

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

<style scoped>
:deep(.purchase-price-trend-tabs .ant-tabs-nav) {
  margin-bottom: 0;
}
</style>
