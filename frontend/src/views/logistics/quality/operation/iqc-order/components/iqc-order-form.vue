<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order/components -->
<!-- 文件名称：iqc-order-form.vue -->
<!-- 功能描述：IQC进货检验单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form iqc-order-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="iqc-order-form-tabs"
    >
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
                :label="t('entity.iqcorder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.iqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.sourcecode')"
                name="sourceCode"
              >
                <a-input
                  v-model:value="formState.sourceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.sourcecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorder.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.code')"
                name="iqcOrderCode"
              >
                <a-input
                  v-model:value="formState.iqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.suppliercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.totalpurchasequantity')"
                name="totalPurchaseQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalPurchaseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.totalpurchasequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorder.totalsamplequantity')"
                name="totalSampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalSampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.totalsamplequantity') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.totalqualifiedquantity')"
                name="totalQualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.totalqualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.totalunqualifiedquantity')"
                name="totalUnqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalUnqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.totalunqualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.totalinspectionreturnquantity')"
                name="totalInspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalInspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.totalinspectionreturnquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.judgestatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.judgeby')"
                name="judgeBy"
              >
                <a-input
                  v-model:value="formState.judgeBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorder.judgeby') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.judgedate')"
                name="judgeDate"
              >
                <a-date-picker
                  v-model:value="formState.judgeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorder.judgedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.judgedescription')"
                name="judgeDescription"
              >
                <a-textarea
                  v-model:value="formState.judgeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcorder.judgedescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorder.extfield')"
                name="ExtField"
              >
                <a-textarea
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcorder.extfield') })"
                  :rows="2"
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
      ref="iqcOrderItemTableRef"
      v-model="childIqcOrderItemRows"
      :columns="iqcOrderItemFormColumns"
      :title="t('entity.iqcorderitem._self')"
      :add-button-entity="t('entity.iqcorderitem._self')"
      id-field="iqcOrderItemId"
      :default-row="createDefaultIqcOrderItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * IQC进货检验单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/iqc-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IqcOrderCreate } from '@/types/logistics/quality/operation/iqc-order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","sourceCode","inspectionDate","iqcOrderCode","supplierCode","totalPurchaseQuantity","totalSampleQuantity","totalQualifiedQuantity","totalUnqualifiedQuantity","totalInspectionReturnQuantity","judgeStatus","judgeBy","judgeDate","judgeDescription","ExtField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childIqcOrderItemRows = ref<Record<string, unknown>[]>([])
const iqcOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 iqcOrderItem 可编辑列 */
const iqcOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.iqcorderitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'materialCode',
    title: t('entity.iqcorderitem.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: t('entity.iqcorderitem.materialname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'batchNo',
    title: t('entity.iqcorderitem.batchno'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.iqcorderitem.batchno') }),
  },
  {
    key: 'purchaseQuantity',
    title: t('entity.iqcorderitem.purchasequantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'standardCode',
    title: t('entity.iqcorderitem.standardcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'samplingSchemeCode',
    title: t('entity.iqcorderitem.samplingschemecode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inspectionMethod',
    title: t('entity.iqcorderitem.inspectionmethod'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<IqcOrderCreate & { iqcOrderId?: string }> | null | undefined) {
  childIqcOrderItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultIqcOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childIqcOrderItemRows.value.length + 1) * 10,
    materialCode: '',
    materialName: '',
    batchNo: '',
    purchaseQuantity: 0,
    standardCode: '',
    samplingSchemeCode: '',
    inspectionMethod: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.iqcOrderId ?? ''
  return {
    ...formState,
    items: iqcOrderItemTableRef.value?.getRows?.() ?? childIqcOrderItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      iqcOrderId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IqcOrderCreate & { iqcOrderId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 iqcOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.iqcOrderId) {
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
    const isCreate = !props.formData?.iqcOrderId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorder.plantcode') }),
      trigger: 'blur'
    }
  ],
  sourceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorder.sourcecode') }),
      trigger: 'blur'
    }
  ],
  iqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorder.code') }),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorder.suppliercode') }),
      trigger: 'blur'
    }
  ],
  totalPurchaseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalpurchasequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalpurchasequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalSampleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalsamplequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalsamplequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalQualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalqualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalqualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalUnqualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalunqualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalunqualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalInspectionReturnQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalinspectionreturnquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.totalinspectionreturnquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  judgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.judgestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorder.judgestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await iqcOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalPurchaseQuantity' in payload) {
    const rawtotalPurchaseQuantity = payload.totalPurchaseQuantity
    payload.totalPurchaseQuantity = typeof rawtotalPurchaseQuantity === 'number' ? rawtotalPurchaseQuantity : Number(rawtotalPurchaseQuantity)
  }
  if ('totalSampleQuantity' in payload) {
    const rawtotalSampleQuantity = payload.totalSampleQuantity
    payload.totalSampleQuantity = typeof rawtotalSampleQuantity === 'number' ? rawtotalSampleQuantity : Number(rawtotalSampleQuantity)
  }
  if ('totalQualifiedQuantity' in payload) {
    const rawtotalQualifiedQuantity = payload.totalQualifiedQuantity
    payload.totalQualifiedQuantity = typeof rawtotalQualifiedQuantity === 'number' ? rawtotalQualifiedQuantity : Number(rawtotalQualifiedQuantity)
  }
  if ('totalUnqualifiedQuantity' in payload) {
    const rawtotalUnqualifiedQuantity = payload.totalUnqualifiedQuantity
    payload.totalUnqualifiedQuantity = typeof rawtotalUnqualifiedQuantity === 'number' ? rawtotalUnqualifiedQuantity : Number(rawtotalUnqualifiedQuantity)
  }
  if ('totalInspectionReturnQuantity' in payload) {
    const rawtotalInspectionReturnQuantity = payload.totalInspectionReturnQuantity
    payload.totalInspectionReturnQuantity = typeof rawtotalInspectionReturnQuantity === 'number' ? rawtotalInspectionReturnQuantity : Number(rawtotalInspectionReturnQuantity)
  }
  if ('judgeStatus' in payload) {
    const rawjudgeStatus = payload.judgeStatus
    payload.judgeStatus = typeof rawjudgeStatus === 'number' ? rawjudgeStatus : Number(rawjudgeStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.iqcOrderId)
  childIqcOrderItemRows.value = []
  iqcOrderItemTableRef.value?.resetRows?.()
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
