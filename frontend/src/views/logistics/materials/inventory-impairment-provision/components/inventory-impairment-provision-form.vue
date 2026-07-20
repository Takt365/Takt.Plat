<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/inventory-impairment-provision/components -->
<!-- 文件名称：inventory-impairment-provision-form.vue -->
<!-- 功能描述：存货跌价准备实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="inventory-impairment-provision-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.inventoryImpairmentProvisionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('periodDate')"
                name="periodDate"
              >
                <a-date-picker
                  v-model:value="formState.periodDate"
                  :placeholder="pi.ph('periodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.inventoryImpairmentProvisionId"
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
                :label="pi.label('provisionScope')"
                name="provisionScope"
              >
                <TaktSelect
                  v-model:value="formState.provisionScope"
                  dict-type="logistics_inventory_provision_scope"
                  :placeholder="pi.ph('provisionScope')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stockQuantity')"
                name="stockQuantity"
              >
                <a-input-number
                  v-model:value="formState.stockQuantity"
                  :placeholder="pi.ph('stockQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitCost')"
                name="unitCost"
              >
                <a-input-number
                  v-model:value="formState.unitCost"
                  :placeholder="pi.ph('unitCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inventoryCost')"
                name="inventoryCost"
              >
                <a-input-number
                  v-model:value="formState.inventoryCost"
                  :placeholder="pi.ph('inventoryCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('estimatedSellingPrice')"
                name="estimatedSellingPrice"
              >
                <a-input-number
                  v-model:value="formState.estimatedSellingPrice"
                  :placeholder="pi.ph('estimatedSellingPrice')"
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
                :label="pi.label('estimatedCompletionCost')"
                name="estimatedCompletionCost"
              >
                <a-input-number
                  v-model:value="formState.estimatedCompletionCost"
                  :placeholder="pi.ph('estimatedCompletionCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('estimatedSellingCost')"
                name="estimatedSellingCost"
              >
                <a-input-number
                  v-model:value="formState.estimatedSellingCost"
                  :placeholder="pi.ph('estimatedSellingCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netRealizableValue')"
                name="netRealizableValue"
              >
                <a-input-number
                  v-model:value="formState.netRealizableValue"
                  :placeholder="pi.ph('netRealizableValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitNetRealizableValue')"
                name="unitNetRealizableValue"
              >
                <a-input-number
                  v-model:value="formState.unitNetRealizableValue"
                  :placeholder="pi.ph('unitNetRealizableValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('openingProvision')"
                name="openingProvision"
              >
                <a-input-number
                  v-model:value="formState.openingProvision"
                  :placeholder="pi.ph('openingProvision')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('provisionAmount')"
                name="provisionAmount"
              >
                <a-input-number
                  v-model:value="formState.provisionAmount"
                  :placeholder="pi.ph('provisionAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reversalAmount')"
                name="reversalAmount"
              >
                <a-input-number
                  v-model:value="formState.reversalAmount"
                  :placeholder="pi.ph('reversalAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingProvision')"
                name="closingProvision"
              >
                <a-input-number
                  v-model:value="formState.closingProvision"
                  :placeholder="pi.ph('closingProvision')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('impairmentLoss')"
                name="impairmentLoss"
              >
                <a-input-number
                  v-model:value="formState.impairmentLoss"
                  :placeholder="pi.ph('impairmentLoss')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('carryingAmount')"
                name="carryingAmount"
              >
                <a-input-number
                  v-model:value="formState.carryingAmount"
                  :placeholder="pi.ph('carryingAmount')"
                  style="width: 100%"
                  disabled
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
                :label="pi.label('currency')"
                name="currency"
              >
                <TaktSelect
                  v-model:value="formState.currency"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('currency')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('impairmentReason')"
                name="impairmentReason"
              >
                <a-input
                  v-model:value="formState.impairmentReason"
                  :placeholder="pi.ph('impairmentReason')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('provisionStatus')"
                name="provisionStatus"
              >
                <TaktSelect
                  v-model:value="formState.provisionStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('provisionStatus')"
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
 * 存货跌价准备实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/inventory-impairment-provision/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useInventoryImpairmentProvisionI18n } from '../composables/use-inventory-impairment-provision-i18n'

