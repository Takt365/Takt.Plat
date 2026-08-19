<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/price-delta-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本差异推移独立页（月成本 + 0价格组 + 价格差异组） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <price-delta-trend-query-form
      v-model:plant-code="plantCode"
      v-model:material-type="materialType"
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
      export-permission="logistics:manufacturing:bom:pricedelta:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <div
      v-if="comparePeriod"
      class="mb-2 text-sm text-text-secondary"
    >
      {{ t(`${localePrefix}.compareHint`, { base: basePeriod || '—', compare: comparePeriod }) }}
    </div>
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
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
        :virtual="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodCost(record as BomPriceDeltaTrend, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'priceDelta'">
            {{ formatPriceDelta(record as BomPriceDeltaTrend) }}
          </template>
          <template
            v-else-if="
              column.key === 'zeroPriceGroup'
                || column.key === 'priceDeltaTrend'
                || column.key === 'componentDeltaGroup'
                || column.key === 'productDescription'
            "
          >
            <a-tooltip
              v-if="String(text ?? '').trim()"
              placement="topLeft"
              :overlay-style="longTextTooltipStyle"
            >
              <template #title>
                <div class="max-h-80 max-w-md overflow-auto whitespace-pre-wrap break-all text-xs">
                  {{ text }}
                </div>
              </template>
              <span class="block max-w-full truncate">{{ text }}</span>
            </a-tooltip>
            <span v-else>—</span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      :disabled="loading"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 成本差异推移：产品月成本 + 0价格组 + PriceDeltaTrend
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportBomPriceDeltaTrendData,
  getBomPriceDeltaTrendList,
  getBomPriceDeltaTrendPlantOptions,
} from '@/api/logistics/manufacturing/bom/price-delta-trend'
import type { BomPriceDeltaTrend } from '@/types/logistics/manufacturing/bom/price-delta-trend'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/takt-paged'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTableRefresh } from '@/composables/use-table-refresh'
import {
  buildDefaultCostingPeriodRange,
  periodRangeToCostingDateQuery,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-material-cost-period'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-export-file-name'
import PriceDeltaTrendQueryForm from '@/views/logistics/manufacturing/bom/price-delta-trend/components/price-delta-trend-query-form.vue'

/** 长文本 Tooltip 样式（限制悬停层尺寸，避免整页撑爆） */
const longTextTooltipStyle = {
  maxWidth: '480px',
} as const

const localePrefix = 'logistics.manufacturing.bom.price-delta-trend.page'
const { t } = useI18n()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 物料类型（默认 FERT） */
const materialType = ref<string | undefined>()
/** 机种（可选） */
const modelCode = ref<string | undefined>()
/** 产品（可选） */
const productCode = ref<string | undefined>()
/** 核算期间 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
/** 行 */
const rows = ref<BomPriceDeltaTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
const basePeriod = ref('')
const comparePeriod = ref('')
const loading = ref(false)
const exportLoading = ref(false)
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

const hasQuery = computed(
  () =>
    !!plantCode.value?.trim()
    && !!materialType.value?.trim()
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1],
)

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
      width: 200,
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
      title: t(`${localePrefix}.priceDelta`),
      dataIndex: 'priceDelta',
      key: 'priceDelta',
      width: 100,
      align: 'right',
    },
    {
      title: t(`${localePrefix}.zeroPriceGroup`),
      dataIndex: 'zeroPriceGroup',
      key: 'zeroPriceGroup',
      width: 200,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.priceDeltaTrend`),
      dataIndex: 'priceDeltaTrend',
      key: 'priceDeltaTrend',
      width: 220,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.componentDeltaGroup`),
      dataIndex: 'componentDeltaGroup',
      key: 'componentDeltaGroup',
      width: 220,
      ellipsis: true,
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param record 行
 * @returns key
 */
function getRowKey(record: BomPriceDeltaTrend): string {
  return `${record.plantCode}|${record.modelCode}|${record.productCode}`
}

