<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing/components -->
<!-- 文件名称：routing-item-panel.vue -->
<!-- 功能描述：工艺路线主表实体主表实体右侧明细 routingItem 独立 CRUD（按主表选中 routingId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="routing-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.routingitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:routing:create"
      update-permission="logistics:manufacturing:bom:routing:update"
      delete-permission="logistics:manufacturing:bom:routing:delete"
      import-permission="logistics:manufacturing:bom:routing:import"
      export-permission="logistics:manufacturing:bom:routing:export"
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
    <div class="routing-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getRoutingItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="routingItemId"
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
      <RoutingItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterRoutingId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-routing-routing-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('routingCode')">
      <a-form-item :label="t('entity.routingitem.routingcode')">
        <a-input
          v-model:value="advancedQueryForm.routingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.routingitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="t('entity.routingitem.baseunit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') })"
          show-count
          :maxlength="5"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseQuantity')">
      <a-form-item :label="t('entity.routingitem.basequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.baseQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.basequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardMinutes')">
      <a-form-item :label="t('entity.routingitem.standardminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.standardMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timeUnit')">
      <a-form-item :label="t('entity.routingitem.timeunit')">
        <a-input
          v-model:value="advancedQueryForm.timeUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardShorts')">
      <a-form-item :label="t('entity.routingitem.standardshorts')">
        <a-input-number
          v-model:value="advancedQueryForm.standardShorts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardshorts') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pointsUnit')">
      <a-form-item :label="t('entity.routingitem.pointsunit')">
        <a-input
          v-model:value="advancedQueryForm.pointsUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pointsToMinutesRate')">
      <a-form-item :label="t('entity.routingitem.pointstominutesrate')">
        <TaktSelect
          v-model:value="advancedQueryForm.pointsToMinutesRate"
          dict-type="logistics_points_to_minutes_rate"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedMinutes')">
      <a-form-item :label="t('entity.routingitem.convertedminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.convertedminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('setupMinutes')">
      <a-form-item :label="t('entity.routingitem.setupminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.setupMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.setupminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teardownMinutes')">
      <a-form-item :label="t('entity.routingitem.teardownminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.teardownMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.teardownminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isInspection')">
      <a-form-item :label="t('entity.routingitem.isinspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.isInspection"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.isinspection') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processDescription')">
      <a-form-item :label="t('entity.routingitem.processdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.processDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.routingitem.processdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processSegmentType')">
      <a-form-item :label="t('entity.routingitem.processsegmenttype')">
        <TaktSelect
          v-model:value="advancedQueryForm.processSegmentType"
          dict-type="logistics_process_segment_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.processsegmenttype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extJson')">
      <a-form-item :label="t('entity.routingitem.extjson')">
        <a-input
          v-model:value="advancedQueryForm.extJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.extjson') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.routingitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.routingitem._self"
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
      id-column-key="routingItemId"
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
 * 工艺路线主表实体子表 routingItem 右栏面板
 * @module views/logistics/manufacturing/bom/routing/components
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
import RoutingItemForm from './routing-item-form.vue'
import { useRoutingMasterContext } from '../composables/use-routing-master-context'
import {
  getRoutingItemList,
  getRoutingItemById,
  createRoutingItem,
  updateRoutingItem,
  deleteRoutingItemById,
  deleteRoutingItemBatch,
  getRoutingItemTemplate,
  importRoutingItem,
  exportRoutingItem,
} from '@/api/logistics/manufacturing/bom/routing-item'
import type { RoutingItem, RoutingItemQuery } from '@/types/logistics/manufacturing/bom/routing-item'

const { t } = useI18n()
const { selectedMasterRow } = useRoutingMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktRoutingItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.routingitem._self') }),
)

const loading = ref(false)
const dataSource = ref<RoutingItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<RoutingItem | null>(null)
const selectedRows = ref<RoutingItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<RoutingItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  routingCode: '',
  lineNumber: undefined as number | undefined,
  baseUnit: '',
  baseQuantity: undefined as number | undefined,
  standardMinutes: undefined as number | undefined,
  timeUnit: '',
  standardShorts: undefined as number | undefined,
  pointsUnit: '',
  pointsToMinutesRate: '' as string,
  convertedMinutes: undefined as number | undefined,
  setupMinutes: undefined as number | undefined,
  teardownMinutes: undefined as number | undefined,
  isInspection: undefined as number | undefined,
  processDescription: '',
  processSegmentType: undefined as number | undefined,
  extJson: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'routingCode', label: t('entity.routingitem.routingcode') },
  { key: 'lineNumber', label: t('entity.routingitem.linenumber') },
  { key: 'baseUnit', label: t('entity.routingitem.baseunit') },
  { key: 'baseQuantity', label: t('entity.routingitem.basequantity') },
  { key: 'standardMinutes', label: t('entity.routingitem.standardminutes') },
  { key: 'timeUnit', label: t('entity.routingitem.timeunit') },
  { key: 'standardShorts', label: t('entity.routingitem.standardshorts') },
  { key: 'pointsUnit', label: t('entity.routingitem.pointsunit') },
  { key: 'pointsToMinutesRate', label: t('entity.routingitem.pointstominutesrate') },
  { key: 'convertedMinutes', label: t('entity.routingitem.convertedminutes') },
  { key: 'setupMinutes', label: t('entity.routingitem.setupminutes') },
  { key: 'teardownMinutes', label: t('entity.routingitem.teardownminutes') },
  { key: 'isInspection', label: t('entity.routingitem.isinspection') },
  { key: 'processDescription', label: t('entity.routingitem.processdescription') },
  { key: 'processSegmentType', label: t('entity.routingitem.processsegmenttype') },
  { key: 'extJson', label: t('entity.routingitem.extjson') },
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
  routingCode: '',
  lineNumber: undefined as number | undefined,
  baseUnit: '',
  baseQuantity: undefined as number | undefined,
  standardMinutes: undefined as number | undefined,
  timeUnit: '',
  standardShorts: undefined as number | undefined,
  pointsUnit: '',
  pointsToMinutesRate: '' as string,
  convertedMinutes: undefined as number | undefined,
  setupMinutes: undefined as number | undefined,
  teardownMinutes: undefined as number | undefined,
  isInspection: undefined as number | undefined,
  processDescription: '',
  processSegmentType: undefined as number | undefined,
  extJson: '',
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

