<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/message/components -->
<!-- 文件名称：message-form.vue -->
<!-- 功能描述：在线消息新增表单；两步提交：① createMessage 落库 ② sendMessageById SignalR 推送；defineExpose validate/getValues/resetFields/submitCreateAndPushAsync -->
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
    <a-row :gutter="24">
      <a-col :span="8">
        <a-form-item
          :label="t('common.page.entity.tenantcode')"
          name="tenantCode"
        >
          <a-input
            v-model:value="formState.tenantCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="8">
        <a-form-item
          :label="t('common.page.entity.companycode')"
          name="companyCode"
        >
          <a-input
            v-model:value="formState.companyCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="8">
        <a-form-item
          :label="t('common.page.entity.companydefaultculture')"
          name="companyDefaultCulture"
        >
          <a-input
            v-model:value="formState.companyDefaultCulture"
            :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.fromusername')"
          name="fromUserName"
        >
          <a-input
            v-model:value="formState.fromUserName"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.message.fromusername') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.fromuserid')"
          name="fromUserId"
        >
          <a-input
            v-model:value="formState.fromUserId"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.fromuserid') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="isCreate"
        :span="24"
      >
        <a-form-item
          :label="t('entity.message.tousernames')"
          name="recipientMode"
        >
          <a-radio-group
            v-model:value="formState.recipientMode"
            :disabled="loading"
          >
            <a-radio
              v-if="canUseSendToAll"
              value="all"
            >
              {{ t('foundation.message.page.recipient.all') }}
            </a-radio>
            <a-radio value="list">
              {{ t('foundation.message.page.recipient.list.label') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
      </a-col>
      <a-col
        v-if="isCreate && formState.recipientMode === 'list'"
        :span="24"
      >
        <a-form-item
          :label="t('foundation.message.page.recipient.list.select')"
          name="toUserIds"
        >
          <TaktSelect
            v-model="formState.toUserIds"
            api-url="TaktUsers/options"
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            :placeholder="t('foundation.message.page.recipient.list.placeholder')"
            size="small"
            show-search
            multiple
            :max-tag-count="5"
            @change="handleToUserListChange"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="!isCreate"
        :span="12"
      >
        <a-form-item
          :label="t('entity.message.tousername')"
          name="toUserName"
        >
          <a-input
            v-model:value="formState.toUserName"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="!isCreate"
        :span="12"
      >
        <a-form-item
          :label="t('entity.message.touserid')"
          name="toUserId"
        >
          <a-input
            v-model:value="formState.toUserId"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.touserid') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.iscc')"
          name="isCc"
        >
          <TaktSelect
            v-model:value="formState.isCc"
            dict-type="sys_yes_no_type"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.message.title')"
          name="messageTitle"
        >
          <a-input
            v-model:value="formState.messageTitle"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.title') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.type')"
          name="messageType"
        >
          <TaktSelect
            v-model:value="formState.messageType"
            dict-type="sys_message_type"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.type') })"
            size="small"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="needsFileUpload"
        :span="24"
      >
        <a-form-item
          :label="t('entity.message.attachments')"
          name="attachments"
        >
          <a-upload
            v-model:file-list="uploadFileList"
            :accept="uploadAccept"
            :list-type="uploadListType"
            :max-count="1"
            :disabled="loading || fileUploading"
            :before-upload="handleBeforeFileUpload"
            @remove="handleFileRemove"
          >
            <template v-if="uploadListType === 'picture-card'">
              <div v-if="uploadFileList.length < 1">
                <plus-outlined />
              </div>
            </template>
            <a-button
              v-else
              size="small"
              :loading="fileUploading"
            >
              {{ t('foundation.message.page.upload.select') }}
            </a-button>
          </a-upload>
          <div class="mt-1 text-xs text-text-secondary">
            {{ uploadHintText }}
          </div>
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.group')"
          name="messageGroup"
        >
          <TaktSelect
            v-model:value="formState.messageGroup"
            dict-type="sys_message_group_category"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.group') })"
            size="small"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.message.content')"
          name="messageContent"
        >
          <a-textarea
            v-model:value="formState.messageContent"
            :placeholder="messageContentPlaceholder"
            :rows="3"
            size="small"
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 在线消息新增表单（两步：先落库，再 SignalR 推送）
 * @module views/foundation/message/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { PlusOutlined } from '@ant-design/icons-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { Message, MessageCreate, MessageBatchCreate, MessageAttachmentItem } from '@/types/foundation/message'
