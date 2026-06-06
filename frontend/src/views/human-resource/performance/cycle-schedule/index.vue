<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/cycle-schedule -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：绩效考核周期日程安排管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-performance-cycle-schedule">
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
      create-permission="humanresource:performance:cycleschedule:create"
      update-permission="humanresource:performance:cycleschedule:update"
      delete-permission="humanresource:performance:cycleschedule:delete"
      import-permission="humanresource:performance:cycleschedule:import"
      export-permission="humanresource:performance:cycleschedule:export"
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
      :row-key="getCycleScheduleId"
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
      <CycleScheduleForm
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
      <a-form-item :label="t('entity.cycleSchedule.cyclecode')">
        <a-input
          v-model:value="advancedQueryForm.cycleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclecode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.cyclename')">
        <a-input
          v-model:value="advancedQueryForm.cycleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclename') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.cycletype')">
        <a-input
          v-model:value="advancedQueryForm.cycleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cycletype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.cycleyear')">
        <a-input
          v-model:value="advancedQueryForm.cycleYear"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cycleyear') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.cyclesequence')">
        <a-input
          v-model:value="advancedQueryForm.cycleSequence"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclesequence') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.applicabledepartment')">
        <a-input
          v-model:value="advancedQueryForm.applicableDepartment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.applicabledepartment') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.description')">
        <a-input
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.description') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.cycleSchedule.status')">
        <a-input
          v-model:value="advancedQueryForm.cycleScheduleStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.status') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.cycleSchedule._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.cycleSchedule._self"
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
      :id-column-key="'cycleScheduleId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 绩效考核周期日程安排管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/performance/cycle-schedule
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import CycleScheduleForm from './components/cycle-schedule-form.vue'
import { getCycleScheduleList, getCycleScheduleById, createCycleSchedule, updateCycleSchedule, deleteCycleScheduleById, deleteCycleScheduleBatch, getCycleScheduleTemplate, importCycleSchedule, exportCycleSchedule } from '@/api/human-resource/performance/cycle-schedule'
import type { CycleSchedule, CycleScheduleQuery, CycleScheduleCreate, CycleScheduleUpdate } from '@/types/human-resource/performance/cycle-schedule'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktCycleSchedule')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.cycleSchedule._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<CycleSchedule[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<CycleSchedule | null>(null)
const selectedRows = ref<CycleSchedule[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<CycleSchedule>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'cycleScheduleId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'cycleScheduleId',
    key: 'cycleScheduleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleScheduleId') ?? ''
  },
  {
    title: t('entity.cycleSchedule.cyclecode'),
    dataIndex: 'cycleCode',
    key: 'cycleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleCode') ?? ''
  },
  {
    title: t('entity.cycleSchedule.cyclename'),
    dataIndex: 'cycleName',
    key: 'cycleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleName') ?? ''
  },
  {
    title: t('entity.cycleSchedule.cycletype'),
    dataIndex: 'cycleType',
    key: 'cycleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleType') ?? ''
  },
  {
    title: t('entity.cycleSchedule.cycleyear'),
    dataIndex: 'cycleYear',
    key: 'cycleYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleYear') ?? ''
  },
  {
    title: t('entity.cycleSchedule.cyclesequence'),
    dataIndex: 'cycleSequence',
    key: 'cycleSequence',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'cycleSequence') ?? ''
  },
  {
    title: t('entity.cycleSchedule.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.goalsettingduedate'),
    dataIndex: 'goalSettingDueDate',
    key: 'goalSettingDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'goalSettingDueDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.selfevaluationduedate'),
    dataIndex: 'selfEvaluationDueDate',
    key: 'selfEvaluationDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'selfEvaluationDueDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.supervisorreviewduedate'),
    dataIndex: 'supervisorReviewDueDate',
    key: 'supervisorReviewDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'supervisorReviewDueDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.interviewduedate'),
    dataIndex: 'interviewDueDate',
    key: 'interviewDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'interviewDueDate') ?? ''
  },
  {
    title: t('entity.cycleSchedule.resultconfirmationduedate'),
    dataIndex: 'resultConfirmationDueDate',
    key: 'resultConfirmationDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCycleScheduleField(record, 'resultConfirmationDueDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:performance:cycleschedule:update',
        onClick: (record: CycleSchedule) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:performance:cycleschedule:delete',
        onClick: (record: CycleSchedule) => handleDeleteOne(record)
      }
    ]
  })
])

const getCycleScheduleId = (record: any): string => record?.[entityIdName] ?? ''
const getCycleScheduleField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: CycleSchedule[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CycleSchedule, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCycleScheduleId(selectedRow.value) === getCycleScheduleId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CycleSchedule[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: CycleSchedule) => ({
  onClick: () => {
    const key = getCycleScheduleId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCycleScheduleId(item)))
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
    const params: CycleScheduleQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCycleScheduleList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CycleSchedule] 加载数据失败', { error })
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
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.cycleSchedule._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: CycleSchedule) {
  formTitle.value = t('common.page.button.edit') + t('entity.cycleSchedule._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.cycleSchedule._self') }))
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
      await updateCycleSchedule(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.cycleSchedule._self') }))
    } else {
      await createCycleSchedule(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.cycleSchedule._self') }))
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
  const res = await getCycleScheduleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCycleSchedule(file, sheetName)
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
    const exportQuery: CycleScheduleQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCycleSchedule(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.cycleSchedule._self') }))
  } catch (error: any) {
    logger.error('[CycleSchedule] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.cycleSchedule._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: CycleSchedule) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.cycleSchedule._self'), name: t('common.tip.this.target', { target: t('entity.cycleSchedule._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCycleScheduleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.cycleSchedule._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.cycleSchedule._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.cycleSchedule._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCycleScheduleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.cycleSchedule._self') }))
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
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
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
.human-resource-performance-cycle-schedule {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
