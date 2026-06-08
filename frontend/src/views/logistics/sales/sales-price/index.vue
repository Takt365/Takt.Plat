<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-price -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售价格实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-sales-sales-price">
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
      create-permission="logistics:sales:salesprice:create"
      update-permission="logistics:sales:salesprice:update"
      delete-permission="logistics:sales:salesprice:delete"
      import-permission="logistics:sales:salesprice:import"
      export-permission="logistics:sales:salesprice:export"
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
      :id-column-key="'salesPriceId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalesPriceId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'priceStatus'">
          <TaktDictTag
            :value="getSalesPriceField(record, 'priceStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.salesPriceItem._self') }}</div>
          <a-table
            v-if="hasSalesPriceItemRows(record)"
            :columns="salesPriceItemExpandColumns"
            :data-source="getSalesPriceItemRows(record)"
            :row-key="(row: SalesPriceItem, index?: number) => row?.salesPriceItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.salesPriceChangeLog._self') }}</div>
          <a-table
            v-if="hasSalesPriceChangeLogRows(record)"
            :columns="salesPriceChangeLogExpandColumns"
            :data-source="getSalesPriceChangeLogRows(record)"
            :row-key="(row: SalesPriceChangeLog, index?: number) => row?.salesPriceChangeLogId || String(index ?? 0)"
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
      <SalesPriceForm
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
      :storage-key="'takt-query-fields-logistics-sales-sales-price'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.salesPrice.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPrice.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPriceCode')">
      <a-form-item :label="t('entity.salesPrice.code')">
        <a-input
          v-model:value="advancedQueryForm.salesPriceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPrice.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.salesPrice.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPrice.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceType')">
      <a-form-item :label="t('entity.salesPrice.pricetype')">
        <a-input-number
          v-model:value="advancedQueryForm.priceType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPrice.pricetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveStartDateStart')">
      <a-form-item :label="t('entity.salesPrice.effectivestartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesPrice.effectivestartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveStartDateEnd')">
      <a-form-item :label="t('entity.salesPrice.effectivestartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesPrice.effectivestartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveEndDateStart')">
      <a-form-item :label="t('entity.salesPrice.effectiveenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesPrice.effectiveenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveEndDateEnd')">
      <a-form-item :label="t('entity.salesPrice.effectiveenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesPrice.effectiveenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceStatus')">
      <a-form-item :label="t('entity.salesPrice.pricestatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesPrice.pricestatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.salesPrice._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salesPrice._self"
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
      :id-column-key="'salesPriceId'"
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
 * Takt销售价格实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-price
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SalesPriceForm from './components/sales-price-form.vue'
import { getSalesPriceList, getSalesPriceById, createSalesPrice, updateSalesPrice, deleteSalesPriceById, deleteSalesPriceBatch, getSalesPriceTemplate, importSalesPrice, exportSalesPrice } from '@/api/logistics/sales/sales-price'
import * as salesPriceItemApi from '@/api/logistics/sales/sales-price-item'
import * as salesPriceChangeLogApi from '@/api/logistics/sales/sales-price-change-log'
import type { SalesPriceItem, SalesPriceItemQuery } from '@/types/logistics/sales/sales-price-item'
import type { SalesPriceChangeLog, SalesPriceChangeLogQuery } from '@/types/logistics/sales/sales-price-change-log'
import type { SalesPrice, SalesPriceQuery, SalesPriceCreate, SalesPriceUpdate } from '@/types/logistics/sales/sales-price'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesPrice')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salesPrice._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesPrice[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesPrice | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesPrice[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesPrice>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  salesPriceCode: '',
  customerCode: '',
  priceType: undefined as number | undefined,
  effectiveStartDateStart: '',
  effectiveStartDateEnd: '',
  effectiveEndDateStart: '',
  effectiveEndDateEnd: '',
  priceStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.salesPrice.plantcode') },
  { key: 'salesPriceCode', label: t('entity.salesPrice.code') },
  { key: 'customerCode', label: t('entity.salesPrice.customercode') },
  { key: 'priceType', label: t('entity.salesPrice.pricetype') },
  { key: 'effectiveStartDateStart', label: t('entity.salesPrice.effectivestartdatestart') },
  { key: 'effectiveStartDateEnd', label: t('entity.salesPrice.effectivestartdateend') },
  { key: 'effectiveEndDateStart', label: t('entity.salesPrice.effectiveenddatestart') },
  { key: 'effectiveEndDateEnd', label: t('entity.salesPrice.effectiveenddateend') },
  { key: 'priceStatus', label: t('entity.salesPrice.pricestatus') },
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
const entityIdName = 'salesPriceId'
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

/** 展开行预览：salesPriceItem 列 */
const salesPriceItemExpandColumns = computed(() => [
  {
    title: t('entity.salesPriceItem.salespricename'),
    dataIndex: 'salesPriceName',
    key: 'salesPriceName',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.salespricecode'),
    dataIndex: 'salesPriceCode',
    key: 'salesPriceCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.salesunit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.salesprice'),
    dataIndex: 'salesPrice',
    key: 'salesPrice',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.minorderquantity'),
    dataIndex: 'minOrderQuantity',
    key: 'minOrderQuantity',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceItem.maxorderquantity'),
    dataIndex: 'maxOrderQuantity',
    key: 'maxOrderQuantity',
    ellipsis: true,
  },
])

