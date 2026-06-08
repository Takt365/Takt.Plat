<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-quotation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售报价实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-sales-sales-quotation">
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
      create-permission="logistics:sales:salesquotation:create"
      update-permission="logistics:sales:salesquotation:update"
      delete-permission="logistics:sales:salesquotation:delete"
      import-permission="logistics:sales:salesquotation:import"
      export-permission="logistics:sales:salesquotation:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="true"
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

    <!-- 表格 -->
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'salesQuotationId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalesQuotationId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.salesQuotationItem._self') }}</div>
          <a-table
            v-if="hasSalesQuotationItemRows(record)"
            :columns="salesQuotationItemExpandColumns"
            :data-source="getSalesQuotationItemRows(record)"
            :row-key="(row: SalesQuotationItem, index?: number) => row?.salesQuotationItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
      </template>
    </TaktSingleTable>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <SalesQuotationForm
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
      :storage-key="'takt-query-fields-logistics-sales-sales-quotation'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.salesQuotation.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesQuotationCode')">
      <a-form-item :label="t('entity.salesQuotation.code')">
        <a-input
          v-model:value="advancedQueryForm.salesQuotationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.salesQuotation.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.salesQuotation.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.customername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationDateStart')">
      <a-form-item :label="t('entity.salesQuotation.quotationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.quotationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesQuotation.quotationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationDateEnd')">
      <a-form-item :label="t('entity.salesQuotation.quotationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.quotationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesQuotation.quotationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validUntilDateStart')">
      <a-form-item :label="t('entity.salesQuotation.validuntildatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validUntilDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesQuotation.validuntildatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validUntilDateEnd')">
      <a-form-item :label="t('entity.salesQuotation.validuntildateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validUntilDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesQuotation.validuntildateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesBy')">
      <a-form-item :label="t('entity.salesQuotation.salesby')">
        <a-input
          v-model:value="advancedQueryForm.salesBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.salesby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.salesQuotation.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.salesQuotation.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.salesQuotation.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.salesQuotation.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.salesQuotation.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationStatus')">
      <a-form-item :label="t('entity.salesQuotation.quotationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.quotationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.quotationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesOrderCode')">
      <a-form-item :label="t('entity.salesQuotation.salesordercode')">
        <a-input
          v-model:value="advancedQueryForm.salesOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesQuotation.salesordercode') })"
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
      <div v-show="isFieldVisible('extFieldJson')">
      <a-form-item :label="t('common.page.entity.extfieldjson')">
        <a-input
          v-model:value="advancedQueryForm.extFieldJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.salesQuotation._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salesQuotation._self"
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
      :id-column-key="'salesQuotationId'"
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
 * Takt销售报价实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-quotation
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SalesQuotationForm from './components/sales-quotation-form.vue'
import { getSalesQuotationList, getSalesQuotationById, createSalesQuotation, updateSalesQuotation, deleteSalesQuotationById, deleteSalesQuotationBatch, getSalesQuotationTemplate, importSalesQuotation, exportSalesQuotation } from '@/api/logistics/sales/sales-quotation'
import * as salesQuotationItemApi from '@/api/logistics/sales/sales-quotation-item'
import type { SalesQuotationItem, SalesQuotationItemQuery } from '@/types/logistics/sales/sales-quotation-item'
import type { SalesQuotation, SalesQuotationQuery, SalesQuotationCreate, SalesQuotationUpdate } from '@/types/logistics/sales/sales-quotation'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesQuotation')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salesQuotation._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesQuotation[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesQuotation | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesQuotation[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesQuotation>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  salesQuotationCode: '',
  customerCode: '',
  customerName: '',
  quotationDateStart: '',
  quotationDateEnd: '',
  validUntilDateStart: '',
  validUntilDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  quotationStatus: undefined as number | undefined,
  salesOrderCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.salesQuotation.plantcode') },
  { key: 'salesQuotationCode', label: t('entity.salesQuotation.code') },
  { key: 'customerCode', label: t('entity.salesQuotation.customercode') },
  { key: 'customerName', label: t('entity.salesQuotation.customername') },
  { key: 'quotationDateStart', label: t('entity.salesQuotation.quotationdatestart') },
  { key: 'quotationDateEnd', label: t('entity.salesQuotation.quotationdateend') },
  { key: 'validUntilDateStart', label: t('entity.salesQuotation.validuntildatestart') },
  { key: 'validUntilDateEnd', label: t('entity.salesQuotation.validuntildateend') },
  { key: 'salesBy', label: t('entity.salesQuotation.salesby') },
  { key: 'totalQuantity', label: t('entity.salesQuotation.totalquantity') },
  { key: 'totalAmount', label: t('entity.salesQuotation.totalamount') },
  { key: 'discountAmount', label: t('entity.salesQuotation.discountamount') },
  { key: 'taxAmount', label: t('entity.salesQuotation.taxamount') },
  { key: 'actualAmount', label: t('entity.salesQuotation.actualamount') },
  { key: 'quotationStatus', label: t('entity.salesQuotation.quotationstatus') },
  { key: 'salesOrderCode', label: t('entity.salesQuotation.salesordercode') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
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
const entityIdName = 'salesQuotationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：salesQuotationItem 列 */
const salesQuotationItemExpandColumns = computed(() => [
  {
    title: t('entity.salesQuotationItem.salesquotationname'),
    dataIndex: 'salesQuotationName',
    key: 'salesQuotationName',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.salesquotationcode'),
    dataIndex: 'salesQuotationCode',
    key: 'salesQuotationCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.salesunit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    ellipsis: true,
  },
  {
    title: t('entity.salesQuotationItem.quotationquantity'),
    dataIndex: 'quotationQuantity',
    key: 'quotationQuantity',
    ellipsis: true,
  },
])

/** 读取主表行上的 salesQuotationItem 子表缓存 */
function getSalesQuotationItemRows(record: SalesQuotation): SalesQuotationItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 salesQuotationItem 子表 */
function hasSalesQuotationItemRows(record: SalesQuotation): boolean {
  return getSalesQuotationItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadSalesQuotationDetail(record: SalesQuotation): Promise<SalesQuotation | null> {
  const id = getSalesQuotationId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesQuotationById(id)
    const index = dataSource.value.findIndex((row) => getSalesQuotationId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesQuotation
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 salesQuotationItem 子表（SalesQuotationItemQuery + salesQuotationItemApi，与主表 SalesQuotationQuery 分离） */
async function loadSalesQuotationItemForSalesQuotation(record: SalesQuotation): Promise<SalesQuotationItem[]> {
  const masterId = getSalesQuotationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SalesQuotationItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      salesQuotationId: masterId,
    }
    const result = await salesQuotationItemApi.getSalesQuotationItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSalesQuotationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as SalesQuotation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureSalesQuotationChildrenLoaded(record: SalesQuotation) {
  if (!hasSalesQuotationItemRows(record)) {
    await loadSalesQuotationItemForSalesQuotation(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: SalesQuotation) {
  const key = getSalesQuotationId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureSalesQuotationChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesQuotationId',
    key: 'salesQuotationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesQuotationId') ?? ''
  },
  {
    title: t('entity.salesQuotation.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.salesQuotation.code'),
    dataIndex: 'salesQuotationCode',
    key: 'salesQuotationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesQuotationCode') ?? ''
  },
  {
    title: t('entity.salesQuotation.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.salesQuotation.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.salesQuotation.quotationdate'),
    dataIndex: 'quotationDate',
    key: 'quotationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'quotationDate') ?? ''
  },
  {
    title: t('entity.salesQuotation.validuntildate'),
    dataIndex: 'validUntilDate',
    key: 'validUntilDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'validUntilDate') ?? ''
  },
  {
    title: t('entity.salesQuotation.salesby'),
    dataIndex: 'salesBy',
    key: 'salesBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesBy') ?? ''
  },
  {
    title: t('entity.salesQuotation.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.salesQuotation.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.salesQuotation.discountamount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'discountAmount') ?? ''
  },
  {
    title: t('entity.salesQuotation.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.salesQuotation.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.salesQuotation.quotationstatus'),
    dataIndex: 'quotationStatus',
    key: 'quotationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'quotationStatus') ?? ''
  },
  {
    title: t('entity.salesQuotation.salesordercode'),
    dataIndex: 'salesOrderCode',
    key: 'salesOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesOrderCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:salesquotation:update',
        onClick: (record: SalesQuotation) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:salesquotation:delete',
        onClick: (record: SalesQuotation) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesQuotationId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesQuotationField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesQuotation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesQuotation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSalesQuotationId(selectedRow.value) === getSalesQuotationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesQuotation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalesQuotation) => ({
  onClick: () => {
    const key = getSalesQuotationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSalesQuotationId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: SalesQuotationQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSalesQuotationList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesQuotation] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  plantCode: '',
  salesQuotationCode: '',
  customerCode: '',
  customerName: '',
  quotationDateStart: '',
  quotationDateEnd: '',
  validUntilDateStart: '',
  validUntilDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  quotationStatus: undefined as number | undefined,
  salesOrderCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salesQuotation._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SalesQuotation) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salesQuotation._self') })
  formLoading.value = true
  try {
    const detail = await loadSalesQuotationDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salesQuotation._self') }))
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
      await updateSalesQuotation(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salesQuotation._self') }))
    } else {
      await createSalesQuotation(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salesQuotation._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getSalesQuotationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalesQuotation(file, sheetName)
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: SalesQuotationQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSalesQuotation(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.salesQuotation._self') }))
  } catch (error: any) {
    logger.error('[SalesQuotation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salesQuotation._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesQuotation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salesQuotation._self'), name: t('common.tip.this.target', { target: t('entity.salesQuotation._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesQuotationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salesQuotation._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salesQuotation._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salesQuotation._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalesQuotationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salesQuotation._self') }))
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
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  salesQuotationCode: '',
  customerCode: '',
  customerName: '',
  quotationDateStart: '',
  quotationDateEnd: '',
  validUntilDateStart: '',
  validUntilDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  quotationStatus: undefined as number | undefined,
  salesOrderCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.logistics-sales-sales-quotation {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
