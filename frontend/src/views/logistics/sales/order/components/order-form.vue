<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/order/components -->
<!-- 文件名称：order-form.vue -->
<!-- 功能描述：Takt销售订单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form order-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="order-form-tabs"
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
                  :disabled="!!formData?.salesOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrderCode')"
                name="salesOrderCode"
              >
                <a-input
                  v-model:value="formState.salesOrderCode"
                  :placeholder="pi.ph('salesOrderCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.salesOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.salesOrderId"
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
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderDate')"
                name="orderDate"
              >
                <a-date-picker
                  v-model:value="formState.orderDate"
                  :placeholder="pi.ph('orderDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requiredDeliveryDate')"
                name="requiredDeliveryDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredDeliveryDate"
                  :placeholder="pi.ph('requiredDeliveryDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualDeliveryDate')"
                name="actualDeliveryDate"
              >
                <a-date-picker
                  v-model:value="formState.actualDeliveryDate"
                  :placeholder="pi.ph('actualDeliveryDate')"
                  value-format="YYYY-MM-DD"
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
                :label="pi.label('totalQuantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="pi.ph('totalQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalAmount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="pi.ph('totalAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountAmount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="pi.ph('discountAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualAmount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="pi.ph('actualAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippedQuantity')"
                name="shippedQuantity"
              >
                <a-input-number
                  v-model:value="formState.shippedQuantity"
                  :placeholder="pi.ph('shippedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippedAmount')"
                name="shippedAmount"
              >
                <a-input-number
                  v-model:value="formState.shippedAmount"
                  :placeholder="pi.ph('shippedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivedAmount')"
                name="receivedAmount"
              >
                <a-input-number
                  v-model:value="formState.receivedAmount"
                  :placeholder="pi.ph('receivedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deliveryMethod')"
                name="deliveryMethod"
              >
                <TaktSelect
                  v-model:value="formState.deliveryMethod"
                  dict-type="logistics_delivery_method_type"
                  :placeholder="pi.ph('deliveryMethod')"
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
                :label="pi.label('paymentMethod')"
                name="paymentMethod"
              >
                <TaktSelect
                  v-model:value="formState.paymentMethod"
                  dict-type="accounting_payment_method_type"
                  :placeholder="pi.ph('paymentMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deliveryAddress')"
                name="deliveryAddress"
              >
                <a-textarea
                  v-model:value="formState.deliveryAddress"
                  :placeholder="pi.ph('deliveryAddress')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('orderStatus')"
                name="orderStatus"
              >
                <TaktSelect
                  v-model:value="formState.orderStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('orderStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deliveryStatus')"
                name="deliveryStatus"
              >
                <TaktSelect
                  v-model:value="formState.deliveryStatus"
                  dict-type="logistics_delivery_status"
                  :placeholder="pi.ph('deliveryStatus')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="salesOrderItemTableRef"
      v-model="childSalesOrderItemRows"
      :columns="salesOrderItemFormColumns"
      :title="salesOrderItemPi.self()"
      :add-button-entity="salesOrderItemPi.self()"
      id-field="salesOrderItemId"
      :default-row="createDefaultSalesOrderItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售订单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesOrderI18n } from '../composables/use-order-i18n'

/** 实体字段 i18n */
const pi = useSalesOrderI18n()

import type { SalesOrderCreate } from '@/types/logistics/sales/order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","salesOrderCode","customerCode","customerName","orderDate","requiredDeliveryDate","actualDeliveryDate","salesBy","totalQuantity","totalAmount","discountAmount","taxAmount","actualAmount","shippedQuantity","shippedAmount","receivedAmount","deliveryMethod","paymentMethod","deliveryAddress","orderStatus","deliveryStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useSalesOrderItemI18n } from '../composables/use-order-item-i18n'

const salesOrderItemPi = useSalesOrderItemI18n()

const childSalesOrderItemRows = ref<Record<string, unknown>[]>([])
const salesOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 salesOrderItem 可编辑列 */
const salesOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: salesOrderItemPi.label('lineNumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'materialCode',
    title: salesOrderItemPi.label('materialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: salesOrderItemPi.label('materialName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialSpecification',
    title: salesOrderItemPi.label('materialSpecification'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesOrderItemPi.ph('materialSpecification'),
  },
  {
    key: 'salesUnit',
    title: salesOrderItemPi.label('salesUnit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'orderQuantity',
    title: salesOrderItemPi.label('orderQuantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'shippedQuantity',
    title: salesOrderItemPi.label('shippedQuantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'salesPerUnit',
    title: salesOrderItemPi.label('salesPerUnit'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesOrderCreate & { salesOrderId?: string }> | null | undefined) {
  childSalesOrderItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultSalesOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childSalesOrderItemRows.value.length + 1) * 10,
    materialCode: '',
    materialName: '',
    materialSpecification: '',
    salesUnit: '',
    orderQuantity: 0,
    shippedQuantity: 0,
    salesPerUnit: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesOrderId ?? ''
  return {
    ...formState,
    items: salesOrderItemTableRef.value?.getRows?.() ?? childSalesOrderItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      salesOrderId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesOrderCreate & { salesOrderId?: string }> | null
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
  deliveryMethod: 0,
  paymentMethod: 0,
  orderStatus: 1,
  deliveryStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
    const isCreate = !props.formData?.salesOrderId
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
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  salesOrderCode: [
    {
      required: true,
      message: pi.ph('salesOrderCode'),
      trigger: 'blur'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
  customerName: [
    {
      required: true,
      message: pi.ph('customerName'),
      trigger: 'blur'
    }
  ],
  orderDate: [
    {
      required: true,
      message: pi.ph('orderDate'),
      trigger: 'change'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  discountAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shippedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shippedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shippedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shippedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shippedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shippedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  receivedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('receivedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('receivedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deliveryMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deliveryMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deliveryMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paymentMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  orderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deliveryStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deliveryStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deliveryStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    payload.totalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
  }
  if ('totalAmount' in payload) {
    const rawtotalAmount = payload.totalAmount
    payload.totalAmount = typeof rawtotalAmount === 'number' ? rawtotalAmount : Number(rawtotalAmount)
  }
  if ('discountAmount' in payload) {
    const rawdiscountAmount = payload.discountAmount
    payload.discountAmount = typeof rawdiscountAmount === 'number' ? rawdiscountAmount : Number(rawdiscountAmount)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('actualAmount' in payload) {
    const rawactualAmount = payload.actualAmount
    payload.actualAmount = typeof rawactualAmount === 'number' ? rawactualAmount : Number(rawactualAmount)
  }
  if ('shippedQuantity' in payload) {
    const rawshippedQuantity = payload.shippedQuantity
    payload.shippedQuantity = typeof rawshippedQuantity === 'number' ? rawshippedQuantity : Number(rawshippedQuantity)
  }
  if ('shippedAmount' in payload) {
    const rawshippedAmount = payload.shippedAmount
    payload.shippedAmount = typeof rawshippedAmount === 'number' ? rawshippedAmount : Number(rawshippedAmount)
  }
  if ('receivedAmount' in payload) {
    const rawreceivedAmount = payload.receivedAmount
    payload.receivedAmount = typeof rawreceivedAmount === 'number' ? rawreceivedAmount : Number(rawreceivedAmount)
  }
  if ('deliveryMethod' in payload) {
    const rawdeliveryMethod = payload.deliveryMethod
    payload.deliveryMethod = typeof rawdeliveryMethod === 'number' ? rawdeliveryMethod : Number(rawdeliveryMethod)
  }
  if ('paymentMethod' in payload) {
    const rawpaymentMethod = payload.paymentMethod
    payload.paymentMethod = typeof rawpaymentMethod === 'number' ? rawpaymentMethod : Number(rawpaymentMethod)
  }
  if ('orderStatus' in payload) {
    const raworderStatus = payload.orderStatus
    payload.orderStatus = typeof raworderStatus === 'number' ? raworderStatus : Number(raworderStatus)
  }
  if ('deliveryStatus' in payload) {
    const rawdeliveryStatus = payload.deliveryStatus
    payload.deliveryStatus = typeof rawdeliveryStatus === 'number' ? rawdeliveryStatus : Number(rawdeliveryStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesOrderId)
  childSalesOrderItemRows.value = []
  salesOrderItemTableRef.value?.resetRows?.()
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
