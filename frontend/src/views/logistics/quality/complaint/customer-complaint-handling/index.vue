<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-handling -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客诉处理记录实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-complaint-customer-complaint-handling">
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
      create-permission="logistics:quality:complaint:customercomplainthandling:create"
      update-permission="logistics:quality:complaint:customercomplainthandling:update"
      delete-permission="logistics:quality:complaint:customercomplainthandling:delete"
      import-permission="logistics:quality:complaint:customercomplainthandling:import"
      export-permission="logistics:quality:complaint:customercomplainthandling:export"
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
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'customerComplaintHandlingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCustomerComplaintHandlingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <CustomerComplaintHandlingForm
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-customer-complaint-handling'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('complaintHandlingCode')">
      <a-form-item :label="t('entity.customerComplaintHandling.complainthandlingcode')">
        <a-input
          v-model:value="advancedQueryForm.complaintHandlingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complainthandlingcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintId')">
      <a-form-item :label="t('entity.customerComplaintHandling.complaintid')">
        <a-input
          v-model:value="advancedQueryForm.complaintId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintNo')">
      <a-form-item :label="t('entity.customerComplaintHandling.complaintno')">
        <a-input
          v-model:value="advancedQueryForm.complaintNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintItemId')">
      <a-form-item :label="t('entity.customerComplaintHandling.complaintitemid')">
        <a-input
          v-model:value="advancedQueryForm.complaintItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintitemid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStage')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingstage')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingStage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingstage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingMethod')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingDescription')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.handlingDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerComplaintHandling.handlingdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('causeAnalysis')">
      <a-form-item :label="t('entity.customerComplaintHandling.causeanalysis')">
        <a-input
          v-model:value="advancedQueryForm.causeAnalysis"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.causeanalysis') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('correctiveAction')">
      <a-form-item :label="t('entity.customerComplaintHandling.correctiveaction')">
        <a-input
          v-model:value="advancedQueryForm.correctiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.correctiveaction') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('preventiveAction')">
      <a-form-item :label="t('entity.customerComplaintHandling.preventiveaction')">
        <a-input
          v-model:value="advancedQueryForm.preventiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.preventiveaction') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDept')">
      <a-form-item :label="t('entity.customerComplaintHandling.responsibledept')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.responsibledept') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleBy')">
      <a-form-item :label="t('entity.customerComplaintHandling.responsibleby')">
        <a-input
          v-model:value="advancedQueryForm.responsibleBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.responsibleby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlerBy')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlerby')">
        <a-input
          v-model:value="advancedQueryForm.handlerBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlerby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateStart')">
      <a-form-item :label="t('entity.customerComplaintHandling.plannedcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.plannedcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateEnd')">
      <a-form-item :label="t('entity.customerComplaintHandling.plannedcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.plannedcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateStart')">
      <a-form-item :label="t('entity.customerComplaintHandling.actualcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.actualcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateEnd')">
      <a-form-item :label="t('entity.customerComplaintHandling.actualcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.actualcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStatus')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingCost')">
      <a-form-item :label="t('entity.customerComplaintHandling.handlingcost')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerFeedback')">
      <a-form-item :label="t('entity.customerComplaintHandling.customerfeedback')">
        <a-input
          v-model:value="advancedQueryForm.customerFeedback"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.customerfeedback') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerSatisfaction')">
      <a-form-item :label="t('entity.customerComplaintHandling.customersatisfaction')">
        <a-input-number
          v-model:value="advancedQueryForm.customerSatisfaction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.customersatisfaction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachmentPaths')">
      <a-form-item :label="t('entity.customerComplaintHandling.attachmentpaths')">
        <a-input
          v-model:value="advancedQueryForm.attachmentPaths"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.attachmentpaths') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customerComplaintHandling._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customerComplaintHandling._self"
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
      :id-column-key="'customerComplaintHandlingId'"
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
 * 客诉处理记录实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint-handling
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import CustomerComplaintHandlingForm from './components/customer-complaint-handling-form.vue'
import { getCustomerComplaintHandlingList, getCustomerComplaintHandlingById, createCustomerComplaintHandling, updateCustomerComplaintHandling, deleteCustomerComplaintHandlingById, deleteCustomerComplaintHandlingBatch, getCustomerComplaintHandlingTemplate, importCustomerComplaintHandling, exportCustomerComplaintHandling } from '@/api/logistics/quality/complaint/customer-complaint-handling'
import type { CustomerComplaintHandling, CustomerComplaintHandlingQuery, CustomerComplaintHandlingCreate, CustomerComplaintHandlingUpdate } from '@/types/logistics/quality/complaint/customer-complaint-handling'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaintHandling')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customerComplaintHandling._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerComplaintHandling[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CustomerComplaintHandling | null>(null)
/** 表格多选行 */
const selectedRows = ref<CustomerComplaintHandling[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerComplaintHandling>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  complaintHandlingCode: '',
  complaintId: '',
  complaintNo: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  attachmentPaths: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'complaintHandlingCode', label: t('entity.customerComplaintHandling.complainthandlingcode') },
  { key: 'complaintId', label: t('entity.customerComplaintHandling.complaintid') },
  { key: 'complaintNo', label: t('entity.customerComplaintHandling.complaintno') },
  { key: 'complaintItemId', label: t('entity.customerComplaintHandling.complaintitemid') },
  { key: 'handlingStage', label: t('entity.customerComplaintHandling.handlingstage') },
  { key: 'handlingMethod', label: t('entity.customerComplaintHandling.handlingmethod') },
  { key: 'handlingDescription', label: t('entity.customerComplaintHandling.handlingdescription') },
  { key: 'causeAnalysis', label: t('entity.customerComplaintHandling.causeanalysis') },
  { key: 'correctiveAction', label: t('entity.customerComplaintHandling.correctiveaction') },
  { key: 'preventiveAction', label: t('entity.customerComplaintHandling.preventiveaction') },
  { key: 'responsibleDept', label: t('entity.customerComplaintHandling.responsibledept') },
  { key: 'responsibleBy', label: t('entity.customerComplaintHandling.responsibleby') },
  { key: 'handlerBy', label: t('entity.customerComplaintHandling.handlerby') },
  { key: 'handlingAtStart', label: t('entity.customerComplaintHandling.handlingatstart') },
  { key: 'handlingAtEnd', label: t('entity.customerComplaintHandling.handlingatend') },
  { key: 'plannedCompletionDateStart', label: t('entity.customerComplaintHandling.plannedcompletiondatestart') },
  { key: 'plannedCompletionDateEnd', label: t('entity.customerComplaintHandling.plannedcompletiondateend') },
  { key: 'actualCompletionDateStart', label: t('entity.customerComplaintHandling.actualcompletiondatestart') },
  { key: 'actualCompletionDateEnd', label: t('entity.customerComplaintHandling.actualcompletiondateend') },
  { key: 'handlingStatus', label: t('entity.customerComplaintHandling.handlingstatus') },
  { key: 'handlingCost', label: t('entity.customerComplaintHandling.handlingcost') },
  { key: 'customerFeedback', label: t('entity.customerComplaintHandling.customerfeedback') },
  { key: 'customerSatisfaction', label: t('entity.customerComplaintHandling.customersatisfaction') },
  { key: 'attachmentPaths', label: t('entity.customerComplaintHandling.attachmentpaths') },
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
const entityIdName = 'customerComplaintHandlingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)


