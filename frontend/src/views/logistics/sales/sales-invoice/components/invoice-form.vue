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
    class="takt-generated-form invoice-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerName')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="pi.ph('customerName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
      section-border
    />
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
import { useSalesInvoiceI18n } from '../composables/use-invoice-i18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceI18n()

import type { SalesInvoiceCreate } from '@/types/logistics/sales/invoice'
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
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","yearMonth","customerCode","customerName","accountingDocumentCode","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useSalesInvoiceItemI18n } from '../composables/use-invoice-item-i18n'

const salesInvoiceItemPi = useSalesInvoiceItemI18n()

const childSalesInvoiceItemRows = ref<Record<string, unknown>[]>([])
const salesInvoiceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

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
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'postingDate',
    title: salesInvoiceItemPi.label('postingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'currency',
    title: salesInvoiceItemPi.label('currency'),
    editor: 'input',
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
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialType',
    title: salesInvoiceItemPi.label('materialType'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: salesInvoiceItemPi.label('materialName'),
    editor: 'input',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null | undefined) {
  childSalesInvoiceItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultSalesInvoiceItemRow(): Record<string, unknown> {
  return {
    accountingDocumentCode: '',
    lineNumber: (childSalesInvoiceItemRows.value.length + 1) * 10,
    postingDate: '',
    currency: '',
    modelName: '',
    materialCode: '',
    materialType: '',
    materialName: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesInvoiceId ?? ''
  return {
    ...formState,
    items: salesInvoiceItemTableRef.value?.getRows?.() ?? childSalesInvoiceItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      salesInvoiceId: masterId,
    })),
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


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
  customerName: [
    {
      required: true,
      message: pi.ph('customerName'),
      trigger: 'blur'
    }
  ],
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
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

