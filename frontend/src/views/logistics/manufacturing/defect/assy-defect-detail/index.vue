<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect-detail -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：组立不良明细实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-defect-assy-defect-detail">
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
      create-permission="logistics:manufacturing:defect:assydefectdetail:create"
      update-permission="logistics:manufacturing:defect:assydefectdetail:update"
      delete-permission="logistics:manufacturing:defect:assydefectdetail:delete"
      import-permission="logistics:manufacturing:defect:assydefectdetail:import"
      export-permission="logistics:manufacturing:defect:assydefectdetail:export"
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
      :row-key="getAssyDefectDetailId"
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
      <AssyDefectDetailForm
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
      <a-form-item :label="t('entity.assyDefectDetail.assydefectid')">
        <a-input
          v-model:value="advancedQueryForm.assyDefectId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.assydefectid') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.prodordercode') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.linenumber')">
        <a-input
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.linenumber') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.defectcategory')">
        <a-input
          v-model:value="advancedQueryForm.defectCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.defectcategory') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.defectqty')">
        <a-input
          v-model:value="advancedQueryForm.defectQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.defectqty') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.cumulativedefectqty')">
        <a-input
          v-model:value="advancedQueryForm.cumulativeDefectQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.cumulativedefectqty') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.randomcardno')">
        <a-input
          v-model:value="advancedQueryForm.randomCardNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.randomcardno') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.assyDefectDetail.occurrenceengineering')">
        <a-input
          v-model:value="advancedQueryForm.occurrenceEngineering"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefectDetail.occurrenceengineering') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.assyDefectDetail._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assyDefectDetail._self"
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
      :id-column-key="'assyDefectDetailId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 组立不良明细实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/assy-defect-detail
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import AssyDefectDetailForm from './components/assy-defect-detail-form.vue'
import { getAssyDefectDetailList, getAssyDefectDetailById, createAssyDefectDetail, updateAssyDefectDetail, deleteAssyDefectDetailById, deleteAssyDefectDetailBatch, getAssyDefectDetailTemplate, importAssyDefectDetail, exportAssyDefectDetail } from '@/api/logistics/manufacturing/defect/assy-defect-detail'
import type { AssyDefectDetail, AssyDefectDetailQuery, AssyDefectDetailCreate, AssyDefectDetailUpdate } from '@/types/logistics/manufacturing/defect/assy-defect-detail'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktAssyDefectDetail')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assyDefectDetail._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<AssyDefectDetail[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<AssyDefectDetail | null>(null)
const selectedRows = ref<AssyDefectDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<AssyDefectDetail>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  assyDefectId: '',
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectCategory: '',
  defectQty: undefined as number | undefined,
  cumulativeDefectQty: undefined as number | undefined,
  randomCardNo: '',
  occurrenceEngineering: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'assyDefectDetailId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assyDefectDetailId',
    key: 'assyDefectDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'assyDefectDetailId') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.assydefectid'),
    dataIndex: 'assyDefectId',
    key: 'assyDefectId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'assyDefectId') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.assydefectname'),
    dataIndex: 'assyDefectName',
    key: 'assyDefectName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'assyDefectName') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.defectcategory'),
    dataIndex: 'defectCategory',
    key: 'defectCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'defectCategory') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.defectqty'),
    dataIndex: 'defectQty',
    key: 'defectQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'defectQty') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.cumulativedefectqty'),
    dataIndex: 'cumulativeDefectQty',
    key: 'cumulativeDefectQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'cumulativeDefectQty') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.randomcardno'),
    dataIndex: 'randomCardNo',
    key: 'randomCardNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'randomCardNo') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.occurrenceengineering'),
    dataIndex: 'occurrenceEngineering',
    key: 'occurrenceEngineering',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'occurrenceEngineering') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.teststep'),
    dataIndex: 'testStep',
    key: 'testStep',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'testStep') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.defectsymptom'),
    dataIndex: 'defectSymptom',
    key: 'defectSymptom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'defectSymptom') ?? ''
  },
  {
    title: t('entity.assyDefectDetail.defectlocation'),
    dataIndex: 'defectLocation',
    key: 'defectLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectDetailField(record, 'defectLocation') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:defect:assydefectdetail:update',
        onClick: (record: AssyDefectDetail) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:defect:assydefectdetail:delete',
        onClick: (record: AssyDefectDetail) => handleDeleteOne(record)
      }
    ]
  })
])

const getAssyDefectDetailId = (record: any): string => record?.[entityIdName] ?? ''
const getAssyDefectDetailField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: AssyDefectDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: AssyDefectDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssyDefectDetailId(selectedRow.value) === getAssyDefectDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AssyDefectDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: AssyDefectDetail) => ({
  onClick: () => {
    const key = getAssyDefectDetailId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssyDefectDetailId(item)))
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
    const params: AssyDefectDetailQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAssyDefectDetailList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AssyDefectDetail] 加载数据失败', { error })
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
  assyDefectId: '',
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectCategory: '',
  defectQty: undefined as number | undefined,
  cumulativeDefectQty: undefined as number | undefined,
  randomCardNo: '',
  occurrenceEngineering: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.assyDefectDetail._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: AssyDefectDetail) {
  formTitle.value = t('common.page.button.edit') + t('entity.assyDefectDetail._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.assyDefectDetail._self') }))
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
      await updateAssyDefectDetail(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.assyDefectDetail._self') }))
    } else {
      await createAssyDefectDetail(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.assyDefectDetail._self') }))
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
  const res = await getAssyDefectDetailTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssyDefectDetail(file, sheetName)
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
    const exportQuery: AssyDefectDetailQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAssyDefectDetail(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.assyDefectDetail._self') }))
  } catch (error: any) {
    logger.error('[AssyDefectDetail] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.assyDefectDetail._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: AssyDefectDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.assyDefectDetail._self'), name: t('common.tip.this.target', { target: t('entity.assyDefectDetail._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyDefectDetailById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.assyDefectDetail._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.assyDefectDetail._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.assyDefectDetail._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssyDefectDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assyDefectDetail._self') }))
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
  assyDefectId: '',
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectCategory: '',
  defectQty: undefined as number | undefined,
  cumulativeDefectQty: undefined as number | undefined,
  randomCardNo: '',
  occurrenceEngineering: '',
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
.logistics-manufacturing-defect-assy-defect-detail {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
