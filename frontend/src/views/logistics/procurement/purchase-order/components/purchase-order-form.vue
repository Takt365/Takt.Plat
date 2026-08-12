<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-order/components -->
<!-- 文件名称：purchase-order-form.vue -->
<!-- 功能描述：Takt采购订单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-order-form flex flex-col min-h-0 overflow-visible"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
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
      ref="purchaseOrderItemTableRef"
      v-model="childPurchaseOrderItemRows"
      :columns="purchaseOrderItemFormColumns"
      :title="purchaseOrderItemPi.self()"
      :add-button-entity="purchaseOrderItemPi.self()"
      id-field="purchaseOrderItemId"
      :default-row="createDefaultPurchaseOrderItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchaseUnit="{ record }">
        <TaktSelect
          v-model:value="record.purchaseUnit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('purchaseUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchasePerUnit="{ record }">
        <TaktSelect
          v-model:value="record.purchasePerUnit"
          dict-type="logistics_price_unit_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('purchasePerUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-discountRate="{ record }">
        <TaktSelect
          v-model:value="record.discountRate"
          dict-type="logistics_discount_rate_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('discountRate')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-deliveryStatus="{ record }">
        <TaktSelect
          v-model:value="record.deliveryStatus"
          dict-type="logistics_delivery_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('deliveryStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseOrderItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt采购订单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseOrderI18n } from '../composables/use-purchase-order-i18n'

/** 实体字段 i18n */
const pi = usePurchaseOrderI18n()

import type { PurchaseOrderCreate } from '@/types/logistics/procurement/purchase-order'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseOrderCode","purchaseRequestId","purchaseRequestCode","supplierCode","supplierName1","orderDate","requiredArrivalDate","actualArrivalDate","purchaseGroup","totalQuantity","totalAmount","discountAmount","currencyCode","exchangeRate","taxCode","taxRate","taxAmount","actualAmount","receivedQuantity","receivedAmount","paidAmount","paymentMethod","deliveryMethod","deliveryAddress","orderStatus","deliveryStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { usePurchaseOrderItemI18n } from '../composables/use-purchase-order-item-i18n'

