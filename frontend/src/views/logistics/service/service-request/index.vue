<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-request -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务请求实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:service:request:create"
      update-permission="logistics:service:request:update"
      delete-permission="logistics:service:request:delete"
      import-permission="logistics:service:request:import"
      export-permission="logistics:service:request:export"
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
      :id-column-key="'serviceRequestId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getServiceRequestId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'priority'">
          <TaktDictTag
            :value="getServiceRequestField(record, 'priority')"
            dict-type="sys_priority_level_category"
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
      <ServiceRequestForm
        :key="formData?.serviceRequestId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-service-service-request'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.servicerequest.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceRequestCode')">
      <a-form-item :label="t('entity.servicerequest.code')">
        <a-input
          v-model:value="advancedQueryForm.serviceRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientId')">
      <a-form-item :label="t('entity.servicerequest.clientid')">
        <a-input
          v-model:value="advancedQueryForm.clientId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="t('entity.servicerequest.clientcode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName')">
      <a-form-item :label="t('entity.servicerequest.clientname')">
        <a-input
          v-model:value="advancedQueryForm.clientName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractId')">
      <a-form-item :label="t('entity.servicerequest.servicecontractid')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.servicecontractid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractCode')">
      <a-form-item :label="t('entity.servicerequest.servicecontractcode')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.servicecontractcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestDateStart')">
      <a-form-item :label="t('entity.servicerequest.requestdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requestDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestDateEnd')">
      <a-form-item :label="t('entity.servicerequest.requestdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requestDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedServiceDateStart')">
      <a-form-item :label="t('entity.servicerequest.expectedservicedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedServiceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.expectedservicedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedServiceDateEnd')">
      <a-form-item :label="t('entity.servicerequest.expectedservicedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedServiceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.expectedservicedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestType')">
      <a-form-item :label="t('entity.servicerequest.requesttype')">
        <a-input-number
          v-model:value="advancedQueryForm.requestType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requesttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceChannel')">
      <a-form-item :label="t('entity.servicerequest.sourcechannel')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceChannel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.sourcechannel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.servicerequest.priority')">
        <TaktSelect
          v-model:value="advancedQueryForm.priority"
          dict-type="sys_priority_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.priority') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestStatus')">
      <a-form-item :label="t('entity.servicerequest.requeststatus')">
        <a-input-number
          v-model:value="advancedQueryForm.requestStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requeststatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestSubject')">
      <a-form-item :label="t('entity.servicerequest.requestsubject')">
        <a-input
          v-model:value="advancedQueryForm.requestSubject"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requestsubject') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestDescription')">
      <a-form-item :label="t('entity.servicerequest.requestdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.requestDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.servicerequest.requestdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPerson')">
      <a-form-item :label="t('entity.servicerequest.contactperson')">
        <a-input
          v-model:value="advancedQueryForm.contactPerson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactperson') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="t('entity.servicerequest.contactphone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactphone') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="t('entity.servicerequest.contactemail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.contactemail') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceAddress')">
      <a-form-item :label="t('entity.servicerequest.serviceaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.serviceAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.servicerequest.serviceaddress') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedEmployeeId')">
      <a-form-item :label="t('entity.servicerequest.assignedemployeeid')">
        <a-input
          v-model:value="advancedQueryForm.assignedEmployeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedemployeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedEmployeeName')">
      <a-form-item :label="t('entity.servicerequest.assignedemployeename')">
        <a-input
          v-model:value="advancedQueryForm.assignedEmployeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedemployeename') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedAtStart')">
      <a-form-item :label="t('entity.servicerequest.assignedatstart')">
        <a-input
          v-model:value="advancedQueryForm.assignedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.assignedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assignedAtEnd')">
      <a-form-item :label="t('entity.servicerequest.assignedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.assignedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.assignedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedAtStart')">
      <a-form-item :label="t('entity.servicerequest.closedatstart')">
        <a-input
          v-model:value="advancedQueryForm.closedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.closedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedAtEnd')">
      <a-form-item :label="t('entity.servicerequest.closedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.closedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.closedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.servicerequest._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.servicerequest._self"
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
      :id-column-key="'serviceRequestId'"
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
 * 服务请求实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-request
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ServiceRequestForm from './components/service-request-form.vue'
import { getServiceRequestList, getServiceRequestById, createServiceRequest, updateServiceRequest, deleteServiceRequestById, deleteServiceRequestBatch, getServiceRequestTemplate, importServiceRequest, exportServiceRequest, updateServiceRequestStatus } from '@/api/logistics/customer-service/service-request'
import type { ServiceRequest, ServiceRequestQuery } from '@/types/logistics/customer-service/service-request'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceRequest')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.servicerequest._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ServiceRequest[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ServiceRequest | null>(null)
/** 表格多选行 */
const selectedRows = ref<ServiceRequest[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ServiceRequest> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  serviceRequestCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  expectedServiceDateStart: '',
  expectedServiceDateEnd: '',
  requestType: undefined as number | undefined,
  sourceChannel: undefined as number | undefined,
  priority: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  requestSubject: '',
  requestDescription: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  serviceAddress: '',
  assignedEmployeeId: '',
  assignedEmployeeName: '',
  assignedAtStart: '',
  assignedAtEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.servicerequest.plantcode') },
  { key: 'serviceRequestCode', label: t('entity.servicerequest.code') },
  { key: 'clientId', label: t('entity.servicerequest.clientid') },
  { key: 'clientCode', label: t('entity.servicerequest.clientcode') },
  { key: 'clientName', label: t('entity.servicerequest.clientname') },
  { key: 'serviceContractId', label: t('entity.servicerequest.servicecontractid') },
  { key: 'serviceContractCode', label: t('entity.servicerequest.servicecontractcode') },
  { key: 'requestDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.servicerequest.requestdate')) },
  { key: 'requestDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.servicerequest.requestdate')) },
  { key: 'expectedServiceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.servicerequest.expectedservicedate')) },
  { key: 'expectedServiceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.servicerequest.expectedservicedate')) },
  { key: 'requestType', label: t('entity.servicerequest.requesttype') },
  { key: 'sourceChannel', label: t('entity.servicerequest.sourcechannel') },
  { key: 'priority', label: t('entity.servicerequest.priority') },
  { key: 'requestStatus', label: t('entity.servicerequest.requeststatus') },
  { key: 'requestSubject', label: t('entity.servicerequest.requestsubject') },
  { key: 'requestDescription', label: t('entity.servicerequest.requestdescription') },
  { key: 'contactPerson', label: t('entity.servicerequest.contactperson') },
  { key: 'contactPhone', label: t('entity.servicerequest.contactphone') },
  { key: 'contactEmail', label: t('entity.servicerequest.contactemail') },
  { key: 'serviceAddress', label: t('entity.servicerequest.serviceaddress') },
  { key: 'assignedEmployeeId', label: t('entity.servicerequest.assignedemployeeid') },
  { key: 'assignedEmployeeName', label: t('entity.servicerequest.assignedemployeename') },
  { key: 'assignedAtStart', label: t('entity.servicerequest.assignedatstart') },
  { key: 'assignedAtEnd', label: t('entity.servicerequest.assignedatend') },
  { key: 'closedAtStart', label: t('entity.servicerequest.closedatstart') },
  { key: 'closedAtEnd', label: t('entity.servicerequest.closedatend') },
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
const entityIdName = 'serviceRequestId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ServiceRequestQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ServiceRequestQuery>): ServiceRequestQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ServiceRequestQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ServiceRequestQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('serviceRequestCode', form.serviceRequestCode)
  assignTrimmed('clientId', form.clientId)
  assignTrimmed('clientCode', form.clientCode)
  assignTrimmed('clientName', form.clientName)
  assignTrimmed('serviceContractId', form.serviceContractId)
  assignTrimmed('serviceContractCode', form.serviceContractCode)
  assignTrimmed('requestDateStart', form.requestDateStart)
  assignTrimmed('requestDateEnd', form.requestDateEnd)
  assignTrimmed('expectedServiceDateStart', form.expectedServiceDateStart)
  assignTrimmed('expectedServiceDateEnd', form.expectedServiceDateEnd)
  if (form.requestType !== undefined && form.requestType !== null) {
    query.requestType = form.requestType
  }
  if (form.sourceChannel !== undefined && form.sourceChannel !== null) {
    query.sourceChannel = form.sourceChannel
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  if (form.requestStatus !== undefined && form.requestStatus !== null) {
    query.requestStatus = form.requestStatus
  }
  assignTrimmed('requestSubject', form.requestSubject)
  assignTrimmed('requestDescription', form.requestDescription)
  assignTrimmed('contactPerson', form.contactPerson)
  assignTrimmed('contactPhone', form.contactPhone)
  assignTrimmed('contactEmail', form.contactEmail)
  assignTrimmed('serviceAddress', form.serviceAddress)
  assignTrimmed('assignedEmployeeId', form.assignedEmployeeId)
  assignTrimmed('assignedEmployeeName', form.assignedEmployeeName)
  assignTrimmed('assignedAtStart', form.assignedAtStart)
  assignTrimmed('assignedAtEnd', form.assignedAtEnd)
  assignTrimmed('closedAtStart', form.closedAtStart)
  assignTrimmed('closedAtEnd', form.closedAtEnd)
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
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceRequestId') ?? ''
  },
  {
    title: t('entity.servicerequest.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.servicerequest.code'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceRequestCode') ?? ''
  },
  {
    title: t('entity.servicerequest.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'clientId') ?? ''
  },
  {
    title: t('entity.servicerequest.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'clientCode') ?? ''
  },
  {
    title: t('entity.servicerequest.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'clientName') ?? ''
  },
  {
    title: t('entity.servicerequest.servicecontractid'),
    dataIndex: 'serviceContractId',
    key: 'serviceContractId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceContractId') ?? ''
  },
  {
    title: t('entity.servicerequest.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceContractCode') ?? ''
  },
  {
    title: t('entity.servicerequest.requestdate'),
    dataIndex: 'requestDate',
    key: 'requestDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'requestDate') ?? ''
  },
  {
    title: t('entity.servicerequest.expectedservicedate'),
    dataIndex: 'expectedServiceDate',
    key: 'expectedServiceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'expectedServiceDate') ?? ''
  },
  {
    title: t('entity.servicerequest.requesttype'),
    dataIndex: 'requestType',
    key: 'requestType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'requestType') ?? ''
  },
  {
    title: t('entity.servicerequest.sourcechannel'),
    dataIndex: 'sourceChannel',
    key: 'sourceChannel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'sourceChannel') ?? ''
  },
  {
    title: t('entity.servicerequest.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.requeststatus'),
    dataIndex: 'requestStatus',
    key: 'requestStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'requestStatus') ?? ''
  },
  {
    title: t('entity.servicerequest.requestsubject'),
    dataIndex: 'requestSubject',
    key: 'requestSubject',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'requestSubject') ?? ''
  },
  {
    title: t('entity.servicerequest.requestdescription'),
    dataIndex: 'requestDescription',
    key: 'requestDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'requestDescription') ?? ''
  },
  {
    title: t('entity.servicerequest.contactperson'),
    dataIndex: 'contactPerson',
    key: 'contactPerson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'contactPerson') ?? ''
  },
  {
    title: t('entity.servicerequest.contactphone'),
    dataIndex: 'contactPhone',
    key: 'contactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'contactPhone') ?? ''
  },
  {
    title: t('entity.servicerequest.contactemail'),
    dataIndex: 'contactEmail',
    key: 'contactEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'contactEmail') ?? ''
  },
  {
    title: t('entity.servicerequest.serviceaddress'),
    dataIndex: 'serviceAddress',
    key: 'serviceAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceAddress') ?? ''
  },
  {
    title: t('entity.servicerequest.assignedemployeeid'),
    dataIndex: 'assignedEmployeeId',
    key: 'assignedEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'assignedEmployeeId') ?? ''
  },
  {
    title: t('entity.servicerequest.assignedemployeename'),
    dataIndex: 'assignedEmployeeName',
    key: 'assignedEmployeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'assignedEmployeeName') ?? ''
  },
  {
    title: t('entity.servicerequest.assignedat'),
    dataIndex: 'assignedAt',
    key: 'assignedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'assignedAt') ?? ''
  },
  {
    title: t('entity.servicerequest.closedat'),
    dataIndex: 'closedAt',
    key: 'closedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'closedAt') ?? ''
  },
  {
    title: t('entity.servicerequest.servicecontract'),
    dataIndex: 'serviceContract',
    key: 'serviceContract',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceRequestField(record, 'serviceContract') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:request:update',
        onClick: (record: ServiceRequest) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:request:delete',
        onClick: (record: ServiceRequest) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getServiceRequestId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getServiceRequestField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceRequest[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ServiceRequest, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getServiceRequestId(selectedRow.value) === getServiceRequestId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceRequest[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ServiceRequest) => ({
  onClick: () => {
    const key = getServiceRequestId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getServiceRequestId(item)))
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
    const res = await getServiceRequestList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ServiceRequest] 加载数据失败', { error })
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
  serviceRequestCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  expectedServiceDateStart: '',
  expectedServiceDateEnd: '',
  requestType: undefined as number | undefined,
  sourceChannel: undefined as number | undefined,
  priority: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  requestSubject: '',
  requestDescription: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  serviceAddress: '',
  assignedEmployeeId: '',
  assignedEmployeeName: '',
  assignedAtStart: '',
  assignedAtEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.servicerequest._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: ServiceRequest) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.servicerequest._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.servicerequest._self') }))
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
      await updateServiceRequest(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.servicerequest._self') }))
    } else {
      await createServiceRequest(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.servicerequest._self') }))
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
  const res = await getServiceRequestTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceRequest(file, sheetName)
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
    const exportMeta = await exportServiceRequest(
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
    message.success(t('common.feedback.export.success', { target: t('entity.servicerequest._self') }))
  } catch (error: any) {
    logger.error('[ServiceRequest] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.servicerequest._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ServiceRequest) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.servicerequest._self'), name: t('common.tip.this.target', { target: t('entity.servicerequest._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceRequestById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.servicerequest._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.servicerequest._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.servicerequest._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteServiceRequestBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.servicerequest._self') }))
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
  serviceRequestCode: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  serviceContractId: '',
  serviceContractCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  expectedServiceDateStart: '',
  expectedServiceDateEnd: '',
  requestType: undefined as number | undefined,
  sourceChannel: undefined as number | undefined,
  priority: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  requestSubject: '',
  requestDescription: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  serviceAddress: '',
  assignedEmployeeId: '',
  assignedEmployeeName: '',
  assignedAtStart: '',
  assignedAtEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
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
