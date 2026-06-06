<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint/components -->
<!-- 文件名称：customer-complaint-form.vue -->
<!-- 功能描述：客诉主表实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="customer-complaint-form-tabs"
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
                :label="t('entity.customerComplaint.code')"
                name="customerComplaintCode"
              >
                <a-input
                  v-model:value="formState.customerComplaintCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.customerid')"
                name="customerId"
              >
                <a-input
                  v-model:value="formState.customerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.complaintdate')"
                name="complaintDate"
              >
                <a-date-picker
                  v-model:value="formState.complaintDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.complaintmethod')"
                name="complaintMethod"
              >
                <a-input-number
                  v-model:value="formState.complaintMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.complainttype')"
                name="complaintType"
              >
                <a-input-number
                  v-model:value="formState.complaintType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complainttype') })"
                  size="small"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.complaintlevel')"
                name="complaintLevel"
              >
                <a-input-number
                  v-model:value="formState.complaintLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintlevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.responsibledeptid')"
                name="responsibleDeptId"
              >
                <a-input
                  v-model:value="formState.responsibleDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsibledeptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.responsibledeptname')"
                name="responsibleDeptName"
              >
                <a-input
                  v-model:value="formState.responsibleDeptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsibledeptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.responsiblepersonid')"
                name="responsiblePersonId"
              >
                <a-input
                  v-model:value="formState.responsiblePersonId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsiblepersonid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.responsiblepersonname')"
                name="responsiblePersonName"
              >
                <a-input
                  v-model:value="formState.responsiblePersonName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.responsiblepersonname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.requiredreplydate')"
                name="requiredReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredReplyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.requiredreplydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.actualreplydate')"
                name="actualReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.actualReplyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.actualreplydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.complaintstatus')"
                name="complaintStatus"
              >
                <a-input-number
                  v-model:value="formState.complaintStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customerComplaint.complaintdescription')"
                name="complaintDescription"
              >
                <a-textarea
                  v-model:value="formState.complaintDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerComplaint.complaintdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.handlingresult')"
                name="handlingResult"
              >
                <a-input
                  v-model:value="formState.handlingResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.handlingresult') })"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.customersatisfaction')"
                name="customerSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.customerSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customersatisfaction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaint.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.items') })"
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
 * 客诉主表实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerComplaintCreate } from '@/types/logistics/quality/complaint/customer-complaint'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","customerComplaintCode","customerId","customerName","customerCode","complaintDate","complaintMethod","complaintType","complaintLevel","responsibleDeptId","responsibleDeptName","responsiblePersonId","responsiblePersonName","requiredReplyDate","actualReplyDate","complaintStatus","complaintDescription","handlingResult","customerSatisfaction","relatedPlant","sortOrder","items","extFieldJson","remark"]


interface Props {
  formData?: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null
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
    const isCreate = !props.formData?.customerComplaintId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  customerComplaintCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.code') }),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customerid') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.customername') }),
      trigger: 'blur'
    }
  ],
  complaintDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintdate') }),
      trigger: 'change'
    }
  ],
  complaintMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintmethod') }),
      trigger: 'change'
    }
  ],
  complaintType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complainttype') }),
      trigger: 'change'
    }
  ],
  complaintLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintlevel') }),
      trigger: 'change'
    }
  ],
  complaintStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.complaintstatus') }),
      trigger: 'change'
    }
  ],
  complaintDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaint.complaintdescription') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaint.sortorder') }),
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
