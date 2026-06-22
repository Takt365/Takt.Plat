<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/serial/outbound/components -->
<!-- 文件名称：outbound-item-panel.vue -->
<!-- 功能描述：序列号出库主表实体主表实体右侧明细 serialOutboundItem 独立 CRUD（按主表选中 outboundId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="outbound-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.serialoutbounditem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:serial:outbound:create"
      update-permission="logistics:serial:outbound:update"
      delete-permission="logistics:serial:outbound:delete"
      import-permission="logistics:serial:outbound:import"
      export-permission="logistics:serial:outbound:export"
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
    <div class="outbound-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSerialOutboundItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="serialOutboundItemId"
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
      <SerialOutboundItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSerialOutboundId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-serial-outbound-outbound-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('outboundNo')">
      <a-form-item :label="t('entity.serialoutbounditem.outboundno')">
        <a-input
          v-model:value="advancedQueryForm.outboundNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.outboundno') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.serialoutbounditem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundSerialNo')">
      <a-form-item :label="t('entity.serialoutbounditem.outboundserialno')">
        <a-input
          v-model:value="advancedQueryForm.outboundSerialNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.outboundserialno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceInboundId')">
      <a-form-item :label="t('entity.serialoutbounditem.referenceinboundid')">
        <a-input
          v-model:value="advancedQueryForm.referenceInboundId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.referenceinboundid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceInboundNo')">
      <a-form-item :label="t('entity.serialoutbounditem.referenceinboundno')">
        <a-input
          v-model:value="advancedQueryForm.referenceInboundNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.referenceinboundno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceInboundLineNumber')">
      <a-form-item :label="t('entity.serialoutbounditem.referenceinboundlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.referenceInboundLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbounditem.referenceinboundlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundTimeStart')">
      <a-form-item :label="t('entity.serialoutbounditem.outboundtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbounditem.outboundtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundTimeEnd')">
      <a-form-item :label="t('entity.serialoutbounditem.outboundtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbounditem.outboundtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
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
      :title="t('common.dialog.title.import', { entity: t('entity.serialoutbounditem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.serialoutbounditem._self"
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
      id-column-key="serialOutboundItemId"
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
 * 序列号出库主表实体子表 serialOutboundItem 右栏面板
 * @module views/logistics/serial/outbound/components
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
import SerialOutboundItemForm from './outbound-item-form.vue'
import { useSerialOutboundMasterContext } from '../composables/use-outbound-master-context'
import {
  getSerialOutboundItemList,
  getSerialOutboundItemById,
  createSerialOutboundItem,
  updateSerialOutboundItem,
  deleteSerialOutboundItemById,
  deleteSerialOutboundItemBatch,
  getSerialOutboundItemTemplate,
  importSerialOutboundItem,
  exportSerialOutboundItem,
} from '@/api/logistics/serial/outbound-item'
import type { SerialOutboundItem, SerialOutboundItemQuery } from '@/types/logistics/serial/outbound-item'

const { t } = useI18n()
const { selectedMasterRow } = useSerialOutboundMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSerialOutboundItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serialoutbounditem._self') }),
)

const loading = ref(false)
const dataSource = ref<SerialOutboundItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SerialOutboundItem | null>(null)
const selectedRows = ref<SerialOutboundItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SerialOutboundItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  outboundNo: '',
  lineNumber: undefined as number | undefined,
  outboundSerialNo: '',
  referenceInboundId: '',
  referenceInboundNo: '',
  referenceInboundLineNumber: undefined as number | undefined,
  outboundTimeStart: '',
  outboundTimeEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'outboundNo', label: t('entity.serialoutbounditem.outboundno') },
  { key: 'lineNumber', label: t('entity.serialoutbounditem.linenumber') },
  { key: 'outboundSerialNo', label: t('entity.serialoutbounditem.outboundserialno') },
  { key: 'referenceInboundId', label: t('entity.serialoutbounditem.referenceinboundid') },
  { key: 'referenceInboundNo', label: t('entity.serialoutbounditem.referenceinboundno') },
  { key: 'referenceInboundLineNumber', label: t('entity.serialoutbounditem.referenceinboundlinenumber') },
  { key: 'outboundTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serialoutbounditem.outboundtime')) },
  { key: 'outboundTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serialoutbounditem.outboundtime')) },
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
  outboundNo: '',
  lineNumber: undefined as number | undefined,
  outboundSerialNo: '',
  referenceInboundId: '',
  referenceInboundNo: '',
  referenceInboundLineNumber: undefined as number | undefined,
  outboundTimeStart: '',
  outboundTimeEnd: '',
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

