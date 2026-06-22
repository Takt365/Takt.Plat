<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/components -->
<!-- 文件名称：customer-satisfaction-survey-item-panel.vue -->
<!-- 功能描述：客户满意度调查表主表实体主表实体右侧明细 customerSatisfactionSurveyItem 独立 CRUD（按主表选中 surveyId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="customer-satisfaction-survey-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.customersatisfactionsurveyitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:complaint:customersatisfactionsurvey:create"
      update-permission="logistics:quality:complaint:customersatisfactionsurvey:update"
      delete-permission="logistics:quality:complaint:customersatisfactionsurvey:delete"
      import-permission="logistics:quality:complaint:customersatisfactionsurvey:import"
      export-permission="logistics:quality:complaint:customersatisfactionsurvey:export"
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
    <div class="customer-satisfaction-survey-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getCustomerSatisfactionSurveyItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="customerSatisfactionSurveyItemId"
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
      <CustomerSatisfactionSurveyItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterCustomerSatisfactionSurveyId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-complaint-customer-satisfaction-survey-customer-satisfaction-survey-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('customerSatisfactionSurveyCode')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.customersatisfactionsurveycode')">
        <a-input
          v-model:value="advancedQueryForm.customerSatisfactionSurveyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.customersatisfactionsurveycode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('categoryType')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.categorytype')">
        <a-input-number
          v-model:value="advancedQueryForm.categoryType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.categorytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.itemname')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.itemname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemDescription')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.itemdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.itemDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customersatisfactionsurveyitem.itemdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weight')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.weight')">
        <a-input-number
          v-model:value="advancedQueryForm.weight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.weight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('score')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.score')">
        <a-input-number
          v-model:value="advancedQueryForm.score"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.score') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('satisfactionLevel')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.satisfactionlevel')">
        <a-input-number
          v-model:value="advancedQueryForm.satisfactionLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.satisfactionlevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerFeedback')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.customerfeedback')">
        <a-input
          v-model:value="advancedQueryForm.customerFeedback"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.customerfeedback') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementSuggestion')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.improvementsuggestion')">
        <a-input
          v-model:value="advancedQueryForm.improvementSuggestion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.improvementsuggestion') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpAction')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.followupaction')">
        <a-input
          v-model:value="advancedQueryForm.followUpAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.followupaction') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpStatus')">
      <a-form-item :label="t('entity.customersatisfactionsurveyitem.followupstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.followUpStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.followupstatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customersatisfactionsurveyitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customersatisfactionsurveyitem._self"
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
      id-column-key="customerSatisfactionSurveyItemId"
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
 * 客户满意度调查表主表实体子表 customerSatisfactionSurveyItem 右栏面板
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
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
import CustomerSatisfactionSurveyItemForm from './customer-satisfaction-survey-item-form.vue'
import { useCustomerSatisfactionSurveyMasterContext } from '../composables/use-customer-satisfaction-survey-master-context'
import {
  getCustomerSatisfactionSurveyItemList,
  getCustomerSatisfactionSurveyItemById,
  createCustomerSatisfactionSurveyItem,
  updateCustomerSatisfactionSurveyItem,
  deleteCustomerSatisfactionSurveyItemById,
  deleteCustomerSatisfactionSurveyItemBatch,
  getCustomerSatisfactionSurveyItemTemplate,
  importCustomerSatisfactionSurveyItem,
  exportCustomerSatisfactionSurveyItem,
} from '@/api/logistics/quality/complaint/customer-satisfaction-survey-item'
import type { CustomerSatisfactionSurveyItem, CustomerSatisfactionSurveyItemQuery } from '@/types/logistics/quality/complaint/customer-satisfaction-survey-item'

const { t } = useI18n()
const { selectedMasterRow } = useCustomerSatisfactionSurveyMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerSatisfactionSurveyItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customersatisfactionsurveyitem._self') }),
)

const loading = ref(false)
const dataSource = ref<CustomerSatisfactionSurveyItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<CustomerSatisfactionSurveyItem | null>(null)
const selectedRows = ref<CustomerSatisfactionSurveyItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<CustomerSatisfactionSurveyItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  customerSatisfactionSurveyCode: '',
  lineNumber: undefined as number | undefined,
  categoryType: undefined as number | undefined,
  itemName: '',
  itemDescription: '',
  weight: undefined as number | undefined,
  score: undefined as number | undefined,
  satisfactionLevel: undefined as number | undefined,
  customerFeedback: '',
  improvementSuggestion: '',
  followUpAction: '',
  followUpStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'customerSatisfactionSurveyCode', label: t('entity.customersatisfactionsurveyitem.customersatisfactionsurveycode') },
  { key: 'lineNumber', label: t('entity.customersatisfactionsurveyitem.linenumber') },
  { key: 'categoryType', label: t('entity.customersatisfactionsurveyitem.categorytype') },
  { key: 'itemName', label: t('entity.customersatisfactionsurveyitem.itemname') },
  { key: 'itemDescription', label: t('entity.customersatisfactionsurveyitem.itemdescription') },
  { key: 'weight', label: t('entity.customersatisfactionsurveyitem.weight') },
  { key: 'score', label: t('entity.customersatisfactionsurveyitem.score') },
  { key: 'satisfactionLevel', label: t('entity.customersatisfactionsurveyitem.satisfactionlevel') },
  { key: 'customerFeedback', label: t('entity.customersatisfactionsurveyitem.customerfeedback') },
  { key: 'improvementSuggestion', label: t('entity.customersatisfactionsurveyitem.improvementsuggestion') },
  { key: 'followUpAction', label: t('entity.customersatisfactionsurveyitem.followupaction') },
  { key: 'followUpStatus', label: t('entity.customersatisfactionsurveyitem.followupstatus') },
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
  customerSatisfactionSurveyCode: '',
  lineNumber: undefined as number | undefined,
  categoryType: undefined as number | undefined,
  itemName: '',
  itemDescription: '',
  weight: undefined as number | undefined,
  score: undefined as number | undefined,
  satisfactionLevel: undefined as number | undefined,
  customerFeedback: '',
  improvementSuggestion: '',
  followUpAction: '',
  followUpStatus: undefined as number | undefined,
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

