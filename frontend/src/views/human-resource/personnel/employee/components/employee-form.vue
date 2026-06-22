<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee/components -->
<!-- 文件名称：employee-form.vue -->
<!-- 功能描述：员工实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
                  :placeholder="requiredPlaceholder('entity.employee.birthdate')"
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
                  :placeholder="requiredPlaceholder('entity.employee.idcardno')"
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
                  :placeholder="requiredPlaceholder('entity.employee.mobile')"
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
                  :placeholder="optionalPlaceholder('entity.employee.email')"
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
                  dict-type="hr_native_place_code"
                  :placeholder="requiredSelectPlaceholder('entity.employee.nativeplace')"
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
                  dict-type="hr_ethnic_code"
                  :placeholder="requiredSelectPlaceholder('entity.employee.ethnicity')"
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
                  :placeholder="requiredSelectPlaceholder('entity.employee.politicalstatus')"
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
                  :placeholder="optionalPlaceholder('entity.employee.graduateschool')"
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
                  :placeholder="optionalPlaceholder('entity.employee.major')"
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
                  disabled
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
                  disabled
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
                  disabled
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
                  disabled
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
                  disabled
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.resignationtype') })"
                  size="small"
                  style="width: 100%"
                  disabled
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.resignationreason') })"
                  size="small"
                  allow-clear
                  disabled
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.primarydeptid') })"
                  size="small"
                  allow-clear
                  disabled
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.primarypostid') })"
                  size="small"
                  allow-clear
                  disabled
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
                  :placeholder="requiredPlaceholder('entity.employee.emergencycontactname')"
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
                  :placeholder="requiredPlaceholder('entity.employee.emergencycontactphone')"
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
                  :placeholder="requiredPlaceholder('entity.employee.homeaddress')"
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
                  :placeholder="optionalPlaceholder('entity.employee.photourl')"
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
                  :placeholder="optionalPlaceholder('entity.employee.deptids')"
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
                  :placeholder="optionalPlaceholder('entity.employee.postids')"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.ExtField')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
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
 * 员工实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeCreate } from '@/types/human-resource/personnel/employee'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { isValidIdCard, isValidPhone } from '@/utils/regex'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 任职/离职投影字段（只读，由上岗/调动/离职审批回写） */
const projectionManagedFields = [
  'joinedDate',
  'probationEndDate',
  'regularDate',
  'terminationDate',
  'lastWorkDate',
  'resignationType',
  'resignationReason',
  'primaryDeptId',
  'primaryPostId',
] as const

/**
 * 选填字段占位文案
 * @param entityKey entity.* 翻译键
 * @returns {string} 占位符
 */
function optionalPlaceholder(entityKey: string) {
  return t('common.page.form.placeholder.optional', { field: t(entityKey) })
}

/**
 * 必填字段占位文案
 * @param entityKey entity.* 翻译键
 * @returns {string} 占位符
 */
function requiredPlaceholder(entityKey: string) {
  return t('common.page.form.placeholder.required', { field: t(entityKey) })
}

/**
 * 必填字典选择占位文案
 * @param entityKey entity.* 翻译键
 * @returns {string} 占位符
 */
function requiredSelectPlaceholder(entityKey: string) {
  return t('common.page.form.placeholder.select', { field: t(entityKey) })
}

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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeNo","name","gender","birthDate","idCardNo","mobile","email","nativePlace","ethnicity","politicalStatus","maritalStatus","education","graduateSchool","major","joinedDate","probationEndDate","regularDate","terminationDate","lastWorkDate","resignationType","resignationReason","employeeStatus","primaryDeptId","primaryPostId","isBuiltIn","emergencyContactName","emergencyContactPhone","homeAddress","photoUrl","employeeDeptIds","employeePostIds","ExtField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EmployeeCreate & { employeeId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.employeeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeNo: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.no') }),
      trigger: 'blur'
    }
  ],
  name: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.name') }),
      trigger: 'blur'
    }
  ],
  gender: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.gender') }),
      trigger: 'change'
    }
  ],
  birthDate: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.birthdate') }),
      trigger: 'change'
    }
  ],
  idCardNo: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.idcardno') }),
      trigger: 'blur'
    },
    {
      validator: async (_rule, value: string) => {
        if (!value || isValidIdCard(value)) return
        return Promise.reject(t('common.validation.invalidformat', { field: t('entity.employee.idcardno') }))
      },
      trigger: 'blur'
    }
  ],
  mobile: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.mobile') }),
      trigger: 'blur'
    },
    {
      validator: async (_rule, value: string) => {
        if (!value || isValidPhone(value, formState.companyDefaultCulture as string)) return
        return Promise.reject(t('common.validation.invalidformat', { field: t('entity.employee.mobile') }))
      },
      trigger: 'blur'
    }
  ],
  nativePlace: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.nativeplace') }),
      trigger: 'change'
    },
    {
      validator: async (_rule, value: string) => {
        if (!value || /^\d{6}$/.test(value)) return
        return Promise.reject(t('common.validation.invalidformat', { field: t('entity.employee.nativeplace') }))
      },
      trigger: 'change'
    }
  ],
  ethnicity: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.ethnicity') }),
      trigger: 'change'
    }
  ],
  politicalStatus: [
    {
      validator: async (_rule, value: number | null | undefined) => {
        if (value === undefined || value === null) {
          return Promise.reject(t('common.validation.required', { field: t('entity.employee.politicalstatus') }))
        }
      },
      trigger: 'change'
    }
  ],
  maritalStatus: [
    {
      validator: async (_rule, value: number | null | undefined) => {
        if (value === undefined || value === null) {
          return Promise.reject(t('common.validation.required', { field: t('entity.employee.maritalstatus') }))
        }
      },
      trigger: 'change'
    }
  ],
  emergencyContactName: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.emergencycontactname') }),
      trigger: 'blur'
    }
  ],
  emergencyContactPhone: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.emergencycontactphone') }),
      trigger: 'blur'
    }
  ],
  homeAddress: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.homeaddress') }),
      trigger: 'blur'
    }
  ],
  employeeStatus: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.status') }),
      trigger: 'change'
    }
  ],
  isBuiltIn: [
    {
      required: true,
      message: t('common.validation.required', { field: t('entity.employee.isbuiltin') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（剔除投影只读字段，字典整型字段转 number） */
function getValues(): Record<string, any> {
  const result = { ...formState }
  for (const key of projectionManagedFields) {
    delete result[key]
  }
  for (const key of ['gender', 'ethnicity', 'politicalStatus', 'maritalStatus', 'education', 'employeeStatus', 'isBuiltIn', 'resignationType']) {
    const value = result[key]
    if (value !== undefined && value !== null && value !== '') {
      result[key] = Number(value)
    }
  }
  return result
}

/** 重置表单与子表行 */
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
