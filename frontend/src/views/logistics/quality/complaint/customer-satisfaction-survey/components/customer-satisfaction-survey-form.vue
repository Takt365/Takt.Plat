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
    class="takt-generated-form customer-satisfaction-survey-form flex flex-col min-h-0 overflow-visible"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
      :title="customerSatisfactionSurveyItemPi.self()"
      :add-button-entity="customerSatisfactionSurveyItemPi.self()"
      id-field="customerSatisfactionSurveyItemId"
      :default-row="createDefaultCustomerSatisfactionSurveyItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-surveyId="{ record }">
        <TaktSelect
          v-model:value="record.surveyId"
          api-url="TaktCustomerSatisfactionSurveys/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerSatisfactionSurveyItemPi.queryPh('surveyId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-categoryType="{ record }">
        <TaktSelect
          v-model:value="record.categoryType"
          dict-type="logistics_quality_satisfaction_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerSatisfactionSurveyItemPi.ph('categoryType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-satisfactionLevel="{ record }">
        <TaktSelect
          v-model:value="record.satisfactionLevel"
          dict-type="logistics_quality_satisfaction_level"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerSatisfactionSurveyItemPi.ph('satisfactionLevel')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-followUpStatus="{ record }">
        <TaktSelect
          v-model:value="record.followUpStatus"
          dict-type="logistics_quality_follow_up_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerSatisfactionSurveyItemPi.ph('followUpStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerSatisfactionSurveyItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客户满意度调查表主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerSatisfactionSurveyI18n } from '../composables/use-customer-satisfaction-survey-i18n'

/** 实体字段 i18n */
const pi = useCustomerSatisfactionSurveyI18n()

import type { CustomerSatisfactionSurveyCreate } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","customerSatisfactionSurveyCode","customerId","customerName1","customerCode","surveyDate","surveyMethod","surveyType","surveyPeriod","surveyorBy","customerContact","customerPhone","overallSatisfaction","totalScore","qualityScore","deliveryScore","serviceScore","priceScore","technicalScore","customerPraise","customerFeedback","improvementPlan","relatedComplaintId","attachments","surveyStatus","plantCode","followUpStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useCustomerSatisfactionSurveyItemI18n } from '../composables/use-customer-satisfaction-survey-item-i18n'

const customerSatisfactionSurveyItemPi = useCustomerSatisfactionSurveyItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childCustomerSatisfactionSurveyItemRows = ref<Record<string, unknown>[]>([])
const customerSatisfactionSurveyItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedCustomerSatisfactionSurveyItemRow(row: Record<string, unknown>): boolean {
  const id = row.customerSatisfactionSurveyItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextCustomerSatisfactionSurveyItemLineNumber(): number {
  const rows = customerSatisfactionSurveyItemTableRef.value?.getRows?.() ?? childCustomerSatisfactionSurveyItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 customerSatisfactionSurveyItem 可编辑列 */
const customerSatisfactionSurveyItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'surveyId',
    title: customerSatisfactionSurveyItemPi.label('surveyId'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: customerSatisfactionSurveyItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'categoryType',
    title: customerSatisfactionSurveyItemPi.label('categoryType'),
    width: 140,
  },
  {
    key: 'itemName',
    title: customerSatisfactionSurveyItemPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: customerSatisfactionSurveyItemPi.label('itemDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: customerSatisfactionSurveyItemPi.ph('itemDescription'),
    width: 180,
  },
  {
    key: 'weight',
    title: customerSatisfactionSurveyItemPi.label('weight'),
    width: 140,
  },
  {
    key: 'score',
    title: customerSatisfactionSurveyItemPi.label('score'),
    width: 140,
  },
  {
    key: 'satisfactionLevel',
    title: customerSatisfactionSurveyItemPi.label('satisfactionLevel'),
    width: 140,
  },
  {
    key: 'customerFeedback',
    title: customerSatisfactionSurveyItemPi.label('customerFeedback'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerSatisfactionSurveyItemPi.ph('customerFeedback'),
  },
  {
    key: 'improvementSuggestion',
    title: customerSatisfactionSurveyItemPi.label('improvementSuggestion'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerSatisfactionSurveyItemPi.ph('improvementSuggestion'),
  },
  {
    key: 'followUpAction',
    title: customerSatisfactionSurveyItemPi.label('followUpAction'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerSatisfactionSurveyItemPi.ph('followUpAction'),
  },
  {
    key: 'followUpStatus',
    title: customerSatisfactionSurveyItemPi.label('followUpStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: customerSatisfactionSurveyItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerSatisfactionSurveyCreate & { customerSatisfactionSurveyId?: string }> | null | undefined) {
  const rows_customerSatisfactionSurveyItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childCustomerSatisfactionSurveyItemRows.value = rows_customerSatisfactionSurveyItem
}

function createDefaultCustomerSatisfactionSurveyItemRow(): Record<string, unknown> {
  return {
    surveyId: '',
    lineNumber: allocateNextCustomerSatisfactionSurveyItemLineNumber(),
    categoryType: 0,
    itemName: '',
    itemDescription: '',
    weight: 0,
    score: 0,
    satisfactionLevel: 0,
    customerFeedback: '',
    improvementSuggestion: '',
    followUpAction: '',
    followUpStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.customerSatisfactionSurveyId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: customerSatisfactionSurveyItemTableRef.value?.getRows?.() ?? childCustomerSatisfactionSurveyItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        customerSatisfactionSurveyCode: masterId,
      }
      if (isUpdate && isPersistedCustomerSatisfactionSurveyItemRow(row)) {
        normalized.customerSatisfactionSurveyItemId = row.customerSatisfactionSurveyItemId
      } else {
        delete normalized.customerSatisfactionSurveyItemId
      }
      return normalized
    }),
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  surveyMethod: 0,
  surveyType: 0,
  surveyPeriod: 1,
  surveyStatus: 0,
  followUpStatus: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

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
      message: pi.ph('customerSatisfactionSurveyCode'),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: pi.ph('customerId'),
      trigger: 'change'
    }
  ],
  customerName1: [
    {
      required: true,
      message: pi.ph('customerName1'),
      trigger: 'blur'
    }
  ],
  surveyDate: [
    {
      required: true,
      message: pi.ph('surveyDate'),
      trigger: 'change'
    }
  ],
  surveyMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('surveyMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('surveyMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('surveyType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('surveyType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyPeriod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('surveyPeriod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('surveyPeriod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overallSatisfaction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overallSatisfaction'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overallSatisfaction'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  surveyStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('surveyStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('surveyStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  followUpStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('followUpStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('followUpStatus'))
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
