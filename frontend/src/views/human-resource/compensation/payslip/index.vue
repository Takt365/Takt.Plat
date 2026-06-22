<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/payslip -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工薪资条管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-compensation-payslip">
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
      create-permission="humanresource:training:course:create"
      update-permission="humanresource:training:course:update"
      delete-permission="humanresource:training:course:delete"
      import-permission="humanresource:training:course:import"
      export-permission="humanresource:training:course:export"
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
      :id-column-key="'payslipId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPayslipId"
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
      <PayslipForm
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
      :storage-key="'takt-query-fields-human-resource-compensation-payslip'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.payslip.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.payslip.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('payPeriod')">
      <a-form-item :label="t('entity.payslip.payperiod')">
        <a-input
          v-model:value="advancedQueryForm.payPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.payperiod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseSalary')">
      <a-form-item :label="t('entity.payslip.basesalary')">
        <a-input-number
          v-model:value="advancedQueryForm.baseSalary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.basesalary') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('positionAllowance')">
      <a-form-item :label="t('entity.payslip.positionallowance')">
        <a-input-number
          v-model:value="advancedQueryForm.positionAllowance"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.positionallowance') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('performanceBonus')">
      <a-form-item :label="t('entity.payslip.performancebonus')">
        <a-input-number
          v-model:value="advancedQueryForm.performanceBonus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.performancebonus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimePay')">
      <a-form-item :label="t('entity.payslip.overtimepay')">
        <a-input-number
          v-model:value="advancedQueryForm.overtimePay"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.overtimepay') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowanceTotal')">
      <a-form-item :label="t('entity.payslip.allowancetotal')">
        <a-input-number
          v-model:value="advancedQueryForm.allowanceTotal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.allowancetotal') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossAmount')">
      <a-form-item :label="t('entity.payslip.grossamount')">
        <a-input-number
          v-model:value="advancedQueryForm.grossAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.grossamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('socialSecurityDeduction')">
      <a-form-item :label="t('entity.payslip.socialsecuritydeduction')">
        <a-input-number
          v-model:value="advancedQueryForm.socialSecurityDeduction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.socialsecuritydeduction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('housingFundDeduction')">
      <a-form-item :label="t('entity.payslip.housingfunddeduction')">
        <a-input-number
          v-model:value="advancedQueryForm.housingFundDeduction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.housingfunddeduction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxDeduction')">
      <a-form-item :label="t('entity.payslip.taxdeduction')">
        <a-input-number
          v-model:value="advancedQueryForm.taxDeduction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.taxdeduction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherDeduction')">
      <a-form-item :label="t('entity.payslip.otherdeduction')">
        <a-input-number
          v-model:value="advancedQueryForm.otherDeduction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.otherdeduction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netAmount')">
      <a-form-item :label="t('entity.payslip.netamount')">
        <a-input-number
          v-model:value="advancedQueryForm.netAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.netamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueStatus')">
      <a-form-item :label="t('entity.payslip.issuestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.issueStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.issuestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueDateStart')">
      <a-form-item :label="t('entity.payslip.issuedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.issueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.payslip.issuedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('issueDateEnd')">
      <a-form-item :label="t('entity.payslip.issuedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.issueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.payslip.issuedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.payslip.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.relatedplant') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.ExtField')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.payslip._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.payslip._self"
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
      :id-column-key="'payslipId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 员工薪资条管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/payslip
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PayslipForm from './components/payslip-form.vue'
import { getPayslipList, getPayslipById, createPayslip, updatePayslip, deletePayslipById, deletePayslipBatch, getPayslipTemplate, importPayslip, exportPayslip } from '@/api/human-resource/compensation-benefits/payslip'
import type { Payslip, PayslipQuery, PayslipCreate, PayslipUpdate } from '@/types/human-resource/compensation-benefits/payslip'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPayslip')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.payslip._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Payslip[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Payslip | null>(null)
/** 表格多选行 */
const selectedRows = ref<Payslip[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Payslip>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  baseSalary: undefined as number | undefined,
  positionAllowance: undefined as number | undefined,
  performanceBonus: undefined as number | undefined,
  overtimePay: undefined as number | undefined,
  allowanceTotal: undefined as number | undefined,
  grossAmount: undefined as number | undefined,
  socialSecurityDeduction: undefined as number | undefined,
  housingFundDeduction: undefined as number | undefined,
  taxDeduction: undefined as number | undefined,
  otherDeduction: undefined as number | undefined,
  netAmount: undefined as number | undefined,
  issueStatus: undefined as number | undefined,
  issueDateStart: '',
  issueDateEnd: '',
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.payslip.employeeid') },
  { key: 'employeeName', label: t('entity.payslip.employeename') },
  { key: 'payPeriod', label: t('entity.payslip.payperiod') },
  { key: 'baseSalary', label: t('entity.payslip.basesalary') },
  { key: 'positionAllowance', label: t('entity.payslip.positionallowance') },
  { key: 'performanceBonus', label: t('entity.payslip.performancebonus') },
  { key: 'overtimePay', label: t('entity.payslip.overtimepay') },
  { key: 'allowanceTotal', label: t('entity.payslip.allowancetotal') },
  { key: 'grossAmount', label: t('entity.payslip.grossamount') },
  { key: 'socialSecurityDeduction', label: t('entity.payslip.socialsecuritydeduction') },
  { key: 'housingFundDeduction', label: t('entity.payslip.housingfunddeduction') },
  { key: 'taxDeduction', label: t('entity.payslip.taxdeduction') },
  { key: 'otherDeduction', label: t('entity.payslip.otherdeduction') },
  { key: 'netAmount', label: t('entity.payslip.netamount') },
  { key: 'issueStatus', label: t('entity.payslip.issuestatus') },
  { key: 'issueDateStart', label: t('entity.payslip.issuedatestart') },
  { key: 'issueDateEnd', label: t('entity.payslip.issuedateend') },
  { key: 'relatedPlant', label: t('entity.payslip.relatedplant') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
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
const entityIdName = 'payslipId'
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
    dataIndex: 'payslipId',
    key: 'payslipId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'payslipId') ?? ''
  },
  {
    title: t('entity.payslip.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.payslip.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.payslip.payperiod'),
    dataIndex: 'payPeriod',
    key: 'payPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'payPeriod') ?? ''
  },
  {
    title: t('entity.payslip.basesalary'),
    dataIndex: 'baseSalary',
    key: 'baseSalary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'baseSalary') ?? ''
  },
  {
    title: t('entity.payslip.positionallowance'),
    dataIndex: 'positionAllowance',
    key: 'positionAllowance',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'positionAllowance') ?? ''
  },
  {
    title: t('entity.payslip.performancebonus'),
    dataIndex: 'performanceBonus',
    key: 'performanceBonus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'performanceBonus') ?? ''
  },
  {
    title: t('entity.payslip.overtimepay'),
    dataIndex: 'overtimePay',
    key: 'overtimePay',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'overtimePay') ?? ''
  },
  {
    title: t('entity.payslip.allowancetotal'),
    dataIndex: 'allowanceTotal',
    key: 'allowanceTotal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'allowanceTotal') ?? ''
  },
  {
    title: t('entity.payslip.grossamount'),
    dataIndex: 'grossAmount',
    key: 'grossAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'grossAmount') ?? ''
  },
  {
    title: t('entity.payslip.socialsecuritydeduction'),
    dataIndex: 'socialSecurityDeduction',
    key: 'socialSecurityDeduction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'socialSecurityDeduction') ?? ''
  },
  {
    title: t('entity.payslip.housingfunddeduction'),
    dataIndex: 'housingFundDeduction',
    key: 'housingFundDeduction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'housingFundDeduction') ?? ''
  },
  {
    title: t('entity.payslip.taxdeduction'),
    dataIndex: 'taxDeduction',
    key: 'taxDeduction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'taxDeduction') ?? ''
  },
  {
    title: t('entity.payslip.otherdeduction'),
    dataIndex: 'otherDeduction',
    key: 'otherDeduction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'otherDeduction') ?? ''
  },
  {
    title: t('entity.payslip.netamount'),
    dataIndex: 'netAmount',
    key: 'netAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'netAmount') ?? ''
  },
  {
    title: t('entity.payslip.issuestatus'),
    dataIndex: 'issueStatus',
    key: 'issueStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'issueStatus') ?? ''
  },
  {
    title: t('entity.payslip.issuedate'),
    dataIndex: 'issueDate',
    key: 'issueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'issueDate') ?? ''
  },
  {
    title: t('entity.payslip.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPayslipField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:training:course:update',
        onClick: (record: Payslip) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:training:course:delete',
        onClick: (record: Payslip) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPayslipId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPayslipField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Payslip[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Payslip, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPayslipId(selectedRow.value) === getPayslipId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Payslip[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Payslip) => ({
  onClick: () => {
    const key = getPayslipId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPayslipId(item)))
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
    const params: PayslipQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPayslipList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Payslip] 加载数据失败', { error })
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
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  baseSalary: undefined as number | undefined,
  positionAllowance: undefined as number | undefined,
  performanceBonus: undefined as number | undefined,
  overtimePay: undefined as number | undefined,
  allowanceTotal: undefined as number | undefined,
  grossAmount: undefined as number | undefined,
  socialSecurityDeduction: undefined as number | undefined,
  housingFundDeduction: undefined as number | undefined,
  taxDeduction: undefined as number | undefined,
  otherDeduction: undefined as number | undefined,
  netAmount: undefined as number | undefined,
  issueStatus: undefined as number | undefined,
  issueDateStart: '',
  issueDateEnd: '',
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.payslip._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Payslip) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.payslip._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.payslip._self') }))
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
      await updatePayslip(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.payslip._self') }))
    } else {
      await createPayslip(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.payslip._self') }))
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
  const res = await getPayslipTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPayslip(file, sheetName)
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
    const exportQuery: PayslipQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPayslip(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.payslip._self') }))
  } catch (error: any) {
    logger.error('[Payslip] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.payslip._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Payslip) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.payslip._self'), name: t('common.tip.this.target', { target: t('entity.payslip._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePayslipById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.payslip._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.payslip._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.payslip._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePayslipBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.payslip._self') }))
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
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  baseSalary: undefined as number | undefined,
  positionAllowance: undefined as number | undefined,
  performanceBonus: undefined as number | undefined,
  overtimePay: undefined as number | undefined,
  allowanceTotal: undefined as number | undefined,
  grossAmount: undefined as number | undefined,
  socialSecurityDeduction: undefined as number | undefined,
  housingFundDeduction: undefined as number | undefined,
  taxDeduction: undefined as number | undefined,
  otherDeduction: undefined as number | undefined,
  netAmount: undefined as number | undefined,
  issueStatus: undefined as number | undefined,
  issueDateStart: '',
  issueDateEnd: '',
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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
.human-resource-compensation-payslip {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
