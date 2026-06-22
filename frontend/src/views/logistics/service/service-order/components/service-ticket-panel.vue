<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-order/components -->
<!-- 文件名称：service-ticket-panel.vue -->
<!-- 功能描述：服务订单实体主表实体右侧明细 serviceTicket 独立 CRUD（按主表选中 serviceOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="service-ticket-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.serviceticket._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:service:order:create"
      update-permission="logistics:service:order:update"
      delete-permission="logistics:service:order:delete"
      import-permission="logistics:service:order:import"
      export-permission="logistics:service:order:export"
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
    <div class="service-ticket-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getServiceTicketId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="serviceTicketId"
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
      <ServiceTicketForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterServiceOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-service-service-order-service-ticket"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.serviceticket.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceTicketCode')">
      <a-form-item :label="t('entity.serviceticket.code')">
        <a-input
          v-model:value="advancedQueryForm.serviceTicketCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.code') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientId')">
      <a-form-item :label="t('entity.serviceticket.clientid')">
        <a-input
          v-model:value="advancedQueryForm.clientId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="t('entity.serviceticket.clientcode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName')">
      <a-form-item :label="t('entity.serviceticket.clientname')">
        <a-input
          v-model:value="advancedQueryForm.clientName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.clientname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceRequestId')">
      <a-form-item :label="t('entity.serviceticket.servicerequestid')">
        <a-input
          v-model:value="advancedQueryForm.serviceRequestId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicerequestid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceRequestCode')">
      <a-form-item :label="t('entity.serviceticket.servicerequestcode')">
        <a-input
          v-model:value="advancedQueryForm.serviceRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicerequestcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceOrderCode')">
      <a-form-item :label="t('entity.serviceticket.serviceordercode')">
        <a-input
          v-model:value="advancedQueryForm.serviceOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.serviceordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractId')">
      <a-form-item :label="t('entity.serviceticket.servicecontractid')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicecontractid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractCode')">
      <a-form-item :label="t('entity.serviceticket.servicecontractcode')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicecontractcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketType')">
      <a-form-item :label="t('entity.serviceticket.tickettype')">
        <a-input-number
          v-model:value="advancedQueryForm.ticketType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.tickettype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.serviceticket.priority')">
        <TaktSelect
          v-model:value="advancedQueryForm.priority"
          dict-type="sys_priority_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.priority') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketStatus')">
      <a-form-item :label="t('entity.serviceticket.ticketstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.ticketStatus"
          dict-type="sys_ticket_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.ticketstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketSubject')">
      <a-form-item :label="t('entity.serviceticket.ticketsubject')">
        <a-input
          v-model:value="advancedQueryForm.ticketSubject"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.ticketsubject') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.serviceticket.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solutionDescription')">
      <a-form-item :label="t('entity.serviceticket.solutiondescription')">
        <a-textarea
          v-model:value="advancedQueryForm.solutionDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceticket.solutiondescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceLocation')">
      <a-form-item :label="t('entity.serviceticket.servicelocation')">
        <a-input
          v-model:value="advancedQueryForm.serviceLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.servicelocation') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedEmployeeId')">
      <a-form-item :label="t('entity.serviceticket.assignedemployeeid')">
        <a-input
          v-model:value="advancedQueryForm.assignedEmployeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.assignedemployeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedEmployeeName')">
      <a-form-item :label="t('entity.serviceticket.assignedemployeename')">
        <a-input
          v-model:value="advancedQueryForm.assignedEmployeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.assignedemployeename') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledStartTimeStart')">
      <a-form-item :label="t('entity.serviceticket.scheduledstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledStartTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.scheduledstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledEndTimeStart')">
      <a-form-item :label="t('entity.serviceticket.scheduledendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledEndTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.scheduledendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.serviceticket.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.serviceticket.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptanceResult')">
      <a-form-item :label="t('entity.serviceticket.acceptanceresult')">
        <a-input-number
          v-model:value="advancedQueryForm.acceptanceResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.acceptanceresult') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="t('entity.serviceticket.acceptedby')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.acceptedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="t('entity.serviceticket.acceptedatstart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.acceptedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="t('entity.serviceticket.acceptedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.acceptedatend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.serviceticket._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.serviceticket._self"
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
      id-column-key="serviceTicketId"
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
 * 服务订单实体子表 serviceTicket 右栏面板
 * @module views/logistics/service/service-order/components
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
import ServiceTicketForm from './service-ticket-form.vue'
import { useServiceOrderMasterContext } from '../composables/use-service-order-master-context'
import {
  getServiceTicketList,
  getServiceTicketById,
  createServiceTicket,
  updateServiceTicket,
  deleteServiceTicketById,
  deleteServiceTicketBatch,
  getServiceTicketTemplate,
  importServiceTicket,
  exportServiceTicket,
} from '@/api/logistics/customer-service/service-ticket'
import type { ServiceTicket, ServiceTicketQuery } from '@/types/logistics/customer-service/service-ticket'

const { t } = useI18n()
const { selectedMasterRow } = useServiceOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceTicket')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serviceticket._self') }),
)

