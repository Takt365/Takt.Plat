<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news/components -->
<!-- 文件名称：news-form.vue -->
<!-- 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form news-form flex flex-col min-h-0"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
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
                :label="t('entity.news.code')"
                name="newsCode"
              >
                <a-input
                  v-model:value="formState.newsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.newsId"
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
                  show-count
                  :maxlength="200"
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
                  show-count
                  :maxlength="2000"
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
                  show-count
                  :maxlength="500"
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
                  show-count
                  :maxlength="500"
                  allow-clear
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
                :label="t('entity.news.istop')"
                name="isTop"
              >
                <a-input-number
                  v-model:value="formState.isTop"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.istop') })"
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
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.effectivetime')"
                name="effectiveTime"
              >
                <a-date-picker
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.effectivetime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.expiretime')"
                name="expireTime"
              >
                <a-date-picker
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.expiretime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                  style="width: 100%"
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
                :label="t('entity.news.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.publisherid')"
                name="publisherId"
              >
                <a-input
                  v-model:value="formState.publisherId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.publishername')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishername') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.publishtime')"
                name="publishTime"
              >
                <a-date-picker
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.publishtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.status')"
                name="newsStatus"
              >
                <TaktSelect
                  v-model:value="formState.newsStatus"
                  dict-type="sys_publish_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.status') })"
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
    <!-- 下：子表 attachments -->
    <TaktEditableTable
      ref="newsAttachmentTableRef"
      v-model="childNewsAttachmentRows"
      :columns="newsAttachmentFormColumns"
      :title="t('entity.newsattachment._self')"
      :add-button-entity="t('entity.newsattachment._self')"
      id-field="newsAttachmentId"
      :default-row="createDefaultNewsAttachmentRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/news-center/news/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NewsCreate } from '@/types/routine/news-center/news'
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
const formFields = ["tenantCode","companyCode","cultureCode","newsCode","newsCategory","newsTitle","newsSummary","tags","newsContent","newsCoverImage","isTop","isRecommended","effectiveTime","expireTime","readCount","likeCount","commentCount","favoriteCount","shareCount","attachmentCount","deptId","deptName","publisherId","publisherName","publishTime","newsStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childNewsAttachmentRows = ref<Record<string, unknown>[]>([])
const newsAttachmentTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 newsAttachment 可编辑列 */
const newsAttachmentFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'fileId',
    title: t('entity.newsattachment.fileid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileName',
    title: t('entity.newsattachment.filename'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'filePath',
    title: t('entity.newsattachment.filepath'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileSize',
    title: t('entity.newsattachment.filesize'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileType',
    title: t('entity.newsattachment.filetype'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.newsattachment.filetype') }),
  },
  {
    key: 'fileExtension',
    title: t('entity.newsattachment.fileextension'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.newsattachment.fileextension') }),
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
function syncChildRowsFromFormData(val: Partial<NewsCreate & { newsId?: string }> | null | undefined) {
  childNewsAttachmentRows.value = ((val as any)?.attachments ?? []) as Record<string, unknown>[]
}

function createDefaultNewsAttachmentRow(): Record<string, unknown> {
  return {
    fileId: '',
    fileName: '',
    filePath: '',
    fileSize: '',
    fileType: '',
    fileExtension: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.newsId ?? ''
  return {
    ...formState,
    attachments: newsAttachmentTableRef.value?.getRows?.() ?? childNewsAttachmentRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      newsId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<NewsCreate & { newsId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  newsStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 newsId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.newsId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).attachments
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
    const isCreate = !props.formData?.newsId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  newsCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.code') }),
      trigger: 'blur'
    }
  ],
  newsCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.category') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.category') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  isTop: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.istop') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.istop') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isRecommended: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.isrecommended') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.isrecommended') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  readCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.readcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.readcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  likeCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.likecount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.likecount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  commentCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.commentcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.commentcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  favoriteCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.favoritecount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.favoritecount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shareCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.sharecount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.sharecount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  attachmentCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.attachmentcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.attachmentcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  newsStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.news.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await newsAttachmentTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('newsCategory' in payload) {
    const rawnewsCategory = payload.newsCategory
    payload.newsCategory = typeof rawnewsCategory === 'number' ? rawnewsCategory : Number(rawnewsCategory)
  }
  if ('isTop' in payload) {
    const rawisTop = payload.isTop
    payload.isTop = typeof rawisTop === 'number' ? rawisTop : Number(rawisTop)
  }
  if ('isRecommended' in payload) {
    const rawisRecommended = payload.isRecommended
    payload.isRecommended = typeof rawisRecommended === 'number' ? rawisRecommended : Number(rawisRecommended)
  }
  if ('readCount' in payload) {
    const rawreadCount = payload.readCount
    payload.readCount = typeof rawreadCount === 'number' ? rawreadCount : Number(rawreadCount)
  }
  if ('likeCount' in payload) {
    const rawlikeCount = payload.likeCount
    payload.likeCount = typeof rawlikeCount === 'number' ? rawlikeCount : Number(rawlikeCount)
  }
  if ('commentCount' in payload) {
    const rawcommentCount = payload.commentCount
    payload.commentCount = typeof rawcommentCount === 'number' ? rawcommentCount : Number(rawcommentCount)
  }
  if ('favoriteCount' in payload) {
    const rawfavoriteCount = payload.favoriteCount
    payload.favoriteCount = typeof rawfavoriteCount === 'number' ? rawfavoriteCount : Number(rawfavoriteCount)
  }
  if ('shareCount' in payload) {
    const rawshareCount = payload.shareCount
    payload.shareCount = typeof rawshareCount === 'number' ? rawshareCount : Number(rawshareCount)
  }
  if ('attachmentCount' in payload) {
    const rawattachmentCount = payload.attachmentCount
    payload.attachmentCount = typeof rawattachmentCount === 'number' ? rawattachmentCount : Number(rawattachmentCount)
  }
  if ('newsStatus' in payload) {
    const rawnewsStatus = payload.newsStatus
    payload.newsStatus = typeof rawnewsStatus === 'number' ? rawnewsStatus : Number(rawnewsStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.newsId)
  childNewsAttachmentRows.value = []
  newsAttachmentTableRef.value?.resetRows?.()
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
