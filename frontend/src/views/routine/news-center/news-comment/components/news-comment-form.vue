<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news-comment/components -->
<!-- 文件名称：news-comment-form.vue -->
<!-- 功能描述：新闻中心评论实体 支持多级回复维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="news-comment-form-tabs"
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
                :label="t('entity.newsComment.newsid')"
                name="newsId"
              >
                <a-input
                  v-model:value="formState.newsId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.newsid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.parentid')"
                name="parentId"
              >
                <a-input
                  v-model:value="formState.parentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.parentid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.userid')"
                name="userId"
              >
                <a-input
                  v-model:value="formState.userId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.userid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.username') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.useravatar')"
                name="userAvatar"
              >
                <a-input
                  v-model:value="formState.userAvatar"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.useravatar') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.replytouserid')"
                name="replyToUserId"
              >
                <a-input
                  v-model:value="formState.replyToUserId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.replytouserid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.replytousername')"
                name="replyToUserName"
              >
                <a-input
                  v-model:value="formState.replyToUserName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.replytousername') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newsComment.commentcontent')"
                name="commentContent"
              >
                <a-textarea
                  v-model:value="formState.commentContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.newsComment.commentcontent') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.commenttime')"
                name="commentTime"
              >
                <a-input
                  v-model:value="formState.commentTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.commenttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.likecount')"
                name="likeCount"
              >
                <a-input-number
                  v-model:value="formState.likeCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.likecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.replycount')"
                name="replyCount"
              >
                <a-input-number
                  v-model:value="formState.replyCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.replycount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.commentlevel')"
                name="commentLevel"
              >
                <a-input-number
                  v-model:value="formState.commentLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.commentlevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.commentstatus')"
                name="commentStatus"
              >
                <a-input-number
                  v-model:value="formState.commentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.commentstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newsComment.likes')"
                name="likes"
              >
                <a-input
                  v-model:value="formState.likes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.likes') })"
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
 * 新闻中心评论实体 支持多级回复维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/news-center/news-comment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NewsCommentCreate } from '@/types/routine/news-center/news-comment'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","newsId","parentId","userId","userName","userAvatar","replyToUserId","replyToUserName","commentContent","commentTime","likeCount","replyCount","commentLevel","flowInstanceId","commentStatus","likes","extFieldJson","remark"]


interface Props {
  formData?: Partial<NewsCommentCreate & { newsCommentId?: string }> | null
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
    const isCreate = !props.formData?.newsCommentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  newsId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.newsid') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.parentid') }),
      trigger: 'blur'
    }
  ],
  userId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.userid') }),
      trigger: 'blur'
    }
  ],
  userName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.username') }),
      trigger: 'blur'
    }
  ],
  commentContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.commentcontent') }),
      trigger: 'blur'
    }
  ],
  commentTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newsComment.commenttime') }),
      trigger: 'blur'
    }
  ],
  likeCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.newsComment.likecount') }),
      trigger: 'change'
    }
  ],
  replyCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.newsComment.replycount') }),
      trigger: 'change'
    }
  ],
  commentLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.newsComment.commentlevel') }),
      trigger: 'change'
    }
  ],
  commentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.newsComment.commentstatus') }),
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
