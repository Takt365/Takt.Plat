<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-reassignment/components -->
<!-- 文件名称：employee-reassignment-form.vue -->
<!-- 功能描述：员工调动记录维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="employee-reassignment-form-tabs"
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
                :label="t('entity.employeeReassignment.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.reassignmenttype')"
                name="reassignmentType"
              >
                <a-input-number
                  v-model:value="formState.reassignmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.reassignmenttype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.fromdeptid')"
                name="fromDeptId"
              >
                <a-input
                  v-model:value="formState.fromDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.fromdeptname')"
                name="fromDeptName"
              >
                <a-input
                  v-model:value="formState.fromDeptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.frompostid')"
                name="fromPostId"
              >
                <a-input
                  v-model:value="formState.fromPostId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.frompostid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.frompostname')"
                name="fromPostName"
              >
                <a-input
                  v-model:value="formState.fromPostName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.frompostname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.todeptid')"
                name="toDeptId"
              >
                <a-input
                  v-model:value="formState.toDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptid') })"
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
                :label="t('entity.employeeReassignment.todeptname')"
                name="toDeptName"
              >
                <a-input
                  v-model:value="formState.toDeptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.topostid')"
                name="toPostId"
              >
                <a-input
                  v-model:value="formState.toPostId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.topostid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.topostname')"
                name="toPostName"
              >
                <a-input
                  v-model:value="formState.toPostName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.topostname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employeeReassignment.reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.reason') })"
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
 * 员工调动记录维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/personnel/employee-reassignment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeReassignmentCreate } from '@/types/human-resource/personnel/employee-reassignment'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","reassignmentType","fromDeptId","fromDeptName","fromPostId","fromPostName","toDeptId","toDeptName","toPostId","toPostName","effectiveDate","reason","extFieldJson","remark"]


interface Props {
  formData?: Partial<EmployeeReassignmentCreate & { employeeReassignmentId?: string }> | null
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
    const isCreate = !props.formData?.employeeReassignmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.employeeid') }),
      trigger: 'blur'
    }
  ],
  reassignmentType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.reassignmenttype') }),
      trigger: 'change'
    }
  ],
  fromDeptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptid') }),
      trigger: 'blur'
    }
  ],
  fromDeptName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptname') }),
      trigger: 'blur'
    }
  ],
  toDeptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptid') }),
      trigger: 'blur'
    }
  ],
  toDeptName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptname') }),
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
