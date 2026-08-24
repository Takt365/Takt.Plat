<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-model-trend/components -->
<!-- 文件名称：purchase-model-trend-panel.vue -->
<!-- 功能描述：采购机种推移转置涨跌表 -->
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
        :id-column-key="'materialCode'"
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
            <template v-if="isCarriedPeriodPrice(record as PurchaseModelTrend, String(column.key))">
              <a-tooltip
                :title="carriedPeriodTooltip(record as PurchaseModelTrend, String(column.key))"
              >
                <span class="cursor-help border-b border-dotted border-text-secondary text-text-secondary">
                  {{ formatPeriodPrice(record as PurchaseModelTrend, String(column.key)) }}*
                </span>
              </a-tooltip>
            </template>
            <template v-else>
              {{ formatPeriodPrice(record as PurchaseModelTrend, String(column.key)) }}
            </template>
          </template>
          <template v-else-if="column.key === 'currencyCode'">
            <TaktDictTag
              :value="(record as PurchaseModelTrend).currencyCode"
              dict-type="accounting_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as PurchaseModelTrend).trend || 'none')">
              {{ trendLabel((record as PurchaseModelTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as PurchaseModelTrend).varianceAmount)">
              {{ formatCost((record as PurchaseModelTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as PurchaseModelTrend).varianceAmount)">
              {{ formatPercent((record as PurchaseModelTrend).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div class="space-y-1">
            <div class="truncate text-sm text-text-secondary">
              {{ summaryText }}
            </div>
            <div
              v-if="hasQuery && comparePeriod"
              class="text-sm text-text"
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
 * 采购价格推移转置涨跌表
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportPurchaseModelTrendAnalysis,
  getPurchaseModelTrendAnalysis,
} from '@/api/logistics/procurement/purchase-model-trend'
import type { PurchaseModelTrend } from '@/types/logistics/procurement/purchase-model-trend'
import { useTableRefresh } from '@/composables/use-table-refresh'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/takt-paged'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
  /** 工厂代码 */
  plantCode?: string
  /** 年月区间 */
  periodRange?: [string, string] | null
  /** 产品物料类型（机种推移必填） */
  materialType?: string
  /** 供应商编码 */
  supplierCode?: string
  /** 物料编码关键字 */
  materialCode?: string
  /** 价格类型 */
  priceType?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.procurement.purchase-price-trend.page'
const { t } = useI18n()

/** 行数据 */
const rows = ref<PurchaseModelTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 行总数（筛选后） */
const materialCount = ref(0)
/** 环比基准月 */
const basePeriod = ref('')
/** 环比对比月 */
const comparePeriod = ref('')
/** 涨跌计数 */
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

/** 工厂必填 */
const hasQuery = computed(() => !!props.plantCode?.trim())

/** 摘要 */
const summaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.summaryModel`, { count: materialCount.value })
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.purchaseprice.materialcode'),
      dataIndex: 'materialCode',
      key: 'materialCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t(`${localePrefix}.columns.modelGroup`),
      dataIndex: 'modelGroup',
      key: 'modelGroup',
      width: 220,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.columns.productGroup`),
      dataIndex: 'productGroup',
      key: 'productGroup',
      width: 220,
      ellipsis: true,
    },
    {
      title: t('entity.purchaseprice.materialdescription'),
      dataIndex: 'materialText',
      key: 'materialText',
      width: 180,
      ellipsis: true,
    },
    {
      title: t('entity.purchaseprice.suppliercode'),
      dataIndex: 'supplierCode',
      key: 'supplierCode',
      width: 120,
      ellipsis: true,
    },
    {
      title: t('entity.supplier.name1'),
      dataIndex: 'supplierName',
      key: 'supplierName',
      width: 160,
      ellipsis: true,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodUnitPrices', period],
      key: `period_${period}`,
      width: 110,
      align: 'right',
    })
  }
  cols.push(
    {
      title: t(`${localePrefix}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 80,
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 110,
      align: 'right',
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.variancePercent`),
      dataIndex: 'variancePercent',
      key: 'variancePercent',
      width: 90,
      align: 'right',
      fixed: 'right',
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {PurchaseModelTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: PurchaseModelTrend): string {
  return `${record.plantCode}|${record.materialCode}|${record.supplierCode}`
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
 * 格式化环比变动率
 * @param {number | null | undefined} value 比率
 * @returns {string} 百分比文本
 */
function formatPercent(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return `${(value * 100).toFixed(2)}%`
}

/**
 * 涨跌文案
 * @param {string} trend 涨跌码
 * @returns {string} 文案
 */
function trendLabel(trend: string): string {
  const key = trend || 'none'
  return t(`${localePrefix}.trend.${key}`)
}

/**
 * 涨跌样式
 * @param {string} trend 涨跌码
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up') return 'text-red-500'
  if (trend === 'down') return 'text-green-600'
  return 'text-text-secondary'
}

/**
 * 差额样式
 * @param {number | null | undefined} amount 差额
 * @returns {string} class
 */
function varianceClass(amount?: number | null): string {
  if (amount == null || amount === 0) return 'text-text-secondary'
  return amount > 0 ? 'text-red-500' : 'text-green-600'
}

/**
 * 格式化期间单价
 * @param {PurchaseModelTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodPrice(record: PurchaseModelTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodUnitPrices?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 是否缺月回填价（来源 ≠ 展示月：回填为最近价格日期 yyyy-MM-dd）
 * @param {PurchaseModelTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {boolean} 是否回填
 */
function isCarriedPeriodPrice(record: PurchaseModelTrend, columnKey: string): boolean {
  const period = columnKey.replace(/^period_/, '')
  const price = record.periodUnitPrices?.[period]
  if (price == null || Number.isNaN(price)) return false
  const source = record.periodPriceSourcePeriods?.[period]
  if (!source) return false
  return String(source) !== String(period)
}

/**
 * 回填悬停提示（最近价格日期）
 * @param {PurchaseModelTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 提示
 */
function carriedPeriodTooltip(record: PurchaseModelTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const source = record.periodPriceSourcePeriods?.[period] || '—'
  return t(`${localePrefix}.carriedFrom`, { period: source })
}

/**
 * 期间区间 → API 参数
 * @param {[string, string] | null | undefined} range 年月区间
 * @returns periodDateStart / periodDateEnd / focusPeriod
 */
function periodRangeToQuery(range: [string, string] | null | undefined) {
  if (!range?.[0]) {
    return {}
  }
  const periodDateStart = `${range[0]}-01`
  if (!range[1]) {
    return { periodDateStart, focusPeriod: range[0] }
  }
  return {
    periodDateStart,
    periodDateEnd: `${range[1]}-01`,
    focusPeriod: range[1],
  }
}

/**
 * 构建查询
 * @returns 查询 DTO 或 null
 */
function buildQuery() {
  const plant = props.plantCode?.trim()
  const type = props.materialType?.trim()
  if (!plant) {
    return null
  }
  if (!type) {
    return null
  }
  return {
    plantCode: plant,
    materialType: type || undefined,
    supplierCode: props.supplierCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    priceType: props.priceType,
    onlyEnabled: true,
    trendFilter: props.trendFilter || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToQuery(props.periodRange),
  }
}

/** 清空 */
function clear() {
  rows.value = []
  periodOrder.value = []
  materialCount.value = 0
  basePeriod.value = ''
  comparePeriod.value = ''
  upCount.value = 0
  downCount.value = 0
  flatCount.value = 0
  total.value = 0
  pageIndex.value = getTaktDefaultPageIndex()
  hasRows.value = false
}

/** 加载数据 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getPurchaseModelTrendAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    materialCount.value = result.materialCount ?? 0
    basePeriod.value = result.basePeriod ?? ''
    comparePeriod.value = result.comparePeriod ?? ''
    upCount.value = result.upCount ?? 0
    downCount.value = result.downCount ?? 0
    flatCount.value = result.flatCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
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
 * 分页变更
 * @param {number} page 页码
 * @param {number} size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 清单导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (materialCount.value === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  try {
    const exportFn = exportPurchaseModelTrendAnalysis
    const exportMeta = await exportFn({
      ...query,
      pageIndex: 1,
      pageSize: 1,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: `purchase-model-trend_${query.plantCode}`,
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

/** 测算表体高度 */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  const headerApprox = 55
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - headerApprox)
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
  () => props.trendFilter,
  () => {
    if (!props.plantCode?.trim()) {
      return
    }
    void reload()
  },
)

useTableRefresh(reload)

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
