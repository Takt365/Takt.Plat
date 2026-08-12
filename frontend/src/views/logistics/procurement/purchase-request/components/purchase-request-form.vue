<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-request/components -->
<!-- 文件名称：purchase-request-form.vue -->
<!-- 功能描述：Takt采购申请实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-request-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-request-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="purchaseRequestItemTableRef"
      v-model="childPurchaseRequestItemRows"
      :columns="purchaseRequestItemFormColumns"
      :title="purchaseRequestItemPi.self()"
      :add-button-entity="purchaseRequestItemPi.self()"
      id-field="purchaseRequestItemId"
      :default-row="createDefaultPurchaseRequestItemRow"
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
          :placeholder="purchaseRequestItemPi.ph('allocationCategory')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-requestUnit="{ record }">
        <TaktSelect
          v-model:value="record.requestUnit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.ph('requestUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchasePerUnit="{ record }">
        <TaktSelect
          v-model:value="record.purchasePerUnit"
          dict-type="logistics_price_unit_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.ph('purchasePerUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseRequestItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt采购申请实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-request/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseRequestI18n } from '../composables/use-purchase-request-i18n'

/** 实体字段 i18n */
const pi = usePurchaseRequestI18n()

import type { PurchaseRequestCreate } from '@/types/logistics/procurement/purchase-request'
import { applyTaxRateFromTaxCode } from '@/utils/tax-code'
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
    target.cultureCode = userStore.userInfo?.companyDefaultCulture || 'zh-CN'
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
const formFields = ["tenantCode","companyCode","plantCode","purchaseRequestCode","purchaseInquiryId","purchaseInquiryCode","purchasePlanId","purchasePlanCode","chainScheme","poDecision","countersignId","countersignCode","requestDate","requiredArrivalDate","requestId","requestBy","supplierCode","supplierName1","currencyCode","taxCode","taxRate","taxAmount","totalQuantity","totalAmount","convertedQuantity","convertedAmount","requestReason","cultureCode","requestStatus","convertedStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { usePurchaseRequestItemI18n } from '../composables/use-purchase-request-item-i18n'

const purchaseRequestItemPi = usePurchaseRequestItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseRequestItemRows = ref<Record<string, unknown>[]>([])
const purchaseRequestItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseRequestItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseRequestItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseRequestItemLineNumber(): number {
  const rows = purchaseRequestItemTableRef.value?.getRows?.() ?? childPurchaseRequestItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseRequestItem 可编辑列 */
const purchaseRequestItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'purchasePlanItemId',
    title: purchaseRequestItemPi.label('purchasePlanItemId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseRequestItemPi.ph('purchasePlanItemId'),
  },
  {
    key: 'lineNumber',
    title: purchaseRequestItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'allocationCategory',
    title: purchaseRequestItemPi.label('allocationCategory'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: purchaseRequestItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'requestUnit',
    title: purchaseRequestItemPi.label('requestUnit'),
    width: 140,
  },
  {
    key: 'requestQuantity',
    title: purchaseRequestItemPi.label('requestQuantity'),
    width: 140,
  },
  {
    key: 'convertedQuantity',
    title: purchaseRequestItemPi.label('convertedQuantity'),
    width: 140,
  },
  {
    key: 'purchasePerUnit',
    title: purchaseRequestItemPi.label('purchasePerUnit'),
    width: 140,
  },
  {
    key: 'purchaseRequestUnitPrice',
    title: purchaseRequestItemPi.label('purchaseRequestUnitPrice'),
    width: 140,
  },
  {
    key: 'taxIncludedAmount',
    title: purchaseRequestItemPi.label('taxIncludedAmount'),
    width: 140,
  },
  {
    key: 'untaxedAmount',
    title: purchaseRequestItemPi.label('untaxedAmount'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: purchaseRequestItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseRequestItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null | undefined) {
  const rows_purchaseRequestItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseRequestItemRows.value = rows_purchaseRequestItem
}

function createDefaultPurchaseRequestItemRow(): Record<string, unknown> {
  return {
    purchasePlanItemId: '',
    lineNumber: allocateNextPurchaseRequestItemLineNumber(),
    allocationCategory: '',
    materialCode: '',
    requestUnit: '',
    requestQuantity: 0,
    convertedQuantity: 0,
    purchasePerUnit: 0,
    purchaseRequestUnitPrice: 0,
    taxIncludedAmount: 0,
    untaxedAmount: 0,
    taxAmount: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseRequestId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseRequestItemTableRef.value?.getRows?.() ?? childPurchaseRequestItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        purchaseRequestId: masterId,
      }
      if (isUpdate && isPersistedPurchaseRequestItemRow(row)) {
        normalized.purchaseRequestItemId = row.purchaseRequestItemId
      } else {
        delete normalized.purchaseRequestItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null
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
  chainScheme: 1,
  currencyCode: "CNY",
  cultureCode: "zh-CN",
  taxCode: "J2",
  taxRate: 13,
  requestStatus: 0,
  convertedStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseRequestId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseRequestId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
      formState.taxRate = applyTaxRateFromTaxCode(formState.taxCode, formState.taxRate ?? 13)
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
    const isCreate = !props.formData?.purchaseRequestId
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
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  purchaseRequestCode: [
    {
      required: true,
      message: pi.ph('purchaseRequestCode'),
      trigger: 'blur'
    }
  ],
  chainScheme: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('chainScheme'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('chainScheme'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestDate: [
    {
      required: true,
      message: pi.ph('requestDate'),
      trigger: 'change'
    }
  ],
  requestBy: [
    {
      required: true,
      message: pi.ph('requestBy'),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  supplierName1: [
    {
      required: true,
      message: pi.ph('supplierName1'),
      trigger: 'blur'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  taxRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseRequestItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */

/**
 * 税码变更时回填税率
 * @param {string | number | undefined} value 税码
 * @returns {void}
 */

/**
 * 区域文化变更：重选税码默认项并回填税率
 * @param {string | number | undefined} value 区域文化
 * @returns {void}
 */
function handleCultureCodeChange(value: string | number | undefined) {
  const culture = value == null ? '' : String(value)
  formState.cultureCode = culture
  const defaultTax = dictDataStore.getDictDefaultValue('accounting_tax_code', 'dictValue', culture)
  formState.taxCode = defaultTax == null ? '' : String(defaultTax)
  formState.taxRate = applyTaxRateFromTaxCode(formState.taxCode, formState.taxRate ?? 13)
}

function handleTaxCodeChange(value: string | number | undefined) {
  const code = value == null ? '' : String(value)
  formState.taxCode = code
  formState.taxRate = applyTaxRateFromTaxCode(code, formState.taxRate ?? 13)
}

function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('chainScheme' in payload) {
    const rawchainScheme = payload.chainScheme
    payload.chainScheme = typeof rawchainScheme === 'number' ? rawchainScheme : Number(rawchainScheme)
  }
  if ('poDecision' in payload) {
    const rawpoDecision = payload.poDecision
    payload.poDecision = typeof rawpoDecision === 'number' ? rawpoDecision : Number(rawpoDecision)
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    payload.taxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
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
  if ('requestStatus' in payload) {
    const rawrequestStatus = payload.requestStatus
    payload.requestStatus = typeof rawrequestStatus === 'number' ? rawrequestStatus : Number(rawrequestStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseRequestId)
  childPurchaseRequestItemRows.value = []
  purchaseRequestItemTableRef.value?.resetRows?.()
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
