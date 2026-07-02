<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/logging/quartz-log/components -->
<!-- 文件名称：quartz-log-form.vue -->
<!-- 功能描述：Quartz 任务执行日志实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quartz-log-form-tabs"
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
                :label="t('entity.quartzlog.quartztaskid')"
                name="quartzTaskId"
              >
                <a-input
                  v-model:value="formState.quartzTaskId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.quartztaskid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.taskname')"
                name="taskName"
              >
                <a-input
                  v-model:value="formState.taskName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.taskname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.jobgroup')"
                name="jobGroup"
              >
                <TaktSelect
                  v-model:value="formState.jobGroup"
                  dict-type="sys_quartz_job_group"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.jobgroup') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.tasktype')"
                name="taskType"
              >
                <TaktSelect
                  v-model:value="formState.taskType"
                  dict-type="sys_quartz_task_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.tasktype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.executetime')"
                name="executeTime"
              >
                <a-date-picker
                  v-model:value="formState.executeTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.executeduration')"
                name="executeDuration"
              >
                <a-input
                  v-model:value="formState.executeDuration"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeduration') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzlog.executeparams')"
                name="executeParams"
              >
                <a-input
                  v-model:value="formState.executeParams"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeparams') })"
                  show-count
                  :maxlength="1000"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzlog.executemessage')"
                name="executeMessage"
              >
                <a-input
                  v-model:value="formState.executeMessage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executemessage') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzlog.errorinfo')"
                name="errorInfo"
              >
                <a-input
                  v-model:value="formState.errorInfo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.errorinfo') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzlog.executeip')"
                name="executeIp"
              >
                <a-input
                  v-model:value="formState.executeIp"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeip') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzlog.executehost')"
                name="executeHost"
              >
                <a-input
                  v-model:value="formState.executeHost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executehost') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.quartzlog.executestatus')"
                name="executeStatus"
              >
                <a-input-number
                  v-model:value="formState.executeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executestatus') })"
                  style="width: 100%"
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
 * Quartz 任务执行日志实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/quartz-log/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QuartzLogCreate } from '@/types/statistics/logging/quartz-log'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","quartzTaskId","taskName","jobGroup","taskType","executeTime","executeDuration","executeParams","executeMessage","errorInfo","executeIp","executeHost","executeStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QuartzLogCreate & { quartzLogId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  jobGroup: "default",
  taskType: "assembly"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 quartzLogId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.quartzLogId) {
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
    const isCreate = !props.formData?.quartzLogId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  quartzTaskId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.quartztaskid') }),
      trigger: 'blur'
    }
  ],
  taskName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.taskname') }),
      trigger: 'blur'
    }
  ],
  jobGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzlog.jobgroup') }),
      trigger: 'change'
    }
  ],
  taskType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzlog.tasktype') }),
      trigger: 'change'
    }
  ],
  executeTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetime') }),
      trigger: 'change'
    }
  ],
  executeDuration: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeduration') }),
      trigger: 'blur'
    }
  ],
  executeParams: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeparams') }),
      trigger: 'blur'
    }
  ],
  executeMessage: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executemessage') }),
      trigger: 'blur'
    }
  ],
  errorInfo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.errorinfo') }),
      trigger: 'blur'
    }
  ],
  executeIp: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeip') }),
      trigger: 'blur'
    }
  ],
  executeHost: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executehost') }),
      trigger: 'blur'
    }
  ],
  executeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executestatus') }))
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
  if ('executeStatus' in payload) {
    const rawexecuteStatus = payload.executeStatus
    payload.executeStatus = typeof rawexecuteStatus === 'number' ? rawexecuteStatus : Number(rawexecuteStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.quartzLogId)

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
