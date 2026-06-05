<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/statistics/logging/oper-log/components -->
<!-- 文件名称：oper-log-form.vue -->
<!-- 功能描述：操作日志实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="oper-log-form-tabs"
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
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.username') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.opermodule')"
                name="operModule"
              >
                <a-input
                  v-model:value="formState.operModule"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opermodule') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.opertype')"
                name="operType"
              >
                <a-input
                  v-model:value="formState.operType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opertype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.opermethod')"
                name="operMethod"
              >
                <a-input
                  v-model:value="formState.operMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opermethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.requestmethod')"
                name="requestMethod"
              >
                <a-input
                  v-model:value="formState.requestMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.requestmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.operurl')"
                name="operUrl"
              >
                <a-input
                  v-model:value="formState.operUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.operurl') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.requestparam')"
                name="requestParam"
              >
                <a-input
                  v-model:value="formState.requestParam"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.requestparam') })"
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
                :label="t('entity.operLog.jsonresult')"
                name="jsonResult"
              >
                <a-input
                  v-model:value="formState.jsonResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.jsonresult') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.operstatus')"
                name="operStatus"
              >
                <a-input-number
                  v-model:value="formState.operStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.operstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.errormsg')"
                name="errorMsg"
              >
                <a-input
                  v-model:value="formState.errorMsg"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.errormsg') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.operip')"
                name="operIp"
              >
                <a-input
                  v-model:value="formState.operIp"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.operip') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.operlocation')"
                name="operLocation"
              >
                <a-input
                  v-model:value="formState.operLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.operlocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.opertime')"
                name="operTime"
              >
                <a-input
                  v-model:value="formState.operTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.opertime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.operLog.elapsedtime')"
                name="elapsedTime"
              >
                <a-input-number
                  v-model:value="formState.elapsedTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operLog.elapsedtime') })"
                  size="small"
                  style="width: 100%"
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
 * 操作日志实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/logging/oper-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { OperLogCreate } from '@/types/statistics/logging/oper-log'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","userName","operModule","operType","operMethod","requestMethod","operUrl","requestParam","jsonResult","operStatus","errorMsg","operIp","operLocation","operTime","elapsedTime","extFieldJson","remark"]


interface Props {
  formData?: Partial<OperLogCreate & { operLogId?: string }> | null
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
    const isCreate = !props.formData?.operLogId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  userName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.operLog.username') }),
      trigger: 'blur'
    }
  ],
  operStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.operLog.operstatus') }),
      trigger: 'change'
    }
  ],
  operTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.operLog.opertime') }),
      trigger: 'blur'
    }
  ],
  elapsedTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.operLog.elapsedtime') }),
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
