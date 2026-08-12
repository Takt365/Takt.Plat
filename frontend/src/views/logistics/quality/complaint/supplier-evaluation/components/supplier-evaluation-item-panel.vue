<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-item-panel.vue -->
<!-- 功能描述：供应商评价考核主表实体主表实体右侧明细 supplierEvaluationItem 独立 CRUD（按主表选中 supplierEvaluationCode 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="supplier-evaluation-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:complaint:supplier:evaluation:create"
      update-permission="logistics:quality:complaint:supplier:evaluation:update"
      delete-permission="logistics:quality:complaint:supplier:evaluation:delete"
      import-permission="logistics:quality:complaint:supplier:evaluation:import"
      export-permission="logistics:quality:complaint:supplier:evaluation:export"
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
      class="supplier-evaluation-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getSupplierEvaluationItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="supplierEvaluationItemId"
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
      <SupplierEvaluationItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSupplierEvaluationId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-complaint-supplier-evaluation-supplier-evaluation-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('evaluationId')">
      <a-form-item :label="pi.queryLabel('evaluationId')">
        <TaktSelect
          v-model:value="advancedQueryForm.evaluationId"
          api-url="TaktSupplierEvaluations/options"
          :placeholder="pi.queryPh('evaluationId', 'select')"
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
      <div v-show="isFieldVisible('categoryType')">
      <a-form-item :label="pi.queryLabel('categoryType')">
        <TaktSelect
          v-model:value="advancedQueryForm.categoryType"
          dict-type="logistics_quality_evaluation_category"
          :placeholder="pi.queryPh('categoryType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="pi.queryLabel('itemName')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="pi.queryPh('itemName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemDescription')">
      <a-form-item :label="pi.queryLabel('itemDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.itemDescription"
          :placeholder="pi.queryPh('itemDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weight')">
      <a-form-item :label="pi.queryLabel('weight')">
        <a-input-number
          v-model:value="advancedQueryForm.weight"
          :placeholder="pi.queryPh('weight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringStandard')">
      <a-form-item :label="pi.queryLabel('scoringStandard')">
        <a-input
          v-model:value="advancedQueryForm.scoringStandard"
          :placeholder="pi.queryPh('scoringStandard', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('score')">
      <a-form-item :label="pi.queryLabel('score')">
        <a-input-number
          v-model:value="advancedQueryForm.score"
          :placeholder="pi.queryPh('score', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ratingLevel')">
      <a-form-item :label="pi.queryLabel('ratingLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.ratingLevel"
          dict-type="logistics_quality_supplier_rating"
          :placeholder="pi.queryPh('ratingLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationComment')">
      <a-form-item :label="pi.queryLabel('evaluationComment')">
        <a-input
          v-model:value="advancedQueryForm.evaluationComment"
          :placeholder="pi.queryPh('evaluationComment', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('existingIssues')">
      <a-form-item :label="pi.queryLabel('existingIssues')">
        <a-input
          v-model:value="advancedQueryForm.existingIssues"
          :placeholder="pi.queryPh('existingIssues', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementRequirement')">
      <a-form-item :label="pi.queryLabel('improvementRequirement')">
        <a-input
          v-model:value="advancedQueryForm.improvementRequirement"
          :placeholder="pi.queryPh('improvementRequirement', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationRequired')">
      <a-form-item :label="pi.queryLabel('rectificationRequired')">
        <a-input-number
          v-model:value="advancedQueryForm.rectificationRequired"
          :placeholder="pi.queryPh('rectificationRequired', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineStart')">
      <a-form-item :label="pi.queryLabel('rectificationDeadlineStart')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineStart"
          :placeholder="pi.queryPh('rectificationDeadlineStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineEnd')">
      <a-form-item :label="pi.queryLabel('rectificationDeadlineEnd')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineEnd"
          :placeholder="pi.queryPh('rectificationDeadlineEnd', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationStatus')">
      <a-form-item :label="pi.queryLabel('rectificationStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.rectificationStatus"
          dict-type="logistics_quality_rectification_status"
          :placeholder="pi.queryPh('rectificationStatus', 'select')"
          allow-clear
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
        :entity-i18n-key="SUPPLIEREVALUATIONITEM_SELF_I18N_KEY"
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
      id-column-key="supplierEvaluationItemId"
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
 * 供应商评价考核主表实体子表 supplierEvaluationItem 右栏面板
 * @module views/logistics/quality/complaint/supplier-evaluation/components
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
import SupplierEvaluationItemForm from './supplier-evaluation-item-form.vue'
import { useSupplierEvaluationMasterContext } from '../composables/use-supplier-evaluation-master-context'
import {
  getSupplierEvaluationItemList,
  getSupplierEvaluationItemById,
  createSupplierEvaluationItem,
  updateSupplierEvaluationItem,
  deleteSupplierEvaluationItemById,
  deleteSupplierEvaluationItemBatch,
  getSupplierEvaluationItemTemplate,
  importSupplierEvaluationItem,
  exportSupplierEvaluationItem,
} from '@/api/logistics/quality/complaint/supplier-evaluation-item'
import type { SupplierEvaluationItem, SupplierEvaluationItemQuery } from '@/types/logistics/quality/complaint/supplier-evaluation-item'

