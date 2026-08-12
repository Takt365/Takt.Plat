<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-form.vue -->
<!-- 功能描述：Takt销售发票主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form invoice-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="invoice-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
      ref="salesInvoiceItemTableRef"
      v-model="childSalesInvoiceItemRows"
      :columns="salesInvoiceItemFormColumns"
      :title="salesInvoiceItemPi.self()"
      :add-button-entity="salesInvoiceItemPi.self()"
      id-field="salesInvoiceItemId"
      :default-row="createDefaultSalesInvoiceItemRow"
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
          :placeholder="salesInvoiceItemPi.queryPh('plantCode', 'select')"
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
          :placeholder="salesInvoiceItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-departureCountry="{ record }">
        <TaktSelect
          v-model:value="record.departureCountry"
          dict-type="sys_country_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('departureCountry')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-warehouseCode="{ record }">
        <TaktSelect
          v-model:value="record.warehouseCode"
          api-url="TaktWarehouses/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('warehouseCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-profitCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.profitCenterCode"
          api-url="TaktProfitCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('profitCenterCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-destinationCountryOrder="{ record }">
        <TaktSelect
          v-model:value="record.destinationCountryOrder"
          dict-type="sys_country_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('destinationCountryOrder')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-postedBy="{ record }">
        <TaktSelect
          v-model:value="record.postedBy"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('postedBy', 'select')"
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
          :placeholder="salesInvoiceItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售发票主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesInvoiceI18n } from '../composables/use-invoice-i18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceI18n()

