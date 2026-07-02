<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-inquiry/components -->
<!-- 文件名称：purchase-inquiry-item-panel.vue -->
<!-- 功能描述：采购询价实体主表实体右侧明细 purchaseInquiryItem 独立 CRUD（按主表选中 purchaseInquiryId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="purchase-inquiry-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.purchaseinquiryitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:procurement:purchase:inquiry:create"
      update-permission="logistics:procurement:purchase:inquiry:update"
      delete-permission="logistics:procurement:purchase:inquiry:delete"
      import-permission="logistics:procurement:purchase:inquiry:import"
      export-permission="logistics:procurement:purchase:inquiry:export"
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
    <div class="purchase-inquiry-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPurchaseInquiryItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchaseInquiryItemId"
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
      <PurchaseInquiryItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchaseInquiryId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-procurement-purchase-inquiry-purchase-inquiry-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('purchaseInquiryCode')">
      <a-form-item :label="t('entity.purchaseinquiryitem.purchaseinquirycode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInquiryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.purchaseinquirycode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.purchaseinquiryitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allocationCategory')">
      <a-form-item :label="t('entity.purchaseinquiryitem.allocationcategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.allocationCategory"
          dict-type="logistics_allocation_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.allocationcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.purchaseinquiryitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.purchaseinquiryitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.purchaseinquiryitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryUnit')">
      <a-form-item :label="t('entity.purchaseinquiryitem.inquiryunit')">
        <a-input
          v-model:value="advancedQueryForm.inquiryUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.inquiryunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryQuantity')">
      <a-form-item :label="t('entity.purchaseinquiryitem.inquiryquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.inquiryQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.inquiryquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotedUnitPrice')">
      <a-form-item :label="t('entity.purchaseinquiryitem.quotedunitprice')">
        <a-textarea
          v-model:value="advancedQueryForm.quotedUnitPrice"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseinquiryitem.quotedunitprice') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotedAmount')">
      <a-form-item :label="t('entity.purchaseinquiryitem.quotedamount')">
        <a-textarea
          v-model:value="advancedQueryForm.quotedAmount"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseinquiryitem.quotedamount') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetSupplierCode')">
      <a-form-item :label="t('entity.purchaseinquiryitem.targetsuppliercode')">
        <a-input
          v-model:value="advancedQueryForm.targetSupplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.targetsuppliercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetSupplierName')">
      <a-form-item :label="t('entity.purchaseinquiryitem.targetsuppliername')">
        <a-input
          v-model:value="advancedQueryForm.targetSupplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.targetsuppliername') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseinquiryitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseinquiryitem._self"
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
      id-column-key="purchaseInquiryItemId"
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
 * 采购询价实体子表 purchaseInquiryItem 右栏面板
 * @module views/logistics/procurement/purchase-inquiry/components
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
import PurchaseInquiryItemForm from './purchase-inquiry-item-form.vue'
import { usePurchaseInquiryMasterContext } from '../composables/use-purchase-inquiry-master-context'
import {
  getPurchaseInquiryItemList,
  getPurchaseInquiryItemById,
  createPurchaseInquiryItem,
  updatePurchaseInquiryItem,
  deletePurchaseInquiryItemById,
  deletePurchaseInquiryItemBatch,
  getPurchaseInquiryItemTemplate,
  importPurchaseInquiryItem,
  exportPurchaseInquiryItem,
} from '@/api/logistics/procurement/purchase-inquiry-item'
import type { PurchaseInquiryItem, PurchaseInquiryItemQuery } from '@/types/logistics/procurement/purchase-inquiry-item'

const { t } = useI18n()
const { selectedMasterRow } = usePurchaseInquiryMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseInquiryItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseinquiryitem._self') }),
)

const loading = ref(false)
const dataSource = ref<PurchaseInquiryItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchaseInquiryItem | null>(null)
const selectedRows = ref<PurchaseInquiryItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseInquiryItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchaseInquiryCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  inquiryUnit: '',
  inquiryQuantity: undefined as number | undefined,
  quotedUnitPrice: undefined as number | undefined,
  quotedAmount: undefined as number | undefined,
  targetSupplierCode: '',
  targetSupplierName: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'purchaseInquiryCode', label: t('entity.purchaseinquiryitem.purchaseinquirycode') },
  { key: 'lineNumber', label: t('entity.purchaseinquiryitem.linenumber') },
  { key: 'allocationCategory', label: t('entity.purchaseinquiryitem.allocationcategory') },
  { key: 'materialCode', label: t('entity.purchaseinquiryitem.materialcode') },
  { key: 'materialName', label: t('entity.purchaseinquiryitem.materialname') },
  { key: 'materialSpecification', label: t('entity.purchaseinquiryitem.materialspecification') },
  { key: 'inquiryUnit', label: t('entity.purchaseinquiryitem.inquiryunit') },
  { key: 'inquiryQuantity', label: t('entity.purchaseinquiryitem.inquiryquantity') },
  { key: 'quotedUnitPrice', label: t('entity.purchaseinquiryitem.quotedunitprice') },
  { key: 'quotedAmount', label: t('entity.purchaseinquiryitem.quotedamount') },
  { key: 'targetSupplierCode', label: t('entity.purchaseinquiryitem.targetsuppliercode') },
  { key: 'targetSupplierName', label: t('entity.purchaseinquiryitem.targetsuppliername') },
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
  purchaseInquiryCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  inquiryUnit: '',
  inquiryQuantity: undefined as number | undefined,
  quotedUnitPrice: undefined as number | undefined,
  quotedAmount: undefined as number | undefined,
  targetSupplierCode: '',
  targetSupplierName: '',
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

