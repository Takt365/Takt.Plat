<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/changeover -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：切换记录实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-changeover">
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
      create-permission="logistics:manufacturing:output:changeover:create"
      update-permission="logistics:manufacturing:output:changeover:update"
      delete-permission="logistics:manufacturing:output:changeover:delete"
      import-permission="logistics:manufacturing:output:changeover:import"
      export-permission="logistics:manufacturing:output:changeover:export"
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
      :row-key="getChangeoverId"
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
      <ChangeoverForm
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
      <a-form-item :label="t('entity.changeover.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.productioncategory')">
        <a-input
          v-model:value="advancedQueryForm.productionCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.productioncategory') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.productionline')">
        <a-input
          v-model:value="advancedQueryForm.productionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.productionline') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.readsoptime')">
        <a-input
          v-model:value="advancedQueryForm.readSopTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.readsoptime') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.personcount')">
        <a-input
          v-model:value="advancedQueryForm.personCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.personcount') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.totalsoptime')">
        <a-input
          v-model:value="advancedQueryForm.totalSopTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.totalsoptime') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.count')">
        <a-input
          v-model:value="advancedQueryForm.changeoverCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.count') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.changeover.time')">
        <a-input
          v-model:value="advancedQueryForm.changeoverTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.time') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.changeover._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.changeover._self"
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
      :id-column-key="'changeoverId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 切换记录实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/changeover
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import ChangeoverForm from './components/changeover-form.vue'
import { getChangeoverList, getChangeoverById, createChangeover, updateChangeover, deleteChangeoverById, deleteChangeoverBatch, getChangeoverTemplate, importChangeover, exportChangeover } from '@/api/logistics/manufacturing/output/changeover'
import type { Changeover, ChangeoverQuery, ChangeoverCreate, ChangeoverUpdate } from '@/types/logistics/manufacturing/output/changeover'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktChangeover')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.changeover._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Changeover[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Changeover | null>(null)
const selectedRows = ref<Changeover[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Changeover>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  productionCategory: '',
  productionLine: '',
  readSopTime: undefined as number | undefined,
  personCount: undefined as number | undefined,
  totalSopTime: undefined as number | undefined,
  changeoverCount: undefined as number | undefined,
  changeoverTime: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'changeoverId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'changeoverId',
    key: 'changeoverId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'changeoverId') ?? ''
  },
  {
    title: t('entity.changeover.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.changeover.productioncategory'),
    dataIndex: 'productionCategory',
    key: 'productionCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'productionCategory') ?? ''
  },
  {
    title: t('entity.changeover.productiondate'),
    dataIndex: 'productionDate',
    key: 'productionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'productionDate') ?? ''
  },
  {
    title: t('entity.changeover.productionline'),
    dataIndex: 'productionLine',
    key: 'productionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'productionLine') ?? ''
  },
  {
    title: t('entity.changeover.readsoptime'),
    dataIndex: 'readSopTime',
    key: 'readSopTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'readSopTime') ?? ''
  },
  {
    title: t('entity.changeover.personcount'),
    dataIndex: 'personCount',
    key: 'personCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'personCount') ?? ''
  },
  {
    title: t('entity.changeover.totalsoptime'),
    dataIndex: 'totalSopTime',
    key: 'totalSopTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'totalSopTime') ?? ''
  },
  {
    title: t('entity.changeover.count'),
    dataIndex: 'changeoverCount',
    key: 'changeoverCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'changeoverCount') ?? ''
  },
  {
    title: t('entity.changeover.time'),
    dataIndex: 'changeoverTime',
    key: 'changeoverTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'changeoverTime') ?? ''
  },
  {
    title: t('entity.changeover.totalchangeovertime'),
    dataIndex: 'totalChangeoverTime',
    key: 'totalChangeoverTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getChangeoverField(record, 'totalChangeoverTime') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:changeover:update',
        onClick: (record: Changeover) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:changeover:delete',
        onClick: (record: Changeover) => handleDeleteOne(record)
      }
    ]
  })
])

const getChangeoverId = (record: any): string => record?.[entityIdName] ?? ''
const getChangeoverField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Changeover[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Changeover, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getChangeoverId(selectedRow.value) === getChangeoverId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Changeover[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Changeover) => ({
  onClick: () => {
    const key = getChangeoverId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getChangeoverId(item)))
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
    const params: ChangeoverQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getChangeoverList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Changeover] 加载数据失败', { error })
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
  productionCategory: '',
  productionLine: '',
  readSopTime: undefined as number | undefined,
  personCount: undefined as number | undefined,
  totalSopTime: undefined as number | undefined,
  changeoverCount: undefined as number | undefined,
  changeoverTime: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.changeover._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Changeover) {
  formTitle.value = t('common.page.button.edit') + t('entity.changeover._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.changeover._self') }))
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
      await updateChangeover(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.changeover._self') }))
    } else {
      await createChangeover(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.changeover._self') }))
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
  const res = await getChangeoverTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importChangeover(file, sheetName)
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
    const exportQuery: ChangeoverQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportChangeover(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.changeover._self') }))
  } catch (error: any) {
    logger.error('[Changeover] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.changeover._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Changeover) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.changeover._self'), name: t('common.tip.this.target', { target: t('entity.changeover._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteChangeoverById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.changeover._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.changeover._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.changeover._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteChangeoverBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.changeover._self') }))
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
  productionCategory: '',
  productionLine: '',
  readSopTime: undefined as number | undefined,
  personCount: undefined as number | undefined,
  totalSopTime: undefined as number | undefined,
  changeoverCount: undefined as number | undefined,
  changeoverTime: undefined as number | undefined,
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
.logistics-manufacturing-output-changeover {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
