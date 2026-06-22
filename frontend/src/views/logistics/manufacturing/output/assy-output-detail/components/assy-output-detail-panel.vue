<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output-detail/components -->
<!-- 文件名称：assy-output-detail-panel.vue -->
<!-- 功能描述：组立日报主表实体右侧明细 assyOutputDetail 独立 CRUD（按主表选中 assyOutputId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="assy-output-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.assyoutputdetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:output:assyoutput:create"
      update-permission="logistics:manufacturing:output:assyoutput:update"
      delete-permission="logistics:manufacturing:output:assyoutput:delete"
      import-permission="logistics:manufacturing:output:assyoutput:import"
      export-permission="logistics:manufacturing:output:assyoutput:export"
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
    <div class="assy-output-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getAssyOutputDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="assyOutputDetailId"
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
      <AssyOutputDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterAssyOutputId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-output-assy-output-detail-assy-output-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.assyoutputdetail.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.assyoutputdetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timePeriod')">
      <a-form-item :label="t('entity.assyoutputdetail.timeperiod')">
        <a-input
          v-model:value="advancedQueryForm.timePeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.timeperiod') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodActualQty')">
      <a-form-item :label="t('entity.assyoutputdetail.prodactualqty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodActualQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodactualqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeMinutes')">
      <a-form-item :label="t('entity.assyoutputdetail.downtimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.downtimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.downtimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReason')">
      <a-form-item :label="t('entity.assyoutputdetail.downtimereason')">
        <a-input
          v-model:value="advancedQueryForm.downtimeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.downtimereason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeDescription')">
      <a-form-item :label="t('entity.assyoutputdetail.downtimedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.downtimeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyoutputdetail.downtimedescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedReason')">
      <a-form-item :label="t('entity.assyoutputdetail.unachievedreason')">
        <a-input
          v-model:value="advancedQueryForm.unachievedReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.unachievedreason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedDescription')">
      <a-form-item :label="t('entity.assyoutputdetail.unachieveddescription')">
        <a-textarea
          v-model:value="advancedQueryForm.unachievedDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyoutputdetail.unachieveddescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inputMinutes')">
      <a-form-item :label="t('entity.assyoutputdetail.inputminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.inputMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.inputminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodMinutes')">
      <a-form-item :label="t('entity.assyoutputdetail.prodminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.prodMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualMinutes')">
      <a-form-item :label="t('entity.assyoutputdetail.actualminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.actualMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.actualminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('achievementRate')">
      <a-form-item :label="t('entity.assyoutputdetail.achievementrate')">
        <a-input-number
          v-model:value="advancedQueryForm.achievementRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.achievementrate') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.assyoutputdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assyoutputdetail._self"
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
      id-column-key="assyOutputDetailId"
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
 * 组立日报子表 assyOutputDetail 右栏面板
 * @module views/logistics/manufacturing/output/assy-output-detail/components
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
import AssyOutputDetailForm from './assy-output-detail-form.vue'
import { useAssyOutputMasterContext } from '../composables/use-assy-output-master-context'
import {
  getAssyOutputDetailList,
  getAssyOutputDetailById,
  createAssyOutputDetail,
  updateAssyOutputDetail,
  deleteAssyOutputDetailById,
  deleteAssyOutputDetailBatch,
  getAssyOutputDetailTemplate,
  importAssyOutputDetail,
  exportAssyOutputDetail,
} from '@/api/logistics/manufacturing/output/assy-output-detail'
import type { AssyOutputDetail, AssyOutputDetailQuery } from '@/types/logistics/manufacturing/output/assy-output-detail'

const { t } = useI18n()
const { selectedMasterRow } = useAssyOutputMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyOutputDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assyoutputdetail._self') }),
)

