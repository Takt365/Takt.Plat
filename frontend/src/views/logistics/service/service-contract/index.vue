<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务合同实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:service:contract:create"
      update-permission="logistics:service:contract:update"
      delete-permission="logistics:service:contract:delete"
      import-permission="logistics:service:contract:import"
      export-permission="logistics:service:contract:export"
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
      :id-column-key="'serviceContractId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getServiceContractId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <ServiceContractForm
        :key="formData?.serviceContractId ?? 'create'"
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
      <a-form-item :label="t('entity.servicecontract.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceContractCode')">
      <a-form-item :label="t('entity.servicecontract.code')">
        <a-input
          v-model:value="advancedQueryForm.serviceContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractName')">
      <a-form-item :label="t('entity.servicecontract.contractname')">
        <a-input
          v-model:value="advancedQueryForm.contractName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.contractname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientId')">
      <a-form-item :label="t('entity.servicecontract.clientid')">
        <a-input
          v-model:value="advancedQueryForm.clientId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.clientid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="t('entity.servicecontract.clientcode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.clientcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName')">
      <a-form-item :label="t('entity.servicecontract.clientname')">
        <a-input
          v-model:value="advancedQueryForm.clientName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.clientname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractType')">
      <a-form-item :label="t('entity.servicecontract.contracttype')">
        <a-input-number
          v-model:value="advancedQueryForm.contractType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.contracttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractStatus')">
      <a-form-item :label="t('entity.servicecontract.contractstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.contractStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.contractstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('signDateStart')">
      <a-form-item :label="t('entity.servicecontract.signdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.signDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.signdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('signDateEnd')">
      <a-form-item :label="t('entity.servicecontract.signdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.signDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.signdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.servicecontract.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.servicecontract.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.servicecontract.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.servicecontract.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicecontract.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractAmount')">
      <a-form-item :label="t('entity.servicecontract.contractamount')">
        <a-input-number
          v-model:value="advancedQueryForm.contractAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.contractamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.servicecontract.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.currencycode') })"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="t('entity.servicecontract.paymentterms')">
        <a-input-number
          v-model:value="advancedQueryForm.paymentTerms"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.paymentterms') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScope')">
      <a-form-item :label="t('entity.servicecontract.servicescope')">
        <a-textarea
          v-model:value="advancedQueryForm.serviceScope"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.servicecontract.servicescope') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('slaResponseHours')">
      <a-form-item :label="t('entity.servicecontract.slaresponsehours')">
        <a-input-number
          v-model:value="advancedQueryForm.slaResponseHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.slaresponsehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('slaResolveHours')">
      <a-form-item :label="t('entity.servicecontract.slaresolvehours')">
        <a-input-number
          v-model:value="advancedQueryForm.slaResolveHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.slaresolvehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountManager')">
      <a-form-item :label="t('entity.servicecontract.accountmanager')">
        <a-input
          v-model:value="advancedQueryForm.accountManager"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicecontract.accountmanager') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.servicecontract._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.servicecontract._self"
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
 * 服务合同实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-contract
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ServiceContractForm from './components/service-contract-form.vue'
import { getServiceContractList, getServiceContractById, createServiceContract, updateServiceContract, deleteServiceContractById, deleteServiceContractBatch, getServiceContractTemplate, importServiceContract, exportServiceContract, updateServiceContractStatus } from '@/api/logistics/customer-service/service-contract'
import type { ServiceContract, ServiceContractQuery } from '@/types/logistics/customer-service/service-contract'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktServiceContract')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.servicecontract._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ServiceContract[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<ServiceContract> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.servicecontract.plantcode') },
  { key: 'serviceContractCode', label: t('entity.servicecontract.code') },
  { key: 'contractName', label: t('entity.servicecontract.contractname') },
  { key: 'clientId', label: t('entity.servicecontract.clientid') },
  { key: 'clientCode', label: t('entity.servicecontract.clientcode') },
  { key: 'clientName', label: t('entity.servicecontract.clientname') },
  { key: 'contractType', label: t('entity.servicecontract.contracttype') },
  { key: 'contractStatus', label: t('entity.servicecontract.contractstatus') },
  { key: 'signDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.servicecontract.signdate')) },
  { key: 'signDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.servicecontract.signdate')) },
  { key: 'effectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.servicecontract.effectivedate')) },
  { key: 'effectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.servicecontract.effectivedate')) },
  { key: 'expiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.servicecontract.expirydate')) },
  { key: 'expiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.servicecontract.expirydate')) },
  { key: 'contractAmount', label: t('entity.servicecontract.contractamount') },
  { key: 'currencyCode', label: t('entity.servicecontract.currencycode') },
  { key: 'paymentTerms', label: t('entity.servicecontract.paymentterms') },
  { key: 'serviceScope', label: t('entity.servicecontract.servicescope') },
  { key: 'slaResponseHours', label: t('entity.servicecontract.slaresponsehours') },
  { key: 'slaResolveHours', label: t('entity.servicecontract.slaresolvehours') },
  { key: 'accountManager', label: t('entity.servicecontract.accountmanager') },
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
const entityIdName = 'serviceContractId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ServiceContractQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ServiceContractQuery>): ServiceContractQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ServiceContractQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ServiceContractQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('serviceContractCode', form.serviceContractCode)
  assignTrimmed('contractName', form.contractName)
  assignTrimmed('clientId', form.clientId)
  assignTrimmed('clientCode', form.clientCode)
  assignTrimmed('clientName', form.clientName)
  if (form.contractType !== undefined && form.contractType !== null) {
    query.contractType = form.contractType
  }
  if (form.contractStatus !== undefined && form.contractStatus !== null) {
    query.contractStatus = form.contractStatus
  }
  assignTrimmed('signDateStart', form.signDateStart)
  assignTrimmed('signDateEnd', form.signDateEnd)
  assignTrimmed('effectiveDateStart', form.effectiveDateStart)
  assignTrimmed('effectiveDateEnd', form.effectiveDateEnd)
  assignTrimmed('expiryDateStart', form.expiryDateStart)
  assignTrimmed('expiryDateEnd', form.expiryDateEnd)
  if (form.contractAmount !== undefined && form.contractAmount !== null) {
    query.contractAmount = form.contractAmount
  }
  assignTrimmed('currencyCode', form.currencyCode)
  if (form.paymentTerms !== undefined && form.paymentTerms !== null) {
    query.paymentTerms = form.paymentTerms
  }
  assignTrimmed('serviceScope', form.serviceScope)
  if (form.slaResponseHours !== undefined && form.slaResponseHours !== null) {
    query.slaResponseHours = form.slaResponseHours
  }
  if (form.slaResolveHours !== undefined && form.slaResolveHours !== null) {
    query.slaResolveHours = form.slaResolveHours
  }
  assignTrimmed('accountManager', form.accountManager)
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
    title: t('entity.servicecontract.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.servicecontract.code'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'serviceContractCode') ?? ''
  },
  {
    title: t('entity.servicecontract.contractname'),
    dataIndex: 'contractName',
    key: 'contractName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractName') ?? ''
  },
  {
    title: t('entity.servicecontract.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientId') ?? ''
  },
  {
    title: t('entity.servicecontract.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientCode') ?? ''
  },
  {
    title: t('entity.servicecontract.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'clientName') ?? ''
  },
  {
    title: t('entity.servicecontract.contracttype'),
    dataIndex: 'contractType',
    key: 'contractType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractType') ?? ''
  },
  {
    title: t('entity.servicecontract.contractstatus'),
    dataIndex: 'contractStatus',
    key: 'contractStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractStatus') ?? ''
  },
  {
    title: t('entity.servicecontract.signdate'),
    dataIndex: 'signDate',
    key: 'signDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'signDate') ?? ''
  },
  {
    title: t('entity.servicecontract.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.servicecontract.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.servicecontract.contractamount'),
    dataIndex: 'contractAmount',
    key: 'contractAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'contractAmount') ?? ''
  },
  {
    title: t('entity.servicecontract.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.servicecontract.paymentterms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'paymentTerms') ?? ''
  },
  {
    title: t('entity.servicecontract.servicescope'),
    dataIndex: 'serviceScope',
    key: 'serviceScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'serviceScope') ?? ''
  },
  {
    title: t('entity.servicecontract.slaresponsehours'),
    dataIndex: 'slaResponseHours',
    key: 'slaResponseHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'slaResponseHours') ?? ''
  },
  {
    title: t('entity.servicecontract.slaresolvehours'),
    dataIndex: 'slaResolveHours',
    key: 'slaResolveHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getServiceContractField(record, 'slaResolveHours') ?? ''
  },
  {
    title: t('entity.servicecontract.accountmanager'),
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
        permission: 'logistics:service:contract:update',
        onClick: (record: ServiceContract) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:contract:delete',
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
    } else if (selectedRow.value && getServiceContractId(selectedRow.value) === getServiceContractId(record)) {
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
    const res = await getServiceContractList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.servicecontract._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: ServiceContract) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.servicecontract._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.servicecontract._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.servicecontract._self') }))
    } else {
      await createServiceContract(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.servicecontract._self') }))
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
    const exportMeta = await exportServiceContract(
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
    message.success(t('common.feedback.export.success', { target: t('entity.servicecontract._self') }))
  } catch (error: any) {
    logger.error('[ServiceContract] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.servicecontract._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ServiceContract) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.servicecontract._self'), name: t('common.tip.this.target', { target: t('entity.servicecontract._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceContractById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.servicecontract._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.servicecontract._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.servicecontract._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteServiceContractBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.servicecontract._self') }))
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