import { createAndSendMessages } from '@/api/foundation/message'
import { getFileById } from '@/api/foundation/file'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { usePermissionStore } from '@/stores/identity/permission'
import { uploadTaktFileSmart } from '@/utils/takt-file-chunk-upload'

/** 两步提交结果：批量落库 + SignalR 推送 */
export interface MessageFormCreateResult {
  /** 已落库消息（首条） */
  created?: Message
  /** 批量落库结果 */
  createdList: Message[]
  /** 是否已完成推送（batch-send 成功即 true） */
  pushed: boolean
  /** 推送失败 */
  pushFailed?: boolean
}

/** 接收者模式：全员广播 / 指定用户列表 */
type MessageRecipientMode = 'all' | 'list'

/** 解析后的接收者（用户 ID + 登录名） */
interface MessageRecipientItem {
  id: string
  userName: string
}

/** sys_message_type 字典「文本」对应 dictValue（种子 TaktDictDataSeedData） */
const DEFAULT_MESSAGE_TYPE = 'text'
/** sys_message_group_category 字典「协同」对应 dictValue */
const DEFAULT_MESSAGE_GROUP = 'collaboration'

/** 列表模式最多可选接收者数 */
const MAX_RECIPIENT_LIST_SIZE = 5

/** 超级管理员 UserType（与 TaktUserType.SuperAdmin 对齐） */
const SUPER_ADMIN_USER_TYPE = 2

/** 字典 dictValue → 后端 TaktMessageType 枚举（与 sys_message_type 一致） */
const MESSAGE_TYPE_DICT_TO_API: Record<string, number> = {
  text: 1,
  image: 2,
  file: 3,
  takt365: 4,
  video: 5,
  voice: 6,
}

/** 需要上传附件的消息类型 dictValue（非文本、非系统消息） */
const MESSAGE_TYPE_NEEDS_FILE = new Set(['image', 'file', 'video', 'voice'])

/** API 数值 → 字典 dictValue */
const MESSAGE_TYPE_API_TO_DICT: Record<number, string> = {
  1: 'text',
  2: 'image',
  3: 'file',
  4: 'takt365',
  5: 'video',
  6: 'voice',
}

/** 字典 dictValue → 后端 TaktMessageGroup 枚举（与 sys_message_group_category 一致） */
const MESSAGE_GROUP_DICT_TO_API: Record<string, number> = {
  collaboration: 1,
  officialdoc: 2,
  document: 3,
  announcement: 4,
  other: 5,
  message: 6,
  reminder: 7,
}

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：按钮权限（第二步 SignalR 发送） */
const permissionStore = usePermissionStore()
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'companyCode', 'companyDefaultCulture', 'fromUserName', 'fromUserId', 'toUserName', 'recipientMode', 'toUserIds', 'toUserId', 'isCc', 'messageTitle', 'messageContent', 'messageType', 'messageGroup', 'attachments']
/** TaktYesNo：是 */
const DEFAULT_IS_CC = 1

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（表单只读）
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

/**
 * 新增态：发送者默认为当前登录用户（只读展示）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
 */
function applySenderDefaults(target: Record<string, unknown>, force = false) {
  const currentUserName = userStore.username || userStore.userInfo?.username || ''
  const currentUserId = userStore.userId || (userStore.userInfo?.userId != null ? String(userStore.userInfo.userId) : '')
  if (formFields.includes('fromUserName') && (force || !target.fromUserName)) {
    target.fromUserName = currentUserName
  }
  if (formFields.includes('fromUserId') && (force || !target.fromUserId)) {
    target.fromUserId = currentUserId
  }
}

/**
 * 新增态业务默认值
 * @param target 表单数据
 */
