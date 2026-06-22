<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-analysis/components -->
<!-- 文件名称：perf-analysis-form.vue -->
<!-- 功能描述：分析改进维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="perf-analysis-form-tabs"
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
                :label="t('entity.perfanalysis.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.assessmentid')"
                name="assessmentId"
              >
                <a-input
                  v-model:value="formState.assessmentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.assessmentid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.plantitle')"
                name="planTitle"
              >
                <a-input
                  v-model:value="formState.planTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.plantitle') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.improvementarea')"
                name="improvementArea"
              >
                <a-input
                  v-model:value="formState.improvementArea"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementarea') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.currentsituation')"
                name="currentSituation"
              >
                <a-input
                  v-model:value="formState.currentSituation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.currentsituation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.improvementgoal')"
                name="improvementGoal"
              >
                <a-input
                  v-model:value="formState.improvementGoal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementgoal') })"
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
                :label="t('entity.perfanalysis.improvementactions')"
                name="improvementActions"
              >
                <a-input
                  v-model:value="formState.improvementActions"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementactions') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.plandate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.plandate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.targetcompletiondate')"
                name="targetCompletionDate"
              >
                <a-date-picker
                  v-model:value="formState.targetCompletionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.targetcompletiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.progresspercentage')"
                name="progressPercentage"
              >
                <a-input-number
                  v-model:value="formState.progressPercentage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.progresspercentage') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfanalysis.resultdescription')"
                name="resultDescription"
              >
                <a-textarea
                  v-model:value="formState.resultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfanalysis.resultdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.mentorid')"
                name="mentorId"
              >
                <a-input
                  v-model:value="formState.mentorId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.mentorid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.improvementstatus')"
                name="improvementStatus"
              >
                <a-input-number
                  v-model:value="formState.improvementStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfanalysis.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.relatedplant') })"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
 * 分析改进维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-analysis/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PerfAnalysisCreate } from '@/types/human-resource/performance/perf-analysis'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","employeeName","assessmentId","planTitle","improvementArea","currentSituation","improvementGoal","improvementActions","planDate","targetCompletionDate","progressPercentage","resultDescription","mentorId","improvementStatus","relatedPlant","ExtField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PerfAnalysisCreate & { perfAnalysisId?: string }> | null
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
    const isCreate = !props.formData?.perfAnalysisId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeename') }),
      trigger: 'blur'
    }
  ],
  assessmentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.assessmentid') }),
      trigger: 'blur'
    }
  ],
  planTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.plantitle') }),
      trigger: 'blur'
    }
  ],
  improvementArea: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementarea') }),
      trigger: 'blur'
    }
  ],
  currentSituation: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.currentsituation') }),
      trigger: 'blur'
    }
  ],
  improvementGoal: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementgoal') }),
      trigger: 'blur'
    }
  ],
  improvementActions: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementactions') }),
      trigger: 'blur'
    }
  ],
  planDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.plandate') }),
      trigger: 'change'
    }
  ],
  targetCompletionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.targetcompletiondate') }),
      trigger: 'change'
    }
  ],
  progressPercentage: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.progresspercentage') }),
      trigger: 'change'
    }
  ],
  resultDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.resultdescription') }),
      trigger: 'blur'
    }
  ],
  mentorId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.mentorid') }),
      trigger: 'blur'
    }
  ],
  improvementStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.improvementstatus') }),
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
