<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-price/components -->
<!-- 文件名称：purchase-price-item-panel.vue -->
<!-- 功能描述：Takt采购价格实体主表实体右侧明细 purchasePriceItem 独立 CRUD（按主表选中 purchasePriceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="purchase-price-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:procurement:purchase:price:create"
      update-permission="logistics:procurement:purchase:price:update"
      delete-permission="logistics:procurement:purchase:price:delete"
      import-permission="logistics:procurement:purchase:price:import"
      export-permission="logistics:procurement:purchase:price:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="purchase-price-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPurchasePriceItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchasePriceItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <PurchasePriceItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchasePriceId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-procurement-purchase-price-purchase-price-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('purchasePriceCode')">
      <a-form-item :label="pi.queryLabel('purchasePriceCode')">
        <a-input
          v-model:value="advancedQueryForm.purchasePriceCode"
          :placeholder="pi.queryPh('purchasePriceCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasePriceSeq')">
      <a-form-item :label="pi.queryLabel('purchasePriceSeq')">
        <a-input-number
          v-model:value="advancedQueryForm.purchasePriceSeq"
          :placeholder="pi.queryPh('purchasePriceSeq', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceType')">
      <a-form-item :label="pi.queryLabel('priceType')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceType"
          dict-type="logistics_price_type"
          :placeholder="pi.queryPh('priceType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleType')">
      <a-form-item :label="pi.queryLabel('scaleType')">
        <TaktSelect
          v-model:value="advancedQueryForm.scaleType"
          dict-type="logistics_scale_type"
          :placeholder="pi.queryPh('scaleType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleBasis')">
      <a-form-item :label="pi.queryLabel('scaleBasis')">
        <TaktSelect
          v-model:value="advancedQueryForm.scaleBasis"
          dict-type="logistics_scale_basis"
          :placeholder="pi.queryPh('scaleBasis', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleQuantity')">
      <a-form-item :label="pi.queryLabel('scaleQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.scaleQuantity"
          :placeholder="pi.queryPh('scaleQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleUnit')">
      <a-form-item :label="pi.queryLabel('scaleUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.scaleUnit"
          dict-type="logistics_unit_of_measure_code"
          :placeholder="pi.queryPh('scaleUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleValue')">
      <a-form-item :label="pi.queryLabel('scaleValue')">
        <a-input-number
          v-model:value="advancedQueryForm.scaleValue"
          :placeholder="pi.queryPh('scaleValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scaleCurrency')">
      <a-form-item :label="pi.queryLabel('scaleCurrency')">
        <TaktSelect
          v-model:value="advancedQueryForm.scaleCurrency"
          dict-type="accounting_currency_code"
          :placeholder="pi.queryPh('scaleCurrency', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('calculationType')">
      <a-form-item :label="pi.queryLabel('calculationType')">
        <TaktSelect
          v-model:value="advancedQueryForm.calculationType"
          dict-type="logistics_calculation_type"
          :placeholder="pi.queryPh('calculationType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('price')">
      <a-form-item :label="pi.queryLabel('price')">
        <a-input-number
          v-model:value="advancedQueryForm.price"
          :placeholder="pi.queryPh('price', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('untaxedPrice')">
      <a-form-item :label="pi.queryLabel('untaxedPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.untaxedPrice"
          :placeholder="pi.queryPh('untaxedPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxIncludedPrice')">
      <a-form-item :label="pi.queryLabel('taxIncludedPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.taxIncludedPrice"
          :placeholder="pi.queryPh('taxIncludedPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conditionCurrency')">
      <a-form-item :label="pi.queryLabel('conditionCurrency')">
        <TaktSelect
          v-model:value="advancedQueryForm.conditionCurrency"
          dict-type="accounting_currency_code"
          :placeholder="pi.queryPh('conditionCurrency', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceUnit')">
      <a-form-item :label="pi.queryLabel('priceUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceUnit"
          dict-type="logistics_price_unit_param"
          :placeholder="pi.queryPh('priceUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitOfMeasure')">
      <a-form-item :label="pi.queryLabel('unitOfMeasure')">
        <TaktSelect
          v-model:value="advancedQueryForm.unitOfMeasure"
          dict-type="logistics_unit_of_measure_code"
          :placeholder="pi.queryPh('unitOfMeasure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minOrderQuantity')">
      <a-form-item :label="pi.queryLabel('minOrderQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.minOrderQuantity"
          :placeholder="pi.queryPh('minOrderQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('roundingValue')">
      <a-form-item :label="pi.queryLabel('roundingValue')">
        <a-input-number
          v-model:value="advancedQueryForm.roundingValue"
          :placeholder="pi.queryPh('roundingValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDeliveryTimeDays')">
      <a-form-item :label="pi.queryLabel('plannedDeliveryTimeDays')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDeliveryTimeDays"
          :placeholder="pi.queryPh('plannedDeliveryTimeDays', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no_type"
          :placeholder="pi.queryPh('isObsolete', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="PURCHASEPRICEITEM_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="purchasePriceItemId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购价格实体子表 purchasePriceItem 右栏面板
 * @module views/logistics/procurement/purchase-price/components
 */
