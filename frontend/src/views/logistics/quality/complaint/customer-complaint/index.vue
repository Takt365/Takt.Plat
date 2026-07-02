<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客诉主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:quality:complaint:customer:create"
      update-permission="logistics:quality:complaint:customer:update"
      delete-permission="logistics:quality:complaint:customer:delete"
      import-permission="logistics:quality:complaint:customer:import"
      export-permission="logistics:quality:complaint:customer:export"
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
      :master-row-key="getCustomerComplaintId"
      :master-row-selection="rowSelection"
      master-id-column-key="customerComplaintId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <CustomerComplaintItemPanel
          ref="customerComplaintItemPanelRef"
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
      <CustomerComplaintForm
        :key="formData?.customerComplaintId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-customer-complaint'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('customerComplaintCode')">
      <a-form-item :label="t('entity.customercomplaint.code')">
        <a-input
          v-model:value="advancedQueryForm.customerComplaintCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerId')">
      <a-form-item :label="t('entity.customercomplaint.customerid')">
        <a-input
          v-model:value="advancedQueryForm.customerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customerid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.customercomplaint.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.customercomplaint.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateStart')">
      <a-form-item :label="t('entity.customercomplaint.complaintdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateEnd')">
      <a-form-item :label="t('entity.customercomplaint.complaintdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintMethod')">
      <a-form-item :label="t('entity.customercomplaint.complaintmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.complaintMethod"
          dict-type="logistics_quality_complaint_method"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintType')">
      <a-form-item :label="t('entity.customercomplaint.complainttype')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complainttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintLevel')">
      <a-form-item :label="t('entity.customercomplaint.complaintlevel')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintlevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptId')">
      <a-form-item :label="t('entity.customercomplaint.responsibledeptid')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsibledeptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptName')">
      <a-form-item :label="t('entity.customercomplaint.responsibledeptname')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsibledeptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsiblePersonId')">
      <a-form-item :label="t('entity.customercomplaint.responsiblepersonid')">
        <a-input
          v-model:value="advancedQueryForm.responsiblePersonId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsiblepersonid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsiblePersonName')">
      <a-form-item :label="t('entity.customercomplaint.responsiblepersonname')">
        <a-input
          v-model:value="advancedQueryForm.responsiblePersonName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsiblepersonname') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateStart')">
      <a-form-item :label="t('entity.customercomplaint.requiredreplydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.requiredreplydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateEnd')">
      <a-form-item :label="t('entity.customercomplaint.requiredreplydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.requiredreplydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateStart')">
      <a-form-item :label="t('entity.customercomplaint.actualreplydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.actualreplydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateEnd')">
      <a-form-item :label="t('entity.customercomplaint.actualreplydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.actualreplydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintStatus')">
      <a-form-item :label="t('entity.customercomplaint.complaintstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDescription')">
      <a-form-item :label="t('entity.customercomplaint.complaintdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.complaintDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customercomplaint.complaintdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingResult')">
      <a-form-item :label="t('entity.customercomplaint.handlingresult')">
        <a-input
          v-model:value="advancedQueryForm.handlingResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.handlingresult') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerSatisfaction')">
      <a-form-item :label="t('entity.customercomplaint.customersatisfaction')">
        <a-input-number
          v-model:value="advancedQueryForm.customerSatisfaction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customersatisfaction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.customercomplaint.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.relatedplant') })"
          show-count
          :maxlength="4"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customercomplaint._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customercomplaint._self"
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
      :id-column-key="'customerComplaintId'"
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
 * 客诉主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint
 */
