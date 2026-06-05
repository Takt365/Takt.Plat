<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-issue-pcba-rework/components -->
<!-- 文件名称：quality-issue-pcba-rework-form.vue -->
<!-- 功能描述：品质问题应对明细 - PCBA不良改修应对维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-issue-pcba-rework-form-tabs"
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
                :label="t('entity.qualityIssuePcbaRework.qualityissueid')"
                name="qualityIssueId"
              >
                <a-input
                  v-model:value="formState.qualityIssueId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissueid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.qualityissuecode')"
                name="qualityIssueCode"
              >
                <a-input
                  v-model:value="formState.qualityIssueCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissuecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbadefectparts')"
                name="pcbaDefectParts"
              >
                <a-input
                  v-model:value="formState.pcbaDefectParts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbadefectparts') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbareworkcost')"
                name="pcbaReworkCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaReworkCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareworkcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbareworktimeminutes')"
                name="pcbaReworkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.pcbaReworkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareworktimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes')"
                name="pcbaReinspectionTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.pcbaReinspectionTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes') })"
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
                :label="t('entity.qualityIssuePcbaRework.pcbatravelcost')"
                name="pcbaTravelCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaTravelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbatravelcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbawarehousecost')"
                name="pcbaWarehouseCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaWarehouseCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbawarehousecost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbaotherexpenses')"
                name="pcbaOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.pcbaOtherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbaotherexpenses') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbareworknote')"
                name="pcbaReworkNote"
              >
                <a-textarea
                  v-model:value="formState.pcbaReworkNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssuePcbaRework.pcbareworknote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbascrapcost')"
                name="pcbaScrapCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaScrapCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbascrapcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbacustomername')"
                name="pcbaCustomerName"
              >
                <a-input
                  v-model:value="formState.pcbaCustomerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbacustomername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbadebitnoteno')"
                name="pcbaDebitNoteNo"
              >
                <a-textarea
                  v-model:value="formState.pcbaDebitNoteNo"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssuePcbaRework.pcbadebitnoteno') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbaotherexpenses2')"
                name="pcbaOtherExpenses2"
              >
                <a-input-number
                  v-model:value="formState.pcbaOtherExpenses2"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbaotherexpenses2') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbanote')"
                name="pcbaNote"
              >
                <a-textarea
                  v-model:value="formState.pcbaNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityIssuePcbaRework.pcbanote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityIssuePcbaRework.pcbarecorder')"
                name="pcbaRecorder"
              >
                <a-input
                  v-model:value="formState.pcbaRecorder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.pcbarecorder') })"
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
 * 品质问题应对明细 - PCBA不良改修应对维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-issue-pcba-rework/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssuePcbaReworkCreate } from '@/types/logistics/quality/cost/quality-issue-pcba-rework'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","qualityIssueId","qualityIssueCode","lineNumber","pcbaDefectParts","pcbaReworkCost","pcbaReworkTimeMinutes","pcbaReinspectionTimeMinutes","pcbaTravelCost","pcbaWarehouseCost","pcbaOtherExpenses","pcbaReworkNote","pcbaScrapCost","pcbaCustomerName","pcbaDebitNoteNo","pcbaOtherExpenses2","pcbaNote","pcbaRecorder","extFieldJson","remark"]


interface Props {
  formData?: Partial<QualityIssuePcbaReworkCreate & { qualityIssuePcbaReworkId?: string }> | null
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
    const isCreate = !props.formData?.qualityIssuePcbaReworkId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  qualityIssueId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissueid') }),
      trigger: 'blur'
    }
  ],
  qualityIssueCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityIssuePcbaRework.qualityissuecode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.linenumber') }),
      trigger: 'change'
    }
  ],
  pcbaReworkCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbareworkcost') }),
      trigger: 'change'
    }
  ],
  pcbaReworkTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbareworktimeminutes') }),
      trigger: 'change'
    }
  ],
  pcbaReinspectionTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbareinspectiontimeminutes') }),
      trigger: 'change'
    }
  ],
  pcbaTravelCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbatravelcost') }),
      trigger: 'change'
    }
  ],
  pcbaWarehouseCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbawarehousecost') }),
      trigger: 'change'
    }
  ],
  pcbaOtherExpenses: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbaotherexpenses') }),
      trigger: 'change'
    }
  ],
  pcbaScrapCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbascrapcost') }),
      trigger: 'change'
    }
  ],
  pcbaOtherExpenses2: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityIssuePcbaRework.pcbaotherexpenses2') }),
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
