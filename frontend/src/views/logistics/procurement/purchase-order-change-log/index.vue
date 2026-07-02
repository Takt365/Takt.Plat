<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-order-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购订单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:procurement:purchase:order:create"
      update-permission="logistics:procurement:purchase:order:update"
      delete-permission="logistics:procurement:purchase:order:delete"
      import-permission="logistics:procurement:purchase:order:import"
      export-permission="logistics:procurement:purchase:order:export"
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
      :master-row-key="getPurchaseOrderId"
      :master-row-selection="rowSelection"
      master-id-column-key="purchaseOrderId"
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
        <template v-if="column.key === 'orderStatus'">
          <a-switch
            :checked="getPurchaseOrderField(record, 'orderStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleOrderStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'paymentMethod'">
          <TaktDictTag
            :value="getPurchaseOrderField(record, 'paymentMethod')"
            dict-type="accounting_payment_method_type"
          />
        </template>
        <template v-else-if="column.key === 'deliveryMethod'">
          <TaktDictTag
            :value="getPurchaseOrderField(record, 'deliveryMethod')"
            dict-type="logistics_delivery_method_type"
          />
        </template>
        <template v-else-if="column.key === 'deliveryStatus'">
          <TaktDictTag
            :value="getPurchaseOrderField(record, 'deliveryStatus')"
            dict-type="logistics_delivery_status"
          />
        </template>
      </template>
      <template #detail>
        <PurchaseOrderChangeLogPanel
          ref="purchaseOrderChangeLogPanelRef"
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
      <PurchaseOrderForm
        :key="formData?.purchaseOrderId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-procurement-purchase-order-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchaseorder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.plantcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="t('entity.purchaseorder.code')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.code') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestId')">
      <a-form-item :label="t('entity.purchaseorder.purchaserequestid')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.purchaserequestid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestCode')">
      <a-form-item :label="t('entity.purchaseorder.purchaserequestcode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.purchaserequestcode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.purchaseorder.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.suppliercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.purchaseorder.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.suppliername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateStart')">
      <a-form-item :label="t('entity.purchaseorder.orderdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.orderdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateEnd')">
      <a-form-item :label="t('entity.purchaseorder.orderdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.orderdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseorder.requiredarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.requiredarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseorder.requiredarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.requiredarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseorder.actualarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.actualarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseorder.actualarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.actualarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="t('entity.purchaseorder.purchasegroup')">
        <a-input
          v-model:value="advancedQueryForm.purchaseGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.purchasegroup') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.purchaseorder.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.purchaseorder.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.purchaseorder.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.purchaseorder.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.purchaseorder.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedQuantity')">
      <a-form-item :label="t('entity.purchaseorder.receivedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.receivedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedAmount')">
      <a-form-item :label="t('entity.purchaseorder.receivedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.receivedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paidAmount')">
      <a-form-item :label="t('entity.purchaseorder.paidamount')">
        <a-input-number
          v-model:value="advancedQueryForm.paidAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseorder.paidamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentMethod')">
      <a-form-item :label="t('entity.purchaseorder.paymentmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentMethod"
          dict-type="accounting_payment_method_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.paymentmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryMethod')">
      <a-form-item :label="t('entity.purchaseorder.deliverymethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.deliveryMethod"
          dict-type="logistics_delivery_method_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.deliverymethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryAddress')">
      <a-form-item :label="t('entity.purchaseorder.deliveryaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.deliveryAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseorder.deliveryaddress') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderStatus')">
      <a-form-item :label="t('entity.purchaseorder.orderstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.orderstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryStatus')">
      <a-form-item :label="t('entity.purchaseorder.deliverystatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.deliveryStatus"
          dict-type="logistics_delivery_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseorder.deliverystatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseorder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseorder._self"
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
 * @module views/logistics/procurement/purchase-order-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import PurchaseOrderForm from './components/purchase-order-form.vue'
