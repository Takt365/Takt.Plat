<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/knowledge-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：知识库变更日志实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-help-desk-knowledge-change-log">
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
      create-permission="routine:helpdesk:knowledgechangelog:create"
      update-permission="routine:helpdesk:knowledgechangelog:update"
      delete-permission="routine:helpdesk:knowledgechangelog:delete"

      export-permission="routine:helpdesk:knowledgechangelog:export"
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
      :row-key="getKnowledgeChangeLogId"
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
      <KnowledgeChangeLogForm
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
      <a-form-item :label="t('entity.knowledgeChangeLog.knowledgeid')">
        <a-input
          v-model:value="advancedQueryForm.knowledgeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.knowledgeid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.knowledgetitle')">
        <a-input
          v-model:value="advancedQueryForm.knowledgeTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.knowledgetitle') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.changetype')">
        <a-input
          v-model:value="advancedQueryForm.changeType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changetype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.changesummary')">
        <a-input
          v-model:value="advancedQueryForm.changeSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changesummary') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.changefields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changefields') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.changereason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changereason') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.knowledgeChangeLog.versionatchange')">
        <a-input
          v-model:value="advancedQueryForm.versionAtChange"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.versionatchange') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('common.page.entity.remark')">
        <a-input
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.remark') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'knowledgeChangeLogId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 知识库变更日志实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/help-desk/knowledge-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import KnowledgeChangeLogForm from './components/knowledge-change-log-form.vue'
import { getKnowledgeChangeLogList, getKnowledgeChangeLogById, createKnowledgeChangeLog, updateKnowledgeChangeLog, deleteKnowledgeChangeLogById, deleteKnowledgeChangeLogBatch, exportKnowledgeChangeLog } from '@/api/routine/help-desk/knowledge-change-log'
import type { KnowledgeChangeLog, KnowledgeChangeLogQuery, KnowledgeChangeLogCreate, KnowledgeChangeLogUpdate } from '@/types/routine/help-desk/knowledge-change-log'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktKnowledgeChangeLog')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.knowledgeChangeLog._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<KnowledgeChangeLog[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<KnowledgeChangeLog | null>(null)
const selectedRows = ref<KnowledgeChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<KnowledgeChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  knowledgeId: '',
  knowledgeTitle: '',
  changeType: undefined as number | undefined,
  changeSummary: '',
  changeFields: '',
  changeReason: '',
  versionAtChange: undefined as number | undefined,
  remark: '',
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'knowledgeChangeLogId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'knowledgeChangeLogId',
    key: 'knowledgeChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'knowledgeChangeLogId') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.knowledgeid'),
    dataIndex: 'knowledgeId',
    key: 'knowledgeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'knowledgeId') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.knowledgename'),
    dataIndex: 'knowledgeName',
    key: 'knowledgeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'knowledgeName') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.knowledgetitle'),
    dataIndex: 'knowledgeTitle',
    key: 'knowledgeTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'knowledgeTitle') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'changeType') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'changeSummary') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'changeFields') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'changeReason') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.versionatchange'),
    dataIndex: 'versionAtChange',
    key: 'versionAtChange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'versionAtChange') ?? ''
  },
  {
    title: t('entity.knowledgeChangeLog.knowledge'),
    dataIndex: 'knowledge',
    key: 'knowledge',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeChangeLogField(record, 'knowledge') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:helpdesk:knowledgechangelog:update',
        onClick: (record: KnowledgeChangeLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:helpdesk:knowledgechangelog:delete',
        onClick: (record: KnowledgeChangeLog) => handleDeleteOne(record)
      }
    ]
  })
])

const getKnowledgeChangeLogId = (record: any): string => record?.[entityIdName] ?? ''
const getKnowledgeChangeLogField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: KnowledgeChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: KnowledgeChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getKnowledgeChangeLogId(selectedRow.value) === getKnowledgeChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: KnowledgeChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: KnowledgeChangeLog) => ({
  onClick: () => {
    const key = getKnowledgeChangeLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getKnowledgeChangeLogId(item)))
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
    const params: KnowledgeChangeLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getKnowledgeChangeLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[KnowledgeChangeLog] 加载数据失败', { error })
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
  knowledgeId: '',
  knowledgeTitle: '',
  changeType: undefined as number | undefined,
  changeSummary: '',
  changeFields: '',
  changeReason: '',
  versionAtChange: undefined as number | undefined,
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.knowledgeChangeLog._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: KnowledgeChangeLog) {
  formTitle.value = t('common.page.button.edit') + t('entity.knowledgeChangeLog._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.knowledgeChangeLog._self') }))
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
      await updateKnowledgeChangeLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.knowledgeChangeLog._self') }))
    } else {
      await createKnowledgeChangeLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.knowledgeChangeLog._self') }))
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
    const exportQuery: KnowledgeChangeLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportKnowledgeChangeLog(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.knowledgeChangeLog._self') }))
  } catch (error: any) {
    logger.error('[KnowledgeChangeLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.knowledgeChangeLog._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: KnowledgeChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.knowledgeChangeLog._self'), name: t('common.tip.this.target', { target: t('entity.knowledgeChangeLog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteKnowledgeChangeLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.knowledgeChangeLog._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.knowledgeChangeLog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.knowledgeChangeLog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteKnowledgeChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.knowledgeChangeLog._self') }))
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
  knowledgeId: '',
  knowledgeTitle: '',
  changeType: undefined as number | undefined,
  changeSummary: '',
  changeFields: '',
  changeReason: '',
  versionAtChange: undefined as number | undefined,
  remark: '',
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
.routine-help-desk-knowledge-change-log {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
