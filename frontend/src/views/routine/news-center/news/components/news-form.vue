<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news/components -->
<!-- 文件名称：news-form.vue -->
<!-- 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="news-form-tabs"
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
                :label="t('entity.news.code')"
                name="newsCode"
              >
                <a-input
                  v-model:value="formState.newsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.category')"
                name="newsCategory"
              >
                <a-input-number
                  v-model:value="formState.newsCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.category') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.title')"
                name="newsTitle"
              >
                <a-input
                  v-model:value="formState.newsTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.summary')"
                name="newsSummary"
              >
                <a-input
                  v-model:value="formState.newsSummary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.summary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.tags') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.content')"
                name="newsContent"
              >
                <a-textarea
                  v-model:value="formState.newsContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.news.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.coverimage')"
                name="newsCoverImage"
              >
                <a-input
                  v-model:value="formState.newsCoverImage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.coverimage') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.istop')"
                name="isTop"
              >
                <a-input-number
                  v-model:value="formState.isTop"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.istop') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.isrecommended')"
                name="isRecommended"
              >
                <a-input-number
                  v-model:value="formState.isRecommended"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.isrecommended') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.effectivetime')"
                name="effectiveTime"
              >
                <a-input
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.effectivetime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.expiretime')"
                name="expireTime"
              >
                <a-input
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.expiretime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.readcount')"
                name="readCount"
              >
                <a-input-number
                  v-model:value="formState.readCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.readcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.likecount')"
                name="likeCount"
              >
                <a-input-number
                  v-model:value="formState.likeCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.likecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.commentcount')"
                name="commentCount"
              >
                <a-input-number
                  v-model:value="formState.commentCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.commentcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.favoritecount')"
                name="favoriteCount"
              >
                <a-input-number
                  v-model:value="formState.favoriteCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.favoritecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.sharecount')"
                name="shareCount"
              >
                <a-input-number
                  v-model:value="formState.shareCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sharecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.attachmentcount')"
                name="attachmentCount"
              >
                <a-input-number
                  v-model:value="formState.attachmentCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.attachmentcount') })"
                  size="small"
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
                :label="t('entity.news.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publisherid')"
                name="publisherId"
              >
                <a-input
                  v-model:value="formState.publisherId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publishername')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.status')"
                name="newsStatus"
              >
                <a-input-number
                  v-model:value="formState.newsStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.attachments') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.comments')"
                name="comments"
              >
                <a-input
                  v-model:value="formState.comments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.comments') })"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.likes')"
                name="likes"
              >
                <a-input
                  v-model:value="formState.likes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.likes') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.reads')"
                name="reads"
              >
                <a-input
                  v-model:value="formState.reads"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.reads') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.favorites')"
                name="favorites"
              >
                <a-input
                  v-model:value="formState.favorites"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.favorites') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.shares')"
                name="shares"
              >
                <a-input
                  v-model:value="formState.shares"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.shares') })"
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
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/routine/news-center/news/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NewsCreate } from '@/types/routine/news-center/news'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","newsCode","newsCategory","newsTitle","newsSummary","tags","newsContent","newsCoverImage","isTop","isRecommended","effectiveTime","expireTime","readCount","likeCount","commentCount","favoriteCount","shareCount","attachmentCount","flowInstanceId","deptId","deptName","publisherId","publisherName","publishTime","sortOrder","newsStatus","attachments","comments","likes","reads","favorites","shares","extFieldJson","remark"]


interface Props {
  formData?: Partial<NewsCreate & { newsId?: string }> | null
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
    const isCreate = !props.formData?.newsId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  newsCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.code') }),
      trigger: 'blur'
    }
  ],
  newsCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.category') }),
      trigger: 'change'
    }
  ],
  newsTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.title') }),
      trigger: 'blur'
    }
  ],
  newsContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.content') }),
      trigger: 'blur'
    }
  ],
  isTop: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.istop') }),
      trigger: 'change'
    }
  ],
  isRecommended: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.isrecommended') }),
      trigger: 'change'
    }
  ],
  readCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.readcount') }),
      trigger: 'change'
    }
  ],
  likeCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.likecount') }),
      trigger: 'change'
    }
  ],
  commentCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.commentcount') }),
      trigger: 'change'
    }
  ],
  favoriteCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.favoritecount') }),
      trigger: 'change'
    }
  ],
  shareCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.sharecount') }),
      trigger: 'change'
    }
  ],
  attachmentCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.attachmentcount') }),
      trigger: 'change'
    }
  ],
  publisherId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') }),
      trigger: 'blur'
    }
  ],
  publisherName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.publishername') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.sortorder') }),
      trigger: 'change'
    }
  ],
  newsStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.status') }),
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
