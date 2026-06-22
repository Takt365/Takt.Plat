<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/views/foundation/numbering/components -->
<!-- 文件名称：numbering-form.vue -->
<!-- 功能描述：编号规则表单，创建/编辑 TaktNumberings；defineExpose validate、getValues -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.rulecode')"
          name="ruleCode"
        >
          <a-input
            v-model:value="formState.ruleCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulecode') })"
            :disabled="!!formData?.numberingId"
            show-count
            :maxlength="NUMBERING_RULE_CODE_MAX_LENGTH"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.rulename')"
          name="ruleName"
        >
          <a-input
            v-model:value="formState.ruleName"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulename') })"
            show-count
            :maxlength="NUMBERING_RULE_NAME_MAX_LENGTH"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.documenttype')"
          name="documentType"
        >
          <TaktSelect
            v-model:value="formState.documentType"
            :options="businessDomainOptions"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.documenttype') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.departmentcode')"
          name="departmentCode"
        >
          <TaktSelect
            v-model:value="formState.departmentCode"
            api-url="TaktIsoCodes/options"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.departmentcode') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.prefixcode')"
          name="prefixCode"
        >
          <a-input
            v-model:value="formState.prefixCode"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.numbering.prefixcode') })"
            show-count
            :maxlength="NUMBERING_PREFIX_CODE_MAX_LENGTH"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.dateformat')"
          name="dateFormat"
        >
          <TaktSelect
            v-model:value="formState.dateFormat"
            dict-type="sys_numbering_date_format_config"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.dateformat') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.sequencelength')"
          name="sequenceLength"
        >
          <a-input-number
            v-model:value="formState.sequenceLength"
            :min="6"
            :max="10"
            :step="2"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencelength') })"
            class="w-full"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.sequencestep')"
          name="sequenceStep"
        >
          <a-input-number
            v-model:value="formState.sequenceStep"
            :min="1"
            :max="10"
            :step="1"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencestep') })"
            class="w-full"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.suffixcode')"
          name="suffixCode"
        >
          <a-input
            v-model:value="formState.suffixCode"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.numbering.suffixcode') })"
            show-count
            :maxlength="NUMBERING_SUFFIX_CODE_MAX_LENGTH"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.resetperiod')"
          name="resetPeriod"
        >
          <TaktSelect
            v-model:value="formState.resetPeriod"
            dict-type="sys_reset_period_config"
            disabled
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.resetperiod') })"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="formState.numberingId"
        :span="12"
      >
        <a-form-item
          :label="t('entity.numbering.examplecode')"
        >
          <a-input
            :value="formState.exampleCode"
            show-count
            :maxlength="NUMBERING_EXAMPLE_CODE_MAX_LENGTH"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="formState.numberingId"
        :span="12"
      >
        <a-form-item
          :label="t('entity.numbering.currentsequence')"
          name="currentSequence"
        >
          <a-input-number
            v-model:value="formState.currentSequence"
            :min="0"
            disabled
            class="w-full"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.separator')"
          name="separator"
        >
          <a-radio-group v-model:value="formState.separator">
            <a-radio value="-">
              -
            </a-radio>
            <a-radio value="">
              {{ t('foundation.numbering.page.separator.empty') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.isbuiltin')"
          name="isBuiltIn"
        >
          <TaktSelect
            v-model:value="formState.isBuiltIn"
            dict-type="sys_yes_no_type"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.numbering.status')"
          name="status"
        >
          <TaktSelect
            v-model:value="formState.status"
            dict-type="sys_normal_disable_status"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.status') })"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('common.page.entity.remark')"
          name="remark"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <a-textarea
            v-model:value="formState.remark"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { ref, watch, computed, onMounted } from 'vue'
import type { FormInstance, Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import type { Numbering, NumberingCreate, NumberingUpdate } from '@/types/foundation/numbering'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import {
  normalizeNumberingResetPeriod,
  resolveRequiredResetPeriod,
} from '@/utils/takt-numbering-reset-period'

/** 表单/提交默认重置周期（不使用日期时） */
const DEFAULT_RESET_PERIOD = 'none'
const NUMBERING_RULE_CODE_MAX_LENGTH = 50
/** 规则名称最大长度（与 TaktNumbering.RuleName Length=100 一致） */
const NUMBERING_RULE_NAME_MAX_LENGTH = 100
/** 前缀编码最大长度（与 TaktNumbering.PrefixCode Length=4 一致） */
const NUMBERING_PREFIX_CODE_MAX_LENGTH = 4
/** 后缀编码最大长度（与 TaktNumbering.SuffixCode Length=4 一致） */
const NUMBERING_SUFFIX_CODE_MAX_LENGTH = 4
/** 起始编码最大长度（与 TaktNumbering.ExampleCode Length=100 一致） */
const NUMBERING_EXAMPLE_CODE_MAX_LENGTH = 100
/** 一级菜单业务领域选项（与 DocumentType 存储文本一致） */
const BUSINESS_DOMAIN_VALUES = [
  'Foundation',
  'Routine',
  'Accounting',
  'Logistics',
  'HumanResource',
  'Identity',
  'Workflow',
  'Code',
] as const

/** 与表单绑定的本地模型 */
type NumberingFormModel = {
  numberingId?: string
  ruleCode: string
  ruleName: string
  documentType: string
  departmentCode: string
  prefixCode: string
  dateFormat: string
  sequenceLength: number
  sequenceStep: number
  suffixCode: string
  resetPeriod: string
  exampleCode: string
  currentSequence: number
  separator: '' | '-'
  isBuiltIn: number
  status: number
  remark: string
}

export type NumberingFormValues = NumberingCreate | NumberingUpdate

interface Props {
  /** 编辑模式下的编号规则 */
  formData?: Partial<Numbering> | null
  /** 提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

const { t } = useI18n()
/** Pinia：字典缓存（表单打开前确保已加载，避免下拉空白） */
const dictDataStore = useDictDataStore()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 表单 ref */
const formRef = ref<FormInstance>()

/**
 * 空表单默认值
 * @returns {NumberingFormModel} 默认模型
 */
function emptyModel(): NumberingFormModel {
  return {
    ruleCode: '',
    ruleName: '',
    documentType: 'Foundation',
    departmentCode: '',
    prefixCode: '',
    dateFormat: 'none',
    sequenceLength: 6,
    sequenceStep: 1,
    suffixCode: '',
    resetPeriod: DEFAULT_RESET_PERIOD,
    exampleCode: '',
    currentSequence: 0,
    separator: '-',
    isBuiltIn: 0,
    status: 1,
    remark: ''
  }
}

/** 表单绑定状态 */
const formState = ref<NumberingFormModel>(emptyModel())

/** 业务领域下拉选项 */
const businessDomainOptions = computed(() =>
  BUSINESS_DOMAIN_VALUES.map((value) => ({ label: value, value })),
)

/**
 * 表单日期格式：空值映射为字典 none
 * @param value 后端或表单日期格式
 * @returns {string} 字典 dictValue
 */
function mapDateFormatForForm(value?: string | null): string {
  const trimmed = value?.trim()
  return trimmed ? trimmed : 'none'
}

/**
 * 提交日期格式：none 传 undefined
 * @param value 字典选中值
 * @returns {string | undefined} API 日期格式
 */
function mapDateFormatForSubmit(value: string): string | undefined {
  const trimmed = value.trim()
  return !trimmed || trimmed === 'none' ? undefined : trimmed
}

/**
 * 归一化重置周期为字典 dictValue（编辑回填兼容 daily/monthly/yearly 等）
 * @param value 后端 resetPeriod
 * @returns {string} sys_reset_period_config dictValue
 */
function normalizeResetPeriodForForm(value?: string | null): string {
  return normalizeNumberingResetPeriod(value)
}

/**
 * 提交重置周期：与 DateFormat 粒度一致
 * @param dateFormat 表单日期格式
 * @param resetPeriod 表单重置周期
 * @returns {string} API resetPeriod
 */
function normalizeResetPeriodForSubmit(dateFormat: string, resetPeriod: string): string {
  return resolveRequiredResetPeriod(dateFormat) ?? normalizeNumberingResetPeriod(resetPeriod)
}

/**
 * 表单分隔符：连字符或无（旧数据 _ 归一为 -）
 * @param value 后端或表单分隔符
 * @returns {'' | '-'} 单选值
 */
function normalizeSeparatorForForm(value?: string | null): '' | '-' {
  if (value === '-') {
    return '-'
  }
  if (value === '' || value == null) {
    return '-'
  }
  return '-'
}

/**
 * 提交分隔符
 * @param value 单选值
 * @returns {string} API 分隔符
 */
function mapSeparatorForSubmit(value: '' | '-'): string {
  if (value === '-') {
    return '-'
  }
  return ''
}

/**
 * 流水位数：6/8/10，非法值回退默认 6
 * @param value 后端或表单流水位数
 * @returns {number} 合法流水位数
 */
function normalizeSequenceLengthForForm(value?: number | null): number {
  if (value === 6 || value === 8 || value === 10) {
    return value
  }
  return 6
}

/**
 * 流水步长：1～10，非法值回退默认 1
 * @param value 后端或表单流水步长
 * @returns {number} 合法流水步长
 */
function normalizeSequenceStepForForm(value?: number | null): number {
  if (value == null || Number.isNaN(value)) {
    return 1
  }
  return Math.min(10, Math.max(1, value))
}

/** 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  ruleCode: [{ required: true, message: t('common.validation.required', { field: t('entity.numbering.rulecode') }) }],
  ruleName: [{ required: true, message: t('common.validation.required', { field: t('entity.numbering.rulename') }) }],
  documentType: [{ required: true, message: t('common.validation.required', { field: t('entity.numbering.documenttype') }) }],
  departmentCode: [{ required: true, message: t('common.validation.required', { field: t('entity.numbering.departmentcode') }) }],
  sequenceLength: [{ required: true, message: t('common.validation.required', { field: t('entity.numbering.sequencelength') }) }],
  resetPeriod: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.numbering.resetperiod') }) }],
  status: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.numbering.status') }) }]
}))

watch(
  () => formState.value.dateFormat,
  (dateFormat) => {
    const required = resolveRequiredResetPeriod(dateFormat)
    if (required) {
      formState.value.resetPeriod = required
    }
  },
)

watch(
  () => props.formData,
  (val) => {
    if (val?.numberingId) {
      const next: NumberingFormModel = {
        ...emptyModel(),
        ruleCode: val.ruleCode ?? '',
        ruleName: val.ruleName ?? '',
        documentType: val.documentType?.trim() || 'Foundation',
        departmentCode: val.departmentCode ?? '',
        prefixCode: val.prefixCode ?? '',
        dateFormat: mapDateFormatForForm(val.dateFormat),
        sequenceLength: normalizeSequenceLengthForForm(val.sequenceLength),
        sequenceStep: normalizeSequenceStepForForm(val.sequenceStep),
        suffixCode: val.suffixCode ?? '',
        resetPeriod: resolveRequiredResetPeriod(mapDateFormatForForm(val.dateFormat))
          ?? normalizeResetPeriodForForm(val.resetPeriod),
        exampleCode: val.exampleCode ?? '',
        currentSequence: val.currentSequence ?? 0,
        separator: normalizeSeparatorForForm(val.separator),
        isBuiltIn: val.isBuiltIn ?? 0,
        status: val.status ?? 1,
        remark: val.remark ?? ''
      }
      next.numberingId = val.numberingId
      formState.value = next
    } else {
      formState.value = emptyModel()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/**
 * 可选字符串：空串转 undefined
 * @param s 输入
 * @returns {string | undefined} 修剪后或 undefined
 */
function optionalString(s: string): string | undefined {
  const trimmed = s.trim()
  return trimmed.length > 0 ? trimmed : undefined
}

/**
 * 租户/公司隔离字段：编辑态保留原值，新增态从登录上下文注入
 * @returns {Pick<NumberingCreate, 'tenantCode' | 'companyCode' | 'companyDefaultCulture'>}
 */
function resolveScopeFields(): Pick<NumberingCreate, 'tenantCode' | 'companyCode' | 'companyDefaultCulture'> {
  return {
    tenantCode: props.formData?.tenantCode ?? tenantStore.tenantCode ?? userStore.userInfo?.tenantCode ?? '',
    companyCode: props.formData?.companyCode ?? tenantStore.companyCode ?? userStore.userInfo?.companyCode ?? '',
    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
  }
}

/**
 * 校验表单
 * @returns {Promise<void>}
 */
function validate() {
  return formRef.value?.validate()
}

/**
 * 获取提交 DTO
 * @returns {NumberingFormValues} 创建或更新 DTO
 */
function getValues(): NumberingFormValues {
  const s = formState.value
  const base: NumberingCreate = {
    ...resolveScopeFields(),
    ruleCode: s.ruleCode,
    ruleName: s.ruleName,
    documentType: s.documentType.trim(),
    departmentCode: s.departmentCode,
    sequenceLength: s.sequenceLength,
    sequenceStep: s.sequenceStep,
    resetPeriod: normalizeResetPeriodForSubmit(s.dateFormat, s.resetPeriod),
    separator: mapSeparatorForSubmit(s.separator),
    isBuiltIn: s.isBuiltIn,
    status: s.status,
    prefixCode: optionalString(s.prefixCode),
    dateFormat: mapDateFormatForSubmit(s.dateFormat),
    suffixCode: optionalString(s.suffixCode),
    remark: optionalString(s.remark),
  }
  if (s.numberingId) {
    return {
      ...base,
      numberingId: s.numberingId,
    }
  }
  return base
}

/**
 * 清空表单并恢复默认值
 */
function resetFields() {
  formState.value = emptyModel()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