const entityIdName = 'customerSatisfactionSurveyItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.customerSatisfactionSurveyId)
const masterCustomerSatisfactionSurveyId = computed(() => selectedMasterRow.value?.customerSatisfactionSurveyId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getCustomerSatisfactionSurveyItemId(record: CustomerSatisfactionSurveyItem | Record<string, unknown>): string {
  return String((record as CustomerSatisfactionSurveyItem)?.[entityIdName] ?? '')
}

function getCustomerSatisfactionSurveyItemField(record: CustomerSatisfactionSurveyItem | Record<string, unknown>, field: string): unknown {
  return (record as CustomerSatisfactionSurveyItem)?.[field as keyof CustomerSatisfactionSurveyItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerSatisfactionSurveyItemId',
    key: 'customerSatisfactionSurveyItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'customerSatisfactionSurveyItemId') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.surveyid'),
    dataIndex: 'surveyId',
    key: 'surveyId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'surveyId') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.customersatisfactionsurveycode'),
    dataIndex: 'customerSatisfactionSurveyCode',
    key: 'customerSatisfactionSurveyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'customerSatisfactionSurveyCode') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'categoryType') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'itemName') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'itemDescription') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'weight') ?? ''),
  },
  {
    title: t('entity.customersatisfactionsurveyitem.score'),
    dataIndex: 'score',
    key: 'score',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'score') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customersatisfactionsurvey:update',
        onClick: (record: CustomerSatisfactionSurveyItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customersatisfactionsurvey:delete',
        onClick: (record: CustomerSatisfactionSurveyItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerSatisfactionSurveyItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerSatisfactionSurveyItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCustomerSatisfactionSurveyItemId(selectedRow.value) === getCustomerSatisfactionSurveyItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerSatisfactionSurveyItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: CustomerSatisfactionSurveyItem) {
  const key = getCustomerSatisfactionSurveyItemId(record)
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
 * @returns {CustomerSatisfactionSurveyItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerSatisfactionSurveyItemQuery>): CustomerSatisfactionSurveyItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerSatisfactionSurveyItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    surveyId: masterCustomerSatisfactionSurveyId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerSatisfactionSurveyItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('customerSatisfactionSurveyCode', form.customerSatisfactionSurveyCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.categoryType !== undefined && form.categoryType !== null) {
    query.categoryType = form.categoryType
  }
  assignTrimmed('itemName', form.itemName)
  assignTrimmed('itemDescription', form.itemDescription)
  if (form.weight !== undefined && form.weight !== null) {
    query.weight = form.weight
  }
  if (form.score !== undefined && form.score !== null) {
    query.score = form.score
  }
  if (form.satisfactionLevel !== undefined && form.satisfactionLevel !== null) {
    query.satisfactionLevel = form.satisfactionLevel
  }
  assignTrimmed('customerFeedback', form.customerFeedback)
  assignTrimmed('improvementSuggestion', form.improvementSuggestion)
  assignTrimmed('followUpAction', form.followUpAction)
  if (form.followUpStatus !== undefined && form.followUpStatus !== null) {
    query.followUpStatus = form.followUpStatus
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
    const res = await getCustomerSatisfactionSurveyItemList(buildListQuery())
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
watch(masterCustomerSatisfactionSurveyId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customersatisfactionsurveyitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: CustomerSatisfactionSurveyItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customersatisfactionsurveyitem._self') })
  formLoading.value = true
  try {
    const detail = await getCustomerSatisfactionSurveyItemById(getCustomerSatisfactionSurveyItemId(record))
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
      entity: t('entity.customersatisfactionsurveyitem._self'),
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
    const id = formData.value?.customerSatisfactionSurveyItemId
    if (id) {
      await updateCustomerSatisfactionSurveyItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.customersatisfactionsurveyitem._self') }))
    } else {
      await createCustomerSatisfactionSurveyItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.customersatisfactionsurveyitem._self') }))
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

async function handleDeleteOne(record: CustomerSatisfactionSurveyItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.customersatisfactionsurveyitem._self'),
      name: t('common.tip.this.target', { target: t('entity.customersatisfactionsurveyitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerSatisfactionSurveyItemById(getCustomerSatisfactionSurveyItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.customersatisfactionsurveyitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.customersatisfactionsurveyitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.customersatisfactionsurveyitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getCustomerSatisfactionSurveyItemId(r)).filter(Boolean)
      await deleteCustomerSatisfactionSurveyItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customersatisfactionsurveyitem._self') }))
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
  const res = await getCustomerSatisfactionSurveyItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerSatisfactionSurveyItem(file, sheetName)
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
    const exportMeta = await exportCustomerSatisfactionSurveyItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.customersatisfactionsurveyitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.customersatisfactionsurveyitem._self') }))
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
