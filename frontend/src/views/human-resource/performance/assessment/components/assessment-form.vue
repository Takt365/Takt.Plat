<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/assessment/components -->
<!-- 文件名称：assessment-form.vue -->
<!-- 功能描述：员工绩效考核评估维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="assessment-form-tabs"
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
                :label="t('entity.assessment.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.period')"
                name="assessmentPeriod"
              >
                <a-input
                  v-model:value="formState.assessmentPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.period') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.date')"
                name="assessmentDate"
              >
                <a-date-picker
                  v-model:value="formState.assessmentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.date') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.schememetricid')"
                name="schemeMetricId"
              >
                <a-input
                  v-model:value="formState.schemeMetricId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.schememetricid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.selfscore')"
                name="selfScore"
              >
                <a-input-number
                  v-model:value="formState.selfScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.selfscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.assessment.selfevaluationnotes')"
                name="selfEvaluationNotes"
              >
                <a-textarea
                  v-model:value="formState.selfEvaluationNotes"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assessment.selfevaluationnotes') })"
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
                :label="t('entity.assessment.supervisorscore')"
                name="supervisorScore"
              >
                <a-input-number
                  v-model:value="formState.supervisorScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.supervisorcomments')"
                name="supervisorComments"
              >
                <a-input
                  v-model:value="formState.supervisorComments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorcomments') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.finalscore')"
                name="finalScore"
              >
                <a-input-number
                  v-model:value="formState.finalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.finalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.performancegrade')"
                name="performanceGrade"
              >
                <a-input
                  v-model:value="formState.performanceGrade"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.performancegrade') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.reviewerid')"
                name="reviewerId"
              >
                <a-input
                  v-model:value="formState.reviewerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.reviewerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.interviewdate')"
                name="interviewDate"
              >
                <a-date-picker
                  v-model:value="formState.interviewDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.interviewdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.assessment.interviewnotes')"
                name="interviewNotes"
              >
                <a-textarea
                  v-model:value="formState.interviewNotes"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assessment.interviewnotes') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.status')"
                name="assessmentStatus"
              >
                <a-input-number
                  v-model:value="formState.assessmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assessment.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.relatedplant') })"
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
 * 员工绩效考核评估维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/performance/assessment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssessmentCreate } from '@/types/human-resource/performance/assessment'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","employeeName","assessmentPeriod","assessmentDate","schemeMetricId","selfScore","selfEvaluationNotes","supervisorScore","supervisorComments","finalScore","performanceGrade","reviewerId","interviewDate","interviewNotes","assessmentStatus","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<AssessmentCreate & { assessmentId?: string }> | null
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
    const isCreate = !props.formData?.assessmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.employeename') }),
      trigger: 'blur'
    }
  ],
  assessmentPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.period') }),
      trigger: 'blur'
    }
  ],
  assessmentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.date') }),
      trigger: 'change'
    }
  ],
  schemeMetricId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.schememetricid') }),
      trigger: 'blur'
    }
  ],
  selfScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.selfscore') }),
      trigger: 'change'
    }
  ],
  selfEvaluationNotes: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.selfevaluationnotes') }),
      trigger: 'blur'
    }
  ],
  supervisorScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.supervisorscore') }),
      trigger: 'change'
    }
  ],
  supervisorComments: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorcomments') }),
      trigger: 'blur'
    }
  ],
  finalScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.finalscore') }),
      trigger: 'change'
    }
  ],
  performanceGrade: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.performancegrade') }),
      trigger: 'blur'
    }
  ],
  reviewerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.reviewerid') }),
      trigger: 'blur'
    }
  ],
  interviewDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.interviewdate') }),
      trigger: 'change'
    }
  ],
  interviewNotes: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assessment.interviewnotes') }),
      trigger: 'blur'
    }
  ],
  assessmentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assessment.status') }),
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
