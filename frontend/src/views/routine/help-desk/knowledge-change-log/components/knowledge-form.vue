<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/knowledge-change-log/components -->
<!-- 文件名称：knowledge-form.vue -->
<!-- 功能描述：服务台知识库实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form knowledge-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="knowledge-form-tabs"
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
                :label="t('entity.knowledge.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.title') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.knowledge.content') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.summary')"
                name="summary"
              >
                <a-input
                  v-model:value="formState.summary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.summary') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.categorycode')"
                name="categoryCode"
              >
                <a-input
                  v-model:value="formState.categoryCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.categorycode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.knowledgeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.tags') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.status')"
                name="knowledgeStatus"
              >
                <a-input-number
                  v-model:value="formState.knowledgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.status') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.viewcount')"
                name="viewCount"
              >
                <a-input-number
                  v-model:value="formState.viewCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.viewcount') })"
                  style="width: 100%"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.helpfulcount')"
                name="helpfulCount"
              >
                <a-input-number
                  v-model:value="formState.helpfulCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.helpfulcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.unhelpfulcount')"
                name="unhelpfulCount"
              >
                <a-input-number
                  v-model:value="formState.unhelpfulCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.unhelpfulcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.ispublished')"
                name="isPublished"
              >
                <a-input-number
                  v-model:value="formState.isPublished"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.ispublished') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.version')"
                name="version"
              >
                <a-input-number
                  v-model:value="formState.version"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.version') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.publishedat')"
                name="publishedAt"
              >
                <a-input
                  v-model:value="formState.publishedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.publishedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.revisedat')"
                name="revisedAt"
              >
                <a-input
                  v-model:value="formState.revisedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.revisedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.knowledge.extfield')"
                name="ExtField"
              >
                <a-textarea
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.knowledge.extfield') })"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="knowledgeChangeLogTableRef"
      v-model="childKnowledgeChangeLogRows"
      :columns="knowledgeChangeLogFormColumns"
      :title="t('entity.knowledgechangelog._self')"
      :add-button-entity="t('entity.knowledgechangelog._self')"
      id-field="knowledgeChangeLogId"
      :default-row="createDefaultKnowledgeChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 服务台知识库实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/knowledge-change-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { KnowledgeCreate } from '@/types/routine/help-desk/knowledge'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","title","content","summary","categoryCode","tags","knowledgeStatus","viewCount","helpfulCount","unhelpfulCount","isPublished","version","publishedAt","revisedAt","ExtField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childKnowledgeChangeLogRows = ref<Record<string, unknown>[]>([])
const knowledgeChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 knowledgeChangeLog 可编辑列 */
const knowledgeChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'knowledgeTitle',
    title: t('entity.knowledgechangelog.knowledgetitle'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.knowledgechangelog.knowledgetitle') }),
  },
  {
    key: 'changeType',
    title: t('entity.knowledgechangelog.changetype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'changeSummary',
    title: t('entity.knowledgechangelog.changesummary'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.knowledgechangelog.changesummary') }),
  },
  {
    key: 'changeFields',
    title: t('entity.knowledgechangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.knowledgechangelog.changefields') }),
  },
  {
    key: 'changeReason',
    title: t('entity.knowledgechangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.knowledgechangelog.changereason') }),
  },
  {
    key: 'versionAtChange',
    title: t('entity.knowledgechangelog.versionatchange'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'ExtField',
    title: t('entity.knowledgechangelog.extfield'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.knowledgechangelog.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<KnowledgeCreate & { knowledgeId?: string }> | null | undefined) {
  childKnowledgeChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultKnowledgeChangeLogRow(): Record<string, unknown> {
  return {
    knowledgeTitle: '',
    changeType: 0,
    changeSummary: '',
    changeFields: '',
    changeReason: '',
    versionAtChange: 0,
    ExtField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.knowledgeId ?? ''
  return {
    ...formState,
    changeLogs: knowledgeChangeLogTableRef.value?.getRows?.() ?? childKnowledgeChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      knowledgeId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<KnowledgeCreate & { knowledgeId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 knowledgeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.knowledgeId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.knowledgeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.knowledge.title') }),
      trigger: 'blur'
    }
  ],
  knowledgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  viewCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.viewcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.viewcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  helpfulCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.helpfulcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.helpfulcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unhelpfulCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.unhelpfulcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.unhelpfulcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPublished: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.ispublished') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.ispublished') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  version: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.version') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.knowledge.version') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await knowledgeChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('knowledgeStatus' in payload) {
    const rawknowledgeStatus = payload.knowledgeStatus
    payload.knowledgeStatus = typeof rawknowledgeStatus === 'number' ? rawknowledgeStatus : Number(rawknowledgeStatus)
  }
  if ('viewCount' in payload) {
    const rawviewCount = payload.viewCount
    payload.viewCount = typeof rawviewCount === 'number' ? rawviewCount : Number(rawviewCount)
  }
  if ('helpfulCount' in payload) {
    const rawhelpfulCount = payload.helpfulCount
    payload.helpfulCount = typeof rawhelpfulCount === 'number' ? rawhelpfulCount : Number(rawhelpfulCount)
  }
  if ('unhelpfulCount' in payload) {
    const rawunhelpfulCount = payload.unhelpfulCount
    payload.unhelpfulCount = typeof rawunhelpfulCount === 'number' ? rawunhelpfulCount : Number(rawunhelpfulCount)
  }
  if ('isPublished' in payload) {
    const rawisPublished = payload.isPublished
    payload.isPublished = typeof rawisPublished === 'number' ? rawisPublished : Number(rawisPublished)
  }
  if ('version' in payload) {
    const rawversion = payload.version
    payload.version = typeof rawversion === 'number' ? rawversion : Number(rawversion)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.knowledgeId)
  childKnowledgeChangeLogRows.value = []
  knowledgeChangeLogTableRef.value?.resetRows?.()
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
