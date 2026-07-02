<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-invoice -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购发票实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:procurement:purchase:invoice:create"
      update-permission="logistics:procurement:purchase:invoice:update"
      delete-permission="logistics:procurement:purchase:invoice:delete"
      import-permission="logistics:procurement:purchase:invoice:import"
      export-permission="logistics:procurement:purchase:invoice:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getPurchaseInvoiceId"
      :master-row-selection="rowSelection"
      master-id-column-key="purchaseInvoiceId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'invoiceStatus'">
          <TaktDictTag
            :value="getPurchaseInvoiceField(record, 'invoiceStatus')"
            dict-type="logistics_invoice_status"
          />
        </template>
        <template v-else-if="column.key === 'paymentMethod'">
          <TaktDictTag
            :value="getPurchaseInvoiceField(record, 'paymentMethod')"
            dict-type="accounting_payment_method_type"
          />
        </template>
      </template>
      <template #detail>
        <PurchaseInvoiceItemPanel
          ref="purchaseInvoiceItemPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <PurchaseInvoiceForm
        :key="formData?.purchaseInvoiceId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-procurement-purchase-invoice'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchaseinvoice.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.plantcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInvoiceCode')">
      <a-form-item :label="t('entity.purchaseinvoice.code')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInvoiceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="t('entity.purchaseinvoice.purchaseordercode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.purchaseordercode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.purchaseinvoice.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.suppliercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.purchaseinvoice.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.suppliername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('invoiceDateStart')">
      <a-form-item :label="t('entity.purchaseinvoice.invoicedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.invoiceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinvoice.invoicedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('invoiceDateEnd')">
      <a-form-item :label="t('entity.purchaseinvoice.invoicedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.invoiceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinvoice.invoicedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.purchaseinvoice.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.purchaseinvoice.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.purchaseinvoice.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paidAmount')">
      <a-form-item :label="t('entity.purchaseinvoice.paidamount')">
        <a-input-number
          v-model:value="advancedQueryForm.paidAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.paidamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('invoiceStatus')">
      <a-form-item :label="t('entity.purchaseinvoice.invoicestatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.invoiceStatus"
          dict-type="logistics_invoice_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinvoice.invoicestatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentMethod')">
      <a-form-item :label="t('entity.purchaseinvoice.paymentmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentMethod"
          dict-type="accounting_payment_method_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinvoice.paymentmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxInvoiceNo')">
      <a-form-item :label="t('entity.purchaseinvoice.taxinvoiceno')">
        <a-input
          v-model:value="advancedQueryForm.taxInvoiceNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinvoice.taxinvoiceno') })"
          show-count
          :maxlength="50"
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

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseinvoice._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseinvoice._self"
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'purchaseInvoiceId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购发票实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-invoice
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import PurchaseInvoiceForm from './components/purchase-invoice-form.vue'
import PurchaseInvoiceItemPanel from './components/purchase-invoice-item-panel.vue'
import { providePurchaseInvoiceMasterContext } from './composables/use-purchase-invoice-master-context'
import { getPurchaseInvoiceList, getPurchaseInvoiceById, createPurchaseInvoice, updatePurchaseInvoice, deletePurchaseInvoiceById, deletePurchaseInvoiceBatch, getPurchaseInvoiceTemplate, importPurchaseInvoice, exportPurchaseInvoice, updatePurchaseInvoiceStatus } from '@/api/logistics/procurement/purchase-invoice'
import type { PurchaseInvoice, PurchaseInvoiceQuery } from '@/types/logistics/procurement/purchase-invoice'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseInvoice')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseinvoice._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseInvoice[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PurchaseInvoice | null>(null)
/** 表格多选行 */
const selectedRows = ref<PurchaseInvoice[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchaseInvoice> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchaseInvoiceCode: '',
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  invoiceDateStart: '',
  invoiceDateEnd: '',
  totalAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  invoiceStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  taxInvoiceNo: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.purchaseinvoice.plantcode') },
  { key: 'purchaseInvoiceCode', label: t('entity.purchaseinvoice.code') },
  { key: 'purchaseOrderCode', label: t('entity.purchaseinvoice.purchaseordercode') },
  { key: 'supplierCode', label: t('entity.purchaseinvoice.suppliercode') },
  { key: 'supplierName', label: t('entity.purchaseinvoice.suppliername') },
  { key: 'invoiceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseinvoice.invoicedate')) },
  { key: 'invoiceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseinvoice.invoicedate')) },
  { key: 'totalAmount', label: t('entity.purchaseinvoice.totalamount') },
  { key: 'taxAmount', label: t('entity.purchaseinvoice.taxamount') },
  { key: 'actualAmount', label: t('entity.purchaseinvoice.actualamount') },
  { key: 'paidAmount', label: t('entity.purchaseinvoice.paidamount') },
  { key: 'invoiceStatus', label: t('entity.purchaseinvoice.invoicestatus') },
  { key: 'paymentMethod', label: t('entity.purchaseinvoice.paymentmethod') },
  { key: 'taxInvoiceNo', label: t('entity.purchaseinvoice.taxinvoiceno') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'purchaseInvoiceId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = providePurchaseInvoiceMasterContext()
const purchaseInvoiceItemPanelRef = ref<InstanceType<typeof PurchaseInvoiceItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PurchaseInvoiceQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseInvoiceQuery>): PurchaseInvoiceQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseInvoiceQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseInvoiceQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('purchaseInvoiceCode', form.purchaseInvoiceCode)
  assignTrimmed('purchaseOrderCode', form.purchaseOrderCode)
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('supplierName', form.supplierName)
  assignTrimmed('invoiceDateStart', form.invoiceDateStart)
  assignTrimmed('invoiceDateEnd', form.invoiceDateEnd)
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    query.totalAmount = form.totalAmount
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    query.actualAmount = form.actualAmount
  }
  if (form.paidAmount !== undefined && form.paidAmount !== null) {
    query.paidAmount = form.paidAmount
  }
  if (form.invoiceStatus !== undefined && form.invoiceStatus !== null) {
    query.invoiceStatus = form.invoiceStatus
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    query.paymentMethod = form.paymentMethod
  }
  assignTrimmed('taxInvoiceNo', form.taxInvoiceNo)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: PurchaseInvoice | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getPurchaseInvoiceId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as PurchaseInvoice
  const key = getPurchaseInvoiceId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadPurchaseInvoiceDetail(record: PurchaseInvoice): Promise<PurchaseInvoice | null> {
  const id = getPurchaseInvoiceId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchaseInvoiceById(id)
    const index = dataSource.value.findIndex((row) => getPurchaseInvoiceId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchaseInvoice
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseInvoiceId',
    key: 'purchaseInvoiceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'purchaseInvoiceId') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.code'),
    dataIndex: 'purchaseInvoiceCode',
    key: 'purchaseInvoiceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'purchaseInvoiceCode') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.purchaseordercode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.invoicedate'),
    dataIndex: 'invoiceDate',
    key: 'invoiceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'invoiceDate') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.paidamount'),
    dataIndex: 'paidAmount',
    key: 'paidAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'paidAmount') ?? ''
  },
  {
    title: t('entity.purchaseinvoice.invoicestatus'),
    dataIndex: 'invoiceStatus',
    key: 'invoiceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseinvoice.paymentmethod'),
    dataIndex: 'paymentMethod',
    key: 'paymentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseinvoice.taxinvoiceno'),
    dataIndex: 'taxInvoiceNo',
    key: 'taxInvoiceNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInvoiceField(record, 'taxInvoiceNo') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:invoice:update',
        onClick: (record: PurchaseInvoice) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:invoice:delete',
        onClick: (record: PurchaseInvoice) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchaseInvoiceId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseInvoiceField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseInvoice[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: PurchaseInvoice, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getPurchaseInvoiceId(selectedRow.value) === getPurchaseInvoiceId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseInvoice[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getPurchaseInvoiceList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseInvoice] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  plantCode: '',
  purchaseInvoiceCode: '',
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  invoiceDateStart: '',
  invoiceDateEnd: '',
  totalAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  invoiceStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  taxInvoiceNo: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseinvoice._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseInvoice) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseinvoice._self') })
  formLoading.value = true
  try {
    const detail = await loadPurchaseInvoiceDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseinvoice._self') }))
  }
}
/** 提交新增/编辑表单 */
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
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updatePurchaseInvoice(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseinvoice._self') }))
    } else {
      await createPurchaseInvoice(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseinvoice._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  purchaseInvoiceItemPanelRef.value?.reload?.()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getPurchaseInvoiceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseInvoice(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportPurchaseInvoice(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseinvoice._self') }))
  } catch (error: any) {
    logger.error('[PurchaseInvoice] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseinvoice._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseInvoice) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseinvoice._self'), name: t('common.tip.this.target', { target: t('entity.purchaseinvoice._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseInvoiceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinvoice._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseinvoice._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseinvoice._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseInvoiceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinvoice._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  purchaseInvoiceCode: '',
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  invoiceDateStart: '',
  invoiceDateEnd: '',
  totalAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  invoiceStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  taxInvoiceNo: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
</script>
