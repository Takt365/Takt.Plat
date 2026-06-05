<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/statistics/logging/login-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：登录日志实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-logging-login-log">
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
      create-permission="statistics:logging:loginlog:create"
      update-permission="statistics:logging:loginlog:update"
      delete-permission="statistics:logging:loginlog:delete"

      export-permission="statistics:logging:loginlog:export"
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
      :row-key="getLoginLogId"
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
      <LoginLogForm
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
      <a-form-item :label="t('entity.loginLog.username')">
        <a-input
          v-model:value="advancedQueryForm.username"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.username') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.logintype')">
        <a-input
          v-model:value="advancedQueryForm.loginType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.logintype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.browser')">
        <a-input
          v-model:value="advancedQueryForm.browser"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.browser') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.os')">
        <a-input
          v-model:value="advancedQueryForm.os"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.os') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.useragent')">
        <a-input
          v-model:value="advancedQueryForm.userAgent"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.useragent') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.loginresult')">
        <a-input
          v-model:value="advancedQueryForm.loginResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.loginresult') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.loginmessage')">
        <a-input
          v-model:value="advancedQueryForm.loginMessage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.loginmessage') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.loginLog.loginip')">
        <a-input
          v-model:value="advancedQueryForm.loginIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginLog.loginip') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'loginLogId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 登录日志实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/logging/login-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import LoginLogForm from './components/login-log-form.vue'
import { getLoginLogList, getLoginLogById, createLoginLog, updateLoginLog, deleteLoginLogById, deleteLoginLogBatch, exportLoginLogData } from '@/api/statistics/logging/login-log'
import type { LoginLog, LoginLogQuery, LoginLogCreate, LoginLogUpdate } from '@/types/statistics/logging/login-log'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktLoginLog')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.loginLog._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<LoginLog[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<LoginLog | null>(null)
const selectedRows = ref<LoginLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<LoginLog>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  username: '',
  loginType: '',
  browser: '',
  os: '',
  userAgent: '',
  loginResult: undefined as number | undefined,
  loginMessage: '',
  loginIp: '',
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'loginLogId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'loginLogId',
    key: 'loginLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginLogId') ?? ''
  },
  {
    title: t('entity.loginLog.username'),
    dataIndex: 'username',
    key: 'username',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'username') ?? ''
  },
  {
    title: t('entity.loginLog.logintype'),
    dataIndex: 'loginType',
    key: 'loginType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginType') ?? ''
  },
  {
    title: t('entity.loginLog.browser'),
    dataIndex: 'browser',
    key: 'browser',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'browser') ?? ''
  },
  {
    title: t('entity.loginLog.os'),
    dataIndex: 'os',
    key: 'os',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'os') ?? ''
  },
  {
    title: t('entity.loginLog.useragent'),
    dataIndex: 'userAgent',
    key: 'userAgent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'userAgent') ?? ''
  },
  {
    title: t('entity.loginLog.loginresult'),
    dataIndex: 'loginResult',
    key: 'loginResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginResult') ?? ''
  },
  {
    title: t('entity.loginLog.loginmessage'),
    dataIndex: 'loginMessage',
    key: 'loginMessage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginMessage') ?? ''
  },
  {
    title: t('entity.loginLog.loginip'),
    dataIndex: 'loginIp',
    key: 'loginIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginIp') ?? ''
  },
  {
    title: t('entity.loginLog.loginlocation'),
    dataIndex: 'loginLocation',
    key: 'loginLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginLocation') ?? ''
  },
  {
    title: t('entity.loginLog.logoutat'),
    dataIndex: 'logoutAt',
    key: 'logoutAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'logoutAt') ?? ''
  },
  {
    title: t('entity.loginLog.sessionduration'),
    dataIndex: 'sessionDuration',
    key: 'sessionDuration',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'sessionDuration') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:logging:loginlog:update',
        onClick: (record: LoginLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:loginlog:delete',
        onClick: (record: LoginLog) => handleDeleteOne(record)
      }
    ]
  })
])

const getLoginLogId = (record: any): string => record?.[entityIdName] ?? ''
const getLoginLogField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: LoginLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: LoginLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getLoginLogId(selectedRow.value) === getLoginLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: LoginLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: LoginLog) => ({
  onClick: () => {
    const key = getLoginLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getLoginLogId(item)))
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
    const params: LoginLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getLoginLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[LoginLog] 加载数据失败', { error })
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
  username: '',
  loginType: '',
  browser: '',
  os: '',
  userAgent: '',
  loginResult: undefined as number | undefined,
  loginMessage: '',
  loginIp: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.loginLog._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: LoginLog) {
  formTitle.value = t('common.page.button.edit') + t('entity.loginLog._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.loginLog._self') }))
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
      await updateLoginLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.loginLog._self') }))
    } else {
      await createLoginLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.loginLog._self') }))
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
    const exportQuery: LoginLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportLoginLogData(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.loginLog._self') }))
  } catch (error: any) {
    logger.error('[LoginLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.loginLog._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: LoginLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.loginLog._self'), name: t('common.tip.this.target', { target: t('entity.loginLog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteLoginLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.loginLog._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.loginLog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.loginLog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteLoginLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.loginLog._self') }))
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
  username: '',
  loginType: '',
  browser: '',
  os: '',
  userAgent: '',
  loginResult: undefined as number | undefined,
  loginMessage: '',
  loginIp: '',
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
.statistics-logging-login-log {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
