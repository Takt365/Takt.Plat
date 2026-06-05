<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-conference-center-conference">
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
      create-permission="routine:conferencecenter:conference:create"
      update-permission="routine:conferencecenter:conference:update"
      delete-permission="routine:conferencecenter:conference:delete"
      import-permission="routine:conferencecenter:conference:import"
      export-permission="routine:conferencecenter:conference:export"
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
      :row-key="getConferenceId"
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
      <ConferenceForm
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
      <a-form-item :label="t('entity.conference.code')">
        <a-input
          v-model:value="advancedQueryForm.conferenceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.code') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.title') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.type')">
        <a-input
          v-model:value="advancedQueryForm.conferenceType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.type') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.status')">
        <a-input
          v-model:value="advancedQueryForm.conferenceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.status') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.location')">
        <a-input
          v-model:value="advancedQueryForm.location"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.location') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.meetinglink')">
        <a-input
          v-model:value="advancedQueryForm.meetingLink"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.meetinglink') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.agenda')">
        <a-input
          v-model:value="advancedQueryForm.agenda"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.agenda') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.conference.content')">
        <a-input
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.content') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.conference._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.conference._self"
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
      :id-column-key="'conferenceId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/conference-center/conference
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import ConferenceForm from './components/conference-form.vue'
import { getConferenceList, getConferenceById, createConference, updateConference, deleteConferenceById, deleteConferenceBatch, getConferenceTemplate, importConference, exportConference } from '@/api/routine/conference-center/conference'
import type { Conference, ConferenceQuery, ConferenceCreate, ConferenceUpdate } from '@/types/routine/conference-center/conference'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktConference')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.conference._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Conference[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Conference | null>(null)
const selectedRows = ref<Conference[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Conference>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  location: '',
  meetingLink: '',
  agenda: '',
  content: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'conferenceId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'conferenceId',
    key: 'conferenceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceId') ?? ''
  },
  {
    title: t('entity.conference.code'),
    dataIndex: 'conferenceCode',
    key: 'conferenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceCode') ?? ''
  },
  {
    title: t('entity.conference.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'title') ?? ''
  },
  {
    title: t('entity.conference.type'),
    dataIndex: 'conferenceType',
    key: 'conferenceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceType') ?? ''
  },
  {
    title: t('entity.conference.status'),
    dataIndex: 'conferenceStatus',
    key: 'conferenceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceStatus') ?? ''
  },
  {
    title: t('entity.conference.starttime'),
    dataIndex: 'startTime',
    key: 'startTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'startTime') ?? ''
  },
  {
    title: t('entity.conference.endtime'),
    dataIndex: 'endTime',
    key: 'endTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'endTime') ?? ''
  },
  {
    title: t('entity.conference.location'),
    dataIndex: 'location',
    key: 'location',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'location') ?? ''
  },
  {
    title: t('entity.conference.meetinglink'),
    dataIndex: 'meetingLink',
    key: 'meetingLink',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'meetingLink') ?? ''
  },
  {
    title: t('entity.conference.agenda'),
    dataIndex: 'agenda',
    key: 'agenda',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'agenda') ?? ''
  },
  {
    title: t('entity.conference.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'content') ?? ''
  },
  {
    title: t('entity.conference.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'summary') ?? ''
  },
  {
    title: t('entity.conference.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'tags') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:conferencecenter:conference:update',
        onClick: (record: Conference) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:conferencecenter:conference:delete',
        onClick: (record: Conference) => handleDeleteOne(record)
      }
    ]
  })
])

const getConferenceId = (record: any): string => record?.[entityIdName] ?? ''
const getConferenceField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Conference[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Conference, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getConferenceId(selectedRow.value) === getConferenceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Conference[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Conference) => ({
  onClick: () => {
    const key = getConferenceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getConferenceId(item)))
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
    const params: ConferenceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getConferenceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Conference] 加载数据失败', { error })
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
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  location: '',
  meetingLink: '',
  agenda: '',
  content: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.conference._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Conference) {
  formTitle.value = t('common.page.button.edit') + t('entity.conference._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.conference._self') }))
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
      await updateConference(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.conference._self') }))
    } else {
      await createConference(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.conference._self') }))
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
  const res = await getConferenceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importConference(file, sheetName)
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
    const exportQuery: ConferenceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportConference(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.conference._self') }))
  } catch (error: any) {
    logger.error('[Conference] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.conference._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Conference) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.conference._self'), name: t('common.tip.this.target', { target: t('entity.conference._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteConferenceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.conference._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.conference._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.conference._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteConferenceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.conference._self') }))
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
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  location: '',
  meetingLink: '',
  agenda: '',
  content: '',
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
.routine-conference-center-conference {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
