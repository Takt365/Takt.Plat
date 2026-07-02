<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/vendor/components -->
<!-- 文件名称：vendor-form.vue -->
<!-- 功能描述：Takt经销商实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="vendor-form-tabs"
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
                :label="t('entity.vendor.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.code')"
                name="vendorCode"
              >
                <a-input
                  v-model:value="formState.vendorCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.name')"
                name="vendorName"
              >
                <a-input
                  v-model:value="formState.vendorName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.name') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.shortname')"
                name="vendorShortName"
              >
                <a-input
                  v-model:value="formState.vendorShortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.shortname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.type')"
                name="vendorType"
              >
                <TaktSelect
                  v-model:value="formState.vendorType"
                  dict-type="logistics_vendor_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.type') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.industrysector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.industrysector') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.taxnumber')"
                name="vendorTaxNumber"
              >
                <a-input
                  v-model:value="formState.vendorTaxNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.taxnumber') })"
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
                :label="t('entity.vendor.registrationcountry')"
                name="registrationCountry"
              >
                <a-input
                  v-model:value="formState.registrationCountry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.registrationcountry') })"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.vendor.registrationaddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress1') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.vendor.registrationaddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress2') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.vendor.registrationaddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress3') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.phone')"
                name="vendorPhone"
              >
                <a-input
                  v-model:value="formState.vendorPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.phone') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.fax')"
                name="vendorFax"
              >
                <a-input
                  v-model:value="formState.vendorFax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.fax') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.email')"
                name="vendorEmail"
              >
                <a-input
                  v-model:value="formState.vendorEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.email') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.website')"
                name="vendorWebsite"
              >
                <a-input
                  v-model:value="formState.vendorWebsite"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.website') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.contactperson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactperson') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactphone') })"
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
                :label="t('entity.vendor.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactemail') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.currencycode') })"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.paymentterms')"
                name="paymentTerms"
              >
                <TaktSelect
                  v-model:value="formState.paymentTerms"
                  dict-type="accounting_payment_terms_param"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.paymentterms') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.creditlevel')"
                name="creditLevel"
              >
                <TaktSelect
                  v-model:value="formState.creditLevel"
                  dict-type="logistics_credit_rating_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.creditlevel') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.creditamount')"
                name="creditAmount"
              >
                <a-input-number
                  v-model:value="formState.creditAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.creditamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.authorizedbrand')"
                name="authorizedBrand"
              >
                <a-input
                  v-model:value="formState.authorizedBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.authorizedbrand') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.agentregion')"
                name="agentRegion"
              >
                <a-input
                  v-model:value="formState.agentRegion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.agentregion') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.level')"
                name="vendorLevel"
              >
                <TaktSelect
                  v-model:value="formState.vendorLevel"
                  dict-type="logistics_grade_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.level') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.evaluationscore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.evaluationscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.vendor.isqualified')"
                name="isQualified"
              >
                <a-input-number
                  v-model:value="formState.isQualified"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.isqualified') })"
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
                :label="t('entity.vendor.status')"
                name="vendorStatus"
              >
                <TaktSelect
                  v-model:value="formState.vendorStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.status') })"
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
 * Takt经销商实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/vendor/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { VendorCreate } from '@/types/logistics/procurement/vendor'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","vendorCode","vendorName","vendorShortName","vendorType","industrySector","vendorTaxNumber","registrationCountry","registrationAddress1","registrationAddress2","registrationAddress3","vendorPhone","vendorFax","vendorEmail","vendorWebsite","contactPerson","contactPhone","contactEmail","currencyCode","paymentTerms","creditLevel","creditAmount","authorizedBrand","agentRegion","vendorLevel","evaluationScore","isQualified","vendorStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<VendorCreate & { vendorId?: string }> | null
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
  vendorType: 0,
  paymentTerms: 0,
  creditLevel: 0,
  vendorLevel: 0,
  vendorStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 vendorId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.vendorId) {
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
    const isCreate = !props.formData?.vendorId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.vendor.plantcode') }),
      trigger: 'blur'
    }
  ],
  vendorCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.vendor.code') }),
      trigger: 'blur'
    }
  ],
  vendorName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.vendor.name') }),
      trigger: 'blur'
    }
  ],
  vendorType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.vendor.currencycode') }),
      trigger: 'blur'
    }
  ],
  paymentTerms: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.paymentterms') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.paymentterms') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  creditLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.creditlevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.creditlevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  creditAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.creditamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.creditamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  vendorLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.level') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.level') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationScore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.evaluationscore') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.evaluationscore') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isQualified: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.isqualified') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.isqualified') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  vendorStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.vendor.status') }))
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
  if ('vendorType' in payload) {
    const rawvendorType = payload.vendorType
    payload.vendorType = typeof rawvendorType === 'number' ? rawvendorType : Number(rawvendorType)
  }
  if ('paymentTerms' in payload) {
    const rawpaymentTerms = payload.paymentTerms
    payload.paymentTerms = typeof rawpaymentTerms === 'number' ? rawpaymentTerms : Number(rawpaymentTerms)
  }
  if ('creditLevel' in payload) {
    const rawcreditLevel = payload.creditLevel
    payload.creditLevel = typeof rawcreditLevel === 'number' ? rawcreditLevel : Number(rawcreditLevel)
  }
  if ('creditAmount' in payload) {
    const rawcreditAmount = payload.creditAmount
    payload.creditAmount = typeof rawcreditAmount === 'number' ? rawcreditAmount : Number(rawcreditAmount)
  }
  if ('vendorLevel' in payload) {
    const rawvendorLevel = payload.vendorLevel
    payload.vendorLevel = typeof rawvendorLevel === 'number' ? rawvendorLevel : Number(rawvendorLevel)
  }
  if ('evaluationScore' in payload) {
    const rawevaluationScore = payload.evaluationScore
    payload.evaluationScore = typeof rawevaluationScore === 'number' ? rawevaluationScore : Number(rawevaluationScore)
  }
  if ('isQualified' in payload) {
    const rawisQualified = payload.isQualified
    payload.isQualified = typeof rawisQualified === 'number' ? rawisQualified : Number(rawisQualified)
  }
  if ('vendorStatus' in payload) {
    const rawvendorStatus = payload.vendorStatus
    payload.vendorStatus = typeof rawvendorStatus === 'number' ? rawvendorStatus : Number(rawvendorStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.vendorId)

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
