<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference/components -->
<!-- 文件名称：conference-form.vue -->
<!-- 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="conference-form-tabs"
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
                :label="t('entity.conference.code')"
                name="conferenceCode"
              >
                <a-input
                  v-model:value="formState.conferenceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.type')"
                name="conferenceType"
              >
                <a-input-number
                  v-model:value="formState.conferenceType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.status')"
                name="conferenceStatus"
              >
                <a-input-number
                  v-model:value="formState.conferenceStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.starttime')"
                name="startTime"
              >
                <a-input
                  v-model:value="formState.startTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.starttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.endtime')"
                name="endTime"
              >
                <a-input
                  v-model:value="formState.endTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.endtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.location')"
                name="location"
              >
                <a-input
                  v-model:value="formState.location"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.location') })"
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
                :label="t('entity.conference.meetinglink')"
                name="meetingLink"
              >
                <a-input
                  v-model:value="formState.meetingLink"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.meetinglink') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.agenda')"
                name="agenda"
              >
                <a-input
                  v-model:value="formState.agenda"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.agenda') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.conference.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.conference.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.summary')"
                name="summary"
              >
                <a-input
                  v-model:value="formState.summary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.summary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.tags') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.organizerid')"
                name="organizerId"
              >
                <a-input
                  v-model:value="formState.organizerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.organizerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.organizername')"
                name="organizerName"
              >
                <a-input
                  v-model:value="formState.organizerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.organizername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.maxparticipants')"
                name="maxParticipants"
              >
                <a-input-number
                  v-model:value="formState.maxParticipants"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.maxparticipants') })"
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
                :label="t('entity.conference.reminderminutes')"
                name="reminderMinutes"
              >
                <a-input-number
                  v-model:value="formState.reminderMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.reminderminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.participants')"
                name="participants"
              >
                <a-input
                  v-model:value="formState.participants"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.participants') })"
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
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/conference-center/conference/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ConferenceCreate } from '@/types/routine/conference-center/conference'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","conferenceCode","title","conferenceType","conferenceStatus","startTime","endTime","location","meetingLink","agenda","content","summary","tags","organizerId","organizerName","deptId","deptName","maxParticipants","reminderMinutes","flowInstanceId","participants","extFieldJson","remark"]


interface Props {
  formData?: Partial<ConferenceCreate & { conferenceId?: string }> | null
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
    const isCreate = !props.formData?.conferenceId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  conferenceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.code') }),
      trigger: 'blur'
    }
  ],
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.title') }),
      trigger: 'blur'
    }
  ],
  conferenceType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.type') }),
      trigger: 'change'
    }
  ],
  conferenceStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.status') }),
      trigger: 'change'
    }
  ],
  startTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.starttime') }),
      trigger: 'blur'
    }
  ],
  endTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.endtime') }),
      trigger: 'blur'
    }
  ],
  organizerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.organizerid') }),
      trigger: 'blur'
    }
  ],
  organizerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.conference.organizername') }),
      trigger: 'blur'
    }
  ],
  maxParticipants: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.maxparticipants') }),
      trigger: 'change'
    }
  ],
  reminderMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.reminderminutes') }),
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
