<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/logging/delta-log/components -->
<!-- 文件名称：delta-log-form.vue -->
<!-- 功能描述：差异日志实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="delta-log-form-tabs"
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
                :label="t('entity.deltaLog.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.username') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.opertype')"
                name="operType"
              >
                <a-input
                  v-model:value="formState.operType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.opertype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.tablename')"
                name="tableName"
              >
                <a-input
                  v-model:value="formState.tableName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.tablename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.primarykeyid')"
                name="primaryKeyId"
              >
                <a-input
                  v-model:value="formState.primaryKeyId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.primarykeyid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.beforedata')"
                name="beforeData"
              >
                <a-input
                  v-model:value="formState.beforeData"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.beforedata') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.afterdata')"
                name="afterData"
              >
                <a-input
                  v-model:value="formState.afterData"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.afterdata') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.diffdata')"
                name="diffData"
              >
                <a-input
                  v-model:value="formState.diffData"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.diffdata') })"
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
                :label="t('entity.deltaLog.sqlstatement')"
                name="sqlStatement"
              >
                <a-input
                  v-model:value="formState.sqlStatement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.sqlstatement') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.operip')"
                name="operIp"
              >
                <a-input
                  v-model:value="formState.operIp"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.operip') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.operlocation')"
                name="operLocation"
              >
                <a-input
                  v-model:value="formState.operLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.operlocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.opertime')"
                name="operTime"
              >
                <a-input
                  v-model:value="formState.operTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.opertime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.deltaLog.elapsedtime')"
                name="elapsedTime"
              >
                <a-input-number
                  v-model:value="formState.elapsedTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltaLog.elapsedtime') })"
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
 * 差异日志实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/logging/delta-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DeltaLogCreate } from '@/types/statistics/logging/delta-log'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","userName","operType","tableName","primaryKeyId","beforeData","afterData","diffData","sqlStatement","operIp","operLocation","operTime","elapsedTime","extFieldJson","remark"]


interface Props {
  formData?: Partial<DeltaLogCreate & { deltaLogId?: string }> | null
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
    const isCreate = !props.formData?.deltaLogId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  userName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.deltaLog.username') }),
      trigger: 'blur'
    }
  ],
  operType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.deltaLog.opertype') }),
      trigger: 'blur'
    }
  ],
  tableName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.deltaLog.tablename') }),
      trigger: 'blur'
    }
  ],
  operTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.deltaLog.opertime') }),
      trigger: 'blur'
    }
  ],
  elapsedTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.deltaLog.elapsedtime') }),
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
