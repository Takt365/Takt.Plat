<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-analysis -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：BOM 成本分析：TaktBomMaterialCost 转置单表（机种/产品/品名/周期/涨跌/环比） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <material-cost-analysis-query-form
      v-model:plant-code="plantCode"
      v-model:model-code="modelCode"
      v-model:product-code="productCode"
      v-model:period-range="periodRange"
      :loading="loading"
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
      :export-disabled="!hasQuery || total === 0"
      :export-loading="exportLoading"
      :refresh-loading="loading"
      :right-actions="trendFilterActions"
      export-permission="logistics:manufacturing:bom:material:cost:analysis:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <div
      ref="tableWrapRef"
      class="min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        entity-scope="company"
        table-mode="single"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'productCode'"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodCost(record as BomMaterialCostItemTransposed, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemTransposed).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemTransposed).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemTransposed).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemTransposed).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemTransposed).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemTransposed).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div
            v-if="total > 0 && periodOrder.length > 0"
            class="flex max-w-full flex-wrap gap-x-3 gap-y-1 text-sm text-text"
          >
            <span class="font-medium shrink-0">{{ summaryLabel }}</span>
            <span
              v-for="period in periodOrder"
              :key="period"
              class="shrink-0"
            >
              {{ period }}: {{ formatCost(periodCostTotals[period]) }}
            </span>
            <span
              class="shrink-0 font-medium"
              :class="varianceClass(varianceAmountTotal)"
            >
              {{ t(`${localePrefix}.columns.varianceAmount`) }}: {{ formatCost(varianceAmountTotal) }}
            </span>
          </div>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 成本分析：TaktBomMaterialCost 转置（机种 / 产品 / 品名 / 周期 / 涨跌）
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiListCheck,
} from '@remixicon/vue'
import {
  exportBomMaterialCostItemTransposed,
  getBomMaterialCostItemTransposedList,
} from '@/api/logistics/manufacturing/bom/material-cost-item'
import type { BomMaterialCostItemTransposed } from '@/types/logistics/manufacturing/bom/material-cost-trend'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/takt-paged'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTenantStore } from '@/stores/identity/tenant'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-export-file-name'
import MaterialCostAnalysisQueryForm from './components/material-cost-item-analysis-query-form.vue'
import {
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  bomMomTrendSortRank,
  compareBomMomNullableNumber,
  periodRangeToCostingDateQuery,
  useMaterialCostAnalysis,
} from './composables/use-material-cost-item-analysis'

