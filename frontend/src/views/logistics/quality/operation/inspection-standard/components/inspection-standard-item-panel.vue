<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard/components -->
<!-- 文件名称：inspection-standard-item-panel.vue -->
<!-- 功能描述：检验标准实体主表实体右侧明细 inspectionStandardItem 独立 CRUD（按主表选中 inspectionStandardId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="inspection-standard-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.inspectionstandarditem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:inspectionstandard:create"
      update-permission="logistics:quality:operation:inspectionstandard:update"
      delete-permission="logistics:quality:operation:inspectionstandard:delete"
      import-permission="logistics:quality:operation:inspectionstandard:import"
      export-permission="logistics:quality:operation:inspectionstandard:export"
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
    <div class="inspection-standard-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getInspectionStandardItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="inspectionStandardItemId"
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
      <InspectionStandardItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterInspectionStandardId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-inspection-standard-inspection-standard-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.inspectionstandarditem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemCode')">
      <a-form-item :label="t('entity.inspectionstandarditem.itemcode')">
        <a-input
          v-model:value="advancedQueryForm.itemCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="t('entity.inspectionstandarditem.itemname')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemType')">
      <a-form-item :label="t('entity.inspectionstandarditem.itemtype')">
        <a-input-number
          v-model:value="advancedQueryForm.itemType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectLevel')">
      <a-form-item :label="t('entity.inspectionstandarditem.defectlevel')">
        <a-input
          v-model:value="advancedQueryForm.defectLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.defectlevel') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionMode')">
      <a-form-item :label="t('entity.inspectionstandarditem.inspectionmode')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionMode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.inspectionmode') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardValue')">
      <a-form-item :label="t('entity.inspectionstandarditem.standardvalue')">
        <a-input
          v-model:value="advancedQueryForm.standardValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.standardvalue') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('upperLimit')">
      <a-form-item :label="t('entity.inspectionstandarditem.upperlimit')">
        <a-input
          v-model:value="advancedQueryForm.upperLimit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.upperlimit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lowerLimit')">
      <a-form-item :label="t('entity.inspectionstandarditem.lowerlimit')">
        <a-input
          v-model:value="advancedQueryForm.lowerLimit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.lowerlimit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionTool')">
      <a-form-item :label="t('entity.inspectionstandarditem.inspectiontool')">
        <a-input
          v-model:value="advancedQueryForm.inspectionTool"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.inspectiontool') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionMethodDescription')">
      <a-form-item :label="t('entity.inspectionstandarditem.inspectionmethoddescription')">
        <a-textarea
          v-model:value="advancedQueryForm.inspectionMethodDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.inspectionstandarditem.inspectionmethoddescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptanceCriteria')">
      <a-form-item :label="t('entity.inspectionstandarditem.acceptancecriteria')">
        <a-input
          v-model:value="advancedQueryForm.acceptanceCriteria"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.acceptancecriteria') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rejectionCriteria')">
      <a-form-item :label="t('entity.inspectionstandarditem.rejectioncriteria')">
        <a-input
          v-model:value="advancedQueryForm.rejectionCriteria"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.rejectioncriteria') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQualifiedBasis')">
      <a-form-item :label="t('entity.inspectionstandarditem.isqualifiedbasis')">
        <a-input-number
          v-model:value="advancedQueryForm.isQualifiedBasis"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.isqualifiedbasis') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.inspectionstandarditem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.inspectionstandarditem._self"
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
      id-column-key="inspectionStandardItemId"
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
 * 检验标准实体子表 inspectionStandardItem 右栏面板
 * @module views/logistics/quality/operation/inspection-standard/components
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
import InspectionStandardItemForm from './inspection-standard-item-form.vue'
import { useInspectionStandardMasterContext } from '../composables/use-inspection-standard-master-context'
import {
  getInspectionStandardItemList,
  getInspectionStandardItemById,
  createInspectionStandardItem,
  updateInspectionStandardItem,
  deleteInspectionStandardItemById,
  deleteInspectionStandardItemBatch,
  getInspectionStandardItemTemplate,
  importInspectionStandardItem,
  exportInspectionStandardItem,
} from '@/api/logistics/quality/operation/inspection-standard-item'
import type { InspectionStandardItem, InspectionStandardItemQuery } from '@/types/logistics/quality/operation/inspection-standard-item'

const { t } = useI18n()
const { selectedMasterRow } = useInspectionStandardMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktInspectionStandardItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.inspectionstandarditem._self') }),
)

