<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-item-form.vue -->
<!-- 功能描述：Takt销售发票实体子表 salesInvoiceItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form invoice-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="invoice-item-form-tabs"
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
                :label="pi.label('accountingDocumentCode')"
                name="accountingDocumentCode"
              >
                <a-input
                  v-model:value="formState.accountingDocumentCode"
                  :placeholder="pi.ph('accountingDocumentCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postingDate')"
                name="postingDate"
              >
                <a-date-picker
                  v-model:value="formState.postingDate"
                  :placeholder="pi.ph('postingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelName')"
                name="modelName"
              >
                <a-input
                  v-model:value="formState.modelName"
                  :placeholder="pi.ph('modelName')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialType')"
                name="materialType"
              >
                <TaktSelect
                  v-model:value="formState.materialType"
                  dict-type="logistics_material_type"
                  :placeholder="pi.ph('materialType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('profitCenterCode')"
                name="profitCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.profitCenterCode"
                  api-url="TaktProfitCenters/options"
                  :placeholder="pi.ph('profitCenterCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitle')"
                name="accountTitle"
              >
                <TaktSelect
                  v-model:value="formState.accountTitle"
                  api-url="TaktAccountTitles/options"
                  :placeholder="pi.ph('accountTitle')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quantity')"
                name="quantity"
              >
                <a-input-number
                  v-model:value="formState.quantity"
                  :placeholder="pi.ph('quantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unit')"
                name="unit"
              >
                <TaktSelect
                  v-model:value="formState.unit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('unit')"
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
                :label="pi.label('localCurrencyAmount')"
                name="localCurrencyAmount"
              >
                <a-input-number
                  v-model:value="formState.localCurrencyAmount"
                  :placeholder="pi.ph('localCurrencyAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('transactionCurrencyAmount')"
                name="transactionCurrencyAmount"
              >
                <a-input-number
                  v-model:value="formState.transactionCurrencyAmount"
                  :placeholder="pi.ph('transactionCurrencyAmount')"
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
                :label="pi.label('documentType')"
                name="documentType"
              >
                <TaktSelect
                  v-model:value="formState.documentType"
                  dict-type="logistics_accounting_document_type"
                  :placeholder="pi.ph('documentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCode')"
                name="referenceDocumentCode"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCode"
                  :placeholder="pi.ph('referenceDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentItem')"
                name="referenceDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.referenceDocumentItem"
                  :placeholder="pi.ph('referenceDocumentItem')"
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
 * Takt销售发票实体子表 salesInvoiceItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesInvoiceItemI18n } from '../composables/use-invoice-item-i18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceItemI18n()

import type { SalesInvoiceItemCreate } from '@/types/logistics/sales/invoice-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["accountingDocumentCode","lineNumber","postingDate","modelName","materialCode","materialType","profitCenterCode","accountTitle","quantity","unit","localCurrencyAmount","transactionCurrencyAmount","taxIncludedPrice","untaxedPrice","taxAmount","documentType","referenceDocumentCode","referenceDocumentItem","isObsolete"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceItemCreate & { salesInvoiceItemId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  materialType: "ROH",
  documentType: "RV"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesInvoiceItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesInvoiceItemId) {
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
  accountingDocumentCode: [
    {
      required: true,
      message: pi.ph('accountingDocumentCode'),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  postingDate: [
    {
      required: true,
      message: pi.ph('postingDate'),
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
  materialType: [
    {
      required: true,
      message: pi.ph('materialType'),
      trigger: 'change'
    }
  ],
  quantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('quantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('quantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unit: [
    {
      required: true,
      message: pi.ph('unit'),
      trigger: 'change'
    }
  ],
  localCurrencyAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('localCurrencyAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('localCurrencyAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  transactionCurrencyAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('transactionCurrencyAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('transactionCurrencyAmount'))
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
  documentType: [
    {
      required: true,
      message: pi.ph('documentType'),
      trigger: 'change'
    }
  ],
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

/** 映射为 Create/Update DTO（含主表外键 salesInvoiceId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('quantity' in payload) {
    const rawquantity = payload.quantity
    payload.quantity = typeof rawquantity === 'number' ? rawquantity : Number(rawquantity)
  }
  if ('localCurrencyAmount' in payload) {
    const rawlocalCurrencyAmount = payload.localCurrencyAmount
    payload.localCurrencyAmount = typeof rawlocalCurrencyAmount === 'number' ? rawlocalCurrencyAmount : Number(rawlocalCurrencyAmount)
  }
  if ('transactionCurrencyAmount' in payload) {
    const rawtransactionCurrencyAmount = payload.transactionCurrencyAmount
    payload.transactionCurrencyAmount = typeof rawtransactionCurrencyAmount === 'number' ? rawtransactionCurrencyAmount : Number(rawtransactionCurrencyAmount)
  }
  if ('taxIncludedPrice' in payload) {
    const rawtaxIncludedPrice = payload.taxIncludedPrice
    payload.taxIncludedPrice = typeof rawtaxIncludedPrice === 'number' ? rawtaxIncludedPrice : Number(rawtaxIncludedPrice)
  }
  if ('untaxedPrice' in payload) {
    const rawuntaxedPrice = payload.untaxedPrice
    payload.untaxedPrice = typeof rawuntaxedPrice === 'number' ? rawuntaxedPrice : Number(rawuntaxedPrice)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('referenceDocumentItem' in payload) {
    const rawreferenceDocumentItem = payload.referenceDocumentItem
    payload.referenceDocumentItem = typeof rawreferenceDocumentItem === 'number' ? rawreferenceDocumentItem : Number(rawreferenceDocumentItem)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.salesInvoiceId = props.masterId
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
