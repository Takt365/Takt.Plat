<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/logging/oper-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：操作日志实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-logging-oper-log">
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
      create-permission="statistics:logging:operlog:create"
      update-permission="statistics:logging:operlog:update"
      delete-permission="statistics:logging:operlog:delete"

      export-permission="statistics:logging:operlog:export"
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
      :row-key="getOperLogId"
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
      <OperLogForm
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
      <a-form-item :label="t('entity.operLog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.username') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.opermodule')">
        <a-input
          v-model:value="advancedQueryForm.operModule"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opermodule') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.opertype')">
        <a-input
          v-model:value="advancedQueryForm.operType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opertype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.opermethod')">
        <a-input
          v-model:value="advancedQueryForm.operMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opermethod') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.requestmethod')">
        <a-input
          v-model:value="advancedQueryForm.requestMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.requestmethod') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.operurl')">
        <a-input
          v-model:value="advancedQueryForm.operUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.operurl') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.requestparam')">
        <a-input
          v-model:value="advancedQueryForm.requestParam"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.requestparam') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.operLog.jsonresult')">
        <a-input
          v-model:value="advancedQueryForm.jsonResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.jsonresult') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'operLogId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 操作日志实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/logging/oper-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import OperLogForm from './components/oper-log-form.vue'
import { getOperLogList, getOperLogById, createOperLog, updateOperLog, deleteOperLogById, deleteOperLogBatch, exportOperLog } from '@/api/statistics/logging/oper-log'
import type { OperLog, OperLogQuery, OperLogCreate, OperLogUpdate } from '@/types/statistics/logging/oper-log'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktOperLog')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.operLog._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<OperLog[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<OperLog | null>(null)
const selectedRows = ref<OperLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<OperLog>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  userName: '',
  operModule: '',
  operType: '',
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'operLogId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'operLogId',
    key: 'operLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operLogId') ?? ''
  },
  {
    title: t('entity.operLog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.operLog.opermodule'),
    dataIndex: 'operModule',
    key: 'operModule',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operModule') ?? ''
  },
  {
    title: t('entity.operLog.opertype'),
    dataIndex: 'operType',
    key: 'operType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operType') ?? ''
  },
  {
    title: t('entity.operLog.opermethod'),
    dataIndex: 'operMethod',
    key: 'operMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operMethod') ?? ''
  },
  {
    title: t('entity.operLog.requestmethod'),
    dataIndex: 'requestMethod',
    key: 'requestMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'requestMethod') ?? ''
  },
  {
    title: t('entity.operLog.operurl'),
    dataIndex: 'operUrl',
    key: 'operUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operUrl') ?? ''
  },
  {
    title: t('entity.operLog.requestparam'),
    dataIndex: 'requestParam',
    key: 'requestParam',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'requestParam') ?? ''
  },
  {
    title: t('entity.operLog.jsonresult'),
    dataIndex: 'jsonResult',
    key: 'jsonResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'jsonResult') ?? ''
  },
  {
    title: t('entity.operLog.operstatus'),
    dataIndex: 'operStatus',
    key: 'operStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operStatus') ?? ''
  },
  {
    title: t('entity.operLog.errormsg'),
    dataIndex: 'errorMsg',
    key: 'errorMsg',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'errorMsg') ?? ''
  },
  {
    title: t('entity.operLog.operip'),
    dataIndex: 'operIp',
    key: 'operIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operIp') ?? ''
  },
  {
    title: t('entity.operLog.operlocation'),
    dataIndex: 'operLocation',
    key: 'operLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operLocation') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:logging:operlog:update',
        onClick: (record: OperLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:operlog:delete',
        onClick: (record: OperLog) => handleDeleteOne(record)
      }
    ]
  })
])

const getOperLogId = (record: any): string => record?.[entityIdName] ?? ''
const getOperLogField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: OperLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: OperLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getOperLogId(selectedRow.value) === getOperLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: OperLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: OperLog) => ({
  onClick: () => {
    const key = getOperLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getOperLogId(item)))
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
    const params: OperLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getOperLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[OperLog] 加载数据失败', { error })
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
  userName: '',
  operModule: '',
  operType: '',
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.operLog._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: OperLog) {
  formTitle.value = t('common.page.button.edit') + t('entity.operLog._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.operLog._self') }))
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
      await updateOperLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.operLog._self') }))
    } else {
      await createOperLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.operLog._self') }))
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
    const exportQuery: OperLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportOperLog(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.operLog._self') }))
  } catch (error: any) {
    logger.error('[OperLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.operLog._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: OperLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.operLog._self'), name: t('common.tip.this.target', { target: t('entity.operLog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOperLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.operLog._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.operLog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.operLog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteOperLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.operLog._self') }))
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
  userName: '',
  operModule: '',
  operType: '',
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
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
.statistics-logging-oper-log {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
