<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee/components -->
<!-- 文件名称：employee-form.vue -->
<!-- 功能描述：员工实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form employee-form flex flex-col min-h-0 overflow-visible"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('employeeCode')"
                name="employeeCode"
              >
                <a-input
                  v-model:value="formState.employeeCode"
                  :placeholder="pi.ph('employeeCode')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  :disabled="!!formData?.employeeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('employeeName')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="pi.ph('employeeName')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('gender')"
                name="gender"
              >
                <TaktSelect
                  v-model:value="formState.gender"
                  dict-type="sys_user_gender_category"
                  :placeholder="pi.ph('gender')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('birthDate')"
                name="birthDate"
              >
                <a-date-picker
                  v-model:value="formState.birthDate"
                  :placeholder="pi.ph('birthDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('idCardNo')"
                name="idCardNo"
              >
                <a-input
                  v-model:value="formState.idCardNo"
                  :placeholder="pi.ph('idCardNo')"
                  show-count
                  :maxlength="18"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mobile')"
                name="mobile"
              >
                <a-input
                  v-model:value="formState.mobile"
                  :placeholder="pi.ph('mobile')"
                  show-count
                  :maxlength="11"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('email')"
                name="email"
              >
                <a-input
                  v-model:value="formState.email"
                  :placeholder="pi.ph('email')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('nativePlace')"
                name="nativePlace"
              >
                <TaktSelect
                  v-model:value="formState.nativePlace"
                  dict-type="hr_native_place_code"
                  :placeholder="pi.ph('nativePlace')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ethnicity')"
                name="ethnicity"
              >
                <TaktSelect
                  v-model:value="formState.ethnicity"
                  dict-type="hr_ethnic_code"
                  :placeholder="pi.ph('ethnicity')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('politicalAffiliation')"
                name="politicalAffiliation"
              >
                <TaktSelect
                  v-model:value="formState.politicalAffiliation"
                  dict-type="hr_political_affiliation"
                  :placeholder="pi.ph('politicalAffiliation')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('maritalStatus')"
                name="maritalStatus"
              >
                <TaktSelect
                  v-model:value="formState.maritalStatus"
                  dict-type="hr_marital_status"
                  :placeholder="pi.ph('maritalStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('employeeStatus')"
                name="employeeStatus"
              >
                <TaktSelect
                  v-model:value="formState.employeeStatus"
                  dict-type="hr_employee_status"
                  :placeholder="pi.ph('employeeStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isBuiltIn')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isBuiltIn')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('avatar')"
                name="avatar"
              >
                <a-input
                  v-model:value="formState.avatar"
                  :placeholder="pi.ph('avatar')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('employeeDeptIds')"
                name="employeeDeptIds"
              >
                <a-input
                  v-model:value="formState.employeeDeptIds"
                  :placeholder="pi.ph('employeeDeptIds')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('employeePostIds')"
                name="employeePostIds"
              >
                <a-input
                  v-model:value="formState.employeePostIds"
                  :placeholder="pi.ph('employeePostIds')"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 employeeAddresses -->
    <TaktEditableTable
      ref="employeeAddressTableRef"
      v-model="childEmployeeAddressRows"
      :columns="employeeAddressFormColumns"
      :title="employeeAddressPi.self()"
      :add-button-entity="employeeAddressPi.self()"
      id-field="employeeAddressId"
      :default-row="createDefaultEmployeeAddressRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-addressType="{ record }">
        <TaktSelect
          v-model:value="record.addressType"
          dict-type="hr_employee_address_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="employeeAddressPi.ph('addressType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-country="{ record }">
        <TaktSelect
          v-model:value="record.country"
          dict-type="sys_country_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="employeeAddressPi.ph('country')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-province="{ record }">
        <TaktSelect
          v-model:value="record.province"
          api-url="TaktAdminDivisions/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="employeeAddressPi.queryPh('province', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-city="{ record }">
        <TaktSelect
          v-model:value="record.city"
          api-url="TaktAdminDivisions/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="employeeAddressPi.queryPh('city', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-district="{ record }">
        <TaktSelect
          v-model:value="record.district"
          api-url="TaktAdminDivisions/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="employeeAddressPi.queryPh('district', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 员工实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useEmployeeI18n } from '../composables/use-employee-i18n'

/** 实体字段 i18n */
const pi = useEmployeeI18n()

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeCode","employeeName","gender","birthDate","idCardNo","mobile","email","nativePlace","ethnicity","politicalAffiliation","maritalStatus","employeeStatus","isBuiltIn","avatar","employeeDeptIds","employeePostIds","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useEmployeeAddressI18n } from '../composables/use-employee-address-i18n'

const employeeAddressPi = useEmployeeAddressI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childEmployeeAddressRows = ref<Record<string, unknown>[]>([])
const employeeAddressTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 employeeAddress 可编辑列 */
const employeeAddressFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'employeeName',
    title: employeeAddressPi.label('employeeName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'addressType',
    title: employeeAddressPi.label('addressType'),
    width: 140,
  },
  {
    key: 'country',
    title: employeeAddressPi.label('country'),
    width: 140,
  },
  {
    key: 'province',
    title: employeeAddressPi.label('province'),
    width: 140,
  },
  {
    key: 'city',
    title: employeeAddressPi.label('city'),
    width: 140,
  },
  {
    key: 'district',
    title: employeeAddressPi.label('district'),
    width: 140,
  },
  {
    key: 'address1',
    title: employeeAddressPi.label('address1'),
    editor: 'textarea',
    rows: 1,
    placeholder: employeeAddressPi.ph('address1'),
    width: 180,
  },
  {
    key: 'address2',
    title: employeeAddressPi.label('address2'),
    editor: 'textarea',
    rows: 1,
    placeholder: employeeAddressPi.ph('address2'),
    width: 180,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<EmployeeCreate & { employeeId?: string }> | null | undefined) {
  const rows_employeeAddress = ((val as any)?.employeeAddresses ?? []) as Record<string, unknown>[]
  childEmployeeAddressRows.value = rows_employeeAddress
}

function createDefaultEmployeeAddressRow(): Record<string, unknown> {
  return {
    employeeName: '',
    addressType: 0,
    country: '',
    province: '',
    city: '',
    district: '',
    address1: '',
    address2: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.employeeId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    employeeAddresses: employeeAddressTableRef.value?.getRows?.() ?? childEmployeeAddressRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      employeeId: masterId,
    })),
  }
}

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
    delete (next as any).employeeAddresses
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
  employeeCode: [
    {
      required: true,
      message: pi.ph('employeeCode'),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: pi.ph('employeeName'),
      trigger: 'blur'
    }
  ],
  gender: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('gender'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('gender'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  birthDate: [
    {
      required: true,
      message: pi.ph('birthDate'),
      trigger: 'change'
    }
  ],
  idCardNo: [
    {
      required: true,
      message: pi.ph('idCardNo'),
      trigger: 'blur'
    }
  ],
  mobile: [
    {
      required: true,
      message: pi.ph('mobile'),
      trigger: 'blur'
    }
  ],
  nativePlace: [
    {
      required: true,
      message: pi.ph('nativePlace'),
      trigger: 'change'
    }
  ],
  ethnicity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ethnicity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ethnicity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  politicalAffiliation: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('politicalAffiliation'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('politicalAffiliation'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maritalStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maritalStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maritalStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  employeeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('employeeStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('employeeStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBuiltIn: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await employeeAddressTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('gender' in payload) {
    const rawgender = payload.gender
    payload.gender = typeof rawgender === 'number' ? rawgender : Number(rawgender)
  }
  if ('ethnicity' in payload) {
    const rawethnicity = payload.ethnicity
    payload.ethnicity = typeof rawethnicity === 'number' ? rawethnicity : Number(rawethnicity)
  }
  if ('politicalAffiliation' in payload) {
    const rawpoliticalAffiliation = payload.politicalAffiliation
    payload.politicalAffiliation = typeof rawpoliticalAffiliation === 'number' ? rawpoliticalAffiliation : Number(rawpoliticalAffiliation)
  }
  if ('maritalStatus' in payload) {
    const rawmaritalStatus = payload.maritalStatus
    payload.maritalStatus = typeof rawmaritalStatus === 'number' ? rawmaritalStatus : Number(rawmaritalStatus)
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
  childEmployeeAddressRows.value = []
  employeeAddressTableRef.value?.resetRows?.()
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