const { t } = useI18n()
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const { formatCost, formatPercent, trendLabel, trendClass, varianceClass } = useMaterialCostAnalysis()
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 机种 */
const modelCode = ref<string | undefined>()
/** 产品（可空） */
const productCode = ref<string | undefined>()
/** 核算期间 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
/** 涨跌筛选：空=全部；changed/up/down */
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
/** 转置行 */
const rows = ref<BomMaterialCostItemTransposed[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** loading */
const loading = ref(false)
const exportLoading = ref(false)
/** 分页（ensure 前用安全默认值） */
const pageIndex = ref(1)
const pageSize = ref(20)
const total = ref(0)
/** 表体 */
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

/** 是否已具备查询条件 */
const hasQuery = computed(
  () => !!plantCode.value?.trim() && !!periodRange.value?.[0],
)

/** 列：机种 / 产品 / 品名 / 周期… / 涨跌 / 环比差额 / 环比% */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcost.modelcode'),
      dataIndex: 'modelCode',
      key: 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcost.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcost.productdescription'),
      dataIndex: 'productDescription',
      key: 'productDescription',
      width: 180,
      ellipsis: true,
    },
  ]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodCosts', period],
      key: `period_${period}`,
      width: 120,
      align: 'right',
    })
  }
  cols.push(
    {
      title: t(`${localePrefix}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 88,
      fixed: 'right',
      sorter: (a: BomMaterialCostItemTransposed, b: BomMaterialCostItemTransposed) =>
        bomMomTrendSortRank(a.trend) - bomMomTrendSortRank(b.trend),
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 120,
      align: 'right',
      fixed: 'right',
      sorter: (a: BomMaterialCostItemTransposed, b: BomMaterialCostItemTransposed) =>
        compareBomMomNullableNumber(a.varianceAmount, b.varianceAmount),
    },
    {
      title: t(`${localePrefix}.columns.variancePercent`),
      dataIndex: 'variancePercent',
      key: 'variancePercent',
      width: 100,
      align: 'right',
      fixed: 'right',
      sorter: (a: BomMaterialCostItemTransposed, b: BomMaterialCostItemTransposed) =>
        compareBomMomNullableNumber(a.variancePercent, b.variancePercent),
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/** 合计首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 全量期间成本合计（API：分页前、已应用涨跌筛选） */
const periodCostTotals = ref<Record<string, number>>({})
/** 全量环比差额合计（API：分页前、已应用涨跌筛选） */
const varianceAmountTotal = ref<number | null>(null)

/**
 * 行主键
 * @param {BomMaterialCostItemTransposed} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemTransposed): string {
  return `${record.modelCode ?? ''}|${record.productCode}`
}

/**
 * 期间成本
 * @param {BomMaterialCostItemTransposed} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodCost(record: BomMaterialCostItemTransposed, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodCosts?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return formatCost(value)
}

/**
 * 构建转置查询（数据源 TaktBomMaterialCost）
 * @returns 查询或 null
 */
function buildQuery() {
  const plant = plantCode.value?.trim()
  if (!plant || !periodRange.value?.[0]) {
    return null
  }
  const rangeEnd = periodRange.value?.[1]?.trim() || periodRange.value[0]
  return {
    plantCode: plant,
    modelCode: modelCode.value?.trim() || undefined,
    productCode: productCode.value?.trim() || undefined,
    focusPeriod: rangeEnd,
    trendFilter: trendFilter.value || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToCostingDateQuery(periodRange.value),
  }
}

/** 清空 */
function clearData() {
  rows.value = []
  periodOrder.value = []
  periodCostTotals.value = {}
  varianceAmountTotal.value = null
  total.value = 0
}

/** 加载 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    clearData()
    return
  }
  loading.value = true
  try {
    const result = await getBomMaterialCostItemTransposedList(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    periodCostTotals.value = result.periodCostTotals ?? {}
    varianceAmountTotal.value = result.varianceAmountTotal ?? null
  } catch (error: unknown) {
    clearData()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

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
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 涨跌筛选 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 刷新 */
function handleRefresh() {
  void loadData()
}

/** 重置 */
async function handleReset() {
  plantCode.value = (await resolveCurrentCompanyRelatedPlantCode()) || undefined
  modelCode.value = undefined
  productCode.value = undefined
  periodRange.value = buildDefaultCostingPeriodRange(3)
  trendFilter.value = ''
  pageIndex.value = getTaktDefaultPageIndex()
  clearData()
}

/**
 * 分页
 * @param {number} page 页码
 * @param {number} size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  exportLoading.value = true
  try {
    const exportBase = buildBomExportBaseName('DTA BOM成本推移表', [
      query.plantCode,
      query.modelCode,
      query.productCode,
    ])
    const exportMeta = await exportBomMaterialCostItemTransposed(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      'DTA BOM成本推移表',
      buildBomExportFileName('DTA BOM成本推移表', [
        query.plantCode,
        query.modelCode,
        query.productCode,
      ]),
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: exportBase,
    })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t(`${localePrefix}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
  } finally {
    exportLoading.value = false
  }
}

function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - 47)
}

function startTableScrollObserve(): void {
  stopTableScrollObserve()
  const wrap = tableWrapRef.value
  if (!wrap) return
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => recalcTableScrollY())
  tableScrollResizeObserver.observe(wrap)
}

function stopTableScrollObserve(): void {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
}

watch(
  () => tenantStore.companyCode,
  () => {
    void handleReset()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  plantCode.value = (await resolveCurrentCompanyRelatedPlantCode()) || undefined
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})
</script>