function applyCreateDefaults(target: Record<string, unknown>) {
  applySenderDefaults(target, true)
  const canSendToAll = userStore.userType === SUPER_ADMIN_USER_TYPE
  if (target.recipientMode === 'all' && !canSendToAll) {
    target.recipientMode = 'list'
  }
  if (target.recipientMode !== 'all' && target.recipientMode !== 'list') {
    target.recipientMode = 'list'
  }
  if (!Array.isArray(target.toUserIds)) {
    target.toUserIds = []
  }
  if (target.messageType === undefined || target.messageType === null || target.messageType === '') {
    target.messageType = DEFAULT_MESSAGE_TYPE
  }
  if (target.messageGroup === undefined || target.messageGroup === null || target.messageGroup === '') {
    target.messageGroup = DEFAULT_MESSAGE_GROUP
  }
  if (target.isCc === undefined || target.isCc === null || target.isCc === '') {
    target.isCc = DEFAULT_IS_CC
  }
}

/**
 * 表单 messageType 转为 API 数值枚举
 * @param value 字典值或数值
 * @returns {number} TaktMessageType
 */
function resolveMessageTypeForApi(value: unknown): number {
  if (typeof value === 'number' && !Number.isNaN(value)) {
    return value
  }
  if (typeof value === 'string' && value in MESSAGE_TYPE_DICT_TO_API) {
    return MESSAGE_TYPE_DICT_TO_API[value]
  }
  const parsed = Number(value)
  return Number.isFinite(parsed) ? parsed : MESSAGE_TYPE_DICT_TO_API[DEFAULT_MESSAGE_TYPE]
}

/**
 * 表单 messageGroup 转为 API 数值枚举
 * @param value 字典值或数值
 * @returns {number} TaktMessageGroup
 */
/**
 * 表单 messageType 规范为字典 dictValue
 * @param value 字典值或 API 数值
 * @returns {string} sys_message_type dictValue
 */
function resolveMessageTypeDictValue(value: unknown): string {
  if (typeof value === 'string' && value.trim() !== '') {
    return value.trim()
  }
  const apiValue = resolveMessageTypeForApi(value)
  return MESSAGE_TYPE_API_TO_DICT[apiValue] ?? DEFAULT_MESSAGE_TYPE
}

function resolveMessageGroupForApi(value: unknown): number {
  if (typeof value === 'number' && !Number.isNaN(value)) {
    return value
  }
  if (typeof value === 'string' && value in MESSAGE_GROUP_DICT_TO_API) {
    return MESSAGE_GROUP_DICT_TO_API[value]
  }
  const parsed = Number(value)
  return Number.isFinite(parsed) ? parsed : MESSAGE_GROUP_DICT_TO_API[DEFAULT_MESSAGE_GROUP]
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MessageCreate & { messageId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** 是否新增态（无 messageId） */
const isCreate = computed(() => !props.formData?.messageId)

/** 是否可使用「当前公司全部用户」接收模式（仅超级管理员） */
const canUseSendToAll = computed(() => userStore.userType === SUPER_ADMIN_USER_TYPE)

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 附件上传中 */
const fileUploading = ref(false)
/** 已上传附件展示名 */
const uploadedFileLabel = ref('')
/** 上传组件文件列表（仅 UI） */
const uploadFileList = ref<UploadFile[]>([])

/** 当前消息类型 dictValue */
const messageTypeDictValue = computed(() => resolveMessageTypeDictValue(formState.messageType))

/** 是否需上传附件（图片/文件/视频/语音） */
const needsFileUpload = computed(() => MESSAGE_TYPE_NEEDS_FILE.has(messageTypeDictValue.value))

/** 上传 accept */
const uploadAccept = computed(() => {
  switch (messageTypeDictValue.value) {
    case 'image':
      return 'image/*'
    case 'video':
      return 'video/*'
    case 'voice':
      return 'audio/*'
    default:
      return undefined
  }
})

/** 图片用卡片样式，其余用文本列表 */
const uploadListType = computed<UploadProps['listType']>(() => (
  messageTypeDictValue.value === 'image' ? 'picture-card' : 'text'
))

/** 上传区提示文案 */
const uploadHintText = computed(() => {
  switch (messageTypeDictValue.value) {
    case 'image':
      return t('foundation.message.page.upload.image.hint')
    case 'video':
      return t('foundation.message.page.upload.video.hint')
    case 'voice':
      return t('foundation.message.page.upload.voice.hint')
    default:
      return t('foundation.message.page.upload.file.hint')
  }
})

/** 消息内容占位：附件类型可为选填说明 */
const messageContentPlaceholder = computed(() => {
  if (needsFileUpload.value) {
    return t('foundation.message.page.upload.content.optional')
  }
  return t('common.page.form.placeholder.required', { field: t('entity.message.content') })
})

/** 已解析的列表模式接收者（与 toUserIds 同步） */
const recipientUsers = ref<MessageRecipientItem[]>([])

/** 编辑态灌入 formData；新增态 reset 并注入默认值 */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    applyScopeDefaults(next)
    const isCreate = !props.formData?.messageId
    if (isCreate) {
      applyCreateDefaults(next)
    }
    Object.assign(formState, next)
    recipientUsers.value = []
    syncUploadFileListFromState()
  },
  { immediate: true, deep: true }
)

