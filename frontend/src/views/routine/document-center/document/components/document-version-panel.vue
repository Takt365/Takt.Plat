<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：document-version-panel.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制主表实体右侧明细 documentVersion 独立 CRUD（按主表选中 documentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="document-version-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.documentversion._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="routine:document:center:create"
      update-permission="routine:document:center:update"
      delete-permission="routine:document:center:delete"
      import-permission="routine:document:center:import"
      export-permission="routine:document:center:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
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
      @refresh="handleRefresh"
    />
    <div class="document-version-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getDocumentVersionId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="documentVersionId"
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
      <DocumentVersionForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterDocumentId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-routine-document-center-document-document-version"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('versionNo')">
      <a-form-item :label="t('entity.documentversion.versionno')">
        <a-input-number
          v-model:value="advancedQueryForm.versionNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.versionno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('versionNote')">
      <a-form-item :label="t('entity.documentversion.versionnote')">
        <a-textarea
          v-model:value="advancedQueryForm.versionNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.documentversion.versionnote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileId')">
      <a-form-item :label="t('entity.documentversion.fileid')">
        <a-input
          v-model:value="advancedQueryForm.fileId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.fileid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="t('entity.documentversion.filename')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filename') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('filePath')">
      <a-form-item :label="t('entity.documentversion.filepath')">
        <a-input
          v-model:value="advancedQueryForm.filePath"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filepath') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileSize')">
      <a-form-item :label="t('entity.documentversion.filesize')">
        <a-input
          v-model:value="advancedQueryForm.fileSize"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filesize') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileType')">
      <a-form-item :label="t('entity.documentversion.filetype')">
        <a-input
          v-model:value="advancedQueryForm.fileType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filetype') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileExtension')">
      <a-form-item :label="t('entity.documentversion.fileextension')">
        <a-input
          v-model:value="advancedQueryForm.fileExtension"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.fileextension') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedBy')">
      <a-form-item :label="t('entity.documentversion.revisedby')">
        <a-input
          v-model:value="advancedQueryForm.revisedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.revisedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedByName')">
      <a-form-item :label="t('entity.documentversion.revisedbyname')">
        <a-input
          v-model:value="advancedQueryForm.revisedByName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.revisedbyname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedAtStart')">
      <a-form-item :label="t('entity.documentversion.revisedatstart')">
        <a-input
          v-model:value="advancedQueryForm.revisedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.revisedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedAtEnd')">
      <a-form-item :label="t('entity.documentversion.revisedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.revisedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.documentversion.revisedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ t('common.page.entity.extfield') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
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
      :title="t('common.dialog.title.import', { entity: t('entity.documentversion._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.documentversion._self"
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
      id-column-key="documentVersionId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制子表 documentVersion 右栏面板
 * @module views/routine/document-center/document/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import DocumentVersionForm from './document-version-form.vue'
import { useDocumentMasterContext } from '../composables/use-document-master-context'
import {
  getDocumentVersionList,
  getDocumentVersionById,
  createDocumentVersion,
  updateDocumentVersion,
  deleteDocumentVersionById,
  deleteDocumentVersionBatch,
  getDocumentVersionTemplate,
  importDocumentVersion,
  exportDocumentVersion,
} from '@/api/routine/document-center/document-version'
import type { DocumentVersion, DocumentVersionQuery } from '@/types/routine/document-center/document-version'

const { t } = useI18n()
const { selectedMasterRow } = useDocumentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktDocumentVersion')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.documentversion._self') }),
)

