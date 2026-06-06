<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-form.vue -->
<!-- 功能描述：供应商评价考核主表实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="supplier-evaluation-form-tabs"
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
                :label="t('entity.supplierEvaluation.code')"
                name="supplierEvaluationCode"
              >
                <a-input
                  v-model:value="formState.supplierEvaluationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.supplierid')"
                name="supplierId"
              >
                <a-input
                  v-model:value="formState.supplierId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.suppliername')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationdate')"
                name="evaluationDate"
              >
                <a-date-picker
                  v-model:value="formState.evaluationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationperiod')"
                name="evaluationPeriod"
              >
                <a-input-number
                  v-model:value="formState.evaluationPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationperiod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationtype')"
                name="evaluationType"
              >
                <a-input-number
                  v-model:value="formState.evaluationType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationtype') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluatorby')"
                name="evaluatorBy"
              >
                <a-input
                  v-model:value="formState.evaluatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluatorby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationdept')"
                name="evaluationDept"
              >
                <a-input
                  v-model:value="formState.evaluationDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationdept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.overallrating')"
                name="overallRating"
              >
                <a-input-number
                  v-model:value="formState.overallRating"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.overallrating') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.totalscore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.totalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.qualityscore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.qualityscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.deliveryscore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.deliveryscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.pricescore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.pricescore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.servicescore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.servicescore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.technicalscore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.technicalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.mainstrengths')"
                name="mainStrengths"
              >
                <a-input
                  v-model:value="formState.mainStrengths"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainstrengths') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.mainissues')"
                name="mainIssues"
              >
                <a-input
                  v-model:value="formState.mainIssues"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainissues') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.improvementrequirements')"
                name="improvementRequirements"
              >
                <a-input
                  v-model:value="formState.improvementRequirements"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.improvementrequirements') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationconclusion')"
                name="evaluationConclusion"
              >
                <a-input-number
                  v-model:value="formState.evaluationConclusion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationconclusion') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.rectificationdeadline')"
                name="rectificationDeadline"
              >
                <a-input
                  v-model:value="formState.rectificationDeadline"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationdeadline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.evaluationstatus')"
                name="evaluationStatus"
              >
                <a-input-number
                  v-model:value="formState.evaluationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.rectificationstatus')"
                name="rectificationStatus"
              >
                <a-input-number
                  v-model:value="formState.rectificationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierEvaluation.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.items') })"
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
 * 供应商评价考核主表实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierEvaluationCreate } from '@/types/logistics/quality/complaint/supplier-evaluation'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","supplierEvaluationCode","supplierId","supplierName","supplierCode","evaluationDate","evaluationPeriod","evaluationType","evaluatorBy","evaluationDept","overallRating","totalScore","qualityScore","deliveryScore","priceScore","serviceScore","technicalScore","mainStrengths","mainIssues","improvementRequirements","evaluationConclusion","rectificationDeadline","evaluationStatus","rectificationStatus","relatedPlant","sortOrder","items","extFieldJson","remark"]


interface Props {
  formData?: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null
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
    const isCreate = !props.formData?.supplierEvaluationId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  supplierEvaluationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') }),
      trigger: 'blur'
    }
  ],
  supplierId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') }),
      trigger: 'blur'
    }
  ],
  supplierName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') }),
      trigger: 'blur'
    }
  ],
  evaluationDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdate') }),
      trigger: 'change'
    }
  ],
  evaluationPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationperiod') }),
      trigger: 'change'
    }
  ],
  evaluationType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationtype') }),
      trigger: 'change'
    }
  ],
  overallRating: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.overallrating') }),
      trigger: 'change'
    }
  ],
  evaluationConclusion: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationconclusion') }),
      trigger: 'change'
    }
  ],
  evaluationStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationstatus') }),
      trigger: 'change'
    }
  ],
  rectificationStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.rectificationstatus') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.sortorder') }),
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
