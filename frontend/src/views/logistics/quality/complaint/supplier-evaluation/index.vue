<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：供应商评价考核主表实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-complaint-supplier-evaluation">
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
      create-permission="logistics:quality:complaint:supplierevaluation:create"
      update-permission="logistics:quality:complaint:supplierevaluation:update"
      delete-permission="logistics:quality:complaint:supplierevaluation:delete"
      import-permission="logistics:quality:complaint:supplierevaluation:import"
      export-permission="logistics:quality:complaint:supplierevaluation:export"
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
      :row-key="getSupplierEvaluationId"
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
      <SupplierEvaluationForm
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
      <a-form-item :label="t('entity.supplierEvaluation.code')">
        <a-input
          v-model:value="advancedQueryForm.supplierEvaluationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.supplierid')">
        <a-input
          v-model:value="advancedQueryForm.supplierId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.evaluationperiod')">
        <a-input
          v-model:value="advancedQueryForm.evaluationPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationperiod') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.evaluationtype')">
        <a-input
          v-model:value="advancedQueryForm.evaluationType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationtype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.evaluatorby')">
        <a-input
          v-model:value="advancedQueryForm.evaluatorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluatorby') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.supplierEvaluation.evaluationdept')">
        <a-input
          v-model:value="advancedQueryForm.evaluationDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationdept') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.supplierEvaluation._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.supplierEvaluation._self"
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
      :id-column-key="'supplierEvaluationId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 供应商评价考核主表实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import SupplierEvaluationForm from './components/supplier-evaluation-form.vue'
import { getSupplierEvaluationList, getSupplierEvaluationById, createSupplierEvaluation, updateSupplierEvaluation, deleteSupplierEvaluationById, deleteSupplierEvaluationBatch, getSupplierEvaluationTemplate, importSupplierEvaluation, exportSupplierEvaluation } from '@/api/logistics/quality/complaint/supplier-evaluation'
import type { SupplierEvaluation, SupplierEvaluationQuery, SupplierEvaluationCreate, SupplierEvaluationUpdate } from '@/types/logistics/quality/complaint/supplier-evaluation'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktSupplierEvaluation')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.supplierEvaluation._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<SupplierEvaluation[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<SupplierEvaluation | null>(null)
const selectedRows = ref<SupplierEvaluation[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SupplierEvaluation>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'supplierEvaluationId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'supplierEvaluationId',
    key: 'supplierEvaluationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierEvaluationId') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.code'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierEvaluationCode') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.supplierid'),
    dataIndex: 'supplierId',
    key: 'supplierId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierId') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationdate'),
    dataIndex: 'evaluationDate',
    key: 'evaluationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDate') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationperiod'),
    dataIndex: 'evaluationPeriod',
    key: 'evaluationPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationPeriod') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationtype'),
    dataIndex: 'evaluationType',
    key: 'evaluationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationType') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluatorby'),
    dataIndex: 'evaluatorBy',
    key: 'evaluatorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluatorBy') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationdept'),
    dataIndex: 'evaluationDept',
    key: 'evaluationDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDept') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.overallrating'),
    dataIndex: 'overallRating',
    key: 'overallRating',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'overallRating') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.totalscore'),
    dataIndex: 'totalScore',
    key: 'totalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'totalScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.qualityscore'),
    dataIndex: 'qualityScore',
    key: 'qualityScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'qualityScore') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:supplierevaluation:update',
        onClick: (record: SupplierEvaluation) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:supplierevaluation:delete',
        onClick: (record: SupplierEvaluation) => handleDeleteOne(record)
      }
    ]
  })
])

const getSupplierEvaluationId = (record: any): string => record?.[entityIdName] ?? ''
const getSupplierEvaluationField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: SupplierEvaluation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SupplierEvaluation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSupplierEvaluationId(selectedRow.value) === getSupplierEvaluationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SupplierEvaluation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: SupplierEvaluation) => ({
  onClick: () => {
    const key = getSupplierEvaluationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSupplierEvaluationId(item)))
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
    const params: SupplierEvaluationQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSupplierEvaluationList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SupplierEvaluation] 加载数据失败', { error })
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
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.supplierEvaluation._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: SupplierEvaluation) {
  formTitle.value = t('common.page.button.edit') + t('entity.supplierEvaluation._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.supplierEvaluation._self') }))
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
      await updateSupplierEvaluation(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.supplierEvaluation._self') }))
    } else {
      await createSupplierEvaluation(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.supplierEvaluation._self') }))
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
  const res = await getSupplierEvaluationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSupplierEvaluation(file, sheetName)
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
    const exportQuery: SupplierEvaluationQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSupplierEvaluation(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.supplierEvaluation._self') }))
  } catch (error: any) {
    logger.error('[SupplierEvaluation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.supplierEvaluation._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: SupplierEvaluation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.supplierEvaluation._self'), name: t('common.tip.this.target', { target: t('entity.supplierEvaluation._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierEvaluationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.supplierEvaluation._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.supplierEvaluation._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.supplierEvaluation._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSupplierEvaluationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.supplierEvaluation._self') }))
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
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
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
.logistics-quality-complaint-supplier-evaluation {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
