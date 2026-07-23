<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/expense/components -->
<!-- 文件名称：expense-form.vue -->
<!-- 功能描述：费用单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form expense-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="expense-form-tabs"
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
                :label="pi.label('expenseCode')"
                name="expenseCode"
              >
                <a-input
                  v-model:value="formState.expenseCode"
                  :placeholder="pi.ph('expenseCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.expenseId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseTitle')"
                name="expenseTitle"
              >
                <a-input
                  v-model:value="formState.expenseTitle"
                  :placeholder="pi.ph('expenseTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseType')"
                name="expenseType"
              >
                <TaktSelect
                  v-model:value="formState.expenseType"
                  dict-type="accounting_expense_type"
                  :placeholder="pi.ph('expenseType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.expenseId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierName1')"
                name="supplierName1"
              >
                <a-input
                  v-model:value="formState.supplierName1"
                  :placeholder="pi.ph('supplierName1')"
                  show-count
                  :maxlength="140"
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
                :label="pi.label('costCenter')"
                name="costCenter"
              >
                <TaktSelect
                  v-model:value="formState.costCenter"
                  api-url="TaktCostCenters/tree-options"
                  :placeholder="pi.ph('costCenter')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countersignId')"
                name="countersignId"
              >
                <TaktSelect
                  v-model:value="formState.countersignId"
                  api-url="TaktCountersigns/options"
                  :placeholder="pi.ph('countersignId')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderCode')"
                name="purchaseOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.purchaseOrderCode"
                  api-url="TaktPurchaseOrders/options"
                  :placeholder="pi.ph('purchaseOrderCode')"
                  :disabled="!!formData?.expenseId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseRequestCode')"
                name="purchaseRequestCode"
              >
                <TaktSelect
                  v-model:value="formState.purchaseRequestCode"
                  api-url="TaktPurchaseRequests/options"
                  :placeholder="pi.ph('purchaseRequestCode')"
                  :disabled="!!formData?.expenseId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseAmount')"
                name="expenseAmount"
              >
                <a-input-number
                  v-model:value="formState.expenseAmount"
                  :placeholder="pi.ph('expenseAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseDate')"
                name="expenseDate"
              >
                <a-date-picker
                  v-model:value="formState.expenseDate"
                  :placeholder="pi.ph('expenseDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('relatedPlant')"
                name="relatedPlant"
              >
                <TaktSelect
                  v-model:value="formState.relatedPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('relatedPlant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expenseStatus')"
                name="expenseStatus"
              >
                <TaktSelect
                  v-model:value="formState.expenseStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('expenseStatus')"
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
    <!-- 下：子表 expenseDetails -->
    <TaktEditableTable
      ref="expenseDetailTableRef"
      v-model="childExpenseDetailRows"
      :columns="expenseDetailFormColumns"
      :title="expenseDetailPi.self()"
      :add-button-entity="expenseDetailPi.self()"
      id-field="expenseDetailId"
      :default-row="createDefaultExpenseDetailRow"
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
          :placeholder="expenseDetailPi.ph('allocationCategory')"
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
          :placeholder="expenseDetailPi.queryPh('accountTitle', 'select')"
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
          :placeholder="expenseDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 费用单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/expense/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useExpenseI18n } from '../composables/use-expense-i18n'

/** 实体字段 i18n */
const pi = useExpenseI18n()

import type { ExpenseCreate } from '@/types/accounting/financial/expense'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","expenseCode","expenseTitle","expenseType","supplierCode","supplierName1","applicantBy","applicationDept","costBearerDept","costCenter","countersignId","purchaseOrderCode","purchaseRequestCode","expenseAmount","taxRate","taxAmount","expenseDate","applicationReason","attachments","relatedPlant","expenseStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useExpenseDetailI18n } from '../composables/use-expense-detail-i18n'

