<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：assy-defect-detail-panel.vue -->
<!-- 功能描述：组立不良日报实体主表实体右侧明细 assyDefectDetail 独立 CRUD（按主表选中 assyDefectId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="assy-defect-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.assydefectdetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:defect:assydefect:create"
      update-permission="logistics:manufacturing:defect:assydefect:update"
      delete-permission="logistics:manufacturing:defect:assydefect:delete"
      import-permission="logistics:manufacturing:defect:assydefect:import"
      export-permission="logistics:manufacturing:defect:assydefect:export"
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
    <div class="assy-defect-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getAssyDefectDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="assyDefectDetailId"
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
      <AssyDefectDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterAssyDefectId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-defect-assy-defect-assy-defect-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.assydefectdetail.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.prodordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.assydefectdetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectCategory')">
      <a-form-item :label="t('entity.assydefectdetail.defectcategory')">
        <a-input
          v-model:value="advancedQueryForm.defectCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectcategory') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectQty')">
      <a-form-item :label="t('entity.assydefectdetail.defectqty')">
        <a-input-number
          v-model:value="advancedQueryForm.defectQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cumulativeDefectQty')">
      <a-form-item :label="t('entity.assydefectdetail.cumulativedefectqty')">
        <a-input-number
          v-model:value="advancedQueryForm.cumulativeDefectQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.cumulativedefectqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('randomCardNo')">
      <a-form-item :label="t('entity.assydefectdetail.randomcardno')">
        <a-input
          v-model:value="advancedQueryForm.randomCardNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.randomcardno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('occurrenceEngineering')">
      <a-form-item :label="t('entity.assydefectdetail.occurrenceengineering')">
        <a-input
          v-model:value="advancedQueryForm.occurrenceEngineering"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.occurrenceengineering') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('testStep')">
      <a-form-item :label="t('entity.assydefectdetail.teststep')">
        <a-input
          v-model:value="advancedQueryForm.testStep"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.teststep') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectSymptom')">
      <a-form-item :label="t('entity.assydefectdetail.defectsymptom')">
        <a-input
          v-model:value="advancedQueryForm.defectSymptom"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectsymptom') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectLocation')">
      <a-form-item :label="t('entity.assydefectdetail.defectlocation')">
        <a-input
          v-model:value="advancedQueryForm.defectLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectlocation') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectReason')">
      <a-form-item :label="t('entity.assydefectdetail.defectreason')">
        <a-input
          v-model:value="advancedQueryForm.defectReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectreason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repairOperator')">
      <a-form-item :label="t('entity.assydefectdetail.repairoperator')">
        <a-input
          v-model:value="advancedQueryForm.repairOperator"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.repairoperator') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.assydefectdetail.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assydefectdetail.extfield') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.assydefectdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assydefectdetail._self"
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
      id-column-key="assyDefectDetailId"
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
 * 组立不良日报实体子表 assyDefectDetail 右栏面板
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import AssyDefectDetailForm from './assy-defect-detail-form.vue'
import { useAssyDefectMasterContext } from '../composables/use-assy-defect-master-context'
import {
  getAssyDefectDetailList,
  getAssyDefectDetailById,
  createAssyDefectDetail,
  updateAssyDefectDetail,
  deleteAssyDefectDetailById,
  deleteAssyDefectDetailBatch,
  getAssyDefectDetailTemplate,
  importAssyDefectDetail,
  exportAssyDefectDetail,
} from '@/api/logistics/manufacturing/defect/assy-defect-detail'
import type { AssyDefectDetail, AssyDefectDetailQuery } from '@/types/logistics/manufacturing/defect/assy-defect-detail'

const { t } = useI18n()
const { selectedMasterRow } = useAssyDefectMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyDefectDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assydefectdetail._self') }),
)