const purchaseOrderItemPi = usePurchaseOrderItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseOrderItemRows = ref<Record<string, unknown>[]>([])
const purchaseOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseOrderItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseOrderItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseOrderItemLineNumber(): number {
  const rows = purchaseOrderItemTableRef.value?.getRows?.() ?? childPurchaseOrderItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseOrderItem 可编辑列 */
const purchaseOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: purchaseOrderItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'requestCode',
    title: purchaseOrderItemPi.label('requestCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseOrderItemPi.ph('requestCode'),
  },
  {
    key: 'requestLineNumber',
    title: purchaseOrderItemPi.label('requestLineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: purchaseOrderItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'purchaseUnit',
    title: purchaseOrderItemPi.label('purchaseUnit'),
    width: 140,
  },
  {
    key: 'orderQuantity',
    title: purchaseOrderItemPi.label('orderQuantity'),
    width: 140,
  },
  {
    key: 'receivedQuantity',
    title: purchaseOrderItemPi.label('receivedQuantity'),
    width: 140,
  },
  {
    key: 'purchasePerUnit',
    title: purchaseOrderItemPi.label('purchasePerUnit'),
    width: 140,
  },
  {
    key: 'purchaseUnitPrice',
    title: purchaseOrderItemPi.label('purchaseUnitPrice'),
    width: 140,
  },
  {
    key: 'discountRate',
    title: purchaseOrderItemPi.label('discountRate'),
    width: 140,
  },
  {
    key: 'discountAmount',
    title: purchaseOrderItemPi.label('discountAmount'),
    width: 140,
  },
  {
    key: 'taxIncludedAmount',
    title: purchaseOrderItemPi.label('taxIncludedAmount'),
    width: 140,
  },
  {
    key: 'untaxedAmount',
    title: purchaseOrderItemPi.label('untaxedAmount'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: purchaseOrderItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'purchaseAmount',
    title: purchaseOrderItemPi.label('purchaseAmount'),
    width: 140,
  },
  {
    key: 'deliveryStatus',
    title: purchaseOrderItemPi.label('deliveryStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseOrderItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseOrderCreate & { purchaseOrderId?: string }> | null | undefined) {
  const rows_purchaseOrderItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseOrderItemRows.value = rows_purchaseOrderItem
}

function createDefaultPurchaseOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextPurchaseOrderItemLineNumber(),
    requestCode: '',
    requestLineNumber: 0,
    materialCode: '',
    purchaseUnit: '',
    orderQuantity: 0,
    receivedQuantity: 0,
    purchasePerUnit: 0,
    purchaseUnitPrice: 0,
    discountRate: 0,
    discountAmount: 0,
    taxIncludedAmount: 0,
    untaxedAmount: 0,
    taxAmount: 0,
    purchaseAmount: 0,
    deliveryStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseOrderItemTableRef.value?.getRows?.() ?? childPurchaseOrderItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        purchaseOrderId: masterId,
      }
      if (isUpdate && isPersistedPurchaseOrderItemRow(row)) {
        normalized.purchaseOrderItemId = row.purchaseOrderItemId
      } else {
        delete normalized.purchaseOrderItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseOrderCreate & { purchaseOrderId?: string }> | null
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
  currencyCode: "CNY",
  taxCode: "J2",
  paymentMethod: 0,
  deliveryMethod: 0,
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseOrderId) {
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
    const isCreate = !props.formData?.purchaseOrderId
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
  purchaseOrderCode: [
    {
      required: true,
      message: pi.ph('purchaseOrderCode'),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  supplierName1: [
    {
      required: true,
      message: pi.ph('supplierName1'),
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
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  exchangeRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('exchangeRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('exchangeRate'))
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
  receivedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('receivedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('receivedQuantity'))
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
  paidAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paidAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paidAmount'))
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
  await purchaseOrderItemTableRef.value?.validate?.()
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
  if ('exchangeRate' in payload) {
    const rawexchangeRate = payload.exchangeRate
    payload.exchangeRate = typeof rawexchangeRate === 'number' ? rawexchangeRate : Number(rawexchangeRate)
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    payload.taxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('actualAmount' in payload) {
    const rawactualAmount = payload.actualAmount
    payload.actualAmount = typeof rawactualAmount === 'number' ? rawactualAmount : Number(rawactualAmount)
  }
  if ('receivedQuantity' in payload) {
    const rawreceivedQuantity = payload.receivedQuantity
    payload.receivedQuantity = typeof rawreceivedQuantity === 'number' ? rawreceivedQuantity : Number(rawreceivedQuantity)
  }
  if ('receivedAmount' in payload) {
    const rawreceivedAmount = payload.receivedAmount
    payload.receivedAmount = typeof rawreceivedAmount === 'number' ? rawreceivedAmount : Number(rawreceivedAmount)
  }
  if ('paidAmount' in payload) {
    const rawpaidAmount = payload.paidAmount
    payload.paidAmount = typeof rawpaidAmount === 'number' ? rawpaidAmount : Number(rawpaidAmount)
  }
  if ('paymentMethod' in payload) {
    const rawpaymentMethod = payload.paymentMethod
    payload.paymentMethod = typeof rawpaymentMethod === 'number' ? rawpaymentMethod : Number(rawpaymentMethod)
  }
  if ('deliveryMethod' in payload) {
    const rawdeliveryMethod = payload.deliveryMethod
    payload.deliveryMethod = typeof rawdeliveryMethod === 'number' ? rawdeliveryMethod : Number(rawdeliveryMethod)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseOrderId)
  childPurchaseOrderItemRows.value = []
  purchaseOrderItemTableRef.value?.resetRows?.()
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