const entityIdName = 'serialOutboundItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.serialOutboundId)
const masterSerialOutboundId = computed(() => selectedMasterRow.value?.serialOutboundId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSerialOutboundItemId(record: SerialOutboundItem | Record<string, unknown>): string {
  return String((record as SerialOutboundItem)?.[entityIdName] ?? '')
}

function getSerialOutboundItemField(record: SerialOutboundItem | Record<string, unknown>, field: string): unknown {
  return (record as SerialOutboundItem)?.[field as keyof SerialOutboundItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'serialOutboundItemId',
    key: 'serialOutboundItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'serialOutboundItemId') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.outboundid'),
    dataIndex: 'outboundId',
    key: 'outboundId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'outboundId') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.outboundno'),
    dataIndex: 'outboundNo',
    key: 'outboundNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'outboundNo') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.outboundserialno'),
    dataIndex: 'outboundSerialNo',
    key: 'outboundSerialNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'outboundSerialNo') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.referenceinboundid'),
    dataIndex: 'referenceInboundId',
    key: 'referenceInboundId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'referenceInboundId') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.referenceinboundno'),
    dataIndex: 'referenceInboundNo',
    key: 'referenceInboundNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'referenceInboundNo') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.referenceinboundlinenumber'),
    dataIndex: 'referenceInboundLineNumber',
    key: 'referenceInboundLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'referenceInboundLineNumber') ?? ''),
  },
  {
    title: t('entity.serialoutbounditem.outboundtime'),
    dataIndex: 'outboundTime',
    key: 'outboundTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SerialOutboundItem }) =>
      String(getSerialOutboundItemField(record, 'outboundTime') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:serial:outbound:update',
        onClick: (record: SerialOutboundItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:serial:outbound:delete',
        onClick: (record: SerialOutboundItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SerialOutboundItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SerialOutboundItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSerialOutboundItemId(selectedRow.value) === getSerialOutboundItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SerialOutboundItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SerialOutboundItem) {
  const key = getSerialOutboundItemId(record)
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
 * @returns {SerialOutboundItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SerialOutboundItemQuery>): SerialOutboundItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SerialOutboundItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    outboundId: masterSerialOutboundId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SerialOutboundItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('outboundNo', form.outboundNo)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('outboundSerialNo', form.outboundSerialNo)
  assignTrimmed('referenceInboundId', form.referenceInboundId)
  assignTrimmed('referenceInboundNo', form.referenceInboundNo)
  if (form.referenceInboundLineNumber !== undefined && form.referenceInboundLineNumber !== null) {
    query.referenceInboundLineNumber = form.referenceInboundLineNumber
  }
  assignTrimmed('outboundTimeStart', form.outboundTimeStart)
  assignTrimmed('outboundTimeEnd', form.outboundTimeEnd)
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
    const res = await getSerialOutboundItemList(buildListQuery())
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
watch(masterSerialOutboundId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serialoutbounditem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SerialOutboundItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serialoutbounditem._self') })
  formLoading.value = true
  try {
    const detail = await getSerialOutboundItemById(getSerialOutboundItemId(record))
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
      entity: t('entity.serialoutbounditem._self'),
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
    const id = formData.value?.serialOutboundItemId
    if (id) {
      await updateSerialOutboundItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.serialoutbounditem._self') }))
    } else {
      await createSerialOutboundItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.serialoutbounditem._self') }))
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

async function handleDeleteOne(record: SerialOutboundItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.serialoutbounditem._self'),
      name: t('common.tip.this.target', { target: t('entity.serialoutbounditem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSerialOutboundItemById(getSerialOutboundItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.serialoutbounditem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.serialoutbounditem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.serialoutbounditem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getSerialOutboundItemId(r)).filter(Boolean)
      await deleteSerialOutboundItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serialoutbounditem._self') }))
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
  const res = await getSerialOutboundItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSerialOutboundItem(file, sheetName)
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
    const exportMeta = await exportSerialOutboundItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.serialoutbounditem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.serialoutbounditem._self') }))
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
