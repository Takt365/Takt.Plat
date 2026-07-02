<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/master-demand-schedule/components -->
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
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.mdscode')"
                name="mdsCode"
              >
                <a-input
                  v-model:value="formState.mdsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.mdscode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterDemandScheduleLineId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.demandsourcetype')"
                name="demandSourceType"
              >
                <TaktSelect
                  v-model:value="formState.demandSourceType"
                  dict-type="mds_demand_source_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.demandsourcetype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.salesorderid')"
                name="salesOrderId"
              >
                <a-input
                  v-model:value="formState.salesOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.salesorderlinenumber')"
                name="salesOrderLineNumber"
              >
                <a-input-number
                  v-model:value="formState.salesOrderLineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesorderlinenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.salesplanid')"
                name="salesPlanId"
              >
                <a-input
                  v-model:value="formState.salesPlanId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesplanid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.salesplanlinenumber')"
                name="salesPlanLineNumber"
              >
                <a-input-number
                  v-model:value="formState.salesPlanLineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.salesplanlinenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.masterDemandScheduleLineId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandscheduleline.bucketstart')"
                name="bucketStart"
              >
                <a-date-picker
                  v-model:value="formState.bucketStart"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.bucketstart') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
 * @module views/logistics/manufacturing/planning/master-demand-schedule/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MasterDemandScheduleLineCreate } from '@/types/logistics/manufacturing/planning/master-demand-schedule-line'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["mdsCode","demandSourceType","salesOrderId","salesOrderLineNumber","salesPlanId","salesPlanLineNumber","materialCode","bucketStart"]


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
      message: t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.mdscode') }),
      trigger: 'blur'
    }
  ],
  demandSourceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.demandsourcetype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.demandsourcetype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.masterdemandscheduleline.materialcode') }),
      trigger: 'blur'
    }
  ],
  bucketStart: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.masterdemandscheduleline.bucketstart') }),
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
  if ('salesPlanLineNumber' in payload) {
    const rawsalesPlanLineNumber = payload.salesPlanLineNumber
    payload.salesPlanLineNumber = typeof rawsalesPlanLineNumber === 'number' ? rawsalesPlanLineNumber : Number(rawsalesPlanLineNumber)
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
