<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客户满意度调查表主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getCustomerSatisfactionSurveyId"
      :master-row-selection="rowSelection"
      master-id-column-key="customerSatisfactionSurveyId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="logistics:quality:complaint:customer:satisfaction:survey:create"
      update-permission="logistics:quality:complaint:customer:satisfaction:survey:update"
      delete-permission="logistics:quality:complaint:customer:satisfaction:survey:delete"
      import-permission="logistics:quality:complaint:customer:satisfaction:survey:import"
      export-permission="logistics:quality:complaint:customer:satisfaction:survey:export"
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
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'surveyMethod'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'surveyMethod')"
            dict-type="logistics_quality_survey_method"
          />
        </template>
        <template v-else-if="column.key === 'surveyType'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'surveyType')"
            dict-type="logistics_quality_survey_type"
          />
        </template>
        <template v-else-if="column.key === 'surveyPeriod'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'surveyPeriod')"
            dict-type="logistics_quality_period"
          />
        </template>
        <template v-else-if="column.key === 'overallSatisfaction'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'overallSatisfaction')"
            dict-type="logistics_quality_satisfaction_level"
          />
        </template>
        <template v-else-if="column.key === 'surveyStatus'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'surveyStatus')"
            dict-type="logistics_quality_survey_status"
          />
        </template>
        <template v-else-if="column.key === 'followUpStatus'">
          <TaktDictTag
            :value="getCustomerSatisfactionSurveyDictValue(record, 'followUpStatus')"
            dict-type="logistics_quality_follow_up_status"
          />
        </template>
      </template>
      <template #detail>
        <CustomerSatisfactionSurveyItemPanel
          ref="customerSatisfactionSurveyItemPanelRef"
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
      <CustomerSatisfactionSurveyForm
        :key="formData?.customerSatisfactionSurveyId ?? 'create'"
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
      <a-form-item :label="pi.queryLabel('customerSatisfactionSurveyCode')">
        <a-input
          v-model:value="advancedQueryForm.customerSatisfactionSurveyCode"
          :placeholder="pi.queryPh('customerSatisfactionSurveyCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerId')">
      <a-form-item :label="pi.queryLabel('customerId')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerId"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName1')">
      <a-form-item :label="pi.queryLabel('customerName1')">
        <a-input
          v-model:value="advancedQueryForm.customerName1"
          :placeholder="pi.queryPh('customerName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="pi.queryLabel('customerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyDateStart')">
      <a-form-item :label="pi.queryLabel('surveyDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.surveyDateStart"
          :placeholder="pi.queryPh('surveyDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyDateEnd')">
      <a-form-item :label="pi.queryLabel('surveyDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.surveyDateEnd"
          :placeholder="pi.queryPh('surveyDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyMethod')">
      <a-form-item :label="pi.queryLabel('surveyMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.surveyMethod"
          dict-type="logistics_quality_survey_method"
          :placeholder="pi.queryPh('surveyMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyType')">
      <a-form-item :label="pi.queryLabel('surveyType')">
        <TaktSelect
          v-model:value="advancedQueryForm.surveyType"
          dict-type="logistics_quality_survey_type"
          :placeholder="pi.queryPh('surveyType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyPeriod')">
      <a-form-item :label="pi.queryLabel('surveyPeriod')">
        <TaktSelect
          v-model:value="advancedQueryForm.surveyPeriod"
          dict-type="logistics_quality_period"
          :placeholder="pi.queryPh('surveyPeriod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyorBy')">
      <a-form-item :label="pi.queryLabel('surveyorBy')">
        <TaktSelect
          v-model:value="advancedQueryForm.surveyorBy"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('surveyorBy', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerContact')">
      <a-form-item :label="pi.queryLabel('customerContact')">
        <a-input
          v-model:value="advancedQueryForm.customerContact"
          :placeholder="pi.queryPh('customerContact', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerPhone')">
      <a-form-item :label="pi.queryLabel('customerPhone')">
        <a-input
          v-model:value="advancedQueryForm.customerPhone"
          :placeholder="pi.queryPh('customerPhone', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overallSatisfaction')">
      <a-form-item :label="pi.queryLabel('overallSatisfaction')">
        <TaktSelect
          v-model:value="advancedQueryForm.overallSatisfaction"
          dict-type="logistics_quality_satisfaction_level"
          :placeholder="pi.queryPh('overallSatisfaction', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScore')">
      <a-form-item :label="pi.queryLabel('totalScore')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScore"
          :placeholder="pi.queryPh('totalScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityScore')">
      <a-form-item :label="pi.queryLabel('qualityScore')">
        <a-input-number
          v-model:value="advancedQueryForm.qualityScore"
          :placeholder="pi.queryPh('qualityScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryScore')">
      <a-form-item :label="pi.queryLabel('deliveryScore')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryScore"
          :placeholder="pi.queryPh('deliveryScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScore')">
      <a-form-item :label="pi.queryLabel('serviceScore')">
        <a-input-number
          v-model:value="advancedQueryForm.serviceScore"
          :placeholder="pi.queryPh('serviceScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceScore')">
      <a-form-item :label="pi.queryLabel('priceScore')">
        <a-input-number
          v-model:value="advancedQueryForm.priceScore"
          :placeholder="pi.queryPh('priceScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('technicalScore')">
      <a-form-item :label="pi.queryLabel('technicalScore')">
        <a-input-number
          v-model:value="advancedQueryForm.technicalScore"
          :placeholder="pi.queryPh('technicalScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerPraise')">
      <a-form-item :label="pi.queryLabel('customerPraise')">
        <a-input
          v-model:value="advancedQueryForm.customerPraise"
          :placeholder="pi.queryPh('customerPraise', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerFeedback')">
      <a-form-item :label="pi.queryLabel('customerFeedback')">
        <a-input
          v-model:value="advancedQueryForm.customerFeedback"
          :placeholder="pi.queryPh('customerFeedback', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementPlan')">
      <a-form-item :label="pi.queryLabel('improvementPlan')">
        <a-input
          v-model:value="advancedQueryForm.improvementPlan"
          :placeholder="pi.queryPh('improvementPlan', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedComplaintId')">
      <a-form-item :label="pi.queryLabel('relatedComplaintId')">
        <TaktSelect
          v-model:value="advancedQueryForm.relatedComplaintId"
          api-url="TaktCustomerComplaints/options"
          :placeholder="pi.queryPh('relatedComplaintId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="pi.queryLabel('attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="pi.queryPh('attachments', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('surveyStatus')">
      <a-form-item :label="pi.queryLabel('surveyStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.surveyStatus"
          dict-type="logistics_quality_survey_status"
          :placeholder="pi.queryPh('surveyStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="pi.queryLabel('relatedPlant')">
        <TaktSelect
          v-model:value="advancedQueryForm.relatedPlant"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('relatedPlant', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpStatus')">
      <a-form-item :label="pi.queryLabel('followUpStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.followUpStatus"
          dict-type="logistics_quality_follow_up_status"
          :placeholder="pi.queryPh('followUpStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="CUSTOMERSATISFACTIONSURVEY_SELF_I18N_KEY"
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
      table-mode="masterDetailMaster"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CustomerSatisfactionSurveyForm from './components/customer-satisfaction-survey-form.vue'
import CustomerSatisfactionSurveyItemPanel from './components/customer-satisfaction-survey-item-panel.vue'
import { provideCustomerSatisfactionSurveyMasterContext, type CustomerSatisfactionSurveyRowRecord } from './composables/use-customer-satisfaction-survey-master-context'
import { getCustomerSatisfactionSurveyList, getCustomerSatisfactionSurveyById, createCustomerSatisfactionSurvey, updateCustomerSatisfactionSurvey, deleteCustomerSatisfactionSurveyById, deleteCustomerSatisfactionSurveyBatch, getCustomerSatisfactionSurveyTemplate, importCustomerSatisfactionSurvey, exportCustomerSatisfactionSurvey, updateCustomerSatisfactionSurveyStatus } from '@/api/logistics/quality/complaint/customer-satisfaction-survey'
import type { CustomerSatisfactionSurvey, CustomerSatisfactionSurveyQuery } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useCustomerSatisfactionSurveyI18n,
  CUSTOMERSATISFACTIONSURVEY_LIST_FIELDS,
  CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS,
  CUSTOMERSATISFACTIONSURVEY_QUERY_FIELDS,
  CUSTOMERSATISFACTIONSURVEY_SELF_I18N_KEY,
} from './composables/use-customer-satisfaction-survey-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCustomerSatisfactionSurveyI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerSatisfactionSurvey')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerSatisfactionSurvey[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CustomerSatisfactionSurveyRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<CustomerSatisfactionSurveyRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerSatisfactionSurvey> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    surveyMethod: undefined as number | undefined,
    surveyType: undefined as number | undefined,
    surveyPeriod: undefined as number | undefined,
    overallSatisfaction: undefined as number | undefined,
    totalScore: undefined as number | undefined,
    qualityScore: undefined as number | undefined,
    deliveryScore: undefined as number | undefined,
    serviceScore: undefined as number | undefined,
    priceScore: undefined as number | undefined,
    technicalScore: undefined as number | undefined,
    surveyStatus: undefined as number | undefined,
    followUpStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  CUSTOMERSATISFACTIONSURVEY_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
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

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCustomerSatisfactionSurveyMasterContext()
const customerSatisfactionSurveyItemPanelRef = ref<InstanceType<typeof CustomerSatisfactionSurveyItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CustomerSatisfactionSurveyQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerSatisfactionSurveyQuery>): CustomerSatisfactionSurveyQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerSatisfactionSurveyQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerSatisfactionSurveyQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.surveyMethod !== undefined && form.surveyMethod !== null) {
    query.surveyMethod = form.surveyMethod
  }
  if (form.surveyType !== undefined && form.surveyType !== null) {
    query.surveyType = form.surveyType
  }
  if (form.surveyPeriod !== undefined && form.surveyPeriod !== null) {
    query.surveyPeriod = form.surveyPeriod
  }
  if (form.overallSatisfaction !== undefined && form.overallSatisfaction !== null) {
    query.overallSatisfaction = form.overallSatisfaction
  }
  if (form.totalScore !== undefined && form.totalScore !== null) {
    query.totalScore = form.totalScore
  }
  if (form.qualityScore !== undefined && form.qualityScore !== null) {
    query.qualityScore = form.qualityScore
  }
  if (form.deliveryScore !== undefined && form.deliveryScore !== null) {
    query.deliveryScore = form.deliveryScore
  }
  if (form.serviceScore !== undefined && form.serviceScore !== null) {
    query.serviceScore = form.serviceScore
  }
  if (form.priceScore !== undefined && form.priceScore !== null) {
    query.priceScore = form.priceScore
  }
  if (form.technicalScore !== undefined && form.technicalScore !== null) {
    query.technicalScore = form.technicalScore
  }
  if (form.surveyStatus !== undefined && form.surveyStatus !== null) {
    query.surveyStatus = form.surveyStatus
  }
  if (form.followUpStatus !== undefined && form.followUpStatus !== null) {
    query.followUpStatus = form.followUpStatus
  }
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
function syncMasterSelection(record: CustomerSatisfactionSurveyRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCustomerSatisfactionSurveyId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CustomerSatisfactionSurveyRowRecord
  const key = getCustomerSatisfactionSurveyId(row)
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
async function loadCustomerSatisfactionSurveyDetail(record: CustomerSatisfactionSurveyRowRecord): Promise<CustomerSatisfactionSurvey | null> {
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
    title: pi.label('customerSatisfactionSurveyCode'),
    dataIndex: 'customerSatisfactionSurveyCode',
    key: 'customerSatisfactionSurveyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerSatisfactionSurveyCode') ?? ''
  },
  {
    title: pi.label('customerId'),
    dataIndex: 'customerId',
    key: 'customerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerId') ?? ''
  },
  {
    title: pi.label('customerName1'),
    dataIndex: 'customerName1',
    key: 'customerName1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerName1') ?? ''
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerCode') ?? ''
  },
  {
    title: pi.label('surveyDate'),
    dataIndex: 'surveyDate',
    key: 'surveyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyDate') ?? ''
  },
  {
    title: pi.label('surveyMethod'),
    dataIndex: 'surveyMethod',
    key: 'surveyMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('surveyType'),
    dataIndex: 'surveyType',
    key: 'surveyType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('surveyPeriod'),
    dataIndex: 'surveyPeriod',
    key: 'surveyPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('surveyorBy'),
    dataIndex: 'surveyorBy',
    key: 'surveyorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'surveyorBy') ?? ''
  },
  {
    title: pi.label('customerContact'),
    dataIndex: 'customerContact',
    key: 'customerContact',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerContact') ?? ''
  },
  {
    title: pi.label('customerPhone'),
    dataIndex: 'customerPhone',
    key: 'customerPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerPhone') ?? ''
  },
  {
    title: pi.label('overallSatisfaction'),
    dataIndex: 'overallSatisfaction',
    key: 'overallSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('totalScore'),
    dataIndex: 'totalScore',
    key: 'totalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'totalScore') ?? ''
  },
  {
    title: pi.label('qualityScore'),
    dataIndex: 'qualityScore',
    key: 'qualityScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'qualityScore') ?? ''
  },
  {
    title: pi.label('deliveryScore'),
    dataIndex: 'deliveryScore',
    key: 'deliveryScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'deliveryScore') ?? ''
  },
  {
    title: pi.label('serviceScore'),
    dataIndex: 'serviceScore',
    key: 'serviceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'serviceScore') ?? ''
  },
  {
    title: pi.label('priceScore'),
    dataIndex: 'priceScore',
    key: 'priceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'priceScore') ?? ''
  },
  {
    title: pi.label('technicalScore'),
    dataIndex: 'technicalScore',
    key: 'technicalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'technicalScore') ?? ''
  },
  {
    title: pi.label('customerPraise'),
    dataIndex: 'customerPraise',
    key: 'customerPraise',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerPraise') ?? ''
  },
  {
    title: pi.label('customerFeedback'),
    dataIndex: 'customerFeedback',
    key: 'customerFeedback',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'customerFeedback') ?? ''
  },
  {
    title: pi.label('improvementPlan'),
    dataIndex: 'improvementPlan',
    key: 'improvementPlan',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'improvementPlan') ?? ''
  },
  {
    title: pi.label('relatedComplaintId'),
    dataIndex: 'relatedComplaintId',
    key: 'relatedComplaintId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'relatedComplaintId') ?? ''
  },
  {
    title: pi.label('attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'attachments') ?? ''
  },
  {
    title: pi.label('surveyStatus'),
    dataIndex: 'surveyStatus',
    key: 'surveyStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('relatedPlant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerSatisfactionSurveyField(record, 'relatedPlant') ?? ''
  },
  {
    title: pi.label('followUpStatus'),
    dataIndex: 'followUpStatus',
    key: 'followUpStatus',
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
        permission: 'logistics:quality:complaint:customer:satisfaction:survey:update',
        onClick: (record: CustomerSatisfactionSurveyRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:satisfaction:survey:delete',
        onClick: (record: CustomerSatisfactionSurveyRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerSatisfactionSurveyId = (record: CustomerSatisfactionSurveyRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerSatisfactionSurveyField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerSatisfactionSurveyDictValue = (
  record: CustomerSatisfactionSurveyRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerSatisfactionSurveyRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CustomerSatisfactionSurveyRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCustomerSatisfactionSurveyId(selectedRow.value) === getCustomerSatisfactionSurveyId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CustomerSatisfactionSurveyRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getCustomerSatisfactionSurveyList(buildListQuery())
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
  customerSatisfactionSurveyCode: '',
  customerId: '',
  customerName1: '',
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
  relatedComplaintId: '',
  attachments: '',
  surveyStatus: undefined as number | undefined,
  relatedPlant: '',
  followUpStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CustomerSatisfactionSurveyRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCustomerSatisfactionSurvey(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  customerSatisfactionSurveyItemPanelRef.value?.reload?.()
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
  const res = await getCustomerSatisfactionSurveyTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCustomerSatisfactionSurvey(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    customerSatisfactionSurveyItemPanelRef.value?.reload?.()
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
    const exportMeta = await exportCustomerSatisfactionSurvey(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[CustomerSatisfactionSurvey] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerSatisfactionSurveyRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerSatisfactionSurveyById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerSatisfactionSurveyBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
  customerSatisfactionSurveyCode: '',
  customerId: '',
  customerName1: '',
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
  relatedComplaintId: '',
  attachments: '',
  surveyStatus: undefined as number | undefined,
  relatedPlant: '',
  followUpStatus: undefined as number | undefined,
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
