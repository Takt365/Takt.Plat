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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('timePeriod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="pi.ph('timePeriod')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="stdCapacity"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="pi.stdCapacityHint()"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('stdCapacity') }}</span>
                  </span>
                </template>
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="pi.ph('stdCapacity')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('prodActualQty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="pi.ph('prodActualQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('downtimeMinutes')"
                name="downtimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.downtimeMinutes"
                  :placeholder="pi.ph('downtimeMinutes')"
                  style="width: 100%"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('downtimeReason')"
                name="downtimeReason"
              >
                <TaktSelect
                  v-model:value="formState.downtimeReason"
                  dict-type="logistics_stop_reason_category"
                  multiple
                  :placeholder="pi.ph('downtimeReason')"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('downtimeDescription')"
                name="downtimeDescription"
              >
                <a-textarea
                  v-model:value="formState.downtimeDescription"
                  :placeholder="pi.ph('downtimeDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('unachievedReason')"
                name="unachievedReason"
              >
                <TaktSelect
                  v-model:value="formState.unachievedReason"
                  dict-type="logistics_nonachievement_reason_category"
                  multiple
                  :placeholder="pi.ph('unachievedReason')"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('unachievedDescription')"
                name="unachievedDescription"
              >
                <a-textarea
                  v-model:value="formState.unachievedDescription"
                  :placeholder="pi.ph('unachievedDescription')"
                  :rows="2"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="confirmMinutes"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="pi.confirmMinutesHint()"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('confirmMinutes') }}</span>
                  </span>
                </template>
                <a-input-number
                  v-model:value="formState.confirmMinutes"
                  :placeholder="pi.ph('confirmMinutes')"
                  style="width: 100%"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="pi.ph('prodOrderCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                  disabled
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
import { RiQuestionLine } from '@remixicon/vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyOutputDetailI18n } from '../composables/use-assy-output-detail-i18n'
import {
  calculateAssyOutputDetailDerived,
  type AssyOutputMasterCalcSnapshot,
} from '../composables/use-assy-output-derived-calc'
import { useAssyOutputDetailDictMultiFormat } from '../composables/use-assy-output-detail-dict-multi-format'
import {
  applyAssyCleaningPeriodDefaults,
  ASSY_CLEANING_STOP_REASON_LABEL,
  isAssyCleaningTimePeriod,
} from '@/utils/takt-production-stat'

/** 实体字段 i18n */
const pi = useAssyOutputDetailI18n()

import type { AssyOutputDetailCreate } from '@/types/logistics/manufacturing/output/assy-output-detail'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
const { formatDowntimeReason, formatUnachievedReason, sortDowntimeReasonValues, sortUnachievedReasonValues, parseDowntimeReasonForSelect, parseUnachievedReasonForSelect } =
  useAssyOutputDetailDictMultiFormat()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["timePeriod","prodActualQty","downtimeMinutes","downtimeReason","downtimeDescription","unachievedReason","unachievedDescription","confirmMinutes","prodOrderCode","lineNumber"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyOutputDetailCreate & { assyOutputDetailId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表派生计算快照（直接人员、标准产能） */
  masterContext?: AssyOutputMasterCalcSnapshot | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterContext: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  if (!Array.isArray(target.downtimeReason)) {
    target.downtimeReason = parseDowntimeReasonForSelect(target.downtimeReason as string)
  }
  if (!Array.isArray(target.unachievedReason)) {
    target.unachievedReason = parseUnachievedReasonForSelect(target.unachievedReason as string)
  }
}

/** 灌入表单时解析多选字典字段（库内 Label → Select DictValue） */
function hydrateDictMultiFields(target: Record<string, unknown>) {
  target.downtimeReason = parseDowntimeReasonForSelect(target.downtimeReason as string)
  target.unachievedReason = parseUnachievedReasonForSelect(target.unachievedReason as string)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 按主表快照刷新明细派生字段 */
function refreshDerivedDisplayFields() {
  const master = props.masterContext
  if (!master) {
    return
  }
  const directLabor = Number(master.directLabor) || 0
  const prodQty = Number(formState.prodActualQty) || 0
  applyAssyCleaningPeriodDefaults(formState, directLabor)
  if (isAssyCleaningTimePeriod(String(formState.timePeriod ?? '')) && prodQty > 0) {
    formState.downtimeReason = parseDowntimeReasonForSelect(ASSY_CLEANING_STOP_REASON_LABEL)
  }
  const isCreate = !props.formData?.assyOutputDetailId
  const mixedProd = isCreate ? 0 : (Number(formState.mixedProd) || 0)
  const derived = calculateAssyOutputDetailDerived(master, {
    prodActualQty: Number(formState.prodActualQty) || 0,
    downtimeMinutes: Number(formState.downtimeMinutes) || 0,
    confirmMinutes: Number(formState.confirmMinutes) || 0,
    mixedProd,
  })
  formState.inputMinutes = derived.inputMinutes
  formState.actualMinutes = derived.actualMinutes
  formState.indirectMinutes = derived.indirectMinutes
  formState.stdCapacity = derived.stdCapacity
  formState.achievementRate = derived.achievementRate
  if (isCreate) {
    formState.mixedProd = 0
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyOutputDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyOutputDetailId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      hydrateDictMultiFields(next)
      Object.assign(formState, next)
      refreshDerivedDisplayFields()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        hydrateDictMultiFields(next)
        Object.assign(formState, next)
      }
      applyFormDefaults(formState)
      refreshDerivedDisplayFields()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

watch(
  () => [
    props.masterContext?.directLabor,
    props.masterContext?.indirectLabor,
    props.masterContext?.stdCapacity,
    props.masterContext?.stdMinutes,
    formState.timePeriod,
    formState.prodActualQty,
    formState.downtimeMinutes,
    formState.confirmMinutes,
  ] as const,
  () => {
    refreshDerivedDisplayFields()
  }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'blur'
    }
  ],
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
  timePeriod: [
    {
      required: true,
      message: pi.ph('timePeriod'),
      trigger: 'blur'
    }
  ],
  prodActualQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downtimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  confirmMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('confirmMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('confirmMinutes'))
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
  if ('confirmMinutes' in payload) {
    const rawConfirmMinutes = payload.confirmMinutes
    payload.confirmMinutes = typeof rawConfirmMinutes === 'number' ? rawConfirmMinutes : Number(rawConfirmMinutes)
  }
  if ('downtimeReason' in payload) {
    payload.downtimeReason = formatDowntimeReason(payload.downtimeReason)
  }
  if ('unachievedReason' in payload) {
    payload.unachievedReason = formatUnachievedReason(payload.unachievedReason)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.assyOutputId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    const next = { ...props.formData } as Record<string, unknown>
    hydrateDictMultiFields(next)
    Object.assign(formState, next)
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
