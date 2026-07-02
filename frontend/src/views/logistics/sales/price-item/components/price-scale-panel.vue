<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-item/components -->
<!-- 文件名称：price-scale-panel.vue -->
<!-- 功能描述：Takt销售价格明细实体主表实体右侧明细 salesPriceScale 独立 CRUD（按主表选中 salesPriceItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="price-scale-panel flex h-full min-h-0 flex-col overflow-hidden">
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
      create-permission="logistics:sales:price:create"
      update-permission="logistics:sales:price:update"
      delete-permission="logistics:sales:price:delete"
      import-permission="logistics:sales:price:import"
      export-permission="logistics:sales:price:export"
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
    <div class="price-scale-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSalesPriceScaleId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="salesPriceScaleId"
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
      <SalesPriceScaleForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSalesPriceItemId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-sales-price-item-price-scale"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('itemId')">
      <a-form-item :label="pi.queryLabel('itemId')">
        <TaktSelect
          v-model:value="advancedQueryForm.itemId"
          api-url="TaktSalesPriceItems/options"
          :placeholder="pi.queryPh('itemId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPriceCode')">
      <a-form-item :label="pi.queryLabel('salesPriceCode')">
        <a-input
          v-model:value="advancedQueryForm.salesPriceCode"
          :placeholder="pi.queryPh('salesPriceCode', 'required')"
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
      <div v-show="isFieldVisible('startQuantity')">
      <a-form-item :label="pi.queryLabel('startQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.startQuantity"
          :placeholder="pi.queryPh('startQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endQuantity')">
      <a-form-item :label="pi.queryLabel('endQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.endQuantity"
          :placeholder="pi.queryPh('endQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scalePrice')">
      <a-form-item :label="pi.queryLabel('scalePrice')">
        <a-input-number
          v-model:value="advancedQueryForm.scalePrice"
          :placeholder="pi.queryPh('scalePrice', 'required')"
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
        :entity-i18n-key="SALESPRICESCALE_SELF_I18N_KEY"
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
      id-column-key="salesPriceScaleId"
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
 * Takt销售价格明细实体子表 salesPriceScale 右栏面板
 * @module views/logistics/sales/price-item/components
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
import SalesPriceScaleForm from './price-scale-form.vue'
import { useSalesPriceItemMasterContext } from '../composables/use-price-item-master-context'
import {
  getSalesPriceScaleList,
  getSalesPriceScaleById,
  createSalesPriceScale,
  updateSalesPriceScale,
  deleteSalesPriceScaleById,
  deleteSalesPriceScaleBatch,
  getSalesPriceScaleTemplate,
  importSalesPriceScale,
  exportSalesPriceScale,
} from '@/api/logistics/sales/price-scale'
import type { SalesPriceScale, SalesPriceScaleQuery } from '@/types/logistics/sales/price-scale'

import {
  useSalesPriceScaleI18n,
  SALESPRICESCALE_LIST_FIELDS,
  SALESPRICESCALE_QUERY_STRING_FIELDS,
  SALESPRICESCALE_QUERY_FIELDS,
  SALESPRICESCALE_SELF_I18N_KEY,
} from '../composables/use-price-scale-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesPriceScaleI18n()

const { t } = useI18n()
const { selectedMasterRow } = useSalesPriceItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesPriceScale')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
const dataSource = ref<SalesPriceScale[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SalesPriceScale | null>(null)
const selectedRows = ref<SalesPriceScale[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SalesPriceScale>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESPRICESCALE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESPRICESCALE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    startQuantity: undefined as number | undefined,
    endQuantity: undefined as number | undefined,
    scalePrice: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SALESPRICESCALE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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

const entityIdName = 'salesPriceScaleId'
const masterSalesPriceItemId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['salesPriceItemId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSalesPriceItemId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSalesPriceScaleId(record: SalesPriceScale | Record<string, unknown>): string {
  return String((record as SalesPriceScale)?.[entityIdName] ?? '')
}

function getSalesPriceScaleField(record: SalesPriceScale | Record<string, unknown>, field: string): unknown {
  return (record as SalesPriceScale)?.[field as keyof SalesPriceScale]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesPriceScaleId',
    key: 'salesPriceScaleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'salesPriceScaleId') ?? ''),
  },
  {
    title: pi.label('itemId'),
    dataIndex: 'itemId',
    key: 'itemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'itemId') ?? ''),
  },
  {
    title: pi.label('salesPriceCode'),
    dataIndex: 'salesPriceCode',
    key: 'salesPriceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'salesPriceCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('startQuantity'),
    dataIndex: 'startQuantity',
    key: 'startQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'startQuantity') ?? ''),
  },
  {
    title: pi.label('endQuantity'),
    dataIndex: 'endQuantity',
    key: 'endQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'endQuantity') ?? ''),
  },
  {
    title: pi.label('scalePrice'),
    dataIndex: 'scalePrice',
    key: 'scalePrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'scalePrice') ?? ''),
  },
  {
    title: pi.label('priceItem'),
    dataIndex: 'priceItem',
    key: 'priceItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesPriceScale }) =>
      String(getSalesPriceScaleField(record, 'priceItem') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:price:update',
        onClick: (record: SalesPriceScale) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:price:delete',
        onClick: (record: SalesPriceScale) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesPriceScale[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesPriceScale, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSalesPriceScaleId(selectedRow.value) === getSalesPriceScaleId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesPriceScale[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SalesPriceScale) {
  const key = getSalesPriceScaleId(record)
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
 * @returns {SalesPriceScaleQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesPriceScaleQuery>): SalesPriceScaleQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesPriceScaleQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    salesPriceItemId: masterSalesPriceItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesPriceScaleQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESPRICESCALE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.startQuantity !== undefined && form.startQuantity !== null) {
    query.startQuantity = form.startQuantity
  }
  if (form.endQuantity !== undefined && form.endQuantity !== null) {
    query.endQuantity = form.endQuantity
  }
  if (form.scalePrice !== undefined && form.scalePrice !== null) {
    query.scalePrice = form.scalePrice
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
    const res = await getSalesPriceScaleList(buildListQuery())
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
watch(masterSalesPriceItemId, () => {
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

async function handleEdit(record: SalesPriceScale) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getSalesPriceScaleById(getSalesPriceScaleId(record))
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
    const id = formData.value?.salesPriceScaleId
    if (id) {
      await updateSalesPriceScale(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesPriceScale(payload)
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

async function handleDeleteOne(record: SalesPriceScale) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesPriceScaleById(getSalesPriceScaleId(record))
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
      const ids = selectedRows.value.map((r) => getSalesPriceScaleId(r)).filter(Boolean)
      await deleteSalesPriceScaleBatch(ids)
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
  const res = await getSalesPriceScaleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesPriceScale(file, sheetName)
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
    const exportMeta = await exportSalesPriceScale(
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
