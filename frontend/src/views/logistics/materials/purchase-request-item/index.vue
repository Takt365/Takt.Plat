<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-request-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购申请明细实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchase-request-item">
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
      create-permission="logistics:materials:purchaserequestitem:create"
      update-permission="logistics:materials:purchaserequestitem:update"
      delete-permission="logistics:materials:purchaserequestitem:delete"
      import-permission="logistics:materials:purchaserequestitem:import"
      export-permission="logistics:materials:purchaserequestitem:export"
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
      :row-key="getPurchaseRequestItemId"
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
      <PurchaseRequestItemForm
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
      <a-form-item :label="t('entity.purchaseRequestItem.purchaserequestid')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.purchaserequestcode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.linenumber')">
        <a-input
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.linenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialspecification') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.requestunit')">
        <a-input
          v-model:value="advancedQueryForm.requestUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestunit') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseRequestItem.requestquantity')">
        <a-input
          v-model:value="advancedQueryForm.requestQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestquantity') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.purchaseRequestItem._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseRequestItem._self"
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
      :id-column-key="'purchaseRequestItemId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购申请明细实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchase-request-item
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import PurchaseRequestItemForm from './components/purchase-request-item-form.vue'
import { getPurchaseRequestItemList, getPurchaseRequestItemById, createPurchaseRequestItem, updatePurchaseRequestItem, deletePurchaseRequestItemById, deletePurchaseRequestItemBatch, getPurchaseRequestItemTemplate, importPurchaseRequestItem, exportPurchaseRequestItem } from '@/api/logistics/materials/purchase-request-item'
import type { PurchaseRequestItem, PurchaseRequestItemQuery, PurchaseRequestItemCreate, PurchaseRequestItemUpdate } from '@/types/logistics/materials/purchase-request-item'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktPurchaseRequestItem')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseRequestItem._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<PurchaseRequestItem[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<PurchaseRequestItem | null>(null)
const selectedRows = ref<PurchaseRequestItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseRequestItem>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchaseRequestId: '',
  purchaseRequestCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  requestUnit: '',
  requestQuantity: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'purchaseRequestItemId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseRequestItemId',
    key: 'purchaseRequestItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'purchaseRequestItemId') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.purchaserequestid'),
    dataIndex: 'purchaseRequestId',
    key: 'purchaseRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'purchaseRequestId') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.purchaserequestname'),
    dataIndex: 'purchaseRequestName',
    key: 'purchaseRequestName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'purchaseRequestName') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.purchaserequestcode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'purchaseRequestCode') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'materialName') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'materialSpecification') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.requestunit'),
    dataIndex: 'requestUnit',
    key: 'requestUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'requestUnit') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.requestquantity'),
    dataIndex: 'requestQuantity',
    key: 'requestQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'requestQuantity') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.convertedquantity'),
    dataIndex: 'convertedQuantity',
    key: 'convertedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'convertedQuantity') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.estimatedunitprice'),
    dataIndex: 'estimatedUnitPrice',
    key: 'estimatedUnitPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'estimatedUnitPrice') ?? ''
  },
  {
    title: t('entity.purchaseRequestItem.estimatedamount'),
    dataIndex: 'estimatedAmount',
    key: 'estimatedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestItemField(record, 'estimatedAmount') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:purchaserequestitem:update',
        onClick: (record: PurchaseRequestItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:purchaserequestitem:delete',
        onClick: (record: PurchaseRequestItem) => handleDeleteOne(record)
      }
    ]
  })
])

const getPurchaseRequestItemId = (record: any): string => record?.[entityIdName] ?? ''
const getPurchaseRequestItemField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: PurchaseRequestItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseRequestItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchaseRequestItemId(selectedRow.value) === getPurchaseRequestItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseRequestItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: PurchaseRequestItem) => ({
  onClick: () => {
    const key = getPurchaseRequestItemId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPurchaseRequestItemId(item)))
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
    const params: PurchaseRequestItemQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPurchaseRequestItemList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseRequestItem] 加载数据失败', { error })
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
  purchaseRequestId: '',
  purchaseRequestCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  requestUnit: '',
  requestQuantity: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.purchaseRequestItem._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: PurchaseRequestItem) {
  formTitle.value = t('common.page.button.edit') + t('entity.purchaseRequestItem._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseRequestItem._self') }))
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
      await updatePurchaseRequestItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseRequestItem._self') }))
    } else {
      await createPurchaseRequestItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseRequestItem._self') }))
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
  const res = await getPurchaseRequestItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseRequestItem(file, sheetName)
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
    const exportQuery: PurchaseRequestItemQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPurchaseRequestItem(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseRequestItem._self') }))
  } catch (error: any) {
    logger.error('[PurchaseRequestItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseRequestItem._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: PurchaseRequestItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseRequestItem._self'), name: t('common.tip.this.target', { target: t('entity.purchaseRequestItem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseRequestItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseRequestItem._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseRequestItem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseRequestItem._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseRequestItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseRequestItem._self') }))
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
  purchaseRequestId: '',
  purchaseRequestCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  requestUnit: '',
  requestQuantity: undefined as number | undefined,
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
.logistics-materials-purchase-request-item {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
