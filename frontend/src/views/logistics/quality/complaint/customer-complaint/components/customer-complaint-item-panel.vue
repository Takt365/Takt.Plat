<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint/components -->
<!-- 文件名称：customer-complaint-item-panel.vue -->
<!-- 功能描述：客诉主表实体主表实体右侧明细 customerComplaintItem 独立 CRUD（按主表选中 complaintId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="customer-complaint-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.customercomplaintitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:complaint:customer:create"
      update-permission="logistics:quality:complaint:customer:update"
      delete-permission="logistics:quality:complaint:customer:delete"
      import-permission="logistics:quality:complaint:customer:import"
      export-permission="logistics:quality:complaint:customer:export"
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
    <div class="customer-complaint-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getCustomerComplaintItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="customerComplaintItemId"
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
      <CustomerComplaintItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterCustomerComplaintId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-complaint-customer-complaint-customer-complaint-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('customerComplaintCode')">
      <a-form-item :label="t('entity.customercomplaintitem.customercomplaintcode')">
        <a-input
          v-model:value="advancedQueryForm.customerComplaintCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.customercomplaintcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.customercomplaintitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCode')">
      <a-form-item :label="t('entity.customercomplaintitem.productcode')">
        <a-input
          v-model:value="advancedQueryForm.productCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.productcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productName')">
      <a-form-item :label="t('entity.customercomplaintitem.productname')">
        <a-input
          v-model:value="advancedQueryForm.productName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.productname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.customercomplaintitem.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.batchno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemType')">
      <a-form-item :label="t('entity.customercomplaintitem.itemtype')">
        <a-input-number
          v-model:value="advancedQueryForm.itemType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.itemtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectDescription')">
      <a-form-item :label="t('entity.customercomplaintitem.defectdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.defectDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customercomplaintitem.defectdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectLevel')">
      <a-form-item :label="t('entity.customercomplaintitem.defectlevel')">
        <a-input
          v-model:value="advancedQueryForm.defectLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.defectlevel') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectQuantity')">
      <a-form-item :label="t('entity.customercomplaintitem.defectquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.defectquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectRate')">
      <a-form-item :label="t('entity.customercomplaintitem.defectrate')">
        <a-input-number
          v-model:value="advancedQueryForm.defectRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.defectrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('causeAnalysis')">
      <a-form-item :label="t('entity.customercomplaintitem.causeanalysis')">
        <a-input
          v-model:value="advancedQueryForm.causeAnalysis"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.causeanalysis') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementAction')">
      <a-form-item :label="t('entity.customercomplaintitem.improvementaction')">
        <a-input
          v-model:value="advancedQueryForm.improvementAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.improvementaction') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementResponsible')">
      <a-form-item :label="t('entity.customercomplaintitem.improvementresponsible')">
        <a-input
          v-model:value="advancedQueryForm.improvementResponsible"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.improvementresponsible') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateStart')">
      <a-form-item :label="t('entity.customercomplaintitem.plannedcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaintitem.plannedcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateEnd')">
      <a-form-item :label="t('entity.customercomplaintitem.plannedcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaintitem.plannedcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateStart')">
      <a-form-item :label="t('entity.customercomplaintitem.actualcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaintitem.actualcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateEnd')">
      <a-form-item :label="t('entity.customercomplaintitem.actualcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaintitem.actualcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementStatus')">
      <a-form-item :label="t('entity.customercomplaintitem.improvementstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.improvementStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.improvementstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachmentPaths')">
      <a-form-item :label="t('entity.customercomplaintitem.attachmentpaths')">
        <a-input
          v-model:value="advancedQueryForm.attachmentPaths"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.attachmentpaths') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customercomplaintitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customercomplaintitem._self"
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
      id-column-key="customerComplaintItemId"
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
 * 客诉主表实体子表 customerComplaintItem 右栏面板
 * @module views/logistics/quality/complaint/customer-complaint/components
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
import CustomerComplaintItemForm from './customer-complaint-item-form.vue'
import { useCustomerComplaintMasterContext } from '../composables/use-customer-complaint-master-context'
import {
  getCustomerComplaintItemList,
  getCustomerComplaintItemById,
  createCustomerComplaintItem,
  updateCustomerComplaintItem,
  deleteCustomerComplaintItemById,
  deleteCustomerComplaintItemBatch,
  getCustomerComplaintItemTemplate,
  importCustomerComplaintItem,
  exportCustomerComplaintItem,
} from '@/api/logistics/quality/complaint/customer-complaint-item'
import type { CustomerComplaintItem, CustomerComplaintItemQuery } from '@/types/logistics/quality/complaint/customer-complaint-item'

