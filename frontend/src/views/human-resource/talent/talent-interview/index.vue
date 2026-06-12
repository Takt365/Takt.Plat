<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/talent-interview -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：面试安排管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-talent-talent-interview">
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
      create-permission="humanresource:talent:talentinterview:create"
      update-permission="humanresource:talent:talentinterview:update"
      delete-permission="humanresource:talent:talentinterview:delete"
      import-permission="humanresource:talent:talentinterview:import"
      export-permission="humanresource:talent:talentinterview:export"
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
      :id-column-key="'talentInterviewId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTalentInterviewId"
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
      <TalentInterviewForm
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
      :storage-key="'takt-query-fields-human-resource-talent-talent-interview'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('jobPostingId')">
      <a-form-item :label="t('entity.talentInterview.jobpostingid')">
        <a-input
          v-model:value="advancedQueryForm.jobPostingId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.jobpostingid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewNo')">
      <a-form-item :label="t('entity.talentInterview.interviewno')">
        <a-input
          v-model:value="advancedQueryForm.interviewNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.interviewno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewStatus')">
      <a-form-item :label="t('entity.talentInterview.interviewstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.interviewStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.interviewstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewRound')">
      <a-form-item :label="t('entity.talentInterview.interviewround')">
        <a-input-number
          v-model:value="advancedQueryForm.interviewRound"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.interviewround') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateStart')">
      <a-form-item :label="t('entity.talentInterview.interviewdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentInterview.interviewdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateEnd')">
      <a-form-item :label="t('entity.talentInterview.interviewdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentInterview.interviewdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewerName')">
      <a-form-item :label="t('entity.talentInterview.interviewername')">
        <a-input
          v-model:value="advancedQueryForm.interviewerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.interviewername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('candidateName')">
      <a-form-item :label="t('entity.talentInterview.candidatename')">
        <a-date-picker
          v-model:value="advancedQueryForm.candidateName"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentInterview.candidatename') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mobile')">
      <a-form-item :label="t('entity.talentInterview.mobile')">
        <a-input
          v-model:value="advancedQueryForm.mobile"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.mobile') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('email')">
      <a-form-item :label="t('entity.talentInterview.email')">
        <a-input
          v-model:value="advancedQueryForm.email"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.email') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewLocation')">
      <a-form-item :label="t('entity.talentInterview.interviewlocation')">
        <a-input
          v-model:value="advancedQueryForm.interviewLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.interviewlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.talentInterview.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentInterview.reason') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.talentInterview._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.talentInterview._self"
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
      :id-column-key="'talentInterviewId'"
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
 * 面试安排管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/talent/talent-interview
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import TalentInterviewForm from './components/talent-interview-form.vue'
import { getTalentInterviewList, getTalentInterviewById, createTalentInterview, updateTalentInterview, deleteTalentInterviewById, deleteTalentInterviewBatch, getTalentInterviewTemplate, importTalentInterview, exportTalentInterview } from '@/api/human-resource/talent/talent-interview'
import type { TalentInterview, TalentInterviewQuery, TalentInterviewCreate, TalentInterviewUpdate } from '@/types/human-resource/talent/talent-interview'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktTalentInterview')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.talentInterview._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<TalentInterview[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<TalentInterview | null>(null)
const selectedRows = ref<TalentInterview[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<TalentInterview>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  jobPostingId: '',
  interviewNo: '',
  interviewStatus: undefined as number | undefined,
  interviewRound: undefined as number | undefined,
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewerName: '',
  candidateName: '',
  mobile: '',
  email: '',
  interviewLocation: '',
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'jobPostingId', label: t('entity.talentInterview.jobpostingid') },
  { key: 'interviewNo', label: t('entity.talentInterview.interviewno') },
  { key: 'interviewStatus', label: t('entity.talentInterview.interviewstatus') },
  { key: 'interviewRound', label: t('entity.talentInterview.interviewround') },
  { key: 'interviewDateStart', label: t('entity.talentInterview.interviewdatestart') },
  { key: 'interviewDateEnd', label: t('entity.talentInterview.interviewdateend') },
  { key: 'interviewerName', label: t('entity.talentInterview.interviewername') },
  { key: 'candidateName', label: t('entity.talentInterview.candidatename') },
  { key: 'mobile', label: t('entity.talentInterview.mobile') },
  { key: 'email', label: t('entity.talentInterview.email') },
  { key: 'interviewLocation', label: t('entity.talentInterview.interviewlocation') },
  { key: 'reason', label: t('entity.talentInterview.reason') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
const visibleQueryFieldKeys = ref<string[]>([])
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'talentInterviewId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'talentInterviewId',
    key: 'talentInterviewId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'talentInterviewId') ?? ''
  },
  {
    title: t('entity.talentInterview.jobpostingid'),
    dataIndex: 'jobPostingId',
    key: 'jobPostingId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'jobPostingId') ?? ''
  },
  {
    title: t('entity.talentInterview.jobpostingname'),
    dataIndex: 'jobPostingName',
    key: 'jobPostingName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'jobPostingName') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewno'),
    dataIndex: 'interviewNo',
    key: 'interviewNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewNo') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewstatus'),
    dataIndex: 'interviewStatus',
    key: 'interviewStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewStatus') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewround'),
    dataIndex: 'interviewRound',
    key: 'interviewRound',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewRound') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewdate'),
    dataIndex: 'interviewDate',
    key: 'interviewDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewDate') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewername'),
    dataIndex: 'interviewerName',
    key: 'interviewerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewerName') ?? ''
  },
  {
    title: t('entity.talentInterview.candidatename'),
    dataIndex: 'candidateName',
    key: 'candidateName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'candidateName') ?? ''
  },
  {
    title: t('entity.talentInterview.mobile'),
    dataIndex: 'mobile',
    key: 'mobile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'mobile') ?? ''
  },
  {
    title: t('entity.talentInterview.email'),
    dataIndex: 'email',
    key: 'email',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'email') ?? ''
  },
  {
    title: t('entity.talentInterview.interviewlocation'),
    dataIndex: 'interviewLocation',
    key: 'interviewLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'interviewLocation') ?? ''
  },
  {
    title: t('entity.talentInterview.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'reason') ?? ''
  },
  {
    title: t('entity.talentInterview.jobposting'),
    dataIndex: 'jobPosting',
    key: 'jobPosting',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'jobPosting') ?? ''
  },
  {
    title: t('entity.talentInterview.talentoffers'),
    dataIndex: 'talentOffers',
    key: 'talentOffers',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentInterviewField(record, 'talentOffers') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:talent:talentinterview:update',
        onClick: (record: TalentInterview) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:talent:talentinterview:delete',
        onClick: (record: TalentInterview) => handleDeleteOne(record)
      }
    ]
  })
])

