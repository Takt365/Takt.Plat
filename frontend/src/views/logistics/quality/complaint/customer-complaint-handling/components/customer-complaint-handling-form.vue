<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-handling/components -->
<!-- 文件名称：customer-complaint-handling-form.vue -->
<!-- 功能描述：客诉处理记录实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="customer-complaint-handling-form-tabs"
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
                :label="t('entity.customerComplaintHandling.complainthandlingcode')"
                name="complaintHandlingCode"
              >
                <a-input
                  v-model:value="formState.complaintHandlingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complainthandlingcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.complaintid')"
                name="complaintId"
              >
                <a-input
                  v-model:value="formState.complaintId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.complaintno')"
                name="complaintNo"
              >
                <a-input
                  v-model:value="formState.complaintNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.complaintitemid')"
                name="complaintItemId"
              >
                <a-input
                  v-model:value="formState.complaintItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintitemid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingstage')"
                name="handlingStage"
              >
                <a-input-number
                  v-model:value="formState.handlingStage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingstage') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingmethod')"
                name="handlingMethod"
              >
                <a-input-number
                  v-model:value="formState.handlingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingdescription')"
                name="handlingDescription"
              >
                <a-textarea
                  v-model:value="formState.handlingDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerComplaintHandling.handlingdescription') })"
                  :rows="2"
                  size="small"
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
                :label="t('entity.customerComplaintHandling.causeanalysis')"
                name="causeAnalysis"
              >
                <a-input
                  v-model:value="formState.causeAnalysis"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.causeanalysis') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.correctiveaction')"
                name="correctiveAction"
              >
                <a-input
                  v-model:value="formState.correctiveAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.correctiveaction') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.preventiveaction')"
                name="preventiveAction"
              >
                <a-input
                  v-model:value="formState.preventiveAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.preventiveaction') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.responsibledept')"
                name="responsibleDept"
              >
                <a-input
                  v-model:value="formState.responsibleDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.responsibledept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.responsibleby')"
                name="responsibleBy"
              >
                <a-input
                  v-model:value="formState.responsibleBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.responsibleby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlerby')"
                name="handlerBy"
              >
                <a-input
                  v-model:value="formState.handlerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlerby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingat')"
                name="handlingAt"
              >
                <a-input
                  v-model:value="formState.handlingAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.plannedcompletiondate')"
                name="plannedCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedCompletionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.plannedcompletiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.actualcompletiondate')"
                name="actualCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.actualCompletionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.actualcompletiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingstatus')"
                name="handlingStatus"
              >
                <a-input-number
                  v-model:value="formState.handlingStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingstatus') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.handlingcost')"
                name="handlingCost"
              >
                <a-input-number
                  v-model:value="formState.handlingCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.customerfeedback') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.customersatisfaction')"
                name="customerSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.customerSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.customersatisfaction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerComplaintHandling.attachmentpaths')"
                name="attachmentPaths"
              >
                <a-input
                  v-model:value="formState.attachmentPaths"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.attachmentpaths') })"
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
 * 客诉处理记录实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint-handling/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerComplaintHandlingCreate } from '@/types/logistics/quality/complaint/customer-complaint-handling'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","complaintHandlingCode","complaintId","complaintNo","complaintItemId","handlingStage","handlingMethod","handlingDescription","causeAnalysis","correctiveAction","preventiveAction","responsibleDept","responsibleBy","handlerBy","handlingAt","plannedCompletionDate","actualCompletionDate","handlingStatus","handlingCost","customerFeedback","customerSatisfaction","attachmentPaths","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerComplaintHandlingCreate & { customerComplaintHandlingId?: string }> | null
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
    const isCreate = !props.formData?.customerComplaintHandlingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  complaintHandlingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complainthandlingcode') }),
      trigger: 'blur'
    }
  ],
  complaintId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintid') }),
      trigger: 'blur'
    }
  ],
  complaintNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.complaintno') }),
      trigger: 'blur'
    }
  ],
  handlingStage: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.handlingstage') }),
      trigger: 'change'
    }
  ],
  handlingMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.handlingmethod') }),
      trigger: 'change'
    }
  ],
  handlingDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerComplaintHandling.handlingdescription') }),
      trigger: 'blur'
    }
  ],
  handlingStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerComplaintHandling.handlingstatus') }),
      trigger: 'change'
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
  return { ...formState }
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
