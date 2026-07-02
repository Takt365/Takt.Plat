<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-inquiry/components -->
<!-- 文件名称：purchase-inquiry-item-form.vue -->
<!-- 功能描述：采购询价实体子表 purchaseInquiryItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-inquiry-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-inquiry-item-form-tabs"
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
                :label="t('entity.purchaseinquiryitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.allocationcategory')"
                name="allocationCategory"
              >
                <TaktSelect
                  v-model:value="formState.allocationCategory"
                  dict-type="logistics_allocation_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.allocationcategory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseInquiryItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialname') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.materialspecification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialspecification') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.inquiryunit')"
                name="inquiryUnit"
              >
                <a-input
                  v-model:value="formState.inquiryUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.inquiryunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.inquiryquantity')"
                name="inquiryQuantity"
              >
                <a-input-number
                  v-model:value="formState.inquiryQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.inquiryquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.purchaseinquiryitem.quotedunitprice')"
                name="quotedUnitPrice"
              >
                <a-textarea
                  v-model:value="formState.quotedUnitPrice"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.purchaseinquiryitem.quotedunitprice') })"
                  :rows="2"
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
 * 采购询价实体子表 purchaseInquiryItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/procurement/purchase-inquiry/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseInquiryItemCreate } from '@/types/logistics/procurement/purchase-inquiry-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","allocationCategory","materialCode","materialName","materialSpecification","inquiryUnit","inquiryQuantity","quotedUnitPrice"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInquiryItemCreate & { purchaseInquiryItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInquiryItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInquiryItemId) {
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
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  allocationCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.allocationcategory') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.materialname') }),
      trigger: 'blur'
    }
  ],
  inquiryUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.inquiryunit') }),
      trigger: 'blur'
    }
  ],
  inquiryQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.inquiryquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiryitem.inquiryquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  quotedUnitPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.quotedunitprice') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 purchaseInquiryId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('inquiryQuantity' in payload) {
    const rawinquiryQuantity = payload.inquiryQuantity
    payload.inquiryQuantity = typeof rawinquiryQuantity === 'number' ? rawinquiryQuantity : Number(rawinquiryQuantity)
  }
  if ('quotedUnitPrice' in payload) {
    const rawquotedUnitPrice = payload.quotedUnitPrice
    payload.quotedUnitPrice = typeof rawquotedUnitPrice === 'number' ? rawquotedUnitPrice : Number(rawquotedUnitPrice)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.purchaseInquiryId = props.masterId
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
