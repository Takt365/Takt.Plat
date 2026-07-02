<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：work-order-material-panel.vue -->
<!-- 功能描述：维护工单实体主表实体右侧明细 maintenanceWorkOrderMaterial 独立 CRUD（按主表选中 maintenanceWorkOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="work-order-material-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.maintenanceworkordermaterial._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:maintenance:work:order:create"
      update-permission="logistics:maintenance:work:order:update"
      delete-permission="logistics:maintenance:work:order:delete"
      import-permission="logistics:maintenance:work:order:import"
      export-permission="logistics:maintenance:work:order:export"
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
    <div class="work-order-material-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaintenanceWorkOrderMaterialId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="maintenanceWorkOrderMaterialId"
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
      <MaintenanceWorkOrderMaterialForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaintenanceWorkOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-maintenance-work-order-work-order-material"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.workordercode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.workordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialId')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.materialid')">
        <a-input
          v-model:value="advancedQueryForm.materialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredQuantity')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.requiredquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.requiredQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.requiredquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issuedQuantity')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.issuedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.issuedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.issuedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.materialunit')">
        <a-input
          v-model:value="advancedQueryForm.materialUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitPrice')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.unitprice')">
        <a-input-number
          v-model:value="advancedQueryForm.unitPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.unitprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('amount')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.amount')">
        <a-input-number
          v-model:value="advancedQueryForm.amount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.amount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.warehousecode')">
        <a-input
          v-model:value="advancedQueryForm.warehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.warehousecode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageLocation')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.storagelocation')">
        <a-input
          v-model:value="advancedQueryForm.storageLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.storagelocation') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueStatus')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.issuestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.issueStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.issuestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueTimeStart')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.issuetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.issueTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.issuetimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueTimeEnd')">
      <a-form-item :label="t('entity.maintenanceworkordermaterial.issuetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.issueTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.issuetimeend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.maintenanceworkordermaterial._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenanceworkordermaterial._self"
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
      id-column-key="maintenanceWorkOrderMaterialId"
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
 * 维护工单实体子表 maintenanceWorkOrderMaterial 右栏面板
 * @module views/logistics/maintenance/work-order/components
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
import MaintenanceWorkOrderMaterialForm from './work-order-material-form.vue'
import { useMaintenanceWorkOrderMasterContext } from '../composables/use-work-order-master-context'
import {
  getMaintenanceWorkOrderMaterialList,
  getMaintenanceWorkOrderMaterialById,
  createMaintenanceWorkOrderMaterial,
  updateMaintenanceWorkOrderMaterial,
  deleteMaintenanceWorkOrderMaterialById,
  deleteMaintenanceWorkOrderMaterialBatch,
  getMaintenanceWorkOrderMaterialTemplate,
  importMaintenanceWorkOrderMaterial,
  exportMaintenanceWorkOrderMaterial,
} from '@/api/logistics/maintenance/work-order-material'
import type { MaintenanceWorkOrderMaterial, MaintenanceWorkOrderMaterialQuery } from '@/types/logistics/maintenance/work-order-material'

const { t } = useI18n()
const { selectedMasterRow } = useMaintenanceWorkOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceWorkOrderMaterial')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenanceworkordermaterial._self') }),
)

const loading = ref(false)
const dataSource = ref<MaintenanceWorkOrderMaterial[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaintenanceWorkOrderMaterial | null>(null)
const selectedRows = ref<MaintenanceWorkOrderMaterial[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaintenanceWorkOrderMaterial>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  workOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  materialName: '',
  requiredQuantity: undefined as number | undefined,
  issuedQuantity: undefined as number | undefined,
  materialUnit: '',
  unitPrice: undefined as number | undefined,
  amount: undefined as number | undefined,
  warehouseCode: '',
  storageLocation: '',
  issueStatus: undefined as number | undefined,
  issueTimeStart: '',
  issueTimeEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'workOrderCode', label: t('entity.maintenanceworkordermaterial.workordercode') },
  { key: 'lineNumber', label: t('entity.maintenanceworkordermaterial.linenumber') },
  { key: 'materialId', label: t('entity.maintenanceworkordermaterial.materialid') },
  { key: 'materialCode', label: t('entity.maintenanceworkordermaterial.materialcode') },
  { key: 'materialName', label: t('entity.maintenanceworkordermaterial.materialname') },
  { key: 'requiredQuantity', label: t('entity.maintenanceworkordermaterial.requiredquantity') },
  { key: 'issuedQuantity', label: t('entity.maintenanceworkordermaterial.issuedquantity') },
  { key: 'materialUnit', label: t('entity.maintenanceworkordermaterial.materialunit') },
  { key: 'unitPrice', label: t('entity.maintenanceworkordermaterial.unitprice') },
  { key: 'amount', label: t('entity.maintenanceworkordermaterial.amount') },
  { key: 'warehouseCode', label: t('entity.maintenanceworkordermaterial.warehousecode') },
  { key: 'storageLocation', label: t('entity.maintenanceworkordermaterial.storagelocation') },
  { key: 'issueStatus', label: t('entity.maintenanceworkordermaterial.issuestatus') },
  { key: 'issueTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkordermaterial.issuetime')) },
  { key: 'issueTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenanceworkordermaterial.issuetime')) },
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
  workOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  materialName: '',
  requiredQuantity: undefined as number | undefined,
  issuedQuantity: undefined as number | undefined,
  materialUnit: '',
  unitPrice: undefined as number | undefined,
  amount: undefined as number | undefined,
  warehouseCode: '',
  storageLocation: '',
  issueStatus: undefined as number | undefined,
  issueTimeStart: '',
  issueTimeEnd: '',
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