/** 切换为文本/系统消息时清空附件 */
watch(messageTypeDictValue, (next, prev) => {
  const nextNeeds = MESSAGE_TYPE_NEEDS_FILE.has(next)
  const prevNeeds = prev != null && MESSAGE_TYPE_NEEDS_FILE.has(prev)
  if (prevNeeds && !nextNeeds) {
    clearUploadedFile()
  }
  if (nextNeeds && prev != null && next !== prev) {
    clearUploadedFile()
  }
})

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (isCreate.value) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 用户资料就绪后，新增态同步发送者 */
watch(
  () => [userStore.userId, userStore.username, userStore.userInfo?.userId, userStore.userInfo?.username] as const,
  () => {
    if (isCreate.value) {
      applySenderDefaults(formState, true)
    }
  },
)

/** 非超级管理员不可使用全员模式，自动切回指定用户 */
watch(
  () => userStore.userType,
  () => {
    if (!isCreate.value) {
      return
    }
    if (!canUseSendToAll.value && formState.recipientMode === 'all') {
      formState.recipientMode = 'list'
    }
  },
)

/**
 * 从下拉选项解析登录用户名
 * @param option 选中项
 * @returns {string} 登录用户名
 */
function resolveUserNameFromSelectOption(
  option: { label?: string; dictLabel?: string; extValue?: string | number } | null | undefined,
): string {
  const extUserName = option?.extValue != null ? String(option.extValue).trim() : ''
  if (extUserName) {
    return extUserName
  }
  const displayLabel = (option?.dictLabel ?? option?.label ?? '').trim()
  const parenIndex = displayLabel.indexOf('(')
  return parenIndex > 0 ? displayLabel.slice(0, parenIndex).trim() : displayLabel
}

/**
 * 列表模式接收者多选变更：同步 toUserIds 与 recipientUsers
 * @param value 选中用户 ID 列表
 * @param option 选中项列表
 */
function handleToUserListChange(
  value: string | number | (string | number)[] | undefined,
  option: { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number } | { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number }[] | null,
) {
  let ids = Array.isArray(value)
    ? value.map((item) => String(item).trim()).filter(Boolean)
    : value != null && String(value).trim() !== ''
      ? [String(value).trim()]
      : []
  if (ids.length > MAX_RECIPIENT_LIST_SIZE) {
    ids = ids.slice(0, MAX_RECIPIENT_LIST_SIZE)
    message.warning(t('foundation.message.page.recipient.list.max', { max: MAX_RECIPIENT_LIST_SIZE }))
  }
  formState.toUserIds = ids
  const options = Array.isArray(option) ? option : option != null ? [option] : []
  recipientUsers.value = ids.map((id) => {
    const matched = options.find((item) => {
      const optionValue = item?.dictValue ?? item?.value
      return optionValue != null && String(optionValue).trim() === id
    })
    return {
      id,
      userName: resolveUserNameFromSelectOption(matched),
    }
  })
}

/** 切换接收者模式时清空列表选择 */
watch(
  () => formState.recipientMode as MessageRecipientMode | undefined,
  (mode) => {
    if (!isCreate.value) {
      return
    }
    if (mode === 'all') {
      formState.toUserIds = []
      recipientUsers.value = []
    }
  },
)

/**
 * 获取列表模式下的接收者清单（校验后调用）
 * @returns {MessageRecipientItem[]} 接收者列表
 */
