<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order/components -->
<!-- 文件名称：iqc-order-item-panel.vue -->
<!-- 功能描述：IQC进货检验单实体主表实体右侧明细 iqcOrderItem 独立 CRUD（按主表选中 defectHandlings 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="iqc-order-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.iqcorderitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:iqc:order:create"
      update-permission="logistics:quality:operation:iqc:order:update"
      delete-permission="logistics:quality:operation:iqc:order:delete"
      import-permission="logistics:quality:operation:iqc:order:import"
      export-permission="logistics:quality:operation:iqc:order:export"
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
    <div class="iqc-order-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getIqcOrderItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="iqcOrderItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
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
      <IqcOrderItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterIqcOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-iqc-order-iqc-order-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('iqcOrderCode')">
      <a-form-item :label="t('entity.iqcorderitem.iqcordercode')">
        <a-input
          v-model:value="advancedQueryForm.iqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.iqcordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.iqcorderitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.iqcorderitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.iqcorderitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.iqcorderitem.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.batchno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseQuantity')">
      <a-form-item :label="t('entity.iqcorderitem.purchasequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.purchaseQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.purchasequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardCode')">
      <a-form-item :label="t('entity.iqcorderitem.standardcode')">
        <a-input
          v-model:value="advancedQueryForm.standardCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.standardcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeCode')">
      <a-form-item :label="t('entity.iqcorderitem.samplingschemecode')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.samplingschemecode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionMethod')">
      <a-form-item :label="t('entity.iqcorderitem.inspectionmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectionmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleQuantity')">
      <a-form-item :label="t('entity.iqcorderitem.samplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.sampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.samplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.iqcorderitem.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unqualifiedQuantity')">
      <a-form-item :label="t('entity.iqcorderitem.unqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.unqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.unqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionReturnQuantity')">
      <a-form-item :label="t('entity.iqcorderitem.inspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleSerialNo')">
      <a-form-item :label="t('entity.iqcorderitem.sampleserialno')">
        <a-input
          v-model:value="advancedQueryForm.sampleSerialNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.sampleserialno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDescription')">
      <a-form-item :label="t('entity.iqcorderitem.inspectiondescription')">
        <a-textarea
          v-model:value="advancedQueryForm.inspectionDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcorderitem.inspectiondescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectorBy')">
      <a-form-item :label="t('entity.iqcorderitem.inspectorby')">
        <a-input
          v-model:value="advancedQueryForm.inspectorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectorby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.iqcorderitem.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.iqcorderitem.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.iqcorderitem.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.judgestatus') })"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.iqcorderitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.iqcorderitem._self"
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
      id-column-key="iqcOrderItemId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * IQC进货检验单实体子表 iqcOrderItem 右栏面板
 * @module views/logistics/quality/operation/iqc-order/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import IqcOrderItemForm from './iqc-order-item-form.vue'
import { useIqcOrderMasterContext } from '../composables/use-iqc-order-master-context'
import {
  getIqcOrderItemList,
  getIqcOrderItemById,
  createIqcOrderItem,
  updateIqcOrderItem,
  deleteIqcOrderItemById,
  deleteIqcOrderItemBatch,
  getIqcOrderItemTemplate,
  importIqcOrderItem,
  exportIqcOrderItem,
} from '@/api/logistics/quality/operation/iqc-order-item'
import type { IqcOrderItem, IqcOrderItemQuery } from '@/types/logistics/quality/operation/iqc-order-item'

const { t } = useI18n()
const { selectedMasterRow } = useIqcOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIqcOrderItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.iqcorderitem._self') }),
)

const loading = ref(false)
const dataSource = ref<IqcOrderItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<IqcOrderItem | null>(null)
const selectedRows = ref<IqcOrderItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<IqcOrderItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  iqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  batchNo: '',
  purchaseQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  sampleSerialNo: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  judgeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'iqcOrderCode', label: t('entity.iqcorderitem.iqcordercode') },
  { key: 'lineNumber', label: t('entity.iqcorderitem.linenumber') },
  { key: 'materialCode', label: t('entity.iqcorderitem.materialcode') },
  { key: 'materialName', label: t('entity.iqcorderitem.materialname') },
  { key: 'batchNo', label: t('entity.iqcorderitem.batchno') },
  { key: 'purchaseQuantity', label: t('entity.iqcorderitem.purchasequantity') },
  { key: 'standardCode', label: t('entity.iqcorderitem.standardcode') },
  { key: 'samplingSchemeCode', label: t('entity.iqcorderitem.samplingschemecode') },
  { key: 'inspectionMethod', label: t('entity.iqcorderitem.inspectionmethod') },
  { key: 'sampleQuantity', label: t('entity.iqcorderitem.samplequantity') },
  { key: 'qualifiedQuantity', label: t('entity.iqcorderitem.qualifiedquantity') },
  { key: 'unqualifiedQuantity', label: t('entity.iqcorderitem.unqualifiedquantity') },
  { key: 'inspectionReturnQuantity', label: t('entity.iqcorderitem.inspectionreturnquantity') },
  { key: 'sampleSerialNo', label: t('entity.iqcorderitem.sampleserialno') },
  { key: 'inspectionDescription', label: t('entity.iqcorderitem.inspectiondescription') },
  { key: 'inspectorBy', label: t('entity.iqcorderitem.inspectorby') },
  { key: 'inspectionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.iqcorderitem.inspectiondate')) },
  { key: 'inspectionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.iqcorderitem.inspectiondate')) },
  { key: 'judgeStatus', label: t('entity.iqcorderitem.judgestatus') },
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
  iqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  batchNo: '',
  purchaseQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  sampleSerialNo: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  judgeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
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

