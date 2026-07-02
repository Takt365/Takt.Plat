<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/components -->
<!-- 文件名称：source-ec-detail-form.vue -->
<!-- 功能描述：设变来源主表实体子表 sourceEcDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form source-ec-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    :disabled="loading || readOnly"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="source-ec-detail-form-tabs"
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
                :label="t('entity.sourceecdetail.sourcefinishedproduct')"
                name="sourceFinishedProduct"
              >
                <a-input
                  v-model:value="formState.sourceFinishedProduct"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcefinishedproduct') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourceparentpart')"
                name="sourceParentPart"
              >
                <a-input
                  v-model:value="formState.sourceParentPart"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourceparentpart') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcelegacypartno')"
                name="sourceLegacyPartNo"
              >
                <a-input
                  v-model:value="formState.sourceLegacyPartNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcelegacypartno') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcelegacypartname')"
                name="sourceLegacyPartName"
              >
                <a-input
                  v-model:value="formState.sourceLegacyPartName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcelegacypartname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcelegacyusage')"
                name="sourceLegacyUsage"
              >
                <a-input-number
                  v-model:value="formState.sourceLegacyUsage"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcelegacyusage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcelegacymountingposition')"
                name="sourceLegacyMountingPosition"
              >
                <a-input
                  v-model:value="formState.sourceLegacyMountingPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcelegacymountingposition') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcereplacementpartno')"
                name="sourceReplacementPartNo"
              >
                <a-input
                  v-model:value="formState.sourceReplacementPartNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcereplacementpartno') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcereplacementpartname')"
                name="sourceReplacementPartName"
              >
                <a-input
                  v-model:value="formState.sourceReplacementPartName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcereplacementpartname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcereplacementusage')"
                name="sourceReplacementUsage"
              >
                <a-input-number
                  v-model:value="formState.sourceReplacementUsage"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcereplacementusage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcereplacementmountingposition')"
                name="sourceReplacementMountingPosition"
              >
                <a-input
                  v-model:value="formState.sourceReplacementMountingPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcereplacementmountingposition') })"
                  show-count
                  :maxlength="40"
                  allow-clear
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
                :label="t('entity.sourceecdetail.sourcebomno')"
                name="sourceBomNo"
              >
                <a-input
                  v-model:value="formState.sourceBomNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcebomno') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourceinterchangeability')"
                name="sourceInterchangeability"
              >
                <a-input
                  v-model:value="formState.sourceInterchangeability"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourceinterchangeability') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcedistinction')"
                name="sourceDistinction"
              >
                <a-input
                  v-model:value="formState.sourceDistinction"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcedistinction') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcearrangementinstruction')"
                name="sourceArrangementInstruction"
              >
                <a-input
                  v-model:value="formState.sourceArrangementInstruction"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcearrangementinstruction') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcelegacypartdisposition')"
                name="sourceLegacyPartDisposition"
              >
                <a-input
                  v-model:value="formState.sourceLegacyPartDisposition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceecdetail.sourcelegacypartdisposition') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceecdetail.sourcebomeffectivedate')"
                name="sourceBomEffectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.sourceBomEffectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.sourcebomeffectivedate') })"
                  value-format="YYYY-MM-DD"
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
 * 设变来源主表实体子表 sourceEcDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/engineering-change/source-ec/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SourceEcDetailCreate } from '@/types/logistics/manufacturing/engineering-change/source-ec-detail'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（16 字段分两 Tab，每 Tab 最多 10 行双列） */
const formContentClass = computed(() => 'takt-form-content-rows-10')
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SourceEcDetailCreate & { sourceEcDetailId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 详情只读模式（禁用全部字段） */
  readOnly?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  readOnly: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 sourceEcDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sourceEcDetailId) {
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
  sourceFinishedProduct: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcefinishedproduct') }),
      trigger: 'blur'
    }
  ],
  sourceParentPart: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourceparentpart') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 sourceEcId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('sourceLegacyUsage' in payload) {
    const rawsourceLegacyUsage = payload.sourceLegacyUsage
    payload.sourceLegacyUsage = typeof rawsourceLegacyUsage === 'number' ? rawsourceLegacyUsage : Number(rawsourceLegacyUsage)
  }
  if ('sourceReplacementUsage' in payload) {
    const rawSourceReplacementUsage = payload.sourceReplacementUsage
    payload.sourceReplacementUsage = typeof rawSourceReplacementUsage === 'number'
      ? rawSourceReplacementUsage
      : Number(rawSourceReplacementUsage)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.sourceEcId = props.masterId
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
  min-height: auto;
}

:deep(.ant-tabs-tabpane) {
  min-height: auto;
}
</style>
