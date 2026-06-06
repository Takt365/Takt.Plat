<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/knowledge -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台知识库实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-help-desk-knowledge">
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
      create-permission="routine:helpdesk:knowledge:create"
      update-permission="routine:helpdesk:knowledge:update"
      delete-permission="routine:helpdesk:knowledge:delete"
      import-permission="routine:helpdesk:knowledge:import"
      export-permission="routine:helpdesk:knowledge:export"
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
      :row-key="getKnowledgeId"
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
      <KnowledgeForm
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
      <a-form-item :label="t('entity.knowledge.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.title') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.content')">
        <a-input
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.content') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.summary') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.categorycode')">
        <a-input
          v-model:value="advancedQueryForm.categoryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.categorycode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.tags') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.status')">
        <a-input
          v-model:value="advancedQueryForm.knowledgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.status') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.sortorder')">
        <a-input
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.sortorder') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledge.viewcount')">
        <a-input
          v-model:value="advancedQueryForm.viewCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.viewcount') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.knowledge._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.knowledge._self"
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
      :id-column-key="'knowledgeId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 服务台知识库实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/help-desk/knowledge
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import KnowledgeForm from './components/knowledge-form.vue'
import { getKnowledgeList, getKnowledgeById, createKnowledge, updateKnowledge, deleteKnowledgeById, deleteKnowledgeBatch, getKnowledgeTemplate, importKnowledge, exportKnowledge } from '@/api/routine/help-desk/knowledge'
import type { Knowledge, KnowledgeQuery, KnowledgeCreate, KnowledgeUpdate } from '@/types/routine/help-desk/knowledge'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktKnowledge')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.knowledge._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Knowledge[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Knowledge | null>(null)
const selectedRows = ref<Knowledge[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Knowledge>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  title: '',
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'knowledgeId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'knowledgeId',
    key: 'knowledgeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'knowledgeId') ?? ''
  },
  {
    title: t('entity.knowledge.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'title') ?? ''
  },
  {
    title: t('entity.knowledge.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'content') ?? ''
  },
  {
    title: t('entity.knowledge.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'summary') ?? ''
  },
  {
    title: t('entity.knowledge.categorycode'),
    dataIndex: 'categoryCode',
    key: 'categoryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'categoryCode') ?? ''
  },
  {
    title: t('entity.knowledge.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'tags') ?? ''
  },
  {
    title: t('entity.knowledge.status'),
    dataIndex: 'knowledgeStatus',
    key: 'knowledgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'knowledgeStatus') ?? ''
  },
  {
    title: t('entity.knowledge.viewcount'),
    dataIndex: 'viewCount',
    key: 'viewCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'viewCount') ?? ''
  },
  {
    title: t('entity.knowledge.helpfulcount'),
    dataIndex: 'helpfulCount',
    key: 'helpfulCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'helpfulCount') ?? ''
  },
  {
    title: t('entity.knowledge.unhelpfulcount'),
    dataIndex: 'unhelpfulCount',
    key: 'unhelpfulCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'unhelpfulCount') ?? ''
  },
  {
    title: t('entity.knowledge.ispublished'),
    dataIndex: 'isPublished',
    key: 'isPublished',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'isPublished') ?? ''
  },
  {
    title: t('entity.knowledge.version'),
    dataIndex: 'version',
    key: 'version',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'version') ?? ''
  },
  {
    title: t('entity.knowledge.publishedat'),
    dataIndex: 'publishedAt',
    key: 'publishedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'publishedAt') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:helpdesk:knowledge:update',
        onClick: (record: Knowledge) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:helpdesk:knowledge:delete',
        onClick: (record: Knowledge) => handleDeleteOne(record)
      }
    ]
  })
])

const getKnowledgeId = (record: any): string => record?.[entityIdName] ?? ''
const getKnowledgeField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Knowledge[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Knowledge, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getKnowledgeId(selectedRow.value) === getKnowledgeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Knowledge[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Knowledge) => ({
  onClick: () => {
    const key = getKnowledgeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getKnowledgeId(item)))
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
    const params: KnowledgeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getKnowledgeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Knowledge] 加载数据失败', { error })
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
  title: '',
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.knowledge._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Knowledge) {
  formTitle.value = t('common.page.button.edit') + t('entity.knowledge._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.knowledge._self') }))
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
      await updateKnowledge(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.knowledge._self') }))
    } else {
      await createKnowledge(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.knowledge._self') }))
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
  const res = await getKnowledgeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importKnowledge(file, sheetName)
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
    const exportQuery: KnowledgeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportKnowledge(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.knowledge._self') }))
  } catch (error: any) {
    logger.error('[Knowledge] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.knowledge._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Knowledge) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.knowledge._self'), name: t('common.tip.this.target', { target: t('entity.knowledge._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteKnowledgeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.knowledge._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.knowledge._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.knowledge._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteKnowledgeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.knowledge._self') }))
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
  title: '',
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
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
.routine-help-desk-knowledge {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
