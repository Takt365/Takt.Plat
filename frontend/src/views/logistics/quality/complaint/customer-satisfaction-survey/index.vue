<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客户满意度调查表主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-complaint-customer-satisfaction-survey">
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
      create-permission="logistics:quality:complaint:customersatisfactionsurvey:create"
      update-permission="logistics:quality:complaint:customersatisfactionsurvey:update"
      delete-permission="logistics:quality:complaint:customersatisfactionsurvey:delete"
      import-permission="logistics:quality:complaint:customersatisfactionsurvey:import"
      export-permission="logistics:quality:complaint:customersatisfactionsurvey:export"
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
      :id-column-key="'customerSatisfactionSurveyId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCustomerSatisfactionSurveyId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.customerSatisfactionSurveyItem._self') }}</div>
          <a-table
            v-if="hasCustomerSatisfactionSurveyItemRows(record)"
            :columns="customerSatisfactionSurveyItemExpandColumns"
            :data-source="getCustomerSatisfactionSurveyItemRows(record)"
            :row-key="(row: CustomerSatisfactionSurveyItem, index?: number) => row?.customerSatisfactionSurveyItemId || String(index ?? 0)"
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
      <CustomerSatisfactionSurveyForm
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-customer-satisfaction-survey'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('customerSatisfactionSurveyCode')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.code')">
        <a-input
          v-model:value="advancedQueryForm.customerSatisfactionSurveyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerId')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customerid')">
        <a-input
          v-model:value="advancedQueryForm.customerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyDateStart')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.surveyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyDateEnd')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.surveyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyMethod')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveymethod')">
        <a-input-number
          v-model:value="advancedQueryForm.surveyMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveymethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyType')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveytype')">
        <a-input-number
          v-model:value="advancedQueryForm.surveyType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyPeriod')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveyperiod')">
        <a-input-number
          v-model:value="advancedQueryForm.surveyPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveyperiod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyorBy')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveyorby')">
        <a-input
          v-model:value="advancedQueryForm.surveyorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveyorby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerContact')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customercontact')">
        <a-input
          v-model:value="advancedQueryForm.customerContact"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customercontact') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerPhone')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customerphone')">
        <a-input
          v-model:value="advancedQueryForm.customerPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerphone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overallSatisfaction')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.overallsatisfaction')">
        <a-input-number
          v-model:value="advancedQueryForm.overallSatisfaction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.overallsatisfaction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.totalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.totalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.qualityscore')">
        <a-input-number
          v-model:value="advancedQueryForm.qualityScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.qualityscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.deliveryscore')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.deliveryscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.servicescore')">
        <a-input-number
          v-model:value="advancedQueryForm.serviceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.servicescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.pricescore')">
        <a-input-number
          v-model:value="advancedQueryForm.priceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.pricescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('technicalScore')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.technicalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.technicalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.technicalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerPraise')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customerpraise')">
        <a-input
          v-model:value="advancedQueryForm.customerPraise"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerpraise') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerFeedback')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.customerfeedback')">
        <a-input
          v-model:value="advancedQueryForm.customerFeedback"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerfeedback') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementPlan')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.improvementplan')">
        <a-input
          v-model:value="advancedQueryForm.improvementPlan"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.improvementplan') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyStatus')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.surveystatus')">
        <a-input-number
          v-model:value="advancedQueryForm.surveyStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveystatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpStatus')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.followupstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.followUpStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.followupstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedComplaintId')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.relatedcomplaintid')">
        <a-input
          v-model:value="advancedQueryForm.relatedComplaintId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.relatedcomplaintid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.customerSatisfactionSurvey.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customerSatisfactionSurvey._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customerSatisfactionSurvey._self"
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
      :id-column-key="'customerSatisfactionSurveyId'"
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
 * 客户满意度调查表主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import CustomerSatisfactionSurveyForm from './components/customer-satisfaction-survey-form.vue'
