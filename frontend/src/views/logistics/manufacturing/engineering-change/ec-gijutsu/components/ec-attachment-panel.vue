<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-attachment-panel.vue -->
<!-- 功能描述：设变主表实体右侧明细 ecAttachment 独立 CRUD（按主表选中 ecId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ec-attachment-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.ecattachment._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:engineering:change:gijutsu:create"
      update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
      delete-permission="logistics:manufacturing:engineering:change:gijutsu:delete"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <div class="ec-attachment-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getEcAttachmentId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecAttachmentId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <EcAttachmentForm
        :key="formData?.ecAttachmentId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :master-id="masterEcId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-ec-attachment-ec-attachment"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ecattachment.ecno')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.ecno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ecattachment.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachmentType')">
      <a-form-item :label="t('entity.ecattachment.attachmenttype')">
        <a-input
          v-model:value="advancedQueryForm.attachmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.attachmenttype') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('docNo')">
      <a-form-item :label="t('entity.ecattachment.docno')">
        <a-input
          v-model:value="advancedQueryForm.docNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.docno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="t('entity.ecattachment.filename')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.filename') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accessUrl')">
      <a-form-item :label="t('entity.ecattachment.accessurl')">
        <a-input
          v-model:value="advancedQueryForm.accessUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.accessurl') })"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('extField')">
      <a-form-item :label="t('common.page.entity.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.ecattachment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecattachment._self"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecAttachmentId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变子表 ecAttachment 右栏面板
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { ref, computed, watch, nextTick } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import EcAttachmentForm from './ec-attachment-form.vue'
import { useEcMasterContext } from '../composables/use-ec-master-context'
import {
  getEcAttachmentList,
  getEcAttachmentById,
  createEcAttachment,
  updateEcAttachment,
  deleteEcAttachmentById,
  deleteEcAttachmentBatch,
  getEcAttachmentTemplate,
  importEcAttachment,
  exportEcAttachment,
} from '@/api/logistics/manufacturing/engineering-change/ec-attachment'
import type { EcAttachment, EcAttachmentQuery } from '@/types/logistics/manufacturing/engineering-change/ec-attachment'

const { t } = useI18n()
const { selectedMasterRow } = useEcMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcAttachment')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ecattachment._self') }),
)

const loading = ref(false)
const dataSource = ref<EcAttachment[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<EcAttachment | null>(null)
const selectedRows = ref<EcAttachment[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EcAttachment>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ecNo: '',
  lineNumber: undefined as number | undefined,
  attachmentType: '',
  docNo: '',
  fileName: '',
  accessUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ecNo', label: t('entity.ecattachment.ecno') },
  { key: 'lineNumber', label: t('entity.ecattachment.linenumber') },
  { key: 'attachmentType', label: t('entity.ecattachment.attachmenttype') },
  { key: 'docNo', label: t('entity.ecattachment.docno') },
  { key: 'fileName', label: t('entity.ecattachment.filename') },
  { key: 'accessUrl', label: t('entity.ecattachment.accessurl') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  ecNo: '',
  lineNumber: undefined as number | undefined,
  attachmentType: '',
  docNo: '',
  fileName: '',
  accessUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'ecAttachmentId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ecGijutsuId)
const masterEcId = computed(() => selectedMasterRow.value?.ecGijutsuId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getEcAttachmentId(record: EcAttachment | Record<string, unknown>): string {
  return String((record as EcAttachment)?.[entityIdName] ?? '')
}

function getEcAttachmentField(record: EcAttachment | Record<string, unknown>, field: string): unknown {
  return (record as EcAttachment)?.[field as keyof EcAttachment]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecAttachmentId',
    key: 'ecAttachmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'ecAttachmentId') ?? ''),
  },
  {
    title: t('entity.ecattachment.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'ecNo') ?? ''),
  },
  {
    title: t('entity.ecattachment.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.ecattachment.attachmenttype'),
    dataIndex: 'attachmentType',
    key: 'attachmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'attachmentType') ?? ''),
  },
  {
    title: t('entity.ecattachment.docno'),
    dataIndex: 'docNo',
    key: 'docNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'docNo') ?? ''),
  },
  {
    title: t('entity.ecattachment.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'fileName') ?? ''),
  },
  {
    title: t('entity.ecattachment.accessurl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'accessUrl') ?? ''),
  },
  {
    title: t('entity.ecattachment.ec'),
    dataIndex: 'ec',
    key: 'ec',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'ec') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
        onClick: (record: EcAttachment) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:delete',
        onClick: (record: EcAttachment) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcAttachment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcAttachment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (
      selectedRow.value != null
      && getEcAttachmentId(selectedRow.value) === getEcAttachmentId(record)
    ) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcAttachment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EcAttachment) {
  const key = getEcAttachmentId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcAttachmentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcAttachmentQuery>): EcAttachmentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcAttachmentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ecId: masterEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcAttachmentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecNo', form.ecNo)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('attachmentType', form.attachmentType)
  assignTrimmed('docNo', form.docNo)
  assignTrimmed('fileName', form.fileName)
  assignTrimmed('accessUrl', form.accessUrl)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getEcAttachmentList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterEcId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ecattachment._self') })
  formData.value = {
    ecNo: String(selectedMasterRow.value?.ecNo ?? ''),
  }
  formVisible.value = true
}

async function handleEdit(record: EcAttachment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecattachment._self') })
  formLoading.value = true
  try {
    const detail = await getEcAttachmentById(getEcAttachmentId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.ecattachment._self'),
    }))
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
    const payload = refInst.getValues?.()
    const id = formData.value?.ecAttachmentId
    if (id) {
      await updateEcAttachment(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.ecattachment._self') }))
    } else {
      await createEcAttachment(payload)
      message.success(t('common.feedback.created', { target: t('entity.ecattachment._self') }))
    }
    formVisible.value = false
    formData.value = {}
    nextTick(() => formRef.value?.resetFields?.())
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
  formData.value = {}
  nextTick(() => formRef.value?.resetFields?.())
}

async function handleDeleteOne(record: EcAttachment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ecattachment._self'),
      name: t('common.tip.this.target', { target: t('entity.ecattachment._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcAttachmentById(getEcAttachmentId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.ecattachment._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ecattachment._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ecattachment._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getEcAttachmentId(r)).filter(Boolean)
      await deleteEcAttachmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ecattachment._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEcAttachmentTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcAttachment(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportEcAttachment(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.ecattachment._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.ecattachment._self') }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
