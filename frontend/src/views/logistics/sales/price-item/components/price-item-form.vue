<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/price-item/components -->
<!-- 文件名称：price-item-form.vue -->
<!-- 功能描述：Takt销售价格明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form price-item-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="price-item-form-tabs"
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
                :label="pi.label('salesPriceId')"
                name="salesPriceId"
              >
                <TaktSelect
                  v-model:value="formState.salesPriceId"
                  api-url="TaktSalesPrices/options"
                  :placeholder="pi.ph('salesPriceId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesPriceCode')"
                name="salesPriceCode"
              >
                <a-input
                  v-model:value="formState.salesPriceCode"
                  :placeholder="pi.ph('salesPriceCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesPriceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesPriceSeq')"
                name="salesPriceSeq"
              >
                <a-input-number
                  v-model:value="formState.salesPriceSeq"
                  :placeholder="pi.ph('salesPriceSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceType')"
                name="priceType"
              >
                <TaktSelect
                  v-model:value="formState.priceType"
                  dict-type="logistics_price_type"
                  :placeholder="pi.ph('priceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleType')"
                name="scaleType"
              >
                <TaktSelect
                  v-model:value="formState.scaleType"
                  dict-type="logistics_scale_type"
                  :placeholder="pi.ph('scaleType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleBasis')"
                name="scaleBasis"
              >
                <TaktSelect
                  v-model:value="formState.scaleBasis"
                  dict-type="logistics_scale_basis"
                  :placeholder="pi.ph('scaleBasis')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantity')"
                name="scaleQuantity"
              >
                <a-input-number
                  v-model:value="formState.scaleQuantity"
                  :placeholder="pi.ph('scaleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleUnit')"
                name="scaleUnit"
              >
                <TaktSelect
                  v-model:value="formState.scaleUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('scaleUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleValue')"
                name="scaleValue"
              >
                <a-input-number
                  v-model:value="formState.scaleValue"
                  :placeholder="pi.ph('scaleValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleCurrency')"
                name="scaleCurrency"
              >
                <TaktSelect
                  v-model:value="formState.scaleCurrency"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('scaleCurrency')"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('calculationType')"
                name="calculationType"
              >
                <TaktSelect
                  v-model:value="formState.calculationType"
                  dict-type="logistics_calculation_type"
                  :placeholder="pi.ph('calculationType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('price')"
                name="price"
              >
                <a-input-number
                  v-model:value="formState.price"
                  :placeholder="pi.ph('price')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <TaktSelect
                  v-model:value="formState.taxCode"
                  dict-type="accounting_tax_code"
                  :placeholder="pi.ph('taxCode')"
                  :disabled="!!formData?.salesPriceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isObsolete')"
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
    <!-- 下：子表 scaleQuantities -->
    <TaktEditableTable
      ref="salesPriceScaleQuantityTableRef"
      v-model="childSalesPriceScaleQuantityRows"
      :columns="salesPriceScaleQuantityFormColumns"
      :title="salesPriceScaleQuantityPi.self()"
      :add-button-entity="salesPriceScaleQuantityPi.self()"
      id-field="salesPriceScaleQuantityId"
      :default-row="createDefaultSalesPriceScaleQuantityRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesPriceScaleQuantityPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售价格明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/price-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesPriceItemI18n } from '../composables/use-price-item-i18n'

/** 实体字段 i18n */
const pi = useSalesPriceItemI18n()

import type { SalesPriceItemCreate } from '@/types/logistics/sales/price-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","salesPriceId","salesPriceCode","salesPriceSeq","priceType","scaleType","scaleBasis","scaleQuantity","scaleUnit","scaleValue","scaleCurrency","calculationType","price","taxCode","isObsolete","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useSalesPriceScaleQuantityI18n } from '../composables/use-price-scale-quantity-i18n'

