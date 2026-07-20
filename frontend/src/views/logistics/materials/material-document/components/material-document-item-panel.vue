<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：material-document-item-panel.vue -->
<!-- 功能描述：物料凭证明细右栏（仅查询/导入/导出；无新增、更新、删除；按主表选中 materialDocumentId 分页） -->
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
    <!-- 工具栏：仅导入/导出（无新增、更新、删除） -->
    <TaktToolsBar
      import-permission="logistics:materials:material:document:import"
      export-permission="logistics:materials:material:document:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
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
      :refresh-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
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
        :row-key="getMaterialDocumentItemId"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialDocumentItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="false"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
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

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-materials-material-document-material-document-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialDocumentCode')">
      <a-form-item :label="pi.queryLabel('materialDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.materialDocumentCode"
          :placeholder="pi.queryPh('materialDocumentCode', 'required')"
          show-count
          :maxlength="10"
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
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="pi.queryLabel('warehouseCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.warehouseCode"
          api-url="TaktWarehouses/options"
          :placeholder="pi.queryPh('warehouseCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movementType')">
      <a-form-item :label="pi.queryLabel('movementType')">
        <TaktSelect
          v-model:value="advancedQueryForm.movementType"
          dict-type="logistics_movement_type"
          :placeholder="pi.queryPh('movementType', 'select')"
          allow-clear
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
      <div v-show="isFieldVisible('quantity')">
      <a-form-item :label="pi.queryLabel('quantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quantity"
          :placeholder="pi.queryPh('quantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialStock')">
      <a-form-item :label="pi.queryLabel('specialStock')">
        <TaktSelect
          v-model:value="advancedQueryForm.specialStock"
          dict-type="logistics_special_stock_type"
          :placeholder="pi.queryPh('specialStock', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="pi.queryLabel('purchaseOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="pi.queryPh('purchaseOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionOrderCode')">
      <a-form-item :label="pi.queryLabel('productionOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.productionOrderCode"
          :placeholder="pi.queryPh('productionOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('projectCode')">
      <a-form-item :label="pi.queryLabel('projectCode')">
        <a-input
          v-model:value="advancedQueryForm.projectCode"
          :placeholder="pi.queryPh('projectCode', 'required')"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('documentDateStart')">
      <a-form-item :label="pi.queryLabel('documentDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentDateStart"
          :placeholder="pi.queryPh('documentDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentDateEnd')">
      <a-form-item :label="pi.queryLabel('documentDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentDateEnd"
          :placeholder="pi.queryPh('documentDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="pi.queryLabel('customerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerCode', 'select')"
          allow-clear
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
 * 物料凭证明细右栏面板（仅查询 / 导入 / 导出）
 * @module views/logistics/materials/material-document/components
 */
import { ref, computed, watch, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { message } from 'ant-design-vue'
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
import { RiQuestionLine } from '@remixicon/vue'
import { useMaterialDocumentMasterContext } from '../composables/use-material-document-master-context'
import {
  getMaterialDocumentItemList,
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

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MATERIALDOCUMENTITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    quantity: undefined as number | undefined,
    localCurrencyAmount: undefined as number | undefined,
    isObsolete: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  MATERIALDOCUMENTITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
    title: pi.label('materialDocumentId'),
    dataIndex: 'materialDocumentId',
    key: 'materialDocumentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentId') ?? ''),
  },
  {
    title: pi.label('materialDocumentCode'),
    dataIndex: 'materialDocumentCode',
    key: 'materialDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'materialDocumentCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'lineNumber') ?? ''),
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
    title: pi.label('movementType'),
    dataIndex: 'movementType',
    key: 'movementType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'movementType') ?? ''),
  },
  {
    title: pi.label('postingDate'),
    dataIndex: 'postingDate',
    key: 'postingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'postingDate') ?? ''),
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
    title: pi.label('projectCode'),
    dataIndex: 'projectCode',
    key: 'projectCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'projectCode') ?? ''),
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
    title: pi.label('documentDate'),
    dataIndex: 'documentDate',
    key: 'documentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'documentDate') ?? ''),
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
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialDocumentItem }) =>
      String(getMaterialDocumentItemField(record, 'isObsolete') ?? ''),
  },
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

/** 汇总行单元格（无行选择列：index 与展示列序一致） */
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
      index: columnIndex,
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

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
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
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    query.quantity = form.quantity
  }
  if (form.localCurrencyAmount !== undefined && form.localCurrencyAmount !== null) {
    query.localCurrencyAmount = form.localCurrencyAmount
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
