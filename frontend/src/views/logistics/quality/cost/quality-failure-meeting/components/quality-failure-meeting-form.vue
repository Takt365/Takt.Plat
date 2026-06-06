<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-failure-meeting/components -->
<!-- 文件名称：quality-failure-meeting-form.vue -->
<!-- 功能描述：品质问题应对明细 - 会议/调查/试验费用维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-failure-meeting-form-tabs"
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
                :label="t('entity.qualityFailureMeeting.qualityfailureid')"
                name="qualityFailureId"
              >
                <a-input
                  v-model:value="formState.qualityFailureId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.qualityfailureid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.qualityfailurecode')"
                name="qualityFailureCode"
              >
                <a-input
                  v-model:value="formState.qualityFailureCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.qualityfailurecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.directmanpowercostperminute')"
                name="directManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.directManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.directmanpowercostperminute') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.indirectmanpowercostperminute')"
                name="indirectManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.indirectManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.indirectmanpowercostperminute') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.meetinginvestigationcontent')"
                name="meetingInvestigationContent"
              >
                <a-textarea
                  v-model:value="formState.meetingInvestigationContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityFailureMeeting.meetinginvestigationcontent') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.meetinginvestigationcost')"
                name="meetingInvestigationCost"
              >
                <a-input-number
                  v-model:value="formState.meetingInvestigationCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.meetinginvestigationcost') })"
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
                :label="t('entity.qualityFailureMeeting.meetingtimeminutes')"
                name="meetingTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.meetingTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.meetingtimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.directparticipantcount')"
                name="directParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.directParticipantCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.directparticipantcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.indirectparticipantcount')"
                name="indirectParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.indirectParticipantCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.indirectparticipantcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.investigationworktimeminutes')"
                name="investigationWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.investigationWorkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.investigationworktimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.travelcost')"
                name="travelCost"
              >
                <a-input-number
                  v-model:value="formState.travelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.travelcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.otherexpenses')"
                name="otherExpenses"
              >
                <a-input-number
                  v-model:value="formState.otherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.otherexpenses') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.otherworktimeminutes')"
                name="otherWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.otherWorkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.otherworktimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.otherapparatuscost')"
                name="otherApparatusCost"
              >
                <a-input-number
                  v-model:value="formState.otherApparatusCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.otherapparatuscost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityFailureMeeting.meetingrecorder')"
                name="meetingRecorder"
              >
                <a-input
                  v-model:value="formState.meetingRecorder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.meetingrecorder') })"
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
 * 品质问题应对明细 - 会议/调查/试验费用维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-failure-meeting/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityFailureMeetingCreate } from '@/types/logistics/quality/cost/quality-failure-meeting'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","qualityFailureId","qualityFailureCode","lineNumber","directManpowerCostPerMinute","indirectManpowerCostPerMinute","meetingInvestigationContent","meetingInvestigationCost","meetingTimeMinutes","directParticipantCount","indirectParticipantCount","investigationWorkTimeMinutes","travelCost","otherExpenses","otherWorkTimeMinutes","otherApparatusCost","meetingRecorder","extFieldJson","remark"]


interface Props {
  formData?: Partial<QualityFailureMeetingCreate & { qualityFailureMeetingId?: string }> | null
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
    const isCreate = !props.formData?.qualityFailureMeetingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  qualityFailureId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.qualityfailureid') }),
      trigger: 'blur'
    }
  ],
  qualityFailureCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityFailureMeeting.qualityfailurecode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.linenumber') }),
      trigger: 'change'
    }
  ],
  directManpowerCostPerMinute: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.directmanpowercostperminute') }),
      trigger: 'change'
    }
  ],
  indirectManpowerCostPerMinute: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.indirectmanpowercostperminute') }),
      trigger: 'change'
    }
  ],
  meetingInvestigationCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.meetinginvestigationcost') }),
      trigger: 'change'
    }
  ],
  meetingTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.meetingtimeminutes') }),
      trigger: 'change'
    }
  ],
  directParticipantCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.directparticipantcount') }),
      trigger: 'change'
    }
  ],
  indirectParticipantCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.indirectparticipantcount') }),
      trigger: 'change'
    }
  ],
  investigationWorkTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.investigationworktimeminutes') }),
      trigger: 'change'
    }
  ],
  travelCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.travelcost') }),
      trigger: 'change'
    }
  ],
  otherExpenses: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.otherexpenses') }),
      trigger: 'change'
    }
  ],
  otherWorkTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.otherworktimeminutes') }),
      trigger: 'change'
    }
  ],
  otherApparatusCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityFailureMeeting.otherapparatuscost') }),
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
