<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference/components -->
<!-- 文件名称：conference-form.vue -->
<!-- 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form conference-form flex flex-col min-h-0"
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
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
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
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.conferenceId"
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
                  show-count
                  :maxlength="200"
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
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.starttime')"
                name="startTime"
              >
                <a-date-picker
                  v-model:value="formState.startTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.starttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.conference.endtime')"
                name="endTime"
              >
                <a-date-picker
                  v-model:value="formState.endTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.endtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                  show-count
                  :maxlength="200"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="2000"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="100"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.conference.reminderminutes')"
                name="reminderMinutes"
              >
                <a-input-number
                  v-model:value="formState.reminderMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.reminderminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.conference.roomid')"
                name="conferenceRoomId"
              >
                <a-input
                  v-model:value="formState.conferenceRoomId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.roomid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.conference.roomname')"
                name="conferenceRoomName"
              >
                <a-input
                  v-model:value="formState.conferenceRoomName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.roomname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
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
    <!-- 下：子表 participants -->
    <TaktEditableTable
      ref="conferenceParticipantTableRef"
      v-model="childConferenceParticipantRows"
      :columns="conferenceParticipantFormColumns"
      :title="t('entity.conferenceparticipant._self')"
      :add-button-entity="t('entity.conferenceparticipant._self')"
      id-field="conferenceParticipantId"
      :default-row="createDefaultConferenceParticipantRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/conference-center/conference/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ConferenceCreate } from '@/types/routine/conference-center/conference'
import { RiQuestionLine } from '@remixicon/vue'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","conferenceCode","title","conferenceType","conferenceStatus","startTime","endTime","location","meetingLink","agenda","content","summary","tags","organizerId","organizerName","deptId","deptName","maxParticipants","reminderMinutes","conferenceRoomId","conferenceRoomName","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childConferenceParticipantRows = ref<Record<string, unknown>[]>([])
const conferenceParticipantTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 conferenceParticipant 可编辑列 */
const conferenceParticipantFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'userId',
    title: t('entity.conferenceparticipant.userid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'userName',
    title: t('entity.conferenceparticipant.username'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'participantRole',
    title: t('entity.conferenceparticipant.participantrole'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'attendanceStatus',
    title: t('entity.conferenceparticipant.attendancestatus'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'checkInTime',
    title: t('entity.conferenceparticipant.checkintime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'checkOutTime',
    title: t('entity.conferenceparticipant.checkouttime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'checkInMethod',
    title: t('entity.conferenceparticipant.checkinmethod'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ConferenceCreate & { conferenceId?: string }> | null | undefined) {
  childConferenceParticipantRows.value = ((val as any)?.participants ?? []) as Record<string, unknown>[]
}

function createDefaultConferenceParticipantRow(): Record<string, unknown> {
  return {
    userId: '',
    userName: '',
    participantRole: 0,
    attendanceStatus: 0,
    checkInTime: '',
    checkOutTime: '',
    checkInMethod: 0,
    extField: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.conferenceId ?? ''
  return {
    ...formState,
    participants: conferenceParticipantTableRef.value?.getRows?.() ?? childConferenceParticipantRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      conferenceId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ConferenceCreate & { conferenceId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 conferenceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.conferenceId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).participants
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
    const isCreate = !props.formData?.conferenceId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
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
  conferenceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  conferenceStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.starttime') }),
      trigger: 'change'
    }
  ],
  endTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.conference.endtime') }),
      trigger: 'change'
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
  maxParticipants: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.maxparticipants') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.maxparticipants') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  reminderMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.reminderminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.conference.reminderminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await conferenceParticipantTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('conferenceType' in payload) {
    const rawconferenceType = payload.conferenceType
    payload.conferenceType = typeof rawconferenceType === 'number' ? rawconferenceType : Number(rawconferenceType)
  }
  if ('conferenceStatus' in payload) {
    const rawconferenceStatus = payload.conferenceStatus
    payload.conferenceStatus = typeof rawconferenceStatus === 'number' ? rawconferenceStatus : Number(rawconferenceStatus)
  }
  if ('maxParticipants' in payload) {
    const rawmaxParticipants = payload.maxParticipants
    payload.maxParticipants = typeof rawmaxParticipants === 'number' ? rawmaxParticipants : Number(rawmaxParticipants)
  }
  if ('reminderMinutes' in payload) {
    const rawreminderMinutes = payload.reminderMinutes
    payload.reminderMinutes = typeof rawreminderMinutes === 'number' ? rawreminderMinutes : Number(rawreminderMinutes)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.conferenceId)
  childConferenceParticipantRows.value = []
  conferenceParticipantTableRef.value?.resetRows?.()
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
