<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：人员稼动率实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-personnel-operation-rate">
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
      create-permission="logistics:manufacturing:output:personneloperationrate:create"
      update-permission="logistics:manufacturing:output:personneloperationrate:update"
      delete-permission="logistics:manufacturing:output:personneloperationrate:delete"
      import-permission="logistics:manufacturing:output:personneloperationrate:import"
      export-permission="logistics:manufacturing:output:personneloperationrate:export"
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
      :row-key="getPersonnelOperationRateId"
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
      <PersonnelOperationRateForm
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
      <a-form-item :label="t('entity.personnelOperationRate.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plantcode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.timecategory')">
        <a-input
          v-model:value="advancedQueryForm.timeCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.timecategory') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.weeknumber')">
        <a-input
          v-model:value="advancedQueryForm.weekNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.weeknumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.monthnumber')">
        <a-input
          v-model:value="advancedQueryForm.monthNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.monthnumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.productionline')">
        <a-input
          v-model:value="advancedQueryForm.productionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionline') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.productionlinename')">
        <a-input
          v-model:value="advancedQueryForm.productionLineName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionlinename') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.shiftno')">
        <a-input
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.shiftno') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.personnelOperationRate.planneddirectpersonnelcount')">
        <a-input
          v-model:value="advancedQueryForm.plannedDirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.planneddirectpersonnelcount') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.personnelOperationRate._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.personnelOperationRate._self"
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
      :id-column-key="'personnelOperationRateId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 人员稼动率实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/personnel-operation-rate
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import PersonnelOperationRateForm from './components/personnel-operation-rate-form.vue'
import { getPersonnelOperationRateList, getPersonnelOperationRateById, createPersonnelOperationRate, updatePersonnelOperationRate, deletePersonnelOperationRateById, deletePersonnelOperationRateBatch, getPersonnelOperationRateTemplate, importPersonnelOperationRate, exportPersonnelOperationRate } from '@/api/logistics/manufacturing/output/personnel-operation-rate'
import type { PersonnelOperationRate, PersonnelOperationRateQuery, PersonnelOperationRateCreate, PersonnelOperationRateUpdate } from '@/types/logistics/manufacturing/output/personnel-operation-rate'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktPersonnelOperationRate')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.personnelOperationRate._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<PersonnelOperationRate[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<PersonnelOperationRate | null>(null)
const selectedRows = ref<PersonnelOperationRate[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PersonnelOperationRate>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  timeCategory: undefined as number | undefined,
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'personnelOperationRateId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'personnelOperationRateId',
    key: 'personnelOperationRateId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'personnelOperationRateId') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.timecategory'),
    dataIndex: 'timeCategory',
    key: 'timeCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'timeCategory') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.weeknumber'),
    dataIndex: 'weekNumber',
    key: 'weekNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'weekNumber') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.monthnumber'),
    dataIndex: 'monthNumber',
    key: 'monthNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'monthNumber') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.productionline'),
    dataIndex: 'productionLine',
    key: 'productionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLine') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.productionlinename'),
    dataIndex: 'productionLineName',
    key: 'productionLineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLineName') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.planneddirectpersonnelcount'),
    dataIndex: 'plannedDirectPersonnelCount',
    key: 'plannedDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.actualdirectpersonnelcount'),
    dataIndex: 'actualDirectPersonnelCount',
    key: 'actualDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plannedindirectpersonnelcount'),
    dataIndex: 'plannedIndirectPersonnelCount',
    key: 'plannedIndirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedIndirectPersonnelCount') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:personneloperationrate:update',
        onClick: (record: PersonnelOperationRate) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:personneloperationrate:delete',
        onClick: (record: PersonnelOperationRate) => handleDeleteOne(record)
      }
    ]
  })
])

const getPersonnelOperationRateId = (record: any): string => record?.[entityIdName] ?? ''
const getPersonnelOperationRateField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: PersonnelOperationRate[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PersonnelOperationRate, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPersonnelOperationRateId(selectedRow.value) === getPersonnelOperationRateId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PersonnelOperationRate[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: PersonnelOperationRate) => ({
  onClick: () => {
    const key = getPersonnelOperationRateId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPersonnelOperationRateId(item)))
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
    const params: PersonnelOperationRateQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPersonnelOperationRateList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 加载数据失败', { error })
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
  timeCategory: undefined as number | undefined,
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.personnelOperationRate._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: PersonnelOperationRate) {
  formTitle.value = t('common.page.button.edit') + t('entity.personnelOperationRate._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.personnelOperationRate._self') }))
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
      await updatePersonnelOperationRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.personnelOperationRate._self') }))
    } else {
      await createPersonnelOperationRate(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.personnelOperationRate._self') }))
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
  const res = await getPersonnelOperationRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPersonnelOperationRate(file, sheetName)
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
    const exportQuery: PersonnelOperationRateQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPersonnelOperationRate(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.personnelOperationRate._self') }))
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.personnelOperationRate._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: PersonnelOperationRate) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.personnelOperationRate._self'), name: t('common.tip.this.target', { target: t('entity.personnelOperationRate._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePersonnelOperationRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.personnelOperationRate._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.personnelOperationRate._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.personnelOperationRate._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePersonnelOperationRateBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.personnelOperationRate._self') }))
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
  timeCategory: undefined as number | undefined,
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
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
.logistics-manufacturing-output-personnel-operation-rate {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
