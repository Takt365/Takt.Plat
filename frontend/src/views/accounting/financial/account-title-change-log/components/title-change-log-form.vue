<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title-change-log/components -->
<!-- 文件名称：title-change-log-form.vue -->
<!-- 功能描述：会计科目变更记录实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
    <a-tabs
      v-model:active-key="activeTab"
      class="title-change-log-form-tabs"
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
                :label="t('entity.accountTitleChangeLog.accounttitleid')"
                name="accountTitleId"
              >
                <a-input
                  v-model:value="formState.accountTitleId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.accounttitleid') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitleChangeLog.titlecode')"
                name="titleCode"
              >
                <a-input
                  v-model:value="formState.titleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.titlecode') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitleChangeLog.changefields')"
                name="changeFields"
              >
                <a-input
                  v-model:value="formState.changeFields"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.changefields') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitleChangeLog.changetime')"
                name="changeTime"
              >
                <a-input
                  v-model:value="formState.changeTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.changetime') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitleChangeLog.changeby')"
                name="changeBy"
              >
                <a-input
                  v-model:value="formState.changeBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.changeby') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitleChangeLog.changereason')"
                name="changeReason"
              >
                <a-input
                  v-model:value="formState.changeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.changereason') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
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
                  :rows="3"
                  show-count
                  :maxlength="500"
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
 * 会计科目变更记录实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/account-title-change-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AccountTitleChangeLogCreate } from '@/types/accounting/financial/account-title-change-log'

const { t } = useI18n()
const formContentClass = computed(() => (formFields.length >= 30 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["accountTitleId","titleCode","changeFields","changeTime","changeBy","changeReason","extFieldJson","remark"]

interface Props {
  formData?: Partial<AccountTitleChangeLogCreate & { accountTitleChangeLogId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

const rules = computed<Record<string, Rule[]>>(() => ({
  accountTitleId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.accounttitleid') }),
      trigger: 'blur'
    }
  ],
  titleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.titlecode') }),
      trigger: 'blur'
    }
  ],
  changeTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitleChangeLog.changetime') }),
      trigger: 'blur'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  activeTab.value = 'tab-0'
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
