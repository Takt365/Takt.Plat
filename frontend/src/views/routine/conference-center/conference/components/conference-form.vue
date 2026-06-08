<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference/components -->
<!-- 文件名称：conference-form.vue -->
<!-- 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      <!-- 子表：conferenceParticipant -->
      <a-tab-pane
        key="child-participants"
        :tab="t('entity.conferenceParticipant._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddConferenceParticipantRow">
            {{ t('common.page.button.create') }}{{ t('entity.conferenceParticipant._self') }}
          </a-button>
        </div>
        <a-table
          :columns="conferenceParticipantFormColumns"
          :data-source="childConferenceParticipantRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'participantRole'">
              <a-input-number
                v-model:value="record.participantRole"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.participantrole') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'attendanceStatus'">
              <a-input-number
                v-model:value="record.attendanceStatus"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.attendancestatus') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'checkInTime'">
              <a-input
                v-model:value="record.checkInTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.checkintime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'checkOutTime'">
              <a-input
                v-model:value="record.checkOutTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceParticipant.checkouttime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveConferenceParticipantRow(index)">
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
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/conference-center/conference/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ConferenceCreate, ConferenceParticipantCreate, ConferenceParticipant } from '@/types/routine/conference-center/conference'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","conferenceCode","title","conferenceType","conferenceStatus","startTime","endTime","location","meetingLink","agenda","content","summary","tags","organizerId","organizerName","deptId","deptName","maxParticipants","reminderMinutes","flowInstanceId","extFieldJson","remark"]

/** conferenceParticipant 子表行（表单 Tab 内嵌） */
const childConferenceParticipantRows = ref<Record<string, unknown>[]>([])

/** 子表 conferenceParticipant 表单列定义 */
const conferenceParticipantFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.participantrole'),
    dataIndex: 'participantRole',
    key: 'participantRole',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.attendancestatus'),
    dataIndex: 'attendanceStatus',
    key: 'attendanceStatus',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.checkintime'),
    dataIndex: 'checkInTime',
    key: 'checkInTime',
    width: 140,
  },
  {
    title: t('entity.conferenceParticipant.checkouttime'),
    dataIndex: 'checkOutTime',
    key: 'checkOutTime',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ConferenceCreate & { conferenceId?: string }> | null | undefined) {
  childConferenceParticipantRows.value = ((val as any)?.participants ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.conferenceParticipantId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 conferenceParticipant 行 */
function handleAddConferenceParticipantRow() {
  childConferenceParticipantRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      userId: '',
      userName: '',
      participantRole: 0,
      attendanceStatus: 0,
      checkInTime: '',
      checkOutTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 conferenceParticipant 行 */
function handleRemoveConferenceParticipantRow(index: number) {
  childConferenceParticipantRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    participants: childConferenceParticipantRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ConferenceCreate & { conferenceId?: string }> | null
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
    delete (next as any).participants
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
  childConferenceParticipantRows.value = []
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
