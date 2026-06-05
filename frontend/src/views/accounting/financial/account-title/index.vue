<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会计科目实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-account-title">
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
      create-permission="accounting:financial:accounttitle:create"
      update-permission="accounting:financial:accounttitle:update"
      delete-permission="accounting:financial:accounttitle:delete"
      import-permission="accounting:financial:accounttitle:import"
      export-permission="accounting:financial:accounttitle:export"
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
      :row-key="getAccountTitleId"
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
      <AccountTitleForm
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
      <a-form-item :label="t('entity.accountTitle.titlecode')">
        <a-input
          v-model:value="advancedQueryForm.titleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlecode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.titlename')">
        <a-input
          v-model:value="advancedQueryForm.titleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlename') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.parentid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.titletype')">
        <a-input
          v-model:value="advancedQueryForm.titleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titletype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.balancedirection')">
        <a-input
          v-model:value="advancedQueryForm.balanceDirection"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.balancedirection') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.titlelevel')">
        <a-input
          v-model:value="advancedQueryForm.titleLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlelevel') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.isleaf')">
        <a-input
          v-model:value="advancedQueryForm.isLeaf"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isleaf') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.accountTitle.isauxiliary')">
        <a-input
          v-model:value="advancedQueryForm.isAuxiliary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isauxiliary') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.accountTitle._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.accountTitle._self"
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
      :id-column-key="'accountTitleId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会计科目实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/account-title
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import AccountTitleForm from './components/account-title-form.vue'
import { getAccountTitleList, getAccountTitleById, createAccountTitle, updateAccountTitle, deleteAccountTitleById, deleteAccountTitleBatch, getAccountTitleTemplate, importAccountTitle, exportAccountTitle } from '@/api/accounting/financial/account-title'
import type { AccountTitle, AccountTitleQuery, AccountTitleCreate, AccountTitleUpdate } from '@/types/accounting/financial/account-title'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktAccountTitle')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.accountTitle._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<AccountTitle[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<AccountTitle | null>(null)
const selectedRows = ref<AccountTitle[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<AccountTitle>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'accountTitleId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'accountTitleId',
    key: 'accountTitleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'accountTitleId') ?? ''
  },
  {
    title: t('entity.accountTitle.titlecode'),
    dataIndex: 'titleCode',
    key: 'titleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleCode') ?? ''
  },
  {
    title: t('entity.accountTitle.titlename'),
    dataIndex: 'titleName',
    key: 'titleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleName') ?? ''
  },
  {
    title: t('entity.accountTitle.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.accountTitle.titletype'),
    dataIndex: 'titleType',
    key: 'titleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleType') ?? ''
  },
  {
    title: t('entity.accountTitle.balancedirection'),
    dataIndex: 'balanceDirection',
    key: 'balanceDirection',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'balanceDirection') ?? ''
  },
  {
    title: t('entity.accountTitle.titlelevel'),
    dataIndex: 'titleLevel',
    key: 'titleLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleLevel') ?? ''
  },
  {
    title: t('entity.accountTitle.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isLeaf') ?? ''
  },
  {
    title: t('entity.accountTitle.isauxiliary'),
    dataIndex: 'isAuxiliary',
    key: 'isAuxiliary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isAuxiliary') ?? ''
  },
  {
    title: t('entity.accountTitle.auxiliarytype'),
    dataIndex: 'auxiliaryType',
    key: 'auxiliaryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'auxiliaryType') ?? ''
  },
  {
    title: t('entity.accountTitle.isquantity'),
    dataIndex: 'isQuantity',
    key: 'isQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isQuantity') ?? ''
  },
  {
    title: t('entity.accountTitle.iscurrency'),
    dataIndex: 'isCurrency',
    key: 'isCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isCurrency') ?? ''
  },
  {
    title: t('entity.accountTitle.iscash'),
    dataIndex: 'isCash',
    key: 'isCash',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isCash') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:accounttitle:update',
        onClick: (record: AccountTitle) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:accounttitle:delete',
        onClick: (record: AccountTitle) => handleDeleteOne(record)
      }
    ]
  })
])

const getAccountTitleId = (record: any): string => record?.[entityIdName] ?? ''
const getAccountTitleField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: AccountTitle[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: AccountTitle, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAccountTitleId(selectedRow.value) === getAccountTitleId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AccountTitle[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: AccountTitle) => ({
  onClick: () => {
    const key = getAccountTitleId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAccountTitleId(item)))
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
    const params: AccountTitleQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAccountTitleList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AccountTitle] 加载数据失败', { error })
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
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.accountTitle._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: AccountTitle) {
  formTitle.value = t('common.page.button.edit') + t('entity.accountTitle._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.accountTitle._self') }))
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
      await updateAccountTitle(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.accountTitle._self') }))
    } else {
      await createAccountTitle(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.accountTitle._self') }))
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
  const res = await getAccountTitleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAccountTitle(file, sheetName)
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
    const exportQuery: AccountTitleQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAccountTitle(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.accountTitle._self') }))
  } catch (error: any) {
    logger.error('[AccountTitle] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.accountTitle._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: AccountTitle) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.accountTitle._self'), name: t('common.tip.this.target', { target: t('entity.accountTitle._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAccountTitleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.accountTitle._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.accountTitle._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.accountTitle._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAccountTitleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.accountTitle._self') }))
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
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
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
.accounting-financial-account-title {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
