<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/training-development/career-development -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工职业发展规划与技能评估管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-training-development-career-development">
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
      create-permission="humanresource:trainingdevelopment:careerdevelopment:create"
      update-permission="humanresource:trainingdevelopment:careerdevelopment:update"
      delete-permission="humanresource:trainingdevelopment:careerdevelopment:delete"
      import-permission="humanresource:trainingdevelopment:careerdevelopment:import"
      export-permission="humanresource:trainingdevelopment:careerdevelopment:export"
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
      :row-key="getCareerDevelopmentId"
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
      <CareerDevelopmentForm
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
      <a-form-item :label="t('entity.careerDevelopment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeeid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeename') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.skillcategory')">
        <a-input
          v-model:value="advancedQueryForm.skillCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillcategory') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.skillname')">
        <a-input
          v-model:value="advancedQueryForm.skillName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.assessmentmethod')">
        <a-input
          v-model:value="advancedQueryForm.assessmentMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentmethod') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.assessmentscore')">
        <a-input
          v-model:value="advancedQueryForm.assessmentScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentscore') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.skilllevel')">
        <a-input
          v-model:value="advancedQueryForm.skillLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skilllevel') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.careerDevelopment.targetposition')">
        <a-input
          v-model:value="advancedQueryForm.targetPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.targetposition') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.careerDevelopment._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.careerDevelopment._self"
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
      :id-column-key="'careerDevelopmentId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 员工职业发展规划与技能评估管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/training-development/career-development
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import CareerDevelopmentForm from './components/career-development-form.vue'
import { getCareerDevelopmentList, getCareerDevelopmentById, createCareerDevelopment, updateCareerDevelopment, deleteCareerDevelopmentById, deleteCareerDevelopmentBatch, getCareerDevelopmentTemplate, importCareerDevelopment, exportCareerDevelopment } from '@/api/human-resource/training-development/career-development'
import type { CareerDevelopment, CareerDevelopmentQuery, CareerDevelopmentCreate, CareerDevelopmentUpdate } from '@/types/human-resource/training-development/career-development'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktCareerDevelopment')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.careerDevelopment._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<CareerDevelopment[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<CareerDevelopment | null>(null)
const selectedRows = ref<CareerDevelopment[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<CareerDevelopment>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  employeeId: '',
  employeeName: '',
  skillCategory: '',
  skillName: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'careerDevelopmentId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'careerDevelopmentId',
    key: 'careerDevelopmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'careerDevelopmentId') ?? ''
  },
  {
    title: t('entity.careerDevelopment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.careerDevelopment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skillcategory'),
    dataIndex: 'skillCategory',
    key: 'skillCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillCategory') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skillname'),
    dataIndex: 'skillName',
    key: 'skillName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillName') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentdate'),
    dataIndex: 'assessmentDate',
    key: 'assessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentDate') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentmethod'),
    dataIndex: 'assessmentMethod',
    key: 'assessmentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentMethod') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentscore'),
    dataIndex: 'assessmentScore',
    key: 'assessmentScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentScore') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skilllevel'),
    dataIndex: 'skillLevel',
    key: 'skillLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillLevel') ?? ''
  },
  {
    title: t('entity.careerDevelopment.targetposition'),
    dataIndex: 'targetPosition',
    key: 'targetPosition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'targetPosition') ?? ''
  },
  {
    title: t('entity.careerDevelopment.developmentplan'),
    dataIndex: 'developmentPlan',
    key: 'developmentPlan',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'developmentPlan') ?? ''
  },
  {
    title: t('entity.careerDevelopment.improvementsuggestions'),
    dataIndex: 'improvementSuggestions',
    key: 'improvementSuggestions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'improvementSuggestions') ?? ''
  },
  {
    title: t('entity.careerDevelopment.nextassessmentdate'),
    dataIndex: 'nextAssessmentDate',
    key: 'nextAssessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'nextAssessmentDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:trainingdevelopment:careerdevelopment:update',
        onClick: (record: CareerDevelopment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:trainingdevelopment:careerdevelopment:delete',
        onClick: (record: CareerDevelopment) => handleDeleteOne(record)
      }
    ]
  })
])

const getCareerDevelopmentId = (record: any): string => record?.[entityIdName] ?? ''
const getCareerDevelopmentField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: CareerDevelopment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CareerDevelopment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCareerDevelopmentId(selectedRow.value) === getCareerDevelopmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CareerDevelopment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: CareerDevelopment) => ({
  onClick: () => {
    const key = getCareerDevelopmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCareerDevelopmentId(item)))
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
    const params: CareerDevelopmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCareerDevelopmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CareerDevelopment] 加载数据失败', { error })
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
  skillCategory: '',
  skillName: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.careerDevelopment._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: CareerDevelopment) {
  formTitle.value = t('common.page.button.edit') + t('entity.careerDevelopment._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.careerDevelopment._self') }))
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
      await updateCareerDevelopment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.careerDevelopment._self') }))
    } else {
      await createCareerDevelopment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.careerDevelopment._self') }))
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
  const res = await getCareerDevelopmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCareerDevelopment(file, sheetName)
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
    const exportQuery: CareerDevelopmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCareerDevelopment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.careerDevelopment._self') }))
  } catch (error: any) {
    logger.error('[CareerDevelopment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.careerDevelopment._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: CareerDevelopment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.careerDevelopment._self'), name: t('common.tip.this.target', { target: t('entity.careerDevelopment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCareerDevelopmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.careerDevelopment._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.careerDevelopment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.careerDevelopment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCareerDevelopmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.careerDevelopment._self') }))
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
  skillCategory: '',
  skillName: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
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
.human-resource-training-development-career-development {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
