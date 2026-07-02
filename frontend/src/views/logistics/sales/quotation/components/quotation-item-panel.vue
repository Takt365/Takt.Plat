<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/quotation/components -->
<!-- 文件名称：quotation-item-panel.vue -->
<!-- 功能描述：Takt销售报价实体主表实体右侧明细 salesQuotationItem 独立 CRUD（按主表选中 salesQuotationId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="quotation-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ pi.self() }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:sales:quotation:create"
      update-permission="logistics:sales:quotation:update"
      delete-permission="logistics:sales:quotation:delete"
      import-permission="logistics:sales:quotation:import"
      export-permission="logistics:sales:quotation:export"
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
    <div class="quotation-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSalesQuotationItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="salesQuotationItemId"
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
      <SalesQuotationItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSalesQuotationId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-sales-quotation-quotation-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('salesQuotationCode')">
      <a-form-item :label="pi.queryLabel('salesQuotationCode')">
        <a-input
          v-model:value="advancedQueryForm.salesQuotationCode"
          :placeholder="pi.queryPh('salesQuotationCode', 'required')"
          show-count
          :maxlength="50"
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
      <div v-show="isFieldVisible('salesUnit')">
      <a-form-item :label="pi.queryLabel('salesUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesUnit"
          dict-type="logistics_unit_of_measure_code"
          :placeholder="pi.queryPh('salesUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationQuantity')">
      <a-form-item :label="pi.queryLabel('quotationQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quotationQuantity"
          :placeholder="pi.queryPh('quotationQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPerUnit')">
      <a-form-item :label="pi.queryLabel('salesPerUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesPerUnit"
          dict-type="logistics_price_unit_param"
          :placeholder="pi.queryPh('salesPerUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitPrice')">
      <a-form-item :label="pi.queryLabel('unitPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.unitPrice"
          :placeholder="pi.queryPh('unitPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountRate')">
      <a-form-item :label="pi.queryLabel('discountRate')">
        <TaktSelect
          v-model:value="advancedQueryForm.discountRate"
          dict-type="logistics_discount_rate_param"
          :placeholder="pi.queryPh('discountRate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="pi.queryLabel('discountAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="pi.queryPh('discountAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="pi.queryLabel('taxRate')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxRate"
          dict-type="accounting_tax_rate_param"
          :placeholder="pi.queryPh('taxRate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="pi.queryLabel('taxAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="pi.queryPh('taxAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subtotalAmount')">
      <a-form-item :label="pi.queryLabel('subtotalAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.subtotalAmount"
          :placeholder="pi.queryPh('subtotalAmount', 'required')"
          style="width: 100%"
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
        :entity-i18n-key="SALESQUOTATIONITEM_SELF_I18N_KEY"
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
      id-column-key="salesQuotationItemId"
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
 * Takt销售报价实体子表 salesQuotationItem 右栏面板
 * @module views/logistics/sales/quotation/components
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
import SalesQuotationItemForm from './quotation-item-form.vue'
import { useSalesQuotationMasterContext } from '../composables/use-quotation-master-context'
import {
  getSalesQuotationItemList,
  getSalesQuotationItemById,
  createSalesQuotationItem,
  updateSalesQuotationItem,
  deleteSalesQuotationItemById,
  deleteSalesQuotationItemBatch,
  getSalesQuotationItemTemplate,
  importSalesQuotationItem,
  exportSalesQuotationItem,
} from '@/api/logistics/sales/quotation-item'
import type { SalesQuotationItem, SalesQuotationItemQuery } from '@/types/logistics/sales/quotation-item'

import {
  useSalesQuotationItemI18n,
  SALESQUOTATIONITEM_LIST_FIELDS,
  SALESQUOTATIONITEM_QUERY_STRING_FIELDS,
  SALESQUOTATIONITEM_QUERY_FIELDS,
  SALESQUOTATIONITEM_SELF_I18N_KEY,
} from '../composables/use-quotation-item-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesQuotationItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useSalesQuotationMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesQuotationItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
const dataSource = ref<SalesQuotationItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SalesQuotationItem | null>(null)
const selectedRows = ref<SalesQuotationItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SalesQuotationItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESQUOTATIONITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESQUOTATIONITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    quotationQuantity: undefined as number | undefined,
    salesPerUnit: undefined as number | undefined,
    unitPrice: undefined as number | undefined,
    discountRate: undefined as number | undefined,
    discountAmount: undefined as number | undefined,
    taxRate: undefined as number | undefined,
    taxAmount: undefined as number | undefined,
    subtotalAmount: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SALESQUOTATIONITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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

const entityIdName = 'salesQuotationItemId'
const masterSalesQuotationId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['salesQuotationId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSalesQuotationId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSalesQuotationItemId(record: SalesQuotationItem | Record<string, unknown>): string {
  return String((record as SalesQuotationItem)?.[entityIdName] ?? '')
}

function getSalesQuotationItemField(record: SalesQuotationItem | Record<string, unknown>, field: string): unknown {
  return (record as SalesQuotationItem)?.[field as keyof SalesQuotationItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesQuotationItemId',
    key: 'salesQuotationItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationItemId') ?? ''),
  },
  {
    title: pi.label('salesQuotationName'),
    dataIndex: 'salesQuotationName',
    key: 'salesQuotationName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationName') ?? ''),
  },
  {
    title: pi.label('salesQuotationCode'),
    dataIndex: 'salesQuotationCode',
    key: 'salesQuotationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('materialName'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialName') ?? ''),
  },
  {
    title: pi.label('materialSpecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: pi.label('salesUnit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesUnit') ?? ''),
  },
  {
    title: pi.label('quotationQuantity'),
    dataIndex: 'quotationQuantity',
    key: 'quotationQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'quotationQuantity') ?? ''),
  },
  {
    title: pi.label('salesPerUnit'),
    dataIndex: 'salesPerUnit',
    key: 'salesPerUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesPerUnit') ?? ''),
  },
  {
    title: pi.label('unitPrice'),
    dataIndex: 'unitPrice',
    key: 'unitPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'unitPrice') ?? ''),
  },
  {
    title: pi.label('discountRate'),
    dataIndex: 'discountRate',
    key: 'discountRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'discountRate') ?? ''),
  },
  {
    title: pi.label('discountAmount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'discountAmount') ?? ''),
  },
  {
    title: pi.label('taxRate'),
    dataIndex: 'taxRate',
    key: 'taxRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'taxRate') ?? ''),
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'taxAmount') ?? ''),
  },
  {
    title: pi.label('subtotalAmount'),
    dataIndex: 'subtotalAmount',
    key: 'subtotalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'subtotalAmount') ?? ''),
  },
  {
    title: pi.label('salesQuotation'),
    dataIndex: 'salesQuotation',
    key: 'salesQuotation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotation') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:quotation:update',
        onClick: (record: SalesQuotationItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:quotation:delete',
        onClick: (record: SalesQuotationItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesQuotationItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesQuotationItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSalesQuotationItemId(selectedRow.value) === getSalesQuotationItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesQuotationItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SalesQuotationItem) {
  const key = getSalesQuotationItemId(record)
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
 * @returns {SalesQuotationItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesQuotationItemQuery>): SalesQuotationItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesQuotationItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    salesQuotationId: masterSalesQuotationId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesQuotationItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESQUOTATIONITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.quotationQuantity !== undefined && form.quotationQuantity !== null) {
    query.quotationQuantity = form.quotationQuantity
  }
  if (form.salesPerUnit !== undefined && form.salesPerUnit !== null) {
    query.salesPerUnit = form.salesPerUnit
  }
  if (form.unitPrice !== undefined && form.unitPrice !== null) {
    query.unitPrice = form.unitPrice
  }
  if (form.discountRate !== undefined && form.discountRate !== null) {
    query.discountRate = form.discountRate
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.subtotalAmount !== undefined && form.subtotalAmount !== null) {
    query.subtotalAmount = form.subtotalAmount
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
    const res = await getSalesQuotationItemList(buildListQuery())
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
watch(masterSalesQuotationId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SalesQuotationItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getSalesQuotationItemById(getSalesQuotationItemId(record))
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
    const id = formData.value?.salesQuotationItemId
    if (id) {
      await updateSalesQuotationItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesQuotationItem(payload)
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

async function handleDeleteOne(record: SalesQuotationItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesQuotationItemById(getSalesQuotationItemId(record))
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
      const ids = selectedRows.value.map((r) => getSalesQuotationItemId(r)).filter(Boolean)
      await deleteSalesQuotationItemBatch(ids)
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
  const res = await getSalesQuotationItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesQuotationItem(file, sheetName)
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
    const exportMeta = await exportSalesQuotationItem(
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
