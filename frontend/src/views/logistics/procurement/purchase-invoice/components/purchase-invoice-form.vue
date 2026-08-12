<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-invoice/components -->
<!-- 文件名称：purchase-invoice-form.vue -->
<!-- 功能描述：Takt采购发票主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-invoice-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-invoice-form-tabs"
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
      ref="purchaseInvoiceItemTableRef"
      v-model="childPurchaseInvoiceItemRows"
      :columns="purchaseInvoiceItemFormColumns"
      :title="purchaseInvoiceItemPi.self()"
      :add-button-entity="purchaseInvoiceItemPi.self()"
      id-field="purchaseInvoiceItemId"
      :default-row="createDefaultPurchaseInvoiceItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-plantCode="{ record }">
        <TaktSelect
          v-model:value="record.plantCode"
          api-url="TaktPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseInvoiceItemPi.queryPh('plantCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-purchaseOrderCode="{ record }">
        <TaktSelect
          v-model:value="record.purchaseOrderCode"
          api-url="TaktPurchaseOrders/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="purchaseInvoiceItemPi.queryPh('purchaseOrderCode', 'select')"
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
          :placeholder="purchaseInvoiceItemPi.queryPh('materialCode', 'select')"
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
          :placeholder="purchaseInvoiceItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt采购发票主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-invoice/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseInvoiceI18n } from '../composables/use-purchase-invoice-i18n'

/** 实体字段 i18n */
const pi = usePurchaseInvoiceI18n()

