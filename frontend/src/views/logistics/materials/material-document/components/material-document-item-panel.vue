<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：material-document-item-panel.vue -->
<!-- 功能描述：Takt物料凭证主表实体主表实体右侧明细 materialDocumentItem 独立 CRUD（按主表选中 materialDocumentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-document-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:materials:material:document:create"
      update-permission="logistics:materials:material:document:update"
      delete-permission="logistics:materials:material:document:delete"
      import-permission="logistics:materials:material:document:import"
      export-permission="logistics:materials:material:document:export"
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
      class="material-document-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getMaterialDocumentItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialDocumentItemId"
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
      <MaterialDocumentItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaterialDocumentId"
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
        :entity-i18n-key="MATERIALDOCUMENTITEM_SELF_I18N_KEY"
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
      id-column-key="materialDocumentItemId"
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
 * Takt物料凭证主表实体子表 materialDocumentItem 右栏面板
 * @module views/logistics/materials/material-document/components
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
import MaterialDocumentItemForm from './material-document-item-form.vue'
import { useMaterialDocumentMasterContext } from '../composables/use-material-document-master-context'
import {
  getMaterialDocumentItemList,
  getMaterialDocumentItemById,
  createMaterialDocumentItem,
  updateMaterialDocumentItem,
  deleteMaterialDocumentItemById,
  deleteMaterialDocumentItemBatch,
  getMaterialDocumentItemTemplate,
  importMaterialDocumentItem,
  exportMaterialDocumentItem,
} from '@/api/logistics/materials/material-document-item'
import type { MaterialDocumentItem, MaterialDocumentItemQuery } from '@/types/logistics/materials/material-document-item'

import {
  useMaterialDocumentItemI18n,
  MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS,
  MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS,
  MATERIALDOCUMENTITEM_QUERY_FIELDS,
  MATERIALDOCUMENTITEM_SELF_I18N_KEY,
} from '../composables/use-material-document-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaterialDocumentItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useMaterialDocumentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialDocumentItem')
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
const dataSource = ref<MaterialDocumentItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaterialDocumentItem | null>(null)
const selectedRows = ref<MaterialDocumentItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaterialDocumentItem>>({})
const formLoading = ref(false)
const formRef = ref()

const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...MATERIALDOCUMENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'materialDocumentItemId'
const masterMaterialDocumentId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['materialDocumentId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterMaterialDocumentId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaterialDocumentItemId(record: MaterialDocumentItem | Record<string, unknown>): string {
  return String((record as MaterialDocumentItem)?.[entityIdName] ?? '')
}

