<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/master-demand-schedule/components -->
<!-- 文件名称：master-demand-schedule-line-panel.vue -->
<!-- 功能描述：主需求计划 MDS 头表主表实体右侧明细 masterDemandScheduleLine 独立 CRUD（按主表选中 masterDemandScheduleId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="master-demand-schedule-line-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.masterdemandscheduleline._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:planning:master:demand:schedule:create"
      update-permission="logistics:manufacturing:planning:master:demand:schedule:update"
      delete-permission="logistics:manufacturing:planning:master:demand:schedule:delete"
      import-permission="logistics:manufacturing:planning:master:demand:schedule:import"
      export-permission="logistics:manufacturing:planning:master:demand:schedule:export"
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
    <div class="master-demand-schedule-line-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMasterDemandScheduleLineId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="masterDemandScheduleLineId"
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
      <MasterDemandScheduleLineForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMasterDemandScheduleId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-planning-master-demand-schedule-master-demand-schedule-line"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('mdsCode')">
      <a-form-item :label="t('entity.masterdemandscheduleline.mdscode')">
        <a-input
          v-model:value="advancedQueryForm.mdsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.mdscode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('demandSourceType')">
      <a-form-item :label="t('entity.masterdemandscheduleline.demandsourcetype')">
        <TaktSelect
          v-model:value="advancedQueryForm.demandSourceType"
          dict-type="mds_demand_source_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.demandsourcetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesOrderId')">
      <a-form-item :label="t('entity.masterdemandscheduleline.salesorderid')">
        <a-input
          v-model:value="advancedQueryForm.salesOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesOrderLineNumber')">
      <a-form-item :label="t('entity.masterdemandscheduleline.salesorderlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.salesOrderLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesorderlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPlanId')">
      <a-form-item :label="t('entity.masterdemandscheduleline.salesplanid')">
        <a-input
          v-model:value="advancedQueryForm.salesPlanId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesplanid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPlanLineNumber')">
      <a-form-item :label="t('entity.masterdemandscheduleline.salesplanlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.salesPlanLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesplanlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.masterdemandscheduleline.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bucketStartStart')">
      <a-form-item :label="t('entity.masterdemandscheduleline.bucketstartstart')">
        <a-input
          v-model:value="advancedQueryForm.bucketStartStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.bucketstartstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bucketStartEnd')">
      <a-form-item :label="t('entity.masterdemandscheduleline.bucketstartend')">
        <a-input
          v-model:value="advancedQueryForm.bucketStartEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.bucketstartend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bucketEndStart')">
      <a-form-item :label="t('entity.masterdemandscheduleline.bucketendstart')">
        <a-input
          v-model:value="advancedQueryForm.bucketEndStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.bucketendstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bucketEndEnd')">
      <a-form-item :label="t('entity.masterdemandscheduleline.bucketendend')">
        <a-input
          v-model:value="advancedQueryForm.bucketEndEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.bucketendend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('demandQuantity')">
      <a-form-item :label="t('entity.masterdemandscheduleline.demandquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.demandQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.demandquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitOfMeasure')">
      <a-form-item :label="t('entity.masterdemandscheduleline.unitofmeasure')">
        <a-input
          v-model:value="advancedQueryForm.unitOfMeasure"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.unitofmeasure') })"
          show-count
          :maxlength="5"
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
      :title="t('common.dialog.title.import', { entity: t('entity.masterdemandscheduleline._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.masterdemandscheduleline._self"
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
      id-column-key="masterDemandScheduleLineId"
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
 * 主需求计划 MDS 头表子表 masterDemandScheduleLine 右栏面板
 * @module views/logistics/manufacturing/planning/master-demand-schedule/components
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
import MasterDemandScheduleLineForm from './master-demand-schedule-line-form.vue'
import { useMasterDemandScheduleMasterContext } from '../composables/use-master-demand-schedule-master-context'
import {
  getMasterDemandScheduleLineList,
  getMasterDemandScheduleLineById,
  createMasterDemandScheduleLine,
  updateMasterDemandScheduleLine,
  deleteMasterDemandScheduleLineById,
  deleteMasterDemandScheduleLineBatch,
  getMasterDemandScheduleLineTemplate,
  importMasterDemandScheduleLine,
  exportMasterDemandScheduleLine,
} from '@/api/logistics/manufacturing/planning/master-demand-schedule-line'
import type { MasterDemandScheduleLine, MasterDemandScheduleLineQuery } from '@/types/logistics/manufacturing/planning/master-demand-schedule-line'

