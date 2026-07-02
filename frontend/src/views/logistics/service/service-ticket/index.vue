<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-ticket -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务工单实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="logistics:service:ticket:create"
      update-permission="logistics:service:ticket:update"
      delete-permission="logistics:service:ticket:delete"
      import-permission="logistics:service:ticket:import"
      export-permission="logistics:service:ticket:export"
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

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'serviceTicketId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getServiceTicketId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'priority'">
          <TaktDictTag
            :value="getServiceTicketField(record, 'priority')"
            dict-type="sys_priority_level_category"
          />
        </template>
        <template v-else-if="column.key === 'ticketStatus'">
          <TaktDictTag
            :value="getServiceTicketField(record, 'ticketStatus')"
            dict-type="sys_ticket_status"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
      <ServiceTicketForm
        :key="formData?.serviceTicketId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-service-service-ticket'"
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
          :maxlength="50"
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
      <div v-show="isFieldVisible('serviceOrderId')">
      <a-form-item :label="t('entity.serviceticket.serviceorderid')">
        <a-input
          v-model:value="advancedQueryForm.serviceOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceticket.serviceorderid') })"
          show-count
          :maxlength="20"
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
          :maxlength="200"
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
          :maxlength="500"
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
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledStartTimeStart')">
      <a-form-item :label="t('entity.serviceticket.scheduledstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledStartTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.scheduledstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledEndTimeStart')">
      <a-form-item :label="t('entity.serviceticket.scheduledendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledEndTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.scheduledendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.scheduledendtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.serviceticket.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.serviceticket.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.serviceticket.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceticket.actualendtimeend') })"
          value-format="YYYY-MM-DD"
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
          :maxlength="50"
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

    <!-- 导入对话框 -->
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'serviceTicketId'"
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
 * 服务工单实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-ticket
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ServiceTicketForm from './components/service-ticket-form.vue'
import { getServiceTicketList, getServiceTicketById, createServiceTicket, updateServiceTicket, deleteServiceTicketById, deleteServiceTicketBatch, getServiceTicketTemplate, importServiceTicket, exportServiceTicket, updateServiceTicketStatus } from '@/api/logistics/customer-service/service-ticket'
import type { ServiceTicket, ServiceTicketQuery } from '@/types/logistics/customer-service/service-ticket'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceTicket')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serviceticket._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ServiceTicket[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ServiceTicket | null>(null)
/** 表格多选行 */
const selectedRows = ref<ServiceTicket[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ServiceTicket> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  serviceTicketCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  serviceOrderId: '',
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
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.serviceticket.plantcode') },
  { key: 'serviceTicketCode', label: t('entity.serviceticket.code') },
  { key: 'clientId', label: t('entity.serviceticket.clientid') },
  { key: 'clientCode', label: t('entity.serviceticket.clientcode') },
  { key: 'clientName', label: t('entity.serviceticket.clientname') },
  { key: 'serviceRequestId', label: t('entity.serviceticket.servicerequestid') },
  { key: 'serviceRequestCode', label: t('entity.serviceticket.servicerequestcode') },
  { key: 'serviceOrderId', label: t('entity.serviceticket.serviceorderid') },
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
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'serviceTicketId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


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
  assignTrimmed('serviceOrderId', form.serviceOrderId)
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
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})







