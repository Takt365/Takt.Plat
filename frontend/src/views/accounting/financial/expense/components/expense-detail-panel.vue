<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/expense/components -->
<!-- 文件名称：expense-detail-panel.vue -->
<!-- 功能描述：费用单实体主表实体右侧明细 expenseDetail 独立 CRUD（按主表选中 expenseId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="expense-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.expensedetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="accounting:financial:expense:create"
      update-permission="accounting:financial:expense:update"
      delete-permission="accounting:financial:expense:delete"
      import-permission="accounting:financial:expense:import"
      export-permission="accounting:financial:expense:export"
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
    <div class="expense-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getExpenseDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="expenseDetailId"
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
      <ExpenseDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterExpenseId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-accounting-financial-expense-expense-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('expenseCode')">
      <a-form-item :label="t('entity.expensedetail.expensecode')">
        <a-input
          v-model:value="advancedQueryForm.expenseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.expensecode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.expensedetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allocationCategory')">
      <a-form-item :label="t('entity.expensedetail.allocationcategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.allocationCategory"
          dict-type="logistics_allocation_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expensedetail.allocationcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="t('entity.expensedetail.itemname')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.itemname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemDescription')">
      <a-form-item :label="t('entity.expensedetail.itemdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.itemDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.expensedetail.itemdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemQuantity')">
      <a-form-item :label="t('entity.expensedetail.itemquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.itemQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.itemquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemAmount')">
      <a-form-item :label="t('entity.expensedetail.itemamount')">
        <a-input-number
          v-model:value="advancedQueryForm.itemAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.itemamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitle')">
      <a-form-item :label="t('entity.expensedetail.accounttitle')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitle"
          api-url="TaktAccountTitles/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expensedetail.accounttitle') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('invoiceNo')">
      <a-form-item :label="t('entity.expensedetail.invoiceno')">
        <a-input
          v-model:value="advancedQueryForm.invoiceNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expensedetail.invoiceno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseDetailDateStart')">
      <a-form-item :label="t('entity.expensedetail.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expenseDetailDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expensedetail.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseDetailDateEnd')">
      <a-form-item :label="t('entity.expensedetail.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expenseDetailDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expensedetail.dateend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.expensedetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.expensedetail._self"
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
      id-column-key="expenseDetailId"
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
 * 费用单实体子表 expenseDetail 右栏面板
 * @module views/accounting/financial/expense/components
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
import ExpenseDetailForm from './expense-detail-form.vue'
import { useExpenseMasterContext } from '../composables/use-expense-master-context'
import {
  getExpenseDetailList,
  getExpenseDetailById,
  createExpenseDetail,
  updateExpenseDetail,
  deleteExpenseDetailById,
  deleteExpenseDetailBatch,
  getExpenseDetailTemplate,
  importExpenseDetail,
  exportExpenseDetail,
} from '@/api/accounting/financial/expense-detail'
import type { ExpenseDetail, ExpenseDetailQuery } from '@/types/accounting/financial/expense-detail'

const { t } = useI18n()
const { selectedMasterRow } = useExpenseMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktExpenseDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.expensedetail._self') }),
)

const loading = ref(false)
const dataSource = ref<ExpenseDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ExpenseDetail | null>(null)
const selectedRows = ref<ExpenseDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ExpenseDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  expenseCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  itemName: '',
  itemDescription: '',
  itemQuantity: undefined as number | undefined,
  itemAmount: undefined as number | undefined,
  accountTitle: '',
  invoiceNo: '',
  expenseDetailDateStart: '',
  expenseDetailDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'expenseCode', label: t('entity.expensedetail.expensecode') },
  { key: 'lineNumber', label: t('entity.expensedetail.linenumber') },
  { key: 'allocationCategory', label: t('entity.expensedetail.allocationcategory') },
  { key: 'itemName', label: t('entity.expensedetail.itemname') },
  { key: 'itemDescription', label: t('entity.expensedetail.itemdescription') },
  { key: 'itemQuantity', label: t('entity.expensedetail.itemquantity') },
  { key: 'itemAmount', label: t('entity.expensedetail.itemamount') },
  { key: 'accountTitle', label: t('entity.expensedetail.accounttitle') },
  { key: 'invoiceNo', label: t('entity.expensedetail.invoiceno') },
  { key: 'expenseDetailDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.expensedetail.date')) },
  { key: 'expenseDetailDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.expensedetail.date')) },
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
  expenseCode: '',
  lineNumber: undefined as number | undefined,
  allocationCategory: '',
  itemName: '',
  itemDescription: '',
  itemQuantity: undefined as number | undefined,
  itemAmount: undefined as number | undefined,
  accountTitle: '',
  invoiceNo: '',
  expenseDetailDateStart: '',
  expenseDetailDateEnd: '',
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

