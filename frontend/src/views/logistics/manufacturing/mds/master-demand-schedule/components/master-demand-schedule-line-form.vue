<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mds/master-demand-schedule/components -->
<!-- 文件名称：master-demand-schedule-line-form.vue -->
<!-- 功能描述：主需求计划 MDS 头表子表 masterDemandScheduleLine 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form master-demand-schedule-line-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="master-demand-schedule-line-form-tabs"
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
                :label="pi.label('mdsCode')"
                name="mdsCode"
              >
                <a-input
                  v-model:value="formState.mdsCode"
                  :placeholder="pi.ph('mdsCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterDemandScheduleLineId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('demandSourceType')"
                name="demandSourceType"
              >
                <TaktSelect
                  v-model:value="formState.demandSourceType"
                  dict-type="mds_demand_source_type"
                  :placeholder="pi.ph('demandSourceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrderId')"
                name="salesOrderId"
              >
                <a-input
                  v-model:value="formState.salesOrderId"
                  :placeholder="pi.ph('salesOrderId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrderLineNumber')"
                name="salesOrderLineNumber"
              >
                <a-input-number
                  v-model:value="formState.salesOrderLineNumber"
                  :placeholder="pi.ph('salesOrderLineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesForecastId')"
                name="salesForecastId"
              >
                <a-input
                  v-model:value="formState.salesForecastId"
                  :placeholder="pi.ph('salesForecastId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesForecastLineNumber')"
                name="salesForecastLineNumber"
              >
                <a-input-number
                  v-model:value="formState.salesForecastLineNumber"
                  :placeholder="pi.ph('salesForecastLineNumber')"
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
                  :disabled="!!formData?.masterDemandScheduleLineId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bucketStart')"
                name="bucketStart"
              >
                <a-date-picker
                  v-model:value="formState.bucketStart"
                  :placeholder="pi.ph('bucketStart')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bucketEnd')"
                name="bucketEnd"
              >
                <a-date-picker
                  v-model:value="formState.bucketEnd"
                  :placeholder="pi.ph('bucketEnd')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('demandQuantity')"
                name="demandQuantity"
              >
                <a-input-number
                  v-model:value="formState.demandQuantity"
                  :placeholder="pi.ph('demandQuantity')"
                  style="width: 100%"
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
                :label="pi.label('unitOfMeasure')"
                name="unitOfMeasure"
              >
                <TaktSelect
                  v-model:value="formState.unitOfMeasure"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('unitOfMeasure')"
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
 * 主需求计划 MDS 头表子表 masterDemandScheduleLine 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/mds/master-demand-schedule/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMasterDemandScheduleLineI18n } from '../composables/use-master-demand-schedule-line-i18n'

/** 实体字段 i18n */
const pi = useMasterDemandScheduleLineI18n()

import type { MasterDemandScheduleLineCreate } from '@/types/logistics/manufacturing/mds/master-demand-schedule-line'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["mdsCode","demandSourceType","salesOrderId","salesOrderLineNumber","salesForecastId","salesForecastLineNumber","materialCode","bucketStart","bucketEnd","demandQuantity","unitOfMeasure"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MasterDemandScheduleLineCreate & { masterDemandScheduleLineId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 masterDemandScheduleLineId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.masterDemandScheduleLineId) {
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
  mdsCode: [
    {
      required: true,
      message: pi.ph('mdsCode'),
      trigger: 'blur'
    }
  ],
  demandSourceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('demandSourceType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('demandSourceType'))
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
  bucketStart: [
    {
      required: true,
      message: pi.ph('bucketStart'),
      trigger: 'change'
    }
  ],
  bucketEnd: [
    {
      required: true,
      message: pi.ph('bucketEnd'),
      trigger: 'change'
    }
  ],
  demandQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('demandQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('demandQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitOfMeasure: [
    {
      required: true,
      message: pi.ph('unitOfMeasure'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 masterDemandScheduleId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('demandSourceType' in payload) {
    const rawdemandSourceType = payload.demandSourceType
    payload.demandSourceType = typeof rawdemandSourceType === 'number' ? rawdemandSourceType : Number(rawdemandSourceType)
  }
  if ('salesOrderLineNumber' in payload) {
    const rawsalesOrderLineNumber = payload.salesOrderLineNumber
    payload.salesOrderLineNumber = typeof rawsalesOrderLineNumber === 'number' ? rawsalesOrderLineNumber : Number(rawsalesOrderLineNumber)
  }
  if ('salesForecastLineNumber' in payload) {
    const rawsalesForecastLineNumber = payload.salesForecastLineNumber
    payload.salesForecastLineNumber = typeof rawsalesForecastLineNumber === 'number' ? rawsalesForecastLineNumber : Number(rawsalesForecastLineNumber)
  }
  if ('demandQuantity' in payload) {
    const rawdemandQuantity = payload.demandQuantity
    payload.demandQuantity = typeof rawdemandQuantity === 'number' ? rawdemandQuantity : Number(rawdemandQuantity)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.masterDemandScheduleId = props.masterId
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
