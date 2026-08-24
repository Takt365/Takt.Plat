<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/countersign/components -->
<!-- 文件名称：countersign-form.vue -->
<!-- 功能描述：会签单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form countersign-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="countersign-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseInquiryId')"
                name="purchaseInquiryId"
              >
                <a-input
                  v-model:value="formState.purchaseInquiryId"
                  :placeholder="pi.ph('purchaseInquiryId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseInquiryCode')"
                name="purchaseInquiryCode"
              >
                <a-input
                  v-model:value="formState.purchaseInquiryCode"
                  :placeholder="pi.ph('purchaseInquiryCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessType')"
                name="businessType"
              >
                <TaktSelect
                  v-model:value="formState.businessType"
                  dict-type="logistics_countersign_business_type"
                  :placeholder="pi.ph('businessType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessKey')"
                name="businessKey"
              >
                <a-input
                  v-model:value="formState.businessKey"
                  :placeholder="pi.ph('businessKey')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stepNo')"
                name="stepNo"
              >
                <a-input-number
                  v-model:value="formState.stepNo"
                  :placeholder="pi.ph('stepNo')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignDepts')"
                name="countersignDepts"
              >
                <a-input
                  v-model:value="formState.countersignDepts"
                  :placeholder="pi.ph('countersignDepts')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('financeDept')"
                name="financeDept"
              >
                <a-input
                  v-model:value="formState.financeDept"
                  :placeholder="pi.ph('financeDept')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetReviewComment')"
                name="budgetReviewComment"
              >
                <a-input
                  v-model:value="formState.budgetReviewComment"
                  :placeholder="pi.ph('budgetReviewComment')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('executiveOffice')"
                name="executiveOffice"
              >
                <a-input
                  v-model:value="formState.executiveOffice"
                  :placeholder="pi.ph('executiveOffice')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantBy')"
                name="applicantBy"
              >
                <TaktSelect
                  v-model:value="formState.applicantBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('applicantBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationDept')"
                name="applicationDept"
              >
                <TaktSelect
                  v-model:value="formState.applicationDept"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('applicationDept')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costBearerDept')"
                name="costBearerDept"
              >
                <TaktSelect
                  v-model:value="formState.costBearerDept"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('costBearerDept')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isBudget')"
                name="isBudget"
              >
                <TaktSelect
                  v-model:value="formState.isBudget"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isBudget')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetItem')"
                name="budgetItem"
              >
                <a-input
                  v-model:value="formState.budgetItem"
                  :placeholder="pi.ph('budgetItem')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('budgetAmount')"
                name="budgetAmount"
              >
                <a-input-number
                  v-model:value="formState.budgetAmount"
                  :placeholder="pi.ph('budgetAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationAmount')"
                name="applicationAmount"
              >
                <a-input-number
                  v-model:value="formState.applicationAmount"
                  :placeholder="pi.ph('applicationAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignTitle')"
                name="countersignTitle"
              >
                <a-input
                  v-model:value="formState.countersignTitle"
                  :placeholder="pi.ph('countersignTitle')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicationReason')"
                name="applicationReason"
              >
                <a-input
                  v-model:value="formState.applicationReason"
                  :placeholder="pi.ph('applicationReason')"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('budgetUsageDescription')"
                name="budgetUsageDescription"
              >
                <a-textarea
                  v-model:value="formState.budgetUsageDescription"
                  :placeholder="pi.ph('budgetUsageDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetAndExpectedBenefit')"
                name="targetAndExpectedBenefit"
              >
                <a-input
                  v-model:value="formState.targetAndExpectedBenefit"
                  :placeholder="pi.ph('targetAndExpectedBenefit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="pi.ph('attachments')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('countersignStatus')"
                name="countersignStatus"
              >
                <TaktSelect
                  v-model:value="formState.countersignStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('countersignStatus')"
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 countersignDetails -->
    <TaktEditableTable
      ref="countersignDetailTableRef"
      v-model="childCountersignDetailRows"
      :columns="countersignDetailFormColumns"
      :title="countersignDetailPi.self()"
      :add-button-entity="countersignDetailPi.self()"
      id-field="countersignDetailId"
      :default-row="createDefaultCountersignDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-allocationCategory="{ record }">
        <TaktSelect
          v-model:value="record.allocationCategory"
          dict-type="logistics_allocation_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="countersignDetailPi.ph('allocationCategory')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-accountTitle="{ record }">
        <TaktSelect
          v-model:value="record.accountTitle"
          api-url="TaktAccountTitles/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="countersignDetailPi.queryPh('accountTitle', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="countersignDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会签单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/countersign/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCountersignI18n } from '../composables/use-countersign-i18n'

/** 实体字段 i18n */
const pi = useCountersignI18n()

import type { CountersignCreate } from '@/types/accounting/financial/countersign'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseInquiryId","purchaseInquiryCode","businessType","businessKey","stepNo","countersignDepts","financeDept","budgetReviewComment","executiveOffice","applicantBy","applicationDept","costBearerDept","isBudget","budgetItem","budgetAmount","applicationAmount","countersignTitle","applicationReason","budgetUsageDescription","targetAndExpectedBenefit","attachments","countersignStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useCountersignDetailI18n } from '../composables/use-countersign-detail-i18n'

