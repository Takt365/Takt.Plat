<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-issue/components -->
<!-- 文件名称：quality-issue-form.vue -->
<!-- 功能描述：品质问题应对主表维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-issue-form-tabs"
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
                :label="t('entity.qualityIssue.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.code')"
                name="qualityIssueCode"
              >
                <a-input
                  v-model:value="formState.qualityIssueCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.issuedate')"
                name="issueDate"
              >
                <a-date-picker
                  v-model:value="formState.issueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityIssue.issuedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.model')"
                name="model"
              >
                <a-input
                  v-model:value="formState.model"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.model') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.lot')"
                name="lot"
              >
                <a-input
                  v-model:value="formState.lot"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.lot') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.qualityproblemsresponse')"
                name="qualityProblemsResponse"
              >
                <a-input
                  v-model:value="formState.qualityProblemsResponse"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.qualityproblemsresponse') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.reworkduetodefects')"
                name="reworkDueToDefects"
              >
                <a-input
                  v-model:value="formState.reworkDueToDefects"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.reworkduetodefects') })"
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
                :label="t('entity.qualityIssue.needrework')"
                name="needRework"
              >
                <a-input
                  v-model:value="formState.needRework"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.needrework') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.totaltimeminutes')"
                name="totalTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.totaltimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.totalcost')"
                name="totalCost"
              >
                <a-input-number
                  v-model:value="formState.totalCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.totalcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.costcurrency')"
                name="costCurrency"
              >
                <a-input
                  v-model:value="formState.costCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.costcurrency') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.meetingitems')"
                name="meetingItems"
              >
                <a-input
                  v-model:value="formState.meetingItems"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.meetingitems') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.assyreworkitems')"
                name="assyReworkItems"
              >
                <a-input
                  v-model:value="formState.assyReworkItems"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.assyreworkitems') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssue.pcbareworkitems')"
                name="pcbaReworkItems"
              >
                <a-input
                  v-model:value="formState.pcbaReworkItems"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.pcbareworkitems') })"
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
 * 品质问题应对主表维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-issue/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssueCreate } from '@/types/logistics/quality/cost/quality-issue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","qualityIssueCode","issueDate","model","lot","qualityProblemsResponse","reworkDueToDefects","needRework","totalTimeMinutes","totalCost","costCurrency","meetingItems","assyReworkItems","pcbaReworkItems","extFieldJson","remark"]


interface Props {
  formData?: Partial<QualityIssueCreate & { qualityIssueId?: string }> | null
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
    const isCreate = !props.formData?.qualityIssueId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.plantcode') }),
      trigger: 'blur'
    }
  ],
  qualityIssueCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.code') }),
      trigger: 'blur'
    }
  ],
  issueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssue.issuedate') }),
      trigger: 'change'
    }
  ],
  model: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.model') }),
      trigger: 'blur'
    }
  ],
  lot: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.lot') }),
      trigger: 'blur'
    }
  ],
  totalTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssue.totaltimeminutes') }),
      trigger: 'change'
    }
  ],
  totalCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssue.totalcost') }),
      trigger: 'change'
    }
  ],
  costCurrency: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssue.costcurrency') }),
      trigger: 'blur'
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
