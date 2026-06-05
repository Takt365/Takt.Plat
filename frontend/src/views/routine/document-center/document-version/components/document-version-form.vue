<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document-version/components -->
<!-- 文件名称：document-version-form.vue -->
<!-- 功能描述：文管文档版本子实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="document-version-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.documentVersion.documentid')"
                name="documentId"
              >
                <a-input
                  v-model:value="formState.documentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.documentid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.versionno')"
                name="versionNo"
              >
                <a-input-number
                  v-model:value="formState.versionNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.versionno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.documentVersion.versionnote')"
                name="versionNote"
              >
                <a-textarea
                  v-model:value="formState.versionNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.documentVersion.versionnote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.fileid')"
                name="fileId"
              >
                <a-input
                  v-model:value="formState.fileId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.fileid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.filepath')"
                name="filePath"
              >
                <a-input
                  v-model:value="formState.filePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filepath') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.filesize')"
                name="fileSize"
              >
                <a-input
                  v-model:value="formState.fileSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filesize') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.filetype')"
                name="fileType"
              >
                <a-input
                  v-model:value="formState.fileType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filetype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.fileextension')"
                name="fileExtension"
              >
                <a-input
                  v-model:value="formState.fileExtension"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.fileextension') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.revisedby')"
                name="revisedBy"
              >
                <a-input
                  v-model:value="formState.revisedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.revisedby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.revisedbyname')"
                name="revisedByName"
              >
                <a-input
                  v-model:value="formState.revisedByName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.revisedbyname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentVersion.revisedat')"
                name="revisedAt"
              >
                <a-input
                  v-model:value="formState.revisedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.revisedat') })"
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
 * 文管文档版本子实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/document-center/document-version/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DocumentVersionCreate } from '@/types/routine/document-center/document-version'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","documentId","versionNo","versionNote","fileId","fileName","filePath","fileSize","fileType","fileExtension","revisedBy","revisedByName","revisedAt","extFieldJson","remark"]


interface Props {
  formData?: Partial<DocumentVersionCreate & { documentVersionId?: string }> | null
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
    const isCreate = !props.formData?.documentVersionId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  documentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.documentid') }),
      trigger: 'blur'
    }
  ],
  versionNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.documentVersion.versionno') }),
      trigger: 'change'
    }
  ],
  fileId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.fileid') }),
      trigger: 'blur'
    }
  ],
  fileName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filename') }),
      trigger: 'blur'
    }
  ],
  filePath: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filepath') }),
      trigger: 'blur'
    }
  ],
  fileSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filesize') }),
      trigger: 'blur'
    }
  ],
  revisedBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.revisedby') }),
      trigger: 'blur'
    }
  ],
  revisedAt: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentVersion.revisedat') }),
      trigger: 'blur'
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