import { getCustomerSatisfactionSurveyList, getCustomerSatisfactionSurveyById, createCustomerSatisfactionSurvey, updateCustomerSatisfactionSurvey, deleteCustomerSatisfactionSurveyById, deleteCustomerSatisfactionSurveyBatch, getCustomerSatisfactionSurveyTemplate, importCustomerSatisfactionSurvey, exportCustomerSatisfactionSurvey } from '@/api/logistics/quality/complaint/customer-satisfaction-survey'
import * as customerSatisfactionSurveyItemApi from '@/api/logistics/quality/complaint/customer-satisfaction-survey-item'
import type { CustomerSatisfactionSurveyItem, CustomerSatisfactionSurveyItemQuery } from '@/types/logistics/quality/complaint/customer-satisfaction-survey-item'
import type { CustomerSatisfactionSurvey, CustomerSatisfactionSurveyQuery, CustomerSatisfactionSurveyCreate, CustomerSatisfactionSurveyUpdate } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerSatisfactionSurvey')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customerSatisfactionSurvey._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerSatisfactionSurvey[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CustomerSatisfactionSurvey | null>(null)
/** 表格多选行 */
const selectedRows = ref<CustomerSatisfactionSurvey[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerSatisfactionSurvey>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  customerSatisfactionSurveyCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  surveyDateStart: '',
  surveyDateEnd: '',
  surveyMethod: undefined as number | undefined,
  surveyType: undefined as number | undefined,
  surveyPeriod: undefined as number | undefined,
  surveyorBy: '',
  customerContact: '',
  customerPhone: '',
  overallSatisfaction: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  customerPraise: '',
  customerFeedback: '',
  improvementPlan: '',
  surveyStatus: undefined as number | undefined,
  followUpStatus: undefined as number | undefined,
  relatedComplaintId: '',
  relatedPlant: '',
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'customerSatisfactionSurveyCode', label: t('entity.customerSatisfactionSurvey.code') },
  { key: 'customerId', label: t('entity.customerSatisfactionSurvey.customerid') },
  { key: 'customerName', label: t('entity.customerSatisfactionSurvey.customername') },
  { key: 'customerCode', label: t('entity.customerSatisfactionSurvey.customercode') },
  { key: 'surveyDateStart', label: t('entity.customerSatisfactionSurvey.surveydatestart') },
  { key: 'surveyDateEnd', label: t('entity.customerSatisfactionSurvey.surveydateend') },
  { key: 'surveyMethod', label: t('entity.customerSatisfactionSurvey.surveymethod') },
  { key: 'surveyType', label: t('entity.customerSatisfactionSurvey.surveytype') },
  { key: 'surveyPeriod', label: t('entity.customerSatisfactionSurvey.surveyperiod') },
  { key: 'surveyorBy', label: t('entity.customerSatisfactionSurvey.surveyorby') },
  { key: 'customerContact', label: t('entity.customerSatisfactionSurvey.customercontact') },
  { key: 'customerPhone', label: t('entity.customerSatisfactionSurvey.customerphone') },
  { key: 'overallSatisfaction', label: t('entity.customerSatisfactionSurvey.overallsatisfaction') },
  { key: 'totalScore', label: t('entity.customerSatisfactionSurvey.totalscore') },
  { key: 'qualityScore', label: t('entity.customerSatisfactionSurvey.qualityscore') },
  { key: 'deliveryScore', label: t('entity.customerSatisfactionSurvey.deliveryscore') },
  { key: 'serviceScore', label: t('entity.customerSatisfactionSurvey.servicescore') },
  { key: 'priceScore', label: t('entity.customerSatisfactionSurvey.pricescore') },
  { key: 'technicalScore', label: t('entity.customerSatisfactionSurvey.technicalscore') },
  { key: 'customerPraise', label: t('entity.customerSatisfactionSurvey.customerpraise') },
  { key: 'customerFeedback', label: t('entity.customerSatisfactionSurvey.customerfeedback') },
  { key: 'improvementPlan', label: t('entity.customerSatisfactionSurvey.improvementplan') },
  { key: 'surveyStatus', label: t('entity.customerSatisfactionSurvey.surveystatus') },
  { key: 'followUpStatus', label: t('entity.customerSatisfactionSurvey.followupstatus') },
  { key: 'relatedComplaintId', label: t('entity.customerSatisfactionSurvey.relatedcomplaintid') },
  { key: 'relatedPlant', label: t('entity.customerSatisfactionSurvey.relatedplant') },
  { key: 'sortOrder', label: t('entity.customerSatisfactionSurvey.sortorder') },
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
const entityIdName = 'customerSatisfactionSurveyId'
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