function getMaterialDocumentItemField(record: MaterialDocumentItem | Record<string, unknown>, field: string): unknown {
  return (record as MaterialDocumentItem)?.[field as keyof MaterialDocumentItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'materialDocumentItemId',
    key: 'materialDocumentItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentItemId') ?? ''),
  },
  {
    title: pi.label('warehouseCode'),
    dataIndex: 'warehouseCode',
    key: 'warehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'warehouseCode') ?? ''),
  },
  {
    title: pi.label('batchCode'),
    dataIndex: 'batchCode',
    key: 'batchCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'batchCode') ?? ''),
  },
  {
    title: pi.label('stockType'),
    dataIndex: 'stockType',
    key: 'stockType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'stockType') ?? ''),
  },
  {
    title: pi.label('restrictedStockFlag'),
    dataIndex: 'restrictedStockFlag',
    key: 'restrictedStockFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'restrictedStockFlag') ?? ''),
  },
  {
    title: pi.label('specialStock'),
    dataIndex: 'specialStock',
    key: 'specialStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'specialStock') ?? ''),
  },
  {
    title: pi.label('supplierCode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'supplierCode') ?? ''),
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'customerCode') ?? ''),
  },
  {
    title: pi.label('debitCreditIndicator'),
    dataIndex: 'debitCreditIndicator',
    key: 'debitCreditIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'debitCreditIndicator') ?? ''),
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'currencyCode') ?? ''),
  },
  {
    title: pi.label('localCurrencyAmount'),
    dataIndex: 'localCurrencyAmount',
    key: 'localCurrencyAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'localCurrencyAmount') ?? ''),
  },
  {
    title: pi.label('alternativeAmount'),
    dataIndex: 'alternativeAmount',
    key: 'alternativeAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'alternativeAmount') ?? ''),
  },
  {
    title: pi.label('quantity'),
    dataIndex: 'quantity',
    key: 'quantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'quantity') ?? ''),
  },
  {
    title: pi.label('baseUnit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'baseUnit') ?? ''),
  },
  {
    title: pi.label('entryQuantity'),
    dataIndex: 'entryQuantity',
    key: 'entryQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'entryQuantity') ?? ''),
  },
  {
    title: pi.label('entryUnit'),
    dataIndex: 'entryUnit',
    key: 'entryUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'entryUnit') ?? ''),
  },
  {
    title: pi.label('poPriceQuantity'),
    dataIndex: 'poPriceQuantity',
    key: 'poPriceQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'poPriceQuantity') ?? ''),
  },
  {
    title: pi.label('poPriceUnit'),
    dataIndex: 'poPriceUnit',
    key: 'poPriceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'poPriceUnit') ?? ''),
  },
  {
    title: pi.label('purchaseOrderCode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'purchaseOrderCode') ?? ''),
  },
  {
    title: pi.label('purchaseOrderItem'),
    dataIndex: 'purchaseOrderItem',
    key: 'purchaseOrderItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'purchaseOrderItem') ?? ''),
  },
  {
    title: pi.label('referenceDocumentYear'),
    dataIndex: 'referenceDocumentYear',
    key: 'referenceDocumentYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentYear') ?? ''),
  },
  {
    title: pi.label('referenceDocumentCode'),
    dataIndex: 'referenceDocumentCode',
    key: 'referenceDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentCode') ?? ''),
  },
  {
    title: pi.label('referenceDocumentItem'),
    dataIndex: 'referenceDocumentItem',
    key: 'referenceDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'referenceDocumentItem') ?? ''),
  },
  {
    title: pi.label('originalMaterialDocumentYear'),
    dataIndex: 'originalMaterialDocumentYear',
    key: 'originalMaterialDocumentYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalMaterialDocumentYear') ?? ''),
  },
  {
    title: pi.label('originalMaterialDocumentCode'),
    dataIndex: 'originalMaterialDocumentCode',
    key: 'originalMaterialDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalMaterialDocumentCode') ?? ''),
  },
  {
    title: pi.label('originalLineNumber'),
    dataIndex: 'originalLineNumber',
    key: 'originalLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'originalLineNumber') ?? ''),
  },
  {
    title: pi.label('deliveryCompletedFlag'),
    dataIndex: 'deliveryCompletedFlag',
    key: 'deliveryCompletedFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'deliveryCompletedFlag') ?? ''),
  },
  {
    title: pi.label('itemText'),
    dataIndex: 'itemText',
    key: 'itemText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'itemText') ?? ''),
  },
  {
    title: pi.label('equipmentCode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'equipmentCode') ?? ''),
  },
  {
    title: pi.label('goodsRecipient'),
    dataIndex: 'goodsRecipient',
    key: 'goodsRecipient',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'goodsRecipient') ?? ''),
  },
  {
    title: pi.label('unloadingPoint'),
    dataIndex: 'unloadingPoint',
    key: 'unloadingPoint',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'unloadingPoint') ?? ''),
  },
  {
    title: pi.label('businessAreaCode'),
    dataIndex: 'businessAreaCode',
    key: 'businessAreaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'businessAreaCode') ?? ''),
  },
  {
    title: pi.label('controllingAreaCode'),
    dataIndex: 'controllingAreaCode',
    key: 'controllingAreaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'controllingAreaCode') ?? ''),
  },
  {
    title: pi.label('tradingPartnerBusinessArea'),
    dataIndex: 'tradingPartnerBusinessArea',
    key: 'tradingPartnerBusinessArea',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'tradingPartnerBusinessArea') ?? ''),
  },
  {
    title: pi.label('productionOrderCode'),
    dataIndex: 'productionOrderCode',
    key: 'productionOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'productionOrderCode') ?? ''),
  },
  {
    title: pi.label('assetCode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'assetCode') ?? ''),
  },
  {
    title: pi.label('assetSubCode'),
    dataIndex: 'assetSubCode',
    key: 'assetSubCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'assetSubCode') ?? ''),
  },
  {
    title: pi.label('fiscalYear'),
    dataIndex: 'fiscalYear',
    key: 'fiscalYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'fiscalYear') ?? ''),
  },
  {
    title: pi.label('postToPreviousPeriodFlag'),
    dataIndex: 'postToPreviousPeriodFlag',
    key: 'postToPreviousPeriodFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postToPreviousPeriodFlag') ?? ''),
  },
  {
    title: pi.label('postToPreviousYearFlag'),
    dataIndex: 'postToPreviousYearFlag',
    key: 'postToPreviousYearFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postToPreviousYearFlag') ?? ''),
  },
  {
    title: pi.label('accountingDocumentCode'),
    dataIndex: 'accountingDocumentCode',
    key: 'accountingDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'accountingDocumentCode') ?? ''),
  },
  {
    title: pi.label('accountingDocumentItem'),
    dataIndex: 'accountingDocumentItem',
    key: 'accountingDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'accountingDocumentItem') ?? ''),
  },
  {
    title: pi.label('revaluationDocumentCode'),
    dataIndex: 'revaluationDocumentCode',
    key: 'revaluationDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'revaluationDocumentCode') ?? ''),
  },
  {
    title: pi.label('revaluationDocumentItem'),
    dataIndex: 'revaluationDocumentItem',
    key: 'revaluationDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'revaluationDocumentItem') ?? ''),
  },
  {
    title: pi.label('reservationCode'),
    dataIndex: 'reservationCode',
    key: 'reservationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationCode') ?? ''),
  },
  {
    title: pi.label('reservationItem'),
    dataIndex: 'reservationItem',
    key: 'reservationItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationItem') ?? ''),
  },
  {
    title: pi.label('finalIssueFlag'),
    dataIndex: 'finalIssueFlag',
    key: 'finalIssueFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'finalIssueFlag') ?? ''),
  },
  {
    title: pi.label('reservationQuantity'),
    dataIndex: 'reservationQuantity',
    key: 'reservationQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'reservationQuantity') ?? ''),
  },
  {
    title: pi.label('receivingMaterialCode'),
    dataIndex: 'receivingMaterialCode',
    key: 'receivingMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingMaterialCode') ?? ''),
  },
  {
    title: pi.label('receivingPlantCode'),
    dataIndex: 'receivingPlantCode',
    key: 'receivingPlantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingPlantCode') ?? ''),
  },
  {
    title: pi.label('receivingWarehouseCode'),
    dataIndex: 'receivingWarehouseCode',
    key: 'receivingWarehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'receivingWarehouseCode') ?? ''),
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'profitCenterCode') ?? ''),
  },
  {
    title: pi.label('valuatedStockQuantity'),
    dataIndex: 'valuatedStockQuantity',
    key: 'valuatedStockQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'valuatedStockQuantity') ?? ''),
  },
  {
    title: pi.label('totalValuatedStockValue'),
    dataIndex: 'totalValuatedStockValue',
    key: 'totalValuatedStockValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'totalValuatedStockValue') ?? ''),
  },
  {
    title: pi.label('priceControl'),
    dataIndex: 'priceControl',
    key: 'priceControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'priceControl') ?? ''),
  },
  {
    title: pi.label('manufacturerPartMaterialCode'),
    dataIndex: 'manufacturerPartMaterialCode',
    key: 'manufacturerPartMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'manufacturerPartMaterialCode') ?? ''),
  },
  {
    title: pi.label('mkpfReferenceCode'),
    dataIndex: 'mkpfReferenceCode',
    key: 'mkpfReferenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'mkpfReferenceCode') ?? ''),
  },
  {
    title: pi.label('imDeliveryCode'),
    dataIndex: 'imDeliveryCode',
    key: 'imDeliveryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'imDeliveryCode') ?? ''),
  },
  {
    title: pi.label('imDeliveryItem'),
    dataIndex: 'imDeliveryItem',
    key: 'imDeliveryItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'imDeliveryItem') ?? ''),
  },
  {
    title: pi.label('postedBy'),
    dataIndex: 'postedBy',
    key: 'postedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postedBy') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'isObsolete') ?? ''),
  },
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'remark') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:document:update',
        onClick: (record: MaterialDocumentItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:document:delete',
        onClick: (record: MaterialDocumentItem) => void handleDeleteOne(record),
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
    idColumnKey: 'materialDocumentItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS)

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
    MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of MATERIALDOCUMENTITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getMaterialDocumentItemField(row, field))
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
  onChange: (keys: (string | number)[], rows: MaterialDocumentItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialDocumentItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMaterialDocumentItemId(selectedRow.value) === getMaterialDocumentItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialDocumentItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaterialDocumentItem) {
  const key = getMaterialDocumentItemId(record)
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
 * @returns {MaterialDocumentItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialDocumentItemQuery>): MaterialDocumentItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialDocumentItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    materialDocumentId: masterMaterialDocumentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialDocumentItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS) {
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
    const res = await getMaterialDocumentItemList(buildListQuery())
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
watch(masterMaterialDocumentId, () => {
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

async function handleEdit(record: MaterialDocumentItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getMaterialDocumentItemById(getMaterialDocumentItemId(record))
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
    const id = formData.value?.materialDocumentItemId
    if (id) {
      await updateMaterialDocumentItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaterialDocumentItem(payload)
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

async function handleDeleteOne(record: MaterialDocumentItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialDocumentItemById(getMaterialDocumentItemId(record))
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
      const ids = selectedRows.value.map((r) => getMaterialDocumentItemId(r)).filter(Boolean)
      await deleteMaterialDocumentItemBatch(ids)
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
  const res = await getMaterialDocumentItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaterialDocumentItem(file, sheetName)
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
    const exportMeta = await exportMaterialDocumentItem(
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
