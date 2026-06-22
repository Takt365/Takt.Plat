<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/post/components -->
<!-- 文件名称：post-form.vue -->
<!-- 功能描述：岗位实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
    <a-tabs v-model:active-key="activeTab">
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
            <a-col :span="24">
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
                :label="t('entity.post.code')"
                name="postCode"
              >
                <a-input
                  v-model:value="formState.postCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.postId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.name')"
                name="postName"
              >
                <a-input
                  v-model:value="formState.postName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.name') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.post.deptid')"
                name="deptId"
              >
                <TaktTreeSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.deptid') })"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.category')"
                name="postCategory"
              >
                <TaktSelect
                  v-model:value="formState.postCategory"
                  dict-type="sys_post_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.category') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.level')"
                name="postLevel"
              >
                <TaktSelect
                  v-model:value="formState.postLevel"
                  dict-type="sys_post_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.level') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.headcount')"
                name="headcount"
              >
                <a-input-number
                  v-model:value="formState.headcount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.headcount') })"
                  :min="0"
                  class="w-full"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.currentcount')"
                name="currentCount"
              >
                <a-input-number
                  v-model:value="formState.currentCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.currentcount') })"
                  :min="0"
                  class="w-full"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.post.responsibilities')"
                name="responsibilities"
              >
                <a-textarea
                  v-model:value="formState.responsibilities"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.responsibilities') })"
                  :rows="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.post.requirements')"
                name="requirements"
              >
                <a-textarea
                  v-model:value="formState.requirements"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.requirements') })"
                  :rows="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.educationrequired')"
                name="educationRequired"
              >
                <TaktSelect
                  v-model:value="formState.educationRequired"
                  dict-type="hr_education_level_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.educationrequired') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.experienceyears')"
                name="experienceYears"
              >
                <a-input-number
                  v-model:value="formState.experienceYears"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.experienceyears') })"
                  :min="1"
                  class="w-full"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.salarymin')"
                name="salaryMin"
              >
                <a-input-number
                  v-model:value="formState.salaryMin"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.post.salarymin') })"
                  :min="0"
                  class="w-full"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.salarymax')"
                name="salaryMax"
              >
                <a-input-number
                  v-model:value="formState.salaryMax"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.post.salarymax') })"
                  :min="0"
                  class="w-full"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.status')"
                name="postStatus"
              >
                <TaktSelect
                  v-model:value="formState.postStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.status') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.isbuiltin')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.isbuiltin') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.post.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.post.description') })"
                  :rows="2"
                  allow-clear
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 岗位维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/organization/post/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PostCreate } from '@/types/human-resource/organization/post'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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

/** 表单内容区高度 class（字段多时 tab 内滚动） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = [
  'tenantCode',
  'companyCode',
  'companyDefaultCulture',
  'postCode',
  'postName',
  'deptId',
  'postCategory',
  'postLevel',
  'headcount',
  'currentCount',
  'responsibilities',
  'requirements',
  'educationRequired',
  'experienceYears',
  'salaryMin',
  'salaryMax',
  'postStatus',
  'isBuiltIn',
  'description',
  'extField',
  'remark',
]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PostCreate & { postId?: string }> | null
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

/** 表单字段默认值（与 TaktPost 实体 DefaultValue 对齐） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  postCategory: 'TEC',
  postLevel: 'P1',
  responsibilities: '',
  requirements: '',
  educationRequired: 1,
  experienceYears: 1,
  headcount: 1,
  currentCount: 0,
  postStatus: 1,
  isBuiltIn: 0,
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/**
 * 字典字符串选择校验（DictValue 为 string）
 * @param fieldKey i18n 字段键
 */
function createDictSelectRule(fieldKey: string): Rule {
  return {
    validator: async (_rule, value) => {
      if (value === undefined || value === null || String(value).trim() === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldKey) }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/**
 * 字典/数值选择校验（与 iso-code-form 一致）
 * @param fieldKey i18n 字段键
 */
function createSelectRule(fieldKey: string): Rule {
  return {
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldKey) }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldKey) }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 postId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.postId) {
      const next = { ...val } as Record<string, unknown>
      if (next.ExtField != null && next.extField == null) {
        next.extField = next.ExtField
        delete next.ExtField
      }
      if (next.deptId != null && next.deptId !== '') {
        next.deptId = String(next.deptId)
      }
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    const isCreate = !props.formData?.postId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  postCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.code') }),
      trigger: 'blur',
    },
  ],
  postName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.name') }),
      trigger: 'blur',
    },
  ],
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.deptid') }),
      trigger: 'change',
    },
  ],
  postCategory: [createDictSelectRule('entity.post.category')],
  postLevel: [createDictSelectRule('entity.post.level')],
  responsibilities: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.responsibilities') }),
      trigger: 'blur',
    },
  ],
  requirements: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.requirements') }),
      trigger: 'blur',
    },
  ],
  educationRequired: [createSelectRule('entity.post.educationrequired')],
  experienceYears: [createSelectRule('entity.post.experienceyears')],
  headcount: [createSelectRule('entity.post.headcount')],
  currentCount: [createSelectRule('entity.post.currentcount')],
  postStatus: [createSelectRule('entity.post.status')],
  isBuiltIn: [createSelectRule('entity.post.isbuiltin')],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  const intFields = [
    'educationRequired',
    'experienceYears',
    'headcount',
    'currentCount',
    'postStatus',
    'isBuiltIn',
  ] as const
  for (const key of intFields) {
    if (key in payload && payload[key] != null && payload[key] !== '') {
      payload[key] = typeof payload[key] === 'number' ? payload[key] : Number(payload[key])
    }
  }
  if ('salaryMin' in payload && payload.salaryMin != null && payload.salaryMin !== '') {
    payload.salaryMin = Number(payload.salaryMin)
  }
  if ('salaryMax' in payload && payload.salaryMax != null && payload.salaryMax !== '') {
    payload.salaryMax = Number(payload.salaryMax)
  }
  if ('deptId' in payload && payload.deptId != null && payload.deptId !== '') {
    payload.deptId = String(payload.deptId)
  }
  if ('extField' in payload) {
    payload.ExtField = payload.extField
    delete payload.extField
  }
  if ('sortOrder' in payload) delete payload.sortOrder
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.postId)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
