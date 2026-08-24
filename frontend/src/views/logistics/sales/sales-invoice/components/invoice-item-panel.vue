<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-item-panel.vue -->
<!-- 功能描述：Takt销售发票主表实体主表实体右侧明细 salesInvoiceItem 独立 CRUD（按主表选中 salesInvoiceId 分页） -->
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
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
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
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

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
 * Takt销售发票主表实体子表 salesInvoiceItem 右栏面板
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
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
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
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'plantCode') ?? ''),
  },
  {
    title: pi.label('billingDocumentCode'),
    dataIndex: 'billingDocumentCode',
    key: 'billingDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'billingDocumentCode') ?? ''),
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
    title: pi.label('billingQuantity'),
    dataIndex: 'billingQuantity',
    key: 'billingQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'billingQuantity') ?? ''),
  },
  {
    title: pi.label('salesUnit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesUnit') ?? ''),
  },
  {
    title: pi.label('baseUnit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'baseUnit') ?? ''),
  },
  {
    title: pi.label('scaleQuantity'),
    dataIndex: 'scaleQuantity',
    key: 'scaleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'scaleQuantity') ?? ''),
  },
  {
    title: pi.label('billingQuantitySku'),
    dataIndex: 'billingQuantitySku',
    key: 'billingQuantitySku',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'billingQuantitySku') ?? ''),
  },
  {
    title: pi.label('netWeight'),
    dataIndex: 'netWeight',
    key: 'netWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'netWeight') ?? ''),
  },
  {
    title: pi.label('grossWeight'),
    dataIndex: 'grossWeight',
    key: 'grossWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'grossWeight') ?? ''),
  },
  {
    title: pi.label('weightUnit'),
    dataIndex: 'weightUnit',
    key: 'weightUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'weightUnit') ?? ''),
  },
  {
    title: pi.label('businessAreaCode'),
    dataIndex: 'businessAreaCode',
    key: 'businessAreaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'businessAreaCode') ?? ''),
  },
  {
    title: pi.label('pricingDate'),
    dataIndex: 'pricingDate',
    key: 'pricingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'pricingDate') ?? ''),
  },
  {
    title: pi.label('serviceRenderedDate'),
    dataIndex: 'serviceRenderedDate',
    key: 'serviceRenderedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'serviceRenderedDate') ?? ''),
  },
  {
    title: pi.label('pricingExchangeRate'),
    dataIndex: 'pricingExchangeRate',
    key: 'pricingExchangeRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'pricingExchangeRate') ?? ''),
  },
  {
    title: pi.label('netAmount'),
    dataIndex: 'netAmount',
    key: 'netAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'netAmount') ?? ''),
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
    title: pi.label('referenceDocumentCategory'),
    dataIndex: 'referenceDocumentCategory',
    key: 'referenceDocumentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'referenceDocumentCategory') ?? ''),
  },
  {
    title: pi.label('salesDocumentCode'),
    dataIndex: 'salesDocumentCode',
    key: 'salesDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesDocumentCode') ?? ''),
  },
  {
    title: pi.label('salesDocumentItem'),
    dataIndex: 'salesDocumentItem',
    key: 'salesDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesDocumentItem') ?? ''),
  },
  {
    title: pi.label('salesDocumentReferenceFlag'),
    dataIndex: 'salesDocumentReferenceFlag',
    key: 'salesDocumentReferenceFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesDocumentReferenceFlag') ?? ''),
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
    title: pi.label('materialDescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'materialDescription') ?? ''),
  },
  {
    title: pi.label('pricingReferenceMaterialCode'),
    dataIndex: 'pricingReferenceMaterialCode',
    key: 'pricingReferenceMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'pricingReferenceMaterialCode') ?? ''),
  },
  {
    title: pi.label('batchCode'),
    dataIndex: 'batchCode',
    key: 'batchCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'batchCode') ?? ''),
  },
  {
    title: pi.label('materialGroup'),
    dataIndex: 'materialGroup',
    key: 'materialGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'materialGroup') ?? ''),
  },
  {
    title: pi.label('salesItemCategory'),
    dataIndex: 'salesItemCategory',
    key: 'salesItemCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesItemCategory') ?? ''),
  },
  {
    title: pi.label('productHierarchy'),
    dataIndex: 'productHierarchy',
    key: 'productHierarchy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'productHierarchy') ?? ''),
  },
  {
    title: pi.label('shippingPoint'),
    dataIndex: 'shippingPoint',
    key: 'shippingPoint',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'shippingPoint') ?? ''),
  },
  {
    title: pi.label('division'),
    dataIndex: 'division',
    key: 'division',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'division') ?? ''),
  },
  {
    title: pi.label('partnerItem'),
    dataIndex: 'partnerItem',
    key: 'partnerItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'partnerItem') ?? ''),
  },
  {
    title: pi.label('departureCountry'),
    dataIndex: 'departureCountry',
    key: 'departureCountry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'departureCountry') ?? ''),
  },
  {
    title: pi.label('plantRegion'),
    dataIndex: 'plantRegion',
    key: 'plantRegion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'plantRegion') ?? ''),
  },
  {
    title: pi.label('pricingFlag'),
    dataIndex: 'pricingFlag',
    key: 'pricingFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'pricingFlag') ?? ''),
  },
  {
    title: pi.label('warehouseCode'),
    dataIndex: 'warehouseCode',
    key: 'warehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'warehouseCode') ?? ''),
  },
  {
    title: pi.label('costAmount'),
    dataIndex: 'costAmount',
    key: 'costAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'costAmount') ?? ''),
  },
  {
    title: pi.label('subtotal1'),
    dataIndex: 'subtotal1',
    key: 'subtotal1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal1') ?? ''),
  },
  {
    title: pi.label('subtotal2'),
    dataIndex: 'subtotal2',
    key: 'subtotal2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal2') ?? ''),
  },
  {
    title: pi.label('subtotal3'),
    dataIndex: 'subtotal3',
    key: 'subtotal3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal3') ?? ''),
  },
  {
    title: pi.label('subtotal4'),
    dataIndex: 'subtotal4',
    key: 'subtotal4',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal4') ?? ''),
  },
  {
    title: pi.label('subtotal5'),
    dataIndex: 'subtotal5',
    key: 'subtotal5',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal5') ?? ''),
  },
  {
    title: pi.label('subtotal6'),
    dataIndex: 'subtotal6',
    key: 'subtotal6',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'subtotal6') ?? ''),
  },
  {
    title: pi.label('statisticsExchangeRate'),
    dataIndex: 'statisticsExchangeRate',
    key: 'statisticsExchangeRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'statisticsExchangeRate') ?? ''),
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
    title: pi.label('creditPrice'),
    dataIndex: 'creditPrice',
    key: 'creditPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'creditPrice') ?? ''),
  },
  {
    title: pi.label('customerGroupSalesOrder'),
    dataIndex: 'customerGroupSalesOrder',
    key: 'customerGroupSalesOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'customerGroupSalesOrder') ?? ''),
  },
  {
    title: pi.label('destinationCountryOrder'),
    dataIndex: 'destinationCountryOrder',
    key: 'destinationCountryOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'destinationCountryOrder') ?? ''),
  },
  {
    title: pi.label('regionOrder'),
    dataIndex: 'regionOrder',
    key: 'regionOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'regionOrder') ?? ''),
  },
  {
    title: pi.label('salesOrganizationOrder'),
    dataIndex: 'salesOrganizationOrder',
    key: 'salesOrganizationOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'salesOrganizationOrder') ?? ''),
  },
  {
    title: pi.label('distributionChannelOrder'),
    dataIndex: 'distributionChannelOrder',
    key: 'distributionChannelOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'distributionChannelOrder') ?? ''),
  },
  {
    title: pi.label('documentCategory'),
    dataIndex: 'documentCategory',
    key: 'documentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'documentCategory') ?? ''),
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
    title: pi.label('grossAmount'),
    dataIndex: 'grossAmount',
    key: 'grossAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'grossAmount') ?? ''),
  },
  {
    title: pi.label('exchangeRateDate'),
    dataIndex: 'exchangeRateDate',
    key: 'exchangeRateDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'exchangeRateDate') ?? ''),
  },
  {
    title: pi.label('postedBy'),
    dataIndex: 'postedBy',
    key: 'postedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'postedBy') ?? ''),
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
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesInvoiceItem }) =>
      String(getSalesInvoiceItemField(record, 'remark') ?? ''),
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
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
    if (!hasAnyListQueryFilter()) {
      return
    }
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
