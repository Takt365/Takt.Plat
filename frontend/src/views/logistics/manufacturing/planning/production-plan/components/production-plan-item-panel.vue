<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/production-plan/components -->
<!-- 文件名称：production-plan-item-panel.vue -->
<!-- 功能描述：Takt生产计划实体主表实体右侧明细 productionPlanItem 独立 CRUD（按主表选中 productionPlanId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="production-plan-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.productionplanitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:planning:production:plan:create"
      update-permission="logistics:manufacturing:planning:production:plan:update"
      delete-permission="logistics:manufacturing:planning:production:plan:delete"
      import-permission="logistics:manufacturing:planning:production:plan:import"
      export-permission="logistics:manufacturing:planning:production:plan:export"
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
    <div class="production-plan-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getProductionPlanItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="productionPlanItemId"
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
      <ProductionPlanItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterProductionPlanId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-planning-production-plan-production-plan-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('productionPlanCode')">
      <a-form-item :label="t('entity.productionplanitem.productionplancode')">
        <a-input
          v-model:value="advancedQueryForm.productionPlanCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.productionplancode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.productionplanitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPlanId')">
      <a-form-item :label="t('entity.productionplanitem.salesplanid')">
        <a-input
          v-model:value="advancedQueryForm.salesPlanId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.salesplanid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPlanCode')">
      <a-form-item :label="t('entity.productionplanitem.salesplancode')">
        <a-input
          v-model:value="advancedQueryForm.salesPlanCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.salesplancode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPlanLineNumber')">
      <a-form-item :label="t('entity.productionplanitem.salesplanlinenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.salesPlanLineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.salesplanlinenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.productionplanitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.productionplanitem.materialname')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.materialname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.productionplanitem.materialspecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.materialspecification') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planUnit')">
      <a-form-item :label="t('entity.productionplanitem.planunit')">
        <a-input
          v-model:value="advancedQueryForm.planUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.planunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planQuantity')">
      <a-form-item :label="t('entity.productionplanitem.planquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.planQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.planquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateStart')">
      <a-form-item :label="t('entity.productionplanitem.plannedstartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionplanitem.plannedstartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateEnd')">
      <a-form-item :label="t('entity.productionplanitem.plannedstartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionplanitem.plannedstartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateStart')">
      <a-form-item :label="t('entity.productionplanitem.plannedenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionplanitem.plannedenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateEnd')">
      <a-form-item :label="t('entity.productionplanitem.plannedenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionplanitem.plannedenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="t('entity.productionplanitem.convertedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.convertedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedUnitCost')">
      <a-form-item :label="t('entity.productionplanitem.estimatedunitcost')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedUnitCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.estimatedunitcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedAmount')">
      <a-form-item :label="t('entity.productionplanitem.estimatedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionplanitem.estimatedamount') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.productionplanitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.productionplanitem._self"
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
      id-column-key="productionPlanItemId"
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
 * Takt生产计划实体子表 productionPlanItem 右栏面板
 * @module views/logistics/manufacturing/planning/production-plan/components
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
import ProductionPlanItemForm from './production-plan-item-form.vue'
import { useProductionPlanMasterContext } from '../composables/use-production-plan-master-context'
import {
  getProductionPlanItemList,
  getProductionPlanItemById,
  createProductionPlanItem,
  updateProductionPlanItem,
  deleteProductionPlanItemById,
  deleteProductionPlanItemBatch,
  getProductionPlanItemTemplate,
  importProductionPlanItem,
  exportProductionPlanItem,
} from '@/api/logistics/manufacturing/planning/production-plan-item'
import type { ProductionPlanItem, ProductionPlanItemQuery } from '@/types/logistics/manufacturing/planning/production-plan-item'

const { t } = useI18n()
const { selectedMasterRow } = useProductionPlanMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktProductionPlanItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.productionplanitem._self') }),
)

const loading = ref(false)
const dataSource = ref<ProductionPlanItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ProductionPlanItem | null>(null)
const selectedRows = ref<ProductionPlanItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ProductionPlanItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  productionPlanCode: '',
  lineNumber: undefined as number | undefined,
  salesPlanId: '',
  salesPlanCode: '',
  salesPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  planUnit: '',
  planQuantity: undefined as number | undefined,
  plannedStartDateStart: '',
  plannedStartDateEnd: '',
  plannedEndDateStart: '',
  plannedEndDateEnd: '',
  convertedQuantity: undefined as number | undefined,
  estimatedUnitCost: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'productionPlanCode', label: t('entity.productionplanitem.productionplancode') },
  { key: 'lineNumber', label: t('entity.productionplanitem.linenumber') },
  { key: 'salesPlanId', label: t('entity.productionplanitem.salesplanid') },
  { key: 'salesPlanCode', label: t('entity.productionplanitem.salesplancode') },
  { key: 'salesPlanLineNumber', label: t('entity.productionplanitem.salesplanlinenumber') },
  { key: 'materialCode', label: t('entity.productionplanitem.materialcode') },
  { key: 'materialName', label: t('entity.productionplanitem.materialname') },
  { key: 'materialSpecification', label: t('entity.productionplanitem.materialspecification') },
  { key: 'planUnit', label: t('entity.productionplanitem.planunit') },
  { key: 'planQuantity', label: t('entity.productionplanitem.planquantity') },
  { key: 'plannedStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.productionplanitem.plannedstartdate')) },
  { key: 'plannedStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.productionplanitem.plannedstartdate')) },
  { key: 'plannedEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.productionplanitem.plannedenddate')) },
  { key: 'plannedEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.productionplanitem.plannedenddate')) },
  { key: 'convertedQuantity', label: t('entity.productionplanitem.convertedquantity') },
  { key: 'estimatedUnitCost', label: t('entity.productionplanitem.estimatedunitcost') },
  { key: 'estimatedAmount', label: t('entity.productionplanitem.estimatedamount') },
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
  productionPlanCode: '',
  lineNumber: undefined as number | undefined,
  salesPlanId: '',
  salesPlanCode: '',
  salesPlanLineNumber: undefined as number | undefined,
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  planUnit: '',
  planQuantity: undefined as number | undefined,
  plannedStartDateStart: '',
  plannedStartDateEnd: '',
  plannedEndDateStart: '',
  plannedEndDateEnd: '',
  convertedQuantity: undefined as number | undefined,
  estimatedUnitCost: undefined as number | undefined,
  estimatedAmount: undefined as number | undefined,
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

