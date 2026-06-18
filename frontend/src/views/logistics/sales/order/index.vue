<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售订单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-sales-sales-order">
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
      create-permission="logistics:sales:salesorder:create"
      update-permission="logistics:sales:salesorder:update"
      delete-permission="logistics:sales:salesorder:delete"
      import-permission="logistics:sales:salesorder:import"
      export-permission="logistics:sales:salesorder:export"
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
      :id-column-key="'salesOrderId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalesOrderId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'orderStatus'">
          <TaktDictTag
            :value="getSalesOrderField(record, 'orderStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.salesOrderItem._self') }}</div>
          <a-table
            v-if="hasSalesOrderItemRows(record)"
            :columns="salesOrderItemExpandColumns"
            :data-source="getSalesOrderItemRows(record)"
            :row-key="(row: SalesOrderItem, index?: number) => row?.salesOrderItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.salesOrderChangeLog._self') }}</div>
          <a-table
            v-if="hasSalesOrderChangeLogRows(record)"
            :columns="salesOrderChangeLogExpandColumns"
            :data-source="getSalesOrderChangeLogRows(record)"
            :row-key="(row: SalesOrderChangeLog, index?: number) => row?.salesOrderChangeLogId || String(index ?? 0)"
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
      <SalesOrderForm
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
      :storage-key="'takt-query-fields-logistics-sales-sales-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.salesOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesOrderCode')">
      <a-form-item :label="t('entity.salesOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.salesOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.salesOrder.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.salesOrder.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateStart')">
      <a-form-item :label="t('entity.salesOrder.orderdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateEnd')">
      <a-form-item :label="t('entity.salesOrder.orderdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredDeliveryDateStart')">
      <a-form-item :label="t('entity.salesOrder.requireddeliverydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredDeliveryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.requireddeliverydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredDeliveryDateEnd')">
      <a-form-item :label="t('entity.salesOrder.requireddeliverydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredDeliveryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.requireddeliverydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualDeliveryDateStart')">
      <a-form-item :label="t('entity.salesOrder.actualdeliverydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualDeliveryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.actualdeliverydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualDeliveryDateEnd')">
      <a-form-item :label="t('entity.salesOrder.actualdeliverydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualDeliveryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.actualdeliverydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesBy')">
      <a-form-item :label="t('entity.salesOrder.salesby')">
        <a-input
          v-model:value="advancedQueryForm.salesBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.salesby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.salesOrder.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.salesOrder.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.salesOrder.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.salesOrder.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.salesOrder.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shippedQuantity')">
      <a-form-item :label="t('entity.salesOrder.shippedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.shippedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.shippedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shippedAmount')">
      <a-form-item :label="t('entity.salesOrder.shippedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.shippedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.shippedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedAmount')">
      <a-form-item :label="t('entity.salesOrder.receivedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.receivedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderStatus')">
      <a-form-item :label="t('entity.salesOrder.orderstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryStatus')">
      <a-form-item :label="t('entity.salesOrder.deliverystatus')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.deliverystatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryMethod')">
      <a-form-item :label="t('entity.salesOrder.deliverymethod')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.deliverymethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentMethod')">
      <a-form-item :label="t('entity.salesOrder.paymentmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.paymentMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.paymentmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryAddress')">
      <a-form-item :label="t('entity.salesOrder.deliveryaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.deliveryAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.salesOrder.deliveryaddress') })"
          :rows="2"
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
      :title="t('common.dialog.title.import', { entity: t('entity.salesOrder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salesOrder._self"
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
      :id-column-key="'salesOrderId'"
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
 * Takt销售订单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SalesOrderForm from './components/sales-order-form.vue'