import type { PurchaseInvoiceCreate } from '@/types/logistics/procurement/purchase-invoice'
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
const formFields = ["tenantCode","companyCode","cultureCode","purchaseInvoiceCode","fiscalYear","documentType","documentDate","postingDate","transactionEventType","referenceCode","supplierCode","currencyCode","exchangeRate","grossAmount","vatAmount","taxJurisdictionCode","cashDiscountDays1","invoiceFlag","headerText","reversalDocumentCode","reversalFiscalYear","taxCode","supplyingCountry","taxExchangeRate","baselineDate","enteredBy","exchangeRateDate","transactionCode","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { usePurchaseInvoiceItemI18n } from '../composables/use-purchase-invoice-item-i18n'

const purchaseInvoiceItemPi = usePurchaseInvoiceItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPurchaseInvoiceItemRows = ref<Record<string, unknown>[]>([])
const purchaseInvoiceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPurchaseInvoiceItemRow(row: Record<string, unknown>): boolean {
  const id = row.purchaseInvoiceItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPurchaseInvoiceItemLineNumber(): number {
  const rows = purchaseInvoiceItemTableRef.value?.getRows?.() ?? childPurchaseInvoiceItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 purchaseInvoiceItem 可编辑列 */
const purchaseInvoiceItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'plantCode',
    title: purchaseInvoiceItemPi.label('plantCode'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: purchaseInvoiceItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'purchaseOrderCode',
    title: purchaseInvoiceItemPi.label('purchaseOrderCode'),
    width: 140,
  },
  {
    key: 'purchaseOrderItem',
    title: purchaseInvoiceItemPi.label('purchaseOrderItem'),
    width: 140,
  },
  {
    key: 'accountAssignmentSeq',
    title: purchaseInvoiceItemPi.label('accountAssignmentSeq'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('accountAssignmentSeq'),
  },
  {
    key: 'materialCode',
    title: purchaseInvoiceItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'valuationArea',
    title: purchaseInvoiceItemPi.label('valuationArea'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valuationArea'),
  },
  {
    key: 'amount',
    title: purchaseInvoiceItemPi.label('amount'),
    width: 140,
  },
  {
    key: 'debitCreditIndicator',
    title: purchaseInvoiceItemPi.label('debitCreditIndicator'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('debitCreditIndicator'),
  },
  {
    key: 'taxCode',
    title: purchaseInvoiceItemPi.label('taxCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('taxCode'),
  },
  {
    key: 'quantity',
    title: purchaseInvoiceItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'orderUnit',
    title: purchaseInvoiceItemPi.label('orderUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('orderUnit'),
  },
  {
    key: 'poPriceQuantity',
    title: purchaseInvoiceItemPi.label('poPriceQuantity'),
    width: 140,
  },
  {
    key: 'poPriceUnit',
    title: purchaseInvoiceItemPi.label('poPriceUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('poPriceUnit'),
  },
  {
    key: 'valuatedStockQuantity',
    title: purchaseInvoiceItemPi.label('valuatedStockQuantity'),
    width: 140,
  },
  {
    key: 'previousPeriodStock',
    title: purchaseInvoiceItemPi.label('previousPeriodStock'),
    width: 140,
  },
  {
    key: 'baseUnit',
    title: purchaseInvoiceItemPi.label('baseUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('baseUnit'),
  },
  {
    key: 'valuationClass',
    title: purchaseInvoiceItemPi.label('valuationClass'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valuationClass'),
  },
  {
    key: 'updatePoHistoryFlag',
    title: purchaseInvoiceItemPi.label('updatePoHistoryFlag'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'subsequentDebitCredit',
    title: purchaseInvoiceItemPi.label('subsequentDebitCredit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('subsequentDebitCredit'),
  },
  {
    key: 'blockReasonPrice',
    title: purchaseInvoiceItemPi.label('blockReasonPrice'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonPrice'),
  },
  {
    key: 'blockReasonQuantity',
    title: purchaseInvoiceItemPi.label('blockReasonQuantity'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonQuantity'),
  },
  {
    key: 'blockReasonQuality',
    title: purchaseInvoiceItemPi.label('blockReasonQuality'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonQuality'),
  },
  {
    key: 'blockReasonEnhanced',
    title: purchaseInvoiceItemPi.label('blockReasonEnhanced'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('blockReasonEnhanced'),
  },
  {
    key: 'valueString',
    title: purchaseInvoiceItemPi.label('valueString'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('valueString'),
  },
  {
    key: 'referenceCode',
    title: purchaseInvoiceItemPi.label('referenceCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceCode'),
  },
  {
    key: 'conditionType',
    title: purchaseInvoiceItemPi.label('conditionType'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('conditionType'),
  },
  {
    key: 'totalValuatedStockValue',
    title: purchaseInvoiceItemPi.label('totalValuatedStockValue'),
    width: 140,
  },
  {
    key: 'previousPeriodValue',
    title: purchaseInvoiceItemPi.label('previousPeriodValue'),
    width: 140,
  },
  {
    key: 'referenceDocumentCode',
    title: purchaseInvoiceItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentYear',
    title: purchaseInvoiceItemPi.label('referenceDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('referenceDocumentYear'),
  },
  {
    key: 'referenceDocumentItem',
    title: purchaseInvoiceItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'stockManagedMaterialCode',
    title: purchaseInvoiceItemPi.label('stockManagedMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('stockManagedMaterialCode'),
  },
  {
    key: 'itemText',
    title: purchaseInvoiceItemPi.label('itemText'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: purchaseInvoiceItemPi.ph('itemText'),
  },
  {
    key: 'materialDocumentItem',
    title: purchaseInvoiceItemPi.label('materialDocumentItem'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: purchaseInvoiceItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseInvoiceCreate & { purchaseInvoiceId?: string }> | null | undefined) {
  const rows_purchaseInvoiceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childPurchaseInvoiceItemRows.value = rows_purchaseInvoiceItem
}

function createDefaultPurchaseInvoiceItemRow(): Record<string, unknown> {
  return {
    plantCode: '',
    lineNumber: allocateNextPurchaseInvoiceItemLineNumber(),
    purchaseOrderCode: '',
    purchaseOrderItem: 0,
    accountAssignmentSeq: '',
    materialCode: '',
    valuationArea: '',
    amount: 0,
    debitCreditIndicator: '',
    taxCode: '',
    quantity: 0,
    orderUnit: '',
    poPriceQuantity: 0,
    poPriceUnit: '',
    valuatedStockQuantity: 0,
    previousPeriodStock: 0,
    baseUnit: '',
    valuationClass: '',
    updatePoHistoryFlag: '',
    subsequentDebitCredit: '',
    blockReasonPrice: '',
    blockReasonQuantity: '',
    blockReasonQuality: '',
    blockReasonEnhanced: '',
    valueString: '',
    referenceCode: '',
    conditionType: '',
    totalValuatedStockValue: 0,
    previousPeriodValue: 0,
    referenceDocumentCode: '',
    referenceDocumentYear: '',
    referenceDocumentItem: 0,
    stockManagedMaterialCode: '',
    itemText: '',
    materialDocumentItem: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseInvoiceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: purchaseInvoiceItemTableRef.value?.getRows?.() ?? childPurchaseInvoiceItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        purchaseInvoiceId: masterId,
      }
      if (isUpdate && isPersistedPurchaseInvoiceItemRow(row)) {
        normalized.purchaseInvoiceItemId = row.purchaseInvoiceItemId
      } else {
        delete normalized.purchaseInvoiceItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInvoiceCreate & { purchaseInvoiceId?: string }> | null
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
  currencyCode: "CNY",
  supplyingCountry: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInvoiceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInvoiceId) {
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
    const isCreate = !props.formData?.purchaseInvoiceId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  purchaseInvoiceCode: [
    {
      required: true,
      message: pi.ph('purchaseInvoiceCode'),
      trigger: 'blur'
    }
  ],
  fiscalYear: [
    {
      required: true,
      message: pi.ph('fiscalYear'),
      trigger: 'blur'
    }
  ],
  documentDate: [
    {
      required: true,
      message: pi.ph('documentDate'),
      trigger: 'change'
    }
  ],
  postingDate: [
    {
      required: true,
      message: pi.ph('postingDate'),
      trigger: 'change'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  grossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grossAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grossAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseInvoiceItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('exchangeRate' in payload) {
    const rawexchangeRate = payload.exchangeRate
    payload.exchangeRate = typeof rawexchangeRate === 'number' ? rawexchangeRate : Number(rawexchangeRate)
  }
  if ('grossAmount' in payload) {
    const rawgrossAmount = payload.grossAmount
    payload.grossAmount = typeof rawgrossAmount === 'number' ? rawgrossAmount : Number(rawgrossAmount)
  }
  if ('vatAmount' in payload) {
    const rawvatAmount = payload.vatAmount
    payload.vatAmount = typeof rawvatAmount === 'number' ? rawvatAmount : Number(rawvatAmount)
  }
  if ('cashDiscountDays1' in payload) {
    const rawcashDiscountDays1 = payload.cashDiscountDays1
    payload.cashDiscountDays1 = typeof rawcashDiscountDays1 === 'number' ? rawcashDiscountDays1 : Number(rawcashDiscountDays1)
  }
  if ('taxExchangeRate' in payload) {
    const rawtaxExchangeRate = payload.taxExchangeRate
    payload.taxExchangeRate = typeof rawtaxExchangeRate === 'number' ? rawtaxExchangeRate : Number(rawtaxExchangeRate)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseInvoiceId)
  childPurchaseInvoiceItemRows.value = []
  purchaseInvoiceItemTableRef.value?.resetRows?.()
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
