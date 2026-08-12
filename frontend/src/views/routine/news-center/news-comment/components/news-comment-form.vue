<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news-comment/components -->
<!-- 文件名称：news-comment-form.vue -->
<!-- 功能描述：新闻中心评论实体 支持多级回复维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form news-comment-form flex flex-col min-h-0"
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
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.newsid')"
                name="newsId"
              >
                <a-input
                  v-model:value="formState.newsId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.newsid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.parentid')"
                name="parentId"
              >
                <a-input
                  v-model:value="formState.parentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.parentid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.userid')"
                name="userId"
              >
                <a-input
                  v-model:value="formState.userId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.userid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.username') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.useravatar')"
                name="userAvatar"
              >
                <a-input
                  v-model:value="formState.userAvatar"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.useravatar') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.replytouserid')"
                name="replyToUserId"
              >
                <a-input
                  v-model:value="formState.replyToUserId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.replytouserid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.newscomment.replytousername')"
                name="replyToUserName"
              >
                <a-input
                  v-model:value="formState.replyToUserName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.replytousername') })"
                  show-count
                  :maxlength="20"
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
                :label="t('entity.newscomment.commentcontent')"
                name="commentContent"
              >
                <a-textarea
                  v-model:value="formState.commentContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.newscomment.commentcontent') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newscomment.commenttime')"
                name="commentTime"
              >
                <a-date-picker
                  v-model:value="formState.commentTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.newscomment.commenttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newscomment.likecount')"
                name="likeCount"
              >
                <a-input-number
                  v-model:value="formState.likeCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.likecount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newscomment.replycount')"
                name="replyCount"
              >
                <a-input-number
                  v-model:value="formState.replyCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.replycount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newscomment.commentlevel')"
                name="commentLevel"
              >
                <a-input-number
                  v-model:value="formState.commentLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.commentlevel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.newscomment.commentstatus')"
                name="commentStatus"
              >
                <a-input-number
                  v-model:value="formState.commentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newscomment.commentstatus') })"
                  style="width: 100%"
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
    <!-- 下：子表 likes -->
    <TaktEditableTable
      ref="newsCommentLikeTableRef"
      v-model="childNewsCommentLikeRows"
      :columns="newsCommentLikeFormColumns"
      :title="t('entity.newscommentlike._self')"
      :add-button-entity="t('entity.newscommentlike._self')"
      id-field="newsCommentLikeId"
      :default-row="createDefaultNewsCommentLikeRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 新闻中心评论实体 支持多级回复维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/news-center/news-comment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NewsCommentCreate } from '@/types/routine/news-center/news-comment'
import { RiQuestionLine } from '@remixicon/vue'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","newsId","parentId","userId","userName","userAvatar","replyToUserId","replyToUserName","commentContent","commentTime","likeCount","replyCount","commentLevel","commentStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childNewsCommentLikeRows = ref<Record<string, unknown>[]>([])
const newsCommentLikeTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 newsCommentLike 可编辑列 */
const newsCommentLikeFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'commentId',
    title: t('entity.newscommentlike.commentid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'userId',
    title: t('entity.newscommentlike.userid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'userName',
    title: t('entity.newscommentlike.username'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'likeTime',
    title: t('entity.newscommentlike.liketime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<NewsCommentCreate & { newsCommentId?: string }> | null | undefined) {
  childNewsCommentLikeRows.value = ((val as any)?.likes ?? []) as Record<string, unknown>[]
}

function createDefaultNewsCommentLikeRow(): Record<string, unknown> {
  return {
    commentId: '',
    userId: '',
    userName: '',
    likeTime: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.newsCommentId ?? ''
  return {
    ...formState,
    likes: newsCommentLikeTableRef.value?.getRows?.() ?? childNewsCommentLikeRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      newsCommentId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<NewsCommentCreate & { newsCommentId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 newsCommentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.newsCommentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).likes
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
    const isCreate = !props.formData?.newsCommentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  newsId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newscomment.newsid') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newscomment.parentid') }),
      trigger: 'blur'
    }
  ],
  userId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newscomment.userid') }),
      trigger: 'blur'
    }
  ],
  userName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newscomment.username') }),
      trigger: 'blur'
    }
  ],
  commentContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.newscomment.commentcontent') }),
      trigger: 'blur'
    }
  ],
  commentTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.newscomment.commenttime') }),
      trigger: 'change'
    }
  ],
  likeCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.likecount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.likecount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  replyCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.replycount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.replycount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  commentLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.commentlevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.commentlevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  commentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.commentstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.newscomment.commentstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await newsCommentLikeTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('likeCount' in payload) {
    const rawlikeCount = payload.likeCount
    payload.likeCount = typeof rawlikeCount === 'number' ? rawlikeCount : Number(rawlikeCount)
  }
  if ('replyCount' in payload) {
    const rawreplyCount = payload.replyCount
    payload.replyCount = typeof rawreplyCount === 'number' ? rawreplyCount : Number(rawreplyCount)
  }
  if ('commentLevel' in payload) {
    const rawcommentLevel = payload.commentLevel
    payload.commentLevel = typeof rawcommentLevel === 'number' ? rawcommentLevel : Number(rawcommentLevel)
  }
  if ('commentStatus' in payload) {
    const rawcommentStatus = payload.commentStatus
    payload.commentStatus = typeof rawcommentStatus === 'number' ? rawcommentStatus : Number(rawcommentStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.newsCommentId)
  childNewsCommentLikeRows.value = []
  newsCommentLikeTableRef.value?.resetRows?.()
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
