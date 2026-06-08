<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task/components -->
<!-- 文件名称：quartz-task-form.vue -->
<!-- 功能描述：Quartz 定时任务实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      <!-- 主表 -->
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
                :label="t('entity.quartzTask.tasktype')"
                name="taskType"
              >
                <a-input-number
                  v-model:value="formState.taskType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.tasktype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.assemblyname')"
                name="assemblyName"
              >
                <a-input
                  v-model:value="formState.assemblyName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.assemblyname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.classname')"
                name="className"
              >
                <a-input
                  v-model:value="formState.className"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.classname') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.apiurl')"
                name="apiUrl"
              >
                <a-input
                  v-model:value="formState.apiUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.apiurl') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.requestmethod')"
                name="requestMethod"
              >
                <a-input
                  v-model:value="formState.requestMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.requestmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.sqlscript')"
                name="sqlScript"
              >
                <a-input
                  v-model:value="formState.sqlScript"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.sqlscript') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.triggertype')"
                name="triggerType"
              >
                <a-input-number
                  v-model:value="formState.triggerType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.triggertype') })"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.quartzTask.intervalseconds')"
                name="intervalSeconds"
              >
                <a-input-number
                  v-model:value="formState.intervalSeconds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.intervalseconds') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.executeparams')"
                name="executeParams"
              >
                <a-input
                  v-model:value="formState.executeParams"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.executeparams') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
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
                :label="t('entity.quartzTask.firstrunat')"
                name="firstRunAt"
              >
                <a-input
                  v-model:value="formState.firstRunAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.firstrunat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.quartzTask.executecount')"
                name="executeCount"
              >
                <a-input-number
                  v-model:value="formState.executeCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.executecount') })"
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
      <!-- 子表：quartzLog -->
      <a-tab-pane
        key="child-quartzLogs"
        :tab="t('entity.quartzLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddQuartzLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.quartzLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="quartzLogFormColumns"
          :data-source="childQuartzLogRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">

            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveQuartzLogRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Quartz 定时任务实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QuartzTaskCreate, QuartzLogCreate, QuartzLog } from '@/types/foundation/quartz-task'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","taskCode","taskName","jobName","jobGroup","taskType","assemblyName","className","apiUrl","requestMethod","sqlScript","triggerType","cronExpression","intervalSeconds","executeParams","taskStatus","concurrent","misfirePolicy","firstRunAt","executeCount","lastRunAt","nextRunAt","description","extFieldJson","remark"]

/** quartzLog 子表行（表单 Tab 内嵌） */
const childQuartzLogRows = ref<Record<string, unknown>[]>([])

/** 子表 quartzLog 表单列定义 */
const quartzLogFormColumns = computed(() => [

  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<QuartzTaskCreate & { quartzTaskId?: string }> | null | undefined) {
  childQuartzLogRows.value = ((val as any)?.quartzLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.quartzLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 quartzLog 行 */
function handleAddQuartzLogRow() {
  childQuartzLogRows.value.push({
    __rowKey: `new-${Date.now()}`,

  })
}

/** 表单 Tab 内删除 quartzLog 行 */
function handleRemoveQuartzLogRow(index: number) {
  childQuartzLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    quartzLogs: childQuartzLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QuartzTaskCreate & { quartzTaskId?: string }> | null
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
    delete (next as any).quartzLogs
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
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

/** 表单校验规则（与 FluentValidation 必填对齐） */
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
  taskType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.tasktype') }),
      trigger: 'change'
    }
  ],
  assemblyName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.assemblyname') }),
      trigger: 'blur'
    }
  ],
  className: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.classname') }),
      trigger: 'blur'
    }
  ],
  triggerType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.triggertype') }),
      trigger: 'change'
    }
  ],
  cronExpression: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.quartzTask.cronexpression') }),
      trigger: 'blur'
    }
  ],
  intervalSeconds: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.intervalseconds') }),
      trigger: 'change'
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
  executeCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.quartzTask.executecount') }),
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
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childQuartzLogRows.value = []
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