const { t } = useI18n()
const { selectedMasterRow } = useCustomerComplaintMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaintItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customercomplaintitem._self') }),
)

const loading = ref(false)
const dataSource = ref<CustomerComplaintItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<CustomerComplaintItem | null>(null)
const selectedRows = ref<CustomerComplaintItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<CustomerComplaintItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  customerComplaintCode: '',
  lineNumber: undefined as number | undefined,
  productCode: '',
  productName: '',
  batchNo: '',
  itemType: undefined as number | undefined,
  defectDescription: '',
  defectLevel: '',
  defectQuantity: undefined as number | undefined,
  defectRate: undefined as number | undefined,
  causeAnalysis: '',
  improvementAction: '',
  improvementResponsible: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  improvementStatus: undefined as number | undefined,
  attachmentPaths: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'customerComplaintCode', label: t('entity.customercomplaintitem.customercomplaintcode') },
  { key: 'lineNumber', label: t('entity.customercomplaintitem.linenumber') },
  { key: 'productCode', label: t('entity.customercomplaintitem.productcode') },
  { key: 'productName', label: t('entity.customercomplaintitem.productname') },
  { key: 'batchNo', label: t('entity.customercomplaintitem.batchno') },
  { key: 'itemType', label: t('entity.customercomplaintitem.itemtype') },
  { key: 'defectDescription', label: t('entity.customercomplaintitem.defectdescription') },
  { key: 'defectLevel', label: t('entity.customercomplaintitem.defectlevel') },
  { key: 'defectQuantity', label: t('entity.customercomplaintitem.defectquantity') },
  { key: 'defectRate', label: t('entity.customercomplaintitem.defectrate') },
  { key: 'causeAnalysis', label: t('entity.customercomplaintitem.causeanalysis') },
  { key: 'improvementAction', label: t('entity.customercomplaintitem.improvementaction') },
  { key: 'improvementResponsible', label: t('entity.customercomplaintitem.improvementresponsible') },
  { key: 'plannedCompletionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplaintitem.plannedcompletiondate')) },
  { key: 'plannedCompletionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplaintitem.plannedcompletiondate')) },
  { key: 'actualCompletionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplaintitem.actualcompletiondate')) },
  { key: 'actualCompletionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplaintitem.actualcompletiondate')) },
  { key: 'improvementStatus', label: t('entity.customercomplaintitem.improvementstatus') },
  { key: 'attachmentPaths', label: t('entity.customercomplaintitem.attachmentpaths') },
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
  customerComplaintCode: '',
  lineNumber: undefined as number | undefined,
  productCode: '',
  productName: '',
  batchNo: '',
  itemType: undefined as number | undefined,
  defectDescription: '',
  defectLevel: '',
  defectQuantity: undefined as number | undefined,
  defectRate: undefined as number | undefined,
  causeAnalysis: '',
  improvementAction: '',
  improvementResponsible: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  improvementStatus: undefined as number | undefined,
  attachmentPaths: '',
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