/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerComplaintHandlingId',
    key: 'customerComplaintHandlingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerComplaintHandlingId') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complainthandlingcode'),
    dataIndex: 'complaintHandlingCode',
    key: 'complaintHandlingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintHandlingCode') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaintid'),
    dataIndex: 'complaintId',
    key: 'complaintId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintId') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaintname'),
    dataIndex: 'complaintName',
    key: 'complaintName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintName') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaintno'),
    dataIndex: 'complaintNo',
    key: 'complaintNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintNo') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaintitemid'),
    dataIndex: 'complaintItemId',
    key: 'complaintItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintItemId') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaintitemname'),
    dataIndex: 'complaintItemName',
    key: 'complaintItemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintItemName') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingstage'),
    dataIndex: 'handlingStage',
    key: 'handlingStage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingStage') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingmethod'),
    dataIndex: 'handlingMethod',
    key: 'handlingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingMethod') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingdescription'),
    dataIndex: 'handlingDescription',
    key: 'handlingDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingDescription') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.causeanalysis'),
    dataIndex: 'causeAnalysis',
    key: 'causeAnalysis',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'causeAnalysis') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.correctiveaction'),
    dataIndex: 'correctiveAction',
    key: 'correctiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'correctiveAction') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.preventiveaction'),
    dataIndex: 'preventiveAction',
    key: 'preventiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'preventiveAction') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.responsibledept'),
    dataIndex: 'responsibleDept',
    key: 'responsibleDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'responsibleDept') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.responsibleby'),
    dataIndex: 'responsibleBy',
    key: 'responsibleBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'responsibleBy') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlerby'),
    dataIndex: 'handlerBy',
    key: 'handlerBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlerBy') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingat'),
    dataIndex: 'handlingAt',
    key: 'handlingAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingAt') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.plannedcompletiondate'),
    dataIndex: 'plannedCompletionDate',
    key: 'plannedCompletionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'plannedCompletionDate') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.actualcompletiondate'),
    dataIndex: 'actualCompletionDate',
    key: 'actualCompletionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'actualCompletionDate') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingstatus'),
    dataIndex: 'handlingStatus',
    key: 'handlingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingStatus') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.handlingcost'),
    dataIndex: 'handlingCost',
    key: 'handlingCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingCost') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.customerfeedback'),
    dataIndex: 'customerFeedback',
    key: 'customerFeedback',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerFeedback') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.customersatisfaction'),
    dataIndex: 'customerSatisfaction',
    key: 'customerSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerSatisfaction') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.attachmentpaths'),
    dataIndex: 'attachmentPaths',
    key: 'attachmentPaths',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'attachmentPaths') ?? ''
  },
  {
    title: t('entity.customerComplaintHandling.complaint'),
    dataIndex: 'complaint',
    key: 'complaint',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaint') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customercomplainthandling:update',
        onClick: (record: CustomerComplaintHandling) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customercomplainthandling:delete',
        onClick: (record: CustomerComplaintHandling) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerComplaintHandlingId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerComplaintHandlingField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerComplaintHandling[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerComplaintHandling, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCustomerComplaintHandlingId(selectedRow.value) === getCustomerComplaintHandlingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerComplaintHandling[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CustomerComplaintHandling) => ({
  onClick: () => {
    const key = getCustomerComplaintHandlingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCustomerComplaintHandlingId(item)))
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
    const params: CustomerComplaintHandlingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCustomerComplaintHandlingList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CustomerComplaintHandling] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  complaintHandlingCode: '',
  complaintId: '',
  complaintNo: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  attachmentPaths: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customerComplaintHandling._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: CustomerComplaintHandling) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customerComplaintHandling._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.customerComplaintHandling._self') }))
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
      await updateCustomerComplaintHandling(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.customerComplaintHandling._self') }))
    } else {
      await createCustomerComplaintHandling(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.customerComplaintHandling._self') }))
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
  const res = await getCustomerComplaintHandlingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerComplaintHandling(file, sheetName)
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
    const exportQuery: CustomerComplaintHandlingQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCustomerComplaintHandling(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.customerComplaintHandling._self') }))
  } catch (error: any) {
    logger.error('[CustomerComplaintHandling] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.customerComplaintHandling._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerComplaintHandling) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.customerComplaintHandling._self'), name: t('common.tip.this.target', { target: t('entity.customerComplaintHandling._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintHandlingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.customerComplaintHandling._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.customerComplaintHandling._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.customerComplaintHandling._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerComplaintHandlingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customerComplaintHandling._self') }))
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
  complaintHandlingCode: '',
  complaintId: '',
  complaintNo: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  attachmentPaths: '',
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
.logistics-quality-complaint-customer-complaint-handling {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
