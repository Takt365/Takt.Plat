<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey-item/components -->
<!-- 文件名称：customer-satisfaction-survey-item-form.vue -->
<!-- 功能描述：客户满意度调查项目明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-satisfaction-survey-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.surveyid')"
                name="surveyId"
              >
                <a-input
                  v-model:value="formState.surveyId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.surveyid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode')"
                name="customerSatisfactionSurveyCode"
              >
                <a-input
                  v-model:value="formState.customerSatisfactionSurveyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.categorytype')"
                name="categoryType"
              >
                <a-input-number
                  v-model:value="formState.categoryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.categorytype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.itemname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.itemdescription')"
                name="itemDescription"
              >
                <a-textarea
                  v-model:value="formState.itemDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerSatisfactionSurveyItem.itemdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.weight')"
                name="weight"
              >
                <a-input-number
                  v-model:value="formState.weight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.weight') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.score')"
                name="score"
              >
                <a-input-number
                  v-model:value="formState.score"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.score') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.satisfactionlevel')"
                name="satisfactionLevel"
              >
                <a-input-number
                  v-model:value="formState.satisfactionLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.satisfactionlevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.customerfeedback') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.improvementsuggestion')"
                name="improvementSuggestion"
              >
                <a-input
                  v-model:value="formState.improvementSuggestion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.improvementsuggestion') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.followupaction')"
                name="followUpAction"
              >
                <a-input
                  v-model:value="formState.followUpAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.followupaction') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurveyItem.followupstatus')"
                name="followUpStatus"
              >
                <a-input-number
                  v-model:value="formState.followUpStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.followupstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客户满意度调查项目明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerSatisfactionSurveyItemCreate } from '@/types/logistics/quality/complaint/customer-satisfaction-survey-item'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","surveyId","customerSatisfactionSurveyCode","lineNumber","categoryType","itemName","itemDescription","weight","score","satisfactionLevel","customerFeedback","improvementSuggestion","followUpAction","followUpStatus","extFieldJson","remark"]


interface Props {
  formData?: Partial<CustomerSatisfactionSurveyItemCreate & { customerSatisfactionSurveyItemId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.customerSatisfactionSurveyItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  surveyId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.surveyid') }),
      trigger: 'blur'
    }
  ],
  customerSatisfactionSurveyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.customersatisfactionsurveycode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurveyItem.linenumber') }),
      trigger: 'change'
    }
  ],
  categoryType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurveyItem.categorytype') }),
      trigger: 'change'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.itemname') }),
      trigger: 'blur'
    }
  ],
  weight: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurveyItem.weight') }),
      trigger: 'change'
    }
  ],
  followUpStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurveyItem.followupstatus') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
