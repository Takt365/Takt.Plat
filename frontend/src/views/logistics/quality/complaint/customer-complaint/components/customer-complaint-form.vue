<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint/components -->
<!-- 文件名称：customer-complaint-form.vue -->
<!-- 功能描述：客诉主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form customer-complaint-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="customer-complaint-form-tabs"
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
                :label="t('entity.customercomplaint.code')"
                name="customerComplaintCode"
              >
                <a-input
                  v-model:value="formState.customerComplaintCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerComplaintId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.customerid')"
                name="customerId"
              >
                <a-input
                  v-model:value="formState.customerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customerid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerComplaintId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.complaintdate')"
                name="complaintDate"
              >
                <a-date-picker
                  v-model:value="formState.complaintDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.complaintmethod')"
                name="complaintMethod"
              >
                <a-input-number
                  v-model:value="formState.complaintMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintmethod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.complainttype')"
                name="complaintType"
              >
                <a-input-number
                  v-model:value="formState.complaintType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complainttype') })"
                  style="width: 100%"
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
                :label="t('entity.customercomplaint.complaintlevel')"
                name="complaintLevel"
              >
                <a-input-number
                  v-model:value="formState.complaintLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintlevel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.responsibledeptid')"
                name="responsibleDeptId"
              >
                <a-input
                  v-model:value="formState.responsibleDeptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsibledeptid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.responsibledeptname')"
                name="responsibleDeptName"
              >
                <a-input
                  v-model:value="formState.responsibleDeptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsibledeptname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.responsiblepersonid')"
                name="responsiblePersonId"
              >
                <a-input
                  v-model:value="formState.responsiblePersonId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsiblepersonid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.responsiblepersonname')"
                name="responsiblePersonName"
              >
                <a-input
                  v-model:value="formState.responsiblePersonName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.responsiblepersonname') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.requiredreplydate')"
                name="requiredReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredReplyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.requiredreplydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.actualreplydate')"
                name="actualReplyDate"
              >
                <a-date-picker
                  v-model:value="formState.actualReplyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.actualreplydate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.complaintstatus')"
                name="complaintStatus"
              >
                <a-input-number
                  v-model:value="formState.complaintStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplaint.complaintdescription')"
                name="complaintDescription"
              >
                <a-textarea
                  v-model:value="formState.complaintDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customercomplaint.complaintdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customercomplaint.handlingresult')"
                name="handlingResult"
              >
                <a-input
                  v-model:value="formState.handlingResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.handlingresult') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
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
                :label="t('entity.customercomplaint.customersatisfaction')"
                name="customerSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.customerSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customersatisfaction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.customercomplaint.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.relatedplant') })"
                  show-count
                  :maxlength="4"
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
      ref="customerComplaintItemTableRef"
      v-model="childCustomerComplaintItemRows"
      :columns="customerComplaintItemFormColumns"
      :title="t('entity.customercomplaintitem._self')"
      :add-button-entity="t('entity.customercomplaintitem._self')"
      id-field="customerComplaintItemId"
      :default-row="createDefaultCustomerComplaintItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客诉主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerComplaintCreate } from '@/types/logistics/quality/complaint/customer-complaint'
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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","customerComplaintCode","customerId","customerName","customerCode","complaintDate","complaintMethod","complaintType","complaintLevel","responsibleDeptId","responsibleDeptName","responsiblePersonId","responsiblePersonName","requiredReplyDate","actualReplyDate","complaintStatus","complaintDescription","handlingResult","customerSatisfaction","relatedPlant","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childCustomerComplaintItemRows = ref<Record<string, unknown>[]>([])
const customerComplaintItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 customerComplaintItem 可编辑列 */
const customerComplaintItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.customercomplaintitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'productCode',
    title: t('entity.customercomplaintitem.productcode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.customercomplaintitem.productcode') }),
  },
  {
    key: 'productName',
    title: t('entity.customercomplaintitem.productname'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.customercomplaintitem.productname') }),
  },
  {
    key: 'batchNo',
    title: t('entity.customercomplaintitem.batchno'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.customercomplaintitem.batchno') }),
  },
  {
    key: 'itemType',
    title: t('entity.customercomplaintitem.itemtype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'defectDescription',
    title: t('entity.customercomplaintitem.defectdescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.required', { field: t('entity.customercomplaintitem.defectdescription') }),
    width: 140,
  },
  {
    key: 'defectLevel',
    title: t('entity.customercomplaintitem.defectlevel'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'defectQuantity',
    title: t('entity.customercomplaintitem.defectquantity'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null | undefined) {
  childCustomerComplaintItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultCustomerComplaintItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childCustomerComplaintItemRows.value.length + 1) * 10,
    productCode: '',
    productName: '',
    batchNo: '',
    itemType: 0,
    defectDescription: '',
    defectLevel: '',
    defectQuantity: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.customerComplaintId ?? ''
  return {
    ...formState,
    items: customerComplaintItemTableRef.value?.getRows?.() ?? childCustomerComplaintItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      complaintId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 customerComplaintId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerComplaintId) {
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
    const isCreate = !props.formData?.customerComplaintId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  customerComplaintCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.code') }),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customerid') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.customername') }),
      trigger: 'blur'
    }
  ],
  complaintDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintdate') }),
      trigger: 'change'
    }
  ],
  complaintMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complainttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complainttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintlevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintlevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.customercomplaint.complaintstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customercomplaint.complaintdescription') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await customerComplaintItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('complaintMethod' in payload) {
    const rawcomplaintMethod = payload.complaintMethod
    payload.complaintMethod = typeof rawcomplaintMethod === 'number' ? rawcomplaintMethod : Number(rawcomplaintMethod)
  }
  if ('complaintType' in payload) {
    const rawcomplaintType = payload.complaintType
    payload.complaintType = typeof rawcomplaintType === 'number' ? rawcomplaintType : Number(rawcomplaintType)
  }
  if ('complaintLevel' in payload) {
    const rawcomplaintLevel = payload.complaintLevel
    payload.complaintLevel = typeof rawcomplaintLevel === 'number' ? rawcomplaintLevel : Number(rawcomplaintLevel)
  }
  if ('complaintStatus' in payload) {
    const rawcomplaintStatus = payload.complaintStatus
    payload.complaintStatus = typeof rawcomplaintStatus === 'number' ? rawcomplaintStatus : Number(rawcomplaintStatus)
  }
  if ('customerSatisfaction' in payload) {
    const rawcustomerSatisfaction = payload.customerSatisfaction
    payload.customerSatisfaction = typeof rawcustomerSatisfaction === 'number' ? rawcustomerSatisfaction : Number(rawcustomerSatisfaction)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerComplaintId)
  childCustomerComplaintItemRows.value = []
  customerComplaintItemTableRef.value?.resetRows?.()
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
