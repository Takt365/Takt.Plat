<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/purchase-sales-inventory/components -->
<!-- 文件名称：purchase-sales-inventory-form.vue -->
<!-- 功能描述：进销存表实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="purchase-sales-inventory-form-tabs"
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
                :label="pi.label('relatedPlant')"
                name="relatedPlant"
              >
                <TaktSelect
                  v-model:value="formState.relatedPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('relatedPlant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('periodCode')"
                name="periodCode"
              >
                <a-input
                  v-model:value="formState.periodCode"
                  :placeholder="pi.ph('periodCode')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  :disabled="!!formData?.purchaseSalesInventoryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterials/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.purchaseSalesInventoryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialName')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="pi.ph('materialName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valuation')"
                name="valuation"
              >
                <TaktSelect
                  v-model:value="formState.valuation"
                  dict-type="logistics_valuation_class_category"
                  :placeholder="pi.ph('valuation')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitCode')"
                name="unitCode"
              >
                <a-input
                  v-model:value="formState.unitCode"
                  :placeholder="pi.ph('unitCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.purchaseSalesInventoryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('openingQty')"
                name="openingQty"
              >
                <a-input-number
                  v-model:value="formState.openingQty"
                  :placeholder="pi.ph('openingQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('openingAmount')"
                name="openingAmount"
              >
                <a-input-number
                  v-model:value="formState.openingAmount"
                  :placeholder="pi.ph('openingAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseQty')"
                name="purchaseQty"
              >
                <a-input-number
                  v-model:value="formState.purchaseQty"
                  :placeholder="pi.ph('purchaseQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseAmount')"
                name="purchaseAmount"
              >
                <a-input-number
                  v-model:value="formState.purchaseAmount"
                  :placeholder="pi.ph('purchaseAmount')"
                  style="width: 100%"
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
                :label="pi.label('productionQty')"
                name="productionQty"
              >
                <a-input-number
                  v-model:value="formState.productionQty"
                  :placeholder="pi.ph('productionQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionAmount')"
                name="productionAmount"
              >
                <a-input-number
                  v-model:value="formState.productionAmount"
                  :placeholder="pi.ph('productionAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('issueQty')"
                name="issueQty"
              >
                <a-input-number
                  v-model:value="formState.issueQty"
                  :placeholder="pi.ph('issueQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('issueCostAmount')"
                name="issueCostAmount"
              >
                <a-input-number
                  v-model:value="formState.issueCostAmount"
                  :placeholder="pi.ph('issueCostAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesRevenueAmount')"
                name="salesRevenueAmount"
              >
                <a-input-number
                  v-model:value="formState.salesRevenueAmount"
                  :placeholder="pi.ph('salesRevenueAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('adjustQty')"
                name="adjustQty"
              >
                <a-input-number
                  v-model:value="formState.adjustQty"
                  :placeholder="pi.ph('adjustQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('adjustAmount')"
                name="adjustAmount"
              >
                <a-input-number
                  v-model:value="formState.adjustAmount"
                  :placeholder="pi.ph('adjustAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingQty')"
                name="closingQty"
              >
                <a-input-number
                  v-model:value="formState.closingQty"
                  :placeholder="pi.ph('closingQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingAmount')"
                name="closingAmount"
              >
                <a-input-number
                  v-model:value="formState.closingAmount"
                  :placeholder="pi.ph('closingAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingUnitCost')"
                name="closingUnitCost"
              >
                <a-input-number
                  v-model:value="formState.closingUnitCost"
                  :placeholder="pi.ph('closingUnitCost')"
                  style="width: 100%"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.purchaseSalesInventoryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('psiStatus')"
                name="psiStatus"
              >
                <TaktSelect
                  v-model:value="formState.psiStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('psiStatus')"
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
            <a-col :span="24">
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
            <a-col :span="24">
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 进销存表实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/purchase-sales-inventory/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseSalesInventoryI18n } from '../composables/use-purchase-sales-inventory-i18n'

