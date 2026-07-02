<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title-change-log/components -->
<!-- 文件名称：account-title-change-log-form.vue -->
<!-- 功能描述：会计科目实体子表 accountTitleChangeLog 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form account-title-change-log-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="account-title-change-log-form-tabs"
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
                :label="t('entity.accounttitlechangelog.code')"
                name="accountTitleCode"
              >
                <a-input
                  v-model:value="formState.accountTitleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitlechangelog.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.accountTitleChangeLogId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitlechangelog.changefields')"
                name="changeFields"
              >
                <a-input
                  v-model:value="formState.changeFields"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitlechangelog.changefields') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitlechangelog.changetime')"
                name="changeTime"
              >
                <a-date-picker
                  v-model:value="formState.changeTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitlechangelog.changetime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitlechangelog.changeby')"
                name="changeBy"
              >
                <a-input
                  v-model:value="formState.changeBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitlechangelog.changeby') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitlechangelog.changereason')"
                name="changeReason"
              >
                <a-input
                  v-model:value="formState.changeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitlechangelog.changereason') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('common.page.entity.extfield') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
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
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会计科目实体子表 accountTitleChangeLog 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/accounting/financial/account-title-change-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AccountTitleChangeLogCreate } from '@/types/accounting/financial/account-title-change-log'
import { RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["accountTitleCode","changeFields","changeTime","changeBy","changeReason","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AccountTitleChangeLogCreate & { accountTitleChangeLogId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 accountTitleChangeLogId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.accountTitleChangeLogId) {
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
  accountTitleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accounttitlechangelog.code') }),
      trigger: 'blur'
    }
  ],
  changeTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accounttitlechangelog.changetime') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 accountTitleId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.accountTitleId = props.masterId
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
