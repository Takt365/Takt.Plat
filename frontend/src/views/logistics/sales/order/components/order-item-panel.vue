<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/order/components -->
<!-- 文件名称：order-item-panel.vue -->
<!-- 功能描述：Takt采购订单实体主表实体右侧明细 purchaseOrderItem 独立 CRUD（按主表选中 purchaseOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="order-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.purchaseorderitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:sales:order:create"
      update-permission="logistics:sales:order:update"
      delete-permission="logistics:sales:order:delete"
      import-permission="logistics:sales:order:import"
      export-permission="logistics:sales:order:export"
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
    <div class="order-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPurchaseOrderItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchaseOrderItemId"
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
      <PurchaseOrderItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchaseOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-sales-order-order-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="t('entity.purchaseorderitem.purchaseordercode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.purchaseordercode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.purchaseorderitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestCode')">
      <a-form-item :label="t('entity.purchaseorderitem.requestcode')">
        <a-input
          v-model:value="advancedQueryForm.requestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.requestcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestLineNumber')">
      <a-form-item :label="t('entity.purchaseorderitem.requestlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.requestLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.requestlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.purchaseorderitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.purchaseorderitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.purchaseorderitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseUnit')">
      <a-form-item :label="t('entity.purchaseorderitem.purchaseunit')">
        <a-input
          v-model:value="advancedQueryForm.purchaseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.purchaseunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderQuantity')">
      <a-form-item :label="t('entity.purchaseorderitem.orderquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.orderQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.orderquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedQuantity')">
      <a-form-item :label="t('entity.purchaseorderitem.receivedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.receivedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitPrice')">
      <a-form-item :label="t('entity.purchaseorderitem.unitprice')">
        <a-input-number
          v-model:value="advancedQueryForm.unitPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.unitprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountRate')">
      <a-form-item :label="t('entity.purchaseorderitem.discountrate')">
        <a-input-number
          v-model:value="advancedQueryForm.discountRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.discountrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.purchaseorderitem.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="t('entity.purchaseorderitem.taxrate')">
        <a-input-number
          v-model:value="advancedQueryForm.taxRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.taxrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.purchaseorderitem.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subtotalAmount')">
      <a-form-item :label="t('entity.purchaseorderitem.subtotalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.subtotalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.subtotalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryStatus')">
      <a-form-item :label="t('entity.purchaseorderitem.deliverystatus')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorderitem.deliverystatus') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.purchaseorderitem.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseorderitem.extfield') })"
          :rows="2"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseorderitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseorderitem._self"
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
      id-column-key="purchaseOrderItemId"
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
 * Takt采购订单实体子表 purchaseOrderItem 右栏面板
 * @module views/logistics/sales/order/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import PurchaseOrderItemForm from './order-item-form.vue'
import { usePurchaseOrderMasterContext } from '../composables/use-order-master-context'
import {
  getPurchaseOrderItemList,
  getPurchaseOrderItemById,
  createPurchaseOrderItem,
  updatePurchaseOrderItem,
  deletePurchaseOrderItemById,
  deletePurchaseOrderItemBatch,
  getPurchaseOrderItemTemplate,
  importPurchaseOrderItem,
  exportPurchaseOrderItem,
} from '@/api/logistics/procurement/order-item'
import type { PurchaseOrderItem, PurchaseOrderItemQuery } from '@/types/logistics/procurement/order-item'

const { t } = useI18n()
const { selectedMasterRow } = usePurchaseOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseOrderItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseorderitem._self') }),
)

