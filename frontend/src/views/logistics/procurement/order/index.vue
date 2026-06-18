<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购订单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchase-order">
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
      create-permission="logistics:materials:purchaseorder:create"
      update-permission="logistics:materials:purchaseorder:update"
      delete-permission="logistics:materials:purchaseorder:delete"
      import-permission="logistics:materials:purchaseorder:import"
      export-permission="logistics:materials:purchaseorder:export"
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
      :id-column-key="'purchaseOrderId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPurchaseOrderId"
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
            :value="getPurchaseOrderField(record, 'orderStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchaseOrderItem._self') }}</div>
          <a-table
            v-if="hasPurchaseOrderItemRows(record)"
            :columns="purchaseOrderItemExpandColumns"
            :data-source="getPurchaseOrderItemRows(record)"
            :row-key="(row: PurchaseOrderItem, index?: number) => row?.purchaseOrderItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchaseOrderChangeLog._self') }}</div>
          <a-table
            v-if="hasPurchaseOrderChangeLogRows(record)"
            :columns="purchaseOrderChangeLogExpandColumns"
            :data-source="getPurchaseOrderChangeLogRows(record)"
            :row-key="(row: PurchaseOrderChangeLog, index?: number) => row?.purchaseOrderChangeLogId || String(index ?? 0)"
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
      <PurchaseOrderForm
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
      :storage-key="'takt-query-fields-logistics-materials-purchase-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchaseOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="t('entity.purchaseOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.purchaseOrder.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.purchaseOrder.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateStart')">
      <a-form-item :label="t('entity.purchaseOrder.orderdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateEnd')">
      <a-form-item :label="t('entity.purchaseOrder.orderdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseOrder.requiredarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.requiredarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseOrder.requiredarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.requiredarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseOrder.actualarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.actualarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseOrder.actualarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.actualarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="t('entity.purchaseOrder.purchasegroup')">
        <a-input
          v-model:value="advancedQueryForm.purchaseGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.purchasegroup') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.purchaseOrder.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.purchaseOrder.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.purchaseOrder.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.purchaseOrder.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.purchaseOrder.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedQuantity')">
      <a-form-item :label="t('entity.purchaseOrder.receivedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.receivedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedAmount')">
      <a-form-item :label="t('entity.purchaseOrder.receivedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.receivedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paidAmount')">
      <a-form-item :label="t('entity.purchaseOrder.paidamount')">
        <a-input-number
          v-model:value="advancedQueryForm.paidAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.paidamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderStatus')">
      <a-form-item :label="t('entity.purchaseOrder.orderstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryStatus')">
      <a-form-item :label="t('entity.purchaseOrder.deliverystatus')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.deliverystatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentMethod')">
      <a-form-item :label="t('entity.purchaseOrder.paymentmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.paymentMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.paymentmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryMethod')">
      <a-form-item :label="t('entity.purchaseOrder.deliverymethod')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.deliverymethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryAddress')">
      <a-form-item :label="t('entity.purchaseOrder.deliveryaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.deliveryAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseOrder.deliveryaddress') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseOrder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseOrder._self"
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
      :id-column-key="'purchaseOrderId'"
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
 * Takt采购订单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/purchase-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PurchaseOrderForm from './components/purchase-order-form.vue'
