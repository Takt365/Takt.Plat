<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：document-form.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="document-form-tabs"
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
                :label="t('entity.document.code')"
                name="documentCode"
              >
                <a-input
                  v-model:value="formState.documentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.category')"
                name="documentCategory"
              >
                <a-input-number
                  v-model:value="formState.documentCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.category') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.status')"
                name="documentStatus"
              >
                <a-input-number
                  v-model:value="formState.documentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.confidentiallevel')"
                name="confidentialLevel"
              >
                <a-input-number
                  v-model:value="formState.confidentialLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.confidentiallevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.version')"
                name="version"
              >
                <a-input-number
                  v-model:value="formState.version"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.version') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.document.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.content') })"
                  :rows="2"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.summary')"
                name="summary"
              >
                <a-input
                  v-model:value="formState.summary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.summary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.tags') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.fileid')"
                name="fileId"
              >
                <a-input
                  v-model:value="formState.fileId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filepath')"
                name="filePath"
              >
                <a-input
                  v-model:value="formState.filePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filepath') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filesize')"
                name="fileSize"
              >
                <a-input
                  v-model:value="formState.fileSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filesize') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filetype')"
                name="fileType"
              >
                <a-input
                  v-model:value="formState.fileType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filetype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.fileextension')"
                name="fileExtension"
              >
                <a-input
                  v-model:value="formState.fileExtension"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileextension') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.effectivetime')"
                name="effectiveTime"
              >
                <a-input
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.effectivetime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.expiretime')"
                name="expireTime"
              >
                <a-input
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.expiretime') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publisherid')"
                name="publisherId"
              >
                <a-input
                  v-model:value="formState.publisherId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publisherid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publishername')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publishername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.istop')"
                name="isTop"
              >
                <a-input-number
                  v-model:value="formState.isTop"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.istop') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.viewcount')"
                name="viewCount"
              >
                <a-input-number
                  v-model:value="formState.viewCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.viewcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.downloadcount')"
                name="downloadCount"
              >
                <a-input-number
                  v-model:value="formState.downloadCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.downloadcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.document.targetscope')"
                name="targetScope"
              >
                <a-textarea
                  v-model:value="formState.targetScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.targetscope') })"
                  :rows="2"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.targetdepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetdepartments') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.targetusers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetusers') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.versions')"
                name="versions"
              >
                <a-input
                  v-model:value="formState.versions"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.versions') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.changelogs')"
                name="changeLogs"
              >
                <a-input
                  v-model:value="formState.changeLogs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.changelogs') })"
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
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/document-center/document/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DocumentCreate } from '@/types/routine/document-center/document'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","documentCode","title","documentCategory","documentStatus","confidentialLevel","version","content","summary","tags","fileId","fileName","filePath","fileSize","fileType","fileExtension","effectiveTime","expireTime","publishTime","publisherId","publisherName","deptId","deptName","isTop","sortOrder","viewCount","downloadCount","targetScope","targetDepartments","targetUsers","versions","changeLogs","extFieldJson","remark"]


interface Props {
  formData?: Partial<DocumentCreate & { documentId?: string }> | null
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
    const isCreate = !props.formData?.documentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  documentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.code') }),
      trigger: 'blur'
    }
  ],
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.title') }),
      trigger: 'blur'
    }
  ],
  documentCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.category') }),
      trigger: 'change'
    }
  ],
  documentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.status') }),
      trigger: 'change'
    }
  ],
  confidentialLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.confidentiallevel') }),
      trigger: 'change'
    }
  ],
  version: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.version') }),
      trigger: 'change'
    }
  ],
  fileSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.filesize') }),
      trigger: 'blur'
    }
  ],
  publisherId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.publisherid') }),
      trigger: 'blur'
    }
  ],
  publisherName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.publishername') }),
      trigger: 'blur'
    }
  ],
  isTop: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.istop') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.sortorder') }),
      trigger: 'change'
    }
  ],
  viewCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.viewcount') }),
      trigger: 'change'
    }
  ],
  downloadCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.downloadcount') }),
      trigger: 'change'
    }
  ],
  targetScope: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.targetscope') }),
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
