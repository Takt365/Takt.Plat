<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchasing/supplier/components -->
<!-- 文件名称：supplier-form.vue -->
<!-- 功能描述：Takt供货商实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
                :label="t('entity.supplier.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.plantcode') })"
                  size="small"
                  allow-clear
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
                  size="small"
                  allow-clear
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
                  size="small"
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
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.type')"
                name="supplierType"
              >
                <a-input-number
                  v-model:value="formState.supplierType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.type') })"
                  size="small"
                  style="width: 100%"
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
                  size="small"
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
                :label="t('entity.supplier.registrationcountry')"
                name="registrationCountry"
              >
                <a-input
                  v-model:value="formState.registrationCountry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.registrationcountry') })"
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
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
                :label="t('entity.supplier.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactemail') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.currencycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.paymentterms')"
                name="paymentTerms"
              >
                <a-input-number
                  v-model:value="formState.paymentTerms"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.paymentterms') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.level')"
                name="supplierLevel"
              >
                <a-input-number
                  v-model:value="formState.supplierLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.level') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.evaluationscore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.evaluationscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.isqualified')"
                name="isQualified"
              >
                <a-input-number
                  v-model:value="formState.isQualified"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.isqualified') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.status')"
                name="supplierStatus"
              >
                <a-input-number
                  v-model:value="formState.supplierStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplier.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.sortorder') })"
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
 * Takt供货商实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchasing/supplier/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierCreate } from '@/types/logistics/materials/supplier'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","supplierCode","supplierName","supplierShortName","supplierType","industrySector","supplierTaxNumber","registrationCountry","registrationAddress1","registrationAddress2","registrationAddress3","supplierPhone","supplierFax","supplierEmail","supplierWebsite","contactPerson","contactPhone","contactEmail","currencyCode","paymentTerms","supplierLevel","evaluationScore","isQualified","supplierStatus","sortOrder","extFieldJson","remark"]


interface Props {
  formData?: Partial<SupplierCreate & { supplierId?: string }> | null
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
    const isCreate = !props.formData?.supplierId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

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
  supplierType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.type') }),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplier.currencycode') }),
      trigger: 'blur'
    }
  ],
  paymentTerms: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.paymentterms') }),
      trigger: 'change'
    }
  ],
  supplierLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.level') }),
      trigger: 'change'
    }
  ],
  evaluationScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.evaluationscore') }),
      trigger: 'change'
    }
  ],
  isQualified: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.isqualified') }),
      trigger: 'change'
    }
  ],
  supplierStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.status') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplier.sortorder') }),
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
