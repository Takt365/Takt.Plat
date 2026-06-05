<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/financial/asset -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：资产实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-asset">
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
      create-permission="accounting:financial:asset:create"
      update-permission="accounting:financial:asset:update"
      delete-permission="accounting:financial:asset:delete"
      import-permission="accounting:financial:asset:import"
      export-permission="accounting:financial:asset:export"
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
      :row-key="getAssetId"
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
      <AssetForm
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
      <a-form-item :label="t('entity.asset.code')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.code') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.name')">
        <a-input
          v-model:value="advancedQueryForm.assetName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.name') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.categoryid')">
        <a-input
          v-model:value="advancedQueryForm.assetCategoryId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.categoryid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.categoryname')">
        <a-input
          v-model:value="advancedQueryForm.assetCategoryName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.categoryname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.type')">
        <a-input
          v-model:value="advancedQueryForm.assetType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.type') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.originalvalue')">
        <a-input
          v-model:value="advancedQueryForm.assetOriginalValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.originalvalue') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.netvalue')">
        <a-input
          v-model:value="advancedQueryForm.assetNetValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.netvalue') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.asset.accumulateddepreciation')">
        <a-input
          v-model:value="advancedQueryForm.accumulatedDepreciation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.accumulateddepreciation') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.asset._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.asset._self"
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
      :id-column-key="'assetId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 资产实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/asset
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import AssetForm from './components/asset-form.vue'
import { getAssetList, getAssetById, createAsset, updateAsset, deleteAssetById, deleteAssetBatch, getAssetTemplate, importAsset, exportAsset } from '@/api/accounting/financial/asset'
import type { Asset, AssetQuery, AssetCreate, AssetUpdate } from '@/types/accounting/financial/asset'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktAsset')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.asset._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Asset[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Asset | null>(null)
const selectedRows = ref<Asset[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Asset>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  assetCode: '',
  assetName: '',
  assetCategoryId: '',
  assetCategoryName: '',
  assetType: undefined as number | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'assetId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assetId',
    key: 'assetId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetId') ?? ''
  },
  {
    title: t('entity.asset.code'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetCode') ?? ''
  },
  {
    title: t('entity.asset.name'),
    dataIndex: 'assetName',
    key: 'assetName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetName') ?? ''
  },
  {
    title: t('entity.asset.categoryid'),
    dataIndex: 'assetCategoryId',
    key: 'assetCategoryId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetCategoryId') ?? ''
  },
  {
    title: t('entity.asset.categoryname'),
    dataIndex: 'assetCategoryName',
    key: 'assetCategoryName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetCategoryName') ?? ''
  },
  {
    title: t('entity.asset.type'),
    dataIndex: 'assetType',
    key: 'assetType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetType') ?? ''
  },
  {
    title: t('entity.asset.originalvalue'),
    dataIndex: 'assetOriginalValue',
    key: 'assetOriginalValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetOriginalValue') ?? ''
  },
  {
    title: t('entity.asset.netvalue'),
    dataIndex: 'assetNetValue',
    key: 'assetNetValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetNetValue') ?? ''
  },
  {
    title: t('entity.asset.accumulateddepreciation'),
    dataIndex: 'accumulatedDepreciation',
    key: 'accumulatedDepreciation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'accumulatedDepreciation') ?? ''
  },
  {
    title: t('entity.asset.costcenterid'),
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'costCenterId') ?? ''
  },
  {
    title: t('entity.asset.costcentername'),
    dataIndex: 'costCenterName',
    key: 'costCenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'costCenterName') ?? ''
  },
  {
    title: t('entity.asset.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.asset.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'deptName') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:asset:update',
        onClick: (record: Asset) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:asset:delete',
        onClick: (record: Asset) => handleDeleteOne(record)
      }
    ]
  })
])

const getAssetId = (record: any): string => record?.[entityIdName] ?? ''
const getAssetField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Asset[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Asset, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssetId(selectedRow.value) === getAssetId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Asset[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Asset) => ({
  onClick: () => {
    const key = getAssetId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssetId(item)))
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
    const params: AssetQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAssetList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Asset] 加载数据失败', { error })
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
  assetCode: '',
  assetName: '',
  assetCategoryId: '',
  assetCategoryName: '',
  assetType: undefined as number | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.asset._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Asset) {
  formTitle.value = t('common.page.button.edit') + t('entity.asset._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.asset._self') }))
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
      await updateAsset(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.asset._self') }))
    } else {
      await createAsset(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.asset._self') }))
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
  const res = await getAssetTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAsset(file, sheetName)
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
    const exportQuery: AssetQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAsset(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.asset._self') }))
  } catch (error: any) {
    logger.error('[Asset] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.asset._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Asset) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.asset._self'), name: t('common.tip.this.target', { target: t('entity.asset._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssetById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.asset._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.asset._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.asset._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssetBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.asset._self') }))
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
  assetCode: '',
  assetName: '',
  assetCategoryId: '',
  assetCategoryName: '',
  assetType: undefined as number | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
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
.accounting-financial-asset {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
