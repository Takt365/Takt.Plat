<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee/components -->
<!-- 文件名称：employee-address-form.vue -->
<!-- 功能描述：员工实体子表 employeeAddress 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form employee-address-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="employee-address-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
                :label="pi.label('addressType')"
                name="addressType"
              >
                <TaktSelect
                  v-model:value="formState.addressType"
                  dict-type="hr_employee_address_type"
                  :placeholder="pi.ph('addressType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('country')"
                name="country"
              >
                <TaktSelect
                  v-model:value="formState.country"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('country')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('province')"
                name="province"
              >
                <TaktSelect
                  v-model:value="formState.province"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('province')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('city')"
                name="city"
              >
                <TaktSelect
                  v-model:value="formState.city"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('city')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('district')"
                name="district"
              >
                <TaktSelect
                  v-model:value="formState.district"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('district')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address1')"
                name="address1"
              >
                <a-textarea
                  v-model:value="formState.address1"
                  :placeholder="pi.ph('address1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address2')"
                name="address2"
              >
                <a-textarea
                  v-model:value="formState.address2"
                  :placeholder="pi.ph('address2')"
                  :rows="2"
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
 * 员工实体子表 employeeAddress 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/human-resource/personnel/employee/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useEmployeeAddressI18n } from '../composables/use-employee-address-i18n'

/** 实体字段 i18n */
const pi = useEmployeeAddressI18n()

import type { EmployeeAddressCreate } from '@/types/human-resource/personnel/employee-address'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["employeeName","addressType","country","province","city","district","address1","address2"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EmployeeAddressCreate & { employeeAddressId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  country: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 employeeAddressId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.employeeAddressId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeName: [
    {
      required: true,
      message: pi.ph('employeeName'),
      trigger: 'blur'
    }
  ],
  addressType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('addressType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('addressType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  country: [
    {
      required: true,
      message: pi.ph('country'),
      trigger: 'change'
    }
  ],
  province: [
    {
      required: true,
      message: pi.ph('province'),
      trigger: 'change'
    }
  ],
  city: [
    {
      required: true,
      message: pi.ph('city'),
      trigger: 'change'
    }
  ],
  district: [
    {
      required: true,
      message: pi.ph('district'),
      trigger: 'change'
    }
  ],
  address1: [
    {
      required: true,
      message: pi.ph('address1'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 employeeId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('addressType' in payload) {
    const rawaddressType = payload.addressType
    payload.addressType = typeof rawaddressType === 'number' ? rawaddressType : Number(rawaddressType)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.employeeId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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
