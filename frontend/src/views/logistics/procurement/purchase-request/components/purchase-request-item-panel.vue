<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-request/components -->
<!-- 文件名称：purchase-request-item-panel.vue -->
<!-- 功能描述：Takt采购申请实体主表实体右侧明细 purchaseRequestItem 独立 CRUD（按主表选中 purchaseRequestId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="purchase-request-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.purchaserequestitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:procurement:purchase:request:create"
      update-permission="logistics:procurement:purchase:request:update"
      delete-permission="logistics:procurement:purchase:request:delete"
      import-permission="logistics:procurement:purchase:request:import"
      export-permission="logistics:procurement:purchase:request:export"
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
    <div class="purchase-request-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPurchaseRequestItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchaseRequestItemId"
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
      <PurchaseRequestItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchaseRequestId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-procurement-purchase-request-purchase-request-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('purchaseRequestCode')">
      <a-form-item :label="t('entity.purchaserequestitem.purchaserequestcode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.purchaserequestcode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.purchaserequestitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allocationCategory')">
      <a-form-item :label="t('entity.purchaserequestitem.allocationcategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.allocationCategory"
          dict-type="logistics_allocation_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaserequestitem.allocationcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.purchaserequestitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.purchaserequestitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.purchaserequestitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestUnit')">
      <a-form-item :label="t('entity.purchaserequestitem.requestunit')">
        <a-input
          v-model:value="advancedQueryForm.requestUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.requestunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestQuantity')">
      <a-form-item :label="t('entity.purchaserequestitem.requestquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.requestQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.requestquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="t('entity.purchaserequestitem.convertedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.convertedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedUnitPrice')">
      <a-form-item :label="t('entity.purchaserequestitem.estimatedunitprice')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedUnitPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.estimatedunitprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedAmount')">
      <a-form-item :label="t('entity.purchaserequestitem.estimatedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.estimatedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceSupplierCode')">
      <a-form-item :label="t('entity.purchaserequestitem.referencesuppliercode')">
        <a-input
          v-model:value="advancedQueryForm.referenceSupplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.referencesuppliercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceSupplierName')">
      <a-form-item :label="t('entity.purchaserequestitem.referencesuppliername')">
        <a-input
          v-model:value="advancedQueryForm.referenceSupplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaserequestitem.referencesuppliername') })"
          show-count
          :maxlength="20"
          allow-clear
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaserequestitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaserequestitem._self"
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
      id-column-key="purchaseRequestItemId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购申请实体子表 purchaseRequestItem 右栏面板
 * @module views/logistics/procurement/purchase-request/components
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
import PurchaseRequestItemForm from './purchase-request-item-form.vue'
import { usePurchaseRequestMasterContext } from '../composables/use-purchase-request-master-context'
import {
  getPurchaseRequestItemList,
  getPurchaseRequestItemById,
  createPurchaseRequestItem,
  updatePurchaseRequestItem,
  deletePurchaseRequestItemById,
  deletePurchaseRequestItemBatch,
  getPurchaseRequestItemTemplate,
  importPurchaseRequestItem,
  exportPurchaseRequestItem,
} from '@/api/logistics/procurement/purchase-request-item'
import type { PurchaseRequestItem, PurchaseRequestItemQuery } from '@/types/logistics/procurement/purchase-request-item'

const { t } = useI18n()
const { selectedMasterRow } = usePurchaseRequestMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseRequestItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaserequestitem._self') }),
)

const loading = ref(false)
const dataSource = ref<PurchaseRequestItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchaseRequestItem | null>(null)
const selectedRows = ref<PurchaseRequestItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseRequestItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchaseRequestCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  requestUnit: '',
  requestQuantity: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  estimatedUnitPrice: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
  referenceSupplierCode: '',
  referenceSupplierName: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'purchaseRequestCode', label: t('entity.purchaserequestitem.purchaserequestcode') },
  { key: 'lineNumber', label: t('entity.purchaserequestitem.linenumber') },
  { key: 'allocationCategory', label: t('entity.purchaserequestitem.allocationcategory') },
  { key: 'materialCode', label: t('entity.purchaserequestitem.materialcode') },
  { key: 'materialName', label: t('entity.purchaserequestitem.materialname') },
  { key: 'materialSpecification', label: t('entity.purchaserequestitem.materialspecification') },
  { key: 'requestUnit', label: t('entity.purchaserequestitem.requestunit') },
  { key: 'requestQuantity', label: t('entity.purchaserequestitem.requestquantity') },
  { key: 'convertedQuantity', label: t('entity.purchaserequestitem.convertedquantity') },
  { key: 'estimatedUnitPrice', label: t('entity.purchaserequestitem.estimatedunitprice') },
  { key: 'estimatedAmount', label: t('entity.purchaserequestitem.estimatedamount') },
  { key: 'referenceSupplierCode', label: t('entity.purchaserequestitem.referencesuppliercode') },
  { key: 'referenceSupplierName', label: t('entity.purchaserequestitem.referencesuppliername') },
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
  purchaseRequestCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  requestUnit: '',
  requestQuantity: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  estimatedUnitPrice: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
  referenceSupplierCode: '',
  referenceSupplierName: '',
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

