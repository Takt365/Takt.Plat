<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing/components -->
<!-- 文件名称：routing-item-form.vue -->
<!-- 功能描述：工艺路线主表实体子表 routingItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="routing-item-form-tabs"
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
                :label="t('entity.routingitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.baseunit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') })"
                  show-count
                  :maxlength="5"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.basequantity')"
                name="baseQuantity"
              >
                <a-input-number
                  v-model:value="formState.baseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.basequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.standardminutes')"
                name="standardMinutes"
              >
                <a-input-number
                  v-model:value="formState.standardMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.timeunit')"
                name="timeUnit"
              >
                <a-input
                  v-model:value="formState.timeUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.standardshorts')"
                name="standardShorts"
              >
                <a-input-number
                  v-model:value="formState.standardShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardshorts') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.pointsunit')"
                name="pointsUnit"
              >
                <a-input
                  v-model:value="formState.pointsUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.pointstominutesrate')"
                name="pointsToMinutesRate"
              >
                <TaktSelect
                  v-model:value="formState.pointsToMinutesRate"
                  dict-type="logistics_points_to_minutes_rate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') })"
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
 * 工艺路线主表实体子表 routingItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/routing/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { RoutingItemCreate } from '@/types/logistics/manufacturing/bom/routing-item'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","baseUnit","baseQuantity","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingItemCreate & { routingItemId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 routingItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.routingItemId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') }),
      trigger: 'blur'
    }
  ],
  baseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.basequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.basequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timeUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') }),
      trigger: 'blur'
    }
  ],
  standardShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardshorts') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardshorts') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pointsUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') }),
      trigger: 'blur'
    }
  ],
  pointsToMinutesRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 routingId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('baseQuantity' in payload) {
    const rawbaseQuantity = payload.baseQuantity
    payload.baseQuantity = typeof rawbaseQuantity === 'number' ? rawbaseQuantity : Number(rawbaseQuantity)
  }
  if ('standardMinutes' in payload) {
    const rawstandardMinutes = payload.standardMinutes
    payload.standardMinutes = typeof rawstandardMinutes === 'number' ? rawstandardMinutes : Number(rawstandardMinutes)
  }
  if ('standardShorts' in payload) {
    const rawstandardShorts = payload.standardShorts
    payload.standardShorts = typeof rawstandardShorts === 'number' ? rawstandardShorts : Number(rawstandardShorts)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.routingId = props.masterId
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
