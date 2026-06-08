<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-onboarding -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：入职待办管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-personnel-employee-onboarding">
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
      create-permission="humanresource:personnel:employeeonboarding:create"
      update-permission="humanresource:personnel:employeeonboarding:update"
      delete-permission="humanresource:personnel:employeeonboarding:delete"
      import-permission="humanresource:personnel:employeeonboarding:import"
      export-permission="humanresource:personnel:employeeonboarding:export"
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
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'employeeOnboardingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEmployeeOnboardingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

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
      <EmployeeOnboardingForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-human-resource-personnel-employee-onboarding'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('offerId')">
      <a-form-item :label="t('entity.employeeOnboarding.offerid')">
        <a-input
          v-model:value="advancedQueryForm.offerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.offerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('todoNo')">
      <a-form-item :label="t('entity.employeeOnboarding.todono')">
        <a-input
          v-model:value="advancedQueryForm.todoNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.todono') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('todoStatus')">
      <a-form-item :label="t('entity.employeeOnboarding.todostatus')">
        <a-input-number
          v-model:value="advancedQueryForm.todoStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.todostatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedJoinedDateStart')">
      <a-form-item :label="t('entity.employeeOnboarding.plannedjoineddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedJoinedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeOnboarding.plannedjoineddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedJoinedDateEnd')">
      <a-form-item :label="t('entity.employeeOnboarding.plannedjoineddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedJoinedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeOnboarding.plannedjoineddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('candidateName')">
      <a-form-item :label="t('entity.employeeOnboarding.candidatename')">
        <a-date-picker
          v-model:value="advancedQueryForm.candidateName"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeOnboarding.candidatename') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mobile')">
      <a-form-item :label="t('entity.employeeOnboarding.mobile')">
        <a-input
          v-model:value="advancedQueryForm.mobile"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.mobile') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.employeeOnboarding.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeJoinedId')">
      <a-form-item :label="t('entity.employeeOnboarding.employeejoinedid')">
        <a-input
          v-model:value="advancedQueryForm.employeeJoinedId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.employeejoinedid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.employeeOnboarding.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboarding.reason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extFieldJson')">
      <a-form-item :label="t('common.page.entity.extfieldjson')">
        <a-input
          v-model:value="advancedQueryForm.extFieldJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.employeeOnboarding._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeeOnboarding._self"
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
      :id-column-key="'employeeOnboardingId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 入职待办管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/personnel/employee-onboarding
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EmployeeOnboardingForm from './components/employee-onboarding-form.vue'
import { getEmployeeOnboardingList, getEmployeeOnboardingById, createEmployeeOnboarding, updateEmployeeOnboarding, deleteEmployeeOnboardingById, deleteEmployeeOnboardingBatch, getEmployeeOnboardingTemplate, importEmployeeOnboarding, exportEmployeeOnboarding } from '@/api/human-resource/personnel/employee-onboarding'
import type { EmployeeOnboarding, EmployeeOnboardingQuery, EmployeeOnboardingCreate, EmployeeOnboardingUpdate } from '@/types/human-resource/personnel/employee-onboarding'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktEmployeeOnboarding')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeeOnboarding._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<EmployeeOnboarding[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<EmployeeOnboarding | null>(null)
const selectedRows = ref<EmployeeOnboarding[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EmployeeOnboarding>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'offerId', label: t('entity.employeeOnboarding.offerid') },
  { key: 'todoNo', label: t('entity.employeeOnboarding.todono') },
  { key: 'todoStatus', label: t('entity.employeeOnboarding.todostatus') },
  { key: 'plannedJoinedDateStart', label: t('entity.employeeOnboarding.plannedjoineddatestart') },
  { key: 'plannedJoinedDateEnd', label: t('entity.employeeOnboarding.plannedjoineddateend') },
  { key: 'candidateName', label: t('entity.employeeOnboarding.candidatename') },
  { key: 'mobile', label: t('entity.employeeOnboarding.mobile') },
  { key: 'employeeId', label: t('entity.employeeOnboarding.employeeid') },
  { key: 'employeeJoinedId', label: t('entity.employeeOnboarding.employeejoinedid') },
  { key: 'reason', label: t('entity.employeeOnboarding.reason') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
const visibleQueryFieldKeys = ref<string[]>([])
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'employeeOnboardingId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeOnboardingId',
    key: 'employeeOnboardingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeOnboardingId') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.offerid'),
    dataIndex: 'offerId',
    key: 'offerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'offerId') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.offername'),
    dataIndex: 'offerName',
    key: 'offerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'offerName') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.todono'),
    dataIndex: 'todoNo',
    key: 'todoNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'todoNo') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.todostatus'),
    dataIndex: 'todoStatus',
    key: 'todoStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'todoStatus') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.plannedjoineddate'),
    dataIndex: 'plannedJoinedDate',
    key: 'plannedJoinedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'plannedJoinedDate') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.candidatename'),
    dataIndex: 'candidateName',
    key: 'candidateName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'candidateName') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.mobile'),
    dataIndex: 'mobile',
    key: 'mobile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'mobile') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.employeejoinedid'),
    dataIndex: 'employeeJoinedId',
    key: 'employeeJoinedId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeJoinedId') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.employeejoinedname'),
    dataIndex: 'employeeJoinedName',
    key: 'employeeJoinedName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeJoinedName') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'reason') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.offer'),
    dataIndex: 'offer',
    key: 'offer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'offer') ?? ''
  },
  {
    title: t('entity.employeeOnboarding.employeejoined'),
    dataIndex: 'employeeJoined',
    key: 'employeeJoined',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeJoined') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:personnel:employeeonboarding:update',
        onClick: (record: EmployeeOnboarding) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:personnel:employeeonboarding:delete',
        onClick: (record: EmployeeOnboarding) => handleDeleteOne(record)
      }
    ]
  })
])

const getEmployeeOnboardingId = (record: any): string => record?.[entityIdName] ?? ''
const getEmployeeOnboardingField = (record: any, field: string): any => record?.[field]

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EmployeeOnboarding[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeOnboarding, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEmployeeOnboardingId(selectedRow.value) === getEmployeeOnboardingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeOnboarding[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: EmployeeOnboarding) => ({
  onClick: () => {
    const key = getEmployeeOnboardingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeOnboardingId(item)))
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
    const params: EmployeeOnboardingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEmployeeOnboardingList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EmployeeOnboarding] 加载数据失败', { error })
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
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeeOnboarding._self') })
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: EmployeeOnboarding) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeeOnboarding._self') })
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeeOnboarding._self') }))
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
      await updateEmployeeOnboarding(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employeeOnboarding._self') }))
    } else {
      await createEmployeeOnboarding(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeeOnboarding._self') }))
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
  const res = await getEmployeeOnboardingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployeeOnboarding(file, sheetName)
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
    const exportQuery: EmployeeOnboardingQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEmployeeOnboarding(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeeOnboarding._self') }))
  } catch (error: any) {
    logger.error('[EmployeeOnboarding] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeeOnboarding._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: EmployeeOnboarding) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeeOnboarding._self'), name: t('common.tip.this.target', { target: t('entity.employeeOnboarding._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeOnboardingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeeOnboarding._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeeOnboarding._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeeOnboarding._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeOnboardingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeeOnboarding._self') }))
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
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
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
.human-resource-personnel-employee-onboarding {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
