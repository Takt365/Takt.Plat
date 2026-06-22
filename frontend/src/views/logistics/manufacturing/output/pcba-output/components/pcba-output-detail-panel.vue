<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-detail-panel.vue -->
<!-- 功能描述：PCBA日报实体主表实体右侧明细 pcbaOutputDetail 独立 CRUD（按主表选中 pcbaOutputId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="pcba-output-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.pcbaoutputdetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:output:pcbaoutput:create"
      update-permission="logistics:manufacturing:output:pcbaoutput:update"
      delete-permission="logistics:manufacturing:output:pcbaoutput:delete"
      import-permission="logistics:manufacturing:output:pcbaoutput:import"
      export-permission="logistics:manufacturing:output:pcbaoutput:export"
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
    <div class="pcba-output-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPcbaOutputDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="pcbaOutputDetailId"
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
      <PcbaOutputDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPcbaOutputId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-output-pcba-output-pcba-output-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.pcbaoutputdetail.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.prodordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.pcbaoutputdetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timePeriod')">
      <a-form-item :label="t('entity.pcbaoutputdetail.timeperiod')">
        <a-input
          v-model:value="advancedQueryForm.timePeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.timeperiod') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.pcbaoutputdetail.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbBoardType')">
      <a-form-item :label="t('entity.pcbaoutputdetail.pcbboardtype')">
        <a-input
          v-model:value="advancedQueryForm.pcbBoardType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.pcbboardtype') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('panelSide')">
      <a-form-item :label="t('entity.pcbaoutputdetail.panelside')">
        <a-input
          v-model:value="advancedQueryForm.panelSide"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.panelside') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchQty')">
      <a-form-item :label="t('entity.pcbaoutputdetail.batchqty')">
        <a-input-number
          v-model:value="advancedQueryForm.batchQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.batchqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dailyCompletedQty')">
      <a-form-item :label="t('entity.pcbaoutputdetail.dailycompletedqty')">
        <a-input-number
          v-model:value="advancedQueryForm.dailyCompletedQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.dailycompletedqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCompletedQty')">
      <a-form-item :label="t('entity.pcbaoutputdetail.totalcompletedqty')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCompletedQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.totalcompletedqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedStatus')">
      <a-form-item :label="t('entity.pcbaoutputdetail.completedstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.completedStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.completedstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNo')">
      <a-form-item :label="t('entity.pcbaoutputdetail.serialno')">
        <a-input
          v-model:value="advancedQueryForm.serialNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.serialno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectCount')">
      <a-form-item :label="t('entity.pcbaoutputdetail.defectcount')">
        <a-input-number
          v-model:value="advancedQueryForm.defectCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.defectcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inputMinutes')">
      <a-form-item :label="t('entity.pcbaoutputdetail.inputminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.inputMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.inputminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repairMinutes')">
      <a-form-item :label="t('entity.pcbaoutputdetail.repairminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.repairMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.repairminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('switchCount')">
      <a-form-item :label="t('entity.pcbaoutputdetail.switchcount')">
        <a-input-number
          v-model:value="advancedQueryForm.switchCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.switchcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('switchTime')">
      <a-form-item :label="t('entity.pcbaoutputdetail.switchtime')">
        <a-input-number
          v-model:value="advancedQueryForm.switchTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.switchtime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stopTime')">
      <a-form-item :label="t('entity.pcbaoutputdetail.stoptime')">
        <a-input-number
          v-model:value="advancedQueryForm.stopTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.stoptime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalMinutes')">
      <a-form-item :label="t('entity.pcbaoutputdetail.totalminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.totalMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.totalminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedReason')">
      <a-form-item :label="t('entity.pcbaoutputdetail.unachievedreason')">
        <a-input
          v-model:value="advancedQueryForm.unachievedReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.unachievedreason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedDescription')">
      <a-form-item :label="t('entity.pcbaoutputdetail.unachieveddescription')">
        <a-textarea
          v-model:value="advancedQueryForm.unachievedDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.pcbaoutputdetail.unachieveddescription') })"
          :rows="2"
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
      <a-form-item :label="t('entity.pcbaoutputdetail.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.pcbaoutputdetail.extfield') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.pcbaoutputdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.pcbaoutputdetail._self"
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
      id-column-key="pcbaOutputDetailId"
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
 * PCBA日报实体子表 pcbaOutputDetail 右栏面板
 * @module views/logistics/manufacturing/output/pcba-output/components
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
import PcbaOutputDetailForm from './pcba-output-detail-form.vue'
import { usePcbaOutputMasterContext } from '../composables/use-pcba-output-master-context'
import {
  getPcbaOutputDetailList,
  getPcbaOutputDetailById,
  createPcbaOutputDetail,
  updatePcbaOutputDetail,
  deletePcbaOutputDetailById,
  deletePcbaOutputDetailBatch,
  getPcbaOutputDetailTemplate,
  importPcbaOutputDetail,
  exportPcbaOutputDetail,
} from '@/api/logistics/manufacturing/output/pcba-output-detail'
import type { PcbaOutputDetail, PcbaOutputDetailQuery } from '@/types/logistics/manufacturing/output/pcba-output-detail'

