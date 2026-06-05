<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-issue-assy-rework/components -->
<!-- 文件名称：quality-issue-assy-rework-form.vue -->
<!-- 功能描述：品质问题应对明细 - 组装不良改修应对维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-issue-assy-rework-form-tabs"
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
                :label="t('entity.qualityIssueAssyRework.qualityissueid')"
                name="qualityIssueId"
              >
                <a-input
                  v-model:value="formState.qualityIssueId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissueid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.qualityissuecode')"
                name="qualityIssueCode"
              >
                <a-input
                  v-model:value="formState.qualityIssueCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissuecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assydefectparts')"
                name="assyDefectParts"
              >
                <a-input
                  v-model:value="formState.assyDefectParts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assydefectparts') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyreworkcost')"
                name="assyReworkCost"
              >
                <a-input-number
                  v-model:value="formState.assyReworkCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreworkcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyreworktimeminutes')"
                name="assyReworkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.assyReworkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreworktimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes')"
                name="assyReinspectionTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.assyReinspectionTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes') })"
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
                :label="t('entity.qualityIssueAssyRework.assytravelcost')"
                name="assyTravelCost"
              >
                <a-input-number
                  v-model:value="formState.assyTravelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assytravelcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assywarehousecost')"
                name="assyWarehouseCost"
              >
                <a-input-number
                  v-model:value="formState.assyWarehouseCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assywarehousecost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyotherexpenses')"
                name="assyOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.assyOtherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyotherexpenses') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyreworknote')"
                name="assyReworkNote"
              >
                <a-textarea
                  v-model:value="formState.assyReworkNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssueAssyRework.assyreworknote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyscrapcost')"
                name="assyScrapCost"
              >
                <a-input-number
                  v-model:value="formState.assyScrapCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyscrapcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assycustomername')"
                name="assyCustomerName"
              >
                <a-input
                  v-model:value="formState.assyCustomerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assycustomername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assydebitnoteno')"
                name="assyDebitNoteNo"
              >
                <a-textarea
                  v-model:value="formState.assyDebitNoteNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssueAssyRework.assydebitnoteno') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyotherexpenses2')"
                name="assyOtherExpenses2"
              >
                <a-input-number
                  v-model:value="formState.assyOtherExpenses2"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyotherexpenses2') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assynote')"
                name="assyNote"
              >
                <a-textarea
                  v-model:value="formState.assyNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssueAssyRework.assynote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssueAssyRework.assyrecorder')"
                name="assyRecorder"
              >
                <a-input
                  v-model:value="formState.assyRecorder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.assyrecorder') })"
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
 * 品质问题应对明细 - 组装不良改修应对维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-issue-assy-rework/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssueAssyReworkCreate } from '@/types/logistics/quality/cost/quality-issue-assy-rework'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","qualityIssueId","qualityIssueCode","lineNumber","assyDefectParts","assyReworkCost","assyReworkTimeMinutes","assyReinspectionTimeMinutes","assyTravelCost","assyWarehouseCost","assyOtherExpenses","assyReworkNote","assyScrapCost","assyCustomerName","assyDebitNoteNo","assyOtherExpenses2","assyNote","assyRecorder","extFieldJson","remark"]


interface Props {
  formData?: Partial<QualityIssueAssyReworkCreate & { qualityIssueAssyReworkId?: string }> | null
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
    const isCreate = !props.formData?.qualityIssueAssyReworkId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  qualityIssueId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissueid') }),
      trigger: 'blur'
    }
  ],
  qualityIssueCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssueAssyRework.qualityissuecode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.linenumber') }),
      trigger: 'change'
    }
  ],
  assyReworkCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyreworkcost') }),
      trigger: 'change'
    }
  ],
  assyReworkTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyreworktimeminutes') }),
      trigger: 'change'
    }
  ],
  assyReinspectionTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyreinspectiontimeminutes') }),
      trigger: 'change'
    }
  ],
  assyTravelCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assytravelcost') }),
      trigger: 'change'
    }
  ],
  assyWarehouseCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assywarehousecost') }),
      trigger: 'change'
    }
  ],
  assyOtherExpenses: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyotherexpenses') }),
      trigger: 'change'
    }
  ],
  assyScrapCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyscrapcost') }),
      trigger: 'change'
    }
  ],
  assyOtherExpenses2: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssueAssyRework.assyotherexpenses2') }),
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
