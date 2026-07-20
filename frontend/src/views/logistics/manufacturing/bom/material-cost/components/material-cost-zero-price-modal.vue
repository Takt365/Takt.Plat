<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：material-cost-zero-price-modal.vue -->
<!-- 功能描述：零价格合并清单（机种+组件+共用产品；X+F+移动平均价=0） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="openProxy"
    :title="t(`${localePrefix}.zeroPrice.title`, { month: costingMonthLabel, model: modelCodeLabel })"
    :width="1100"
    :footer="null"
    :use-viewport-size="false"
    :cancel-text="t('common.page.button.close')"
  >
    <div class="mb-3 flex flex-wrap items-center justify-between gap-2">
      <div class="text-sm text-text-secondary">
        {{
          t(`${localePrefix}.zeroPrice.hint`, {
            month: costingMonthLabel,
            model: modelCodeLabel,
            productCount,
            componentCount: total,
          })
        }}
      </div>
      <a-button
        v-permission="'logistics:manufacturing:bom:material:cost:export'"
        class="takt-button-export"
        :loading="exportLoading"
        :disabled="total === 0"
        @click="handleExport"
      >
        <template #icon>
          <RiExportLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.export') }}
      </a-button>
    </div>
    <TaktSingleTable
      entity-scope="company"
      table-mode="single"
      :columns="columns"
      :data-source="rows"
      :loading="loading"
      :stripe="true"
      :row-key="getRowKey"
      :pagination="false"
      :scroll="{ x: 'max-content', y: 420 }"
      :show-row-selection="false"
      id-column-key="componentCode"
      :visible-column-keys="visibleColumnKeys"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'movingAveragePrice'">
          {{ formatPrice((record as BomMaterialCostItemZeroMovingPrice).movingAveragePrice) }}
        </template>
        <template v-else-if="column.key === 'suggestedComponentCode'">
          {{ (record as BomMaterialCostItemZeroMovingPrice).suggestedComponentCode?.trim() || '—' }}
        </template>
        <template v-else-if="column.key === 'suggestedMovingPrice'">
          {{ formatOptionalPrice((record as BomMaterialCostItemZeroMovingPrice).suggestedMovingPrice) }}
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-if="total > 0"
      class="mt-2"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      :disabled="loading"
      @change="handlePageChange"
    />
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * 零价格合并清单（先选工厂/机种/核算月；按组件合并共用产品）
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiExportLine } from '@remixicon/vue'
import {
  exportBomMaterialCostItemZeroMovingPriceMerged,
  getBomMaterialCostItemZeroMovingPriceMerged,
} from '@/api/logistics/manufacturing/bom/material-cost-item'
import type {
  BomMaterialCostItemZeroMovingPrice,
  BomMaterialCostItemZeroMovingPriceQuery,
} from '@/types/logistics/manufacturing/bom/material-cost-trend'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { costingMonthToDateQuery } from '../utils/bom-material-cost-period'

const localePrefix = 'logistics.manufacturing.bom.material-cost.page'

const props = defineProps<{
  /** 弹窗显隐 */
  open: boolean
  /** 工厂 */
  plantCode?: string
  /** 机种（必填） */
  modelCode?: string
  /** 核算月 yyyy-MM（必选） */
  costingMonth?: string
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
}>()

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktBomMaterialCostItem')

/** 双向绑定 open */
const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v),
})

/** 当前核算月展示 */
const costingMonthLabel = computed(() => (props.costingMonth ?? '').trim() || '—')

/** 机种展示 */
const modelCodeLabel = computed(() => (props.modelCode ?? '').trim() || '—')

/** 合并行 */
const rows = ref<BomMaterialCostItemZeroMovingPrice[]>([])
/** loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 页码 */
const pageIndex = ref(getTaktDefaultPageIndex())
/** 页大小 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数（合并组件数） */
const total = ref(0)
/** 机种下产品数 */
const productCount = ref(0)

