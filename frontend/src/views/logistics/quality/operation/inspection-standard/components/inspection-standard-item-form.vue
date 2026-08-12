<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard/components -->
<!-- 文件名称：inspection-standard-item-form.vue -->
<!-- 功能描述：检验标准实体子表 inspectionStandardItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form inspection-standard-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="inspection-standard-item-form-tabs"
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
                :label="t('entity.inspectionstandarditem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.inspectionStandardItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemname') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.itemtype')"
                name="itemType"
              >
                <TaktSelect
                  v-model:value="formState.itemType"
                  dict-type="logistics_quality_inspection_item_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.itemtype') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.defectlevel')"
                name="defectLevel"
              >
                <TaktSelect
                  v-model:value="formState.defectLevel"
                  dict-type="logistics_quality_defect_severity_code"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.defectlevel') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.inspectionmode')"
                name="inspectionMode"
              >
                <TaktSelect
                  v-model:value="formState.inspectionMode"
                  dict-type="logistics_quality_inspection_mode"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.inspectionmode') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.standardvalue')"
                name="standardValue"
              >
                <a-input
                  v-model:value="formState.standardValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.standardvalue') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionstandarditem.upperlimit')"
                name="upperLimit"
              >
                <a-input
                  v-model:value="formState.upperLimit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.upperlimit') })"
                  show-count
                  :maxlength="20"
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
 * 检验标准实体子表 inspectionStandardItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/operation/inspection-standard/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { InspectionStandardItemCreate } from '@/types/logistics/quality/operation/inspection-standard-item'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","itemCode","itemName","itemType","defectLevel","inspectionMode","standardValue","upperLimit"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<InspectionStandardItemCreate & { inspectionStandardItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 inspectionStandardItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.inspectionStandardItemId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  itemCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.itemname') }),
      trigger: 'blur'
    }
  ],
  itemType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.itemtype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.itemtype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.defectlevel') }),
      trigger: 'blur'
    }
  ],
  inspectionMode: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.inspectionmode') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.inspectionstandarditem.inspectionmode') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.standardvalue') }),
      trigger: 'blur'
    }
  ],
  upperLimit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionstandarditem.upperlimit') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 inspectionStandardId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('itemType' in payload) {
    const rawitemType = payload.itemType
    payload.itemType = typeof rawitemType === 'number' ? rawitemType : Number(rawitemType)
  }
  if ('inspectionMode' in payload) {
    const rawinspectionMode = payload.inspectionMode
    payload.inspectionMode = typeof rawinspectionMode === 'number' ? rawinspectionMode : Number(rawinspectionMode)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.inspectionStandardId = props.masterId
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
