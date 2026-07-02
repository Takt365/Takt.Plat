<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/staffing-requirement/components -->
<!-- 文件名称：staffing-requirement-form.vue -->
<!-- 功能描述：用人需求维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="staffing-requirement-form-tabs"
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
                :label="t('entity.talentstaffingrequirement.reqno')"
                name="reqNo"
              >
                <a-input
                  v-model:value="formState.reqNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reqno') })"
                  show-count
                  :maxlength="30"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.deptid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.postid')"
                name="postId"
              >
                <a-input
                  v-model:value="formState.postId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.postid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.jobgrade')"
                name="jobGrade"
              >
                <a-input
                  v-model:value="formState.jobGrade"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.jobgrade') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.requestqty')"
                name="requestQty"
              >
                <a-input-number
                  v-model:value="formState.requestQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.requestqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.headcounttype')"
                name="headcountType"
              >
                <a-input
                  v-model:value="formState.headcountType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.headcounttype') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.reasoncode')"
                name="reasonCode"
              >
                <a-input
                  v-model:value="formState.reasonCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reasoncode') })"
                  show-count
                  :maxlength="30"
                  allow-clear
                  :disabled="!!formData?.talentStaffingRequirementId"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.replaceemployeeid')"
                name="replaceEmployeeId"
              >
                <a-input
                  v-model:value="formState.replaceEmployeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.replaceemployeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.expectedonboarddate')"
                name="expectedOnboardDate"
              >
                <a-date-picker
                  v-model:value="formState.expectedOnboardDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.expectedonboarddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.contracttype')"
                name="contractType"
              >
                <a-input
                  v-model:value="formState.contractType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.contracttype') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.worklocation')"
                name="workLocation"
              >
                <a-input
                  v-model:value="formState.workLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.worklocation') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.jobdesc')"
                name="jobDesc"
              >
                <a-input
                  v-model:value="formState.jobDesc"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.jobdesc') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.qualification')"
                name="qualification"
              >
                <a-input
                  v-model:value="formState.qualification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.qualification') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.talentstaffingrequirement.budgetyear')"
                name="budgetYear"
              >
                <a-input
                  v-model:value="formState.budgetYear"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.budgetyear') })"
                  show-count
                  :maxlength="4"
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
 * 用人需求维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/talent/staffing-requirement/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TalentStaffingRequirementCreate } from '@/types/human-resource/talent/staffing-requirement'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","reqNo","deptId","postId","jobGrade","requestQty","headcountType","reasonCode","replaceEmployeeId","expectedOnboardDate","contractType","workLocation","jobDesc","qualification","budgetYear","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TalentStaffingRequirementCreate & { talentStaffingRequirementId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 talentStaffingRequirementId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.talentStaffingRequirementId) {
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
    const isCreate = !props.formData?.talentStaffingRequirementId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  reqNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reqno') }),
      trigger: 'blur'
    }
  ],
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.deptid') }),
      trigger: 'blur'
    }
  ],
  postId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.postid') }),
      trigger: 'blur'
    }
  ],
  requestQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.requestqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.requestqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  headcountType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.headcounttype') }),
      trigger: 'blur'
    }
  ],
  reasonCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reasoncode') }),
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
  if ('requestQty' in payload) {
    const rawrequestQty = payload.requestQty
    payload.requestQty = typeof rawrequestQty === 'number' ? rawrequestQty : Number(rawrequestQty)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.talentStaffingRequirementId)

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
