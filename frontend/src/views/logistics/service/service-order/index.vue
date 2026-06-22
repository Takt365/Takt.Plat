<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务订单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:service:order:create"
      update-permission="logistics:service:order:update"
      delete-permission="logistics:service:order:delete"
      import-permission="logistics:service:order:import"
      export-permission="logistics:service:order:export"
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
      :master-row-key="getServiceOrderId"
      :master-row-selection="rowSelection"
      master-id-column-key="serviceOrderId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <ServiceTicketPanel
          ref="serviceTicketPanelRef"
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
      <ServiceOrderForm
        :key="formData?.serviceOrderId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-service-service-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.serviceorder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceOrderCode')">
      <a-form-item :label="t('entity.serviceorder.code')">
        <a-input
          v-model:value="advancedQueryForm.serviceOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientId')">
      <a-form-item :label="t('entity.serviceorder.clientid')">
        <a-input
          v-model:value="advancedQueryForm.clientId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.clientid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="t('entity.serviceorder.clientcode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.clientcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName')">
      <a-form-item :label="t('entity.serviceorder.clientname')">
        <a-input
          v-model:value="advancedQueryForm.clientName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.clientname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractId')">
      <a-form-item :label="t('entity.serviceorder.servicecontractid')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.servicecontractid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractCode')">
      <a-form-item :label="t('entity.serviceorder.servicecontractcode')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.servicecontractcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceRequestId')">
      <a-form-item :label="t('entity.serviceorder.servicerequestid')">
        <a-input
          v-model:value="advancedQueryForm.serviceRequestId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.servicerequestid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceRequestCode')">
      <a-form-item :label="t('entity.serviceorder.servicerequestcode')">
        <a-input
          v-model:value="advancedQueryForm.serviceRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.servicerequestcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateStart')">
      <a-form-item :label="t('entity.serviceorder.orderdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.orderdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateEnd')">
      <a-form-item :label="t('entity.serviceorder.orderdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.orderdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderType')">
      <a-form-item :label="t('entity.serviceorder.ordertype')">
        <a-input-number
          v-model:value="advancedQueryForm.orderType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.ordertype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderStatus')">
      <a-form-item :label="t('entity.serviceorder.orderstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.orderStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.orderstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.serviceorder.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="t('entity.serviceorder.discountamount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.discountamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.serviceorder.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="t('entity.serviceorder.actualamount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.actualamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.serviceorder.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.currencycode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateStart')">
      <a-form-item :label="t('entity.serviceorder.plannedstartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.plannedstartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartDateEnd')">
      <a-form-item :label="t('entity.serviceorder.plannedstartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.plannedstartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateStart')">
      <a-form-item :label="t('entity.serviceorder.plannedenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.plannedenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndDateEnd')">
      <a-form-item :label="t('entity.serviceorder.plannedenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.plannedenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartDateStart')">
      <a-form-item :label="t('entity.serviceorder.actualstartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.actualstartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartDateEnd')">
      <a-form-item :label="t('entity.serviceorder.actualstartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.actualstartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndDateStart')">
      <a-form-item :label="t('entity.serviceorder.actualenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.actualenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndDateEnd')">
      <a-form-item :label="t('entity.serviceorder.actualenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceorder.actualenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceBy')">
      <a-form-item :label="t('entity.serviceorder.serviceby')">
        <a-input
          v-model:value="advancedQueryForm.serviceBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceorder.serviceby') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.serviceorder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.serviceorder._self"
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
      :id-column-key="'serviceOrderId'"
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
 * 服务订单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ServiceOrderForm from './components/service-order-form.vue'
import ServiceTicketPanel from './components/service-ticket-panel.vue'
import { provideServiceOrderMasterContext } from './composables/use-service-order-master-context'
import { getServiceOrderList, getServiceOrderById, createServiceOrder, updateServiceOrder, deleteServiceOrderById, deleteServiceOrderBatch, getServiceOrderTemplate, importServiceOrder, exportServiceOrder, updateServiceOrderStatus } from '@/api/logistics/customer-service/service-order'
import type { ServiceOrder, ServiceOrderQuery } from '@/types/logistics/customer-service/service-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serviceorder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ServiceOrder[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ServiceOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<ServiceOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ServiceOrder> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  serviceOrderCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  orderDateStart: '',
  orderDateEnd: '',
  orderType: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  currencyCode: '',
  plannedStartDateStart: '',
  plannedStartDateEnd: '',
  plannedEndDateStart: '',
  plannedEndDateEnd: '',
  actualStartDateStart: '',
  actualStartDateEnd: '',
  actualEndDateStart: '',
  actualEndDateEnd: '',
  serviceBy: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.serviceorder.plantcode') },
  { key: 'serviceOrderCode', label: t('entity.serviceorder.code') },
  { key: 'clientId', label: t('entity.serviceorder.clientid') },
  { key: 'clientCode', label: t('entity.serviceorder.clientcode') },
  { key: 'clientName', label: t('entity.serviceorder.clientname') },
  { key: 'serviceContractId', label: t('entity.serviceorder.servicecontractid') },
  { key: 'serviceContractCode', label: t('entity.serviceorder.servicecontractcode') },
  { key: 'serviceRequestId', label: t('entity.serviceorder.servicerequestid') },
  { key: 'serviceRequestCode', label: t('entity.serviceorder.servicerequestcode') },
  { key: 'orderDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceorder.orderdate')) },
  { key: 'orderDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceorder.orderdate')) },
  { key: 'orderType', label: t('entity.serviceorder.ordertype') },
  { key: 'orderStatus', label: t('entity.serviceorder.orderstatus') },
  { key: 'totalAmount', label: t('entity.serviceorder.totalamount') },
  { key: 'discountAmount', label: t('entity.serviceorder.discountamount') },
  { key: 'taxAmount', label: t('entity.serviceorder.taxamount') },
  { key: 'actualAmount', label: t('entity.serviceorder.actualamount') },
  { key: 'currencyCode', label: t('entity.serviceorder.currencycode') },
  { key: 'plannedStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceorder.plannedstartdate')) },
  { key: 'plannedStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceorder.plannedstartdate')) },
  { key: 'plannedEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceorder.plannedenddate')) },
  { key: 'plannedEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceorder.plannedenddate')) },
  { key: 'actualStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceorder.actualstartdate')) },
  { key: 'actualStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceorder.actualstartdate')) },
  { key: 'actualEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceorder.actualenddate')) },
  { key: 'actualEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceorder.actualenddate')) },
  { key: 'serviceBy', label: t('entity.serviceorder.serviceby') },
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
const entityIdName = 'serviceOrderId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideServiceOrderMasterContext()
const serviceTicketPanelRef = ref<InstanceType<typeof ServiceTicketPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ServiceOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ServiceOrderQuery>): ServiceOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ServiceOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ServiceOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('serviceOrderCode', form.serviceOrderCode)
  assignTrimmed('clientId', form.clientId)
  assignTrimmed('clientCode', form.clientCode)
  assignTrimmed('clientName', form.clientName)
  assignTrimmed('serviceContractId', form.serviceContractId)
  assignTrimmed('serviceContractCode', form.serviceContractCode)
  assignTrimmed('serviceRequestId', form.serviceRequestId)
  assignTrimmed('serviceRequestCode', form.serviceRequestCode)
  assignTrimmed('orderDateStart', form.orderDateStart)
  assignTrimmed('orderDateEnd', form.orderDateEnd)
  if (form.orderType !== undefined && form.orderType !== null) {
    query.orderType = form.orderType
  }
  if (form.orderStatus !== undefined && form.orderStatus !== null) {
    query.orderStatus = form.orderStatus
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
  assignTrimmed('currencyCode', form.currencyCode)
  assignTrimmed('plannedStartDateStart', form.plannedStartDateStart)
  assignTrimmed('plannedStartDateEnd', form.plannedStartDateEnd)
  assignTrimmed('plannedEndDateStart', form.plannedEndDateStart)
  assignTrimmed('plannedEndDateEnd', form.plannedEndDateEnd)
  assignTrimmed('actualStartDateStart', form.actualStartDateStart)
  assignTrimmed('actualStartDateEnd', form.actualStartDateEnd)
  assignTrimmed('actualEndDateStart', form.actualEndDateStart)
  assignTrimmed('actualEndDateEnd', form.actualEndDateEnd)
  assignTrimmed('serviceBy', form.serviceBy)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: ServiceOrder | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getServiceOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as ServiceOrder
  const key = getServiceOrderId(row)
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
async function loadServiceOrderDetail(record: ServiceOrder): Promise<ServiceOrder | null> {
  const id = getServiceOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getServiceOrderById(id)
    const index = dataSource.value.findIndex((row) => getServiceOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ServiceOrder
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
    dataIndex: 'serviceOrderId',
    key: 'serviceOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceOrderId') ?? ''
  },
  {
    title: t('entity.serviceorder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.serviceorder.code'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceOrderCode') ?? ''
  },
  {
    title: t('entity.serviceorder.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'clientId') ?? ''
  },
  {
    title: t('entity.serviceorder.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'clientCode') ?? ''
  },
  {
    title: t('entity.serviceorder.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'clientName') ?? ''
  },
  {
    title: t('entity.serviceorder.servicecontractid'),
    dataIndex: 'serviceContractId',
    key: 'serviceContractId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceContractId') ?? ''
  },
  {
    title: t('entity.serviceorder.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceContractCode') ?? ''
  },
  {
    title: t('entity.serviceorder.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceRequestId') ?? ''
  },
  {
    title: t('entity.serviceorder.servicerequestcode'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceRequestCode') ?? ''
  },
  {
    title: t('entity.serviceorder.orderdate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'orderDate') ?? ''
  },
  {
    title: t('entity.serviceorder.ordertype'),
    dataIndex: 'orderType',
    key: 'orderType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'orderType') ?? ''
  },
  {
    title: t('entity.serviceorder.orderstatus'),
    dataIndex: 'orderStatus',
    key: 'orderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'orderStatus') ?? ''
  },
  {
    title: t('entity.serviceorder.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.serviceorder.discountamount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'discountAmount') ?? ''
  },
  {
    title: t('entity.serviceorder.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.serviceorder.actualamount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'actualAmount') ?? ''
  },
  {
    title: t('entity.serviceorder.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.serviceorder.plannedstartdate'),
    dataIndex: 'plannedStartDate',
    key: 'plannedStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'plannedStartDate') ?? ''
  },
  {
    title: t('entity.serviceorder.plannedenddate'),
    dataIndex: 'plannedEndDate',
    key: 'plannedEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'plannedEndDate') ?? ''
  },
  {
    title: t('entity.serviceorder.actualstartdate'),
    dataIndex: 'actualStartDate',
    key: 'actualStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'actualStartDate') ?? ''
  },
  {
    title: t('entity.serviceorder.actualenddate'),
    dataIndex: 'actualEndDate',
    key: 'actualEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'actualEndDate') ?? ''
  },
  {
    title: t('entity.serviceorder.serviceby'),
    dataIndex: 'serviceBy',
    key: 'serviceBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceOrderField(record, 'serviceBy') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:order:update',
        onClick: (record: ServiceOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:order:delete',
        onClick: (record: ServiceOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getServiceOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getServiceOrderField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: ServiceOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getServiceOrderId(selectedRow.value) === getServiceOrderId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getServiceOrderList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ServiceOrder] 加载数据失败', { error })
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
  serviceOrderCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  orderDateStart: '',
  orderDateEnd: '',
  orderType: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  currencyCode: '',
  plannedStartDateStart: '',
  plannedStartDateEnd: '',
  plannedEndDateStart: '',
  plannedEndDateEnd: '',
  actualStartDateStart: '',
  actualStartDateEnd: '',
  actualEndDateStart: '',
  actualEndDateEnd: '',
  serviceBy: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serviceorder._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ServiceOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serviceorder._self') })
  formLoading.value = true
  try {
    const detail = await loadServiceOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.serviceorder._self') }))
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
      await updateServiceOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.serviceorder._self') }))
    } else {
      await createServiceOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.serviceorder._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  serviceTicketPanelRef.value?.reload?.()
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
  const res = await getServiceOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceOrder(file, sheetName)
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
    const exportMeta = await exportServiceOrder(
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
    message.success(t('common.feedback.export.success', { target: t('entity.serviceorder._self') }))
  } catch (error: any) {
    logger.error('[ServiceOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.serviceorder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ServiceOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.serviceorder._self'), name: t('common.tip.this.target', { target: t('entity.serviceorder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.serviceorder._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.serviceorder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.serviceorder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteServiceOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serviceorder._self') }))
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
  serviceOrderCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  orderDateStart: '',
  orderDateEnd: '',
  orderType: undefined as number | undefined,
  orderStatus: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  currencyCode: '',
  plannedStartDateStart: '',
  plannedStartDateEnd: '',
  plannedEndDateStart: '',
  plannedEndDateEnd: '',
  actualStartDateStart: '',
  actualStartDateEnd: '',
  actualEndDateStart: '',
  actualEndDateEnd: '',
  serviceBy: '',
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
