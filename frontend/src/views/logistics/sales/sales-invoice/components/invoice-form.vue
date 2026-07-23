<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-form.vue -->
<!-- 功能描述：Takt销售发票实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('yearMonth')"
                name="yearMonth"
              >
                <a-input
                  v-model:value="formState.yearMonth"
                  :placeholder="pi.ph('yearMonth')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('customerName1')"
                name="customerName1"
              >
                <a-input
                  v-model:value="formState.customerName1"
                  :placeholder="pi.ph('customerName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxRate')"
                name="taxRate"
              >
                <TaktSelect
                  v-model:value="formState.taxRate"
                  dict-type="accounting_tax_rate_param"
                  :placeholder="pi.ph('taxRate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('accountingDocumentCode')"
                name="accountingDocumentCode"
              >
                <a-input
                  v-model:value="formState.accountingDocumentCode"
                  :placeholder="pi.ph('accountingDocumentCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceId"
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
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
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
      <template #cell-materialType="{ record }">
        <TaktSelect
          v-model:value="record.materialType"
          dict-type="logistics_material_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('materialType')"
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
      <template #cell-accountTitle="{ record }">
        <TaktSelect
          v-model:value="record.accountTitle"
          api-url="TaktAccountTitles/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('accountTitle', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-unit="{ record }">
        <TaktSelect
          v-model:value="record.unit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('unit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-documentType="{ record }">
        <TaktSelect
          v-model:value="record.documentType"
          dict-type="logistics_accounting_document_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('documentType')"
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
 * Takt销售发票实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","yearMonth","customerCode","customerName1","currencyCode","taxRate","taxAmount","accountingDocumentCode","extField","remark"]


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
    key: 'accountingDocumentCode',
    title: salesInvoiceItemPi.label('accountingDocumentCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: salesInvoiceItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'postingDate',
    title: salesInvoiceItemPi.label('postingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'modelName',
    title: salesInvoiceItemPi.label('modelName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('modelName'),
  },
  {
    key: 'materialCode',
    title: salesInvoiceItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialType',
    title: salesInvoiceItemPi.label('materialType'),
    width: 140,
  },
  {
    key: 'profitCenterCode',
    title: salesInvoiceItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'accountTitle',
    title: salesInvoiceItemPi.label('accountTitle'),
    width: 140,
  },
  {
    key: 'quantity',
    title: salesInvoiceItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'unit',
    title: salesInvoiceItemPi.label('unit'),
    width: 140,
  },
  {
    key: 'localCurrencyAmount',
    title: salesInvoiceItemPi.label('localCurrencyAmount'),
    width: 140,
  },
  {
    key: 'transactionCurrencyAmount',
    title: salesInvoiceItemPi.label('transactionCurrencyAmount'),
    width: 140,
  },
  {
    key: 'taxIncludedPrice',
    title: salesInvoiceItemPi.label('taxIncludedPrice'),
    width: 140,
  },
  {
    key: 'untaxedPrice',
    title: salesInvoiceItemPi.label('untaxedPrice'),
    width: 140,
  },
  {
    key: 'taxAmount',
    title: salesInvoiceItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'documentType',
    title: salesInvoiceItemPi.label('documentType'),
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
    key: 'isObsolete',
    title: salesInvoiceItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null | undefined) {
  const rows_salesInvoiceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSalesInvoiceItemRows.value = rows_salesInvoiceItem
}

function createDefaultSalesInvoiceItemRow(): Record<string, unknown> {
  return {
    accountingDocumentCode: '',
    lineNumber: allocateNextSalesInvoiceItemLineNumber(),
    postingDate: '',
    modelName: '',
    materialCode: '',
    materialType: '',
    profitCenterCode: '',
    accountTitle: '',
    quantity: 0,
    unit: '',
    localCurrencyAmount: 0,
    transactionCurrencyAmount: 0,
    taxIncludedPrice: 0,
    untaxedPrice: 0,
    taxAmount: 0,
    documentType: '',
    referenceDocumentCode: '',
    referenceDocumentItem: 0,
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
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
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
  taxRate: 10
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
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  yearMonth: [
    {
      required: true,
      message: pi.ph('yearMonth'),
      trigger: 'blur'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
  customerName1: [
    {
      required: true,
      message: pi.ph('customerName1'),
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
  accountingDocumentCode: [
    {
      required: true,
      message: pi.ph('accountingDocumentCode'),
      trigger: 'blur'
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
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    payload.taxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
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