const entityIdName = 'purchaseRequestItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.purchaseRequestId)
const masterPurchaseRequestId = computed(() => selectedMasterRow.value?.purchaseRequestId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchaseRequestItemId(record: PurchaseRequestItem | Record<string, unknown>): string {
  return String((record as PurchaseRequestItem)?.[entityIdName] ?? '')
}

function getPurchaseRequestItemField(record: PurchaseRequestItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchaseRequestItem)?.[field as keyof PurchaseRequestItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseRequestItemId',
    key: 'purchaseRequestItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'purchaseRequestItemId') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.purchaserequestcode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'purchaseRequestCode') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.allocationcategory'),
    dataIndex: 'allocationCategory',
    key: 'allocationCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'allocationCategory') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.requestunit'),
    dataIndex: 'requestUnit',
    key: 'requestUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'requestUnit') ?? ''),
  },
  {
    title: t('entity.purchaserequestitem.requestquantity'),
    dataIndex: 'requestQuantity',
    key: 'requestQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseRequestItem }) =>
      String(getPurchaseRequestItemField(record, 'requestQuantity') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:request:update',
        onClick: (record: PurchaseRequestItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:request:delete',
        onClick: (record: PurchaseRequestItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseRequestItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseRequestItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPurchaseRequestItemId(selectedRow.value) === getPurchaseRequestItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseRequestItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchaseRequestItem) {
  const key = getPurchaseRequestItemId(record)
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
 * @returns {PurchaseRequestItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseRequestItemQuery>): PurchaseRequestItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseRequestItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchaseRequestId: masterPurchaseRequestId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseRequestItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('purchaseRequestCode', form.purchaseRequestCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('allocationCategory', form.allocationCategory)
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('requestUnit', form.requestUnit)
  if (form.requestQuantity !== undefined && form.requestQuantity !== null) {
    query.requestQuantity = form.requestQuantity
  }
  if (form.convertedQuantity !== undefined && form.convertedQuantity !== null) {
    query.convertedQuantity = form.convertedQuantity
  }
  if (form.estimatedUnitPrice !== undefined && form.estimatedUnitPrice !== null) {
    query.estimatedUnitPrice = form.estimatedUnitPrice
  }
  if (form.estimatedAmount !== undefined && form.estimatedAmount !== null) {
    query.estimatedAmount = form.estimatedAmount
  }
  assignTrimmed('referenceSupplierCode', form.referenceSupplierCode)
  assignTrimmed('referenceSupplierName', form.referenceSupplierName)
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
    const res = await getPurchaseRequestItemList(buildListQuery())
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
watch(masterPurchaseRequestId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaserequestitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PurchaseRequestItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaserequestitem._self') })
  formLoading.value = true
  try {
    const detail = await getPurchaseRequestItemById(getPurchaseRequestItemId(record))
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
      entity: t('entity.purchaserequestitem._self'),
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
    const id = formData.value?.purchaseRequestItemId
    if (id) {
      await updatePurchaseRequestItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.purchaserequestitem._self') }))
    } else {
      await createPurchaseRequestItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.purchaserequestitem._self') }))
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

async function handleDeleteOne(record: PurchaseRequestItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.purchaserequestitem._self'),
      name: t('common.tip.this.target', { target: t('entity.purchaserequestitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseRequestItemById(getPurchaseRequestItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.purchaserequestitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.purchaserequestitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.purchaserequestitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPurchaseRequestItemId(r)).filter(Boolean)
      await deletePurchaseRequestItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaserequestitem._self') }))
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
  const res = await getPurchaseRequestItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseRequestItem(file, sheetName)
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
    const exportMeta = await exportPurchaseRequestItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaserequestitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.purchaserequestitem._self') }))
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
