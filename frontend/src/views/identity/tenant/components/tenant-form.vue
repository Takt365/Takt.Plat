<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/identity/tenant/components -->
<!-- 文件名称：tenant-form.vue -->
<!-- 功能描述：租户实体 代表系统中的独立租户维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="tenant-form-tabs"
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
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.name')"
                name="tenantName"
              >
                <a-input
                  v-model:value="formState.tenantName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.subscriptionstarttime')"
                name="subscriptionStartTime"
              >
                <a-input
                  v-model:value="formState.subscriptionStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.subscriptionstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.subscriptionendtime')"
                name="subscriptionEndTime"
              >
                <a-input
                  v-model:value="formState.subscriptionEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.subscriptionendtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactname')"
                name="contactName"
              >
                <a-input
                  v-model:value="formState.contactName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactphone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactemail') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.isbuiltin')"
                name="isBuiltIn"
              >
                <a-input-number
                  v-model:value="formState.isBuiltIn"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.isbuiltin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.status')"
                name="tenantStatus"
              >
                <a-input-number
                  v-model:value="formState.tenantStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.userids')"
                name="userIds"
              >
                <a-input
                  v-model:value="formState.userIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.userids') })"
                  size="small"
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
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
 * 租户实体 代表系统中的独立租户维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/identity/tenant/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TenantCreate } from '@/types/identity/tenant'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","tenantName","subscriptionStartTime","subscriptionEndTime","contactName","contactPhone","contactEmail","isBuiltIn","tenantStatus","userIds","extFieldJson","remark"]


interface Props {
  formData?: Partial<TenantCreate & { tenantId?: string }> | null
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

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.tenantId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  tenantName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.name') }),
      trigger: 'blur'
    }
  ],
  subscriptionStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.subscriptionstarttime') }),
      trigger: 'blur'
    }
  ],
  subscriptionEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.subscriptionendtime') }),
      trigger: 'blur'
    }
  ],
  contactEmail: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.contactemail') }),
      trigger: 'blur'
    }
  ],
  isBuiltIn: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.tenant.isbuiltin') }),
      trigger: 'change'
    }
  ],
  tenantStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.tenant.status') }),
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
