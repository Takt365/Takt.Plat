<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/expense -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：费用单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:financial:expense:create"
      update-permission="accounting:financial:expense:update"
      delete-permission="accounting:financial:expense:delete"
      import-permission="accounting:financial:expense:import"
      export-permission="accounting:financial:expense:export"
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
      :master-row-key="getExpenseId"
      :master-row-selection="rowSelection"
      master-id-column-key="expenseId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'expenseType'">
          <TaktDictTag
            :value="getExpenseField(record, 'expenseType')"
            dict-type="accounting_expense_type"
          />
        </template>
        <template v-else-if="column.key === 'expenseStatus'">
          <TaktDictTag
            :value="getExpenseField(record, 'expenseStatus')"
            dict-type="sys_approval_status"
          />
        </template>
        <template v-else-if="column.key === 'taxRate'">
          <TaktDictTag
            :value="getExpenseField(record, 'taxRate')"
            dict-type="accounting_tax_rate_param"
          />
        </template>
      </template>
      <template #detail>
        <ExpenseDetailPanel
          ref="expenseDetailPanelRef"
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
      <ExpenseForm
        :key="formData?.expenseId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-expense'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('expenseCode')">
      <a-form-item :label="t('entity.expense.code')">
        <a-input
          v-model:value="advancedQueryForm.expenseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.code') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseTitle')">
      <a-form-item :label="t('entity.expense.title')">
        <a-input
          v-model:value="advancedQueryForm.expenseTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.title') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseType')">
      <a-form-item :label="t('entity.expense.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.expenseType"
          dict-type="accounting_expense_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.expense.suppliercode')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierCode"
          api-url="TaktSuppliers/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.expense.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.suppliername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantBy')">
      <a-form-item :label="t('entity.expense.applicantby')">
        <TaktSelect
          v-model:value="advancedQueryForm.applicantBy"
          api-url="TaktEmployees/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.applicantby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationDept')">
      <a-form-item :label="t('entity.expense.applicationdept')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.applicationDept"
          api-url="TaktDepts/tree-options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.applicationdept') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costBearerDept')">
      <a-form-item :label="t('entity.expense.costbearerdept')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.costBearerDept"
          api-url="TaktDepts/tree-options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.costbearerdept') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenter')">
      <a-form-item :label="t('entity.expense.costcenter')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.costCenter"
          api-url="TaktCostCenters/tree-options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.costcenter') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignId')">
      <a-form-item :label="t('entity.expense.countersignid')">
        <TaktSelect
          v-model:value="advancedQueryForm.countersignId"
          api-url="TaktCountersigns/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.countersignid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="t('entity.expense.purchaseordercode')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseOrderCode"
          api-url="TaktPurchaseOrders/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.purchaseordercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestCode')">
      <a-form-item :label="t('entity.expense.purchaserequestcode')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseRequestCode"
          api-url="TaktPurchaseRequests/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.purchaserequestcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseAmount')">
      <a-form-item :label="t('entity.expense.amount')">
        <a-input-number
          v-model:value="advancedQueryForm.expenseAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.amount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="t('entity.expense.taxrate')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxRate"
          dict-type="accounting_tax_rate_param"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.taxrate') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="t('entity.expense.taxamount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.taxamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseDateStart')">
      <a-form-item :label="t('entity.expense.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expenseDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseDateEnd')">
      <a-form-item :label="t('entity.expense.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expenseDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.dateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationReason')">
      <a-form-item :label="t('entity.expense.applicationreason')">
        <a-input
          v-model:value="advancedQueryForm.applicationReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.applicationreason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.expense.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.attachments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.expense.relatedplant')">
        <TaktSelect
          v-model:value="advancedQueryForm.relatedPlant"
          api-url="TaktPlants/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expenseStatus')">
      <a-form-item :label="t('entity.expense.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.expenseStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.expense.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.expense.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.expense.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.expense.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.expense.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.expense.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.expense.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.expense.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.expense.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.expense.flowinstanceid') })"
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

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.expense._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.expense._self"
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
      :id-column-key="'expenseId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 费用单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/expense
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ExpenseForm from './components/expense-form.vue'
import ExpenseDetailPanel from './components/expense-detail-panel.vue'
import { provideExpenseMasterContext } from './composables/use-expense-master-context'
import { getExpenseList, getExpenseById, createExpense, updateExpense, deleteExpenseById, deleteExpenseBatch, getExpenseTemplate, importExpense, exportExpense, updateExpenseStatus } from '@/api/accounting/financial/expense'
import type { Expense, ExpenseQuery } from '@/types/accounting/financial/expense'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktExpense')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.expense._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Expense[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Expense | null>(null)
/** 表格多选行 */
const selectedRows = ref<Expense[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Expense> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  expenseCode: '',
  expenseTitle: '',
  expenseType: undefined as number | undefined,
  supplierCode: '',
  supplierName: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  costCenter: '',
  countersignId: '',
  purchaseOrderCode: '',
  purchaseRequestCode: '',
  expenseAmount: undefined as number | undefined,
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  expenseDateStart: '',
  expenseDateEnd: '',
  applicationReason: '',
  attachments: '',
  relatedPlant: '',
  expenseStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'expenseCode', label: t('entity.expense.code') },
  { key: 'expenseTitle', label: t('entity.expense.title') },
  { key: 'expenseType', label: t('entity.expense.type') },
  { key: 'supplierCode', label: t('entity.expense.suppliercode') },
  { key: 'supplierName', label: t('entity.expense.suppliername') },
  { key: 'applicantBy', label: t('entity.expense.applicantby') },
  { key: 'applicationDept', label: t('entity.expense.applicationdept') },
  { key: 'costBearerDept', label: t('entity.expense.costbearerdept') },
  { key: 'costCenter', label: t('entity.expense.costcenter') },
  { key: 'countersignId', label: t('entity.expense.countersignid') },
  { key: 'purchaseOrderCode', label: t('entity.expense.purchaseordercode') },
  { key: 'purchaseRequestCode', label: t('entity.expense.purchaserequestcode') },
  { key: 'expenseAmount', label: t('entity.expense.amount') },
  { key: 'taxRate', label: t('entity.expense.taxrate') },
  { key: 'taxAmount', label: t('entity.expense.taxamount') },
  { key: 'expenseDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.expense.date')) },
  { key: 'expenseDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.expense.date')) },
  { key: 'applicationReason', label: t('entity.expense.applicationreason') },
  { key: 'attachments', label: t('entity.expense.attachments') },
  { key: 'relatedPlant', label: t('entity.expense.relatedplant') },
  { key: 'expenseStatus', label: t('entity.expense.status') },
  { key: 'approvalStatus', label: t('entity.expense.approvalstatus') },
  { key: 'initiatorId', label: t('entity.expense.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.expense.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.expense.initiatedatend') },
  { key: 'approvedBy', label: t('entity.expense.approvedby') },
  { key: 'approvedAtStart', label: t('entity.expense.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.expense.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.expense.flowinstanceid') },
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
const entityIdName = 'expenseId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideExpenseMasterContext()
const expenseDetailPanelRef = ref<InstanceType<typeof ExpenseDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ExpenseQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ExpenseQuery>): ExpenseQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ExpenseQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ExpenseQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('expenseCode', form.expenseCode)
  assignTrimmed('expenseTitle', form.expenseTitle)
  if (form.expenseType !== undefined && form.expenseType !== null) {
    query.expenseType = form.expenseType
  }
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('supplierName', form.supplierName)
  assignTrimmed('applicantBy', form.applicantBy)
  assignTrimmed('applicationDept', form.applicationDept)
  assignTrimmed('costBearerDept', form.costBearerDept)
  assignTrimmed('costCenter', form.costCenter)
  assignTrimmed('countersignId', form.countersignId)
  assignTrimmed('purchaseOrderCode', form.purchaseOrderCode)
  assignTrimmed('purchaseRequestCode', form.purchaseRequestCode)
  if (form.expenseAmount !== undefined && form.expenseAmount !== null) {
    query.expenseAmount = form.expenseAmount
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  assignTrimmed('expenseDateStart', form.expenseDateStart)
  assignTrimmed('expenseDateEnd', form.expenseDateEnd)
  assignTrimmed('applicationReason', form.applicationReason)
  assignTrimmed('attachments', form.attachments)
  assignTrimmed('relatedPlant', form.relatedPlant)
  if (form.expenseStatus !== undefined && form.expenseStatus !== null) {
    query.expenseStatus = form.expenseStatus
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: Expense | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getExpenseId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as Expense
  const key = getExpenseId(row)
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
async function loadExpenseDetail(record: Expense): Promise<Expense | null> {
  const id = getExpenseId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getExpenseById(id)
    const index = dataSource.value.findIndex((row) => getExpenseId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Expense
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
    dataIndex: 'expenseId',
    key: 'expenseId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseId') ?? ''
  },
  {
    title: t('entity.expense.code'),
    dataIndex: 'expenseCode',
    key: 'expenseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseCode') ?? ''
  },
  {
    title: t('entity.expense.title'),
    dataIndex: 'expenseTitle',
    key: 'expenseTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseTitle') ?? ''
  },
  {
    title: t('entity.expense.type'),
    dataIndex: 'expenseType',
    key: 'expenseType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.expense.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.expense.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.expense.applicantby'),
    dataIndex: 'applicantBy',
    key: 'applicantBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'applicantBy') ?? ''
  },
  {
    title: t('entity.expense.applicationdept'),
    dataIndex: 'applicationDept',
    key: 'applicationDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'applicationDept') ?? ''
  },
  {
    title: t('entity.expense.costbearerdept'),
    dataIndex: 'costBearerDept',
    key: 'costBearerDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'costBearerDept') ?? ''
  },
  {
    title: t('entity.expense.costcenter'),
    dataIndex: 'costCenter',
    key: 'costCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'costCenter') ?? ''
  },
  {
    title: t('entity.expense.countersignid'),
    dataIndex: 'countersignId',
    key: 'countersignId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'countersignId') ?? ''
  },
  {
    title: t('entity.expense.purchaseordercode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: t('entity.expense.purchaserequestcode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'purchaseRequestCode') ?? ''
  },
  {
    title: t('entity.expense.amount'),
    dataIndex: 'expenseAmount',
    key: 'expenseAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseAmount') ?? ''
  },
  {
    title: t('entity.expense.taxrate'),
    dataIndex: 'taxRate',
    key: 'taxRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'taxRate') ?? ''
  },
  {
    title: t('entity.expense.taxamount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'taxAmount') ?? ''
  },
  {
    title: t('entity.expense.date'),
    dataIndex: 'expenseDate',
    key: 'expenseDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseDate') ?? ''
  },
  {
    title: t('entity.expense.applicationreason'),
    dataIndex: 'applicationReason',
    key: 'applicationReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'applicationReason') ?? ''
  },
  {
    title: t('entity.expense.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'attachments') ?? ''
  },
  {
    title: t('entity.expense.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'relatedPlant') ?? ''
  },
  {
    title: t('entity.expense.status'),
    dataIndex: 'expenseStatus',
    key: 'expenseStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getExpenseField(record, 'expenseStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:expense:update',
        onClick: (record: Expense) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:expense:delete',
        onClick: (record: Expense) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getExpenseId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getExpenseField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Expense[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: Expense, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getExpenseId(selectedRow.value) === getExpenseId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Expense[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getExpenseList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Expense] 加载数据失败', { error })
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
  expenseCode: '',
  expenseTitle: '',
  expenseType: undefined as number | undefined,
  supplierCode: '',
  supplierName: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  costCenter: '',
  countersignId: '',
  purchaseOrderCode: '',
  purchaseRequestCode: '',
  expenseAmount: undefined as number | undefined,
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  expenseDateStart: '',
  expenseDateEnd: '',
  applicationReason: '',
  attachments: '',
  relatedPlant: '',
  expenseStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.expense._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Expense) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.expense._self') })
  formLoading.value = true
  try {
    const detail = await loadExpenseDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.expense._self') }))
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
      await updateExpense(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.expense._self') }))
    } else {
      await createExpense(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.expense._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  expenseDetailPanelRef.value?.reload?.()
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
  const res = await getExpenseTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importExpense(file, sheetName)
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
    const exportMeta = await exportExpense(
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
    message.success(t('common.feedback.export.success', { target: t('entity.expense._self') }))
  } catch (error: any) {
    logger.error('[Expense] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.expense._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Expense) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.expense._self'), name: t('common.tip.this.target', { target: t('entity.expense._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteExpenseById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.expense._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.expense._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.expense._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteExpenseBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.expense._self') }))
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
  expenseCode: '',
  expenseTitle: '',
  expenseType: undefined as number | undefined,
  supplierCode: '',
  supplierName: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  costCenter: '',
  countersignId: '',
  purchaseOrderCode: '',
  purchaseRequestCode: '',
  expenseAmount: undefined as number | undefined,
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  expenseDateStart: '',
  expenseDateEnd: '',
  applicationReason: '',
  attachments: '',
  relatedPlant: '',
  expenseStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
