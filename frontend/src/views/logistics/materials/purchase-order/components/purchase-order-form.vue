<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-order/components -->
<!-- 文件名称：purchase-order-form.vue -->
<!-- 功能描述：Takt采购订单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="purchase-order-form-tabs"
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
                :label="t('entity.purchaseOrder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.code')"
                name="purchaseOrderCode"
              >
                <a-input
                  v-model:value="formState.purchaseOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.suppliername')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.orderdate')"
                name="orderDate"
              >
                <a-date-picker
                  v-model:value="formState.orderDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.requiredarrivaldate')"
                name="requiredArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredArrivalDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.requiredarrivaldate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.actualarrivaldate')"
                name="actualArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.actualArrivalDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.actualarrivaldate') })"
                  value-format="YYYY-MM-DD"
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
                :label="t('entity.purchaseOrder.purchasegroup')"
                name="purchaseGroup"
              >
                <a-input
                  v-model:value="formState.purchaseGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.purchasegroup') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.totalquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.totalamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.discountamount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.discountamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.taxamount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.taxamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.actualamount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.actualamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.receivedquantity')"
                name="receivedQuantity"
              >
                <a-input-number
                  v-model:value="formState.receivedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.receivedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.receivedamount')"
                name="receivedAmount"
              >
                <a-input-number
                  v-model:value="formState.receivedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.receivedamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.paidamount')"
                name="paidAmount"
              >
                <a-input-number
                  v-model:value="formState.paidAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.paidamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.orderstatus')"
                name="orderStatus"
              >
                <a-input-number
                  v-model:value="formState.orderStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.orderstatus') })"
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
                :label="t('entity.purchaseOrder.deliverystatus')"
                name="deliveryStatus"
              >
                <a-input-number
                  v-model:value="formState.deliveryStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.deliverystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.paymentmethod')"
                name="paymentMethod"
              >
                <a-input-number
                  v-model:value="formState.paymentMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.paymentmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.deliverymethod')"
                name="deliveryMethod"
              >
                <a-input-number
                  v-model:value="formState.deliveryMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.deliverymethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.purchaseOrder.deliveryaddress')"
                name="deliveryAddress"
              >
                <a-textarea
                  v-model:value="formState.deliveryAddress"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseOrder.deliveryaddress') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.items') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseOrder.changelogs')"
                name="changeLogs"
              >
                <a-input
                  v-model:value="formState.changeLogs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.changelogs') })"
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
 * Takt采购订单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/materials/purchase-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseOrderCreate } from '@/types/logistics/materials/purchase-order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","purchaseOrderCode","supplierCode","supplierName","orderDate","requiredArrivalDate","actualArrivalDate","purchaseGroup","totalQuantity","totalAmount","discountAmount","taxAmount","actualAmount","receivedQuantity","receivedAmount","paidAmount","orderStatus","deliveryStatus","paymentMethod","deliveryMethod","deliveryAddress","items","changeLogs","extFieldJson","remark"]


interface Props {
  formData?: Partial<PurchaseOrderCreate & { purchaseOrderId?: string }> | null
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
    const isCreate = !props.formData?.purchaseOrderId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.plantcode') }),
      trigger: 'blur'
    }
  ],
  purchaseOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.code') }),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliercode') }),
      trigger: 'blur'
    }
  ],
  supplierName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseOrder.suppliername') }),
      trigger: 'blur'
    }
  ],
  orderDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderdate') }),
      trigger: 'change'
    }
  ],
  totalQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.totalquantity') }),
      trigger: 'change'
    }
  ],
  totalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.totalamount') }),
      trigger: 'change'
    }
  ],
  discountAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.discountamount') }),
      trigger: 'change'
    }
  ],
  taxAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.taxamount') }),
      trigger: 'change'
    }
  ],
  actualAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.actualamount') }),
      trigger: 'change'
    }
  ],
  receivedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.receivedquantity') }),
      trigger: 'change'
    }
  ],
  receivedAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.receivedamount') }),
      trigger: 'change'
    }
  ],
  paidAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.paidamount') }),
      trigger: 'change'
    }
  ],
  orderStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.orderstatus') }),
      trigger: 'change'
    }
  ],
  deliveryStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.deliverystatus') }),
      trigger: 'change'
    }
  ],
  paymentMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.paymentmethod') }),
      trigger: 'change'
    }
  ],
  deliveryMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseOrder.deliverymethod') }),
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