/** 展开行预览：salesPriceChangeLog 列 */
const salesPriceChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.salesPriceChangeLog.salespricename'),
    dataIndex: 'salesPriceName',
    key: 'salesPriceName',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.salesPriceChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
])

/** 读取主表行上的 salesPriceItem 子表缓存 */
function getSalesPriceItemRows(record: SalesPrice): SalesPriceItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 salesPriceItem 子表 */
function hasSalesPriceItemRows(record: SalesPrice): boolean {
  return getSalesPriceItemRows(record).length > 0
}

/** 读取主表行上的 salesPriceChangeLog 子表缓存 */
function getSalesPriceChangeLogRows(record: SalesPrice): SalesPriceChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 salesPriceChangeLog 子表 */
function hasSalesPriceChangeLogRows(record: SalesPrice): boolean {
  return getSalesPriceChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadSalesPriceDetail(record: SalesPrice): Promise<SalesPrice | null> {
  const id = getSalesPriceId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesPriceById(id)
    const index = dataSource.value.findIndex((row) => getSalesPriceId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesPrice
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 salesPriceItem 子表（SalesPriceItemQuery + salesPriceItemApi，与主表 SalesPriceQuery 分离） */
async function loadSalesPriceItemForSalesPrice(record: SalesPrice): Promise<SalesPriceItem[]> {
  const masterId = getSalesPriceId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SalesPriceItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      salesPriceId: masterId,
    }
    const result = await salesPriceItemApi.getSalesPriceItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSalesPriceId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as SalesPrice
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 salesPriceChangeLog 子表（SalesPriceChangeLogQuery + salesPriceChangeLogApi，与主表 SalesPriceQuery 分离） */
async function loadSalesPriceChangeLogForSalesPrice(record: SalesPrice): Promise<SalesPriceChangeLog[]> {
  const masterId = getSalesPriceId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SalesPriceChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      salesPriceId: masterId,
    }
    const result = await salesPriceChangeLogApi.getSalesPriceChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSalesPriceId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as SalesPrice
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureSalesPriceChildrenLoaded(record: SalesPrice) {
  if (!hasSalesPriceItemRows(record)) {
    await loadSalesPriceItemForSalesPrice(record)
  }
  if (!hasSalesPriceChangeLogRows(record)) {
    await loadSalesPriceChangeLogForSalesPrice(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: SalesPrice) {
  const key = getSalesPriceId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureSalesPriceChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesPriceId',
    key: 'salesPriceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'salesPriceId') ?? ''
  },
  {
    title: t('entity.salesPrice.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.salesPrice.code'),
    dataIndex: 'salesPriceCode',
    key: 'salesPriceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'salesPriceCode') ?? ''
  },
  {
    title: t('entity.salesPrice.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.salesPrice.pricetype'),
    dataIndex: 'priceType',
    key: 'priceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'priceType') ?? ''
  },
  {
    title: t('entity.salesPrice.effectivestartdate'),
    dataIndex: 'effectiveStartDate',
    key: 'effectiveStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'effectiveStartDate') ?? ''
  },
  {
    title: t('entity.salesPrice.effectiveenddate'),
    dataIndex: 'effectiveEndDate',
    key: 'effectiveEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesPriceField(record, 'effectiveEndDate') ?? ''
  },
  {
    title: t('entity.salesPrice.pricestatus'),
    dataIndex: 'priceStatus',
    key: 'priceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:salesprice:update',
        onClick: (record: SalesPrice) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:salesprice:delete',
        onClick: (record: SalesPrice) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesPriceId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesPriceField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesPrice[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesPrice, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSalesPriceId(selectedRow.value) === getSalesPriceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesPrice[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalesPrice) => ({
  onClick: () => {
    const key = getSalesPriceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSalesPriceId(item)))
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
    const params: SalesPriceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSalesPriceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesPrice] 加载数据失败', { error })
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
  salesPriceCode: '',
  customerCode: '',
  priceType: undefined as number | undefined,
  effectiveStartDateStart: '',
  effectiveStartDateEnd: '',
  effectiveEndDateStart: '',
  effectiveEndDateEnd: '',
  priceStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salesPrice._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SalesPrice) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salesPrice._self') })
  formLoading.value = true
  try {
    const detail = await loadSalesPriceDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salesPrice._self') }))
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
      await updateSalesPrice(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salesPrice._self') }))
    } else {
      await createSalesPrice(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salesPrice._self') }))
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
  const res = await getSalesPriceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalesPrice(file, sheetName)
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
    const exportQuery: SalesPriceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSalesPrice(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.salesPrice._self') }))
  } catch (error: any) {
    logger.error('[SalesPrice] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salesPrice._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesPrice) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salesPrice._self'), name: t('common.tip.this.target', { target: t('entity.salesPrice._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesPriceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salesPrice._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salesPrice._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salesPrice._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalesPriceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salesPrice._self') }))
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
  salesPriceCode: '',
  customerCode: '',
  priceType: undefined as number | undefined,
  effectiveStartDateStart: '',
  effectiveStartDateEnd: '',
  effectiveEndDateStart: '',
  effectiveEndDateEnd: '',
  priceStatus: undefined as number | undefined,
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
.logistics-sales-sales-price {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