const entityIdName = 'iqcOrderItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.iqcOrderId)
const masterIqcOrderId = computed(() => selectedMasterRow.value?.iqcOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getIqcOrderItemId(record: IqcOrderItem | Record<string, unknown>): string {
  return String((record as IqcOrderItem)?.[entityIdName] ?? '')
}

function getIqcOrderItemField(record: IqcOrderItem | Record<string, unknown>, field: string): unknown {
  return (record as IqcOrderItem)?.[field as keyof IqcOrderItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'iqcOrderItemId',
    key: 'iqcOrderItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'iqcOrderItemId') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.iqcordercode'),
    dataIndex: 'iqcOrderCode',
    key: 'iqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'iqcOrderCode') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'batchNo') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.purchasequantity'),
    dataIndex: 'purchaseQuantity',
    key: 'purchaseQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'purchaseQuantity') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'standardCode') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.samplingschemecode'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'samplingSchemeCode') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.inspectionmethod'),
    dataIndex: 'inspectionMethod',
    key: 'inspectionMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'inspectionMethod') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.samplequantity'),
    dataIndex: 'sampleQuantity',
    key: 'sampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'sampleQuantity') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.qualifiedquantity'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'qualifiedQuantity') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.unqualifiedquantity'),
    dataIndex: 'unqualifiedQuantity',
    key: 'unqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'unqualifiedQuantity') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.inspectionreturnquantity'),
    dataIndex: 'inspectionReturnQuantity',
    key: 'inspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'inspectionReturnQuantity') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.sampleserialno'),
    dataIndex: 'sampleSerialNo',
    key: 'sampleSerialNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'sampleSerialNo') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.inspectiondescription'),
    dataIndex: 'inspectionDescription',
    key: 'inspectionDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'inspectionDescription') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.inspectorby'),
    dataIndex: 'inspectorBy',
    key: 'inspectorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'inspectorBy') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'inspectionDate') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'judgeStatus') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.order'),
    dataIndex: 'order',
    key: 'order',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'order') ?? ''),
  },
  {
    title: t('entity.iqcorderitem.defecthandlings'),
    dataIndex: 'defectHandlings',
    key: 'defectHandlings',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IqcOrderItem }) =>
      String(getIqcOrderItemField(record, 'defectHandlings') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:iqc:order:update',
        onClick: (record: IqcOrderItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:iqc:order:delete',
        onClick: (record: IqcOrderItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IqcOrderItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IqcOrderItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getIqcOrderItemId(selectedRow.value) === getIqcOrderItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IqcOrderItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: IqcOrderItem) {
  const key = getIqcOrderItemId(record)
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
 * @returns {IqcOrderItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IqcOrderItemQuery>): IqcOrderItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IqcOrderItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    defectHandlings: masterIqcOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IqcOrderItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('iqcOrderCode', form.iqcOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('batchNo', form.batchNo)
  if (form.purchaseQuantity !== undefined && form.purchaseQuantity !== null) {
    query.purchaseQuantity = form.purchaseQuantity
  }
  assignTrimmed('standardCode', form.standardCode)
  assignTrimmed('samplingSchemeCode', form.samplingSchemeCode)
  if (form.inspectionMethod !== undefined && form.inspectionMethod !== null) {
    query.inspectionMethod = form.inspectionMethod
  }
  if (form.sampleQuantity !== undefined && form.sampleQuantity !== null) {
    query.sampleQuantity = form.sampleQuantity
  }
  if (form.qualifiedQuantity !== undefined && form.qualifiedQuantity !== null) {
    query.qualifiedQuantity = form.qualifiedQuantity
  }
  if (form.unqualifiedQuantity !== undefined && form.unqualifiedQuantity !== null) {
    query.unqualifiedQuantity = form.unqualifiedQuantity
  }
  if (form.inspectionReturnQuantity !== undefined && form.inspectionReturnQuantity !== null) {
    query.inspectionReturnQuantity = form.inspectionReturnQuantity
  }
  assignTrimmed('sampleSerialNo', form.sampleSerialNo)
  assignTrimmed('inspectionDescription', form.inspectionDescription)
  assignTrimmed('inspectorBy', form.inspectorBy)
  assignTrimmed('inspectionDateStart', form.inspectionDateStart)
  assignTrimmed('inspectionDateEnd', form.inspectionDateEnd)
  if (form.judgeStatus !== undefined && form.judgeStatus !== null) {
    query.judgeStatus = form.judgeStatus
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
    const res = await getIqcOrderItemList(buildListQuery())
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
watch(masterIqcOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.iqcorderitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: IqcOrderItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.iqcorderitem._self') })
  formLoading.value = true
  try {
    const detail = await getIqcOrderItemById(getIqcOrderItemId(record))
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
      entity: t('entity.iqcorderitem._self'),
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
    const id = formData.value?.iqcOrderItemId
    if (id) {
      await updateIqcOrderItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.iqcorderitem._self') }))
    } else {
      await createIqcOrderItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.iqcorderitem._self') }))
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

async function handleDeleteOne(record: IqcOrderItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.iqcorderitem._self'),
      name: t('common.tip.this.target', { target: t('entity.iqcorderitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIqcOrderItemById(getIqcOrderItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.iqcorderitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.iqcorderitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.iqcorderitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getIqcOrderItemId(r)).filter(Boolean)
      await deleteIqcOrderItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.iqcorderitem._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getIqcOrderItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importIqcOrderItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
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
    const exportMeta = await exportIqcOrderItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.iqcorderitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.iqcorderitem._self') }))
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