const loading = ref(false)
const dataSource = ref<AssyDefectDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
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
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectCategory: '',
  defectQty: undefined as number | undefined,
  cumulativeDefectQty: undefined as number | undefined,
  randomCardNo: '',
  occurrenceEngineering: '',
  testStep: '',
  defectSymptom: '',
  defectLocation: '',
  defectReason: '',
  repairOperator: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'prodOrderCode', label: t('entity.assydefectdetail.prodordercode') },
  { key: 'lineNumber', label: t('entity.assydefectdetail.linenumber') },
  { key: 'defectCategory', label: t('entity.assydefectdetail.defectcategory') },
  { key: 'defectQty', label: t('entity.assydefectdetail.defectqty') },
  { key: 'cumulativeDefectQty', label: t('entity.assydefectdetail.cumulativedefectqty') },
  { key: 'randomCardNo', label: t('entity.assydefectdetail.randomcardno') },
  { key: 'occurrenceEngineering', label: t('entity.assydefectdetail.occurrenceengineering') },
  { key: 'testStep', label: t('entity.assydefectdetail.teststep') },
  { key: 'defectSymptom', label: t('entity.assydefectdetail.defectsymptom') },
  { key: 'defectLocation', label: t('entity.assydefectdetail.defectlocation') },
  { key: 'defectReason', label: t('entity.assydefectdetail.defectreason') },
  { key: 'repairOperator', label: t('entity.assydefectdetail.repairoperator') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.assydefectdetail.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

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
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectCategory: '',
  defectQty: undefined as number | undefined,
  cumulativeDefectQty: undefined as number | undefined,
  randomCardNo: '',
  occurrenceEngineering: '',
  testStep: '',
  defectSymptom: '',
  defectLocation: '',
  defectReason: '',
  repairOperator: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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

const entityIdName = 'assyDefectDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.assyDefectId)
const masterAssyDefectId = computed(() => selectedMasterRow.value?.assyDefectId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getAssyDefectDetailId(record: AssyDefectDetail | Record<string, unknown>): string {
  return String((record as AssyDefectDetail)?.[entityIdName] ?? '')
}

function getAssyDefectDetailField(record: AssyDefectDetail | Record<string, unknown>, field: string): unknown {
  return (record as AssyDefectDetail)?.[field as keyof AssyDefectDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assyDefectDetailId',
    key: 'assyDefectDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'assyDefectDetailId') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.defectcategory'),
    dataIndex: 'defectCategory',
    key: 'defectCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'defectCategory') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.defectqty'),
    dataIndex: 'defectQty',
    key: 'defectQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'defectQty') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.cumulativedefectqty'),
    dataIndex: 'cumulativeDefectQty',
    key: 'cumulativeDefectQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'cumulativeDefectQty') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.randomcardno'),
    dataIndex: 'randomCardNo',
    key: 'randomCardNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'randomCardNo') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.occurrenceengineering'),
    dataIndex: 'occurrenceEngineering',
    key: 'occurrenceEngineering',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'occurrenceEngineering') ?? ''),
  },
  {
    title: t('entity.assydefectdetail.teststep'),
    dataIndex: 'testStep',
    key: 'testStep',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyDefectDetail }) =>
      String(getAssyDefectDetailField(record, 'testStep') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:defect:assydefect:update',
        onClick: (record: AssyDefectDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:defect:assydefect:delete',
        onClick: (record: AssyDefectDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

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
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: AssyDefectDetail) {
  const key = getAssyDefectDetailId(record)
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
 * @returns {AssyDefectDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssyDefectDetailQuery>): AssyDefectDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssyDefectDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    assyDefectId: masterAssyDefectId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssyDefectDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('prodOrderCode', form.prodOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('defectCategory', form.defectCategory)
  if (form.defectQty !== undefined && form.defectQty !== null) {
    query.defectQty = form.defectQty
  }
  if (form.cumulativeDefectQty !== undefined && form.cumulativeDefectQty !== null) {
    query.cumulativeDefectQty = form.cumulativeDefectQty
  }
  assignTrimmed('randomCardNo', form.randomCardNo)
  assignTrimmed('occurrenceEngineering', form.occurrenceEngineering)
  assignTrimmed('testStep', form.testStep)
  assignTrimmed('defectSymptom', form.defectSymptom)
  assignTrimmed('defectLocation', form.defectLocation)
  assignTrimmed('defectReason', form.defectReason)
  assignTrimmed('repairOperator', form.repairOperator)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
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
    const res = await getAssyDefectDetailList(buildListQuery())
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
watch(masterAssyDefectId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assydefectdetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: AssyDefectDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assydefectdetail._self') })
  formLoading.value = true
  try {
    const detail = await getAssyDefectDetailById(getAssyDefectDetailId(record))
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
      entity: t('entity.assydefectdetail._self'),
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
    const id = formData.value?.assyDefectDetailId
    if (id) {
      await updateAssyDefectDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.assydefectdetail._self') }))
    } else {
      await createAssyDefectDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.assydefectdetail._self') }))
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

async function handleDeleteOne(record: AssyDefectDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.assydefectdetail._self'),
      name: t('common.tip.this.target', { target: t('entity.assydefectdetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyDefectDetailById(getAssyDefectDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.assydefectdetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.assydefectdetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.assydefectdetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getAssyDefectDetailId(r)).filter(Boolean)
      await deleteAssyDefectDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assydefectdetail._self') }))
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
  const res = await getAssyDefectDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssyDefectDetail(file, sheetName)
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
    const exportMeta = await exportAssyDefectDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.assydefectdetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.assydefectdetail._self') }))
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
