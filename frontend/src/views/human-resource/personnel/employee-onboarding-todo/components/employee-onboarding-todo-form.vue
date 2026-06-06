<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-onboarding-todo/components -->
<!-- 文件名称：employee-onboarding-todo-form.vue -->
<!-- 功能描述：入职待办维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="employee-onboarding-todo-form-tabs"
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
                :label="t('entity.employeeOnboardingTodo.offerid')"
                name="offerId"
              >
                <a-input
                  v-model:value="formState.offerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.offerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.todono')"
                name="todoNo"
              >
                <a-input
                  v-model:value="formState.todoNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.todono') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.todostatus')"
                name="todoStatus"
              >
                <a-input-number
                  v-model:value="formState.todoStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.todostatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.plannedjoineddate')"
                name="plannedJoinedDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedJoinedDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeOnboardingTodo.plannedjoineddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.candidatename')"
                name="candidateName"
              >
                <a-date-picker
                  v-model:value="formState.candidateName"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeOnboardingTodo.candidatename') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.mobile')"
                name="mobile"
              >
                <a-input
                  v-model:value="formState.mobile"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.mobile') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.employeeid') })"
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
                :label="t('entity.employeeOnboardingTodo.employeejoinedid')"
                name="employeeJoinedId"
              >
                <a-input
                  v-model:value="formState.employeeJoinedId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.employeejoinedid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeOnboardingTodo.reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.reason') })"
                  size="small"
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
 * 入职待办维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/personnel/employee-onboarding-todo/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeOnboardingTodoCreate } from '@/types/human-resource/personnel/employee-onboarding-todo'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","offerId","todoNo","todoStatus","plannedJoinedDate","candidateName","mobile","employeeId","employeeJoinedId","reason","extFieldJson","remark"]


interface Props {
  formData?: Partial<EmployeeOnboardingTodoCreate & { employeeOnboardingTodoId?: string }> | null
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
    const isCreate = !props.formData?.employeeOnboardingTodoId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  offerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.offerid') }),
      trigger: 'blur'
    }
  ],
  todoNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeOnboardingTodo.todono') }),
      trigger: 'blur'
    }
  ],
  todoStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employeeOnboardingTodo.todostatus') }),
      trigger: 'change'
    }
  ],
  plannedJoinedDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employeeOnboardingTodo.plannedjoineddate') }),
      trigger: 'change'
    }
  ],
  candidateName: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employeeOnboardingTodo.candidatename') }),
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
