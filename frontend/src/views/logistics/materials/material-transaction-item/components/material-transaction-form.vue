<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-transaction-item/components -->
<!-- 文件名称：material-transaction-form.vue -->
<!-- 功能描述：Takt物料交易主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-transaction-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-transaction-form-tabs"
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
                :label="t('entity.materialtransaction.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.code')"
                name="materialTransactionCode"
              >
                <a-input
                  v-model:value="formState.materialTransactionCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.transactiondate')"
                name="transactionDate"
              >
                <a-date-picker
                  v-model:value="formState.transactionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.transactiondirection')"
                name="transactionDirection"
              >
                <a-input-number
                  v-model:value="formState.transactionDirection"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.transactiondirection') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.transactiontype')"
                name="transactionType"
              >
                <TaktSelect
                  v-model:value="formState.transactionType"
                  dict-type="logistics_inbound_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiontype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.businessaction')"
                name="businessAction"
              >
                <a-input-number
                  v-model:value="formState.businessAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.businessaction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.sourcecode')"
                name="sourceCode"
              >
                <a-input
                  v-model:value="formState.sourceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.sourcecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
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
                :label="t('entity.materialtransaction.partnercode')"
                name="partnerCode"
              >
                <a-input
                  v-model:value="formState.partnerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.partnercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.partnername')"
                name="partnerName"
              >
                <a-input
                  v-model:value="formState.partnerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.partnername') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.warehousecode')"
                name="warehouseCode"
              >
                <a-input
                  v-model:value="formState.warehouseCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.warehousecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.locationcode')"
                name="locationCode"
              >
                <a-input
                  v-model:value="formState.locationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.locationcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.targetwarehousecode')"
                name="targetWarehouseCode"
              >
                <a-input
                  v-model:value="formState.targetWarehouseCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.targetwarehousecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.targetlocationcode')"
                name="targetLocationCode"
              >
                <a-input
                  v-model:value="formState.targetLocationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.targetlocationcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialTransactionId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.relatedcompany')"
                name="relatedCompany"
              >
                <a-input
                  v-model:value="formState.relatedCompany"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.relatedcompany') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.totalquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.transactionstatus')"
                name="transactionStatus"
              >
                <a-input-number
                  v-model:value="formState.transactionStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.transactionstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialtransaction.posteddate')"
                name="postedDate"
              >
                <a-date-picker
                  v-model:value="formState.postedDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.posteddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                :label="t('entity.materialtransaction.postedby')"
                name="postedBy"
              >
                <a-input
                  v-model:value="formState.postedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.postedby') })"
                  show-count
                  :maxlength="50"
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
      ref="materialTransactionItemTableRef"
      v-model="childMaterialTransactionItemRows"
      :columns="materialTransactionItemFormColumns"
      :title="t('entity.materialtransactionitem._self')"
      :add-button-entity="t('entity.materialtransactionitem._self')"
      id-field="materialTransactionItemId"
      :default-row="createDefaultMaterialTransactionItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料交易主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-transaction-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaterialTransactionCreate } from '@/types/logistics/materials/material-transaction'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialTransactionCode","transactionDate","transactionDirection","transactionType","businessAction","sourceCode","partnerCode","partnerName","warehouseCode","locationCode","targetWarehouseCode","targetLocationCode","relatedCompany","totalQuantity","transactionStatus","postedDate","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childMaterialTransactionItemRows = ref<Record<string, unknown>[]>([])
const materialTransactionItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 materialTransactionItem 可编辑列 */
const materialTransactionItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.materialtransactionitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'sourceCode',
    title: t('entity.materialtransactionitem.sourcecode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialtransactionitem.sourcecode') }),
  },
  {
    key: 'sourceLineNumber',
    title: t('entity.materialtransactionitem.sourcelinenumber'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'materialCode',
    title: t('entity.materialtransactionitem.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialName',
    title: t('entity.materialtransactionitem.materialname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialSpecification',
    title: t('entity.materialtransactionitem.materialspecification'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialtransactionitem.materialspecification') }),
  },
  {
    key: 'transactionUnit',
    title: t('entity.materialtransactionitem.transactionunit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'transactionQuantity',
    title: t('entity.materialtransactionitem.transactionquantity'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialTransactionCreate & { materialTransactionId?: string }> | null | undefined) {
  childMaterialTransactionItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultMaterialTransactionItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childMaterialTransactionItemRows.value.length + 1) * 10,
    sourceCode: '',
    sourceLineNumber: 0,
    materialCode: '',
    materialName: '',
    materialSpecification: '',
    transactionUnit: '',
    transactionQuantity: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialTransactionId ?? ''
  return {
    ...formState,
    items: materialTransactionItemTableRef.value?.getRows?.() ?? childMaterialTransactionItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      materialTransactionId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialTransactionCreate & { materialTransactionId?: string }> | null
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
  transactionType: 4
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialTransactionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialTransactionId) {
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
    const isCreate = !props.formData?.materialTransactionId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.plantcode') }),
      trigger: 'blur'
    }
  ],
  materialTransactionCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.code') }),
      trigger: 'blur'
    }
  ],
  transactionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiondate') }),
      trigger: 'change'
    }
  ],
  transactionDirection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiondirection') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiondirection') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  transactionType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiontype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactiontype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  businessAction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.businessaction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.businessaction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warehouseCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.warehousecode') }),
      trigger: 'blur'
    }
  ],
  locationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.locationcode') }),
      trigger: 'blur'
    }
  ],
  relatedCompany: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialtransaction.relatedcompany') }),
      trigger: 'blur'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.totalquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.totalquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  transactionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactionstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialtransaction.transactionstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialTransactionItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('transactionDirection' in payload) {
    const rawtransactionDirection = payload.transactionDirection
    payload.transactionDirection = typeof rawtransactionDirection === 'number' ? rawtransactionDirection : Number(rawtransactionDirection)
  }
  if ('transactionType' in payload) {
    const rawtransactionType = payload.transactionType
    payload.transactionType = typeof rawtransactionType === 'number' ? rawtransactionType : Number(rawtransactionType)
  }
  if ('businessAction' in payload) {
    const rawbusinessAction = payload.businessAction
    payload.businessAction = typeof rawbusinessAction === 'number' ? rawbusinessAction : Number(rawbusinessAction)
  }
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    payload.totalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
  }
  if ('transactionStatus' in payload) {
    const rawtransactionStatus = payload.transactionStatus
    payload.transactionStatus = typeof rawtransactionStatus === 'number' ? rawtransactionStatus : Number(rawtransactionStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialTransactionId)
  childMaterialTransactionItemRows.value = []
  materialTransactionItemTableRef.value?.resetRows?.()
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