/**
 * 格式化期间成本
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns 文本
 */
function formatPeriodCost(record: BomPriceDeltaTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodCosts?.[period]
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '—'
  }
  return Number(value).toFixed(5)
}

/**
 * 格式化差异（期间最大月 − 前一月）
 * @param record 行
 * @returns 文本
 */
function formatPriceDelta(record: BomPriceDeltaTrend): string {
  const value = record.priceDelta
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '—'
  }
  return Number(value).toFixed(5)
}

/** 构建查询 */
function buildQuery(overrides?: Record<string, unknown>) {
  const plant = plantCode.value?.trim() ?? ''
  const dates = periodRangeToCostingDateQuery(periodRange.value)
  const focus = periodRange.value?.[1]
  return {
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    plantCode: plant,
    materialType: materialType.value?.trim() || undefined,
    modelCode: modelCode.value?.trim() || undefined,
    productCode: productCode.value?.trim() || undefined,
    costingDateStart: dates.costingDateStart,
    costingDateEnd: dates.costingDateEnd,
    focusPeriod: focus,
    ...overrides,
  }
}

/** 加载 */
async function loadData() {
  if (!hasQuery.value) {
    rows.value = []
    total.value = 0
    periodOrder.value = []
    return
  }
  loading.value = true
  try {
    const res = await getBomPriceDeltaTrendList(buildQuery())
    rows.value = res.paged?.data ?? []
    total.value = res.paged?.total ?? 0
    periodOrder.value = res.periodOrder ?? []
    basePeriod.value = res.basePeriod ?? ''
    comparePeriod.value = res.comparePeriod ?? ''
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
    rows.value = []
    total.value = 0
  }     finally {
    loading.value = false
  }
}

useTableRefresh(loadData)

/** 查询 */
async function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!materialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
}

/** 重置 */
async function handleReset() {
  materialType.value = undefined
  modelCode.value = undefined
  productCode.value = undefined
  periodRange.value = buildDefaultCostingPeriodRange(3)
  pageIndex.value = getTaktDefaultPageIndex()
  rows.value = []
  total.value = 0
  periodOrder.value = []
  basePeriod.value = ''
  comparePeriod.value = ''
  await applyDefaultPlant()
}

/**
 * 默认工厂：公司关联工厂须落在本页 plant-options 中
 */
async function applyDefaultPlant() {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  try {
    const plants = await getBomPriceDeltaTrendPlantOptions()
    const values = (plants ?? [])
      .map((p) => String(p.dictValue ?? '').trim())
      .filter(Boolean)
    if (related && values.some((v) => v.toUpperCase() === related.toUpperCase())) {
      plantCode.value = related
      return
    }
    plantCode.value = values[0]
  } catch {
    plantCode.value = related || undefined
  }
}

/** 刷新 */
async function handleRefresh() {
  await loadData()
}

/**
 * 分页
 * @param page 页码
 * @param size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 导出 */
async function handleExport() {
  if (!hasQuery.value) {
    return
  }
  exportLoading.value = true
  try {
    const query = buildQuery()
    const plant = plantCode.value?.trim() ?? ''
    const sheetTitle = 'DTA BOM成本差异推移'
    const exportBase = buildBomExportBaseName(sheetTitle, [plant])
    const exportMeta = await exportBomPriceDeltaTrendData(
      query,
      sheetTitle,
      buildBomExportFileName(sheetTitle, [plant]),
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

function bindTableScroll() {
  tableScrollResizeObserver?.disconnect()
  const el = tableWrapRef.value
  if (!el) {
    return
  }
  const update = () => {
    tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, el.clientHeight - 8)
  }
  update()
  tableScrollResizeObserver = new ResizeObserver(update)
  tableScrollResizeObserver.observe(el)
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  bindTableScroll()
  await applyDefaultPlant()
})

onBeforeUnmount(() => {
  tableScrollResizeObserver?.disconnect()
})
</script>
