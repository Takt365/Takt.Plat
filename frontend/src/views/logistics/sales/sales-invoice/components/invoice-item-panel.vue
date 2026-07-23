<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-item-panel.vue -->
<!-- 功能描述：Takt销售发票实体主表实体右侧明细 salesInvoiceItem 独立 CRUD（按主表选中 salesInvoiceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="invoice-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:sales:invoice:create"
      update-permission="logistics:sales:invoice:update"
      delete-permission="logistics:sales:invoice:delete"
      import-permission="logistics:sales:invoice:import"
      export-permission="logistics:sales:invoice:export"
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
      class="invoice-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getSalesInvoiceItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="salesInvoiceItemId"
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
      <SalesInvoiceItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSalesInvoiceId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-sales-sales-invoice-invoice-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('accountingDocumentCode')">
      <a-form-item :label="pi.queryLabel('accountingDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.accountingDocumentCode"
          :placeholder="pi.queryPh('accountingDocumentCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingDateStart')">
      <a-form-item :label="pi.queryLabel('postingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.postingDateStart"
          :placeholder="pi.queryPh('postingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingDateEnd')">
      <a-form-item :label="pi.queryLabel('postingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.postingDateEnd"
          :placeholder="pi.queryPh('postingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelName')">
      <a-form-item :label="pi.queryLabel('modelName')">
        <a-input
          v-model:value="advancedQueryForm.modelName"
          :placeholder="pi.queryPh('modelName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktMaterialPlants/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="pi.queryLabel('materialType')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialType"
          dict-type="logistics_material_type"
          :placeholder="pi.queryPh('materialType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="pi.queryLabel('materialName')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="pi.queryPh('materialName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterCode')">
      <a-form-item :label="pi.queryLabel('profitCenterCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.profitCenterCode"
          api-url="TaktProfitCenters/options"
          :placeholder="pi.queryPh('profitCenterCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitle')">
      <a-form-item :label="pi.queryLabel('accountTitle')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitle"
          api-url="TaktAccountTitles/options"
          :placeholder="pi.queryPh('accountTitle', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantity')">
      <a-form-item :label="pi.queryLabel('quantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quantity"
          :placeholder="pi.queryPh('quantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unit')">
      <a-form-item :label="pi.queryLabel('unit')">
        <TaktSelect
          v-model:value="advancedQueryForm.unit"
          dict-type="logistics_unit_of_measure_code"
          :placeholder="pi.queryPh('unit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('localCurrencyAmount')">
      <a-form-item :label="pi.queryLabel('localCurrencyAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.localCurrencyAmount"
          :placeholder="pi.queryPh('localCurrencyAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionCurrencyAmount')">
      <a-form-item :label="pi.queryLabel('transactionCurrencyAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.transactionCurrencyAmount"
          :placeholder="pi.queryPh('transactionCurrencyAmount', 'required')"
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
      <div v-show="isFieldVisible('untaxedPrice')">
      <a-form-item :label="pi.queryLabel('untaxedPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.untaxedPrice"
          :placeholder="pi.queryPh('untaxedPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="pi.queryLabel('taxAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="pi.queryPh('taxAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentType')">
      <a-form-item :label="pi.queryLabel('documentType')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentType"
          dict-type="logistics_accounting_document_type"
          :placeholder="pi.queryPh('documentType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentCode')">
      <a-form-item :label="pi.queryLabel('referenceDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.referenceDocumentCode"
          :placeholder="pi.queryPh('referenceDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentItem')">
      <a-form-item :label="pi.queryLabel('referenceDocumentItem')">
        <a-input-number
          v-model:value="advancedQueryForm.referenceDocumentItem"
          :placeholder="pi.queryPh('referenceDocumentItem', 'required')"
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
        :entity-i18n-key="SALESINVOICEITEM_SELF_I18N_KEY"
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
      id-column-key="salesInvoiceItemId"
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
 * Takt销售发票实体子表 salesInvoiceItem 右栏面板
 * @module views/logistics/sales/sales-invoice/components
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
import SalesInvoiceItemForm from './invoice-item-form.vue'
import { useSalesInvoiceMasterContext } from '../composables/use-invoice-master-context'
import {
  getSalesInvoiceItemList,
  getSalesInvoiceItemById,
  createSalesInvoiceItem,
  updateSalesInvoiceItem,
  deleteSalesInvoiceItemById,
  deleteSalesInvoiceItemBatch,
  getSalesInvoiceItemTemplate,
  importSalesInvoiceItem,
  exportSalesInvoiceItem,
} from '@/api/logistics/sales/invoice-item'
import type { SalesInvoiceItem, SalesInvoiceItemQuery } from '@/types/logistics/sales/invoice-item'

import {
  useSalesInvoiceItemI18n,
  SALESINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  SALESINVOICEITEM_SUMMARY_SUM_FIELDS,
  SALESINVOICEITEM_QUERY_STRING_FIELDS,
  SALESINVOICEITEM_QUERY_FIELDS,
  SALESINVOICEITEM_SELF_I18N_KEY,
} from '../composables/use-invoice-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesInvoiceItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useSalesInvoiceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesInvoiceItem')
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
const dataSource = ref<SalesInvoiceItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SalesInvoiceItem | null>(null)
const selectedRows = ref<SalesInvoiceItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SalesInvoiceItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESINVOICEITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESINVOICEITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    quantity: undefined as number | undefined,
    localCurrencyAmount: undefined as number | undefined,
    transactionCurrencyAmount: undefined as number | undefined,
    taxIncludedPrice: undefined as number | undefined,
    untaxedPrice: undefined as number | undefined,
    taxAmount: undefined as number | undefined,
    referenceDocumentItem: undefined as number | undefined,
    isObsolete: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SALESINVOICEITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...SALESINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...SALESINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'salesInvoiceItemId'
