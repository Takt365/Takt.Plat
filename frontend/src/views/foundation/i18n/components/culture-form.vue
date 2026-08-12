<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/foundation/i18n/components -->
<!-- 文件名称：culture-form.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：区域文化表单（新增/编辑）；defineExpose validate、getFormData、resetFields -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="culture-form">
    <a-form
      ref="formRef"
      :model="formState"
      :rules="formRules"
      :label-col="{ span: 4 }"
      :wrapper-col="{ span: 20 }"
      layout="horizontal"
      label-align="right"
    >
      <a-form-item
        :label="t('entity.culture.languagename')"
        name="languageName"
      >
        <a-input
          v-model:value="formState.languageName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.culture.languagename') })"
          :disabled="props.loading"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.culture.code')"
        name="cultureCode"
      >
        <TaktSelect
          v-model:value="formState.cultureCode"
          dict-type="sys_culture_code"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.culture.code') })"
          :disabled="props.loading || !!formState.cultureId"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.culture.nativename')"
        name="nativeName"
      >
        <a-input
          v-model:value="formState.nativeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.culture.nativename') })"
          :disabled="props.loading || !!formState.cultureId"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.culture.icon')"
        name="icon"
      >
        <a-input
          v-model:value="formState.icon"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.culture.icon') })"
          :disabled="props.loading"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.culture.isdefault')"
        name="isDefault"
      >
        <TaktSelect
          v-model:value="formState.isDefault"
          dict-type="sys_yes_no_type"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.culture.isdefault') })"
          :disabled="props.loading"
        />
      </a-form-item>
      <a-form-item
        :label="t('common.page.entity.remark')"
        name="remark"
      >
        <a-textarea
          v-model:value="formState.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="3"
          :disabled="props.loading"
        />
      </a-form-item>
    </a-form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, watch, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { Culture, CultureCreate, CultureUpdate } from '@/types/foundation/culture'
import { useTenantStore } from '@/stores/identity/tenant'
import { useDictDataStore } from '@/stores/foundation/dict-data'

const SYS_CULTURE_CODE_DICT = 'sys_culture_code'

type CultureFormState = Omit<CultureCreate, 'icon' | 'remark' | 'tenantCode' | 'translationList' | 'ExtField' | 'sortOrder'> & {
  cultureId?: string
  icon: string
  remark: string
}

interface Props {
  /** 编辑模式下的区域文化数据 */
  formData?: Culture | null
  /** 提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

const { t } = useI18n()
/** 当前租户编码（Create DTO 必填，与请求头 X-Tenant-Code 一致） */
const tenantStore = useTenantStore()
/** 字典缓存（cultureCode → nativeName 取自 DictLabel） */
const dictDataStore = useDictDataStore()

/** 表单 ref */
const formRef = ref()
/** 表单绑定状态 */
const formState = reactive<CultureFormState>(createEmptyFormState())

/** 校验规则 */
const formRules = computed<Record<string, Rule[]>>(() => ({
  cultureCode: [
    { required: true, message: t('common.page.form.placeholder.select', { field: t('entity.culture.code') }), trigger: 'change' }
  ],
  languageName: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.culture.languagename') }), trigger: 'blur' }
  ],
  nativeName: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.culture.nativename') }), trigger: 'blur' }
  ]
}))

/**
 * 空表单初始态
 * @returns {CultureFormState} 初始状态
 */
function createEmptyFormState(): CultureFormState {
  return {
    languageName: '',
    cultureCode: '',
    nativeName: '',
    icon: '',
    isDefault: 1,
    remark: ''
  }
}

/**
 * 将 Culture DTO 灌入表单
 * @param data 区域文化 DTO；为空则重置
 */
function applyFormData(data: Culture | null | undefined) {
  if (data) {
    Object.assign(formState, {
      cultureId: data.cultureId,
      languageName: data.languageName || '',
      cultureCode: data.cultureCode || '',
      nativeName: data.nativeName || '',
      icon: data.icon || '',
      isDefault: data.isDefault ?? 1,
      remark: data.remark || ''
    })
  } else {
    Object.assign(formState, createEmptyFormState(), { cultureId: undefined })
  }
}

watch(
  () => props.formData,
  (newData) => {
    applyFormData(newData)
  },
  { immediate: true, deep: true }
)

/**
 * 新增时选择区域编码：NativeName 与 sys_culture_code.DictLabel 对齐（本族语 + 地区缩写）
 * @param code BCP47 区域编码
 */
function applyNativeNameFromCultureDict(code: string | undefined) {
  if (!code?.trim() || formState.cultureId) {
    return
  }
  const option = dictDataStore.getDictOption(code, SYS_CULTURE_CODE_DICT)
  const nativeLabel = option?.dictLabel?.trim()
  if (nativeLabel) {
    formState.nativeName = nativeLabel
  }
}

watch(
  () => formState.cultureCode,
  (code) => {
    applyNativeNameFromCultureDict(code)
  }
)

onMounted(async () => {
  await dictDataStore.loadAllDictDataAsync()
  applyNativeNameFromCultureDict(formState.cultureCode)
})

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate() {
  await formRef.value?.validate()
}

/**
 * 重置表单
 * @returns {void}
 */
function resetFields() {
  formRef.value?.resetFields()
  applyFormData(props.formData)
}

/**
 * 获取提交 DTO（CultureCreate / CultureUpdate）
 * @returns {CultureCreate | CultureUpdate} 区域文化 DTO
 */
function getFormData(): CultureCreate | CultureUpdate {
  const tenantCode = tenantStore.tenantCode?.trim() || ''
  const base = {
    tenantCode,
    languageName: formState.languageName,
    cultureCode: formState.cultureCode,
    nativeName: formState.nativeName,
    icon: formState.icon || undefined,
    isDefault: formState.isDefault,
    remark: formState.remark || undefined
  } as CultureCreate
  if (formState.cultureId) {
    return { ...base, cultureId: formState.cultureId } as CultureUpdate
  }
  return base
}

defineExpose({
  validate,
  getFormData,
  resetFields
})
</script>