import { getSalesOrderList, getSalesOrderById, createSalesOrder, updateSalesOrder, deleteSalesOrderById, deleteSalesOrderBatch, getSalesOrderTemplate, importSalesOrder, exportSalesOrder } from '@/api/logistics/sales/sales-order'
import * as salesOrderItemApi from '@/api/logistics/sales/sales-order-item'
import * as salesOrderChangeLogApi from '@/api/logistics/sales/sales-order-change-log'
import type { SalesOrderItem, SalesOrderItemQuery } from '@/types/logistics/sales/sales-order-item'
import type { SalesOrderChangeLog, SalesOrderChangeLogQuery } from '@/types/logistics/sales/sales-order-change-log'
import type { SalesOrder, SalesOrderQuery, SalesOrderCreate, SalesOrderUpdate } from '@/types/logistics/sales/sales-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salesOrder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesOrder[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesOrder>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  salesOrderCode: '',
  customerCode: '',
  customerName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredDeliveryDateStart: '',
  requiredDeliveryDateEnd: '',
  actualDeliveryDateStart: '',
  actualDeliveryDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  shippedQuantity: undefined as number | undefined,
  shippedAmount: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryAddress: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.salesOrder.plantcode') },
  { key: 'salesOrderCode', label: t('entity.salesOrder.code') },
  { key: 'customerCode', label: t('entity.salesOrder.customercode') },
  { key: 'customerName', label: t('entity.salesOrder.customername') },
  { key: 'orderDateStart', label: t('entity.salesOrder.orderdatestart') },
  { key: 'orderDateEnd', label: t('entity.salesOrder.orderdateend') },
  { key: 'requiredDeliveryDateStart', label: t('entity.salesOrder.requireddeliverydatestart') },
  { key: 'requiredDeliveryDateEnd', label: t('entity.salesOrder.requireddeliverydateend') },
  { key: 'actualDeliveryDateStart', label: t('entity.salesOrder.actualdeliverydatestart') },
  { key: 'actualDeliveryDateEnd', label: t('entity.salesOrder.actualdeliverydateend') },
  { key: 'salesBy', label: t('entity.salesOrder.salesby') },
  { key: 'totalQuantity', label: t('entity.salesOrder.totalquantity') },
  { key: 'totalAmount', label: t('entity.salesOrder.totalamount') },
  { key: 'discountAmount', label: t('entity.salesOrder.discountamount') },
  { key: 'taxAmount', label: t('entity.salesOrder.taxamount') },
  { key: 'actualAmount', label: t('entity.salesOrder.actualamount') },
  { key: 'shippedQuantity', label: t('entity.salesOrder.shippedquantity') },
  { key: 'shippedAmount', label: t('entity.salesOrder.shippedamount') },
  { key: 'receivedAmount', label: t('entity.salesOrder.receivedamount') },
  { key: 'orderStatus', label: t('entity.salesOrder.orderstatus') },
  { key: 'deliveryStatus', label: t('entity.salesOrder.deliverystatus') },
  { key: 'deliveryMethod', label: t('entity.salesOrder.deliverymethod') },
  { key: 'paymentMethod', label: t('entity.salesOrder.paymentmethod') },
  { key: 'deliveryAddress', label: t('entity.salesOrder.deliveryaddress') },
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
const entityIdName = 'salesOrderId'
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

/** 展开行预览：salesOrderItem 列 */
const salesOrderItemExpandColumns = computed(() => [
  {
    title: t('entity.salesOrderItem.salesordername'),
    dataIndex: 'salesOrderName',
    key: 'salesOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.salesordercode'),
    dataIndex: 'salesOrderCode',
    key: 'salesOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.salesunit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderItem.orderquantity'),
    dataIndex: 'orderQuantity',
    key: 'orderQuantity',
    ellipsis: true,
  },
])

/** 展开行预览：salesOrderChangeLog 列 */
const salesOrderChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.salesOrderChangeLog.salesordername'),
    dataIndex: 'salesOrderName',
    key: 'salesOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderChangeLog.ordercode'),
    dataIndex: 'orderCode',
    key: 'orderCode',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.salesOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
])

/** 读取主表行上的 salesOrderItem 子表缓存 */
function getSalesOrderItemRows(record: SalesOrder): SalesOrderItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 salesOrderItem 子表 */
function hasSalesOrderItemRows(record: SalesOrder): boolean {
  return getSalesOrderItemRows(record).length > 0
}

