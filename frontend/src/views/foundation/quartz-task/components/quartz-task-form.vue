<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task/components -->
<!-- 文件名称：quartz-task-form.vue -->
<!-- 功能描述：Quartz 定时任务实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quartz-task-form-tabs"
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
                :label="t('entity.quartzTask.taskcode')"
                name="taskCode"
              >
                <a-input
                  v-model:value="formState.taskCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.taskname')"
                name="taskName"
              >
                <a-input
                  v-model:value="formState.taskName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.jobname')"
                name="jobName"
              >
                <a-input
                  v-model:value="formState.jobName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.jobgroup')"
                name="jobGroup"
              >
                <a-input
                  v-model:value="formState.jobGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobgroup') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.cronexpression')"
                name="cronExpression"
              >
                <a-input
                  v-model:value="formState.cronExpression"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.cronexpression') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.jobtype')"
                name="jobType"
              >
                <a-input
                  v-model:value="formState.jobType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobtype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.jobparams')"
                name="jobParams"
              >
                <a-input
                  v-model:value="formState.jobParams"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobparams') })"
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
                :label="t('entity.quartzTask.taskstatus')"
                name="taskStatus"
              >
                <a-input-number
                  v-model:value="formState.taskStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.concurrent')"
                name="concurrent"
              >
                <a-input-number
                  v-model:value="formState.concurrent"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.concurrent') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.misfirepolicy')"
                name="misfirePolicy"
              >
                <a-input-number
                  v-model:value="formState.misfirePolicy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.misfirepolicy') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.lastrunat')"
                name="lastRunAt"
              >
                <a-input
                  v-model:value="formState.lastRunAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.lastrunat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.nextrunat')"
                name="nextRunAt"
              >
                <a-input
                  v-model:value="formState.nextRunAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.nextrunat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzTask.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.quartzTask.description') })"
                  :rows="2"
                  size="small"
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
 * Quartz 定时任务实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/foundation/quartz-task/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QuartzTaskCreate } from '@/types/foundation/quartz-task'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","taskCode","taskName","jobName","jobGroup","cronExpression","jobType","jobParams","taskStatus","concurrent","misfirePolicy","lastRunAt","nextRunAt","description","extFieldJson","remark"]


interface Props {
  formData?: Partial<QuartzTaskCreate & { quartzTaskId?: string }> | null
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
    const isCreate = !props.formData?.quartzTaskId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  taskCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskcode') }),
      trigger: 'blur'
    }
  ],
  taskName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskname') }),
      trigger: 'blur'
    }
  ],
  jobName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobname') }),
      trigger: 'blur'
    }
  ],
  jobGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobgroup') }),
      trigger: 'blur'
    }
  ],
  cronExpression: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.cronexpression') }),
      trigger: 'blur'
    }
  ],
  jobType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobtype') }),
      trigger: 'blur'
    }
  ],
  taskStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.taskstatus') }),
      trigger: 'change'
    }
  ],
  concurrent: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.concurrent') }),
      trigger: 'change'
    }
  ],
  misfirePolicy: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.misfirepolicy') }),
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
