<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/countersign -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会签单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:financial:countersign:create"
      update-permission="accounting:financial:countersign:update"
      delete-permission="accounting:financial:countersign:delete"
      import-permission="accounting:financial:countersign:import"
      export-permission="accounting:financial:countersign:export"
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
      :master-row-key="getCountersignId"
      :master-row-selection="rowSelection"
      master-id-column-key="countersignId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'businessType'">
          <TaktDictTag
            :value="getCountersignField(record, 'businessType')"
            dict-type="logistics_countersign_business_type"
          />
        </template>
        <template v-else-if="column.key === 'isBudget'">
          <TaktDictTag
            :value="getCountersignField(record, 'isBudget')"
            dict-type="sys_yes_no_type"
          />
        </template>
        <template v-else-if="column.key === 'countersignStatus'">
          <TaktDictTag
            :value="getCountersignField(record, 'countersignStatus')"
            dict-type="sys_approval_status"
          />
        </template>
      </template>
      <template #detail>
        <CountersignDetailPanel
          ref="countersignDetailPanelRef"
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
      <CountersignForm
        :key="formData?.countersignId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-countersign'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('countersignCode')">
      <a-form-item :label="t('entity.countersign.code')">
        <a-input
          v-model:value="advancedQueryForm.countersignCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInquiryId')">
      <a-form-item :label="t('entity.countersign.purchaseinquiryid')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInquiryId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.purchaseinquiryid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInquiryCode')">
      <a-form-item :label="t('entity.countersign.purchaseinquirycode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInquiryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.purchaseinquirycode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessType')">
      <a-form-item :label="t('entity.countersign.businesstype')">
        <TaktSelect
          v-model:value="advancedQueryForm.businessType"
          dict-type="logistics_countersign_business_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.businesstype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessKey')">
      <a-form-item :label="t('entity.countersign.businesskey')">
        <a-input
          v-model:value="advancedQueryForm.businessKey"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.businesskey') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stepNo')">
      <a-form-item :label="t('entity.countersign.stepno')">
        <a-input-number
          v-model:value="advancedQueryForm.stepNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.stepno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignDepts')">
      <a-form-item :label="t('entity.countersign.depts')">
        <a-input
          v-model:value="advancedQueryForm.countersignDepts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.depts') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('financeDept')">
      <a-form-item :label="t('entity.countersign.financedept')">
        <a-input
          v-model:value="advancedQueryForm.financeDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.financedept') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetReviewComment')">
      <a-form-item :label="t('entity.countersign.budgetreviewcomment')">
        <a-input
          v-model:value="advancedQueryForm.budgetReviewComment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetreviewcomment') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executiveOffice')">
      <a-form-item :label="t('entity.countersign.executiveoffice')">
        <a-input
          v-model:value="advancedQueryForm.executiveOffice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.executiveoffice') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantBy')">
      <a-form-item :label="t('entity.countersign.applicantby')">
        <a-input
          v-model:value="advancedQueryForm.applicantBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicantby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationDept')">
      <a-form-item :label="t('entity.countersign.applicationdept')">
        <a-input
          v-model:value="advancedQueryForm.applicationDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationdept') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costBearerDept')">
      <a-form-item :label="t('entity.countersign.costbearerdept')">
        <a-input
          v-model:value="advancedQueryForm.costBearerDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.costbearerdept') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBudget')">
      <a-form-item :label="t('entity.countersign.isbudget')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBudget"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.isbudget') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetItem')">
      <a-form-item :label="t('entity.countersign.budgetitem')">
        <a-input
          v-model:value="advancedQueryForm.budgetItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetitem') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetAmount')">
      <a-form-item :label="t('entity.countersign.budgetamount')">
        <a-input-number
          v-model:value="advancedQueryForm.budgetAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationAmount')">
      <a-form-item :label="t('entity.countersign.applicationamount')">
        <a-input-number
          v-model:value="advancedQueryForm.applicationAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignTitle')">
      <a-form-item :label="t('entity.countersign.title')">
        <a-input
          v-model:value="advancedQueryForm.countersignTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.title') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationReason')">
      <a-form-item :label="t('entity.countersign.applicationreason')">
        <a-input
          v-model:value="advancedQueryForm.applicationReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationreason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetUsageDescription')">
      <a-form-item :label="t('entity.countersign.budgetusagedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.budgetUsageDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.countersign.budgetusagedescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetAndExpectedBenefit')">
      <a-form-item :label="t('entity.countersign.targetandexpectedbenefit')">
        <a-input
          v-model:value="advancedQueryForm.targetAndExpectedBenefit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.targetandexpectedbenefit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.countersign.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.attachments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignStatus')">
      <a-form-item :label="t('entity.countersign.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.countersignStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.countersign.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.countersign.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.countersign.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.countersign.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.countersign.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.countersign.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.countersign.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.countersign.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.countersign.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.flowinstanceid') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.countersign._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.countersign._self"
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
      :id-column-key="'countersignId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会签单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/countersign
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CountersignForm from './components/countersign-form.vue'
import CountersignDetailPanel from './components/countersign-detail-panel.vue'
import { provideCountersignMasterContext } from './composables/use-countersign-master-context'
import { getCountersignList, getCountersignById, createCountersign, updateCountersign, deleteCountersignById, deleteCountersignBatch, getCountersignTemplate, importCountersign, exportCountersign, updateCountersignStatus } from '@/api/accounting/financial/countersign'
import type { Countersign, CountersignQuery } from '@/types/accounting/financial/countersign'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCountersign')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.countersign._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Countersign[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Countersign | null>(null)
/** 表格多选行 */
const selectedRows = ref<Countersign[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Countersign> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  countersignCode: '',
  purchaseInquiryId: '',
  purchaseInquiryCode: '',
  businessType: '',
  businessKey: '',
  stepNo: undefined as number | undefined,
  countersignDepts: '',
  financeDept: '',
  budgetReviewComment: '',
  executiveOffice: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  isBudget: undefined as number | undefined,
  budgetItem: '',
  budgetAmount: undefined as number | undefined,
  applicationAmount: undefined as number | undefined,
  countersignTitle: '',
  applicationReason: '',
  budgetUsageDescription: '',
  targetAndExpectedBenefit: '',
  attachments: '',
  countersignStatus: undefined as number | undefined,
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
  { key: 'countersignCode', label: t('entity.countersign.code') },
  { key: 'purchaseInquiryId', label: t('entity.countersign.purchaseinquiryid') },
  { key: 'purchaseInquiryCode', label: t('entity.countersign.purchaseinquirycode') },
  { key: 'businessType', label: t('entity.countersign.businesstype') },
  { key: 'businessKey', label: t('entity.countersign.businesskey') },
  { key: 'stepNo', label: t('entity.countersign.stepno') },
  { key: 'countersignDepts', label: t('entity.countersign.depts') },
  { key: 'financeDept', label: t('entity.countersign.financedept') },
  { key: 'budgetReviewComment', label: t('entity.countersign.budgetreviewcomment') },
  { key: 'executiveOffice', label: t('entity.countersign.executiveoffice') },
  { key: 'applicantBy', label: t('entity.countersign.applicantby') },
  { key: 'applicationDept', label: t('entity.countersign.applicationdept') },
  { key: 'costBearerDept', label: t('entity.countersign.costbearerdept') },
  { key: 'isBudget', label: t('entity.countersign.isbudget') },
  { key: 'budgetItem', label: t('entity.countersign.budgetitem') },
  { key: 'budgetAmount', label: t('entity.countersign.budgetamount') },
  { key: 'applicationAmount', label: t('entity.countersign.applicationamount') },
  { key: 'countersignTitle', label: t('entity.countersign.title') },
  { key: 'applicationReason', label: t('entity.countersign.applicationreason') },
  { key: 'budgetUsageDescription', label: t('entity.countersign.budgetusagedescription') },
  { key: 'targetAndExpectedBenefit', label: t('entity.countersign.targetandexpectedbenefit') },
  { key: 'attachments', label: t('entity.countersign.attachments') },
  { key: 'countersignStatus', label: t('entity.countersign.status') },
  { key: 'approvalStatus', label: t('entity.countersign.approvalstatus') },
  { key: 'initiatorId', label: t('entity.countersign.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.countersign.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.countersign.initiatedatend') },
  { key: 'approvedBy', label: t('entity.countersign.approvedby') },
  { key: 'approvedAtStart', label: t('entity.countersign.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.countersign.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.countersign.flowinstanceid') },
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
const entityIdName = 'countersignId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCountersignMasterContext()
const countersignDetailPanelRef = ref<InstanceType<typeof CountersignDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CountersignQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CountersignQuery>): CountersignQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CountersignQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CountersignQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('countersignCode', form.countersignCode)
  assignTrimmed('purchaseInquiryId', form.purchaseInquiryId)
  assignTrimmed('purchaseInquiryCode', form.purchaseInquiryCode)
  assignTrimmed('businessType', form.businessType)
  assignTrimmed('businessKey', form.businessKey)
  if (form.stepNo !== undefined && form.stepNo !== null) {
    query.stepNo = form.stepNo
  }
  assignTrimmed('countersignDepts', form.countersignDepts)
  assignTrimmed('financeDept', form.financeDept)
  assignTrimmed('budgetReviewComment', form.budgetReviewComment)
  assignTrimmed('executiveOffice', form.executiveOffice)
  assignTrimmed('applicantBy', form.applicantBy)
  assignTrimmed('applicationDept', form.applicationDept)
  assignTrimmed('costBearerDept', form.costBearerDept)
  if (form.isBudget !== undefined && form.isBudget !== null) {
    query.isBudget = form.isBudget
  }
  assignTrimmed('budgetItem', form.budgetItem)
  if (form.budgetAmount !== undefined && form.budgetAmount !== null) {
    query.budgetAmount = form.budgetAmount
  }
  if (form.applicationAmount !== undefined && form.applicationAmount !== null) {
    query.applicationAmount = form.applicationAmount
  }
  assignTrimmed('countersignTitle', form.countersignTitle)
  assignTrimmed('applicationReason', form.applicationReason)
  assignTrimmed('budgetUsageDescription', form.budgetUsageDescription)
  assignTrimmed('targetAndExpectedBenefit', form.targetAndExpectedBenefit)
  assignTrimmed('attachments', form.attachments)
  if (form.countersignStatus !== undefined && form.countersignStatus !== null) {
    query.countersignStatus = form.countersignStatus
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
function syncMasterSelection(record: Countersign | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCountersignId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as Countersign
  const key = getCountersignId(row)
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
async function loadCountersignDetail(record: Countersign): Promise<Countersign | null> {
  const id = getCountersignId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCountersignById(id)
    const index = dataSource.value.findIndex((row) => getCountersignId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Countersign
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
    dataIndex: 'countersignId',
    key: 'countersignId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignId') ?? ''
  },
  {
    title: t('entity.countersign.code'),
    dataIndex: 'countersignCode',
    key: 'countersignCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignCode') ?? ''
  },
  {
    title: t('entity.countersign.purchaseinquiryid'),
    dataIndex: 'purchaseInquiryId',
    key: 'purchaseInquiryId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'purchaseInquiryId') ?? ''
  },
  {
    title: t('entity.countersign.purchaseinquirycode'),
    dataIndex: 'purchaseInquiryCode',
    key: 'purchaseInquiryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'purchaseInquiryCode') ?? ''
  },
  {
    title: t('entity.countersign.businesstype'),
    dataIndex: 'businessType',
    key: 'businessType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.countersign.businesskey'),
    dataIndex: 'businessKey',
    key: 'businessKey',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'businessKey') ?? ''
  },
  {
    title: t('entity.countersign.stepno'),
    dataIndex: 'stepNo',
    key: 'stepNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'stepNo') ?? ''
  },
  {
    title: t('entity.countersign.depts'),
    dataIndex: 'countersignDepts',
    key: 'countersignDepts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignDepts') ?? ''
  },
  {
    title: t('entity.countersign.financedept'),
    dataIndex: 'financeDept',
    key: 'financeDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'financeDept') ?? ''
  },
  {
    title: t('entity.countersign.budgetreviewcomment'),
    dataIndex: 'budgetReviewComment',
    key: 'budgetReviewComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetReviewComment') ?? ''
  },
  {
    title: t('entity.countersign.executiveoffice'),
    dataIndex: 'executiveOffice',
    key: 'executiveOffice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'executiveOffice') ?? ''
  },
  {
    title: t('entity.countersign.applicantby'),
    dataIndex: 'applicantBy',
    key: 'applicantBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicantBy') ?? ''
  },
  {
    title: t('entity.countersign.applicationdept'),
    dataIndex: 'applicationDept',
    key: 'applicationDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationDept') ?? ''
  },
  {
    title: t('entity.countersign.costbearerdept'),
    dataIndex: 'costBearerDept',
    key: 'costBearerDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'costBearerDept') ?? ''
  },
  {
    title: t('entity.countersign.isbudget'),
    dataIndex: 'isBudget',
    key: 'isBudget',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.countersign.budgetitem'),
    dataIndex: 'budgetItem',
    key: 'budgetItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetItem') ?? ''
  },
  {
    title: t('entity.countersign.budgetamount'),
    dataIndex: 'budgetAmount',
    key: 'budgetAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetAmount') ?? ''
  },
  {
    title: t('entity.countersign.applicationamount'),
    dataIndex: 'applicationAmount',
    key: 'applicationAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationAmount') ?? ''
  },
  {
    title: t('entity.countersign.title'),
    dataIndex: 'countersignTitle',
    key: 'countersignTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignTitle') ?? ''
  },
  {
    title: t('entity.countersign.applicationreason'),
    dataIndex: 'applicationReason',
    key: 'applicationReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationReason') ?? ''
  },
  {
    title: t('entity.countersign.budgetusagedescription'),
    dataIndex: 'budgetUsageDescription',
    key: 'budgetUsageDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetUsageDescription') ?? ''
  },
  {
    title: t('entity.countersign.targetandexpectedbenefit'),
    dataIndex: 'targetAndExpectedBenefit',
    key: 'targetAndExpectedBenefit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'targetAndExpectedBenefit') ?? ''
  },
  {
    title: t('entity.countersign.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'attachments') ?? ''
  },
  {
    title: t('entity.countersign.status'),
    dataIndex: 'countersignStatus',
    key: 'countersignStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:countersign:update',
        onClick: (record: Countersign) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:countersign:delete',
        onClick: (record: Countersign) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCountersignId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCountersignField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Countersign[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: Countersign, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCountersignId(selectedRow.value) === getCountersignId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Countersign[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getCountersignList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Countersign] 加载数据失败', { error })
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
  countersignCode: '',
  purchaseInquiryId: '',
  purchaseInquiryCode: '',
  businessType: '',
  businessKey: '',
  stepNo: undefined as number | undefined,
  countersignDepts: '',
  financeDept: '',
  budgetReviewComment: '',
  executiveOffice: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  isBudget: undefined as number | undefined,
  budgetItem: '',
  budgetAmount: undefined as number | undefined,
  applicationAmount: undefined as number | undefined,
  countersignTitle: '',
  applicationReason: '',
  budgetUsageDescription: '',
  targetAndExpectedBenefit: '',
  attachments: '',
  countersignStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.countersign._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Countersign) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.countersign._self') })
  formLoading.value = true
  try {
    const detail = await loadCountersignDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.countersign._self') }))
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
      await updateCountersign(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.countersign._self') }))
    } else {
      await createCountersign(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.countersign._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  countersignDetailPanelRef.value?.reload?.()
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
  const res = await getCountersignTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCountersign(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    countersignDetailPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportCountersign(
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
    message.success(t('common.feedback.export.success', { target: t('entity.countersign._self') }))
  } catch (error: any) {
    logger.error('[Countersign] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.countersign._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Countersign) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.countersign._self'), name: t('common.tip.this.target', { target: t('entity.countersign._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCountersignById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.countersign._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.countersign._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.countersign._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCountersignBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.countersign._self') }))
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
  countersignCode: '',
  purchaseInquiryId: '',
  purchaseInquiryCode: '',
  businessType: '',
  businessKey: '',
  stepNo: undefined as number | undefined,
  countersignDepts: '',
  financeDept: '',
  budgetReviewComment: '',
  executiveOffice: '',
  applicantBy: '',
  applicationDept: '',
  costBearerDept: '',
  isBudget: undefined as number | undefined,
  budgetItem: '',
  budgetAmount: undefined as number | undefined,
  applicationAmount: undefined as number | undefined,
  countersignTitle: '',
  applicationReason: '',
  budgetUsageDescription: '',
  targetAndExpectedBenefit: '',
  attachments: '',
  countersignStatus: undefined as number | undefined,
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