import PurchaseOrderChangeLogPanel from './components/purchase-order-change-log-panel.vue'
import { providePurchaseOrderMasterContext } from './composables/use-purchase-order-master-context'
import { getPurchaseOrderList, getPurchaseOrderById, createPurchaseOrder, updatePurchaseOrder, deletePurchaseOrderById, deletePurchaseOrderBatch, getPurchaseOrderTemplate, importPurchaseOrder, exportPurchaseOrder, updatePurchaseOrderStatus } from '@/api/logistics/procurement/purchase-order'
import type { PurchaseOrder, PurchaseOrderQuery } from '@/types/logistics/procurement/purchase-order'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseorder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseOrder[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<PurchaseOrder> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchaseOrderCode: '',
  purchaseRequestId: '',
  purchaseRequestCode: '',
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
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.purchaseorder.plantcode') },
  { key: 'purchaseOrderCode', label: t('entity.purchaseorder.code') },
  { key: 'purchaseRequestId', label: t('entity.purchaseorder.purchaserequestid') },
  { key: 'purchaseRequestCode', label: t('entity.purchaseorder.purchaserequestcode') },
  { key: 'supplierCode', label: t('entity.purchaseorder.suppliercode') },
  { key: 'supplierName', label: t('entity.purchaseorder.suppliername') },
  { key: 'orderDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.orderdate')) },
  { key: 'orderDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.orderdate')) },
  { key: 'requiredArrivalDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.requiredarrivaldate')) },
  { key: 'requiredArrivalDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.requiredarrivaldate')) },
  { key: 'actualArrivalDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.actualarrivaldate')) },
  { key: 'actualArrivalDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseorder.actualarrivaldate')) },
  { key: 'purchaseGroup', label: t('entity.purchaseorder.purchasegroup') },
  { key: 'totalQuantity', label: t('entity.purchaseorder.totalquantity') },
  { key: 'totalAmount', label: t('entity.purchaseorder.totalamount') },
  { key: 'discountAmount', label: t('entity.purchaseorder.discountamount') },
  { key: 'taxAmount', label: t('entity.purchaseorder.taxamount') },
  { key: 'actualAmount', label: t('entity.purchaseorder.actualamount') },
  { key: 'receivedQuantity', label: t('entity.purchaseorder.receivedquantity') },
  { key: 'receivedAmount', label: t('entity.purchaseorder.receivedamount') },
  { key: 'paidAmount', label: t('entity.purchaseorder.paidamount') },
  { key: 'paymentMethod', label: t('entity.purchaseorder.paymentmethod') },
  { key: 'deliveryMethod', label: t('entity.purchaseorder.deliverymethod') },
  { key: 'deliveryAddress', label: t('entity.purchaseorder.deliveryaddress') },
  { key: 'orderStatus', label: t('entity.purchaseorder.orderstatus') },
  { key: 'deliveryStatus', label: t('entity.purchaseorder.deliverystatus') },
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
const entityIdName = 'purchaseOrderId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = providePurchaseOrderMasterContext()
const purchaseOrderChangeLogPanelRef = ref<InstanceType<typeof PurchaseOrderChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PurchaseOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseOrderQuery>): PurchaseOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('purchaseOrderCode', form.purchaseOrderCode)
  assignTrimmed('purchaseRequestId', form.purchaseRequestId)
  assignTrimmed('purchaseRequestCode', form.purchaseRequestCode)
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('supplierName', form.supplierName)
  assignTrimmed('orderDateStart', form.orderDateStart)
  assignTrimmed('orderDateEnd', form.orderDateEnd)
  assignTrimmed('requiredArrivalDateStart', form.requiredArrivalDateStart)
  assignTrimmed('requiredArrivalDateEnd', form.requiredArrivalDateEnd)
  assignTrimmed('actualArrivalDateStart', form.actualArrivalDateStart)
  assignTrimmed('actualArrivalDateEnd', form.actualArrivalDateEnd)
  assignTrimmed('purchaseGroup', form.purchaseGroup)
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    query.totalQuantity = form.totalQuantity
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    query.totalAmount = form.totalAmount
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    query.actualAmount = form.actualAmount
  }
  if (form.receivedQuantity !== undefined && form.receivedQuantity !== null) {
    query.receivedQuantity = form.receivedQuantity
  }
  if (form.receivedAmount !== undefined && form.receivedAmount !== null) {
    query.receivedAmount = form.receivedAmount
  }
  if (form.paidAmount !== undefined && form.paidAmount !== null) {
    query.paidAmount = form.paidAmount
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    query.paymentMethod = form.paymentMethod
  }
  if (form.deliveryMethod !== undefined && form.deliveryMethod !== null) {
    query.deliveryMethod = form.deliveryMethod
  }
  assignTrimmed('deliveryAddress', form.deliveryAddress)
  if (form.orderStatus !== undefined && form.orderStatus !== null) {
    query.orderStatus = form.orderStatus
  }
  if (form.deliveryStatus !== undefined && form.deliveryStatus !== null) {
    query.deliveryStatus = form.deliveryStatus
  }
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
function syncMasterSelection(record: PurchaseOrder | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getPurchaseOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as PurchaseOrder
  const key = getPurchaseOrderId(row)
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
    title: t('entity.purchaseorder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchaseorder.code'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: t('entity.purchaseorder.purchaserequestid'),
    dataIndex: 'purchaseRequestId',
    key: 'purchaseRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseRequestId') ?? ''
  },
  {
    title: t('entity.purchaseorder.purchaserequestcode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseRequestCode') ?? ''
  },
  {
    title: t('entity.purchaseorder.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.purchaseorder.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.purchaseorder.orderdate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'orderDate') ?? ''
  },
  {
    title: t('entity.purchaseorder.requiredarrivaldate'),
    dataIndex: 'requiredArrivalDate',
    key: 'requiredArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'requiredArrivalDate') ?? ''
  },
  {
    title: t('entity.purchaseorder.actualarrivaldate'),
    dataIndex: 'actualArrivalDate',
    key: 'actualArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualArrivalDate') ?? ''
  },
  {
    title: t('entity.purchaseorder.purchasegroup'),
    dataIndex: 'purchaseGroup',
    key: 'purchaseGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseGroup') ?? ''
  },
  {
    title: t('entity.purchaseorder.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.purchaseorder.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.discountamount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'discountAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.receivedquantity'),
    dataIndex: 'receivedQuantity',
    key: 'receivedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedQuantity') ?? ''
  },
  {
    title: t('entity.purchaseorder.receivedamount'),
    dataIndex: 'receivedAmount',
    key: 'receivedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.paidamount'),
    dataIndex: 'paidAmount',
    key: 'paidAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'paidAmount') ?? ''
  },
  {
    title: t('entity.purchaseorder.paymentmethod'),
    dataIndex: 'paymentMethod',
    key: 'paymentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseorder.deliverymethod'),
    dataIndex: 'deliveryMethod',
    key: 'deliveryMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseorder.deliveryaddress'),
    dataIndex: 'deliveryAddress',
    key: 'deliveryAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'deliveryAddress') ?? ''
  },
  {
    title: t('entity.purchaseorder.orderstatus'),
    dataIndex: 'orderStatus',
    key: 'orderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseorder.deliverystatus'),
    dataIndex: 'deliveryStatus',
    key: 'deliveryStatus',
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
        permission: 'logistics:procurement:purchase:order:update',
        onClick: (record: PurchaseOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:order:delete',
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
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: PurchaseOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getPurchaseOrderId(selectedRow.value) === getPurchaseOrderId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getPurchaseOrderList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  plantCode: '',
  purchaseOrderCode: '',
  purchaseRequestId: '',
  purchaseRequestCode: '',
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
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseorder._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseorder._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseorder._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.purchaseorder._self') }))
    } else {
      await createPurchaseOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseorder._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  purchaseOrderChangeLogPanelRef.value?.reload?.()
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
    const exportMeta = await exportPurchaseOrder(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseorder._self') }))
  } catch (error: any) {
    logger.error('[PurchaseOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseorder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseorder._self'), name: t('common.tip.this.target', { target: t('entity.purchaseorder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseorder._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseorder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseorder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseorder._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleOrderStatusChange(record: PurchaseOrder, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getPurchaseOrderField(record, 'orderStatus')
  const id = getPurchaseOrderId(record)
  const row = dataSource.value.find((item) => getPurchaseOrderId(item) === id)
  if (row) {
    row.orderStatus = newVal
  }
  try {
    await updatePurchaseOrderStatus({ purchaseOrderId: id, orderStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.orderStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  purchaseOrderCode: '',
  purchaseRequestId: '',
  purchaseRequestCode: '',
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
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
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