const loading = ref(false)
const dataSource = ref<ServiceTicket[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ServiceTicket | null>(null)
const selectedRows = ref<ServiceTicket[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ServiceTicket>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  serviceTicketCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  serviceOrderCode: '',
  serviceContractId: '',
  serviceContractCode: '',
  ticketType: undefined as number | undefined,
  priority: undefined as number | undefined,
  ticketStatus: undefined as number | undefined,
  ticketSubject: '',
  faultDescription: '',
  solutionDescription: '',
  serviceLocation: '',
  assignedEmployeeId: '',
  assignedEmployeeName: '',
  scheduledStartTimeStart: '',
  scheduledStartTimeEnd: '',
  scheduledEndTimeStart: '',
  scheduledEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  acceptanceResult: undefined as number | undefined,
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.serviceticket.plantcode') },
  { key: 'serviceTicketCode', label: t('entity.serviceticket.code') },
  { key: 'clientId', label: t('entity.serviceticket.clientid') },
  { key: 'clientCode', label: t('entity.serviceticket.clientcode') },
  { key: 'clientName', label: t('entity.serviceticket.clientname') },
  { key: 'serviceRequestId', label: t('entity.serviceticket.servicerequestid') },
  { key: 'serviceRequestCode', label: t('entity.serviceticket.servicerequestcode') },
  { key: 'serviceOrderCode', label: t('entity.serviceticket.serviceordercode') },
  { key: 'serviceContractId', label: t('entity.serviceticket.servicecontractid') },
  { key: 'serviceContractCode', label: t('entity.serviceticket.servicecontractcode') },
  { key: 'ticketType', label: t('entity.serviceticket.tickettype') },
  { key: 'priority', label: t('entity.serviceticket.priority') },
  { key: 'ticketStatus', label: t('entity.serviceticket.ticketstatus') },
  { key: 'ticketSubject', label: t('entity.serviceticket.ticketsubject') },
  { key: 'faultDescription', label: t('entity.serviceticket.faultdescription') },
  { key: 'solutionDescription', label: t('entity.serviceticket.solutiondescription') },
  { key: 'serviceLocation', label: t('entity.serviceticket.servicelocation') },
  { key: 'assignedEmployeeId', label: t('entity.serviceticket.assignedemployeeid') },
  { key: 'assignedEmployeeName', label: t('entity.serviceticket.assignedemployeename') },
  { key: 'scheduledStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceticket.scheduledstarttime')) },
  { key: 'scheduledStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceticket.scheduledstarttime')) },
  { key: 'scheduledEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceticket.scheduledendtime')) },
  { key: 'scheduledEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceticket.scheduledendtime')) },
  { key: 'actualStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceticket.actualstarttime')) },
  { key: 'actualStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceticket.actualstarttime')) },
  { key: 'actualEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.serviceticket.actualendtime')) },
  { key: 'actualEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.serviceticket.actualendtime')) },
  { key: 'acceptanceResult', label: t('entity.serviceticket.acceptanceresult') },
  { key: 'acceptedBy', label: t('entity.serviceticket.acceptedby') },
  { key: 'acceptedAtStart', label: t('entity.serviceticket.acceptedatstart') },
  { key: 'acceptedAtEnd', label: t('entity.serviceticket.acceptedatend') },
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
  serviceTicketCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  serviceOrderCode: '',
  serviceContractId: '',
  serviceContractCode: '',
  ticketType: undefined as number | undefined,
  priority: undefined as number | undefined,
  ticketStatus: undefined as number | undefined,
  ticketSubject: '',
  faultDescription: '',
  solutionDescription: '',
  serviceLocation: '',
  assignedEmployeeId: '',
  assignedEmployeeName: '',
  scheduledStartTimeStart: '',
  scheduledStartTimeEnd: '',
  scheduledEndTimeStart: '',
  scheduledEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  acceptanceResult: undefined as number | undefined,
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
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

