<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/knowledge/components -->
<!-- 文件名称：knowledge-form.vue -->
<!-- 功能描述：服务台知识库实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="knowledge-form-tabs"
    >
      <!-- 主表 -->
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
                :label="t('entity.knowledge.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.title') })"
                  size="small"
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
                  size="small"
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
                  size="small"
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
                  size="small"
                  allow-clear
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
                  size="small"
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
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.sortorder') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.viewcount')"
                name="viewCount"
              >
                <a-input-number
                  v-model:value="formState.viewCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.viewcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.helpfulcount')"
                name="helpfulCount"
              >
                <a-input-number
                  v-model:value="formState.helpfulCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.helpfulcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.unhelpfulcount')"
                name="unhelpfulCount"
              >
                <a-input-number
                  v-model:value="formState.unhelpfulCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.unhelpfulcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.ispublished')"
                name="isPublished"
              >
                <a-input-number
                  v-model:value="formState.isPublished"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.ispublished') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.version')"
                name="version"
              >
                <a-input-number
                  v-model:value="formState.version"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.version') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.publishedat')"
                name="publishedAt"
              >
                <a-input
                  v-model:value="formState.publishedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.publishedat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.knowledge.revisedat')"
                name="revisedAt"
              >
                <a-input
                  v-model:value="formState.revisedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.revisedat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.ExtField')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
      <!-- 子表：knowledgeChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.knowledgeChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddKnowledgeChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.knowledgeChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="knowledgeChangeLogFormColumns"
          :data-source="childKnowledgeChangeLogRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'knowledgeTitle'">
              <a-input
                v-model:value="record.knowledgeTitle"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.knowledgetitle') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeSummary'">
              <a-input
                v-model:value="record.changeSummary"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changesummary') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'versionAtChange'">
              <a-input-number
                v-model:value="record.versionAtChange"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledgeChangeLog.versionatchange') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'ExtField'">
              <a-input
                v-model:value="record.ExtField"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveKnowledgeChangeLogRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 服务台知识库实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/knowledge/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { KnowledgeCreate, KnowledgeChangeLogCreate, KnowledgeChangeLog } from '@/types/routine/help-desk/knowledge'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","title","content","summary","categoryCode","tags","knowledgeStatus","sortOrder","viewCount","helpfulCount","unhelpfulCount","isPublished","version","publishedAt","revisedAt","ExtField","remark"]

/** knowledgeChangeLog 子表行（表单 Tab 内嵌） */
const childKnowledgeChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 knowledgeChangeLog 表单列定义 */
const knowledgeChangeLogFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.knowledgetitle'),
    dataIndex: 'knowledgeTitle',
    key: 'knowledgeTitle',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('entity.knowledgeChangeLog.versionatchange'),
    dataIndex: 'versionAtChange',
    key: 'versionAtChange',
    width: 140,
  },
  {
    title: t('common.page.entity.ExtField'),
    dataIndex: 'ExtField',
    key: 'ExtField',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<KnowledgeCreate & { knowledgeId?: string }> | null | undefined) {
  childKnowledgeChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.knowledgeChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 knowledgeChangeLog 行 */
function handleAddKnowledgeChangeLogRow() {
  childKnowledgeChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      knowledgeTitle: '',
      changeType: 0,
      changeSummary: '',
      changeFields: '',
      changeReason: '',
      versionAtChange: 0,
      ExtField: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 knowledgeChangeLog 行 */
function handleRemoveKnowledgeChangeLogRow(index: number) {
  childKnowledgeChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    changeLogs: childKnowledgeChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<KnowledgeCreate & { knowledgeId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
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
  knowledgeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.status') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.sortorder') }),
      trigger: 'change'
    }
  ],
  viewCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.viewcount') }),
      trigger: 'change'
    }
  ],
  helpfulCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.helpfulcount') }),
      trigger: 'change'
    }
  ],
  unhelpfulCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.unhelpfulcount') }),
      trigger: 'change'
    }
  ],
  isPublished: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.ispublished') }),
      trigger: 'change'
    }
  ],
  version: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.knowledge.version') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childKnowledgeChangeLogRows.value = []
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