import { ref, computed, onMounted, h } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import TaktDictTag from '@/components/common/takt-dict-tag/index.vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CustomerComplaintForm from './components/customer-complaint-form.vue'
import CustomerComplaintItemPanel from './components/customer-complaint-item-panel.vue'
import { provideCustomerComplaintMasterContext } from './composables/use-customer-complaint-master-context'
import { getCustomerComplaintList, getCustomerComplaintById, createCustomerComplaint, updateCustomerComplaint, deleteCustomerComplaintById, deleteCustomerComplaintBatch, getCustomerComplaintTemplate, importCustomerComplaint, exportCustomerComplaint, updateCustomerComplaintStatus } from '@/api/logistics/quality/complaint/customer-complaint'
import type { CustomerComplaint, CustomerComplaintQuery } from '@/types/logistics/quality/complaint/customer-complaint'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaint')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customercomplaint._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerComplaint[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CustomerComplaint | null>(null)
/** 表格多选行 */
const selectedRows = ref<CustomerComplaint[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerComplaint> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  customerComplaintCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  complaintDateStart: '',
  complaintDateEnd: '',
  complaintMethod: undefined as number | undefined,
  complaintType: undefined as number | undefined,
  complaintLevel: undefined as number | undefined,
  responsibleDeptId: '',
  responsibleDeptName: '',
  responsiblePersonId: '',
  responsiblePersonName: '',
  requiredReplyDateStart: '',
  requiredReplyDateEnd: '',
  actualReplyDateStart: '',
  actualReplyDateEnd: '',
  complaintStatus: undefined as number | undefined,
  complaintDescription: '',
  handlingResult: '',
  customerSatisfaction: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'customerComplaintCode', label: t('entity.customercomplaint.code') },
  { key: 'customerId', label: t('entity.customercomplaint.customerid') },
  { key: 'customerName', label: t('entity.customercomplaint.customername') },
  { key: 'customerCode', label: t('entity.customercomplaint.customercode') },
  { key: 'complaintDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.complaintdate')) },
  { key: 'complaintDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.complaintdate')) },
  { key: 'complaintMethod', label: t('entity.customercomplaint.complaintmethod') },
  { key: 'complaintType', label: t('entity.customercomplaint.complainttype') },
  { key: 'complaintLevel', label: t('entity.customercomplaint.complaintlevel') },
  { key: 'responsibleDeptId', label: t('entity.customercomplaint.responsibledeptid') },
  { key: 'responsibleDeptName', label: t('entity.customercomplaint.responsibledeptname') },
  { key: 'responsiblePersonId', label: t('entity.customercomplaint.responsiblepersonid') },
  { key: 'responsiblePersonName', label: t('entity.customercomplaint.responsiblepersonname') },
  { key: 'requiredReplyDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.requiredreplydate')) },
  { key: 'requiredReplyDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.requiredreplydate')) },
  { key: 'actualReplyDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.actualreplydate')) },
  { key: 'actualReplyDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplaint.actualreplydate')) },
  { key: 'complaintStatus', label: t('entity.customercomplaint.complaintstatus') },
  { key: 'complaintDescription', label: t('entity.customercomplaint.complaintdescription') },
  { key: 'handlingResult', label: t('entity.customercomplaint.handlingresult') },
  { key: 'customerSatisfaction', label: t('entity.customercomplaint.customersatisfaction') },
  { key: 'relatedPlant', label: t('entity.customercomplaint.relatedplant') },
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
const entityIdName = 'customerComplaintId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCustomerComplaintMasterContext()
const customerComplaintItemPanelRef = ref<InstanceType<typeof CustomerComplaintItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CustomerComplaintQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerComplaintQuery>): CustomerComplaintQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerComplaintQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerComplaintQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('customerComplaintCode', form.customerComplaintCode)
  assignTrimmed('customerId', form.customerId)
  assignTrimmed('customerName', form.customerName)
  assignTrimmed('customerCode', form.customerCode)
  assignTrimmed('complaintDateStart', form.complaintDateStart)
  assignTrimmed('complaintDateEnd', form.complaintDateEnd)
  if (form.complaintMethod !== undefined && form.complaintMethod !== null) {
    query.complaintMethod = form.complaintMethod
  }
  if (form.complaintType !== undefined && form.complaintType !== null) {
    query.complaintType = form.complaintType
  }
  if (form.complaintLevel !== undefined && form.complaintLevel !== null) {
    query.complaintLevel = form.complaintLevel
  }
  assignTrimmed('responsibleDeptId', form.responsibleDeptId)
  assignTrimmed('responsibleDeptName', form.responsibleDeptName)
  assignTrimmed('responsiblePersonId', form.responsiblePersonId)
  assignTrimmed('responsiblePersonName', form.responsiblePersonName)
  assignTrimmed('requiredReplyDateStart', form.requiredReplyDateStart)
  assignTrimmed('requiredReplyDateEnd', form.requiredReplyDateEnd)
  assignTrimmed('actualReplyDateStart', form.actualReplyDateStart)
  assignTrimmed('actualReplyDateEnd', form.actualReplyDateEnd)
  if (form.complaintStatus !== undefined && form.complaintStatus !== null) {
    query.complaintStatus = form.complaintStatus
  }
  assignTrimmed('complaintDescription', form.complaintDescription)
  assignTrimmed('handlingResult', form.handlingResult)
  if (form.customerSatisfaction !== undefined && form.customerSatisfaction !== null) {
    query.customerSatisfaction = form.customerSatisfaction
  }
  assignTrimmed('relatedPlant', form.relatedPlant)
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
function syncMasterSelection(record: CustomerComplaint | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCustomerComplaintId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CustomerComplaint
  const key = getCustomerComplaintId(row)
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
async function loadCustomerComplaintDetail(record: CustomerComplaint): Promise<CustomerComplaint | null> {
  const id = getCustomerComplaintId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCustomerComplaintById(id)
    const index = dataSource.value.findIndex((row) => getCustomerComplaintId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as CustomerComplaint
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
    dataIndex: 'customerComplaintId',
    key: 'customerComplaintId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerComplaintId') ?? ''
  },
  {
    title: t('entity.customercomplaint.code'),
    dataIndex: 'customerComplaintCode',
    key: 'customerComplaintCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerComplaintCode') ?? ''
  },
  {
    title: t('entity.customercomplaint.customerid'),
    dataIndex: 'customerId',
    key: 'customerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerId') ?? ''
  },
  {
    title: t('entity.customercomplaint.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.customercomplaint.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.customercomplaint.complaintdate'),
    dataIndex: 'complaintDate',
    key: 'complaintDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDate') ?? ''
  },
  {
    title: t('entity.customercomplaint.complaintmethod'),
    dataIndex: 'complaintMethod',
    key: 'complaintMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => h(TaktDictTag, {
      value: getCustomerComplaintField(record, 'complaintMethod'),
      dictType: 'logistics_quality_complaint_method',
    }),
  },
  {
    title: t('entity.customercomplaint.complainttype'),
    dataIndex: 'complaintType',
    key: 'complaintType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintType') ?? ''
  },
  {
    title: t('entity.customercomplaint.complaintlevel'),
    dataIndex: 'complaintLevel',
    key: 'complaintLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintLevel') ?? ''
  },
  {
    title: t('entity.customercomplaint.responsibledeptid'),
    dataIndex: 'responsibleDeptId',
    key: 'responsibleDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptId') ?? ''
  },
  {
    title: t('entity.customercomplaint.responsibledeptname'),
    dataIndex: 'responsibleDeptName',
    key: 'responsibleDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptName') ?? ''
  },
  {
    title: t('entity.customercomplaint.responsiblepersonid'),
    dataIndex: 'responsiblePersonId',
    key: 'responsiblePersonId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsiblePersonId') ?? ''
  },
  {
    title: t('entity.customercomplaint.responsiblepersonname'),
    dataIndex: 'responsiblePersonName',
    key: 'responsiblePersonName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsiblePersonName') ?? ''
  },
  {
    title: t('entity.customercomplaint.requiredreplydate'),
    dataIndex: 'requiredReplyDate',
    key: 'requiredReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'requiredReplyDate') ?? ''
  },
  {
    title: t('entity.customercomplaint.actualreplydate'),
    dataIndex: 'actualReplyDate',
    key: 'actualReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'actualReplyDate') ?? ''
  },
  {
    title: t('entity.customercomplaint.complaintstatus'),
    dataIndex: 'complaintStatus',
    key: 'complaintStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintStatus') ?? ''
  },
  {
    title: t('entity.customercomplaint.complaintdescription'),
    dataIndex: 'complaintDescription',
    key: 'complaintDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDescription') ?? ''
  },
  {
    title: t('entity.customercomplaint.handlingresult'),
    dataIndex: 'handlingResult',
    key: 'handlingResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'handlingResult') ?? ''
  },
  {
    title: t('entity.customercomplaint.customersatisfaction'),
    dataIndex: 'customerSatisfaction',
    key: 'customerSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerSatisfaction') ?? ''
  },
  {
    title: t('entity.customercomplaint.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customer:update',
        onClick: (record: CustomerComplaint) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:delete',
        onClick: (record: CustomerComplaint) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerComplaintId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerComplaintField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerComplaint[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CustomerComplaint, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCustomerComplaintId(selectedRow.value) === getCustomerComplaintId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerComplaint[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getCustomerComplaintList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CustomerComplaint] 加载数据失败', { error })
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
  customerComplaintCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  complaintDateStart: '',
  complaintDateEnd: '',
  complaintMethod: undefined as number | undefined,
  complaintType: undefined as number | undefined,
  complaintLevel: undefined as number | undefined,
  responsibleDeptId: '',
  responsibleDeptName: '',
  responsiblePersonId: '',
  responsiblePersonName: '',
  requiredReplyDateStart: '',
  requiredReplyDateEnd: '',
  actualReplyDateStart: '',
  actualReplyDateEnd: '',
  complaintStatus: undefined as number | undefined,
  complaintDescription: '',
  handlingResult: '',
  customerSatisfaction: undefined as number | undefined,
  relatedPlant: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customercomplaint._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CustomerComplaint) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customercomplaint._self') })
  formLoading.value = true
  try {
    const detail = await loadCustomerComplaintDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.customercomplaint._self') }))
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
      await updateCustomerComplaint(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.customercomplaint._self') }))
    } else {
      await createCustomerComplaint(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.customercomplaint._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  customerComplaintItemPanelRef.value?.reload?.()
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
  const res = await getCustomerComplaintTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerComplaint(file, sheetName)
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
    const exportMeta = await exportCustomerComplaint(
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
    message.success(t('common.feedback.export.success', { target: t('entity.customercomplaint._self') }))
  } catch (error: any) {
    logger.error('[CustomerComplaint] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.customercomplaint._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerComplaint) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.customercomplaint._self'), name: t('common.tip.this.target', { target: t('entity.customercomplaint._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplaint._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.customercomplaint._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.customercomplaint._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerComplaintBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplaint._self') }))
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
  customerComplaintCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  complaintDateStart: '',
  complaintDateEnd: '',
  complaintMethod: undefined as number | undefined,
  complaintType: undefined as number | undefined,
  complaintLevel: undefined as number | undefined,
  responsibleDeptId: '',
  responsibleDeptName: '',
  responsiblePersonId: '',
  responsiblePersonName: '',
  requiredReplyDateStart: '',
  requiredReplyDateEnd: '',
  actualReplyDateStart: '',
  actualReplyDateEnd: '',
  complaintStatus: undefined as number | undefined,
  complaintDescription: '',
  handlingResult: '',
  customerSatisfaction: undefined as number | undefined,
  relatedPlant: '',
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
