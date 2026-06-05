<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/foundation/numbering/components -->
<!-- 文件名称：numbering-form.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：编号规则表单，创建/编辑 TaktNumberings -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    :label-col="{ span: 6 }"
    :wrapper-col="{ span: 18 }"
    layout="horizontal"
  >
    <a-form-item
      :label="t('entity.numbering.rulecode')"
      name="ruleCode"
    >
      <a-input
        v-model:value="formState.ruleCode"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderRuleCode')"
        :disabled="!!formState.numberingId"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.rulename')"
      name="ruleName"
    >
      <a-input
        v-model:value="formState.ruleName"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderRuleName')"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.documenttype')"
      name="documentType"
    >
      <a-input-number
        v-model:value="formState.documentType"
        :min="0"
        style="width: 100%"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.departmentcode')"
      name="departmentCode"
    >
      <a-input
        v-model:value="formState.departmentCode"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderDeptCode')"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.prefix')"
      name="prefix"
    >
      <a-input
        v-model:value="formState.prefix"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderPrefix')"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.dateformat')"
      name="dateFormat"
    >
      <a-input
        v-model:value="formState.dateFormat"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderDateFormat')"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.sequencelength')"
      name="sequenceLength"
    >
      <a-input-number
        v-model:value="formState.sequenceLength"
        :min="1"
        :max="20"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderNumberLength')"
        style="width: 100%"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.sequencestep')"
      name="sequenceStep"
    >
      <a-input-number
        v-model:value="formState.sequenceStep"
        :min="1"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderStep')"
        style="width: 100%"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.suffix')"
      name="suffix"
    >
      <a-input
        v-model:value="formState.suffix"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderSuffix')"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.resetperiod')"
      name="resetPeriod"
    >
      <a-select
        v-model:value="formState.resetPeriod"
        :placeholder="t('common.page.form.placeholder.selectonly')"
        allow-clear
      >
        <a-select-option value="none">none</a-select-option>
        <a-select-option value="daily">daily</a-select-option>
        <a-select-option value="monthly">monthly</a-select-option>
        <a-select-option value="yearly">yearly</a-select-option>
      </a-select>
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.currentsequence')"
      name="currentSequence"
    >
      <a-input-number
        v-model:value="formState.currentSequence"
        :min="0"
        style="width: 100%"
      />
    </a-form-item>
    <a-form-item
      :label="t('entity.numbering.separator')"
      name="separator"
    >
      <a-input
        v-model:value="formState.separator"
        allow-clear
      />
    </a-form-item>
    <a-form-item
      :label="t('common.page.entity.remark')"
      name="remark"
    >
      <a-textarea
        v-model:value="formState.remark"
        :placeholder="t('routine.tasks.numbering-rule.form.placeholderRemark')"
        :rows="2"
        allow-clear
      />
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import type { FormInstance, Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import type { Numbering, NumberingCreate, NumberingUpdate } from '@/types/foundation/numbering'

/** 与 a-input 绑定的表单模型 */
type NumberingFormModel = {
  numberingId?: string
  ruleCode: string
  ruleName: string
  documentType: number
  departmentCode: string
  prefix: string
  dateFormat: string
  sequenceLength: number
  sequenceStep: number
  suffix: string
  resetPeriod: string
  currentSequence: number
  separator: string
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
    documentType: 0,
    departmentCode: '',
    prefix: '',
    dateFormat: '',
    sequenceLength: 5,
    sequenceStep: 1,
    suffix: '',
    resetPeriod: 'none',
    currentSequence: 0,
    separator: '-',
    isBuiltIn: 0,
    status: 1,
    remark: ''
  }
}

/** 表单绑定状态 */
const formState = ref<NumberingFormModel>(emptyModel())

/** 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  ruleCode: [{ required: true, message: t('routine.tasks.numbering-rule.validation.ruleCode') }],
  ruleName: [{ required: true, message: t('routine.tasks.numbering-rule.validation.ruleName') }],
  departmentCode: [{ required: true, message: t('routine.tasks.numbering-rule.validation.ruleCode') }],
  sequenceLength: [{ required: true, message: t('routine.tasks.numbering-rule.validation.numberLength') }],
  resetPeriod: [{ required: true, message: t('common.page.form.placeholder.selectonly') }],
  separator: [{ required: true, message: t('routine.tasks.numbering-rule.validation.ruleCode') }]
}))

watch(
  () => props.formData,
  (val) => {
    if (val) {
      const next: NumberingFormModel = {
        ...emptyModel(),
        ruleCode: val.ruleCode ?? '',
        ruleName: val.ruleName ?? '',
        documentType: val.documentType ?? 0,
        departmentCode: val.departmentCode ?? '',
        prefix: val.prefix ?? '',
        dateFormat: val.dateFormat ?? '',
        sequenceLength: val.sequenceLength ?? 5,
        sequenceStep: val.sequenceStep ?? 1,
        suffix: val.suffix ?? '',
        resetPeriod: val.resetPeriod ?? 'none',
        currentSequence: val.currentSequence ?? 0,
        separator: val.separator ?? '-',
        isBuiltIn: val.isBuiltIn ?? 0,
        status: val.status ?? 1,
        remark: val.remark ?? ''
      }
      if (val.numberingId) {
        next.numberingId = val.numberingId
      }
      formState.value = next
    } else {
      formState.value = emptyModel()
    }
  },
  { immediate: true }
)

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
    ruleCode: s.ruleCode,
    ruleName: s.ruleName,
    documentType: s.documentType,
    departmentCode: s.departmentCode,
    sequenceLength: s.sequenceLength,
    sequenceStep: s.sequenceStep,
    resetPeriod: s.resetPeriod,
    currentSequence: s.currentSequence,
    separator: s.separator,
    isBuiltIn: s.isBuiltIn,
    status: s.status,
    prefix: optionalString(s.prefix),
    dateFormat: optionalString(s.dateFormat),
    suffix: optionalString(s.suffix),
    remark: optionalString(s.remark)
  }
  if (s.numberingId) {
    return { ...base, numberingId: s.numberingId }
  }
  return base
}

defineExpose({ validate, getValues })
</script>