import { getPurchaseOrderList, getPurchaseOrderById, createPurchaseOrder, updatePurchaseOrder, deletePurchaseOrderById, deletePurchaseOrderBatch, getPurchaseOrderTemplate, importPurchaseOrder, exportPurchaseOrder } from '@/api/logistics/materials/purchase-order'
import * as purchaseOrderItemApi from '@/api/logistics/materials/purchase-order-item'
import * as purchaseOrderChangeLogApi from '@/api/logistics/materials/purchase-order-change-log'
import type { PurchaseOrderItem, PurchaseOrderItemQuery } from '@/types/logistics/materials/purchase-order-item'
import type { PurchaseOrderChangeLog, PurchaseOrderChangeLogQuery } from '@/types/logistics/materials/purchase-order-change-log'
import type { PurchaseOrder, PurchaseOrderQuery, PurchaseOrderCreate, PurchaseOrderUpdate } from '@/types/logistics/materials/purchase-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseOrder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseOrder[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PurchaseOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<PurchaseOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchaseOrder>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  actualArrivalDateStart: '',
  actualArrivalDateEnd: '',
  purchaseGroup: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.purchaseOrder.plantcode') },
  { key: 'purchaseOrderCode', label: t('entity.purchaseOrder.code') },
  { key: 'supplierCode', label: t('entity.purchaseOrder.suppliercode') },
  { key: 'supplierName', label: t('entity.purchaseOrder.suppliername') },
  { key: 'orderDateStart', label: t('entity.purchaseOrder.orderdatestart') },
  { key: 'orderDateEnd', label: t('entity.purchaseOrder.orderdateend') },
  { key: 'requiredArrivalDateStart', label: t('entity.purchaseOrder.requiredarrivaldatestart') },
  { key: 'requiredArrivalDateEnd', label: t('entity.purchaseOrder.requiredarrivaldateend') },
  { key: 'actualArrivalDateStart', label: t('entity.purchaseOrder.actualarrivaldatestart') },
  { key: 'actualArrivalDateEnd', label: t('entity.purchaseOrder.actualarrivaldateend') },
  { key: 'purchaseGroup', label: t('entity.purchaseOrder.purchasegroup') },
  { key: 'totalQuantity', label: t('entity.purchaseOrder.totalquantity') },
  { key: 'totalAmount', label: t('entity.purchaseOrder.totalamount') },
  { key: 'discountAmount', label: t('entity.purchaseOrder.discountamount') },
  { key: 'taxAmount', label: t('entity.purchaseOrder.taxamount') },
  { key: 'actualAmount', label: t('entity.purchaseOrder.actualamount') },
  { key: 'receivedQuantity', label: t('entity.purchaseOrder.receivedquantity') },
  { key: 'receivedAmount', label: t('entity.purchaseOrder.receivedamount') },
  { key: 'paidAmount', label: t('entity.purchaseOrder.paidamount') },
  { key: 'orderStatus', label: t('entity.purchaseOrder.orderstatus') },
  { key: 'deliveryStatus', label: t('entity.purchaseOrder.deliverystatus') },
  { key: 'paymentMethod', label: t('entity.purchaseOrder.paymentmethod') },
  { key: 'deliveryMethod', label: t('entity.purchaseOrder.deliverymethod') },
  { key: 'deliveryAddress', label: t('entity.purchaseOrder.deliveryaddress') },
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
const entityIdName = 'purchaseOrderId'
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

/** 展开行预览：purchaseOrderItem 列 */
const purchaseOrderItemExpandColumns = computed(() => [
  {
    title: t('entity.purchaseOrderItem.purchaseordername'),
    dataIndex: 'purchaseOrderName',
    key: 'purchaseOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.purchaseordercode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.requestcode'),
    dataIndex: 'requestCode',
    key: 'requestCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.requestlinenumber'),
    dataIndex: 'requestLineNumber',
    key: 'requestLineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    ellipsis: true,
  },
])

/** 展开行预览：purchaseOrderChangeLog 列 */
const purchaseOrderChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.purchaseOrderChangeLog.purchaseordername'),
    dataIndex: 'purchaseOrderName',
    key: 'purchaseOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderChangeLog.ordercode'),
    dataIndex: 'orderCode',
    key: 'orderCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
])

/** 读取主表行上的 purchaseOrderItem 子表缓存 */
function getPurchaseOrderItemRows(record: PurchaseOrder): PurchaseOrderItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 purchaseOrderItem 子表 */
function hasPurchaseOrderItemRows(record: PurchaseOrder): boolean {
  return getPurchaseOrderItemRows(record).length > 0
}

