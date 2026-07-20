<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task/components -->
<!-- 文件名称：quartz-task-form.vue -->
<!-- 功能描述：Quartz 定时任务实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quartz-task-form-tabs"
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
                :label="t('entity.quartztask.taskcode')"
                name="taskCode"
              >
                <a-input
                  v-model:value="formState.taskCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.quartzTaskId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.taskname')"
                name="taskName"
              >
                <a-input
                  v-model:value="formState.taskName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.jobname')"
                name="jobName"
              >
                <a-input
                  v-model:value="formState.jobName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.jobname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.jobgroup')"
                name="jobGroup"
              >
                <TaktSelect
                  v-model:value="formState.jobGroup"
                  dict-type="sys_quartz_job_group"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.jobgroup') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.tasktype')"
                name="taskType"
              >
                <TaktSelect
                  v-model:value="formState.taskType"
                  dict-type="sys_quartz_task_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.tasktype') })"
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="taskTypeFieldVisibility.assemblyName"
              :span="12"
            >
              <a-form-item
                name="assemblyName"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t(QUARTZ_TASK_EXEC_FIELD_I18N.assemblyName.hint)"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t(QUARTZ_TASK_EXEC_FIELD_I18N.assemblyName.label) }}</span>
                  </span>
                </template>
                <a-input
                  v-model:value="formState.assemblyName"
                  :placeholder="t(QUARTZ_TASK_EXEC_FIELD_I18N.assemblyName.placeholder)"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="taskTypeFieldVisibility.className"
              :span="12"
            >
              <a-form-item
                name="className"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t(QUARTZ_TASK_EXEC_FIELD_I18N.className.hint)"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t(QUARTZ_TASK_EXEC_FIELD_I18N.className.label) }}</span>
                  </span>
                </template>
                <a-input
                  v-model:value="formState.className"
                  :placeholder="t(QUARTZ_TASK_EXEC_FIELD_I18N.className.placeholder)"
                  show-count
                  :maxlength="255"
                  allow-clear
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
            <a-col
              v-if="taskTypeFieldVisibility.apiUrl"
              :span="12"
            >
              <a-form-item
                name="apiUrl"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t(QUARTZ_TASK_EXEC_FIELD_I18N.apiUrl.hint)"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t(QUARTZ_TASK_EXEC_FIELD_I18N.apiUrl.label) }}</span>
                  </span>
                </template>
                <a-input
                  v-model:value="formState.apiUrl"
                  :placeholder="t(QUARTZ_TASK_EXEC_FIELD_I18N.apiUrl.placeholder)"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="taskTypeFieldVisibility.requestMethod"
              :span="12"
            >
              <a-form-item
                name="requestMethod"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t(QUARTZ_TASK_EXEC_FIELD_I18N.requestMethod.hint)"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t(QUARTZ_TASK_EXEC_FIELD_I18N.requestMethod.label) }}</span>
                  </span>
                </template>
                <a-input
                  v-model:value="formState.requestMethod"
                  :placeholder="t(QUARTZ_TASK_EXEC_FIELD_I18N.requestMethod.placeholder)"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="taskTypeFieldVisibility.sqlScript"
              :span="12"
            >
              <a-form-item
                name="sqlScript"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t(QUARTZ_TASK_EXEC_FIELD_I18N.sqlScript.hint)"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t(QUARTZ_TASK_EXEC_FIELD_I18N.sqlScript.label) }}</span>
                  </span>
                </template>
                <a-input
                  v-model:value="formState.sqlScript"
                  :placeholder="t(QUARTZ_TASK_EXEC_FIELD_I18N.sqlScript.placeholder)"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.triggertype')"
                name="triggerType"
              >
                <TaktSelect
                  v-model:value="formState.triggerType"
                  dict-type="sys_quartz_trigger_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.triggertype') })"
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="triggerTypeFieldVisibility.cronExpression"
              :span="12"
            >
              <a-form-item
                name="cronExpression"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.form.placeholder.quartztask.cronexpression')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('entity.quartztask.cronexpression') }}</span>
                  </span>
                </template>
                <takt-cron-editor
                  v-model="formState.cronExpression"
                  :placeholder="t('common.page.form.placeholder.quartztask.cronexpression')"
                />
              </a-form-item>
            </a-col>
            <a-col
              v-if="triggerTypeFieldVisibility.intervalSeconds"
              :span="12"
            >
              <a-form-item
                :label="t('entity.quartztask.intervalseconds')"
                name="intervalSeconds"
              >
                <a-input-number
                  v-model:value="formState.intervalSeconds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.intervalseconds') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.executeparams')"
                name="executeParams"
              >
                <a-input
                  v-model:value="formState.executeParams"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.executeparams') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.concurrent')"
                name="concurrent"
              >
                <TaktSelect
                  v-model:value="formState.concurrent"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.concurrent') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartztask.misfirepolicy')"
                name="misfirePolicy"
              >
                <TaktSelect
                  v-model:value="formState.misfirePolicy"
                  dict-type="sys_quartz_misfire_policy"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.misfirepolicy') })"
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
                :label="t('entity.quartztask.taskdescription')"
                name="taskDescription"
              >
                <a-textarea
                  v-model:value="formState.taskDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.quartztask.taskdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartztask.taskstatus')"
                name="taskStatus"
              >
                <TaktSelect
                  v-model:value="formState.taskStatus"
                  dict-type="sys_quartz_task_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.taskstatus') })"
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
 * Quartz 定时任务实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QuartzTaskCreate } from '@/types/foundation/quartz-task'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import {
  QUARTZ_TASK_EXEC_FIELD_I18N,
  buildQuartzTaskTypeFieldVisibility,
  clearQuartzTaskTypeHiddenFields,
} from '@/views/foundation/quartz-task/utils/quartz-task-type-fields'
import { stripQuartzTaskEngineManagedFields } from '@/views/foundation/quartz-task/utils/quartz-task-engine-fields'
import {
  clearQuartzTriggerHiddenFields,
  normalizeQuartzTriggerTypeValue,
} from '@/views/foundation/quartz-task/utils/quartz-task-trigger-fields'
import { TaktQuartzTriggerType } from '@/constants/takt-constants'

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","taskCode","taskName","jobName","jobGroup","taskType","assemblyName","className","apiUrl","requestMethod","sqlScript","triggerType","cronExpression","intervalSeconds","executeParams","concurrent","misfirePolicy","taskDescription","taskStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QuartzTaskCreate & { quartzTaskId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** 当前 taskType 下执行参数字段显隐（assembly / http / sql） */
const taskTypeFieldVisibility = computed(() => buildQuartzTaskTypeFieldVisibility(formState.taskType))

/** 当前 triggerType 下 Cron / Simple 互斥字段显隐 */
const triggerTypeFieldVisibility = computed(() => {
  const triggerType = normalizeQuartzTriggerTypeValue(formState.triggerType)
  return {
    cronExpression: triggerType === TaktQuartzTriggerType.Cron,
    intervalSeconds: triggerType === TaktQuartzTriggerType.Simple,
  }
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  jobGroup: "default",
  taskType: "assembly",
  triggerType: 1,
  misfirePolicy: 0,
  taskStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 quartzTaskId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.quartzTaskId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      stripQuartzTaskEngineManagedFields(formState)
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
    const isCreate = !props.formData?.quartzTaskId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 切换任务类型时清空不可见执行参数字段并清除对应校验 */
watch(
  () => formState.taskType,
  (taskType) => {
    clearQuartzTaskTypeHiddenFields(formState, taskType)
    formRef.value?.clearValidate(['assemblyName', 'className', 'apiUrl', 'requestMethod', 'sqlScript'])
  },
)

/** 切换触发器类型时清空 Cron / Simple 互斥字段并清除对应校验 */
watch(
  () => formState.triggerType,
  (triggerType) => {
    clearQuartzTriggerHiddenFields(formState, triggerType)
    formRef.value?.clearValidate(['cronExpression', 'intervalSeconds'])
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  taskCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskcode') }),
      trigger: 'blur'
    }
  ],
  taskName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskname') }),
      trigger: 'blur'
    }
  ],
  jobName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartztask.jobname') }),
      trigger: 'blur'
    }
  ],
  jobGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartztask.jobgroup') }),
      trigger: 'change'
    }
  ],
  taskType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartztask.tasktype') }),
      trigger: 'change'
    }
  ],
  assemblyName: taskTypeFieldVisibility.value.assemblyName ? [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t(QUARTZ_TASK_EXEC_FIELD_I18N.assemblyName.label) }),
      trigger: 'blur'
    }
  ] : [],
  className: taskTypeFieldVisibility.value.className ? [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t(QUARTZ_TASK_EXEC_FIELD_I18N.className.label) }),
      trigger: 'blur'
    }
  ] : [],
  apiUrl: taskTypeFieldVisibility.value.apiUrl ? [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t(QUARTZ_TASK_EXEC_FIELD_I18N.apiUrl.label) }),
      trigger: 'blur'
    }
  ] : [],
  requestMethod: taskTypeFieldVisibility.value.requestMethod ? [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t(QUARTZ_TASK_EXEC_FIELD_I18N.requestMethod.label) }),
      trigger: 'blur'
    }
  ] : [],
  sqlScript: taskTypeFieldVisibility.value.sqlScript ? [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t(QUARTZ_TASK_EXEC_FIELD_I18N.sqlScript.label) }),
      trigger: 'blur'
    },
    {
      validator: async (_rule, value) => {
        const raw = typeof value === 'string' ? value.trim() : ''
        if (!raw) {
          return Promise.resolve()
        }
        if (
          raw.length > 200
          || /\s/.test(raw)
          || raw.includes('..')
          || !/\.sql$/i.test(raw)
          || raw.startsWith('/')
          || raw.startsWith('~/')
          || /^[a-zA-Z]:[\\/]/.test(raw)
        ) {
          return Promise.reject(t(QUARTZ_TASK_EXEC_FIELD_I18N.sqlScript.hint))
        }
        return Promise.resolve()
      },
      trigger: 'blur'
    }
  ] : [],
  triggerType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.triggertype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.triggertype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cronExpression: triggerTypeFieldVisibility.value.cronExpression ? [
    {
      required: true,
      message: t('common.page.form.placeholder.quartztask.cronexpression'),
      trigger: 'change'
    }
  ] : [],
  intervalSeconds: triggerTypeFieldVisibility.value.intervalSeconds ? [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.intervalseconds') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.intervalseconds') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }] : [],
  concurrent: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.concurrent') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.concurrent') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  misfirePolicy: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.misfirepolicy') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.misfirepolicy') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taskStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.taskstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartztask.taskstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  clearQuartzTaskTypeHiddenFields(payload, formState.taskType)
  clearQuartzTriggerHiddenFields(payload, formState.triggerType)
  stripQuartzTaskEngineManagedFields(payload)
  if ('triggerType' in payload) {
    const rawtriggerType = payload.triggerType
    payload.triggerType = typeof rawtriggerType === 'number' ? rawtriggerType : Number(rawtriggerType)
  }
  if ('intervalSeconds' in payload) {
    const rawintervalSeconds = payload.intervalSeconds
    payload.intervalSeconds = typeof rawintervalSeconds === 'number' ? rawintervalSeconds : Number(rawintervalSeconds)
  }
  if ('concurrent' in payload) {
    const rawconcurrent = payload.concurrent
    payload.concurrent = typeof rawconcurrent === 'number' ? rawconcurrent : Number(rawconcurrent)
  }
  if ('misfirePolicy' in payload) {
    const rawmisfirePolicy = payload.misfirePolicy
    payload.misfirePolicy = typeof rawmisfirePolicy === 'number' ? rawmisfirePolicy : Number(rawmisfirePolicy)
  }
  if ('taskStatus' in payload) {
    const rawtaskStatus = payload.taskStatus
    payload.taskStatus = typeof rawtaskStatus === 'number' ? rawtaskStatus : Number(rawtaskStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
    stripQuartzTaskEngineManagedFields(formState)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.quartzTaskId)

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
