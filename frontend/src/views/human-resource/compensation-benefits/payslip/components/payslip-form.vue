<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation-benefits/payslip/components -->
<!-- 文件名称：payslip-form.vue -->
<!-- 功能描述：员工薪资条维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="payslip-form-tabs"
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
                :label="t('entity.payslip.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.payperiod')"
                name="payPeriod"
              >
                <a-input
                  v-model:value="formState.payPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.payperiod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.basesalary')"
                name="baseSalary"
              >
                <a-input-number
                  v-model:value="formState.baseSalary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.basesalary') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.positionallowance')"
                name="positionAllowance"
              >
                <a-input-number
                  v-model:value="formState.positionAllowance"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.positionallowance') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.performancebonus')"
                name="performanceBonus"
              >
                <a-input-number
                  v-model:value="formState.performanceBonus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.performancebonus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.overtimepay')"
                name="overtimePay"
              >
                <a-input-number
                  v-model:value="formState.overtimePay"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.overtimepay') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.allowancetotal')"
                name="allowanceTotal"
              >
                <a-input-number
                  v-model:value="formState.allowanceTotal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.allowancetotal') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.grossamount')"
                name="grossAmount"
              >
                <a-input-number
                  v-model:value="formState.grossAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.grossamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.socialsecuritydeduction')"
                name="socialSecurityDeduction"
              >
                <a-input-number
                  v-model:value="formState.socialSecurityDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.socialsecuritydeduction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.housingfunddeduction')"
                name="housingFundDeduction"
              >
                <a-input-number
                  v-model:value="formState.housingFundDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.housingfunddeduction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.taxdeduction')"
                name="taxDeduction"
              >
                <a-input-number
                  v-model:value="formState.taxDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.taxdeduction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.otherdeduction')"
                name="otherDeduction"
              >
                <a-input-number
                  v-model:value="formState.otherDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.otherdeduction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.netamount')"
                name="netAmount"
              >
                <a-input-number
                  v-model:value="formState.netAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.netamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.issuestatus')"
                name="issueStatus"
              >
                <a-input-number
                  v-model:value="formState.issueStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.issuestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.issuedate')"
                name="issueDate"
              >
                <a-date-picker
                  v-model:value="formState.issueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.payslip.issuedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.relatedplant') })"
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
 * 员工薪资条维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/compensation-benefits/payslip/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PayslipCreate } from '@/types/human-resource/compensation-benefits/payslip'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","employeeName","payPeriod","baseSalary","positionAllowance","performanceBonus","overtimePay","allowanceTotal","grossAmount","socialSecurityDeduction","housingFundDeduction","taxDeduction","otherDeduction","netAmount","issueStatus","issueDate","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<PayslipCreate & { payslipId?: string }> | null
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
    const isCreate = !props.formData?.payslipId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.employeename') }),
      trigger: 'blur'
    }
  ],
  payPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.payperiod') }),
      trigger: 'blur'
    }
  ],
  baseSalary: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.basesalary') }),
      trigger: 'change'
    }
  ],
  positionAllowance: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.positionallowance') }),
      trigger: 'change'
    }
  ],
  performanceBonus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.performancebonus') }),
      trigger: 'change'
    }
  ],
  overtimePay: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.overtimepay') }),
      trigger: 'change'
    }
  ],
  allowanceTotal: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.allowancetotal') }),
      trigger: 'change'
    }
  ],
  grossAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.grossamount') }),
      trigger: 'change'
    }
  ],
  socialSecurityDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.socialsecuritydeduction') }),
      trigger: 'change'
    }
  ],
  housingFundDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.housingfunddeduction') }),
      trigger: 'change'
    }
  ],
  taxDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.taxdeduction') }),
      trigger: 'change'
    }
  ],
  otherDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.otherdeduction') }),
      trigger: 'change'
    }
  ],
  netAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.netamount') }),
      trigger: 'change'
    }
  ],
  issueStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.payslip.issuestatus') }),
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
