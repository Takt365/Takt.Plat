<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title/components -->
<!-- 文件名称：title-form.vue -->
<!-- 功能描述：会计科目实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="title-form-tabs"
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
                :label="t('entity.accountTitle.titlecode')"
                name="titleCode"
              >
                <a-input
                  v-model:value="formState.titleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlecode') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlename')"
                name="titleName"
              >
                <a-input
                  v-model:value="formState.titleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlename') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.parentid')"
                name="parentId"
              >
                <a-input
                  v-model:value="formState.parentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.parentid') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titletype')"
                name="titleType"
              >
                <a-input
                  v-model:value="formState.titleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titletype') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.balancedirection')"
                name="balanceDirection"
              >
                <a-input
                  v-model:value="formState.balanceDirection"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.balancedirection') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlelevel')"
                name="titleLevel"
              >
                <a-input
                  v-model:value="formState.titleLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlelevel') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.isauxiliary')"
                name="isAuxiliary"
              >
                <a-input
                  v-model:value="formState.isAuxiliary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isauxiliary') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.auxiliarytype')"
                name="auxiliaryType"
              >
                <a-input
                  v-model:value="formState.auxiliaryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.auxiliarytype') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.isquantity')"
                name="isQuantity"
              >
                <a-input
                  v-model:value="formState.isQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isquantity') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.iscurrency')"
                name="isCurrency"
              >
                <a-input
                  v-model:value="formState.isCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.iscurrency') })"
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
                :label="t('entity.accountTitle.iscash')"
                name="isCash"
              >
                <a-input
                  v-model:value="formState.isCash"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.iscash') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.isbank')"
                name="isBank"
              >
                <a-input
                  v-model:value="formState.isBank"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isbank') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.relatedplant') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlestatus')"
                name="titleStatus"
              >
                <a-input
                  v-model:value="formState.titleStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlestatus') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.validfrom')"
                name="validFrom"
              >
                <a-input
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validfrom') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.validto')"
                name="validTo"
              >
                <a-input
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validto') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.sortorder')"
                name="sortOrder"
              >
                <a-input
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.sortorder') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.extfieldjson') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.accountTitle.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.accountTitle.remark') })"
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
 * 会计科目实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/financial/account-title/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AccountTitleCreate } from '@/types/accounting/financial/account-title'

const { t } = useI18n()
const formContentClass = computed(() => (formFields.length >= 30 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["titleCode","titleName","parentId","titleType","balanceDirection","titleLevel","isAuxiliary","auxiliaryType","isQuantity","isCurrency","isCash","isBank","relatedPlant","titleStatus","validFrom","validTo","sortOrder","extFieldJson","remark"]

interface Props {
  formData?: Partial<AccountTitleCreate & { accountTitleId?: string }> | null
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
  titleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlecode') }),
      trigger: 'blur'
    }
  ],
  titleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlename') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.parentid') }),
      trigger: 'blur'
    }
  ],
  titleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titletype') }),
      trigger: 'change'
    }
  ],
  balanceDirection: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.balancedirection') }),
      trigger: 'change'
    }
  ],
  titleLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titlelevel') }),
      trigger: 'change'
    }
  ],
  isAuxiliary: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isauxiliary') }),
      trigger: 'change'
    }
  ],
  auxiliaryType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.auxiliarytype') }),
      trigger: 'change'
    }
  ],
  isQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isquantity') }),
      trigger: 'change'
    }
  ],
  isCurrency: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.iscurrency') }),
      trigger: 'change'
    }
  ],
  isCash: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.iscash') }),
      trigger: 'change'
    }
  ],
  isBank: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isbank') }),
      trigger: 'change'
    }
  ],
  titleStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titlestatus') }),
      trigger: 'change'
    }
  ],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validfrom') }),
      trigger: 'blur'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validto') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.sortorder') }),
      trigger: 'change'
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
