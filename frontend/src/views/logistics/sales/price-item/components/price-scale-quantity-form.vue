<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-item/components -->
<!-- 文件名称：price-scale-quantity-form.vue -->
<!-- 功能描述：Takt销售价格明细实体子表 salesPriceScaleQuantity 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form price-scale-quantity-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="price-scale-quantity-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesPriceCode')"
                name="salesPriceCode"
              >
                <a-input
                  v-model:value="formState.salesPriceCode"
                  :placeholder="pi.ph('salesPriceCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesPriceScaleQuantityId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesPriceSeq')"
                name="salesPriceSeq"
              >
                <a-input-number
                  v-model:value="formState.salesPriceSeq"
                  :placeholder="pi.ph('salesPriceSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesScaleSeq')"
                name="salesScaleSeq"
              >
                <a-input-number
                  v-model:value="formState.salesScaleSeq"
                  :placeholder="pi.ph('salesScaleSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantity')"
                name="scaleQuantity"
              >
                <a-input-number
                  v-model:value="formState.scaleQuantity"
                  :placeholder="pi.ph('scaleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('price')"
                name="price"
              >
                <a-input-number
                  v-model:value="formState.price"
                  :placeholder="pi.ph('price')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('untaxedPrice')"
                name="untaxedPrice"
              >
                <a-input-number
                  v-model:value="formState.untaxedPrice"
                  :placeholder="pi.ph('untaxedPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxIncludedPrice')"
                name="taxIncludedPrice"
              >
                <a-input-number
                  v-model:value="formState.taxIncludedPrice"
                  :placeholder="pi.ph('taxIncludedPrice')"
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
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isObsolete')"
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
 * Takt销售价格明细实体子表 salesPriceScaleQuantity 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/price-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesPriceScaleQuantityI18n } from '../composables/use-price-scale-quantity-i18n'

/** 实体字段 i18n */
const pi = useSalesPriceScaleQuantityI18n()

import type { SalesPriceScaleQuantityCreate } from '@/types/logistics/sales/price-scale-quantity'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["salesPriceCode","salesPriceSeq","salesScaleSeq","scaleQuantity","price","untaxedPrice","taxIncludedPrice","taxAmount","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesPriceScaleQuantityCreate & { salesPriceScaleQuantityId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesPriceScaleQuantityId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesPriceScaleQuantityId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  salesPriceCode: [
    {
      required: true,
      message: pi.ph('salesPriceCode'),
      trigger: 'blur'
    }
  ],
  salesPriceSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('salesPriceSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('salesPriceSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  salesScaleSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('salesScaleSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('salesScaleSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scaleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  price: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('price'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('price'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  untaxedPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('untaxedPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('untaxedPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxIncludedPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxIncludedPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxIncludedPrice'))
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
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
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

/** 映射为 Create/Update DTO（含主表外键 salesPriceItemId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('salesPriceSeq' in payload) {
    const rawsalesPriceSeq = payload.salesPriceSeq
    payload.salesPriceSeq = typeof rawsalesPriceSeq === 'number' ? rawsalesPriceSeq : Number(rawsalesPriceSeq)
  }
  if ('salesScaleSeq' in payload) {
    const rawsalesScaleSeq = payload.salesScaleSeq
    payload.salesScaleSeq = typeof rawsalesScaleSeq === 'number' ? rawsalesScaleSeq : Number(rawsalesScaleSeq)
  }
  if ('scaleQuantity' in payload) {
    const rawscaleQuantity = payload.scaleQuantity
    payload.scaleQuantity = typeof rawscaleQuantity === 'number' ? rawscaleQuantity : Number(rawscaleQuantity)
  }
  if ('price' in payload) {
    const rawprice = payload.price
    payload.price = typeof rawprice === 'number' ? rawprice : Number(rawprice)
  }
  if ('untaxedPrice' in payload) {
    const rawuntaxedPrice = payload.untaxedPrice
    payload.untaxedPrice = typeof rawuntaxedPrice === 'number' ? rawuntaxedPrice : Number(rawuntaxedPrice)
  }
  if ('taxIncludedPrice' in payload) {
    const rawtaxIncludedPrice = payload.taxIncludedPrice
    payload.taxIncludedPrice = typeof rawtaxIncludedPrice === 'number' ? rawtaxIncludedPrice : Number(rawtaxIncludedPrice)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.salesPriceItemId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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