function resolveRecipientList(): MessageRecipientItem[] {
  const ids = Array.isArray(formState.toUserIds)
    ? formState.toUserIds.map((item: unknown) => String(item).trim()).filter(Boolean)
    : []
  return ids.map((id) => {
    const cached = recipientUsers.value.find((item) => item.id === id)
    return cached ?? { id, userName: '' }
  })
}

/**
 * 解析 Attachments JSON
 * @param raw JSON 字符串
 * @returns {MessageAttachmentItem[]} 附件项列表
 */
function parseMessageAttachments(raw: unknown): MessageAttachmentItem[] {
  if (raw == null || typeof raw !== 'string' || !raw.trim()) {
    return []
  }
  try {
    const parsed = JSON.parse(raw) as unknown
    if (!Array.isArray(parsed)) {
      return []
    }
    return parsed.filter((item): item is MessageAttachmentItem => {
      return item != null
        && typeof item === 'object'
        && typeof (item as MessageAttachmentItem).accessUrl === 'string'
        && (item as MessageAttachmentItem).accessUrl.trim() !== ''
    })
  } catch {
    return []
  }
}

/**
 * 是否包含有效附件
 * @param raw JSON 字符串
 * @returns {boolean} 是否有附件
 */
function hasMessageAttachments(raw: unknown): boolean {
  return parseMessageAttachments(raw).length > 0
}

/**
 * 根据 formState.attachments 同步上传列表展示
 */
function syncUploadFileListFromState() {
  const items = parseMessageAttachments(formState.attachments)
  const first = items[0]
  if (!first?.accessUrl) {
    uploadFileList.value = []
    return
  }
  const url = first.accessUrl.trim()
  uploadFileList.value = [{
    uid: '-1',
    name: uploadedFileLabel.value || first.fileOriginalName || first.fileName || url.split('/').pop() || 'file',
    status: 'done',
    url: messageTypeDictValue.value === 'image' ? url : undefined,
  }]
}

/**
 * 清空已上传附件
 */
function clearUploadedFile() {
  formState.attachments = ''
  uploadedFileLabel.value = ''
  uploadFileList.value = []
}

/**
 * 上传至 TaktFiles 并写入 Attachments JSON
 * @param file 本地文件
 * @returns {Promise<boolean>} 是否拦截默认上传
 */
async function handleBeforeFileUpload(file: globalThis.File): Promise<boolean> {
  if (props.loading || fileUploading.value) {
    return false
  }
  fileUploading.value = true
  try {
    const result = await uploadTaktFileSmart(file)
    let accessUrl = result.accessUrl?.trim() ?? ''
    if (!accessUrl && result.fileId) {
      const detail = await getFileById(result.fileId)
      accessUrl = detail.accessUrl?.trim() ?? ''
    }
    if (!accessUrl) {
      message.error(t('foundation.message.page.upload.failed'))
      return false
    }
    const item: MessageAttachmentItem = {
      fileId: result.fileId,
      fileName: result.fileName,
      fileOriginalName: result.fileOriginalName,
      accessUrl,
      fileSize: result.fileSize,
      fileType: result.fileType,
      fileExtension: result.fileExtension,
      sortOrder: 1,
    }
    formState.attachments = JSON.stringify([item])
    uploadedFileLabel.value = result.fileOriginalName || result.fileName || file.name
    syncUploadFileListFromState()
    message.success(t('foundation.message.page.upload.success'))
  } catch {
    message.error(t('foundation.message.page.upload.failed'))
    return false
  } finally {
    fileUploading.value = false
  }
  return false
}

/**
 * 移除已上传附件
 * @returns {boolean} 允许移除
 */