/** 读取主表行上的 purchaseOrderChangeLog 子表缓存 */
function getPurchaseOrderChangeLogRows(record: PurchaseOrder): PurchaseOrderChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 purchaseOrderChangeLog 子表 */
function hasPurchaseOrderChangeLogRows(record: PurchaseOrder): boolean {
  return getPurchaseOrderChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadPurchaseOrderDetail(record: PurchaseOrder): Promise<PurchaseOrder | null> {
  const id = getPurchaseOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchaseOrderById(id)
    const index = dataSource.value.findIndex((row) => getPurchaseOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchaseOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 purchaseOrderItem 子表（PurchaseOrderItemQuery + purchaseOrderItemApi，与主表 PurchaseOrderQuery 分离） */
async function loadPurchaseOrderItemForPurchaseOrder(record: PurchaseOrder): Promise<PurchaseOrderItem[]> {
  const masterId = getPurchaseOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchaseOrderItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchaseOrderId: masterId,
    }
    const result = await purchaseOrderItemApi.getPurchaseOrderItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchaseOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as PurchaseOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 purchaseOrderChangeLog 子表（PurchaseOrderChangeLogQuery + purchaseOrderChangeLogApi，与主表 PurchaseOrderQuery 分离） */
async function loadPurchaseOrderChangeLogForPurchaseOrder(record: PurchaseOrder): Promise<PurchaseOrderChangeLog[]> {
  const masterId = getPurchaseOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchaseOrderChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchaseOrderId: masterId,
    }
    const result = await purchaseOrderChangeLogApi.getPurchaseOrderChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchaseOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as PurchaseOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensurePurchaseOrderChildrenLoaded(record: PurchaseOrder) {
  if (!hasPurchaseOrderItemRows(record)) {
    await loadPurchaseOrderItemForPurchaseOrder(record)
  }
  if (!hasPurchaseOrderChangeLogRows(record)) {
    await loadPurchaseOrderChangeLogForPurchaseOrder(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: PurchaseOrder) {
  const key = getPurchaseOrderId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensurePurchaseOrderChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseOrderId',
    key: 'purchaseOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseOrderId') ?? ''
  },
  {
    title: t('entity.purchaseOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchaseOrder.code'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: t('entity.purchaseOrder.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.purchaseOrder.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.purchaseOrder.orderdate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'orderDate') ?? ''
  },
  {
    title: t('entity.purchaseOrder.requiredarrivaldate'),
    dataIndex: 'requiredArrivalDate',
    key: 'requiredArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'requiredArrivalDate') ?? ''
  },
  {
    title: t('entity.purchaseOrder.actualarrivaldate'),
    dataIndex: 'actualArrivalDate',
    key: 'actualArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualArrivalDate') ?? ''
  },
  {
    title: t('entity.purchaseOrder.purchasegroup'),
    dataIndex: 'purchaseGroup',
    key: 'purchaseGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseGroup') ?? ''
  },
  {
    title: t('entity.purchaseOrder.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.purchaseOrder.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.discountamount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'discountAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.receivedquantity'),
    dataIndex: 'receivedQuantity',
    key: 'receivedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedQuantity') ?? ''
  },
  {
    title: t('entity.purchaseOrder.receivedamount'),
    dataIndex: 'receivedAmount',
    key: 'receivedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.paidamount'),
    dataIndex: 'paidAmount',
    key: 'paidAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'paidAmount') ?? ''
  },
  {
    title: t('entity.purchaseOrder.orderstatus'),
    dataIndex: 'orderStatus',
    key: 'orderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseOrder.deliverystatus'),
    dataIndex: 'deliveryStatus',
    key: 'deliveryStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'deliveryStatus') ?? ''
  },
  {
    title: t('entity.purchaseOrder.paymentmethod'),
    dataIndex: 'paymentMethod',
    key: 'paymentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'paymentMethod') ?? ''
  },
  {
    title: t('entity.purchaseOrder.deliverymethod'),
    dataIndex: 'deliveryMethod',
    key: 'deliveryMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'deliveryMethod') ?? ''
  },
  {
    title: t('entity.purchaseOrder.deliveryaddress'),
    dataIndex: 'deliveryAddress',
    key: 'deliveryAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'deliveryAddress') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:purchaseorder:update',
        onClick: (record: PurchaseOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:purchaseorder:delete',
        onClick: (record: PurchaseOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchaseOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseOrderField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchaseOrderId(selectedRow.value) === getPurchaseOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PurchaseOrder) => ({
  onClick: () => {
    const key = getPurchaseOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPurchaseOrderId(item)))
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
    const params: PurchaseOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPurchaseOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseOrder] 加载数据失败', { error })
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
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  actualArrivalDateStart: '',
  actualArrivalDateEnd: '',
  purchaseGroup: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseOrder._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseOrder._self') })
  formLoading.value = true
  try {
    const detail = await loadPurchaseOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseOrder._self') }))
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
      await updatePurchaseOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseOrder._self') }))
    } else {
      await createPurchaseOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseOrder._self') }))
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
  const res = await getPurchaseOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseOrder(file, sheetName)
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
    const exportQuery: PurchaseOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPurchaseOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseOrder._self') }))
  } catch (error: any) {
    logger.error('[PurchaseOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseOrder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseOrder._self'), name: t('common.tip.this.target', { target: t('entity.purchaseOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseOrder._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseOrder._self') }))
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
  purchaseOrderCode: '',
  supplierCode: '',
  supplierName: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  actualArrivalDateStart: '',
  actualArrivalDateEnd: '',
  purchaseGroup: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
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
.logistics-materials-purchase-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
