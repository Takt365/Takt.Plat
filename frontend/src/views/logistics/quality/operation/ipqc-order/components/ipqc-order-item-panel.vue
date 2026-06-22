<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order/components -->
<!-- 文件名称：ipqc-order-item-panel.vue -->
<!-- 功能描述：IPQC制程检验单实体主表实体右侧明细 ipqcOrderItem 独立 CRUD（按主表选中 ipqcOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ipqc-order-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.ipqcorderitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:ipqcorder:create"
      update-permission="logistics:quality:operation:ipqcorder:update"
      delete-permission="logistics:quality:operation:ipqcorder:delete"
      import-permission="logistics:quality:operation:ipqcorder:import"
      export-permission="logistics:quality:operation:ipqcorder:export"
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
    <div class="ipqc-order-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getIpqcOrderItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ipqcOrderItemId"
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
      <IpqcOrderItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterIpqcOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-ipqc-order-ipqc-order-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ipqcOrderCode')">
      <a-form-item :label="t('entity.ipqcorderitem.ipqcordercode')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.ipqcordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ipqcorderitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.ipqcorderitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.ipqcorderitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.ipqcorderitem.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.batchno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.productionquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.productionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.productionquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardCode')">
      <a-form-item :label="t('entity.ipqcorderitem.standardcode')">
        <a-input
          v-model:value="advancedQueryForm.standardCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.standardcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeCode')">
      <a-form-item :label="t('entity.ipqcorderitem.samplingschemecode')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.samplingschemecode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionMethod')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectionmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectionmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.samplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.sampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.samplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unqualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.unqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.unqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.unqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionReturnQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.ipqcorderitem.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.judgestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleSerialNo')">
      <a-form-item :label="t('entity.ipqcorderitem.sampleserialno')">
        <a-input
          v-model:value="advancedQueryForm.sampleSerialNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.sampleserialno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDescription')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondescription')">
        <a-textarea
          v-model:value="advancedQueryForm.inspectionDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcorderitem.inspectiondescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectorBy')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectorby')">
        <a-input
          v-model:value="advancedQueryForm.inspectorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectorby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorderitem.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorderitem.inspectiondateend') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.ipqcorderitem.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcorderitem.extfield') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ipqcorderitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ipqcorderitem._self"
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
      id-column-key="ipqcOrderItemId"
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
 * IPQC制程检验单实体子表 ipqcOrderItem 右栏面板
 * @module views/logistics/quality/operation/ipqc-order/components
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
import IpqcOrderItemForm from './ipqc-order-item-form.vue'
import { useIpqcOrderMasterContext } from '../composables/use-ipqc-order-master-context'
import {
  getIpqcOrderItemList,
  getIpqcOrderItemById,
  createIpqcOrderItem,
  updateIpqcOrderItem,
  deleteIpqcOrderItemById,
  deleteIpqcOrderItemBatch,
  getIpqcOrderItemTemplate,
  importIpqcOrderItem,
  exportIpqcOrderItem,
} from '@/api/logistics/quality/operation/ipqc-order-item'
import type { IpqcOrderItem, IpqcOrderItemQuery } from '@/types/logistics/quality/operation/ipqc-order-item'

const { t } = useI18n()
const { selectedMasterRow } = useIpqcOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIpqcOrderItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcorderitem._self') }),
)

const loading = ref(false)
const dataSource = ref<IpqcOrderItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<IpqcOrderItem | null>(null)
const selectedRows = ref<IpqcOrderItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<IpqcOrderItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  batchNo: '',
  productionQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  judgeStatus: undefined as number | undefined,
  sampleSerialNo: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ipqcOrderCode', label: t('entity.ipqcorderitem.ipqcordercode') },
  { key: 'lineNumber', label: t('entity.ipqcorderitem.linenumber') },
  { key: 'materialCode', label: t('entity.ipqcorderitem.materialcode') },
  { key: 'materialName', label: t('entity.ipqcorderitem.materialname') },
  { key: 'batchNo', label: t('entity.ipqcorderitem.batchno') },
  { key: 'productionQuantity', label: t('entity.ipqcorderitem.productionquantity') },
  { key: 'standardCode', label: t('entity.ipqcorderitem.standardcode') },
  { key: 'samplingSchemeCode', label: t('entity.ipqcorderitem.samplingschemecode') },
  { key: 'inspectionMethod', label: t('entity.ipqcorderitem.inspectionmethod') },
  { key: 'sampleQuantity', label: t('entity.ipqcorderitem.samplequantity') },
  { key: 'qualifiedQuantity', label: t('entity.ipqcorderitem.qualifiedquantity') },
  { key: 'unqualifiedQuantity', label: t('entity.ipqcorderitem.unqualifiedquantity') },
  { key: 'inspectionReturnQuantity', label: t('entity.ipqcorderitem.inspectionreturnquantity') },
  { key: 'judgeStatus', label: t('entity.ipqcorderitem.judgestatus') },
  { key: 'sampleSerialNo', label: t('entity.ipqcorderitem.sampleserialno') },
  { key: 'inspectionDescription', label: t('entity.ipqcorderitem.inspectiondescription') },
  { key: 'inspectorBy', label: t('entity.ipqcorderitem.inspectorby') },
  { key: 'inspectionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ipqcorderitem.inspectiondate')) },
  { key: 'inspectionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ipqcorderitem.inspectiondate')) },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.ipqcorderitem.extfield') },
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
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  batchNo: '',
  productionQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  judgeStatus: undefined as number | undefined,
  sampleSerialNo: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
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

