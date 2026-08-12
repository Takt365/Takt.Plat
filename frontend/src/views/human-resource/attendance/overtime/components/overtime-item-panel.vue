<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：overtime-item-panel.vue -->
<!-- 功能描述：加班申请主表实体右侧明细 overtimeItem 独立 CRUD（按主表选中 overtimeId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="overtime-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.overtimeitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="human:resource:attendance:overtime:create"
      update-permission="human:resource:attendance:overtime:update"
      delete-permission="human:resource:attendance:overtime:delete"
      import-permission="human:resource:attendance:overtime:import"
      export-permission="human:resource:attendance:overtime:export"
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
    <div class="overtime-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getOvertimeItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="overtimeItemId"
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
      <OvertimeItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterOvertimeId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-human-resource-attendance-overtime-overtime-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.overtimeitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.overtimeitem.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.overtimeitem.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeename') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedHours')">
      <a-form-item :label="t('entity.overtimeitem.plannedhours')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.plannedhours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.overtimeitem.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtimeitem.actualstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.overtimeitem.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtimeitem.actualstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.overtimeitem.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtimeitem.actualendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.overtimeitem.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtimeitem.actualendtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualHours')">
      <a-form-item :label="t('entity.overtimeitem.actualhours')">
        <a-input-number
          v-model:value="advancedQueryForm.actualHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.actualhours') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.overtimeitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.overtimeitem._self"
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
      id-column-key="overtimeItemId"
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
 * 加班申请子表 overtimeItem 右栏面板
 * @module views/human-resource/attendance/overtime/components
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
import OvertimeItemForm from './overtime-item-form.vue'
import { useOvertimeMasterContext } from '../composables/use-overtime-master-context'
import {
  getOvertimeItemList,
  getOvertimeItemById,
  createOvertimeItem,
  updateOvertimeItem,
  deleteOvertimeItemById,
  deleteOvertimeItemBatch,
  getOvertimeItemTemplate,
  importOvertimeItem,
  exportOvertimeItem,
} from '@/api/human-resource/attendance/overtime-item'
import type { OvertimeItem, OvertimeItemQuery } from '@/types/human-resource/attendance/overtime-item'

const { t } = useI18n()
const { selectedMasterRow } = useOvertimeMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktOvertimeItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.overtimeitem._self') }),
)

const loading = ref(false)
const dataSource = ref<OvertimeItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<OvertimeItem | null>(null)
const selectedRows = ref<OvertimeItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<OvertimeItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  lineNumber: undefined as number | undefined,
  employeeId: '',
  employeeName: '',
  plannedHours: undefined as number | undefined,
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  actualHours: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'lineNumber', label: t('entity.overtimeitem.linenumber') },
  { key: 'employeeId', label: t('entity.overtimeitem.employeeid') },
  { key: 'employeeName', label: t('entity.overtimeitem.employeename') },
  { key: 'plannedHours', label: t('entity.overtimeitem.plannedhours') },
  { key: 'actualStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.overtimeitem.actualstarttime')) },
  { key: 'actualStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.overtimeitem.actualstarttime')) },
  { key: 'actualEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.overtimeitem.actualendtime')) },
  { key: 'actualEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.overtimeitem.actualendtime')) },
  { key: 'actualHours', label: t('entity.overtimeitem.actualhours') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

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
  employeeId: '',
  employeeName: '',
  plannedHours: undefined as number | undefined,
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  actualHours: undefined as number | undefined,
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

const entityIdName = 'overtimeItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.overtimeId)
const masterOvertimeId = computed(() => selectedMasterRow.value?.overtimeId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getOvertimeItemId(record: OvertimeItem | Record<string, unknown>): string {
  return String((record as OvertimeItem)?.[entityIdName] ?? '')
}

function getOvertimeItemField(record: OvertimeItem | Record<string, unknown>, field: string): unknown {
  return (record as OvertimeItem)?.[field as keyof OvertimeItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'overtimeItemId',
    key: 'overtimeItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'overtimeItemId') ?? ''),
  },
  {
    title: t('entity.overtimeitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.overtimeitem.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'employeeId') ?? ''),
  },
  {
    title: t('entity.overtimeitem.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'employeeName') ?? ''),
  },
  {
    title: t('entity.overtimeitem.plannedhours'),
    dataIndex: 'plannedHours',
    key: 'plannedHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'plannedHours') ?? ''),
  },
  {
    title: t('entity.overtimeitem.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'actualStartTime') ?? ''),
  },
  {
    title: t('entity.overtimeitem.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'actualEndTime') ?? ''),
  },
  {
    title: t('entity.overtimeitem.actualhours'),
    dataIndex: 'actualHours',
    key: 'actualHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'actualHours') ?? ''),
  },
  {
    title: t('entity.overtimeitem.overtime'),
    dataIndex: 'overtime',
    key: 'overtime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: OvertimeItem }) =>
      String(getOvertimeItemField(record, 'overtime') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:attendance:overtime:update',
        onClick: (record: OvertimeItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:attendance:overtime:delete',
        onClick: (record: OvertimeItem) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: OvertimeItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: OvertimeItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getOvertimeItemId(selectedRow.value) === getOvertimeItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: OvertimeItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: OvertimeItem) {
  const key = getOvertimeItemId(record)
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
 * @returns {OvertimeItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<OvertimeItemQuery>): OvertimeItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: OvertimeItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    overtimeId: masterOvertimeId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof OvertimeItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('employeeName', form.employeeName)
  if (form.plannedHours !== undefined && form.plannedHours !== null) {
    query.plannedHours = form.plannedHours
  }
  assignTrimmed('actualStartTimeStart', form.actualStartTimeStart)
  assignTrimmed('actualStartTimeEnd', form.actualStartTimeEnd)
  assignTrimmed('actualEndTimeStart', form.actualEndTimeStart)
  assignTrimmed('actualEndTimeEnd', form.actualEndTimeEnd)
  if (form.actualHours !== undefined && form.actualHours !== null) {
    query.actualHours = form.actualHours
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
    const res = await getOvertimeItemList(buildListQuery())
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
watch(masterOvertimeId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.overtimeitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: OvertimeItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.overtimeitem._self') })
  formLoading.value = true
  try {
    const detail = await getOvertimeItemById(getOvertimeItemId(record))
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
      entity: t('entity.overtimeitem._self'),
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
    const id = formData.value?.overtimeItemId
    if (id) {
      await updateOvertimeItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.overtimeitem._self') }))
    } else {
      await createOvertimeItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.overtimeitem._self') }))
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

async function handleDeleteOne(record: OvertimeItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.overtimeitem._self'),
      name: t('common.tip.this.target', { target: t('entity.overtimeitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOvertimeItemById(getOvertimeItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.overtimeitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.overtimeitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.overtimeitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getOvertimeItemId(r)).filter(Boolean)
      await deleteOvertimeItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.overtimeitem._self') }))
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
  const res = await getOvertimeItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importOvertimeItem(file, sheetName)
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
    const exportMeta = await exportOvertimeItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.overtimeitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.overtimeitem._self') }))
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
