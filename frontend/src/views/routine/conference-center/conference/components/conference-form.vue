<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference/components -->
<!-- 文件名称：conference-form.vue -->
<!-- 功能描述：会议中心主实体维护弹窗内嵌表单（上主下从级联保存）；会议内容使用 takt-rich-editor。defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form conference-form flex flex-col min-h-0 overflow-visible"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '会议中心' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.conferenceId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceCode')"
                name="conferenceCode"
              >
                <a-input
                  v-model:value="formState.conferenceCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceTitle')"
                name="conferenceTitle"
              >
                <a-input
                  v-model:value="formState.conferenceTitle"
                  :placeholder="pi.ph('conferenceTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceType')"
                name="conferenceType"
              >
                <TaktSelect
                  v-model:value="formState.conferenceType"
                  dict-type="routine_conference_type"
                  :placeholder="pi.ph('conferenceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceStatus')"
                name="conferenceStatus"
              >
                <TaktSelect
                  v-model:value="formState.conferenceStatus"
                  dict-type="routine_conference_status"
                  :placeholder="pi.ph('conferenceStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('startTime')"
                name="startTime"
              >
                <a-date-picker
                  v-model:value="formState.startTime"
                  :placeholder="pi.ph('startTime')"
                  show-time
                  value-format="YYYY-MM-DD HH:mm:ss"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('endTime')"
                name="endTime"
              >
                <a-date-picker
                  v-model:value="formState.endTime"
                  :placeholder="pi.ph('endTime')"
                  show-time
                  value-format="YYYY-MM-DD HH:mm:ss"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('location')"
                name="location"
              >
                <a-input
                  v-model:value="formState.location"
                  :placeholder="pi.ph('location')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingLink')"
                name="meetingLink"
              >
                <a-input
                  v-model:value="formState.meetingLink"
                  :placeholder="pi.ph('meetingLink')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('conferenceContent')"
                name="conferenceContent"
              >
                <takt-rich-editor
                  v-model:value="formState.conferenceContent"
                  :placeholder="pi.ph('conferenceContent')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('agenda')"
                name="agenda"
              >
                <takt-rich-editor
                  v-model:value="formState.agenda"
                  :placeholder="pi.ph('agenda')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceSummary')"
                name="conferenceSummary"
              >
                <a-input
                  v-model:value="formState.conferenceSummary"
                  :placeholder="pi.ph('conferenceSummary')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceTags')"
                name="conferenceTags"
              >
                <a-input
                  v-model:value="formState.conferenceTags"
                  :placeholder="pi.ph('conferenceTags')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('organizerId')"
                name="organizerId"
              >
                <TaktSelect
                  v-model:value="formState.organizerId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('organizerId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('organizerName')"
                name="organizerName"
              >
                <a-input
                  v-model:value="formState.organizerName"
                  :placeholder="pi.ph('organizerName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptId')"
                name="deptId"
              >
                <TaktSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('deptId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxParticipants')"
                name="maxParticipants"
              >
                <a-input-number
                  v-model:value="formState.maxParticipants"
                  :placeholder="pi.ph('maxParticipants')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reminderMinutes')"
                name="reminderMinutes"
              >
                <a-input-number
                  v-model:value="formState.reminderMinutes"
                  :placeholder="pi.ph('reminderMinutes')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceRoomId')"
                name="conferenceRoomId"
              >
                <TaktSelect
                  v-model:value="formState.conferenceRoomId"
                  api-url="TaktConferenceRooms/options"
                  :placeholder="pi.ph('conferenceRoomId')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conferenceRoomName')"
                name="conferenceRoomName"
              >
                <a-input
                  v-model:value="formState.conferenceRoomName"
                  :placeholder="pi.ph('conferenceRoomName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <TaktEditableTable
      ref="conferenceParticipantTableRef"
      v-model="childConferenceParticipantRows"
      :columns="conferenceParticipantFormColumns"
      :title="conferenceParticipantPi.self()"
      :add-button-entity="conferenceParticipantPi.self()"
      id-field="conferenceParticipantId"
      :default-row="createDefaultConferenceParticipantRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-userId="{ record }">
        <TaktSelect
          v-model:value="record.userId"
          api-url="TaktUsers/options"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="conferenceParticipantPi.ph('userId')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-participantRole="{ record }">
        <TaktSelect
          v-model:value="record.participantRole"
          dict-type="routine_conference_participant_role"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="conferenceParticipantPi.ph('participantRole')"
          :disabled="loading"
        />
      </template>
      <template #cell-attendanceStatus="{ record }">
        <TaktSelect
          v-model:value="record.attendanceStatus"
          dict-type="routine_conference_attendance_status"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="conferenceParticipantPi.ph('attendanceStatus')"
          :disabled="loading"
        />
      </template>
      <template #cell-checkInMethod="{ record }">
        <TaktSelect
          v-model:value="record.checkInMethod"
          dict-type="routine_conference_check_in_method"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="conferenceParticipantPi.ph('checkInMethod')"
          :disabled="loading"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体维护表单；会议内容 / 议程使用 takt-rich-editor
 * @module views/routine/conference-center/conference/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { RiQuestionLine } from '@remixicon/vue'
import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { useTaktFormNumbering } from '@/composables/use-takt-form-numbering'
import { useConferenceI18n } from '../composables/use-conference-i18n'
import { useConferenceParticipantI18n } from '../composables/use-conference-participant-i18n'
import type { Conference } from '@/types/routine/conference-center/conference'