const entityIdName = 'expenseDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.expenseId)
const masterExpenseId = computed(() => selectedMasterRow.value?.expenseId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getExpenseDetailId(record: ExpenseDetail | Record<string, unknown>): string {
  return String((record as ExpenseDetail)?.[entityIdName] ?? '')
}

function getExpenseDetailField(record: ExpenseDetail | Record<string, unknown>, field: string): unknown {
  return (record as ExpenseDetail)?.[field as keyof ExpenseDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'expenseDetailId',
    key: 'expenseDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'expenseDetailId') ?? ''),
  },
  {
    title: t('entity.expensedetail.expensecode'),
    dataIndex: 'expenseCode',
    key: 'expenseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'expenseCode') ?? ''),
  },
  {
    title: t('entity.expensedetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.expensedetail.allocationcategory'),
    dataIndex: 'allocationCategory',
    key: 'allocationCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'allocationCategory') ?? ''),
  },
  {
    title: t('entity.expensedetail.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'itemName') ?? ''),
  },
  {
    title: t('entity.expensedetail.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'itemDescription') ?? ''),
  },
  {
    title: t('entity.expensedetail.itemquantity'),
    dataIndex: 'itemQuantity',
    key: 'itemQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'itemQuantity') ?? ''),
  },
  {
    title: t('entity.expensedetail.itemamount'),
    dataIndex: 'itemAmount',
    key: 'itemAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'itemAmount') ?? ''),
  },
  {
    title: t('entity.expensedetail.accounttitle'),
    dataIndex: 'accountTitle',
    key: 'accountTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ExpenseDetail }) =>
      String(getExpenseDetailField(record, 'accountTitle') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:expense:update',
        onClick: (record: ExpenseDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:expense:delete',
        onClick: (record: ExpenseDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ExpenseDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ExpenseDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getExpenseDetailId(selectedRow.value) === getExpenseDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ExpenseDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ExpenseDetail) {
  const key = getExpenseDetailId(record)
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
 * @returns {ExpenseDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ExpenseDetailQuery>): ExpenseDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ExpenseDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    expenseId: masterExpenseId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ExpenseDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('expenseCode', form.expenseCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('allocationCategory', form.allocationCategory)
  assignTrimmed('itemName', form.itemName)
  assignTrimmed('itemDescription', form.itemDescription)
  if (form.itemQuantity !== undefined && form.itemQuantity !== null) {
    query.itemQuantity = form.itemQuantity
  }
  if (form.itemAmount !== undefined && form.itemAmount !== null) {
    query.itemAmount = form.itemAmount
  }
  assignTrimmed('accountTitle', form.accountTitle)
  assignTrimmed('invoiceNo', form.invoiceNo)
  assignTrimmed('expenseDetailDateStart', form.expenseDetailDateStart)
  assignTrimmed('expenseDetailDateEnd', form.expenseDetailDateEnd)
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
    const res = await getExpenseDetailList(buildListQuery())
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
watch(masterExpenseId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.expensedetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ExpenseDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.expensedetail._self') })
  formLoading.value = true
  try {
    const detail = await getExpenseDetailById(getExpenseDetailId(record))
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
      entity: t('entity.expensedetail._self'),
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
    const id = formData.value?.expenseDetailId
    if (id) {
      await updateExpenseDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.expensedetail._self') }))
    } else {
      await createExpenseDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.expensedetail._self') }))
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

async function handleDeleteOne(record: ExpenseDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.expensedetail._self'),
      name: t('common.tip.this.target', { target: t('entity.expensedetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteExpenseDetailById(getExpenseDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.expensedetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.expensedetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.expensedetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getExpenseDetailId(r)).filter(Boolean)
      await deleteExpenseDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.expensedetail._self') }))
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
  const res = await getExpenseDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importExpenseDetail(file, sheetName)
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
    const exportMeta = await exportExpenseDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.expensedetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.expensedetail._self') }))
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
