<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/post/components -->
<!-- 文件名称：post-form.vue -->
<!-- 功能描述：岗位实体 代表组织架构中的岗位/职位 参照 SAP Position维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="post-form-tabs"
    >
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
                :label="t('entity.post.code')"
                name="postCode"
              >
                <a-input
                  v-model:value="formState.postCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.code') })"
                  size="small"
                  allow-clear
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
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.type')"
                name="postType"
              >
                <a-input-number
                  v-model:value="formState.postType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.level')"
                name="postLevel"
              >
                <a-input-number
                  v-model:value="formState.postLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.level') })"
                  size="small"
                  style="width: 100%"
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
                  size="small"
                  style="width: 100%"
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
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.post.responsibilities')"
                name="responsibilities"
              >
                <a-input
                  v-model:value="formState.responsibilities"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.responsibilities') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.requirements')"
                name="requirements"
              >
                <a-input
                  v-model:value="formState.requirements"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.requirements') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.educationrequired')"
                name="educationRequired"
              >
                <a-input-number
                  v-model:value="formState.educationRequired"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.educationrequired') })"
                  size="small"
                  style="width: 100%"
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
                  size="small"
                  style="width: 100%"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.salarymin') })"
                  size="small"
                  style="width: 100%"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.salarymax') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.status')"
                name="postStatus"
              >
                <a-input-number
                  v-model:value="formState.postStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.isbuiltin')"
                name="isBuiltIn"
              >
                <a-input-number
                  v-model:value="formState.isBuiltIn"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.isbuiltin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.post.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.sortorder') })"
                  size="small"
                  style="width: 100%"
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
                  size="small"
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
                :label="t('entity.post.employeeids')"
                name="employeeIds"
              >
                <a-input
                  v-model:value="formState.employeeIds"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.employeeids') })"
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
 * 岗位实体 代表组织架构中的岗位/职位 参照 SAP Position维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/organization/post/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PostCreate } from '@/types/human-resource/organization/post'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","postCode","postName","deptId","postType","postLevel","headcount","currentCount","responsibilities","requirements","educationRequired","experienceYears","salaryMin","salaryMax","postStatus","isBuiltIn","sortOrder","description","employeeIds","extFieldJson","remark"]


interface Props {
  formData?: Partial<PostCreate & { postId?: string }> | null
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
    const isCreate = !props.formData?.postId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  postCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.code') }),
      trigger: 'blur'
    }
  ],
  postName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.name') }),
      trigger: 'blur'
    }
  ],
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.post.deptid') }),
      trigger: 'blur'
    }
  ],
  postType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.type') }),
      trigger: 'change'
    }
  ],
  headcount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.headcount') }),
      trigger: 'change'
    }
  ],
  currentCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.currentcount') }),
      trigger: 'change'
    }
  ],
  postStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.status') }),
      trigger: 'change'
    }
  ],
  isBuiltIn: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.isbuiltin') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.post.sortorder') }),
      trigger: 'change'
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
