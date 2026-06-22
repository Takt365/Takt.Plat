<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/it-asset-change-log/components -->
<!-- 文件名称：it-asset-change-log-panel.vue -->
<!-- 功能描述：服务台 IT 设备保修扩展实体主表实体右侧明细 itAssetChangeLog 独立 CRUD（按主表选中 itAssetId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="it-asset-change-log-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.itassetchangelog._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="routine:helpdesk:itasset:create"
      update-permission="routine:helpdesk:itasset:update"
      delete-permission="routine:helpdesk:itasset:delete"

      export-permission="routine:helpdesk:itasset:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"

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
    <div class="it-asset-change-log-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getItAssetChangeLogId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="itAssetChangeLogId"
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
      <ItAssetChangeLogForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterItAssetId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-routine-help-desk-it-asset-change-log-it-asset-change-log"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="t('entity.itassetchangelog.assetcode')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itassetchangelog.assetcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeType')">
      <a-form-item :label="t('entity.itassetchangelog.changetype')">
        <a-input-number
          v-model:value="advancedQueryForm.changeType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itassetchangelog.changetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeSummary')">
      <a-form-item :label="t('entity.itassetchangelog.changesummary')">
        <a-input
          v-model:value="advancedQueryForm.changeSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itassetchangelog.changesummary') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeFields')">
      <a-form-item :label="t('entity.itassetchangelog.changefields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itassetchangelog.changefields') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeReason')">
      <a-form-item :label="t('entity.itassetchangelog.changereason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itassetchangelog.changereason') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.itassetchangelog.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.itassetchangelog.extfield') })"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="itAssetChangeLogId"
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
 * 服务台 IT 设备保修扩展实体子表 itAssetChangeLog 右栏面板
 * @module views/routine/help-desk/it-asset-change-log/components
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
import ItAssetChangeLogForm from './it-asset-change-log-form.vue'
import { useItAssetMasterContext } from '../composables/use-it-asset-master-context'
import {
  getItAssetChangeLogList,
  getItAssetChangeLogById,
  createItAssetChangeLog,
  updateItAssetChangeLog,
  deleteItAssetChangeLogById,
  deleteItAssetChangeLogBatch,
  exportItAssetChangeLog,
} from '@/api/routine/help-desk/it-asset-change-log'
import type { ItAssetChangeLog, ItAssetChangeLogQuery } from '@/types/routine/help-desk/it-asset-change-log'

const { t } = useI18n()
const { selectedMasterRow } = useItAssetMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktItAssetChangeLog')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.itassetchangelog._self') }),
)

const loading = ref(false)
const dataSource = ref<ItAssetChangeLog[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ItAssetChangeLog | null>(null)
const selectedRows = ref<ItAssetChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ItAssetChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  assetCode: '',
  changeType: undefined as number | undefined,
  changeSummary: '',
  changeFields: '',
  changeReason: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'assetCode', label: t('entity.itassetchangelog.assetcode') },
  { key: 'changeType', label: t('entity.itassetchangelog.changetype') },
  { key: 'changeSummary', label: t('entity.itassetchangelog.changesummary') },
  { key: 'changeFields', label: t('entity.itassetchangelog.changefields') },
  { key: 'changeReason', label: t('entity.itassetchangelog.changereason') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.itassetchangelog.extfield') },
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
  assetCode: '',
  changeType: undefined as number | undefined,
  changeSummary: '',
  changeFields: '',
  changeReason: '',
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

const entityIdName = 'itAssetChangeLogId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.itAssetId)
const masterItAssetId = computed(() => selectedMasterRow.value?.itAssetId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getItAssetChangeLogId(record: ItAssetChangeLog | Record<string, unknown>): string {
  return String((record as ItAssetChangeLog)?.[entityIdName] ?? '')
}

function getItAssetChangeLogField(record: ItAssetChangeLog | Record<string, unknown>, field: string): unknown {
  return (record as ItAssetChangeLog)?.[field as keyof ItAssetChangeLog]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'itAssetChangeLogId',
    key: 'itAssetChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'itAssetChangeLogId') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.assetcode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'assetCode') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'changeType') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'changeSummary') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'changeFields') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'changeReason') ?? ''),
  },
  {
    title: t('entity.itassetchangelog.itasset'),
    dataIndex: 'itAsset',
    key: 'itAsset',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ItAssetChangeLog }) =>
      String(getItAssetChangeLogField(record, 'itAsset') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:helpdesk:itasset:update',
        onClick: (record: ItAssetChangeLog) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:helpdesk:itasset:delete',
        onClick: (record: ItAssetChangeLog) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ItAssetChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ItAssetChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getItAssetChangeLogId(selectedRow.value) === getItAssetChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ItAssetChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ItAssetChangeLog) {
  const key = getItAssetChangeLogId(record)
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
 * @returns {ItAssetChangeLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ItAssetChangeLogQuery>): ItAssetChangeLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ItAssetChangeLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    itAssetId: masterItAssetId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ItAssetChangeLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('assetCode', form.assetCode)
  if (form.changeType !== undefined && form.changeType !== null) {
    query.changeType = form.changeType
  }
  assignTrimmed('changeSummary', form.changeSummary)
  assignTrimmed('changeFields', form.changeFields)
  assignTrimmed('changeReason', form.changeReason)
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
    const res = await getItAssetChangeLogList(buildListQuery())
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
watch(masterItAssetId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.itassetchangelog._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ItAssetChangeLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.itassetchangelog._self') })
  formLoading.value = true
  try {
    const detail = await getItAssetChangeLogById(getItAssetChangeLogId(record))
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
      entity: t('entity.itassetchangelog._self'),
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
    const id = formData.value?.itAssetChangeLogId
    if (id) {
      await updateItAssetChangeLog(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.itassetchangelog._self') }))
    } else {
      await createItAssetChangeLog(payload)
      message.success(t('common.feedback.created', { target: t('entity.itassetchangelog._self') }))
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

async function handleDeleteOne(record: ItAssetChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.itassetchangelog._self'),
      name: t('common.tip.this.target', { target: t('entity.itassetchangelog._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteItAssetChangeLogById(getItAssetChangeLogId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.itassetchangelog._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.itassetchangelog._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.itassetchangelog._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getItAssetChangeLogId(r)).filter(Boolean)
      await deleteItAssetChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.itassetchangelog._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportItAssetChangeLog(
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
    message.success(t('common.feedback.export.success', { target: t('entity.itassetchangelog._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.itassetchangelog._self') }))
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
