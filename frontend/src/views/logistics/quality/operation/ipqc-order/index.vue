<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IPQC制程检验单实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-ipqc-order">
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
      create-permission="logistics:quality:operation:ipqcorder:create"
      update-permission="logistics:quality:operation:ipqcorder:update"
      delete-permission="logistics:quality:operation:ipqcorder:delete"
      import-permission="logistics:quality:operation:ipqcorder:import"
      export-permission="logistics:quality:operation:ipqcorder:export"
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
      :row-key="getIpqcOrderId"
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
      <IpqcOrderForm
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
      <a-form-item :label="t('entity.ipqcOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.processcode')">
        <a-input
          v-model:value="advancedQueryForm.processCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.processname')">
        <a-input
          v-model:value="advancedQueryForm.processName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.totalproductionquantity')">
        <a-input
          v-model:value="advancedQueryForm.totalProductionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalproductionquantity') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.totalsamplequantity')">
        <a-input
          v-model:value="advancedQueryForm.totalSampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalsamplequantity') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ipqcOrder.totalqualifiedquantity')">
        <a-input
          v-model:value="advancedQueryForm.totalQualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalqualifiedquantity') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.ipqcOrder._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ipqcOrder._self"
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
      :id-column-key="'ipqcOrderId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * IPQC制程检验单实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import IpqcOrderForm from './components/ipqc-order-form.vue'
import { getIpqcOrderList, getIpqcOrderById, createIpqcOrder, updateIpqcOrder, deleteIpqcOrderById, deleteIpqcOrderBatch, getIpqcOrderTemplate, importIpqcOrder, exportIpqcOrder } from '@/api/logistics/quality/operation/ipqc-order'
import type { IpqcOrder, IpqcOrderQuery, IpqcOrderCreate, IpqcOrderUpdate } from '@/types/logistics/quality/operation/ipqc-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktIpqcOrder')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcOrder._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<IpqcOrder[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<IpqcOrder | null>(null)
const selectedRows = ref<IpqcOrder[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<IpqcOrder>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  sourceCode: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'ipqcOrderId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ipqcOrderId',
    key: 'ipqcOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'ipqcOrderId') ?? ''
  },
  {
    title: t('entity.ipqcOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.ipqcOrder.code'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'ipqcOrderCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processName') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalproductionquantity'),
    dataIndex: 'totalProductionQuantity',
    key: 'totalProductionQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalProductionQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalsamplequantity'),
    dataIndex: 'totalSampleQuantity',
    key: 'totalSampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalSampleQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalqualifiedquantity'),
    dataIndex: 'totalQualifiedQuantity',
    key: 'totalQualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalQualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalunqualifiedquantity'),
    dataIndex: 'totalUnqualifiedQuantity',
    key: 'totalUnqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalUnqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalinspectionreturnquantity'),
    dataIndex: 'totalInspectionReturnQuantity',
    key: 'totalInspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalInspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:ipqcorder:update',
        onClick: (record: IpqcOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqcorder:delete',
        onClick: (record: IpqcOrder) => handleDeleteOne(record)
      }
    ]
  })
])

const getIpqcOrderId = (record: any): string => record?.[entityIdName] ?? ''
const getIpqcOrderField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: IpqcOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IpqcOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getIpqcOrderId(selectedRow.value) === getIpqcOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IpqcOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: IpqcOrder) => ({
  onClick: () => {
    const key = getIpqcOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getIpqcOrderId(item)))
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
    const params: IpqcOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getIpqcOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[IpqcOrder] 加载数据失败', { error })
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
  sourceCode: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.ipqcOrder._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: IpqcOrder) {
  formTitle.value = t('common.page.button.edit') + t('entity.ipqcOrder._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ipqcOrder._self') }))
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
      await updateIpqcOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ipqcOrder._self') }))
    } else {
      await createIpqcOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ipqcOrder._self') }))
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
  const res = await getIpqcOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importIpqcOrder(file, sheetName)
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
    const exportQuery: IpqcOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportIpqcOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcOrder._self') }))
  } catch (error: any) {
    logger.error('[IpqcOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ipqcOrder._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: IpqcOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ipqcOrder._self'), name: t('common.tip.this.target', { target: t('entity.ipqcOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcOrder._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ipqcOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ipqcOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteIpqcOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcOrder._self') }))
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
  sourceCode: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
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
.logistics-quality-operation-ipqc-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
