<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/serial/product-serial-outbound/components -->
<!-- 文件名称：product-serial-outbound-form.vue -->
<!-- 功能描述：产品序列号出库主表实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="product-serial-outbound-form-tabs"
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
                :label="t('entity.productSerialOutbound.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.outboundno')"
                name="outboundNo"
              >
                <a-input
                  v-model:value="formState.outboundNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.outboundno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.shippinginvoiceno')"
                name="shippingInvoiceNo"
              >
                <a-input
                  v-model:value="formState.shippingInvoiceNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippinginvoiceno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.outbounddate')"
                name="outboundDate"
              >
                <a-date-picker
                  v-model:value="formState.outboundDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.outbounddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.destination')"
                name="destination"
              >
                <a-input
                  v-model:value="formState.destination"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destination') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.shippingmethod')"
                name="shippingMethod"
              >
                <a-input
                  v-model:value="formState.shippingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippingmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.destinationport')"
                name="destinationPort"
              >
                <a-input
                  v-model:value="formState.destinationPort"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destinationport') })"
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
                :label="t('entity.productSerialOutbound.outboundtype')"
                name="outboundType"
              >
                <a-input-number
                  v-model:value="formState.outboundType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.outboundtype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.warehousecode')"
                name="warehouseCode"
              >
                <a-input
                  v-model:value="formState.warehouseCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.warehousecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.locationcode')"
                name="locationCode"
              >
                <a-input
                  v-model:value="formState.locationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.locationcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.relatedcompany')"
                name="relatedCompany"
              >
                <a-input
                  v-model:value="formState.relatedCompany"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.relatedcompany') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.totalquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productSerialOutbound.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.items') })"
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
 * 产品序列号出库主表实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/serial/product-serial-outbound/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ProductSerialOutboundCreate } from '@/types/logistics/serial/product-serial-outbound'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","outboundNo","shippingInvoiceNo","outboundDate","destination","shippingMethod","destinationPort","outboundType","warehouseCode","locationCode","relatedCompany","totalQuantity","items","extFieldJson","remark"]


interface Props {
  formData?: Partial<ProductSerialOutboundCreate & { productSerialOutboundId?: string }> | null
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
    const isCreate = !props.formData?.productSerialOutboundId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.plantcode') }),
      trigger: 'blur'
    }
  ],
  outboundNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.outboundno') }),
      trigger: 'blur'
    }
  ],
  shippingInvoiceNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippinginvoiceno') }),
      trigger: 'blur'
    }
  ],
  outboundDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.outbounddate') }),
      trigger: 'change'
    }
  ],
  destination: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destination') }),
      trigger: 'blur'
    }
  ],
  shippingMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.shippingmethod') }),
      trigger: 'blur'
    }
  ],
  destinationPort: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.destinationport') }),
      trigger: 'blur'
    }
  ],
  outboundType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.outboundtype') }),
      trigger: 'change'
    }
  ],
  warehouseCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.warehousecode') }),
      trigger: 'blur'
    }
  ],
  locationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.locationcode') }),
      trigger: 'blur'
    }
  ],
  relatedCompany: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productSerialOutbound.relatedcompany') }),
      trigger: 'blur'
    }
  ],
  totalQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productSerialOutbound.totalquantity') }),
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