const entityIdName = 'productionPlanItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.productionPlanId)
const masterProductionPlanId = computed(() => selectedMasterRow.value?.productionPlanId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getProductionPlanItemId(record: ProductionPlanItem | Record<string, unknown>): string {
  return String((record as ProductionPlanItem)?.[entityIdName] ?? '')
}

function getProductionPlanItemField(record: ProductionPlanItem | Record<string, unknown>, field: string): unknown {
  return (record as ProductionPlanItem)?.[field as keyof ProductionPlanItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'productionPlanItemId',
    key: 'productionPlanItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'productionPlanItemId') ?? ''),
  },
  {
    title: t('entity.productionplanitem.productionplancode'),
    dataIndex: 'productionPlanCode',
    key: 'productionPlanCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'productionPlanCode') ?? ''),
  },
  {
    title: t('entity.productionplanitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.productionplanitem.salesplanid'),
    dataIndex: 'salesPlanId',
    key: 'salesPlanId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesPlanId') ?? ''),
  },
  {
    title: t('entity.productionplanitem.salesplancode'),
    dataIndex: 'salesPlanCode',
    key: 'salesPlanCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesPlanCode') ?? ''),
  },
  {
    title: t('entity.productionplanitem.salesplanlinenumber'),
    dataIndex: 'salesPlanLineNumber',
    key: 'salesPlanLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesPlanLineNumber') ?? ''),
  },
  {
    title: t('entity.productionplanitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.productionplanitem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialName') ?? ''),
  },
  {
    title: t('entity.productionplanitem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialSpecification') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:planning:production:plan:update',
        onClick: (record: ProductionPlanItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:planning:production:plan:delete',
        onClick: (record: ProductionPlanItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ProductionPlanItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ProductionPlanItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getProductionPlanItemId(selectedRow.value) === getProductionPlanItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ProductionPlanItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ProductionPlanItem) {
  const key = getProductionPlanItemId(record)
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
 * @returns {ProductionPlanItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ProductionPlanItemQuery>): ProductionPlanItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ProductionPlanItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    productionPlanId: masterProductionPlanId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ProductionPlanItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('productionPlanCode', form.productionPlanCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('salesPlanId', form.salesPlanId)
  assignTrimmed('salesPlanCode', form.salesPlanCode)
  if (form.salesPlanLineNumber !== undefined && form.salesPlanLineNumber !== null) {
    query.salesPlanLineNumber = form.salesPlanLineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('planUnit', form.planUnit)
  if (form.planQuantity !== undefined && form.planQuantity !== null) {
    query.planQuantity = form.planQuantity
  }
  assignTrimmed('plannedStartDateStart', form.plannedStartDateStart)
  assignTrimmed('plannedStartDateEnd', form.plannedStartDateEnd)
  assignTrimmed('plannedEndDateStart', form.plannedEndDateStart)
  assignTrimmed('plannedEndDateEnd', form.plannedEndDateEnd)
  if (form.convertedQuantity !== undefined && form.convertedQuantity !== null) {
    query.convertedQuantity = form.convertedQuantity
  }
  if (form.estimatedUnitCost !== undefined && form.estimatedUnitCost !== null) {
    query.estimatedUnitCost = form.estimatedUnitCost
  }
  if (form.estimatedAmount !== undefined && form.estimatedAmount !== null) {
    query.estimatedAmount = form.estimatedAmount
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
    const res = await getProductionPlanItemList(buildListQuery())
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
watch(masterProductionPlanId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.productionplanitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ProductionPlanItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.productionplanitem._self') })
  formLoading.value = true
  try {
    const detail = await getProductionPlanItemById(getProductionPlanItemId(record))
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
      entity: t('entity.productionplanitem._self'),
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
    const id = formData.value?.productionPlanItemId
    if (id) {
      await updateProductionPlanItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.productionplanitem._self') }))
    } else {
      await createProductionPlanItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.productionplanitem._self') }))
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

async function handleDeleteOne(record: ProductionPlanItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.productionplanitem._self'),
      name: t('common.tip.this.target', { target: t('entity.productionplanitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProductionPlanItemById(getProductionPlanItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.productionplanitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.productionplanitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.productionplanitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getProductionPlanItemId(r)).filter(Boolean)
      await deleteProductionPlanItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.productionplanitem._self') }))
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
  const res = await getProductionPlanItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importProductionPlanItem(file, sheetName)
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
    const exportMeta = await exportProductionPlanItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.productionplanitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.productionplanitem._self') }))
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
