<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/purchase-plan/components -->
<!-- 文件名称：purchase-plan-item-panel.vue -->
<!-- 功能描述：Takt采购计划实体主表实体右侧明细 purchasePlanItem 独立 CRUD（按主表选中 purchasePlanId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="purchase-plan-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.purchaseplanitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:planning:purchaseplan:create"
      update-permission="logistics:manufacturing:planning:purchaseplan:update"
      delete-permission="logistics:manufacturing:planning:purchaseplan:delete"
      import-permission="logistics:manufacturing:planning:purchaseplan:import"
      export-permission="logistics:manufacturing:planning:purchaseplan:export"
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
    <div class="purchase-plan-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPurchasePlanItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchasePlanItemId"
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
      <PurchasePlanItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchasePlanId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-planning-purchase-plan-purchase-plan-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('purchasePlanCode')">
      <a-form-item :label="t('entity.purchaseplanitem.purchaseplancode')">
        <a-input
          v-model:value="advancedQueryForm.purchasePlanCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.purchaseplancode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.purchaseplanitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionPlanId')">
      <a-form-item :label="t('entity.purchaseplanitem.productionplanid')">
        <a-input
          v-model:value="advancedQueryForm.productionPlanId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.productionplanid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionPlanCode')">
      <a-form-item :label="t('entity.purchaseplanitem.productionplancode')">
        <a-input
          v-model:value="advancedQueryForm.productionPlanCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.productionplancode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionPlanLineNumber')">
      <a-form-item :label="t('entity.purchaseplanitem.productionplanlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.productionPlanLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.productionplanlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.purchaseplanitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.purchaseplanitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.purchaseplanitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planUnit')">
      <a-form-item :label="t('entity.purchaseplanitem.planunit')">
        <a-input
          v-model:value="advancedQueryForm.planUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.planunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planQuantity')">
      <a-form-item :label="t('entity.purchaseplanitem.planquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.planQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.planquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseplanitem.plannedarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseplanitem.plannedarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseplanitem.plannedarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseplanitem.plannedarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="t('entity.purchaseplanitem.convertedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.convertedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedUnitPrice')">
      <a-form-item :label="t('entity.purchaseplanitem.estimatedunitprice')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedUnitPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.estimatedunitprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedAmount')">
      <a-form-item :label="t('entity.purchaseplanitem.estimatedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.estimatedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceSupplierCode')">
      <a-form-item :label="t('entity.purchaseplanitem.referencesuppliercode')">
        <a-input
          v-model:value="advancedQueryForm.referenceSupplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.referencesuppliercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceSupplierName')">
      <a-form-item :label="t('entity.purchaseplanitem.referencesuppliername')">
        <a-input
          v-model:value="advancedQueryForm.referenceSupplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseplanitem.referencesuppliername') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseplanitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseplanitem._self"
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
      id-column-key="purchasePlanItemId"
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
 * Takt采购计划实体子表 purchasePlanItem 右栏面板
 * @module views/logistics/manufacturing/planning/purchase-plan/components
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
import PurchasePlanItemForm from './purchase-plan-item-form.vue'
import { usePurchasePlanMasterContext } from '../composables/use-purchase-plan-master-context'
import {
  getPurchasePlanItemList,
  getPurchasePlanItemById,
  createPurchasePlanItem,
  updatePurchasePlanItem,
  deletePurchasePlanItemById,
  deletePurchasePlanItemBatch,
  getPurchasePlanItemTemplate,
  importPurchasePlanItem,
  exportPurchasePlanItem,
} from '@/api/logistics/manufacturing/planning/purchase-plan-item'
import type { PurchasePlanItem, PurchasePlanItemQuery } from '@/types/logistics/manufacturing/planning/purchase-plan-item'

const { t } = useI18n()
const { selectedMasterRow } = usePurchasePlanMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchasePlanItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseplanitem._self') }),
)

const loading = ref(false)
const dataSource = ref<PurchasePlanItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchasePlanItem | null>(null)
const selectedRows = ref<PurchasePlanItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchasePlanItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  purchasePlanCode: '',
  lineNumber: undefined as number | undefined,
  productionPlanId: '',
  productionPlanCode: '',
  productionPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  planUnit: '',
  planQuantity: undefined as number | undefined,
  plannedArrivalDateStart: '',
  plannedArrivalDateEnd: '',
  convertedQuantity: undefined as number | undefined,
  estimatedUnitPrice: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
  referenceSupplierCode: '',
  referenceSupplierName: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'purchasePlanCode', label: t('entity.purchaseplanitem.purchaseplancode') },
  { key: 'lineNumber', label: t('entity.purchaseplanitem.linenumber') },
  { key: 'productionPlanId', label: t('entity.purchaseplanitem.productionplanid') },
  { key: 'productionPlanCode', label: t('entity.purchaseplanitem.productionplancode') },
  { key: 'productionPlanLineNumber', label: t('entity.purchaseplanitem.productionplanlinenumber') },
  { key: 'materialCode', label: t('entity.purchaseplanitem.materialcode') },
  { key: 'materialName', label: t('entity.purchaseplanitem.materialname') },
  { key: 'materialSpecification', label: t('entity.purchaseplanitem.materialspecification') },
  { key: 'planUnit', label: t('entity.purchaseplanitem.planunit') },
  { key: 'planQuantity', label: t('entity.purchaseplanitem.planquantity') },
  { key: 'plannedArrivalDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseplanitem.plannedarrivaldate')) },
  { key: 'plannedArrivalDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseplanitem.plannedarrivaldate')) },
  { key: 'convertedQuantity', label: t('entity.purchaseplanitem.convertedquantity') },
  { key: 'estimatedUnitPrice', label: t('entity.purchaseplanitem.estimatedunitprice') },
  { key: 'estimatedAmount', label: t('entity.purchaseplanitem.estimatedamount') },
  { key: 'referenceSupplierCode', label: t('entity.purchaseplanitem.referencesuppliercode') },
  { key: 'referenceSupplierName', label: t('entity.purchaseplanitem.referencesuppliername') },
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
  purchasePlanCode: '',
  lineNumber: undefined as number | undefined,
  productionPlanId: '',
  productionPlanCode: '',
  productionPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  planUnit: '',
  planQuantity: undefined as number | undefined,
  plannedArrivalDateStart: '',
  plannedArrivalDateEnd: '',
  convertedQuantity: undefined as number | undefined,
  estimatedUnitPrice: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
  referenceSupplierCode: '',
  referenceSupplierName: '',
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