/** 实体字段 i18n */
const pi = useConferenceI18n()
/** 子表字段 i18n */
const conferenceParticipantPi = useConferenceParticipantI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()

/** Create 字段名列表（与 formState 键对齐） */
const formFields = [
  'tenantCode',
  'companyCode',
  'cultureCode',
  'plantCode',
  'conferenceCode',
  'conferenceTitle',
  'conferenceType',
  'conferenceStatus',
  'startTime',
  'endTime',
  'location',
  'meetingLink',
  'conferenceContent',
  'agenda',
  'conferenceSummary',
  'conferenceTags',
  'organizerId',
  'organizerName',
  'deptId',
  'deptName',
  'maxParticipants',
  'reminderMinutes',
  'conferenceRoomId',
  'conferenceRoomName',
  'extField',
  'remark',
]
/** 表单内容区高度 class */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/**
 * 上下文隔离字段注入
 * @param target 表单数据
 * @param force 新增态强制覆盖
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}

/** 弹窗内 TaktSelect 下拉挂载到 body */
function resolveSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childConferenceParticipantRows = ref<Record<string, unknown>[]>([])
const conferenceParticipantTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/**
 * 子表行是否已持久化
 * @param row 行数据
 */
function isPersistedConferenceParticipantRow(row: Record<string, unknown>): boolean {
  const id = row.conferenceParticipantId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一子表行号 */
function allocateNextConferenceParticipantLineNumber(): number {
  const rows = conferenceParticipantTableRef.value?.getRows?.() ?? childConferenceParticipantRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 conferenceParticipant 可编辑列 */
const conferenceParticipantFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: conferenceParticipantPi.label('lineNumber'),
    editor: 'inputNumber',
    min: 0,
    width: 100,
  },
  {
    key: 'userId',
    title: conferenceParticipantPi.label('userId'),
    width: 160,
    required: true,
  },
  {
    key: 'userName',
    title: conferenceParticipantPi.label('userName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'participantRole',
    title: conferenceParticipantPi.label('participantRole'),
    width: 140,
  },
  {
    key: 'attendanceStatus',
    title: conferenceParticipantPi.label('attendanceStatus'),
    width: 140,
  },
  {
    key: 'checkInTime',
    title: conferenceParticipantPi.label('checkInTime'),
    editor: 'datePicker',
    showTime: true,
    valueFormat: 'YYYY-MM-DD HH:mm:ss',
    width: 180,
  },
  {
    key: 'checkOutTime',
    title: conferenceParticipantPi.label('checkOutTime'),
    editor: 'datePicker',
    showTime: true,
    valueFormat: 'YYYY-MM-DD HH:mm:ss',
    width: 180,
  },
  {
    key: 'checkInMethod',
    title: conferenceParticipantPi.label('checkInMethod'),
    width: 140,
  },
])

/**
 * 编辑态从 formData 同步子表行
 * @param val 主表 DTO
 */
function syncChildRowsFromFormData(val: Partial<Conference & { conferenceId?: string }> | null | undefined) {
  const rows = ((val as { participants?: Record<string, unknown>[] })?.participants ?? []) as Record<string, unknown>[]
  childConferenceParticipantRows.value = rows
}

function createDefaultConferenceParticipantRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextConferenceParticipantLineNumber(),
    userId: '',
    userName: '',
    participantRole: 0,
    attendanceStatus: 0,
    checkInMethod: 0,
  }
}

/** 组装 Create/Update 载荷 */
function buildSubmitPayload() {
  const masterId = props.formData?.conferenceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    participants: (conferenceParticipantTableRef.value?.getRows?.() ?? childConferenceParticipantRows.value).map((row) => {
      const normalized: Record<string, unknown> = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        conferenceId: masterId,
      }
      if (isUpdate && isPersistedConferenceParticipantRow(row)) {
        normalized.conferenceParticipantId = row.conferenceParticipantId
      } else {
        delete normalized.conferenceParticipantId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO */
interface Props {
  formData?: Partial<Conference & { conferenceId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

const formRef = ref()
const formState = reactive<Record<string, unknown>>({})
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  conferenceType: 0,
  conferenceStatus: 0,
  maxParticipants: 0,
  reminderMinutes: 0,
}

function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

watch(
  () => props.formData,
  (val) => {
    if (val?.conferenceId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as { participants?: unknown }).participants
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
      applyScopeDefaults(formState, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true },
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.conferenceId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.conferenceId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'conferenceCode',
})

const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  conferenceCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  conferenceTitle: [
    { required: true, message: pi.ph('conferenceTitle'), trigger: 'blur' },
  ],
  conferenceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('conferenceType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('conferenceType'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  startTime: [
    { required: true, message: pi.ph('startTime'), trigger: 'change' },
  ],
  endTime: [
    { required: true, message: pi.ph('endTime'), trigger: 'change' },
  ],
}))

async function validate() {
  await formRef.value?.validate()
  await conferenceParticipantTableRef.value?.validate?.()
  return formState
}

function getValues(): Record<string, unknown> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    const scopedPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.conferenceId) {
    payload.conferenceId = props.formData.conferenceId
    delete payload.numberingRuleCode
  }
  return payload
}

function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState, !props.formData?.conferenceId)
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