const entityIdName = 'serviceTicketId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.serviceOrderId)
const masterServiceOrderId = computed(() => selectedMasterRow.value?.serviceOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getServiceTicketId(record: ServiceTicket | Record<string, unknown>): string {
  return String((record as ServiceTicket)?.[entityIdName] ?? '')
}

function getServiceTicketField(record: ServiceTicket | Record<string, unknown>, field: string): unknown {
  return (record as ServiceTicket)?.[field as keyof ServiceTicket]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'serviceTicketId',
    key: 'serviceTicketId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'serviceTicketId') ?? ''),
  },
  {
    title: t('entity.serviceticket.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'plantCode') ?? ''),
  },
  {
    title: t('entity.serviceticket.code'),
    dataIndex: 'serviceTicketCode',
    key: 'serviceTicketCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'serviceTicketCode') ?? ''),
  },
  {
    title: t('entity.serviceticket.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'clientId') ?? ''),
  },
  {
    title: t('entity.serviceticket.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'clientCode') ?? ''),
  },
  {
    title: t('entity.serviceticket.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'clientName') ?? ''),
  },
  {
    title: t('entity.serviceticket.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'serviceRequestId') ?? ''),
  },
  {
    title: t('entity.serviceticket.servicerequestcode'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'serviceRequestCode') ?? ''),
  },
  {
    title: t('entity.serviceticket.serviceordercode'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ServiceTicket }) =>
      String(getServiceTicketField(record, 'serviceOrderCode') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:order:update',
        onClick: (record: ServiceTicket) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:order:delete',
        onClick: (record: ServiceTicket) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceTicket[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ServiceTicket, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getServiceTicketId(selectedRow.value) === getServiceTicketId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceTicket[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ServiceTicket) {
  const key = getServiceTicketId(record)
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
 * @returns {ServiceTicketQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ServiceTicketQuery>): ServiceTicketQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ServiceTicketQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    serviceOrderId: masterServiceOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ServiceTicketQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('serviceTicketCode', form.serviceTicketCode)
  assignTrimmed('clientId', form.clientId)
  assignTrimmed('clientCode', form.clientCode)
  assignTrimmed('clientName', form.clientName)
  assignTrimmed('serviceRequestId', form.serviceRequestId)
  assignTrimmed('serviceRequestCode', form.serviceRequestCode)
  assignTrimmed('serviceOrderCode', form.serviceOrderCode)
  assignTrimmed('serviceContractId', form.serviceContractId)
  assignTrimmed('serviceContractCode', form.serviceContractCode)
  if (form.ticketType !== undefined && form.ticketType !== null) {
    query.ticketType = form.ticketType
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  if (form.ticketStatus !== undefined && form.ticketStatus !== null) {
    query.ticketStatus = form.ticketStatus
  }
  assignTrimmed('ticketSubject', form.ticketSubject)
  assignTrimmed('faultDescription', form.faultDescription)
  assignTrimmed('solutionDescription', form.solutionDescription)
  assignTrimmed('serviceLocation', form.serviceLocation)
  assignTrimmed('assignedEmployeeId', form.assignedEmployeeId)
  assignTrimmed('assignedEmployeeName', form.assignedEmployeeName)
  assignTrimmed('scheduledStartTimeStart', form.scheduledStartTimeStart)
  assignTrimmed('scheduledStartTimeEnd', form.scheduledStartTimeEnd)
  assignTrimmed('scheduledEndTimeStart', form.scheduledEndTimeStart)
  assignTrimmed('scheduledEndTimeEnd', form.scheduledEndTimeEnd)
  assignTrimmed('actualStartTimeStart', form.actualStartTimeStart)
  assignTrimmed('actualStartTimeEnd', form.actualStartTimeEnd)
  assignTrimmed('actualEndTimeStart', form.actualEndTimeStart)
  assignTrimmed('actualEndTimeEnd', form.actualEndTimeEnd)
  if (form.acceptanceResult !== undefined && form.acceptanceResult !== null) {
    query.acceptanceResult = form.acceptanceResult
  }
  assignTrimmed('acceptedBy', form.acceptedBy)
  assignTrimmed('acceptedAtStart', form.acceptedAtStart)
  assignTrimmed('acceptedAtEnd', form.acceptedAtEnd)
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
    const res = await getServiceTicketList(buildListQuery())
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
watch(masterServiceOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serviceticket._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ServiceTicket) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serviceticket._self') })
  formLoading.value = true
  try {
    const detail = await getServiceTicketById(getServiceTicketId(record))
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
      entity: t('entity.serviceticket._self'),
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
    const id = formData.value?.serviceTicketId
    if (id) {
      await updateServiceTicket(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.serviceticket._self') }))
    } else {
      await createServiceTicket(payload)
      message.success(t('common.feedback.created', { target: t('entity.serviceticket._self') }))
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

async function handleDeleteOne(record: ServiceTicket) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.serviceticket._self'),
      name: t('common.tip.this.target', { target: t('entity.serviceticket._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceTicketById(getServiceTicketId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.serviceticket._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.serviceticket._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.serviceticket._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getServiceTicketId(r)).filter(Boolean)
      await deleteServiceTicketBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serviceticket._self') }))
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
  const res = await getServiceTicketTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceTicket(file, sheetName)
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
    const exportMeta = await exportServiceTicket(
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
    message.success(t('common.feedback.export.success', { target: t('entity.serviceticket._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.serviceticket._self') }))
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