/** 实体字段 i18n */
const pi = useInventoryImpairmentProvisionI18n()
import type { InventoryImpairmentProvisionCreate } from '@/types/logistics/materials/inventory-impairment-provision'
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
  formData?: Partial<InventoryImpairmentProvisionCreate & { inventoryImpairmentProvisionId?: string }> | null
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
  provisionScope: 1,
  currency: "CNY"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 inventoryImpairmentProvisionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.inventoryImpairmentProvisionId) {
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
    if (!props.formData?.inventoryImpairmentProvisionId) {
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
  periodDate: [
    {
      required: true,
      message: pi.ph('periodDate'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  valuation: [
    {
      required: true,
      message: pi.ph('valuation'),
      trigger: 'change'
    }
  ],
  provisionScope: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('provisionScope'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('provisionScope'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stockQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stockQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stockQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('unitCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('unitCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inventoryCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inventoryCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inventoryCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  estimatedSellingPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('estimatedSellingPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('estimatedSellingPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  estimatedCompletionCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('estimatedCompletionCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('estimatedCompletionCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  estimatedSellingCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('estimatedSellingCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('estimatedSellingCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  netRealizableValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('netRealizableValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('netRealizableValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitNetRealizableValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('unitNetRealizableValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('unitNetRealizableValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  openingProvision: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('openingProvision'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('openingProvision'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  provisionAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('provisionAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('provisionAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  reversalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('reversalAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('reversalAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  closingProvision: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('closingProvision'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('closingProvision'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  impairmentLoss: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('impairmentLoss'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('impairmentLoss'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  carryingAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('carryingAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('carryingAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currency: [
    {
      required: true,
      message: pi.ph('currency'),
      trigger: 'change'
    }
  ],
  provisionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('provisionStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('provisionStatus'))
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
  if ('provisionScope' in payload) {
    const rawprovisionScope = payload.provisionScope
    payload.provisionScope = typeof rawprovisionScope === 'number' ? rawprovisionScope : Number(rawprovisionScope)
  }
  if ('stockQuantity' in payload) {
    const rawstockQuantity = payload.stockQuantity
    payload.stockQuantity = typeof rawstockQuantity === 'number' ? rawstockQuantity : Number(rawstockQuantity)
  }
  if ('unitCost' in payload) {
    const rawunitCost = payload.unitCost
    payload.unitCost = typeof rawunitCost === 'number' ? rawunitCost : Number(rawunitCost)
  }
  if ('inventoryCost' in payload) {
    const rawinventoryCost = payload.inventoryCost
    payload.inventoryCost = typeof rawinventoryCost === 'number' ? rawinventoryCost : Number(rawinventoryCost)
  }
  if ('estimatedSellingPrice' in payload) {
    const rawestimatedSellingPrice = payload.estimatedSellingPrice
    payload.estimatedSellingPrice = typeof rawestimatedSellingPrice === 'number' ? rawestimatedSellingPrice : Number(rawestimatedSellingPrice)
  }
  if ('estimatedCompletionCost' in payload) {
    const rawestimatedCompletionCost = payload.estimatedCompletionCost
    payload.estimatedCompletionCost = typeof rawestimatedCompletionCost === 'number' ? rawestimatedCompletionCost : Number(rawestimatedCompletionCost)
  }
  if ('estimatedSellingCost' in payload) {
    const rawestimatedSellingCost = payload.estimatedSellingCost
    payload.estimatedSellingCost = typeof rawestimatedSellingCost === 'number' ? rawestimatedSellingCost : Number(rawestimatedSellingCost)
  }
  if ('netRealizableValue' in payload) {
    const rawnetRealizableValue = payload.netRealizableValue
    payload.netRealizableValue = typeof rawnetRealizableValue === 'number' ? rawnetRealizableValue : Number(rawnetRealizableValue)
  }
  if ('unitNetRealizableValue' in payload) {
    const rawunitNetRealizableValue = payload.unitNetRealizableValue
    payload.unitNetRealizableValue = typeof rawunitNetRealizableValue === 'number' ? rawunitNetRealizableValue : Number(rawunitNetRealizableValue)
  }
  if ('openingProvision' in payload) {
    const rawopeningProvision = payload.openingProvision
    payload.openingProvision = typeof rawopeningProvision === 'number' ? rawopeningProvision : Number(rawopeningProvision)
  }
  if ('provisionAmount' in payload) {
    const rawprovisionAmount = payload.provisionAmount
    payload.provisionAmount = typeof rawprovisionAmount === 'number' ? rawprovisionAmount : Number(rawprovisionAmount)
  }
  if ('reversalAmount' in payload) {
    const rawreversalAmount = payload.reversalAmount
    payload.reversalAmount = typeof rawreversalAmount === 'number' ? rawreversalAmount : Number(rawreversalAmount)
  }
  if ('closingProvision' in payload) {
    const rawclosingProvision = payload.closingProvision
    payload.closingProvision = typeof rawclosingProvision === 'number' ? rawclosingProvision : Number(rawclosingProvision)
  }
  if ('impairmentLoss' in payload) {
    const rawimpairmentLoss = payload.impairmentLoss
    payload.impairmentLoss = typeof rawimpairmentLoss === 'number' ? rawimpairmentLoss : Number(rawimpairmentLoss)
  }
  if ('carryingAmount' in payload) {
    const rawcarryingAmount = payload.carryingAmount
    payload.carryingAmount = typeof rawcarryingAmount === 'number' ? rawcarryingAmount : Number(rawcarryingAmount)
  }
  if ('provisionStatus' in payload) {
    const rawprovisionStatus = payload.provisionStatus
    payload.provisionStatus = typeof rawprovisionStatus === 'number' ? rawprovisionStatus : Number(rawprovisionStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.inventoryImpairmentProvisionId)

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
