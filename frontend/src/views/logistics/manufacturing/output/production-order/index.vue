<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：生产工单实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-production-order">
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
      create-permission="logistics:manufacturing:output:productionorder:create"
      update-permission="logistics:manufacturing:output:productionorder:update"
      delete-permission="logistics:manufacturing:output:productionorder:delete"
      import-permission="logistics:manufacturing:output:productionorder:import"
      export-permission="logistics:manufacturing:output:productionorder:export"
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
      :row-key="getProductionOrderId"
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
      <ProductionOrderForm
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
      <a-form-item :label="t('entity.productionOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.prodordertype')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.prodordertype') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.prodordercode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.materialcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.prodorderqty')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.prodorderqty') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.producedqty')">
        <a-input
          v-model:value="advancedQueryForm.producedQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.producedqty') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.unitofmeasure')">
        <a-input
          v-model:value="advancedQueryForm.unitOfMeasure"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.unitofmeasure') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.productionOrder.priority')">
        <a-input
          v-model:value="advancedQueryForm.priority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionOrder.priority') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.productionOrder._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.productionOrder._self"
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
      :id-column-key="'productionOrderId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 生产工单实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/production-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import ProductionOrderForm from './components/production-order-form.vue'
import { getProductionOrderList, getProductionOrderById, createProductionOrder, updateProductionOrder, deleteProductionOrderById, deleteProductionOrderBatch, getProductionOrderTemplate, importProductionOrder, exportProductionOrder } from '@/api/logistics/manufacturing/output/production-order'
import type { ProductionOrder, ProductionOrderQuery, ProductionOrderCreate, ProductionOrderUpdate } from '@/types/logistics/manufacturing/output/production-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktProductionOrder')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.productionOrder._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<ProductionOrder[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<ProductionOrder | null>(null)
const selectedRows = ref<ProductionOrder[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ProductionOrder>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  prodOrderType: '',
  prodOrderCode: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  producedQty: undefined as number | undefined,
  unitOfMeasure: '',
  priority: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'productionOrderId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'productionOrderId',
    key: 'productionOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'productionOrderId') ?? ''
  },
  {
    title: t('entity.productionOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.productionOrder.prodordertype'),
    dataIndex: 'prodOrderType',
    key: 'prodOrderType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'prodOrderType') ?? ''
  },
  {
    title: t('entity.productionOrder.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.productionOrder.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.productionOrder.prodorderqty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'prodOrderQty') ?? ''
  },
  {
    title: t('entity.productionOrder.producedqty'),
    dataIndex: 'producedQty',
    key: 'producedQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'producedQty') ?? ''
  },
  {
    title: t('entity.productionOrder.unitofmeasure'),
    dataIndex: 'unitOfMeasure',
    key: 'unitOfMeasure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'unitOfMeasure') ?? ''
  },
  {
    title: t('entity.productionOrder.actualstartdate'),
    dataIndex: 'actualStartDate',
    key: 'actualStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'actualStartDate') ?? ''
  },
  {
    title: t('entity.productionOrder.actualenddate'),
    dataIndex: 'actualEndDate',
    key: 'actualEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'actualEndDate') ?? ''
  },
  {
    title: t('entity.productionOrder.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'priority') ?? ''
  },
  {
    title: t('entity.productionOrder.workcenter'),
    dataIndex: 'workCenter',
    key: 'workCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'workCenter') ?? ''
  },
  {
    title: t('entity.productionOrder.prodline'),
    dataIndex: 'prodLine',
    key: 'prodLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductionOrderField(record, 'prodLine') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:productionorder:update',
        onClick: (record: ProductionOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:productionorder:delete',
        onClick: (record: ProductionOrder) => handleDeleteOne(record)
      }
    ]
  })
])

const getProductionOrderId = (record: any): string => record?.[entityIdName] ?? ''
const getProductionOrderField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: ProductionOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ProductionOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getProductionOrderId(selectedRow.value) === getProductionOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ProductionOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: ProductionOrder) => ({
  onClick: () => {
    const key = getProductionOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getProductionOrderId(item)))
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
    const params: ProductionOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getProductionOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ProductionOrder] 加载数据失败', { error })
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
  prodOrderType: '',
  prodOrderCode: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  producedQty: undefined as number | undefined,
  unitOfMeasure: '',
  priority: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.productionOrder._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: ProductionOrder) {
  formTitle.value = t('common.page.button.edit') + t('entity.productionOrder._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.productionOrder._self') }))
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
      await updateProductionOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.productionOrder._self') }))
    } else {
      await createProductionOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.productionOrder._self') }))
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
  const res = await getProductionOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importProductionOrder(file, sheetName)
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
    const exportQuery: ProductionOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportProductionOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.productionOrder._self') }))
  } catch (error: any) {
    logger.error('[ProductionOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.productionOrder._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: ProductionOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.productionOrder._self'), name: t('common.tip.this.target', { target: t('entity.productionOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProductionOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.productionOrder._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.productionOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.productionOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteProductionOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.productionOrder._self') }))
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
  prodOrderType: '',
  prodOrderCode: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  producedQty: undefined as number | undefined,
  unitOfMeasure: '',
  priority: undefined as number | undefined,
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
.logistics-manufacturing-output-production-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
