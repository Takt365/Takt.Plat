<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-transaction/components -->
<!-- 文件名称：material-transaction-item-panel.vue -->
<!-- 功能描述：Takt物料交易主表实体主表实体右侧明细 materialTransactionItem 独立 CRUD（按主表选中 materialTransactionId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-transaction-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.materialtransactionitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:materials:materialtransaction:create"
      update-permission="logistics:materials:materialtransaction:update"
      delete-permission="logistics:materials:materialtransaction:delete"
      import-permission="logistics:materials:materialtransaction:import"
      export-permission="logistics:materials:materialtransaction:export"
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
    <div class="material-transaction-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaterialTransactionItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialTransactionItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MaterialTransactionItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaterialTransactionId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-materials-material-transaction-material-transaction-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialTransactionCode')">
      <a-form-item :label="t('entity.materialtransactionitem.materialtransactioncode')">
        <a-input
          v-model:value="advancedQueryForm.materialTransactionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.materialtransactioncode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.materialtransactionitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.materialtransactionitem.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.sourcecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLineNumber')">
      <a-form-item :label="t('entity.materialtransactionitem.sourcelinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.sourcelinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.materialtransactionitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.materialtransactionitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.materialtransactionitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionUnit')">
      <a-form-item :label="t('entity.materialtransactionitem.transactionunit')">
        <a-input
          v-model:value="advancedQueryForm.transactionUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.transactionunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionQuantity')">
      <a-form-item :label="t('entity.materialtransactionitem.transactionquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.transactionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.transactionquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.materialtransactionitem.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.batchno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="t('entity.materialtransactionitem.warehousecode')">
        <a-input
          v-model:value="advancedQueryForm.warehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.warehousecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('locationCode')">
      <a-form-item :label="t('entity.materialtransactionitem.locationcode')">
        <a-input
          v-model:value="advancedQueryForm.locationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.locationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetWarehouseCode')">
      <a-form-item :label="t('entity.materialtransactionitem.targetwarehousecode')">
        <a-input
          v-model:value="advancedQueryForm.targetWarehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.targetwarehousecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetLocationCode')">
      <a-form-item :label="t('entity.materialtransactionitem.targetlocationcode')">
        <a-input
          v-model:value="advancedQueryForm.targetLocationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.targetlocationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitPrice')">
      <a-form-item :label="t('entity.materialtransactionitem.unitprice')">
        <a-input-number
          v-model:value="advancedQueryForm.unitPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.unitprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineAmount')">
      <a-form-item :label="t('entity.materialtransactionitem.lineamount')">
        <a-input-number
          v-model:value="advancedQueryForm.lineAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransactionitem.lineamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.materialtransactionitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.materialtransactionitem._self"
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
      id-column-key="materialTransactionItemId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt物料交易主表实体子表 materialTransactionItem 右栏面板
 * @module views/logistics/materials/material-transaction/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import MaterialTransactionItemForm from './material-transaction-item-form.vue'
import { useMaterialTransactionMasterContext } from '../composables/use-material-transaction-master-context'
import {
  getMaterialTransactionItemList,
  getMaterialTransactionItemById,
  createMaterialTransactionItem,
  updateMaterialTransactionItem,
  deleteMaterialTransactionItemById,
  deleteMaterialTransactionItemBatch,
  getMaterialTransactionItemTemplate,
  importMaterialTransactionItem,
  exportMaterialTransactionItem,
} from '@/api/logistics/materials/material-transaction-item'
import type { MaterialTransactionItem, MaterialTransactionItemQuery } from '@/types/logistics/materials/material-transaction-item'

const { t } = useI18n()
const { selectedMasterRow } = useMaterialTransactionMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialTransactionItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.materialtransactionitem._self') }),
)

