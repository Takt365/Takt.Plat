<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation-item/components -->
<!-- 文件名称：supplier-evaluation-item-form.vue -->
<!-- 功能描述：供应商评价考核项目明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="supplier-evaluation-item-form-tabs"
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
                :label="t('entity.supplierEvaluationItem.evaluationid')"
                name="evaluationId"
              >
                <a-input
                  v-model:value="formState.evaluationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.evaluationid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.supplierevaluationcode')"
                name="supplierEvaluationCode"
              >
                <a-input
                  v-model:value="formState.supplierEvaluationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.supplierevaluationcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.categorytype')"
                name="categoryType"
              >
                <a-input-number
                  v-model:value="formState.categoryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.categorytype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.itemname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.itemdescription')"
                name="itemDescription"
              >
                <a-textarea
                  v-model:value="formState.itemDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplierEvaluationItem.itemdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.weight')"
                name="weight"
              >
                <a-input-number
                  v-model:value="formState.weight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.weight') })"
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
                :label="t('entity.supplierEvaluationItem.scoringstandard')"
                name="scoringStandard"
              >
                <a-input
                  v-model:value="formState.scoringStandard"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.scoringstandard') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.score')"
                name="score"
              >
                <a-input-number
                  v-model:value="formState.score"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.score') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.ratinglevel')"
                name="ratingLevel"
              >
                <a-input-number
                  v-model:value="formState.ratingLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.ratinglevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.evaluationcomment')"
                name="evaluationComment"
              >
                <a-input
                  v-model:value="formState.evaluationComment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.evaluationcomment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.existingissues')"
                name="existingIssues"
              >
                <a-input
                  v-model:value="formState.existingIssues"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.existingissues') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.improvementrequirement')"
                name="improvementRequirement"
              >
                <a-input
                  v-model:value="formState.improvementRequirement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.improvementrequirement') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.rectificationrequired')"
                name="rectificationRequired"
              >
                <a-input-number
                  v-model:value="formState.rectificationRequired"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.rectificationrequired') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.rectificationdeadline')"
                name="rectificationDeadline"
              >
                <a-input
                  v-model:value="formState.rectificationDeadline"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.rectificationdeadline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluationItem.rectificationstatus')"
                name="rectificationStatus"
              >
                <a-input-number
                  v-model:value="formState.rectificationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.rectificationstatus') })"
                  size="small"
                  style="width: 100%"
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
 * 供应商评价考核项目明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierEvaluationItemCreate } from '@/types/logistics/quality/complaint/supplier-evaluation-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","evaluationId","supplierEvaluationCode","lineNumber","categoryType","itemName","itemDescription","weight","scoringStandard","score","ratingLevel","evaluationComment","existingIssues","improvementRequirement","rectificationRequired","rectificationDeadline","rectificationStatus","extFieldJson","remark"]


interface Props {
  formData?: Partial<SupplierEvaluationItemCreate & { supplierEvaluationItemId?: string }> | null
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
    const isCreate = !props.formData?.supplierEvaluationItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  evaluationId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.evaluationid') }),
      trigger: 'blur'
    }
  ],
  supplierEvaluationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.supplierevaluationcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluationItem.linenumber') }),
      trigger: 'change'
    }
  ],
  categoryType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluationItem.categorytype') }),
      trigger: 'change'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluationItem.itemname') }),
      trigger: 'blur'
    }
  ],
  weight: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluationItem.weight') }),
      trigger: 'change'
    }
  ],
  rectificationRequired: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluationItem.rectificationrequired') }),
      trigger: 'change'
    }
  ],
  rectificationStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluationItem.rectificationstatus') }),
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