/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'serviceTicketId',
    key: 'serviceTicketId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceTicketId') ?? ''
  },
  {
    title: t('entity.serviceticket.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.serviceticket.code'),
    dataIndex: 'serviceTicketCode',
    key: 'serviceTicketCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceTicketCode') ?? ''
  },
  {
    title: t('entity.serviceticket.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'clientId') ?? ''
  },
  {
    title: t('entity.serviceticket.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'clientCode') ?? ''
  },
  {
    title: t('entity.serviceticket.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'clientName') ?? ''
  },
  {
    title: t('entity.serviceticket.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceRequestId') ?? ''
  },
  {
    title: t('entity.serviceticket.servicerequestcode'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceRequestCode') ?? ''
  },
  {
    title: t('entity.serviceticket.serviceorderid'),
    dataIndex: 'serviceOrderId',
    key: 'serviceOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceOrderId') ?? ''
  },
  {
    title: t('entity.serviceticket.serviceordercode'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceOrderCode') ?? ''
  },
  {
    title: t('entity.serviceticket.servicecontractid'),
    dataIndex: 'serviceContractId',
    key: 'serviceContractId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceContractId') ?? ''
  },
  {
    title: t('entity.serviceticket.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceContractCode') ?? ''
  },
  {
    title: t('entity.serviceticket.tickettype'),
    dataIndex: 'ticketType',
    key: 'ticketType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'ticketType') ?? ''
  },
  {
    title: t('entity.serviceticket.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.serviceticket.ticketstatus'),
    dataIndex: 'ticketStatus',
    key: 'ticketStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.serviceticket.ticketsubject'),
    dataIndex: 'ticketSubject',
    key: 'ticketSubject',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'ticketSubject') ?? ''
  },
  {
    title: t('entity.serviceticket.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'faultDescription') ?? ''
  },
  {
    title: t('entity.serviceticket.solutiondescription'),
    dataIndex: 'solutionDescription',
    key: 'solutionDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'solutionDescription') ?? ''
  },
  {
    title: t('entity.serviceticket.servicelocation'),
    dataIndex: 'serviceLocation',
    key: 'serviceLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceLocation') ?? ''
  },
  {
    title: t('entity.serviceticket.assignedemployeeid'),
    dataIndex: 'assignedEmployeeId',
    key: 'assignedEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'assignedEmployeeId') ?? ''
  },
  {
    title: t('entity.serviceticket.assignedemployeename'),
    dataIndex: 'assignedEmployeeName',
    key: 'assignedEmployeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'assignedEmployeeName') ?? ''
  },
  {
    title: t('entity.serviceticket.scheduledstarttime'),
    dataIndex: 'scheduledStartTime',
    key: 'scheduledStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'scheduledStartTime') ?? ''
  },
  {
    title: t('entity.serviceticket.scheduledendtime'),
    dataIndex: 'scheduledEndTime',
    key: 'scheduledEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'scheduledEndTime') ?? ''
  },
  {
    title: t('entity.serviceticket.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'actualStartTime') ?? ''
  },
  {
    title: t('entity.serviceticket.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'actualEndTime') ?? ''
  },
  {
    title: t('entity.serviceticket.acceptanceresult'),
    dataIndex: 'acceptanceResult',
    key: 'acceptanceResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'acceptanceResult') ?? ''
  },
  {
    title: t('entity.serviceticket.acceptedby'),
    dataIndex: 'acceptedBy',
    key: 'acceptedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'acceptedBy') ?? ''
  },
  {
    title: t('entity.serviceticket.acceptedat'),
    dataIndex: 'acceptedAt',
    key: 'acceptedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'acceptedAt') ?? ''
  },
  {
    title: t('entity.serviceticket.servicerequest'),
    dataIndex: 'serviceRequest',
    key: 'serviceRequest',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceRequest') ?? ''
  },
  {
    title: t('entity.serviceticket.serviceorder'),
    dataIndex: 'serviceOrder',
    key: 'serviceOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceOrder') ?? ''
  },
  {
    title: t('entity.serviceticket.servicecontract'),
    dataIndex: 'serviceContract',
    key: 'serviceContract',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceTicketField(record, 'serviceContract') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:ticket:update',
        onClick: (record: ServiceTicket) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:ticket:delete',
        onClick: (record: ServiceTicket) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getServiceTicketId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getServiceTicketField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
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
    } else if (selectedRow.value && getServiceTicketId(selectedRow.value) === getServiceTicketId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceTicket[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ServiceTicket) => ({
  onClick: () => {
    const key = getServiceTicketId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getServiceTicketId(item)))
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
    const res = await getServiceTicketList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ServiceTicket] 加载数据失败', { error })
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
  serviceTicketCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  serviceOrderId: '',
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serviceticket._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: ServiceTicket) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serviceticket._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.serviceticket._self') }))
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
      await updateServiceTicket(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.serviceticket._self') }))
    } else {
      await createServiceTicket(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.serviceticket._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getServiceTicketTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceTicket(file, sheetName)
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
    const exportMeta = await exportServiceTicket(
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
    message.success(t('common.feedback.export.success', { target: t('entity.serviceticket._self') }))
  } catch (error: any) {
    logger.error('[ServiceTicket] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.serviceticket._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ServiceTicket) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.serviceticket._self'), name: t('common.tip.this.target', { target: t('entity.serviceticket._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceTicketById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.serviceticket._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.serviceticket._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.serviceticket._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteServiceTicketBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serviceticket._self') }))
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
  serviceTicketCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceRequestId: '',
  serviceRequestCode: '',
  serviceOrderId: '',
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
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
