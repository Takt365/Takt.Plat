<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-inquiry/components -->
<!-- 文件名称：purchase-inquiry-form.vue -->
<!-- 功能描述：采购询价实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-inquiry-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-inquiry-form-tabs"
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
                :label="t('entity.purchaseinquiry.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.plantcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.purchaseInquiryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.code')"
                name="purchaseInquiryCode"
              >
                <a-input
                  v-model:value="formState.purchaseInquiryCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.code') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.purchaseInquiryId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.inquirydate')"
                name="inquiryDate"
              >
                <a-date-picker
                  v-model:value="formState.inquiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.quotedeadlinedate')"
                name="quoteDeadlineDate"
              >
                <a-date-picker
                  v-model:value="formState.quoteDeadlineDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.quotedeadlinedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.inquiryid')"
                name="inquiryId"
              >
                <a-input
                  v-model:value="formState.inquiryId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.inquiryby')"
                name="inquiryBy"
              >
                <a-input
                  v-model:value="formState.inquiryBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryby') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.suppliercode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.purchaseInquiryId"
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
                :label="t('entity.purchaseinquiry.suppliername')"
                name="supplierName"
              >
                <a-input
                  v-model:value="formState.supplierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.suppliername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.totalquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.totalamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.convertedquantity')"
                name="convertedQuantity"
              >
                <a-input-number
                  v-model:value="formState.convertedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.convertedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.convertedamount')"
                name="convertedAmount"
              >
                <a-input-number
                  v-model:value="formState.convertedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.convertedamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.inquiryreason')"
                name="inquiryReason"
              >
                <a-input
                  v-model:value="formState.inquiryReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryreason') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.inquirystatus')"
                name="inquiryStatus"
              >
                <TaktSelect
                  v-model:value="formState.inquiryStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirystatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseinquiry.convertedstatus')"
                name="convertedStatus"
              >
                <TaktSelect
                  v-model:value="formState.convertedStatus"
                  dict-type="sys_convert_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedstatus') })"
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
      ref="purchaseInquiryItemTableRef"
      v-model="childPurchaseInquiryItemRows"
      :columns="purchaseInquiryItemFormColumns"
      :title="t('entity.purchaseinquiryitem._self')"
      :add-button-entity="t('entity.purchaseinquiryitem._self')"
      id-field="purchaseInquiryItemId"
      :default-row="createDefaultPurchaseInquiryItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 采购询价实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-inquiry/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseInquiryCreate } from '@/types/logistics/procurement/purchase-inquiry'
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","purchaseInquiryCode","inquiryDate","quoteDeadlineDate","inquiryId","inquiryBy","supplierCode","supplierName","totalQuantity","totalAmount","convertedQuantity","convertedAmount","inquiryReason","inquiryStatus","convertedStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childPurchaseInquiryItemRows = ref<Record<string, unknown>[]>([])
const purchaseInquiryItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 purchaseInquiryItem 可编辑列 */
const purchaseInquiryItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.purchaseinquiryitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'allocationCategory',
    title: t('entity.purchaseinquiryitem.allocationcategory'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialCode',
    title: t('entity.purchaseinquiryitem.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: t('entity.purchaseinquiryitem.materialname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialSpecification',
    title: t('entity.purchaseinquiryitem.materialspecification'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.purchaseinquiryitem.materialspecification') }),
  },
  {
    key: 'inquiryUnit',
    title: t('entity.purchaseinquiryitem.inquiryunit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inquiryQuantity',
    title: t('entity.purchaseinquiryitem.inquiryquantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'quotedUnitPrice',
    title: t('entity.purchaseinquiryitem.quotedunitprice'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiryitem.quotedunitprice') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseInquiryCreate & { purchaseInquiryId?: string }> | null | undefined) {
  childPurchaseInquiryItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultPurchaseInquiryItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childPurchaseInquiryItemRows.value.length + 1) * 10,
    allocationCategory: '',
    materialCode: '',
    materialName: '',
    materialSpecification: '',
    inquiryUnit: '',
    inquiryQuantity: 0,
    quotedUnitPrice: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseInquiryId ?? ''
  return {
    ...formState,
    items: purchaseInquiryItemTableRef.value?.getRows?.() ?? childPurchaseInquiryItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      purchaseInquiryId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInquiryCreate & { purchaseInquiryId?: string }> | null
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
  inquiryStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInquiryId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInquiryId) {
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
    const isCreate = !props.formData?.purchaseInquiryId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.plantcode') }),
      trigger: 'blur'
    }
  ],
  purchaseInquiryCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.code') }),
      trigger: 'blur'
    }
  ],
  inquiryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirydate') }),
      trigger: 'change'
    }
  ],
  inquiryBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryby') }),
      trigger: 'blur'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.totalquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.totalquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.totalamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.totalamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inquiryStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirystatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirystatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseInquiryItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    payload.totalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
  }
  if ('totalAmount' in payload) {
    const rawtotalAmount = payload.totalAmount
    payload.totalAmount = typeof rawtotalAmount === 'number' ? rawtotalAmount : Number(rawtotalAmount)
  }
  if ('convertedQuantity' in payload) {
    const rawconvertedQuantity = payload.convertedQuantity
    payload.convertedQuantity = typeof rawconvertedQuantity === 'number' ? rawconvertedQuantity : Number(rawconvertedQuantity)
  }
  if ('convertedAmount' in payload) {
    const rawconvertedAmount = payload.convertedAmount
    payload.convertedAmount = typeof rawconvertedAmount === 'number' ? rawconvertedAmount : Number(rawconvertedAmount)
  }
  if ('inquiryStatus' in payload) {
    const rawinquiryStatus = payload.inquiryStatus
    payload.inquiryStatus = typeof rawinquiryStatus === 'number' ? rawinquiryStatus : Number(rawinquiryStatus)
  }
  if ('convertedStatus' in payload) {
    const rawconvertedStatus = payload.convertedStatus
    payload.convertedStatus = typeof rawconvertedStatus === 'number' ? rawconvertedStatus : Number(rawconvertedStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseInquiryId)
  childPurchaseInquiryItemRows.value = []
  purchaseInquiryItemTableRef.value?.resetRows?.()
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