const loading = ref(false)
const dataSource = ref<InspectionStandardItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<InspectionStandardItem | null>(null)
const selectedRows = ref<InspectionStandardItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<InspectionStandardItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  lineNumber: undefined as number | undefined,
  itemCode: '',
  itemName: '',
  itemType: undefined as number | undefined,
  defectLevel: '',
  inspectionMode: undefined as number | undefined,
  standardValue: '',
  upperLimit: '',
  lowerLimit: '',
  inspectionTool: '',
  inspectionMethodDescription: '',
  acceptanceCriteria: '',
  rejectionCriteria: '',
  isQualifiedBasis: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'lineNumber', label: t('entity.inspectionstandarditem.linenumber') },
  { key: 'itemCode', label: t('entity.inspectionstandarditem.itemcode') },
  { key: 'itemName', label: t('entity.inspectionstandarditem.itemname') },
  { key: 'itemType', label: t('entity.inspectionstandarditem.itemtype') },
  { key: 'defectLevel', label: t('entity.inspectionstandarditem.defectlevel') },
  { key: 'inspectionMode', label: t('entity.inspectionstandarditem.inspectionmode') },
  { key: 'standardValue', label: t('entity.inspectionstandarditem.standardvalue') },
  { key: 'upperLimit', label: t('entity.inspectionstandarditem.upperlimit') },
  { key: 'lowerLimit', label: t('entity.inspectionstandarditem.lowerlimit') },
  { key: 'inspectionTool', label: t('entity.inspectionstandarditem.inspectiontool') },
  { key: 'inspectionMethodDescription', label: t('entity.inspectionstandarditem.inspectionmethoddescription') },
  { key: 'acceptanceCriteria', label: t('entity.inspectionstandarditem.acceptancecriteria') },
  { key: 'rejectionCriteria', label: t('entity.inspectionstandarditem.rejectioncriteria') },
  { key: 'isQualifiedBasis', label: t('entity.inspectionstandarditem.isqualifiedbasis') },
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
  lineNumber: undefined as number | undefined,
  itemCode: '',
  itemName: '',
  itemType: undefined as number | undefined,
  defectLevel: '',
  inspectionMode: undefined as number | undefined,
  standardValue: '',
  upperLimit: '',
  lowerLimit: '',
  inspectionTool: '',
  inspectionMethodDescription: '',
  acceptanceCriteria: '',
  rejectionCriteria: '',
  isQualifiedBasis: undefined as number | undefined,
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

const entityIdName = 'inspectionStandardItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.inspectionStandardId)
const masterInspectionStandardId = computed(() => selectedMasterRow.value?.inspectionStandardId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getInspectionStandardItemId(record: InspectionStandardItem | Record<string, unknown>): string {
  return String((record as InspectionStandardItem)?.[entityIdName] ?? '')
}

function getInspectionStandardItemField(record: InspectionStandardItem | Record<string, unknown>, field: string): unknown {
  return (record as InspectionStandardItem)?.[field as keyof InspectionStandardItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'inspectionStandardItemId',
    key: 'inspectionStandardItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'inspectionStandardItemId') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.itemcode'),
    dataIndex: 'itemCode',
    key: 'itemCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'itemCode') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'itemName') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'itemType') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.defectlevel'),
    dataIndex: 'defectLevel',
    key: 'defectLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'defectLevel') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.inspectionmode'),
    dataIndex: 'inspectionMode',
    key: 'inspectionMode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'inspectionMode') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.standardvalue'),
    dataIndex: 'standardValue',
    key: 'standardValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'standardValue') ?? ''),
  },
  {
    title: t('entity.inspectionstandarditem.upperlimit'),
    dataIndex: 'upperLimit',
    key: 'upperLimit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: InspectionStandardItem }) =>
      String(getInspectionStandardItemField(record, 'upperLimit') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:inspectionstandard:update',
        onClick: (record: InspectionStandardItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:inspectionstandard:delete',
        onClick: (record: InspectionStandardItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: InspectionStandardItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: InspectionStandardItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getInspectionStandardItemId(selectedRow.value) === getInspectionStandardItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: InspectionStandardItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: InspectionStandardItem) {
  const key = getInspectionStandardItemId(record)
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
 * @returns {InspectionStandardItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<InspectionStandardItemQuery>): InspectionStandardItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: InspectionStandardItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    inspectionStandardId: masterInspectionStandardId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof InspectionStandardItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('itemCode', form.itemCode)
  assignTrimmed('itemName', form.itemName)
  if (form.itemType !== undefined && form.itemType !== null) {
    query.itemType = form.itemType
  }
  assignTrimmed('defectLevel', form.defectLevel)
  if (form.inspectionMode !== undefined && form.inspectionMode !== null) {
    query.inspectionMode = form.inspectionMode
  }
  assignTrimmed('standardValue', form.standardValue)
  assignTrimmed('upperLimit', form.upperLimit)
  assignTrimmed('lowerLimit', form.lowerLimit)
  assignTrimmed('inspectionTool', form.inspectionTool)
  assignTrimmed('inspectionMethodDescription', form.inspectionMethodDescription)
  assignTrimmed('acceptanceCriteria', form.acceptanceCriteria)
  assignTrimmed('rejectionCriteria', form.rejectionCriteria)
  if (form.isQualifiedBasis !== undefined && form.isQualifiedBasis !== null) {
    query.isQualifiedBasis = form.isQualifiedBasis
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
    const res = await getInspectionStandardItemList(buildListQuery())
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
watch(masterInspectionStandardId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.inspectionstandarditem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: InspectionStandardItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.inspectionstandarditem._self') })
  formLoading.value = true
  try {
    const detail = await getInspectionStandardItemById(getInspectionStandardItemId(record))
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
      entity: t('entity.inspectionstandarditem._self'),
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
    const id = formData.value?.inspectionStandardItemId
    if (id) {
      await updateInspectionStandardItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.inspectionstandarditem._self') }))
    } else {
      await createInspectionStandardItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.inspectionstandarditem._self') }))
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

async function handleDeleteOne(record: InspectionStandardItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.inspectionstandarditem._self'),
      name: t('common.tip.this.target', { target: t('entity.inspectionstandarditem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteInspectionStandardItemById(getInspectionStandardItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.inspectionstandarditem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.inspectionstandarditem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.inspectionstandarditem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getInspectionStandardItemId(r)).filter(Boolean)
      await deleteInspectionStandardItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.inspectionstandarditem._self') }))
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
  const res = await getInspectionStandardItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importInspectionStandardItem(file, sheetName)
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
    const exportMeta = await exportInspectionStandardItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.inspectionstandarditem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.inspectionstandarditem._self') }))
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