const masterSalesInvoiceId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['salesInvoiceId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSalesInvoiceId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSalesInvoiceItemId(record: SalesInvoiceItem | Record<string, unknown>): string {
  return String((record as SalesInvoiceItem)?.[entityIdName] ?? '')
}

function getSalesInvoiceItemField(record: SalesInvoiceItem | Record<string, unknown>, field: string): unknown {
  return (record as SalesInvoiceItem)?.[field as keyof SalesInvoiceItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesInvoiceItemId',
    key: 'salesInvoiceItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesInvoiceItemId') ?? ''),
  },
  {
    title: pi.label('salesInvoiceId'),
    dataIndex: 'salesInvoiceId',
    key: 'salesInvoiceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesInvoiceId') ?? ''),
  },
  {
    title: pi.label('salesInvoiceName'),
    dataIndex: 'salesInvoiceName',
    key: 'salesInvoiceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesInvoiceName') ?? ''),
  },
  {
    title: pi.label('accountingDocumentCode'),
    dataIndex: 'accountingDocumentCode',
    key: 'accountingDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'accountingDocumentCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('postingDate'),
    dataIndex: 'postingDate',
    key: 'postingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'postingDate') ?? ''),
  },
  {
    title: pi.label('modelName'),
    dataIndex: 'modelName',
    key: 'modelName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'modelName') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('materialType'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'materialType') ?? ''),
  },
  {
    title: pi.label('materialName'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'materialName') ?? ''),
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'profitCenterCode') ?? ''),
  },
  {
    title: pi.label('accountTitle'),
    dataIndex: 'accountTitle',
    key: 'accountTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'accountTitle') ?? ''),
  },
  {
    title: pi.label('quantity'),
    dataIndex: 'quantity',
    key: 'quantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'quantity') ?? ''),
  },
  {
    title: pi.label('unit'),
    dataIndex: 'unit',
    key: 'unit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'unit') ?? ''),
  },
  {
    title: pi.label('localCurrencyAmount'),
    dataIndex: 'localCurrencyAmount',
    key: 'localCurrencyAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'localCurrencyAmount') ?? ''),
  },
  {
    title: pi.label('transactionCurrencyAmount'),
    dataIndex: 'transactionCurrencyAmount',
    key: 'transactionCurrencyAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'transactionCurrencyAmount') ?? ''),
  },
  {
    title: pi.label('taxIncludedPrice'),
    dataIndex: 'taxIncludedPrice',
    key: 'taxIncludedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'taxIncludedPrice') ?? ''),
  },
  {
    title: pi.label('untaxedPrice'),
    dataIndex: 'untaxedPrice',
    key: 'untaxedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'untaxedPrice') ?? ''),
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'taxAmount') ?? ''),
  },
  {
    title: pi.label('documentType'),
    dataIndex: 'documentType',
    key: 'documentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'documentType') ?? ''),
  },
  {
    title: pi.label('referenceDocumentCode'),
    dataIndex: 'referenceDocumentCode',
    key: 'referenceDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'referenceDocumentCode') ?? ''),
  },
  {
    title: pi.label('referenceDocumentItem'),
    dataIndex: 'referenceDocumentItem',
    key: 'referenceDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'referenceDocumentItem') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:invoice:update',
        onClick: (record: SalesInvoiceItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:invoice:delete',
        onClick: (record: SalesInvoiceItem) => void handleDeleteOne(record),
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
    idColumnKey: 'salesInvoiceItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(SALESINVOICEITEM_SUMMARY_SUM_FIELDS)

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
    SALESINVOICEITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof SALESINVOICEITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of SALESINVOICEITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getSalesInvoiceItemField(row, field))
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
  onChange: (keys: (string | number)[], rows: SalesInvoiceItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesInvoiceItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSalesInvoiceItemId(selectedRow.value) === getSalesInvoiceItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesInvoiceItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SalesInvoiceItem) {
  const key = getSalesInvoiceItemId(record)
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
 * @returns {SalesInvoiceItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesInvoiceItemQuery>): SalesInvoiceItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesInvoiceItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    salesInvoiceId: masterSalesInvoiceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesInvoiceItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESINVOICEITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    query.quantity = form.quantity
  }
  if (form.localCurrencyAmount !== undefined && form.localCurrencyAmount !== null) {
    query.localCurrencyAmount = form.localCurrencyAmount
  }
  if (form.transactionCurrencyAmount !== undefined && form.transactionCurrencyAmount !== null) {
    query.transactionCurrencyAmount = form.transactionCurrencyAmount
  }
  if (form.taxIncludedPrice !== undefined && form.taxIncludedPrice !== null) {
    query.taxIncludedPrice = form.taxIncludedPrice
  }
  if (form.untaxedPrice !== undefined && form.untaxedPrice !== null) {
    query.untaxedPrice = form.untaxedPrice
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.referenceDocumentItem !== undefined && form.referenceDocumentItem !== null) {
    query.referenceDocumentItem = form.referenceDocumentItem
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
    const res = await getSalesInvoiceItemList(buildListQuery())
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
watch(masterSalesInvoiceId, () => {
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

async function handleEdit(record: SalesInvoiceItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getSalesInvoiceItemById(getSalesInvoiceItemId(record))
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
    const id = formData.value?.salesInvoiceItemId
    if (id) {
      await updateSalesInvoiceItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesInvoiceItem(payload)
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

async function handleDeleteOne(record: SalesInvoiceItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesInvoiceItemById(getSalesInvoiceItemId(record))
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
      const ids = selectedRows.value.map((r) => getSalesInvoiceItemId(r)).filter(Boolean)
      await deleteSalesInvoiceItemBatch(ids)
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
  const res = await getSalesInvoiceItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesInvoiceItem(file, sheetName)
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
    const exportMeta = await exportSalesInvoiceItem(
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
