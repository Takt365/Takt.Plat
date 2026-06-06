<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-issue-pcba-rework -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质问题应对明细 - PCBA不良改修应对管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-cost-quality-issue-pcba-rework">
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
      create-permission="logistics:quality:cost:qualityissuepcbarework:create"
      update-permission="logistics:quality:cost:qualityissuepcbarework:update"
      delete-permission="logistics:quality:cost:qualityissuepcbarework:delete"
      import-permission="logistics:quality:cost:qualityissuepcbarework:import"
      export-permission="logistics:quality:cost:qualityissuepcbarework:export"
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
      :row-key="getQualityIssuePcbaReworkId"
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
      <QualityIssuePcbaReworkForm
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
      <a-form-item :label="t('entity.qualityIssuePcbaRework.qualityissueid')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissueid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.qualityissuecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissuecode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.linenumber')">
        <a-input
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.linenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.pcbadefectparts')">
        <a-input
          v-model:value="advancedQueryForm.pcbaDefectParts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbadefectparts') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.pcbareworkcost')">
        <a-input
          v-model:value="advancedQueryForm.pcbaReworkCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareworkcost') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.pcbareworktimeminutes')">
        <a-input
          v-model:value="advancedQueryForm.pcbaReworkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareworktimeminutes') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes')">
        <a-input
          v-model:value="advancedQueryForm.pcbaReinspectionTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.qualityIssuePcbaRework.pcbatravelcost')">
        <a-input
          v-model:value="advancedQueryForm.pcbaTravelCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbatravelcost') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.qualityIssuePcbaRework._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityIssuePcbaRework._self"
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
      :id-column-key="'qualityIssuePcbaReworkId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 品质问题应对明细 - PCBA不良改修应对管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-issue-pcba-rework
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import QualityIssuePcbaReworkForm from './components/quality-issue-pcba-rework-form.vue'
import { getQualityIssuePcbaReworkList, getQualityIssuePcbaReworkById, createQualityIssuePcbaRework, updateQualityIssuePcbaRework, deleteQualityIssuePcbaReworkById, deleteQualityIssuePcbaReworkBatch, getQualityIssuePcbaReworkTemplate, importQualityIssuePcbaRework, exportQualityIssuePcbaRework } from '@/api/logistics/quality/cost/quality-issue-pcba-rework'
import type { QualityIssuePcbaRework, QualityIssuePcbaReworkQuery, QualityIssuePcbaReworkCreate, QualityIssuePcbaReworkUpdate } from '@/types/logistics/quality/cost/quality-issue-pcba-rework'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktQualityIssuePcbaRework')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityIssuePcbaRework._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<QualityIssuePcbaRework[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<QualityIssuePcbaRework | null>(null)
const selectedRows = ref<QualityIssuePcbaRework[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityIssuePcbaRework>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityIssueId: '',
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  pcbaDefectParts: '',
  pcbaReworkCost: undefined as number | undefined,
  pcbaReworkTimeMinutes: undefined as number | undefined,
  pcbaReinspectionTimeMinutes: undefined as number | undefined,
  pcbaTravelCost: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'qualityIssuePcbaReworkId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIssuePcbaReworkId',
    key: 'qualityIssuePcbaReworkId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'qualityIssuePcbaReworkId') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.qualityissueid'),
    dataIndex: 'qualityIssueId',
    key: 'qualityIssueId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'qualityIssueId') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.qualityissuename'),
    dataIndex: 'qualityIssueName',
    key: 'qualityIssueName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'qualityIssueName') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.qualityissuecode'),
    dataIndex: 'qualityIssueCode',
    key: 'qualityIssueCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'qualityIssueCode') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbadefectparts'),
    dataIndex: 'pcbaDefectParts',
    key: 'pcbaDefectParts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaDefectParts') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbareworkcost'),
    dataIndex: 'pcbaReworkCost',
    key: 'pcbaReworkCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaReworkCost') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbareworktimeminutes'),
    dataIndex: 'pcbaReworkTimeMinutes',
    key: 'pcbaReworkTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaReworkTimeMinutes') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes'),
    dataIndex: 'pcbaReinspectionTimeMinutes',
    key: 'pcbaReinspectionTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaReinspectionTimeMinutes') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbatravelcost'),
    dataIndex: 'pcbaTravelCost',
    key: 'pcbaTravelCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaTravelCost') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbawarehousecost'),
    dataIndex: 'pcbaWarehouseCost',
    key: 'pcbaWarehouseCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaWarehouseCost') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbaotherexpenses'),
    dataIndex: 'pcbaOtherExpenses',
    key: 'pcbaOtherExpenses',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaOtherExpenses') ?? ''
  },
  {
    title: t('entity.qualityIssuePcbaRework.pcbareworknote'),
    dataIndex: 'pcbaReworkNote',
    key: 'pcbaReworkNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIssuePcbaReworkField(record, 'pcbaReworkNote') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:qualityissuepcbarework:update',
        onClick: (record: QualityIssuePcbaRework) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:qualityissuepcbarework:delete',
        onClick: (record: QualityIssuePcbaRework) => handleDeleteOne(record)
      }
    ]
  })
])

const getQualityIssuePcbaReworkId = (record: any): string => record?.[entityIdName] ?? ''
const getQualityIssuePcbaReworkField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: QualityIssuePcbaRework[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIssuePcbaRework, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityIssuePcbaReworkId(selectedRow.value) === getQualityIssuePcbaReworkId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityIssuePcbaRework[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: QualityIssuePcbaRework) => ({
  onClick: () => {
    const key = getQualityIssuePcbaReworkId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQualityIssuePcbaReworkId(item)))
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
    const params: QualityIssuePcbaReworkQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQualityIssuePcbaReworkList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QualityIssuePcbaRework] 加载数据失败', { error })
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
  pcbaDefectParts: '',
  pcbaReworkCost: undefined as number | undefined,
  pcbaReworkTimeMinutes: undefined as number | undefined,
  pcbaReinspectionTimeMinutes: undefined as number | undefined,
  pcbaTravelCost: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.qualityIssuePcbaRework._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: QualityIssuePcbaRework) {
  formTitle.value = t('common.page.button.edit') + t('entity.qualityIssuePcbaRework._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.qualityIssuePcbaRework._self') }))
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
      await updateQualityIssuePcbaRework(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.qualityIssuePcbaRework._self') }))
    } else {
      await createQualityIssuePcbaRework(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.qualityIssuePcbaRework._self') }))
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
  const res = await getQualityIssuePcbaReworkTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIssuePcbaRework(file, sheetName)
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
    const exportQuery: QualityIssuePcbaReworkQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQualityIssuePcbaRework(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityIssuePcbaRework._self') }))
  } catch (error: any) {
    logger.error('[QualityIssuePcbaRework] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.qualityIssuePcbaRework._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: QualityIssuePcbaRework) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.qualityIssuePcbaRework._self'), name: t('common.tip.this.target', { target: t('entity.qualityIssuePcbaRework._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIssuePcbaReworkById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIssuePcbaRework._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.qualityIssuePcbaRework._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.qualityIssuePcbaRework._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQualityIssuePcbaReworkBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIssuePcbaRework._self') }))
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
  pcbaDefectParts: '',
  pcbaReworkCost: undefined as number | undefined,
  pcbaReworkTimeMinutes: undefined as number | undefined,
  pcbaReinspectionTimeMinutes: undefined as number | undefined,
  pcbaTravelCost: undefined as number | undefined,
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
.logistics-quality-cost-quality-issue-pcba-rework {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
