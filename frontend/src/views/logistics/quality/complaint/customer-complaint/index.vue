<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客诉主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-complaint-customer-complaint">
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
      create-permission="logistics:quality:complaint:customercomplaint:create"
      update-permission="logistics:quality:complaint:customercomplaint:update"
      delete-permission="logistics:quality:complaint:customercomplaint:delete"
      import-permission="logistics:quality:complaint:customercomplaint:import"
      export-permission="logistics:quality:complaint:customercomplaint:export"
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
      :id-column-key="'customerComplaintId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCustomerComplaintId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.customerComplaintItem._self') }}</div>
          <a-table
            v-if="hasCustomerComplaintItemRows(record)"
            :columns="customerComplaintItemExpandColumns"
            :data-source="getCustomerComplaintItemRows(record)"
            :row-key="(row: CustomerComplaintItem, index?: number) => row?.customerComplaintItemId || String(index ?? 0)"
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
      <CustomerComplaintForm
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
      <a-form-item :label="t('entity.customerComplaint.code')">
        <a-input
          v-model:value="advancedQueryForm.customerComplaintCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerId')">
      <a-form-item :label="t('entity.customerComplaint.customerid')">
        <a-input
          v-model:value="advancedQueryForm.customerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.customerComplaint.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.customerComplaint.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateStart')">
      <a-form-item :label="t('entity.customerComplaint.complaintdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateEnd')">
      <a-form-item :label="t('entity.customerComplaint.complaintdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintMethod')">
      <a-form-item :label="t('entity.customerComplaint.complaintmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintType')">
      <a-form-item :label="t('entity.customerComplaint.complainttype')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complainttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintLevel')">
      <a-form-item :label="t('entity.customerComplaint.complaintlevel')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintlevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptId')">
      <a-form-item :label="t('entity.customerComplaint.responsibledeptid')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsibledeptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptName')">
      <a-form-item :label="t('entity.customerComplaint.responsibledeptname')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsibledeptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsiblePersonId')">
      <a-form-item :label="t('entity.customerComplaint.responsiblepersonid')">
        <a-input
          v-model:value="advancedQueryForm.responsiblePersonId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsiblepersonid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsiblePersonName')">
      <a-form-item :label="t('entity.customerComplaint.responsiblepersonname')">
        <a-input
          v-model:value="advancedQueryForm.responsiblePersonName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsiblepersonname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateStart')">
      <a-form-item :label="t('entity.customerComplaint.requiredreplydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.requiredreplydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateEnd')">
      <a-form-item :label="t('entity.customerComplaint.requiredreplydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.requiredreplydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateStart')">
      <a-form-item :label="t('entity.customerComplaint.actualreplydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.actualreplydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateEnd')">
      <a-form-item :label="t('entity.customerComplaint.actualreplydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.actualreplydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintStatus')">
      <a-form-item :label="t('entity.customerComplaint.complaintstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.complaintStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDescription')">
      <a-form-item :label="t('entity.customerComplaint.complaintdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.complaintDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerComplaint.complaintdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingResult')">
      <a-form-item :label="t('entity.customerComplaint.handlingresult')">
        <a-input
          v-model:value="advancedQueryForm.handlingResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.handlingresult') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerSatisfaction')">
      <a-form-item :label="t('entity.customerComplaint.customersatisfaction')">
        <a-input-number
          v-model:value="advancedQueryForm.customerSatisfaction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customersatisfaction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.customerComplaint.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.customerComplaint.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customerComplaint._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customerComplaint._self"
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
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import CustomerComplaintForm from './components/customer-complaint-form.vue'
import { getCustomerComplaintList, getCustomerComplaintById, createCustomerComplaint, updateCustomerComplaint, deleteCustomerComplaintById, deleteCustomerComplaintBatch, getCustomerComplaintTemplate, importCustomerComplaint, exportCustomerComplaint } from '@/api/logistics/quality/complaint/customer-complaint'
import * as customerComplaintItemApi from '@/api/logistics/quality/complaint/customer-complaint-item'
import type { CustomerComplaintItem, CustomerComplaintItemQuery } from '@/types/logistics/quality/complaint/customer-complaint-item'
import type { CustomerComplaint, CustomerComplaintQuery, CustomerComplaintCreate, CustomerComplaintUpdate } from '@/types/logistics/quality/complaint/customer-complaint'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaint')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customerComplaint._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerComplaint[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
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
const formData = ref<Partial<CustomerComplaint>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
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
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'customerComplaintCode', label: t('entity.customerComplaint.code') },
  { key: 'customerId', label: t('entity.customerComplaint.customerid') },
  { key: 'customerName', label: t('entity.customerComplaint.customername') },
  { key: 'customerCode', label: t('entity.customerComplaint.customercode') },
  { key: 'complaintDateStart', label: t('entity.customerComplaint.complaintdatestart') },
  { key: 'complaintDateEnd', label: t('entity.customerComplaint.complaintdateend') },
  { key: 'complaintMethod', label: t('entity.customerComplaint.complaintmethod') },
  { key: 'complaintType', label: t('entity.customerComplaint.complainttype') },
  { key: 'complaintLevel', label: t('entity.customerComplaint.complaintlevel') },
  { key: 'responsibleDeptId', label: t('entity.customerComplaint.responsibledeptid') },
  { key: 'responsibleDeptName', label: t('entity.customerComplaint.responsibledeptname') },
  { key: 'responsiblePersonId', label: t('entity.customerComplaint.responsiblepersonid') },
  { key: 'responsiblePersonName', label: t('entity.customerComplaint.responsiblepersonname') },
  { key: 'requiredReplyDateStart', label: t('entity.customerComplaint.requiredreplydatestart') },
  { key: 'requiredReplyDateEnd', label: t('entity.customerComplaint.requiredreplydateend') },
  { key: 'actualReplyDateStart', label: t('entity.customerComplaint.actualreplydatestart') },
  { key: 'actualReplyDateEnd', label: t('entity.customerComplaint.actualreplydateend') },
  { key: 'complaintStatus', label: t('entity.customerComplaint.complaintstatus') },
  { key: 'complaintDescription', label: t('entity.customerComplaint.complaintdescription') },
  { key: 'handlingResult', label: t('entity.customerComplaint.handlingresult') },
  { key: 'customerSatisfaction', label: t('entity.customerComplaint.customersatisfaction') },
  { key: 'relatedPlant', label: t('entity.customerComplaint.relatedplant') },
  { key: 'sortOrder', label: t('entity.customerComplaint.sortorder') },
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
const entityIdName = 'customerComplaintId'
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

