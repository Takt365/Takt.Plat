<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/work-center/components -->
<!-- 文件名称：work-center-resource-form.vue -->
<!-- 功能描述：工作中心子表 workCenterResource 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-center-resource-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="work-center-resource-form-tabs"
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
                :label="pi.label('resourceCode')"
                name="resourceCode"
              >
                <a-input
                  v-model:value="formState.resourceCode"
                  :placeholder="pi.ph('resourceCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.workCenterResourceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resourceName')"
                name="resourceName"
              >
                <a-input
                  v-model:value="formState.resourceName"
                  :placeholder="pi.ph('resourceName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resourceType')"
                name="resourceType"
              >
                <TaktSelect
                  v-model:value="formState.resourceType"
                  dict-type="work_center_resource_type"
                  :placeholder="pi.ph('resourceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parallelCapacity')"
                name="parallelCapacity"
              >
                <a-input-number
                  v-model:value="formState.parallelCapacity"
                  :placeholder="pi.ph('parallelCapacity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('efficiencyRate')"
                name="efficiencyRate"
              >
                <a-input-number
                  v-model:value="formState.efficiencyRate"
                  :placeholder="pi.ph('efficiencyRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resourceStatus')"
                name="resourceStatus"
              >
                <TaktSelect
                  v-model:value="formState.resourceStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('resourceStatus')"
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
 * 工作中心子表 workCenterResource 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/aps/work-center/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useWorkCenterResourceI18n } from '../composables/use-work-center-resource-i18n'

/** 实体字段 i18n */
const pi = useWorkCenterResourceI18n()

import type { WorkCenterResourceCreate } from '@/types/logistics/manufacturing/aps/work-center-resource'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["resourceCode","resourceName","resourceType","parallelCapacity","efficiencyRate","resourceStatus"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<WorkCenterResourceCreate & { workCenterResourceId?: string }> | null
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
  resourceStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 workCenterResourceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.workCenterResourceId) {
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
  resourceCode: [
    {
      required: true,
      message: pi.ph('resourceCode'),
      trigger: 'blur'
    }
  ],
  resourceName: [
    {
      required: true,
      message: pi.ph('resourceName'),
      trigger: 'blur'
    }
  ],
  resourceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('resourceType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('resourceType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  parallelCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  efficiencyRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('efficiencyRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('efficiencyRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  resourceStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('resourceStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('resourceStatus'))
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

/** 映射为 Create/Update DTO（含主表外键 workCenterId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('resourceType' in payload) {
    const rawresourceType = payload.resourceType
    payload.resourceType = typeof rawresourceType === 'number' ? rawresourceType : Number(rawresourceType)
  }
  if ('parallelCapacity' in payload) {
    const rawparallelCapacity = payload.parallelCapacity
    payload.parallelCapacity = typeof rawparallelCapacity === 'number' ? rawparallelCapacity : Number(rawparallelCapacity)
  }
  if ('efficiencyRate' in payload) {
    const rawefficiencyRate = payload.efficiencyRate
    payload.efficiencyRate = typeof rawefficiencyRate === 'number' ? rawefficiencyRate : Number(rawefficiencyRate)
  }
  if ('resourceStatus' in payload) {
    const rawresourceStatus = payload.resourceStatus
    payload.resourceStatus = typeof rawresourceStatus === 'number' ? rawresourceStatus : Number(rawresourceStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.workCenterId = props.masterId
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
