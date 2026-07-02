<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/customer/components -->
<!-- 文件名称：customer-form.vue -->
<!-- 功能描述：Takt客户信息实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-form-tabs"
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="pi.ph('customerCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerName')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="pi.ph('customerName')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerShortName')"
                name="customerShortName"
              >
                <a-input
                  v-model:value="formState.customerShortName"
                  :placeholder="pi.ph('customerShortName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerType')"
                name="customerType"
              >
                <TaktSelect
                  v-model:value="formState.customerType"
                  dict-type="logistics_customer_category"
                  :placeholder="pi.ph('customerType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industrySector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="pi.ph('industrySector')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerTaxNumber')"
                name="customerTaxNumber"
              >
                <a-input
                  v-model:value="formState.customerTaxNumber"
                  :placeholder="pi.ph('customerTaxNumber')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRate')"
                name="taxRate"
              >
                <TaktSelect
                  v-model:value="formState.taxRate"
                  dict-type="accounting_tax_rate_param"
                  :placeholder="pi.ph('taxRate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationCountry')"
                name="registrationCountry"
              >
                <a-input
                  v-model:value="formState.registrationCountry"
                  :placeholder="pi.ph('registrationCountry')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="pi.ph('registrationAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="pi.ph('registrationAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="pi.ph('registrationAddress3')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerPhone')"
                name="customerPhone"
              >
                <a-input
                  v-model:value="formState.customerPhone"
                  :placeholder="pi.ph('customerPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerFax')"
                name="customerFax"
              >
                <a-input
                  v-model:value="formState.customerFax"
                  :placeholder="pi.ph('customerFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerEmail')"
                name="customerEmail"
              >
                <a-input
                  v-model:value="formState.customerEmail"
                  :placeholder="pi.ph('customerEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerWebsite')"
                name="customerWebsite"
              >
                <a-input
                  v-model:value="formState.customerWebsite"
                  :placeholder="pi.ph('customerWebsite')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPerson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="pi.ph('contactPerson')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPhone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="pi.ph('contactPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactEmail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="pi.ph('contactEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="pi.ph('currencyCode')"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentTerms')"
                name="paymentTerms"
              >
                <TaktSelect
                  v-model:value="formState.paymentTerms"
                  dict-type="accounting_payment_terms_param"
                  :placeholder="pi.ph('paymentTerms')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('creditLevel')"
                name="creditLevel"
              >
                <TaktSelect
                  v-model:value="formState.creditLevel"
                  dict-type="logistics_credit_rating_category"
                  :placeholder="pi.ph('creditLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('creditAmount')"
                name="creditAmount"
              >
                <a-input-number
                  v-model:value="formState.creditAmount"
                  :placeholder="pi.ph('creditAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountRate')"
                name="discountRate"
              >
                <TaktSelect
                  v-model:value="formState.discountRate"
                  dict-type="logistics_discount_rate_param"
                  :placeholder="pi.ph('discountRate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesBy')"
                name="salesBy"
              >
                <TaktSelect
                  v-model:value="formState.salesBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('salesBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerLevel')"
                name="customerLevel"
              >
                <TaktSelect
                  v-model:value="formState.customerLevel"
                  dict-type="logistics_customer_level_category"
                  :placeholder="pi.ph('customerLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationScore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="pi.ph('evaluationScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('customerStatus')"
                name="customerStatus"
              >
                <TaktSelect
                  v-model:value="formState.customerStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('customerStatus')"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt客户信息实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/customer/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerI18n } from '../composables/use-customer-i18n'

/** 实体字段 i18n */
const pi = useCustomerI18n()
import type { CustomerCreate } from '@/types/logistics/sales/customer'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.companyDefaultCulture) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerCreate & { customerId?: string }> | null
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
  customerType: 0,
  taxRate: 13,
  paymentTerms: "prepayship",
  creditLevel: 0,
  discountRate: 0,
  customerLevel: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.customerId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: pi.ph('customerName'),
      trigger: 'blur'
    }
  ],
  customerType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'blur'
    }
  ],
  paymentTerms: [
    {
      required: true,
      message: pi.ph('paymentTerms'),
      trigger: 'change'
    }
  ],
  creditLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('creditLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('creditLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  creditAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('creditAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('creditAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  discountRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  customerLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationScore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  customerStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('customerType' in payload) {
    const rawcustomerType = payload.customerType
    payload.customerType = typeof rawcustomerType === 'number' ? rawcustomerType : Number(rawcustomerType)
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    payload.taxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
  }
  if ('creditLevel' in payload) {
    const rawcreditLevel = payload.creditLevel
    payload.creditLevel = typeof rawcreditLevel === 'number' ? rawcreditLevel : Number(rawcreditLevel)
  }
  if ('creditAmount' in payload) {
    const rawcreditAmount = payload.creditAmount
    payload.creditAmount = typeof rawcreditAmount === 'number' ? rawcreditAmount : Number(rawcreditAmount)
  }
  if ('discountRate' in payload) {
    const rawdiscountRate = payload.discountRate
    payload.discountRate = typeof rawdiscountRate === 'number' ? rawdiscountRate : Number(rawdiscountRate)
  }
  if ('customerLevel' in payload) {
    const rawcustomerLevel = payload.customerLevel
    payload.customerLevel = typeof rawcustomerLevel === 'number' ? rawcustomerLevel : Number(rawcustomerLevel)
  }
  if ('evaluationScore' in payload) {
    const rawevaluationScore = payload.evaluationScore
    payload.evaluationScore = typeof rawevaluationScore === 'number' ? rawevaluationScore : Number(rawevaluationScore)
  }
  if ('customerStatus' in payload) {
    const rawcustomerStatus = payload.customerStatus
    payload.customerStatus = typeof rawcustomerStatus === 'number' ? rawcustomerStatus : Number(rawcustomerStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerId)

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
