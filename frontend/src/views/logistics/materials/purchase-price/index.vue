<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-price -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购价格实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchase-price">
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
      create-permission="logistics:materials:purchaseprice:create"
      update-permission="logistics:materials:purchaseprice:update"
      delete-permission="logistics:materials:purchaseprice:delete"
      import-permission="logistics:materials:purchaseprice:import"
      export-permission="logistics:materials:purchaseprice:export"
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
      :id-column-key="'purchasePriceId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPurchasePriceId"
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
            :value="getPurchasePriceField(record, 'priceStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchasePriceItem._self') }}</div>
          <a-table
            v-if="hasPurchasePriceItemRows(record)"
            :columns="purchasePriceItemExpandColumns"
            :data-source="getPurchasePriceItemRows(record)"
            :row-key="(row: PurchasePriceItem, index?: number) => row?.purchasePriceItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchasePriceChangeLog._self') }}</div>
          <a-table
            v-if="hasPurchasePriceChangeLogRows(record)"
            :columns="purchasePriceChangeLogExpandColumns"
            :data-source="getPurchasePriceChangeLogRows(record)"
            :row-key="(row: PurchasePriceChangeLog, index?: number) => row?.purchasePriceChangeLogId || String(index ?? 0)"
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
      <PurchasePriceForm
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
      :storage-key="'takt-query-fields-logistics-materials-purchase-price'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchasePrice.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchasePrice.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasePriceCode')">
      <a-form-item :label="t('entity.purchasePrice.code')">
        <a-input
          v-model:value="advancedQueryForm.purchasePriceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchasePrice.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.purchasePrice.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchasePrice.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceType')">
      <a-form-item :label="t('entity.purchasePrice.pricetype')">
        <a-input-number
          v-model:value="advancedQueryForm.priceType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchasePrice.pricetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveStartDateStart')">
      <a-form-item :label="t('entity.purchasePrice.effectivestartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchasePrice.effectivestartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveStartDateEnd')">
      <a-form-item :label="t('entity.purchasePrice.effectivestartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchasePrice.effectivestartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveEndDateStart')">
      <a-form-item :label="t('entity.purchasePrice.effectiveenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchasePrice.effectiveenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveEndDateEnd')">
      <a-form-item :label="t('entity.purchasePrice.effectiveenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchasePrice.effectiveenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceStatus')">
      <a-form-item :label="t('entity.purchasePrice.pricestatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchasePrice.pricestatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchasePrice._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchasePrice._self"
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
      :id-column-key="'purchasePriceId'"
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
 * Takt采购价格实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/purchase-price
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PurchasePriceForm from './components/purchase-price-form.vue'
import { getPurchasePriceList, getPurchasePriceById, createPurchasePrice, updatePurchasePrice, deletePurchasePriceById, deletePurchasePriceBatch, getPurchasePriceTemplate, importPurchasePrice, exportPurchasePrice } from '@/api/logistics/materials/purchase-price'
import * as purchasePriceItemApi from '@/api/logistics/materials/purchase-price-item'
import * as purchasePriceChangeLogApi from '@/api/logistics/materials/purchase-price-change-log'
import type { PurchasePriceItem, PurchasePriceItemQuery } from '@/types/logistics/materials/purchase-price-item'
import type { PurchasePriceChangeLog, PurchasePriceChangeLogQuery } from '@/types/logistics/materials/purchase-price-change-log'
import type { PurchasePrice, PurchasePriceQuery, PurchasePriceCreate, PurchasePriceUpdate } from '@/types/logistics/materials/purchase-price'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchasePrice')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchasePrice._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchasePrice[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PurchasePrice | null>(null)
/** 表格多选行 */
const selectedRows = ref<PurchasePrice[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchasePrice>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchasePriceCode: '',
  supplierCode: '',
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
  { key: 'plantCode', label: t('entity.purchasePrice.plantcode') },
  { key: 'purchasePriceCode', label: t('entity.purchasePrice.code') },
  { key: 'supplierCode', label: t('entity.purchasePrice.suppliercode') },
  { key: 'priceType', label: t('entity.purchasePrice.pricetype') },
  { key: 'effectiveStartDateStart', label: t('entity.purchasePrice.effectivestartdatestart') },
  { key: 'effectiveStartDateEnd', label: t('entity.purchasePrice.effectivestartdateend') },
  { key: 'effectiveEndDateStart', label: t('entity.purchasePrice.effectiveenddatestart') },
  { key: 'effectiveEndDateEnd', label: t('entity.purchasePrice.effectiveenddateend') },
  { key: 'priceStatus', label: t('entity.purchasePrice.pricestatus') },
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
const entityIdName = 'purchasePriceId'
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

/** 展开行预览：purchasePriceItem 列 */
const purchasePriceItemExpandColumns = computed(() => [
  {
    title: t('entity.purchasePriceItem.purchasepricename'),
    dataIndex: 'purchasePriceName',
    key: 'purchasePriceName',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.purchasepricecode'),
    dataIndex: 'purchasePriceCode',
    key: 'purchasePriceCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.purchaseunit'),
    dataIndex: 'purchaseUnit',
    key: 'purchaseUnit',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceItem.purchaseprice'),
    dataIndex: 'purchasePrice',
    key: 'purchasePrice',
    ellipsis: true,
  },
])

