<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/components -->
<!-- 文件名称：schedule-item-form.vue -->
<!-- 功能描述：APS排程主表子表 apsScheduleItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form schedule-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="schedule-item-form-tabs"
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
                :label="pi.label('apsOrderId')"
                name="apsOrderId"
              >
                <TaktSelect
                  v-model:value="formState.apsOrderId"
                  api-url="TaktApsOrders/options"
                  :placeholder="pi.ph('apsOrderId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('apsOperationId')"
                name="apsOperationId"
              >
                <TaktSelect
                  v-model:value="formState.apsOperationId"
                  api-url="TaktApsOperations/options"
                  :placeholder="pi.ph('apsOperationId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingItemId')"
                name="routingItemId"
              >
                <TaktSelect
                  v-model:value="formState.routingItemId"
                  api-url="TaktRoutingItems/options"
                  :placeholder="pi.ph('routingItemId')"
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
                :label="pi.label('workOrderCode')"
                name="workOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.workOrderCode"
                  api-url="TaktProductionOrders/options"
                  :placeholder="pi.ph('workOrderCode')"
                  :disabled="!!formData?.apsScheduleItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productCode')"
                name="productCode"
              >
                <TaktSelect
                  v-model:value="formState.productCode"
                  api-url="TaktMaterials/options"
                  :placeholder="pi.ph('productCode')"
                  :disabled="!!formData?.apsScheduleItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productName')"
                name="productName"
              >
                <a-input
                  v-model:value="formState.productName"
                  :placeholder="pi.ph('productName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenterCode')"
                name="workCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.workCenterCode"
                  api-url="TaktWorkCenters/options"
                  :placeholder="pi.ph('workCenterCode')"
                  :disabled="!!formData?.apsScheduleItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenterName')"
                name="workCenterName"
              >
                <a-input
                  v-model:value="formState.workCenterName"
                  :placeholder="pi.ph('workCenterName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processCode')"
                name="processCode"
              >
                <a-input
                  v-model:value="formState.processCode"
                  :placeholder="pi.ph('processCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.apsScheduleItemId"
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
                :label="pi.label('processName')"
                name="processName"
              >
                <a-input
                  v-model:value="formState.processName"
                  :placeholder="pi.ph('processName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processSequence')"
                name="processSequence"
              >
                <a-input-number
                  v-model:value="formState.processSequence"
                  :placeholder="pi.ph('processSequence')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processStandardST')"
                name="processStandardST"
              >
                <a-input-number
                  v-model:value="formState.processStandardST"
                  :placeholder="pi.ph('processStandardST')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processStandardSTUnit')"
                name="processStandardSTUnit"
              >
                <a-input-number
                  v-model:value="formState.processStandardSTUnit"
                  :placeholder="pi.ph('processStandardSTUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('extraMinutes')"
                name="extraMinutes"
              >
                <a-input-number
                  v-model:value="formState.extraMinutes"
                  :placeholder="pi.ph('extraMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planQuantity')"
                name="planQuantity"
              >
                <a-input-number
                  v-model:value="formState.planQuantity"
                  :placeholder="pi.ph('planQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planStartTime')"
                name="planStartTime"
              >
                <a-date-picker
                  v-model:value="formState.planStartTime"
                  :placeholder="pi.ph('planStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planEndTime')"
                name="planEndTime"
              >
                <a-date-picker
                  v-model:value="formState.planEndTime"
                  :placeholder="pi.ph('planEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualStartTime')"
                name="actualStartTime"
              >
                <a-date-picker
                  v-model:value="formState.actualStartTime"
                  :placeholder="pi.ph('actualStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualEndTime')"
                name="actualEndTime"
              >
                <a-date-picker
                  v-model:value="formState.actualEndTime"
                  :placeholder="pi.ph('actualEndTime')"
                  value-format="YYYY-MM-DD"
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
                :label="pi.label('processStatus')"
                name="processStatus"
              >
                <a-input-number
                  v-model:value="formState.processStatus"
                  :placeholder="pi.ph('processStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="pi.ph('priority')"
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
 * APS排程主表子表 apsScheduleItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/aps/aps-schedule/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useApsScheduleItemI18n } from '../composables/use-schedule-item-i18n'

/** 实体字段 i18n */
const pi = useApsScheduleItemI18n()

import type { ApsScheduleItemCreate } from '@/types/logistics/manufacturing/aps/schedule-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["apsOrderId","apsOperationId","routingItemId","lineNumber","workOrderCode","productCode","productName","workCenterCode","workCenterName","processCode","processName","processSequence","processStandardST","processStandardSTUnit","extraMinutes","planQuantity","planStartTime","planEndTime","actualStartTime","actualEndTime","processStatus","priority","isObsolete"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsScheduleItemCreate & { apsScheduleItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 apsScheduleItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsScheduleItemId) {
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
  workOrderCode: [
    {
      required: true,
      message: pi.ph('workOrderCode'),
      trigger: 'change'
    }
  ],
  productCode: [
    {
      required: true,
      message: pi.ph('productCode'),
      trigger: 'change'
    }
  ],
  productName: [
    {
      required: true,
      message: pi.ph('productName'),
      trigger: 'blur'
    }
  ],
  processCode: [
    {
      required: true,
      message: pi.ph('processCode'),
      trigger: 'blur'
    }
  ],
  processName: [
    {
      required: true,
      message: pi.ph('processName'),
      trigger: 'blur'
    }
  ],
  processSequence: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processSequence'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processSequence'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  processStandardST: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processStandardST'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processStandardST'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  processStandardSTUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processStandardSTUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processStandardSTUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  extraMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('extraMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('extraMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  planQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('planQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('planQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  planStartTime: [
    {
      required: true,
      message: pi.ph('planStartTime'),
      trigger: 'change'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: pi.ph('planEndTime'),
      trigger: 'change'
    }
  ],
  processStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priority'))
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

/** 映射为 Create/Update DTO（含主表外键 apsScheduleId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('processSequence' in payload) {
    const rawprocessSequence = payload.processSequence
    payload.processSequence = typeof rawprocessSequence === 'number' ? rawprocessSequence : Number(rawprocessSequence)
  }
  if ('processStandardST' in payload) {
    const rawprocessStandardST = payload.processStandardST
    payload.processStandardST = typeof rawprocessStandardST === 'number' ? rawprocessStandardST : Number(rawprocessStandardST)
  }
  if ('processStandardSTUnit' in payload) {
    const rawprocessStandardSTUnit = payload.processStandardSTUnit
    payload.processStandardSTUnit = typeof rawprocessStandardSTUnit === 'number' ? rawprocessStandardSTUnit : Number(rawprocessStandardSTUnit)
  }
  if ('extraMinutes' in payload) {
    const rawextraMinutes = payload.extraMinutes
    payload.extraMinutes = typeof rawextraMinutes === 'number' ? rawextraMinutes : Number(rawextraMinutes)
  }
  if ('planQuantity' in payload) {
    const rawplanQuantity = payload.planQuantity
    payload.planQuantity = typeof rawplanQuantity === 'number' ? rawplanQuantity : Number(rawplanQuantity)
  }
  if ('processStatus' in payload) {
    const rawprocessStatus = payload.processStatus
    payload.processStatus = typeof rawprocessStatus === 'number' ? rawprocessStatus : Number(rawprocessStatus)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.apsScheduleId = props.masterId
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
