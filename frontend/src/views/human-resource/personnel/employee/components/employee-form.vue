<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee/components -->
<!-- 文件名称：employee-form.vue -->
<!-- 功能描述：员工实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="employee-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.employee.no')"
                name="employeeNo"
              >
                <a-input
                  v-model:value="formState.employeeNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.no') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.name')"
                name="name"
              >
                <a-input
                  v-model:value="formState.name"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.gender')"
                name="gender"
              >
                <a-input-number
                  v-model:value="formState.gender"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.gender') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.birthdate')"
                name="birthDate"
              >
                <a-date-picker
                  v-model:value="formState.birthDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.birthdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.idcardno')"
                name="idCardNo"
              >
                <a-input
                  v-model:value="formState.idCardNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.idcardno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.mobile')"
                name="mobile"
              >
                <a-input
                  v-model:value="formState.mobile"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.mobile') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.email')"
                name="email"
              >
                <a-input
                  v-model:value="formState.email"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.email') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.nativeplace')"
                name="nativePlace"
              >
                <TaktSelect
                  v-model:value="formState.nativePlace"
                  dict-type="hr_native_place"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.nativeplace') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.ethnicity')"
                name="ethnicity"
              >
                <TaktSelect
                  v-model:value="formState.ethnicity"
                  dict-type="hr_ethnic_group"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.ethnicity') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.politicalstatus')"
                name="politicalStatus"
              >
                <TaktSelect
                  v-model:value="formState.politicalStatus"
                  dict-type="hr_political_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.politicalstatus') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.maritalstatus')"
                name="maritalStatus"
              >
                <a-input-number
                  v-model:value="formState.maritalStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.maritalstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.education')"
                name="education"
              >
                <a-input-number
                  v-model:value="formState.education"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.education') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.graduateschool')"
                name="graduateSchool"
              >
                <a-input
                  v-model:value="formState.graduateSchool"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.graduateschool') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.major')"
                name="major"
              >
                <a-input
                  v-model:value="formState.major"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.major') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.joineddate')"
                name="joinedDate"
              >
                <a-date-picker
                  v-model:value="formState.joinedDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.joineddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.probationenddate')"
                name="probationEndDate"
              >
                <a-date-picker
                  v-model:value="formState.probationEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.probationenddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.regulardate')"
                name="regularDate"
              >
                <a-date-picker
                  v-model:value="formState.regularDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.regulardate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.terminationdate')"
                name="terminationDate"
              >
                <a-date-picker
                  v-model:value="formState.terminationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.terminationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.lastworkdate')"
                name="lastWorkDate"
              >
                <a-date-picker
                  v-model:value="formState.lastWorkDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.lastworkdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.resignationtype')"
                name="resignationType"
              >
                <a-input-number
                  v-model:value="formState.resignationType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.resignationtype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.resignationreason')"
                name="resignationReason"
              >
                <a-input
                  v-model:value="formState.resignationReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.resignationreason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.status')"
                name="employeeStatus"
              >
                <a-input-number
                  v-model:value="formState.employeeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.primarydeptid')"
                name="primaryDeptId"
              >
                <a-input
                  v-model:value="formState.primaryDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.primarydeptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.primarypostid')"
                name="primaryPostId"
              >
                <a-input
                  v-model:value="formState.primaryPostId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.primarypostid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.isbuiltin')"
                name="isBuiltIn"
              >
                <a-input-number
                  v-model:value="formState.isBuiltIn"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.isbuiltin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.emergencycontactname')"
                name="emergencyContactName"
              >
                <a-input
                  v-model:value="formState.emergencyContactName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.emergencycontactphone')"
                name="emergencyContactPhone"
              >
                <a-input
                  v-model:value="formState.emergencyContactPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactphone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.employee.homeaddress')"
                name="homeAddress"
              >
                <a-textarea
                  v-model:value="formState.homeAddress"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.employee.homeaddress') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.photourl')"
                name="photoUrl"
              >
                <a-input
                  v-model:value="formState.photoUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.photourl') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.deptids')"
                name="employeeDeptIds"
              >
                <a-input
                  v-model:value="formState.employeeDeptIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.deptids') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.postids')"
                name="employeePostIds"
              >
                <a-input
                  v-model:value="formState.employeePostIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.postids') })"
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
 * 员工实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/personnel/employee/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeCreate } from '@/types/human-resource/personnel/employee'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeNo","name","gender","birthDate","idCardNo","mobile","email","nativePlace","ethnicity","politicalStatus","maritalStatus","education","graduateSchool","major","joinedDate","probationEndDate","regularDate","terminationDate","lastWorkDate","resignationType","resignationReason","employeeStatus","primaryDeptId","primaryPostId","isBuiltIn","emergencyContactName","emergencyContactPhone","homeAddress","photoUrl","employeeDeptIds","employeePostIds","extFieldJson","remark"]


interface Props {
  formData?: Partial<EmployeeCreate & { employeeId?: string }> | null
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
    const isCreate = !props.formData?.employeeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  employeeNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.no') }),
      trigger: 'blur'
    }
  ],
  name: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.name') }),
      trigger: 'blur'
    }
  ],
  gender: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employee.gender') }),
      trigger: 'change'
    }
  ],
  employeeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employee.status') }),
      trigger: 'change'
    }
  ],
  isBuiltIn: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employee.isbuiltin') }),
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
