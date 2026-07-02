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
    class="takt-generated-form"
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="6"
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
                  show-count
                  :maxlength="80"
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
                  show-count
                  :maxlength="18"
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
                  show-count
                  :maxlength="11"
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
                  show-count
                  :maxlength="100"
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.nativeplace') })"
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
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.ethnicity') })"
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
                  show-count
                  :maxlength="100"
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
                  show-count
                  :maxlength="50"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.employee.isbuiltin')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.isbuiltin') })"
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
                  show-count
                  :maxlength="50"
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
                  show-count
                  :maxlength="20"
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
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.employee.avatar')"
                name="avatar"
              >
                <a-input
                  v-model:value="formState.avatar"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.avatar') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.employee.deptids')"
                name="employeeDeptIds"
              >
                <a-input
                  v-model:value="formState.employeeDeptIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.deptids') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.employee.postids')"
                name="employeePostIds"
              >
                <a-input
                  v-model:value="formState.employeePostIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.postids') })"
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
 * 员工实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeCreate } from '@/types/human-resource/personnel/employee'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeNo","name","gender","birthDate","idCardNo","mobile","email","nativePlace","ethnicity","politicalStatus","maritalStatus","education","graduateSchool","major","joinedDate","probationEndDate","regularDate","terminationDate","lastWorkDate","resignationType","resignationReason","employeeStatus","primaryDeptId","primaryPostId","isBuiltIn","emergencyContactName","emergencyContactPhone","homeAddress","avatar","employeeDeptIds","employeePostIds","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EmployeeCreate & { employeeId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  isBuiltIn: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 employeeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.employeeId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
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
  gender: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.gender') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.gender') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  birthDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employee.birthdate') }),
      trigger: 'change'
    }
  ],
  idCardNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.idcardno') }),
      trigger: 'blur'
    }
  ],
  mobile: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.mobile') }),
      trigger: 'blur'
    }
  ],
  nativePlace: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.employee.nativeplace') }),
      trigger: 'change'
    }
  ],
  ethnicity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.ethnicity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.ethnicity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  politicalStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.politicalstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.politicalstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maritalStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.maritalstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.maritalstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  employeeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBuiltIn: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.isbuiltin') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.employee.isbuiltin') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  emergencyContactName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactname') }),
      trigger: 'blur'
    }
  ],
  emergencyContactPhone: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactphone') }),
      trigger: 'blur'
    }
  ],
  homeAddress: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.employee.homeaddress') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('gender' in payload) {
    const rawgender = payload.gender
    payload.gender = typeof rawgender === 'number' ? rawgender : Number(rawgender)
  }
  if ('ethnicity' in payload) {
    const rawethnicity = payload.ethnicity
    payload.ethnicity = typeof rawethnicity === 'number' ? rawethnicity : Number(rawethnicity)
  }
  if ('politicalStatus' in payload) {
    const rawpoliticalStatus = payload.politicalStatus
    payload.politicalStatus = typeof rawpoliticalStatus === 'number' ? rawpoliticalStatus : Number(rawpoliticalStatus)
  }
  if ('maritalStatus' in payload) {
    const rawmaritalStatus = payload.maritalStatus
    payload.maritalStatus = typeof rawmaritalStatus === 'number' ? rawmaritalStatus : Number(rawmaritalStatus)
  }
  if ('education' in payload) {
    const raweducation = payload.education
    payload.education = typeof raweducation === 'number' ? raweducation : Number(raweducation)
  }
  if ('resignationType' in payload) {
    const rawresignationType = payload.resignationType
    payload.resignationType = typeof rawresignationType === 'number' ? rawresignationType : Number(rawresignationType)
  }
  if ('employeeStatus' in payload) {
    const rawemployeeStatus = payload.employeeStatus
    payload.employeeStatus = typeof rawemployeeStatus === 'number' ? rawemployeeStatus : Number(rawemployeeStatus)
  }
  if ('isBuiltIn' in payload) {
    const rawisBuiltIn = payload.isBuiltIn
    payload.isBuiltIn = typeof rawisBuiltIn === 'number' ? rawisBuiltIn : Number(rawisBuiltIn)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.employeeId)

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