const entityIdName = 'purchaseInquiryItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.purchaseInquiryId)
const masterPurchaseInquiryId = computed(() => selectedMasterRow.value?.purchaseInquiryId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchaseInquiryItemId(record: PurchaseInquiryItem | Record<string, unknown>): string {
  return String((record as PurchaseInquiryItem)?.[entityIdName] ?? '')
}

function getPurchaseInquiryItemField(record: PurchaseInquiryItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchaseInquiryItem)?.[field as keyof PurchaseInquiryItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseInquiryItemId',
    key: 'purchaseInquiryItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'purchaseInquiryItemId') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.purchaseinquirycode'),
    dataIndex: 'purchaseInquiryCode',
    key: 'purchaseInquiryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'purchaseInquiryCode') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.allocationcategory'),
    dataIndex: 'allocationCategory',
    key: 'allocationCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'allocationCategory') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.inquiryunit'),
    dataIndex: 'inquiryUnit',
    key: 'inquiryUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'inquiryUnit') ?? ''),
  },
  {
    title: t('entity.purchaseinquiryitem.inquiryquantity'),
    dataIndex: 'inquiryQuantity',
    key: 'inquiryQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInquiryItem }) =>
      String(getPurchaseInquiryItemField(record, 'inquiryQuantity') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:inquiry:update',
        onClick: (record: PurchaseInquiryItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:inquiry:delete',
        onClick: (record: PurchaseInquiryItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseInquiryItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseInquiryItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPurchaseInquiryItemId(selectedRow.value) === getPurchaseInquiryItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseInquiryItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchaseInquiryItem) {
  const key = getPurchaseInquiryItemId(record)
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
 * @returns {PurchaseInquiryItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseInquiryItemQuery>): PurchaseInquiryItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseInquiryItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchaseInquiryId: masterPurchaseInquiryId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseInquiryItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('purchaseInquiryCode', form.purchaseInquiryCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('allocationCategory', form.allocationCategory)
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('inquiryUnit', form.inquiryUnit)
  if (form.inquiryQuantity !== undefined && form.inquiryQuantity !== null) {
    query.inquiryQuantity = form.inquiryQuantity
  }
  if (form.quotedUnitPrice !== undefined && form.quotedUnitPrice !== null) {
    query.quotedUnitPrice = form.quotedUnitPrice
  }
  if (form.quotedAmount !== undefined && form.quotedAmount !== null) {
    query.quotedAmount = form.quotedAmount
  }
  assignTrimmed('targetSupplierCode', form.targetSupplierCode)
  assignTrimmed('targetSupplierName', form.targetSupplierName)
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
    const res = await getPurchaseInquiryItemList(buildListQuery())
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
watch(masterPurchaseInquiryId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseinquiryitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PurchaseInquiryItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseinquiryitem._self') })
  formLoading.value = true
  try {
    const detail = await getPurchaseInquiryItemById(getPurchaseInquiryItemId(record))
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
      entity: t('entity.purchaseinquiryitem._self'),
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
    const id = formData.value?.purchaseInquiryItemId
    if (id) {
      await updatePurchaseInquiryItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseinquiryitem._self') }))
    } else {
      await createPurchaseInquiryItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.purchaseinquiryitem._self') }))
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

async function handleDeleteOne(record: PurchaseInquiryItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.purchaseinquiryitem._self'),
      name: t('common.tip.this.target', { target: t('entity.purchaseinquiryitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseInquiryItemById(getPurchaseInquiryItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinquiryitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.purchaseinquiryitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.purchaseinquiryitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPurchaseInquiryItemId(r)).filter(Boolean)
      await deletePurchaseInquiryItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinquiryitem._self') }))
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
  const res = await getPurchaseInquiryItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseInquiryItem(file, sheetName)
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
    const exportMeta = await exportPurchaseInquiryItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseinquiryitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.purchaseinquiryitem._self') }))
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
