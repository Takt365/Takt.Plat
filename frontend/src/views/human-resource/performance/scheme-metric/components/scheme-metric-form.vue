<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/scheme-metric/components -->
<!-- 文件名称：scheme-metric-form.vue -->
<!-- 功能描述：绩效方案指标维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="scheme-metric-form-tabs"
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
                :label="t('entity.schemeMetric.schemecode')"
                name="schemeCode"
              >
                <a-input
                  v-model:value="formState.schemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.schemename')"
                name="schemeName"
              >
                <a-input
                  v-model:value="formState.schemeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.applicabledepartment')"
                name="applicableDepartment"
              >
                <a-input
                  v-model:value="formState.applicableDepartment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.applicabledepartment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.cycletype')"
                name="cycleType"
              >
                <a-input
                  v-model:value="formState.cycleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.cycletype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.scoringstandard')"
                name="scoringStandard"
              >
                <a-input
                  v-model:value="formState.scoringStandard"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringstandard') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.selfevaluationweight')"
                name="selfEvaluationWeight"
              >
                <a-input-number
                  v-model:value="formState.selfEvaluationWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.selfevaluationweight') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.supervisorweight')"
                name="supervisorWeight"
              >
                <a-input-number
                  v-model:value="formState.supervisorWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.supervisorweight') })"
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
                :label="t('entity.schemeMetric.metriccode')"
                name="metricCode"
              >
                <a-input
                  v-model:value="formState.metricCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metriccode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.metricname')"
                name="metricName"
              >
                <a-input
                  v-model:value="formState.metricName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metricname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.category')"
                name="category"
              >
                <a-input
                  v-model:value="formState.category"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.category') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.metrictype')"
                name="metricType"
              >
                <a-input
                  v-model:value="formState.metricType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metrictype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.scoringcriteria')"
                name="scoringCriteria"
              >
                <a-input
                  v-model:value="formState.scoringCriteria"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringcriteria') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.standardweight')"
                name="standardWeight"
              >
                <a-input-number
                  v-model:value="formState.standardWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.standardweight') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.status')"
                name="schemeMetricStatus"
              >
                <a-input-number
                  v-model:value="formState.schemeMetricStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.schemeMetric.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.relatedplant') })"
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
 * 绩效方案指标维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/performance/scheme-metric/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SchemeMetricCreate } from '@/types/human-resource/performance/scheme-metric'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","schemeCode","schemeName","applicableDepartment","cycleType","scoringStandard","selfEvaluationWeight","supervisorWeight","metricCode","metricName","category","metricType","scoringCriteria","standardWeight","sortOrder","schemeMetricStatus","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<SchemeMetricCreate & { schemeMetricId?: string }> | null
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
    const isCreate = !props.formData?.schemeMetricId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  schemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemecode') }),
      trigger: 'blur'
    }
  ],
  schemeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemename') }),
      trigger: 'blur'
    }
  ],
  applicableDepartment: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.applicabledepartment') }),
      trigger: 'blur'
    }
  ],
  cycleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.cycletype') }),
      trigger: 'blur'
    }
  ],
  scoringStandard: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringstandard') }),
      trigger: 'blur'
    }
  ],
  selfEvaluationWeight: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.schemeMetric.selfevaluationweight') }),
      trigger: 'change'
    }
  ],
  supervisorWeight: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.schemeMetric.supervisorweight') }),
      trigger: 'change'
    }
  ],
  metricCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metriccode') }),
      trigger: 'blur'
    }
  ],
  metricName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metricname') }),
      trigger: 'blur'
    }
  ],
  category: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.category') }),
      trigger: 'blur'
    }
  ],
  metricType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metrictype') }),
      trigger: 'blur'
    }
  ],
  scoringCriteria: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringcriteria') }),
      trigger: 'blur'
    }
  ],
  standardWeight: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.schemeMetric.standardweight') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.schemeMetric.sortorder') }),
      trigger: 'change'
    }
  ],
  schemeMetricStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.schemeMetric.status') }),
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