const loading = ref(false)
const dataSource = ref<PurchaseOrderItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchaseOrderItem | null>(null)
const selectedRows = ref<PurchaseOrderItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseOrderItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchaseOrderCode: '',
  lineNumber: undefined as number | undefined,
  requestCode: '',
  requestLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  purchaseUnit: '',
  orderQuantity: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  unitPrice: undefined as number | undefined,
  discountRate: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  subtotalAmount: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'purchaseOrderCode', label: t('entity.purchaseorderitem.purchaseordercode') },
  { key: 'lineNumber', label: t('entity.purchaseorderitem.linenumber') },
  { key: 'requestCode', label: t('entity.purchaseorderitem.requestcode') },
  { key: 'requestLineNumber', label: t('entity.purchaseorderitem.requestlinenumber') },
  { key: 'materialCode', label: t('entity.purchaseorderitem.materialcode') },
  { key: 'materialName', label: t('entity.purchaseorderitem.materialname') },
  { key: 'materialSpecification', label: t('entity.purchaseorderitem.materialspecification') },
  { key: 'purchaseUnit', label: t('entity.purchaseorderitem.purchaseunit') },
  { key: 'orderQuantity', label: t('entity.purchaseorderitem.orderquantity') },
  { key: 'receivedQuantity', label: t('entity.purchaseorderitem.receivedquantity') },
  { key: 'unitPrice', label: t('entity.purchaseorderitem.unitprice') },
  { key: 'discountRate', label: t('entity.purchaseorderitem.discountrate') },
  { key: 'discountAmount', label: t('entity.purchaseorderitem.discountamount') },
  { key: 'taxRate', label: t('entity.purchaseorderitem.taxrate') },
  { key: 'taxAmount', label: t('entity.purchaseorderitem.taxamount') },
  { key: 'subtotalAmount', label: t('entity.purchaseorderitem.subtotalamount') },
  { key: 'deliveryStatus', label: t('entity.purchaseorderitem.deliverystatus') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.purchaseorderitem.extfield') },
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
  purchaseOrderCode: '',
  lineNumber: undefined as number | undefined,
  requestCode: '',
  requestLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  purchaseUnit: '',
  orderQuantity: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  unitPrice: undefined as number | undefined,
  discountRate: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  subtotalAmount: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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

const entityIdName = 'purchaseOrderItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.purchaseOrderId)
const masterPurchaseOrderId = computed(() => selectedMasterRow.value?.purchaseOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchaseOrderItemId(record: PurchaseOrderItem | Record<string, unknown>): string {
  return String((record as PurchaseOrderItem)?.[entityIdName] ?? '')
}

function getPurchaseOrderItemField(record: PurchaseOrderItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchaseOrderItem)?.[field as keyof PurchaseOrderItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseOrderItemId',
    key: 'purchaseOrderItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'purchaseOrderItemId') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.purchaseordercode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'purchaseOrderCode') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.requestcode'),
    dataIndex: 'requestCode',
    key: 'requestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'requestCode') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.requestlinenumber'),
    dataIndex: 'requestLineNumber',
    key: 'requestLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'requestLineNumber') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: t('entity.purchaseorderitem.purchaseunit'),
    dataIndex: 'purchaseUnit',
    key: 'purchaseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseOrderItem }) =>
      String(getPurchaseOrderItemField(record, 'purchaseUnit') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:order:update',
        onClick: (record: PurchaseOrderItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:order:delete',
        onClick: (record: PurchaseOrderItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseOrderItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseOrderItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchaseOrderItemId(selectedRow.value) === getPurchaseOrderItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseOrderItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchaseOrderItem) {
  const key = getPurchaseOrderItemId(record)
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
 * @returns {PurchaseOrderItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseOrderItemQuery>): PurchaseOrderItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseOrderItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchaseOrderId: masterPurchaseOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseOrderItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('purchaseOrderCode', form.purchaseOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('requestCode', form.requestCode)
  if (form.requestLineNumber !== undefined && form.requestLineNumber !== null) {
    query.requestLineNumber = form.requestLineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('purchaseUnit', form.purchaseUnit)
  if (form.orderQuantity !== undefined && form.orderQuantity !== null) {
    query.orderQuantity = form.orderQuantity
  }
  if (form.receivedQuantity !== undefined && form.receivedQuantity !== null) {
    query.receivedQuantity = form.receivedQuantity
  }
  if (form.unitPrice !== undefined && form.unitPrice !== null) {
    query.unitPrice = form.unitPrice
  }
  if (form.discountRate !== undefined && form.discountRate !== null) {
    query.discountRate = form.discountRate
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.subtotalAmount !== undefined && form.subtotalAmount !== null) {
    query.subtotalAmount = form.subtotalAmount
  }
  if (form.deliveryStatus !== undefined && form.deliveryStatus !== null) {
    query.deliveryStatus = form.deliveryStatus
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
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
    const res = await getPurchaseOrderItemList(buildListQuery())
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
watch(masterPurchaseOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseorderitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PurchaseOrderItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseorderitem._self') })
  formLoading.value = true
  try {
    const detail = await getPurchaseOrderItemById(getPurchaseOrderItemId(record))
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
      entity: t('entity.purchaseorderitem._self'),
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
    const id = formData.value?.purchaseOrderItemId
    if (id) {
      await updatePurchaseOrderItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseorderitem._self') }))
    } else {
      await createPurchaseOrderItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.purchaseorderitem._self') }))
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

async function handleDeleteOne(record: PurchaseOrderItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.purchaseorderitem._self'),
      name: t('common.tip.this.target', { target: t('entity.purchaseorderitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseOrderItemById(getPurchaseOrderItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseorderitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.purchaseorderitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.purchaseorderitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPurchaseOrderItemId(r)).filter(Boolean)
      await deletePurchaseOrderItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseorderitem._self') }))
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
  const res = await getPurchaseOrderItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseOrderItem(file, sheetName)
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
    const exportMeta = await exportPurchaseOrderItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseorderitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.purchaseorderitem._self') }))
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