const expenseDetailPi = useExpenseDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childExpenseDetailRows = ref<Record<string, unknown>[]>([])
const expenseDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedExpenseDetailRow(row: Record<string, unknown>): boolean {
  const id = row.expenseDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextExpenseDetailLineNumber(): number {
  const rows = expenseDetailTableRef.value?.getRows?.() ?? childExpenseDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 expenseDetail 可编辑列 */
const expenseDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: expenseDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'allocationCategory',
    title: expenseDetailPi.label('allocationCategory'),
    width: 140,
  },
  {
    key: 'itemName',
    title: expenseDetailPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: expenseDetailPi.label('itemDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: expenseDetailPi.ph('itemDescription'),
    width: 180,
  },
  {
    key: 'itemQuantity',
    title: expenseDetailPi.label('itemQuantity'),
    width: 140,
  },
  {
    key: 'itemAmount',
    title: expenseDetailPi.label('itemAmount'),
    width: 140,
  },
  {
    key: 'accountTitle',
    title: expenseDetailPi.label('accountTitle'),
    width: 140,
  },
  {
    key: 'invoiceNo',
    title: expenseDetailPi.label('invoiceNo'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: expenseDetailPi.ph('invoiceNo'),
  },
  {
    key: 'expenseDetailDate',
    title: expenseDetailPi.label('expenseDetailDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'isObsolete',
    title: expenseDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ExpenseCreate & { expenseId?: string }> | null | undefined) {
  const rows_expenseDetail = ((val as any)?.expenseDetails ?? []) as Record<string, unknown>[]
  childExpenseDetailRows.value = rows_expenseDetail
}

function createDefaultExpenseDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextExpenseDetailLineNumber(),
    allocationCategory: '',
    itemName: '',
    itemDescription: '',
    itemQuantity: 0,
    itemAmount: 0,
    accountTitle: '',
    invoiceNo: '',
    expenseDetailDate: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.expenseId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    expenseDetails: expenseDetailTableRef.value?.getRows?.() ?? childExpenseDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        expenseId: masterId,
      }
      if (isUpdate && isPersistedExpenseDetailRow(row)) {
        normalized.expenseDetailId = row.expenseDetailId
      } else {
        delete normalized.expenseDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ExpenseCreate & { expenseId?: string }> | null
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
  expenseType: 1,
  taxRate: 10,
  expenseStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 expenseId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.expenseId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).expenseDetails
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
    const isCreate = !props.formData?.expenseId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  expenseCode: [
    {
      required: true,
      message: pi.ph('expenseCode'),
      trigger: 'blur'
    }
  ],
  expenseTitle: [
    {
      required: true,
      message: pi.ph('expenseTitle'),
      trigger: 'blur'
    }
  ],
  expenseType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('expenseType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('expenseType'))
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
  expenseAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('expenseAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('expenseAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  expenseDate: [
    {
      required: true,
      message: pi.ph('expenseDate'),
      trigger: 'change'
    }
  ],
  relatedPlant: [
    {
      required: true,
      message: pi.ph('relatedPlant'),
      trigger: 'change'
    }
  ],
  expenseStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('expenseStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('expenseStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await expenseDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('expenseType' in payload) {
    const rawexpenseType = payload.expenseType
    payload.expenseType = typeof rawexpenseType === 'number' ? rawexpenseType : Number(rawexpenseType)
  }
  if ('expenseAmount' in payload) {
    const rawexpenseAmount = payload.expenseAmount
    payload.expenseAmount = typeof rawexpenseAmount === 'number' ? rawexpenseAmount : Number(rawexpenseAmount)
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    payload.taxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('expenseStatus' in payload) {
    const rawexpenseStatus = payload.expenseStatus
    payload.expenseStatus = typeof rawexpenseStatus === 'number' ? rawexpenseStatus : Number(rawexpenseStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.expenseId)
  childExpenseDetailRows.value = []
  expenseDetailTableRef.value?.resetRows?.()
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