const salesPriceScaleQuantityPi = useSalesPriceScaleQuantityI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSalesPriceScaleQuantityRows = ref<Record<string, unknown>[]>([])
const salesPriceScaleQuantityTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSalesPriceScaleQuantityRow(row: Record<string, unknown>): boolean {
  const id = row.salesPriceScaleQuantityId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSalesPriceScaleQuantityLineNumber(): number {
  const rows = salesPriceScaleQuantityTableRef.value?.getRows?.() ?? childSalesPriceScaleQuantityRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 salesPriceScaleQuantity 可编辑列 */
const salesPriceScaleQuantityFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'salesPriceCode',
    title: salesPriceScaleQuantityPi.label('salesPriceCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'salesPriceSeq',
    title: salesPriceScaleQuantityPi.label('salesPriceSeq'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: salesPriceScaleQuantityPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'scaleQuantity',
    title: salesPriceScaleQuantityPi.label('scaleQuantity'),
    width: 140,
  },
  {
    key: 'amount',
    title: salesPriceScaleQuantityPi.label('amount'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: salesPriceScaleQuantityPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesPriceItemCreate & { salesPriceItemId?: string }> | null | undefined) {
  const rows_salesPriceScaleQuantity = ((val as any)?.scaleQuantities ?? []) as Record<string, unknown>[]
  childSalesPriceScaleQuantityRows.value = rows_salesPriceScaleQuantity
}

function createDefaultSalesPriceScaleQuantityRow(): Record<string, unknown> {
  return {
    salesPriceCode: '',
    salesPriceSeq: 0,
    lineNumber: allocateNextSalesPriceScaleQuantityLineNumber(),
    scaleQuantity: 0,
    amount: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesPriceItemId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    scaleQuantities: salesPriceScaleQuantityTableRef.value?.getRows?.() ?? childSalesPriceScaleQuantityRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        salesPriceItemId: masterId,
      }
      if (isUpdate && isPersistedSalesPriceScaleQuantityRow(row)) {
        normalized.salesPriceScaleQuantityId = row.salesPriceScaleQuantityId
      } else {
        delete normalized.salesPriceScaleQuantityId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesPriceItemCreate & { salesPriceItemId?: string }> | null
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
  scaleType: "A",
  scaleBasis: "C",
  scaleCurrency: "CNY",
  calculationType: "A",
  taxCode: "J1"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesPriceItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesPriceItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).scaleQuantities
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
    const isCreate = !props.formData?.salesPriceItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  salesPriceId: [
    {
      required: true,
      message: pi.ph('salesPriceId'),
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
  salesPriceSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('salesPriceSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('salesPriceSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priceType: [
    {
      required: true,
      message: pi.ph('priceType'),
      trigger: 'change'
    }
  ],
  scaleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scaleValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scaleValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scaleValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  calculationType: [
    {
      required: true,
      message: pi.ph('calculationType'),
      trigger: 'change'
    }
  ],
  price: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('price'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('price'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesPriceScaleQuantityTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('salesPriceSeq' in payload) {
    const rawsalesPriceSeq = payload.salesPriceSeq
    payload.salesPriceSeq = typeof rawsalesPriceSeq === 'number' ? rawsalesPriceSeq : Number(rawsalesPriceSeq)
  }
  if ('scaleQuantity' in payload) {
    const rawscaleQuantity = payload.scaleQuantity
    payload.scaleQuantity = typeof rawscaleQuantity === 'number' ? rawscaleQuantity : Number(rawscaleQuantity)
  }
  if ('scaleValue' in payload) {
    const rawscaleValue = payload.scaleValue
    payload.scaleValue = typeof rawscaleValue === 'number' ? rawscaleValue : Number(rawscaleValue)
  }
  if ('price' in payload) {
    const rawprice = payload.price
    payload.price = typeof rawprice === 'number' ? rawprice : Number(rawprice)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesPriceItemId)
  childSalesPriceScaleQuantityRows.value = []
  salesPriceScaleQuantityTableRef.value?.resetRows?.()
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