const loading = ref(false)
const dataSource = ref<MaterialTransactionItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaterialTransactionItem | null>(null)
const selectedRows = ref<MaterialTransactionItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaterialTransactionItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  materialTransactionCode: '',
  lineNumber: undefined as number | undefined,
  sourceCode: '',
  sourceLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  transactionUnit: '',
  transactionQuantity: undefined as number | undefined,
  batchNo: '',
  warehouseCode: '',
  locationCode: '',
  targetWarehouseCode: '',
  targetLocationCode: '',
  unitPrice: undefined as number | undefined,
  lineAmount: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'materialTransactionCode', label: t('entity.materialtransactionitem.materialtransactioncode') },
  { key: 'lineNumber', label: t('entity.materialtransactionitem.linenumber') },
  { key: 'sourceCode', label: t('entity.materialtransactionitem.sourcecode') },
  { key: 'sourceLineNumber', label: t('entity.materialtransactionitem.sourcelinenumber') },
  { key: 'materialCode', label: t('entity.materialtransactionitem.materialcode') },
  { key: 'materialName', label: t('entity.materialtransactionitem.materialname') },
  { key: 'materialSpecification', label: t('entity.materialtransactionitem.materialspecification') },
  { key: 'transactionUnit', label: t('entity.materialtransactionitem.transactionunit') },
  { key: 'transactionQuantity', label: t('entity.materialtransactionitem.transactionquantity') },
  { key: 'batchNo', label: t('entity.materialtransactionitem.batchno') },
  { key: 'warehouseCode', label: t('entity.materialtransactionitem.warehousecode') },
  { key: 'locationCode', label: t('entity.materialtransactionitem.locationcode') },
  { key: 'targetWarehouseCode', label: t('entity.materialtransactionitem.targetwarehousecode') },
  { key: 'targetLocationCode', label: t('entity.materialtransactionitem.targetlocationcode') },
  { key: 'unitPrice', label: t('entity.materialtransactionitem.unitprice') },
  { key: 'lineAmount', label: t('entity.materialtransactionitem.lineamount') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  materialTransactionCode: '',
  lineNumber: undefined as number | undefined,
  sourceCode: '',
  sourceLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  transactionUnit: '',
  transactionQuantity: undefined as number | undefined,
  batchNo: '',
  warehouseCode: '',
  locationCode: '',
  targetWarehouseCode: '',
  targetLocationCode: '',
  unitPrice: undefined as number | undefined,
  lineAmount: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'materialTransactionItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.materialTransactionId)
const masterMaterialTransactionId = computed(() => selectedMasterRow.value?.materialTransactionId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaterialTransactionItemId(record: MaterialTransactionItem | Record<string, unknown>): string {
  return String((record as MaterialTransactionItem)?.[entityIdName] ?? '')
}

function getMaterialTransactionItemField(record: MaterialTransactionItem | Record<string, unknown>, field: string): unknown {
  return (record as MaterialTransactionItem)?.[field as keyof MaterialTransactionItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'materialTransactionItemId',
    key: 'materialTransactionItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'materialTransactionItemId') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.materialtransactioncode'),
    dataIndex: 'materialTransactionCode',
    key: 'materialTransactionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'materialTransactionCode') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'sourceCode') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.sourcelinenumber'),
    dataIndex: 'sourceLineNumber',
    key: 'sourceLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'sourceLineNumber') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: t('entity.materialtransactionitem.transactionunit'),
    dataIndex: 'transactionUnit',
    key: 'transactionUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialTransactionItem }) =>
      String(getMaterialTransactionItemField(record, 'transactionUnit') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:materialtransaction:update',
        onClick: (record: MaterialTransactionItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:materialtransaction:delete',
        onClick: (record: MaterialTransactionItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialTransactionItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialTransactionItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMaterialTransactionItemId(selectedRow.value) === getMaterialTransactionItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialTransactionItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaterialTransactionItem) {
  const key = getMaterialTransactionItemId(record)
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
 * @returns {MaterialTransactionItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialTransactionItemQuery>): MaterialTransactionItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialTransactionItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    materialTransactionId: masterMaterialTransactionId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialTransactionItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('materialTransactionCode', form.materialTransactionCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('sourceCode', form.sourceCode)
  if (form.sourceLineNumber !== undefined && form.sourceLineNumber !== null) {
    query.sourceLineNumber = form.sourceLineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('transactionUnit', form.transactionUnit)
  if (form.transactionQuantity !== undefined && form.transactionQuantity !== null) {
    query.transactionQuantity = form.transactionQuantity
  }
  assignTrimmed('batchNo', form.batchNo)
  assignTrimmed('warehouseCode', form.warehouseCode)
  assignTrimmed('locationCode', form.locationCode)
  assignTrimmed('targetWarehouseCode', form.targetWarehouseCode)
  assignTrimmed('targetLocationCode', form.targetLocationCode)
  if (form.unitPrice !== undefined && form.unitPrice !== null) {
    query.unitPrice = form.unitPrice
  }
  if (form.lineAmount !== undefined && form.lineAmount !== null) {
    query.lineAmount = form.lineAmount
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
    const res = await getMaterialTransactionItemList(buildListQuery())
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
watch(masterMaterialTransactionId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.materialtransactionitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaterialTransactionItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.materialtransactionitem._self') })
  formLoading.value = true
  try {
    const detail = await getMaterialTransactionItemById(getMaterialTransactionItemId(record))
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
      entity: t('entity.materialtransactionitem._self'),
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
    const id = formData.value?.materialTransactionItemId
    if (id) {
      await updateMaterialTransactionItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.materialtransactionitem._self') }))
    } else {
      await createMaterialTransactionItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.materialtransactionitem._self') }))
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

async function handleDeleteOne(record: MaterialTransactionItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.materialtransactionitem._self'),
      name: t('common.tip.this.target', { target: t('entity.materialtransactionitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialTransactionItemById(getMaterialTransactionItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.materialtransactionitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.materialtransactionitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.materialtransactionitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaterialTransactionItemId(r)).filter(Boolean)
      await deleteMaterialTransactionItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.materialtransactionitem._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaterialTransactionItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaterialTransactionItem(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

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
    const exportMeta = await exportMaterialTransactionItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.materialtransactionitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.materialtransactionitem._self') }))
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
