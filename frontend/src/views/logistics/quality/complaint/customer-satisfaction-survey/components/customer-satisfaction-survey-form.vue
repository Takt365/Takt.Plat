<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/components -->
<!-- 文件名称：customer-satisfaction-survey-form.vue -->
<!-- 功能描述：客户满意度调查表主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form customer-satisfaction-survey-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-satisfaction-survey-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.code')"
                name="customerSatisfactionSurveyCode"
              >
                <a-input
                  v-model:value="formState.customerSatisfactionSurveyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerSatisfactionSurveyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customerid')"
                name="customerId"
              >
                <a-input
                  v-model:value="formState.customerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customerid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerSatisfactionSurveyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveydate')"
                name="surveyDate"
              >
                <a-date-picker
                  v-model:value="formState.surveyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveymethod')"
                name="surveyMethod"
              >
                <a-input-number
                  v-model:value="formState.surveyMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.surveymethod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveytype')"
                name="surveyType"
              >
                <a-input-number
                  v-model:value="formState.surveyType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.surveytype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveyperiod')"
                name="surveyPeriod"
              >
                <a-input-number
                  v-model:value="formState.surveyPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.surveyperiod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveyorby')"
                name="surveyorBy"
              >
                <a-input
                  v-model:value="formState.surveyorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.surveyorby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customercontact')"
                name="customerContact"
              >
                <a-input
                  v-model:value="formState.customerContact"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customercontact') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customerphone')"
                name="customerPhone"
              >
                <a-input
                  v-model:value="formState.customerPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customerphone') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.overallsatisfaction')"
                name="overallSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.overallSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.overallsatisfaction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.totalscore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.totalscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.qualityscore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.qualityscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.deliveryscore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.deliveryscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.servicescore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.servicescore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.pricescore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.pricescore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.technicalscore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.technicalscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customerpraise')"
                name="customerPraise"
              >
                <a-input
                  v-model:value="formState.customerPraise"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customerpraise') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customerfeedback') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.improvementplan')"
                name="improvementPlan"
              >
                <a-input
                  v-model:value="formState.improvementPlan"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.improvementplan') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.surveystatus')"
                name="surveyStatus"
              >
                <a-input-number
                  v-model:value="formState.surveyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.surveystatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.followupstatus')"
                name="followUpStatus"
              >
                <a-input-number
                  v-model:value="formState.followUpStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.followupstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.relatedcomplaintid')"
                name="relatedComplaintId"
              >
                <a-input
                  v-model:value="formState.relatedComplaintId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.relatedcomplaintid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurvey.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.relatedplant') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
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
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="customerSatisfactionSurveyItemTableRef"
      v-model="childCustomerSatisfactionSurveyItemRows"
      :columns="customerSatisfactionSurveyItemFormColumns"
      :title="t('entity.customersatisfactionsurveyitem._self')"
      :add-button-entity="t('entity.customersatisfactionsurveyitem._self')"
      id-field="customerSatisfactionSurveyItemId"
      :default-row="createDefaultCustomerSatisfactionSurveyItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客户满意度调查表主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerSatisfactionSurveyCreate } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","customerSatisfactionSurveyCode","customerId","customerName","customerCode","surveyDate","surveyMethod","surveyType","surveyPeriod","surveyorBy","customerContact","customerPhone","overallSatisfaction","totalScore","qualityScore","deliveryScore","serviceScore","priceScore","technicalScore","customerPraise","customerFeedback","improvementPlan","surveyStatus","followUpStatus","relatedComplaintId","relatedPlant","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childCustomerSatisfactionSurveyItemRows = ref<Record<string, unknown>[]>([])
const customerSatisfactionSurveyItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 customerSatisfactionSurveyItem 可编辑列 */
const customerSatisfactionSurveyItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.customersatisfactionsurveyitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'categoryType',
    title: t('entity.customersatisfactionsurveyitem.categorytype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'itemName',
    title: t('entity.customersatisfactionsurveyitem.itemname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: t('entity.customersatisfactionsurveyitem.itemdescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.customersatisfactionsurveyitem.itemdescription') }),
    width: 140,
  },
  {
    key: 'weight',
    title: t('entity.customersatisfactionsurveyitem.weight'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'score',
    title: t('entity.customersatisfactionsurveyitem.score'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'satisfactionLevel',
    title: t('entity.customersatisfactionsurveyitem.satisfactionlevel'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'customerFeedback',
    title: t('entity.customersatisfactionsurveyitem.customerfeedback'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.customersatisfactionsurveyitem.customerfeedback') }),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerSatisfactionSurveyCreate & { customerSatisfactionSurveyId?: string }> | null | undefined) {
  childCustomerSatisfactionSurveyItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultCustomerSatisfactionSurveyItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childCustomerSatisfactionSurveyItemRows.value.length + 1) * 10,
    categoryType: 0,
    itemName: '',
    itemDescription: '',
    weight: 0,
    score: 0,
    satisfactionLevel: 0,
    customerFeedback: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.customerSatisfactionSurveyId ?? ''
  return {
    ...formState,
    items: customerSatisfactionSurveyItemTableRef.value?.getRows?.() ?? childCustomerSatisfactionSurveyItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      surveyId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerSatisfactionSurveyCreate & { customerSatisfactionSurveyId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 customerSatisfactionSurveyId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerSatisfactionSurveyId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.customerSatisfactionSurveyId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  customerSatisfactionSurveyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.code') }),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customerid') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurvey.customername') }),
      trigger: 'blur'
    }
  ],
  surveyDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveydate') }),
      trigger: 'change'
    }
  ],
  surveyMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveymethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveymethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveytype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveytype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyPeriod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveyperiod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveyperiod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overallSatisfaction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.overallsatisfaction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.overallsatisfaction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveystatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.surveystatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  followUpStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.followupstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurvey.followupstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await customerSatisfactionSurveyItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('surveyMethod' in payload) {
    const rawsurveyMethod = payload.surveyMethod
    payload.surveyMethod = typeof rawsurveyMethod === 'number' ? rawsurveyMethod : Number(rawsurveyMethod)
  }
  if ('surveyType' in payload) {
    const rawsurveyType = payload.surveyType
    payload.surveyType = typeof rawsurveyType === 'number' ? rawsurveyType : Number(rawsurveyType)
  }
  if ('surveyPeriod' in payload) {
    const rawsurveyPeriod = payload.surveyPeriod
    payload.surveyPeriod = typeof rawsurveyPeriod === 'number' ? rawsurveyPeriod : Number(rawsurveyPeriod)
  }
  if ('overallSatisfaction' in payload) {
    const rawoverallSatisfaction = payload.overallSatisfaction
    payload.overallSatisfaction = typeof rawoverallSatisfaction === 'number' ? rawoverallSatisfaction : Number(rawoverallSatisfaction)
  }
  if ('totalScore' in payload) {
    const rawtotalScore = payload.totalScore
    payload.totalScore = typeof rawtotalScore === 'number' ? rawtotalScore : Number(rawtotalScore)
  }
  if ('qualityScore' in payload) {
    const rawqualityScore = payload.qualityScore
    payload.qualityScore = typeof rawqualityScore === 'number' ? rawqualityScore : Number(rawqualityScore)
  }
  if ('deliveryScore' in payload) {
    const rawdeliveryScore = payload.deliveryScore
    payload.deliveryScore = typeof rawdeliveryScore === 'number' ? rawdeliveryScore : Number(rawdeliveryScore)
  }
  if ('serviceScore' in payload) {
    const rawserviceScore = payload.serviceScore
    payload.serviceScore = typeof rawserviceScore === 'number' ? rawserviceScore : Number(rawserviceScore)
  }
  if ('priceScore' in payload) {
    const rawpriceScore = payload.priceScore
    payload.priceScore = typeof rawpriceScore === 'number' ? rawpriceScore : Number(rawpriceScore)
  }
  if ('technicalScore' in payload) {
    const rawtechnicalScore = payload.technicalScore
    payload.technicalScore = typeof rawtechnicalScore === 'number' ? rawtechnicalScore : Number(rawtechnicalScore)
  }
  if ('surveyStatus' in payload) {
    const rawsurveyStatus = payload.surveyStatus
    payload.surveyStatus = typeof rawsurveyStatus === 'number' ? rawsurveyStatus : Number(rawsurveyStatus)
  }
  if ('followUpStatus' in payload) {
    const rawfollowUpStatus = payload.followUpStatus
    payload.followUpStatus = typeof rawfollowUpStatus === 'number' ? rawfollowUpStatus : Number(rawfollowUpStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerSatisfactionSurveyId)
  childCustomerSatisfactionSurveyItemRows.value = []
  customerSatisfactionSurveyItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
