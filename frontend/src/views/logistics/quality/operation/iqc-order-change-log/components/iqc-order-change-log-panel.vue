<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order-change-log/components -->
<!-- 文件名称：iqc-order-change-log-panel.vue -->
<!-- 功能描述：IQC进货检验单实体主表实体右侧明细 iqcOrderChangeLog 独立 CRUD（按主表选中 iqcOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="iqc-order-change-log-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.iqcorderchangelog._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:iqcorder:create"
      update-permission="logistics:quality:operation:iqcorder:update"
      delete-permission="logistics:quality:operation:iqcorder:delete"

      export-permission="logistics:quality:operation:iqcorder:export"
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
    <div class="iqc-order-change-log-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getIqcOrderChangeLogId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="iqcOrderChangeLogId"
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
      <IqcOrderChangeLogForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterIqcOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-iqc-order-change-log-iqc-order-change-log"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('changeFields')">
      <a-form-item :label="t('entity.iqcorderchangelog.changefields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderchangelog.changefields') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeType')">
      <a-form-item :label="t('entity.iqcorderchangelog.changetype')">
        <a-input-number
          v-model:value="advancedQueryForm.changeType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderchangelog.changetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeReason')">
      <a-form-item :label="t('entity.iqcorderchangelog.changereason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderchangelog.changereason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeBy')">
      <a-form-item :label="t('entity.iqcorderchangelog.changeby')">
        <a-input
          v-model:value="advancedQueryForm.changeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderchangelog.changeby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeStart')">
      <a-form-item :label="t('entity.iqcorderchangelog.changetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorderchangelog.changetimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeEnd')">
      <a-form-item :label="t('entity.iqcorderchangelog.changetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorderchangelog.changetimeend') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.iqcorderchangelog.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcorderchangelog.extfield') })"
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
      id-column-key="iqcOrderChangeLogId"
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
 * IQC进货检验单实体子表 iqcOrderChangeLog 右栏面板
 * @module views/logistics/quality/operation/iqc-order-change-log/components
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
import IqcOrderChangeLogForm from './iqc-order-change-log-form.vue'
import { useIqcOrderMasterContext } from '../composables/use-iqc-order-master-context'
import {
  getIqcOrderChangeLogList,
  getIqcOrderChangeLogById,
  createIqcOrderChangeLog,
  updateIqcOrderChangeLog,
  deleteIqcOrderChangeLogById,
  deleteIqcOrderChangeLogBatch,
  exportIqcOrderChangeLog,
} from '@/api/logistics/quality/operation/iqc-order-change-log'
import type { IqcOrderChangeLog, IqcOrderChangeLogQuery } from '@/types/logistics/quality/operation/iqc-order-change-log'

const { t } = useI18n()
const { selectedMasterRow } = useIqcOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIqcOrderChangeLog')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.iqcorderchangelog._self') }),
)

const loading = ref(false)
const dataSource = ref<IqcOrderChangeLog[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<IqcOrderChangeLog | null>(null)
const selectedRows = ref<IqcOrderChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<IqcOrderChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  changeFields: '',
  changeType: undefined as number | undefined,
  changeReason: '',
  changeBy: '',
  changeTimeStart: '',
  changeTimeEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'changeFields', label: t('entity.iqcorderchangelog.changefields') },
  { key: 'changeType', label: t('entity.iqcorderchangelog.changetype') },
  { key: 'changeReason', label: t('entity.iqcorderchangelog.changereason') },
  { key: 'changeBy', label: t('entity.iqcorderchangelog.changeby') },
  { key: 'changeTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.iqcorderchangelog.changetime')) },
  { key: 'changeTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.iqcorderchangelog.changetime')) },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.iqcorderchangelog.extfield') },
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
  changeFields: '',
  changeType: undefined as number | undefined,
  changeReason: '',
  changeBy: '',
  changeTimeStart: '',
  changeTimeEnd: '',
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

const entityIdName = 'iqcOrderChangeLogId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.iqcOrderId)
const masterIqcOrderId = computed(() => selectedMasterRow.value?.iqcOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getIqcOrderChangeLogId(record: IqcOrderChangeLog | Record<string, unknown>): string {
  return String((record as IqcOrderChangeLog)?.[entityIdName] ?? '')
}

function getIqcOrderChangeLogField(record: IqcOrderChangeLog | Record<string, unknown>, field: string): unknown {
  return (record as IqcOrderChangeLog)?.[field as keyof IqcOrderChangeLog]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'iqcOrderChangeLogId',
    key: 'iqcOrderChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'iqcOrderChangeLogId') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'changeFields') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'changeType') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'changeReason') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'changeBy') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'changeTime') ?? ''),
  },
  {
    title: t('entity.iqcorderchangelog.order'),
    dataIndex: 'order',
    key: 'order',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderChangeLog }) =>
      String(getIqcOrderChangeLogField(record, 'order') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:iqcorder:update',
        onClick: (record: IqcOrderChangeLog) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:iqcorder:delete',
        onClick: (record: IqcOrderChangeLog) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IqcOrderChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IqcOrderChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getIqcOrderChangeLogId(selectedRow.value) === getIqcOrderChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IqcOrderChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: IqcOrderChangeLog) {
  const key = getIqcOrderChangeLogId(record)
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
 * @returns {IqcOrderChangeLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IqcOrderChangeLogQuery>): IqcOrderChangeLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IqcOrderChangeLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    iqcOrderId: masterIqcOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IqcOrderChangeLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('changeFields', form.changeFields)
  if (form.changeType !== undefined && form.changeType !== null) {
    query.changeType = form.changeType
  }
  assignTrimmed('changeReason', form.changeReason)
  assignTrimmed('changeBy', form.changeBy)
  assignTrimmed('changeTimeStart', form.changeTimeStart)
  assignTrimmed('changeTimeEnd', form.changeTimeEnd)
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
    const res = await getIqcOrderChangeLogList(buildListQuery())
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
watch(masterIqcOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.iqcorderchangelog._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: IqcOrderChangeLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.iqcorderchangelog._self') })
  formLoading.value = true
  try {
    const detail = await getIqcOrderChangeLogById(getIqcOrderChangeLogId(record))
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
      entity: t('entity.iqcorderchangelog._self'),
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
    const id = formData.value?.iqcOrderChangeLogId
    if (id) {
      await updateIqcOrderChangeLog(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.iqcorderchangelog._self') }))
    } else {
      await createIqcOrderChangeLog(payload)
      message.success(t('common.feedback.created', { target: t('entity.iqcorderchangelog._self') }))
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

async function handleDeleteOne(record: IqcOrderChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.iqcorderchangelog._self'),
      name: t('common.tip.this.target', { target: t('entity.iqcorderchangelog._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIqcOrderChangeLogById(getIqcOrderChangeLogId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.iqcorderchangelog._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.iqcorderchangelog._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.iqcorderchangelog._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getIqcOrderChangeLogId(r)).filter(Boolean)
      await deleteIqcOrderChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.iqcorderchangelog._self') }))
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
    const exportMeta = await exportIqcOrderChangeLog(
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
    message.success(t('common.feedback.export.success', { target: t('entity.iqcorderchangelog._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.iqcorderchangelog._self') }))
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
