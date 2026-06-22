<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/components -->
<!-- 文件名称：customer-satisfaction-survey-item-form.vue -->
<!-- 功能描述：客户满意度调查表主表实体子表 customerSatisfactionSurveyItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form customer-satisfaction-survey-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-satisfaction-survey-item-form-tabs"
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
                :label="t('entity.customersatisfactionsurveyitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.categorytype')"
                name="categoryType"
              >
                <a-input-number
                  v-model:value="formState.categoryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.categorytype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.itemname') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.itemdescription')"
                name="itemDescription"
              >
                <a-textarea
                  v-model:value="formState.itemDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customersatisfactionsurveyitem.itemdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.weight')"
                name="weight"
              >
                <a-input-number
                  v-model:value="formState.weight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.weight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.score')"
                name="score"
              >
                <a-input-number
                  v-model:value="formState.score"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.score') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.satisfactionlevel')"
                name="satisfactionLevel"
              >
                <a-input-number
                  v-model:value="formState.satisfactionLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.satisfactionlevel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customersatisfactionsurveyitem.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.customerfeedback') })"
                  show-count
                  :maxlength="2000"
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
 * 客户满意度调查表主表实体子表 customerSatisfactionSurveyItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerSatisfactionSurveyItemCreate } from '@/types/logistics/quality/complaint/customer-satisfaction-survey-item'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","categoryType","itemName","itemDescription","weight","score","satisfactionLevel","customerFeedback"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerSatisfactionSurveyItemCreate & { customerSatisfactionSurveyItemId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 customerSatisfactionSurveyItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerSatisfactionSurveyItemId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  categoryType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.categorytype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.categorytype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customersatisfactionsurveyitem.itemname') }),
      trigger: 'blur'
    }
  ],
  weight: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.weight') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customersatisfactionsurveyitem.weight') }))
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

/** 映射为 Create/Update DTO（含主表外键 surveyId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('categoryType' in payload) {
    const rawcategoryType = payload.categoryType
    payload.categoryType = typeof rawcategoryType === 'number' ? rawcategoryType : Number(rawcategoryType)
  }
  if ('weight' in payload) {
    const rawweight = payload.weight
    payload.weight = typeof rawweight === 'number' ? rawweight : Number(rawweight)
  }
  if ('score' in payload) {
    const rawscore = payload.score
    payload.score = typeof rawscore === 'number' ? rawscore : Number(rawscore)
  }
  if ('satisfactionLevel' in payload) {
    const rawsatisfactionLevel = payload.satisfactionLevel
    payload.satisfactionLevel = typeof rawsatisfactionLevel === 'number' ? rawsatisfactionLevel : Number(rawsatisfactionLevel)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.surveyId = props.masterId
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
