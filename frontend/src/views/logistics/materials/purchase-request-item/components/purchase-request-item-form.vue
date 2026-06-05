<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-request-item/components -->
<!-- 文件名称：purchase-request-item-form.vue -->
<!-- 功能描述：Takt采购申请明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="purchase-request-item-form-tabs"
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
                :label="t('entity.purchaseRequestItem.purchaserequestid')"
                name="purchaseRequestId"
              >
                <a-input
                  v-model:value="formState.purchaseRequestId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.purchaserequestcode')"
                name="purchaseRequestCode"
              >
                <a-input
                  v-model:value="formState.purchaseRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.materialspecification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialspecification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.requestunit')"
                name="requestUnit"
              >
                <a-input
                  v-model:value="formState.requestUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestunit') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.requestquantity')"
                name="requestQuantity"
              >
                <a-input-number
                  v-model:value="formState.requestQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.convertedquantity')"
                name="convertedQuantity"
              >
                <a-input-number
                  v-model:value="formState.convertedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.convertedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.estimatedunitprice')"
                name="estimatedUnitPrice"
              >
                <a-input-number
                  v-model:value="formState.estimatedUnitPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.estimatedunitprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.estimatedamount')"
                name="estimatedAmount"
              >
                <a-input-number
                  v-model:value="formState.estimatedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.estimatedamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.referencesuppliercode')"
                name="referenceSupplierCode"
              >
                <a-input
                  v-model:value="formState.referenceSupplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.referencesuppliercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequestItem.referencesuppliername')"
                name="referenceSupplierName"
              >
                <a-input
                  v-model:value="formState.referenceSupplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.referencesuppliername') })"
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
 * Takt采购申请明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchase-request-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseRequestItemCreate } from '@/types/logistics/materials/purchase-request-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","purchaseRequestId","purchaseRequestCode","lineNumber","materialCode","materialName","materialSpecification","requestUnit","requestQuantity","convertedQuantity","estimatedUnitPrice","estimatedAmount","referenceSupplierCode","referenceSupplierName","extFieldJson","remark"]


interface Props {
  formData?: Partial<PurchaseRequestItemCreate & { purchaseRequestItemId?: string }> | null
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
    const isCreate = !props.formData?.purchaseRequestItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  purchaseRequestId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestid') }),
      trigger: 'blur'
    }
  ],
  purchaseRequestCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.purchaserequestcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequestItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialname') }),
      trigger: 'blur'
    }
  ],
  requestUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestunit') }),
      trigger: 'blur'
    }
  ],
  requestQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequestItem.requestquantity') }),
      trigger: 'change'
    }
  ],
  convertedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequestItem.convertedquantity') }),
      trigger: 'change'
    }
  ],
  estimatedUnitPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequestItem.estimatedunitprice') }),
      trigger: 'change'
    }
  ],
  estimatedAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequestItem.estimatedamount') }),
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