const { t } = useI18n()
const { selectedMasterRow } = useMasterDemandScheduleMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMasterDemandScheduleLine')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.masterdemandscheduleline._self') }),
)

const loading = ref(false)
const dataSource = ref<MasterDemandScheduleLine[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MasterDemandScheduleLine | null>(null)
const selectedRows = ref<MasterDemandScheduleLine[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MasterDemandScheduleLine>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  mdsCode: '',
  demandSourceType: undefined as number | undefined,
  salesOrderId: '',
  salesOrderLineNumber: undefined as number | undefined,
  salesPlanId: '',
  salesPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  bucketStartStart: '',
  bucketStartEnd: '',
  bucketEndStart: '',
  bucketEndEnd: '',
  demandQuantity: undefined as number | undefined,
  unitOfMeasure: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'mdsCode', label: t('entity.masterdemandscheduleline.mdscode') },
  { key: 'demandSourceType', label: t('entity.masterdemandscheduleline.demandsourcetype') },
  { key: 'salesOrderId', label: t('entity.masterdemandscheduleline.salesorderid') },
  { key: 'salesOrderLineNumber', label: t('entity.masterdemandscheduleline.salesorderlinenumber') },
  { key: 'salesPlanId', label: t('entity.masterdemandscheduleline.salesplanid') },
  { key: 'salesPlanLineNumber', label: t('entity.masterdemandscheduleline.salesplanlinenumber') },
  { key: 'materialCode', label: t('entity.masterdemandscheduleline.materialcode') },
  { key: 'bucketStartStart', label: t('entity.masterdemandscheduleline.bucketstartstart') },
  { key: 'bucketStartEnd', label: t('entity.masterdemandscheduleline.bucketstartend') },
  { key: 'bucketEndStart', label: t('entity.masterdemandscheduleline.bucketendstart') },
  { key: 'bucketEndEnd', label: t('entity.masterdemandscheduleline.bucketendend') },
  { key: 'demandQuantity', label: t('entity.masterdemandscheduleline.demandquantity') },
  { key: 'unitOfMeasure', label: t('entity.masterdemandscheduleline.unitofmeasure') },
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
  mdsCode: '',
  demandSourceType: undefined as number | undefined,
  salesOrderId: '',
  salesOrderLineNumber: undefined as number | undefined,
  salesPlanId: '',
  salesPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  bucketStartStart: '',
  bucketStartEnd: '',
  bucketEndStart: '',
  bucketEndEnd: '',
  demandQuantity: undefined as number | undefined,
  unitOfMeasure: '',
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

const entityIdName = 'masterDemandScheduleLineId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.masterDemandScheduleId)
const masterMasterDemandScheduleId = computed(() => selectedMasterRow.value?.masterDemandScheduleId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMasterDemandScheduleLineId(record: MasterDemandScheduleLine | Record<string, unknown>): string {
  return String((record as MasterDemandScheduleLine)?.[entityIdName] ?? '')
}

function getMasterDemandScheduleLineField(record: MasterDemandScheduleLine | Record<string, unknown>, field: string): unknown {
  return (record as MasterDemandScheduleLine)?.[field as keyof MasterDemandScheduleLine]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'masterDemandScheduleLineId',
    key: 'masterDemandScheduleLineId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'masterDemandScheduleLineId') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.mdscode'),
    dataIndex: 'mdsCode',
    key: 'mdsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'mdsCode') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.demandsourcetype'),
    dataIndex: 'demandSourceType',
    key: 'demandSourceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'demandSourceType') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.salesorderid'),
    dataIndex: 'salesOrderId',
    key: 'salesOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'salesOrderId') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.salesorderlinenumber'),
    dataIndex: 'salesOrderLineNumber',
    key: 'salesOrderLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'salesOrderLineNumber') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.salesplanid'),
    dataIndex: 'salesPlanId',
    key: 'salesPlanId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'salesPlanId') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.salesplanlinenumber'),
    dataIndex: 'salesPlanLineNumber',
    key: 'salesPlanLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'salesPlanLineNumber') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.masterdemandscheduleline.bucketstart'),
    dataIndex: 'bucketStart',
    key: 'bucketStart',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'bucketStart') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:planning:master:demand:schedule:update',
        onClick: (record: MasterDemandScheduleLine) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:planning:master:demand:schedule:delete',
        onClick: (record: MasterDemandScheduleLine) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MasterDemandScheduleLine[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MasterDemandScheduleLine, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMasterDemandScheduleLineId(selectedRow.value) === getMasterDemandScheduleLineId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MasterDemandScheduleLine[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MasterDemandScheduleLine) {
  const key = getMasterDemandScheduleLineId(record)
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
 * @returns {MasterDemandScheduleLineQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MasterDemandScheduleLineQuery>): MasterDemandScheduleLineQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MasterDemandScheduleLineQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    masterDemandScheduleId: masterMasterDemandScheduleId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MasterDemandScheduleLineQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('mdsCode', form.mdsCode)
  if (form.demandSourceType !== undefined && form.demandSourceType !== null) {
    query.demandSourceType = form.demandSourceType
  }
  assignTrimmed('salesOrderId', form.salesOrderId)
  if (form.salesOrderLineNumber !== undefined && form.salesOrderLineNumber !== null) {
    query.salesOrderLineNumber = form.salesOrderLineNumber
  }
  assignTrimmed('salesPlanId', form.salesPlanId)
  if (form.salesPlanLineNumber !== undefined && form.salesPlanLineNumber !== null) {
    query.salesPlanLineNumber = form.salesPlanLineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('bucketStartStart', form.bucketStartStart)
  assignTrimmed('bucketStartEnd', form.bucketStartEnd)
  assignTrimmed('bucketEndStart', form.bucketEndStart)
  assignTrimmed('bucketEndEnd', form.bucketEndEnd)
  if (form.demandQuantity !== undefined && form.demandQuantity !== null) {
    query.demandQuantity = form.demandQuantity
  }
  assignTrimmed('unitOfMeasure', form.unitOfMeasure)
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
    const res = await getMasterDemandScheduleLineList(buildListQuery())
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
watch(masterMasterDemandScheduleId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.masterdemandscheduleline._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MasterDemandScheduleLine) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.masterdemandscheduleline._self') })
  formLoading.value = true
  try {
    const detail = await getMasterDemandScheduleLineById(getMasterDemandScheduleLineId(record))
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
      entity: t('entity.masterdemandscheduleline._self'),
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
    const id = formData.value?.masterDemandScheduleLineId
    if (id) {
      await updateMasterDemandScheduleLine(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.masterdemandscheduleline._self') }))
    } else {
      await createMasterDemandScheduleLine(payload)
      message.success(t('common.feedback.created', { target: t('entity.masterdemandscheduleline._self') }))
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

async function handleDeleteOne(record: MasterDemandScheduleLine) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.masterdemandscheduleline._self'),
      name: t('common.tip.this.target', { target: t('entity.masterdemandscheduleline._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMasterDemandScheduleLineById(getMasterDemandScheduleLineId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.masterdemandscheduleline._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.masterdemandscheduleline._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.masterdemandscheduleline._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMasterDemandScheduleLineId(r)).filter(Boolean)
      await deleteMasterDemandScheduleLineBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.masterdemandscheduleline._self') }))
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
  const res = await getMasterDemandScheduleLineTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMasterDemandScheduleLine(file, sheetName)
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
    const exportMeta = await exportMasterDemandScheduleLine(
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
    message.success(t('common.feedback.export.success', { target: t('entity.masterdemandscheduleline._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.masterdemandscheduleline._self') }))
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