/** 展开行预览：customerComplaintItem 列 */
const customerComplaintItemExpandColumns = computed(() => [
  {
    title: t('entity.customerComplaintItem.complaintid'),
    dataIndex: 'complaintId',
    key: 'complaintId',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.complaintname'),
    dataIndex: 'complaintName',
    key: 'complaintName',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.customercomplaintcode'),
    dataIndex: 'customerComplaintCode',
    key: 'customerComplaintCode',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.productcode'),
    dataIndex: 'productCode',
    key: 'productCode',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.productname'),
    dataIndex: 'productName',
    key: 'productName',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    ellipsis: true,
  },
  {
    title: t('entity.customerComplaintItem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    ellipsis: true,
  },
])

/** 读取主表行上的 customerComplaintItem 子表缓存 */
function getCustomerComplaintItemRows(record: CustomerComplaint): CustomerComplaintItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 customerComplaintItem 子表 */
function hasCustomerComplaintItemRows(record: CustomerComplaint): boolean {
  return getCustomerComplaintItemRows(record).length > 0
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
/** 懒加载 customerComplaintItem 子表（CustomerComplaintItemQuery + customerComplaintItemApi，与主表 CustomerComplaintQuery 分离） */
async function loadCustomerComplaintItemForCustomerComplaint(record: CustomerComplaint): Promise<CustomerComplaintItem[]> {
  const masterId = getCustomerComplaintId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: CustomerComplaintItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      customerComplaintCode: masterId,
    }
    const result = await customerComplaintItemApi.getCustomerComplaintItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getCustomerComplaintId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as CustomerComplaint
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureCustomerComplaintChildrenLoaded(record: CustomerComplaint) {
  if (!hasCustomerComplaintItemRows(record)) {
    await loadCustomerComplaintItemForCustomerComplaint(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: CustomerComplaint) {
  const key = getCustomerComplaintId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureCustomerComplaintChildrenLoaded(record)
  expandedRowKeys.value = [key]
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
    title: t('entity.customerComplaint.code'),
    dataIndex: 'customerComplaintCode',
    key: 'customerComplaintCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerComplaintCode') ?? ''
  },
  {
    title: t('entity.customerComplaint.customerid'),
    dataIndex: 'customerId',
    key: 'customerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerId') ?? ''
  },
  {
    title: t('entity.customerComplaint.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.customerComplaint.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.customerComplaint.complaintdate'),
    dataIndex: 'complaintDate',
    key: 'complaintDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDate') ?? ''
  },
  {
    title: t('entity.customerComplaint.complaintmethod'),
    dataIndex: 'complaintMethod',
    key: 'complaintMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintMethod') ?? ''
  },
  {
    title: t('entity.customerComplaint.complainttype'),
    dataIndex: 'complaintType',
    key: 'complaintType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintType') ?? ''
  },
  {
    title: t('entity.customerComplaint.complaintlevel'),
    dataIndex: 'complaintLevel',
    key: 'complaintLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintLevel') ?? ''
  },
  {
    title: t('entity.customerComplaint.responsibledeptid'),
    dataIndex: 'responsibleDeptId',
    key: 'responsibleDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptId') ?? ''
  },
  {
    title: t('entity.customerComplaint.responsibledeptname'),
    dataIndex: 'responsibleDeptName',
    key: 'responsibleDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptName') ?? ''
  },
  {
    title: t('entity.customerComplaint.responsiblepersonid'),
    dataIndex: 'responsiblePersonId',
    key: 'responsiblePersonId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsiblePersonId') ?? ''
  },
  {
    title: t('entity.customerComplaint.responsiblepersonname'),
    dataIndex: 'responsiblePersonName',
    key: 'responsiblePersonName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsiblePersonName') ?? ''
  },
  {
    title: t('entity.customerComplaint.requiredreplydate'),
    dataIndex: 'requiredReplyDate',
    key: 'requiredReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'requiredReplyDate') ?? ''
  },
  {
    title: t('entity.customerComplaint.actualreplydate'),
    dataIndex: 'actualReplyDate',
    key: 'actualReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'actualReplyDate') ?? ''
  },
  {
    title: t('entity.customerComplaint.complaintstatus'),
    dataIndex: 'complaintStatus',
    key: 'complaintStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintStatus') ?? ''
  },
  {
    title: t('entity.customerComplaint.complaintdescription'),
    dataIndex: 'complaintDescription',
    key: 'complaintDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDescription') ?? ''
  },
  {
    title: t('entity.customerComplaint.handlingresult'),
    dataIndex: 'handlingResult',
    key: 'handlingResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'handlingResult') ?? ''
  },
  {
    title: t('entity.customerComplaint.customersatisfaction'),
    dataIndex: 'customerSatisfaction',
    key: 'customerSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerSatisfaction') ?? ''
  },
  {
    title: t('entity.customerComplaint.relatedplant'),
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
        permission: 'logistics:quality:complaint:customercomplaint:update',
        onClick: (record: CustomerComplaint) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customercomplaint:delete',
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
  },
  onSelect: (record: CustomerComplaint, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCustomerComplaintId(selectedRow.value) === getCustomerComplaintId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerComplaint[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CustomerComplaint) => ({
  onClick: () => {
    const key = getCustomerComplaintId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCustomerComplaintId(item)))
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
    const params: CustomerComplaintQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCustomerComplaintList(params)
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
  currentPage.value = 1
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customerComplaint._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CustomerComplaint) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customerComplaint._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.customerComplaint._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.customerComplaint._self') }))
    } else {
      await createCustomerComplaint(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.customerComplaint._self') }))
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: CustomerComplaintQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCustomerComplaint(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.customerComplaint._self') }))
  } catch (error: any) {
    logger.error('[CustomerComplaint] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.customerComplaint._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerComplaint) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.customerComplaint._self'), name: t('common.tip.this.target', { target: t('entity.customerComplaint._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.customerComplaint._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.customerComplaint._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.customerComplaint._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerComplaintBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customerComplaint._self') }))
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
.logistics-quality-complaint-customer-complaint {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
