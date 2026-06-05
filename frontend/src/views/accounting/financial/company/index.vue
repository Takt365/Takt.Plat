<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/financial/company -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公司实体 代表租户下的独立公司/工厂管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-company">
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
      create-permission="accounting:financial:company:create"
      update-permission="accounting:financial:company:update"
      delete-permission="accounting:financial:company:delete"
      import-permission="accounting:financial:company:import"
      export-permission="accounting:financial:company:export"
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
      :row-key="getCompanyId"
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
      <CompanyForm
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
      <a-form-item :label="t('entity.company.name')">
        <a-input
          v-model:value="advancedQueryForm.companyName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.name') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.shortname')">
        <a-input
          v-model:value="advancedQueryForm.companyShortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.shortname') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.type')">
        <a-input
          v-model:value="advancedQueryForm.companyType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.type') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.enterprisenature')">
        <a-input
          v-model:value="advancedQueryForm.enterpriseNature"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisenature') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.industryattribute')">
        <a-input
          v-model:value="advancedQueryForm.industryAttribute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.industryattribute') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.enterprisescale')">
        <a-input
          v-model:value="advancedQueryForm.enterpriseScale"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisescale') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.businessscope')">
        <a-input
          v-model:value="advancedQueryForm.businessScope"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businessscope') })"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.company.registrationaddress1')">
        <a-input
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationaddress1') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.company._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.company._self"
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
      :id-column-key="'companyId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 公司实体 代表租户下的独立公司/工厂管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/company
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { mergeDefaultColumns } from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'
import CompanyForm from './components/company-form.vue'
import { getCompanyList, getCompanyById, createCompany, updateCompany, deleteCompanyById, deleteCompanyBatch, getCompanyTemplate, importCompany, exportCompany } from '@/api/accounting/financial/company'
import type { Company, CompanyQuery, CompanyCreate, CompanyUpdate } from '@/types/accounting/financial/company'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktCompany')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.company._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Company[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Company | null>(null)
const selectedRows = ref<Company[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Company>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
})
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'companyId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'companyId',
    key: 'companyId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyId') ?? ''
  },
  {
    title: t('entity.company.name'),
    dataIndex: 'companyName',
    key: 'companyName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyName') ?? ''
  },
  {
    title: t('entity.company.shortname'),
    dataIndex: 'companyShortName',
    key: 'companyShortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyShortName') ?? ''
  },
  {
    title: t('entity.company.type'),
    dataIndex: 'companyType',
    key: 'companyType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyType') ?? ''
  },
  {
    title: t('entity.company.enterprisenature'),
    dataIndex: 'enterpriseNature',
    key: 'enterpriseNature',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'enterpriseNature') ?? ''
  },
  {
    title: t('entity.company.industryattribute'),
    dataIndex: 'industryAttribute',
    key: 'industryAttribute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'industryAttribute') ?? ''
  },
  {
    title: t('entity.company.enterprisescale'),
    dataIndex: 'enterpriseScale',
    key: 'enterpriseScale',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'enterpriseScale') ?? ''
  },
  {
    title: t('entity.company.businessscope'),
    dataIndex: 'businessScope',
    key: 'businessScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessScope') ?? ''
  },
  {
    title: t('entity.company.registrationaddress1'),
    dataIndex: 'registrationAddress1',
    key: 'registrationAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress1') ?? ''
  },
  {
    title: t('entity.company.registrationaddress2'),
    dataIndex: 'registrationAddress2',
    key: 'registrationAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress2') ?? ''
  },
  {
    title: t('entity.company.registrationaddress3'),
    dataIndex: 'registrationAddress3',
    key: 'registrationAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress3') ?? ''
  },
  {
    title: t('entity.company.registrationregion'),
    dataIndex: 'registrationRegion',
    key: 'registrationRegion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationRegion') ?? ''
  },
  {
    title: t('entity.company.registrationprovince'),
    dataIndex: 'registrationProvince',
    key: 'registrationProvince',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationProvince') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:company:update',
        onClick: (record: Company) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:company:delete',
        onClick: (record: Company) => handleDeleteOne(record)
      }
    ]
  })
])

const getCompanyId = (record: any): string => record?.[entityIdName] ?? ''
const getCompanyField = (record: any, field: string): any => record?.[field]

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
  onChange: (keys: (string | number)[], rows: Company[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Company, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCompanyId(selectedRow.value) === getCompanyId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Company[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Company) => ({
  onClick: () => {
    const key = getCompanyId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCompanyId(item)))
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
    const params: CompanyQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCompanyList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Company] 加载数据失败', { error })
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
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.page.button.create') + t('entity.company._self')
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Company) {
  formTitle.value = t('common.page.button.edit') + t('entity.company._self')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.company._self') }))
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
      await updateCompany(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.company._self') }))
    } else {
      await createCompany(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.company._self') }))
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
  const res = await getCompanyTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCompany(file, sheetName)
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
    const exportQuery: CompanyQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCompany(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.company._self') }))
  } catch (error: any) {
    logger.error('[Company] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.company._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Company) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.company._self'), name: t('common.tip.this.target', { target: t('entity.company._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCompanyById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.company._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.company._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.company._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCompanyBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.company._self') }))
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
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
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
.accounting-financial-company {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
