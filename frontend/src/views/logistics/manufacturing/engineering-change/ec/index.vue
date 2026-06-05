<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-engineering-change-ec">
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
      create-permission="logistics:manufacturing:engineeringchange:ec:create"
      update-permission="logistics:manufacturing:engineeringchange:ec:update"
      delete-permission="logistics:manufacturing:engineeringchange:ec:delete"
      import-permission="logistics:manufacturing:engineeringchange:ec:import"
      export-permission="logistics:manufacturing:engineeringchange:ec:export"
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
      :row-key="getEcId"
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
      <EcForm
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
      <a-form-item :label="t('entity.ec.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.no')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.no') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.changestatus')">
        <a-input
          v-model:value="advancedQueryForm.changeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.changestatus') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.title')">
        <a-input
          v-model:value="advancedQueryForm.ecTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.title') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.detailtext')">
        <a-input
          v-model:value="advancedQueryForm.ecDetailText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.detailtext') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.leader')">
        <a-input
          v-model:value="advancedQueryForm.ecLeader"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.leader') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.lossamount')">
        <a-input
          v-model:value="advancedQueryForm.ecLossAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.lossamount') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.ec.distinction')">
        <a-input
          v-model:value="advancedQueryForm.ecDistinction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.distinction') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.ec._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ec._self"
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
      :id-column-key="'ecId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import EcForm from './components/ec-form.vue'
import { getEcList, getEcById, createEc, updateEc, deleteEcById, deleteEcBatch, getEcTemplate, importEc, exportEc } from '@/api/logistics/manufacturing/engineering-change/ec'
import type { Ec, EcQuery, EcCreate, EcUpdate } from '@/types/logistics/manufacturing/engineering-change/ec'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktEc')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ec._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Ec[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Ec | null>(null)
const selectedRows = ref<Ec[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Ec>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  ecNo: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'ecId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecId',
    key: 'ecId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecId') ?? ''
  },
  {
    title: t('entity.ec.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ec.no'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecNo') ?? ''
  },
  {
    title: t('entity.ec.issuedate'),
    dataIndex: 'ecIssueDate',
    key: 'ecIssueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecIssueDate') ?? ''
  },
  {
    title: t('entity.ec.changestatus'),
    dataIndex: 'changeStatus',
    key: 'changeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'changeStatus') ?? ''
  },
  {
    title: t('entity.ec.title'),
    dataIndex: 'ecTitle',
    key: 'ecTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecTitle') ?? ''
  },
  {
    title: t('entity.ec.detailtext'),
    dataIndex: 'ecDetailText',
    key: 'ecDetailText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecDetailText') ?? ''
  },
  {
    title: t('entity.ec.leader'),
    dataIndex: 'ecLeader',
    key: 'ecLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecLeader') ?? ''
  },
  {
    title: t('entity.ec.lossamount'),
    dataIndex: 'ecLossAmount',
    key: 'ecLossAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecLossAmount') ?? ''
  },
  {
    title: t('entity.ec.distinction'),
    dataIndex: 'ecDistinction',
    key: 'ecDistinction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecDistinction') ?? ''
  },
  {
    title: t('entity.ec.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.ec.entrydate'),
    dataIndex: 'ecEntryDate',
    key: 'ecEntryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecEntryDate') ?? ''
  },
  {
    title: t('entity.ec.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'flowInstanceId') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineeringchange:ec:update',
        onClick: (record: Ec) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineeringchange:ec:delete',
        onClick: (record: Ec) => handleDeleteOne(record)
      }
    ]
  })
])

const getEcId = (record: any): string => record?.[entityIdName] ?? ''
const getEcField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Ec[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Ec, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEcId(selectedRow.value) === getEcId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Ec[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Ec) => ({
  onClick: () => {
    const key = getEcId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEcId(item)))
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
    const params: EcQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEcList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Ec] 加载数据失败', { error })
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
  ecNo: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.ec._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Ec) {
  formTitle.value = t('common.page.button.edit') + t('entity.ec._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ec._self') }))
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
      await updateEc(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ec._self') }))
    } else {
      await createEc(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ec._self') }))
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
  const res = await getEcTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEc(file, sheetName)
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
    const exportQuery: EcQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEc(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.ec._self') }))
  } catch (error: any) {
    logger.error('[Ec] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ec._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Ec) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ec._self'), name: t('common.tip.this.target', { target: t('entity.ec._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ec._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ec._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ec._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEcBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ec._self') }))
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
  ecNo: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
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
.logistics-manufacturing-engineering-change-ec {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