/** 展开行预览：customerSatisfactionSurveyItem 列 */
const customerSatisfactionSurveyItemExpandColumns = computed(() => [
  {
    title: t('entity.customerSatisfactionSurveyItem.surveyid'),
    dataIndex: 'surveyId',
    key: 'surveyId',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.surveyname'),
    dataIndex: 'surveyName',
    key: 'surveyName',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode'),
    dataIndex: 'customerSatisfactionSurveyCode',
    key: 'customerSatisfactionSurveyCode',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    ellipsis: true,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    ellipsis: true,
  },
])

/** 读取主表行上的 customerSatisfactionSurveyItem 子表缓存 */
function getCustomerSatisfactionSurveyItemRows(record: CustomerSatisfactionSurvey): CustomerSatisfactionSurveyItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 customerSatisfactionSurveyItem 子表 */
function hasCustomerSatisfactionSurveyItemRows(record: CustomerSatisfactionSurvey): boolean {
  return getCustomerSatisfactionSurveyItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadCustomerSatisfactionSurveyDetail(record: CustomerSatisfactionSurvey): Promise<CustomerSatisfactionSurvey | null> {
  const id = getCustomerSatisfactionSurveyId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCustomerSatisfactionSurveyById(id)
    const index = dataSource.value.findIndex((row) => getCustomerSatisfactionSurveyId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as CustomerSatisfactionSurvey
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 customerSatisfactionSurveyItem 子表（CustomerSatisfactionSurveyItemQuery + customerSatisfactionSurveyItemApi，与主表 CustomerSatisfactionSurveyQuery 分离） */
async function loadCustomerSatisfactionSurveyItemForCustomerSatisfactionSurvey(record: CustomerSatisfactionSurvey): Promise<CustomerSatisfactionSurveyItem[]> {
  const masterId = getCustomerSatisfactionSurveyId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: CustomerSatisfactionSurveyItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      customerSatisfactionSurveyCode: masterId,
    }
    const result = await customerSatisfactionSurveyItemApi.getCustomerSatisfactionSurveyItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getCustomerSatisfactionSurveyId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as CustomerSatisfactionSurvey
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureCustomerSatisfactionSurveyChildrenLoaded(record: CustomerSatisfactionSurvey) {
  if (!hasCustomerSatisfactionSurveyItemRows(record)) {
    await loadCustomerSatisfactionSurveyItemForCustomerSatisfactionSurvey(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: CustomerSatisfactionSurvey) {
  const key = getCustomerSatisfactionSurveyId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureCustomerSatisfactionSurveyChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerSatisfactionSurveyId',
    key: 'customerSatisfactionSurveyId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerSatisfactionSurveyId') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.code'),
    dataIndex: 'customerSatisfactionSurveyCode',
    key: 'customerSatisfactionSurveyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerSatisfactionSurveyCode') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customerid'),
    dataIndex: 'customerId',
    key: 'customerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerId') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveydate'),
    dataIndex: 'surveyDate',
    key: 'surveyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyDate') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveymethod'),
    dataIndex: 'surveyMethod',
    key: 'surveyMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyMethod') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveytype'),
    dataIndex: 'surveyType',
    key: 'surveyType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyType') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveyperiod'),
    dataIndex: 'surveyPeriod',
    key: 'surveyPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyPeriod') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveyorby'),
    dataIndex: 'surveyorBy',
    key: 'surveyorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyorBy') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customercontact'),
    dataIndex: 'customerContact',
    key: 'customerContact',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerContact') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customerphone'),
    dataIndex: 'customerPhone',
    key: 'customerPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerPhone') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.overallsatisfaction'),
    dataIndex: 'overallSatisfaction',
    key: 'overallSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'overallSatisfaction') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.totalscore'),
    dataIndex: 'totalScore',
    key: 'totalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'totalScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.qualityscore'),
    dataIndex: 'qualityScore',
    key: 'qualityScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'qualityScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.deliveryscore'),
    dataIndex: 'deliveryScore',
    key: 'deliveryScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'deliveryScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.servicescore'),
    dataIndex: 'serviceScore',
    key: 'serviceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'serviceScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.pricescore'),
    dataIndex: 'priceScore',
    key: 'priceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'priceScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.technicalscore'),
    dataIndex: 'technicalScore',
    key: 'technicalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'technicalScore') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customerpraise'),
    dataIndex: 'customerPraise',
    key: 'customerPraise',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerPraise') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.customerfeedback'),
    dataIndex: 'customerFeedback',
    key: 'customerFeedback',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerFeedback') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.improvementplan'),
    dataIndex: 'improvementPlan',
    key: 'improvementPlan',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'improvementPlan') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.surveystatus'),
    dataIndex: 'surveyStatus',
    key: 'surveyStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyStatus') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.followupstatus'),
    dataIndex: 'followUpStatus',
    key: 'followUpStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'followUpStatus') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.relatedcomplaintid'),
    dataIndex: 'relatedComplaintId',
    key: 'relatedComplaintId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'relatedComplaintId') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.relatedcomplaintname'),
    dataIndex: 'relatedComplaintName',
    key: 'relatedComplaintName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'relatedComplaintName') ?? ''
  },
  {
    title: t('entity.customerSatisfactionSurvey.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customersatisfactionsurvey:update',
        onClick: (record: CustomerSatisfactionSurvey) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customersatisfactionsurvey:delete',
        onClick: (record: CustomerSatisfactionSurvey) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerSatisfactionSurveyId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerSatisfactionSurveyField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerSatisfactionSurvey[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerSatisfactionSurvey, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCustomerSatisfactionSurveyId(selectedRow.value) === getCustomerSatisfactionSurveyId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerSatisfactionSurvey[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CustomerSatisfactionSurvey) => ({
  onClick: () => {
    const key = getCustomerSatisfactionSurveyId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCustomerSatisfactionSurveyId(item)))
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
    const params: CustomerSatisfactionSurveyQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCustomerSatisfactionSurveyList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CustomerSatisfactionSurvey] 加载数据失败', { error })
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
  customerSatisfactionSurveyCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  surveyDateStart: '',
  surveyDateEnd: '',
  surveyMethod: undefined as number | undefined,
  surveyType: undefined as number | undefined,
  surveyPeriod: undefined as number | undefined,
  surveyorBy: '',
  customerContact: '',
  customerPhone: '',
  overallSatisfaction: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  customerPraise: '',
  customerFeedback: '',
  improvementPlan: '',
  surveyStatus: undefined as number | undefined,
  followUpStatus: undefined as number | undefined,
  relatedComplaintId: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customerSatisfactionSurvey._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CustomerSatisfactionSurvey) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customerSatisfactionSurvey._self') })
  formLoading.value = true
  try {
    const detail = await loadCustomerSatisfactionSurveyDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.customerSatisfactionSurvey._self') }))
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
      await updateCustomerSatisfactionSurvey(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.customerSatisfactionSurvey._self') }))
    } else {
      await createCustomerSatisfactionSurvey(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.customerSatisfactionSurvey._self') }))
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
  const res = await getCustomerSatisfactionSurveyTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerSatisfactionSurvey(file, sheetName)
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
    const exportQuery: CustomerSatisfactionSurveyQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCustomerSatisfactionSurvey(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.customerSatisfactionSurvey._self') }))
  } catch (error: any) {
    logger.error('[CustomerSatisfactionSurvey] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.customerSatisfactionSurvey._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerSatisfactionSurvey) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.customerSatisfactionSurvey._self'), name: t('common.tip.this.target', { target: t('entity.customerSatisfactionSurvey._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerSatisfactionSurveyById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.customerSatisfactionSurvey._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.customerSatisfactionSurvey._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.customerSatisfactionSurvey._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerSatisfactionSurveyBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customerSatisfactionSurvey._self') }))
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
  customerSatisfactionSurveyCode: '',
  customerId: '',
  customerName: '',
  customerCode: '',
  surveyDateStart: '',
  surveyDateEnd: '',
  surveyMethod: undefined as number | undefined,
  surveyType: undefined as number | undefined,
  surveyPeriod: undefined as number | undefined,
  surveyorBy: '',
  customerContact: '',
  customerPhone: '',
  overallSatisfaction: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  customerPraise: '',
  customerFeedback: '',
  improvementPlan: '',
  surveyStatus: undefined as number | undefined,
  followUpStatus: undefined as number | undefined,
  relatedComplaintId: '',
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
.logistics-quality-complaint-customer-satisfaction-survey {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
