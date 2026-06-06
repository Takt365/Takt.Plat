<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation-benefits/tax-calc/components -->
<!-- 文件名称：tax-calc-form.vue -->
<!-- 功能描述：个税计算规则维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="tax-calc-form-tabs"
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
                :label="t('entity.taxCalc.rulecode')"
                name="ruleCode"
              >
                <a-input
                  v-model:value="formState.ruleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.rulecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.rulename')"
                name="ruleName"
              >
                <a-input
                  v-model:value="formState.ruleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.rulename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.taxyear')"
                name="taxYear"
              >
                <a-input-number
                  v-model:value="formState.taxYear"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.taxyear') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.taxthreshold')"
                name="taxThreshold"
              >
                <a-input-number
                  v-model:value="formState.taxThreshold"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.taxthreshold') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.taxableincomemin')"
                name="taxableIncomeMin"
              >
                <a-input-number
                  v-model:value="formState.taxableIncomeMin"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.taxableincomemin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.taxableincomemax')"
                name="taxableIncomeMax"
              >
                <a-input-number
                  v-model:value="formState.taxableIncomeMax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.taxableincomemax') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.taxrate')"
                name="taxRate"
              >
                <a-input-number
                  v-model:value="formState.taxRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.taxrate') })"
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
                :label="t('entity.taxCalc.quickdeduction')"
                name="quickDeduction"
              >
                <a-input-number
                  v-model:value="formState.quickDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.quickdeduction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.specialdeductionstandard')"
                name="specialDeductionStandard"
              >
                <a-input-number
                  v-model:value="formState.specialDeductionStandard"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.specialdeductionstandard') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.socialsecuritydeductionrate')"
                name="socialSecurityDeductionRate"
              >
                <a-input-number
                  v-model:value="formState.socialSecurityDeductionRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.socialsecuritydeductionrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.housingfunddeductionrate')"
                name="housingFundDeductionRate"
              >
                <a-input-number
                  v-model:value="formState.housingFundDeductionRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.housingfunddeductionrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.calculationformula')"
                name="calculationFormula"
              >
                <a-input
                  v-model:value="formState.calculationFormula"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.calculationformula') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.taxCalc.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.taxCalc.description') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.taxCalc.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.status')"
                name="taxCalcStatus"
              >
                <a-input-number
                  v-model:value="formState.taxCalcStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.taxCalc.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.taxCalc.relatedplant') })"
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
 * 个税计算规则维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/compensation-benefits/tax-calc/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TaxCalcCreate } from '@/types/human-resource/compensation-benefits/tax-calc'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ruleCode","ruleName","taxYear","taxThreshold","taxableIncomeMin","taxableIncomeMax","taxRate","quickDeduction","specialDeductionStandard","socialSecurityDeductionRate","housingFundDeductionRate","calculationFormula","description","effectiveDate","taxCalcStatus","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<TaxCalcCreate & { taxCalcId?: string }> | null
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
    const isCreate = !props.formData?.taxCalcId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  ruleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.taxCalc.rulecode') }),
      trigger: 'blur'
    }
  ],
  ruleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.taxCalc.rulename') }),
      trigger: 'blur'
    }
  ],
  taxYear: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.taxyear') }),
      trigger: 'change'
    }
  ],
  taxThreshold: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.taxthreshold') }),
      trigger: 'change'
    }
  ],
  taxableIncomeMin: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.taxableincomemin') }),
      trigger: 'change'
    }
  ],
  taxableIncomeMax: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.taxableincomemax') }),
      trigger: 'change'
    }
  ],
  taxRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.taxrate') }),
      trigger: 'change'
    }
  ],
  quickDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.quickdeduction') }),
      trigger: 'change'
    }
  ],
  specialDeductionStandard: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.specialdeductionstandard') }),
      trigger: 'change'
    }
  ],
  socialSecurityDeductionRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.socialsecuritydeductionrate') }),
      trigger: 'change'
    }
  ],
  housingFundDeductionRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.housingfunddeductionrate') }),
      trigger: 'change'
    }
  ],
  calculationFormula: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.taxCalc.calculationformula') }),
      trigger: 'blur'
    }
  ],
  description: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.taxCalc.description') }),
      trigger: 'blur'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.effectivedate') }),
      trigger: 'change'
    }
  ],
  taxCalcStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.taxCalc.status') }),
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