const getTalentInterviewId = (record: any): string => record?.[entityIdName] ?? ''
const getTalentInterviewField = (record: any, field: string): any => record?.[field]

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TalentInterview[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TalentInterview, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getTalentInterviewId(selectedRow.value) === getTalentInterviewId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TalentInterview[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: TalentInterview) => ({
  onClick: () => {
    const key = getTalentInterviewId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTalentInterviewId(item)))
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
    const params: TalentInterviewQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getTalentInterviewList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TalentInterview] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  jobPostingId: '',
  interviewNo: '',
  interviewStatus: undefined as number | undefined,
  interviewRound: undefined as number | undefined,
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewerName: '',
  candidateName: '',
  mobile: '',
  email: '',
  interviewLocation: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.talentInterview._self') })
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: TalentInterview) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.talentInterview._self') })
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.talentInterview._self') }))
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
      await updateTalentInterview(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.talentInterview._self') }))
    } else {
      await createTalentInterview(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.talentInterview._self') }))
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
  const res = await getTalentInterviewTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTalentInterview(file, sheetName)
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
    const exportQuery: TalentInterviewQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportTalentInterview(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.talentInterview._self') }))
  } catch (error: any) {
    logger.error('[TalentInterview] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.talentInterview._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: TalentInterview) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.talentInterview._self'), name: t('common.tip.this.target', { target: t('entity.talentInterview._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTalentInterviewById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.talentInterview._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.talentInterview._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.talentInterview._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTalentInterviewBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.talentInterview._self') }))
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
  jobPostingId: '',
  interviewNo: '',
  interviewStatus: undefined as number | undefined,
  interviewRound: undefined as number | undefined,
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewerName: '',
  candidateName: '',
  mobile: '',
  email: '',
  interviewLocation: '',
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
.human-resource-talent-talent-interview {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