const loading = ref(false)
const dataSource = ref<DocumentVersion[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<DocumentVersion | null>(null)
const selectedRows = ref<DocumentVersion[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<DocumentVersion>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  versionNo: undefined as number | undefined,
  versionNote: '',
  fileId: '',
  fileName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  revisedBy: '',
  revisedByName: '',
  revisedAtStart: '',
  revisedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'versionNo', label: t('entity.documentversion.versionno') },
  { key: 'versionNote', label: t('entity.documentversion.versionnote') },
  { key: 'fileId', label: t('entity.documentversion.fileid') },
  { key: 'fileName', label: t('entity.documentversion.filename') },
  { key: 'filePath', label: t('entity.documentversion.filepath') },
  { key: 'fileSize', label: t('entity.documentversion.filesize') },
  { key: 'fileType', label: t('entity.documentversion.filetype') },
  { key: 'fileExtension', label: t('entity.documentversion.fileextension') },
  { key: 'revisedBy', label: t('entity.documentversion.revisedby') },
  { key: 'revisedByName', label: t('entity.documentversion.revisedbyname') },
  { key: 'revisedAtStart', label: t('entity.documentversion.revisedatstart') },
  { key: 'revisedAtEnd', label: t('entity.documentversion.revisedatend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

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
  versionNo: undefined as number | undefined,
  versionNote: '',
  fileId: '',
  fileName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  revisedBy: '',
  revisedByName: '',
  revisedAtStart: '',
  revisedAtEnd: '',
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

const entityIdName = 'documentVersionId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.documentId)
const masterDocumentId = computed(() => selectedMasterRow.value?.documentId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getDocumentVersionId(record: DocumentVersion | Record<string, unknown>): string {
  return String((record as DocumentVersion)?.[entityIdName] ?? '')
}

function getDocumentVersionField(record: DocumentVersion | Record<string, unknown>, field: string): unknown {
  return (record as DocumentVersion)?.[field as keyof DocumentVersion]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'documentVersionId',
    key: 'documentVersionId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'documentVersionId') ?? ''),
  },
  {
    title: t('entity.documentversion.versionno'),
    dataIndex: 'versionNo',
    key: 'versionNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'versionNo') ?? ''),
  },
  {
    title: t('entity.documentversion.versionnote'),
    dataIndex: 'versionNote',
    key: 'versionNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'versionNote') ?? ''),
  },
  {
    title: t('entity.documentversion.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'fileId') ?? ''),
  },
  {
    title: t('entity.documentversion.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'fileName') ?? ''),
  },
  {
    title: t('entity.documentversion.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'filePath') ?? ''),
  },
  {
    title: t('entity.documentversion.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'fileSize') ?? ''),
  },
  {
    title: t('entity.documentversion.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'fileType') ?? ''),
  },
  {
    title: t('entity.documentversion.fileextension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: DocumentVersion }) =>
      String(getDocumentVersionField(record, 'fileExtension') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:document:center:update',
        onClick: (record: DocumentVersion) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:document:center:delete',
        onClick: (record: DocumentVersion) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DocumentVersion[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DocumentVersion, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getDocumentVersionId(selectedRow.value) === getDocumentVersionId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: DocumentVersion[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: DocumentVersion) {
  const key = getDocumentVersionId(record)
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
 * @returns {DocumentVersionQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<DocumentVersionQuery>): DocumentVersionQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: DocumentVersionQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    documentId: masterDocumentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof DocumentVersionQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  if (form.versionNo !== undefined && form.versionNo !== null) {
    query.versionNo = form.versionNo
  }
  assignTrimmed('versionNote', form.versionNote)
  assignTrimmed('fileId', form.fileId)
  assignTrimmed('fileName', form.fileName)
  assignTrimmed('filePath', form.filePath)
  assignTrimmed('fileSize', form.fileSize)
  assignTrimmed('fileType', form.fileType)
  assignTrimmed('fileExtension', form.fileExtension)
  assignTrimmed('revisedBy', form.revisedBy)
  assignTrimmed('revisedByName', form.revisedByName)
  assignTrimmed('revisedAtStart', form.revisedAtStart)
  assignTrimmed('revisedAtEnd', form.revisedAtEnd)
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
    const res = await getDocumentVersionList(buildListQuery())
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
watch(masterDocumentId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.documentversion._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: DocumentVersion) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.documentversion._self') })
  formLoading.value = true
  try {
    const detail = await getDocumentVersionById(getDocumentVersionId(record))
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
      entity: t('entity.documentversion._self'),
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
    const id = formData.value?.documentVersionId
    if (id) {
      await updateDocumentVersion(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.documentversion._self') }))
    } else {
      await createDocumentVersion(payload)
      message.success(t('common.feedback.created', { target: t('entity.documentversion._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: DocumentVersion) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.documentversion._self'),
      name: t('common.tip.this.target', { target: t('entity.documentversion._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDocumentVersionById(getDocumentVersionId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.documentversion._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.documentversion._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.documentversion._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getDocumentVersionId(r)).filter(Boolean)
      await deleteDocumentVersionBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.documentversion._self') }))
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
  const res = await getDocumentVersionTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importDocumentVersion(file, sheetName)
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
    const exportMeta = await exportDocumentVersion(
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
    message.success(t('common.feedback.export.success', { target: t('entity.documentversion._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.documentversion._self') }))
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
