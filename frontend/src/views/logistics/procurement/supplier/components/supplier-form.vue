<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/supplier/components -->
<!-- 文件名称：supplier-form.vue -->
<!-- 功能描述：Takt供货商实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="supplier-form-tabs"
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
                :label="t('entity.supplier.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.supplierId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.code')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.supplierId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.name')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.name') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.shortname')"
                name="supplierShortName"
              >
                <a-input
                  v-model:value="formState.supplierShortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.shortname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.type')"
                name="supplierType"
              >
                <TaktSelect
                  v-model:value="formState.supplierType"
                  dict-type="logistics_supplier_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.type') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.industrysector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.industrysector') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.taxnumber')"
                name="supplierTaxNumber"
              >
                <a-input
                  v-model:value="formState.supplierTaxNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.taxnumber') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.taxrate')"
                name="taxRate"
              >
                <TaktSelect
                  v-model:value="formState.taxRate"
                  dict-type="accounting_tax_rate_param"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.taxrate') })"
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
                :label="t('entity.supplier.registrationcountry')"
                name="registrationCountry"
              >
                <a-input
                  v-model:value="formState.registrationCountry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.registrationcountry') })"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.registrationaddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress1') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.registrationaddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress2') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.registrationaddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress3') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.phone')"
                name="supplierPhone"
              >
                <a-input
                  v-model:value="formState.supplierPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.phone') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.fax')"
                name="supplierFax"
              >
                <a-input
                  v-model:value="formState.supplierFax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.fax') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.email')"
                name="supplierEmail"
              >
                <a-input
                  v-model:value="formState.supplierEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.email') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.website')"
                name="supplierWebsite"
              >
                <a-input
                  v-model:value="formState.supplierWebsite"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.website') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.contactperson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactperson') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactphone') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactemail') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.currencycode') })"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.supplierId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.paymentterms')"
                name="paymentTerms"
              >
                <TaktSelect
                  v-model:value="formState.paymentTerms"
                  dict-type="accounting_payment_terms_param"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.paymentterms') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.level')"
                name="supplierLevel"
              >
                <TaktSelect
                  v-model:value="formState.supplierLevel"
                  dict-type="logistics_grade_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.level') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.evaluationscore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.evaluationscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplier.status')"
                name="supplierStatus"
              >
                <TaktSelect
                  v-model:value="formState.supplierStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.status') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt供货商实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/supplier/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierCreate } from '@/types/logistics/procurement/supplier'
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","supplierCode","supplierName","supplierShortName","supplierType","industrySector","supplierTaxNumber","taxRate","registrationCountry","registrationAddress1","registrationAddress2","registrationAddress3","supplierPhone","supplierFax","supplierEmail","supplierWebsite","contactPerson","contactPhone","contactEmail","currencyCode","paymentTerms","supplierLevel","evaluationScore","supplierStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SupplierCreate & { supplierId?: string }> | null
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
  supplierType: 0,
  taxRate: 13,
  paymentTerms: 0,
  supplierLevel: 0,
  supplierStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 supplierId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.supplierId) {
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
    const isCreate = !props.formData?.supplierId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplier.plantcode') }),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplier.code') }),
      trigger: 'blur'
    }
  ],
  supplierName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplier.name') }),
      trigger: 'blur'
    }
  ],
  supplierType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplier.currencycode') }),
      trigger: 'blur'
    }
  ],
  paymentTerms: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.paymentterms') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.paymentterms') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.taxrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.taxrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  supplierLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.level') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.level') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationScore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.evaluationscore') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.evaluationscore') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  supplierStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplier.status') }))
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
  if ('supplierType' in payload) {
    const rawsupplierType = payload.supplierType
    payload.supplierType = typeof rawsupplierType === 'number' ? rawsupplierType : Number(rawsupplierType)
  }
  if ('paymentTerms' in payload) {
    const rawpaymentTerms = payload.paymentTerms
    payload.paymentTerms = typeof rawpaymentTerms === 'number' ? rawpaymentTerms : Number(rawpaymentTerms)
  }
  if ('taxRate' in payload) {
    const rawTaxRate = payload.taxRate
    payload.taxRate = typeof rawTaxRate === 'number' ? rawTaxRate : Number(rawTaxRate)
  }
  if ('supplierLevel' in payload) {
    const rawsupplierLevel = payload.supplierLevel
    payload.supplierLevel = typeof rawsupplierLevel === 'number' ? rawsupplierLevel : Number(rawsupplierLevel)
  }
  if ('evaluationScore' in payload) {
    const rawevaluationScore = payload.evaluationScore
    payload.evaluationScore = typeof rawevaluationScore === 'number' ? rawevaluationScore : Number(rawevaluationScore)
  }
  if ('supplierStatus' in payload) {
    const rawsupplierStatus = payload.supplierStatus
    payload.supplierStatus = typeof rawsupplierStatus === 'number' ? rawsupplierStatus : Number(rawsupplierStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.supplierId)

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
