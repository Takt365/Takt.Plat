<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract/components -->
<!-- 文件名称：service-contract-form.vue -->
<!-- 功能描述：服务合同实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="service-contract-form-tabs"
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
                :label="t('entity.serviceContract.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.code')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contractname')"
                name="contractName"
              >
                <a-input
                  v-model:value="formState.contractName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contracttype')"
                name="contractType"
              >
                <a-input-number
                  v-model:value="formState.contractType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contracttype') })"
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
                :label="t('entity.serviceContract.contractstatus')"
                name="contractStatus"
              >
                <a-input-number
                  v-model:value="formState.contractStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.signdate')"
                name="signDate"
              >
                <a-date-picker
                  v-model:value="formState.signDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.signdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.expirydate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.expirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contractamount')"
                name="contractAmount"
              >
                <a-input-number
                  v-model:value="formState.contractAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.currencycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.paymentterms')"
                name="paymentTerms"
              >
                <a-input-number
                  v-model:value="formState.paymentTerms"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.paymentterms') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceContract.servicescope')"
                name="serviceScope"
              >
                <a-textarea
                  v-model:value="formState.serviceScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceContract.servicescope') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.slaresponsehours')"
                name="slaResponseHours"
              >
                <a-input-number
                  v-model:value="formState.slaResponseHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresponsehours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.slaresolvehours')"
                name="slaResolveHours"
              >
                <a-input-number
                  v-model:value="formState.slaResolveHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresolvehours') })"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.serviceContract.accountmanager')"
                name="accountManager"
              >
                <a-input
                  v-model:value="formState.accountManager"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.accountmanager') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.serviceorders')"
                name="serviceOrders"
              >
                <a-input
                  v-model:value="formState.serviceOrders"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.serviceorders') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.servicerequests')"
                name="serviceRequests"
              >
                <a-input
                  v-model:value="formState.serviceRequests"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.servicerequests') })"
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
 * 服务合同实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/service/service-contract/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ServiceContractCreate } from '@/types/logistics/customer-service/service-contract'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","serviceContractCode","contractName","clientId","clientCode","clientName","contractType","contractStatus","signDate","effectiveDate","expiryDate","contractAmount","currencyCode","paymentTerms","serviceScope","slaResponseHours","slaResolveHours","accountManager","sortOrder","serviceOrders","serviceRequests","extFieldJson","remark"]


interface Props {
  formData?: Partial<ServiceContractCreate & { serviceContractId?: string }> | null
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
    const isCreate = !props.formData?.serviceContractId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.plantcode') }),
      trigger: 'blur'
    }
  ],
  serviceContractCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.code') }),
      trigger: 'blur'
    }
  ],
  contractName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractname') }),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientid') }),
      trigger: 'blur'
    }
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientcode') }),
      trigger: 'blur'
    }
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientname') }),
      trigger: 'blur'
    }
  ],
  contractType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contracttype') }),
      trigger: 'change'
    }
  ],
  contractStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contractstatus') }),
      trigger: 'change'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedate') }),
      trigger: 'change'
    }
  ],
  contractAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contractamount') }),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.currencycode') }),
      trigger: 'blur'
    }
  ],
  paymentTerms: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.paymentterms') }),
      trigger: 'change'
    }
  ],
  slaResponseHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.slaresponsehours') }),
      trigger: 'change'
    }
  ],
  slaResolveHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.slaresolvehours') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.sortorder') }),
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
