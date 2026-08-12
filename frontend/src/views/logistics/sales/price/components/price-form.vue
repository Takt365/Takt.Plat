<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price/components -->
<!-- 文件名称：price-form.vue -->
<!-- 功能描述：Takt销售价格实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form price-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="price-form-tabs"
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
      ref="salesPriceItemTableRef"
      v-model="childSalesPriceItemRows"
      :columns="salesPriceItemFormColumns"
      :title="salesPriceItemPi.self()"
      :add-button-entity="salesPriceItemPi.self()"
      id-field="salesPriceItemId"
      :default-row="createDefaultSalesPriceItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-priceType="{ record }">
        <TaktSelect
          v-model:value="record.priceType"
          dict-type="logistics_price_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('priceType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleType="{ record }">
        <TaktSelect
          v-model:value="record.scaleType"
          dict-type="logistics_scale_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('scaleType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleBasis="{ record }">
        <TaktSelect
          v-model:value="record.scaleBasis"
          dict-type="logistics_scale_basis"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('scaleBasis')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleUnit="{ record }">
        <TaktSelect
          v-model:value="record.scaleUnit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('scaleUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-scaleCurrencyCode="{ record }">
        <TaktSelect
          v-model:value="record.scaleCurrencyCode"
          dict-type="accounting_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('scaleCurrencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-calculationType="{ record }">
        <TaktSelect
          v-model:value="record.calculationType"
          dict-type="logistics_calculation_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('calculationType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-conditionCurrencyCode="{ record }">
        <TaktSelect
          v-model:value="record.conditionCurrencyCode"
          dict-type="accounting_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('conditionCurrencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-priceUnit="{ record }">
        <TaktSelect
          v-model:value="record.priceUnit"
          dict-type="logistics_price_unit_param"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('priceUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-unitOfMeasure="{ record }">
        <TaktSelect
          v-model:value="record.unitOfMeasure"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceItemPi.ph('unitOfMeasure')"
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
          :placeholder="salesPriceItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售价格实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/price/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesPriceI18n } from '../composables/use-price-i18n'

/** 实体字段 i18n */
const pi = useSalesPriceI18n()

import type { SalesPriceCreate } from '@/types/logistics/sales/price'
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
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","salesPriceCode","priceType","customerCode","materialCode","materialDescription","salesGroup","taxCode","grBasedInvoiceInspection","pricingDateControl","validFrom","validTo","salesQuotationId","salesQuotationCode","variableKey","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useSalesPriceItemI18n } from '../composables/use-price-item-i18n'

const salesPriceItemPi = useSalesPriceItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSalesPriceItemRows = ref<Record<string, unknown>[]>([])
const salesPriceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 salesPriceItem 可编辑列 */
const salesPriceItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'salesPriceSeq',
    title: salesPriceItemPi.label('salesPriceSeq'),
    width: 140,
  },
  {
    key: 'priceType',
    title: salesPriceItemPi.label('priceType'),
    width: 140,
  },
  {
    key: 'scaleType',
    title: salesPriceItemPi.label('scaleType'),
    width: 140,
  },
  {
    key: 'scaleBasis',
    title: salesPriceItemPi.label('scaleBasis'),
    width: 140,
  },
  {
    key: 'scaleQuantity',
    title: salesPriceItemPi.label('scaleQuantity'),
    width: 140,
  },
  {
    key: 'scaleUnit',
    title: salesPriceItemPi.label('scaleUnit'),
    width: 140,
  },
  {
    key: 'scaleValue',
    title: salesPriceItemPi.label('scaleValue'),
    width: 140,
  },
  {
    key: 'scaleCurrencyCode',
    title: salesPriceItemPi.label('scaleCurrencyCode'),
    width: 140,
  },
  {
    key: 'calculationType',
    title: salesPriceItemPi.label('calculationType'),
    width: 140,
  },
  {
    key: 'price',
    title: salesPriceItemPi.label('price'),
    width: 140,
  },
  {
    key: 'untaxedPrice',
    title: salesPriceItemPi.label('untaxedPrice'),
    width: 140,
  },
  {
    key: 'taxIncludedPrice',
    title: salesPriceItemPi.label('taxIncludedPrice'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: salesPriceItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'conditionCurrencyCode',
    title: salesPriceItemPi.label('conditionCurrencyCode'),
    width: 140,
  },
  {
    key: 'priceUnit',
    title: salesPriceItemPi.label('priceUnit'),
    width: 140,
  },
  {
    key: 'unitOfMeasure',
    title: salesPriceItemPi.label('unitOfMeasure'),
    width: 140,
  },
  {
    key: 'minOrderQuantity',
    title: salesPriceItemPi.label('minOrderQuantity'),
    width: 140,
  },
  {
    key: 'roundingValue',
    title: salesPriceItemPi.label('roundingValue'),
    width: 140,
  },
  {
    key: 'plannedDeliveryTimeDays',
    title: salesPriceItemPi.label('plannedDeliveryTimeDays'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: salesPriceItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'scaleQuantities',
    title: salesPriceItemPi.label('scaleQuantities'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesPriceItemPi.ph('scaleQuantities'),
  },
  {
    key: 'scaleValues',
    title: salesPriceItemPi.label('scaleValues'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesPriceItemPi.ph('scaleValues'),
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesPriceCreate & { salesPriceId?: string }> | null | undefined) {
  const rows_salesPriceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSalesPriceItemRows.value = rows_salesPriceItem
}

function createDefaultSalesPriceItemRow(): Record<string, unknown> {
  return {
    salesPriceSeq: 0,
    priceType: '',
    scaleType: '',
    scaleBasis: '',
    scaleQuantity: 0,
    scaleUnit: '',
    scaleValue: 0,
    scaleCurrencyCode: '',
    calculationType: '',
    price: 0,
    untaxedPrice: 0,
    taxIncludedPrice: 0,
    taxAmount: 0,
    conditionCurrencyCode: '',
    priceUnit: 0,
    unitOfMeasure: '',
    minOrderQuantity: 0,
    roundingValue: 0,
    plannedDeliveryTimeDays: 0,
    isObsolete: 0,
    scaleQuantities: '',
    scaleValues: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesPriceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: salesPriceItemTableRef.value?.getRows?.() ?? childSalesPriceItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      salesPriceId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesPriceCreate & { salesPriceId?: string }> | null
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
  priceType: "PB00",
  taxCode: "J2"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesPriceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesPriceId) {
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
    const isCreate = !props.formData?.salesPriceId
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
  salesPriceCode: [
    {
      required: true,
      message: pi.ph('salesPriceCode'),
      trigger: 'blur'
    }
  ],
  priceType: [
    {
      required: true,
      message: pi.ph('priceType'),
      trigger: 'change'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  grBasedInvoiceInspection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pricingDateControl: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  validFrom: [
    {
      required: true,
      message: pi.ph('validFrom'),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: pi.ph('validTo'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesPriceItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('grBasedInvoiceInspection' in payload) {
    const rawgrBasedInvoiceInspection = payload.grBasedInvoiceInspection
    payload.grBasedInvoiceInspection = typeof rawgrBasedInvoiceInspection === 'number' ? rawgrBasedInvoiceInspection : Number(rawgrBasedInvoiceInspection)
  }
  if ('pricingDateControl' in payload) {
    const rawpricingDateControl = payload.pricingDateControl
    payload.pricingDateControl = typeof rawpricingDateControl === 'number' ? rawpricingDateControl : Number(rawpricingDateControl)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesPriceId)
  childSalesPriceItemRows.value = []
  salesPriceItemTableRef.value?.resetRows?.()
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