const entityIdName = 'ipqcOrderItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ipqcOrderId)
const masterIpqcOrderId = computed(() => selectedMasterRow.value?.ipqcOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getIpqcOrderItemId(record: IpqcOrderItem | Record<string, unknown>): string {
  return String((record as IpqcOrderItem)?.[entityIdName] ?? '')
}

function getIpqcOrderItemField(record: IpqcOrderItem | Record<string, unknown>, field: string): unknown {
  return (record as IpqcOrderItem)?.[field as keyof IpqcOrderItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ipqcOrderItemId',
    key: 'ipqcOrderItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'ipqcOrderItemId') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.ipqcordercode'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'ipqcOrderCode') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'batchNo') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.productionquantity'),
    dataIndex: 'productionQuantity',
    key: 'productionQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'productionQuantity') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'standardCode') ?? ''),
  },
  {
    title: t('entity.ipqcorderitem.samplingschemecode'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcOrderItem }) =>
      String(getIpqcOrderItemField(record, 'samplingSchemeCode') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:ipqcorder:update',
        onClick: (record: IpqcOrderItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqcorder:delete',
        onClick: (record: IpqcOrderItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IpqcOrderItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IpqcOrderItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getIpqcOrderItemId(selectedRow.value) === getIpqcOrderItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IpqcOrderItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: IpqcOrderItem) {
  const key = getIpqcOrderItemId(record)
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
 * @returns {IpqcOrderItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IpqcOrderItemQuery>): IpqcOrderItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IpqcOrderItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ipqcOrderId: masterIpqcOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IpqcOrderItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ipqcOrderCode', form.ipqcOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('batchNo', form.batchNo)
  if (form.productionQuantity !== undefined && form.productionQuantity !== null) {
    query.productionQuantity = form.productionQuantity
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
  if (form.judgeStatus !== undefined && form.judgeStatus !== null) {
    query.judgeStatus = form.judgeStatus
  }
  assignTrimmed('sampleSerialNo', form.sampleSerialNo)
  assignTrimmed('inspectionDescription', form.inspectionDescription)
  assignTrimmed('inspectorBy', form.inspectorBy)
  assignTrimmed('inspectionDateStart', form.inspectionDateStart)
  assignTrimmed('inspectionDateEnd', form.inspectionDateEnd)
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
    const res = await getIpqcOrderItemList(buildListQuery())
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
watch(masterIpqcOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ipqcorderitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: IpqcOrderItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ipqcorderitem._self') })
  formLoading.value = true
  try {
    const detail = await getIpqcOrderItemById(getIpqcOrderItemId(record))
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
      entity: t('entity.ipqcorderitem._self'),
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
    const id = formData.value?.ipqcOrderItemId
    if (id) {
      await updateIpqcOrderItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.ipqcorderitem._self') }))
    } else {
      await createIpqcOrderItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.ipqcorderitem._self') }))
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

async function handleDeleteOne(record: IpqcOrderItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ipqcorderitem._self'),
      name: t('common.tip.this.target', { target: t('entity.ipqcorderitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcOrderItemById(getIpqcOrderItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorderitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ipqcorderitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ipqcorderitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getIpqcOrderItemId(r)).filter(Boolean)
      await deleteIpqcOrderItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorderitem._self') }))
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
  const res = await getIpqcOrderItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importIpqcOrderItem(file, sheetName)
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
    const exportMeta = await exportIpqcOrderItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcorderitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.ipqcorderitem._self') }))
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
