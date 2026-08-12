<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/variance-cost-trend/components -->
<!-- 文件名称：variance-cost-trend-panel.vue -->
<!-- 功能描述：差异成本推移表（有无差异组件×移动单价月度推移）；defineExpose reload/handleExport/clear -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-1 flex-col overflow-hidden">
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'componentCode'"
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
            <div class="inline-flex max-w-full flex-wrap items-center justify-end gap-1">
              <span :class="periodChangeTypeClass(getPeriodChangeType(record as BomVarianceCostTrend, String(column.key)))">
                {{ periodChangeTypeLabel(getPeriodChangeType(record as BomVarianceCostTrend, String(column.key))) }}
              </span>
              <span
                v-if="hasPeriodPrice(record as BomVarianceCostTrend, String(column.key))"
                class="text-text"
              >
                {{ formatPeriodPrice(record as BomVarianceCostTrend, String(column.key)) }}
              </span>
            </div>
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomVarianceCostTrend).trend || 'none')">
              {{ trendLabel((record as BomVarianceCostTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomVarianceCostTrend).varianceAmount)">
              {{ formatPrice((record as BomVarianceCostTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomVarianceCostTrend).varianceAmount)">
              {{ formatPercent((record as BomVarianceCostTrend).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div class="space-y-1">
            <div class="truncate">
              {{ summaryText }}
            </div>
            <div
              v-if="hasQuery && comparePeriod"
              class="text-text"
            >
              {{ t(`${localePrefix}.trendSummary`, {
                base: basePeriod || '—',
                compare: comparePeriod,
                newCount,
                removed: removedCount,
                version: versionCount,
              }) }}
            </div>
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
 * 差异成本推移面板：有无差异组件 × 移动单价月度推移
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import {
  exportBomVarianceCostTrendAnalysis,
  getBomVarianceCostTrendAnalysis,
} from '@/api/logistics/manufacturing/bom/variance-cost-trend'
import type { BomVarianceCostTrend } from '@/types/logistics/manufacturing/bom/variance-cost-trend'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/takt-paged'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bom-export-file-name'
import { periodRangeToMovingPricePeriodQuery } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-item-analysis'
import { useBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-analysis-master-context'

const props = defineProps<{
  /** 差异筛选：空=全部；new/removed/version */
  trendFilter?: string
  /** 全量排序：trend / varianceDesc / componentCode */
  sortBy?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.manufacturing.bom.variance-cost-trend.page'
const { t } = useI18n()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCodes,
  queryProductCodes,
  periodRange,
} = useBomMaterialCostAnalysisMasterContext()

const rows = ref<BomVarianceCostTrend[]>([])
const periodOrder = ref<string[]>([])
const productCodes = ref<string[]>([])
const componentCount = ref(0)
const basePeriod = ref('')
const comparePeriod = ref('')
const newCount = ref(0)
const removedCount = ref(0)
const versionCount = ref(0)
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

/** 已选机种 */
const selectedModelCodes = computed(() =>
  (queryModelCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean),
)

/** 已选产品（可选；空=机种下全部） */
const selectedProductCodes = computed(() =>
  (queryProductCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean),
)

const hasQuery = computed(
  () =>
    !!queryPlantCode.value?.trim()
    && selectedModelCodes.value.length > 0
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1],
)

const summaryText = computed(() => {
  if (!hasQuery.value) {
    return t(`${localePrefix}.selectModelRequired`)
  }
  return t(`${localePrefix}.summary`, {
    plant: queryPlantCode.value,
    model: selectedModelCodes.value.join(', '),
    productCount: productCodes.value.length,
    componentCount: componentCount.value,
  })
})

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
      title: t('entity.bommaterialcostitem.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 120,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.sequencecode'),
      dataIndex: 'sequenceCode',
      key: 'sequenceCode',
      width: 72,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.bomlevel'),
      dataIndex: 'bomLevel',
      key: 'bomLevel',
      width: 72,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.bomitemcode'),
      dataIndex: 'bomItemCode',
      key: 'bomItemCode',
      width: 88,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.previousComponentCode`),
      dataIndex: 'previousComponentCode',
      key: 'previousComponentCode',
      width: 140,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.componentcode'),
      dataIndex: 'componentCode',
      key: 'componentCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcostitem.componentdescription'),
      dataIndex: 'componentDescription',
      key: 'componentDescription',
      width: 160,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.componentquantity'),
      dataIndex: 'componentQuantity',
      key: 'componentQuantity',
      width: 96,
      align: 'right',
    },
    {
      title: t('entity.bommaterialcostitem.productionrelated'),
      dataIndex: 'productionRelated',
      key: 'productionRelated',
      width: 88,
    },
    {
      title: t('entity.bommaterialcostitem.purchasetype'),
      dataIndex: 'purchaseType',
      key: 'purchaseType',
      width: 80,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodMovingPrices', period],
      key: `period_${period}`,
      width: 132,
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
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 128,
      align: 'right',
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.variancePercent`),
      dataIndex: 'variancePercent',
      key: 'variancePercent',
      width: 100,
      align: 'right',
      fixed: 'right',
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {BomVarianceCostTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: BomVarianceCostTrend): string {
  return [
    record.plantCode,
    record.modelCode,
    record.productCode,
    record.sequenceCode,
    record.bomLevel,
    record.bomItemCode,
    record.previousComponentCode ?? '',
    record.componentCode,
    record.trend ?? ''].join('|')
}

/**
 * @param {number | null | undefined} value 移动单价
 * @returns {string} 文本（5 位小数，与成本口径一致）
 */
function formatPrice(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return Number(value).toFixed(5)
}

/**
 * @param {BomVarianceCostTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 移动单价
 */
function formatPeriodPrice(record: BomVarianceCostTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  return formatPrice(record.periodMovingPrices?.[period])
}

/**
 * @param {BomVarianceCostTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {boolean} 是否有移动单价
 */
function hasPeriodPrice(record: BomVarianceCostTrend, columnKey: string): boolean {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodMovingPrices?.[period]
  return value != null && !Number.isNaN(value)
}

/**
 * @param {BomVarianceCostTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 变动码
 */
function getPeriodChangeType(record: BomVarianceCostTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  return record.periodChangeTypes?.[period] || (hasPeriodPrice(record, columnKey) ? 'present' : 'absent')
}

/**
 * @param {string} type 变动码
 * @returns {string} 文案
 */
function periodChangeTypeLabel(type: string): string {
  return t(`${localePrefix}.periodChange.${type}`)
}

/**
 * @param {string} type 变动码
 * @returns {string} class
 */
function periodChangeTypeClass(type: string): string {
  if (type === 'new' || type === 'up') return 'text-red-500'
  if (type === 'removed' || type === 'down') return 'text-green-600'
  if (type === 'version') return 'text-primary font-medium'
  return 'text-text-secondary'
}

/**
 * @param {string} trend 涨跌
 * @returns {string} 文案
 */
function trendLabel(trend: string): string {
  return t(`${localePrefix}.trend.${trend}`)
}

/**
 * @param {string} trend 涨跌
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up' || trend === 'new') return 'text-red-500 font-medium'
  if (trend === 'down' || trend === 'removed') return 'text-green-600 font-medium'
  if (trend === 'version') return 'text-primary font-medium'
  return 'text-text-secondary'
}

/**
 * @param {number | null | undefined} value 差额
 * @returns {string} class
 */
function varianceClass(value?: number | null): string {
  if (value == null || value === 0) return 'text-text-secondary'
  return value > 0 ? 'text-red-500' : 'text-green-600'
}

/**
 * @param {number | null | undefined} value 百分点
 * @returns {string} 文本
 */
function formatPercent(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return `${value.toFixed(2)}%`
}

/**
 * 构建查询
 * @param {string} [trendFilterOverride] 筛选覆盖
 * @param {string} [sortByOverride] 全量排序覆盖
 * @returns 查询或 null
 */
function buildQuery(trendFilterOverride?: string, sortByOverride?: string) {
  const plantCode = queryPlantCode.value?.trim()
  const materialType = queryMaterialType.value?.trim()
  const models = selectedModelCodes.value
  if (!plantCode || !materialType) {
    return null
  }
  const rangeEnd = periodRange.value?.[1]?.trim() || periodRange.value?.[0]?.trim()
  const filter =
    trendFilterOverride !== undefined ? trendFilterOverride : props.trendFilter
  const sort =
    (sortByOverride !== undefined ? sortByOverride : props.sortBy)?.trim() || 'trend'
  const products = selectedProductCodes.value
  return {
    plantCode,
    // 空=后端按对比月全机种（避免多选机种 GET 查询串截断）
    modelCodes: models.length > 0 ? models.join(',') : undefined,
    materialType,
    productCodes: products.length > 0 ? products.join(',') : undefined,
    focusPeriod: rangeEnd || undefined,
    trendFilter: filter || undefined,
    sortBy: sort,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToMovingPricePeriodQuery(periodRange.value),
  }
}

/**
 * 加载分析
 * @param {string} [trendFilterOverride] 筛选覆盖
 * @param {string} [sortByOverride] 全量排序覆盖
 * @returns {Promise<void>}
 */
async function loadData(trendFilterOverride?: string, sortByOverride?: string): Promise<void> {
  const query = buildQuery(trendFilterOverride, sortByOverride)
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    await ensureTaktPaginationConfigAsync()
    const result = await getBomVarianceCostTrendAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    productCodes.value = result.productCodes ?? []
    componentCount.value = result.componentCount ?? 0
    basePeriod.value = result.basePeriod || ''
    comparePeriod.value = result.comparePeriod || ''
    newCount.value = result.newCount ?? 0
    removedCount.value = result.removedCount ?? 0
    versionCount.value = result.versionCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch (e) {
    clear()
    message.error(t(`${localePrefix}.queryFailed`))
    console.error(e)
  } finally {
    loading.value = false
  }
}

/**
 * 重新加载（重置页码）
 * @param {string} [trendFilterOverride] 筛选覆盖
 * @param {string} [sortByOverride] 全量排序覆盖
 * @returns {Promise<void>}
 */
async function reload(trendFilterOverride?: string, sortByOverride?: string): Promise<void> {
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData(trendFilterOverride, sortByOverride)
}

/** 清空 */
function clear() {
  rows.value = []
  total.value = 0
  periodOrder.value = []
  productCodes.value = []
  componentCount.value = 0
  basePeriod.value = ''
  comparePeriod.value = ''
  newCount.value = 0
  removedCount.value = 0
  versionCount.value = 0
  hasRows.value = false
}

/**
 * 分页
 * @param {number} page 页码
 * @param {number} size 页大小
 */
function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  void loadData()
}

/** 导出 */
async function handleExport(): Promise<void> {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectModelRequired`))
    return
  }
  try {
    const sheetTitle = 'DTA BOM组件差异成本推移表'
    const modelLabel = selectedModelCodes.value.join('_')
    const exportBase = buildBomExportBaseName(sheetTitle, [
      query.plantCode,
      modelLabel])
    const exportMeta = await exportBomVarianceCostTrendAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      sheetTitle,
      buildBomExportFileName(sheetTitle, [query.plantCode, modelLabel]),
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
    window.URL.revokeObjectURL(url)
    message.success(t(`${localePrefix}.exportSuccess`))
  } catch (e) {
    message.error(t(`${localePrefix}.exportFailed`))
    console.error(e)
  }
}

function measureTableScrollY() {
  const el = tableWrapRef.value
  if (!el) return
  const h = Math.floor(el.clientHeight)
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, h - 8)
}

onMounted(() => {
  measureTableScrollY()
  if (typeof ResizeObserver !== 'undefined' && tableWrapRef.value) {
    tableScrollResizeObserver = new ResizeObserver(() => measureTableScrollY())
    tableScrollResizeObserver.observe(tableWrapRef.value)
  }
})

onBeforeUnmount(() => {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
})

defineExpose({
  reload,
  handleExport,
  clear,
})
</script>