/** 展开行预览：purchasePriceChangeLog 列 */
const purchasePriceChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.purchasePriceChangeLog.purchasepricename'),
    dataIndex: 'purchasePriceName',
    key: 'purchasePriceName',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.purchasePriceChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
])

/** 读取主表行上的 purchasePriceItem 子表缓存 */
function getPurchasePriceItemRows(record: PurchasePrice): PurchasePriceItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 purchasePriceItem 子表 */
function hasPurchasePriceItemRows(record: PurchasePrice): boolean {
  return getPurchasePriceItemRows(record).length > 0
}

/** 读取主表行上的 purchasePriceChangeLog 子表缓存 */
function getPurchasePriceChangeLogRows(record: PurchasePrice): PurchasePriceChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 purchasePriceChangeLog 子表 */
function hasPurchasePriceChangeLogRows(record: PurchasePrice): boolean {
  return getPurchasePriceChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadPurchasePriceDetail(record: PurchasePrice): Promise<PurchasePrice | null> {
  const id = getPurchasePriceId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchasePriceById(id)
    const index = dataSource.value.findIndex((row) => getPurchasePriceId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchasePrice
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 purchasePriceItem 子表（PurchasePriceItemQuery + purchasePriceItemApi，与主表 PurchasePriceQuery 分离） */
async function loadPurchasePriceItemForPurchasePrice(record: PurchasePrice): Promise<PurchasePriceItem[]> {
  const masterId = getPurchasePriceId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchasePriceItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchasePriceId: masterId,
    }
    const result = await purchasePriceItemApi.getPurchasePriceItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchasePriceId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as PurchasePrice
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 purchasePriceChangeLog 子表（PurchasePriceChangeLogQuery + purchasePriceChangeLogApi，与主表 PurchasePriceQuery 分离） */
async function loadPurchasePriceChangeLogForPurchasePrice(record: PurchasePrice): Promise<PurchasePriceChangeLog[]> {
  const masterId = getPurchasePriceId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchasePriceChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchasePriceId: masterId,
    }
    const result = await purchasePriceChangeLogApi.getPurchasePriceChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchasePriceId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as PurchasePrice
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensurePurchasePriceChildrenLoaded(record: PurchasePrice) {
  if (!hasPurchasePriceItemRows(record)) {
    await loadPurchasePriceItemForPurchasePrice(record)
  }
  if (!hasPurchasePriceChangeLogRows(record)) {
    await loadPurchasePriceChangeLogForPurchasePrice(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: PurchasePrice) {
  const key = getPurchasePriceId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensurePurchasePriceChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchasePriceId',
    key: 'purchasePriceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'purchasePriceId') ?? ''
  },
  {
    title: t('entity.purchasePrice.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchasePrice.code'),
    dataIndex: 'purchasePriceCode',
    key: 'purchasePriceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'purchasePriceCode') ?? ''
  },
  {
    title: t('entity.purchasePrice.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.purchasePrice.pricetype'),
    dataIndex: 'priceType',
    key: 'priceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'priceType') ?? ''
  },
  {
    title: t('entity.purchasePrice.effectivestartdate'),
    dataIndex: 'effectiveStartDate',
    key: 'effectiveStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'effectiveStartDate') ?? ''
  },
  {
    title: t('entity.purchasePrice.effectiveenddate'),
    dataIndex: 'effectiveEndDate',
    key: 'effectiveEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchasePriceField(record, 'effectiveEndDate') ?? ''
  },
  {
    title: t('entity.purchasePrice.pricestatus'),
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
        permission: 'logistics:materials:purchaseprice:update',
        onClick: (record: PurchasePrice) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:purchaseprice:delete',
        onClick: (record: PurchasePrice) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchasePriceId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchasePriceField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchasePrice[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchasePrice, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchasePriceId(selectedRow.value) === getPurchasePriceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchasePrice[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PurchasePrice) => ({
  onClick: () => {
    const key = getPurchasePriceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPurchasePriceId(item)))
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
    const params: PurchasePriceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPurchasePriceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchasePrice] 加载数据失败', { error })
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
  purchasePriceCode: '',
  supplierCode: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchasePrice._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchasePrice) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchasePrice._self') })
  formLoading.value = true
  try {
    const detail = await loadPurchasePriceDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchasePrice._self') }))
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
      await updatePurchasePrice(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchasePrice._self') }))
    } else {
      await createPurchasePrice(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchasePrice._self') }))
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
  const res = await getPurchasePriceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchasePrice(file, sheetName)
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
    const exportQuery: PurchasePriceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPurchasePrice(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchasePrice._self') }))
  } catch (error: any) {
    logger.error('[PurchasePrice] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchasePrice._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchasePrice) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchasePrice._self'), name: t('common.tip.this.target', { target: t('entity.purchasePrice._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchasePriceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchasePrice._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchasePrice._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchasePrice._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchasePriceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchasePrice._self') }))
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
  purchasePriceCode: '',
  supplierCode: '',
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
.logistics-materials-purchase-price {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
