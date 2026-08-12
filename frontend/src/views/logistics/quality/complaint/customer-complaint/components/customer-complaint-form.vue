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
    class="takt-generated-form customer-complaint-form flex flex-col min-h-0 overflow-visible"
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
      ref="customerComplaintItemTableRef"
      v-model="childCustomerComplaintItemRows"
      :columns="customerComplaintItemFormColumns"
      :title="customerComplaintItemPi.self()"
      :add-button-entity="customerComplaintItemPi.self()"
      id-field="customerComplaintItemId"
      :default-row="createDefaultCustomerComplaintItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-complaintId="{ record }">
        <TaktSelect
          v-model:value="record.complaintId"
          api-url="TaktCustomerComplaints/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.queryPh('complaintId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-productCode="{ record }">
        <TaktSelect
          v-model:value="record.productCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.queryPh('productCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-itemType="{ record }">
        <TaktSelect
          v-model:value="record.itemType"
          dict-type="logistics_quality_complaint_item_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('itemType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectLevel="{ record }">
        <TaktSelect
          v-model:value="record.defectLevel"
          dict-type="logistics_quality_defect_severity_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('defectLevel')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-improvementResponsible="{ record }">
        <TaktSelect
          v-model:value="record.improvementResponsible"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.queryPh('improvementResponsible', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-improvementStatus="{ record }">
        <TaktSelect
          v-model:value="record.improvementStatus"
          dict-type="logistics_quality_improvement_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="customerComplaintItemPi.ph('improvementStatus')"
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
          :placeholder="customerComplaintItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客诉主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerComplaintI18n } from '../composables/use-customer-complaint-i18n'

/** 实体字段 i18n */
const pi = useCustomerComplaintI18n()

import type { CustomerComplaintCreate } from '@/types/logistics/quality/complaint/customer-complaint'
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
const formFields = ["tenantCode","companyCode","cultureCode","customerComplaintCode","customerId","customerName1","customerCode","complaintDate","complaintMethod","complaintType","complaintLevel","responsibleDeptId","responsibleDeptName","responsiblePersonId","responsiblePersonName","requiredReplyDate","actualReplyDate","complaintDescription","handlingResult","customerSatisfaction","attachments","plantCode","complaintStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useCustomerComplaintItemI18n } from '../composables/use-customer-complaint-item-i18n'

const customerComplaintItemPi = useCustomerComplaintItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childCustomerComplaintItemRows = ref<Record<string, unknown>[]>([])
const customerComplaintItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedCustomerComplaintItemRow(row: Record<string, unknown>): boolean {
  const id = row.customerComplaintItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextCustomerComplaintItemLineNumber(): number {
  const rows = customerComplaintItemTableRef.value?.getRows?.() ?? childCustomerComplaintItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 customerComplaintItem 可编辑列 */
const customerComplaintItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'complaintId',
    title: customerComplaintItemPi.label('complaintId'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: customerComplaintItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'productCode',
    title: customerComplaintItemPi.label('productCode'),
    width: 140,
  },
  {
    key: 'productName',
    title: customerComplaintItemPi.label('productName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('productName'),
  },
  {
    key: 'batchCode',
    title: customerComplaintItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('batchCode'),
  },
  {
    key: 'itemType',
    title: customerComplaintItemPi.label('itemType'),
    width: 140,
  },
  {
    key: 'defectDescription',
    title: customerComplaintItemPi.label('defectDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: customerComplaintItemPi.ph('defectDescription'),
    width: 180,
  },
  {
    key: 'defectLevel',
    title: customerComplaintItemPi.label('defectLevel'),
    width: 140,
  },
  {
    key: 'defectQuantity',
    title: customerComplaintItemPi.label('defectQuantity'),
    width: 140,
  },
  {
    key: 'defectRate',
    title: customerComplaintItemPi.label('defectRate'),
    width: 140,
  },
  {
    key: 'causeAnalysis',
    title: customerComplaintItemPi.label('causeAnalysis'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('causeAnalysis'),
  },
  {
    key: 'improvementAction',
    title: customerComplaintItemPi.label('improvementAction'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('improvementAction'),
  },
  {
    key: 'improvementResponsible',
    title: customerComplaintItemPi.label('improvementResponsible'),
    width: 140,
  },
  {
    key: 'plannedCompletionDate',
    title: customerComplaintItemPi.label('plannedCompletionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'actualCompletionDate',
    title: customerComplaintItemPi.label('actualCompletionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'attachmentPaths',
    title: customerComplaintItemPi.label('attachmentPaths'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: customerComplaintItemPi.ph('attachmentPaths'),
  },
  {
    key: 'improvementStatus',
    title: customerComplaintItemPi.label('improvementStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: customerComplaintItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerComplaintCreate & { customerComplaintId?: string }> | null | undefined) {
  const rows_customerComplaintItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childCustomerComplaintItemRows.value = rows_customerComplaintItem
}

function createDefaultCustomerComplaintItemRow(): Record<string, unknown> {
  return {
    complaintId: '',
    lineNumber: allocateNextCustomerComplaintItemLineNumber(),
    productCode: '',
    productName: '',
    batchCode: '',
    itemType: 0,
    defectDescription: '',
    defectLevel: '',
    defectQuantity: 0,
    defectRate: 0,
    causeAnalysis: '',
    improvementAction: '',
    improvementResponsible: '',
    plannedCompletionDate: '',
    actualCompletionDate: '',
    attachmentPaths: '',
    improvementStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.customerComplaintId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: customerComplaintItemTableRef.value?.getRows?.() ?? childCustomerComplaintItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        customerComplaintCode: masterId,
      }
      if (isUpdate && isPersistedCustomerComplaintItemRow(row)) {
        normalized.customerComplaintItemId = row.customerComplaintItemId
      } else {
        delete normalized.customerComplaintItemId
      }
      return normalized
    }),
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  complaintMethod: 0,
  complaintType: 0,
  complaintLevel: 0,
  complaintStatus: 0
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
      message: pi.ph('customerComplaintCode'),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: pi.ph('customerId'),
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
  complaintDate: [
    {
      required: true,
      message: pi.ph('complaintDate'),
      trigger: 'change'
    }
  ],
  complaintMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  complaintDescription: [
    {
      required: true,
      message: pi.ph('complaintDescription'),
      trigger: 'blur'
    }
  ],
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  complaintStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('complaintStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('complaintStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  if ('customerSatisfaction' in payload) {
    const rawcustomerSatisfaction = payload.customerSatisfaction
    payload.customerSatisfaction = typeof rawcustomerSatisfaction === 'number' ? rawcustomerSatisfaction : Number(rawcustomerSatisfaction)
  }
  if ('complaintStatus' in payload) {
    const rawcomplaintStatus = payload.complaintStatus
    payload.complaintStatus = typeof rawcomplaintStatus === 'number' ? rawcomplaintStatus : Number(rawcomplaintStatus)
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
