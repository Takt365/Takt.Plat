<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-transfer -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工调动记录管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-personnel-employee-transfer">
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
      create-permission="humanresource:personnel:employeetransfer:create"
      update-permission="humanresource:personnel:employeetransfer:update"
      delete-permission="humanresource:personnel:employeetransfer:delete"
      import-permission="humanresource:personnel:employeetransfer:import"
      export-permission="humanresource:personnel:employeetransfer:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
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
      @import="handleImport"
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
      :row-key="getEmployeeTransferId"
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
      <EmployeeTransferForm
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
      <a-form-item :label="t('entity.employeeTransfer.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.employeeid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.transfertype')">
        <a-input
          v-model:value="advancedQueryForm.transferType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.transfertype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.fromdeptid')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.fromdeptid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.fromdeptname')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.fromdeptname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.frompostid')">
        <a-input
          v-model:value="advancedQueryForm.fromPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.frompostid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.frompostname')">
        <a-input
          v-model:value="advancedQueryForm.fromPostName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.frompostname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.todeptid')">
        <a-input
          v-model:value="advancedQueryForm.toDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.todeptid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.employeeTransfer.todeptname')">
        <a-input
          v-model:value="advancedQueryForm.toDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeTransfer.todeptname') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.employeeTransfer._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeeTransfer._self"
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'employeeTransferId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 员工调动记录管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/personnel/employee-transfer
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import EmployeeTransferForm from './components/employee-transfer-form.vue'
import { getEmployeeTransferList, getEmployeeTransferById, createEmployeeTransfer, updateEmployeeTransfer, deleteEmployeeTransferById, deleteEmployeeTransferBatch, getEmployeeTransferTemplate, importEmployeeTransfer, exportEmployeeTransfer } from '@/api/human-resource/personnel/employee-transfer'
import type { EmployeeTransfer, EmployeeTransferQuery, EmployeeTransferCreate, EmployeeTransferUpdate } from '@/types/human-resource/personnel/employee-transfer'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktEmployeeTransfer')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeeTransfer._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<EmployeeTransfer[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<EmployeeTransfer | null>(null)
const selectedRows = ref<EmployeeTransfer[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EmployeeTransfer>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  employeeId: '',
  transferType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'employeeTransferId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeTransferId',
    key: 'employeeTransferId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'employeeTransferId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.employeeTransfer.transfertype'),
    dataIndex: 'transferType',
    key: 'transferType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'transferType') ?? ''
  },
  {
    title: t('entity.employeeTransfer.fromdeptid'),
    dataIndex: 'fromDeptId',
    key: 'fromDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'fromDeptId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.fromdeptname'),
    dataIndex: 'fromDeptName',
    key: 'fromDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'fromDeptName') ?? ''
  },
  {
    title: t('entity.employeeTransfer.frompostid'),
    dataIndex: 'fromPostId',
    key: 'fromPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'fromPostId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.frompostname'),
    dataIndex: 'fromPostName',
    key: 'fromPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'fromPostName') ?? ''
  },
  {
    title: t('entity.employeeTransfer.todeptid'),
    dataIndex: 'toDeptId',
    key: 'toDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'toDeptId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.todeptname'),
    dataIndex: 'toDeptName',
    key: 'toDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'toDeptName') ?? ''
  },
  {
    title: t('entity.employeeTransfer.topostid'),
    dataIndex: 'toPostId',
    key: 'toPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'toPostId') ?? ''
  },
  {
    title: t('entity.employeeTransfer.topostname'),
    dataIndex: 'toPostName',
    key: 'toPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'toPostName') ?? ''
  },
  {
    title: t('entity.employeeTransfer.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeTransferField(record, 'effectiveDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:personnel:employeetransfer:update',
        onClick: (record: EmployeeTransfer) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:personnel:employeetransfer:delete',
        onClick: (record: EmployeeTransfer) => handleDeleteOne(record)
      }
    ]
  })
])

const getEmployeeTransferId = (record: any): string => record?.[entityIdName] ?? ''
const getEmployeeTransferField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: EmployeeTransfer[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeTransfer, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEmployeeTransferId(selectedRow.value) === getEmployeeTransferId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeTransfer[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: EmployeeTransfer) => ({
  onClick: () => {
    const key = getEmployeeTransferId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeTransferId(item)))
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
    const params: EmployeeTransferQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEmployeeTransferList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EmployeeTransfer] 加载数据失败', { error })
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
  employeeId: '',
  transferType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.employeeTransfer._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: EmployeeTransfer) {
  formTitle.value = t('common.page.button.edit') + t('entity.employeeTransfer._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeeTransfer._self') }))
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
      await updateEmployeeTransfer(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employeeTransfer._self') }))
    } else {
      await createEmployeeTransfer(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeeTransfer._self') }))
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
function handleImport() {
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEmployeeTransferTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployeeTransfer(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: EmployeeTransferQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEmployeeTransfer(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeeTransfer._self') }))
  } catch (error: any) {
    logger.error('[EmployeeTransfer] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeeTransfer._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: EmployeeTransfer) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeeTransfer._self'), name: t('common.tip.this.target', { target: t('entity.employeeTransfer._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeTransferById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeeTransfer._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeeTransfer._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeeTransfer._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeTransferBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeeTransfer._self') }))
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
  employeeId: '',
  transferType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
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
.human-resource-personnel-employee-transfer {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
