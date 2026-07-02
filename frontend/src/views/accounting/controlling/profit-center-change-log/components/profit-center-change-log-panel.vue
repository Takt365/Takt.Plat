<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/profit-center-change-log/components -->
<!-- 文件名称：profit-center-change-log-panel.vue -->
<!-- 功能描述：利润中心实体主表实体右侧明细 profitCenterChangeLog 独立 CRUD（按主表选中 profitCenterId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="profit-center-change-log-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ pi.self() }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="accounting:controlling:profit:center:create"
      update-permission="accounting:controlling:profit:center:update"
      delete-permission="accounting:controlling:profit:center:delete"

      export-permission="accounting:controlling:profit:center:export"
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
    <div class="profit-center-change-log-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getProfitCenterChangeLogId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="profitCenterChangeLogId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
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
      <ProfitCenterChangeLogForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterProfitCenterId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-accounting-controlling-profit-center-change-log-profit-center-change-log"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('profitCenterCode')">
      <a-form-item :label="pi.queryLabel('profitCenterCode')">
        <a-input
          v-model:value="advancedQueryForm.profitCenterCode"
          :placeholder="pi.queryPh('profitCenterCode', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeFields')">
      <a-form-item :label="pi.queryLabel('changeFields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="pi.queryPh('changeFields', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeStart')">
      <a-form-item :label="pi.queryLabel('changeTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeStart"
          :placeholder="pi.queryPh('changeTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeEnd')">
      <a-form-item :label="pi.queryLabel('changeTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeEnd"
          :placeholder="pi.queryPh('changeTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeBy')">
      <a-form-item :label="pi.queryLabel('changeBy')">
        <a-input
          v-model:value="advancedQueryForm.changeBy"
          :placeholder="pi.queryPh('changeBy', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeReason')">
      <a-form-item :label="pi.queryLabel('changeReason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="pi.queryPh('changeReason', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      id-column-key="profitCenterChangeLogId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 利润中心实体子表 profitCenterChangeLog 右栏面板
 * @module views/accounting/controlling/profit-center-change-log/components
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
import ProfitCenterChangeLogForm from './profit-center-change-log-form.vue'
import { useProfitCenterMasterContext } from '../composables/use-profit-center-master-context'
import {
  getProfitCenterChangeLogList,
  getProfitCenterChangeLogById,
  createProfitCenterChangeLog,
  updateProfitCenterChangeLog,
  deleteProfitCenterChangeLogById,
  deleteProfitCenterChangeLogBatch,
  exportProfitCenterChangeLog,
} from '@/api/accounting/controlling/profit-center-change-log'
import type { ProfitCenterChangeLog, ProfitCenterChangeLogQuery } from '@/types/accounting/controlling/profit-center-change-log'

import {
  useProfitCenterChangeLogI18n,
  PROFITCENTERCHANGELOG_LIST_FIELDS,
  PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS,
  PROFITCENTERCHANGELOG_QUERY_FIELDS,
  PROFITCENTERCHANGELOG_SELF_I18N_KEY,
} from '../composables/use-profit-center-change-log-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useProfitCenterChangeLogI18n()

const { t } = useI18n()
const { selectedMasterRow } = useProfitCenterMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktProfitCenterChangeLog')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
const dataSource = ref<ProfitCenterChangeLog[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ProfitCenterChangeLog | null>(null)
const selectedRows = ref<ProfitCenterChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ProfitCenterChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,

  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  PROFITCENTERCHANGELOG_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
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

const entityIdName = 'profitCenterChangeLogId'
const masterProfitCenterId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['profitCenterId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterProfitCenterId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getProfitCenterChangeLogId(record: ProfitCenterChangeLog | Record<string, unknown>): string {
  return String((record as ProfitCenterChangeLog)?.[entityIdName] ?? '')
}

function getProfitCenterChangeLogField(record: ProfitCenterChangeLog | Record<string, unknown>, field: string): unknown {
  return (record as ProfitCenterChangeLog)?.[field as keyof ProfitCenterChangeLog]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'profitCenterChangeLogId',
    key: 'profitCenterChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'profitCenterChangeLogId') ?? ''),
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'profitCenterCode') ?? ''),
  },
  {
    title: pi.label('changeFields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'changeFields') ?? ''),
  },
  {
    title: pi.label('changeTime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'changeTime') ?? ''),
  },
  {
    title: pi.label('changeBy'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'changeBy') ?? ''),
  },
  {
    title: pi.label('changeReason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'changeReason') ?? ''),
  },
  {
    title: pi.label('profitCenter'),
    dataIndex: 'profitCenter',
    key: 'profitCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProfitCenterChangeLog }) =>
      String(getProfitCenterChangeLogField(record, 'profitCenter') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:controlling:profit:center:update',
        onClick: (record: ProfitCenterChangeLog) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:profit:center:delete',
        onClick: (record: ProfitCenterChangeLog) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ProfitCenterChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ProfitCenterChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getProfitCenterChangeLogId(selectedRow.value) === getProfitCenterChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ProfitCenterChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ProfitCenterChangeLog) {
  const key = getProfitCenterChangeLogId(record)
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
 * @returns {ProfitCenterChangeLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ProfitCenterChangeLogQuery>): ProfitCenterChangeLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ProfitCenterChangeLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    profitCenterId: masterProfitCenterId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ProfitCenterChangeLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PROFITCENTERCHANGELOG_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
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
    const res = await getProfitCenterChangeLogList(buildListQuery())
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
watch(masterProfitCenterId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ProfitCenterChangeLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getProfitCenterChangeLogById(getProfitCenterChangeLogId(record))
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
      entity: pi.self(),
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
    const id = formData.value?.profitCenterChangeLogId
    if (id) {
      await updateProfitCenterChangeLog(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createProfitCenterChangeLog(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
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

async function handleDeleteOne(record: ProfitCenterChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProfitCenterChangeLogById(getProfitCenterChangeLogId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getProfitCenterChangeLogId(r)).filter(Boolean)
      await deleteProfitCenterChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
    const exportMeta = await exportProfitCenterChangeLog(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
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
