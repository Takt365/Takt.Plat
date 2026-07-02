<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：bill-of-material-item-panel.vue -->
<!-- 功能描述：Takt物料清单实体主表实体右侧明细 billOfMaterialItem 独立 CRUD（按主表选中 billOfMaterialId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="bill-of-material-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.billofmaterialitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:bill:of:material:create"
      update-permission="logistics:manufacturing:bom:bill:of:material:update"
      delete-permission="logistics:manufacturing:bom:bill:of:material:delete"
      import-permission="logistics:manufacturing:bom:bill:of:material:import"
      export-permission="logistics:manufacturing:bom:bill:of:material:export"
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
    <div class="bill-of-material-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getBillOfMaterialItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="billOfMaterialItemId"
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
      <BillOfMaterialItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterBillOfMaterialId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-bill-of-material-bill-of-material-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="t('entity.billofmaterialitem.bomcode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.bomcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.billofmaterialitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialId')">
      <a-form-item :label="t('entity.billofmaterialitem.materialid')">
        <a-input
          v-model:value="advancedQueryForm.materialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.billofmaterialitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageQuantity')">
      <a-form-item :label="t('entity.billofmaterialitem.usagequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.usageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.usagequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="t('entity.billofmaterialitem.materialunit')">
        <a-input
          v-model:value="advancedQueryForm.materialUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapRate')">
      <a-form-item :label="t('entity.billofmaterialitem.scraprate')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.scraprate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualUsageQuantity')">
      <a-form-item :label="t('entity.billofmaterialitem.actualusagequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.actualUsageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.actualusagequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operationSeq')">
      <a-form-item :label="t('entity.billofmaterialitem.operationseq')">
        <a-input-number
          v-model:value="advancedQueryForm.operationSeq"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.operationseq') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="t('entity.billofmaterialitem.workcenter')">
        <a-input
          v-model:value="advancedQueryForm.workCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.workcenter') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('position')">
      <a-form-item :label="t('entity.billofmaterialitem.position')">
        <a-input
          v-model:value="advancedQueryForm.position"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.position') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteGroup')">
      <a-form-item :label="t('entity.billofmaterialitem.substitutegroup')">
        <a-input
          v-model:value="advancedQueryForm.substituteGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutegroup') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substitutePriority')">
      <a-form-item :label="t('entity.billofmaterialitem.substitutepriority')">
        <a-input-number
          v-model:value="advancedQueryForm.substitutePriority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutepriority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOptional')">
      <a-form-item :label="t('entity.billofmaterialitem.isoptional')">
        <a-input-number
          v-model:value="advancedQueryForm.isOptional"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.isoptional') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPhantom')">
      <a-form-item :label="t('entity.billofmaterialitem.isphantom')">
        <a-input-number
          v-model:value="advancedQueryForm.isPhantom"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.isphantom') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.billofmaterialitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.billofmaterialitem._self"
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
      id-column-key="billOfMaterialItemId"
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
 * Takt物料清单实体子表 billOfMaterialItem 右栏面板
 * @module views/logistics/manufacturing/bom/bill-of-material/components
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
import BillOfMaterialItemForm from './bill-of-material-item-form.vue'
import { useBillOfMaterialMasterContext } from '../composables/use-bill-of-material-master-context'
import {
  getBillOfMaterialItemList,
  getBillOfMaterialItemById,
  createBillOfMaterialItem,
  updateBillOfMaterialItem,
  deleteBillOfMaterialItemById,
  deleteBillOfMaterialItemBatch,
  getBillOfMaterialItemTemplate,
  importBillOfMaterialItem,
  exportBillOfMaterialItem,
} from '@/api/logistics/manufacturing/bom/bill-of-material-item'
import type { BillOfMaterialItem, BillOfMaterialItemQuery } from '@/types/logistics/manufacturing/bom/bill-of-material-item'

const { t } = useI18n()
const { selectedMasterRow } = useBillOfMaterialMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterialItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.billofmaterialitem._self') }),
)

