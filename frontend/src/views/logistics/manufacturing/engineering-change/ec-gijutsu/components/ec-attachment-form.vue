<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-attachment-form.vue -->
<!-- 功能描述：设变子表 ecAttachment 独立 CRUD 弹窗表单；集成 takt-upload-file 上传至 TaktFile 后回写附件字段；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-attachment-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-attachment-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecattachment.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.ecno') })"
                  show-count
                  :maxlength="10"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecattachment.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.linenumber') })"
                  style="width: 100%"
                  :min="0"
                  :disabled="loading || fileUploading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecattachment.attachmenttype')"
                name="attachmentType"
              >
                <TaktSelect
                  v-model:value="formState.attachmentType"
                  dict-type="logistics_ec_attachment_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecattachment.attachmenttype') })"
                  allow-clear
                  class="w-full"
                  :disabled="loading || fileUploading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecattachment.docno')"
                name="docNo"
              >
                <a-input
                  v-model:value="formState.docNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.docno') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="loading || fileUploading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecattachment.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.filename') })"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ecattachment.accessurl')"
                name="accessUrl"
              >
                <takt-upload-file
                  tabs-type="files"
                  :files-auto-upload="true"
                  :files-multiple="false"
                  :files-max-count="1"
                  :files-disabled="loading || fileUploading"
                  :files-max-size="taktFileMaxSizeMb"
                  :files-accept="taktFileAccept"
                  :files-hint="t('foundation.file.page.upload.hint', { max: taktFileMaxSizeMb })"
                  :files-custom-request="handleFilesCustomRequest"
                  v-model:files-file-list="filesFileList"
                  @files:remove="handleFileRemove"
                />
                <a-input
                  v-if="formState.accessUrl"
                  v-model:value="formState.accessUrl"
                  class="mt-2"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecattachment.accessurl') })"
                  show-count
                  :maxlength="500"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.extfield')"
                name="extField"
              >
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') })"
                  :rows="2"
                  show-count
                  :maxlength="400"
                  allow-clear
                  :disabled="loading || fileUploading"
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
                  :disabled="loading || fileUploading"
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
 * 设变子表 ecAttachment 维护表单 · takt-upload-file 上传至 TaktFile 后自动回填 fileName / accessUrl
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcAttachmentCreate } from '@/types/logistics/manufacturing/engineering-change/ec-attachment'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getFileById } from '@/api/foundation/file'
import { uploadTaktFileSmart } from '@/utils/takt-file-chunk-upload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/takt-file-upload-policy'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'companyCode', 'companyDefaultCulture', 'ecNo', 'lineNumber', 'attachmentType', 'docNo', 'fileName', 'accessUrl', 'extField', 'remark']

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcAttachmentCreate & { ecAttachmentId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 文件上传 loading */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept（后端策略） */
const taktFileAccept = ref('')
/** 上传体积上限 MB（后端策略） */
const taktFileMaxSizeMb = ref(500)

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (target.lineNumber === undefined || target.lineNumber === null || target.lineNumber === '') {
    target.lineNumber = 10
  }
  if (!String(target.attachmentType ?? '').trim()) {
    target.attachmentType = 'EC'
  }
}

/** 根据 accessUrl 同步 takt-upload-file 列表展示 */
function syncUploadFileListFromState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url || url === '-') {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
  }]
}

/**
 * 将 TaktFiles 上传结果回填至表单
 * @param file 本地文件
 * @param result 上传结果
 */
async function applyUploadResultToForm(file: globalThis.File, result: Awaited<ReturnType<typeof uploadTaktFileSmart>>) {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  formState.accessUrl = accessUrl
  formState.fileName = result.fileOriginalName?.trim()
    || result.fileName?.trim()
    || file.name
  syncUploadFileListFromState()
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/** takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('upload disabled'))
    return
  }
  const originFile = options.file as globalThis.File
  fileUploading.value = true
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(originFile)
      await applyUploadResultToForm(originFile, result)
      options.onSuccess?.(result)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
      message.error(t('common.feedback.failed'))
      options.onError?.(err)
    } finally {
      fileUploading.value = false
    }
  })()
}

/** 移除已上传文件 */
function handleFileRemove() {
  formState.accessUrl = ''
  formState.fileName = ''
  filesFileList.value = []
}

/** 挂载后加载后端上传策略（accept / maxSize） */
onMounted(async () => {
  try {
    const policy = await loadTaktFileUploadBasePolicy()
    taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
    taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
  } catch {
    // 回退默认值；实际上传校验仍由后端 API 返回
  }
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecAttachmentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecAttachmentId) {
      const next = { ...val } as Record<string, unknown>
      if (next.ExtField != null && next.extField == null) {
        next.extField = next.ExtField
        delete next.ExtField
      }
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      Object.assign(formState, next)
      syncUploadFileListFromState()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        if (next.ExtField != null && next.extField == null) {
          next.extField = next.ExtField
          delete next.ExtField
        }
        Object.assign(formState, next)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      syncUploadFileListFromState()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.ecAttachmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecattachment.ecno') }),
      trigger: 'blur',
    },
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecattachment.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecattachment.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  attachmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecattachment.attachmenttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  docNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecattachment.docno') }),
      trigger: 'blur',
    },
  ],
  fileName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecattachment.filename') }),
      trigger: 'change',
    },
  ],
  accessUrl: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecattachment.accessurl') }),
      trigger: 'change',
    },
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 ecId） */
function getValues(): Record<string, any> {
  const payload: Record<string, unknown> = {
    tenantCode: formState.tenantCode,
    companyCode: formState.companyCode,
    companyDefaultCulture: formState.companyDefaultCulture,
    ecId: props.masterId,
    ecNo: String(formState.ecNo ?? '').trim(),
    lineNumber: typeof formState.lineNumber === 'number' ? formState.lineNumber : Number(formState.lineNumber),
    attachmentType: String(formState.attachmentType ?? '').trim(),
    docNo: String(formState.docNo ?? '').trim(),
    fileName: String(formState.fileName ?? '').trim(),
    accessUrl: String(formState.accessUrl ?? '').trim(),
    extField: formState.extField,
    remark: formState.remark,
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    const next = { ...props.formData } as Record<string, unknown>
    if (next.ExtField != null && next.extField == null) {
      next.extField = next.ExtField
      delete next.ExtField
    }
    Object.assign(formState, next)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecAttachmentId)
  syncUploadFileListFromState()
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
