<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-attachment/components -->
<!-- 文件名称：employee-attachment-form.vue -->
<!-- 功能描述：员工档案附件维护弹窗内嵌表单；文件上传统一走 TaktFile，本表单仅保存附件名称与 AccessUrl；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div class="takt-form-content-rows-5">
      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item
            :label="t('common.page.entity.tenantcode')"
            name="tenantCode"
          >
            <a-input
              v-model:value="formState.tenantCode"
              disabled
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
              disabled
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.employeeattachment.employeeid')"
            name="employeeId"
          >
            <TaktSelect
              v-model:value="formState.employeeId"
              :options="employeeOptions"
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeattachment.employeeid') })"
              show-search
              :filter-option="filterOption"
              :disabled="loading"
              allow-clear
            />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item
            :label="t('entity.employeeattachment.attachmentname')"
            name="attachmentName"
          >
            <a-input
              v-model:value="formState.attachmentName"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeattachment.attachmentname') })"
              show-count
              :maxlength="100"
              allow-clear
              :disabled="loading"
            />
          </a-form-item>
        </a-col>
        <a-col :span="24">
          <a-form-item
            :label="t('entity.employeeattachment.accessurl')"
            name="accessUrl"
          >
            <a-upload
              v-model:file-list="uploadFileList"
              :max-count="1"
              :disabled="loading || fileUploading"
              :before-upload="handleBeforeFileUpload"
              @remove="handleFileRemove"
            >
              <a-button :loading="fileUploading">
                {{ t('common.page.button.upload') }}
              </a-button>
            </a-upload>
            <a-input
              v-if="formState.accessUrl"
              v-model:value="formState.accessUrl"
              class="mt-2"
              :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeattachment.accessurl') })"
              show-count
              :maxlength="1000"
              allow-clear
              :disabled="loading"
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
              :rows="3"
              show-count
              :maxlength="400"
              allow-clear
              :disabled="loading"
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
              :rows="3"
              show-count
              :maxlength="400"
              allow-clear
              :disabled="loading"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 员工档案附件维护表单
 * @module views/human-resource/personnel/employee-attachment/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { UploadFile } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { EmployeeAttachmentCreate } from '@/types/human-resource/personnel/employee-attachment'
import type { TaktSelectOption } from '@/types/common'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee'
import { getFileById } from '@/api/foundation/file'
import { uploadTaktFileSmart } from '@/utils/takt-file-chunk-upload'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'companyCode','cultureCode', 'employeeId', 'attachmentName', 'accessUrl', 'extField', 'remark']

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EmployeeAttachmentCreate & { employeeAttachmentId?: string }> | null
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
/** 员工下拉选项 */
const employeeOptions = ref<TaktSelectOption[]>([])
/** 文件上传 loading */
const fileUploading = ref(false)
/** 上传组件文件列表 */
const uploadFileList = ref<UploadFile[]>([])

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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}

/** TaktSelect 搜索：按 label / extLabel 模糊匹配 */
function filterOption(input: string, option: Record<string, unknown>) {
  const label = String(option?.label ?? option?.dictLabel ?? '')
  const ext = String(option?.extLabel ?? '')
  const kw = input.trim().toLowerCase()
  return label.toLowerCase().includes(kw) || ext.toLowerCase().includes(kw)
}

/** 根据 accessUrl 同步上传列表展示 */
function syncUploadFileListFromState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url) {
    uploadFileList.value = []
    return
  }
  uploadFileList.value = [{
    uid: '-1',
    name: url.split('/').pop() || 'file',
    status: 'done',
    url: undefined,
  }]
}

/**
 * 上传至 TaktFiles 并回填 AccessUrl
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
      message.error(t('common.feedback.failed'))
      return false
    }
    formState.accessUrl = accessUrl
    syncUploadFileListFromState()
    formRef.value?.validateFields(['accessUrl']).catch(() => undefined)
    message.success(t('common.feedback.success'))
  } catch {
    message.error(t('common.feedback.failed'))
    return false
  } finally {
    fileUploading.value = false
  }
  return false
}

/** 移除已上传文件 */
function handleFileRemove() {
  formState.accessUrl = ''
  uploadFileList.value = []
}

/** 加载员工下拉 */
async function loadEmployeeOptions() {
  try {
    employeeOptions.value = await getEmployeeOptions()
  } catch {
    employeeOptions.value = []
  }
}

/** 编辑态灌入 formData；新增态恢复默认值 */
watch(
  () => props.formData,
  (val) => {
    if (val?.employeeAttachmentId) {
      const next = { ...val } as Record<string, unknown>
      if (next.employeeId != null) {
        next.employeeId = String(next.employeeId)
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
        if (next.employeeId != null) {
          next.employeeId = String(next.employeeId)
        }
        Object.assign(formState, next)
      }
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
    const isCreate = !props.formData?.employeeAttachmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [{
    required: true,
    message: t('common.page.form.placeholder.select', { field: t('entity.employeeattachment.employeeid') }),
    trigger: 'change',
  }],
  attachmentName: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.employeeattachment.attachmentname') }),
    trigger: 'blur',
  }],
  accessUrl: [{
    required: true,
    message: t('common.page.form.placeholder.required', { field: t('entity.employeeattachment.accessurl') }),
    trigger: 'change',
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, unknown> {
  return {
    tenantCode: formState.tenantCode,
    companyCode: formState.companyCode,
    companyDefaultCulture: formState.companyDefaultCulture,
    employeeId: formState.employeeId ? String(formState.employeeId) : '',
    attachmentName: String(formState.attachmentName ?? '').trim(),
    accessUrl: String(formState.accessUrl ?? '').trim(),
    extField: formState.extField,
    remark: formState.remark,
  }
}

/** 重置表单 */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.employeeAttachmentId)
  syncUploadFileListFromState()
  formRef.value?.clearValidate()
}

onMounted(() => {
  loadEmployeeOptions()
})

defineExpose({ validate, getValues, resetFields })
</script>
