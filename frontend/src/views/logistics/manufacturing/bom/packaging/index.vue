<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/packaging -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料包装信息实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-bom-packaging">
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
      create-permission="logistics:manufacturing:bom:packaging:create"
      update-permission="logistics:manufacturing:bom:packaging:update"
      delete-permission="logistics:manufacturing:bom:packaging:delete"
      import-permission="logistics:manufacturing:bom:packaging:import"
      export-permission="logistics:manufacturing:bom:packaging:export"
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
      :row-key="getPackagingId"
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
      <PackagingForm
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
      <a-form-item :label="t('entity.packaging.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.materialcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.hscode')">
        <a-input
          v-model:value="advancedQueryForm.hsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hscode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.hsname')">
        <a-input
          v-model:value="advancedQueryForm.hsName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hsname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.additionalcode')">
        <a-input
          v-model:value="advancedQueryForm.additionalCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.additionalcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.origincountryregioncode')">
        <a-input
          v-model:value="advancedQueryForm.originCountryRegionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregioncode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.origincountryregionname')">
        <a-input
          v-model:value="advancedQueryForm.originCountryRegionName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregionname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.packaging.destinationcountryregioncode')">
        <a-input
          v-model:value="advancedQueryForm.destinationCountryRegionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.destinationcountryregioncode') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.packaging._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.packaging._self"
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
      :id-column-key="'packagingId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt物料包装信息实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/packaging
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import PackagingForm from './components/packaging-form.vue'
import { getPackagingList, getPackagingById, createPackaging, updatePackaging, deletePackagingById, deletePackagingBatch, getPackagingTemplate, importPackaging, exportPackaging } from '@/api/logistics/manufacturing/bom/packaging'
import type { Packaging, PackagingQuery, PackagingCreate, PackagingUpdate } from '@/types/logistics/manufacturing/bom/packaging'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktPackaging')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.packaging._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Packaging[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Packaging | null>(null)
const selectedRows = ref<Packaging[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Packaging>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'packagingId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'packagingId',
    key: 'packagingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packagingId') ?? ''
  },
  {
    title: t('entity.packaging.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.packaging.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.packaging.hscode'),
    dataIndex: 'hsCode',
    key: 'hsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'hsCode') ?? ''
  },
  {
    title: t('entity.packaging.hsname'),
    dataIndex: 'hsName',
    key: 'hsName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'hsName') ?? ''
  },
  {
    title: t('entity.packaging.additionalcode'),
    dataIndex: 'additionalCode',
    key: 'additionalCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'additionalCode') ?? ''
  },
  {
    title: t('entity.packaging.origincountryregioncode'),
    dataIndex: 'originCountryRegionCode',
    key: 'originCountryRegionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'originCountryRegionCode') ?? ''
  },
  {
    title: t('entity.packaging.origincountryregionname'),
    dataIndex: 'originCountryRegionName',
    key: 'originCountryRegionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'originCountryRegionName') ?? ''
  },
  {
    title: t('entity.packaging.destinationcountryregioncode'),
    dataIndex: 'destinationCountryRegionCode',
    key: 'destinationCountryRegionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'destinationCountryRegionCode') ?? ''
  },
  {
    title: t('entity.packaging.destinationcountryregionname'),
    dataIndex: 'destinationCountryRegionName',
    key: 'destinationCountryRegionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'destinationCountryRegionName') ?? ''
  },
  {
    title: t('entity.packaging.regulatoryconditioncode'),
    dataIndex: 'regulatoryConditionCode',
    key: 'regulatoryConditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'regulatoryConditionCode') ?? ''
  },
  {
    title: t('entity.packaging.tariffratetype'),
    dataIndex: 'tariffRateType',
    key: 'tariffRateType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'tariffRateType') ?? ''
  },
  {
    title: t('entity.packaging.grossweight'),
    dataIndex: 'grossWeight',
    key: 'grossWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'grossWeight') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:packaging:update',
        onClick: (record: Packaging) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:packaging:delete',
        onClick: (record: Packaging) => handleDeleteOne(record)
      }
    ]
  })
])

const getPackagingId = (record: any): string => record?.[entityIdName] ?? ''
const getPackagingField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Packaging[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Packaging, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPackagingId(selectedRow.value) === getPackagingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Packaging[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Packaging) => ({
  onClick: () => {
    const key = getPackagingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPackagingId(item)))
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
    const params: PackagingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPackagingList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Packaging] 加载数据失败', { error })
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
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.packaging._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Packaging) {
  formTitle.value = t('common.page.button.edit') + t('entity.packaging._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.packaging._self') }))
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
      await updatePackaging(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.packaging._self') }))
    } else {
      await createPackaging(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.packaging._self') }))
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
  const res = await getPackagingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPackaging(file, sheetName)
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
    const exportQuery: PackagingQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPackaging(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.packaging._self') }))
  } catch (error: any) {
    logger.error('[Packaging] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.packaging._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Packaging) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.packaging._self'), name: t('common.tip.this.target', { target: t('entity.packaging._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePackagingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.packaging._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.packaging._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.packaging._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePackagingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.packaging._self') }))
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
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
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
.logistics-manufacturing-bom-packaging {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
