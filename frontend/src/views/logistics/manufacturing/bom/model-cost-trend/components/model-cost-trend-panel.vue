<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend/components -->
<!-- 文件名称：model-cost-trend-panel.vue -->
<!-- 功能描述：TaktBomMaterialCostItem 展开行按 summary/detail 合并键×核算月转置涨跌明细 -->
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
        :id-column-key="'modelCode'"
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
            <div
              v-if="isDetailMode"
              class="inline-flex max-w-full flex-wrap items-center justify-end gap-1"
            >
              <span
                :class="periodChangeTypeClass(getPeriodChangeType(record as BomMaterialCostItemModelCostTrend, String(column.key)))"
              >
                {{ periodChangeTypeLabel(getPeriodChangeType(record as BomMaterialCostItemModelCostTrend, String(column.key))) }}
              </span>
              <span
                v-if="hasPeriodCost(record as BomMaterialCostItemModelCostTrend, String(column.key))"
                class="text-text"
              >
                {{ formatPeriodPrice(record as BomMaterialCostItemModelCostTrend, String(column.key)) }}
              </span>
            </div>
            <template v-else>
              {{ formatPeriodPrice(record as BomMaterialCostItemModelCostTrend, String(column.key)) }}
            </template>
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemModelCostTrend).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemModelCostTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemModelCostTrend).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemModelCostTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemModelCostTrend).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemModelCostTrend).variancePercent) }}
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
              v-if="hasQuery && isDetailMode && modelComparePeriod"
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
 * 机种成本推移：合并键 × 月材料成本转置表
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import {
  exportBomMaterialCostItemModelCostTrendAnalysis,
  getBomMaterialCostItemModelCostTrendAnalysis,
} from '@/api/logistics/manufacturing/bom/model-cost-trend'
import type { BomMaterialCostItemModelCostTrend } from '@/types/logistics/manufacturing/bom/model-cost-trend'
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
  /** 涨跌筛选 */
  trendFilter?: string
  /** 全量排序：productCountDesc / productCountAsc / trend */
  sortBy?: string
  /** 合并模式：summary=粗合并；detail=差异组件键 */
  mergeMode?: 'summary' | 'detail' | string
  /** 是否必须选择产品（BOM 成本推移页） */
  requireProduct?: boolean
  /** 静态 locales 前缀；默认机种成本推移 */
  localePrefix?: string
}>()

/** 是否差异组件明细 Tab */
const isDetailMode = computed(() => String(props.mergeMode || 'summary').toLowerCase() === 'detail')

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

/** 静态 locales 前缀 */
const localePrefix = computed(
  () => props.localePrefix || 'logistics.manufacturing.bom.model-cost-trend.page',
)
const { t } = useI18n()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCodes,
  queryProductCode,
  queryComponentCodes,
  periodRange,
} = useBomMaterialCostAnalysisMasterContext()

/** 分析行 */
const rows = ref<BomMaterialCostItemModelCostTrend[]>([])
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

/** 是否已填查询条件（工厂即可；机种/物料可选） */
const hasQuery = computed(() => !!queryPlantCode.value?.trim())

/** 摘要 */
const summaryText = computed(() => {
  if (!hasQuery.value) {
    return t(`${localePrefix.value}.selectPlantRequired`)
  }
  const key = isDetailMode.value ? `${localePrefix.value}.summaryDetail` : `${localePrefix.value}.summary`
  const models = (queryModelCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean)
  const components = (queryComponentCodes.value ?? []).map((c) => String(c).trim()).filter(Boolean)
  return t(key, {
    plant: queryPlantCode.value,
    model: models.length ? models.join(',') : t(`${localePrefix.value}.modelAll`),
    component: components.length ? components.join(',') : t(`${localePrefix.value}.componentAll`),
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

/** 动态列：机种组 → 产品组 → 组件 → 组件描述 → 其余清单字段（不含机种名称） */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t(`${localePrefix.value}.modelGroup`),
      dataIndex: 'modelCode',
      key: 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t(`${localePrefix.value}.productCodes`),
      dataIndex: 'productCodes',
      key: 'productCodes',
      width: 280,
      ellipsis: true,
      fixed: 'left',
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
  cols.push({
    title: t(`${localePrefix.value}.productCount`),
    dataIndex: 'productCount',
    key: 'productCount',
    width: 80,
    align: 'right',
  })
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
  })
  cols.push({
    title: t(`${localePrefix.value}.columns.varianceAmount`),
    dataIndex: 'varianceAmount',
    key: 'varianceAmount',
    width: 120,
    align: 'right',
    fixed: 'right',
  })
  cols.push({
    title: t(`${localePrefix.value}.columns.variancePercent`),
    dataIndex: 'variancePercent',
    key: 'variancePercent',
    width: 100,
    align: 'right',
    fixed: 'right',
  })
  return cols
})

/** 可见列键 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/** 合计首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 全量期间成本合计（API：分页前、已应用涨跌筛选） */
const periodCostTotals = ref<Record<string, number>>({})
/** 全量环比差额合计（API：分页前、已应用涨跌筛选） */
const varianceAmountTotal = ref<number | null>(null)