/** 读取主表行上的 salesOrderChangeLog 子表缓存 */
function getSalesOrderChangeLogRows(record: SalesOrder): SalesOrderChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 salesOrderChangeLog 子表 */
function hasSalesOrderChangeLogRows(record: SalesOrder): boolean {
  return getSalesOrderChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadSalesOrderDetail(record: SalesOrder): Promise<SalesOrder | null> {
  const id = getSalesOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesOrderById(id)
    const index = dataSource.value.findIndex((row) => getSalesOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 salesOrderItem 子表（SalesOrderItemQuery + salesOrderItemApi，与主表 SalesOrderQuery 分离） */
async function loadSalesOrderItemForSalesOrder(record: SalesOrder): Promise<SalesOrderItem[]> {
  const masterId = getSalesOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SalesOrderItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      salesOrderId: masterId,
    }
    const result = await salesOrderItemApi.getSalesOrderItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSalesOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as SalesOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 salesOrderChangeLog 子表（SalesOrderChangeLogQuery + salesOrderChangeLogApi，与主表 SalesOrderQuery 分离） */
async function loadSalesOrderChangeLogForSalesOrder(record: SalesOrder): Promise<SalesOrderChangeLog[]> {
  const masterId = getSalesOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SalesOrderChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      salesOrderId: masterId,
    }
    const result = await salesOrderChangeLogApi.getSalesOrderChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSalesOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as SalesOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureSalesOrderChildrenLoaded(record: SalesOrder) {
  if (!hasSalesOrderItemRows(record)) {
    await loadSalesOrderItemForSalesOrder(record)
  }
  if (!hasSalesOrderChangeLogRows(record)) {
    await loadSalesOrderChangeLogForSalesOrder(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: SalesOrder) {
  const key = getSalesOrderId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureSalesOrderChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesOrderId',
    key: 'salesOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'salesOrderId') ?? ''
  },
  {
    title: t('entity.salesOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.salesOrder.code'),
    dataIndex: 'salesOrderCode',
    key: 'salesOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'salesOrderCode') ?? ''
  },
  {
    title: t('entity.salesOrder.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.salesOrder.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.salesOrder.orderdate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'orderDate') ?? ''
  },
  {
    title: t('entity.salesOrder.requireddeliverydate'),
    dataIndex: 'requiredDeliveryDate',
    key: 'requiredDeliveryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'requiredDeliveryDate') ?? ''
  },
  {
    title: t('entity.salesOrder.actualdeliverydate'),
    dataIndex: 'actualDeliveryDate',
    key: 'actualDeliveryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'actualDeliveryDate') ?? ''
  },
  {
    title: t('entity.salesOrder.salesby'),
    dataIndex: 'salesBy',
    key: 'salesBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'salesBy') ?? ''
  },
  {
    title: t('entity.salesOrder.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.salesOrder.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.discountamount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'discountAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.shippedquantity'),
    dataIndex: 'shippedQuantity',
    key: 'shippedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'shippedQuantity') ?? ''
  },
  {
    title: t('entity.salesOrder.shippedamount'),
    dataIndex: 'shippedAmount',
    key: 'shippedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'shippedAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.receivedamount'),
    dataIndex: 'receivedAmount',
    key: 'receivedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'receivedAmount') ?? ''
  },
  {
    title: t('entity.salesOrder.orderstatus'),
    dataIndex: 'orderStatus',
    key: 'orderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salesOrder.deliverystatus'),
    dataIndex: 'deliveryStatus',
    key: 'deliveryStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'deliveryStatus') ?? ''
  },
  {
    title: t('entity.salesOrder.deliverymethod'),
    dataIndex: 'deliveryMethod',
    key: 'deliveryMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'deliveryMethod') ?? ''
  },
  {
    title: t('entity.salesOrder.paymentmethod'),
    dataIndex: 'paymentMethod',
    key: 'paymentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'paymentMethod') ?? ''
  },
  {
    title: t('entity.salesOrder.deliveryaddress'),
    dataIndex: 'deliveryAddress',
    key: 'deliveryAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesOrderField(record, 'deliveryAddress') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:salesorder:update',
        onClick: (record: SalesOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:salesorder:delete',
        onClick: (record: SalesOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesOrderField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSalesOrderId(selectedRow.value) === getSalesOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalesOrder) => ({
  onClick: () => {
    const key = getSalesOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSalesOrderId(item)))
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
    const params: SalesOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSalesOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesOrder] 加载数据失败', { error })
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
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  plantCode: '',
  salesOrderCode: '',
  customerCode: '',
  customerName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredDeliveryDateStart: '',
  requiredDeliveryDateEnd: '',
  actualDeliveryDateStart: '',
  actualDeliveryDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  shippedQuantity: undefined as number | undefined,
  shippedAmount: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryAddress: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salesOrder._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SalesOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salesOrder._self') })
  formLoading.value = true
  try {
    const detail = await loadSalesOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salesOrder._self') }))
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
      await updateSalesOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salesOrder._self') }))
    } else {
      await createSalesOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salesOrder._self') }))
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
  const res = await getSalesOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalesOrder(file, sheetName)
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
    const exportQuery: SalesOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSalesOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.salesOrder._self') }))
  } catch (error: any) {
    logger.error('[SalesOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salesOrder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salesOrder._self'), name: t('common.tip.this.target', { target: t('entity.salesOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salesOrder._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salesOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salesOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalesOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salesOrder._self') }))
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
  salesOrderCode: '',
  customerCode: '',
  customerName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredDeliveryDateStart: '',
  requiredDeliveryDateEnd: '',
  actualDeliveryDateStart: '',
  actualDeliveryDateEnd: '',
  salesBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  shippedQuantity: undefined as number | undefined,
  shippedAmount: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryAddress: '',
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
.logistics-sales-sales-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
