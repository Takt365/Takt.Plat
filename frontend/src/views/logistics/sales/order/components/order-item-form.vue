<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/order/components -->
<!-- 文件名称：order-item-form.vue -->
<!-- 功能描述：Takt销售订单实体子表 salesOrderItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form order-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="order-item-form-tabs"
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
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterials/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.salesOrderItemId"
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
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialSpecification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="pi.ph('materialSpecification')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesUnit')"
                name="salesUnit"
              >
                <TaktSelect
                  v-model:value="formState.salesUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('salesUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderQuantity')"
                name="orderQuantity"
              >
                <a-input-number
                  v-model:value="formState.orderQuantity"
                  :placeholder="pi.ph('orderQuantity')"
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
                :label="pi.label('salesPerUnit')"
                name="salesPerUnit"
              >
                <TaktSelect
                  v-model:value="formState.salesPerUnit"
                  dict-type="logistics_price_unit_param"
                  :placeholder="pi.ph('salesPerUnit')"
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
 * Takt销售订单实体子表 salesOrderItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesOrderItemI18n } from '../composables/use-order-item-i18n'

/** 实体字段 i18n */
const pi = useSalesOrderItemI18n()

import type { SalesOrderItemCreate } from '@/types/logistics/sales/order-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","materialCode","materialName","materialSpecification","salesUnit","orderQuantity","shippedQuantity","salesPerUnit"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesOrderItemCreate & { salesOrderItemId?: string }> | null
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
  salesPerUnit: 1000
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesOrderItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesOrderItemId) {
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
  salesUnit: [
    {
      required: true,
      message: pi.ph('salesUnit'),
      trigger: 'change'
    }
  ],
  orderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderQuantity'))
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
  salesPerUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('salesPerUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('salesPerUnit'))
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

/** 映射为 Create/Update DTO（含主表外键 salesOrderId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('orderQuantity' in payload) {
    const raworderQuantity = payload.orderQuantity
    payload.orderQuantity = typeof raworderQuantity === 'number' ? raworderQuantity : Number(raworderQuantity)
  }
  if ('shippedQuantity' in payload) {
    const rawshippedQuantity = payload.shippedQuantity
    payload.shippedQuantity = typeof rawshippedQuantity === 'number' ? rawshippedQuantity : Number(rawshippedQuantity)
  }
  if ('salesPerUnit' in payload) {
    const rawsalesPerUnit = payload.salesPerUnit
    payload.salesPerUnit = typeof rawsalesPerUnit === 'number' ? rawsalesPerUnit : Number(rawsalesPerUnit)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.salesOrderId = props.masterId
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
