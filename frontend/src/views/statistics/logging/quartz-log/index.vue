<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/statistics/logging/quartz-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 任务执行日志实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-logging-quartz-log">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="statistics:logging:quartzlog:create"
      update-permission="statistics:logging:quartzlog:update"
      delete-permission="statistics:logging:quartzlog:delete"

      export-permission="statistics:logging:quartzlog:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"

      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      :columns="displayColumns"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQuartzLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :large-screen-column-count="9"
      :small-screen-column-count="5"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <QuartzLogForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.quartzLog.quartztaskid')">
        <a-input
          v-model:value="advancedQueryForm.quartzTaskId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.quartztaskid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.username') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.jobname')">
        <a-input
          v-model:value="advancedQueryForm.jobName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.jobname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.jobgroup')">
        <a-input
          v-model:value="advancedQueryForm.jobGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.jobgroup') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.triggername')">
        <a-input
          v-model:value="advancedQueryForm.triggerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.triggername') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.executestatus')">
        <a-input
          v-model:value="advancedQueryForm.executeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.executestatus') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.errormsg')">
        <a-input
          v-model:value="advancedQueryForm.errorMsg"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.errormsg') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.quartzLog.costtime')">
        <a-input
          v-model:value="advancedQueryForm.costTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzLog.costtime') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'quartzLogId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Quartz 任务执行日志实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/logging/quartz-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import QuartzLogForm from './components/quartz-log-form.vue'
import { getQuartzLogList, getQuartzLogById, createQuartzLog, updateQuartzLog, deleteQuartzLogById, deleteQuartzLogBatch, exportQuartzLog } from '@/api/statistics/logging/quartz-log'
import type { QuartzLog, QuartzLogQuery, QuartzLogCreate, QuartzLogUpdate } from '@/types/statistics/logging/quartz-log'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktQuartzLog')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.quartzLog._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<QuartzLog[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<QuartzLog | null>(null)
const selectedRows = ref<QuartzLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QuartzLog>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  quartzTaskId: '',
  userName: '',
  jobName: '',
  jobGroup: '',
  triggerName: '',
  executeStatus: undefined as number | undefined,
  errorMsg: '',
  costTime: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'quartzLogId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'quartzLogId',
    key: 'quartzLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzLogId') ?? ''
  },
  {
    title: t('entity.quartzLog.quartztaskid'),
    dataIndex: 'quartzTaskId',
    key: 'quartzTaskId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzTaskId') ?? ''
  },
  {
    title: t('entity.quartzLog.quartztaskname'),
    dataIndex: 'quartzTaskName',
    key: 'quartzTaskName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzTaskName') ?? ''
  },
  {
    title: t('entity.quartzLog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.quartzLog.jobname'),
    dataIndex: 'jobName',
    key: 'jobName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'jobName') ?? ''
  },
  {
    title: t('entity.quartzLog.jobgroup'),
    dataIndex: 'jobGroup',
    key: 'jobGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'jobGroup') ?? ''
  },
  {
    title: t('entity.quartzLog.triggername'),
    dataIndex: 'triggerName',
    key: 'triggerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'triggerName') ?? ''
  },
  {
    title: t('entity.quartzLog.executestatus'),
    dataIndex: 'executeStatus',
    key: 'executeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeStatus') ?? ''
  },
  {
    title: t('entity.quartzLog.errormsg'),
    dataIndex: 'errorMsg',
    key: 'errorMsg',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'errorMsg') ?? ''
  },
  {
    title: t('entity.quartzLog.executetime'),
    dataIndex: 'executeTime',
    key: 'executeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeTime') ?? ''
  },
  {
    title: t('entity.quartzLog.costtime'),
    dataIndex: 'costTime',
    key: 'costTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'costTime') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:logging:quartzlog:update',
        onClick: (record: QuartzLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:quartzlog:delete',
        onClick: (record: QuartzLog) => handleDeleteOne(record)
      }
    ]
  })
])

const getQuartzLogId = (record: any): string => record?.[entityIdName] ?? ''
const getQuartzLogField = (record: any, field: string): any => record?.[field]

const mergedColumns = computed((): any => mergeDefaultColumns(columns.value as any, t, true))
const displayColumns = computed(() => {
  const keys = visibleColumnKeys.value || []
  const merged = mergedColumns.value || []
  if (keys.length === 0) return merged
  const keysSet = new Set(keys.map((k: any) => String(k)))
  return merged.filter((col: any) => {
    const colKey = col.key || col.dataIndex || col.title
    return colKey && keysSet.has(String(colKey))
  })
})

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QuartzLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QuartzLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQuartzLogId(selectedRow.value) === getQuartzLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QuartzLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: QuartzLog) => ({
  onClick: () => {
    const key = getQuartzLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQuartzLogId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: QuartzLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQuartzLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QuartzLog] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function handleSearch() {
  currentPage.value = 1
  loadData()
}

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  quartzTaskId: '',
  userName: '',
  jobName: '',
  jobGroup: '',
  triggerName: '',
  executeStatus: undefined as number | undefined,
  errorMsg: '',
  costTime: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.quartzLog._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: QuartzLog) {
  formTitle.value = t('common.page.button.edit') + t('entity.quartzLog._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.quartzLog._self') }))
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
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateQuartzLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.quartzLog._self') }))
    } else {
      await createQuartzLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.quartzLog._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: QuartzLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQuartzLog(exportQuery, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.quartzLog._self') }))
  } catch (error: any) {
    logger.error('[QuartzLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.quartzLog._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: QuartzLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.quartzLog._self'), name: t('common.tip.this.target', { target: t('entity.quartzLog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQuartzLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.quartzLog._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.quartzLog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.quartzLog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQuartzLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.quartzLog._self') }))
      loadData()
    }
  })
}
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  quartzTaskId: '',
  userName: '',
  jobName: '',
  jobGroup: '',
  triggerName: '',
  executeStatus: undefined as number | undefined,
  errorMsg: '',
  costTime: undefined as number | undefined,
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = columns.value.map((c: any) => c.key || c.dataIndex).filter(Boolean)
}

function handleRefresh() {
  loadData()
}

function handleTableChange() {}
function handleResizeColumn() {}
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.statistics-logging-quartz-log {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
