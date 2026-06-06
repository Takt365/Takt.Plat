<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：PCBA日报实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-pcba-output">
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
      create-permission="logistics:manufacturing:output:pcbaoutput:create"
      update-permission="logistics:manufacturing:output:pcbaoutput:update"
      delete-permission="logistics:manufacturing:output:pcbaoutput:delete"
      import-permission="logistics:manufacturing:output:pcbaoutput:import"
      export-permission="logistics:manufacturing:output:pcbaoutput:export"
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
      :row-key="getPcbaOutputId"
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
      <PcbaOutputForm
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
      <a-form-item :label="t('entity.pcbaOutput.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.prodcategory')">
        <a-input
          v-model:value="advancedQueryForm.prodCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodcategory') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.prodline')">
        <a-input
          v-model:value="advancedQueryForm.prodLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodline') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.shiftno')">
        <a-input
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.shiftno') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodordercode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.modelcode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.modelcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.batchno') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.pcbaOutput.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.materialcode') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.pcbaOutput._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.pcbaOutput._self"
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
      :id-column-key="'pcbaOutputId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * PCBA日报实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import PcbaOutputForm from './components/pcba-output-form.vue'
import { getPcbaOutputList, getPcbaOutputById, createPcbaOutput, updatePcbaOutput, deletePcbaOutputById, deletePcbaOutputBatch, getPcbaOutputTemplate, importPcbaOutput, exportPcbaOutput } from '@/api/logistics/manufacturing/output/pcba-output'
import type { PcbaOutput, PcbaOutputQuery, PcbaOutputCreate, PcbaOutputUpdate } from '@/types/logistics/manufacturing/output/pcba-output'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktPcbaOutput')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.pcbaOutput._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<PcbaOutput[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<PcbaOutput | null>(null)
const selectedRows = ref<PcbaOutput[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PcbaOutput>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  prodCategory: '',
  prodLine: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'pcbaOutputId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'pcbaOutputId',
    key: 'pcbaOutputId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'pcbaOutputId') ?? ''
  },
  {
    title: t('entity.pcbaOutput.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodcategory'),
    dataIndex: 'prodCategory',
    key: 'prodCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodCategory') ?? ''
  },
  {
    title: t('entity.pcbaOutput.proddate'),
    dataIndex: 'prodDate',
    key: 'prodDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodDate') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodline'),
    dataIndex: 'prodLine',
    key: 'prodLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodLine') ?? ''
  },
  {
    title: t('entity.pcbaOutput.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'modelCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'batchNo') ?? ''
  },
  {
    title: t('entity.pcbaOutput.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodorderqty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodOrderQty') ?? ''
  },
  {
    title: t('entity.pcbaOutput.stdminutes'),
    dataIndex: 'stdMinutes',
    key: 'stdMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'stdMinutes') ?? ''
  },
  {
    title: t('entity.pcbaOutput.stdshorts'),
    dataIndex: 'stdShorts',
    key: 'stdShorts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'stdShorts') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:update',
        onClick: (record: PcbaOutput) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:delete',
        onClick: (record: PcbaOutput) => handleDeleteOne(record)
      }
    ]
  })
])

const getPcbaOutputId = (record: any): string => record?.[entityIdName] ?? ''
const getPcbaOutputField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: PcbaOutput[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PcbaOutput, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPcbaOutputId(selectedRow.value) === getPcbaOutputId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PcbaOutput[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: PcbaOutput) => ({
  onClick: () => {
    const key = getPcbaOutputId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPcbaOutputId(item)))
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
    const params: PcbaOutputQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPcbaOutputList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PcbaOutput] 加载数据失败', { error })
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
  plantCode: '',
  prodCategory: '',
  prodLine: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.pcbaOutput._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: PcbaOutput) {
  formTitle.value = t('common.page.button.edit') + t('entity.pcbaOutput._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.pcbaOutput._self') }))
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
      await updatePcbaOutput(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.pcbaOutput._self') }))
    } else {
      await createPcbaOutput(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.pcbaOutput._self') }))
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
  const res = await getPcbaOutputTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPcbaOutput(file, sheetName)
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
    const exportQuery: PcbaOutputQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPcbaOutput(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.pcbaOutput._self') }))
  } catch (error: any) {
    logger.error('[PcbaOutput] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.pcbaOutput._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: PcbaOutput) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.pcbaOutput._self'), name: t('common.tip.this.target', { target: t('entity.pcbaOutput._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePcbaOutputById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaOutput._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.pcbaOutput._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.pcbaOutput._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePcbaOutputBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaOutput._self') }))
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
  plantCode: '',
  prodCategory: '',
  prodLine: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
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
.logistics-manufacturing-output-pcba-output {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
