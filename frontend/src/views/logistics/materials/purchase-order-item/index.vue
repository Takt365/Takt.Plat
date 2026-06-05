<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-order-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购订单明细实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchase-order-item">
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
      create-permission="logistics:materials:purchaseorderitem:create"
      update-permission="logistics:materials:purchaseorderitem:update"
      delete-permission="logistics:materials:purchaseorderitem:delete"
      import-permission="logistics:materials:purchaseorderitem:import"
      export-permission="logistics:materials:purchaseorderitem:export"
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
      :row-key="getPurchaseOrderItemId"
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
      <PurchaseOrderItemForm
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
      <a-form-item :label="t('entity.purchaseOrderItem.purchaseorderid')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseorderid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.purchaseordercode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseordercode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.linenumber')">
        <a-input
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.linenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.requestcode')">
        <a-input
          v-model:value="advancedQueryForm.requestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.requestcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.requestlinenumber')">
        <a-input
          v-model:value="advancedQueryForm.requestLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.requestlinenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.purchaseOrderItem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialspecification') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.purchaseOrderItem._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseOrderItem._self"
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
      :id-column-key="'purchaseOrderItemId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购订单明细实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchase-order-item
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import PurchaseOrderItemForm from './components/purchase-order-item-form.vue'
import { getPurchaseOrderItemList, getPurchaseOrderItemById, createPurchaseOrderItem, updatePurchaseOrderItem, deletePurchaseOrderItemById, deletePurchaseOrderItemBatch, getPurchaseOrderItemTemplate, importPurchaseOrderItem, exportPurchaseOrderItem } from '@/api/logistics/materials/purchase-order-item'
import type { PurchaseOrderItem, PurchaseOrderItemQuery, PurchaseOrderItemCreate, PurchaseOrderItemUpdate } from '@/types/logistics/materials/purchase-order-item'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktPurchaseOrderItem')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseOrderItem._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<PurchaseOrderItem[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<PurchaseOrderItem | null>(null)
const selectedRows = ref<PurchaseOrderItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseOrderItem>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchaseOrderId: '',
  purchaseOrderCode: '',
  lineNumber: undefined as number | undefined,
  requestCode: '',
  requestLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'purchaseOrderItemId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseOrderItemId',
    key: 'purchaseOrderItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'purchaseOrderItemId') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.purchaseorderid'),
    dataIndex: 'purchaseOrderId',
    key: 'purchaseOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'purchaseOrderId') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.purchaseordername'),
    dataIndex: 'purchaseOrderName',
    key: 'purchaseOrderName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'purchaseOrderName') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.purchaseordercode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.requestcode'),
    dataIndex: 'requestCode',
    key: 'requestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'requestCode') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.requestlinenumber'),
    dataIndex: 'requestLineNumber',
    key: 'requestLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'requestLineNumber') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'materialName') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'materialSpecification') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.purchaseunit'),
    dataIndex: 'purchaseUnit',
    key: 'purchaseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'purchaseUnit') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.orderquantity'),
    dataIndex: 'orderQuantity',
    key: 'orderQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'orderQuantity') ?? ''
  },
  {
    title: t('entity.purchaseOrderItem.receivedquantity'),
    dataIndex: 'receivedQuantity',
    key: 'receivedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderItemField(record, 'receivedQuantity') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:purchaseorderitem:update',
        onClick: (record: PurchaseOrderItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:purchaseorderitem:delete',
        onClick: (record: PurchaseOrderItem) => handleDeleteOne(record)
      }
    ]
  })
])

const getPurchaseOrderItemId = (record: any): string => record?.[entityIdName] ?? ''
const getPurchaseOrderItemField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: PurchaseOrderItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseOrderItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchaseOrderItemId(selectedRow.value) === getPurchaseOrderItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseOrderItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: PurchaseOrderItem) => ({
  onClick: () => {
    const key = getPurchaseOrderItemId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPurchaseOrderItemId(item)))
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
    const params: PurchaseOrderItemQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPurchaseOrderItemList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseOrderItem] 加载数据失败', { error })
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
  purchaseOrderId: '',
  purchaseOrderCode: '',
  lineNumber: undefined as number | undefined,
  requestCode: '',
  requestLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.purchaseOrderItem._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: PurchaseOrderItem) {
  formTitle.value = t('common.page.button.edit') + t('entity.purchaseOrderItem._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseOrderItem._self') }))
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
      await updatePurchaseOrderItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseOrderItem._self') }))
    } else {
      await createPurchaseOrderItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseOrderItem._self') }))
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
  const res = await getPurchaseOrderItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseOrderItem(file, sheetName)
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
    const exportQuery: PurchaseOrderItemQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPurchaseOrderItem(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseOrderItem._self') }))
  } catch (error: any) {
    logger.error('[PurchaseOrderItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseOrderItem._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: PurchaseOrderItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseOrderItem._self'), name: t('common.tip.this.target', { target: t('entity.purchaseOrderItem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseOrderItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseOrderItem._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseOrderItem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseOrderItem._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseOrderItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseOrderItem._self') }))
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
  purchaseOrderId: '',
  purchaseOrderCode: '',
  lineNumber: undefined as number | undefined,
  requestCode: '',
  requestLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
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
.logistics-materials-purchase-order-item {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
