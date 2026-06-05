<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-document-center-document">
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
      create-permission="routine:documentcenter:document:create"
      update-permission="routine:documentcenter:document:update"
      delete-permission="routine:documentcenter:document:delete"
      import-permission="routine:documentcenter:document:import"
      export-permission="routine:documentcenter:document:export"
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
      :row-key="getDocumentId"
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
      <DocumentForm
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
      <a-form-item :label="t('entity.document.code')">
        <a-input
          v-model:value="advancedQueryForm.documentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.code') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.title') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.category')">
        <a-input
          v-model:value="advancedQueryForm.documentCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.category') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.status')">
        <a-input
          v-model:value="advancedQueryForm.documentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.status') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.confidentiallevel')">
        <a-input
          v-model:value="advancedQueryForm.confidentialLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.confidentiallevel') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.version')">
        <a-input
          v-model:value="advancedQueryForm.version"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.version') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.content')">
        <a-input
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.content') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.document.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.summary') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.document._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.document._self"
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
      :id-column-key="'documentId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/document-center/document
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import DocumentForm from './components/document-form.vue'
import { getDocumentList, getDocumentById, createDocument, updateDocument, deleteDocumentById, deleteDocumentBatch, getDocumentTemplate, importDocument, exportDocument } from '@/api/routine/document-center/document'
import type { Document, DocumentQuery, DocumentCreate, DocumentUpdate } from '@/types/routine/document-center/document'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktDocument')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.document._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Document[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Document | null>(null)
const selectedRows = ref<Document[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Document>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'documentId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'documentId',
    key: 'documentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentId') ?? ''
  },
  {
    title: t('entity.document.code'),
    dataIndex: 'documentCode',
    key: 'documentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentCode') ?? ''
  },
  {
    title: t('entity.document.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'title') ?? ''
  },
  {
    title: t('entity.document.category'),
    dataIndex: 'documentCategory',
    key: 'documentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentCategory') ?? ''
  },
  {
    title: t('entity.document.status'),
    dataIndex: 'documentStatus',
    key: 'documentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentStatus') ?? ''
  },
  {
    title: t('entity.document.confidentiallevel'),
    dataIndex: 'confidentialLevel',
    key: 'confidentialLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'confidentialLevel') ?? ''
  },
  {
    title: t('entity.document.version'),
    dataIndex: 'version',
    key: 'version',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'version') ?? ''
  },
  {
    title: t('entity.document.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'content') ?? ''
  },
  {
    title: t('entity.document.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'summary') ?? ''
  },
  {
    title: t('entity.document.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'tags') ?? ''
  },
  {
    title: t('entity.document.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileId') ?? ''
  },
  {
    title: t('entity.document.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileName') ?? ''
  },
  {
    title: t('entity.document.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'filePath') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:documentcenter:document:update',
        onClick: (record: Document) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:documentcenter:document:delete',
        onClick: (record: Document) => handleDeleteOne(record)
      }
    ]
  })
])

const getDocumentId = (record: any): string => record?.[entityIdName] ?? ''
const getDocumentField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Document[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Document, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getDocumentId(selectedRow.value) === getDocumentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Document[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Document) => ({
  onClick: () => {
    const key = getDocumentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getDocumentId(item)))
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
    const params: DocumentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getDocumentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Document] 加载数据失败', { error })
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
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.document._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Document) {
  formTitle.value = t('common.page.button.edit') + t('entity.document._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.document._self') }))
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
      await updateDocument(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.document._self') }))
    } else {
      await createDocument(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.document._self') }))
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
  const res = await getDocumentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importDocument(file, sheetName)
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
    const exportQuery: DocumentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportDocument(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.document._self') }))
  } catch (error: any) {
    logger.error('[Document] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.document._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Document) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.document._self'), name: t('common.tip.this.target', { target: t('entity.document._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDocumentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.document._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.document._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.document._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteDocumentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.document._self') }))
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
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
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
.routine-document-center-document {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
