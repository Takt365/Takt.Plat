<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/production-plan/components -->
<!-- 文件名称：production-plan-item-panel.vue -->
<!-- 功能描述：Takt生产计划实体主表实体右侧明细 productionPlanItem 独立 CRUD（按主表选中 productionPlanId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="production-plan-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:mrp:production:plan:create"
      update-permission="logistics:manufacturing:mrp:production:plan:update"
      delete-permission="logistics:manufacturing:mrp:production:plan:delete"
      import-permission="logistics:manufacturing:mrp:production:plan:import"
      export-permission="logistics:manufacturing:mrp:production:plan:export"
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
    <div
      ref="detailTableWrapRef"
      class="production-plan-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
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
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
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
      storage-key="takt-query-fields-logistics-manufacturing-mrp-production-plan-production-plan-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('productionPlanCode')">
      <a-form-item :label="pi.queryLabel('productionPlanCode')">
        <a-input
          v-model:value="advancedQueryForm.productionPlanCode"
          :placeholder="pi.queryPh('productionPlanCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesForecastId')">
      <a-form-item :label="pi.queryLabel('salesForecastId')">
        <a-input
          v-model:value="advancedQueryForm.salesForecastId"
          :placeholder="pi.queryPh('salesForecastId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesForecastCode')">
      <a-form-item :label="pi.queryLabel('salesForecastCode')">
        <a-input
          v-model:value="advancedQueryForm.salesForecastCode"
          :placeholder="pi.queryPh('salesForecastCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesForecastLineNumber')">
      <a-form-item :label="pi.queryLabel('salesForecastLineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.salesForecastLineNumber"
          :placeholder="pi.queryPh('salesForecastLineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialRequirementsPlanningItemId')">
      <a-form-item :label="pi.queryLabel('materialRequirementsPlanningItemId')">
        <a-input
          v-model:value="advancedQueryForm.materialRequirementsPlanningItemId"
          :placeholder="pi.queryPh('materialRequirementsPlanningItemId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktMaterials/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="pi.queryLabel('materialName')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="pi.queryPh('materialName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="pi.queryLabel('materialSpecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="pi.queryPh('materialSpecification', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="pi.queryLabel('modelCode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="pi.queryPh('modelCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelName')">
      <a-form-item :label="pi.queryLabel('modelName')">
        <a-input
          v-model:value="advancedQueryForm.modelName"
          :placeholder="pi.queryPh('modelName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planUnit')">
      <a-form-item :label="pi.queryLabel('planUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.planUnit"
          dict-type="logistics_unit_of_measure_code"
          :placeholder="pi.queryPh('planUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planQuantity')">
      <a-form-item :label="pi.queryLabel('planQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.planQuantity"
          :placeholder="pi.queryPh('planQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateStart')">
      <a-form-item :label="pi.queryLabel('plannedStartDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateStart"
          :placeholder="pi.queryPh('plannedStartDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateEnd')">
      <a-form-item :label="pi.queryLabel('plannedStartDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateEnd"
          :placeholder="pi.queryPh('plannedStartDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateStart')">
      <a-form-item :label="pi.queryLabel('plannedEndDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateStart"
          :placeholder="pi.queryPh('plannedEndDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateEnd')">
      <a-form-item :label="pi.queryLabel('plannedEndDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateEnd"
          :placeholder="pi.queryPh('plannedEndDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="pi.queryLabel('convertedQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="pi.queryPh('convertedQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedUnitCost')">
      <a-form-item :label="pi.queryLabel('estimatedUnitCost')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedUnitCost"
          :placeholder="pi.queryPh('estimatedUnitCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('estimatedAmount')">
      <a-form-item :label="pi.queryLabel('estimatedAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.estimatedAmount"
          :placeholder="pi.queryPh('estimatedAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no_type"
          :placeholder="pi.queryPh('isObsolete', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="PRODUCTIONPLANITEM_SELF_I18N_KEY"
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
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt生产计划实体子表 productionPlanItem 右栏面板
 * @module views/logistics/manufacturing/mrp/production-plan/components
 */
import { ref, computed, watch, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/table-columns'
import { formatSummaryValue } from '@/components/business/takt-editable-table/editable-table-utils'
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
} from '@/api/logistics/manufacturing/mrp/production-plan-item'
import type { ProductionPlanItem, ProductionPlanItemQuery } from '@/types/logistics/manufacturing/mrp/production-plan-item'

import {
  useProductionPlanItemI18n,
  PRODUCTIONPLANITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  PRODUCTIONPLANITEM_SUMMARY_SUM_FIELDS,
  PRODUCTIONPLANITEM_QUERY_STRING_FIELDS,
  PRODUCTIONPLANITEM_QUERY_FIELDS,
  PRODUCTIONPLANITEM_SELF_I18N_KEY,
} from '../composables/use-production-plan-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useProductionPlanItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useProductionPlanMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktProductionPlanItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
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
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(PRODUCTIONPLANITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PRODUCTIONPLANITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    salesForecastLineNumber: undefined as number | undefined,
    planQuantity: undefined as number | undefined,
    convertedQuantity: undefined as number | undefined,
    estimatedUnitCost: undefined as number | undefined,
    estimatedAmount: undefined as number | undefined,
    isObsolete: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  PRODUCTIONPLANITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...PRODUCTIONPLANITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...PRODUCTIONPLANITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'productionPlanItemId'
const masterProductionPlanId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['productionPlanId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterProductionPlanId.value !== '')
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
    title: pi.label('productionPlanId'),
    dataIndex: 'productionPlanId',
    key: 'productionPlanId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'productionPlanId') ?? ''),
  },
  {
    title: pi.label('productionPlanCode'),
    dataIndex: 'productionPlanCode',
    key: 'productionPlanCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'productionPlanCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('salesForecastId'),
    dataIndex: 'salesForecastId',
    key: 'salesForecastId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesForecastId') ?? ''),
  },
  {
    title: pi.label('salesForecastCode'),
    dataIndex: 'salesForecastCode',
    key: 'salesForecastCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesForecastCode') ?? ''),
  },
  {
    title: pi.label('salesForecastLineNumber'),
    dataIndex: 'salesForecastLineNumber',
    key: 'salesForecastLineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'salesForecastLineNumber') ?? ''),
  },
  {
    title: pi.label('materialRequirementsPlanningItemId'),
    dataIndex: 'materialRequirementsPlanningItemId',
    key: 'materialRequirementsPlanningItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialRequirementsPlanningItemId') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('materialName'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialName') ?? ''),
  },
  {
    title: pi.label('materialSpecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: pi.label('modelCode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'modelCode') ?? ''),
  },
  {
    title: pi.label('modelName'),
    dataIndex: 'modelName',
    key: 'modelName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'modelName') ?? ''),
  },
  {
    title: pi.label('planUnit'),
    dataIndex: 'planUnit',
    key: 'planUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'planUnit') ?? ''),
  },
  {
    title: pi.label('planQuantity'),
    dataIndex: 'planQuantity',
    key: 'planQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'planQuantity') ?? ''),
  },
  {
    title: pi.label('plannedStartDate'),
    dataIndex: 'plannedStartDate',
    key: 'plannedStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'plannedStartDate') ?? ''),
  },
  {
    title: pi.label('plannedEndDate'),
    dataIndex: 'plannedEndDate',
    key: 'plannedEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'plannedEndDate') ?? ''),
  },
  {
    title: pi.label('convertedQuantity'),
    dataIndex: 'convertedQuantity',
    key: 'convertedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'convertedQuantity') ?? ''),
  },
  {
    title: pi.label('estimatedUnitCost'),
    dataIndex: 'estimatedUnitCost',
    key: 'estimatedUnitCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'estimatedUnitCost') ?? ''),
  },
  {
    title: pi.label('estimatedAmount'),
    dataIndex: 'estimatedAmount',
    key: 'estimatedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'estimatedAmount') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ProductionPlanItem }) =>
      String(getProductionPlanItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:mrp:production:plan:update',
        onClick: (record: ProductionPlanItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:mrp:production:plan:delete',
        onClick: (record: ProductionPlanItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'approval')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'productionPlanItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'approval',
  })
})

