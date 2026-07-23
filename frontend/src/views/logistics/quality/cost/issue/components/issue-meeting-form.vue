<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue/components -->
<!-- 文件名称：issue-meeting-form.vue -->
<!-- 功能描述：品质问题应对主表子表 qualityIssueMeeting 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form issue-meeting-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="issue-meeting-form-tabs"
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
                :label="pi.label('directManpowerCostPerMinute')"
                name="directManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.directManpowerCostPerMinute"
                  :placeholder="pi.ph('directManpowerCostPerMinute')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectManpowerCostPerMinute')"
                name="indirectManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.indirectManpowerCostPerMinute"
                  :placeholder="pi.ph('indirectManpowerCostPerMinute')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('meetingInvestigationContent')"
                name="meetingInvestigationContent"
              >
                <a-textarea
                  v-model:value="formState.meetingInvestigationContent"
                  :placeholder="pi.ph('meetingInvestigationContent')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingInvestigationCost')"
                name="meetingInvestigationCost"
              >
                <a-input-number
                  v-model:value="formState.meetingInvestigationCost"
                  :placeholder="pi.ph('meetingInvestigationCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingTimeMinutes')"
                name="meetingTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.meetingTimeMinutes"
                  :placeholder="pi.ph('meetingTimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('directParticipantCount')"
                name="directParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.directParticipantCount"
                  :placeholder="pi.ph('directParticipantCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectParticipantCount')"
                name="indirectParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.indirectParticipantCount"
                  :placeholder="pi.ph('indirectParticipantCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('investigationWorkTimeMinutes')"
                name="investigationWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.investigationWorkTimeMinutes"
                  :placeholder="pi.ph('investigationWorkTimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('travelCost')"
                name="travelCost"
              >
                <a-input-number
                  v-model:value="formState.travelCost"
                  :placeholder="pi.ph('travelCost')"
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
                :label="pi.label('otherExpenses')"
                name="otherExpenses"
              >
                <a-input-number
                  v-model:value="formState.otherExpenses"
                  :placeholder="pi.ph('otherExpenses')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('otherWorkTimeMinutes')"
                name="otherWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.otherWorkTimeMinutes"
                  :placeholder="pi.ph('otherWorkTimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('otherApparatusCost')"
                name="otherApparatusCost"
              >
                <a-input-number
                  v-model:value="formState.otherApparatusCost"
                  :placeholder="pi.ph('otherApparatusCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingRecorder')"
                name="meetingRecorder"
              >
                <a-input
                  v-model:value="formState.meetingRecorder"
                  :placeholder="pi.ph('meetingRecorder')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
 * 品质问题应对主表子表 qualityIssueMeeting 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/issue/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useQualityIssueMeetingI18n } from '../composables/use-issue-meeting-i18n'

/** 实体字段 i18n */
const pi = useQualityIssueMeetingI18n()

import type { QualityIssueMeetingCreate } from '@/types/logistics/quality/cost/issue-meeting'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","directManpowerCostPerMinute","indirectManpowerCostPerMinute","meetingInvestigationContent","meetingInvestigationCost","meetingTimeMinutes","directParticipantCount","indirectParticipantCount","investigationWorkTimeMinutes","travelCost","otherExpenses","otherWorkTimeMinutes","otherApparatusCost","meetingRecorder","isObsolete"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIssueMeetingCreate & { qualityIssueMeetingId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIssueMeetingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIssueMeetingId) {
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
  directManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('directManpowerCostPerMinute'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('directManpowerCostPerMinute'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('indirectManpowerCostPerMinute'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('indirectManpowerCostPerMinute'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  meetingInvestigationCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('meetingInvestigationCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('meetingInvestigationCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  meetingTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('meetingTimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('meetingTimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  directParticipantCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('directParticipantCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('directParticipantCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectParticipantCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('indirectParticipantCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('indirectParticipantCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  investigationWorkTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('investigationWorkTimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('investigationWorkTimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  travelCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('travelCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('travelCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('otherExpenses'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('otherExpenses'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherWorkTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('otherWorkTimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('otherWorkTimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherApparatusCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('otherApparatusCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('otherApparatusCost'))
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

/** 映射为 Create/Update DTO（含主表外键 qualityIssueId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('directManpowerCostPerMinute' in payload) {
    const rawdirectManpowerCostPerMinute = payload.directManpowerCostPerMinute
    payload.directManpowerCostPerMinute = typeof rawdirectManpowerCostPerMinute === 'number' ? rawdirectManpowerCostPerMinute : Number(rawdirectManpowerCostPerMinute)
  }
  if ('indirectManpowerCostPerMinute' in payload) {
    const rawindirectManpowerCostPerMinute = payload.indirectManpowerCostPerMinute
    payload.indirectManpowerCostPerMinute = typeof rawindirectManpowerCostPerMinute === 'number' ? rawindirectManpowerCostPerMinute : Number(rawindirectManpowerCostPerMinute)
  }
  if ('meetingInvestigationCost' in payload) {
    const rawmeetingInvestigationCost = payload.meetingInvestigationCost
    payload.meetingInvestigationCost = typeof rawmeetingInvestigationCost === 'number' ? rawmeetingInvestigationCost : Number(rawmeetingInvestigationCost)
  }
  if ('meetingTimeMinutes' in payload) {
    const rawmeetingTimeMinutes = payload.meetingTimeMinutes
    payload.meetingTimeMinutes = typeof rawmeetingTimeMinutes === 'number' ? rawmeetingTimeMinutes : Number(rawmeetingTimeMinutes)
  }
  if ('directParticipantCount' in payload) {
    const rawdirectParticipantCount = payload.directParticipantCount
    payload.directParticipantCount = typeof rawdirectParticipantCount === 'number' ? rawdirectParticipantCount : Number(rawdirectParticipantCount)
  }
  if ('indirectParticipantCount' in payload) {
    const rawindirectParticipantCount = payload.indirectParticipantCount
    payload.indirectParticipantCount = typeof rawindirectParticipantCount === 'number' ? rawindirectParticipantCount : Number(rawindirectParticipantCount)
  }
  if ('investigationWorkTimeMinutes' in payload) {
    const rawinvestigationWorkTimeMinutes = payload.investigationWorkTimeMinutes
    payload.investigationWorkTimeMinutes = typeof rawinvestigationWorkTimeMinutes === 'number' ? rawinvestigationWorkTimeMinutes : Number(rawinvestigationWorkTimeMinutes)
  }
  if ('travelCost' in payload) {
    const rawtravelCost = payload.travelCost
    payload.travelCost = typeof rawtravelCost === 'number' ? rawtravelCost : Number(rawtravelCost)
  }
  if ('otherExpenses' in payload) {
    const rawotherExpenses = payload.otherExpenses
    payload.otherExpenses = typeof rawotherExpenses === 'number' ? rawotherExpenses : Number(rawotherExpenses)
  }
  if ('otherWorkTimeMinutes' in payload) {
    const rawotherWorkTimeMinutes = payload.otherWorkTimeMinutes
    payload.otherWorkTimeMinutes = typeof rawotherWorkTimeMinutes === 'number' ? rawotherWorkTimeMinutes : Number(rawotherWorkTimeMinutes)
  }
  if ('otherApparatusCost' in payload) {
    const rawotherApparatusCost = payload.otherApparatusCost
    payload.otherApparatusCost = typeof rawotherApparatusCost === 'number' ? rawotherApparatusCost : Number(rawotherApparatusCost)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.qualityIssueId = props.masterId
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
