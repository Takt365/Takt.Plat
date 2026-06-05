<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-issue-assy-rework -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质问题应对明细 - 组装不良改修应对管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-cost-quality-issue-assy-rework">
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
      create-permission="logistics:quality:cost:qualityissueassyrework:create"
      update-permission="logistics:quality:cost:qualityissueassyrework:update"
      delete-permission="logistics:quality:cost:qualityissueassyrework:delete"
      import-permission="logistics:quality:cost:qualityissueassyrework:import"
      export-permission="logistics:quality:cost:qualityissueassyrework:export"
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
      :row-key="getQualityIssueAssyReworkId"
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
      <QualityIssueAssyReworkForm
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
      <a-form-item :label="t('entity.qualityIssueAssyRework.qualityissueid')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissueid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.qualityissuecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissuecode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.linenumber')">
        <a-input
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.linenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.assydefectparts')">
        <a-input
          v-model:value="advancedQueryForm.assyDefectParts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assydefectparts') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.assyreworkcost')">
        <a-input
          v-model:value="advancedQueryForm.assyReworkCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreworkcost') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.assyreworktimeminutes')">
        <a-input
          v-model:value="advancedQueryForm.assyReworkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreworktimeminutes') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes')">
        <a-input
          v-model:value="advancedQueryForm.assyReinspectionTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssueAssyRework.assytravelcost')">
        <a-input
          v-model:value="advancedQueryForm.assyTravelCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assytravelcost') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.qualityIssueAssyRework._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityIssueAssyRework._self"
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
      :id-column-key="'qualityIssueAssyReworkId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 品质问题应对明细 - 组装不良改修应对管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-issue-assy-rework
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import QualityIssueAssyReworkForm from './components/quality-issue-assy-rework-form.vue'
import { getQualityIssueAssyReworkList, getQualityIssueAssyReworkById, createQualityIssueAssyRework, updateQualityIssueAssyRework, deleteQualityIssueAssyReworkById, deleteQualityIssueAssyReworkBatch, getQualityIssueAssyReworkTemplate, importQualityIssueAssyRework, exportQualityIssueAssyRework } from '@/api/logistics/quality/cost/quality-issue-assy-rework'
import type { QualityIssueAssyRework, QualityIssueAssyReworkQuery, QualityIssueAssyReworkCreate, QualityIssueAssyReworkUpdate } from '@/types/logistics/quality/cost/quality-issue-assy-rework'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktQualityIssueAssyRework')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityIssueAssyRework._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<QualityIssueAssyRework[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<QualityIssueAssyRework | null>(null)
const selectedRows = ref<QualityIssueAssyRework[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityIssueAssyRework>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityIssueId: '',
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  assyDefectParts: '',
  assyReworkCost: undefined as number | undefined,
  assyReworkTimeMinutes: undefined as number | undefined,
  assyReinspectionTimeMinutes: undefined as number | undefined,
  assyTravelCost: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'qualityIssueAssyReworkId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIssueAssyReworkId',
    key: 'qualityIssueAssyReworkId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'qualityIssueAssyReworkId') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.qualityissueid'),
    dataIndex: 'qualityIssueId',
    key: 'qualityIssueId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'qualityIssueId') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.qualityissuename'),
    dataIndex: 'qualityIssueName',
    key: 'qualityIssueName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'qualityIssueName') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.qualityissuecode'),
    dataIndex: 'qualityIssueCode',
    key: 'qualityIssueCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'qualityIssueCode') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assydefectparts'),
    dataIndex: 'assyDefectParts',
    key: 'assyDefectParts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyDefectParts') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assyreworkcost'),
    dataIndex: 'assyReworkCost',
    key: 'assyReworkCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyReworkCost') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assyreworktimeminutes'),
    dataIndex: 'assyReworkTimeMinutes',
    key: 'assyReworkTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyReworkTimeMinutes') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes'),
    dataIndex: 'assyReinspectionTimeMinutes',
    key: 'assyReinspectionTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyReinspectionTimeMinutes') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assytravelcost'),
    dataIndex: 'assyTravelCost',
    key: 'assyTravelCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyTravelCost') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assywarehousecost'),
    dataIndex: 'assyWarehouseCost',
    key: 'assyWarehouseCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyWarehouseCost') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assyotherexpenses'),
    dataIndex: 'assyOtherExpenses',
    key: 'assyOtherExpenses',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyOtherExpenses') ?? ''
  },
  {
    title: t('entity.qualityIssueAssyRework.assyreworknote'),
    dataIndex: 'assyReworkNote',
    key: 'assyReworkNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssueAssyReworkField(record, 'assyReworkNote') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:qualityissueassyrework:update',
        onClick: (record: QualityIssueAssyRework) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:qualityissueassyrework:delete',
        onClick: (record: QualityIssueAssyRework) => handleDeleteOne(record)
      }
    ]
  })
])

const getQualityIssueAssyReworkId = (record: any): string => record?.[entityIdName] ?? ''
const getQualityIssueAssyReworkField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: QualityIssueAssyRework[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIssueAssyRework, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityIssueAssyReworkId(selectedRow.value) === getQualityIssueAssyReworkId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityIssueAssyRework[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: QualityIssueAssyRework) => ({
  onClick: () => {
    const key = getQualityIssueAssyReworkId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQualityIssueAssyReworkId(item)))
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
    const params: QualityIssueAssyReworkQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQualityIssueAssyReworkList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QualityIssueAssyRework] 加载数据失败', { error })
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
  qualityIssueId: '',
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  assyDefectParts: '',
  assyReworkCost: undefined as number | undefined,
  assyReworkTimeMinutes: undefined as number | undefined,
  assyReinspectionTimeMinutes: undefined as number | undefined,
  assyTravelCost: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.qualityIssueAssyRework._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: QualityIssueAssyRework) {
  formTitle.value = t('common.page.button.edit') + t('entity.qualityIssueAssyRework._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.qualityIssueAssyRework._self') }))
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
      await updateQualityIssueAssyRework(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.qualityIssueAssyRework._self') }))
    } else {
      await createQualityIssueAssyRework(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.qualityIssueAssyRework._self') }))
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
  const res = await getQualityIssueAssyReworkTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIssueAssyRework(file, sheetName)
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
    const exportQuery: QualityIssueAssyReworkQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQualityIssueAssyRework(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityIssueAssyRework._self') }))
  } catch (error: any) {
    logger.error('[QualityIssueAssyRework] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.qualityIssueAssyRework._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: QualityIssueAssyRework) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.qualityIssueAssyRework._self'), name: t('common.tip.this.target', { target: t('entity.qualityIssueAssyRework._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIssueAssyReworkById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIssueAssyRework._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.qualityIssueAssyRework._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.qualityIssueAssyRework._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQualityIssueAssyReworkBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIssueAssyRework._self') }))
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
  qualityIssueId: '',
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  assyDefectParts: '',
  assyReworkCost: undefined as number | undefined,
  assyReworkTimeMinutes: undefined as number | undefined,
  assyReinspectionTimeMinutes: undefined as number | undefined,
  assyTravelCost: undefined as number | undefined,
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
.logistics-quality-cost-quality-issue-assy-rework {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
