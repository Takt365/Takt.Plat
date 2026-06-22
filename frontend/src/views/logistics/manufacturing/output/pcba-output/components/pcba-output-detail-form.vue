<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-detail-form.vue -->
<!-- 功能描述：PCBA日报实体子表 pcbaOutputDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-detail-form-tabs"
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
                :label="t('entity.pcbaoutputdetail.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.prodordercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.timeperiod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.timeperiod') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.shiftno') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.pcbboardtype')"
                name="pcbBoardType"
              >
                <a-input
                  v-model:value="formState.pcbBoardType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.pcbboardtype') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.panelside')"
                name="panelSide"
              >
                <a-input
                  v-model:value="formState.panelSide"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.panelside') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.batchqty')"
                name="batchQty"
              >
                <a-input-number
                  v-model:value="formState.batchQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.batchqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutputdetail.dailycompletedqty')"
                name="dailyCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.dailyCompletedQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.dailycompletedqty') })"
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
 * PCBA日报实体子表 pcbaOutputDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaOutputDetailCreate } from '@/types/logistics/manufacturing/output/pcba-output-detail'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["prodOrderCode","lineNumber","timePeriod","shiftNo","pcbBoardType","panelSide","batchQty","dailyCompletedQty"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputDetailCreate & { pcbaOutputDetailId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputDetailId) {
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
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.prodordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timePeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.timeperiod') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.shiftno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.shiftno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbBoardType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.pcbboardtype') }),
      trigger: 'blur'
    }
  ],
  panelSide: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutputdetail.panelside') }),
      trigger: 'blur'
    }
  ],
  batchQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.batchqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.batchqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  dailyCompletedQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.dailycompletedqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutputdetail.dailycompletedqty') }))
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

/** 映射为 Create/Update DTO（含主表外键 pcbaOutputId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('batchQty' in payload) {
    const rawbatchQty = payload.batchQty
    payload.batchQty = typeof rawbatchQty === 'number' ? rawbatchQty : Number(rawbatchQty)
  }
  if ('dailyCompletedQty' in payload) {
    const rawdailyCompletedQty = payload.dailyCompletedQty
    payload.dailyCompletedQty = typeof rawdailyCompletedQty === 'number' ? rawdailyCompletedQty : Number(rawdailyCompletedQty)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.pcbaOutputId = props.masterId
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