/** 实体字段 i18n */
const pi = usePurchaseSalesInventoryI18n()
import type { PurchaseSalesInventoryCreate } from '@/types/accounting/financial/purchase-sales-inventory'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.companyDefaultCulture) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseSalesInventoryCreate & { purchaseSalesInventoryId?: string }> | null
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
  currencyCode: "CNY"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseSalesInventoryId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseSalesInventoryId) {
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
    if (!props.formData?.purchaseSalesInventoryId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  relatedPlant: [
    {
      required: true,
      message: pi.ph('relatedPlant'),
      trigger: 'change'
    }
  ],
  periodCode: [
    {
      required: true,
      message: pi.ph('periodCode'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  materialName: [
    {
      required: true,
      message: pi.ph('materialName'),
      trigger: 'blur'
    }
  ],
  valuation: [
    {
      required: true,
      message: pi.ph('valuation'),
      trigger: 'change'
    }
  ],
  openingQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('openingQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('openingQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  openingAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('openingAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('openingAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productionQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('productionQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('productionQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productionAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('productionAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('productionAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issueQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('issueQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('issueQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issueCostAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('issueCostAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('issueCostAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  salesRevenueAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('salesRevenueAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('salesRevenueAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  adjustQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('adjustQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('adjustQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  adjustAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('adjustAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('adjustAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  closingQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('closingQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('closingQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  closingAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('closingAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('closingAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  closingUnitCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('closingUnitCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('closingUnitCost'))
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
  psiStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('psiStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('psiStatus'))
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
  if ('openingQty' in payload) {
    const rawopeningQty = payload.openingQty
    payload.openingQty = typeof rawopeningQty === 'number' ? rawopeningQty : Number(rawopeningQty)
  }
  if ('openingAmount' in payload) {
    const rawopeningAmount = payload.openingAmount
    payload.openingAmount = typeof rawopeningAmount === 'number' ? rawopeningAmount : Number(rawopeningAmount)
  }
  if ('purchaseQty' in payload) {
    const rawpurchaseQty = payload.purchaseQty
    payload.purchaseQty = typeof rawpurchaseQty === 'number' ? rawpurchaseQty : Number(rawpurchaseQty)
  }
  if ('purchaseAmount' in payload) {
    const rawpurchaseAmount = payload.purchaseAmount
    payload.purchaseAmount = typeof rawpurchaseAmount === 'number' ? rawpurchaseAmount : Number(rawpurchaseAmount)
  }
  if ('productionQty' in payload) {
    const rawproductionQty = payload.productionQty
    payload.productionQty = typeof rawproductionQty === 'number' ? rawproductionQty : Number(rawproductionQty)
  }
  if ('productionAmount' in payload) {
    const rawproductionAmount = payload.productionAmount
    payload.productionAmount = typeof rawproductionAmount === 'number' ? rawproductionAmount : Number(rawproductionAmount)
  }
  if ('issueQty' in payload) {
    const rawissueQty = payload.issueQty
    payload.issueQty = typeof rawissueQty === 'number' ? rawissueQty : Number(rawissueQty)
  }
  if ('issueCostAmount' in payload) {
    const rawissueCostAmount = payload.issueCostAmount
    payload.issueCostAmount = typeof rawissueCostAmount === 'number' ? rawissueCostAmount : Number(rawissueCostAmount)
  }
  if ('salesRevenueAmount' in payload) {
    const rawsalesRevenueAmount = payload.salesRevenueAmount
    payload.salesRevenueAmount = typeof rawsalesRevenueAmount === 'number' ? rawsalesRevenueAmount : Number(rawsalesRevenueAmount)
  }
  if ('adjustQty' in payload) {
    const rawadjustQty = payload.adjustQty
    payload.adjustQty = typeof rawadjustQty === 'number' ? rawadjustQty : Number(rawadjustQty)
  }
  if ('adjustAmount' in payload) {
    const rawadjustAmount = payload.adjustAmount
    payload.adjustAmount = typeof rawadjustAmount === 'number' ? rawadjustAmount : Number(rawadjustAmount)
  }
  if ('closingQty' in payload) {
    const rawclosingQty = payload.closingQty
    payload.closingQty = typeof rawclosingQty === 'number' ? rawclosingQty : Number(rawclosingQty)
  }
  if ('closingAmount' in payload) {
    const rawclosingAmount = payload.closingAmount
    payload.closingAmount = typeof rawclosingAmount === 'number' ? rawclosingAmount : Number(rawclosingAmount)
  }
  if ('closingUnitCost' in payload) {
    const rawclosingUnitCost = payload.closingUnitCost
    payload.closingUnitCost = typeof rawclosingUnitCost === 'number' ? rawclosingUnitCost : Number(rawclosingUnitCost)
  }
  if ('psiStatus' in payload) {
    const rawpsiStatus = payload.psiStatus
    payload.psiStatus = typeof rawpsiStatus === 'number' ? rawpsiStatus : Number(rawpsiStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseSalesInventoryId)

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
