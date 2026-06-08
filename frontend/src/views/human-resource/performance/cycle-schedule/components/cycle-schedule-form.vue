<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/cycle-schedule/components -->
<!-- 文件名称：cycle-schedule-form.vue -->
<!-- 功能描述：绩效考核周期日程安排维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="cycle-schedule-form-tabs"
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
                :label="t('entity.cycleSchedule.cyclecode')"
                name="cycleCode"
              >
                <a-input
                  v-model:value="formState.cycleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.cyclename')"
                name="cycleName"
              >
                <a-input
                  v-model:value="formState.cycleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.cycletype')"
                name="cycleType"
              >
                <a-input
                  v-model:value="formState.cycleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cycletype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.cycleyear')"
                name="cycleYear"
              >
                <a-input-number
                  v-model:value="formState.cycleYear"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cycleyear') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.cyclesequence')"
                name="cycleSequence"
              >
                <a-input-number
                  v-model:value="formState.cycleSequence"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclesequence') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.enddate') })"
                  value-format="YYYY-MM-DD"
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
                :label="t('entity.cycleSchedule.goalsettingduedate')"
                name="goalSettingDueDate"
              >
                <a-date-picker
                  v-model:value="formState.goalSettingDueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.goalsettingduedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.selfevaluationduedate')"
                name="selfEvaluationDueDate"
              >
                <a-date-picker
                  v-model:value="formState.selfEvaluationDueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.selfevaluationduedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.supervisorreviewduedate')"
                name="supervisorReviewDueDate"
              >
                <a-date-picker
                  v-model:value="formState.supervisorReviewDueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.supervisorreviewduedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.interviewduedate')"
                name="interviewDueDate"
              >
                <a-date-picker
                  v-model:value="formState.interviewDueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.interviewduedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.resultconfirmationduedate')"
                name="resultConfirmationDueDate"
              >
                <a-date-picker
                  v-model:value="formState.resultConfirmationDueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.resultconfirmationduedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.applicabledepartment')"
                name="applicableDepartment"
              >
                <a-input
                  v-model:value="formState.applicableDepartment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.applicabledepartment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.cycleSchedule.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.cycleSchedule.description') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.status')"
                name="cycleScheduleStatus"
              >
                <a-input-number
                  v-model:value="formState.cycleScheduleStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.cycleSchedule.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.relatedplant') })"
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
 * 绩效考核周期日程安排维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/cycle-schedule/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CycleScheduleCreate } from '@/types/human-resource/performance/cycle-schedule'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","cycleCode","cycleName","cycleType","cycleYear","cycleSequence","startDate","endDate","goalSettingDueDate","selfEvaluationDueDate","supervisorReviewDueDate","interviewDueDate","resultConfirmationDueDate","applicableDepartment","description","cycleScheduleStatus","relatedPlant","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CycleScheduleCreate & { cycleScheduleId?: string }> | null
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
    const isCreate = !props.formData?.cycleScheduleId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  cycleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclecode') }),
      trigger: 'blur'
    }
  ],
  cycleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cyclename') }),
      trigger: 'blur'
    }
  ],
  cycleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.cycletype') }),
      trigger: 'blur'
    }
  ],
  cycleYear: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.cycleyear') }),
      trigger: 'change'
    }
  ],
  cycleSequence: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.cyclesequence') }),
      trigger: 'change'
    }
  ],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.enddate') }),
      trigger: 'change'
    }
  ],
  goalSettingDueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.goalsettingduedate') }),
      trigger: 'change'
    }
  ],
  selfEvaluationDueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.selfevaluationduedate') }),
      trigger: 'change'
    }
  ],
  supervisorReviewDueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.supervisorreviewduedate') }),
      trigger: 'change'
    }
  ],
  interviewDueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.interviewduedate') }),
      trigger: 'change'
    }
  ],
  resultConfirmationDueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.resultconfirmationduedate') }),
      trigger: 'change'
    }
  ],
  applicableDepartment: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.applicabledepartment') }),
      trigger: 'blur'
    }
  ],
  description: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.cycleSchedule.description') }),
      trigger: 'blur'
    }
  ],
  cycleScheduleStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.cycleSchedule.status') }),
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