const countersignDetailPi = useCountersignDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childCountersignDetailRows = ref<Record<string, unknown>[]>([])
const countersignDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedCountersignDetailRow(row: Record<string, unknown>): boolean {
  const id = row.countersignDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextCountersignDetailLineNumber(): number {
  const rows = countersignDetailTableRef.value?.getRows?.() ?? childCountersignDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 countersignDetail 可编辑列 */
const countersignDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: countersignDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'allocationCategory',
    title: countersignDetailPi.label('allocationCategory'),
    width: 140,
  },
  {
    key: 'accountTitle',
    title: countersignDetailPi.label('accountTitle'),
    width: 140,
  },
  {
    key: 'itemName',
    title: countersignDetailPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: countersignDetailPi.label('itemDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: countersignDetailPi.ph('itemDescription'),
    width: 180,
  },
  {
    key: 'itemQuantity',
    title: countersignDetailPi.label('itemQuantity'),
    width: 140,
  },
  {
    key: 'itemAmount',
    title: countersignDetailPi.label('itemAmount'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: countersignDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CountersignCreate & { countersignId?: string }> | null | undefined) {
  const rows_countersignDetail = ((val as any)?.countersignDetails ?? []) as Record<string, unknown>[]
  childCountersignDetailRows.value = rows_countersignDetail
}

function createDefaultCountersignDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextCountersignDetailLineNumber(),
    allocationCategory: '',
    accountTitle: '',
    itemName: '',
    itemDescription: '',
    itemQuantity: 0,
    itemAmount: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.countersignId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    countersignDetails: countersignDetailTableRef.value?.getRows?.() ?? childCountersignDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        countersignId: masterId,
      }
      if (isUpdate && isPersistedCountersignDetailRow(row)) {
        normalized.countersignDetailId = row.countersignDetailId
      } else {
        delete normalized.countersignDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CountersignCreate & { countersignId?: string }> | null
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
  businessType: "INQUIRY",
  countersignStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 countersignId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.countersignId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).countersignDetails
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.countersignId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  businessType: [
    {
      required: true,
      message: pi.ph('businessType'),
      trigger: 'change'
    }
  ],
  stepNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stepNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stepNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  applicantBy: [
    {
      required: true,
      message: pi.ph('applicantBy'),
      trigger: 'change'
    }
  ],
  isBudget: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBudget'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBudget'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  budgetAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('budgetAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('budgetAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  applicationAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('applicationAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('applicationAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  countersignStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('countersignStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('countersignStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await countersignDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('stepNo' in payload) {
    const rawstepNo = payload.stepNo
    if (rawstepNo === undefined || rawstepNo === null || rawstepNo === '') {
      delete payload.stepNo
    } else {
      const numstepNo = typeof rawstepNo === 'number' ? rawstepNo : Number(rawstepNo)
      if (Number.isFinite(numstepNo)) payload.stepNo = numstepNo
      else delete payload.stepNo
    }
  }
  if ('isBudget' in payload) {
    const rawisBudget = payload.isBudget
    if (rawisBudget === undefined || rawisBudget === null || rawisBudget === '') {
      delete payload.isBudget
    } else {
      const numisBudget = typeof rawisBudget === 'number' ? rawisBudget : Number(rawisBudget)
      if (Number.isFinite(numisBudget)) payload.isBudget = numisBudget
      else delete payload.isBudget
    }
  }
  if ('budgetAmount' in payload) {
    const rawbudgetAmount = payload.budgetAmount
    if (rawbudgetAmount === undefined || rawbudgetAmount === null || rawbudgetAmount === '') {
      delete payload.budgetAmount
    } else {
      const numbudgetAmount = typeof rawbudgetAmount === 'number' ? rawbudgetAmount : Number(rawbudgetAmount)
      if (Number.isFinite(numbudgetAmount)) payload.budgetAmount = numbudgetAmount
      else delete payload.budgetAmount
    }
  }
  if ('applicationAmount' in payload) {
    const rawapplicationAmount = payload.applicationAmount
    if (rawapplicationAmount === undefined || rawapplicationAmount === null || rawapplicationAmount === '') {
      delete payload.applicationAmount
    } else {
      const numapplicationAmount = typeof rawapplicationAmount === 'number' ? rawapplicationAmount : Number(rawapplicationAmount)
      if (Number.isFinite(numapplicationAmount)) payload.applicationAmount = numapplicationAmount
      else delete payload.applicationAmount
    }
  }
  if ('countersignStatus' in payload) {
    const rawcountersignStatus = payload.countersignStatus
    if (rawcountersignStatus === undefined || rawcountersignStatus === null || rawcountersignStatus === '') {
      delete payload.countersignStatus
    } else {
      const numcountersignStatus = typeof rawcountersignStatus === 'number' ? rawcountersignStatus : Number(rawcountersignStatus)
      if (Number.isFinite(numcountersignStatus)) payload.countersignStatus = numcountersignStatus
      else delete payload.countersignStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.countersignId) {
    payload.countersignId = props.formData.countersignId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.countersignId)
  childCountersignDetailRows.value = []
  countersignDetailTableRef.value?.resetRows?.()
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
