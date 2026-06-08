<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/serial/product-serial-outbound -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：产品序列号出库主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-serial-product-serial-outbound">
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
      create-permission="logistics:serial:productserialoutbound:create"
      update-permission="logistics:serial:productserialoutbound:update"
      delete-permission="logistics:serial:productserialoutbound:delete"
      import-permission="logistics:serial:productserialoutbound:import"
      export-permission="logistics:serial:productserialoutbound:export"
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
      :id-column-key="'productSerialOutboundId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getProductSerialOutboundId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.productSerialOutboundItem._self') }}</div>
          <a-table
            v-if="hasProductSerialOutboundItemRows(record)"
            :columns="productSerialOutboundItemExpandColumns"
            :data-source="getProductSerialOutboundItemRows(record)"
            :row-key="(row: ProductSerialOutboundItem, index?: number) => row?.productSerialOutboundItemId || String(index ?? 0)"
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
      <ProductSerialOutboundForm
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
      :storage-key="'takt-query-fields-logistics-serial-product-serial-outbound'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.productSerialOutbound.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundNo')">
      <a-form-item :label="t('entity.productSerialOutbound.outboundno')">
        <a-input
          v-model:value="advancedQueryForm.outboundNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.outboundno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shippingInvoiceNo')">
      <a-form-item :label="t('entity.productSerialOutbound.shippinginvoiceno')">
        <a-input
          v-model:value="advancedQueryForm.shippingInvoiceNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippinginvoiceno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundDateStart')">
      <a-form-item :label="t('entity.productSerialOutbound.outbounddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.outbounddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundDateEnd')">
      <a-form-item :label="t('entity.productSerialOutbound.outbounddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.outbounddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('destination')">
      <a-form-item :label="t('entity.productSerialOutbound.destination')">
        <a-input
          v-model:value="advancedQueryForm.destination"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destination') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shippingMethod')">
      <a-form-item :label="t('entity.productSerialOutbound.shippingmethod')">
        <a-input
          v-model:value="advancedQueryForm.shippingMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippingmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('destinationPort')">
      <a-form-item :label="t('entity.productSerialOutbound.destinationport')">
        <a-input
          v-model:value="advancedQueryForm.destinationPort"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destinationport') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundType')">
      <a-form-item :label="t('entity.productSerialOutbound.outboundtype')">
        <a-input-number
          v-model:value="advancedQueryForm.outboundType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.outboundtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="t('entity.productSerialOutbound.warehousecode')">
        <a-input
          v-model:value="advancedQueryForm.warehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.warehousecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('locationCode')">
      <a-form-item :label="t('entity.productSerialOutbound.locationcode')">
        <a-input
          v-model:value="advancedQueryForm.locationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.locationcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedCompany')">
      <a-form-item :label="t('entity.productSerialOutbound.relatedcompany')">
        <a-input
          v-model:value="advancedQueryForm.relatedCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.relatedcompany') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.productSerialOutbound.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.totalquantity') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.productSerialOutbound._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.productSerialOutbound._self"
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
      :id-column-key="'productSerialOutboundId'"
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
 * 产品序列号出库主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/serial/product-serial-outbound
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ProductSerialOutboundForm from './components/product-serial-outbound-form.vue'
import { getProductSerialOutboundList, getProductSerialOutboundById, createProductSerialOutbound, updateProductSerialOutbound, deleteProductSerialOutboundById, deleteProductSerialOutboundBatch, getProductSerialOutboundTemplate, importProductSerialOutbound, exportProductSerialOutbound } from '@/api/logistics/serial/product-serial-outbound'
import * as productSerialOutboundItemApi from '@/api/logistics/serial/product-serial-outbound-item'
import type { ProductSerialOutboundItem, ProductSerialOutboundItemQuery } from '@/types/logistics/serial/product-serial-outbound-item'
import type { ProductSerialOutbound, ProductSerialOutboundQuery, ProductSerialOutboundCreate, ProductSerialOutboundUpdate } from '@/types/logistics/serial/product-serial-outbound'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktProductSerialOutbound')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.productSerialOutbound._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ProductSerialOutbound[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ProductSerialOutbound | null>(null)
/** 表格多选行 */
const selectedRows = ref<ProductSerialOutbound[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ProductSerialOutbound>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  outboundNo: '',
  shippingInvoiceNo: '',
  outboundDateStart: '',
  outboundDateEnd: '',
  destination: '',
  shippingMethod: '',
  destinationPort: '',
  outboundType: undefined as number | undefined,
  warehouseCode: '',
  locationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.productSerialOutbound.plantcode') },
  { key: 'outboundNo', label: t('entity.productSerialOutbound.outboundno') },
  { key: 'shippingInvoiceNo', label: t('entity.productSerialOutbound.shippinginvoiceno') },
  { key: 'outboundDateStart', label: t('entity.productSerialOutbound.outbounddatestart') },
  { key: 'outboundDateEnd', label: t('entity.productSerialOutbound.outbounddateend') },
  { key: 'destination', label: t('entity.productSerialOutbound.destination') },
  { key: 'shippingMethod', label: t('entity.productSerialOutbound.shippingmethod') },
  { key: 'destinationPort', label: t('entity.productSerialOutbound.destinationport') },
  { key: 'outboundType', label: t('entity.productSerialOutbound.outboundtype') },
  { key: 'warehouseCode', label: t('entity.productSerialOutbound.warehousecode') },
  { key: 'locationCode', label: t('entity.productSerialOutbound.locationcode') },
  { key: 'relatedCompany', label: t('entity.productSerialOutbound.relatedcompany') },
  { key: 'totalQuantity', label: t('entity.productSerialOutbound.totalquantity') },
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
const entityIdName = 'productSerialOutboundId'
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

/** 展开行预览：productSerialOutboundItem 列 */
const productSerialOutboundItemExpandColumns = computed(() => [
  {
    title: t('entity.productSerialOutboundItem.outboundid'),
    dataIndex: 'outboundId',
    key: 'outboundId',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.outboundname'),
    dataIndex: 'outboundName',
    key: 'outboundName',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.outboundno'),
    dataIndex: 'outboundNo',
    key: 'outboundNo',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.outboundserialno'),
    dataIndex: 'outboundSerialNo',
    key: 'outboundSerialNo',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.referenceinboundid'),
    dataIndex: 'referenceInboundId',
    key: 'referenceInboundId',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.referenceinboundname'),
    dataIndex: 'referenceInboundName',
    key: 'referenceInboundName',
    ellipsis: true,
  },
  {
    title: t('entity.productSerialOutboundItem.referenceinboundno'),
    dataIndex: 'referenceInboundNo',
    key: 'referenceInboundNo',
    ellipsis: true,
  },
])

/** 读取主表行上的 productSerialOutboundItem 子表缓存 */
function getProductSerialOutboundItemRows(record: ProductSerialOutbound): ProductSerialOutboundItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 productSerialOutboundItem 子表 */
function hasProductSerialOutboundItemRows(record: ProductSerialOutbound): boolean {
  return getProductSerialOutboundItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadProductSerialOutboundDetail(record: ProductSerialOutbound): Promise<ProductSerialOutbound | null> {
  const id = getProductSerialOutboundId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getProductSerialOutboundById(id)
    const index = dataSource.value.findIndex((row) => getProductSerialOutboundId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ProductSerialOutbound
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 productSerialOutboundItem 子表（ProductSerialOutboundItemQuery + productSerialOutboundItemApi，与主表 ProductSerialOutboundQuery 分离） */
async function loadProductSerialOutboundItemForProductSerialOutbound(record: ProductSerialOutbound): Promise<ProductSerialOutboundItem[]> {
  const masterId = getProductSerialOutboundId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ProductSerialOutboundItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      productSerialOutboundId: masterId,
    }
    const result = await productSerialOutboundItemApi.getProductSerialOutboundItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getProductSerialOutboundId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as ProductSerialOutbound
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureProductSerialOutboundChildrenLoaded(record: ProductSerialOutbound) {
  if (!hasProductSerialOutboundItemRows(record)) {
    await loadProductSerialOutboundItemForProductSerialOutbound(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: ProductSerialOutbound) {
  const key = getProductSerialOutboundId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureProductSerialOutboundChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'productSerialOutboundId',
    key: 'productSerialOutboundId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'productSerialOutboundId') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.outboundno'),
    dataIndex: 'outboundNo',
    key: 'outboundNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'outboundNo') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.shippinginvoiceno'),
    dataIndex: 'shippingInvoiceNo',
    key: 'shippingInvoiceNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'shippingInvoiceNo') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.outbounddate'),
    dataIndex: 'outboundDate',
    key: 'outboundDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'outboundDate') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.destination'),
    dataIndex: 'destination',
    key: 'destination',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'destination') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.shippingmethod'),
    dataIndex: 'shippingMethod',
    key: 'shippingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'shippingMethod') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.destinationport'),
    dataIndex: 'destinationPort',
    key: 'destinationPort',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'destinationPort') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.outboundtype'),
    dataIndex: 'outboundType',
    key: 'outboundType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'outboundType') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.warehousecode'),
    dataIndex: 'warehouseCode',
    key: 'warehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'warehouseCode') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.locationcode'),
    dataIndex: 'locationCode',
    key: 'locationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'locationCode') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.relatedcompany'),
    dataIndex: 'relatedCompany',
    key: 'relatedCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'relatedCompany') ?? ''
  },
  {
    title: t('entity.productSerialOutbound.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProductSerialOutboundField(record, 'totalQuantity') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:serial:productserialoutbound:update',
        onClick: (record: ProductSerialOutbound) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:serial:productserialoutbound:delete',
        onClick: (record: ProductSerialOutbound) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getProductSerialOutboundId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getProductSerialOutboundField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ProductSerialOutbound[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ProductSerialOutbound, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getProductSerialOutboundId(selectedRow.value) === getProductSerialOutboundId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ProductSerialOutbound[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ProductSerialOutbound) => ({
  onClick: () => {
    const key = getProductSerialOutboundId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getProductSerialOutboundId(item)))
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
    const params: ProductSerialOutboundQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getProductSerialOutboundList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ProductSerialOutbound] 加载数据失败', { error })
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
  outboundNo: '',
  shippingInvoiceNo: '',
  outboundDateStart: '',
  outboundDateEnd: '',
  destination: '',
  shippingMethod: '',
  destinationPort: '',
  outboundType: undefined as number | undefined,
  warehouseCode: '',
  locationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.productSerialOutbound._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ProductSerialOutbound) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.productSerialOutbound._self') })
  formLoading.value = true
  try {
    const detail = await loadProductSerialOutboundDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.productSerialOutbound._self') }))
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
      await updateProductSerialOutbound(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.productSerialOutbound._self') }))
    } else {
      await createProductSerialOutbound(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.productSerialOutbound._self') }))
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
  const res = await getProductSerialOutboundTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importProductSerialOutbound(file, sheetName)
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
    const exportQuery: ProductSerialOutboundQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportProductSerialOutbound(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.productSerialOutbound._self') }))
  } catch (error: any) {
    logger.error('[ProductSerialOutbound] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.productSerialOutbound._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ProductSerialOutbound) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.productSerialOutbound._self'), name: t('common.tip.this.target', { target: t('entity.productSerialOutbound._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProductSerialOutboundById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.productSerialOutbound._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.productSerialOutbound._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.productSerialOutbound._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteProductSerialOutboundBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.productSerialOutbound._self') }))
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
  outboundNo: '',
  shippingInvoiceNo: '',
  outboundDateStart: '',
  outboundDateEnd: '',
  destination: '',
  shippingMethod: '',
  destinationPort: '',
  outboundType: undefined as number | undefined,
  warehouseCode: '',
  locationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
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
.logistics-serial-product-serial-outbound {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
