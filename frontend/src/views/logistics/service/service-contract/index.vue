<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务合同实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-service-service-contract">
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
      create-permission="logistics:service:servicecontract:create"
      update-permission="logistics:service:servicecontract:update"
      delete-permission="logistics:service:servicecontract:delete"
      import-permission="logistics:service:servicecontract:import"
      export-permission="logistics:service:servicecontract:export"
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
      :id-column-key="'serviceContractId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getServiceContractId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.serviceOrder._self') }}</div>
          <a-table
            v-if="hasServiceOrderRows(record)"
            :columns="serviceOrderExpandColumns"
            :data-source="getServiceOrderRows(record)"
            :row-key="(row: ServiceOrder, index?: number) => row?.serviceOrderId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.serviceRequest._self') }}</div>
          <a-table
            v-if="hasServiceRequestRows(record)"
            :columns="serviceRequestExpandColumns"
            :data-source="getServiceRequestRows(record)"
            :row-key="(row: ServiceRequest, index?: number) => row?.serviceRequestId || String(index ?? 0)"
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
      <ServiceContractForm
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
      :storage-key="'takt-query-fields-logistics-service-service-contract'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.serviceContract.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractCode')">
      <a-form-item :label="t('entity.serviceContract.code')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractName')">
      <a-form-item :label="t('entity.serviceContract.contractname')">
        <a-input
          v-model:value="advancedQueryForm.contractName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientId')">
      <a-form-item :label="t('entity.serviceContract.clientid')">
        <a-input
          v-model:value="advancedQueryForm.clientId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="t('entity.serviceContract.clientcode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName')">
      <a-form-item :label="t('entity.serviceContract.clientname')">
        <a-input
          v-model:value="advancedQueryForm.clientName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractType')">
      <a-form-item :label="t('entity.serviceContract.contracttype')">
        <a-input-number
          v-model:value="advancedQueryForm.contractType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contracttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractStatus')">
      <a-form-item :label="t('entity.serviceContract.contractstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.contractStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('signDateStart')">
      <a-form-item :label="t('entity.serviceContract.signdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.signDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.signdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('signDateEnd')">
      <a-form-item :label="t('entity.serviceContract.signdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.signDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.signdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.serviceContract.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.serviceContract.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.serviceContract.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.serviceContract.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractAmount')">
      <a-form-item :label="t('entity.serviceContract.contractamount')">
        <a-input-number
          v-model:value="advancedQueryForm.contractAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.serviceContract.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.currencycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="t('entity.serviceContract.paymentterms')">
        <a-input-number
          v-model:value="advancedQueryForm.paymentTerms"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.paymentterms') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScope')">
      <a-form-item :label="t('entity.serviceContract.servicescope')">
        <a-textarea
          v-model:value="advancedQueryForm.serviceScope"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceContract.servicescope') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('slaResponseHours')">
      <a-form-item :label="t('entity.serviceContract.slaresponsehours')">
        <a-input-number
          v-model:value="advancedQueryForm.slaResponseHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresponsehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('slaResolveHours')">
      <a-form-item :label="t('entity.serviceContract.slaresolvehours')">
        <a-input-number
          v-model:value="advancedQueryForm.slaResolveHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresolvehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountManager')">
      <a-form-item :label="t('entity.serviceContract.accountmanager')">
        <a-input
          v-model:value="advancedQueryForm.accountManager"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.accountmanager') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.serviceContract.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.serviceContract._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.serviceContract._self"
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
      :id-column-key="'serviceContractId'"
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
 * 服务合同实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-contract
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ServiceContractForm from './components/service-contract-form.vue'
import { getServiceContractList, getServiceContractById, createServiceContract, updateServiceContract, deleteServiceContractById, deleteServiceContractBatch, getServiceContractTemplate, importServiceContract, exportServiceContract } from '@/api/logistics/customer-service/service-contract'
import * as serviceOrderApi from '@/api/logistics/customer-service/service-order'
import * as serviceRequestApi from '@/api/logistics/customer-service/service-request'
import type { ServiceOrder, ServiceOrderQuery } from '@/types/logistics/customer-service/service-order'
import type { ServiceRequest, ServiceRequestQuery } from '@/types/logistics/customer-service/service-request'
import type { ServiceContract, ServiceContractQuery, ServiceContractCreate, ServiceContractUpdate } from '@/types/logistics/customer-service/service-contract'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceContract')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.serviceContract._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ServiceContract[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ServiceContract | null>(null)
/** 表格多选行 */
const selectedRows = ref<ServiceContract[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ServiceContract>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  serviceContractCode: '',
  contractName: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  contractType: undefined as number | undefined,
  contractStatus: undefined as number | undefined,
  signDateStart: '',
  signDateEnd: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  contractAmount: undefined as number | undefined,
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  serviceScope: '',
  slaResponseHours: undefined as number | undefined,
  slaResolveHours: undefined as number | undefined,
  accountManager: '',
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.serviceContract.plantcode') },
  { key: 'serviceContractCode', label: t('entity.serviceContract.code') },
  { key: 'contractName', label: t('entity.serviceContract.contractname') },
  { key: 'clientId', label: t('entity.serviceContract.clientid') },
  { key: 'clientCode', label: t('entity.serviceContract.clientcode') },
  { key: 'clientName', label: t('entity.serviceContract.clientname') },
  { key: 'contractType', label: t('entity.serviceContract.contracttype') },
  { key: 'contractStatus', label: t('entity.serviceContract.contractstatus') },
  { key: 'signDateStart', label: t('entity.serviceContract.signdatestart') },
  { key: 'signDateEnd', label: t('entity.serviceContract.signdateend') },
  { key: 'effectiveDateStart', label: t('entity.serviceContract.effectivedatestart') },
  { key: 'effectiveDateEnd', label: t('entity.serviceContract.effectivedateend') },
  { key: 'expiryDateStart', label: t('entity.serviceContract.expirydatestart') },
  { key: 'expiryDateEnd', label: t('entity.serviceContract.expirydateend') },
  { key: 'contractAmount', label: t('entity.serviceContract.contractamount') },
  { key: 'currencyCode', label: t('entity.serviceContract.currencycode') },
  { key: 'paymentTerms', label: t('entity.serviceContract.paymentterms') },
  { key: 'serviceScope', label: t('entity.serviceContract.servicescope') },
  { key: 'slaResponseHours', label: t('entity.serviceContract.slaresponsehours') },
  { key: 'slaResolveHours', label: t('entity.serviceContract.slaresolvehours') },
  { key: 'accountManager', label: t('entity.serviceContract.accountmanager') },
  { key: 'sortOrder', label: t('entity.serviceContract.sortorder') },
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
const entityIdName = 'serviceContractId'
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

/** 展开行预览：serviceOrder 列 */
const serviceOrderExpandColumns = computed(() => [
  {
    title: t('entity.serviceOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.code'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.servicecontractname'),
    dataIndex: 'serviceContractName',
    key: 'serviceContractName',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceOrder.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    ellipsis: true,
  },
])

/** 展开行预览：serviceRequest 列 */
const serviceRequestExpandColumns = computed(() => [
  {
    title: t('entity.serviceRequest.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.code'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.servicecontractname'),
    dataIndex: 'serviceContractName',
    key: 'serviceContractName',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    ellipsis: true,
  },
  {
    title: t('entity.serviceRequest.requestdate'),
    dataIndex: 'requestDate',
    key: 'requestDate',
    ellipsis: true,
  },
])

/** 读取主表行上的 serviceOrder 子表缓存 */
function getServiceOrderRows(record: ServiceContract): ServiceOrder[] {
  return (record as any)?.serviceOrders ?? []
}

/** 主表行是否已加载 serviceOrder 子表 */
function hasServiceOrderRows(record: ServiceContract): boolean {
  return getServiceOrderRows(record).length > 0
}

/** 读取主表行上的 serviceRequest 子表缓存 */
function getServiceRequestRows(record: ServiceContract): ServiceRequest[] {
  return (record as any)?.serviceRequests ?? []
}

/** 主表行是否已加载 serviceRequest 子表 */
function hasServiceRequestRows(record: ServiceContract): boolean {
  return getServiceRequestRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadServiceContractDetail(record: ServiceContract): Promise<ServiceContract | null> {
  const id = getServiceContractId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getServiceContractById(id)
    const index = dataSource.value.findIndex((row) => getServiceContractId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ServiceContract
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 serviceOrder 子表（ServiceOrderQuery + serviceOrderApi，与主表 ServiceContractQuery 分离） */
async function loadServiceOrderForServiceContract(record: ServiceContract): Promise<ServiceOrder[]> {
  const masterId = getServiceContractId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ServiceOrderQuery = {
      pageIndex: 1,
      pageSize: 500,
      serviceContractId: masterId,
    }
    const result = await serviceOrderApi.getServiceOrderList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getServiceContractId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, serviceOrders: rows } as ServiceContract
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 serviceRequest 子表（ServiceRequestQuery + serviceRequestApi，与主表 ServiceContractQuery 分离） */
async function loadServiceRequestForServiceContract(record: ServiceContract): Promise<ServiceRequest[]> {
  const masterId = getServiceContractId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ServiceRequestQuery = {
      pageIndex: 1,
      pageSize: 500,
      serviceContractId: masterId,
    }
    const result = await serviceRequestApi.getServiceRequestList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getServiceContractId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, serviceRequests: rows } as ServiceContract
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureServiceContractChildrenLoaded(record: ServiceContract) {
  if (!hasServiceOrderRows(record)) {
    await loadServiceOrderForServiceContract(record)
  }
  if (!hasServiceRequestRows(record)) {
    await loadServiceRequestForServiceContract(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: ServiceContract) {
  const key = getServiceContractId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureServiceContractChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'serviceContractId',
    key: 'serviceContractId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'serviceContractId') ?? ''
  },
  {
    title: t('entity.serviceContract.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.serviceContract.code'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'serviceContractCode') ?? ''
  },
  {
    title: t('entity.serviceContract.contractname'),
    dataIndex: 'contractName',
    key: 'contractName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractName') ?? ''
  },
  {
    title: t('entity.serviceContract.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientId') ?? ''
  },
  {
    title: t('entity.serviceContract.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientCode') ?? ''
  },
  {
    title: t('entity.serviceContract.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientName') ?? ''
  },
  {
    title: t('entity.serviceContract.contracttype'),
    dataIndex: 'contractType',
    key: 'contractType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractType') ?? ''
  },
  {
    title: t('entity.serviceContract.contractstatus'),
    dataIndex: 'contractStatus',
    key: 'contractStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractStatus') ?? ''
  },
  {
    title: t('entity.serviceContract.signdate'),
    dataIndex: 'signDate',
    key: 'signDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'signDate') ?? ''
  },
  {
    title: t('entity.serviceContract.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.serviceContract.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.serviceContract.contractamount'),
    dataIndex: 'contractAmount',
    key: 'contractAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractAmount') ?? ''
  },
  {
    title: t('entity.serviceContract.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.serviceContract.paymentterms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'paymentTerms') ?? ''
  },
  {
    title: t('entity.serviceContract.servicescope'),
    dataIndex: 'serviceScope',
    key: 'serviceScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'serviceScope') ?? ''
  },
  {
    title: t('entity.serviceContract.slaresponsehours'),
    dataIndex: 'slaResponseHours',
    key: 'slaResponseHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'slaResponseHours') ?? ''
  },
  {
    title: t('entity.serviceContract.slaresolvehours'),
    dataIndex: 'slaResolveHours',
    key: 'slaResolveHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'slaResolveHours') ?? ''
  },
  {
    title: t('entity.serviceContract.accountmanager'),
    dataIndex: 'accountManager',
    key: 'accountManager',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'accountManager') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:service:servicecontract:update',
        onClick: (record: ServiceContract) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:servicecontract:delete',
        onClick: (record: ServiceContract) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getServiceContractId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getServiceContractField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceContract[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ServiceContract, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getServiceContractId(selectedRow.value) === getServiceContractId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ServiceContract[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ServiceContract) => ({
  onClick: () => {
    const key = getServiceContractId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getServiceContractId(item)))
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
    const params: ServiceContractQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getServiceContractList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ServiceContract] 加载数据失败', { error })
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
  serviceContractCode: '',
  contractName: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  contractType: undefined as number | undefined,
  contractStatus: undefined as number | undefined,
  signDateStart: '',
  signDateEnd: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  contractAmount: undefined as number | undefined,
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  serviceScope: '',
  slaResponseHours: undefined as number | undefined,
  slaResolveHours: undefined as number | undefined,
  accountManager: '',
  sortOrder: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.serviceContract._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ServiceContract) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.serviceContract._self') })
  formLoading.value = true
  try {
    const detail = await loadServiceContractDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.serviceContract._self') }))
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
      await updateServiceContract(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.serviceContract._self') }))
    } else {
      await createServiceContract(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.serviceContract._self') }))
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
  const res = await getServiceContractTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importServiceContract(file, sheetName)
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
    const exportQuery: ServiceContractQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportServiceContract(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.serviceContract._self') }))
  } catch (error: any) {
    logger.error('[ServiceContract] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.serviceContract._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ServiceContract) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.serviceContract._self'), name: t('common.tip.this.target', { target: t('entity.serviceContract._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceContractById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.serviceContract._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.serviceContract._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.serviceContract._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteServiceContractBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.serviceContract._self') }))
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
  serviceContractCode: '',
  contractName: '',
  clientId: '',
  clientCode: '',
  clientName: '',
  contractType: undefined as number | undefined,
  contractStatus: undefined as number | undefined,
  signDateStart: '',
  signDateEnd: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  contractAmount: undefined as number | undefined,
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  serviceScope: '',
  slaResponseHours: undefined as number | undefined,
  slaResolveHours: undefined as number | undefined,
  accountManager: '',
  sortOrder: undefined as number | undefined,
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
.logistics-service-service-contract {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
