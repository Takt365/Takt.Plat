<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-order-item/components -->
<!-- 文件名称：purchase-order-item-form.vue -->
<!-- 功能描述：Takt采购订单明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="purchase-order-item-form-tabs"
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
                :label="t('entity.purchaseOrderItem.purchaseorderid')"
                name="purchaseOrderId"
              >
                <a-input
                  v-model:value="formState.purchaseOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseorderid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.purchaseordercode')"
                name="purchaseOrderCode"
              >
                <a-input
                  v-model:value="formState.purchaseOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.requestcode')"
                name="requestCode"
              >
                <a-input
                  v-model:value="formState.requestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.requestcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.requestlinenumber')"
                name="requestLineNumber"
              >
                <a-input-number
                  v-model:value="formState.requestLineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.requestlinenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialname') })"
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
                :label="t('entity.purchaseOrderItem.materialspecification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialspecification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.purchaseunit')"
                name="purchaseUnit"
              >
                <a-input
                  v-model:value="formState.purchaseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.orderquantity')"
                name="orderQuantity"
              >
                <a-input-number
                  v-model:value="formState.orderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.orderquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.receivedquantity')"
                name="receivedQuantity"
              >
                <a-input-number
                  v-model:value="formState.receivedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.receivedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.unitprice')"
                name="unitPrice"
              >
                <a-input-number
                  v-model:value="formState.unitPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.unitprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.discountrate')"
                name="discountRate"
              >
                <a-input-number
                  v-model:value="formState.discountRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.discountrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.discountamount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.discountamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.taxrate')"
                name="taxRate"
              >
                <a-input-number
                  v-model:value="formState.taxRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.taxrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.taxamount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.taxamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrderItem.subtotalamount')"
                name="subtotalAmount"
              >
                <a-input-number
                  v-model:value="formState.subtotalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.subtotalamount') })"
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
                :label="t('entity.purchaseOrderItem.deliverystatus')"
                name="deliveryStatus"
              >
                <a-input-number
                  v-model:value="formState.deliveryStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.deliverystatus') })"
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
 * Takt采购订单明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchase-order-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseOrderItemCreate } from '@/types/logistics/materials/purchase-order-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","purchaseOrderId","purchaseOrderCode","lineNumber","requestCode","requestLineNumber","materialCode","materialName","materialSpecification","purchaseUnit","orderQuantity","receivedQuantity","unitPrice","discountRate","discountAmount","taxRate","taxAmount","subtotalAmount","deliveryStatus","extFieldJson","remark"]


interface Props {
  formData?: Partial<PurchaseOrderItemCreate & { purchaseOrderItemId?: string }> | null
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
    const isCreate = !props.formData?.purchaseOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  purchaseOrderId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseorderid') }),
      trigger: 'blur'
    }
  ],
  purchaseOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.materialname') }),
      trigger: 'blur'
    }
  ],
  purchaseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrderItem.purchaseunit') }),
      trigger: 'blur'
    }
  ],
  orderQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.orderquantity') }),
      trigger: 'change'
    }
  ],
  receivedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.receivedquantity') }),
      trigger: 'change'
    }
  ],
  unitPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.unitprice') }),
      trigger: 'change'
    }
  ],
  discountRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.discountrate') }),
      trigger: 'change'
    }
  ],
  discountAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.discountamount') }),
      trigger: 'change'
    }
  ],
  taxRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.taxrate') }),
      trigger: 'change'
    }
  ],
  taxAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.taxamount') }),
      trigger: 'change'
    }
  ],
  subtotalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.subtotalamount') }),
      trigger: 'change'
    }
  ],
  deliveryStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrderItem.deliverystatus') }),
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
