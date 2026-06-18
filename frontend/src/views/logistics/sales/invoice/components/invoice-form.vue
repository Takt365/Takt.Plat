<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：sales-invoice-form.vue -->
<!-- 功能描述：Takt销售发票实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="sales-invoice-form-tabs"
    >
      <!-- 主表 -->
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
                :label="t('entity.salesInvoice.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.code')"
                name="salesInvoiceCode"
              >
                <a-input
                  v-model:value="formState.salesInvoiceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.salesordercode')"
                name="salesOrderCode"
              >
                <a-input
                  v-model:value="formState.salesOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.salesordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.customercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.customername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.invoicedate')"
                name="invoiceDate"
              >
                <a-date-picker
                  v-model:value="formState.invoiceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.invoicedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.totalamount') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.taxamount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.taxamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.actualamount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.actualamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.invoicestatus')"
                name="invoiceStatus"
              >
                <a-input-number
                  v-model:value="formState.invoiceStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.invoicestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.paymentmethod')"
                name="paymentMethod"
              >
                <a-input-number
                  v-model:value="formState.paymentMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.paymentmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesInvoice.taxinvoiceno')"
                name="taxInvoiceNo"
              >
                <a-input
                  v-model:value="formState.taxInvoiceNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.taxinvoiceno') })"
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
      <!-- 子表：salesInvoiceItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.salesInvoiceItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSalesInvoiceItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.salesInvoiceItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="salesInvoiceItemFormColumns"
          :data-source="childSalesInvoiceItemRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.materialcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialName'">
              <a-input
                v-model:value="record.materialName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.materialname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialSpecification'">
              <a-input
                v-model:value="record.materialSpecification"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.materialspecification') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'salesUnit'">
              <a-input
                v-model:value="record.salesUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.salesunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'invoiceQuantity'">
              <a-input-number
                v-model:value="record.invoiceQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.invoicequantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'unitPrice'">
              <a-input-number
                v-model:value="record.unitPrice"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.unitprice') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'discountRate'">
              <a-input-number
                v-model:value="record.discountRate"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesInvoiceItem.discountrate') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSalesInvoiceItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售发票实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SalesInvoiceCreate, SalesInvoiceItemCreate, SalesInvoiceItem } from '@/types/logistics/sales/sales-invoice'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","salesInvoiceCode","salesOrderCode","customerCode","customerName","invoiceDate","totalAmount","taxAmount","actualAmount","invoiceStatus","paymentMethod","taxInvoiceNo","extFieldJson","remark"]

/** salesInvoiceItem 子表行（表单 Tab 内嵌） */
const childSalesInvoiceItemRows = ref<Record<string, unknown>[]>([])

/** 子表 salesInvoiceItem 表单列定义 */
const salesInvoiceItemFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.salesunit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.invoicequantity'),
    dataIndex: 'invoiceQuantity',
    key: 'invoiceQuantity',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.unitprice'),
    dataIndex: 'unitPrice',
    key: 'unitPrice',
    width: 140,
  },
  {
    title: t('entity.salesInvoiceItem.discountrate'),
    dataIndex: 'discountRate',
    key: 'discountRate',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null | undefined) {
  childSalesInvoiceItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.salesInvoiceItemId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 salesInvoiceItem 行 */
function handleAddSalesInvoiceItemRow() {
  childSalesInvoiceItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      materialCode: '',
      materialName: '',
      materialSpecification: '',
      salesUnit: '',
      invoiceQuantity: 0,
      unitPrice: 0,
      discountRate: 0,
  })
}

/** 表单 Tab 内删除 salesInvoiceItem 行 */
function handleRemoveSalesInvoiceItemRow(index: number) {
  childSalesInvoiceItemRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childSalesInvoiceItemRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
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
  salesInvoiceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.code') }),
      trigger: 'blur'
    }
  ],
  customerCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.customercode') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesInvoice.customername') }),
      trigger: 'blur'
    }
  ],
  invoiceDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.invoicedate') }),
      trigger: 'change'
    }
  ],
  totalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.totalamount') }),
      trigger: 'change'
    }
  ],
  taxAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.taxamount') }),
      trigger: 'change'
    }
  ],
  actualAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.actualamount') }),
      trigger: 'change'
    }
  ],
  invoiceStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.invoicestatus') }),
      trigger: 'change'
    }
  ],
  paymentMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesInvoice.paymentmethod') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childSalesInvoiceItemRows.value = []
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
