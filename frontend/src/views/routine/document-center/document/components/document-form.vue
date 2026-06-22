<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：document-form.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form document-form flex flex-col min-h-0"
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
                  show-count
                  :maxlength="20"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.documentId"
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
                  show-count
                  :maxlength="200"
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
                  show-count
                  :maxlength="2000"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="200"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="100"
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
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.effectivetime')"
                name="effectiveTime"
              >
                <a-date-picker
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.effectivetime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.expiretime')"
                name="expireTime"
              >
                <a-date-picker
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.expiretime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                <a-date-picker
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.publishtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="20"
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
                  show-count
                  :maxlength="100"
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
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.targetdepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetdepartments') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
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
                :label="t('entity.document.targetusers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetusers') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.document.extfield')"
                name="ExtField"
              >
                <a-textarea
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.extfield') })"
                  :rows="2"
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
    <!-- 下：子表 versions -->
    <TaktEditableTable
      ref="documentVersionTableRef"
      v-model="childDocumentVersionRows"
      :columns="documentVersionFormColumns"
      :title="t('entity.documentversion._self')"
      :add-button-entity="t('entity.documentversion._self')"
      id-field="documentVersionId"
      :default-row="createDefaultDocumentVersionRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/document-center/document/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DocumentCreate } from '@/types/routine/document-center/document'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","documentCode","title","documentCategory","documentStatus","confidentialLevel","version","content","summary","tags","fileId","fileName","filePath","fileSize","fileType","fileExtension","effectiveTime","expireTime","publishTime","publisherId","publisherName","deptId","deptName","isTop","viewCount","downloadCount","targetScope","targetDepartments","targetUsers","ExtField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childDocumentVersionRows = ref<Record<string, unknown>[]>([])
const documentVersionTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 documentVersion 可编辑列 */
const documentVersionFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'versionNo',
    title: t('entity.documentversion.versionno'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'versionNote',
    title: t('entity.documentversion.versionnote'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.documentversion.versionnote') }),
    width: 140,
  },
  {
    key: 'fileId',
    title: t('entity.documentversion.fileid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileName',
    title: t('entity.documentversion.filename'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'filePath',
    title: t('entity.documentversion.filepath'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileSize',
    title: t('entity.documentversion.filesize'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileType',
    title: t('entity.documentversion.filetype'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.documentversion.filetype') }),
  },
  {
    key: 'fileExtension',
    title: t('entity.documentversion.fileextension'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.documentversion.fileextension') }),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<DocumentCreate & { documentId?: string }> | null | undefined) {
  childDocumentVersionRows.value = ((val as any)?.versions ?? []) as Record<string, unknown>[]
}

function createDefaultDocumentVersionRow(): Record<string, unknown> {
  return {
    versionNo: 0,
    versionNote: '',
    fileId: '',
    fileName: '',
    filePath: '',
    fileSize: '',
    fileType: '',
    fileExtension: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.documentId ?? ''
  return {
    ...formState,
    versions: documentVersionTableRef.value?.getRows?.() ?? childDocumentVersionRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      documentId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DocumentCreate & { documentId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 documentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.documentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).versions
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
    const isCreate = !props.formData?.documentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
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
  documentCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.category') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.category') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  documentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  confidentialLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.confidentiallevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.confidentiallevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  version: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.version') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.version') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  isTop: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.istop') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.istop') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  viewCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.viewcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.viewcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downloadCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.downloadcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.document.downloadcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  targetScope: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.targetscope') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await documentVersionTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('documentCategory' in payload) {
    const rawdocumentCategory = payload.documentCategory
    payload.documentCategory = typeof rawdocumentCategory === 'number' ? rawdocumentCategory : Number(rawdocumentCategory)
  }
  if ('documentStatus' in payload) {
    const rawdocumentStatus = payload.documentStatus
    payload.documentStatus = typeof rawdocumentStatus === 'number' ? rawdocumentStatus : Number(rawdocumentStatus)
  }
  if ('confidentialLevel' in payload) {
    const rawconfidentialLevel = payload.confidentialLevel
    payload.confidentialLevel = typeof rawconfidentialLevel === 'number' ? rawconfidentialLevel : Number(rawconfidentialLevel)
  }
  if ('version' in payload) {
    const rawversion = payload.version
    payload.version = typeof rawversion === 'number' ? rawversion : Number(rawversion)
  }
  if ('isTop' in payload) {
    const rawisTop = payload.isTop
    payload.isTop = typeof rawisTop === 'number' ? rawisTop : Number(rawisTop)
  }
  if ('viewCount' in payload) {
    const rawviewCount = payload.viewCount
    payload.viewCount = typeof rawviewCount === 'number' ? rawviewCount : Number(rawviewCount)
  }
  if ('downloadCount' in payload) {
    const rawdownloadCount = payload.downloadCount
    payload.downloadCount = typeof rawdownloadCount === 'number' ? rawdownloadCount : Number(rawdownloadCount)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.documentId)
  childDocumentVersionRows.value = []
  documentVersionTableRef.value?.resetRows?.()
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
