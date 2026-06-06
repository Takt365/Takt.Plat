<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/countersign/components -->
<!-- 文件名称：countersign-form.vue -->
<!-- 功能描述：会签单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="countersign-form-tabs"
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
                :label="t('entity.countersign.code')"
                name="countersignCode"
              >
                <a-input
                  v-model:value="formState.countersignCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.depts')"
                name="countersignDepts"
              >
                <a-input
                  v-model:value="formState.countersignDepts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.depts') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.financedept')"
                name="financeDept"
              >
                <a-input
                  v-model:value="formState.financeDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.financedept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.budgetreviewcomment')"
                name="budgetReviewComment"
              >
                <a-input
                  v-model:value="formState.budgetReviewComment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetreviewcomment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.executiveoffice')"
                name="executiveOffice"
              >
                <a-input
                  v-model:value="formState.executiveOffice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.executiveoffice') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.applicantby')"
                name="applicantBy"
              >
                <a-input
                  v-model:value="formState.applicantBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicantby') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.countersign.applicationdept')"
                name="applicationDept"
              >
                <a-input
                  v-model:value="formState.applicationDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationdept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.costbearerdept')"
                name="costBearerDept"
              >
                <a-input
                  v-model:value="formState.costBearerDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.costbearerdept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.isbudget')"
                name="isBudget"
              >
                <a-input-number
                  v-model:value="formState.isBudget"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.isbudget') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.budgetitem')"
                name="budgetItem"
              >
                <a-input
                  v-model:value="formState.budgetItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetitem') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.budgetamount')"
                name="budgetAmount"
              >
                <a-input-number
                  v-model:value="formState.budgetAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.budgetamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.applicationamount')"
                name="applicationAmount"
              >
                <a-input-number
                  v-model:value="formState.applicationAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.title')"
                name="countersignTitle"
              >
                <a-input
                  v-model:value="formState.countersignTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.applicationreason')"
                name="applicationReason"
              >
                <a-input
                  v-model:value="formState.applicationReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.applicationreason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.countersign.budgetusagedescription')"
                name="budgetUsageDescription"
              >
                <a-textarea
                  v-model:value="formState.budgetUsageDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.countersign.budgetusagedescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.targetandexpectedbenefit')"
                name="targetAndExpectedBenefit"
              >
                <a-input
                  v-model:value="formState.targetAndExpectedBenefit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.targetandexpectedbenefit') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.countersign.attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.attachments') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.countersign.status')"
                name="countersignStatus"
              >
                <a-input-number
                  v-model:value="formState.countersignStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.countersign.status') })"
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
 * 会签单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/countersign/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CountersignCreate } from '@/types/accounting/financial/countersign'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","countersignCode","countersignDepts","financeDept","budgetReviewComment","executiveOffice","flowInstanceId","applicantBy","applicationDept","costBearerDept","isBudget","budgetItem","budgetAmount","applicationAmount","countersignTitle","applicationReason","budgetUsageDescription","targetAndExpectedBenefit","attachments","countersignStatus","extFieldJson","remark"]


interface Props {
  formData?: Partial<CountersignCreate & { countersignId?: string }> | null
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
    const isCreate = !props.formData?.countersignId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  countersignCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.countersign.code') }),
      trigger: 'blur'
    }
  ],
  applicantBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.countersign.applicantby') }),
      trigger: 'blur'
    }
  ],
  isBudget: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.countersign.isbudget') }),
      trigger: 'change'
    }
  ],
  budgetAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.countersign.budgetamount') }),
      trigger: 'change'
    }
  ],
  applicationAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.countersign.applicationamount') }),
      trigger: 'change'
    }
  ],
  countersignStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.countersign.status') }),
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
