<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract/components -->
<!-- 文件名称：service-order-panel.vue -->
<!-- 功能描述：服务合同实体主表实体右侧明细 serviceOrder 独立 CRUD（按主表选中 serviceContractId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="service-order-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.serviceorder._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:service:contract:create"
      update-permission="logistics:service:contract:update"
      delete-permission="logistics:service:contract:delete"
      import-permission="logistics:service:contract:import"
      export-permission="logistics:service:contract:export"
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
    <div class="service-order-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getServiceOrderId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="serviceOrderId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
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
      <ServiceOrderForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterServiceContractId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-service-service-contract-service-order"
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
          :maxlength="20"
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
          :maxlength="20"
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
          :maxlength="20"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="serviceOrderId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 服务合同实体子表 serviceOrder 右栏面板
 * @module views/logistics/service/service-contract/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import ServiceOrderForm from './service-order-form.vue'
import { useServiceContractMasterContext } from '../composables/use-service-contract-master-context'
import {
  getServiceOrderList,
  getServiceOrderById,
  createServiceOrder,
  updateServiceOrder,
  deleteServiceOrderById,
  deleteServiceOrderBatch,
  getServiceOrderTemplate,
  importServiceOrder,
  exportServiceOrder,
} from '@/api/logistics/customer-service/service-order'
import type { ServiceOrder, ServiceOrderQuery } from '@/types/logistics/customer-service/service-order'

const { t } = useI18n()
const { selectedMasterRow } = useServiceContractMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceOrder')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serviceorder._self') }),
)

const loading = ref(false)
const dataSource = ref<ServiceOrder[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ServiceOrder | null>(null)
const selectedRows = ref<ServiceOrder[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ServiceOrder>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  serviceOrderCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
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
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.serviceorder.plantcode') },
  { key: 'serviceOrderCode', label: t('entity.serviceorder.code') },
  { key: 'clientId', label: t('entity.serviceorder.clientid') },
  { key: 'clientCode', label: t('entity.serviceorder.clientcode') },
  { key: 'clientName', label: t('entity.serviceorder.clientname') },
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

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  serviceOrderCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
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
const columnSettingVisible = ref(false)
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

const entityIdName = 'serviceOrderId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.serviceContractId)
const masterServiceContractId = computed(() => selectedMasterRow.value?.serviceContractId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getServiceOrderId(record: ServiceOrder | Record<string, unknown>): string {
  return String((record as ServiceOrder)?.[entityIdName] ?? '')
}

function getServiceOrderField(record: ServiceOrder | Record<string, unknown>, field: string): unknown {
  return (record as ServiceOrder)?.[field as keyof ServiceOrder]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'serviceOrderId',
    key: 'serviceOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'serviceOrderId') ?? ''),
  },
  {
    title: t('entity.serviceorder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'plantCode') ?? ''),
  },
  {
    title: t('entity.serviceorder.code'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'serviceOrderCode') ?? ''),
  },
  {
    title: t('entity.serviceorder.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'clientId') ?? ''),
  },
  {
    title: t('entity.serviceorder.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'clientCode') ?? ''),
  },
  {
    title: t('entity.serviceorder.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'clientName') ?? ''),
  },
  {
    title: t('entity.serviceorder.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'serviceContractCode') ?? ''),
  },
  {
    title: t('entity.serviceorder.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'serviceRequestId') ?? ''),
  },
  {
    title: t('entity.serviceorder.servicerequestcode'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceOrder }) =>
      String(getServiceOrderField(record, 'serviceRequestCode') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:contract:update',
        onClick: (record: ServiceOrder) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:contract:delete',
        onClick: (record: ServiceOrder) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ServiceOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getServiceOrderId(selectedRow.value) === getServiceOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ServiceOrder) {
  const key = getServiceOrderId(record)
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
 * @returns {ServiceOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ServiceOrderQuery>): ServiceOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ServiceOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    serviceContractId: masterServiceContractId.value,
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
    const res = await getServiceOrderList(buildListQuery())
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
watch(masterServiceContractId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serviceorder._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ServiceOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serviceorder._self') })
  formLoading.value = true
  try {
    const detail = await getServiceOrderById(getServiceOrderId(record))
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
      entity: t('entity.serviceorder._self'),
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
    const id = formData.value?.serviceOrderId
    if (id) {
      await updateServiceOrder(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.serviceorder._self') }))
    } else {
      await createServiceOrder(payload)
      message.success(t('common.feedback.created', { target: t('entity.serviceorder._self') }))
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

async function handleDeleteOne(record: ServiceOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.serviceorder._self'),
      name: t('common.tip.this.target', { target: t('entity.serviceorder._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceOrderById(getServiceOrderId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.serviceorder._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.serviceorder._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.serviceorder._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getServiceOrderId(r)).filter(Boolean)
      await deleteServiceOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serviceorder._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getServiceOrderTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceOrder(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

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
    const exportMeta = await exportServiceOrder(
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
    message.success(t('common.feedback.export.success', { target: t('entity.serviceorder._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.serviceorder._self') }))
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
