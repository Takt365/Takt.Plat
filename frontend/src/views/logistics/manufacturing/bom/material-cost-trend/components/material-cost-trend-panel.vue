<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：material-cost-trend-panel.vue -->
<!-- 功能描述：产品成本推移报表：单个产品下明细组件×月材料成本转置涨跌 -->
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
        table-mode="single"
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
              <span
                v-if="isPresenceChangeType(getPeriodChangeType(record as BomMaterialCostItemComponentMovingPrice, String(column.key)))"
                :class="periodChangeTypeClass(getPeriodChangeType(record as BomMaterialCostItemComponentMovingPrice, String(column.key)))"
              >
                {{ periodChangeTypeLabel(getPeriodChangeType(record as BomMaterialCostItemComponentMovingPrice, String(column.key))) }}
              </span>
              <span
                v-if="hasPeriodCost(record as BomMaterialCostItemComponentMovingPrice, String(column.key))"
                :class="periodChangeTypeClass(getPeriodChangeType(record as BomMaterialCostItemComponentMovingPrice, String(column.key)))"
              >
                {{ formatPeriodCost(record as BomMaterialCostItemComponentMovingPrice, String(column.key)) }}
              </span>
              <span
                v-else-if="getPeriodChangeType(record as BomMaterialCostItemComponentMovingPrice, String(column.key)) === 'removed'"
                :class="periodChangeTypeClass('removed')"
              >
                —
              </span>
              <span
                v-else
                class="text-text-secondary"
              >
                —
              </span>
            </div>
          </template>
          <template v-else-if="column.key === 'currency'">
            <TaktDictTag
              :value="(record as BomMaterialCostItemComponentMovingPrice).currency"
              dict-type="accounting_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemComponentMovingPrice).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemComponentMovingPrice).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemComponentMovingPrice).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemComponentMovingPrice).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemComponentMovingPrice).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemComponentMovingPrice).variancePercent) }}
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
                up: upCount,
                down: downCount,
                flat: flatCount,
                added: newCount,
                removed: removedCount,
              }) }}
            </div>
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
 * 产品成本推移：单个产品下明细表组件行转置
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import {
  exportBomMaterialCostItemComponentMovingPriceAnalysis,
  getBomMaterialCostItemComponentMovingPriceAnalysis,
} from '@/api/logistics/manufacturing/bom/material-cost-item'
import type { BomMaterialCostItemComponentMovingPrice } from '@/types/logistics/manufacturing/bom/material-cost-trend'
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
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  bomMomTrendSortRank,
  compareBomMomNullableNumber,
  periodRangeToMovingPricePeriodQuery,
  useMaterialCostAnalysis,
} from '../composables/use-material-cost-item-analysis'
import { useBomMaterialCostAnalysisMasterContext } from '../composables/use-material-cost-analysis-master-context'

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const { t } = useI18n()
const { formatCost, formatPercent, trendLabel, trendClass, varianceClass, periodChangeTypeLabel, periodChangeTypeClass } = useMaterialCostAnalysis()
const { queryPlantCode, queryModelCode, queryProductCode, periodRange } =
  useBomMaterialCostAnalysisMasterContext()

