<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation-benefits/social-security/components -->
<!-- 文件名称：social-security-form.vue -->
<!-- 功能描述：员工社保缴纳记录维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="social-security-form-tabs"
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
                :label="t('entity.socialSecurity.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.payperiod')"
                name="payPeriod"
              >
                <a-input
                  v-model:value="formState.payPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.payperiod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.base')"
                name="socialSecurityBase"
              >
                <a-input-number
                  v-model:value="formState.socialSecurityBase"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.base') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.pensionamount')"
                name="pensionAmount"
              >
                <a-input-number
                  v-model:value="formState.pensionAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.pensionamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.medicalamount')"
                name="medicalAmount"
              >
                <a-input-number
                  v-model:value="formState.medicalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.medicalamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.unemploymentamount')"
                name="unemploymentAmount"
              >
                <a-input-number
                  v-model:value="formState.unemploymentAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.unemploymentamount') })"
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
                :label="t('entity.socialSecurity.injuryamount')"
                name="injuryAmount"
              >
                <a-input-number
                  v-model:value="formState.injuryAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.injuryamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.maternityamount')"
                name="maternityAmount"
              >
                <a-input-number
                  v-model:value="formState.maternityAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.maternityamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.housingfundbase')"
                name="housingFundBase"
              >
                <a-input-number
                  v-model:value="formState.housingFundBase"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.housingfundbase') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.housingfundamount')"
                name="housingFundAmount"
              >
                <a-input-number
                  v-model:value="formState.housingFundAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.housingfundamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.totalamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.paystatus')"
                name="payStatus"
              >
                <a-input-number
                  v-model:value="formState.payStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.paystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialSecurity.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.relatedplant') })"
                  size="small"
                  allow-clear
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
 * 员工社保缴纳记录维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/compensation-benefits/social-security/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SocialSecurityCreate } from '@/types/human-resource/compensation-benefits/social-security'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","employeeName","payPeriod","socialSecurityBase","pensionAmount","medicalAmount","unemploymentAmount","injuryAmount","maternityAmount","housingFundBase","housingFundAmount","totalAmount","payStatus","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<SocialSecurityCreate & { socialSecurityId?: string }> | null
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
    const isCreate = !props.formData?.socialSecurityId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.employeename') }),
      trigger: 'blur'
    }
  ],
  payPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.socialSecurity.payperiod') }),
      trigger: 'blur'
    }
  ],
  socialSecurityBase: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.base') }),
      trigger: 'change'
    }
  ],
  pensionAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.pensionamount') }),
      trigger: 'change'
    }
  ],
  medicalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.medicalamount') }),
      trigger: 'change'
    }
  ],
  unemploymentAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.unemploymentamount') }),
      trigger: 'change'
    }
  ],
  injuryAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.injuryamount') }),
      trigger: 'change'
    }
  ],
  maternityAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.maternityamount') }),
      trigger: 'change'
    }
  ],
  housingFundBase: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.housingfundbase') }),
      trigger: 'change'
    }
  ],
  housingFundAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.housingfundamount') }),
      trigger: 'change'
    }
  ],
  totalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.totalamount') }),
      trigger: 'change'
    }
  ],
  payStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.socialSecurity.paystatus') }),
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