const loading = ref(false)
const dataSource = ref<BillOfMaterialItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<BillOfMaterialItem | null>(null)
const selectedRows = ref<BillOfMaterialItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<BillOfMaterialItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'bomCode', label: t('entity.billofmaterialitem.bomcode') },
  { key: 'lineNumber', label: t('entity.billofmaterialitem.linenumber') },
  { key: 'materialId', label: t('entity.billofmaterialitem.materialid') },
  { key: 'materialCode', label: t('entity.billofmaterialitem.materialcode') },
  { key: 'usageQuantity', label: t('entity.billofmaterialitem.usagequantity') },
  { key: 'materialUnit', label: t('entity.billofmaterialitem.materialunit') },
  { key: 'scrapRate', label: t('entity.billofmaterialitem.scraprate') },
  { key: 'actualUsageQuantity', label: t('entity.billofmaterialitem.actualusagequantity') },
  { key: 'operationSeq', label: t('entity.billofmaterialitem.operationseq') },
  { key: 'workCenter', label: t('entity.billofmaterialitem.workcenter') },
  { key: 'position', label: t('entity.billofmaterialitem.position') },
  { key: 'substituteGroup', label: t('entity.billofmaterialitem.substitutegroup') },
  { key: 'substitutePriority', label: t('entity.billofmaterialitem.substitutepriority') },
  { key: 'isOptional', label: t('entity.billofmaterialitem.isoptional') },
  { key: 'isPhantom', label: t('entity.billofmaterialitem.isphantom') },
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
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
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

const entityIdName = 'billOfMaterialItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.billOfMaterialId)
const masterBillOfMaterialId = computed(() => selectedMasterRow.value?.billOfMaterialId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getBillOfMaterialItemId(record: BillOfMaterialItem | Record<string, unknown>): string {
  return String((record as BillOfMaterialItem)?.[entityIdName] ?? '')
}

function getBillOfMaterialItemField(record: BillOfMaterialItem | Record<string, unknown>, field: string): unknown {
  return (record as BillOfMaterialItem)?.[field as keyof BillOfMaterialItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'billOfMaterialItemId',
    key: 'billOfMaterialItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'billOfMaterialItemId') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'bomCode') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.materialid'),
    dataIndex: 'materialId',
    key: 'materialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'materialId') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.usagequantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'usageQuantity') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'materialUnit') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.scraprate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'scrapRate') ?? ''),
  },
  {
    title: t('entity.billofmaterialitem.actualusagequantity'),
    dataIndex: 'actualUsageQuantity',
    key: 'actualUsageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialItem }) =>
      String(getBillOfMaterialItemField(record, 'actualUsageQuantity') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:update',
        onClick: (record: BillOfMaterialItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:delete',
        onClick: (record: BillOfMaterialItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterialItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BillOfMaterialItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getBillOfMaterialItemId(selectedRow.value) === getBillOfMaterialItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BillOfMaterialItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: BillOfMaterialItem) {
  const key = getBillOfMaterialItemId(record)
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
 * @returns {BillOfMaterialItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialItemQuery>): BillOfMaterialItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    billOfMaterialId: masterBillOfMaterialId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('bomCode', form.bomCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialId', form.materialId)
  assignTrimmed('materialCode', form.materialCode)
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    query.usageQuantity = form.usageQuantity
  }
  assignTrimmed('materialUnit', form.materialUnit)
  if (form.scrapRate !== undefined && form.scrapRate !== null) {
    query.scrapRate = form.scrapRate
  }
  if (form.actualUsageQuantity !== undefined && form.actualUsageQuantity !== null) {
    query.actualUsageQuantity = form.actualUsageQuantity
  }
  if (form.operationSeq !== undefined && form.operationSeq !== null) {
    query.operationSeq = form.operationSeq
  }
  assignTrimmed('workCenter', form.workCenter)
  assignTrimmed('position', form.position)
  assignTrimmed('substituteGroup', form.substituteGroup)
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    query.substitutePriority = form.substitutePriority
  }
  if (form.isOptional !== undefined && form.isOptional !== null) {
    query.isOptional = form.isOptional
  }
  if (form.isPhantom !== undefined && form.isPhantom !== null) {
    query.isPhantom = form.isPhantom
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
    const res = await getBillOfMaterialItemList(buildListQuery())
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
watch(masterBillOfMaterialId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billofmaterialitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: BillOfMaterialItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billofmaterialitem._self') })
  formLoading.value = true
  try {
    const detail = await getBillOfMaterialItemById(getBillOfMaterialItemId(record))
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
      entity: t('entity.billofmaterialitem._self'),
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
    const id = formData.value?.billOfMaterialItemId
    if (id) {
      await updateBillOfMaterialItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.billofmaterialitem._self') }))
    } else {
      await createBillOfMaterialItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.billofmaterialitem._self') }))
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

async function handleDeleteOne(record: BillOfMaterialItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.billofmaterialitem._self'),
      name: t('common.tip.this.target', { target: t('entity.billofmaterialitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialItemById(getBillOfMaterialItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.billofmaterialitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.billofmaterialitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getBillOfMaterialItemId(r)).filter(Boolean)
      await deleteBillOfMaterialItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialitem._self') }))
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
  const res = await getBillOfMaterialItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importBillOfMaterialItem(file, sheetName)
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
    const exportMeta = await exportBillOfMaterialItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.billofmaterialitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.billofmaterialitem._self') }))
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
