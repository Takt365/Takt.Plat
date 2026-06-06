<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/assessment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工绩效考核评估管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-performance-assessment">
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
      create-permission="humanresource:performance:assessment:create"
      update-permission="humanresource:performance:assessment:update"
      delete-permission="humanresource:performance:assessment:delete"
      import-permission="humanresource:performance:assessment:import"
      export-permission="humanresource:performance:assessment:export"
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
      :row-key="getAssessmentId"
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
      <AssessmentForm
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
      <a-form-item :label="t('entity.assessment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeeid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeename') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.period')">
        <a-input
          v-model:value="advancedQueryForm.assessmentPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.period') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.schememetricid')">
        <a-input
          v-model:value="advancedQueryForm.schemeMetricId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.schememetricid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.selfscore')">
        <a-input
          v-model:value="advancedQueryForm.selfScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.selfscore') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.selfevaluationnotes')">
        <a-input
          v-model:value="advancedQueryForm.selfEvaluationNotes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.selfevaluationnotes') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.supervisorscore')">
        <a-input
          v-model:value="advancedQueryForm.supervisorScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorscore') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assessment.supervisorcomments')">
        <a-input
          v-model:value="advancedQueryForm.supervisorComments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorcomments') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.assessment._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assessment._self"
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
      :id-column-key="'assessmentId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 员工绩效考核评估管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/performance/assessment
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import AssessmentForm from './components/assessment-form.vue'
import { getAssessmentList, getAssessmentById, createAssessment, updateAssessment, deleteAssessmentById, deleteAssessmentBatch, getAssessmentTemplate, importAssessment, exportAssessment } from '@/api/human-resource/performance/assessment'
import type { Assessment, AssessmentQuery, AssessmentCreate, AssessmentUpdate } from '@/types/human-resource/performance/assessment'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktAssessment')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assessment._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Assessment[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Assessment | null>(null)
const selectedRows = ref<Assessment[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Assessment>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'assessmentId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assessmentId',
    key: 'assessmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentId') ?? ''
  },
  {
    title: t('entity.assessment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.assessment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.assessment.period'),
    dataIndex: 'assessmentPeriod',
    key: 'assessmentPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentPeriod') ?? ''
  },
  {
    title: t('entity.assessment.date'),
    dataIndex: 'assessmentDate',
    key: 'assessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentDate') ?? ''
  },
  {
    title: t('entity.assessment.schememetricid'),
    dataIndex: 'schemeMetricId',
    key: 'schemeMetricId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'schemeMetricId') ?? ''
  },
  {
    title: t('entity.assessment.schememetricname'),
    dataIndex: 'schemeMetricName',
    key: 'schemeMetricName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'schemeMetricName') ?? ''
  },
  {
    title: t('entity.assessment.selfscore'),
    dataIndex: 'selfScore',
    key: 'selfScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'selfScore') ?? ''
  },
  {
    title: t('entity.assessment.selfevaluationnotes'),
    dataIndex: 'selfEvaluationNotes',
    key: 'selfEvaluationNotes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'selfEvaluationNotes') ?? ''
  },
  {
    title: t('entity.assessment.supervisorscore'),
    dataIndex: 'supervisorScore',
    key: 'supervisorScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'supervisorScore') ?? ''
  },
  {
    title: t('entity.assessment.supervisorcomments'),
    dataIndex: 'supervisorComments',
    key: 'supervisorComments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'supervisorComments') ?? ''
  },
  {
    title: t('entity.assessment.finalscore'),
    dataIndex: 'finalScore',
    key: 'finalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'finalScore') ?? ''
  },
  {
    title: t('entity.assessment.performancegrade'),
    dataIndex: 'performanceGrade',
    key: 'performanceGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'performanceGrade') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:performance:assessment:update',
        onClick: (record: Assessment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:performance:assessment:delete',
        onClick: (record: Assessment) => handleDeleteOne(record)
      }
    ]
  })
])

const getAssessmentId = (record: any): string => record?.[entityIdName] ?? ''
const getAssessmentField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Assessment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Assessment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssessmentId(selectedRow.value) === getAssessmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Assessment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Assessment) => ({
  onClick: () => {
    const key = getAssessmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssessmentId(item)))
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
    const params: AssessmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAssessmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Assessment] 加载数据失败', { error })
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
  employeeName: '',
  assessmentPeriod: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.assessment._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Assessment) {
  formTitle.value = t('common.page.button.edit') + t('entity.assessment._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.assessment._self') }))
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
      await updateAssessment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.assessment._self') }))
    } else {
      await createAssessment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.assessment._self') }))
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
  const res = await getAssessmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssessment(file, sheetName)
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
    const exportQuery: AssessmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAssessment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.assessment._self') }))
  } catch (error: any) {
    logger.error('[Assessment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.assessment._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Assessment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.assessment._self'), name: t('common.tip.this.target', { target: t('entity.assessment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssessmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.assessment._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.assessment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.assessment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssessmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assessment._self') }))
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
  employeeName: '',
  assessmentPeriod: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
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
.human-resource-performance-assessment {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
