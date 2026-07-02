<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-form.vue -->
<!-- 功能描述：供应商评价考核主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form supplier-evaluation-form flex flex-col min-h-0"
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
                :label="t('entity.supplierevaluation.code')"
                name="supplierEvaluationCode"
              >
                <a-input
                  v-model:value="formState.supplierEvaluationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.supplierEvaluationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.supplierid')"
                name="supplierId"
              >
                <a-input
                  v-model:value="formState.supplierId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.supplierid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.suppliername')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.suppliername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.suppliercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.supplierEvaluationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationdate')"
                name="evaluationDate"
              >
                <a-date-picker
                  v-model:value="formState.evaluationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationperiod')"
                name="evaluationPeriod"
              >
                <a-input-number
                  v-model:value="formState.evaluationPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationperiod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationtype')"
                name="evaluationType"
              >
                <a-input-number
                  v-model:value="formState.evaluationType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationtype') })"
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
                :label="t('entity.supplierevaluation.evaluatorby')"
                name="evaluatorBy"
              >
                <a-input
                  v-model:value="formState.evaluatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluatorby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationdept')"
                name="evaluationDept"
              >
                <a-input
                  v-model:value="formState.evaluationDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationdept') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.overallrating')"
                name="overallRating"
              >
                <a-input-number
                  v-model:value="formState.overallRating"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.overallrating') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.totalscore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.totalscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.qualityscore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.qualityscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.deliveryscore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.deliveryscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.pricescore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.pricescore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.servicescore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.servicescore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.technicalscore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.technicalscore') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.supplierevaluation.mainstrengths')"
                name="mainStrengths"
              >
                <a-input
                  v-model:value="formState.mainStrengths"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.mainstrengths') })"
                  show-count
                  :maxlength="2000"
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
                :label="t('entity.supplierevaluation.mainissues')"
                name="mainIssues"
              >
                <a-input
                  v-model:value="formState.mainIssues"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.mainissues') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.improvementrequirements')"
                name="improvementRequirements"
              >
                <a-input
                  v-model:value="formState.improvementRequirements"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.improvementrequirements') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationconclusion')"
                name="evaluationConclusion"
              >
                <a-input-number
                  v-model:value="formState.evaluationConclusion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationconclusion') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.rectificationdeadline')"
                name="rectificationDeadline"
              >
                <a-date-picker
                  v-model:value="formState.rectificationDeadline"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.rectificationdeadline') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.evaluationstatus')"
                name="evaluationStatus"
              >
                <a-input-number
                  v-model:value="formState.evaluationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.rectificationstatus')"
                name="rectificationStatus"
              >
                <a-input-number
                  v-model:value="formState.rectificationStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.rectificationstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.supplierevaluation.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.relatedplant') })"
                  show-count
                  :maxlength="4"
                  allow-clear
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="supplierEvaluationItemTableRef"
      v-model="childSupplierEvaluationItemRows"
      :columns="supplierEvaluationItemFormColumns"
      :title="t('entity.supplierevaluationitem._self')"
      :add-button-entity="t('entity.supplierevaluationitem._self')"
      id-field="supplierEvaluationItemId"
      :default-row="createDefaultSupplierEvaluationItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 供应商评价考核主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SupplierEvaluationCreate } from '@/types/logistics/quality/complaint/supplier-evaluation'
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","supplierEvaluationCode","supplierId","supplierName","supplierCode","evaluationDate","evaluationPeriod","evaluationType","evaluatorBy","evaluationDept","overallRating","totalScore","qualityScore","deliveryScore","priceScore","serviceScore","technicalScore","mainStrengths","mainIssues","improvementRequirements","evaluationConclusion","rectificationDeadline","evaluationStatus","rectificationStatus","relatedPlant","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childSupplierEvaluationItemRows = ref<Record<string, unknown>[]>([])
const supplierEvaluationItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 supplierEvaluationItem 可编辑列 */
const supplierEvaluationItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.supplierevaluationitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'categoryType',
    title: t('entity.supplierevaluationitem.categorytype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'itemName',
    title: t('entity.supplierevaluationitem.itemname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: t('entity.supplierevaluationitem.itemdescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.supplierevaluationitem.itemdescription') }),
    width: 140,
  },
  {
    key: 'weight',
    title: t('entity.supplierevaluationitem.weight'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'scoringStandard',
    title: t('entity.supplierevaluationitem.scoringstandard'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.supplierevaluationitem.scoringstandard') }),
  },
  {
    key: 'score',
    title: t('entity.supplierevaluationitem.score'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'ratingLevel',
    title: t('entity.supplierevaluationitem.ratinglevel'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null | undefined) {
  childSupplierEvaluationItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultSupplierEvaluationItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childSupplierEvaluationItemRows.value.length + 1) * 10,
    categoryType: 0,
    itemName: '',
    itemDescription: '',
    weight: 0,
    scoringStandard: '',
    score: 0,
    ratingLevel: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.supplierEvaluationId ?? ''
  return {
    ...formState,
    items: supplierEvaluationItemTableRef.value?.getRows?.() ?? childSupplierEvaluationItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      evaluationId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 supplierEvaluationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.supplierEvaluationId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
    const isCreate = !props.formData?.supplierEvaluationId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  supplierEvaluationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.code') }),
      trigger: 'blur'
    }
  ],
  supplierId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.supplierid') }),
      trigger: 'blur'
    }
  ],
  supplierName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.suppliername') }),
      trigger: 'blur'
    }
  ],
  evaluationDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationdate') }),
      trigger: 'change'
    }
  ],
  evaluationPeriod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationperiod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationperiod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationtype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationtype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overallRating: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.overallrating') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.overallrating') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationConclusion: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationconclusion') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationconclusion') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  rectificationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.rectificationstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.rectificationstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await supplierEvaluationItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('evaluationPeriod' in payload) {
    const rawevaluationPeriod = payload.evaluationPeriod
    payload.evaluationPeriod = typeof rawevaluationPeriod === 'number' ? rawevaluationPeriod : Number(rawevaluationPeriod)
  }
  if ('evaluationType' in payload) {
    const rawevaluationType = payload.evaluationType
    payload.evaluationType = typeof rawevaluationType === 'number' ? rawevaluationType : Number(rawevaluationType)
  }
  if ('overallRating' in payload) {
    const rawoverallRating = payload.overallRating
    payload.overallRating = typeof rawoverallRating === 'number' ? rawoverallRating : Number(rawoverallRating)
  }
  if ('totalScore' in payload) {
    const rawtotalScore = payload.totalScore
    payload.totalScore = typeof rawtotalScore === 'number' ? rawtotalScore : Number(rawtotalScore)
  }
  if ('qualityScore' in payload) {
    const rawqualityScore = payload.qualityScore
    payload.qualityScore = typeof rawqualityScore === 'number' ? rawqualityScore : Number(rawqualityScore)
  }
  if ('deliveryScore' in payload) {
    const rawdeliveryScore = payload.deliveryScore
    payload.deliveryScore = typeof rawdeliveryScore === 'number' ? rawdeliveryScore : Number(rawdeliveryScore)
  }
  if ('priceScore' in payload) {
    const rawpriceScore = payload.priceScore
    payload.priceScore = typeof rawpriceScore === 'number' ? rawpriceScore : Number(rawpriceScore)
  }
  if ('serviceScore' in payload) {
    const rawserviceScore = payload.serviceScore
    payload.serviceScore = typeof rawserviceScore === 'number' ? rawserviceScore : Number(rawserviceScore)
  }
  if ('technicalScore' in payload) {
    const rawtechnicalScore = payload.technicalScore
    payload.technicalScore = typeof rawtechnicalScore === 'number' ? rawtechnicalScore : Number(rawtechnicalScore)
  }
  if ('evaluationConclusion' in payload) {
    const rawevaluationConclusion = payload.evaluationConclusion
    payload.evaluationConclusion = typeof rawevaluationConclusion === 'number' ? rawevaluationConclusion : Number(rawevaluationConclusion)
  }
  if ('evaluationStatus' in payload) {
    const rawevaluationStatus = payload.evaluationStatus
    payload.evaluationStatus = typeof rawevaluationStatus === 'number' ? rawevaluationStatus : Number(rawevaluationStatus)
  }
  if ('rectificationStatus' in payload) {
    const rawrectificationStatus = payload.rectificationStatus
    payload.rectificationStatus = typeof rawrectificationStatus === 'number' ? rawrectificationStatus : Number(rawrectificationStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.supplierEvaluationId)
  childSupplierEvaluationItemRows.value = []
  supplierEvaluationItemTableRef.value?.resetRows?.()
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