function handleFileRemove(): boolean {
  clearUploadedFile()
  return true
}

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  fromUserName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.message.fromusername') }),
      trigger: 'blur'
    }
  ],
  toUserIds: [
    {
      validator: async () => {
        if (!isCreate.value || formState.recipientMode !== 'list') {
          return Promise.resolve()
        }
        const ids = Array.isArray(formState.toUserIds) ? formState.toUserIds : []
        if (ids.length === 0) {
          return Promise.reject(t('foundation.message.page.recipient.list.required'))
        }
        if (ids.length > MAX_RECIPIENT_LIST_SIZE) {
          return Promise.reject(t('foundation.message.page.recipient.list.max', { max: MAX_RECIPIENT_LIST_SIZE }))
        }
        return Promise.resolve()
      },
      trigger: 'change',
    },
  ],
  messageContent: needsFileUpload.value
    ? []
    : [
        {
          required: true,
          message: t('common.page.form.placeholder.required', { field: t('entity.message.content') }),
          trigger: 'blur',
        },
      ],
  attachments: needsFileUpload.value
    ? [
        {
          validator: async (_rule, value) => {
            if (hasMessageAttachments(value)) {
              return Promise.resolve()
            }
            return Promise.reject(t('foundation.message.page.upload.required'))
          },
          trigger: 'change',
        },
      ]
    : [],
  messageType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.message.type') }),
      trigger: 'change'
    }
  ],
  messageGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.message.group') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射消息正文为批量创建 DTO（不含接收者） */
function getMessageBody(): Omit<MessageBatchCreate, 'sendToAll' | 'toUserIds'> {
  const fromUserId = formState.fromUserId != null && String(formState.fromUserId).trim() !== ''
    ? String(formState.fromUserId).trim()
    : undefined
  const attachments = formState.attachments != null && String(formState.attachments).trim() !== ''
    ? String(formState.attachments).trim()
    : undefined
  return {
    fromUserName: formState.fromUserName,
    fromUserId,
    messageTitle: formState.messageTitle,
    messageContent: formState.messageContent ?? '',
    attachments,
    messageType: resolveMessageTypeForApi(formState.messageType),
    messageGroup: resolveMessageGroupForApi(formState.messageGroup),
    isCc: resolveYesNoForApi(formState.isCc),
  }
}

/**
 * 表单是否抄送转为 API 数值（TaktYesNo）
 * @param value 字典值或数值
 * @returns {number} 0=否，1=是
 */
function resolveYesNoForApi(value: unknown): number {
  if (value === 1 || value === '1' || value === true) {
    return 1
  }
  if (value === 0 || value === '0' || value === false) {
    return 0
  }
  const parsed = Number(value)
  return parsed === 0 ? 0 : DEFAULT_IS_CC
}

/** 映射为 Create DTO（字典值转后端枚举；不含表单只读隔离字段） */
function getValues(recipient?: MessageRecipientItem): Record<string, any> {
  const body = getMessageBody()
  const toUserId = recipient?.id
    ?? (formState.toUserId != null && String(formState.toUserId).trim() !== ''
      ? String(formState.toUserId).trim()
      : undefined)
  const toUserName = recipient?.userName
    ?? (formState.toUserName != null ? String(formState.toUserName).trim() : '')
  return {
    ...body,
    toUserName,
    toUserId,
  }
}

/** 重置表单 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  recipientUsers.value = []
  clearUploadedFile()
}

/**
 * 批量落库并推送：全员或指定用户列表（服务端逐人 Create + Send）
 * @returns {Promise<MessageFormCreateResult>} 落库结果与推送状态
 */
async function submitCreateAndPushAsync(): Promise<MessageFormCreateResult> {
  await validate()
  const mode = formState.recipientMode as MessageRecipientMode
  if (mode === 'all' && !canUseSendToAll.value) {
    throw new Error(t('foundation.message.page.recipient.send.to.all.forbidden'))
  }
  const body = getMessageBody()
  const payload: MessageBatchCreate = {
    ...body,
    sendToAll: mode === 'all',
    toUserIds: mode === 'list'
      ? resolveRecipientList().map((item) => item.id)
      : [],
  }
  if (!payload.sendToAll && (!payload.toUserIds || payload.toUserIds.length === 0)) {
    throw new Error(t('foundation.message.page.recipient.list.required'))
  }
  if (!permissionStore.hasPermission('foundation:message:send')) {
    return { createdList: [], pushed: false, pushFailed: true }
  }
  const createdList = await createAndSendMessages(payload)
  return {
    created: createdList[0],
    createdList,
    pushed: true,
  }
}

defineExpose({ validate, getValues, resetFields, submitCreateAndPushAsync })
</script>