/** 明细组件行 */
const rows = ref<BomMaterialCostItemComponentMovingPrice[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 明细行总数 */
const componentCount = ref(0)
/** 环比 */
const basePeriod = ref('')
const comparePeriod = ref('')
const upCount = ref(0)
const downCount = ref(0)
const flatCount = ref(0)
const newCount = ref(0)
const removedCount = ref(0)
/** 分页 */
const pageIndex = ref(1)
const pageSize = ref(20)
const total = ref(0)
/** 表体 */
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

/** 工厂 + 产品必填 */
const hasQuery = computed(
  () => !!queryPlantCode.value?.trim() && !!queryProductCode.value?.trim(),
)

/** 摘要 */
const summaryText = computed(() => {
  if (!queryPlantCode.value?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  if (!queryProductCode.value?.trim()) {
    return t(`${localePrefix}.selectProductRequired`)
  }
  return t(`${localePrefix}.summary`, {
    plant: queryPlantCode.value,
    model: queryModelCode.value?.trim() || '—',
    product: queryProductCode.value,
    componentCount: componentCount.value,
  })
})

/** 列：BOM 明细行 + 周期 + 涨跌/环比 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcostitem.sequenceno'),
      dataIndex: 'sequenceNo',
      key: 'sequenceNo',
      width: 72,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcostitem.bomlevel'),
      dataIndex: 'bomLevel',
      key: 'bomLevel',
      width: 72,
    },
    {
      title: t('entity.bommaterialcostitem.bomitemno'),
      dataIndex: 'bomItemNo',
      key: 'bomItemNo',
      width: 88,
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
      width: 180,
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
  cols.push(
    {
      title: t(`${localePrefix}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 88,
      fixed: 'right',
      sorter: (a: BomMaterialCostItemComponentMovingPrice, b: BomMaterialCostItemComponentMovingPrice) =>
        bomMomTrendSortRank(a.trend) - bomMomTrendSortRank(b.trend),
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 120,
      align: 'right',
      fixed: 'right',
      sorter: (a: BomMaterialCostItemComponentMovingPrice, b: BomMaterialCostItemComponentMovingPrice) =>
        compareBomMomNullableNumber(a.varianceAmount, b.varianceAmount),
    },
    {
      title: t(`${localePrefix}.columns.variancePercent`),
      dataIndex: 'variancePercent',
      key: 'variancePercent',
      width: 100,
      align: 'right',
      fixed: 'right',
      sorter: (a: BomMaterialCostItemComponentMovingPrice, b: BomMaterialCostItemComponentMovingPrice) =>
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
 * @param {BomMaterialCostItemComponentMovingPrice} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemComponentMovingPrice): string {
  return [
    record.plantCode,
    record.productCode,
    record.sequenceNo ?? '',
    record.bomLevel ?? '',
    record.bomItemNo ?? '',
    record.componentCode,
    String(record.componentQuantity ?? ''),
    record.productionRelated ?? '',
    record.purchaseType,
  ].join('|')
}

/**
 * 期间成本
 * @param {BomMaterialCostItemComponentMovingPrice} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodCost(record: BomMaterialCostItemComponentMovingPrice, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodMaterialCosts?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return formatCost(value)
}

/**
 * 期间变动码
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns present / absent / new / removed / up / down / flat
 */
function getPeriodChangeType(
  record: BomMaterialCostItemComponentMovingPrice,
  columnKey: string,
): string {
  const period = columnKey.replace(/^period_/, '')
  return record.periodChangeTypes?.[period] || (hasPeriodCost(record, columnKey) ? 'present' : 'absent')
}

/**
 * 是否有该月成本
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns 是否有值
 */
function hasPeriodCost(record: BomMaterialCostItemComponentMovingPrice, columnKey: string): boolean {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodMaterialCosts?.[period]
  return value != null && !Number.isNaN(value)
}

/**
 * 是否为「有无物料」类标识（新增/剔除优先于价格）
 * @param changeType 变动码
 * @returns 是否展示存在标识
 */
function isPresenceChangeType(changeType: string): boolean {
  return changeType === 'new' || changeType === 'removed'
}

/**
 * 构建查询（单个产品，不按机种合并）
 * @returns 查询或 null
 */
function buildQuery() {
  const plantCode = queryPlantCode.value?.trim()
  const productCode = queryProductCode.value?.trim()
  if (!plantCode || !productCode) {
    return null
  }
  const rangeEnd = periodRange.value?.[1]?.trim() || periodRange.value?.[0]?.trim()
  return {
    plantCode,
    modelCode: queryModelCode.value?.trim() || '',
    productCode,
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
  componentCount.value = 0
  basePeriod.value = ''
  comparePeriod.value = ''
  upCount.value = 0
  downCount.value = 0
  flatCount.value = 0
  newCount.value = 0
  removedCount.value = 0
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
    const result = await getBomMaterialCostItemComponentMovingPriceAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    periodCostTotals.value = result.periodCostTotals ?? {}
    varianceAmountTotal.value = result.varianceAmountTotal ?? null
    componentCount.value = result.componentCount ?? rows.value.length
    basePeriod.value = result.basePeriod ?? ''
    comparePeriod.value = result.comparePeriod ?? ''
    upCount.value = result.upCount ?? 0
    downCount.value = result.downCount ?? 0
    flatCount.value = result.flatCount ?? 0
    newCount.value = result.newCount ?? 0
    removedCount.value = result.removedCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/** 重新加载 */
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
    message.warning(t(`${localePrefix}.selectProductRequired`))
    return
  }
  try {
    const exportBase = buildBomExportBaseName('DTA 产品成本推移表', [
      query.plantCode,
      query.modelCode,
      query.productCode,
    ])
    const exportMeta = await exportBomMaterialCostItemComponentMovingPriceAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      'DTA 产品成本推移表',
      buildBomExportFileName('DTA 产品成本推移表', [
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

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>