const entityIdName = 'maintenanceWorkOrderMaterialId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.maintenanceWorkOrderId)
const masterMaintenanceWorkOrderId = computed(() => selectedMasterRow.value?.maintenanceWorkOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaintenanceWorkOrderMaterialId(record: MaintenanceWorkOrderMaterial | Record<string, unknown>): string {
  return String((record as MaintenanceWorkOrderMaterial)?.[entityIdName] ?? '')
}

function getMaintenanceWorkOrderMaterialField(record: MaintenanceWorkOrderMaterial | Record<string, unknown>, field: string): unknown {
  return (record as MaintenanceWorkOrderMaterial)?.[field as keyof MaintenanceWorkOrderMaterial]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceWorkOrderMaterialId',
    key: 'maintenanceWorkOrderMaterialId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'maintenanceWorkOrderMaterialId') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'workOrderCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.materialid'),
    dataIndex: 'materialId',
    key: 'materialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'materialId') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.requiredquantity'),
    dataIndex: 'requiredQuantity',
    key: 'requiredQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'requiredQuantity') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.issuedquantity'),
    dataIndex: 'issuedQuantity',
    key: 'issuedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'issuedQuantity') ?? ''),
  },
  {
    title: t('entity.maintenanceworkordermaterial.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceWorkOrderMaterial }) =>
      String(getMaintenanceWorkOrderMaterialField(record, 'materialUnit') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:work:order:update',
        onClick: (record: MaintenanceWorkOrderMaterial) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:work:order:delete',
        onClick: (record: MaintenanceWorkOrderMaterial) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceWorkOrderMaterial[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaintenanceWorkOrderMaterial, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMaintenanceWorkOrderMaterialId(selectedRow.value) === getMaintenanceWorkOrderMaterialId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceWorkOrderMaterial[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaintenanceWorkOrderMaterial) {
  const key = getMaintenanceWorkOrderMaterialId(record)
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
 * @returns {MaintenanceWorkOrderMaterialQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaintenanceWorkOrderMaterialQuery>): MaintenanceWorkOrderMaterialQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaintenanceWorkOrderMaterialQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    maintenanceWorkOrderId: masterMaintenanceWorkOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaintenanceWorkOrderMaterialQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('workOrderCode', form.workOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialId', form.materialId)
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  if (form.requiredQuantity !== undefined && form.requiredQuantity !== null) {
    query.requiredQuantity = form.requiredQuantity
  }
  if (form.issuedQuantity !== undefined && form.issuedQuantity !== null) {
    query.issuedQuantity = form.issuedQuantity
  }
  assignTrimmed('materialUnit', form.materialUnit)
  if (form.unitPrice !== undefined && form.unitPrice !== null) {
    query.unitPrice = form.unitPrice
  }
  if (form.amount !== undefined && form.amount !== null) {
    query.amount = form.amount
  }
  assignTrimmed('warehouseCode', form.warehouseCode)
  assignTrimmed('storageLocation', form.storageLocation)
  if (form.issueStatus !== undefined && form.issueStatus !== null) {
    query.issueStatus = form.issueStatus
  }
  assignTrimmed('issueTimeStart', form.issueTimeStart)
  assignTrimmed('issueTimeEnd', form.issueTimeEnd)
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
    const res = await getMaintenanceWorkOrderMaterialList(buildListQuery())
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
watch(masterMaintenanceWorkOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenanceworkordermaterial._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaintenanceWorkOrderMaterial) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenanceworkordermaterial._self') })
  formLoading.value = true
  try {
    const detail = await getMaintenanceWorkOrderMaterialById(getMaintenanceWorkOrderMaterialId(record))
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
      entity: t('entity.maintenanceworkordermaterial._self'),
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
    const id = formData.value?.maintenanceWorkOrderMaterialId
    if (id) {
      await updateMaintenanceWorkOrderMaterial(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.maintenanceworkordermaterial._self') }))
    } else {
      await createMaintenanceWorkOrderMaterial(payload)
      message.success(t('common.feedback.created', { target: t('entity.maintenanceworkordermaterial._self') }))
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

async function handleDeleteOne(record: MaintenanceWorkOrderMaterial) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.maintenanceworkordermaterial._self'),
      name: t('common.tip.this.target', { target: t('entity.maintenanceworkordermaterial._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceWorkOrderMaterialById(getMaintenanceWorkOrderMaterialId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkordermaterial._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.maintenanceworkordermaterial._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.maintenanceworkordermaterial._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaintenanceWorkOrderMaterialId(r)).filter(Boolean)
      await deleteMaintenanceWorkOrderMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenanceworkordermaterial._self') }))
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
  const res = await getMaintenanceWorkOrderMaterialTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceWorkOrderMaterial(file, sheetName)
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
    const exportMeta = await exportMaintenanceWorkOrderMaterial(
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenanceworkordermaterial._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.maintenanceworkordermaterial._self') }))
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