const { t } = useI18n()
const { selectedMasterRow } = usePcbaOutputMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPcbaOutputDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.pcbaoutputdetail._self') }),
)

const loading = ref(false)
const dataSource = ref<PcbaOutputDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PcbaOutputDetail | null>(null)
const selectedRows = ref<PcbaOutputDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PcbaOutputDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  timePeriod: '',
  shiftNo: undefined as number | undefined,
  pcbBoardType: '',
  panelSide: '',
  batchQty: undefined as number | undefined,
  dailyCompletedQty: undefined as number | undefined,
  totalCompletedQty: undefined as number | undefined,
  completedStatus: undefined as number | undefined,
  serialNo: '',
  defectCount: undefined as number | undefined,
  inputMinutes: undefined as number | undefined,
  repairMinutes: undefined as number | undefined,
  switchCount: undefined as number | undefined,
  switchTime: undefined as number | undefined,
  stopTime: undefined as number | undefined,
  totalMinutes: undefined as number | undefined,
  unachievedReason: '',
  unachievedDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'prodOrderCode', label: t('entity.pcbaoutputdetail.prodordercode') },
  { key: 'lineNumber', label: t('entity.pcbaoutputdetail.linenumber') },
  { key: 'timePeriod', label: t('entity.pcbaoutputdetail.timeperiod') },
  { key: 'shiftNo', label: t('entity.pcbaoutputdetail.shiftno') },
  { key: 'pcbBoardType', label: t('entity.pcbaoutputdetail.pcbboardtype') },
  { key: 'panelSide', label: t('entity.pcbaoutputdetail.panelside') },
  { key: 'batchQty', label: t('entity.pcbaoutputdetail.batchqty') },
  { key: 'dailyCompletedQty', label: t('entity.pcbaoutputdetail.dailycompletedqty') },
  { key: 'totalCompletedQty', label: t('entity.pcbaoutputdetail.totalcompletedqty') },
  { key: 'completedStatus', label: t('entity.pcbaoutputdetail.completedstatus') },
  { key: 'serialNo', label: t('entity.pcbaoutputdetail.serialno') },
  { key: 'defectCount', label: t('entity.pcbaoutputdetail.defectcount') },
  { key: 'inputMinutes', label: t('entity.pcbaoutputdetail.inputminutes') },
  { key: 'repairMinutes', label: t('entity.pcbaoutputdetail.repairminutes') },
  { key: 'switchCount', label: t('entity.pcbaoutputdetail.switchcount') },
  { key: 'switchTime', label: t('entity.pcbaoutputdetail.switchtime') },
  { key: 'stopTime', label: t('entity.pcbaoutputdetail.stoptime') },
  { key: 'totalMinutes', label: t('entity.pcbaoutputdetail.totalminutes') },
  { key: 'unachievedReason', label: t('entity.pcbaoutputdetail.unachievedreason') },
  { key: 'unachievedDescription', label: t('entity.pcbaoutputdetail.unachieveddescription') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.pcbaoutputdetail.extfield') },
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
  shiftNo: undefined as number | undefined,
  pcbBoardType: '',
  panelSide: '',
  batchQty: undefined as number | undefined,
  dailyCompletedQty: undefined as number | undefined,
  totalCompletedQty: undefined as number | undefined,
  completedStatus: undefined as number | undefined,
  serialNo: '',
  defectCount: undefined as number | undefined,
  inputMinutes: undefined as number | undefined,
  repairMinutes: undefined as number | undefined,
  switchCount: undefined as number | undefined,
  switchTime: undefined as number | undefined,
  stopTime: undefined as number | undefined,
  totalMinutes: undefined as number | undefined,
  unachievedReason: '',
  unachievedDescription: '',
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

const entityIdName = 'pcbaOutputDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.pcbaOutputId)
const masterPcbaOutputId = computed(() => selectedMasterRow.value?.pcbaOutputId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPcbaOutputDetailId(record: PcbaOutputDetail | Record<string, unknown>): string {
  return String((record as PcbaOutputDetail)?.[entityIdName] ?? '')
}

function getPcbaOutputDetailField(record: PcbaOutputDetail | Record<string, unknown>, field: string): unknown {
  return (record as PcbaOutputDetail)?.[field as keyof PcbaOutputDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'pcbaOutputDetailId',
    key: 'pcbaOutputDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'pcbaOutputDetailId') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'timePeriod') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'shiftNo') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.pcbboardtype'),
    dataIndex: 'pcbBoardType',
    key: 'pcbBoardType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'pcbBoardType') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.panelside'),
    dataIndex: 'panelSide',
    key: 'panelSide',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'panelSide') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.batchqty'),
    dataIndex: 'batchQty',
    key: 'batchQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'batchQty') ?? ''),
  },
  {
    title: t('entity.pcbaoutputdetail.dailycompletedqty'),
    dataIndex: 'dailyCompletedQty',
    key: 'dailyCompletedQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'dailyCompletedQty') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:update',
        onClick: (record: PcbaOutputDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:delete',
        onClick: (record: PcbaOutputDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PcbaOutputDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PcbaOutputDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPcbaOutputDetailId(selectedRow.value) === getPcbaOutputDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PcbaOutputDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PcbaOutputDetail) {
  const key = getPcbaOutputDetailId(record)
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
 * @returns {PcbaOutputDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PcbaOutputDetailQuery>): PcbaOutputDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PcbaOutputDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    pcbaOutputId: masterPcbaOutputId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PcbaOutputDetailQuery, value: string | undefined) => {
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
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  assignTrimmed('pcbBoardType', form.pcbBoardType)
  assignTrimmed('panelSide', form.panelSide)
  if (form.batchQty !== undefined && form.batchQty !== null) {
    query.batchQty = form.batchQty
  }
  if (form.dailyCompletedQty !== undefined && form.dailyCompletedQty !== null) {
    query.dailyCompletedQty = form.dailyCompletedQty
  }
  if (form.totalCompletedQty !== undefined && form.totalCompletedQty !== null) {
    query.totalCompletedQty = form.totalCompletedQty
  }
  if (form.completedStatus !== undefined && form.completedStatus !== null) {
    query.completedStatus = form.completedStatus
  }
  assignTrimmed('serialNo', form.serialNo)
  if (form.defectCount !== undefined && form.defectCount !== null) {
    query.defectCount = form.defectCount
  }
  if (form.inputMinutes !== undefined && form.inputMinutes !== null) {
    query.inputMinutes = form.inputMinutes
  }
  if (form.repairMinutes !== undefined && form.repairMinutes !== null) {
    query.repairMinutes = form.repairMinutes
  }
  if (form.switchCount !== undefined && form.switchCount !== null) {
    query.switchCount = form.switchCount
  }
  if (form.switchTime !== undefined && form.switchTime !== null) {
    query.switchTime = form.switchTime
  }
  if (form.stopTime !== undefined && form.stopTime !== null) {
    query.stopTime = form.stopTime
  }
  if (form.totalMinutes !== undefined && form.totalMinutes !== null) {
    query.totalMinutes = form.totalMinutes
  }
  assignTrimmed('unachievedReason', form.unachievedReason)
  assignTrimmed('unachievedDescription', form.unachievedDescription)
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
    const res = await getPcbaOutputDetailList(buildListQuery())
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
watch(masterPcbaOutputId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.pcbaoutputdetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PcbaOutputDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.pcbaoutputdetail._self') })
  formLoading.value = true
  try {
    const detail = await getPcbaOutputDetailById(getPcbaOutputDetailId(record))
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
      entity: t('entity.pcbaoutputdetail._self'),
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
    const id = formData.value?.pcbaOutputDetailId
    if (id) {
      await updatePcbaOutputDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.pcbaoutputdetail._self') }))
    } else {
      await createPcbaOutputDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.pcbaoutputdetail._self') }))
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

async function handleDeleteOne(record: PcbaOutputDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.pcbaoutputdetail._self'),
      name: t('common.tip.this.target', { target: t('entity.pcbaoutputdetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePcbaOutputDetailById(getPcbaOutputDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaoutputdetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.pcbaoutputdetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.pcbaoutputdetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPcbaOutputDetailId(r)).filter(Boolean)
      await deletePcbaOutputDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaoutputdetail._self') }))
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
  const res = await getPcbaOutputDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPcbaOutputDetail(file, sheetName)
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
    const exportMeta = await exportPcbaOutputDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.pcbaoutputdetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.pcbaoutputdetail._self') }))
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
