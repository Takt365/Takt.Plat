<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/foundation/i18n/components -->
<!-- 文件名称：translation-form.vue -->
<!-- 创建时间：2026-01-29 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：翻译表单（主表），用于创建/编辑单条翻译 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="translation-form">
    <a-form
      ref="formRef"
      :model="formState"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
      layout="horizontal"
    >
      <a-form-item
        :label="t('entity.translation.resourcekey')"
        name="i18nKey"
      >
        <a-input
          v-model:value="formState.i18nKey"
          :placeholder="t('routine.localization.translation.placeholders.resourceKeyExample')"
        />
      </a-form-item>
      <a-form-item
        :label="t('routine.localization.translation.form.languageSub')"
        name="cultureCode"
      >
        <TaktSelect
          v-model:value="formState.cultureCode"
          :options="cultureOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.translation.culturecode') })"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :loading="cultureOptionsLoading"
          allow-clear
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.translation.translationvalue')"
        name="translationText"
      >
        <a-input
          v-model:value="formState.translationText"
          :placeholder="t('routine.localization.translation.placeholders.translationValueInLanguage')"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.translation.resourcetype')"
        name="resourceType"
      >
        <a-select
          v-model:value="formState.resourceType"
          :placeholder="t('routine.localization.translation.placeholders.resourceTypeSelect')"
          allow-clear
        >
          <a-select-option :value="0">
            {{ t('routine.localization.translation.options.frontend') }}
          </a-select-option>
          <a-select-option :value="1">
            {{ t('routine.localization.translation.options.backend') }}
          </a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item
        :label="t('entity.translation.resourcegroup')"
        name="resourceGroup"
      >
        <a-input-number
          v-model:value="formState.resourceGroup"
          :min="0"
          :placeholder="t('routine.localization.translation.placeholders.resourceGroupOptional')"
          style="width: 100%"
        />
      </a-form-item>
      <a-form-item
        :label="t('common.page.entity.remark')"
        name="remark"
      >
        <a-textarea
          v-model:value="formState.remark"
          :placeholder="t('routine.localization.translation.placeholders.remarkOptional')"
          :rows="2"
        />
      </a-form-item>
    </a-form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, watch, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { getCultureOptions } from '@/api/foundation/culture'
import type { Translation, TranslationCreate, TranslationUpdate } from '@/types/foundation/translation'
import type { TaktSelectOption } from '@/types/common'

interface Props {
  /** 编辑模式下的翻译数据 */
  formData?: Translation | null
  /** 提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

const { t } = useI18n()

/** 表单 ref */
const formRef = ref()
/** 区域文化下拉选项 */
const cultureOptions = ref<TaktSelectOption[]>([])
/** 区域文化选项 loading */
const cultureOptionsLoading = ref(false)

type TranslationFormState = Omit<TranslationCreate, 'resourceGroup' | 'remark'> & {
  translationId?: string
  resourceGroup: number
  remark: string
}

/** 表单绑定状态 */
const formState = reactive<TranslationFormState>({
  i18nKey: '',
  cultureId: '',
  cultureCode: '',
  translationText: '',
  resourceType: 0,
  resourceGroup: 0,
  remark: ''
})

/** 校验规则 */
const formRules = computed<Record<string, Rule[]>>(() => ({
  i18nKey: [{ required: true, message: t('routine.localization.translation.rules.resourceKeyRequired'), trigger: 'blur' }],
  cultureCode: [{ required: true, message: t('routine.localization.translation.rules.cultureCodeRequired'), trigger: 'change' }],
  translationText: [{ required: true, message: t('routine.localization.translation.rules.translationValueRequired'), trigger: 'blur' }],
  resourceType: [{ required: true, message: t('routine.localization.translation.rules.resourceTypeRequired'), trigger: 'change' }]
}))

/**
 * 解析资源类别（兼容历史字符串）
 * @param value 原始值
 * @returns {number} 0=前端，1=后端
 */
function parseResourceType(value: unknown): number {
  if (value === 1 || value === '1' || value === 'Backend') return 1
  return 0
}

/**
 * 解析资源分组为数字
 * @param value 原始值
 * @returns {number} 分组编号
 */
function parseResourceGroup(value: unknown): number {
  const n = Number(value)
  return Number.isFinite(n) ? n : 0
}

/**
 * 加载区域文化下拉选项
 * @returns {Promise<void>}
 */
async function loadCultureOptions() {
  try {
    cultureOptionsLoading.value = true
    const list = await getCultureOptions()
    const listRaw = (list || []) as unknown[]
    cultureOptions.value = listRaw.map((x): TaktSelectOption => {
      const r = x as Record<string, unknown>
      const ev = r['extValue']
      const base: TaktSelectOption = {
        dictLabel: `${String(r['dictLabel'] ?? '')} (${String(r['extLabel'] ?? r['dictValue'] ?? '')})`,
        dictValue: (r['dictValue'] ?? '') as string | number,
        sortOrder: typeof r['sortOrder'] === 'number' ? r['sortOrder'] : Number(r['sortOrder'] ?? 0)
      }
      if (ev != null && String(ev) !== '') {
        base.extValue = ev as string | number
      }
      return base
    })
  } finally {
    cultureOptionsLoading.value = false
  }
}

watch(
  () => props.formData,
  (v) => {
    if (v) {
      Object.assign(formState, {
        translationId: v.translationId,
        i18nKey: v.i18nKey ?? '',
        cultureId: v.cultureId ?? '',
        cultureCode: v.cultureCode ?? '',
        translationText: v.translationText ?? '',
        resourceType: parseResourceType(v.resourceType),
        resourceGroup: parseResourceGroup(v.resourceGroup),
        remark: v.remark ?? ''
      })
    } else {
      Object.assign(formState, {
        translationId: undefined,
        i18nKey: '',
        cultureId: '',
        cultureCode: '',
        translationText: '',
        resourceType: 0,
        resourceGroup: 0,
        remark: ''
      })
    }
  },
  { immediate: true, deep: true }
)

watch(
  () => formState.cultureCode,
  (code) => {
    const opt = cultureOptions.value.find(
      (o) => o.dictValue === code || o.extValue === code
    )
    formState.cultureId = String(opt?.extValue ?? '')
  }
)

onMounted(() => {
  loadCultureOptions()
})

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate() {
  await formRef.value?.validate()
}

/**
 * 获取提交 DTO
 * @returns {TranslationCreate | TranslationUpdate} 翻译 DTO
 */
function getFormData(): TranslationCreate | TranslationUpdate {
  const opt = cultureOptions.value.find(
    (o) => o.dictValue === formState.cultureCode || o.extValue === formState.cultureCode
  )
  const cultureId = String(opt?.extValue ?? formState.cultureId ?? '')
  const base: TranslationCreate = {
    i18nKey: formState.i18nKey,
    cultureCode: formState.cultureCode,
    translationText: formState.translationText,
    resourceType: formState.resourceType,
    cultureId,
    resourceGroup: formState.resourceGroup
  }
  if (formState.remark && formState.remark.trim() !== '') {
    base.remark = formState.remark
  }
  if (formState.translationId) {
    return { ...base, translationId: formState.translationId }
  }
  return base
}

defineExpose({ validate, getFormData })
</script>

<style scoped lang="css">
.translation-form {
  padding: 0;
}
</style>