/** 列定义 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.bommaterialcost.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 90,
    ellipsis: true,
  },
  {
    title: t('entity.bommaterialcost.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.bommaterialcostitem.componentcode'),
    dataIndex: 'componentCode',
    key: 'componentCode',
    width: 130,
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
    title: t(`${localePrefix}.zeroPrice.productCodes`),
    dataIndex: 'productCodes',
    key: 'productCodes',
    width: 240,
    ellipsis: true,
  },
  {
    title: t(`${localePrefix}.zeroPrice.productCount`),
    dataIndex: 'productCount',
    key: 'productCount',
    width: 90,
    align: 'right',
  },
  {
    title: t('entity.bommaterialcostitem.movingaverageprice'),
    dataIndex: 'movingAveragePrice',
    key: 'movingAveragePrice',
    width: 110,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.zeroPrice.suggestedComponentCode`),
    dataIndex: 'suggestedComponentCode',
    key: 'suggestedComponentCode',
    width: 140,
    ellipsis: true,
  },
  {
    title: t(`${localePrefix}.zeroPrice.suggestedMovingPrice`),
    dataIndex: 'suggestedMovingPrice',
    key: 'suggestedMovingPrice',
    width: 120,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.costingMonth`),
    dataIndex: 'costingPeriod',
    key: 'costingPeriod',
    width: 100,
  },
])

/** 可见列 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {BomMaterialCostItemZeroMovingPrice} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemZeroMovingPrice): string {
  return `${record.plantCode}|${record.modelCode}|${record.componentCode}|${record.costingPeriod}`
}

/**
 * 格式化价格
 * @param {unknown} value 价格
 * @returns {string} 文本
 */
function formatPrice(value: unknown): string {
  const n = Number(value)
  if (!Number.isFinite(n)) {
    return '—'
  }
  return n.toFixed(5)
}

/**
 * 格式化可选价格（null/undefined/非数字 → 空）
 * @param {unknown} value 价格
 * @returns {string} 文本
 */
function formatOptionalPrice(value: unknown): string {
  if (value === null || value === undefined || value === '') {
    return '—'
  }
  return formatPrice(value)
}

/**
 * 构建零价合并查询
 * @param {Partial<BomMaterialCostItemZeroMovingPriceQuery>} overrides 覆盖
 * @returns {BomMaterialCostItemZeroMovingPriceQuery | null} 查询
 */
function buildZeroPriceQuery(
  overrides?: Partial<BomMaterialCostItemZeroMovingPriceQuery>,
): BomMaterialCostItemZeroMovingPriceQuery | null {
  const plant = (props.plantCode ?? '').trim()
  const model = (props.modelCode ?? '').trim()
  const month = (props.costingMonth ?? '').trim()
  if (!plant || !model || !month) {
    return null
  }
  return {
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    plantCode: plant,
    modelCode: model,
    ...costingMonthToDateQuery(month),
    ...overrides,
  }
}

/** 加载合并清单 */
async function loadData() {
  const query = buildZeroPriceQuery()
  if (!query) {
    rows.value = []
    total.value = 0
    productCount.value = 0
    return
  }
  loading.value = true
  try {
    const res = await getBomMaterialCostItemZeroMovingPriceMerged(query)
    rows.value = res.paged?.data ?? []
    total.value = res.paged?.total ?? res.componentCount ?? 0
    productCount.value = res.productCodes?.length ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    rows.value = []
    total.value = 0
    productCount.value = 0
  } finally {
    loading.value = false
  }
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

/** 导出当前筛选全部合并行 */
async function handleExport() {
  const month = (props.costingMonth ?? '').trim()
  const model = (props.modelCode ?? '').trim()
  const query = buildZeroPriceQuery({
    pageIndex: 1,
    pageSize: 100000,
  })
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  exportLoading.value = true
  try {
    const exportMeta = await exportBomMaterialCostItemZeroMovingPriceMerged(
      query,
      excelNames.sheet,
      `${excelNames.fileBase}_zero_price_${model}_${month}`,
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: `${excelNames.fileBase}_zero_price_${model}_${month}`,
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
    message.success(t(`${localePrefix}.zeroPrice.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.zeroPrice.exportFailed`))
  } finally {
    exportLoading.value = false
  }
}

watch(
  () => props.open,
  async (visible) => {
    if (!visible) {
      return
    }
    if (!(props.plantCode ?? '').trim()) {
      message.warning(t(`${localePrefix}.selectPlantRequired`))
      emit('update:open', false)
      return
    }
    if (!(props.modelCode ?? '').trim()) {
      message.warning(t(`${localePrefix}.selectModelRequired`))
      emit('update:open', false)
      return
    }
    if (!(props.costingMonth ?? '').trim()) {
      message.warning(t(`${localePrefix}.costNeedMonth`))
      emit('update:open', false)
      return
    }
    await ensureTaktPaginationConfigAsync()
    pageIndex.value = getTaktDefaultPageIndex()
    pageSize.value = getTaktDefaultPageSize()
    await loadData()
  },
)
</script>