const summarySumFieldSet = new Set<string>(PRODUCTIONPLANITEM_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    PRODUCTIONPLANITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof PRODUCTIONPLANITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of PRODUCTIONPLANITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getProductionPlanItemField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
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
  for (const key of PRODUCTIONPLANITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.salesForecastLineNumber !== undefined && form.salesForecastLineNumber !== null) {
    query.salesForecastLineNumber = form.salesForecastLineNumber
  }
  if (form.planQuantity !== undefined && form.planQuantity !== null) {
    query.planQuantity = form.planQuantity
  }
  if (form.convertedQuantity !== undefined && form.convertedQuantity !== null) {
    query.convertedQuantity = form.convertedQuantity
  }
  if (form.estimatedUnitCost !== undefined && form.estimatedUnitCost !== null) {
    query.estimatedUnitCost = form.estimatedUnitCost
  }
  if (form.estimatedAmount !== undefined && form.estimatedAmount !== null) {
    query.estimatedAmount = form.estimatedAmount
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
  }
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

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selected) => {
  if (selected) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ProductionPlanItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
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
      entity: pi.self(),
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
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createProductionPlanItem(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
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
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProductionPlanItemById(getProductionPlanItemId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getProductionPlanItemId(r)).filter(Boolean)
      await deleteProductionPlanItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
  const res = await getProductionPlanItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importProductionPlanItem(file, sheetName)
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
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