const loading = ref(false)
const dataSource = ref<AssyOutputDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<AssyOutputDetail | null>(null)
const selectedRows = ref<AssyOutputDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<AssyOutputDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  timePeriod: '',
  prodActualQty: undefined as number | undefined,
  downtimeMinutes: undefined as number | undefined,
  downtimeReason: '',
  downtimeDescription: '',
  unachievedReason: '',
  unachievedDescription: '',
  inputMinutes: undefined as number | undefined,
  prodMinutes: undefined as number | undefined,
  actualMinutes: undefined as number | undefined,
  achievementRate: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'prodOrderCode', label: t('entity.assyoutputdetail.prodordercode') },
  { key: 'lineNumber', label: t('entity.assyoutputdetail.linenumber') },
  { key: 'timePeriod', label: t('entity.assyoutputdetail.timeperiod') },
  { key: 'prodActualQty', label: t('entity.assyoutputdetail.prodactualqty') },
  { key: 'downtimeMinutes', label: t('entity.assyoutputdetail.downtimeminutes') },
  { key: 'downtimeReason', label: t('entity.assyoutputdetail.downtimereason') },
  { key: 'downtimeDescription', label: t('entity.assyoutputdetail.downtimedescription') },
  { key: 'unachievedReason', label: t('entity.assyoutputdetail.unachievedreason') },
  { key: 'unachievedDescription', label: t('entity.assyoutputdetail.unachieveddescription') },
  { key: 'inputMinutes', label: t('entity.assyoutputdetail.inputminutes') },
  { key: 'prodMinutes', label: t('entity.assyoutputdetail.prodminutes') },
  { key: 'actualMinutes', label: t('entity.assyoutputdetail.actualminutes') },
  { key: 'achievementRate', label: t('entity.assyoutputdetail.achievementrate') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
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
  timePeriod: '',
  prodActualQty: undefined as number | undefined,
  downtimeMinutes: undefined as number | undefined,
  downtimeReason: '',
  downtimeDescription: '',
  unachievedReason: '',
  unachievedDescription: '',
  inputMinutes: undefined as number | undefined,
  prodMinutes: undefined as number | undefined,
  actualMinutes: undefined as number | undefined,
  achievementRate: undefined as number | undefined,
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

const entityIdName = 'assyOutputDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.assyOutputId)
const masterAssyOutputId = computed(() => selectedMasterRow.value?.assyOutputId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getAssyOutputDetailId(record: AssyOutputDetail | Record<string, unknown>): string {
  return String((record as AssyOutputDetail)?.[entityIdName] ?? '')
}

function getAssyOutputDetailField(record: AssyOutputDetail | Record<string, unknown>, field: string): unknown {
  return (record as AssyOutputDetail)?.[field as keyof AssyOutputDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assyOutputDetailId',
    key: 'assyOutputDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'assyOutputDetailId') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'timePeriod') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.prodactualqty'),
    dataIndex: 'prodActualQty',
    key: 'prodActualQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'prodActualQty') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.downtimeminutes'),
    dataIndex: 'downtimeMinutes',
    key: 'downtimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeMinutes') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.downtimereason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeReason') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.downtimedescription'),
    dataIndex: 'downtimeDescription',
    key: 'downtimeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeDescription') ?? ''),
  },
  {
    title: t('entity.assyoutputdetail.unachievedreason'),
    dataIndex: 'unachievedReason',
    key: 'unachievedReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'unachievedReason') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:assyoutput:update',
        onClick: (record: AssyOutputDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:assyoutput:delete',
        onClick: (record: AssyOutputDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyOutputDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: AssyOutputDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssyOutputDetailId(selectedRow.value) === getAssyOutputDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AssyOutputDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: AssyOutputDetail) {
  const key = getAssyOutputDetailId(record)
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
 * @returns {AssyOutputDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssyOutputDetailQuery>): AssyOutputDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssyOutputDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    assyOutputId: masterAssyOutputId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssyOutputDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('prodOrderCode', form.prodOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('timePeriod', form.timePeriod)
  if (form.prodActualQty !== undefined && form.prodActualQty !== null) {
    query.prodActualQty = form.prodActualQty
  }
  if (form.downtimeMinutes !== undefined && form.downtimeMinutes !== null) {
    query.downtimeMinutes = form.downtimeMinutes
  }
  assignTrimmed('downtimeReason', form.downtimeReason)
  assignTrimmed('downtimeDescription', form.downtimeDescription)
  assignTrimmed('unachievedReason', form.unachievedReason)
  assignTrimmed('unachievedDescription', form.unachievedDescription)
  if (form.inputMinutes !== undefined && form.inputMinutes !== null) {
    query.inputMinutes = form.inputMinutes
  }
  if (form.prodMinutes !== undefined && form.prodMinutes !== null) {
    query.prodMinutes = form.prodMinutes
  }
  if (form.actualMinutes !== undefined && form.actualMinutes !== null) {
    query.actualMinutes = form.actualMinutes
  }
  if (form.achievementRate !== undefined && form.achievementRate !== null) {
    query.achievementRate = form.achievementRate
  }
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
    const res = await getAssyOutputDetailList(buildListQuery())
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
watch(masterAssyOutputId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assyoutputdetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: AssyOutputDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assyoutputdetail._self') })
  formLoading.value = true
  try {
    const detail = await getAssyOutputDetailById(getAssyOutputDetailId(record))
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
      entity: t('entity.assyoutputdetail._self'),
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
    const id = formData.value?.assyOutputDetailId
    if (id) {
      await updateAssyOutputDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.assyoutputdetail._self') }))
    } else {
      await createAssyOutputDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.assyoutputdetail._self') }))
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

async function handleDeleteOne(record: AssyOutputDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.assyoutputdetail._self'),
      name: t('common.tip.this.target', { target: t('entity.assyoutputdetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyOutputDetailById(getAssyOutputDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.assyoutputdetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.assyoutputdetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.assyoutputdetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getAssyOutputDetailId(r)).filter(Boolean)
      await deleteAssyOutputDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assyoutputdetail._self') }))
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
  const res = await getAssyOutputDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssyOutputDetail(file, sheetName)
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
    const exportMeta = await exportAssyOutputDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.assyoutputdetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.assyoutputdetail._self') }))
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