const entityIdName = 'purchasePlanItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.purchasePlanId)
const masterPurchasePlanId = computed(() => selectedMasterRow.value?.purchasePlanId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchasePlanItemId(record: PurchasePlanItem | Record<string, unknown>): string {
  return String((record as PurchasePlanItem)?.[entityIdName] ?? '')
}

function getPurchasePlanItemField(record: PurchasePlanItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchasePlanItem)?.[field as keyof PurchasePlanItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchasePlanItemId',
    key: 'purchasePlanItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'purchasePlanItemId') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.purchaseplancode'),
    dataIndex: 'purchasePlanCode',
    key: 'purchasePlanCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'purchasePlanCode') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.productionplanid'),
    dataIndex: 'productionPlanId',
    key: 'productionPlanId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'productionPlanId') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.productionplancode'),
    dataIndex: 'productionPlanCode',
    key: 'productionPlanCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'productionPlanCode') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.productionplanlinenumber'),
    dataIndex: 'productionPlanLineNumber',
    key: 'productionPlanLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'productionPlanLineNumber') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.purchaseplanitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchasePlanItem }) =>
      String(getPurchasePlanItemField(record, 'materialSpecification') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:planning:purchaseplan:update',
        onClick: (record: PurchasePlanItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:planning:purchaseplan:delete',
        onClick: (record: PurchasePlanItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchasePlanItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchasePlanItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchasePlanItemId(selectedRow.value) === getPurchasePlanItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchasePlanItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchasePlanItem) {
  const key = getPurchasePlanItemId(record)
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
 * @returns {PurchasePlanItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchasePlanItemQuery>): PurchasePlanItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchasePlanItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchasePlanId: masterPurchasePlanId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchasePlanItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('purchasePlanCode', form.purchasePlanCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('productionPlanId', form.productionPlanId)
  assignTrimmed('productionPlanCode', form.productionPlanCode)
  if (form.productionPlanLineNumber !== undefined && form.productionPlanLineNumber !== null) {
    query.productionPlanLineNumber = form.productionPlanLineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('planUnit', form.planUnit)
  if (form.planQuantity !== undefined && form.planQuantity !== null) {
    query.planQuantity = form.planQuantity
  }
  assignTrimmed('plannedArrivalDateStart', form.plannedArrivalDateStart)
  assignTrimmed('plannedArrivalDateEnd', form.plannedArrivalDateEnd)
  if (form.convertedQuantity !== undefined && form.convertedQuantity !== null) {
    query.convertedQuantity = form.convertedQuantity
  }
  if (form.estimatedUnitPrice !== undefined && form.estimatedUnitPrice !== null) {
    query.estimatedUnitPrice = form.estimatedUnitPrice
  }
  if (form.estimatedAmount !== undefined && form.estimatedAmount !== null) {
    query.estimatedAmount = form.estimatedAmount
  }
  assignTrimmed('referenceSupplierCode', form.referenceSupplierCode)
  assignTrimmed('referenceSupplierName', form.referenceSupplierName)
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
    const res = await getPurchasePlanItemList(buildListQuery())
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
watch(masterPurchasePlanId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseplanitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PurchasePlanItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseplanitem._self') })
  formLoading.value = true
  try {
    const detail = await getPurchasePlanItemById(getPurchasePlanItemId(record))
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
      entity: t('entity.purchaseplanitem._self'),
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
    const id = formData.value?.purchasePlanItemId
    if (id) {
      await updatePurchasePlanItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseplanitem._self') }))
    } else {
      await createPurchasePlanItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.purchaseplanitem._self') }))
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

async function handleDeleteOne(record: PurchasePlanItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.purchaseplanitem._self'),
      name: t('common.tip.this.target', { target: t('entity.purchaseplanitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchasePlanItemById(getPurchasePlanItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseplanitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.purchaseplanitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.purchaseplanitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPurchasePlanItemId(r)).filter(Boolean)
      await deletePurchasePlanItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseplanitem._self') }))
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
  const res = await getPurchasePlanItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchasePlanItem(file, sheetName)
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
    const exportMeta = await exportPurchasePlanItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseplanitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.purchaseplanitem._self') }))
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
