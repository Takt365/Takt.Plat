<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/training-development/career-development/components -->
<!-- 文件名称：career-development-form.vue -->
<!-- 功能描述：员工职业发展规划与技能评估维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="career-development-form-tabs"
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
                :label="t('entity.careerDevelopment.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.skillcategory')"
                name="skillCategory"
              >
                <a-input
                  v-model:value="formState.skillCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.skillname')"
                name="skillName"
              >
                <a-input
                  v-model:value="formState.skillName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.assessmentdate')"
                name="assessmentDate"
              >
                <a-date-picker
                  v-model:value="formState.assessmentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.assessmentdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.assessmentmethod')"
                name="assessmentMethod"
              >
                <a-input
                  v-model:value="formState.assessmentMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.assessmentscore')"
                name="assessmentScore"
              >
                <a-input-number
                  v-model:value="formState.assessmentScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentscore') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.skilllevel')"
                name="skillLevel"
              >
                <a-input
                  v-model:value="formState.skillLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skilllevel') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.targetposition')"
                name="targetPosition"
              >
                <a-input
                  v-model:value="formState.targetPosition"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.targetposition') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.developmentplan')"
                name="developmentPlan"
              >
                <a-input
                  v-model:value="formState.developmentPlan"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.developmentplan') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.improvementsuggestions')"
                name="improvementSuggestions"
              >
                <a-input
                  v-model:value="formState.improvementSuggestions"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.improvementsuggestions') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.nextassessmentdate')"
                name="nextAssessmentDate"
              >
                <a-date-picker
                  v-model:value="formState.nextAssessmentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.nextassessmentdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.status')"
                name="careerDevelopmentStatus"
              >
                <a-input-number
                  v-model:value="formState.careerDevelopmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.careerDevelopment.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.relatedplant') })"
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
 * 员工职业发展规划与技能评估维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training-development/career-development/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CareerDevelopmentCreate } from '@/types/human-resource/training-development/career-development'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","employeeId","employeeName","skillCategory","skillName","assessmentDate","assessmentMethod","assessmentScore","skillLevel","targetPosition","developmentPlan","improvementSuggestions","nextAssessmentDate","careerDevelopmentStatus","relatedPlant","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CareerDevelopmentCreate & { careerDevelopmentId?: string }> | null
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
    const isCreate = !props.formData?.careerDevelopmentId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeename') }),
      trigger: 'blur'
    }
  ],
  skillCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillcategory') }),
      trigger: 'blur'
    }
  ],
  skillName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillname') }),
      trigger: 'blur'
    }
  ],
  assessmentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.assessmentdate') }),
      trigger: 'change'
    }
  ],
  assessmentMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentmethod') }),
      trigger: 'blur'
    }
  ],
  assessmentScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.assessmentscore') }),
      trigger: 'change'
    }
  ],
  skillLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skilllevel') }),
      trigger: 'blur'
    }
  ],
  targetPosition: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.targetposition') }),
      trigger: 'blur'
    }
  ],
  developmentPlan: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.developmentplan') }),
      trigger: 'blur'
    }
  ],
  improvementSuggestions: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.improvementsuggestions') }),
      trigger: 'blur'
    }
  ],
  nextAssessmentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.nextassessmentdate') }),
      trigger: 'change'
    }
  ],
  careerDevelopmentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.status') }),
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