import {
  useSupplierEvaluationItemI18n,
  SUPPLIEREVALUATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS,
  SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS,
  SUPPLIEREVALUATIONITEM_QUERY_FIELDS,
  SUPPLIEREVALUATIONITEM_SELF_I18N_KEY,
} from '../composables/use-supplier-evaluation-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSupplierEvaluationItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useSupplierEvaluationMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSupplierEvaluationItem')
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
const dataSource = ref<SupplierEvaluationItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SupplierEvaluationItem | null>(null)
const selectedRows = ref<SupplierEvaluationItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SupplierEvaluationItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    categoryType: undefined as number | undefined,
    weight: undefined as number | undefined,
    score: undefined as number | undefined,
    ratingLevel: undefined as number | undefined,
    rectificationRequired: undefined as number | undefined,
    rectificationStatus: undefined as number | undefined,
    isObsolete: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SUPPLIEREVALUATIONITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...SUPPLIEREVALUATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...SUPPLIEREVALUATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'supplierEvaluationItemId'
const masterSupplierEvaluationId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['supplierEvaluationId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSupplierEvaluationId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSupplierEvaluationItemId(record: SupplierEvaluationItem | Record<string, unknown>): string {
  return String((record as SupplierEvaluationItem)?.[entityIdName] ?? '')
}

function getSupplierEvaluationItemField(record: SupplierEvaluationItem | Record<string, unknown>, field: string): unknown {
  return (record as SupplierEvaluationItem)?.[field as keyof SupplierEvaluationItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'supplierEvaluationItemId',
    key: 'supplierEvaluationItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'supplierEvaluationItemId') ?? ''),
  },
  {
    title: pi.label('evaluationId'),
    dataIndex: 'evaluationId',
    key: 'evaluationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'evaluationId') ?? ''),
  },
  {
    title: pi.label('supplierEvaluationCode'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'supplierEvaluationCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('categoryType'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'categoryType') ?? ''),
  },
  {
    title: pi.label('itemName'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'itemName') ?? ''),
  },
  {
    title: pi.label('itemDescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'itemDescription') ?? ''),
  },
  {
    title: pi.label('weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'weight') ?? ''),
  },
  {
    title: pi.label('scoringStandard'),
    dataIndex: 'scoringStandard',
    key: 'scoringStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'scoringStandard') ?? ''),
  },
  {
    title: pi.label('score'),
    dataIndex: 'score',
    key: 'score',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'score') ?? ''),
  },
  {
    title: pi.label('ratingLevel'),
    dataIndex: 'ratingLevel',
    key: 'ratingLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'ratingLevel') ?? ''),
  },
  {
    title: pi.label('evaluationComment'),
    dataIndex: 'evaluationComment',
    key: 'evaluationComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'evaluationComment') ?? ''),
  },
  {
    title: pi.label('existingIssues'),
    dataIndex: 'existingIssues',
    key: 'existingIssues',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'existingIssues') ?? ''),
  },
  {
    title: pi.label('improvementRequirement'),
    dataIndex: 'improvementRequirement',
    key: 'improvementRequirement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'improvementRequirement') ?? ''),
  },
  {
    title: pi.label('rectificationRequired'),
    dataIndex: 'rectificationRequired',
    key: 'rectificationRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'rectificationRequired') ?? ''),
  },
  {
    title: pi.label('rectificationDeadline'),
    dataIndex: 'rectificationDeadline',
    key: 'rectificationDeadline',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'rectificationDeadline') ?? ''),
  },
  {
    title: pi.label('rectificationStatus'),
    dataIndex: 'rectificationStatus',
    key: 'rectificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'rectificationStatus') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:supplier:evaluation:update',
        onClick: (record: SupplierEvaluationItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:supplier:evaluation:delete',
        onClick: (record: SupplierEvaluationItem) => void handleDeleteOne(record),
      }],
  })])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'supplierEvaluationItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS)

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
    SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getSupplierEvaluationItemField(row, field))
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
  onChange: (keys: (string | number)[], rows: SupplierEvaluationItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SupplierEvaluationItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSupplierEvaluationItemId(selectedRow.value) === getSupplierEvaluationItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SupplierEvaluationItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SupplierEvaluationItem) {
  const key = getSupplierEvaluationItemId(record)
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
 * @returns {SupplierEvaluationItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SupplierEvaluationItemQuery>): SupplierEvaluationItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SupplierEvaluationItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    supplierEvaluationCode: masterSupplierEvaluationId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SupplierEvaluationItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.categoryType !== undefined && form.categoryType !== null) {
    query.categoryType = form.categoryType
  }
  if (form.weight !== undefined && form.weight !== null) {
    query.weight = form.weight
  }
  if (form.score !== undefined && form.score !== null) {
    query.score = form.score
  }
  if (form.ratingLevel !== undefined && form.ratingLevel !== null) {
    query.ratingLevel = form.ratingLevel
  }
  if (form.rectificationRequired !== undefined && form.rectificationRequired !== null) {
    query.rectificationRequired = form.rectificationRequired
  }
  if (form.rectificationStatus !== undefined && form.rectificationStatus !== null) {
    query.rectificationStatus = form.rectificationStatus
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
    const res = await getSupplierEvaluationItemList(buildListQuery())
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
watch(masterSupplierEvaluationId, () => {
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

async function handleEdit(record: SupplierEvaluationItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getSupplierEvaluationItemById(getSupplierEvaluationItemId(record))
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
    const id = formData.value?.supplierEvaluationItemId
    if (id) {
      await updateSupplierEvaluationItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSupplierEvaluationItem(payload)
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

async function handleDeleteOne(record: SupplierEvaluationItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierEvaluationItemById(getSupplierEvaluationItemId(record))
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
      const ids = selectedRows.value.map((r) => getSupplierEvaluationItemId(r)).filter(Boolean)
      await deleteSupplierEvaluationItemBatch(ids)
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
  const res = await getSupplierEvaluationItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSupplierEvaluationItem(file, sheetName)
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
    const exportMeta = await exportSupplierEvaluationItem(
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