import type { SalesInvoiceCreate } from '@/types/logistics/sales/invoice'
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
const formFields = ["tenantCode","companyCode","cultureCode","billingDocumentCode","billingType","billingCategory","documentCategory","currencyCode","salesOrganization","distributionChannel","pricingProcedure","conditionCode","shippingConditions","billingDate","customerGroup","incoterms1","incoterms2","postingStatus","accountingExchangeRate","paymentTerms","accountAssignmentGroup","countryCode","netAmount","payerCode","customerCode","statisticsCurrencyCode","foreignTradeCode","cancelledBillingDocument","invoiceListType","division","hierarchyTypePricing","tradingPartner","taxDepartureCountry","organizationSalesTaxNumber","countrySalesTaxNumber","referenceCode","cancelledFlag","exchangeRateDate","paymentReference","reversalReason","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useSalesInvoiceItemI18n } from '../composables/use-invoice-item-i18n'

const salesInvoiceItemPi = useSalesInvoiceItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSalesInvoiceItemRows = ref<Record<string, unknown>[]>([])
const salesInvoiceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSalesInvoiceItemRow(row: Record<string, unknown>): boolean {
  const id = row.salesInvoiceItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSalesInvoiceItemLineNumber(): number {
  const rows = salesInvoiceItemTableRef.value?.getRows?.() ?? childSalesInvoiceItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 salesInvoiceItem 可编辑列 */
const salesInvoiceItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'plantCode',
    title: salesInvoiceItemPi.label('plantCode'),
    width: 140,
  },
  {
    key: 'billingDocumentCode',
    title: salesInvoiceItemPi.label('billingDocumentCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: salesInvoiceItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'billingQuantity',
    title: salesInvoiceItemPi.label('billingQuantity'),
    width: 140,
  },
  {
    key: 'salesUnit',
    title: salesInvoiceItemPi.label('salesUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesUnit'),
  },
  {
    key: 'baseUnit',
    title: salesInvoiceItemPi.label('baseUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('baseUnit'),
  },
  {
    key: 'scaleQuantity',
    title: salesInvoiceItemPi.label('scaleQuantity'),
    width: 140,
  },
  {
    key: 'billingQuantitySku',
    title: salesInvoiceItemPi.label('billingQuantitySku'),
    width: 140,
  },
  {
    key: 'netWeight',
    title: salesInvoiceItemPi.label('netWeight'),
    width: 140,
  },
  {
    key: 'grossWeight',
    title: salesInvoiceItemPi.label('grossWeight'),
    width: 140,
  },
  {
    key: 'weightUnit',
    title: salesInvoiceItemPi.label('weightUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('weightUnit'),
  },
  {
    key: 'businessAreaCode',
    title: salesInvoiceItemPi.label('businessAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('businessAreaCode'),
  },
  {
    key: 'pricingDate',
    title: salesInvoiceItemPi.label('pricingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'serviceRenderedDate',
    title: salesInvoiceItemPi.label('serviceRenderedDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'pricingExchangeRate',
    title: salesInvoiceItemPi.label('pricingExchangeRate'),
    width: 140,
  },
  {
    key: 'netAmount',
    title: salesInvoiceItemPi.label('netAmount'),
    width: 140,
  },
  {
    key: 'referenceDocumentCode',
    title: salesInvoiceItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentItem',
    title: salesInvoiceItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'referenceDocumentCategory',
    title: salesInvoiceItemPi.label('referenceDocumentCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('referenceDocumentCategory'),
  },
  {
    key: 'salesDocumentCode',
    title: salesInvoiceItemPi.label('salesDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesDocumentCode'),
  },
  {
    key: 'salesDocumentItem',
    title: salesInvoiceItemPi.label('salesDocumentItem'),
    width: 140,
  },
  {
    key: 'salesDocumentReferenceFlag',
    title: salesInvoiceItemPi.label('salesDocumentReferenceFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesDocumentReferenceFlag'),
  },
  {
    key: 'materialCode',
    title: salesInvoiceItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialDescription',
    title: salesInvoiceItemPi.label('materialDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: salesInvoiceItemPi.ph('materialDescription'),
    width: 180,
  },
  {
    key: 'pricingReferenceMaterialCode',
    title: salesInvoiceItemPi.label('pricingReferenceMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('pricingReferenceMaterialCode'),
  },
  {
    key: 'batchCode',
    title: salesInvoiceItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('batchCode'),
  },
  {
    key: 'materialGroup',
    title: salesInvoiceItemPi.label('materialGroup'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('materialGroup'),
  },
  {
    key: 'salesItemCategory',
    title: salesInvoiceItemPi.label('salesItemCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesItemCategory'),
  },
  {
    key: 'productHierarchy',
    title: salesInvoiceItemPi.label('productHierarchy'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('productHierarchy'),
  },
  {
    key: 'shippingPoint',
    title: salesInvoiceItemPi.label('shippingPoint'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('shippingPoint'),
  },
  {
    key: 'division',
    title: salesInvoiceItemPi.label('division'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('division'),
  },
  {
    key: 'partnerItem',
    title: salesInvoiceItemPi.label('partnerItem'),
    width: 140,
  },
  {
    key: 'departureCountry',
    title: salesInvoiceItemPi.label('departureCountry'),
    width: 140,
  },
  {
    key: 'plantRegion',
    title: salesInvoiceItemPi.label('plantRegion'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('plantRegion'),
  },
  {
    key: 'pricingFlag',
    title: salesInvoiceItemPi.label('pricingFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('pricingFlag'),
  },
  {
    key: 'warehouseCode',
    title: salesInvoiceItemPi.label('warehouseCode'),
    width: 140,
  },
  {
    key: 'costAmount',
    title: salesInvoiceItemPi.label('costAmount'),
    width: 140,
  },
  {
    key: 'subtotal1',
    title: salesInvoiceItemPi.label('subtotal1'),
    width: 140,
  },
  {
    key: 'subtotal2',
    title: salesInvoiceItemPi.label('subtotal2'),
    width: 140,
  },
  {
    key: 'subtotal3',
    title: salesInvoiceItemPi.label('subtotal3'),
    width: 140,
  },
  {
    key: 'subtotal4',
    title: salesInvoiceItemPi.label('subtotal4'),
    width: 140,
  },
  {
    key: 'subtotal5',
    title: salesInvoiceItemPi.label('subtotal5'),
    width: 140,
  },
  {
    key: 'subtotal6',
    title: salesInvoiceItemPi.label('subtotal6'),
    width: 140,
  },
  {
    key: 'statisticsExchangeRate',
    title: salesInvoiceItemPi.label('statisticsExchangeRate'),
    width: 140,
  },
  {
    key: 'profitCenterCode',
    title: salesInvoiceItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'creditPrice',
    title: salesInvoiceItemPi.label('creditPrice'),
    width: 140,
  },
  {
    key: 'customerGroupSalesOrder',
    title: salesInvoiceItemPi.label('customerGroupSalesOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('customerGroupSalesOrder'),
  },
  {
    key: 'destinationCountryOrder',
    title: salesInvoiceItemPi.label('destinationCountryOrder'),
    width: 140,
  },
  {
    key: 'regionOrder',
    title: salesInvoiceItemPi.label('regionOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('regionOrder'),
  },
  {
    key: 'salesOrganizationOrder',
    title: salesInvoiceItemPi.label('salesOrganizationOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesOrganizationOrder'),
  },
  {
    key: 'distributionChannelOrder',
    title: salesInvoiceItemPi.label('distributionChannelOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('distributionChannelOrder'),
  },
  {
    key: 'documentCategory',
    title: salesInvoiceItemPi.label('documentCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('documentCategory'),
  },
  {
    key: 'taxAmount',
    title: salesInvoiceItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'grossAmount',
    title: salesInvoiceItemPi.label('grossAmount'),
    width: 140,
  },
  {
    key: 'exchangeRateDate',
    title: salesInvoiceItemPi.label('exchangeRateDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'postedBy',
    title: salesInvoiceItemPi.label('postedBy'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: salesInvoiceItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null | undefined) {
  const rows_salesInvoiceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSalesInvoiceItemRows.value = rows_salesInvoiceItem
}

function createDefaultSalesInvoiceItemRow(): Record<string, unknown> {
  return {
    plantCode: '',
    billingDocumentCode: '',
    lineNumber: allocateNextSalesInvoiceItemLineNumber(),
    billingQuantity: 0,
    salesUnit: '',
    baseUnit: '',
    scaleQuantity: 0,
    billingQuantitySku: 0,
    netWeight: 0,
    grossWeight: 0,
    weightUnit: '',
    businessAreaCode: '',
    pricingDate: '',
    serviceRenderedDate: '',
    pricingExchangeRate: 0,
    netAmount: 0,
    referenceDocumentCode: '',
    referenceDocumentItem: 0,
    referenceDocumentCategory: '',
    salesDocumentCode: '',
    salesDocumentItem: 0,
    salesDocumentReferenceFlag: '',
    materialCode: '',
    materialDescription: '',
    pricingReferenceMaterialCode: '',
    batchCode: '',
    materialGroup: '',
    salesItemCategory: '',
    productHierarchy: '',
    shippingPoint: '',
    division: '',
    partnerItem: 0,
    departureCountry: '',
    plantRegion: '',
    pricingFlag: '',
    warehouseCode: '',
    costAmount: 0,
    subtotal1: 0,
    subtotal2: 0,
    subtotal3: 0,
    subtotal4: 0,
    subtotal5: 0,
    subtotal6: 0,
    statisticsExchangeRate: 0,
    profitCenterCode: '',
    creditPrice: 0,
    customerGroupSalesOrder: '',
    destinationCountryOrder: '',
    regionOrder: '',
    salesOrganizationOrder: '',
    distributionChannelOrder: '',
    documentCategory: '',
    taxAmount: 0,
    grossAmount: 0,
    exchangeRateDate: '',
    postedBy: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesInvoiceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: salesInvoiceItemTableRef.value?.getRows?.() ?? childSalesInvoiceItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        salesInvoiceId: masterId,
      }
      if (isUpdate && isPersistedSalesInvoiceItemRow(row)) {
        normalized.salesInvoiceItemId = row.salesInvoiceItemId
      } else {
        delete normalized.salesInvoiceItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null
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
  shippingConditions: "Z1",
  countryCode: "CN",
  statisticsCurrencyCode: "CNY",
  taxDepartureCountry: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesInvoiceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesInvoiceId) {
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
    const isCreate = !props.formData?.salesInvoiceId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billingDocumentCode: [
    {
      required: true,
      message: pi.ph('billingDocumentCode'),
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
  billingDate: [
    {
      required: true,
      message: pi.ph('billingDate'),
      trigger: 'change'
    }
  ],
  netAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('netAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('netAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesInvoiceItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('accountingExchangeRate' in payload) {
    const rawaccountingExchangeRate = payload.accountingExchangeRate
    payload.accountingExchangeRate = typeof rawaccountingExchangeRate === 'number' ? rawaccountingExchangeRate : Number(rawaccountingExchangeRate)
  }
  if ('netAmount' in payload) {
    const rawnetAmount = payload.netAmount
    payload.netAmount = typeof rawnetAmount === 'number' ? rawnetAmount : Number(rawnetAmount)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesInvoiceId)
  childSalesInvoiceItemRows.value = []
  salesInvoiceItemTableRef.value?.resetRows?.()
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