const entityIdName = 'routingItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.routingId)
const masterRoutingId = computed(() => selectedMasterRow.value?.routingId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getRoutingItemId(record: RoutingItem | Record<string, unknown>): string {
  return String((record as RoutingItem)?.[entityIdName] ?? '')
}

function getRoutingItemField(record: RoutingItem | Record<string, unknown>, field: string): unknown {
  return (record as RoutingItem)?.[field as keyof RoutingItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'routingItemId',
    key: 'routingItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'routingItemId') ?? ''),
  },
  {
    title: t('entity.routingitem.routingcode'),
    dataIndex: 'routingCode',
    key: 'routingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'routingCode') ?? ''),
  },
  {
    title: t('entity.routingitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.routingitem.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'baseUnit') ?? ''),
  },
  {
    title: t('entity.routingitem.basequantity'),
    dataIndex: 'baseQuantity',
    key: 'baseQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'baseQuantity') ?? ''),
  },
  {
    title: t('entity.routingitem.standardminutes'),
    dataIndex: 'standardMinutes',
    key: 'standardMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'standardMinutes') ?? ''),
  },
  {
    title: t('entity.routingitem.timeunit'),
    dataIndex: 'timeUnit',
    key: 'timeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'timeUnit') ?? ''),
  },
  {
    title: t('entity.routingitem.standardshorts'),
    dataIndex: 'standardShorts',
    key: 'standardShorts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'standardShorts') ?? ''),
  },
  {
    title: t('entity.routingitem.pointsunit'),
    dataIndex: 'pointsUnit',
    key: 'pointsUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItem }) =>
      String(getRoutingItemField(record, 'pointsUnit') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:routing:update',
        onClick: (record: RoutingItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:routing:delete',
        onClick: (record: RoutingItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: RoutingItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: RoutingItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getRoutingItemId(selectedRow.value) === getRoutingItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: RoutingItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: RoutingItem) {
  const key = getRoutingItemId(record)
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
 * @returns {RoutingItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<RoutingItemQuery>): RoutingItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: RoutingItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    routingId: masterRoutingId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof RoutingItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('routingCode', form.routingCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('baseUnit', form.baseUnit)
  if (form.baseQuantity !== undefined && form.baseQuantity !== null) {
    query.baseQuantity = form.baseQuantity
  }
  if (form.standardMinutes !== undefined && form.standardMinutes !== null) {
    query.standardMinutes = form.standardMinutes
  }
  assignTrimmed('timeUnit', form.timeUnit)
  if (form.standardShorts !== undefined && form.standardShorts !== null) {
    query.standardShorts = form.standardShorts
  }
  assignTrimmed('pointsUnit', form.pointsUnit)
  assignTrimmed('pointsToMinutesRate', form.pointsToMinutesRate)
  if (form.convertedMinutes !== undefined && form.convertedMinutes !== null) {
    query.convertedMinutes = form.convertedMinutes
  }
  if (form.setupMinutes !== undefined && form.setupMinutes !== null) {
    query.setupMinutes = form.setupMinutes
  }
  if (form.teardownMinutes !== undefined && form.teardownMinutes !== null) {
    query.teardownMinutes = form.teardownMinutes
  }
  if (form.isInspection !== undefined && form.isInspection !== null) {
    query.isInspection = form.isInspection
  }
  assignTrimmed('processDescription', form.processDescription)
  if (form.processSegmentType !== undefined && form.processSegmentType !== null) {
    query.processSegmentType = form.processSegmentType
  }
  assignTrimmed('extJson', form.extJson)
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
    const res = await getRoutingItemList(buildListQuery())
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
watch(masterRoutingId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.routingitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: RoutingItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.routingitem._self') })
  formLoading.value = true
  try {
    const detail = await getRoutingItemById(getRoutingItemId(record))
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
      entity: t('entity.routingitem._self'),
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
    const id = formData.value?.routingItemId
    if (id) {
      await updateRoutingItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.routingitem._self') }))
    } else {
      await createRoutingItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.routingitem._self') }))
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

async function handleDeleteOne(record: RoutingItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.routingitem._self'),
      name: t('common.tip.this.target', { target: t('entity.routingitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteRoutingItemById(getRoutingItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.routingitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.routingitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.routingitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getRoutingItemId(r)).filter(Boolean)
      await deleteRoutingItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.routingitem._self') }))
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
  const res = await getRoutingItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importRoutingItem(file, sheetName)
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
    const exportMeta = await exportRoutingItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.routingitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.routingitem._self') }))
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