/**
 * 行主键
 * @param {BomMaterialCostItemModelCostTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemModelCostTrend): string {
  return [
    record.plantCode,
    record.modelCode,
    record.componentCode,
    record.productionRelated ?? '',
    record.purchaseType].join('|')
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
 * 月度有无/变动文案
 * @param changeType present / absent / new / removed / up / down / flat
 * @returns 文本
 */
function periodChangeTypeLabel(changeType: string): string {
  const key = `${localePrefix.value}.periodChange.${changeType}`
  const text = t(key)
  return text === key ? changeType : text
}

/**
 * 月度有无样式
 * @param changeType 变动码
 * @returns class
 */
function periodChangeTypeClass(changeType: string): string {
  if (changeType === 'new') return 'text-blue-600 font-medium'
  if (changeType === 'removed') return 'text-orange-600 font-medium'
  if (changeType === 'up') return 'text-red-600'
  if (changeType === 'down') return 'text-green-600'
  if (changeType === 'absent') return 'text-text-secondary'
  return ''
}

/**
 * 涨跌样式
 * @param {string} trend 趋势码
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up' || trend === 'new') return 'text-red-600 font-medium'
  if (trend === 'down' || trend === 'removed') return 'text-green-600 font-medium'
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
 * 期间材料成本是否存在
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns 是否有成本
 */
function hasPeriodCost(record: BomMaterialCostItemModelCostTrend, columnKey: string): boolean {
  const period = resolvePeriodKey(columnKey)
  const value = record.periodMaterialCosts?.[period] ?? record.periodUnitPrices?.[period]
  return value != null && !Number.isNaN(value)
}

/**
 * 期间变动码
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns present / absent / new / removed / up / down / flat
 */
function getPeriodChangeType(record: BomMaterialCostItemModelCostTrend, columnKey: string): string {
  const period = resolvePeriodKey(columnKey)
  return record.periodChangeTypes?.[period] || (hasPeriodCost(record, columnKey) ? 'present' : 'absent')
}

/**
 * 格式化期间材料成本
 * @param {BomMaterialCostItemModelCostTrend} record 行
 * @param {string} columnKey 列键
 * @returns {string} 文本
 */
function formatPeriodPrice(record: BomMaterialCostItemModelCostTrend, columnKey: string): string {
  const period = resolvePeriodKey(columnKey)
  const value = record.periodMaterialCosts?.[period] ?? record.periodUnitPrices?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 构建查询
 * @param trendFilterOverride 涨跌筛选覆盖值（工具栏点击时显式传入）
 * @param sortByOverride 全量排序覆盖值（工具栏点击时显式传入）
 * @returns 查询 DTO 或 null
 */
function buildQuery(trendFilterOverride?: string, sortByOverride?: string) {
  const plantCode = queryPlantCode.value?.trim()
  const materialType = queryMaterialType.value?.trim()
  if (!plantCode || !materialType) {
    return null
  }
  const models = (queryModelCodes.value ?? [])
    .map((c) => String(c).trim())
    .filter(Boolean)
  const components = (queryComponentCodes.value ?? [])
    .map((c) => String(c).trim())
    .filter(Boolean)
  const product = queryProductCode.value?.trim()
  const rangeEnd = periodRange.value?.[1]?.trim()
  const filter =
    trendFilterOverride !== undefined ? trendFilterOverride : props.trendFilter
  const sortBy =
    (sortByOverride !== undefined ? sortByOverride : props.sortBy)?.trim()
    || 'productCountDesc'
  return {
    plantCode,
    materialType,
    modelCodes: models.length ? models.join(',') : undefined,
    componentCodes: components.length ? components.join(',') : undefined,
    productCode: product || undefined,
    focusPeriod: rangeEnd || undefined,
    trendFilter: filter || undefined,
    sortBy,
    mergeMode: isDetailMode.value ? 'detail' : 'summary',
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

/**
 * 加载
 * @param trendFilterOverride 涨跌筛选覆盖值
 * @param sortByOverride 全量排序覆盖值
 */
async function loadData(trendFilterOverride?: string, sortByOverride?: string) {
  const query = buildQuery(trendFilterOverride, sortByOverride)
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getBomMaterialCostItemModelCostTrendAnalysis(query)
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
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix.value}.queryFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/**
 * 重新加载（重置页码）
 * @param trendFilterOverride 涨跌筛选覆盖值（点涨/跌时传入 up/down）
 * @param sortByOverride 全量排序覆盖值
 */
async function reload(trendFilterOverride?: string, sortByOverride?: string) {
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData(trendFilterOverride, sortByOverride)
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
    const sheetTitle = isDetailMode.value ? 'DTA 差异组件推移表' : 'DTA BOM通用组件成本推移表'
    const exportBase = buildBomExportBaseName(sheetTitle, [
      query.plantCode,
      query.modelCodes,
      query.componentCodes,
      query.productCode])
    const exportMeta = await exportBomMaterialCostItemModelCostTrendAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      sheetTitle,
      buildBomExportFileName(sheetTitle, [
        query.plantCode,
        query.modelCodes,
        query.componentCodes,
        query.productCode]),
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

/** 实测 scroll.y（扣除表头） */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  const y = Math.floor(wrap.clientHeight - 47)
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
