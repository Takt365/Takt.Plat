<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output/components -->
<!-- 文件名称：assy-output-detail-form.vue -->
<!-- 功能描述：组立日报子表 assyOutputDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-output-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-output-detail-form-tabs"
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
                :label="t('entity.assyoutputdetail.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodordercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.assyOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.timeperiod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.timeperiod') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.prodactualqty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodactualqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.downtimeminutes')"
                name="downtimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.downtimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.downtimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.downtimereason')"
                name="downtimeReason"
              >
                <TaktSelect
                  v-model:value="formState.downtimeReason"
                  dict-type="logistics_stop_reason_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.downtimereason') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.assyoutputdetail.downtimedescription')"
                name="downtimeDescription"
              >
                <a-textarea
                  v-model:value="formState.downtimeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyoutputdetail.downtimedescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyoutputdetail.unachievedreason')"
                name="unachievedReason"
              >
                <TaktSelect
                  v-model:value="formState.unachievedReason"
                  dict-type="logistics_nonachievement_reason_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.unachievedreason') })"
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
 * 组立日报子表 assyOutputDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/output/assy-output/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssyOutputDetailCreate } from '@/types/logistics/manufacturing/output/assy-output-detail'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["prodOrderCode","lineNumber","timePeriod","prodActualQty","downtimeMinutes","downtimeReason","downtimeDescription","unachievedReason"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyOutputDetailCreate & { assyOutputDetailId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyOutputDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyOutputDetailId) {
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
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.prodordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timePeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyoutputdetail.timeperiod') }),
      trigger: 'blur'
    }
  ],
  prodActualQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.prodactualqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.prodactualqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downtimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.downtimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.assyoutputdetail.downtimeminutes') }))
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

/** 映射为 Create/Update DTO（含主表外键 assyOutputId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('prodActualQty' in payload) {
    const rawprodActualQty = payload.prodActualQty
    payload.prodActualQty = typeof rawprodActualQty === 'number' ? rawprodActualQty : Number(rawprodActualQty)
  }
  if ('downtimeMinutes' in payload) {
    const rawdowntimeMinutes = payload.downtimeMinutes
    payload.downtimeMinutes = typeof rawdowntimeMinutes === 'number' ? rawdowntimeMinutes : Number(rawdowntimeMinutes)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.assyOutputId = props.masterId
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
