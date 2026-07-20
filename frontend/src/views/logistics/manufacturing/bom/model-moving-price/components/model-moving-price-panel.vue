<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-moving-price/components -->
<!-- 文件名称：model-moving-price-panel.vue -->
<!-- 功能描述：TaktBomMaterialCostItem 展开行按合并键×期间转置涨跌明细（机种/产品推移共用） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-1 flex-col overflow-hidden">
    <div
      ref="tableWrapRef"
      class="min-h-0 flex-1 overflow-hidden"
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
        :scroll="{ y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodPrice(record as BomMaterialCostItemModelMovingPrice, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'currency'">
            <TaktDictTag
              :value="(record as BomMaterialCostItemModelMovingPrice).currency"
              dict-type="accounting_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemModelMovingPrice).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemModelMovingPrice).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemModelMovingPrice).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemModelMovingPrice).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemModelMovingPrice).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemModelMovingPrice).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template
          v-if="total > 0"
          #summary
        >
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
                :align="cell.align"
              >
                <span :class="cell.className">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
        <template #footerRemark>
          <div class="space-y-1">
            <div class="truncate">
              {{ summaryText }}
            </div>
            <div
              v-if="hasQuery && modelComparePeriod"
              class="text-text"
            >
              {{ t(`${localePrefix}.modelTrendSummary`, {
                base: modelBasePeriod || '—',
                compare: modelComparePeriod,
                cost: formatCost(modelFocusCost),
                trend: trendLabel(modelTrend || 'none'),
                variance: formatCost(modelVarianceAmount),
                percent: formatPercent(modelVariancePercent),
              }) }}
            </div>
            <div
              v-if="hasQuery && comparePeriod"
              class="text-text"
            >
              {{ t(`${localePrefix}.trendSummary`, {
                base: basePeriod || '—',
                compare: comparePeriod,
                up: upCount,
                down: downCount,
                flat: flatCount,
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
 * 机种成本推移：合并键 × 月材料成本转置表
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { resolveTableSummaryLabelColumnKey } from '@/utils/table-columns'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import {
  exportBomMaterialCostItemModelMovingPriceAnalysis,
  getBomMaterialCostItemModelMovingPriceAnalysis,
} from '@/api/logistics/manufacturing/bom/material-cost-item'
import type { BomMaterialCostItemModelMovingPrice } from '@/types/logistics/manufacturing/bom/material-cost-trend'
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
import {
  bomMomTrendSortRank,
  compareBomMomNullableNumber,
  periodRangeToMovingPricePeriodQuery,
} from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-item-analysis'
import { useBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/use-material-cost-analysis-master-context'

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
  /** 是否必须选择产品（BOM 成本推移页） */
  requireProduct?: boolean
  /** 静态 locales 前缀；默认机种成本推移 */
  localePrefix?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

/** 静态 locales 前缀 */
const localePrefix = computed(
  () => props.localePrefix || 'logistics.manufacturing.bom.model-moving-price.page',
)
const { t } = useI18n()
const { queryPlantCode, queryModelCode, queryProductCode, periodRange } = useBomMaterialCostAnalysisMasterContext()

/** 分析行 */
const rows = ref<BomMaterialCostItemModelMovingPrice[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 产品组 */
const productCodes = ref<string[]>([])
/** 分析行总数 */
const componentCount = ref(0)
/** 机种各月材料成本 */
const modelPeriodMaterialCosts = ref<Record<string, number>>({})
/** 机种环比 */
const modelTrend = ref('none')
const modelBasePeriod = ref('')
const modelComparePeriod = ref('')
const modelVarianceAmount = ref<number | null>(null)
const modelVariancePercent = ref<number | null>(null)
/** 分析行环比 */
const basePeriod = ref('')
const comparePeriod = ref('')
const upCount = ref(0)
const downCount = ref(0)
const flatCount = ref(0)
/** 分页 */
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
/** 表体外壳 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 表体 scroll.y */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** ResizeObserver */
let tableScrollResizeObserver: ResizeObserver | null = null

/** 是否已填查询条件（工厂即可；机种/产品可选） */
const hasQuery = computed(() => !!queryPlantCode.value?.trim())

/** 摘要 */
const summaryText = computed(() => {
  if (!hasQuery.value) {
    return t(`${localePrefix.value}.selectPlantRequired`)
  }
  return t(`${localePrefix.value}.summary`, {
    plant: queryPlantCode.value,
    model: queryModelCode.value?.trim() || '—',
    product: queryProductCode.value?.trim() || '—',
    productCount: productCodes.value.length,
    componentCount: componentCount.value,
  })
})

/** 机种关注月成本 */
const modelFocusCost = computed(() => {
  const period = modelComparePeriod.value || comparePeriod.value
  if (!period) return null
  const value = modelPeriodMaterialCosts.value[period]
  return value == null ? null : value
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcostitem.componentcode'),
      dataIndex: 'componentCode',
      key: 'componentCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.modeldestination.modelname'),
      dataIndex: 'modelName',
      key: 'modelName',
      width: 160,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix.value}.productCodes`),
      dataIndex: 'productCodes',
      key: 'productCodes',
      width: 120,
      ellipsis: true,
    },
    {
      title: t('entity.bommaterialcostitem.componentdescription'),
      dataIndex: 'componentDescription',
      key: 'componentDescription',
      width: 160,
      ellipsis: true,
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
    },
    {
      title: t(`${localePrefix.value}.productCount`),
      dataIndex: 'productCount',
      key: 'productCount',
      width: 80,
      align: 'right',
    },
    {
      title: t('entity.bommaterialcostitem.movingpricecurrency'),
      dataIndex: 'currency',
      key: 'currency',
      width: 80,
    },
  ]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodMaterialCosts', period],
      key: `period_${period}`,
      width: 120,
      align: 'right',
    })
  }
  cols.push({
    title: t(`${localePrefix.value}.columns.trend`),
    dataIndex: 'trend',
    key: 'trend',
    width: 88,
    fixed: 'right',
    sorter: (a: BomMaterialCostItemModelMovingPrice, b: BomMaterialCostItemModelMovingPrice) =>
      bomMomTrendSortRank(a.trend) - bomMomTrendSortRank(b.trend),
  })
  cols.push({
    title: t(`${localePrefix.value}.columns.varianceAmount`),
    dataIndex: 'varianceAmount',
    key: 'varianceAmount',
    width: 120,
    align: 'right',
    fixed: 'right',
    sorter: (a: BomMaterialCostItemModelMovingPrice, b: BomMaterialCostItemModelMovingPrice) =>
      compareBomMomNullableNumber(a.varianceAmount, b.varianceAmount),
  })
  cols.push({
    title: t(`${localePrefix.value}.columns.variancePercent`),
    dataIndex: 'variancePercent',
    key: 'variancePercent',
    width: 100,
    align: 'right',
    fixed: 'right',
    sorter: (a: BomMaterialCostItemModelMovingPrice, b: BomMaterialCostItemModelMovingPrice) =>
      compareBomMomNullableNumber(a.variancePercent, b.variancePercent),
  })
  return cols
})

/** 可见列键 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/** 合计行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 全量期间成本合计（API：分页前、已应用涨跌筛选） */
const periodCostTotals = ref<Record<string, number>>({})
/** 全量环比差额合计（API：分页前、已应用涨跌筛选） */
const varianceAmountTotal = ref<number | null>(null)

/**
 * 合计文案所在列：跳过序号，取第一个业务数据列
 * @returns {string | undefined} 列 key
 */
const summaryLabelColumnKey = computed(() => resolveTableSummaryLabelColumnKey(columns.value))

/** 表尾合计单元格（全量周期成本 + 环比差额；文案在第一个业务数据列，无行选择偏移） */
const summaryCells = computed(() => {
  const labelKey = summaryLabelColumnKey.value
  return columns.value.map((col, index) => {
    const key = String(col.key ?? index)
    const align = (col.align as 'left' | 'right' | 'center' | undefined) ?? 'left'
    if (labelKey && key === labelKey) {
      return { key, index, align, text: summaryLabel.value, className: 'font-medium text-text' }
    }
    if (key.startsWith('period_')) {
      const period = key.replace(/^period_/, '')
      const totalVal = periodCostTotals.value[period]
      return {
        key,
        index,
        align: 'right' as const,
        text: totalVal == null ? '' : formatCost(totalVal),
        className: 'font-medium text-text',
      }
    }
    if (key === 'varianceAmount') {
      return {
        key,
        index,
        align: 'right' as const,
        text: formatCost(varianceAmountTotal.value),
        className: `font-medium ${varianceClass(varianceAmountTotal.value)}`.trim(),
      }
    }
    return { key, index, align, text: '', className: '' }
  })
})

/**
 * 行主键
 * @param {BomMaterialCostItemModelMovingPrice} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemModelMovingPrice): string {
  return [
    record.plantCode,
    record.componentCode,
    record.productionRelated ?? '',
    record.purchaseType,
  ].join('|')
}

/**
 * 格式化金额
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatCost(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 格式化百分比
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatPercent(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return `${value.toFixed(2)}%`
}

/**
 * 涨跌文案
 * @param {string} trend 趋势码
 * @returns {string} 文本
 */
function trendLabel(trend: string): string {
  const key = `${localePrefix.value}.trend.${trend}`
  const text = t(key)
  return text === key ? trend : text
}

/**
 * 涨跌样式
 * @param {string} trend 趋势码
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up') return 'text-red-600 font-medium'
  if (trend === 'down') return 'text-green-600 font-medium'
  return ''
}

/**
 * 差额样式
 * @param {number | null | undefined} value 差额
 * @returns {string} class
 */
function varianceClass(value?: number | null): string {
  if (value == null) return ''
  if (value > 0) return 'text-red-600'
  if (value < 0) return 'text-green-600'
  return ''
}

/**
 * 期间键
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} yyyy-MM
 */
function resolvePeriodKey(columnKey: string): string {
  return columnKey.replace(/^period_/, '')
}

/**
 * 格式化期间材料成本
 * @param {BomMaterialCostItemModelMovingPrice} record 行
 * @param {string} columnKey 列键
 * @returns {string} 文本
 */
function formatPeriodPrice(record: BomMaterialCostItemModelMovingPrice, columnKey: string): string {
  const period = resolvePeriodKey(columnKey)
  const value = record.periodMaterialCosts?.[period] ?? record.periodUnitPrices?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 构建查询
 * @returns 查询 DTO 或 null
 */
function buildQuery() {
  const plantCode = queryPlantCode.value?.trim()
  if (!plantCode) {
    return null
  }
  const modelCode = queryModelCode.value?.trim()
  const product = queryProductCode.value?.trim()
  const rangeEnd = periodRange.value?.[1]?.trim()
  return {
    plantCode,
    modelCode: modelCode || undefined,
    productCode: product || undefined,
    focusPeriod: rangeEnd || undefined,
    trendFilter: props.trendFilter || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToMovingPricePeriodQuery(periodRange.value),
  }
}

/** 清空 */
function clear() {
  rows.value = []
  periodOrder.value = []
  periodCostTotals.value = {}
  varianceAmountTotal.value = null
  productCodes.value = []
  componentCount.value = 0
  modelPeriodMaterialCosts.value = {}
  modelTrend.value = 'none'
  modelBasePeriod.value = ''
  modelComparePeriod.value = ''
  modelVarianceAmount.value = null
  modelVariancePercent.value = null
  basePeriod.value = ''
  comparePeriod.value = ''
  upCount.value = 0
  downCount.value = 0
  flatCount.value = 0
  total.value = 0
  pageIndex.value = getTaktDefaultPageIndex()
  hasRows.value = false
}

/** 加载 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getBomMaterialCostItemModelMovingPriceAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    periodCostTotals.value = result.periodCostTotals ?? {}
    varianceAmountTotal.value = result.varianceAmountTotal ?? null
    productCodes.value = result.productCodes ?? []
    componentCount.value = result.componentCount ?? 0
    modelPeriodMaterialCosts.value = result.modelPeriodMaterialCosts ?? {}
    modelTrend.value = result.modelTrend ?? 'none'
    modelBasePeriod.value = result.modelBasePeriod ?? ''
    modelComparePeriod.value = result.modelComparePeriod ?? ''
    modelVarianceAmount.value = result.modelVarianceAmount ?? null
    modelVariancePercent.value = result.modelVariancePercent ?? null
    basePeriod.value = result.basePeriod ?? ''
    comparePeriod.value = result.comparePeriod ?? ''
    upCount.value = result.upCount ?? 0
    downCount.value = result.downCount ?? 0
    flatCount.value = result.flatCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch {
    clear()
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/** 重新加载（重置页码） */
async function reload() {
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
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
    message.warning(t(`${localePrefix.value}.selectPlantRequired`))
    return
  }
  try {
    const exportBase = buildBomExportBaseName('DTA 机种成本推移表', [
      query.plantCode,
      query.modelCode,
      query.productCode,
    ])
    const exportMeta = await exportBomMaterialCostItemModelMovingPriceAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      'DTA 机种成本推移表',
      buildBomExportFileName('DTA 机种成本推移表', [
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
    message.success(t(`${localePrefix.value}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix.value}.exportFailed`))
  }
}

/** 实测 scroll.y（扣除表头 + 合计行） */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  const summaryH = total.value > 0 ? 39 : 0
  const y = Math.floor(wrap.clientHeight - 47 - summaryH)
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, y)
}

/** 监听外壳 */
function startTableScrollObserve(): void {
  stopTableScrollObserve()
  const wrap = tableWrapRef.value
  if (!wrap) return
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => {
    recalcTableScrollY()
  })
  tableScrollResizeObserver.observe(wrap)
}

/** 停止监听 */
function stopTableScrollObserve(): void {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>