import { ref, computed, watch, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/table-columns'
import { formatSummaryValue } from '@/components/business/takt-editable-table/editable-table-utils'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import PurchasePriceItemForm from './purchase-price-item-form.vue'
import { usePurchasePriceMasterContext } from '../composables/use-purchase-price-master-context'
import {
  getPurchasePriceItemList,
  getPurchasePriceItemById,
  createPurchasePriceItem,
  updatePurchasePriceItem,
  deletePurchasePriceItemById,
  deletePurchasePriceItemBatch,
  getPurchasePriceItemTemplate,
  importPurchasePriceItem,
  exportPurchasePriceItem,
} from '@/api/logistics/procurement/purchase-price-item'
import type { PurchasePriceItem, PurchasePriceItemQuery } from '@/types/logistics/procurement/purchase-price-item'

import {
  usePurchasePriceItemI18n,
  PURCHASEPRICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  PURCHASEPRICEITEM_SUMMARY_SUM_FIELDS,
  PURCHASEPRICEITEM_QUERY_STRING_FIELDS,
  PURCHASEPRICEITEM_QUERY_FIELDS,
  PURCHASEPRICEITEM_SELF_I18N_KEY,
} from '../composables/use-purchase-price-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = usePurchasePriceItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = usePurchasePriceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchasePriceItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
const dataSource = ref<PurchasePriceItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchasePriceItem | null>(null)
const selectedRows = ref<PurchasePriceItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchasePriceItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(PURCHASEPRICEITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PURCHASEPRICEITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    purchasePriceSeq: undefined as number | undefined,
    scaleQuantity: undefined as number | undefined,
    scaleValue: undefined as number | undefined,
    price: undefined as number | undefined,
    untaxedPrice: undefined as number | undefined,
    taxIncludedPrice: undefined as number | undefined,
    priceUnit: undefined as number | undefined,
    minOrderQuantity: undefined as number | undefined,
    roundingValue: undefined as number | undefined,
    plannedDeliveryTimeDays: undefined as number | undefined,
    isObsolete: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  PURCHASEPRICEITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...PURCHASEPRICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...PURCHASEPRICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'purchasePriceItemId'
const masterPurchasePriceId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['purchasePriceId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterPurchasePriceId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchasePriceItemId(record: PurchasePriceItem | Record<string, unknown>): string {
  return String((record as PurchasePriceItem)?.[entityIdName] ?? '')
}

function getPurchasePriceItemField(record: PurchasePriceItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchasePriceItem)?.[field as keyof PurchasePriceItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchasePriceItemId',
    key: 'purchasePriceItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'purchasePriceItemId') ?? ''),
  },
  {
    title: pi.label('purchasePriceId'),
    dataIndex: 'purchasePriceId',
    key: 'purchasePriceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'purchasePriceId') ?? ''),
  },
  {
    title: pi.label('purchasePriceCode'),
    dataIndex: 'purchasePriceCode',
    key: 'purchasePriceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'purchasePriceCode') ?? ''),
  },
  {
    title: pi.label('purchasePriceSeq'),
    dataIndex: 'purchasePriceSeq',
    key: 'purchasePriceSeq',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'purchasePriceSeq') ?? ''),
  },
  {
    title: pi.label('priceType'),
    dataIndex: 'priceType',
    key: 'priceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'priceType') ?? ''),
  },
  {
    title: pi.label('scaleType'),
    dataIndex: 'scaleType',
    key: 'scaleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleType') ?? ''),
  },
  {
    title: pi.label('scaleBasis'),
    dataIndex: 'scaleBasis',
    key: 'scaleBasis',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleBasis') ?? ''),
  },
  {
    title: pi.label('scaleQuantity'),
    dataIndex: 'scaleQuantity',
    key: 'scaleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleQuantity') ?? ''),
  },
  {
    title: pi.label('scaleUnit'),
    dataIndex: 'scaleUnit',
    key: 'scaleUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleUnit') ?? ''),
  },
  {
    title: pi.label('scaleValue'),
    dataIndex: 'scaleValue',
    key: 'scaleValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleValue') ?? ''),
  },
  {
    title: pi.label('scaleCurrency'),
    dataIndex: 'scaleCurrency',
    key: 'scaleCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'scaleCurrency') ?? ''),
  },
  {
    title: pi.label('calculationType'),
    dataIndex: 'calculationType',
    key: 'calculationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'calculationType') ?? ''),
  },
  {
    title: pi.label('price'),
    dataIndex: 'price',
    key: 'price',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'price') ?? ''),
  },
  {
    title: pi.label('untaxedPrice'),
    dataIndex: 'untaxedPrice',
    key: 'untaxedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'untaxedPrice') ?? ''),
  },
  {
    title: pi.label('taxIncludedPrice'),
    dataIndex: 'taxIncludedPrice',
    key: 'taxIncludedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'taxIncludedPrice') ?? ''),
  },
  {
    title: pi.label('conditionCurrency'),
    dataIndex: 'conditionCurrency',
    key: 'conditionCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'conditionCurrency') ?? ''),
  },
  {
    title: pi.label('priceUnit'),
    dataIndex: 'priceUnit',
    key: 'priceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'priceUnit') ?? ''),
  },
  {
    title: pi.label('unitOfMeasure'),
    dataIndex: 'unitOfMeasure',
    key: 'unitOfMeasure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'unitOfMeasure') ?? ''),
  },
  {
    title: pi.label('minOrderQuantity'),
    dataIndex: 'minOrderQuantity',
    key: 'minOrderQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'minOrderQuantity') ?? ''),
  },
  {
    title: pi.label('roundingValue'),
    dataIndex: 'roundingValue',
    key: 'roundingValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'roundingValue') ?? ''),
  },
  {
    title: pi.label('plannedDeliveryTimeDays'),
    dataIndex: 'plannedDeliveryTimeDays',
    key: 'plannedDeliveryTimeDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'plannedDeliveryTimeDays') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePriceItem }) =>
      String(getPurchasePriceItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:price:update',
        onClick: (record: PurchasePriceItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:price:delete',
        onClick: (record: PurchasePriceItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'purchasePriceItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(PURCHASEPRICEITEM_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    PURCHASEPRICEITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof PURCHASEPRICEITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of PURCHASEPRICEITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getPurchasePriceItemField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchasePriceItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchasePriceItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPurchasePriceItemId(selectedRow.value) === getPurchasePriceItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchasePriceItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchasePriceItem) {
  const key = getPurchasePriceItemId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PurchasePriceItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchasePriceItemQuery>): PurchasePriceItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchasePriceItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchasePriceId: masterPurchasePriceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchasePriceItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PURCHASEPRICEITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.purchasePriceSeq !== undefined && form.purchasePriceSeq !== null) {
    query.purchasePriceSeq = form.purchasePriceSeq
  }
  if (form.scaleQuantity !== undefined && form.scaleQuantity !== null) {
    query.scaleQuantity = form.scaleQuantity
  }
  if (form.scaleValue !== undefined && form.scaleValue !== null) {
    query.scaleValue = form.scaleValue
  }
  if (form.price !== undefined && form.price !== null) {
    query.price = form.price
  }
  if (form.untaxedPrice !== undefined && form.untaxedPrice !== null) {
    query.untaxedPrice = form.untaxedPrice
  }
  if (form.taxIncludedPrice !== undefined && form.taxIncludedPrice !== null) {
    query.taxIncludedPrice = form.taxIncludedPrice
  }
  if (form.priceUnit !== undefined && form.priceUnit !== null) {
    query.priceUnit = form.priceUnit
  }
  if (form.minOrderQuantity !== undefined && form.minOrderQuantity !== null) {
    query.minOrderQuantity = form.minOrderQuantity
  }
  if (form.roundingValue !== undefined && form.roundingValue !== null) {
    query.roundingValue = form.roundingValue
  }
  if (form.plannedDeliveryTimeDays !== undefined && form.plannedDeliveryTimeDays !== null) {
    query.plannedDeliveryTimeDays = form.plannedDeliveryTimeDays
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getPurchasePriceItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterPurchasePriceId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selected) => {
  if (selected) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PurchasePriceItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getPurchasePriceItemById(getPurchasePriceItemId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.purchasePriceItemId
    if (id) {
      await updatePurchasePriceItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createPurchasePriceItem(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: PurchasePriceItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchasePriceItemById(getPurchasePriceItemId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPurchasePriceItemId(r)).filter(Boolean)
      await deletePurchasePriceItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getPurchasePriceItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importPurchasePriceItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportPurchasePriceItem(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