const entityIdName = 'customerComplaintItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.customerComplaintId)
const masterCustomerComplaintId = computed(() => selectedMasterRow.value?.customerComplaintId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getCustomerComplaintItemId(record: CustomerComplaintItem | Record<string, unknown>): string {
  return String((record as CustomerComplaintItem)?.[entityIdName] ?? '')
}

function getCustomerComplaintItemField(record: CustomerComplaintItem | Record<string, unknown>, field: string): unknown {
  return (record as CustomerComplaintItem)?.[field as keyof CustomerComplaintItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerComplaintItemId',
    key: 'customerComplaintItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'customerComplaintItemId') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.complaintid'),
    dataIndex: 'complaintId',
    key: 'complaintId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'complaintId') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.customercomplaintcode'),
    dataIndex: 'customerComplaintCode',
    key: 'customerComplaintCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'customerComplaintCode') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.productcode'),
    dataIndex: 'productCode',
    key: 'productCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'productCode') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.productname'),
    dataIndex: 'productName',
    key: 'productName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'productName') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'batchNo') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'itemType') ?? ''),
  },
  {
    title: t('entity.customercomplaintitem.defectdescription'),
    dataIndex: 'defectDescription',
    key: 'defectDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerComplaintItem }) =>
      String(getCustomerComplaintItemField(record, 'defectDescription') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customer:update',
        onClick: (record: CustomerComplaintItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:delete',
        onClick: (record: CustomerComplaintItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerComplaintItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerComplaintItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getCustomerComplaintItemId(selectedRow.value) === getCustomerComplaintItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerComplaintItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: CustomerComplaintItem) {
  const key = getCustomerComplaintItemId(record)
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
 * @returns {CustomerComplaintItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerComplaintItemQuery>): CustomerComplaintItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerComplaintItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    complaintId: masterCustomerComplaintId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerComplaintItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('customerComplaintCode', form.customerComplaintCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('productCode', form.productCode)
  assignTrimmed('productName', form.productName)
  assignTrimmed('batchNo', form.batchNo)
  if (form.itemType !== undefined && form.itemType !== null) {
    query.itemType = form.itemType
  }
  assignTrimmed('defectDescription', form.defectDescription)
  assignTrimmed('defectLevel', form.defectLevel)
  if (form.defectQuantity !== undefined && form.defectQuantity !== null) {
    query.defectQuantity = form.defectQuantity
  }
  if (form.defectRate !== undefined && form.defectRate !== null) {
    query.defectRate = form.defectRate
  }
  assignTrimmed('causeAnalysis', form.causeAnalysis)
  assignTrimmed('improvementAction', form.improvementAction)
  assignTrimmed('improvementResponsible', form.improvementResponsible)
  assignTrimmed('plannedCompletionDateStart', form.plannedCompletionDateStart)
  assignTrimmed('plannedCompletionDateEnd', form.plannedCompletionDateEnd)
  assignTrimmed('actualCompletionDateStart', form.actualCompletionDateStart)
  assignTrimmed('actualCompletionDateEnd', form.actualCompletionDateEnd)
  if (form.improvementStatus !== undefined && form.improvementStatus !== null) {
    query.improvementStatus = form.improvementStatus
  }
  assignTrimmed('attachmentPaths', form.attachmentPaths)
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
    const res = await getCustomerComplaintItemList(buildListQuery())
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
watch(masterCustomerComplaintId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customercomplaintitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: CustomerComplaintItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customercomplaintitem._self') })
  formLoading.value = true
  try {
    const detail = await getCustomerComplaintItemById(getCustomerComplaintItemId(record))
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
      entity: t('entity.customercomplaintitem._self'),
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
    const id = formData.value?.customerComplaintItemId
    if (id) {
      await updateCustomerComplaintItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.customercomplaintitem._self') }))
    } else {
      await createCustomerComplaintItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.customercomplaintitem._self') }))
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

async function handleDeleteOne(record: CustomerComplaintItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.customercomplaintitem._self'),
      name: t('common.tip.this.target', { target: t('entity.customercomplaintitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintItemById(getCustomerComplaintItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplaintitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.customercomplaintitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.customercomplaintitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getCustomerComplaintItemId(r)).filter(Boolean)
      await deleteCustomerComplaintItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplaintitem._self') }))
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
  const res = await getCustomerComplaintItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerComplaintItem(file, sheetName)
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
    const exportMeta = await exportCustomerComplaintItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.customercomplaintitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.customercomplaintitem._self') }))
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
